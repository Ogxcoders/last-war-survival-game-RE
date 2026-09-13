using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngineInternal.Input;

[NativeHeader("Modules/Input/Private/InputInternal.h")]
[NativeHeader("Modules/Input/Private/InputModuleBindings.h")]
internal class NativeInputSystem
{
	public static NativeUpdateCallback onUpdate;

	public static Action<NativeInputUpdateType> onBeforeUpdate;

	public static Func<NativeInputUpdateType, bool> onShouldRunUpdate;

	private static Action<int, string> s_OnDeviceDiscoveredCallback;

	public static Action<int, string> onDeviceDiscovered
	{
		get
		{
			return s_OnDeviceDiscoveredCallback;
		}
		set
		{
			s_OnDeviceDiscoveredCallback = value;
			hasDeviceDiscoveredCallback = s_OnDeviceDiscoveredCallback != null;
		}
	}

	internal static extern bool hasDeviceDiscoveredCallback
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeProperty(IsThreadSafe = true)]
	public static extern double currentTime
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[NativeProperty(IsThreadSafe = true)]
	public static extern double currentTimeOffsetToRealtimeSinceStartup
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	static NativeInputSystem()
	{
		hasDeviceDiscoveredCallback = false;
	}

	[RequiredByNativeCode]
	internal static void NotifyBeforeUpdate(NativeInputUpdateType updateType)
	{
		onBeforeUpdate?.Invoke(updateType);
	}

	[RequiredByNativeCode]
	internal unsafe static void NotifyUpdate(NativeInputUpdateType updateType, IntPtr eventBuffer)
	{
		NativeUpdateCallback nativeUpdateCallback = onUpdate;
		NativeInputEventBuffer* ptr = (NativeInputEventBuffer*)eventBuffer.ToPointer();
		if (nativeUpdateCallback == null)
		{
			ptr->eventCount = 0;
			ptr->sizeInBytes = 0;
		}
		else
		{
			nativeUpdateCallback(updateType, ptr);
		}
	}

	[RequiredByNativeCode]
	internal static void NotifyDeviceDiscovered(int deviceId, string deviceDescriptor)
	{
		s_OnDeviceDiscoveredCallback?.Invoke(deviceId, deviceDescriptor);
	}

	[RequiredByNativeCode]
	internal static void ShouldRunUpdate(NativeInputUpdateType updateType, out bool retval)
	{
		retval = onShouldRunUpdate?.Invoke(updateType) ?? true;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("AllocateInputDeviceId")]
	public static extern int AllocateDeviceId();

	public unsafe static void QueueInputEvent<TInputEvent>(ref TInputEvent inputEvent) where TInputEvent : struct
	{
		QueueInputEvent((IntPtr)UnsafeUtility.AddressOf(ref inputEvent));
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	public static extern void QueueInputEvent(IntPtr inputEvent);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern long IOCTL(int deviceId, int code, IntPtr data, int sizeInBytes);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void SetPollingFrequency(float hertz);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void Update(NativeInputUpdateType updateType);

	[Obsolete("This is not needed any longer.")]
	public static void SetUpdateMask(NativeInputUpdateType mask)
	{
	}
}
