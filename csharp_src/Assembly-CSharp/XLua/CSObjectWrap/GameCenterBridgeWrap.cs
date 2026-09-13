using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameCenterBridgeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GameCenterBridge);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 12, 0, 0);
		Utils.RegisterFunc(L, -4, "IsGameCenterAvailable", _m_IsGameCenterAvailable_xlua_st_);
		Utils.RegisterFunc(L, -4, "AutoLogin", _m_AutoLogin_xlua_st_);
		Utils.RegisterFunc(L, -4, "Login", _m_Login_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetAutoLoginCallback", _m_SetAutoLoginCallback_xlua_st_);
		Utils.RegisterFunc(L, -4, "OnNativeCallback", _m_OnNativeCallback_xlua_st_);
		Utils.RegisterFunc(L, -4, "SubmitScore", _m_SubmitScore_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowLeaderboard", _m_ShowLeaderboard_xlua_st_);
		Utils.RegisterFunc(L, -4, "ReportAchievement", _m_ReportAchievement_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowAchievements", _m_ShowAchievements_xlua_st_);
		Utils.RegisterFunc(L, -4, "DebugLogError", _m_DebugLogError_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsIOSVersionOrGreater", _m_IsIOSVersionOrGreater_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "GameCenterBridge does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsGameCenterAvailable_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = GameCenterBridge.IsGameCenterAvailable();
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
			GameCenterBridge.AutoLogin();
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
			GameCenterBridge.Login(ObjectTranslatorPool.Instance.Find(L).GetDelegate<Action<bool, GameCenterAuthData>>(L, 1));
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
			GameCenterBridge.SetAutoLoginCallback(ObjectTranslatorPool.Instance.Find(L).GetDelegate<Action<bool, GameCenterAuthData>>(L, 1));
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
			GameCenterBridge.OnNativeCallback(funcName, data);
			return 0;
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
			GameCenterBridge.SubmitScore(leaderboardID, score);
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
			GameCenterBridge.ShowLeaderboard(Lua.lua_tostring(L, 1));
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
			GameCenterBridge.ReportAchievement(achievementID, percentComplete);
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
			GameCenterBridge.ShowAchievements();
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
			GameCenterBridge.DebugLogError(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsIOSVersionOrGreater_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = GameCenterBridge.IsIOSVersionOrGreater(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
