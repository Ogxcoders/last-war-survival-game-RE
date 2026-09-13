using System;
using System.Collections.Generic;
using BestHTTP.Extensions;
using BestHTTP.WebSocket;

namespace BestHTTP.SocketIO.Transports;

internal sealed class WebSocketTransport : ITransport
{
	private Packet PacketWithAttachment;

	private byte[] Buffer;

	public TransportTypes Type => TransportTypes.WebSocket;

	public TransportStates State { get; private set; }

	public SocketManager Manager { get; private set; }

	public bool IsRequestInProgress => false;

	public bool IsPollingInProgress => false;

	public BestHTTP.WebSocket.WebSocket Implementation { get; private set; }

	public WebSocketTransport(SocketManager manager)
	{
		State = TransportStates.Closed;
		Manager = manager;
	}

	public void Open()
	{
		if (State == TransportStates.Closed)
		{
			Uri uri = null;
			string text = new UriBuilder(HTTPProtocolFactory.IsSecureProtocol(Manager.Uri) ? "wss" : "ws", Manager.Uri.Host, Manager.Uri.Port, Manager.Uri.GetRequestPathAndQueryURL()).Uri.ToString();
			string text2 = "{0}?EIO={1}&transport=websocket{3}";
			if (Manager.Handshake != null)
			{
				text2 += "&sid={2}";
			}
			bool flag = !Manager.Options.QueryParamsOnlyForHandshake || (Manager.Options.QueryParamsOnlyForHandshake && Manager.Handshake == null);
			uri = new Uri(string.Format(text2, text, 4, (Manager.Handshake != null) ? Manager.Handshake.Sid : string.Empty, flag ? Manager.Options.BuildQueryParams() : string.Empty));
			Implementation = new BestHTTP.WebSocket.WebSocket(uri);
			Implementation.OnOpen = OnOpen;
			Implementation.OnMessage = OnMessage;
			Implementation.OnBinary = OnBinary;
			Implementation.OnError = OnError;
			Implementation.OnClosed = OnClosed;
			Implementation.Open();
			State = TransportStates.Connecting;
		}
	}

	public void Close()
	{
		if (State != TransportStates.Closed)
		{
			State = TransportStates.Closed;
			if (Implementation != null)
			{
				Implementation.Close();
			}
			else
			{
				HTTPManager.Logger.Warning("WebSocketTransport", "Close - WebSocket Implementation already null!");
			}
			Implementation = null;
		}
	}

	public void Poll()
	{
	}

	private void OnOpen(BestHTTP.WebSocket.WebSocket ws)
	{
		if (ws == Implementation)
		{
			HTTPManager.Logger.Information("WebSocketTransport", "OnOpen");
			State = TransportStates.Opening;
			if (Manager.UpgradingTransport == this)
			{
				Send(new Packet(TransportEventTypes.Ping, SocketIOEventTypes.Unknown, "/", "probe"));
			}
		}
	}

	private void OnMessage(BestHTTP.WebSocket.WebSocket ws, string message)
	{
		if (ws != Implementation)
		{
			return;
		}
		if ((int)HTTPManager.Logger.Level <= 0)
		{
			HTTPManager.Logger.Verbose("WebSocketTransport", "OnMessage: " + message);
		}
		Packet packet = null;
		try
		{
			packet = new Packet(message);
		}
		catch (Exception ex)
		{
			HTTPManager.Logger.Exception("WebSocketTransport", "OnMessage Packet parsing", ex);
		}
		if (packet == null)
		{
			HTTPManager.Logger.Error("WebSocketTransport", "Message parsing failed. Message: " + message);
			return;
		}
		try
		{
			if (packet.AttachmentCount == 0)
			{
				OnPacket(packet);
			}
			else
			{
				PacketWithAttachment = packet;
			}
		}
		catch (Exception ex2)
		{
			HTTPManager.Logger.Exception("WebSocketTransport", "OnMessage OnPacket", ex2);
		}
	}

	private void OnBinary(BestHTTP.WebSocket.WebSocket ws, byte[] data)
	{
		if (ws != Implementation)
		{
			return;
		}
		if ((int)HTTPManager.Logger.Level <= 0)
		{
			HTTPManager.Logger.Verbose("WebSocketTransport", "OnBinary");
		}
		if (PacketWithAttachment == null)
		{
			return;
		}
		PacketWithAttachment.AddAttachmentFromServer(data, copyFull: false);
		if (!PacketWithAttachment.HasAllAttachment)
		{
			return;
		}
		try
		{
			OnPacket(PacketWithAttachment);
		}
		catch (Exception ex)
		{
			HTTPManager.Logger.Exception("WebSocketTransport", "OnBinary", ex);
		}
		finally
		{
			PacketWithAttachment = null;
		}
	}

	private void OnError(BestHTTP.WebSocket.WebSocket ws, Exception ex)
	{
		if (ws != Implementation)
		{
			return;
		}
		string err = string.Empty;
		if (ex != null)
		{
			err = ex.Message + " " + ex.StackTrace;
		}
		else
		{
			switch (ws.InternalRequest.State)
			{
			case HTTPRequestStates.Finished:
				err = ((!ws.InternalRequest.Response.IsSuccess && ws.InternalRequest.Response.StatusCode != 101) ? $"Request Finished Successfully, but the server sent an error. Status Code: {ws.InternalRequest.Response.StatusCode}-{ws.InternalRequest.Response.Message} Message: {ws.InternalRequest.Response.DataAsText}" : $"Request finished. Status Code: {ws.InternalRequest.Response.StatusCode.ToString()} Message: {ws.InternalRequest.Response.Message}");
				break;
			case HTTPRequestStates.Error:
				err = (("Request Finished with Error! : " + ws.InternalRequest.Exception != null) ? (ws.InternalRequest.Exception.Message + " " + ws.InternalRequest.Exception.StackTrace) : string.Empty);
				break;
			case HTTPRequestStates.Aborted:
				err = "Request Aborted!";
				break;
			case HTTPRequestStates.ConnectionTimedOut:
				err = "Connection Timed Out!";
				break;
			case HTTPRequestStates.TimedOut:
				err = "Processing the request Timed Out!";
				break;
			}
		}
		if (Manager.UpgradingTransport != this)
		{
			((IManager)Manager).OnTransportError((ITransport)this, err);
		}
		else
		{
			Manager.UpgradingTransport = null;
		}
	}

	private void OnClosed(BestHTTP.WebSocket.WebSocket ws, ushort code, string message)
	{
		if (ws == Implementation)
		{
			HTTPManager.Logger.Information("WebSocketTransport", "OnClosed");
			Close();
			if (Manager.UpgradingTransport != this)
			{
				((IManager)Manager).TryToReconnect();
			}
			else
			{
				Manager.UpgradingTransport = null;
			}
		}
	}

	public void Send(Packet packet)
	{
		if (State == TransportStates.Closed || State == TransportStates.Paused)
		{
			return;
		}
		string text = packet.Encode();
		if ((int)HTTPManager.Logger.Level <= 0)
		{
			HTTPManager.Logger.Verbose("WebSocketTransport", "Send: " + text);
		}
		if (packet.AttachmentCount != 0 || (packet.Attachments != null && packet.Attachments.Count != 0))
		{
			if (packet.Attachments == null)
			{
				throw new ArgumentException("packet.Attachments are null!");
			}
			if (packet.AttachmentCount != packet.Attachments.Count)
			{
				throw new ArgumentException("packet.AttachmentCount != packet.Attachments.Count. Use the packet.AddAttachment function to add data to a packet!");
			}
		}
		Implementation.Send(text);
		if (packet.AttachmentCount == 0)
		{
			return;
		}
		int num = packet.Attachments[0].Length + 1;
		for (int i = 1; i < packet.Attachments.Count; i++)
		{
			if (packet.Attachments[i].Length + 1 > num)
			{
				num = packet.Attachments[i].Length + 1;
			}
		}
		if (Buffer == null || Buffer.Length < num)
		{
			Array.Resize(ref Buffer, num);
		}
		for (int j = 0; j < packet.AttachmentCount; j++)
		{
			Buffer[0] = 4;
			Array.Copy(packet.Attachments[j], 0, Buffer, 1, packet.Attachments[j].Length);
			Implementation.Send(Buffer, 0uL, (ulong)packet.Attachments[j].Length + 1uL);
		}
	}

	public void Send(List<Packet> packets)
	{
		for (int i = 0; i < packets.Count; i++)
		{
			Send(packets[i]);
		}
		packets.Clear();
	}

	private void OnPacket(Packet packet)
	{
		switch (packet.TransportEvent)
		{
		case TransportEventTypes.Open:
			if (State != TransportStates.Opening)
			{
				HTTPManager.Logger.Warning("WebSocketTransport", "Received 'Open' packet while state is '" + State.ToString() + "'");
			}
			else
			{
				State = TransportStates.Open;
			}
			break;
		case TransportEventTypes.Pong:
			if (packet.Payload == "probe")
			{
				State = TransportStates.Open;
				((IManager)Manager).OnTransportProbed((ITransport)this);
			}
			break;
		}
		if (Manager.UpgradingTransport != this)
		{
			((IManager)Manager).OnPacket(packet);
		}
	}
}
