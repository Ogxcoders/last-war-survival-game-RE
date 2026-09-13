namespace FibMatrix.Rendering;

public class RenderTextureSetting : QualitySettingGroup
{
	public RenderTextureType type;

	public override void OnDisable()
	{
		RenderStateRegisterCenter.DepthTextureStateRegister.Register(this, require: false);
		RenderStateRegisterCenter.DepthNormalTextureStateRegister.Register(this, require: false);
		RenderStateRegisterCenter.OpaqueTextureStateRegister.Register(this, require: false);
		base.OnDisable();
	}

	public override void Switch(EnQualityLevel level)
	{
		bool require = base[level];
		switch (type)
		{
		case RenderTextureType.Depth:
			RenderStateRegisterCenter.DepthTextureStateRegister.Register(this, require);
			break;
		case RenderTextureType.DepthNormal:
			RenderStateRegisterCenter.DepthNormalTextureStateRegister.Register(this, require);
			break;
		case RenderTextureType.Opaque:
			RenderStateRegisterCenter.OpaqueTextureStateRegister.Register(this, require);
			break;
		}
		base.Switch(level);
	}
}
