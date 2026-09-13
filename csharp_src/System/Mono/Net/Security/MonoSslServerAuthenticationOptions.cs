using System;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Mono.Net.Security.Private;
using Mono.Security.Interface;

namespace Mono.Net.Security;

internal sealed class MonoSslServerAuthenticationOptions : MonoSslAuthenticationOptions, IMonoSslServerAuthenticationOptions, IMonoAuthenticationOptions
{
	public SslServerAuthenticationOptions Options { get; }

	public override bool ServerMode => true;

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

	public override bool ClientCertificateRequired
	{
		get
		{
			return Options.ClientCertificateRequired;
		}
		set
		{
			Options.ClientCertificateRequired = value;
		}
	}

	public ServerCertificateSelectionCallback ServerCertificateSelectionCallback
	{
		get
		{
			return Options.ServerCertificateSelectionCallback;
		}
		set
		{
			Options.ServerCertificateSelectionCallback = value;
		}
	}

	MonoServerCertificateSelectionCallback IMonoSslServerAuthenticationOptions.ServerCertificateSelectionCallback
	{
		get
		{
			return CallbackHelpers.PublicToMono(ServerCertificateSelectionCallback);
		}
		set
		{
			ServerCertificateSelectionCallback = CallbackHelpers.MonoToPublic(value);
		}
	}

	public override string TargetHost
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

	public override X509Certificate ServerCertificate
	{
		get
		{
			return Options.ServerCertificate;
		}
		set
		{
			Options.ServerCertificate = value;
		}
	}

	public override X509CertificateCollection ClientCertificates
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

	public MonoSslServerAuthenticationOptions(SslServerAuthenticationOptions options)
	{
		Options = options;
	}

	public MonoSslServerAuthenticationOptions()
	{
		Options = new SslServerAuthenticationOptions();
	}
}
