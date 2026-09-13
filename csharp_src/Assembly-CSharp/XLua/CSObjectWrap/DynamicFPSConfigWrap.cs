using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DynamicFPSConfigWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(DynamicFPSConfig);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 10, 2, 0);
		Utils.RegisterFunc(L, -4, "Initialize", _m_Initialize_xlua_st_);
		Utils.RegisterFunc(L, -4, "AcquireHighFPSLocker", _m_AcquireHighFPSLocker_xlua_st_);
		Utils.RegisterFunc(L, -4, "FreeHighFPSLocker", _m_FreeHighFPSLocker_xlua_st_);
		Utils.RegisterFunc(L, -4, "AcquireHighFPSLockerForSeconds", _m_AcquireHighFPSLockerForSeconds_xlua_st_);
		Utils.RegisterFunc(L, -4, "AcquireHighFPSLockerForChildrenScrollComponents", _m_AcquireHighFPSLockerForChildrenScrollComponents_xlua_st_);
		Utils.RegisterFunc(L, -4, "FreeHighFPSLockerForChildrenScrollComponents", _m_FreeHighFPSLockerForChildrenScrollComponents_xlua_st_);
		Utils.RegisterFunc(L, -4, "AcquireHighFPSLockerGameObject", _m_AcquireHighFPSLockerGameObject_xlua_st_);
		Utils.RegisterFunc(L, -4, "FreeHighFPSLockerForGameObject", _m_FreeHighFPSLockerForGameObject_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "INVALID_ID", -1);
		Utils.RegisterFunc(L, -2, "enabled", _g_get_enabled);
		Utils.RegisterFunc(L, -2, "nLocker", _g_get_nLocker);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "DynamicFPSConfig does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Initialize_xlua_st_(IntPtr L)
	{
		try
		{
			int normalFPS = Lua.xlua_tointeger(L, 1);
			int highFPS = Lua.xlua_tointeger(L, 2);
			bool defaultHighFPS = Lua.lua_toboolean(L, 3);
			DynamicFPSConfig.Initialize(normalFPS, highFPS, defaultHighFPS);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AcquireHighFPSLocker_xlua_st_(IntPtr L)
	{
		try
		{
			int value = DynamicFPSConfig.AcquireHighFPSLocker();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FreeHighFPSLocker_xlua_st_(IntPtr L)
	{
		try
		{
			int value = DynamicFPSConfig.FreeHighFPSLocker(Lua.xlua_tointeger(L, 1));
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AcquireHighFPSLockerForSeconds_xlua_st_(IntPtr L)
	{
		try
		{
			DynamicFPSConfig.AcquireHighFPSLockerForSeconds((float)Lua.lua_tonumber(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AcquireHighFPSLockerForChildrenScrollComponents_xlua_st_(IntPtr L)
	{
		try
		{
			DynamicFPSConfig.AcquireHighFPSLockerForChildrenScrollComponents((GameObject)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(GameObject)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FreeHighFPSLockerForChildrenScrollComponents_xlua_st_(IntPtr L)
	{
		try
		{
			DynamicFPSConfig.FreeHighFPSLockerForChildrenScrollComponents((GameObject)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(GameObject)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AcquireHighFPSLockerGameObject_xlua_st_(IntPtr L)
	{
		try
		{
			DynamicFPSConfig.AcquireHighFPSLockerGameObject((GameObject)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(GameObject)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FreeHighFPSLockerForGameObject_xlua_st_(IntPtr L)
	{
		try
		{
			DynamicFPSConfig.FreeHighFPSLockerForGameObject((GameObject)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(GameObject)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enabled(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, DynamicFPSConfig.enabled);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_nLocker(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, DynamicFPSConfig.nLocker);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
