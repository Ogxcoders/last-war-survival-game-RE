using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TerrainQuadLayerRenderer : ScriptableRendererFeature
{
	private class TerrainQuadLayerRenderPass : ScriptableRenderPass
	{
		private ITerrainQuadLayer m_TerrainQuadLayer;

		private const string ProfilerTag = "TerrainQuadLayerRenderPass";

		private readonly ProfilingSampler _profilingSampler = new ProfilingSampler("TerrainQuadLayerRenderPass");

		private RenderTargetIdentifier _renderTargetIdentifier;

		private int _width;

		private int _height;

		private int _downScaledWidth;

		private int _downScaledHeight;

		private int _rtNoiseNameID;

		private int _texNoiseNameID;

		private int _rtLayerNameID;

		private int _texLayerNameID;

		private RenderTexture _noiseRT;

		public void Setup(ITerrainQuadLayer terrainQuadLayer, RenderTargetIdentifier renderTargetIdentifier, int width, int height)
		{
			m_TerrainQuadLayer = terrainQuadLayer;
			_renderTargetIdentifier = renderTargetIdentifier;
			_width = width;
			_height = height;
			float num = 0.125f;
			_downScaledWidth = (int)((float)_width * num);
			_downScaledHeight = (int)((float)_height * num);
			_rtNoiseNameID = Shader.PropertyToID("TerrainNoiseMap");
			_texNoiseNameID = Shader.PropertyToID("_TerrainNoiseMap");
			_rtLayerNameID = Shader.PropertyToID("TerrainLayerMap");
			_texLayerNameID = Shader.PropertyToID("_TerrainLayerMap");
		}

		public void Release()
		{
			RenderTexture.ReleaseTemporary(_noiseRT);
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			if (!SystemInfo.supportsInstancing || !m_TerrainQuadLayer.Ready() || string.IsNullOrEmpty(m_TerrainQuadLayer.passId))
			{
				return;
			}
			CommandBuffer commandBuffer = CommandBufferPool.Get("Terrain Quad Layer Render Pass");
			bool flag = m_TerrainQuadLayer.Prepare();
			if (m_TerrainQuadLayer.passId == RenderPass_S2)
			{
				if (flag)
				{
					if (_noiseRT == null)
					{
						_noiseRT = RenderTexture.GetTemporary(_downScaledWidth, _downScaledHeight, 0, RenderTextureFormat.ARGB32);
					}
					commandBuffer.SetRenderTarget(_noiseRT);
					commandBuffer.ClearRenderTarget(clearDepth: false, clearColor: true, Color.black);
					m_TerrainQuadLayer.DrawNoise(commandBuffer);
				}
				commandBuffer.SetGlobalTexture(_texNoiseNameID, _noiseRT);
			}
			if (renderingData.cameraData.isHdrEnabled)
			{
				commandBuffer.GetTemporaryRT(_rtLayerNameID, _width, _height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
				commandBuffer.SetRenderTarget(_rtLayerNameID);
				commandBuffer.ClearRenderTarget(clearDepth: false, clearColor: true, Color.black);
				m_TerrainQuadLayer.DrawLayers(commandBuffer);
				commandBuffer.Blit(_rtLayerNameID, _renderTargetIdentifier);
				commandBuffer.ReleaseTemporaryRT(_rtLayerNameID);
			}
			else
			{
				commandBuffer.SetRenderTarget(_renderTargetIdentifier);
				m_TerrainQuadLayer.DrawLayers(commandBuffer);
			}
			context.ExecuteCommandBuffer(commandBuffer);
			commandBuffer.Clear();
			CommandBufferPool.Release(commandBuffer);
		}
	}

	public static string RenderPass_S2 = "s2";

	public static string RenderPass_S3 = "s3";

	private TerrainQuadLayerRenderPass m_ScriptablePass;

	private ITerrainQuadLayer m_TerrainQuadLayer;

	public override void Create()
	{
		m_ScriptablePass = new TerrainQuadLayerRenderPass();
		m_ScriptablePass.renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (renderingData.cameraData.renderType == CameraRenderType.Base && m_TerrainQuadLayer != null)
		{
			RenderTargetIdentifier cameraColorTarget = renderer.cameraColorTarget;
			RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
			int width = cameraTargetDescriptor.width;
			int height = cameraTargetDescriptor.height;
			m_ScriptablePass.Setup(m_TerrainQuadLayer, cameraColorTarget, width, height);
			renderer.EnqueuePass(m_ScriptablePass);
		}
	}

	public void SetTerrainQuadLayer(ITerrainQuadLayer terrainQuadLayer)
	{
		m_TerrainQuadLayer = terrainQuadLayer;
	}

	public void AttachToRenderer(Camera camera)
	{
		UniversalAdditionalCameraData component = camera.GetComponent<UniversalAdditionalCameraData>();
		if (!component.scriptableRenderer.rendererFeatures.Contains(this))
		{
			component.scriptableRenderer.rendererFeatures.Add(this);
		}
	}

	public void DetachFromRenderer(Camera camera)
	{
		UniversalAdditionalCameraData component = camera.GetComponent<UniversalAdditionalCameraData>();
		if (component.scriptableRenderer.rendererFeatures.Contains(this))
		{
			component.scriptableRenderer.rendererFeatures.Remove(this);
			m_ScriptablePass.Release();
		}
	}
}
