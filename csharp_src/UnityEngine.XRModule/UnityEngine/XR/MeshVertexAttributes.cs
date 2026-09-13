using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR;

[Flags]
[UsedByNativeCode]
[NativeHeader("Modules/XR/Subsystems/Meshing/XRMeshBindings.h")]
public enum MeshVertexAttributes
{
	None = 0,
	Normals = 1,
	Tangents = 2,
	UVs = 4,
	Colors = 8
}
