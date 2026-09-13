using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering.Universal;

[MovedFrom("UnityEngine.Rendering.Internal.Universal")]
public class DrawSkyboxPass : ScriptableRenderPass
{
	public DrawSkyboxPass(RenderPassEvent evt)
	{
		base.renderPassEvent = evt;
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		context.DrawSkybox(renderingData.cameraData.camera);
	}
}
