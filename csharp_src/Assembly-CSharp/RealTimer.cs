using System.Threading;
using BaseUtils;
using UnityEngine;

public class RealTimer
{
	public static bool useNativeRealTime;

	private static long _elapsedMilliseconds;

	public static volatile int frameCount;

	public static long elapsedMilliseconds => Interlocked.Read(ref _elapsedMilliseconds);

	public static long GetUnityRealtime()
	{
		return (long)((double)Time.realtimeSinceStartup * 1000.0);
	}

	public static void Reset()
	{
		long value = (useNativeRealTime ? GameEntry.Sdk.GetRealtime() : GetUnityRealtime());
		Interlocked.Exchange(ref _elapsedMilliseconds, value);
	}

	public static void Update()
	{
		long value = (useNativeRealTime ? GameEntry.Sdk.GetRealtime() : GetUnityRealtime());
		Interlocked.Exchange(ref _elapsedMilliseconds, value);
		BaseUtils.RealTimer.elapsedMilliseconds = elapsedMilliseconds;
	}
}
