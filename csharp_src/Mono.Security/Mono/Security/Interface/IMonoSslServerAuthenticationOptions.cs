using System.Security.Cryptography.X509Certificates;

namespace Mono.Security.Interface;

internal interface IMonoSslServerAuthenticationOptions : IMonoAuthenticationOptions
{
	bool ClientCertificateRequired { get; set; }

	MonoServerCertificateSelectionCallback ServerCertificateSelectionCallback { get; set; }

	X509Certificate ServerCertificate { get; set; }
}
