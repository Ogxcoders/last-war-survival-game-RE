using UnityEngine;

public abstract class BaseCityTroopState
{
	public CityTroop cityTroop;

	public CityTroopMachine machine;

	public BaseCityTroopState(CityTroop troop, CityTroopMachine troopMachine)
	{
		cityTroop = troop;
		machine = troopMachine;
	}

	public abstract void OnEnter();

	public abstract void OnUpdate(float deltaTime);

	public abstract void OnLeave();

	public virtual void OnDrag()
	{
	}

	public virtual void OnBeginDrag()
	{
	}

	public virtual void OnEndDrag()
	{
	}

	public virtual void CityTroopMoveSignal(Vector3 pos)
	{
	}

	protected Vector3 CheckCanMovePosByPos(Vector3 pos)
	{
		Vector3 result = Vector3.zero;
		int num = SceneManager.World.WorldToTileIndex(pos);
		if (GameEntry.Data.Fog.IsUnlock(num))
		{
			if (SceneManager.World.GetPointType(num) != 100)
			{
				if (!cityTroop.IsTruck() && SceneManager.World.GetPointType(num) == 2)
				{
					GameEntry.Lua.CallWithReturn<bool, int, string>("DataCenter.GuideManager:CheckDoTriggerGuide", 23, "1");
				}
				else
				{
					result = pos;
				}
			}
		}
		else
		{
			UIUtils.ShowTips("300716", 3f);
		}
		return result;
	}
}
