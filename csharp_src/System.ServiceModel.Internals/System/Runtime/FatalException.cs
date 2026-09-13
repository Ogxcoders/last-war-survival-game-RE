using System.Runtime.Serialization;

namespace System.Runtime;

[Serializable]
internal class FatalException : SystemException
{
	public FatalException()
	{
	}

	public FatalException(string message)
		: base(message)
	{
	}

	public FatalException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected FatalException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
