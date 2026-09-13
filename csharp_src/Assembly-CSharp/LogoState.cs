using UnityEngine;

public class LogoState : LoadingStateBase
{
	private float _logoAnimLength;

	private float _animTime;

	public LogoState(AppStartupLoading startupLoading)
		: base(startupLoading)
	{
	}

	public override void OnEnter(params object[] args)
	{
		_animTime = 0f;
		_logoAnimLength = -1f;
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
		if (_startupLoading.UILoading != null)
		{
			if (_logoAnimLength < 0f)
			{
				_logoAnimLength = _startupLoading.UILoading.GetLogoAnimLength();
			}
			_animTime += Time.deltaTime;
			if (_animTime > _logoAnimLength)
			{
				_startupLoading.SetState(LoadingState.Permission);
			}
		}
	}
}
