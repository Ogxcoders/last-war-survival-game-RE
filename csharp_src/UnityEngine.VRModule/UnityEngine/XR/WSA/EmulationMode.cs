using UnityEngine.Bindings;

namespace UnityEngine.XR.WSA;

[NativeHeader("Modules/VR/HoloLens/PerceptionRemoting.h")]
internal enum EmulationMode
{
	None,
	RemoteDevice,
	Simulated
}
