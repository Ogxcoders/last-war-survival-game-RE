using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LWBattleRVOAgentWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(LWBattleRVOAgent);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 4, 4, 4);
		Utils.RegisterFunc(L, -3, "Update", _m_Update);
		Utils.RegisterFunc(L, -3, "SetActive", _m_SetActive);
		Utils.RegisterFunc(L, -3, "SetTargetPosition", _m_SetTargetPosition);
		Utils.RegisterFunc(L, -3, "SetCurPosition", _m_SetCurPosition);
		Utils.RegisterFunc(L, -2, "sid", _g_get_sid);
		Utils.RegisterFunc(L, -2, "mgr", _g_get_mgr);
		Utils.RegisterFunc(L, -2, "speed", _g_get_speed);
		Utils.RegisterFunc(L, -2, "active", _g_get_active);
		Utils.RegisterFunc(L, -1, "sid", _s_set_sid);
		Utils.RegisterFunc(L, -1, "mgr", _s_set_mgr);
		Utils.RegisterFunc(L, -1, "speed", _s_set_speed);
		Utils.RegisterFunc(L, -1, "active", _s_set_active);
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
				LWBattleRVOAgent o = new LWBattleRVOAgent();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LWBattleRVOAgent constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Update(IntPtr L)
	{
		try
		{
			((LWBattleRVOAgent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Update();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetActive(IntPtr L)
	{
		try
		{
			LWBattleRVOAgent obj = (LWBattleRVOAgent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool active = Lua.lua_toboolean(L, 2);
			obj.SetActive(active);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTargetPosition(IntPtr L)
	{
		try
		{
			LWBattleRVOAgent obj = (LWBattleRVOAgent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float z = (float)Lua.lua_tonumber(L, 3);
			obj.SetTargetPosition(x, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetCurPosition(IntPtr L)
	{
		try
		{
			LWBattleRVOAgent obj = (LWBattleRVOAgent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float z = (float)Lua.lua_tonumber(L, 3);
			obj.SetCurPosition(x, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sid(IntPtr L)
	{
		try
		{
			LWBattleRVOAgent lWBattleRVOAgent = (LWBattleRVOAgent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, lWBattleRVOAgent.sid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mgr(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LWBattleRVOAgent lWBattleRVOAgent = (LWBattleRVOAgent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, lWBattleRVOAgent.mgr);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_speed(IntPtr L)
	{
		try
		{
			LWBattleRVOAgent lWBattleRVOAgent = (LWBattleRVOAgent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, lWBattleRVOAgent.speed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_active(IntPtr L)
	{
		try
		{
			LWBattleRVOAgent lWBattleRVOAgent = (LWBattleRVOAgent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, lWBattleRVOAgent.active);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sid(IntPtr L)
	{
		try
		{
			((LWBattleRVOAgent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).sid = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mgr(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((LWBattleRVOAgent)objectTranslator.FastGetCSObj(L, 1)).mgr = (LWBattleRVOManager)objectTranslator.GetObject(L, 2, typeof(LWBattleRVOManager));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_speed(IntPtr L)
	{
		try
		{
			((LWBattleRVOAgent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).speed = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_active(IntPtr L)
	{
		try
		{
			((LWBattleRVOAgent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).active = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
