using System;
using MiniGame.Core;
using VEngine;

namespace MiniGame.Biubiu.Client;

public class ResourceHolder : IResourceHolder
{
	private Asset _asset;

	public object Data => _asset.asset;

	public bool IsDone => _asset.isDone;

	public bool IsError => _asset.isError;

	public bool IsValid => _asset.asset != null;

	public float Progress => _asset.progress;

	public string ErrorMessage { get; private set; }

	public Action<IResourceHolder> OnDone { get; set; }

	public ResourceHolder(Asset asset)
	{
		_asset = asset;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, new Action<Asset>(OnAssetCompleted));
	}

	public T As<T>()
	{
		return (T)Data;
	}

	public void Dispose()
	{
		Asset asset = _asset;
		asset.completed = (Action<Asset>)Delegate.Remove(asset.completed, new Action<Asset>(OnAssetCompleted));
		if (!_asset.isDone)
		{
			Asset asset2 = _asset;
			asset2.completed = (Action<Asset>)Delegate.Combine(asset2.completed, new Action<Asset>(OnLoadingToRelease));
		}
		else
		{
			_asset.Release();
		}
	}

	private void OnAssetCompleted(Asset asset)
	{
		OnDone?.Invoke(this);
	}

	private void OnLoadingToRelease(Asset asset)
	{
		asset.completed = (Action<Asset>)Delegate.Remove(asset.completed, new Action<Asset>(OnLoadingToRelease));
		asset.Release();
	}
}
