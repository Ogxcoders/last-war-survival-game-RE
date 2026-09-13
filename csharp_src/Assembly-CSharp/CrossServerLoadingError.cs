using GameFramework;

public class CrossServerLoadingError : FsmBaseState
{
	public override int id => 9;

	public CrossServerLoadingError(CrossServerFsmManager mgr)
		: base(mgr)
	{
	}

	public override void OnEnter(params object[] args)
	{
		int num = -1;
		if (args.Length != 0)
		{
			num = (int)args[0];
		}
		switch (num)
		{
		case 0:
			_mgr.Dispose();
			break;
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
		case 10:
		case 11:
			Log.Error($"Cross server error code:{num}");
			ApplicationLaunch.Instance.ReStartGame();
			_mgr.Dispose();
			break;
		}
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
	}
}
