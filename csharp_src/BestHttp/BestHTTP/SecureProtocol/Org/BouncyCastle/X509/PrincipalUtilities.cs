using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

public class PrincipalUtilities
{
	public static X509Name GetIssuerX509Principal(X509Certificate cert)
	{
		try
		{
			return TbsCertificateStructure.GetInstance(Asn1Object.FromByteArray(cert.GetTbsCertificate())).Issuer;
		}
		catch (Exception e)
		{
			throw new CertificateEncodingException("Could not extract issuer", e);
		}
	}

	public static X509Name GetSubjectX509Principal(X509Certificate cert)
	{
		try
		{
			return TbsCertificateStructure.GetInstance(Asn1Object.FromByteArray(cert.GetTbsCertificate())).Subject;
		}
		catch (Exception e)
		{
			throw new CertificateEncodingException("Could not extract subject", e);
		}
	}

	public static X509Name GetIssuerX509Principal(X509Crl crl)
	{
		try
		{
			return TbsCertificateList.GetInstance(Asn1Object.FromByteArray(crl.GetTbsCertList())).Issuer;
		}
		catch (Exception e)
		{
			throw new CrlException("Could not extract issuer", e);
		}
	}
}
