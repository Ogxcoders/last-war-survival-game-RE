using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineVector3Wrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Vector3);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 6, 7, 6, 3);
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
		Utils.RegisterFunc(L, -3, "ToVector2", _m_ToVector2);
		Utils.RegisterFunc(L, -2, "normalized", _g_get_normalized);
		Utils.RegisterFunc(L, -2, "magnitude", _g_get_magnitude);
		Utils.RegisterFunc(L, -2, "sqrMagnitude", _g_get_sqrMagnitude);
		Utils.RegisterFunc(L, -2, "x", _g_get_x);
		Utils.RegisterFunc(L, -2, "y", _g_get_y);
		Utils.RegisterFunc(L, -2, "z", _g_get_z);
		Utils.RegisterFunc(L, -1, "x", _s_set_x);
		Utils.RegisterFunc(L, -1, "y", _s_set_y);
		Utils.RegisterFunc(L, -1, "z", _s_set_z);
		Utils.EndObjectRegister(typeFromHandle, L, translator, __CSIndexer, __NewIndexer, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 26, 10, 0);
		Utils.RegisterFunc(L, -4, "Slerp", _m_Slerp_xlua_st_);
		Utils.RegisterFunc(L, -4, "SlerpUnclamped", _m_SlerpUnclamped_xlua_st_);
		Utils.RegisterFunc(L, -4, "OrthoNormalize", _m_OrthoNormalize_xlua_st_);
		Utils.RegisterFunc(L, -4, "RotateTowards", _m_RotateTowards_xlua_st_);
		Utils.RegisterFunc(L, -4, "Lerp", _m_Lerp_xlua_st_);
		Utils.RegisterFunc(L, -4, "LerpUnclamped", _m_LerpUnclamped_xlua_st_);
		Utils.RegisterFunc(L, -4, "MoveTowards", _m_MoveTowards_xlua_st_);
		Utils.RegisterFunc(L, -4, "SmoothDamp", _m_SmoothDamp_xlua_st_);
		Utils.RegisterFunc(L, -4, "Scale", _m_Scale_xlua_st_);
		Utils.RegisterFunc(L, -4, "Cross", _m_Cross_xlua_st_);
		Utils.RegisterFunc(L, -4, "Reflect", _m_Reflect_xlua_st_);
		Utils.RegisterFunc(L, -4, "Normalize", _m_Normalize_xlua_st_);
		Utils.RegisterFunc(L, -4, "Dot", _m_Dot_xlua_st_);
		Utils.RegisterFunc(L, -4, "Project", _m_Project_xlua_st_);
		Utils.RegisterFunc(L, -4, "ProjectOnPlane", _m_ProjectOnPlane_xlua_st_);
		Utils.RegisterFunc(L, -4, "Angle", _m_Angle_xlua_st_);
		Utils.RegisterFunc(L, -4, "SignedAngle", _m_SignedAngle_xlua_st_);
		Utils.RegisterFunc(L, -4, "Distance", _m_Distance_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClampMagnitude", _m_ClampMagnitude_xlua_st_);
		Utils.RegisterFunc(L, -4, "Magnitude", _m_Magnitude_xlua_st_);
		Utils.RegisterFunc(L, -4, "SqrMagnitude", _m_SqrMagnitude_xlua_st_);
		Utils.RegisterFunc(L, -4, "Min", _m_Min_xlua_st_);
		Utils.RegisterFunc(L, -4, "Max", _m_Max_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "kEpsilon", 1E-05f);
		Utils.RegisterObject(L, translator, -4, "kEpsilonNormalSqrt", 1E-15f);
		Utils.RegisterFunc(L, -2, "zero", _g_get_zero);
		Utils.RegisterFunc(L, -2, "one", _g_get_one);
		Utils.RegisterFunc(L, -2, "forward", _g_get_forward);
		Utils.RegisterFunc(L, -2, "back", _g_get_back);
		Utils.RegisterFunc(L, -2, "up", _g_get_up);
		Utils.RegisterFunc(L, -2, "down", _g_get_down);
		Utils.RegisterFunc(L, -2, "left", _g_get_left);
		Utils.RegisterFunc(L, -2, "right", _g_get_right);
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
			if (Lua.lua_gettop(L) == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float x = (float)Lua.lua_tonumber(L, 2);
				float y = (float)Lua.lua_tonumber(L, 3);
				float z = (float)Lua.lua_tonumber(L, 4);
				Vector3 val = new Vector3(x, y, z);
				objectTranslator.PushUnityEngineVector3(L, val);
				return 1;
			}
			if (Lua.lua_gettop(L) == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float x2 = (float)Lua.lua_tonumber(L, 2);
				float y2 = (float)Lua.lua_tonumber(L, 3);
				Vector3 val2 = new Vector3(x2, y2);
				objectTranslator.PushUnityEngineVector3(L, val2);
				return 1;
			}
			if (Lua.lua_gettop(L) == 1)
			{
				objectTranslator.PushUnityEngineVector3(L, default(Vector3));
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Vector3 constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int __CSIndexer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
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
			if (objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
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
			if (objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				objectTranslator.Get(L, 2, out Vector3 val2);
				objectTranslator.PushUnityEngineVector3(L, val + val2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of + operator, need UnityEngine.Vector3!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __SubMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				objectTranslator.Get(L, 2, out Vector3 val2);
				objectTranslator.PushUnityEngineVector3(L, val - val2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of - operator, need UnityEngine.Vector3!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __UnmMeta(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		try
		{
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.PushUnityEngineVector3(L, -val);
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
			if (objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				float num = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.PushUnityEngineVector3(L, val * num);
				return 1;
			}
			if (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<Vector3>(L, 2))
			{
				float num2 = (float)Lua.lua_tonumber(L, 1);
				objectTranslator.Get(L, 2, out Vector3 val2);
				objectTranslator.PushUnityEngineVector3(L, num2 * val2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of * operator, need UnityEngine.Vector3!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __DivMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				float num = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.PushUnityEngineVector3(L, val / num);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of / operator, need UnityEngine.Vector3!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __EqMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				objectTranslator.Get(L, 2, out Vector3 val2);
				Lua.lua_pushboolean(L, val == val2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of == operator, need UnityEngine.Vector3!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Slerp_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			Vector3 val3 = Vector3.Slerp(t: (float)Lua.lua_tonumber(L, 3), a: val, b: val2);
			objectTranslator.PushUnityEngineVector3(L, val3);
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
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			Vector3 val3 = Vector3.SlerpUnclamped(t: (float)Lua.lua_tonumber(L, 3), a: val, b: val2);
			objectTranslator.PushUnityEngineVector3(L, val3);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OrthoNormalize_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				objectTranslator.Get(L, 2, out Vector3 val2);
				Vector3.OrthoNormalize(ref val, ref val2);
				objectTranslator.PushUnityEngineVector3(L, val);
				objectTranslator.UpdateUnityEngineVector3(L, 1, val);
				objectTranslator.PushUnityEngineVector3(L, val2);
				objectTranslator.UpdateUnityEngineVector3(L, 2, val2);
				return 2;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector3 val3);
				objectTranslator.Get(L, 2, out Vector3 val4);
				objectTranslator.Get(L, 3, out Vector3 val5);
				Vector3.OrthoNormalize(ref val3, ref val4, ref val5);
				objectTranslator.PushUnityEngineVector3(L, val3);
				objectTranslator.UpdateUnityEngineVector3(L, 1, val3);
				objectTranslator.PushUnityEngineVector3(L, val4);
				objectTranslator.UpdateUnityEngineVector3(L, 2, val4);
				objectTranslator.PushUnityEngineVector3(L, val5);
				objectTranslator.UpdateUnityEngineVector3(L, 3, val5);
				return 3;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Vector3.OrthoNormalize!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RotateTowards_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			Vector3 val3 = Vector3.RotateTowards(maxRadiansDelta: (float)Lua.lua_tonumber(L, 3), maxMagnitudeDelta: (float)Lua.lua_tonumber(L, 4), current: val, target: val2);
			objectTranslator.PushUnityEngineVector3(L, val3);
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
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			Vector3 val3 = Vector3.Lerp(t: (float)Lua.lua_tonumber(L, 3), a: val, b: val2);
			objectTranslator.PushUnityEngineVector3(L, val3);
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
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			Vector3 val3 = Vector3.LerpUnclamped(t: (float)Lua.lua_tonumber(L, 3), a: val, b: val2);
			objectTranslator.PushUnityEngineVector3(L, val3);
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
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			Vector3 val3 = Vector3.MoveTowards(maxDistanceDelta: (float)Lua.lua_tonumber(L, 3), current: val, target: val2);
			objectTranslator.PushUnityEngineVector3(L, val3);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SmoothDamp_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				objectTranslator.Get(L, 2, out Vector3 val2);
				objectTranslator.Get(L, 3, out Vector3 val3);
				float smoothTime = (float)Lua.lua_tonumber(L, 4);
				Vector3 val4 = Vector3.SmoothDamp(val, val2, ref val3, smoothTime);
				objectTranslator.PushUnityEngineVector3(L, val4);
				objectTranslator.PushUnityEngineVector3(L, val3);
				objectTranslator.UpdateUnityEngineVector3(L, 3, val3);
				return 2;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val5);
				objectTranslator.Get(L, 2, out Vector3 val6);
				objectTranslator.Get(L, 3, out Vector3 val7);
				float smoothTime2 = (float)Lua.lua_tonumber(L, 4);
				float maxSpeed = (float)Lua.lua_tonumber(L, 5);
				Vector3 val8 = Vector3.SmoothDamp(val5, val6, ref val7, smoothTime2, maxSpeed);
				objectTranslator.PushUnityEngineVector3(L, val8);
				objectTranslator.PushUnityEngineVector3(L, val7);
				objectTranslator.UpdateUnityEngineVector3(L, 3, val7);
				return 2;
			}
			if (num == 6 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				objectTranslator.Get(L, 1, out Vector3 val9);
				objectTranslator.Get(L, 2, out Vector3 val10);
				objectTranslator.Get(L, 3, out Vector3 val11);
				float smoothTime3 = (float)Lua.lua_tonumber(L, 4);
				float maxSpeed2 = (float)Lua.lua_tonumber(L, 5);
				float deltaTime = (float)Lua.lua_tonumber(L, 6);
				Vector3 val12 = Vector3.SmoothDamp(val9, val10, ref val11, smoothTime3, maxSpeed2, deltaTime);
				objectTranslator.PushUnityEngineVector3(L, val12);
				objectTranslator.PushUnityEngineVector3(L, val11);
				objectTranslator.UpdateUnityEngineVector3(L, 3, val11);
				return 2;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Vector3.SmoothDamp!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			float newX = (float)Lua.lua_tonumber(L, 2);
			float newY = (float)Lua.lua_tonumber(L, 3);
			float newZ = (float)Lua.lua_tonumber(L, 4);
			val.Set(newX, newY, newZ);
			objectTranslator.UpdateUnityEngineVector3(L, 1, val);
			return 0;
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
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			Vector3 val3 = Vector3.Scale(val, val2);
			objectTranslator.PushUnityEngineVector3(L, val3);
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
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			val.Scale(val2);
			objectTranslator.UpdateUnityEngineVector3(L, 1, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Cross_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			Vector3 val3 = Vector3.Cross(val, val2);
			objectTranslator.PushUnityEngineVector3(L, val3);
			return 1;
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
			objectTranslator.Get(L, 1, out Vector3 val);
			int hashCode = val.GetHashCode();
			Lua.xlua_pushinteger(L, hashCode);
			objectTranslator.UpdateUnityEngineVector3(L, 1, val);
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
			objectTranslator.Get(L, 1, out Vector3 val);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<object>(L, 2))
			{
				object obj = objectTranslator.GetObject(L, 2, typeof(object));
				bool value = val.Equals(obj);
				Lua.lua_pushboolean(L, value);
				objectTranslator.UpdateUnityEngineVector3(L, 1, val);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				bool value2 = val.Equals(val2);
				Lua.lua_pushboolean(L, value2);
				objectTranslator.UpdateUnityEngineVector3(L, 1, val);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Vector3.Equals!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Reflect_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			Vector3 val3 = Vector3.Reflect(val, val2);
			objectTranslator.PushUnityEngineVector3(L, val3);
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
			objectTranslator.Get(L, 1, out Vector3 val);
			Vector3 val2 = Vector3.Normalize(val);
			objectTranslator.PushUnityEngineVector3(L, val2);
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
			objectTranslator.Get(L, 1, out Vector3 val);
			val.Normalize();
			objectTranslator.UpdateUnityEngineVector3(L, 1, val);
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
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			float num = Vector3.Dot(val, val2);
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
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			Vector3 val3 = Vector3.Project(val, val2);
			objectTranslator.PushUnityEngineVector3(L, val3);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ProjectOnPlane_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			Vector3 val3 = Vector3.ProjectOnPlane(val, val2);
			objectTranslator.PushUnityEngineVector3(L, val3);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Angle_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			float num = Vector3.Angle(val, val2);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SignedAngle_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			objectTranslator.Get(L, 3, out Vector3 val3);
			float num = Vector3.SignedAngle(val, val2, val3);
			Lua.lua_pushnumber(L, num);
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
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			float num = Vector3.Distance(val, val2);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClampMagnitude_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			Vector3 val2 = Vector3.ClampMagnitude(maxLength: (float)Lua.lua_tonumber(L, 2), vector: val);
			objectTranslator.PushUnityEngineVector3(L, val2);
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
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector3 val);
			float num = Vector3.Magnitude(val);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SqrMagnitude_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector3 val);
			float num = Vector3.SqrMagnitude(val);
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
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			Vector3 val3 = Vector3.Min(val, val2);
			objectTranslator.PushUnityEngineVector3(L, val3);
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
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			Vector3 val3 = Vector3.Max(val, val2);
			objectTranslator.PushUnityEngineVector3(L, val3);
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
			objectTranslator.Get(L, 1, out Vector3 val);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				string str2 = val.ToString();
				Lua.lua_pushstring(L, str2);
				objectTranslator.UpdateUnityEngineVector3(L, 1, val);
				return 1;
			}
			case 2:
				if (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING)
				{
					string text = Lua.lua_tostring(L, 2);
					string str = val.ToString(text);
					Lua.lua_pushstring(L, str);
					objectTranslator.UpdateUnityEngineVector3(L, 1, val);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Vector3.ToString!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToVector2(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			Vector2 val2 = val.ToVector2();
			objectTranslator.PushUnityEngineVector2(L, val2);
			objectTranslator.UpdateUnityEngineVector3(L, 1, val);
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
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.PushUnityEngineVector3(L, val.normalized);
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
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector3 val);
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
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector3 val);
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
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector3(L, Vector3.zero);
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
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector3(L, Vector3.one);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_forward(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector3(L, Vector3.forward);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_back(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector3(L, Vector3.back);
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
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector3(L, Vector3.up);
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
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector3(L, Vector3.down);
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
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector3(L, Vector3.left);
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
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector3(L, Vector3.right);
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
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector3(L, Vector3.positiveInfinity);
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
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector3(L, Vector3.negativeInfinity);
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
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector3 val);
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
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector3 val);
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
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector3 val);
			Lua.lua_pushnumber(L, val.z);
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
			objectTranslator.Get(L, 1, out Vector3 val);
			val.x = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.UpdateUnityEngineVector3(L, 1, val);
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
			objectTranslator.Get(L, 1, out Vector3 val);
			val.y = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.UpdateUnityEngineVector3(L, 1, val);
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
			objectTranslator.Get(L, 1, out Vector3 val);
			val.z = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.UpdateUnityEngineVector3(L, 1, val);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
