using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class FOWSystemRevealerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(FOWSystem.Revealer);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 9, 9);
		Utils.RegisterFunc(L, -2, "isActive", _g_get_isActive);
		Utils.RegisterFunc(L, -2, "los", _g_get_los);
		Utils.RegisterFunc(L, -2, "pos", _g_get_pos);
		Utils.RegisterFunc(L, -2, "inner", _g_get_inner);
		Utils.RegisterFunc(L, -2, "outer", _g_get_outer);
		Utils.RegisterFunc(L, -2, "cachedBuffer", _g_get_cachedBuffer);
		Utils.RegisterFunc(L, -2, "cachedSize", _g_get_cachedSize);
		Utils.RegisterFunc(L, -2, "cachedX", _g_get_cachedX);
		Utils.RegisterFunc(L, -2, "cachedY", _g_get_cachedY);
		Utils.RegisterFunc(L, -1, "isActive", _s_set_isActive);
		Utils.RegisterFunc(L, -1, "los", _s_set_los);
		Utils.RegisterFunc(L, -1, "pos", _s_set_pos);
		Utils.RegisterFunc(L, -1, "inner", _s_set_inner);
		Utils.RegisterFunc(L, -1, "outer", _s_set_outer);
		Utils.RegisterFunc(L, -1, "cachedBuffer", _s_set_cachedBuffer);
		Utils.RegisterFunc(L, -1, "cachedSize", _s_set_cachedSize);
		Utils.RegisterFunc(L, -1, "cachedX", _s_set_cachedX);
		Utils.RegisterFunc(L, -1, "cachedY", _s_set_cachedY);
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
				FOWSystem.Revealer o = new FOWSystem.Revealer();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to FOWSystem.Revealer constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isActive(IntPtr L)
	{
		try
		{
			FOWSystem.Revealer revealer = (FOWSystem.Revealer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, revealer.isActive);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_los(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FOWSystem.Revealer revealer = (FOWSystem.Revealer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushFOWSystemLOSChecks(L, revealer.los);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FOWSystem.Revealer revealer = (FOWSystem.Revealer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, revealer.pos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_inner(IntPtr L)
	{
		try
		{
			FOWSystem.Revealer revealer = (FOWSystem.Revealer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, revealer.inner);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_outer(IntPtr L)
	{
		try
		{
			FOWSystem.Revealer revealer = (FOWSystem.Revealer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, revealer.outer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cachedBuffer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FOWSystem.Revealer revealer = (FOWSystem.Revealer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, revealer.cachedBuffer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cachedSize(IntPtr L)
	{
		try
		{
			FOWSystem.Revealer revealer = (FOWSystem.Revealer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, revealer.cachedSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cachedX(IntPtr L)
	{
		try
		{
			FOWSystem.Revealer revealer = (FOWSystem.Revealer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, revealer.cachedX);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cachedY(IntPtr L)
	{
		try
		{
			FOWSystem.Revealer revealer = (FOWSystem.Revealer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, revealer.cachedY);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isActive(IntPtr L)
	{
		try
		{
			((FOWSystem.Revealer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isActive = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_los(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FOWSystem.Revealer revealer = (FOWSystem.Revealer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out FOWSystem.LOSChecks val);
			revealer.los = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FOWSystem.Revealer revealer = (FOWSystem.Revealer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			revealer.pos = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_inner(IntPtr L)
	{
		try
		{
			((FOWSystem.Revealer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).inner = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_outer(IntPtr L)
	{
		try
		{
			((FOWSystem.Revealer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).outer = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cachedBuffer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((FOWSystem.Revealer)objectTranslator.FastGetCSObj(L, 1)).cachedBuffer = (bool[])objectTranslator.GetObject(L, 2, typeof(bool[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cachedSize(IntPtr L)
	{
		try
		{
			((FOWSystem.Revealer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).cachedSize = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cachedX(IntPtr L)
	{
		try
		{
			((FOWSystem.Revealer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).cachedX = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cachedY(IntPtr L)
	{
		try
		{
			((FOWSystem.Revealer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).cachedY = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
