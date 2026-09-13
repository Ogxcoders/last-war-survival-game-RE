using System;
using System.Collections.Generic;
using UnityEngine;

namespace VEngine;

public class Bundle : Loadable
{
	protected internal static readonly Dictionary<string, Bundle> Cache = new Dictionary<string, Bundle>();

	protected internal static readonly List<Bundle> Unused = new List<Bundle>();

	protected BundleInfo info;

	protected internal AssetBundle assetBundle { get; set; }

	public BundleType type { get; private set; }

	protected override void OnUnused()
	{
		Unused.Add(this);
	}

	internal static Bundle LoadInternal(BundleInfo info, bool mustCompleteOnNextFrame, bool reverseAddLoadable, bool isSyncLoad, int assetId = 0)
	{
		if (info == null)
		{
			throw new NullReferenceException();
		}
		if (!Cache.TryGetValue(info.name, out var value))
		{
			string bundlePathOrURL = Versions.GetBundlePathOrURL(info, isSyncLoad);
			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				value = new WebBundle
				{
					pathOrURL = bundlePathOrURL,
					info = info,
					reverseAddLoadable = reverseAddLoadable,
					type = BundleType.Web
				};
			}
			else if (!string.IsNullOrEmpty(Versions.DownloadURL) && bundlePathOrURL.StartsWith(Versions.DownloadURL))
			{
				value = new DownloadBundle
				{
					pathOrURL = bundlePathOrURL,
					info = info,
					reverseAddLoadable = reverseAddLoadable,
					type = BundleType.Download
				};
				AssetsStatistics.OnBundleLoaded(info, assetId);
			}
			else
			{
				value = new LocalBundle
				{
					pathOrURL = bundlePathOrURL,
					info = info,
					reverseAddLoadable = reverseAddLoadable,
					type = BundleType.Local
				};
			}
			Cache.Add(info.name, value);
		}
		else if (value.type == BundleType.Download)
		{
			AssetsStatistics.OnBundleLoaded(info, assetId);
		}
		value.mustCompleteOnNextFrame = mustCompleteOnNextFrame;
		value.Load();
		if (mustCompleteOnNextFrame)
		{
			value.LoadImmediate();
		}
		return value;
	}

	internal static void UpdateBundles()
	{
		for (int i = 0; i < Unused.Count; i++)
		{
			Bundle bundle = Unused[i];
			if (Updater.busy)
			{
				break;
			}
			if (bundle.isDone)
			{
				Unused.RemoveAt(i);
				i--;
				if (bundle.reference.unused && bundle.Unload() && Cache.TryGetValue(bundle.info.name, out var value) && value == bundle)
				{
					Cache.Remove(bundle.info.name);
				}
			}
		}
	}

	public static void UnloadUnusedBundles()
	{
		while (Unused.Count > 0)
		{
			int num;
			for (num = 0; num < Unused.Count; num++)
			{
				Bundle bundle = Unused[num];
				Unused.RemoveAt(num);
				num--;
				if (bundle.reference.unused && bundle.Unload() && Cache.TryGetValue(bundle.info.name, out var value) && value == bundle)
				{
					Cache.Remove(bundle.info.name);
				}
			}
		}
	}

	public static void DebugOutputCache()
	{
	}

	protected override void OnUnload()
	{
		if (!(assetBundle == null))
		{
			assetBundle.Unload(unloadAllLoadedObjects: true);
			assetBundle = null;
		}
	}

	public static bool IsBundleInCache(string assetBundleName)
	{
		if (Cache == null || !Cache.ContainsKey(assetBundleName))
		{
			return false;
		}
		return true;
	}
}
