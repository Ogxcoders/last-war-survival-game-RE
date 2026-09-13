using System;
using System.Collections.Generic;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class PostEventLogWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(PostEventLog);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 15, 1, 1);
		Utils.RegisterFunc(L, -4, "init", _m_init_xlua_st_);
		Utils.RegisterFunc(L, -4, "stop", _m_stop_xlua_st_);
		Utils.RegisterFunc(L, -4, "PostException", _m_PostException_xlua_st_);
		Utils.RegisterFunc(L, -4, "TaEnableAutoTrack", _m_TaEnableAutoTrack_xlua_st_);
		Utils.RegisterFunc(L, -4, "TaUserSet", _m_TaUserSet_xlua_st_);
		Utils.RegisterFunc(L, -4, "TaUserSetOnce", _m_TaUserSetOnce_xlua_st_);
		Utils.RegisterFunc(L, -4, "TaUserAdd", _m_TaUserAdd_xlua_st_);
		Utils.RegisterFunc(L, -4, "TaUserAppend", _m_TaUserAppend_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetSuperProperties", _m_SetSuperProperties_xlua_st_);
		Utils.RegisterFunc(L, -4, "TrackMap", _m_TrackMap_xlua_st_);
		Utils.RegisterFunc(L, -4, "Track", _m_Track_xlua_st_);
		Utils.RegisterFunc(L, -4, "Record", _m_Record_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "POSTURL", "");
		Utils.RegisterObject(L, translator, -4, "POSTURL_CN", "");
		Utils.RegisterFunc(L, -2, "hasInit", _g_get_hasInit);
		Utils.RegisterFunc(L, -1, "hasInit", _s_set_hasInit);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "PostEventLog does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_init_xlua_st_(IntPtr L)
	{
		try
		{
			PostEventLog.init();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_stop_xlua_st_(IntPtr L)
	{
		try
		{
			PostEventLog.stop();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PostException_xlua_st_(IntPtr L)
	{
		try
		{
			string action = Lua.lua_tostring(L, 1);
			string logString = Lua.lua_tostring(L, 2);
			string longText = Lua.lua_tostring(L, 3);
			PostEventLog.PostException(action, logString, longText);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TaEnableAutoTrack_xlua_st_(IntPtr L)
	{
		try
		{
			PostEventLog.TaEnableAutoTrack(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TaUserSet_xlua_st_(IntPtr L)
	{
		try
		{
			PostEventLog.TaUserSet(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TaUserSetOnce_xlua_st_(IntPtr L)
	{
		try
		{
			PostEventLog.TaUserSetOnce(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TaUserAdd_xlua_st_(IntPtr L)
	{
		try
		{
			PostEventLog.TaUserAdd(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TaUserAppend_xlua_st_(IntPtr L)
	{
		try
		{
			PostEventLog.TaUserAppend(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSuperProperties_xlua_st_(IntPtr L)
	{
		try
		{
			PostEventLog.SetSuperProperties(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TrackMap_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string eventName = Lua.lua_tostring(L, 1);
			Dictionary<string, object> prop = (Dictionary<string, object>)objectTranslator.GetObject(L, 2, typeof(Dictionary<string, object>));
			PostEventLog.TrackMap(eventName, prop);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Track_xlua_st_(IntPtr L)
	{
		try
		{
			string eventName = Lua.lua_tostring(L, 1);
			string prop = Lua.lua_tostring(L, 2);
			PostEventLog.Track(eventName, prop);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Record_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				PostEventLog.Record(Lua.lua_tostring(L, 1));
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string action = Lua.lua_tostring(L, 1);
				string param = Lua.lua_tostring(L, 2);
				PostEventLog.Record(action, param);
				return 0;
			}
			if (num >= 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 2) || Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string action2 = Lua.lua_tostring(L, 1);
				string[] args = objectTranslator.GetParams<string>(L, 2);
				PostEventLog.Record(action2, args);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				string action3 = Lua.lua_tostring(L, 1);
				string param2 = Lua.lua_tostring(L, 2);
				string param3 = Lua.lua_tostring(L, 3);
				PostEventLog.Record(action3, param2, param3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PostEventLog.Record!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_hasInit(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, PostEventLog.hasInit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_hasInit(IntPtr L)
	{
		try
		{
			PostEventLog.hasInit = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
