using GameFramework;
using Main.Scripts.Scene.LightAndDark;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WorldFogRendererRenderer : ScriptableRendererFeature
{
	private class WorldFogRendererPass : ScriptableRenderPass
	{
		private WorldFogInstanceRenderer mInstanceRender;

		private const string ProfilerTag = "TerrainQuadLayerRenderPass";

		private readonly ProfilingSampler _profilingSampler = new ProfilingSampler("TerrainQuadLayerRenderPass");

		private int _width;

		private int _height;

		private int _downScaledWidth;

		private int _downScaledHeight;

		private int _rtFogNameID;

		public RenderTexture mFogRT;

		private RenderTargetIdentifier mRenderTargetIdentifier;

		public Material mFowBlurMat;

		private RenderTexture tempRT1;

		private RenderTexture tempRT2;

		public void Setup(WorldFogInstanceRenderer instanceRenderer, RenderTargetIdentifier renderTargetIdentifier, int width, int height)
		{
			mInstanceRender = instanceRenderer;
			mRenderTargetIdentifier = renderTargetIdentifier;
			_width = width;
			_height = height;
			float num = 0.125f;
			_downScaledWidth = (int)((float)_width * num);
			_downScaledHeight = (int)((float)_height * num);
			_rtFogNameID = Shader.PropertyToID("_FogRT");
		}

		public void Release()
		{
			RenderTexture.ReleaseTemporary(mFogRT);
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			if (!SystemInfo.supportsInstancing || mInstanceRender == null || !mInstanceRender.Ready())
			{
				return;
			}
			CommandBuffer commandBuffer = CommandBufferPool.Get("Fog RT Render Pass");
			bool num = mInstanceRender.TryCalFogBrushMatrix();
			bool flag = mInstanceRender.TryCalDiscoFogBrushMatrix();
			bool flag2 = mInstanceRender.TryCalMarchFogBrushMatrix();
			if (num || flag || flag2)
			{
				if (mFogRT == null)
				{
					if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8))
					{
						Log.Error("WorldFogRendererPass R8 RT not supported, fallback to ARGB32");
						mFogRT = RenderTexture.GetTemporary(_downScaledWidth, _downScaledHeight, 0, RenderTextureFormat.ARGB32);
					}
					else
					{
						mFogRT = RenderTexture.GetTemporary(_downScaledWidth, _downScaledHeight, 0, RenderTextureFormat.R8);
					}
					mFogRT.name = "FogRT";
				}
				commandBuffer.SetRenderTarget(mFogRT);
				commandBuffer.ClearRenderTarget(clearDepth: false, clearColor: true, Color.black);
				mInstanceRender.DrawFogBrushRT(commandBuffer);
			}
			mInstanceRender.mWorldFogMaterial.SetTexture(_rtFogNameID, mFogRT);
			commandBuffer.SetGlobalTexture(_rtFogNameID, mFogRT);
			commandBuffer.SetRenderTarget(mRenderTargetIdentifier);
			context.ExecuteCommandBuffer(commandBuffer);
			commandBuffer.Clear();
			CommandBufferPool.Release(commandBuffer);
		}
	}

	public static string RenderPass_FogBrush = "s2";

	private WorldFogRendererPass m_ScriptablePass;

	private WorldFogInstanceRenderer mInstanceRender;

	public override void Create()
	{
		m_ScriptablePass = new WorldFogRendererPass();
		m_ScriptablePass.renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (renderingData.cameraData.renderType == CameraRenderType.Base)
		{
			RenderTargetIdentifier cameraColorTarget = renderer.cameraColorTarget;
			RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
			int width = cameraTargetDescriptor.width;
			int height = cameraTargetDescriptor.height;
			m_ScriptablePass.Setup(mInstanceRender, cameraColorTarget, width, height);
			renderer.EnqueuePass(m_ScriptablePass);
		}
	}

	public void SetFogInstanceRender(WorldFogInstanceRenderer instanceRender)
	{
		mInstanceRender = instanceRender;
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
