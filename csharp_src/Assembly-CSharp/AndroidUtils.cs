using System;
using UnityEngine;

public static class AndroidUtils
{
	public static void Quit()
	{
		GameEntry.Sdk.Android.Call("ExitGame");
	}

	public static int GetKeyboardHeight()
	{
		AndroidJavaObject androidJavaObject = GameEntry.Sdk.Android.Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView", Array.Empty<object>());
		using AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("android.graphics.Rect");
		androidJavaObject.Call("getWindowVisibleDisplayFrame", androidJavaObject2);
		return Screen.height - androidJavaObject2.Call<int>("height", Array.Empty<object>());
	}
}
