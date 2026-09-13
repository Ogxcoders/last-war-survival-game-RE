using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ClientConfigStaticWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ClientConfigStatic);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 3, 1, 1);
		Utils.RegisterFunc(L, -4, "SetPushABTestGroup", _m_SetPushABTestGroup_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetPushABTestGroup", _m_GetPushABTestGroup_xlua_st_);
		Utils.RegisterFunc(L, -2, "PushABTestGroup", _g_get_PushABTestGroup);
		Utils.RegisterFunc(L, -1, "PushABTestGroup", _s_set_PushABTestGroup);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "ClientConfigStatic does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPushABTestGroup_xlua_st_(IntPtr L)
	{
		try
		{
			string deviceID = Lua.lua_tostring(L, 1);
			int value = Lua.xlua_tointeger(L, 2);
			ClientConfigStatic.SetPushABTestGroup(deviceID, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPushABTestGroup_xlua_st_(IntPtr L)
	{
		try
		{
			int pushABTestGroup = ClientConfigStatic.GetPushABTestGroup(Lua.lua_tostring(L, 1));
			Lua.xlua_pushinteger(L, pushABTestGroup);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_PushABTestGroup(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, ClientConfigStatic.PushABTestGroup);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_PushABTestGroup(IntPtr L)
	{
		try
		{
			ClientConfigStatic.PushABTestGroup = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
