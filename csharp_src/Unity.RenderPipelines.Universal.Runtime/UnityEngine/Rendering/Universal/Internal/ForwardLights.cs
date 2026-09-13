using System;
using Unity.Collections;

namespace UnityEngine.Rendering.Universal.Internal;

public class ForwardLights
{
	private static class LightConstantBuffer
	{
		public static int _MainLightPosition;

		public static int _MainLightColor;

		public static int _AdditionalLightsCount;

		public static int _AdditionalLightsPosition;

		public static int _AdditionalLightsColor;

		public static int _AdditionalLightsAttenuation;

		public static int _AdditionalLightsSpotDir;

		public static int _AdditionalLightOcclusionProbeChannel;
	}

	private int m_AdditionalLightsBufferId;

	private int m_AdditionalLightsIndicesId;

	private const string k_SetupLightConstants = "Setup Light Constants";

	private MixedLightingSetup m_MixedLightingSetup;

	private Vector4 k_DefaultLightPosition = new Vector4(0f, 0f, 1f, 0f);

	private Vector4 k_DefaultLightColor = Color.black;

	private Vector4 k_DefaultLightAttenuation = new Vector4(0f, 1f, 0f, 1f);

	private Vector4 k_DefaultLightSpotDirection = new Vector4(0f, 0f, 1f, 0f);

	private Vector4 k_DefaultLightsProbeChannel = new Vector4(-1f, 1f, -1f, -1f);

	private Vector4[] m_AdditionalLightPositions;

	private Vector4[] m_AdditionalLightColors;

	private Vector4[] m_AdditionalLightAttenuations;

	private Vector4[] m_AdditionalLightSpotDirections;

	private Vector4[] m_AdditionalLightOcclusionProbeChannels;

	private bool m_UseStructuredBuffer;

	public ForwardLights()
	{
		m_UseStructuredBuffer = RenderingUtils.useStructuredBuffer;
		LightConstantBuffer._MainLightPosition = Shader.PropertyToID("_MainLightPosition");
		LightConstantBuffer._MainLightColor = Shader.PropertyToID("_MainLightColor");
		LightConstantBuffer._AdditionalLightsCount = Shader.PropertyToID("_AdditionalLightsCount");
		if (m_UseStructuredBuffer)
		{
			m_AdditionalLightsBufferId = Shader.PropertyToID("_AdditionalLightsBuffer");
			m_AdditionalLightsIndicesId = Shader.PropertyToID("_AdditionalLightsIndices");
			return;
		}
		LightConstantBuffer._AdditionalLightsPosition = Shader.PropertyToID("_AdditionalLightsPosition");
		LightConstantBuffer._AdditionalLightsColor = Shader.PropertyToID("_AdditionalLightsColor");
		LightConstantBuffer._AdditionalLightsAttenuation = Shader.PropertyToID("_AdditionalLightsAttenuation");
		LightConstantBuffer._AdditionalLightsSpotDir = Shader.PropertyToID("_AdditionalLightsSpotDir");
		LightConstantBuffer._AdditionalLightOcclusionProbeChannel = Shader.PropertyToID("_AdditionalLightsOcclusionProbes");
		int maxVisibleAdditionalLights = UniversalRenderPipeline.maxVisibleAdditionalLights;
		m_AdditionalLightPositions = new Vector4[maxVisibleAdditionalLights];
		m_AdditionalLightColors = new Vector4[maxVisibleAdditionalLights];
		m_AdditionalLightAttenuations = new Vector4[maxVisibleAdditionalLights];
		m_AdditionalLightSpotDirections = new Vector4[maxVisibleAdditionalLights];
		m_AdditionalLightOcclusionProbeChannels = new Vector4[maxVisibleAdditionalLights];
	}

	public void Setup(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		int additionalLightsCount = renderingData.lightData.additionalLightsCount;
		bool shadeAdditionalLightsPerVertex = renderingData.lightData.shadeAdditionalLightsPerVertex;
		CommandBuffer commandBuffer = CommandBufferPool.Get("Setup Light Constants");
		SetupShaderLightConstants(commandBuffer, ref renderingData);
		CoreUtils.SetKeyword(commandBuffer, ShaderKeywordStrings.AdditionalLightsVertex, additionalLightsCount > 0 && shadeAdditionalLightsPerVertex);
		CoreUtils.SetKeyword(commandBuffer, ShaderKeywordStrings.AdditionalLightsPixel, additionalLightsCount > 0 && !shadeAdditionalLightsPerVertex);
		CoreUtils.SetKeyword(commandBuffer, ShaderKeywordStrings.MixedLightingSubtractive, renderingData.lightData.supportsMixedLighting && m_MixedLightingSetup == MixedLightingSetup.Subtractive);
		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
	}

	private void InitializeLightConstants(NativeArray<VisibleLight> lights, int lightIndex, out Vector4 lightPos, out Vector4 lightColor, out Vector4 lightAttenuation, out Vector4 lightSpotDir, out Vector4 lightOcclusionProbeChannel)
	{
		lightPos = k_DefaultLightPosition;
		lightColor = k_DefaultLightColor;
		lightAttenuation = k_DefaultLightAttenuation;
		lightSpotDir = k_DefaultLightSpotDirection;
		lightOcclusionProbeChannel = k_DefaultLightsProbeChannel;
		if (lightIndex >= 0)
		{
			VisibleLight visibleLight = lights[lightIndex];
			if (visibleLight.lightType == LightType.Directional)
			{
				Vector4 vector = -visibleLight.localToWorldMatrix.GetColumn(2);
				lightPos = new Vector4(vector.x, vector.y, vector.z, 0f);
			}
			else
			{
				Vector4 column = visibleLight.localToWorldMatrix.GetColumn(3);
				lightPos = new Vector4(column.x, column.y, column.z, 1f);
			}
			lightColor = visibleLight.finalColor;
			if (visibleLight.lightType != LightType.Directional)
			{
				float num = visibleLight.range * visibleLight.range;
				float num2 = 0.64000005f * num - num;
				float num3 = 1f / num2;
				float y = (0f - num) / num2;
				float num4 = 1f / Mathf.Max(0.0001f, visibleLight.range * visibleLight.range);
				lightAttenuation.x = ((Application.isMobilePlatform || SystemInfo.graphicsDeviceType == GraphicsDeviceType.Switch) ? num3 : num4);
				lightAttenuation.y = y;
			}
			if (visibleLight.lightType == LightType.Spot)
			{
				Vector4 column2 = visibleLight.localToWorldMatrix.GetColumn(2);
				lightSpotDir = new Vector4(0f - column2.x, 0f - column2.y, 0f - column2.z, 0f);
				float num5 = Mathf.Cos(MathF.PI / 180f * visibleLight.spotAngle * 0.5f);
				float num6 = ((!(visibleLight.light != null)) ? Mathf.Cos(2f * Mathf.Atan(Mathf.Tan(visibleLight.spotAngle * 0.5f * (MathF.PI / 180f)) * 46f / 64f) * 0.5f) : Mathf.Cos(visibleLight.light.innerSpotAngle * (MathF.PI / 180f) * 0.5f));
				float num7 = Mathf.Max(0.001f, num6 - num5);
				float num8 = 1f / num7;
				float w = (0f - num5) * num8;
				lightAttenuation.z = num8;
				lightAttenuation.w = w;
			}
			Light light = visibleLight.light;
			int num9 = ((light != null) ? light.bakingOutput.occlusionMaskChannel : (-1));
			lightOcclusionProbeChannel.x = ((num9 == -1) ? 0f : ((float)num9));
			lightOcclusionProbeChannel.y = ((num9 == -1) ? 1f : 0f);
			if (light != null && light.bakingOutput.mixedLightingMode == MixedLightingMode.Subtractive && light.bakingOutput.lightmapBakeType == LightmapBakeType.Mixed && m_MixedLightingSetup == MixedLightingSetup.None && visibleLight.light.shadows != LightShadows.None)
			{
				m_MixedLightingSetup = MixedLightingSetup.Subtractive;
			}
		}
	}

	private void SetupShaderLightConstants(CommandBuffer cmd, ref RenderingData renderingData)
	{
		m_MixedLightingSetup = MixedLightingSetup.None;
		SetupMainLightConstants(cmd, ref renderingData.lightData);
		SetupAdditionalLightConstants(cmd, ref renderingData);
	}

	private void SetupMainLightConstants(CommandBuffer cmd, ref LightData lightData)
	{
		InitializeLightConstants(lightData.visibleLights, lightData.mainLightIndex, out var lightPos, out var lightColor, out var _, out var _, out var _);
		cmd.SetGlobalVector(LightConstantBuffer._MainLightPosition, lightPos);
		cmd.SetGlobalVector(LightConstantBuffer._MainLightColor, lightColor);
	}

	private void SetupAdditionalLightConstants(CommandBuffer cmd, ref RenderingData renderingData)
	{
		ref LightData lightData = ref renderingData.lightData;
		CullingResults cullResults = renderingData.cullResults;
		NativeArray<VisibleLight> visibleLights = lightData.visibleLights;
		int maxVisibleAdditionalLights = UniversalRenderPipeline.maxVisibleAdditionalLights;
		int num = SetupPerObjectLightIndices(cullResults, ref lightData);
		if (num > 0)
		{
			if (m_UseStructuredBuffer)
			{
				NativeArray<ShaderInput.LightData> data = new NativeArray<ShaderInput.LightData>(num, Allocator.Temp);
				int i = 0;
				int num2 = 0;
				ShaderInput.LightData value = default(ShaderInput.LightData);
				for (; i < visibleLights.Length; i++)
				{
					if (num2 >= maxVisibleAdditionalLights)
					{
						break;
					}
					_ = visibleLights[i];
					if (lightData.mainLightIndex != i)
					{
						InitializeLightConstants(visibleLights, i, out value.position, out value.color, out value.attenuation, out value.spotDirection, out value.occlusionProbeChannels);
						data[num2] = value;
						num2++;
					}
				}
				ComputeBuffer lightDataBuffer = ShaderData.instance.GetLightDataBuffer(num);
				lightDataBuffer.SetData(data);
				int lightAndReflectionProbeIndexCount = cullResults.lightAndReflectionProbeIndexCount;
				ComputeBuffer lightIndicesBuffer = ShaderData.instance.GetLightIndicesBuffer(lightAndReflectionProbeIndexCount);
				cmd.SetGlobalBuffer(m_AdditionalLightsBufferId, lightDataBuffer);
				cmd.SetGlobalBuffer(m_AdditionalLightsIndicesId, lightIndicesBuffer);
				data.Dispose();
			}
			else
			{
				int j = 0;
				int num3 = 0;
				for (; j < visibleLights.Length; j++)
				{
					if (num3 >= maxVisibleAdditionalLights)
					{
						break;
					}
					_ = visibleLights[j];
					if (lightData.mainLightIndex != j)
					{
						InitializeLightConstants(visibleLights, j, out m_AdditionalLightPositions[num3], out m_AdditionalLightColors[num3], out m_AdditionalLightAttenuations[num3], out m_AdditionalLightSpotDirections[num3], out m_AdditionalLightOcclusionProbeChannels[num3]);
						num3++;
					}
				}
				cmd.SetGlobalVectorArray(LightConstantBuffer._AdditionalLightsPosition, m_AdditionalLightPositions);
				cmd.SetGlobalVectorArray(LightConstantBuffer._AdditionalLightsColor, m_AdditionalLightColors);
				cmd.SetGlobalVectorArray(LightConstantBuffer._AdditionalLightsAttenuation, m_AdditionalLightAttenuations);
				cmd.SetGlobalVectorArray(LightConstantBuffer._AdditionalLightsSpotDir, m_AdditionalLightSpotDirections);
				cmd.SetGlobalVectorArray(LightConstantBuffer._AdditionalLightOcclusionProbeChannel, m_AdditionalLightOcclusionProbeChannels);
			}
			cmd.SetGlobalVector(LightConstantBuffer._AdditionalLightsCount, new Vector4(lightData.maxPerObjectAdditionalLightsCount, 0f, 0f, 0f));
		}
		else
		{
			cmd.SetGlobalVector(LightConstantBuffer._AdditionalLightsCount, Vector4.zero);
		}
	}

	private int SetupPerObjectLightIndices(CullingResults cullResults, ref LightData lightData)
	{
		if (lightData.additionalLightsCount == 0)
		{
			return lightData.additionalLightsCount;
		}
		NativeArray<VisibleLight> visibleLights = lightData.visibleLights;
		NativeArray<int> lightIndexMap = cullResults.GetLightIndexMap(Allocator.Temp);
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < visibleLights.Length; i++)
		{
			if (num2 >= UniversalRenderPipeline.maxVisibleAdditionalLights)
			{
				break;
			}
			_ = visibleLights[i];
			if (i == lightData.mainLightIndex)
			{
				lightIndexMap[i] = -1;
				num++;
			}
			else
			{
				lightIndexMap[i] -= num;
				num2++;
			}
		}
		for (int j = num + num2; j < lightIndexMap.Length; j++)
		{
			lightIndexMap[j] = -1;
		}
		cullResults.SetLightIndexMap(lightIndexMap);
		if (m_UseStructuredBuffer && num2 > 0)
		{
			int lightAndReflectionProbeIndexCount = cullResults.lightAndReflectionProbeIndexCount;
			cullResults.FillLightAndReflectionProbeIndices(ShaderData.instance.GetLightIndicesBuffer(lightAndReflectionProbeIndexCount));
		}
		lightIndexMap.Dispose();
		return num2;
	}
}
