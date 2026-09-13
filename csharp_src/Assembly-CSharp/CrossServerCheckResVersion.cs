using GameFramework;

public class CrossServerCheckResVersion : FsmBaseState
{
	private InGameCheckResVersionTools _checkResVersionTools;

	public override int id => 1;

	public CrossServerCheckResVersion(CrossServerFsmManager mgr)
		: base(mgr)
	{
	}

	public override void OnEnter(params object[] args)
	{
		Log.Info($"[CrossServerCheckResVersion]  OnEnter: time:{GameEntry.Timer.GetServerTime()}");
		_checkResVersionTools = new InGameCheckResVersionTools();
		_checkResVersionTools.Start(InGameCheckResVersionResult);
	}

	public override void OnExit()
	{
		Log.Info("[CrossServerCheckResVersion] OnExit");
		if (_checkResVersionTools != null)
		{
			_checkResVersionTools.Dispose();
			_checkResVersionTools = null;
		}
	}

	public override void OnUpdate()
	{
		if (_checkResVersionTools != null)
		{
			_checkResVersionTools.Update();
		}
	}

	private void InGameCheckResVersionResult(InGameCheckResVersionTools.CheckResult result, string reason)
	{
		Log.Info($"[CrossServerCheckResVersion]  CheckResult:{result.ToString()}  time:{GameEntry.Timer.GetServerTime()} reason:{reason}");
		switch (result)
		{
		case InGameCheckResVersionTools.CheckResult.NoGateway:
			GotoLoadingError(1);
			break;
		case InGameCheckResVersionTools.CheckResult.TimeOut:
			GotoLoadingError(3, reason);
			break;
		case InGameCheckResVersionTools.CheckResult.HttpError:
			GotoLoadingError(2, reason);
			break;
		case InGameCheckResVersionTools.CheckResult.Error_UpdateType:
			GotoLoadingError(4, "update type = 1");
			break;
		case InGameCheckResVersionTools.CheckResult.Success_NeedUpdate:
			if (!NetworkURLConfig.IsOnline || GameEntry.Resource.SkipUpdateBundle)
			{
				GotoGSL();
				break;
			}
			GotoLoadingError(5, "");
			break;
		case InGameCheckResVersionTools.CheckResult.Success_NoNeedUpdate:
			GotoGSL();
			break;
		default:
			GotoLoadingError(4, $"result:{result} reason:{reason}");
			break;
		}
	}

	private void GotoGSL()
	{
		_mgr.SetState(2);
	}
}
