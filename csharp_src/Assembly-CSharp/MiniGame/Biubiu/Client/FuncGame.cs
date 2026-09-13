using Leopotam.EcsLite;

namespace MiniGame.Biubiu.Client;

public static class FuncGame
{
	public static GameBiuBiuPlayerBase GetPlayer(EcsWorld world)
	{
		return GetPlayer(world.GetShared<SharedRuntime>());
	}

	public static GameBiuBiuPlayerBase GetPlayer(SharedRuntime shared)
	{
		if (shared.ResourceLoader != null && shared.ResourceLoader is GameLoader { LoaderEnv: not null } gameLoader)
		{
			return gameLoader.LoaderEnv.Player;
		}
		return null;
	}
}
