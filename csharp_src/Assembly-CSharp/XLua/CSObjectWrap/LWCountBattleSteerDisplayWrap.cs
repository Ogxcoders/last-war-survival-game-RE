using System;
using LW.CountBattle;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LWCountBattleSteerDisplayWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SteerDisplay);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 1, 0);
		Utils.RegisterFunc(L, -3, "Push", _m_Push);
		Utils.RegisterFunc(L, -3, "Update", _m_Update);
		Utils.RegisterFunc(L, -2, "Position", _g_get_Position);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 2, 2);
		Utils.RegisterFunc(L, -2, "AUTO_INC_ID", _g_get_AUTO_INC_ID);
		Utils.RegisterFunc(L, -2, "DELAY_TICKS", _g_get_DELAY_TICKS);
		Utils.RegisterFunc(L, -1, "AUTO_INC_ID", _s_set_AUTO_INC_ID);
		Utils.RegisterFunc(L, -1, "DELAY_TICKS", _s_set_DELAY_TICKS);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				float firstTickTimeFix = (float)Lua.lua_tonumber(L, 3);
				SteerDisplay o = new SteerDisplay(val, firstTickTimeFix);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				SteerDisplay o2 = new SteerDisplay(val2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LW.CountBattle.SteerDisplay constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Push(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerDisplay steerDisplay = (SteerDisplay)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			float timestamp = (float)Lua.lua_tonumber(L, 3);
			steerDisplay.Push(val, timestamp);
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
			SteerDisplay obj = (SteerDisplay)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float dt = (float)Lua.lua_tonumber(L, 2);
			obj.Update(dt);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Position(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerDisplay steerDisplay = (SteerDisplay)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, steerDisplay.Position);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AUTO_INC_ID(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SteerDisplay.AUTO_INC_ID);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DELAY_TICKS(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SteerDisplay.DELAY_TICKS);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_AUTO_INC_ID(IntPtr L)
	{
		try
		{
			SteerDisplay.AUTO_INC_ID = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_DELAY_TICKS(IntPtr L)
	{
		try
		{
			SteerDisplay.DELAY_TICKS = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
