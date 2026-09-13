using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GMSwitchLoggerLogWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GMSwitch.Logger.Log);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 5, 5);
		Utils.RegisterFunc(L, -2, "condition", _g_get_condition);
		Utils.RegisterFunc(L, -2, "truncatedCondition", _g_get_truncatedCondition);
		Utils.RegisterFunc(L, -2, "stacktrace", _g_get_stacktrace);
		Utils.RegisterFunc(L, -2, "time", _g_get_time);
		Utils.RegisterFunc(L, -2, "logType", _g_get_logType);
		Utils.RegisterFunc(L, -1, "condition", _s_set_condition);
		Utils.RegisterFunc(L, -1, "truncatedCondition", _s_set_truncatedCondition);
		Utils.RegisterFunc(L, -1, "stacktrace", _s_set_stacktrace);
		Utils.RegisterFunc(L, -1, "time", _s_set_time);
		Utils.RegisterFunc(L, -1, "logType", _s_set_logType);
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
				GMSwitch.Logger.Log o = new GMSwitch.Logger.Log();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GMSwitch.Logger.Log constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_condition(IntPtr L)
	{
		try
		{
			GMSwitch.Logger.Log log = (GMSwitch.Logger.Log)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, log.condition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_truncatedCondition(IntPtr L)
	{
		try
		{
			GMSwitch.Logger.Log log = (GMSwitch.Logger.Log)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, log.truncatedCondition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_stacktrace(IntPtr L)
	{
		try
		{
			GMSwitch.Logger.Log log = (GMSwitch.Logger.Log)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, log.stacktrace);
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
			GMSwitch.Logger.Log log = (GMSwitch.Logger.Log)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, log.time);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_logType(IntPtr L)
	{
		try
		{
			GMSwitch.Logger.Log log = (GMSwitch.Logger.Log)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, log.logType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_condition(IntPtr L)
	{
		try
		{
			((GMSwitch.Logger.Log)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).condition = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_truncatedCondition(IntPtr L)
	{
		try
		{
			((GMSwitch.Logger.Log)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).truncatedCondition = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_stacktrace(IntPtr L)
	{
		try
		{
			((GMSwitch.Logger.Log)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).stacktrace = Lua.lua_tostring(L, 2);
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
			((GMSwitch.Logger.Log)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).time = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_logType(IntPtr L)
	{
		try
		{
			((GMSwitch.Logger.Log)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).logType = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
