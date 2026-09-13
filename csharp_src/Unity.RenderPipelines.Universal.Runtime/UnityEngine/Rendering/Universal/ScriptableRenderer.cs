using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering.Universal;

[MovedFrom("UnityEngine.Rendering.LWRP")]
public abstract class ScriptableRenderer : IDisposable
{
	public class RenderingFeatures
	{
		public bool cameraStacking { get; set; }
	}

	private static class RenderPassBlock
	{
		public static readonly int BeforeRendering = 0;

		public static readonly int MainRenderingOpaque = 1;

		public static readonly int MainRenderingTransparent = 2;

		public static readonly int AfterRendering = 3;
	}

	internal static ScriptableRenderer current = null;

	private const int k_RenderPassBlockCount = 4;

	private List<ScriptableRenderPass> m_ActiveRenderPassQueue = new List<ScriptableRenderPass>(32);

	private List<ScriptableRendererFeature> m_RendererFeatures = new List<ScriptableRendererFeature>(10);

	private RenderTargetIdentifier m_CameraColorTarget;

	private RenderTargetIdentifier m_CameraDepthTarget;

	private bool m_FirstTimeCameraColorTargetIsBound = true;

	private bool m_FirstTimeCameraDepthTargetIsBound = true;

	private bool m_XRRenderTargetNeedsClear;

	private const string k_SetCameraRenderStateTag = "Set Camera Data";

	private const string k_SetRenderTarget = "Set RenderTarget";

	private const string k_ReleaseResourcesTag = "Release Resources";

	private static RenderTargetIdentifier[] m_ActiveColorAttachments = new RenderTargetIdentifier[8] { 0, 0, 0, 0, 0, 0, 0, 0 };

	private static RenderTargetIdentifier m_ActiveDepthAttachment;

	private static bool m_InsideStereoRenderBlock;

	private static RenderTargetIdentifier[][] m_TrimmedColorAttachmentCopies = new RenderTargetIdentifier[9][]
	{
		new RenderTargetIdentifier[0],
		new RenderTargetIdentifier[1] { 0 },
		new RenderTargetIdentifier[2] { 0, 0 },
		new RenderTargetIdentifier[3] { 0, 0, 0 },
		new RenderTargetIdentifier[4] { 0, 0, 0, 0 },
		new RenderTargetIdentifier[5] { 0, 0, 0, 0, 0 },
		new RenderTargetIdentifier[6] { 0, 0, 0, 0, 0, 0 },
		new RenderTargetIdentifier[7] { 0, 0, 0, 0, 0, 0, 0 },
		new RenderTargetIdentifier[8] { 0, 0, 0, 0, 0, 0, 0, 0 }
	};

	public RenderTargetIdentifier cameraColorTarget => m_CameraColorTarget;

	public RenderTargetIdentifier cameraDepth => m_CameraDepthTarget;

	public List<ScriptableRendererFeature> rendererFeatures => m_RendererFeatures;

	protected List<ScriptableRenderPass> activeRenderPassQueue => m_ActiveRenderPassQueue;

	public RenderingFeatures supportedRenderingFeatures { get; set; } = new RenderingFeatures();

	public static void SetCameraMatrices(CommandBuffer cmd, ref CameraData cameraData, bool setInverseMatrices)
	{
		if (!cameraData.isStereoEnabled)
		{
			Matrix4x4 viewMatrix = cameraData.GetViewMatrix();
			Matrix4x4 projectionMatrix = cameraData.GetProjectionMatrix();
			cmd.SetViewProjectionMatrices(viewMatrix, projectionMatrix);
			if (setInverseMatrices)
			{
				Matrix4x4 value = Matrix4x4.Scale(new Vector3(1f, 1f, -1f)) * viewMatrix;
				Matrix4x4 inverse = value.inverse;
				cmd.SetGlobalMatrix(ShaderPropertyId.worldToCameraMatrix, value);
				cmd.SetGlobalMatrix(ShaderPropertyId.cameraToWorldMatrix, inverse);
				Matrix4x4 value2 = Matrix4x4.Inverse(cameraData.GetGPUProjectionMatrix() * viewMatrix);
				cmd.SetGlobalMatrix(ShaderPropertyId.inverseViewAndProjectionMatrix, value2);
			}
		}
	}

	private void SetPerCameraShaderVariables(CommandBuffer cmd, ref CameraData cameraData)
	{
		Camera camera = cameraData.camera;
		Rect pixelRect = cameraData.pixelRect;
		float num = pixelRect.width * cameraData.renderScale;
		float num2 = pixelRect.height * cameraData.renderScale;
		float num3 = pixelRect.width;
		float num4 = pixelRect.height;
		float nearClipPlane = camera.nearClipPlane;
		float farClipPlane = camera.farClipPlane;
		float num5 = (Mathf.Approximately(nearClipPlane, 0f) ? 0f : (1f / nearClipPlane));
		float num6 = (Mathf.Approximately(farClipPlane, 0f) ? 0f : (1f / farClipPlane));
		float w = (camera.orthographic ? 1f : 0f);
		float num7 = 1f - farClipPlane * num5;
		float num8 = farClipPlane * num5;
		Vector4 value = new Vector4(num7, num8, num7 * num6, num8 * num6);
		if (SystemInfo.usesReversedZBuffer)
		{
			value.y += value.x;
			value.x = 0f - value.x;
			value.w += value.z;
			value.z = 0f - value.z;
		}
		Vector4 value2 = new Vector4(camera.orthographicSize * cameraData.aspectRatio, camera.orthographicSize, 0f, w);
		cmd.SetGlobalVector(ShaderPropertyId.worldSpaceCameraPos, camera.transform.position);
		cmd.SetGlobalVector(ShaderPropertyId.screenParams, new Vector4(num3, num4, 1f + 1f / num3, 1f + 1f / num4));
		cmd.SetGlobalVector(ShaderPropertyId.scaledScreenParams, new Vector4(num, num2, 1f + 1f / num, 1f + 1f / num2));
		cmd.SetGlobalVector(ShaderPropertyId.zBufferParams, value);
		cmd.SetGlobalVector(ShaderPropertyId.orthoParams, value2);
	}

	private void SetShaderTimeValues(CommandBuffer cmd, float time, float deltaTime, float smoothDeltaTime)
	{
		float f = time / 8f;
		float f2 = time / 4f;
		float f3 = time / 2f;
		Vector4 value = time * new Vector4(0.05f, 1f, 2f, 3f);
		Vector4 value2 = new Vector4(Mathf.Sin(f), Mathf.Sin(f2), Mathf.Sin(f3), Mathf.Sin(time));
		Vector4 value3 = new Vector4(Mathf.Cos(f), Mathf.Cos(f2), Mathf.Cos(f3), Mathf.Cos(time));
		Vector4 value4 = new Vector4(deltaTime, 1f / deltaTime, smoothDeltaTime, 1f / smoothDeltaTime);
		Vector4 value5 = new Vector4(time, Mathf.Sin(time), Mathf.Cos(time), 0f);
		cmd.SetGlobalVector(UniversalRenderPipeline.PerFrameBuffer._Time, value);
		cmd.SetGlobalVector(UniversalRenderPipeline.PerFrameBuffer._SinTime, value2);
		cmd.SetGlobalVector(UniversalRenderPipeline.PerFrameBuffer._CosTime, value3);
		cmd.SetGlobalVector(UniversalRenderPipeline.PerFrameBuffer.unity_DeltaTime, value4);
		cmd.SetGlobalVector(UniversalRenderPipeline.PerFrameBuffer._TimeParameters, value5);
	}

	internal static void ConfigureActiveTarget(RenderTargetIdentifier colorAttachment, RenderTargetIdentifier depthAttachment)
	{
		m_ActiveColorAttachments[0] = colorAttachment;
		for (int i = 1; i < m_ActiveColorAttachments.Length; i++)
		{
			m_ActiveColorAttachments[i] = 0;
		}
		m_ActiveDepthAttachment = depthAttachment;
	}

	public ScriptableRenderer(ScriptableRendererData data)
	{
		foreach (ScriptableRendererFeature rendererFeature in data.rendererFeatures)
		{
			if (!(rendererFeature == null))
			{
				rendererFeature.Create();
				m_RendererFeatures.Add(rendererFeature);
			}
		}
		Clear(CameraRenderType.Base);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public void ConfigureCameraTarget(RenderTargetIdentifier colorTarget, RenderTargetIdentifier depthTarget)
	{
		m_CameraColorTarget = colorTarget;
		m_CameraDepthTarget = depthTarget;
	}

	public abstract void Setup(ScriptableRenderContext context, ref RenderingData renderingData);

	public virtual void SetupLights(ScriptableRenderContext context, ref RenderingData renderingData)
	{
	}

	public virtual void SetupCullingParameters(ref ScriptableCullingParameters cullingParameters, ref CameraData cameraData)
	{
	}

	public virtual void FinishRendering(CommandBuffer cmd)
	{
	}

	public void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		ref CameraData cameraData = ref renderingData.cameraData;
		Camera camera = cameraData.camera;
		bool isStereoEnabled = cameraData.isStereoEnabled;
		CommandBuffer commandBuffer = CommandBufferPool.Get("Set Camera Data");
		float time = Time.time;
		float deltaTime = Time.deltaTime;
		float smoothDeltaTime = Time.smoothDeltaTime;
		ClearRenderingState(commandBuffer);
		SetPerCameraShaderVariables(commandBuffer, ref cameraData);
		SetShaderTimeValues(commandBuffer, time, deltaTime, smoothDeltaTime);
		context.ExecuteCommandBuffer(commandBuffer);
		commandBuffer.Clear();
		SortStable(m_ActiveRenderPassQueue);
		NativeArray<RenderPassEvent> blockEventLimits = new NativeArray<RenderPassEvent>(4, Allocator.Temp);
		blockEventLimits[RenderPassBlock.BeforeRendering] = RenderPassEvent.BeforeRenderingPrepasses;
		blockEventLimits[RenderPassBlock.MainRenderingOpaque] = RenderPassEvent.AfterRenderingOpaques;
		blockEventLimits[RenderPassBlock.MainRenderingTransparent] = RenderPassEvent.AfterRenderingPostProcessing;
		blockEventLimits[RenderPassBlock.AfterRendering] = (RenderPassEvent)2147483647;
		NativeArray<int> blockRanges = new NativeArray<int>(blockEventLimits.Length + 1, Allocator.Temp);
		FillBlockRanges(blockEventLimits, blockRanges);
		blockEventLimits.Dispose();
		SetupLights(context, ref renderingData);
		ExecuteBlock(RenderPassBlock.BeforeRendering, blockRanges, context, ref renderingData);
		for (int i = 0; i < renderingData.cameraData.numberOfXRPasses; i++)
		{
			context.SetupCameraProperties(camera, isStereoEnabled, i);
			SetCameraMatrices(commandBuffer, ref cameraData, setInverseMatrices: true);
			SetShaderTimeValues(commandBuffer, time, deltaTime, smoothDeltaTime);
			context.ExecuteCommandBuffer(commandBuffer);
			commandBuffer.Clear();
			if (isStereoEnabled)
			{
				BeginXRRendering(context, camera, i);
			}
			ExecuteBlock(RenderPassBlock.MainRenderingOpaque, blockRanges, context, ref renderingData, i);
			ExecuteBlock(RenderPassBlock.MainRenderingTransparent, blockRanges, context, ref renderingData, i);
			ExecuteBlock(RenderPassBlock.AfterRendering, blockRanges, context, ref renderingData, i);
			if (isStereoEnabled)
			{
				EndXRRendering(context, in renderingData, i);
			}
		}
		InternalFinishRendering(context, cameraData.resolveFinalTarget);
		blockRanges.Dispose();
		CommandBufferPool.Release(commandBuffer);
	}

	public void EnqueuePass(ScriptableRenderPass pass)
	{
		m_ActiveRenderPassQueue.Add(pass);
	}

	[Obsolete("Use GetCameraClearFlag(ref CameraData cameraData) instead")]
	protected static ClearFlag GetCameraClearFlag(CameraClearFlags cameraClearFlags)
	{
		if (Application.isMobilePlatform)
		{
			return ClearFlag.All;
		}
		if ((cameraClearFlags == CameraClearFlags.Skybox && RenderSettings.skybox != null) || cameraClearFlags == CameraClearFlags.Nothing)
		{
			return ClearFlag.Depth;
		}
		return ClearFlag.All;
	}

	protected static ClearFlag GetCameraClearFlag(ref CameraData cameraData)
	{
		CameraClearFlags clearFlags = cameraData.camera.clearFlags;
		if (cameraData.renderType == CameraRenderType.UI)
		{
			if (clearFlags != CameraClearFlags.Color)
			{
				if (!cameraData.clearDepth)
				{
					return ClearFlag.None;
				}
				return ClearFlag.Depth;
			}
			return ClearFlag.All;
		}
		if (cameraData.renderType == CameraRenderType.Overlay)
		{
			if (!cameraData.clearDepth)
			{
				return ClearFlag.None;
			}
			return ClearFlag.Depth;
		}
		if (Application.isMobilePlatform)
		{
			return ClearFlag.All;
		}
		if ((clearFlags == CameraClearFlags.Skybox && RenderSettings.skybox != null) || clearFlags == CameraClearFlags.Nothing)
		{
			return ClearFlag.Depth;
		}
		return ClearFlag.All;
	}

	private void ClearRenderingState(CommandBuffer cmd)
	{
		cmd.DisableShaderKeyword(ShaderKeywordStrings.MainLightShadows);
		cmd.DisableShaderKeyword(ShaderKeywordStrings.MainLightShadowCascades);
		cmd.DisableShaderKeyword(ShaderKeywordStrings.AdditionalLightsVertex);
		cmd.DisableShaderKeyword(ShaderKeywordStrings.AdditionalLightsPixel);
		cmd.DisableShaderKeyword(ShaderKeywordStrings.AdditionalLightShadows);
		cmd.DisableShaderKeyword(ShaderKeywordStrings.SoftShadows);
		cmd.DisableShaderKeyword(ShaderKeywordStrings.MixedLightingSubtractive);
		cmd.DisableShaderKeyword(ShaderKeywordStrings.LinearToSRGBConversion);
		cmd.DisableShaderKeyword(ShaderKeywordStrings.AllowSoftParticles);
	}

	internal void Clear(CameraRenderType cameraType)
	{
		m_ActiveColorAttachments[0] = BuiltinRenderTextureType.CameraTarget;
		for (int i = 1; i < m_ActiveColorAttachments.Length; i++)
		{
			m_ActiveColorAttachments[i] = 0;
		}
		m_ActiveDepthAttachment = BuiltinRenderTextureType.CameraTarget;
		m_InsideStereoRenderBlock = false;
		m_FirstTimeCameraColorTargetIsBound = cameraType == CameraRenderType.Base || cameraType == CameraRenderType.UI;
		m_FirstTimeCameraDepthTargetIsBound = true;
		m_ActiveRenderPassQueue.Clear();
		m_CameraColorTarget = BuiltinRenderTextureType.CameraTarget;
		m_CameraDepthTarget = BuiltinRenderTextureType.CameraTarget;
	}

	private void ExecuteBlock(int blockIndex, NativeArray<int> blockRanges, ScriptableRenderContext context, ref RenderingData renderingData, int eyeIndex = 0, bool submit = false)
	{
		int num = blockRanges[blockIndex + 1];
		for (int i = blockRanges[blockIndex]; i < num; i++)
		{
			ScriptableRenderPass renderPass = m_ActiveRenderPassQueue[i];
			ExecuteRenderPass(context, renderPass, ref renderingData, eyeIndex);
		}
		if (submit)
		{
			context.Submit();
		}
	}

	private void ExecuteRenderPass(ScriptableRenderContext context, ScriptableRenderPass renderPass, ref RenderingData renderingData, int eyeIndex)
	{
		ref CameraData cameraData = ref renderingData.cameraData;
		Camera camera = cameraData.camera;
		bool firstTimeStereo = false;
		CommandBuffer commandBuffer = CommandBufferPool.Get("Set RenderTarget");
		renderPass.Configure(commandBuffer, cameraData.cameraTargetDescriptor);
		renderPass.eyeIndex = eyeIndex;
		SetRenderPassAttachments(commandBuffer, renderPass, ref cameraData, ref firstTimeStereo);
		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
		if (firstTimeStereo && cameraData.isStereoEnabled)
		{
			context.StartMultiEye(camera, eyeIndex);
			XRUtils.DrawOcclusionMesh(commandBuffer, camera);
		}
		renderPass.Execute(context, ref renderingData);
	}

	private void SetRenderPassAttachments(CommandBuffer cmd, ScriptableRenderPass renderPass, ref CameraData cameraData, ref bool firstTimeStereo)
	{
		Camera camera = cameraData.camera;
		ClearFlag cameraClearFlag = GetCameraClearFlag(ref cameraData);
		if (RenderingUtils.GetValidColorBufferCount(renderPass.colorAttachments) == 0)
		{
			return;
		}
		if (RenderingUtils.IsMRT(renderPass.colorAttachments))
		{
			bool flag = false;
			bool flag2 = false;
			int num = RenderingUtils.IndexOf(renderPass.colorAttachments, m_CameraColorTarget);
			if (num != -1 && (m_FirstTimeCameraColorTargetIsBound || (cameraData.isXRMultipass && m_XRRenderTargetNeedsClear)))
			{
				m_FirstTimeCameraColorTargetIsBound = false;
				firstTimeStereo = true;
				flag = (cameraClearFlag & ClearFlag.Color) != (renderPass.clearFlag & ClearFlag.Color) || CoreUtils.ConvertSRGBToActiveColorSpace(camera.backgroundColor) != renderPass.clearColor;
				if (cameraData.isXRMultipass && m_XRRenderTargetNeedsClear)
				{
					flag2 = (cameraClearFlag & ClearFlag.Depth) != (renderPass.clearFlag & ClearFlag.Depth);
				}
				m_XRRenderTargetNeedsClear = false;
			}
			if (renderPass.depthAttachment == m_CameraDepthTarget && m_FirstTimeCameraDepthTargetIsBound)
			{
				m_FirstTimeCameraDepthTargetIsBound = false;
				flag2 = (cameraClearFlag & ClearFlag.Depth) != (renderPass.clearFlag & ClearFlag.Depth);
			}
			if (flag)
			{
				if ((cameraClearFlag & ClearFlag.Color) != ClearFlag.None)
				{
					SetRenderTarget(cmd, renderPass.colorAttachments[num], renderPass.depthAttachment, ClearFlag.Color, CoreUtils.ConvertSRGBToActiveColorSpace(camera.backgroundColor));
				}
				if ((renderPass.clearFlag & ClearFlag.Color) != ClearFlag.None)
				{
					uint num2 = RenderingUtils.CountDistinct(renderPass.colorAttachments, m_CameraColorTarget);
					RenderTargetIdentifier[] array = m_TrimmedColorAttachmentCopies[num2];
					int num3 = 0;
					for (int i = 0; i < renderPass.colorAttachments.Length; i++)
					{
						if (renderPass.colorAttachments[i] != m_CameraColorTarget && renderPass.colorAttachments[i] != 0)
						{
							array[num3] = renderPass.colorAttachments[i];
							num3++;
						}
					}
					if (num3 != num2)
					{
						Debug.LogError("writeIndex and otherTargetsCount values differed. writeIndex:" + num3 + " otherTargetsCount:" + num2);
					}
					SetRenderTarget(cmd, array, m_CameraDepthTarget, ClearFlag.Color, renderPass.clearColor);
				}
			}
			ClearFlag clearFlag = ClearFlag.None;
			clearFlag |= (flag2 ? (cameraClearFlag & ClearFlag.Depth) : (renderPass.clearFlag & ClearFlag.Depth));
			clearFlag |= ((!flag) ? (renderPass.clearFlag & ClearFlag.Color) : ClearFlag.None);
			if (RenderingUtils.SequenceEqual(renderPass.colorAttachments, m_ActiveColorAttachments) && !(renderPass.depthAttachment != m_ActiveDepthAttachment) && clearFlag == ClearFlag.None)
			{
				return;
			}
			int num4 = RenderingUtils.LastValid(renderPass.colorAttachments);
			if (num4 >= 0)
			{
				int num5 = num4 + 1;
				RenderTargetIdentifier[] array2 = m_TrimmedColorAttachmentCopies[num5];
				for (int j = 0; j < num5; j++)
				{
					array2[j] = renderPass.colorAttachments[j];
				}
				SetRenderTarget(cmd, array2, renderPass.depthAttachment, clearFlag, renderPass.clearColor);
			}
			return;
		}
		RenderTargetIdentifier colorAttachment = renderPass.colorAttachment;
		RenderTargetIdentifier renderTargetIdentifier = renderPass.depthAttachment;
		if (!renderPass.overrideCameraTarget)
		{
			if (renderPass.renderPassEvent < RenderPassEvent.BeforeRenderingOpaques)
			{
				return;
			}
			colorAttachment = m_CameraColorTarget;
			renderTargetIdentifier = m_CameraDepthTarget;
		}
		ClearFlag clearFlag2 = ClearFlag.None;
		Color clearColor;
		if (colorAttachment == m_CameraColorTarget && (m_FirstTimeCameraColorTargetIsBound || (cameraData.isXRMultipass && m_XRRenderTargetNeedsClear)))
		{
			m_FirstTimeCameraColorTargetIsBound = false;
			clearFlag2 |= cameraClearFlag & ClearFlag.Color;
			clearColor = CoreUtils.ConvertSRGBToActiveColorSpace(camera.backgroundColor);
			firstTimeStereo = true;
			if (m_FirstTimeCameraDepthTargetIsBound || (cameraData.isXRMultipass && m_XRRenderTargetNeedsClear))
			{
				m_FirstTimeCameraDepthTargetIsBound = false;
				clearFlag2 |= cameraClearFlag & ClearFlag.Depth;
			}
			m_XRRenderTargetNeedsClear = false;
		}
		else
		{
			clearFlag2 |= renderPass.clearFlag & ClearFlag.Color;
			clearColor = renderPass.clearColor;
		}
		if (m_CameraDepthTarget != BuiltinRenderTextureType.CameraTarget && (renderTargetIdentifier == m_CameraDepthTarget || colorAttachment == m_CameraDepthTarget) && m_FirstTimeCameraDepthTargetIsBound)
		{
			m_FirstTimeCameraDepthTargetIsBound = false;
			clearFlag2 |= cameraClearFlag & ClearFlag.Depth;
		}
		else
		{
			clearFlag2 |= renderPass.clearFlag & ClearFlag.Depth;
		}
		if (colorAttachment != m_ActiveColorAttachments[0] || renderTargetIdentifier != m_ActiveDepthAttachment || clearFlag2 != ClearFlag.None)
		{
			SetRenderTarget(cmd, colorAttachment, renderTargetIdentifier, clearFlag2, clearColor);
		}
	}

	private void BeginXRRendering(ScriptableRenderContext context, Camera camera, int eyeIndex)
	{
		context.StartMultiEye(camera, eyeIndex);
		m_InsideStereoRenderBlock = true;
		m_XRRenderTargetNeedsClear = true;
	}

	private void EndXRRendering(ScriptableRenderContext context, in RenderingData renderingData, int eyeIndex)
	{
		Camera camera = renderingData.cameraData.camera;
		context.StopMultiEye(camera);
		bool isFinalPass = renderingData.cameraData.resolveFinalTarget && eyeIndex == renderingData.cameraData.numberOfXRPasses - 1;
		context.StereoEndRender(camera, eyeIndex, isFinalPass);
		m_InsideStereoRenderBlock = false;
	}

	internal static void SetRenderTarget(CommandBuffer cmd, RenderTargetIdentifier colorAttachment, RenderTargetIdentifier depthAttachment, ClearFlag clearFlag, Color clearColor)
	{
		m_ActiveColorAttachments[0] = colorAttachment;
		for (int i = 1; i < m_ActiveColorAttachments.Length; i++)
		{
			m_ActiveColorAttachments[i] = 0;
		}
		m_ActiveDepthAttachment = depthAttachment;
		RenderBufferLoadAction colorLoadAction = (((clearFlag & ClearFlag.Color) != ClearFlag.None) ? RenderBufferLoadAction.DontCare : RenderBufferLoadAction.Load);
		RenderBufferLoadAction depthLoadAction = (((clearFlag & ClearFlag.Depth) != ClearFlag.None) ? RenderBufferLoadAction.DontCare : RenderBufferLoadAction.Load);
		TextureDimension dimension = (m_InsideStereoRenderBlock ? XRGraphics.eyeTextureDesc.dimension : TextureDimension.Tex2D);
		SetRenderTarget(cmd, colorAttachment, colorLoadAction, RenderBufferStoreAction.Store, depthAttachment, depthLoadAction, RenderBufferStoreAction.Store, clearFlag, clearColor, dimension);
	}

	private static void SetRenderTarget(CommandBuffer cmd, RenderTargetIdentifier colorAttachment, RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction, ClearFlag clearFlags, Color clearColor, TextureDimension dimension)
	{
		if (dimension == TextureDimension.Tex2DArray)
		{
			CoreUtils.SetRenderTarget(cmd, colorAttachment, clearFlags, clearColor);
		}
		else
		{
			CoreUtils.SetRenderTarget(cmd, colorAttachment, colorLoadAction, colorStoreAction, clearFlags, clearColor);
		}
	}

	private static void SetRenderTarget(CommandBuffer cmd, RenderTargetIdentifier colorAttachment, RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction, RenderTargetIdentifier depthAttachment, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction, ClearFlag clearFlags, Color clearColor, TextureDimension dimension)
	{
		if (depthAttachment == BuiltinRenderTextureType.CameraTarget)
		{
			SetRenderTarget(cmd, colorAttachment, colorLoadAction, colorStoreAction, clearFlags, clearColor, dimension);
		}
		else if (dimension == TextureDimension.Tex2DArray)
		{
			CoreUtils.SetRenderTarget(cmd, colorAttachment, depthAttachment, clearFlags, clearColor);
		}
		else
		{
			CoreUtils.SetRenderTarget(cmd, colorAttachment, colorLoadAction, colorStoreAction, depthAttachment, depthLoadAction, depthStoreAction, clearFlags, clearColor);
		}
	}

	private static void SetRenderTarget(CommandBuffer cmd, RenderTargetIdentifier[] colorAttachments, RenderTargetIdentifier depthAttachment, ClearFlag clearFlag, Color clearColor)
	{
		m_ActiveColorAttachments = colorAttachments;
		m_ActiveDepthAttachment = depthAttachment;
		CoreUtils.SetRenderTarget(cmd, colorAttachments, depthAttachment, clearFlag, clearColor);
	}

	[Conditional("UNITY_EDITOR")]
	private void DrawGizmos(ScriptableRenderContext context, Camera camera, GizmoSubset gizmoSubset)
	{
	}

	private void FillBlockRanges(NativeArray<RenderPassEvent> blockEventLimits, NativeArray<int> blockRanges)
	{
		int index = 0;
		int i = 0;
		blockRanges[index++] = 0;
		for (int j = 0; j < blockEventLimits.Length - 1; j++)
		{
			for (; i < m_ActiveRenderPassQueue.Count && m_ActiveRenderPassQueue[i].renderPassEvent < blockEventLimits[j]; i++)
			{
			}
			blockRanges[index++] = i;
		}
		blockRanges[index] = m_ActiveRenderPassQueue.Count;
	}

	private void InternalFinishRendering(ScriptableRenderContext context, bool resolveFinalTarget)
	{
		CommandBuffer commandBuffer = CommandBufferPool.Get("Release Resources");
		for (int i = 0; i < m_ActiveRenderPassQueue.Count; i++)
		{
			m_ActiveRenderPassQueue[i].FrameCleanup(commandBuffer);
		}
		if (resolveFinalTarget)
		{
			for (int j = 0; j < m_ActiveRenderPassQueue.Count; j++)
			{
				m_ActiveRenderPassQueue[j].OnFinishCameraStackRendering(commandBuffer);
			}
			FinishRendering(commandBuffer);
		}
		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
	}

	internal static void SortStable(List<ScriptableRenderPass> list)
	{
		for (int i = 1; i < list.Count; i++)
		{
			ScriptableRenderPass scriptableRenderPass = list[i];
			int num = i - 1;
			while (num >= 0 && scriptableRenderPass < list[num])
			{
				list[num + 1] = list[num];
				num--;
			}
			list[num + 1] = scriptableRenderPass;
		}
	}
}
