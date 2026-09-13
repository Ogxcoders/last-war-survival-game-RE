using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Win32.SafeHandles;

[SecurityCritical]
public sealed class SafeAccessTokenHandle : SafeHandle
{
	public static SafeAccessTokenHandle InvalidHandle
	{
		[SecurityCritical]
		get
		{
			return new SafeAccessTokenHandle(IntPtr.Zero);
		}
	}

	public override bool IsInvalid
	{
		[SecurityCritical]
		get
		{
			if (!(handle == IntPtr.Zero))
			{
				return handle == new IntPtr(-1);
			}
			return true;
		}
	}

	private SafeAccessTokenHandle()
		: base(IntPtr.Zero, ownsHandle: true)
	{
	}

	public SafeAccessTokenHandle(IntPtr handle)
		: base(IntPtr.Zero, ownsHandle: true)
	{
		SetHandle(handle);
	}

	[SecurityCritical]
	protected override bool ReleaseHandle()
	{
		return true;
	}
}
