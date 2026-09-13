using System.Runtime.Serialization;

namespace System.Data;

[Serializable]
public class NoNullAllowedException : DataException
{
	protected NoNullAllowedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public NoNullAllowedException()
		: base("Null not allowed.")
	{
		base.HResult = -2146232026;
	}

	public NoNullAllowedException(string s)
		: base(s)
	{
		base.HResult = -2146232026;
	}

	public NoNullAllowedException(string message, Exception innerException)
		: base(message, innerException)
	{
		base.HResult = -2146232026;
	}
}
