using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.EdEC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Oiw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Sec;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkcs;

public sealed class PrivateKeyInfoFactory
{
	private PrivateKeyInfoFactory()
	{
	}

	public static PrivateKeyInfo CreatePrivateKeyInfo(AsymmetricKeyParameter privateKey)
	{
		return CreatePrivateKeyInfo(privateKey, null);
	}

	public static PrivateKeyInfo CreatePrivateKeyInfo(AsymmetricKeyParameter privateKey, Asn1Set attributes)
	{
		if (privateKey == null)
		{
			throw new ArgumentNullException("privateKey");
		}
		if (!privateKey.IsPrivate)
		{
			throw new ArgumentException("Public key passed - private key expected", "privateKey");
		}
		if (privateKey is ElGamalPrivateKeyParameters)
		{
			ElGamalPrivateKeyParameters elGamalPrivateKeyParameters = (ElGamalPrivateKeyParameters)privateKey;
			ElGamalParameters parameters = elGamalPrivateKeyParameters.Parameters;
			return new PrivateKeyInfo(new AlgorithmIdentifier(OiwObjectIdentifiers.ElGamalAlgorithm, new ElGamalParameter(parameters.P, parameters.G).ToAsn1Object()), new DerInteger(elGamalPrivateKeyParameters.X), attributes);
		}
		if (privateKey is DsaPrivateKeyParameters)
		{
			DsaPrivateKeyParameters dsaPrivateKeyParameters = (DsaPrivateKeyParameters)privateKey;
			DsaParameters parameters2 = dsaPrivateKeyParameters.Parameters;
			return new PrivateKeyInfo(new AlgorithmIdentifier(X9ObjectIdentifiers.IdDsa, new DsaParameter(parameters2.P, parameters2.Q, parameters2.G).ToAsn1Object()), new DerInteger(dsaPrivateKeyParameters.X), attributes);
		}
		if (privateKey is DHPrivateKeyParameters)
		{
			DHPrivateKeyParameters dHPrivateKeyParameters = (DHPrivateKeyParameters)privateKey;
			DHParameter dHParameter = new DHParameter(dHPrivateKeyParameters.Parameters.P, dHPrivateKeyParameters.Parameters.G, dHPrivateKeyParameters.Parameters.L);
			return new PrivateKeyInfo(new AlgorithmIdentifier(dHPrivateKeyParameters.AlgorithmOid, dHParameter.ToAsn1Object()), new DerInteger(dHPrivateKeyParameters.X), attributes);
		}
		if (privateKey is RsaKeyParameters)
		{
			AlgorithmIdentifier privateKeyAlgorithm = new AlgorithmIdentifier(PkcsObjectIdentifiers.RsaEncryption, DerNull.Instance);
			RsaPrivateKeyStructure rsaPrivateKeyStructure;
			if (privateKey is RsaPrivateCrtKeyParameters)
			{
				RsaPrivateCrtKeyParameters rsaPrivateCrtKeyParameters = (RsaPrivateCrtKeyParameters)privateKey;
				rsaPrivateKeyStructure = new RsaPrivateKeyStructure(rsaPrivateCrtKeyParameters.Modulus, rsaPrivateCrtKeyParameters.PublicExponent, rsaPrivateCrtKeyParameters.Exponent, rsaPrivateCrtKeyParameters.P, rsaPrivateCrtKeyParameters.Q, rsaPrivateCrtKeyParameters.DP, rsaPrivateCrtKeyParameters.DQ, rsaPrivateCrtKeyParameters.QInv);
			}
			else
			{
				RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)privateKey;
				rsaPrivateKeyStructure = new RsaPrivateKeyStructure(rsaKeyParameters.Modulus, BigInteger.Zero, rsaKeyParameters.Exponent, BigInteger.Zero, BigInteger.Zero, BigInteger.Zero, BigInteger.Zero, BigInteger.Zero);
			}
			return new PrivateKeyInfo(privateKeyAlgorithm, rsaPrivateKeyStructure.ToAsn1Object(), attributes);
		}
		if (privateKey is ECPrivateKeyParameters)
		{
			ECPrivateKeyParameters eCPrivateKeyParameters = (ECPrivateKeyParameters)privateKey;
			DerBitString publicKey = new DerBitString(ECKeyPairGenerator.GetCorrespondingPublicKey(eCPrivateKeyParameters).Q.GetEncoded(compressed: false));
			ECDomainParameters parameters3 = eCPrivateKeyParameters.Parameters;
			int bitLength = parameters3.N.BitLength;
			AlgorithmIdentifier privateKeyAlgorithm2;
			ECPrivateKeyStructure privateKey2;
			if (eCPrivateKeyParameters.AlgorithmName == "ECGOST3410")
			{
				if (eCPrivateKeyParameters.PublicKeyParamSet == null)
				{
					throw Platform.CreateNotImplementedException("Not a CryptoPro parameter set");
				}
				Gost3410PublicKeyAlgParameters parameters4 = new Gost3410PublicKeyAlgParameters(eCPrivateKeyParameters.PublicKeyParamSet, CryptoProObjectIdentifiers.GostR3411x94CryptoProParamSet);
				privateKeyAlgorithm2 = new AlgorithmIdentifier(CryptoProObjectIdentifiers.GostR3410x2001, parameters4);
				privateKey2 = new ECPrivateKeyStructure(bitLength, eCPrivateKeyParameters.D, publicKey, null);
			}
			else
			{
				X962Parameters parameters5 = ((eCPrivateKeyParameters.PublicKeyParamSet != null) ? new X962Parameters(eCPrivateKeyParameters.PublicKeyParamSet) : new X962Parameters(new X9ECParameters(parameters3.Curve, parameters3.G, parameters3.N, parameters3.H, parameters3.GetSeed())));
				privateKey2 = new ECPrivateKeyStructure(bitLength, eCPrivateKeyParameters.D, publicKey, parameters5);
				privateKeyAlgorithm2 = new AlgorithmIdentifier(X9ObjectIdentifiers.IdECPublicKey, parameters5);
			}
			return new PrivateKeyInfo(privateKeyAlgorithm2, privateKey2, attributes);
		}
		if (privateKey is Gost3410PrivateKeyParameters)
		{
			Gost3410PrivateKeyParameters gost3410PrivateKeyParameters = (Gost3410PrivateKeyParameters)privateKey;
			if (gost3410PrivateKeyParameters.PublicKeyParamSet == null)
			{
				throw Platform.CreateNotImplementedException("Not a CryptoPro parameter set");
			}
			byte[] array = gost3410PrivateKeyParameters.X.ToByteArrayUnsigned();
			byte[] array2 = new byte[array.Length];
			for (int i = 0; i != array2.Length; i++)
			{
				array2[i] = array[array.Length - 1 - i];
			}
			Gost3410PublicKeyAlgParameters gost3410PublicKeyAlgParameters = new Gost3410PublicKeyAlgParameters(gost3410PrivateKeyParameters.PublicKeyParamSet, CryptoProObjectIdentifiers.GostR3411x94CryptoProParamSet, null);
			return new PrivateKeyInfo(new AlgorithmIdentifier(CryptoProObjectIdentifiers.GostR3410x94, gost3410PublicKeyAlgParameters.ToAsn1Object()), new DerOctetString(array2), attributes);
		}
		if (privateKey is X448PrivateKeyParameters)
		{
			X448PrivateKeyParameters x448PrivateKeyParameters = (X448PrivateKeyParameters)privateKey;
			return new PrivateKeyInfo(new AlgorithmIdentifier(EdECObjectIdentifiers.id_X448), new DerOctetString(x448PrivateKeyParameters.GetEncoded()), attributes, x448PrivateKeyParameters.GeneratePublicKey().GetEncoded());
		}
		if (privateKey is X25519PrivateKeyParameters)
		{
			X25519PrivateKeyParameters x25519PrivateKeyParameters = (X25519PrivateKeyParameters)privateKey;
			return new PrivateKeyInfo(new AlgorithmIdentifier(EdECObjectIdentifiers.id_X25519), new DerOctetString(x25519PrivateKeyParameters.GetEncoded()), attributes, x25519PrivateKeyParameters.GeneratePublicKey().GetEncoded());
		}
		if (privateKey is Ed448PrivateKeyParameters)
		{
			Ed448PrivateKeyParameters ed448PrivateKeyParameters = (Ed448PrivateKeyParameters)privateKey;
			return new PrivateKeyInfo(new AlgorithmIdentifier(EdECObjectIdentifiers.id_Ed448), new DerOctetString(ed448PrivateKeyParameters.GetEncoded()), attributes, ed448PrivateKeyParameters.GeneratePublicKey().GetEncoded());
		}
		if (privateKey is Ed25519PrivateKeyParameters)
		{
			Ed25519PrivateKeyParameters ed25519PrivateKeyParameters = (Ed25519PrivateKeyParameters)privateKey;
			return new PrivateKeyInfo(new AlgorithmIdentifier(EdECObjectIdentifiers.id_Ed25519), new DerOctetString(ed25519PrivateKeyParameters.GetEncoded()), attributes, ed25519PrivateKeyParameters.GeneratePublicKey().GetEncoded());
		}
		throw new ArgumentException("Class provided is not convertible: " + Platform.GetTypeName(privateKey));
	}

	public static PrivateKeyInfo CreatePrivateKeyInfo(char[] passPhrase, EncryptedPrivateKeyInfo encInfo)
	{
		return CreatePrivateKeyInfo(passPhrase, wrongPkcs12Zero: false, encInfo);
	}

	public static PrivateKeyInfo CreatePrivateKeyInfo(char[] passPhrase, bool wrongPkcs12Zero, EncryptedPrivateKeyInfo encInfo)
	{
		AlgorithmIdentifier encryptionAlgorithm = encInfo.EncryptionAlgorithm;
		IBufferedCipher obj = (PbeUtilities.CreateEngine(encryptionAlgorithm) as IBufferedCipher) ?? throw new Exception("Unknown encryption algorithm: " + encryptionAlgorithm.Algorithm);
		ICipherParameters parameters = PbeUtilities.GenerateCipherParameters(encryptionAlgorithm, passPhrase, wrongPkcs12Zero);
		obj.Init(forEncryption: false, parameters);
		return PrivateKeyInfo.GetInstance(obj.DoFinal(encInfo.GetEncryptedData()));
	}
}
