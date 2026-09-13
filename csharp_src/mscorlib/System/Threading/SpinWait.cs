using Internal.Runtime.Augments;

namespace System.Threading;

public struct SpinWait
{
	internal const int YieldThreshold = 10;

	private const int Sleep0EveryHowManyYields = 5;

	internal const int DefaultSleep1Threshold = 20;

	internal static readonly int SpinCountforSpinBeforeWait = (PlatformHelper.IsSingleProcessor ? 1 : 35);

	internal const int Sleep1ThresholdForLongSpinBeforeWait = 40;

	private int _count;

	public int Count
	{
		get
		{
			return _count;
		}
		internal set
		{
			_count = value;
		}
	}

	public bool NextSpinWillYield
	{
		get
		{
			if (_count < 10)
			{
				return PlatformHelper.IsSingleProcessor;
			}
			return true;
		}
	}

	public void SpinOnce()
	{
		SpinOnceCore(20);
	}

	public void SpinOnce(int sleep1Threshold)
	{
		if (sleep1Threshold < -1)
		{
			throw new ArgumentOutOfRangeException("sleep1Threshold", sleep1Threshold, "Number must be either non-negative and less than or equal to Int32.MaxValue or -1.");
		}
		if (sleep1Threshold >= 0 && sleep1Threshold < 10)
		{
			sleep1Threshold = 10;
		}
		SpinOnceCore(sleep1Threshold);
	}

	private void SpinOnceCore(int sleep1Threshold)
	{
		if ((_count >= 10 && ((_count >= sleep1Threshold && sleep1Threshold >= 0) || (_count - 10) % 2 == 0)) || PlatformHelper.IsSingleProcessor)
		{
			if (_count >= sleep1Threshold && sleep1Threshold >= 0)
			{
				RuntimeThread.Sleep(1);
			}
			else if (((_count >= 10) ? ((_count - 10) / 2) : _count) % 5 == 4)
			{
				RuntimeThread.Sleep(0);
			}
			else
			{
				RuntimeThread.Yield();
			}
		}
		else
		{
			int num = RuntimeThread.OptimalMaxSpinWaitsPerSpinIteration;
			if (_count <= 30 && 1 << _count < num)
			{
				num = 1 << _count;
			}
			RuntimeThread.SpinWait(num);
		}
		_count = ((_count == int.MaxValue) ? 10 : (_count + 1));
	}

	public void Reset()
	{
		_count = 0;
	}

	public static void SpinUntil(Func<bool> condition)
	{
		SpinUntil(condition, -1);
	}

	public static bool SpinUntil(Func<bool> condition, TimeSpan timeout)
	{
		long num = (long)timeout.TotalMilliseconds;
		if (num < -1 || num > int.MaxValue)
		{
			throw new ArgumentOutOfRangeException("timeout", timeout, "The timeout must represent a value between -1 and Int32.MaxValue, inclusive.");
		}
		return SpinUntil(condition, (int)num);
	}

	public static bool SpinUntil(Func<bool> condition, int millisecondsTimeout)
	{
		if (millisecondsTimeout < -1)
		{
			throw new ArgumentOutOfRangeException("millisecondsTimeout", millisecondsTimeout, "The timeout must represent a value between -1 and Int32.MaxValue, inclusive.");
		}
		if (condition == null)
		{
			throw new ArgumentNullException("condition", "The condition argument is null.");
		}
		uint num = 0u;
		if (millisecondsTimeout != 0 && millisecondsTimeout != -1)
		{
			num = TimeoutHelper.GetTime();
		}
		SpinWait spinWait = default(SpinWait);
		while (!condition())
		{
			if (millisecondsTimeout == 0)
			{
				return false;
			}
			spinWait.SpinOnce();
			if (millisecondsTimeout != -1 && spinWait.NextSpinWillYield && millisecondsTimeout <= TimeoutHelper.GetTime() - num)
			{
				return false;
			}
		}
		return true;
	}
}
