namespace Joker;

public class SchedulerThreadMain : SchedulerThreadBase
{
	public SchedulerThreadMain(ESchedulerType type, int id)
		: base(type, id)
	{
	}

	public override void Start()
	{
	}

	internal void DoUpdate()
	{
		Update();
	}

	internal void DoLateUpdate()
	{
		LateUpdate();
	}
}
