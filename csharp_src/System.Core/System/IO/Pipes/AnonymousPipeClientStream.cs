using Microsoft.Win32.SafeHandles;

namespace System.IO.Pipes;

public sealed class AnonymousPipeClientStream : PipeStream
{
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

	public AnonymousPipeClientStream(string pipeHandleAsString)
		: this(PipeDirection.In, pipeHandleAsString)
	{
	}

	public AnonymousPipeClientStream(PipeDirection direction, string pipeHandleAsString)
		: base(direction, 0)
	{
		if (direction == PipeDirection.InOut)
		{
			throw new NotSupportedException("Anonymous pipes can only be in one direction.");
		}
		if (pipeHandleAsString == null)
		{
			throw new ArgumentNullException("pipeHandleAsString");
		}
		long result = 0L;
		if (!long.TryParse(pipeHandleAsString, out result))
		{
			throw new ArgumentException("Invalid handle.", "pipeHandleAsString");
		}
		SafePipeHandle safePipeHandle = new SafePipeHandle((IntPtr)result, ownsHandle: true);
		if (safePipeHandle.IsInvalid)
		{
			throw new ArgumentException("Invalid handle.", "pipeHandleAsString");
		}
		Init(direction, safePipeHandle);
	}

	public AnonymousPipeClientStream(PipeDirection direction, SafePipeHandle safePipeHandle)
		: base(direction, 0)
	{
		if (direction == PipeDirection.InOut)
		{
			throw new NotSupportedException("Anonymous pipes can only be in one direction.");
		}
		if (safePipeHandle == null)
		{
			throw new ArgumentNullException("safePipeHandle");
		}
		if (safePipeHandle.IsInvalid)
		{
			throw new ArgumentException("Invalid handle.", "safePipeHandle");
		}
		Init(direction, safePipeHandle);
	}

	private void Init(PipeDirection direction, SafePipeHandle safePipeHandle)
	{
		ValidateHandleIsPipe(safePipeHandle);
		InitializeHandle(safePipeHandle, isExposed: true, isAsync: false);
		base.State = PipeState.Connected;
	}

	~AnonymousPipeClientStream()
	{
		Dispose(disposing: false);
	}
}
