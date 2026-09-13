using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineParticleSystemMinMaxCurveWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ParticleSystem.MinMaxCurve);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 1, 8, 8);
		Utils.RegisterFunc(L, -3, "Evaluate", _m_Evaluate);
		Utils.RegisterFunc(L, -2, "mode", _g_get_mode);
		Utils.RegisterFunc(L, -2, "curveMultiplier", _g_get_curveMultiplier);
		Utils.RegisterFunc(L, -2, "curveMax", _g_get_curveMax);
		Utils.RegisterFunc(L, -2, "curveMin", _g_get_curveMin);
		Utils.RegisterFunc(L, -2, "constantMax", _g_get_constantMax);
		Utils.RegisterFunc(L, -2, "constantMin", _g_get_constantMin);
		Utils.RegisterFunc(L, -2, "constant", _g_get_constant);
		Utils.RegisterFunc(L, -2, "curve", _g_get_curve);
		Utils.RegisterFunc(L, -1, "mode", _s_set_mode);
		Utils.RegisterFunc(L, -1, "curveMultiplier", _s_set_curveMultiplier);
		Utils.RegisterFunc(L, -1, "curveMax", _s_set_curveMax);
		Utils.RegisterFunc(L, -1, "curveMin", _s_set_curveMin);
		Utils.RegisterFunc(L, -1, "constantMax", _s_set_constantMax);
		Utils.RegisterFunc(L, -1, "constantMin", _s_set_constantMin);
		Utils.RegisterFunc(L, -1, "constant", _s_set_constant);
		Utils.RegisterFunc(L, -1, "curve", _s_set_curve);
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
			if (Lua.lua_gettop(L) == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				float constant = (float)Lua.lua_tonumber(L, 2);
				ParticleSystem.MinMaxCurve minMaxCurve = new ParticleSystem.MinMaxCurve(constant);
				objectTranslator.Push(L, minMaxCurve);
				return 1;
			}
			if (Lua.lua_gettop(L) == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<AnimationCurve>(L, 3))
			{
				float multiplier = (float)Lua.lua_tonumber(L, 2);
				AnimationCurve curve = (AnimationCurve)objectTranslator.GetObject(L, 3, typeof(AnimationCurve));
				ParticleSystem.MinMaxCurve minMaxCurve2 = new ParticleSystem.MinMaxCurve(multiplier, curve);
				objectTranslator.Push(L, minMaxCurve2);
				return 1;
			}
			if (Lua.lua_gettop(L) == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<AnimationCurve>(L, 3) && objectTranslator.Assignable<AnimationCurve>(L, 4))
			{
				float multiplier2 = (float)Lua.lua_tonumber(L, 2);
				AnimationCurve min = (AnimationCurve)objectTranslator.GetObject(L, 3, typeof(AnimationCurve));
				AnimationCurve max = (AnimationCurve)objectTranslator.GetObject(L, 4, typeof(AnimationCurve));
				ParticleSystem.MinMaxCurve minMaxCurve3 = new ParticleSystem.MinMaxCurve(multiplier2, min, max);
				objectTranslator.Push(L, minMaxCurve3);
				return 1;
			}
			if (Lua.lua_gettop(L) == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float min2 = (float)Lua.lua_tonumber(L, 2);
				float max2 = (float)Lua.lua_tonumber(L, 3);
				ParticleSystem.MinMaxCurve minMaxCurve4 = new ParticleSystem.MinMaxCurve(min2, max2);
				objectTranslator.Push(L, minMaxCurve4);
				return 1;
			}
			if (Lua.lua_gettop(L) == 1)
			{
				objectTranslator.Push(L, default(ParticleSystem.MinMaxCurve));
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.MinMaxCurve constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Evaluate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxCurve v);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				float time = (float)Lua.lua_tonumber(L, 2);
				float num2 = v.Evaluate(time);
				Lua.lua_pushnumber(L, num2);
				objectTranslator.Update(L, 1, v);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float time2 = (float)Lua.lua_tonumber(L, 2);
				float lerpFactor = (float)Lua.lua_tonumber(L, 3);
				float num3 = v.Evaluate(time2, lerpFactor);
				Lua.lua_pushnumber(L, num3);
				objectTranslator.Update(L, 1, v);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.MinMaxCurve.Evaluate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxCurve v);
			objectTranslator.Push(L, v.mode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_curveMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.MinMaxCurve v);
			Lua.lua_pushnumber(L, v.curveMultiplier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_curveMax(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxCurve v);
			objectTranslator.Push(L, v.curveMax);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_curveMin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxCurve v);
			objectTranslator.Push(L, v.curveMin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_constantMax(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.MinMaxCurve v);
			Lua.lua_pushnumber(L, v.constantMax);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_constantMin(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.MinMaxCurve v);
			Lua.lua_pushnumber(L, v.constantMin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_constant(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.MinMaxCurve v);
			Lua.lua_pushnumber(L, v.constant);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_curve(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxCurve v);
			objectTranslator.Push(L, v.curve);
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
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxCurve v);
			objectTranslator.Get(L, 2, out ParticleSystemCurveMode v2);
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
	private static int _s_set_curveMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxCurve v);
			v.curveMultiplier = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_curveMax(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxCurve v);
			v.curveMax = (AnimationCurve)objectTranslator.GetObject(L, 2, typeof(AnimationCurve));
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_curveMin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxCurve v);
			v.curveMin = (AnimationCurve)objectTranslator.GetObject(L, 2, typeof(AnimationCurve));
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_constantMax(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxCurve v);
			v.constantMax = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_constantMin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxCurve v);
			v.constantMin = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_constant(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxCurve v);
			v.constant = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_curve(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.MinMaxCurve v);
			v.curve = (AnimationCurve)objectTranslator.GetObject(L, 2, typeof(AnimationCurve));
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
