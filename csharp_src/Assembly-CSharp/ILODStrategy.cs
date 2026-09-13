using System.Collections.Generic;

public interface ILODStrategy
{
	int CalculateLODLevel(int currentLevel, List<ISceneLODNode> nodes);
}
