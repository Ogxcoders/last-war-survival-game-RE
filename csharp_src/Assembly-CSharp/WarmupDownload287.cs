using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using GameFramework;
using UnityEngine;
using UnityEngine.Networking;
using VEngine;

public class WarmupDownload287
{
	private class _WarmupInfo
	{
		public string name;

		public int version;

		public ulong size;

		public uint crc;

		public string url;

		public UnityWebRequestAsyncOperation requestAsync;

		public Manifest manifest;

		public void SendRequest()
		{
			url = Versions.GetDownloadURL(string.Format("{0}{1}_v{2}", name, "_small", version));
			UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
			requestAsync = unityWebRequest.SendWebRequest();
			requestAsync.completed += RequestAsyncOnCompleted;
		}

		public void Stop()
		{
			requestAsync?.webRequest.Abort();
			requestAsync = null;
		}

		private void RequestAsyncOnCompleted(AsyncOperation obj)
		{
			UnityWebRequest webRequest = (obj as UnityWebRequestAsyncOperation).webRequest;
			requestAsync = null;
			if (!string.IsNullOrEmpty(webRequest.error))
			{
				Log.Error($"[WarmupDownload287] warmup manifest {name}_{version} request failed. url: {webRequest.url}, code: {webRequest.responseCode}, error: {webRequest.error}");
				return;
			}
			try
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				Log.Info($"[WarmupDownload287] Unzip {name}_{version}");
				Unzip(webRequest.downloadHandler.data);
				if (manifest == null)
				{
					return;
				}
				List<DownloadInfo> list = new List<DownloadInfo>();
				DownloadChecker downloadChecker = new DownloadChecker();
				downloadChecker.Collect(Versions.DownloadDataPath);
				GameEntry.Resource.GetDownloadSizeByPackage(manifest, list, 0, Versions.WarmupDataPath, downloadChecker);
				int[] requiredPackages = ResourcePackageManager.GetRequiredPackages();
				if (requiredPackages != null)
				{
					int i = 0;
					for (int num = requiredPackages.Length; i < num; i++)
					{
						GameEntry.Resource.GetDownloadSizeByPackage(manifest, list, requiredPackages[i], Versions.WarmupDataPath, downloadChecker);
					}
				}
				StartDownload(list);
				Log.Info($"[WarmupDownload287] after StartDownload {name}_{version}, {list.Count}, {stopwatch.Elapsed.TotalMilliseconds}");
			}
			catch (Exception message)
			{
				Log.Error(message);
			}
		}

		private void Unzip(byte[] data)
		{
			using MemoryStream stream = new MemoryStream(data);
			using ZipArchive zipArchive = new ZipArchive(stream);
			ZipArchiveEntry entry = zipArchive.GetEntry(name);
			if (entry == null)
			{
				return;
			}
			using Stream stream2 = entry.Open();
			manifest = new Manifest();
			using StreamReader sr = new StreamReader(stream2);
			manifest.ParseManifest(sr);
		}
	}

	private static bool _init = false;

	private static bool _hasUpdate = false;

	public static string warmupInfo = string.Empty;

	private static List<_WarmupInfo> _warmupInfos;

	private static List<Download> _downloads;

	private static ulong _totalSize;

	public static void StartWarmupDownload()
	{
		if (ClientConfig.SKIP_UPDATE_TABLE_AND_LOCALIZATION || Versions.SkipUpdate || _init)
		{
			return;
		}
		Log.Info("[WarmupDownload287] StartWarmupDownload " + WarmupDownload287.warmupInfo);
		if (string.IsNullOrEmpty(WarmupDownload287.warmupInfo))
		{
			return;
		}
		try
		{
			List<string> manifestNamesInUse = GameEntry.Resource.GetManifestNamesInUse();
			_warmupInfos = new List<_WarmupInfo>();
			_downloads = new List<Download>();
			string[] array = WarmupDownload287.warmupInfo.Split(new char[1] { ';' });
			foreach (string text in array)
			{
				if (string.IsNullOrEmpty(text))
				{
					continue;
				}
				string[] array2 = text.Split(new char[1] { ',' });
				if (array2.Length >= 4)
				{
					string text2 = array2[0];
					if (manifestNamesInUse.Contains(text2))
					{
						int version = int.Parse(array2[1]);
						ulong size = ulong.Parse(array2[2]);
						uint crc = uint.Parse(array2[3]);
						_warmupInfos.Add(new _WarmupInfo
						{
							name = text2.ToLower(),
							version = version,
							size = size,
							crc = crc
						});
					}
				}
			}
			int j = 0;
			for (int count = _warmupInfos.Count; j < count; j++)
			{
				_WarmupInfo warmupInfo = _warmupInfos[j];
				Manifest manifest = Versions.GetManifest(_warmupInfos[j].name);
				if (manifest != null)
				{
					if (warmupInfo.version > manifest.version)
					{
						warmupInfo.SendRequest();
						Log.Info($"[WarmupDownload287] warmup start {warmupInfo.name} v{warmupInfo.version}, {manifest.version}");
					}
				}
				else
				{
					warmupInfo.SendRequest();
					Log.Info($"[WarmupDownload287] warmup start {warmupInfo.name} v{warmupInfo.version}, new manifest.");
				}
			}
			_ = _warmupInfos.Count;
			_ = 0;
		}
		catch (Exception message)
		{
			Log.Error(message);
		}
		finally
		{
			_init = true;
		}
	}

	public static void StopWarmupDownload()
	{
		Log.Info("[WarmupDownload287] stop WarmupDownload.");
		if (_warmupInfos != null && _warmupInfos.Count > 0)
		{
			int i = 0;
			for (int count = _warmupInfos.Count; i < count; i++)
			{
				_warmupInfos[i].Stop();
			}
			_warmupInfos.Clear();
		}
		if (_hasUpdate)
		{
			Updater.RemoveUpdateCallback(Update);
			_hasUpdate = false;
		}
		if (_downloads != null && _downloads.Count > 0)
		{
			for (int num = _downloads.Count - 1; num >= 0; num--)
			{
				_downloads[num].Cancel();
			}
			_downloads.Clear();
		}
		_totalSize = 0uL;
		_init = false;
	}

	private static void StartDownload(List<DownloadInfo> downloadInfos)
	{
		if (!_init)
		{
			return;
		}
		foreach (DownloadInfo downloadInfo in downloadInfos)
		{
			_downloads.Add(Download.DownloadAsync(downloadInfo, 5));
			_totalSize += downloadInfo.size;
		}
		if (!_hasUpdate)
		{
			Updater.AddUpdateCallback(Update);
			_hasUpdate = true;
		}
	}

	private static void Update()
	{
		for (int num = _downloads.Count - 1; num >= 0; num--)
		{
			if (_downloads[num].isDone)
			{
				_downloads.RemoveAt(num);
			}
		}
		if (_downloads.Count <= 0)
		{
			Log.Info("[WarmupDownload287] all warmup download is done, remove update.");
			Updater.RemoveUpdateCallback(Update);
			_hasUpdate = false;
		}
	}
}
