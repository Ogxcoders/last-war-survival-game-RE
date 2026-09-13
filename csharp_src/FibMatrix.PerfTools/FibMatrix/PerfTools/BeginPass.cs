using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FibMatrix.PerfTools;

public class BeginPass : ScriptableRenderPass
{
	private string _ProfilerTag;

	public BeginPass(string profilerTag)
	{
		_ProfilerTag = profilerTag;
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		CommandBuffer commandBuffer = CommandBufferPool.Get(_ProfilerTag);
		Camera camera = renderingData.cameraData.camera;
		bool clearDepth = renderingData.cameraData.clearDepth;
		if (camera.TryGetComponent<UniversalAdditionalCameraData>(out var component))
		{
			RenderTargetIdentifier color;
			RenderTargetIdentifier depth;
			if (renderingData.cameraData.GetType().GetField("renderer", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(renderingData.cameraData) is ScriptableRenderer scriptableRenderer)
			{
				color = scriptableRenderer.cameraColorTarget;
				depth = scriptableRenderer.cameraDepth;
			}
			else
			{
				color = BuiltinRenderTextureType.CameraTarget;
				depth = BuiltinRenderTextureType.CameraTarget;
			}
			commandBuffer.SetRenderTarget(color, depth);
			if (component.renderType == CameraRenderType.Base)
			{
				commandBuffer.ClearRenderTarget(clearDepth: true, clearColor: true, Color.black);
			}
			else
			{
				commandBuffer.ClearRenderTarget(clearDepth, clearColor: true, Color.black);
			}
			context.ExecuteCommandBuffer(commandBuffer);
		}
		CommandBufferPool.Release(commandBuffer);
	}
}
