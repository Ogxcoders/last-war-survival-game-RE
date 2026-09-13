using UnityEngine;

public class CityCityTruckManager : CityManagerBase
{
	public TruckManagerBase TruckManagerBase = new TruckManagerBase();

	private bool _visible = true;

	public CityCityTruckManager(CityScene scene)
		: base(scene)
	{
	}

	public override void Init()
	{
		base.Init();
		GameEntry.Event.Subscribe(EventId.UpdateRoadData, UpdateRoad);
		GameEntry.Event.Subscribe(EventId.UPDATE_BUILD_DATA, UpdateMainCache);
		GameEntry.Event.Subscribe(EventId.SetCityPeopleAndCarVisible, SetCityPeopleAndCarVisibleSignal);
	}

	public override void UnInit()
	{
		GameEntry.Event.Unsubscribe(EventId.UpdateRoadData, UpdateRoad);
		GameEntry.Event.Unsubscribe(EventId.UPDATE_BUILD_DATA, UpdateMainCache);
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
		TruckManagerBase.SetBuildCacheChange(isChange: true);
	}

	public override void OnUpdate(float dea)
	{
		RefreshVisible();
	}

	public GameObject GetPeopleObjByIndex(int index)
	{
		return TruckManagerBase.GetPeopleObjByIndex(index);
	}

	public float PauseAndPlayAnim(int index, string anim)
	{
		return TruckManagerBase.PauseAndPlayAnim(index, anim);
	}

	public void Resume(int index)
	{
		TruckManagerBase.Resume(index);
	}

	public void ClearAllTruck()
	{
		TruckManagerBase.ClearAllTruck();
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
			if (visible)
			{
				TruckManagerBase = new TruckManagerBase();
			}
			_visible = visible;
			if (!_visible)
			{
				TruckManagerBase.Destroy();
			}
		}
	}

	private void RefreshVisible()
	{
		if (_visible)
		{
			TruckManagerBase.ShowRandomTruckAndPeople(scene.DynamicObjNode);
		}
	}
}
