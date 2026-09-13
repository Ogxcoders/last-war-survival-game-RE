using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using GameFramework;
using SFSLitJson;
using ThinkingAnalytics;
using UnityEngine;

public static class PostEventLog
{
	public static class Defines
	{
		public const string LAUNCH = "LAUNCH";

		public const string OBB_FETCH = "OBB_FETCH";

		public const string OBB_SUCCESS = "OBB_SUCCESS";

		public const string ResourcesInitialized = "resources_initialized";

		public const string CHECK_VERSION_START = "CHECK_VERSION_START";

		public const string CHECK_VERSION_SUCCESS = "CHECK_VERSION_SUCCESS";

		public const string CHECK_VERSION_FAILED = "CHECK_VERSION_FAILED";

		public const string CHECK_VERSION_TIMEOUT = "CHECK_VERSION_TIMEOUT";

		public const string CHECK_VERSION_ONUPDATE = "CHECK_VERSION_ONUPDATE";

		public const string DOWNLOAD_MANIFEST_START = "DOWNLOAD_MANIFEST_START";

		public const string DOWNLOAD_MANIFEST_SUCCESS = "DOWNLOAD_MANIFEST_SUCCESS";

		public const string DOWNLOAD_MANIFEST_FAILED = "DOWNLOAD_MANIFEST_FAILED";

		public const string DOWNLOAD_MANIFEST_TIMEOUT = "DOWNLOAD_MANIFEST_TIMEOUT";

		public const string DOWNLOAD_MANIFEST_RETRY = "DOWNLOAD_MANIFEST_RETRY";

		public const string DOWNLOAD_START = "DOWNLOAD_START";

		public const string DOWNLOAD_FINISH = "DOWNLOAD_FINISH";

		public const string DOWNLOAD_FAILED = "DOWNLOAD_FAILED";

		public const string CONVERT_BUNDLE = "CONVERT_BUNDLE";

		public const string START_CONNECT = "START_CONNECT";

		public const string CONNECT_TIME_OUT = "CONNECT_TIME_OUT";

		public const string GET_SERVERLIST = "GET_SERVERLIST";

		public const string SERVERLIST_TIME_OUT = "SERVERLIST_TIME_OUT";

		public const string SERVERLIST_FAILED = "SERVERLIST_FAILED";

		public const string LONG_TIME_NOT_PUSH_INIT = "LONG_TIME_NOT_PUSH_INIT";

		public const string GET_SERVERNOTICE = "GET_SERVERNOTICE";

		public const string SERVERNOTICE_TIME_OUT = "SERVERNOTICE_TIME_OUT";

		public const string SERVERNOTICE_FAILED = "SERVERNOTICE_FAILED";

		public const string ACCOUNT_CONNECT_TIME_OUT = "ACCOUNT_CONNECT_TIME_OUT";

		public const string LOGIN_START = "LOGIN_START";

		public const string LOGIN_FINISH = "LOGIN_FINISH";

		public const string LOGIN_COMPLETE = "LOGIN_COMPLETE";

		public const string LOGIN_FAILED = "LOGIN_FAILED";

		public const string SERVER_STATUS = "SERVER_STATUS";

		public const string PUSH_INIT_RECV = "PUSH_INIT_RECV";

		public const string LOGIN_PARSE_ERROR = "LOGIN_PARSE_ERROR";

		public const string DISCONNECT_RETRY = "DISCONNECT_RETRY";

		public const string SOCKET_ERROR = "SOCKET_ERROR";

		public const string DNS_SUCESS = "DNS_SUCESS";

		public const string SOCKET_SHUTDOWN = "SOCKET_SHUTDOWN";

		public const string APP_QUIT = "APP_QUIT";

		public const string CheckResVersionState = "check_res_version_state";

		public const string PermissionState = "permission_state";

		public const string DownloadManifestState = "download_manifest_state";

		public const string DownloadUpdateState = "download_update_state";

		public const string ConnectGameState = "connect_game_state";

		public const string AccountSelectState = "account_select_state";

		public const string ConnectFailBI = "connect_fail";

		public const string AccountConnectFailBI = "account_connect_fail";

		public const string LoginState = "login_state";

		public const string Login_Retry = "login_retry";

		public const string Login_TimeOut = "login_timeout";

		public const string LoginFailBI = "login_fail";

		public const string LoadingErrorState = "loading_error_state";

		public const string WarmupUpdateRate = "warmup_update_rate";

		public const string PushInitState = "pushinit_state";

		public const string LoadSceneState = "loadscene_state";

		public const string EnterGameState = "entergame_state";

		public const string LoadDataTableState = "load_data_table_state";

		public const string On_Application_Pause = "on_application_pause";

		public const string Application_Did_Enter_Background = "application_did_enter_background";

		public const string Application_Will_Enter_Foreground = "application_will_enter_foreground";

		public const string Reload_Game = "reload_game";

		public const string Disconnect_Retry = "disconnect_retry";

		public const string On_Low_Memory = "on_low_memory";

		public const string Open_Loading_UI = "open_loading_ui";

		public const string On_Loading_UI_Load = "on_loading_ui_load";

		public const string Hide_Splash = "hide_splash";

		public const string Init_Loading_UI_Text = "init_loading_ui_text";

		public const string InitNetProxy = "InitNetProxy";

		public const string ConnectNetSucceed = "ConnectNetSucceed";

		public const string ConnectNetFailed = "ConnectNetFailed";

		public const string ConnectNetAllFailed = "ConnectNetAllFailed";

		public const string GetCrossServerList = "GetCrossServerList";

		public const string CrossServerListTimeOut = "CrossServerListTimeOut";

		public const string CrossServerListRetry = "CrossServerListRetry";

		public const string CrossServerListSucceed = "CrossServerListSucceed";

		public const string CrossServerListFailed = "CrossServerListFailed";

		public const string InitSmartFox = "InitSmartFox";

		public const string ConnectSmartFoxSucceed = "ConnectSmartFoxSucceed";

		public const string CrossServerRetry = "CrossServerRetry";

		public const string InitCrossNetProxy = "InitCrossNetProxy";

		public const string ConnectCrossNetProxySucceed = "ConnectCrossNetProxySucceed";

		public const string IOS_CHECK_SECURITY = "ios_check_security";

		public const string FirstLaunchSkipUpdate = "FirstLaunchSkipUpdate";

		public const string CreateMarchDeltaTime = "CreateMarchDeltaTime";

		public const string CurNetProxy = "CurNetProxy";

		public const string CurCrossNetProxy = "CurCrossNetProxy";

		public const string DOWNLOAD_PAGE_SUCCESS = "DOWNLOAD_PAGE_SUCCESS";

		public const string FIREBASE_APPID_READY = "FIREBASE_APPID_READY";

		public const string FIREBASE_APPID_ALL_READY = "FIREBASE_APPID_ALL_READY";

		public const string DMA_AGREE_RECORD = "DMA_AGREE_RECORD";

		public const string SOUND_EFFECT_OPEN = "c_sound_effect_open";

		public const string SOUND_MUSIC_OPEN = "c_sound_music_open";

		public const string C_CALENDAR_USE = "c_calendar_use";

		public const string MiniGameTrackEvent = "MiniGameTrackEvent";

		public const string C_ODM_INFO = "c_odm_info";

		public const string C_SOUND_DUB_REMOTE_SUCCESS = "c_sound_dub_remote_success";
	}

	private static string[] pN = new string[4] { "param0", "param1", "param2", "param3" };

	private static Dictionary<string, string> pairs = new Dictionary<string, string>();

	private static StringBuilder sb = new StringBuilder();

	private static PostEventThread postEventThread = new PostEventThread();

	public static bool hasInit = false;

	private static Dictionary<string, int> actionCountMap = new Dictionary<string, int>();

	private static long posteventId = 0L;

	public const string POSTURL = "";

	public const string POSTURL_CN = "";

	private static Dictionary<string, string> exceptionDict = new Dictionary<string, string>();

	private static int getActionCount(string action)
	{
		if (actionCountMap.ContainsKey(action))
		{
			actionCountMap[action]++;
		}
		else
		{
			actionCountMap[action] = 1;
		}
		return actionCountMap[action];
	}

	public static void init()
	{
		if (!hasInit)
		{
			postEventThread.Start();
			Thread.Sleep(10);
			hasInit = true;
			posteventId = DateTimeOffset.Now.ToUnixTimeMilliseconds();
		}
	}

	public static void stop()
	{
		if (hasInit)
		{
			postEventThread.Stop();
			hasInit = false;
		}
	}

	public static void PostException(string action, string logString, string longText)
	{
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
			else if (jsonData2.IsLong)
			{
				dictionary[key] = (long)jsonData2;
			}
			else
			{
				Debug.LogWarning("ThinkingAnalyticsTrack not support type. " + json);
			}
		}
		return dictionary;
	}

	public static void TaEnableAutoTrack(int trackEvents)
	{
		try
		{
			ThinkingAnalyticsAPI.EnableAutoTrack((AUTO_TRACK_EVENTS)trackEvents);
		}
		catch (Exception)
		{
			Log.Error("TaEnableAutoTrack {0} error", trackEvents);
		}
	}

	public static void TaUserSet(string userSet)
	{
		try
		{
			if (!string.IsNullOrEmpty(userSet))
			{
				ThinkingAnalyticsAPI.UserSet(JsonDataToDict(userSet));
			}
		}
		catch (Exception)
		{
			Log.Error("TaUserSet {0} error", userSet);
		}
	}

	public static void TaUserSetOnce(string userOnce)
	{
		try
		{
			if (!string.IsNullOrEmpty(userOnce))
			{
				ThinkingAnalyticsAPI.UserSetOnce(JsonDataToDict(userOnce));
			}
		}
		catch (Exception)
		{
			Log.Error("TaUserSetOnce {0} error", userOnce);
		}
	}

	public static void TaUserAdd(string userAdd)
	{
		try
		{
			if (!string.IsNullOrEmpty(userAdd))
			{
				ThinkingAnalyticsAPI.UserSetOnce(JsonDataToDict(userAdd));
			}
		}
		catch (Exception)
		{
			Log.Error("TaUserAdd {0} error", userAdd);
		}
	}

	public static void TaUserAppend(string userAppend)
	{
		try
		{
			if (!string.IsNullOrEmpty(userAppend))
			{
				ThinkingAnalyticsAPI.UserSetOnce(JsonDataToDict(userAppend));
			}
		}
		catch (Exception)
		{
			Log.Error("TaUserAppend {0} error", userAppend);
		}
	}

	public static void SetSuperProperties(string superProp)
	{
		try
		{
			if (!string.IsNullOrEmpty(superProp))
			{
				ThinkingAnalyticsAPI.SetSuperProperties(JsonDataToDict(superProp));
			}
		}
		catch (Exception)
		{
			Log.Error("SetSuperProperties {0} error", superProp);
		}
	}

	public static void TrackMap(string eventName, Dictionary<string, object> prop)
	{
		try
		{
			if (prop == null)
			{
				ThinkingAnalyticsAPI.Track(eventName);
			}
			else
			{
				ThinkingAnalyticsAPI.Track(eventName, prop);
			}
		}
		catch (Exception)
		{
			Log.Error("Track {0} error {1}", eventName, prop);
		}
	}

	public static void Track(string eventName, string prop)
	{
		try
		{
			if (string.IsNullOrEmpty(prop))
			{
				ThinkingAnalyticsAPI.Track(eventName);
				return;
			}
			Dictionary<string, object> properties = JsonDataToDict(prop);
			ThinkingAnalyticsAPI.Track(eventName, properties);
		}
		catch (Exception)
		{
			Log.Error("Track {0} error {1}", eventName, prop);
		}
	}

	public static void Record(string action)
	{
	}

	public static void Record(string action, string param1)
	{
	}

	public static void Record(string action, string param1, string param2)
	{
	}

	public static void Record(string action, params string[] args)
	{
	}

	private static void Record(string action, Dictionary<string, string> dictionary = null)
	{
		GameEntryProxy.SettingManagerProxy setting = GameEntryProxy.Setting;
		GameEntryProxy.DeviceManagerProxy device = GameEntryProxy.Device;
		GameEntryProxy.GlobalDataManagerProxy globalData = GameEntryProxy.GlobalData;
		try
		{
			sb.Clear();
			sb.Append("&action=").Append(action);
			sb.AppendFormat("&actioncount={0}", getActionCount(action));
			sb.AppendFormat("&timestamp={0}", DateTimeOffset.Now.ToUnixTimeMilliseconds());
			sb.Append("&uid=").Append(setting.GetPublicString2("Setting.GAME_UID", "\"\""));
			sb.Append("&zone=").Append(setting.GetPublicString2("SERVER_ZONE", "\"\""));
			sb.Append("&deviceId=").Append(device.GetDeviceUid());
			sb.Append("&country=").Append(globalData.fromCountry);
			sb.Append("&version=").Append(GameEntryProxy.Sdk.Version);
			sb.Append("&buildcode=").Append(GameEntryProxy.Sdk.VersionCode);
			sb.Append("&platform=").Append(string.IsNullOrEmpty(globalData.analyticID) ? "\"\"" : globalData.analyticID);
			sb.Append("&posteventId=").Append(posteventId);
			sb.Append("&net=").Append(GameEntryProxy.Device.GetNetworkTypeDesc());
			sb.Append("&line=").Append(GameEntryProxy.Network.getCurLine());
			if (dictionary != null)
			{
				foreach (KeyValuePair<string, string> item in dictionary)
				{
					string key = item.Key;
					string value = item.Value;
					sb.Append("&").Append(key).Append("=")
						.Append(string.IsNullOrEmpty(value) ? "\"\"" : value);
				}
			}
			postEventThread.AddTask(new PostEventThreadTask(sb.ToString()));
		}
		catch (Exception)
		{
			Log.Error("record log {0} error", action);
		}
	}
}
