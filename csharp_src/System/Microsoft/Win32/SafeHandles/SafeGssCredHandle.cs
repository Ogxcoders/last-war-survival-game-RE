using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Win32.SafeHandles;

internal class SafeGssCredHandle : SafeHandle
{
	public override bool IsInvalid => handle == IntPtr.Zero;

	public static SafeGssCredHandle Create(string username, string password, bool isNtlmOnly)
	{
		if (string.IsNullOrEmpty(username))
		{
			return new SafeGssCredHandle();
		}
		SafeGssCredHandle outputCredHandle = null;
		using SafeGssNameHandle desiredName = SafeGssNameHandle.CreateUser(username);
		global::Interop.NetSecurityNative.Status minorStatus;
		global::Interop.NetSecurityNative.Status status = ((!string.IsNullOrEmpty(password)) ? global::Interop.NetSecurityNative.InitiateCredWithPassword(out minorStatus, isNtlmOnly, desiredName, password, Encoding.UTF8.GetByteCount(password), out outputCredHandle) : global::Interop.NetSecurityNative.InitiateCredSpNego(out minorStatus, desiredName, out outputCredHandle));
		if (status != global::Interop.NetSecurityNative.Status.GSS_S_COMPLETE)
		{
			outputCredHandle.Dispose();
			throw new global::Interop.NetSecurityNative.GssApiException(status, minorStatus);
		}
		return outputCredHandle;
	}

	private SafeGssCredHandle()
		: base(IntPtr.Zero, ownsHandle: true)
	{
	}

	protected override bool ReleaseHandle()
	{
		global::Interop.NetSecurityNative.Status minorStatus;
		global::Interop.NetSecurityNative.Status num = global::Interop.NetSecurityNative.ReleaseCred(out minorStatus, ref handle);
		SetHandle(IntPtr.Zero);
		return num == global::Interop.NetSecurityNative.Status.GSS_S_COMPLETE;
	}
}
