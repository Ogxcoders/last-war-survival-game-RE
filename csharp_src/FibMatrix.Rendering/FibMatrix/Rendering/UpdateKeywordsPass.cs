using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FibMatrix.Rendering;

[ExcludeFromPreset]
public class UpdateKeywordsPass : ScriptableRenderPass
{
	private const string ProfilerTag = "ClearKeywordsPass";

	private readonly UpdateKeywordsRendererFeature.KeywordsState[] Keywords;

	public UpdateKeywordsPass(RenderPassEvent evt, UpdateKeywordsRendererFeature.KeywordsState[] keywords)
	{
		base.renderPassEvent = evt;
		Keywords = keywords;
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		CommandBuffer commandBuffer = CommandBufferPool.Get("ClearKeywordsPass");
		if (Keywords != null)
		{
			for (int i = 0; i < Keywords.Length; i++)
			{
				CoreUtils.SetKeyword(commandBuffer, Keywords[i].keywords, Keywords[i].enable);
			}
		}
		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
	}
}
