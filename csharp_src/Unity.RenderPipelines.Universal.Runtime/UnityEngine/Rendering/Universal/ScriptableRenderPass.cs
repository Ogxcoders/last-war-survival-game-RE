using System;
using System.Collections.Generic;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering.Universal;

[MovedFrom("UnityEngine.Rendering.LWRP")]
public abstract class ScriptableRenderPass
{
	private RenderTargetIdentifier[] m_ColorAttachments = new RenderTargetIdentifier[1] { BuiltinRenderTextureType.CameraTarget };

	private RenderTargetIdentifier m_DepthAttachment = BuiltinRenderTextureType.CameraTarget;

	private ClearFlag m_ClearFlag;

	private Color m_ClearColor = Color.black;

	public RenderPassEvent renderPassEvent { get; set; }

	public RenderTargetIdentifier[] colorAttachments => m_ColorAttachments;

	public RenderTargetIdentifier colorAttachment => m_ColorAttachments[0];

	public RenderTargetIdentifier depthAttachment => m_DepthAttachment;

	public ClearFlag clearFlag => m_ClearFlag;

	public Color clearColor => m_ClearColor;

	internal int eyeIndex { get; set; }

	internal bool overrideCameraTarget { get; set; }

	internal bool isBlitRenderPass { get; set; }

	public ScriptableRenderPass()
	{
		renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
		m_ColorAttachments = new RenderTargetIdentifier[8]
		{
			BuiltinRenderTextureType.CameraTarget,
			0,
			0,
			0,
			0,
			0,
			0,
			0
		};
		m_DepthAttachment = BuiltinRenderTextureType.CameraTarget;
		m_ClearFlag = ClearFlag.None;
		m_ClearColor = Color.black;
		overrideCameraTarget = false;
		isBlitRenderPass = false;
		eyeIndex = 0;
	}

	public void ConfigureTarget(RenderTargetIdentifier colorAttachment, RenderTargetIdentifier depthAttachment)
	{
		m_DepthAttachment = depthAttachment;
		ConfigureTarget(colorAttachment);
	}

	public void ConfigureTarget(RenderTargetIdentifier[] colorAttachments, RenderTargetIdentifier depthAttachment)
	{
		overrideCameraTarget = true;
		uint validColorBufferCount = RenderingUtils.GetValidColorBufferCount(colorAttachments);
		if (validColorBufferCount > SystemInfo.supportedRenderTargetCount)
		{
			Debug.LogError("Trying to set " + validColorBufferCount + " renderTargets, which is more than the maximum supported:" + SystemInfo.supportedRenderTargetCount);
		}
		m_ColorAttachments = colorAttachments;
		m_DepthAttachment = depthAttachment;
	}

	public void ConfigureTarget(RenderTargetIdentifier colorAttachment)
	{
		overrideCameraTarget = true;
		m_ColorAttachments[0] = colorAttachment;
		for (int i = 1; i < m_ColorAttachments.Length; i++)
		{
			m_ColorAttachments[i] = 0;
		}
	}

	public void ConfigureTarget(RenderTargetIdentifier[] colorAttachments)
	{
		ConfigureTarget(colorAttachments, BuiltinRenderTextureType.CameraTarget);
	}

	public void ConfigureClear(ClearFlag clearFlag, Color clearColor)
	{
		m_ClearFlag = clearFlag;
		m_ClearColor = clearColor;
	}

	public virtual void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
	{
	}

	public virtual void FrameCleanup(CommandBuffer cmd)
	{
	}

	internal virtual void OnFinishCameraStackRendering(CommandBuffer cmd)
	{
	}

	public abstract void Execute(ScriptableRenderContext context, ref RenderingData renderingData);

	public void Blit(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material = null, int passIndex = 0)
	{
		ScriptableRenderer.SetRenderTarget(cmd, destination, BuiltinRenderTextureType.CameraTarget, clearFlag, clearColor);
		cmd.Blit(source, destination, material, passIndex);
	}

	[Obsolete("RenderPostProcessing only works with Post-processing v2. The use of the Post-processing Stack V2 is deprecated in the Universal Render Pipeline.")]
	public void RenderPostProcessing(CommandBuffer cmd, ref CameraData cameraData, RenderTextureDescriptor sourceDescriptor, RenderTargetIdentifier source, RenderTargetIdentifier destination, bool opaqueOnly, bool flip)
	{
	}

	public DrawingSettings CreateDrawingSettings(ShaderTagId shaderTagId, ref RenderingData renderingData, SortingCriteria sortingCriteria)
	{
		Camera camera = renderingData.cameraData.camera;
		SortingSettings sortingSettings = new SortingSettings(camera);
		sortingSettings.criteria = sortingCriteria;
		SortingSettings sortingSettings2 = sortingSettings;
		DrawingSettings result = new DrawingSettings(shaderTagId, sortingSettings2);
		result.perObjectData = renderingData.perObjectData;
		result.mainLightIndex = renderingData.lightData.mainLightIndex;
		result.enableDynamicBatching = renderingData.supportsDynamicBatching;
		result.enableInstancing = camera.cameraType != CameraType.Preview;
		return result;
	}

	public DrawingSettings CreateDrawingSettings(List<ShaderTagId> shaderTagIdList, ref RenderingData renderingData, SortingCriteria sortingCriteria)
	{
		if (shaderTagIdList == null || shaderTagIdList.Count == 0)
		{
			Debug.LogWarning("ShaderTagId list is invalid. DrawingSettings is created with default pipeline ShaderTagId");
			return CreateDrawingSettings(new ShaderTagId("UniversalPipeline"), ref renderingData, sortingCriteria);
		}
		DrawingSettings result = CreateDrawingSettings(shaderTagIdList[0], ref renderingData, sortingCriteria);
		for (int i = 1; i < shaderTagIdList.Count; i++)
		{
			result.SetShaderPassName(i, shaderTagIdList[i]);
		}
		return result;
	}

	public static bool operator <(ScriptableRenderPass lhs, ScriptableRenderPass rhs)
	{
		return lhs.renderPassEvent < rhs.renderPassEvent;
	}

	public static bool operator >(ScriptableRenderPass lhs, ScriptableRenderPass rhs)
	{
		return lhs.renderPassEvent > rhs.renderPassEvent;
	}

	internal void SetRenderTarget(CommandBuffer cmd, RenderTargetIdentifier colorAttachment, RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction, ClearFlag clearFlags, Color clearColor, TextureDimension dimension)
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
}
