using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine;

public class DeviceManager
{
	private string m_strDeviceUid = "";

	private HashSet<string> _dma_counrtys = new HashSet<string>
	{
		"AT", "BE", "BG", "CY", "CZ", "DE", "DK", "EE", "ES", "FI",
		"FR", "GR", "HR", "HU", "IE", "IT", "LT", "LU", "LV", "MT",
		"NL", "PL", "PT", "RO", "SE", "SI", "SK", "UK", "LI", "NO",
		"IS"
	};

	public int GetNetworkStatus()
	{
		return GetNetworkType() switch
		{
			NetworkReachability.ReachableViaLocalAreaNetwork => 1, 
			NetworkReachability.ReachableViaCarrierDataNetwork => 2, 
			_ => 0, 
		};
	}

	public NetworkReachability GetNetworkType()
	{
		return Application.internetReachability;
	}

	public string GetNetworkTypeDesc()
	{
		NetworkReachability networkType = GetNetworkType();
		string result = "";
		switch (networkType)
		{
		case NetworkReachability.NotReachable:
			result = "no net";
			break;
		case NetworkReachability.ReachableViaCarrierDataNetwork:
			result = "4g";
			break;
		case NetworkReachability.ReachableViaLocalAreaNetwork:
			result = "wifi";
			break;
		}
		return result;
	}

	public string GetSerialID()
	{
		return GameEntry.Sdk.GetSerialID();
	}

	public string GetDeviceInfo()
	{
		return GameEntry.Sdk.GetDeviceInfo();
	}

	public string GetHandSetInfo()
	{
		return GameEntry.Sdk.GetHandSetInfo();
	}

	public string GetNewAndroidDeviceID()
	{
		return GameEntry.Sdk.GenerateHighVersionUUID();
	}

	public string GetOSVersion()
	{
		return SystemInfo.operatingSystem;
	}

	public float GetBatteryLevel()
	{
		return SystemInfo.batteryLevel;
	}

	public BatteryStatus GetBatteryStatus()
	{
		return SystemInfo.batteryStatus;
	}

	public string GetProcessorType()
	{
		return SystemInfo.processorType;
	}

	public int GetProcessorCount()
	{
		return SystemInfo.processorCount;
	}

	public string GetProcessorFrequency()
	{
		return $"{SystemInfo.processorFrequency} MHz";
	}

	public int GetSystemMemorySize()
	{
		return SystemInfo.systemMemorySize;
	}

	public string GetDeviceModel()
	{
		return SystemInfo.deviceModel;
	}

	public DeviceType GetDeviceType()
	{
		return SystemInfo.deviceType;
	}

	private string GenerateDeviceUniqueId()
	{
		return SystemInfo.deviceUniqueIdentifier + UnityEngine.Random.Range(0, 1000000);
	}

	public string GetDeviceUid()
	{
		if (!m_strDeviceUid.IsNullOrEmpty())
		{
			return m_strDeviceUid;
		}
		string text = GameEntry.Setting.GetString("DEVICE_ID", "");
		if (string.IsNullOrEmpty(text))
		{
			text = GameEntry.Sdk.GetDeviceUDID();
			if (string.IsNullOrEmpty(text))
			{
				Debug.LogWarning("原生层获取设备ID为空,请及时排查~");
				text = GenerateDeviceUniqueId();
			}
			text = ((!CommonUtils.IsDebug()) ? (text + "_n3d") : (text + "_3d"));
			GameEntry.Setting.SetString("DEVICE_ID", text);
		}
		if (SDKManager.IS_IPhonePlayer())
		{
			string text2 = text;
			string text3 = "_n3d";
			string text4 = "_3d";
			if (text2.EndsWith(text3))
			{
				text2 = text2.Substring(0, text2.Length - text3.Length);
			}
			else if (text2.EndsWith(text4))
			{
				text2 = text2.Substring(0, text2.Length - text4.Length);
			}
			Log.Info("save to kc : " + text2);
			GameEntry.Sdk.SendDataToNative("SAVE_DEVICE_ID_TO_KEY_CHAIN", text2);
		}
		m_strDeviceUid = text;
		return text;
	}

	public string GetDeviceUid_Transcoding()
	{
		string deviceUid = GetDeviceUid();
		return SetDeviceUidToTranscoding(deviceUid);
	}

	public string SetDeviceUidToTranscoding(string deviceId)
	{
		string text = Convert.ToBase64String(Encoding.UTF8.GetBytes(deviceId));
		return "lwDid_" + text;
	}

	public string GetDeviceString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("Model={0}|", SystemInfo.deviceModel);
		stringBuilder.AppendFormat("Memory={0}|", SystemInfo.systemMemorySize);
		stringBuilder.AppendFormat("Vendor={0}|", SystemInfo.graphicsDeviceVendor);
		stringBuilder.AppendFormat("Processor={0}|", SystemInfo.processorFrequency);
		stringBuilder.AppendFormat("Graphics={0}|", SystemInfo.graphicsDeviceName);
		stringBuilder.AppendFormat("DeviceType={0}|", getGraphicsDeviceType());
		return stringBuilder.ToString();
	}

	public int GetOpenGL()
	{
		int result = 0;
		try
		{
			using AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			using AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			using AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getApplication", Array.Empty<object>());
			using AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("getSystemService", new object[1] { "activity" });
			using AndroidJavaObject androidJavaObject4 = androidJavaObject3.Call<AndroidJavaObject>("getDeviceConfigurationInfo", Array.Empty<object>());
			return androidJavaObject4.Get<int>("reqGlEsVersion");
		}
		catch (Exception)
		{
			return result;
		}
	}

	public string getGraphicsDeviceType()
	{
		try
		{
			int openGL = GetOpenGL();
			int num = openGL >> 16;
			int num2 = openGL & 0xFFFF;
			string value = $"OpenGL{num}.{num2}";
			bool num3 = isSupportDxt();
			bool num4 = IsSupportASTC();
			bool flag = IsSupportEtc2();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(value);
			if (num4)
			{
				stringBuilder.Append("-ASTC");
			}
			if (flag)
			{
				stringBuilder.Append("-ETC2");
			}
			if (num3)
			{
				stringBuilder.Append("-DXT");
			}
			if (SystemInfo.SupportsTextureFormat(TextureFormat.ASTC_4x4))
			{
				stringBuilder.Append("-ASTC_4x4");
			}
			if (SystemInfo.SupportsTextureFormat(TextureFormat.ASTC_6x6))
			{
				stringBuilder.Append("-ASTC_6x6");
			}
			return stringBuilder.ToString();
		}
		catch (Exception value2)
		{
			Console.WriteLine(value2);
		}
		return "";
	}

	public bool isSupportDxt()
	{
		if (!SystemInfo.SupportsTextureFormat(TextureFormat.DXT1) && !SystemInfo.SupportsTextureFormat(TextureFormat.DXT5) && !SystemInfo.SupportsTextureFormat(TextureFormat.DXT1Crunched))
		{
			return SystemInfo.SupportsTextureFormat(TextureFormat.DXT5Crunched);
		}
		return true;
	}

	public bool IsSupportASTC()
	{
		bool flag = false;
		for (TextureFormat textureFormat = TextureFormat.ASTC_4x4; textureFormat <= TextureFormat.ASTC_RGBA_12x12; textureFormat++)
		{
			flag = SystemInfo.SupportsTextureFormat(textureFormat);
			if (!flag)
			{
				return flag;
			}
		}
		return flag;
	}

	public bool IsSupportEtc2()
	{
		bool flag = false;
		for (TextureFormat textureFormat = TextureFormat.ETC2_RGB; textureFormat <= TextureFormat.ETC2_RGBA8; textureFormat++)
		{
			flag = SystemInfo.SupportsTextureFormat(textureFormat);
			if (!flag)
			{
				return flag;
			}
		}
		return flag;
	}

	public bool IsEUCountry()
	{
		if (_dma_counrtys.Contains(GameEntry.GlobalData.fromCountry))
		{
			return true;
		}
		return false;
	}

	public bool IsKR()
	{
		if (GameEntry.GlobalData.fromCountry == "KR")
		{
			return true;
		}
		return false;
	}
}
