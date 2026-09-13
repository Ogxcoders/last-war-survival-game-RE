using UnityEngine;

namespace Unity.Collections;

public static class NativeLeakDetection
{
	private static int s_NativeLeakDetectionMode;

	private const string kNativeLeakDetectionModePrefsString = "Unity.Colletions.NativeLeakDetection.Mode";

	public static NativeLeakDetectionMode Mode
	{
		get
		{
			if (s_NativeLeakDetectionMode == 0)
			{
				Initialize();
			}
			return (NativeLeakDetectionMode)s_NativeLeakDetectionMode;
		}
		set
		{
			if (s_NativeLeakDetectionMode != (int)value)
			{
				s_NativeLeakDetectionMode = (int)value;
			}
		}
	}

	[RuntimeInitializeOnLoadMethod]
	private static void Initialize()
	{
		s_NativeLeakDetectionMode = 1;
	}
}
