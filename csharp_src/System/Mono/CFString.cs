using System;
using System.Runtime.InteropServices;

namespace Mono;

internal class CFString : CFObject
{
	private string str;

	public int Length
	{
		get
		{
			if (str != null)
			{
				return str.Length;
			}
			return (int)CFStringGetLength(base.Handle);
		}
	}

	public CFString(IntPtr handle, bool own)
		: base(handle, own)
	{
	}

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern IntPtr CFStringCreateWithCharacters(IntPtr alloc, IntPtr chars, IntPtr length);

	public unsafe static CFString Create(string value)
	{
		IntPtr intPtr;
		fixed (char* ptr = value)
		{
			intPtr = CFStringCreateWithCharacters(IntPtr.Zero, (IntPtr)ptr, (IntPtr)value.Length);
		}
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		return new CFString(intPtr, own: true);
	}

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern IntPtr CFStringGetLength(IntPtr handle);

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern int CFStringCompare(IntPtr theString1, IntPtr theString2, int compareOptions);

	public static int Compare(IntPtr string1, IntPtr string2, int compareOptions = 0)
	{
		return CFStringCompare(string1, string2, compareOptions);
	}

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern IntPtr CFStringGetCharactersPtr(IntPtr handle);

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern IntPtr CFStringGetCharacters(IntPtr handle, CFRange range, IntPtr buffer);

	public unsafe static string AsString(IntPtr handle)
	{
		if (handle == IntPtr.Zero)
		{
			return null;
		}
		int num = (int)CFStringGetLength(handle);
		if (num == 0)
		{
			return string.Empty;
		}
		IntPtr intPtr = CFStringGetCharactersPtr(handle);
		IntPtr intPtr2 = IntPtr.Zero;
		if (intPtr == IntPtr.Zero)
		{
			CFRange range = new CFRange(0, num);
			intPtr2 = Marshal.AllocHGlobal(num * 2);
			CFStringGetCharacters(handle, range, intPtr2);
			intPtr = intPtr2;
		}
		string result = new string((char*)(void*)intPtr, 0, num);
		if (intPtr2 != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(intPtr2);
		}
		return result;
	}

	public override string ToString()
	{
		if (str == null)
		{
			str = AsString(base.Handle);
		}
		return str;
	}

	public static implicit operator string(CFString str)
	{
		return str.ToString();
	}

	public static implicit operator CFString(string str)
	{
		return Create(str);
	}
}
