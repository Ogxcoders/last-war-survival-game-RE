using UnityEngine.Bindings;

namespace UnityEngine.XR.WSA;

[NativeHeader("Modules/VR/HoloLens/PerceptionRemoting.h")]
public enum HolographicStreamerConnectionState
{
	Disconnected,
	Connecting,
	Connected
}
