using System;
using System.Collections.Generic;
using ThinkingAnalytics.Utils;
using UnityEngine;

namespace ThinkingAnalytics.Wrapper;

public class ThinkingAnalyticsWrapper
{
	public interface IDynamicSuperPropertiesTrackerListener
	{
		string getDynamicSuperPropertiesString();
	}

	private class DynamicListenerAdapter : AndroidJavaProxy
	{
		public DynamicListenerAdapter()
			: base("cn.thinkingdata.android.ThinkingAnalyticsSDK$DynamicSuperPropertiesTrackerListener")
		{
		}

		public string getDynamicSuperPropertiesString()
		{
			Dictionary<string, object> obj = ((mDynamicSuperProperties == null) ? new Dictionary<string, object>() : mDynamicSuperProperties.GetDynamicSuperProperties());
			return TD_MiniJSON.Serialize(obj);
		}
	}

	public interface IAutoTrackEventTrackerListener
	{
		string eventCallback(int type, string properties);
	}

	private class AutoTrackListenerAdapter : AndroidJavaProxy
	{
		public AutoTrackListenerAdapter()
			: base("cn.thinkingdata.android.ThinkingAnalyticsSDK$AutoTrackEventTrackerListener")
		{
		}

		private string eventCallback(int type, string properties)
		{
			Dictionary<string, object> obj;
			if (mAutoTrackEventCallback != null)
			{
				Dictionary<string, object> properties2 = TD_MiniJSON.Deserialize(properties);
				obj = mAutoTrackEventCallback.AutoTrackEventCallback(type, properties2);
			}
			else
			{
				obj = new Dictionary<string, object>();
			}
			return TD_MiniJSON.Serialize(obj);
		}
	}

	private static readonly string JSON_CLASS = "org.json.JSONObject";

	private static readonly AndroidJavaClass sdkClass = new AndroidJavaClass("cn.thinkingdata.android.ThinkingAnalyticsSDK");

	private static readonly AndroidJavaClass configClass = new AndroidJavaClass("cn.thinkingdata.android.TDConfig");

	private static Dictionary<string, AndroidJavaObject> light_instances = null;

	private static string default_appId = null;

	public static MonoBehaviour sMono;

	private static IDynamicSuperProperties mDynamicSuperProperties;

	private static IAutoTrackEventCallback mAutoTrackEventCallback;

	private static System.Random rnd = new System.Random();

	private static AndroidJavaObject getJSONObject(string dataString)
	{
		if (dataString.Equals("null"))
		{
			return null;
		}
		try
		{
			return new AndroidJavaObject(JSON_CLASS, dataString);
		}
		catch (Exception ex)
		{
			TD_Log.w("ThinkingAnalytics: unexpected exception: " + ex);
		}
		return null;
	}

	private static string getTimeString(DateTime dateTime)
	{
		long num = (TimeZoneInfo.ConvertTimeToUtc(dateTime).Ticks - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).Ticks) / 10000;
		AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.Date", num);
		return getInstance(default_appId).Call<string>("getTimeString", new object[1] { androidJavaObject });
	}

	private static AndroidJavaObject getInstance(string appId)
	{
		AndroidJavaObject androidJavaObject = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
		if (string.IsNullOrEmpty(appId))
		{
			appId = default_appId;
		}
		AndroidJavaObject androidJavaObject2 = ((light_instances == null || !light_instances.ContainsKey(appId)) ? sdkClass.CallStatic<AndroidJavaObject>("sharedInstance", new object[2] { androidJavaObject, appId }) : light_instances[appId]);
		if (androidJavaObject2 == null)
		{
			androidJavaObject2 = sdkClass.CallStatic<AndroidJavaObject>("sharedInstance", new object[2] { androidJavaObject, default_appId });
		}
		return androidJavaObject2;
	}

	private static void enableLog(bool enable)
	{
		sdkClass.CallStatic("enableTrackLog", enable);
	}

	private static void setVersionInfo(string libName, string version)
	{
		sdkClass.CallStatic("setCustomerLibInfo", libName, version);
	}

	private static void init(ThinkingAnalyticsAPI.Token token)
	{
		AndroidJavaObject androidJavaObject = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject androidJavaObject2 = null;
		if (!string.IsNullOrEmpty(token.GetInstanceName()))
		{
			androidJavaObject2 = configClass.CallStatic<AndroidJavaObject>("getInstance", new object[4]
			{
				androidJavaObject,
				token.appid,
				token.serverUrl,
				token.GetInstanceName()
			});
			if (string.IsNullOrEmpty(default_appId))
			{
				default_appId = token.GetInstanceName();
			}
		}
		else
		{
			androidJavaObject2 = configClass.CallStatic<AndroidJavaObject>("getInstance", new object[3] { androidJavaObject, token.appid, token.serverUrl });
			if (string.IsNullOrEmpty(default_appId))
			{
				default_appId = token.appid;
			}
		}
		androidJavaObject2.Call("setModeInt", (int)token.mode);
		string timeZoneId = token.getTimeZoneId();
		if (timeZoneId != null && timeZoneId.Length > 0)
		{
			AndroidJavaObject androidJavaObject3 = new AndroidJavaClass("java.util.TimeZone").CallStatic<AndroidJavaObject>("getTimeZone", new object[1] { timeZoneId });
			if (androidJavaObject3 != null)
			{
				androidJavaObject2.Call<AndroidJavaObject>("setDefaultTimeZone", new object[1] { androidJavaObject3 });
			}
		}
		if (token.enableEncrypt)
		{
			androidJavaObject2.Call("enableEncrypt", true);
			AndroidJavaObject androidJavaObject4 = new AndroidJavaObject("cn.thinkingdata.android.encrypt.TDSecreteKey", token.encryptPublicKey, token.encryptVersion, "AES", "RSA");
			androidJavaObject2.Call("setSecretKey", androidJavaObject4);
		}
		sdkClass.CallStatic<AndroidJavaObject>("sharedInstance", new object[1] { androidJavaObject2 });
	}

	private static void flush(string appId)
	{
		getInstance(appId).Call("flush");
	}

	private static AndroidJavaObject getDate(DateTime dateTime)
	{
		long num = (TimeZoneInfo.ConvertTimeToUtc(dateTime).Ticks - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).Ticks) / 10000;
		return new AndroidJavaObject("java.util.Date", num);
	}

	private static void track(string eventName, string properties, DateTime dateTime, string appId)
	{
		AndroidJavaObject date = getDate(dateTime);
		new AndroidJavaClass("java.util.TimeZone");
		AndroidJavaObject androidJavaObject = null;
		getInstance(appId).Call("track", eventName, getJSONObject(properties), date, androidJavaObject);
	}

	private static void track(string eventName, string properties, DateTime dateTime, TimeZoneInfo timeZone, string appId)
	{
		AndroidJavaObject date = getDate(dateTime);
		AndroidJavaObject androidJavaObject = null;
		if (timeZone != null && timeZone.Id != null && timeZone.Id.Length > 0)
		{
			androidJavaObject = new AndroidJavaClass("java.util.TimeZone").CallStatic<AndroidJavaObject>("getTimeZone", new object[1] { timeZone.Id });
		}
		getInstance(appId).Call("track", eventName, getJSONObject(properties), date, androidJavaObject);
	}

	private static void trackForAll(string eventName, string properties, DateTime dateTime, TimeZoneInfo timeZone)
	{
		string appId = "";
		AndroidJavaObject date = getDate(dateTime);
		AndroidJavaObject androidJavaObject = null;
		if (timeZone != null && timeZone.Id != null && timeZone.Id.Length > 0)
		{
			androidJavaObject = new AndroidJavaClass("java.util.TimeZone").CallStatic<AndroidJavaObject>("getTimeZone", new object[1] { timeZone.Id });
		}
		getInstance(appId).Call("track", eventName, getJSONObject(properties), date, androidJavaObject);
	}

	private static void track(ThinkingAnalyticsEvent taEvent, string appId)
	{
		AndroidJavaObject androidJavaObject = null;
		switch (taEvent.EventType)
		{
		case ThinkingAnalyticsEvent.Type.FIRST:
		{
			androidJavaObject = new AndroidJavaObject("cn.thinkingdata.android.TDFirstEvent", taEvent.EventName, getJSONObject(getFinalEventProperties(taEvent.Properties)));
			string extraId = taEvent.ExtraId;
			if (!string.IsNullOrEmpty(extraId))
			{
				androidJavaObject.Call("setFirstCheckId", extraId);
			}
			break;
		}
		case ThinkingAnalyticsEvent.Type.UPDATABLE:
			androidJavaObject = new AndroidJavaObject("cn.thinkingdata.android.TDUpdatableEvent", taEvent.EventName, getJSONObject(getFinalEventProperties(taEvent.Properties)), taEvent.ExtraId);
			break;
		case ThinkingAnalyticsEvent.Type.OVERWRITABLE:
			androidJavaObject = new AndroidJavaObject("cn.thinkingdata.android.TDOverWritableEvent", taEvent.EventName, getJSONObject(getFinalEventProperties(taEvent.Properties)), taEvent.ExtraId);
			break;
		}
		if (androidJavaObject == null)
		{
			TD_Log.w("Unexpected java event object. Returning...");
			return;
		}
		_ = taEvent.EventTime;
		if (taEvent.EventTime != DateTime.MinValue)
		{
			AndroidJavaObject date = getDate(taEvent.EventTime);
			new AndroidJavaClass("java.util.TimeZone");
			AndroidJavaObject androidJavaObject2 = null;
			if (taEvent.EventTimeZone != null)
			{
				androidJavaObject2 = new AndroidJavaClass("java.util.TimeZone").CallStatic<AndroidJavaObject>("getTimeZone", new object[1] { taEvent.EventTimeZone.Id });
				androidJavaObject.Call("setEventTime", date, androidJavaObject2);
			}
			else
			{
				androidJavaObject.Call("setEventTime", date);
			}
		}
		getInstance(appId).Call("track", androidJavaObject);
	}

	private static void track(string eventName, string properties, string appId)
	{
		getInstance(appId).Call("track", eventName, getJSONObject(properties));
	}

	private static void setSuperProperties(string superProperties, string appId)
	{
		getInstance(appId).Call("setSuperProperties", getJSONObject(superProperties));
	}

	private static void unsetSuperProperty(string superPropertyName, string appId)
	{
		getInstance(appId).Call("unsetSuperProperty", superPropertyName);
	}

	private static void clearSuperProperty(string appId)
	{
		getInstance(appId).Call("clearSuperProperties");
	}

	private static Dictionary<string, object> getSuperProperties(string appId)
	{
		Dictionary<string, object> result = null;
		AndroidJavaObject androidJavaObject = getInstance(appId).Call<AndroidJavaObject>("getSuperProperties", Array.Empty<object>());
		if (androidJavaObject != null)
		{
			result = TD_MiniJSON.Deserialize(androidJavaObject.Call<string>("toString", Array.Empty<object>()));
		}
		return result;
	}

	private static Dictionary<string, object> getPresetProperties(string appId)
	{
		Dictionary<string, object> result = null;
		AndroidJavaObject androidJavaObject = getInstance(appId).Call<AndroidJavaObject>("getPresetProperties", Array.Empty<object>()).Call<AndroidJavaObject>("toEventPresetProperties", Array.Empty<object>());
		if (androidJavaObject != null)
		{
			result = TD_MiniJSON.Deserialize(androidJavaObject.Call<string>("toString", Array.Empty<object>()));
		}
		return result;
	}

	private static void timeEvent(string eventName, string appId)
	{
		getInstance(appId).Call("timeEvent", eventName);
	}

	private static void timeEventForAll(string eventName)
	{
		getInstance("").Call("timeEvent", eventName);
	}

	private static void identify(string uniqueId, string appId)
	{
		getInstance(appId).Call("identify", uniqueId);
	}

	private static string getDistinctId(string appId)
	{
		return getInstance(appId).Call<string>("getDistinctId", Array.Empty<object>());
	}

	private static void login(string uniqueId, string appId)
	{
		getInstance(appId).Call("login", uniqueId);
	}

	private static void userSetOnce(string properties, string appId)
	{
		getInstance(appId).Call("user_setOnce", getJSONObject(properties));
	}

	private static void userSetOnce(string properties, DateTime dateTime, string appId)
	{
		getInstance(appId).Call("user_setOnce", getJSONObject(properties), getDate(dateTime));
	}

	private static void userSet(string properties, string appId)
	{
		getInstance(appId).Call("user_set", getJSONObject(properties));
	}

	private static void userSet(string properties, DateTime dateTime, string appId)
	{
		getInstance(appId).Call("user_set", getJSONObject(properties), getDate(dateTime));
	}

	private static void userUnset(List<string> properties, string appId)
	{
		userUnset(properties, DateTime.Now, appId);
	}

	private static void userUnset(List<string> properties, DateTime dateTime, string appId)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (string property in properties)
		{
			dictionary.Add(property, 0);
		}
		getInstance(appId).Call("user_unset", getJSONObject(TD_MiniJSON.Serialize(dictionary)), getDate(dateTime));
	}

	private static void userAdd(string properties, string appId)
	{
		getInstance(appId).Call("user_add", getJSONObject(properties));
	}

	private static void userAdd(string properties, DateTime dateTime, string appId)
	{
		getInstance(appId).Call("user_add", getJSONObject(properties), getDate(dateTime));
	}

	private static void userAppend(string properties, string appId)
	{
		getInstance(appId).Call("user_append", getJSONObject(properties));
	}

	private static void userAppend(string properties, DateTime dateTime, string appId)
	{
		getInstance(appId).Call("user_append", getJSONObject(properties), getDate(dateTime));
	}

	private static void userUniqAppend(string properties, string appId)
	{
		getInstance(appId).Call("user_uniqAppend", getJSONObject(properties));
	}

	private static void userUniqAppend(string properties, DateTime dateTime, string appId)
	{
		getInstance(appId).Call("user_uniqAppend", getJSONObject(properties), getDate(dateTime));
	}

	private static void userDelete(string appId)
	{
		getInstance(appId).Call("user_delete");
	}

	private static void userDelete(DateTime dateTime, string appId)
	{
		getInstance(appId).Call("user_delete", getDate(dateTime));
	}

	private static void logout(string appId)
	{
		getInstance(appId).Call("logout");
	}

	private static string getDeviceId()
	{
		return getInstance(default_appId).Call<string>("getDeviceId", Array.Empty<object>());
	}

	private static void setDynamicSuperProperties(IDynamicSuperProperties dynamicSuperProperties, string appId)
	{
		DynamicListenerAdapter dynamicListenerAdapter = new DynamicListenerAdapter();
		getInstance(appId).Call("setDynamicSuperPropertiesTrackerListener", dynamicListenerAdapter);
	}

	private static void setNetworkType(ThinkingAnalyticsAPI.NetworkType networkType)
	{
		switch (networkType)
		{
		case ThinkingAnalyticsAPI.NetworkType.DEFAULT:
			getInstance(default_appId).Call("setNetworkType", 0);
			break;
		case ThinkingAnalyticsAPI.NetworkType.WIFI:
			getInstance(default_appId).Call("setNetworkType", 1);
			break;
		case ThinkingAnalyticsAPI.NetworkType.ALL:
			getInstance(default_appId).Call("setNetworkType", 2);
			break;
		}
	}

	private static void enableAutoTrack(AUTO_TRACK_EVENTS events, string properties, string appId)
	{
		getInstance(appId).Call("enableAutoTrack", (int)events, getJSONObject(properties));
	}

	private static void enableAutoTrack(AUTO_TRACK_EVENTS events, IAutoTrackEventCallback eventCallback, string appId)
	{
		AutoTrackListenerAdapter autoTrackListenerAdapter = new AutoTrackListenerAdapter();
		getInstance(appId).Call("enableAutoTrack", (int)events, autoTrackListenerAdapter);
	}

	private static void setAutoTrackProperties(AUTO_TRACK_EVENTS events, string properties, string appId)
	{
		getInstance(appId).Call("setAutoTrackProperties", (int)events, getJSONObject(properties));
	}

	private static void setTrackStatus(TA_TRACK_STATUS status, string appId)
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("cn.thinkingdata.android.ThinkingAnalyticsSDK$TATrackStatus");
		AndroidJavaObject androidJavaObject = status switch
		{
			TA_TRACK_STATUS.PAUSE => androidJavaClass.GetStatic<AndroidJavaObject>("PAUSE"), 
			TA_TRACK_STATUS.STOP => androidJavaClass.GetStatic<AndroidJavaObject>("STOP"), 
			TA_TRACK_STATUS.SAVE_ONLY => androidJavaClass.GetStatic<AndroidJavaObject>("SAVE_ONLY"), 
			_ => androidJavaClass.GetStatic<AndroidJavaObject>("NORMAL"), 
		};
		getInstance(appId).Call("setTrackStatus", androidJavaObject);
	}

	private static void optOutTracking(string appId)
	{
		getInstance(appId).Call("optOutTracking");
	}

	private static void optOutTrackingAndDeleteUser(string appId)
	{
		getInstance(appId).Call("optOutTrackingAndDeleteUser");
	}

	private static void optInTracking(string appId)
	{
		getInstance(appId).Call("optInTracking");
	}

	private static void enableTracking(bool enabled, string appId)
	{
		getInstance(appId).Call("enableTracking", enabled);
	}

	private static string createLightInstance()
	{
		string text = Guid.NewGuid().ToString("N");
		AndroidJavaObject value = getInstance(default_appId).Call<AndroidJavaObject>("createLightInstance", Array.Empty<object>());
		if (light_instances == null)
		{
			light_instances = new Dictionary<string, AndroidJavaObject>();
		}
		light_instances.Add(text, value);
		return text;
	}

	private static void calibrateTime(long timestamp)
	{
		sdkClass.CallStatic("calibrateTime", timestamp);
	}

	private static void calibrateTimeWithNtp(string ntpServer)
	{
		sdkClass.CallStatic("calibrateTimeWithNtpForUnity", ntpServer);
	}

	private static void enableThirdPartySharing(TAThirdPartyShareType shareType, string properties, string appId)
	{
		getInstance(appId).Call("enableThirdPartySharing", (int)shareType, getJSONObject(properties));
	}

	private static string serilize<T>(Dictionary<string, T> data)
	{
		return TD_MiniJSON.Serialize(data, getTimeString);
	}

	public static void ShareInstance(ThinkingAnalyticsAPI.Token token, MonoBehaviour mono, bool initRequired = true)
	{
		sMono = mono;
		if (initRequired)
		{
			init(token);
		}
	}

	public static void EnableLog(bool enable)
	{
		enableLog(enable);
	}

	public static void SetVersionInfo(string version)
	{
		setVersionInfo("Unity", version);
	}

	public static void Identify(string uniqueId, string appId)
	{
		identify(uniqueId, appId);
	}

	public static string GetDistinctId(string appId)
	{
		return getDistinctId(appId);
	}

	public static void Login(string accountId, string appId)
	{
		login(accountId, appId);
	}

	public static void Logout(string appId)
	{
		logout(appId);
	}

	public static void EnableAutoTrack(AUTO_TRACK_EVENTS events, Dictionary<string, object> properties, string appId)
	{
		enableAutoTrack(events, serilize(properties), appId);
	}

	public static void EnableAutoTrack(AUTO_TRACK_EVENTS events, IAutoTrackEventCallback eventCallback, string appId)
	{
		mAutoTrackEventCallback = eventCallback;
		enableAutoTrack(events, eventCallback, appId);
	}

	public static void SetAutoTrackProperties(AUTO_TRACK_EVENTS events, Dictionary<string, object> properties, string appId)
	{
		setAutoTrackProperties(events, serilize(properties), appId);
	}

	private static string getFinalEventProperties(Dictionary<string, object> properties)
	{
		TD_PropertiesChecker.CheckProperties(properties);
		if (mDynamicSuperProperties != null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			TD_PropertiesChecker.MergeProperties(mDynamicSuperProperties.GetDynamicSuperProperties(), dictionary);
			TD_PropertiesChecker.MergeProperties(properties, dictionary);
			return serilize(dictionary);
		}
		return serilize(properties);
	}

	public static void Track(string eventName, Dictionary<string, object> properties, string appId)
	{
		TD_PropertiesChecker.CheckString(eventName);
		track(eventName, getFinalEventProperties(properties), appId);
	}

	public static void Track(string eventName, Dictionary<string, object> properties, DateTime datetime, string appId)
	{
		TD_PropertiesChecker.CheckString(eventName);
		track(eventName, getFinalEventProperties(properties), datetime, appId);
	}

	public static void Track(string eventName, Dictionary<string, object> properties, DateTime datetime, TimeZoneInfo timeZone, string appId)
	{
		TD_PropertiesChecker.CheckString(eventName);
		track(eventName, getFinalEventProperties(properties), datetime, timeZone, appId);
	}

	public static void TrackForAll(string eventName, Dictionary<string, object> properties, DateTime datetime, TimeZoneInfo timeZone)
	{
		TD_PropertiesChecker.CheckString(eventName);
		trackForAll(eventName, getFinalEventProperties(properties), datetime, timeZone);
	}

	public static void Track(ThinkingAnalyticsEvent taEvent, string appId)
	{
		if (taEvent == null || !taEvent.EventType.HasValue)
		{
			TD_Log.w("Ignoring invalid TA event");
			return;
		}
		_ = taEvent.EventTime;
		TD_PropertiesChecker.CheckString(taEvent.EventName);
		TD_PropertiesChecker.CheckProperties(taEvent.Properties);
		track(taEvent, appId);
	}

	public static void SetSuperProperties(Dictionary<string, object> superProperties, string appId)
	{
		TD_PropertiesChecker.CheckProperties(superProperties);
		setSuperProperties(serilize(superProperties), appId);
	}

	public static void UnsetSuperProperty(string superPropertyName, string appId)
	{
		TD_PropertiesChecker.CheckString(superPropertyName);
		unsetSuperProperty(superPropertyName, appId);
	}

	public static void ClearSuperProperty(string appId)
	{
		clearSuperProperty(appId);
	}

	public static void TimeEvent(string eventName, string appId)
	{
		TD_PropertiesChecker.CheckString(eventName);
		timeEvent(eventName, appId);
	}

	public static void TimeEventForAll(string eventName)
	{
		TD_PropertiesChecker.CheckString(eventName);
		timeEventForAll(eventName);
	}

	public static Dictionary<string, object> GetSuperProperties(string appId)
	{
		return getSuperProperties(appId);
	}

	public static Dictionary<string, object> GetPresetProperties(string appId)
	{
		return getPresetProperties(appId);
	}

	public static void UserSet(Dictionary<string, object> properties, string appId)
	{
		TD_PropertiesChecker.CheckProperties(properties);
		userSet(serilize(properties), appId);
	}

	public static void UserSet(Dictionary<string, object> properties, DateTime dateTime, string appId)
	{
		TD_PropertiesChecker.CheckProperties(properties);
		userSet(serilize(properties), dateTime, appId);
	}

	public static void UserSetOnce(Dictionary<string, object> properties, string appId)
	{
		TD_PropertiesChecker.CheckProperties(properties);
		userSetOnce(serilize(properties), appId);
	}

	public static void UserSetOnce(Dictionary<string, object> properties, DateTime dateTime, string appId)
	{
		TD_PropertiesChecker.CheckProperties(properties);
		userSetOnce(serilize(properties), dateTime, appId);
	}

	public static void UserUnset(List<string> properties, string appId)
	{
		TD_PropertiesChecker.CheckProperties(properties);
		userUnset(properties, appId);
	}

	public static void UserUnset(List<string> properties, DateTime dateTime, string appId)
	{
		TD_PropertiesChecker.CheckProperties(properties);
		userUnset(properties, dateTime, appId);
	}

	public static void UserAdd(Dictionary<string, object> properties, string appId)
	{
		TD_PropertiesChecker.CheckProperties(properties);
		userAdd(serilize(properties), appId);
	}

	public static void UserAdd(Dictionary<string, object> properties, DateTime dateTime, string appId)
	{
		TD_PropertiesChecker.CheckProperties(properties);
		userAdd(serilize(properties), dateTime, appId);
	}

	public static void UserAppend(Dictionary<string, object> properties, string appId)
	{
		TD_PropertiesChecker.CheckProperties(properties);
		userAppend(serilize(properties), appId);
	}

	public static void UserAppend(Dictionary<string, object> properties, DateTime dateTime, string appId)
	{
		TD_PropertiesChecker.CheckProperties(properties);
		userAppend(serilize(properties), dateTime, appId);
	}

	public static void UserUniqAppend(Dictionary<string, object> properties, string appId)
	{
		TD_PropertiesChecker.CheckProperties(properties);
		userUniqAppend(serilize(properties), appId);
	}

	public static void UserUniqAppend(Dictionary<string, object> properties, DateTime dateTime, string appId)
	{
		TD_PropertiesChecker.CheckProperties(properties);
		userUniqAppend(serilize(properties), dateTime, appId);
	}

	public static void UserDelete(string appId)
	{
		userDelete(appId);
	}

	public static void UserDelete(DateTime dateTime, string appId)
	{
		userDelete(dateTime, appId);
	}

	public static void Flush(string appId)
	{
		flush(appId);
	}

	public static void SetNetworkType(ThinkingAnalyticsAPI.NetworkType networkType)
	{
		setNetworkType(networkType);
	}

	public static string GetDeviceId()
	{
		return getDeviceId();
	}

	public static void SetDynamicSuperProperties(IDynamicSuperProperties dynamicSuperProperties, string appId)
	{
		if (!TD_PropertiesChecker.CheckProperties(dynamicSuperProperties.GetDynamicSuperProperties()))
		{
			TD_Log.d("TA.Wrapper(" + appId + ") - Cannot set dynamic super properties due to invalid properties.");
		}
		mDynamicSuperProperties = dynamicSuperProperties;
		setDynamicSuperProperties(dynamicSuperProperties, appId);
	}

	public static void SetTrackStatus(TA_TRACK_STATUS status, string appId)
	{
		setTrackStatus(status, appId);
	}

	public static void OptOutTracking(string appId)
	{
		optOutTracking(appId);
	}

	public static void OptOutTrackingAndDeleteUser(string appId)
	{
		optOutTrackingAndDeleteUser(appId);
	}

	public static void OptInTracking(string appId)
	{
		optInTracking(appId);
	}

	public static void EnableTracking(bool enabled, string appId)
	{
		enableTracking(enabled, appId);
	}

	public static string CreateLightInstance()
	{
		return createLightInstance();
	}

	public static void CalibrateTime(long timestamp)
	{
		calibrateTime(timestamp);
	}

	public static void CalibrateTimeWithNtp(string ntpServer)
	{
		calibrateTimeWithNtp(ntpServer);
	}

	public static void EnableThirdPartySharing(TAThirdPartyShareType shareType, Dictionary<string, object> properties = null, string appId = "")
	{
		if (properties == null)
		{
			properties = new Dictionary<string, object>();
		}
		enableThirdPartySharing(shareType, serilize(properties), appId);
	}
}
