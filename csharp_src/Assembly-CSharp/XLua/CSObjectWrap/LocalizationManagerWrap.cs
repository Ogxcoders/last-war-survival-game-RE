using System;
using System.Collections.Generic;
using System.IO;
using GameFramework.Localization;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LocalizationManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(LocalizationManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 21, 5, 2);
		Utils.RegisterFunc(L, -3, "Initialize", _m_Initialize);
		Utils.RegisterFunc(L, -3, "Shutdown", _m_Shutdown);
		Utils.RegisterFunc(L, -3, "Uninitialize", _m_Uninitialize);
		Utils.RegisterFunc(L, -3, "SetSwapLocaleFile", _m_SetSwapLocaleFile);
		Utils.RegisterFunc(L, -3, "UseSwapLocaleFile", _m_UseSwapLocaleFile);
		Utils.RegisterFunc(L, -3, "LoadDictionary", _m_LoadDictionary);
		Utils.RegisterFunc(L, -3, "ReloadDictionaryByUpdate", _m_ReloadDictionaryByUpdate);
		Utils.RegisterFunc(L, -3, "ReloadDictionaryByUpdateDevLocale", _m_ReloadDictionaryByUpdateDevLocale);
		Utils.RegisterFunc(L, -3, "IsSuported", _m_IsSuported);
		Utils.RegisterFunc(L, -3, "SetSuportedLanguages", _m_SetSuportedLanguages);
		Utils.RegisterFunc(L, -3, "UpdateSkinData", _m_UpdateSkinData);
		Utils.RegisterFunc(L, -3, "GetString", _m_GetString);
		Utils.RegisterFunc(L, -3, "GetLanguageName", _m_GetLanguageName);
		Utils.RegisterFunc(L, -3, "GetLanguageNameToLoading", _m_GetLanguageNameToLoading);
		Utils.RegisterFunc(L, -3, "GetLanguageNameByInt", _m_GetLanguageNameByInt);
		Utils.RegisterFunc(L, -3, "GetLanguage", _m_GetLanguage);
		Utils.RegisterFunc(L, -3, "SetLanguage", _m_SetLanguage);
		Utils.RegisterFunc(L, -3, "GetFontByLanguage", _m_GetFontByLanguage);
		Utils.RegisterFunc(L, -3, "HasKey", _m_HasKey);
		Utils.RegisterFunc(L, -3, "InitFont", _m_InitFont);
		Utils.RegisterFunc(L, -3, "ClearFontDynamicData", _m_ClearFontDynamicData);
		Utils.RegisterFunc(L, -2, "IsInitDone", _g_get_IsInitDone);
		Utils.RegisterFunc(L, -2, "IsInitSuccess", _g_get_IsInitSuccess);
		Utils.RegisterFunc(L, -2, "ShowKey", _g_get_ShowKey);
		Utils.RegisterFunc(L, -2, "Language", _g_get_Language);
		Utils.RegisterFunc(L, -2, "DictionaryCount", _g_get_DictionaryCount);
		Utils.RegisterFunc(L, -1, "ShowKey", _s_set_ShowKey);
		Utils.RegisterFunc(L, -1, "Language", _s_set_Language);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 4, 1, 0);
		Utils.RegisterFunc(L, -4, "ParseTxtDictionary", _m_ParseTxtDictionary_xlua_st_);
		Utils.RegisterFunc(L, -4, "ParseBinDictionary", _m_ParseBinDictionary_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetLanguageName", _m_GetLanguageName_xlua_st_);
		Utils.RegisterFunc(L, -2, "SystemLanguage", _g_get_SystemLanguage);
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
				LocalizationManager o = new LocalizationManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LocalizationManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Initialize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LocalizationManager localizationManager = (LocalizationManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Language val);
			localizationManager.Initialize(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Shutdown(IntPtr L)
	{
		try
		{
			((LocalizationManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Shutdown();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Uninitialize(IntPtr L)
	{
		try
		{
			((LocalizationManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Uninitialize();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSwapLocaleFile(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LocalizationManager localizationManager = (LocalizationManager)objectTranslator.FastGetCSObj(L, 1);
			LocaleFileParallel.LocaleFileSwapPayload swapLocaleFile = (LocaleFileParallel.LocaleFileSwapPayload)objectTranslator.GetObject(L, 2, typeof(LocaleFileParallel.LocaleFileSwapPayload));
			localizationManager.SetSwapLocaleFile(swapLocaleFile);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UseSwapLocaleFile(IntPtr L)
	{
		try
		{
			LocalizationManager localizationManager = (LocalizationManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool safeMode = Lua.lua_toboolean(L, 2);
				localizationManager.UseSwapLocaleFile(safeMode);
				return 0;
			}
			if (num == 1)
			{
				localizationManager.UseSwapLocaleFile();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LocalizationManager.UseSwapLocaleFile!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadDictionary(IntPtr L)
	{
		try
		{
			LocalizationManager obj = (LocalizationManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string dictionaryName = Lua.lua_tostring(L, 2);
			obj.LoadDictionary(dictionaryName);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReloadDictionaryByUpdate(IntPtr L)
	{
		try
		{
			LocalizationManager obj = (LocalizationManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int version = Lua.xlua_tointeger(L, 2);
			string localeAbb = Lua.lua_tostring(L, 3);
			obj.ReloadDictionaryByUpdate(version, localeAbb);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReloadDictionaryByUpdateDevLocale(IntPtr L)
	{
		try
		{
			LocalizationManager obj = (LocalizationManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string localeAbb = Lua.lua_tostring(L, 2);
			obj.ReloadDictionaryByUpdateDevLocale(localeAbb);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ParseTxtDictionary_xlua_st_(IntPtr L)
	{
		try
		{
			Dictionary<string, LocalizationManager.DialogEntry> dictionary = (Dictionary<string, LocalizationManager.DialogEntry>)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Dictionary<string, LocalizationManager.DialogEntry>));
			string text = Lua.lua_tostring(L, 2);
			bool value = LocalizationManager.ParseTxtDictionary(dictionary, text);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ParseBinDictionary_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<string, string> dictionary = (Dictionary<string, string>)objectTranslator.GetObject(L, 1, typeof(Dictionary<string, string>));
			MemoryStream memoryStream = (MemoryStream)objectTranslator.GetObject(L, 2, typeof(MemoryStream));
			bool value = LocalizationManager.ParseBinDictionary(dictionary, memoryStream);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsSuported(IntPtr L)
	{
		try
		{
			bool value = ((LocalizationManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsSuported();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSuportedLanguages(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LocalizationManager localizationManager = (LocalizationManager)objectTranslator.FastGetCSObj(L, 1);
			List<int> suportedLanguages = (List<int>)objectTranslator.GetObject(L, 2, typeof(List<int>));
			localizationManager.SetSuportedLanguages(suportedLanguages);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateSkinData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LocalizationManager localizationManager = (LocalizationManager)objectTranslator.FastGetCSObj(L, 1);
			LuaTable rowData = (LuaTable)objectTranslator.GetObject(L, 2, typeof(LuaTable));
			localizationManager.UpdateSkinData(rowData);
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
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LocalizationManager localizationManager = (LocalizationManager)objectTranslator.FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			object[] args = objectTranslator.GetParams<object>(L, 3);
			string str = localizationManager.GetString(key, args);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLanguageName(IntPtr L)
	{
		try
		{
			string languageName = ((LocalizationManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetLanguageName();
			Lua.lua_pushstring(L, languageName);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLanguageNameToLoading(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LocalizationManager localizationManager = (LocalizationManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Language val);
			string languageNameToLoading = localizationManager.GetLanguageNameToLoading(val);
			Lua.lua_pushstring(L, languageNameToLoading);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLanguageNameByInt(IntPtr L)
	{
		try
		{
			LocalizationManager obj = (LocalizationManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int language = Lua.xlua_tointeger(L, 2);
			string languageNameByInt = obj.GetLanguageNameByInt(language);
			Lua.lua_pushstring(L, languageNameByInt);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLanguageName_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Language val);
			string languageName = LocalizationManager.GetLanguageName(val);
			Lua.lua_pushstring(L, languageName);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLanguage(IntPtr L)
	{
		try
		{
			int language = ((LocalizationManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetLanguage();
			Lua.xlua_pushinteger(L, language);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLanguage(IntPtr L)
	{
		try
		{
			LocalizationManager obj = (LocalizationManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int language = Lua.xlua_tointeger(L, 2);
			obj.SetLanguage(language);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFontByLanguage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Font fontByLanguage = ((LocalizationManager)objectTranslator.FastGetCSObj(L, 1)).GetFontByLanguage();
			objectTranslator.Push(L, fontByLanguage);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HasKey(IntPtr L)
	{
		try
		{
			LocalizationManager obj = (LocalizationManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_InitFont(IntPtr L)
	{
		try
		{
			((LocalizationManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InitFont();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearFontDynamicData(IntPtr L)
	{
		try
		{
			((LocalizationManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearFontDynamicData();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsInitDone(IntPtr L)
	{
		try
		{
			LocalizationManager localizationManager = (LocalizationManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, localizationManager.IsInitDone);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsInitSuccess(IntPtr L)
	{
		try
		{
			LocalizationManager localizationManager = (LocalizationManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, localizationManager.IsInitSuccess);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ShowKey(IntPtr L)
	{
		try
		{
			LocalizationManager localizationManager = (LocalizationManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, localizationManager.ShowKey);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Language(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LocalizationManager localizationManager = (LocalizationManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushGameFrameworkLocalizationLanguage(L, localizationManager.Language);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SystemLanguage(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushGameFrameworkLocalizationLanguage(L, LocalizationManager.SystemLanguage);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DictionaryCount(IntPtr L)
	{
		try
		{
			LocalizationManager localizationManager = (LocalizationManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, localizationManager.DictionaryCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ShowKey(IntPtr L)
	{
		try
		{
			((LocalizationManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ShowKey = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Language(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LocalizationManager localizationManager = (LocalizationManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Language val);
			localizationManager.Language = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
