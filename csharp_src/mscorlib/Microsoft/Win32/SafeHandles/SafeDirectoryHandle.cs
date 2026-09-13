using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.SafeHandles;

internal sealed class SafeDirectoryHandle : SafeHandle
{
	public override bool IsInvalid => handle == IntPtr.Zero;

	private SafeDirectoryHandle()
		: base(IntPtr.Zero, ownsHandle: true)
	{
	}

	protected override bool ReleaseHandle()
	{
		return Interop.Sys.CloseDir(handle) == 0;
	}
}
