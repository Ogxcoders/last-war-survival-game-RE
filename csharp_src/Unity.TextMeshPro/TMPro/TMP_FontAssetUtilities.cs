using System.Collections.Generic;

namespace TMPro;

public class TMP_FontAssetUtilities
{
	private static readonly TMP_FontAssetUtilities s_Instance;

	private static HashSet<int> k_SearchedAssets;

	private static bool k_IsFontEngineInitialized;

	public static TMP_FontAssetUtilities instance => s_Instance;

	static TMP_FontAssetUtilities()
	{
		s_Instance = new TMP_FontAssetUtilities();
	}

	public static TMP_Character GetCharacterFromFontAsset(uint unicode, TMP_FontAsset sourceFontAsset, bool includeFallbacks, FontStyles fontStyle, FontWeight fontWeight, out bool isAlternativeTypeface)
	{
		if (includeFallbacks)
		{
			if (k_SearchedAssets == null)
			{
				k_SearchedAssets = new HashSet<int>();
			}
			else
			{
				k_SearchedAssets.Clear();
			}
		}
		return GetCharacterFromFontAsset_Internal(unicode, sourceFontAsset, includeFallbacks, fontStyle, fontWeight, out isAlternativeTypeface);
	}

	private static TMP_Character GetCharacterFromFontAsset_Internal(uint unicode, TMP_FontAsset sourceFontAsset, bool includeFallbacks, FontStyles fontStyle, FontWeight fontWeight, out bool isAlternativeTypeface)
	{
		isAlternativeTypeface = false;
		TMP_Character value = null;
		bool flag = (fontStyle & FontStyles.Italic) == FontStyles.Italic;
		if (flag || fontWeight != FontWeight.Regular)
		{
			TMP_FontWeightPair[] fontWeightTable = sourceFontAsset.fontWeightTable;
			int num = 4;
			switch (fontWeight)
			{
			case FontWeight.Thin:
				num = 1;
				break;
			case FontWeight.ExtraLight:
				num = 2;
				break;
			case FontWeight.Light:
				num = 3;
				break;
			case FontWeight.Regular:
				num = 4;
				break;
			case FontWeight.Medium:
				num = 5;
				break;
			case FontWeight.SemiBold:
				num = 6;
				break;
			case FontWeight.Bold:
				num = 7;
				break;
			case FontWeight.Heavy:
				num = 8;
				break;
			case FontWeight.Black:
				num = 9;
				break;
			}
			TMP_FontAsset tMP_FontAsset = (flag ? fontWeightTable[num].italicTypeface : fontWeightTable[num].regularTypeface);
			if (tMP_FontAsset != null)
			{
				if (tMP_FontAsset.characterLookupTable.TryGetValue(unicode, out value))
				{
					isAlternativeTypeface = true;
					return value;
				}
				if (tMP_FontAsset.atlasPopulationMode == AtlasPopulationMode.Dynamic && tMP_FontAsset.TryAddCharacterInternal(unicode, out value))
				{
					isAlternativeTypeface = true;
					return value;
				}
			}
		}
		if (sourceFontAsset.characterLookupTable.TryGetValue(unicode, out value))
		{
			return value;
		}
		if (sourceFontAsset.atlasPopulationMode == AtlasPopulationMode.Dynamic && sourceFontAsset.TryAddCharacterInternal(unicode, out value))
		{
			return value;
		}
		if (value == null && includeFallbacks && sourceFontAsset.fallbackFontAssetTable != null)
		{
			List<TMP_FontAsset> fallbackFontAssetTable = sourceFontAsset.fallbackFontAssetTable;
			int count = fallbackFontAssetTable.Count;
			if (count == 0)
			{
				return null;
			}
			for (int i = 0; i < count; i++)
			{
				TMP_FontAsset tMP_FontAsset2 = fallbackFontAssetTable[i];
				if (tMP_FontAsset2 == null)
				{
					continue;
				}
				int instanceID = tMP_FontAsset2.instanceID;
				if (k_SearchedAssets.Add(instanceID))
				{
					sourceFontAsset.FallbackSearchQueryLookup.Add(instanceID);
					value = GetCharacterFromFontAsset_Internal(unicode, tMP_FontAsset2, includeFallbacks: true, fontStyle, fontWeight, out isAlternativeTypeface);
					if (value != null)
					{
						return value;
					}
				}
			}
		}
		return null;
	}

	public static TMP_Character GetCharacterFromFontAssets(uint unicode, TMP_FontAsset sourceFontAsset, List<TMP_FontAsset> fontAssets, bool includeFallbacks, FontStyles fontStyle, FontWeight fontWeight, out bool isAlternativeTypeface)
	{
		isAlternativeTypeface = false;
		if (fontAssets == null || fontAssets.Count == 0)
		{
			return null;
		}
		if (includeFallbacks)
		{
			if (k_SearchedAssets == null)
			{
				k_SearchedAssets = new HashSet<int>();
			}
			else
			{
				k_SearchedAssets.Clear();
			}
		}
		int count = fontAssets.Count;
		for (int i = 0; i < count; i++)
		{
			TMP_FontAsset tMP_FontAsset = fontAssets[i];
			if (!(tMP_FontAsset == null))
			{
				sourceFontAsset.FallbackSearchQueryLookup.Add(tMP_FontAsset.instanceID);
				TMP_Character characterFromFontAsset_Internal = GetCharacterFromFontAsset_Internal(unicode, tMP_FontAsset, includeFallbacks, fontStyle, fontWeight, out isAlternativeTypeface);
				if (characterFromFontAsset_Internal != null)
				{
					return characterFromFontAsset_Internal;
				}
			}
		}
		return null;
	}

	public static TMP_SpriteCharacter GetSpriteCharacterFromSpriteAsset(uint unicode, TMP_SpriteAsset spriteAsset, bool includeFallbacks)
	{
		if (spriteAsset == null)
		{
			return null;
		}
		if (spriteAsset.spriteCharacterLookupTable.TryGetValue(unicode, out var value))
		{
			return value;
		}
		if (includeFallbacks)
		{
			if (k_SearchedAssets == null)
			{
				k_SearchedAssets = new HashSet<int>();
			}
			else
			{
				k_SearchedAssets.Clear();
			}
			k_SearchedAssets.Add(spriteAsset.instanceID);
			List<TMP_SpriteAsset> fallbackSpriteAssets = spriteAsset.fallbackSpriteAssets;
			if (fallbackSpriteAssets != null && fallbackSpriteAssets.Count > 0)
			{
				int count = fallbackSpriteAssets.Count;
				for (int i = 0; i < count; i++)
				{
					TMP_SpriteAsset tMP_SpriteAsset = fallbackSpriteAssets[i];
					if (tMP_SpriteAsset == null)
					{
						continue;
					}
					int instanceID = tMP_SpriteAsset.instanceID;
					if (k_SearchedAssets.Add(instanceID))
					{
						value = GetSpriteCharacterFromSpriteAsset_Internal(unicode, tMP_SpriteAsset, includeFallbacks: true);
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

	private static TMP_SpriteCharacter GetSpriteCharacterFromSpriteAsset_Internal(uint unicode, TMP_SpriteAsset spriteAsset, bool includeFallbacks)
	{
		if (spriteAsset.spriteCharacterLookupTable.TryGetValue(unicode, out var value))
		{
			return value;
		}
		if (includeFallbacks)
		{
			List<TMP_SpriteAsset> fallbackSpriteAssets = spriteAsset.fallbackSpriteAssets;
			if (fallbackSpriteAssets != null && fallbackSpriteAssets.Count > 0)
			{
				int count = fallbackSpriteAssets.Count;
				for (int i = 0; i < count; i++)
				{
					TMP_SpriteAsset tMP_SpriteAsset = fallbackSpriteAssets[i];
					if (tMP_SpriteAsset == null)
					{
						continue;
					}
					int instanceID = tMP_SpriteAsset.instanceID;
					if (k_SearchedAssets.Add(instanceID))
					{
						value = GetSpriteCharacterFromSpriteAsset_Internal(unicode, tMP_SpriteAsset, includeFallbacks: true);
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
