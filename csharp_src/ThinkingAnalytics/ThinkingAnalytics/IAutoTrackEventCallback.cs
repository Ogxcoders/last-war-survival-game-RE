using System.Collections.Generic;

namespace ThinkingAnalytics;

public interface IAutoTrackEventCallback
{
	Dictionary<string, object> AutoTrackEventCallback(int type, Dictionary<string, object> properties);
}
