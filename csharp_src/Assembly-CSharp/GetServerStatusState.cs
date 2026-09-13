using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using UnityEngine.Networking;

public class GetServerStatusState : LoadingStateBase
{
	private float _elapseTime;

	private float _timeout = 5f;

	private int _maxTryCount = 3;

	private int _tryCount;

	private UnityWebRequest _httpRequest;

	public GetServerStatusState(AppStartupLoading startupLoading)
		: base(startupLoading)
	{
	}

	public override void OnEnter(params object[] args)
	{
		_elapseTime = 0f;
		_tryCount = 1;
		_httpRequest = GameEntry.Network.GetServerStatus();
	}

	public override void OnExit()
	{
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
			if (_tryCount < _maxTryCount)
			{
				_httpRequest.Abort();
				_httpRequest.Dispose();
				_httpRequest = GameEntry.Network.GetServerStatus();
				_tryCount++;
				_elapseTime = 0f;
				GameEntry.Event.Fire(EventId.NetworkRetry, true);
				Log.Info("get server status try count: {0}", _tryCount);
			}
			else
			{
				Log.Error("get server status timeout");
				_httpRequest.Dispose();
				_httpRequest = null;
				OnGetServerStatus(null, "E108");
			}
		}
		else if (_httpRequest.isHttpError || _httpRequest.isNetworkError)
		{
			Log.Error("get server status net error: {0}", _httpRequest.error);
			string err = (_httpRequest.isHttpError ? "E102" : "E101");
			_httpRequest.Dispose();
			_httpRequest = null;
			OnGetServerStatus(null, err);
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
					ServerStatusRespon serverStatusRespon = JsonUtility.FromJson<ServerStatusRespon>(_httpRequest.downloadHandler.text);
					if (serverStatusRespon == null)
					{
						throw new Exception("json error");
					}
					OnGetServerStatus(serverStatusRespon, "E000");
				}
				catch (Exception)
				{
					Log.Error("get server status json error:" + _httpRequest.downloadHandler.text);
					OnGetServerStatus(null, "E103");
				}
			}
			else
			{
				Log.Error("get server status data error");
				OnGetServerStatus(null, "E104");
			}
			_httpRequest.Dispose();
			_httpRequest = null;
		}
	}

	private void OnGetServerStatus(ServerStatusRespon response, string err)
	{
		try
		{
			GameEntry.Event.Fire(EventId.NetworkRetry, false);
			if (err == "E000")
			{
				PostEventLog.TrackMap("SERVER_STATUS", new Dictionary<string, object> { { "errMsg", response.message } });
				if (response.code != 0)
				{
					_startupLoading.SetState(LoadingState.LoadingError, "E109", response.time);
				}
				else
				{
					_startupLoading.SetState(LoadingState.LoadingError, "E124");
				}
			}
			else
			{
				_startupLoading.SetState(LoadingState.LoadingError, err);
			}
			Log.Info("GetServerStatusState:: err:" + err + " errMsg:" + response?.message);
		}
		catch (Exception ex)
		{
			Log.Error(ex.Message);
		}
	}
}
