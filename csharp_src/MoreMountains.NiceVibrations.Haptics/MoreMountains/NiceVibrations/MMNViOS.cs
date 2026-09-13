namespace MoreMountains.NiceVibrations;

public static class MMNViOS
{
	private static bool iOSHapticsInitialized;

	private static void MMNViOS_InstantiateFeedbackGenerators()
	{
	}

	private static void MMNViOS_ReleaseFeedbackGenerators()
	{
	}

	private static void MMNViOS_SelectionHaptic()
	{
	}

	private static void MMNViOS_SuccessHaptic()
	{
	}

	private static void MMNViOS_WarningHaptic()
	{
	}

	private static void MMNViOS_FailureHaptic()
	{
	}

	private static void MMNViOS_LightImpactHaptic()
	{
	}

	private static void MMNViOS_MediumImpactHaptic()
	{
	}

	private static void MMNViOS_HeavyImpactHaptic()
	{
	}

	private static void MMNViOS_RigidImpactHaptic()
	{
	}

	private static void MMNViOS_SoftImpactHaptic()
	{
	}

	public static void iOSInitializeHaptics()
	{
		if (MMNVPlatform.iOS())
		{
			MMNViOS_InstantiateFeedbackGenerators();
			iOSHapticsInitialized = true;
		}
	}

	public static void iOSReleaseHaptics()
	{
		if (MMNVPlatform.iOS())
		{
			MMNViOS_ReleaseFeedbackGenerators();
		}
	}

	public static void iOSTriggerHaptics(HapticTypes type, bool defaultToRegularVibrate = false)
	{
		if (!MMNVPlatform.iOS())
		{
			return;
		}
		if (!iOSHapticsInitialized)
		{
			iOSInitializeHaptics();
		}
		if (iOSHapticsSupported())
		{
			switch (type)
			{
			case HapticTypes.Selection:
				MMNViOS_SelectionHaptic();
				break;
			case HapticTypes.Success:
				MMNViOS_SuccessHaptic();
				break;
			case HapticTypes.Warning:
				MMNViOS_WarningHaptic();
				break;
			case HapticTypes.Failure:
				MMNViOS_FailureHaptic();
				break;
			case HapticTypes.LightImpact:
				MMNViOS_LightImpactHaptic();
				break;
			case HapticTypes.MediumImpact:
				MMNViOS_MediumImpactHaptic();
				break;
			case HapticTypes.HeavyImpact:
				MMNViOS_HeavyImpactHaptic();
				break;
			case HapticTypes.RigidImpact:
				MMNViOS_RigidImpactHaptic();
				break;
			case HapticTypes.SoftImpact:
				MMNViOS_SoftImpactHaptic();
				break;
			}
		}
	}

	public static string iOSSDKVersion()
	{
		return null;
	}

	public static float ComputeiOSVersion()
	{
		int result = 0;
		int.TryParse("0.0.0".Split(new char[1] { '.' })[0], out result);
		return result;
	}

	public static bool iOSHapticsSupported()
	{
		return false;
	}
}
