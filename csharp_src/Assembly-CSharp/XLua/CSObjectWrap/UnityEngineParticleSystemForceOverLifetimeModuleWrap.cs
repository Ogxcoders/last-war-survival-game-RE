using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineParticleSystemForceOverLifetimeModuleWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ParticleSystem.ForceOverLifetimeModule);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 9, 9);
		Utils.RegisterFunc(L, -2, "enabled", _g_get_enabled);
		Utils.RegisterFunc(L, -2, "x", _g_get_x);
		Utils.RegisterFunc(L, -2, "y", _g_get_y);
		Utils.RegisterFunc(L, -2, "z", _g_get_z);
		Utils.RegisterFunc(L, -2, "xMultiplier", _g_get_xMultiplier);
		Utils.RegisterFunc(L, -2, "yMultiplier", _g_get_yMultiplier);
		Utils.RegisterFunc(L, -2, "zMultiplier", _g_get_zMultiplier);
		Utils.RegisterFunc(L, -2, "space", _g_get_space);
		Utils.RegisterFunc(L, -2, "randomized", _g_get_randomized);
		Utils.RegisterFunc(L, -1, "enabled", _s_set_enabled);
		Utils.RegisterFunc(L, -1, "x", _s_set_x);
		Utils.RegisterFunc(L, -1, "y", _s_set_y);
		Utils.RegisterFunc(L, -1, "z", _s_set_z);
		Utils.RegisterFunc(L, -1, "xMultiplier", _s_set_xMultiplier);
		Utils.RegisterFunc(L, -1, "yMultiplier", _s_set_yMultiplier);
		Utils.RegisterFunc(L, -1, "zMultiplier", _s_set_zMultiplier);
		Utils.RegisterFunc(L, -1, "space", _s_set_space);
		Utils.RegisterFunc(L, -1, "randomized", _s_set_randomized);
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
				objectTranslator.Push(L, default(ParticleSystem.ForceOverLifetimeModule));
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.ForceOverLifetimeModule constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enabled(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ForceOverLifetimeModule v);
			Lua.lua_pushboolean(L, v.enabled);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_x(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ForceOverLifetimeModule v);
			objectTranslator.Push(L, v.x);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_y(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ForceOverLifetimeModule v);
			objectTranslator.Push(L, v.y);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_z(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ForceOverLifetimeModule v);
			objectTranslator.Push(L, v.z);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_xMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ForceOverLifetimeModule v);
			Lua.lua_pushnumber(L, v.xMultiplier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_yMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ForceOverLifetimeModule v);
			Lua.lua_pushnumber(L, v.yMultiplier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_zMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ForceOverLifetimeModule v);
			Lua.lua_pushnumber(L, v.zMultiplier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_space(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ForceOverLifetimeModule v);
			objectTranslator.Push(L, v.space);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_randomized(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.ForceOverLifetimeModule v);
			Lua.lua_pushboolean(L, v.randomized);
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
			objectTranslator.Get(L, 1, out ParticleSystem.ForceOverLifetimeModule v);
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
	private static int _s_set_x(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ForceOverLifetimeModule v);
			objectTranslator.Get(L, 2, out ParticleSystem.MinMaxCurve v2);
			v.x = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_y(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ForceOverLifetimeModule v);
			objectTranslator.Get(L, 2, out ParticleSystem.MinMaxCurve v2);
			v.y = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_z(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ForceOverLifetimeModule v);
			objectTranslator.Get(L, 2, out ParticleSystem.MinMaxCurve v2);
			v.z = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_xMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ForceOverLifetimeModule v);
			v.xMultiplier = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_yMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ForceOverLifetimeModule v);
			v.yMultiplier = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_zMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ForceOverLifetimeModule v);
			v.zMultiplier = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_space(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ForceOverLifetimeModule v);
			objectTranslator.Get(L, 2, out ParticleSystemSimulationSpace v2);
			v.space = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_randomized(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.ForceOverLifetimeModule v);
			v.randomized = Lua.lua_toboolean(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
