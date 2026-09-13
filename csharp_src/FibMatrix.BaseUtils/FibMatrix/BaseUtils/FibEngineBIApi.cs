using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace FibMatrix.BaseUtils;

public class FibEngineBIApi
{
	private const int k_VersionCode = 9;

	private const string k_EngeneBiAppId = "114";

	private static int s_BIIndex;

	private static string s_ProjectName;

	public static void SendEditorUseFunctionEvent(string funcName, Dictionary<string, object> eventInfoMap = null)
	{
		if (!Application.isEditor)
		{
			return;
		}
		if (string.IsNullOrEmpty(funcName))
		{
			Debug.LogError("invalid funcName");
			return;
		}
		if (eventInfoMap == null)
		{
			eventInfoMap = new Dictionary<string, object>();
		}
		eventInfoMap["funcName"] = funcName;
		SendEvent("UseFunction", eventInfoMap);
	}

	public static void SendEvent(string eventName, Dictionary<string, object> eventInfoMap = null)
	{
		SendGroupEventsImpl("114", new List<string> { eventName }, new List<Dictionary<string, object>> { eventInfoMap });
	}

	public static void SendGroupEvents(List<string> eventNames, List<Dictionary<string, object>> eventInfoMapList = null)
	{
		SendGroupEventsImpl("114", eventNames, eventInfoMapList);
	}

	internal static void SendGroupEventsImpl(string biProjectAppId, List<string> eventNames, List<Dictionary<string, object>> eventInfoMapList = null)
	{
		if (eventInfoMapList != null && eventInfoMapList.Count != eventNames.Count)
		{
			Debug.LogError("bi events and info not the same count, some will be ignored");
		}
		if (string.IsNullOrEmpty(biProjectAppId))
		{
			Debug.LogError("biProjectAppId is null or empty");
		}
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < eventNames.Count; i++)
		{
			Dictionary<string, object> commonData = GetCommonData();
			commonData["event"] = eventNames[i];
			Dictionary<string, object> dictionary = eventInfoMapList[i];
			if (dictionary == null)
			{
				dictionary = new Dictionary<string, object>();
			}
			dictionary["_ProjectName"] = FindProjectName();
			dictionary["_VersionCode"] = 9;
			commonData["eventinfo"] = dictionary;
			arrayList.Add(commonData);
		}
		string text = GenBiMsgJson(arrayList, biProjectAppId);
		BaseUtilRuntimeCfg instance = BaseUtilRuntimeCfg.Instance;
		bool flag = false;
		if (instance != null)
		{
			flag = instance.biConfig.onlyLoggingNotSend;
		}
		if (flag)
		{
			Debug.Log(text);
			return;
		}
		UploadHandlerRaw uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(text));
		new UnityWebRequest("https://bi-tracker-cn.rivergame.net/event/tracker", "POST", null, uploadHandler).SendWebRequest();
	}

	private static string GenBiMsgJson(ArrayList packets, string biProjectAppId)
	{
		new Dictionary<string, object>
		{
			{
				"rid",
				GetTimestampSince1970() + SystemInfo.deviceUniqueIdentifier
			},
			{ "v", "0.1.0" },
			{ "s", "client" },
			{ "rc", 0 },
			{ "app", biProjectAppId },
			{ "l", packets }
		};
		return "error: engine bi shouldn't be used in player";
	}

	private static long GetTimestampSince1970()
	{
		return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds;
	}

	private static Dictionary<string, object> GetCommonData()
	{
		s_BIIndex++;
		return new Dictionary<string, object>
		{
			["temp_id"] = SystemInfo.deviceUniqueIdentifier,
			["platform"] = "UnityEditor",
			["qn"] = s_BIIndex,
			["time"] = (int)(Time.realtimeSinceStartup * 1000f),
			["network_type"] = "wifi",
			["debug"] = 0,
			["uid"] = "",
			["device_id"] = SystemInfo.deviceUniqueIdentifier,
			["sid"] = -1,
			["app_version"] = "1.0." + 9
		};
	}

	public static string FindProjectName()
	{
		BaseUtilRuntimeCfg instance = BaseUtilRuntimeCfg.Instance;
		string text = "";
		if (instance != null)
		{
			text = instance.devCodeName;
		}
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		if (!string.IsNullOrEmpty(s_ProjectName))
		{
			return s_ProjectName;
		}
		string directoryName = Path.GetDirectoryName(Application.dataPath);
		s_ProjectName = Path.GetFileName(directoryName);
		int num = 20;
		while (!string.IsNullOrEmpty(directoryName) && num-- > 0)
		{
			string path = Path.Combine(directoryName, ".git/config");
			if (File.Exists(path))
			{
				string[] array = File.ReadAllLines(path);
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].Equals("[remote \"origin\"]"))
					{
						string text2 = array[i + 1];
						string text3 = "rivergame.net";
						int num2 = 0;
						num2 = ((!text2.Contains(text3)) ? (text2.LastIndexOf("/") + 1) : (text2.IndexOf(text3) + text3.Length + 1));
						s_ProjectName = text2.Substring(num2);
						break;
					}
				}
			}
			directoryName = Path.GetDirectoryName(directoryName);
		}
		return s_ProjectName;
	}
}
