public class WorldCityTruckManager : WorldManagerBase
{
	public TruckManagerBase TruckManagerBase = new TruckManagerBase();

	private bool _visible = true;

	private int lodCache;

	public WorldCityTruckManager(WorldScene scene)
		: base(scene)
	{
	}

	public override void Init()
	{
		base.Init();
		GameEntry.Event.Subscribe(EventId.UpdateRoadData, UpdateRoad);
		GameEntry.Event.Subscribe(EventId.UPDATE_BUILD_DATA, UpdateMainCache);
		GameEntry.Event.Subscribe(EventId.ChangeCameraLod, UpdateLod);
		GameEntry.Event.Subscribe(EventId.SetCityPeopleAndCarVisible, SetCityPeopleAndCarVisibleSignal);
	}

	public override void UnInit()
	{
		GameEntry.Event.Unsubscribe(EventId.UpdateRoadData, UpdateRoad);
		GameEntry.Event.Unsubscribe(EventId.UPDATE_BUILD_DATA, UpdateMainCache);
		GameEntry.Event.Unsubscribe(EventId.ChangeCameraLod, UpdateLod);
		GameEntry.Event.Unsubscribe(EventId.SetCityPeopleAndCarVisible, SetCityPeopleAndCarVisibleSignal);
		TruckManagerBase.Destroy();
	}

	private void UpdateRoad(object userData)
	{
		TruckManagerBase.ExpireRoadCache();
	}

	private void UpdateMainCache(object userData)
	{
		if (TruckManagerBase.MainBuildUuid == (long)userData)
		{
			TruckManagerBase.UpdateMainBuildCheck();
		}
	}

	public override void OnUpdate(float dea)
	{
		RefreshVisible();
	}

	public void ClearAllTruck()
	{
		TruckManagerBase.ClearAllTruck();
	}

	private void UpdateLod(object obj)
	{
		lodCache = (int)obj;
		RefreshVisible();
	}

	private void SetCityPeopleAndCarVisibleSignal(object userData)
	{
		if (userData != null)
		{
			switch ((CityPeopleAndCarVisibleType)userData.ToInt())
			{
			case CityPeopleAndCarVisibleType.AllShow:
				SetVisible(visible: true);
				break;
			case CityPeopleAndCarVisibleType.AllHide:
				SetVisible(visible: false);
				break;
			}
		}
	}

	private void SetVisible(bool visible)
	{
		if (_visible != visible)
		{
			_visible = visible;
			RefreshVisible();
		}
	}

	private void RefreshVisible()
	{
		if (_visible && lodCache <= 1)
		{
			TruckManagerBase.ShowRandomTruckAndPeople(world.DynamicObjNode);
		}
		else
		{
			ClearAllTruck();
		}
	}
}
