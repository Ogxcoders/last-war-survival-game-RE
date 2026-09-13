using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Pipes;

public sealed class NamedPipeServerStream : PipeStream
{
	private sealed class SharedServer
	{
		private static readonly Dictionary<string, SharedServer> s_servers = new Dictionary<string, SharedServer>();

		private readonly int _maxCount;

		private int _currentCount;

		internal string PipeName { get; }

		internal Socket ListeningSocket { get; }

		internal static SharedServer Get(string path, int maxCount)
		{
			lock (s_servers)
			{
				if (s_servers.TryGetValue(path, out var value))
				{
					if (value._currentCount == value._maxCount)
					{
						throw new IOException("All pipe instances are busy.");
					}
					if (value._currentCount == maxCount)
					{
						throw new UnauthorizedAccessException(SR.Format("Access to the path '{0}' is denied.", path));
					}
				}
				else
				{
					value = new SharedServer(path, maxCount);
					s_servers.Add(path, value);
				}
				value._currentCount++;
				return value;
			}
		}

		internal void Dispose(bool disposing)
		{
			lock (s_servers)
			{
				if (_currentCount == 1)
				{
					s_servers.Remove(PipeName);
					global::Interop.Sys.Unlink(PipeName);
					if (disposing)
					{
						ListeningSocket.Dispose();
					}
				}
				else
				{
					_currentCount--;
				}
			}
		}

		private SharedServer(string path, int maxCount)
		{
			global::Interop.Sys.Unlink(path);
			Socket socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);
			try
			{
				socket.Bind(new UnixDomainSocketEndPoint(path));
				socket.Listen(int.MaxValue);
			}
			catch
			{
				socket.Dispose();
				throw;
			}
			PipeName = path;
			ListeningSocket = socket;
			_maxCount = maxCount;
		}
	}

	private SharedServer _instance;

	private PipeDirection _direction;

	private PipeOptions _options;

	private int _inBufferSize;

	private int _outBufferSize;

	private HandleInheritability _inheritability;

	public const int MaxAllowedServerInstances = -1;

	public override int InBufferSize
	{
		get
		{
			CheckPipePropertyOperations();
			if (!CanRead)
			{
				throw new NotSupportedException("Stream does not support reading.");
			}
			return base.InternalHandle?.NamedPipeSocket?.ReceiveBufferSize ?? _inBufferSize;
		}
	}

	public override int OutBufferSize
	{
		get
		{
			CheckPipePropertyOperations();
			if (!CanWrite)
			{
				throw new NotSupportedException("Stream does not support writing.");
			}
			return base.InternalHandle?.NamedPipeSocket?.SendBufferSize ?? _outBufferSize;
		}
	}

	private void Create(string pipeName, PipeDirection direction, int maxNumberOfServerInstances, PipeTransmissionMode transmissionMode, PipeOptions options, int inBufferSize, int outBufferSize, HandleInheritability inheritability)
	{
		if (transmissionMode == PipeTransmissionMode.Message)
		{
			throw new PlatformNotSupportedException("Message transmission mode is not supported on this platform.");
		}
		_instance = SharedServer.Get(PipeStream.GetPipePath(".", pipeName), (maxNumberOfServerInstances == -1) ? int.MaxValue : maxNumberOfServerInstances);
		_direction = direction;
		_options = options;
		_inBufferSize = inBufferSize;
		_outBufferSize = outBufferSize;
		_inheritability = inheritability;
	}

	public void WaitForConnection()
	{
		CheckConnectOperationsServer();
		if (base.State == PipeState.Connected)
		{
			throw new InvalidOperationException("Already in a connected state.");
		}
		Socket result = _instance.ListeningSocket.AcceptAsync().GetAwaiter().GetResult();
		HandleAcceptedSocket(result);
	}

	public Task WaitForConnectionAsync(CancellationToken cancellationToken)
	{
		CheckConnectOperationsServer();
		if (base.State == PipeState.Connected)
		{
			throw new InvalidOperationException("Already in a connected state.");
		}
		if (!cancellationToken.IsCancellationRequested)
		{
			return WaitForConnectionAsyncCore();
		}
		return Task.FromCanceled(cancellationToken);
		async Task WaitForConnectionAsyncCore()
		{
			HandleAcceptedSocket(await _instance.ListeningSocket.AcceptAsync().ConfigureAwait(continueOnCapturedContext: false));
		}
	}

	private void HandleAcceptedSocket(Socket acceptedSocket)
	{
		SafePipeHandle safePipeHandle = new SafePipeHandle(acceptedSocket);
		try
		{
			if (base.IsCurrentUserOnly)
			{
				uint eUid = global::Interop.Sys.GetEUid();
				if (global::Interop.Sys.GetPeerID(safePipeHandle, out var euid) == -1)
				{
					throw PipeStream.CreateExceptionForLastError(_instance?.PipeName);
				}
				if (eUid != euid)
				{
					throw new UnauthorizedAccessException($"Client connection (user id {euid}) was refused because it was not owned by the current user (id {eUid}).");
				}
			}
			PipeStream.ConfigureSocket(acceptedSocket, safePipeHandle, _direction, _inBufferSize, _outBufferSize, _inheritability);
		}
		catch
		{
			safePipeHandle.Dispose();
			acceptedSocket.Dispose();
			throw;
		}
		InitializeHandle(safePipeHandle, isExposed: false, (_options & PipeOptions.Asynchronous) != 0);
		base.State = PipeState.Connected;
	}

	internal override void DisposeCore(bool disposing)
	{
		Interlocked.Exchange(ref _instance, null)?.Dispose(disposing);
	}

	public void Disconnect()
	{
		CheckDisconnectOperations();
		base.State = PipeState.Disconnected;
		base.InternalHandle.Dispose();
		InitializeHandle(null, isExposed: false, isAsync: false);
	}

	public string GetImpersonationUserName()
	{
		CheckWriteOperations();
		string peerUserName = global::Interop.Sys.GetPeerUserName(base.InternalHandle?.NamedPipeSocketHandle ?? throw new InvalidOperationException("Pipe handle has not been set.  Did your PipeStream implementation call InitializeHandle?"));
		if (peerUserName != null)
		{
			return peerUserName;
		}
		throw PipeStream.CreateExceptionForLastError(_instance?.PipeName);
	}

	public void RunAsClient(PipeStreamImpersonationWorker impersonationWorker)
	{
		CheckWriteOperations();
		SafeHandle socket = base.InternalHandle?.NamedPipeSocketHandle ?? throw new InvalidOperationException("Pipe handle has not been set.  Did your PipeStream implementation call InitializeHandle?");
		uint eUid = global::Interop.Sys.GetEUid();
		if (global::Interop.Sys.GetPeerID(socket, out var euid) == -1)
		{
			throw PipeStream.CreateExceptionForLastError(_instance?.PipeName);
		}
		if (global::Interop.Sys.SetEUid(euid) == -1)
		{
			throw PipeStream.CreateExceptionForLastError(_instance?.PipeName);
		}
		try
		{
			impersonationWorker();
		}
		finally
		{
			global::Interop.Sys.SetEUid(eUid);
		}
	}

	public NamedPipeServerStream(string pipeName)
		: this(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.None, 0, 0, HandleInheritability.None)
	{
	}

	public NamedPipeServerStream(string pipeName, PipeDirection direction)
		: this(pipeName, direction, 1, PipeTransmissionMode.Byte, PipeOptions.None, 0, 0, HandleInheritability.None)
	{
	}

	public NamedPipeServerStream(string pipeName, PipeDirection direction, int maxNumberOfServerInstances)
		: this(pipeName, direction, maxNumberOfServerInstances, PipeTransmissionMode.Byte, PipeOptions.None, 0, 0, HandleInheritability.None)
	{
	}

	public NamedPipeServerStream(string pipeName, PipeDirection direction, int maxNumberOfServerInstances, PipeTransmissionMode transmissionMode)
		: this(pipeName, direction, maxNumberOfServerInstances, transmissionMode, PipeOptions.None, 0, 0, HandleInheritability.None)
	{
	}

	public NamedPipeServerStream(string pipeName, PipeDirection direction, int maxNumberOfServerInstances, PipeTransmissionMode transmissionMode, PipeOptions options)
		: this(pipeName, direction, maxNumberOfServerInstances, transmissionMode, options, 0, 0, HandleInheritability.None)
	{
	}

	public NamedPipeServerStream(string pipeName, PipeDirection direction, int maxNumberOfServerInstances, PipeTransmissionMode transmissionMode, PipeOptions options, int inBufferSize, int outBufferSize)
		: this(pipeName, direction, maxNumberOfServerInstances, transmissionMode, options, inBufferSize, outBufferSize, HandleInheritability.None)
	{
	}

	private NamedPipeServerStream(string pipeName, PipeDirection direction, int maxNumberOfServerInstances, PipeTransmissionMode transmissionMode, PipeOptions options, int inBufferSize, int outBufferSize, HandleInheritability inheritability)
		: base(direction, transmissionMode, outBufferSize)
	{
		if (pipeName == null)
		{
			throw new ArgumentNullException("pipeName");
		}
		if (pipeName.Length == 0)
		{
			throw new ArgumentException("pipeName cannot be an empty string.");
		}
		if ((options & (PipeOptions)536870911) != PipeOptions.None)
		{
			throw new ArgumentOutOfRangeException("options", "options contains an invalid flag.");
		}
		if (inBufferSize < 0)
		{
			throw new ArgumentOutOfRangeException("inBufferSize", "Non negative number is required.");
		}
		if ((maxNumberOfServerInstances < 1 || maxNumberOfServerInstances > 254) && maxNumberOfServerInstances != -1)
		{
			throw new ArgumentOutOfRangeException("maxNumberOfServerInstances", "maxNumberOfServerInstances must either be a value between 1 and 254, or NamedPipeServerStream.MaxAllowedServerInstances (to obtain the maximum number allowed by system resources).");
		}
		if (inheritability < HandleInheritability.None || inheritability > HandleInheritability.Inheritable)
		{
			throw new ArgumentOutOfRangeException("inheritability", "HandleInheritability.None or HandleInheritability.Inheritable required.");
		}
		if ((options & PipeOptions.CurrentUserOnly) != PipeOptions.None)
		{
			base.IsCurrentUserOnly = true;
		}
		Create(pipeName, direction, maxNumberOfServerInstances, transmissionMode, options, inBufferSize, outBufferSize, inheritability);
	}

	public NamedPipeServerStream(PipeDirection direction, bool isAsync, bool isConnected, SafePipeHandle safePipeHandle)
		: base(direction, PipeTransmissionMode.Byte, 0)
	{
		if (safePipeHandle == null)
		{
			throw new ArgumentNullException("safePipeHandle");
		}
		if (safePipeHandle.IsInvalid)
		{
			throw new ArgumentException("Invalid handle.", "safePipeHandle");
		}
		ValidateHandleIsPipe(safePipeHandle);
		InitializeHandle(safePipeHandle, isExposed: true, isAsync);
		if (isConnected)
		{
			base.State = PipeState.Connected;
		}
	}

	~NamedPipeServerStream()
	{
		Dispose(disposing: false);
	}

	public Task WaitForConnectionAsync()
	{
		return WaitForConnectionAsync(CancellationToken.None);
	}

	public IAsyncResult BeginWaitForConnection(AsyncCallback callback, object state)
	{
		return TaskToApm.Begin(WaitForConnectionAsync(), callback, state);
	}

	public void EndWaitForConnection(IAsyncResult asyncResult)
	{
		TaskToApm.End(asyncResult);
	}

	private void CheckConnectOperationsServer()
	{
		if (base.State == PipeState.Closed)
		{
			throw Error.GetPipeNotOpen();
		}
		if (base.InternalHandle != null && base.InternalHandle.IsClosed)
		{
			throw Error.GetPipeNotOpen();
		}
		if (base.State == PipeState.Broken)
		{
			throw new IOException("Pipe is broken.");
		}
	}

	private void CheckDisconnectOperations()
	{
		if (base.State == PipeState.WaitingToConnect)
		{
			throw new InvalidOperationException("Pipe hasn't been connected yet.");
		}
		if (base.State == PipeState.Disconnected)
		{
			throw new InvalidOperationException("Already in a disconnected state.");
		}
		_ = base.InternalHandle;
		if (base.State == PipeState.Closed || (base.InternalHandle != null && base.InternalHandle.IsClosed))
		{
			throw Error.GetPipeNotOpen();
		}
	}

	public NamedPipeServerStream(string pipeName, PipeDirection direction, int maxNumberOfServerInstances, PipeTransmissionMode transmissionMode, PipeOptions options, int inBufferSize, int outBufferSize, PipeSecurity pipeSecurity)
		: this(pipeName, direction, maxNumberOfServerInstances, transmissionMode, options, inBufferSize, outBufferSize, pipeSecurity, HandleInheritability.None, (PipeAccessRights)0)
	{
	}

	public NamedPipeServerStream(string pipeName, PipeDirection direction, int maxNumberOfServerInstances, PipeTransmissionMode transmissionMode, PipeOptions options, int inBufferSize, int outBufferSize, PipeSecurity pipeSecurity, HandleInheritability inheritability)
		: this(pipeName, direction, maxNumberOfServerInstances, transmissionMode, options, inBufferSize, outBufferSize, pipeSecurity, inheritability, (PipeAccessRights)0)
	{
	}

	public NamedPipeServerStream(string pipeName, PipeDirection direction, int maxNumberOfServerInstances, PipeTransmissionMode transmissionMode, PipeOptions options, int inBufferSize, int outBufferSize, PipeSecurity pipeSecurity, HandleInheritability inheritability, PipeAccessRights additionalAccessRights)
		: this(pipeName, direction, maxNumberOfServerInstances, transmissionMode, options, inBufferSize, outBufferSize, inheritability)
	{
		if (additionalAccessRights != 0 || pipeSecurity != null)
		{
			throw new PlatformNotSupportedException();
		}
	}
}
