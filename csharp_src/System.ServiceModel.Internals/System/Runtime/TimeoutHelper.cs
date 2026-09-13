using System.Threading;

namespace System.Runtime;

internal struct TimeoutHelper
{
	private DateTime deadline;

	private bool deadlineSet;

	private TimeSpan originalTimeout;

	public static readonly TimeSpan MaxWait = TimeSpan.FromMilliseconds(2147483647.0);

	public TimeSpan OriginalTimeout => originalTimeout;

	public TimeoutHelper(TimeSpan timeout)
	{
		originalTimeout = timeout;
		deadline = DateTime.MaxValue;
		deadlineSet = timeout == TimeSpan.MaxValue;
	}

	public static bool IsTooLarge(TimeSpan timeout)
	{
		if (timeout > MaxWait)
		{
			return timeout != TimeSpan.MaxValue;
		}
		return false;
	}

	public static TimeSpan FromMilliseconds(int milliseconds)
	{
		if (milliseconds == -1)
		{
			return TimeSpan.MaxValue;
		}
		return TimeSpan.FromMilliseconds(milliseconds);
	}

	public static int ToMilliseconds(TimeSpan timeout)
	{
		if (timeout == TimeSpan.MaxValue)
		{
			return -1;
		}
		long num = Ticks.FromTimeSpan(timeout);
		if (num / 10000 > int.MaxValue)
		{
			return int.MaxValue;
		}
		return Ticks.ToMilliseconds(num);
	}

	public static TimeSpan Min(TimeSpan val1, TimeSpan val2)
	{
		if (val1 > val2)
		{
			return val2;
		}
		return val1;
	}

	public static TimeSpan Add(TimeSpan timeout1, TimeSpan timeout2)
	{
		return Ticks.ToTimeSpan(Ticks.Add(Ticks.FromTimeSpan(timeout1), Ticks.FromTimeSpan(timeout2)));
	}

	public static DateTime Add(DateTime time, TimeSpan timeout)
	{
		if (timeout >= TimeSpan.Zero && DateTime.MaxValue - time <= timeout)
		{
			return DateTime.MaxValue;
		}
		if (timeout <= TimeSpan.Zero && DateTime.MinValue - time >= timeout)
		{
			return DateTime.MinValue;
		}
		return time + timeout;
	}

	public static DateTime Subtract(DateTime time, TimeSpan timeout)
	{
		return Add(time, TimeSpan.Zero - timeout);
	}

	public static TimeSpan Divide(TimeSpan timeout, int factor)
	{
		if (timeout == TimeSpan.MaxValue)
		{
			return TimeSpan.MaxValue;
		}
		return Ticks.ToTimeSpan(Ticks.FromTimeSpan(timeout) / factor + 1);
	}

	public TimeSpan RemainingTime()
	{
		if (!deadlineSet)
		{
			SetDeadline();
			return originalTimeout;
		}
		if (deadline == DateTime.MaxValue)
		{
			return TimeSpan.MaxValue;
		}
		TimeSpan timeSpan = deadline - DateTime.UtcNow;
		if (timeSpan <= TimeSpan.Zero)
		{
			return TimeSpan.Zero;
		}
		return timeSpan;
	}

	public TimeSpan ElapsedTime()
	{
		return originalTimeout - RemainingTime();
	}

	private void SetDeadline()
	{
		deadline = DateTime.UtcNow + originalTimeout;
		deadlineSet = true;
	}

	public static void ThrowIfNegativeArgument(TimeSpan timeout)
	{
		ThrowIfNegativeArgument(timeout, "timeout");
	}

	public static void ThrowIfNegativeArgument(TimeSpan timeout, string argumentName)
	{
		if (timeout < TimeSpan.Zero)
		{
			throw Fx.Exception.ArgumentOutOfRange(argumentName, timeout, InternalSR.TimeoutMustBeNonNegative(argumentName, timeout));
		}
	}

	public static void ThrowIfNonPositiveArgument(TimeSpan timeout)
	{
		ThrowIfNonPositiveArgument(timeout, "timeout");
	}

	public static void ThrowIfNonPositiveArgument(TimeSpan timeout, string argumentName)
	{
		if (timeout <= TimeSpan.Zero)
		{
			throw Fx.Exception.ArgumentOutOfRange(argumentName, timeout, InternalSR.TimeoutMustBePositive(argumentName, timeout));
		}
	}

	public static bool WaitOne(WaitHandle waitHandle, TimeSpan timeout)
	{
		ThrowIfNegativeArgument(timeout);
		if (timeout == TimeSpan.MaxValue)
		{
			waitHandle.WaitOne();
			return true;
		}
		return waitHandle.WaitOne(timeout, exitContext: false);
	}
}
