using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[NativeHeader("Runtime/Input/GetInput.h")]
[NativeHeader("PlatformDependent/AndroidPlayer/Source/AndroidInput.h")]
[NativeHeader("PlatformDependent/AndroidPlayer/Source/TouchInput.h")]
public class AndroidInput
{
	public static int touchCountSecondary => GetTouchCount_Bindings();

	public static bool secondaryTouchEnabled => IsInputDeviceEnabled_Bindings();

	public static int secondaryTouchWidth => GetTouchpadWidth();

	public static int secondaryTouchHeight => GetTouchpadHeight();

	private AndroidInput()
	{
	}

	public static Touch GetSecondaryTouch(int index)
	{
		return GetTouch_Bindings(index);
	}

	[FreeFunction]
	[NativeThrows]
	internal static Touch GetTouch_Bindings(int index)
	{
		GetTouch_Bindings_Injected(index, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	[NativeConditional("PLATFORM_ANDROID")]
	internal static extern int GetTouchCount_Bindings();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	[NativeConditional("PLATFORM_ANDROID")]
	internal static extern bool IsInputDeviceEnabled_Bindings();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeConditional("PLATFORM_ANDROID")]
	[FreeFunction]
	internal static extern int GetTouchpadWidth();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeConditional("PLATFORM_ANDROID")]
	[FreeFunction]
	internal static extern int GetTouchpadHeight();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetTouch_Bindings_Injected(int index, out Touch ret);
}
