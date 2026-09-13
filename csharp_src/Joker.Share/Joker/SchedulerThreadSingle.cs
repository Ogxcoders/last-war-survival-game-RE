using System.Threading;

namespace Joker;

public class SchedulerThreadSingle : SchedulerThreadBase
{
	protected readonly Thread _thread;

	public SchedulerThreadSingle(ESchedulerType type, int id)
		: base(type, id)
	{
		_thread = new Thread(_Loop);
	}

	public override void Start()
	{
		_thread.Start();
	}

	private void _Loop()
	{
		while (!base.IsDisposed)
		{
			Update();
			LateUpdate();
			Thread.Sleep(1);
		}
	}

	public override void Dispose()
	{
		if (!base.IsDisposed)
		{
			base.Dispose();
			_thread.Join();
		}
	}
}
