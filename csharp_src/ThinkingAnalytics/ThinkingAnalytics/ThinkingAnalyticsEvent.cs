using System;
using System.Collections.Generic;

namespace ThinkingAnalytics;

public class ThinkingAnalyticsEvent
{
	public enum Type
	{
		FIRST,
		UPDATABLE,
		OVERWRITABLE
	}

	public Type? EventType { get; set; }

	public string EventName { get; }

	public Dictionary<string, object> Properties { get; }

	public DateTime EventTime { get; set; }

	public TimeZoneInfo EventTimeZone { get; set; }

	public string ExtraId { get; set; }

	public ThinkingAnalyticsEvent(string eventName, Dictionary<string, object> properties)
	{
		EventName = eventName;
		Properties = properties;
	}
}
