using System;
using ThinkingSDK.PC.Utils;

namespace ThinkingSDK.PC.Time;

public class ThinkingSDKCalibratedTime : ThinkingSDKTimeInter
{
	private ThinkingSDKTimeCalibration mCalibratedTime;

	private long mSystemElapsedRealtime;

	private TimeZoneInfo mTimeZone;

	private DateTime mDate;

	public ThinkingSDKCalibratedTime(ThinkingSDKTimeCalibration calibrateTimeInter, TimeZoneInfo timeZoneInfo)
	{
		mCalibratedTime = calibrateTimeInter;
		mTimeZone = timeZoneInfo;
		mDate = mCalibratedTime.NowDate();
		ThinkingSDKLogger.Print("CurrentDate = " + mDate.ToString("UTC yyyy-MM-dd HH:mm:ss.fff"));
	}

	public string GetTime(TimeZoneInfo timeZone)
	{
		if (timeZone == null)
		{
			return ThinkingSDKUtil.FormatDate(mDate, mTimeZone);
		}
		return ThinkingSDKUtil.FormatDate(mDate, timeZone);
	}

	public double GetZoneOffset(TimeZoneInfo timeZone)
	{
		if (timeZone == null)
		{
			return ThinkingSDKUtil.ZoneOffset(mDate, mTimeZone);
		}
		return ThinkingSDKUtil.ZoneOffset(mDate, timeZone);
	}
}
