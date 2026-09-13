using ThinkingSDK.PC.Config;

namespace ThinkingSDK.PC.Utils;

public class ThinkingSDKLogger
{
	public static void Print(string str)
	{
		ThinkingSDKPublicConfig.IsPrintLog();
	}
}
