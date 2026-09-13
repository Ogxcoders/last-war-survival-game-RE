using System;

namespace ThinkingSDK.PC.Time;

public class ThinkingSDKDefinedTime : ThinkingSDKTimeInter
{
	private string mTime;

	private double mZoneOffset;

	public ThinkingSDKDefinedTime(string time, double zoneOffset)
	{
		mTime = time;
		mZoneOffset = zoneOffset;
	}

	public string GetTime(TimeZoneInfo timeZone)
	{
		return mTime;
	}

	public double GetZoneOffset(TimeZoneInfo timeZone)
	{
		return mZoneOffset;
	}
}
