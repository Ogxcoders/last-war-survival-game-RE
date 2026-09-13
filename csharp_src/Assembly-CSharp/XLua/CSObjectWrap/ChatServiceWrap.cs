using System;
using System.Collections.Generic;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ChatServiceWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ChatService);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 12, 0, 0);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "GetSign", _m_GetSign);
		Utils.RegisterFunc(L, -3, "Uninit", _m_Uninit);
		Utils.RegisterFunc(L, -3, "RequestServerList", _m_RequestServerList);
		Utils.RegisterFunc(L, -3, "Connect", _m_Connect);
		Utils.RegisterFunc(L, -3, "Disconnect", _m_Disconnect);
		Utils.RegisterFunc(L, -3, "IsConnected", _m_IsConnected);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "SendLuaMessage", _m_SendLuaMessage);
		Utils.RegisterFunc(L, -3, "RequestTranslate", _m_RequestTranslate);
		Utils.RegisterFunc(L, -3, "RequestWiki", _m_RequestWiki);
		Utils.RegisterFunc(L, -3, "Shutdown", _m_Shutdown);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 1, 0);
		Utils.RegisterFunc(L, -2, "Instance", _g_get_Instance);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "ChatService does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ChatService chatService = (ChatService)objectTranslator.FastGetCSObj(L, 1);
			string chat_app_id = Lua.lua_tostring(L, 2);
			string playerUid = Lua.lua_tostring(L, 3);
			Action<string, string, int> callback = objectTranslator.GetDelegate<Action<string, string, int>>(L, 4);
			chatService.Init(chat_app_id, playerUid, callback);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSign(IntPtr L)
	{
		try
		{
			string sign = ((ChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetSign();
			Lua.lua_pushstring(L, sign);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Uninit(IntPtr L)
	{
		try
		{
			((ChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Uninit();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RequestServerList(IntPtr L)
	{
		try
		{
			ChatService obj = (ChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string url = Lua.lua_tostring(L, 2);
			int ud = Lua.xlua_tointeger(L, 3);
			bool value = obj.RequestServerList(url, ud);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Connect(IntPtr L)
	{
		try
		{
			ChatService obj = (ChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string protocol = Lua.lua_tostring(L, 2);
			string ip = Lua.lua_tostring(L, 3);
			int port = Lua.xlua_tointeger(L, 4);
			string token = Lua.lua_tostring(L, 5);
			bool value = obj.Connect(protocol, ip, port, token);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Disconnect(IntPtr L)
	{
		try
		{
			((ChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Disconnect();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsConnected(IntPtr L)
	{
		try
		{
			bool value = ((ChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsConnected();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnUpdate(IntPtr L)
	{
		try
		{
			((ChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnUpdate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SendLuaMessage(IntPtr L)
	{
		try
		{
			ChatService obj = (ChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string jsonMsg = Lua.lua_tostring(L, 2);
			obj.SendLuaMessage(jsonMsg);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RequestTranslate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ChatService chatService = (ChatService)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action<string, string>>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				string uri = Lua.lua_tostring(L, 2);
				string postParams = Lua.lua_tostring(L, 3);
				Action<string, string> cb = objectTranslator.GetDelegate<Action<string, string>>(L, 4);
				int timeOut = Lua.xlua_tointeger(L, 5);
				chatService.RequestTranslate(uri, postParams, cb, timeOut);
				return 0;
			}
			if (num == 6 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Dictionary<string, string>>(L, 3) && objectTranslator.Assignable<Action<string, string, string>>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && (Lua.lua_isnil(L, 6) || Lua.lua_type(L, 6) == LuaTypes.LUA_TSTRING))
			{
				string uri2 = Lua.lua_tostring(L, 2);
				Dictionary<string, string> postParams2 = (Dictionary<string, string>)objectTranslator.GetObject(L, 3, typeof(Dictionary<string, string>));
				Action<string, string, string> cb2 = objectTranslator.GetDelegate<Action<string, string, string>>(L, 4);
				int tempTimeOut = Lua.xlua_tointeger(L, 5);
				string transIndex = Lua.lua_tostring(L, 6);
				chatService.RequestTranslate(uri2, postParams2, cb2, tempTimeOut, transIndex);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ChatService.RequestTranslate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RequestWiki(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ChatService chatService = (ChatService)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 6 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action<string, string>>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<Dictionary<string, string>>(L, 6))
			{
				string uri = Lua.lua_tostring(L, 2);
				string json = Lua.lua_tostring(L, 3);
				Action<string, string> cb = objectTranslator.GetDelegate<Action<string, string>>(L, 4);
				int timeOut = Lua.xlua_tointeger(L, 5);
				Dictionary<string, string> headers = (Dictionary<string, string>)objectTranslator.GetObject(L, 6, typeof(Dictionary<string, string>));
				chatService.RequestWiki(uri, json, cb, timeOut, headers);
				return 0;
			}
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action<string, string>>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				string uri2 = Lua.lua_tostring(L, 2);
				string json2 = Lua.lua_tostring(L, 3);
				Action<string, string> cb2 = objectTranslator.GetDelegate<Action<string, string>>(L, 4);
				int timeOut2 = Lua.xlua_tointeger(L, 5);
				chatService.RequestWiki(uri2, json2, cb2, timeOut2);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action<string, string>>(L, 4))
			{
				string uri3 = Lua.lua_tostring(L, 2);
				string json3 = Lua.lua_tostring(L, 3);
				Action<string, string> cb3 = objectTranslator.GetDelegate<Action<string, string>>(L, 4);
				chatService.RequestWiki(uri3, json3, cb3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ChatService.RequestWiki!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Shutdown(IntPtr L)
	{
		try
		{
			((ChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Shutdown();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Instance(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, ChatService.Instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
