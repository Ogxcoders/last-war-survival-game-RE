using System.Reflection;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FibMatrix.PerfTools.RenderDataForURP;

public class CopyPass : ScriptableRenderPass
{
	private string _ProfilerTag;

	private static readonly RenderTargetIdentifier k_TargetNone = new RenderTargetIdentifier(BuiltinRenderTextureType.None);

	private static RenderTargetIdentifier m_OutputTarget = k_TargetNone;

	public static void SetOutputTarget(RenderTargetIdentifier outputTarget)
	{
		m_OutputTarget = outputTarget;
	}

	public static void ClearOutputTarget()
	{
		m_OutputTarget = k_TargetNone;
	}

	public CopyPass(string profilerTag)
	{
		_ProfilerTag = profilerTag;
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		CommandBuffer commandBuffer = CommandBufferPool.Get(_ProfilerTag);
		if (!renderingData.cameraData.isSceneViewCamera && !m_OutputTarget.Equals(k_TargetNone))
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
