using System;
using UnityEngine;

namespace VEngine;

public class BundledAsset : Asset
{
	private Dependencies dependencies;

	private AssetBundleRequest request;

	internal static BundledAsset Create(string path, Type type)
	{
		return new BundledAsset
		{
			pathOrURL = path,
			type = type,
			reverseAddLoadable = Loadable.reverseState
		};
	}

	internal static bool IsAssetDownloaded(string path)
	{
		if (!Versions.GetDependencies(path, out var bundle, out var bundles))
		{
			return false;
		}
		if (!Versions.IsDownloaded(bundle))
		{
			return false;
		}
		BundleInfo[] array = bundles;
		for (int i = 0; i < array.Length; i++)
		{
			if (!Versions.IsDownloaded(array[i]))
			{
				return false;
			}
		}
		return true;
	}

	protected override void OnLoad()
	{
		dependencies = new Dependencies
		{
			pathOrURL = base.pathOrURL,
			reverseAddLoadable = reverseAddLoadable,
			isSyncLoad = base.mustCompleteOnNextFrame
		};
		dependencies.Load();
		base.status = LoadableStatus.DependentLoading;
	}

	protected override void OnUnload()
	{
		if (dependencies != null)
		{
			dependencies.Unload();
			dependencies = null;
		}
		request = null;
		base.asset = null;
	}

	public override void LoadImmediate()
	{
		if (base.isDone)
		{
			return;
		}
		if (dependencies == null)
		{
			Finish("BundledAsset.LoadImmediate, dependencies == null");
			return;
		}
		if (!dependencies.isDone)
		{
			dependencies.LoadImmediate();
		}
		if (dependencies.assetBundle == null)
		{
			Finish("BundledAsset.LoadImmediate, dependencies.assetBundle == null");
			return;
		}
		base.asset = dependencies.assetBundle.LoadAsset(base.pathOrURL, base.type);
		if (base.asset == null)
		{
			Finish("BundledAsset.LoadImmediate, asset == null");
		}
		else
		{
			Finish();
		}
	}

	protected override void OnUpdate()
	{
		switch (base.status)
		{
		case LoadableStatus.Loading:
			UpdateLoading();
			break;
		case LoadableStatus.DependentLoading:
			UpdateDependencies();
			break;
		}
	}

	private void UpdateLoading()
	{
		if (request == null)
		{
			Finish("BundledAsset.UpdateLoading, request == null");
			return;
		}
		base.progress = 0.5f + request.progress * 0.5f;
		if (request.isDone)
		{
			base.asset = request.asset;
			if (base.asset == null)
			{
				Finish("BundledAsset.UpdateLoading, asset == null");
			}
			else
			{
				Finish();
			}
		}
	}

	private void UpdateDependencies()
	{
		if (dependencies == null)
		{
			Finish("BundledAsset.UpdateDependencies, dependencies == null");
			return;
		}
		base.progress = 0.5f * dependencies.progress;
		if (dependencies.isDone)
		{
			AssetBundle assetBundle = dependencies.assetBundle;
			if (assetBundle == null)
			{
				Finish("BundledAsset.UpdateDependencies, assetBundle == null");
				return;
			}
			request = assetBundle.LoadAssetAsync(base.pathOrURL, base.type);
			base.status = LoadableStatus.Loading;
		}
	}
}
