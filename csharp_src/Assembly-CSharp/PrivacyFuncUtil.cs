using System;
using System.Collections.Generic;
using KWSVerification;
using Sfs2X.Entities.Data;
using UnityEngine;

public class PrivacyFuncUtil
{
	public enum IsShowDMAType
	{
		NotShow,
		Show
	}

	private static PrivacyFuncUtil _instance;

	public Dictionary<string, string> PrivacyKey;

	public static PrivacyFuncUtil Instance => _instance ?? (_instance = new PrivacyFuncUtil());

	public string Country { get; set; } = "DEFAULT";

	public string IPCountry { get; set; } = "DEFAULT";

	public PrivacyFuncUtil()
	{
		PrivacyKey = new Dictionary<string, string>
		{
			["V1"] = "PrivacyConfirm_0305",
			["V2"] = "PrivacyConfirm_0306",
			["IOSV2"] = "PrivacyConfirm_0306_ios",
			["V3"] = "PrivacyConfirm_0307"
		};
	}

	public void Dispose()
	{
		PrivacyKey = null;
	}

	public void ShowUIPrivacy(int mode)
	{
		UIPrivacyView.Instance.OpenPrivacyView();
	}

	public void ShowUIPrivacyKR(int mode)
	{
		UIPrivacyKRView.Instance.OpenPrivacyView();
	}

	public int CanShowDMA()
	{
		if (GetCountry() == "US")
		{
			return IsShowDMAType.Show.ToInt();
		}
		int result = IsShowDMAType.NotShow.ToInt();
		if (!GameEntry.Setting.GetBool(PrivacyKey["V3"]))
		{
			result = IsShowDMAType.Show.ToInt();
		}
		return result;
	}

	public void SavePrivacyKey()
	{
		GameEntry.Setting.SetBool(PrivacyKey["V3"], value: true);
	}

	public void ShowPrivacy(int show)
	{
		bool flag = IsPrivacyConfirmed();
		bool flag2 = GameEntry.Device.IsKR();
		if (GetCountry() == "US")
		{
			HandleUSPrivacy();
		}
		else if (flag2 && !flag)
		{
			ShowUIPrivacyKR(show);
		}
		else
		{
			ShowUIPrivacy(show);
		}
	}

	public bool IsPrivacyConfirmed()
	{
		bool flag = SDKManager.IS_IPhonePlayer();
		bool num = GameEntry.Setting.GetBool(PrivacyKey["V1"]);
		bool flag2 = GameEntry.Setting.GetBool(PrivacyKey["V2"]);
		bool flag3 = GameEntry.Setting.GetBool(PrivacyKey["IOSV2"]);
		if (!num && !(flag && flag3))
		{
			return !flag && flag2;
		}
		return true;
	}

	public string GetAirKey()
	{
		return GameEntry.Device.GetDeviceUid_Transcoding();
	}

	public string GetCountry()
	{
		AccountData accountData = GetAccountData();
		if (accountData != null && accountData.accountInfo != null && !string.IsNullOrEmpty(accountData.accountInfo.airKey) && !string.IsNullOrEmpty(accountData.accountInfo.country))
		{
			return accountData.accountInfo.country;
		}
		if (IPCountry == "US")
		{
			return "US";
		}
		return Country;
	}

	public string GetLanguage()
	{
		if (!string.IsNullOrEmpty(GameEntry.Setting.GetString("PRIVACY_COPPA_GM_COUNTRY", "")))
		{
			return "en";
		}
		return GameEntry.Localization.GetLanguageName() ?? "en";
	}

	public string GetUid(bool considerNewPlayerFlag = false)
	{
		if (considerNewPlayerFlag && PlayerPrefs.HasKey("PRIVACY_COPPA_NEW_PLAYER"))
		{
			return "";
		}
		return GameEntry.Setting.GetString("Setting.GAME_UID", "");
	}

	public string GetZone(bool considerNewPlayerFlag = false)
	{
		if (considerNewPlayerFlag && PlayerPrefs.HasKey("PRIVACY_COPPA_NEW_PLAYER"))
		{
			return "";
		}
		return GameEntry.Setting.GetString("SERVER_ZONE", "");
	}

	public float GetConfirmDelayDays()
	{
		if (PlayerPrefs.HasKey("PRIVACY_COPPA_NEW_PLAYER"))
		{
			return 0f;
		}
		if (PlayerPrefs.HasKey("PRIVACY_COPPA_GM_DELAYDAY"))
		{
			return 0.0035f;
		}
		return 90f;
	}

	public void SaveAccountData(AccountData accountData)
	{
		if (accountData == null)
		{
			DebugError("AccountInfo is null, cannot save");
			return;
		}
		string text = JsonUtility.ToJson(accountData);
		PlayerPrefs.SetString("PRIVACY_COPPA_ACCOUNT_INFO", text);
		DebugLog("Account info saved: " + text);
	}

	public AccountData GetAccountData()
	{
		string text = PlayerPrefs.GetString("PRIVACY_COPPA_ACCOUNT_INFO", "");
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		AccountData accountData = JsonUtility.FromJson<AccountData>(text);
		if (accountData == null)
		{
			DebugError("Failed to parse AccountInfo from JSON");
		}
		return accountData;
	}

	public void SignCoppaNewPlayer()
	{
		PlayerPrefs.SetString("PRIVACY_COPPA_CURRENT_STATE", "non_us_regions");
		PlayerPrefs.SetString("PRIVACY_COPPA_OPEN_FROM", "Loading");
		string text = GameEntry.Setting.GetString("PRIVACY_COPPA_GM_COUNTRY", "");
		if (!string.IsNullOrEmpty(text))
		{
			Country = text;
		}
		if (PlayerPrefs.HasKey("PRIVACY_COPPA_NEW_PLAYER"))
		{
			DebugLog("SignCoppaNewPlayer COPPA_NEW_PLAYER already checked, skipping.");
		}
		else if (string.IsNullOrEmpty(GameEntry.Setting.GetString("Setting.GAME_UID", "")))
		{
			PlayerPrefs.SetInt("PRIVACY_COPPA_NEW_PLAYER", 1);
			DebugLog("SignCoppaNewPlayer set COPPA_NEW_PLAYER = 1");
		}
		else
		{
			DebugLog("SignCoppaNewPlayer set COPPA_NEW_PLAYER = 0");
		}
	}

	public long GetFinalConfirmTime()
	{
		AccountData accountData = GetAccountData();
		if (accountData != null && accountData.accountInfo != null && !string.IsNullOrEmpty(accountData.accountInfo.finalConfirmTime) && long.TryParse(accountData.accountInfo.finalConfirmTime, out var result))
		{
			return result;
		}
		return 0L;
	}

	public int GetCoppaAge()
	{
		AccountData accountData = GetAccountData();
		if (accountData != null && accountData.accountInfo != null && !string.IsNullOrEmpty(accountData.accountInfo.age) && int.TryParse(accountData.accountInfo.age, out var result))
		{
			return result;
		}
		return 0;
	}

	public string GetCoppaEmail()
	{
		AccountData accountData = GetAccountData();
		if (accountData != null && accountData.accountInfo != null && !string.IsNullOrEmpty(accountData.accountInfo.email))
		{
			return accountData.accountInfo.email;
		}
		return "";
	}

	public bool IsClientSwitchOn()
	{
		string key = $"CLIENT_SWITCH_CACHE_ON_{14}";
		if (!PlayerPrefs.HasKey(key))
		{
			DebugLog("ClientSwitch is ON by default as no client switch found.");
			return true;
		}
		bool flag = PlayerPrefs.GetInt(key, 0) == 1;
		DebugLog("ClientSwitch is " + (flag ? "ON" : "OFF") + " .");
		return flag;
	}

	public bool IsGrayDevice()
	{
		bool flag = GrayUtils.IsGrayDevice(100);
		DebugLog("GrayDevice is " + (flag ? "ON" : "OFF"));
		return flag;
	}

	public bool IsFunctionOn()
	{
		if (!IsClientSwitchOn())
		{
			return false;
		}
		if (!IsGrayDevice())
		{
			return false;
		}
		return true;
	}

	public void HandleUSPrivacy()
	{
		if (!IsFunctionOn())
		{
			DebugLog("TotalFunction is off!");
			SetCoppaState("non_us_regions", showPrivacy: false);
			return;
		}
		LogEvent_CoppaStateTriggerIn();
		AccountData accountData = GetAccountData();
		if (accountData != null && accountData.need_account && accountData.accountInfo != null && !string.IsNullOrEmpty(accountData.accountInfo.airKey) && !string.IsNullOrEmpty(accountData.accountInfo.age) && accountData.accountInfo.age != "0" && int.TryParse(accountData.accountInfo.age, out var result) && result > 12)
		{
			string text = PlayerPrefs.GetString("PRIVACY_COPPA_ACCOUNT_INFO", "");
			DebugLog("Using cached accountData " + text);
			HandleCoppaState(accountData);
			return;
		}
		KWSVerificationManager.Instance.GetAccountInfo(GetAirKey(), GetCountry(), GetLanguage(), 0, GetUid(considerNewPlayerFlag: true), GetZone(considerNewPlayerFlag: true), delegate(AccountInfoResponse response)
		{
			FirstVerify_ProcessAccountInfoResponse(response);
		}, delegate(string error)
		{
			FirstVerify_HandleAccountInfoError(error);
		});
	}

	private void FirstVerify_ProcessAccountInfoResponse(AccountInfoResponse response)
	{
		DebugLog("Account info retrieved successfully");
		if (response?.data == null)
		{
			DebugError("Response data is null");
			return;
		}
		SaveAccountData(response.data);
		HandleCoppaState(response.data);
	}

	public bool HandleCoppaState(AccountData accountData)
	{
		if (accountData == null || !accountData.need_account)
		{
			DebugLog("function switch off, non US regions");
			return SetCoppaState("non_us_regions", showPrivacy: false);
		}
		if (accountData.accountInfo == null || string.IsNullOrEmpty(accountData.accountInfo.airKey))
		{
			return HandleNoAccount();
		}
		return HandleYesAccount(accountData.accountInfo);
	}

	private bool HandleNoAccount()
	{
		DebugLog("HandleNoAccount, need to create");
		if (PlayerPrefs.HasKey("PRIVACY_COPPA_NEW_PLAYER"))
		{
			DebugLog("New player: UsUncertifiedNewPlayer, Create account ");
			return SetCoppaState("us_uncertified_new_player", showPrivacy: true);
		}
		DebugLog("Old player: UsUncertifiedOldPlayer, Account exists, but not verified");
		KWSVerificationManager.Instance.CreateAccount(GetAirKey(), GetCountry(), GetLanguage(), 0, needParent: false, GetUid(considerNewPlayerFlag: true), GetZone(considerNewPlayerFlag: true), GetConfirmDelayDays(), delegate(CreateAccountResponse response)
		{
			DebugLog("Old player: Account created successfully");
			SaveAccountData(response.data);
			SetCoppaState("us_uncertified_old_player", showPrivacy: false);
		}, delegate(string error)
		{
			DebugError("Old player: Failed to create account: " + error);
			SetCoppaState("non_us_regions", showPrivacy: false);
			LogEvent_CoppaStateException(error);
		});
		return false;
	}

	private bool HandleYesAccount(AccountInfo accountInfo)
	{
		if (string.IsNullOrEmpty(accountInfo.age) || accountInfo.age == "0")
		{
			return HandleNoAge(accountInfo);
		}
		return HandleYesAge(accountInfo);
	}

	private bool HandleNoAge(AccountInfo accountInfo)
	{
		DebugLog("No age information found, checking if user is new or existing");
		if (string.IsNullOrEmpty(accountInfo.finalConfirmTime) || accountInfo.finalConfirmTime == "0")
		{
			DebugLog("No final confirm time , treating as new user");
			return HandleNoDelayDays(accountInfo);
		}
		if (Country == "US" || IPCountry == "US")
		{
			DebugLog("yes final confirm time, treating as old user");
			return HandleYesDelayDays(accountInfo);
		}
		DebugLog("old user but not non US regions or ip");
		return SetCoppaState("non_us_regions", showPrivacy: false);
	}

	private bool HandleYesAge(AccountInfo accountInfo)
	{
		DebugLog("HandleYesAge, checking age verification status");
		if (!int.TryParse(accountInfo.age, out var result) || result < 3)
		{
			DebugError("Invalid age: " + accountInfo.age);
			return false;
		}
		if (result > 12)
		{
			return HandleAdultUser();
		}
		return HandleChildUser(accountInfo, result);
	}

	private bool HandleNoDelayDays(AccountInfo accountInfo)
	{
		DebugLog("No delay days, treating as new user");
		return SetCoppaState("us_uncertified_new_player", showPrivacy: true);
	}

	private bool HandleYesDelayDays(AccountInfo accountInfo)
	{
		DebugLog("Has delay days, treating as old user");
		if (!long.TryParse(accountInfo.finalConfirmTime, out var result))
		{
			DebugError(" HandleYesDelayDays Invalid finalConfirmTime");
			return false;
		}
		if (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() > result)
		{
			DebugLog("Grace period expired, forcing verification");
			return SetCoppaState("us_uncertified_old_time_out", showPrivacy: true);
		}
		DebugLog("Within grace period, allowing gameplay");
		return SetCoppaState("us_uncertified_old_player", showPrivacy: false);
	}

	private bool HandleAdultUser()
	{
		DebugLog("Adult user, no parent verification needed");
		return SetCoppaState("us_certified_adult", showPrivacy: false);
	}

	private bool HandleChildUser(AccountInfo accountInfo, int age)
	{
		if (string.IsNullOrEmpty(accountInfo.email))
		{
			DebugLog("Child user, email not sent yet");
			return SetCoppaState("us_uncertified_need_mail", showPrivacy: true);
		}
		return HandleChildWithEmail(accountInfo);
	}

	private bool HandleChildWithEmail(AccountInfo accountInfo)
	{
		if (accountInfo.parentConfirmed == "1")
		{
			DebugLog("Parent approved privacy collection");
			return SetCoppaState("us_certified_children", showPrivacy: false);
		}
		DebugLog("Parent confirmation pending");
		return SetCoppaState("us_uncertified_mail_pending", showPrivacy: true);
	}

	private bool SetCoppaState(string state, bool showPrivacy)
	{
		PlayerPrefs.SetString("PRIVACY_COPPA_CURRENT_STATE", state);
		if (showPrivacy)
		{
			UIPrivacyCoppaView.Instance.OpenPrivacyView();
		}
		else
		{
			GameEntry.Event.Fire(EventId.UIPrivacy_Confirm);
		}
		return showPrivacy;
	}

	private void FirstVerify_HandleAccountInfoError(string error)
	{
		DebugError("Failed to get account info: " + error);
		SetCoppaState("non_us_regions", showPrivacy: false);
		LogEvent_CoppaStateException(error);
	}

	public void PushInit(ISFSObject message)
	{
		DebugLog("PushInit called with message");
		PlayerPrefs.SetInt("PRIVACY_COPPA_ACCOUNT_STATE", 0);
		if (message.ContainsKey("kids"))
		{
			int num = message.GetInt("kids");
			PlayerPrefs.SetInt("PRIVACY_COPPA_ACCOUNT_STATE", num);
			DebugLog($"PushInit fromeCountry:{Country} kids: {num}");
		}
		IPCountry = (message.ContainsKey("realLoginCountry") ? message.GetUtfString("realLoginCountry") : "DEFAULT");
		string text = GameEntry.Setting.GetString("PRIVACY_COPPA_GM_IPCOUNTRY", "");
		if (!string.IsNullOrEmpty(text))
		{
			IPCountry = text;
		}
		DebugLog("PushInit realLoginCountry: " + IPCountry);
	}

	public void Coppa_HandleSecondVerify_IpCountry()
	{
		DebugLog("Parkour Coppa_HandleSecondVerify_IpCountry called");
		if (!(Country == "US") && IPCountry == "US")
		{
			PlayerPrefs.SetString("PRIVACY_COPPA_OPEN_FROM", "Parkour");
			HandleUSPrivacy();
		}
	}

	public void ShowUIPrivacyUS(int mode)
	{
		if (!IsFunctionOn())
		{
			DebugLog("TotalFunction is off!");
			return;
		}
		AccountData accountData = GetAccountData();
		if (accountData != null && accountData.accountInfo != null && !HandleCoppaState(accountData))
		{
			UIPrivacyCoppaView.Instance.OpenPrivacyView();
		}
	}

	public void LogEvent_CoppaStateTriggerIn()
	{
		LogEvent("COPPA_STATE_TRIGGER_IN", null);
	}

	public void LogEvent_CoppaStateException(string msg)
	{
		LogEvent("COPPA_STATE_EXCEPTION", new Dictionary<string, object> { { "errorMsg", msg } });
	}

	public void LogEvent(string eventName, Dictionary<string, object> additionalData)
	{
		DebugLog("LogEvent: " + eventName);
		Dictionary<string, object> dictionary = new Dictionary<string, object>
		{
			{
				"detail",
				GameEntry.Device.GetDeviceUid()
			},
			{
				"airkey",
				GetAirKey()
			},
			{
				"uid",
				GetUid()
			},
			{
				"s_para1",
				GetCountry()
			},
			{ "s_para2", IPCountry },
			{ "s_para3", Country },
			{
				"s_para4",
				GetLanguage()
			}
		};
		if (additionalData != null)
		{
			foreach (KeyValuePair<string, object> additionalDatum in additionalData)
			{
				dictionary[additionalDatum.Key] = additionalDatum.Value;
			}
		}
		PostEventLog.TrackMap(eventName, dictionary);
	}

	private void DebugLog(string message)
	{
	}

	private void DebugError(string message)
	{
		Debug.LogError("[COPPA] " + message);
	}
}
