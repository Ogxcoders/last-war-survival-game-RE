using MiniGame.Core;

namespace MiniGame.Biubiu.Client;

public class GameBiuBiuEditorRunTime : GameBiuBiuRunTime
{
	public bool Config { get; set; }

	public override IGameSharedEnv GetSharedEnv()
	{
		GameBiubiuEnvClient gameBiubiuEnvClient = GameBiubiuClient.InitSharedEnvEditable(new GameLoader(base.LoadEnv, base.BindUI, base.Serializer));
		gameBiubiuEnvClient.SharedSnapshotType = (Config ? SharedSnapshotType.Config : gameBiubiuEnvClient.SharedSnapshotType);
		return gameBiubiuEnvClient;
	}

	public override void InitRunTime(GameWorld gameWorld, GameLevel gameLevel, bool verify = true)
	{
		GameBiubiuClient.InitClientEditable(gameWorld, gameLevel);
	}
}
