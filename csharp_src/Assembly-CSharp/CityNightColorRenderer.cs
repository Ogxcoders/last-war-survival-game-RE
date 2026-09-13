using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CityNightColorRenderer : ScriptableRendererFeature
{
	private class CityNightColorRendererPass : ScriptableRenderPass
	{
		private RenderTargetIdentifier mRenderTargetIdentifier;

		public void Release()
		{
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			CommandBuffer commandBuffer = CommandBufferPool.Get("S4_NightColor");
			Color mNightColor = CityNightColorRenderer.mNightColor;
			commandBuffer.SetGlobalColor(mEvColorId, mNightColor);
			commandBuffer.SetGlobalInt(mEvColorOnId, 1);
			commandBuffer.SetGlobalColor(mCityFogColorId, mCityFogColor);
			context.ExecuteCommandBuffer(commandBuffer);
			commandBuffer.Clear();
			CommandBufferPool.Release(commandBuffer);
		}
	}

	private class CityNightColorClearPass : ScriptableRenderPass
	{
		public void Release()
		{
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			CommandBuffer commandBuffer = CommandBufferPool.Get("S4_NightColorClear");
			commandBuffer.SetGlobalInt(mEvColorOnId, 0);
			context.ExecuteCommandBuffer(commandBuffer);
			commandBuffer.Clear();
			CommandBufferPool.Release(commandBuffer);
		}
	}

	public static readonly int mEvColorOnId = Shader.PropertyToID("_EvColorOn");

	public static readonly int mEvColorId = Shader.PropertyToID("_EvColor");

	public static readonly int mCityFogColorId = Shader.PropertyToID("_CityFogColor");

	public static Color mNightColor = Color.gray;

	public static Color mCityFogColor = Color.gray;

	private CityNightColorRendererPass m_NightColorPass;

	private CityNightColorClearPass m_NightClearPass;

	public override void Create()
	{
		m_NightColorPass = new CityNightColorRendererPass();
		m_NightClearPass = new CityNightColorClearPass();
		m_NightColorPass.renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
		m_NightClearPass.renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (renderingData.cameraData.renderType == CameraRenderType.Base)
		{
			if (renderingData.cameraData.targetTexture == null)
			{
				renderer.EnqueuePass(m_NightColorPass);
			}
			else
			{
				renderer.EnqueuePass(m_NightClearPass);
			}
		}
	}

	public void AttachToRenderer(Camera camera, Color color, Color fogColor)
	{
		UniversalAdditionalCameraData component = camera.GetComponent<UniversalAdditionalCameraData>();
		if (!component.scriptableRenderer.rendererFeatures.Contains(this))
		{
			component.scriptableRenderer.rendererFeatures.Add(this);
		}
		mNightColor = color;
		mCityFogColor = fogColor;
	}

	public void DetachFromRenderer(Camera camera)
	{
		UniversalAdditionalCameraData component = camera.GetComponent<UniversalAdditionalCameraData>();
		if (component.scriptableRenderer.rendererFeatures.Contains(this))
		{
			component.scriptableRenderer.rendererFeatures.Remove(this);
			m_NightColorPass.Release();
			m_NightClearPass.Release();
		}
		Shader.SetGlobalColor(mEvColorId, Color.grey);
		Shader.SetGlobalInt(mEvColorOnId, 0);
	}

	public void SetNightColor(Color color, Color fogColor)
	{
		mNightColor = color;
		mCityFogColor = fogColor;
	}
}
