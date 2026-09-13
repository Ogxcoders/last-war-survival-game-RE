using System.Collections.ObjectModel;

namespace System.Security.Cryptography.X509Certificates;

public sealed class CertificateRequest
{
	public Collection<X509Extension> CertificateExtensions
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public HashAlgorithmName HashAlgorithm
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public PublicKey PublicKey
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public X500DistinguishedName SubjectName
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public CertificateRequest(X500DistinguishedName subjectName, ECDsa key, HashAlgorithmName hashAlgorithm)
	{
		throw new PlatformNotSupportedException();
	}

	public CertificateRequest(X500DistinguishedName subjectName, RSA key, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		throw new PlatformNotSupportedException();
	}

	public CertificateRequest(X500DistinguishedName subjectName, PublicKey publicKey, HashAlgorithmName hashAlgorithm)
	{
		throw new PlatformNotSupportedException();
	}

	public CertificateRequest(string subjectName, ECDsa key, HashAlgorithmName hashAlgorithm)
	{
		throw new PlatformNotSupportedException();
	}

	public CertificateRequest(string subjectName, RSA key, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		throw new PlatformNotSupportedException();
	}

	public X509Certificate2 Create(X500DistinguishedName issuerName, X509SignatureGenerator generator, DateTimeOffset notBefore, DateTimeOffset notAfter, byte[] serialNumber)
	{
		throw new PlatformNotSupportedException();
	}

	public X509Certificate2 Create(X509Certificate2 issuerCertificate, DateTimeOffset notBefore, DateTimeOffset notAfter, byte[] serialNumber)
	{
		throw new PlatformNotSupportedException();
	}

	public X509Certificate2 CreateSelfSigned(DateTimeOffset notBefore, DateTimeOffset notAfter)
	{
		throw new PlatformNotSupportedException();
	}

	public byte[] CreateSigningRequest()
	{
		throw new PlatformNotSupportedException();
	}

	public byte[] CreateSigningRequest(X509SignatureGenerator signatureGenerator)
	{
		throw new PlatformNotSupportedException();
	}
}
