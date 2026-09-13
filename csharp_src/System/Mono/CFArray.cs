using System;
using System.Runtime.InteropServices;
using ObjCRuntimeInternal;

namespace Mono;

internal class CFArray : CFObject
{
	private static readonly IntPtr kCFTypeArrayCallbacks;

	public int Count => (int)CFArrayGetCount(base.Handle);

	public IntPtr this[int index] => CFArrayGetValueAtIndex(base.Handle, (IntPtr)index);

	public CFArray(IntPtr handle, bool own)
		: base(handle, own)
	{
	}

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern IntPtr CFArrayCreate(IntPtr allocator, IntPtr values, IntPtr numValues, IntPtr callbacks);

	static CFArray()
	{
		IntPtr intPtr = CFObject.dlopen("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation", 0);
		if (intPtr == IntPtr.Zero)
		{
			return;
		}
		try
		{
			kCFTypeArrayCallbacks = CFObject.GetIndirect(intPtr, "kCFTypeArrayCallBacks");
		}
		finally
		{
			CFObject.dlclose(intPtr);
		}
	}

	public static CFArray FromNativeObjects(params INativeObject[] values)
	{
		return new CFArray(Create(values), own: true);
	}

	public unsafe static IntPtr Create(params IntPtr[] values)
	{
		if (values == null)
		{
			throw new ArgumentNullException("values");
		}
		fixed (IntPtr* ptr = values)
		{
			return CFArrayCreate(IntPtr.Zero, (IntPtr)ptr, (IntPtr)values.Length, kCFTypeArrayCallbacks);
		}
	}

	internal unsafe static CFArray CreateArray(params IntPtr[] values)
	{
		if (values == null)
		{
			throw new ArgumentNullException("values");
		}
		fixed (IntPtr* ptr = values)
		{
			return new CFArray(CFArrayCreate(IntPtr.Zero, (IntPtr)ptr, (IntPtr)values.Length, kCFTypeArrayCallbacks), own: false);
		}
	}

	public static CFArray CreateArray(params INativeObject[] values)
	{
		return new CFArray(Create(values), own: true);
	}

	public static IntPtr Create(params INativeObject[] values)
	{
		if (values == null)
		{
			throw new ArgumentNullException("values");
		}
		IntPtr[] array = new IntPtr[values.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = values[i].Handle;
		}
		return Create(array);
	}

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern IntPtr CFArrayGetCount(IntPtr handle);

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern IntPtr CFArrayGetValueAtIndex(IntPtr handle, IntPtr index);

	public static T[] ArrayFromHandle<T>(IntPtr handle, Func<IntPtr, T> creation) where T : class, INativeObject
	{
		if (handle == IntPtr.Zero)
		{
			return null;
		}
		IntPtr intPtr = CFArrayGetCount(handle);
		T[] array = new T[(int)intPtr];
		for (uint num = 0u; num < (uint)(int)intPtr; num++)
		{
			array[num] = creation(CFArrayGetValueAtIndex(handle, (IntPtr)num));
		}
		return array;
	}
}
