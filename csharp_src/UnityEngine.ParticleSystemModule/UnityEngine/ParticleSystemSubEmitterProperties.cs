using System;

namespace UnityEngine;

[Flags]
public enum ParticleSystemSubEmitterProperties
{
	InheritNothing = 0,
	InheritEverything = 0x1F,
	InheritColor = 1,
	InheritSize = 2,
	InheritRotation = 4,
	InheritLifetime = 8,
	InheritDuration = 0x10
}
