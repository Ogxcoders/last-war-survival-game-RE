using System.Collections.Generic;
using ThinkingSDK.PC.Constant;

namespace ThinkingSDK.PC.DataModel;

public class ThinkingSDKOverWritableEvent : ThinkingSDKEventData
{
	private string mEventID;

	public ThinkingSDKOverWritableEvent(string eventName, string eventID)
		: base(eventName)
	{
		mEventID = eventID;
	}

	public override string GetDataType()
	{
		return "track_overwrite";
	}

	public override Dictionary<string, object> ToDictionary()
	{
		Dictionary<string, object> dictionary = base.ToDictionary();
		dictionary[ThinkingSDKConstant.EVENT_ID] = mEventID;
		return dictionary;
	}
}
