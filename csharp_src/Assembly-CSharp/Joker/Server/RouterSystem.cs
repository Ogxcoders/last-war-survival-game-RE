using System;

namespace Joker.Server;

public class RouterSystem : ISystem, IStartupSync, IShutdownSync
{
	private World _world;

	private NetworkSystem _network;

	private long _channel;

	World ISystem.World
	{
		get
		{
			return _world;
		}
		set
		{
			_world = value;
		}
	}

	public void Startup()
	{
		_network = _world.GetSystem<NetworkSystem>();
		NetworkSystem network = _network;
		network.OnConnect = (Action<long>)Delegate.Combine(network.OnConnect, new Action<long>(_OnConnected));
		NetworkSystem network2 = _network;
		network2.OnDisconnect = (Action<long>)Delegate.Combine(network2.OnDisconnect, new Action<long>(_OnDisconnected));
		NetworkSystem network3 = _network;
		network3.OnError = (Action<long, int>)Delegate.Combine(network3.OnError, new Action<long, int>(_OnError));
		_channel = _network.Connect(Singleton<WorldService>.Instance.GetWorldAddress(1000));
		Log.Info($"RouterSystem Startup {_channel}");
	}

	public void Shutdown()
	{
		Log.Info($"RouterSystem Shutdown {_channel}");
	}

	public void RouteMessage(IRouteServerMessage message)
	{
		if (message.GetTo() == _world.Id)
		{
			_world.EnqueueMessage(message.Message, message.GetFrom());
			return;
		}
		World world = Singleton<WorldService>.Instance.GetWorld(message.GetTo());
		if (world != null)
		{
			world.EnqueueMessage(MessageService.Instance.CloneMessage(message), null);
		}
		else
		{
			_network.Send(_channel, message);
		}
	}

	public bool ProcessMessage(IMessage message)
	{
		if (message is IRouteInnerMessage)
		{
			return true;
		}
		return false;
	}

	public void SendToWorld(int to, IMessage message)
	{
		S2SRouteServerMessage message2 = new S2SRouteServerMessage
		{
			From = _world.Id,
			To = to,
			Message = message
		};
		SendRaw(to, message2);
	}

	public void SendToWorldWithActor(int to, IMessage message, string accountId, string roomId)
	{
		S2SRouteServerWithActorMessage message2 = new S2SRouteServerWithActorMessage
		{
			From = _world.Id,
			To = to,
			Message = message,
			AccountId = accountId,
			TargetId = roomId
		};
		SendRaw(to, message2);
	}

	public void SendToActor(string accountId, string roomId, IMessage message)
	{
		S2SRouteActorMessage message2 = new S2SRouteActorMessage
		{
			Message = message,
			AccountId = accountId,
			RoomId = roomId
		};
		string id = (string.IsNullOrEmpty(roomId) ? accountId : roomId);
		int targetId = Singleton<WorldService>.Instance.GetTargetId(id);
		SendRaw(targetId, message2);
	}

	public void SendToActorWithServer(string accountId, string targetId, IMessage message)
	{
		int targetId2 = Singleton<WorldService>.Instance.GetTargetId(targetId);
		S2SRouteActorWithServerMessage message2 = new S2SRouteActorWithServerMessage
		{
			From = _world.Id,
			To = targetId2,
			AccountId = accountId,
			TargetId = targetId,
			Message = message
		};
		SendRaw(targetId2, message2);
	}

	public void SendToServer(string targetId, IMessage message)
	{
		int targetId2 = Singleton<WorldService>.Instance.GetTargetId(targetId);
		SendToWorld(targetId2, message);
	}

	public void SendRaw(int to, IMessage message)
	{
		World world = Singleton<WorldService>.Instance.GetWorld(to);
		if (world != null)
		{
			IMessage message2 = MessageService.Instance.CloneMessage(message);
			Log.Debug($"{world.Name} {world.Id} [local route] OnMessage {_channel} {message2}");
			world.EnqueueMessage(message2, _channel);
		}
		else
		{
			_network.Send(_channel, message);
		}
	}

	public void _OnConnected(long channel)
	{
		if (_channel == channel)
		{
			S2RRouterRegisterMessage s2RRouterRegisterMessage = new S2RRouterRegisterMessage();
			s2RRouterRegisterMessage.Node = _world.Id;
			s2RRouterRegisterMessage.FromChannel = _channel;
			_network.Send(_channel, s2RRouterRegisterMessage);
			Log.Info($"RegisterServerMessage {_channel} {s2RRouterRegisterMessage.Node}");
		}
	}

	public void _OnDisconnected(long channel)
	{
		if (channel == _channel)
		{
			_channel = -1L;
		}
	}

	private void _OnError(long channel, int error)
	{
		if (error == 100208)
		{
			Log.Info($"Disconnected from client {channel}");
		}
		else
		{
			Log.Error($"RouterSystem _OnError {channel} {NetworkSystem.GetErrorDesc(error)} world:{_world.Id}");
		}
	}
}
