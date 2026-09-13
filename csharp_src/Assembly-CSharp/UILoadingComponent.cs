using System;
using AIHelp;
using GameFramework;
using GameFramework.Localization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;
using VEngine;
using XLua;

public class UILoadingComponent : MonoBehaviour
{
	private enum State
	{
		None,
		Download,
		Login
	}

	private class UILoadingInfo
	{
		public int[] Weights { get; set; }

		public string[] Prefabs { get; set; }

		public UILoadingInfo(int[] weights, string[] prefabs)
		{
			Weights = weights;
			Prefabs = prefabs;
		}
	}

	public TextMeshProUGUIEx versionText;

	public Image background;

	public VideoPlayer videoPlayer;

	public Animator animator;

	public Slider progressBar;

	public TextMeshProUGUIEx loadingText;

	public TextMeshProUGUIEx tipText;

	public TextMeshProUGUIEx downloadText;

	public Image logoImage;

	public GameObject wifiObj;

	public Button btnNotice;

	public TextMeshProUGUIEx txtNotice;

	public Button btnServe;

	public TextMeshProUGUIEx txtServe;

	public GameObject serverRedDot;

	public TextMeshProUGUIEx serverRedDotCount;

	public Button btnHelp;

	public TextMeshProUGUIEx txtHelp;

	public GameObject maintenanceDialog;

	public TextMeshProUGUIEx maintenanceDialogTitle;

	public TextMeshProUGUIEx maintenanceDialogContent;

	public Button maintenanceBtn;

	public TextMeshProUGUIEx maintenanceBtnText;

	public GameObject handleSliderArea;

	public GameObject koreanDecorate;

	public GameObject subLoadingContainer;

	public Button btnAccountSelect;

	public Button btnNewGame;

	public TextMeshProUGUIEx txtBtnLogin;

	public TextMeshProUGUIEx txtBtnNewGame;

	private const string LOG_TAG = "[UILoading] ";

	private bool showLogo;

	private bool isPlayingUIAnim;

	private float totalTime;

	private bool isRefreshLocalizationText;

	private bool localeTextInitd;

	private State state;

	private const string DEFAULT_KEY = "Update game data:{0}/{1}M";

	private string _updatekey = "";

	private const string PROGRESS_KEY = "progress: {0}%";

	private string _progresskey = "";

	private UnityWebRequest _noticeRequest;

	private float _elapseTime;

	private float _timeout = 3f;

	private int _maxTryCount = 3;

	private int _tryCount;

	private ISubLoadingComponent _subLoadingComponent;

	public Button btnClear;

	public TextMeshProUGUIEx btnClearText;

	public GameObject clearConfirm;

	public TextMeshProUGUIEx clearConfirmTitle;

	public TextMeshProUGUIEx clearConfirmContent;

	public Button btnClearConfirmOk;

	public Button btnClearConfirmNo;

	public TextMeshProUGUIEx btnClearConfirmOk_Text;

	public TextMeshProUGUIEx btnClearConfirmNo_Text;

	public UILoadingComponentSafeMode safeModeObj;

	private bool luaStateInited;

	private Asset _asset;

	private readonly string[] _arabicCountryList = new string[11]
	{
		"SA", "QA", "KW", "AE", "OM", "BH", "EG", "IQ", "JO", "LB",
		"MA"
	};

	private LuaTable scriptEnv;

	private Action luaOnStart;

	private Action luaOnDestroy;

	private Action<string> luaOnNoticeDataBack;

	private float loadingProgress;

	private float finishDuration = 0.1f;

	private float finishStartTime = -1f;

	private float elapsedTime;

	private float finishStartProgress;

	private string updatekey
	{
		get
		{
			if (string.IsNullOrEmpty(_updatekey))
			{
				_updatekey = GameEntry.Localization.GetString("129045");
				if (string.IsNullOrEmpty(_updatekey))
				{
					_updatekey = "Update game data:{0}/{1}M";
				}
			}
			return _updatekey;
		}
	}

	private string progresskey
	{
		get
		{
			if (string.IsNullOrEmpty(_progresskey))
			{
				_progresskey = GameEntry.Localization.GetString("limit_length_loading_tips_13");
				if (string.IsNullOrEmpty(_progresskey))
				{
					_progresskey = "progress: {0}%";
				}
			}
			return _progresskey;
		}
	}

	public void OnStart()
	{
		if (!luaStateInited)
		{
			InitLuaState(null);
			GameEntry.Event.Subscribe(EventId.ReInitLoadingLuaState, InitLuaState);
		}
		luaOnStart?.Invoke();
	}

	public void CSOpen(object userData, bool initLuaState = true)
	{
		safeModeObj?.Bind();
		InitLoading();
		luaStateInited = false;
		if (initLuaState)
		{
			InitLuaState(null);
		}
		GameEntry.Event.Subscribe(EventId.UILOADING_STATE_TEXT_CHANGE, OnLoadingStateTextChange);
		GameEntry.Event.Subscribe(EventId.BeginDownloadUpdate, OnStartBundleDownload);
		GameEntry.Event.Subscribe(EventId.EndDownloadUpdate, OnStartLogin);
		GameEntry.Event.Subscribe(EventId.NetworkRetry, OnNetworkRetry);
		if (initLuaState)
		{
			GameEntry.Event.Subscribe(EventId.ReInitLoadingLuaState, InitLuaState);
		}
		GameEntry.Sound.PlayLoadingBgMusic();
		if (!StartupConfig.Inst.earlyHideSplash && !StartupConfig.Inst.lateHideSplash)
		{
			GameEntry.Sdk.HideSplash();
		}
		if (initLuaState)
		{
			OnStart();
		}
	}

	private void InitLoading()
	{
		ApplicationLaunch.StepLog("UILoading CSOpen");
		Language language = GetLanguage();
		string text = "Assets/Main/Loading/Prefabs/UILoading_Normal.prefab";
		text = GetPrefabPath();
		DebugLog($"UILoading Language: {language} SubPrefabPath: {text}");
		if (_asset != null)
		{
			_asset.Release();
			_asset = null;
		}
		_asset = GameEntry.Resource.LoadAsset(text, typeof(GameObject));
		if (_asset != null && !_asset.isError)
		{
			GameObject subloading = _asset.asset as GameObject;
			InitSubLoading(subloading, language);
		}
		ApplicationLaunch.StepLog("UILoading CSOpen InitSubLoading");
	}

	private string GetPrefabPath()
	{
		if (string.IsNullOrEmpty(GameEntry.Setting.GetString("USER_REG_TIME", "")))
		{
			GameEntry.Setting.SetBool("AMERICA_NEW_LOADING", value: true);
		}
		string text = "Assets/Main/Loading/Prefabs/UILoading_Normal.prefab";
		int num = PlayerPrefs.GetInt("SEASON_MAP_TYPE");
		if (num != 0)
		{
			DownloadChecker downloadChecker = new DownloadChecker();
			downloadChecker.Collect(Versions.DownloadDataPath);
			if (num == 2 && ResourcePackageManager.IsSeasonResDownloadedByPackageId(1002, downloadChecker))
			{
				return "Assets/Main/Loading/Prefabs/UILoading_SeasonLondon.prefab";
			}
			if (num == 3 && ResourcePackageManager.IsSeasonResDownloadedByPackageId(1003, downloadChecker))
			{
				return "Assets/Main/Loading/Prefabs/UILoading_SeasonSnow.prefab";
			}
			if (num == 4 && ResourcePackageManager.IsSeasonResDownloadedByPackageId(1004, downloadChecker))
			{
				return "Assets/Main/Loading/Prefabs/UILoading_SeasonMummy.prefab";
			}
			if (num == 5 && ResourcePackageManager.IsSeasonResDownloadedByPackageId(1005, downloadChecker))
			{
				return "Assets/Main/Loading/Prefabs/UILoading_SeasonDark.prefab";
			}
			if (num == 6 && ResourcePackageManager.IsSeasonResDownloadedByPackageId(1006, downloadChecker))
			{
				return "Assets/Main/Loading/Prefabs/UILoading_SeasonNineNation.prefab";
			}
			return "Assets/Main/Loading/Prefabs/UILoading_Normal.prefab";
		}
		return SetLoadingConfig();
	}

	private string SetLoadingConfig()
	{
		string text = "";
		Language language = GetLanguage();
		string country = GetCountry();
		if (PlayerPrefs.GetInt("IS_FIRST_LOGIN", 1) == 1 && IsSystemLanguageSimplifiedChinese())
		{
			text = "Assets/Main/Loading/Prefabs/UILoading_ChineseSimplified.prefab";
			PlayerPrefs.SetInt("IS_FIRST_LOGIN", 0);
			return text;
		}
		PlayerPrefs.SetInt("IS_FIRST_LOGIN", 0);
		GameObject gameObject = GameObject.Find("Container");
		if (gameObject != null)
		{
			LanguagePrefabSelector component2;
			if (gameObject.TryGetComponent<LoadingPrefabSelector>(out var component))
			{
				text = component.GetPrefabPath(language, country);
			}
			else if (gameObject.TryGetComponent<LanguagePrefabSelector>(out component2))
			{
				text = component2.GetPrefabPathFromConfig(language);
			}
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "Assets/Main/Loading/Prefabs/UILoading_Normal.prefab";
		}
		return text;
	}

	public static bool IsSystemLanguageSimplifiedChinese()
	{
		if (Application.systemLanguage != SystemLanguage.ChineseSimplified)
		{
			return Application.systemLanguage == SystemLanguage.Chinese;
		}
		return true;
	}

	public static int GetWeightedRandomIndex(int[] weights)
	{
		return new System.Random().Next(0, weights.Length);
	}

	private void InitSubLoading(GameObject subloading, Language language)
	{
		if (subloading != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(subloading);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.SetParent(subLoadingContainer.transform, worldPositionStays: false);
			component.localScale = Vector3.one;
			component.offsetMin = Vector3.zero;
			component.offsetMax = Vector3.zero;
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.one;
			component.pivot = new Vector2(0.5f, 0.5f);
			component.SetAsLastSibling();
			_subLoadingComponent = gameObject.GetComponent<ISubLoadingComponent>();
			loadingText = _subLoadingComponent.LoadingText;
		}
		if (clearConfirm != null)
		{
			clearConfirm.SetActive(value: false);
		}
		wifiObj.SetActive(value: false);
		btnHelp.gameObject.SetActive(value: false);
		maintenanceDialog.SetActive(value: false);
		serverRedDot.SetActive(value: false);
		_subLoadingComponent.CSOpen();
		_subLoadingComponent.SetProgressBar(0f);
		_subLoadingComponent.SetBg();
		_subLoadingComponent.SetIcon();
		_subLoadingComponent.SetSlider();
		tipText.text = "";
		tipText.gameObject.SetActive(value: false);
		if ((bool)_subLoadingComponent.DownloadText)
		{
			_subLoadingComponent.DownloadText.text = "";
		}
		if ((bool)_subLoadingComponent.VersionText)
		{
			_subLoadingComponent.VersionText.text = "";
		}
		state = State.Login;
		isRefreshLocalizationText = false;
		localeTextInitd = false;
		if (GameEntry.Localization.IsInitDone)
		{
			ShowTextComp();
			InitLocalizationText();
			localeTextInitd = true;
			RefreshLocalizationText();
		}
		else
		{
			HideTextComp();
		}
		maintenanceBtn.onClick.AddListener(delegate
		{
			Application.Quit();
		});
		if (btnClear != null)
		{
			btnClear.onClick.AddListener(OnClearBtnClick);
		}
		if (btnClearConfirmOk != null)
		{
			btnClearConfirmOk.onClick.AddListener(OnClearConfirmBtnClick);
		}
		if (btnClearConfirmNo != null)
		{
			btnClearConfirmNo.onClick.AddListener(OnClearConfirmNoBtnClick);
		}
	}

	private void OnClearBtnClick()
	{
		if (clearConfirm != null)
		{
			clearConfirm.SetActive(value: true);
		}
	}

	private void OnClearConfirmNoBtnClick()
	{
		if (clearConfirm != null)
		{
			clearConfirm.SetActive(value: false);
		}
	}

	private void OnClearConfirmBtnClick()
	{
		CommonUtils.DeleteCache(cacheBundle: false);
		DownloadResGroupCommonManager.SetClearedAllBundleCacheFlag();
		ApplicationLaunch.Instance.ReloadGameInOtherProcess();
	}

	private Language GetLanguage()
	{
		Language language = GameEntry.Setting.UserLanguage;
		if (language == Language.Unspecified)
		{
			language = LocalizationManager.SystemLanguage;
		}
		return language;
	}

	private string GetCountry()
	{
		string fromCountry = GameEntry.GlobalData.fromCountry;
		return GameEntry.Setting.GetString("SERVER_COUNTRY", fromCountry);
	}

	private bool IsAmerica()
	{
		Language language = GetLanguage();
		bool flag = GameEntry.Setting.GetBool("AMERICA_NEW_LOADING", defaultValue: false);
		DebugLog($"UILoading IsAmerica Language: {language}, hasShown: {flag}");
		return language == Language.English && flag;
	}

	private void InitLocalizationText()
	{
		PostEventLog.TrackMap("init_loading_ui_text", SDKManager.AddBILaunchTimeProperty(StartupConfig.Inst.AddBIProperty()));
		txtNotice.text = GameEntry.Localization.GetString("2700000");
		txtServe.text = GameEntry.Localization.GetString("2700001");
		txtHelp.text = GameEntry.Localization.GetString("120060");
		maintenanceDialogTitle.text = GameEntry.Localization.GetString("2700002");
		maintenanceDialogContent.text = GameEntry.Localization.GetString("129012");
		maintenanceBtnText.text = GameEntry.Localization.GetString("110006");
		if (btnClearText != null)
		{
			btnClearText.text = GameEntry.Localization.GetString("clearcache_002");
		}
		if (clearConfirmTitle != null)
		{
			clearConfirmTitle.text = GameEntry.Localization.GetString("clearcache_002");
		}
		if (clearConfirmContent != null)
		{
			clearConfirmContent.text = GameEntry.Localization.GetString("clearcache_001");
		}
		if (btnClearConfirmOk_Text != null)
		{
			btnClearConfirmOk_Text.text = GameEntry.Localization.GetString("110006");
		}
		if (btnClearConfirmNo_Text != null)
		{
			btnClearConfirmNo_Text.text = GameEntry.Localization.GetString("110106");
		}
		if (txtBtnLogin != null)
		{
			txtBtnLogin.text = GameEntry.Localization.GetString("btn_Account");
		}
		if (txtBtnNewGame != null)
		{
			txtBtnNewGame.text = GameEntry.Localization.GetString("100833");
		}
	}

	private void HideTextComp()
	{
		btnNotice.gameObject.SetActive(value: false);
		btnServe.gameObject.SetActive(value: false);
		btnClear.gameObject.SetActive(value: false);
		tipText.gameObject.SetActive(value: false);
		txtBtnLogin.gameObject.SetActive(value: false);
		txtBtnNewGame.gameObject.SetActive(value: false);
	}

	private void ShowTextComp()
	{
		btnNotice.gameObject.SetActive(value: true);
		btnServe.gameObject.SetActive(value: true);
		btnClear.gameObject.SetActive(value: true);
		tipText.gameObject.SetActive(value: true);
		txtBtnLogin.gameObject.SetActive(value: true);
		txtBtnNewGame.gameObject.SetActive(value: true);
	}

	private void InitLuaState(object obj)
	{
		luaOnDestroy?.Invoke();
		LuaEnv env = GameEntry.Lua.Env;
		env.beforeDispose = (Action)Delegate.Combine(env.beforeDispose, new Action(BeforeLuaEnvDispose));
		scriptEnv = env.NewTable();
		LuaTable luaTable = env.NewTable();
		luaTable.Set("__index", env.Global);
		scriptEnv.SetMetaTable(luaTable);
		luaTable.Dispose();
		scriptEnv.Set("self", this);
		string filepath = "Loading.LoadingView";
		byte[] chunk = XLuaManager.CustomLoader(ref filepath);
		env.DoString(chunk, "LoadingView", scriptEnv);
		scriptEnv.Get<string, Action>("OnStart", out luaOnStart);
		scriptEnv.Get<string, Action>("OnDestroy", out luaOnDestroy);
		scriptEnv.Get<string, Action<string>>("OnNoticeDataBack", out luaOnNoticeDataBack);
		isRefreshLocalizationText = false;
		luaStateInited = true;
	}

	private void BeforeLuaEnvDispose()
	{
		scriptEnv = null;
		luaOnStart = null;
		luaOnDestroy = null;
		luaOnNoticeDataBack = null;
	}

	public void CSClose(object userData)
	{
		GameEntry.Event.Unsubscribe(EventId.UILOADING_STATE_TEXT_CHANGE, OnLoadingStateTextChange);
		GameEntry.Event.Unsubscribe(EventId.BeginDownloadUpdate, OnStartBundleDownload);
		GameEntry.Event.Unsubscribe(EventId.EndDownloadUpdate, OnStartLogin);
		GameEntry.Event.Unsubscribe(EventId.NetworkRetry, OnNetworkRetry);
		GameEntry.Event.Unsubscribe(EventId.ReInitLoadingLuaState, InitLuaState);
		luaOnDestroy?.Invoke();
		luaOnDestroy = null;
		luaOnStart = null;
		luaOnNoticeDataBack = null;
		_subLoadingComponent?.CSClose();
		if (_asset != null)
		{
			_asset.Release();
			_asset = null;
		}
	}

	private void OnLoadingStateTextChange(object data)
	{
		if (!GameEntry.Localization.IsInitDone || !GameEntry.Localization.IsInitSuccess)
		{
			return;
		}
		LoadingState currState = ApplicationLaunch.Instance.Loading.currState;
		string text = string.Empty;
		string text2 = string.Empty;
		switch (currState)
		{
		case LoadingState.Permission:
			text = "limit_length_loading_tips_01";
			break;
		case LoadingState.CheckResVersion:
			text = "limit_length_loading_tips_03";
			break;
		case LoadingState.DownloadManifest:
			text = "limit_length_loading_tips_04";
			if (data != null)
			{
				(int, float) obj = ((int, float))data;
				int item = obj.Item1;
				float num = obj.Item2 * 100f;
				string key = "limit_length_loading_tips_11";
				text2 = GameEntry.Localization.GetString(key, item + 1, num.ToString("F2"));
			}
			break;
		case LoadingState.DownloadUpdate:
			text = "limit_length_loading_tips_06";
			if (data != null)
			{
				text = "limit_length_loading_tips_07";
			}
			break;
		case LoadingState.LoadDataTable:
			text = "limit_length_loading_tips_02";
			break;
		case LoadingState.GetServerList:
			text = "limit_length_loading_tips_03";
			break;
		case LoadingState.ConnectGame:
			text = "limit_length_loading_tips_08";
			break;
		case LoadingState.Login:
			text = "limit_length_loading_tips_09";
			break;
		case LoadingState.PushInit:
		case LoadingState.LoadScene:
		case LoadingState.EnterGame:
			text = "limit_length_loading_tips_10";
			break;
		}
		if (!string.IsNullOrEmpty(text))
		{
			string text3 = GameEntry.Localization.GetString(text);
			_subLoadingComponent.LoadingText.text = text3;
		}
		if ((bool)_subLoadingComponent.DownloadText)
		{
			_subLoadingComponent.DownloadText.text = text2;
		}
	}

	private void ResetLoadingProgress()
	{
		loadingProgress = 0f;
		finishDuration = 1f;
		finishStartTime = -1f;
		elapsedTime = 0f;
		finishStartProgress = 0f;
	}

	private bool IsLoadingProgressFinish()
	{
		return elapsedTime - finishDuration >= 0f;
	}

	private void UpdateLoadingProgress(float target)
	{
		if (target < 1f)
		{
			loadingProgress += (target - loadingProgress) * 0.1f;
			finishStartTime = -1f;
		}
		else
		{
			if (finishStartTime < 0f)
			{
				finishStartTime = Time.time;
				finishStartProgress = loadingProgress;
			}
			elapsedTime = Time.time - finishStartTime;
			float t = Mathf.Clamp01((float)(1.0 - Math.Pow(1f - Mathf.Clamp01(elapsedTime / finishDuration), 4.0)));
			loadingProgress = Mathf.Lerp(finishStartProgress, 1f, t);
		}
		_subLoadingComponent?.SetProgressBar(loadingProgress);
		tipText.gameObject.SetActive(loadingProgress > 0f);
	}

	private void Update()
	{
		if (showLogo)
		{
			totalTime += Time.deltaTime;
			if ((double)totalTime >= 2.5)
			{
				showLogo = false;
				totalTime = 0f;
			}
		}
		UpdateNoticeRequest();
		if (GameEntry.Localization.IsInitDone && !localeTextInitd)
		{
			ShowTextComp();
			InitLocalizationText();
			localeTextInitd = true;
		}
		if (GameEntry.Localization.IsInitDone && !isRefreshLocalizationText)
		{
			RefreshLocalizationText();
			isRefreshLocalizationText = true;
		}
		if (state == State.Login)
		{
			if (IsLoadingProgressFinish())
			{
				GameEntry.Event.Fire(EventId.UILOADING_PROGRESS_FINISH);
			}
			UpdateLoadingProgress(ApplicationLaunch.Instance.Loading.LoadingProgress);
		}
		else if (state == State.Download)
		{
			float bundleDownloadProgress = ApplicationLaunch.Instance.Loading.BundleDownloadProgress;
			float num = ByteToMegaByte(ApplicationLaunch.Instance.Loading.BundleDownloadTotalBytes);
			float num2 = bundleDownloadProgress * num;
			if ((bool)_subLoadingComponent.DownloadText)
			{
				_subLoadingComponent.DownloadText.text = string.Format(updatekey, num2.ToString("f2"), num.ToString("f2"));
			}
			UpdateLoadingProgress((bundleDownloadProgress <= 0f) ? 0.0001f : bundleDownloadProgress);
		}
		serverRedDot.SetActive(AIHelpProxy.UnreadMsgCount > 0);
		serverRedDotCount.text = string.Concat(AIHelpProxy.UnreadMsgCount);
	}

	public float GetLogoAnimLength()
	{
		AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;
		foreach (AnimationClip animationClip in animationClips)
		{
			if (animationClip.name == "uiloadAnimator")
			{
				return animationClip.length;
			}
		}
		return 0f;
	}

	public void FetchNoticeData()
	{
		if (_noticeRequest == null)
		{
			PostEventLog.Track("GET_SERVERNOTICE", null);
			_noticeRequest = GameEntry.Network.GetServerNotice();
		}
	}

	private void UpdateNoticeRequest()
	{
		if (_noticeRequest == null)
		{
			return;
		}
		_elapseTime += Time.deltaTime;
		if (_elapseTime > _timeout)
		{
			PostEventLog.Record("SERVERNOTICE_TIME_OUT", _tryCount.ToString());
			if (_tryCount < _maxTryCount)
			{
				_noticeRequest.Abort();
				_noticeRequest.Dispose();
				_noticeRequest = GameEntry.Network.GetServerNotice();
				_tryCount++;
				_elapseTime = 0f;
				GameEntry.Event.Fire(EventId.NetworkRetry, true);
				Log.Info("get servernotice try count: {0}", _tryCount);
			}
			else
			{
				Log.Error("get servernotice timeout");
				_noticeRequest.Dispose();
				_noticeRequest = null;
				OnGetServerNotice(null, "E108", "timeout, reach max try count");
			}
		}
		else if (_noticeRequest.isHttpError || _noticeRequest.isNetworkError)
		{
			Log.Error("get servernotice net error: {0}", _noticeRequest.error);
			string error = _noticeRequest.error;
			_noticeRequest.Dispose();
			_noticeRequest = null;
			OnGetServerNotice(null, "E101", error);
		}
		else
		{
			if (!_noticeRequest.isDone || _noticeRequest.isHttpError || _noticeRequest.isNetworkError)
			{
				return;
			}
			if (!_noticeRequest.downloadHandler.text.IsNullOrEmpty())
			{
				try
				{
					LoginServerNoticeRespon loginServerNoticeRespon = JsonUtility.FromJson<LoginServerNoticeRespon>(_noticeRequest.downloadHandler.text);
					if (loginServerNoticeRespon == null)
					{
						throw new Exception("error json::" + _noticeRequest.downloadHandler.text);
					}
					if (loginServerNoticeRespon.code == 0 && !string.IsNullOrEmpty(loginServerNoticeRespon.notice))
					{
						OnGetServerNotice(loginServerNoticeRespon, "E000", "success");
					}
					else
					{
						OnGetServerNotice(null, "E104", $"res_code:{loginServerNoticeRespon.code}, res_notice:{loginServerNoticeRespon.notice}");
					}
				}
				catch (Exception)
				{
					Log.Info("gsl return {0}", _noticeRequest.downloadHandler.text);
					OnGetServerNotice(null, "E103", "invalid json: " + _noticeRequest.downloadHandler.text);
				}
			}
			else
			{
				OnGetServerNotice(null, "E104", "empty data");
			}
			_noticeRequest.Dispose();
			_noticeRequest = null;
		}
	}

	private void OnGetServerNotice(LoginServerNoticeRespon res, string code, string msg)
	{
		if (code == "E000")
		{
			luaOnNoticeDataBack?.Invoke(res.notice);
			return;
		}
		Log.Error("get servernotice error: {0} => {1}", code, msg);
		luaOnNoticeDataBack?.Invoke(null);
	}

	private void RefreshLocalizationText()
	{
		if (loadingText != null)
		{
			loadingText.text = GameEntry.Localization.GetString("limit_length_loading_tips_01");
		}
		_subLoadingComponent.SetVersionText();
		ApplicationLaunch.UpdateTrackingResVersion();
	}

	private void OnStartLogin(object ud)
	{
		DebugLog("OnStartLogin");
		state = State.Login;
		_subLoadingComponent?.SetProgressBar(0.0001f);
		tipText.gameObject.SetActive(value: false);
		if ((bool)_subLoadingComponent?.DownloadText)
		{
			_subLoadingComponent.DownloadText.text = "";
		}
		ResetLoadingProgress();
	}

	public void ShowAccountSelect(bool show)
	{
		btnAccountSelect.gameObject.SetActive(show);
	}

	public void ShowNewGame(bool show)
	{
		btnNewGame.gameObject.SetActive(show);
	}

	public void RegistNewGameAction(UnityAction action)
	{
		if (btnNewGame != null)
		{
			btnNewGame.onClick.RemoveAllListeners();
			btnNewGame.onClick.AddListener(action);
		}
	}

	public void RegistSelectAccountAction(UnityAction action)
	{
		if (btnAccountSelect != null)
		{
			btnAccountSelect.onClick.RemoveAllListeners();
			btnAccountSelect.onClick.AddListener(action);
		}
	}

	private void OnNetworkRetry(object ud)
	{
		bool active = (bool)ud;
		wifiObj.SetActive(active);
	}

	private void OnStartBundleDownload(object ud)
	{
		DebugLog("OnStartBundleDownload");
		state = State.Download;
		_subLoadingComponent?.SetProgressBar(0.0001f);
		tipText.gameObject.SetActive(value: false);
		float num = ByteToMegaByte(ApplicationLaunch.Instance.Loading.BundleDownloadTotalBytes);
		if ((bool)_subLoadingComponent?.DownloadText)
		{
			_subLoadingComponent.DownloadText.text = string.Format(updatekey, 0f.ToString("f2"), num.ToString("f2"));
		}
		ResetLoadingProgress();
	}

	private void DebugLog(string msg)
	{
		Log.Info("[UILoading] " + msg);
	}

	private float ByteToMegaByte(float byteSize)
	{
		return byteSize / 1048576f;
	}
}
