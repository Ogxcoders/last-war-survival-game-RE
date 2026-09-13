using System;

namespace UnityEngine;

public class iPhoneSettings
{
	[Obsolete("verticalOrientation property is deprecated. Please use Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown instead.", false)]
	public static bool verticalOrientation => false;

	[Obsolete("screenCanDarken property is deprecated. Please use (Screen.sleepTimeout != SleepTimeout.NeverSleep) instead.", false)]
	public static bool screenCanDarken => false;

	[Obsolete("locationServiceEnabledByUser property is deprecated. Please use Input.location.isEnabledByUser instead.", true)]
	public static bool locationServiceEnabledByUser => false;

	[Obsolete("StartLocationServiceUpdates method is deprecated. Please use Input.location.Start instead.", true)]
	public static void StartLocationServiceUpdates(float desiredAccuracyInMeters, float updateDistanceInMeters)
	{
	}

	[Obsolete("StartLocationServiceUpdates method is deprecated. Please use Input.location.Start instead.", true)]
	public static void StartLocationServiceUpdates(float desiredAccuracyInMeters)
	{
	}

	[Obsolete("StartLocationServiceUpdates method is deprecated. Please use Input.location.Start instead.", true)]
	public static void StartLocationServiceUpdates()
	{
	}

	[Obsolete("StopLocationServiceUpdates method is deprecated. Please use Input.location.Stop instead.", true)]
	public static void StopLocationServiceUpdates()
	{
	}
}
