using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Runtime.Interop;

[SecurityCritical]
internal sealed class SafeEventLogWriteHandle : SafeHandleZeroOrMinusOneIsInvalid
{
	[SecurityCritical]
	private SafeEventLogWriteHandle()
		: base(ownsHandle: true)
	{
	}

	[SecurityCritical]
	public static SafeEventLogWriteHandle RegisterEventSource(string uncServerName, string sourceName)
	{
		SafeEventLogWriteHandle safeEventLogWriteHandle = UnsafeNativeMethods.RegisterEventSource(uncServerName, sourceName);
		Marshal.GetLastWin32Error();
		_ = safeEventLogWriteHandle.IsInvalid;
		return safeEventLogWriteHandle;
	}

	[DllImport("advapi32", SetLastError = true)]
	private static extern bool DeregisterEventSource(IntPtr hEventLog);

	[SecurityCritical]
	protected override bool ReleaseHandle()
	{
		return DeregisterEventSource(handle);
	}
}
