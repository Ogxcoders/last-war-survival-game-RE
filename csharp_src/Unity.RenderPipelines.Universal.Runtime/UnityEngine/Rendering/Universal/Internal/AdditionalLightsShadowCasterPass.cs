using System;
using System.Collections.Generic;
using Unity.Collections;

namespace UnityEngine.Rendering.Universal.Internal;

public class AdditionalLightsShadowCasterPass : ScriptableRenderPass
{
	private static class AdditionalShadowsConstantBuffer
	{
		public static int _AdditionalLightsWorldToShadow;

		public static int _AdditionalShadowParams;

		public static int _AdditionalShadowOffset0;

		public static int _AdditionalShadowOffset1;

		public static int _AdditionalShadowOffset2;

		public static int _AdditionalShadowOffset3;

		public static int _AdditionalShadowmapSize;
	}

	public static int m_AdditionalShadowsBufferId;

	public static int m_AdditionalShadowsIndicesId;

	private bool m_UseStructuredBuffer;

	private const int k_ShadowmapBufferBits = 16;

	private RenderTargetHandle m_AdditionalLightsShadowmap;

	private RenderTexture m_AdditionalLightsShadowmapTexture;

	private int m_ShadowmapWidth;

	private int m_ShadowmapHeight;

	private ShadowSliceData[] m_AdditionalLightSlices;

	private Matrix4x4[] m_AdditionalLightsWorldToShadow;

	private Vector4[] m_AdditionalLightsShadowParams;

	private ShaderInput.ShadowData[] m_AdditionalLightsShadowData;

	private List<int> m_AdditionalShadowCastingLightIndices = new List<int>();

	private List<int> m_AdditionalShadowCastingLightIndicesMap = new List<int>();

	private bool m_SupportsBoxFilterForShadows;

	private const string m_ProfilerTag = "Render Additional Shadows";

	private ProfilingSampler m_ProfilingSampler = new ProfilingSampler("Render Additional Shadows");

	public AdditionalLightsShadowCasterPass(RenderPassEvent evt)
	{
		base.renderPassEvent = evt;
		AdditionalShadowsConstantBuffer._AdditionalLightsWorldToShadow = Shader.PropertyToID("_AdditionalLightsWorldToShadow");
		AdditionalShadowsConstantBuffer._AdditionalShadowParams = Shader.PropertyToID("_AdditionalShadowParams");
		AdditionalShadowsConstantBuffer._AdditionalShadowOffset0 = Shader.PropertyToID("_AdditionalShadowOffset0");
		AdditionalShadowsConstantBuffer._AdditionalShadowOffset1 = Shader.PropertyToID("_AdditionalShadowOffset1");
		AdditionalShadowsConstantBuffer._AdditionalShadowOffset2 = Shader.PropertyToID("_AdditionalShadowOffset2");
		AdditionalShadowsConstantBuffer._AdditionalShadowOffset3 = Shader.PropertyToID("_AdditionalShadowOffset3");
		AdditionalShadowsConstantBuffer._AdditionalShadowmapSize = Shader.PropertyToID("_AdditionalShadowmapSize");
		m_AdditionalLightsShadowmap.Init("_AdditionalLightsShadowmapTexture");
		m_AdditionalShadowsBufferId = Shader.PropertyToID("_AdditionalShadowsBuffer");
		m_AdditionalShadowsIndicesId = Shader.PropertyToID("_AdditionalShadowsIndices");
		m_UseStructuredBuffer = RenderingUtils.useStructuredBuffer;
		m_SupportsBoxFilterForShadows = Application.isMobilePlatform || SystemInfo.graphicsDeviceType == GraphicsDeviceType.Switch;
		if (!m_UseStructuredBuffer)
		{
			int maxVisibleAdditionalLights = UniversalRenderPipeline.maxVisibleAdditionalLights;
			m_AdditionalLightsWorldToShadow = new Matrix4x4[maxVisibleAdditionalLights];
			m_AdditionalLightsShadowParams = new Vector4[maxVisibleAdditionalLights];
		}
	}

	public bool Setup(ref RenderingData renderingData)
	{
		Clear();
		m_ShadowmapWidth = renderingData.shadowData.additionalLightsShadowmapWidth;
		m_ShadowmapHeight = renderingData.shadowData.additionalLightsShadowmapHeight;
		NativeArray<VisibleLight> visibleLights = renderingData.lightData.visibleLights;
		int additionalLightsCount = renderingData.lightData.additionalLightsCount;
		if (m_AdditionalLightSlices == null || m_AdditionalLightSlices.Length < additionalLightsCount)
		{
			m_AdditionalLightSlices = new ShadowSliceData[additionalLightsCount];
		}
		if (m_AdditionalLightsShadowData == null || m_AdditionalLightsShadowData.Length < additionalLightsCount)
		{
			m_AdditionalLightsShadowData = new ShaderInput.ShadowData[additionalLightsCount];
		}
		int num = 0;
		bool supportsSoftShadows = renderingData.shadowData.supportsSoftShadows;
		for (int i = 0; i < visibleLights.Length && m_AdditionalShadowCastingLightIndices.Count < additionalLightsCount; i++)
		{
			VisibleLight visibleLight = visibleLights[i];
			if (i == renderingData.lightData.mainLightIndex)
			{
				continue;
			}
			int count = m_AdditionalShadowCastingLightIndices.Count;
			bool flag = false;
			if (renderingData.cullResults.GetShadowCasterBounds(i, out var _))
			{
				if (!renderingData.shadowData.supportsAdditionalLightShadows)
				{
					continue;
				}
				if (IsValidShadowCastingLight(ref renderingData.lightData, i) && ShadowUtils.ExtractSpotLightMatrix(ref renderingData.cullResults, ref renderingData.shadowData, i, out var shadowMatrix, out m_AdditionalLightSlices[count].viewMatrix, out m_AdditionalLightSlices[count].projectionMatrix))
				{
					m_AdditionalShadowCastingLightIndices.Add(i);
					Light light = visibleLight.light;
					float shadowStrength = light.shadowStrength;
					float y = ((supportsSoftShadows && light.shadows == LightShadows.Soft) ? 1f : 0f);
					Vector4 vector = new Vector4(shadowStrength, y, 0f, 0f);
					if (m_UseStructuredBuffer)
					{
						m_AdditionalLightsShadowData[count].worldToShadowMatrix = shadowMatrix;
						m_AdditionalLightsShadowData[count].shadowParams = vector;
					}
					else
					{
						m_AdditionalLightsWorldToShadow[count] = shadowMatrix;
						m_AdditionalLightsShadowParams[count] = vector;
					}
					flag = true;
					num++;
				}
			}
			if (m_UseStructuredBuffer)
			{
				int item = (flag ? count : (-1));
				m_AdditionalShadowCastingLightIndicesMap.Add(item);
			}
			else if (!flag)
			{
				Matrix4x4 identity = Matrix4x4.identity;
				m_AdditionalShadowCastingLightIndices.Add(i);
				m_AdditionalLightsWorldToShadow[count] = identity;
				m_AdditionalLightsShadowParams[count] = Vector4.zero;
				m_AdditionalLightSlices[count].viewMatrix = identity;
				m_AdditionalLightSlices[count].projectionMatrix = identity;
			}
		}
		if (num == 0)
		{
			return false;
		}
		int additionalLightsShadowmapWidth = renderingData.shadowData.additionalLightsShadowmapWidth;
		int additionalLightsShadowmapHeight = renderingData.shadowData.additionalLightsShadowmapHeight;
		int maxTileResolutionInAtlas = ShadowUtils.GetMaxTileResolutionInAtlas(additionalLightsShadowmapWidth, additionalLightsShadowmapHeight, num);
		int num2 = m_ShadowmapWidth / maxTileResolutionInAtlas * (m_ShadowmapHeight / maxTileResolutionInAtlas);
		if (num <= num2 / 2)
		{
			m_ShadowmapHeight /= 2;
		}
		int num3 = additionalLightsShadowmapWidth / maxTileResolutionInAtlas;
		float num4 = 1f / (float)m_ShadowmapWidth;
		float num5 = 1f / (float)m_ShadowmapHeight;
		int num6 = 0;
		int count2 = m_AdditionalShadowCastingLightIndices.Count;
		Matrix4x4 identity2 = Matrix4x4.identity;
		identity2.m00 = (float)maxTileResolutionInAtlas * num4;
		identity2.m11 = (float)maxTileResolutionInAtlas * num5;
		for (int j = 0; j < count2; j++)
		{
			if (m_UseStructuredBuffer || !Mathf.Approximately(m_AdditionalLightsShadowParams[j].x, 0f))
			{
				m_AdditionalLightSlices[j].offsetX = num6 % num3 * maxTileResolutionInAtlas;
				m_AdditionalLightSlices[j].offsetY = num6 / num3 * maxTileResolutionInAtlas;
				m_AdditionalLightSlices[j].resolution = maxTileResolutionInAtlas;
				identity2.m03 = (float)m_AdditionalLightSlices[j].offsetX * num4;
				identity2.m13 = (float)m_AdditionalLightSlices[j].offsetY * num5;
				if (m_UseStructuredBuffer)
				{
					m_AdditionalLightsShadowData[j].worldToShadowMatrix = identity2 * m_AdditionalLightsShadowData[j].worldToShadowMatrix;
				}
				else
				{
					m_AdditionalLightsWorldToShadow[j] = identity2 * m_AdditionalLightsWorldToShadow[j];
				}
				num6++;
			}
		}
		return true;
	}

	public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
	{
		m_AdditionalLightsShadowmapTexture = ShadowUtils.GetTemporaryShadowTexture(m_ShadowmapWidth, m_ShadowmapHeight, 16);
		ConfigureTarget(new RenderTargetIdentifier(m_AdditionalLightsShadowmapTexture));
		ConfigureClear(ClearFlag.All, Color.black);
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		if (renderingData.shadowData.supportsAdditionalLightShadows)
		{
			RenderAdditionalShadowmapAtlas(ref context, ref renderingData.cullResults, ref renderingData.lightData, ref renderingData.shadowData);
		}
	}

	public override void FrameCleanup(CommandBuffer cmd)
	{
		if (cmd == null)
		{
			throw new ArgumentNullException("cmd");
		}
		if ((bool)m_AdditionalLightsShadowmapTexture)
		{
			RenderTexture.ReleaseTemporary(m_AdditionalLightsShadowmapTexture);
			m_AdditionalLightsShadowmapTexture = null;
		}
	}

	private void Clear()
	{
		m_AdditionalShadowCastingLightIndices.Clear();
		m_AdditionalShadowCastingLightIndicesMap.Clear();
		m_AdditionalLightsShadowmapTexture = null;
	}

	private void RenderAdditionalShadowmapAtlas(ref ScriptableRenderContext context, ref CullingResults cullResults, ref LightData lightData, ref ShadowData shadowData)
	{
		NativeArray<VisibleLight> visibleLights = lightData.visibleLights;
		bool flag = false;
		CommandBuffer commandBuffer = CommandBufferPool.Get("Render Additional Shadows");
		using (new ProfilingScope(commandBuffer, m_ProfilingSampler))
		{
			bool flag2 = false;
			int count = m_AdditionalShadowCastingLightIndices.Count;
			for (int i = 0; i < count; i++)
			{
				if (m_UseStructuredBuffer || !Mathf.Approximately(m_AdditionalLightsShadowParams[i].x, 0f))
				{
					int num = m_AdditionalShadowCastingLightIndices[i];
					VisibleLight shadowLight = visibleLights[num];
					ShadowSliceData shadowSliceData = m_AdditionalLightSlices[i];
					ShadowDrawingSettings settings = new ShadowDrawingSettings(cullResults, num);
					Vector4 shadowBias = ShadowUtils.GetShadowBias(ref shadowLight, num, ref shadowData, shadowSliceData.projectionMatrix, shadowSliceData.resolution);
					ShadowUtils.SetupShadowCasterConstantBuffer(commandBuffer, ref shadowLight, shadowBias);
					ShadowUtils.RenderShadowSlice(commandBuffer, ref context, ref shadowSliceData, ref settings);
					flag |= shadowLight.light.shadows == LightShadows.Soft;
					flag2 = true;
				}
			}
			bool flag3 = shadowData.supportsMainLightShadows && lightData.mainLightIndex != -1 && visibleLights[lightData.mainLightIndex].light.shadows == LightShadows.Soft;
			bool flag4 = shadowData.supportsSoftShadows && (flag3 || flag);
			CoreUtils.SetKeyword(commandBuffer, ShaderKeywordStrings.AdditionalLightShadows, flag2);
			CoreUtils.SetKeyword(commandBuffer, ShaderKeywordStrings.SoftShadows, flag4);
			if (flag2)
			{
				SetupAdditionalLightsShadowReceiverConstants(commandBuffer, ref shadowData, flag4);
			}
		}
		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
	}

	private void SetupAdditionalLightsShadowReceiverConstants(CommandBuffer cmd, ref ShadowData shadowData, bool softShadows)
	{
		int count = m_AdditionalShadowCastingLightIndices.Count;
		float num = 1f / (float)shadowData.additionalLightsShadowmapWidth;
		float num2 = 1f / (float)shadowData.additionalLightsShadowmapHeight;
		float num3 = 0.5f * num;
		float num4 = 0.5f * num2;
		cmd.SetGlobalTexture(m_AdditionalLightsShadowmap.id, m_AdditionalLightsShadowmapTexture);
		if (m_UseStructuredBuffer)
		{
			NativeArray<ShaderInput.ShadowData> data = new NativeArray<ShaderInput.ShadowData>(count, Allocator.Temp);
			ShaderInput.ShadowData value = default(ShaderInput.ShadowData);
			for (int i = 0; i < count; i++)
			{
				value.worldToShadowMatrix = m_AdditionalLightsShadowData[i].worldToShadowMatrix;
				value.shadowParams = m_AdditionalLightsShadowData[i].shadowParams;
				data[i] = value;
			}
			ComputeBuffer shadowDataBuffer = ShaderData.instance.GetShadowDataBuffer(count);
			shadowDataBuffer.SetData(data);
			ComputeBuffer shadowIndicesBuffer = ShaderData.instance.GetShadowIndicesBuffer(m_AdditionalShadowCastingLightIndicesMap.Count);
			shadowIndicesBuffer.SetData(m_AdditionalShadowCastingLightIndicesMap, 0, 0, m_AdditionalShadowCastingLightIndicesMap.Count);
			cmd.SetGlobalBuffer(m_AdditionalShadowsBufferId, shadowDataBuffer);
			cmd.SetGlobalBuffer(m_AdditionalShadowsIndicesId, shadowIndicesBuffer);
			data.Dispose();
		}
		else
		{
			cmd.SetGlobalMatrixArray(AdditionalShadowsConstantBuffer._AdditionalLightsWorldToShadow, m_AdditionalLightsWorldToShadow);
			cmd.SetGlobalVectorArray(AdditionalShadowsConstantBuffer._AdditionalShadowParams, m_AdditionalLightsShadowParams);
		}
		if (softShadows)
		{
			if (m_SupportsBoxFilterForShadows)
			{
				cmd.SetGlobalVector(AdditionalShadowsConstantBuffer._AdditionalShadowOffset0, new Vector4(0f - num3, 0f - num4, 0f, 0f));
				cmd.SetGlobalVector(AdditionalShadowsConstantBuffer._AdditionalShadowOffset1, new Vector4(num3, 0f - num4, 0f, 0f));
				cmd.SetGlobalVector(AdditionalShadowsConstantBuffer._AdditionalShadowOffset2, new Vector4(0f - num3, num4, 0f, 0f));
				cmd.SetGlobalVector(AdditionalShadowsConstantBuffer._AdditionalShadowOffset3, new Vector4(num3, num4, 0f, 0f));
			}
			cmd.SetGlobalVector(AdditionalShadowsConstantBuffer._AdditionalShadowmapSize, new Vector4(num, num2, shadowData.additionalLightsShadowmapWidth, shadowData.additionalLightsShadowmapHeight));
		}
	}

	private bool IsValidShadowCastingLight(ref LightData lightData, int i)
	{
		if (i == lightData.mainLightIndex)
		{
			return false;
		}
		VisibleLight visibleLight = lightData.visibleLights[i];
		if (visibleLight.lightType == LightType.Point || visibleLight.lightType == LightType.Directional)
		{
			return false;
		}
		Light light = visibleLight.light;
		if (light != null && light.shadows != LightShadows.None)
		{
			return !Mathf.Approximately(light.shadowStrength, 0f);
		}
		return false;
	}
}
