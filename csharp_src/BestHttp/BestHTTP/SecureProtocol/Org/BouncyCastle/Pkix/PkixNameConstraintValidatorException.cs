using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkix;

[Serializable]
public class PkixNameConstraintValidatorException : Exception
{
	public PkixNameConstraintValidatorException(string msg)
		: base(msg)
	{
	}
}
