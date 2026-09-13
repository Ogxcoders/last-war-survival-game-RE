using UnityEngine;

public static class WindowFullScreen
{
	private static int WindowHeight;

	private static int WindowWidth;

	private static bool isFullScreen;

	public static void SetFullScreen()
	{
		WindowHeight = Screen.height;
		WindowWidth = Screen.width;
		Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.ExclusiveFullScreen);
	}

	public static void ResetFullScreen()
	{
		Resolution currentResolution = Screen.currentResolution;
		if (WindowHeight == 0 || WindowWidth == 0 || WindowHeight >= currentResolution.height || WindowWidth >= currentResolution.width)
		{
			WindowWidth = currentResolution.width / 2;
			WindowHeight = currentResolution.height / 2;
		}
		Screen.SetResolution(WindowWidth, WindowHeight, FullScreenMode.Windowed);
	}

	public static void SwitchFullScreen()
	{
		if (isFullScreen)
		{
			ResetFullScreen();
		}
		else
		{
			SetFullScreen();
		}
		isFullScreen = !isFullScreen;
	}
}
