using UnityEngine;
using XLua;

[LuaCallCSharp(GenFlag.No)]
public static class ClientConfigStatic
{
	public static int PushABTestGroup;

	public static void SetPushABTestGroup(string deviceID, int value)
	{
		PushABTestGroup = value;
		PlayerPrefs.SetInt("PUSH_AB_" + deviceID, PushABTestGroup);
	}

	public static int GetPushABTestGroup(string deviceID)
	{
		return PlayerPrefs.GetInt("PUSH_AB_" + deviceID, -1);
	}
}
