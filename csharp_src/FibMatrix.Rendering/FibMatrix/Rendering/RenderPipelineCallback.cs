using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace FibMatrix.Rendering;

[ExecuteAlways]
public class RenderPipelineCallback : MonoBehaviour
{
	public event Action<ScriptableRenderContext, Camera> ActionBeginCameraRendering;

	public event Action<ScriptableRenderContext, Camera> ActionEndCameraRendering;

	private void OnEnable()
	{
		RenderPipelineManager.beginCameraRendering += BeginCameraRendering;
		RenderPipelineManager.endCameraRendering += EndCameraRendering;
	}

	private void OnDisable()
	{
		RenderPipelineManager.beginCameraRendering -= EndCameraRendering;
		RenderPipelineManager.endCameraRendering -= EndCameraRendering;
	}

	private void BeginCameraRendering(ScriptableRenderContext context, Camera camera)
	{
		this.ActionBeginCameraRendering?.Invoke(context, camera);
	}

	private void EndCameraRendering(ScriptableRenderContext context, Camera camera)
	{
		this.ActionEndCameraRendering?.Invoke(context, camera);
	}
}
