using System;
using System.Collections.Generic;
using System.IO;
using GameFramework;
using VEngine;

public class PackageDownloadChecker : IDownloadChecker
{
	private Dictionary<string, long> _downloads;

	public void Collect(string downloadPath)
	{
		_downloads = new Dictionary<string, long>();
		foreach (string item in Directory.EnumerateFiles(downloadPath, "*"))
		{
			string fileName = Path.GetFileName(item);
			_downloads.Add(fileName, -1L);
		}
	}

	public void AddDownload(Download download)
	{
		try
		{
			string fileName = Path.GetFileName(download.info.savePath);
			if (download.status == DownloadStatus.Success)
			{
				_downloads[fileName] = (long)download.info.size;
			}
		}
		catch (Exception arg)
		{
			Log.Error($"PackageDownloadChecker AddDownload: {arg}");
		}
	}

	public bool IsDownloaded(BundleInfo bundle)
	{
		if (Versions.SkipUpdate || bundle.inPlayerAssets)
		{
			return true;
		}
		if (_downloads.ContainsKey(bundle.name))
		{
			return true;
		}
		if (_downloads.ContainsKey(bundle.alias))
		{
			return true;
		}
		return false;
	}

	public void RemoveBundle(BundleInfo bundle)
	{
		if (!bundle.inPlayerAssets)
		{
			if (_downloads.ContainsKey(bundle.name))
			{
				bundle.isDownloaded = false;
				_downloads.Remove(bundle.name);
			}
			else if (_downloads.ContainsKey(bundle.alias))
			{
				bundle.isDownloaded = false;
				_downloads.Remove(bundle.alias);
			}
		}
	}
}
