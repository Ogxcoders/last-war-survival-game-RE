using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class NetworkURLConfigWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(NetworkURLConfig);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 12, 8, 1);
		Utils.RegisterFunc(L, -4, "RefreshType", _m_RefreshType_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetURLGroupEnv", _m_SetURLGroupEnv_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetHostListByCurGroupType", _m_GetHostListByCurGroupType_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetCheckVersionURL", _m_GetCheckVersionURL_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetDownloadURL", _m_GetDownloadURL_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetBattleReportHostByCurGroupType", _m_GetBattleReportHostByCurGroupType_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetBattleReportDownloadHostByCurGroupType", _m_GetBattleReportDownloadHostByCurGroupType_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetMailRankDataHostByCurGroupType", _m_GetMailRankDataHostByCurGroupType_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsNeedSkipUpdate", _m_IsNeedSkipUpdate_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetBattleReportOSSURLType", _m_GetBattleReportOSSURLType_xlua_st_);
		Utils.RegisterFunc(L, -4, "ModifyAddressStr", _m_ModifyAddressStr_xlua_st_);
		Utils.RegisterFunc(L, -2, "URLGroupType", _g_get_URLGroupType);
		Utils.RegisterFunc(L, -2, "IsOnline", _g_get_IsOnline);
		Utils.RegisterFunc(L, -2, "IsLocal", _g_get_IsLocal);
		Utils.RegisterFunc(L, -2, "IsPressureTest", _g_get_IsPressureTest);
		Utils.RegisterFunc(L, -2, "IsAWS", _g_get_IsAWS);
		Utils.RegisterFunc(L, -2, "IsChangeDebugURLGroup", _g_get_IsChangeDebugURLGroup);
		Utils.RegisterFunc(L, -2, "PackageName", _g_get_PackageName);
		Utils.RegisterFunc(L, -2, "DownloadURL", _g_get_DownloadURL);
		Utils.RegisterFunc(L, -1, "IsChangeDebugURLGroup", _s_set_IsChangeDebugURLGroup);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "NetworkURLConfig does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshType_xlua_st_(IntPtr L)
	{
		try
		{
			NetworkURLConfig.RefreshType();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetURLGroupEnv_xlua_st_(IntPtr L)
	{
		try
		{
			NetworkURLConfig.SetURLGroupEnv(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetHostListByCurGroupType_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string[] hostListByCurGroupType = NetworkURLConfig.GetHostListByCurGroupType();
			objectTranslator.Push(L, hostListByCurGroupType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCheckVersionURL_xlua_st_(IntPtr L)
	{
		try
		{
			string checkVersionURL = NetworkURLConfig.GetCheckVersionURL(Lua.lua_tostring(L, 1));
			Lua.lua_pushstring(L, checkVersionURL);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDownloadURL_xlua_st_(IntPtr L)
	{
		try
		{
			string downloadURL = NetworkURLConfig.GetDownloadURL(Lua.lua_tostring(L, 1));
			Lua.lua_pushstring(L, downloadURL);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBattleReportHostByCurGroupType_xlua_st_(IntPtr L)
	{
		try
		{
			bool forceOnline = Lua.lua_toboolean(L, 1);
			bool isFull = Lua.lua_toboolean(L, 2);
			bool isAddressMode = Lua.lua_toboolean(L, 3);
			string address = Lua.lua_tostring(L, 4);
			string battleReportHostByCurGroupType = NetworkURLConfig.GetBattleReportHostByCurGroupType(forceOnline, isFull, isAddressMode, address);
			Lua.lua_pushstring(L, battleReportHostByCurGroupType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBattleReportDownloadHostByCurGroupType_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				bool isAddressMode = Lua.lua_toboolean(L, 1);
				string address = Lua.lua_tostring(L, 2);
				string battleReportDownloadHostByCurGroupType = NetworkURLConfig.GetBattleReportDownloadHostByCurGroupType(isAddressMode, address);
				Lua.lua_pushstring(L, battleReportDownloadHostByCurGroupType);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				bool isAddressMode2 = Lua.lua_toboolean(L, 1);
				string address2 = Lua.lua_tostring(L, 2);
				bool forceUseOnline = Lua.lua_toboolean(L, 3);
				string battleReportDownloadHostByCurGroupType2 = NetworkURLConfig.GetBattleReportDownloadHostByCurGroupType(isAddressMode2, address2, forceUseOnline);
				Lua.lua_pushstring(L, battleReportDownloadHostByCurGroupType2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to NetworkURLConfig.GetBattleReportDownloadHostByCurGroupType!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMailRankDataHostByCurGroupType_xlua_st_(IntPtr L)
	{
		try
		{
			bool isAddressMode = Lua.lua_toboolean(L, 1);
			string address = Lua.lua_tostring(L, 2);
			string mailRankDataHostByCurGroupType = NetworkURLConfig.GetMailRankDataHostByCurGroupType(isAddressMode, address);
			Lua.lua_pushstring(L, mailRankDataHostByCurGroupType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsNeedSkipUpdate_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = NetworkURLConfig.IsNeedSkipUpdate();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBattleReportOSSURLType_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BattleReportOSSURLType battleReportOSSURLType = NetworkURLConfig.GetBattleReportOSSURLType(Lua.lua_tostring(L, 1));
			objectTranslator.Push(L, battleReportOSSURLType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ModifyAddressStr_xlua_st_(IntPtr L)
	{
		try
		{
			string str = NetworkURLConfig.ModifyAddressStr(Lua.lua_tostring(L, 1));
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_URLGroupType(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushURLGroupType(L, NetworkURLConfig.URLGroupType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsOnline(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, NetworkURLConfig.IsOnline);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsLocal(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, NetworkURLConfig.IsLocal);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsPressureTest(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, NetworkURLConfig.IsPressureTest);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsAWS(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, NetworkURLConfig.IsAWS);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsChangeDebugURLGroup(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, NetworkURLConfig.IsChangeDebugURLGroup);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_PackageName(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, NetworkURLConfig.PackageName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DownloadURL(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, NetworkURLConfig.DownloadURL);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsChangeDebugURLGroup(IntPtr L)
	{
		try
		{
			NetworkURLConfig.IsChangeDebugURLGroup = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
