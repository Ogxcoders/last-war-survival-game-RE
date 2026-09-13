using System;
using System.Collections.Generic;
using SFSLitJson;
using UnityEngine;
using XLua;

[LuaCallCSharp(GenFlag.No)]
public static class PlayGamesBridge
{
	private static PlayGamesAuthData _authData;

	private static Action<PlayGamesAuthData> s_manualLoginCallback;

	private static Action<PlayGamesAuthData> s_autoLoginCallback;

	private static Dictionary<string, string> _global = new Dictionary<string, string>
	{
		{ "com.fun.lastwar.leaderboard.event.frontline.weekly", "CgkInc_lsssVEAIQDQ" },
		{ "com.fun.lastwar.leaderboard.frontline.chapter.1", "CgkInc_lsssVEAIQDg" },
		{ "com.fun.lastwar.leaderboard.frontline.chapter.2", "CgkInc_lsssVEAIQDw" },
		{ "com.fun.lastwar.leaderboard.frontline.chapter.3", "CgkInc_lsssVEAIQEA" },
		{ "com.fun.lastwar.leaderboard.frontline.chapter.4", "CgkInc_lsssVEAIQEQ" },
		{ "com.fun.lastwar.leaderboard.frontline.chapter.5", "CgkInc_lsssVEAIQEg" },
		{ "com.fun.lastwar.leaderboard.frontline.chapter.6", "CgkInc_lsssVEAIQEw" },
		{ "com.fun.lastwar.achievement.build.level.20", "CgkInc_lsssVEAIQAQ" },
		{ "com.fun.lastwar.achievement.build.level.30", "CgkInc_lsssVEAIQAg" },
		{ "com.fun.lastwar.achievement.build.level.35", "CgkInc_lsssVEAIQAw" },
		{ "com.fun.lastwar.achievement.frontline.clear.chapter.30", "CgkInc_lsssVEAIQBA" },
		{ "com.fun.lastwar.achievement.frontline.clear.chapter.40", "CgkInc_lsssVEAIQBQ" },
		{ "com.fun.lastwar.achievement.frontline.clear.chapter.48", "CgkInc_lsssVEAIQBg" },
		{ "com.fun.lastwar.achievement.login.days.30", "CgkInc_lsssVEAIQBw" },
		{ "com.fun.lastwar.achievement.login.days.150", "CgkInc_lsssVEAIQCA" },
		{ "com.fun.lastwar.achievement.login.days.365", "CgkInc_lsssVEAIQCQ" },
		{ "com.fun.lastwar.achievement.power.100m", "CgkInc_lsssVEAIQCg" },
		{ "com.fun.lastwar.achievement.power.300m", "CgkInc_lsssVEAIQCw" },
		{ "com.fun.lastwar.achievement.power.500m", "CgkInc_lsssVEAIQDA" }
	};

	private static Dictionary<string, string> _vietnam = new Dictionary<string, string>
	{
		{ "com.fun.lastwar.leaderboard.event.frontline.weekly", "CgkIo_SAt-wTEAIQAQ" },
		{ "com.fun.lastwar.leaderboard.frontline.chapter.1", "CgkIo_SAt-wTEAIQAg" },
		{ "com.fun.lastwar.leaderboard.frontline.chapter.2", "CgkIo_SAt-wTEAIQAw" },
		{ "com.fun.lastwar.leaderboard.frontline.chapter.3", "CgkIo_SAt-wTEAIQBA" },
		{ "com.fun.lastwar.leaderboard.frontline.chapter.4", "CgkIo_SAt-wTEAIQBQ" },
		{ "com.fun.lastwar.leaderboard.frontline.chapter.5", "CgkIo_SAt-wTEAIQBg" },
		{ "com.fun.lastwar.leaderboard.frontline.chapter.6", "CgkIo_SAt-wTEAIQBw" },
		{ "com.fun.lastwar.achievement.build.level.20", "CgkIo_SAt-wTEAIQCA" },
		{ "com.fun.lastwar.achievement.build.level.30", "CgkIo_SAt-wTEAIQCQ" },
		{ "com.fun.lastwar.achievement.build.level.35", "CgkIo_SAt-wTEAIQCg" },
		{ "com.fun.lastwar.achievement.frontline.clear.chapter.30", "CgkIo_SAt-wTEAIQEQ" },
		{ "com.fun.lastwar.achievement.frontline.clear.chapter.40", "CgkIo_SAt-wTEAIQEg" },
		{ "com.fun.lastwar.achievement.frontline.clear.chapter.48", "CgkIo_SAt-wTEAIQEw" },
		{ "com.fun.lastwar.achievement.login.days.30", "CgkIo_SAt-wTEAIQCw" },
		{ "com.fun.lastwar.achievement.login.days.150", "CgkIo_SAt-wTEAIQDA" },
		{ "com.fun.lastwar.achievement.login.days.365", "CgkIo_SAt-wTEAIQDQ" },
		{ "com.fun.lastwar.achievement.power.100m", "CgkIo_SAt-wTEAIQDg" },
		{ "com.fun.lastwar.achievement.power.300m", "CgkIo_SAt-wTEAIQDw" },
		{ "com.fun.lastwar.achievement.power.500m", "CgkIo_SAt-wTEAIQEA" }
	};

	public static string ConvertName(string originalName)
	{
		if (GameEntry.Sdk.IsVNPlatform())
		{
			if (_vietnam.ContainsKey(originalName))
			{
				return _vietnam[originalName];
			}
		}
		else if (_global.ContainsKey(originalName))
		{
			return _global[originalName];
		}
		return originalName;
	}

	public static bool IsPlayGamesAvailable()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return false;
		}
		string b = (GameEntry.Sdk.IsVNPlatform() ? "1.0.321" : "1.0.322");
		if (StringUtils.VersionCompare(GameEntry.Sdk.Version, b) < 0)
		{
			return false;
		}
		if (!IsClientSwitchOn())
		{
			return false;
		}
		if (!IsGrayDevice())
		{
			DebugLogError("Play Games Services is not available: Device is not in gray list.");
			return false;
		}
		return true;
	}

	public static bool IsClientSwitchOn()
	{
		string key = $"CLIENT_SWITCH_CACHE_ON_{14}";
		if (!PlayerPrefs.HasKey(key))
		{
			DebugLogError("ClientSwitch is ON by default as no client switch found.");
			return true;
		}
		bool flag = PlayerPrefs.GetInt(key, 0) == 1;
		DebugLogError("ClientSwitch is " + (flag ? "ON" : "OFF") + " .");
		return flag;
	}

	public static bool IsGrayDevice()
	{
		bool flag = GrayUtils.IsGrayDevice(GameEntry.Sdk.IsVNPlatform() ? 100 : 100);
		DebugLogError("GrayDevice is " + (flag ? "ON" : "OFF"));
		return flag;
	}

	public static void AutoLogin()
	{
		if (IsPlayGamesAvailable())
		{
			DebugLogError("Requesting AutoLogin PGS_SignInSilently ...");
			s_manualLoginCallback = null;
			GameEntry.Sdk.SendDataToNative("PGS_SignInSilently", "");
		}
	}

	public static void Login(Action<PlayGamesAuthData> onComplete)
	{
		if (IsPlayGamesAvailable())
		{
			DebugLogError("Requesting Login with callback...");
			s_manualLoginCallback = onComplete;
			GameEntry.Sdk.SendDataToNative("PGS_SignIn", "");
		}
	}

	public static void LoginSilently(Action<PlayGamesAuthData> onComplete)
	{
		if (IsPlayGamesAvailable())
		{
			DebugLogError("Requesting LoginSilently with callback...");
			s_manualLoginCallback = onComplete;
			GameEntry.Sdk.SendDataToNative("PGS_SignInSilently", "");
		}
	}

	public static void SetAutoLoginCallback(Action<PlayGamesAuthData> onComplete)
	{
		s_autoLoginCallback = onComplete;
	}

	public static void OnNativeCallback(string funcName, string data)
	{
		DebugLogError("OnNativeCallback Received: " + funcName + ", Data: " + data);
		switch (funcName)
		{
		case "PGS_OnPGSAuthResult":
			_authData = JsonMapper.ToObject<PlayGamesAuthData>(data);
			if (s_manualLoginCallback != null)
			{
				DebugLogError("Invoking manual login callback.");
				s_manualLoginCallback(_authData);
				s_manualLoginCallback = null;
			}
			if (s_autoLoginCallback != null)
			{
				DebugLogError("Invoking auto login callback.");
				s_autoLoginCallback(_authData);
			}
			break;
		case "PGS_OnPGSLeaderboardResult":
			DebugLogError("Score submitted successfully for leaderboard: " + data);
			break;
		case "PGS_OnPGSAchievementResult":
			DebugLogError("Achievement reported successfully: " + data);
			break;
		case "PGS_OnPGSUiOpened":
			DebugLogError("PlayGames UI was opened.");
			break;
		case "PGS_OnPGSError":
			DebugLogError("PlayGames Error: " + data);
			break;
		default:
			DebugLogError("Unknown callback function: " + funcName);
			break;
		}
	}

	public static PlayGamesAuthData GetAuthData()
	{
		if (!IsPlayGamesAvailable())
		{
			return null;
		}
		return _authData;
	}

	public static void SubmitScore(string leaderboardID, long score)
	{
		if (IsPlayGamesAvailable())
		{
			leaderboardID = ConvertName(leaderboardID);
			JsonData jsonData = new JsonData
			{
				["score"] = score,
				["leaderboardId"] = leaderboardID
			};
			GameEntry.Sdk.SendDataToNative("PGS_SubmitScore", jsonData.ToJson());
		}
	}

	public static void ShowLeaderboard(string leaderboardID)
	{
		if (IsPlayGamesAvailable())
		{
			leaderboardID = ConvertName(leaderboardID);
			JsonData jsonData = new JsonData { ["leaderboardID"] = leaderboardID };
			GameEntry.Sdk.SendDataToNative("PGS_ShowLeaderboardUI", jsonData.ToJson());
		}
	}

	public static void ReportAchievement(string achievementID, double percentComplete)
	{
		if (IsPlayGamesAvailable())
		{
			achievementID = ConvertName(achievementID);
			int num = (int)Math.Round(percentComplete);
			JsonData jsonData = new JsonData
			{
				["achievementId"] = achievementID,
				["steps"] = num
			};
			GameEntry.Sdk.SendDataToNative("PGS_IncrementAchievement", jsonData.ToJson());
		}
	}

	public static void ShowAchievements()
	{
		if (IsPlayGamesAvailable())
		{
			GameEntry.Sdk.SendDataToNative("PGS_ShowAchievementsUI", "");
		}
	}

	public static void DebugLogError(string message)
	{
	}
}
