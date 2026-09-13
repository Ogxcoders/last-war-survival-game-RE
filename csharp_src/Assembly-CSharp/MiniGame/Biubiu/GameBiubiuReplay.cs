using System.Collections.Generic;

namespace MiniGame.Biubiu;

public class GameBiubiuReplay
{
	public string LevelPath;

	public string MD5;

	public List<ISyncCommand> Commands = new List<ISyncCommand>();
}
