using System.Collections.Generic;

public class WorldTruckManager : WorldManagerBase
{
	private Dictionary<long, WorldCityTruck> _cityTruckDict = new Dictionary<long, WorldCityTruck>();

	public WorldTruckManager(WorldScene scene)
		: base(scene)
	{
	}

	public new void UnInit()
	{
		_cityTruckDict.Clear();
	}

	private void CreateTruck(List<int> roadPoints, long orderUuid)
	{
		if (roadPoints.Count > 0)
		{
			WorldCityTruck value = new WorldCityTruck();
			_cityTruckDict.Add(orderUuid, value);
		}
	}

	private void RemoveTruckByUid(long orderUuid)
	{
		_cityTruckDict.ContainsKey(orderUuid);
	}

	public void RemoveAllTruck()
	{
		foreach (KeyValuePair<long, WorldCityTruck> item in _cityTruckDict)
		{
			_ = item;
		}
	}

	public override void OnUpdate(float deltaTime)
	{
		base.OnUpdate(deltaTime);
	}

	public WorldCityTruck GetCityTruck(long orderUid)
	{
		if (_cityTruckDict.ContainsKey(orderUid))
		{
			return _cityTruckDict[orderUid];
		}
		return null;
	}
}
