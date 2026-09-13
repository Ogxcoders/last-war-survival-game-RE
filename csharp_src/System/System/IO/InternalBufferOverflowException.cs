using System.Runtime.Serialization;

namespace System.IO;

[Serializable]
public class InternalBufferOverflowException : SystemException
{
	public InternalBufferOverflowException()
		: base("Internal buffer overflow occurred.")
	{
	}

	public InternalBufferOverflowException(string message)
		: base(message)
	{
	}

	protected InternalBufferOverflowException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public InternalBufferOverflowException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
