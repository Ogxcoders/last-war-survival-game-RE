using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineBoundsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Bounds);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 1, 11, 5, 5);
		Utils.RegisterFunc(L, -4, "__eq", __EqMeta);
		Utils.RegisterFunc(L, -3, "GetHashCode", _m_GetHashCode);
		Utils.RegisterFunc(L, -3, "Equals", _m_Equals);
		Utils.RegisterFunc(L, -3, "SetMinMax", _m_SetMinMax);
		Utils.RegisterFunc(L, -3, "Encapsulate", _m_Encapsulate);
		Utils.RegisterFunc(L, -3, "Expand", _m_Expand);
		Utils.RegisterFunc(L, -3, "Intersects", _m_Intersects);
		Utils.RegisterFunc(L, -3, "IntersectRay", _m_IntersectRay);
		Utils.RegisterFunc(L, -3, "ToString", _m_ToString);
		Utils.RegisterFunc(L, -3, "Contains", _m_Contains);
		Utils.RegisterFunc(L, -3, "SqrDistance", _m_SqrDistance);
		Utils.RegisterFunc(L, -3, "ClosestPoint", _m_ClosestPoint);
		Utils.RegisterFunc(L, -2, "center", _g_get_center);
		Utils.RegisterFunc(L, -2, "size", _g_get_size);
		Utils.RegisterFunc(L, -2, "extents", _g_get_extents);
		Utils.RegisterFunc(L, -2, "min", _g_get_min);
		Utils.RegisterFunc(L, -2, "max", _g_get_max);
		Utils.RegisterFunc(L, -1, "center", _s_set_center);
		Utils.RegisterFunc(L, -1, "size", _s_set_size);
		Utils.RegisterFunc(L, -1, "extents", _s_set_extents);
		Utils.RegisterFunc(L, -1, "min", _s_set_min);
		Utils.RegisterFunc(L, -1, "max", _s_set_max);
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
			if (Lua.lua_gettop(L) == 3 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				objectTranslator.Get(L, 3, out Vector3 val2);
				Bounds val3 = new Bounds(val, val2);
				objectTranslator.PushUnityEngineBounds(L, val3);
				return 1;
			}
			if (Lua.lua_gettop(L) == 1)
			{
				objectTranslator.PushUnityEngineBounds(L, default(Bounds));
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Bounds constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __EqMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Bounds>(L, 1) && objectTranslator.Assignable<Bounds>(L, 2))
			{
				objectTranslator.Get(L, 1, out Bounds val);
				objectTranslator.Get(L, 2, out Bounds val2);
				Lua.lua_pushboolean(L, val == val2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of == operator, need UnityEngine.Bounds!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetHashCode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Bounds val);
			int hashCode = val.GetHashCode();
			Lua.xlua_pushinteger(L, hashCode);
			objectTranslator.UpdateUnityEngineBounds(L, 1, val);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Equals(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Bounds val);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<object>(L, 2))
			{
				object obj = objectTranslator.GetObject(L, 2, typeof(object));
				bool value = val.Equals(obj);
				Lua.lua_pushboolean(L, value);
				objectTranslator.UpdateUnityEngineBounds(L, 1, val);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Bounds>(L, 2))
			{
				objectTranslator.Get(L, 2, out Bounds val2);
				bool value2 = val.Equals(val2);
				Lua.lua_pushboolean(L, value2);
				objectTranslator.UpdateUnityEngineBounds(L, 1, val);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Bounds.Equals!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetMinMax(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Bounds val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			objectTranslator.Get(L, 3, out Vector3 val3);
			val.SetMinMax(val2, val3);
			objectTranslator.UpdateUnityEngineBounds(L, 1, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Encapsulate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Bounds val);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				val.Encapsulate(val2);
				objectTranslator.UpdateUnityEngineBounds(L, 1, val);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Bounds>(L, 2))
			{
				objectTranslator.Get(L, 2, out Bounds val3);
				val.Encapsulate(val3);
				objectTranslator.UpdateUnityEngineBounds(L, 1, val);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Bounds.Encapsulate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Expand(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Bounds val);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				float amount = (float)Lua.lua_tonumber(L, 2);
				val.Expand(amount);
				objectTranslator.UpdateUnityEngineBounds(L, 1, val);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				val.Expand(val2);
				objectTranslator.UpdateUnityEngineBounds(L, 1, val);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Bounds.Expand!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Intersects(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Bounds val);
			objectTranslator.Get(L, 2, out Bounds val2);
			bool value = val.Intersects(val2);
			Lua.lua_pushboolean(L, value);
			objectTranslator.UpdateUnityEngineBounds(L, 1, val);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IntersectRay(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Bounds val);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Ray>(L, 2))
			{
				objectTranslator.Get(L, 2, out Ray val2);
				bool value = val.IntersectRay(val2);
				Lua.lua_pushboolean(L, value);
				objectTranslator.UpdateUnityEngineBounds(L, 1, val);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Ray>(L, 2))
			{
				objectTranslator.Get(L, 2, out Ray val3);
				float distance;
				bool value2 = val.IntersectRay(val3, out distance);
				Lua.lua_pushboolean(L, value2);
				Lua.lua_pushnumber(L, distance);
				objectTranslator.UpdateUnityEngineBounds(L, 1, val);
				return 2;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Bounds.IntersectRay!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToString(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Bounds val);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				string str2 = val.ToString();
				Lua.lua_pushstring(L, str2);
				objectTranslator.UpdateUnityEngineBounds(L, 1, val);
				return 1;
			}
			case 2:
				if (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING)
				{
					string text = Lua.lua_tostring(L, 2);
					string str = val.ToString(text);
					Lua.lua_pushstring(L, str);
					objectTranslator.UpdateUnityEngineBounds(L, 1, val);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Bounds.ToString!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Contains(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Bounds val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			bool value = val.Contains(val2);
			Lua.lua_pushboolean(L, value);
			objectTranslator.UpdateUnityEngineBounds(L, 1, val);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SqrDistance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Bounds val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			float num = val.SqrDistance(val2);
			Lua.lua_pushnumber(L, num);
			objectTranslator.UpdateUnityEngineBounds(L, 1, val);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClosestPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Bounds val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			Vector3 val3 = val.ClosestPoint(val2);
			objectTranslator.PushUnityEngineVector3(L, val3);
			objectTranslator.UpdateUnityEngineBounds(L, 1, val);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_center(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Bounds val);
			objectTranslator.PushUnityEngineVector3(L, val.center);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_size(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Bounds val);
			objectTranslator.PushUnityEngineVector3(L, val.size);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_extents(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Bounds val);
			objectTranslator.PushUnityEngineVector3(L, val.extents);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_min(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Bounds val);
			objectTranslator.PushUnityEngineVector3(L, val.min);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_max(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Bounds val);
			objectTranslator.PushUnityEngineVector3(L, val.max);
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
			objectTranslator.Get(L, 1, out Bounds val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			val.center = val2;
			objectTranslator.UpdateUnityEngineBounds(L, 1, val);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_size(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Bounds val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			val.size = val2;
			objectTranslator.UpdateUnityEngineBounds(L, 1, val);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_extents(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Bounds val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			val.extents = val2;
			objectTranslator.UpdateUnityEngineBounds(L, 1, val);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_min(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Bounds val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			val.min = val2;
			objectTranslator.UpdateUnityEngineBounds(L, 1, val);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_max(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Bounds val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			val.max = val2;
			objectTranslator.UpdateUnityEngineBounds(L, 1, val);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
