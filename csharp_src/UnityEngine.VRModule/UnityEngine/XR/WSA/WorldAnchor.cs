using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA;

[RequireComponent(typeof(Transform))]
[UsedByNativeCode]
[NativeHeader("Modules/VR/HoloLens/WorldAnchor/WorldAnchor.h")]
[MovedFrom("UnityEngine.VR.WSA")]
public class WorldAnchor : Component
{
	public delegate void OnTrackingChangedDelegate(WorldAnchor worldAnchor, bool located);

	[Obsolete("Support for built-in VR will be removed in Unity 2020.1. Please update to the new Unity XR Plugin System. More information about the new XR Plugin System can be found at https://docs.unity3d.com/2019.3/Documentation/Manual/XR.html.", false)]
	public extern bool isLocated
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public event OnTrackingChangedDelegate OnTrackingChanged;

	private WorldAnchor()
	{
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeConditional("ENABLE_HOLOLENS_MODULE")]
	[NativeName("SetSpatialAnchor_Internal")]
	[Obsolete("Support for built-in VR will be removed in Unity 2020.1. Please update to the new Unity XR Plugin System. More information about the new XR Plugin System can be found at https://docs.unity3d.com/2019.3/Documentation/Manual/XR.html.", false)]
	public extern void SetNativeSpatialAnchorPtr(IntPtr spatialAnchorPtr);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[Obsolete("Support for built-in VR will be removed in Unity 2020.1. Please update to the new Unity XR Plugin System. More information about the new XR Plugin System can be found at https://docs.unity3d.com/2019.3/Documentation/Manual/XR.html.", false)]
	[NativeName("GetSpatialAnchor_Internal")]
	[NativeConditional("ENABLE_HOLOLENS_MODULE")]
	public extern IntPtr GetNativeSpatialAnchorPtr();

	[RequiredByNativeCode]
	private static void Internal_TriggerEventOnTrackingLost(WorldAnchor worldAnchor, bool located)
	{
		if (worldAnchor != null && worldAnchor.OnTrackingChanged != null)
		{
			worldAnchor.OnTrackingChanged(worldAnchor, located);
		}
	}
}
