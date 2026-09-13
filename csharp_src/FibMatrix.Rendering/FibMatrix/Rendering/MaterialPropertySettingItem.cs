using System;
using System.Collections.Generic;
using UnityEngine;

namespace FibMatrix.Rendering;

[Serializable]
public abstract class MaterialPropertySettingItem<T> : MaterialPropertySettingItemBase
{
	public string attribute;

	public T value;

	public bool local;

	public override void Apply(List<Renderer> renderers, List<Material> materials)
	{
		if (attribute != null && !string.IsNullOrEmpty(attribute.Trim()))
		{
			if (local)
			{
				base.Apply(renderers, materials);
			}
			else
			{
				ApplyGlobal();
			}
		}
	}

	public abstract void ApplyGlobal();
}
