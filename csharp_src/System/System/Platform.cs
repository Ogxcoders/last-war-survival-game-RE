using System.Runtime.InteropServices;

namespace System;

internal static class Platform
{
	private static bool checkedOS;

	private static bool isMacOS;

	private static bool isAix;

	private static bool isIBMi;

	private static bool isFreeBSD;

	private static bool isOpenBSD;

	public static bool IsMacOS
	{
		get
		{
			if (!checkedOS)
			{
				CheckOS();
			}
			return isMacOS;
		}
	}

	public static bool IsFreeBSD
	{
		get
		{
			if (!checkedOS)
			{
				CheckOS();
			}
			return isFreeBSD;
		}
	}

	public static bool IsOpenBSD
	{
		get
		{
			if (!checkedOS)
			{
				CheckOS();
			}
			return isOpenBSD;
		}
	}

	public static bool IsIBMi
	{
		get
		{
			if (!checkedOS)
			{
				CheckOS();
			}
			return isIBMi;
		}
	}

	public static bool IsAix
	{
		get
		{
			if (!checkedOS)
			{
				CheckOS();
			}
			return isAix;
		}
	}

	[DllImport("libc")]
	private static extern int uname(IntPtr buf);

	private static void CheckOS()
	{
		if (Environment.OSVersion.Platform != PlatformID.Unix)
		{
			checkedOS = true;
			return;
		}
		IntPtr intPtr = Marshal.AllocHGlobal(8192);
		if (uname(intPtr) == 0)
		{
			switch (Marshal.PtrToStringAnsi(intPtr))
			{
			case "Darwin":
				isMacOS = true;
				break;
			case "FreeBSD":
				isFreeBSD = true;
				break;
			case "AIX":
				isAix = true;
				break;
			case "OS400":
				isIBMi = true;
				break;
			case "OpenBSD":
				isOpenBSD = true;
				break;
			}
		}
		Marshal.FreeHGlobal(intPtr);
		checkedOS = true;
	}
}
