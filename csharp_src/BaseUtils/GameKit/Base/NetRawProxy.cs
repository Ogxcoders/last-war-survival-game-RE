using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using BaseUtils;
using GameFramework;
using ProtoBufNet;
using Sfs2X.Bitswarm;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using UnityEngine;

namespace GameKit.Base;

public class NetRawProxy : INetProxy
{
	private enum ConnectSubState
	{
		None,
		LookUpHost,
		BeginConnect,
		ConnectSuccess
	}

	private float lastPingPongTime;

	private int offMaxTime = 12;

	private string host;

	private int port;

	private string path;

	private NetIOService ioService;

	private Timer timerKeepAlive;

	private MessageDispather m_dispatcher;

	private bool m_disposed;

	private NetConnection conn;

	public INetManager parent;

	private long _pingpongS;

	private long _pingpongE;

	private long _lastPingPongTime;

	private int _ping;

	private bool _waitConnectPingPong;

	private BaseEvent _cacheConnectEvent;

	private long _startConnectTime;

	protected bool _isCurProxy;

	private NetHandShakeInterceptor _interceptor = new NetHandShakeInterceptor();

	public static bool USE_IPV6 = false;

	private static bool USE_HUGE_PING_PONG = true;

	private readonly int _connectionType;

	private ushort _sid;

	private int ppLogSendTimes;

	private int ppLogRecvTimes;

	public string proxyName { get; private set; }

	public string proxyHost => host;

	public int proxyPort => port;

	public long connectTime { get; private set; }

	public long resolveDnsTime { get; private set; }

	public ProxyStatus Status { get; private set; }

	private ConnectSubState connectSubStatus { get; set; }

	public int proxyConnectionType => _connectionType;

	public bool syncServerTime { get; private set; }

	public NetProfiler profiler { get; private set; }

	public bool IsConnected
	{
		get
		{
			if (conn != null)
			{
				return conn.IsConnected;
			}
			return false;
		}
	}

	public bool IsConnecting => Status == ProxyStatus.connecting;

	public bool IsPingPongTimeOut
	{
		get
		{
			if (lastPingPongTime == 0f)
			{
				return false;
			}
			if (offMaxTime > 0 && lastPingPongTime + (float)offMaxTime < Time.realtimeSinceStartup)
			{
				return true;
			}
			return false;
		}
	}

	public NetRawProxy(string name, string h, int p, int connectionType, INetManager net, ushort sid)
	{
		proxyName = name;
		_connectionType = connectionType;
		if (connectionType == 1)
		{
			if (!Uri.TryCreate(h, UriKind.Absolute, out var result))
			{
				if (h.Contains("://"))
				{
					throw new Exception("UnSupport server url " + h);
				}
				h = "ws://" + h;
				result = new Uri(h);
			}
			host = result.Host;
			path = result.PathAndQuery;
			if (string.IsNullOrEmpty(path))
			{
				path = "/";
			}
		}
		else
		{
			host = h;
		}
		port = p;
		Status = ProxyStatus.init;
		connectSubStatus = ConnectSubState.None;
		parent = net;
		_isCurProxy = false;
		syncServerTime = false;
		profiler = new NetProfiler(this);
		_sid = sid;
		init();
	}

	private void init()
	{
		m_dispatcher = new MessageDispather(this);
		ioService = new NetIOService(m_dispatcher);
		timerKeepAlive = new Timer(4000.0);
		timerKeepAlive.Elapsed += CheckKeepAlive;
		timerKeepAlive.Enabled = false;
		_waitConnectPingPong = false;
		_isCurProxy = false;
		syncServerTime = false;
		_cacheConnectEvent = null;
		_startConnectTime = 0L;
		connectTime = 0L;
		resolveDnsTime = 0L;
		ppLogSendTimes = 0;
		ppLogRecvTimes = 0;
	}

	private void CheckKeepAlive(object sender, ElapsedEventArgs e)
	{
		ioService.Post(delegate
		{
			if (conn != null && conn.IsConnected)
			{
				_pingpongS = DateTime.Now.Ticks;
				if (_waitConnectPingPong && USE_HUGE_PING_PONG && _connectionType != 1)
				{
					conn.SendMessage(new CustomPingPongRequest());
					NetLogInfo("send custom ping pong");
				}
				else
				{
					conn.SendMessage(new PingPongRequest());
					if (ppLogSendTimes < 5)
					{
						ppLogSendTimes++;
						NetLogInfo($"PingPong send: {ppLogSendTimes}");
					}
				}
			}
		});
	}

	public void UpdateSmartFoxClient()
	{
		profiler.Tick();
		if (ioService != null)
		{
			ioService.Poll();
		}
	}

	public int GetPing()
	{
		return _ping;
	}

	public int GetLastPingPongTime()
	{
		return (int)((DateTime.Now.Ticks - _lastPingPongTime) / 10000);
	}

	public void Connect()
	{
		_waitConnectPingPong = false;
		_isCurProxy = false;
		syncServerTime = false;
		_cacheConnectEvent = null;
		_startConnectTime = DateTime.Now.Ticks;
		SyncPingPong();
		lookupHostName();
	}

	private void lookupHostName()
	{
		NetConnection state = (conn = new NetConnection(ioService, m_dispatcher, proxyName, profiler, _sid));
		connectSubStatus = ConnectSubState.LookUpHost;
		try
		{
			resolveDnsTime = DateTime.Now.Ticks;
			NetLogInfo("dns begin");
			Dns.BeginGetHostAddresses(host, OnHostNameResolved, state);
		}
		catch (SocketException ex)
		{
			conn.Error("Dns error: " + ex.Message + " " + ex.StackTrace, ex.SocketErrorCode);
		}
	}

	private void PostLog(LogLevel level, string msg)
	{
		if (ioService == null)
		{
			return;
		}
		ioService.Post(delegate
		{
			string msg2 = msg;
			if (level == LogLevel.Error)
			{
				NetLogError(msg2);
			}
			else
			{
				NetLogInfo(msg2);
			}
		});
	}

	private void OnHostNameResolved(IAsyncResult result)
	{
		resolveDnsTime = (int)((DateTime.Now.Ticks - resolveDnsTime) / 10000);
		PostLog(LogLevel.Info, "dns end");
		if (((NetConnection)result.AsyncState).IsDisposed())
		{
			return;
		}
		IPAddress[] array = null;
		try
		{
			array = Dns.EndGetHostAddresses(result);
		}
		catch (SocketException ex)
		{
			PostLog(LogLevel.Error, $"dns error:{ex.SocketErrorCode} {ex.Message} {ex.StackTrace}");
			conn.Error("Dns error: " + ex.Message + " " + ex.StackTrace, ex.SocketErrorCode);
		}
		bool flag = false;
		IPAddress[] array2 = array;
		foreach (IPAddress iPAddress in array2)
		{
			try
			{
				if (USE_IPV6 && iPAddress.AddressFamily == AddressFamily.InterNetworkV6)
				{
					PostLog(LogLevel.Info, $"BeginConnect hostAddressesV6:{iPAddress.AddressFamily} {iPAddress}");
					connectTime = DateTime.Now.Ticks;
					conn.BeginConnect(iPAddress, port, HandleConnect, conn);
					flag = true;
					break;
				}
				if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
				{
					PostLog(LogLevel.Info, $"BeginConnect hostAddressesV4:{iPAddress.AddressFamily} {iPAddress}");
					connectTime = DateTime.Now.Ticks;
					conn.BeginConnect(iPAddress, port, HandleConnect, conn);
					flag = true;
					break;
				}
			}
			catch (SocketException ex2)
			{
				PostLog(LogLevel.Error, $"hostAddresses:SocketException {ex2.SocketErrorCode} {ex2.Message} {ex2.StackTrace}");
				conn.Error("Connection error: " + ex2.Message + " " + ex2.StackTrace, ex2.SocketErrorCode);
			}
			catch (Exception ex3)
			{
				PostLog(LogLevel.Error, "hostAddresses:OtherException " + ex3.Message + " " + ex3.StackTrace);
			}
		}
		if (flag)
		{
			Status = ProxyStatus.connecting;
			connectSubStatus = ConnectSubState.BeginConnect;
		}
		else
		{
			PostLog(LogLevel.Error, "no line to connect");
		}
	}

	private void HandleConnect(IAsyncResult ar)
	{
		PostLog(LogLevel.Info, "Handle Connect step1");
		connectTime = (int)((DateTime.Now.Ticks - connectTime) / 10000);
		NetConnection c = ar.AsyncState as NetConnection;
		try
		{
			if (c.IsDisposed())
			{
				return;
			}
			c.EndConnect(ar);
			PostLog(LogLevel.Info, "Handle Connect step2");
			Action handlePostConnect = delegate
			{
				ioService.Post(delegate
				{
					OnConnection(new SFSEvent(SFSEvent.CONNECTION, new Hashtable { ["success"] = true }));
				});
				ioService.Post(delegate
				{
					c.Init();
				});
				connectSubStatus = ConnectSubState.ConnectSuccess;
			};
			if (_connectionType == 1)
			{
				_interceptor.Execute(host, port, path, c.socket, delegate(Exception exception)
				{
					if (exception != null)
					{
						c.Error("Connection error: " + exception.Message + " " + exception.StackTrace, (exception is SocketException ex2) ? ex2.SocketErrorCode : SocketError.SocketError);
					}
					else
					{
						handlePostConnect();
					}
				});
			}
			else
			{
				handlePostConnect();
			}
		}
		catch (SocketException ex)
		{
			PostLog(LogLevel.Error, $"HandleConnect SocketException:{ex.SocketErrorCode} {ex}");
			c.Error("Connection error: " + ex.Message + " " + ex.StackTrace, ex.SocketErrorCode);
		}
	}

	public void OnConnection(BaseEvent e)
	{
		if (timerKeepAlive != null)
		{
			timerKeepAlive.Enabled = true;
		}
		bool flag = (bool)e.Params["success"];
		if (flag)
		{
			Status = ProxyStatus.connected;
		}
		else
		{
			Status = ProxyStatus.connectError;
		}
		NetLogInfo($"OnConnection success:{flag} conn:{conn != null}");
		if (flag && conn != null)
		{
			_waitConnectPingPong = true;
			_cacheConnectEvent = e;
			ppLogSendTimes = 0;
			ppLogRecvTimes = 0;
			conn.PostRecv(_sid);
			CheckKeepAlive(null, null);
		}
	}

	public void OnConnectionLost(string reason, SocketError code)
	{
		NetLogError($"OnConnectionLost reason:{reason} code:{code}");
		if (timerKeepAlive != null)
		{
			timerKeepAlive.Enabled = false;
		}
		_waitConnectPingPong = false;
		_cacheConnectEvent = null;
		Status = ProxyStatus.connectError;
		if (connectSubStatus == ConnectSubState.LookUpHost || connectSubStatus == ConnectSubState.BeginConnect)
		{
			_cacheConnectEvent = new SFSEvent(SFSEvent.CONNECTION, new Hashtable
			{
				["success"] = false,
				["errorMessage"] = string.Empty
			});
			parent.OnConnection(this, _cacheConnectEvent);
		}
		_cacheConnectEvent = null;
		OnRawSocketError(reason, code);
		parent.OnConnectionLost(reason, this);
	}

	private void OnLogout(BaseEvent e)
	{
		parent.OnLogout(e);
		NetLogInfo("OnLogout");
	}

	public virtual void OnExtensionResponse(string cmd, SFSObject so)
	{
		SyncPingPong();
		MessageFactoryProxy.Instance.DispatchResponse(cmd, so);
		if (!_isCurProxy)
		{
			NetLogInfo("OnExtensionResponse,_isCurProxy:false," + cmd);
		}
	}

	public void OnLogin(BaseEvent e)
	{
		SyncPingPong();
		parent.OnLogin(e);
		NetLogInfo("OnLogin");
	}

	public void OnLoginError(BaseEvent e)
	{
		parent.OnLoginError(e);
		NetLogError("OnLoginError");
	}

	private void OnPublicMessage(BaseEvent e)
	{
	}

	private void OnLogError(BaseEvent e)
	{
		string msg = (string)e.Params["message"];
		NetLogError(msg);
	}

	private void OnRawSocketError(string error, SocketError se)
	{
		string msg = $"Socket error : {proxyName}_{se == SocketError.NotConnected}_{error}";
		NetLogError(msg);
	}

	public void OnPingPong(IMessage msg, long recvTime)
	{
		_pingpongE = DateTime.Now.Ticks;
		_ping = (int)((_pingpongE - _pingpongS) / 10000);
		_lastPingPongTime = _pingpongE;
		SyncPingPong();
		profiler.RecordPin(_ping);
		if (!_isCurProxy)
		{
			NetLogInfo("OnPingPong _isCurProxy:false");
		}
		bool arg = false;
		if (_waitConnectPingPong)
		{
			int num = (int)((DateTime.Now.Ticks - _startConnectTime) / 10000);
			NetLogInfo($"OnPingPong,useTime:{num}");
			if (parent != null)
			{
				if (!parent.OnConnection(this, _cacheConnectEvent))
				{
					Disconnect();
				}
				else
				{
					_isCurProxy = true;
					syncServerTime = true;
					arg = true;
					NetLogInfo($"Choose Line success,ping:{_ping}");
				}
			}
			_waitConnectPingPong = false;
			_cacheConnectEvent = null;
		}
		else if (ppLogRecvTimes < 5)
		{
			ppLogRecvTimes++;
			NetLogInfo($"PingPong recv: {ppLogRecvTimes}");
		}
		if (syncServerTime && msg.Content.ContainsKey("serverTime") && msg.Content.ContainsKey("clientTime"))
		{
			long arg2 = msg.Content.GetLong("serverTime");
			long arg3 = msg.Content.GetLong("clientTime");
			GameEntryProxy.Timer.SyncServerTime(recvTime, arg2, arg3, arg);
		}
	}

	public void SyncPingPong(int time = -1)
	{
		if (time == -1)
		{
			lastPingPongTime = Time.realtimeSinceStartup;
		}
		else
		{
			lastPingPongTime = time;
		}
	}

	public void Disconnect()
	{
		_isCurProxy = false;
		syncServerTime = false;
		if (conn != null)
		{
			timerKeepAlive.Dispose();
			timerKeepAlive = null;
			conn.Close(graceful: true, "disconnect");
			conn.Dispose();
			conn = null;
			Status = ProxyStatus.init;
			connectSubStatus = ConnectSubState.None;
			if (_waitConnectPingPong)
			{
				int num = (int)((DateTime.Now.Ticks - _startConnectTime) / 10000);
				NetLogInfo($"Disconnect in PingPong,useTime:{num}");
			}
			else
			{
				NetLogInfo("Disconnect");
			}
		}
	}

	public void UpdateSrcServerId(ushort sid)
	{
		_sid = sid;
		if (conn != null)
		{
			conn.UpdateSrcServerId(sid);
		}
	}

	public void Send(IRequest request)
	{
		if (conn != null && conn.IsConnected)
		{
			conn.SendMessage(request);
		}
		else
		{
			NetLogError($"Send msg when conn not ready. [conn is null:{conn == null}]");
		}
	}

	public void FlushProfiler(Action<Dictionary<string, string>> cb)
	{
		profiler.Flush(cb);
	}

	public void NetLogInfo(string msg)
	{
		Log.Info($"[Net] [{proxyName}] :[{port}]: {msg}");
	}

	public void NetLogError(string msg)
	{
		Log.Error($"[Net] [{proxyName}] :[{port}]: {msg}");
	}
}
