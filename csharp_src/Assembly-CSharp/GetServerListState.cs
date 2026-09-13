using System;
using System.Collections.Generic;
using BestHTTP;
using GameFramework;
using RGNetUtils;
using UnityEngine;
using UnityEngine.Networking;

public class GetServerListState : LoadingStateBase
{
	private float _elapseTime;

	private float _timeout = 5f;

	private int _maxTryCount = 3;

	private int _tryCount;

	private bool _useUnityWebRequest = true;

	private UnityWebRequest _httpRequest;

	private HTTPRequest _bestHttpRequest;

	public GetServerListState(AppStartupLoading startupLoading)
		: base(startupLoading)
	{
	}

	public override void OnEnter(params object[] args)
	{
		Log.Info("get serverlist enter !");
		CleanHttpRequest();
		PostEventLog.Track("GET_SERVERLIST", null);
		GameEntry.Network.GameServerUrlList = null;
		_elapseTime = 0f;
		_tryCount = 1;
		_useUnityWebRequest = true;
		_maxTryCount = 4;
		_httpRequest = GameEntry.Network.GetServerList();
		_useUnityWebRequest = !_useUnityWebRequest;
	}

	private void OnServerListRequestCallback(HTTPRequest request, HTTPResponse response)
	{
		request.Callback = null;
		Log.Info($"get serverlist callBack state: {ApplicationLaunch.Instance.Loading.currState} !");
		if (ApplicationLaunch.Instance.Loading.currState != LoadingState.GetServerList)
		{
			Log.Error("GetServerList invalid state !");
			return;
		}
		if (response != null && response.IsSuccess)
		{
			if (!response.DataAsText.IsNullOrEmpty())
			{
				try
				{
					LoginServerListRespon loginServerListRespon = JsonUtility.FromJson<LoginServerListRespon>(response.DataAsText);
					if (loginServerListRespon == null)
					{
						throw new Exception("gsl error json::" + response.DataAsText);
					}
					if (!string.IsNullOrEmpty(loginServerListRespon.bin))
					{
						string bin = loginServerListRespon.bin;
						string gSLResp = AESHelper.GetGSLResp(GameEntry.Device.GetDeviceUid(), bin);
						loginServerListRespon = JsonUtility.FromJson<LoginServerListRespon>(gSLResp);
						if (loginServerListRespon == null)
						{
							throw new Exception("gsl encrypt error json::" + bin + " -> " + gSLResp);
						}
					}
					if (loginServerListRespon.code == 0 && loginServerListRespon.serverList != null && loginServerListRespon.serverList.Length != 0)
					{
						OnGetServerList(loginServerListRespon, "E000", "success");
					}
					else
					{
						Log.Info($"gsl return1 {response.DataAsText} code:{loginServerListRespon.code}");
						if (loginServerListRespon.code != 0)
						{
							OnGetServerList(loginServerListRespon, "E116", $"gsl code {loginServerListRespon.code}");
						}
						else
						{
							OnGetServerList(loginServerListRespon, "E120", "server list empty: " + response.DataAsText);
						}
					}
				}
				catch (Exception)
				{
					Log.Info("gsl return1 {0}", response.DataAsText);
					OnGetServerList(null, "E103", "invalid json: " + response.DataAsText);
				}
			}
			else
			{
				OnGetServerList(null, "E104", "empty data");
			}
			_bestHttpRequest?.Dispose();
			_bestHttpRequest = null;
			return;
		}
		PostEventLog.Record("SERVERLIST_TIME_OUT", _tryCount.ToString());
		if (_tryCount < _maxTryCount)
		{
			CleanHttpRequest();
			if (_useUnityWebRequest)
			{
				_httpRequest = GameEntry.Network.GetServerList();
			}
			else
			{
				_bestHttpRequest = GameEntry.Network.GetServerListRequest();
				_bestHttpRequest.Timeout = TimeSpan.FromSeconds(_timeout);
				_bestHttpRequest.Callback = OnServerListRequestCallback;
				_bestHttpRequest.Send();
			}
			_useUnityWebRequest = !_useUnityWebRequest;
			_tryCount++;
			GameEntry.Event.Fire(EventId.NetworkRetry, true);
			Log.Info("get serverlist try count: {0}", _tryCount);
		}
		else
		{
			Log.Error("get serverlist timeout");
			if (_bestHttpRequest != null && ClientSwitch.IsOn(19))
			{
				UDPTraceroute.StartAndReportAsync(_bestHttpRequest.Uri.Host, "SERVERLIST_TIME_OUT_TRACEROUTE", 24, 2, 300);
			}
			_bestHttpRequest?.Dispose();
			_bestHttpRequest = null;
			OnGetServerList(null, "E108", "timeout, reach max try count");
		}
	}

	private void CleanHttpRequest()
	{
		if (_bestHttpRequest != null)
		{
			_bestHttpRequest.Abort();
			_bestHttpRequest.Dispose();
			_bestHttpRequest = null;
		}
		if (_httpRequest != null)
		{
			_httpRequest.Abort();
			_httpRequest.Dispose();
			_httpRequest = null;
		}
	}

	public override void OnExit()
	{
		CleanHttpRequest();
	}

	public override void OnUpdate()
	{
		if (_httpRequest == null)
		{
			return;
		}
		_elapseTime += Time.deltaTime;
		if (_elapseTime > _timeout)
		{
			PostEventLog.Record("SERVERLIST_TIME_OUT", _tryCount.ToString());
			if (_tryCount < _maxTryCount)
			{
				CleanHttpRequest();
				if (_useUnityWebRequest)
				{
					_httpRequest = GameEntry.Network.GetServerList();
				}
				else
				{
					_bestHttpRequest = GameEntry.Network.GetServerListRequest();
					_bestHttpRequest.Timeout = TimeSpan.FromSeconds(_timeout);
					_bestHttpRequest.Callback = OnServerListRequestCallback;
					_bestHttpRequest.Send();
				}
				_useUnityWebRequest = !_useUnityWebRequest;
				_tryCount++;
				_elapseTime = 0f;
				GameEntry.Event.Fire(EventId.NetworkRetry, true);
				Log.Info("get serverlist try count: {0}", _tryCount);
			}
			else
			{
				Log.Error("get serverlist timeout");
				if (_httpRequest != null && ClientSwitch.IsOn(19))
				{
					UDPTraceroute.StartAndReportAsync(_httpRequest.uri.Host, "SERVERLIST_TIME_OUT_TRACEROUTE", 24, 2, 300);
				}
				_httpRequest.Dispose();
				_httpRequest = null;
				OnGetServerList(null, "E108", "timeout, reach max try count");
			}
		}
		else
		{
			if (!_httpRequest.isDone || _httpRequest.isHttpError || _httpRequest.isNetworkError)
			{
				return;
			}
			if (!_httpRequest.downloadHandler.text.IsNullOrEmpty())
			{
				try
				{
					LoginServerListRespon loginServerListRespon = JsonUtility.FromJson<LoginServerListRespon>(_httpRequest.downloadHandler.text);
					if (loginServerListRespon == null)
					{
						throw new Exception("error json::" + _httpRequest.downloadHandler.text);
					}
					if (!string.IsNullOrEmpty(loginServerListRespon.bin))
					{
						string bin = loginServerListRespon.bin;
						string gSLResp = AESHelper.GetGSLResp(GameEntry.Device.GetDeviceUid(), bin);
						loginServerListRespon = JsonUtility.FromJson<LoginServerListRespon>(gSLResp);
						if (loginServerListRespon == null)
						{
							throw new Exception("gsl encrypt error json::" + bin + " -> " + gSLResp);
						}
					}
					if (loginServerListRespon.code == 0 && loginServerListRespon.serverList != null && loginServerListRespon.serverList.Length != 0)
					{
						OnGetServerList(loginServerListRespon, "E000", "success");
					}
					else
					{
						Log.Info($"gsl return2 {_httpRequest.downloadHandler.text} {loginServerListRespon.code}");
						if (loginServerListRespon.code != 0)
						{
							OnGetServerList(loginServerListRespon, "E116", $"gsl code {loginServerListRespon.code}");
						}
						else
						{
							OnGetServerList(loginServerListRespon, "E120", "server list empty: " + _httpRequest.downloadHandler.text);
						}
					}
				}
				catch (Exception)
				{
					Log.Info("gsl return2 {0}", _httpRequest.downloadHandler.text);
					OnGetServerList(null, "E103", "invalid json: " + _httpRequest.downloadHandler.text);
				}
			}
			else
			{
				OnGetServerList(null, "E104", "empty data");
			}
			if (_httpRequest != null)
			{
				_httpRequest.Dispose();
				_httpRequest = null;
			}
		}
	}

	private void OnGetServerList(LoginServerListRespon response, string err, string msg)
	{
		try
		{
			GameEntry.Event.Fire(EventId.NetworkRetry, false);
			if (err != "E000")
			{
				if (response != null)
				{
					switch (response.code)
					{
					case 201:
						err = "E116_201";
						break;
					case 211:
						Log.Info("[AT]ClearAT_211");
						_startupLoading.ClearAccessToken();
						_startupLoading.ClearRefreshToken();
						_startupLoading.ClearLoginKey();
						GameEntry.Setting.Save();
						break;
					case 212:
						Log.Info("[AT]ClearGUID&AT_212");
						_startupLoading.ClearAllAccountSettings();
						_startupLoading.ClearAccessToken();
						_startupLoading.ClearRefreshToken();
						_startupLoading.ClearLoginKey();
						GameEntry.Setting.Save();
						break;
					case 213:
						err = "E213";
						break;
					}
				}
				_startupLoading.SetState(LoadingState.LoadingError, err);
				PostEventLog.TrackMap("SERVERLIST_FAILED", new Dictionary<string, object> { 
				{
					"errMsg",
					err + ", " + msg
				} });
				return;
			}
			GameEntry.Network.ServerList = response.serverList;
			for (int i = 0; i < response.serverList.Length; i++)
			{
				LoginServerInfo loginServerInfo = response.serverList[i];
				if (!SDKManager.IS_UNITY_EDITOR() && !CommonUtils.IsDebug())
				{
					Log.Info($"Server list [{i}] {loginServerInfo.gameUid}");
				}
			}
			_startupLoading.SaveGameLoginToken(response.at, response.rt);
			_startupLoading.ClearLoginKey();
			if (_startupLoading.IsShowServerList || (!NetworkURLConfig.IsOnline && string.IsNullOrEmpty(GameEntry.Network.GameServerUrl)))
			{
				Log.Info("show debug choose server ");
				GameEntry.Lua.UIManager.OpenWindow("UIDebugChooseServer", new Action<string, int, string, string, int>(DebugConnectGameServer));
				return;
			}
			if (!NetworkURLConfig.IsOnline)
			{
				DebugConnectGameServer(GameEntry.Network.GameServerUrl, GameEntry.Network.GameServerPort, GameEntry.Network.ZoneName, GameEntry.Network.Uid, GameEntry.Network.GameServerConnectionType);
				return;
			}
			LoginServerInfo loginServerInfo2 = response.GetLastLoggedServerInfo();
			if (loginServerInfo2 == null)
			{
				loginServerInfo2 = response.serverList[0];
			}
			Log.Info($"dont show debug choose server {loginServerInfo2.ip} {loginServerInfo2.port}, {loginServerInfo2.zone} {loginServerInfo2.gameUid}");
			if (ClientSwitch.IsOn(21) && string.IsNullOrEmpty(loginServerInfo2.gameUid))
			{
				AccountServerInfo loginServer = response.loginServer;
				_startupLoading.SetState(LoadingState.AccountSelect, loginServer, loginServerInfo2);
				return;
			}
			string ip = loginServerInfo2.ip;
			int connectionType = 0;
			int port = loginServerInfo2.port;
			if (ClientSwitch.IsOn(26) && !string.IsNullOrEmpty(loginServerInfo2.ws_ip))
			{
				ip = loginServerInfo2.ws_ip;
				connectionType = 1;
				port = 80;
			}
			Log.Info("[AT]SetGUID_GetSListState1:" + loginServerInfo2.gameUid);
			_startupLoading.SaveGameServerSetting("", port, loginServerInfo2.zone, loginServerInfo2.gameUid, "", connectionType);
			_startupLoading.ToConnectGame(ip, port, loginServerInfo2.zone, loginServerInfo2.gameUid ?? "", connectionType, loginServerInfo2.package_seperate_info, loginServerInfo2.ws_ip);
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
		}
	}

	private void DebugConnectGameServer(string ip, int port, string zone, string uid, int connectionType)
	{
		if (ip.IsNullOrEmpty() || zone.IsNullOrEmpty() || port == 0)
		{
			Log.Error("DebugConnectGameServer param error {0}, {1}, {2}", ip, port, zone);
			return;
		}
		Log.Info("[AT]SetGUID_GetSListState2:" + uid);
		_startupLoading.SaveGameServerSetting(ip, port, zone, uid, "", connectionType);
		if (connectionType == 1)
		{
			int num = ip.IndexOf("://");
			num = ((num >= 0) ? (num + 3) : 0);
			if (ip.IndexOf('/', num) < 0)
			{
				string text = zone;
				if (text.StartsWith("APS"))
				{
					text = text.Substring(3);
				}
				ip = ip + "/s" + text;
			}
			ClientSwitch.ForceSetSwitch(26, isOn: true);
		}
		string strRequiredPackages = null;
		int num2 = GameEntry.Network.ServerList.Length;
		for (int i = 0; i < num2; i++)
		{
			LoginServerInfo loginServerInfo = GameEntry.Network.ServerList[i];
			if (loginServerInfo.zone == zone)
			{
				strRequiredPackages = loginServerInfo.package_seperate_info;
				break;
			}
		}
		_startupLoading.ToConnectGame(ip, port, zone, uid, connectionType, strRequiredPackages, null);
		LoadingDebugTool.IsShowServerList = false;
	}
}
