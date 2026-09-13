using System;
using System.Collections.Generic;
using BestHTTP;
using GameKit.Base;
using Main.Scripts.Network;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using UnityEngine.Networking;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class NetworkManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(NetworkManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 40, 14, 9);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "Shutdown", _m_Shutdown);
		Utils.RegisterFunc(L, -3, "getFutureManager", _m_getFutureManager);
		Utils.RegisterFunc(L, -3, "getCurLine", _m_getCurLine);
		Utils.RegisterFunc(L, -3, "Connect", _m_Connect);
		Utils.RegisterFunc(L, -3, "Send", _m_Send);
		Utils.RegisterFunc(L, -3, "SendLuaMessage", _m_SendLuaMessage);
		Utils.RegisterFunc(L, -3, "Disconnect", _m_Disconnect);
		Utils.RegisterFunc(L, -3, "KillConnection", _m_KillConnection);
		Utils.RegisterFunc(L, -3, "SyncPingPong", _m_SyncPingPong);
		Utils.RegisterFunc(L, -3, "OnConnection", _m_OnConnection);
		Utils.RegisterFunc(L, -3, "OnConnectionLost", _m_OnConnectionLost);
		Utils.RegisterFunc(L, -3, "OnLogin", _m_OnLogin);
		Utils.RegisterFunc(L, -3, "OnLoginError", _m_OnLoginError);
		Utils.RegisterFunc(L, -3, "OnExtensionResponse", _m_OnExtensionResponse);
		Utils.RegisterFunc(L, -3, "IsMainLine", _m_IsMainLine);
		Utils.RegisterFunc(L, -3, "OnLogout", _m_OnLogout);
		Utils.RegisterFunc(L, -3, "GetServerInfo", _m_GetServerInfo);
		Utils.RegisterFunc(L, -3, "GetServerIdBySharedUser", _m_GetServerIdBySharedUser);
		Utils.RegisterFunc(L, -3, "SelectFinalGateServer", _m_SelectFinalGateServer);
		Utils.RegisterFunc(L, -3, "GetCurFinalGateServer", _m_GetCurFinalGateServer);
		Utils.RegisterFunc(L, -3, "ClearFinalGateServer", _m_ClearFinalGateServer);
		Utils.RegisterFunc(L, -3, "GetCrossServerListRequest", _m_GetCrossServerListRequest);
		Utils.RegisterFunc(L, -3, "GetServerListRequest", _m_GetServerListRequest);
		Utils.RegisterFunc(L, -3, "GetServerList", _m_GetServerList);
		Utils.RegisterFunc(L, -3, "GetCrossServerList", _m_GetCrossServerList);
		Utils.RegisterFunc(L, -3, "GetCrossServerListWebRequest", _m_GetCrossServerListWebRequest);
		Utils.RegisterFunc(L, -3, "GetServerNotice", _m_GetServerNotice);
		Utils.RegisterFunc(L, -3, "GetServerStatus", _m_GetServerStatus);
		Utils.RegisterFunc(L, -3, "GetPing", _m_GetPing);
		Utils.RegisterFunc(L, -3, "GetLastPingPongTime", _m_GetLastPingPongTime);
		Utils.RegisterFunc(L, -3, "IsPressureTestServer", _m_IsPressureTestServer);
		Utils.RegisterFunc(L, -3, "IsDebugConnectOnlineServer", _m_IsDebugConnectOnlineServer);
		Utils.RegisterFunc(L, -3, "CancelBattleReport", _m_CancelBattleReport);
		Utils.RegisterFunc(L, -3, "GetBattleReport", _m_GetBattleReport);
		Utils.RegisterFunc(L, -3, "UpdateSrcServerId", _m_UpdateSrcServerId);
		Utils.RegisterFunc(L, -3, "IfDownloadBattleReportDisableCache", _m_IfDownloadBattleReportDisableCache);
		Utils.RegisterFunc(L, -3, "IfUseZstdCompress", _m_IfUseZstdCompress);
		Utils.RegisterFunc(L, -3, "ExecuteZstdDecompressor", _m_ExecuteZstdDecompressor);
		Utils.RegisterFunc(L, -3, "DownloadBattleReport", _m_DownloadBattleReport);
		Utils.RegisterFunc(L, -2, "GameServerUrlList", _g_get_GameServerUrlList);
		Utils.RegisterFunc(L, -2, "GameServerUrl", _g_get_GameServerUrl);
		Utils.RegisterFunc(L, -2, "GameServerPort", _g_get_GameServerPort);
		Utils.RegisterFunc(L, -2, "ZoneName", _g_get_ZoneName);
		Utils.RegisterFunc(L, -2, "Uid", _g_get_Uid);
		Utils.RegisterFunc(L, -2, "Logined", _g_get_Logined);
		Utils.RegisterFunc(L, -2, "GameServerConnectionType", _g_get_GameServerConnectionType);
		Utils.RegisterFunc(L, -2, "IsConnected", _g_get_IsConnected);
		Utils.RegisterFunc(L, -2, "IsConnecting", _g_get_IsConnecting);
		Utils.RegisterFunc(L, -2, "IsPingPongTimeOut", _g_get_IsPingPongTimeOut);
		Utils.RegisterFunc(L, -2, "isNetworkValid", _g_get_isNetworkValid);
		Utils.RegisterFunc(L, -2, "ServerList", _g_get_ServerList);
		Utils.RegisterFunc(L, -2, "OnConnectionEvent", _g_get_OnConnectionEvent);
		Utils.RegisterFunc(L, -2, "OnConnectLostEvent", _g_get_OnConnectLostEvent);
		Utils.RegisterFunc(L, -1, "GameServerUrlList", _s_set_GameServerUrlList);
		Utils.RegisterFunc(L, -1, "GameServerUrl", _s_set_GameServerUrl);
		Utils.RegisterFunc(L, -1, "GameServerPort", _s_set_GameServerPort);
		Utils.RegisterFunc(L, -1, "ZoneName", _s_set_ZoneName);
		Utils.RegisterFunc(L, -1, "Uid", _s_set_Uid);
		Utils.RegisterFunc(L, -1, "GameServerConnectionType", _s_set_GameServerConnectionType);
		Utils.RegisterFunc(L, -1, "ServerList", _s_set_ServerList);
		Utils.RegisterFunc(L, -1, "OnConnectionEvent", _s_set_OnConnectionEvent);
		Utils.RegisterFunc(L, -1, "OnConnectLostEvent", _s_set_OnConnectLostEvent);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 3, 1, 1);
		Utils.RegisterFunc(L, -4, "FlushNetProfilerToShuShu", _m_FlushNetProfilerToShuShu_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "ENCRYPT_SERVERLIST", true);
		Utils.RegisterFunc(L, -2, "ForceUseOnlineCDN", _g_get_ForceUseOnlineCDN);
		Utils.RegisterFunc(L, -1, "ForceUseOnlineCDN", _s_set_ForceUseOnlineCDN);
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
				NetworkManager o = new NetworkManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to NetworkManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnUpdate(IntPtr L)
	{
		try
		{
			NetworkManager obj = (NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_Shutdown(IntPtr L)
	{
		try
		{
			((NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Shutdown();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_getFutureManager(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FutureManager futureManager = ((NetworkManager)objectTranslator.FastGetCSObj(L, 1)).getFutureManager();
			objectTranslator.Push(L, futureManager);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_getCurLine(IntPtr L)
	{
		try
		{
			string curLine = ((NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).getCurLine();
			Lua.lua_pushstring(L, curLine);
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
			((NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Connect();
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
			NetworkManager networkManager = (NetworkManager)objectTranslator.FastGetCSObj(L, 1);
			IRequest request = (IRequest)objectTranslator.GetObject(L, 2, typeof(IRequest));
			networkManager.Send(request);
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
			NetworkManager obj = (NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string msgId = Lua.lua_tostring(L, 2);
			byte[] sfsObjBinary = Lua.lua_tobytes(L, 3);
			obj.SendLuaMessage(msgId, sfsObjBinary);
			return 0;
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
			((NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Disconnect();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_KillConnection(IntPtr L)
	{
		try
		{
			((NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).KillConnection();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SyncPingPong(IntPtr L)
	{
		try
		{
			NetworkManager networkManager = (NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int time = Lua.xlua_tointeger(L, 2);
				networkManager.SyncPingPong(time);
				return 0;
			}
			if (num == 1)
			{
				networkManager.SyncPingPong();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to NetworkManager.SyncPingPong!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnConnection(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			NetworkManager networkManager = (NetworkManager)objectTranslator.FastGetCSObj(L, 1);
			INetProxy proxy = (INetProxy)objectTranslator.GetObject(L, 2, typeof(INetProxy));
			BaseEvent e = (BaseEvent)objectTranslator.GetObject(L, 3, typeof(BaseEvent));
			bool value = networkManager.OnConnection(proxy, e);
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
			NetworkManager networkManager = (NetworkManager)objectTranslator.FastGetCSObj(L, 1);
			string reason = Lua.lua_tostring(L, 2);
			INetProxy proxy = (INetProxy)objectTranslator.GetObject(L, 3, typeof(INetProxy));
			networkManager.OnConnectionLost(reason, proxy);
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
			NetworkManager networkManager = (NetworkManager)objectTranslator.FastGetCSObj(L, 1);
			BaseEvent e = (BaseEvent)objectTranslator.GetObject(L, 2, typeof(BaseEvent));
			networkManager.OnLogin(e);
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
			NetworkManager networkManager = (NetworkManager)objectTranslator.FastGetCSObj(L, 1);
			BaseEvent e = (BaseEvent)objectTranslator.GetObject(L, 2, typeof(BaseEvent));
			networkManager.OnLoginError(e);
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
			NetworkManager networkManager = (NetworkManager)objectTranslator.FastGetCSObj(L, 1);
			string cmd = Lua.lua_tostring(L, 2);
			SFSObject so = (SFSObject)objectTranslator.GetObject(L, 3, typeof(SFSObject));
			networkManager.OnExtensionResponse(cmd, so);
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
			bool value = ((NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsMainLine();
			Lua.lua_pushboolean(L, value);
			return 1;
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
			NetworkManager networkManager = (NetworkManager)objectTranslator.FastGetCSObj(L, 1);
			BaseEvent e = (BaseEvent)objectTranslator.GetObject(L, 2, typeof(BaseEvent));
			networkManager.OnLogout(e);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FlushNetProfilerToShuShu_xlua_st_(IntPtr L)
	{
		try
		{
			NetworkManager.FlushNetProfilerToShuShu((Dictionary<string, string>)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Dictionary<string, string>)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetServerInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			NetworkManager obj = (NetworkManager)objectTranslator.FastGetCSObj(L, 1);
			int id = Lua.xlua_tointeger(L, 2);
			LoginServerInfo serverInfo = obj.GetServerInfo(id);
			objectTranslator.Push(L, serverInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetServerIdBySharedUser(IntPtr L)
	{
		try
		{
			string serverIdBySharedUser = ((NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetServerIdBySharedUser();
			Lua.lua_pushstring(L, serverIdBySharedUser);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SelectFinalGateServer(IntPtr L)
	{
		try
		{
			NetworkManager obj = (NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string host = Lua.lua_tostring(L, 2);
			obj.SelectFinalGateServer(host);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurFinalGateServer(IntPtr L)
	{
		try
		{
			string curFinalGateServer = ((NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCurFinalGateServer();
			Lua.lua_pushstring(L, curFinalGateServer);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearFinalGateServer(IntPtr L)
	{
		try
		{
			((NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearFinalGateServer();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCrossServerListRequest(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			HTTPRequest crossServerListRequest = ((NetworkManager)objectTranslator.FastGetCSObj(L, 1)).GetCrossServerListRequest();
			objectTranslator.Push(L, crossServerListRequest);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetServerListRequest(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			HTTPRequest serverListRequest = ((NetworkManager)objectTranslator.FastGetCSObj(L, 1)).GetServerListRequest();
			objectTranslator.Push(L, serverListRequest);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetServerList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityWebRequest serverList = ((NetworkManager)objectTranslator.FastGetCSObj(L, 1)).GetServerList();
			objectTranslator.Push(L, serverList);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCrossServerList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			NetworkManager obj = (NetworkManager)objectTranslator.FastGetCSObj(L, 1);
			int targetServerId = Lua.xlua_tointeger(L, 2);
			HTTPRequest crossServerList = obj.GetCrossServerList(targetServerId);
			objectTranslator.Push(L, crossServerList);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCrossServerListWebRequest(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			NetworkManager obj = (NetworkManager)objectTranslator.FastGetCSObj(L, 1);
			int targetServerId = Lua.xlua_tointeger(L, 2);
			UnityWebRequest crossServerListWebRequest = obj.GetCrossServerListWebRequest(targetServerId);
			objectTranslator.Push(L, crossServerListWebRequest);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetServerNotice(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityWebRequest serverNotice = ((NetworkManager)objectTranslator.FastGetCSObj(L, 1)).GetServerNotice();
			objectTranslator.Push(L, serverNotice);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetServerStatus(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityWebRequest serverStatus = ((NetworkManager)objectTranslator.FastGetCSObj(L, 1)).GetServerStatus();
			objectTranslator.Push(L, serverStatus);
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
			int ping = ((NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetPing();
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
			int lastPingPongTime = ((NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetLastPingPongTime();
			Lua.xlua_pushinteger(L, lastPingPongTime);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsPressureTestServer(IntPtr L)
	{
		try
		{
			bool value = ((NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsPressureTestServer();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsDebugConnectOnlineServer(IntPtr L)
	{
		try
		{
			bool value = ((NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsDebugConnectOnlineServer();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CancelBattleReport(IntPtr L)
	{
		try
		{
			NetworkManager obj = (NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int cancelIndex = Lua.xlua_tointeger(L, 2);
			obj.CancelBattleReport(cancelIndex);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBattleReport(IntPtr L)
	{
		try
		{
			NetworkManager obj = (NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string uuid = Lua.lua_tostring(L, 2);
			int cancelIndex = Lua.xlua_tointeger(L, 3);
			bool isFull = Lua.lua_toboolean(L, 4);
			bool isAddressMode = Lua.lua_toboolean(L, 5);
			string address = Lua.lua_tostring(L, 6);
			obj.GetBattleReport(uuid, cancelIndex, isFull, isAddressMode, address);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateSrcServerId(IntPtr L)
	{
		try
		{
			NetworkManager obj = (NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			ushort sid = (ushort)Lua.xlua_tointeger(L, 2);
			obj.UpdateSrcServerId(sid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IfDownloadBattleReportDisableCache(IntPtr L)
	{
		try
		{
			bool value = ((NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IfDownloadBattleReportDisableCache();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IfUseZstdCompress(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			NetworkManager networkManager = (NetworkManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<HTTPResponse>(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				HTTPResponse resp = (HTTPResponse)objectTranslator.GetObject(L, 2, typeof(HTTPResponse));
				string uuid = Lua.lua_tostring(L, 3);
				bool value = networkManager.IfUseZstdCompress(resp, uuid);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<HTTPResponse>(L, 2))
			{
				HTTPResponse resp2 = (HTTPResponse)objectTranslator.GetObject(L, 2, typeof(HTTPResponse));
				bool value2 = networkManager.IfUseZstdCompress(resp2);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to NetworkManager.IfUseZstdCompress!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ExecuteZstdDecompressor(IntPtr L)
	{
		try
		{
			NetworkManager obj = (NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			byte[] data = Lua.lua_tobytes(L, 2);
			string uuid = Lua.lua_tostring(L, 3);
			byte[] str = obj.ExecuteZstdDecompressor(data, uuid);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DownloadBattleReport(IntPtr L)
	{
		try
		{
			NetworkManager obj = (NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string uuid = Lua.lua_tostring(L, 2);
			string extra = Lua.lua_tostring(L, 3);
			bool isAddressMode = Lua.lua_toboolean(L, 4);
			string address = Lua.lua_tostring(L, 5);
			bool immediate = Lua.lua_toboolean(L, 6);
			obj.DownloadBattleReport(uuid, extra, isAddressMode, address, immediate);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GameServerUrlList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			NetworkManager networkManager = (NetworkManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, networkManager.GameServerUrlList);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GameServerUrl(IntPtr L)
	{
		try
		{
			NetworkManager networkManager = (NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, networkManager.GameServerUrl);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GameServerPort(IntPtr L)
	{
		try
		{
			NetworkManager networkManager = (NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, networkManager.GameServerPort);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ZoneName(IntPtr L)
	{
		try
		{
			NetworkManager networkManager = (NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, networkManager.ZoneName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Uid(IntPtr L)
	{
		try
		{
			NetworkManager networkManager = (NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, networkManager.Uid);
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
			NetworkManager networkManager = (NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, networkManager.Logined);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GameServerConnectionType(IntPtr L)
	{
		try
		{
			NetworkManager networkManager = (NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, networkManager.GameServerConnectionType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsConnected(IntPtr L)
	{
		try
		{
			NetworkManager networkManager = (NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, networkManager.IsConnected);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsConnecting(IntPtr L)
	{
		try
		{
			NetworkManager networkManager = (NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, networkManager.IsConnecting);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsPingPongTimeOut(IntPtr L)
	{
		try
		{
			NetworkManager networkManager = (NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, networkManager.IsPingPongTimeOut);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isNetworkValid(IntPtr L)
	{
		try
		{
			NetworkManager networkManager = (NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, networkManager.isNetworkValid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ServerList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			NetworkManager networkManager = (NetworkManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, networkManager.ServerList);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ForceUseOnlineCDN(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, NetworkManager.ForceUseOnlineCDN);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OnConnectionEvent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			NetworkManager networkManager = (NetworkManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, networkManager.OnConnectionEvent);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OnConnectLostEvent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			NetworkManager networkManager = (NetworkManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, networkManager.OnConnectLostEvent);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_GameServerUrlList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((NetworkManager)objectTranslator.FastGetCSObj(L, 1)).GameServerUrlList = (string[])objectTranslator.GetObject(L, 2, typeof(string[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_GameServerUrl(IntPtr L)
	{
		try
		{
			((NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GameServerUrl = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_GameServerPort(IntPtr L)
	{
		try
		{
			((NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GameServerPort = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ZoneName(IntPtr L)
	{
		try
		{
			((NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ZoneName = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Uid(IntPtr L)
	{
		try
		{
			((NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Uid = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_GameServerConnectionType(IntPtr L)
	{
		try
		{
			((NetworkManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GameServerConnectionType = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ServerList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((NetworkManager)objectTranslator.FastGetCSObj(L, 1)).ServerList = (LoginServerInfo[])objectTranslator.GetObject(L, 2, typeof(LoginServerInfo[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ForceUseOnlineCDN(IntPtr L)
	{
		try
		{
			NetworkManager.ForceUseOnlineCDN = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnConnectionEvent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((NetworkManager)objectTranslator.FastGetCSObj(L, 1)).OnConnectionEvent = objectTranslator.GetDelegate<Action<string, string>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnConnectLostEvent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((NetworkManager)objectTranslator.FastGetCSObj(L, 1)).OnConnectLostEvent = objectTranslator.GetDelegate<Action<string>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
