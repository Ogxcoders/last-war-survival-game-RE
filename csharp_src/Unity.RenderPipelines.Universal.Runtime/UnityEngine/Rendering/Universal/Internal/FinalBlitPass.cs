namespace UnityEngine.Rendering.Universal.Internal;

public class FinalBlitPass : ScriptableRenderPass
{
	private const string m_ProfilerTag = "Final Blit Pass";

	private RenderTargetHandle m_Source;

	private Material m_BlitMaterial;

	private TextureDimension m_TargetDimension;

	public FinalBlitPass(RenderPassEvent evt, Material blitMaterial)
	{
		m_BlitMaterial = blitMaterial;
		base.renderPassEvent = evt;
	}

	public void Setup(RenderTextureDescriptor baseDescriptor, RenderTargetHandle colorHandle)
	{
		m_Source = colorHandle;
		m_TargetDimension = baseDescriptor.dimension;
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		if (m_BlitMaterial == null)
		{
			Debug.LogErrorFormat("Missing {0}. {1} render pass will not execute. Check for missing reference in the renderer resources.", m_BlitMaterial, GetType().Name);
			return;
		}
		ref CameraData cameraData = ref renderingData.cameraData;
		RenderTargetIdentifier dest = ((cameraData.targetTexture != null) ? new RenderTargetIdentifier(cameraData.targetTexture) : ((RenderTargetIdentifier)BuiltinRenderTextureType.CameraTarget));
		bool flag = Display.main.requiresSrgbBlitToBackbuffer;
		bool isSceneViewCamera = cameraData.isSceneViewCamera;
		if (cameraData.isStereoEnabled)
		{
			flag = !XRGraphics.eyeTextureDesc.sRGB;
		}
		CommandBuffer commandBuffer = CommandBufferPool.Get("Final Blit Pass");
		if (flag)
		{
			commandBuffer.EnableShaderKeyword(ShaderKeywordStrings.LinearToSRGBConversion);
		}
		else
		{
			commandBuffer.DisableShaderKeyword(ShaderKeywordStrings.LinearToSRGBConversion);
		}
		Material material = (cameraData.isStereoEnabled ? null : m_BlitMaterial);
		commandBuffer.SetGlobalTexture("_BlitTex", m_Source.Identifier());
		if (cameraData.isStereoEnabled || isSceneViewCamera || cameraData.isDefaultViewport)
		{
			commandBuffer.SetRenderTarget(BuiltinRenderTextureType.CameraTarget, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.DontCare);
			commandBuffer.Blit(m_Source.Identifier(), dest, material);
		}
		else
		{
			SetRenderTarget(commandBuffer, dest, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, ClearFlag.None, Color.black, m_TargetDimension);
			Camera camera = cameraData.camera;
			commandBuffer.SetViewProjectionMatrices(Matrix4x4.identity, Matrix4x4.identity);
			commandBuffer.SetViewport(cameraData.pixelRect);
			commandBuffer.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, material);
			commandBuffer.SetViewProjectionMatrices(camera.worldToCameraMatrix, camera.projectionMatrix);
		}
		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
	}
}
