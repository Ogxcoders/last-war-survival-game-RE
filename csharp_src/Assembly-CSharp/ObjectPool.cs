using System;
using UnityEngine;

public class ObjectPool : BasePool
{
	private ObjectPoolMgr mgr;

	public float lastDespawnTime = float.MaxValue;

	private float _cleanPoolTime = 30f;

	private int _maxPooledObjectCount = -1;

	private ObjectPoolTag _tag;

	public float NextCleanTime => lastDespawnTime + _cleanPoolTime;

	public ObjectPoolTag tag => _tag;

	public new string prefabPath => _prefabPath;

	public new event Action OnClear;

	public ObjectPool(ObjectPoolMgr mgr, string prefabPath, ResourceManager resourceManager, ObjectPoolTag tag = ObjectPoolTag.Normal)
		: base(prefabPath, resourceManager)
	{
		this.mgr = mgr;
		_tag = tag;
		ObjectPoolTagConfig config = ObjectPoolTagConfig.GetConfig(tag);
		if (config != null)
		{
			_cleanPoolTime = config.cleanPoolTime;
			_maxPooledObjectCount = config.maxPooledObjectCount;
		}
	}

	protected override void BeforeSpawnObject()
	{
		base.BeforeSpawnObject();
		ClearDespawnTime();
	}

	protected override void AfterSpawnObject(GameObject gameObject)
	{
		base.AfterSpawnObject(gameObject);
		gameObject.transform.SetParent(null);
		gameObject.SetActive(value: true);
	}

	protected override bool BeforeDeSpawnObject(GameObject gameObject)
	{
		if (_maxPooledObjectCount > 0 && _objCount >= _maxPooledObjectCount)
		{
			return false;
		}
		lastDespawnTime = Time.realtimeSinceStartup;
		gameObject.SetActive(value: false);
		if (mgr != null)
		{
			gameObject.transform.SetParent(mgr.Root);
		}
		return true;
	}

	protected override void AfterDeSpawnObject()
	{
		if (mgr != null && _objCount == GetPoolCount())
		{
			mgr.RegisterToCleanPoolList(_prefabPath);
		}
	}

	protected override void AfterDeSpawnImmediate()
	{
		base.AfterDeSpawnImmediate();
		SetWaitForClear();
	}

	public override void BeforeInstantiate()
	{
		base.BeforeInstantiate();
		ClearDespawnTime();
	}

	public bool TryClean()
	{
		if (Time.realtimeSinceStartup - lastDespawnTime >= _cleanPoolTime)
		{
			Clear();
		}
		if (_objCount <= 0)
		{
			if (_request != null)
			{
				return _request.isDone;
			}
			return true;
		}
		return false;
	}

	public void SetCleanTime(float time)
	{
		_cleanPoolTime = time;
	}

	public void ClearDespawnTime()
	{
		lastDespawnTime = float.MaxValue;
		if (mgr != null)
		{
			mgr.UnRegisterToCleanPoolList(_prefabPath);
		}
	}

	public void SetWaitForClear()
	{
		lastDespawnTime = 0f;
		if (mgr != null)
		{
			mgr.RegisterToCleanPoolList(_prefabPath);
		}
	}
}
