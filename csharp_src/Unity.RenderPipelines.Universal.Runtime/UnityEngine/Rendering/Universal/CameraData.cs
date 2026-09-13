using System;
using System.Collections.Generic;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering.Universal;

[MovedFrom("UnityEngine.Rendering.LWRP")]
public struct CameraData
{
	private Matrix4x4 m_ViewMatrix;

	private Matrix4x4 m_ProjectionMatrix;

	public Camera camera;

	public CameraRenderType renderType;

	public RenderTexture targetTexture;

	public RenderTextureDescriptor cameraTargetDescriptor;

	internal Rect pixelRect;

	internal int pixelWidth;

	internal int pixelHeight;

	internal float aspectRatio;

	public float renderScale;

	public bool clearDepth;

	public CameraType cameraType;

	public bool isDefaultViewport;

	public bool isHdrEnabled;

	public bool requiresDepthTexture;

	public bool requiresOpaqueTexture;

	public bool isSceneViewCamera;

	public SortingCriteria defaultOpaqueSortFlags;

	public bool isStereoEnabled;

	internal int numberOfXRPasses;

	internal bool isXRMultipass;

	public float maxShadowDistance;

	public bool postProcessEnabled;

	public IEnumerator<Action<RenderTargetIdentifier, CommandBuffer>> captureActions;

	public LayerMask volumeLayerMask;

	public Transform volumeTrigger;

	public bool isStopNaNEnabled;

	public bool isDitheringEnabled;

	public AntialiasingMode antialiasing;

	public bool doFxaaInUberPost;

	public AntialiasingQuality antialiasingQuality;

	internal ScriptableRenderer renderer;

	public bool resolveFinalTarget;

	public bool isPreviewCamera => cameraType == CameraType.Preview;

	internal void SetViewAndProjectionMatrix(Matrix4x4 viewMatrix, Matrix4x4 projectionMatrix)
	{
		m_ViewMatrix = viewMatrix;
		m_ProjectionMatrix = projectionMatrix;
	}

	public Matrix4x4 GetViewMatrix()
	{
		return m_ViewMatrix;
	}

	public Matrix4x4 GetProjectionMatrix()
	{
		return m_ProjectionMatrix;
	}

	public Matrix4x4 GetGPUProjectionMatrix()
	{
		return GL.GetGPUProjectionMatrix(m_ProjectionMatrix, IsCameraProjectionMatrixFlipped());
	}

	public bool IsCameraProjectionMatrixFlipped()
	{
		ScriptableRenderer current = ScriptableRenderer.current;
		if (current != null)
		{
			bool flag = current.cameraColorTarget != BuiltinRenderTextureType.CameraTarget || targetTexture != null;
			return SystemInfo.graphicsUVStartsAtTop && flag;
		}
		return true;
	}
}
