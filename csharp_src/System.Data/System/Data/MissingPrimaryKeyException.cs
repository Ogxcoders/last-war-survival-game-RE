using System.Runtime.Serialization;

namespace System.Data;

[Serializable]
public class MissingPrimaryKeyException : DataException
{
	protected MissingPrimaryKeyException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public MissingPrimaryKeyException()
		: base("Missing primary key.")
	{
		base.HResult = -2146232027;
	}

	public MissingPrimaryKeyException(string s)
		: base(s)
	{
		base.HResult = -2146232027;
	}

	public MissingPrimaryKeyException(string message, Exception innerException)
		: base(message, innerException)
	{
		base.HResult = -2146232027;
	}
}
