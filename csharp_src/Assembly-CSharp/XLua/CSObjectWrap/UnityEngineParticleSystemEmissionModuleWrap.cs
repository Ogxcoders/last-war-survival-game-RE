using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineParticleSystemEmissionModuleWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ParticleSystem.EmissionModule);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 4, 6, 6);
		Utils.RegisterFunc(L, -3, "SetBursts", _m_SetBursts);
		Utils.RegisterFunc(L, -3, "GetBursts", _m_GetBursts);
		Utils.RegisterFunc(L, -3, "SetBurst", _m_SetBurst);
		Utils.RegisterFunc(L, -3, "GetBurst", _m_GetBurst);
		Utils.RegisterFunc(L, -2, "enabled", _g_get_enabled);
		Utils.RegisterFunc(L, -2, "rateOverTime", _g_get_rateOverTime);
		Utils.RegisterFunc(L, -2, "rateOverTimeMultiplier", _g_get_rateOverTimeMultiplier);
		Utils.RegisterFunc(L, -2, "rateOverDistance", _g_get_rateOverDistance);
		Utils.RegisterFunc(L, -2, "rateOverDistanceMultiplier", _g_get_rateOverDistanceMultiplier);
		Utils.RegisterFunc(L, -2, "burstCount", _g_get_burstCount);
		Utils.RegisterFunc(L, -1, "enabled", _s_set_enabled);
		Utils.RegisterFunc(L, -1, "rateOverTime", _s_set_rateOverTime);
		Utils.RegisterFunc(L, -1, "rateOverTimeMultiplier", _s_set_rateOverTimeMultiplier);
		Utils.RegisterFunc(L, -1, "rateOverDistance", _s_set_rateOverDistance);
		Utils.RegisterFunc(L, -1, "rateOverDistanceMultiplier", _s_set_rateOverDistanceMultiplier);
		Utils.RegisterFunc(L, -1, "burstCount", _s_set_burstCount);
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
				objectTranslator.Push(L, default(ParticleSystem.EmissionModule));
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.EmissionModule constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetBursts(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.EmissionModule v);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<ParticleSystem.Burst[]>(L, 2))
			{
				ParticleSystem.Burst[] bursts = (ParticleSystem.Burst[])objectTranslator.GetObject(L, 2, typeof(ParticleSystem.Burst[]));
				v.SetBursts(bursts);
				objectTranslator.Update(L, 1, v);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<ParticleSystem.Burst[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				ParticleSystem.Burst[] bursts2 = (ParticleSystem.Burst[])objectTranslator.GetObject(L, 2, typeof(ParticleSystem.Burst[]));
				int size = Lua.xlua_tointeger(L, 3);
				v.SetBursts(bursts2, size);
				objectTranslator.Update(L, 1, v);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.EmissionModule.SetBursts!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBursts(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.EmissionModule v);
			ParticleSystem.Burst[] bursts = (ParticleSystem.Burst[])objectTranslator.GetObject(L, 2, typeof(ParticleSystem.Burst[]));
			int bursts2 = v.GetBursts(bursts);
			Lua.xlua_pushinteger(L, bursts2);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetBurst(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.EmissionModule v);
			int index = Lua.xlua_tointeger(L, 2);
			objectTranslator.Get(L, 3, out ParticleSystem.Burst v2);
			v.SetBurst(index, v2);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBurst(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.EmissionModule v);
			int index = Lua.xlua_tointeger(L, 2);
			ParticleSystem.Burst burst = v.GetBurst(index);
			objectTranslator.Push(L, burst);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enabled(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.EmissionModule v);
			Lua.lua_pushboolean(L, v.enabled);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rateOverTime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.EmissionModule v);
			objectTranslator.Push(L, v.rateOverTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rateOverTimeMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.EmissionModule v);
			Lua.lua_pushnumber(L, v.rateOverTimeMultiplier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rateOverDistance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.EmissionModule v);
			objectTranslator.Push(L, v.rateOverDistance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rateOverDistanceMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.EmissionModule v);
			Lua.lua_pushnumber(L, v.rateOverDistanceMultiplier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_burstCount(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.EmissionModule v);
			Lua.xlua_pushinteger(L, v.burstCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_enabled(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.EmissionModule v);
			v.enabled = Lua.lua_toboolean(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_rateOverTime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.EmissionModule v);
			objectTranslator.Get(L, 2, out ParticleSystem.MinMaxCurve v2);
			v.rateOverTime = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_rateOverTimeMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.EmissionModule v);
			v.rateOverTimeMultiplier = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_rateOverDistance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.EmissionModule v);
			objectTranslator.Get(L, 2, out ParticleSystem.MinMaxCurve v2);
			v.rateOverDistance = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_rateOverDistanceMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.EmissionModule v);
			v.rateOverDistanceMultiplier = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_burstCount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.EmissionModule v);
			v.burstCount = Lua.xlua_tointeger(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
