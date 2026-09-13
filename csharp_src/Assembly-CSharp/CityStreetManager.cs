using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityStreetManager
{
	private Dictionary<int, CityStreet> _streets = new Dictionary<int, CityStreet>();

	private List<CityStreet> randStreets = new List<CityStreet>();

	private bool _needUpdate = true;

	public void ExpireCache()
	{
		_needUpdate = true;
	}

	private void LoadStreet()
	{
		if (!_needUpdate)
		{
			return;
		}
		_needUpdate = false;
		Dictionary<int, CityStreet> dictionary = new Dictionary<int, CityStreet>();
		Dictionary<int, CityRoadPathParam> allRoads = GameEntry.Data.Road.getAllRoads();
		List<int> list = new List<int>(allRoads.Keys);
		foreach (KeyValuePair<int, CityRoadPathParam> item in allRoads)
		{
			if (item.Value.alikePathType(CityRoadPathParam.PathType.VIADUCT) && item.Value.alikePathType(CityRoadPathParam.PathType.MAIN_ROAD))
			{
				list.Add(item.Key);
			}
		}
		int count = list.Count;
		List<CityStreet> list2 = new List<CityStreet>();
		for (int i = 0; i < count; i++)
		{
			if (list.Count == 0)
			{
				break;
			}
			CityStreet cityStreet = new CityStreet(i);
			Vector2Int vector2Int = SceneManager.World.IndexToTilePos(list[0]);
			if (allRoads.TryGetValue(list[0], out var value))
			{
				if (value.alikePathType(CityRoadPathParam.PathType.VIADUCT) && value.alikePathType(CityRoadPathParam.PathType.MAIN_ROAD))
				{
					list.RemoveAt(0);
					continue;
				}
				List<int> list3 = new List<int>();
				list3.Add(list[0]);
				cityStreet.RoadList.Add(vector2Int);
				fillRoad(Vector2Int.zero, vector2Int, value, Vector2Int.zero, cityStreet, allRoads, 0, list3, isReserve: false);
				if (cityStreet.RoadList.Count > 1)
				{
					Vector2Int dir = cityStreet.RoadList[1] - cityStreet.RoadList[0];
					if (!dir.Equals(Vector2Int.zero) && !cityStreet.isSingle)
					{
						if (dir.x != 0)
						{
							dir.x = -dir.x;
						}
						else
						{
							dir.y = -dir.y;
						}
						fillRoad(Vector2Int.zero, vector2Int, value, dir, cityStreet, allRoads, 0, list3, isReserve: true);
					}
				}
				foreach (int item2 in list3)
				{
					list.Remove(item2);
				}
				list2.Add(cityStreet);
			}
			else
			{
				list.RemoveAt(0);
			}
		}
		List<CityStreet> list4 = mergeStreet(list2);
		fillExist(list4);
		foreach (CityStreet item3 in list4)
		{
			if (item3.RoadList.Count > 3 || item3.Exits.Count > 1)
			{
				dictionary.Add(item3.StreetId, item3);
			}
		}
		_streets = dictionary;
	}

	public List<Vector2Int> autoFindPath()
	{
		LoadStreet();
		List<Vector2Int> list = new List<Vector2Int>();
		if (randStreets.Count <= 0)
		{
			foreach (CityStreet value2 in _streets.Values)
			{
				if (value2.isSingle)
				{
					for (int i = 0; i < 2; i++)
					{
						randStreets.Add(value2);
					}
				}
				randStreets.Add(value2);
			}
			TruckManagerBase.ListRandom(randStreets);
		}
		if (randStreets.Count <= 0)
		{
			return new List<Vector2Int>();
		}
		CityStreet cityStreet = randStreets[0];
		randStreets.Remove(cityStreet);
		if (cityStreet.Exits.Count > 0)
		{
			int num = Random.Range(-1, cityStreet.Exits.Count);
			if (num != -1)
			{
				CityStreet.CityStreetExit cityStreetExit = cityStreet.Exits[num];
				int num2 = cityStreet.RoadList.IndexOf(cityStreetExit.Pos);
				if (num2 >= 0)
				{
					bool flag = true;
					if (!cityStreet.isSingle)
					{
						if (num2 == 0)
						{
							flag = false;
						}
						else if (num2 != cityStreet.RoadList.Count - 1)
						{
							flag = Random.Range(0, 100) < 50;
						}
					}
					if (flag)
					{
						list.AddRange(cityStreet.RoadList.GetRange(0, num2 + 1));
					}
					else
					{
						List<Vector2Int> list2 = new List<Vector2Int>(cityStreet.RoadList.GetRange(num2, cityStreet.RoadList.Count - num2));
						list2.Reverse();
						list.AddRange(list2);
					}
					if (_streets.TryGetValue(cityStreetExit.OutStreetId, out var value))
					{
						int num3 = value.RoadList.IndexOf(cityStreetExit.OutPos);
						if (num3 >= 0)
						{
							bool flag2 = false;
							if (!value.isSingle)
							{
								if (num3 <= 1)
								{
									flag2 = true;
								}
								else if (num3 < value.RoadList.Count - 2)
								{
									flag2 = Random.Range(0, 100) < 50;
								}
							}
							List<Vector2Int> list3 = new List<Vector2Int>();
							if (flag2)
							{
								list3.AddRange(value.RoadList.GetRange(num3, value.RoadList.Count - num3));
							}
							else
							{
								list3.AddRange(value.RoadList.GetRange(0, num3 + 1));
							}
							if (list3.IndexOf(cityStreetExit.Pos) != -1)
							{
								list3.Remove(cityStreetExit.OutPos);
								list3.Remove(cityStreetExit.Pos);
							}
							if (list.Count > 0 && list3.Count > 0 && Vector2Int.Distance(list[list.Count - 1], list3[0]) > 1.5f)
							{
								list3.Reverse();
							}
							list.AddRange(list3);
						}
					}
				}
			}
		}
		if (list.Count > 0)
		{
			return list;
		}
		bool flag3 = true;
		if (!cityStreet.isSingle)
		{
			flag3 = Random.Range(0, 100) < 50;
		}
		if (flag3)
		{
			list.AddRange(cityStreet.RoadList);
		}
		else
		{
			List<Vector2Int> list4 = new List<Vector2Int>(cityStreet.RoadList);
			list4.Reverse();
			list.AddRange(list4);
		}
		return list;
	}

	private void fillRoad(Vector2Int lastRoad, Vector2Int startRoad, CityRoadPathParam startParam, Vector2Int dir, CityStreet street, Dictionary<int, CityRoadPathParam> allRoads, int times, List<int> needRemoveIds, bool isReserve)
	{
		if (times > 100)
		{
			return;
		}
		List<Vector2Int> neighborFromParam = CityRoadPathParam.getNeighborFromParam(lastRoad, startRoad, allRoads);
		Vector2Int vector2Int = Vector2Int.zero;
		CityRoadPathParam cityRoadPathParam = null;
		foreach (Vector2Int item2 in neighborFromParam)
		{
			if (street.RoadList.Contains(item2) || !allRoads.TryGetValue(SceneManager.World.TilePosToIndex(item2), out var value))
			{
				continue;
			}
			if (dir.Equals(Vector2Int.zero))
			{
				if (startParam.alikePathType(CityRoadPathParam.PathType.MAIN_ROAD) ^ value.alikePathType(CityRoadPathParam.PathType.MAIN_ROAD))
				{
					if (startParam.alikePathType(CityRoadPathParam.PathType.MAIN_ROAD) && !value.alikePathType(CityRoadPathParam.PathType.VIADUCT))
					{
						street.isSingle = true;
						street.Exits.Add(new CityStreet.CityStreetExit(startRoad, item2));
					}
					continue;
				}
				if (startParam.alikePathType(CityRoadPathParam.PathType.MAIN_ROAD))
				{
					street.isSingle = true;
				}
				dir = item2 - startRoad;
			}
			int item = SceneManager.World.TilePosToIndex(item2);
			if ((item2 - startRoad).Equals(dir))
			{
				vector2Int = item2;
				cityRoadPathParam = value;
				if (isReserve)
				{
					street.RoadList.Insert(0, item2);
				}
				else
				{
					street.RoadList.Add(item2);
				}
				needRemoveIds.Add(item);
			}
			else if (!value.alikePathType(CityRoadPathParam.PathType.VIADUCT))
			{
				street.Exits.Add(new CityStreet.CityStreetExit(startRoad, item2));
			}
		}
		if (!(vector2Int == Vector2Int.zero) && cityRoadPathParam != null)
		{
			fillRoad(startRoad, vector2Int, cityRoadPathParam, dir, street, allRoads, times + 1, needRemoveIds, isReserve);
		}
	}

	private List<CityStreet> mergeStreet(List<CityStreet> streets)
	{
		List<CityStreet> list = new List<CityStreet>();
		int count = streets.Count;
		List<CityStreet> list2 = new List<CityStreet>(streets);
		for (int i = 0; i < count; i++)
		{
			if (list2.Count == 0)
			{
				break;
			}
			CityStreet cityStreet = list2[0];
			if (cityStreet.RoadList.Count <= 1)
			{
				list2.Remove(cityStreet);
				continue;
			}
			List<CityStreet> list3 = new List<CityStreet>(streets);
			for (int j = 0; j < 100; j++)
			{
				bool reserve;
				bool sourceStart;
				CityStreet canMergeStreet = getCanMergeStreet(cityStreet, list3, out reserve, out sourceStart);
				if (canMergeStreet == null)
				{
					break;
				}
				list3.Remove(canMergeStreet);
				list2.Remove(canMergeStreet);
				mergeRoadAndExit(cityStreet.RoadList, new List<Vector2Int>(canMergeStreet.RoadList), cityStreet.Exits, new List<CityStreet.CityStreetExit>(canMergeStreet.Exits), reserve, sourceStart, out var mergeRoad, out var mergeExit);
				cityStreet.RoadList = mergeRoad;
				cityStreet.Exits = mergeExit;
			}
			list.Add(cityStreet);
			list2.Remove(cityStreet);
		}
		return list;
	}

	private CityStreet getCanMergeStreet(CityStreet sourceStreet, List<CityStreet> streets, out bool reserve, out bool sourceStart)
	{
		reserve = false;
		sourceStart = false;
		Vector2Int vector2Int = sourceStreet.RoadList[1] - sourceStreet.RoadList[0];
		for (int i = 0; i < streets.Count; i++)
		{
			CityStreet cityStreet = streets[i];
			if (cityStreet.StreetId == sourceStreet.StreetId || (sourceStreet.isSingle ^ cityStreet.isSingle))
			{
				continue;
			}
			Vector2Int other = Vector2Int.zero;
			if (cityStreet.RoadList.Count > 1)
			{
				other = cityStreet.RoadList[1] - cityStreet.RoadList[0];
			}
			if ((!sourceStreet.isSingle || other.Equals(vector2Int)) && other.x * vector2Int.x + other.y * vector2Int.y != 0)
			{
				reserve = !vector2Int.Equals(other);
				if (cityStreet.RoadList.Contains(sourceStreet.RoadList[0] - vector2Int))
				{
					return cityStreet;
				}
				if (cityStreet.RoadList.Contains(sourceStreet.RoadList[sourceStreet.RoadList.Count - 1] + vector2Int))
				{
					sourceStart = true;
					return cityStreet;
				}
			}
		}
		return null;
	}

	private void mergeRoadAndExit(List<Vector2Int> sourceRoad, List<Vector2Int> targetRoad, List<CityStreet.CityStreetExit> sourceExit, List<CityStreet.CityStreetExit> targetExit, bool reverse, bool startSource, out List<Vector2Int> mergeRoad, out List<CityStreet.CityStreetExit> mergeExit)
	{
		mergeRoad = new List<Vector2Int>();
		mergeExit = new List<CityStreet.CityStreetExit>();
		mergeExit.AddRange(sourceExit);
		mergeExit.AddRange(targetExit);
		if (reverse)
		{
			targetRoad.Reverse();
			targetExit.Reverse();
		}
		if (sourceRoad.All((Vector2Int a) => targetRoad.Any((Vector2Int b) => b.Equals(a))))
		{
			mergeRoad.AddRange(targetRoad);
		}
		else if (targetRoad.All((Vector2Int a) => sourceRoad.Any((Vector2Int b) => b.Equals(a))))
		{
			mergeRoad.AddRange(sourceRoad);
		}
		else if (startSource)
		{
			mergeRoad.AddRange(sourceRoad);
			mergeRoad.AddRange(targetRoad);
		}
		else
		{
			mergeRoad.AddRange(targetRoad);
			mergeRoad.AddRange(sourceRoad);
		}
		mergeRoad = mergeRoad.Distinct().ToList();
	}

	private CityStreet findPosInStreet(Vector2Int pos, int streetId, List<CityStreet> streets)
	{
		foreach (CityStreet street in streets)
		{
			if (street.StreetId != streetId && street.RoadList.Contains(pos))
			{
				return street;
			}
		}
		return null;
	}

	private void fillExist(List<CityStreet> streets)
	{
		foreach (CityStreet street in streets)
		{
			List<Vector2Int> list = new List<Vector2Int>();
			List<CityStreet.CityStreetExit> list2 = new List<CityStreet.CityStreetExit>();
			foreach (CityStreet.CityStreetExit exit in street.Exits)
			{
				CityStreet cityStreet = findPosInStreet(exit.OutPos, street.StreetId, streets);
				if (cityStreet != null && cityStreet.StreetId != street.StreetId && !list.Contains(exit.OutPos))
				{
					exit.OutStreetId = cityStreet.StreetId;
					list.Add(exit.OutPos);
				}
				else
				{
					list2.Add(exit);
				}
			}
			foreach (CityStreet.CityStreetExit item in list2)
			{
				street.Exits.Remove(item);
			}
		}
	}
}
