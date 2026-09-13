using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FibMatrix.PerfTools;

public class OverdrawRendererFeature
{
	private BeginPass _BeginPass;

	private OverdrawPass _OpaquePass;

	private OverdrawPass _TransparentPass;

	private EndPass _EndPass;

	private Shader _OpaqueShader;

	private Shader _TransparentShader;

	public void Create()
	{
		_OpaqueShader = Shader.Find("Hidden/PerfTools/OverdrawMonitor/URP/OverdrawOpaque");
		_TransparentShader = Shader.Find("Hidden/PerfTools/OverdrawMonitor/URP/OverdrawTransparent");
		if (!_OpaqueShader || !_TransparentShader)
		{
			Debug.LogError("shader not found");
			return;
		}
		_BeginPass = new BeginPass("Overdraw Monitor Begin");
		_BeginPass.renderPassEvent = RenderPassEvent.AfterRenderingPrePasses;
		_OpaquePass = new OverdrawPass("Overdraw Monitor Opaque", RenderQueueRange.opaque, _OpaqueShader, isOpaque: true);
		_OpaquePass.renderPassEvent = RenderPassEvent.AfterRenderingPrePasses;
		_TransparentPass = new OverdrawPass("Overdraw Monitor Transparent", RenderQueueRange.transparent, _TransparentShader, isOpaque: false);
		_TransparentPass.renderPassEvent = RenderPassEvent.AfterRenderingPrePasses;
		_EndPass = new EndPass("Overdraw Monitor End");
		_EndPass.renderPassEvent = RenderPassEvent.AfterRenderingPrePasses;
	}

	public void setRenderTarget(RenderTargetIdentifier renderTarget)
	{
		_EndPass?.setRenderTarget(renderTarget);
	}

	public void setOutputTarget(RenderTargetIdentifier outputTarget)
	{
		_EndPass?.setOutputTarget(outputTarget);
	}

	public void AddRenderPasses(ScriptableRenderer renderer)
	{
		renderer.EnqueuePass(_BeginPass);
		renderer.EnqueuePass(_OpaquePass);
		renderer.EnqueuePass(_TransparentPass);
		renderer.EnqueuePass(_EndPass);
	}
}
