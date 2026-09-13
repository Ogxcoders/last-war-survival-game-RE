using System;
using GameFramework;
using UnityEngine;

namespace VEngine;

internal class DownloadBundle : Bundle
{
	private Download download;

	private AssetBundleCreateRequest request;

	public override void LoadImmediate()
	{
		if (!base.isDone)
		{
			Log.Error("DownloadBundle LoadImmediate " + base.pathOrURL);
		}
	}

	protected override void OnLoad()
	{
		string file = (BundleInfo.UseBundleAlias ? info.alias : info.name);
		download = Download.DownloadAsync(base.pathOrURL, Versions.GetDownloadDataPath(file), 1, null, info.size, info.crc);
		Download obj = download;
		obj.completed = (Action<Download>)Delegate.Combine(obj.completed, new Action<Download>(OnDownloaded));
	}

	protected override void OnUnload()
	{
		ForceStop();
		base.OnUnload();
	}

	private void OnDownloaded(Download obj)
	{
		if (download.status == DownloadStatus.Failed)
		{
			Finish(download.error);
		}
		else if (!(base.assetBundle != null))
		{
			request = AssetBundle.LoadFromFileAsync(obj.info.savePath);
			Versions.SetBundlePathOrURl(info.name, obj.info.savePath);
			base.status = LoadableStatus.Loading;
		}
	}

	protected override void OnUpdate()
	{
		if (base.status != LoadableStatus.Loading)
		{
			return;
		}
		if (download != null && !download.isDone)
		{
			base.progress = (float)download.downloadedBytes * 1f / (float)download.info.size * 0.5f;
			if (!string.IsNullOrEmpty(download.error))
			{
				Finish(download.error);
				return;
			}
		}
		if (request != null)
		{
			base.progress = 0.5f + request.progress;
			if (request.isDone)
			{
				base.assetBundle = request.assetBundle;
				Finish((base.assetBundle == null) ? "DownloadBundle.OnUpdate, assetBundle == null" : null);
				request = null;
			}
		}
	}

	protected override void ForceStop()
	{
		if (!download.isDone)
		{
			download.Cancel();
		}
		if (request != null)
		{
			AssetBundle assetBundle = request.assetBundle;
			if ((bool)assetBundle)
			{
				assetBundle.Unload(unloadAllLoadedObjects: false);
			}
			request = null;
		}
	}
}
