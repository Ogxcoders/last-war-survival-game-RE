using System;
using UnityEngine;

public class WorldVFXObj : IDisposable
{
	private InstanceRequest requestInst;

	private float duration;

	private float delay;

	private Action<GameObject> callback;

	public int id;

	private Vector3 pos;

	private string prefabPath;

	public void Dispose()
	{
		if (requestInst != null)
		{
			requestInst.Destroy();
			requestInst = null;
		}
		prefabPath = null;
		callback = null;
	}

	public void SetData(int id, string prefabPath, Vector3 pos, float duration, float delay, Action<GameObject> callback)
	{
		this.id = id;
		this.prefabPath = prefabPath;
		this.pos = pos;
		this.duration = duration;
		this.delay = delay;
		this.callback = callback;
		if (delay == 0f)
		{
			Instantiate();
		}
	}

	private void Instantiate()
	{
		requestInst = GameEntry.Resource.InstantiateAsync(prefabPath);
		if (requestInst == null)
		{
			return;
		}
		requestInst.completed += delegate
		{
			GameObject gameObject = requestInst.gameObject;
			if (!(gameObject == null))
			{
				gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
				gameObject.transform.position = pos;
				if (callback != null)
				{
					callback(gameObject);
					callback = null;
				}
			}
		};
	}

	public bool OnUpdate(float deltaTime)
	{
		if (delay > 0f)
		{
			delay -= deltaTime;
			if (delay <= 0f)
			{
				Instantiate();
			}
			return false;
		}
		duration -= deltaTime;
		return duration <= 0f;
	}
}
