using System;
using System.Collections.Generic;

namespace UnityEngine.UI;

internal static class MaterialCache
{
	private static readonly Dictionary<Hash128, MaterialEntry> s_MaterialMap = new Dictionary<Hash128, MaterialEntry>();

	public static Material Register(Material material, Hash128 hash, Action<Material> onModify)
	{
		if (!hash.isValid)
		{
			return null;
		}
		if (!s_MaterialMap.TryGetValue(hash, out var value))
		{
			value = new MaterialEntry
			{
				material = new Material(material)
				{
					hideFlags = HideFlags.HideAndDontSave
				}
			};
			onModify(value.material);
			s_MaterialMap.Add(hash, value);
		}
		value.referenceCount++;
		return value.material;
	}

	public static void Unregister(Hash128 hash)
	{
		if (hash.isValid && s_MaterialMap.TryGetValue(hash, out var value) && --value.referenceCount <= 0)
		{
			value.Release();
			s_MaterialMap.Remove(hash);
		}
	}
}
