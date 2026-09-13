using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class AppStartupLoadingWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(AppStartupLoading);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 27, 17, 12);
		Utils.RegisterFunc(L, -3, "ClearUpdateAfterLogin", _m_ClearUpdateAfterLogin);
		Utils.RegisterFunc(L, -3, "Start", _m_Start);
		Utils.RegisterFunc(L, -3, "Shutdown", _m_Shutdown);
		Utils.RegisterFunc(L, -3, "CloseUILoading", _m_CloseUILoading);
		Utils.RegisterFunc(L, -3, "ClosePrivacyView", _m_ClosePrivacyView);
		Utils.RegisterFunc(L, -3, "ReConnect", _m_ReConnect);
		Utils.RegisterFunc(L, -3, "StartConnect", _m_StartConnect);
		Utils.RegisterFunc(L, -3, "StartConnectGame", _m_StartConnectGame);
		Utils.RegisterFunc(L, -3, "ToConnectGame", _m_ToConnectGame);
		Utils.RegisterFunc(L, -3, "ToDownloadManifest", _m_ToDownloadManifest);
		Utils.RegisterFunc(L, -3, "ToLoadDataTable", _m_ToLoadDataTable);
		Utils.RegisterFunc(L, -3, "GetState", _m_GetState);
		Utils.RegisterFunc(L, -3, "SetState", _m_SetState);
		Utils.RegisterFunc(L, -3, "ExitCurState", _m_ExitCurState);
		Utils.RegisterFunc(L, -3, "HideSplashEndOfFrame", _m_HideSplashEndOfFrame);
		Utils.RegisterFunc(L, -3, "Update", _m_Update);
		Utils.RegisterFunc(L, -3, "OnInitError", _m_OnInitError);
		Utils.RegisterFunc(L, -3, "OnAuthSuccess", _m_OnAuthSuccess);
		Utils.RegisterFunc(L, -3, "PreloadAssets", _m_PreloadAssets);
		Utils.RegisterFunc(L, -3, "LoadGameServerSetting", _m_LoadGameServerSetting);
		Utils.RegisterFunc(L, -3, "SaveGameServerSetting", _m_SaveGameServerSetting);
		Utils.RegisterFunc(L, -3, "SaveGameLoginToken", _m_SaveGameLoginToken);
		Utils.RegisterFunc(L, -3, "ClearAccessToken", _m_ClearAccessToken);
		Utils.RegisterFunc(L, -3, "ClearRefreshToken", _m_ClearRefreshToken);
		Utils.RegisterFunc(L, -3, "ClearLoginKey", _m_ClearLoginKey);
		Utils.RegisterFunc(L, -3, "ClearAllAccountSettings", _m_ClearAllAccountSettings);
		Utils.RegisterFunc(L, -3, "ClearGameServerSetting", _m_ClearGameServerSetting);
		Utils.RegisterFunc(L, -2, "currState", _g_get_currState);
		Utils.RegisterFunc(L, -2, "BundleDownloadTotalBytes", _g_get_BundleDownloadTotalBytes);
		Utils.RegisterFunc(L, -2, "BundleDownloadProgress", _g_get_BundleDownloadProgress);
		Utils.RegisterFunc(L, -2, "LoadingProgress", _g_get_LoadingProgress);
		Utils.RegisterFunc(L, -2, "IsPushInitReceived", _g_get_IsPushInitReceived);
		Utils.RegisterFunc(L, -2, "IsNeedIdentification", _g_get_IsNeedIdentification);
		Utils.RegisterFunc(L, -2, "IsChild", _g_get_IsChild);
		Utils.RegisterFunc(L, -2, "LoginTryCount", _g_get_LoginTryCount);
		Utils.RegisterFunc(L, -2, "IsLoading", _g_get_IsLoading);
		Utils.RegisterFunc(L, -2, "updateAfterLogin", _g_get_updateAfterLogin);
		Utils.RegisterFunc(L, -2, "checkResVersionError", _g_get_checkResVersionError);
		Utils.RegisterFunc(L, -2, "UILoading", _g_get_UILoading);
		Utils.RegisterFunc(L, -2, "IsShowServerList", _g_get_IsShowServerList);
		Utils.RegisterFunc(L, -2, "PermissionRecv", _g_get_PermissionRecv);
		Utils.RegisterFunc(L, -2, "isCanCloseLoading", _g_get_isCanCloseLoading);
		Utils.RegisterFunc(L, -2, "isErrorSuccess", _g_get_isErrorSuccess);
		Utils.RegisterFunc(L, -2, "isZipModeFinish", _g_get_isZipModeFinish);
		Utils.RegisterFunc(L, -1, "BundleDownloadTotalBytes", _s_set_BundleDownloadTotalBytes);
		Utils.RegisterFunc(L, -1, "BundleDownloadProgress", _s_set_BundleDownloadProgress);
		Utils.RegisterFunc(L, -1, "IsPushInitReceived", _s_set_IsPushInitReceived);
		Utils.RegisterFunc(L, -1, "IsNeedIdentification", _s_set_IsNeedIdentification);
		Utils.RegisterFunc(L, -1, "IsChild", _s_set_IsChild);
		Utils.RegisterFunc(L, -1, "LoginTryCount", _s_set_LoginTryCount);
		Utils.RegisterFunc(L, -1, "UILoading", _s_set_UILoading);
		Utils.RegisterFunc(L, -1, "IsShowServerList", _s_set_IsShowServerList);
		Utils.RegisterFunc(L, -1, "PermissionRecv", _s_set_PermissionRecv);
		Utils.RegisterFunc(L, -1, "isCanCloseLoading", _s_set_isCanCloseLoading);
		Utils.RegisterFunc(L, -1, "isErrorSuccess", _s_set_isErrorSuccess);
		Utils.RegisterFunc(L, -1, "isZipModeFinish", _s_set_isZipModeFinish);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 3, 0, 0);
		Utils.RegisterFunc(L, -4, "GetCurrStateProgressValue", _m_GetCurrStateProgressValue_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "LoginMaxTryCount", 3);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 1)
			{
				AppStartupLoading o = new AppStartupLoading();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to AppStartupLoading constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearUpdateAfterLogin(IntPtr L)
	{
		try
		{
			AppStartupLoading obj = (AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool value = Lua.lua_toboolean(L, 2);
			obj.ClearUpdateAfterLogin(value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Start(IntPtr L)
	{
		try
		{
			AppStartupLoading obj = (AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool isReload = Lua.lua_toboolean(L, 2);
			bool showLogo = Lua.lua_toboolean(L, 3);
			obj.Start(isReload, showLogo);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Shutdown(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Shutdown();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CloseUILoading(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CloseUILoading();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClosePrivacyView(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClosePrivacyView();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurrStateProgressValue_xlua_st_(IntPtr L)
	{
		try
		{
			float currStateProgressValue = AppStartupLoading.GetCurrStateProgressValue();
			Lua.lua_pushnumber(L, currStateProgressValue);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReConnect(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ReConnect();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StartConnect(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StartConnect();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StartConnectGame(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StartConnectGame();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToConnectGame(IntPtr L)
	{
		try
		{
			AppStartupLoading obj = (AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string ip = Lua.lua_tostring(L, 2);
			int port = Lua.xlua_tointeger(L, 3);
			string zone = Lua.lua_tostring(L, 4);
			string uid = Lua.lua_tostring(L, 5);
			int connectionType = Lua.xlua_tointeger(L, 6);
			string strRequiredPackages = Lua.lua_tostring(L, 7);
			string wsIp = Lua.lua_tostring(L, 8);
			obj.ToConnectGame(ip, port, zone, uid, connectionType, strRequiredPackages, wsIp);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToDownloadManifest(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ToDownloadManifest();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToLoadDataTable(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ToLoadDataTable();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AppStartupLoading appStartupLoading = (AppStartupLoading)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out LoadingState v);
			LoadingStateBase state = appStartupLoading.GetState(v);
			objectTranslator.Push(L, state);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AppStartupLoading appStartupLoading = (AppStartupLoading)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out LoadingState v);
			object[] args = objectTranslator.GetParams<object>(L, 3);
			appStartupLoading.SetState(v, args);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ExitCurState(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ExitCurState();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HideSplashEndOfFrame(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).HideSplashEndOfFrame();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Update(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Update();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnInitError(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnInitError();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnAuthSuccess(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnAuthSuccess();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PreloadAssets(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PreloadAssets();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadGameServerSetting(IntPtr L)
	{
		try
		{
			string ip;
			int port;
			string zone;
			string uid;
			int connectionType;
			bool value = ((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LoadGameServerSetting(out ip, out port, out zone, out uid, out connectionType);
			Lua.lua_pushboolean(L, value);
			Lua.lua_pushstring(L, ip);
			Lua.xlua_pushinteger(L, port);
			Lua.lua_pushstring(L, zone);
			Lua.lua_pushstring(L, uid);
			Lua.xlua_pushinteger(L, connectionType);
			return 6;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SaveGameServerSetting(IntPtr L)
	{
		try
		{
			AppStartupLoading obj = (AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string ip = Lua.lua_tostring(L, 2);
			int port = Lua.xlua_tointeger(L, 3);
			string zone = Lua.lua_tostring(L, 4);
			string uid = Lua.lua_tostring(L, 5);
			string uuid = Lua.lua_tostring(L, 6);
			int connectionType = Lua.xlua_tointeger(L, 7);
			obj.SaveGameServerSetting(ip, port, zone, uid, uuid, connectionType);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SaveGameLoginToken(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AppStartupLoading appStartupLoading = (AppStartupLoading)objectTranslator.FastGetCSObj(L, 1);
			LoginToken at = (LoginToken)objectTranslator.GetObject(L, 2, typeof(LoginToken));
			LoginToken rt = (LoginToken)objectTranslator.GetObject(L, 3, typeof(LoginToken));
			appStartupLoading.SaveGameLoginToken(at, rt);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearAccessToken(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearAccessToken();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearRefreshToken(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearRefreshToken();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearLoginKey(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearLoginKey();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearAllAccountSettings(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearAllAccountSettings();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearGameServerSetting(IntPtr L)
	{
		try
		{
			AppStartupLoading appStartupLoading = (AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool clearZone = Lua.lua_toboolean(L, 2);
				appStartupLoading.ClearGameServerSetting(clearZone);
				return 0;
			}
			if (num == 1)
			{
				appStartupLoading.ClearGameServerSetting();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to AppStartupLoading.ClearGameServerSetting!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_currState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AppStartupLoading appStartupLoading = (AppStartupLoading)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, appStartupLoading.currState);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_BundleDownloadTotalBytes(IntPtr L)
	{
		try
		{
			AppStartupLoading appStartupLoading = (AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, appStartupLoading.BundleDownloadTotalBytes);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_BundleDownloadProgress(IntPtr L)
	{
		try
		{
			AppStartupLoading appStartupLoading = (AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, appStartupLoading.BundleDownloadProgress);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LoadingProgress(IntPtr L)
	{
		try
		{
			AppStartupLoading appStartupLoading = (AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, appStartupLoading.LoadingProgress);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsPushInitReceived(IntPtr L)
	{
		try
		{
			AppStartupLoading appStartupLoading = (AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, appStartupLoading.IsPushInitReceived);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsNeedIdentification(IntPtr L)
	{
		try
		{
			AppStartupLoading appStartupLoading = (AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, appStartupLoading.IsNeedIdentification);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsChild(IntPtr L)
	{
		try
		{
			AppStartupLoading appStartupLoading = (AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, appStartupLoading.IsChild);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LoginTryCount(IntPtr L)
	{
		try
		{
			AppStartupLoading appStartupLoading = (AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, appStartupLoading.LoginTryCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsLoading(IntPtr L)
	{
		try
		{
			AppStartupLoading appStartupLoading = (AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, appStartupLoading.IsLoading);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_updateAfterLogin(IntPtr L)
	{
		try
		{
			AppStartupLoading appStartupLoading = (AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, appStartupLoading.updateAfterLogin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_checkResVersionError(IntPtr L)
	{
		try
		{
			AppStartupLoading appStartupLoading = (AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, appStartupLoading.checkResVersionError);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_UILoading(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AppStartupLoading appStartupLoading = (AppStartupLoading)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, appStartupLoading.UILoading);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsShowServerList(IntPtr L)
	{
		try
		{
			AppStartupLoading appStartupLoading = (AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, appStartupLoading.IsShowServerList);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_PermissionRecv(IntPtr L)
	{
		try
		{
			AppStartupLoading appStartupLoading = (AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, appStartupLoading.PermissionRecv);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isCanCloseLoading(IntPtr L)
	{
		try
		{
			AppStartupLoading appStartupLoading = (AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, appStartupLoading.isCanCloseLoading);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isErrorSuccess(IntPtr L)
	{
		try
		{
			AppStartupLoading appStartupLoading = (AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, appStartupLoading.isErrorSuccess);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isZipModeFinish(IntPtr L)
	{
		try
		{
			AppStartupLoading appStartupLoading = (AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, appStartupLoading.isZipModeFinish);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_BundleDownloadTotalBytes(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).BundleDownloadTotalBytes = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_BundleDownloadProgress(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).BundleDownloadProgress = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsPushInitReceived(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsPushInitReceived = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsNeedIdentification(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsNeedIdentification = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsChild(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsChild = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_LoginTryCount(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LoginTryCount = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_UILoading(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((AppStartupLoading)objectTranslator.FastGetCSObj(L, 1)).UILoading = (UILoadingComponent)objectTranslator.GetObject(L, 2, typeof(UILoadingComponent));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsShowServerList(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsShowServerList = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_PermissionRecv(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PermissionRecv = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isCanCloseLoading(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isCanCloseLoading = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isErrorSuccess(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isErrorSuccess = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isZipModeFinish(IntPtr L)
	{
		try
		{
			((AppStartupLoading)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isZipModeFinish = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
