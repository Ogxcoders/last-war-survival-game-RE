using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SimpleAnimationStateWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SimpleAnimation.State);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 10, 7);
		Utils.RegisterFunc(L, -2, "enabled", _g_get_enabled);
		Utils.RegisterFunc(L, -2, "isValid", _g_get_isValid);
		Utils.RegisterFunc(L, -2, "time", _g_get_time);
		Utils.RegisterFunc(L, -2, "normalizedTime", _g_get_normalizedTime);
		Utils.RegisterFunc(L, -2, "speed", _g_get_speed);
		Utils.RegisterFunc(L, -2, "name", _g_get_name);
		Utils.RegisterFunc(L, -2, "weight", _g_get_weight);
		Utils.RegisterFunc(L, -2, "length", _g_get_length);
		Utils.RegisterFunc(L, -2, "clip", _g_get_clip);
		Utils.RegisterFunc(L, -2, "wrapMode", _g_get_wrapMode);
		Utils.RegisterFunc(L, -1, "enabled", _s_set_enabled);
		Utils.RegisterFunc(L, -1, "time", _s_set_time);
		Utils.RegisterFunc(L, -1, "normalizedTime", _s_set_normalizedTime);
		Utils.RegisterFunc(L, -1, "speed", _s_set_speed);
		Utils.RegisterFunc(L, -1, "name", _s_set_name);
		Utils.RegisterFunc(L, -1, "weight", _s_set_weight);
		Utils.RegisterFunc(L, -1, "wrapMode", _s_set_wrapMode);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "SimpleAnimation.State does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enabled(IntPtr L)
	{
		try
		{
			SimpleAnimation.State state = (SimpleAnimation.State)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, state.enabled);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isValid(IntPtr L)
	{
		try
		{
			SimpleAnimation.State state = (SimpleAnimation.State)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, state.isValid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_time(IntPtr L)
	{
		try
		{
			SimpleAnimation.State state = (SimpleAnimation.State)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, state.time);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_normalizedTime(IntPtr L)
	{
		try
		{
			SimpleAnimation.State state = (SimpleAnimation.State)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, state.normalizedTime);
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
			SimpleAnimation.State state = (SimpleAnimation.State)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, state.speed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_name(IntPtr L)
	{
		try
		{
			SimpleAnimation.State state = (SimpleAnimation.State)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, state.name);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_weight(IntPtr L)
	{
		try
		{
			SimpleAnimation.State state = (SimpleAnimation.State)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, state.weight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_length(IntPtr L)
	{
		try
		{
			SimpleAnimation.State state = (SimpleAnimation.State)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, state.length);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_clip(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SimpleAnimation.State state = (SimpleAnimation.State)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, state.clip);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_wrapMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SimpleAnimation.State state = (SimpleAnimation.State)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, state.wrapMode);
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
			((SimpleAnimation.State)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).enabled = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_time(IntPtr L)
	{
		try
		{
			((SimpleAnimation.State)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).time = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_normalizedTime(IntPtr L)
	{
		try
		{
			((SimpleAnimation.State)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).normalizedTime = (float)Lua.lua_tonumber(L, 2);
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
			((SimpleAnimation.State)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).speed = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_name(IntPtr L)
	{
		try
		{
			((SimpleAnimation.State)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).name = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_weight(IntPtr L)
	{
		try
		{
			((SimpleAnimation.State)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).weight = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_wrapMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SimpleAnimation.State state = (SimpleAnimation.State)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out WrapMode v);
			state.wrapMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
