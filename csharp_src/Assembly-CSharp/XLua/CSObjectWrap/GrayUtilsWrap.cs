using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GrayUtilsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GrayUtils);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 5, 3, 1);
		Utils.RegisterFunc(L, -4, "InGrayServer", _m_InGrayServer_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsGrayDevice", _m_IsGrayDevice_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetPercentageFromMd5", _m_GetPercentageFromMd5_xlua_st_);
		Utils.RegisterFunc(L, -4, "CheckLuaSwitchWithServerFormat", _m_CheckLuaSwitchWithServerFormat_xlua_st_);
		Utils.RegisterFunc(L, -2, "isGrayServer", _g_get_isGrayServer);
		Utils.RegisterFunc(L, -2, "isGM", _g_get_isGM);
		Utils.RegisterFunc(L, -2, "hasFileLog", _g_get_hasFileLog);
		Utils.RegisterFunc(L, -1, "hasFileLog", _s_set_hasFileLog);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "GrayUtils does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InGrayServer_xlua_st_(IntPtr L)
	{
		try
		{
			int start = Lua.xlua_tointeger(L, 1);
			int end = Lua.xlua_tointeger(L, 2);
			bool value = GrayUtils.InGrayServer(start, end);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsGrayDevice_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				int percent = Lua.xlua_tointeger(L, 1);
				bool gmAlways = Lua.lua_toboolean(L, 2);
				bool value = GrayUtils.IsGrayDevice(percent, gmAlways);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 1 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1))
			{
				bool value2 = GrayUtils.IsGrayDevice(Lua.xlua_tointeger(L, 1));
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
			if (num == 0)
			{
				bool value3 = GrayUtils.IsGrayDevice();
				Lua.lua_pushboolean(L, value3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GrayUtils.IsGrayDevice!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPercentageFromMd5_xlua_st_(IntPtr L)
	{
		try
		{
			double percentageFromMd = GrayUtils.GetPercentageFromMd5(Lua.lua_tostring(L, 1));
			Lua.lua_pushnumber(L, percentageFromMd);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckLuaSwitchWithServerFormat_xlua_st_(IntPtr L)
	{
		try
		{
			string key = Lua.lua_tostring(L, 1);
			string param = Lua.lua_tostring(L, 2);
			int serverId = Lua.xlua_tointeger(L, 3);
			bool value = GrayUtils.CheckLuaSwitchWithServerFormat(key, param, serverId);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isGrayServer(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, GrayUtils.isGrayServer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isGM(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, GrayUtils.isGM);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_hasFileLog(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, GrayUtils.hasFileLog);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_hasFileLog(IntPtr L)
	{
		try
		{
			GrayUtils.hasFileLog = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
