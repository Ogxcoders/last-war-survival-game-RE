namespace MiniGame.Core.Server;

public class C2SGameRoomEnter : RoomMessage
{
	public string LevelPath;

	public int MaxPlayers;

	public string LogicType;

	public string LogicVersion;

	public string ResourceVersion;

	public bool IsObserver;

	public string PlayerName;
}
