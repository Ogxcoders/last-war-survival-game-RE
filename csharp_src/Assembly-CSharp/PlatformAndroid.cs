using System;
using GameFramework;
using UnityEngine;

public class PlatformAndroid : AndroidJavaProxy, IPlatformNative
{
	public static class BillingResponseCode
	{
		public const int SERVICE_TIMEOUT = -3;

		public const int FEATURE_NOT_SUPPORTED = -2;

		public const int SERVICE_DISCONNECTED = -1;

		public const int OK = 0;

		public const int USER_CANCELED = 1;

		public const int SERVICE_UNAVAILABLE = 2;

		public const int BILLING_UNAVAILABLE = 3;

		public const int ITEM_UNAVAILABLE = 4;

		public const int DEVELOPER_ERROR = 5;

		public const int ERROR = 6;

		public const int ITEM_ALREADY_OWNED = 7;

		public const int ITEM_NOT_OWNED = 8;
	}

	private AndroidJavaObject currentActivity;

	public const string GOOGLE_PUBLIC_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAiYVPWSTL1D2+Rbyx9uyAx6PlPuXGGufEqInq1LfN3Thu7g6XYniKB0zhUKElEhSHxKMHJSJYK7TpUx6NGmW075+VHngPNHa94wZakn/pUvwoumRad/FJUoTlk3JeNfemlcUHwSNyzrykt1tCYqakgsQzETZY/Bp0C+NjOVzZFqMqZkmHy+n7IJx1GVOU9AW9HHaUmqAvPDLsolpKDoPw5KN1XIw+Z04HDjgpG3m9p+YaDNuUUWcJu12oQ+2qTrwzZSwwLe/hlG6uFciD6aar7/rbXydFZjYGQVeMMj4YQhe4T7ROEq6tlobn2yEeeJw8JMj92K46XbVeCMnJK+Vv3wIDAQAB";

	public const string GOOGLE_PROJ_NUM = "571473307808";

	public bool HasSignedIn { get; set; }

	public string UID { get; set; }

	public GamePlatform ID => GamePlatform.GooglePlay;

	public PaymentChannel PaymentChannel => PaymentChannel.GooglePay;

	public LoginPlatform LoginPlatform { get; set; }

	public PlatformAndroid(string listenerClassName)
		: base(listenerClassName)
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		currentActivity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
	}

	public string GetLaunchPushID()
	{
		AndroidJavaObject androidJavaObject = currentActivity.Call<AndroidJavaObject>("getIntent", Array.Empty<object>());
		if (androidJavaObject != null && androidJavaObject.Call<bool>("hasExtra", new object[1] { "pushid" }))
		{
			return androidJavaObject.Call<string>("getStringExtra", new object[1] { "pushid" });
		}
		return string.Empty;
	}

	public void Call(string funcName, params object[] args)
	{
		try
		{
			if (currentActivity != null)
			{
				currentActivity.Call(funcName, args);
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	public T Call<T>(string funcName, params object[] args)
	{
		T result = default(T);
		try
		{
			if (currentActivity != null)
			{
				result = currentActivity.Call<T>(funcName, args);
				return result;
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		return result;
	}

	public void CallStatic(string funcName, params object[] args)
	{
		try
		{
			if (currentActivity != null)
			{
				currentActivity.CallStatic(funcName, args);
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	public T CallStatic<T>(string funcName, params object[] args)
	{
		T result = default(T);
		try
		{
			if (currentActivity != null)
			{
				result = currentActivity.CallStatic<T>(funcName, args);
				return result;
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		return result;
	}

	public T Get<T>(string fieldName)
	{
		try
		{
			if (currentActivity != null)
			{
				return currentActivity.Get<T>(fieldName);
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		return default(T);
	}

	public T GetStatic<T>(string fieldName)
	{
		try
		{
			if (currentActivity != null)
			{
				return currentActivity.GetStatic<T>(fieldName);
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		return default(T);
	}

	public void SendDataToGame(string funcName, string data)
	{
		GameEntry.Sdk.SendDataToGame(funcName, data);
	}

	public object GetDataFromGame(string funcName, string data)
	{
		object obj = null;
		try
		{
			switch (funcName)
			{
			case "getPlatform":
				obj = ID.ToString();
				break;
			case "getGpk":
				obj = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAiYVPWSTL1D2+Rbyx9uyAx6PlPuXGGufEqInq1LfN3Thu7g6XYniKB0zhUKElEhSHxKMHJSJYK7TpUx6NGmW075+VHngPNHa94wZakn/pUvwoumRad/FJUoTlk3JeNfemlcUHwSNyzrykt1tCYqakgsQzETZY/Bp0C+NjOVzZFqMqZkmHy+n7IJx1GVOU9AW9HHaUmqAvPDLsolpKDoPw5KN1XIw+Z04HDjgpG3m9p+YaDNuUUWcJu12oQ+2qTrwzZSwwLe/hlG6uFciD6aar7/rbXydFZjYGQVeMMj4YQhe4T7ROEq6tlobn2yEeeJw8JMj92K46XbVeCMnJK+Vv3wIDAQAB";
				break;
			case "getBuglyId":
				obj = "";
				break;
			}
		}
		catch (Exception message)
		{
			Log.Error(message);
		}
		if (obj == null)
		{
			Log.Error("GetDataFromGame: funcName = {0}, data = {1}, ret is null!", funcName, data);
		}
		else
		{
			Log.Info("GetDataFromGame: funcName = {0}, data = {1}, ret = {2}", funcName, data, obj);
		}
		return obj;
	}

	public void SendDataToNative(string funcName, string data)
	{
		Call("SendDataToNative", funcName, data);
	}

	public string GetDataFromNative(string funcName, string data)
	{
		string text = Call<string>("GetDataFromNative", new object[2] { funcName, data });
		if (string.IsNullOrEmpty(text))
		{
			text = "";
		}
		return text;
	}

	public void InitPlatform(string proxyName)
	{
		Call("InitPlatform", proxyName, this);
	}

	public void SignIn(string json)
	{
		Call("SignIn", json);
	}

	public void SignOut()
	{
		Call("SignOut");
	}

	public void Pay(int channelId, string json)
	{
		Call("Pay", channelId, json);
	}

	public void QueryPurchaseOrder()
	{
		SendDataToNative("Pay_queryPurchase", "");
	}

	public void ConsumeProduct(string orderId, int status)
	{
		Call("ConsumeProduct", orderId, status);
	}

	public string GetPermissionByType(string data)
	{
		return GetDataFromNative("PM_GetPermit", data);
	}

	public void Restart(string data)
	{
		Call("restart", data);
	}

	public string GetRestartData()
	{
		return Call<string>("getRestartData", Array.Empty<object>());
	}

	public long GetRealtime()
	{
		return CallStatic<long>("getElapsedRealtime", Array.Empty<object>());
	}
}
