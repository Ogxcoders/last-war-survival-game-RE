using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameCenterAuthDataWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GameCenterAuthData);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 7, 7);
		Utils.RegisterFunc(L, -2, "displayName", _g_get_displayName);
		Utils.RegisterFunc(L, -2, "playerID", _g_get_playerID);
		Utils.RegisterFunc(L, -2, "teamPlayerID", _g_get_teamPlayerID);
		Utils.RegisterFunc(L, -2, "publicKeyUrl", _g_get_publicKeyUrl);
		Utils.RegisterFunc(L, -2, "signature", _g_get_signature);
		Utils.RegisterFunc(L, -2, "salt", _g_get_salt);
		Utils.RegisterFunc(L, -2, "timestamp", _g_get_timestamp);
		Utils.RegisterFunc(L, -1, "displayName", _s_set_displayName);
		Utils.RegisterFunc(L, -1, "playerID", _s_set_playerID);
		Utils.RegisterFunc(L, -1, "teamPlayerID", _s_set_teamPlayerID);
		Utils.RegisterFunc(L, -1, "publicKeyUrl", _s_set_publicKeyUrl);
		Utils.RegisterFunc(L, -1, "signature", _s_set_signature);
		Utils.RegisterFunc(L, -1, "salt", _s_set_salt);
		Utils.RegisterFunc(L, -1, "timestamp", _s_set_timestamp);
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
				GameCenterAuthData o = new GameCenterAuthData();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GameCenterAuthData constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_displayName(IntPtr L)
	{
		try
		{
			GameCenterAuthData gameCenterAuthData = (GameCenterAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, gameCenterAuthData.displayName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_playerID(IntPtr L)
	{
		try
		{
			GameCenterAuthData gameCenterAuthData = (GameCenterAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, gameCenterAuthData.playerID);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_teamPlayerID(IntPtr L)
	{
		try
		{
			GameCenterAuthData gameCenterAuthData = (GameCenterAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, gameCenterAuthData.teamPlayerID);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_publicKeyUrl(IntPtr L)
	{
		try
		{
			GameCenterAuthData gameCenterAuthData = (GameCenterAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, gameCenterAuthData.publicKeyUrl);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_signature(IntPtr L)
	{
		try
		{
			GameCenterAuthData gameCenterAuthData = (GameCenterAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, gameCenterAuthData.signature);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_salt(IntPtr L)
	{
		try
		{
			GameCenterAuthData gameCenterAuthData = (GameCenterAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, gameCenterAuthData.salt);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_timestamp(IntPtr L)
	{
		try
		{
			GameCenterAuthData gameCenterAuthData = (GameCenterAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, gameCenterAuthData.timestamp);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_displayName(IntPtr L)
	{
		try
		{
			((GameCenterAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).displayName = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_playerID(IntPtr L)
	{
		try
		{
			((GameCenterAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).playerID = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_teamPlayerID(IntPtr L)
	{
		try
		{
			((GameCenterAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).teamPlayerID = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_publicKeyUrl(IntPtr L)
	{
		try
		{
			((GameCenterAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).publicKeyUrl = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_signature(IntPtr L)
	{
		try
		{
			((GameCenterAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).signature = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_salt(IntPtr L)
	{
		try
		{
			((GameCenterAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).salt = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_timestamp(IntPtr L)
	{
		try
		{
			((GameCenterAuthData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).timestamp = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
