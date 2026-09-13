using System;
using System.Collections.Generic;
using System.IO;
using GameFramework;

namespace VEngine;

public sealed class RawAssetFile : Loadable
{
	private static readonly Dictionary<string, RawAssetFile> Cache = new Dictionary<string, RawAssetFile>();

	public Action<RawAssetFile> completed;

	private BundleInfo info;

	private Download download;

	private string rawSavePath;

	private string downloadUrl;

	public string localRawAssetPath;

	public string name { get; private set; }

	public string fileExt { get; private set; }

	protected override void OnLoad()
	{
		if (!Versions.GetDependencies(name, out info, out var _))
		{
			Finish("rawFile not found in Versions/Manifest.");
			return;
		}
		downloadUrl = Versions.GetDownloadURL(info.name);
		rawSavePath = Versions.GetDownloadDataPath(info.alias);
		localRawAssetPath = "";
		if (string.IsNullOrEmpty(fileExt))
		{
			localRawAssetPath = rawSavePath;
		}
		else
		{
			localRawAssetPath = Path.ChangeExtension(rawSavePath, fileExt);
		}
		if (Validate(localRawAssetPath, needCrcCheck: true))
		{
			Finish();
			return;
		}
		download = Download.DownloadAsync(downloadUrl, localRawAssetPath, 1, null, info.size, info.crc);
		Download obj = download;
		obj.completed = (Action<Download>)Delegate.Combine(obj.completed, new Action<Download>(OnDownloaded));
		base.status = LoadableStatus.Downloading;
		base.progress = 0f;
	}

	private void OnDownloaded(Download obj)
	{
		if (download != null && download.status == DownloadStatus.Failed)
		{
			Finish(download.error);
		}
		else if (!Validate(localRawAssetPath, needCrcCheck: false))
		{
			Finish("Downloaded file invalid.");
		}
		else
		{
			Finish(base.error);
		}
	}

	protected override void OnUpdate()
	{
		LoadableStatus loadableStatus = base.status;
		if (loadableStatus == LoadableStatus.Downloading && download != null && !download.isDone)
		{
			base.progress = (float)download.downloadedBytes * 1f / (float)download.info.size * 0.5f;
			if (!string.IsNullOrEmpty(download.error))
			{
				Finish(download.error);
			}
		}
	}

	protected override void OnComplete()
	{
		Action<RawAssetFile> action = completed;
		completed = null;
		action?.Invoke(this);
		if (Cache.ContainsKey(name))
		{
			Cache.Remove(name);
		}
	}

	protected override void OnUnused()
	{
		completed = null;
	}

	protected override void ForceStop()
	{
		if (download != null && !download.isDone)
		{
			download.Cancel();
		}
	}

	protected override void OnUnload()
	{
		if (download != null)
		{
			ForceStop();
			download = null;
		}
		base.OnUnload();
	}

	public static void SafeDelete(string path)
	{
		try
		{
			if (!string.IsNullOrEmpty(path) && File.Exists(path))
			{
				File.Delete(path);
			}
		}
		catch (Exception ex)
		{
			Log.Error("SafeDelete failed, path:" + path + ", ex:" + ex.Message);
		}
	}

	private bool Validate(string path, bool needCrcCheck)
	{
		if (string.IsNullOrEmpty(path) || !File.Exists(path))
		{
			return false;
		}
		try
		{
			using FileStream fileStream = File.OpenRead(path);
			if (info.size != (ulong)fileStream.Length)
			{
				Log.Error("File size is wrong,path:" + path);
				return false;
			}
			if (needCrcCheck && Utility.ComputeCRC32(fileStream) != info.crc)
			{
				Log.Error("File cre is wrong,path:" + path);
				return false;
			}
			return true;
		}
		catch (Exception ex)
		{
			Log.Error("RawFile Validate exception：" + ex.Message);
			return false;
		}
	}

	public static RawAssetFile LoadAsync(string rawAssetPath, string fileExt = "")
	{
		if (!Cache.TryGetValue(rawAssetPath, out var value))
		{
			value = new RawAssetFile
			{
				name = rawAssetPath,
				fileExt = fileExt
			};
			Cache.Add(rawAssetPath, value);
		}
		value.Load();
		return value;
	}
}
