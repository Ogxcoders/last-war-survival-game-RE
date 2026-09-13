using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using ThinkingSDK.PC.Constant;
using ThinkingSDK.PC.Storage;
using UnityEngine;

namespace ThinkingSDK.PC.Utils;

public class ThinkingSDKUtil
{
	public static List<string> DisPresetProperties = GetDisPresetProperties();

	public static bool IsValiadURL(string url)
	{
		if (url != null && url.Length != 0 && url.Contains("http"))
		{
			return url.Contains("https");
		}
		return false;
	}

	public static bool IsEmptyString(string str)
	{
		if (str != null)
		{
			return str.Length == 0;
		}
		return true;
	}

	public static Dictionary<string, object> DeviceInfo()
	{
		return new Dictionary<string, object>
		{
			[ThinkingSDKConstant.DEVICE_ID] = ThinkingSDKDeviceInfo.DeviceID(),
			[ThinkingSDKConstant.LIB_VERSION] = ThinkingSDKAppInfo.LibVersion(),
			[ThinkingSDKConstant.LIB] = ThinkingSDKAppInfo.LibName(),
			[ThinkingSDKConstant.OS] = ThinkingSDKDeviceInfo.OS(),
			[ThinkingSDKConstant.SCREEN_HEIGHT] = ThinkingSDKDeviceInfo.ScreenHeight(),
			[ThinkingSDKConstant.SCREEN_WIDTH] = ThinkingSDKDeviceInfo.ScreenWidth(),
			[ThinkingSDKConstant.MANUFACTURE] = ThinkingSDKDeviceInfo.Manufacture(),
			[ThinkingSDKConstant.DEVICE_MODEL] = ThinkingSDKDeviceInfo.DeviceModel(),
			[ThinkingSDKConstant.SYSTEM_LANGUAGE] = ThinkingSDKDeviceInfo.MachineLanguage(),
			[ThinkingSDKConstant.OS_VERSION] = ThinkingSDKDeviceInfo.OSVersion(),
			[ThinkingSDKConstant.APP_VERSION] = ThinkingSDKAppInfo.AppVersion(),
			[ThinkingSDKConstant.NETWORK_TYPE] = ThinkingSDKDeviceInfo.NetworkType(),
			[ThinkingSDKConstant.APP_BUNDLEID] = ThinkingSDKAppInfo.AppIdentifier()
		};
	}

	private static List<string> GetDisPresetProperties()
	{
		List<string> list = new List<string>();
		TextAsset textAsset = Resources.Load<TextAsset>("ta_public_config");
		if (textAsset != null && textAsset.text != null)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(textAsset.text);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("resources");
			for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
			{
				XmlNode xmlNode2 = xmlNode.ChildNodes[i];
				if (xmlNode2.NodeType != XmlNodeType.Element)
				{
					continue;
				}
				XmlElement xmlElement = xmlNode2 as XmlElement;
				if (!xmlElement.HasAttributes || !(xmlElement.GetAttribute("name") == "TDDisPresetProperties") || !xmlElement.HasChildNodes)
				{
					continue;
				}
				for (int j = 0; j < xmlElement.ChildNodes.Count; j++)
				{
					XmlNode xmlNode3 = xmlElement.ChildNodes[j];
					if (xmlNode3.NodeType == XmlNodeType.Element)
					{
						list.Add(xmlNode3.InnerText);
					}
				}
			}
		}
		return list;
	}

	public static string RandomID(bool persistent = true)
	{
		string text = null;
		if (persistent)
		{
			text = (string)ThinkingSDKFile.GetData(ThinkingSDKConstant.RANDOM_ID, typeof(string));
		}
		if (string.IsNullOrEmpty(text))
		{
			text = Guid.NewGuid().ToString("N");
			if (persistent)
			{
				ThinkingSDKFile.SaveData(ThinkingSDKConstant.RANDOM_ID, text);
			}
		}
		return text;
	}

	public static double ZoneOffset(DateTime dateTime, TimeZoneInfo timeZone)
	{
		bool flag = true;
		TimeSpan timeSpan = default(TimeSpan);
		try
		{
			timeSpan = timeZone.BaseUtcOffset;
		}
		catch (Exception)
		{
			flag = false;
		}
		try
		{
			if (timeZone.IsDaylightSavingTime(dateTime))
			{
				TimeSpan ts = TimeSpan.FromHours(1.0);
				timeSpan = timeSpan.Add(ts);
			}
		}
		catch (Exception)
		{
			flag = false;
		}
		if (!flag)
		{
			timeSpan = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);
		}
		return timeSpan.TotalHours;
	}

	public static string FormatDate(DateTime dateTime, TimeZoneInfo timeZone)
	{
		bool flag = true;
		DateTime dateTime2 = dateTime.ToUniversalTime();
		TimeSpan timeSpan = default(TimeSpan);
		try
		{
			timeSpan = timeZone.BaseUtcOffset;
		}
		catch (Exception)
		{
			flag = false;
		}
		try
		{
			if (timeZone.IsDaylightSavingTime(dateTime))
			{
				TimeSpan ts = TimeSpan.FromHours(1.0);
				timeSpan = timeSpan.Add(ts);
			}
		}
		catch (Exception)
		{
			flag = false;
		}
		if (!flag)
		{
			timeSpan = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);
		}
		return (dateTime2 + timeSpan).ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
	}

	public static void AddDictionary(Dictionary<string, object> originalDic, Dictionary<string, object> subDic)
	{
		foreach (KeyValuePair<string, object> item in subDic)
		{
			originalDic[item.Key] = item.Value;
		}
	}

	public static long GetTimeStamp()
	{
		return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds);
	}
}
