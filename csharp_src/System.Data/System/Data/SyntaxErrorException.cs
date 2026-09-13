using System.Runtime.Serialization;

namespace System.Data;

[Serializable]
public class SyntaxErrorException : InvalidExpressionException
{
	protected SyntaxErrorException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public SyntaxErrorException()
	{
	}

	public SyntaxErrorException(string s)
		: base(s)
	{
	}

	public SyntaxErrorException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
