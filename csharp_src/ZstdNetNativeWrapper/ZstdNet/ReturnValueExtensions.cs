using System;
using System.Runtime.InteropServices;

namespace ZstdNet;

internal static class ReturnValueExtensions
{
	public static UIntPtr EnsureZdictSuccess(this UIntPtr returnValue)
	{
		if (ExternMethods.ZDICT_isError(returnValue) != 0)
		{
			ThrowException(returnValue, Marshal.PtrToStringAnsi(ExternMethods.ZDICT_getErrorName(returnValue)));
		}
		return returnValue;
	}

	public static UIntPtr EnsureZstdSuccess(this UIntPtr returnValue)
	{
		if (ExternMethods.ZSTD_isError(returnValue) != 0)
		{
			ThrowException(returnValue, Marshal.PtrToStringAnsi(ExternMethods.ZSTD_getErrorName(returnValue)));
		}
		return returnValue;
	}

	private static void ThrowException(UIntPtr returnValue, string message)
	{
		throw new ZstdException((ZSTD_ErrorCode)(-(int)(ulong)returnValue), message);
	}

	public static IntPtr EnsureZstdSuccess(this IntPtr returnValue)
	{
		if (returnValue == IntPtr.Zero)
		{
			throw new ZstdException(ZSTD_ErrorCode.ZSTD_error_GENERIC, "Failed to create a structure");
		}
		return returnValue;
	}
}
