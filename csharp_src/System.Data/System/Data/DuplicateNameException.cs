using System.Runtime.Serialization;

namespace System.Data;

[Serializable]
public class DuplicateNameException : DataException
{
	protected DuplicateNameException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public DuplicateNameException()
		: base("Duplicate name not allowed.")
	{
		base.HResult = -2146232030;
	}

	public DuplicateNameException(string s)
		: base(s)
	{
		base.HResult = -2146232030;
	}

	public DuplicateNameException(string message, Exception innerException)
		: base(message, innerException)
	{
		base.HResult = -2146232030;
	}
}
