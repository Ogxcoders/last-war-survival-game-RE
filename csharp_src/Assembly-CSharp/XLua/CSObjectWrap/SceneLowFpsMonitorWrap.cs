using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SceneLowFpsMonitorWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SceneLowFpsMonitor);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 1, 4, 0);
		Utils.RegisterFunc(L, -3, "StartStats", _m_StartStats);
		Utils.RegisterFunc(L, -2, "CurrentFPS", _g_get_CurrentFPS);
		Utils.RegisterFunc(L, -2, "AvgFps", _g_get_AvgFps);
		Utils.RegisterFunc(L, -2, "NewAvgFps", _g_get_NewAvgFps);
		Utils.RegisterFunc(L, -2, "TargetFrameRate", _g_get_TargetFrameRate);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 1, 0);
		Utils.RegisterFunc(L, -2, "Instance", _g_get_Instance);
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
				SceneLowFpsMonitor o = new SceneLowFpsMonitor();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SceneLowFpsMonitor constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StartStats(IntPtr L)
	{
		try
		{
			((SceneLowFpsMonitor)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StartStats();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurrentFPS(IntPtr L)
	{
		try
		{
			SceneLowFpsMonitor sceneLowFpsMonitor = (SceneLowFpsMonitor)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, sceneLowFpsMonitor.CurrentFPS);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AvgFps(IntPtr L)
	{
		try
		{
			SceneLowFpsMonitor sceneLowFpsMonitor = (SceneLowFpsMonitor)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, sceneLowFpsMonitor.AvgFps);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Instance(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, SceneLowFpsMonitor.Instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_NewAvgFps(IntPtr L)
	{
		try
		{
			SceneLowFpsMonitor sceneLowFpsMonitor = (SceneLowFpsMonitor)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, sceneLowFpsMonitor.NewAvgFps);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TargetFrameRate(IntPtr L)
	{
		try
		{
			SceneLowFpsMonitor sceneLowFpsMonitor = (SceneLowFpsMonitor)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, sceneLowFpsMonitor.TargetFrameRate);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
