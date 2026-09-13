namespace UnityEngine.Rendering.Universal;

internal class PostProcessPassCompat : ScriptableRenderPass
{
	private RenderTargetHandle m_Source;

	private RenderTargetHandle m_Destination;

	private RenderTextureDescriptor m_Descriptor;

	private RenderTargetHandle m_TemporaryColorTexture;

	private bool m_IsOpaquePostProcessing;

	private const string k_RenderPostProcessingTag = "Render PostProcessing Effects (Compat)";

	public PostProcessPassCompat(RenderPassEvent evt, bool renderOpaques = false)
	{
		m_IsOpaquePostProcessing = renderOpaques;
		m_TemporaryColorTexture.Init("_TemporaryColorTexture");
		base.renderPassEvent = evt;
	}

	public void Setup(RenderTextureDescriptor baseDescriptor, RenderTargetHandle sourceHandle, RenderTargetHandle destinationHandle)
	{
		m_Descriptor = baseDescriptor;
		m_Source = sourceHandle;
		m_Destination = destinationHandle;
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
	}
}
