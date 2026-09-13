using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LuaArrAccessAPIWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(LuaArrAccessAPI);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 5, 1, 1);
		Utils.RegisterFunc(L, -4, "RegisterPinFunc", _m_RegisterPinFunc_xlua_st_);
		Utils.RegisterFunc(L, -4, "PinFunction", _m_PinFunction_xlua_st_);
		Utils.RegisterFunc(L, -4, "Init", _m_Init_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateLuaShareAccess", _m_CreateLuaShareAccess_xlua_st_);
		Utils.RegisterFunc(L, -2, "IsLuajit", _g_get_IsLuajit);
		Utils.RegisterFunc(L, -1, "IsLuajit", _s_set_IsLuajit);
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
				LuaArrAccessAPI o = new LuaArrAccessAPI();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LuaArrAccessAPI constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RegisterPinFunc_xlua_st_(IntPtr L)
	{
		try
		{
			LuaArrAccessAPI.RegisterPinFunc(Lua.lua_touserdata(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PinFunction_xlua_st_(IntPtr L)
	{
		try
		{
			int value = LuaArrAccessAPI.PinFunction(Lua.lua_touserdata(L, 1));
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init_xlua_st_(IntPtr L)
	{
		try
		{
			LuaArrAccessAPI.Init(Lua.lua_toboolean(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateLuaShareAccess_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LuaArrAccess o = LuaArrAccessAPI.CreateLuaShareAccess();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsLuajit(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, LuaArrAccessAPI.IsLuajit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsLuajit(IntPtr L)
	{
		try
		{
			LuaArrAccessAPI.IsLuajit = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
