namespace FibMatrix.Rendering;

public class PostProcessStateRegister : RenderStateRegister
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
