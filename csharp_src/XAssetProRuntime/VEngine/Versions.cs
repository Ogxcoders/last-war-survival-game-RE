using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FibMatrix;
using GameFramework;
using UnityEngine;

namespace VEngine;

public static class Versions
{
	public const string APIVersion = "6.1.5";

	public static readonly List<Manifest> Manifests = new List<Manifest>();

	private static readonly Dictionary<string, Manifest> NameWithManifests = new Dictionary<string, Manifest>();

	internal static readonly Dictionary<string, string> BundleWithPathOrUrLs = new Dictionary<string, string>();

	internal static readonly HashSet<string> BundleWithPathOrUrLsUseFlag = new HashSet<string>();

	internal static readonly List<BundleInfo> BundleWithPathOrUrLsUseList = new List<BundleInfo>();

	public static bool ENABLE_BUNDLE_USE_TRACK = false;

	private static readonly Dictionary<string, string> NameWithPaths = new Dictionary<string, string>();

	public static bool SkipUpdate;

	public static bool IsSimulation;

	public static bool CheckWhiteList;

	public static bool IsGMUser;

	public static Func<string, string> getDownloadURL;

	public static bool syncLoadPackageManifest;

	private static List<int> BUNDLE_USE_TRACK_COUNT_UP_DELTA = new List<int> { 1200, 1200, 800, 500 };

	private static int bundleUseTrackPlayerLv = -1;

	private static int bundleUseTrackPlayerServerId = -1;

	private static HashSet<string> _playerAssetsHash;

	public static readonly List<string> WhiteListFailed = new List<string>();

	public static bool UseBundlePackageID = true;

	public static bool BundleFastValidation = true;

	private static HashSet<string> assetDownloaded = new HashSet<string>();

	private static HashSet<string> bundleDownloaded = new HashSet<string>();

	private static Dictionary<string, HashSet<string>> bundleAssetDownloaded = new Dictionary<string, HashSet<string>>();

	public static bool UseBundleDownloadedCache = false;

	public static bool ENABLE_MANIFEST_DOWNGRADE = true;

	private static readonly List<string> _errorBundles = new List<string>();

	private static readonly string _errorBundleFilePath = Path.Combine(Application.persistentDataPath, "error.bundles");

	public static Func<string, Type, Asset> FuncCreateAsset { get; set; }

	public static Func<string, bool, Scene> FuncCreateScene { get; set; }

	public static Func<string, bool, ManifestFile> FuncCreateManifest { get; set; }

	public static Func<string, bool> FuncIsAssetDownloaded { get; set; }

	public static string ManifestsVersion
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < Manifests.Count; i++)
			{
				Manifest manifest = Manifests[i];
				stringBuilder.Append(manifest.version);
				if (i < Manifests.Count - 1)
				{
					stringBuilder.Append(".");
				}
			}
			return stringBuilder.ToString();
		}
	}

	public static string PlayerDataPath { get; set; }

	public static string DownloadURL { get; set; }

	public static string CheckVersionURL { get; set; }

	public static string DownloadDataPath { get; set; }

	public static string WarmupDataPath { get; private set; }

	public static string LocalProtocol { get; set; }

	public static string PlatformName { get; set; }

	public static Func<string, string> customLoadPath { get; set; }

	private static List<string> WhiteList { get; set; }

	public static bool CanDowngrade => ENABLE_MANIFEST_DOWNGRADE;

	public static Manifest GetManifest(string name)
	{
		if (NameWithManifests.TryGetValue(name, out var value))
		{
			return value;
		}
		return null;
	}

	public static Asset CreateAsset(string path, Type type)
	{
		if (string.IsNullOrEmpty(path))
		{
			throw new ArgumentException("path");
		}
		AssetsStatistics.OnAssetsLoaded(path, type);
		return FuncCreateAsset(path, type);
	}

	public static bool IsAssetDownloaded(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			throw new ArgumentException("path");
		}
		return FuncIsAssetDownloaded(path);
	}

	public static Scene CreateScene(string path, bool additive)
	{
		if (string.IsNullOrEmpty(path))
		{
			throw new ArgumentException("path");
		}
		GetActualPath(ref path);
		return FuncCreateScene(path, additive);
	}

	public static ManifestFile CreateManifest(string name, bool builtin)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentException("name");
		}
		return FuncCreateManifest(name.ToLower(), builtin);
	}

	public static void OnReadAsset(string assetPath)
	{
		if (customLoadPath == null)
		{
			return;
		}
		string text = CustomLoadPath(assetPath);
		if (!string.IsNullOrEmpty(text))
		{
			if (!NameWithPaths.TryGetValue(text, out var value))
			{
				NameWithPaths[text] = assetPath;
			}
			else if (!value.Equals(assetPath))
			{
				Logger.W(text + " already exist " + value);
			}
		}
	}

	public static void Override(Manifest target)
	{
		string name = target.name;
		if (NameWithManifests.TryGetValue(name, out var value))
		{
			if (CanDowngrade || value.version < target.version)
			{
				target.id = value.id;
				NameWithManifests[name] = target;
				Manifests[value.id] = target;
			}
		}
		else
		{
			target.id = Manifests.Count;
			Manifests.Add(target);
			NameWithManifests.Add(name, target);
		}
	}

	public static void GetActualPath(ref string path)
	{
		if (NameWithPaths.TryGetValue(path, out var value))
		{
			path = value;
		}
	}

	private static string CustomLoadPath(string assetPath)
	{
		return customLoadPath(assetPath);
	}

	public static string GetDownloadDataPath(string file)
	{
		return DownloadDataPath + "/" + file;
	}

	public static string GetDownloadDataSystemPath(string file)
	{
		return Path.Combine(DownloadDataPath, file);
	}

	public static string GetPlayerDataURL(string file)
	{
		return LocalProtocol + PlayerDataPath + "/" + file;
	}

	public static string GetPlayerDataPath(string file)
	{
		return PlayerDataPath + "/" + file;
	}

	public static string GetDownloadURL(string file)
	{
		if (getDownloadURL != null)
		{
			string text = getDownloadURL(file);
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
		}
		return DownloadURL + PlatformName + "/" + file;
	}

	public static string GetTemporaryPath(string file)
	{
		string text = Application.temporaryCachePath + "/" + file;
		string directoryName = Path.GetDirectoryName(text);
		if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
		return text;
	}

	public static void ClearDownloadData()
	{
		if (Directory.Exists(DownloadDataPath))
		{
			Directory.Delete(DownloadDataPath, recursive: true);
			Directory.CreateDirectory(DownloadDataPath);
		}
		BundleWithPathOrUrLs.Clear();
	}

	public static void InitializeOnLoad()
	{
		if (FuncCreateAsset == null)
		{
			FuncCreateAsset = BundledAsset.Create;
		}
		if (FuncIsAssetDownloaded == null)
		{
			FuncIsAssetDownloaded = BundledAsset.IsAssetDownloaded;
		}
		if (FuncCreateScene == null)
		{
			FuncCreateScene = BundledScene.Create;
		}
		if (FuncCreateManifest == null)
		{
			FuncCreateManifest = ManifestFile.Create;
		}
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.OSXPlayer && Application.platform != RuntimePlatform.IPhonePlayer)
		{
			if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
			{
				LocalProtocol = "file:///";
			}
			else
			{
				LocalProtocol = string.Empty;
			}
		}
		else
		{
			LocalProtocol = "file://";
		}
		if (string.IsNullOrEmpty(PlatformName))
		{
			PlatformName = Utility.GetPlatformName();
		}
		if (string.IsNullOrEmpty(PlayerDataPath))
		{
			PlayerDataPath = Application.streamingAssetsPath + "/AssetBundles";
		}
		if (string.IsNullOrEmpty(DownloadDataPath))
		{
			DownloadDataPath = Application.persistentDataPath + "/AssetBundles";
		}
		if (!Directory.Exists(DownloadDataPath))
		{
			Directory.CreateDirectory(DownloadDataPath);
		}
		if (string.IsNullOrEmpty(WarmupDataPath))
		{
			WarmupDataPath = Application.persistentDataPath + "/warmup";
		}
		if (!Directory.Exists(WarmupDataPath))
		{
			Directory.CreateDirectory(WarmupDataPath);
		}
	}

	public static InitializeVersions InitializeAsync()
	{
		Log.Info("InitializeAsync");
		PlayerSettings playerSettings = Resources.Load<PlayerSettings>("PlayerSettings");
		if (playerSettings == null)
		{
			playerSettings = ScriptableObject.CreateInstance<PlayerSettings>();
		}
		List<string> assets = playerSettings.assets;
		_playerAssetsHash = new HashSet<string>();
		foreach (string item in assets)
		{
			_playerAssetsHash.Add(item);
		}
		assetDownloaded.Clear();
		bundleDownloaded.Clear();
		bundleAssetDownloaded.Clear();
		WhiteList = playerSettings.whiteList;
		if (CommonUtils.CheckIsOverridePackage())
		{
			Log.Info("[OverlayInstall] isOverride");
			CommonUtils.DeleteCache(cacheBundle: true);
			CommonUtils.WriteVersion();
		}
		InitializeOnLoad();
		InitializeVersions initializeVersions = new InitializeVersions();
		initializeVersions.manifests = ((!Debug.isDebugBuild) ? playerSettings.manifests.ToArray() : new string[2] { "GameRes", "DllRes" });
		initializeVersions.bkgroundManifest = playerSettings.bkgroundManifest;
		initializeVersions.packageResManifest = playerSettings.packageResManifest;
		return initializeVersions;
	}

	public static void DeleteManifest()
	{
		PlayerSettings playerSettings = Resources.Load<PlayerSettings>("PlayerSettings");
		if (playerSettings == null)
		{
			playerSettings = ScriptableObject.CreateInstance<PlayerSettings>();
		}
		string text = string.Join("-", playerSettings.manifests);
		Debug.Log("Versions::DeleteManifest " + text);
		foreach (string manifest in playerSettings.manifests)
		{
			string text2 = manifest.ToLower();
			string text3 = Path.Combine(Application.persistentDataPath, "AssetBundles", text2);
			Debug.Log("Versions::DeleteManifest check " + text3);
			if (File.Exists(text3))
			{
				File.Delete(text3);
			}
			Debug.Log("Versions::DeleteManifest delete " + text3);
			string text4 = Path.Combine(Application.persistentDataPath, "AssetBundles", Manifest.GetVersionFile(text2));
			Debug.Log("Versions::DeleteManifest check " + text4);
			if (File.Exists(text4))
			{
				File.Delete(text4);
			}
			Debug.Log("Versions::DeleteManifest delete " + text4);
		}
	}

	public static void DeleteBundle(BundleInfo bundle)
	{
		if (!bundle.isSplitBundle)
		{
			return;
		}
		string path = Path.Combine(DownloadDataPath, bundle.name);
		if (File.Exists(path))
		{
			File.Delete(path);
			ClearBundleDownloaded(bundle);
			return;
		}
		path = Path.Combine(DownloadDataPath, bundle.alias);
		if (File.Exists(path))
		{
			File.Delete(path);
			ClearBundleDownloaded(bundle);
		}
	}

	public static UpdateVersions UpdateAsync(params string[] manifests)
	{
		UpdateVersions updateVersions = new UpdateVersions();
		updateVersions.manifests = manifests;
		updateVersions.Start();
		return updateVersions;
	}

	public static DownloadVersions DownloadAsync(DownloadInfo[] groups, int queueID = 0)
	{
		DownloadVersions downloadVersions = new DownloadVersions();
		downloadVersions.groups = groups;
		downloadVersions.queueID = queueID;
		downloadVersions.Start();
		return downloadVersions;
	}

	public static void SyncInPlayerAssets(BundleInfo bundle)
	{
		if (_playerAssetsHash != null)
		{
			bundle.inPlayerAssets = _playerAssetsHash.Contains(bundle.name);
		}
	}

	public static bool IsDownloaded(BundleInfo bundle)
	{
		if (SkipUpdate || _playerAssetsHash.Contains(bundle.name))
		{
			return true;
		}
		FileInfo fileInfo = new FileInfo(GetDownloadDataPath(bundle.name));
		if (fileInfo.Exists && fileInfo.Length == (long)bundle.size)
		{
			return true;
		}
		FileInfo fileInfo2 = new FileInfo(GetDownloadDataPath(bundle.alias));
		if (fileInfo2.Exists)
		{
			return fileInfo2.Length == (long)bundle.size;
		}
		return false;
	}

	public static bool IsOriginDownloaded(BundleInfo bundle)
	{
		if (SkipUpdate || _playerAssetsHash.Contains(bundle.name))
		{
			return true;
		}
		FileInfo fileInfo = new FileInfo(GetDownloadDataPath(bundle.name));
		if (fileInfo.Exists)
		{
			return fileInfo.Length == (long)bundle.size;
		}
		return false;
	}

	public static bool IsAliasDownloaded(BundleInfo bundle)
	{
		if (SkipUpdate || _playerAssetsHash.Contains(bundle.name))
		{
			return true;
		}
		FileInfo fileInfo = new FileInfo(GetDownloadDataPath(bundle.alias));
		if (fileInfo.Exists)
		{
			return fileInfo.Length == (long)bundle.size;
		}
		return false;
	}

	public static bool IsRemote(BundleInfo bundle)
	{
		if (bundle.isUnusedAssetsPackage && !IsGMUser)
		{
			Log.Info("Unused Bundle: " + bundle.name + " Loaded!");
		}
		return bundle.isSplitBundle;
	}

	public static bool IsChanged(string manifest)
	{
		ManifestVersionFile manifestVersionFile = ManifestVersionFile.Load(GetDownloadDataPath(Manifest.GetVersionFile(manifest)));
		Manifest manifest2 = Manifests.Find((Manifest m) => manifest.Contains(m.name));
		if (manifest2 != null)
		{
			if (!CanDowngrade)
			{
				return manifest2.version < manifestVersionFile.version;
			}
			return true;
		}
		return true;
	}

	public static bool IsInWhiteList(string bundleName)
	{
		return WhiteList.Contains(bundleName);
	}

	internal static void SetBundlePathOrURl(string assetBundleName, string url)
	{
		BundleWithPathOrUrLs[assetBundleName] = url;
	}

	internal static string GetBundlePathOrURL(BundleInfo info, bool isSyncLoad = true)
	{
		string name = info.name;
		if (BundleWithPathOrUrLs.TryGetValue(name, out var value))
		{
			CheckBundleUseToTrack(info);
			return value;
		}
		if (_playerAssetsHash.Contains(name))
		{
			value = GetPlayerDataPath(name);
			BundleWithPathOrUrLs[name] = value;
			CheckBundleUseToTrack(info);
			return value;
		}
		if (IsOriginDownloaded(info))
		{
			value = GetDownloadDataPath(name);
			BundleWithPathOrUrLs[name] = value;
			CheckBundleUseToTrack(info);
			return value;
		}
		if (IsAliasDownloaded(info))
		{
			value = GetDownloadDataPath(info.alias);
			BundleWithPathOrUrLs[name] = value;
			CheckBundleUseToTrack(info);
			return value;
		}
		if (!isSyncLoad)
		{
			if (IsRemote(info))
			{
				CheckBundleUseToTrack(info);
				return GetDownloadURL(name);
			}
			Log.Error("bundle {0} not find in local return path {1}", name, value);
		}
		else
		{
			Log.Error("bundle " + info.name + " is Load by sync function, can`t use remote load");
		}
		return GetDownloadDataPath(name);
	}

	public static void SyncGameLogicInfo(string paramStr)
	{
		if (!string.IsNullOrEmpty(paramStr))
		{
			string[] array = paramStr.Split(new char[1] { ';' });
			if (array.Length >= 1 && !string.IsNullOrEmpty(array[0]) && int.TryParse(array[0], out var result) && result > bundleUseTrackPlayerLv)
			{
				CheckBundleUseToTrackByLevelUp(result);
				bundleUseTrackPlayerLv = result;
			}
			if (array.Length >= 2 && !string.IsNullOrEmpty(array[1]) && int.TryParse(array[1], out var result2))
			{
				bundleUseTrackPlayerServerId = result2;
			}
			AssetsStatistics.SyncGameLogicInfo(paramStr);
		}
	}

	public static void ClearGameLogicInfo()
	{
		BundleWithPathOrUrLsUseFlag.Clear();
		BundleWithPathOrUrLsUseList.Clear();
		bundleUseTrackPlayerLv = -1;
		bundleUseTrackPlayerServerId = -1;
		AssetsStatistics.ClearGameLogicInfo();
	}

	public static void CheckBundleUseToTrack(BundleInfo info)
	{
		if (!ENABLE_BUNDLE_USE_TRACK || !BundleWithPathOrUrLsUseFlag.Add(info.name))
		{
			return;
		}
		BundleWithPathOrUrLsUseList.Add(info);
		int num = BUNDLE_USE_TRACK_COUNT_UP_DELTA[0];
		if (BundleWithPathOrUrLsUseFlag.Count > num)
		{
			TrackBundleUseInfo(bundleUseTrackPlayerLv, bundleUseTrackPlayerServerId);
			BUNDLE_USE_TRACK_COUNT_UP_DELTA[0] += BUNDLE_USE_TRACK_COUNT_UP_DELTA[2];
			BUNDLE_USE_TRACK_COUNT_UP_DELTA[1] = BUNDLE_USE_TRACK_COUNT_UP_DELTA[2];
			if (BUNDLE_USE_TRACK_COUNT_UP_DELTA.Count > 3)
			{
				BUNDLE_USE_TRACK_COUNT_UP_DELTA.RemoveAt(2);
			}
		}
	}

	public static void CheckBundleUseToTrackByLevelUp(int level)
	{
		TrackBundleUseInfo(level, bundleUseTrackPlayerServerId);
		BUNDLE_USE_TRACK_COUNT_UP_DELTA[0] = BundleWithPathOrUrLsUseFlag.Count;
	}

	private static void TrackBundleUseInfo(int level, int serverId = 0)
	{
		int num = GetManifest("gameres")?.version ?? 0;
		string downloadURL = GetDownloadURL(string.Format("gameres{0}_v{1}", "_small", num));
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(downloadURL);
		stringBuilder.Append("|");
		stringBuilder.Append(level);
		stringBuilder.Append("|");
		stringBuilder.Append(serverId);
		int num2 = BundleWithPathOrUrLsUseList.Count - 1;
		int num3 = BundleWithPathOrUrLsUseList.Count - BUNDLE_USE_TRACK_COUNT_UP_DELTA[1];
		if (num2 >= 0 && num3 >= 0)
		{
			for (int num4 = num2; num4 >= num3; num4--)
			{
				BundleInfo bundleInfo = BundleWithPathOrUrLsUseList[num4];
				stringBuilder.Append("|");
				stringBuilder.Append(bundleInfo.id);
			}
			string text = StringArrayCompressor.Compress(stringBuilder.ToString());
			Log.Info("BundleUseIndex:" + text);
		}
	}

	public static AssetInfo GetAsset(ref string path)
	{
		GetActualPath(ref path);
		foreach (KeyValuePair<string, Manifest> nameWithManifest in NameWithManifests)
		{
			AssetInfo asset = nameWithManifest.Value.GetAsset(path);
			if (asset != null)
			{
				return asset;
			}
		}
		return null;
	}

	public static bool GetDependencies(string assetPath, out BundleInfo bundle, out BundleInfo[] bundles)
	{
		for (int num = Manifests.Count - 1; num >= 0; num--)
		{
			Manifest manifest = Manifests[num];
			AssetInfo asset = manifest.GetAsset(assetPath);
			if (asset != null)
			{
				bundle = manifest.GetBundle(asset.bundle);
				bundles = manifest.GetDependencies(bundle);
				return true;
			}
		}
		bundle = null;
		bundles = null;
		return false;
	}

	public static bool GetDependencies(string assetPath, out BundleInfo bundle, out BundleInfo[] bundles, out int assetId)
	{
		for (int num = Manifests.Count - 1; num >= 0; num--)
		{
			Manifest manifest = Manifests[num];
			AssetInfo asset = manifest.GetAsset(assetPath);
			if (asset != null)
			{
				bundle = manifest.GetBundle(asset.bundle);
				bundles = manifest.GetDependencies(bundle);
				assetId = asset.id;
				return true;
			}
		}
		bundle = null;
		bundles = null;
		assetId = 0;
		return false;
	}

	public static List<BundleInfo> GetBundlesWithAssets(Manifest[] manifests, string[] assetNames)
	{
		List<BundleInfo> list = new List<BundleInfo>();
		if (manifests != null)
		{
			foreach (Manifest manifest in manifests)
			{
				foreach (string path in assetNames)
				{
					AssetInfo asset = manifest.GetAsset(path);
					BundleInfo[] bundles = manifest.GetBundles(asset);
					foreach (BundleInfo bundleInfo in bundles)
					{
						if (!bundleInfo.inPlayerAssets)
						{
							list.Add(bundleInfo);
						}
					}
				}
			}
		}
		return list;
	}

	public static List<BundleInfo> GetBundlesWithGroups(Manifest[] manifests, string[] groupsNames)
	{
		List<BundleInfo> list = new List<BundleInfo>();
		if (manifests != null)
		{
			for (int i = 0; i < manifests.Length; i++)
			{
				foreach (BundleInfo bundlesWithGroup in manifests[i].GetBundlesWithGroups(groupsNames))
				{
					if (!bundlesWithGroup.inPlayerAssets)
					{
						list.Add(bundlesWithGroup);
					}
				}
			}
		}
		return list;
	}

	public static List<BundleInfo> GetBundlesWithPackages(Manifest[] manifests, int packageId)
	{
		List<BundleInfo> list = new List<BundleInfo>();
		if (manifests != null)
		{
			for (int i = 0; i < manifests.Length; i++)
			{
				foreach (BundleInfo item in manifests[i].GetBundlesWithPackageID(packageId))
				{
					if (!item.inPlayerAssets)
					{
						list.Add(item);
					}
				}
			}
		}
		return list;
	}

	public static List<BundleInfo> GetBundlesWithGroupsWithoutCheckPlayerAssets(Manifest[] manifests, string[] groupsNames)
	{
		List<BundleInfo> list = new List<BundleInfo>();
		if (manifests != null)
		{
			for (int i = 0; i < manifests.Length; i++)
			{
				foreach (BundleInfo bundlesWithGroup in manifests[i].GetBundlesWithGroups(groupsNames))
				{
					list.Add(bundlesWithGroup);
				}
			}
		}
		return list;
	}

	public static string[] GetAllAssetPaths()
	{
		HashSet<string> hashSet = new HashSet<string>();
		foreach (Manifest manifest in Manifests)
		{
			hashSet.UnionWith(manifest.AllAssetPaths);
		}
		return hashSet.ToArray();
	}

	public static bool IsPackageDownloaded(string manifestName, int packageId, IDownloadChecker downloadChecker = null)
	{
		Manifest manifest = GetManifest(manifestName);
		if (manifest == null)
		{
			return false;
		}
		Func<BundleInfo, bool> func = null;
		func = ((downloadChecker == null) ? new Func<BundleInfo, bool>(IsDownloaded) : new Func<BundleInfo, bool>(downloadChecker.IsDownloaded));
		foreach (BundleInfo item in manifest.GetBundlesWithPackageID(packageId))
		{
			if (!item.isDownloaded)
			{
				if (!func(item))
				{
					return false;
				}
				item.isDownloaded = true;
			}
		}
		return true;
	}

	public static bool IsPackageHasBundle(string manifestName, int packageId)
	{
		Manifest manifest = GetManifest(manifestName);
		if (manifest == null)
		{
			return false;
		}
		return manifest.GetBundlesWithPackageID(packageId).Count > 0;
	}

	public static void ClearBundleDownloaded(BundleInfo info)
	{
		string name = info.name;
		if (BundleWithPathOrUrLs.TryGetValue(name, out var _))
		{
			BundleWithPathOrUrLs.Remove(name);
		}
		if (bundleDownloaded.Contains(name))
		{
			bundleDownloaded.Remove(name);
		}
		if (bundleAssetDownloaded == null || !bundleAssetDownloaded.ContainsKey(name))
		{
			return;
		}
		foreach (string item in bundleAssetDownloaded[name])
		{
			if (assetDownloaded.Contains(item))
			{
				assetDownloaded.Remove(item);
			}
		}
		bundleAssetDownloaded.Remove(name);
	}

	public static bool IsAssetDownloaded_InCache(string prefabPath)
	{
		if (assetDownloaded.Contains(prefabPath))
		{
			return true;
		}
		if (!GetDependencies(prefabPath, out var bundle, out var bundles))
		{
			return false;
		}
		if (!IsBundleDownloaded(bundle) || !IsDownloadBundleLoaded(bundle))
		{
			return false;
		}
		BundleInfo[] array = bundles;
		foreach (BundleInfo bundleInfo in array)
		{
			if (!IsBundleDownloaded(bundleInfo) || !IsDownloadBundleLoaded(bundleInfo))
			{
				return false;
			}
		}
		if (!bundleAssetDownloaded.ContainsKey(bundle.name))
		{
			HashSet<string> value = new HashSet<string>();
			bundleAssetDownloaded.Add(bundle.name, value);
		}
		bundleAssetDownloaded[bundle.name].Add(prefabPath);
		assetDownloaded.Add(prefabPath);
		return true;
	}

	internal static bool IsBundleDownloaded(BundleInfo info)
	{
		string name = info.name;
		if (BundleWithPathOrUrLs.TryGetValue(name, out var _))
		{
			return true;
		}
		if (bundleDownloaded.Contains(name))
		{
			return true;
		}
		if (_playerAssetsHash.Contains(name))
		{
			bundleDownloaded.Add(name);
			return true;
		}
		if (IsOriginDownloaded(info))
		{
			bundleDownloaded.Add(name);
			return true;
		}
		if (IsAliasDownloaded(info))
		{
			bundleDownloaded.Add(name);
			return true;
		}
		return false;
	}

	internal static bool IsDownloadBundleLoaded(BundleInfo bundleInfo)
	{
		if (Bundle.Cache.TryGetValue(bundleInfo.name, out var value) && value.type == BundleType.Download)
		{
			return value.isDone;
		}
		return true;
	}

	public static void ClearCache()
	{
		BundleWithPathOrUrLs.Clear();
		assetDownloaded.Clear();
		bundleDownloaded.Clear();
		bundleAssetDownloaded.Clear();
	}

	public static void ProcessErrorBundleFile()
	{
		_errorBundles.Clear();
		if (File.Exists(_errorBundleFilePath))
		{
			try
			{
				string[] array = File.ReadAllLines(_errorBundleFilePath);
				foreach (string text in array)
				{
					if (!string.IsNullOrWhiteSpace(text))
					{
						_errorBundles.Add(text.Trim());
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error($"Failed to read {_errorBundleFilePath}: {arg}");
				return;
			}
		}
		if (_errorBundles.Count == 0)
		{
			return;
		}
		List<string> list = new List<string>();
		foreach (string errorBundle in _errorBundles)
		{
			try
			{
				Log.Error("Versions.ErrorBundle delete " + errorBundle);
				if (File.Exists(errorBundle))
				{
					File.Delete(errorBundle);
				}
				if (File.Exists(errorBundle))
				{
					list.Add(errorBundle);
				}
			}
			catch (Exception message)
			{
				Log.Error(message);
				list.Add(errorBundle);
			}
		}
		_errorBundles.Clear();
		_errorBundles.AddRange(list);
		try
		{
			if (_errorBundles.Count > 0)
			{
				File.WriteAllLines(_errorBundleFilePath, _errorBundles);
			}
			else if (File.Exists(_errorBundleFilePath))
			{
				File.Delete(_errorBundleFilePath);
			}
		}
		catch (Exception arg2)
		{
			Log.Error($"Failed writing error bundle file: {arg2}");
		}
	}

	public static void AddErrorBundle(string bundlePath)
	{
		try
		{
			_errorBundles.Add(bundlePath);
			File.WriteAllLines(_errorBundleFilePath, _errorBundles);
		}
		catch (Exception message)
		{
			Log.Error(message);
		}
	}
}
