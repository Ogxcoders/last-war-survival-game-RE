using System.Collections.Generic;
using System.Net;

namespace Joker;

public class NetworkSessionSystem : NetworkSystem
{
	protected readonly TCreateNetworkSession _factory;

	protected readonly Dictionary<long, NetworkSession> _sessions = new Dictionary<long, NetworkSession>();

	protected readonly Queue<(long channel, IMessage message)> _messages = new Queue<(long, IMessage)>();

	public NetworkSessionSystem(int port)
		: base(new IPEndPoint(IPAddress.Any, port), NetworkProtocol.TCP)
	{
		_factory = CreateSession;
		InitSessionCallback();
	}

	public NetworkSessionSystem(TCreateNetworkSession creator, int port)
		: base(new IPEndPoint(IPAddress.Any, port), NetworkProtocol.TCP)
	{
		_factory = creator;
		InitSessionCallback();
	}

	private void InitSessionCallback()
	{
		OnAccept = OnAcceptCallback;
		OnMessage = OnMessageCallback;
		OnError = OnErrorCallback;
		OnDisconnect = OnDisconnectCallback;
	}

	protected virtual void OnAcceptCallback(long chanelId, IPEndPoint address)
	{
		if (_sessions.ContainsKey(chanelId))
		{
			Log.Error($"OnAcceptCallback: Session already exists {chanelId}");
			return;
		}
		NetworkSession value = _factory(chanelId, address);
		_sessions.Add(chanelId, value);
	}

	protected virtual void OnMessageCallback(long channelId, IMessage message)
	{
		_messages.Enqueue((channelId, message));
	}

	protected virtual void OnErrorCallback(long channelId, int error)
	{
		if (error != 100208 && error != 10054)
		{
			Log.Error($"OnErrorCallback: error {error} : {channelId}");
		}
	}

	protected virtual void OnDisconnectCallback(long channelId)
	{
		if (_sessions.TryGetValue(channelId, out var value))
		{
			value.Dispose();
			_sessions.Remove(channelId);
		}
		else
		{
			Log.Error($"OnDisconnectCallback: Session not exists {channelId}");
		}
	}

	public override void Update()
	{
		base.Update();
		while (_messages.Count > 0)
		{
			(long, IMessage) tuple = _messages.Dequeue();
			if (_sessions.TryGetValue(tuple.Item1, out var value))
			{
				value.HandleMessage(tuple.Item2);
			}
			else
			{
				Log.Error($"OnMessageCallback: Session not exists {tuple.Item1}");
			}
		}
	}

	protected virtual NetworkSession CreateSession(long sessionId, IPEndPoint endPoint)
	{
		return new NetworkSession(this, sessionId, endPoint);
	}
}
