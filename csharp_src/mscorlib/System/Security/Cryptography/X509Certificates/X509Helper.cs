using System.Text;
using Microsoft.Win32.SafeHandles;
using Mono;

namespace System.Security.Cryptography.X509Certificates;

internal static class X509Helper
{
	private static ISystemCertificateProvider CertificateProvider => DependencyInjector.SystemProvider.CertificateProvider;

	public static X509CertificateImpl InitFromCertificate(X509Certificate cert)
	{
		return CertificateProvider.Import(cert);
	}

	public static X509CertificateImpl InitFromCertificate(X509CertificateImpl impl)
	{
		return impl?.Clone();
	}

	public static bool IsValid(X509CertificateImpl impl)
	{
		return impl?.IsValid ?? false;
	}

	internal static void ThrowIfContextInvalid(X509CertificateImpl impl)
	{
		if (!IsValid(impl))
		{
			throw GetInvalidContextException();
		}
	}

	internal static Exception GetInvalidContextException()
	{
		return new CryptographicException(Locale.GetText("Certificate instance is empty."));
	}

	public static X509CertificateImpl Import(byte[] rawData)
	{
		return CertificateProvider.Import(rawData);
	}

	public static X509CertificateImpl Import(byte[] rawData, SafePasswordHandle password, X509KeyStorageFlags keyStorageFlags)
	{
		return CertificateProvider.Import(rawData, password, keyStorageFlags);
	}

	public static byte[] Export(X509CertificateImpl impl, X509ContentType contentType, SafePasswordHandle password)
	{
		ThrowIfContextInvalid(impl);
		return impl.Export(contentType, password);
	}

	public static bool Equals(X509CertificateImpl first, X509CertificateImpl second)
	{
		if (!IsValid(first) || !IsValid(second))
		{
			return false;
		}
		if (first.Equals(second, out var result))
		{
			return result;
		}
		byte[] rawData = first.RawData;
		byte[] rawData2 = second.RawData;
		if (rawData == null)
		{
			return rawData2 == null;
		}
		if (rawData2 == null)
		{
			return false;
		}
		if (rawData.Length != rawData2.Length)
		{
			return false;
		}
		for (int i = 0; i < rawData.Length; i++)
		{
			if (rawData[i] != rawData2[i])
			{
				return false;
			}
		}
		return true;
	}

	public static string ToHexString(byte[] data)
	{
		if (data != null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < data.Length; i++)
			{
				stringBuilder.Append(data[i].ToString("X2"));
			}
			return stringBuilder.ToString();
		}
		return null;
	}
}
