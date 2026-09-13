using MiniGame.Core.Server;

namespace MiniGame.Biubiu;

public class GameBiubiuEnterResp : IMessageEnter
{
	public string LevelPath { get; set; }

	public int MaxPlayers { get; set; }
}
