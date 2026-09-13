using MiniGame.Core;

namespace MiniGame.Test.Client;

public static class GameTestClient
{
	public static void InitSystems(GameWorld world)
	{
		GameTestShare.InitComponents(world);
		world.PrepareSystems.Add(new SystemPrepare());
		world.ViewSystems.Add(new SystemView());
		GameTestShare.InitSystems(world);
	}
}
