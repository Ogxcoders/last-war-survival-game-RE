using GameFramework.Localization;
using UnityEngine;

public class LoadDataTableState : LoadingStateBase
{
	private float _stateTime;

	public LoadDataTableState(AppStartupLoading startupLoading)
		: base(startupLoading)
	{
	}

	public override void OnEnter(params object[] args)
	{
		PostEventLog.TrackMap("load_data_table_state", SDKManager.AddBILaunchTimeProperty());
		_stateTime = 0f;
		_startupLoading.PreloadAssets();
		GameEntry.pb.Reload();
		GameEntry.Localization.Language = GameEntry.Setting.UserLanguage;
		UIRunTimeConfig.IsArabic = GameEntry.Setting.UserLanguage == Language.Arabic;
		SuperTextMesh.IsArabicLanguage = GameEntry.Setting.UserLanguage == Language.Arabic;
		MirrorVersionConfig.RefreshOpenFlag();
		ClientConfig.DeleteOldTableFile();
		DynamicAtlasManager.Instance.PrepareSelfCheck();
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
		_stateTime += Time.deltaTime;
		if (GameEntry.Localization.IsInitDone && GameEntry.pb.IsInitDone)
		{
			if (GameEntry.Localization.IsInitSuccess && GameEntry.pb.IsInitSuccess)
			{
				if (_startupLoading.updateAfterLogin)
				{
					_startupLoading.StartConnectGame();
				}
				else
				{
					_startupLoading.StartConnect();
				}
			}
			else
			{
				_startupLoading.SetState(LoadingState.LoadingError, "E111");
			}
		}
		else if (_stateTime > 60f)
		{
			_startupLoading.SetState(LoadingState.LoadingError, "E112");
		}
	}
}
