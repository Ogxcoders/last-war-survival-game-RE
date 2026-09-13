using System.Collections.Generic;
using UnityEngine;

public class CityRoadPathParam
{
	public enum PathType
	{
		NORMAL,
		VIADUCT,
		MAIN_ROAD
	}

	public enum DirectType
	{
		HORIZONTAL,
		PORTRAIT,
		WEST_TO_EAST,
		EAST_TO_WEST,
		NORTH_TO_SOUTH,
		SOUTH_TO_NORTH
	}

	private int pathType;

	private int directType;

	public void addPathType(PathType pt)
	{
		pathType |= 1 << pt.ToInt();
	}

	public void addDirectType(DirectType dt)
	{
		directType |= 1 << dt.ToInt();
	}

	public bool alikePathType(PathType pt)
	{
		return (pathType & (1 << pt.ToInt())) != 0;
	}

	public bool alikeDirectType(DirectType dt)
	{
		return (directType & (1 << dt.ToInt())) != 0;
	}

	public void Reset()
	{
		pathType = 0;
		directType = 0;
	}

	public static bool checkRoadParam(WorldTileData src, WorldTileData neighbor, Vector2Int endPos)
	{
		CityRoadPathParam roadByPointId = GameEntry.Data.Road.GetRoadByPointId(SceneManager.World.TilePosToIndex(src.Pos));
		CityRoadPathParam roadByPointId2 = GameEntry.Data.Road.GetRoadByPointId(SceneManager.World.TilePosToIndex(neighbor.Pos));
		if (roadByPointId == null || roadByPointId2 == null)
		{
			return false;
		}
		if (roadByPointId.alikePathType(PathType.VIADUCT))
		{
			if (src.Parent == null)
			{
				return false;
			}
			if (roadByPointId.alikePathType(PathType.MAIN_ROAD))
			{
				Vector2Int vector2Int = src.Pos - src.Parent.Pos;
				Vector2Int other = neighbor.Pos - src.Pos;
				return vector2Int.Equals(other);
			}
			if (roadByPointId.alikeDirectType(DirectType.HORIZONTAL))
			{
				return (neighbor.Pos - src.Pos).y == 0;
			}
			if (roadByPointId.alikeDirectType(DirectType.PORTRAIT))
			{
				return (neighbor.Pos - src.Pos).x == 0;
			}
		}
		if (roadByPointId.alikePathType(PathType.NORMAL))
		{
			return true;
		}
		if (roadByPointId.alikePathType(PathType.MAIN_ROAD))
		{
			Vector2Int vector2Int2 = Vector2Int.zero;
			Vector2 vector = Vector2.zero;
			if (roadByPointId.alikeDirectType(DirectType.WEST_TO_EAST))
			{
				vector2Int2 = Vector2Int.right;
				vector = Vector2.up;
			}
			if (roadByPointId.alikeDirectType(DirectType.EAST_TO_WEST))
			{
				vector2Int2 = Vector2Int.left;
				vector = Vector2.down;
			}
			if (roadByPointId.alikeDirectType(DirectType.NORTH_TO_SOUTH))
			{
				vector2Int2 = Vector2Int.down;
				vector = Vector2.right;
			}
			if (roadByPointId.alikeDirectType(DirectType.SOUTH_TO_NORTH))
			{
				vector2Int2 = Vector2Int.up;
				vector = Vector2.left;
			}
			Vector2Int vector2Int3 = neighbor.Pos - src.Pos;
			if (vector2Int2.Equals(vector2Int3))
			{
				return true;
			}
			if (roadByPointId2 != null && roadByPointId2.alikePathType(PathType.NORMAL))
			{
				return vector.Equals(vector2Int3);
			}
			return false;
		}
		return false;
	}

	public static PathType genPathType(Vector2Int startPos, Vector2Int endPos)
	{
		CityRoadPathParam roadByPointId = GameEntry.Data.Road.GetRoadByPointId(SceneManager.World.TilePosToIndex(startPos));
		CityRoadPathParam roadByPointId2 = GameEntry.Data.Road.GetRoadByPointId(SceneManager.World.TilePosToIndex(endPos));
		Vector2Int vector2Int = endPos - startPos;
		if (roadByPointId2 == null)
		{
			if (roadByPointId == null)
			{
				return PathType.NORMAL;
			}
			if (roadByPointId.alikePathType(PathType.MAIN_ROAD))
			{
				return PathType.MAIN_ROAD;
			}
			if (roadByPointId.alikePathType(PathType.VIADUCT))
			{
				return PathType.VIADUCT;
			}
			return PathType.NORMAL;
		}
		if (roadByPointId2.alikePathType(PathType.VIADUCT))
		{
			if (roadByPointId2.alikePathType(PathType.MAIN_ROAD))
			{
				if (roadByPointId == null)
				{
					return PathType.MAIN_ROAD;
				}
				if (roadByPointId.alikePathType(PathType.VIADUCT))
				{
					if (roadByPointId.alikeDirectType(DirectType.HORIZONTAL) && vector2Int.x != 0)
					{
						return PathType.VIADUCT;
					}
					if (roadByPointId.alikeDirectType(DirectType.PORTRAIT) && vector2Int.y != 0)
					{
						return PathType.VIADUCT;
					}
				}
				return PathType.MAIN_ROAD;
			}
			return PathType.VIADUCT;
		}
		if (roadByPointId2.alikePathType(PathType.MAIN_ROAD))
		{
			return PathType.MAIN_ROAD;
		}
		return PathType.NORMAL;
	}

	public static List<Vector2Int> getNeighborFromParam(Vector2Int lastPos, Vector2Int nowPos, Dictionary<int, CityRoadPathParam> allRoads)
	{
		int key = SceneManager.World.TilePosToIndex(nowPos);
		List<Vector2Int> list = new List<Vector2Int>();
		if (allRoads.TryGetValue(key, out var value))
		{
			if (value.alikePathType(PathType.VIADUCT))
			{
				if (value.alikePathType(PathType.MAIN_ROAD))
				{
					if (lastPos.Equals(Vector2Int.zero))
					{
						return list;
					}
					Vector2Int vector2Int = nowPos - lastPos;
					list.Add(nowPos + vector2Int);
					return list;
				}
				if (value.alikeDirectType(DirectType.HORIZONTAL))
				{
					list.Add(nowPos + Vector2Int.left);
					list.Add(nowPos + Vector2Int.right);
				}
				else if (value.alikeDirectType(DirectType.PORTRAIT))
				{
					list.Add(nowPos + Vector2Int.up);
					list.Add(nowPos + Vector2Int.down);
				}
			}
			else if (value.alikePathType(PathType.MAIN_ROAD))
			{
				if (value.alikeDirectType(DirectType.WEST_TO_EAST))
				{
					list.Add(nowPos + Vector2Int.right);
					list.Add(nowPos + Vector2Int.up);
				}
				else if (value.alikeDirectType(DirectType.EAST_TO_WEST))
				{
					list.Add(nowPos + Vector2Int.left);
					list.Add(nowPos + Vector2Int.down);
				}
				else if (value.alikeDirectType(DirectType.SOUTH_TO_NORTH))
				{
					list.Add(nowPos + Vector2Int.up);
					list.Add(nowPos + Vector2Int.left);
				}
				else if (value.alikeDirectType(DirectType.NORTH_TO_SOUTH))
				{
					list.Add(nowPos + Vector2Int.down);
					list.Add(nowPos + Vector2Int.right);
				}
			}
			else
			{
				list.Add(nowPos + Vector2Int.right);
				list.Add(nowPos + Vector2Int.left);
				list.Add(nowPos + Vector2Int.up);
				list.Add(nowPos + Vector2Int.down);
			}
		}
		return list;
	}
}
