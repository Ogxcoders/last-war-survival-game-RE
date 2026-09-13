namespace System.Security.Cryptography.X509Certificates;

public static class RSACertificateExtensions
{
	public static RSA GetRSAPrivateKey(this X509Certificate2 certificate)
	{
		if (certificate == null)
		{
			throw new ArgumentNullException("certificate");
		}
		if (!certificate.HasPrivateKey)
		{
			return null;
		}
		return certificate.Impl.GetRSAPrivateKey();
	}

	public static RSA GetRSAPublicKey(this X509Certificate2 certificate)
	{
		if (certificate == null)
		{
			throw new ArgumentNullException("certificate");
		}
		return certificate.PublicKey.Key as RSA;
	}

	public static X509Certificate2 CopyWithPrivateKey(this X509Certificate2 certificate, RSA privateKey)
	{
		if (certificate == null)
		{
			throw new ArgumentNullException("certificate");
		}
		if (privateKey == null)
		{
			throw new ArgumentNullException("privateKey");
		}
		return (X509Certificate2)certificate.Impl.CopyWithPrivateKey(privateKey).CreateCertificate();
	}
}
