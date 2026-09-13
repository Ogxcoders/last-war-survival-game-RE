using UnityEngine;

namespace FibMatrix.Rendering;

public static class RenderPipelineCallbackUtilities
{
	public static RenderPipelineCallback GetOrCreateRenderPipelineCallback(GameObject gameObject, ref RenderPipelineCallback renderPipelineCallback)
	{
		if (renderPipelineCallback == null)
		{
			renderPipelineCallback = gameObject.GetComponentInChildren<RenderPipelineCallback>(includeInactive: true);
		}
		if (renderPipelineCallback == null)
		{
			renderPipelineCallback = gameObject.AddComponent<RenderPipelineCallback>();
		}
		return renderPipelineCallback;
	}
}
