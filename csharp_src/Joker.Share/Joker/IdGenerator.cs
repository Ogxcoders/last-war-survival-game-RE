using System;
using System.Threading;

namespace Joker;

public static class IdGenerator
{
	public const int KMaxZone = 1024;

	public const uint KMask12BIT = 4095u;

	public const uint KMask32BIT = uint.MaxValue;

	public const uint KMask20BIT = 1048575u;

	private static readonly long _msEpoch2022;

	private static long _msStructCounter;

	private static long _msIncrementCounter;

	static IdGenerator()
	{
		_msEpoch2022 = new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks / 10000;
	}

	private static uint TimeSince2022()
	{
		return (uint)((DateTime.UtcNow.Ticks / 10000 - _msEpoch2022) / 1000);
	}

	public static long GenerateId()
	{
		return IdStruct.ToLong(TimeSince2022(), value: (uint)(Interlocked.Increment(ref _msStructCounter) % 1048575), process: App.Process);
	}

	public static long GenerateIncrementId()
	{
		uint time = TimeSince2022();
		long num = Interlocked.Increment(ref _msIncrementCounter);
		return IdIncrement.ToLong(time, (uint)num % uint.MaxValue);
	}
}
