public abstract class FsmBaseState
{
	protected CrossServerFsmManager _mgr;

	public abstract int id { get; }

	public FsmBaseState(CrossServerFsmManager mgr)
	{
		_mgr = mgr;
	}

	public abstract void OnEnter(params object[] args);

	public abstract void OnExit();

	public abstract void OnUpdate();

	protected void GotoLoadingError(int code, params object[] p)
	{
		if (_mgr != null)
		{
			_mgr.SetState(9, code, p);
		}
	}
}
