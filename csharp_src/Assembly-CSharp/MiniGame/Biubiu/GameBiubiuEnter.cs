using MiniGame.Core.Server;

namespace MiniGame.Biubiu;

public class GameBiubiuEnter : IMessageEnter
{
	public string LevelPath { get; set; }

	public int MaxPlayers { get; set; }
}
