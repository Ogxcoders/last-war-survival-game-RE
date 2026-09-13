using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ConfigCacheWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ConfigCache);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 4, 0, 0);
		Utils.RegisterFunc(L, -3, "GetTemplateData", _m_GetTemplateData);
		Utils.RegisterFunc(L, -3, "TryGetTemplateData", _m_TryGetTemplateData);
		Utils.RegisterFunc(L, -3, "UpdateTemplateData", _m_UpdateTemplateData);
		Utils.RegisterFunc(L, -3, "reset", _m_reset);
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
				ConfigCache o = new ConfigCache();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ConfigCache constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTemplateData(IntPtr L)
	{
		try
		{
			ConfigCache obj = (ConfigCache)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string tabName = Lua.lua_tostring(L, 2);
			int id = Lua.xlua_tointeger(L, 3);
			string colName = Lua.lua_tostring(L, 4);
			string templateData = obj.GetTemplateData(tabName, id, colName);
			Lua.lua_pushstring(L, templateData);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryGetTemplateData(IntPtr L)
	{
		try
		{
			ConfigCache obj = (ConfigCache)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string tabName = Lua.lua_tostring(L, 2);
			int id = Lua.xlua_tointeger(L, 3);
			string colName = Lua.lua_tostring(L, 4);
			string str = obj.TryGetTemplateData(tabName, id, colName);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateTemplateData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ConfigCache configCache = (ConfigCache)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TTABLE))
			{
				string tabName = Lua.lua_tostring(L, 2);
				int id = Lua.xlua_tointeger(L, 3);
				LuaTable rowData = (LuaTable)objectTranslator.GetObject(L, 4, typeof(LuaTable));
				configCache.UpdateTemplateData(tabName, id, rowData);
				return 0;
			}
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 5) || Lua.lua_type(L, 5) == LuaTypes.LUA_TSTRING))
			{
				string tabName2 = Lua.lua_tostring(L, 2);
				int id2 = Lua.xlua_tointeger(L, 3);
				string colName = Lua.lua_tostring(L, 4);
				string value = Lua.lua_tostring(L, 5);
				configCache.UpdateTemplateData(tabName2, id2, colName, value);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ConfigCache.UpdateTemplateData!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_reset(IntPtr L)
	{
		try
		{
			((ConfigCache)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).reset();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
