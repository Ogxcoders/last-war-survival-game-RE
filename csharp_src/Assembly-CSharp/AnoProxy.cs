using System;
using GameFramework;
using UnityEngine;

public static class AnoProxy
{
	public static void SdkInitEx()
	{
		try
		{
			if (StringUtils.VersionCompare(Application.version, "1.0.279") > 0)
			{
				AnoSdk.SdkInitEx(6095058, "96f79e22fe02fdd22ba9111961471c4d");
			}
		}
		catch (Exception arg)
		{
			Log.Error($"AnoSdk.SdkInitEx Exception {arg}");
		}
	}

	public static void AnoSetGamestatus(bool pause)
	{
		try
		{
			if (StringUtils.VersionCompare(Application.version, "1.0.279") > 0)
			{
				if (pause)
				{
					AnoSdk.AnoSetGamestatus(GameStatus.BACKEND);
				}
				else
				{
					AnoSdk.AnoSetGamestatus(GameStatus.FRONTEND);
				}
			}
		}
		catch (Exception arg)
		{
			Log.Error($"AnoSdk.AnoSetGamestatus Exception {arg}");
		}
	}

	public static void AnoUserLogin(string uid)
	{
		try
		{
			if (StringUtils.VersionCompare(Application.version, "1.0.279") > 0)
			{
				AnoSdk.AnoUserLogin(8, uid);
			}
		}
		catch (Exception arg)
		{
			Log.Error($"AnoSdk.AnoUserLogin Exception {arg}");
		}
	}
}
