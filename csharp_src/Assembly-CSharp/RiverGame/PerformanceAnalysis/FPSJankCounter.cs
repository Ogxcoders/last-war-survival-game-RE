using System;
using System.Runtime.CompilerServices;

namespace RiverGame.PerformanceAnalysis;

public class FPSJankCounter : FPSCounterBase
{
	private const float FilmFrameTime1 = 1f / 24f;

	private const float FilmFrameTime2 = 1f / 12f;

	private const float FilmFrameTime3 = 0.125f;

	private const int FrameTimeLength = 3;

	private float[] m_FrameTimes = new float[3];

	private int m_FrameTimeIndex;

	public float startTime
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private set;
	}

	public int smallJankCount
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private set;
	}

	public int bigJankCount
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private set;
	}

	public float smallJankTime
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private set;
	}

	public float bigJankTime
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private set;
	}

	public override void StartNew()
	{
		smallJankCount = 0;
		bigJankCount = 0;
		smallJankTime = 0f;
		bigJankTime = 0f;
		Array.Clear(m_FrameTimes, 0, 3);
		m_FrameTimeIndex = 0;
		startTime = ForegroundTimer.unscaledTime;
	}

	protected override void Count()
	{
		float unscaledDeltaTime = ForegroundTimer.unscaledDeltaTime;
		if (m_FrameTimeIndex >= 3)
		{
			float num = 0f;
			for (int i = 0; i < 3; i++)
			{
				num += m_FrameTimes[i];
			}
			float num2 = num / 3f;
			if (unscaledDeltaTime > num2 * 2f)
			{
				if (unscaledDeltaTime > 0.125f)
				{
					bigJankTime += unscaledDeltaTime;
					bigJankCount++;
				}
				else if (unscaledDeltaTime > 1f / 12f)
				{
					smallJankTime += unscaledDeltaTime;
					smallJankCount++;
				}
			}
		}
		m_FrameTimes[m_FrameTimeIndex++ % 3] = unscaledDeltaTime;
	}

	public float GetDuration()
	{
		return ForegroundTimer.unscaledTime - startTime;
	}
}
