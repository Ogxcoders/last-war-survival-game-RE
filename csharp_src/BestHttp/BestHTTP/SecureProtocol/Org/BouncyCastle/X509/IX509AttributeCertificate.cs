using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

public interface IX509AttributeCertificate : IX509Extension
{
	int Version { get; }

	BigInteger SerialNumber { get; }

	DateTime NotBefore { get; }

	DateTime NotAfter { get; }

	AttributeCertificateHolder Holder { get; }

	AttributeCertificateIssuer Issuer { get; }

	bool IsValidNow { get; }

	X509Attribute[] GetAttributes();

	X509Attribute[] GetAttributes(string oid);

	bool[] GetIssuerUniqueID();

	bool IsValid(DateTime date);

	void CheckValidity();

	void CheckValidity(DateTime date);

	byte[] GetSignature();

	void Verify(AsymmetricKeyParameter publicKey);

	byte[] GetEncoded();
}
