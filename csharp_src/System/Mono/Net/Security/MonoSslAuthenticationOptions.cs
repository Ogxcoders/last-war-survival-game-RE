using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Mono.Security.Interface;

namespace Mono.Net.Security;

internal abstract class MonoSslAuthenticationOptions : IMonoAuthenticationOptions
{
	public abstract bool ServerMode { get; }

	public abstract bool AllowRenegotiation { get; set; }

	public abstract RemoteCertificateValidationCallback RemoteCertificateValidationCallback { get; set; }

	public abstract SslProtocols EnabledSslProtocols { get; set; }

	public abstract EncryptionPolicy EncryptionPolicy { get; set; }

	public abstract X509RevocationMode CertificateRevocationCheckMode { get; set; }

	public abstract string TargetHost { get; set; }

	public abstract X509Certificate ServerCertificate { get; set; }

	public abstract X509CertificateCollection ClientCertificates { get; set; }

	public abstract bool ClientCertificateRequired { get; set; }

	internal ServerCertSelectionCallback ServerCertSelectionDelegate { get; set; }
}
