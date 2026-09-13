using System.Collections.Generic;
using System.Text;

namespace System.Security.Cryptography.X509Certificates;

internal abstract class X509Certificate2Impl : X509CertificateImpl
{
	public abstract bool Archived { get; set; }

	public abstract IEnumerable<X509Extension> Extensions { get; }

	public abstract string FriendlyName { get; set; }

	public abstract X500DistinguishedName IssuerName { get; }

	public abstract AsymmetricAlgorithm PrivateKey { get; set; }

	public abstract PublicKey PublicKey { get; }

	public abstract string SignatureAlgorithm { get; }

	public abstract X500DistinguishedName SubjectName { get; }

	public abstract int Version { get; }

	internal abstract X509CertificateImplCollection IntermediateCertificates { get; }

	internal abstract X509Certificate2Impl FallbackImpl { get; }

	public abstract string GetNameInfo(X509NameType nameType, bool forIssuer);

	public abstract bool Verify(X509Certificate2 thisCertificate);

	public abstract void AppendPrivateKeyInfo(StringBuilder sb);

	public sealed override X509CertificateImpl CopyWithPrivateKey(RSA privateKey)
	{
		X509Certificate2Impl obj = (X509Certificate2Impl)Clone();
		obj.PrivateKey = privateKey;
		return obj;
	}

	public sealed override X509Certificate CreateCertificate()
	{
		return new X509Certificate2(this);
	}

	public abstract void Reset();
}
