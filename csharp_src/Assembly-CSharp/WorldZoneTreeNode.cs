using System.Collections.Generic;
using UnityEngine;

public class WorldZoneTreeNode
{
	public RectInt rect;

	public List<WorldZoneTreeNode> nodes = new List<WorldZoneTreeNode>();

	public List<WorldZone> zones = new List<WorldZone>();

	public WorldZoneTreeNode(RectInt rc)
	{
		rect = rc;
	}

	public void CrateSubNodes(int depth)
	{
		int num = Mathf.CeilToInt((float)rect.width * 0.5f);
		int num2 = Mathf.CeilToInt((float)rect.height * 0.5f);
		nodes.Clear();
		nodes.Add(new WorldZoneTreeNode(new RectInt(rect.x, rect.y, num, num2)));
		nodes.Add(new WorldZoneTreeNode(new RectInt(rect.x + num, rect.y, num, num2)));
		nodes.Add(new WorldZoneTreeNode(new RectInt(rect.x, rect.y + num2, num, num2)));
		nodes.Add(new WorldZoneTreeNode(new RectInt(rect.x + num, rect.y + num2, num, num2)));
		if (depth <= 0)
		{
			return;
		}
		foreach (WorldZoneTreeNode node in nodes)
		{
			node.CrateSubNodes(depth - 1);
		}
	}

	private bool IsRectIntersect(RectInt rc1, RectInt rc2)
	{
		if (Mathf.Max(rc1.xMin, rc2.xMin) > Mathf.Min(rc1.xMax, rc2.xMax) || Mathf.Max(rc1.yMin, rc2.yMin) > Mathf.Min(rc1.yMax, rc2.yMax))
		{
			return false;
		}
		return true;
	}

	public void AddZone(WorldZone zone)
	{
		if (nodes.Count > 0)
		{
			for (int i = 0; i < nodes.Count; i++)
			{
				if (IsRectIntersect(nodes[i].rect, zone.rect))
				{
					nodes[i].AddZone(zone);
				}
			}
		}
		else
		{
			zones.Add(zone);
		}
	}

	public void FindZone(RectInt rect, ref List<WorldZone> findZones, ref int count)
	{
		if (nodes.Count > 0)
		{
			for (int i = 0; i < nodes.Count; i++)
			{
				count++;
				if (IsRectIntersect(nodes[i].rect, rect))
				{
					nodes[i].FindZone(rect, ref findZones, ref count);
				}
			}
		}
		else
		{
			if (zones.Count <= 0)
			{
				return;
			}
			for (int j = 0; j < zones.Count; j++)
			{
				if (!zones[j].finded)
				{
					count++;
					if (IsRectIntersect(zones[j].rect, rect))
					{
						zones[j].finded = true;
						zones[j].findState++;
						findZones.Add(zones[j]);
					}
				}
			}
		}
	}
}
