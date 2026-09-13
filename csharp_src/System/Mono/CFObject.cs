using System;
using System.Runtime.InteropServices;
using ObjCRuntimeInternal;

namespace Mono;

internal class CFObject : IDisposable, INativeObject
{
	public const string CoreFoundationLibrary = "/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation";

	private const string SystemLibrary = "/usr/lib/libSystem.dylib";

	public IntPtr Handle { get; private set; }

	[DllImport("/usr/lib/libSystem.dylib")]
	public static extern IntPtr dlopen(string path, int mode);

	[DllImport("/usr/lib/libSystem.dylib")]
	private static extern IntPtr dlsym(IntPtr handle, string symbol);

	[DllImport("/usr/lib/libSystem.dylib")]
	public static extern void dlclose(IntPtr handle);

	public static IntPtr GetIndirect(IntPtr handle, string symbol)
	{
		return dlsym(handle, symbol);
	}

	public static CFString GetStringConstant(IntPtr handle, string symbol)
	{
		IntPtr intPtr = dlsym(handle, symbol);
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		IntPtr intPtr2 = Marshal.ReadIntPtr(intPtr);
		if (intPtr2 == IntPtr.Zero)
		{
			return null;
		}
		return new CFString(intPtr2, own: false);
	}

	public static IntPtr GetIntPtr(IntPtr handle, string symbol)
	{
		IntPtr intPtr = dlsym(handle, symbol);
		if (intPtr == IntPtr.Zero)
		{
			return IntPtr.Zero;
		}
		return Marshal.ReadIntPtr(intPtr);
	}

	public static IntPtr GetCFObjectHandle(IntPtr handle, string symbol)
	{
		IntPtr intPtr = dlsym(handle, symbol);
		if (intPtr == IntPtr.Zero)
		{
			return IntPtr.Zero;
		}
		return Marshal.ReadIntPtr(intPtr);
	}

	public CFObject(IntPtr handle, bool own)
	{
		Handle = handle;
		if (!own)
		{
			Retain();
		}
	}

	~CFObject()
	{
		Dispose(disposing: false);
	}

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	internal static extern IntPtr CFRetain(IntPtr handle);

	private void Retain()
	{
		CFRetain(Handle);
	}

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	internal static extern void CFRelease(IntPtr handle);

	private void Release()
	{
		CFRelease(Handle);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (Handle != IntPtr.Zero)
		{
			Release();
			Handle = IntPtr.Zero;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
