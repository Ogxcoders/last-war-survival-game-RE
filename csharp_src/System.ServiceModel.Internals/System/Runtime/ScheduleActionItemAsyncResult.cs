namespace System.Runtime;

internal abstract class ScheduleActionItemAsyncResult : AsyncResult
{
	private static Action<object> doWork = DoWork;

	protected ScheduleActionItemAsyncResult(AsyncCallback callback, object state)
		: base(callback, state)
	{
	}

	protected void Schedule()
	{
		ActionItem.Schedule(doWork, this);
	}

	private static void DoWork(object state)
	{
		ScheduleActionItemAsyncResult scheduleActionItemAsyncResult = (ScheduleActionItemAsyncResult)state;
		Exception ex = null;
		try
		{
			scheduleActionItemAsyncResult.OnDoWork();
		}
		catch (Exception ex2)
		{
			if (Fx.IsFatal(ex2))
			{
				throw;
			}
			ex = ex2;
		}
		scheduleActionItemAsyncResult.Complete(completedSynchronously: false, ex);
	}

	protected abstract void OnDoWork();

	public static void End(IAsyncResult result)
	{
		AsyncResult.End<ScheduleActionItemAsyncResult>(result);
	}
}
