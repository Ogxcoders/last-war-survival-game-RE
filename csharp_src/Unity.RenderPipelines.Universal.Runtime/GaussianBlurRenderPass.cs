using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GaussianBlurRenderPass : ScriptableRenderPass
{
	public Material m_VerticalBlurMat;

	public Material m_HorizontalBlurMat;

	private int blurCount;

	private int downSample;

	private float intensity;

	private RenderTargetHandle m_temporaryColorTexture01;

	private RenderTargetHandle m_temporaryColorTexture02;

	private RenderTargetHandle m_temporaryColorTexture03;

	private string m_ProfilerTag;

	private static int _offset = Shader.PropertyToID("_offset");

	public FilterMode filterMode { get; set; }

	private RenderTargetIdentifier source { get; set; }

	private RenderTargetHandle destination { get; set; }

	public GaussianBlurRenderPass(RenderPassEvent renderPassEvent, Material VerticalBlurMat, Material HorizontalBlurMat, string tag, int downSample, int blurCount, float intensity)
	{
		base.renderPassEvent = renderPassEvent;
		m_VerticalBlurMat = VerticalBlurMat;
		m_HorizontalBlurMat = HorizontalBlurMat;
		this.downSample = downSample;
		this.blurCount = blurCount;
		this.intensity = intensity;
		m_ProfilerTag = tag;
		m_temporaryColorTexture01.Init("_temporaryColorTexture1");
		m_temporaryColorTexture02.Init("_temporaryColorTexture2");
		m_temporaryColorTexture03.Init("_temporaryColorTexture3");
	}

	public void Setup(RenderTargetIdentifier source, RenderTargetHandle destination)
	{
		this.source = source;
		this.destination = destination;
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		if ((renderingData.cameraData.camera.cullingMask & (1 << LayerMask.NameToLayer("Hud3D"))) <= 0)
		{
			CommandBuffer commandBuffer = CommandBufferPool.Get(m_ProfilerTag);
			RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
			cameraTargetDescriptor.width >>= downSample;
			cameraTargetDescriptor.height >>= downSample;
			cameraTargetDescriptor.depthBufferBits = 0;
			commandBuffer.GetTemporaryRT(m_temporaryColorTexture01.id, cameraTargetDescriptor, FilterMode.Bilinear);
			commandBuffer.GetTemporaryRT(m_temporaryColorTexture02.id, cameraTargetDescriptor, FilterMode.Bilinear);
			commandBuffer.GetTemporaryRT(m_temporaryColorTexture03.id, cameraTargetDescriptor, FilterMode.Bilinear);
			commandBuffer.Blit(source, m_temporaryColorTexture03.Identifier());
			for (int i = 0; i < blurCount; i++)
			{
				m_VerticalBlurMat.SetVector(_offset, new Vector2(0f, intensity));
				commandBuffer.Blit(m_temporaryColorTexture03.Identifier(), m_temporaryColorTexture01.Identifier(), m_VerticalBlurMat);
				m_HorizontalBlurMat.SetVector(_offset, new Vector2(intensity, 0f));
				commandBuffer.Blit(m_temporaryColorTexture01.Identifier(), m_temporaryColorTexture02.Identifier(), m_HorizontalBlurMat);
				commandBuffer.Blit(m_temporaryColorTexture02.Identifier(), m_temporaryColorTexture03.Identifier());
			}
			commandBuffer.Blit(m_temporaryColorTexture03.Identifier(), source);
			context.ExecuteCommandBuffer(commandBuffer);
			CommandBufferPool.Release(commandBuffer);
		}
	}
}
