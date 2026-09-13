using ThinkingSDK.PC.Config;
using ThinkingSDK.PC.Constant;
using UnityEngine;

namespace ThinkingSDK.PC.Utils;

public class ThinkingSDKAppInfo
{
	public static string LibVersion()
	{
		if (ThinkingSDKUtil.DisPresetProperties.Contains(ThinkingSDKConstant.LIB_VERSION))
		{
			return "";
		}
		return ThinkingSDKPublicConfig.Version();
	}

	public static string LibName()
	{
		if (ThinkingSDKUtil.DisPresetProperties.Contains(ThinkingSDKConstant.LIB))
		{
			return "";
		}
		return ThinkingSDKPublicConfig.Name();
	}

	public static string AppVersion()
	{
		if (ThinkingSDKUtil.DisPresetProperties.Contains(ThinkingSDKConstant.APP_VERSION))
		{
			return "";
		}
		return Application.version;
	}

	public static string AppIdentifier()
	{
		if (ThinkingSDKUtil.DisPresetProperties.Contains(ThinkingSDKConstant.APP_BUNDLEID))
		{
			return "";
		}
		return Application.identifier;
	}
}
