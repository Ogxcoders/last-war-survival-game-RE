using System.Collections.Generic;
using Protobuf;
using Sfs2X.Entities.Data;
using UnityEngine;

public class WorldGreen
{
	private WorldScene world;

	private HashSet<int> _greenTileChangeList = new HashSet<int>();

	private HashSet<int> _greenTile = new HashSet<int>();

	private Dictionary<int, long> _greenAreaInfo = new Dictionary<int, long>();

	private int _lwAoiBlockSize = 10;

	private int _lwAoiBlockCount = 100;

	public WorldGreen(WorldScene scene)
	{
		world = scene;
	}

	public void Clear()
	{
		_greenTileChangeList.Clear();
		_greenAreaInfo.Clear();
		_greenTile.Clear();
	}

	public bool IsGreen(int index)
	{
		return _greenTile.Contains(index);
	}

	public bool CanGreen(int pointIndex, bool showTip = false)
	{
		if (IsGreen(pointIndex))
		{
			if (showTip)
			{
				UIUtils.ShowTips("season_oasis_tips_6", 3f);
			}
			return false;
		}
		if (!IsNearbyGreen(pointIndex))
		{
			if (showTip)
			{
				UIUtils.ShowTips("season_oasis_tips_0", 3f);
			}
			return false;
		}
		return true;
	}

	public bool IsNearbyGreen(int pointIndex, int range = 1)
	{
		Vector2Int vector2Int = world.IndexToTilePos(pointIndex);
		for (int i = vector2Int.y - range; i <= vector2Int.y + range; i++)
		{
			for (int j = vector2Int.x - range; j <= vector2Int.x + range; j++)
			{
				if ((j != vector2Int.x || i != vector2Int.y) && IsGreen(world.TilePosToIndex(new Vector2Int(j, i))))
				{
					return true;
				}
			}
		}
		return false;
	}

	public void ParseWorldCityGreens(WorldAllCityGreenInfo list)
	{
		if (list == null || list.InfoList.Count == 0)
		{
			return;
		}
		foreach (CityGreenInfo info in list.InfoList)
		{
			WorldZoneData zoneData = world.GetZoneData(info.CityId);
			if (zoneData != null)
			{
				zoneData.Rate = (int)(info.GreenRate * 1000.0);
			}
		}
	}

	public void ParseWorldGreenArea(ISFSArray green, ISFSObject message)
	{
		int num = int.MaxValue;
		int num2 = int.MinValue;
		int num3 = int.MaxValue;
		int num4 = int.MinValue;
		_greenTileChangeList.Clear();
		for (int i = 0; i < green.Count; i++)
		{
			IndexGreenPoints indexGreenPoints = IndexGreenPoints.Parser.ParseFrom(green.GetByteArray(i).Bytes);
			if (indexGreenPoints == null || indexGreenPoints.Index < 0)
			{
				continue;
			}
			if (!_greenAreaInfo.TryGetValue(indexGreenPoints.Index, out var value))
			{
				_greenAreaInfo.Add(indexGreenPoints.Index, indexGreenPoints.TimeStamp);
				GetBlockArea(indexGreenPoints.Index, out var xMin, out var yMin, out var xMax, out var yMax);
				num = Mathf.Min(num, xMin);
				num2 = Mathf.Max(num2, xMax);
				num3 = Mathf.Min(num3, yMin);
				num4 = Mathf.Max(num4, yMax);
				foreach (int point in indexGreenPoints.Points)
				{
					SetGreenInfo(point, isGreen: true);
				}
			}
			else
			{
				if (value == indexGreenPoints.TimeStamp)
				{
					continue;
				}
				_greenAreaInfo[indexGreenPoints.Index] = indexGreenPoints.TimeStamp;
				GetBlockArea(indexGreenPoints.Index, out var xMin2, out var yMin2, out var xMax2, out var yMax2);
				foreach (int point2 in indexGreenPoints.Points)
				{
					_greenTileChangeList.Add(point2);
				}
				for (int j = xMin2; j < xMax2; j++)
				{
					for (int k = yMin2; k < yMax2; k++)
					{
						int num5 = world.TilePosToIndex(new Vector2Int(j, k));
						if (SetGreenInfo(num5, _greenTileChangeList.Contains(num5)))
						{
							num = Mathf.Min(num, j);
							num2 = Mathf.Max(num2, j);
							num3 = Mathf.Min(num3, k);
							num4 = Mathf.Max(num4, k);
						}
					}
				}
			}
		}
		_greenTileChangeList.Clear();
		if (num <= num2 && num3 <= num4)
		{
			world.UpdateGreenArea(num, num3, num2, num4);
		}
	}

	public void UpdateGreenArea(WorldAreaGreenInfo areaGreens)
	{
		_greenTileChangeList.Clear();
		bool isGreen = areaGreens.Type == WorldAreaGreenInfo.GreenType.Default || areaGreens.Type == WorldAreaGreenInfo.GreenType.Green;
		int num = int.MaxValue;
		int num2 = int.MaxValue;
		int num3 = int.MinValue;
		int num4 = int.MinValue;
		foreach (int point in areaGreens.Points)
		{
			Vector2Int vector2Int = world.IndexToTilePos(point);
			num = Mathf.Min(num, vector2Int.x);
			num2 = Mathf.Min(num2, vector2Int.y);
			num3 = Mathf.Max(num3, vector2Int.x);
			num4 = Mathf.Max(num4, vector2Int.y);
			_greenTileChangeList.Add(point);
			SetGreenInfo(point, isGreen);
		}
		world.GreenAreaChange(WorldAreaGreenInfo.GreenType.Green, _greenTileChangeList);
		world.UpdateGreenArea(num, num2, num3, num4);
	}

	public bool SetGreenInfo(int pointIndex, bool isGreen)
	{
		if (isGreen)
		{
			return _greenTile.Add(pointIndex);
		}
		return _greenTile.Remove(pointIndex);
	}

	public void GetBlockArea(int index, out int xMin, out int yMin, out int xMax, out int yMax)
	{
		int num = index % _lwAoiBlockCount;
		int num2 = index / _lwAoiBlockCount;
		int num3 = num * _lwAoiBlockSize;
		int num4 = num2 * _lwAoiBlockSize;
		xMin = num3;
		yMin = num4;
		xMax = num3 + _lwAoiBlockSize;
		yMax = num4 + _lwAoiBlockSize;
	}
}
