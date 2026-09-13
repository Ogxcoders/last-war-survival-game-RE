using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;

namespace UnityEngine;

[NativeHeader("Runtime/Video/MoviePlayback.h")]
[NativeHeader("PlatformDependent/AndroidPlayer/Source/EntryPoint.h")]
[NativeHeader("Runtime/Input/GetInput.h")]
[NativeHeader("Runtime/Export/Handheld/Handheld.bindings.h")]
public class Handheld
{
	[Obsolete("Property Handheld.use32BitDisplayBuffer has been deprecated. Modifying it has no effect, use PlayerSettings instead.")]
	public static bool use32BitDisplayBuffer
	{
		get
		{
			return GetUse32BitDisplayBuffer_Bindings();
		}
		set
		{
		}
	}

	public static bool PlayFullScreenMovie(string path, [DefaultValue("Color.black")] Color bgColor, [DefaultValue("FullScreenMovieControlMode.Full")] FullScreenMovieControlMode controlMode, [DefaultValue("FullScreenMovieScalingMode.AspectFit")] FullScreenMovieScalingMode scalingMode)
	{
		return PlayFullScreenMovie_Bindings(path, bgColor, controlMode, scalingMode);
	}

	[ExcludeFromDocs]
	public static bool PlayFullScreenMovie(string path, Color bgColor, FullScreenMovieControlMode controlMode)
	{
		FullScreenMovieScalingMode scalingMode = FullScreenMovieScalingMode.AspectFit;
		return PlayFullScreenMovie_Bindings(path, bgColor, controlMode, scalingMode);
	}

	[ExcludeFromDocs]
	public static bool PlayFullScreenMovie(string path, Color bgColor)
	{
		FullScreenMovieScalingMode scalingMode = FullScreenMovieScalingMode.AspectFit;
		FullScreenMovieControlMode controlMode = FullScreenMovieControlMode.Full;
		return PlayFullScreenMovie_Bindings(path, bgColor, controlMode, scalingMode);
	}

	[ExcludeFromDocs]
	public static bool PlayFullScreenMovie(string path)
	{
		FullScreenMovieScalingMode scalingMode = FullScreenMovieScalingMode.AspectFit;
		FullScreenMovieControlMode controlMode = FullScreenMovieControlMode.Full;
		Color black = Color.black;
		return PlayFullScreenMovie_Bindings(path, black, controlMode, scalingMode);
	}

	[FreeFunction("PlayFullScreenMovie")]
	private static bool PlayFullScreenMovie_Bindings(string path, Color bgColor, FullScreenMovieControlMode controlMode, FullScreenMovieScalingMode scalingMode)
	{
		return PlayFullScreenMovie_Bindings_Injected(path, ref bgColor, controlMode, scalingMode);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("Vibrate")]
	public static extern void Vibrate();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("GetUse32BitDisplayBuffer_Bindings")]
	private static extern bool GetUse32BitDisplayBuffer_Bindings();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("SetActivityIndicatorStyle_Bindings")]
	private static extern void SetActivityIndicatorStyleImpl_Bindings(int style);

	public static void SetActivityIndicatorStyle(AndroidActivityIndicatorStyle style)
	{
		SetActivityIndicatorStyleImpl_Bindings((int)style);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("GetActivityIndicatorStyle_Bindings")]
	public static extern int GetActivityIndicatorStyle();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("StartActivityIndicator_Bindings")]
	public static extern void StartActivityIndicator();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("StopActivityIndicator_Bindings")]
	public static extern void StopActivityIndicator();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ClearShaderCache_Bindings")]
	public static extern void ClearShaderCache();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool PlayFullScreenMovie_Bindings_Injected(string path, ref Color bgColor, FullScreenMovieControlMode controlMode, FullScreenMovieScalingMode scalingMode);
}
