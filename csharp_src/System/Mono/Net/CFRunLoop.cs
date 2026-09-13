using System;
using System.Runtime.InteropServices;

namespace Mono.Net;

internal class CFRunLoop : CFObject
{
	public static CFRunLoop CurrentRunLoop => new CFRunLoop(CFRunLoopGetCurrent(), own: false);

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern void CFRunLoopAddSource(IntPtr rl, IntPtr source, IntPtr mode);

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern void CFRunLoopRemoveSource(IntPtr rl, IntPtr source, IntPtr mode);

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern int CFRunLoopRunInMode(IntPtr mode, double seconds, bool returnAfterSourceHandled);

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern IntPtr CFRunLoopGetCurrent();

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern void CFRunLoopStop(IntPtr rl);

	public CFRunLoop(IntPtr handle, bool own)
		: base(handle, own)
	{
	}

	public void AddSource(IntPtr source, CFString mode)
	{
		CFRunLoopAddSource(base.Handle, source, mode.Handle);
	}

	public void RemoveSource(IntPtr source, CFString mode)
	{
		CFRunLoopRemoveSource(base.Handle, source, mode.Handle);
	}

	public int RunInMode(CFString mode, double seconds, bool returnAfterSourceHandled)
	{
		return CFRunLoopRunInMode(mode.Handle, seconds, returnAfterSourceHandled);
	}

	public void Stop()
	{
		CFRunLoopStop(base.Handle);
	}
}
