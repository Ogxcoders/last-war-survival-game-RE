using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class PathManager
{
	public enum EstimateType
	{
		Manhattan,
		Euclidian,
		Diagonal
	}

	private EstimateType m_EstimateType;

	private Predicate<WorldTileData> m_NeighborFilter;

	private OpenListHelp m_OpenNodes = new OpenListHelp(1000);

	private Dictionary<FindPathType, Dictionary<Vector2Int, WorldTileData>> worldTileDataDicCache = new Dictionary<FindPathType, Dictionary<Vector2Int, WorldTileData>>();

	private Dictionary<FindPathType, Vector2Int> cityMainPointCache = new Dictionary<FindPathType, Vector2Int>();

	private Dictionary<FindPathType, int> cityLevelCache = new Dictionary<FindPathType, int>();

	private Dictionary<FindPathType, HashSet<Vector2Int>> roadCache = new Dictionary<FindPathType, HashSet<Vector2Int>>();

	private WorldTileData[] _neighbors = new WorldTileData[9];

	private List<Vector2Int> _paths = new List<Vector2Int>();

	private int m_PathID;

	private int m_NextPathID;

	private int GetNextPathID()
	{
		return ++m_NextPathID;
	}

	public void ExpireRoadCache()
	{
		roadCache = new Dictionary<FindPathType, HashSet<Vector2Int>>();
	}

	public bool IsTruckRoad(Vector2Int point, FindPathType findPathType)
	{
		roadCache.TryGetValue(findPathType, out var value);
		if (value == null)
		{
			LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetAllRoadData");
			value = new HashSet<Vector2Int>();
			if (luaTable != null)
			{
				for (int i = 1; i <= luaTable.Length; i++)
				{
					LuaTable luaTable2 = (LuaTable)luaTable[i];
					if (luaTable2 != null && luaTable2.ContainsKey("pointId"))
					{
						int index = luaTable2.Get<int>("pointId");
						value.Add(SceneManager.World.IndexToTilePos(index));
					}
				}
			}
			roadCache[findPathType] = value;
		}
		return value.Contains(point);
	}

	private void CheckWorldTileData(FindPathType findPathType)
	{
		LuaBuildData buildingDataByBuildId = GameEntry.Data.Building.GetBuildingDataByBuildId(10100000);
		if (buildingDataByBuildId == null)
		{
			return;
		}
		int level = buildingDataByBuildId.level;
		Vector2Int vector2Int = SceneManager.World.IndexToTilePos(buildingDataByBuildId.pointId) + new Vector2Int(-1, -1);
		int num = (cityLevelCache.ContainsKey(findPathType) ? cityLevelCache[findPathType] : 0);
		Vector2Int vector2Int2 = (cityMainPointCache.ContainsKey(findPathType) ? cityMainPointCache[findPathType] : Vector2Int.zero);
		Dictionary<Vector2Int, WorldTileData> dictionary = (worldTileDataDicCache.ContainsKey(findPathType) ? worldTileDataDicCache[findPathType] : new Dictionary<Vector2Int, WorldTileData>());
		if (dictionary.Count != 0 && level == num && !(vector2Int != vector2Int2))
		{
			return;
		}
		dictionary.Clear();
		int num2 = 10100000;
		int buildId = num2 + buildingDataByBuildId.level;
		SceneManager.World.GetBuildOffsetRangeByBuildId(buildId);
		SceneManager.World.GetBuildTileByItemId(num2);
		int num3 = GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetBuildMaxRadius");
		Vector2Int vector2Int3 = vector2Int;
		for (int i = -num3; i <= num3; i++)
		{
			for (int j = -num3; j <= num3; j++)
			{
				if (i != 0 || j != 0)
				{
					Vector2Int vector2Int4 = new Vector2Int(vector2Int3.x + i, vector2Int3.y + j);
					WorldTileData tileData = GetTileData(vector2Int4, findPathType);
					if (tileData != null)
					{
						tileData.reset();
						continue;
					}
					WorldTileData worldTileData = new WorldTileData();
					worldTileData.Pos = vector2Int4;
					dictionary[vector2Int4] = worldTileData;
				}
			}
		}
		cityLevelCache[findPathType] = level;
		cityMainPointCache[findPathType] = vector2Int;
		worldTileDataDicCache[findPathType] = dictionary;
	}

	private WorldTileData GetTileData(Vector2Int point, FindPathType findPathType)
	{
		worldTileDataDicCache.TryGetValue(findPathType, out var value);
		if (value == null)
		{
			return null;
		}
		if (value.TryGetValue(point, out var value2))
		{
			return value2;
		}
		return null;
	}

	public List<Vector2Int> FindingPath(Vector2Int start, Vector2Int end, FindPathType findPathType = FindPathType.WorldDomeTruck, Predicate<WorldTileData> neighborFilter = null, EstimateType estimateType = EstimateType.Manhattan)
	{
		CheckWorldTileData(findPathType);
		WorldTileData tileData = GetTileData(start, findPathType);
		WorldTileData tileData2 = GetTileData(end, findPathType);
		if (tileData == null || tileData2 == null)
		{
			return new List<Vector2Int>();
		}
		if (tileData.Pos == tileData2.Pos)
		{
			return GenPath(tileData, tileData2);
		}
		tileData.Parent = null;
		tileData2.Parent = null;
		m_NeighborFilter = neighborFilter;
		m_EstimateType = estimateType;
		m_PathID = GetNextPathID();
		WorldTileData[] neighbors = _neighbors;
		bool flag = false;
		tileData.Gcost = 0f;
		tileData.Ecost = 0f;
		tileData.PathID = m_PathID;
		m_OpenNodes.Add(tileData);
		bool flag2 = false;
		while (m_OpenNodes.Count > 0)
		{
			if (flag2)
			{
				m_OpenNodes.Rebuild();
				flag2 = false;
			}
			WorldTileData worldTileData = m_OpenNodes.Remove();
			worldTileData.HasClosed = true;
			if (worldTileData.Pos == tileData2.Pos)
			{
				flag = true;
				break;
			}
			int neighborsNew = GetNeighborsNew(worldTileData, neighbors, end, findPathType);
			bool flag3 = worldTileData.Parent != null;
			for (int i = 0; i < neighborsNew; i++)
			{
				WorldTileData worldTileData2 = neighbors[i];
				float num = 1f;
				if (flag3 && isLine(worldTileData, worldTileData.Parent, worldTileData2))
				{
					num -= 0.4f;
				}
				float num2 = GetDistance(worldTileData2, tileData2) + num;
				if (worldTileData2.PathID != m_PathID)
				{
					worldTileData2.Ecost = GetDistance(worldTileData2, tileData2);
					worldTileData2.PathID = m_PathID;
					worldTileData2.Gcost = 0f;
					worldTileData2.HasClosed = false;
				}
				if (worldTileData2.HasClosed)
				{
					continue;
				}
				float num3 = worldTileData.Gcost + num2;
				if (worldTileData2.Gcost == 0f || worldTileData2.Gcost > num3)
				{
					worldTileData2.Gcost = num3;
					worldTileData2.Parent = worldTileData;
					if (!m_OpenNodes.Add(worldTileData2))
					{
						flag2 = true;
					}
				}
			}
		}
		m_OpenNodes.Clear();
		if (flag)
		{
			return GenPath(tileData, tileData2);
		}
		_paths.Clear();
		return _paths;
	}

	private bool isLine(WorldTileData cur, WorldTileData pre, WorldTileData test)
	{
		int num = cur.Pos.x - pre.Pos.x;
		int num2 = cur.Pos.y - pre.Pos.y;
		int num3 = test.Pos.x - cur.Pos.x;
		int num4 = test.Pos.y - cur.Pos.y;
		if ((num == num3 && num == 0) || (num2 == num4 && num2 == 0))
		{
			return true;
		}
		return false;
	}

	private List<Vector2Int> GenPath(WorldTileData start, WorldTileData end)
	{
		List<Vector2Int> list = new List<Vector2Int>();
		if (end != null)
		{
			WorldTileData worldTileData = end;
			while (true)
			{
				list.Add(worldTileData.Pos);
				if (worldTileData == start || worldTileData.Parent == null)
				{
					break;
				}
				worldTileData = worldTileData.Parent;
			}
			list.Reverse();
		}
		return list;
	}

	private float GetDistance(WorldTileData a, WorldTileData b)
	{
		int num = Mathf.Abs(a.Pos.x - b.Pos.x);
		int num2 = Mathf.Abs(a.Pos.y - b.Pos.y);
		switch (m_EstimateType)
		{
		case EstimateType.Manhattan:
			return num + num2;
		case EstimateType.Euclidian:
			return num + num2;
		case EstimateType.Diagonal:
		{
			int num3 = Mathf.Min(num, num2);
			int num4 = num + num2;
			return 1.4f * (float)num3 + (float)(num4 - num3);
		}
		default:
			return 0f;
		}
	}

	private int GetNeighborsNew(WorldTileData cur, WorldTileData[] neighbors, Vector2Int endPos, FindPathType findPathType = FindPathType.WorldDomeTruck)
	{
		int num = 0;
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if ((i != 0 || j != 0) && (m_EstimateType != EstimateType.Manhattan || Mathf.Abs(i) != 1 || Mathf.Abs(j) != 1))
				{
					Vector2Int point = new Vector2Int(cur.Pos.x + i, cur.Pos.y + j);
					WorldTileData tileData = GetTileData(point, findPathType);
					if (tileData != null && (m_NeighborFilter == null || m_NeighborFilter(tileData)) && (findPathType != FindPathType.WorldDomeTruck || CityRoadPathParam.checkRoadParam(cur, tileData, endPos)))
					{
						neighbors[num] = tileData;
						num++;
					}
				}
			}
		}
		return num;
	}
}
