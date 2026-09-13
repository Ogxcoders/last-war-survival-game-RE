using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LoginServerInfoWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(LoginServerInfo);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 17, 17);
		Utils.RegisterFunc(L, -2, "id", _g_get_id);
		Utils.RegisterFunc(L, -2, "name", _g_get_name);
		Utils.RegisterFunc(L, -2, "ip", _g_get_ip);
		Utils.RegisterFunc(L, -2, "ws_ip", _g_get_ws_ip);
		Utils.RegisterFunc(L, -2, "inner_port", _g_get_inner_port);
		Utils.RegisterFunc(L, -2, "port", _g_get_port);
		Utils.RegisterFunc(L, -2, "zone", _g_get_zone);
		Utils.RegisterFunc(L, -2, "cnserver", _g_get_cnserver);
		Utils.RegisterFunc(L, -2, "server_type", _g_get_server_type);
		Utils.RegisterFunc(L, -2, "gameUid", _g_get_gameUid);
		Utils.RegisterFunc(L, -2, "uid", _g_get_uid);
		Utils.RegisterFunc(L, -2, "merge", _g_get_merge);
		Utils.RegisterFunc(L, -2, "cnToOther", _g_get_cnToOther);
		Utils.RegisterFunc(L, -2, "daoliang_time", _g_get_daoliang_time);
		Utils.RegisterFunc(L, -2, "ptype", _g_get_ptype);
		Utils.RegisterFunc(L, -2, "status", _g_get_status);
		Utils.RegisterFunc(L, -2, "package_seperate_info", _g_get_package_seperate_info);
		Utils.RegisterFunc(L, -1, "id", _s_set_id);
		Utils.RegisterFunc(L, -1, "name", _s_set_name);
		Utils.RegisterFunc(L, -1, "ip", _s_set_ip);
		Utils.RegisterFunc(L, -1, "ws_ip", _s_set_ws_ip);
		Utils.RegisterFunc(L, -1, "inner_port", _s_set_inner_port);
		Utils.RegisterFunc(L, -1, "port", _s_set_port);
		Utils.RegisterFunc(L, -1, "zone", _s_set_zone);
		Utils.RegisterFunc(L, -1, "cnserver", _s_set_cnserver);
		Utils.RegisterFunc(L, -1, "server_type", _s_set_server_type);
		Utils.RegisterFunc(L, -1, "gameUid", _s_set_gameUid);
		Utils.RegisterFunc(L, -1, "uid", _s_set_uid);
		Utils.RegisterFunc(L, -1, "merge", _s_set_merge);
		Utils.RegisterFunc(L, -1, "cnToOther", _s_set_cnToOther);
		Utils.RegisterFunc(L, -1, "daoliang_time", _s_set_daoliang_time);
		Utils.RegisterFunc(L, -1, "ptype", _s_set_ptype);
		Utils.RegisterFunc(L, -1, "status", _s_set_status);
		Utils.RegisterFunc(L, -1, "package_seperate_info", _s_set_package_seperate_info);
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
				LoginServerInfo o = new LoginServerInfo();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LoginServerInfo constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_id(IntPtr L)
	{
		try
		{
			LoginServerInfo loginServerInfo = (LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, loginServerInfo.id);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_name(IntPtr L)
	{
		try
		{
			LoginServerInfo loginServerInfo = (LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, loginServerInfo.name);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ip(IntPtr L)
	{
		try
		{
			LoginServerInfo loginServerInfo = (LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, loginServerInfo.ip);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ws_ip(IntPtr L)
	{
		try
		{
			LoginServerInfo loginServerInfo = (LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, loginServerInfo.ws_ip);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_inner_port(IntPtr L)
	{
		try
		{
			LoginServerInfo loginServerInfo = (LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, loginServerInfo.inner_port);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_port(IntPtr L)
	{
		try
		{
			LoginServerInfo loginServerInfo = (LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, loginServerInfo.port);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_zone(IntPtr L)
	{
		try
		{
			LoginServerInfo loginServerInfo = (LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, loginServerInfo.zone);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cnserver(IntPtr L)
	{
		try
		{
			LoginServerInfo loginServerInfo = (LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, loginServerInfo.cnserver);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_server_type(IntPtr L)
	{
		try
		{
			LoginServerInfo loginServerInfo = (LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, loginServerInfo.server_type);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_gameUid(IntPtr L)
	{
		try
		{
			LoginServerInfo loginServerInfo = (LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, loginServerInfo.gameUid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uid(IntPtr L)
	{
		try
		{
			LoginServerInfo loginServerInfo = (LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, loginServerInfo.uid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_merge(IntPtr L)
	{
		try
		{
			LoginServerInfo loginServerInfo = (LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, loginServerInfo.merge);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cnToOther(IntPtr L)
	{
		try
		{
			LoginServerInfo loginServerInfo = (LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, loginServerInfo.cnToOther);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_daoliang_time(IntPtr L)
	{
		try
		{
			LoginServerInfo loginServerInfo = (LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, loginServerInfo.daoliang_time);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ptype(IntPtr L)
	{
		try
		{
			LoginServerInfo loginServerInfo = (LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, loginServerInfo.ptype);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_status(IntPtr L)
	{
		try
		{
			LoginServerInfo loginServerInfo = (LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, loginServerInfo.status);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_package_seperate_info(IntPtr L)
	{
		try
		{
			LoginServerInfo loginServerInfo = (LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, loginServerInfo.package_seperate_info);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_id(IntPtr L)
	{
		try
		{
			((LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).id = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_name(IntPtr L)
	{
		try
		{
			((LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).name = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ip(IntPtr L)
	{
		try
		{
			((LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ip = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ws_ip(IntPtr L)
	{
		try
		{
			((LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ws_ip = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_inner_port(IntPtr L)
	{
		try
		{
			((LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).inner_port = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_port(IntPtr L)
	{
		try
		{
			((LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).port = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_zone(IntPtr L)
	{
		try
		{
			((LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).zone = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cnserver(IntPtr L)
	{
		try
		{
			((LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).cnserver = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_server_type(IntPtr L)
	{
		try
		{
			((LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).server_type = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_gameUid(IntPtr L)
	{
		try
		{
			((LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).gameUid = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_uid(IntPtr L)
	{
		try
		{
			((LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).uid = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_merge(IntPtr L)
	{
		try
		{
			((LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).merge = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cnToOther(IntPtr L)
	{
		try
		{
			((LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).cnToOther = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_daoliang_time(IntPtr L)
	{
		try
		{
			((LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).daoliang_time = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ptype(IntPtr L)
	{
		try
		{
			((LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ptype = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_status(IntPtr L)
	{
		try
		{
			((LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).status = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_package_seperate_info(IntPtr L)
	{
		try
		{
			((LoginServerInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).package_seperate_info = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
