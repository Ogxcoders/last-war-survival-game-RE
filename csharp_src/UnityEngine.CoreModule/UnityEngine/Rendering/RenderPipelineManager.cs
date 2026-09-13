using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering;

public static class RenderPipelineManager
{
	internal static RenderPipelineAsset s_CurrentPipelineAsset;

	private static Camera[] s_Cameras = new Camera[0];

	private static int s_CameraCapacity = 0;

	public static RenderPipeline currentPipeline { get; private set; }

	public static event Action<ScriptableRenderContext, Camera[]> beginFrameRendering;

	public static event Action<ScriptableRenderContext, Camera> beginCameraRendering;

	public static event Action<ScriptableRenderContext, Camera[]> endFrameRendering;

	public static event Action<ScriptableRenderContext, Camera> endCameraRendering;

	internal static void BeginFrameRendering(ScriptableRenderContext context, Camera[] cameras)
	{
		RenderPipelineManager.beginFrameRendering?.Invoke(context, cameras);
	}

	internal static void BeginCameraRendering(ScriptableRenderContext context, Camera camera)
	{
		RenderPipelineManager.beginCameraRendering?.Invoke(context, camera);
	}

	internal static void EndFrameRendering(ScriptableRenderContext context, Camera[] cameras)
	{
		RenderPipelineManager.endFrameRendering?.Invoke(context, cameras);
	}

	internal static void EndCameraRendering(ScriptableRenderContext context, Camera camera)
	{
		RenderPipelineManager.endCameraRendering?.Invoke(context, camera);
	}

	[RequiredByNativeCode]
	internal static void CleanupRenderPipeline()
	{
		if (currentPipeline != null && !currentPipeline.disposed)
		{
			currentPipeline.Dispose();
			s_CurrentPipelineAsset = null;
			currentPipeline = null;
			SupportedRenderingFeatures.active = new SupportedRenderingFeatures();
		}
	}

	private static void GetCameras(ScriptableRenderContext context)
	{
		int numberOfCameras = context.GetNumberOfCameras();
		if (numberOfCameras != s_CameraCapacity)
		{
			Array.Resize(ref s_Cameras, numberOfCameras);
			s_CameraCapacity = numberOfCameras;
		}
		for (int i = 0; i < numberOfCameras; i++)
		{
			s_Cameras[i] = context.GetCamera(i);
		}
	}

	[RequiredByNativeCode]
	private static void DoRenderLoop_Internal(RenderPipelineAsset pipe, IntPtr loopPtr)
	{
		PrepareRenderPipeline(pipe);
		if (currentPipeline != null)
		{
			ScriptableRenderContext context = new ScriptableRenderContext(loopPtr);
			Array.Clear(s_Cameras, 0, s_Cameras.Length);
			GetCameras(context);
			currentPipeline.InternalRender(context, s_Cameras);
			Array.Clear(s_Cameras, 0, s_Cameras.Length);
		}
	}

	internal static void PrepareRenderPipeline(RenderPipelineAsset pipelineAsset)
	{
		if ((object)s_CurrentPipelineAsset != pipelineAsset)
		{
			CleanupRenderPipeline();
			s_CurrentPipelineAsset = pipelineAsset;
		}
		if (s_CurrentPipelineAsset != null && (currentPipeline == null || currentPipeline.disposed))
		{
			currentPipeline = s_CurrentPipelineAsset.InternalCreatePipeline();
		}
	}
}
