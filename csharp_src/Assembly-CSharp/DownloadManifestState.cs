using System.Collections.Generic;
using GameFramework;
using VEngine;

public class DownloadManifestState : LoadingStateBase
{
	private List<DownloadManifest> _downloadManifests = new List<DownloadManifest>();

	private int _retryCounter;

	public DownloadManifestState(AppStartupLoading startupLoading)
		: base(startupLoading)
	{
	}

	public override void OnEnter(params object[] args)
	{
		_retryCounter = 0;
		PostEventLog.TrackMap("download_manifest_state", SDKManager.AddBILaunchTimeProperty());
		Log.Info("[DownloadManifest] start download manifest");
		bool num = GameEntry.Setting.CheckFirstLaunchSkipUpdate();
		if (num)
		{
			ClientSwitch.ForceSetSwitchAndSave(48, isOn: false);
		}
		if (!num && _startupLoading.updateAfterLogin && ResourcePackageManager.GetRequiredPackages().Length != 0)
		{
			Log.Info("[DownloadManifestState] DisableFirstLaunchSkipUpdate.");
			GameEntry.Setting.DisableFirstLaunchSkipUpdate();
		}
		if (num || NetworkURLConfig.IsNeedSkipUpdate())
		{
			Log.Info("[DownloadManifest] FirstLaunch skip update  => LoadDataTable");
			_startupLoading.SetState(LoadingState.LoadDataTable, false);
			return;
		}
		if (_startupLoading.updateAfterLogin && _startupLoading.checkResVersionError)
		{
			Log.Info("[DownloadManifest] checkResVersion Error skip update  => LoadDataTable");
			_startupLoading.SetState(LoadingState.LoadDataTable, false);
			return;
		}
		if (GameEntry.Resource.SkipUpdateBundle)
		{
			Log.Info("[DownloadManifest] Skip download manifest cause SKIP_UPDATE");
			_startupLoading.SetState(LoadingState.DownloadUpdate, null);
			return;
		}
		foreach (string item in GameEntry.Resource.GetManifestNamesInUse())
		{
			DownloadManifest downloadManifest = DownloadManifest.LoadAsync(item);
			downloadManifest.SetRetryAction(RetryCallback);
			_downloadManifests.Add(downloadManifest);
		}
	}

	private void RetryCallback(int retryCount)
	{
		if (retryCount > _retryCounter)
		{
			_retryCounter = retryCount;
		}
	}

	public override void OnExit()
	{
		foreach (DownloadManifest downloadManifest in _downloadManifests)
		{
			downloadManifest.Dispose();
		}
		_downloadManifests.Clear();
	}

	public override void OnUpdate()
	{
		if (_downloadManifests.Count <= 0)
		{
			return;
		}
		bool flag = true;
		bool flag2 = false;
		float num = 0f;
		foreach (DownloadManifest downloadManifest in _downloadManifests)
		{
			downloadManifest.OnUpdate();
			if (!downloadManifest.isDone)
			{
				flag = false;
			}
			else if (!string.IsNullOrEmpty(downloadManifest.error))
			{
				flag2 = true;
				break;
			}
			num += downloadManifest.downloadProgress;
		}
		num /= (float)_downloadManifests.Count;
		GameEntry.Event.Fire(EventId.UILOADING_STATE_TEXT_CHANGE, (_retryCounter, num));
		if (flag2)
		{
			_startupLoading.SetState(LoadingState.LoadingError, "E114");
		}
		else if (flag)
		{
			Manifest[] array = _downloadManifests.ConvertAll((DownloadManifest a) => a.manifest).ToArray();
			AppStartupLoading startupLoading = _startupLoading;
			object[] args = array;
			startupLoading.SetState(LoadingState.DownloadUpdate, args);
		}
	}
}
