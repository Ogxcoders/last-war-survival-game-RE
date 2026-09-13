using System;
using MiniGame.Core;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class SnapshotGameObjectHolder : IResourceHolder
{
	public object Data { get; set; }

	public bool IsDone => true;

	public bool IsError => false;

	public bool IsValid => true;

	public float Progress => 1f;

	public string ErrorMessage { get; private set; }

	public Action<IResourceHolder> OnDone { get; set; }

	public T As<T>()
	{
		return (T)Data;
	}

	public SnapshotGameObjectHolder(GameObject data)
	{
		Data = data;
	}

	public void Dispose()
	{
		if (Data != null)
		{
			UnityEngine.Object.Destroy(Data as GameObject);
			Data = null;
		}
	}
}
