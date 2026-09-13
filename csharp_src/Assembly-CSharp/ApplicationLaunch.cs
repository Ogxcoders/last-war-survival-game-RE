using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using AIHelp;
using FM_Mono;
using FibMatrix;
using GameFramework;
using GameKit.Base;
using PVEBattleLogic;
using RiverBISDK;
using RiverGame.PerformanceAnalysis;
using TimelineScript;
using UnityEngine;
using UnityEngine.Profiling;
using VEngine;
using Zendesk;

public class ApplicationLaunch : MonoBehaviour
{
	public static bool kParallelInit = true;

	public const int FLAG_DATATABLE = 1;

	public const int FLAG_LOCALE = 2;

	public const int FLAG_LWSCRIPT = 4;

	public const int FLAG_ALLSET = 7;

	private ConcurrentQueue<LaunchTask> _taskQueue;

	private int flagDone;

	private int flagSucceed;

	protected static ApplicationLaunch _instance;

	private const string googlePublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAiYVPWSTL1D2+Rbyx9uyAx6PlPuXGGufEqInq1LfN3Thu7g6XYniKB0zhUKElEhSHxKMHJSJYK7TpUx6NGmW075+VHngPNHa94wZakn/pUvwoumRad/FJUoTlk3JeNfemlcUHwSNyzrykt1tCYqakgsQzETZY/Bp0C+NjOVzZFqMqZkmHy+n7IJx1GVOU9AW9HHaUmqAvPDLsolpKDoPw5KN1XIw+Z04HDjgpG3m9p+YaDNuUUWcJu12oQ+2qTrwzZSwwLe/hlG6uFciD6aar7/rbXydFZjYGQVeMMj4YQhe4T7ROEq6tlobn2yEeeJw8JMj92K46XbVeCMnJK+Vv3wIDAQAB";

	private long enterBackGroundTime;

	private bool isReloadGame;

	internal int hotUpdateCount;

	private LogFile logFile;

	private double _lastUpdateTime;

	private InGameCheckResVersionTools _checkResVersionTools;

	private static float _lastLaunchStepTimestamp = 0f;

	public static float awakeTime = 0f;

	private bool isResourceInitOk;

	private bool _isRegistered;

	public static string ParallelInitLockFile => Application.persistentDataPath + "/thread.lock";

	public bool taskAllDone => flagDone == 7;

	public bool taskAllSucceed => flagSucceed == 7;

	public CheckResVersionParallel checkResVersionParallel { get; private set; }

	public static ApplicationLaunch Instance => _instance;

	public bool isReloading => isReloadGame;

	internal int reloadGameCount { get; private set; }

	internal int disconnectRetryCount { get; private set; }

	public AppStartupLoading Loading { get; protected set; }

	private void SetTaskDone(int flag)
	{
		flagDone |= flag;
	}

	private void SetTaskSucceed(int flag)
	{
		flagDone |= flag;
		flagSucceed |= flag;
	}

	public bool TaskDone(int flag)
	{
		return (flagDone & flag) == flag;
	}

	public bool TaskSucceed(int flag)
	{
		return (flagSucceed & flag) == flag;
	}

	private void StartParallelInitTask()
	{
		_taskQueue = new ConcurrentQueue<LaunchTask>();
		flagDone = 0;
		flagSucceed = 0;
		EnqueueTask(new LaunchTask(ELaunchTask.DataFileInitialize));
		EnqueueTask(new LaunchTask(ELaunchTask.LocaleFileInitialize));
		EnqueueTask(new LaunchTask(ELaunchTask.LwScriptInitialize));
		EnqueueTask(new LaunchTask(ELaunchTask.CheckResVersion));
	}

	public static void EnqueueTask(LaunchTask task)
	{
		_instance._taskQueue.Enqueue(task);
		Log.Info($"[ParallelInit] EnqueueTask {task.id}");
	}

	private void UpdateLaunchTask()
	{
		LaunchTask result;
		while (_taskQueue.TryDequeue(out result))
		{
			switch (result.id)
			{
			case ELaunchTask.DataFileInitialize:
				ClientConfig.CheckDataFileEnv();
				ThreadPool.QueueUserWorkItem(ClientConfig.th_CheckDataFile, result.payload);
				break;
			case ELaunchTask.DataFileSelect:
				try
				{
					ClientConfig.mt_SelectDataFile(result.payload);
					SetTaskSucceed(1);
				}
				catch (Exception arg6)
				{
					Log.Error($"[ParallelInit] mt_SelectDataFile {arg6}");
					SetTaskDone(1);
				}
				break;
			case ELaunchTask.LocaleFileInitialize:
				ThreadPool.QueueUserWorkItem(ClientConfig.th_CheckLocaleFile, GameEntry.Setting.UserLanguage);
				break;
			case ELaunchTask.LocaleFileSelect:
				try
				{
					ClientConfig.mt_SelectLocaleFile(result.payload);
				}
				catch (Exception arg5)
				{
					Log.Error($"[ParallelInit] mt_SelectLocaleFile {arg5}");
					SetTaskDone(2);
				}
				break;
			case ELaunchTask.LocaleFileLoadInPersistent:
				GameEntry.Localization.SetSwapLocaleFile(null);
				ThreadPool.QueueUserWorkItem(LocaleFileParallel.th_LoadLocaleFileInPersistent, result.payload);
				break;
			case ELaunchTask.LocaleFileLoadInPackage:
				try
				{
					GameEntry.Localization.SetSwapLocaleFile(null);
					LocaleFileParallel.mt_LoadLocaleFileInPackage(result.payload);
				}
				catch (Exception arg4)
				{
					Log.Error($"[ParallelInit] mt_LoadLocaleFileInPackage {arg4}");
					SetTaskDone(2);
				}
				break;
			case ELaunchTask.LocaleFileSwap:
				try
				{
					LocaleFileParallel.mt_LocaleFileSwap(result.payload);
					SetTaskSucceed(2);
				}
				catch (Exception arg3)
				{
					Log.Error($"[ParallelInit] mt_LocaleFileSwap {arg3}");
					SetTaskDone(2);
				}
				break;
			case ELaunchTask.LwScriptInitialize:
				LWLuaFileUpdateParallel.mt_CheckLwScript(result.payload);
				ThreadPool.QueueUserWorkItem(LWLuaFileUpdateParallel.th_LwScriptInit, result.payload);
				break;
			case ELaunchTask.LwScriptCopyFromPackage:
				try
				{
					LWLuaFileUpdateParallel.mt_CopyLwScriptFileFromPackage(result.payload);
				}
				catch (Exception arg2)
				{
					Log.Error($"[ParallelInit] [LWLuaFileUpdate] mt_CopyLwScriptFileFromPackage {arg2}");
					SetTaskDone(4);
				}
				break;
			case ELaunchTask.LwScriptLoad:
				ThreadPool.QueueUserWorkItem(LWLuaFileUpdateParallel.th_LwScriptLoad, result.payload);
				break;
			case ELaunchTask.LwScriptSwap:
				try
				{
					LWLuaFileUpdateParallel.mt_LwScriptSwap(result.payload);
					SetTaskSucceed(4);
				}
				catch (Exception arg)
				{
					Log.Error($"[ParallelInit] [LWLuaFileUpdate] mt_LwScriptSwap {arg}");
					SetTaskDone(4);
				}
				break;
			case ELaunchTask.CheckResVersion:
				checkResVersionParallel = new CheckResVersionParallel();
				checkResVersionParallel.StartVersionRequest();
				break;
			case ELaunchTask.SetTaskDone:
				SetTaskDone((int)result.payload);
				break;
			}
		}
		if (checkResVersionParallel != null && checkResVersionParallel.queryStatus == CheckResVersionParallel.QueryStatus.Querying)
		{
			checkResVersionParallel.OnUpdate();
		}
	}

	public void ShutdownCheckResVersion()
	{
		if (checkResVersionParallel != null)
		{
			checkResVersionParallel.StopAllRequest();
			checkResVersionParallel = null;
		}
	}

	public static void ResetStep()
	{
		_lastLaunchStepTimestamp = Time.realtimeSinceStartup;
	}

	public static void StepLog(string msg)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		Log.Info($"[Loading] {msg}, f:{Time.frameCount}, t:{realtimeSinceStartup}, Δt:{realtimeSinceStartup - _lastLaunchStepTimestamp}");
		_lastLaunchStepTimestamp = realtimeSinceStartup;
	}

	private void Awake()
	{
		awakeTime = Time.realtimeSinceStartup;
		ResetStep();
		StepLog("Application Awake!");
		if (File.Exists(ParallelInitLockFile))
		{
			kParallelInit = false;
			StepLog("disable parallel init by lock file");
		}
		else
		{
			kParallelInit = ClientSwitch.IsCacheOn(48);
			StepLog((kParallelInit ? "enable" : "disable") + " parallel init by switch");
		}
		AnoProxy.SdkInitEx();
		if (Runtime.IsDllLocalDirLocked())
		{
			Log.Info("DllLocalDir is locked, try to unlock it");
			Versions.DeleteManifest();
			Runtime.UnlockDllLocalDir();
		}
		CultureInfo.DefaultThreadCurrentUICulture = (CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US"));
		Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.ScriptOnly);
		Application.SetStackTraceLogType(LogType.Exception, StackTraceLogType.ScriptOnly);
		Application.SetStackTraceLogType(LogType.Assert, StackTraceLogType.ScriptOnly);
		Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
		Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
		Application.lowMemory += OnLowMemory;
		_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		AIHelpProxy.Init();
		ConfigBastHttp();
		GameEntry.Init();
		ZendeskInit.Init();
		GameEntry.Resource.Loggable = GameEntry.Setting.GetBool("Setting.Resource.Logger", defaultValue: false);
		try
		{
			ConfigBI();
			ConfigureRemoteReport();
		}
		catch (Exception message)
		{
			Log.Error(message);
		}
		RealTimer.Reset();
		LWLuaFile.ConfigPath();
		ClientConfig.ConfigPath();
		LWLuaFileUpdate.ConfigPath();
		ChooseDebugUpdateFunc(PostAwake);
	}

	private void ChooseDebugUpdateFunc(Action callback)
	{
		callback();
	}

	private void PostAwake()
	{
		StepLog($"Use ParallelInit {kParallelInit}");
		if (kParallelInit)
		{
			StartParallelInitTask();
		}
		Loading = new AppStartupLoading();
		StepLog("Resource.Initialize");
		GameEntry.Resource.Initialize(OnResourcesInitialized);
		AIHelpProxy.SetAIHelpDataList();
	}

	private void ConfigAIHelp()
	{
		AIHelpProxy.UpdateUserInfo(GameEntry.Device.GetDeviceUid(), "--", "", "", "");
	}

	private void SafeQuit(Action quitFunc)
	{
		if (quitFunc == null)
		{
			return;
		}
		try
		{
			quitFunc?.Invoke();
		}
		catch (Exception ex)
		{
			Log.Error("quit error: {0}", ex.StackTrace);
		}
	}

	private void OnApplicationQuit()
	{
		SafeQuit(delegate
		{
			PostEventLog.Record("APP_QUIT");
		});
		SafeQuit(SceneManager.Destroy);
		SafeQuit((Loading != null) ? new Action(Loading.Shutdown) : null);
		SafeQuit(GameEntry.Shutdown);
		SafeQuit(PostEventLog.stop);
		SafeQuit(DisposeRemoteReportHandler);
		SafeQuit(FibMatrix.Logger.DisposeTargets);
		SafeQuit(GlobalMonobehaviourDispatcher.instance.OnApplicationQuitListener);
	}

	private void Update()
	{
		RealTimer.frameCount = Time.frameCount;
		RealTimer.Update();
		try
		{
			if (FileLoggerTarget.instance != null)
			{
				FileLoggerTarget.UpdateUploadRequests();
			}
			if (kParallelInit)
			{
				UpdateLaunchTask();
			}
			if (isReloadGame && !XLuaManager.DelayLuaStartGame)
			{
				ReloadGameImpl();
				isReloadGame = false;
			}
			Loading.Update();
			GameEntry.Update(Time.deltaTime);
			SceneManager.Update();
			if ((double)Time.time - _lastUpdateTime > 1.0)
			{
				CheckOffline();
				_lastUpdateTime = Time.time;
			}
			if (isReloadGame && XLuaManager.DelayLuaStartGame)
			{
				ReloadGameImpl();
				isReloadGame = false;
			}
			if (_checkResVersionTools != null)
			{
				_checkResVersionTools.Update();
			}
			FibMatrix.Logger.Update();
		}
		catch (Exception ex)
		{
			Log.Error("app launch error {0}_{1}", ex.Message, ex.StackTrace);
		}
	}

	private void LateUpdate()
	{
		ForegroundTimer.Tick();
		SceneManager.LateUpdate();
	}

	private void FixedUpdate()
	{
		SceneManager.FixedUpdate();
		LoadingStart();
	}

	private void LoadingStart()
	{
		if (isResourceInitOk && GameEntry.Sdk.IsShowLogoOk() && !FileLoggerTarget.uploading)
		{
			Loading.Start(isReload: false, showLogo: true);
			isResourceInitOk = false;
		}
	}

	protected void OnResourcesInitialized(bool succ)
	{
		StepLog($"OnResourcesInitialized {succ}");
		isResourceInitOk = true;
		ConfigAIHelp();
		GameEntry.Sdk.SetDeviceLevelSuperProperty();
		GameEntry.Sdk.runtimeInfo.LogRuntimeInfo();
		PostEventLog.TrackMap("GL_SUPPORTS", new Dictionary<string, object>
		{
			{
				"CS",
				SystemInfo.supportsComputeShaders ? 1 : 0
			},
			{
				"maxCSInputs",
				SystemInfo.maxComputeBufferInputsCompute
			},
			{
				"maxVSInputs",
				SystemInfo.maxComputeBufferInputsVertex
			},
			{
				"maxPSInputs",
				SystemInfo.maxComputeBufferInputsFragment
			}
		});
		PostEventLog.TrackMap("resources_initialized", SDKManager.AddBILaunchTimeProperty());
		if (StartupConfig.Inst.resInitdLoadRightnow)
		{
			LoadingStart();
		}
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void ReloadGameInOtherProcess()
	{
		Log.Info("ReloadGameInOtherProcess");
		if (StringUtils.VersionCompare(GameEntry.Sdk.Version, "1.0.170") >= 0 || CommonUtils.IsDebug())
		{
			string gameSessionId = GameEntry.Setting.gameSessionId;
			Log.Info("GameEntry::Set restart data:" + gameSessionId);
			GameEntry.Sdk.Restart(gameSessionId);
		}
		else
		{
			Quit();
		}
	}

	public void ReloadGame()
	{
		PostEventLog.Track("reload_game", null);
		isReloadGame = true;
		reloadGameCount++;
		StopInGameCheckResVersion();
		GameEntry.Setting?.Save();
		string stackTrace = Environment.StackTrace;
		Log.Info("Reload Game!   " + stackTrace);
	}

	private void SafeReload(Action reloadFunc)
	{
		if (reloadFunc == null)
		{
			return;
		}
		try
		{
			reloadFunc?.Invoke();
		}
		catch (Exception ex)
		{
			Log.Error("Reload error: {0} \n stack:{1}", ex.Message, ex.StackTrace);
		}
	}

	private void ReloadGameImpl()
	{
		SafeReload(GameEntry.Lua.ExitGame);
		SafeReload(SceneManager.Destroy);
		SafeReload(GameEntry.Event.Shutdown);
		SafeReload(GameEntry.Network.Shutdown);
		SafeReload(GameEntry.Timer.Shutdown);
		SafeReload(GameEntry.Sdk.Shutdown);
		SafeReload(GameEntry.BuildAnimatorManager.Shutdown);
		SafeReload(GameEntry.Sound.StopAllSounds);
		SafeReload(SingletonBehaviour<GameObjectPool>.Instance.ClearPool);
		SafeReload(GameEntry.Data.Reset);
		SafeReload(GameEntry.GlobalData.Reset);
		SafeReload(GameEntry.Resource.Clear);
		SafeReload(GameEntry.ConfigCache.reset);
		SafeReload(GameEntry.Lua.Shutdown);
		SafeReload(ChatService.Instance.Shutdown);
		SafeReload(GameEntry.NetworkCross.Shutdown);
		SafeReload(PVEBattleLogicManager.Dispose);
		SafeReload(PVELogManager.Instance.ClearAll);
		SafeReload(TimelineInteractionManager.Inst.Clear);
		SafeReload(TimelineAudioManager.Inst.Clear);
		SafeReload(SoundResourceDownloadManager.Instance.UnInit);
		SafeReload(GameEntry.Localization.Shutdown);
		GMSwitch.Reload();
		if (Loading != null)
		{
			SafeReload(StopAllDownloadTask);
		}
		if (Loading != null)
		{
			SafeReload(Loading.ExitCurState);
		}
		Log.Info("ReloadGameImpl finish");
		ResetStep();
		ChooseDebugUpdateFunc(PostReloadGameImpl);
	}

	private void PostReloadGameImpl()
	{
		if (kParallelInit)
		{
			StartParallelInitTask();
		}
		Loading.Start(isReload: true, showLogo: false);
	}

	private void StopAllDownloadTask()
	{
		Log.Info("ApplicationLaunch::StopAllDownloadTask");
		Download.ClearAllDownloads();
	}

	public void ReloadGameCheckResVerion()
	{
		ReloadGame();
	}

	public void ReStartGame()
	{
		ReloadGame();
	}

	private void OnApplicationPause(bool pause)
	{
		if (GameEntry.GlobalData != null)
		{
			GameEntry.GlobalData.isInBackGround = pause;
		}
		ForegroundTimer.OnApplicationPause(pause);
		if (pause)
		{
			ApplicationDidEnterBackground();
		}
		else
		{
			ApplicationWillEnterForeground();
		}
		GlobalMonobehaviourDispatcher.instance.OnApplicationPauseListener(pause);
		AnoProxy.AnoSetGamestatus(pause);
	}

	private bool IsNeedReConnect()
	{
		if (SceneManager.IsSceneNone())
		{
			return false;
		}
		if (Loading.IsLoading)
		{
			return false;
		}
		if (GameEntry.GlobalData.pushOffWithQuitGame)
		{
			return false;
		}
		long localSeconds = GameEntry.Timer.GetLocalSeconds();
		if (!GameEntry.Network.IsConnected || localSeconds - enterBackGroundTime >= 60)
		{
			return true;
		}
		return false;
	}

	private bool IsNeedReload()
	{
		if (GameEntry.Timer != null)
		{
			long localSeconds = GameEntry.Timer.GetLocalSeconds();
			if (enterBackGroundTime > 0 && localSeconds - enterBackGroundTime >= 180)
			{
				return true;
			}
		}
		return false;
	}

	private void ApplicationDidEnterBackground()
	{
		PostEventLog.Track("application_did_enter_background", null);
		if (GameEntry.Lua?.UIManager != null && GameEntry.Lua.UIManager.IsWindowOpen("UIChatNew"))
		{
			GameEntry.Lua.UIManager.DestroyWindow("UIChatNew");
		}
		enterBackGroundTime = GameEntry.Timer?.GetLocalSeconds() ?? 0;
		GameEntry.Setting?.Save();
		if (GameEntry.Event != null)
		{
			GameEntry.Event.Fire(EventId.APP_APPLICATION_PAUSE, true);
		}
	}

	private void ApplicationWillEnterForeground()
	{
		Log.Info("ApplicationWillEnterForeground~~~~~");
		PostEventLog.Track("application_will_enter_foreground", null);
		PushNoticeManager.pushDataToHttpServer();
		if (GameEntry.Sdk != null)
		{
			GameEntry.Sdk.ApplicationWillEnterForeground();
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Invoke("DoAfterEnterForeground", 0.5f);
		}
		else
		{
			DoAfterEnterForeground();
		}
	}

	private void DoAfterEnterForeground()
	{
		if (IsNeedReload())
		{
			Log.Info("Need Reload");
			DoInGameReload();
		}
		else if (IsNeedReConnect())
		{
			Log.Info("Need ReConnect");
			GameEntry.Network.SyncPingPong(0);
			DisconnectRetry();
		}
		else
		{
			try
			{
				if (Instance.Loading.currState == LoadingState.EnterGame && GameEntry.Sdk != null)
				{
					GameEntry.Sdk.SendDataToNative("Pay_queryPurchase", "");
				}
			}
			catch
			{
			}
		}
		if (GameEntry.Event != null)
		{
			GameEntry.Event.Fire(EventId.APP_APPLICATION_PAUSE, false);
		}
	}

	private void DoInGameReload()
	{
		try
		{
			LoadingState currState = Loading.currState;
			if (currState >= LoadingState.ConnectGame && currState <= LoadingState.LoadingError && GameEntry.Setting.GetInt("FUN_BUILD_MAIN_LEVEL", 0) >= 10)
			{
				InGameCheckResVersionTools inGameCheckResVersionTools = new InGameCheckResVersionTools();
				if (inGameCheckResVersionTools.Start(InGameCheckResVersionResult))
				{
					Log.Info("InGameCheck ResVersion");
					_checkResVersionTools = inGameCheckResVersionTools;
					GameEntry.Network.SyncPingPong(0);
					DisconnectRetry();
					return;
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error("DoInGameReload Expception " + ex.Message + " " + ex.StackTrace);
		}
		Log.Info("Need reload");
		ReloadGame();
	}

	private void InGameCheckResVersionResult(InGameCheckResVersionTools.CheckResult result, string reason)
	{
		switch (result)
		{
		case InGameCheckResVersionTools.CheckResult.NoGateway:
			return;
		case InGameCheckResVersionTools.CheckResult.Success_NoNeedUpdate:
			Log.Info("InGameCheckResVersionTools NoNeedUpdate");
			Instance.StopInGameCheckResVersion();
			return;
		}
		if ((CommonUtils.IsDebug() || GameEntry.Resource.SkipUpdateBundle) && result == InGameCheckResVersionTools.CheckResult.Success_NeedUpdate)
		{
			Log.Info("InGameCheckResVersionTools NoNeedUpdate");
			Instance.StopInGameCheckResVersion();
		}
		else
		{
			Log.Info("InGameCheckResVersionTools Reload!! " + reason);
			Instance.ReloadGame();
		}
	}

	private void StopInGameCheckResVersion()
	{
		if (_checkResVersionTools != null)
		{
			Log.Info("StopInGameCheckResVersion");
			_checkResVersionTools.Dispose();
			_checkResVersionTools = null;
		}
	}

	private void CheckOffline()
	{
		if (!GameEntry.GlobalData.pushOffWithQuitGame && !Loading.IsLoading && !CrossServerUtil.IsCrossing() && ((GameEntry.Network.Logined && GameEntry.Network.IsPingPongTimeOut) || !GameEntry.Network.isNetworkValid))
		{
			PostEventLog.Record("DISCONNECT_RETRY", "PingPongTimeOut");
			GameEntry.Network.SyncPingPong(0);
			GameEntry.Network.Disconnect();
			DisconnectRetry();
		}
	}

	public void DisconnectRetry()
	{
		try
		{
			if (GameEntry.Lua.UIManager.IsWindowOpen("UIDisconnect") || GameEntry.Lua.UIManager.IsWindowOpen("UIExitGameTip"))
			{
				return;
			}
			if (GameEntry.Lua.UIManager.IsWindowOpen("UICrossDisconnect"))
			{
				GameEntry.Event.Fire(EventId.CloseCrossDisconnectView);
			}
		}
		catch (Exception)
		{
		}
		disconnectRetryCount++;
		GameEntry.OnDisconnectRetry();
		Log.Info("ApplicationLaunch::DisconnectRetry");
		GameEntry.Lua.UIManager.OpenWindow("UIDisconnect", "TopMost");
		PostEventLog.Track("disconnect_retry", null);
	}

	private void OnLowMemory()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("OnLowMemory");
		stringBuilder.AppendLine("TempAllocatorSize " + Profiler.GetTempAllocatorSize());
		stringBuilder.AppendLine("MonoHeapSize " + Profiler.GetMonoHeapSizeLong());
		stringBuilder.AppendLine("MonoUsedSize " + Profiler.GetMonoUsedSizeLong());
		stringBuilder.AppendLine("TotalAllocatedMemory " + Profiler.GetTotalAllocatedMemoryLong());
		stringBuilder.AppendLine("TotalUnusedReservedMemory " + Profiler.GetTotalUnusedReservedMemoryLong());
		stringBuilder.AppendLine("TotalReservedMemory " + Profiler.GetTotalReservedMemoryLong());
		stringBuilder.AppendLine("AllocatedMemoryForGraphicsDriver " + Profiler.GetAllocatedMemoryForGraphicsDriver());
		Log.Info(stringBuilder.ToString());
		try
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("TempAllocatorSize", Profiler.GetTempAllocatorSize());
			dictionary.Add("MonoHeapSize", Profiler.GetMonoHeapSizeLong());
			dictionary.Add("MonoUsedSize", Profiler.GetMonoUsedSizeLong());
			dictionary.Add("TotalAllocatedMemory", Profiler.GetTotalAllocatedMemoryLong());
			dictionary.Add("TotalUnusedReservedMemory", Profiler.GetTotalUnusedReservedMemoryLong());
			dictionary.Add("TotalReservedMemory", Profiler.GetTotalReservedMemoryLong());
			dictionary.Add("AllocatedMemoryForGraphicsDriver", Profiler.GetAllocatedMemoryForGraphicsDriver());
			PostEventLog.TrackMap("on_low_memory", dictionary);
		}
		catch (Exception)
		{
			PostEventLog.Track("on_low_memory", null);
		}
	}

	protected void ConfigBastHttp()
	{
		BestHTTPAdapter.Configure();
	}

	private void ConfigureRemoteReport()
	{
		ClientInfo clientInfo = new ClientInfo();
		clientInfo.guid = GameUtility.GetGameSessionId();
		clientInfo.clientVer = GameEntry.Sdk.Version + "." + GameEntry.Sdk.VersionCode;
		clientInfo.packVer = GameEntry.Resource.GetResVersion();
		clientInfo.store = Application.identifier;
		clientInfo.country = GameEntry.GlobalData.fromCountry;
		clientInfo.device = SystemInfo.deviceName;
		clientInfo.model = SystemInfo.deviceModel;
		string deviceUid_Transcoding = GameEntry.Device.GetDeviceUid_Transcoding();
		string text = GameEntry.Setting.GetString("Setting.GAME_UID", "");
		string text2 = GameEntry.Setting.GetString("SERVER_ZONE", "");
		clientInfo.deviceId = deviceUid_Transcoding;
		clientInfo.runtime = GameUtility.GetPlatformName();
		clientInfo.system = SystemInfo.operatingSystem;
		string text3 = "https://lw-c-log.lastwarapp.com/logstores/client/track";
		FibMatrix.Logger.ConfigureRemoteLoggerTarget(text3, text, text2, clientInfo);
		FibMatrix.Logger.AppendRemoteTarget();
		if (new HashSet<string> { "1075104887000018", "1382671792000688", "1556653404001642", "1174378596001642", "1354585319000628" }.Contains(text))
		{
			FibMatrix.Logger.AppendTarget(FileLoggerTarget.InitAndUpload(text, clientInfo.guid, AESHelper.GetMd5Hash));
			GrayUtils.hasFileLog = true;
		}
		Log.Info("BIConfig::clientInfo.guid = " + clientInfo.guid + "  clientInfo.clientVer = " + clientInfo.clientVer + "  clientInfo.packVer = " + clientInfo.packVer + "  clientInfo.store = " + clientInfo.store + "  clientInfo.country = " + clientInfo.country + "  clientInfo.device = " + clientInfo.device + "  clientInfo.model = " + clientInfo.model + "  clientInfo.deviceId = " + clientInfo.deviceId + "  clientInfo.runtime = " + clientInfo.runtime + "  clientInfo.system = " + clientInfo.system + "  uid = " + text + "  zone = " + text2 + "  targetUrl = " + text3 + "  postUrl = " + BIConfig.postUrl);
		try
		{
			if (!_isRegistered)
			{
				Debug.unityLogger.logHandler = new CustomLogHandler();
				Application.logMessageReceivedThreaded += RemoteLogHandler;
				AppDomain.CurrentDomain.UnhandledException += RemoteUncaughtExceptionHandler;
				_isRegistered = true;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
		}
	}

	private void RemoteLogHandler(string logString, string stackTrace, LogType type)
	{
		switch (type)
		{
		case LogType.Log:
		{
			Dictionary<string, string> context2 = new Dictionary<string, string> { { "stackTrace", stackTrace } };
			FibMatrix.Logger.Info(logString, context2, LogTargetType.Runtime | LogTargetType.Network);
			break;
		}
		case LogType.Warning:
			FibMatrix.Logger.Warning(logString, stackTrace);
			break;
		case LogType.Error:
		case LogType.Assert:
		case LogType.Exception:
		{
			Dictionary<string, string> context = new Dictionary<string, string> { { "stackTrace", stackTrace } };
			FibMatrix.Logger.Error(logString, context);
			break;
		}
		}
	}

	private void RemoteUncaughtExceptionHandler(object sender, UnhandledExceptionEventArgs args)
	{
		if (args == null || args.ExceptionObject == null)
		{
			return;
		}
		try
		{
			if (args.ExceptionObject.GetType() != typeof(Exception))
			{
				return;
			}
		}
		catch
		{
			return;
		}
		Exception ex = (Exception)args.ExceptionObject;
		FibMatrix.Logger.Error(ex.Message, ex.StackTrace);
	}

	private void DisposeRemoteReportHandler()
	{
		try
		{
			if (_isRegistered)
			{
				Application.logMessageReceivedThreaded -= RemoteLogHandler;
				AppDomain.CurrentDomain.UnhandledException -= RemoteUncaughtExceptionHandler;
			}
			_isRegistered = false;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
		}
	}

	private void ConfigBI()
	{
		Dictionary<string, object> initInfoDict = new Dictionary<string, object>
		{
			{
				"app_id",
				(object)"502"
			},
			{
				"platform",
				(object)GameUtility.GetPlatformName()
			},
			{
				"channel",
				(object)null
			},
			{
				"country",
				(object)GameEntry.GlobalData.fromCountry
			},
			{
				"isCN",
				(object)false
			},
			{
				"app_version",
				(object)(GameEntry.Sdk.Version ?? "")
			},
			{
				"pf_version",
				(object)GameEntry.Resource.GetResVersion()
			},
			{
				"device_id",
				(object)GameEntry.Device.GetDeviceUid()
			},
			{
				"airKey",
				(object)GameEntry.Device.GetDeviceUid_Transcoding()
			}
		};
		Log.Info("Init BI.ConfigBI");
		string value = GameEntry.Setting.GetString("Setting.GAME_UID", "");
		string value2 = GameEntry.Setting.GetString("SERVER_ZONE", "");
		BIManager.InitBI(initInfoDict, string.IsNullOrEmpty(value));
		if (!string.IsNullOrEmpty(value))
		{
			BIManager.UpdateGameInfo(new Dictionary<string, object>(2)
			{
				{ "uid", value },
				{ "sid", value2 }
			});
		}
		GlobalMonobehaviourDispatcher.onApplicationQuit += BIManager.Dispose;
		GlobalMonobehaviourDispatcher.onApplicationPause += BIManager.OnApplicationPaused;
	}

	public static void UpdateTrackingResVersion()
	{
		string resVersion = GameEntry.Resource.GetResVersion();
		FibMatrix.Logger.CurrentRemoteLoggerTarget?.UpdateResVersion(resVersion);
		BIManager.UpdateResVersion(resVersion);
	}
}
