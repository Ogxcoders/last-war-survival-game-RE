using System;
using BestHTTP;
using GameFramework;
using UnityEngine;

public class CrossServerGetServerList : FsmBaseState
{
	private HTTPRequest _bestHttpRequest;

	private float _timeout = 5f;

	private string _errMsg;

	public override int id => 2;

	public CrossServerGetServerList(CrossServerFsmManager mgr)
		: base(mgr)
	{
	}

	public override void OnEnter(params object[] args)
	{
		_errMsg = null;
		_bestHttpRequest = null;
		if (_mgr.TryGetGSLResult(out var url, out var port, out var zoneName, out var connectionType))
		{
			Log.Info($"[CrossServerGetServerList] use sfs result,ip:{url}, port:{port}, zone:{zoneName}, connectionType:{connectionType}");
			string uid = GameEntry.Network.Uid;
			_mgr.SetState(3, url, port, zoneName, uid ?? "", connectionType);
		}
		else
		{
			Log.Error("[CrossServerGetServerList] sfs not valid");
			_bestHttpRequest = GameEntry.Network.GetCrossServerListRequest();
			_bestHttpRequest.Timeout = TimeSpan.FromSeconds(_timeout);
			_bestHttpRequest.Callback = OnServerListRequestCallback;
			_bestHttpRequest.Send();
		}
	}

	private void OnServerListRequestCallback(HTTPRequest originalrequest, HTTPResponse response)
	{
		if (response != null && response.IsSuccess)
		{
			try
			{
				Log.Info("[CrossServerGetServerList] get serverlist Success");
				LoginServerListRespon loginServerListRespon = JsonUtility.FromJson<LoginServerListRespon>(response.DataAsText);
				if (loginServerListRespon.code == 0 && loginServerListRespon.serverList != null && loginServerListRespon.serverList.Length != 0)
				{
					OnGetServerList(loginServerListRespon);
				}
			}
			catch (Exception ex)
			{
				_errMsg = ex.ToString();
			}
			finally
			{
				if (!string.IsNullOrEmpty(_errMsg))
				{
					GotoLoadingError(6, _errMsg);
				}
			}
		}
		else
		{
			if (response != null)
			{
				_errMsg = response.Message;
			}
			Log.Error("[CrossServerGetServerList] get serverlist faild :" + _errMsg);
			GotoLoadingError(6, _errMsg);
		}
		_bestHttpRequest?.Dispose();
		_bestHttpRequest = null;
	}

	private void OnGetServerList(LoginServerListRespon response)
	{
		GameEntry.Network.ServerList = response.serverList;
		Log.Info("[CrossServerGetServerList] Cross Server list count {0}", response.serverList.Length);
		for (int i = 0; i < response.serverList.Length; i++)
		{
			LoginServerInfo loginServerInfo = response.serverList[i];
			Log.Info($"[CrossServerGetServerList] Server list [{i}] :{loginServerInfo.ip} {loginServerInfo.port}, {loginServerInfo.zone} {loginServerInfo.gameUid}");
		}
		LoginServerInfo loginServerInfo2 = response.serverList[0];
		int num = 0;
		int num2 = loginServerInfo2.port;
		string text = loginServerInfo2.ip;
		if (ClientSwitch.IsOn(26) && !string.IsNullOrEmpty(loginServerInfo2.ws_ip))
		{
			text = loginServerInfo2.ws_ip;
			num = 1;
			num2 = 80;
		}
		_mgr.SetState(3, text, num2, loginServerInfo2.zone, loginServerInfo2.gameUid ?? "", num);
	}

	public override void OnExit()
	{
		if (_bestHttpRequest != null)
		{
			_bestHttpRequest.Abort();
			_bestHttpRequest.Dispose();
			_bestHttpRequest = null;
		}
		_errMsg = null;
	}

	public override void OnUpdate()
	{
	}
}
