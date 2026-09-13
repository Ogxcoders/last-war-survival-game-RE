using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AreaHighlightingFeature : ScriptableRendererFeature
{
	[Serializable]
	public class AreaSettings
	{
		public Vector3 centerPos;

		public int worldSize = 256;

		public Material mat;

		public Color inSideColor;

		public Color addSideColor = Color.black;

		public Color drakColor = Color.black;

		public Color jibanColor = Color.white;

		[Range(0f, 3f)]
		public float brightness = 1f;

		[Range(0f, 3f)]
		public float saturation = 1f;

		[Range(0f, 3f)]
		public float contrast = 1f;

		public float diffusePower = 1f;

		public float diffuseScale = 1f;

		[Range(0f, 1f)]
		public float _cutThreshold = 0.444f;

		public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRendering;

		public bool isDebug;
	}

	public AreaSettings m_Setting = new AreaSettings();

	private AreaHighlightingPass m_pass;

	private RenderTargetHandle m_renderTargetHandle;

	public bool IsDebug => m_Setting.isDebug;

	public override void Create()
	{
		m_pass = new AreaHighlightingPass(m_Setting.worldSize, m_Setting.centerPos, m_Setting.mat, 0f, m_Setting.inSideColor, m_Setting.brightness, m_Setting.saturation, m_Setting.contrast, m_Setting.renderPassEvent, m_Setting.diffusePower, m_Setting.diffuseScale, m_Setting._cutThreshold, m_Setting.addSideColor, m_Setting.isDebug, m_Setting.drakColor);
		m_pass.Init();
		m_renderTargetHandle.Init("ScreenTexture");
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (!renderingData.cameraData.isSceneViewCamera)
		{
			RenderTargetIdentifier cameraColorTarget = renderer.cameraColorTarget;
			RenderTargetHandle renderTargetHandle = m_renderTargetHandle;
			m_pass.Setup(cameraColorTarget, renderTargetHandle);
			renderer.EnqueuePass(m_pass);
		}
	}
}
