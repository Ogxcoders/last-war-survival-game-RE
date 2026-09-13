using Microsoft.Win32.SafeHandles;

namespace System.IO.Pipes;

public sealed class AnonymousPipeServerStream : PipeStream
{
	private SafePipeHandle _clientHandle;

	private bool _clientHandleExposed;

	public SafePipeHandle ClientSafePipeHandle
	{
		get
		{
			_clientHandleExposed = true;
			return _clientHandle;
		}
	}

	public override PipeTransmissionMode TransmissionMode => PipeTransmissionMode.Byte;

	public override PipeTransmissionMode ReadMode
	{
		set
		{
			CheckPipePropertyOperations();
			switch (value)
			{
			default:
				throw new ArgumentOutOfRangeException("value", "For named pipes, transmission mode can be TransmissionMode.Byte or PipeTransmissionMode.Message. For anonymous pipes, transmission mode can be TransmissionMode.Byte.");
			case PipeTransmissionMode.Message:
				throw new NotSupportedException("Anonymous pipes do not support PipeTransmissionMode.Message ReadMode.");
			case PipeTransmissionMode.Byte:
				break;
			}
		}
	}

	private void Create(PipeDirection direction, HandleInheritability inheritability, int bufferSize)
	{
		SafePipeHandle writer;
		SafePipeHandle reader;
		if (direction == PipeDirection.In)
		{
			PipeStream.CreateAnonymousPipe(inheritability, out writer, out reader);
		}
		else
		{
			PipeStream.CreateAnonymousPipe(inheritability, out reader, out writer);
		}
		if (bufferSize > 0 && global::Interop.Sys.Fcntl.CanGetSetPipeSz)
		{
			global::Interop.Sys.Fcntl.SetPipeSz(writer, bufferSize);
		}
		InitializeHandle(writer, isExposed: false, isAsync: false);
		_clientHandle = reader;
		base.State = PipeState.Connected;
	}

	public AnonymousPipeServerStream()
		: this(PipeDirection.Out, HandleInheritability.None, 0)
	{
	}

	public AnonymousPipeServerStream(PipeDirection direction)
		: this(direction, HandleInheritability.None, 0)
	{
	}

	public AnonymousPipeServerStream(PipeDirection direction, HandleInheritability inheritability)
		: this(direction, inheritability, 0)
	{
	}

	public AnonymousPipeServerStream(PipeDirection direction, SafePipeHandle serverSafePipeHandle, SafePipeHandle clientSafePipeHandle)
		: base(direction, 0)
	{
		if (direction == PipeDirection.InOut)
		{
			throw new NotSupportedException("Anonymous pipes can only be in one direction.");
		}
		if (serverSafePipeHandle == null)
		{
			throw new ArgumentNullException("serverSafePipeHandle");
		}
		if (clientSafePipeHandle == null)
		{
			throw new ArgumentNullException("clientSafePipeHandle");
		}
		if (serverSafePipeHandle.IsInvalid)
		{
			throw new ArgumentException("Invalid handle.", "serverSafePipeHandle");
		}
		if (clientSafePipeHandle.IsInvalid)
		{
			throw new ArgumentException("Invalid handle.", "clientSafePipeHandle");
		}
		ValidateHandleIsPipe(serverSafePipeHandle);
		ValidateHandleIsPipe(clientSafePipeHandle);
		InitializeHandle(serverSafePipeHandle, isExposed: true, isAsync: false);
		_clientHandle = clientSafePipeHandle;
		_clientHandleExposed = true;
		base.State = PipeState.Connected;
	}

	public AnonymousPipeServerStream(PipeDirection direction, HandleInheritability inheritability, int bufferSize)
		: base(direction, bufferSize)
	{
		if (direction == PipeDirection.InOut)
		{
			throw new NotSupportedException("Anonymous pipes can only be in one direction.");
		}
		if (inheritability < HandleInheritability.None || inheritability > HandleInheritability.Inheritable)
		{
			throw new ArgumentOutOfRangeException("inheritability", "HandleInheritability.None or HandleInheritability.Inheritable required.");
		}
		Create(direction, inheritability, bufferSize);
	}

	~AnonymousPipeServerStream()
	{
		Dispose(disposing: false);
	}

	public string GetClientHandleAsString()
	{
		_clientHandleExposed = true;
		GC.SuppressFinalize(_clientHandle);
		return _clientHandle.DangerousGetHandle().ToString();
	}

	public void DisposeLocalCopyOfClientHandle()
	{
		if (_clientHandle != null && !_clientHandle.IsClosed)
		{
			_clientHandle.Dispose();
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_clientHandleExposed && _clientHandle != null && !_clientHandle.IsClosed)
			{
				_clientHandle.Dispose();
			}
		}
		finally
		{
			base.Dispose(disposing);
		}
	}

	public AnonymousPipeServerStream(PipeDirection direction, HandleInheritability inheritability, int bufferSize, PipeSecurity pipeSecurity)
		: this(direction, inheritability, bufferSize)
	{
		if (pipeSecurity != null)
		{
			throw new PlatformNotSupportedException();
		}
	}
}
