using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Pipes;

public abstract class PipeStream : Stream
{
	internal const bool CheckOperationsRequiresSetHandle = false;

	private static readonly char[] s_invalidFileNameChars = Path.GetInvalidFileNameChars();

	private static readonly char[] s_invalidPathNameChars = Path.GetInvalidPathChars();

	private static readonly string s_pipePrefix = Path.Combine(Path.GetTempPath(), "CoreFxPipe_");

	internal const string AnonymousPipeName = "anonymous";

	private static readonly Task<int> s_zeroTask = Task.FromResult(0);

	private SafePipeHandle _handle;

	private bool _canRead;

	private bool _canWrite;

	private bool _isAsync;

	private bool _isCurrentUserOnly;

	private bool _isMessageComplete;

	private bool _isFromExistingHandle;

	private bool _isHandleExposed;

	private PipeTransmissionMode _readMode;

	private PipeTransmissionMode _transmissionMode;

	private PipeDirection _pipeDirection;

	private int _outBufferSize;

	private PipeState _state;

	public virtual PipeTransmissionMode TransmissionMode
	{
		get
		{
			CheckPipePropertyOperations();
			return PipeTransmissionMode.Byte;
		}
	}

	public virtual int InBufferSize
	{
		get
		{
			CheckPipePropertyOperations();
			if (!CanRead)
			{
				throw new NotSupportedException("Stream does not support reading.");
			}
			return GetPipeBufferSize();
		}
	}

	public virtual int OutBufferSize
	{
		get
		{
			CheckPipePropertyOperations();
			if (!CanWrite)
			{
				throw new NotSupportedException("Stream does not support writing.");
			}
			return GetPipeBufferSize();
		}
	}

	public virtual PipeTransmissionMode ReadMode
	{
		get
		{
			CheckPipePropertyOperations();
			return PipeTransmissionMode.Byte;
		}
		set
		{
			CheckPipePropertyOperations();
			switch (value)
			{
			default:
				throw new ArgumentOutOfRangeException("value", "For named pipes, transmission mode can be TransmissionMode.Byte or PipeTransmissionMode.Message. For anonymous pipes, transmission mode can be TransmissionMode.Byte.");
			case PipeTransmissionMode.Message:
				throw new PlatformNotSupportedException("Message transmission mode is not supported on this platform.");
			case PipeTransmissionMode.Byte:
				break;
			}
		}
	}

	public bool IsConnected
	{
		get
		{
			return State == PipeState.Connected;
		}
		protected set
		{
			_state = (value ? PipeState.Connected : PipeState.Disconnected);
		}
	}

	public bool IsAsync => _isAsync;

	public bool IsMessageComplete
	{
		get
		{
			if (_state == PipeState.WaitingToConnect)
			{
				throw new InvalidOperationException("Pipe hasn't been connected yet.");
			}
			if (_state == PipeState.Disconnected)
			{
				throw new InvalidOperationException("Pipe is in a disconnected state.");
			}
			if (_state == PipeState.Closed || (_handle != null && _handle.IsClosed))
			{
				throw Error.GetPipeNotOpen();
			}
			if (_readMode != PipeTransmissionMode.Message)
			{
				throw new InvalidOperationException("ReadMode is not of PipeTransmissionMode.Message.");
			}
			return _isMessageComplete;
		}
	}

	public SafePipeHandle SafePipeHandle
	{
		get
		{
			if (_handle == null)
			{
				throw new InvalidOperationException("Pipe handle has not been set.  Did your PipeStream implementation call InitializeHandle?");
			}
			if (_handle.IsClosed)
			{
				throw Error.GetPipeNotOpen();
			}
			_isHandleExposed = true;
			return _handle;
		}
	}

	internal SafePipeHandle InternalHandle => _handle;

	protected bool IsHandleExposed => _isHandleExposed;

	public override bool CanRead => _canRead;

	public override bool CanWrite => _canWrite;

	public override bool CanSeek => false;

	public override long Length
	{
		get
		{
			throw Error.GetSeekNotSupported();
		}
	}

	public override long Position
	{
		get
		{
			throw Error.GetSeekNotSupported();
		}
		set
		{
			throw Error.GetSeekNotSupported();
		}
	}

	internal PipeState State
	{
		get
		{
			return _state;
		}
		set
		{
			_state = value;
		}
	}

	internal bool IsCurrentUserOnly
	{
		get
		{
			return _isCurrentUserOnly;
		}
		set
		{
			_isCurrentUserOnly = value;
		}
	}

	internal static string GetPipePath(string serverName, string pipeName)
	{
		if (serverName != "." && serverName != global::Interop.Sys.GetHostName())
		{
			throw new PlatformNotSupportedException("Access to remote named pipes is not supported on this platform.");
		}
		if (string.Equals(pipeName, "anonymous", StringComparison.OrdinalIgnoreCase))
		{
			throw new ArgumentOutOfRangeException("pipeName", "The pipeName \\\"anonymous\\\" is reserved.");
		}
		if (Path.IsPathRooted(pipeName))
		{
			if (pipeName.IndexOfAny(s_invalidPathNameChars) >= 0 || pipeName[pipeName.Length - 1] == Path.DirectorySeparatorChar)
			{
				throw new PlatformNotSupportedException("The name of a pipe on this platform must be a valid file name or a valid absolute path to a file name.");
			}
			return pipeName;
		}
		if (pipeName.IndexOfAny(s_invalidFileNameChars) >= 0)
		{
			throw new PlatformNotSupportedException("The name of a pipe on this platform must be a valid file name or a valid absolute path to a file name.");
		}
		return s_pipePrefix + pipeName;
	}

	internal void ValidateHandleIsPipe(SafePipeHandle safePipeHandle)
	{
		if (safePipeHandle.NamedPipeSocket == null && CheckPipeCall(global::Interop.Sys.FStat(safePipeHandle, out var output)) == 0 && (output.Mode & 0xF000) != 4096 && (output.Mode & 0xF000) != 49152)
		{
			throw new IOException("Invalid pipe handle.");
		}
	}

	private void InitializeAsyncHandle(SafePipeHandle handle)
	{
	}

	internal virtual void DisposeCore(bool disposing)
	{
	}

	private unsafe int ReadCore(Span<byte> buffer)
	{
		Socket namedPipeSocket = _handle.NamedPipeSocket;
		if (namedPipeSocket != null)
		{
			try
			{
				return namedPipeSocket.Receive(buffer, SocketFlags.None);
			}
			catch (SocketException e)
			{
				throw GetIOExceptionForSocketException(e);
			}
		}
		fixed (byte* reference = &MemoryMarshal.GetReference(buffer))
		{
			return CheckPipeCall(global::Interop.Sys.Read(_handle, reference, buffer.Length));
		}
	}

	private unsafe void WriteCore(ReadOnlySpan<byte> buffer)
	{
		Socket namedPipeSocket = _handle.NamedPipeSocket;
		if (namedPipeSocket != null)
		{
			try
			{
				while (buffer.Length > 0)
				{
					int start = namedPipeSocket.Send(buffer, SocketFlags.None);
					buffer = buffer.Slice(start);
				}
			}
			catch (SocketException e)
			{
				throw GetIOExceptionForSocketException(e);
			}
		}
		fixed (byte* reference = &MemoryMarshal.GetReference(buffer))
		{
			while (buffer.Length > 0)
			{
				int start2 = CheckPipeCall(global::Interop.Sys.Write(_handle, reference, buffer.Length));
				buffer = buffer.Slice(start2);
			}
		}
	}

	private async Task<int> ReadAsyncCore(Memory<byte> destination, CancellationToken cancellationToken)
	{
		Socket socket = InternalHandle.NamedPipeSocket;
		try
		{
			if (cancellationToken.CanBeCanceled)
			{
				cancellationToken.ThrowIfCancellationRequested();
				if (socket.Available == 0)
				{
					Task<int> t = socket.ReceiveAsync(Array.Empty<byte>(), SocketFlags.None);
					if (!t.IsCompletedSuccessfully)
					{
						TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
						using (cancellationToken.Register(delegate(object s)
						{
							((TaskCompletionSource<bool>)s).TrySetResult(result: true);
						}, taskCompletionSource))
						{
							object obj = t;
							if (obj == await Task.WhenAny(t, taskCompletionSource.Task).ConfigureAwait(continueOnCapturedContext: false))
							{
								t.GetAwaiter().GetResult();
							}
							cancellationToken.ThrowIfCancellationRequested();
						}
					}
				}
			}
			ArraySegment<byte> segment;
			return await (MemoryMarshal.TryGetArray((ReadOnlyMemory<byte>)destination, out segment) ? socket.ReceiveAsync(segment, SocketFlags.None) : socket.ReceiveAsync(destination.ToArray(), SocketFlags.None)).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (SocketException e)
		{
			throw GetIOExceptionForSocketException(e);
		}
	}

	private async Task WriteAsyncCore(ReadOnlyMemory<byte> source, CancellationToken cancellationToken)
	{
		try
		{
			byte[] buffer;
			int offset;
			int count;
			if (MemoryMarshal.TryGetArray(source, out var segment))
			{
				buffer = segment.Array;
				offset = segment.Offset;
				count = segment.Count;
			}
			else
			{
				buffer = source.ToArray();
				offset = 0;
				count = buffer.Length;
			}
			while (count > 0)
			{
				cancellationToken.ThrowIfCancellationRequested();
				int num = await _handle.NamedPipeSocket.SendAsync(new ArraySegment<byte>(buffer, offset, count), SocketFlags.None).ConfigureAwait(continueOnCapturedContext: false);
				count -= num;
				offset += num;
			}
		}
		catch (SocketException e)
		{
			throw GetIOExceptionForSocketException(e);
		}
	}

	private IOException GetIOExceptionForSocketException(SocketException e)
	{
		if (e.SocketErrorCode == SocketError.Shutdown)
		{
			State = PipeState.Broken;
		}
		return new IOException(e.Message, e);
	}

	public void WaitForPipeDrain()
	{
		CheckWriteOperations();
		if (!CanWrite)
		{
			throw Error.GetWriteNotSupported();
		}
		throw new PlatformNotSupportedException();
	}

	private static void CreateDirectory(string directoryPath)
	{
		if (global::Interop.Sys.MkDir(directoryPath, 511) < 0)
		{
			global::Interop.ErrorInfo lastErrorInfo = global::Interop.Sys.GetLastErrorInfo();
			if (lastErrorInfo.Error != global::Interop.Error.EEXIST)
			{
				throw global::Interop.GetExceptionForIoErrno(lastErrorInfo, directoryPath, isDirectory: true);
			}
		}
	}

	internal unsafe static void CreateAnonymousPipe(HandleInheritability inheritability, out SafePipeHandle reader, out SafePipeHandle writer)
	{
		reader = new SafePipeHandle();
		writer = new SafePipeHandle();
		int* ptr = stackalloc int[2];
		CreateAnonymousPipe(inheritability, ptr);
		reader.SetHandle(*ptr);
		writer.SetHandle(ptr[1]);
	}

	internal int CheckPipeCall(int result)
	{
		if (result == -1)
		{
			global::Interop.ErrorInfo lastErrorInfo = global::Interop.Sys.GetLastErrorInfo();
			if (lastErrorInfo.Error == global::Interop.Error.EPIPE)
			{
				State = PipeState.Broken;
			}
			throw global::Interop.GetExceptionForIoErrno(lastErrorInfo);
		}
		return result;
	}

	private int GetPipeBufferSize()
	{
		if (!global::Interop.Sys.Fcntl.CanGetSetPipeSz)
		{
			throw new PlatformNotSupportedException();
		}
		if (_handle == null)
		{
			return _outBufferSize;
		}
		return CheckPipeCall(global::Interop.Sys.Fcntl.GetPipeSz(_handle));
	}

	internal unsafe static void CreateAnonymousPipe(HandleInheritability inheritability, int* fdsptr)
	{
		global::Interop.Sys.PipeFlags flags = (((inheritability & HandleInheritability.Inheritable) == 0) ? global::Interop.Sys.PipeFlags.O_CLOEXEC : ((global::Interop.Sys.PipeFlags)0));
		global::Interop.CheckIo(global::Interop.Sys.Pipe(fdsptr, flags));
	}

	internal static void ConfigureSocket(Socket s, SafePipeHandle pipeHandle, PipeDirection direction, int inBufferSize, int outBufferSize, HandleInheritability inheritability)
	{
		if (inBufferSize > 0)
		{
			s.ReceiveBufferSize = inBufferSize;
		}
		if (outBufferSize > 0)
		{
			s.SendBufferSize = outBufferSize;
		}
		if (inheritability != HandleInheritability.Inheritable)
		{
			global::Interop.Sys.Fcntl.SetCloseOnExec(pipeHandle);
		}
		switch (direction)
		{
		case PipeDirection.In:
			s.Shutdown(SocketShutdown.Send);
			break;
		case PipeDirection.Out:
			s.Shutdown(SocketShutdown.Receive);
			break;
		}
	}

	internal static Exception CreateExceptionForLastError(string pipeName = null)
	{
		global::Interop.ErrorInfo lastErrorInfo = global::Interop.Sys.GetLastErrorInfo();
		if (lastErrorInfo.Error != global::Interop.Error.ENOTSUP)
		{
			return global::Interop.GetExceptionForIoErrno(lastErrorInfo, pipeName);
		}
		return new PlatformNotSupportedException(SR.Format("The operating system returned error '{0}' indicating that the operation is not supported.", "ENOTSUP"));
	}

	protected PipeStream(PipeDirection direction, int bufferSize)
	{
		if (direction < PipeDirection.In || direction > PipeDirection.InOut)
		{
			throw new ArgumentOutOfRangeException("direction", "For named pipes, the pipe direction can be PipeDirection.In, PipeDirection.Out or PipeDirection.InOut. For anonymous pipes, the pipe direction can be PipeDirection.In or PipeDirection.Out.");
		}
		if (bufferSize < 0)
		{
			throw new ArgumentOutOfRangeException("bufferSize", "Non negative number is required.");
		}
		Init(direction, PipeTransmissionMode.Byte, bufferSize);
	}

	protected PipeStream(PipeDirection direction, PipeTransmissionMode transmissionMode, int outBufferSize)
	{
		if (direction < PipeDirection.In || direction > PipeDirection.InOut)
		{
			throw new ArgumentOutOfRangeException("direction", "For named pipes, the pipe direction can be PipeDirection.In, PipeDirection.Out or PipeDirection.InOut. For anonymous pipes, the pipe direction can be PipeDirection.In or PipeDirection.Out.");
		}
		if (transmissionMode < PipeTransmissionMode.Byte || transmissionMode > PipeTransmissionMode.Message)
		{
			throw new ArgumentOutOfRangeException("transmissionMode", "For named pipes, transmission mode can be TransmissionMode.Byte or PipeTransmissionMode.Message. For anonymous pipes, transmission mode can be TransmissionMode.Byte.");
		}
		if (outBufferSize < 0)
		{
			throw new ArgumentOutOfRangeException("outBufferSize", "Non negative number is required.");
		}
		Init(direction, transmissionMode, outBufferSize);
	}

	private void Init(PipeDirection direction, PipeTransmissionMode transmissionMode, int outBufferSize)
	{
		_readMode = transmissionMode;
		_transmissionMode = transmissionMode;
		_pipeDirection = direction;
		if ((_pipeDirection & PipeDirection.In) != 0)
		{
			_canRead = true;
		}
		if ((_pipeDirection & PipeDirection.Out) != 0)
		{
			_canWrite = true;
		}
		_outBufferSize = outBufferSize;
		_isMessageComplete = true;
		_state = PipeState.WaitingToConnect;
	}

	protected void InitializeHandle(SafePipeHandle handle, bool isExposed, bool isAsync)
	{
		if (isAsync && handle != null)
		{
			InitializeAsyncHandle(handle);
		}
		_handle = handle;
		_isAsync = isAsync;
		_isHandleExposed = isExposed;
		_isFromExistingHandle = isExposed;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (_isAsync)
		{
			return ReadAsync(buffer, offset, count, CancellationToken.None).GetAwaiter().GetResult();
		}
		CheckReadWriteArgs(buffer, offset, count);
		if (!CanRead)
		{
			throw Error.GetReadNotSupported();
		}
		CheckReadOperations();
		return ReadCore(new Span<byte>(buffer, offset, count));
	}

	public override int Read(Span<byte> buffer)
	{
		if (_isAsync)
		{
			return base.Read(buffer);
		}
		if (!CanRead)
		{
			throw Error.GetReadNotSupported();
		}
		CheckReadOperations();
		return ReadCore(buffer);
	}

	public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		CheckReadWriteArgs(buffer, offset, count);
		if (!CanRead)
		{
			throw Error.GetReadNotSupported();
		}
		if (cancellationToken.IsCancellationRequested)
		{
			return Task.FromCanceled<int>(cancellationToken);
		}
		CheckReadOperations();
		if (!_isAsync)
		{
			return base.ReadAsync(buffer, offset, count, cancellationToken);
		}
		if (count == 0)
		{
			UpdateMessageCompletion(completion: false);
			return s_zeroTask;
		}
		return ReadAsyncCore(new Memory<byte>(buffer, offset, count), cancellationToken);
	}

	public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (!_isAsync)
		{
			return base.ReadAsync(buffer, cancellationToken);
		}
		if (!CanRead)
		{
			throw Error.GetReadNotSupported();
		}
		if (cancellationToken.IsCancellationRequested)
		{
			return new ValueTask<int>(Task.FromCanceled<int>(cancellationToken));
		}
		CheckReadOperations();
		if (buffer.Length == 0)
		{
			UpdateMessageCompletion(completion: false);
			return new ValueTask<int>(0);
		}
		return new ValueTask<int>(ReadAsyncCore(buffer, cancellationToken));
	}

	public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		if (_isAsync)
		{
			return TaskToApm.Begin(ReadAsync(buffer, offset, count, CancellationToken.None), callback, state);
		}
		return base.BeginRead(buffer, offset, count, callback, state);
	}

	public override int EndRead(IAsyncResult asyncResult)
	{
		if (_isAsync)
		{
			return TaskToApm.End<int>(asyncResult);
		}
		return base.EndRead(asyncResult);
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		if (_isAsync)
		{
			WriteAsync(buffer, offset, count, CancellationToken.None).GetAwaiter().GetResult();
			return;
		}
		CheckReadWriteArgs(buffer, offset, count);
		if (!CanWrite)
		{
			throw Error.GetWriteNotSupported();
		}
		CheckWriteOperations();
		WriteCore(new ReadOnlySpan<byte>(buffer, offset, count));
	}

	public override void Write(ReadOnlySpan<byte> buffer)
	{
		if (_isAsync)
		{
			base.Write(buffer);
			return;
		}
		if (!CanWrite)
		{
			throw Error.GetWriteNotSupported();
		}
		CheckWriteOperations();
		WriteCore(buffer);
	}

	public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		CheckReadWriteArgs(buffer, offset, count);
		if (!CanWrite)
		{
			throw Error.GetWriteNotSupported();
		}
		if (cancellationToken.IsCancellationRequested)
		{
			return Task.FromCanceled<int>(cancellationToken);
		}
		CheckWriteOperations();
		if (!_isAsync)
		{
			return base.WriteAsync(buffer, offset, count, cancellationToken);
		}
		if (count == 0)
		{
			return Task.CompletedTask;
		}
		return WriteAsyncCore(new ReadOnlyMemory<byte>(buffer, offset, count), cancellationToken);
	}

	public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (!_isAsync)
		{
			return base.WriteAsync(buffer, cancellationToken);
		}
		if (!CanWrite)
		{
			throw Error.GetWriteNotSupported();
		}
		if (cancellationToken.IsCancellationRequested)
		{
			return new ValueTask(Task.FromCanceled<int>(cancellationToken));
		}
		CheckWriteOperations();
		if (buffer.Length == 0)
		{
			return default(ValueTask);
		}
		return new ValueTask(WriteAsyncCore(buffer, cancellationToken));
	}

	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		if (_isAsync)
		{
			return TaskToApm.Begin(WriteAsync(buffer, offset, count, CancellationToken.None), callback, state);
		}
		return base.BeginWrite(buffer, offset, count, callback, state);
	}

	public override void EndWrite(IAsyncResult asyncResult)
	{
		if (_isAsync)
		{
			TaskToApm.End(asyncResult);
		}
		else
		{
			base.EndWrite(asyncResult);
		}
	}

	private void CheckReadWriteArgs(byte[] buffer, int offset, int count)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer", "Buffer cannot be null.");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset", "Non negative number is required.");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count", "Non negative number is required.");
		}
		if (buffer.Length - offset < count)
		{
			throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
		}
	}

	[Conditional("DEBUG")]
	private static void DebugAssertHandleValid(SafePipeHandle handle)
	{
	}

	[Conditional("DEBUG")]
	private static void DebugAssertReadWriteArgs(byte[] buffer, int offset, int count, SafePipeHandle handle)
	{
	}

	public unsafe override int ReadByte()
	{
		byte result = default(byte);
		if (Read(new Span<byte>(&result, 1)) <= 0)
		{
			return -1;
		}
		return result;
	}

	public unsafe override void WriteByte(byte value)
	{
		Write(new ReadOnlySpan<byte>(&value, 1));
	}

	public override void Flush()
	{
		CheckWriteOperations();
		if (!CanWrite)
		{
			throw Error.GetWriteNotSupported();
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (_handle != null && !_handle.IsClosed)
			{
				_handle.Dispose();
			}
			DisposeCore(disposing);
		}
		finally
		{
			base.Dispose(disposing);
		}
		_state = PipeState.Closed;
	}

	internal void UpdateMessageCompletion(bool completion)
	{
		_isMessageComplete = completion || _state == PipeState.Broken;
	}

	public override void SetLength(long value)
	{
		throw Error.GetSeekNotSupported();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw Error.GetSeekNotSupported();
	}

	protected internal virtual void CheckPipePropertyOperations()
	{
		if (_state == PipeState.Closed || (_handle != null && _handle.IsClosed))
		{
			throw Error.GetPipeNotOpen();
		}
	}

	protected internal void CheckReadOperations()
	{
		if (_state == PipeState.WaitingToConnect)
		{
			throw new InvalidOperationException("Pipe hasn't been connected yet.");
		}
		if (_state == PipeState.Disconnected)
		{
			throw new InvalidOperationException("Pipe is in a disconnected state.");
		}
		if (_state == PipeState.Closed || (_handle != null && _handle.IsClosed))
		{
			throw Error.GetPipeNotOpen();
		}
	}

	protected internal void CheckWriteOperations()
	{
		if (_state == PipeState.WaitingToConnect)
		{
			throw new InvalidOperationException("Pipe hasn't been connected yet.");
		}
		if (_state == PipeState.Disconnected)
		{
			throw new InvalidOperationException("Pipe is in a disconnected state.");
		}
		if (_state == PipeState.Broken)
		{
			throw new IOException("Pipe is broken.");
		}
		if (_state == PipeState.Closed || (_handle != null && _handle.IsClosed))
		{
			throw Error.GetPipeNotOpen();
		}
	}

	public PipeSecurity GetAccessControl()
	{
		if (State == PipeState.Closed)
		{
			throw Error.GetPipeNotOpen();
		}
		return new PipeSecurity(SafePipeHandle, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
	}

	public void SetAccessControl(PipeSecurity pipeSecurity)
	{
		if (pipeSecurity == null)
		{
			throw new ArgumentNullException("pipeSecurity");
		}
		CheckPipePropertyOperations();
		pipeSecurity.Persist(SafePipeHandle);
	}
}
