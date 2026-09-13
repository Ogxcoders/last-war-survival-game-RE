using System.Collections.Generic;
using UnityEngine;

namespace TMPro;

public class TMP_ResourceManager
{
	private static readonly TMP_ResourceManager s_instance;

	private static TMP_Settings s_TextSettings;

	private static readonly List<TMP_FontAsset> s_FontAssetReferences;

	private static readonly Dictionary<int, TMP_FontAsset> s_FontAssetReferenceLookup;

	static TMP_ResourceManager()
	{
		s_instance = new TMP_ResourceManager();
		s_FontAssetReferences = new List<TMP_FontAsset>();
		s_FontAssetReferenceLookup = new Dictionary<int, TMP_FontAsset>();
	}

	internal static TMP_Settings GetTextSettings()
	{
		if (s_TextSettings == null)
		{
			s_TextSettings = Resources.Load<TMP_Settings>("TextSettings");
		}
		return s_TextSettings;
	}

	public static void AddFontAsset(TMP_FontAsset fontAsset)
	{
		int hashCode = fontAsset.hashCode;
		if (!s_FontAssetReferenceLookup.ContainsKey(hashCode))
		{
			s_FontAssetReferences.Add(fontAsset);
			s_FontAssetReferenceLookup.Add(hashCode, fontAsset);
		}
	}

	public static bool TryGetFontAsset(int hashcode, out TMP_FontAsset fontAsset)
	{
		fontAsset = null;
		return s_FontAssetReferenceLookup.TryGetValue(hashcode, out fontAsset);
	}

	internal static void RebuildFontAssetCache(int instanceID)
	{
		for (int i = 0; i < s_FontAssetReferences.Count; i++)
		{
			TMP_FontAsset tMP_FontAsset = s_FontAssetReferences[i];
			if (tMP_FontAsset.FallbackSearchQueryLookup.Contains(instanceID))
			{
				tMP_FontAsset.ReadFontAssetDefinition();
			}
		}
	}
}
