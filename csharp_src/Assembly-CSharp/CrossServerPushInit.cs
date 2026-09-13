using UnityEngine;

public class CrossServerPushInit : FsmBaseState
{
	private float _elapseTime;

	private const float pushInitTimeOut = 13f;

	public override int id => 5;

	public CrossServerPushInit(CrossServerFsmManager mgr)
		: base(mgr)
	{
	}

	public override void OnEnter(params object[] args)
	{
		_elapseTime = 0f;
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
		_elapseTime += Time.deltaTime;
		if (ApplicationLaunch.Instance.Loading.IsPushInitReceived)
		{
			_mgr.SetState(6);
		}
		else if (_elapseTime > 13f)
		{
			GotoLoadingError(11);
		}
	}
}
