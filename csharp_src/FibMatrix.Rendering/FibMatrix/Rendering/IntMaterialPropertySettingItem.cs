using UnityEngine;

namespace FibMatrix.Rendering;

public class IntMaterialPropertySettingItem : MaterialPropertySettingItem<int>
{
	public override void Apply(Material material)
	{
		material.SetInt(attribute, value);
	}

	public override void ApplyGlobal()
	{
		Shader.SetGlobalInt(attribute, value);
	}
}
