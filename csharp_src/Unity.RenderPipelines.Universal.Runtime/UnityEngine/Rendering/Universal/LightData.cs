using Unity.Collections;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering.Universal;

[MovedFrom("UnityEngine.Rendering.LWRP")]
public struct LightData
{
	public int mainLightIndex;

	public int additionalLightsCount;

	public int maxPerObjectAdditionalLightsCount;

	public NativeArray<VisibleLight> visibleLights;

	public bool shadeAdditionalLightsPerVertex;

	public bool supportsMixedLighting;
}
