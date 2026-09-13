using System;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

namespace FibMatrix.Rendering;

public static class RenderObjectsSettingsExtensions
{
	public static T CreateRenderObjectsPass<T>(this RenderObjects.RenderObjectsSettings settings) where T : RenderObjectsPass
	{
		RenderObjects.FilterSettings filterSettings = settings.filterSettings;
		if (settings.Event < RenderPassEvent.BeforeRenderingPrepasses)
		{
			settings.Event = RenderPassEvent.BeforeRenderingPrepasses;
		}
		T val = Activator.CreateInstance(typeof(T), settings.passTag, settings.Event, filterSettings.PassNames, filterSettings.RenderQueueType, filterSettings.LayerMask.value, settings.cameraSettings) as T;
		val.overrideMaterial = settings.overrideMaterial;
		val.overrideMaterialPassIndex = settings.overrideMaterialPassIndex;
		if (settings.overrideDepthState)
		{
			val.SetDetphState(settings.enableWrite, settings.depthCompareFunction);
		}
		if (settings.stencilSettings.overrideStencilState)
		{
			val.SetStencilState(settings.stencilSettings.stencilReference, settings.stencilSettings.stencilCompareFunction, settings.stencilSettings.passOperation, settings.stencilSettings.failOperation, settings.stencilSettings.zFailOperation);
		}
		return val;
	}
}
