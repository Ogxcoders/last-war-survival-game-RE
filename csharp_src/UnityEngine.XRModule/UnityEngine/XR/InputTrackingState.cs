using System;

namespace UnityEngine.XR;

[Flags]
public enum InputTrackingState : uint
{
	None = 0u,
	Position = 1u,
	Rotation = 2u,
	Velocity = 4u,
	AngularVelocity = 8u,
	Acceleration = 0x10u,
	AngularAcceleration = 0x20u,
	All = 0x3Fu
}
