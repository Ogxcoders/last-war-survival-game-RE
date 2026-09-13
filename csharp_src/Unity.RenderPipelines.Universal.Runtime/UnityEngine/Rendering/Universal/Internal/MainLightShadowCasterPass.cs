using System;

namespace UnityEngine.Rendering.Universal.Internal;

public class MainLightShadowCasterPass : ScriptableRenderPass
{
	private static class MainLightShadowConstantBuffer
	{
		public static int _WorldToShadow;

		public static int _ShadowParams;

		public static int _CascadeShadowSplitSpheres0;

		public static int _CascadeShadowSplitSpheres1;

		public static int _CascadeShadowSplitSpheres2;

		public static int _CascadeShadowSplitSpheres3;

		public static int _CascadeShadowSplitSphereRadii;

		public static int _ShadowOffset0;

		public static int _ShadowOffset1;

		public static int _ShadowOffset2;

		public static int _ShadowOffset3;

		public static int _ShadowmapSize;
	}

	private const int k_MaxCascades = 4;

	private const int k_ShadowmapBufferBits = 16;

	private int m_ShadowmapWidth;

	private int m_ShadowmapHeight;

	private int m_ShadowCasterCascadesCount;

	private bool m_SupportsBoxFilterForShadows;

	private RenderTargetHandle m_MainLightShadowmap;

	private RenderTexture m_MainLightShadowmapTexture;

	private Matrix4x4[] m_MainLightShadowMatrices;

	private ShadowSliceData[] m_CascadeSlices;

	private Vector4[] m_CascadeSplitDistances;

	private const string m_ProfilerTag = "Render Main Shadowmap";

	private ProfilingSampler m_ProfilingSampler = new ProfilingSampler("Render Main Shadowmap");

	public MainLightShadowCasterPass(RenderPassEvent evt)
	{
		base.renderPassEvent = evt;
		m_MainLightShadowMatrices = new Matrix4x4[5];
		m_CascadeSlices = new ShadowSliceData[4];
		m_CascadeSplitDistances = new Vector4[4];
		MainLightShadowConstantBuffer._WorldToShadow = Shader.PropertyToID("_MainLightWorldToShadow");
		MainLightShadowConstantBuffer._ShadowParams = Shader.PropertyToID("_MainLightShadowParams");
		MainLightShadowConstantBuffer._CascadeShadowSplitSpheres0 = Shader.PropertyToID("_CascadeShadowSplitSpheres0");
		MainLightShadowConstantBuffer._CascadeShadowSplitSpheres1 = Shader.PropertyToID("_CascadeShadowSplitSpheres1");
		MainLightShadowConstantBuffer._CascadeShadowSplitSpheres2 = Shader.PropertyToID("_CascadeShadowSplitSpheres2");
		MainLightShadowConstantBuffer._CascadeShadowSplitSpheres3 = Shader.PropertyToID("_CascadeShadowSplitSpheres3");
		MainLightShadowConstantBuffer._CascadeShadowSplitSphereRadii = Shader.PropertyToID("_CascadeShadowSplitSphereRadii");
		MainLightShadowConstantBuffer._ShadowOffset0 = Shader.PropertyToID("_MainLightShadowOffset0");
		MainLightShadowConstantBuffer._ShadowOffset1 = Shader.PropertyToID("_MainLightShadowOffset1");
		MainLightShadowConstantBuffer._ShadowOffset2 = Shader.PropertyToID("_MainLightShadowOffset2");
		MainLightShadowConstantBuffer._ShadowOffset3 = Shader.PropertyToID("_MainLightShadowOffset3");
		MainLightShadowConstantBuffer._ShadowmapSize = Shader.PropertyToID("_MainLightShadowmapSize");
		m_MainLightShadowmap.Init("_MainLightShadowmapTexture");
		m_SupportsBoxFilterForShadows = Application.isMobilePlatform || SystemInfo.graphicsDeviceType == GraphicsDeviceType.Switch;
	}

	public bool Setup(ref RenderingData renderingData)
	{
		if (!renderingData.shadowData.supportsMainLightShadows)
		{
			return false;
		}
		Clear();
		int mainLightIndex = renderingData.lightData.mainLightIndex;
		if (mainLightIndex == -1)
		{
			return false;
		}
		VisibleLight visibleLight = renderingData.lightData.visibleLights[mainLightIndex];
		Light light = visibleLight.light;
		if (light.shadows == LightShadows.None)
		{
			return false;
		}
		if (visibleLight.lightType != LightType.Directional)
		{
			Debug.LogWarning("Only directional lights are supported as main light.");
		}
		if (!renderingData.cullResults.GetShadowCasterBounds(mainLightIndex, out var _))
		{
			return false;
		}
		m_ShadowCasterCascadesCount = renderingData.shadowData.mainLightShadowCascadesCount;
		int maxTileResolutionInAtlas = ShadowUtils.GetMaxTileResolutionInAtlas(renderingData.shadowData.mainLightShadowmapWidth, renderingData.shadowData.mainLightShadowmapHeight, m_ShadowCasterCascadesCount);
		m_ShadowmapWidth = renderingData.shadowData.mainLightShadowmapWidth;
		m_ShadowmapHeight = ((m_ShadowCasterCascadesCount == 2) ? (renderingData.shadowData.mainLightShadowmapHeight >> 1) : renderingData.shadowData.mainLightShadowmapHeight);
		for (int i = 0; i < m_ShadowCasterCascadesCount; i++)
		{
			if (!ShadowUtils.ExtractDirectionalLightMatrix(ref renderingData.cullResults, ref renderingData.shadowData, mainLightIndex, i, m_ShadowmapWidth, m_ShadowmapHeight, maxTileResolutionInAtlas, light.shadowNearPlane, out m_CascadeSplitDistances[i], out m_CascadeSlices[i], out m_CascadeSlices[i].viewMatrix, out m_CascadeSlices[i].projectionMatrix))
			{
				return false;
			}
		}
		return true;
	}

	public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
	{
		m_MainLightShadowmapTexture = ShadowUtils.GetTemporaryShadowTexture(m_ShadowmapWidth, m_ShadowmapHeight, 16);
		ConfigureTarget(new RenderTargetIdentifier(m_MainLightShadowmapTexture));
		ConfigureClear(ClearFlag.All, Color.black);
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		RenderMainLightCascadeShadowmap(ref context, ref renderingData.cullResults, ref renderingData.lightData, ref renderingData.shadowData);
	}

	public override void FrameCleanup(CommandBuffer cmd)
	{
		if (cmd == null)
		{
			throw new ArgumentNullException("cmd");
		}
		if ((bool)m_MainLightShadowmapTexture)
		{
			RenderTexture.ReleaseTemporary(m_MainLightShadowmapTexture);
			m_MainLightShadowmapTexture = null;
		}
	}

	private void Clear()
	{
		m_MainLightShadowmapTexture = null;
		for (int i = 0; i < m_MainLightShadowMatrices.Length; i++)
		{
			m_MainLightShadowMatrices[i] = Matrix4x4.identity;
		}
		for (int j = 0; j < m_CascadeSplitDistances.Length; j++)
		{
			m_CascadeSplitDistances[j] = new Vector4(0f, 0f, 0f, 0f);
		}
		for (int k = 0; k < m_CascadeSlices.Length; k++)
		{
			m_CascadeSlices[k].Clear();
		}
	}

	private void RenderMainLightCascadeShadowmap(ref ScriptableRenderContext context, ref CullingResults cullResults, ref LightData lightData, ref ShadowData shadowData)
	{
		int mainLightIndex = lightData.mainLightIndex;
		if (mainLightIndex == -1)
		{
			return;
		}
		VisibleLight shadowLight = lightData.visibleLights[mainLightIndex];
		CommandBuffer commandBuffer = CommandBufferPool.Get("Render Main Shadowmap");
		using (new ProfilingScope(commandBuffer, m_ProfilingSampler))
		{
			ShadowDrawingSettings settings = new ShadowDrawingSettings(cullResults, mainLightIndex);
			for (int i = 0; i < m_ShadowCasterCascadesCount; i++)
			{
				ShadowSplitData splitData = settings.splitData;
				splitData.cullingSphere = m_CascadeSplitDistances[i];
				settings.splitData = splitData;
				Vector4 shadowBias = ShadowUtils.GetShadowBias(ref shadowLight, mainLightIndex, ref shadowData, m_CascadeSlices[i].projectionMatrix, m_CascadeSlices[i].resolution);
				ShadowUtils.SetupShadowCasterConstantBuffer(commandBuffer, ref shadowLight, shadowBias);
				ShadowUtils.RenderShadowSlice(commandBuffer, ref context, ref m_CascadeSlices[i], ref settings, m_CascadeSlices[i].projectionMatrix, m_CascadeSlices[i].viewMatrix);
			}
			bool state = shadowLight.light.shadows == LightShadows.Soft && shadowData.supportsSoftShadows;
			CoreUtils.SetKeyword(commandBuffer, ShaderKeywordStrings.MainLightShadows, state: true);
			CoreUtils.SetKeyword(commandBuffer, ShaderKeywordStrings.MainLightShadowCascades, shadowData.mainLightShadowCascadesCount > 1);
			CoreUtils.SetKeyword(commandBuffer, ShaderKeywordStrings.SoftShadows, state);
			SetupMainLightShadowReceiverConstants(commandBuffer, shadowLight, shadowData.supportsSoftShadows);
		}
		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
	}

	private void SetupMainLightShadowReceiverConstants(CommandBuffer cmd, VisibleLight shadowLight, bool supportsSoftShadows)
	{
		Light light = shadowLight.light;
		bool flag = shadowLight.light.shadows == LightShadows.Soft && supportsSoftShadows;
		int shadowCasterCascadesCount = m_ShadowCasterCascadesCount;
		for (int i = 0; i < shadowCasterCascadesCount; i++)
		{
			m_MainLightShadowMatrices[i] = m_CascadeSlices[i].shadowTransform;
		}
		Matrix4x4 zero = Matrix4x4.zero;
		zero.m22 = (SystemInfo.usesReversedZBuffer ? 1f : 0f);
		for (int j = shadowCasterCascadesCount; j <= 4; j++)
		{
			m_MainLightShadowMatrices[j] = zero;
		}
		float num = 1f / (float)m_ShadowmapWidth;
		float num2 = 1f / (float)m_ShadowmapHeight;
		float num3 = 0.5f * num;
		float num4 = 0.5f * num2;
		float y = (flag ? 1f : 0f);
		cmd.SetGlobalTexture(m_MainLightShadowmap.id, m_MainLightShadowmapTexture);
		cmd.SetGlobalMatrixArray(MainLightShadowConstantBuffer._WorldToShadow, m_MainLightShadowMatrices);
		cmd.SetGlobalVector(MainLightShadowConstantBuffer._ShadowParams, new Vector4(light.shadowStrength, y, 0f, 0f));
		if (m_ShadowCasterCascadesCount > 1)
		{
			cmd.SetGlobalVector(MainLightShadowConstantBuffer._CascadeShadowSplitSpheres0, m_CascadeSplitDistances[0]);
			cmd.SetGlobalVector(MainLightShadowConstantBuffer._CascadeShadowSplitSpheres1, m_CascadeSplitDistances[1]);
			cmd.SetGlobalVector(MainLightShadowConstantBuffer._CascadeShadowSplitSpheres2, m_CascadeSplitDistances[2]);
			cmd.SetGlobalVector(MainLightShadowConstantBuffer._CascadeShadowSplitSpheres3, m_CascadeSplitDistances[3]);
			cmd.SetGlobalVector(MainLightShadowConstantBuffer._CascadeShadowSplitSphereRadii, new Vector4(m_CascadeSplitDistances[0].w * m_CascadeSplitDistances[0].w, m_CascadeSplitDistances[1].w * m_CascadeSplitDistances[1].w, m_CascadeSplitDistances[2].w * m_CascadeSplitDistances[2].w, m_CascadeSplitDistances[3].w * m_CascadeSplitDistances[3].w));
		}
		if (supportsSoftShadows)
		{
			if (m_SupportsBoxFilterForShadows)
			{
				cmd.SetGlobalVector(MainLightShadowConstantBuffer._ShadowOffset0, new Vector4(0f - num3, 0f - num4, 0f, 0f));
				cmd.SetGlobalVector(MainLightShadowConstantBuffer._ShadowOffset1, new Vector4(num3, 0f - num4, 0f, 0f));
				cmd.SetGlobalVector(MainLightShadowConstantBuffer._ShadowOffset2, new Vector4(0f - num3, num4, 0f, 0f));
				cmd.SetGlobalVector(MainLightShadowConstantBuffer._ShadowOffset3, new Vector4(num3, num4, 0f, 0f));
			}
			cmd.SetGlobalVector(MainLightShadowConstantBuffer._ShadowmapSize, new Vector4(num, num2, m_ShadowmapWidth, m_ShadowmapHeight));
		}
	}
}
