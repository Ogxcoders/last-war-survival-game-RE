using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FibMatrix.Rendering;

public class CustomRenderObjectsPass : ScriptableRenderPass
{
	private RenderQueueType renderQueueType;

	private FilteringSettings m_FilteringSettings;

	private RenderObjects.CustomCameraSettings m_CameraSettings;

	private CustomRenderObjectsFeature.RenderTargetSettings m_RenderTargetSettings;

	private string m_ProfilerTag;

	private ProfilingSampler m_ProfilingSampler;

	private List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>();

	private RenderTargetHandle destination;

	private RenderStateBlock m_RenderStateBlock;

	public Material overrideMaterial { get; set; }

	public int overrideMaterialPassIndex { get; set; }

	public void SetDetphState(bool writeEnabled, CompareFunction function = CompareFunction.Less)
	{
		m_RenderStateBlock.mask |= RenderStateMask.Depth;
		m_RenderStateBlock.depthState = new DepthState(writeEnabled, function);
	}

	public void SetStencilState(int reference, CompareFunction compareFunction, StencilOp passOp, StencilOp failOp, StencilOp zFailOp)
	{
		StencilState defaultValue = StencilState.defaultValue;
		defaultValue.enabled = true;
		defaultValue.SetCompareFunction(compareFunction);
		defaultValue.SetPassOperation(passOp);
		defaultValue.SetFailOperation(failOp);
		defaultValue.SetZFailOperation(zFailOp);
		m_RenderStateBlock.mask |= RenderStateMask.Stencil;
		m_RenderStateBlock.stencilReference = reference;
		m_RenderStateBlock.stencilState = defaultValue;
	}

	public CustomRenderObjectsPass(CustomRenderObjectsFeature.RenderObjectsSettings settings)
		: this(settings.passTag, settings)
	{
	}

	public CustomRenderObjectsPass(string tag, CustomRenderObjectsFeature.RenderObjectsSettings settings)
		: this(tag, (RenderPassEvent)((int)settings.Event + (int)settings.eventOffset), settings.filterSettings.PassNames, settings.filterSettings.RenderQueueType, settings.filterSettings.LayerMask, (uint)settings.filterSettings.RenderingLayerMask, settings.cameraSettings, settings.renderTargetSettings, settings)
	{
	}

	public CustomRenderObjectsPass(string profilerTag, RenderPassEvent renderPassEvent, string[] shaderTags, RenderQueueType renderQueueType, int layerMask, uint renderingLayerMask, RenderObjects.CustomCameraSettings cameraSettings, CustomRenderObjectsFeature.RenderTargetSettings renderTargetSettings, CustomRenderObjectsFeature.RenderObjectsSettings settings)
	{
		m_ProfilerTag = profilerTag;
		m_ProfilingSampler = new ProfilingSampler(profilerTag);
		base.renderPassEvent = renderPassEvent;
		this.renderQueueType = renderQueueType;
		overrideMaterial = null;
		overrideMaterialPassIndex = 0;
		m_FilteringSettings = new FilteringSettings((renderQueueType == RenderQueueType.Transparent) ? RenderQueueRange.transparent : RenderQueueRange.opaque, layerMask, renderingLayerMask);
		if (shaderTags != null && shaderTags.Length != 0)
		{
			foreach (string name in shaderTags)
			{
				m_ShaderTagIdList.Add(new ShaderTagId(name));
			}
		}
		else
		{
			m_ShaderTagIdList.Add(new ShaderTagId("UniversalForward"));
			m_ShaderTagIdList.Add(new ShaderTagId("LightweightForward"));
			m_ShaderTagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));
		}
		m_RenderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);
		m_CameraSettings = cameraSettings;
		m_RenderTargetSettings = renderTargetSettings;
		overrideMaterial = settings.overrideMaterial;
		overrideMaterialPassIndex = settings.overrideMaterialPassIndex;
		if (settings.overrideDepthState)
		{
			SetDetphState(settings.enableWrite, settings.depthCompareFunction);
		}
		if (settings.stencilSettings.overrideStencilState)
		{
			SetStencilState(settings.stencilSettings.stencilReference, settings.stencilSettings.stencilCompareFunction, settings.stencilSettings.passOperation, settings.stencilSettings.failOperation, settings.stencilSettings.zFailOperation);
		}
		destination.Init(m_RenderTargetSettings.name);
	}

	public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
	{
		if (cmd == null)
		{
			throw new ArgumentNullException("cmd");
		}
		bool flag = m_RenderTargetSettings.scale * (float)cameraTextureDescriptor.width > 1f && m_RenderTargetSettings.scale * (float)cameraTextureDescriptor.height > 1f;
		if (m_RenderTargetSettings != null && m_RenderTargetSettings.overrideTarget && !string.IsNullOrEmpty(m_RenderTargetSettings.name) && (flag || !(m_RenderTargetSettings.size.SqrMagnitude() < 1f)))
		{
			RenderTextureDescriptor desc = cameraTextureDescriptor;
			desc.colorFormat = m_RenderTargetSettings.format;
			desc.depthBufferBits = m_RenderTargetSettings.depth;
			desc.msaaSamples = ((m_RenderTargetSettings.format == RenderTextureFormat.Depth) ? 1 : desc.msaaSamples);
			desc.width = Math.Max(2, Convert.ToInt32(flag ? ((float)cameraTextureDescriptor.width * m_RenderTargetSettings.scale) : m_RenderTargetSettings.size.x));
			desc.height = Math.Max(2, Convert.ToInt32(flag ? ((float)cameraTextureDescriptor.height * m_RenderTargetSettings.scale) : m_RenderTargetSettings.size.y));
			cmd.GetTemporaryRT(destination.id, desc, m_RenderTargetSettings.filterMode);
			ConfigureTarget(destination.Identifier());
			ConfigureClear(m_RenderTargetSettings.clearFlag, m_RenderTargetSettings.clearColor);
		}
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		SortingCriteria sortingCriteria = ((renderQueueType == RenderQueueType.Transparent) ? SortingCriteria.CommonTransparent : renderingData.cameraData.defaultOpaqueSortFlags);
		DrawingSettings drawingSettings = CreateDrawingSettings(m_ShaderTagIdList, ref renderingData, sortingCriteria);
		drawingSettings.overrideMaterial = overrideMaterial;
		drawingSettings.overrideMaterialPassIndex = overrideMaterialPassIndex;
		ref CameraData cameraData = ref renderingData.cameraData;
		Camera camera = cameraData.camera;
		Rect pixelRect = renderingData.cameraData.pixelRect;
		float aspect = pixelRect.width / pixelRect.height;
		CommandBuffer commandBuffer = CommandBufferPool.Get(m_ProfilerTag);
		using (new ProfilingScope(commandBuffer, m_ProfilingSampler))
		{
			if (m_CameraSettings.overrideCamera && cameraData.isStereoEnabled)
			{
				Debug.LogWarning("RenderObjects pass is configured to override camera matrices. While rendering in stereo camera matrices cannot be overriden.");
			}
			if (m_CameraSettings.overrideCamera && !cameraData.isStereoEnabled)
			{
				Matrix4x4 proj = Matrix4x4.Perspective(m_CameraSettings.cameraFieldOfView, aspect, camera.nearClipPlane, camera.farClipPlane);
				proj = GL.GetGPUProjectionMatrix(proj, cameraData.IsCameraProjectionMatrixFlipped());
				Matrix4x4 viewMatrix = cameraData.GetViewMatrix();
				Vector4 column = viewMatrix.GetColumn(3);
				viewMatrix.SetColumn(3, column + m_CameraSettings.offset);
				RenderingUtils.SetViewAndProjectionMatrices(commandBuffer, viewMatrix, proj, setInverseMatrices: false);
			}
			context.ExecuteCommandBuffer(commandBuffer);
			commandBuffer.Clear();
			context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref m_FilteringSettings, ref m_RenderStateBlock);
			if (m_CameraSettings.overrideCamera && m_CameraSettings.restoreCamera && !cameraData.isStereoEnabled)
			{
				RenderingUtils.SetViewAndProjectionMatrices(commandBuffer, cameraData.GetViewMatrix(), cameraData.GetGPUProjectionMatrix(), setInverseMatrices: false);
			}
		}
		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
	}

	public override void FrameCleanup(CommandBuffer cmd)
	{
		if (cmd == null)
		{
			throw new ArgumentNullException("cmd");
		}
		cmd.ReleaseTemporaryRT(destination.id);
	}
}
