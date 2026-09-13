namespace System.IO;

internal static class Error
{
	internal static Exception GetEndOfFile()
	{
		return new EndOfStreamException("Unable to read beyond the end of the stream.");
	}

	internal static Exception GetPipeNotOpen()
	{
		return new ObjectDisposedException(null, "Cannot access a closed pipe.");
	}

	internal static Exception GetReadNotSupported()
	{
		return new NotSupportedException("Stream does not support reading.");
	}

	internal static Exception GetSeekNotSupported()
	{
		return new NotSupportedException("Stream does not support seeking.");
	}

	internal static Exception GetWriteNotSupported()
	{
		return new NotSupportedException("Stream does not support writing.");
	}

	internal static Exception GetOperationAborted()
	{
		return new IOException("IO operation was aborted unexpectedly.");
	}
}
