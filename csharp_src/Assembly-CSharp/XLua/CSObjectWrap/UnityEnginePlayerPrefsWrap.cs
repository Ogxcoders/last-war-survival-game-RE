using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEnginePlayerPrefsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(PlayerPrefs);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 11, 0, 0);
		Utils.RegisterFunc(L, -4, "SetInt", _m_SetInt_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetInt", _m_GetInt_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetFloat", _m_SetFloat_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetFloat", _m_GetFloat_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetString", _m_SetString_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetString", _m_GetString_xlua_st_);
		Utils.RegisterFunc(L, -4, "HasKey", _m_HasKey_xlua_st_);
		Utils.RegisterFunc(L, -4, "DeleteKey", _m_DeleteKey_xlua_st_);
		Utils.RegisterFunc(L, -4, "DeleteAll", _m_DeleteAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "Save", _m_Save_xlua_st_);
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
				PlayerPrefs o = new PlayerPrefs();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.PlayerPrefs constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetInt_xlua_st_(IntPtr L)
	{
		try
		{
			string key = Lua.lua_tostring(L, 1);
			int value = Lua.xlua_tointeger(L, 2);
			PlayerPrefs.SetInt(key, value);
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
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				int value = PlayerPrefs.GetInt(Lua.lua_tostring(L, 1));
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				string key = Lua.lua_tostring(L, 1);
				int defaultValue = Lua.xlua_tointeger(L, 2);
				int value2 = PlayerPrefs.GetInt(key, defaultValue);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.PlayerPrefs.GetInt!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFloat_xlua_st_(IntPtr L)
	{
		try
		{
			string key = Lua.lua_tostring(L, 1);
			float value = (float)Lua.lua_tonumber(L, 2);
			PlayerPrefs.SetFloat(key, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFloat_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				float num2 = PlayerPrefs.GetFloat(Lua.lua_tostring(L, 1));
				Lua.lua_pushnumber(L, num2);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				string key = Lua.lua_tostring(L, 1);
				float defaultValue = (float)Lua.lua_tonumber(L, 2);
				float num3 = PlayerPrefs.GetFloat(key, defaultValue);
				Lua.lua_pushnumber(L, num3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.PlayerPrefs.GetFloat!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetString_xlua_st_(IntPtr L)
	{
		try
		{
			string key = Lua.lua_tostring(L, 1);
			string value = Lua.lua_tostring(L, 2);
			PlayerPrefs.SetString(key, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetString_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				string str = PlayerPrefs.GetString(Lua.lua_tostring(L, 1));
				Lua.lua_pushstring(L, str);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string key = Lua.lua_tostring(L, 1);
				string defaultValue = Lua.lua_tostring(L, 2);
				string str2 = PlayerPrefs.GetString(key, defaultValue);
				Lua.lua_pushstring(L, str2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.PlayerPrefs.GetString!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HasKey_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = PlayerPrefs.HasKey(Lua.lua_tostring(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DeleteKey_xlua_st_(IntPtr L)
	{
		try
		{
			PlayerPrefs.DeleteKey(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DeleteAll_xlua_st_(IntPtr L)
	{
		try
		{
			PlayerPrefs.DeleteAll();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Save_xlua_st_(IntPtr L)
	{
		try
		{
			PlayerPrefs.Save();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
