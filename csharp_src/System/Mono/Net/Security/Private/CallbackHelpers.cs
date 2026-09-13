using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Mono.Security.Interface;

namespace Mono.Net.Security.Private;

internal static class CallbackHelpers
{
	internal static MonoRemoteCertificateValidationCallback PublicToMono(RemoteCertificateValidationCallback callback)
	{
		if (callback == null)
		{
			return null;
		}
		return (string h, X509Certificate c, X509Chain ch, MonoSslPolicyErrors e) => callback(h, c, ch, (SslPolicyErrors)e);
	}

	internal static MonoRemoteCertificateValidationCallback InternalToMono(RemoteCertValidationCallback callback)
	{
		if (callback == null)
		{
			return null;
		}
		return (string h, X509Certificate c, X509Chain ch, MonoSslPolicyErrors e) => callback(h, c, ch, (SslPolicyErrors)e);
	}

	internal static RemoteCertificateValidationCallback InternalToPublic(string hostname, RemoteCertValidationCallback callback)
	{
		if (callback == null)
		{
			return null;
		}
		return (object s, X509Certificate c, X509Chain ch, SslPolicyErrors e) => callback(hostname, c, ch, e);
	}

	internal static MonoLocalCertificateSelectionCallback InternalToMono(LocalCertSelectionCallback callback)
	{
		if (callback == null)
		{
			return null;
		}
		return (string t, X509CertificateCollection lc, X509Certificate rc, string[] ai) => callback(t, lc, rc, ai);
	}

	internal static LocalCertificateSelectionCallback MonoToPublic(MonoLocalCertificateSelectionCallback callback)
	{
		if (callback == null)
		{
			return null;
		}
		return (object s, string t, X509CertificateCollection lc, X509Certificate rc, string[] ai) => callback(t, lc, rc, ai);
	}

	internal static RemoteCertValidationCallback MonoToInternal(MonoRemoteCertificateValidationCallback callback)
	{
		if (callback == null)
		{
			return null;
		}
		return (string h, X509Certificate c, X509Chain ch, SslPolicyErrors e) => callback(h, c, ch, (MonoSslPolicyErrors)e);
	}

	internal static LocalCertSelectionCallback MonoToInternal(MonoLocalCertificateSelectionCallback callback)
	{
		if (callback == null)
		{
			return null;
		}
		return (string t, X509CertificateCollection lc, X509Certificate rc, string[] ai) => callback(t, lc, rc, ai);
	}

	internal static ServerCertificateSelectionCallback MonoToPublic(MonoServerCertificateSelectionCallback callback)
	{
		if (callback == null)
		{
			return null;
		}
		return (object s, string h) => callback(s, h);
	}

	internal static MonoServerCertificateSelectionCallback PublicToMono(ServerCertificateSelectionCallback callback)
	{
		if (callback == null)
		{
			return null;
		}
		return (object s, string h) => callback(s, h);
	}
}
