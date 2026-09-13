using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Win32.SafeHandles;

internal sealed class SafeGssNameHandle : SafeHandle
{
	public override bool IsInvalid => handle == IntPtr.Zero;

	public static SafeGssNameHandle CreateUser(string name)
	{
		global::Interop.NetSecurityNative.Status minorStatus;
		SafeGssNameHandle outputName;
		global::Interop.NetSecurityNative.Status status = global::Interop.NetSecurityNative.ImportUserName(out minorStatus, name, Encoding.UTF8.GetByteCount(name), out outputName);
		if (status != global::Interop.NetSecurityNative.Status.GSS_S_COMPLETE)
		{
			outputName.Dispose();
			throw new global::Interop.NetSecurityNative.GssApiException(status, minorStatus);
		}
		return outputName;
	}

	public static SafeGssNameHandle CreatePrincipal(string name)
	{
		global::Interop.NetSecurityNative.Status minorStatus;
		SafeGssNameHandle outputName;
		global::Interop.NetSecurityNative.Status status = global::Interop.NetSecurityNative.ImportPrincipalName(out minorStatus, name, Encoding.UTF8.GetByteCount(name), out outputName);
		if (status != global::Interop.NetSecurityNative.Status.GSS_S_COMPLETE)
		{
			outputName.Dispose();
			throw new global::Interop.NetSecurityNative.GssApiException(status, minorStatus);
		}
		return outputName;
	}

	protected override bool ReleaseHandle()
	{
		global::Interop.NetSecurityNative.Status minorStatus;
		global::Interop.NetSecurityNative.Status num = global::Interop.NetSecurityNative.ReleaseName(out minorStatus, ref handle);
		SetHandle(IntPtr.Zero);
		return num == global::Interop.NetSecurityNative.Status.GSS_S_COMPLETE;
	}

	private SafeGssNameHandle()
		: base(IntPtr.Zero, ownsHandle: true)
	{
	}
}
