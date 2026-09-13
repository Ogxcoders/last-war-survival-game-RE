using System;
using System.Collections.Generic;

namespace Joker.Server;

public class GameRoomOld : IDisposable
{
	protected List<string> _actors = new List<string>();

	public GameServer Server { get; private set; }

	public string ID { get; private set; }

	public string Level { get; private set; }

	public int MaxPlayers { get; private set; }

	public List<string> Players => _actors;

	public GameRoomOld(GameServer server, string id, string level, int maxPlayer)
	{
		ID = id;
		Level = level;
		Server = server;
		MaxPlayers = maxPlayer;
	}

	public virtual IGameActor GetActor(string id)
	{
		return Server.GetLocalActor(id);
	}

	public virtual bool TryEnterActor(IGameActor actor)
	{
		if (_actors.Count >= MaxPlayers)
		{
			return false;
		}
		if (_actors.Contains(actor.AccountId))
		{
			return false;
		}
		_actors.Add(actor.AccountId);
		return true;
	}

	public virtual bool TryLeavelActor(IGameActor actor)
	{
		if (_actors.Count == 0)
		{
			return false;
		}
		if (!_actors.Contains(actor.AccountId))
		{
			return false;
		}
		_actors.Remove(actor.AccountId);
		return true;
	}

	public virtual void Tick(float deltaTime)
	{
	}

	public virtual void HandleMessage(IGameActor actor, IMessage message)
	{
	}

	public virtual bool IsValid()
	{
		return false;
	}

	public virtual void Dispose()
	{
	}
}
