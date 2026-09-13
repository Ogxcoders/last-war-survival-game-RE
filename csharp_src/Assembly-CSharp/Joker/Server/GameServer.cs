using System.Collections.Generic;
using System.Net;

namespace Joker.Server;

public class GameServer : World, IStartupSync, IShutdownSync, IUpdate, ILateUpdate
{
	protected long _channel;

	protected readonly Dictionary<string, GameLocalActor> _players = new Dictionary<string, GameLocalActor>();

	protected readonly List<IGameActor> _updatePlayers = new List<IGameActor>();

	protected readonly Dictionary<string, GameRemoteActor> _remotePlayers = new Dictionary<string, GameRemoteActor>();

	protected readonly List<GameRoomOld> _rooms = new List<GameRoomOld>();

	public NetworkSystem Network { get; private set; }

	public RouterSystem Router { get; private set; }

	public void Startup()
	{
		Log.Info("GameServer Startup");
		Network = AddSystem<NetworkSystem, NetworkProtocol>(NetworkProtocol.TCP);
		Router = AddSystem<RouterSystem>();
		Network.OnMessage = _OnMessage;
		Network.OnAccept = _OnAccept;
		_channel = Network.Connect(new IPEndPoint(IPAddress.Loopback, 2525));
	}

	public void Shutdown()
	{
		Log.Info("GameServer Shutdown");
	}

	public void Update()
	{
		(IMessage, object) message;
		while (TryDequeueMessage(out message))
		{
			if (!Router.ProcessMessage(message.Item1))
			{
				DoProcessMessage(message.Item1, message.Item2);
			}
		}
	}

	public void LateUpdate()
	{
		for (int num = _updatePlayers.Count - 1; num >= 0; num--)
		{
			if (_updatePlayers[num].ProcessMessages())
			{
				_updatePlayers.RemoveSwapBackAt(num);
			}
		}
		for (int num2 = _rooms.Count - 1; num2 >= 0; num2--)
		{
			GameRoomOld gameRoomOld = _rooms[num2];
			gameRoomOld.Tick(Singleton<TimeService>.Instance.DeltaTime);
			if (!gameRoomOld.IsValid())
			{
				_rooms.RemoveSwapBackAt(num2);
				gameRoomOld.Dispose();
			}
		}
	}

	public GameRoomOld GetOrCreateRoom(string id, string level, int maxPlayers)
	{
		for (int i = 0; i < _rooms.Count; i++)
		{
			GameRoomOld gameRoomOld = _rooms[i];
			if (gameRoomOld.ID == id)
			{
				return gameRoomOld;
			}
		}
		GameRoomOld gameRoomOld2 = CreateRoom(id, level, maxPlayers);
		_rooms.Add(gameRoomOld2);
		return gameRoomOld2;
	}

	public GameRoomOld GetRoom(string id)
	{
		return _rooms.Find((GameRoomOld room) => room.ID == id);
	}

	public GameLocalActor GetLocalActor(string id)
	{
		if (_players.TryGetValue(id, out var value))
		{
			return value;
		}
		return null;
	}

	public GameRemoteActor GetRemoteActor(string account, string actor)
	{
		if (!_remotePlayers.TryGetValue(account, out var value))
		{
			value = CreateRemoteActor(-1, account, actor);
			_remotePlayers.Add(account, value);
		}
		return value;
	}

	private void _OnAccept(long channel, IPEndPoint address)
	{
		Log.Info($"GameServer connect to {address} {channel}");
	}

	private void _OnMessage(long channel, IMessage message)
	{
		Log.Debug($"GameServer {base.Id} OnMessage {channel} {message}");
		EnqueueMessage(message, channel);
	}

	protected virtual void DoProcessMessage(IMessage message, object sender)
	{
		if (message is IRouteActorMessage routeActorMessage)
		{
			if (TryGetGameActor(routeActorMessage.GetAccountId(), out var actor))
			{
				if (actor.EnqueueMessage(routeActorMessage, routeActorMessage.Message, sender))
				{
					_updatePlayers.Add(actor);
				}
			}
			else
			{
				Log.Error("can not find actor " + routeActorMessage.GetAccountId() + " " + routeActorMessage.GetActorId());
			}
		}
		else if (message is IRouteServerMessage { Message: var message2 } routeServerMessage)
		{
			if (message2 is C2SLogin c2SLogin)
			{
				if (TryAddGameActor(routeServerMessage.GetFrom(), c2SLogin.AccountId, c2SLogin.ActorId, out var actor2))
				{
					S2CLogin message3 = new S2CLogin
					{
						Code = 0,
						AccountId = c2SLogin.AccountId,
						ActorId = c2SLogin.ActorId
					};
					actor2.SendToServer(routeServerMessage.GetFrom(), message3);
				}
			}
			else if (message2 is C2SLogout c2SLogout)
			{
				if (TryGetGameActor(c2SLogout.AccountId, out var actor3))
				{
					GetRoom(actor3.ActorId)?.TryLeavelActor(actor3);
				}
				GameLocalActor actor4;
				bool flag = TryDelGameActor(c2SLogout.AccountId, out actor4);
				S2CLogout message4 = new S2CLogout
				{
					Code = ((!flag) ? 1 : 0),
					AccountId = c2SLogout.AccountId,
					ActorId = actor4.ActorId
				};
				actor4.SendToServer(routeServerMessage.GetFrom(), message4);
			}
		}
		Log.Info($"GameServer processing message: {message}");
	}

	protected virtual bool TryGetGameActor(string accountId, out IGameActor actor)
	{
		if (_players.TryGetValue(accountId, out var value))
		{
			actor = value;
			return true;
		}
		actor = null;
		return false;
	}

	protected virtual bool TryAddGameActor(int clientWorld, string accountId, string actorId, out GameLocalActor actor)
	{
		if (_players.TryGetValue(accountId, out var value))
		{
			actor = value;
			return true;
		}
		actor = CreateLocalActor(clientWorld, accountId, actorId);
		_players.Add(accountId, actor);
		return true;
	}

	protected virtual bool TryDelGameActor(string accountId, out GameLocalActor actor)
	{
		if (_players.TryGetValue(accountId, out actor))
		{
			_players.Remove(accountId);
			return true;
		}
		return false;
	}

	protected virtual GameLocalActor CreateLocalActor(int clientWorld, string accountId, string actorId)
	{
		return new GameLocalActor(this, clientWorld, accountId, actorId);
	}

	protected virtual GameRemoteActor CreateRemoteActor(int clientWorld, string accountId, string actorId)
	{
		return new GameRemoteActor(this, clientWorld, accountId, actorId);
	}

	protected virtual GameRoomOld CreateRoom(string roomId, string level, int maxPlayer)
	{
		return new GameRoomOld(this, roomId, level, maxPlayer);
	}
}
