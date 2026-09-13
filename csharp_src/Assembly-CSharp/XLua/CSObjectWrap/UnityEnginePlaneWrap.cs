using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEnginePlaneWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Plane);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 10, 3, 2);
		Utils.RegisterFunc(L, -3, "SetNormalAndPosition", _m_SetNormalAndPosition);
		Utils.RegisterFunc(L, -3, "Set3Points", _m_Set3Points);
		Utils.RegisterFunc(L, -3, "Flip", _m_Flip);
		Utils.RegisterFunc(L, -3, "Translate", _m_Translate);
		Utils.RegisterFunc(L, -3, "ClosestPointOnPlane", _m_ClosestPointOnPlane);
		Utils.RegisterFunc(L, -3, "GetDistanceToPoint", _m_GetDistanceToPoint);
		Utils.RegisterFunc(L, -3, "GetSide", _m_GetSide);
		Utils.RegisterFunc(L, -3, "SameSide", _m_SameSide);
		Utils.RegisterFunc(L, -3, "Raycast", _m_Raycast);
		Utils.RegisterFunc(L, -3, "ToString", _m_ToString);
		Utils.RegisterFunc(L, -2, "normal", _g_get_normal);
		Utils.RegisterFunc(L, -2, "distance", _g_get_distance);
		Utils.RegisterFunc(L, -2, "flipped", _g_get_flipped);
		Utils.RegisterFunc(L, -1, "normal", _s_set_normal);
		Utils.RegisterFunc(L, -1, "distance", _s_set_distance);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 0, 0);
		Utils.RegisterFunc(L, -4, "Translate", _m_Translate_xlua_st_);
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
				Plane plane = new Plane(val, val2);
				objectTranslator.Push(L, plane);
				return 1;
			}
			if (Lua.lua_gettop(L) == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				float d = (float)Lua.lua_tonumber(L, 3);
				Plane plane2 = new Plane(val3, d);
				objectTranslator.Push(L, plane2);
				return 1;
			}
			if (Lua.lua_gettop(L) == 4 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<Vector3>(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val4);
				objectTranslator.Get(L, 3, out Vector3 val5);
				objectTranslator.Get(L, 4, out Vector3 val6);
				Plane plane3 = new Plane(val4, val5, val6);
				objectTranslator.Push(L, plane3);
				return 1;
			}
			if (Lua.lua_gettop(L) == 1)
			{
				objectTranslator.Push(L, default(Plane));
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Plane constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetNormalAndPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Plane v);
			objectTranslator.Get(L, 2, out Vector3 val);
			objectTranslator.Get(L, 3, out Vector3 val2);
			v.SetNormalAndPosition(val, val2);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set3Points(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Plane v);
			objectTranslator.Get(L, 2, out Vector3 val);
			objectTranslator.Get(L, 3, out Vector3 val2);
			objectTranslator.Get(L, 4, out Vector3 val3);
			v.Set3Points(val, val2, val3);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Flip(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Plane v);
			v.Flip();
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Translate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Plane v);
			objectTranslator.Get(L, 2, out Vector3 val);
			v.Translate(val);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Translate_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Plane v);
			objectTranslator.Get(L, 2, out Vector3 val);
			Plane plane = Plane.Translate(v, val);
			objectTranslator.Push(L, plane);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClosestPointOnPlane(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Plane v);
			objectTranslator.Get(L, 2, out Vector3 val);
			Vector3 val2 = v.ClosestPointOnPlane(val);
			objectTranslator.PushUnityEngineVector3(L, val2);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDistanceToPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Plane v);
			objectTranslator.Get(L, 2, out Vector3 val);
			float distanceToPoint = v.GetDistanceToPoint(val);
			Lua.lua_pushnumber(L, distanceToPoint);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSide(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Plane v);
			objectTranslator.Get(L, 2, out Vector3 val);
			bool side = v.GetSide(val);
			Lua.lua_pushboolean(L, side);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SameSide(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Plane v);
			objectTranslator.Get(L, 2, out Vector3 val);
			objectTranslator.Get(L, 3, out Vector3 val2);
			bool value = v.SameSide(val, val2);
			Lua.lua_pushboolean(L, value);
			objectTranslator.Update(L, 1, v);
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
			objectTranslator.Get(L, 1, out Plane v);
			objectTranslator.Get(L, 2, out Ray val);
			float enter;
			bool value = v.Raycast(val, out enter);
			Lua.lua_pushboolean(L, value);
			Lua.lua_pushnumber(L, enter);
			objectTranslator.Update(L, 1, v);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToString(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Plane v);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				string str2 = v.ToString();
				Lua.lua_pushstring(L, str2);
				objectTranslator.Update(L, 1, v);
				return 1;
			}
			case 2:
				if (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING)
				{
					string text = Lua.lua_tostring(L, 2);
					string str = v.ToString(text);
					Lua.lua_pushstring(L, str);
					objectTranslator.Update(L, 1, v);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Plane.ToString!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_normal(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Plane v);
			objectTranslator.PushUnityEngineVector3(L, v.normal);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_distance(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Plane v);
			Lua.lua_pushnumber(L, v.distance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_flipped(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Plane v);
			objectTranslator.Push(L, v.flipped);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_normal(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Plane v);
			objectTranslator.Get(L, 2, out Vector3 val);
			v.normal = val;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_distance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Plane v);
			v.distance = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
