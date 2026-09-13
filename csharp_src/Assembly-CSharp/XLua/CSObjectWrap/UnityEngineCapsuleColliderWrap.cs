using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineCapsuleColliderWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CapsuleCollider);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 4, 4);
		Utils.RegisterFunc(L, -2, "center", _g_get_center);
		Utils.RegisterFunc(L, -2, "radius", _g_get_radius);
		Utils.RegisterFunc(L, -2, "height", _g_get_height);
		Utils.RegisterFunc(L, -2, "direction", _g_get_direction);
		Utils.RegisterFunc(L, -1, "center", _s_set_center);
		Utils.RegisterFunc(L, -1, "radius", _s_set_radius);
		Utils.RegisterFunc(L, -1, "height", _s_set_height);
		Utils.RegisterFunc(L, -1, "direction", _s_set_direction);
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
				CapsuleCollider o = new CapsuleCollider();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.CapsuleCollider constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_center(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CapsuleCollider capsuleCollider = (CapsuleCollider)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, capsuleCollider.center);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_radius(IntPtr L)
	{
		try
		{
			CapsuleCollider capsuleCollider = (CapsuleCollider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, capsuleCollider.radius);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_height(IntPtr L)
	{
		try
		{
			CapsuleCollider capsuleCollider = (CapsuleCollider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, capsuleCollider.height);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_direction(IntPtr L)
	{
		try
		{
			CapsuleCollider capsuleCollider = (CapsuleCollider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, capsuleCollider.direction);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_center(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CapsuleCollider capsuleCollider = (CapsuleCollider)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			capsuleCollider.center = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_radius(IntPtr L)
	{
		try
		{
			((CapsuleCollider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).radius = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_height(IntPtr L)
	{
		try
		{
			((CapsuleCollider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).height = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_direction(IntPtr L)
	{
		try
		{
			((CapsuleCollider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).direction = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
