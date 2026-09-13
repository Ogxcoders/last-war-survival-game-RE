using System;
using UnityEngine;
using UnityEngine.Events;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineApplicationWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Application);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 22, 30, 3);
		Utils.RegisterFunc(L, -4, "Quit", _m_Quit_xlua_st_);
		Utils.RegisterFunc(L, -4, "Unload", _m_Unload_xlua_st_);
		Utils.RegisterFunc(L, -4, "CanStreamedLevelBeLoaded", _m_CanStreamedLevelBeLoaded_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsPlaying", _m_IsPlaying_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetBuildTags", _m_GetBuildTags_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetBuildTags", _m_SetBuildTags_xlua_st_);
		Utils.RegisterFunc(L, -4, "HasProLicense", _m_HasProLicense_xlua_st_);
		Utils.RegisterFunc(L, -4, "RequestAdvertisingIdentifierAsync", _m_RequestAdvertisingIdentifierAsync_xlua_st_);
		Utils.RegisterFunc(L, -4, "OpenURL", _m_OpenURL_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetStackTraceLogType", _m_GetStackTraceLogType_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetStackTraceLogType", _m_SetStackTraceLogType_xlua_st_);
		Utils.RegisterFunc(L, -4, "RequestUserAuthorization", _m_RequestUserAuthorization_xlua_st_);
		Utils.RegisterFunc(L, -4, "HasUserAuthorization", _m_HasUserAuthorization_xlua_st_);
		Utils.RegisterFunc(L, -4, "lowMemory", _e_lowMemory);
		Utils.RegisterFunc(L, -4, "logMessageReceived", _e_logMessageReceived);
		Utils.RegisterFunc(L, -4, "logMessageReceivedThreaded", _e_logMessageReceivedThreaded);
		Utils.RegisterFunc(L, -4, "onBeforeRender", _e_onBeforeRender);
		Utils.RegisterFunc(L, -4, "focusChanged", _e_focusChanged);
		Utils.RegisterFunc(L, -4, "deepLinkActivated", _e_deepLinkActivated);
		Utils.RegisterFunc(L, -4, "wantsToQuit", _e_wantsToQuit);
		Utils.RegisterFunc(L, -4, "quitting", _e_quitting);
		Utils.RegisterFunc(L, -2, "isPlaying", _g_get_isPlaying);
		Utils.RegisterFunc(L, -2, "isFocused", _g_get_isFocused);
		Utils.RegisterFunc(L, -2, "buildGUID", _g_get_buildGUID);
		Utils.RegisterFunc(L, -2, "runInBackground", _g_get_runInBackground);
		Utils.RegisterFunc(L, -2, "isBatchMode", _g_get_isBatchMode);
		Utils.RegisterFunc(L, -2, "dataPath", _g_get_dataPath);
		Utils.RegisterFunc(L, -2, "streamingAssetsPath", _g_get_streamingAssetsPath);
		Utils.RegisterFunc(L, -2, "persistentDataPath", _g_get_persistentDataPath);
		Utils.RegisterFunc(L, -2, "temporaryCachePath", _g_get_temporaryCachePath);
		Utils.RegisterFunc(L, -2, "absoluteURL", _g_get_absoluteURL);
		Utils.RegisterFunc(L, -2, "unityVersion", _g_get_unityVersion);
		Utils.RegisterFunc(L, -2, "version", _g_get_version);
		Utils.RegisterFunc(L, -2, "installerName", _g_get_installerName);
		Utils.RegisterFunc(L, -2, "identifier", _g_get_identifier);
		Utils.RegisterFunc(L, -2, "installMode", _g_get_installMode);
		Utils.RegisterFunc(L, -2, "sandboxType", _g_get_sandboxType);
		Utils.RegisterFunc(L, -2, "productName", _g_get_productName);
		Utils.RegisterFunc(L, -2, "companyName", _g_get_companyName);
		Utils.RegisterFunc(L, -2, "cloudProjectId", _g_get_cloudProjectId);
		Utils.RegisterFunc(L, -2, "targetFrameRate", _g_get_targetFrameRate);
		Utils.RegisterFunc(L, -2, "consoleLogPath", _g_get_consoleLogPath);
		Utils.RegisterFunc(L, -2, "backgroundLoadingPriority", _g_get_backgroundLoadingPriority);
		Utils.RegisterFunc(L, -2, "genuine", _g_get_genuine);
		Utils.RegisterFunc(L, -2, "genuineCheckAvailable", _g_get_genuineCheckAvailable);
		Utils.RegisterFunc(L, -2, "platform", _g_get_platform);
		Utils.RegisterFunc(L, -2, "isMobilePlatform", _g_get_isMobilePlatform);
		Utils.RegisterFunc(L, -2, "isConsolePlatform", _g_get_isConsolePlatform);
		Utils.RegisterFunc(L, -2, "systemLanguage", _g_get_systemLanguage);
		Utils.RegisterFunc(L, -2, "internetReachability", _g_get_internetReachability);
		Utils.RegisterFunc(L, -2, "isEditor", _g_get_isEditor);
		Utils.RegisterFunc(L, -1, "runInBackground", _s_set_runInBackground);
		Utils.RegisterFunc(L, -1, "targetFrameRate", _s_set_targetFrameRate);
		Utils.RegisterFunc(L, -1, "backgroundLoadingPriority", _s_set_backgroundLoadingPriority);
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
				Application o = new Application();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Application constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Quit_xlua_st_(IntPtr L)
	{
		try
		{
			switch (Lua.lua_gettop(L))
			{
			case 0:
				Application.Quit();
				return 0;
			case 1:
				if (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1))
				{
					Application.Quit(Lua.xlua_tointeger(L, 1));
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Application.Quit!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Unload_xlua_st_(IntPtr L)
	{
		try
		{
			Application.Unload();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CanStreamedLevelBeLoaded_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1))
			{
				bool value = Application.CanStreamedLevelBeLoaded(Lua.xlua_tointeger(L, 1));
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				bool value2 = Application.CanStreamedLevelBeLoaded(Lua.lua_tostring(L, 1));
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Application.CanStreamedLevelBeLoaded!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsPlaying_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = Application.IsPlaying((UnityEngine.Object)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(UnityEngine.Object)));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBuildTags_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string[] buildTags = Application.GetBuildTags();
			objectTranslator.Push(L, buildTags);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetBuildTags_xlua_st_(IntPtr L)
	{
		try
		{
			Application.SetBuildTags((string[])ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(string[])));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HasProLicense_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = Application.HasProLicense();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RequestAdvertisingIdentifierAsync_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = Application.RequestAdvertisingIdentifierAsync(ObjectTranslatorPool.Instance.Find(L).GetDelegate<Application.AdvertisingIdentifierCallback>(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OpenURL_xlua_st_(IntPtr L)
	{
		try
		{
			Application.OpenURL(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetStackTraceLogType_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out LogType v);
			StackTraceLogType stackTraceLogType = Application.GetStackTraceLogType(v);
			objectTranslator.Push(L, stackTraceLogType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetStackTraceLogType_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out LogType v);
			objectTranslator.Get(L, 2, out StackTraceLogType v2);
			Application.SetStackTraceLogType(v, v2);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RequestUserAuthorization_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out UserAuthorization v);
			AsyncOperation o = Application.RequestUserAuthorization(v);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HasUserAuthorization_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out UserAuthorization v);
			bool value = Application.HasUserAuthorization(v);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isPlaying(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Application.isPlaying);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isFocused(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Application.isFocused);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_buildGUID(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, Application.buildGUID);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_runInBackground(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Application.runInBackground);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isBatchMode(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Application.isBatchMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_dataPath(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, Application.dataPath);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_streamingAssetsPath(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, Application.streamingAssetsPath);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_persistentDataPath(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, Application.persistentDataPath);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_temporaryCachePath(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, Application.temporaryCachePath);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_absoluteURL(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, Application.absoluteURL);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_unityVersion(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, Application.unityVersion);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_version(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, Application.version);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_installerName(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, Application.installerName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_identifier(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, Application.identifier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_installMode(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Application.installMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sandboxType(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Application.sandboxType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_productName(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, Application.productName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_companyName(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, Application.companyName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cloudProjectId(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, Application.cloudProjectId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetFrameRate(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, Application.targetFrameRate);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_consoleLogPath(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, Application.consoleLogPath);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_backgroundLoadingPriority(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Application.backgroundLoadingPriority);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_genuine(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Application.genuine);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_genuineCheckAvailable(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Application.genuineCheckAvailable);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_platform(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Application.platform);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isMobilePlatform(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Application.isMobilePlatform);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isConsolePlatform(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Application.isConsolePlatform);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_systemLanguage(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Application.systemLanguage);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_internetReachability(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Application.internetReachability);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isEditor(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Application.isEditor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_runInBackground(IntPtr L)
	{
		try
		{
			Application.runInBackground = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetFrameRate(IntPtr L)
	{
		try
		{
			Application.targetFrameRate = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_backgroundLoadingPriority(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ThreadPriority v);
			Application.backgroundLoadingPriority = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_lowMemory(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			Application.LowMemoryCallback lowMemoryCallback = objectTranslator.GetDelegate<Application.LowMemoryCallback>(L, 2);
			if (lowMemoryCallback == null)
			{
				return Lua.luaL_error(L, "#2 need UnityEngine.Application.LowMemoryCallback!");
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "+"))
			{
				Application.lowMemory += lowMemoryCallback;
				return 0;
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "-"))
			{
				Application.lowMemory -= lowMemoryCallback;
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Application.lowMemory!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_logMessageReceived(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			Application.LogCallback logCallback = objectTranslator.GetDelegate<Application.LogCallback>(L, 2);
			if (logCallback == null)
			{
				return Lua.luaL_error(L, "#2 need UnityEngine.Application.LogCallback!");
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "+"))
			{
				Application.logMessageReceived += logCallback;
				return 0;
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "-"))
			{
				Application.logMessageReceived -= logCallback;
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Application.logMessageReceived!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_logMessageReceivedThreaded(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			Application.LogCallback logCallback = objectTranslator.GetDelegate<Application.LogCallback>(L, 2);
			if (logCallback == null)
			{
				return Lua.luaL_error(L, "#2 need UnityEngine.Application.LogCallback!");
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "+"))
			{
				Application.logMessageReceivedThreaded += logCallback;
				return 0;
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "-"))
			{
				Application.logMessageReceivedThreaded -= logCallback;
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Application.logMessageReceivedThreaded!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_onBeforeRender(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			UnityAction unityAction = objectTranslator.GetDelegate<UnityAction>(L, 2);
			if (unityAction == null)
			{
				return Lua.luaL_error(L, "#2 need UnityEngine.Events.UnityAction!");
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "+"))
			{
				Application.onBeforeRender += unityAction;
				return 0;
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "-"))
			{
				Application.onBeforeRender -= unityAction;
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Application.onBeforeRender!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_focusChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			Action<bool> action = objectTranslator.GetDelegate<Action<bool>>(L, 2);
			if (action == null)
			{
				return Lua.luaL_error(L, "#2 need System.Action<bool>!");
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "+"))
			{
				Application.focusChanged += action;
				return 0;
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "-"))
			{
				Application.focusChanged -= action;
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Application.focusChanged!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_deepLinkActivated(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			Action<string> action = objectTranslator.GetDelegate<Action<string>>(L, 2);
			if (action == null)
			{
				return Lua.luaL_error(L, "#2 need System.Action<string>!");
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "+"))
			{
				Application.deepLinkActivated += action;
				return 0;
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "-"))
			{
				Application.deepLinkActivated -= action;
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Application.deepLinkActivated!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_wantsToQuit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			Func<bool> func = objectTranslator.GetDelegate<Func<bool>>(L, 2);
			if (func == null)
			{
				return Lua.luaL_error(L, "#2 need System.Func<bool>!");
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "+"))
			{
				Application.wantsToQuit += func;
				return 0;
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "-"))
			{
				Application.wantsToQuit -= func;
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Application.wantsToQuit!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_quitting(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			Action action = objectTranslator.GetDelegate<Action>(L, 2);
			if (action == null)
			{
				return Lua.luaL_error(L, "#2 need System.Action!");
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "+"))
			{
				Application.quitting += action;
				return 0;
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "-"))
			{
				Application.quitting -= action;
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Application.quitting!");
	}
}
