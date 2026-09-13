namespace UnityEngine.Rendering.Universal;

internal class InvokeOnRenderObjectCallbackPass : ScriptableRenderPass
{
	public InvokeOnRenderObjectCallbackPass(RenderPassEvent evt)
	{
		base.renderPassEvent = evt;
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		context.InvokeOnRenderObjectCallback();
	}
}
