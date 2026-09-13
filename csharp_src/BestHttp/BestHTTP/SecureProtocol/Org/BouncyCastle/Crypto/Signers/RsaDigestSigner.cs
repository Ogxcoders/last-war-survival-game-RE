using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.TeleTrust;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Encodings;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers;

public class RsaDigestSigner : ISigner
{
	private readonly IAsymmetricBlockCipher rsaEngine;

	private readonly AlgorithmIdentifier algId;

	private readonly IDigest digest;

	private bool forSigning;

	private static readonly IDictionary oidMap;

	public virtual string AlgorithmName => digest.AlgorithmName + "withRSA";

	static RsaDigestSigner()
	{
		oidMap = Platform.CreateHashtable();
		oidMap["RIPEMD128"] = TeleTrusTObjectIdentifiers.RipeMD128;
		oidMap["RIPEMD160"] = TeleTrusTObjectIdentifiers.RipeMD160;
		oidMap["RIPEMD256"] = TeleTrusTObjectIdentifiers.RipeMD256;
		oidMap["SHA-1"] = X509ObjectIdentifiers.IdSha1;
		oidMap["SHA-224"] = NistObjectIdentifiers.IdSha224;
		oidMap["SHA-256"] = NistObjectIdentifiers.IdSha256;
		oidMap["SHA-384"] = NistObjectIdentifiers.IdSha384;
		oidMap["SHA-512"] = NistObjectIdentifiers.IdSha512;
		oidMap["MD2"] = PkcsObjectIdentifiers.MD2;
		oidMap["MD4"] = PkcsObjectIdentifiers.MD4;
		oidMap["MD5"] = PkcsObjectIdentifiers.MD5;
	}

	public RsaDigestSigner(IDigest digest)
		: this(digest, (DerObjectIdentifier)oidMap[digest.AlgorithmName])
	{
	}

	public RsaDigestSigner(IDigest digest, DerObjectIdentifier digestOid)
		: this(digest, new AlgorithmIdentifier(digestOid, DerNull.Instance))
	{
	}

	public RsaDigestSigner(IDigest digest, AlgorithmIdentifier algId)
		: this(new RsaCoreEngine(), digest, algId)
	{
	}

	public RsaDigestSigner(IRsa rsa, IDigest digest, DerObjectIdentifier digestOid)
		: this(rsa, digest, new AlgorithmIdentifier(digestOid, DerNull.Instance))
	{
	}

	public RsaDigestSigner(IRsa rsa, IDigest digest, AlgorithmIdentifier algId)
		: this(new RsaBlindedEngine(rsa), digest, algId)
	{
	}

	public RsaDigestSigner(IAsymmetricBlockCipher rsaEngine, IDigest digest, AlgorithmIdentifier algId)
	{
		this.rsaEngine = new Pkcs1Encoding(rsaEngine);
		this.digest = digest;
		this.algId = algId;
	}

	public virtual void Init(bool forSigning, ICipherParameters parameters)
	{
		this.forSigning = forSigning;
		AsymmetricKeyParameter asymmetricKeyParameter = ((!(parameters is ParametersWithRandom)) ? ((AsymmetricKeyParameter)parameters) : ((AsymmetricKeyParameter)((ParametersWithRandom)parameters).Parameters));
		if (forSigning && !asymmetricKeyParameter.IsPrivate)
		{
			throw new InvalidKeyException("Signing requires private key.");
		}
		if (!forSigning && asymmetricKeyParameter.IsPrivate)
		{
			throw new InvalidKeyException("Verification requires public key.");
		}
		Reset();
		rsaEngine.Init(forSigning, parameters);
	}

	public virtual void Update(byte input)
	{
		digest.Update(input);
	}

	public virtual void BlockUpdate(byte[] input, int inOff, int length)
	{
		digest.BlockUpdate(input, inOff, length);
	}

	public virtual byte[] GenerateSignature()
	{
		if (!forSigning)
		{
			throw new InvalidOperationException("RsaDigestSigner not initialised for signature generation.");
		}
		byte[] array = new byte[digest.GetDigestSize()];
		digest.DoFinal(array, 0);
		byte[] array2 = DerEncode(array);
		return rsaEngine.ProcessBlock(array2, 0, array2.Length);
	}

	public virtual bool VerifySignature(byte[] signature)
	{
		if (forSigning)
		{
			throw new InvalidOperationException("RsaDigestSigner not initialised for verification");
		}
		byte[] array = new byte[digest.GetDigestSize()];
		digest.DoFinal(array, 0);
		byte[] array2;
		byte[] array3;
		try
		{
			array2 = rsaEngine.ProcessBlock(signature, 0, signature.Length);
			array3 = DerEncode(array);
		}
		catch (Exception)
		{
			return false;
		}
		if (array2.Length == array3.Length)
		{
			return Arrays.ConstantTimeAreEqual(array2, array3);
		}
		if (array2.Length == array3.Length - 2)
		{
			int num = array2.Length - array.Length - 2;
			int num2 = array3.Length - array.Length - 2;
			array3[1] -= 2;
			array3[3] -= 2;
			int num3 = 0;
			for (int i = 0; i < array.Length; i++)
			{
				num3 |= array2[num + i] ^ array3[num2 + i];
			}
			for (int j = 0; j < num; j++)
			{
				num3 |= array2[j] ^ array3[j];
			}
			return num3 == 0;
		}
		return false;
	}

	public virtual void Reset()
	{
		digest.Reset();
	}

	private byte[] DerEncode(byte[] hash)
	{
		if (algId == null)
		{
			return hash;
		}
		return new DigestInfo(algId, hash).GetDerEncoded();
	}
}
