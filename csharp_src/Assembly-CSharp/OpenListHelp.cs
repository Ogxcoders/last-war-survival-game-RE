using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class OpenListHelp
{
	public class WorldTileCompare : IComparer
	{
		public int Compare(object a, object b)
		{
			WorldTileData obj = (WorldTileData)a;
			WorldTileData worldTileData = (WorldTileData)b;
			return obj.Fcost.CompareTo(worldTileData.Fcost);
		}
	}

	private WorldTileCompare worldTileCompare = new WorldTileCompare();

	private HashSet<Vector2Int> hasAddPos;

	private ArrayList arrayList;

	public int Count => arrayList.Count;

	public OpenListHelp(int numberOfElements)
	{
		hasAddPos = new HashSet<Vector2Int>();
		arrayList = new ArrayList(numberOfElements);
	}

	public void Clear()
	{
		arrayList.Clear();
		hasAddPos.Clear();
	}

	public bool Add(WorldTileData node)
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		if (hasAddPos.Contains(node.Pos))
		{
			return false;
		}
		hasAddPos.Add(node.Pos);
		arrayList.Add(node);
		return true;
	}

	public void Rebuild()
	{
	}

	public WorldTileData Remove()
	{
		arrayList.Sort(worldTileCompare);
		WorldTileData worldTileData = arrayList[0] as WorldTileData;
		hasAddPos.Remove(worldTileData.Pos);
		arrayList.RemoveAt(0);
		return worldTileData;
	}
}
