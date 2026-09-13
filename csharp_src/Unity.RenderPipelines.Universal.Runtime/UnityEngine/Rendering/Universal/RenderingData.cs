using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering.Universal;

[MovedFrom("UnityEngine.Rendering.LWRP")]
public struct RenderingData
{
	public CullingResults cullResults;

	public CameraData cameraData;

	public LightData lightData;

	public ShadowData shadowData;

	public PostProcessingData postProcessingData;

	public bool supportsDynamicBatching;

	public PerObjectData perObjectData;

	[Obsolete("killAlphaInFinalBlit is deprecated in the Universal Render Pipeline since it is no longer needed on any supported platform.")]
	public bool killAlphaInFinalBlit;

	public bool postProcessingEnabled;
}
