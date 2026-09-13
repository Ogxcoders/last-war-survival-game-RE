using System.Diagnostics;

namespace System.Threading.Tasks;

[DebuggerDisplay("ShouldExitCurrentIteration = {ShouldExitCurrentIteration}")]
public class ParallelLoopState
{
	private readonly ParallelLoopStateFlags _flagsBase;

	internal virtual bool InternalShouldExitCurrentIteration
	{
		get
		{
			throw new NotSupportedException("This method is not supported.");
		}
	}

	public bool ShouldExitCurrentIteration => InternalShouldExitCurrentIteration;

	public bool IsStopped => (_flagsBase.LoopStateFlags & 4) != 0;

	public bool IsExceptional => (_flagsBase.LoopStateFlags & 1) != 0;

	internal virtual long? InternalLowestBreakIteration
	{
		get
		{
			throw new NotSupportedException("This method is not supported.");
		}
	}

	public long? LowestBreakIteration => InternalLowestBreakIteration;

	internal ParallelLoopState(ParallelLoopStateFlags fbase)
	{
		_flagsBase = fbase;
	}

	public void Stop()
	{
		_flagsBase.Stop();
	}

	internal virtual void InternalBreak()
	{
		throw new NotSupportedException("This method is not supported.");
	}

	public void Break()
	{
		InternalBreak();
	}

	internal static void Break(int iteration, ParallelLoopStateFlags32 pflags)
	{
		int oldState = 0;
		if (!pflags.AtomicLoopStateUpdate(2, 13, ref oldState))
		{
			if ((oldState & 4) != 0)
			{
				throw new InvalidOperationException("Break was called after Stop was called.");
			}
			return;
		}
		int lowestBreakIteration = pflags._lowestBreakIteration;
		if (iteration >= lowestBreakIteration)
		{
			return;
		}
		SpinWait spinWait = default(SpinWait);
		while (Interlocked.CompareExchange(ref pflags._lowestBreakIteration, iteration, lowestBreakIteration) != lowestBreakIteration)
		{
			spinWait.SpinOnce();
			lowestBreakIteration = pflags._lowestBreakIteration;
			if (iteration > lowestBreakIteration)
			{
				break;
			}
		}
	}

	internal static void Break(long iteration, ParallelLoopStateFlags64 pflags)
	{
		int oldState = 0;
		if (!pflags.AtomicLoopStateUpdate(2, 13, ref oldState))
		{
			if ((oldState & 4) != 0)
			{
				throw new InvalidOperationException("Break was called after Stop was called.");
			}
			return;
		}
		long lowestBreakIteration = pflags.LowestBreakIteration;
		if (iteration >= lowestBreakIteration)
		{
			return;
		}
		SpinWait spinWait = default(SpinWait);
		while (Interlocked.CompareExchange(ref pflags._lowestBreakIteration, iteration, lowestBreakIteration) != lowestBreakIteration)
		{
			spinWait.SpinOnce();
			lowestBreakIteration = pflags.LowestBreakIteration;
			if (iteration > lowestBreakIteration)
			{
				break;
			}
		}
	}
}
