using System.Collections.Generic;
using UnityEngine;

namespace VEngine;

public sealed class InstantiateObject : Operation
{
	internal static readonly List<InstantiateObject> AllObjects = new List<InstantiateObject>();

	private Asset asset;

	public string path { get; internal set; }

	public GameObject result { get; private set; }

	public override void Start()
	{
		base.Start();
		asset = Asset.LoadAsync(path, typeof(GameObject));
		AllObjects.Add(this);
	}

	public static InstantiateObject InstantiateAsync(string assetPath)
	{
		InstantiateObject instantiateObject = new InstantiateObject();
		instantiateObject.path = assetPath;
		instantiateObject.Start();
		return instantiateObject;
	}

	protected override void Update()
	{
		OperationStatus operationStatus = base.status;
		if (operationStatus != OperationStatus.Processing)
		{
			return;
		}
		if (asset == null)
		{
			Finish("asset == null");
			return;
		}
		base.progress = asset.progress;
		if (!asset.isDone)
		{
			return;
		}
		if (asset.status == LoadableStatus.FailedToLoad)
		{
			Finish("asset.status == LoadableStatus.LoadFailed");
			return;
		}
		if (asset.asset == null)
		{
			Finish("asset.asset == null");
		}
		result = Object.Instantiate(asset.asset as GameObject);
		Finish();
	}

	public void Destroy()
	{
		if (!base.isDone)
		{
			Finish("User Cancelled");
			return;
		}
		if (base.status == OperationStatus.Success && result != null)
		{
			Object.DestroyImmediate(result);
			result = null;
		}
		if (asset != null)
		{
			if (string.IsNullOrEmpty(asset.error))
			{
				asset.Release();
			}
			asset = null;
		}
	}

	public static void UpdateObjects()
	{
		for (int i = 0; i < AllObjects.Count; i++)
		{
			InstantiateObject instantiateObject = AllObjects[i];
			if (Updater.busy)
			{
				break;
			}
			if (instantiateObject.isDone && !(instantiateObject.result != null))
			{
				AllObjects.RemoveAt(i);
				i--;
				instantiateObject.Destroy();
			}
		}
	}
}
