using System;

namespace Mono.Security.Interface;

[Flags]
public enum MonoSslPolicyErrors
{
	None = 0,
	RemoteCertificateNotAvailable = 1,
	RemoteCertificateNameMismatch = 2,
	RemoteCertificateChainErrors = 4
}
