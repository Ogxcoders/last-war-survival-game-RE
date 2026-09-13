namespace System.Security.Cryptography.X509Certificates;

public abstract class X509SignatureGenerator
{
	public PublicKey PublicKey
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	protected X509SignatureGenerator()
	{
		throw new PlatformNotSupportedException();
	}

	protected abstract PublicKey BuildPublicKey();

	public static X509SignatureGenerator CreateForECDsa(ECDsa key)
	{
		throw new PlatformNotSupportedException();
	}

	public static X509SignatureGenerator CreateForRSA(RSA key, RSASignaturePadding signaturePadding)
	{
		throw new PlatformNotSupportedException();
	}

	public abstract byte[] GetSignatureAlgorithmIdentifier(HashAlgorithmName hashAlgorithm);

	public abstract byte[] SignData(byte[] data, HashAlgorithmName hashAlgorithm);
}
