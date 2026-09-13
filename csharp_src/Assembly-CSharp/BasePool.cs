using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using VEngine;

public abstract class BasePool
{
	protected string _prefabPath;

	protected Asset _request;

	protected Stack<GameObject> _pool = new Stack<GameObject>();

	protected int _objCount;

	public Asset Request => _request;

	public string prefabPath => _prefabPath;

	public event Action OnClear;

	public BasePool(string prefabPath, ResourceManager resourceManager)
	{
		_prefabPath = prefabPath;
		_request = resourceManager.LoadAssetAsync(prefabPath, typeof(GameObject));
		if (_request == null)
		{
			Log.Error("ObjectPool Init Error:req is null  " + prefabPath);
		}
	}

	public GameObject Spawn()
	{
		if (_request == null)
		{
			return null;
		}
		BeforeSpawnObject();
		if (_pool.Count > 0)
		{
			GameObject gameObject = _pool.Pop();
			if (gameObject == null)
			{
				Log.Error("ObjectPool :: null object in pool! " + _prefabPath);
				if (_request.asset == null)
				{
					return null;
				}
				_objCount++;
				GameObject gameObject2 = UnityEngine.Object.Instantiate(_request.asset) as GameObject;
				AfterSpawnObject(gameObject2);
				return gameObject2;
			}
			AfterSpawnObject(gameObject);
			return gameObject;
		}
		if (_request.asset == null)
		{
			Log.Error($"ObjectPool::Spawn error {_prefabPath} ,requestStatus:{_request.status}");
			return null;
		}
		_objCount++;
		return UnityEngine.Object.Instantiate(_request.asset) as GameObject;
	}

	public void DeSpawn(GameObject obj)
	{
		if (!BeforeDeSpawnObject(obj))
		{
			DeSpawnImmediate(obj);
			return;
		}
		_pool.Push(obj);
		AfterDeSpawnObject();
	}

	public bool Clear()
	{
		while (_pool.Count != 0)
		{
			UnityEngine.Object.Destroy(_pool.Pop());
			_objCount--;
		}
		_pool.Clear();
		if (_request != null && _objCount == 0)
		{
			_request.Release();
			_request = null;
			if (this.OnClear != null)
			{
				this.OnClear();
			}
			return true;
		}
		return false;
	}

	public void DeSpawnImmediate(GameObject obj)
	{
		if (!(obj == null))
		{
			UnityEngine.Object.Destroy(obj);
			_objCount--;
			AfterDeSpawnImmediate();
		}
	}

	public virtual void BeforeInstantiate()
	{
	}

	protected virtual void BeforeSpawnObject()
	{
	}

	protected virtual void AfterSpawnObject(GameObject gameObject)
	{
	}

	protected virtual bool BeforeDeSpawnObject(GameObject gameObject)
	{
		return true;
	}

	protected virtual void AfterDeSpawnObject()
	{
	}

	protected virtual void AfterDeSpawnImmediate()
	{
	}

	public int GetPoolCount()
	{
		return _pool.Count;
	}

	public int GetObjCount()
	{
		return _objCount;
	}
}
