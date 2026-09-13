using System.Collections.Generic;

namespace UnityEngine.TextCore;

internal static class FontUtilities
{
	private static List<int> s_SearchedFontAssets;

	internal static Character GetCharacterFromFontAsset(uint unicode, FontAsset sourceFontAsset, bool includeFallbacks, FontStyles fontStyle, FontWeight fontWeight, out bool isAlternativeTypeface, out FontAsset fontAsset)
	{
		if (includeFallbacks)
		{
			if (s_SearchedFontAssets == null)
			{
				s_SearchedFontAssets = new List<int>();
			}
			else
			{
				s_SearchedFontAssets.Clear();
			}
		}
		return GetCharacterFromFontAsset_Internal(unicode, sourceFontAsset, includeFallbacks, fontStyle, fontWeight, out isAlternativeTypeface, out fontAsset);
	}

	internal static Character GetCharacterFromFontAssets(uint unicode, List<FontAsset> fontAssets, bool includeFallbacks, FontStyles fontStyle, FontWeight fontWeight, out bool isAlternativeTypeface, out FontAsset fontAsset)
	{
		isAlternativeTypeface = false;
		if (fontAssets == null || fontAssets.Count == 0)
		{
			fontAsset = null;
			return null;
		}
		if (includeFallbacks)
		{
			if (s_SearchedFontAssets == null)
			{
				s_SearchedFontAssets = new List<int>();
			}
			else
			{
				s_SearchedFontAssets.Clear();
			}
		}
		int count = fontAssets.Count;
		for (int i = 0; i < count; i++)
		{
			if (!(fontAssets[i] == null))
			{
				Character characterFromFontAsset_Internal = GetCharacterFromFontAsset_Internal(unicode, fontAssets[i], includeFallbacks, fontStyle, fontWeight, out isAlternativeTypeface, out fontAsset);
				if (characterFromFontAsset_Internal != null)
				{
					return characterFromFontAsset_Internal;
				}
			}
		}
		fontAsset = null;
		return null;
	}

	private static Character GetCharacterFromFontAsset_Internal(uint unicode, FontAsset sourceFontAsset, bool includeFallbacks, FontStyles fontStyle, FontWeight fontWeight, out bool isAlternativeTypeface, out FontAsset fontAsset)
	{
		fontAsset = null;
		isAlternativeTypeface = false;
		Character value = null;
		bool flag = (fontStyle & FontStyles.Italic) == FontStyles.Italic;
		if (flag || fontWeight != FontWeight.Regular)
		{
			FontWeights[] fontWeightTable = sourceFontAsset.fontWeightTable;
			switch (fontWeight)
			{
			case FontWeight.Thin:
				fontAsset = (flag ? fontWeightTable[1].italicTypeface : fontWeightTable[1].regularTypeface);
				break;
			case FontWeight.ExtraLight:
				fontAsset = (flag ? fontWeightTable[2].italicTypeface : fontWeightTable[2].regularTypeface);
				break;
			case FontWeight.Light:
				fontAsset = (flag ? fontWeightTable[3].italicTypeface : fontWeightTable[3].regularTypeface);
				break;
			case FontWeight.Regular:
				fontAsset = (flag ? fontWeightTable[4].italicTypeface : fontWeightTable[4].regularTypeface);
				break;
			case FontWeight.Medium:
				fontAsset = (flag ? fontWeightTable[5].italicTypeface : fontWeightTable[5].regularTypeface);
				break;
			case FontWeight.SemiBold:
				fontAsset = (flag ? fontWeightTable[6].italicTypeface : fontWeightTable[6].regularTypeface);
				break;
			case FontWeight.Bold:
				fontAsset = (flag ? fontWeightTable[7].italicTypeface : fontWeightTable[7].regularTypeface);
				break;
			case FontWeight.Heavy:
				fontAsset = (flag ? fontWeightTable[8].italicTypeface : fontWeightTable[8].regularTypeface);
				break;
			case FontWeight.Black:
				fontAsset = (flag ? fontWeightTable[9].italicTypeface : fontWeightTable[9].regularTypeface);
				break;
			}
			if (fontAsset != null)
			{
				if (fontAsset.characterLookupTable.TryGetValue(unicode, out value))
				{
					isAlternativeTypeface = true;
					return value;
				}
				if (fontAsset.atlasPopulationMode == FontAsset.AtlasPopulationMode.Dynamic && fontAsset.TryAddCharacter(unicode, out value))
				{
					isAlternativeTypeface = true;
					return value;
				}
			}
		}
		if (sourceFontAsset.characterLookupTable.TryGetValue(unicode, out value))
		{
			fontAsset = sourceFontAsset;
			return value;
		}
		if (sourceFontAsset.atlasPopulationMode == FontAsset.AtlasPopulationMode.Dynamic && sourceFontAsset.TryAddCharacter(unicode, out value))
		{
			fontAsset = sourceFontAsset;
			return value;
		}
		if (value == null && includeFallbacks && sourceFontAsset.fallbackFontAssetTable != null)
		{
			List<FontAsset> fallbackFontAssetTable = sourceFontAsset.fallbackFontAssetTable;
			int count = fallbackFontAssetTable.Count;
			if (fallbackFontAssetTable != null && count > 0)
			{
				for (int i = 0; i < count; i++)
				{
					if (value != null)
					{
						break;
					}
					FontAsset fontAsset2 = fallbackFontAssetTable[i];
					if (fontAsset2 == null)
					{
						continue;
					}
					int instanceID = fontAsset2.GetInstanceID();
					if (!s_SearchedFontAssets.Contains(instanceID))
					{
						s_SearchedFontAssets.Add(instanceID);
						value = GetCharacterFromFontAsset_Internal(unicode, fontAsset2, includeFallbacks, fontStyle, fontWeight, out isAlternativeTypeface, out fontAsset);
						if (value != null)
						{
							return value;
						}
					}
				}
			}
		}
		return null;
	}
}
