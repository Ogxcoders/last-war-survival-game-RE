using System;
using System.Collections;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SystemArrayWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Array);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 10, 7, 0);
		Utils.RegisterFunc(L, -3, "CopyTo", _m_CopyTo);
		Utils.RegisterFunc(L, -3, "Clone", _m_Clone);
		Utils.RegisterFunc(L, -3, "GetLongLength", _m_GetLongLength);
		Utils.RegisterFunc(L, -3, "GetValue", _m_GetValue);
		Utils.RegisterFunc(L, -3, "SetValue", _m_SetValue);
		Utils.RegisterFunc(L, -3, "GetEnumerator", _m_GetEnumerator);
		Utils.RegisterFunc(L, -3, "GetLength", _m_GetLength);
		Utils.RegisterFunc(L, -3, "GetLowerBound", _m_GetLowerBound);
		Utils.RegisterFunc(L, -3, "GetUpperBound", _m_GetUpperBound);
		Utils.RegisterFunc(L, -3, "Initialize", _m_Initialize);
		Utils.RegisterFunc(L, -2, "LongLength", _g_get_LongLength);
		Utils.RegisterFunc(L, -2, "IsFixedSize", _g_get_IsFixedSize);
		Utils.RegisterFunc(L, -2, "IsReadOnly", _g_get_IsReadOnly);
		Utils.RegisterFunc(L, -2, "IsSynchronized", _g_get_IsSynchronized);
		Utils.RegisterFunc(L, -2, "SyncRoot", _g_get_SyncRoot);
		Utils.RegisterFunc(L, -2, "Length", _g_get_Length);
		Utils.RegisterFunc(L, -2, "Rank", _g_get_Rank);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 10, 0, 0);
		Utils.RegisterFunc(L, -4, "CreateInstance", _m_CreateInstance_xlua_st_);
		Utils.RegisterFunc(L, -4, "BinarySearch", _m_BinarySearch_xlua_st_);
		Utils.RegisterFunc(L, -4, "Copy", _m_Copy_xlua_st_);
		Utils.RegisterFunc(L, -4, "IndexOf", _m_IndexOf_xlua_st_);
		Utils.RegisterFunc(L, -4, "LastIndexOf", _m_LastIndexOf_xlua_st_);
		Utils.RegisterFunc(L, -4, "Reverse", _m_Reverse_xlua_st_);
		Utils.RegisterFunc(L, -4, "Sort", _m_Sort_xlua_st_);
		Utils.RegisterFunc(L, -4, "Clear", _m_Clear_xlua_st_);
		Utils.RegisterFunc(L, -4, "ConstrainedCopy", _m_ConstrainedCopy_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "System.Array does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateInstance_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Type>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				Type elementType = (Type)objectTranslator.GetObject(L, 1, typeof(Type));
				int length = Lua.xlua_tointeger(L, 2);
				Array o = Array.CreateInstance(elementType, length);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Type>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				Type elementType2 = (Type)objectTranslator.GetObject(L, 1, typeof(Type));
				int length2 = Lua.xlua_tointeger(L, 2);
				int length3 = Lua.xlua_tointeger(L, 3);
				Array o2 = Array.CreateInstance(elementType2, length2, length3);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Type>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				Type elementType3 = (Type)objectTranslator.GetObject(L, 1, typeof(Type));
				int length4 = Lua.xlua_tointeger(L, 2);
				int length5 = Lua.xlua_tointeger(L, 3);
				int length6 = Lua.xlua_tointeger(L, 4);
				Array o3 = Array.CreateInstance(elementType3, length4, length5, length6);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num >= 1 && objectTranslator.Assignable<Type>(L, 1) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 2) || LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)))
			{
				Type elementType4 = (Type)objectTranslator.GetObject(L, 1, typeof(Type));
				long[] lengths = objectTranslator.GetParams<long>(L, 2);
				Array o4 = Array.CreateInstance(elementType4, lengths);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num >= 1 && objectTranslator.Assignable<Type>(L, 1) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 2) || LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2)))
			{
				Type elementType5 = (Type)objectTranslator.GetObject(L, 1, typeof(Type));
				int[] lengths2 = objectTranslator.GetParams<int>(L, 2);
				Array o5 = Array.CreateInstance(elementType5, lengths2);
				objectTranslator.Push(L, o5);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Type>(L, 1) && objectTranslator.Assignable<int[]>(L, 2) && objectTranslator.Assignable<int[]>(L, 3))
			{
				Type elementType6 = (Type)objectTranslator.GetObject(L, 1, typeof(Type));
				int[] lengths3 = (int[])objectTranslator.GetObject(L, 2, typeof(int[]));
				int[] lowerBounds = (int[])objectTranslator.GetObject(L, 3, typeof(int[]));
				Array o6 = Array.CreateInstance(elementType6, lengths3, lowerBounds);
				objectTranslator.Push(L, o6);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Array.CreateInstance!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CopyTo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Array array = (Array)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Array>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				Array array2 = (Array)objectTranslator.GetObject(L, 2, typeof(Array));
				int index = Lua.xlua_tointeger(L, 3);
				array.CopyTo(array2, index);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Array>(L, 2) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)))
			{
				Array array3 = (Array)objectTranslator.GetObject(L, 2, typeof(Array));
				long index2 = Lua.lua_toint64(L, 3);
				array.CopyTo(array3, index2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Array.CopyTo!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clone(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			object o = ((Array)objectTranslator.FastGetCSObj(L, 1)).Clone();
			objectTranslator.PushAny(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BinarySearch_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Array>(L, 1) && objectTranslator.Assignable<object>(L, 2))
			{
				Array array = (Array)objectTranslator.GetObject(L, 1, typeof(Array));
				object value = objectTranslator.GetObject(L, 2, typeof(object));
				int value2 = Array.BinarySearch(array, value);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Array>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<object>(L, 4))
			{
				Array array2 = (Array)objectTranslator.GetObject(L, 1, typeof(Array));
				int index = Lua.xlua_tointeger(L, 2);
				int length = Lua.xlua_tointeger(L, 3);
				object value3 = objectTranslator.GetObject(L, 4, typeof(object));
				int value4 = Array.BinarySearch(array2, index, length, value3);
				Lua.xlua_pushinteger(L, value4);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Array>(L, 1) && objectTranslator.Assignable<object>(L, 2) && objectTranslator.Assignable<IComparer>(L, 3))
			{
				Array array3 = (Array)objectTranslator.GetObject(L, 1, typeof(Array));
				object value5 = objectTranslator.GetObject(L, 2, typeof(object));
				IComparer comparer = (IComparer)objectTranslator.GetObject(L, 3, typeof(IComparer));
				int value6 = Array.BinarySearch(array3, value5, comparer);
				Lua.xlua_pushinteger(L, value6);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Array>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<object>(L, 4) && objectTranslator.Assignable<IComparer>(L, 5))
			{
				Array array4 = (Array)objectTranslator.GetObject(L, 1, typeof(Array));
				int index2 = Lua.xlua_tointeger(L, 2);
				int length2 = Lua.xlua_tointeger(L, 3);
				object value7 = objectTranslator.GetObject(L, 4, typeof(object));
				IComparer comparer2 = (IComparer)objectTranslator.GetObject(L, 5, typeof(IComparer));
				int value8 = Array.BinarySearch(array4, index2, length2, value7, comparer2);
				Lua.xlua_pushinteger(L, value8);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Array.BinarySearch!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Copy_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Array>(L, 1) && objectTranslator.Assignable<Array>(L, 2) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)))
			{
				Array sourceArray = (Array)objectTranslator.GetObject(L, 1, typeof(Array));
				Array destinationArray = (Array)objectTranslator.GetObject(L, 2, typeof(Array));
				long length = Lua.lua_toint64(L, 3);
				Array.Copy(sourceArray, destinationArray, length);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Array>(L, 1) && objectTranslator.Assignable<Array>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				Array sourceArray2 = (Array)objectTranslator.GetObject(L, 1, typeof(Array));
				Array destinationArray2 = (Array)objectTranslator.GetObject(L, 2, typeof(Array));
				int length2 = Lua.xlua_tointeger(L, 3);
				Array.Copy(sourceArray2, destinationArray2, length2);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<Array>(L, 1) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && objectTranslator.Assignable<Array>(L, 3) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) || Lua.lua_isint64(L, 4)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) || Lua.lua_isint64(L, 5)))
			{
				Array sourceArray3 = (Array)objectTranslator.GetObject(L, 1, typeof(Array));
				long sourceIndex = Lua.lua_toint64(L, 2);
				Array destinationArray3 = (Array)objectTranslator.GetObject(L, 3, typeof(Array));
				long destinationIndex = Lua.lua_toint64(L, 4);
				long length3 = Lua.lua_toint64(L, 5);
				Array.Copy(sourceArray3, sourceIndex, destinationArray3, destinationIndex, length3);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<Array>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Array>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				Array sourceArray4 = (Array)objectTranslator.GetObject(L, 1, typeof(Array));
				int sourceIndex2 = Lua.xlua_tointeger(L, 2);
				Array destinationArray4 = (Array)objectTranslator.GetObject(L, 3, typeof(Array));
				int destinationIndex2 = Lua.xlua_tointeger(L, 4);
				int length4 = Lua.xlua_tointeger(L, 5);
				Array.Copy(sourceArray4, sourceIndex2, destinationArray4, destinationIndex2, length4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Array.Copy!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLongLength(IntPtr L)
	{
		try
		{
			Array obj = (Array)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int dimension = Lua.xlua_tointeger(L, 2);
			long longLength = obj.GetLongLength(dimension);
			Lua.lua_pushint64(L, longLength);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetValue(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Array array = (Array)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)))
			{
				long index = Lua.lua_toint64(L, 2);
				object value = array.GetValue(index);
				objectTranslator.PushAny(L, value);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int index2 = Lua.xlua_tointeger(L, 2);
				object value2 = array.GetValue(index2);
				objectTranslator.PushAny(L, value2);
				return 1;
			}
			if (num == 3 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)))
			{
				long index3 = Lua.lua_toint64(L, 2);
				long index4 = Lua.lua_toint64(L, 3);
				object value3 = array.GetValue(index3, index4);
				objectTranslator.PushAny(L, value3);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int index5 = Lua.xlua_tointeger(L, 2);
				int index6 = Lua.xlua_tointeger(L, 3);
				object value4 = array.GetValue(index5, index6);
				objectTranslator.PushAny(L, value4);
				return 1;
			}
			if (num == 4 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) || Lua.lua_isint64(L, 4)))
			{
				long index7 = Lua.lua_toint64(L, 2);
				long index8 = Lua.lua_toint64(L, 3);
				long index9 = Lua.lua_toint64(L, 4);
				object value5 = array.GetValue(index7, index8, index9);
				objectTranslator.PushAny(L, value5);
				return 1;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				int index10 = Lua.xlua_tointeger(L, 2);
				int index11 = Lua.xlua_tointeger(L, 3);
				int index12 = Lua.xlua_tointeger(L, 4);
				object value6 = array.GetValue(index10, index11, index12);
				objectTranslator.PushAny(L, value6);
				return 1;
			}
			if (num >= 1 && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 2) || LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)))
			{
				long[] indices = objectTranslator.GetParams<long>(L, 2);
				object value7 = array.GetValue(indices);
				objectTranslator.PushAny(L, value7);
				return 1;
			}
			if (num >= 1 && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 2) || LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2)))
			{
				int[] indices2 = objectTranslator.GetParams<int>(L, 2);
				object value8 = array.GetValue(indices2);
				objectTranslator.PushAny(L, value8);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Array.GetValue!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IndexOf_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Array>(L, 1) && objectTranslator.Assignable<object>(L, 2))
			{
				Array array = (Array)objectTranslator.GetObject(L, 1, typeof(Array));
				object value = objectTranslator.GetObject(L, 2, typeof(object));
				int value2 = Array.IndexOf(array, value);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Array>(L, 1) && objectTranslator.Assignable<object>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				Array array2 = (Array)objectTranslator.GetObject(L, 1, typeof(Array));
				object value3 = objectTranslator.GetObject(L, 2, typeof(object));
				int startIndex = Lua.xlua_tointeger(L, 3);
				int value4 = Array.IndexOf(array2, value3, startIndex);
				Lua.xlua_pushinteger(L, value4);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Array>(L, 1) && objectTranslator.Assignable<object>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				Array array3 = (Array)objectTranslator.GetObject(L, 1, typeof(Array));
				object value5 = objectTranslator.GetObject(L, 2, typeof(object));
				int startIndex2 = Lua.xlua_tointeger(L, 3);
				int count = Lua.xlua_tointeger(L, 4);
				int value6 = Array.IndexOf(array3, value5, startIndex2, count);
				Lua.xlua_pushinteger(L, value6);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Array.IndexOf!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LastIndexOf_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Array>(L, 1) && objectTranslator.Assignable<object>(L, 2))
			{
				Array array = (Array)objectTranslator.GetObject(L, 1, typeof(Array));
				object value = objectTranslator.GetObject(L, 2, typeof(object));
				int value2 = Array.LastIndexOf(array, value);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Array>(L, 1) && objectTranslator.Assignable<object>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				Array array2 = (Array)objectTranslator.GetObject(L, 1, typeof(Array));
				object value3 = objectTranslator.GetObject(L, 2, typeof(object));
				int startIndex = Lua.xlua_tointeger(L, 3);
				int value4 = Array.LastIndexOf(array2, value3, startIndex);
				Lua.xlua_pushinteger(L, value4);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Array>(L, 1) && objectTranslator.Assignable<object>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				Array array3 = (Array)objectTranslator.GetObject(L, 1, typeof(Array));
				object value5 = objectTranslator.GetObject(L, 2, typeof(object));
				int startIndex2 = Lua.xlua_tointeger(L, 3);
				int count = Lua.xlua_tointeger(L, 4);
				int value6 = Array.LastIndexOf(array3, value5, startIndex2, count);
				Lua.xlua_pushinteger(L, value6);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Array.LastIndexOf!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Reverse_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<Array>(L, 1))
			{
				Array.Reverse((Array)objectTranslator.GetObject(L, 1, typeof(Array)));
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Array>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				Array array = (Array)objectTranslator.GetObject(L, 1, typeof(Array));
				int index = Lua.xlua_tointeger(L, 2);
				int length = Lua.xlua_tointeger(L, 3);
				Array.Reverse(array, index, length);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Array.Reverse!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetValue(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Array array = (Array)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<object>(L, 2) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)))
			{
				object value = objectTranslator.GetObject(L, 2, typeof(object));
				long index = Lua.lua_toint64(L, 3);
				array.SetValue(value, index);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<object>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				object value2 = objectTranslator.GetObject(L, 2, typeof(object));
				int index2 = Lua.xlua_tointeger(L, 3);
				array.SetValue(value2, index2);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<object>(L, 2) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) || Lua.lua_isint64(L, 4)))
			{
				object value3 = objectTranslator.GetObject(L, 2, typeof(object));
				long index3 = Lua.lua_toint64(L, 3);
				long index4 = Lua.lua_toint64(L, 4);
				array.SetValue(value3, index3, index4);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<object>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				object value4 = objectTranslator.GetObject(L, 2, typeof(object));
				int index5 = Lua.xlua_tointeger(L, 3);
				int index6 = Lua.xlua_tointeger(L, 4);
				array.SetValue(value4, index5, index6);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<object>(L, 2) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) || Lua.lua_isint64(L, 4)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) || Lua.lua_isint64(L, 5)))
			{
				object value5 = objectTranslator.GetObject(L, 2, typeof(object));
				long index7 = Lua.lua_toint64(L, 3);
				long index8 = Lua.lua_toint64(L, 4);
				long index9 = Lua.lua_toint64(L, 5);
				array.SetValue(value5, index7, index8, index9);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<object>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				object value6 = objectTranslator.GetObject(L, 2, typeof(object));
				int index10 = Lua.xlua_tointeger(L, 3);
				int index11 = Lua.xlua_tointeger(L, 4);
				int index12 = Lua.xlua_tointeger(L, 5);
				array.SetValue(value6, index10, index11, index12);
				return 0;
			}
			if (num >= 2 && objectTranslator.Assignable<object>(L, 2) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 3) || LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)))
			{
				object value7 = objectTranslator.GetObject(L, 2, typeof(object));
				long[] indices = objectTranslator.GetParams<long>(L, 3);
				array.SetValue(value7, indices);
				return 0;
			}
			if (num >= 2 && objectTranslator.Assignable<object>(L, 2) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 3) || LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3)))
			{
				object value8 = objectTranslator.GetObject(L, 2, typeof(object));
				int[] indices2 = objectTranslator.GetParams<int>(L, 3);
				array.SetValue(value8, indices2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Array.SetValue!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Sort_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<Array>(L, 1))
			{
				Array.Sort((Array)objectTranslator.GetObject(L, 1, typeof(Array)));
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Array>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				Array array = (Array)objectTranslator.GetObject(L, 1, typeof(Array));
				int index = Lua.xlua_tointeger(L, 2);
				int length = Lua.xlua_tointeger(L, 3);
				Array.Sort(array, index, length);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Array>(L, 1) && objectTranslator.Assignable<IComparer>(L, 2))
			{
				Array array2 = (Array)objectTranslator.GetObject(L, 1, typeof(Array));
				IComparer comparer = (IComparer)objectTranslator.GetObject(L, 2, typeof(IComparer));
				Array.Sort(array2, comparer);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Array>(L, 1) && objectTranslator.Assignable<Array>(L, 2))
			{
				Array keys = (Array)objectTranslator.GetObject(L, 1, typeof(Array));
				Array items = (Array)objectTranslator.GetObject(L, 2, typeof(Array));
				Array.Sort(keys, items);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Array>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<IComparer>(L, 4))
			{
				Array array3 = (Array)objectTranslator.GetObject(L, 1, typeof(Array));
				int index2 = Lua.xlua_tointeger(L, 2);
				int length2 = Lua.xlua_tointeger(L, 3);
				IComparer comparer2 = (IComparer)objectTranslator.GetObject(L, 4, typeof(IComparer));
				Array.Sort(array3, index2, length2, comparer2);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Array>(L, 1) && objectTranslator.Assignable<Array>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				Array keys2 = (Array)objectTranslator.GetObject(L, 1, typeof(Array));
				Array items2 = (Array)objectTranslator.GetObject(L, 2, typeof(Array));
				int index3 = Lua.xlua_tointeger(L, 3);
				int length3 = Lua.xlua_tointeger(L, 4);
				Array.Sort(keys2, items2, index3, length3);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Array>(L, 1) && objectTranslator.Assignable<Array>(L, 2) && objectTranslator.Assignable<IComparer>(L, 3))
			{
				Array keys3 = (Array)objectTranslator.GetObject(L, 1, typeof(Array));
				Array items3 = (Array)objectTranslator.GetObject(L, 2, typeof(Array));
				IComparer comparer3 = (IComparer)objectTranslator.GetObject(L, 3, typeof(IComparer));
				Array.Sort(keys3, items3, comparer3);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<Array>(L, 1) && objectTranslator.Assignable<Array>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<IComparer>(L, 5))
			{
				Array keys4 = (Array)objectTranslator.GetObject(L, 1, typeof(Array));
				Array items4 = (Array)objectTranslator.GetObject(L, 2, typeof(Array));
				int index4 = Lua.xlua_tointeger(L, 3);
				int length4 = Lua.xlua_tointeger(L, 4);
				IComparer comparer4 = (IComparer)objectTranslator.GetObject(L, 5, typeof(IComparer));
				Array.Sort(keys4, items4, index4, length4, comparer4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Array.Sort!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetEnumerator(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			IEnumerator enumerator = ((Array)objectTranslator.FastGetCSObj(L, 1)).GetEnumerator();
			objectTranslator.PushAny(L, enumerator);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLength(IntPtr L)
	{
		try
		{
			Array obj = (Array)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int dimension = Lua.xlua_tointeger(L, 2);
			int length = obj.GetLength(dimension);
			Lua.xlua_pushinteger(L, length);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLowerBound(IntPtr L)
	{
		try
		{
			Array obj = (Array)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int dimension = Lua.xlua_tointeger(L, 2);
			int lowerBound = obj.GetLowerBound(dimension);
			Lua.xlua_pushinteger(L, lowerBound);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetUpperBound(IntPtr L)
	{
		try
		{
			Array obj = (Array)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int dimension = Lua.xlua_tointeger(L, 2);
			int upperBound = obj.GetUpperBound(dimension);
			Lua.xlua_pushinteger(L, upperBound);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clear_xlua_st_(IntPtr L)
	{
		try
		{
			Array array = (Array)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Array));
			int index = Lua.xlua_tointeger(L, 2);
			int length = Lua.xlua_tointeger(L, 3);
			Array.Clear(array, index, length);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ConstrainedCopy_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Array sourceArray = (Array)objectTranslator.GetObject(L, 1, typeof(Array));
			int sourceIndex = Lua.xlua_tointeger(L, 2);
			Array destinationArray = (Array)objectTranslator.GetObject(L, 3, typeof(Array));
			int destinationIndex = Lua.xlua_tointeger(L, 4);
			int length = Lua.xlua_tointeger(L, 5);
			Array.ConstrainedCopy(sourceArray, sourceIndex, destinationArray, destinationIndex, length);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Initialize(IntPtr L)
	{
		try
		{
			((Array)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Initialize();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LongLength(IntPtr L)
	{
		try
		{
			Array array = (Array)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, array.LongLength);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsFixedSize(IntPtr L)
	{
		try
		{
			Array array = (Array)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, array.IsFixedSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsReadOnly(IntPtr L)
	{
		try
		{
			Array array = (Array)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, array.IsReadOnly);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsSynchronized(IntPtr L)
	{
		try
		{
			Array array = (Array)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, array.IsSynchronized);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SyncRoot(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Array array = (Array)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushAny(L, array.SyncRoot);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Length(IntPtr L)
	{
		try
		{
			Array array = (Array)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, array.Length);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Rank(IntPtr L)
	{
		try
		{
			Array array = (Array)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, array.Rank);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
