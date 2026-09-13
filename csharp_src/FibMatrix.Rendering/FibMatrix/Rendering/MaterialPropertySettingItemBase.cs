using System;
using System.Collections.Generic;
using UnityEngine;

namespace FibMatrix.Rendering;

[Serializable]
public abstract class MaterialPropertySettingItemBase
{
	public virtual void Apply(List<Renderer> renderers, List<Material> materials)
	{
		if (renderers != null)
		{
			foreach (Renderer renderer in renderers)
			{
				if (renderer != null && renderer.sharedMaterial != null)
				{
					Apply(renderer.sharedMaterial);
				}
			}
		}
		if (materials == null)
		{
			return;
		}
		foreach (Material material in materials)
		{
			if (material != null)
			{
				Apply(material);
			}
		}
	}

	public abstract void Apply(Material material);
}
