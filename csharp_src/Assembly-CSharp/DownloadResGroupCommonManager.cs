using System;
using System.Collections.Generic;
using System.Linq;
using GameFramework;
using VEngine;

public class DownloadResGroupCommonManager
{
	private static DownloadResGroupCommonManager _instance;

	private bool _hasUpdate;

	private Dictionary<int, DownloadResGroupCommonData> _downloadMap = new Dictionary<int, DownloadResGroupCommonData>();

	private Dictionary<int, DownloadResGroupCommonData> _loaderCache = new Dictionary<int, DownloadResGroupCommonData>();

	private List<int> deletingPackageConfigIdList = new List<int>();

	private Dictionary<int, List<BundleInfo>> deletingBundleListMap = new Dictionary<int, List<BundleInfo>>();

	private Dictionary<int, DownloadResGroupCommonData> deletingMap = new Dictionary<int, DownloadResGroupCommonData>();

	private List<int> finishedList = new List<int>();

	private int maxDeleteBundleNumEachFrame = 3;

	public static DownloadResGroupCommonManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new DownloadResGroupCommonManager();
			}
			return _instance;
		}
	}

	public void Clear()
	{
		if (_hasUpdate)
		{
			Updater.RemoveUpdateCallback(Update);
		}
		_hasUpdate = false;
		_downloadMap.Clear();
		_loaderCache.Clear();
		deletingPackageConfigIdList.Clear();
		deletingBundleListMap.Clear();
		deletingMap.Clear();
		finishedList.Clear();
	}

	public int GetCurrentDownloadConfigId()
	{
		if (_downloadMap.Count > 0)
		{
			return _downloadMap.Keys.First();
		}
		return -1;
	}

	public DownloadResGroupCommonData CreateDownloadData(int configId)
	{
		int packageId = GameEntry.ConfigCache.GetTemplateData("download_packs", configId, "pack_id").ToInt();
		DownloadResGroupCommonData value = null;
		Manifest manifest = Versions.GetManifest(GameEntry.Resource.GameResManifestName);
		if (!_loaderCache.TryGetValue(configId, out value))
		{
			value = new DownloadResGroupCommonData();
			_loaderCache.Add(configId, value);
			value.manifest = manifest;
			value.configId = configId;
			value.packageId = packageId;
			value.RecalcDownloadInfo();
			value.InitBundleInfo();
		}
		return value;
	}

	public bool IsDownload(int configId)
	{
		if (SDKManager.IS_UNITY_EDITOR())
		{
			return true;
		}
		if (configId <= 0)
		{
			Log.Error($"config value is error. configId:{configId}");
			return false;
		}
		return ResourcePackageManager.IsPackageDownloaded(GameEntry.ConfigCache.GetTemplateData("download_packs", configId, "pack_id").ToInt());
	}

	public DownloadResGroupCommonData StartDownload(int configId)
	{
		DownloadResGroupCommonData downloadResGroupCommonData = AddDownload(configId);
		if (downloadResGroupCommonData != null)
		{
			downloadResGroupCommonData.SendRequest();
			if (!_hasUpdate)
			{
				Updater.AddUpdateCallback(Update);
				_hasUpdate = true;
			}
		}
		return downloadResGroupCommonData;
	}

	public DownloadResGroupCommonData AddDownload(int configId)
	{
		DownloadResGroupCommonData value = null;
		if (!_downloadMap.TryGetValue(configId, out value))
		{
			value = CreateDownloadData(configId);
			if (value == null)
			{
				return null;
			}
			_downloadMap.Add(value.configId, value);
		}
		return value;
	}

	public DownloadResGroupCommonData StopDownload(int configId)
	{
		if (_downloadMap.TryGetValue(configId, out var value))
		{
			value.Stop();
			_downloadMap.Remove(configId);
			GameEntry.Event.Fire(EventId.CommonResourceGroupDownloadStop, configId);
			return value;
		}
		return null;
	}

	public DownloadResGroupCommonData[] DeleteDownloadPackageList(int[] configIdList)
	{
		DownloadResGroupCommonData[] array = new DownloadResGroupCommonData[configIdList.Length];
		HashSet<int> hashSet = new HashSet<int>();
		for (int i = 0; i < configIdList.Length; i++)
		{
			int num = configIdList[i];
			if (!deletingMap.ContainsKey(num))
			{
				if (_downloadMap.ContainsKey(num))
				{
					StopDownload(num);
				}
				DownloadResGroupCommonData downloadResGroupCommonData = CreateDownloadData(num);
				if (downloadResGroupCommonData == null)
				{
					return null;
				}
				deletingMap[num] = downloadResGroupCommonData;
				hashSet.Add(downloadResGroupCommonData.packageId);
				array[i] = downloadResGroupCommonData;
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j] != null)
			{
				List<BundleInfo> value = new List<BundleInfo>(array[j].CalcCanDeleteBundleInfoList(hashSet));
				deletingBundleListMap[array[j].configId] = value;
				deletingPackageConfigIdList.Add(array[j].configId);
			}
		}
		if (!_hasUpdate)
		{
			Updater.AddUpdateCallback(Update);
			_hasUpdate = true;
		}
		return array;
	}

	public bool IsPackageDeleteTotallyCompleted(int[] packageIdList, int configId)
	{
		HashSet<int> hashSet = new HashSet<int>();
		for (int i = 0; i < packageIdList.Length; i++)
		{
			hashSet.Add(packageIdList[i]);
		}
		DownloadResGroupCommonData downloadResGroupCommonData = CreateDownloadData(configId);
		if (downloadResGroupCommonData == null)
		{
			return true;
		}
		downloadResGroupCommonData.CalcCanDeleteBundleInfoList(hashSet);
		return downloadResGroupCommonData.IsDeleteTotallyCompleted();
	}

	public void StopDeletingPackage(int configId)
	{
		for (int i = 0; i < deletingPackageConfigIdList.Count; i++)
		{
			if (deletingPackageConfigIdList[i] == configId)
			{
				deletingPackageConfigIdList.RemoveAt(i);
			}
		}
		deletingBundleListMap.Remove(configId);
		deletingMap.Remove(configId);
	}

	public bool IsAnyPackageDeleting()
	{
		return deletingMap.Count > 0;
	}

	public void StopAllDownload()
	{
		if (_downloadMap == null || _downloadMap.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<int, DownloadResGroupCommonData> item in _downloadMap)
		{
			item.Value.Stop();
			GameEntry.Event.Fire(EventId.CommonResourceGroupDownloadStop, item.Key);
		}
		_downloadMap.Clear();
	}

	public DownloadResGroupCommonData GetLoadManifestData(int configId)
	{
		if (_loaderCache.TryGetValue(configId, out var value))
		{
			return value;
		}
		return null;
	}

	private void Update()
	{
		finishedList.Clear();
		foreach (KeyValuePair<int, DownloadResGroupCommonData> item in _downloadMap)
		{
			if (item.Value.CheckFinish())
			{
				GameEntry.Event.Fire(EventId.CommonResourceGroupDownloadFinish, item.Key);
				finishedList.Add(item.Key);
			}
		}
		for (int i = 0; i < finishedList.Count; i++)
		{
			_downloadMap.Remove(finishedList[i]);
		}
		if (deletingPackageConfigIdList.Count > 0)
		{
			int key = deletingPackageConfigIdList[deletingPackageConfigIdList.Count - 1];
			List<BundleInfo> list = deletingBundleListMap[key];
			int num = Math.Max(0, list.Count - maxDeleteBundleNumEachFrame);
			for (int num2 = list.Count - 1; num2 >= num; num2--)
			{
				BundleInfo bundleInfo = list[num2];
				bool flag = Bundle.IsBundleInCache(bundleInfo.name);
				if (ResourcePackageManager.IsDownloaded(bundleInfo) && !flag)
				{
					GameEntry.Resource.DeleteBundle(bundleInfo);
				}
				list.RemoveAt(num2);
			}
			if (list.Count <= 0)
			{
				deletingPackageConfigIdList.RemoveAt(deletingPackageConfigIdList.Count - 1);
				deletingBundleListMap.Remove(key);
				DownloadResGroupCommonData downloadResGroupCommonData = deletingMap[key];
				downloadResGroupCommonData.InvokeDeletedCompleted();
				deletingMap.Remove(key);
				ResourcePackageManager.ClearPackageDownloadedCache(downloadResGroupCommonData.packageId);
			}
		}
		if (_downloadMap.Count <= 0 && deletingPackageConfigIdList.Count <= 0)
		{
			if (_hasUpdate)
			{
				Updater.RemoveUpdateCallback(Update);
			}
			_hasUpdate = false;
		}
	}

	public static void SetClearedAllBundleCacheFlag()
	{
		GameEntry.Setting.SetPublicString("PlayerDownloadCenter_HasClearedAllBundleCache", "Str_True");
	}

	public static bool GetClearedAllBundleCacheFlag()
	{
		return GameEntry.Setting.GetPublicString("PlayerDownloadCenter_HasClearedAllBundleCache", "Str_False") == "Str_True";
	}

	public static void ResetClearedAllBundleCacheFlag()
	{
		GameEntry.Setting.SetPublicString("PlayerDownloadCenter_HasClearedAllBundleCache", "Str_False");
	}

	public static void AddWaitDeletePackageId(int packageId)
	{
		string waitDeletePackageIds = GetWaitDeletePackageIds();
		string[] array = waitDeletePackageIds.Split(new char[1] { '|' });
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == packageId.ToString())
			{
				return;
			}
		}
		waitDeletePackageIds = ((!string.IsNullOrEmpty(waitDeletePackageIds)) ? (waitDeletePackageIds + "|" + packageId) : packageId.ToString());
		GameEntry.Setting.PlayerPrefsSetString("PlayerDownloadCenter_WaitDeletePackageId", waitDeletePackageIds);
	}

	public static void RemoveWaitDeletePackageId(int packageId)
	{
		string waitDeletePackageIds = GetWaitDeletePackageIds();
		string value = string.Join("|", from s in waitDeletePackageIds.Split(new char[1] { '|' })
			where s != packageId.ToString()
			select s);
		GameEntry.Setting.PlayerPrefsSetString("PlayerDownloadCenter_WaitDeletePackageId", value);
	}

	public static string GetWaitDeletePackageIds()
	{
		return GameEntry.Setting.PlayerPrefsGetString("PlayerDownloadCenter_WaitDeletePackageId", "");
	}

	public static void SetPackageDeleteTimes(int packageId, int times)
	{
		GameEntry.Setting.PlayerPrefsSetInt("PlayerDownloadCenter_PackageDeleteTimes_" + packageId, times);
	}

	public static int GetPackageDeleteTimes(int packageId)
	{
		return GameEntry.Setting.PlayerPrefsGetInt("PlayerDownloadCenter_PackageDeleteTimes_" + packageId, 0);
	}

	public static void PrintLogError(string str)
	{
		Log.Error(str);
	}

	public static string GetVersionsSkipUpdateFlag()
	{
		return Versions.SkipUpdate.ToString();
	}
}
