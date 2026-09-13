using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Nova.Runtime.Core.Scripts;

public sealed class DistortedUvBufferPass : ScriptableRenderPass
{
	private const string ProfilerTag = "DistortedUvBufferPass";

	private readonly ProfilingSampler _profilingSampler = new ProfilingSampler("DistortedUvBufferPass");

	private readonly RenderQueueRange _renderQueueRange = RenderQueueRange.all;

	private readonly ShaderTagId _shaderTagId;

	private Func<RenderTargetIdentifier> _getCameraDepthTargetIdentifier;

	private FilteringSettings _filteringSettings;

	private RenderTargetIdentifier _renderTargetIdentifier;

	public DistortedUvBufferPass(string lightMode)
	{
		_filteringSettings = new FilteringSettings(_renderQueueRange);
		base.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
		_shaderTagId = new ShaderTagId(lightMode);
	}

	public void Setup(RenderTargetIdentifier renderTargetIdentifier, Func<RenderTargetIdentifier> getCameraDepthTargetIdentifier)
	{
		_renderTargetIdentifier = renderTargetIdentifier;
		_getCameraDepthTargetIdentifier = getCameraDepthTargetIdentifier;
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		CommandBuffer commandBuffer = CommandBufferPool.Get("DistortedUvBufferPass");
		commandBuffer.Clear();
		if (_getCameraDepthTargetIdentifier == null)
		{
			commandBuffer.SetRenderTarget(_renderTargetIdentifier, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.DontCare);
		}
		else
		{
			commandBuffer.SetRenderTarget(_renderTargetIdentifier, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, _getCameraDepthTargetIdentifier(), RenderBufferLoadAction.Load, RenderBufferStoreAction.DontCare);
		}
		commandBuffer.ClearRenderTarget(clearDepth: false, clearColor: true, Color.grey);
		using (new ProfilingScope(commandBuffer, _profilingSampler))
		{
			context.ExecuteCommandBuffer(commandBuffer);
			commandBuffer.Clear();
			DrawingSettings drawingSettings = CreateDrawingSettings(_shaderTagId, ref renderingData, SortingCriteria.CommonTransparent);
			context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref _filteringSettings);
		}
		context.ExecuteCommandBuffer(commandBuffer);
		commandBuffer.Clear();
		CommandBufferPool.Release(commandBuffer);
	}
}
