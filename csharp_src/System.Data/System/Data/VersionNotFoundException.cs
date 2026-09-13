using System.Runtime.Serialization;

namespace System.Data;

[Serializable]
public class VersionNotFoundException : DataException
{
	protected VersionNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public VersionNotFoundException()
		: base("Version not found.")
	{
		base.HResult = -2146232023;
	}

	public VersionNotFoundException(string s)
		: base(s)
	{
		base.HResult = -2146232023;
	}

	public VersionNotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
		base.HResult = -2146232023;
	}
}
