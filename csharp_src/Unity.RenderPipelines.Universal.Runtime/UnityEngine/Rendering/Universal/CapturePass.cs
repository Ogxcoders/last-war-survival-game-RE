using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal;

internal class CapturePass : ScriptableRenderPass
{
	private RenderTargetHandle m_CameraColorHandle;

	private const string m_ProfilerTag = "Capture Pass";

	public CapturePass(RenderPassEvent evt)
	{
		base.renderPassEvent = evt;
	}

	public void Setup(RenderTargetHandle colorHandle)
	{
		m_CameraColorHandle = colorHandle;
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		CommandBuffer commandBuffer = CommandBufferPool.Get("Capture Pass");
		RenderTargetIdentifier arg = m_CameraColorHandle.Identifier();
		IEnumerator<Action<RenderTargetIdentifier, CommandBuffer>> captureActions = renderingData.cameraData.captureActions;
		captureActions.Reset();
		while (captureActions.MoveNext())
		{
			captureActions.Current(arg, commandBuffer);
		}
		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
	}
}
