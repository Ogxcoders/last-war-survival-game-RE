using UnityEngine;

namespace FibMatrix.Rendering;

public class Vector4MaterialPropertySettingItem : MaterialPropertySettingItem<Vector4>
{
	public override void Apply(Material material)
	{
		material.SetVector(attribute, value);
	}

	public override void ApplyGlobal()
	{
		Shader.SetGlobalVector(attribute, value);
	}
}
