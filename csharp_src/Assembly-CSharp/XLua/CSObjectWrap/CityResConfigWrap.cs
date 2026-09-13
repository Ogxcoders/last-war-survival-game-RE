using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class CityResConfigWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CityResConfig);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 10, 10);
		Utils.RegisterFunc(L, -2, "objId", _g_get_objId);
		Utils.RegisterFunc(L, -2, "resType", _g_get_resType);
		Utils.RegisterFunc(L, -2, "maxBlood", _g_get_maxBlood);
		Utils.RegisterFunc(L, -2, "refreshCd", _g_get_refreshCd);
		Utils.RegisterFunc(L, -2, "buffId", _g_get_buffId);
		Utils.RegisterFunc(L, -2, "outBuffRate", _g_get_outBuffRate);
		Utils.RegisterFunc(L, -2, "superRate", _g_get_superRate);
		Utils.RegisterFunc(L, -2, "outSuperBuffRate", _g_get_outSuperBuffRate);
		Utils.RegisterFunc(L, -2, "extraParaFloat", _g_get_extraParaFloat);
		Utils.RegisterFunc(L, -2, "extraParaString", _g_get_extraParaString);
		Utils.RegisterFunc(L, -1, "objId", _s_set_objId);
		Utils.RegisterFunc(L, -1, "resType", _s_set_resType);
		Utils.RegisterFunc(L, -1, "maxBlood", _s_set_maxBlood);
		Utils.RegisterFunc(L, -1, "refreshCd", _s_set_refreshCd);
		Utils.RegisterFunc(L, -1, "buffId", _s_set_buffId);
		Utils.RegisterFunc(L, -1, "outBuffRate", _s_set_outBuffRate);
		Utils.RegisterFunc(L, -1, "superRate", _s_set_superRate);
		Utils.RegisterFunc(L, -1, "outSuperBuffRate", _s_set_outSuperBuffRate);
		Utils.RegisterFunc(L, -1, "extraParaFloat", _s_set_extraParaFloat);
		Utils.RegisterFunc(L, -1, "extraParaString", _s_set_extraParaString);
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
				CityResConfig o = new CityResConfig();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityResConfig constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_objId(IntPtr L)
	{
		try
		{
			CityResConfig cityResConfig = (CityResConfig)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, cityResConfig.objId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_resType(IntPtr L)
	{
		try
		{
			CityResConfig cityResConfig = (CityResConfig)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, cityResConfig.resType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxBlood(IntPtr L)
	{
		try
		{
			CityResConfig cityResConfig = (CityResConfig)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, cityResConfig.maxBlood);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_refreshCd(IntPtr L)
	{
		try
		{
			CityResConfig cityResConfig = (CityResConfig)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, cityResConfig.refreshCd);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_buffId(IntPtr L)
	{
		try
		{
			CityResConfig cityResConfig = (CityResConfig)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, cityResConfig.buffId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_outBuffRate(IntPtr L)
	{
		try
		{
			CityResConfig cityResConfig = (CityResConfig)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, cityResConfig.outBuffRate);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_superRate(IntPtr L)
	{
		try
		{
			CityResConfig cityResConfig = (CityResConfig)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, cityResConfig.superRate);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_outSuperBuffRate(IntPtr L)
	{
		try
		{
			CityResConfig cityResConfig = (CityResConfig)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, cityResConfig.outSuperBuffRate);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_extraParaFloat(IntPtr L)
	{
		try
		{
			CityResConfig cityResConfig = (CityResConfig)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, cityResConfig.extraParaFloat);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_extraParaString(IntPtr L)
	{
		try
		{
			CityResConfig cityResConfig = (CityResConfig)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, cityResConfig.extraParaString);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_objId(IntPtr L)
	{
		try
		{
			((CityResConfig)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).objId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_resType(IntPtr L)
	{
		try
		{
			((CityResConfig)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).resType = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maxBlood(IntPtr L)
	{
		try
		{
			((CityResConfig)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).maxBlood = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_refreshCd(IntPtr L)
	{
		try
		{
			((CityResConfig)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).refreshCd = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_buffId(IntPtr L)
	{
		try
		{
			((CityResConfig)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).buffId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_outBuffRate(IntPtr L)
	{
		try
		{
			((CityResConfig)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).outBuffRate = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_superRate(IntPtr L)
	{
		try
		{
			((CityResConfig)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).superRate = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_outSuperBuffRate(IntPtr L)
	{
		try
		{
			((CityResConfig)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).outSuperBuffRate = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_extraParaFloat(IntPtr L)
	{
		try
		{
			((CityResConfig)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).extraParaFloat = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_extraParaString(IntPtr L)
	{
		try
		{
			((CityResConfig)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).extraParaString = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
