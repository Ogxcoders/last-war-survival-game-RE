using System;
using UnityEngine;

namespace FibMatrix.Rendering;

public class BoolMaterialPropertySettingItem : MaterialPropertySettingItem<bool>
{
	public override void Apply(Material material)
	{
		material.SetFloat(attribute, Convert.ToSingle(value));
	}

	public override void ApplyGlobal()
	{
		Shader.SetGlobalFloat(attribute, Convert.ToSingle(value));
	}
}
