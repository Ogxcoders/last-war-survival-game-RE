using System.Runtime.Serialization;

namespace System;

[Serializable]
public sealed class InsufficientMemoryException : OutOfMemoryException
{
	public InsufficientMemoryException()
		: base("Insufficient memory to continue the execution of the program.")
	{
		base.HResult = -2146233027;
	}

	public InsufficientMemoryException(string message)
		: base(message)
	{
		base.HResult = -2146233027;
	}

	public InsufficientMemoryException(string message, Exception innerException)
		: base(message, innerException)
	{
		base.HResult = -2146233027;
	}

	private InsufficientMemoryException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
