using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Android;

public class AndroidDevice
{
	public static AndroidHardwareType hardwareType => RunningOnChromeOS() ? AndroidHardwareType.ChromeOS : AndroidHardwareType.Generic;

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("android::systeminfo::RunningOnChromeOS")]
	[NativeHeader("PlatformDependent/AndroidPlayer/Source/AndroidSystemInfo.h")]
	private static extern bool RunningOnChromeOS();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeHeader("PlatformDependent/AndroidPlayer/Source/SustainedPerformance.h")]
	[FreeFunction]
	public static extern void SetSustainedPerformanceMode(bool enabled);
}
