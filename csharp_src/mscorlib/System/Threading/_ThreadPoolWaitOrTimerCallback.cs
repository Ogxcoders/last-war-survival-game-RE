using System.Security;

namespace System.Threading;

internal class _ThreadPoolWaitOrTimerCallback
{
	private WaitOrTimerCallback _waitOrTimerCallback;

	private ExecutionContext _executionContext;

	private object _state;

	[SecurityCritical]
	private static ContextCallback _ccbt;

	[SecurityCritical]
	private static ContextCallback _ccbf;

	[SecuritySafeCritical]
	static _ThreadPoolWaitOrTimerCallback()
	{
		_ccbt = WaitOrTimerCallback_Context_t;
		_ccbf = WaitOrTimerCallback_Context_f;
	}

	[SecurityCritical]
	internal _ThreadPoolWaitOrTimerCallback(WaitOrTimerCallback waitOrTimerCallback, object state, bool compressStack, ref StackCrawlMark stackMark)
	{
		_waitOrTimerCallback = waitOrTimerCallback;
		_state = state;
		if (compressStack && !ExecutionContext.IsFlowSuppressed())
		{
			_executionContext = ExecutionContext.Capture(ref stackMark, ExecutionContext.CaptureOptions.IgnoreSyncCtx | ExecutionContext.CaptureOptions.OptimizeDefaultCase);
		}
	}

	[SecurityCritical]
	private static void WaitOrTimerCallback_Context_t(object state)
	{
		WaitOrTimerCallback_Context(state, timedOut: true);
	}

	[SecurityCritical]
	private static void WaitOrTimerCallback_Context_f(object state)
	{
		WaitOrTimerCallback_Context(state, timedOut: false);
	}

	private static void WaitOrTimerCallback_Context(object state, bool timedOut)
	{
		_ThreadPoolWaitOrTimerCallback threadPoolWaitOrTimerCallback = (_ThreadPoolWaitOrTimerCallback)state;
		threadPoolWaitOrTimerCallback._waitOrTimerCallback(threadPoolWaitOrTimerCallback._state, timedOut);
	}

	[SecurityCritical]
	internal static void PerformWaitOrTimerCallback(object state, bool timedOut)
	{
		_ThreadPoolWaitOrTimerCallback threadPoolWaitOrTimerCallback = (_ThreadPoolWaitOrTimerCallback)state;
		if (threadPoolWaitOrTimerCallback._executionContext == null)
		{
			threadPoolWaitOrTimerCallback._waitOrTimerCallback(threadPoolWaitOrTimerCallback._state, timedOut);
			return;
		}
		using ExecutionContext executionContext = threadPoolWaitOrTimerCallback._executionContext.CreateCopy();
		if (timedOut)
		{
			ExecutionContext.Run(executionContext, _ccbt, threadPoolWaitOrTimerCallback, preserveSyncCtx: true);
		}
		else
		{
			ExecutionContext.Run(executionContext, _ccbf, threadPoolWaitOrTimerCallback, preserveSyncCtx: true);
		}
	}
}
