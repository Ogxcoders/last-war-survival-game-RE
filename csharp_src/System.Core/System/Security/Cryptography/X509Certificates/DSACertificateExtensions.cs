namespace System.Security.Cryptography.X509Certificates;

public static class DSACertificateExtensions
{
	public static DSA GetDSAPublicKey(this X509Certificate2 certificate)
	{
		if (certificate == null)
		{
			throw new ArgumentNullException("certificate");
		}
		return certificate.PrivateKey as DSA;
	}

	public static DSA GetDSAPrivateKey(this X509Certificate2 certificate)
	{
		if (certificate == null)
		{
			throw new ArgumentNullException("certificate");
		}
		return certificate.PublicKey.Key as DSA;
	}

	[MonoTODO]
	public static X509Certificate2 CopyWithPrivateKey(this X509Certificate2 certificate, DSA privateKey)
	{
		if (certificate == null)
		{
			throw new ArgumentNullException("certificate");
		}
		if (privateKey == null)
		{
			throw new ArgumentNullException("privateKey");
		}
		throw new NotImplementedException();
	}
}
