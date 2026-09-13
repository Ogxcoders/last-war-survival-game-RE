using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.SafeHandles;

public sealed class SafeNCryptKeyHandle : SafeNCryptHandle
{
	public SafeNCryptKeyHandle()
	{
	}

	public SafeNCryptKeyHandle(IntPtr handle, SafeHandle parentHandle)
		: base(handle, parentHandle)
	{
	}

	protected override bool ReleaseNativeHandle()
	{
		return false;
	}
}
