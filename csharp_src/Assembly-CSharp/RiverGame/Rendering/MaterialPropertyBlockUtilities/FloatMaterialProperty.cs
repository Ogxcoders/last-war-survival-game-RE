using System.Runtime.CompilerServices;
using UnityEngine;

namespace RiverGame.Rendering.MaterialPropertyBlockUtilities;

public class FloatMaterialProperty : MaterialProperty<float>
{
	public FloatMaterialProperty(string propertyName, float value)
		: base(propertyName, value)
	{
	}

	public FloatMaterialProperty(int propertyNameId, int propertyInstancingNameId, float value)
		: base(propertyNameId, propertyInstancingNameId, value)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void ApplyToMaterialPropertyBlock(MaterialPropertyBlock mpb, bool useInstancingName = true)
	{
		mpb.SetFloat(useInstancingName ? base.propertyInstancingNameId : base.propertyNameId, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void ApplyToMaterial(Material material)
	{
		material.SetFloat(base.propertyNameId, value);
	}
}
