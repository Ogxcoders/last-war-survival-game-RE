using System;
using ThinkingSDK.PC.Constant;
using ThinkingSDK.PC.Storage;
using UnityEngine;

namespace ThinkingSDK.PC.Utils;

public class ThinkingSDKDeviceInfo
{
	public static string DeviceID()
	{
		if (ThinkingSDKUtil.DisPresetProperties.Contains(ThinkingSDKConstant.DEVICE_ID))
		{
			return "";
		}
		return SystemInfo.deviceUniqueIdentifier;
	}

	private static string RandomDeviceID()
	{
		string text = (string)ThinkingSDKFile.GetData(ThinkingSDKConstant.RANDOM_DEVICE_ID, typeof(string));
		if (string.IsNullOrEmpty(text))
		{
			text = Guid.NewGuid().ToString("N");
			ThinkingSDKFile.SaveData(ThinkingSDKConstant.RANDOM_DEVICE_ID, text);
		}
		return text;
	}

	public static string NetworkType()
	{
		if (ThinkingSDKUtil.DisPresetProperties.Contains(ThinkingSDKConstant.NETWORK_TYPE))
		{
			return "";
		}
		string result = "NULL";
		if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
		{
			result = "Mobile";
		}
		else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
		{
			result = "LAN";
		}
		return result;
	}

	public static string Carrier()
	{
		if (ThinkingSDKUtil.DisPresetProperties.Contains(ThinkingSDKConstant.CARRIER))
		{
			return "";
		}
		return "NULL";
	}

	public static string OS()
	{
		if (ThinkingSDKUtil.DisPresetProperties.Contains(ThinkingSDKConstant.OS))
		{
			return "";
		}
		string result = "other";
		if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Linux)
		{
			result = "Linux";
		}
		else if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX)
		{
			result = "MacOSX";
		}
		else if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows)
		{
			result = "Windows";
		}
		return result;
	}

	public static string OSVersion()
	{
		if (ThinkingSDKUtil.DisPresetProperties.Contains(ThinkingSDKConstant.OS_VERSION))
		{
			return "";
		}
		return SystemInfo.operatingSystem;
	}

	public static int ScreenWidth()
	{
		if (ThinkingSDKUtil.DisPresetProperties.Contains(ThinkingSDKConstant.SCREEN_WIDTH))
		{
			return 0;
		}
		return Screen.currentResolution.width;
	}

	public static int ScreenHeight()
	{
		if (ThinkingSDKUtil.DisPresetProperties.Contains(ThinkingSDKConstant.SCREEN_HEIGHT))
		{
			return 0;
		}
		return Screen.currentResolution.height;
	}

	public static string Manufacture()
	{
		if (ThinkingSDKUtil.DisPresetProperties.Contains(ThinkingSDKConstant.MANUFACTURE))
		{
			return "";
		}
		return SystemInfo.graphicsDeviceVendor;
	}

	public static string DeviceModel()
	{
		if (ThinkingSDKUtil.DisPresetProperties.Contains(ThinkingSDKConstant.DEVICE_MODEL))
		{
			return "";
		}
		return SystemInfo.deviceModel;
	}

	public static string MachineLanguage()
	{
		if (ThinkingSDKUtil.DisPresetProperties.Contains(ThinkingSDKConstant.SYSTEM_LANGUAGE))
		{
			return "";
		}
		return Application.systemLanguage switch
		{
			SystemLanguage.Afrikaans => "af", 
			SystemLanguage.Arabic => "ar", 
			SystemLanguage.Basque => "eu", 
			SystemLanguage.Belarusian => "be", 
			SystemLanguage.Bulgarian => "bg", 
			SystemLanguage.Catalan => "ca", 
			SystemLanguage.Chinese => "zh", 
			SystemLanguage.Czech => "cs", 
			SystemLanguage.Danish => "da", 
			SystemLanguage.Dutch => "nl", 
			SystemLanguage.English => "en", 
			SystemLanguage.Estonian => "et", 
			SystemLanguage.Faroese => "fo", 
			SystemLanguage.Finnish => "fu", 
			SystemLanguage.French => "fr", 
			SystemLanguage.German => "de", 
			SystemLanguage.Greek => "el", 
			SystemLanguage.Hebrew => "he", 
			SystemLanguage.Icelandic => "is", 
			SystemLanguage.Indonesian => "id", 
			SystemLanguage.Italian => "it", 
			SystemLanguage.Japanese => "ja", 
			SystemLanguage.Korean => "ko", 
			SystemLanguage.Latvian => "lv", 
			SystemLanguage.Lithuanian => "lt", 
			SystemLanguage.Norwegian => "nn", 
			SystemLanguage.Polish => "pl", 
			SystemLanguage.Portuguese => "pt", 
			SystemLanguage.Romanian => "ro", 
			SystemLanguage.Russian => "ru", 
			SystemLanguage.SerboCroatian => "sr", 
			SystemLanguage.Slovak => "sk", 
			SystemLanguage.Slovenian => "sl", 
			SystemLanguage.Spanish => "es", 
			SystemLanguage.Swedish => "sv", 
			SystemLanguage.Thai => "th", 
			SystemLanguage.Turkish => "tr", 
			SystemLanguage.Ukrainian => "uk", 
			SystemLanguage.Vietnamese => "vi", 
			SystemLanguage.ChineseSimplified => "zh", 
			SystemLanguage.ChineseTraditional => "zh", 
			SystemLanguage.Hungarian => "hu", 
			SystemLanguage.Unknown => "unknown", 
			_ => "", 
		};
	}
}
