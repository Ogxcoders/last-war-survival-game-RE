using System;
using System.ComponentModel;
using UnityEngine.XR;

namespace UnityEngine.VR;

[Obsolete("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead (UnityUpgradable) -> UnityEngine.XR.XRDevice", true)]
[EditorBrowsable(EditorBrowsableState.Never)]
public static class VRDevice
{
	public static bool isPresent
	{
		get
		{
			throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
		}
	}

	public static UnityEngine.XR.UserPresenceState userPresence
	{
		get
		{
			throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
		}
	}

	[Obsolete("family is deprecated.  Use XRSettings.loadedDeviceName instead.", true)]
	public static string family
	{
		get
		{
			throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
		}
	}

	public static string model
	{
		get
		{
			throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
		}
	}

	public static float refreshRate
	{
		get
		{
			throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
		}
	}

	public static UnityEngine.XR.TrackingSpaceType GetTrackingSpaceType()
	{
		throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
	}

	public static bool SetTrackingSpaceType(UnityEngine.XR.TrackingSpaceType trackingSpaceType)
	{
		throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
	}

	public static IntPtr GetNativePtr()
	{
		throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
	}

	[Obsolete("DisableAutoVRCameraTracking has been moved and renamed.  Use UnityEngine.XR.XRDevice.DisableAutoXRCameraTracking instead (UnityUpgradable) -> UnityEngine.XR.XRDevice.DisableAutoXRCameraTracking(*)", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static void DisableAutoVRCameraTracking(Camera camera, bool disabled)
	{
		throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
	}
}
