using UnityEngine;

namespace MoreMountains.NiceVibrations;

public static class MMVibrationManager
{
	public static float iOSVersion;

	public static long LightDuration;

	public static long MediumDuration;

	public static long HeavyDuration;

	public static long RigidDuration;

	public static long SoftDuration;

	public static int LightAmplitude;

	public static int MediumAmplitude;

	public static int HeavyAmplitude;

	public static int RigidAmplitude;

	public static int SoftAmplitude;

	private static bool _vibrationsActive;

	private static bool _debugLogActive;

	private static bool _hapticsPlayedOnce;

	private static long[] _rigidImpactPattern;

	private static int[] _rigidImpactPatternAmplitude;

	private static long[] _softImpactPattern;

	private static int[] _softImpactPatternAmplitude;

	private static long[] _lightImpactPattern;

	private static int[] _lightImpactPatternAmplitude;

	private static long[] _mediumImpactPattern;

	private static int[] _mediumImpactPatternAmplitude;

	private static long[] _HeavyImpactPattern;

	private static int[] _HeavyImpactPatternAmplitude;

	private static long[] _successPattern;

	private static int[] _successPatternAmplitude;

	private static long[] _warningPattern;

	private static int[] _warningPatternAmplitude;

	private static long[] _failurePattern;

	private static int[] _failurePatternAmplitude;

	static MMVibrationManager()
	{
		LightDuration = 15L;
		MediumDuration = 40L;
		HeavyDuration = 80L;
		RigidDuration = 20L;
		SoftDuration = 80L;
		LightAmplitude = 35;
		MediumAmplitude = 120;
		HeavyAmplitude = 255;
		RigidAmplitude = 255;
		SoftAmplitude = 40;
		_vibrationsActive = true;
		_debugLogActive = false;
		_hapticsPlayedOnce = false;
		_rigidImpactPattern = new long[2] { 0L, RigidDuration };
		_rigidImpactPatternAmplitude = new int[2] { 0, RigidAmplitude };
		_softImpactPattern = new long[2] { 0L, SoftDuration };
		_softImpactPatternAmplitude = new int[2] { 0, SoftAmplitude };
		_lightImpactPattern = new long[2] { 0L, LightDuration };
		_lightImpactPatternAmplitude = new int[2] { 0, LightAmplitude };
		_mediumImpactPattern = new long[2] { 0L, MediumDuration };
		_mediumImpactPatternAmplitude = new int[2] { 0, MediumAmplitude };
		_HeavyImpactPattern = new long[2] { 0L, HeavyDuration };
		_HeavyImpactPatternAmplitude = new int[2] { 0, HeavyAmplitude };
		_successPattern = new long[4] { 0L, LightDuration, LightDuration, HeavyDuration };
		_successPatternAmplitude = new int[4] { 0, LightAmplitude, 0, HeavyAmplitude };
		_warningPattern = new long[4] { 0L, HeavyDuration, LightDuration, MediumDuration };
		_warningPatternAmplitude = new int[4] { 0, HeavyAmplitude, 0, MediumAmplitude };
		_failurePattern = new long[8] { 0L, MediumDuration, LightDuration, MediumDuration, LightDuration, HeavyDuration, LightDuration, LightDuration };
		_failurePatternAmplitude = new int[8] { 0, MediumAmplitude, 0, MediumAmplitude, 0, HeavyAmplitude, 0, LightAmplitude };
		DebugLog("[MMVibrationManager] Initialize vibration manager");
		iOSVersion = MMNViOS.ComputeiOSVersion();
	}

	public static void SetHapticsActive(bool status)
	{
		DebugLog("[MMVibrationManager] Set haptics active : " + status);
		_vibrationsActive = status;
		if (!status)
		{
			StopContinuousHaptic(alsoRumble: true);
		}
	}

	public static bool HapticsSupported()
	{
		if (iOS())
		{
			if (iOSVersion >= 13f)
			{
				return MMNViOSCoreHaptics.CoreHapticsSupported();
			}
			return MMNViOS.iOSHapticsSupported();
		}
		if (Android())
		{
			return MMNVAndroid.AndroidHasVibrator();
		}
		return false;
	}

	public static void SetDebugMode(bool log)
	{
		_debugLogActive = log;
		MMNViOSCoreHaptics.SetDebugMode(newStatus: true);
	}

	public static bool Android()
	{
		return MMNVPlatform.Android();
	}

	public static bool iOS()
	{
		return MMNVPlatform.iOS();
	}

	public static void Vibrate()
	{
		DebugLog("[MMVibrationManager] Vibrate");
		if (!_vibrationsActive)
		{
			return;
		}
		if (Android())
		{
			MMNVAndroid.AndroidVibrate(MediumDuration);
		}
		else if (iOS())
		{
			if (iOSVersion >= 13f && HapticsSupported())
			{
				MMNViOSCoreHaptics.PlayTransientHapticPattern(0.8f, 0.8f, threaded: true);
				_hapticsPlayedOnce = true;
			}
			else
			{
				MMNViOS.iOSTriggerHaptics(HapticTypes.MediumImpact);
			}
		}
	}

	public static void Haptic(HapticTypes type, bool defaultToRegularVibrate = false, bool alsoRumble = false, MonoBehaviour coroutineSupport = null, int controllerID = -1)
	{
		if (!_vibrationsActive)
		{
			return;
		}
		DebugLog("[MMVibrationManager] Regular Haptic");
		if (Android())
		{
			switch (type)
			{
			case HapticTypes.Selection:
				MMNVAndroid.AndroidVibrate(LightDuration, LightAmplitude);
				break;
			case HapticTypes.Success:
				MMNVAndroid.AndroidVibrate(_successPattern, _successPatternAmplitude, -1);
				break;
			case HapticTypes.Warning:
				MMNVAndroid.AndroidVibrate(_warningPattern, _warningPatternAmplitude, -1);
				break;
			case HapticTypes.Failure:
				MMNVAndroid.AndroidVibrate(_failurePattern, _failurePatternAmplitude, -1);
				break;
			case HapticTypes.LightImpact:
				MMNVAndroid.AndroidVibrate(_lightImpactPattern, _lightImpactPatternAmplitude, -1);
				break;
			case HapticTypes.MediumImpact:
				MMNVAndroid.AndroidVibrate(_mediumImpactPattern, _mediumImpactPatternAmplitude, -1);
				break;
			case HapticTypes.HeavyImpact:
				MMNVAndroid.AndroidVibrate(_HeavyImpactPattern, _HeavyImpactPatternAmplitude, -1);
				break;
			case HapticTypes.RigidImpact:
				MMNVAndroid.AndroidVibrate(_rigidImpactPattern, _rigidImpactPatternAmplitude, -1);
				break;
			case HapticTypes.SoftImpact:
				MMNVAndroid.AndroidVibrate(_softImpactPattern, _softImpactPatternAmplitude, -1);
				break;
			}
		}
		else if (iOS())
		{
			MMNViOS.iOSTriggerHaptics(type, defaultToRegularVibrate);
		}
		if (alsoRumble)
		{
			_ = coroutineSupport != null;
		}
	}

	public static void TransientHaptic(float intensity, float sharpness, bool alsoRumble = false, MonoBehaviour coroutineSupport = null, int controllerID = -1)
	{
		TransientHaptic(vibrateiOS: true, intensity, sharpness, vibrateAndroid: true, intensity, sharpness, vibrateAndroidIfNoSupport: true, alsoRumble, intensity, sharpness, controllerID, coroutineSupport);
	}

	public static void TransientHaptic(bool vibrateiOS, float iOSIntensity, float iOSSharpness, bool vibrateAndroid, float androidIntensity = 1f, float androidSharpness = 1f, bool vibrateAndroidIfNoSupport = false, bool rumble = true, float rumbleLowFrequency = 1f, float rumbleHighFrequency = 1f, int controllerID = -1, MonoBehaviour coroutineSupport = null, bool threaded = true)
	{
		if (!_vibrationsActive)
		{
			return;
		}
		DebugLog("[MMVibrationManager] Transient Haptic");
		if (Android() && vibrateAndroid)
		{
			if (!MMNVAndroid.AndroidHasAmplitudeControl() && !vibrateAndroidIfNoSupport)
			{
				return;
			}
			androidIntensity = Remap(androidIntensity, 0f, 1f, 0f, 255f);
			MMNVAndroid.AndroidVibrate(100L, (int)androidIntensity);
		}
		else if (iOS() && vibrateiOS)
		{
			if (iOSVersion >= 13f && HapticsSupported())
			{
				MMNViOSCoreHaptics.PlayTransientHapticPattern(iOSIntensity, iOSSharpness, threaded);
				_hapticsPlayedOnce = true;
			}
			else if (iOSIntensity < 0.3f)
			{
				MMNViOS.iOSTriggerHaptics(HapticTypes.LightImpact);
			}
			else if (iOSIntensity >= 0.3f && iOSIntensity < 0.6f)
			{
				MMNViOS.iOSTriggerHaptics(HapticTypes.MediumImpact);
			}
			else
			{
				MMNViOS.iOSTriggerHaptics(HapticTypes.HeavyImpact);
			}
		}
		if (rumble)
		{
			_ = coroutineSupport != null;
		}
	}

	public static void ContinuousHaptic(float intensity, float sharpness, float duration, HapticTypes fallbackOldiOS = HapticTypes.None, MonoBehaviour mono = null, bool alsoRumble = false, int controllerID = -1, bool threaded = false, bool fullIntensity = true)
	{
		ContinuousHaptic(vibrateiOS: true, intensity, sharpness, fallbackOldiOS, vibrateAndroid: true, intensity, sharpness, vibrateAndroidIfNoSupport: false, alsoRumble, intensity, sharpness, controllerID, duration, mono, threaded, fullIntensity);
	}

	public static void ContinuousHaptic(bool vibrateiOS, float iOSIntensity, float iOSSharpness, HapticTypes fallbackOldiOS, bool vibrateAndroid, float androidIntensity, float androidSharpness, bool vibrateAndroidIfNoSupport, bool rumble, float rumbleLowFrequency, float rumbleHighFrequency, int controllerID, float duration, MonoBehaviour mono = null, bool threaded = false, bool fullIntensity = true)
	{
		if (!_vibrationsActive)
		{
			return;
		}
		DebugLog("[MMVibrationManager] Continuous Haptic");
		if (Android() && vibrateAndroid)
		{
			if (!MMNVAndroid.AndroidHasAmplitudeControl() && !vibrateAndroidIfNoSupport)
			{
				return;
			}
			androidIntensity = Remap(androidIntensity, 0f, 1f, 0f, 255f);
			MMNVAndroid.AndroidVibrate((long)(duration * 1000f), (int)androidIntensity);
		}
		else if (iOS() && vibrateiOS)
		{
			if (iOSVersion >= 13f && HapticsSupported())
			{
				MMNViOSCoreHaptics.PlayContinuousHapticPattern(iOSIntensity, iOSSharpness, duration, mono, threaded, fullIntensity);
				_hapticsPlayedOnce = true;
			}
			else
			{
				MMNViOS.iOSTriggerHaptics(fallbackOldiOS);
			}
		}
		if (rumble)
		{
			_ = mono != null;
		}
	}

	public static void UpdateContinuousHaptic(float intensity, float sharpness, bool alsoRumble = false, int controllerID = -1, bool threaded = false)
	{
		UpdateContinuousHaptic(ios: true, intensity, sharpness, android: true, intensity, sharpness, alsoRumble, intensity, sharpness, controllerID, threaded);
	}

	public static void UpdateContinuousHaptic(bool ios, float iosIntensity, float iosSharpness, bool android, float androidIntensity, float androidSharpness, bool rumble, float rumbleLowFrequency, float rumbleHighFrequency, int controllerID = -1, bool threaded = false)
	{
		if (iOS() && ios && iOSVersion >= 13f && HapticsSupported())
		{
			MMNViOSCoreHaptics.UpdateContinuousHapticPattern(iosIntensity, iosSharpness, threaded);
			_hapticsPlayedOnce = true;
		}
	}

	public static void StopAllHaptics(bool alsoRumble = false)
	{
		if (_hapticsPlayedOnce)
		{
			DebugLog("[MMVibrationManager] Stop all haptics");
			MMNViOSCoreHaptics.StopEngine();
			MMNVAndroid.AndroidCancelVibrations();
		}
	}

	public static void StopContinuousHaptic(bool alsoRumble = false)
	{
		DebugLog("[MMVibrationManager] Stop Continuous Haptic");
		MMNViOSCoreHaptics.StopHapticPatterns();
		MMNVAndroid.AndroidCancelVibrations();
	}

	public static void AdvancedHapticPattern(string iOSJSONString, long[] androidPattern, int[] androidAmplitudes, int androidRepeat, long[] rumblePattern, int[] rumbleLowFreqAmplitudes, int[] rumbleHighFreqAmplitudes, int rumbleRepeat, HapticTypes fallbackOldiOS = HapticTypes.None, MonoBehaviour coroutineSupport = null, int controllerID = -1, bool threaded = false)
	{
		AdvancedHapticPattern(ios: true, iOSJSONString, android: true, androidPattern, androidAmplitudes, androidRepeat, vibrateAndroidIfNoSupport: false, rumble: true, rumblePattern, rumbleLowFreqAmplitudes, rumbleHighFreqAmplitudes, rumbleRepeat, fallbackOldiOS, coroutineSupport, controllerID, threaded);
	}

	public static void AdvancedHapticPattern(bool ios, string iOSJSONString, bool android, long[] androidPattern, int[] androidAmplitudes, int androidRepeat, bool vibrateAndroidIfNoSupport, bool rumble, long[] rumblePattern, int[] rumbleLowFreqAmplitudes, int[] rumbleHighFreqAmplitudes, int rumbleRepeat, HapticTypes fallbackOldiOS = HapticTypes.None, MonoBehaviour coroutineSupport = null, int controllerID = -1, bool threaded = false)
	{
		if (!_vibrationsActive)
		{
			return;
		}
		DebugLog("[MMVibrationManager] Advanced Haptic Pattern");
		if (Android())
		{
			if (MMNVAndroid.AndroidHasAmplitudeControl() || vibrateAndroidIfNoSupport)
			{
				MMNVAndroid.AndroidVibrate(androidPattern, androidAmplitudes, androidRepeat, threaded);
			}
		}
		else if (iOS())
		{
			if (iOSVersion >= 13f && HapticsSupported())
			{
				MMNViOSCoreHaptics.PlayCoreHapticsFromJSON(iOSJSONString, threaded);
				_hapticsPlayedOnce = true;
			}
			else
			{
				MMNViOS.iOSTriggerHaptics(fallbackOldiOS);
			}
		}
	}

	private static void DebugLog(string log)
	{
		if (_debugLogActive)
		{
			Debug.Log(log);
		}
	}

	public static float Remap(float x, float A, float B, float C, float D)
	{
		return C + (x - A) / (B - A) * (D - C);
	}
}
