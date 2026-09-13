using System;
using FibMatrix;
using GameFramework;
using VEngine;

public class EnterGameState : LoadingStateBase
{
	private bool ea;

	private bool enterGame;

	public EnterGameState(AppStartupLoading startupLoading)
		: base(startupLoading)
	{
	}

	public override void OnEnter(params object[] args)
	{
		enterGame = false;
		GameEntry.Event.Subscribe(EventId.UILOADING_PROGRESS_FINISH, HandleUILoadingProgressFinish);
	}

	private void HandleUILoadingProgressFinish(object obj)
	{
		if (!enterGame)
		{
			enterGame = true;
			OnEnter();
		}
	}

	private void OnEnter()
	{
		PostEventLog.TrackMap("entergame_state", SDKManager.AddBILaunchTimeProperty());
		SceneManager.ResetNightColor();
		SceneManager.ResetRollStrengthZ();
		if (!SceneManager.IsInPVE())
		{
			SceneManager.ResetSceneShadow();
		}
		int num = 0;
		try
		{
			num = GameEntry.Lua.CallWithReturn<int>("LuaEntry.Player:GetNewBeeMigrateWay");
		}
		catch (Exception ex)
		{
			num = 0;
			Log.Error("LuaEntry.Player:GetNewBeeMigrateWay exception!!! exception:{0}", ex.ToString());
		}
		if (num % 2 != 1 && (_startupLoading.isCanCloseLoading || !GameEntry.Lua.CallWithReturn<bool>("DataCenter.GuideManager:IsStartId")))
		{
			_startupLoading.CloseUILoading();
			_startupLoading.ClosePrivacyView();
		}
		GameEntry.Setting.UpdateFirstLaunchFlag(launchFinish: true);
		GameEntry.GlobalData.recordGaid();
		GameEntry.Sdk.LogEvent("app_launch");
		ShumeiSdkManager.Instance.SendDeviceIdToServer();
		GameEntry.Event.Fire(EventId.LOAD_COMPLETE);
		GameEntry.Event.Fire(EventId.Guide_video_Play);
		if (Versions.BundleFastValidation)
		{
			DownloadValidator.Start();
		}
		ea = false;
		if (ea)
		{
			WarmupDownload287.StartWarmupDownload();
		}
		else
		{
			WarmupDownload.StartWarmupDownload();
		}
		PrivacyFuncUtil.Instance.Coppa_HandleSecondVerify_IpCountry();
		try
		{
			if (FileLoggerTarget.instance != null)
			{
				GrayUtils.hasFileLog = false;
				FibMatrix.Logger.RemoveTarget(FileLoggerTarget.instance);
				FileLoggerTarget.instance.Dispose();
			}
		}
		catch (Exception message)
		{
			Log.Error(message);
		}
	}

	private void SendToServerBuildInfo()
	{
	}

	public override void OnExit()
	{
		GameEntry.Event.Unsubscribe(EventId.UILOADING_PROGRESS_FINISH, HandleUILoadingProgressFinish);
		if (Versions.BundleFastValidation)
		{
			DownloadValidator.Stop();
		}
		if (ea)
		{
			WarmupDownload287.StopWarmupDownload();
		}
		else
		{
			WarmupDownload.StopWarmupDownload();
		}
		DownloadManifestManager.Instance.StopAllDownload();
	}

	public override void OnUpdate()
	{
	}
}
