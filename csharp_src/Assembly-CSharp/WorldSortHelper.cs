using System.Collections.Generic;
using UnityEngine;

public class WorldSortHelper
{
	private int _count;

	private SortHelpHeap _helper;

	public WorldSortHelper(int count)
	{
		_count = count;
		_helper = new SortHelpHeap(count);
	}

	public void GetSortList(Vector2Int cameraPoint, Dictionary<int, PointInfo> sortList, List<PointInfo> outList)
	{
		if (sortList.Count < _count)
		{
			foreach (KeyValuePair<int, PointInfo> sort in sortList)
			{
				outList.Add(sort.Value);
			}
			sortList.Clear();
		}
		else
		{
			_helper.GetSortList(cameraPoint, sortList, outList);
		}
	}

	public bool IsOutOfViewRange(int i, Vector2Int cameraPoint)
	{
		Vector2Int vector2Int = SceneManager.World.IndexToTilePos(i);
		if (Mathf.Max(SortHelpHeap.fastAbs(vector2Int.x - cameraPoint.x), SortHelpHeap.fastAbs(vector2Int.y - cameraPoint.y)) >= 25)
		{
			return true;
		}
		return false;
	}
}
