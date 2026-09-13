using System;
using System.Collections.Generic;
using ThinkingSDK.PC.Time;

namespace ThinkingSDK.PC.DataModel;

public abstract class ThinkingSDKBaseData
{
	private string mType;

	private ThinkingSDKTimeInter mTime;

	private string mDistinctID;

	private string mEventName;

	private string mAccountID;

	private string mUUID;

	private Dictionary<string, object> mProperties = new Dictionary<string, object>();

	public Dictionary<string, object> Properties()
	{
		return mProperties;
	}

	public void SetEventName(string eventName)
	{
		mEventName = eventName;
	}

	public void SetEventType(string eventType)
	{
		mType = eventType;
	}

	public string EventName()
	{
		return mEventName;
	}

	public void SetTime(ThinkingSDKTimeInter time)
	{
		mTime = time;
	}

	public ThinkingSDKTimeInter EventTime()
	{
		return mTime;
	}

	public void SetDataType(string type)
	{
		mType = type;
	}

	public virtual string GetDataType()
	{
		return mType;
	}

	public string AccountID()
	{
		return mAccountID;
	}

	public string DistinctID()
	{
		return mDistinctID;
	}

	public void SetAccountID(string accuntID)
	{
		mAccountID = accuntID;
	}

	public void SetDistinctID(string distinctID)
	{
		mDistinctID = distinctID;
	}

	public string UUID()
	{
		return mUUID;
	}

	public ThinkingSDKBaseData()
	{
	}

	public ThinkingSDKBaseData(ThinkingSDKTimeInter time, string eventName)
	{
		SetBaseData(eventName);
		SetTime(time);
	}

	public ThinkingSDKBaseData(string eventName)
	{
		SetBaseData(eventName);
	}

	public void SetBaseData(string eventName)
	{
		mEventName = eventName;
		mUUID = Guid.NewGuid().ToString();
	}

	public ThinkingSDKBaseData(ThinkingSDKTimeInter time, string eventName, Dictionary<string, object> properties)
		: this(time, eventName)
	{
		if (properties != null)
		{
			SetProperties(properties);
		}
	}

	public abstract Dictionary<string, object> ToDictionary();

	public void SetProperties(Dictionary<string, object> properties, bool isOverwrite = true)
	{
		if (isOverwrite)
		{
			foreach (KeyValuePair<string, object> property in properties)
			{
				mProperties[property.Key] = property.Value;
			}
			return;
		}
		foreach (KeyValuePair<string, object> property2 in properties)
		{
			if (!mProperties.ContainsKey(property2.Key))
			{
				mProperties[property2.Key] = property2.Value;
			}
		}
	}
}
