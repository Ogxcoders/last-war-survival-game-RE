using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class WorldPathfinding
{
	public static string PathToString(List<Vector2Int> path)
	{
		return string.Join(";", path.Select((Vector2Int p) => SceneManager.World.TilePosToIndex(p)));
	}
}
