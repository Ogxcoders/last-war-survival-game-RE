using System;
using SFSLitJson;
using UnityEngine;
using XLua;

[LuaCallCSharp(GenFlag.No)]
public static class GameCenterBridge
{
	private static Action<bool, GameCenterAuthData> s_manualLoginCallback;

	private static Action<bool, GameCenterAuthData> s_autoLoginCallback;

	private static ITimer _delayTimer;

	private static int? _iosMajorVersion;

	public static bool IsGameCenterAvailable()
	{
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			return false;
		}
		if (!IsIOSVersionOrGreater(26))
		{
			return false;
		}
		if (StringUtils.VersionCompare(GameEntry.Sdk.Version, "1.0.313") < 0)
		{
			return false;
		}
		if (PlayerPrefs.GetInt("SettingKeys_GAMECENTER_ON", 0) != 1)
		{
			return false;
		}
		return true;
	}

	public static void AutoLogin()
	{
		if (IsGameCenterAvailable())
		{
			DebugLogError("[GameCenterBridge] Requesting AutoLogin...");
			s_manualLoginCallback = null;
			DestroyDelayTimer();
			GameEntry.Sdk.SendDataToNative("GameCenter_Login", "");
		}
	}

	public static void Login(Action<bool, GameCenterAuthData> onComplete)
	{
		if (!IsGameCenterAvailable())
		{
			onComplete?.Invoke(arg1: false, new GameCenterAuthData
			{
				displayName = "GameCenter not available"
			});
			return;
		}
		DebugLogError("[GameCenterBridge] Requesting Manual Login with callback...");
		s_manualLoginCallback = onComplete;
		DestroyDelayTimer();
		GameEntry.Sdk.SendDataToNative("GameCenter_ShowLoginUI", "");
	}

	public static void SetAutoLoginCallback(Action<bool, GameCenterAuthData> onComplete)
	{
		s_autoLoginCallback = onComplete;
	}

	private static void DestroyDelayTimer()
	{
		if (_delayTimer != null)
		{
			GameEntry.Timer.CancelTimer(_delayTimer);
			_delayTimer = null;
		}
	}

	public static void OnNativeCallback(string funcName, string data)
	{
		DebugLogError("[GameCenterBridge] OnNativeCallback Received: " + funcName + ", Data: " + data);
		switch (funcName)
		{
		case "GameCenter_OnAuthSuccess":
		case "GameCenter_OnAuthFailed":
		{
			bool success = funcName == "GameCenter_OnAuthSuccess";
			GameCenterAuthData authData = (success ? JsonMapper.ToObject<GameCenterAuthData>(data) : new GameCenterAuthData
			{
				displayName = data
			});
			if (s_manualLoginCallback != null)
			{
				DebugLogError("[GameCenterBridge] Invoking manual login callback.");
				DestroyDelayTimer();
				_delayTimer = GameEntry.Timer.RegisterTimer(0.3f, delegate
				{
					DebugLogError("[GameCenterBridge] Invoking manual login callback.3");
					s_manualLoginCallback(success, authData);
					s_manualLoginCallback = null;
					_delayTimer = null;
				});
			}
			if (s_autoLoginCallback != null)
			{
				DebugLogError("[GameCenterBridge] Invoking auto login callback.");
				s_autoLoginCallback(success, authData);
			}
			break;
		}
		case "GameCenter_OnSubmitScoreSuccess":
			DebugLogError("[GameCenterBridge] Score submitted successfully for leaderboard: " + data);
			break;
		case "GameCenter_OnSubmitScoreFailed":
			DebugLogError("[GameCenterBridge] Score submission failed: " + data);
			break;
		case "GameCenter_OnReportAchievementSuccess":
			DebugLogError("[GameCenterBridge] Achievement reported successfully: " + data);
			break;
		case "GameCenter_OnReportAchievementFailed":
			DebugLogError("[GameCenterBridge] Achievement report failed: " + data);
			break;
		case "GameCenter_OnUIAfterClosed":
			DebugLogError("[GameCenterBridge] Game Center UI was closed.");
			break;
		default:
			DebugLogError("[GameCenterBridge] Unknown callback function: " + funcName);
			break;
		}
	}

	public static void SubmitScore(string leaderboardID, long score)
	{
		if (IsGameCenterAvailable())
		{
			JsonData jsonData = new JsonData
			{
				["score"] = score,
				["leaderboardID"] = leaderboardID
			};
			GameEntry.Sdk.SendDataToNative("GameCenter_SubmitScore", jsonData.ToJson());
		}
	}

	public static void ShowLeaderboard(string leaderboardID)
	{
		if (IsGameCenterAvailable())
		{
			JsonData jsonData = new JsonData { ["leaderboardID"] = leaderboardID };
			GameEntry.Sdk.SendDataToNative("GameCenter_ShowLeaderboard", jsonData.ToJson());
		}
	}

	public static void ReportAchievement(string achievementID, double percentComplete)
	{
		if (IsGameCenterAvailable())
		{
			JsonData jsonData = new JsonData
			{
				["achievementID"] = achievementID,
				["percentComplete"] = percentComplete
			};
			GameEntry.Sdk.SendDataToNative("GameCenter_ReportAchievement", jsonData.ToJson());
		}
	}

	public static void ShowAchievements()
	{
		if (IsGameCenterAvailable())
		{
			GameEntry.Sdk.SendDataToNative("GameCenter_ShowAchievements", "");
		}
	}

	public static void DebugLogError(string message)
	{
	}

	public static bool IsIOSVersionOrGreater(int targetVersion)
	{
		if (!_iosMajorVersion.HasValue)
		{
			int value = -1;
			_iosMajorVersion = value;
		}
		return _iosMajorVersion.Value >= targetVersion;
	}
}
