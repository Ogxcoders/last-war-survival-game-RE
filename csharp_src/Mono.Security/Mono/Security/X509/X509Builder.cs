using System;
using System.Globalization;
using System.Security.Cryptography;

namespace Mono.Security.X509;

public abstract class X509Builder
{
	private const string defaultHash = "SHA1";

	private string hashName;

	public string Hash
	{
		get
		{
			return hashName;
		}
		set
		{
			if (hashName == null)
			{
				hashName = "SHA1";
			}
			else
			{
				hashName = value;
			}
		}
	}

	protected X509Builder()
	{
		hashName = "SHA1";
	}

	protected abstract ASN1 ToBeSigned(string hashName);

	protected string GetOid(string hashName)
	{
		return hashName.ToLower(CultureInfo.InvariantCulture) switch
		{
			"md2" => "1.2.840.113549.1.1.2", 
			"md4" => "1.2.840.113549.1.1.3", 
			"md5" => "1.2.840.113549.1.1.4", 
			"sha1" => "1.2.840.113549.1.1.5", 
			"sha256" => "1.2.840.113549.1.1.11", 
			"sha384" => "1.2.840.113549.1.1.12", 
			"sha512" => "1.2.840.113549.1.1.13", 
			_ => throw new NotSupportedException("Unknown hash algorithm " + hashName), 
		};
	}

	public virtual byte[] Sign(AsymmetricAlgorithm aa)
	{
		if (aa is RSA)
		{
			return Sign(aa as RSA);
		}
		if (aa is DSA)
		{
			return Sign(aa as DSA);
		}
		throw new NotSupportedException("Unknown Asymmetric Algorithm " + aa.ToString());
	}

	private byte[] Build(ASN1 tbs, string hashoid, byte[] signature)
	{
		ASN1 aSN = new ASN1(48);
		aSN.Add(tbs);
		aSN.Add(PKCS7.AlgorithmIdentifier(hashoid));
		byte[] array = new byte[signature.Length + 1];
		Buffer.BlockCopy(signature, 0, array, 1, signature.Length);
		aSN.Add(new ASN1(3, array));
		return aSN.GetBytes();
	}

	public virtual byte[] Sign(RSA key)
	{
		string oid = GetOid(hashName);
		ASN1 aSN = ToBeSigned(oid);
		byte[] rgbHash = HashAlgorithm.Create(hashName).ComputeHash(aSN.GetBytes());
		RSAPKCS1SignatureFormatter rSAPKCS1SignatureFormatter = new RSAPKCS1SignatureFormatter(key);
		rSAPKCS1SignatureFormatter.SetHashAlgorithm(hashName);
		byte[] signature = rSAPKCS1SignatureFormatter.CreateSignature(rgbHash);
		return Build(aSN, oid, signature);
	}

	public virtual byte[] Sign(DSA key)
	{
		string hashoid = "1.2.840.10040.4.3";
		ASN1 aSN = ToBeSigned(hashoid);
		HashAlgorithm hashAlgorithm = HashAlgorithm.Create(hashName);
		if (!(hashAlgorithm is SHA1))
		{
			throw new NotSupportedException("Only SHA-1 is supported for DSA");
		}
		byte[] rgbHash = hashAlgorithm.ComputeHash(aSN.GetBytes());
		DSASignatureFormatter dSASignatureFormatter = new DSASignatureFormatter(key);
		dSASignatureFormatter.SetHashAlgorithm(hashName);
		byte[] src = dSASignatureFormatter.CreateSignature(rgbHash);
		byte[] array = new byte[20];
		Buffer.BlockCopy(src, 0, array, 0, 20);
		byte[] array2 = new byte[20];
		Buffer.BlockCopy(src, 20, array2, 0, 20);
		ASN1 aSN2 = new ASN1(48);
		aSN2.Add(new ASN1(2, array));
		aSN2.Add(new ASN1(2, array2));
		return Build(aSN, hashoid, aSN2.GetBytes());
	}
}
