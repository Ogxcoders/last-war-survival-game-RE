using System;
using System.Runtime.InteropServices;

namespace Mono.Btls;

internal class MonoBtlsX509VerifyParam : MonoBtlsObject
{
	internal class BoringX509VerifyParamHandle : MonoBtlsHandle
	{
		public BoringX509VerifyParamHandle(IntPtr handle)
			: base(handle, ownsHandle: true)
		{
		}

		protected override bool ReleaseHandle()
		{
			mono_btls_x509_verify_param_free(handle);
			return true;
		}
	}

	internal new BoringX509VerifyParamHandle Handle => (BoringX509VerifyParamHandle)base.Handle;

	public bool CanModify => mono_btls_x509_verify_param_can_modify(Handle.DangerousGetHandle()) != 0;

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_verify_param_new();

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_verify_param_copy(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_verify_param_lookup(IntPtr name);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_verify_param_can_modify(IntPtr param);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_verify_param_set_name(IntPtr handle, IntPtr name);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_verify_param_set_host(IntPtr handle, IntPtr name, int namelen);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_verify_param_add_host(IntPtr handle, IntPtr name, int namelen);

	[DllImport("libmono-btls-shared")]
	private static extern ulong mono_btls_x509_verify_param_get_flags(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_verify_param_set_flags(IntPtr handle, ulong flags);

	[DllImport("libmono-btls-shared")]
	private static extern MonoBtlsX509VerifyFlags mono_btls_x509_verify_param_get_mono_flags(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_verify_param_set_mono_flags(IntPtr handle, MonoBtlsX509VerifyFlags flags);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_verify_param_set_purpose(IntPtr handle, MonoBtlsX509Purpose purpose);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_verify_param_get_depth(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_verify_param_set_depth(IntPtr handle, int depth);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_x509_verify_param_set_time(IntPtr handle, long time);

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_x509_verify_param_get_peername(IntPtr handle);

	[DllImport("libmono-btls-shared")]
	private static extern void mono_btls_x509_verify_param_free(IntPtr handle);

	internal MonoBtlsX509VerifyParam()
		: base(new BoringX509VerifyParamHandle(mono_btls_x509_verify_param_new()))
	{
	}

	internal MonoBtlsX509VerifyParam(BoringX509VerifyParamHandle handle)
		: base(handle)
	{
	}

	public MonoBtlsX509VerifyParam Copy()
	{
		IntPtr intPtr = mono_btls_x509_verify_param_copy(Handle.DangerousGetHandle());
		CheckError(intPtr != IntPtr.Zero, "Copy");
		return new MonoBtlsX509VerifyParam(new BoringX509VerifyParamHandle(intPtr));
	}

	public static MonoBtlsX509VerifyParam GetSslClient()
	{
		return Lookup("ssl_client", fail: true);
	}

	public static MonoBtlsX509VerifyParam GetSslServer()
	{
		return Lookup("ssl_server", fail: true);
	}

	public static MonoBtlsX509VerifyParam Lookup(string name, bool fail = false)
	{
		IntPtr intPtr = IntPtr.Zero;
		IntPtr zero = IntPtr.Zero;
		try
		{
			intPtr = Marshal.StringToHGlobalAnsi(name);
			zero = mono_btls_x509_verify_param_lookup(intPtr);
			if (zero == IntPtr.Zero)
			{
				if (!fail)
				{
					return null;
				}
				throw new MonoBtlsException("X509_VERIFY_PARAM_lookup() could not find '{0}'.", name);
			}
			return new MonoBtlsX509VerifyParam(new BoringX509VerifyParamHandle(zero));
		}
		finally
		{
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}
	}

	private void WantToModify()
	{
		if (!CanModify)
		{
			throw new MonoBtlsException("Attempting to modify read-only MonoBtlsX509VerifyParam instance.");
		}
	}

	public void SetName(string name)
	{
		WantToModify();
		IntPtr intPtr = IntPtr.Zero;
		try
		{
			intPtr = Marshal.StringToHGlobalAnsi(name);
			int ret = mono_btls_x509_verify_param_set_name(Handle.DangerousGetHandle(), intPtr);
			CheckError(ret, "SetName");
		}
		finally
		{
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}
	}

	public void SetHost(string name)
	{
		WantToModify();
		IntPtr intPtr = IntPtr.Zero;
		try
		{
			intPtr = Marshal.StringToHGlobalAnsi(name);
			int ret = mono_btls_x509_verify_param_set_host(Handle.DangerousGetHandle(), intPtr, name.Length);
			CheckError(ret, "SetHost");
		}
		finally
		{
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}
	}

	public void AddHost(string name)
	{
		WantToModify();
		IntPtr intPtr = IntPtr.Zero;
		try
		{
			intPtr = Marshal.StringToHGlobalAnsi(name);
			int ret = mono_btls_x509_verify_param_add_host(Handle.DangerousGetHandle(), intPtr, name.Length);
			CheckError(ret, "AddHost");
		}
		finally
		{
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}
	}

	public ulong GetFlags()
	{
		return mono_btls_x509_verify_param_get_flags(Handle.DangerousGetHandle());
	}

	public void SetFlags(ulong flags)
	{
		WantToModify();
		int ret = mono_btls_x509_verify_param_set_flags(Handle.DangerousGetHandle(), flags);
		CheckError(ret, "SetFlags");
	}

	public MonoBtlsX509VerifyFlags GetMonoFlags()
	{
		return mono_btls_x509_verify_param_get_mono_flags(Handle.DangerousGetHandle());
	}

	public void SetMonoFlags(MonoBtlsX509VerifyFlags flags)
	{
		WantToModify();
		int ret = mono_btls_x509_verify_param_set_mono_flags(Handle.DangerousGetHandle(), flags);
		CheckError(ret, "SetMonoFlags");
	}

	public void SetPurpose(MonoBtlsX509Purpose purpose)
	{
		WantToModify();
		int ret = mono_btls_x509_verify_param_set_purpose(Handle.DangerousGetHandle(), purpose);
		CheckError(ret, "SetPurpose");
	}

	public int GetDepth()
	{
		return mono_btls_x509_verify_param_get_depth(Handle.DangerousGetHandle());
	}

	public void SetDepth(int depth)
	{
		WantToModify();
		int ret = mono_btls_x509_verify_param_set_depth(Handle.DangerousGetHandle(), depth);
		CheckError(ret, "SetDepth");
	}

	public void SetTime(DateTime time)
	{
		WantToModify();
		DateTime value = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		long time2 = (long)time.Subtract(value).TotalSeconds;
		int ret = mono_btls_x509_verify_param_set_time(Handle.DangerousGetHandle(), time2);
		CheckError(ret, "SetTime");
	}

	public string GetPeerName()
	{
		IntPtr intPtr = mono_btls_x509_verify_param_get_peername(Handle.DangerousGetHandle());
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		return Marshal.PtrToStringAnsi(intPtr);
	}
}
