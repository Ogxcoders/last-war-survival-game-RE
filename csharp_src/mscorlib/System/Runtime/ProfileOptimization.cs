using System.Security;

namespace System.Runtime;

public static class ProfileOptimization
{
	internal static void InternalSetProfileRoot(string directoryPath)
	{
	}

	internal static void InternalStartProfile(string profile, IntPtr ptrNativeAssemblyLoadContext)
	{
	}

	[SecurityCritical]
	public static void SetProfileRoot(string directoryPath)
	{
		InternalSetProfileRoot(directoryPath);
	}

	[SecurityCritical]
	public static void StartProfile(string profile)
	{
		InternalStartProfile(profile, IntPtr.Zero);
	}
}
