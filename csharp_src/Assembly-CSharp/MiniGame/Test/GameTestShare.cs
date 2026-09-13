using MiniGame.Core;

namespace MiniGame.Test;

public static class GameTestShare
{
	public static void InitComponents(GameWorld world)
	{
	}

	public static void InitSystems(GameWorld worlds)
	{
		worlds.LogicSystems.Add(new SystemLogic());
	}
}
