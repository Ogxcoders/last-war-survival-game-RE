using UnityEngine;

namespace RiverGame.Rendering.MaterialPropertyBlockUtilities;

public abstract class MaterialPropertyGroup
{
	public static int IDAllocator;

	public IMaterialProperty[] properties;

	private MaterialPropertyBlock m_MaterialPropertyBlock;

	public virtual int id { get; }

	public void ApplyToMaterialPropertyBlock(MaterialPropertyBlock mpb, bool useInstancingName = true)
	{
		if (properties != null)
		{
			IMaterialProperty[] array = properties;
			for (int i = 0; i < array.Length; i++)
			{
				array[i]?.ApplyToMaterialPropertyBlock(mpb, useInstancingName);
			}
		}
	}

	public void ApplyToMaterial(Material material)
	{
		if (properties != null)
		{
			IMaterialProperty[] array = properties;
			for (int i = 0; i < array.Length; i++)
			{
				array[i]?.ApplyToMaterial(material);
			}
		}
	}

	public MaterialPropertyBlock GetMaterialPropertyBlock(bool useInstancingName = true)
	{
		if (m_MaterialPropertyBlock == null)
		{
			m_MaterialPropertyBlock = new MaterialPropertyBlock();
			ApplyToMaterialPropertyBlock(m_MaterialPropertyBlock, useInstancingName);
		}
		return m_MaterialPropertyBlock;
	}
}
