using System;

namespace ThinkingSDK.PC.Time;

public class ThinkingSDKTimestampCalibration : ThinkingSDKTimeCalibration
{
	public ThinkingSDKTimestampCalibration(long timestamp)
	{
		mStartTime = timestamp;
		mSystemElapsedRealtime = Environment.TickCount;
	}
}
