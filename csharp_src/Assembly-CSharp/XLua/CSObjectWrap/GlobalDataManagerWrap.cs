using System;
using System.Collections.Generic;
using Sfs2X.Entities.Data;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GlobalDataManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GlobalDataManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 16, 55, 55);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "SetAnalyticID", _m_SetAnalyticID);
		Utils.RegisterFunc(L, -3, "SetCnFlagFromServer", _m_SetCnFlagFromServer);
		Utils.RegisterFunc(L, -3, "Reset", _m_Reset);
		Utils.RegisterFunc(L, -3, "GetGlobalValue", _m_GetGlobalValue);
		Utils.RegisterFunc(L, -3, "SetGlobalValue", _m_SetGlobalValue);
		Utils.RegisterFunc(L, -3, "recordGaid", _m_recordGaid);
		Utils.RegisterFunc(L, -3, "isGoogle", _m_isGoogle);
		Utils.RegisterFunc(L, -3, "isGoogleOnlyCheckAnalyticID", _m_isGoogleOnlyCheckAnalyticID);
		Utils.RegisterFunc(L, -3, "isChina", _m_isChina);
		Utils.RegisterFunc(L, -3, "isMiddleEast", _m_isMiddleEast);
		Utils.RegisterFunc(L, -3, "isTencent", _m_isTencent);
		Utils.RegisterFunc(L, -3, "isAmazon", _m_isAmazon);
		Utils.RegisterFunc(L, -3, "isMol", _m_isMol);
		Utils.RegisterFunc(L, -3, "isMycard", _m_isMycard);
		Utils.RegisterFunc(L, -3, "isOnestore", _m_isOnestore);
		Utils.RegisterFunc(L, -2, "HasRequestAllProducts", _g_get_HasRequestAllProducts);
		Utils.RegisterFunc(L, -2, "download_video_url", _g_get_download_video_url);
		Utils.RegisterFunc(L, -2, "download_video_url2", _g_get_download_video_url2);
		Utils.RegisterFunc(L, -2, "downloadurlcdn", _g_get_downloadurlcdn);
		Utils.RegisterFunc(L, -2, "downloadurl", _g_get_downloadurl);
		Utils.RegisterFunc(L, -2, "eu_state", _g_get_eu_state);
		Utils.RegisterFunc(L, -2, "fblikeutil", _g_get_fblikeutil);
		Utils.RegisterFunc(L, -2, "force_merge", _g_get_force_merge);
		Utils.RegisterFunc(L, -2, "force_use_downloadxml", _g_get_force_use_downloadxml);
		Utils.RegisterFunc(L, -2, "lua", _g_get_lua);
		Utils.RegisterFunc(L, -2, "luaCode", _g_get_luaCode);
		Utils.RegisterFunc(L, -2, "luaCode_v3", _g_get_luaCode_v3);
		Utils.RegisterFunc(L, -2, "luaSize", _g_get_luaSize);
		Utils.RegisterFunc(L, -2, "luaVersion", _g_get_luaVersion);
		Utils.RegisterFunc(L, -2, "luaVersion_v3", _g_get_luaVersion_v3);
		Utils.RegisterFunc(L, -2, "luazipSize", _g_get_luazipSize);
		Utils.RegisterFunc(L, -2, "randKey", _g_get_randKey);
		Utils.RegisterFunc(L, -2, "reduce_init_data", _g_get_reduce_init_data);
		Utils.RegisterFunc(L, -2, "serverVersion", _g_get_serverVersion);
		Utils.RegisterFunc(L, -2, "updateType", _g_get_updateType);
		Utils.RegisterFunc(L, -2, "upload_video_url", _g_get_upload_video_url);
		Utils.RegisterFunc(L, -2, "xmlVersion", _g_get_xmlVersion);
		Utils.RegisterFunc(L, -2, "loginServerInfo", _g_get_loginServerInfo);
		Utils.RegisterFunc(L, -2, "gcmRegisterId", _g_get_gcmRegisterId);
		Utils.RegisterFunc(L, -2, "referrer", _g_get_referrer);
		Utils.RegisterFunc(L, -2, "deeplinkParams", _g_get_deeplinkParams);
		Utils.RegisterFunc(L, -2, "AndroidID", _g_get_AndroidID);
		Utils.RegisterFunc(L, -2, "IMEI", _g_get_IMEI);
		Utils.RegisterFunc(L, -2, "analyticID", _g_get_analyticID);
		Utils.RegisterFunc(L, -2, "s_isGooglePlayAvailable", _g_get_s_isGooglePlayAvailable);
		Utils.RegisterFunc(L, -2, "platformUID", _g_get_platformUID);
		Utils.RegisterFunc(L, -2, "parseRegisterId", _g_get_parseRegisterId);
		Utils.RegisterFunc(L, -2, "fromCountry", _g_get_fromCountry);
		Utils.RegisterFunc(L, -2, "gaid", _g_get_gaid);
		Utils.RegisterFunc(L, -2, "isTodayFirstLogin", _g_get_isTodayFirstLogin);
		Utils.RegisterFunc(L, -2, "isNewServer", _g_get_isNewServer);
		Utils.RegisterFunc(L, -2, "version", _g_get_version);
		Utils.RegisterFunc(L, -2, "uuid", _g_get_uuid);
		Utils.RegisterFunc(L, -2, "gaidCache", _g_get_gaidCache);
		Utils.RegisterFunc(L, -2, "isUploadPic", _g_get_isUploadPic);
		Utils.RegisterFunc(L, -2, "isOpenElvaChat", _g_get_isOpenElvaChat);
		Utils.RegisterFunc(L, -2, "isFAQVoteResp", _g_get_isFAQVoteResp);
		Utils.RegisterFunc(L, -2, "cityTileCountry", _g_get_cityTileCountry);
		Utils.RegisterFunc(L, -2, "serverType", _g_get_serverType);
		Utils.RegisterFunc(L, -2, "serverMax", _g_get_serverMax);
		Utils.RegisterFunc(L, -2, "nowGameCnt", _g_get_nowGameCnt);
		Utils.RegisterFunc(L, -2, "freeSpdT", _g_get_freeSpdT);
		Utils.RegisterFunc(L, -2, "TeleportLimitTime", _g_get_TeleportLimitTime);
		Utils.RegisterFunc(L, -2, "TransResForbiddenSwith", _g_get_TransResForbiddenSwith);
		Utils.RegisterFunc(L, -2, "NewTransKingdomLevel", _g_get_NewTransKingdomLevel);
		Utils.RegisterFunc(L, -2, "IsCityMoved", _g_get_IsCityMoved);
		Utils.RegisterFunc(L, -2, "pushOffWithQuitGame", _g_get_pushOffWithQuitGame);
		Utils.RegisterFunc(L, -2, "isInBackGround", _g_get_isInBackGround);
		Utils.RegisterFunc(L, -2, "gameLineBlackList", _g_get_gameLineBlackList);
		Utils.RegisterFunc(L, -2, "LoginServerError", _g_get_LoginServerError);
		Utils.RegisterFunc(L, -1, "HasRequestAllProducts", _s_set_HasRequestAllProducts);
		Utils.RegisterFunc(L, -1, "download_video_url", _s_set_download_video_url);
		Utils.RegisterFunc(L, -1, "download_video_url2", _s_set_download_video_url2);
		Utils.RegisterFunc(L, -1, "downloadurlcdn", _s_set_downloadurlcdn);
		Utils.RegisterFunc(L, -1, "downloadurl", _s_set_downloadurl);
		Utils.RegisterFunc(L, -1, "eu_state", _s_set_eu_state);
		Utils.RegisterFunc(L, -1, "fblikeutil", _s_set_fblikeutil);
		Utils.RegisterFunc(L, -1, "force_merge", _s_set_force_merge);
		Utils.RegisterFunc(L, -1, "force_use_downloadxml", _s_set_force_use_downloadxml);
		Utils.RegisterFunc(L, -1, "lua", _s_set_lua);
		Utils.RegisterFunc(L, -1, "luaCode", _s_set_luaCode);
		Utils.RegisterFunc(L, -1, "luaCode_v3", _s_set_luaCode_v3);
		Utils.RegisterFunc(L, -1, "luaSize", _s_set_luaSize);
		Utils.RegisterFunc(L, -1, "luaVersion", _s_set_luaVersion);
		Utils.RegisterFunc(L, -1, "luaVersion_v3", _s_set_luaVersion_v3);
		Utils.RegisterFunc(L, -1, "luazipSize", _s_set_luazipSize);
		Utils.RegisterFunc(L, -1, "randKey", _s_set_randKey);
		Utils.RegisterFunc(L, -1, "reduce_init_data", _s_set_reduce_init_data);
		Utils.RegisterFunc(L, -1, "serverVersion", _s_set_serverVersion);
		Utils.RegisterFunc(L, -1, "updateType", _s_set_updateType);
		Utils.RegisterFunc(L, -1, "upload_video_url", _s_set_upload_video_url);
		Utils.RegisterFunc(L, -1, "xmlVersion", _s_set_xmlVersion);
		Utils.RegisterFunc(L, -1, "loginServerInfo", _s_set_loginServerInfo);
		Utils.RegisterFunc(L, -1, "gcmRegisterId", _s_set_gcmRegisterId);
		Utils.RegisterFunc(L, -1, "referrer", _s_set_referrer);
		Utils.RegisterFunc(L, -1, "deeplinkParams", _s_set_deeplinkParams);
		Utils.RegisterFunc(L, -1, "AndroidID", _s_set_AndroidID);
		Utils.RegisterFunc(L, -1, "IMEI", _s_set_IMEI);
		Utils.RegisterFunc(L, -1, "analyticID", _s_set_analyticID);
		Utils.RegisterFunc(L, -1, "s_isGooglePlayAvailable", _s_set_s_isGooglePlayAvailable);
		Utils.RegisterFunc(L, -1, "platformUID", _s_set_platformUID);
		Utils.RegisterFunc(L, -1, "parseRegisterId", _s_set_parseRegisterId);
		Utils.RegisterFunc(L, -1, "fromCountry", _s_set_fromCountry);
		Utils.RegisterFunc(L, -1, "gaid", _s_set_gaid);
		Utils.RegisterFunc(L, -1, "isTodayFirstLogin", _s_set_isTodayFirstLogin);
		Utils.RegisterFunc(L, -1, "isNewServer", _s_set_isNewServer);
		Utils.RegisterFunc(L, -1, "version", _s_set_version);
		Utils.RegisterFunc(L, -1, "uuid", _s_set_uuid);
		Utils.RegisterFunc(L, -1, "gaidCache", _s_set_gaidCache);
		Utils.RegisterFunc(L, -1, "isUploadPic", _s_set_isUploadPic);
		Utils.RegisterFunc(L, -1, "isOpenElvaChat", _s_set_isOpenElvaChat);
		Utils.RegisterFunc(L, -1, "isFAQVoteResp", _s_set_isFAQVoteResp);
		Utils.RegisterFunc(L, -1, "cityTileCountry", _s_set_cityTileCountry);
		Utils.RegisterFunc(L, -1, "serverType", _s_set_serverType);
		Utils.RegisterFunc(L, -1, "serverMax", _s_set_serverMax);
		Utils.RegisterFunc(L, -1, "nowGameCnt", _s_set_nowGameCnt);
		Utils.RegisterFunc(L, -1, "freeSpdT", _s_set_freeSpdT);
		Utils.RegisterFunc(L, -1, "TeleportLimitTime", _s_set_TeleportLimitTime);
		Utils.RegisterFunc(L, -1, "TransResForbiddenSwith", _s_set_TransResForbiddenSwith);
		Utils.RegisterFunc(L, -1, "NewTransKingdomLevel", _s_set_NewTransKingdomLevel);
		Utils.RegisterFunc(L, -1, "IsCityMoved", _s_set_IsCityMoved);
		Utils.RegisterFunc(L, -1, "pushOffWithQuitGame", _s_set_pushOffWithQuitGame);
		Utils.RegisterFunc(L, -1, "isInBackGround", _s_set_isInBackGround);
		Utils.RegisterFunc(L, -1, "gameLineBlackList", _s_set_gameLineBlackList);
		Utils.RegisterFunc(L, -1, "LoginServerError", _s_set_LoginServerError);
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
				GlobalDataManager o = new GlobalDataManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GlobalDataManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GlobalDataManager globalDataManager = (GlobalDataManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject dict = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			globalDataManager.Init(dict);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAnalyticID(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetAnalyticID();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetCnFlagFromServer(IntPtr L)
	{
		try
		{
			GlobalDataManager obj = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool cnFlagFromServer = Lua.lua_toboolean(L, 2);
			obj.SetCnFlagFromServer(cnFlagFromServer);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Reset(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Reset();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGlobalValue(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GlobalDataManager obj = (GlobalDataManager)objectTranslator.FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			object globalValue = obj.GetGlobalValue(key);
			objectTranslator.PushAny(L, globalValue);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGlobalValue(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GlobalDataManager globalDataManager = (GlobalDataManager)objectTranslator.FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			object value = objectTranslator.GetObject(L, 3, typeof(object));
			bool value2 = globalDataManager.SetGlobalValue(key, value);
			Lua.lua_pushboolean(L, value2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_recordGaid(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).recordGaid();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_isGoogle(IntPtr L)
	{
		try
		{
			bool value = ((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isGoogle();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_isGoogleOnlyCheckAnalyticID(IntPtr L)
	{
		try
		{
			bool value = ((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isGoogleOnlyCheckAnalyticID();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_isChina(IntPtr L)
	{
		try
		{
			bool value = ((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isChina();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_isMiddleEast(IntPtr L)
	{
		try
		{
			bool value = ((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isMiddleEast();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_isTencent(IntPtr L)
	{
		try
		{
			bool value = ((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isTencent();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_isAmazon(IntPtr L)
	{
		try
		{
			bool value = ((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isAmazon();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_isMol(IntPtr L)
	{
		try
		{
			bool value = ((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isMol();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_isMycard(IntPtr L)
	{
		try
		{
			bool value = ((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isMycard();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_isOnestore(IntPtr L)
	{
		try
		{
			bool value = ((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isOnestore();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_HasRequestAllProducts(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, globalDataManager.HasRequestAllProducts);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_download_video_url(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.download_video_url);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_download_video_url2(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.download_video_url2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_downloadurlcdn(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.downloadurlcdn);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_downloadurl(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.downloadurl);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_eu_state(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, globalDataManager.eu_state);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fblikeutil(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.fblikeutil);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_force_merge(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, globalDataManager.force_merge);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_force_use_downloadxml(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, globalDataManager.force_use_downloadxml);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lua(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.lua);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_luaCode(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.luaCode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_luaCode_v3(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.luaCode_v3);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_luaSize(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, globalDataManager.luaSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_luaVersion(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.luaVersion);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_luaVersion_v3(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.luaVersion_v3);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_luazipSize(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, globalDataManager.luazipSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_randKey(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, globalDataManager.randKey);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_reduce_init_data(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, globalDataManager.reduce_init_data);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_serverVersion(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.serverVersion);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_updateType(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, globalDataManager.updateType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_upload_video_url(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.upload_video_url);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_xmlVersion(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.xmlVersion);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_loginServerInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GlobalDataManager globalDataManager = (GlobalDataManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, globalDataManager.loginServerInfo);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_gcmRegisterId(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.gcmRegisterId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_referrer(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.referrer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_deeplinkParams(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.deeplinkParams);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AndroidID(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.AndroidID);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IMEI(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.IMEI);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_analyticID(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.analyticID);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_s_isGooglePlayAvailable(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, globalDataManager.s_isGooglePlayAvailable);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_platformUID(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.platformUID);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_parseRegisterId(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.parseRegisterId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fromCountry(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.fromCountry);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_gaid(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.gaid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isTodayFirstLogin(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, globalDataManager.isTodayFirstLogin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isNewServer(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, globalDataManager.isNewServer);
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
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.version);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uuid(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.uuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_gaidCache(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, globalDataManager.gaidCache);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isUploadPic(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, globalDataManager.isUploadPic);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isOpenElvaChat(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, globalDataManager.isOpenElvaChat);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isFAQVoteResp(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, globalDataManager.isFAQVoteResp);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cityTileCountry(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, globalDataManager.cityTileCountry);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_serverType(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, globalDataManager.serverType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_serverMax(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, globalDataManager.serverMax);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_nowGameCnt(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, globalDataManager.nowGameCnt);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_freeSpdT(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, globalDataManager.freeSpdT);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TeleportLimitTime(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, globalDataManager.TeleportLimitTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TransResForbiddenSwith(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, globalDataManager.TransResForbiddenSwith);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_NewTransKingdomLevel(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, globalDataManager.NewTransKingdomLevel);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsCityMoved(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, globalDataManager.IsCityMoved);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pushOffWithQuitGame(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, globalDataManager.pushOffWithQuitGame);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isInBackGround(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, globalDataManager.isInBackGround);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_gameLineBlackList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GlobalDataManager globalDataManager = (GlobalDataManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, globalDataManager.gameLineBlackList);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LoginServerError(IntPtr L)
	{
		try
		{
			GlobalDataManager globalDataManager = (GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, globalDataManager.LoginServerError);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_HasRequestAllProducts(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).HasRequestAllProducts = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_download_video_url(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).download_video_url = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_download_video_url2(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).download_video_url2 = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_downloadurlcdn(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).downloadurlcdn = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_downloadurl(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).downloadurl = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_eu_state(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).eu_state = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fblikeutil(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).fblikeutil = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_force_merge(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).force_merge = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_force_use_downloadxml(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).force_use_downloadxml = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lua(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).lua = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_luaCode(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).luaCode = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_luaCode_v3(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).luaCode_v3 = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_luaSize(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).luaSize = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_luaVersion(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).luaVersion = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_luaVersion_v3(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).luaVersion_v3 = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_luazipSize(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).luazipSize = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_randKey(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).randKey = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_reduce_init_data(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).reduce_init_data = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_serverVersion(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).serverVersion = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_updateType(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).updateType = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_upload_video_url(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).upload_video_url = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_xmlVersion(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).xmlVersion = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_loginServerInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GlobalDataManager globalDataManager = (GlobalDataManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out GlobalDataManager.LoginServerInfo v);
			globalDataManager.loginServerInfo = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_gcmRegisterId(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).gcmRegisterId = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_referrer(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).referrer = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_deeplinkParams(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).deeplinkParams = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_AndroidID(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).AndroidID = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IMEI(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IMEI = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_analyticID(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).analyticID = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_s_isGooglePlayAvailable(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).s_isGooglePlayAvailable = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_platformUID(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).platformUID = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_parseRegisterId(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).parseRegisterId = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fromCountry(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).fromCountry = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_gaid(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).gaid = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isTodayFirstLogin(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isTodayFirstLogin = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isNewServer(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isNewServer = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_version(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).version = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_uuid(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).uuid = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_gaidCache(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).gaidCache = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isUploadPic(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isUploadPic = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isOpenElvaChat(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isOpenElvaChat = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isFAQVoteResp(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isFAQVoteResp = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cityTileCountry(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).cityTileCountry = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_serverType(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).serverType = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_serverMax(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).serverMax = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_nowGameCnt(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).nowGameCnt = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_freeSpdT(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).freeSpdT = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_TeleportLimitTime(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).TeleportLimitTime = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_TransResForbiddenSwith(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).TransResForbiddenSwith = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_NewTransKingdomLevel(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).NewTransKingdomLevel = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsCityMoved(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsCityMoved = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pushOffWithQuitGame(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).pushOffWithQuitGame = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isInBackGround(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isInBackGround = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_gameLineBlackList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((GlobalDataManager)objectTranslator.FastGetCSObj(L, 1)).gameLineBlackList = (HashSet<string>)objectTranslator.GetObject(L, 2, typeof(HashSet<string>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_LoginServerError(IntPtr L)
	{
		try
		{
			((GlobalDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LoginServerError = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
