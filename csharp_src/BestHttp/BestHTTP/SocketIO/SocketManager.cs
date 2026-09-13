using System;
using System.Collections.Generic;
using System.Threading;
using BestHTTP.Extensions;
using BestHTTP.SocketIO.Events;
using BestHTTP.SocketIO.JsonEncoders;
using BestHTTP.SocketIO.Transports;

namespace BestHTTP.SocketIO;

public sealed class SocketManager : IHeartbeat, IManager
{
	public enum States
	{
		Initial,
		Closed,
		Opening,
		Open,
		Paused,
		Reconnecting
	}

	public static IJsonEncoder DefaultEncoder = new DefaultJSonEncoder();

	public const int MinProtocolVersion = 4;

	private States state;

	private int nextAckId;

	private Dictionary<string, Socket> Namespaces = new Dictionary<string, Socket>();

	private List<Socket> Sockets = new List<Socket>();

	private List<Packet> OfflinePackets;

	private DateTime LastHeartbeat = DateTime.MinValue;

	private DateTime ReconnectAt;

	private DateTime ConnectionStarted;

	private bool closing;

	private bool IsWaitingPong;

	public States State
	{
		get
		{
			return state;
		}
		private set
		{
			PreviousState = state;
			state = value;
		}
	}

	public SocketOptions Options { get; private set; }

	public Uri Uri { get; private set; }

	public HandshakeData Handshake { get; private set; }

	public ITransport Transport { get; private set; }

	public ulong RequestCounter { get; internal set; }

	public Socket Socket => GetSocket();

	public Socket this[string nsp] => GetSocket(nsp);

	public int ReconnectAttempts { get; private set; }

	public IJsonEncoder Encoder { get; set; }

	internal uint Timestamp => (uint)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;

	internal int NextAckId => Interlocked.Increment(ref nextAckId);

	internal States PreviousState { get; private set; }

	internal ITransport UpgradingTransport { get; set; }

	public SocketManager(Uri uri)
		: this(uri, new SocketOptions())
	{
	}

	public SocketManager(Uri uri, SocketOptions options)
	{
		Uri = uri;
		Options = options;
		State = States.Initial;
		PreviousState = States.Initial;
		Encoder = DefaultEncoder;
	}

	public Socket GetSocket()
	{
		return GetSocket("/");
	}

	public Socket GetSocket(string nsp)
	{
		if (string.IsNullOrEmpty(nsp))
		{
			throw new ArgumentNullException("Namespace parameter is null or empty!");
		}
		Socket value = null;
		if (!Namespaces.TryGetValue(nsp, out value))
		{
			value = new Socket(nsp, this);
			Namespaces.Add(nsp, value);
			Sockets.Add(value);
			((ISocket)value).Open();
		}
		return value;
	}

	void IManager.Remove(Socket socket)
	{
		Namespaces.Remove(socket.Namespace);
		Sockets.Remove(socket);
		if (Sockets.Count == 0)
		{
			Close();
		}
	}

	public void Open()
	{
		if (State == States.Initial || State == States.Closed || State == States.Reconnecting)
		{
			HTTPManager.Logger.Information("SocketManager", "Opening");
			ReconnectAt = DateTime.MinValue;
			switch (Options.ConnectWith)
			{
			case TransportTypes.Polling:
				Transport = new PollingTransport(this);
				break;
			case TransportTypes.WebSocket:
				Transport = new WebSocketTransport(this);
				break;
			}
			Transport.Open();
			((IManager)this).EmitEvent("connecting", Array.Empty<object>());
			State = States.Opening;
			ConnectionStarted = DateTime.UtcNow;
			HTTPManager.Heartbeats.Subscribe(this);
			GetSocket("/");
		}
	}

	public void Close()
	{
		((IManager)this).Close(removeSockets: true);
	}

	void IManager.Close(bool removeSockets)
	{
		if (State == States.Closed || closing)
		{
			return;
		}
		closing = true;
		HTTPManager.Logger.Information("SocketManager", "Closing");
		HTTPManager.Heartbeats.Unsubscribe(this);
		if (removeSockets)
		{
			while (Sockets.Count > 0)
			{
				((ISocket)Sockets[Sockets.Count - 1]).Disconnect(removeSockets);
			}
		}
		else
		{
			for (int i = 0; i < Sockets.Count; i++)
			{
				((ISocket)Sockets[i]).Disconnect(removeSockets);
			}
		}
		State = States.Closed;
		LastHeartbeat = DateTime.MinValue;
		if (OfflinePackets != null)
		{
			OfflinePackets.Clear();
		}
		if (removeSockets)
		{
			Namespaces.Clear();
		}
		Handshake = null;
		if (Transport != null)
		{
			Transport.Close();
		}
		Transport = null;
		closing = false;
	}

	void IManager.TryToReconnect()
	{
		if (State == States.Reconnecting || State == States.Closed)
		{
			return;
		}
		if (!Options.Reconnection || HTTPManager.IsQuitting)
		{
			Close();
			return;
		}
		if (++ReconnectAttempts >= Options.ReconnectionAttempts)
		{
			((IManager)this).EmitEvent("reconnect_failed", Array.Empty<object>());
			Close();
			return;
		}
		Random random = new Random();
		int num = (int)Options.ReconnectionDelay.TotalMilliseconds * ReconnectAttempts;
		ReconnectAt = DateTime.UtcNow + TimeSpan.FromMilliseconds(Math.Min(random.Next((int)((float)num - (float)num * Options.RandomizationFactor), (int)((float)num + (float)num * Options.RandomizationFactor)), (int)Options.ReconnectionDelayMax.TotalMilliseconds));
		((IManager)this).Close(removeSockets: false);
		State = States.Reconnecting;
		for (int i = 0; i < Sockets.Count; i++)
		{
			((ISocket)Sockets[i]).Open();
		}
		HTTPManager.Heartbeats.Subscribe(this);
		HTTPManager.Logger.Information("SocketManager", "Reconnecting");
	}

	bool IManager.OnTransportConnected(ITransport trans)
	{
		if (State != States.Opening)
		{
			return false;
		}
		if (PreviousState == States.Reconnecting)
		{
			((IManager)this).EmitEvent("reconnect", Array.Empty<object>());
		}
		State = States.Open;
		ReconnectAttempts = 0;
		SendOfflinePackets();
		HTTPManager.Logger.Information("SocketManager", "Open");
		if (Transport.Type != TransportTypes.WebSocket && Handshake.Upgrades.Contains("websocket"))
		{
			UpgradingTransport = new WebSocketTransport(this);
			UpgradingTransport.Open();
		}
		return true;
	}

	void IManager.OnTransportError(ITransport trans, string err)
	{
		((IManager)this).EmitError(SocketIOErrors.Internal, err);
		trans.Close();
		((IManager)this).TryToReconnect();
	}

	void IManager.OnTransportProbed(ITransport trans)
	{
		HTTPManager.Logger.Information("SocketManager", "\"probe\" packet received");
		Options.ConnectWith = trans.Type;
		State = States.Paused;
	}

	private ITransport SelectTransport()
	{
		if (State != States.Open || Transport == null)
		{
			return null;
		}
		if (!Transport.IsRequestInProgress)
		{
			return Transport;
		}
		return null;
	}

	private void SendOfflinePackets()
	{
		ITransport transport = SelectTransport();
		if (OfflinePackets != null && OfflinePackets.Count > 0 && transport != null)
		{
			transport.Send(OfflinePackets);
			OfflinePackets.Clear();
		}
	}

	void IManager.SendPacket(Packet packet)
	{
		ITransport transport = SelectTransport();
		if (transport != null)
		{
			try
			{
				transport.Send(packet);
				return;
			}
			catch (Exception ex)
			{
				((IManager)this).EmitError(SocketIOErrors.Internal, ex.Message + " " + ex.StackTrace);
				return;
			}
		}
		if (OfflinePackets == null)
		{
			OfflinePackets = new List<Packet>();
		}
		OfflinePackets.Add(packet.Clone());
	}

	void IManager.OnPacket(Packet packet)
	{
		if (State == States.Closed)
		{
			return;
		}
		switch (packet.TransportEvent)
		{
		case TransportEventTypes.Open:
			if (Handshake == null)
			{
				Handshake = new HandshakeData();
				if (!Handshake.Parse(packet.Payload))
				{
					HTTPManager.Logger.Warning("SocketManager", "Expected handshake data, but wasn't able to pars. Payload: " + packet.Payload);
				}
				((IManager)this).OnTransportConnected(Transport);
				return;
			}
			break;
		case TransportEventTypes.Ping:
			((IManager)this).SendPacket(new Packet(TransportEventTypes.Pong, SocketIOEventTypes.Unknown, "/", string.Empty));
			break;
		case TransportEventTypes.Pong:
			IsWaitingPong = false;
			break;
		}
		Socket value = null;
		if (Namespaces.TryGetValue(packet.Namespace, out value))
		{
			((ISocket)value).OnPacket(packet);
		}
		else
		{
			HTTPManager.Logger.Warning("SocketManager", "Namespace \"" + packet.Namespace + "\" not found!");
		}
	}

	public void EmitAll(string eventName, params object[] args)
	{
		for (int i = 0; i < Sockets.Count; i++)
		{
			Sockets[i].Emit(eventName, args);
		}
	}

	void IManager.EmitEvent(string eventName, params object[] args)
	{
		Socket value = null;
		if (Namespaces.TryGetValue("/", out value))
		{
			((ISocket)value).EmitEvent(eventName, args);
		}
	}

	void IManager.EmitEvent(SocketIOEventTypes type, params object[] args)
	{
		((IManager)this).EmitEvent(EventNames.GetNameFor(type), args);
	}

	void IManager.EmitError(SocketIOErrors errCode, string msg)
	{
		((IManager)this).EmitEvent(SocketIOEventTypes.Error, new object[1]
		{
			new Error(errCode, msg)
		});
	}

	void IManager.EmitAll(string eventName, params object[] args)
	{
		for (int i = 0; i < Sockets.Count; i++)
		{
			((ISocket)Sockets[i]).EmitEvent(eventName, args);
		}
	}

	void IHeartbeat.OnHeartbeatUpdate(TimeSpan dif)
	{
		switch (State)
		{
		case States.Paused:
			if (!Transport.IsRequestInProgress && !Transport.IsPollingInProgress)
			{
				State = States.Open;
				Transport.Close();
				Transport = UpgradingTransport;
				UpgradingTransport = null;
				Transport.Send(new Packet(TransportEventTypes.Upgrade, SocketIOEventTypes.Unknown, "/", string.Empty));
				goto case States.Open;
			}
			break;
		case States.Opening:
			if (DateTime.UtcNow - ConnectionStarted >= Options.Timeout)
			{
				((IManager)this).EmitError(SocketIOErrors.Internal, "Connection timed out!");
				((IManager)this).EmitEvent("connect_error", Array.Empty<object>());
				((IManager)this).EmitEvent("connect_timeout", Array.Empty<object>());
				((IManager)this).TryToReconnect();
			}
			break;
		case States.Reconnecting:
			if (ReconnectAt != DateTime.MinValue && DateTime.UtcNow >= ReconnectAt)
			{
				((IManager)this).EmitEvent("reconnect_attempt", Array.Empty<object>());
				((IManager)this).EmitEvent("reconnecting", Array.Empty<object>());
				Open();
			}
			break;
		case States.Open:
		{
			ITransport transport = null;
			if (Transport != null && Transport.State == TransportStates.Open)
			{
				transport = Transport;
			}
			if (transport == null || transport.State != TransportStates.Open)
			{
				break;
			}
			transport.Poll();
			SendOfflinePackets();
			if (LastHeartbeat == DateTime.MinValue)
			{
				LastHeartbeat = DateTime.UtcNow;
				break;
			}
			if (!IsWaitingPong && DateTime.UtcNow - LastHeartbeat > Handshake.PingInterval)
			{
				((IManager)this).SendPacket(new Packet(TransportEventTypes.Ping, SocketIOEventTypes.Unknown, "/", string.Empty));
				LastHeartbeat = DateTime.UtcNow;
				IsWaitingPong = true;
			}
			if (IsWaitingPong && DateTime.UtcNow - LastHeartbeat > Handshake.PingTimeout)
			{
				IsWaitingPong = false;
				((IManager)this).TryToReconnect();
			}
			break;
		}
		}
	}
}
