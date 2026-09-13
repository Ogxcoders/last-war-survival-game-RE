using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FibMatrix.RenderingHotUpdatable;

[ExcludeFromPreset]
public class CustomRenderObjectsFeatureHotUpdatable : ScriptableRendererFeature
{
	[Serializable]
	public class RenderObjectsSettings
	{
		public string passTag = "RenderObjectsFeature";

		public CameraType cameraType = (CameraType)(-1);

		public RenderPassEvent Event = RenderPassEvent.AfterRenderingOpaques;

		public uint eventOffset;

		public FilterSettings filterSettings = new FilterSettings();

		public Material overrideMaterial;

		public bool overrideGlobalProperties;

		public int overrideMaterialPassIndex;

		public bool overrideDepthState;

		public CompareFunction depthCompareFunction = CompareFunction.LessEqual;

		public bool enableWrite = true;

		public StencilStateData stencilSettings = new StencilStateData();

		public RenderObjects.CustomCameraSettings cameraSettings = new RenderObjects.CustomCameraSettings();

		public RenderTargetSettings renderTargetSettings = new RenderTargetSettings();
	}

	[Serializable]
	public class RenderTargetSettings
	{
		public bool overrideTarget;

		public string name;

		public float scale = 1f;

		public Vector2 size;

		public int depth;

		public RenderTextureFormat format;

		public ClearFlag clearFlag;

		public Color clearColor;

		public FilterMode filterMode = FilterMode.Bilinear;
	}

	[Serializable]
	public class FilterSettings
	{
		public RenderQueueType RenderQueueType;

		public LayerMask LayerMask;

		public string[] PassNames;

		public int RenderingLayerMask;

		public FilterSettings()
		{
			RenderQueueType = RenderQueueType.Opaque;
			LayerMask = 0;
			RenderingLayerMask = -1;
		}
	}

	public RenderObjectsSettings settings = new RenderObjectsSettings();

	private CustomRenderObjectsPassHotUpdatable renderObjectsPass;

	public override void Create()
	{
		renderObjectsPass = new CustomRenderObjectsPassHotUpdatable(settings);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (renderingData.cameraData.renderType == CameraRenderType.Base && settings.cameraType.HasFlag(renderingData.cameraData.cameraType))
		{
			renderer.EnqueuePass(renderObjectsPass);
		}
	}
}
