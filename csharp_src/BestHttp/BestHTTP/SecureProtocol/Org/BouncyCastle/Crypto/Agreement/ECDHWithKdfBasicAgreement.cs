using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement.Kdf;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement;

public class ECDHWithKdfBasicAgreement : ECDHBasicAgreement
{
	private readonly string algorithm;

	private readonly IDerivationFunction kdf;

	public ECDHWithKdfBasicAgreement(string algorithm, IDerivationFunction kdf)
	{
		if (algorithm == null)
		{
			throw new ArgumentNullException("algorithm");
		}
		if (kdf == null)
		{
			throw new ArgumentNullException("kdf");
		}
		this.algorithm = algorithm;
		this.kdf = kdf;
	}

	public override BigInteger CalculateAgreement(ICipherParameters pubKey)
	{
		BigInteger r = base.CalculateAgreement(pubKey);
		int defaultKeySize = GeneratorUtilities.GetDefaultKeySize(algorithm);
		DHKdfParameters parameters = new DHKdfParameters(new DerObjectIdentifier(algorithm), defaultKeySize, BigIntToBytes(r));
		kdf.Init(parameters);
		byte[] array = new byte[defaultKeySize / 8];
		kdf.GenerateBytes(array, 0, array.Length);
		return new BigInteger(1, array);
	}

	private byte[] BigIntToBytes(BigInteger r)
	{
		int byteLength = X9IntegerConverter.GetByteLength(privKey.Parameters.Curve);
		return X9IntegerConverter.IntegerToBytes(r, byteLength);
	}
}
