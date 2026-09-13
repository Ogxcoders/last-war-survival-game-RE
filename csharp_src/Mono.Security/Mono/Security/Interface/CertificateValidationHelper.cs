using Mono.Net.Security;

namespace Mono.Security.Interface;

public static class CertificateValidationHelper
{
	private const string SecurityLibrary = "/System/Library/Frameworks/Security.framework/Security";

	private static readonly bool noX509Chain;

	private static readonly bool supportsTrustAnchors;

	public static bool SupportsX509Chain => !noX509Chain;

	public static bool SupportsTrustAnchors => supportsTrustAnchors;

	static CertificateValidationHelper()
	{
		noX509Chain = true;
		supportsTrustAnchors = false;
	}

	public static ICertificateValidator GetValidator(MonoTlsSettings settings)
	{
		return (ICertificateValidator)NoReflectionHelper.GetDefaultValidator(settings);
	}
}
