using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class AutoDestroyWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(AutoDestroy);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 4, 4);
		Utils.RegisterFunc(L, -2, "time", _g_get_time);
		Utils.RegisterFunc(L, -2, "handle", _g_get_handle);
		Utils.RegisterFunc(L, -2, "realDestroy", _g_get_realDestroy);
		Utils.RegisterFunc(L, -2, "removeSelf", _g_get_removeSelf);
		Utils.RegisterFunc(L, -1, "time", _s_set_time);
		Utils.RegisterFunc(L, -1, "handle", _s_set_handle);
		Utils.RegisterFunc(L, -1, "realDestroy", _s_set_realDestroy);
		Utils.RegisterFunc(L, -1, "removeSelf", _s_set_removeSelf);
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
				AutoDestroy o = new AutoDestroy();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to AutoDestroy constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_time(IntPtr L)
	{
		try
		{
			AutoDestroy autoDestroy = (AutoDestroy)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, autoDestroy.time);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_handle(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AutoDestroy autoDestroy = (AutoDestroy)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, autoDestroy.handle);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_realDestroy(IntPtr L)
	{
		try
		{
			AutoDestroy autoDestroy = (AutoDestroy)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, autoDestroy.realDestroy);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_removeSelf(IntPtr L)
	{
		try
		{
			AutoDestroy autoDestroy = (AutoDestroy)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, autoDestroy.removeSelf);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_time(IntPtr L)
	{
		try
		{
			((AutoDestroy)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).time = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_handle(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((AutoDestroy)objectTranslator.FastGetCSObj(L, 1)).handle = (InstanceRequest)objectTranslator.GetObject(L, 2, typeof(InstanceRequest));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_realDestroy(IntPtr L)
	{
		try
		{
			((AutoDestroy)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).realDestroy = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_removeSelf(IntPtr L)
	{
		try
		{
			((AutoDestroy)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).removeSelf = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
