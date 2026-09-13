using System;

namespace UnityEngine.Rendering;

public abstract class RenderPipeline
{
	public bool disposed { get; private set; }

	protected abstract void Render(ScriptableRenderContext context, Camera[] cameras);

	protected static void BeginFrameRendering(ScriptableRenderContext context, Camera[] cameras)
	{
		RenderPipelineManager.BeginFrameRendering(context, cameras);
	}

	protected static void BeginCameraRendering(ScriptableRenderContext context, Camera camera)
	{
		RenderPipelineManager.BeginCameraRendering(context, camera);
	}

	protected static void EndFrameRendering(ScriptableRenderContext context, Camera[] cameras)
	{
		RenderPipelineManager.EndFrameRendering(context, cameras);
	}

	protected static void EndCameraRendering(ScriptableRenderContext context, Camera camera)
	{
		RenderPipelineManager.EndCameraRendering(context, camera);
	}

	internal void InternalRender(ScriptableRenderContext context, Camera[] cameras)
	{
		if (disposed)
		{
			throw new ObjectDisposedException($"{this} has been disposed. Do not call Render on disposed a RenderPipeline.");
		}
		Render(context, cameras);
	}

	internal void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
		disposed = true;
	}

	protected virtual void Dispose(bool disposing)
	{
	}
}
