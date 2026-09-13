using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ApplicationLaunchWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ApplicationLaunch);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 9, 5, 0);
		Utils.RegisterFunc(L, -3, "TaskDone", _m_TaskDone);
		Utils.RegisterFunc(L, -3, "TaskSucceed", _m_TaskSucceed);
		Utils.RegisterFunc(L, -3, "ShutdownCheckResVersion", _m_ShutdownCheckResVersion);
		Utils.RegisterFunc(L, -3, "Quit", _m_Quit);
		Utils.RegisterFunc(L, -3, "ReloadGameInOtherProcess", _m_ReloadGameInOtherProcess);
		Utils.RegisterFunc(L, -3, "ReloadGame", _m_ReloadGame);
		Utils.RegisterFunc(L, -3, "ReloadGameCheckResVerion", _m_ReloadGameCheckResVerion);
		Utils.RegisterFunc(L, -3, "ReStartGame", _m_ReStartGame);
		Utils.RegisterFunc(L, -3, "DisconnectRetry", _m_DisconnectRetry);
		Utils.RegisterFunc(L, -2, "taskAllDone", _g_get_taskAllDone);
		Utils.RegisterFunc(L, -2, "taskAllSucceed", _g_get_taskAllSucceed);
		Utils.RegisterFunc(L, -2, "checkResVersionParallel", _g_get_checkResVersionParallel);
		Utils.RegisterFunc(L, -2, "isReloading", _g_get_isReloading);
		Utils.RegisterFunc(L, -2, "Loading", _g_get_Loading);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 9, 4, 2);
		Utils.RegisterFunc(L, -4, "EnqueueTask", _m_EnqueueTask_xlua_st_);
		Utils.RegisterFunc(L, -4, "ResetStep", _m_ResetStep_xlua_st_);
		Utils.RegisterFunc(L, -4, "StepLog", _m_StepLog_xlua_st_);
		Utils.RegisterFunc(L, -4, "UpdateTrackingResVersion", _m_UpdateTrackingResVersion_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "FLAG_DATATABLE", 1);
		Utils.RegisterObject(L, translator, -4, "FLAG_LOCALE", 2);
		Utils.RegisterObject(L, translator, -4, "FLAG_LWSCRIPT", 4);
		Utils.RegisterObject(L, translator, -4, "FLAG_ALLSET", 7);
		Utils.RegisterFunc(L, -2, "ParallelInitLockFile", _g_get_ParallelInitLockFile);
		Utils.RegisterFunc(L, -2, "Instance", _g_get_Instance);
		Utils.RegisterFunc(L, -2, "kParallelInit", _g_get_kParallelInit);
		Utils.RegisterFunc(L, -2, "awakeTime", _g_get_awakeTime);
		Utils.RegisterFunc(L, -1, "kParallelInit", _s_set_kParallelInit);
		Utils.RegisterFunc(L, -1, "awakeTime", _s_set_awakeTime);
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
				ApplicationLaunch o = new ApplicationLaunch();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ApplicationLaunch constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TaskDone(IntPtr L)
	{
		try
		{
			ApplicationLaunch obj = (ApplicationLaunch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int flag = Lua.xlua_tointeger(L, 2);
			bool value = obj.TaskDone(flag);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TaskSucceed(IntPtr L)
	{
		try
		{
			ApplicationLaunch obj = (ApplicationLaunch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int flag = Lua.xlua_tointeger(L, 2);
			bool value = obj.TaskSucceed(flag);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnqueueTask_xlua_st_(IntPtr L)
	{
		try
		{
			ApplicationLaunch.EnqueueTask((LaunchTask)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(LaunchTask)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShutdownCheckResVersion(IntPtr L)
	{
		try
		{
			((ApplicationLaunch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ShutdownCheckResVersion();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetStep_xlua_st_(IntPtr L)
	{
		try
		{
			ApplicationLaunch.ResetStep();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StepLog_xlua_st_(IntPtr L)
	{
		try
		{
			ApplicationLaunch.StepLog(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Quit(IntPtr L)
	{
		try
		{
			((ApplicationLaunch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Quit();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReloadGameInOtherProcess(IntPtr L)
	{
		try
		{
			((ApplicationLaunch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ReloadGameInOtherProcess();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReloadGame(IntPtr L)
	{
		try
		{
			((ApplicationLaunch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ReloadGame();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReloadGameCheckResVerion(IntPtr L)
	{
		try
		{
			((ApplicationLaunch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ReloadGameCheckResVerion();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReStartGame(IntPtr L)
	{
		try
		{
			((ApplicationLaunch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ReStartGame();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DisconnectRetry(IntPtr L)
	{
		try
		{
			((ApplicationLaunch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DisconnectRetry();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateTrackingResVersion_xlua_st_(IntPtr L)
	{
		try
		{
			ApplicationLaunch.UpdateTrackingResVersion();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ParallelInitLockFile(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, ApplicationLaunch.ParallelInitLockFile);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_taskAllDone(IntPtr L)
	{
		try
		{
			ApplicationLaunch applicationLaunch = (ApplicationLaunch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, applicationLaunch.taskAllDone);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_taskAllSucceed(IntPtr L)
	{
		try
		{
			ApplicationLaunch applicationLaunch = (ApplicationLaunch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, applicationLaunch.taskAllSucceed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_checkResVersionParallel(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ApplicationLaunch applicationLaunch = (ApplicationLaunch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, applicationLaunch.checkResVersionParallel);
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
			ObjectTranslatorPool.Instance.Find(L).Push(L, ApplicationLaunch.Instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isReloading(IntPtr L)
	{
		try
		{
			ApplicationLaunch applicationLaunch = (ApplicationLaunch)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, applicationLaunch.isReloading);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Loading(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ApplicationLaunch applicationLaunch = (ApplicationLaunch)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, applicationLaunch.Loading);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_kParallelInit(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, ApplicationLaunch.kParallelInit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_awakeTime(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, ApplicationLaunch.awakeTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_kParallelInit(IntPtr L)
	{
		try
		{
			ApplicationLaunch.kParallelInit = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_awakeTime(IntPtr L)
	{
		try
		{
			ApplicationLaunch.awakeTime = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
