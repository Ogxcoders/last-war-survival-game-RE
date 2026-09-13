using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Ocsp;

public class RespID
{
	internal readonly ResponderID id;

	public RespID(ResponderID id)
	{
		this.id = id;
	}

	public RespID(X509Name name)
	{
		id = new ResponderID(name);
	}

	public RespID(AsymmetricKeyParameter publicKey)
	{
		try
		{
			SubjectPublicKeyInfo subjectPublicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey);
			byte[] str = DigestUtilities.CalculateDigest("SHA1", subjectPublicKeyInfo.PublicKeyData.GetBytes());
			id = new ResponderID(new DerOctetString(str));
		}
		catch (Exception ex)
		{
			throw new OcspException("problem creating ID: " + ex, ex);
		}
	}

	public ResponderID ToAsn1Object()
	{
		return id;
	}

	public override bool Equals(object obj)
	{
		if (obj == this)
		{
			return true;
		}
		if (!(obj is RespID respID))
		{
			return false;
		}
		return id.Equals(respID.id);
	}

	public override int GetHashCode()
	{
		return id.GetHashCode();
	}
}
