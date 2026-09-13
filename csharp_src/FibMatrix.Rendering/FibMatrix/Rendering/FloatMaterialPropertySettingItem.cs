using UnityEngine;

namespace FibMatrix.Rendering;

public class FloatMaterialPropertySettingItem : MaterialPropertySettingItem<float>
{
	public override void Apply(Material material)
	{
		material.SetFloat(attribute, value);
	}

	public override void ApplyGlobal()
	{
		Shader.SetGlobalFloat(attribute, value);
	}
}
