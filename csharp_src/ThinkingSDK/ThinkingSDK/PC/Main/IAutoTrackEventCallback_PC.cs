using System.Collections.Generic;

namespace ThinkingSDK.PC.Main;

public interface IAutoTrackEventCallback_PC
{
	Dictionary<string, object> AutoTrackEventCallback_PC(int type, Dictionary<string, object> properties);
}
