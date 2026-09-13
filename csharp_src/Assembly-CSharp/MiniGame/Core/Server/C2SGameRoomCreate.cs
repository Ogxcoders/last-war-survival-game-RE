namespace MiniGame.Core.Server;

public class C2SGameRoomCreate : RoomMessage
{
	public int LifeSeconds;

	public string LogicType;

	public string ResourceVersion;

	public string LogicVersion;

	public int MaxPlayers;

	public string LevelPath;
}
