using System;
using System.Collections.Generic;
using GameFramework;
using ProtoBufNet;
using RGNetUtils;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectGameState : LoadingStateBase
{
	private float _elapseTime;

	private float _timeout = 6f;

	private int _maxTryCount = 3;

	private int _tryCount;

	private UnityWebRequest _checkServerStatus;

	private int _cacheInputPort;

	private string _cacheInputZone;

	private string _cacheInputUid;

	private int _cacheInputConnectType;

	private string[] _cacheUrlList;

	private string[] _cacheWSUrlList;

	public ConnectGameState(AppStartupLoading startupLoading)
		: base(startupLoading)
	{
	}

	public override void OnEnter(params object[] args)
	{
		PostEventLog.TrackMap("connect_game_state", SDKManager.AddBILaunchTimeProperty());
		_timeout = 6f;
		_elapseTime = 0f;
		_tryCount = 1;
		_startupLoading.LoginTryCount++;
		string text = (string)args[0];
		if (GameEntry.Network.GameServerUrlList == null || GameEntry.Network.GameServerUrlList.Length == 0)
		{
			if (text.IsNullOrEmpty())
			{
				Log.Error("[ConnectGameState] ip list is empty!");
			}
			if ((_cacheUrlList = text.Split(new char[1] { '|' })).Length == 0)
			{
				Log.Error("ConnectGameState::OnEnter GameServerUrlList length 0 ！");
			}
			_cacheInputPort = (int)args[1];
			_cacheInputZone = (string)args[2];
			_cacheInputUid = (string)args[3];
			_cacheInputConnectType = (int)args[4];
		}
		else
		{
			_cacheUrlList = GameEntry.Network.GameServerUrlList;
			_cacheInputPort = GameEntry.Network.GameServerPort;
			_cacheInputZone = GameEntry.Network.ZoneName;
			_cacheInputUid = GameEntry.Network.Uid;
			_cacheInputConnectType = GameEntry.Network.GameServerConnectionType;
			Log.Info(string.Format("[ConnectGameState] use old ip {0} {1} {2} {3} {4},input:{5}", string.Join(",", _cacheUrlList), _cacheInputPort, _cacheInputZone, _cacheInputUid, _cacheInputConnectType, text));
		}
		if (string.IsNullOrEmpty(_cacheInputUid))
		{
			_cacheInputUid = (string)args[3];
			Log.Info("[ConnectGameState] Network.Uid==null,arg[3]=" + _cacheInputUid);
		}
		if (args.Length > 5 && args[5] is string text2 && !string.IsNullOrEmpty(text2))
		{
			_cacheWSUrlList = text2.Split(new char[1] { '|' });
		}
		InitSwitch(_cacheInputZone);
		Log.Info("[AT]SetNetUid_ConEnter:" + _cacheInputUid);
		Connect(_cacheUrlList, _cacheInputPort, _cacheInputZone, _cacheInputUid, _cacheInputConnectType);
		_checkServerStatus = GameEntry.Network.GetServerStatus();
		NetworkURLConfig.IsChangeDebugURLGroup = false;
	}

	private void Connect(string[] urlList, int port, string zone, string uid, int conntype)
	{
		GameEntry.Network.GameServerUrlList = urlList;
		GameEntry.Network.GameServerPort = port;
		GameEntry.Network.ZoneName = zone;
		GameEntry.Network.Uid = uid;
		GameEntry.Network.GameServerConnectionType = conntype;
		GameEntry.Network.OnConnectionEvent = OnGameConnection;
		GameEntry.Network.Connect();
	}

	public override void OnExit()
	{
		_cacheInputPort = 0;
		_cacheInputZone = null;
		_cacheInputUid = null;
		_cacheInputConnectType = 0;
		_cacheUrlList = null;
		_cacheWSUrlList = null;
		GameEntry.Network.OnConnectionEvent = null;
		if (_checkServerStatus != null)
		{
			_checkServerStatus.Dispose();
			_checkServerStatus = null;
		}
	}

	public override void OnUpdate()
	{
		_elapseTime += Time.deltaTime;
		if (_elapseTime > _timeout)
		{
			PostEventLog.TrackMap("CONNECT_TIME_OUT", new Dictionary<string, object> { 
			{
				"errmsg",
				$"{_tryCount}"
			} });
			ErrorToDoConnectRetry("E108", "connect game server time out");
		}
		if (_checkServerStatus == null)
		{
			return;
		}
		if (_checkServerStatus.isHttpError || _checkServerStatus.isNetworkError)
		{
			Log.Error("check Server Status http error");
			_checkServerStatus.Dispose();
			_checkServerStatus = null;
		}
		else
		{
			if (!_checkServerStatus.isDone)
			{
				return;
			}
			string text = _checkServerStatus.downloadHandler.text;
			Log.Info("Check server status result: " + text);
			if (!string.IsNullOrEmpty(text))
			{
				try
				{
					if (JsonUtility.FromJson<ServerStatusRespon>(text).code != 0 && NetworkURLConfig.IsOnline)
					{
						_startupLoading.SetState(LoadingState.LoadingError, "E109");
					}
				}
				catch (Exception ex)
				{
					Log.Error("_checkServerStatus error " + ex.ToString());
				}
			}
			_checkServerStatus.Dispose();
			_checkServerStatus = null;
		}
	}

	private void InitSwitch(string zone)
	{
		try
		{
			bool isGM = GrayUtils.isGM;
			int result = 0;
			if (!string.IsNullOrEmpty(zone) && zone.Length > 3)
			{
				int.TryParse(zone.Substring(3), out result);
			}
			bool flag = false;
			flag = ((!isGM) ? GrayUtils.CheckLuaSwitchWithServerFormat("EnableServerRangeRouting", "k1", result) : GrayUtils.CheckLuaSwitchWithServerFormat("EnableServerRangeRouting", "k2", result));
			bool flag2 = ClientSwitch.IsOn(29);
			bool flag3 = flag2 && flag;
			Log.Info($"NetPacketConst.useForwardServerId {flag3}, server_gateway:gm:{isGM} sid:{result} clientswitch:{flag2}  functionOn:{flag} result:{flag3}");
			NetPacketConst.useForwardServerId = flag3;
		}
		catch (Exception arg)
		{
			Log.Error("NetPacketConst.useForwardServerId false, Exception:", arg);
			NetPacketConst.useForwardServerId = false;
		}
	}

	private void ErrorToDoConnectRetry(string err, string errorMessage)
	{
		if (_tryCount < _maxTryCount)
		{
			_elapseTime = 0f;
			_tryCount++;
			Log.Info("connect game try count :{0} ", _tryCount);
			GameEntry.Network.Disconnect();
			if (_tryCount % 2 == 0 && _cacheWSUrlList != null && ClientSwitch.IsOn(44))
			{
				Log.Info($"[ConnectGameState] retry WebSocket {_tryCount}");
				Log.Info("[AT]SetNetUid_Con1:" + _cacheInputUid);
				Connect(_cacheWSUrlList, 80, _cacheInputZone, _cacheInputUid, 1);
			}
			else
			{
				string arg = ((_cacheInputConnectType == 0) ? "Tcp" : "WebSocket");
				Log.Info($"[ConnectGameState] retry {arg} {_tryCount}");
				Log.Info("[AT]SetNetUid_Con2:" + _cacheInputUid);
				Connect(_cacheUrlList, _cacheInputPort, _cacheInputZone, _cacheInputUid, _cacheInputConnectType);
			}
			GameEntry.Event.Fire(EventId.NetworkRetry, true);
		}
		else
		{
			Log.Info("[ConnectGameState] On GameConnection err:" + errorMessage);
			GameEntry.Network.Disconnect();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("err_code", err);
			if (!string.IsNullOrEmpty(errorMessage))
			{
				dictionary.Add("err_msg", errorMessage);
			}
			PostEventLog.TrackMap("connect_fail", dictionary);
			if (err == "E108")
			{
				TraceNetWorkTimeOutRoute();
			}
			_startupLoading.SetState(LoadingState.GetServerStatus);
		}
	}

	public static void TraceNetWorkTimeOutRoute()
	{
		if (!ClientSwitch.IsOn(19) || GameEntry.Network.GameServerUrlList == null || GameEntry.Network.GameServerUrlList.Length == 0)
		{
			return;
		}
		List<string> list = new List<string>();
		for (int i = 0; i < GameEntry.Network.GameServerUrlList.Length; i++)
		{
			string hostFromInput = GetHostFromInput(GameEntry.Network.GameServerUrlList[i]);
			if (hostFromInput != string.Empty)
			{
				list.Add(hostFromInput);
			}
		}
		if (list.Count > 0)
		{
			UDPTraceroute.StartAndReportAsync(list.ToArray(), "CONNECT_TIME_OUT_TRACEROUTE", 24, 1, 300);
		}
	}

	public static string GetHostFromInput(string input)
	{
		if (Uri.TryCreate(input, UriKind.Absolute, out var result))
		{
			return result.Host;
		}
		if (Uri.CheckHostName(input) != UriHostNameType.Unknown)
		{
			return input;
		}
		return string.Empty;
	}

	private void OnGameConnection(string err, string errorMessage)
	{
		if (err == "E000")
		{
			GameEntry.Event.Fire(EventId.NetworkRetry, false);
			_startupLoading.SetState(LoadingState.Login);
			string text = ((GameEntry.Network.GameServerConnectionType == 0) ? "Tcp" : "WebSocket");
			Log.Info("ConnectGameState::On GameConnection Success , " + text);
		}
		else
		{
			ErrorToDoConnectRetry(err, errorMessage);
		}
	}
}
