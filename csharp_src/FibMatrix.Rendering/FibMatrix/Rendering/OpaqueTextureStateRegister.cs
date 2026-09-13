namespace FibMatrix.Rendering;

public class OpaqueTextureStateRegister : RenderStateRegister
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
