using UnityEngine;

namespace FibMatrix.Rendering;

public class TextureMaterialPropertySettingItem : MaterialPropertySettingItem<Texture>
{
	public override void Apply(Material material)
	{
		material.SetTexture(attribute, value);
	}

	public override void ApplyGlobal()
	{
		Shader.SetGlobalTexture(attribute, value);
	}
}
