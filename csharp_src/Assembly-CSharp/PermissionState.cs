using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using GameFramework;
using Main.Scripts.Application.LoadingState;
using UnityEngine;

public class PermissionState : LoadingStateBase
{
	private HttpRequest _zipDownloadRequest;

	private bool downloadFinish;

	public PermissionState(AppStartupLoading startupLoading)
		: base(startupLoading)
	{
	}

	public override void OnEnter(params object[] args)
	{
		PostEventLog.TrackMap("permission_state", SDKManager.AddBILaunchTimeProperty());
		GameEntry.Sdk.DMAPrivacyAllowed();
		if (SDKManager.IS_IPhonePlayer())
		{
			string dataFromNative = GameEntry.Sdk.GetDataFromNative("Get_isSecurityCheckPassed", "");
			Log.Info("checkJB:" + dataFromNative);
			if (!string.IsNullOrEmpty(dataFromNative))
			{
				PostEventLog.TrackMap("ios_check_security", new Dictionary<string, object> { 
				{
					"errMsg",
					dataFromNative ?? ""
				} });
			}
		}
		if (SDKManager.IS_Android() && !SDKManager.IS_UNITY_EDITOR() && !CheckActivity())
		{
			if (!GameEntry.Lua.HasGameStart && XLuaManager.DelayLuaStartGame)
			{
				Log.Info("delay lua start game");
				GameEntry.Lua.StartGame();
			}
			_startupLoading.SetState(LoadingState.LoadingError, "E901");
		}
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
		if (_startupLoading.UILoading != null && GameEntry.Localization.IsInitDone)
		{
			_startupLoading.SetState(LoadingState.CheckResVersion);
			GameEntry.Sdk.DoInitShop();
		}
	}

	private void StartZipDownLoad()
	{
		if (GameEntry.Resource.SkipUpdateBundle)
		{
			downloadFinish = true;
		}
	}

	private void UnzipDatabase(byte[] data)
	{
		string text = Application.persistentDataPath + "/ZipDocument/";
		if (!string.IsNullOrEmpty(text) && !Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		string text2 = text + "getnewlua.zip";
		FileStream fileStream = new FileStream(text2, FileMode.Create);
		fileStream.Write(data, 0, data.Length);
		fileStream.Dispose();
		string text3 = text + "getnewlua/";
		if (Directory.Exists(text3))
		{
			Directory.Delete(text3, recursive: true);
		}
		if (!string.IsNullOrEmpty(text3) && !Directory.Exists(text3))
		{
			Directory.CreateDirectory(text3);
		}
		using (ZipArchive zipArchive = ZipFile.OpenRead(text2))
		{
			foreach (ZipArchiveEntry entry in zipArchive.Entries)
			{
				string text4 = Path.Combine(text3, entry.FullName);
				if (entry.FullName.EndsWith("/"))
				{
					if (!string.IsNullOrEmpty(text4) && !Directory.Exists(text4))
					{
						Directory.CreateDirectory(text4);
					}
				}
				else if ((entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) || entry.FullName.EndsWith(".lua", StringComparison.OrdinalIgnoreCase)) && text4.StartsWith(text3, StringComparison.Ordinal))
				{
					entry.ExtractToFile(text4, overwrite: true);
				}
			}
		}
		string version = GameEntry.Sdk.Version;
		if (Directory.Exists(Path.Combine(text3, version)))
		{
			ApplicationLaunch.Instance.Loading.isZipModeFinish = true;
			GameEntry.Lua.ExitGame();
			GameEntry.Lua.Shutdown();
			GameEntry.Lua.Initialize();
			GameEntry.Lua.StartGame();
		}
		downloadFinish = true;
	}

	private bool CheckActivity()
	{
		try
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
			try
			{
				string text = androidJavaObject.Call<AndroidJavaObject>("getClass", Array.Empty<object>()).Call<string>("getName", Array.Empty<object>());
				Log.Info("PermissionState::CheckActivity ActivityName:" + text);
			}
			catch (Exception ex)
			{
				Log.Error("PermissionState::CheckActivity ActivityName exception: " + ex.Message);
			}
			try
			{
				string text2 = androidJavaObject.Call<string>("getPackageName", Array.Empty<object>());
				Log.Info("PermissionState::CheckActivity packageName:" + text2);
				string text3 = androidJavaObject.Call<AndroidJavaObject>("getPackageManager", Array.Empty<object>()).Call<AndroidJavaObject>("getPackageInfo", new object[2] { text2, 0 }).Get<AndroidJavaObject>("applicationInfo")
					.Get<string>("className");
				Log.Info("PermissionState::CheckActivity ApplicationName:" + text3);
			}
			catch (Exception ex2)
			{
				Log.Error("PermissionState::CheckActivity ApplicationName exception: " + ex2.Message);
			}
			bool flag = false;
			try
			{
				string text4 = "uk.lgl.MainActivity";
				androidJavaObject.Call<AndroidJavaObject>("getClass", Array.Empty<object>()).Call<AndroidJavaObject>("getClassLoader", Array.Empty<object>()).Call<AndroidJavaObject>("loadClass", new object[1] { text4 });
				flag = true;
			}
			catch (Exception)
			{
			}
			Log.Info($"PermissionState::CheckActivity exist loader {flag}");
		}
		catch (Exception ex4)
		{
			Log.Error("PermissionState::CheckActivity exception: " + ex4.Message);
		}
		return true;
	}
}
