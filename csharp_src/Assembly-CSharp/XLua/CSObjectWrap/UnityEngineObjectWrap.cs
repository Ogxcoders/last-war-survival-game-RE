using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineObjectWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UnityEngine.Object);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 1, 5, 2, 2);
		Utils.RegisterFunc(L, -4, "__eq", __EqMeta);
		Utils.RegisterFunc(L, -3, "GetInstanceID", _m_GetInstanceID);
		Utils.RegisterFunc(L, -3, "GetHashCode", _m_GetHashCode);
		Utils.RegisterFunc(L, -3, "Equals", _m_Equals);
		Utils.RegisterFunc(L, -3, "ToString", _m_ToString);
		Utils.RegisterFunc(L, -3, "IsNull", _m_IsNull);
		Utils.RegisterFunc(L, -2, "name", _g_get_name);
		Utils.RegisterFunc(L, -2, "hideFlags", _g_get_hideFlags);
		Utils.RegisterFunc(L, -1, "name", _s_set_name);
		Utils.RegisterFunc(L, -1, "hideFlags", _s_set_hideFlags);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 7, 0, 0);
		Utils.RegisterFunc(L, -4, "Instantiate", _m_Instantiate_xlua_st_);
		Utils.RegisterFunc(L, -4, "Destroy", _m_Destroy_xlua_st_);
		Utils.RegisterFunc(L, -4, "DestroyImmediate", _m_DestroyImmediate_xlua_st_);
		Utils.RegisterFunc(L, -4, "FindObjectsOfType", _m_FindObjectsOfType_xlua_st_);
		Utils.RegisterFunc(L, -4, "DontDestroyOnLoad", _m_DontDestroyOnLoad_xlua_st_);
		Utils.RegisterFunc(L, -4, "FindObjectOfType", _m_FindObjectOfType_xlua_st_);
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
				UnityEngine.Object o = new UnityEngine.Object();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Object constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __EqMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<UnityEngine.Object>(L, 1) && objectTranslator.Assignable<UnityEngine.Object>(L, 2))
			{
				UnityEngine.Object obj = (UnityEngine.Object)objectTranslator.GetObject(L, 1, typeof(UnityEngine.Object));
				UnityEngine.Object obj2 = (UnityEngine.Object)objectTranslator.GetObject(L, 2, typeof(UnityEngine.Object));
				Lua.lua_pushboolean(L, obj == obj2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of == operator, need UnityEngine.Object!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetInstanceID(IntPtr L)
	{
		try
		{
			int instanceID = ((UnityEngine.Object)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetInstanceID();
			Lua.xlua_pushinteger(L, instanceID);
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
			int hashCode = ((UnityEngine.Object)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetHashCode();
			Lua.xlua_pushinteger(L, hashCode);
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
			UnityEngine.Object obj = (UnityEngine.Object)objectTranslator.FastGetCSObj(L, 1);
			object obj2 = objectTranslator.GetObject(L, 2, typeof(object));
			bool value = obj.Equals(obj2);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Instantiate_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<UnityEngine.Object>(L, 1))
			{
				UnityEngine.Object o = UnityEngine.Object.Instantiate((UnityEngine.Object)objectTranslator.GetObject(L, 1, typeof(UnityEngine.Object)));
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 1 && objectTranslator.Assignable<UnityEngine.Object>(L, 1))
			{
				UnityEngine.Object o2 = UnityEngine.Object.Instantiate((UnityEngine.Object)objectTranslator.GetObject(L, 1, typeof(UnityEngine.Object)));
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<UnityEngine.Object>(L, 1) && objectTranslator.Assignable<Transform>(L, 2))
			{
				UnityEngine.Object original = (UnityEngine.Object)objectTranslator.GetObject(L, 1, typeof(UnityEngine.Object));
				Transform parent = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
				UnityEngine.Object o3 = UnityEngine.Object.Instantiate(original, parent);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<UnityEngine.Object>(L, 1) && objectTranslator.Assignable<Transform>(L, 2))
			{
				UnityEngine.Object original2 = (UnityEngine.Object)objectTranslator.GetObject(L, 1, typeof(UnityEngine.Object));
				Transform parent2 = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
				UnityEngine.Object o4 = UnityEngine.Object.Instantiate(original2, parent2);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<UnityEngine.Object>(L, 1) && objectTranslator.Assignable<Transform>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				UnityEngine.Object original3 = (UnityEngine.Object)objectTranslator.GetObject(L, 1, typeof(UnityEngine.Object));
				Transform parent3 = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
				bool instantiateInWorldSpace = Lua.lua_toboolean(L, 3);
				UnityEngine.Object o5 = UnityEngine.Object.Instantiate(original3, parent3, instantiateInWorldSpace);
				objectTranslator.Push(L, o5);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<UnityEngine.Object>(L, 1) && objectTranslator.Assignable<Transform>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				UnityEngine.Object original4 = (UnityEngine.Object)objectTranslator.GetObject(L, 1, typeof(UnityEngine.Object));
				Transform parent4 = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
				bool instantiateInWorldSpace2 = Lua.lua_toboolean(L, 3);
				UnityEngine.Object o6 = UnityEngine.Object.Instantiate(original4, parent4, instantiateInWorldSpace2);
				objectTranslator.Push(L, o6);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<UnityEngine.Object>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3))
			{
				UnityEngine.Object original5 = (UnityEngine.Object)objectTranslator.GetObject(L, 1, typeof(UnityEngine.Object));
				objectTranslator.Get(L, 2, out Vector3 val);
				objectTranslator.Get(L, 3, out Quaternion val2);
				UnityEngine.Object o7 = UnityEngine.Object.Instantiate(original5, val, val2);
				objectTranslator.Push(L, o7);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<UnityEngine.Object>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3))
			{
				UnityEngine.Object original6 = (UnityEngine.Object)objectTranslator.GetObject(L, 1, typeof(UnityEngine.Object));
				objectTranslator.Get(L, 2, out Vector3 val3);
				objectTranslator.Get(L, 3, out Quaternion val4);
				UnityEngine.Object o8 = UnityEngine.Object.Instantiate(original6, val3, val4);
				objectTranslator.Push(L, o8);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<UnityEngine.Object>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3) && objectTranslator.Assignable<Transform>(L, 4))
			{
				UnityEngine.Object original7 = (UnityEngine.Object)objectTranslator.GetObject(L, 1, typeof(UnityEngine.Object));
				objectTranslator.Get(L, 2, out Vector3 val5);
				objectTranslator.Get(L, 3, out Quaternion val6);
				UnityEngine.Object o9 = UnityEngine.Object.Instantiate(parent: (Transform)objectTranslator.GetObject(L, 4, typeof(Transform)), original: original7, position: val5, rotation: val6);
				objectTranslator.Push(L, o9);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<UnityEngine.Object>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3) && objectTranslator.Assignable<Transform>(L, 4))
			{
				UnityEngine.Object original8 = (UnityEngine.Object)objectTranslator.GetObject(L, 1, typeof(UnityEngine.Object));
				objectTranslator.Get(L, 2, out Vector3 val7);
				objectTranslator.Get(L, 3, out Quaternion val8);
				UnityEngine.Object o10 = UnityEngine.Object.Instantiate(parent: (Transform)objectTranslator.GetObject(L, 4, typeof(Transform)), original: original8, position: val7, rotation: val8);
				objectTranslator.Push(L, o10);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Object.Instantiate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Destroy_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<UnityEngine.Object>(L, 1))
			{
				UnityEngine.Object.Destroy((UnityEngine.Object)objectTranslator.GetObject(L, 1, typeof(UnityEngine.Object)));
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<UnityEngine.Object>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				UnityEngine.Object obj = (UnityEngine.Object)objectTranslator.GetObject(L, 1, typeof(UnityEngine.Object));
				float t = (float)Lua.lua_tonumber(L, 2);
				UnityEngine.Object.Destroy(obj, t);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Object.Destroy!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DestroyImmediate_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<UnityEngine.Object>(L, 1))
			{
				UnityEngine.Object.DestroyImmediate((UnityEngine.Object)objectTranslator.GetObject(L, 1, typeof(UnityEngine.Object)));
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<UnityEngine.Object>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				UnityEngine.Object obj = (UnityEngine.Object)objectTranslator.GetObject(L, 1, typeof(UnityEngine.Object));
				bool allowDestroyingAssets = Lua.lua_toboolean(L, 2);
				UnityEngine.Object.DestroyImmediate(obj, allowDestroyingAssets);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Object.DestroyImmediate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindObjectsOfType_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Object[] o = UnityEngine.Object.FindObjectsOfType((Type)objectTranslator.GetObject(L, 1, typeof(Type)));
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DontDestroyOnLoad_xlua_st_(IntPtr L)
	{
		try
		{
			UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(UnityEngine.Object)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindObjectOfType_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Object o = UnityEngine.Object.FindObjectOfType((Type)objectTranslator.GetObject(L, 1, typeof(Type)));
			objectTranslator.Push(L, o);
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
			string str = ((UnityEngine.Object)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ToString();
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsNull(IntPtr L)
	{
		try
		{
			bool value = ((UnityEngine.Object)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsNull();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_name(IntPtr L)
	{
		try
		{
			UnityEngine.Object obj = (UnityEngine.Object)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, obj.name);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_hideFlags(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Object obj = (UnityEngine.Object)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, obj.hideFlags);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_name(IntPtr L)
	{
		try
		{
			((UnityEngine.Object)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).name = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_hideFlags(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Object obj = (UnityEngine.Object)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out HideFlags v);
			obj.hideFlags = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
