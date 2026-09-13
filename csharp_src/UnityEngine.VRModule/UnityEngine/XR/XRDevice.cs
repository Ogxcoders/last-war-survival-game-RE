using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR;

[NativeConditional("ENABLE_VR")]
public static class XRDevice
{
	[StaticAccessor("GetIVRDevice()", StaticAccessorType.ArrowWithDefaultReturnIfNull)]
	[NativeName("DeviceConnected")]
	public static extern bool isPresent
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[Obsolete("This is obsolete, and should no longer be used.  Please use CommonUsages.userPresence.")]
	public static extern UserPresenceState userPresence
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[StaticAccessor("GetIVRDevice()", StaticAccessorType.ArrowWithDefaultReturnIfNull)]
	[NativeName("DeviceName")]
	[Obsolete("family is deprecated.  Use XRSettings.loadedDeviceName instead.", false)]
	public static extern string family
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[StaticAccessor("GetIVRDevice()", StaticAccessorType.ArrowWithDefaultReturnIfNull)]
	[NativeName("DeviceModel")]
	[Obsolete("This is obsolete, and should no longer be used.  Please use InputDevice.name.")]
	public static extern string model
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[NativeName("DeviceRefreshRate")]
	[StaticAccessor("GetIVRDevice()", StaticAccessorType.ArrowWithDefaultReturnIfNull)]
	public static extern float refreshRate
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public static extern float fovZoomFactor
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("SetProjectionZoomFactor")]
		[StaticAccessor("GetIVRDevice()", StaticAccessorType.ArrowWithDefaultReturnIfNull)]
		set;
	}

	[Obsolete("This is obsolete, and should no longer be used.  Please use XRInputSubsystem.GetTrackingOriginMode.")]
	public static extern TrackingOriginMode trackingOriginMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public static event Action<string> deviceLoaded;

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetIVRDevice()", StaticAccessorType.ArrowWithDefaultReturnIfNull)]
	public static extern IntPtr GetNativePtr();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetIVRDevice()", StaticAccessorType.ArrowWithDefaultReturnIfNull)]
	[Obsolete("This is obsolete, and should no longer be used.  Please use XRInputSubsystem.GetTrackingOriginMode.")]
	public static extern TrackingSpaceType GetTrackingSpaceType();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetIVRDevice()", StaticAccessorType.ArrowWithDefaultReturnIfNull)]
	[Obsolete("This is obsolete, and should no longer be used.  Please use XRInputSubsystem.TrySetTrackingOriginMode.")]
	public static extern bool SetTrackingSpaceType(TrackingSpaceType trackingSpaceType);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetIVRDevice()", StaticAccessorType.ArrowWithDefaultReturnIfNull)]
	[NativeName("DisableAutoVRCameraTracking")]
	public static extern void DisableAutoXRCameraTracking([NotNull] Camera camera, bool disabled);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetIVRDevice()", StaticAccessorType.ArrowWithDefaultReturnIfNull)]
	[NativeName("UpdateEyeTextureMSAASetting")]
	public static extern void UpdateEyeTextureMSAASetting();

	[RequiredByNativeCode]
	private static void InvokeDeviceLoaded(string loadedDeviceName)
	{
		if (XRDevice.deviceLoaded != null)
		{
			XRDevice.deviceLoaded(loadedDeviceName);
		}
	}

	static XRDevice()
	{
		XRDevice.deviceLoaded = null;
	}
}
