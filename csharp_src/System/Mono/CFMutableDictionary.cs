using System;
using System.Runtime.InteropServices;

namespace Mono;

internal class CFMutableDictionary : CFDictionary
{
	public CFMutableDictionary(IntPtr handle, bool own)
		: base(handle, own)
	{
	}

	public void SetValue(IntPtr key, IntPtr val)
	{
		CFDictionarySetValue(base.Handle, key, val);
	}

	public static CFMutableDictionary Create()
	{
		IntPtr intPtr = CFDictionaryCreateMutable(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
		if (intPtr == IntPtr.Zero)
		{
			throw new InvalidOperationException();
		}
		return new CFMutableDictionary(intPtr, own: true);
	}

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern void CFDictionarySetValue(IntPtr handle, IntPtr key, IntPtr val);

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern IntPtr CFDictionaryCreateMutable(IntPtr allocator, IntPtr capacity, IntPtr keyCallback, IntPtr valueCallbacks);
}
