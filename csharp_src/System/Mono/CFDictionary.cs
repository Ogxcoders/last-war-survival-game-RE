using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Mono;

internal class CFDictionary : CFObject
{
	private static readonly IntPtr KeyCallbacks;

	private static readonly IntPtr ValueCallbacks;

	public IntPtr this[IntPtr key] => GetValue(key);

	static CFDictionary()
	{
		IntPtr intPtr = CFObject.dlopen("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation", 0);
		if (intPtr == IntPtr.Zero)
		{
			return;
		}
		try
		{
			KeyCallbacks = CFObject.GetIndirect(intPtr, "kCFTypeDictionaryKeyCallBacks");
			ValueCallbacks = CFObject.GetIndirect(intPtr, "kCFTypeDictionaryValueCallBacks");
		}
		finally
		{
			CFObject.dlclose(intPtr);
		}
	}

	public CFDictionary(IntPtr handle, bool own)
		: base(handle, own)
	{
	}

	public static CFDictionary FromObjectAndKey(IntPtr obj, IntPtr key)
	{
		return new CFDictionary(CFDictionaryCreate(IntPtr.Zero, new IntPtr[1] { key }, new IntPtr[1] { obj }, (IntPtr)1, KeyCallbacks, ValueCallbacks), own: true);
	}

	public static CFDictionary FromKeysAndObjects(IList<Tuple<IntPtr, IntPtr>> items)
	{
		IntPtr[] array = new IntPtr[items.Count];
		IntPtr[] array2 = new IntPtr[items.Count];
		for (int i = 0; i < items.Count; i++)
		{
			array[i] = items[i].Item1;
			array2[i] = items[i].Item2;
		}
		return new CFDictionary(CFDictionaryCreate(IntPtr.Zero, array, array2, (IntPtr)items.Count, KeyCallbacks, ValueCallbacks), own: true);
	}

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern IntPtr CFDictionaryCreate(IntPtr allocator, IntPtr[] keys, IntPtr[] vals, IntPtr len, IntPtr keyCallbacks, IntPtr valCallbacks);

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern IntPtr CFDictionaryGetValue(IntPtr handle, IntPtr key);

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern IntPtr CFDictionaryCreateCopy(IntPtr allocator, IntPtr handle);

	public CFDictionary Copy()
	{
		return new CFDictionary(CFDictionaryCreateCopy(IntPtr.Zero, base.Handle), own: true);
	}

	public CFMutableDictionary MutableCopy()
	{
		return new CFMutableDictionary(CFDictionaryCreateMutableCopy(IntPtr.Zero, IntPtr.Zero, base.Handle), own: true);
	}

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern IntPtr CFDictionaryCreateMutableCopy(IntPtr allocator, IntPtr capacity, IntPtr theDict);

	public IntPtr GetValue(IntPtr key)
	{
		return CFDictionaryGetValue(base.Handle, key);
	}
}
