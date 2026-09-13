using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SystemCollectionsGenericDictionary_2_SystemStringUnityEngineGameObject_Wrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Dictionary<string, GameObject>);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 11, 4, 0);
		Utils.RegisterFunc(L, -3, "get_Item", _m_get_Item);
		Utils.RegisterFunc(L, -3, "set_Item", _m_set_Item);
		Utils.RegisterFunc(L, -3, "Add", _m_Add);
		Utils.RegisterFunc(L, -3, "Clear", _m_Clear);
		Utils.RegisterFunc(L, -3, "ContainsKey", _m_ContainsKey);
		Utils.RegisterFunc(L, -3, "ContainsValue", _m_ContainsValue);
		Utils.RegisterFunc(L, -3, "GetEnumerator", _m_GetEnumerator);
		Utils.RegisterFunc(L, -3, "GetObjectData", _m_GetObjectData);
		Utils.RegisterFunc(L, -3, "OnDeserialization", _m_OnDeserialization);
		Utils.RegisterFunc(L, -3, "Remove", _m_Remove);
		Utils.RegisterFunc(L, -3, "TryGetValue", _m_TryGetValue);
		Utils.RegisterFunc(L, -2, "Comparer", _g_get_Comparer);
		Utils.RegisterFunc(L, -2, "Count", _g_get_Count);
		Utils.RegisterFunc(L, -2, "Keys", _g_get_Keys);
		Utils.RegisterFunc(L, -2, "Values", _g_get_Values);
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
				Dictionary<string, GameObject> o = new Dictionary<string, GameObject>();
				objectTranslator.Push(L, o);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				Dictionary<string, GameObject> o2 = new Dictionary<string, GameObject>(Lua.xlua_tointeger(L, 2));
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<IEqualityComparer<string>>(L, 2))
			{
				Dictionary<string, GameObject> o3 = new Dictionary<string, GameObject>((IEqualityComparer<string>)objectTranslator.GetObject(L, 2, typeof(IEqualityComparer<string>)));
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (Lua.lua_gettop(L) == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<IEqualityComparer<string>>(L, 3))
			{
				int capacity = Lua.xlua_tointeger(L, 2);
				IEqualityComparer<string> comparer = (IEqualityComparer<string>)objectTranslator.GetObject(L, 3, typeof(IEqualityComparer<string>));
				Dictionary<string, GameObject> o4 = new Dictionary<string, GameObject>(capacity, comparer);
				objectTranslator.Push(L, o4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Collections.Generic.Dictionary<string, UnityEngine.GameObject> constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_get_Item(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<string, GameObject> dictionary = (Dictionary<string, GameObject>)objectTranslator.FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			objectTranslator.Push(L, dictionary[key]);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_set_Item(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<string, GameObject> dictionary = (Dictionary<string, GameObject>)objectTranslator.FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			GameObject value = (GameObject)objectTranslator.GetObject(L, 3, typeof(GameObject));
			dictionary[key] = value;
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Add(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<string, GameObject> dictionary = (Dictionary<string, GameObject>)objectTranslator.FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			GameObject value = (GameObject)objectTranslator.GetObject(L, 3, typeof(GameObject));
			dictionary.Add(key, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clear(IntPtr L)
	{
		try
		{
			((Dictionary<string, GameObject>)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Clear();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ContainsKey(IntPtr L)
	{
		try
		{
			Dictionary<string, GameObject> obj = (Dictionary<string, GameObject>)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			bool value = obj.ContainsKey(key);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ContainsValue(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<string, GameObject> dictionary = (Dictionary<string, GameObject>)objectTranslator.FastGetCSObj(L, 1);
			GameObject value = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
			bool value2 = dictionary.ContainsValue(value);
			Lua.lua_pushboolean(L, value2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetEnumerator(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<string, GameObject>.Enumerator enumerator = ((Dictionary<string, GameObject>)objectTranslator.FastGetCSObj(L, 1)).GetEnumerator();
			objectTranslator.Push(L, enumerator);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetObjectData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<string, GameObject> dictionary = (Dictionary<string, GameObject>)objectTranslator.FastGetCSObj(L, 1);
			SerializationInfo info = (SerializationInfo)objectTranslator.GetObject(L, 2, typeof(SerializationInfo));
			objectTranslator.Get(L, 3, out StreamingContext v);
			dictionary.GetObjectData(info, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDeserialization(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<string, GameObject> dictionary = (Dictionary<string, GameObject>)objectTranslator.FastGetCSObj(L, 1);
			object sender = objectTranslator.GetObject(L, 2, typeof(object));
			dictionary.OnDeserialization(sender);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Remove(IntPtr L)
	{
		try
		{
			Dictionary<string, GameObject> obj = (Dictionary<string, GameObject>)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			bool value = obj.Remove(key);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryGetValue(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<string, GameObject> obj = (Dictionary<string, GameObject>)objectTranslator.FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			GameObject value2;
			bool value = obj.TryGetValue(key, out value2);
			Lua.lua_pushboolean(L, value);
			objectTranslator.Push(L, value2);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Comparer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<string, GameObject> dictionary = (Dictionary<string, GameObject>)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushAny(L, dictionary.Comparer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Count(IntPtr L)
	{
		try
		{
			Dictionary<string, GameObject> dictionary = (Dictionary<string, GameObject>)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, dictionary.Count);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Keys(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<string, GameObject> dictionary = (Dictionary<string, GameObject>)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, dictionary.Keys);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Values(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<string, GameObject> dictionary = (Dictionary<string, GameObject>)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, dictionary.Values);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
