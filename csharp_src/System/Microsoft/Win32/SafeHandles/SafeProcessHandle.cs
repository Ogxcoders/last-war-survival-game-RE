using System;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles;

[SuppressUnmanagedCodeSecurity]
public sealed class SafeProcessHandle : SafeHandleZeroOrMinusOneIsInvalid
{
	internal static SafeProcessHandle InvalidHandle = new SafeProcessHandle(IntPtr.Zero);

	internal SafeProcessHandle()
		: base(ownsHandle: true)
	{
	}

	internal SafeProcessHandle(IntPtr handle)
		: base(ownsHandle: true)
	{
		SetHandle(handle);
	}

	[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	public SafeProcessHandle(IntPtr existingHandle, bool ownsHandle)
		: base(ownsHandle)
	{
		SetHandle(existingHandle);
	}

	internal void InitialSetHandle(IntPtr h)
	{
		handle = h;
	}

	protected override bool ReleaseHandle()
	{
		return NativeMethods.CloseProcess(handle);
	}
}
