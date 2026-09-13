using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SystemCollectionsGenericList_1_SystemInt64_Wrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(List<long>);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 29, 2, 1);
		Utils.RegisterFunc(L, -3, "Add", _m_Add);
		Utils.RegisterFunc(L, -3, "AddRange", _m_AddRange);
		Utils.RegisterFunc(L, -3, "AsReadOnly", _m_AsReadOnly);
		Utils.RegisterFunc(L, -3, "BinarySearch", _m_BinarySearch);
		Utils.RegisterFunc(L, -3, "Clear", _m_Clear);
		Utils.RegisterFunc(L, -3, "Contains", _m_Contains);
		Utils.RegisterFunc(L, -3, "CopyTo", _m_CopyTo);
		Utils.RegisterFunc(L, -3, "Exists", _m_Exists);
		Utils.RegisterFunc(L, -3, "Find", _m_Find);
		Utils.RegisterFunc(L, -3, "FindAll", _m_FindAll);
		Utils.RegisterFunc(L, -3, "FindIndex", _m_FindIndex);
		Utils.RegisterFunc(L, -3, "FindLast", _m_FindLast);
		Utils.RegisterFunc(L, -3, "FindLastIndex", _m_FindLastIndex);
		Utils.RegisterFunc(L, -3, "ForEach", _m_ForEach);
		Utils.RegisterFunc(L, -3, "GetEnumerator", _m_GetEnumerator);
		Utils.RegisterFunc(L, -3, "GetRange", _m_GetRange);
		Utils.RegisterFunc(L, -3, "IndexOf", _m_IndexOf);
		Utils.RegisterFunc(L, -3, "Insert", _m_Insert);
		Utils.RegisterFunc(L, -3, "InsertRange", _m_InsertRange);
		Utils.RegisterFunc(L, -3, "LastIndexOf", _m_LastIndexOf);
		Utils.RegisterFunc(L, -3, "Remove", _m_Remove);
		Utils.RegisterFunc(L, -3, "RemoveAll", _m_RemoveAll);
		Utils.RegisterFunc(L, -3, "RemoveAt", _m_RemoveAt);
		Utils.RegisterFunc(L, -3, "RemoveRange", _m_RemoveRange);
		Utils.RegisterFunc(L, -3, "Reverse", _m_Reverse);
		Utils.RegisterFunc(L, -3, "Sort", _m_Sort);
		Utils.RegisterFunc(L, -3, "ToArray", _m_ToArray);
		Utils.RegisterFunc(L, -3, "TrimExcess", _m_TrimExcess);
		Utils.RegisterFunc(L, -3, "TrueForAll", _m_TrueForAll);
		Utils.RegisterFunc(L, -2, "Capacity", _g_get_Capacity);
		Utils.RegisterFunc(L, -2, "Count", _g_get_Count);
		Utils.RegisterFunc(L, -1, "Capacity", _s_set_Capacity);
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
				List<long> o = new List<long>();
				objectTranslator.Push(L, o);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				List<long> o2 = new List<long>(Lua.xlua_tointeger(L, 2));
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<IEnumerable<long>>(L, 2))
			{
				List<long> o3 = new List<long>((IEnumerable<long>)objectTranslator.GetObject(L, 2, typeof(IEnumerable<long>)));
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Collections.Generic.List<long> constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int __CSIndexer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<List<long>>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				List<long> list = (List<long>)objectTranslator.FastGetCSObj(L, 1);
				int index = Lua.xlua_tointeger(L, 2);
				Lua.lua_pushboolean(L, value: true);
				Lua.lua_pushint64(L, list[index]);
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
			if (objectTranslator.Assignable<List<long>>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)))
			{
				List<long> obj = (List<long>)objectTranslator.FastGetCSObj(L, 1);
				int index = Lua.xlua_tointeger(L, 2);
				obj[index] = Lua.lua_toint64(L, 3);
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
			List<long> obj = (List<long>)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long item = Lua.lua_toint64(L, 2);
			obj.Add(item);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddRange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<long> list = (List<long>)objectTranslator.FastGetCSObj(L, 1);
			IEnumerable<long> collection = (IEnumerable<long>)objectTranslator.GetObject(L, 2, typeof(IEnumerable<long>));
			list.AddRange(collection);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AsReadOnly(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ReadOnlyCollection<long> o = ((List<long>)objectTranslator.FastGetCSObj(L, 1)).AsReadOnly();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BinarySearch(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<long> list = (List<long>)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)))
			{
				long item = Lua.lua_toint64(L, 2);
				int value = list.BinarySearch(item);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 3 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && objectTranslator.Assignable<IComparer<long>>(L, 3))
			{
				long item2 = Lua.lua_toint64(L, 2);
				IComparer<long> comparer = (IComparer<long>)objectTranslator.GetObject(L, 3, typeof(IComparer<long>));
				int value2 = list.BinarySearch(item2, comparer);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) || Lua.lua_isint64(L, 4)) && objectTranslator.Assignable<IComparer<long>>(L, 5))
			{
				int index = Lua.xlua_tointeger(L, 2);
				int count = Lua.xlua_tointeger(L, 3);
				long item3 = Lua.lua_toint64(L, 4);
				IComparer<long> comparer2 = (IComparer<long>)objectTranslator.GetObject(L, 5, typeof(IComparer<long>));
				int value3 = list.BinarySearch(index, count, item3, comparer2);
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Collections.Generic.List<long>.BinarySearch!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clear(IntPtr L)
	{
		try
		{
			((List<long>)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Clear();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Contains(IntPtr L)
	{
		try
		{
			List<long> obj = (List<long>)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long item = Lua.lua_toint64(L, 2);
			bool value = obj.Contains(item);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CopyTo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<long> list = (List<long>)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<long[]>(L, 2))
			{
				long[] array = (long[])objectTranslator.GetObject(L, 2, typeof(long[]));
				list.CopyTo(array);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<long[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				long[] array2 = (long[])objectTranslator.GetObject(L, 2, typeof(long[]));
				int arrayIndex = Lua.xlua_tointeger(L, 3);
				list.CopyTo(array2, arrayIndex);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<long[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int index = Lua.xlua_tointeger(L, 2);
				long[] array3 = (long[])objectTranslator.GetObject(L, 3, typeof(long[]));
				int arrayIndex2 = Lua.xlua_tointeger(L, 4);
				int count = Lua.xlua_tointeger(L, 5);
				list.CopyTo(index, array3, arrayIndex2, count);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Collections.Generic.List<long>.CopyTo!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Exists(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<long> list = (List<long>)objectTranslator.FastGetCSObj(L, 1);
			Predicate<long> match = objectTranslator.GetDelegate<Predicate<long>>(L, 2);
			bool value = list.Exists(match);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Find(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<long> list = (List<long>)objectTranslator.FastGetCSObj(L, 1);
			Predicate<long> match = objectTranslator.GetDelegate<Predicate<long>>(L, 2);
			long n = list.Find(match);
			Lua.lua_pushint64(L, n);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindAll(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<long> list = (List<long>)objectTranslator.FastGetCSObj(L, 1);
			Predicate<long> match = objectTranslator.GetDelegate<Predicate<long>>(L, 2);
			List<long> o = list.FindAll(match);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<long> list = (List<long>)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Predicate<long>>(L, 2))
			{
				Predicate<long> match = objectTranslator.GetDelegate<Predicate<long>>(L, 2);
				int value = list.FindIndex(match);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Predicate<long>>(L, 3))
			{
				int startIndex = Lua.xlua_tointeger(L, 2);
				Predicate<long> match2 = objectTranslator.GetDelegate<Predicate<long>>(L, 3);
				int value2 = list.FindIndex(startIndex, match2);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Predicate<long>>(L, 4))
			{
				int startIndex2 = Lua.xlua_tointeger(L, 2);
				int count = Lua.xlua_tointeger(L, 3);
				Predicate<long> match3 = objectTranslator.GetDelegate<Predicate<long>>(L, 4);
				int value3 = list.FindIndex(startIndex2, count, match3);
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Collections.Generic.List<long>.FindIndex!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindLast(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<long> list = (List<long>)objectTranslator.FastGetCSObj(L, 1);
			Predicate<long> match = objectTranslator.GetDelegate<Predicate<long>>(L, 2);
			long n = list.FindLast(match);
			Lua.lua_pushint64(L, n);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindLastIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<long> list = (List<long>)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Predicate<long>>(L, 2))
			{
				Predicate<long> match = objectTranslator.GetDelegate<Predicate<long>>(L, 2);
				int value = list.FindLastIndex(match);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Predicate<long>>(L, 3))
			{
				int startIndex = Lua.xlua_tointeger(L, 2);
				Predicate<long> match2 = objectTranslator.GetDelegate<Predicate<long>>(L, 3);
				int value2 = list.FindLastIndex(startIndex, match2);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Predicate<long>>(L, 4))
			{
				int startIndex2 = Lua.xlua_tointeger(L, 2);
				int count = Lua.xlua_tointeger(L, 3);
				Predicate<long> match3 = objectTranslator.GetDelegate<Predicate<long>>(L, 4);
				int value3 = list.FindLastIndex(startIndex2, count, match3);
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Collections.Generic.List<long>.FindLastIndex!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ForEach(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<long> list = (List<long>)objectTranslator.FastGetCSObj(L, 1);
			Action<long> action = objectTranslator.GetDelegate<Action<long>>(L, 2);
			list.ForEach(action);
			return 0;
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
			List<long>.Enumerator enumerator = ((List<long>)objectTranslator.FastGetCSObj(L, 1)).GetEnumerator();
			objectTranslator.Push(L, enumerator);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<long> obj = (List<long>)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			int count = Lua.xlua_tointeger(L, 3);
			List<long> range = obj.GetRange(index, count);
			objectTranslator.Push(L, range);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IndexOf(IntPtr L)
	{
		try
		{
			List<long> list = (List<long>)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)))
			{
				long item = Lua.lua_toint64(L, 2);
				int value = list.IndexOf(item);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 3 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				long item2 = Lua.lua_toint64(L, 2);
				int index = Lua.xlua_tointeger(L, 3);
				int value2 = list.IndexOf(item2, index);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 4 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				long item3 = Lua.lua_toint64(L, 2);
				int index2 = Lua.xlua_tointeger(L, 3);
				int count = Lua.xlua_tointeger(L, 4);
				int value3 = list.IndexOf(item3, index2, count);
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Collections.Generic.List<long>.IndexOf!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Insert(IntPtr L)
	{
		try
		{
			List<long> obj = (List<long>)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			long item = Lua.lua_toint64(L, 3);
			obj.Insert(index, item);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InsertRange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<long> list = (List<long>)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			IEnumerable<long> collection = (IEnumerable<long>)objectTranslator.GetObject(L, 3, typeof(IEnumerable<long>));
			list.InsertRange(index, collection);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LastIndexOf(IntPtr L)
	{
		try
		{
			List<long> list = (List<long>)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)))
			{
				long item = Lua.lua_toint64(L, 2);
				int value = list.LastIndexOf(item);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 3 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				long item2 = Lua.lua_toint64(L, 2);
				int index = Lua.xlua_tointeger(L, 3);
				int value2 = list.LastIndexOf(item2, index);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 4 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				long item3 = Lua.lua_toint64(L, 2);
				int index2 = Lua.xlua_tointeger(L, 3);
				int count = Lua.xlua_tointeger(L, 4);
				int value3 = list.LastIndexOf(item3, index2, count);
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Collections.Generic.List<long>.LastIndexOf!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Remove(IntPtr L)
	{
		try
		{
			List<long> obj = (List<long>)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long item = Lua.lua_toint64(L, 2);
			bool value = obj.Remove(item);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveAll(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<long> list = (List<long>)objectTranslator.FastGetCSObj(L, 1);
			Predicate<long> match = objectTranslator.GetDelegate<Predicate<long>>(L, 2);
			int value = list.RemoveAll(match);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveAt(IntPtr L)
	{
		try
		{
			List<long> obj = (List<long>)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			obj.RemoveAt(index);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveRange(IntPtr L)
	{
		try
		{
			List<long> obj = (List<long>)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			int count = Lua.xlua_tointeger(L, 3);
			obj.RemoveRange(index, count);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Reverse(IntPtr L)
	{
		try
		{
			List<long> list = (List<long>)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
				list.Reverse();
				return 0;
			case 3:
				if (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
				{
					int index = Lua.xlua_tointeger(L, 2);
					int count = Lua.xlua_tointeger(L, 3);
					list.Reverse(index, count);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Collections.Generic.List<long>.Reverse!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Sort(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<long> list = (List<long>)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			switch (num)
			{
			case 1:
				list.Sort();
				return 0;
			case 2:
				if (objectTranslator.Assignable<IComparer<long>>(L, 2))
				{
					IComparer<long> comparer = (IComparer<long>)objectTranslator.GetObject(L, 2, typeof(IComparer<long>));
					list.Sort(comparer);
					return 0;
				}
				break;
			}
			if (num == 2 && objectTranslator.Assignable<Comparison<long>>(L, 2))
			{
				Comparison<long> comparison = objectTranslator.GetDelegate<Comparison<long>>(L, 2);
				list.Sort(comparison);
				return 0;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<IComparer<long>>(L, 4))
			{
				int index = Lua.xlua_tointeger(L, 2);
				int count = Lua.xlua_tointeger(L, 3);
				IComparer<long> comparer2 = (IComparer<long>)objectTranslator.GetObject(L, 4, typeof(IComparer<long>));
				list.Sort(index, count, comparer2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Collections.Generic.List<long>.Sort!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToArray(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			long[] o = ((List<long>)objectTranslator.FastGetCSObj(L, 1)).ToArray();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TrimExcess(IntPtr L)
	{
		try
		{
			((List<long>)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).TrimExcess();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TrueForAll(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<long> list = (List<long>)objectTranslator.FastGetCSObj(L, 1);
			Predicate<long> match = objectTranslator.GetDelegate<Predicate<long>>(L, 2);
			bool value = list.TrueForAll(match);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Capacity(IntPtr L)
	{
		try
		{
			List<long> list = (List<long>)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, list.Capacity);
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
			List<long> list = (List<long>)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, list.Count);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Capacity(IntPtr L)
	{
		try
		{
			((List<long>)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Capacity = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
