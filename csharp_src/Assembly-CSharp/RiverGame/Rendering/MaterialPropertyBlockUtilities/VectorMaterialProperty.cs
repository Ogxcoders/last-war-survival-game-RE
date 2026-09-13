using System.Runtime.CompilerServices;
using UnityEngine;

namespace RiverGame.Rendering.MaterialPropertyBlockUtilities;

public class VectorMaterialProperty : MaterialProperty<Vector4>
{
	public VectorMaterialProperty(string propertyName, Vector4 value)
		: base(propertyName, value)
	{
	}

	public VectorMaterialProperty(int propertyNameId, int propertyInstancingNameId, Vector4 value)
		: base(propertyNameId, propertyInstancingNameId, value)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void ApplyToMaterialPropertyBlock(MaterialPropertyBlock mpb, bool useInstancingName = true)
	{
		mpb.SetVector(useInstancingName ? base.propertyInstancingNameId : base.propertyNameId, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void ApplyToMaterial(Material material)
	{
		material.SetVector(base.propertyNameId, value);
	}
}
