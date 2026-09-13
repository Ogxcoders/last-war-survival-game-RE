using System.Collections.Generic;
using ThinkingSDK.PC.Constant;
using ThinkingSDK.PC.Time;

namespace ThinkingSDK.PC.DataModel;

public class ThinkingSDKUserData : ThinkingSDKBaseData
{
	public ThinkingSDKUserData(ThinkingSDKTimeInter time, string eventType, Dictionary<string, object> properties)
	{
		SetEventType(eventType);
		SetTime(time);
		SetBaseData(null);
		SetProperties(properties);
	}

	public override Dictionary<string, object> ToDictionary()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary[ThinkingSDKConstant.TYPE] = GetDataType();
		dictionary[ThinkingSDKConstant.TIME] = EventTime().GetTime(null);
		dictionary[ThinkingSDKConstant.DISTINCT_ID] = DistinctID();
		if (!string.IsNullOrEmpty(AccountID()))
		{
			dictionary[ThinkingSDKConstant.ACCOUNT_ID] = AccountID();
		}
		dictionary[ThinkingSDKConstant.UUID] = UUID();
		dictionary[ThinkingSDKConstant.PROPERTIES] = Properties();
		return dictionary;
	}
}
