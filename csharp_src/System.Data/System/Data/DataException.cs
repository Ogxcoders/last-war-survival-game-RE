using System.Runtime.Serialization;

namespace System.Data;

[Serializable]
public class DataException : SystemException
{
	protected DataException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public DataException()
		: base("Data Exception.")
	{
		base.HResult = -2146232032;
	}

	public DataException(string s)
		: base(s)
	{
		base.HResult = -2146232032;
	}

	public DataException(string s, Exception innerException)
		: base(s, innerException)
	{
	}
}
