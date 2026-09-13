using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Coffee.UIExtensions;
using GameFramework;
using GameKit.Base;
using Main.Scripts.Application.LoadingState;
using ProtoBufNet;
using RGNetUtils;
using SFSLitJson;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.Networking;
using VEngine;

public class CheckResVersionState : LoadingStateBase
{
	private bool _useDirectLine;

	private List<string> _checkUrls = new List<string>();

	private List<HttpRequest> _requests = new List<HttpRequest>(2);

	private Dictionary<string, string> _requestHostMap = new Dictionary<string, string>(2);

	public CheckResVersionState(AppStartupLoading startupLoading)
		: base(startupLoading)
	{
	}

	public override void OnEnter(params object[] args)
	{
		_checkUrls.Clear();
		try
		{
			_startupLoading.ClearGameServerSetting();
			GameEntry.Network.ClearFinalGateServer();
			GameEntry.Sdk.SetDeviceIdSuperProperty();
			PostEventLog.TrackMap("check_res_version_state", SDKManager.AddBILaunchTimeProperty(new Dictionary<string, object> { 
			{
				"DelayLuaStartGame",
				XLuaManager.DelayLuaStartGame
			} }));
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
		}
		if (ApplicationLaunch.kParallelInit && ApplicationLaunch.Instance.checkResVersionParallel != null)
		{
			Log.Info("[ParallelInit] use checkResVersionParallel true");
			return;
		}
		Log.Info("[ParallelInit] use checkResVersionParallel false");
		_useDirectLine = false;
		StartVersionRequest();
	}

	public override void OnExit()
	{
		_useDirectLine = false;
		StopAllRequest();
		_requests.Clear();
		try
		{
			GameEntry.ResetCrossServerComponent();
		}
		catch (Exception ex)
		{
			Log.Error("GameEntry.ResetCrossServerComponent error : " + ex.Message);
		}
	}

	public override void OnUpdate()
	{
		if (!GameEntry.Lua.HasGameStart && XLuaManager.DelayLuaStartGame)
		{
			Log.Info("delay lua start game");
			GameEntry.Lua.StartGame();
		}
		if (ApplicationLaunch.kParallelInit && ApplicationLaunch.Instance.checkResVersionParallel != null)
		{
			CheckResVersionParallel checkResVersionParallel = ApplicationLaunch.Instance.checkResVersionParallel;
			if (checkResVersionParallel.queryStatus == CheckResVersionParallel.QueryStatus.Querying)
			{
				return;
			}
			if (checkResVersionParallel.queryStatus == CheckResVersionParallel.QueryStatus.Succeed)
			{
				if (OnCheckSuccess(checkResVersionParallel.succeedRequest, checkResVersionParallel.succeedDownloadHandler))
				{
					Log.Info("[ParallelInit] use checkResVersionParallel query Succeed");
					checkResVersionParallel.CopyRequestHost(_requestHostMap);
					OnRequestSuccess(checkResVersionParallel.succeedRequest, checkResVersionParallel.succeedDownloadHandler);
				}
				else
				{
					Log.Info("[ParallelInit] use checkResVersionParallel query Succeed, check failed");
					PostEventLog.TrackMap("CHECK_VERSION_FAILED", new Dictionary<string, object>
					{
						{
							"url",
							checkResVersionParallel.succeedRequest.url
						},
						{ "errMsg", "CheckSuccess Failed" }
					});
					_startupLoading.SetState(LoadingState.LoadingError, "E119");
					if (ClientSwitch.IsOn(19))
					{
						TracerouteCheckUrls();
					}
				}
			}
			else if (checkResVersionParallel.queryStatus == CheckResVersionParallel.QueryStatus.None)
			{
				Log.Info($"[ParallelInit] CheckResVersion queryStatus == {checkResVersionParallel.queryStatus}, fallback to CheckResVersionState, _useDirectLine: {_useDirectLine}");
				_useDirectLine = false;
				StartVersionRequest();
			}
			else if (checkResVersionParallel.queryStatus == CheckResVersionParallel.QueryStatus.Failed || checkResVersionParallel.queryStatus == CheckResVersionParallel.QueryStatus.Timeout)
			{
				Log.Info($"[ParallelInit] CheckResVersion queryStatus == {checkResVersionParallel.queryStatus}, fallback to CheckResVersionState, _useDirectLine: {_useDirectLine}");
				_useDirectLine = false;
				StartVersionRequest();
			}
			ApplicationLaunch.Instance.ShutdownCheckResVersion();
			return;
		}
		int count = _requests.Count;
		for (int i = 0; i < count; i++)
		{
			if (_requests.Count == 0)
			{
				break;
			}
			_requests[i].OnUpdate();
		}
	}

	private void StartVersionRequest(bool direct = false)
	{
		string[] hostListByCurGroupType = NetworkURLConfig.GetHostListByCurGroupType();
		int num = hostListByCurGroupType.Length;
		string[] array = new string[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = NetworkURLConfig.GetCheckVersionURL(hostListByCurGroupType[i]);
			_checkUrls.Add(hostListByCurGroupType[i]);
		}
		StartRequestQueue(array, hostListByCurGroupType);
	}

	private void StartRequestQueue(string[] urls, string[] hosts)
	{
		_requests.Clear();
		_requestHostMap.Clear();
		int num = urls.Length;
		for (int i = 0; i < num; i++)
		{
			string text = urls[i];
			HttpRequest httpRequest = new HttpRequest(text);
			httpRequest.onFailed += OnRequestFailed;
			httpRequest.onTimeOut += OnRequestTimeOut;
			httpRequest.onSuccess += OnRequestSuccess;
			httpRequest.checkSuccess += OnCheckSuccess;
			httpRequest.SendRequest();
			_requests.Add(httpRequest);
			_requestHostMap.Add(text, hosts[i]);
			Log.Info("CheckResVersionState::send:" + text);
		}
	}

	private void OnRequestFailed(HttpRequest req, string error)
	{
		Log.Info("CheckResVersion::Failed,err:" + error);
		bool flag = true;
		int count = _requests.Count;
		for (int i = 0; i < count; i++)
		{
			if (_requests.Count == 0)
			{
				break;
			}
			HttpRequest httpRequest = _requests[i];
			if (httpRequest.status != HttpRequest.Status.Failed && httpRequest.status != HttpRequest.Status.TimeOut)
			{
				flag = false;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		if (!_useDirectLine)
		{
			_useDirectLine = true;
			StartVersionRequest(direct: true);
			return;
		}
		PostEventLog.TrackMap("CHECK_VERSION_FAILED", new Dictionary<string, object>
		{
			{ "url", req.url },
			{ "errMsg", error }
		});
		_startupLoading.SetState(LoadingState.LoadingError, "E119");
		if (ClientSwitch.IsOn(19))
		{
			TracerouteCheckUrls();
		}
	}

	private void OnRequestTimeOut(HttpRequest req)
	{
		Log.Info("CheckResVersion::Timeout,url:" + req.url);
		bool flag = true;
		int count = _requests.Count;
		for (int i = 0; i < count; i++)
		{
			if (_requests.Count == 0)
			{
				break;
			}
			HttpRequest httpRequest = _requests[i];
			if (httpRequest.status != HttpRequest.Status.Failed && httpRequest.status != HttpRequest.Status.TimeOut)
			{
				flag = false;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		if (!_useDirectLine)
		{
			_useDirectLine = true;
			StartVersionRequest(direct: true);
			return;
		}
		PostEventLog.TrackMap("CHECK_VERSION_TIMEOUT", new Dictionary<string, object> { { "url", req.url } });
		_startupLoading.SetState(LoadingState.LoadingError, "E118");
		if (ClientSwitch.IsOn(19))
		{
			TracerouteCheckUrls();
		}
	}

	private void TracerouteCheckUrls()
	{
		List<string> list = new List<string>();
		if (_checkUrls.Count > 0)
		{
			for (int i = 0; i < _checkUrls.Count; i++)
			{
				try
				{
					Uri uri = new Uri(_checkUrls[i]);
					list.Add(uri.Host);
				}
				catch (Exception ex)
				{
					Log.Error("Parse url " + _checkUrls[i] + " error " + ex.Message + " ");
					throw;
				}
			}
		}
		if (list.Count > 0)
		{
			UDPTraceroute.StartAndReportAsync(list.ToArray(), "CHECK_VERSION_FAILED_TRACEROUTE", 24, 1, 300);
		}
	}

	private bool OnCheckSuccess(HttpRequest arg2, DownloadHandler downloadHandler)
	{
		if (!NetworkURLConfig.IsOnline)
		{
			return true;
		}
		bool flag = false;
		try
		{
			JsonData jsonData = JsonMapper.ToObject(downloadHandler.text);
			IDictionary dictionary = jsonData;
			if (dictionary.Contains("code") && !string.IsNullOrEmpty(jsonData["code"].ToString()))
			{
				flag = true;
			}
			else if (dictionary.Contains("updateType"))
			{
				int num = ((string)jsonData["updateType"]).ToInt();
				if (num >= 0 && num <= 3 && dictionary.Contains("hotUpdateMsg") && !string.IsNullOrEmpty((string)jsonData["hotUpdateMsg"]))
				{
					flag = true;
				}
			}
		}
		catch (Exception)
		{
			flag = false;
		}
		if (!flag)
		{
			Log.Error("CheckResVersion::OnCheckSuccess error,result:" + downloadHandler.text);
		}
		return flag;
	}

	protected virtual void InitClientSwitch(object data)
	{
		ClientSwitch.Parse((string)data);
	}

	private void OnRequestSuccess(HttpRequest req, DownloadHandler downloadHandler)
	{
		try
		{
			Log.Info("CheckResVersion::OnRequestSuccess::url:" + req.url + " , raw json:" + downloadHandler.text);
			JsonData jsonData = JsonMapper.ToObject(downloadHandler.text);
			IDictionary dictionary = jsonData;
			if (dictionary.Contains("updateType"))
			{
				try
				{
					if (((string)jsonData["updateType"]).ToInt() == 2)
					{
						string text = (string)jsonData["downloadurl"];
						_startupLoading.SetState(LoadingState.LoadingError, "error force update", text);
						return;
					}
				}
				catch (Exception ex)
				{
					Log.Error("CheckResVersion::updateType error:" + ex.ToString());
				}
			}
			if (dictionary.Contains("code"))
			{
				string text2 = jsonData["code"].ToString();
				string text3 = jsonData["msg"].ToString();
				text3 = text2 + "::" + text3;
				throw new Exception(text3);
			}
			if (dictionary.Contains("resMsg"))
			{
				try
				{
					AESHelper.Setp((string)jsonData["resMsg"]);
				}
				catch (Exception ex2)
				{
					Log.Error("CheckResVersion::resMsg error:" + ex2.ToString());
				}
			}
			int num = ((string)jsonData["updateType"]).ToInt();
			string downLoadUrl = (string)jsonData["downloadurl"];
			string text4 = (string)jsonData["hotUpdateMsg"];
			try
			{
				SaveManifestVersion(text4);
			}
			catch (IOException message)
			{
				Log.Error(message);
				_startupLoading.SetState(LoadingState.LoadingError, "E900");
				return;
			}
			catch (Exception message2)
			{
				Log.Error(message2);
				_startupLoading.SetState(LoadingState.LoadingError, "E114");
				return;
			}
			string value = "";
			if (!_requestHostMap.TryGetValue(req.url, out value))
			{
				throw new Exception("CheckResVersion OnRequestSuccess Get Host Error !");
			}
			GameEntry.Network.SelectFinalGateServer(value);
			PostEventLog.TrackMap("CHECK_VERSION_SUCCESS", SDKManager.AddBILaunchTimeProperty(new Dictionary<string, object>
			{
				{ "url", req.url },
				{ "urlHost", value }
			}));
			LWLuaFileUpdate.ResetFileVersionInfo();
			string text5 = string.Empty;
			if (dictionary.Contains("lwfile2"))
			{
				text5 = (string)jsonData["lwfile2"];
				LWLuaFileUpdate.SetFileVersionInfo(text5);
			}
			GameEntry.Setting.IsReview = dictionary.Contains("checkok");
			Log.Info($"CheckResVersion::isReview:{GameEntry.Setting.IsReview}");
			ClientConfig.SKIP_UPDATE_TABLE_AND_LOCALIZATION = GameEntry.Setting.IsReview;
			if (!GameEntry.Setting.IsReview)
			{
				if (!ClientConfig.LocalMode)
				{
					ClientConfig.ParseTableVersionInfo((string)jsonData["table_version"]);
				}
				ClientConfig.SplitLocaleVersionInfo((string)jsonData["locale"], ref ClientConfig.REMOTE_LOCALE_VERSION, ref ClientConfig.REMOTE_LOCALE_SUPPORT);
			}
			if (dictionary.Contains("client_config"))
			{
				InitClientSwitch((string)jsonData["client_config"]);
			}
			ShumeiSdkManager.Instance.CallCreate();
			if (ClientSwitch.IsOn(10))
			{
				GameEntry.Sdk.SendDataToNative("SWITCH_DelayPauseResume", "1");
			}
			NetReceiveProfiler.recordCMDId = GrayUtils.InGrayServer(1, 69) || GrayUtils.isGM;
			if (ClientSwitch.IsOn(37))
			{
				UIParticle.ENABLE_STEP_BAKE_MESH = true;
				Log.Info("ClientSwitch ENABLE_WORLD_MARCH_STEP_EXECUTE");
			}
			else
			{
				UIParticle.ENABLE_STEP_BAKE_MESH = false;
			}
			if (ClientSwitch.IsOn(39))
			{
				Log.Info("ClientSwitch ENABLE_WORLD_MARCH_MESSAGE_MERGE");
			}
			if (ClientSwitch.IsOn(40))
			{
				Log.Info("ClientSwitch ENABLE_WORLD_MARCH_MESSAGE_STEP");
			}
			if (ClientSwitch.IsOn(23))
			{
				Log.Info("ClientSwitch ENABLE_BUNDLE_USE_TRACK");
				Versions.ENABLE_BUNDLE_USE_TRACK = true;
			}
			if (ClientSwitch.IsOn(22))
			{
				Log.Info("ClientSwitch ENABLE_ASSETS_USE_TRACK");
				AssetsStatistics.ENABLE_ASSETS_USE_TRACK = true;
			}
			else
			{
				AssetsStatistics.ENABLE_ASSETS_USE_TRACK = false;
			}
			if (ClientSwitch.IsOn(38))
			{
				AssetsStatistics.ENABLE_BUNDLE_LOAD_TRACK = true;
			}
			else
			{
				AssetsStatistics.ENABLE_BUNDLE_LOAD_TRACK = false;
			}
			Loadable.unuse_not_done = ClientSwitch.IsOn(1);
			Log.Info($"ClientSwitch  ENABLE_UNUSE_NOTDONE_ASSET {Loadable.unuse_not_done}");
			ProfilerRuntime.Enabled = CommonUtils.IsDebug() || GrayUtils.InGrayServer(1, 68);
			if (ProfilerRuntime.Enabled)
			{
				ProfilerRuntime.SetReportSettings(10, 120f, 120f);
				ProfilerRuntime.SetReportCallback(delegate
				{
					ProfilerRuntime.DumpToConsole();
					ProfilerRuntime.ResetSamples();
				});
				GameEntry.Lua.SafeDoString("ProfilerUtil.InitRuntimeSample()");
			}
			NetPacketConst.useNewPacket = !ClientSwitch.IsOn(4) && typeof(SFSObject).GetProperty("DataHolder") != null;
			NetPacketConst.useZStd = NetPacketConst.invokeZstdDllMapSucc && !ClientSwitch.IsOn(5) && StringUtils.VersionCompare(GameEntry.Sdk.Version, "1.0.260") >= 0;
			if (!ClientSwitch.IsOn(5) && StringUtils.VersionCompare(GameEntry.Sdk.Version, "1.0.260") >= 0 && !NetPacketConst.invokeZstdDllMapSucc)
			{
				Log.Info("Zstd dll invoke failed");
			}
			RealTimer.useNativeRealTime = StringUtils.VersionCompare(GameEntry.Sdk.Version, "1.0.251") >= 0;
			TimerComponent.useRealTime = true;
			Log.Info($"use realtime sync time {TimerComponent.useRealTime}, use native realtime {RealTimer.useNativeRealTime}.");
			Download.ASYNC_CALL = true;
			Download.USE_DOWNLOAD_QUEUE = true;
			GameEntry.Setting.SetBool("ENABLE_DISPOSE_OLD_LUA_ENV_KEY", ClientSwitch.IsOn(15));
			NetRawProxy.USE_IPV6 = true;
			bool flag = true;
			_startupLoading.ClearUpdateAfterLogin(flag);
			Log.Info($"[CheckResVersionState] updateAfterLogin {flag}");
			Versions.BundleFastValidation = ClientSwitch.IsOn(2);
			Log.Info($"[CheckResVersionState] BundleFastValidation {Versions.BundleFastValidation}");
			BundleInfo.UseBundleAlias = ClientSwitch.IsOff(17);
			Log.Info($"[CheckResVersionState] UseBundleAlias {BundleInfo.UseBundleAlias}");
			Versions.UseBundleDownloadedCache = ClientSwitch.IsOn(18);
			Log.Info($"[CheckResVersionState] UseBundleDownloadedCache {Versions.UseBundleDownloadedCache}");
			Download.USE_CDN_SPEED_TEST = ClientSwitch.IsOn(20) && GrayUtils.IsGrayDevice(20);
			Log.Info($"[CheckResVersionState] USE_CDN_SPEED_TEST {Download.USE_CDN_SPEED_TEST}");
			if (dictionary.Contains("warmup"))
			{
				WarmupDownload.warmupInfo = (string)jsonData["warmup"];
				WarmupDownload287.warmupInfo = (string)jsonData["warmup"];
			}
			if (GameEntry.Setting.CheckFirstLaunchSkipUpdate(init: true))
			{
				GameEntry.Setting.FirstLaunchSkipUpdateNewestVersion = true;
				string systemLanguageConfig = GameEntry.Lua.CallWithReturn<string, string, string>("CSharpCallLuaInterface.GetConfigStr", "first_launch_skip_update", "k3");
				bool flag2 = false;
				try
				{
					flag2 = StringUtils.FirstLaunchUpdateLanguageCompare(systemLanguageConfig);
				}
				catch (Exception arg)
				{
					Log.Error($"CheckResVersion FirstLaunchUpdateLanguageCompare error : {arg}");
				}
				if (!flag2)
				{
					GameEntry.Setting.DisableFirstLaunchSkipUpdate();
					string text6 = "CheckResVersion FirstLaunchUpdateLanguageCompare false ";
					Log.Info(text6);
					PostEventLog.TrackMap("FirstLaunchSkipUpdate", new Dictionary<string, object>
					{
						{ "status", false },
						{ "reason", text6 },
						{
							"lw_res_version",
							GameEntry.Resource.GetResVersion()
						}
					});
				}
				else
				{
					string forceUpdateMsg = string.Empty;
					if (dictionary.Contains("firstLaunchForceUpdateMsg"))
					{
						forceUpdateMsg = (string)jsonData["firstLaunchForceUpdateMsg"];
					}
					int versionDiff = GameEntry.Lua.CallWithReturn<int, string, string>("CSharpCallLuaInterface.GetConfigNum", "first_launch_skip_update", "k1");
					int remote_Table_Version = ClientConfig.Remote_Table_Version;
					bool flag3 = true;
					try
					{
						flag3 = StringUtils.FirstLaunchUpdateVersionCompare(text4, text5, forceUpdateMsg, versionDiff, remote_Table_Version, out var newestVersion);
						GameEntry.Setting.FirstLaunchSkipUpdateNewestVersion = newestVersion;
					}
					catch (Exception arg2)
					{
						Log.Error($"CheckResVersion FirstLaunchUpdateVersionCompare error : {arg2}");
					}
					if (flag3)
					{
						GameEntry.Setting.DisableFirstLaunchSkipUpdate();
						string text7 = "CheckResVersion FirstLaunchUpdateVersionCompare true ";
						Log.Info(text7);
						PostEventLog.TrackMap("FirstLaunchSkipUpdate", new Dictionary<string, object>
						{
							{ "status", false },
							{ "reason", text7 },
							{
								"lw_res_version",
								GameEntry.Resource.GetResVersion()
							}
						});
					}
				}
				if (GameEntry.Setting.IsReview)
				{
					GameEntry.Setting.FirstLaunchSkipUpdateNewestVersion = true;
				}
			}
			try
			{
				ResourcePackageManager.SetRequiredPackagesByCheckResVersionState(dictionary, jsonData);
			}
			catch (Exception ex3)
			{
				Log.Error("CheckResVersion::SetRequiredPackages error:" + ex3.ToString());
			}
			string text8 = "Google Play";
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				text8 = "AppStore";
			}
			string message3 = GameEntry.Localization.GetString("129013", text8);
			switch (num)
			{
			case 1:
				UIUtils.ShowMessage(message3, 2, "110003", "", delegate
				{
					if (!downLoadUrl.IsNullOrEmpty())
					{
						SDKManager.OpenURL(downLoadUrl);
					}
				}, delegate
				{
					_startupLoading.ToDownloadManifest();
				});
				break;
			case 2:
				UIUtils.ShowMessage(message3, 1, "110003", "", delegate
				{
					if (!downLoadUrl.IsNullOrEmpty())
					{
						SDKManager.OpenURL(downLoadUrl);
					}
				}, delegate
				{
					if (!downLoadUrl.IsNullOrEmpty())
					{
						SDKManager.OpenURL(downLoadUrl);
					}
				});
				break;
			default:
				if (!LWLuaFileUpdate.initSucceed)
				{
					Log.Error("[LWLuaFileUpdate] CheckResVersion::LW_LUA_SETUP_CHECK false.");
					UIUtils.ShowMessage(GameEntry.Localization.GetString("install_tips_capacity") + "\nE900\n" + GameEntry.Device.GetDeviceUid(), 1, "390457", "", delegate
					{
						LWLuaFileUpdate.SetClearFlag();
						_startupLoading.ToDownloadManifest();
					}, delegate
					{
						LWLuaFileUpdate.SetClearFlag();
						_startupLoading.ToDownloadManifest();
					});
				}
				else
				{
					_startupLoading.ToDownloadManifest();
				}
				break;
			}
		}
		catch (Exception ex4)
		{
			string text9 = "json error.use package version.err:" + ex4.ToString() + ",raw json:" + downloadHandler.text;
			PostEventLog.TrackMap("CHECK_VERSION_FAILED", new Dictionary<string, object>
			{
				{ "url", req.url },
				{ "errMsg", text9 }
			});
			Log.Error(text9);
			_startupLoading.ToLoadDataTable();
		}
		StopAllRequest();
	}

	private void StopAllRequest()
	{
		int count = _requests.Count;
		for (int i = 0; i < count; i++)
		{
			if (_requests.Count == 0)
			{
				break;
			}
			_requests[i].Dispose();
		}
	}

	private static void SaveManifestVersion(string data)
	{
		if (string.IsNullOrEmpty(data))
		{
			return;
		}
		string[] array = data.Split(new char[1] { ';' });
		if (array.Length == 0)
		{
			return;
		}
		foreach (string text in array)
		{
			if (!string.IsNullOrEmpty(text))
			{
				string[] array2 = text.Split(new char[1] { ',' });
				if (array2.Length >= 4)
				{
					string text2 = array2[0];
					string text3 = array2[1];
					string text4 = array2[2];
					string text5 = array2[3];
					string text6 = text3 + "," + text4 + "," + text5;
					File.WriteAllText(GameEntry.Resource.GetTempDownloadPath(text2.ToLower() + ".version"), text6);
					Log.Info("CheckResVersionState::SaveManifestVersion " + text2 + ": " + text6);
				}
			}
		}
	}
}
