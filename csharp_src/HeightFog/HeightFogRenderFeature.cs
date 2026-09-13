using UnityEngine.Rendering.Universal;

public class HeightFogRenderFeature : ScriptableRendererFeature
{
	public HeightFogSettings fogSetting = new HeightFogSettings();

	private HeightFogPass pass;

	public override void Create()
	{
		pass = new HeightFogPass(RenderPassEvent.BeforeRenderingPostProcessing);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		pass.renderPassEvent = fogSetting.renderPassEvent;
		pass.Setup(renderer.cameraColorTarget, fogSetting);
		renderer.EnqueuePass(pass);
	}
}
