namespace FibMatrix.Rendering;

public class DepthNormalTextureStateRegister : RenderStateRegister
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
