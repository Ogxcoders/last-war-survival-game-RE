using System.Security;

namespace System.Threading;

internal sealed class QueueUserWorkItemCallback : IThreadPoolWorkItem
{
	private WaitCallback callback;

	private ExecutionContext context;

	private object state;

	[SecurityCritical]
	internal static ContextCallback ccb = WaitCallback_Context;

	[SecurityCritical]
	internal QueueUserWorkItemCallback(WaitCallback waitCallback, object stateObj, bool compressStack, ref StackCrawlMark stackMark)
	{
		callback = waitCallback;
		state = stateObj;
		if (compressStack && !ExecutionContext.IsFlowSuppressed())
		{
			context = ExecutionContext.Capture(ref stackMark, ExecutionContext.CaptureOptions.IgnoreSyncCtx | ExecutionContext.CaptureOptions.OptimizeDefaultCase);
		}
	}

	internal QueueUserWorkItemCallback(WaitCallback waitCallback, object stateObj, ExecutionContext ec)
	{
		callback = waitCallback;
		state = stateObj;
		context = ec;
	}

	[SecurityCritical]
	void IThreadPoolWorkItem.ExecuteWorkItem()
	{
		if (context == null)
		{
			WaitCallback waitCallback = callback;
			callback = null;
			waitCallback(state);
		}
		else
		{
			ExecutionContext.Run(context, ccb, this, preserveSyncCtx: true);
		}
	}

	[SecurityCritical]
	void IThreadPoolWorkItem.MarkAborted(ThreadAbortException tae)
	{
	}

	[SecurityCritical]
	private static void WaitCallback_Context(object state)
	{
		QueueUserWorkItemCallback queueUserWorkItemCallback = (QueueUserWorkItemCallback)state;
		queueUserWorkItemCallback.callback(queueUserWorkItemCallback.state);
	}
}
