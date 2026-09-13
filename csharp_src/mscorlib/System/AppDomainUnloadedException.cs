using System.Runtime.Serialization;

namespace System;

[Serializable]
public class AppDomainUnloadedException : SystemException
{
	internal const int COR_E_APPDOMAINUNLOADED = -2146234348;

	public AppDomainUnloadedException()
		: base("Attempted to access an unloaded AppDomain.")
	{
		base.HResult = -2146234348;
	}

	public AppDomainUnloadedException(string message)
		: base(message)
	{
		base.HResult = -2146234348;
	}

	public AppDomainUnloadedException(string message, Exception innerException)
		: base(message, innerException)
	{
		base.HResult = -2146234348;
	}

	protected AppDomainUnloadedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
