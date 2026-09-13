using System.Collections.Generic;

public class PVESceneLODStrategy : ILODStrategy
{
	public int CalculateLODLevel(int currentLevel, List<ISceneLODNode> nodes)
	{
		if (!SceneManager.IsInPVE())
		{
			return 0;
		}
		int num = GameEntry.Setting.GetInt("GAME_QUALITY_CONFIG_KEY", -1);
		if (num == -1)
		{
			return 0;
		}
		int num2 = 0;
		if (num >= 6)
		{
			return 0;
		}
		if (num <= 3)
		{
			return 2;
		}
		return 1;
	}
}
