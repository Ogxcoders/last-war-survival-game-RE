using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UILoadingComponentSafeMode : MonoBehaviour
{
	private enum MsgBoxFunc
	{
		None,
		ClearCache,
		DisableParallel
	}

	public GameObject safeModeObj;

	public TextMeshProUGUIEx txtTitle;

	public Button btnServe;

	public TextMeshProUGUIEx txtServe;

	public Button btnClear;

	public TextMeshProUGUIEx txtClear;

	public Button btnDisableParallel;

	public TextMeshProUGUIEx txtDisableParallel;

	public GameObject clearConfirm;

	public TextMeshProUGUIEx clearConfirmTitle;

	public TextMeshProUGUIEx clearConfirmContent;

	public Button btnClearConfirmOk;

	public Button btnClearConfirmNo;

	public TextMeshProUGUIEx btnClearConfirmOk_Text;

	public TextMeshProUGUIEx btnClearConfirmNo_Text;

	private MsgBoxFunc _msgBoxFunc;

	private bool _localeLoaded;

	private Dictionary<string, string> _localeDefault = new Dictionary<string, string>();

	private string GetString(string key)
	{
		if (_localeLoaded)
		{
			return GameEntry.Localization.GetString(key);
		}
		if (_localeDefault.TryGetValue(key, out var value))
		{
			return value;
		}
		return key;
	}

	public void Bind()
	{
		btnServe.onClick.AddListener(HandleBtnServe);
		btnClear.onClick.AddListener(HandleBtnClear);
		btnDisableParallel.onClick.AddListener(HandleBtnDisableParallel);
		btnClearConfirmOk.onClick.AddListener(HandleBtnClearConfirmOk);
		btnClearConfirmNo.onClick.AddListener(HandleBtnClearConfirmNo);
		Hide();
		_localeDefault.Add("2700001", "Support");
		_localeDefault.Add("clearcache_001", "If you can't log into the game, check your network first. If the issue persists, try clearing the cache. <color=#f53c3d>Clearing the cache will re-download the latest updates</color>. Please wait.");
		_localeDefault.Add("clearcache_002", "Clear Cache");
		_localeDefault.Add("limit_length_loading_title01", "Repair");
		_localeDefault.Add("limit_length_loading_title02", "Safe Mode");
		_localeDefault.Add("limit_length_loading_01", "Startup failed. Please start the repair to log in.");
		_localeDefault.Add("110006", "Confirm");
		_localeDefault.Add("110106", "Cancel");
	}

	public void Show()
	{
		safeModeObj.SetActive(value: true);
		_localeLoaded = ApplicationLaunch.Instance.TaskSucceed(2);
		if (_localeLoaded)
		{
			GameEntry.Localization.UseSwapLocaleFile(safeMode: true);
		}
		txtTitle.text = GetString("limit_length_loading_title02");
		txtServe.text = GetString("2700001");
		txtClear.text = GetString("clearcache_002");
		txtDisableParallel.text = GetString("limit_length_loading_title01");
		btnClearConfirmOk_Text.text = GetString("110006");
		btnClearConfirmNo_Text.text = GetString("110106");
	}

	public void Hide()
	{
		safeModeObj.SetActive(value: false);
		clearConfirm.SetActive(value: false);
	}

	private void HandleBtnServe()
	{
		ZendeskSupportView.ShowMessaging();
	}

	private void HandleBtnClear()
	{
		ShowMsgBox(MsgBoxFunc.ClearCache);
	}

	private void HandleBtnDisableParallel()
	{
		ShowMsgBox(MsgBoxFunc.DisableParallel);
	}

	private void ShowMsgBox(MsgBoxFunc msgBoxFunc)
	{
		_msgBoxFunc = msgBoxFunc;
		clearConfirm.SetActive(value: true);
		if (_msgBoxFunc == MsgBoxFunc.ClearCache)
		{
			clearConfirmTitle.text = GetString("clearcache_002");
			clearConfirmContent.text = GetString("clearcache_001");
		}
		else if (_msgBoxFunc == MsgBoxFunc.DisableParallel)
		{
			clearConfirmTitle.text = GetString("limit_length_loading_title01");
			clearConfirmContent.text = GetString("limit_length_loading_01");
		}
	}

	private void HandleBtnClearConfirmOk()
	{
		MsgBoxFunc msgBoxFunc = _msgBoxFunc;
		_msgBoxFunc = MsgBoxFunc.None;
		clearConfirm.SetActive(value: false);
		switch (msgBoxFunc)
		{
		case MsgBoxFunc.ClearCache:
		{
			bool num = File.Exists(ApplicationLaunch.ParallelInitLockFile);
			CommonUtils.DeleteCache(cacheBundle: false);
			if (num && !File.Exists(ApplicationLaunch.ParallelInitLockFile))
			{
				using (File.Create(ApplicationLaunch.ParallelInitLockFile))
				{
				}
			}
			ApplicationLaunch.Instance.ReloadGameInOtherProcess();
			break;
		}
		case MsgBoxFunc.DisableParallel:
			if (!File.Exists(ApplicationLaunch.ParallelInitLockFile))
			{
				using (File.Create(ApplicationLaunch.ParallelInitLockFile))
				{
				}
			}
			ApplicationLaunch.Instance.ReloadGameInOtherProcess();
			break;
		}
	}

	private void HandleBtnClearConfirmNo()
	{
		_msgBoxFunc = MsgBoxFunc.None;
		clearConfirm.SetActive(value: false);
	}
}
