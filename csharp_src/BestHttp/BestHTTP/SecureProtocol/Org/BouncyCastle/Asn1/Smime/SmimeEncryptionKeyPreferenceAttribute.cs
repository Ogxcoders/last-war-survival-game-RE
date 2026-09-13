using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Smime;

public class SmimeEncryptionKeyPreferenceAttribute : AttributeX509
{
	public SmimeEncryptionKeyPreferenceAttribute(IssuerAndSerialNumber issAndSer)
		: base(SmimeAttributes.EncrypKeyPref, new DerSet(new DerTaggedObject(explicitly: false, 0, issAndSer)))
	{
	}

	public SmimeEncryptionKeyPreferenceAttribute(RecipientKeyIdentifier rKeyID)
		: base(SmimeAttributes.EncrypKeyPref, new DerSet(new DerTaggedObject(explicitly: false, 1, rKeyID)))
	{
	}

	public SmimeEncryptionKeyPreferenceAttribute(Asn1OctetString sKeyID)
		: base(SmimeAttributes.EncrypKeyPref, new DerSet(new DerTaggedObject(explicitly: false, 2, sKeyID)))
	{
	}
}
