using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Android.Runtime;

public static class AndroidEnvironment
{
	public const string AndroidLogAppName = "Mono.Android";

	private static object lock_ = new object();

	private static bool certStoreInitOk = false;

	[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
	internal static extern void monodroid_free(IntPtr ptr);

	[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
	private static extern IntPtr _monodroid_timezone_get_default_id();

	[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
	private static extern int _monodroid_getifaddrs(out IntPtr ifap);

	[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
	private static extern void _monodroid_freeifaddrs(IntPtr ifap);

	private static string GetDefaultTimeZone()
	{
		IntPtr ptr = _monodroid_timezone_get_default_id();
		try
		{
			return Marshal.PtrToStringAnsi(ptr);
		}
		finally
		{
			monodroid_free(ptr);
		}
	}

	private static SynchronizationContext GetDefaultSyncContext()
	{
		return null;
	}

	private static IWebProxy GetDefaultProxy()
	{
		return null;
	}

	private static int GetInterfaceAddresses(out IntPtr ifap)
	{
		return _monodroid_getifaddrs(out ifap);
	}

	private static void FreeInterfaceAddresses(IntPtr ifap)
	{
		_monodroid_freeifaddrs(ifap);
	}

	private static void DetectCPUAndArchitecture(out ushort builtForCPU, out ushort runningOnCPU, out bool is64bit)
	{
		builtForCPU = 0;
		runningOnCPU = 0;
		is64bit = Environment.Is64BitProcess;
	}

	private static bool TrustEvaluateSsl(List<byte[]> certsRawData)
	{
		throw new NotImplementedException();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool _gd_mono_init_cert_store();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern byte[] _gd_mono_android_cert_store_lookup(string alias);

	private static void InitCertStore()
	{
		if (certStoreInitOk)
		{
			return;
		}
		lock (lock_)
		{
			certStoreInitOk = _gd_mono_init_cert_store();
		}
	}

	private static byte[] CertStoreLookup(long hash, bool userStore)
	{
		InitCertStore();
		if (!certStoreInitOk)
		{
			return null;
		}
		string alias = string.Format("{0}:{1:x8}.0", userStore ? "user" : "system", hash);
		return _gd_mono_android_cert_store_lookup(alias);
	}
}
