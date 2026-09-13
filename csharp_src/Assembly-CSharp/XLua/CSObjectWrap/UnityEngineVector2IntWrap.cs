using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineVector2IntWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Vector2Int);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 6, 6, 4, 2);
		Utils.RegisterFunc(L, -4, "__unm", __UnmMeta);
		Utils.RegisterFunc(L, -4, "__add", __AddMeta);
		Utils.RegisterFunc(L, -4, "__sub", __SubMeta);
		Utils.RegisterFunc(L, -4, "__mul", __MulMeta);
		Utils.RegisterFunc(L, -4, "__div", __DivMeta);
		Utils.RegisterFunc(L, -4, "__eq", __EqMeta);
		Utils.RegisterFunc(L, -3, "Set", _m_Set);
		Utils.RegisterFunc(L, -3, "Scale", _m_Scale);
		Utils.RegisterFunc(L, -3, "Clamp", _m_Clamp);
		Utils.RegisterFunc(L, -3, "Equals", _m_Equals);
		Utils.RegisterFunc(L, -3, "GetHashCode", _m_GetHashCode);
		Utils.RegisterFunc(L, -3, "ToString", _m_ToString);
		Utils.RegisterFunc(L, -2, "x", _g_get_x);
		Utils.RegisterFunc(L, -2, "y", _g_get_y);
		Utils.RegisterFunc(L, -2, "magnitude", _g_get_magnitude);
		Utils.RegisterFunc(L, -2, "sqrMagnitude", _g_get_sqrMagnitude);
		Utils.RegisterFunc(L, -1, "x", _s_set_x);
		Utils.RegisterFunc(L, -1, "y", _s_set_y);
		Utils.EndObjectRegister(typeFromHandle, L, translator, __CSIndexer, __NewIndexer, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 8, 6, 0);
		Utils.RegisterFunc(L, -4, "Distance", _m_Distance_xlua_st_);
		Utils.RegisterFunc(L, -4, "Min", _m_Min_xlua_st_);
		Utils.RegisterFunc(L, -4, "Max", _m_Max_xlua_st_);
		Utils.RegisterFunc(L, -4, "Scale", _m_Scale_xlua_st_);
		Utils.RegisterFunc(L, -4, "FloorToInt", _m_FloorToInt_xlua_st_);
		Utils.RegisterFunc(L, -4, "CeilToInt", _m_CeilToInt_xlua_st_);
		Utils.RegisterFunc(L, -4, "RoundToInt", _m_RoundToInt_xlua_st_);
		Utils.RegisterFunc(L, -2, "zero", _g_get_zero);
		Utils.RegisterFunc(L, -2, "one", _g_get_one);
		Utils.RegisterFunc(L, -2, "up", _g_get_up);
		Utils.RegisterFunc(L, -2, "down", _g_get_down);
		Utils.RegisterFunc(L, -2, "left", _g_get_left);
		Utils.RegisterFunc(L, -2, "right", _g_get_right);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int x = Lua.xlua_tointeger(L, 2);
				int y = Lua.xlua_tointeger(L, 3);
				Vector2Int vector2Int = new Vector2Int(x, y);
				objectTranslator.Push(L, vector2Int);
				return 1;
			}
			if (Lua.lua_gettop(L) == 1)
			{
				objectTranslator.Push(L, default(Vector2Int));
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Vector2Int constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int __CSIndexer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Vector2Int>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector2Int v);
				int index = Lua.xlua_tointeger(L, 2);
				Lua.lua_pushboolean(L, value: true);
				Lua.xlua_pushinteger(L, v[index]);
				return 2;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.lua_pushboolean(L, value: false);
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int __NewIndexer(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		try
		{
			if (objectTranslator.Assignable<Vector2Int>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector2Int v);
				int index = Lua.xlua_tointeger(L, 2);
				v[index] = Lua.xlua_tointeger(L, 3);
				Lua.lua_pushboolean(L, value: true);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.lua_pushboolean(L, value: false);
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __UnmMeta(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		try
		{
			objectTranslator.Get(L, 1, out Vector2Int v);
			objectTranslator.Push(L, -v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __AddMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Vector2Int>(L, 1) && objectTranslator.Assignable<Vector2Int>(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector2Int v);
				objectTranslator.Get(L, 2, out Vector2Int v2);
				objectTranslator.Push(L, v + v2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of + operator, need UnityEngine.Vector2Int!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __SubMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Vector2Int>(L, 1) && objectTranslator.Assignable<Vector2Int>(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector2Int v);
				objectTranslator.Get(L, 2, out Vector2Int v2);
				objectTranslator.Push(L, v - v2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of - operator, need UnityEngine.Vector2Int!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __MulMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Vector2Int>(L, 1) && objectTranslator.Assignable<Vector2Int>(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector2Int v);
				objectTranslator.Get(L, 2, out Vector2Int v2);
				objectTranslator.Push(L, v * v2);
				return 1;
			}
			if (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<Vector2Int>(L, 2))
			{
				int num = Lua.xlua_tointeger(L, 1);
				objectTranslator.Get(L, 2, out Vector2Int v3);
				objectTranslator.Push(L, num * v3);
				return 1;
			}
			if (objectTranslator.Assignable<Vector2Int>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector2Int v4);
				int num2 = Lua.xlua_tointeger(L, 2);
				objectTranslator.Push(L, v4 * num2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of * operator, need UnityEngine.Vector2Int!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __DivMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Vector2Int>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector2Int v);
				int num = Lua.xlua_tointeger(L, 2);
				objectTranslator.Push(L, v / num);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of / operator, need UnityEngine.Vector2Int!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __EqMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Vector2Int>(L, 1) && objectTranslator.Assignable<Vector2Int>(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector2Int v);
				objectTranslator.Get(L, 2, out Vector2Int v2);
				Lua.lua_pushboolean(L, v == v2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of == operator, need UnityEngine.Vector2Int!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector2Int v);
			int x = Lua.xlua_tointeger(L, 2);
			int y = Lua.xlua_tointeger(L, 3);
			v.Set(x, y);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Distance_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector2Int v);
			objectTranslator.Get(L, 2, out Vector2Int v2);
			float num = Vector2Int.Distance(v, v2);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Min_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector2Int v);
			objectTranslator.Get(L, 2, out Vector2Int v2);
			Vector2Int vector2Int = Vector2Int.Min(v, v2);
			objectTranslator.Push(L, vector2Int);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Max_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector2Int v);
			objectTranslator.Get(L, 2, out Vector2Int v2);
			Vector2Int vector2Int = Vector2Int.Max(v, v2);
			objectTranslator.Push(L, vector2Int);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Scale_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector2Int v);
			objectTranslator.Get(L, 2, out Vector2Int v2);
			Vector2Int vector2Int = Vector2Int.Scale(v, v2);
			objectTranslator.Push(L, vector2Int);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Scale(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector2Int v);
			objectTranslator.Get(L, 2, out Vector2Int v2);
			v.Scale(v2);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clamp(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector2Int v);
			objectTranslator.Get(L, 2, out Vector2Int v2);
			objectTranslator.Get(L, 3, out Vector2Int v3);
			v.Clamp(v2, v3);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FloorToInt_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector2 val);
			Vector2Int vector2Int = Vector2Int.FloorToInt(val);
			objectTranslator.Push(L, vector2Int);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CeilToInt_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector2 val);
			Vector2Int vector2Int = Vector2Int.CeilToInt(val);
			objectTranslator.Push(L, vector2Int);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RoundToInt_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector2 val);
			Vector2Int vector2Int = Vector2Int.RoundToInt(val);
			objectTranslator.Push(L, vector2Int);
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
			objectTranslator.Get(L, 1, out Vector2Int v);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<object>(L, 2))
			{
				object obj = objectTranslator.GetObject(L, 2, typeof(object));
				bool value = v.Equals(obj);
				Lua.lua_pushboolean(L, value);
				objectTranslator.Update(L, 1, v);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Vector2Int>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector2Int v2);
				bool value2 = v.Equals(v2);
				Lua.lua_pushboolean(L, value2);
				objectTranslator.Update(L, 1, v);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Vector2Int.Equals!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetHashCode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector2Int v);
			int hashCode = v.GetHashCode();
			Lua.xlua_pushinteger(L, hashCode);
			objectTranslator.Update(L, 1, v);
			return 1;
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
			objectTranslator.Get(L, 1, out Vector2Int v);
			string str = v.ToString();
			Lua.lua_pushstring(L, str);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_x(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector2Int v);
			Lua.xlua_pushinteger(L, v.x);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_y(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector2Int v);
			Lua.xlua_pushinteger(L, v.y);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_magnitude(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector2Int v);
			Lua.lua_pushnumber(L, v.magnitude);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sqrMagnitude(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector2Int v);
			Lua.xlua_pushinteger(L, v.sqrMagnitude);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_zero(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Vector2Int.zero);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_one(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Vector2Int.one);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_up(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Vector2Int.up);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_down(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Vector2Int.down);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_left(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Vector2Int.left);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_right(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Vector2Int.right);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_x(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector2Int v);
			v.x = Lua.xlua_tointeger(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_y(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector2Int v);
			v.y = Lua.xlua_tointeger(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
