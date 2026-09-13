using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;

public class VerticalDepthRendererFeature : ScriptableRendererFeature
{
	private class VerticalDepthRenderPass : DepthOnlyPass
	{
		public const string VerticalDepthTextureName = "_VerticalDepthTexture";

		public const string VerticalDepthTextureParameter = "_VerticalDepthTextureParameter";

		public const string VerticalDepthParameter = "_VerticalDepthParameter";

		private Camera verticalDepthCamera;

		private RenderTexture rtDepth;

		private RenderTexture rtColor;

		public VerticalDepthRenderPass(RenderPassEvent evt, RenderQueueRange renderQueueRange, LayerMask layerMask)
			: base(evt, renderQueueRange, layerMask)
		{
		}

		public void Setup(Camera verticalDepthCamera, RenderTexture rtDepth, RenderTexture rtColor)
		{
			this.verticalDepthCamera = verticalDepthCamera;
			this.rtDepth = rtDepth;
			this.rtColor = rtColor;
		}

		public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
		{
			if (Vaild())
			{
				ConfigureTarget(new RenderTargetIdentifier(rtDepth));
				ConfigureClear(ClearFlag.All, Color.black);
			}
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			if (Vaild())
			{
				ref CameraData cameraData = ref renderingData.cameraData;
				CommandBuffer commandBuffer = CommandBufferPool.Get();
				float orthographicSize = verticalDepthCamera.orthographicSize;
				float num = orthographicSize * verticalDepthCamera.aspect;
				Matrix4x4 proj = Matrix4x4.Ortho(0f - num, num, 0f - orthographicSize, orthographicSize, verticalDepthCamera.nearClipPlane, verticalDepthCamera.farClipPlane);
				proj = GL.GetGPUProjectionMatrix(proj, cameraData.IsCameraProjectionMatrixFlipped());
				RenderingUtils.SetViewAndProjectionMatrices(commandBuffer, verticalDepthCamera.worldToCameraMatrix, proj, setInverseMatrices: false);
				context.ExecuteCommandBuffer(commandBuffer);
				CommandBufferPool.Release(commandBuffer);
				base.Execute(context, ref renderingData);
				CommandBuffer commandBuffer2 = CommandBufferPool.Get();
				commandBuffer2.SetRenderTarget(new RenderTargetIdentifier(rtColor));
				commandBuffer2.Blit(new RenderTargetIdentifier(rtDepth), new RenderTargetIdentifier(rtColor));
				commandBuffer2.SetRenderTarget(new RenderTargetIdentifier(rtDepth));
				Vector2 vector = new Vector2(verticalDepthCamera.transform.position.x, verticalDepthCamera.transform.position.z);
				Vector2 vector2 = new Vector2(verticalDepthCamera.orthographicSize * verticalDepthCamera.aspect, verticalDepthCamera.orthographicSize);
				Vector4 value = new Vector4(vector.x - vector2.x, vector.y - vector2.y, vector2.x * 2f, vector2.y * 2f);
				Vector4 value2 = new Vector4(verticalDepthCamera.nearClipPlane, verticalDepthCamera.farClipPlane, verticalDepthCamera.nearClipPlane, verticalDepthCamera.farClipPlane);
				commandBuffer2.SetGlobalTexture("_VerticalDepthTexture", new RenderTargetIdentifier(rtColor));
				commandBuffer2.SetGlobalVector("_VerticalDepthTextureParameter", value);
				commandBuffer2.SetGlobalVector("_VerticalDepthParameter", value2);
				RenderingUtils.SetViewAndProjectionMatrices(commandBuffer2, cameraData.GetViewMatrix(), cameraData.GetGPUProjectionMatrix(), setInverseMatrices: false);
				context.ExecuteCommandBuffer(commandBuffer2);
				CommandBufferPool.Release(commandBuffer2);
			}
		}

		public override void FrameCleanup(CommandBuffer cmd)
		{
			if (Vaild())
			{
				base.FrameCleanup(cmd);
			}
		}

		private bool Vaild()
		{
			if (verticalDepthCamera != null && rtDepth != null)
			{
				return rtColor != null;
			}
			return false;
		}
	}

	private VerticalDepthRenderPass m_ScriptablePass;

	private RenderTargetHandle destination;

	private const string DestinationName = "vertical_depth_destination";

	public RenderPassEvent renderPassEvent;

	public LayerMask layerMask;

	public Camera verticalDepthCamera { get; set; }

	public RenderTexture rtDepth { get; set; }

	public RenderTexture rtColor { get; set; }

	public bool autoDisable { get; set; } = true;

	public override void Create()
	{
		m_ScriptablePass = new VerticalDepthRenderPass(renderPassEvent, RenderQueueRange.opaque, layerMask);
		destination.Init("vertical_depth_destination");
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (renderingData.cameraData.renderType == CameraRenderType.Base)
		{
			m_ScriptablePass.Setup(verticalDepthCamera, rtDepth, rtColor);
			renderer.EnqueuePass(m_ScriptablePass);
			if (autoDisable)
			{
				SetActive(active: false);
			}
		}
	}
}
