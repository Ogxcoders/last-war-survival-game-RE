using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GMSwitchWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GMSwitch);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 12, 4, 1);
		Utils.RegisterFunc(L, -4, "Init", _m_Init_xlua_st_);
		Utils.RegisterFunc(L, -4, "Dispose", _m_Dispose_xlua_st_);
		Utils.RegisterFunc(L, -4, "Reload", _m_Reload_xlua_st_);
		Utils.RegisterFunc(L, -4, "Update", _m_Update_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearDebugLog", _m_ClearDebugLog_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetBool", _m_GetBool_xlua_st_);
		Utils.RegisterFunc(L, -4, "LuaSetBool", _m_LuaSetBool_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetInt", _m_GetInt_xlua_st_);
		Utils.RegisterFunc(L, -4, "LuaSetInt", _m_LuaSetInt_xlua_st_);
		Utils.RegisterFunc(L, -4, "LuaSetObject", _m_LuaSetObject_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetObject", _m_GetObject_xlua_st_);
		Utils.RegisterFunc(L, -2, "FocusMyClick", _g_get_FocusMyClick);
		Utils.RegisterFunc(L, -2, "DebugLogQueue", _g_get_DebugLogQueue);
		Utils.RegisterFunc(L, -2, "IsGM", _g_get_IsGM);
		Utils.RegisterFunc(L, -2, "DebugClickLogWarning", _g_get_DebugClickLogWarning);
		Utils.RegisterFunc(L, -1, "IsGM", _s_set_IsGM);
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
				GMSwitch o = new GMSwitch();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GMSwitch constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init_xlua_st_(IntPtr L)
	{
		try
		{
			GMSwitch.Init();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Dispose_xlua_st_(IntPtr L)
	{
		try
		{
			GMSwitch.Dispose();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Reload_xlua_st_(IntPtr L)
	{
		try
		{
			GMSwitch.Reload();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Update_xlua_st_(IntPtr L)
	{
		try
		{
			GMSwitch.Update((float)Lua.lua_tonumber(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearDebugLog_xlua_st_(IntPtr L)
	{
		try
		{
			GMSwitch.ClearDebugLog();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBool_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				string key = Lua.lua_tostring(L, 1);
				bool defaultVal = Lua.lua_toboolean(L, 2);
				bool value = GMSwitch.GetBool(key, defaultVal);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				bool value2 = GMSwitch.GetBool(Lua.lua_tostring(L, 1));
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GMSwitch.GetBool!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LuaSetBool_xlua_st_(IntPtr L)
	{
		try
		{
			string key = Lua.lua_tostring(L, 1);
			bool value = Lua.lua_toboolean(L, 2);
			GMSwitch.LuaSetBool(key, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetInt_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				string key = Lua.lua_tostring(L, 1);
				int defaultVal = Lua.xlua_tointeger(L, 2);
				int value = GMSwitch.GetInt(key, defaultVal);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				int value2 = GMSwitch.GetInt(Lua.lua_tostring(L, 1));
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GMSwitch.GetInt!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LuaSetInt_xlua_st_(IntPtr L)
	{
		try
		{
			string key = Lua.lua_tostring(L, 1);
			int value = Lua.xlua_tointeger(L, 2);
			GMSwitch.LuaSetInt(key, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LuaSetObject_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string key = Lua.lua_tostring(L, 1);
			object value = objectTranslator.GetObject(L, 2, typeof(object));
			GMSwitch.LuaSetObject(key, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetObject_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			object o = GMSwitch.GetObject(Lua.lua_tostring(L, 1));
			objectTranslator.PushAny(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_FocusMyClick(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, GMSwitch.FocusMyClick);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DebugLogQueue(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GMSwitch.DebugLogQueue);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsGM(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, GMSwitch.IsGM);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DebugClickLogWarning(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, GMSwitch.DebugClickLogWarning);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsGM(IntPtr L)
	{
		try
		{
			GMSwitch.IsGM = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
