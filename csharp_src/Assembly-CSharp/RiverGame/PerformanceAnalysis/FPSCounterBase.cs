using UnityEngine;

namespace RiverGame.PerformanceAnalysis;

public abstract class FPSCounterBase
{
	private int m_LastFrameCount;

	public bool TryCount()
	{
		if (Time.frameCount > m_LastFrameCount && ForegroundTimer.unscaledDeltaTime > 0f)
		{
			m_LastFrameCount = Time.frameCount;
			Count();
			return true;
		}
		return false;
	}

	public abstract void StartNew();

	protected abstract void Count();
}
