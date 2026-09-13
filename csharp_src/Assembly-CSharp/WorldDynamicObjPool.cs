using System.Collections.Generic;
using UnityEngine;

public class WorldDynamicObjPool : AutoDisposePool
{
	private List<AutoAdjustLod> _cacheLod = new List<AutoAdjustLod>();

	private List<AutoAdjustScale> _cacheScales = new List<AutoAdjustScale>();

	private List<AutoFaceToCamera> _cacheFaceCamera = new List<AutoFaceToCamera>();

	private List<ParticleSystem> _partsicle = new List<ParticleSystem>();

	public WorldDynamicObjPool(AutoDisposePoolManager manager, string prefabPath, ResourceManager resourceManager)
		: base(manager, prefabPath, resourceManager)
	{
	}

	protected override bool BeforeDeSpawnObject(GameObject gameObject)
	{
		gameObject.SetActive(value: false);
		return true;
	}

	protected override void AfterSpawnObject(GameObject gameObject)
	{
		base.AfterSpawnObject(gameObject);
		gameObject.SetActive(value: true);
	}
}
