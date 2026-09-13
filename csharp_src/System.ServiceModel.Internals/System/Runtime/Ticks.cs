using System.Runtime.Interop;
using System.Security;

namespace System.Runtime;

internal static class Ticks
{
	public static long Now
	{
		[SecuritySafeCritical]
		get
		{
			UnsafeNativeMethods.GetSystemTimeAsFileTime(out var time);
			return time;
		}
	}

	public static long FromMilliseconds(int milliseconds)
	{
		checked
		{
			return unchecked((long)milliseconds) * 10000L;
		}
	}

	public static int ToMilliseconds(long ticks)
	{
		checked
		{
			return (int)unchecked(ticks / 10000);
		}
	}

	public static long FromTimeSpan(TimeSpan duration)
	{
		return duration.Ticks;
	}

	public static TimeSpan ToTimeSpan(long ticks)
	{
		return new TimeSpan(ticks);
	}

	public static long Add(long firstTicks, long secondTicks)
	{
		if (firstTicks == long.MaxValue || firstTicks == long.MinValue)
		{
			return firstTicks;
		}
		if (secondTicks == long.MaxValue || secondTicks == long.MinValue)
		{
			return secondTicks;
		}
		if (firstTicks >= 0 && long.MaxValue - firstTicks <= secondTicks)
		{
			return 9223372036854775806L;
		}
		if (firstTicks <= 0 && long.MinValue - firstTicks >= secondTicks)
		{
			return -9223372036854775807L;
		}
		return checked(firstTicks + secondTicks);
	}
}
