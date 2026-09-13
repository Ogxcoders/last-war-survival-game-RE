using MoreMountains.NiceVibrations;

public class Vibrator
{
	public static bool HapticsSupported()
	{
		return MMVibrationManager.HapticsSupported();
	}

	public static void SetDebugMode(bool log)
	{
		MMVibrationManager.SetDebugMode(log);
	}

	public static void SoftImpact()
	{
		MMVibrationManager.Haptic(HapticTypes.SoftImpact);
	}

	public static void LightImpact()
	{
		MMVibrationManager.Haptic(HapticTypes.LightImpact);
	}

	public static void MediumImpact()
	{
		MMVibrationManager.Haptic(HapticTypes.MediumImpact);
	}

	public static void HeavyImpact()
	{
		MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
	}

	public static void Warning()
	{
		MMVibrationManager.Haptic(HapticTypes.Warning);
	}

	public static void Selection()
	{
		MMVibrationManager.Haptic(HapticTypes.Selection);
	}

	public static void Success()
	{
		MMVibrationManager.Haptic(HapticTypes.Success);
	}

	public static void Failure()
	{
		MMVibrationManager.Haptic(HapticTypes.Failure);
	}

	public static void RigidImpact()
	{
		MMVibrationManager.Haptic(HapticTypes.RigidImpact);
	}

	public static void Vibrate()
	{
		MMVibrationManager.Vibrate();
	}

	public static void ContinuousHaptic(float intensity, float sharpness, float duration, int fallbackOldiOS)
	{
		MMVibrationManager.ContinuousHaptic(vibrateiOS: true, intensity, sharpness, (HapticTypes)fallbackOldiOS, vibrateAndroid: true, intensity, sharpness, vibrateAndroidIfNoSupport: true, rumble: false, intensity, sharpness, -1, duration);
	}

	public static void StopContinuousHaptic()
	{
		MMVibrationManager.StopContinuousHaptic();
	}
}
