using System;
using System.Runtime.InteropServices;

namespace Mono.Net;

internal class CFUrl : CFObject
{
	public CFUrl(IntPtr handle, bool own)
		: base(handle, own)
	{
	}

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern IntPtr CFURLCreateWithString(IntPtr allocator, IntPtr str, IntPtr baseURL);

	public static CFUrl Create(string absolute)
	{
		if (string.IsNullOrEmpty(absolute))
		{
			return null;
		}
		CFString cFString = CFString.Create(absolute);
		IntPtr intPtr = CFURLCreateWithString(IntPtr.Zero, cFString.Handle, IntPtr.Zero);
		cFString.Dispose();
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		return new CFUrl(intPtr, own: true);
	}
}
