using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal;

public class Shadow3SDPass : ScriptableRenderPass
{
	public float shadowScreenRangeScale = 1f;

	public float shadowRTSWidthcale = 1f;

	public float shadowRTHeightScale = 1f;

	public float heightRange = 32f;

	public bool rtFormatUseShadowmap = true;

	public bool rtPrecision16Bit = true;

	public bool pointFilter = true;

	public bool customFilter;

	private FilteringSettings m_FilteringSettings;

	private static List<ShaderTagId> s_ShaderTagIdList = new List<ShaderTagId>
	{
		new ShaderTagId(PassType.ShadowCaster.ToString())
	};

	private static RenderStateBlock s_RenderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);

	private RenderTargetIdentifier m_ColorTarget;

	private ProfilingSampler m_ProfilingSampler;

	private const string k_Shadow3SDMapTexName = "_Shadow3SDMap";

	private const string k_ProfilerTag = "Shadow3SD";

	public Material debugOverrideMaterial { get; set; }

	private RenderTexture m_Destination { get; set; }

	public Shadow3SDPass(int layerMask)
	{
		base.renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
		debugOverrideMaterial = null;
		m_FilteringSettings = new FilteringSettings(RenderQueueRange.opaque, layerMask);
		m_ProfilingSampler = new ProfilingSampler("Shadow3SD");
	}

	public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
	{
		if (m_Destination != null)
		{
			RenderTexture.ReleaseTemporary(m_Destination);
			m_Destination = null;
		}
		int width = (int)((float)cameraTextureDescriptor.width * shadowRTSWidthcale);
		int height = (int)((float)cameraTextureDescriptor.height * shadowRTHeightScale);
		if (rtFormatUseShadowmap)
		{
			m_Destination = ShadowUtils.GetTemporaryShadowTexture(width, height, rtPrecision16Bit ? 16 : 32);
		}
		else
		{
			m_Destination = RenderTexture.GetTemporary(width, height, rtPrecision16Bit ? 16 : 32, rtPrecision16Bit ? RenderTextureFormat.RHalf : RenderTextureFormat.RFloat);
			m_Destination.wrapMode = TextureWrapMode.Clamp;
		}
		m_Destination.filterMode = ((!pointFilter) ? FilterMode.Bilinear : FilterMode.Point);
		m_Destination.name = "_Shadow3SDMap";
		ConfigureTarget(new RenderTargetIdentifier(m_Destination));
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		int mainLightIndex = renderingData.lightData.mainLightIndex;
		if (mainLightIndex == -1)
		{
			return;
		}
		CommandBuffer commandBuffer = CommandBufferPool.Get("Shadow3SD");
		using (new ProfilingScope(commandBuffer, m_ProfilingSampler))
		{
			commandBuffer.ClearRenderTarget(clearDepth: true, clearColor: true, Color.black);
			commandBuffer.SetGlobalTexture("_Shadow3SDMap", m_Destination);
			Camera camera = renderingData.cameraData.camera;
			Vector4 value = -camera.transform.forward;
			value.w = 1f / shadowScreenRangeScale;
			commandBuffer.SetGlobalVector("_CameraWorldDir", value);
			VisibleLight visibleLight = renderingData.lightData.visibleLights[mainLightIndex];
			Light light = visibleLight.light;
			Vector3 vector = -visibleLight.localToWorldMatrix.GetColumn(2);
			commandBuffer.SetGlobalVector("_LightDirection", vector);
			Vector4 zero = Vector4.zero;
			ShadowData shadowData = renderingData.shadowData;
			float num = Mathf.Tan(Mathf.Max(20f, camera.fieldOfView) * (MathF.PI / 180f) * 0.5f) / (float)m_Destination.height * 2f;
			float w = (0f - shadowData.bias[mainLightIndex].y) * num;
			float num2 = (0f - shadowData.bias[mainLightIndex].x) * num;
			zero = (vector - camera.transform.forward) * num2;
			zero.w = w;
			bool flag = light.shadows == LightShadows.Soft && UniversalRenderPipeline.asset.supportsSoftShadows;
			if (flag)
			{
				zero *= 1.5f;
			}
			commandBuffer.SetGlobalVector("_Shadow3SDParam", zero);
			commandBuffer.EnableScissorRect(new Rect(2f, 2f, m_Destination.width - 4, m_Destination.height - 4));
			float y = (flag ? 1f : 0f);
			float z = 0.5f / (float)m_Destination.width;
			float w2 = 0.5f / (float)m_Destination.height;
			Vector4 value2 = new Vector4(light.shadowStrength, y, z, w2);
			commandBuffer.SetGlobalVector("_MainLightShadowParams", value2);
			CoreUtils.SetKeyword(commandBuffer, ShaderKeywordStrings.MainLightShadows3SD, state: true);
			CoreUtils.SetKeyword(commandBuffer, ShaderKeywordStrings.SoftShadows, flag);
			CoreUtils.SetKeyword(commandBuffer, "_SHADOW3SD_USE_COLOR_RT", !rtFormatUseShadowmap);
			Matrix4x4 casterMatrix = SetWorldToShadow3SDCasterMatrix(commandBuffer, renderingData.cameraData, vector, value.w);
			context.ExecuteCommandBuffer(commandBuffer);
			if (!customFilter)
			{
				ShadowDrawingSettings settings = new ShadowDrawingSettings(renderingData.cullResults, mainLightIndex);
				ShadowSplitData splitData = settings.splitData;
				renderingData.cullResults.ComputeDirectionalShadowMatricesAndCullingPrimitives(mainLightIndex, 0, 1, Vector3.right, 1024, 1f, out var _, out var _, out var shadowSplitData);
				splitData.cullingSphere = shadowSplitData.cullingSphere;
				settings.splitData = splitData;
				context.DrawShadows(ref settings);
			}
			else
			{
				SortingCriteria sortingCriteria = SortingCriteria.OptimizeStateChanges;
				DrawingSettings drawingSettings = CreateDrawingSettings(s_ShaderTagIdList, ref renderingData, sortingCriteria);
				drawingSettings.overrideMaterial = debugOverrideMaterial;
				drawingSettings.overrideMaterialPassIndex = 0;
				context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref m_FilteringSettings, ref s_RenderStateBlock);
			}
			commandBuffer.Clear();
			commandBuffer.DisableScissorRect();
			SetWorldToShadow3SDReceiverMatrix(commandBuffer, ref casterMatrix);
		}
		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
	}

	private Matrix4x4 SetWorldToShadow3SDCasterMatrix(CommandBuffer cmd, CameraData cameraData, Vector3 lightDir, float invShadowScreenRangeScale)
	{
		Matrix4x4 viewMatrix = cameraData.GetViewMatrix();
		Matrix4x4 matrix4x = cameraData.GetGPUProjectionMatrix() * viewMatrix;
		lightDir /= lightDir.y;
		Matrix4x4 matrix4x2 = new Matrix4x4(new Vector4(1f, 0f, 0f, 0f), new Vector4(0f - lightDir.x, 0f, 0f - lightDir.z, 0f), new Vector4(0f, 0f, 1f, 0f), new Vector4(0f, 0f, 0f, 1f));
		Matrix4x4 matrix4x3 = matrix4x * matrix4x2;
		matrix4x3 = new Matrix4x4(new Vector4(invShadowScreenRangeScale, 0f, 0f, 0f), new Vector4(0f, invShadowScreenRangeScale, 0f, 0f), new Vector4(0f, 0f, 1f, 0f), new Vector4(0f, 0f, 0f, 1f)) * matrix4x3;
		if (SystemInfo.usesReversedZBuffer)
		{
			matrix4x3.SetRow(2, new Vector4(0f, 0.5f / heightRange, 0f, 0.5f));
		}
		else
		{
			matrix4x3.SetRow(2, new Vector4(0f, -1f / heightRange, 0f, 0f));
		}
		cmd.SetGlobalMatrix("_WorldToShadow3SDCaster", matrix4x3);
		cmd.SetGlobalFloat("_HeightRange", heightRange);
		return matrix4x3;
	}

	private void SetWorldToShadow3SDReceiverMatrix(CommandBuffer cmd, ref Matrix4x4 casterMatrix)
	{
		Matrix4x4 matrix4x = new Matrix4x4(new Vector4(0.5f, 0f, 0f, 0f), new Vector4(0f, 0.5f, 0f, 0f), new Vector4(0f, 0f, 1f, 0f), new Vector4(0.5f, 0.5f, 0f, 1f));
		if (SystemInfo.graphicsUVStartsAtTop)
		{
			matrix4x.SetRow(1, new Vector4(0f, -0.5f, 0f, 0.5f));
		}
		if (!SystemInfo.usesReversedZBuffer)
		{
			matrix4x.SetRow(2, new Vector4(0f, 0f, 0.5f, 0.5f));
		}
		cmd.SetGlobalMatrix("_WorldToShadow3SDReceiver", matrix4x * casterMatrix);
	}

	public override void FrameCleanup(CommandBuffer cmd)
	{
		if (m_Destination != null)
		{
			RenderTexture.ReleaseTemporary(m_Destination);
			m_Destination = null;
		}
		CoreUtils.SetKeyword(cmd, ShaderKeywordStrings.MainLightShadows3SD, state: false);
	}
}
