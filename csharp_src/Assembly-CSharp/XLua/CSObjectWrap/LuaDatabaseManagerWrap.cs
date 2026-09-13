using System;
using System.Collections.Generic;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LuaDatabaseManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(LuaDatabaseManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 7, 0, 0);
		Utils.RegisterFunc(L, -4, "InitDataBase", _m_InitDataBase_xlua_st_);
		Utils.RegisterFunc(L, -4, "UninitDatabase", _m_UninitDatabase_xlua_st_);
		Utils.RegisterFunc(L, -4, "ExecuteMultiSQL", _m_ExecuteMultiSQL_xlua_st_);
		Utils.RegisterFunc(L, -4, "ExecuteSTMT", _m_ExecuteSTMT_xlua_st_);
		Utils.RegisterFunc(L, -4, "ExecuteUrgentSQL", _m_ExecuteUrgentSQL_xlua_st_);
		Utils.RegisterFunc(L, -4, "ExecuteSQL", _m_ExecuteSQL_xlua_st_);
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
				LuaDatabaseManager o = new LuaDatabaseManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LuaDatabaseManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitDataBase_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string dbFileName = Lua.lua_tostring(L, 1);
			Action<bool> callback = objectTranslator.GetDelegate<Action<bool>>(L, 2);
			LuaDatabaseManager.InitDataBase(dbFileName, callback);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UninitDatabase_xlua_st_(IntPtr L)
	{
		try
		{
			LuaDatabaseManager.UninitDatabase();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ExecuteMultiSQL_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<List<string>>(L, 1) && objectTranslator.Assignable<Action<LuaTable>>(L, 2))
			{
				List<string> cmdStr = (List<string>)objectTranslator.GetObject(L, 1, typeof(List<string>));
				Action<LuaTable> callback = objectTranslator.GetDelegate<Action<LuaTable>>(L, 2);
				LuaDatabaseManager.ExecuteMultiSQL(cmdStr, callback);
				return 0;
			}
			if (num == 1 && objectTranslator.Assignable<List<string>>(L, 1))
			{
				LuaDatabaseManager.ExecuteMultiSQL((List<string>)objectTranslator.GetObject(L, 1, typeof(List<string>)));
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LuaDatabaseManager.ExecuteMultiSQL!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ExecuteSTMT_xlua_st_(IntPtr L)
	{
		try
		{
			LuaDatabaseManager.ExecuteSTMT();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ExecuteUrgentSQL_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action<LuaTable>>(L, 2))
			{
				string cmdStr = Lua.lua_tostring(L, 1);
				Action<LuaTable> callback = objectTranslator.GetDelegate<Action<LuaTable>>(L, 2);
				LuaDatabaseManager.ExecuteUrgentSQL(cmdStr, callback);
				return 0;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				LuaDatabaseManager.ExecuteUrgentSQL(Lua.lua_tostring(L, 1));
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LuaDatabaseManager.ExecuteUrgentSQL!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ExecuteSQL_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action<LuaTable>>(L, 2))
			{
				string cmdStr = Lua.lua_tostring(L, 1);
				Action<LuaTable> callback = objectTranslator.GetDelegate<Action<LuaTable>>(L, 2);
				LuaDatabaseManager.ExecuteSQL(cmdStr, callback);
				return 0;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				LuaDatabaseManager.ExecuteSQL(Lua.lua_tostring(L, 1));
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LuaDatabaseManager.ExecuteSQL!");
	}
}
