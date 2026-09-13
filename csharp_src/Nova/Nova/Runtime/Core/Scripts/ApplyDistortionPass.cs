using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Nova.Runtime.Core.Scripts;

public sealed class ApplyDistortionPass : ScriptableRenderPass
{
	private const string RenderPassName = "ApplyDistortionPass";

	private const string ProfilingSamplerName = "SrcToDest";

	private readonly bool _applyToSceneView;

	private readonly int _distortionBufferPropertyId = Shader.PropertyToID("_ScreenSpaceUvTexture");

	private readonly int _mainTexPropertyId = Shader.PropertyToID("_MainTex");

	private readonly Material _material;

	private readonly ProfilingSampler _profilingSampler;

	private ScriptableRenderer _renderer;

	private RenderTargetIdentifier _distortedUvBufferIdentifier;

	private RenderTargetHandle _tempRenderTargetHandle;

	private bool _distortInPostFx;

	public ApplyDistortionPass(bool applyToSceneView, Shader shader)
	{
		_applyToSceneView = applyToSceneView;
		_profilingSampler = new ProfilingSampler("SrcToDest");
		_tempRenderTargetHandle.Init("_TempRT");
		_material = CoreUtils.CreateEngineMaterial(shader);
		base.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
	}

	public void Setup(ScriptableRenderer renderer, RenderTargetIdentifier distortedUvBufferIdentifier, bool distortInPostFx)
	{
		_renderer = renderer;
		_distortedUvBufferIdentifier = distortedUvBufferIdentifier;
		_distortInPostFx = distortInPostFx;
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		if (!_applyToSceneView && renderingData.cameraData.cameraType == CameraType.SceneView)
		{
			return;
		}
		if (_distortInPostFx)
		{
			CommandBuffer commandBuffer = CommandBufferPool.Get("ApplyDistortionPass");
			commandBuffer.Clear();
			commandBuffer.SetGlobalTexture(_distortionBufferPropertyId, _distortedUvBufferIdentifier);
			CoreUtils.SetKeyword(commandBuffer, ShaderKeywordStrings.FX_Distortion, state: true);
			context.ExecuteCommandBuffer(commandBuffer);
			CommandBufferPool.Release(commandBuffer);
		}
		else if (!(_material == null))
		{
			CommandBuffer commandBuffer2 = CommandBufferPool.Get("ApplyDistortionPass");
			commandBuffer2.Clear();
			RenderTargetIdentifier cameraColorTarget = _renderer.cameraColorTarget;
			RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
			cameraTargetDescriptor.depthBufferBits = 0;
			commandBuffer2.GetTemporaryRT(_tempRenderTargetHandle.id, cameraTargetDescriptor);
			using (new ProfilingScope(commandBuffer2, _profilingSampler))
			{
				commandBuffer2.SetGlobalTexture(_mainTexPropertyId, cameraColorTarget);
				commandBuffer2.SetGlobalTexture(_distortionBufferPropertyId, _distortedUvBufferIdentifier);
				Blit(commandBuffer2, cameraColorTarget, _tempRenderTargetHandle.Identifier(), _material);
			}
			Blit(commandBuffer2, _tempRenderTargetHandle.Identifier(), cameraColorTarget);
			commandBuffer2.ReleaseTemporaryRT(_tempRenderTargetHandle.id);
			context.ExecuteCommandBuffer(commandBuffer2);
			CommandBufferPool.Release(commandBuffer2);
		}
	}

	public override void FrameCleanup(CommandBuffer cmd)
	{
		CoreUtils.SetKeyword(cmd, ShaderKeywordStrings.FX_Distortion, state: false);
	}
}
