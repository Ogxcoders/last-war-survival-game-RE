using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using ThinkingAnalytics.TAException;
using ThinkingAnalytics.Utils;
using ThinkingAnalytics.Wrapper;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThinkingAnalytics;

[DisallowMultipleComponent]
public class ThinkingAnalyticsAPI : MonoBehaviour
{
	[Serializable]
	public struct Token
	{
		public string appid;

		public string serverUrl;

		public TAMode mode;

		public TATimeZone timeZone;

		public string timeZoneId;

		public bool enableEncrypt;

		public int encryptVersion;

		public string encryptPublicKey;

		public SSLPinningMode pinningMode;

		public bool allowInvalidCertificates;

		public bool validatesDomainName;

		private string instanceName;

		public Token(string appId, string serverUrl, TAMode mode = TAMode.NORMAL, TATimeZone timeZone = TATimeZone.Local, string timeZoneId = null, string instanceName = null)
		{
			appid = appId.Replace(" ", "");
			this.serverUrl = serverUrl;
			this.mode = mode;
			this.timeZone = timeZone;
			this.timeZoneId = timeZoneId;
			enableEncrypt = false;
			encryptVersion = 0;
			encryptPublicKey = null;
			pinningMode = SSLPinningMode.NONE;
			allowInvalidCertificates = false;
			validatesDomainName = true;
			if (!string.IsNullOrEmpty(instanceName))
			{
				instanceName = instanceName.Replace(" ", "");
			}
			this.instanceName = instanceName;
		}

		public string GetInstanceName()
		{
			return instanceName;
		}

		public string getTimeZoneId()
		{
			return timeZone switch
			{
				TATimeZone.UTC => "UTC", 
				TATimeZone.Asia_Shanghai => "Asia/Shanghai", 
				TATimeZone.Asia_Tokyo => "Asia/Tokyo", 
				TATimeZone.America_Los_Angeles => "America/Los_Angeles", 
				TATimeZone.America_New_York => "America/New_York", 
				TATimeZone.Other => timeZoneId, 
				_ => null, 
			};
		}
	}

	public enum TATimeZone
	{
		Local = 0,
		UTC = 1,
		Asia_Shanghai = 2,
		Asia_Tokyo = 3,
		America_Los_Angeles = 4,
		America_New_York = 5,
		Other = 100
	}

	public enum TAMode
	{
		NORMAL,
		DEBUG,
		DEBUG_ONLY
	}

	public enum NetworkType
	{
		DEFAULT = 1,
		WIFI,
		ALL
	}

	[Header("Configuration")]
	[Tooltip("是否手动初始化SDK")]
	public bool startManually = true;

	[Tooltip("是否打开 Log")]
	public bool enableLog = true;

	[Tooltip("设置网络类型")]
	public NetworkType networkType = NetworkType.DEFAULT;

	[Header("Project")]
	[Tooltip("项目相关配置, APP ID 会在项目申请时给出")]
	[HideInInspector]
	public Token[] tokens = new Token[1];

	private static ThinkingAnalyticsAPI sThinkingAnalyticsAPI;

	private static bool tracking_enabled = false;

	private static List<Dictionary<string, object>> eventCaches = new List<Dictionary<string, object>>();

	public static void EnableLog(bool enable, string appId = "")
	{
		if (sThinkingAnalyticsAPI != null)
		{
			sThinkingAnalyticsAPI.enableLog = enable;
			TD_Log.EnableLog(enable);
			ThinkingAnalyticsWrapper.EnableLog(enable);
		}
	}

	public static void Identify(string firstId, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.Identify(firstId, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[2] { firstId, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static string GetDistinctId(string appId = "")
	{
		if (tracking_enabled)
		{
			return ThinkingAnalyticsWrapper.GetDistinctId(appId);
		}
		return null;
	}

	public static void Login(string account, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.Login(account, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[2] { account, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void Logout(string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.Logout(appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[1] { appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void EnableAutoTrack(AUTO_TRACK_EVENTS events, Dictionary<string, object> properties = null, string appId = "")
	{
		if (tracking_enabled)
		{
			if (properties == null)
			{
				properties = new Dictionary<string, object>();
			}
			ThinkingAnalyticsWrapper.EnableAutoTrack(events, properties, appId);
			if ((events & AUTO_TRACK_EVENTS.APP_CRASH) != AUTO_TRACK_EVENTS.NONE && !TD_PublicConfig.DisableCSharpException)
			{
				ThinkingSDKExceptionHandler.RegisterTAExceptionHandler(properties);
			}
			if ((events & AUTO_TRACK_EVENTS.APP_SCENE_LOAD) != AUTO_TRACK_EVENTS.NONE)
			{
				SceneManager.sceneLoaded += OnSceneLoaded;
			}
			if ((events & AUTO_TRACK_EVENTS.APP_SCENE_UNLOAD) != AUTO_TRACK_EVENTS.NONE)
			{
				SceneManager.sceneUnloaded += OnSceneUnloaded;
			}
		}
		else
		{
			MethodBase currentMethod = MethodBase.GetCurrentMethod();
			object[] value = new object[3] { events, properties, appId };
			eventCaches.Add(new Dictionary<string, object>
			{
				{ "method", currentMethod },
				{ "parameters", value }
			});
		}
	}

	public static void EnableAutoTrack(AUTO_TRACK_EVENTS events, IAutoTrackEventCallback eventCallback, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.EnableAutoTrack(events, eventCallback, appId);
			if ((events & AUTO_TRACK_EVENTS.APP_CRASH) != AUTO_TRACK_EVENTS.NONE && !TD_PublicConfig.DisableCSharpException)
			{
				ThinkingSDKExceptionHandler.RegisterTAExceptionHandler(eventCallback);
			}
			if ((events & AUTO_TRACK_EVENTS.APP_SCENE_LOAD) != AUTO_TRACK_EVENTS.NONE)
			{
				SceneManager.sceneLoaded += OnSceneLoaded;
			}
			if ((events & AUTO_TRACK_EVENTS.APP_SCENE_UNLOAD) != AUTO_TRACK_EVENTS.NONE)
			{
				SceneManager.sceneUnloaded += OnSceneUnloaded;
			}
		}
		else
		{
			MethodBase currentMethod = MethodBase.GetCurrentMethod();
			object[] value = new object[3] { events, eventCallback, appId };
			eventCaches.Add(new Dictionary<string, object>
			{
				{ "method", currentMethod },
				{ "parameters", value }
			});
		}
	}

	public static void SetAutoTrackProperties(AUTO_TRACK_EVENTS events, Dictionary<string, object> properties, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.SetAutoTrackProperties(events, properties, appId);
			if ((events & AUTO_TRACK_EVENTS.APP_CRASH) != AUTO_TRACK_EVENTS.NONE && !TD_PublicConfig.DisableCSharpException)
			{
				ThinkingSDKExceptionHandler.SetAutoTrackProperties(properties);
			}
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[3] { events, properties, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void Track(string eventName, string appId = "")
	{
		Track(eventName, null, appId);
	}

	public static void Track(string eventName, Dictionary<string, object> properties, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.Track(eventName, properties, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[3] { eventName, properties, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	[Obsolete("Method is deprecated, please use Track(string eventName, Dictionary<string, object> properties, DateTime date, TimeZoneInfo timeZone, string appId = \"\") instead.")]
	public static void Track(string eventName, Dictionary<string, object> properties, DateTime date, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.Track(eventName, properties, date, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[4] { eventName, properties, date, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void Track(string eventName, Dictionary<string, object> properties, DateTime date, TimeZoneInfo timeZone, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.Track(eventName, properties, date, timeZone, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[5] { eventName, properties, date, timeZone, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	private static void TrackForAll(string eventName, Dictionary<string, object> properties, DateTime date, TimeZoneInfo timeZone)
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.TrackForAll(eventName, properties, date, timeZone);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[4] { eventName, properties, date, timeZone };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void Track(ThinkingAnalyticsEvent analyticsEvent, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.Track(analyticsEvent, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[2] { analyticsEvent, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void Flush(string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.Flush(appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[1] { appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		Dictionary<string, object> properties = new Dictionary<string, object>
		{
			{ "#scene_name", scene.name },
			{ "#scene_path", scene.path }
		};
		TrackForAll("ta_scene_loaded", properties, DateTime.Now, null);
		TimeEventForAll("ta_scene_unloaded");
	}

	public static void OnSceneUnloaded(Scene scene)
	{
		Dictionary<string, object> properties = new Dictionary<string, object>
		{
			{ "#scene_name", scene.name },
			{ "#scene_path", scene.path }
		};
		TrackForAll("ta_scene_unloaded", properties, DateTime.Now, null);
	}

	public static void SetSuperProperties(Dictionary<string, object> superProperties, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.SetSuperProperties(superProperties, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[2] { superProperties, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void UnsetSuperProperty(string property, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.UnsetSuperProperty(property, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[2] { property, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static Dictionary<string, object> GetSuperProperties(string appId = "")
	{
		if (tracking_enabled)
		{
			return ThinkingAnalyticsWrapper.GetSuperProperties(appId);
		}
		return null;
	}

	public static void ClearSuperProperties(string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.ClearSuperProperty(appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[1] { appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static TDPresetProperties GetPresetProperties(string appId = "")
	{
		if (tracking_enabled)
		{
			return new TDPresetProperties(ThinkingAnalyticsWrapper.GetPresetProperties(appId));
		}
		return null;
	}

	public static void SetDynamicSuperProperties(IDynamicSuperProperties dynamicSuperProperties, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.SetDynamicSuperProperties(dynamicSuperProperties, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[2] { dynamicSuperProperties, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void TimeEvent(string eventName, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.TimeEvent(eventName, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[2] { eventName, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	private static void TimeEventForAll(string eventName)
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.TimeEventForAll(eventName);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[1] { eventName };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void UserSet(Dictionary<string, object> properties, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.UserSet(properties, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[2] { properties, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void UserSet(Dictionary<string, object> properties, DateTime dateTime, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.UserSet(properties, dateTime, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[3] { properties, dateTime, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void UserUnset(string property, string appId = "")
	{
		UserUnset(new List<string> { property }, appId);
	}

	public static void UserUnset(List<string> properties, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.UserUnset(properties, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[2] { properties, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void UserUnset(List<string> properties, DateTime dateTime, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.UserUnset(properties, dateTime, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[3] { properties, dateTime, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void UserSetOnce(Dictionary<string, object> properties, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.UserSetOnce(properties, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[2] { properties, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void UserSetOnce(Dictionary<string, object> properties, DateTime dateTime, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.UserSetOnce(properties, dateTime, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[3] { properties, dateTime, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void UserAdd(string property, object value, string appId = "")
	{
		UserAdd(new Dictionary<string, object> { { property, value } }, appId);
	}

	public static void UserAdd(Dictionary<string, object> properties, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.UserAdd(properties, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[2] { properties, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void UserAdd(Dictionary<string, object> properties, DateTime dateTime, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.UserAdd(properties, dateTime, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[3] { properties, dateTime, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void UserAppend(Dictionary<string, object> properties, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.UserAppend(properties, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[2] { properties, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void UserAppend(Dictionary<string, object> properties, DateTime dateTime, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.UserAppend(properties, dateTime, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[3] { properties, dateTime, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void UserUniqAppend(Dictionary<string, object> properties, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.UserUniqAppend(properties, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[2] { properties, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void UserUniqAppend(Dictionary<string, object> properties, DateTime dateTime, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.UserUniqAppend(properties, dateTime, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[3] { properties, dateTime, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void UserDelete(string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.UserDelete(appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[1] { appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void UserDelete(DateTime dateTime, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.UserDelete(dateTime, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[2] { dateTime, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static void SetNetworkType(NetworkType networkType, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.SetNetworkType(networkType);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[2] { networkType, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static string GetDeviceId()
	{
		if (tracking_enabled)
		{
			return ThinkingAnalyticsWrapper.GetDeviceId();
		}
		return null;
	}

	public static void SetTrackStatus(TA_TRACK_STATUS status, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.SetTrackStatus(status, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[2] { status, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	[Obsolete("Method is deprecated, please use SetTrackStatus() instead.")]
	public static void OptOutTracking(string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.OptOutTracking(appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[1] { appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	[Obsolete("Method is deprecated, please use SetTrackStatus() instead.")]
	public static void OptOutTrackingAndDeleteUser(string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.OptOutTrackingAndDeleteUser(appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[1] { appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	[Obsolete("Method is deprecated, please use SetTrackStatus() instead.")]
	public static void OptInTracking(string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.OptInTracking(appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[1] { appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	[Obsolete("Method is deprecated, please use SetTrackStatus() instead.")]
	public static void EnableTracking(bool enabled, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.EnableTracking(enabled, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[2] { enabled, appId };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static string CreateLightInstance(string appId = "")
	{
		if (tracking_enabled)
		{
			return ThinkingAnalyticsWrapper.CreateLightInstance();
		}
		return null;
	}

	public static void CalibrateTime(long timestamp)
	{
		ThinkingAnalyticsWrapper.CalibrateTime(timestamp);
	}

	public static void CalibrateTimeWithNtp(string ntpServer)
	{
		ThinkingAnalyticsWrapper.CalibrateTimeWithNtp(ntpServer);
	}

	public static void EnableThirdPartySharing(TAThirdPartyShareType shareType, Dictionary<string, object> properties = null, string appId = "")
	{
		if (tracking_enabled)
		{
			ThinkingAnalyticsWrapper.EnableThirdPartySharing(shareType, properties, appId);
			return;
		}
		MethodBase currentMethod = MethodBase.GetCurrentMethod();
		object[] value = new object[1] { shareType };
		eventCaches.Add(new Dictionary<string, object>
		{
			{ "method", currentMethod },
			{ "parameters", value }
		});
	}

	public static string GetLocalRegion()
	{
		return RegionInfo.CurrentRegion.TwoLetterISORegionName;
	}

	public static void StartThinkingAnalytics(string appId, string serverUrl)
	{
		TAMode mode = TAMode.NORMAL;
		TATimeZone timeZone = TATimeZone.Local;
		StartThinkingAnalytics(new Token(appId, serverUrl, mode, timeZone));
	}

	public static void StartThinkingAnalytics(Token token)
	{
		StartThinkingAnalytics(new Token[1] { token });
	}

	public static void StartThinkingAnalytics(Token[] tokens = null)
	{
		tracking_enabled = true;
		if (tracking_enabled)
		{
			TD_PublicConfig.GetPublicConfig();
			TD_Log.EnableLog(sThinkingAnalyticsAPI.enableLog);
			ThinkingAnalyticsWrapper.EnableLog(sThinkingAnalyticsAPI.enableLog);
			ThinkingAnalyticsWrapper.SetVersionInfo(TD_PublicConfig.LIB_VERSION);
			if (tokens == null)
			{
				tokens = sThinkingAnalyticsAPI.tokens;
			}
			try
			{
				for (int i = 0; i < tokens.Length; i++)
				{
					Token token = tokens[i];
					if (!string.IsNullOrEmpty(token.appid))
					{
						token.appid = token.appid.Replace(" ", "");
						TD_Log.d("ThinkingAnalytics start with APPID: " + token.appid + ", SERVERURL: " + token.serverUrl + ", MODE: " + token.mode);
						ThinkingAnalyticsWrapper.ShareInstance(token, sThinkingAnalyticsAPI);
						ThinkingAnalyticsWrapper.SetNetworkType(sThinkingAnalyticsAPI.networkType);
					}
				}
			}
			catch
			{
			}
		}
		FlushEventCaches();
	}

	private static void FlushEventCaches()
	{
		List<Dictionary<string, object>> list = new List<Dictionary<string, object>>(eventCaches);
		eventCaches.Clear();
		foreach (Dictionary<string, object> item in list)
		{
			if (item.ContainsKey("method") && item.ContainsKey("parameters"))
			{
				MethodBase obj = (MethodBase)item["method"];
				object[] parameters = (object[])item["parameters"];
				obj.Invoke(null, parameters);
			}
		}
	}

	private void Awake()
	{
		if (sThinkingAnalyticsAPI == null)
		{
			sThinkingAnalyticsAPI = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			if (!startManually)
			{
				StartThinkingAnalytics();
			}
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
	}

	private void OnApplicationQuit()
	{
	}
}
