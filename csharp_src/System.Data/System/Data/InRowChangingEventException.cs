using System.Runtime.Serialization;

namespace System.Data;

[Serializable]
public class InRowChangingEventException : DataException
{
	protected InRowChangingEventException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public InRowChangingEventException()
		: base("Operation not supported in the RowChanging event.")
	{
		base.HResult = -2146232029;
	}

	public InRowChangingEventException(string s)
		: base(s)
	{
		base.HResult = -2146232029;
	}

	public InRowChangingEventException(string message, Exception innerException)
		: base(message, innerException)
	{
		base.HResult = -2146232029;
	}
}
