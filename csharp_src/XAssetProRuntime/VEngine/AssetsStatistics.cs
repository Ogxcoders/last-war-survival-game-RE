using System;
using System.Collections.Generic;
using System.Text;
using FibMatrix;
using GameFramework;
using UnityEngine;
using UnityEngine.Networking;

namespace VEngine;

public static class AssetsStatistics
{
	[Serializable]
	public class AssetsStatisticsData
	{
		public string packver;

		public int[] assetsList;

		public string uid;

		public int server;

		public int level;
	}

	internal static readonly HashSet<int> assetsUseFlag = new HashSet<int>();

	internal static readonly List<AssetInfo> assetsUseList = new List<AssetInfo>();

	private static readonly HashSet<int> asyncAssetsUseFlag = new HashSet<int>();

	private static readonly List<int> asyncAssetsUseList = new List<int>(32);

	public static bool ENABLE_ASSETS_USE_TRACK = true;

	public static bool ENABLE_BUNDLE_LOAD_TRACK = true;

	private static int bundleUseTrackPlayerLv = -1;

	private static int bundleUseTrackPlayerServerId = -1;

	private static string bundleUseTrackPlayerUid = "";

	private static string bundleUseTrackPackVer = "";

	private static List<int> ASSETS_USE_TRACK_COUNT_UP_DELTA = new List<int> { 800, 800, 500, 300 };

	private const int MaxBatchSize = 30;

	private const float MinInterval = 10f;

	private const float MaxInterval = 30f;

	private static float lastTs = -1f;

	private static float interval;

	private static HashSet<int> requiredPackages;

	private static int lastTrackIndex = 0;

	private static string SERVER_URL = "https://lastwar-client-res-counters.lastwar.com";

	private static string SERVER_URL_TOKEN = "9f41e5e6a7d845d0a4b6f6c1229d2f37";

	private static float Interval
	{
		get
		{
			if (interval == 0f)
			{
				interval = UnityEngine.Random.Range(10f, 30f);
			}
			return interval;
		}
	}

	public static void SyncGameLogicInfo(string paramStr)
	{
		if (!string.IsNullOrEmpty(paramStr))
		{
			string[] array = paramStr.Split(new char[1] { ';' });
			if (array.Length >= 1 && !string.IsNullOrEmpty(array[0]) && int.TryParse(array[0], out var result) && result > bundleUseTrackPlayerLv)
			{
				CheckAssetsUseToTrackByLevelUp(result);
				CheckBundleUseToTrackByLevelUp(result);
				bundleUseTrackPlayerLv = result;
			}
			if (array.Length >= 2 && !string.IsNullOrEmpty(array[1]) && int.TryParse(array[1], out var result2))
			{
				bundleUseTrackPlayerServerId = result2;
			}
			if (array.Length >= 3 && !string.IsNullOrEmpty(array[2]))
			{
				bundleUseTrackPlayerUid = array[2];
			}
			if (array.Length >= 4 && !string.IsNullOrEmpty(array[3]))
			{
				bundleUseTrackPackVer = array[3];
			}
		}
	}

	public static void ClearGameLogicInfo()
	{
		assetsUseFlag.Clear();
		assetsUseList.Clear();
		asyncAssetsUseFlag.Clear();
		asyncAssetsUseList.Clear();
		bundleUseTrackPlayerLv = -1;
		bundleUseTrackPlayerServerId = -1;
		bundleUseTrackPlayerUid = "";
		bundleUseTrackPackVer = "";
	}

	public static void OnAssetsLoaded(string path, Type type)
	{
		if (ENABLE_ASSETS_USE_TRACK)
		{
			AssetInfo asset = Versions.GetAsset(ref path);
			if (asset != null && assetsUseFlag.Add(asset.id))
			{
				assetsUseList.Add(asset);
				TryTrackAssetsUseInfo(bundleUseTrackPlayerLv, bundleUseTrackPlayerServerId);
			}
		}
	}

	private static void TryTrackAssetsUseInfo(int level, int server)
	{
		int num = ASSETS_USE_TRACK_COUNT_UP_DELTA[0];
		if (assetsUseFlag.Count >= num)
		{
			TrackAssetsUseInfo(level, server);
			ASSETS_USE_TRACK_COUNT_UP_DELTA[0] += ASSETS_USE_TRACK_COUNT_UP_DELTA[2];
			ASSETS_USE_TRACK_COUNT_UP_DELTA[1] = ASSETS_USE_TRACK_COUNT_UP_DELTA[2];
			if (ASSETS_USE_TRACK_COUNT_UP_DELTA.Count > 3)
			{
				ASSETS_USE_TRACK_COUNT_UP_DELTA.RemoveAt(2);
			}
		}
	}

	public static void CheckAssetsUseToTrackByLevelUp(int level)
	{
		ASSETS_USE_TRACK_COUNT_UP_DELTA[0] = assetsUseFlag.Count;
		if (assetsUseFlag.Count - lastTrackIndex > 1)
		{
			ASSETS_USE_TRACK_COUNT_UP_DELTA[1] = assetsUseFlag.Count - lastTrackIndex;
		}
		TryTrackAssetsUseInfo(level, bundleUseTrackPlayerServerId);
	}

	public static void SyncInfoToServer(string packver, string uid, int[] assetsList, int level, int server)
	{
		if (!(Application.identifier != "com.fun.lastwar.gp"))
		{
			string s = JsonUtility.ToJson(new AssetsStatisticsData
			{
				packver = packver,
				assetsList = assetsList,
				uid = uid,
				level = level,
				server = server
			});
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			UnityWebRequest unityWebRequest = new UnityWebRequest(SERVER_URL + "/ingest", "POST");
			unityWebRequest.uploadHandler = new UploadHandlerRaw(bytes);
			unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
			unityWebRequest.SetRequestHeader("Content-Type", "application/json");
			unityWebRequest.SetRequestHeader("x-api-key", SERVER_URL_TOKEN);
			unityWebRequest.SendWebRequest();
		}
	}

	public static void TrackAssetsUseInfo(int level, int serverId = 0)
	{
		int num = Versions.GetManifest("gameres")?.version ?? 0;
		string downloadURL = Versions.GetDownloadURL(string.Format("gameres{0}_v{1}", "_small", num));
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(downloadURL);
		stringBuilder.Append("|");
		stringBuilder.Append(level);
		stringBuilder.Append("|");
		stringBuilder.Append(serverId);
		int num2 = assetsUseList.Count - 1;
		int num3 = assetsUseList.Count - ASSETS_USE_TRACK_COUNT_UP_DELTA[1];
		if (num2 >= 0 && num3 >= 0)
		{
			lastTrackIndex = num2;
			int[] array = new int[num2 - num3 + 1];
			int num4 = 0;
			for (int num5 = num2; num5 >= num3; num5--)
			{
				AssetInfo assetInfo = assetsUseList[num5];
				stringBuilder.Append("|");
				stringBuilder.Append(assetInfo.id);
				array[num4++] = assetInfo.id;
			}
			SyncInfoToServer("1.0." + num, bundleUseTrackPlayerUid, array, level, serverId);
			string text = StringArrayCompressor.Compress(stringBuilder.ToString());
			Log.Info("AssetsUseIndex:" + text);
		}
	}

	public static void OnBundleLoaded(BundleInfo info, int assetId)
	{
		if (ENABLE_BUNDLE_LOAD_TRACK && info != null)
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (lastTs < 0f)
			{
				lastTs = realtimeSinceStartup;
			}
			if (!TryCheckPackageExclude(info.downloadModes) && asyncAssetsUseFlag.Add(assetId))
			{
				asyncAssetsUseList.Add(assetId);
			}
			bool num = realtimeSinceStartup - lastTs >= Interval;
			bool flag = asyncAssetsUseList.Count >= 30;
			if ((num || flag) && asyncAssetsUseList.Count > 0)
			{
				lastTs = realtimeSinceStartup;
				TryTrackBundleUseInfo(bundleUseTrackPlayerLv, bundleUseTrackPlayerServerId);
			}
		}
	}

	private static bool TryCheckPackageExclude(int[] packages)
	{
		for (int i = 0; i < packages.Length; i++)
		{
			if (requiredPackages != null && requiredPackages.Contains(packages[i]))
			{
				return true;
			}
			if (packages[i] == 1)
			{
				return true;
			}
		}
		return false;
	}

	private static void TryTrackBundleUseInfo(int level, int server)
	{
		TrackBundleUseInfo(level, server);
	}

	public static void CheckBundleUseToTrackByLevelUp(int level)
	{
		TryTrackBundleUseInfo(level, bundleUseTrackPlayerServerId);
	}

	private static void TrackBundleUseInfo(int level, int serverId = 0)
	{
		int num = Versions.GetManifest("gameres")?.version ?? 0;
		string downloadURL = Versions.GetDownloadURL(string.Format("gameres{0}_v{1}", "_small", num));
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("1.1." + num);
		stringBuilder.Append("|");
		stringBuilder.Append(downloadURL);
		stringBuilder.Append("|");
		stringBuilder.Append(level);
		stringBuilder.Append("|");
		stringBuilder.Append(serverId);
		int count = asyncAssetsUseList.Count;
		if (count > 0)
		{
			int[] array = new int[count];
			int num2 = 0;
			for (int i = 0; i < count; i++)
			{
				int num3 = asyncAssetsUseList[i];
				stringBuilder.Append("|");
				stringBuilder.Append(num3);
				array[num2++] = num3;
			}
			SyncInfoToServer("1.1." + num, bundleUseTrackPlayerUid, array, level, serverId);
			asyncAssetsUseList.Clear();
			Log.Info($"TestBundleLoaded:[track info] = {stringBuilder}");
		}
	}

	public static void SetRequiredPackages(HashSet<int> packages)
	{
		if (ENABLE_BUNDLE_LOAD_TRACK)
		{
			requiredPackages = packages;
		}
	}
}
