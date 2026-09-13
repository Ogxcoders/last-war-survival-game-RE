using Unity.Jobs;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering;

[NativeHeader("Runtime/Camera/BatchRendererGroup.h")]
[UsedByNativeCode]
internal struct BatchRendererCullingOutput
{
	public JobHandle cullingJobsFence;

	public unsafe Plane* cullingPlanes;

	public unsafe BatchVisibility* batchVisibility;

	public unsafe int* visibleIndices;

	public int cullingPlanesCount;

	public int batchVisibilityCount;

	public int visibleIndicesCount;
}
