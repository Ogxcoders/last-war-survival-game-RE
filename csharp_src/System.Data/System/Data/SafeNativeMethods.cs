using System.Runtime.InteropServices;

namespace System.Data;

internal static class SafeNativeMethods
{
	internal static IntPtr LocalAlloc(IntPtr initialSize)
	{
		IntPtr intPtr = Marshal.AllocHGlobal(initialSize);
		ZeroMemory(intPtr, (int)initialSize);
		return intPtr;
	}

	internal static void LocalFree(IntPtr ptr)
	{
		Marshal.FreeHGlobal(ptr);
	}

	internal static void ZeroMemory(IntPtr ptr, int length)
	{
		Marshal.Copy(new byte[length], 0, ptr, length);
	}

	internal static IntPtr GetProcAddress(IntPtr HModule, string funcName)
	{
		throw new PlatformNotSupportedException("SafeNativeMethods.GetProcAddress is not supported on non-Windows platforms");
	}
}
