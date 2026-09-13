using UnityEngine;

namespace Tayx.Graphy.Battery;

public class Battery
{
	private static object[] PARAM_BATTERY;

	private static AndroidJavaObject manager;

	public static float electricity => ToMA(manager.Call<int>("getIntProperty", PARAM_BATTERY));

	public static float voltage { get; private set; }

	public static int capacity { get; private set; }

	static Battery()
	{
		PARAM_BATTERY = new object[1] { 2 };
		AndroidJavaObject androidJavaObject = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
		manager = androidJavaObject.Call<AndroidJavaObject>("getSystemService", new object[1] { "batterymanager" });
		capacity = (int)(ToMA(manager.Call<int>("getIntProperty", new object[1] { 1 })) / ((float)manager.Call<int>("getIntProperty", new object[1] { 4 }) / 100f));
		AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("registerReceiver", new object[2]
		{
			null,
			new AndroidJavaObject("android.content.IntentFilter", "android.intent.action.BATTERY_CHANGED")
		});
		if (androidJavaObject2 != null)
		{
			voltage = (float)androidJavaObject2.Call<int>("getIntExtra", new object[2] { "voltage", 0 }) / 1000f;
		}
	}

	private static float ToMA(float maOrua)
	{
		if (!(maOrua < 10000f))
		{
			return maOrua / 1000f;
		}
		return maOrua;
	}
}
