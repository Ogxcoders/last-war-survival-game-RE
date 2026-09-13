using System.Collections.Generic;

namespace ThinkingAnalytics;

public class TDFirstEvent : ThinkingAnalyticsEvent
{
	public TDFirstEvent(string eventName, Dictionary<string, object> properties)
		: base(eventName, properties)
	{
		base.EventType = Type.FIRST;
	}

	public void SetFirstCheckId(string firstCheckId)
	{
		base.ExtraId = firstCheckId;
	}
}
