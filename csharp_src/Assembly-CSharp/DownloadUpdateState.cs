using System;
using System.Collections.Generic;
using System.IO;
using FM_Mono;
using GameFramework;
using RiverGame.PerformanceAnalysis;
using UnityEngine;
using VEngine;

public class DownloadUpdateState : LoadingStateBase
{
	private enum Status
	{
		None,
		Download,
		PrepareConvert,
		Convert,
		SetupDownload
	}

	private List<Manifest> _downloadManifests;

	private List<Manifest> _downloadManifestsBk;

	private DownloadVersions _downloadVersions;

	private bool _tableChange;

	private bool _bundleChange;

	private float _downloadDuration;

	private ulong _downloadSize;

	private bool _gapFrame4Download;

	private bool _gapFrame4Setup;

	private DownloadUpdateStateBundleChecker _downloadChecker;

	private float _convertDuration = 1f;

	private float _convertElapsed;

	private Status _status;

	public DownloadUpdateState(AppStartupLoading startupLoading)
		: base(startupLoading)
	{
	}

	public override void OnEnter(params object[] args)
	{
		ResourcePackageManager.RemoveDownloadCompleteCallback();
		_gapFrame4Download = false;
		_gapFrame4Setup = false;
		_downloadSize = 0uL;
		Download.ResetCounter();
		PostEventLog.TrackMap("download_update_state", SDKManager.AddBILaunchTimeProperty());
		List<DownloadInfo> downloadInfos = new List<DownloadInfo>();
		ulong num = 0uL;
		_downloadDuration = ForegroundTimer.realtimeSinceStartup;
		if (!GameEntry.Resource.SkipUpdateBundle)
		{
			_downloadManifests = new List<Manifest>();
			_downloadManifestsBk = new List<Manifest>();
			Manifest[] obj = (Manifest[])args;
			string text = GameEntry.Resource.GetBkgroundManifestName().ToLower();
			string text2 = GameEntry.Resource.GetPackageResManifestName().ToLower();
			Manifest[] array = obj;
			foreach (Manifest manifest in array)
			{
				if (text == manifest.name)
				{
					_downloadManifestsBk.Add(manifest);
				}
				else if (manifest.name != text2)
				{
					_downloadManifests.Add(manifest);
				}
			}
			ulong num2 = 0uL;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			Versions.ProcessErrorBundleFile();
			_downloadChecker = new DownloadUpdateStateBundleChecker();
			_downloadChecker.Collect(Versions.DownloadDataPath);
			if (GrayUtils.isGM)
			{
				_downloadChecker.Log("UpdateState");
			}
			foreach (Manifest downloadManifest in _downloadManifests)
			{
				if (downloadManifest.name == GameEntry.Resource.GameResManifestName)
				{
					(ulong, ulong) downloadSizeByPackage = GameEntry.Resource.GetDownloadSizeByPackage(downloadManifest, downloadInfos, 0, null, _downloadChecker, useWarmup: true);
					num2 += downloadSizeByPackage.Item2;
					Log.Info("[DownloadUpdateState] manifest [" + downloadManifest.name + "] need download size: " + Utility.FormatBytes(downloadSizeByPackage.Item2));
					continue;
				}
				(ulong, ulong) downloadSizeByPackage2 = GameEntry.Resource.GetDownloadSizeByPackage(downloadManifest, downloadInfos, 0, null, null, useWarmup: true);
				num2 += downloadSizeByPackage2.Item2;
				Log.Info("[DownloadUpdateState] manifest [" + downloadManifest.name + "] need download size: " + Utility.FormatBytes(downloadSizeByPackage2.Item2) + ", strict");
			}
			ulong[] array2 = null;
			int[] requiredPackages = ResourcePackageManager.GetRequiredPackages();
			if (requiredPackages != null)
			{
				array2 = new ulong[requiredPackages.Length];
				Manifest manifest2 = null;
				foreach (Manifest downloadManifest2 in _downloadManifests)
				{
					if (!(downloadManifest2.name == GameEntry.Resource.GameResManifestName))
					{
						continue;
					}
					manifest2 = downloadManifest2;
					if (manifest2.assets == null || manifest2.assets.Count == 0)
					{
						manifest2 = Versions.GetManifest(downloadManifest2.name);
					}
					if (manifest2 != null && manifest2.groups != null)
					{
						int j = 0;
						for (int num3 = requiredPackages.Length; j < num3; j++)
						{
							(ulong, ulong) downloadSizeByPackage3 = GameEntry.Resource.GetDownloadSizeByPackage(manifest2, downloadInfos, requiredPackages[j], null, _downloadChecker, useWarmup: true);
							num2 += downloadSizeByPackage3.Item2;
							array2[j] = downloadSizeByPackage3.Item2;
							Log.Info($"[DownloadUpdateState] package: {requiredPackages[j]}, bundleSize: {Utility.FormatBytes(downloadSizeByPackage3.Item2)} / {Utility.FormatBytes(downloadSizeByPackage3.Item1)}");
						}
					}
				}
			}
			_bundleChange = num2 != 0;
			Log.Info($"[DownloadUpdateState] bundleChange: {_bundleChange}, bundleSize: {Utility.FormatBytes(num2)}, timeCost: {Time.realtimeSinceStartup - realtimeSinceStartup}");
			num += num2;
			ulong num4 = LWLuaFileUpdate.CheckUpdate(downloadInfos);
			num += num4;
			Log.Info("[DownloadUpdateState] download size: lwFile " + Utility.FormatBytes(num4) + " ");
			bool FIX_DOWNLOAD_SIZE = ClientSwitch.IsOn(13);
			_ = FIX_DOWNLOAD_SIZE;
		}
		if (!ClientConfig.SKIP_UPDATE_TABLE_AND_LOCALIZATION)
		{
			if (!ClientConfig.LocalMode)
			{
				ulong downloadSize = TableFileUpdate.GetDownloadSize(downloadInfos);
				_tableChange = downloadSize != 0;
				num += downloadSize;
				Log.Info("[DownloadUpdateState] download size: table " + Utility.FormatBytes(downloadSize) + " ");
			}
			LocalizationFileUpdate.StartDownloadBG();
		}
		if (num != 0)
		{
			_status = Status.Download;
			Log.Info("[DownloadUpdateState] download size: total " + Utility.FormatBytes(num) + " ");
			_startupLoading.BundleDownloadTotalBytes = num;
			ApplicationLaunch.Instance.hotUpdateCount++;
			PostEventLog.TrackMap("DOWNLOAD_START", new Dictionary<string, object> { 
			{
				"size1",
				num / 1024 / 1024
			} });
			GameEntry.Event.Fire(EventId.BeginDownloadUpdate);
			_downloadVersions = GameEntry.Resource.DownloadUpdates(downloadInfos);
		}
		else
		{
			_status = Status.PrepareConvert;
		}
	}

	private bool IsSame(string cachePath, DownloadInfo downloadInfo)
	{
		FileInfo fileInfo = new FileInfo(cachePath);
		if (fileInfo.Exists && fileInfo.Length > 0 && fileInfo.Length == (long)downloadInfo.size)
		{
			return Utility.ComputeCRC32(cachePath) == downloadInfo.crc;
		}
		return false;
	}

	public override void OnExit()
	{
		_downloadVersions = null;
		_status = Status.None;
	}

	public override void OnUpdate()
	{
		switch (_status)
		{
		case Status.None:
			Log.Error("[DownloadUpdateState] _status == None");
			break;
		case Status.Download:
			if (_downloadVersions == null)
			{
				break;
			}
			if (_downloadVersions.isDone)
			{
				_downloadDuration = ForegroundTimer.realtimeSinceStartup - _downloadDuration;
				GameEntry.Event.Fire(EventId.EndDownloadUpdate);
				if (!_gapFrame4Download)
				{
					_gapFrame4Download = true;
				}
				else if (_downloadVersions.isError)
				{
					Log.Error("DownloadUpdate error " + _downloadVersions.error);
					PostEventLog.TrackMap("DOWNLOAD_FAILED", new Dictionary<string, object> { { "download_duration", _downloadDuration } });
					_startupLoading.SetState(LoadingState.LoadingError, "E115");
				}
				else
				{
					_status = Status.PrepareConvert;
				}
			}
			else
			{
				_startupLoading.BundleDownloadProgress = _downloadVersions.progress;
			}
			break;
		case Status.PrepareConvert:
		{
			bool flag = ClientSwitch.IsOn(13);
			if (_downloadChecker != null && _downloadChecker.originBundleNames.Count > 0 && flag)
			{
				_convertDuration = 1f;
				_convertElapsed = 0f;
				_bundleChange = true;
				Loadable.ForceUnloadLoading();
				Bundle.Cache.Clear();
				Asset.Cache.Clear();
				AssetBundle.UnloadAllAssetBundles(unloadAllObjects: false);
				PostEventLog.TrackMap("CONVERT_BUNDLE", new Dictionary<string, object> { 
				{
					"i_para1",
					_downloadChecker.originBundleNames.Count
				} });
				_status = Status.Convert;
			}
			else
			{
				_status = Status.SetupDownload;
			}
			break;
		}
		case Status.Convert:
			_convertElapsed += Time.unscaledDeltaTime;
			if (_convertElapsed >= _convertDuration)
			{
				_status = Status.SetupDownload;
			}
			if (!ConvertBundleName())
			{
				_status = Status.SetupDownload;
			}
			break;
		case Status.SetupDownload:
		{
			if (_downloadVersions != null)
			{
				PostEventLog.TrackMap("DOWNLOAD_FINISH", new Dictionary<string, object>
				{
					{ "download_duration", _downloadDuration },
					{ "size1", _downloadSize },
					{
						"f_para1",
						Download.total_download_count
					}
				});
			}
			if (!File.Exists(ClientConfig.CURRENT_TABLE_FILE_PATH))
			{
				Log.Error("DownloadUpdate error data table not exist. " + ClientConfig.CURRENT_TABLE_FILE_PATH);
				_startupLoading.SetState(LoadingState.LoadingError, "lua table " + Path.GetFileName(ClientConfig.CURRENT_TABLE_FILE_PATH) + " disappear.");
				break;
			}
			if (_bundleChange)
			{
				GameEntry.Event.Fire(EventId.UILOADING_STATE_TEXT_CHANGE, true);
			}
			if (!_gapFrame4Setup)
			{
				_gapFrame4Setup = true;
				break;
			}
			string text = OnDownloadComplete(_bundleChange);
			if (!string.IsNullOrEmpty(text))
			{
				_startupLoading.SetState(LoadingState.LoadingError, text);
			}
			else
			{
				_startupLoading.SetState(LoadingState.LoadDataTable, _tableChange);
			}
			break;
		}
		}
	}

	private Manifest GetDownloadManifest(string name)
	{
		if (_downloadManifests == null)
		{
			return null;
		}
		return _downloadManifests.Find((Manifest m) => m.name == name);
	}

	private void OverrideManifest(bool refreshMemory = true)
	{
		if (_downloadManifests == null)
		{
			return;
		}
		foreach (Manifest downloadManifest in _downloadManifests)
		{
			GameEntry.Resource.OverrideManifest(downloadManifest, refreshMemory);
		}
	}

	private void CopyDll(Runtime.UpdateDllContext context)
	{
		Log.Info("[DownloadUpdateState] Copy Begin");
		Manifest downloadManifest = GetDownloadManifest(GameEntry.Resource.DllResManifestName);
		Manifest manifest = Versions.GetManifest(GameEntry.Resource.DllResManifestName);
		List<BundleInfo> bundlesWithGroups = Versions.GetBundlesWithGroups(new Manifest[1] { downloadManifest }, null);
		List<BundleInfo> bundlesWithGroups2 = Versions.GetBundlesWithGroups(new Manifest[1] { manifest }, null);
		HashSet<string> hashSet = new HashSet<string>();
		foreach (BundleInfo item in bundlesWithGroups2)
		{
			Log.Info("[DownloadUpdateState] Copy LocalBundle " + item.name);
			hashSet.Add(item.name);
		}
		string[] files = Directory.GetFiles(Path.Combine(Runtime.persistentDataPath, "Assemblies"), "*.*", SearchOption.TopDirectoryOnly);
		List<string> list = new List<string>();
		Log.Info($"[DownloadUpdateState] Copy GetAllFileListCount {files.Length}");
		foreach (BundleInfo item2 in bundlesWithGroups)
		{
			Log.Info("[DownloadUpdateState] Copy DownloadBundle " + item2.name);
			AssetBundle assetBundle = AssetBundle.LoadFromFile(Versions.GetBundlePathOrURL(item2));
			string text = assetBundle.GetAllAssetNames()[0];
			TextAsset textAsset = assetBundle.LoadAsset<TextAsset>(text);
			string text2 = textAsset.name;
			if (text2.EndsWith(".encrypt"))
			{
				text2 = text2.Substring(0, text2.Length - ".encrypt".Length);
				Debug.Log("dllFileName change to " + text2);
			}
			string text3 = "";
			text3 = ((!text.EndsWith(".encrypt.bytes")) ? ((!text.EndsWith(".bytes")) ? Path.GetExtension(text) : ".dll") : ".mdl");
			byte[] bytes = textAsset.bytes;
			assetBundle.Unload(unloadAllLoadedObjects: true);
			context.CopyDllTo(bytes, text2 + text3);
			list.Add(text2 + text3);
		}
		string[] array = files;
		foreach (string text4 in array)
		{
			Log.Info("History File " + text4);
			string fileName = Path.GetFileName(text4);
			if (!list.Contains(fileName))
			{
				Log.Info("[DownloadUpdateState] Copy Delete Unused File " + text4);
				try
				{
					File.Delete(text4);
				}
				catch (Exception arg)
				{
					Log.Error($"[DownloadUpdateState] Copy Delete Unused File Error {text4} {arg}");
				}
			}
		}
		Log.Info("[DownloadUpdateState] Copy done");
	}

	private bool ConvertBundleName()
	{
		try
		{
			if (_downloadChecker.originBundleNames.Count > 0)
			{
				string text = _downloadChecker.originBundleNames.Dequeue();
				Manifest manifest = null;
				if (text.StartsWith("gameres"))
				{
					manifest = GetDownloadManifest(GameEntry.Resource.GameResManifestName);
					if (manifest == null || manifest.assets == null || manifest.assets.Count == 0)
					{
						manifest = Versions.GetManifest(GameEntry.Resource.GameResManifestName);
					}
				}
				else if (text.StartsWith("dllres"))
				{
					manifest = GetDownloadManifest(GameEntry.Resource.DllResManifestName);
					if (manifest == null || manifest.assets == null || manifest.assets.Count == 0)
					{
						manifest = Versions.GetManifest(GameEntry.Resource.DllResManifestName);
					}
				}
				if (manifest != null)
				{
					BundleInfo bundle = manifest.GetBundle(text);
					if (bundle != null)
					{
						string downloadDataPath = Versions.GetDownloadDataPath(text);
						string downloadDataPath2 = Versions.GetDownloadDataPath(bundle.alias);
						bool flag = File.Exists(downloadDataPath);
						bool flag2 = File.Exists(downloadDataPath2);
						if (flag && !flag2)
						{
							try
							{
								File.Move(downloadDataPath, downloadDataPath2);
								Log.Info($"[DownloadUpdateState] Convert BundleName {downloadDataPath} => {downloadDataPath2}, [{_downloadChecker.originBundleNames.Count}], {Time.frameCount}");
							}
							catch (Exception arg)
							{
								Log.Error($"[DownloadUpdateState] Convert BundleName failed, {downloadDataPath}, {downloadDataPath2}, Exception: {arg}");
							}
						}
						else if (flag && flag2)
						{
							try
							{
								File.Delete(downloadDataPath);
							}
							catch (Exception arg2)
							{
								Log.Error($"[DownloadUpdateState] Convert BundleName failed, {downloadDataPath}, {downloadDataPath2}, Exception: {arg2}");
							}
						}
						else
						{
							Log.Error($"[DownloadUpdateState] Convert BundleName failed, {downloadDataPath}({flag}), {downloadDataPath2}({flag2})");
						}
					}
				}
				else
				{
					Log.Error("[DownloadUpdateState] Convert BundleName manifest == null, " + text);
				}
			}
		}
		catch (Exception message)
		{
			Log.Error(message);
		}
		return _downloadChecker.originBundleNames.Count > 0;
	}

	private string OnDownloadComplete(bool resUpdate)
	{
		bool flag = false;
		try
		{
			if (_tableChange)
			{
				TableFileUpdate.ApplyUpdate();
			}
			flag = true;
		}
		catch (Exception arg)
		{
			Log.Error($"[DownloadUpdateState] TableFileUpdate.ApplyUpdate Failed, {arg}");
		}
		Manifest manifest = Versions.GetManifest(GameEntry.Resource.DllResManifestName);
		Manifest downloadManifest = GetDownloadManifest(GameEntry.Resource.DllResManifestName);
		if (downloadManifest != null && downloadManifest.version > 0 && manifest != null && manifest.version > 0 && manifest.version != downloadManifest.version)
		{
			Log.Info($"[DownloadUpdateState] dll version changed, dll: {manifest.version}, {downloadManifest.version}");
			if (manifest.version <= downloadManifest.version || Versions.CanDowngrade)
			{
				string text = "E123";
				try
				{
					if (LWLuaFileUpdate.hasUpdate)
					{
						LWLuaFileUpdate.SaveUpdateFile();
					}
					Runtime.UpdateDllContext updateDllContext = new Runtime.UpdateDllContext();
					updateDllContext.Initialize();
					OverrideManifest(refreshMemory: false);
					CopyDll(updateDllContext);
					Log.Info("[DownloadUpdateState] dll version changed, need restart");
					_startupLoading.isErrorSuccess = true;
					text = "E999";
					updateDllContext.Dispose();
				}
				catch (Exception ex)
				{
					Log.Error($"[DownloadUpdateState] dll override and copy dll error {ex}");
					text = ((!ex.Message.Contains("Disk full")) ? "E123" : "E902");
				}
				return text;
			}
			Log.Info("[DownloadUpdateState] skip copy dll!");
		}
		bool flag2 = false;
		Manifest manifest2 = Versions.GetManifest(GameEntry.Resource.LuaManifestName);
		Manifest manifest3 = Versions.GetManifest(GameEntry.Resource.DataTableManifestName);
		Manifest downloadManifest2 = GetDownloadManifest(GameEntry.Resource.LuaManifestName);
		Manifest downloadManifest3 = GetDownloadManifest(GameEntry.Resource.DataTableManifestName);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		if ((downloadManifest2 != null && downloadManifest2.version > 0 && manifest2 != null && manifest2.version > 0 && manifest2.version != downloadManifest2.version) || (downloadManifest3 != null && downloadManifest3.version > 0 && manifest3 != null && manifest3.version > 0 && manifest3.version != downloadManifest3.version))
		{
			Log.Info("[DownloadUpdateState] enter lua change");
			flag2 = true;
			num = manifest2.version;
			num2 = downloadManifest2.version;
			num3 = manifest3.version;
			num4 = downloadManifest3.version;
		}
		if (_tableChange)
		{
			Log.Info("[DownloadUpdateState] enter lua data table change");
			flag2 = true;
		}
		if (LWLuaFileUpdate.hasUpdate)
		{
			flag2 = true;
		}
		if (flag2)
		{
			Log.Info($"[DownloadUpdateState] lua version changed, lua: {num}, {num2}, datatable: {num3}, {num4}");
			GameEntry.Lua.ExitGame();
			GameEntry.Lua.Shutdown();
		}
		if (_tableChange && !flag)
		{
			TableFileUpdate.ApplyUpdate();
		}
		string text2 = string.Empty;
		if (LWLuaFileUpdate.hasUpdate && !LWLuaFileUpdate.ApplyUpdate())
		{
			text2 = "E900";
		}
		if (!resUpdate)
		{
			Manifest manifest4 = Versions.GetManifest(GameEntry.Resource.GameResManifestName);
			Manifest downloadManifest4 = GetDownloadManifest(GameEntry.Resource.GameResManifestName);
			if (manifest4 != null && manifest4.version > 0 && downloadManifest4 != null && downloadManifest4.version > 0 && manifest4.version != downloadManifest4.version)
			{
				resUpdate = true;
				Log.Info($"[DownloadUpdateState] GameResUpdate.{manifest4.version}->{downloadManifest4.version}");
			}
		}
		if (resUpdate)
		{
			GameEntry.Sound.StopAllSounds();
			Log.Info("[DownloadUpdateState] StopAllSounds");
		}
		GameEntry.Resource.UnloadUnusedAssets();
		Log.Info("[DownloadUpdateState] Unload  resource  finish");
		if (resUpdate)
		{
			Log.Info("[DownloadUpdateState] Unload  Bundle  and  Asset");
			Loadable.ForceUnloadLoading();
			Bundle.Cache.Clear();
			Asset.Cache.Clear();
			AssetBundle.UnloadAllAssetBundles(unloadAllObjects: false);
			try
			{
				Runtime.UpdateDllContext updateDllContext2 = new Runtime.UpdateDllContext();
				updateDllContext2.Initialize();
				OverrideManifest();
				updateDllContext2.Dispose();
			}
			catch (Exception arg2)
			{
				Log.Error($"[DownloadUpdateState] dll override error {arg2}");
				return "E123";
			}
			Log.Info("[DownloadUpdateState] OverrideManifest");
		}
		GameEntry.Localization.InitFont();
		ApplicationLaunch.UpdateTrackingResVersion();
		if (flag2)
		{
			GameEntry.Lua.Initialize();
			GameEntry.Lua.StartGame();
			Log.Info($"[DownloadUpdateState] XLua Initialized after download: {0}", Time.realtimeSinceStartup);
		}
		Log.Info("[DownloadUpdateState] onDownLoadComplete finish");
		Log.Info("[DownloadUpdateState] Setup ResourcePackageManager DownloadChecker");
		ResourcePackageManager.SetupDownloadChecker();
		if (!string.IsNullOrEmpty(text2) && XLuaManager.s_useLwLuaFile)
		{
			Log.Error("[DownloadUpdateState] onDownLoadComplete LWLuaFile Update error.");
			return text2;
		}
		return "";
	}
}
