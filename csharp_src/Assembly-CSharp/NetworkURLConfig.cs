using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using VEngine;

public static class NetworkURLConfig
{
	private static URLGroupType _URLGroupType;

	private static string _packageName;

	public static URLGroupType URLGroupType => URLGroupType.Online;

	public static bool IsOnline => true;

	public static bool IsLocal => false;

	public static bool IsPressureTest => false;

	public static bool IsAWS => false;

	public static bool IsChangeDebugURLGroup
	{
		get
		{
			return PlayerPrefs.GetInt("IS_CHANGE_DEBUG_CHOOSE_URL_GROUP", 0) == 1;
		}
		set
		{
			PlayerPrefs.SetInt("IS_CHANGE_DEBUG_CHOOSE_URL_GROUP", value ? 1 : 0);
			if (value)
			{
				_packageName = "";
				Log.Info("[AT]ClearGUID&AT_DebugUrlGroup");
				PlayerPrefs.SetString("Setting.GAME_UID", "");
				PlayerPrefs.SetString("SERVER_ZONE", "");
				PlayerPrefs.SetString("Login.access_token", "");
				PlayerPrefs.SetInt("Login.access_token_time", 0);
				PlayerPrefs.SetString("Login.refresh_token", "");
				PlayerPrefs.SetInt("Login.refresh_token_time", 0);
				PlayerPrefs.SetString("Login.login_key", "");
				PlayerPrefs.SetInt("DEBUG_LAST_SERVERID", 0);
				PlayerPrefs.Save();
			}
		}
	}

	public static string PackageName
	{
		get
		{
			if (string.IsNullOrEmpty(_packageName))
			{
				_packageName = Application.identifier;
				if (_packageName.Equals("com.fun.lastwar.debug"))
				{
					_packageName = "com.fun.lastwar.gp";
				}
			}
			return _packageName;
		}
	}

	public static string DownloadURL => ConstURLConfig.onlineDownloadURL_;

	static NetworkURLConfig()
	{
		if (CommonUtils.IsDebug() && !PlayerPrefs.HasKey("DEBUG_CHOOSE_URL_GROUP_NEW"))
		{
			PlayerPrefs.SetInt("DEBUG_CHOOSE_URL_GROUP_NEW", 5);
			IsChangeDebugURLGroup = true;
		}
		RefreshType();
	}

	public static void RefreshType()
	{
		_URLGroupType = (URLGroupType)PlayerPrefs.GetInt("DEBUG_CHOOSE_URL_GROUP_NEW", 0);
	}

	public static void SetURLGroupEnv(string env)
	{
		if (Enum.TryParse<URLGroupType>(env, out var result) && result != _URLGroupType)
		{
			PlayerPrefs.SetInt("DEBUG_CHOOSE_URL_GROUP_NEW", (int)result);
			_URLGroupType = result;
			IsChangeDebugURLGroup = true;
			PlayerPrefs.Save();
		}
	}

	public static string[] GetHostListByCurGroupType()
	{
		if (ConstURLConfig.GetServerListConfigDic.TryGetValue(URLGroupType, out var value))
		{
			if (URLGroupType == URLGroupType.Online && GrayUtils.GetPercentageFromMd5(GameEntry.Device.GetDeviceUid()) <= 5.0)
			{
				return ConstURLConfig.online_ea_CheckVersionHostList;
			}
			return value;
		}
		return Array.Empty<string>();
	}

	public static string GetCheckVersionURL(string host)
	{
		int num = GameEntry.Setting.GetInt("Setting.GM_FLAG");
		string packageName = PackageName;
		string platformName = Versions.PlatformName;
		string text = GameEntry.Setting.GetString("SERVER_ZONE", "");
		string text2 = "";
		string text3 = "";
		string text4 = string.Empty;
		if (string.IsNullOrEmpty(text4))
		{
			text4 = host + $"/gameservice/getlsu3dversion.php?packageName={packageName}&platform={platformName}&appVersion={GameEntry.Sdk.Version}&gm={((num > 0) ? 1 : 0)}&server={text}&uid={text2}&deviceId={text3}&table_env={ClientConfig.TableEnvName}&buildId={GameEntry.Sdk.VersionCode}&returnJson=1";
		}
		return text4 + "&unityVersion=440";
	}

	public static string GetDownloadURL(string filename)
	{
		string platformName = Versions.PlatformName;
		string packageName = PackageName;
		return DownloadURL + packageName + "/" + platformName + "/" + filename;
	}

	public static string GetBattleReportHostByCurGroupType(bool forceOnline, bool isFull, bool isAddressMode, string address)
	{
		string value = "";
		if (forceOnline)
		{
			value = ((!isAddressMode) ? (isFull ? ConstURLConfig.OnlineDownloadBattleReportCDN : ConstURLConfig.OnlineBattleReportCDN) : ConstURLConfig.OnlineBattleReportAddressCDN);
		}
		else if (isAddressMode)
		{
			BattleReportOSSURLType battleReportOSSURLType = GetBattleReportOSSURLType(address);
			ConstURLConfig.BattleReportAddressConfigDic.TryGetValue(battleReportOSSURLType, out value);
		}
		else if (isFull)
		{
			ConstURLConfig.BattleReportDownloadConfigDic.TryGetValue(URLGroupType, out value);
		}
		else
		{
			ConstURLConfig.BattleReportConfigDic.TryGetValue(URLGroupType, out value);
		}
		return value;
	}

	public static string GetBattleReportDownloadHostByCurGroupType(bool isAddressMode, string address)
	{
		string value = "";
		if (isAddressMode)
		{
			BattleReportOSSURLType battleReportOSSURLType = GetBattleReportOSSURLType(address);
			ConstURLConfig.BattleReportAddressConfigDic.TryGetValue(battleReportOSSURLType, out value);
		}
		else
		{
			ConstURLConfig.BattleReportDownloadConfigDic.TryGetValue(URLGroupType, out value);
		}
		return value;
	}

	public static string GetBattleReportDownloadHostByCurGroupType(bool isAddressMode, string address, bool forceUseOnline)
	{
		string value = "";
		if (isAddressMode)
		{
			BattleReportOSSURLType key = GetBattleReportOSSURLType(address);
			if (forceUseOnline)
			{
				key = BattleReportOSSURLType.Online;
			}
			ConstURLConfig.BattleReportAddressConfigDic.TryGetValue(key, out value);
		}
		else
		{
			URLGroupType key2 = URLGroupType;
			if (forceUseOnline)
			{
				key2 = URLGroupType.Online;
			}
			ConstURLConfig.BattleReportDownloadConfigDic.TryGetValue(key2, out value);
		}
		return value;
	}

	public static string GetMailRankDataHostByCurGroupType(bool isAddressMode, string address)
	{
		string value = "";
		if (isAddressMode)
		{
			BattleReportOSSURLType battleReportOSSURLType = GetBattleReportOSSURLType(address);
			ConstURLConfig.BattleReportAddressConfigDic.TryGetValue(battleReportOSSURLType, out value);
		}
		else
		{
			ConstURLConfig.MailRankDataConfigDic.TryGetValue(URLGroupType, out value);
		}
		return value;
	}

	public static bool IsNeedSkipUpdate()
	{
		return false;
	}

	public static BattleReportOSSURLType GetBattleReportOSSURLType(string address)
	{
		if (string.IsNullOrEmpty(address))
		{
			return BattleReportOSSURLType.Unknown;
		}
		foreach (KeyValuePair<BattleReportOSSURLType, string> item in ConstURLConfig.BattleReportAddressSubMap)
		{
			if (address.StartsWith(item.Value))
			{
				return item.Key;
			}
		}
		return BattleReportOSSURLType.Unknown;
	}

	public static string ModifyAddressStr(string address)
	{
		if (string.IsNullOrEmpty(address))
		{
			return "";
		}
		BattleReportOSSURLType battleReportOSSURLType = GetBattleReportOSSURLType(address);
		ConstURLConfig.BattleReportAddressSubMap.TryGetValue(battleReportOSSURLType, out var value);
		if (string.IsNullOrEmpty(value) || !address.StartsWith(value) || address.Length <= value.Length)
		{
			return "";
		}
		if (!string.IsNullOrEmpty(value))
		{
			return address.Substring(value.Length);
		}
		return "";
	}
}
