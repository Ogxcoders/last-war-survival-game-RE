public class CreditLimitState : LoadingStateBase
{
	private long _frameSinceLastCheck;

	private static readonly long CheckInterval = 15L;

	public CreditLimitState(AppStartupLoading startupLoading)
		: base(startupLoading)
	{
	}

	public override void OnEnter(params object[] args)
	{
		if ((!(GameEntry.Data?.Player?.CheckSwitch("refund_client2", defaultVal: false))) ?? true)
		{
			GameEntry.Lua.UIManager.OpenWindow("UIRefund");
		}
		else
		{
			GameEntry.Lua.UIManager.OpenWindow("LWRefundPunish");
		}
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
		if (_frameSinceLastCheck < CheckInterval)
		{
			_frameSinceLastCheck++;
			return;
		}
		if ((!(GameEntry.Data?.Player?.CheckSwitch("refund_client2", defaultVal: false))) ?? true)
		{
			if (GameEntry.Lua.CallWithReturn<long>("CSharpCallLuaInterface.GetCreditValue") >= 0)
			{
				ApplicationLaunch.Instance.ReStartGame();
			}
		}
		else if (GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.RefundPunishCanLogIn"))
		{
			ApplicationLaunch.Instance.ReStartGame();
		}
		_frameSinceLastCheck = 0L;
	}
}
