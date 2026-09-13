namespace MiniGame.Core.Server;

public interface IMessageEnter
{
	string LevelPath { get; set; }

	int MaxPlayers { get; set; }
}
