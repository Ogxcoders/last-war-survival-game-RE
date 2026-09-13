using System;

namespace UnityEngine.AI;

[Flags]
public enum NavMeshBuildDebugFlags
{
	None = 0,
	InputGeometry = 1,
	Voxels = 2,
	Regions = 4,
	RawContours = 8,
	SimplifiedContours = 0x10,
	PolygonMeshes = 0x20,
	PolygonMeshesDetail = 0x40,
	All = 0x7F
}
