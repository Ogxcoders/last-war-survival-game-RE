using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Mono.Security.Interface;

internal interface IMonoAuthenticationOptions
{
	bool AllowRenegotiation { get; set; }

	RemoteCertificateValidationCallback RemoteCertificateValidationCallback { get; set; }

	SslProtocols EnabledSslProtocols { get; set; }

	EncryptionPolicy EncryptionPolicy { get; set; }

	X509RevocationMode CertificateRevocationCheckMode { get; set; }
}
