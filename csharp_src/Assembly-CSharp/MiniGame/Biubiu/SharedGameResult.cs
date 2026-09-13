using System.Collections.Generic;

namespace MiniGame.Biubiu;

public class SharedGameResult
{
	public string VerifyJson;

	public List<ISyncCommand> Commands;

	public GameBiubiuReplay Replay;

	public SharedGameStatistics Statistics;

	public int Result;
}
