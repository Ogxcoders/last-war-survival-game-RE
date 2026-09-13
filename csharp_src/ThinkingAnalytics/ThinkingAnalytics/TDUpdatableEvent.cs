using System.Collections.Generic;

namespace ThinkingAnalytics;

public class TDUpdatableEvent : ThinkingAnalyticsEvent
{
	public TDUpdatableEvent(string eventName, Dictionary<string, object> properties, string eventId)
		: base(eventName, properties)
	{
		base.EventType = Type.UPDATABLE;
		base.ExtraId = eventId;
	}
}
