using System.Runtime.InteropServices;

namespace System.IO;

internal class HGlobalUnmanagedMemoryStream : UnmanagedMemoryStream
{
	private IntPtr ptr;

	public unsafe HGlobalUnmanagedMemoryStream(byte* pointer, long length, IntPtr ptr)
		: base(pointer, length, length, FileAccess.ReadWrite)
	{
		this.ptr = ptr;
	}

	protected override void Dispose(bool disposing)
	{
		if (_isOpen)
		{
			Marshal.FreeHGlobal(ptr);
		}
		base.Dispose(disposing);
	}
}
