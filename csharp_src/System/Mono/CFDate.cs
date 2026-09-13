using System;
using System.Runtime.InteropServices;
using ObjCRuntimeInternal;

namespace Mono;

internal class CFDate : INativeObject, IDisposable
{
	private IntPtr handle;

	public IntPtr Handle => handle;

	internal CFDate(IntPtr handle, bool owns)
	{
		this.handle = handle;
		if (!owns)
		{
			CFObject.CFRetain(handle);
		}
	}

	~CFDate()
	{
		Dispose(disposing: false);
	}

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern IntPtr CFDateCreate(IntPtr allocator, double at);

	public static CFDate Create(DateTime date)
	{
		DateTime dateTime = new DateTime(2001, 1, 1);
		double totalSeconds = (date - dateTime).TotalSeconds;
		IntPtr intPtr = CFDateCreate(IntPtr.Zero, totalSeconds);
		if (intPtr == IntPtr.Zero)
		{
			throw new NotSupportedException();
		}
		return new CFDate(intPtr, owns: true);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (handle != IntPtr.Zero)
		{
			CFObject.CFRelease(handle);
			handle = IntPtr.Zero;
		}
	}
}
