using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace System.Net.Security;

internal class SslAuthenticationOptions
{
	internal bool AllowRenegotiation { get; set; }

	internal string TargetHost { get; set; }

	internal X509CertificateCollection ClientCertificates { get; set; }

	internal List<SslApplicationProtocol> ApplicationProtocols { get; }

	internal bool IsServer { get; set; }

	internal RemoteCertificateValidationCallback RemoteCertificateValidationCallback { get; set; }

	internal LocalCertificateSelectionCallback LocalCertificateSelectionCallback { get; set; }

	internal X509Certificate ServerCertificate { get; set; }

	internal SslProtocols EnabledSslProtocols { get; set; }

	internal X509RevocationMode CertificateRevocationCheckMode { get; set; }

	internal EncryptionPolicy EncryptionPolicy { get; set; }

	internal bool RemoteCertRequired { get; set; }

	internal bool CheckCertName { get; set; }

	internal RemoteCertValidationCallback CertValidationDelegate { get; set; }

	internal LocalCertSelectionCallback CertSelectionDelegate { get; set; }

	internal ServerCertSelectionCallback ServerCertSelectionDelegate { get; set; }

	internal SslAuthenticationOptions(SslClientAuthenticationOptions sslClientAuthenticationOptions, RemoteCertValidationCallback remoteCallback, LocalCertSelectionCallback localCallback)
	{
		AllowRenegotiation = sslClientAuthenticationOptions.AllowRenegotiation;
		ApplicationProtocols = sslClientAuthenticationOptions.ApplicationProtocols;
		CertValidationDelegate = remoteCallback;
		CheckCertName = true;
		EnabledSslProtocols = sslClientAuthenticationOptions.EnabledSslProtocols;
		EncryptionPolicy = sslClientAuthenticationOptions.EncryptionPolicy;
		IsServer = false;
		RemoteCertRequired = true;
		RemoteCertificateValidationCallback = sslClientAuthenticationOptions.RemoteCertificateValidationCallback;
		TargetHost = sslClientAuthenticationOptions.TargetHost;
		CertSelectionDelegate = localCallback;
		CertificateRevocationCheckMode = sslClientAuthenticationOptions.CertificateRevocationCheckMode;
		ClientCertificates = sslClientAuthenticationOptions.ClientCertificates;
		LocalCertificateSelectionCallback = sslClientAuthenticationOptions.LocalCertificateSelectionCallback;
	}

	internal SslAuthenticationOptions(SslServerAuthenticationOptions sslServerAuthenticationOptions)
	{
		AllowRenegotiation = sslServerAuthenticationOptions.AllowRenegotiation;
		ApplicationProtocols = sslServerAuthenticationOptions.ApplicationProtocols;
		CheckCertName = false;
		EnabledSslProtocols = sslServerAuthenticationOptions.EnabledSslProtocols;
		EncryptionPolicy = sslServerAuthenticationOptions.EncryptionPolicy;
		IsServer = true;
		RemoteCertRequired = sslServerAuthenticationOptions.ClientCertificateRequired;
		RemoteCertificateValidationCallback = sslServerAuthenticationOptions.RemoteCertificateValidationCallback;
		TargetHost = string.Empty;
		CertificateRevocationCheckMode = sslServerAuthenticationOptions.CertificateRevocationCheckMode;
		ServerCertificate = sslServerAuthenticationOptions.ServerCertificate;
	}
}
