using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineParticleSystemVelocityOverLifetimeModuleWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ParticleSystem.VelocityOverLifetimeModule);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 24, 24);
		Utils.RegisterFunc(L, -2, "enabled", _g_get_enabled);
		Utils.RegisterFunc(L, -2, "x", _g_get_x);
		Utils.RegisterFunc(L, -2, "y", _g_get_y);
		Utils.RegisterFunc(L, -2, "z", _g_get_z);
		Utils.RegisterFunc(L, -2, "xMultiplier", _g_get_xMultiplier);
		Utils.RegisterFunc(L, -2, "yMultiplier", _g_get_yMultiplier);
		Utils.RegisterFunc(L, -2, "zMultiplier", _g_get_zMultiplier);
		Utils.RegisterFunc(L, -2, "orbitalX", _g_get_orbitalX);
		Utils.RegisterFunc(L, -2, "orbitalY", _g_get_orbitalY);
		Utils.RegisterFunc(L, -2, "orbitalZ", _g_get_orbitalZ);
		Utils.RegisterFunc(L, -2, "orbitalXMultiplier", _g_get_orbitalXMultiplier);
		Utils.RegisterFunc(L, -2, "orbitalYMultiplier", _g_get_orbitalYMultiplier);
		Utils.RegisterFunc(L, -2, "orbitalZMultiplier", _g_get_orbitalZMultiplier);
		Utils.RegisterFunc(L, -2, "orbitalOffsetX", _g_get_orbitalOffsetX);
		Utils.RegisterFunc(L, -2, "orbitalOffsetY", _g_get_orbitalOffsetY);
		Utils.RegisterFunc(L, -2, "orbitalOffsetZ", _g_get_orbitalOffsetZ);
		Utils.RegisterFunc(L, -2, "orbitalOffsetXMultiplier", _g_get_orbitalOffsetXMultiplier);
		Utils.RegisterFunc(L, -2, "orbitalOffsetYMultiplier", _g_get_orbitalOffsetYMultiplier);
		Utils.RegisterFunc(L, -2, "orbitalOffsetZMultiplier", _g_get_orbitalOffsetZMultiplier);
		Utils.RegisterFunc(L, -2, "radial", _g_get_radial);
		Utils.RegisterFunc(L, -2, "radialMultiplier", _g_get_radialMultiplier);
		Utils.RegisterFunc(L, -2, "speedModifier", _g_get_speedModifier);
		Utils.RegisterFunc(L, -2, "speedModifierMultiplier", _g_get_speedModifierMultiplier);
		Utils.RegisterFunc(L, -2, "space", _g_get_space);
		Utils.RegisterFunc(L, -1, "enabled", _s_set_enabled);
		Utils.RegisterFunc(L, -1, "x", _s_set_x);
		Utils.RegisterFunc(L, -1, "y", _s_set_y);
		Utils.RegisterFunc(L, -1, "z", _s_set_z);
		Utils.RegisterFunc(L, -1, "xMultiplier", _s_set_xMultiplier);
		Utils.RegisterFunc(L, -1, "yMultiplier", _s_set_yMultiplier);
		Utils.RegisterFunc(L, -1, "zMultiplier", _s_set_zMultiplier);
		Utils.RegisterFunc(L, -1, "orbitalX", _s_set_orbitalX);
		Utils.RegisterFunc(L, -1, "orbitalY", _s_set_orbitalY);
		Utils.RegisterFunc(L, -1, "orbitalZ", _s_set_orbitalZ);
		Utils.RegisterFunc(L, -1, "orbitalXMultiplier", _s_set_orbitalXMultiplier);
		Utils.RegisterFunc(L, -1, "orbitalYMultiplier", _s_set_orbitalYMultiplier);
		Utils.RegisterFunc(L, -1, "orbitalZMultiplier", _s_set_orbitalZMultiplier);
		Utils.RegisterFunc(L, -1, "orbitalOffsetX", _s_set_orbitalOffsetX);
		Utils.RegisterFunc(L, -1, "orbitalOffsetY", _s_set_orbitalOffsetY);
		Utils.RegisterFunc(L, -1, "orbitalOffsetZ", _s_set_orbitalOffsetZ);
		Utils.RegisterFunc(L, -1, "orbitalOffsetXMultiplier", _s_set_orbitalOffsetXMultiplier);
		Utils.RegisterFunc(L, -1, "orbitalOffsetYMultiplier", _s_set_orbitalOffsetYMultiplier);
		Utils.RegisterFunc(L, -1, "orbitalOffsetZMultiplier", _s_set_orbitalOffsetZMultiplier);
		Utils.RegisterFunc(L, -1, "radial", _s_set_radial);
		Utils.RegisterFunc(L, -1, "radialMultiplier", _s_set_radialMultiplier);
		Utils.RegisterFunc(L, -1, "speedModifier", _s_set_speedModifier);
		Utils.RegisterFunc(L, -1, "speedModifierMultiplier", _s_set_speedModifierMultiplier);
		Utils.RegisterFunc(L, -1, "space", _s_set_space);
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
				objectTranslator.Push(L, default(ParticleSystem.VelocityOverLifetimeModule));
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.VelocityOverLifetimeModule constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enabled(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
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
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
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
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
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
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
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
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
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
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
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
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			Lua.lua_pushnumber(L, v.zMultiplier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_orbitalX(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			objectTranslator.Push(L, v.orbitalX);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_orbitalY(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			objectTranslator.Push(L, v.orbitalY);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_orbitalZ(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			objectTranslator.Push(L, v.orbitalZ);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_orbitalXMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			Lua.lua_pushnumber(L, v.orbitalXMultiplier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_orbitalYMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			Lua.lua_pushnumber(L, v.orbitalYMultiplier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_orbitalZMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			Lua.lua_pushnumber(L, v.orbitalZMultiplier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_orbitalOffsetX(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			objectTranslator.Push(L, v.orbitalOffsetX);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_orbitalOffsetY(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			objectTranslator.Push(L, v.orbitalOffsetY);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_orbitalOffsetZ(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			objectTranslator.Push(L, v.orbitalOffsetZ);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_orbitalOffsetXMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			Lua.lua_pushnumber(L, v.orbitalOffsetXMultiplier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_orbitalOffsetYMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			Lua.lua_pushnumber(L, v.orbitalOffsetYMultiplier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_orbitalOffsetZMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			Lua.lua_pushnumber(L, v.orbitalOffsetZMultiplier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_radial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			objectTranslator.Push(L, v.radial);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_radialMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			Lua.lua_pushnumber(L, v.radialMultiplier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_speedModifier(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			objectTranslator.Push(L, v.speedModifier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_speedModifierMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			Lua.lua_pushnumber(L, v.speedModifierMultiplier);
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
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			objectTranslator.Push(L, v.space);
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
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
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
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
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
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
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
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
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
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
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
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
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
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
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
	private static int _s_set_orbitalX(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			objectTranslator.Get(L, 2, out ParticleSystem.MinMaxCurve v2);
			v.orbitalX = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_orbitalY(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			objectTranslator.Get(L, 2, out ParticleSystem.MinMaxCurve v2);
			v.orbitalY = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_orbitalZ(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			objectTranslator.Get(L, 2, out ParticleSystem.MinMaxCurve v2);
			v.orbitalZ = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_orbitalXMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			v.orbitalXMultiplier = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_orbitalYMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			v.orbitalYMultiplier = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_orbitalZMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			v.orbitalZMultiplier = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_orbitalOffsetX(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			objectTranslator.Get(L, 2, out ParticleSystem.MinMaxCurve v2);
			v.orbitalOffsetX = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_orbitalOffsetY(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			objectTranslator.Get(L, 2, out ParticleSystem.MinMaxCurve v2);
			v.orbitalOffsetY = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_orbitalOffsetZ(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			objectTranslator.Get(L, 2, out ParticleSystem.MinMaxCurve v2);
			v.orbitalOffsetZ = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_orbitalOffsetXMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			v.orbitalOffsetXMultiplier = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_orbitalOffsetYMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			v.orbitalOffsetYMultiplier = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_orbitalOffsetZMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			v.orbitalOffsetZMultiplier = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_radial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			objectTranslator.Get(L, 2, out ParticleSystem.MinMaxCurve v2);
			v.radial = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_radialMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			v.radialMultiplier = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_speedModifier(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			objectTranslator.Get(L, 2, out ParticleSystem.MinMaxCurve v2);
			v.speedModifier = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_speedModifierMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
			v.speedModifierMultiplier = (float)Lua.lua_tonumber(L, 2);
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
			objectTranslator.Get(L, 1, out ParticleSystem.VelocityOverLifetimeModule v);
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
}
