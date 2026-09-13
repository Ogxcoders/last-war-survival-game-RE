using UnityEngine;

namespace RiverGame.Rendering.MaterialPropertyBlockUtilities;

public interface IMaterialProperty
{
	void ApplyToMaterialPropertyBlock(MaterialPropertyBlock mpb, bool useInstancingName = true);

	void ApplyToMaterial(Material material);
}
