using System;

namespace UnityEngine.XR;

[Obsolete("This is obsolete, and should no longer be used.  Please use CommonUsages.userPresence.")]
public enum UserPresenceState
{
	Unsupported = -1,
	NotPresent,
	Present,
	Unknown
}
