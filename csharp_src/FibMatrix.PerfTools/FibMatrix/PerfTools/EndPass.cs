using System.Reflection;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FibMatrix.PerfTools;

public class EndPass : ScriptableRenderPass
{
	private static readonly RenderTargetIdentifier k_TargetNone = new RenderTargetIdentifier(BuiltinRenderTextureType.None);

	private string _ProfilerTag;

	private RenderTargetIdentifier m_RenderTarget = k_TargetNone;

	private RenderTargetIdentifier m_OutputTarget = k_TargetNone;

	public EndPass(string profilerTag)
	{
		_ProfilerTag = profilerTag;
	}

	public void setRenderTarget(RenderTargetIdentifier renderTarget)
	{
		m_RenderTarget = renderTarget;
	}

	public void setOutputTarget(RenderTargetIdentifier outputTarget)
	{
		m_OutputTarget = outputTarget;
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		CommandBuffer commandBuffer = CommandBufferPool.Get(_ProfilerTag);
		if (renderingData.cameraData.camera.TryGetComponent<UniversalAdditionalCameraData>(out var _) && !m_OutputTarget.Equals(k_TargetNone))
		{
			RenderTargetIdentifier renderTargetIdentifier;
			RenderTargetIdentifier depth;
			if (renderingData.cameraData.GetType().GetField("renderer", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(renderingData.cameraData) is ScriptableRenderer scriptableRenderer)
			{
				renderTargetIdentifier = scriptableRenderer.cameraColorTarget;
				depth = scriptableRenderer.cameraDepth;
			}
			else
			{
				renderTargetIdentifier = BuiltinRenderTextureType.CameraTarget;
				depth = BuiltinRenderTextureType.CameraTarget;
			}
			Blit(commandBuffer, renderTargetIdentifier, m_OutputTarget);
			commandBuffer.SetRenderTarget(renderTargetIdentifier, depth);
			context.ExecuteCommandBuffer(commandBuffer);
			commandBuffer.Clear();
		}
		CommandBufferPool.Release(commandBuffer);
	}
}
