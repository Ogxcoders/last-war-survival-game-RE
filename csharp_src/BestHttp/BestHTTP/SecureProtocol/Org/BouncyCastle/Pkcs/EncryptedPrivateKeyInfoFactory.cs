using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkcs;

public sealed class EncryptedPrivateKeyInfoFactory
{
	private EncryptedPrivateKeyInfoFactory()
	{
	}

	public static EncryptedPrivateKeyInfo CreateEncryptedPrivateKeyInfo(DerObjectIdentifier algorithm, char[] passPhrase, byte[] salt, int iterationCount, AsymmetricKeyParameter key)
	{
		return CreateEncryptedPrivateKeyInfo(algorithm.Id, passPhrase, salt, iterationCount, PrivateKeyInfoFactory.CreatePrivateKeyInfo(key));
	}

	public static EncryptedPrivateKeyInfo CreateEncryptedPrivateKeyInfo(string algorithm, char[] passPhrase, byte[] salt, int iterationCount, AsymmetricKeyParameter key)
	{
		return CreateEncryptedPrivateKeyInfo(algorithm, passPhrase, salt, iterationCount, PrivateKeyInfoFactory.CreatePrivateKeyInfo(key));
	}

	public static EncryptedPrivateKeyInfo CreateEncryptedPrivateKeyInfo(string algorithm, char[] passPhrase, byte[] salt, int iterationCount, PrivateKeyInfo keyInfo)
	{
		IBufferedCipher obj = (PbeUtilities.CreateEngine(algorithm) as IBufferedCipher) ?? throw new Exception("Unknown encryption algorithm: " + algorithm);
		Asn1Encodable asn1Encodable = PbeUtilities.GenerateAlgorithmParameters(algorithm, salt, iterationCount);
		ICipherParameters parameters = PbeUtilities.GenerateCipherParameters(algorithm, passPhrase, asn1Encodable);
		obj.Init(forEncryption: true, parameters);
		byte[] encoding = obj.DoFinal(keyInfo.GetEncoded());
		return new EncryptedPrivateKeyInfo(new AlgorithmIdentifier(PbeUtilities.GetObjectIdentifier(algorithm), asn1Encodable), encoding);
	}
}
