using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Experimental.Rendering.Universal;

[ExcludeFromPreset]
[MovedFrom("UnityEngine.Experimental.Rendering.LWRP")]
public class RenderObjects : ScriptableRendererFeature
{
	[Serializable]
	public class RenderObjectsSettings
	{
		public string passTag = "RenderObjectsFeature";

		public RenderPassEvent Event = RenderPassEvent.AfterRenderingOpaques;

		public FilterSettings filterSettings = new FilterSettings();

		public Material overrideMaterial;

		public int overrideMaterialPassIndex;

		public bool overrideDepthState;

		public CompareFunction depthCompareFunction = CompareFunction.LessEqual;

		public bool enableWrite = true;

		public StencilStateData stencilSettings = new StencilStateData();

		public CustomCameraSettings cameraSettings = new CustomCameraSettings();
	}

	[Serializable]
	public class FilterSettings
	{
		public RenderQueueType RenderQueueType;

		public LayerMask LayerMask;

		public string[] PassNames;

		public FilterSettings()
		{
			RenderQueueType = RenderQueueType.Opaque;
			LayerMask = 0;
		}
	}

	[Serializable]
	public class CustomCameraSettings
	{
		public bool overrideCamera;

		public bool restoreCamera = true;

		public Vector4 offset;

		public float cameraFieldOfView = 60f;
	}

	public RenderObjectsSettings settings = new RenderObjectsSettings();

	private RenderObjectsPass renderObjectsPass;

	public override void Create()
	{
		FilterSettings filterSettings = settings.filterSettings;
		if (settings.Event < RenderPassEvent.BeforeRenderingPrepasses)
		{
			settings.Event = RenderPassEvent.BeforeRenderingPrepasses;
		}
		renderObjectsPass = new RenderObjectsPass(settings.passTag, settings.Event, filterSettings.PassNames, filterSettings.RenderQueueType, filterSettings.LayerMask, settings.cameraSettings);
		renderObjectsPass.overrideMaterial = settings.overrideMaterial;
		renderObjectsPass.overrideMaterialPassIndex = settings.overrideMaterialPassIndex;
		if (settings.overrideDepthState)
		{
			renderObjectsPass.SetDetphState(settings.enableWrite, settings.depthCompareFunction);
		}
		if (settings.stencilSettings.overrideStencilState)
		{
			renderObjectsPass.SetStencilState(settings.stencilSettings.stencilReference, settings.stencilSettings.stencilCompareFunction, settings.stencilSettings.passOperation, settings.stencilSettings.failOperation, settings.stencilSettings.zFailOperation);
		}
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(renderObjectsPass);
	}
}
