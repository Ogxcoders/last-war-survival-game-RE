using UnityEngine;

public static class LoadingDebugTool
{
	public static bool IsShowServerList
	{
		get
		{
			if (CommonUtils.IsDebug())
			{
				return PlayerPrefs.GetInt("RELOAD_DEBUG_SHOW_SERVER_LIST", 0) == 1;
			}
			return false;
		}
		set
		{
			if (CommonUtils.IsDebug())
			{
				PlayerPrefs.SetInt("RELOAD_DEBUG_SHOW_SERVER_LIST", value ? 1 : 0);
				PlayerPrefs.Save();
			}
		}
	}
}
