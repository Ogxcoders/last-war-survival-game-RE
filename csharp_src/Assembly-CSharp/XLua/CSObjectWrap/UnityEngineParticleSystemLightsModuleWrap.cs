using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineParticleSystemLightsModuleWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ParticleSystem.LightsModule);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 12, 12);
		Utils.RegisterFunc(L, -2, "enabled", _g_get_enabled);
		Utils.RegisterFunc(L, -2, "ratio", _g_get_ratio);
		Utils.RegisterFunc(L, -2, "useRandomDistribution", _g_get_useRandomDistribution);
		Utils.RegisterFunc(L, -2, "light", _g_get_light);
		Utils.RegisterFunc(L, -2, "useParticleColor", _g_get_useParticleColor);
		Utils.RegisterFunc(L, -2, "sizeAffectsRange", _g_get_sizeAffectsRange);
		Utils.RegisterFunc(L, -2, "alphaAffectsIntensity", _g_get_alphaAffectsIntensity);
		Utils.RegisterFunc(L, -2, "range", _g_get_range);
		Utils.RegisterFunc(L, -2, "rangeMultiplier", _g_get_rangeMultiplier);
		Utils.RegisterFunc(L, -2, "intensity", _g_get_intensity);
		Utils.RegisterFunc(L, -2, "intensityMultiplier", _g_get_intensityMultiplier);
		Utils.RegisterFunc(L, -2, "maxLights", _g_get_maxLights);
		Utils.RegisterFunc(L, -1, "enabled", _s_set_enabled);
		Utils.RegisterFunc(L, -1, "ratio", _s_set_ratio);
		Utils.RegisterFunc(L, -1, "useRandomDistribution", _s_set_useRandomDistribution);
		Utils.RegisterFunc(L, -1, "light", _s_set_light);
		Utils.RegisterFunc(L, -1, "useParticleColor", _s_set_useParticleColor);
		Utils.RegisterFunc(L, -1, "sizeAffectsRange", _s_set_sizeAffectsRange);
		Utils.RegisterFunc(L, -1, "alphaAffectsIntensity", _s_set_alphaAffectsIntensity);
		Utils.RegisterFunc(L, -1, "range", _s_set_range);
		Utils.RegisterFunc(L, -1, "rangeMultiplier", _s_set_rangeMultiplier);
		Utils.RegisterFunc(L, -1, "intensity", _s_set_intensity);
		Utils.RegisterFunc(L, -1, "intensityMultiplier", _s_set_intensityMultiplier);
		Utils.RegisterFunc(L, -1, "maxLights", _s_set_maxLights);
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
				objectTranslator.Push(L, default(ParticleSystem.LightsModule));
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.LightsModule constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enabled(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.LightsModule v);
			Lua.lua_pushboolean(L, v.enabled);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ratio(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.LightsModule v);
			Lua.lua_pushnumber(L, v.ratio);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useRandomDistribution(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.LightsModule v);
			Lua.lua_pushboolean(L, v.useRandomDistribution);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_light(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.LightsModule v);
			objectTranslator.Push(L, v.light);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useParticleColor(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.LightsModule v);
			Lua.lua_pushboolean(L, v.useParticleColor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sizeAffectsRange(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.LightsModule v);
			Lua.lua_pushboolean(L, v.sizeAffectsRange);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_alphaAffectsIntensity(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.LightsModule v);
			Lua.lua_pushboolean(L, v.alphaAffectsIntensity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_range(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.LightsModule v);
			objectTranslator.Push(L, v.range);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rangeMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.LightsModule v);
			Lua.lua_pushnumber(L, v.rangeMultiplier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_intensity(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.LightsModule v);
			objectTranslator.Push(L, v.intensity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_intensityMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.LightsModule v);
			Lua.lua_pushnumber(L, v.intensityMultiplier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxLights(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.LightsModule v);
			Lua.xlua_pushinteger(L, v.maxLights);
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
			objectTranslator.Get(L, 1, out ParticleSystem.LightsModule v);
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
	private static int _s_set_ratio(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.LightsModule v);
			v.ratio = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useRandomDistribution(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.LightsModule v);
			v.useRandomDistribution = Lua.lua_toboolean(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_light(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.LightsModule v);
			v.light = (Light)objectTranslator.GetObject(L, 2, typeof(Light));
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useParticleColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.LightsModule v);
			v.useParticleColor = Lua.lua_toboolean(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sizeAffectsRange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.LightsModule v);
			v.sizeAffectsRange = Lua.lua_toboolean(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_alphaAffectsIntensity(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.LightsModule v);
			v.alphaAffectsIntensity = Lua.lua_toboolean(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_range(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.LightsModule v);
			objectTranslator.Get(L, 2, out ParticleSystem.MinMaxCurve v2);
			v.range = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_rangeMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.LightsModule v);
			v.rangeMultiplier = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_intensity(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.LightsModule v);
			objectTranslator.Get(L, 2, out ParticleSystem.MinMaxCurve v2);
			v.intensity = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_intensityMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.LightsModule v);
			v.intensityMultiplier = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maxLights(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.LightsModule v);
			v.maxLights = Lua.xlua_tointeger(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
