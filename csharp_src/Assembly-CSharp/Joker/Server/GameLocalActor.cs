using System;
using System.Collections.Generic;

namespace Joker.Server;

public class GameLocalActor : IGameActor
{
	private readonly GameServer _server;

	private bool _needTickMessage;

	private readonly Queue<(IRouteActorMessage route, IMessage Message, object Source)> _messageQueue = new Queue<(IRouteActorMessage, IMessage, object)>();

	public int ClientWorld { get; }

	public string AccountId { get; }

	public string ActorId { get; }

	public int MessageCount => _messageQueue.Count;

	public GameServer Server => _server;

	public GameLocalActor(GameServer server, int clientWorld, string accountId, string actorId)
	{
		ClientWorld = clientWorld;
		AccountId = accountId;
		ActorId = actorId;
		_server = server;
		_needTickMessage = false;
	}

	public bool EnqueueMessage(IRouteActorMessage route, IMessage message, object source)
	{
		_messageQueue.Enqueue((route, message, source));
		return _messageQueue.Count == 1;
	}

	public bool ProcessMessages()
	{
		while (_messageQueue.Count > 0)
		{
			var (route, message, source) = _messageQueue.Dequeue();
			if (!DoProcessMessage(route, message, source))
			{
				Log.Warning("GameLocalActor ProcessMessages " + message.GetType().Name + " failed");
			}
		}
		return true;
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

	public void Send(IMessage message, object source)
	{
	}

	public GameRemoteActor GetRemotePlayer(string accountId, string actorId)
	{
		return _server.GetRemoteActor(accountId, actorId);
	}

	public virtual bool DoProcessMessage(IRouteActorMessage route, IMessage message, object source)
	{
		throw new NotImplementedException("DoProcessMessage");
	}
}
