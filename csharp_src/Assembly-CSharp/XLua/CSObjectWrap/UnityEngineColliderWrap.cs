using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineColliderWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Collider);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 3, 7, 5);
		Utils.RegisterFunc(L, -3, "ClosestPoint", _m_ClosestPoint);
		Utils.RegisterFunc(L, -3, "Raycast", _m_Raycast);
		Utils.RegisterFunc(L, -3, "ClosestPointOnBounds", _m_ClosestPointOnBounds);
		Utils.RegisterFunc(L, -2, "enabled", _g_get_enabled);
		Utils.RegisterFunc(L, -2, "attachedRigidbody", _g_get_attachedRigidbody);
		Utils.RegisterFunc(L, -2, "isTrigger", _g_get_isTrigger);
		Utils.RegisterFunc(L, -2, "contactOffset", _g_get_contactOffset);
		Utils.RegisterFunc(L, -2, "bounds", _g_get_bounds);
		Utils.RegisterFunc(L, -2, "sharedMaterial", _g_get_sharedMaterial);
		Utils.RegisterFunc(L, -2, "material", _g_get_material);
		Utils.RegisterFunc(L, -1, "enabled", _s_set_enabled);
		Utils.RegisterFunc(L, -1, "isTrigger", _s_set_isTrigger);
		Utils.RegisterFunc(L, -1, "contactOffset", _s_set_contactOffset);
		Utils.RegisterFunc(L, -1, "sharedMaterial", _s_set_sharedMaterial);
		Utils.RegisterFunc(L, -1, "material", _s_set_material);
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
				Collider o = new Collider();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Collider constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClosestPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Collider collider = (Collider)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Vector3 val2 = collider.ClosestPoint(val);
			objectTranslator.PushUnityEngineVector3(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Raycast(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Collider collider = (Collider)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Ray val);
			RaycastHit hitInfo;
			bool value = collider.Raycast(maxDistance: (float)Lua.lua_tonumber(L, 3), ray: val, hitInfo: out hitInfo);
			Lua.lua_pushboolean(L, value);
			objectTranslator.Push(L, hitInfo);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClosestPointOnBounds(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Collider collider = (Collider)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Vector3 val2 = collider.ClosestPointOnBounds(val);
			objectTranslator.PushUnityEngineVector3(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enabled(IntPtr L)
	{
		try
		{
			Collider collider = (Collider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, collider.enabled);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_attachedRigidbody(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Collider collider = (Collider)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, collider.attachedRigidbody);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isTrigger(IntPtr L)
	{
		try
		{
			Collider collider = (Collider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, collider.isTrigger);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_contactOffset(IntPtr L)
	{
		try
		{
			Collider collider = (Collider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, collider.contactOffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_bounds(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Collider collider = (Collider)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineBounds(L, collider.bounds);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sharedMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Collider collider = (Collider)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, collider.sharedMaterial);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_material(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Collider collider = (Collider)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, collider.material);
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
			((Collider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).enabled = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isTrigger(IntPtr L)
	{
		try
		{
			((Collider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isTrigger = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_contactOffset(IntPtr L)
	{
		try
		{
			((Collider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).contactOffset = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sharedMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Collider)objectTranslator.FastGetCSObj(L, 1)).sharedMaterial = (PhysicMaterial)objectTranslator.GetObject(L, 2, typeof(PhysicMaterial));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_material(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Collider)objectTranslator.FastGetCSObj(L, 1)).material = (PhysicMaterial)objectTranslator.GetObject(L, 2, typeof(PhysicMaterial));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
