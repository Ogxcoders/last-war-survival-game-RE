namespace FibMatrix.Rendering;

public class DepthTextureStateRegister : RenderStateRegister
{
	public override void OnEnable()
	{
		RenderQualitySetting.UpdateMainCameraState();
	}

	public override void OnDisable()
	{
		RenderQualitySetting.UpdateMainCameraState();
	}
}
