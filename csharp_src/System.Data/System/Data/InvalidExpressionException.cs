using System.Runtime.Serialization;

namespace System.Data;

[Serializable]
public class InvalidExpressionException : DataException
{
	protected InvalidExpressionException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public InvalidExpressionException()
	{
	}

	public InvalidExpressionException(string s)
		: base(s)
	{
	}

	public InvalidExpressionException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
