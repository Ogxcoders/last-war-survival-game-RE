using System.Collections.Generic;
using UnityEngine;

public class HSRTroop
{
	private WorldMarch marchInfo;

	private Transform parent;

	private Transform transform;

	private GameObject go;

	private HSRMarch hsrMarch;

	private List<HSRCarriage> carriages = new List<HSRCarriage>();

	public HSRTroop(HSRMarch hsrMarch, WorldMarch marchInfo, Transform parent)
	{
		this.marchInfo = marchInfo;
		this.parent = parent;
		this.hsrMarch = hsrMarch;
		Init();
	}

	public void Refresh(HSRMarch hsrMarch, WorldMarch marchInfo)
	{
		this.hsrMarch = hsrMarch;
		this.marchInfo = marchInfo;
		foreach (HSRCarriage carriage in carriages)
		{
			carriage.Refresh();
		}
	}

	public void Destroy()
	{
		foreach (HSRCarriage carriage in carriages)
		{
			carriage.Destroy();
		}
		carriages.Clear();
		if (go != null)
		{
			Object.Destroy(go);
		}
		transform = null;
		marchInfo = null;
		parent = null;
		hsrMarch = null;
	}

	private void Init()
	{
		go = new GameObject("HSR");
		transform = go.transform;
		transform.SetParent(parent);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		int carriageCount = hsrMarch.GetCarriageCount();
		for (int i = 0; i < carriageCount; i++)
		{
			HSRCarriage item = new HSRCarriage(hsrMarch, i, transform, HSRMarchManager.GetInstance().GetSpeed());
			carriages.Add(item);
		}
		OnUpdate();
	}

	public void OnUpdate()
	{
		long serverTime = GameEntry.Timer.GetServerTime();
		foreach (HSRCarriage carriage in carriages)
		{
			carriage.OnUpdate(serverTime);
		}
	}
}
