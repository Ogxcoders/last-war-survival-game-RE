using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XLua;

public class PathUtils
{
	private static readonly Vector2Int[] MainRoundRoadOffset = new Vector2Int[16]
	{
		new Vector2Int(2, 2),
		new Vector2Int(2, 1),
		new Vector2Int(2, 0),
		new Vector2Int(2, -1),
		new Vector2Int(2, -2),
		new Vector2Int(1, -2),
		new Vector2Int(0, -2),
		new Vector2Int(-1, -2),
		new Vector2Int(-2, -2),
		new Vector2Int(-2, -1),
		new Vector2Int(-2, 0),
		new Vector2Int(-2, 1),
		new Vector2Int(-2, 2),
		new Vector2Int(-1, 2),
		new Vector2Int(0, 2),
		new Vector2Int(1, 2)
	};

	private static readonly Dictionary<Vector2Int, Vector2Int> MainRoundEntryOffset = new Dictionary<Vector2Int, Vector2Int>
	{
		{
			new Vector2Int(3, 2),
			new Vector2Int(2, 2)
		},
		{
			new Vector2Int(3, 1),
			new Vector2Int(2, 1)
		},
		{
			new Vector2Int(3, 0),
			new Vector2Int(2, 0)
		},
		{
			new Vector2Int(3, -1),
			new Vector2Int(2, -1)
		},
		{
			new Vector2Int(3, -2),
			new Vector2Int(2, -2)
		},
		{
			new Vector2Int(2, -3),
			new Vector2Int(2, -2)
		},
		{
			new Vector2Int(1, -3),
			new Vector2Int(1, -2)
		},
		{
			new Vector2Int(0, -3),
			new Vector2Int(0, -2)
		},
		{
			new Vector2Int(-1, -3),
			new Vector2Int(-1, -2)
		},
		{
			new Vector2Int(-2, -3),
			new Vector2Int(-2, -2)
		},
		{
			new Vector2Int(-3, -2),
			new Vector2Int(-2, -2)
		},
		{
			new Vector2Int(-3, -1),
			new Vector2Int(-2, -1)
		},
		{
			new Vector2Int(-3, 0),
			new Vector2Int(-2, 0)
		},
		{
			new Vector2Int(-3, 1),
			new Vector2Int(-2, 1)
		},
		{
			new Vector2Int(-3, 2),
			new Vector2Int(-2, 2)
		},
		{
			new Vector2Int(-2, 3),
			new Vector2Int(-2, 2)
		},
		{
			new Vector2Int(-1, 3),
			new Vector2Int(-1, 2)
		},
		{
			new Vector2Int(0, 3),
			new Vector2Int(0, 2)
		},
		{
			new Vector2Int(1, 3),
			new Vector2Int(1, 2)
		},
		{
			new Vector2Int(2, 3),
			new Vector2Int(2, 2)
		}
	};

	private static string PathToString(List<Vector2Int> path)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < path.Count; i++)
		{
			if (i != 0)
			{
				stringBuilder.Append(";");
			}
			stringBuilder.Append(PointToString(path[i]));
		}
		return stringBuilder.ToString() + ";";
	}

	private static string PointToString(Vector2Int pos)
	{
		return $"{pos.x},{pos.y}";
	}

	private static IEnumerator GetTruckPathStringEnumerator(long targetBuildingUuid, Dictionary<ResourceType, long> resDict, Action<string> onComplete)
	{
		List<LuaBuildData> list = new List<LuaBuildData>();
		if (resDict != null)
		{
			if (resDict.ContainsKey(ResourceType.Oil) && resDict[ResourceType.Oil] > 0)
			{
				LuaBuildData buildingDataByBuildId = GameEntry.Data.Building.GetBuildingDataByBuildId(439000);
				if (buildingDataByBuildId != null && buildingDataByBuildId.IsActive())
				{
					list.Add(buildingDataByBuildId);
				}
			}
			if (resDict.ContainsKey(ResourceType.Metal) && resDict[ResourceType.Metal] > 0)
			{
				LuaBuildData buildingDataByBuildId2 = GameEntry.Data.Building.GetBuildingDataByBuildId(441000);
				if (buildingDataByBuildId2 != null && buildingDataByBuildId2.IsActive())
				{
					list.Add(buildingDataByBuildId2);
				}
			}
			if (resDict.ContainsKey(ResourceType.Water) && resDict[ResourceType.Water] > 0)
			{
				LuaBuildData buildingDataByBuildId3 = GameEntry.Data.Building.GetBuildingDataByBuildId(438000);
				if (buildingDataByBuildId3 != null && buildingDataByBuildId3.IsActive())
				{
					list.Add(buildingDataByBuildId3);
				}
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		LuaBuildData buildingDataByBuildId4 = GameEntry.Data.Building.GetBuildingDataByBuildId(10100000);
		Vector2Int pos = SceneManager.World.IndexToTilePos(buildingDataByBuildId4.pointId);
		Vector2Int[] buildingNeighbors = GetBuildingNeighbors(10100000, buildingDataByBuildId4.pointId);
		bool flag = true;
		_ = Vector2Int.zero;
		List<Vector2Int> list2;
		foreach (LuaBuildData item in list)
		{
			Vector2Int targetPos = SceneManager.World.IndexToTilePos(item.pointId);
			Vector2Int[] buildingNeighbors2 = GetBuildingNeighbors(item.uuid);
			if (flag)
			{
				list2 = null;
				Array.Sort(buildingNeighbors, (Vector2Int a, Vector2Int b) => Vector2Int.Distance(a, targetPos).CompareTo(Vector2Int.Distance(b, targetPos)));
				Vector2Int[] array = buildingNeighbors;
				for (int num = 0; num < array.Length; num++)
				{
					if (IsTruckRoad(array[num]))
					{
						Vector2Int[] array2 = buildingNeighbors2;
						for (int num2 = 0; num2 < array2.Length; num2++)
						{
							IsTruckRoad(array2[num2]);
						}
						if (list2 != null && list2.Count > 1)
						{
							break;
						}
					}
				}
				if (list2 != null && list2.Count > 1)
				{
					flag = false;
					_ = list2[list2.Count - 1];
					stringBuilder.Append("garage@");
					stringBuilder.Append(buildingDataByBuildId4.uuid);
					stringBuilder.Append("@");
					stringBuilder.Append(PointToString(pos));
					stringBuilder.Append(";");
					stringBuilder.Append(PointToString(list2[0]));
					stringBuilder.Append($"|{item.buildId}@");
					stringBuilder.Append(item.uuid);
					stringBuilder.Append("@");
					stringBuilder.Append(PathToString(list2));
				}
			}
			else
			{
				list2 = null;
				Vector2Int[] array = buildingNeighbors2;
				for (int num = 0; num < array.Length; num++)
				{
					IsTruckRoad(array[num]);
				}
				if (list2 != null && list2.Count > 1)
				{
					_ = list2[list2.Count - 1];
					stringBuilder.Append($"|{item.buildId}@");
					stringBuilder.Append(item.uuid);
					stringBuilder.Append("@");
					stringBuilder.Append(PathToString(list2));
				}
			}
		}
		LuaBuildData buildingDataByUuid = GameEntry.Data.Building.GetBuildingDataByUuid(targetBuildingUuid);
		Vector2Int targetPos2 = SceneManager.World.IndexToTilePos(buildingDataByUuid.pointId);
		Vector2Int[] buildingNeighbors3 = GetBuildingNeighbors(buildingDataByUuid.uuid);
		list2 = null;
		if (flag)
		{
			Array.Sort(buildingNeighbors, (Vector2Int a, Vector2Int b) => Vector2Int.Distance(a, targetPos2).CompareTo(Vector2Int.Distance(b, targetPos2)));
			Vector2Int[] array = buildingNeighbors;
			for (int num = 0; num < array.Length; num++)
			{
				if (IsTruckRoad(array[num]))
				{
					Vector2Int[] array2 = buildingNeighbors3;
					for (int num2 = 0; num2 < array2.Length; num2++)
					{
						IsTruckRoad(array2[num2]);
					}
					if (list2 != null && list2.Count > 1)
					{
						break;
					}
				}
			}
		}
		else
		{
			Vector2Int[] array = buildingNeighbors3;
			for (int num = 0; num < array.Length; num++)
			{
				IsTruckRoad(array[num]);
			}
		}
		if (list2 != null && list2.Count > 1)
		{
			if (flag)
			{
				stringBuilder.Append("garage@");
				stringBuilder.Append(buildingDataByBuildId4.uuid);
				stringBuilder.Append("@");
				stringBuilder.Append(PointToString(pos));
				stringBuilder.Append(";");
				stringBuilder.Append(PointToString(list2[0]));
			}
			stringBuilder.Append("|dest@");
			stringBuilder.Append("(null)");
			stringBuilder.Append("@");
			stringBuilder.Append(PathToString(list2));
		}
		onComplete?.Invoke(stringBuilder.ToString());
		yield break;
	}

	public static Vector2Int[] GetBuildingNeighbors(long buildUuid)
	{
		LuaBuildData buildingDataByUuid = GameEntry.Data.Building.GetBuildingDataByUuid(buildUuid);
		if (buildingDataByUuid == null)
		{
			return null;
		}
		return GetBuildingNeighbors(buildingDataByUuid.buildId, buildingDataByUuid.pointId);
	}

	public static Vector2Int[] GetBuildingNeighbors(int itemId, int pointId)
	{
		Vector2Int vector2Int = SceneManager.World.IndexToTilePos(pointId);
		int num = GameEntry.ConfigCache.GetTemplateData("building", itemId, "tiles").ToInt();
		List<Vector2Int> list = new List<Vector2Int>();
		for (int i = 0; i < num; i++)
		{
			list.Add(vector2Int + new Vector2Int(-i, -num));
			list.Add(vector2Int + new Vector2Int(1, -i));
			list.Add(vector2Int + new Vector2Int(-i, 1));
			list.Add(vector2Int + new Vector2Int(-num, -i));
		}
		return list.ToArray();
	}

	public static List<Vector2Int> GetBuildingNeighbors(LuaBuildData buildingDate, out List<Vector2Int> entries)
	{
		Vector2Int vector2Int = SceneManager.World.IndexToTilePos(buildingDate.pointId);
		int num = GameEntry.ConfigCache.GetTemplateData("building", buildingDate.buildId, "tiles").ToInt();
		entries = new List<Vector2Int>();
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num; j++)
			{
				entries.Add(vector2Int + new Vector2Int(-i, -j));
			}
		}
		List<Vector2Int> list = new List<Vector2Int>();
		for (int k = 0; k < num; k++)
		{
			list.Add(vector2Int + new Vector2Int(-k, -num));
			list.Add(vector2Int + new Vector2Int(1, -k));
			list.Add(vector2Int + new Vector2Int(-k, 1));
			list.Add(vector2Int + new Vector2Int(-num, -k));
		}
		return list;
	}

	public static Vector2Int GetNeighborDir(Vector2Int pos, Vector2Int buildPos, int tiles)
	{
		Vector2Int vector2Int = pos - buildPos;
		if (vector2Int.y > 0)
		{
			return Vector2Int.up;
		}
		if (vector2Int.x > 0)
		{
			return Vector2Int.right;
		}
		if (tiles == -vector2Int.x)
		{
			return Vector2Int.left;
		}
		return Vector2Int.down;
	}

	private static float CalNeighborWeight(Vector2Int pos, Vector2Int startPos, Vector2Int buildPos, int tiles)
	{
		float num = Vector2Int.Distance(pos, startPos);
		if (!SceneManager.World.IsSelfRoad(SceneManager.World.TilePosToIndex(pos)))
		{
			num += 100000f;
		}
		Vector2Int neighborDir = GetNeighborDir(pos, buildPos, tiles);
		if (neighborDir == Vector2Int.right || neighborDir == Vector2Int.left)
		{
			num += 1000f;
		}
		else if (neighborDir == Vector2Int.up)
		{
			num += 5000f;
		}
		return num;
	}

	public static Vector2Int[] GetSortedBuildingNeighbors(int itemId, int pointId, Vector2Int startPos)
	{
		Vector2Int[] buildingNeighbors = GetBuildingNeighbors(itemId, pointId);
		Vector2Int buildPos = SceneManager.World.IndexToTilePos(pointId);
		int tiles = GameEntry.ConfigCache.GetTemplateData("building", itemId, "tiles").ToInt();
		Array.Sort(buildingNeighbors, delegate(Vector2Int a, Vector2Int b)
		{
			float num = CalNeighborWeight(a, startPos, buildPos, tiles);
			float value = CalNeighborWeight(b, startPos, buildPos, tiles);
			return num.CompareTo(value);
		});
		return buildingNeighbors;
	}

	public static Vector2Int[] GetSortedBuildingNeighbors(LuaBuildData buildingDate, Vector2Int startPos, Vector2Int[] neighbors)
	{
		Vector2Int buildPos = SceneManager.World.IndexToTilePos(buildingDate.pointId);
		int tiles = GameEntry.ConfigCache.GetTemplateData("building", buildingDate.buildId, "tiles").ToInt();
		Array.Sort(neighbors, delegate(Vector2Int a, Vector2Int b)
		{
			float num = CalNeighborWeight(a, startPos, buildPos, tiles);
			float value = CalNeighborWeight(b, startPos, buildPos, tiles);
			return num.CompareTo(value);
		});
		return neighbors;
	}

	private static Vector2Int[] GetMainNeighbors()
	{
		return MainRoundRoadOffset.Select((Vector2Int i) => i + GameEntry.Data.Building.GetMainPos()).ToArray();
	}

	private static Vector2Int[] GetMainEntries()
	{
		return MainRoundEntryOffset.Keys.Select((Vector2Int i) => i + GameEntry.Data.Building.GetMainPos()).ToArray();
	}

	public static bool IsWalkAble(Vector2Int pos, Vector2Int targetPos, FindPathType findPathType)
	{
		switch (findPathType)
		{
		case FindPathType.WorldDomeTruck:
		{
			bool flag = SceneManager.World.IsTileWalkable(SceneManager.World.TileToWorld(pos));
			if (!(IsTruckRoad(pos) && flag))
			{
				return pos.Equals(targetPos);
			}
			return true;
		}
		case FindPathType.OnlyRoad:
			if (!IsRoad(pos))
			{
				return pos.Equals(targetPos);
			}
			return true;
		default:
			return false;
		}
	}

	public static bool IsTruckRoad(Vector2Int pos)
	{
		return SceneManager.World.IsTruckRoad(pos, FindPathType.WorldDomeTruck);
	}

	public static bool IsRoad(Vector2Int pos)
	{
		return SceneManager.World.IsTruckRoad(pos, FindPathType.OnlyRoad);
	}

	public static void GetTruckPathString(long targetBuildingUuid, Dictionary<ResourceType, long> resDict, Action<string> onComplete)
	{
		new Task(GetTruckPathStringEnumerator(targetBuildingUuid, resDict, onComplete));
	}

	private static List<Vector2Int> StringToPath(string pathStr)
	{
		List<Vector2Int> list = new List<Vector2Int>();
		if (!pathStr.IsNullOrEmpty())
		{
			string[] array = pathStr.Split(new char[1] { ';' });
			foreach (string text in array)
			{
				if (!string.IsNullOrEmpty(text))
				{
					string[] array2 = text.Split(new char[1] { ',' });
					list.Add(new Vector2Int(int.Parse(array2[0]), int.Parse(array2[1])));
				}
			}
		}
		return list;
	}

	public static List<PathInfo> StringToPathInfos(string pathStr)
	{
		List<PathInfo> list = new List<PathInfo>();
		if (string.IsNullOrEmpty(pathStr))
		{
			return list;
		}
		string[] array = pathStr.Split(new char[1] { '|' });
		foreach (string obj in array)
		{
			PathInfo pathInfo = new PathInfo();
			string[] array2 = obj.Split(new char[1] { '@' });
			pathInfo.type = array2[0];
			pathInfo.uuid = array2[1];
			pathInfo.path = StringToPath(array2[2]);
			list.Add(pathInfo);
		}
		return list;
	}

	public static List<Vector2Int> StringToPoint(string pathStr)
	{
		List<Vector2Int> list = new List<Vector2Int>();
		if (!pathStr.IsNullOrEmpty())
		{
			string[] array = pathStr.Split(new char[1] { ';' });
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[1] { ',' });
				list.Add(new Vector2Int(int.Parse(array2[0]), int.Parse(array2[1])));
			}
		}
		return list;
	}

	public static float CalRobotMoveNeedTime(Vector3 startPos, Vector3 endPos)
	{
		startPos = new Vector3(startPos.x, 0f, startPos.z);
		endPos = new Vector3(endPos.x, 0f, endPos.z);
		float num = Vector3.Distance(startPos, endPos);
		float num2 = Mathf.Pow(15f, 2f) / 24f;
		if (num2 > num)
		{
			return Mathf.Sqrt(2f * num / 12f);
		}
		float num3 = (num - num2) / 15f;
		return 1.25f + num3;
	}

	public static Vector2Int GetNearestMainOutPos(Vector2Int targetPos)
	{
		List<Vector2Int> truckPathMainOutPosList = GetTruckPathMainOutPosList();
		truckPathMainOutPosList.Sort((Vector2Int a, Vector2Int b) => Vector2Int.Distance(a, targetPos).CompareTo(Vector2Int.Distance(b, targetPos)));
		return truckPathMainOutPosList[0];
	}

	public static List<Vector2Int> GetTruckPathMainOutPosList()
	{
		Vector2Int mainPos = GameEntry.Data.Building.GetMainPos();
		return new List<Vector2Int>
		{
			mainPos + new Vector2Int(0, 3),
			mainPos + new Vector2Int(-1, 3),
			mainPos + new Vector2Int(0, -4),
			mainPos + new Vector2Int(-1, -4),
			mainPos + new Vector2Int(3, 0),
			mainPos + new Vector2Int(3, -1),
			mainPos + new Vector2Int(-4, 0),
			mainPos + new Vector2Int(-4, -1)
		};
	}

	public static List<Vector2Int> GetTruckPathMainOutPosList(out List<Vector2Int> entries)
	{
		Vector2Int mainPos = GameEntry.Data.Building.GetMainPos();
		entries = new List<Vector2Int>
		{
			mainPos + new Vector2Int(0, 2),
			mainPos + new Vector2Int(-1, 2),
			mainPos + new Vector2Int(0, -3),
			mainPos + new Vector2Int(-1, -3),
			mainPos + new Vector2Int(2, 0),
			mainPos + new Vector2Int(2, -1),
			mainPos + new Vector2Int(-3, 0),
			mainPos + new Vector2Int(-3, -1)
		};
		return GetTruckPathMainOutPosList();
	}

	private static Vector2Int GetOuterPosInnerPos(Vector2Int outPos)
	{
		Vector2Int mainPos = GameEntry.Data.Building.GetMainPos();
		mainPos += new Vector2Int(-1, -1);
		Vector2Int vector2Int = outPos - mainPos;
		if (vector2Int.x == 0)
		{
			return outPos + ((vector2Int.y > 0) ? Vector2Int.down : Vector2Int.up);
		}
		return outPos + ((vector2Int.x > 0) ? Vector2Int.left : Vector2Int.right);
	}

	private static Vector2Int GetMainBuildNextPointFromDif(Vector2Int dif)
	{
		if (dif.y == -1 && dif.x != 1)
		{
			return Vector2Int.right;
		}
		if (dif.x == 1 && dif.y != 1)
		{
			return Vector2Int.up;
		}
		if (dif.y == 1 && dif.x != -1)
		{
			return Vector2Int.left;
		}
		return Vector2Int.down;
	}

	public static List<Vector2Int> FindRoadMainBuildToOutPos(Vector2Int outPos)
	{
		List<Vector2Int> list = new List<Vector2Int>();
		Vector2Int mainPos = GameEntry.Data.Building.GetMainPos();
		mainPos += new Vector2Int(-1, -1);
		list.Add(mainPos);
		Vector2Int outerPosInnerPos = GetOuterPosInnerPos(outPos);
		Vector2Int truckPathEndPos = GetTruckPathEndPos();
		list.Add(truckPathEndPos);
		int num = 0;
		while (outerPosInnerPos != truckPathEndPos && num < 10)
		{
			Vector2Int dif = truckPathEndPos - mainPos;
			truckPathEndPos += GetMainBuildNextPointFromDif(dif);
			list.Add(truckPathEndPos);
			num++;
		}
		return list;
	}

	public static Vector2Int GetTruckPathEndPos()
	{
		return GameEntry.Data.Building.GetMainPos() + new Vector2Int(0, -1);
	}

	public static List<Vector2Int> FindRoadOutPosToMainBuild(Vector2Int outPos)
	{
		List<Vector2Int> list = new List<Vector2Int>();
		Vector2Int mainPos = GameEntry.Data.Building.GetMainPos();
		mainPos += new Vector2Int(-1, -1);
		Vector2Int outerPosInnerPos = GetOuterPosInnerPos(outPos);
		Vector2Int truckPathEndPos = GetTruckPathEndPos();
		list.Add(outerPosInnerPos);
		int num = 0;
		while (truckPathEndPos != outerPosInnerPos && num < 10)
		{
			Vector2Int dif = outerPosInnerPos - mainPos;
			outerPosInnerPos += GetMainBuildNextPointFromDif(dif);
			list.Add(outerPosInnerPos);
			num++;
		}
		list.Add(mainPos);
		return list;
	}

	public static List<Vector2Int> FindMainBuildRoadRoundPath(Vector2Int inPos, Vector2Int outPos)
	{
		List<Vector2Int> list = new List<Vector2Int>();
		Vector2Int mainPos = GameEntry.Data.Building.GetMainPos();
		mainPos += new Vector2Int(-1, -1);
		Vector2Int outerPosInnerPos = GetOuterPosInnerPos(inPos);
		Vector2Int outerPosInnerPos2 = GetOuterPosInnerPos(outPos);
		list.Add(outerPosInnerPos);
		int num = 0;
		while (outerPosInnerPos2 != outerPosInnerPos && num < 10)
		{
			Vector2Int dif = outerPosInnerPos - mainPos;
			outerPosInnerPos += GetMainBuildNextPointFromDif(dif);
			list.Add(outerPosInnerPos);
			num++;
		}
		return list;
	}

	public static Vector2Int[] GetPeopleNearestMainOutPos(Vector2Int[] targetPosNeighbors, Vector2Int targetPos)
	{
		Vector2Int[] array = new Vector2Int[2];
		Vector2Int[] mainNeighbors = GetMainNeighbors();
		bool flag = false;
		Vector2Int[] array2 = mainNeighbors;
		foreach (Vector2Int vector2Int in array2)
		{
			if (!IsTruckRoad(vector2Int))
			{
				return null;
			}
			if (targetPosNeighbors.Contains(vector2Int))
			{
				array[0] = vector2Int;
				array[1] = vector2Int;
				flag = true;
			}
		}
		if (flag)
		{
			return array;
		}
		List<Vector2Int> list = new List<Vector2Int>();
		array2 = GetMainEntries();
		foreach (Vector2Int vector2Int2 in array2)
		{
			if (IsTruckRoad(vector2Int2))
			{
				list.Add(vector2Int2);
			}
		}
		if (list.Count == 0)
		{
			return null;
		}
		list.Sort(delegate(Vector2Int a, Vector2Int b)
		{
			float num = Vector2Int.Distance(a, targetPos);
			float value = Vector2Int.Distance(b, targetPos);
			return num.CompareTo(value);
		});
		Vector2Int vector2Int3 = list[0];
		Vector2Int vector2Int4 = MainRoundEntryOffset[vector2Int3 - GameEntry.Data.Building.GetMainPos()] + GameEntry.Data.Building.GetMainPos();
		array[0] = vector2Int3;
		array[1] = vector2Int4;
		return array;
	}

	public static List<Vector2Int> GetPeopleMainAroundPath(Vector2Int startPos)
	{
		List<Vector2Int> list = new List<Vector2Int>();
		List<Vector2Int> list2 = GetMainNeighbors().ToList();
		int num = list2.IndexOf(startPos);
		if (num == -1)
		{
			return list;
		}
		bool flag = UnityEngine.Random.Range(0, 2) == 0;
		int num2 = 2;
		int num3 = 0;
		int num4 = num;
		while (num3 < num2)
		{
			if (flag)
			{
				num4--;
				if (num4 < 0)
				{
					num4 += list2.Count;
				}
			}
			else
			{
				num4 = (num4 + 1) % list2.Count;
			}
			Vector2Int item = list2[num4];
			list.Add(item);
			if (num == num4)
			{
				num3++;
			}
		}
		return list;
	}

	public static Vector3 GetDroneCenterPos(Vector3 dronePos)
	{
		return dronePos + new Vector3((0f - SceneManager.World.TileSize) / 2f, 0f, (0f - SceneManager.World.TileSize) / 2f - 0.4f);
	}

	public static Vector3 GetRoadRobotWorkStartPosForLua(LuaTable roads)
	{
		List<int> list = new List<int>();
		if (roads != null)
		{
			for (int i = 1; i <= roads.Length; i++)
			{
				int item = roads[i].ToInt();
				list.Add(item);
			}
		}
		return GetRoadRobotWorkStartPos(list);
	}

	public static Vector3 GetRoadRobotWorkStartPos(List<int> roads)
	{
		if (roads == null || roads.Count == 0)
		{
			return Vector3.zero;
		}
		Vector3 vector2;
		if (roads.Count == 1)
		{
			Vector3 vector = SceneManager.World.TileIndexToWorld(roads[0]);
			vector2 = Vector3.Cross(rhs: Vector3.Normalize(new Vector3(SceneManager.World.TileSize, 0f, 0f)), lhs: Vector3.up) * SceneManager.World.TileSize;
			return vector + vector2;
		}
		Vector3 rhs = Vector3.Normalize(SceneManager.World.TileIndexToWorld(roads[1]) - SceneManager.World.TileIndexToWorld(roads[0]));
		vector2 = Vector3.Cross(Vector3.up, rhs) * SceneManager.World.TileSize;
		return SceneManager.World.TileIndexToWorld(roads[0]) + vector2;
	}
}
