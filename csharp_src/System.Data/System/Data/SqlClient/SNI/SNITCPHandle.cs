using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.SqlClient.SNI;

internal class SNITCPHandle : SNIHandle
{
	private readonly string _targetServer;

	private readonly object _callbackObject;

	private readonly Socket _socket;

	private NetworkStream _tcpStream;

	private Stream _stream;

	private SslStream _sslStream;

	private SslOverTdsStream _sslOverTdsStream;

	private SNIAsyncCallback _receiveCallback;

	private SNIAsyncCallback _sendCallback;

	private bool _validateCert = true;

	private int _bufferSize = 4096;

	private uint _status = uint.MaxValue;

	private Guid _connectionId = Guid.NewGuid();

	private const int MaxParallelIpAddresses = 64;

	public override Guid ConnectionId => _connectionId;

	public override uint Status => _status;

	public override void Dispose()
	{
		lock (this)
		{
			if (_sslOverTdsStream != null)
			{
				_sslOverTdsStream.Dispose();
				_sslOverTdsStream = null;
			}
			if (_sslStream != null)
			{
				_sslStream.Dispose();
				_sslStream = null;
			}
			if (_tcpStream != null)
			{
				_tcpStream.Dispose();
				_tcpStream = null;
			}
			_stream = null;
		}
	}

	public SNITCPHandle(string serverName, int port, long timerExpire, object callbackObject, bool parallel)
	{
		_callbackObject = callbackObject;
		_targetServer = serverName;
		try
		{
			TimeSpan timeSpan = default(TimeSpan);
			bool flag = long.MaxValue == timerExpire;
			if (!flag)
			{
				timeSpan = DateTime.FromFileTime(timerExpire) - DateTime.Now;
				timeSpan = ((timeSpan.Ticks < 0) ? TimeSpan.FromTicks(0L) : timeSpan);
			}
			if (parallel)
			{
				Task<IPAddress[]> hostAddressesAsync = Dns.GetHostAddressesAsync(serverName);
				hostAddressesAsync.Wait(timeSpan);
				IPAddress[] result = hostAddressesAsync.Result;
				if (result.Length > 64)
				{
					ReportTcpSNIError(0u, 47u, string.Empty);
					return;
				}
				Task<Socket> task = ParallelConnectAsync(result, port);
				if (!(flag ? task.Wait(-1) : task.Wait(timeSpan)))
				{
					ReportTcpSNIError(0u, 40u, string.Empty);
					return;
				}
				_socket = task.Result;
			}
			else
			{
				_socket = Connect(serverName, port, flag ? TimeSpan.FromMilliseconds(2147483647.0) : timeSpan);
			}
			if (_socket == null || !_socket.Connected)
			{
				if (_socket != null)
				{
					_socket.Dispose();
					_socket = null;
				}
				ReportTcpSNIError(0u, 40u, string.Empty);
				return;
			}
			_socket.NoDelay = true;
			_tcpStream = new NetworkStream(_socket, ownsSocket: true);
			_sslOverTdsStream = new SslOverTdsStream(_tcpStream);
			_sslStream = new SslStream((Stream)_sslOverTdsStream, leaveInnerStreamOpen: true, (RemoteCertificateValidationCallback)ValidateServerCertificate, (LocalCertificateSelectionCallback)null);
		}
		catch (SocketException sniException)
		{
			ReportTcpSNIError(sniException);
			return;
		}
		catch (Exception sniException2)
		{
			ReportTcpSNIError(sniException2);
			return;
		}
		_stream = _tcpStream;
		_status = 0u;
	}

	private static Socket Connect(string serverName, int port, TimeSpan timeout)
	{
		IPAddress[] hostAddresses = Dns.GetHostAddresses(serverName);
		IPAddress iPAddress = null;
		IPAddress iPAddress2 = null;
		IPAddress[] array = hostAddresses;
		foreach (IPAddress iPAddress3 in array)
		{
			if (iPAddress3.AddressFamily == AddressFamily.InterNetwork)
			{
				iPAddress = iPAddress3;
			}
			else if (iPAddress3.AddressFamily == AddressFamily.InterNetworkV6)
			{
				iPAddress2 = iPAddress3;
			}
		}
		hostAddresses = new IPAddress[2] { iPAddress, iPAddress2 };
		Socket[] sockets = new Socket[2];
		CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		cancellationTokenSource.CancelAfter(timeout);
		cancellationTokenSource.Token.Register(Cancel);
		Socket result = null;
		for (int j = 0; j < sockets.Length; j++)
		{
			try
			{
				if (hostAddresses[j] == null)
				{
					continue;
				}
				sockets[j] = new Socket(hostAddresses[j].AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				sockets[j].Connect(hostAddresses[j], port);
				if (sockets[j] != null)
				{
					if (sockets[j].Connected)
					{
						result = sockets[j];
						break;
					}
					sockets[j].Dispose();
					sockets[j] = null;
				}
			}
			catch
			{
			}
		}
		return result;
		void Cancel()
		{
			for (int k = 0; k < sockets.Length; k++)
			{
				try
				{
					if (sockets[k] != null && !sockets[k].Connected)
					{
						sockets[k].Dispose();
						sockets[k] = null;
					}
				}
				catch
				{
				}
			}
		}
	}

	private static Task<Socket> ParallelConnectAsync(IPAddress[] serverAddresses, int port)
	{
		if (serverAddresses == null)
		{
			throw new ArgumentNullException("serverAddresses");
		}
		if (serverAddresses.Length == 0)
		{
			throw new ArgumentOutOfRangeException("serverAddresses");
		}
		List<Socket> list = new List<Socket>(serverAddresses.Length);
		List<Task> list2 = new List<Task>(serverAddresses.Length);
		TaskCompletionSource<Socket> taskCompletionSource = new TaskCompletionSource<Socket>();
		StrongBox<Exception> lastError = new StrongBox<Exception>();
		StrongBox<int> pendingCompleteCount = new StrongBox<int>(serverAddresses.Length);
		foreach (IPAddress iPAddress in serverAddresses)
		{
			Socket socket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			list.Add(socket);
			try
			{
				list2.Add(socket.ConnectAsync(iPAddress, port));
			}
			catch (Exception exception)
			{
				list2.Add(Task.FromException(exception));
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			ParallelConnectHelper(list[j], list2[j], taskCompletionSource, pendingCompleteCount, lastError, list);
		}
		return taskCompletionSource.Task;
	}

	private static async void ParallelConnectHelper(Socket socket, Task connectTask, TaskCompletionSource<Socket> tcs, StrongBox<int> pendingCompleteCount, StrongBox<Exception> lastError, List<Socket> sockets)
	{
		bool success = false;
		try
		{
			await connectTask.ConfigureAwait(continueOnCapturedContext: false);
			success = tcs.TrySetResult(socket);
			if (!success)
			{
				return;
			}
			foreach (Socket socket2 in sockets)
			{
				if (socket2 != socket)
				{
					socket2.Dispose();
				}
			}
		}
		catch (Exception value)
		{
			Interlocked.Exchange(ref lastError.Value, value);
		}
		finally
		{
			if (!success && Interlocked.Decrement(ref pendingCompleteCount.Value) == 0)
			{
				if (lastError.Value != null)
				{
					tcs.TrySetException(lastError.Value);
				}
				else
				{
					tcs.TrySetCanceled();
				}
				foreach (Socket socket3 in sockets)
				{
					socket3.Dispose();
				}
			}
		}
	}

	public override uint EnableSsl(uint options)
	{
		_validateCert = (options & 1) != 0;
		try
		{
			_sslStream.AuthenticateAsClient(_targetServer);
			_sslOverTdsStream.FinishHandshake();
		}
		catch (AuthenticationException sniException)
		{
			return ReportTcpSNIError(sniException);
		}
		catch (InvalidOperationException sniException2)
		{
			return ReportTcpSNIError(sniException2);
		}
		_stream = _sslStream;
		return 0u;
	}

	public override void DisableSsl()
	{
		_sslStream.Dispose();
		_sslStream = null;
		_sslOverTdsStream.Dispose();
		_sslOverTdsStream = null;
		_stream = _tcpStream;
	}

	private bool ValidateServerCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors policyErrors)
	{
		if (!_validateCert)
		{
			return true;
		}
		return SNICommon.ValidateSslServerCertificate(_targetServer, sender, cert, chain, policyErrors);
	}

	public override void SetBufferSize(int bufferSize)
	{
		_bufferSize = bufferSize;
	}

	public override uint Send(SNIPacket packet)
	{
		lock (this)
		{
			try
			{
				packet.WriteToStream(_stream);
				return 0u;
			}
			catch (ObjectDisposedException sniException)
			{
				return ReportTcpSNIError(sniException);
			}
			catch (SocketException sniException2)
			{
				return ReportTcpSNIError(sniException2);
			}
			catch (IOException sniException3)
			{
				return ReportTcpSNIError(sniException3);
			}
		}
	}

	public override uint Receive(out SNIPacket packet, int timeoutInMilliseconds)
	{
		lock (this)
		{
			packet = null;
			try
			{
				if (timeoutInMilliseconds > 0)
				{
					_socket.ReceiveTimeout = timeoutInMilliseconds;
				}
				else
				{
					if (timeoutInMilliseconds != -1)
					{
						ReportTcpSNIError(0u, 11u, string.Empty);
						return 258u;
					}
					_socket.ReceiveTimeout = 0;
				}
				packet = new SNIPacket(_bufferSize);
				packet.ReadFromStream(_stream);
				if (packet.Length == 0)
				{
					Win32Exception ex = new Win32Exception();
					return ReportErrorAndReleasePacket(packet, (uint)ex.NativeErrorCode, 0u, ex.Message);
				}
				return 0u;
			}
			catch (ObjectDisposedException sniException)
			{
				return ReportErrorAndReleasePacket(packet, sniException);
			}
			catch (SocketException sniException2)
			{
				return ReportErrorAndReleasePacket(packet, sniException2);
			}
			catch (IOException ex2)
			{
				uint result = ReportErrorAndReleasePacket(packet, ex2);
				if (ex2.InnerException is SocketException && ((SocketException)ex2.InnerException).SocketErrorCode == SocketError.TimedOut)
				{
					result = 258u;
				}
				return result;
			}
			finally
			{
				_socket.ReceiveTimeout = 0;
			}
		}
	}

	public override void SetAsyncCallbacks(SNIAsyncCallback receiveCallback, SNIAsyncCallback sendCallback)
	{
		_receiveCallback = receiveCallback;
		_sendCallback = sendCallback;
	}

	public override uint SendAsync(SNIPacket packet, bool disposePacketAfterSendAsync, SNIAsyncCallback callback = null)
	{
		SNIAsyncCallback callback2 = callback ?? _sendCallback;
		lock (this)
		{
			packet.WriteToStreamAsync(_stream, callback2, SNIProviders.TCP_PROV, disposePacketAfterSendAsync);
		}
		return 997u;
	}

	public override uint ReceiveAsync(ref SNIPacket packet)
	{
		packet = new SNIPacket(_bufferSize);
		try
		{
			packet.ReadFromStreamAsync(_stream, _receiveCallback);
			return 997u;
		}
		catch (Exception ex) when (ex is ObjectDisposedException || ex is SocketException || ex is IOException)
		{
			return ReportErrorAndReleasePacket(packet, ex);
		}
	}

	public override uint CheckConnection()
	{
		try
		{
			if (!_socket.Connected || _socket.Poll(0, SelectMode.SelectError))
			{
				return 1u;
			}
		}
		catch (SocketException sniException)
		{
			return ReportTcpSNIError(sniException);
		}
		catch (ObjectDisposedException sniException2)
		{
			return ReportTcpSNIError(sniException2);
		}
		return 0u;
	}

	private uint ReportTcpSNIError(Exception sniException)
	{
		_status = 1u;
		return SNICommon.ReportSNIError(SNIProviders.TCP_PROV, 35u, sniException);
	}

	private uint ReportTcpSNIError(uint nativeError, uint sniError, string errorMessage)
	{
		_status = 1u;
		return SNICommon.ReportSNIError(SNIProviders.TCP_PROV, nativeError, sniError, errorMessage);
	}

	private uint ReportErrorAndReleasePacket(SNIPacket packet, Exception sniException)
	{
		packet?.Release();
		return ReportTcpSNIError(sniException);
	}

	private uint ReportErrorAndReleasePacket(SNIPacket packet, uint nativeError, uint sniError, string errorMessage)
	{
		packet?.Release();
		return ReportTcpSNIError(nativeError, sniError, errorMessage);
	}
}
