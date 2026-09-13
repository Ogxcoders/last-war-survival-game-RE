using System;
using System.Reflection;

namespace Joker.Server;

public class TGameServer<TLocal, TRemote, TRoom> : GameServer where TLocal : GameLocalActor where TRemote : GameRemoteActor where TRoom : GameRoomOld
{
	protected readonly ConstructorInfo _factoryLocalActor;

	protected readonly ConstructorInfo _factoryRemoteActor;

	protected readonly ConstructorInfo _factoryRoom;

	public TGameServer()
	{
		_factoryLocalActor = typeof(TLocal).GetConstructor(new Type[4]
		{
			typeof(GameServer),
			typeof(int),
			typeof(string),
			typeof(string)
		});
		_factoryRemoteActor = typeof(TRemote).GetConstructor(new Type[4]
		{
			typeof(GameServer),
			typeof(int),
			typeof(string),
			typeof(string)
		});
		_factoryRoom = typeof(TRoom).GetConstructor(new Type[4]
		{
			typeof(GameServer),
			typeof(string),
			typeof(string),
			typeof(int)
		});
	}

	protected override GameLocalActor CreateLocalActor(int clientWorld, string accountId, string actorId)
	{
		return (GameLocalActor)_factoryLocalActor.Invoke(new object[4] { this, clientWorld, accountId, actorId });
	}

	protected override GameRemoteActor CreateRemoteActor(int clientWorld, string accountId, string actorId)
	{
		return (GameRemoteActor)_factoryRemoteActor.Invoke(new object[4] { this, clientWorld, accountId, actorId });
	}

	protected override GameRoomOld CreateRoom(string id, string level, int maxPlayers)
	{
		return (GameRoomOld)_factoryRoom.Invoke(new object[4] { this, id, level, maxPlayers });
	}
}
public class TGameServer<TLocal> : GameServer where TLocal : GameLocalActor
{
	protected readonly ConstructorInfo _factoryLocalActor;

	public TGameServer()
	{
		_factoryLocalActor = typeof(TLocal).GetConstructor(new Type[4]
		{
			typeof(GameServer),
			typeof(int),
			typeof(string),
			typeof(string)
		});
	}

	protected override GameLocalActor CreateLocalActor(int clientWorld, string accountId, string actorId)
	{
		return (GameLocalActor)_factoryLocalActor.Invoke(new object[4] { this, clientWorld, accountId, actorId });
	}

	protected override GameRemoteActor CreateRemoteActor(int clientWorld, string accountId, string actorId)
	{
		throw new NotImplementedException();
	}
}
public class TGameServer<TLocal, TRoom> : GameServer where TLocal : GameLocalActor where TRoom : GameRoomOld
{
	protected readonly ConstructorInfo _factoryLocalActor;

	protected readonly ConstructorInfo _factoryRoom;

	public TGameServer()
	{
		_factoryLocalActor = typeof(TLocal).GetConstructor(new Type[4]
		{
			typeof(GameServer),
			typeof(int),
			typeof(string),
			typeof(string)
		});
		_factoryRoom = typeof(TRoom).GetConstructor(new Type[4]
		{
			typeof(GameServer),
			typeof(string),
			typeof(string),
			typeof(int)
		});
	}

	protected override GameLocalActor CreateLocalActor(int clientWorld, string accountId, string actorId)
	{
		return (GameLocalActor)_factoryLocalActor.Invoke(new object[4] { this, clientWorld, accountId, actorId });
	}

	protected override GameRemoteActor CreateRemoteActor(int clientWorld, string accountId, string actorId)
	{
		throw new NotImplementedException();
	}

	protected override GameRoomOld CreateRoom(string id, string level, int maxPlayers)
	{
		return (GameRoomOld)_factoryRoom.Invoke(new object[4] { this, id, level, maxPlayers });
	}
}
public class TGameServer : GameServer
{
	protected readonly Func<GameServer, long, string, string, GameLocalActor> _factoryLocalActor;

	protected readonly Func<GameServer, long, string, string, GameRemoteActor> _factoryRemoteActor;

	protected readonly Func<GameServer, string, string, int, GameRoomOld> _factoryRoom;

	public TGameServer(Func<GameServer, long, string, string, GameLocalActor> local)
	{
		_factoryLocalActor = local;
	}

	public TGameServer(Func<GameServer, long, string, string, GameLocalActor> local, Func<GameServer, string, string, int, GameRoomOld> room)
	{
		_factoryLocalActor = local;
		_factoryRoom = room;
	}

	public TGameServer(Func<GameServer, long, string, string, GameLocalActor> local, Func<GameServer, long, string, string, GameRemoteActor> remote, Func<GameServer, string, string, int, GameRoomOld> room)
	{
		_factoryLocalActor = local;
		_factoryRemoteActor = remote;
		_factoryRoom = room;
	}

	protected override GameLocalActor CreateLocalActor(int clientWorld, string accountId, string actorId)
	{
		return _factoryLocalActor(this, clientWorld, accountId, actorId);
	}

	protected override GameRemoteActor CreateRemoteActor(int clientWorld, string accountId, string actorId)
	{
		return _factoryRemoteActor(this, clientWorld, accountId, actorId);
	}

	protected override GameRoomOld CreateRoom(string id, string level, int maxPlayers)
	{
		return _factoryRoom(this, id, level, maxPlayers);
	}
}
