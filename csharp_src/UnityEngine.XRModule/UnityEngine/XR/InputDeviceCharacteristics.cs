using System;

namespace UnityEngine.XR;

[Flags]
public enum InputDeviceCharacteristics : uint
{
	None = 0u,
	HeadMounted = 1u,
	Camera = 2u,
	HeldInHand = 4u,
	HandTracking = 8u,
	EyeTracking = 0x10u,
	TrackedDevice = 0x20u,
	Controller = 0x40u,
	TrackingReference = 0x80u,
	Left = 0x100u,
	Right = 0x200u,
	Simulated6DOF = 0x400u
}
