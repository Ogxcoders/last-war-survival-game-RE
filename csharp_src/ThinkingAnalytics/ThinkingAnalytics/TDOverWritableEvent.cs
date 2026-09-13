using System.Collections.Generic;

namespace ThinkingAnalytics;

public class TDOverWritableEvent : ThinkingAnalyticsEvent
{
	public TDOverWritableEvent(string eventName, Dictionary<string, object> properties, string eventId)
		: base(eventName, properties)
	{
		base.EventType = Type.OVERWRITABLE;
		base.ExtraId = eventId;
	}
}
