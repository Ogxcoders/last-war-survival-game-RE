using System;
using System.ComponentModel;

namespace UnityEngine.VR;

[Obsolete("TrackingSpaceType has been moved.  Use UnityEngine.XR.TrackingSpaceType instead (UnityUpgradable) -> UnityEngine.XR.TrackingSpaceType", true)]
[EditorBrowsable(EditorBrowsableState.Never)]
public enum TrackingSpaceType
{
	Stationary,
	RoomScale
}
