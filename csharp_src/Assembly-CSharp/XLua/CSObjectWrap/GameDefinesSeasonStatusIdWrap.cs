using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameDefinesSeasonStatusIdWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GameDefines.SeasonStatusId);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 3, 3);
		Utils.RegisterFunc(L, -2, "SEASON_MUMMY_CURSE1", _g_get_SEASON_MUMMY_CURSE1);
		Utils.RegisterFunc(L, -2, "SEASON_MUMMY_CURSE2", _g_get_SEASON_MUMMY_CURSE2);
		Utils.RegisterFunc(L, -2, "SEASON_MUMMY_CURSE3", _g_get_SEASON_MUMMY_CURSE3);
		Utils.RegisterFunc(L, -1, "SEASON_MUMMY_CURSE1", _s_set_SEASON_MUMMY_CURSE1);
		Utils.RegisterFunc(L, -1, "SEASON_MUMMY_CURSE2", _s_set_SEASON_MUMMY_CURSE2);
		Utils.RegisterFunc(L, -1, "SEASON_MUMMY_CURSE3", _s_set_SEASON_MUMMY_CURSE3);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "GameDefines.SeasonStatusId does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SEASON_MUMMY_CURSE1(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, GameDefines.SeasonStatusId.SEASON_MUMMY_CURSE1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SEASON_MUMMY_CURSE2(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, GameDefines.SeasonStatusId.SEASON_MUMMY_CURSE2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SEASON_MUMMY_CURSE3(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, GameDefines.SeasonStatusId.SEASON_MUMMY_CURSE3);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_SEASON_MUMMY_CURSE1(IntPtr L)
	{
		try
		{
			GameDefines.SeasonStatusId.SEASON_MUMMY_CURSE1 = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_SEASON_MUMMY_CURSE2(IntPtr L)
	{
		try
		{
			GameDefines.SeasonStatusId.SEASON_MUMMY_CURSE2 = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_SEASON_MUMMY_CURSE3(IntPtr L)
	{
		try
		{
			GameDefines.SeasonStatusId.SEASON_MUMMY_CURSE3 = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
