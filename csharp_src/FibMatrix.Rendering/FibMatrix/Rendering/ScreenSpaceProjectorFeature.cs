using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FibMatrix.Rendering;

public class ScreenSpaceProjectorFeature : CustomRenderObjectsFeature
{
	private class ScreenSpaceProjectorPass : CustomRenderObjectsPass
	{
		private ScreenSpaceProjectorFeature m_Feature;

		private RenderTexture m_RT;

		public ScreenSpaceProjectorPass(RenderObjectsSettings settings)
			: base(settings)
		{
		}

		public void Setup(ScreenSpaceProjectorFeature feature)
		{
			m_Feature = feature;
		}

		public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
		{
			m_RT = RenderTexture.GetTemporary(Mathf.Max(4, (int)((float)cameraTextureDescriptor.width * m_Feature.rtScale)), Mathf.Max(4, (int)((float)cameraTextureDescriptor.height * m_Feature.rtScale)), 0, m_Feature.rtFormat);
			m_RT.hideFlags = HideFlags.HideAndDontSave;
			m_RT.name = m_Feature.rtName;
			ConfigureTarget(new RenderTargetIdentifier(m_RT));
			ConfigureClear(ClearFlag.Color, Color.gray);
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			base.Execute(context, ref renderingData);
			CommandBuffer commandBuffer = CommandBufferPool.Get("ScreenSpaceProjector");
			commandBuffer.SetGlobalTexture(m_Feature.rtName, m_RT);
			commandBuffer.SetGlobalFloat("_AOProjectorIntensity", m_Feature.intensity);
			commandBuffer.SetGlobalFloat("_AOProjectorScreenRange", 1f / m_Feature.sreenRangeScale);
			commandBuffer.SetGlobalColor("_AOProjectorColor", m_Feature.aoColor);
			context.ExecuteCommandBuffer(commandBuffer);
			CommandBufferPool.Release(commandBuffer);
		}

		public override void FrameCleanup(CommandBuffer cmd)
		{
			RenderTexture.ReleaseTemporary(m_RT);
			m_RT = null;
		}
	}

	public string rtName;

	[Range(0.1f, 1f)]
	public float rtScale = 0.5f;

	[Range(0.1f, 2f)]
	public float sreenRangeScale = 1.2f;

	[Range(0f, 1f)]
	public float intensity = 1f;

	public Color aoColor = Color.black;

	public RenderTextureFormat rtFormat = RenderTextureFormat.R8;

	private ScreenSpaceProjectorPass m_Pass;

	public override void Create()
	{
		m_Pass = new ScreenSpaceProjectorPass(settings);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (renderingData.cameraData.renderType == CameraRenderType.Base)
		{
			m_Pass.Setup(this);
			if (settings.cameraType.HasFlag(renderingData.cameraData.cameraType))
			{
				renderer.EnqueuePass(m_Pass);
			}
		}
	}
}
