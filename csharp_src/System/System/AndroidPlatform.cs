using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Mono;
using Mono.Btls;

namespace System;

internal static class AndroidPlatform
{
	private delegate int GetInterfaceAddressesDelegate(out IntPtr ifap);

	private delegate void FreeInterfaceAddressesDelegate(IntPtr ifap);

	private static readonly Converter<List<byte[]>, bool> trustEvaluateSsl;

	private static readonly Func<long, bool, byte[]> certStoreLookup;

	private static readonly Func<IWebProxy> getDefaultProxy;

	private static readonly GetInterfaceAddressesDelegate getInterfaceAddresses;

	private static readonly FreeInterfaceAddressesDelegate freeInterfaceAddresses;

	static AndroidPlatform()
	{
		Type type = Type.GetType("Android.Runtime.AndroidEnvironment, Mono.Android", throwOnError: true);
		trustEvaluateSsl = (Converter<List<byte[]>, bool>)Delegate.CreateDelegate(typeof(Converter<List<byte[]>, bool>), type, "TrustEvaluateSsl", ignoreCase: false, throwOnBindFailure: true);
		certStoreLookup = (Func<long, bool, byte[]>)Delegate.CreateDelegate(typeof(Func<long, bool, byte[]>), type, "CertStoreLookup", ignoreCase: false, throwOnBindFailure: true);
		SystemDependencyProvider.Initialize();
		getDefaultProxy = (Func<IWebProxy>)Delegate.CreateDelegate(typeof(Func<IWebProxy>), type, "GetDefaultProxy", ignoreCase: false, throwOnBindFailure: true);
		getInterfaceAddresses = (GetInterfaceAddressesDelegate)Delegate.CreateDelegate(typeof(GetInterfaceAddressesDelegate), type, "GetInterfaceAddresses", ignoreCase: false, throwOnBindFailure: false);
		freeInterfaceAddresses = (FreeInterfaceAddressesDelegate)Delegate.CreateDelegate(typeof(FreeInterfaceAddressesDelegate), type, "FreeInterfaceAddresses", ignoreCase: false, throwOnBindFailure: false);
	}

	internal static bool TrustEvaluateSsl(X509CertificateCollection collection)
	{
		List<byte[]> list = new List<byte[]>(collection.Count);
		foreach (X509Certificate item in collection)
		{
			list.Add(item.GetRawCertData());
		}
		return trustEvaluateSsl(list);
	}

	internal static MonoBtlsX509 CertStoreLookup(MonoBtlsX509Name name)
	{
		long hash = name.GetHash();
		long hashOld = name.GetHashOld();
		byte[] array = certStoreLookup(hash, arg2: false);
		if (array == null)
		{
			array = certStoreLookup(hashOld, arg2: false);
		}
		if (array == null)
		{
			array = certStoreLookup(hash, arg2: true);
		}
		if (array == null)
		{
			array = certStoreLookup(hashOld, arg2: true);
		}
		if (array == null)
		{
			return null;
		}
		return MonoBtlsX509.LoadFromData(array, MonoBtlsX509Format.DER);
	}

	internal static IWebProxy GetDefaultProxy()
	{
		return getDefaultProxy();
	}

	internal static int GetInterfaceAddresses(out IntPtr ifap)
	{
		ifap = IntPtr.Zero;
		if (getInterfaceAddresses == null)
		{
			return -1;
		}
		return getInterfaceAddresses(out ifap);
	}

	internal static void FreeInterfaceAddresses(IntPtr ifap)
	{
		if (freeInterfaceAddresses != null)
		{
			freeInterfaceAddresses(ifap);
		}
	}
}
