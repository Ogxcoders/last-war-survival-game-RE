using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FibMatrix.Rendering;

public class WaterColorPass : RenderObjectsPass
{
	private float scale;

	public bool smallColorRTBlit = true;

	public bool enableSceneDepth = true;

	public bool readDepthInsteadOfCopy = true;

	public bool isHdrEnabled = true;

	public const string SceneDepthOff = "_DEPTH_TEXTURE_OFF";

	public const string ReadDepthInsteadOfCopyKW = "_READ_DEPTH_INSTEAD_OF_COPY";

	public const string ResolveDepthForSmallRTKW = "_RESOLVE_DEPTH_FOR_SMALL_RT";

	public const string SamplerWaterColorTextureKW = "_SAMPLE_WATER_COLOR_TEXTURE";

	public const string SmallWaterRT = "_SMALL_WATER_RT";

	private const string m_ProfilerTag = "Water Render Feature Pass";

	private const string m_ProfilerTagClear = "Water Update Next Stage Keywords";

	private bool m_ARGBHalfSupport;

	private string waterColorTextureName { get; set; }

	private RenderTexture destination { get; set; }

	public WaterColorPass(string profilerTag, RenderPassEvent renderPassEvent, string[] shaderTags, RenderQueueType renderQueueType, int layerMask, RenderObjects.CustomCameraSettings cameraSettings, string waterColorTextureName)
		: base(profilerTag, renderPassEvent, shaderTags, renderQueueType, layerMask, cameraSettings)
	{
		this.waterColorTextureName = waterColorTextureName;
		m_ARGBHalfSupport = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);
	}

	public void Setup(float scale, bool enableSceneDepth, bool readDepthInsteadOfCopy, bool isHdrEnabled)
	{
		this.scale = scale;
		this.enableSceneDepth = enableSceneDepth;
		this.readDepthInsteadOfCopy = readDepthInsteadOfCopy;
		this.isHdrEnabled = isHdrEnabled;
	}

	public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
	{
		if (smallColorRTBlit)
		{
			RenderTextureFormat format = ((isHdrEnabled && m_ARGBHalfSupport) ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.ARGB32);
			if (destination != null)
			{
				RenderTexture.ReleaseTemporary(destination);
				destination = null;
			}
			destination = RenderTexture.GetTemporary(Math.Max(2, Convert.ToInt32((float)cameraTextureDescriptor.width * scale)), Math.Max(2, Convert.ToInt32((float)cameraTextureDescriptor.height * scale)), 0, format);
			destination.name = waterColorTextureName;
			destination.filterMode = FilterMode.Bilinear;
			ConfigureTarget(new RenderTargetIdentifier(destination));
			ConfigureClear(ClearFlag.All, Color.clear);
		}
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		CommandBuffer commandBuffer = CommandBufferPool.Get("Water Render Feature Pass");
		if (!smallColorRTBlit)
		{
			CoreUtils.SetKeyword(commandBuffer, "_DEPTH_TEXTURE_OFF", !enableSceneDepth);
			context.ExecuteCommandBuffer(commandBuffer);
			CommandBufferPool.Release(commandBuffer);
			return;
		}
		CoreUtils.SetKeyword(commandBuffer, "_DEPTH_TEXTURE_OFF", !enableSceneDepth);
		CoreUtils.SetKeyword(commandBuffer, "_READ_DEPTH_INSTEAD_OF_COPY", readDepthInsteadOfCopy);
		CoreUtils.SetKeyword(commandBuffer, "_RESOLVE_DEPTH_FOR_SMALL_RT", (double)scale - 0.5 < 1E-05);
		CoreUtils.SetKeyword(commandBuffer, "_SMALL_WATER_RT", state: true);
		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
		base.Execute(context, ref renderingData);
		commandBuffer = CommandBufferPool.Get("Water Update Next Stage Keywords");
		commandBuffer.SetGlobalTexture(waterColorTextureName, destination);
		CoreUtils.SetKeyword(commandBuffer, "_READ_DEPTH_INSTEAD_OF_COPY", state: false);
		CoreUtils.SetKeyword(commandBuffer, "_RESOLVE_DEPTH_FOR_SMALL_RT", state: false);
		CoreUtils.SetKeyword(commandBuffer, "_SAMPLE_WATER_COLOR_TEXTURE", state: true);
		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
	}

	public override void FrameCleanup(CommandBuffer cmd)
	{
		if (cmd == null)
		{
			throw new ArgumentNullException("cmd");
		}
		CoreUtils.SetKeyword(cmd, "_SAMPLE_WATER_COLOR_TEXTURE", state: false);
		CoreUtils.SetKeyword(cmd, "_SMALL_WATER_RT", state: false);
		CoreUtils.SetKeyword(cmd, "_DEPTH_TEXTURE_OFF", state: false);
		if (destination != null)
		{
			RenderTexture.ReleaseTemporary(destination);
			destination = null;
		}
	}
}
