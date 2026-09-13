using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class CitySpaceManTriggerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CitySpaceManTrigger);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 4, 4);
		Utils.RegisterFunc(L, -2, "ObjectId", _g_get_ObjectId);
		Utils.RegisterFunc(L, -2, "resType", _g_get_resType);
		Utils.RegisterFunc(L, -2, "TriggerEnterAction", _g_get_TriggerEnterAction);
		Utils.RegisterFunc(L, -2, "TriggerExitAction", _g_get_TriggerExitAction);
		Utils.RegisterFunc(L, -1, "ObjectId", _s_set_ObjectId);
		Utils.RegisterFunc(L, -1, "resType", _s_set_resType);
		Utils.RegisterFunc(L, -1, "TriggerEnterAction", _s_set_TriggerEnterAction);
		Utils.RegisterFunc(L, -1, "TriggerExitAction", _s_set_TriggerExitAction);
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
				CitySpaceManTrigger o = new CitySpaceManTrigger();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CitySpaceManTrigger constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ObjectId(IntPtr L)
	{
		try
		{
			CitySpaceManTrigger citySpaceManTrigger = (CitySpaceManTrigger)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, citySpaceManTrigger.ObjectId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_resType(IntPtr L)
	{
		try
		{
			CitySpaceManTrigger citySpaceManTrigger = (CitySpaceManTrigger)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, citySpaceManTrigger.resType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TriggerEnterAction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CitySpaceManTrigger citySpaceManTrigger = (CitySpaceManTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, citySpaceManTrigger.TriggerEnterAction);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TriggerExitAction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CitySpaceManTrigger citySpaceManTrigger = (CitySpaceManTrigger)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, citySpaceManTrigger.TriggerExitAction);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ObjectId(IntPtr L)
	{
		try
		{
			((CitySpaceManTrigger)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ObjectId = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_resType(IntPtr L)
	{
		try
		{
			((CitySpaceManTrigger)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).resType = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_TriggerEnterAction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((CitySpaceManTrigger)objectTranslator.FastGetCSObj(L, 1)).TriggerEnterAction = objectTranslator.GetDelegate<Action<long, int>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_TriggerExitAction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((CitySpaceManTrigger)objectTranslator.FastGetCSObj(L, 1)).TriggerExitAction = objectTranslator.GetDelegate<Action<long>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
