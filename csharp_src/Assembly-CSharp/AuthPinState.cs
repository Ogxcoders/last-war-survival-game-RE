using System;
using GameFramework;

public class AuthPinState : LoadingStateBase
{
	public AuthPinState(AppStartupLoading startupLoading)
		: base(startupLoading)
	{
	}

	public override void OnEnter(params object[] args)
	{
		_startupLoading.IsPushInitReceived = false;
		ShowAuthUI();
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
	}

	private void ShowAuthUI()
	{
		try
		{
			GameEntry.Lua.UIManager.OpenWindow("UIPinInput", "0");
		}
		catch (Exception ex)
		{
			Log.Error(ex.Message);
		}
	}
}
