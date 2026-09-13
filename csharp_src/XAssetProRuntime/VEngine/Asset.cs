using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VEngine;

public class Asset : Loadable, IEnumerator
{
	protected internal static readonly Dictionary<string, Asset> Cache = new Dictionary<string, Asset>();

	protected internal static readonly List<Asset> Unused = new List<Asset>();

	public Action<Asset> completed;

	private static readonly Dictionary<string, int> AssetLoadCountDict = new Dictionary<string, int>();

	public static Action<string> EditorHookBeforeRequestAsset;

	public UnityEngine.Object asset { get; protected set; }

	protected Type type { get; set; }

	public object Current => null;

	public bool MoveNext()
	{
		return !base.isDone;
	}

	public void Reset()
	{
	}

	public T Get<T>() where T : UnityEngine.Object
	{
		return asset as T;
	}

	protected override void OnComplete()
	{
		if (completed == null)
		{
			return;
		}
		Action<Asset> action = completed;
		completed = null;
		try
		{
			action(this);
		}
		catch (Exception arg)
		{
			Debug.LogError($"Error in Asset.OnComplete callback for {base.pathOrURL}: {arg}");
		}
	}

	protected override void OnUnused()
	{
		completed = null;
		Unused.Add(this);
	}

	public static void KeepAliveOnLoad(Asset asset)
	{
		asset.keepAliveOnLoad = true;
	}

	public static Asset LoadAsync(string path, Type type, Action<Asset> completed = null)
	{
		return LoadInternal(path, type, mustCompleteOnNextFrame: false, completed);
	}

	public static Asset Load(string path, Type type)
	{
		return LoadInternal(path, type, mustCompleteOnNextFrame: true);
	}

	internal static Asset LoadInternal(string path, Type type, bool mustCompleteOnNextFrame, Action<Asset> completed = null)
	{
		if (Versions.GetAsset(ref path) == null)
		{
			Logger.E("FileNotFoundException {0}", path);
			return null;
		}
		if (!Cache.TryGetValue(path, out var value))
		{
			value = Versions.CreateAsset(path, type);
			Cache.Add(path, value);
		}
		if (completed != null)
		{
			Asset obj = value;
			obj.completed = (Action<Asset>)Delegate.Combine(obj.completed, completed);
		}
		value.mustCompleteOnNextFrame = mustCompleteOnNextFrame;
		if (AssetLoadCountDict.TryGetValue(path, out var value2))
		{
			value2++;
			AssetLoadCountDict[path] = value2;
		}
		else
		{
			AssetLoadCountDict[path] = 1;
		}
		value.Load();
		if (mustCompleteOnNextFrame)
		{
			value.LoadImmediate();
		}
		return value;
	}

	public static void UpdateAssets()
	{
		for (int i = 0; i < Unused.Count; i++)
		{
			Asset asset = Unused[i];
			if (Updater.busy)
			{
				break;
			}
			if (asset.isDone)
			{
				Unused.RemoveAt(i);
				i--;
				if (asset.reference.unused)
				{
					asset.Unload();
					Cache.Remove(asset.pathOrURL);
				}
			}
		}
	}

	public static void RemoveCachedUnusedAssets()
	{
		for (int i = 0; i < Unused.Count; i++)
		{
			Asset asset = Unused[i];
			if (asset.isDone)
			{
				Unused.RemoveAt(i);
				i--;
				if (asset.reference.unused)
				{
					asset.Unload();
					Cache.Remove(asset.pathOrURL);
				}
			}
		}
	}

	public static void UnloadUnusedAssets()
	{
		while (Unused.Count > 0)
		{
			int num;
			for (num = 0; num < Unused.Count; num++)
			{
				Asset asset = Unused[num];
				Unused.RemoveAt(num);
				num--;
				if (asset.reference.unused)
				{
					asset.Unload();
					Cache.Remove(asset.pathOrURL);
				}
			}
		}
		Bundle.UnloadUnusedBundles();
		Resources.UnloadUnusedAssets();
	}

	public static void DebugOutputCache()
	{
	}

	public static void DebugLoadCount()
	{
	}
}
