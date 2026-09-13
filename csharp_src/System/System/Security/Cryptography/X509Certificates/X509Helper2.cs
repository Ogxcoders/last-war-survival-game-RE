using System.IO;
using Mono.Btls;
using Mono.Security.X509;

namespace System.Security.Cryptography.X509Certificates;

internal static class X509Helper2
{
	[System.MonoTODO("Investigate replacement; see comments in source.")]
	internal static Mono.Security.X509.X509Certificate GetMonoCertificate(X509Certificate2 certificate)
	{
		if (certificate.Impl is X509Certificate2ImplMono x509Certificate2ImplMono)
		{
			return x509Certificate2ImplMono.MonoCertificate;
		}
		return new Mono.Security.X509.X509Certificate(certificate.RawData);
	}

	internal static X509ChainImpl CreateChainImpl(bool useMachineContext)
	{
		return new X509ChainImplMono(useMachineContext);
	}

	public static bool IsValid(X509ChainImpl impl)
	{
		return impl?.IsValid ?? false;
	}

	internal static void ThrowIfContextInvalid(X509ChainImpl impl)
	{
		if (!IsValid(impl))
		{
			throw GetInvalidChainContextException();
		}
	}

	internal static Exception GetInvalidChainContextException()
	{
		return new CryptographicException(Locale.GetText("Chain instance is empty."));
	}

	[Obsolete("This is only used by Mono.Security's X509Store and will be replaced shortly.")]
	internal static long GetSubjectNameHash(X509Certificate certificate)
	{
		X509Helper.ThrowIfContextInvalid(certificate.Impl);
		using MonoBtlsX509 monoBtlsX = GetNativeInstance(certificate.Impl);
		using MonoBtlsX509Name monoBtlsX509Name = monoBtlsX.GetSubjectName();
		return monoBtlsX509Name.GetHash();
	}

	[Obsolete("This is only used by Mono.Security's X509Store and will be replaced shortly.")]
	internal static void ExportAsPEM(X509Certificate certificate, Stream stream, bool includeHumanReadableForm)
	{
		X509Helper.ThrowIfContextInvalid(certificate.Impl);
		using MonoBtlsX509 monoBtlsX = GetNativeInstance(certificate.Impl);
		using MonoBtlsBio bio = MonoBtlsBio.CreateMonoStream(stream);
		monoBtlsX.ExportAsPEM(bio, includeHumanReadableForm);
	}

	private static MonoBtlsX509 GetNativeInstance(X509CertificateImpl impl)
	{
		X509Helper.ThrowIfContextInvalid(impl);
		if (impl is X509CertificateImplBtls x509CertificateImplBtls)
		{
			return x509CertificateImplBtls.X509.Copy();
		}
		return MonoBtlsX509.LoadFromData(impl.RawData, MonoBtlsX509Format.DER);
	}
}
