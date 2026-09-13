using System;
using GameFramework.Localization;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SettingManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SettingManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 42, 5, 5);
		Utils.RegisterFunc(L, -3, "Load", _m_Load);
		Utils.RegisterFunc(L, -3, "Save", _m_Save);
		Utils.RegisterFunc(L, -3, "HasSetting", _m_HasSetting);
		Utils.RegisterFunc(L, -3, "RemoveSetting", _m_RemoveSetting);
		Utils.RegisterFunc(L, -3, "RemoveAllSettings", _m_RemoveAllSettings);
		Utils.RegisterFunc(L, -3, "GetBool", _m_GetBool);
		Utils.RegisterFunc(L, -3, "SetBool", _m_SetBool);
		Utils.RegisterFunc(L, -3, "GetInt", _m_GetInt);
		Utils.RegisterFunc(L, -3, "SetInt", _m_SetInt);
		Utils.RegisterFunc(L, -3, "GetFloat", _m_GetFloat);
		Utils.RegisterFunc(L, -3, "SetFloat", _m_SetFloat);
		Utils.RegisterFunc(L, -3, "GetString", _m_GetString);
		Utils.RegisterFunc(L, -3, "SetString", _m_SetString);
		Utils.RegisterFunc(L, -3, "GetPublicBool", _m_GetPublicBool);
		Utils.RegisterFunc(L, -3, "GetPrivateBool", _m_GetPrivateBool);
		Utils.RegisterFunc(L, -3, "SetPublicBool", _m_SetPublicBool);
		Utils.RegisterFunc(L, -3, "SetPrivateBool", _m_SetPrivateBool);
		Utils.RegisterFunc(L, -3, "GetPublicInt", _m_GetPublicInt);
		Utils.RegisterFunc(L, -3, "SetPublicInt", _m_SetPublicInt);
		Utils.RegisterFunc(L, -3, "SetPrivateInt", _m_SetPrivateInt);
		Utils.RegisterFunc(L, -3, "GetPrivateInt", _m_GetPrivateInt);
		Utils.RegisterFunc(L, -3, "SetPrivateFloat", _m_SetPrivateFloat);
		Utils.RegisterFunc(L, -3, "GetPrivateFloat", _m_GetPrivateFloat);
		Utils.RegisterFunc(L, -3, "GetPublicFloat", _m_GetPublicFloat);
		Utils.RegisterFunc(L, -3, "SetPublicFloat", _m_SetPublicFloat);
		Utils.RegisterFunc(L, -3, "GetPublicString", _m_GetPublicString);
		Utils.RegisterFunc(L, -3, "GetPrivateString", _m_GetPrivateString);
		Utils.RegisterFunc(L, -3, "SetPublicString", _m_SetPublicString);
		Utils.RegisterFunc(L, -3, "SetPrivateString", _m_SetPrivateString);
		Utils.RegisterFunc(L, -3, "PlayerPrefsGetBool", _m_PlayerPrefsGetBool);
		Utils.RegisterFunc(L, -3, "PlayerPrefsSetBool", _m_PlayerPrefsSetBool);
		Utils.RegisterFunc(L, -3, "PlayerPrefsGetInt", _m_PlayerPrefsGetInt);
		Utils.RegisterFunc(L, -3, "PlayerPrefsSetInt", _m_PlayerPrefsSetInt);
		Utils.RegisterFunc(L, -3, "PlayerPrefsGetFloat", _m_PlayerPrefsGetFloat);
		Utils.RegisterFunc(L, -3, "PlayerPrefsSetFloat", _m_PlayerPrefsSetFloat);
		Utils.RegisterFunc(L, -3, "PlayerPrefsGetString", _m_PlayerPrefsGetString);
		Utils.RegisterFunc(L, -3, "PlayerPrefsSetString", _m_PlayerPrefsSetString);
		Utils.RegisterFunc(L, -3, "UpdateFirstLaunchFlag", _m_UpdateFirstLaunchFlag);
		Utils.RegisterFunc(L, -3, "IsFirstLaunch", _m_IsFirstLaunch);
		Utils.RegisterFunc(L, -3, "CheckFirstLaunchSkipUpdate", _m_CheckFirstLaunchSkipUpdate);
		Utils.RegisterFunc(L, -3, "FirstLaunchSkipUpdateRunning", _m_FirstLaunchSkipUpdateRunning);
		Utils.RegisterFunc(L, -3, "DisableFirstLaunchSkipUpdate", _m_DisableFirstLaunchSkipUpdate);
		Utils.RegisterFunc(L, -2, "UserLanguage", _g_get_UserLanguage);
		Utils.RegisterFunc(L, -2, "GameUid", _g_get_GameUid);
		Utils.RegisterFunc(L, -2, "IsReview", _g_get_IsReview);
		Utils.RegisterFunc(L, -2, "FirstLaunchSkipUpdateNewestVersion", _g_get_FirstLaunchSkipUpdateNewestVersion);
		Utils.RegisterFunc(L, -2, "gameSessionId", _g_get_gameSessionId);
		Utils.RegisterFunc(L, -1, "UserLanguage", _s_set_UserLanguage);
		Utils.RegisterFunc(L, -1, "GameUid", _s_set_GameUid);
		Utils.RegisterFunc(L, -1, "IsReview", _s_set_IsReview);
		Utils.RegisterFunc(L, -1, "FirstLaunchSkipUpdateNewestVersion", _s_set_FirstLaunchSkipUpdateNewestVersion);
		Utils.RegisterFunc(L, -1, "gameSessionId", _s_set_gameSessionId);
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
				SettingManager o = new SettingManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SettingManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Load(IntPtr L)
	{
		try
		{
			bool value = ((SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Load();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Save(IntPtr L)
	{
		try
		{
			bool value = ((SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Save();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HasSetting(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string settingName = Lua.lua_tostring(L, 2);
			bool value = obj.HasSetting(settingName);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveSetting(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string settingName = Lua.lua_tostring(L, 2);
			obj.RemoveSetting(settingName);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveAllSettings(IntPtr L)
	{
		try
		{
			((SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RemoveAllSettings();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBool(IntPtr L)
	{
		try
		{
			SettingManager settingManager = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string settingName = Lua.lua_tostring(L, 2);
				bool value = settingManager.GetBool(settingName);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				string settingName2 = Lua.lua_tostring(L, 2);
				bool defaultValue = Lua.lua_toboolean(L, 3);
				bool value2 = settingManager.GetBool(settingName2, defaultValue);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SettingManager.GetBool!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetBool(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string settingName = Lua.lua_tostring(L, 2);
			bool value = Lua.lua_toboolean(L, 3);
			obj.SetBool(settingName, value);
			return 0;
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
			SettingManager settingManager = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string settingName = Lua.lua_tostring(L, 2);
				int value = settingManager.GetInt(settingName);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string settingName2 = Lua.lua_tostring(L, 2);
				int defaultValue = Lua.xlua_tointeger(L, 3);
				int value2 = settingManager.GetInt(settingName2, defaultValue);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SettingManager.GetInt!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetInt(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string settingName = Lua.lua_tostring(L, 2);
			int value = Lua.xlua_tointeger(L, 3);
			obj.SetInt(settingName, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFloat(IntPtr L)
	{
		try
		{
			SettingManager settingManager = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string settingName = Lua.lua_tostring(L, 2);
				float num2 = settingManager.GetFloat(settingName);
				Lua.lua_pushnumber(L, num2);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string settingName2 = Lua.lua_tostring(L, 2);
				float defaultValue = (float)Lua.lua_tonumber(L, 3);
				float num3 = settingManager.GetFloat(settingName2, defaultValue);
				Lua.lua_pushnumber(L, num3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SettingManager.GetFloat!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFloat(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string settingName = Lua.lua_tostring(L, 2);
			float value = (float)Lua.lua_tonumber(L, 3);
			obj.SetFloat(settingName, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetString(IntPtr L)
	{
		try
		{
			SettingManager settingManager = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string settingName = Lua.lua_tostring(L, 2);
				string str = settingManager.GetString(settingName);
				Lua.lua_pushstring(L, str);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				string settingName2 = Lua.lua_tostring(L, 2);
				string defaultValue = Lua.lua_tostring(L, 3);
				string str2 = settingManager.GetString(settingName2, defaultValue);
				Lua.lua_pushstring(L, str2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SettingManager.GetString!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetString(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string settingName = Lua.lua_tostring(L, 2);
			string value = Lua.lua_tostring(L, 3);
			obj.SetString(settingName, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPublicBool(IntPtr L)
	{
		try
		{
			SettingManager settingManager = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string settingName = Lua.lua_tostring(L, 2);
				bool publicBool = settingManager.GetPublicBool(settingName);
				Lua.lua_pushboolean(L, publicBool);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				string settingName2 = Lua.lua_tostring(L, 2);
				bool defaultValue = Lua.lua_toboolean(L, 3);
				bool publicBool2 = settingManager.GetPublicBool(settingName2, defaultValue);
				Lua.lua_pushboolean(L, publicBool2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SettingManager.GetPublicBool!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPrivateBool(IntPtr L)
	{
		try
		{
			SettingManager settingManager = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string settingName = Lua.lua_tostring(L, 2);
				bool privateBool = settingManager.GetPrivateBool(settingName);
				Lua.lua_pushboolean(L, privateBool);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				string settingName2 = Lua.lua_tostring(L, 2);
				bool defaultValue = Lua.lua_toboolean(L, 3);
				bool privateBool2 = settingManager.GetPrivateBool(settingName2, defaultValue);
				Lua.lua_pushboolean(L, privateBool2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SettingManager.GetPrivateBool!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPublicBool(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string settingName = Lua.lua_tostring(L, 2);
			bool value = Lua.lua_toboolean(L, 3);
			obj.SetPublicBool(settingName, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPrivateBool(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string settingName = Lua.lua_tostring(L, 2);
			bool value = Lua.lua_toboolean(L, 3);
			obj.SetPrivateBool(settingName, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPublicInt(IntPtr L)
	{
		try
		{
			SettingManager settingManager = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string settingName = Lua.lua_tostring(L, 2);
				int publicInt = settingManager.GetPublicInt(settingName);
				Lua.xlua_pushinteger(L, publicInt);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string settingName2 = Lua.lua_tostring(L, 2);
				int defaultValue = Lua.xlua_tointeger(L, 3);
				int publicInt2 = settingManager.GetPublicInt(settingName2, defaultValue);
				Lua.xlua_pushinteger(L, publicInt2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SettingManager.GetPublicInt!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPublicInt(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string settingName = Lua.lua_tostring(L, 2);
			int value = Lua.xlua_tointeger(L, 3);
			obj.SetPublicInt(settingName, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPrivateInt(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string settingName = Lua.lua_tostring(L, 2);
			int value = Lua.xlua_tointeger(L, 3);
			obj.SetPrivateInt(settingName, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPrivateInt(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string settingName = Lua.lua_tostring(L, 2);
			int value = Lua.xlua_tointeger(L, 3);
			int privateInt = obj.GetPrivateInt(settingName, value);
			Lua.xlua_pushinteger(L, privateInt);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPrivateFloat(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string settingName = Lua.lua_tostring(L, 2);
			float value = (float)Lua.lua_tonumber(L, 3);
			obj.SetPrivateFloat(settingName, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPrivateFloat(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string settingName = Lua.lua_tostring(L, 2);
			float value = (float)Lua.lua_tonumber(L, 3);
			float privateFloat = obj.GetPrivateFloat(settingName, value);
			Lua.lua_pushnumber(L, privateFloat);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPublicFloat(IntPtr L)
	{
		try
		{
			SettingManager settingManager = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string settingName = Lua.lua_tostring(L, 2);
				float publicFloat = settingManager.GetPublicFloat(settingName);
				Lua.lua_pushnumber(L, publicFloat);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string settingName2 = Lua.lua_tostring(L, 2);
				float defaultValue = (float)Lua.lua_tonumber(L, 3);
				float publicFloat2 = settingManager.GetPublicFloat(settingName2, defaultValue);
				Lua.lua_pushnumber(L, publicFloat2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SettingManager.GetPublicFloat!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPublicFloat(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string settingName = Lua.lua_tostring(L, 2);
			float value = (float)Lua.lua_tonumber(L, 3);
			obj.SetPublicFloat(settingName, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPublicString(IntPtr L)
	{
		try
		{
			SettingManager settingManager = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string settingName = Lua.lua_tostring(L, 2);
				string publicString = settingManager.GetPublicString(settingName);
				Lua.lua_pushstring(L, publicString);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				string settingName2 = Lua.lua_tostring(L, 2);
				string defaultValue = Lua.lua_tostring(L, 3);
				string publicString2 = settingManager.GetPublicString(settingName2, defaultValue);
				Lua.lua_pushstring(L, publicString2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SettingManager.GetPublicString!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPrivateString(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string settingName = Lua.lua_tostring(L, 2);
			string defaultValue = Lua.lua_tostring(L, 3);
			string privateString = obj.GetPrivateString(settingName, defaultValue);
			Lua.lua_pushstring(L, privateString);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPublicString(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string settingName = Lua.lua_tostring(L, 2);
			string value = Lua.lua_tostring(L, 3);
			obj.SetPublicString(settingName, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPrivateString(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string settingName = Lua.lua_tostring(L, 2);
			string value = Lua.lua_tostring(L, 3);
			obj.SetPrivateString(settingName, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayerPrefsGetBool(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			bool defaultValue = Lua.lua_toboolean(L, 3);
			bool value = obj.PlayerPrefsGetBool(key, defaultValue);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayerPrefsSetBool(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			bool value = Lua.lua_toboolean(L, 3);
			obj.PlayerPrefsSetBool(key, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayerPrefsGetInt(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			int defaultValue = Lua.xlua_tointeger(L, 3);
			int value = obj.PlayerPrefsGetInt(key, defaultValue);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayerPrefsSetInt(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			int value = Lua.xlua_tointeger(L, 3);
			obj.PlayerPrefsSetInt(key, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayerPrefsGetFloat(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			float defaultValue = (float)Lua.lua_tonumber(L, 3);
			float num = obj.PlayerPrefsGetFloat(key, defaultValue);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayerPrefsSetFloat(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			float value = (float)Lua.lua_tonumber(L, 3);
			obj.PlayerPrefsSetFloat(key, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayerPrefsGetString(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			string defaultValue = Lua.lua_tostring(L, 3);
			string str = obj.PlayerPrefsGetString(key, defaultValue);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayerPrefsSetString(IntPtr L)
	{
		try
		{
			SettingManager obj = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			string value = Lua.lua_tostring(L, 3);
			obj.PlayerPrefsSetString(key, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateFirstLaunchFlag(IntPtr L)
	{
		try
		{
			SettingManager settingManager = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool launchFinish = Lua.lua_toboolean(L, 2);
				settingManager.UpdateFirstLaunchFlag(launchFinish);
				return 0;
			}
			if (num == 1)
			{
				settingManager.UpdateFirstLaunchFlag();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SettingManager.UpdateFirstLaunchFlag!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsFirstLaunch(IntPtr L)
	{
		try
		{
			bool value = ((SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsFirstLaunch();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckFirstLaunchSkipUpdate(IntPtr L)
	{
		try
		{
			SettingManager settingManager = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool init = Lua.lua_toboolean(L, 2);
				bool value = settingManager.CheckFirstLaunchSkipUpdate(init);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 1)
			{
				bool value2 = settingManager.CheckFirstLaunchSkipUpdate();
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SettingManager.CheckFirstLaunchSkipUpdate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FirstLaunchSkipUpdateRunning(IntPtr L)
	{
		try
		{
			((SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).FirstLaunchSkipUpdateRunning();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DisableFirstLaunchSkipUpdate(IntPtr L)
	{
		try
		{
			((SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DisableFirstLaunchSkipUpdate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_UserLanguage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SettingManager settingManager = (SettingManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushGameFrameworkLocalizationLanguage(L, settingManager.UserLanguage);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GameUid(IntPtr L)
	{
		try
		{
			SettingManager settingManager = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, settingManager.GameUid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsReview(IntPtr L)
	{
		try
		{
			SettingManager settingManager = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, settingManager.IsReview);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_FirstLaunchSkipUpdateNewestVersion(IntPtr L)
	{
		try
		{
			SettingManager settingManager = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, settingManager.FirstLaunchSkipUpdateNewestVersion);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_gameSessionId(IntPtr L)
	{
		try
		{
			SettingManager settingManager = (SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, settingManager.gameSessionId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_UserLanguage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SettingManager settingManager = (SettingManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Language val);
			settingManager.UserLanguage = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_GameUid(IntPtr L)
	{
		try
		{
			((SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GameUid = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsReview(IntPtr L)
	{
		try
		{
			((SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsReview = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_FirstLaunchSkipUpdateNewestVersion(IntPtr L)
	{
		try
		{
			((SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).FirstLaunchSkipUpdateNewestVersion = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_gameSessionId(IntPtr L)
	{
		try
		{
			((SettingManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).gameSessionId = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
