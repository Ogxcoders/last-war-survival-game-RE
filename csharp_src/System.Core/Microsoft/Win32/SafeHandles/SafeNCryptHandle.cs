using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.SafeHandles;

public abstract class SafeNCryptHandle : SafeHandleZeroOrMinusOneIsInvalid
{
	public override bool IsInvalid
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	protected SafeNCryptHandle()
		: base(ownsHandle: true)
	{
	}

	protected SafeNCryptHandle(IntPtr handle, SafeHandle parentHandle)
		: base(ownsHandle: false)
	{
		throw new NotImplementedException();
	}

	protected override bool ReleaseHandle()
	{
		return false;
	}

	protected abstract bool ReleaseNativeHandle();
}
