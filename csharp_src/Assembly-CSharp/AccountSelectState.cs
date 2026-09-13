using System;
using System.Collections.Generic;
using GameFramework;
using RGNetUtils;
using Sfs2X.Entities.Data;
using UnityEngine;

public class AccountSelectState : LoadingStateBase
{
	private enum ConnectStatus : byte
	{
		None,
		Connecting,
		Connected,
		ConnectFailed
	}

	private LoginServerInfo _loginServerInfo;

	private AccountServerInfo _accountServerInfo;

	private float _elapseTime;

	private float _timeout = 2f;

	private int _maxTryCount = 3;

	private int _tryCount;

	private float _shumeiTimeOut = 1.5f;

	private float _shumeiCountingTime;

	private bool _newGameBannedByShumei;

	private bool _receiveShumeiExceptLevel;

	private bool _enableShumei;

	private ConnectStatus _connectStatus;

	public AccountSelectState(AppStartupLoading startupLoading)
		: base(startupLoading)
	{
	}

	public override void OnEnter(params object[] args)
	{
		PostEventLog.TrackMap("account_select_state", SDKManager.AddBILaunchTimeProperty());
		_accountServerInfo = args[0] as AccountServerInfo;
		_loginServerInfo = args[1] as LoginServerInfo;
		_enableShumei = ClientSwitch.IsOn(36);
		Debug.Log($"AccountSelectState:: ShuMei ClientSwitch is On ? {_enableShumei}");
		UILoadingComponent uILoading = _startupLoading.UILoading;
		if (uILoading != null)
		{
			uILoading.ShowAccountSelect(show: true);
			uILoading.ShowNewGame(!_enableShumei);
			uILoading.RegistNewGameAction(OnSelectNewGame);
			uILoading.RegistSelectAccountAction(OnClickOtherAccount);
		}
		_shumeiCountingTime = 0f;
		_newGameBannedByShumei = false;
		_receiveShumeiExceptLevel = false;
		_connectStatus = ConnectStatus.None;
		ConnectLoginServer();
		GameEntry.Event.Subscribe(EventId.LoginPushShumeiExceptionLevel, OnPushShumeiExceptLevel);
	}

	private void OnPushShumeiExceptLevel(object msg)
	{
		_receiveShumeiExceptLevel = true;
		bool newGameBannedByShumei = false;
		if (msg != null && msg is ISFSObject obj)
		{
			newGameBannedByShumei = obj.TryGetInt("creat_account_rick_level") == 1;
		}
		_newGameBannedByShumei = newGameBannedByShumei;
		UILoadingComponent uILoading = _startupLoading.UILoading;
		if (uILoading != null)
		{
			uILoading.ShowNewGame(!_newGameBannedByShumei);
		}
	}

	private void OnSelectNewGame()
	{
		if (_loginServerInfo != null)
		{
			string ip = _loginServerInfo.ip;
			int connectionType = 0;
			int port = _loginServerInfo.port;
			if (ClientSwitch.IsOn(26) && !string.IsNullOrEmpty(_loginServerInfo.ws_ip))
			{
				ip = _loginServerInfo.ws_ip;
				connectionType = 1;
				port = 80;
			}
			Log.Info("[AT]SetGUID_SelectNewGame:" + _loginServerInfo.gameUid);
			_startupLoading.SaveGameServerSetting("", port, _loginServerInfo.zone, _loginServerInfo.gameUid, "", connectionType);
			_startupLoading.ToConnectGame(ip, port, _loginServerInfo.zone, _loginServerInfo.gameUid ?? "", connectionType, _loginServerInfo.package_seperate_info, _loginServerInfo.ws_ip);
		}
	}

	private void OnClickOtherAccount()
	{
		GameEntry.Lua.UIManager.OpenWindow("UIChooseSwitchAccount", 110008, true);
	}

	private void ConnectLoginServer()
	{
		if (_accountServerInfo != null)
		{
			_timeout = 2f;
			_elapseTime = 0f;
			_tryCount = 1;
			string text = _accountServerInfo.ip;
			int gameServerConnectionType = 0;
			int gameServerPort = _accountServerInfo.port;
			if (ClientSwitch.IsOn(26) && !string.IsNullOrEmpty(_accountServerInfo.ws_ip))
			{
				text = _accountServerInfo.ws_ip;
				gameServerConnectionType = 1;
				gameServerPort = _accountServerInfo.ws_port;
			}
			string text2 = text;
			string[] array = text2.Split(new char[1] { '|' });
			GameEntry.Network.GameServerUrlList = array;
			if (array.Length == 0)
			{
				Log.Error("ConnectLoginServer ::OnEnter UrlList length 0 ！");
			}
			else
			{
				GameEntry.Network.GameServerUrl = array[0];
			}
			GameEntry.Network.GameServerPort = gameServerPort;
			GameEntry.Network.ZoneName = "";
			Log.Info("[AT]ClearNetworkUid");
			GameEntry.Network.Uid = "";
			GameEntry.Network.GameServerConnectionType = gameServerConnectionType;
			Debug.Log($"ConnectLoginServer::{text2},{GameEntry.Network.GameServerPort},{GameEntry.Network.ZoneName},{GameEntry.Network.Uid},{GameEntry.Network.GameServerConnectionType}");
			GameEntry.Network.OnConnectionEvent = OnLoginServerConnection;
			GameEntry.Network.OnConnectLostEvent = OnLoginConnectLost;
			GameEntry.Network.Connect();
			NetworkURLConfig.IsChangeDebugURLGroup = false;
			_connectStatus = ConnectStatus.Connecting;
			GameEntry.GlobalData.LoginServerError = true;
		}
	}

	public override void OnExit()
	{
		UILoadingComponent uILoading = _startupLoading.UILoading;
		if (uILoading != null)
		{
			uILoading.ShowNewGame(show: false);
			uILoading.ShowAccountSelect(show: false);
			uILoading.RegistNewGameAction(null);
			uILoading.RegistSelectAccountAction(null);
		}
		_loginServerInfo = null;
		GameEntry.Network.OnConnectionEvent = null;
		GameEntry.Network.OnConnectLostEvent = null;
		GameEntry.Network.GameServerUrl = null;
		GameEntry.Network.GameServerUrlList = null;
		GameEntry.Network.Shutdown();
		_shumeiCountingTime = 0f;
		_newGameBannedByShumei = false;
		_receiveShumeiExceptLevel = false;
		_connectStatus = ConnectStatus.None;
		GameEntry.Event.Unsubscribe(EventId.LoginPushShumeiExceptionLevel, OnPushShumeiExceptLevel);
	}

	public override void OnUpdate()
	{
		if (_enableShumei && !_receiveShumeiExceptLevel)
		{
			_shumeiCountingTime += Time.deltaTime;
			if (_shumeiCountingTime > _shumeiTimeOut)
			{
				UILoadingComponent uILoading = _startupLoading.UILoading;
				if (uILoading != null)
				{
					uILoading.ShowNewGame(show: true);
				}
				_receiveShumeiExceptLevel = true;
			}
		}
		if (_connectStatus == ConnectStatus.Connecting)
		{
			_elapseTime += Time.deltaTime;
			if (_elapseTime > _timeout)
			{
				PostEventLog.TrackMap("ACCOUNT_CONNECT_TIME_OUT", new Dictionary<string, object> { 
				{
					"errmsg",
					$"{_tryCount}"
				} });
				ErrorToDoConnectRetry("E108", "connect login server time out");
			}
		}
	}

	private void OnLoginConnectLost(string errorMessage)
	{
		Log.Info("ConnectLoginServer:: OnLoginConnectLost err:" + errorMessage);
		GameEntry.Event.Fire(EventId.NetworkRetry, true);
		Log.Info("ConnectLoginServer:: OnLoginConnectLost reconnect begin");
		_connectStatus = ConnectStatus.None;
		ConnectLoginServer();
	}

	private void ErrorToDoConnectRetry(string err, string errorMessage)
	{
		if (_tryCount < _maxTryCount)
		{
			_elapseTime = 0f;
			_tryCount++;
			Log.Info("connect login try count :{0} ", _tryCount);
			GameEntry.Network.Disconnect();
			GameEntry.Network.Connect();
			GameEntry.Event.Fire(EventId.NetworkRetry, true);
			return;
		}
		_connectStatus = ConnectStatus.ConnectFailed;
		Log.Info("ConnectLoginServer::On OnLoginServerConnection err:" + errorMessage);
		GameEntry.Network.Disconnect();
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("err_code", err);
		if (!string.IsNullOrEmpty(errorMessage))
		{
			dictionary.Add("err_msg", errorMessage);
		}
		PostEventLog.TrackMap("account_connect_fail", dictionary);
		if (err == "E108")
		{
			TraceNetWorkTimeOutRoute();
		}
		OnSelectNewGame();
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

	private void OnLoginServerConnection(string err, string errorMessage)
	{
		if (err == "E000")
		{
			Log.Info("ConnectLoginServer:OnLoginServerConnection Success");
			_connectStatus = ConnectStatus.Connected;
			GameEntry.GlobalData.LoginServerError = false;
			GameEntry.Event.Fire(EventId.NetworkRetry, false);
			if (_enableShumei && !_receiveShumeiExceptLevel)
			{
				ShumeiSdkManager.Instance.SendDeviceIdToServer();
			}
		}
		else
		{
			ErrorToDoConnectRetry(err, errorMessage);
		}
	}
}
