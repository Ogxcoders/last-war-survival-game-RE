using System.Collections.Generic;
using System.Linq;
using GameFramework;
using VEngine;

public class DownloadManifestManager
{
	private static DownloadManifestManager _instance;

	private bool _hasUpdate;

	private Dictionary<int, DownLoadGroupData> _downloadMap = new Dictionary<int, DownLoadGroupData>();

	private Dictionary<int, DownLoadGroupData> _loaderCache = new Dictionary<int, DownLoadGroupData>();

	public static DownloadManifestManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new DownloadManifestManager();
			}
			return _instance;
		}
	}

	public int GetCurrentDownloadConfigId()
	{
		if (_downloadMap.Count > 0)
		{
			return _downloadMap.Keys.First();
		}
		return -1;
	}

	public DownLoadGroupData CreateDownloadData(int configId)
	{
		int num = GameEntry.ConfigCache.GetTemplateData("download_packs", configId, "pack_id").ToInt();
		int[] array = null;
		if (num >= 1001 && num < 1009)
		{
			array = ((!ResourcePackageManager.IsPackageDownloaded(1000)) ? new int[2] { 1000, num } : new int[1] { num });
		}
		else if (num >= 5000 && num < 5501)
		{
			array = ((!ResourcePackageManager.IsPackageDownloaded(5000)) ? new int[2] { 5000, num } : new int[1] { num });
		}
		if (array == null)
		{
			Log.Error($"[DownloadManifestManager] no packages data . {configId}");
			return null;
		}
		DownLoadGroupData value = null;
		Manifest manifest = Versions.GetManifest(GameEntry.Resource.GameResManifestName);
		if (!_loaderCache.TryGetValue(configId, out value))
		{
			value = new DownLoadGroupData();
			_loaderCache.Add(configId, value);
		}
		value.manifest = manifest;
		value.configId = configId;
		value.packageIds = array;
		value.CalcDownloadInfo();
		return value;
	}

	public DownLoadGroupData AddNewDownload(DownLoadGroupData loader)
	{
		DownLoadGroupData value = null;
		if (!_downloadMap.TryGetValue(loader.configId, out value))
		{
			value = loader;
			_downloadMap.Add(value.configId, value);
		}
		if (value != null)
		{
			value.SendRequest();
			if (!_hasUpdate)
			{
				Updater.AddUpdateCallback(Update);
				_hasUpdate = true;
			}
		}
		return value;
	}

	public DownLoadGroupData AddNewDownload(int configId, bool isStart)
	{
		DownLoadGroupData value = null;
		if (!_downloadMap.TryGetValue(configId, out value))
		{
			value = CreateDownloadData(configId);
			if (value == null)
			{
				Log.Info($"[DownloadManifestManager] no data : {configId}");
				return null;
			}
			_downloadMap.Add(value.configId, value);
		}
		if (value != null)
		{
			if (isStart)
			{
				value.SendRequest();
			}
			if (!_hasUpdate)
			{
				Updater.AddUpdateCallback(Update);
				_hasUpdate = true;
			}
		}
		return value;
	}

	public void StopAllDownload()
	{
		Log.Info("[DownloadManifestManager] StopAllDownload.");
		if (_downloadMap != null && _downloadMap.Count > 0)
		{
			foreach (KeyValuePair<int, DownLoadGroupData> item in _downloadMap)
			{
				item.Value.Stop();
				GameEntry.Event.Fire(EventId.LWSeasonResourceDownloadStop, item.Key);
			}
			_downloadMap.Clear();
		}
		if (_hasUpdate)
		{
			Updater.RemoveUpdateCallback(Update);
			_hasUpdate = false;
		}
	}

	public DownLoadGroupData GetLoadManifestData(int configId)
	{
		if (_downloadMap.TryGetValue(configId, out var value))
		{
			return value;
		}
		return null;
	}

	public DownLoadGroupData StopDownload(int configId)
	{
		if (_downloadMap.TryGetValue(configId, out var value))
		{
			value.Stop();
			_downloadMap.Remove(configId);
			GameEntry.Event.Fire(EventId.LWSeasonResourceDownloadStop, configId);
			return value;
		}
		return null;
	}

	private void Update()
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, DownLoadGroupData> item in _downloadMap)
		{
			if (item.Value.CheckFinish())
			{
				list.Add(item.Key);
			}
		}
		foreach (int item2 in list)
		{
			GameEntry.Event.Fire(EventId.LWSeasonResourceDownloadFinish, item2);
			_downloadMap.Remove(item2);
		}
		if (_downloadMap.Count <= 0)
		{
			if (_hasUpdate)
			{
				Updater.RemoveUpdateCallback(Update);
			}
			_hasUpdate = false;
		}
	}
}
