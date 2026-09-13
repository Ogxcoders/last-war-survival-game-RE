using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.SafeHandles;

public sealed class SafeMemoryMappedViewHandle : SafeBuffer
{
	private IntPtr mmap_handle;

	internal SafeMemoryMappedViewHandle(IntPtr mmap_handle, IntPtr base_address, long size)
		: base(ownsHandle: true)
	{
		this.mmap_handle = mmap_handle;
		handle = base_address;
		Initialize((ulong)size);
	}

	internal void Flush()
	{
		MemoryMapImpl.Flush(mmap_handle);
	}

	protected override bool ReleaseHandle()
	{
		if (handle != (IntPtr)(-1))
		{
			return MemoryMapImpl.Unmap(mmap_handle);
		}
		throw new NotImplementedException();
	}
}
