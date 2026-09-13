using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LuaBuildDataWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(LuaBuildData);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 1, 6, 6);
		Utils.RegisterFunc(L, -3, "IsActive", _m_IsActive);
		Utils.RegisterFunc(L, -2, "uuid", _g_get_uuid);
		Utils.RegisterFunc(L, -2, "pointId", _g_get_pointId);
		Utils.RegisterFunc(L, -2, "state", _g_get_state);
		Utils.RegisterFunc(L, -2, "buildId", _g_get_buildId);
		Utils.RegisterFunc(L, -2, "level", _g_get_level);
		Utils.RegisterFunc(L, -2, "buildUpdateTime", _g_get_buildUpdateTime);
		Utils.RegisterFunc(L, -1, "uuid", _s_set_uuid);
		Utils.RegisterFunc(L, -1, "pointId", _s_set_pointId);
		Utils.RegisterFunc(L, -1, "state", _s_set_state);
		Utils.RegisterFunc(L, -1, "buildId", _s_set_buildId);
		Utils.RegisterFunc(L, -1, "level", _s_set_level);
		Utils.RegisterFunc(L, -1, "buildUpdateTime", _s_set_buildUpdateTime);
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
			if (Lua.lua_gettop(L) == 7 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7))
			{
				long uid = Lua.lua_toint64(L, 2);
				long updateTime = Lua.lua_toint64(L, 3);
				int point = Lua.xlua_tointeger(L, 4);
				int tempState = Lua.xlua_tointeger(L, 5);
				int itemId = Lua.xlua_tointeger(L, 6);
				int lv = Lua.xlua_tointeger(L, 7);
				LuaBuildData o = new LuaBuildData(uid, updateTime, point, tempState, itemId, lv);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (Lua.lua_gettop(L) == 1)
			{
				LuaBuildData o2 = new LuaBuildData();
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LuaBuildData constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsActive(IntPtr L)
	{
		try
		{
			bool value = ((LuaBuildData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsActive();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uuid(IntPtr L)
	{
		try
		{
			LuaBuildData luaBuildData = (LuaBuildData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, luaBuildData.uuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pointId(IntPtr L)
	{
		try
		{
			LuaBuildData luaBuildData = (LuaBuildData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, luaBuildData.pointId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_state(IntPtr L)
	{
		try
		{
			LuaBuildData luaBuildData = (LuaBuildData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, luaBuildData.state);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_buildId(IntPtr L)
	{
		try
		{
			LuaBuildData luaBuildData = (LuaBuildData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, luaBuildData.buildId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_level(IntPtr L)
	{
		try
		{
			LuaBuildData luaBuildData = (LuaBuildData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, luaBuildData.level);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_buildUpdateTime(IntPtr L)
	{
		try
		{
			LuaBuildData luaBuildData = (LuaBuildData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, luaBuildData.buildUpdateTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_uuid(IntPtr L)
	{
		try
		{
			((LuaBuildData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).uuid = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pointId(IntPtr L)
	{
		try
		{
			((LuaBuildData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).pointId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_state(IntPtr L)
	{
		try
		{
			((LuaBuildData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).state = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_buildId(IntPtr L)
	{
		try
		{
			((LuaBuildData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).buildId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_level(IntPtr L)
	{
		try
		{
			((LuaBuildData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).level = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_buildUpdateTime(IntPtr L)
	{
		try
		{
			((LuaBuildData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).buildUpdateTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
