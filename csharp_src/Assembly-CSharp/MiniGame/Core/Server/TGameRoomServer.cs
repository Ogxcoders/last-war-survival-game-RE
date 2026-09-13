using System;
using System.Reflection;
using Joker;

namespace MiniGame.Core.Server;

public class TGameRoomServer<T> : GameRoomServer where T : GameRoom
{
	protected readonly ConstructorInfo _constructor;

	public TGameRoomServer()
	{
		_constructor = typeof(T).GetConstructor(new Type[3]
		{
			typeof(World),
			typeof(NetworkSession),
			typeof(C2SGameRoomCreate)
		});
	}

	public override GameRoom CreateRoom(C2SGameRoomCreate msg, NetworkSession session)
	{
		return (GameRoom)_constructor.Invoke(new object[3] { this, session, msg });
	}
}
