using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineVector4Wrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Vector4);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 6, 7, 7, 4);
		Utils.RegisterFunc(L, -4, "__add", __AddMeta);
		Utils.RegisterFunc(L, -4, "__sub", __SubMeta);
		Utils.RegisterFunc(L, -4, "__unm", __UnmMeta);
		Utils.RegisterFunc(L, -4, "__mul", __MulMeta);
		Utils.RegisterFunc(L, -4, "__div", __DivMeta);
		Utils.RegisterFunc(L, -4, "__eq", __EqMeta);
		Utils.RegisterFunc(L, -3, "Set", _m_Set);
		Utils.RegisterFunc(L, -3, "Scale", _m_Scale);
		Utils.RegisterFunc(L, -3, "GetHashCode", _m_GetHashCode);
		Utils.RegisterFunc(L, -3, "Equals", _m_Equals);
		Utils.RegisterFunc(L, -3, "Normalize", _m_Normalize);
		Utils.RegisterFunc(L, -3, "ToString", _m_ToString);
		Utils.RegisterFunc(L, -3, "SqrMagnitude", _m_SqrMagnitude);
		Utils.RegisterFunc(L, -2, "normalized", _g_get_normalized);
		Utils.RegisterFunc(L, -2, "magnitude", _g_get_magnitude);
		Utils.RegisterFunc(L, -2, "sqrMagnitude", _g_get_sqrMagnitude);
		Utils.RegisterFunc(L, -2, "x", _g_get_x);
		Utils.RegisterFunc(L, -2, "y", _g_get_y);
		Utils.RegisterFunc(L, -2, "z", _g_get_z);
		Utils.RegisterFunc(L, -2, "w", _g_get_w);
		Utils.RegisterFunc(L, -1, "x", _s_set_x);
		Utils.RegisterFunc(L, -1, "y", _s_set_y);
		Utils.RegisterFunc(L, -1, "z", _s_set_z);
		Utils.RegisterFunc(L, -1, "w", _s_set_w);
		Utils.EndObjectRegister(typeFromHandle, L, translator, __CSIndexer, __NewIndexer, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 14, 4, 0);
		Utils.RegisterFunc(L, -4, "Lerp", _m_Lerp_xlua_st_);
		Utils.RegisterFunc(L, -4, "LerpUnclamped", _m_LerpUnclamped_xlua_st_);
		Utils.RegisterFunc(L, -4, "MoveTowards", _m_MoveTowards_xlua_st_);
		Utils.RegisterFunc(L, -4, "Scale", _m_Scale_xlua_st_);
		Utils.RegisterFunc(L, -4, "Normalize", _m_Normalize_xlua_st_);
		Utils.RegisterFunc(L, -4, "Dot", _m_Dot_xlua_st_);
		Utils.RegisterFunc(L, -4, "Project", _m_Project_xlua_st_);
		Utils.RegisterFunc(L, -4, "Distance", _m_Distance_xlua_st_);
		Utils.RegisterFunc(L, -4, "Magnitude", _m_Magnitude_xlua_st_);
		Utils.RegisterFunc(L, -4, "Min", _m_Min_xlua_st_);
		Utils.RegisterFunc(L, -4, "Max", _m_Max_xlua_st_);
		Utils.RegisterFunc(L, -4, "SqrMagnitude", _m_SqrMagnitude_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "kEpsilon", 1E-05f);
		Utils.RegisterFunc(L, -2, "zero", _g_get_zero);
		Utils.RegisterFunc(L, -2, "one", _g_get_one);
		Utils.RegisterFunc(L, -2, "positiveInfinity", _g_get_positiveInfinity);
		Utils.RegisterFunc(L, -2, "negativeInfinity", _g_get_negativeInfinity);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				float x = (float)Lua.lua_tonumber(L, 2);
				float y = (float)Lua.lua_tonumber(L, 3);
				float z = (float)Lua.lua_tonumber(L, 4);
				float w = (float)Lua.lua_tonumber(L, 5);
				Vector4 val = new Vector4(x, y, z, w);
				objectTranslator.PushUnityEngineVector4(L, val);
				return 1;
			}
			if (Lua.lua_gettop(L) == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float x2 = (float)Lua.lua_tonumber(L, 2);
				float y2 = (float)Lua.lua_tonumber(L, 3);
				float z2 = (float)Lua.lua_tonumber(L, 4);
				Vector4 val2 = new Vector4(x2, y2, z2);
				objectTranslator.PushUnityEngineVector4(L, val2);
				return 1;
			}
			if (Lua.lua_gettop(L) == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float x3 = (float)Lua.lua_tonumber(L, 2);
				float y3 = (float)Lua.lua_tonumber(L, 3);
				Vector4 val3 = new Vector4(x3, y3);
				objectTranslator.PushUnityEngineVector4(L, val3);
				return 1;
			}
			if (Lua.lua_gettop(L) == 1)
			{
				objectTranslator.PushUnityEngineVector4(L, default(Vector4));
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Vector4 constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int __CSIndexer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Vector4>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector4 val);
				int index = Lua.xlua_tointeger(L, 2);
				Lua.lua_pushboolean(L, value: true);
				Lua.lua_pushnumber(L, val[index]);
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
			if (objectTranslator.Assignable<Vector4>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector4 val);
				int index = Lua.xlua_tointeger(L, 2);
				val[index] = (float)Lua.lua_tonumber(L, 3);
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
	private static int __AddMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Vector4>(L, 1) && objectTranslator.Assignable<Vector4>(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector4 val);
				objectTranslator.Get(L, 2, out Vector4 val2);
				objectTranslator.PushUnityEngineVector4(L, val + val2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of + operator, need UnityEngine.Vector4!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __SubMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Vector4>(L, 1) && objectTranslator.Assignable<Vector4>(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector4 val);
				objectTranslator.Get(L, 2, out Vector4 val2);
				objectTranslator.PushUnityEngineVector4(L, val - val2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of - operator, need UnityEngine.Vector4!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __UnmMeta(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		try
		{
			objectTranslator.Get(L, 1, out Vector4 val);
			objectTranslator.PushUnityEngineVector4(L, -val);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __MulMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Vector4>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector4 val);
				float num = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.PushUnityEngineVector4(L, val * num);
				return 1;
			}
			if (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<Vector4>(L, 2))
			{
				float num2 = (float)Lua.lua_tonumber(L, 1);
				objectTranslator.Get(L, 2, out Vector4 val2);
				objectTranslator.PushUnityEngineVector4(L, num2 * val2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of * operator, need UnityEngine.Vector4!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __DivMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Vector4>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector4 val);
				float num = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.PushUnityEngineVector4(L, val / num);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of / operator, need UnityEngine.Vector4!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __EqMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Vector4>(L, 1) && objectTranslator.Assignable<Vector4>(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector4 val);
				objectTranslator.Get(L, 2, out Vector4 val2);
				Lua.lua_pushboolean(L, val == val2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of == operator, need UnityEngine.Vector4!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector4 val);
			float newX = (float)Lua.lua_tonumber(L, 2);
			float newY = (float)Lua.lua_tonumber(L, 3);
			float newZ = (float)Lua.lua_tonumber(L, 4);
			float newW = (float)Lua.lua_tonumber(L, 5);
			val.Set(newX, newY, newZ, newW);
			objectTranslator.UpdateUnityEngineVector4(L, 1, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Lerp_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector4 val);
			objectTranslator.Get(L, 2, out Vector4 val2);
			Vector4 val3 = Vector4.Lerp(t: (float)Lua.lua_tonumber(L, 3), a: val, b: val2);
			objectTranslator.PushUnityEngineVector4(L, val3);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LerpUnclamped_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector4 val);
			objectTranslator.Get(L, 2, out Vector4 val2);
			Vector4 val3 = Vector4.LerpUnclamped(t: (float)Lua.lua_tonumber(L, 3), a: val, b: val2);
			objectTranslator.PushUnityEngineVector4(L, val3);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MoveTowards_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector4 val);
			objectTranslator.Get(L, 2, out Vector4 val2);
			Vector4 val3 = Vector4.MoveTowards(maxDistanceDelta: (float)Lua.lua_tonumber(L, 3), current: val, target: val2);
			objectTranslator.PushUnityEngineVector4(L, val3);
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
			objectTranslator.Get(L, 1, out Vector4 val);
			objectTranslator.Get(L, 2, out Vector4 val2);
			Vector4 val3 = Vector4.Scale(val, val2);
			objectTranslator.PushUnityEngineVector4(L, val3);
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
			objectTranslator.Get(L, 1, out Vector4 val);
			objectTranslator.Get(L, 2, out Vector4 val2);
			val.Scale(val2);
			objectTranslator.UpdateUnityEngineVector4(L, 1, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetHashCode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector4 val);
			int hashCode = val.GetHashCode();
			Lua.xlua_pushinteger(L, hashCode);
			objectTranslator.UpdateUnityEngineVector4(L, 1, val);
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
			objectTranslator.Get(L, 1, out Vector4 val);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<object>(L, 2))
			{
				object obj = objectTranslator.GetObject(L, 2, typeof(object));
				bool value = val.Equals(obj);
				Lua.lua_pushboolean(L, value);
				objectTranslator.UpdateUnityEngineVector4(L, 1, val);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Vector4>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector4 val2);
				bool value2 = val.Equals(val2);
				Lua.lua_pushboolean(L, value2);
				objectTranslator.UpdateUnityEngineVector4(L, 1, val);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Vector4.Equals!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Normalize_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector4 val);
			Vector4 val2 = Vector4.Normalize(val);
			objectTranslator.PushUnityEngineVector4(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Normalize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector4 val);
			val.Normalize();
			objectTranslator.UpdateUnityEngineVector4(L, 1, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Dot_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector4 val);
			objectTranslator.Get(L, 2, out Vector4 val2);
			float num = Vector4.Dot(val, val2);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Project_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector4 val);
			objectTranslator.Get(L, 2, out Vector4 val2);
			Vector4 val3 = Vector4.Project(val, val2);
			objectTranslator.PushUnityEngineVector4(L, val3);
			return 1;
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
			objectTranslator.Get(L, 1, out Vector4 val);
			objectTranslator.Get(L, 2, out Vector4 val2);
			float num = Vector4.Distance(val, val2);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Magnitude_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector4 val);
			float num = Vector4.Magnitude(val);
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
			objectTranslator.Get(L, 1, out Vector4 val);
			objectTranslator.Get(L, 2, out Vector4 val2);
			Vector4 val3 = Vector4.Min(val, val2);
			objectTranslator.PushUnityEngineVector4(L, val3);
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
			objectTranslator.Get(L, 1, out Vector4 val);
			objectTranslator.Get(L, 2, out Vector4 val2);
			Vector4 val3 = Vector4.Max(val, val2);
			objectTranslator.PushUnityEngineVector4(L, val3);
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
			objectTranslator.Get(L, 1, out Vector4 val);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				string str2 = val.ToString();
				Lua.lua_pushstring(L, str2);
				objectTranslator.UpdateUnityEngineVector4(L, 1, val);
				return 1;
			}
			case 2:
				if (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING)
				{
					string text = Lua.lua_tostring(L, 2);
					string str = val.ToString(text);
					Lua.lua_pushstring(L, str);
					objectTranslator.UpdateUnityEngineVector4(L, 1, val);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Vector4.ToString!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SqrMagnitude_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector4 val);
			float num = Vector4.SqrMagnitude(val);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SqrMagnitude(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector4 val);
			float num = val.SqrMagnitude();
			Lua.lua_pushnumber(L, num);
			objectTranslator.UpdateUnityEngineVector4(L, 1, val);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_normalized(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector4 val);
			objectTranslator.PushUnityEngineVector4(L, val.normalized);
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
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector4 val);
			Lua.lua_pushnumber(L, val.magnitude);
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
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector4 val);
			Lua.lua_pushnumber(L, val.sqrMagnitude);
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
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector4(L, Vector4.zero);
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
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector4(L, Vector4.one);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_positiveInfinity(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector4(L, Vector4.positiveInfinity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_negativeInfinity(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector4(L, Vector4.negativeInfinity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_x(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector4 val);
			Lua.lua_pushnumber(L, val.x);
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
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector4 val);
			Lua.lua_pushnumber(L, val.y);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_z(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector4 val);
			Lua.lua_pushnumber(L, val.z);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_w(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector4 val);
			Lua.lua_pushnumber(L, val.w);
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
			objectTranslator.Get(L, 1, out Vector4 val);
			val.x = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.UpdateUnityEngineVector4(L, 1, val);
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
			objectTranslator.Get(L, 1, out Vector4 val);
			val.y = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.UpdateUnityEngineVector4(L, 1, val);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_z(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector4 val);
			val.z = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.UpdateUnityEngineVector4(L, 1, val);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_w(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector4 val);
			val.w = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.UpdateUnityEngineVector4(L, 1, val);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
