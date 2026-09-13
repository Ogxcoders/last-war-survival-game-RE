using System;
using Data.Csv;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DataCsvRowWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Row);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 10, 2, 0);
		Utils.RegisterFunc(L, -3, "HasKey", _m_HasKey);
		Utils.RegisterFunc(L, -3, "GetData", _m_GetData);
		Utils.RegisterFunc(L, -3, "GetString", _m_GetString);
		Utils.RegisterFunc(L, -3, "GetInt", _m_GetInt);
		Utils.RegisterFunc(L, -3, "TryGetInt", _m_TryGetInt);
		Utils.RegisterFunc(L, -3, "GetFloat", _m_GetFloat);
		Utils.RegisterFunc(L, -3, "get_Item", _m_get_Item);
		Utils.RegisterFunc(L, -3, "ToString", _m_ToString);
		Utils.RegisterFunc(L, -3, "GetStringArray", _m_GetStringArray);
		Utils.RegisterFunc(L, -3, "GetLong", _m_GetLong);
		Utils.RegisterFunc(L, -2, "Count", _g_get_Count);
		Utils.RegisterFunc(L, -2, "_csv", _g_get__csv);
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
			if (Lua.lua_gettop(L) == 3 && objectTranslator.Assignable<CSVCacheder>(L, 2) && objectTranslator.Assignable<string[]>(L, 3))
			{
				CSVCacheder csv = (CSVCacheder)objectTranslator.GetObject(L, 2, typeof(CSVCacheder));
				string[] value = (string[])objectTranslator.GetObject(L, 3, typeof(string[]));
				Row o = new Row(csv, value);
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Data.Csv.Row constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HasKey(IntPtr L)
	{
		try
		{
			Row obj = (Row)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			bool value = obj.HasKey(key);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetData(IntPtr L)
	{
		try
		{
			Row row = (Row)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				string key = Lua.lua_tostring(L, 2);
				bool log = Lua.lua_toboolean(L, 3);
				string data = row.GetData(key, log);
				Lua.lua_pushstring(L, data);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string key2 = Lua.lua_tostring(L, 2);
				string data2 = row.GetData(key2);
				Lua.lua_pushstring(L, data2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Data.Csv.Row.GetData!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetString(IntPtr L)
	{
		try
		{
			Row obj = (Row)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			string str = obj.GetString(key);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetInt(IntPtr L)
	{
		try
		{
			Row row = (Row)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string key = Lua.lua_tostring(L, 2);
				int defalut = Lua.xlua_tointeger(L, 3);
				int value = row.GetInt(key, defalut);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string key2 = Lua.lua_tostring(L, 2);
				int value2 = row.GetInt(key2);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Data.Csv.Row.GetInt!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryGetInt(IntPtr L)
	{
		try
		{
			Row row = (Row)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string key = Lua.lua_tostring(L, 2);
				int defalut = Lua.xlua_tointeger(L, 3);
				int value = row.TryGetInt(key, defalut);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string key2 = Lua.lua_tostring(L, 2);
				int value2 = row.TryGetInt(key2);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Data.Csv.Row.TryGetInt!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFloat(IntPtr L)
	{
		try
		{
			Row obj = (Row)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			float num = obj.GetFloat(key);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_get_Item(IntPtr L)
	{
		try
		{
			Row row = (Row)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			Lua.lua_pushstring(L, row[key]);
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
			string str = ((Row)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ToString();
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetStringArray(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Row row = (Row)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				string key = Lua.lua_tostring(L, 2);
				char spliter = (char)Lua.xlua_tointeger(L, 3);
				bool copy = Lua.lua_toboolean(L, 4);
				string[] stringArray = row.GetStringArray(key, spliter, copy);
				objectTranslator.Push(L, stringArray);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string key2 = Lua.lua_tostring(L, 2);
				char spliter2 = (char)Lua.xlua_tointeger(L, 3);
				string[] stringArray2 = row.GetStringArray(key2, spliter2);
				objectTranslator.Push(L, stringArray2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Data.Csv.Row.GetStringArray!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLong(IntPtr L)
	{
		try
		{
			Row obj = (Row)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			long n = obj.GetLong(key);
			Lua.lua_pushint64(L, n);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Count(IntPtr L)
	{
		try
		{
			Row row = (Row)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, row.Count);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get__csv(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Row row = (Row)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, row._csv);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
