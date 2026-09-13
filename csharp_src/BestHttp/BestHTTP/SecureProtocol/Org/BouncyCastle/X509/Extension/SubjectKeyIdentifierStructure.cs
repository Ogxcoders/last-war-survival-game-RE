using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Extension;

public class SubjectKeyIdentifierStructure : SubjectKeyIdentifier
{
	public SubjectKeyIdentifierStructure(Asn1OctetString encodedValue)
		: base((Asn1OctetString)X509ExtensionUtilities.FromExtensionValue(encodedValue))
	{
	}

	private static Asn1OctetString FromPublicKey(AsymmetricKeyParameter pubKey)
	{
		try
		{
			return (Asn1OctetString)new SubjectKeyIdentifier(SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(pubKey)).ToAsn1Object();
		}
		catch (Exception ex)
		{
			throw new CertificateParsingException("Exception extracting certificate details: " + ex.ToString());
		}
	}

	public SubjectKeyIdentifierStructure(AsymmetricKeyParameter pubKey)
		: base(FromPublicKey(pubKey))
	{
	}
}
