using System;
using BestHTTP.SignalR.Messages;
using BestHTTP.WebSocket;

namespace BestHTTP.SignalR.Transports;

public sealed class WebSocketTransport : TransportBase
{
	private BestHTTP.WebSocket.WebSocket wSocket;

	public override bool SupportsKeepAlive => true;

	public override TransportTypes Type => TransportTypes.WebSocket;

	public WebSocketTransport(Connection connection)
		: base("webSockets", connection)
	{
	}

	public override void Connect()
	{
		if (wSocket != null)
		{
			HTTPManager.Logger.Warning("WebSocketTransport", "Start - WebSocket already created!");
			return;
		}
		if (base.State != TransportStates.Reconnecting)
		{
			base.State = TransportStates.Connecting;
		}
		RequestTypes type = ((base.State != TransportStates.Reconnecting) ? RequestTypes.Connect : RequestTypes.Reconnect);
		Uri uri = base.Connection.BuildUri(type, this);
		wSocket = new BestHTTP.WebSocket.WebSocket(uri);
		BestHTTP.WebSocket.WebSocket webSocket = wSocket;
		webSocket.OnOpen = (OnWebSocketOpenDelegate)Delegate.Combine(webSocket.OnOpen, new OnWebSocketOpenDelegate(WSocket_OnOpen));
		BestHTTP.WebSocket.WebSocket webSocket2 = wSocket;
		webSocket2.OnMessage = (OnWebSocketMessageDelegate)Delegate.Combine(webSocket2.OnMessage, new OnWebSocketMessageDelegate(WSocket_OnMessage));
		BestHTTP.WebSocket.WebSocket webSocket3 = wSocket;
		webSocket3.OnClosed = (OnWebSocketClosedDelegate)Delegate.Combine(webSocket3.OnClosed, new OnWebSocketClosedDelegate(WSocket_OnClosed));
		BestHTTP.WebSocket.WebSocket webSocket4 = wSocket;
		webSocket4.OnErrorDesc = (OnWebSocketErrorDescriptionDelegate)Delegate.Combine(webSocket4.OnErrorDesc, new OnWebSocketErrorDescriptionDelegate(WSocket_OnError));
		base.Connection.PrepareRequest(wSocket.InternalRequest, type);
		wSocket.Open();
	}

	protected override void SendImpl(string json)
	{
		if (wSocket != null && wSocket.IsOpen)
		{
			wSocket.Send(json);
		}
	}

	public override void Stop()
	{
		if (wSocket != null)
		{
			wSocket.OnOpen = null;
			wSocket.OnMessage = null;
			wSocket.OnClosed = null;
			wSocket.OnErrorDesc = null;
			wSocket.Close();
			wSocket = null;
		}
	}

	protected override void Started()
	{
	}

	protected override void Aborted()
	{
		if (wSocket != null && wSocket.IsOpen)
		{
			wSocket.Close();
			wSocket = null;
		}
	}

	private void WSocket_OnOpen(BestHTTP.WebSocket.WebSocket webSocket)
	{
		if (webSocket == wSocket)
		{
			HTTPManager.Logger.Information("WebSocketTransport", "WSocket_OnOpen");
			OnConnected();
		}
	}

	private void WSocket_OnMessage(BestHTTP.WebSocket.WebSocket webSocket, string message)
	{
		if (webSocket == wSocket)
		{
			IServerMessage serverMessage = TransportBase.Parse(base.Connection.JsonEncoder, message);
			if (serverMessage != null)
			{
				base.Connection.OnMessage(serverMessage);
			}
		}
	}

	private void WSocket_OnClosed(BestHTTP.WebSocket.WebSocket webSocket, ushort code, string message)
	{
		if (webSocket == wSocket)
		{
			string text = code + " : " + message;
			HTTPManager.Logger.Information("WebSocketTransport", "WSocket_OnClosed " + text);
			if (base.State == TransportStates.Closing)
			{
				base.State = TransportStates.Closed;
			}
			else
			{
				base.Connection.Error(text);
			}
		}
	}

	private void WSocket_OnError(BestHTTP.WebSocket.WebSocket webSocket, string reason)
	{
		if (webSocket == wSocket)
		{
			if (base.State == TransportStates.Closing || base.State == TransportStates.Closed)
			{
				AbortFinished();
				return;
			}
			HTTPManager.Logger.Error("WebSocketTransport", "WSocket_OnError " + reason);
			base.State = TransportStates.Closed;
			base.Connection.Error(reason);
		}
	}
}
