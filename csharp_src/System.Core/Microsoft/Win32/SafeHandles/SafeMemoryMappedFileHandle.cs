using System;
using System.IO.MemoryMappedFiles;

namespace Microsoft.Win32.SafeHandles;

public sealed class SafeMemoryMappedFileHandle : SafeHandleZeroOrMinusOneIsInvalid
{
	public SafeMemoryMappedFileHandle(IntPtr preexistingHandle, bool ownsHandle)
		: base(ownsHandle)
	{
		handle = preexistingHandle;
	}

	protected override bool ReleaseHandle()
	{
		MemoryMapImpl.CloseMapping(handle);
		handle = IntPtr.Zero;
		return true;
	}
}
