using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ClientSwitchWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ClientSwitch);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 53, 0, 0);
		Utils.RegisterFunc(L, -4, "Parse", _m_Parse_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsOn", _m_IsOn_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsCacheOn", _m_IsCacheOn_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsOff", _m_IsOff_xlua_st_);
		Utils.RegisterFunc(L, -4, "HasSwitchData", _m_HasSwitchData_xlua_st_);
		Utils.RegisterFunc(L, -4, "ForceSetSwitch", _m_ForceSetSwitch_xlua_st_);
		Utils.RegisterFunc(L, -4, "ForceSetSwitchAndSave", _m_ForceSetSwitchAndSave_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "ENABLE_TABLE_PATCH", 0);
		Utils.RegisterObject(L, translator, -4, "ENABLE_UNUSE_NOTDONE_ASSET", 1);
		Utils.RegisterObject(L, translator, -4, "ENABLE_BUNDLE_FAST_VALIDATION", 2);
		Utils.RegisterObject(L, translator, -4, "ENABLE_WORLD_DYNAMIC_POOL", 3);
		Utils.RegisterObject(L, translator, -4, "DISABLE_NEW_NET_PACKET", 4);
		Utils.RegisterObject(L, translator, -4, "DISABLE_ZSTD", 5);
		Utils.RegisterObject(L, translator, -4, "ENABLE_SMART_FOX_CROSS_SERVER", 8);
		Utils.RegisterObject(L, translator, -4, "ENABLE_DELAY_PAUSE_RESUME", 10);
		Utils.RegisterObject(L, translator, -4, "ENABLE_SAMSUNG_SOFTKEYBOARD_MOD", 11);
		Utils.RegisterObject(L, translator, -4, "ENABLE_LW_LUA_BINARY_PATCH", 12);
		Utils.RegisterObject(L, translator, -4, "ENABLE_FIX_DOWNLOADSIZE", 13);
		Utils.RegisterObject(L, translator, -4, "ENABLE_COPPA_VERIFY", 14);
		Utils.RegisterObject(L, translator, -4, "ENABLE_DISPOSE_OLD_LUA_ENV", 15);
		Utils.RegisterObject(L, translator, -4, "DISABLE_BUNDLE_ALIAS", 17);
		Utils.RegisterObject(L, translator, -4, "ENABLE_ASSET_LOAD_CACHE", 18);
		Utils.RegisterObject(L, translator, -4, "ENABLE_TRACEROUTE", 19);
		Utils.RegisterObject(L, translator, -4, "ENABLE_CDN_SPEED_TEST", 20);
		Utils.RegisterObject(L, translator, -4, "ENABLE_ACCOUNT_SELECT_STATE", 21);
		Utils.RegisterObject(L, translator, -4, "ENABLE_ASSETS_USE_TRACK", 22);
		Utils.RegisterObject(L, translator, -4, "ENABLE_BUNDLE_USE_TRACK", 23);
		Utils.RegisterObject(L, translator, -4, "DISABLE_FIRST_LAUNCH_SKIP_UPDATE", 24);
		Utils.RegisterObject(L, translator, -4, "ENABLE_SHUMEI_SDK", 25);
		Utils.RegisterObject(L, translator, -4, "ENABLE_SERVER_WS_CONNECTION", 26);
		Utils.RegisterObject(L, translator, -4, "SM_INPUT_HEIGHT", 27);
		Utils.RegisterObject(L, translator, -4, "LUA_TABLE_UTIL_CONTAINS_KEY_USE_NEW_JUDGE", 28);
		Utils.RegisterObject(L, translator, -4, "ENABLE_CROSS_FORWARD", 29);
		Utils.RegisterObject(L, translator, -4, "ENABLE_ASYNC_LOAD_IMAGE_CALLBACK_CHECK", 30);
		Utils.RegisterObject(L, translator, -4, "EVENT_FRAME_BLEND", 32);
		Utils.RegisterObject(L, translator, -4, "ENABLE_SOUND_RESOURCE_DOWNLOAD", 33);
		Utils.RegisterObject(L, translator, -4, "ENABLE_DYNAMIC_ATLAS_V2", 34);
		Utils.RegisterObject(L, translator, -4, "ENABLE_REPLACE_GPU_SKIN_PREFAB", 35);
		Utils.RegisterObject(L, translator, -4, "ENABLE_SHUMEI_CREATE_ACCOUNT_RISK_BAN", 36);
		Utils.RegisterObject(L, translator, -4, "ENABLE_WORLD_MARCH_STEP_EXECUTE", 37);
		Utils.RegisterObject(L, translator, -4, "ENABLE_BUNDLE_LOAD_TRACK", 38);
		Utils.RegisterObject(L, translator, -4, "ENABLE_WORLD_MARCH_MESSAGE_MERGE", 39);
		Utils.RegisterObject(L, translator, -4, "ENABLE_WORLD_MARCH_MESSAGE_STEP", 40);
		Utils.RegisterObject(L, translator, -4, "ENABLE_NEW_GRAPHIC_LEVEL", 41);
		Utils.RegisterObject(L, translator, -4, "ENABLE_OPT_PVE_RVO", 42);
		Utils.RegisterObject(L, translator, -4, "ENABLE_PAY_PRODUCT_DATA_PATCH", 43);
		Utils.RegisterObject(L, translator, -4, "ENABLE_CONNECT_GAME_WEBSOCKET_FALLBACK", 44);
		Utils.RegisterObject(L, translator, -4, "INPUT_FIX_ISON", 45);
		Utils.RegisterObject(L, translator, -4, "ENABLE_IOS_NATIVE_SOCKET", 46);
		Utils.RegisterObject(L, translator, -4, "ENABLE_PC_UNINSTALL_ACE_DRIVERS", 47);
		Utils.RegisterObject(L, translator, -4, "ENABLE_PARALLEL_INIT", 48);
		Utils.RegisterObject(L, translator, -4, "ENABLE_DISPOSE_OLD_LUA_ENV_KEY", "ENABLE_DISPOSE_OLD_LUA_ENV_KEY");
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
				ClientSwitch o = new ClientSwitch();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ClientSwitch constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Parse_xlua_st_(IntPtr L)
	{
		try
		{
			ClientSwitch.Parse(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsOn_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = ClientSwitch.IsOn(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsCacheOn_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = ClientSwitch.IsCacheOn(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsOff_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = ClientSwitch.IsOff(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HasSwitchData_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = ClientSwitch.HasSwitchData(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ForceSetSwitch_xlua_st_(IntPtr L)
	{
		try
		{
			int index = Lua.xlua_tointeger(L, 1);
			bool isOn = Lua.lua_toboolean(L, 2);
			ClientSwitch.ForceSetSwitch(index, isOn);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ForceSetSwitchAndSave_xlua_st_(IntPtr L)
	{
		try
		{
			int index = Lua.xlua_tointeger(L, 1);
			bool isOn = Lua.lua_toboolean(L, 2);
			ClientSwitch.ForceSetSwitchAndSave(index, isOn);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
