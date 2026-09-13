using System;
using System.Collections.Generic;
using BestHTTP.JSON;
using BestHTTP.SocketIO.Events;

namespace BestHTTP.SocketIO;

public sealed class Socket : ISocket
{
	private Dictionary<int, SocketIOAckCallback> AckCallbacks;

	private EventTable EventCallbacks;

	private List<object> arguments = new List<object>();

	public SocketManager Manager { get; private set; }

	public string Namespace { get; private set; }

	public string Id { get; private set; }

	public bool IsOpen { get; private set; }

	public bool AutoDecodePayload { get; set; }

	internal Socket(string nsp, SocketManager manager)
	{
		Namespace = nsp;
		Manager = manager;
		IsOpen = false;
		AutoDecodePayload = true;
		EventCallbacks = new EventTable(this);
	}

	void ISocket.Open()
	{
		if (Manager.State == SocketManager.States.Open)
		{
			OnTransportOpen(Manager.Socket, null);
			return;
		}
		Manager.Socket.Off("connect", OnTransportOpen);
		Manager.Socket.On("connect", OnTransportOpen);
		if (Manager.Options.AutoConnect && Manager.State == SocketManager.States.Initial)
		{
			Manager.Open();
		}
	}

	public void Disconnect()
	{
		((ISocket)this).Disconnect(remove: true);
	}

	void ISocket.Disconnect(bool remove)
	{
		if (IsOpen)
		{
			Packet packet = new Packet(TransportEventTypes.Message, SocketIOEventTypes.Disconnect, Namespace, string.Empty);
			((IManager)Manager).SendPacket(packet);
			IsOpen = false;
			((ISocket)this).OnPacket(packet);
		}
		if (AckCallbacks != null)
		{
			AckCallbacks.Clear();
		}
		if (remove)
		{
			EventCallbacks.Clear();
			((IManager)Manager).Remove(this);
		}
	}

	public Socket Emit(string eventName, params object[] args)
	{
		return Emit(eventName, null, args);
	}

	public Socket Emit(string eventName, SocketIOAckCallback callback, params object[] args)
	{
		if (EventNames.IsBlacklisted(eventName))
		{
			throw new ArgumentException("Blacklisted event: " + eventName);
		}
		arguments.Clear();
		arguments.Add(eventName);
		List<byte[]> list = null;
		if (args != null && args.Length != 0)
		{
			int num = 0;
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i] is byte[] item)
				{
					if (list == null)
					{
						list = new List<byte[]>();
					}
					Dictionary<string, object> dictionary = new Dictionary<string, object>(2);
					dictionary.Add("_placeholder", true);
					dictionary.Add("num", num++);
					arguments.Add(dictionary);
					list.Add(item);
				}
				else
				{
					arguments.Add(args[i]);
				}
			}
		}
		string text = null;
		try
		{
			text = Manager.Encoder.Encode(arguments);
		}
		catch (Exception ex)
		{
			((ISocket)this).EmitError(SocketIOErrors.Internal, "Error while encoding payload: " + ex.Message + " " + ex.StackTrace);
			return this;
		}
		arguments.Clear();
		if (text == null)
		{
			throw new ArgumentException("Encoding the arguments to JSON failed!");
		}
		int num2 = 0;
		if (callback != null)
		{
			num2 = Manager.NextAckId;
			if (AckCallbacks == null)
			{
				AckCallbacks = new Dictionary<int, SocketIOAckCallback>();
			}
			AckCallbacks[num2] = callback;
		}
		Packet packet = new Packet(TransportEventTypes.Message, (list == null) ? SocketIOEventTypes.Event : SocketIOEventTypes.BinaryEvent, Namespace, text, 0, num2);
		if (list != null)
		{
			packet.Attachments = list;
		}
		((IManager)Manager).SendPacket(packet);
		return this;
	}

	public Socket EmitAck(Packet originalPacket, params object[] args)
	{
		if (originalPacket == null)
		{
			throw new ArgumentNullException("originalPacket == null!");
		}
		if (originalPacket.SocketIOEvent != SocketIOEventTypes.Event && originalPacket.SocketIOEvent != SocketIOEventTypes.BinaryEvent)
		{
			throw new ArgumentException("Wrong packet - you can't send an Ack for a packet with id == 0 and SocketIOEvent != Event or SocketIOEvent != BinaryEvent!");
		}
		arguments.Clear();
		if (args != null && args.Length != 0)
		{
			arguments.AddRange(args);
		}
		string text = null;
		try
		{
			text = Manager.Encoder.Encode(arguments);
		}
		catch (Exception ex)
		{
			((ISocket)this).EmitError(SocketIOErrors.Internal, "Error while encoding payload: " + ex.Message + " " + ex.StackTrace);
			return this;
		}
		if (text == null)
		{
			throw new ArgumentException("Encoding the arguments to JSON failed!");
		}
		Packet packet = new Packet(TransportEventTypes.Message, (originalPacket.SocketIOEvent == SocketIOEventTypes.Event) ? SocketIOEventTypes.Ack : SocketIOEventTypes.BinaryAck, Namespace, text, 0, originalPacket.Id);
		((IManager)Manager).SendPacket(packet);
		return this;
	}

	public void On(string eventName, SocketIOCallback callback)
	{
		EventCallbacks.Register(eventName, callback, onlyOnce: false, AutoDecodePayload);
	}

	public void On(SocketIOEventTypes type, SocketIOCallback callback)
	{
		string nameFor = EventNames.GetNameFor(type);
		EventCallbacks.Register(nameFor, callback, onlyOnce: false, AutoDecodePayload);
	}

	public void On(string eventName, SocketIOCallback callback, bool autoDecodePayload)
	{
		EventCallbacks.Register(eventName, callback, onlyOnce: false, autoDecodePayload);
	}

	public void On(SocketIOEventTypes type, SocketIOCallback callback, bool autoDecodePayload)
	{
		string nameFor = EventNames.GetNameFor(type);
		EventCallbacks.Register(nameFor, callback, onlyOnce: false, autoDecodePayload);
	}

	public void Once(string eventName, SocketIOCallback callback)
	{
		EventCallbacks.Register(eventName, callback, onlyOnce: true, AutoDecodePayload);
	}

	public void Once(SocketIOEventTypes type, SocketIOCallback callback)
	{
		EventCallbacks.Register(EventNames.GetNameFor(type), callback, onlyOnce: true, AutoDecodePayload);
	}

	public void Once(string eventName, SocketIOCallback callback, bool autoDecodePayload)
	{
		EventCallbacks.Register(eventName, callback, onlyOnce: true, autoDecodePayload);
	}

	public void Once(SocketIOEventTypes type, SocketIOCallback callback, bool autoDecodePayload)
	{
		EventCallbacks.Register(EventNames.GetNameFor(type), callback, onlyOnce: true, autoDecodePayload);
	}

	public void Off()
	{
		EventCallbacks.Clear();
	}

	public void Off(string eventName)
	{
		EventCallbacks.Unregister(eventName);
	}

	public void Off(SocketIOEventTypes type)
	{
		Off(EventNames.GetNameFor(type));
	}

	public void Off(string eventName, SocketIOCallback callback)
	{
		EventCallbacks.Unregister(eventName, callback);
	}

	public void Off(SocketIOEventTypes type, SocketIOCallback callback)
	{
		EventCallbacks.Unregister(EventNames.GetNameFor(type), callback);
	}

	void ISocket.OnPacket(Packet packet)
	{
		switch (packet.SocketIOEvent)
		{
		case SocketIOEventTypes.Connect:
			Id = ((Namespace != "/") ? (Namespace + "#" + Manager.Handshake.Sid) : Manager.Handshake.Sid);
			break;
		case SocketIOEventTypes.Disconnect:
			if (IsOpen)
			{
				IsOpen = false;
				EventCallbacks.Call(EventNames.GetNameFor(SocketIOEventTypes.Disconnect), packet);
				Disconnect();
			}
			break;
		case SocketIOEventTypes.Error:
		{
			bool success = false;
			object obj = Json.Decode(packet.Payload, ref success);
			if (success)
			{
				Error error = ((!(obj is Dictionary<string, object> dictionary) || !dictionary.ContainsKey("code")) ? new Error(SocketIOErrors.Custom, packet.Payload) : new Error((SocketIOErrors)Convert.ToInt32(dictionary["code"]), dictionary["message"] as string));
				EventCallbacks.Call(EventNames.GetNameFor(SocketIOEventTypes.Error), packet, error);
				return;
			}
			break;
		}
		}
		EventCallbacks.Call(packet);
		if ((packet.SocketIOEvent != SocketIOEventTypes.Ack && packet.SocketIOEvent != SocketIOEventTypes.BinaryAck) || AckCallbacks == null)
		{
			return;
		}
		SocketIOAckCallback value = null;
		if (AckCallbacks.TryGetValue(packet.Id, out value) && value != null)
		{
			try
			{
				value(this, packet, AutoDecodePayload ? packet.Decode(Manager.Encoder) : null);
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("Socket", "ackCallback", ex);
			}
		}
		AckCallbacks.Remove(packet.Id);
	}

	void ISocket.EmitEvent(SocketIOEventTypes type, params object[] args)
	{
		((ISocket)this).EmitEvent(EventNames.GetNameFor(type), args);
	}

	void ISocket.EmitEvent(string eventName, params object[] args)
	{
		if (!string.IsNullOrEmpty(eventName))
		{
			EventCallbacks.Call(eventName, null, args);
		}
	}

	void ISocket.EmitError(SocketIOErrors errCode, string msg)
	{
		((ISocket)this).EmitEvent(SocketIOEventTypes.Error, new object[1]
		{
			new Error(errCode, msg)
		});
	}

	private void OnTransportOpen(Socket socket, Packet packet, params object[] args)
	{
		if (Namespace != "/")
		{
			((IManager)Manager).SendPacket(new Packet(TransportEventTypes.Message, SocketIOEventTypes.Connect, Namespace, string.Empty));
		}
		IsOpen = true;
	}
}
