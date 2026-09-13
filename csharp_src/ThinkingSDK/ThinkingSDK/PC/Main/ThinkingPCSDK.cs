using System;
using System.Collections.Generic;
using ThinkingSDK.PC.Config;
using ThinkingSDK.PC.DataModel;
using ThinkingSDK.PC.Time;
using ThinkingSDK.PC.Utils;
using UnityEngine;

namespace ThinkingSDK.PC.Main;

public class ThinkingPCSDK
{
	private static readonly Dictionary<string, ThinkingSDKInstance> Instances = new Dictionary<string, ThinkingSDKInstance>();

	private static readonly Dictionary<string, ThinkingSDKInstance> LightInstances = new Dictionary<string, ThinkingSDKInstance>();

	private static string CurrentAppid;

	private ThinkingPCSDK()
	{
	}

	private static ThinkingSDKInstance GetInstance(string appId)
	{
		ThinkingSDKInstance thinkingSDKInstance = null;
		if (!string.IsNullOrEmpty(appId))
		{
			appId = appId.Replace(" ", "");
			if (LightInstances.ContainsKey(appId))
			{
				thinkingSDKInstance = LightInstances[appId];
			}
			else if (Instances.ContainsKey(appId))
			{
				thinkingSDKInstance = Instances[appId];
			}
		}
		if (thinkingSDKInstance == null)
		{
			thinkingSDKInstance = Instances[CurrentAppid];
		}
		return thinkingSDKInstance;
	}

	public static ThinkingSDKInstance CurrentInstance()
	{
		return Instances[CurrentAppid];
	}

	public static ThinkingSDKInstance Init(string appId, string server, string instanceName, ThinkingSDKConfig config = null, MonoBehaviour mono = null)
	{
		if (ThinkingSDKUtil.IsEmptyString(appId))
		{
			ThinkingSDKLogger.Print("appId is empty");
			return null;
		}
		ThinkingSDKInstance thinkingSDKInstance = null;
		if (!string.IsNullOrEmpty(instanceName))
		{
			if (Instances.ContainsKey(instanceName))
			{
				thinkingSDKInstance = Instances[instanceName];
			}
			else
			{
				thinkingSDKInstance = new ThinkingSDKInstance(appId, server, instanceName, config, mono);
				if (string.IsNullOrEmpty(CurrentAppid))
				{
					CurrentAppid = instanceName;
				}
				Instances[instanceName] = thinkingSDKInstance;
			}
		}
		else if (Instances.ContainsKey(appId))
		{
			thinkingSDKInstance = Instances[appId];
		}
		else
		{
			thinkingSDKInstance = new ThinkingSDKInstance(appId, server, null, config, mono);
			if (string.IsNullOrEmpty(CurrentAppid))
			{
				CurrentAppid = appId;
			}
			Instances[appId] = thinkingSDKInstance;
		}
		return thinkingSDKInstance;
	}

	public static void Identifiy(string distinctID, string appId = "")
	{
		GetInstance(appId).Identifiy(distinctID);
	}

	public static string DistinctId(string appId = "")
	{
		return GetInstance(appId).DistinctId();
	}

	public static void Login(string accountID, string appId = "")
	{
		GetInstance(appId).Login(accountID);
	}

	public static string AccountID(string appId = "")
	{
		return GetInstance(appId).AccountID();
	}

	public static void Logout(string appId = "")
	{
		GetInstance(appId).Logout();
	}

	public static void EnableAutoTrack(AUTO_TRACK_EVENTS events, Dictionary<string, object> properties, string appId = "")
	{
		GetInstance(appId).EnableAutoTrack(events, properties);
	}

	public static void EnableAutoTrack(AUTO_TRACK_EVENTS events, IAutoTrackEventCallback_PC eventCallback, string appId = "")
	{
		GetInstance(appId).EnableAutoTrack(events, eventCallback);
	}

	public static void SetAutoTrackProperties(AUTO_TRACK_EVENTS events, Dictionary<string, object> properties, string appId = "")
	{
		GetInstance(appId).SetAutoTrackProperties(events, properties);
	}

	public static void Track(string eventName, string appId = "")
	{
		GetInstance(appId).Track(eventName);
	}

	public static void Track(string eventName, Dictionary<string, object> properties, string appId = "")
	{
		GetInstance(appId).Track(eventName, properties);
	}

	public static void Track(string eventName, Dictionary<string, object> properties, DateTime date, string appId = "")
	{
		GetInstance(appId).Track(eventName, properties, date);
	}

	public static void Track(string eventName, Dictionary<string, object> properties, DateTime date, TimeZoneInfo timeZone, string appId = "")
	{
		GetInstance(appId).Track(eventName, properties, date, timeZone);
	}

	public static void TrackForAll(string eventName, Dictionary<string, object> properties, DateTime date, TimeZoneInfo timeZone)
	{
		foreach (string key in Instances.Keys)
		{
			GetInstance(key).Track(eventName, properties, date, timeZone);
		}
	}

	public static void Track(ThinkingSDKEventData analyticsEvent, string appId = "")
	{
		GetInstance(appId).Track(analyticsEvent);
	}

	public static void Flush(string appId = "")
	{
		GetInstance(appId).Flush();
	}

	public static void FlushImmediately(string appId = "")
	{
		GetInstance(appId).FlushImmediately();
	}

	public static void SetSuperProperties(Dictionary<string, object> superProperties, string appId = "")
	{
		GetInstance(appId).SetSuperProperties(superProperties);
	}

	public static void UnsetSuperProperty(string propertyKey, string appId = "")
	{
		GetInstance(appId).UnsetSuperProperty(propertyKey);
	}

	public static Dictionary<string, object> SuperProperties(string appId = "")
	{
		return GetInstance(appId).SuperProperties();
	}

	public static Dictionary<string, object> PresetProperties(string appId = "")
	{
		return GetInstance(appId).PresetProperties();
	}

	public static void ClearSuperProperties(string appId = "")
	{
		GetInstance(appId).ClearSuperProperties();
	}

	public static void TimeEvent(string eventName, string appId = "")
	{
		GetInstance(appId).TimeEvent(eventName);
	}

	public static void TimeEventForAll(string eventName)
	{
		foreach (string key in Instances.Keys)
		{
			GetInstance(key).TimeEvent(eventName);
		}
	}

	public static void PauseTimeEvent(bool status, string eventName = "", string appId = "")
	{
		GetInstance(appId).PauseTimeEvent(status, eventName);
	}

	public static void UserSet(Dictionary<string, object> properties, string appId = "")
	{
		GetInstance(appId).UserSet(properties);
	}

	public static void UserSet(Dictionary<string, object> properties, DateTime dateTime, string appId = "")
	{
		GetInstance(appId).UserSet(properties, dateTime);
	}

	public static void UserUnset(string propertyKey, string appId = "")
	{
		GetInstance(appId).UserUnset(propertyKey);
	}

	public static void UserUnset(string propertyKey, DateTime dateTime, string appId = "")
	{
		GetInstance(appId).UserUnset(propertyKey, dateTime);
	}

	public static void UserUnset(List<string> propertyKeys, string appId = "")
	{
		GetInstance(appId).UserUnset(propertyKeys);
	}

	public static void UserUnset(List<string> propertyKeys, DateTime dateTime, string appId = "")
	{
		GetInstance(appId).UserUnset(propertyKeys, dateTime);
	}

	public static void UserSetOnce(Dictionary<string, object> properties, string appId = "")
	{
		GetInstance(appId).UserSetOnce(properties);
	}

	public static void UserSetOnce(Dictionary<string, object> properties, DateTime dateTime, string appId = "")
	{
		GetInstance(appId).UserSetOnce(properties, dateTime);
	}

	public static void UserAdd(Dictionary<string, object> properties, string appId = "")
	{
		GetInstance(appId).UserAdd(properties);
	}

	public static void UserAdd(Dictionary<string, object> properties, DateTime dateTime, string appId = "")
	{
		GetInstance(appId).UserAdd(properties, dateTime);
	}

	public static void UserAppend(Dictionary<string, object> properties, string appId = "")
	{
		GetInstance(appId).UserAppend(properties);
	}

	public static void UserAppend(Dictionary<string, object> properties, DateTime dateTime, string appId = "")
	{
		GetInstance(appId).UserAppend(properties, dateTime);
	}

	public static void UserUniqAppend(Dictionary<string, object> properties, string appId = "")
	{
		GetInstance(appId).UserUniqAppend(properties);
	}

	public static void UserUniqAppend(Dictionary<string, object> properties, DateTime dateTime, string appId = "")
	{
		GetInstance(appId).UserUniqAppend(properties, dateTime);
	}

	public static void UserDelete(string appId = "")
	{
		GetInstance(appId).UserDelete();
	}

	public static void UserDelete(DateTime dateTime, string appId = "")
	{
		GetInstance(appId).UserDelete(dateTime);
	}

	public static void SetDynamicSuperProperties(IDynamicSuperProperties_PC dynamicSuperProperties, string appId = "")
	{
		GetInstance(appId).SetDynamicSuperProperties(dynamicSuperProperties);
	}

	public static void SetTrackStatus(TA_TRACK_STATUS status, string appId = "")
	{
		GetInstance(appId).SetTrackStatus(status);
	}

	public static void OptTracking(bool optTracking, string appId = "")
	{
		GetInstance(appId).OptTracking(optTracking);
	}

	public static void EnableTracking(bool isEnable, string appId = "")
	{
		GetInstance(appId).EnableTracking(isEnable);
	}

	public static void OptTrackingAndDeleteUser(string appId = "")
	{
		GetInstance(appId).OptTrackingAndDeleteUser();
	}

	public static string CreateLightInstance()
	{
		string text = Guid.NewGuid().ToString("N");
		ThinkingSDKInstance value = ThinkingSDKInstance.CreateLightInstance();
		LightInstances[text] = value;
		return text;
	}

	public static void CalibrateTime(long timestamp)
	{
		ThinkingSDKTimestampCalibration timeCalibratieton = new ThinkingSDKTimestampCalibration(timestamp);
		foreach (KeyValuePair<string, ThinkingSDKInstance> instance in Instances)
		{
			instance.Value.SetTimeCalibratieton(timeCalibratieton);
		}
	}

	public static void CalibrateTimeWithNtp(string ntpServer)
	{
		ThinkingSDKNTPCalibration timeCalibratieton = new ThinkingSDKNTPCalibration(ntpServer);
		foreach (KeyValuePair<string, ThinkingSDKInstance> instance in Instances)
		{
			instance.Value.SetTimeCalibratieton(timeCalibratieton);
		}
	}

	public static string GetDeviceId()
	{
		return ThinkingSDKDeviceInfo.DeviceID();
	}

	public static void EnableLog(bool isEnable)
	{
		ThinkingSDKPublicConfig.SetIsPrintLog(isEnable);
	}

	public static void SetLibName(string name)
	{
		ThinkingSDKPublicConfig.SetName(name);
	}

	public static void SetLibVersion(string versionCode)
	{
		ThinkingSDKPublicConfig.SetVersion(versionCode);
	}

	public static string TimeString(DateTime dateTime, string appId = "")
	{
		return GetInstance(appId).TimeString(dateTime);
	}
}
