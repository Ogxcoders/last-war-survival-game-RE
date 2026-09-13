using System.Runtime.Serialization;

namespace System.Data;

[Serializable]
public class RowNotInTableException : DataException
{
	protected RowNotInTableException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public RowNotInTableException()
		: base("Row not found in table.")
	{
		base.HResult = -2146232024;
	}

	public RowNotInTableException(string s)
		: base(s)
	{
		base.HResult = -2146232024;
	}

	public RowNotInTableException(string message, Exception innerException)
		: base(message, innerException)
	{
		base.HResult = -2146232024;
	}
}
