using System.ComponentModel;
using System.Runtime.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace System.Runtime.Interop;

[SuppressUnmanagedCodeSecurity]
internal static class UnsafeNativeMethods
{
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	public struct EventData
	{
		[FieldOffset(0)]
		internal ulong DataPointer;

		[FieldOffset(8)]
		internal uint Size;

		[FieldOffset(12)]
		internal int Reserved;
	}

	[SecurityCritical]
	internal unsafe delegate void EtwEnableCallback([In] ref Guid sourceId, [In] int isEnabled, [In] byte level, [In] long matchAnyKeywords, [In] long matchAllKeywords, [In] void* filterData, [In] void* callbackContext);

	public const string KERNEL32 = "kernel32.dll";

	public const string ADVAPI32 = "advapi32.dll";

	public const int ERROR_INVALID_HANDLE = 6;

	public const int ERROR_MORE_DATA = 234;

	public const int ERROR_ARITHMETIC_OVERFLOW = 534;

	public const int ERROR_NOT_ENOUGH_MEMORY = 8;

	[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
	[SecurityCritical]
	public static extern SafeWaitHandle CreateWaitableTimer(IntPtr mustBeZero, bool manualReset, string timerName);

	[DllImport("kernel32.dll", ExactSpelling = true)]
	[SecurityCritical]
	public static extern bool SetWaitableTimer(SafeWaitHandle handle, ref long dueTime, int period, IntPtr mustBeZero, IntPtr mustBeZeroAlso, bool resume);

	[DllImport("kernel32.dll", SetLastError = true)]
	[SecurityCritical]
	public static extern int QueryPerformanceCounter(out long time);

	[DllImport("kernel32.dll")]
	[SecurityCritical]
	public static extern uint GetSystemTimeAdjustment(out int adjustment, out uint increment, out uint adjustmentDisabled);

	[DllImport("kernel32.dll", SetLastError = true)]
	[SecurityCritical]
	private static extern void GetSystemTimeAsFileTime(out System.Runtime.InteropServices.ComTypes.FILETIME time);

	[SecurityCritical]
	public static void GetSystemTimeAsFileTime(out long time)
	{
		GetSystemTimeAsFileTime(out System.Runtime.InteropServices.ComTypes.FILETIME time2);
		time = 0L;
		time |= (uint)time2.dwHighDateTime;
		time <<= 32;
		time |= (uint)time2.dwLowDateTime;
	}

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	[SecurityCritical]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool GetComputerNameEx([In] ComputerNameFormat nameType, [In][Out][MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpBuffer, [In][Out] ref int size);

	[SecurityCritical]
	internal static string GetComputerName(ComputerNameFormat nameType)
	{
		int size = 0;
		if (!GetComputerNameEx(nameType, null, ref size))
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (lastWin32Error != 234)
			{
				throw Fx.Exception.AsError(new Win32Exception(lastWin32Error));
			}
		}
		if (size < 0)
		{
			Fx.AssertAndThrow("GetComputerName returned an invalid length: " + size);
		}
		StringBuilder stringBuilder = new StringBuilder(size);
		if (!GetComputerNameEx(nameType, stringBuilder, ref size))
		{
			int lastWin32Error2 = Marshal.GetLastWin32Error();
			throw Fx.Exception.AsError(new Win32Exception(lastWin32Error2));
		}
		return stringBuilder.ToString();
	}

	[DllImport("kernel32.dll")]
	[SecurityCritical]
	internal static extern bool IsDebuggerPresent();

	[DllImport("kernel32.dll")]
	[SecurityCritical]
	internal static extern void DebugBreak();

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
	[SecurityCritical]
	internal static extern void OutputDebugString(string lpOutputString);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
	[SecurityCritical]
	internal unsafe static extern uint EventRegister([In] ref Guid providerId, [In] EtwEnableCallback enableCallback, [In] void* callbackContext, [In][Out] ref long registrationHandle);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
	[SecurityCritical]
	internal static extern uint EventUnregister([In] long registrationHandle);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
	[SecurityCritical]
	internal static extern bool EventEnabled([In] long registrationHandle, [In] ref System.Runtime.Diagnostics.EventDescriptor eventDescriptor);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
	[SecurityCritical]
	internal unsafe static extern uint EventWrite([In] long registrationHandle, [In] ref System.Runtime.Diagnostics.EventDescriptor eventDescriptor, [In] uint userDataCount, [In] EventData* userData);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
	[SecurityCritical]
	internal unsafe static extern uint EventWriteTransfer([In] long registrationHandle, [In] ref System.Runtime.Diagnostics.EventDescriptor eventDescriptor, [In] ref Guid activityId, [In] ref Guid relatedActivityId, [In] uint userDataCount, [In] EventData* userData);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
	[SecurityCritical]
	internal unsafe static extern uint EventWriteString([In] long registrationHandle, [In] byte level, [In] long keywords, [In] char* message);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
	[SecurityCritical]
	internal static extern uint EventActivityIdControl([In] int ControlCode, [In][Out] ref Guid ActivityId);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	[SecurityCritical]
	internal static extern bool ReportEvent(SafeHandle hEventLog, ushort type, ushort category, uint eventID, byte[] userSID, ushort numStrings, uint dataLen, HandleRef strings, byte[] rawData);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	[SecurityCritical]
	internal static extern SafeEventLogWriteHandle RegisterEventSource(string uncServerName, string sourceName);
}
