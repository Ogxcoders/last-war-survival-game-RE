using System;
using System.Collections.Generic;
using BaseUtils;
using BestHTTP.WebSocket;
using BestHTTP.WebSocket.Frames;
using GameFramework;
using UnityEngine;

public class WebSocketItem
{
	private struct MsgItem
	{
		public string k;

		public string v;

		public MsgItem(string _k, string _v)
		{
			k = _k;
			v = _v;
		}
	}

	private Action<string, string, int> callback_;

	private WebSocket socket;

	private object locker = new object();

	private Queue<MsgItem> msgQueue = new Queue<MsgItem>();

	private int msgItemNo;

	private float last_ping_time_;

	private string sign;

	private string CalcSign(string appId, string uid, long time)
	{
		return BaseUtils.StringUtils.GetMD5(BaseUtils.StringUtils.GetMD5(appId + uid) + time);
	}

	public void SetCallBack(Action<string, string, int> cb)
	{
		callback_ = cb;
	}

	public bool IsOpen()
	{
		if (socket != null && socket.IsOpen)
		{
			return true;
		}
		return false;
	}

	public string GetSign()
	{
		return sign;
	}

	public bool Connect(string APP_ID, string protocol, string ip, int port, string token)
	{
		long serverTime = GameEntry.Timer.GetServerTime();
		string uid = GameEntry.Data.Player.Uid;
		sign = CalcSign(APP_ID, uid, serverTime);
		string uriString = $"{protocol}://{ip}:{port}";
		socket = new WebSocket(new Uri(uriString))
		{
			StartPingThread = true
		};
		socket.PingFrequency = 3000;
		socket.CloseAfterNoMesssage = TimeSpan.FromSeconds(9.0);
		socket.InternalRequest.SetHeader("APPID", APP_ID);
		socket.InternalRequest.SetHeader("TIME", serverTime.ToString());
		socket.InternalRequest.SetHeader("UID", uid);
		socket.InternalRequest.SetHeader("SIGN", sign);
		socket.InternalRequest.SetHeader("CHATTOKEN", token);
		WebSocket webSocket = socket;
		webSocket.OnOpen = (OnWebSocketOpenDelegate)Delegate.Combine(webSocket.OnOpen, new OnWebSocketOpenDelegate(Socket_OnOpen));
		WebSocket webSocket2 = socket;
		webSocket2.OnMessage = (OnWebSocketMessageDelegate)Delegate.Combine(webSocket2.OnMessage, new OnWebSocketMessageDelegate(Socket_OnMessage));
		WebSocket webSocket3 = socket;
		webSocket3.OnBinary = (OnWebSocketBinaryDelegate)Delegate.Combine(webSocket3.OnBinary, new OnWebSocketBinaryDelegate(Socket_OnBinary));
		WebSocket webSocket4 = socket;
		webSocket4.OnClosed = (OnWebSocketClosedDelegate)Delegate.Combine(webSocket4.OnClosed, new OnWebSocketClosedDelegate(Socket_OnClosed));
		WebSocket webSocket5 = socket;
		webSocket5.OnError = (OnWebSocketErrorDelegate)Delegate.Combine(webSocket5.OnError, new OnWebSocketErrorDelegate(Socket_OnError));
		WebSocket webSocket6 = socket;
		webSocket6.OnIncompleteFrame = (OnWebSocketIncompleteFrameDelegate)Delegate.Combine(webSocket6.OnIncompleteFrame, new OnWebSocketIncompleteFrameDelegate(Socket_OnIncompleteFrame));
		socket.Open();
		last_ping_time_ = Time.realtimeSinceStartup;
		return true;
	}

	public void Send(string json)
	{
		if (socket != null)
		{
			socket.Send(json);
		}
	}

	public void Process()
	{
		try
		{
			lock (locker)
			{
				while (msgQueue.Count > 0)
				{
					MsgItem msgItem = msgQueue.Dequeue();
					msgItemNo++;
					if (callback_ != null)
					{
						callback_(msgItem.k, msgItem.v, msgItemNo);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error("ChatService Process exception!! \n " + ex.ToString());
		}
	}

	public void Close()
	{
		if (socket != null)
		{
			socket.Close();
		}
	}

	private void Socket_OnOpen(WebSocket webSocket)
	{
		lock (locker)
		{
			msgQueue.Enqueue(new MsgItem("onOpen", ""));
		}
	}

	private void Socket_OnMessage(WebSocket webSocket, string message)
	{
		if (message.Equals("heartbeat"))
		{
			last_ping_time_ = Time.realtimeSinceStartup;
			return;
		}
		try
		{
			lock (locker)
			{
				msgQueue.Enqueue(new MsgItem("onMessage", message));
			}
		}
		catch (Exception message2)
		{
			Debug.LogError(message2);
		}
	}

	private void Socket_OnBinary(WebSocket webSocket, byte[] data)
	{
	}

	private void Socket_OnClosed(WebSocket webSocket, ushort code, string message)
	{
		lock (locker)
		{
			msgQueue.Enqueue(new MsgItem("onClose", message));
		}
	}

	private void Socket_OnError(WebSocket webSocket, Exception ex)
	{
		if (ex != null)
		{
			Log.Error("{0}, {1}", webSocket.InternalRequest.Uri, ex);
		}
		if (webSocket != null && webSocket.IsOpen)
		{
			webSocket.OnOpen = (OnWebSocketOpenDelegate)Delegate.Remove(webSocket.OnOpen, new OnWebSocketOpenDelegate(Socket_OnOpen));
			webSocket.OnMessage = (OnWebSocketMessageDelegate)Delegate.Remove(webSocket.OnMessage, new OnWebSocketMessageDelegate(Socket_OnMessage));
			webSocket.OnBinary = (OnWebSocketBinaryDelegate)Delegate.Remove(webSocket.OnBinary, new OnWebSocketBinaryDelegate(Socket_OnBinary));
			webSocket.OnClosed = (OnWebSocketClosedDelegate)Delegate.Remove(webSocket.OnClosed, new OnWebSocketClosedDelegate(Socket_OnClosed));
			webSocket.OnError = (OnWebSocketErrorDelegate)Delegate.Remove(webSocket.OnError, new OnWebSocketErrorDelegate(Socket_OnError));
			webSocket.OnIncompleteFrame = (OnWebSocketIncompleteFrameDelegate)Delegate.Remove(webSocket.OnIncompleteFrame, new OnWebSocketIncompleteFrameDelegate(Socket_OnIncompleteFrame));
			webSocket.Close();
		}
		lock (locker)
		{
			msgQueue.Enqueue(new MsgItem("onError", (ex != null) ? ex.Message : ""));
		}
	}

	private void Socket_OnIncompleteFrame(WebSocket webSocket, WebSocketFrameReader frame)
	{
	}
}
