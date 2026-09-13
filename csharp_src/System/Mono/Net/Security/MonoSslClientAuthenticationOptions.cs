using System;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Mono.Security.Interface;

namespace Mono.Net.Security;

internal sealed class MonoSslClientAuthenticationOptions : MonoSslAuthenticationOptions, IMonoSslClientAuthenticationOptions, IMonoAuthenticationOptions
{
	public SslClientAuthenticationOptions Options { get; }

	public override bool ServerMode => false;

	public override bool AllowRenegotiation
	{
		get
		{
			return Options.AllowRenegotiation;
		}
		set
		{
			Options.AllowRenegotiation = value;
		}
	}

	public override RemoteCertificateValidationCallback RemoteCertificateValidationCallback
	{
		get
		{
			return Options.RemoteCertificateValidationCallback;
		}
		set
		{
			Options.RemoteCertificateValidationCallback = value;
		}
	}

	public override X509RevocationMode CertificateRevocationCheckMode
	{
		get
		{
			return Options.CertificateRevocationCheckMode;
		}
		set
		{
			Options.CertificateRevocationCheckMode = value;
		}
	}

	public override EncryptionPolicy EncryptionPolicy
	{
		get
		{
			return Options.EncryptionPolicy;
		}
		set
		{
			Options.EncryptionPolicy = value;
		}
	}

	public override SslProtocols EnabledSslProtocols
	{
		get
		{
			return Options.EnabledSslProtocols;
		}
		set
		{
			Options.EnabledSslProtocols = value;
		}
	}

	public LocalCertificateSelectionCallback LocalCertificateSelectionCallback
	{
		get
		{
			return Options.LocalCertificateSelectionCallback;
		}
		set
		{
			Options.LocalCertificateSelectionCallback = value;
		}
	}

	public override string TargetHost
	{
		get
		{
			return Options.TargetHost;
		}
		set
		{
			Options.TargetHost = value;
		}
	}

	public override bool ClientCertificateRequired
	{
		get
		{
			throw new NotSupportedException();
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public override X509CertificateCollection ClientCertificates
	{
		get
		{
			return Options.ClientCertificates;
		}
		set
		{
			Options.ClientCertificates = value;
		}
	}

	public override X509Certificate ServerCertificate
	{
		get
		{
			throw new NotSupportedException();
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public MonoSslClientAuthenticationOptions(SslClientAuthenticationOptions options)
	{
		Options = options;
	}

	public MonoSslClientAuthenticationOptions()
	{
		Options = new SslClientAuthenticationOptions();
	}
}
