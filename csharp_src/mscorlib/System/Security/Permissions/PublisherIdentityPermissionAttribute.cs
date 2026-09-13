using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Mono.Security.Cryptography;

namespace System.Security.Permissions;

[Serializable]
[ComVisible(true)]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class PublisherIdentityPermissionAttribute : CodeAccessSecurityAttribute
{
	private string certFile;

	private string signedFile;

	private string x509data;

	public string CertFile
	{
		get
		{
			return certFile;
		}
		set
		{
			certFile = value;
		}
	}

	public string SignedFile
	{
		get
		{
			return signedFile;
		}
		set
		{
			signedFile = value;
		}
	}

	public string X509Certificate
	{
		get
		{
			return x509data;
		}
		set
		{
			x509data = value;
		}
	}

	public PublisherIdentityPermissionAttribute(SecurityAction action)
		: base(action)
	{
	}

	public override IPermission CreatePermission()
	{
		if (base.Unrestricted)
		{
			return new PublisherIdentityPermission(PermissionState.Unrestricted);
		}
		if (x509data != null)
		{
			return new PublisherIdentityPermission(new X509Certificate(CryptoConvert.FromHex(x509data)));
		}
		if (certFile != null)
		{
			return new PublisherIdentityPermission(System.Security.Cryptography.X509Certificates.X509Certificate.CreateFromCertFile(certFile));
		}
		if (signedFile != null)
		{
			return new PublisherIdentityPermission(System.Security.Cryptography.X509Certificates.X509Certificate.CreateFromSignedFile(signedFile));
		}
		return new PublisherIdentityPermission(PermissionState.None);
	}
}
