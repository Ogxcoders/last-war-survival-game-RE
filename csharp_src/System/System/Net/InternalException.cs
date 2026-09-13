using System.Runtime.Serialization;

namespace System.Net;

internal class InternalException : SystemException
{
	internal InternalException()
	{
	}

	internal InternalException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
