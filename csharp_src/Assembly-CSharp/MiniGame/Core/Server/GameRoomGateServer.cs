using System.Collections.Generic;
using System.Net;
using Joker;

namespace MiniGame.Core.Server;

public class GameRoomGateServer : World, IGameServer, IStartupSync, IShutdownSync, IUpdate
{
	private readonly int _idBase;

	private readonly int _idCount;

	private Dictionary<PlayerSession, string> _sessionToId = new Dictionary<PlayerSession, string>();

	private Dictionary<string, PlayerSession> _idToSession = new Dictionary<string, PlayerSession>();

	public NetworkSessionSystem Network { get; private set; }

	public GameRoomGateServer(int idBase, int idCount)
	{
		_idBase = idBase;
		_idCount = idCount;
	}

	public void Startup()
	{
		Network = AddSystem<NetworkSessionSystem, TCreateNetworkSession, int>(CreatePlayerSession, 7758);
	}

	public void Shutdown()
	{
	}

	public void Update()
	{
		(IMessage, object) message;
		while (TryDequeueMessage(out message))
		{
			HandleMessage(message.Item1, message.Item2 as NetworkSession);
		}
	}

	private World GetWorld(string room)
	{
		int targetId = WorldService.GetTargetId(room, _idBase, _idCount);
		return Singleton<WorldService>.Instance.GetWorld(targetId);
	}

	public void ConnectPlayer(PlayerSession session)
	{
		_sessionToId.Add(session, null);
	}

	public void LoginPlayer(PlayerSession session)
	{
		_sessionToId[session] = session.PlayerSessionID;
		_idToSession.Add(session.PlayerSessionID, session);
	}

	public void LogoutPlayer(PlayerSession session)
	{
		_sessionToId.Remove(session);
		_idToSession.Remove(session.PlayerSessionID);
	}

	public void DisconnectPlayer(PlayerSession session)
	{
		_sessionToId.Remove(session);
	}

	private PlayerSession GetPlayer(string sessionId)
	{
		return _idToSession[sessionId];
	}

	private void HandleMessage(IMessage message, NetworkSession session)
	{
		if (!(message is RoomMessage roomMessage))
		{
			Log.Error("Unknown message type {0}", message.GetType());
		}
		else if (session == null)
		{
			GetPlayer(roomMessage.PlayerSessionID).SendMessage(message);
		}
		else
		{
			GetWorld(roomMessage.RoomSessionID).EnqueueMessage(message, roomMessage.PlayerSessionID);
		}
	}

	private NetworkSession CreatePlayerSession(long sessionId, IPEndPoint endPoint)
	{
		return new PlayerSession(this, Network, sessionId, endPoint);
	}
}
