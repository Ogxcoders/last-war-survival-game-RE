using System.Collections.Generic;
using UnityEngine;

public class HSRTroopManager : WorldManagerBase
{
	private Dictionary<long, HSRTroop> troopDict = new Dictionary<long, HSRTroop>();

	public HSRTroopManager(WorldScene scene)
		: base(scene)
	{
	}

	public override void Init()
	{
	}

	public override void UnInit()
	{
		foreach (HSRTroop value in troopDict.Values)
		{
			value.Destroy();
		}
		troopDict.Clear();
	}

	public void AddTroop(WorldMarch marchInfo, Transform parent)
	{
		if (!troopDict.TryGetValue(marchInfo.uuid, out var value))
		{
			HSRMarch march = HSRMarchManager.GetInstance().GetMarch(marchInfo.uuid);
			if (march != null)
			{
				value = new HSRTroop(march, marchInfo, parent);
				troopDict.Add(marchInfo.uuid, value);
			}
		}
	}

	public void RefreshTroop(WorldMarch marchInfo)
	{
		if (troopDict.TryGetValue(marchInfo.uuid, out var value))
		{
			HSRMarch march = HSRMarchManager.GetInstance().GetMarch(marchInfo.uuid);
			if (march != null)
			{
				value.Refresh(march, marchInfo);
			}
		}
	}

	public void RemoveTroop(long uuid)
	{
		if (troopDict.TryGetValue(uuid, out var value))
		{
			value.Destroy();
			troopDict.Remove(uuid);
		}
	}

	public override void OnUpdate(float deltaTime)
	{
		foreach (HSRTroop value in troopDict.Values)
		{
			value.OnUpdate();
		}
	}
}
