using MiniGame.Core.Server;

namespace MiniGame.Biubiu;

public class GameBiubiuStartResp : IMessageStart
{
	public string[] PlayerSessionIDs { get; set; }
}
