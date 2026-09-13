using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkix;

[Serializable]
public class PkixCertPathBuilderException : GeneralSecurityException
{
	public PkixCertPathBuilderException()
	{
	}

	public PkixCertPathBuilderException(string message)
		: base(message)
	{
	}

	public PkixCertPathBuilderException(string message, Exception exception)
		: base(message, exception)
	{
	}
}
