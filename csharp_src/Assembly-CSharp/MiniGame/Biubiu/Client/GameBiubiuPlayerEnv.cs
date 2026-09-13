using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class GameBiubiuPlayerEnv : DataResourceLoaderEnv
{
	public GameBiubiuPlayerEnv(GameBiuBiuPlayerBase player, Transform gameRoot)
		: base(gameRoot)
	{
		Player = player;
	}
}
