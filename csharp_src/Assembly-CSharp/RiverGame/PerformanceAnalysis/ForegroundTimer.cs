using System.Runtime.CompilerServices;
using UnityEngine;

namespace RiverGame.PerformanceAnalysis;

public static class ForegroundTimer
{
	private static int s_LastFrameCount;

	private static float s_LastFrameRealtime;

	private static float s_LastPausedTime;

	public static float unscaledTime
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private set;
	}

	public static float unscaledDeltaTime
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private set;
	}

	public static float realtimeSinceStartup
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return Time.realtimeSinceStartup - totalPauseTime;
		}
	}

	public static float lazyTimeSinceStartup
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return s_LastFrameRealtime - totalPauseTime;
		}
	}

	public static float lazyTimeSinceStartupWithPause
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return s_LastFrameRealtime;
		}
	}

	public static float totalPauseTime
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private set;
	}

	public static bool pause
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private set;
	}

	static ForegroundTimer()
	{
		s_LastPausedTime = -1f;
		totalPauseTime = 0f;
		s_LastFrameRealtime = realtimeSinceStartup;
		unscaledTime = s_LastFrameRealtime;
		unscaledDeltaTime = 0f;
	}

	public static void OnApplicationPause(bool pause)
	{
		ForegroundTimer.pause = pause;
		if (s_LastPausedTime < 0f && !pause)
		{
			s_LastPausedTime = 0f;
			return;
		}
		if (pause)
		{
			s_LastPausedTime = Time.realtimeSinceStartup;
			return;
		}
		float num = Time.realtimeSinceStartup - s_LastPausedTime;
		totalPauseTime += num;
	}

	public static void Tick()
	{
		if (pause)
		{
			s_LastPausedTime = Time.realtimeSinceStartup;
		}
		if (Time.frameCount > s_LastFrameCount)
		{
			float num = realtimeSinceStartup;
			unscaledDeltaTime = (num - s_LastFrameRealtime) / (float)(Time.frameCount - s_LastFrameCount);
			unscaledTime += unscaledDeltaTime;
			s_LastFrameCount = Time.frameCount;
			s_LastFrameRealtime = num;
		}
	}
}
