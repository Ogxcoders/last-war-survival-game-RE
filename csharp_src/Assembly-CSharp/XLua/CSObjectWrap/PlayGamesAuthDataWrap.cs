using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class PlayGamesAuthDataWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(PlayGamesAuthData);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 6, 6);
		Utils.RegisterFunc(L, -2, "authCode", _g_get_authCode);
		Utils.RegisterFunc(L, -2, "playerId", _g_get_playerId);
		Utils.RegisterFunc(L, -2, "displayName", _g_get_displayName);
		Utils.RegisterFunc(L, -2, "success", _g_get_success);
		Utils.RegisterFunc(L, -2, "code", _g_get_code);
		Utils.RegisterFunc(L, -2, "error", _g_get_error);
		Utils.RegisterFunc(L, -1, "authCode", _s_set_authCode);
		Utils.RegisterFunc(L, -1, "playerId", _s_set_playerId);
		Utils.RegisterFunc(L, -1, "displayName", _s_set_displayName);
		Utils.RegisterFunc(L, -1, "success", _s_set_success);
		Utils.RegisterFunc(L, -1, "code", _s_set_code);
		Utils.RegisterFunc(L, -1, "error", _s_set_error);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
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
				PlayGamesAuthData o = new PlayGamesAuthData();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PlayGamesAuthData constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_authCode(IntPtr L)
	{
		try
		{
			PlayGamesAuthData playGamesAuthData = (PlayGamesAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, playGamesAuthData.authCode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_playerId(IntPtr L)
	{
		try
		{
			PlayGamesAuthData playGamesAuthData = (PlayGamesAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, playGamesAuthData.playerId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_displayName(IntPtr L)
	{
		try
		{
			PlayGamesAuthData playGamesAuthData = (PlayGamesAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, playGamesAuthData.displayName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_success(IntPtr L)
	{
		try
		{
			PlayGamesAuthData playGamesAuthData = (PlayGamesAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, playGamesAuthData.success);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_code(IntPtr L)
	{
		try
		{
			PlayGamesAuthData playGamesAuthData = (PlayGamesAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, playGamesAuthData.code);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_error(IntPtr L)
	{
		try
		{
			PlayGamesAuthData playGamesAuthData = (PlayGamesAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, playGamesAuthData.error);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_authCode(IntPtr L)
	{
		try
		{
			((PlayGamesAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).authCode = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_playerId(IntPtr L)
	{
		try
		{
			((PlayGamesAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).playerId = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_displayName(IntPtr L)
	{
		try
		{
			((PlayGamesAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).displayName = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_success(IntPtr L)
	{
		try
		{
			((PlayGamesAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).success = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_code(IntPtr L)
	{
		try
		{
			((PlayGamesAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).code = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_error(IntPtr L)
	{
		try
		{
			((PlayGamesAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).error = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
