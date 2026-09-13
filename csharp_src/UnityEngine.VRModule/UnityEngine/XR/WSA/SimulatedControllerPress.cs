using UnityEngine.Bindings;

namespace UnityEngine.XR.WSA;

[NativeHeader("Modules/VR/HoloLens/HolographicEmulation/HolographicEmulationManager.h")]
internal enum SimulatedControllerPress
{
	PressButton,
	ReleaseButton,
	Grip,
	TouchPadPress,
	Select,
	TouchPadTouch,
	ThumbStick
}
