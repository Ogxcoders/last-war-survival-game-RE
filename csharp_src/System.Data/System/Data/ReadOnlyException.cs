using System.Runtime.Serialization;

namespace System.Data;

[Serializable]
public class ReadOnlyException : DataException
{
	protected ReadOnlyException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public ReadOnlyException()
		: base("Column is marked read only.")
	{
		base.HResult = -2146232025;
	}

	public ReadOnlyException(string s)
		: base(s)
	{
		base.HResult = -2146232025;
	}

	public ReadOnlyException(string message, Exception innerException)
		: base(message, innerException)
	{
		base.HResult = -2146232025;
	}
}
