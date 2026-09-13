using MiniGame.Core.Server;

namespace MiniGame.Biubiu;

public class GameBiubiuEndResp : IMessageEnd
{
	public int Code;

	public SharedGameStatistics Statistics;
}
