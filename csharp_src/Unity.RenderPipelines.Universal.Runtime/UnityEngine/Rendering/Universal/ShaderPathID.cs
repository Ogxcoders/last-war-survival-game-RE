using System;
using System.ComponentModel;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering.Universal;

[MovedFrom("UnityEngine.Rendering.LWRP")]
public enum ShaderPathID
{
	Lit,
	SimpleLit,
	Unlit,
	TerrainLit,
	ParticlesLit,
	ParticlesSimpleLit,
	ParticlesUnlit,
	BakedLit,
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This value is obsolete", false)]
	Count,
	SpeedTree7,
	SpeedTree7Billboard,
	SpeedTree8
}
