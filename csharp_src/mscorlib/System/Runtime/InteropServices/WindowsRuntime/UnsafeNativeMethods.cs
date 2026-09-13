using System.Runtime.CompilerServices;

namespace System.Runtime.InteropServices.WindowsRuntime;

internal static class UnsafeNativeMethods
{
	[MethodImpl(MethodImplOptions.InternalCall)]
	public unsafe static extern int WindowsCreateString(string sourceString, int length, IntPtr* hstring);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern int WindowsDeleteString(IntPtr hstring);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public unsafe static extern char* WindowsGetStringRawBuffer(IntPtr hstring, uint* length);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern bool RoOriginateLanguageException(int error, string message, IntPtr languageException);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void RoReportUnhandledError(IRestrictedErrorInfo error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern IRestrictedErrorInfo GetRestrictedErrorInfo();
}
