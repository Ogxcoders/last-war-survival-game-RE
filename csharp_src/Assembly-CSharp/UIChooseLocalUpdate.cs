using System.Collections.Generic;
using DG.Tweening;
using SuperScrollView;
using UnityEngine;
using UnityEngine.UI;

public class UIChooseLocalUpdate : MonoBehaviour
{
	public Text title;

	public UIChooseLocalUpdateItem tableOptions;

	public UIChooseLocalUpdateItem bundleOptions;

	public UIChooseLocalUpdateItem luaOptions;

	public Text devLocaleText;

	public Toggle devLocaleToggle;

	public Button buttonGo;

	public Text buttonGoText;

	public Button buttonReset;

	public Text buttonResetText;

	public Button selectPop;

	public Text selectTitle;

	public InputField selectFilterInput;

	public LoopListView2 loopListView2;

	public GameObject fetchLoading;

	public Transform fetchLoadingIcon;

	public static string tableEnvLocalUpdateKey = "table_env_local_update";

	public static string bundleBuildIdLocalUpdateKey = "bundle_build_id_local_update";

	public static string luaBuildIdLocalUpdateKey = "lua_build_id_local_update";

	public static string devLocaleLocalUpdateKey = "dev_locale_local_update";

	private List<string> _envsOrig;

	private List<string> _bundleBuildsOrig;

	private List<string> _luaBuildsOrig;

	private List<string> _filteredOptions;

	private UIChooseLocalUpdateItem _popupTarget;

	private string selectedTableEnv;

	private string selectedBundleBuildId;

	private string selectedLuaBuildId;

	private bool useDevLocale;

	private FetchDevBuildInfo _fetchDevBuildInfo;

	public void Load()
	{
		title.text = $"线路: {NetworkURLConfig.URLGroupType}\n选择热更版本";
		buttonGo.onClick.AddListener(OnSetOK);
		buttonReset.onClick.AddListener(OnReset);
		fetchLoading.SetActive(value: false);
		fetchLoadingIcon.DOKill();
		FetchBuildInfo();
	}

	private void FetchBuildInfo()
	{
		HidePopupSelect();
		fetchLoading.SetActive(value: true);
		fetchLoadingIcon.DOLocalRotate(new Vector3(0f, 0f, -360f), 1f, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Restart);
		_fetchDevBuildInfo = new FetchDevBuildInfo();
		_fetchDevBuildInfo.completed = FetchDevBuildInfoOnCompleted;
		_fetchDevBuildInfo.SendRequest();
	}

	private void FetchDevBuildInfoOnCompleted(FetchDevBuildInfo arg1, List<string> arg2, List<string> arg3, List<string> arg4)
	{
		fetchLoading.SetActive(value: false);
		fetchLoadingIcon.DOKill();
		__Load(arg2, arg3, arg4);
		if (arg1.succeed)
		{
			buttonGoText.text = "继续!";
			buttonResetText.text = "重置";
		}
		else
		{
			buttonGoText.text = "跳过!";
			buttonResetText.text = "重试";
		}
	}

	public static string GetSelectedTableEnv()
	{
		if (PlayerPrefs.HasKey(tableEnvLocalUpdateKey))
		{
			return PlayerPrefs.GetString(tableEnvLocalUpdateKey);
		}
		if (string.IsNullOrEmpty(ClientConfig.TABLE_ENV))
		{
			return ClientConfig.GetDataFileEnv();
		}
		return ClientConfig.TABLE_ENV;
	}

	public static string GetSelectedBundleBuildId()
	{
		if (PlayerPrefs.HasKey(bundleBuildIdLocalUpdateKey))
		{
			return PlayerPrefs.GetString(bundleBuildIdLocalUpdateKey);
		}
		return GameEntry.Sdk.VersionCode;
	}

	public static string GetSelectedLuaBuildId()
	{
		if (PlayerPrefs.HasKey(luaBuildIdLocalUpdateKey))
		{
			return PlayerPrefs.GetString(luaBuildIdLocalUpdateKey);
		}
		return GameEntry.Sdk.VersionCode;
	}

	private void __Load(List<string> envs, List<string> bundleBuilds, List<string> luaBuilds)
	{
		_envsOrig = new List<string>();
		int i = 0;
		for (int count = envs.Count; i < count; i++)
		{
			if (envs[i].Contains("table_"))
			{
				_envsOrig.Add(envs[i].Replace("table_", ""));
			}
			else
			{
				_envsOrig.Add(envs[i]);
			}
		}
		_bundleBuildsOrig = new List<string>();
		_bundleBuildsOrig.AddRange(bundleBuilds);
		_luaBuildsOrig = new List<string>();
		_luaBuildsOrig.AddRange(luaBuilds);
		selectedTableEnv = GetSelectedTableEnv();
		selectedBundleBuildId = GetSelectedBundleBuildId();
		selectedLuaBuildId = GetSelectedLuaBuildId();
		tableOptions.Init(this, "配置分支", _envsOrig, delegate(string s)
		{
			selectedTableEnv = s;
		});
		bundleOptions.Init(this, "Bundle ID", _bundleBuildsOrig, delegate(string s)
		{
			selectedBundleBuildId = s;
		});
		luaOptions.Init(this, "Lua ID", _luaBuildsOrig, delegate(string s)
		{
			selectedLuaBuildId = s;
		});
		tableOptions.SetInputValue(selectedTableEnv);
		bundleOptions.SetInputValue(selectedBundleBuildId);
		luaOptions.SetInputValue(selectedLuaBuildId);
		_filteredOptions = new List<string>();
		selectPop.onClick.AddListener(HidePopupSelect);
		HidePopupSelect();
		selectFilterInput.onValueChanged.AddListener(OnSelectFilterInputValueChange);
		loopListView2.InitListView(0, OmGetItemByIndexTableEnv);
		devLocaleText.text = "使用Dev版本多语言";
		useDevLocale = false;
		if (PlayerPrefs.HasKey(devLocaleLocalUpdateKey))
		{
			useDevLocale = PlayerPrefs.GetInt(devLocaleLocalUpdateKey) != 0;
		}
		devLocaleToggle.onValueChanged.AddListener(OnDevLocaleToggleValueChange);
		devLocaleToggle.isOn = useDevLocale;
	}

	private void OnDevLocaleToggleValueChange(bool arg0)
	{
		useDevLocale = arg0;
	}

	private void OnSetOK()
	{
		if (_fetchDevBuildInfo.succeed)
		{
			if (tableOptions.CheckValue(selectedTableEnv) && bundleOptions.CheckValue(selectedBundleBuildId) && luaOptions.CheckValue(selectedLuaBuildId))
			{
				PlayerPrefs.SetString(tableEnvLocalUpdateKey, selectedTableEnv);
				PlayerPrefs.SetString(bundleBuildIdLocalUpdateKey, selectedBundleBuildId);
				PlayerPrefs.SetString(luaBuildIdLocalUpdateKey, selectedLuaBuildId);
				PlayerPrefs.SetInt(devLocaleLocalUpdateKey, useDevLocale ? 1 : 0);
				PlayerPrefs.Save();
				ChooseDebugUpdate.selectedTableEnv = "table_" + selectedTableEnv;
				ChooseDebugUpdate.selectedBundleBuildId = selectedBundleBuildId;
				ChooseDebugUpdate.selectedLuaBuildId = selectedLuaBuildId;
				ChooseDebugUpdate.selectedLocaleDev = useDevLocale;
				GameEntry.Event.Fire(EventId.LOCAL_UPDATE_CONFIG_SET, true);
			}
		}
		else
		{
			GameEntry.Event.Fire(EventId.LOCAL_UPDATE_CONFIG_SET, false);
		}
	}

	private void OnReset()
	{
		if (_fetchDevBuildInfo.succeed)
		{
			selectedTableEnv = ClientConfig.TABLE_ENV;
			selectedBundleBuildId = GameEntry.Sdk.VersionCode;
			selectedLuaBuildId = GameEntry.Sdk.VersionCode;
			tableOptions.SetInputValue(selectedTableEnv);
			bundleOptions.SetInputValue(selectedBundleBuildId);
			luaOptions.SetInputValue(selectedLuaBuildId);
		}
		else
		{
			FetchBuildInfo();
		}
	}

	public void OpenSelectPop(UIChooseLocalUpdateItem target)
	{
		_popupTarget = target;
		selectPop.gameObject.SetActive(value: true);
		_filteredOptions.Clear();
		_filteredOptions.AddRange(target.options);
		loopListView2.SetListItemCount(0);
		loopListView2.SetListItemCount(_filteredOptions.Count);
		selectTitle.text = _popupTarget.inputTitle.text;
	}

	public LoopListViewItem2 OmGetItemByIndexTableEnv(LoopListView2 loopView, int index)
	{
		if (index < 0 || index >= _filteredOptions.Count)
		{
			return null;
		}
		string info = _filteredOptions[index];
		LoopListViewItem2 loopListViewItem = loopView.NewListViewItem("SelectItemTemplate");
		Button component = loopListViewItem.GetComponent<Button>();
		loopListViewItem.GetComponentInChildren<Text>().text = info;
		component.onClick.Clear();
		component.onClick.AddListener(delegate
		{
			OnClickTableEnv(info);
		});
		return loopListViewItem;
	}

	private void OnClickTableEnv(string value)
	{
		if (_popupTarget == tableOptions)
		{
			selectedTableEnv = value;
		}
		else if (_popupTarget == bundleOptions)
		{
			selectedBundleBuildId = value;
		}
		else if (_popupTarget == luaOptions)
		{
			selectedLuaBuildId = value;
		}
		_popupTarget.SetInputValue(value);
		HidePopupSelect();
	}

	private void HidePopupSelect()
	{
		_popupTarget = null;
		selectPop.gameObject.SetActive(value: false);
	}

	private void OnSelectFilterInputValueChange(string arg0)
	{
		_filteredOptions.Clear();
		int i = 0;
		for (int count = _popupTarget.options.Count; i < count; i++)
		{
			if (_popupTarget.options[i].Contains(arg0))
			{
				_filteredOptions.Add(_popupTarget.options[i]);
			}
		}
		loopListView2.SetListItemCount(_filteredOptions.Count);
	}
}
