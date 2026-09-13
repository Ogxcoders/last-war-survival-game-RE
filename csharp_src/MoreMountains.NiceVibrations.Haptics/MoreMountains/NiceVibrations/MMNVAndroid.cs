using System;
using UnityEngine;

namespace MoreMountains.NiceVibrations;

public static class MMNVAndroid
{
	public struct MMNVAndroidVibrateThreadData
	{
		public long[] Pattern;

		public int[] Amplitudes;

		public int Repeat;

		public MMNVAndroidVibrateThreadData(long[] pattern, int[] amplitudes, int repeat)
		{
			Pattern = pattern;
			Amplitudes = amplitudes;
			Repeat = repeat;
		}
	}

	private static MMNVAltThread<MMNVAndroidVibrateThreadData> _androidVibrateThread;

	private static MMNVAndroidVibrateThreadData _androidVibrateThreadData;

	private static int _sdkVersion = -1;

	private static AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

	private static AndroidJavaObject CurrentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

	private static AndroidJavaObject AndroidVibrator = CurrentActivity.Call<AndroidJavaObject>("getSystemService", new object[1] { "vibrator" });

	private static AndroidJavaClass VibrationEffectClass;

	private static AndroidJavaObject VibrationEffect;

	private static int DefaultAmplitude;

	private static IntPtr AndroidVibrateMethodRawClass = AndroidJNIHelper.GetMethodID(AndroidVibrator.GetRawClass(), "vibrate", "(J)V", isStatic: false);

	private static jvalue[] AndroidVibrateMethodRawClassParameters = new jvalue[1];

	public static void AndroidVibrate(long milliseconds)
	{
		if (MMNVPlatform.Android())
		{
			AndroidVibrateMethodRawClassParameters[0].j = milliseconds;
			AndroidJNI.CallVoidMethod(AndroidVibrator.GetRawObject(), AndroidVibrateMethodRawClass, AndroidVibrateMethodRawClassParameters);
		}
	}

	public static void AndroidVibrate(long milliseconds, int amplitude)
	{
		if (MMNVPlatform.Android())
		{
			if (AndroidSDKVersion() < 26)
			{
				AndroidVibrate(milliseconds);
				return;
			}
			AndroidVibrationEffectClassInitialization();
			VibrationEffect = VibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", new object[2] { milliseconds, amplitude });
			AndroidVibrator.Call("vibrate", VibrationEffect);
		}
	}

	public static void AndroidVibrate(long[] pattern, int repeat)
	{
		if (!MMNVPlatform.Android() || pattern == null)
		{
			return;
		}
		if (AndroidSDKVersion() < 26)
		{
			AndroidVibrator.Call("vibrate", pattern, repeat);
			return;
		}
		AndroidVibrationEffectClassInitialization();
		try
		{
			VibrationEffect = VibrationEffectClass.CallStatic<AndroidJavaObject>("createWaveform", new object[2] { pattern, repeat });
			AndroidVibrator.Call("vibrate", VibrationEffect);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	public static void AndroidVibrate(long[] pattern, int[] amplitudes, int repeat, bool threaded = false)
	{
		if (!MMNVPlatform.Android() || pattern == null || amplitudes == null || pattern.Length == 0 || amplitudes.Length == 0)
		{
			return;
		}
		if (AndroidSDKVersion() < 26)
		{
			AndroidVibrator.Call("vibrate", pattern, repeat);
		}
		else if (threaded)
		{
			if (_androidVibrateThread == null)
			{
				_androidVibrateThread = new MMNVAltThread<MMNVAndroidVibrateThreadData>();
				_androidVibrateThreadData = default(MMNVAndroidVibrateThreadData);
			}
			_androidVibrateThreadData.Pattern = pattern;
			_androidVibrateThreadData.Amplitudes = amplitudes;
			_androidVibrateThreadData.Repeat = repeat;
			_androidVibrateThread.Run(AndroidVibrateThread, _androidVibrateThreadData);
		}
		else
		{
			AndroidVibrateNoThread(pattern, amplitudes, repeat);
		}
	}

	private static void AndroidVibrateThread(MMNVAndroidVibrateThreadData threadData)
	{
		AndroidJNI.AttachCurrentThread();
		AndroidVibrateNoThread(threadData.Pattern, threadData.Amplitudes, threadData.Repeat);
		AndroidJNI.DetachCurrentThread();
	}

	private static void AndroidVibrateNoThread(long[] pattern, int[] amplitudes, int repeat)
	{
		if (pattern != null && amplitudes != null)
		{
			AndroidVibrationEffectClassInitialization();
			VibrationEffect = VibrationEffectClass.CallStatic<AndroidJavaObject>("createWaveform", new object[3] { pattern, amplitudes, repeat });
			AndroidVibrator.Call("vibrate", VibrationEffect);
		}
	}

	public static void ClearThreads()
	{
		_androidVibrateThread = null;
		_androidVibrateThread?.CloseThread();
	}

	public static void AndroidCancelVibrations()
	{
		if (MMNVPlatform.Android())
		{
			AndroidVibrator.Call("cancel");
		}
	}

	public static bool AndroidHasVibrator()
	{
		if (!MMNVPlatform.Android())
		{
			return false;
		}
		return AndroidVibrator.Call<bool>("hasVibrator", Array.Empty<object>());
	}

	public static bool AndroidHasAmplitudeControl()
	{
		if (AndroidSDKVersion() < 26)
		{
			return false;
		}
		if (!MMNVPlatform.Android())
		{
			return false;
		}
		return AndroidVibrator.Call<bool>("hasAmplitudeControl", Array.Empty<object>());
	}

	private static void AndroidVibrationEffectClassInitialization()
	{
		if (VibrationEffectClass == null)
		{
			VibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
		}
	}

	public static int AndroidSDKVersion()
	{
		if (_sdkVersion == -1)
		{
			return _sdkVersion = int.Parse(SystemInfo.operatingSystem.Substring(SystemInfo.operatingSystem.IndexOf("-") + 1, 3));
		}
		return _sdkVersion;
	}
}
