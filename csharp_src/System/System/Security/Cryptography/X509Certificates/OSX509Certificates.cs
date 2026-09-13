using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates;

internal static class OSX509Certificates
{
	public enum SecTrustResult
	{
		Invalid,
		Proceed,
		Confirm,
		Deny,
		Unspecified,
		RecoverableTrustFailure,
		FatalTrustFailure,
		ResultOtherError
	}

	public const string SecurityLibrary = "/System/Library/Frameworks/Security.framework/Security";

	public const string CoreFoundationLibrary = "/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation";

	[DllImport("/System/Library/Frameworks/Security.framework/Security")]
	private static extern IntPtr SecCertificateCreateWithData(IntPtr allocator, IntPtr nsdataRef);

	[DllImport("/System/Library/Frameworks/Security.framework/Security")]
	private static extern int SecTrustCreateWithCertificates(IntPtr certOrCertArray, IntPtr policies, out IntPtr sectrustref);

	[DllImport("/System/Library/Frameworks/Security.framework/Security")]
	private static extern int SecTrustSetAnchorCertificates(IntPtr trust, IntPtr anchorCertificates);

	[DllImport("/System/Library/Frameworks/Security.framework/Security")]
	private static extern IntPtr SecPolicyCreateSSL([MarshalAs(UnmanagedType.I1)] bool server, IntPtr cfStringHostname);

	[DllImport("/System/Library/Frameworks/Security.framework/Security")]
	private static extern int SecTrustEvaluate(IntPtr secTrustRef, out SecTrustResult secTrustResultTime);

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation", CharSet = CharSet.Unicode)]
	private static extern IntPtr CFStringCreateWithCharacters(IntPtr allocator, string str, IntPtr count);

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private unsafe static extern IntPtr CFDataCreate(IntPtr allocator, byte* bytes, IntPtr length);

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern void CFRetain(IntPtr handle);

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern void CFRelease(IntPtr handle);

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern IntPtr CFArrayCreate(IntPtr allocator, IntPtr values, IntPtr numValues, IntPtr callbacks);

	private unsafe static IntPtr MakeCFData(byte[] data)
	{
		fixed (byte* bytes = &data[0])
		{
			return CFDataCreate(IntPtr.Zero, bytes, (IntPtr)data.Length);
		}
	}

	private unsafe static IntPtr FromIntPtrs(IntPtr[] values)
	{
		fixed (IntPtr* ptr = values)
		{
			return CFArrayCreate(IntPtr.Zero, (IntPtr)ptr, (IntPtr)values.Length, IntPtr.Zero);
		}
	}

	private static IntPtr GetCertificate(X509Certificate certificate)
	{
		IntPtr nativeAppleCertificate = certificate.Impl.GetNativeAppleCertificate();
		if (nativeAppleCertificate != IntPtr.Zero)
		{
			CFRetain(nativeAppleCertificate);
			return nativeAppleCertificate;
		}
		IntPtr intPtr = MakeCFData(certificate.GetRawCertData());
		nativeAppleCertificate = SecCertificateCreateWithData(IntPtr.Zero, intPtr);
		CFRelease(intPtr);
		return nativeAppleCertificate;
	}

	public static SecTrustResult TrustEvaluateSsl(X509CertificateCollection certificates, X509CertificateCollection anchors, string host)
	{
		if (certificates == null)
		{
			return SecTrustResult.Deny;
		}
		try
		{
			return _TrustEvaluateSsl(certificates, anchors, host);
		}
		catch
		{
			return SecTrustResult.Deny;
		}
	}

	private static SecTrustResult _TrustEvaluateSsl(X509CertificateCollection certificates, X509CertificateCollection anchors, string hostName)
	{
		int count = certificates.Count;
		int num = anchors?.Count ?? 0;
		IntPtr[] array = new IntPtr[count];
		IntPtr[] array2 = new IntPtr[num];
		IntPtr intPtr = IntPtr.Zero;
		IntPtr intPtr2 = IntPtr.Zero;
		IntPtr intPtr3 = IntPtr.Zero;
		IntPtr intPtr4 = IntPtr.Zero;
		IntPtr sectrustref = IntPtr.Zero;
		SecTrustResult secTrustResultTime = SecTrustResult.Deny;
		try
		{
			for (int i = 0; i < count; i++)
			{
				array[i] = GetCertificate(certificates[i]);
				if (array[i] == IntPtr.Zero)
				{
					return SecTrustResult.Deny;
				}
			}
			for (int j = 0; j < num; j++)
			{
				array2[j] = GetCertificate(anchors[j]);
				if (array2[j] == IntPtr.Zero)
				{
					return SecTrustResult.Deny;
				}
			}
			intPtr = FromIntPtrs(array);
			if (hostName != null)
			{
				intPtr4 = CFStringCreateWithCharacters(IntPtr.Zero, hostName, (IntPtr)hostName.Length);
			}
			intPtr3 = SecPolicyCreateSSL(server: true, intPtr4);
			if (SecTrustCreateWithCertificates(intPtr, intPtr3, out sectrustref) != 0)
			{
				return SecTrustResult.Deny;
			}
			if (num > 0)
			{
				intPtr2 = FromIntPtrs(array2);
				SecTrustSetAnchorCertificates(sectrustref, intPtr2);
			}
			SecTrustEvaluate(sectrustref, out secTrustResultTime);
			return secTrustResultTime;
		}
		finally
		{
			if (intPtr != IntPtr.Zero)
			{
				CFRelease(intPtr);
			}
			if (intPtr2 != IntPtr.Zero)
			{
				CFRelease(intPtr2);
			}
			for (int k = 0; k < count; k++)
			{
				if (array[k] != IntPtr.Zero)
				{
					CFRelease(array[k]);
				}
			}
			for (int l = 0; l < num; l++)
			{
				if (array2[l] != IntPtr.Zero)
				{
					CFRelease(array2[l]);
				}
			}
			if (intPtr3 != IntPtr.Zero)
			{
				CFRelease(intPtr3);
			}
			if (intPtr4 != IntPtr.Zero)
			{
				CFRelease(intPtr4);
			}
			if (sectrustref != IntPtr.Zero)
			{
				CFRelease(sectrustref);
			}
		}
	}
}
