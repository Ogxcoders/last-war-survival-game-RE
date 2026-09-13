using System;
using System.Collections.Generic;
using BestHTTP;
using GameFramework;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using UnityEngine;
using XLua;

public class SmartFoxCrossServerComponent : CrossServerComponent
{
	private enum State
	{
		Init,
		GetServerList,
		GetServerListSucceed,
		GetServerListFailed,
		Connecting,
		Connected,
		ConnectFailed,
		ConnectLost
	}

	private SmartFox m_Client;

	private float waitReConnectTime;

	private bool mIsConnecting;

	private List<BaseMessage> mRequestPending = new List<BaseMessage>();

	private List<BaseMessage> mRequestSended = new List<BaseMessage>();

	private List<BaseMessage> mSpecialCommand = new List<BaseMessage>();

	private State _curState;

	private HTTPRequest _getServerRequest;

	private readonly float _getServerTimeout = 10f;

	private readonly int _getServerMaxTryCount = 3;

	private int _getServerTryCount = 1;

	private readonly List<SmartFox> _allProxyList = new List<SmartFox>(4);

	private string[] _allServerList;

	private readonly HashSet<string> _serverListSet = new HashSet<string>();

	private int _serverPort;

	private readonly Dictionary<int, int> _cacheZoneToPort = new Dictionary<int, int>(16);

	private readonly Dictionary<int, string[]> _cacheZoneToServerList = new Dictionary<int, string[]>(16);

	private float repeatRate = 0.5f;

	private float deltaTime = 1f;

	private float reconnectTime = 3f;

	private bool SendPendRequest;

	private SmartFox NewSmartFox(string ip, int port)
	{
		SmartFox smartFox = new SmartFox();
		smartFox.ThreadSafeMode = true;
		smartFox.AddEventListener(SFSEvent.CONNECTION, OnConnected);
		smartFox.AddEventListener(SFSEvent.CONNECTION_LOST, OnDisconnect);
		smartFox.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
		smartFox.AddEventListener(SFSEvent.PUBLIC_MESSAGE, OnPublicMessage);
		smartFox.AddEventListener(SFSEvent.LOGIN, OnLogin);
		smartFox.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
		smartFox.AddEventListener(SFSEvent.LOGOUT, OnLogout);
		smartFox.AddEventListener(SFSEvent.SOCKET_ERROR, OnSocketError);
		smartFox.Connect(ip, port);
		return smartFox;
	}

	private void InitSmartFox()
	{
		_curState = State.Connecting;
		int serverPort = _serverPort;
		string[] allServerList = _allServerList;
		_serverListSet.Clear();
		Log.Info($"SmartFoxCrossServerComponent InitSmartFox serverListLength : {((allServerList != null) ? allServerList.Length : 0)}");
		if (allServerList == null || allServerList.Length == 0)
		{
			Log.Error($"SmartFoxCrossServerComponent InitSmartFox serverList lenght is 0 , port : {serverPort} !");
			return;
		}
		string[] array = allServerList;
		foreach (string text in array)
		{
			if (_serverListSet.Add(text))
			{
				SmartFox item = NewSmartFox(text, serverPort);
				Log.Info($"SmartFoxCrossServerComponent InitSmartFox : {text}:{serverPort}");
				PostEventLog.TrackMap("InitSmartFox", new Dictionary<string, object>
				{
					{
						"netUrl",
						text ?? ""
					},
					{
						"serverPort",
						$"{serverPort}"
					}
				});
				_allProxyList.Add(item);
			}
		}
	}

	public override bool IsConnected()
	{
		if (m_Client != null)
		{
			return m_Client.IsConnected;
		}
		return false;
	}

	private void DisConnectClient(SmartFox client)
	{
		if (client != null)
		{
			client.RemoveAllEventListeners();
			client.Disconnect();
			client = null;
		}
	}

	public override void Disconnect()
	{
		DisConnectClient(m_Client);
		m_Client = null;
		DisconnectAll();
	}

	private void DisconnectAll()
	{
		foreach (SmartFox allProxy in _allProxyList)
		{
			allProxy.RemoveAllEventListeners();
			allProxy.Disconnect();
		}
		_allProxyList.Clear();
	}

	public override void RemoveConnect()
	{
		base.Logined = false;
		Disconnect();
		ClearRequestQueue();
		ClearServerListRequest();
		_curState = State.Init;
	}

	public override void Shutdown()
	{
		_cacheZoneToPort.Clear();
		_cacheZoneToServerList.Clear();
		RemoveConnect();
	}

	private void ClearServerListRequest()
	{
		if (_getServerRequest != null)
		{
			_getServerRequest.Callback = null;
			_getServerRequest.Abort();
			_getServerRequest.Dispose();
			_getServerRequest = null;
		}
	}

	public override void ClearRequestQueue()
	{
		mRequestPending.Clear();
	}

	public override void ClearSpecialCommand()
	{
		mSpecialCommand.Clear();
	}

	public override void AddSpecialCommand(BaseMessage request)
	{
		if (request != null)
		{
			mSpecialCommand.Add(request);
		}
	}

	public override void OnUpdate(float elapseSeconds)
	{
		switch (_curState)
		{
		case State.Init:
		case State.GetServerList:
		case State.GetServerListSucceed:
			return;
		case State.GetServerListFailed:
		case State.ConnectFailed:
			waitReConnectTime += Time.deltaTime;
			if (waitReConnectTime >= reconnectTime)
			{
				PostEventLog.TrackMap("CrossServerRetry", new Dictionary<string, object> { 
				{
					"state",
					$"{_curState}"
				} });
				waitReConnectTime = 0f;
				Log.Info($"SmartFoxCrossServerComponent Connection crossServer lost, reconnect. state : {_curState}");
				_curState = State.ConnectLost;
				DoConnect();
			}
			return;
		case State.Connecting:
			foreach (SmartFox allProxy in _allProxyList)
			{
				allProxy.ProcessEvents();
			}
			if (_curState == State.Connected)
			{
				_allProxyList.Clear();
				break;
			}
			waitReConnectTime += Time.deltaTime;
			if (waitReConnectTime >= reconnectTime)
			{
				PostEventLog.TrackMap("CrossServerRetry", new Dictionary<string, object> { 
				{
					"state",
					$"{_curState}"
				} });
				waitReConnectTime = 0f;
				Log.Info($"SmartFoxCrossServerComponent Connection crossServer lost, reconnect. state : {_curState}");
				_curState = State.ConnectLost;
				DoConnect();
			}
			return;
		}
		if (m_Client == null)
		{
			return;
		}
		if (SendPendRequest)
		{
			deltaTime += Time.deltaTime;
			if (deltaTime > repeatRate)
			{
				onSendPendRequest();
				deltaTime = 0f;
			}
		}
		if (!m_Client.IsConnected)
		{
			waitReConnectTime += Time.deltaTime;
			if (waitReConnectTime >= reconnectTime)
			{
				PostEventLog.TrackMap("CrossServerRetry", new Dictionary<string, object> { 
				{
					"state",
					$"{_curState}"
				} });
				waitReConnectTime = 0f;
				Log.Info($"SmartFoxCrossServerComponent Connection crossServer lost, reconnect. state : {_curState}");
				_curState = State.ConnectLost;
				DoConnect();
				return;
			}
		}
		m_Client.ProcessEvents();
	}

	public override void DoConnect()
	{
		if (PrepareIpAndPort() && _curState != State.Connecting)
		{
			RemoveConnect();
			InitSmartFox();
			waitReConnectTime = 0f;
		}
	}

	public override void Send(IRequest request)
	{
		if (m_Client != null)
		{
			m_Client.Send(request);
		}
	}

	public override void Send(BaseMessage request)
	{
		if (m_Client == null)
		{
			mRequestPending.Add(request);
			DoConnect();
		}
		else if (!m_Client.IsConnected && _curState != State.Connecting)
		{
			mRequestPending.Add(request);
			DoConnect();
		}
		else if (!base.Logined && request.GetMsgId() != "login" && request.GetMsgId() != "login.init")
		{
			mRequestPending.Add(request);
		}
		else
		{
			request.Send();
			mRequestSended.Add(request);
		}
	}

	private bool PrepareIpAndPort()
	{
		if (_curState == State.GetServerList)
		{
			return false;
		}
		int crossServerId = GameEntry.Data.Player.GetCrossServerId();
		if (_cacheZoneToPort.TryGetValue(crossServerId, out _serverPort) && _cacheZoneToServerList.TryGetValue(crossServerId, out _allServerList))
		{
			return true;
		}
		if (CommonUtils.IsDebug())
		{
			LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable, int>("CSharpCallLuaInterface.GetTargetServerIdAndPort", crossServerId);
			if (luaTable != null && luaTable.ContainsKey("ip") && luaTable.ContainsKey("port"))
			{
				_cacheZoneToPort[crossServerId] = luaTable.Get<int>("port");
				_cacheZoneToServerList[crossServerId] = new string[1] { luaTable.Get<string>("ip") };
				return true;
			}
			switch (crossServerId)
			{
			case 9001:
				_cacheZoneToPort[crossServerId] = 8088;
				_cacheZoneToServerList[crossServerId] = new string[1] { "192.168.30.4" };
				return true;
			case 9002:
				_cacheZoneToPort[crossServerId] = 8088;
				_cacheZoneToServerList[crossServerId] = new string[1] { "192.168.20.112" };
				return true;
			}
		}
		PostEventLog.TrackMap("GetCrossServerList", null);
		_curState = State.GetServerList;
		_getServerTryCount = 1;
		ClearServerListRequest();
		_getServerRequest = GameEntry.Network.GetCrossServerList(crossServerId);
		_getServerRequest.Timeout = TimeSpan.FromSeconds(_getServerTimeout);
		_getServerRequest.Callback = OnGetServerCallback;
		_getServerRequest.Tag = crossServerId;
		_getServerRequest.Send();
		return false;
	}

	private void OnGetServerCallback(HTTPRequest request, HTTPResponse response)
	{
		request.Callback = null;
		if (response != null && response.IsSuccess)
		{
			if (!response.DataAsText.IsNullOrEmpty())
			{
				try
				{
					LoginServerListRespon loginServerListRespon = JsonUtility.FromJson<LoginServerListRespon>(response.DataAsText);
					if (loginServerListRespon != null)
					{
						if (loginServerListRespon.code == 0 && loginServerListRespon.serverList != null && loginServerListRespon.serverList.Length != 0)
						{
							LoginServerInfo obj = loginServerListRespon.serverList[0];
							int value = (base.Port = obj.port);
							string ip = obj.ip;
							if (string.IsNullOrEmpty(ip))
							{
								OnGetServerFailed("Empty ip. response=" + response.DataAsText);
							}
							else
							{
								string[] array = ip.Split(new char[1] { '|' });
								if (array.Length == 0)
								{
									OnGetServerFailed("Ip length 0. response=" + response.DataAsText);
								}
								else
								{
									int key = (int)request.Tag;
									_cacheZoneToPort[key] = value;
									_cacheZoneToServerList[key] = array;
									OnGetServerSucceed("response=" + response.DataAsText);
								}
							}
						}
						else
						{
							OnGetServerFailed($"code={loginServerListRespon.code} response={response.DataAsText}");
						}
					}
					else
					{
						OnGetServerFailed("error json response=" + response.DataAsText);
					}
				}
				catch (Exception arg)
				{
					OnGetServerFailed($"try exception e={arg} response={response.DataAsText}");
				}
			}
			else
			{
				OnGetServerFailed("empty response");
			}
			_getServerRequest?.Dispose();
			_getServerRequest = null;
			return;
		}
		PostEventLog.TrackMap("CrossServerListTimeOut", new Dictionary<string, object> { 
		{
			"detail",
			_getServerTryCount.ToString()
		} });
		if (_curState == State.GetServerList)
		{
			if (_getServerTryCount < _getServerMaxTryCount)
			{
				_getServerRequest.Dispose();
				PostEventLog.TrackMap("CrossServerListRetry", null);
				int crossServerId = GameEntry.Data.Player.GetCrossServerId();
				_getServerRequest = GameEntry.Network.GetCrossServerList(crossServerId);
				_getServerRequest.Timeout = TimeSpan.FromSeconds(_getServerTimeout);
				_getServerRequest.Callback = OnGetServerCallback;
				_getServerRequest.Tag = crossServerId;
				_getServerRequest.Send();
				_getServerTryCount++;
				Log.Info($"SmartFoxCrossServerComponent GetCrossServerList try count {_getServerTryCount}");
			}
			else
			{
				_getServerRequest.Dispose();
				_getServerRequest = null;
				Log.Error($"SmartFoxCrossServerComponent GetCrossServerList Error ! TargetServerId:{(int)request.Tag}");
				_curState = State.GetServerListFailed;
			}
		}
	}

	private void OnGetServerSucceed(string info)
	{
		Log.Info("SmartFoxCrossServerComponent GetCrossServerList Succeed ! " + info);
		_curState = State.GetServerListSucceed;
		PostEventLog.TrackMap("CrossServerListSucceed", null);
		_getServerRequest?.Dispose();
		_getServerRequest = null;
		DoConnect();
	}

	private void OnGetServerFailed(string info)
	{
		Log.Info("SmartFoxCrossServerComponent GetCrossServerList Failed ! info:" + info);
		_curState = State.GetServerListFailed;
		waitReConnectTime = 0f;
		PostEventLog.TrackMap("CrossServerListFailed", new Dictionary<string, object> { { "info", info } });
	}

	private void OnConnected(BaseEvent e)
	{
		if (_curState != State.Connecting && m_Client != null && m_Client.IsConnected)
		{
			return;
		}
		waitReConnectTime = 0f;
		SmartFox smartFox = null;
		foreach (SmartFox allProxy in _allProxyList)
		{
			if (smartFox == null && allProxy.IsConnected)
			{
				smartFox = allProxy;
			}
		}
		if (smartFox != null)
		{
			m_Client = smartFox;
			_curState = State.Connected;
			foreach (SmartFox allProxy2 in _allProxyList)
			{
				if (smartFox != allProxy2)
				{
					DisConnectClient(allProxy2);
				}
			}
			Log.Info($"SmartFoxCrossServerComponent client connected ! host : {smartFox.CurrentIp} port : {smartFox.CurrentPort}");
			PostEventLog.TrackMap("ConnectSmartFoxSucceed", new Dictionary<string, object>
			{
				{
					"netUrl",
					smartFox.CurrentIp ?? ""
				},
				{
					"serverPort",
					$"{smartFox.CurrentPort}"
				}
			});
			foreach (BaseMessage item in mRequestPending)
			{
				if (item.GetMsgId() == "login")
				{
					Send(item);
					mRequestPending.Remove(item);
					return;
				}
			}
			new LoginCrossServerMessage().SendRequest();
		}
		else
		{
			Log.Error("SmartFoxCrossServerComponent OnConnected error ! ");
			_curState = State.ConnectFailed;
		}
	}

	private void OnDisconnect(BaseEvent e)
	{
	}

	public override void OnLogin(BaseEvent e)
	{
		SFSObject message = e.Params["data"] as SFSObject;
		foreach (BaseMessage item in mRequestSended)
		{
			if ("login" == item.GetMsgId())
			{
				item.Handle(message);
				mRequestSended.Remove(item);
				break;
			}
		}
		if (mRequestPending.Count == 0 && mSpecialCommand.Count > 0)
		{
			mSpecialCommand.ForEach(delegate(BaseMessage item)
			{
				mRequestPending.Add(item);
			});
			mSpecialCommand.Clear();
		}
		if (mRequestPending.Count > 0)
		{
			SendPendRequest = true;
		}
		base.Logined = true;
		GameEntry.Event.Fire(EventId.CloseCrossDisconnectView);
	}

	private void onSendPendRequest()
	{
		if (m_Client.IsConnected && mRequestPending.Count > 0)
		{
			BaseMessage baseMessage = mRequestPending[0];
			if (baseMessage != null)
			{
				baseMessage.Send();
				mRequestSended.Add(baseMessage);
				mRequestPending.Remove(baseMessage);
				return;
			}
		}
		SendPendRequest = false;
		deltaTime = 1f;
	}

	public override void OnLoginError(BaseEvent e)
	{
		string arg = (string)e.Params["errorMessage"];
		Log.Error("SmartFoxCrossServerComponent: Login error: {0}", arg);
		base.Logined = false;
	}

	public override void OnLogout(BaseEvent e)
	{
		Log.Error("SmartFoxCrossServerComponent: Logout CrossServer");
		base.Logined = false;
	}

	private void OnSocketError(BaseEvent e)
	{
	}

	private void OnExtensionResponse(BaseEvent e)
	{
		string text = (string)e.Params["cmd"];
		SFSObject sFSObject = e.Params["params"] as SFSObject;
		sFSObject?.PutUtfString("IsFromCrossObserver", "1");
		if (GMSwitch.IsGM && GMSwitch.GetBool("DebugLogProtocolMsg"))
		{
			string arg = sFSObject.ToJson();
			Log.Warning($"[Msg][Receive]<color=green>cross extension res <{text}> |</color> {arg}");
		}
		foreach (BaseMessage item in mRequestSended)
		{
			if (text == item.GetMsgId())
			{
				item.Handle(sFSObject);
				mRequestSended.Remove(item);
				return;
			}
		}
		MessageFactory.Instance.DispatchResponse(e);
	}

	private void OnPublicMessage(BaseEvent e)
	{
		Log.Warning("SmartFoxCrossServerComponent public message");
	}

	private int GetRealPort(bool isProxy)
	{
		return base.Port;
	}

	public override int GetPing()
	{
		return -1;
	}

	public override int GetLastPingPongTime()
	{
		return -1;
	}

	public override string GetCurLine()
	{
		if (m_Client != null && m_Client.IsConnected)
		{
			return m_Client.CurrentIp;
		}
		return string.Empty;
	}

	public override int GetCurPort()
	{
		if (m_Client != null && m_Client.IsConnected)
		{
			return m_Client.CurrentPort;
		}
		return 0;
	}
}
