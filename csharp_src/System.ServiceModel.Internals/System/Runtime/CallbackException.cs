using System.Runtime.Serialization;

namespace System.Runtime;

[Serializable]
internal class CallbackException : FatalException
{
	public CallbackException()
	{
	}

	public CallbackException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected CallbackException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
