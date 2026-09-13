using Joker;

namespace MiniGame.Core.Server;

public class GameRoomLocal : GameRoom
{
	public GameRoomLocal(World server, NetworkSession roomOwner, C2SGameRoomCreate msg)
		: base(server, roomOwner, msg)
	{
	}

	public override void NotifySettlement()
	{
	}
}
