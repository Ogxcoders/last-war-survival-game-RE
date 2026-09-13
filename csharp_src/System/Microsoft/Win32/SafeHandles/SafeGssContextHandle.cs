using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.SafeHandles;

internal sealed class SafeGssContextHandle : SafeHandle
{
	public override bool IsInvalid => handle == IntPtr.Zero;

	public SafeGssContextHandle()
		: base(IntPtr.Zero, ownsHandle: true)
	{
	}

	protected override bool ReleaseHandle()
	{
		global::Interop.NetSecurityNative.Status minorStatus;
		global::Interop.NetSecurityNative.Status num = global::Interop.NetSecurityNative.DeleteSecContext(out minorStatus, ref handle);
		SetHandle(IntPtr.Zero);
		return num == global::Interop.NetSecurityNative.Status.GSS_S_COMPLETE;
	}
}
