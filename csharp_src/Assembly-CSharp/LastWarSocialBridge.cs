using UnityEngine;
using XLua;

[LuaCallCSharp(GenFlag.No)]
public static class LastWarSocialBridge
{
	public static void SubmitScore(string leaderboardID, long score)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			GameCenterBridge.SubmitScore(leaderboardID, score);
		}
		else if (Application.platform == RuntimePlatform.Android)
		{
			PlayGamesBridge.SubmitScore(leaderboardID, score);
		}
	}

	public static void ShowLeaderboard(string leaderboardID)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			GameCenterBridge.ShowLeaderboard(leaderboardID);
		}
		else if (Application.platform == RuntimePlatform.Android)
		{
			PlayGamesBridge.ShowLeaderboard(leaderboardID);
		}
	}

	public static void ReportAchievement(string achievementID, double percentComplete)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			GameCenterBridge.ReportAchievement(achievementID, percentComplete);
		}
		else if (Application.platform == RuntimePlatform.Android)
		{
			PlayGamesBridge.ReportAchievement(achievementID, percentComplete);
		}
	}

	public static void ShowAchievements()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			GameCenterBridge.ShowAchievements();
		}
		else if (Application.platform == RuntimePlatform.Android)
		{
			PlayGamesBridge.ShowAchievements();
		}
	}
}
