using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Nova.Runtime.Core.Scripts;

[Serializable]
public sealed class ScreenSpaceDistortion : ScriptableRendererFeature
{
	private const string DistortionLightMode = "DistortedUvBuffer";

	[SerializeField]
	private bool _applyToSceneView = true;

	[SerializeField]
	[Tooltip("在后期中进行扭曲合成，可提高性能；否则在后期前blit2次；但都受后期画面分级控制是否执行扭曲")]
	private bool _distortInPostFx = true;

	[SerializeField]
	[Tooltip("有深度图时，是否用它遮挡的扭曲物体；无深度图时此项无效；用深度图遮挡时无法缩小扭曲RT提升性能")]
	private bool _tryUseDepthTest;

	[SerializeField]
	[Range(0.1f, 1f)]
	private float _rtScale = 0.5f;

	[SerializeField]
	[HideInInspector]
	private Shader _applyDistortionShader;

	[SerializeField]
	[Tooltip("使用扭曲计数脚本，当计数大于0时才执行扭曲渲染流程")]
	private bool _useDistortionCounter;

	private ApplyDistortionPass _applyDistortionPass;

	private DistortedUvBufferPass _distortedUvBufferPass;

	public override void Create()
	{
		_applyDistortionShader = Shader.Find("Hidden/Nova/Particles/ApplyDistortion");
		if (!(_applyDistortionShader == null))
		{
			_distortedUvBufferPass = new DistortedUvBufferPass("DistortedUvBuffer");
			_applyDistortionPass = new ApplyDistortionPass(_applyToSceneView, _applyDistortionShader);
		}
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (_applyDistortionShader == null || renderingData.cameraData.renderType != CameraRenderType.Base || !renderingData.postProcessingData.enableFxDistortion || (_useDistortionCounter && NovaDistortionCounter.Count <= 0))
		{
			return;
		}
		RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
		RenderTextureFormat format = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf) ? RenderTextureFormat.RGHalf : RenderTextureFormat.DefaultHDR);
		bool num = _tryUseDepthTest && renderingData.cameraData.requiresDepthTexture;
		int num2 = cameraTargetDescriptor.width;
		int num3 = cameraTargetDescriptor.height;
		if (!num)
		{
			num2 = (int)((float)num2 * _rtScale);
			num3 = (int)((float)num3 * _rtScale);
		}
		RenderTexture temporary = RenderTexture.GetTemporary(num2, num3, 0, format);
		RenderTargetIdentifier renderTargetIdentifier = new RenderTargetIdentifier(temporary);
		Func<RenderTargetIdentifier> getCameraDepthTargetIdentifier = null;
		if (num)
		{
			getCameraDepthTargetIdentifier = () => renderer.cameraDepth;
		}
		_distortedUvBufferPass.Setup(renderTargetIdentifier, getCameraDepthTargetIdentifier);
		_applyDistortionPass.Setup(renderer, renderTargetIdentifier, _distortInPostFx);
		renderer.EnqueuePass(_distortedUvBufferPass);
		renderer.EnqueuePass(_applyDistortionPass);
		RenderTexture.ReleaseTemporary(temporary);
	}
}
