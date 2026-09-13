using UnityEngine;

namespace RiverGame.Rendering.MaterialPropertyBlockUtilities;

public abstract class MaterialProperty<T> : IMaterialProperty
{
	public T value;

	public int propertyNameId { get; }

	public int propertyInstancingNameId { get; }

	public MaterialProperty()
	{
	}

	public MaterialProperty(string propertyName, T value)
		: this(Shader.PropertyToID(propertyName), Shader.PropertyToID(propertyName + "Instancing"), value)
	{
	}

	public MaterialProperty(int propertyNameId, int propertyInstancingNameId, T value)
	{
		this.propertyNameId = propertyNameId;
		this.propertyInstancingNameId = propertyInstancingNameId;
		this.value = value;
	}

	public abstract void ApplyToMaterialPropertyBlock(MaterialPropertyBlock mpb, bool useInstancingName = true);

	public abstract void ApplyToMaterial(Material material);
}
