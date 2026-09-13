using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class FOWRevealerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(FOWRevealer);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 3, 2, 2);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "OnDisable", _m_OnDisable);
		Utils.RegisterFunc(L, -3, "OnDestroy", _m_OnDestroy);
		Utils.RegisterFunc(L, -2, "lineOfSightCheck", _g_get_lineOfSightCheck);
		Utils.RegisterFunc(L, -2, "isActive", _g_get_isActive);
		Utils.RegisterFunc(L, -1, "lineOfSightCheck", _s_set_lineOfSightCheck);
		Utils.RegisterFunc(L, -1, "isActive", _s_set_isActive);
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
				FOWRevealer o = new FOWRevealer();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to FOWRevealer constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FOWRevealer fOWRevealer = (FOWRevealer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			objectTranslator.Get(L, 3, out Vector2 val2);
			fOWRevealer.Init(val, val2);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDisable(IntPtr L)
	{
		try
		{
			((FOWRevealer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnDisable();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDestroy(IntPtr L)
	{
		try
		{
			((FOWRevealer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnDestroy();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lineOfSightCheck(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FOWRevealer fOWRevealer = (FOWRevealer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushFOWSystemLOSChecks(L, fOWRevealer.lineOfSightCheck);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isActive(IntPtr L)
	{
		try
		{
			FOWRevealer fOWRevealer = (FOWRevealer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, fOWRevealer.isActive);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lineOfSightCheck(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FOWRevealer fOWRevealer = (FOWRevealer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out FOWSystem.LOSChecks val);
			fOWRevealer.lineOfSightCheck = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isActive(IntPtr L)
	{
		try
		{
			((FOWRevealer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isActive = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
