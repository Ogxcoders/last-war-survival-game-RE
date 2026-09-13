using System.Runtime.CompilerServices;
using UnityEngine;

namespace RiverGame.Rendering.MaterialPropertyBlockUtilities;

public class ColorMaterialProperty : MaterialProperty<Color>
{
	public ColorMaterialProperty(string propertyName, Color value)
		: base(propertyName, value)
	{
	}

	public ColorMaterialProperty(int propertyNameId, int propertyInstancingNameId, Color value)
		: base(propertyNameId, propertyInstancingNameId, value)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void ApplyToMaterialPropertyBlock(MaterialPropertyBlock mpb, bool useInstancingName = true)
	{
		mpb.SetColor(useInstancingName ? base.propertyInstancingNameId : base.propertyNameId, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void ApplyToMaterial(Material material)
	{
		material.SetColor(base.propertyNameId, value);
	}
}
