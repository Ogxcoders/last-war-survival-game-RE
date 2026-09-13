using System.Collections.Generic;

namespace FibMatrix.BaseUtils;

public class FibTempTestBIApi
{
	public static void SendEvent(string eventName, Dictionary<string, object> eventInfoMap = null)
	{
		FibEngineBIApi.SendGroupEventsImpl(BaseUtilRuntimeCfg.Instance.biConfig.biTestAppId, new List<string> { eventName }, new List<Dictionary<string, object>> { eventInfoMap });
	}

	public static void SendGroupEvents(List<string> eventNames, List<Dictionary<string, object>> eventInfoMapList = null)
	{
		FibEngineBIApi.SendGroupEventsImpl(BaseUtilRuntimeCfg.Instance.biConfig.biTestAppId, eventNames, eventInfoMapList);
	}
}
