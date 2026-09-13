using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineParticleSystemMinMaxGradientWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ParticleSystem.MinMaxGradient);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 1, 7, 7);
		Utils.RegisterFunc(L, -3, "Evaluate", _m_Evaluate);
		Utils.RegisterFunc(L, -2, "mode", _g_get_mode);
		Utils.RegisterFunc(L, -2, "gradientMax", _g_get_gradientMax);
		Utils.RegisterFunc(L, -2, "gradientMin", _g_get_gradientMin);
		Utils.RegisterFunc(L, -2, "colorMax", _g_get_colorMax);
		Utils.RegisterFunc(L, -2, "colorMin", _g_get_colorMin);
		Utils.RegisterFunc(L, -2, "color", _g_get_color);
		Utils.RegisterFunc(L, -2, "gradient", _g_get_gradient);
		Utils.RegisterFunc(L, -1, "mode", _s_set_mode);
		Utils.RegisterFunc(L, -1, "gradientMax", _s_set_gradientMax);
		Utils.RegisterFunc(L, -1, "gradientMin", _s_set_gradientMin);
		Utils.RegisterFunc(L, -1, "colorMax", _s_set_colorMax);
		Utils.RegisterFunc(L, -1, "colorMin", _s_set_colorMin);
		Utils.RegisterFunc(L, -1, "color", _s_set_color);
		Utils.RegisterFunc(L, -1, "gradient", _s_set_gradient);
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
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<Color>(L, 2))
			{
				objectTranslator.Get(L, 2, out Color val);
				ParticleSystem.MinMaxGradient minMaxGradient = new ParticleSystem.MinMaxGradient(val);
				objectTranslator.Push(L, minMaxGradient);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<Gradient>(L, 2))
			{
				Gradient gradient = (Gradient)objectTranslator.GetObject(L, 2, typeof(Gradient));
				ParticleSystem.MinMaxGradient minMaxGradient2 = new ParticleSystem.MinMaxGradient(gradient);
				objectTranslator.Push(L, minMaxGradient2);
				return 1;
			}
			if (Lua.lua_gettop(L) == 3 && objectTranslator.Assignable<Color>(L, 2) && objectTranslator.Assignable<Color>(L, 3))
			{
				objectTranslator.Get(L, 2, out Color val2);
				objectTranslator.Get(L, 3, out Color val3);
				ParticleSystem.MinMaxGradient minMaxGradient3 = new ParticleSystem.MinMaxGradient(val2, val3);
				objectTranslator.Push(L, minMaxGradient3);
				return 1;
			}
			if (Lua.lua_gettop(L) == 3 && objectTranslator.Assignable<Gradient>(L, 2) && objectTranslator.Assignable<Gradient>(L, 3))
			{
				Gradient min = (Gradient)objectTranslator.GetObject(L, 2, typeof(Gradient));
				Gradient max = (Gradient)objectTranslator.GetObject(L, 3, typeof(Gradient));
				ParticleSystem.MinMaxGradient minMaxGradient4 = new ParticleSystem.MinMaxGradient(min, max);
				objectTranslator.Push(L, minMaxGradient4);
				return 1;
			}
			if (Lua.lua_gettop(L) == 1)
			{
				objectTranslator.Push(L, default(ParticleSystem.MinMaxGradient));
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.MinMaxGradient constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Evaluate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxGradient v);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				float time = (float)Lua.lua_tonumber(L, 2);
				Color val = v.Evaluate(time);
				objectTranslator.PushUnityEngineColor(L, val);
				objectTranslator.Update(L, 1, v);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float time2 = (float)Lua.lua_tonumber(L, 2);
				float lerpFactor = (float)Lua.lua_tonumber(L, 3);
				Color val2 = v.Evaluate(time2, lerpFactor);
				objectTranslator.PushUnityEngineColor(L, val2);
				objectTranslator.Update(L, 1, v);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.MinMaxGradient.Evaluate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxGradient v);
			objectTranslator.Push(L, v.mode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_gradientMax(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxGradient v);
			objectTranslator.Push(L, v.gradientMax);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_gradientMin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxGradient v);
			objectTranslator.Push(L, v.gradientMin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_colorMax(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxGradient v);
			objectTranslator.PushUnityEngineColor(L, v.colorMax);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_colorMin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxGradient v);
			objectTranslator.PushUnityEngineColor(L, v.colorMin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_color(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxGradient v);
			objectTranslator.PushUnityEngineColor(L, v.color);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_gradient(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxGradient v);
			objectTranslator.Push(L, v.gradient);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxGradient v);
			objectTranslator.Get(L, 2, out ParticleSystemGradientMode v2);
			v.mode = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_gradientMax(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxGradient v);
			v.gradientMax = (Gradient)objectTranslator.GetObject(L, 2, typeof(Gradient));
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_gradientMin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxGradient v);
			v.gradientMin = (Gradient)objectTranslator.GetObject(L, 2, typeof(Gradient));
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_colorMax(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxGradient v);
			objectTranslator.Get(L, 2, out Color val);
			v.colorMax = val;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_colorMin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxGradient v);
			objectTranslator.Get(L, 2, out Color val);
			v.colorMin = val;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_color(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxGradient v);
			objectTranslator.Get(L, 2, out Color val);
			v.color = val;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_gradient(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxGradient v);
			v.gradient = (Gradient)objectTranslator.GetObject(L, 2, typeof(Gradient));
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
