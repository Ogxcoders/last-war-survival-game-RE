public class AppUpdateState : LoadingStateBase
{
	public AppUpdateState(AppStartupLoading startupLoading)
		: base(startupLoading)
	{
	}

	public override void OnEnter(params object[] args)
	{
		ShowAppUpgradeMessage();
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
	}

	private void ShowAppUpgradeMessage()
	{
		string text = ((GameEntry.GlobalData.analyticID == "market_global") ? "GOOGLE" : GameEntry.GlobalData.analyticID);
		string message = GameEntry.Localization.GetString("129013", text);
		if (GameEntry.GlobalData.updateType == 1)
		{
			UIUtils.ShowMessage(message, 2, "110003", "", delegate
			{
				if (!GameEntry.GlobalData.downloadurl.IsNullOrEmpty())
				{
					SDKManager.OpenURL(GameEntry.GlobalData.downloadurl);
				}
			}, delegate
			{
				GameEntry.GlobalData.updateType = 0;
				_startupLoading.SetState(LoadingState.PushInit);
			});
		}
		else
		{
			if (GameEntry.GlobalData.updateType != 2)
			{
				return;
			}
			UIUtils.ShowMessage(message, 1, "110003", "", delegate
			{
				if (!GameEntry.GlobalData.downloadurl.IsNullOrEmpty())
				{
					SDKManager.OpenURL(GameEntry.GlobalData.downloadurl);
				}
			}, delegate
			{
				if (!GameEntry.GlobalData.downloadurl.IsNullOrEmpty())
				{
					SDKManager.OpenURL(GameEntry.GlobalData.downloadurl);
				}
			});
		}
	}
}
