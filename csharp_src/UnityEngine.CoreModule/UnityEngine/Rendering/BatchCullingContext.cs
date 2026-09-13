using Unity.Collections;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering;

[UsedByNativeCode]
[NativeHeader("Runtime/Camera/BatchRendererGroup.h")]
public struct BatchCullingContext
{
	public readonly NativeArray<Plane> cullingPlanes;

	public NativeArray<BatchVisibility> batchVisibility;

	public NativeArray<int> visibleIndices;

	public readonly LODParameters lodParameters;

	public BatchCullingContext(NativeArray<Plane> inCullingPlanes, NativeArray<BatchVisibility> inOutBatchVisibility, NativeArray<int> outVisibleIndices, LODParameters inLodParameters)
	{
		cullingPlanes = inCullingPlanes;
		batchVisibility = inOutBatchVisibility;
		visibleIndices = outVisibleIndices;
		lodParameters = inLodParameters;
	}
}
