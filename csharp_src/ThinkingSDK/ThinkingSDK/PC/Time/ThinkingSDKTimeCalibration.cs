using System;

namespace ThinkingSDK.PC.Time;

public class ThinkingSDKTimeCalibration
{
	public long mStartTime;

	public long mSystemElapsedRealtime;

	public DateTime NowDate()
	{
		long num = Environment.TickCount - mSystemElapsedRealtime + mStartTime;
		return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(num);
	}
}
