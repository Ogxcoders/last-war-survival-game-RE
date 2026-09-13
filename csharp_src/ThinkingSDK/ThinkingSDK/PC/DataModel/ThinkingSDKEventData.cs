using System;
using System.Collections.Generic;
using ThinkingSDK.PC.Constant;
using ThinkingSDK.PC.Time;

namespace ThinkingSDK.PC.DataModel;

public class ThinkingSDKEventData : ThinkingSDKBaseData
{
	private DateTime mEventTime;

	private TimeZoneInfo mTimeZone;

	private float mDuration;

	public void SetEventTime(DateTime dateTime)
	{
		mEventTime = dateTime;
	}

	public void SetTimeZone(TimeZoneInfo timeZone)
	{
		mTimeZone = timeZone;
	}

	public DateTime Time()
	{
		return mEventTime;
	}

	public ThinkingSDKEventData(string eventName)
		: base(eventName)
	{
	}

	public ThinkingSDKEventData(ThinkingSDKTimeInter time, string eventName)
		: base(time, eventName)
	{
	}

	public ThinkingSDKEventData(ThinkingSDKTimeInter time, string eventName, Dictionary<string, object> properties)
		: base(time, eventName, properties)
	{
	}

	public override string GetDataType()
	{
		return "track";
	}

	public void SetDuration(float duration)
	{
		mDuration = duration;
	}

	public override Dictionary<string, object> ToDictionary()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary[ThinkingSDKConstant.TYPE] = GetDataType();
		dictionary[ThinkingSDKConstant.TIME] = EventTime().GetTime(mTimeZone);
		dictionary[ThinkingSDKConstant.DISTINCT_ID] = DistinctID();
		if (!string.IsNullOrEmpty(EventName()))
		{
			dictionary[ThinkingSDKConstant.EVENT_NAME] = EventName();
		}
		if (!string.IsNullOrEmpty(AccountID()))
		{
			dictionary[ThinkingSDKConstant.ACCOUNT_ID] = AccountID();
		}
		dictionary[ThinkingSDKConstant.UUID] = UUID();
		Dictionary<string, object> dictionary2 = Properties();
		dictionary2[ThinkingSDKConstant.ZONE_OFFSET] = EventTime().GetZoneOffset(mTimeZone);
		if (mDuration != 0f)
		{
			dictionary2[ThinkingSDKConstant.DURATION] = mDuration;
		}
		dictionary[ThinkingSDKConstant.PROPERTIES] = dictionary2;
		return dictionary;
	}
}
