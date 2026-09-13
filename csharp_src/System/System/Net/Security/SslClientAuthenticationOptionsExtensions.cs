namespace System.Net.Security;

internal static class SslClientAuthenticationOptionsExtensions
{
	public static SslClientAuthenticationOptions ShallowClone(this SslClientAuthenticationOptions options)
	{
		return new SslClientAuthenticationOptions
		{
			AllowRenegotiation = options.AllowRenegotiation,
			ApplicationProtocols = options.ApplicationProtocols,
			CertificateRevocationCheckMode = options.CertificateRevocationCheckMode,
			ClientCertificates = options.ClientCertificates,
			EnabledSslProtocols = options.EnabledSslProtocols,
			EncryptionPolicy = options.EncryptionPolicy,
			LocalCertificateSelectionCallback = options.LocalCertificateSelectionCallback,
			RemoteCertificateValidationCallback = options.RemoteCertificateValidationCallback,
			TargetHost = options.TargetHost
		};
	}
}
