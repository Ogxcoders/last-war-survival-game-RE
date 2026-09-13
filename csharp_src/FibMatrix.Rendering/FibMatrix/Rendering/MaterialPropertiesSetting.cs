using System.Collections.Generic;
using UnityEngine;

namespace FibMatrix.Rendering;

public class MaterialPropertiesSetting : QualitySettingGroup
{
	[SerializeReference]
	public List<MaterialPropertySettingItemBase> attributes;

	public List<Renderer> renderers;

	public List<Material> materials;

	public override void OnBeforeSerialize()
	{
		if (attributes == null)
		{
			attributes = new List<MaterialPropertySettingItemBase>();
		}
		base.OnBeforeSerialize();
	}

	public override void Switch(EnQualityLevel level)
	{
		if (base[level] && attributes != null)
		{
			foreach (MaterialPropertySettingItemBase attribute in attributes)
			{
				attribute?.Apply(renderers, materials);
			}
		}
		base.Switch(level);
	}
}
