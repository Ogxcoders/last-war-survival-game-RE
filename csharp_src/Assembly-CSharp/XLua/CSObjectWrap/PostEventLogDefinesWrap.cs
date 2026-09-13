using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class PostEventLogDefinesWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(PostEventLog.Defines);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 98, 0, 0);
		Utils.RegisterObject(L, translator, -4, "LAUNCH", "LAUNCH");
		Utils.RegisterObject(L, translator, -4, "OBB_FETCH", "OBB_FETCH");
		Utils.RegisterObject(L, translator, -4, "OBB_SUCCESS", "OBB_SUCCESS");
		Utils.RegisterObject(L, translator, -4, "ResourcesInitialized", "resources_initialized");
		Utils.RegisterObject(L, translator, -4, "CHECK_VERSION_START", "CHECK_VERSION_START");
		Utils.RegisterObject(L, translator, -4, "CHECK_VERSION_SUCCESS", "CHECK_VERSION_SUCCESS");
		Utils.RegisterObject(L, translator, -4, "CHECK_VERSION_FAILED", "CHECK_VERSION_FAILED");
		Utils.RegisterObject(L, translator, -4, "CHECK_VERSION_TIMEOUT", "CHECK_VERSION_TIMEOUT");
		Utils.RegisterObject(L, translator, -4, "CHECK_VERSION_ONUPDATE", "CHECK_VERSION_ONUPDATE");
		Utils.RegisterObject(L, translator, -4, "DOWNLOAD_MANIFEST_START", "DOWNLOAD_MANIFEST_START");
		Utils.RegisterObject(L, translator, -4, "DOWNLOAD_MANIFEST_SUCCESS", "DOWNLOAD_MANIFEST_SUCCESS");
		Utils.RegisterObject(L, translator, -4, "DOWNLOAD_MANIFEST_FAILED", "DOWNLOAD_MANIFEST_FAILED");
		Utils.RegisterObject(L, translator, -4, "DOWNLOAD_MANIFEST_TIMEOUT", "DOWNLOAD_MANIFEST_TIMEOUT");
		Utils.RegisterObject(L, translator, -4, "DOWNLOAD_MANIFEST_RETRY", "DOWNLOAD_MANIFEST_RETRY");
		Utils.RegisterObject(L, translator, -4, "DOWNLOAD_START", "DOWNLOAD_START");
		Utils.RegisterObject(L, translator, -4, "DOWNLOAD_FINISH", "DOWNLOAD_FINISH");
		Utils.RegisterObject(L, translator, -4, "DOWNLOAD_FAILED", "DOWNLOAD_FAILED");
		Utils.RegisterObject(L, translator, -4, "CONVERT_BUNDLE", "CONVERT_BUNDLE");
		Utils.RegisterObject(L, translator, -4, "START_CONNECT", "START_CONNECT");
		Utils.RegisterObject(L, translator, -4, "CONNECT_TIME_OUT", "CONNECT_TIME_OUT");
		Utils.RegisterObject(L, translator, -4, "GET_SERVERLIST", "GET_SERVERLIST");
		Utils.RegisterObject(L, translator, -4, "SERVERLIST_TIME_OUT", "SERVERLIST_TIME_OUT");
		Utils.RegisterObject(L, translator, -4, "SERVERLIST_FAILED", "SERVERLIST_FAILED");
		Utils.RegisterObject(L, translator, -4, "LONG_TIME_NOT_PUSH_INIT", "LONG_TIME_NOT_PUSH_INIT");
		Utils.RegisterObject(L, translator, -4, "GET_SERVERNOTICE", "GET_SERVERNOTICE");
		Utils.RegisterObject(L, translator, -4, "SERVERNOTICE_TIME_OUT", "SERVERNOTICE_TIME_OUT");
		Utils.RegisterObject(L, translator, -4, "SERVERNOTICE_FAILED", "SERVERNOTICE_FAILED");
		Utils.RegisterObject(L, translator, -4, "ACCOUNT_CONNECT_TIME_OUT", "ACCOUNT_CONNECT_TIME_OUT");
		Utils.RegisterObject(L, translator, -4, "LOGIN_START", "LOGIN_START");
		Utils.RegisterObject(L, translator, -4, "LOGIN_FINISH", "LOGIN_FINISH");
		Utils.RegisterObject(L, translator, -4, "LOGIN_COMPLETE", "LOGIN_COMPLETE");
		Utils.RegisterObject(L, translator, -4, "LOGIN_FAILED", "LOGIN_FAILED");
		Utils.RegisterObject(L, translator, -4, "SERVER_STATUS", "SERVER_STATUS");
		Utils.RegisterObject(L, translator, -4, "PUSH_INIT_RECV", "PUSH_INIT_RECV");
		Utils.RegisterObject(L, translator, -4, "LOGIN_PARSE_ERROR", "LOGIN_PARSE_ERROR");
		Utils.RegisterObject(L, translator, -4, "DISCONNECT_RETRY", "DISCONNECT_RETRY");
		Utils.RegisterObject(L, translator, -4, "SOCKET_ERROR", "SOCKET_ERROR");
		Utils.RegisterObject(L, translator, -4, "DNS_SUCESS", "DNS_SUCESS");
		Utils.RegisterObject(L, translator, -4, "SOCKET_SHUTDOWN", "SOCKET_SHUTDOWN");
		Utils.RegisterObject(L, translator, -4, "APP_QUIT", "APP_QUIT");
		Utils.RegisterObject(L, translator, -4, "CheckResVersionState", "check_res_version_state");
		Utils.RegisterObject(L, translator, -4, "PermissionState", "permission_state");
		Utils.RegisterObject(L, translator, -4, "DownloadManifestState", "download_manifest_state");
		Utils.RegisterObject(L, translator, -4, "DownloadUpdateState", "download_update_state");
		Utils.RegisterObject(L, translator, -4, "ConnectGameState", "connect_game_state");
		Utils.RegisterObject(L, translator, -4, "AccountSelectState", "account_select_state");
		Utils.RegisterObject(L, translator, -4, "ConnectFailBI", "connect_fail");
		Utils.RegisterObject(L, translator, -4, "AccountConnectFailBI", "account_connect_fail");
		Utils.RegisterObject(L, translator, -4, "LoginState", "login_state");
		Utils.RegisterObject(L, translator, -4, "Login_Retry", "login_retry");
		Utils.RegisterObject(L, translator, -4, "Login_TimeOut", "login_timeout");
		Utils.RegisterObject(L, translator, -4, "LoginFailBI", "login_fail");
		Utils.RegisterObject(L, translator, -4, "LoadingErrorState", "loading_error_state");
		Utils.RegisterObject(L, translator, -4, "WarmupUpdateRate", "warmup_update_rate");
		Utils.RegisterObject(L, translator, -4, "PushInitState", "pushinit_state");
		Utils.RegisterObject(L, translator, -4, "LoadSceneState", "loadscene_state");
		Utils.RegisterObject(L, translator, -4, "EnterGameState", "entergame_state");
		Utils.RegisterObject(L, translator, -4, "LoadDataTableState", "load_data_table_state");
		Utils.RegisterObject(L, translator, -4, "On_Application_Pause", "on_application_pause");
		Utils.RegisterObject(L, translator, -4, "Application_Did_Enter_Background", "application_did_enter_background");
		Utils.RegisterObject(L, translator, -4, "Application_Will_Enter_Foreground", "application_will_enter_foreground");
		Utils.RegisterObject(L, translator, -4, "Reload_Game", "reload_game");
		Utils.RegisterObject(L, translator, -4, "Disconnect_Retry", "disconnect_retry");
		Utils.RegisterObject(L, translator, -4, "On_Low_Memory", "on_low_memory");
		Utils.RegisterObject(L, translator, -4, "Open_Loading_UI", "open_loading_ui");
		Utils.RegisterObject(L, translator, -4, "On_Loading_UI_Load", "on_loading_ui_load");
		Utils.RegisterObject(L, translator, -4, "Hide_Splash", "hide_splash");
		Utils.RegisterObject(L, translator, -4, "Init_Loading_UI_Text", "init_loading_ui_text");
		Utils.RegisterObject(L, translator, -4, "InitNetProxy", "InitNetProxy");
		Utils.RegisterObject(L, translator, -4, "ConnectNetSucceed", "ConnectNetSucceed");
		Utils.RegisterObject(L, translator, -4, "ConnectNetFailed", "ConnectNetFailed");
		Utils.RegisterObject(L, translator, -4, "ConnectNetAllFailed", "ConnectNetAllFailed");
		Utils.RegisterObject(L, translator, -4, "GetCrossServerList", "GetCrossServerList");
		Utils.RegisterObject(L, translator, -4, "CrossServerListTimeOut", "CrossServerListTimeOut");
		Utils.RegisterObject(L, translator, -4, "CrossServerListRetry", "CrossServerListRetry");
		Utils.RegisterObject(L, translator, -4, "CrossServerListSucceed", "CrossServerListSucceed");
		Utils.RegisterObject(L, translator, -4, "CrossServerListFailed", "CrossServerListFailed");
		Utils.RegisterObject(L, translator, -4, "InitSmartFox", "InitSmartFox");
		Utils.RegisterObject(L, translator, -4, "ConnectSmartFoxSucceed", "ConnectSmartFoxSucceed");
		Utils.RegisterObject(L, translator, -4, "CrossServerRetry", "CrossServerRetry");
		Utils.RegisterObject(L, translator, -4, "InitCrossNetProxy", "InitCrossNetProxy");
		Utils.RegisterObject(L, translator, -4, "ConnectCrossNetProxySucceed", "ConnectCrossNetProxySucceed");
		Utils.RegisterObject(L, translator, -4, "IOS_CHECK_SECURITY", "ios_check_security");
		Utils.RegisterObject(L, translator, -4, "FirstLaunchSkipUpdate", "FirstLaunchSkipUpdate");
		Utils.RegisterObject(L, translator, -4, "CreateMarchDeltaTime", "CreateMarchDeltaTime");
		Utils.RegisterObject(L, translator, -4, "CurNetProxy", "CurNetProxy");
		Utils.RegisterObject(L, translator, -4, "CurCrossNetProxy", "CurCrossNetProxy");
		Utils.RegisterObject(L, translator, -4, "DOWNLOAD_PAGE_SUCCESS", "DOWNLOAD_PAGE_SUCCESS");
		Utils.RegisterObject(L, translator, -4, "FIREBASE_APPID_READY", "FIREBASE_APPID_READY");
		Utils.RegisterObject(L, translator, -4, "FIREBASE_APPID_ALL_READY", "FIREBASE_APPID_ALL_READY");
		Utils.RegisterObject(L, translator, -4, "DMA_AGREE_RECORD", "DMA_AGREE_RECORD");
		Utils.RegisterObject(L, translator, -4, "SOUND_EFFECT_OPEN", "c_sound_effect_open");
		Utils.RegisterObject(L, translator, -4, "SOUND_MUSIC_OPEN", "c_sound_music_open");
		Utils.RegisterObject(L, translator, -4, "C_CALENDAR_USE", "c_calendar_use");
		Utils.RegisterObject(L, translator, -4, "MiniGameTrackEvent", "MiniGameTrackEvent");
		Utils.RegisterObject(L, translator, -4, "C_ODM_INFO", "c_odm_info");
		Utils.RegisterObject(L, translator, -4, "C_SOUND_DUB_REMOTE_SUCCESS", "c_sound_dub_remote_success");
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "PostEventLog.Defines does not have a constructor!");
	}
}
