using System;
using System.Collections.Generic;
using BaseUtils;
using BestHTTP;
using GameFramework;
using GameKit.Base;
using UnityEngine;
using UnityEngine.Networking;

public class ChatService
{
	private static ChatService _instance;

	private static readonly object _lock = new object();

	private int WSItemCount;

	private WebSocketItem curWebSocket;

	private string CHAT_APP_ID = "100013";

	private Action<string, string, int> callback_;

	private string cur_player_uid_;

	private HTTPRequest _httpRequest;

	private ITimer _timeoutTimer;

	private bool _finished;

	private int requestServerListTimeOut = 10;

	public static ChatService Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (_lock)
				{
					if (_instance == null)
					{
						_instance = new ChatService();
					}
				}
			}
			return _instance;
		}
	}

	private ChatService()
	{
	}

	public void Init(string chat_app_id, string playerUid, Action<string, string, int> callback)
	{
		Uninit();
		callback_ = callback;
		cur_player_uid_ = playerUid;
		CHAT_APP_ID = chat_app_id;
	}

	public string GetSign()
	{
		return curWebSocket.GetSign();
	}

	public void Uninit()
	{
		CancelRecycleTimer();
		Disconnect();
		callback_ = null;
	}

	public bool RequestServerList(string url, int ud)
	{
		if (callback_ == null || cur_player_uid_.IsNullOrEmpty())
		{
			Log.Warning("[Chat][REQ] REJECT " + $"callbackNull={callback_ == null} " + $"uidEmpty={cur_player_uid_.IsNullOrEmpty()}");
			return false;
		}
		CleanupRequest();
		_finished = false;
		int requestUd = ud;
		string text = GameEntry.Timer.GetServerTimeSeconds().ToString();
		string mD = BaseUtils.StringUtils.GetMD5(BaseUtils.StringUtils.GetMD5(text.Substring(0, 3)) + BaseUtils.StringUtils.GetMD5(text.Substring(text.Length - 3, 3)));
		string mD2 = BaseUtils.StringUtils.GetMD5(CHAT_APP_ID + cur_player_uid_ + mD);
		_httpRequest = new HTTPRequest(new Uri(url), HTTPMethods.Post);
		_httpRequest.Callback = OnHttpRequestCallback;
		_httpRequest.Timeout = TimeSpan.FromSeconds(requestServerListTimeOut);
		_httpRequest.Tag = ud;
		_httpRequest.AddField("t", text);
		_httpRequest.AddField("s", mD2);
		_httpRequest.AddField("a", CHAT_APP_ID);
		_httpRequest.AddField("u", cur_player_uid_);
		int selfServerId = GameEntry.Data.Player.GetSelfServerId();
		_httpRequest.AddField("serverId", selfServerId.ToString());
		if (GameEntry.GlobalData.isChina())
		{
			_httpRequest.AddField("f", "cn");
		}
		_httpRequest.Send();
		Log.Info($"[Chat][REQ] SEND ud={ud}");
		_timeoutTimer = GameEntry.Timer.RegisterTimer(requestServerListTimeOut + 3, delegate
		{
			Log.Info($"[Chat][REQ] RegisterTimer ud={requestUd}");
			FinishRequest(success: false, "", requestUd);
		});
		return true;
	}

	private void OnHttpRequestCallback(HTTPRequest request, HTTPResponse response)
	{
		Log.Info("[Chat][CB] ENTER " + $"finished={_finished} " + $"sameReq={request == _httpRequest} " + $"respOk={response?.IsSuccess ?? false} " + "tag=" + ((request != null) ? request.Tag.ToString() : "null"));
		if (!_finished && request == _httpRequest)
		{
			bool flag = response?.IsSuccess ?? false;
			string data = (flag ? (response.DataAsText ?? "") : "");
			FinishRequest(flag, data, (int)request.Tag);
		}
	}

	private void CleanupRequest()
	{
		CancelRecycleTimer();
		if (_httpRequest != null)
		{
			_httpRequest.Callback = null;
			_httpRequest.Abort();
			_httpRequest.Dispose();
			_httpRequest = null;
		}
	}

	private void CancelRecycleTimer()
	{
		if (_timeoutTimer != null)
		{
			GameEntry.Timer.CancelTimer(_timeoutTimer);
			_timeoutTimer = null;
		}
	}

	private void FinishRequest(bool success, string data = "", int ud = 0)
	{
		if (!_finished)
		{
			_finished = true;
			CleanupRequest();
			callback_?.Invoke("onRequest", data, ud);
		}
	}

	public bool Connect(string protocol, string ip, int port, string token)
	{
		Disconnect();
		WSItemCount++;
		curWebSocket = new WebSocketItem();
		curWebSocket.SetCallBack(callback_);
		curWebSocket.Connect(CHAT_APP_ID, protocol, ip, port, token);
		return true;
	}

	public void Disconnect()
	{
		if (curWebSocket != null)
		{
			curWebSocket.SetCallBack(null);
			curWebSocket.Close();
		}
	}

	public bool IsConnected()
	{
		if (curWebSocket != null && curWebSocket.IsOpen())
		{
			return true;
		}
		return false;
	}

	public void OnUpdate()
	{
		if (curWebSocket != null)
		{
			curWebSocket.Process();
		}
	}

	public void SendLuaMessage(string jsonMsg)
	{
		if (curWebSocket != null)
		{
			curWebSocket.Send(jsonMsg);
		}
	}

	public void RequestTranslate(string uri, string postParams, Action<string, string> cb, int timeOut)
	{
		if (cb == null)
		{
			Log.Error("RequestTranslate no callback??? error!!");
			return;
		}
		int num = 0;
		num = timeOut;
		SingletonBehaviour<WebRequestManager>.Instance.Post(uri, postParams, delegate(UnityWebRequest request, bool hasErr, object userdata)
		{
			if (request.isDone)
			{
				bool flag = true;
				if (hasErr)
				{
					flag = false;
				}
				string text = request.downloadHandler.text;
				cb(flag ? "true" : "false", text);
			}
		}, 0, num);
	}

	public void RequestWiki(string uri, string json, Action<string, string> cb, int timeOut = 15, Dictionary<string, string> headers = null)
	{
		SingletonBehaviour<WebRequestManager>.Instance.PostJson(uri, json, delegate(UnityWebRequest request, bool hasErr, object userdata)
		{
			bool flag = !hasErr && request != null && request.isDone;
			string arg = null;
			if (request != null && request.downloadHandler != null)
			{
				arg = request.downloadHandler.text;
			}
			cb(flag ? "true" : "false", arg);
		}, 0, timeOut, headers);
	}

	public void RequestTranslate(string uri, Dictionary<string, string> postParams, Action<string, string, string> cb, int tempTimeOut, string transIndex)
	{
		if (cb == null)
		{
			Log.Error("RequestTranslate no callback??? error!!");
			return;
		}
		postParams.TryGetValue("sc", out var value);
		postParams.TryGetValue("sf", out var value2);
		postParams.TryGetValue("tf", out var value3);
		postParams.TryGetValue("ch", out var value4);
		postParams.TryGetValue("ui", out var value5);
		postParams.TryGetValue("uid", out var value6);
		postParams.TryGetValue("tk", out var value7);
		postParams.TryGetValue("gs", out var value8);
		postParams.TryGetValue("ul", out var value9);
		postParams.TryGetValue("type", out var value10);
		postParams.TryGetValue("roomGroupType", out var value11);
		if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value5) || string.IsNullOrEmpty(value4) || string.IsNullOrEmpty(value6))
		{
			Log.Error("RequestTranslate param error!!");
			return;
		}
		long serverTime = GameEntry.Timer.GetServerTime();
		string md5Hash = AESHelper.GetMd5Hash($"{value2}{value3}{value4}{serverTime}{value7}{value6}");
		string value12 = ((GameEntry.Setting.GetInt("Setting.GM_FLAG", 0) > 0) ? "1" : "0");
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("sc", value);
		wWWForm.AddField("sf", value2);
		wWWForm.AddField("tf", value3);
		wWWForm.AddField("ch", value4);
		wWWForm.AddField("ui", value5);
		wWWForm.AddField("scene", value4);
		wWWForm.AddField("uid", value6);
		wWWForm.AddField("tk", value7);
		wWWForm.AddField("t", serverTime.ToString());
		wWWForm.AddField("sig", md5Hash);
		wWWForm.AddField("gs", value8);
		wWWForm.AddField("gm", value12);
		wWWForm.AddField("ul", value9);
		wWWForm.AddField("type", value10);
		if (!string.IsNullOrEmpty(value11))
		{
			wWWForm.AddField("roomGroupType", value11);
		}
		int num = 0;
		num = tempTimeOut;
		SingletonBehaviour<WebRequestManager>.Instance.Post(uri, wWWForm, delegate(UnityWebRequest request, bool hasErr, object userdata)
		{
			if (request.isDone)
			{
				bool flag = true;
				if (hasErr)
				{
					flag = false;
				}
				string text = request.downloadHandler.text;
				cb(flag ? "true" : "false", text, transIndex);
			}
		}, 0, num);
	}

	public void Shutdown()
	{
		Uninit();
		if (_httpRequest != null)
		{
			_httpRequest.Abort();
			_httpRequest.Dispose();
			_httpRequest = null;
		}
	}
}
