namespace System.Data;

internal static class Win32NativeMethods
{
	internal static bool IsTokenRestrictedWrapper(IntPtr token)
	{
		throw new PlatformNotSupportedException("Win32NativeMethods.IsTokenRestrictedWrapper is not supported on non-Windows platforms");
	}
}
