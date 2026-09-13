using System.Collections.Generic;
using UnityEngine;

namespace VEngine;

public class Dependencies : Loadable
{
	internal readonly List<Bundle> bundles = new List<Bundle>();

	internal Bundle bundle;

	internal AssetBundle assetBundle => bundle?.assetBundle;

	internal bool isSyncLoad { get; set; }

	protected override void OnLoad()
	{
		if (!Versions.GetDependencies(base.pathOrURL, out var bundleInfo, out var array, out var assetId))
		{
			Finish("Dependencies.OnLoad, Dependencies not found");
			return;
		}
		if (bundleInfo == null)
		{
			Finish("Dependencies.OnLoad, info == null");
			return;
		}
		bundle = Bundle.LoadInternal(bundleInfo, base.mustCompleteOnNextFrame, reverseAddLoadable, isSyncLoad, assetId);
		bundles.Add(bundle);
		if (array != null && array.Length != 0)
		{
			BundleInfo[] array2 = array;
			foreach (BundleInfo info in array2)
			{
				bundles.Add(Bundle.LoadInternal(info, base.mustCompleteOnNextFrame, reverseAddLoadable, isSyncLoad, assetId));
			}
		}
	}

	public override void LoadImmediate()
	{
		if (base.isDone)
		{
			return;
		}
		foreach (Bundle bundle in bundles)
		{
			bundle.LoadImmediate();
		}
	}

	protected override void OnUnload()
	{
		if (bundles.Count > 0)
		{
			foreach (Bundle bundle in bundles)
			{
				if (string.IsNullOrEmpty(bundle.error))
				{
					bundle.Release();
				}
			}
			bundles.Clear();
		}
		this.bundle = null;
	}

	protected override void OnUpdate()
	{
		LoadableStatus loadableStatus = base.status;
		if (loadableStatus != LoadableStatus.Loading)
		{
			return;
		}
		float num = 0f;
		bool flag = true;
		foreach (Bundle bundle in bundles)
		{
			num += bundle.progress;
			if (!string.IsNullOrEmpty(bundle.error))
			{
				base.status = LoadableStatus.FailedToLoad;
				base.error = bundle.error;
				base.progress = 1f;
				return;
			}
			if (!bundle.isDone)
			{
				flag = false;
				break;
			}
		}
		base.progress = num / (float)bundles.Count * 0.5f;
		if (flag)
		{
			if (assetBundle == null)
			{
				Finish("Dependencies.OnLoad, assetBundle == null");
			}
			else
			{
				Finish();
			}
		}
	}
}
