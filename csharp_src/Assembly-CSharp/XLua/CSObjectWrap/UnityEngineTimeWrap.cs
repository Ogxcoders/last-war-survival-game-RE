using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineTimeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Time);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 19, 6);
		Utils.RegisterFunc(L, -2, "time", _g_get_time);
		Utils.RegisterFunc(L, -2, "timeSinceLevelLoad", _g_get_timeSinceLevelLoad);
		Utils.RegisterFunc(L, -2, "deltaTime", _g_get_deltaTime);
		Utils.RegisterFunc(L, -2, "fixedTime", _g_get_fixedTime);
		Utils.RegisterFunc(L, -2, "unscaledTime", _g_get_unscaledTime);
		Utils.RegisterFunc(L, -2, "fixedUnscaledTime", _g_get_fixedUnscaledTime);
		Utils.RegisterFunc(L, -2, "unscaledDeltaTime", _g_get_unscaledDeltaTime);
		Utils.RegisterFunc(L, -2, "fixedUnscaledDeltaTime", _g_get_fixedUnscaledDeltaTime);
		Utils.RegisterFunc(L, -2, "fixedDeltaTime", _g_get_fixedDeltaTime);
		Utils.RegisterFunc(L, -2, "maximumDeltaTime", _g_get_maximumDeltaTime);
		Utils.RegisterFunc(L, -2, "smoothDeltaTime", _g_get_smoothDeltaTime);
		Utils.RegisterFunc(L, -2, "maximumParticleDeltaTime", _g_get_maximumParticleDeltaTime);
		Utils.RegisterFunc(L, -2, "timeScale", _g_get_timeScale);
		Utils.RegisterFunc(L, -2, "frameCount", _g_get_frameCount);
		Utils.RegisterFunc(L, -2, "renderedFrameCount", _g_get_renderedFrameCount);
		Utils.RegisterFunc(L, -2, "realtimeSinceStartup", _g_get_realtimeSinceStartup);
		Utils.RegisterFunc(L, -2, "captureDeltaTime", _g_get_captureDeltaTime);
		Utils.RegisterFunc(L, -2, "captureFramerate", _g_get_captureFramerate);
		Utils.RegisterFunc(L, -2, "inFixedTimeStep", _g_get_inFixedTimeStep);
		Utils.RegisterFunc(L, -1, "fixedDeltaTime", _s_set_fixedDeltaTime);
		Utils.RegisterFunc(L, -1, "maximumDeltaTime", _s_set_maximumDeltaTime);
		Utils.RegisterFunc(L, -1, "maximumParticleDeltaTime", _s_set_maximumParticleDeltaTime);
		Utils.RegisterFunc(L, -1, "timeScale", _s_set_timeScale);
		Utils.RegisterFunc(L, -1, "captureDeltaTime", _s_set_captureDeltaTime);
		Utils.RegisterFunc(L, -1, "captureFramerate", _s_set_captureFramerate);
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
				Time o = new Time();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Time constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_time(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, Time.time);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_timeSinceLevelLoad(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, Time.timeSinceLevelLoad);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_deltaTime(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, Time.deltaTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fixedTime(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, Time.fixedTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_unscaledTime(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, Time.unscaledTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fixedUnscaledTime(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, Time.fixedUnscaledTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_unscaledDeltaTime(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, Time.unscaledDeltaTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fixedUnscaledDeltaTime(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, Time.fixedUnscaledDeltaTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fixedDeltaTime(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, Time.fixedDeltaTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maximumDeltaTime(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, Time.maximumDeltaTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_smoothDeltaTime(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, Time.smoothDeltaTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maximumParticleDeltaTime(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, Time.maximumParticleDeltaTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_timeScale(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, Time.timeScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_frameCount(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, Time.frameCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_renderedFrameCount(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, Time.renderedFrameCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_realtimeSinceStartup(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, Time.realtimeSinceStartup);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_captureDeltaTime(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, Time.captureDeltaTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_captureFramerate(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, Time.captureFramerate);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_inFixedTimeStep(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Time.inFixedTimeStep);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fixedDeltaTime(IntPtr L)
	{
		try
		{
			Time.fixedDeltaTime = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maximumDeltaTime(IntPtr L)
	{
		try
		{
			Time.maximumDeltaTime = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maximumParticleDeltaTime(IntPtr L)
	{
		try
		{
			Time.maximumParticleDeltaTime = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_timeScale(IntPtr L)
	{
		try
		{
			Time.timeScale = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_captureDeltaTime(IntPtr L)
	{
		try
		{
			Time.captureDeltaTime = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_captureFramerate(IntPtr L)
	{
		try
		{
			Time.captureFramerate = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
