using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineQuaternionWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Quaternion);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 2, 8, 6, 5);
		Utils.RegisterFunc(L, -4, "__mul", __MulMeta);
		Utils.RegisterFunc(L, -4, "__eq", __EqMeta);
		Utils.RegisterFunc(L, -3, "Set", _m_Set);
		Utils.RegisterFunc(L, -3, "SetLookRotation", _m_SetLookRotation);
		Utils.RegisterFunc(L, -3, "ToAngleAxis", _m_ToAngleAxis);
		Utils.RegisterFunc(L, -3, "SetFromToRotation", _m_SetFromToRotation);
		Utils.RegisterFunc(L, -3, "Normalize", _m_Normalize);
		Utils.RegisterFunc(L, -3, "GetHashCode", _m_GetHashCode);
		Utils.RegisterFunc(L, -3, "Equals", _m_Equals);
		Utils.RegisterFunc(L, -3, "ToString", _m_ToString);
		Utils.RegisterFunc(L, -2, "eulerAngles", _g_get_eulerAngles);
		Utils.RegisterFunc(L, -2, "normalized", _g_get_normalized);
		Utils.RegisterFunc(L, -2, "x", _g_get_x);
		Utils.RegisterFunc(L, -2, "y", _g_get_y);
		Utils.RegisterFunc(L, -2, "z", _g_get_z);
		Utils.RegisterFunc(L, -2, "w", _g_get_w);
		Utils.RegisterFunc(L, -1, "eulerAngles", _s_set_eulerAngles);
		Utils.RegisterFunc(L, -1, "x", _s_set_x);
		Utils.RegisterFunc(L, -1, "y", _s_set_y);
		Utils.RegisterFunc(L, -1, "z", _s_set_z);
		Utils.RegisterFunc(L, -1, "w", _s_set_w);
		Utils.EndObjectRegister(typeFromHandle, L, translator, __CSIndexer, __NewIndexer, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 15, 1, 0);
		Utils.RegisterFunc(L, -4, "FromToRotation", _m_FromToRotation_xlua_st_);
		Utils.RegisterFunc(L, -4, "Inverse", _m_Inverse_xlua_st_);
		Utils.RegisterFunc(L, -4, "Slerp", _m_Slerp_xlua_st_);
		Utils.RegisterFunc(L, -4, "SlerpUnclamped", _m_SlerpUnclamped_xlua_st_);
		Utils.RegisterFunc(L, -4, "Lerp", _m_Lerp_xlua_st_);
		Utils.RegisterFunc(L, -4, "LerpUnclamped", _m_LerpUnclamped_xlua_st_);
		Utils.RegisterFunc(L, -4, "AngleAxis", _m_AngleAxis_xlua_st_);
		Utils.RegisterFunc(L, -4, "LookRotation", _m_LookRotation_xlua_st_);
		Utils.RegisterFunc(L, -4, "Dot", _m_Dot_xlua_st_);
		Utils.RegisterFunc(L, -4, "Angle", _m_Angle_xlua_st_);
		Utils.RegisterFunc(L, -4, "Euler", _m_Euler_xlua_st_);
		Utils.RegisterFunc(L, -4, "RotateTowards", _m_RotateTowards_xlua_st_);
		Utils.RegisterFunc(L, -4, "Normalize", _m_Normalize_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "kEpsilon", 1E-06f);
		Utils.RegisterFunc(L, -2, "identity", _g_get_identity);
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
				Quaternion val = new Quaternion(x, y, z, w);
				objectTranslator.PushUnityEngineQuaternion(L, val);
				return 1;
			}
			if (Lua.lua_gettop(L) == 1)
			{
				objectTranslator.PushUnityEngineQuaternion(L, default(Quaternion));
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Quaternion constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int __CSIndexer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Quaternion>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out Quaternion val);
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
			if (objectTranslator.Assignable<Quaternion>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out Quaternion val);
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
	private static int __MulMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Quaternion>(L, 1) && objectTranslator.Assignable<Quaternion>(L, 2))
			{
				objectTranslator.Get(L, 1, out Quaternion val);
				objectTranslator.Get(L, 2, out Quaternion val2);
				objectTranslator.PushUnityEngineQuaternion(L, val * val2);
				return 1;
			}
			if (objectTranslator.Assignable<Quaternion>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 1, out Quaternion val3);
				objectTranslator.Get(L, 2, out Vector3 val4);
				objectTranslator.PushUnityEngineVector3(L, val3 * val4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of * operator, need UnityEngine.Quaternion!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __EqMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Quaternion>(L, 1) && objectTranslator.Assignable<Quaternion>(L, 2))
			{
				objectTranslator.Get(L, 1, out Quaternion val);
				objectTranslator.Get(L, 2, out Quaternion val2);
				Lua.lua_pushboolean(L, val == val2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of == operator, need UnityEngine.Quaternion!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FromToRotation_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			Quaternion val3 = Quaternion.FromToRotation(val, val2);
			objectTranslator.PushUnityEngineQuaternion(L, val3);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Inverse_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Quaternion val);
			Quaternion val2 = Quaternion.Inverse(val);
			objectTranslator.PushUnityEngineQuaternion(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Slerp_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Quaternion val);
			objectTranslator.Get(L, 2, out Quaternion val2);
			Quaternion val3 = Quaternion.Slerp(t: (float)Lua.lua_tonumber(L, 3), a: val, b: val2);
			objectTranslator.PushUnityEngineQuaternion(L, val3);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SlerpUnclamped_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Quaternion val);
			objectTranslator.Get(L, 2, out Quaternion val2);
			Quaternion val3 = Quaternion.SlerpUnclamped(t: (float)Lua.lua_tonumber(L, 3), a: val, b: val2);
			objectTranslator.PushUnityEngineQuaternion(L, val3);
			return 1;
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
			objectTranslator.Get(L, 1, out Quaternion val);
			objectTranslator.Get(L, 2, out Quaternion val2);
			Quaternion val3 = Quaternion.Lerp(t: (float)Lua.lua_tonumber(L, 3), a: val, b: val2);
			objectTranslator.PushUnityEngineQuaternion(L, val3);
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
			objectTranslator.Get(L, 1, out Quaternion val);
			objectTranslator.Get(L, 2, out Quaternion val2);
			Quaternion val3 = Quaternion.LerpUnclamped(t: (float)Lua.lua_tonumber(L, 3), a: val, b: val2);
			objectTranslator.PushUnityEngineQuaternion(L, val3);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AngleAxis_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			float angle = (float)Lua.lua_tonumber(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Quaternion val2 = Quaternion.AngleAxis(angle, val);
			objectTranslator.PushUnityEngineQuaternion(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LookRotation_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<Vector3>(L, 1))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				Quaternion val2 = Quaternion.LookRotation(val);
				objectTranslator.PushUnityEngineQuaternion(L, val2);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector3 val3);
				objectTranslator.Get(L, 2, out Vector3 val4);
				Quaternion val5 = Quaternion.LookRotation(val3, val4);
				objectTranslator.PushUnityEngineQuaternion(L, val5);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Quaternion.LookRotation!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Quaternion val);
			float newX = (float)Lua.lua_tonumber(L, 2);
			float newY = (float)Lua.lua_tonumber(L, 3);
			float newZ = (float)Lua.lua_tonumber(L, 4);
			float newW = (float)Lua.lua_tonumber(L, 5);
			val.Set(newX, newY, newZ, newW);
			objectTranslator.UpdateUnityEngineQuaternion(L, 1, val);
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
			objectTranslator.Get(L, 1, out Quaternion val);
			objectTranslator.Get(L, 2, out Quaternion val2);
			float num = Quaternion.Dot(val, val2);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLookRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Quaternion val);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				val.SetLookRotation(val2);
				objectTranslator.UpdateUnityEngineQuaternion(L, 1, val);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				objectTranslator.Get(L, 3, out Vector3 val4);
				val.SetLookRotation(val3, val4);
				objectTranslator.UpdateUnityEngineQuaternion(L, 1, val);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Quaternion.SetLookRotation!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Angle_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Quaternion val);
			objectTranslator.Get(L, 2, out Quaternion val2);
			float num = Quaternion.Angle(val, val2);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Euler_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float x = (float)Lua.lua_tonumber(L, 1);
				float y = (float)Lua.lua_tonumber(L, 2);
				float z = (float)Lua.lua_tonumber(L, 3);
				Quaternion val = Quaternion.Euler(x, y, z);
				objectTranslator.PushUnityEngineQuaternion(L, val);
				return 1;
			}
			if (num == 1 && objectTranslator.Assignable<Vector3>(L, 1))
			{
				objectTranslator.Get(L, 1, out Vector3 val2);
				Quaternion val3 = Quaternion.Euler(val2);
				objectTranslator.PushUnityEngineQuaternion(L, val3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Quaternion.Euler!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToAngleAxis(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Quaternion val);
			val.ToAngleAxis(out var angle, out var axis);
			Lua.lua_pushnumber(L, angle);
			objectTranslator.PushUnityEngineVector3(L, axis);
			objectTranslator.UpdateUnityEngineQuaternion(L, 1, val);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFromToRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Quaternion val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			objectTranslator.Get(L, 3, out Vector3 val3);
			val.SetFromToRotation(val2, val3);
			objectTranslator.UpdateUnityEngineQuaternion(L, 1, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RotateTowards_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Quaternion val);
			objectTranslator.Get(L, 2, out Quaternion val2);
			Quaternion val3 = Quaternion.RotateTowards(maxDegreesDelta: (float)Lua.lua_tonumber(L, 3), from: val, to: val2);
			objectTranslator.PushUnityEngineQuaternion(L, val3);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Normalize_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Quaternion val);
			Quaternion val2 = Quaternion.Normalize(val);
			objectTranslator.PushUnityEngineQuaternion(L, val2);
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
			objectTranslator.Get(L, 1, out Quaternion val);
			val.Normalize();
			objectTranslator.UpdateUnityEngineQuaternion(L, 1, val);
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
			objectTranslator.Get(L, 1, out Quaternion val);
			int hashCode = val.GetHashCode();
			Lua.xlua_pushinteger(L, hashCode);
			objectTranslator.UpdateUnityEngineQuaternion(L, 1, val);
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
			objectTranslator.Get(L, 1, out Quaternion val);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<object>(L, 2))
			{
				object obj = objectTranslator.GetObject(L, 2, typeof(object));
				bool value = val.Equals(obj);
				Lua.lua_pushboolean(L, value);
				objectTranslator.UpdateUnityEngineQuaternion(L, 1, val);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Quaternion>(L, 2))
			{
				objectTranslator.Get(L, 2, out Quaternion val2);
				bool value2 = val.Equals(val2);
				Lua.lua_pushboolean(L, value2);
				objectTranslator.UpdateUnityEngineQuaternion(L, 1, val);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Quaternion.Equals!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToString(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Quaternion val);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				string str2 = val.ToString();
				Lua.lua_pushstring(L, str2);
				objectTranslator.UpdateUnityEngineQuaternion(L, 1, val);
				return 1;
			}
			case 2:
				if (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING)
				{
					string text = Lua.lua_tostring(L, 2);
					string str = val.ToString(text);
					Lua.lua_pushstring(L, str);
					objectTranslator.UpdateUnityEngineQuaternion(L, 1, val);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Quaternion.ToString!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_identity(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineQuaternion(L, Quaternion.identity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_eulerAngles(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Quaternion val);
			objectTranslator.PushUnityEngineVector3(L, val.eulerAngles);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_normalized(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Quaternion val);
			objectTranslator.PushUnityEngineQuaternion(L, val.normalized);
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
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Quaternion val);
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
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Quaternion val);
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
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Quaternion val);
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
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Quaternion val);
			Lua.lua_pushnumber(L, val.w);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_eulerAngles(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Quaternion val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			val.eulerAngles = val2;
			objectTranslator.UpdateUnityEngineQuaternion(L, 1, val);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_x(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Quaternion val);
			val.x = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.UpdateUnityEngineQuaternion(L, 1, val);
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
			objectTranslator.Get(L, 1, out Quaternion val);
			val.y = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.UpdateUnityEngineQuaternion(L, 1, val);
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
			objectTranslator.Get(L, 1, out Quaternion val);
			val.z = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.UpdateUnityEngineQuaternion(L, 1, val);
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
			objectTranslator.Get(L, 1, out Quaternion val);
			val.w = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.UpdateUnityEngineQuaternion(L, 1, val);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
