public class LoadSceneState : LoadingStateBase
{
	public LoadSceneState(AppStartupLoading startupLoading)
		: base(startupLoading)
	{
	}

	public override void OnEnter(params object[] args)
	{
		PostEventLog.TrackMap("loadscene_state", SDKManager.AddBILaunchTimeProperty());
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
		if (_startupLoading.LoadingProgress >= 1f && (SceneManager.World == null || SceneManager.World.IsBuildFinish()))
		{
			_startupLoading.SetState(LoadingState.EnterGame);
		}
	}
}
