using System;
using System.Collections.Generic;
using RiverBISDK;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class RiverBISDKBIManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(BIManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 9, 0, 0);
		Utils.RegisterFunc(L, -4, "InitBI", _m_InitBI_xlua_st_);
		Utils.RegisterFunc(L, -4, "UpdateResVersion", _m_UpdateResVersion_xlua_st_);
		Utils.RegisterFunc(L, -4, "Reset", _m_Reset_xlua_st_);
		Utils.RegisterFunc(L, -4, "OnApplicationPaused", _m_OnApplicationPaused_xlua_st_);
		Utils.RegisterFunc(L, -4, "UpdateGameInfo", _m_UpdateGameInfo_xlua_st_);
		Utils.RegisterFunc(L, -4, "SendToBI", _m_SendToBI_xlua_st_);
		Utils.RegisterFunc(L, -4, "SendToBIFromLua", _m_SendToBIFromLua_xlua_st_);
		Utils.RegisterFunc(L, -4, "Dispose", _m_Dispose_xlua_st_);
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
				BIManager o = new BIManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to RiverBISDK.BIManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitBI_xlua_st_(IntPtr L)
	{
		try
		{
			IDictionary<string, object> initInfoDict = (IDictionary<string, object>)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(IDictionary<string, object>));
			bool firstLaunch = Lua.lua_toboolean(L, 2);
			BIManager.InitBI(initInfoDict, firstLaunch);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateResVersion_xlua_st_(IntPtr L)
	{
		try
		{
			BIManager.UpdateResVersion(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Reset_xlua_st_(IntPtr L)
	{
		try
		{
			BIManager.Reset();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnApplicationPaused_xlua_st_(IntPtr L)
	{
		try
		{
			BIManager.OnApplicationPaused(Lua.lua_toboolean(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateGameInfo_xlua_st_(IntPtr L)
	{
		try
		{
			BIManager.UpdateGameInfo((IDictionary<string, object>)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(IDictionary<string, object>)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SendToBI_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				BIManager.SendToBI(Lua.lua_tostring(L, 1));
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<IDictionary<string, object>>(L, 2))
			{
				string eventName = Lua.lua_tostring(L, 1);
				IDictionary<string, object> eventInfoMap = (IDictionary<string, object>)objectTranslator.GetObject(L, 2, typeof(IDictionary<string, object>));
				BIManager.SendToBI(eventName, eventInfoMap);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to RiverBISDK.BIManager.SendToBI!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SendToBIFromLua_xlua_st_(IntPtr L)
	{
		try
		{
			string eventName = Lua.lua_tostring(L, 1);
			string json = Lua.lua_tostring(L, 2);
			BIManager.SendToBIFromLua(eventName, json);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Dispose_xlua_st_(IntPtr L)
	{
		try
		{
			BIManager.Dispose();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
