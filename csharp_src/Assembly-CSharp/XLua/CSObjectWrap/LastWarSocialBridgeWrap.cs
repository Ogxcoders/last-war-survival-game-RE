using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LastWarSocialBridgeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(LastWarSocialBridge);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 5, 0, 0);
		Utils.RegisterFunc(L, -4, "SubmitScore", _m_SubmitScore_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowLeaderboard", _m_ShowLeaderboard_xlua_st_);
		Utils.RegisterFunc(L, -4, "ReportAchievement", _m_ReportAchievement_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowAchievements", _m_ShowAchievements_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "LastWarSocialBridge does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SubmitScore_xlua_st_(IntPtr L)
	{
		try
		{
			string leaderboardID = Lua.lua_tostring(L, 1);
			long score = Lua.lua_toint64(L, 2);
			LastWarSocialBridge.SubmitScore(leaderboardID, score);
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
			LastWarSocialBridge.ShowLeaderboard(Lua.lua_tostring(L, 1));
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
			LastWarSocialBridge.ReportAchievement(achievementID, percentComplete);
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
			LastWarSocialBridge.ShowAchievements();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
