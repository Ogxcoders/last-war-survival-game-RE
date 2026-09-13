using System;
using System.Collections.Generic;

namespace VEngine;

public class Loadable
{
	protected internal static readonly List<Loadable> Loading = new List<Loadable>();

	public bool reverseAddLoadable;

	public static bool reverseState = false;

	protected readonly Reference reference = new Reference();

	public static bool unuse_not_done = false;

	public LoadableStatus status { get; protected set; }

	public string pathOrURL { get; set; }

	protected bool mustCompleteOnNextFrame { get; set; }

	public string error { get; internal set; }

	public bool isError => !string.IsNullOrEmpty(error);

	public bool isDone
	{
		get
		{
			if (status != LoadableStatus.SuccessToLoad && status != LoadableStatus.Unloaded)
			{
				return status == LoadableStatus.FailedToLoad;
			}
			return true;
		}
	}

	protected internal bool keepAliveOnLoad { get; set; }

	public float progress { get; protected set; }

	public int referenceCount => reference.count;

	protected void Finish(string errorCode = null)
	{
		error = errorCode;
		status = (string.IsNullOrEmpty(errorCode) ? LoadableStatus.SuccessToLoad : LoadableStatus.FailedToLoad);
		progress = 1f;
	}

	public static void UpdateLoadables()
	{
		for (int i = 0; i < Loading.Count; i++)
		{
			Loadable loadable = Loading[i];
			if (Updater.busy)
			{
				break;
			}
			loadable.Update();
			if (loadable.isDone)
			{
				Loading.RemoveAt(i);
				i--;
				loadable.Complete();
			}
		}
		Asset.UpdateAssets();
		Bundle.UpdateBundles();
		ManifestFile.UpdateFiles();
	}

	internal static void Add(Loadable loadable)
	{
		Loading.Add(loadable);
	}

	internal void Update()
	{
		OnUpdate();
	}

	internal void Complete()
	{
		if (status == LoadableStatus.FailedToLoad)
		{
			Logger.E("Unable to load {0} {1} with error: {2}", GetType().Name, pathOrURL, error);
			Release();
		}
		OnComplete();
	}

	protected virtual void OnUpdate()
	{
	}

	protected virtual void OnLoad()
	{
	}

	protected virtual void OnUnload()
	{
	}

	protected virtual void OnComplete()
	{
	}

	public virtual void LoadImmediate()
	{
		throw new InvalidOperationException();
	}

	protected virtual void SyncOnLoad()
	{
		throw new NotImplementedException("must impl SyncLoad if you want to use it");
	}

	protected internal void SyncLoad()
	{
		SyncOnLoad();
	}

	protected internal void Load()
	{
		reference.Retain();
		bool flag = reverseAddLoadable && status == LoadableStatus.Wait;
		if (!flag)
		{
			Add(this);
		}
		if (status == LoadableStatus.Wait)
		{
			status = LoadableStatus.Loading;
			progress = 0f;
			OnLoad();
			if (flag)
			{
				Add(this);
			}
		}
	}

	protected internal bool Unload()
	{
		if (status == LoadableStatus.Unloaded)
		{
			return false;
		}
		OnUnload();
		status = LoadableStatus.Unloaded;
		return true;
	}

	public void Release()
	{
		if (reference.count <= 0)
		{
			Logger.W("Release {0} {1} status:{2} id:{3} {4}. reference.count <= 0", GetType().Name, reference.count, status, pathOrURL, GetHashCode());
		}
		else
		{
			if (keepAliveOnLoad)
			{
				return;
			}
			if (!unuse_not_done && !isDone)
			{
				Logger.E($"ResourceManager::Release when asset is not done. {pathOrURL},{GetHashCode()}, referenceCount:{reference.count}");
				return;
			}
			reference.Release();
			if (reference.unused)
			{
				OnUnused();
			}
		}
	}

	protected virtual void OnUnused()
	{
	}

	protected virtual void ForceStop()
	{
	}

	public static void ForceUnloadLoading()
	{
		int num;
		for (num = 0; num < Loading.Count; num++)
		{
			Loadable loadable = Loading[num];
			loadable.Update();
			if (loadable.isDone)
			{
				loadable.Complete();
			}
			else
			{
				loadable.ForceStop();
				loadable.Finish("Force stop");
			}
			Loading.RemoveAt(num);
			num--;
		}
	}
}
