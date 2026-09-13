namespace System.Threading.Tasks;

internal class ParallelLoopStateFlags
{
	internal const int ParallelLoopStateNone = 0;

	internal const int ParallelLoopStateExceptional = 1;

	internal const int ParallelLoopStateBroken = 2;

	internal const int ParallelLoopStateStopped = 4;

	internal const int ParallelLoopStateCanceled = 8;

	private volatile int _loopStateFlags;

	internal int LoopStateFlags => _loopStateFlags;

	internal bool AtomicLoopStateUpdate(int newState, int illegalStates)
	{
		int oldState = 0;
		return AtomicLoopStateUpdate(newState, illegalStates, ref oldState);
	}

	internal bool AtomicLoopStateUpdate(int newState, int illegalStates, ref int oldState)
	{
		SpinWait spinWait = default(SpinWait);
		while (true)
		{
			oldState = _loopStateFlags;
			if ((oldState & illegalStates) != 0)
			{
				return false;
			}
			if (Interlocked.CompareExchange(ref _loopStateFlags, oldState | newState, oldState) == oldState)
			{
				break;
			}
			spinWait.SpinOnce();
		}
		return true;
	}

	internal void SetExceptional()
	{
		AtomicLoopStateUpdate(1, 0);
	}

	internal void Stop()
	{
		if (!AtomicLoopStateUpdate(4, 2))
		{
			throw new InvalidOperationException("Stop was called after Break was called.");
		}
	}

	internal bool Cancel()
	{
		return AtomicLoopStateUpdate(8, 0);
	}
}
