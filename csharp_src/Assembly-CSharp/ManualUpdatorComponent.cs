using GameKit.Base;
using UnityEngine;

public abstract class ManualUpdatorComponent : MonoBehaviour, IManualUpdator
{
	public float lastUpdateTime { get; set; }

	public abstract void ManualUpdate(float delta);

	protected void OnEnable()
	{
		SingletonBehaviour<ManualUpdateManager>.Instance.Add(this);
	}

	protected void OnDisable()
	{
		SingletonBehaviour<ManualUpdateManager>.Instance.Remove(this);
	}
}
