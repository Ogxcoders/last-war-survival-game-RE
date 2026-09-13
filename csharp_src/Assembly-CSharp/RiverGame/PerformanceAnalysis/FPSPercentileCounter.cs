using System;
using UnityEngine;

namespace RiverGame.PerformanceAnalysis;

public class FPSPercentileCounter : FPSCounterBase
{
	private int m_FrameRange;

	private uint[] m_FrameCounts;

	private uint m_TotalFrameCount;

	public FPSPercentileCounter(int frameRange)
	{
		m_FrameRange = frameRange;
		m_FrameCounts = new uint[Math.Max(0, frameRange + 1)];
	}

	public override void StartNew()
	{
		Array.Clear(m_FrameCounts, 0, m_FrameCounts.Length);
		m_TotalFrameCount = 0u;
	}

	protected override void Count()
	{
		m_FrameCounts[Mathf.Min(Mathf.RoundToInt(1f / ForegroundTimer.unscaledDeltaTime), m_FrameRange)]++;
		m_TotalFrameCount++;
	}

	public int GetFramePercentile(float percentile)
	{
		float num = (float)m_TotalFrameCount * Mathf.Clamp01(percentile / 100f);
		long num2 = 0L;
		for (int i = 0; i < m_FrameCounts.Length; i++)
		{
			num2 += m_FrameCounts[i];
			if ((float)num2 >= num)
			{
				return i;
			}
		}
		return m_FrameRange;
	}
}
