using System.Security.Authentication;

namespace System.Net;

internal static class SecurityProtocol
{
	public const SslProtocols DefaultSecurityProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12;

	public const SslProtocols SystemDefaultSecurityProtocols = SslProtocols.None;
}
