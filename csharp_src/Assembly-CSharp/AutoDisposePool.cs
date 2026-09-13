using UnityEngine;

public abstract class AutoDisposePool : BasePool
{
	public AutoDisposePool prev;

	public AutoDisposePool next;

	private AutoDisposePoolManager _manager;

	private float _lastActiveTime;

	public float lastActiveTime => _lastActiveTime;

	protected AutoDisposePool(AutoDisposePoolManager manager, string prefabPath, ResourceManager resourceManager)
		: base(prefabPath, resourceManager)
	{
		_manager = manager;
		_lastActiveTime = Time.realtimeSinceStartup;
	}

	protected override void BeforeSpawnObject()
	{
		base.BeforeSpawnObject();
		if (GetObjCount() <= 4 || !((float)GetPoolCount() > (float)GetObjCount() / 2f))
		{
			_lastActiveTime = Time.realtimeSinceStartup;
			_manager.OnPoolSpawn(this);
		}
	}
}
