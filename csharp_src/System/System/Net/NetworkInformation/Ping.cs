using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.NetworkInformation;

[System.MonoTODO("IPv6 support is missing")]
public class Ping : Component, IDisposable
{
	private struct cap_user_header_t
	{
		public uint version;

		public int pid;
	}

	private struct cap_user_data_t
	{
		public uint effective;

		public uint permitted;

		public uint inheritable;
	}

	private class IcmpMessage
	{
		private byte[] bytes;

		public byte Type => bytes[0];

		public byte Code => bytes[1];

		public ushort Identifier => (ushort)(bytes[4] + (bytes[5] << 8));

		public ushort Sequence => (ushort)(bytes[6] + (bytes[7] << 8));

		public byte[] Data
		{
			get
			{
				byte[] array = new byte[bytes.Length - 8];
				Buffer.BlockCopy(bytes, 8, array, 0, array.Length);
				return array;
			}
		}

		public IPStatus IPStatus
		{
			get
			{
				switch (Type)
				{
				case 0:
					return IPStatus.Success;
				case 3:
					switch (Code)
					{
					case 0:
						return IPStatus.DestinationNetworkUnreachable;
					case 1:
						return IPStatus.DestinationHostUnreachable;
					case 2:
						return IPStatus.DestinationProtocolUnreachable;
					case 3:
						return IPStatus.DestinationPortUnreachable;
					case 4:
						return IPStatus.BadOption;
					case 5:
						return IPStatus.BadRoute;
					}
					break;
				case 11:
					switch (Code)
					{
					case 0:
						return IPStatus.TimeExceeded;
					case 1:
						return IPStatus.TtlReassemblyTimeExceeded;
					}
					break;
				case 12:
					return IPStatus.ParameterProblem;
				case 4:
					return IPStatus.SourceQuench;
				case 8:
					return IPStatus.Success;
				}
				return IPStatus.Unknown;
			}
		}

		public IcmpMessage(byte[] bytes, int offset, int size)
		{
			this.bytes = new byte[size];
			Buffer.BlockCopy(bytes, offset, this.bytes, 0, size);
		}

		public IcmpMessage(byte type, byte code, ushort identifier, ushort sequence, byte[] data)
		{
			bytes = new byte[data.Length + 8];
			bytes[0] = type;
			bytes[1] = code;
			bytes[4] = (byte)(identifier & 0xFF);
			bytes[5] = (byte)(identifier >> 8);
			bytes[6] = (byte)(sequence & 0xFF);
			bytes[7] = (byte)(sequence >> 8);
			Buffer.BlockCopy(data, 0, bytes, 8, data.Length);
			ushort num = ComputeChecksum(bytes);
			bytes[2] = (byte)(num & 0xFF);
			bytes[3] = (byte)(num >> 8);
		}

		public byte[] GetBytes()
		{
			return bytes;
		}

		private static ushort ComputeChecksum(byte[] data)
		{
			uint num = 0u;
			for (int i = 0; i < data.Length; i += 2)
			{
				ushort num2 = (ushort)((i + 1 < data.Length) ? data[i + 1] : 0);
				num2 <<= 8;
				num2 += data[i];
				num += num2;
			}
			num = (num >> 16) + (num & 0xFFFF);
			return (ushort)(~num);
		}
	}

	private const int DefaultCount = 1;

	private static readonly string[] PingBinPaths;

	private static readonly string PingBinPath;

	private static bool canSendPrivileged;

	private const int default_timeout = 4000;

	private ushort identifier;

	private const uint _LINUX_CAPABILITY_VERSION_1 = 429392688u;

	private static readonly byte[] default_buffer;

	private BackgroundWorker worker;

	private object user_async_state;

	private CancellationTokenSource cts;

	public event PingCompletedEventHandler PingCompleted;

	static Ping()
	{
		PingBinPaths = new string[4] { "/bin/ping", "/sbin/ping", "/usr/sbin/ping", "/system/bin/ping" };
		default_buffer = new byte[0];
		if (Environment.OSVersion.Platform == PlatformID.Unix)
		{
			CheckLinuxCapabilities();
			if (!canSendPrivileged && WindowsIdentity.GetCurrent().Name == "root")
			{
				canSendPrivileged = true;
			}
			string[] pingBinPaths = PingBinPaths;
			foreach (string text in pingBinPaths)
			{
				if (File.Exists(text))
				{
					PingBinPath = text;
					break;
				}
			}
		}
		else
		{
			canSendPrivileged = true;
		}
		if (PingBinPath == null)
		{
			PingBinPath = "/bin/ping";
		}
	}

	public Ping()
	{
		RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
		byte[] array = new byte[2];
		rNGCryptoServiceProvider.GetBytes(array);
		identifier = (ushort)(array[0] + (array[1] << 8));
	}

	[DllImport("libc")]
	private static extern int capget(ref cap_user_header_t header, ref cap_user_data_t data);

	private static void CheckLinuxCapabilities()
	{
		try
		{
			cap_user_header_t header = default(cap_user_header_t);
			cap_user_data_t data = default(cap_user_data_t);
			header.version = 429392688u;
			int num = -1;
			try
			{
				num = capget(ref header, ref data);
			}
			catch (Exception)
			{
			}
			if (num != -1)
			{
				canSendPrivileged = (data.effective & 0x2000) != 0;
			}
		}
		catch
		{
			canSendPrivileged = false;
		}
	}

	void IDisposable.Dispose()
	{
	}

	protected void OnPingCompleted(PingCompletedEventArgs e)
	{
		user_async_state = null;
		worker = null;
		if (cts != null)
		{
			cts.Dispose();
			cts = null;
		}
		if (this.PingCompleted != null)
		{
			this.PingCompleted(this, e);
		}
	}

	public PingReply Send(IPAddress address)
	{
		return Send(address, 4000);
	}

	public PingReply Send(IPAddress address, int timeout)
	{
		return Send(address, timeout, default_buffer);
	}

	public PingReply Send(IPAddress address, int timeout, byte[] buffer)
	{
		return Send(address, timeout, buffer, new PingOptions());
	}

	public PingReply Send(string hostNameOrAddress)
	{
		return Send(hostNameOrAddress, 4000);
	}

	public PingReply Send(string hostNameOrAddress, int timeout)
	{
		return Send(hostNameOrAddress, timeout, default_buffer);
	}

	public PingReply Send(string hostNameOrAddress, int timeout, byte[] buffer)
	{
		return Send(hostNameOrAddress, timeout, buffer, new PingOptions());
	}

	public PingReply Send(string hostNameOrAddress, int timeout, byte[] buffer, PingOptions options)
	{
		IPAddress[] hostAddresses = Dns.GetHostAddresses(hostNameOrAddress);
		return Send(hostAddresses[0], timeout, buffer, options);
	}

	public PingReply Send(IPAddress address, int timeout, byte[] buffer, PingOptions options)
	{
		if (address == null)
		{
			throw new ArgumentNullException("address");
		}
		if (timeout < 0)
		{
			throw new ArgumentOutOfRangeException("timeout", "timeout must be non-negative integer");
		}
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (buffer.Length > 65500)
		{
			throw new ArgumentException("buffer");
		}
		if (canSendPrivileged)
		{
			return SendPrivileged(address, timeout, buffer, options);
		}
		return SendUnprivileged(address, timeout, buffer, options);
	}

	private PingReply SendPrivileged(IPAddress address, int timeout, byte[] buffer, PingOptions options)
	{
		IPEndPoint iPEndPoint = new IPEndPoint(address, 0);
		using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
		if (options != null)
		{
			socket.DontFragment = options.DontFragment;
			socket.Ttl = (short)options.Ttl;
		}
		socket.SendTimeout = timeout;
		socket.ReceiveTimeout = timeout;
		byte[] bytes = new IcmpMessage(8, 0, identifier, 0, buffer).GetBytes();
		socket.SendBufferSize = bytes.Length;
		socket.SendTo(bytes, bytes.Length, SocketFlags.None, iPEndPoint);
		Stopwatch stopwatch = Stopwatch.StartNew();
		bytes = new byte[bytes.Length + 40];
		while (true)
		{
			EndPoint remoteEP = iPEndPoint;
			SocketError errorCode = SocketError.Success;
			int num = socket.ReceiveFrom(bytes, 0, bytes.Length, SocketFlags.None, ref remoteEP, out errorCode);
			switch (errorCode)
			{
			case SocketError.TimedOut:
				return new PingReply(null, new byte[0], options, 0L, IPStatus.TimedOut);
			default:
				throw new NotSupportedException($"Unexpected socket error during ping request: {errorCode}");
			case SocketError.Success:
			{
				long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
				int num2 = (bytes[0] & 0xF) << 2;
				int size = num - num2;
				if (!((IPEndPoint)remoteEP).Address.Equals(iPEndPoint.Address))
				{
					long num3 = timeout - elapsedMilliseconds;
					if (num3 <= 0)
					{
						return new PingReply(null, new byte[0], options, 0L, IPStatus.TimedOut);
					}
					socket.ReceiveTimeout = (int)num3;
					break;
				}
				IcmpMessage icmpMessage = new IcmpMessage(bytes, num2, size);
				if (icmpMessage.Identifier != identifier || icmpMessage.Type == 8)
				{
					long num4 = timeout - elapsedMilliseconds;
					if (num4 <= 0)
					{
						return new PingReply(null, new byte[0], options, 0L, IPStatus.TimedOut);
					}
					socket.ReceiveTimeout = (int)num4;
					break;
				}
				return new PingReply(address, icmpMessage.Data, options, elapsedMilliseconds, icmpMessage.IPStatus);
			}
			}
		}
	}

	private PingReply SendUnprivileged(IPAddress address, int timeout, byte[] buffer, PingOptions options)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		Process process = new Process();
		string arguments = BuildPingArgs(address, timeout, options);
		long roundtripTime = 0L;
		process.StartInfo.FileName = PingBinPath;
		process.StartInfo.Arguments = arguments;
		process.StartInfo.CreateNoWindow = true;
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.RedirectStandardOutput = true;
		process.StartInfo.RedirectStandardError = true;
		IPStatus status = IPStatus.Unknown;
		try
		{
			process.Start();
			process.StandardOutput.ReadToEnd();
			process.StandardError.ReadToEnd();
			roundtripTime = stopwatch.ElapsedMilliseconds;
			if (!process.WaitForExit(timeout) || (process.HasExited && process.ExitCode == 2))
			{
				status = IPStatus.TimedOut;
			}
			else if (process.ExitCode == 0)
			{
				status = IPStatus.Success;
			}
			else if (process.ExitCode == 1)
			{
				status = IPStatus.TtlExpired;
			}
		}
		catch
		{
		}
		finally
		{
			if (!process.HasExited)
			{
				process.Kill();
			}
			process.Dispose();
		}
		return new PingReply(address, buffer, options, roundtripTime, status);
	}

	public void SendAsync(IPAddress address, int timeout, byte[] buffer, object userToken)
	{
		SendAsync(address, 4000, default_buffer, new PingOptions(), userToken);
	}

	public void SendAsync(IPAddress address, int timeout, object userToken)
	{
		SendAsync(address, 4000, default_buffer, userToken);
	}

	public void SendAsync(IPAddress address, object userToken)
	{
		SendAsync(address, 4000, userToken);
	}

	public void SendAsync(string hostNameOrAddress, int timeout, byte[] buffer, object userToken)
	{
		SendAsync(hostNameOrAddress, timeout, buffer, new PingOptions(), userToken);
	}

	public void SendAsync(string hostNameOrAddress, int timeout, byte[] buffer, PingOptions options, object userToken)
	{
		IPAddress address = Dns.GetHostEntry(hostNameOrAddress).AddressList[0];
		SendAsync(address, timeout, buffer, options, userToken);
	}

	public void SendAsync(string hostNameOrAddress, int timeout, object userToken)
	{
		SendAsync(hostNameOrAddress, timeout, default_buffer, userToken);
	}

	public void SendAsync(string hostNameOrAddress, object userToken)
	{
		SendAsync(hostNameOrAddress, 4000, userToken);
	}

	public void SendAsync(IPAddress address, int timeout, byte[] buffer, PingOptions options, object userToken)
	{
		if (worker != null || cts != null)
		{
			throw new InvalidOperationException("Another SendAsync operation is in progress");
		}
		worker = new BackgroundWorker();
		worker.DoWork += delegate(object o, DoWorkEventArgs ea)
		{
			try
			{
				user_async_state = ea.Argument;
				ea.Result = Send(address, timeout, buffer, options);
			}
			catch (Exception result)
			{
				ea.Result = result;
			}
		};
		worker.WorkerSupportsCancellation = true;
		worker.RunWorkerCompleted += delegate(object o, RunWorkerCompletedEventArgs ea)
		{
			OnPingCompleted(new PingCompletedEventArgs(ea.Error, ea.Cancelled, user_async_state, ea.Result as PingReply));
		};
		worker.RunWorkerAsync(userToken);
	}

	public void SendAsyncCancel()
	{
		if (cts != null)
		{
			cts.Cancel();
			return;
		}
		if (worker == null)
		{
			throw new InvalidOperationException("SendAsync operation is not in progress");
		}
		worker.CancelAsync();
	}

	private string BuildPingArgs(IPAddress address, int timeout, PingOptions options)
	{
		CultureInfo invariantCulture = CultureInfo.InvariantCulture;
		StringBuilder stringBuilder = new StringBuilder();
		uint num = Convert.ToUInt32(Math.Floor((double)(timeout + 1000) / 1000.0));
		bool isMacOS = Platform.IsMacOS;
		if (!isMacOS)
		{
			stringBuilder.AppendFormat(invariantCulture, "-q -n -c {0} -w {1} -t {2} -M ", 1, num, options.Ttl);
		}
		else
		{
			stringBuilder.AppendFormat(invariantCulture, "-q -n -c {0} -t {1} -o -m {2} ", 1, num, options.Ttl);
		}
		if (!isMacOS)
		{
			stringBuilder.Append(options.DontFragment ? "do " : "dont ");
		}
		else if (options.DontFragment)
		{
			stringBuilder.Append("-D ");
		}
		stringBuilder.Append(address.ToString());
		return stringBuilder.ToString();
	}

	public Task<PingReply> SendPingAsync(IPAddress address, int timeout, byte[] buffer)
	{
		return SendPingAsync(address, 4000, default_buffer, new PingOptions());
	}

	public Task<PingReply> SendPingAsync(IPAddress address, int timeout)
	{
		return SendPingAsync(address, 4000, default_buffer);
	}

	public Task<PingReply> SendPingAsync(IPAddress address)
	{
		return SendPingAsync(address, 4000);
	}

	public Task<PingReply> SendPingAsync(string hostNameOrAddress, int timeout, byte[] buffer)
	{
		return SendPingAsync(hostNameOrAddress, timeout, buffer, new PingOptions());
	}

	public Task<PingReply> SendPingAsync(string hostNameOrAddress, int timeout, byte[] buffer, PingOptions options)
	{
		IPAddress address = Dns.GetHostEntry(hostNameOrAddress).AddressList[0];
		return SendPingAsync(address, timeout, buffer, options);
	}

	public Task<PingReply> SendPingAsync(string hostNameOrAddress, int timeout)
	{
		return SendPingAsync(hostNameOrAddress, timeout, default_buffer);
	}

	public Task<PingReply> SendPingAsync(string hostNameOrAddress)
	{
		return SendPingAsync(hostNameOrAddress, 4000);
	}

	public Task<PingReply> SendPingAsync(IPAddress address, int timeout, byte[] buffer, PingOptions options)
	{
		if (worker != null || cts != null)
		{
			throw new InvalidOperationException("Another SendAsync operation is in progress");
		}
		cts = new CancellationTokenSource();
		Task<PingReply> task = Task<PingReply>.Factory.StartNew(() => Send(address, timeout, buffer, options), cts.Token);
		task.ContinueWith(delegate(Task<PingReply> t)
		{
			if (t.IsCanceled)
			{
				OnPingCompleted(new PingCompletedEventArgs(null, cancelled: true, null, null));
			}
			else if (t.IsFaulted)
			{
				OnPingCompleted(new PingCompletedEventArgs(t.Exception, cancelled: false, null, null));
			}
			else
			{
				OnPingCompleted(new PingCompletedEventArgs(null, cancelled: false, null, t.Result));
			}
		});
		return task;
	}
}
