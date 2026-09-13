using System;

namespace RiverGame.PerformanceAnalysis;

public class FPSShouldHighTimeCounter : FPSCounterBase
{
	public float highFPSTime;

	public float highFPSTime2;

	public static Func<bool> ShouldHighFps;

	public static Func<bool> ShouldHighFps2;

	public override void StartNew()
	{
		highFPSTime = 0f;
		highFPSTime2 = 0f;
	}

	protected override void Count()
	{
		if (ShouldHighFps != null && ShouldHighFps())
		{
			highFPSTime += ForegroundTimer.unscaledDeltaTime;
		}
		if (ShouldHighFps2 != null && ShouldHighFps2())
		{
			highFPSTime2 += ForegroundTimer.unscaledDeltaTime;
		}
	}
}
