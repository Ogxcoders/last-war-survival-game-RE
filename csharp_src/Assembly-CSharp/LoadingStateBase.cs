public abstract class LoadingStateBase
{
	protected AppStartupLoading _startupLoading;

	public LoadingStateBase(AppStartupLoading startupLoading)
	{
		_startupLoading = startupLoading;
	}

	public abstract void OnEnter(params object[] args);

	public abstract void OnExit();

	public abstract void OnUpdate();
}
