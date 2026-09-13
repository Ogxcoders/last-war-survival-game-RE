using System;
using System.Collections.Generic;
using System.Diagnostics;
using BestHTTP;
using GameFramework;
using GameKit.Base;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using UnityEngine;
using UnityEngine.Networking;

public class CrossServerComponent : INetManager
{
	private enum State
	{
		Init,
		GetServerList,
		GetServerList_SFS,
		GetServerListSucceed,
		GetServerListFailed,
		Connecting,
		Connected,
		ConnectFailed,
		ConnectLost
	}

	public static bool OnlyMainLine = true;

	private INetProxy m_Client;

	private float waitReConnectTime;

	private bool mIsConnecting;

	private List<BaseMessage> mRequestPending = new List<BaseMessage>();

	private List<BaseMessage> mRequestSended = new List<BaseMessage>();

	private List<BaseMessage> mSpecialCommand = new List<BaseMessage>();

	public static bool ENABLE_SFS_CROSS_GETSERVERLIST = false;

	private State _curState;

	private HTTPRequest _getServerRequest;

	private UnityWebRequest _getServerWebRequest;

	private float _getServerElapseTime;

	private bool _useUnityWebRequest = true;

	private readonly float _getServerTimeout = 10f;

	private readonly int _getServerMaxTryCount = 3;

	private int _getServerTryCount = 1;

	private int _getServerTargetId;

	private readonly List<INetProxy> _allProxyList = new List<INetProxy>(4);

	private string[] _allServerList;

	private readonly HashSet<string> _serverListSet = new HashSet<string>();

	private int _serverPort;

	private int _serverConnectionType;

	private readonly Dictionary<int, int> _cacheZoneToPort = new Dictionary<int, int>(16);

	private readonly Dictionary<int, string[]> _cacheZoneToServerList = new Dictionary<int, string[]>(16);

	private readonly Dictionary<int, int> _cacheZoneToConnectionType = new Dictionary<int, int>(16);

	private bool _reConnecting;

	private float repeatRate = 0.5f;

	private float deltaTime = 1f;

	private float reconnectTime = 6f;

	private Stopwatch _flushProfiler = Stopwatch.StartNew();

	private const int FLUSH_TIME = 30000;

	private bool _forceSkipSfsGsl;

	private bool SendPendRequest;

	public string ProxyIP { get; set; }

	public string IP { get; set; }

	public int Port { get; set; }

	public string Zone { get; set; }

	public bool JustUseProxy { get; set; }

	public bool Logined { get; set; }

	public bool BConnected => IsConnected();

	private INetProxy NewProxy(string ip, int port, int connectionType)
	{
		ushort sid = (ushort)GameEntry.Data.Player.GetCrossServerId();
		CrossNetProxy crossNetProxy = new CrossNetProxy(ip, ip, port, connectionType, this, sid);
		crossNetProxy.Connect();
		return crossNetProxy;
	}

	private void InitSmartFox()
	{
		_curState = State.Connecting;
		int serverPort = _serverPort;
		string[] allServerList = _allServerList;
		int serverConnectionType = _serverConnectionType;
		_serverListSet.Clear();
		NetLogInfo($"InitCrossNetProxy serverListLength : {((allServerList != null) ? allServerList.Length : 0)}");
		if (allServerList == null || allServerList.Length == 0)
		{
			NetLogError($"InitCrossNetProxy serverList lenght is 0 , port : {serverPort} !");
			return;
		}
		string[] array = allServerList;
		foreach (string text in array)
		{
			if (_serverListSet.Add(text))
			{
				NetLogInfo($"Init Cross NetRawProxy {text}:{serverPort}");
				INetProxy item = NewProxy(text, serverPort, serverConnectionType);
				PostEventLog.TrackMap("InitCrossNetProxy", new Dictionary<string, object>
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

	public virtual bool IsConnected()
	{
		if (m_Client != null)
		{
			return m_Client.IsConnected;
		}
		return false;
	}

	private void DisConnectClient(INetProxy client)
	{
		if (client != null)
		{
			client.Disconnect();
			client = null;
		}
	}

	public virtual void Disconnect()
	{
		DisConnectClient(m_Client);
		m_Client = null;
		DisconnectAll();
	}

	private void DisconnectAll()
	{
		foreach (INetProxy allProxy in _allProxyList)
		{
			allProxy.Disconnect();
		}
		_allProxyList.Clear();
	}

	public virtual void RemoveConnect()
	{
		Logined = false;
		Disconnect();
		ClearRequestQueue();
		ClearServerListRequest();
		if (GameEntry.Lua != null && GameEntry.Lua.UIManager != null && GameEntry.Lua.UIManager.IsWindowOpen("UICrossDisconnect"))
		{
			GameEntry.Lua.UIManager.DestroyWindow("UICrossDisconnect");
		}
		_curState = State.Init;
	}

	public virtual void Shutdown()
	{
		_cacheZoneToPort.Clear();
		_cacheZoneToServerList.Clear();
		_cacheZoneToConnectionType.Clear();
		_forceSkipSfsGsl = false;
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
		if (_getServerWebRequest != null)
		{
			_getServerWebRequest.Abort();
			_getServerWebRequest.Dispose();
			_getServerWebRequest = null;
		}
	}

	public virtual void ClearRequestQueue()
	{
		mRequestPending.Clear();
	}

	public virtual void ClearSpecialCommand()
	{
		mSpecialCommand.Clear();
	}

	public virtual void AddSpecialCommand(BaseMessage request)
	{
		if (request != null)
		{
			mSpecialCommand.Add(request);
		}
	}

	private void ShowCrossDisconnect(string logInfo, string errorCode)
	{
		NetLogInfo(logInfo);
		_curState = State.ConnectLost;
		DoConnect();
		if (!GameEntry.Lua.UIManager.IsWindowOpen("UIDisconnect") && !GameEntry.Lua.UIManager.IsWindowOpen("UICrossDisconnect"))
		{
			GameEntry.Lua.UIManager.OpenWindow("UICrossDisconnect", "TopMost", errorCode);
		}
	}

	public virtual void OnUpdate(float elapseSeconds)
	{
		switch (_curState)
		{
		case State.Init:
			return;
		case State.GetServerList:
			OnUpdateGetServerListWebRequest();
			return;
		case State.GetServerList_SFS:
			return;
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
				ShowCrossDisconnect($"CrossServerRetry state : {_curState}", "E141");
			}
			return;
		case State.Connecting:
		{
			for (int i = 0; i < _allProxyList.Count; i++)
			{
				_allProxyList[i].UpdateSmartFoxClient();
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
				ShowCrossDisconnect($"CrossServerRetry state : {_curState}", "E142");
			}
			return;
		}
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
		if (m_Client.IsPingPongTimeOut)
		{
			PostEventLog.TrackMap("CrossServerRetry", new Dictionary<string, object> { 
			{
				"state",
				$"{_curState}"
			} });
			waitReConnectTime = 0f;
			_reConnecting = true;
			ShowCrossDisconnect($"PingPong Time Out, reconnect. state : {_curState}", "E143");
			return;
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
				_reConnecting = true;
				ShowCrossDisconnect($"m_Client time out, reconnect. state : {_curState}", "E144");
				return;
			}
		}
		m_Client.UpdateSmartFoxClient();
		if (_flushProfiler.ElapsedMilliseconds > 30000)
		{
			_flushProfiler.Restart();
			m_Client.FlushProfiler(NetworkManager.FlushNetProfilerToShuShu);
		}
	}

	public virtual void DoConnect()
	{
		if (PrepareIpAndPort() && _curState != State.Connecting)
		{
			RemoveConnect();
			InitSmartFox();
			waitReConnectTime = 0f;
		}
	}

	public virtual void Send(IRequest request)
	{
		if (OnlyMainLine)
		{
			GameEntry.Network.Send(request);
		}
		else if (m_Client != null)
		{
			m_Client.Send(request);
		}
	}

	public virtual void Send(BaseMessage request)
	{
		if (OnlyMainLine)
		{
			request.Send();
		}
		else if (m_Client == null)
		{
			mRequestPending.Add(request);
			DoConnect();
		}
		else if (!m_Client.IsConnected && _curState != State.Connecting)
		{
			mRequestPending.Add(request);
			DoConnect();
		}
		else if (!Logined && request.GetMsgId() != "login" && request.GetMsgId() != "login.init")
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
		if (_curState == State.GetServerList || _curState == State.GetServerList_SFS)
		{
			NetLogDebug($"skip PrepareIpAndPort {_curState}");
			return false;
		}
		int crossServerId = GameEntry.Data.Player.GetCrossServerId();
		if (_cacheZoneToPort.TryGetValue(crossServerId, out _serverPort) && _cacheZoneToServerList.TryGetValue(crossServerId, out _allServerList) && _cacheZoneToConnectionType.TryGetValue(crossServerId, out _serverConnectionType))
		{
			return true;
		}
		if (!_forceSkipSfsGsl)
		{
			_curState = State.GetServerList_SFS;
			NetLogInfo("send sfs gsl message");
			GetServerListMessage.Instance.Send(new GetServerListMessage.Request
			{
				serverId = crossServerId
			});
			return false;
		}
		PostEventLog.TrackMap("GetCrossServerList", null);
		_curState = State.GetServerList;
		_getServerTryCount = 1;
		_getServerElapseTime = 0f;
		ClearServerListRequest();
		NetLogInfo("send http gsl request");
		_useUnityWebRequest = true;
		_getServerTargetId = crossServerId;
		_getServerWebRequest = GameEntry.Network.GetCrossServerListWebRequest(crossServerId);
		_useUnityWebRequest = !_useUnityWebRequest;
		return false;
	}

	public void OnGetServerListFromSFS(string zone, string ip, int port, int connectionType)
	{
		if (_curState != State.GetServerList_SFS)
		{
			return;
		}
		NetLogInfo($"sfs gsl result : zone:{zone} ip:{ip} port:{port} connectionType:{connectionType}");
		string[] array = ip.Split(new char[1] { '|' });
		if (array.Length == 0)
		{
			OnGetServerListFromSFSFailed("sfs gsl error Ip length = 0");
			return;
		}
		if (zone.StartsWith("APS"))
		{
			zone = zone.Replace("APS", "");
		}
		int key = int.Parse(zone);
		_cacheZoneToPort[key] = port;
		_cacheZoneToServerList[key] = array;
		_cacheZoneToConnectionType[key] = connectionType;
		OnGetServerSucceed("sfs gsl success");
	}

	public void OnGetServerListFromSFSFailed(string reason)
	{
		_forceSkipSfsGsl = true;
		OnGetServerFailed(reason);
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
							int num = obj.port;
							string text = obj.ip;
							string ws_ip = obj.ws_ip;
							int value = 0;
							if (ClientSwitch.IsOn(26) && !string.IsNullOrEmpty(ws_ip))
							{
								text = ws_ip;
								value = 1;
								num = 80;
							}
							Port = num;
							if (string.IsNullOrEmpty(text))
							{
								OnGetServerFailed("Empty ip. response=" + response.DataAsText);
							}
							else
							{
								string[] array = text.Split(new char[1] { '|' });
								if (array.Length == 0)
								{
									OnGetServerFailed("Ip length 0. response=" + response.DataAsText);
								}
								else
								{
									int key = (int)request.Tag;
									_cacheZoneToPort[key] = num;
									_cacheZoneToServerList[key] = array;
									_cacheZoneToConnectionType[key] = value;
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
		if (_curState != State.GetServerList)
		{
			return;
		}
		if (_getServerTryCount < _getServerMaxTryCount)
		{
			ClearServerListRequest();
			int num2 = (_getServerTargetId = GameEntry.Data.Player.GetCrossServerId());
			if (!ENABLE_SFS_CROSS_GETSERVERLIST)
			{
				if (_useUnityWebRequest)
				{
					_getServerWebRequest = GameEntry.Network.GetCrossServerListWebRequest(num2);
				}
				else
				{
					_getServerRequest = GameEntry.Network.GetCrossServerList(num2);
					_getServerRequest.Timeout = TimeSpan.FromSeconds(_getServerTimeout);
					_getServerRequest.Callback = OnGetServerCallback;
					_getServerRequest.Tag = num2;
					_getServerRequest.Send();
					_useUnityWebRequest = !_useUnityWebRequest;
				}
			}
			_getServerElapseTime = 0f;
			PostEventLog.TrackMap("CrossServerListRetry", null);
			_getServerTryCount++;
			NetLogInfo($"GetCrossServerList try count {_getServerTryCount}");
		}
		else
		{
			_getServerRequest.Dispose();
			_getServerRequest = null;
			NetLogError($"GetCrossServerList Error ! TargetServerId:{(int)request.Tag}");
			_curState = State.GetServerListFailed;
		}
	}

	private void OnGetServerSucceed(string info)
	{
		NetLogInfo("GetCrossServerList Succeed ! " + info);
		_curState = State.GetServerListSucceed;
		PostEventLog.TrackMap("CrossServerListSucceed", null);
		_getServerRequest?.Dispose();
		_getServerRequest = null;
		_getServerWebRequest?.Dispose();
		_getServerWebRequest = null;
		DoConnect();
	}

	private void OnGetServerFailed(string info)
	{
		NetLogError("GetCrossServerList Failed ! info:" + info);
		_curState = State.GetServerListFailed;
		waitReConnectTime = 0f;
		PostEventLog.TrackMap("CrossServerListFailed", new Dictionary<string, object> { { "info", info } });
	}

	public bool OnConnection(INetProxy proxy, BaseEvent e)
	{
		if ((bool)e.Params["success"])
		{
			if (_curState == State.Connecting && m_Client == null)
			{
				waitReConnectTime = 0f;
				NetLogInfo($"CrossServerComponent client connected success! {proxy.proxyName}:{proxy.proxyPort}");
				foreach (INetProxy allProxy in _allProxyList)
				{
					if (!allProxy.proxyName.Equals(proxy.proxyName))
					{
						NetLogInfo("Close other raw_proxy:" + allProxy.proxyName);
						allProxy.Disconnect();
					}
				}
				m_Client = proxy;
				_curState = State.Connected;
				PostEventLog.TrackMap("ConnectCrossNetProxySucceed", new Dictionary<string, object>
				{
					{
						"netUrl",
						proxy.proxyHost ?? ""
					},
					{
						"serverPort",
						$"{proxy.proxyPort}"
					}
				});
				foreach (BaseMessage item in mRequestPending)
				{
					if (item.GetMsgId() == "login")
					{
						Send(item);
						mRequestPending.Remove(item);
						return true;
					}
				}
				new LoginCrossServerMessage().SendRequest();
				return true;
			}
			return false;
		}
		bool flag = true;
		NetLogInfo($"CrossServer OnConnection Failed ! proxy:{proxy.proxyHost} port:{proxy.proxyPort}");
		foreach (INetProxy allProxy2 in _allProxyList)
		{
			if (allProxy2.Status != ProxyStatus.connectError)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			waitReConnectTime = 0f;
			_curState = State.ConnectFailed;
			string text = (string)e.Params["errorMessage"];
			try
			{
				text = text.Split(new char[1] { '\n' })[0];
				text = text.Substring(text.LastIndexOf(':'));
				NetLogError("CrossServer Connect to server failed:" + text);
			}
			catch (Exception ex)
			{
				NetLogError(ex.Message);
			}
		}
		return false;
	}

	private void OnUpdateGetServerListWebRequest()
	{
		if (_getServerWebRequest == null)
		{
			return;
		}
		_getServerElapseTime += Time.deltaTime;
		if (_getServerElapseTime > _getServerTimeout)
		{
			PostEventLog.TrackMap("CrossServerListTimeOut", new Dictionary<string, object> { 
			{
				"detail",
				_getServerTryCount.ToString()
			} });
			if (_curState != State.GetServerList)
			{
				return;
			}
			if (_getServerTryCount < _getServerMaxTryCount)
			{
				ClearServerListRequest();
				int num = (_getServerTargetId = GameEntry.Data.Player.GetCrossServerId());
				if (!ENABLE_SFS_CROSS_GETSERVERLIST)
				{
					if (_useUnityWebRequest)
					{
						_getServerWebRequest = GameEntry.Network.GetCrossServerListWebRequest(num);
					}
					else
					{
						_getServerRequest = GameEntry.Network.GetCrossServerList(num);
						_getServerRequest.Timeout = TimeSpan.FromSeconds(_getServerTimeout);
						_getServerRequest.Callback = OnGetServerCallback;
						_getServerRequest.Tag = num;
						_getServerRequest.Send();
					}
					_useUnityWebRequest = !_useUnityWebRequest;
				}
				_getServerElapseTime = 0f;
				PostEventLog.TrackMap("CrossServerListRetry", null);
				_getServerTryCount++;
				NetLogInfo($"GetCrossServerList OnUpdate try count {_getServerTryCount}");
			}
			else
			{
				ClearServerListRequest();
				NetLogError($"GetCrossServerList OnUpdate Error ! TargetServerId:{_getServerTargetId}");
				_curState = State.GetServerListFailed;
			}
		}
		else
		{
			if (!_getServerWebRequest.isDone || _getServerWebRequest.isHttpError || _getServerWebRequest.isNetworkError)
			{
				return;
			}
			if (!_getServerWebRequest.downloadHandler.text.IsNullOrEmpty())
			{
				try
				{
					LoginServerListRespon loginServerListRespon = JsonUtility.FromJson<LoginServerListRespon>(_getServerWebRequest.downloadHandler.text);
					if (loginServerListRespon != null)
					{
						if (loginServerListRespon.code == 0 && loginServerListRespon.serverList != null && loginServerListRespon.serverList.Length != 0)
						{
							LoginServerInfo obj = loginServerListRespon.serverList[0];
							int num2 = obj.port;
							string text = obj.ip;
							string ws_ip = obj.ws_ip;
							int value = 0;
							if (ClientSwitch.IsOn(26) && !string.IsNullOrEmpty(ws_ip))
							{
								text = ws_ip;
								value = 1;
								num2 = 80;
							}
							Port = num2;
							if (string.IsNullOrEmpty(text))
							{
								OnGetServerFailed("Empty ip. response=" + _getServerWebRequest.downloadHandler.text);
							}
							else
							{
								string[] array = text.Split(new char[1] { '|' });
								if (array.Length == 0)
								{
									OnGetServerFailed("Ip length 0. response=" + _getServerWebRequest.downloadHandler.text);
								}
								else
								{
									int getServerTargetId = _getServerTargetId;
									_cacheZoneToPort[getServerTargetId] = num2;
									_cacheZoneToServerList[getServerTargetId] = array;
									_cacheZoneToConnectionType[getServerTargetId] = value;
									OnGetServerSucceed($"strSId = {getServerTargetId}; response={_getServerWebRequest.downloadHandler.text}");
								}
							}
						}
						else
						{
							OnGetServerFailed($"code={loginServerListRespon.code} response={_getServerWebRequest.downloadHandler.text}");
						}
					}
					else
					{
						OnGetServerFailed("error json response=" + _getServerWebRequest.downloadHandler.text);
					}
				}
				catch (Exception arg)
				{
					OnGetServerFailed($"try exception e={arg} response={_getServerWebRequest.downloadHandler.text}");
				}
			}
			else
			{
				OnGetServerFailed("empty response");
			}
			ClearServerListRequest();
		}
	}

	public void OnConnectionLost(string reason, INetProxy proxy)
	{
	}

	public virtual void OnLogin(BaseEvent e)
	{
		NetLogInfo("CrossServer OnLogin");
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
		if (_reConnecting && SceneManager.IsInWorld())
		{
			SceneManager.World.UpdateViewRequest(isForce: true);
		}
		_reConnecting = false;
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
		Logined = true;
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

	public virtual void OnLoginError(BaseEvent e)
	{
		try
		{
			if (e.Params != null && e.Params["errorMessage"] != null)
			{
				if (e.Params["errorMessage"] is SFSObject sFSObject)
				{
					if (sFSObject.ContainsKey("errorMessage"))
					{
						string utfString = sFSObject.GetUtfString("errorMessage");
						NetLogError("CrossServer: Login error: " + utfString);
					}
				}
				else
				{
					string text = (string)e.Params["errorMessage"];
					NetLogError("CrossServer: Login error: " + text);
				}
			}
			else
			{
				NetLogError("CrossServer: Login error !");
			}
		}
		catch (Exception ex)
		{
			NetLogError("CrossServer Login error . exception : " + ex.Message);
		}
		Logined = false;
	}

	public virtual void OnLogout(BaseEvent e)
	{
		NetLogError("CrossServer: Logout CrossServer");
		Logined = false;
	}

	private void OnSocketError(BaseEvent e)
	{
	}

	public void OnExtensionResponse(string cmd, SFSObject so)
	{
		so?.PutUtfString("IsFromCrossObserver", "1");
		if (GMSwitch.IsGM && GMSwitch.GetBool("DebugLogProtocolMsg"))
		{
			string arg = so.ToJson();
			Log.Warning($"[Msg][Receive]<color=green>cross extension res <{cmd}> |</color> {arg}");
		}
		foreach (BaseMessage item in mRequestSended)
		{
			if (cmd == item.GetMsgId())
			{
				item.Handle(so);
				mRequestSended.Remove(item);
				return;
			}
		}
		MessageFactory.Instance.DispatchResponse(cmd, so);
	}

	public bool IsMainLine()
	{
		return false;
	}

	private void OnPublicMessage(BaseEvent e)
	{
		Log.Warning("public message");
	}

	private int GetRealPort(bool isProxy)
	{
		return Port;
	}

	public virtual int GetPing()
	{
		if (m_Client != null && m_Client.IsConnected)
		{
			return m_Client.GetPing();
		}
		return 0;
	}

	public virtual int GetLastPingPongTime()
	{
		if (m_Client != null && m_Client.IsConnected)
		{
			return m_Client.GetLastPingPongTime();
		}
		return 0;
	}

	public virtual string GetCurLine()
	{
		if (m_Client != null && m_Client.IsConnected)
		{
			return m_Client.proxyHost;
		}
		return string.Empty;
	}

	public virtual int GetCurPort()
	{
		if (m_Client != null && m_Client.IsConnected)
		{
			return m_Client.proxyPort;
		}
		return 0;
	}

	public void NetLogDebug(string message)
	{
	}

	public void NetLogInfo(string message)
	{
		Log.Info("[Net] [cross] " + message);
	}

	public void NetLogError(string message)
	{
		Log.Error("[Net] [cross] " + message);
	}
}
