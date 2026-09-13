using System;
using GameKit.Base;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class CrossServerComponentWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CrossServerComponent);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 26, 7, 6);
		Utils.RegisterFunc(L, -3, "IsConnected", _m_IsConnected);
		Utils.RegisterFunc(L, -3, "Disconnect", _m_Disconnect);
		Utils.RegisterFunc(L, -3, "RemoveConnect", _m_RemoveConnect);
		Utils.RegisterFunc(L, -3, "Shutdown", _m_Shutdown);
		Utils.RegisterFunc(L, -3, "ClearRequestQueue", _m_ClearRequestQueue);
		Utils.RegisterFunc(L, -3, "ClearSpecialCommand", _m_ClearSpecialCommand);
		Utils.RegisterFunc(L, -3, "AddSpecialCommand", _m_AddSpecialCommand);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "DoConnect", _m_DoConnect);
		Utils.RegisterFunc(L, -3, "Send", _m_Send);
		Utils.RegisterFunc(L, -3, "OnGetServerListFromSFS", _m_OnGetServerListFromSFS);
		Utils.RegisterFunc(L, -3, "OnGetServerListFromSFSFailed", _m_OnGetServerListFromSFSFailed);
		Utils.RegisterFunc(L, -3, "OnConnection", _m_OnConnection);
		Utils.RegisterFunc(L, -3, "OnConnectionLost", _m_OnConnectionLost);
		Utils.RegisterFunc(L, -3, "OnLogin", _m_OnLogin);
		Utils.RegisterFunc(L, -3, "OnLoginError", _m_OnLoginError);
		Utils.RegisterFunc(L, -3, "OnLogout", _m_OnLogout);
		Utils.RegisterFunc(L, -3, "OnExtensionResponse", _m_OnExtensionResponse);
		Utils.RegisterFunc(L, -3, "IsMainLine", _m_IsMainLine);
		Utils.RegisterFunc(L, -3, "GetPing", _m_GetPing);
		Utils.RegisterFunc(L, -3, "GetLastPingPongTime", _m_GetLastPingPongTime);
		Utils.RegisterFunc(L, -3, "GetCurLine", _m_GetCurLine);
		Utils.RegisterFunc(L, -3, "GetCurPort", _m_GetCurPort);
		Utils.RegisterFunc(L, -3, "NetLogDebug", _m_NetLogDebug);
		Utils.RegisterFunc(L, -3, "NetLogInfo", _m_NetLogInfo);
		Utils.RegisterFunc(L, -3, "NetLogError", _m_NetLogError);
		Utils.RegisterFunc(L, -2, "ProxyIP", _g_get_ProxyIP);
		Utils.RegisterFunc(L, -2, "IP", _g_get_IP);
		Utils.RegisterFunc(L, -2, "Port", _g_get_Port);
		Utils.RegisterFunc(L, -2, "Zone", _g_get_Zone);
		Utils.RegisterFunc(L, -2, "JustUseProxy", _g_get_JustUseProxy);
		Utils.RegisterFunc(L, -2, "Logined", _g_get_Logined);
		Utils.RegisterFunc(L, -2, "BConnected", _g_get_BConnected);
		Utils.RegisterFunc(L, -1, "ProxyIP", _s_set_ProxyIP);
		Utils.RegisterFunc(L, -1, "IP", _s_set_IP);
		Utils.RegisterFunc(L, -1, "Port", _s_set_Port);
		Utils.RegisterFunc(L, -1, "Zone", _s_set_Zone);
		Utils.RegisterFunc(L, -1, "JustUseProxy", _s_set_JustUseProxy);
		Utils.RegisterFunc(L, -1, "Logined", _s_set_Logined);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 2, 2);
		Utils.RegisterFunc(L, -2, "OnlyMainLine", _g_get_OnlyMainLine);
		Utils.RegisterFunc(L, -2, "ENABLE_SFS_CROSS_GETSERVERLIST", _g_get_ENABLE_SFS_CROSS_GETSERVERLIST);
		Utils.RegisterFunc(L, -1, "OnlyMainLine", _s_set_OnlyMainLine);
		Utils.RegisterFunc(L, -1, "ENABLE_SFS_CROSS_GETSERVERLIST", _s_set_ENABLE_SFS_CROSS_GETSERVERLIST);
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
				CrossServerComponent o = new CrossServerComponent();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CrossServerComponent constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsConnected(IntPtr L)
	{
		try
		{
			bool value = ((CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsConnected();
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
			((CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Disconnect();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveConnect(IntPtr L)
	{
		try
		{
			((CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RemoveConnect();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Shutdown(IntPtr L)
	{
		try
		{
			((CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Shutdown();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearRequestQueue(IntPtr L)
	{
		try
		{
			((CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearRequestQueue();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearSpecialCommand(IntPtr L)
	{
		try
		{
			((CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearSpecialCommand();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddSpecialCommand(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CrossServerComponent crossServerComponent = (CrossServerComponent)objectTranslator.FastGetCSObj(L, 1);
			BaseMessage request = (BaseMessage)objectTranslator.GetObject(L, 2, typeof(BaseMessage));
			crossServerComponent.AddSpecialCommand(request);
			return 0;
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
			CrossServerComponent obj = (CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float elapseSeconds = (float)Lua.lua_tonumber(L, 2);
			obj.OnUpdate(elapseSeconds);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoConnect(IntPtr L)
	{
		try
		{
			((CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DoConnect();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Send(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CrossServerComponent crossServerComponent = (CrossServerComponent)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<IRequest>(L, 2))
			{
				IRequest request = (IRequest)objectTranslator.GetObject(L, 2, typeof(IRequest));
				crossServerComponent.Send(request);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<BaseMessage>(L, 2))
			{
				BaseMessage request2 = (BaseMessage)objectTranslator.GetObject(L, 2, typeof(BaseMessage));
				crossServerComponent.Send(request2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CrossServerComponent.Send!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnGetServerListFromSFS(IntPtr L)
	{
		try
		{
			CrossServerComponent obj = (CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string zone = Lua.lua_tostring(L, 2);
			string ip = Lua.lua_tostring(L, 3);
			int port = Lua.xlua_tointeger(L, 4);
			int connectionType = Lua.xlua_tointeger(L, 5);
			obj.OnGetServerListFromSFS(zone, ip, port, connectionType);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnGetServerListFromSFSFailed(IntPtr L)
	{
		try
		{
			CrossServerComponent obj = (CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string reason = Lua.lua_tostring(L, 2);
			obj.OnGetServerListFromSFSFailed(reason);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnConnection(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CrossServerComponent crossServerComponent = (CrossServerComponent)objectTranslator.FastGetCSObj(L, 1);
			INetProxy proxy = (INetProxy)objectTranslator.GetObject(L, 2, typeof(INetProxy));
			BaseEvent e = (BaseEvent)objectTranslator.GetObject(L, 3, typeof(BaseEvent));
			bool value = crossServerComponent.OnConnection(proxy, e);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnConnectionLost(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CrossServerComponent crossServerComponent = (CrossServerComponent)objectTranslator.FastGetCSObj(L, 1);
			string reason = Lua.lua_tostring(L, 2);
			INetProxy proxy = (INetProxy)objectTranslator.GetObject(L, 3, typeof(INetProxy));
			crossServerComponent.OnConnectionLost(reason, proxy);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnLogin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CrossServerComponent crossServerComponent = (CrossServerComponent)objectTranslator.FastGetCSObj(L, 1);
			BaseEvent e = (BaseEvent)objectTranslator.GetObject(L, 2, typeof(BaseEvent));
			crossServerComponent.OnLogin(e);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnLoginError(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CrossServerComponent crossServerComponent = (CrossServerComponent)objectTranslator.FastGetCSObj(L, 1);
			BaseEvent e = (BaseEvent)objectTranslator.GetObject(L, 2, typeof(BaseEvent));
			crossServerComponent.OnLoginError(e);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnLogout(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CrossServerComponent crossServerComponent = (CrossServerComponent)objectTranslator.FastGetCSObj(L, 1);
			BaseEvent e = (BaseEvent)objectTranslator.GetObject(L, 2, typeof(BaseEvent));
			crossServerComponent.OnLogout(e);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnExtensionResponse(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CrossServerComponent crossServerComponent = (CrossServerComponent)objectTranslator.FastGetCSObj(L, 1);
			string cmd = Lua.lua_tostring(L, 2);
			SFSObject so = (SFSObject)objectTranslator.GetObject(L, 3, typeof(SFSObject));
			crossServerComponent.OnExtensionResponse(cmd, so);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsMainLine(IntPtr L)
	{
		try
		{
			bool value = ((CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsMainLine();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPing(IntPtr L)
	{
		try
		{
			int ping = ((CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetPing();
			Lua.xlua_pushinteger(L, ping);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLastPingPongTime(IntPtr L)
	{
		try
		{
			int lastPingPongTime = ((CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetLastPingPongTime();
			Lua.xlua_pushinteger(L, lastPingPongTime);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurLine(IntPtr L)
	{
		try
		{
			string curLine = ((CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCurLine();
			Lua.lua_pushstring(L, curLine);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurPort(IntPtr L)
	{
		try
		{
			int curPort = ((CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCurPort();
			Lua.xlua_pushinteger(L, curPort);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_NetLogDebug(IntPtr L)
	{
		try
		{
			CrossServerComponent obj = (CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string message = Lua.lua_tostring(L, 2);
			obj.NetLogDebug(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_NetLogInfo(IntPtr L)
	{
		try
		{
			CrossServerComponent obj = (CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string message = Lua.lua_tostring(L, 2);
			obj.NetLogInfo(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_NetLogError(IntPtr L)
	{
		try
		{
			CrossServerComponent obj = (CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string message = Lua.lua_tostring(L, 2);
			obj.NetLogError(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ProxyIP(IntPtr L)
	{
		try
		{
			CrossServerComponent crossServerComponent = (CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, crossServerComponent.ProxyIP);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IP(IntPtr L)
	{
		try
		{
			CrossServerComponent crossServerComponent = (CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, crossServerComponent.IP);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Port(IntPtr L)
	{
		try
		{
			CrossServerComponent crossServerComponent = (CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, crossServerComponent.Port);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Zone(IntPtr L)
	{
		try
		{
			CrossServerComponent crossServerComponent = (CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, crossServerComponent.Zone);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_JustUseProxy(IntPtr L)
	{
		try
		{
			CrossServerComponent crossServerComponent = (CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, crossServerComponent.JustUseProxy);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Logined(IntPtr L)
	{
		try
		{
			CrossServerComponent crossServerComponent = (CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, crossServerComponent.Logined);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_BConnected(IntPtr L)
	{
		try
		{
			CrossServerComponent crossServerComponent = (CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, crossServerComponent.BConnected);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OnlyMainLine(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, CrossServerComponent.OnlyMainLine);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ENABLE_SFS_CROSS_GETSERVERLIST(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, CrossServerComponent.ENABLE_SFS_CROSS_GETSERVERLIST);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ProxyIP(IntPtr L)
	{
		try
		{
			((CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ProxyIP = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IP(IntPtr L)
	{
		try
		{
			((CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IP = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Port(IntPtr L)
	{
		try
		{
			((CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Port = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Zone(IntPtr L)
	{
		try
		{
			((CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Zone = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_JustUseProxy(IntPtr L)
	{
		try
		{
			((CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).JustUseProxy = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Logined(IntPtr L)
	{
		try
		{
			((CrossServerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Logined = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnlyMainLine(IntPtr L)
	{
		try
		{
			CrossServerComponent.OnlyMainLine = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ENABLE_SFS_CROSS_GETSERVERLIST(IntPtr L)
	{
		try
		{
			CrossServerComponent.ENABLE_SFS_CROSS_GETSERVERLIST = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
