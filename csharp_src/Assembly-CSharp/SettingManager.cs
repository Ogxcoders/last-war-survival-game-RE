using GameFramework;
using GameFramework.Localization;
using UnityEngine;

public class SettingManager
{
	public string gameSessionId;

	private string _gameUid = "";

	public Language UserLanguage
	{
		get
		{
			int num = GetInt("Setting.USER_LANGUAGE", (int)LocalizationManager.SystemLanguage);
			if (num == 0)
			{
				Log.Error($"SettingManager UserLanguage is invalid {num}, {LocalizationManager.SystemLanguage}, {Application.systemLanguage}.");
			}
			return (Language)num;
		}
		set
		{
			SetInt("Setting.USER_LANGUAGE", (int)value);
		}
	}

	public string GameUid
	{
		get
		{
			return _gameUid;
		}
		set
		{
			if (value.IsNullOrEmpty())
			{
				Log.Error("settingproxy uid is null");
			}
			else
			{
				_gameUid = value;
			}
		}
	}

	public bool IsReview { get; set; }

	public bool FirstLaunchSkipUpdateNewestVersion { get; set; }

	public bool Load()
	{
		return true;
	}

	public bool Save()
	{
		PlayerPrefs.Save();
		return true;
	}

	public bool HasSetting(string settingName)
	{
		return PlayerPrefs.HasKey(settingName);
	}

	public void RemoveSetting(string settingName)
	{
		PlayerPrefs.DeleteKey(settingName);
	}

	public void RemoveAllSettings()
	{
		PlayerPrefs.DeleteAll();
	}

	public bool GetBool(string settingName)
	{
		return PlayerPrefs.GetInt(settingName) != 0;
	}

	public bool GetBool(string settingName, bool defaultValue)
	{
		return PlayerPrefs.GetInt(settingName, defaultValue ? 1 : 0) != 0;
	}

	public void SetBool(string settingName, bool value)
	{
		PlayerPrefs.SetInt(settingName, value ? 1 : 0);
	}

	public int GetInt(string settingName)
	{
		return PlayerPrefs.GetInt(settingName);
	}

	public int GetInt(string settingName, int defaultValue)
	{
		return PlayerPrefs.GetInt(settingName, defaultValue);
	}

	public void SetInt(string settingName, int value)
	{
		PlayerPrefs.SetInt(settingName, value);
	}

	public float GetFloat(string settingName)
	{
		return PlayerPrefs.GetFloat(settingName);
	}

	public float GetFloat(string settingName, float defaultValue)
	{
		return PlayerPrefs.GetFloat(settingName, defaultValue);
	}

	public void SetFloat(string settingName, float value)
	{
		PlayerPrefs.SetFloat(settingName, value);
	}

	public string GetString(string settingName)
	{
		return PlayerPrefs.GetString(settingName);
	}

	public string GetString(string settingName, string defaultValue)
	{
		return PlayerPrefs.GetString(settingName, defaultValue);
	}

	public void SetString(string settingName, string value)
	{
		PlayerPrefs.SetString(settingName, value);
	}

	public bool GetPublicBool(string settingName)
	{
		return PlayerPrefs.GetInt(settingName) != 0;
	}

	public bool GetPublicBool(string settingName, bool defaultValue)
	{
		return PlayerPrefs.GetInt(settingName, defaultValue ? 1 : 0) != 0;
	}

	public bool GetPrivateBool(string settingName)
	{
		return GetPublicBool(settingName + GameUid);
	}

	public bool GetPrivateBool(string settingName, bool defaultValue)
	{
		return GetPublicBool(settingName + GameUid, defaultValue);
	}

	public void SetPublicBool(string settingName, bool value)
	{
		PlayerPrefs.SetInt(settingName, value ? 1 : 0);
	}

	public void SetPrivateBool(string settingName, bool value)
	{
		if (GameUid.IsNullOrEmpty())
		{
			Log.Info("<color=#ff0000> SetPrivateBool error, settingName: {0}", settingName);
		}
		SetPublicBool(settingName + GameUid, value);
	}

	public int GetPublicInt(string settingName)
	{
		return PlayerPrefs.GetInt(settingName);
	}

	public int GetPublicInt(string settingName, int defaultValue)
	{
		return PlayerPrefs.GetInt(settingName, defaultValue);
	}

	public void SetPublicInt(string settingName, int value)
	{
		PlayerPrefs.SetInt(settingName, value);
	}

	public void SetPrivateInt(string settingName, int value)
	{
		SetPublicInt(settingName + GameUid, value);
	}

	public int GetPrivateInt(string settingName, int value)
	{
		return GetPublicInt(settingName + GameUid, value);
	}

	public void SetPrivateFloat(string settingName, float value)
	{
		SetPublicFloat(settingName + GameUid, value);
	}

	public float GetPrivateFloat(string settingName, float value)
	{
		return GetPublicFloat(settingName + GameUid, value);
	}

	public float GetPublicFloat(string settingName)
	{
		return PlayerPrefs.GetFloat(settingName);
	}

	public float GetPublicFloat(string settingName, float defaultValue)
	{
		return PlayerPrefs.GetFloat(settingName, defaultValue);
	}

	public void SetPublicFloat(string settingName, float value)
	{
		PlayerPrefs.SetFloat(settingName, value);
	}

	public string GetPublicString(string settingName)
	{
		return PlayerPrefs.GetString(settingName);
	}

	public string GetPublicString(string settingName, string defaultValue)
	{
		return PlayerPrefs.GetString(settingName, defaultValue);
	}

	public string GetPrivateString(string settingName, string defaultValue)
	{
		return GetPublicString(settingName + GameUid, defaultValue);
	}

	public void SetPublicString(string settingName, string value)
	{
		PlayerPrefs.SetString(settingName, value);
	}

	public void SetPrivateString(string settingName, string value)
	{
		SetPublicString(settingName + GameUid, value);
	}

	public bool PlayerPrefsGetBool(string key, bool defaultValue)
	{
		key = GameEntry.Data.Player.Uid + key;
		return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
	}

	public void PlayerPrefsSetBool(string key, bool value)
	{
		key = GameEntry.Data.Player.Uid + key;
		PlayerPrefs.SetInt(key, value ? 1 : 0);
	}

	public int PlayerPrefsGetInt(string key, int defaultValue)
	{
		key = GameEntry.Data.Player.Uid + key;
		return PlayerPrefs.GetInt(key, defaultValue);
	}

	public void PlayerPrefsSetInt(string key, int value)
	{
		key = GameEntry.Data.Player.Uid + key;
		PlayerPrefs.SetInt(key, value);
	}

	public float PlayerPrefsGetFloat(string key, float defaultValue)
	{
		key = GameEntry.Data.Player.Uid + key;
		return PlayerPrefs.GetFloat(key, defaultValue);
	}

	public void PlayerPrefsSetFloat(string key, float value)
	{
		key = GameEntry.Data.Player.Uid + key;
		PlayerPrefs.SetFloat(key, value);
	}

	public string PlayerPrefsGetString(string key, string defaultValue)
	{
		key = GameEntry.Data.Player.Uid + key;
		return PlayerPrefs.GetString(key, defaultValue);
	}

	public void PlayerPrefsSetString(string key, string value)
	{
		key = GameEntry.Data.Player.Uid + key;
		PlayerPrefs.SetString(key, value);
	}

	public void UpdateFirstLaunchFlag(bool launchFinish = false)
	{
		if (launchFinish)
		{
			PlayerPrefs.SetInt("FirstLaunchFlag", 2);
		}
		else if (PlayerPrefs.GetInt("FirstLaunchFlag", 0) == 0)
		{
			PlayerPrefs.SetInt("FirstLaunchFlag", 1);
		}
	}

	public bool IsFirstLaunch()
	{
		return PlayerPrefs.GetInt("FirstLaunchFlag", 0) <= 1;
	}

	public bool CheckFirstLaunchSkipUpdate(bool init = false)
	{
		if (GameEntry.Setting.IsReview)
		{
			return true;
		}
		if (ClientSwitch.IsOn(24))
		{
			return false;
		}
		return PlayerPrefs.GetInt("FirstLaunchSkipUpdateFlag", 0) switch
		{
			2 => false, 
			_ => true, 
		};
	}

	public void FirstLaunchSkipUpdateRunning()
	{
		if (PlayerPrefs.GetInt("FirstLaunchSkipUpdateFlag", 0) < 1)
		{
			PlayerPrefs.SetInt("FirstLaunchSkipUpdateFlag", 1);
		}
	}

	public void DisableFirstLaunchSkipUpdate()
	{
		if (PlayerPrefs.GetInt("FirstLaunchSkipUpdateFlag", 0) < 2 && !GameEntry.Setting.IsReview)
		{
			PlayerPrefs.SetInt("FirstLaunchSkipUpdateFlag", 2);
		}
	}
}
