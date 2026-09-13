using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineParticleSystemBurstWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ParticleSystem.Burst);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 7, 7);
		Utils.RegisterFunc(L, -2, "time", _g_get_time);
		Utils.RegisterFunc(L, -2, "count", _g_get_count);
		Utils.RegisterFunc(L, -2, "minCount", _g_get_minCount);
		Utils.RegisterFunc(L, -2, "maxCount", _g_get_maxCount);
		Utils.RegisterFunc(L, -2, "cycleCount", _g_get_cycleCount);
		Utils.RegisterFunc(L, -2, "repeatInterval", _g_get_repeatInterval);
		Utils.RegisterFunc(L, -2, "probability", _g_get_probability);
		Utils.RegisterFunc(L, -1, "time", _s_set_time);
		Utils.RegisterFunc(L, -1, "count", _s_set_count);
		Utils.RegisterFunc(L, -1, "minCount", _s_set_minCount);
		Utils.RegisterFunc(L, -1, "maxCount", _s_set_maxCount);
		Utils.RegisterFunc(L, -1, "cycleCount", _s_set_cycleCount);
		Utils.RegisterFunc(L, -1, "repeatInterval", _s_set_repeatInterval);
		Utils.RegisterFunc(L, -1, "probability", _s_set_probability);
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
			if (Lua.lua_gettop(L) == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float time = (float)Lua.lua_tonumber(L, 2);
				short count = (short)Lua.xlua_tointeger(L, 3);
				ParticleSystem.Burst burst = new ParticleSystem.Burst(time, count);
				objectTranslator.Push(L, burst);
				return 1;
			}
			if (Lua.lua_gettop(L) == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float time2 = (float)Lua.lua_tonumber(L, 2);
				short minCount = (short)Lua.xlua_tointeger(L, 3);
				short maxCount = (short)Lua.xlua_tointeger(L, 4);
				ParticleSystem.Burst burst2 = new ParticleSystem.Burst(time2, minCount, maxCount);
				objectTranslator.Push(L, burst2);
				return 1;
			}
			if (Lua.lua_gettop(L) == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				float time3 = (float)Lua.lua_tonumber(L, 2);
				short minCount2 = (short)Lua.xlua_tointeger(L, 3);
				short maxCount2 = (short)Lua.xlua_tointeger(L, 4);
				int cycleCount = Lua.xlua_tointeger(L, 5);
				float repeatInterval = (float)Lua.lua_tonumber(L, 6);
				ParticleSystem.Burst burst3 = new ParticleSystem.Burst(time3, minCount2, maxCount2, cycleCount, repeatInterval);
				objectTranslator.Push(L, burst3);
				return 1;
			}
			if (Lua.lua_gettop(L) == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<ParticleSystem.MinMaxCurve>(L, 3))
			{
				float time4 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out ParticleSystem.MinMaxCurve v);
				ParticleSystem.Burst burst4 = new ParticleSystem.Burst(time4, v);
				objectTranslator.Push(L, burst4);
				return 1;
			}
			if (Lua.lua_gettop(L) == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<ParticleSystem.MinMaxCurve>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				float time5 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out ParticleSystem.MinMaxCurve v2);
				int cycleCount2 = Lua.xlua_tointeger(L, 4);
				float repeatInterval2 = (float)Lua.lua_tonumber(L, 5);
				ParticleSystem.Burst burst5 = new ParticleSystem.Burst(time5, v2, cycleCount2, repeatInterval2);
				objectTranslator.Push(L, burst5);
				return 1;
			}
			if (Lua.lua_gettop(L) == 1)
			{
				objectTranslator.Push(L, default(ParticleSystem.Burst));
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.Burst constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_time(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.Burst v);
			Lua.lua_pushnumber(L, v.time);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_count(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.Burst v);
			objectTranslator.Push(L, v.count);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_minCount(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.Burst v);
			Lua.xlua_pushinteger(L, v.minCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxCount(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.Burst v);
			Lua.xlua_pushinteger(L, v.maxCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cycleCount(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.Burst v);
			Lua.xlua_pushinteger(L, v.cycleCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_repeatInterval(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.Burst v);
			Lua.lua_pushnumber(L, v.repeatInterval);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_probability(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.Burst v);
			Lua.lua_pushnumber(L, v.probability);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_time(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.Burst v);
			v.time = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_count(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.Burst v);
			objectTranslator.Get(L, 2, out ParticleSystem.MinMaxCurve v2);
			v.count = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_minCount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.Burst v);
			v.minCount = (short)Lua.xlua_tointeger(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maxCount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.Burst v);
			v.maxCount = (short)Lua.xlua_tointeger(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cycleCount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.Burst v);
			v.cycleCount = Lua.xlua_tointeger(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_repeatInterval(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.Burst v);
			v.repeatInterval = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_probability(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.Burst v);
			v.probability = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
