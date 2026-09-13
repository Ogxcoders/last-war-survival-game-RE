using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;

namespace RiverBISDK;

public class BIConfig
{
	private const string BI_TEMP_ID_MEMORY_CACHE_KEY = "BI_TEMP_ID";

	public static int maxQueueList = 10;

	public static int fileMaxLine = 100;

	public static string logFilePath = "";

	public static string tempIdFilePath = "";

	public static int fileHeartTime = 10000;

	public static int timeOut = 5000;

	public static string networkStatus = "";

	public static object systemInfo = "";

	public static long queueNum = 1L;

	public static long index = 0L;

	public static string sdkVersion = "0.0.1";

	public static bool isDebug = false;

	public static long beginGameTime = 0L;

	public static long totalHideGameTime = 0L;

	public static long lastShowGameTime = 0L;

	public static long lastHideGameTime = 0L;

	public static string postUrl = "https://bi-tracker-global.rivergame.net/event/tracker";

	public static string backUrl = "";

	public static string appId = "999";

	public static string deviceId = "";

	public static string airKey = "";

	public static string tempId = "";

	public static string oldTempId = "";

	public static string uid = "";

	public static string sid = "";

	public static string platform = "";

	public static string appVersion = "";

	public static string pfVersion = "";

	public static string channel = "";

	public static string unionid = "";

	public static string country = "";

	public static int firstLaunch = 0;

	public static int hostProcessId = -1;

	private static Dictionary<string, object> _commonData = new Dictionary<string, object>();

	private static string GetStringValueFromDic(IDictionary<string, object> dictionary, string key, string defaultVal = "")
	{
		if (dictionary.TryGetValue(key, out var value))
		{
			if (value == null)
			{
				return defaultVal;
			}
			return value.ToString();
		}
		return defaultVal;
	}

	private static T GetPrimitiveValueFromDic<T>(IDictionary<string, object> dictionary, string key, T defaultVal = default(T)) where T : struct
	{
		if (dictionary.TryGetValue(key, out var value))
		{
			if (value != null)
			{
				return (T)value;
			}
			return defaultVal;
		}
		return defaultVal;
	}

	public static void InitConfig(IDictionary<string, object> initInfoDict, bool firstLaunch)
	{
		oldTempId = GameUtility.GetGameSessionId();
		hostProcessId = -1;
		platform = GetStringValueFromDic(initInfoDict, "platform", "unity_editor");
		appId = GetStringValueFromDic(initInfoDict, "app_id", "999");
		channel = GetStringValueFromDic(initInfoDict, "channel");
		country = GetStringValueFromDic(initInfoDict, "country");
		isDebug = GetPrimitiveValueFromDic(initInfoDict, "debug", defaultVal: false);
		backUrl = GetStringValueFromDic(initInfoDict, "back_url");
		appVersion = GetStringValueFromDic(initInfoDict, "app_version");
		pfVersion = GetStringValueFromDic(initInfoDict, "pf_version");
		networkStatus = NetWorkStatus();
		beginGameTime = GetTimeStamp();
		systemInfo = GetSystemInfo();
		logFilePath = Application.persistentDataPath + "/BIStorage/riverBiLog.txt";
		tempIdFilePath = Application.persistentDataPath + "/BIStorage/riverBiTempId.txt";
		tempId = GetTempId();
		BIConfig.firstLaunch = (firstLaunch ? 1 : 0);
	}

	private static string GetTempId()
	{
		string tempIdFromFile = TextFile.GetTempIdFromFile();
		if (!string.IsNullOrWhiteSpace(tempIdFromFile))
		{
			return tempIdFromFile;
		}
		tempIdFromFile = Guid.NewGuid().ToString();
		TextFile.WriteTempIdToFile(tempIdFromFile);
		return tempIdFromFile;
	}

	public static void UpdateGameInfo(IDictionary<string, object> gameInfoDict)
	{
		if (gameInfoDict.ContainsKey("uid"))
		{
			uid = GetStringValueFromDic(gameInfoDict, "uid");
		}
		if (gameInfoDict.ContainsKey("device_id"))
		{
			deviceId = GetStringValueFromDic(gameInfoDict, "device_id");
		}
		if (gameInfoDict.ContainsKey("airKey"))
		{
			airKey = GetStringValueFromDic(gameInfoDict, "airKey");
		}
		if (gameInfoDict.ContainsKey("sid"))
		{
			sid = GetStringValueFromDic(gameInfoDict, "sid");
		}
		if (gameInfoDict.ContainsKey("app_version"))
		{
			appVersion = GetStringValueFromDic(gameInfoDict, "app_version");
		}
		if (gameInfoDict.ContainsKey("unionid"))
		{
			unionid = GetStringValueFromDic(gameInfoDict, "unionid");
		}
		if (gameInfoDict.ContainsKey("channel"))
		{
			channel = GetStringValueFromDic(gameInfoDict, "channel");
		}
		if (gameInfoDict.ContainsKey("country"))
		{
			country = GetStringValueFromDic(gameInfoDict, "country");
		}
		if (gameInfoDict.ContainsKey("platform"))
		{
			platform = GetStringValueFromDic(gameInfoDict, "platform");
		}
	}

	public static Dictionary<string, object> GetCommonData()
	{
		index++;
		_commonData["temp_id"] = tempId;
		_commonData["platform"] = platform;
		_commonData["qn"] = index;
		_commonData["time"] = GetTimeStamp();
		_commonData["network_type"] = networkStatus;
		_commonData["debug"] = (isDebug ? 1 : 0);
		_commonData["uid"] = uid;
		_commonData["device_id"] = deviceId;
		_commonData["airKey"] = airKey;
		_commonData["sid"] = sid;
		_commonData["app_version"] = appVersion;
		return _commonData;
	}

	public static string NetWorkStatus()
	{
		string result = "";
		switch (Application.internetReachability)
		{
		case NetworkReachability.NotReachable:
			result = "";
			break;
		case NetworkReachability.ReachableViaCarrierDataNetwork:
			result = "3G/4G";
			break;
		case NetworkReachability.ReachableViaLocalAreaNetwork:
			result = "wifi";
			break;
		}
		return result;
	}

	public static object GetSystemInfo()
	{
		try
		{
			return new Hashtable
			{
				{
					"model",
					SystemInfo.deviceModel
				},
				{
					"brand",
					SystemInfo.deviceName
				},
				{
					"deviceType",
					SystemInfo.deviceType
				},
				{
					"npotSupport",
					SystemInfo.npotSupport
				},
				{
					"processorCount",
					SystemInfo.processorCount
				},
				{
					"system",
					SystemInfo.operatingSystem
				},
				{
					"processorType",
					SystemInfo.processorType
				},
				{
					"supportsVibration",
					SystemInfo.supportsVibration
				},
				{
					"systemMemorySize",
					SystemInfo.systemMemorySize
				}
			};
		}
		catch (Exception message)
		{
			Log.Error(message);
		}
		return "";
	}

	public static void UpdateTotalHideTime()
	{
		totalHideGameTime += ((lastShowGameTime > lastHideGameTime && lastHideGameTime != 0L) ? (lastShowGameTime - lastHideGameTime) : 0);
	}

	public static long GetTimeStamp()
	{
		return GameEntryProxy.Timer.GetServerTime();
	}
}
