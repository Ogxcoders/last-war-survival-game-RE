using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Rendering;

[NativeHeader("Runtime/Graphics/DrawSplashScreenAndWatermarks.h")]
public class SplashScreen
{
	public enum StopBehavior
	{
		StopImmediate,
		FadeOut
	}

	public static extern bool isFinished
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction("IsSplashScreenFinished")]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	private static extern void CancelSplashScreen();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	private static extern void BeginSplashScreenFade();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("BeginSplashScreen_Binding")]
	public static extern void Begin();

	public static void Stop(StopBehavior stopBehavior)
	{
		if (stopBehavior == StopBehavior.FadeOut)
		{
			BeginSplashScreenFade();
		}
		else
		{
			CancelSplashScreen();
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("DrawSplashScreen_Binding")]
	public static extern void Draw();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("SetSplashScreenTime")]
	internal static extern void SetTime(float time);
}
