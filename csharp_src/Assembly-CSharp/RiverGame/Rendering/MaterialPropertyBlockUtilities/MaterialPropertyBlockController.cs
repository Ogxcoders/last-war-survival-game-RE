using System.Collections.Generic;
using UnityEngine;

namespace RiverGame.Rendering.MaterialPropertyBlockUtilities;

public class MaterialPropertyBlockController
{
	private ulong tag;

	private static Dictionary<ulong, MaterialPropertyBlock> Tag2MPB = new Dictionary<ulong, MaterialPropertyBlock>();

	private List<MaterialPropertyGroup> materialPropertyGroups { get; } = new List<MaterialPropertyGroup>();

	public MaterialPropertyBlock Add(MaterialPropertyGroup properties)
	{
		if (properties == null)
		{
			return null;
		}
		if (!materialPropertyGroups.Contains(properties))
		{
			materialPropertyGroups.Add(properties);
		}
		tag |= (ulong)(1L << properties.id);
		return RefreshMaterialPropertyBlock();
	}

	public MaterialPropertyBlock Remove(MaterialPropertyGroup properties, bool emptyAsNull = false)
	{
		materialPropertyGroups.Remove(properties);
		tag = 0uL;
		foreach (MaterialPropertyGroup materialPropertyGroup in materialPropertyGroups)
		{
			tag |= (ulong)(1L << materialPropertyGroup.id);
		}
		return RefreshMaterialPropertyBlock();
	}

	public MaterialPropertyBlock RefreshMaterialPropertyBlock()
	{
		if (!Tag2MPB.TryGetValue(tag, out var value))
		{
			value = new MaterialPropertyBlock();
			Tag2MPB.Add(tag, value);
		}
		value.Clear();
		foreach (MaterialPropertyGroup materialPropertyGroup in materialPropertyGroups)
		{
			materialPropertyGroup.ApplyToMaterialPropertyBlock(value);
		}
		return value;
	}
}
