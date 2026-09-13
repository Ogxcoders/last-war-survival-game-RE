using System;

namespace ThinkingSDK.PC.Time;

public interface ThinkingSDKTimeInter
{
	string GetTime(TimeZoneInfo timeZone);

	double GetZoneOffset(TimeZoneInfo timeZone);
}
