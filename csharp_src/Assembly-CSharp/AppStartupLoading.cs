using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using RiverGame.PerformanceAnalysis;
using UnityEngine;
using VEngine;

public class AppStartupLoading
{
	private LoadingState _currState;

	protected LoadingStateBase _loadingState;

	protected LoadingStateBase[] _stateList = new LoadingStateBase[23];

	private float _currentMaxProgress;

	private float _loadingProgress;

	private InstanceRequest _uiloadingInst;

	private Asset bgAsset;

	private Asset _uiloadingAsset;

	public UILoadingComponent UILoading;

	public bool IsShowServerList;

	private const float PromptBundleUpdateBytes = 5242880f;

	private float _bundleDownloadTotalBytes;

	private float _bundleDownloadProgress;

	public bool PermissionRecv;

	public bool isCanCloseLoading;

	public bool isErrorSuccess;

	private bool _isPushInitReceived;

	public const int LoginMaxTryCount = 3;

	public bool isZipModeFinish;

	private int _port;

	private string _ip = string.Empty;

	private string _wsIp = string.Empty;

	private string _zone = string.Empty;

	private string _uid = string.Empty;

	private int _connectionType;

	private bool willDelayProcess;

	private int postStartDelay;

	private int postStartFrameCount;

	private string _privacyKeyV1 = "PrivacyConfirm_0305";

	private bool _cacheIsReload;

	private bool _cacheShowLogo;

	private float oldMaxUpdateTimeSlice;

	private bool oldUseAllTimeSlice;

	private ThreadPriority oldThreadConfig;

	private Coroutine waitEndOfFrameCoroutine;

	public LoadingState currState => _currState;

	public float BundleDownloadTotalBytes
	{
		get
		{
			return _bundleDownloadTotalBytes;
		}
		set
		{
			_bundleDownloadTotalBytes = value;
		}
	}

	public float BundleDownloadProgress
	{
		get
		{
			return _bundleDownloadProgress;
		}
		set
		{
			_bundleDownloadProgress = value;
		}
	}

	public float LoadingProgress => _currentMaxProgress;

	public bool IsPushInitReceived
	{
		get
		{
			return _isPushInitReceived;
		}
		set
		{
			if (_isPushInitReceived != value)
			{
				Log.Info($"[Loading] modify IsPushInitReceived ->{value}");
			}
			_isPushInitReceived = value;
		}
	}

	public bool IsNeedIdentification { get; set; }

	public bool IsChild { get; set; }

	public int LoginTryCount { get; set; }

	public bool IsLoading => _currState != LoadingState.EnterGame;

	public bool updateAfterLogin { get; private set; }

	public bool checkResVersionError { get; private set; }

	public void ClearUpdateAfterLogin(bool value)
	{
		updateAfterLogin = value;
		checkResVersionError = false;
		_port = 0;
		_ip = string.Empty;
		_wsIp = string.Empty;
		_zone = string.Empty;
		_uid = string.Empty;
		_connectionType = 0;
	}

	public AppStartupLoading()
	{
		_stateList[1] = new LogoState(this);
		_stateList[2] = new PermissionState(this);
		_stateList[4] = new CheckResVersionState(this);
		_stateList[5] = new DownloadManifestState(this);
		_stateList[6] = new DownloadUpdateState(this);
		_stateList[7] = new LoadDataTableState(this);
		_stateList[8] = new GetServerListState(this);
		_stateList[9] = new GetServerStatusState(this);
		_stateList[21] = new AccountSelectState(this);
		_stateList[10] = new ConnectGameState(this);
		_stateList[11] = new LoginState(this);
		_stateList[12] = new AppUpdateState(this);
		_stateList[13] = new PushInitState(this);
		_stateList[14] = new AuthPinState(this);
		_stateList[15] = new CNIdentifyState(this);
		_stateList[16] = new LoadSceneState(this);
		_stateList[17] = new EnterGameState(this);
		_stateList[18] = new LoadingErrorState(this);
		_stateList[19] = new MaintenanceState(this);
		_stateList[20] = new CreditLimitState(this);
		_stateList[22] = new KRAuthState(this);
	}

	public void Start(bool isReload, bool showLogo)
	{
		if (isReload)
		{
			StartupConfig.Inst.SwitchOptOffAll();
		}
		if (ApplicationLaunch.kParallelInit)
		{
			StartupConfig.Inst.delayLuaInit = true;
		}
		_cacheIsReload = isReload;
		_cacheShowLogo = showLogo;
		IsShowServerList = CommonUtils.IsDebug() && (!isReload || NetworkURLConfig.IsChangeDebugURLGroup || LoadingDebugTool.IsShowServerList) && GameEntry.Setting.GetBool("SHOW_DEBUG_CHOOSE_SERVER", defaultValue: true);
		Log.Info(string.Format("IsShowServerList: {0} {1} {2}", IsShowServerList, CommonUtils.IsDebug(), GameEntry.Setting.GetBool("SHOW_DEBUG_CHOOSE_SERVER", defaultValue: true)));
		IsPushInitReceived = false;
		IsNeedIdentification = false;
		isErrorSuccess = false;
		LoginTryCount = 0;
		_bundleDownloadTotalBytes = 0f;
		_bundleDownloadProgress = 0f;
		_loadingProgress = 0f;
		_currentMaxProgress = 0f;
		InitSoundSetting();
		SetDelayLua();
		Loadable.reverseState = StartupConfig.Inst.reverseAddLoadable;
		if (!StartupConfig.Inst.delayLuaInit)
		{
			BeforeOpenUI();
		}
		CloseUILoading();
		OpenUILoading(!isReload && showLogo);
		if (!StartupConfig.Inst.delayLuaInit)
		{
			AfterOpenUI();
		}
	}

	private void SetDelayLua()
	{
		willDelayProcess = false;
	}

	private void BeforeOpenUI()
	{
		ApplicationLaunch.StepLog("[ParallelInit] BeforeOpenUI");
		if (!ApplicationLaunch.kParallelInit)
		{
			ClientConfig.CheckDataFileEnv();
			ClientConfig.CheckDataFileOnAppStart();
			ApplicationLaunch.StepLog("CheckDataFileOnAppStart");
		}
		if (!ApplicationLaunch.kParallelInit)
		{
			ClientConfig.CheckLocaleFileOnAppStart(GameEntry.Setting.UserLanguage);
			ApplicationLaunch.StepLog("CheckLocaleFileOnAppStart");
		}
		if (!ApplicationLaunch.kParallelInit)
		{
			LWLuaFileUpdate.InitFileOnAppStart();
			ApplicationLaunch.StepLog("LWLuaFileUpdate.InitFileOnAppStart");
		}
		GameEntry.Lua.Initialize();
		if (!XLuaManager.DelayLuaStartGame)
		{
			GameEntry.Lua.StartGame();
		}
		ApplicationLaunch.StepLog("XLua Initialized");
		if (!ApplicationLaunch.kParallelInit)
		{
			GameEntry.Localization.Initialize(GameEntry.Setting.UserLanguage);
			GameEntry.Localization.ClearFontDynamicData();
			GameEntry.Localization.LoadDictionary("Dialog");
			ApplicationLaunch.StepLog("Localization.Initialize");
		}
		else
		{
			GameEntry.Localization.Initialize(GameEntry.Setting.UserLanguage);
			GameEntry.Localization.ClearFontDynamicData();
			GameEntry.Localization.UseSwapLocaleFile();
			ApplicationLaunch.StepLog("Localization.Initialize kParallelInit");
		}
	}

	private void AfterOpenUI()
	{
		try
		{
			ShowUIPrivacy(_cacheIsReload, _cacheShowLogo);
		}
		catch (Exception arg)
		{
			StartOK(_cacheIsReload, _cacheShowLogo);
			Log.Error($"show UIPrivacy exception {arg}");
		}
	}

	private void StartOK(bool isReload, bool showLogo)
	{
		if (isReload)
		{
			SetState(LoadingState.Permission);
		}
		else if (showLogo)
		{
			SetState(LoadingState.Permission);
		}
		else
		{
			SetState(LoadingState.CheckResVersion);
		}
		Log.Info("Loading Start ok!");
	}

	private void ShowUIPrivacy(bool isReload, bool showLogo)
	{
		Log.Info("ShowUIPrivacy Country:" + GameEntry.GlobalData.fromCountry);
		GameEntry.Event.Unsubscribe(EventId.UIPrivacy_Cancel, OnPrivacyCancel);
		GameEntry.Event.Unsubscribe(EventId.UIPrivacy_Confirm, OnPrivacyConfirm);
		PrivacyFuncUtil.Instance.SignCoppaNewPlayer();
		int num = ShowDMA();
		if (num == 0)
		{
			StartOK(isReload, showLogo);
			return;
		}
		GameEntry.Event.Subscribe(EventId.UIPrivacy_Cancel, OnPrivacyCancel);
		GameEntry.Event.Subscribe(EventId.UIPrivacy_Confirm, OnPrivacyConfirm);
		ClosePrivacyView();
		PrivacyFuncUtil.Instance.ShowPrivacy(num);
	}

	private int ShowDMA()
	{
		int num = 0;
		num = PrivacyFuncUtil.Instance.CanShowDMA();
		Log.Info($"ShowUIPrivacy? {num}");
		return num;
	}

	private void OnPrivacyConfirm(object obj)
	{
		GameEntry.Event.Unsubscribe(EventId.UIPrivacy_Cancel, OnPrivacyCancel);
		GameEntry.Event.Unsubscribe(EventId.UIPrivacy_Confirm, OnPrivacyConfirm);
		GameEntry.Setting.SetBool(_privacyKeyV1, value: true);
		PrivacyFuncUtil.Instance.SavePrivacyKey();
		StartOK(_cacheIsReload, _cacheShowLogo);
	}

	private void OnPrivacyCancel(object obj)
	{
		GameEntry.Event.Unsubscribe(EventId.UIPrivacy_Cancel, OnPrivacyCancel);
		GameEntry.Event.Unsubscribe(EventId.UIPrivacy_Confirm, OnPrivacyConfirm);
		ApplicationLaunch.Instance.Quit();
	}

	public void Shutdown()
	{
		CloseUILoading();
		ClosePrivacyView();
	}

	private void OpenUILoading(bool showLogo)
	{
		if (StartupConfig.Inst.earlyHideSplash)
		{
			GameEntry.Sdk.HideSplash();
		}
		PerformanceMetrics.Metric<int> deviceLevel = PerformanceMetrics.DeviceLevel;
		PostEventLog.TrackMap("open_loading_ui", SDKManager.AddBILaunchTimeProperty(StartupConfig.Inst.AddBIProperty(new Dictionary<string, object>
		{
			{ "pd_dl", deviceLevel.Value },
			{
				"DelayLuaStartGame",
				XLuaManager.DelayLuaStartGame
			}
		})));
		bool syncUI = StartupConfig.Inst.syncUI;
		bool asyncLoadUI = StartupConfig.Inst.asyncLoadUI;
		ApplicationLaunch.StepLog($"OpenUILoading {syncUI} {asyncLoadUI}");
		if (syncUI)
		{
			_uiloadingAsset = GameEntry.Resource.LoadAsset("Assets/Main/Loading/Prefabs/UILoading_Base.prefab", typeof(GameObject));
			ApplicationLaunch.StepLog("UILoading Asset Loaded, sync");
			OnPrefabInstComplete(UnityEngine.Object.Instantiate(_uiloadingAsset.asset as GameObject), showLogo, isSync: true);
		}
		else if (asyncLoadUI)
		{
			BeforeAsyncLoadUI();
			_uiloadingAsset = GameEntry.Resource.LoadAssetAsync("Assets/Main/Loading/Prefabs/UILoading_Base.prefab", typeof(GameObject));
			Asset uiloadingAsset = _uiloadingAsset;
			uiloadingAsset.completed = (Action<Asset>)Delegate.Combine(uiloadingAsset.completed, (Action<Asset>)delegate(Asset prefab)
			{
				ApplicationLaunch.StepLog("UILoading Asset Loaded, async");
				AfterAsyncLoadUI();
				OnPrefabInstComplete(UnityEngine.Object.Instantiate(prefab.asset as GameObject), showLogo, isSync: false);
			});
		}
		else
		{
			BeforeAsyncLoadUI();
			_uiloadingInst = GameEntry.Resource.InstantiateAsyncImmediately("Assets/Main/Loading/Prefabs/UILoading_Base.prefab");
			_uiloadingInst.completed += delegate
			{
				ApplicationLaunch.StepLog("UILoading Asset Loaded, async immediately");
				AfterAsyncLoadUI();
				OnPrefabInstComplete(_uiloadingInst.gameObject, showLogo, isSync: false);
			};
		}
		if (StartupConfig.Inst.reverseAddLoadable)
		{
			Loadable.reverseState = false;
		}
	}

	private void BeforeAsyncLoadUI()
	{
		Log.Info($"BeforeAsyncLoadUI {StartupConfig.Inst.useLongSlice}");
		oldMaxUpdateTimeSlice = Updater.maxUpdateTimeSlice;
		if (StartupConfig.Inst.useLongSlice)
		{
			Updater.maxUpdateTimeSlice = StartupConfig.Inst.longSliceMillSecond;
		}
		oldUseAllTimeSlice = Updater.useAllTimeSlice;
		Updater.useAllTimeSlice = StartupConfig.Inst.useAllSlice;
		oldThreadConfig = Application.backgroundLoadingPriority;
		if (StartupConfig.Inst.hideBackgroundPrior)
		{
			Application.backgroundLoadingPriority = ThreadPriority.High;
		}
	}

	private void AfterAsyncLoadUI()
	{
		Updater.maxUpdateTimeSlice = oldMaxUpdateTimeSlice;
		Updater.useAllTimeSlice = oldUseAllTimeSlice;
		Application.backgroundLoadingPriority = oldThreadConfig;
	}

	private void OnPrefabInstComplete(GameObject go, bool showLogo, bool isSync)
	{
		PostEventLog.TrackMap("on_loading_ui_load", SDKManager.AddBILaunchTimeProperty(StartupConfig.Inst.AddBIProperty()));
		ApplicationLaunch.StepLog($"OnPrefabInstComplete, is sync {isSync}");
		if (StartupConfig.Inst.lateHideSplash)
		{
			HideSplashEndOfFrame();
		}
		if (StartupConfig.Inst.delayLuaInit)
		{
			willDelayProcess = true;
			if (Time.frameCount == 0)
			{
				postStartDelay = 2;
			}
			else
			{
				postStartDelay = 1;
			}
			postStartFrameCount = Time.frameCount;
		}
		UILoading = go.GetComponent<UILoadingComponent>();
		SetUILoadingPosition();
		UILoading.CSOpen(showLogo, !StartupConfig.Inst.delayLuaInit);
	}

	private void SetUILoadingPosition()
	{
		RectTransform component = UILoading.gameObject.GetComponent<RectTransform>();
		Transform transform = GameEntry.UIContainer.Find("Normal");
		RectTransform rectTransform = null;
		if (transform != null)
		{
			rectTransform = transform.GetComponent<RectTransform>();
		}
		if (rectTransform == null)
		{
			rectTransform = new GameObject("Normal", typeof(RectTransform)).GetComponent<RectTransform>();
			rectTransform.gameObject.layer = LayerMask.NameToLayer("UI");
			rectTransform.SetParent(GameEntry.UIContainer, worldPositionStays: false);
			rectTransform.localScale = Vector3.one;
			rectTransform.offsetMin = Vector3.zero;
			rectTransform.offsetMax = Vector3.zero;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			Transform transform2 = GameEntry.UIContainer.Find("Dialog");
			if (transform2 != null)
			{
				transform2.SetAsLastSibling();
			}
		}
		component.SetParent(rectTransform, worldPositionStays: false);
		component.localScale = Vector3.one;
		component.offsetMin = Vector3.zero;
		component.offsetMax = Vector3.zero;
		component.anchorMin = Vector2.zero;
		component.anchorMax = Vector2.one;
		component.pivot = new Vector2(0.5f, 0.5f);
		component.SetAsLastSibling();
	}

	public void CloseUILoading()
	{
		Log.Info("CloseUILoading");
		if (UILoading != null)
		{
			UILoading.CSClose(null);
			if (_uiloadingInst == null)
			{
				UnityEngine.Object.Destroy(UILoading.gameObject);
				if (_uiloadingAsset != null)
				{
					_uiloadingAsset.Release();
				}
			}
			UILoading = null;
		}
		if (_uiloadingInst != null)
		{
			_uiloadingInst.Destroy();
			_uiloadingInst = null;
		}
		if (bgAsset != null)
		{
			bgAsset.Release();
			bgAsset = null;
		}
	}

	public void ClosePrivacyView()
	{
		UIPrivacyView.Instance.ClosePrivacyView();
		UIPrivacyKRView.Instance.ClosePrivacyView();
	}

	public static float GetCurrStateProgressValue()
	{
		return ApplicationLaunch.Instance.Loading.GetCurrStateProgress();
	}

	private float GetCurrStateProgress()
	{
		return _currState switch
		{
			LoadingState.None => 0f, 
			LoadingState.Logo => 0.0001f, 
			LoadingState.Permission => 0.05f, 
			LoadingState.CheckResVersion => 0.1f, 
			LoadingState.DownloadManifest => 0.3f, 
			LoadingState.DownloadUpdate => 0.4f, 
			LoadingState.LoadDataTable => 0.5f, 
			LoadingState.GetServerList => 0.2f, 
			LoadingState.GetServerStatus => 0.55f, 
			LoadingState.AccountSelect => 0.55f, 
			LoadingState.ConnectGame => 0.6f, 
			LoadingState.Login => 0.7f, 
			LoadingState.KRAuth => 0.7f, 
			LoadingState.AppUpdate => 0.8f, 
			LoadingState.PushInit => 0.9f, 
			LoadingState.AuthPin => 0.9f, 
			LoadingState.CNIdentify => 0.9f, 
			LoadingState.LoadScene => 1f, 
			LoadingState.EnterGame => 1f, 
			LoadingState.Maintenance => 1f, 
			LoadingState.CreditLimit => 1f, 
			_ => 0f, 
		};
	}

	public void ReConnect()
	{
		IsShowServerList = false;
		StartConnect();
	}

	public void StartConnect()
	{
		Log.Info("StartConnect");
		if (GameEntry.Network.IsConnected)
		{
			GameEntry.Network.Disconnect();
		}
		IsPushInitReceived = false;
		if (!IsShowServerList && LoadGameServerSetting(out var ip, out var port, out var zone, out var uid, out var connectionType))
		{
			SetState(LoadingState.ConnectGame, ip, port, zone, uid, connectionType);
		}
		else
		{
			SetState(LoadingState.GetServerList);
		}
	}

	public void StartConnectGame()
	{
		Log.Info("StartConnectGame");
		if (GameEntry.Network.IsConnected)
		{
			GameEntry.Network.Disconnect();
		}
		IsPushInitReceived = false;
		SetState(LoadingState.ConnectGame, _ip, _port, _zone, _uid, _connectionType, _wsIp);
	}

	public void ToConnectGame(string ip, int port, string zone, string uid, int connectionType, string strRequiredPackages, string wsIp)
	{
		Log.Info("ToConnectGame");
		if (updateAfterLogin)
		{
			_ip = ip;
			_port = port;
			_zone = zone;
			_uid = uid;
			_wsIp = wsIp;
			_connectionType = connectionType;
			ResourcePackageManager.SetRequiredPackagesByGetServerList(strRequiredPackages);
			SetState(LoadingState.DownloadManifest);
		}
		else
		{
			SetState(LoadingState.ConnectGame, ip, port, zone, uid, connectionType, wsIp);
		}
	}

	public void ToDownloadManifest()
	{
		checkResVersionError = false;
		if (updateAfterLogin)
		{
			SetState(LoadingState.GetServerList);
		}
		else
		{
			SetState(LoadingState.DownloadManifest);
		}
	}

	public void ToLoadDataTable()
	{
		checkResVersionError = true;
		if (updateAfterLogin)
		{
			SetState(LoadingState.GetServerList);
			return;
		}
		SetState(LoadingState.LoadDataTable, false);
	}

	public LoadingStateBase GetState(LoadingState state)
	{
		return _stateList[(int)state];
	}

	public void SetState(LoadingState newState, params object[] args)
	{
		if (_currState != newState)
		{
			ApplicationLaunch.StepLog($"SetState {_currState} => {newState}");
			GameEntry.Event.Fire(EventId.LoadingState, (int)newState);
			try
			{
				_loadingState?.OnExit();
			}
			catch (Exception ex)
			{
				Log.Error(" app loading OnExit staste {0}_{1}", ex.Message, ex.StackTrace);
			}
			_currState = newState;
			_loadingState = GetState(_currState);
			try
			{
				_loadingState?.OnEnter(args);
				GameEntry.Event.Fire(EventId.UILOADING_STATE_TEXT_CHANGE);
			}
			catch (Exception ex2)
			{
				Log.Error(" app loading setState {0}_{1}", ex2.Message, ex2.StackTrace);
			}
		}
	}

	public void ExitCurState()
	{
		_loadingState?.OnExit();
		if (ApplicationLaunch.kParallelInit)
		{
			_loadingState = null;
			_currState = LoadingState.None;
		}
	}

	private IEnumerator EndOfFrameCoroutine()
	{
		if (Time.frameCount == 0)
		{
			yield return null;
		}
		yield return new WaitForEndOfFrame();
		GameEntry.Sdk.HideSplash();
		ApplicationLaunch.Instance.StopCoroutine(waitEndOfFrameCoroutine);
		waitEndOfFrameCoroutine = null;
	}

	public void HideSplashEndOfFrame()
	{
		if (waitEndOfFrameCoroutine != null)
		{
			ApplicationLaunch.Instance.StopCoroutine(waitEndOfFrameCoroutine);
			waitEndOfFrameCoroutine = null;
		}
		waitEndOfFrameCoroutine = ApplicationLaunch.Instance.StartCoroutine(EndOfFrameCoroutine());
	}

	public void Update()
	{
		if (ApplicationLaunch.kParallelInit)
		{
			if (StartupConfig.Inst.delayLuaInit && willDelayProcess && ApplicationLaunch.Instance.taskAllDone)
			{
				willDelayProcess = false;
				if (ApplicationLaunch.Instance.taskAllSucceed)
				{
					BeforeOpenUI();
					UILoading.OnStart();
					AfterOpenUI();
				}
				else
				{
					UILoading.safeModeObj.Show();
				}
			}
		}
		else if (StartupConfig.Inst.delayLuaInit && willDelayProcess && Time.frameCount - postStartFrameCount >= postStartDelay)
		{
			willDelayProcess = false;
			BeforeOpenUI();
			UILoading.OnStart();
			AfterOpenUI();
		}
		UpdateLoadingProgress();
		try
		{
			_loadingState?.OnUpdate();
		}
		catch (Exception ex)
		{
			Log.Error("app loading error {0}_{1}", ex.Message, ex.StackTrace);
		}
	}

	private void UpdateLoadingProgress()
	{
		if (_currState != LoadingState.DownloadUpdate && _currState != LoadingState.LoadingError && _currState != LoadingState.EnterGame)
		{
			float currStateProgress = GetCurrStateProgress();
			_loadingProgress = currStateProgress;
			if (_currentMaxProgress < _loadingProgress)
			{
				_currentMaxProgress = _loadingProgress;
			}
		}
	}

	public void OnInitError()
	{
		SetState(LoadingState.LoadingError, "E106");
	}

	public void OnAuthSuccess()
	{
		SetState(LoadingState.PushInit);
	}

	public void PreloadAssets()
	{
		GameEntry.Lua.PreloadAssets();
	}

	private void InitSoundSetting()
	{
		if (!GameEntry.Setting.HasSetting("isEffectMusicOn"))
		{
			GameEntry.Setting.SetBool("isEffectMusicOn", value: true);
		}
		if (!GameEntry.Setting.HasSetting("isBGMusicOn"))
		{
			GameEntry.Setting.SetBool("isBGMusicOn", value: true);
		}
		if (!GameEntry.Setting.HasSetting("ENV_SOUND_ON"))
		{
			bool value = GameEntry.Setting.GetBool("isEffectMusicOn");
			GameEntry.Setting.SetBool("ENV_SOUND_ON", value);
		}
		if (!GameEntry.Setting.HasSetting("isTaskTipsOn"))
		{
			GameEntry.Setting.SetBool("isTaskTipsOn", value: true);
		}
		bool flag = !GameEntry.Setting.GetBool("isEffectMusicOn");
		bool flag2 = !GameEntry.Setting.GetBool("isBGMusicOn");
		bool mute = !GameEntry.Setting.GetBool("ENV_SOUND_ON");
		GameEntry.Sound.SetSoundGroupMute("Effect", flag);
		GameEntry.Sound.SetSoundGroupMute("Dub", flag);
		GameEntry.Sound.SetSoundGroupMute("Music", flag2);
		GameEntry.Sound.SetSoundGroupMute("AMBSound", mute);
		GameEntry.Sound.SetSoundGroupMute("Hero", flag);
		PostEventLog.TrackMap("c_sound_effect_open", new Dictionary<string, object> { 
		{
			"param1",
			!flag
		} });
		PostEventLog.TrackMap("c_sound_music_open", new Dictionary<string, object> { 
		{
			"param1",
			!flag2
		} });
	}

	public bool LoadGameServerSetting(out string ip, out int port, out string zone, out string uid, out int connectionType)
	{
		ip = GameEntry.Setting.GetString("SERVER_IP", "");
		port = GameEntry.Setting.GetInt("SERVER_PORT", 0);
		zone = GameEntry.Setting.GetString("SERVER_ZONE", "");
		uid = GameEntry.Setting.GetString("Setting.GAME_UID", "");
		connectionType = GameEntry.Setting.GetInt("SERVER_CONNECTION_TYPE", 0);
		if (string.IsNullOrEmpty(ip) || port == 0 || string.IsNullOrEmpty(zone))
		{
			return false;
		}
		return true;
	}

	public void SaveGameServerSetting(string ip, int port, string zone, string uid, string uuid, int connectionType)
	{
		if (!string.IsNullOrEmpty(uid))
		{
			GameEntry.Setting.SetString("Setting.GAME_UID", uid);
		}
		GameEntry.Setting.SetString("SERVER_IP", ip);
		GameEntry.Setting.SetInt("SERVER_PORT", port);
		GameEntry.Setting.SetString("SERVER_ZONE", zone);
		GameEntry.Setting.SetInt("SERVER_CONNECTION_TYPE", connectionType);
		GameEntry.Setting.Save();
		Log.Info($"save server info {ip} {port} {zone} {uid} t:{connectionType}");
	}

	public void SaveGameLoginToken(LoginToken at, LoginToken rt)
	{
		if (at != null && !string.IsNullOrEmpty(at.token))
		{
			GameEntry.Setting.SetString("Login.access_token", at.token);
			GameEntry.Setting.SetInt("Login.access_token_time", at.time);
		}
		if (rt != null && !string.IsNullOrEmpty(rt.token))
		{
			GameEntry.Setting.SetString("Login.refresh_token", rt.token);
			GameEntry.Setting.SetInt("Login.refresh_token_time", rt.time);
		}
		GameEntry.Setting.Save();
	}

	public void ClearAccessToken()
	{
		Log.Info("Clear login at!");
		GameEntry.Setting.SetString("Login.access_token", "");
		GameEntry.Setting.SetInt("Login.access_token_time", 0);
	}

	public void ClearRefreshToken()
	{
		Log.Info("Clear login rt!");
		GameEntry.Setting.SetString("Login.refresh_token", "");
		GameEntry.Setting.SetInt("Login.refresh_token_time", 0);
	}

	public void ClearLoginKey()
	{
		Log.Info("Clear login key!");
		GameEntry.Setting.SetString("Login.login_key", "");
	}

	public void ClearAllAccountSettings()
	{
		Log.Info("Clear server info!");
		GameEntry.Setting.RemoveSetting("Setting.GAME_UID");
		GameEntry.Setting.RemoveSetting("SERVER_ZONE");
		GameEntry.Setting.RemoveSetting("SERVER_IP");
		GameEntry.Setting.RemoveSetting("SERVER_PORT");
	}

	public void ClearGameServerSetting(bool clearZone = false)
	{
		Log.Info($"Clear server info 2! {clearZone}");
		GameEntry.Setting.RemoveSetting("SERVER_IP");
		GameEntry.Setting.RemoveSetting("SERVER_PORT");
		GameEntry.Setting.RemoveSetting("SERVER_CONNECTION_TYPE");
		if (clearZone)
		{
			GameEntry.Setting.RemoveSetting("SERVER_ZONE");
		}
	}
}
