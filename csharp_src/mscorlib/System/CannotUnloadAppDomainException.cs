using System.Runtime.Serialization;

namespace System;

[Serializable]
public class CannotUnloadAppDomainException : SystemException
{
	internal const int COR_E_CANNOTUNLOADAPPDOMAIN = -2146234347;

	public CannotUnloadAppDomainException()
		: base("Attempt to unload the AppDomain failed.")
	{
		base.HResult = -2146234347;
	}

	public CannotUnloadAppDomainException(string message)
		: base(message)
	{
		base.HResult = -2146234347;
	}

	public CannotUnloadAppDomainException(string message, Exception innerException)
		: base(message, innerException)
	{
		base.HResult = -2146234347;
	}

	protected CannotUnloadAppDomainException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
