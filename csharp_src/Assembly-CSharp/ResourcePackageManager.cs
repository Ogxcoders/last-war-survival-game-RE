using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameFramework;
using SFSLitJson;
using VEngine;
using XLua;

public class ResourcePackageManager
{
	private static HashSet<int> requiredPackageByCheckResVersionState = new HashSet<int>();

	private static HashSet<int> requiredPackageByInitMessage = new HashSet<int>();

	private static HashSet<int> requiredPackageByGetServerList = new HashSet<int>();

	private static PackageDownloadChecker _downloadChecker = null;

	private static HashSet<int> _packageIsDownloaded = new HashSet<int>();

	[BlackList]
	public static int[] GetRequiredPackages()
	{
		return GetRequiredPackagesHashSet().ToArray();
	}

	private static HashSet<int> GetRequiredPackagesHashSet()
	{
		HashSet<int> hashSet = new HashSet<int>();
		hashSet.UnionWith(requiredPackageByCheckResVersionState);
		if (!requiredPackageByCheckResVersionState.SetEquals(requiredPackageByInitMessage))
		{
			hashSet.UnionWith(requiredPackageByInitMessage);
		}
		if (!requiredPackageByCheckResVersionState.SetEquals(requiredPackageByGetServerList))
		{
			hashSet.UnionWith(requiredPackageByGetServerList);
		}
		Log.Info("[ResourcePackageManager] GetRequiredPackages1: " + string.Join(", ", requiredPackageByCheckResVersionState));
		Log.Info("[ResourcePackageManager] GetRequiredPackages2: " + string.Join(", ", requiredPackageByInitMessage));
		Log.Info("[ResourcePackageManager] GetRequiredPackages3: " + string.Join(", ", requiredPackageByGetServerList));
		Log.Info("[ResourcePackageManager] GetRequiredPackages4: " + string.Join(", ", hashSet));
		return hashSet;
	}

	[BlackList]
	public static void SetRequiredPackagesByCheckResVersionState(IDictionary dic, JsonData recData)
	{
		List<int> list = new List<int>();
		if (dic != null)
		{
			try
			{
				if (dic.Contains("seasonId"))
				{
					switch (((string)recData["seasonId"]).ToInt())
					{
					case 1:
						list.Add(1002);
						break;
					case 2:
						list.Add(1003);
						break;
					}
				}
				if (dic.Contains("seasonType"))
				{
					switch (((string)recData["seasonType"]).ToInt())
					{
					case 2:
						list.Add(1002);
						break;
					case 3:
						list.Add(1003);
						break;
					case 4:
						list.Add(1004);
						break;
					case 5:
						list.Add(1005);
						list.Add(2001);
						break;
					case 6:
						list.Add(1006);
						break;
					}
				}
			}
			catch (Exception arg)
			{
				Log.Info($"SeasonResourceLogic [SeasonResourceUtil] Init seasonType fail: {arg}");
			}
		}
		list.Add(90001);
		Log.Info("[ResourcePackageManager] SetRequiredPackagesByCheckResVersionState: " + string.Join(", ", list));
		SetRequiredPackages(requiredPackageByCheckResVersionState, list);
	}

	[BlackList]
	public static void SetRequiredPackagesByInitMessage(List<int> requiredPackages)
	{
		Log.Info("[ResourcePackageManager] SetRequiredPackagesByInitMessage: " + string.Join(", ", requiredPackages));
		SetRequiredPackages(requiredPackageByInitMessage, requiredPackages);
		if (!requiredPackageByGetServerList.SetEquals(requiredPackageByInitMessage))
		{
			string text = string.Join(", ", requiredPackageByGetServerList);
			string text2 = string.Join(", ", requiredPackageByInitMessage);
			Log.Error("ResourcePackageManager required package error, requiredPackageByGetServerList: " + text + ", ByInitMessage: " + text2);
		}
		AssetsStatistics.SetRequiredPackages(GetRequiredPackagesHashSet());
	}

	[BlackList]
	public static void SetRequiredPackagesByGetServerList(string strRequiredPackages)
	{
		Log.Info("[ResourcePackageManager] SetRequiredPackagesByGetServerList: " + strRequiredPackages);
		if (string.IsNullOrEmpty(strRequiredPackages))
		{
			requiredPackageByGetServerList.Clear();
			return;
		}
		string[] array = strRequiredPackages.Split(new char[1] { '|' });
		List<int> list = new List<int>();
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			if (int.TryParse(array[i], out var result))
			{
				list.Add(result);
			}
		}
		SetRequiredPackages(requiredPackageByGetServerList, list);
	}

	private static void SetRequiredPackages(HashSet<int> set, List<int> requiredPackages)
	{
		set.Clear();
		foreach (int requiredPackage in requiredPackages)
		{
			if (requiredPackage != 0 && requiredPackage != 1 && requiredPackage != 1000)
			{
				set.Add(requiredPackage);
				if (requiredPackage >= 1001 && requiredPackage < 1009)
				{
					set.Add(1000);
				}
			}
		}
	}

	public static void RemoveDownloadCompleteCallback()
	{
		try
		{
			Download.kCompleted = (Action<Download>)Delegate.Remove(Download.kCompleted, new Action<Download>(DownloadCompleted));
		}
		catch (Exception arg)
		{
			Log.Error($"PackageDownloadChecker RemoveDownloadCompleteCallback: {arg}");
		}
	}

	public static void SetupDownloadChecker()
	{
		try
		{
			_downloadChecker = new PackageDownloadChecker();
			_downloadChecker.Collect(Versions.DownloadDataPath);
			_packageIsDownloaded.Clear();
			Download.kCompleted = (Action<Download>)Delegate.Remove(Download.kCompleted, new Action<Download>(DownloadCompleted));
			Download.kCompleted = (Action<Download>)Delegate.Combine(Download.kCompleted, new Action<Download>(DownloadCompleted));
		}
		catch (Exception arg)
		{
			Log.Error($"PackageDownloadChecker SetupDownloadChecker: {arg}");
		}
	}

	private static void DownloadCompleted(Download obj)
	{
		_downloadChecker.AddDownload(obj);
	}

	public static bool IsPackageHasBundle(int packageId)
	{
		return Versions.IsPackageHasBundle(GameEntry.Resource.GameResManifestName, packageId);
	}

	public static bool IsPackageDownloaded(int packageId, IDownloadChecker downloadChecker = null)
	{
		if (_packageIsDownloaded.Contains(packageId))
		{
			return true;
		}
		string gameResManifestName = GameEntry.Resource.GameResManifestName;
		IDownloadChecker downloadChecker2;
		if (downloadChecker != null)
		{
			downloadChecker2 = downloadChecker;
		}
		else
		{
			IDownloadChecker downloadChecker3 = _downloadChecker;
			downloadChecker2 = downloadChecker3;
		}
		bool num = Versions.IsPackageDownloaded(gameResManifestName, packageId, downloadChecker2);
		if (num)
		{
			_packageIsDownloaded.Add(packageId);
		}
		return num;
	}

	public static ulong GetPackageTotalSize(int packageId)
	{
		Manifest manifest = Versions.GetManifest(GameEntry.Resource.GameResManifestName);
		return GameEntry.Resource.GetTotalSizeByPackage(manifest, packageId);
	}

	public static void ClearPackageDownloadedCache(int packageId)
	{
		if (_packageIsDownloaded.Contains(packageId))
		{
			_packageIsDownloaded.Remove(packageId);
		}
	}

	public static bool IsDownloaded(BundleInfo bundle)
	{
		if (_downloadChecker == null)
		{
			return false;
		}
		if (!bundle.isDownloaded)
		{
			if (!_downloadChecker.IsDownloaded(bundle))
			{
				return false;
			}
			bundle.isDownloaded = true;
		}
		return true;
	}

	public static void RemoveBundle(BundleInfo bundle)
	{
		_downloadChecker.RemoveBundle(bundle);
	}

	public static bool IsSeasonResDownloadedByPackageId(int packageId, IDownloadChecker downloadChecker = null)
	{
		if (packageId >= 1001 && packageId < 1009)
		{
			if (IsPackageDownloaded(packageId, downloadChecker))
			{
				return IsPackageDownloaded(1000, downloadChecker);
			}
			return false;
		}
		return false;
	}

	public static bool IsSeasonResDownloaded(int configId)
	{
		int num = GameEntry.ConfigCache.GetTemplateData("download_packs", configId, "pack_id").ToInt();
		if (num >= 1001 && num < 1009)
		{
			if (IsPackageDownloaded(num))
			{
				return IsPackageDownloaded(1000);
			}
			return false;
		}
		return false;
	}

	public static int[] GetRequiredPackagesWithoutLog()
	{
		HashSet<int> hashSet = new HashSet<int>();
		if (CommonUtils.IsDebug())
		{
			hashSet.UnionWith(requiredPackageByCheckResVersionState);
			if (!requiredPackageByCheckResVersionState.SetEquals(requiredPackageByInitMessage))
			{
				hashSet.UnionWith(requiredPackageByInitMessage);
			}
			if (!requiredPackageByCheckResVersionState.SetEquals(requiredPackageByGetServerList))
			{
				hashSet.UnionWith(requiredPackageByGetServerList);
			}
		}
		return hashSet.ToArray();
	}
}
