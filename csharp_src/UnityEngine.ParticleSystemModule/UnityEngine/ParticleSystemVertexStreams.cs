using System;

namespace UnityEngine;

[Obsolete("ParticleSystemVertexStreams is deprecated. Please use ParticleSystemVertexStream instead.", false)]
[Flags]
public enum ParticleSystemVertexStreams
{
	Position = 1,
	Normal = 2,
	Tangent = 4,
	Color = 8,
	UV = 0x10,
	UV2BlendAndFrame = 0x20,
	CenterAndVertexID = 0x40,
	Size = 0x80,
	Rotation = 0x100,
	Velocity = 0x200,
	Lifetime = 0x400,
	Custom1 = 0x800,
	Custom2 = 0x1000,
	Random = 0x2000,
	None = 0,
	All = int.MaxValue
}
