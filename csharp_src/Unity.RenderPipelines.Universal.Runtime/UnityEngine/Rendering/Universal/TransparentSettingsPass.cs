namespace UnityEngine.Rendering.Universal;

internal class TransparentSettingsPass : ScriptableRenderPass
{
	private bool m_shouldReceiveShadows;

	private const string m_ProfilerTag = "Transparent Settings Pass";

	public TransparentSettingsPass(RenderPassEvent evt, bool shadowReceiveSupported)
	{
		base.renderPassEvent = evt;
		m_shouldReceiveShadows = shadowReceiveSupported;
	}

	public bool Setup(ref RenderingData renderingData)
	{
		return !m_shouldReceiveShadows;
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		CommandBuffer commandBuffer = CommandBufferPool.Get("Transparent Settings Pass");
		CoreUtils.SetKeyword(commandBuffer, ShaderKeywordStrings.MainLightShadows, m_shouldReceiveShadows);
		CoreUtils.SetKeyword(commandBuffer, ShaderKeywordStrings.MainLightShadowCascades, m_shouldReceiveShadows);
		CoreUtils.SetKeyword(commandBuffer, ShaderKeywordStrings.AdditionalLightShadows, m_shouldReceiveShadows);
		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
	}
}
