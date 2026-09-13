using System;
using System.IO;
using UnityEngine;

public static class GameUtility
{
	public const string AssetBundlesOutputPath = "AssetBundles";

	public static string GetStreamingAssetsDirectory()
	{
		if (Application.isEditor)
		{
			return Path.Combine(Environment.CurrentDirectory, "AssetBundles", GetPlatformName()).Replace("\\", "/");
		}
		if (Application.platform == RuntimePlatform.Android)
		{
			return Application.dataPath + "!assets";
		}
		return Application.streamingAssetsPath;
	}

	public static string GetPlatformName()
	{
		return GetPlatformForApp(Application.platform);
	}

	private static string GetPlatformForApp(RuntimePlatform platform)
	{
		return platform switch
		{
			RuntimePlatform.Android => "Android", 
			RuntimePlatform.IPhonePlayer => "iOS", 
			RuntimePlatform.WebGLPlayer => "WebGL", 
			RuntimePlatform.WindowsPlayer => "StandaloneWindows", 
			RuntimePlatform.OSXPlayer => "StandaloneOSXUniversal", 
			_ => string.Empty, 
		};
	}

	public static string GetGameSessionId()
	{
		string text = GameEntryProxy.Setting.gameSessionId;
		if (string.IsNullOrEmpty(text))
		{
			text = Guid.NewGuid().ToString();
			GameEntryProxy.Setting.gameSessionId = text;
		}
		return text;
	}
}
