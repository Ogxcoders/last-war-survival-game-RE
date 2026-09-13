using System;
using GameFramework;
using GameKit.Base;
using UnityEngine;

public class InstanceRequest
{
	public enum State
	{
		Init,
		Loading,
		Instanced,
		Destroy
	}

	private string prefabPath;

	private BasePool pool;

	private Func<string, BasePool> _createPoolFunc;

	public State state;

	public GameObject gameObject;

	public bool isUseCache;

	public ObjectPoolTag _poolTag;

	public string PrefabPath => prefabPath;

	public bool isDone { get; private set; }

	public bool isError { get; private set; }

	public bool poolIsReady
	{
		get
		{
			if (pool?.Request == null)
			{
				return false;
			}
			return pool.Request.isDone;
		}
	}

	public event Action<InstanceRequest> completed;

	public InstanceRequest(string prefabPath, Func<string, BasePool> createPoolFunc, int priority = 0)
	{
		this.prefabPath = prefabPath;
		state = State.Init;
		if (string.IsNullOrEmpty(prefabPath))
		{
			Log.Error("InstanceRequest prefabPath == null !");
		}
		_createPoolFunc = createPoolFunc;
	}

	public InstanceRequest(string prefabPath, ObjectPoolTag poolTag = ObjectPoolTag.Normal, int priority = 0)
	{
		this.prefabPath = prefabPath;
		state = State.Init;
		if (string.IsNullOrEmpty(prefabPath))
		{
			Log.Error("InstanceRequest prefabPath == null !");
		}
		_poolTag = poolTag;
	}

	public void Instantiate()
	{
		if (string.IsNullOrEmpty(prefabPath))
		{
			RealDestroy();
			return;
		}
		state = State.Loading;
		if (_createPoolFunc != null)
		{
			pool = _createPoolFunc(prefabPath);
		}
		else
		{
			pool = GameEntry.Resource.GetObjectPool(prefabPath, _poolTag);
		}
		pool.BeforeInstantiate();
		pool.OnClear += OnPoolClear;
	}

	private void OnPoolClear()
	{
		if (state == State.Loading)
		{
			Log.Error("InstanceRequest::ObjectPool cleared when request is still loading. path=" + prefabPath);
		}
		state = State.Destroy;
	}

	public void RealDestroy()
	{
		if (gameObject != null)
		{
			pool?.DeSpawnImmediate(gameObject);
			gameObject = null;
		}
		if (pool != null)
		{
			pool.OnClear -= OnPoolClear;
			pool = null;
		}
		_createPoolFunc = null;
		state = State.Destroy;
		this.completed = null;
	}

	public void Destroy()
	{
		if (gameObject != null)
		{
			pool.DeSpawn(gameObject);
			gameObject = null;
		}
		if (pool != null)
		{
			pool.OnClear -= OnPoolClear;
			pool = null;
		}
		_createPoolFunc = null;
		state = State.Destroy;
		this.completed = null;
	}

	public bool Update()
	{
		if (state == State.Destroy)
		{
			return false;
		}
		if (pool.Request == null)
		{
			return false;
		}
		if (!pool.Request.isDone)
		{
			return true;
		}
		try
		{
			ProfilerRuntime.BeginSample(prefabPath);
			if (gameObject == null)
			{
				ProfilerRuntime.BeginSample("Pool.Spawn");
				isUseCache = pool.GetPoolCount() > 0;
				gameObject = pool.Spawn();
				state = State.Instanced;
				isDone = true;
				ProfilerRuntime.EndSample();
			}
			isError = gameObject == null;
			if (this.completed != null)
			{
				ProfilerRuntime.BeginSample("Invoke");
				if (isError)
				{
					Log.Info("Resource::InstanceRequest null gameObject, path:" + prefabPath);
				}
				this.completed(this);
				this.completed = null;
				ProfilerRuntime.EndSample();
			}
		}
		catch (Exception message)
		{
			Log.Error(message);
		}
		finally
		{
			ProfilerRuntime.EndSample();
		}
		return false;
	}
}
