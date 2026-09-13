using System.Collections.Generic;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering.Universal;

[MovedFrom("UnityEngine.Rendering.LWRP")]
public struct ShadowData
{
	public bool supportsMainLightShadows;

	public bool requiresScreenSpaceShadowResolve;

	public int mainLightShadowmapWidth;

	public int mainLightShadowmapHeight;

	public int mainLightShadowCascadesCount;

	public Vector3 mainLightShadowCascadesSplit;

	public bool supportsAdditionalLightShadows;

	public int additionalLightsShadowmapWidth;

	public int additionalLightsShadowmapHeight;

	public bool supportsSoftShadows;

	public int shadowmapDepthBufferBits;

	public List<Vector4> bias;
}
