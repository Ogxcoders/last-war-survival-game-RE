using System.Collections.Generic;

namespace Joker.Server;

public class GameRemoteActor : IGameActor
{
	private GameServer _server;

	private readonly int _to;

	private NetworkSystem _network;

	private bool _needTickMessage;

	private readonly Queue<(IRouteActorMessage route, IMessage Message, object Source)> _messageQueue = new Queue<(IRouteActorMessage, IMessage, object)>();

	public int ClientWorld { get; }

	public string AccountId { get; }

	public string ActorId { get; }

	public int MessageCount => _messageQueue.Count;

	public GameRemoteActor(GameServer server, int clientWorld, string accountId, string actorId)
	{
		ClientWorld = clientWorld;
		AccountId = accountId;
		ActorId = actorId;
		_server = server;
		_to = Singleton<WorldService>.Instance.GetTargetId<GameServer>(accountId);
		_needTickMessage = false;
		_network = _server.GetSystem<NetworkSystem>();
	}

	public bool EnqueueMessage(IRouteActorMessage route, IMessage message, object source)
	{
		_messageQueue.Enqueue((route, message, source));
		return _messageQueue.Count == 1;
	}

	public bool ProcessMessages()
	{
		return true;
	}

	public void Send(IMessage message, object source)
	{
	}

	public bool TryProcessMessage(IMessage message, object source)
	{
		return false;
	}

	public void SendToClient(IMessage message)
	{
		_server.Router.SendToWorldWithActor(ClientWorld, message, AccountId, ActorId);
	}

	public void SendToServer(int to, IMessage message)
	{
		_server.Router.SendToWorldWithActor(to, message, AccountId, ActorId);
	}

	public void SendToWorld(int to, IMessage message)
	{
		S2SRouteServerWithActorMessage s2SRouteServerWithActorMessage = new S2SRouteServerWithActorMessage();
		s2SRouteServerWithActorMessage.AccountId = AccountId;
		s2SRouteServerWithActorMessage.TargetId = ActorId;
		s2SRouteServerWithActorMessage.Message = message;
		s2SRouteServerWithActorMessage.From = _server.Id;
		s2SRouteServerWithActorMessage.To = to;
		_server.Router.SendToWorld(to, s2SRouteServerWithActorMessage);
	}
}
