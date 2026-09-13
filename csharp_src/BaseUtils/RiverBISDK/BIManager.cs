using System.Collections.Generic;
using GameFramework;
using SFSLitJson;

namespace RiverBISDK;

public class BIManager
{
	public static void InitBI(IDictionary<string, object> initInfoDict, bool firstLaunch)
	{
		BIManagerCore.instance.InitBI(initInfoDict, firstLaunch);
	}

	public static void UpdateResVersion(string resVersion)
	{
		BIManagerCore.instance.UpdateResVersion(resVersion);
	}

	public static void Reset()
	{
		BIManagerCore.instance.Reset();
	}

	public static void OnApplicationPaused(bool paused)
	{
		BIManagerCore.instance.OnApplicationPaused(paused);
	}

	public static void UpdateGameInfo(IDictionary<string, object> gameInfoDict)
	{
		BIManagerCore.instance.UpdateGameInfo(gameInfoDict);
	}

	public static void SendToBI(string eventName, IDictionary<string, object> eventInfoMap)
	{
		BIManagerCore.instance.SendToBI(eventName, eventInfoMap);
	}

	public static void SendToBI(string eventName)
	{
		BIManagerCore.instance.SendToBI(eventName);
	}

	public static void SendToBIFromLua(string eventName, string json)
	{
		if (string.IsNullOrEmpty(json))
		{
			SendToBI(eventName);
			return;
		}
		Dictionary<string, object> eventInfoMap = JsonDataToDict(json);
		if (!string.IsNullOrEmpty(eventName))
		{
			SendToBI(eventName, eventInfoMap);
		}
	}

	private static Dictionary<string, object> JsonDataToDict(string json)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		if (json.Length == 0)
		{
			return dictionary;
		}
		JsonData jsonData = JsonMapper.ToObject(json);
		foreach (string key in jsonData.Keys)
		{
			JsonData jsonData2 = jsonData[key];
			if (jsonData2.IsInt)
			{
				dictionary[key] = (int)jsonData2;
			}
			else if (jsonData2.IsBoolean)
			{
				dictionary[key] = (bool)jsonData2;
			}
			else if (jsonData2.IsDouble)
			{
				dictionary[key] = (double)jsonData2;
			}
			else if (jsonData2.IsString)
			{
				dictionary[key] = (string)jsonData2;
			}
			else
			{
				Log.Error("ThinkingAnalyticsTrack not support type. " + json);
			}
		}
		return dictionary;
	}

	public static void Dispose()
	{
		BIManagerCore.instance.Dispose();
	}
}
