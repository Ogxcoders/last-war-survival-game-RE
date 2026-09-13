using System;
using GameFramework;

public class CNIdentifyState : LoadingStateBase
{
	public CNIdentifyState(AppStartupLoading startupLoading)
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
			if (ApplicationLaunch.Instance.Loading.IsChild)
			{
				GameEntry.Lua.UIManager.OpenWindow("UIIDCardAgeTips");
			}
			else
			{
				GameEntry.Lua.UIManager.OpenWindow("UIIDCardAuthenticate");
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex.Message);
		}
	}
}
