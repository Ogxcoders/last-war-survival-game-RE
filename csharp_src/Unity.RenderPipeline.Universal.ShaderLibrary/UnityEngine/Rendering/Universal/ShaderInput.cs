namespace UnityEngine.Rendering.Universal;

public static class ShaderInput
{
	[GenerateHLSL(PackingRules.Exact, false, false, false, 1, false, false)]
	public struct LightData
	{
		public Vector4 position;

		public Vector4 color;

		public Vector4 attenuation;

		public Vector4 spotDirection;

		public Vector4 occlusionProbeChannels;
	}

	[GenerateHLSL(PackingRules.Exact, false, false, false, 1, false, false)]
	public struct ShadowData
	{
		public Matrix4x4 worldToShadowMatrix;

		public Vector4 shadowParams;
	}
}
