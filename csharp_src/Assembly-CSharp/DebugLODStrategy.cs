using System.Collections.Generic;

public class DebugLODStrategy : ILODStrategy
{
	public int lod { get; set; }

	public int CalculateLODLevel(int currentLevel, List<ISceneLODNode> nodes)
	{
		return lod;
	}
}
