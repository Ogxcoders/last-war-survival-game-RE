using System;
using System.ComponentModel;

namespace UnityEngine.VR;

[Obsolete("UserPresenceState has been moved.  Use UnityEngine.XR.UserPresenceState instead (UnityUpgradable) -> UnityEngine.XR.UserPresenceState", true)]
[EditorBrowsable(EditorBrowsableState.Never)]
public enum UserPresenceState
{
	Unsupported = -1,
	NotPresent,
	Present,
	Unknown
}
