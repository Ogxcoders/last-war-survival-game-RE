public class MaintenanceState : LoadingStateBase
{
	public MaintenanceState(AppStartupLoading startupLoading)
		: base(startupLoading)
	{
	}

	public override void OnEnter(params object[] args)
	{
		GameEntry.Event.Fire(EventId.CloseDisconnectView);
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
	}
}
