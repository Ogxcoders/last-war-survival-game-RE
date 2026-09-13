using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class PlayGamesBridgeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(PlayGamesBridge);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 16, 0, 0);
		Utils.RegisterFunc(L, -4, "ConvertName", _m_ConvertName_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsPlayGamesAvailable", _m_IsPlayGamesAvailable_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsClientSwitchOn", _m_IsClientSwitchOn_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsGrayDevice", _m_IsGrayDevice_xlua_st_);
		Utils.RegisterFunc(L, -4, "AutoLogin", _m_AutoLogin_xlua_st_);
		Utils.RegisterFunc(L, -4, "Login", _m_Login_xlua_st_);
		Utils.RegisterFunc(L, -4, "LoginSilently", _m_LoginSilently_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetAutoLoginCallback", _m_SetAutoLoginCallback_xlua_st_);
		Utils.RegisterFunc(L, -4, "OnNativeCallback", _m_OnNativeCallback_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetAuthData", _m_GetAuthData_xlua_st_);
		Utils.RegisterFunc(L, -4, "SubmitScore", _m_SubmitScore_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowLeaderboard", _m_ShowLeaderboard_xlua_st_);
		Utils.RegisterFunc(L, -4, "ReportAchievement", _m_ReportAchievement_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowAchievements", _m_ShowAchievements_xlua_st_);
		Utils.RegisterFunc(L, -4, "DebugLogError", _m_DebugLogError_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "PlayGamesBridge does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ConvertName_xlua_st_(IntPtr L)
	{
		try
		{
			string str = PlayGamesBridge.ConvertName(Lua.lua_tostring(L, 1));
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsPlayGamesAvailable_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = PlayGamesBridge.IsPlayGamesAvailable();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsClientSwitchOn_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = PlayGamesBridge.IsClientSwitchOn();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsGrayDevice_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = PlayGamesBridge.IsGrayDevice();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AutoLogin_xlua_st_(IntPtr L)
	{
		try
		{
			PlayGamesBridge.AutoLogin();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Login_xlua_st_(IntPtr L)
	{
		try
		{
			PlayGamesBridge.Login(ObjectTranslatorPool.Instance.Find(L).GetDelegate<Action<PlayGamesAuthData>>(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoginSilently_xlua_st_(IntPtr L)
	{
		try
		{
			PlayGamesBridge.LoginSilently(ObjectTranslatorPool.Instance.Find(L).GetDelegate<Action<PlayGamesAuthData>>(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAutoLoginCallback_xlua_st_(IntPtr L)
	{
		try
		{
			PlayGamesBridge.SetAutoLoginCallback(ObjectTranslatorPool.Instance.Find(L).GetDelegate<Action<PlayGamesAuthData>>(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnNativeCallback_xlua_st_(IntPtr L)
	{
		try
		{
			string funcName = Lua.lua_tostring(L, 1);
			string data = Lua.lua_tostring(L, 2);
			PlayGamesBridge.OnNativeCallback(funcName, data);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAuthData_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PlayGamesAuthData authData = PlayGamesBridge.GetAuthData();
			objectTranslator.Push(L, authData);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SubmitScore_xlua_st_(IntPtr L)
	{
		try
		{
			string leaderboardID = Lua.lua_tostring(L, 1);
			long score = Lua.lua_toint64(L, 2);
			PlayGamesBridge.SubmitScore(leaderboardID, score);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowLeaderboard_xlua_st_(IntPtr L)
	{
		try
		{
			PlayGamesBridge.ShowLeaderboard(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReportAchievement_xlua_st_(IntPtr L)
	{
		try
		{
			string achievementID = Lua.lua_tostring(L, 1);
			double percentComplete = Lua.lua_tonumber(L, 2);
			PlayGamesBridge.ReportAchievement(achievementID, percentComplete);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowAchievements_xlua_st_(IntPtr L)
	{
		try
		{
			PlayGamesBridge.ShowAchievements();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DebugLogError_xlua_st_(IntPtr L)
	{
		try
		{
			PlayGamesBridge.DebugLogError(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
