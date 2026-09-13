using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SystemCollectionsGenericDictionary_2_ResourceTypeSystemInt32_Wrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Dictionary<ResourceType, int>);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 9, 4, 0);
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
		Utils.EndObjectRegister(typeFromHandle, L, translator, __CSIndexer, __NewIndexer, null, null, null);
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
				Dictionary<ResourceType, int> o = new Dictionary<ResourceType, int>();
				objectTranslator.Push(L, o);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				Dictionary<ResourceType, int> o2 = new Dictionary<ResourceType, int>(Lua.xlua_tointeger(L, 2));
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<IEqualityComparer<ResourceType>>(L, 2))
			{
				Dictionary<ResourceType, int> o3 = new Dictionary<ResourceType, int>((IEqualityComparer<ResourceType>)objectTranslator.GetObject(L, 2, typeof(IEqualityComparer<ResourceType>)));
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (Lua.lua_gettop(L) == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<IEqualityComparer<ResourceType>>(L, 3))
			{
				int capacity = Lua.xlua_tointeger(L, 2);
				IEqualityComparer<ResourceType> comparer = (IEqualityComparer<ResourceType>)objectTranslator.GetObject(L, 3, typeof(IEqualityComparer<ResourceType>));
				Dictionary<ResourceType, int> o4 = new Dictionary<ResourceType, int>(capacity, comparer);
				objectTranslator.Push(L, o4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Collections.Generic.Dictionary<ResourceType, int> constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int __CSIndexer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Dictionary<ResourceType, int>>(L, 1) && objectTranslator.Assignable<ResourceType>(L, 2))
			{
				Dictionary<ResourceType, int> dictionary = (Dictionary<ResourceType, int>)objectTranslator.FastGetCSObj(L, 1);
				objectTranslator.Get(L, 2, out ResourceType val);
				Lua.lua_pushboolean(L, value: true);
				Lua.xlua_pushinteger(L, dictionary[val]);
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
			if (objectTranslator.Assignable<Dictionary<ResourceType, int>>(L, 1) && objectTranslator.Assignable<ResourceType>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				Dictionary<ResourceType, int> obj = (Dictionary<ResourceType, int>)objectTranslator.FastGetCSObj(L, 1);
				objectTranslator.Get(L, 2, out ResourceType val);
				obj[val] = Lua.xlua_tointeger(L, 3);
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
	private static int _m_Add(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<ResourceType, int> dictionary = (Dictionary<ResourceType, int>)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ResourceType val);
			int value = Lua.xlua_tointeger(L, 3);
			dictionary.Add(val, value);
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
			((Dictionary<ResourceType, int>)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Clear();
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
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<ResourceType, int> dictionary = (Dictionary<ResourceType, int>)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ResourceType val);
			bool value = dictionary.ContainsKey(val);
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
			Dictionary<ResourceType, int> obj = (Dictionary<ResourceType, int>)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int value = Lua.xlua_tointeger(L, 2);
			bool value2 = obj.ContainsValue(value);
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
			Dictionary<ResourceType, int>.Enumerator enumerator = ((Dictionary<ResourceType, int>)objectTranslator.FastGetCSObj(L, 1)).GetEnumerator();
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
			Dictionary<ResourceType, int> dictionary = (Dictionary<ResourceType, int>)objectTranslator.FastGetCSObj(L, 1);
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
			Dictionary<ResourceType, int> dictionary = (Dictionary<ResourceType, int>)objectTranslator.FastGetCSObj(L, 1);
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
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<ResourceType, int> dictionary = (Dictionary<ResourceType, int>)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ResourceType val);
			bool value = dictionary.Remove(val);
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
			Dictionary<ResourceType, int> dictionary = (Dictionary<ResourceType, int>)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ResourceType val);
			int value2;
			bool value = dictionary.TryGetValue(val, out value2);
			Lua.lua_pushboolean(L, value);
			Lua.xlua_pushinteger(L, value2);
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
			Dictionary<ResourceType, int> dictionary = (Dictionary<ResourceType, int>)objectTranslator.FastGetCSObj(L, 1);
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
			Dictionary<ResourceType, int> dictionary = (Dictionary<ResourceType, int>)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			Dictionary<ResourceType, int> dictionary = (Dictionary<ResourceType, int>)objectTranslator.FastGetCSObj(L, 1);
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
			Dictionary<ResourceType, int> dictionary = (Dictionary<ResourceType, int>)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, dictionary.Values);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
