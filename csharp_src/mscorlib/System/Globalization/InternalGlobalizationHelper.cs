namespace System.Globalization;

internal class InternalGlobalizationHelper
{
	internal const long TicksPerMillisecond = 10000L;

	internal const long TicksPerTenthSecond = 1000000L;

	internal const long TicksPerSecond = 10000000L;

	internal const long MaxSeconds = 922337203685L;

	internal const long MinSeconds = -922337203685L;

	private const int DaysPerYear = 365;

	private const int DaysPer4Years = 1461;

	private const int DaysPer100Years = 36524;

	private const int DaysPer400Years = 146097;

	private const int DaysTo10000 = 3652059;

	private const long TicksPerMinute = 600000000L;

	private const long TicksPerHour = 36000000000L;

	private const long TicksPerDay = 864000000000L;

	internal const long MaxTicks = 3155378975999999999L;

	internal const long MinTicks = 0L;

	internal const long MaxMilliSeconds = 922337203685477L;

	internal const long MinMilliSeconds = -922337203685477L;

	internal const int StringBuilderDefaultCapacity = 16;

	internal const long MaxOffset = 504000000000L;

	internal const long MinOffset = -504000000000L;

	internal static long TimeToTicks(int hour, int minute, int second)
	{
		long num = (long)hour * 3600L + (long)minute * 60L + second;
		if (num > 922337203685L || num < -922337203685L)
		{
			throw new ArgumentOutOfRangeException(null, "TimeSpan overflowed because the duration is too long.");
		}
		return num * 10000000;
	}
}
