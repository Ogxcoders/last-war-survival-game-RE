using System.Collections.Generic;

namespace UnityEngine.TextCore;

internal class MaterialReferenceManager
{
	private static MaterialReferenceManager s_Instance;

	private Dictionary<int, Material> m_FontMaterialReferenceLookup = new Dictionary<int, Material>();

	private Dictionary<int, FontAsset> m_FontAssetReferenceLookup = new Dictionary<int, FontAsset>();

	private Dictionary<int, TextSpriteAsset> m_SpriteAssetReferenceLookup = new Dictionary<int, TextSpriteAsset>();

	private Dictionary<int, TextGradientPreset> m_ColorGradientReferenceLookup = new Dictionary<int, TextGradientPreset>();

	public static MaterialReferenceManager instance
	{
		get
		{
			if (s_Instance == null)
			{
				s_Instance = new MaterialReferenceManager();
			}
			return s_Instance;
		}
	}

	public static void AddFontAsset(FontAsset fontAsset)
	{
		instance.AddFontAssetInternal(fontAsset);
	}

	private void AddFontAssetInternal(FontAsset fontAsset)
	{
		if (!m_FontAssetReferenceLookup.ContainsKey(fontAsset.hashCode))
		{
			m_FontAssetReferenceLookup.Add(fontAsset.hashCode, fontAsset);
			m_FontMaterialReferenceLookup.Add(fontAsset.materialHashCode, fontAsset.material);
		}
	}

	public static void AddSpriteAsset(TextSpriteAsset spriteAsset)
	{
		instance.AddSpriteAssetInternal(spriteAsset);
	}

	private void AddSpriteAssetInternal(TextSpriteAsset spriteAsset)
	{
		if (!m_SpriteAssetReferenceLookup.ContainsKey(spriteAsset.hashCode))
		{
			m_SpriteAssetReferenceLookup.Add(spriteAsset.hashCode, spriteAsset);
			m_FontMaterialReferenceLookup.Add(spriteAsset.hashCode, spriteAsset.material);
		}
	}

	public static void AddSpriteAsset(int hashCode, TextSpriteAsset spriteAsset)
	{
		instance.AddSpriteAssetInternal(hashCode, spriteAsset);
	}

	private void AddSpriteAssetInternal(int hashCode, TextSpriteAsset spriteAsset)
	{
		if (!m_SpriteAssetReferenceLookup.ContainsKey(hashCode))
		{
			m_SpriteAssetReferenceLookup.Add(hashCode, spriteAsset);
			m_FontMaterialReferenceLookup.Add(hashCode, spriteAsset.material);
			if (spriteAsset.hashCode == 0)
			{
				spriteAsset.hashCode = hashCode;
			}
		}
	}

	public static void AddFontMaterial(int hashCode, Material material)
	{
		instance.AddFontMaterialInternal(hashCode, material);
	}

	private void AddFontMaterialInternal(int hashCode, Material material)
	{
		m_FontMaterialReferenceLookup.Add(hashCode, material);
	}

	public static void AddColorGradientPreset(int hashCode, TextGradientPreset spriteAsset)
	{
		instance.AddColorGradientPreset_Internal(hashCode, spriteAsset);
	}

	private void AddColorGradientPreset_Internal(int hashCode, TextGradientPreset spriteAsset)
	{
		if (!m_ColorGradientReferenceLookup.ContainsKey(hashCode))
		{
			m_ColorGradientReferenceLookup.Add(hashCode, spriteAsset);
		}
	}

	public bool Contains(FontAsset font)
	{
		if (m_FontAssetReferenceLookup.ContainsKey(font.hashCode))
		{
			return true;
		}
		return false;
	}

	public bool Contains(TextSpriteAsset sprite)
	{
		if (m_FontAssetReferenceLookup.ContainsKey(sprite.hashCode))
		{
			return true;
		}
		return false;
	}

	public static bool TryGetFontAsset(int hashCode, out FontAsset fontAsset)
	{
		return instance.TryGetFontAssetInternal(hashCode, out fontAsset);
	}

	private bool TryGetFontAssetInternal(int hashCode, out FontAsset fontAsset)
	{
		fontAsset = null;
		if (m_FontAssetReferenceLookup.TryGetValue(hashCode, out fontAsset))
		{
			return true;
		}
		return false;
	}

	public static bool TryGetSpriteAsset(int hashCode, out TextSpriteAsset spriteAsset)
	{
		return instance.TryGetSpriteAssetInternal(hashCode, out spriteAsset);
	}

	private bool TryGetSpriteAssetInternal(int hashCode, out TextSpriteAsset spriteAsset)
	{
		spriteAsset = null;
		if (m_SpriteAssetReferenceLookup.TryGetValue(hashCode, out spriteAsset))
		{
			return true;
		}
		return false;
	}

	public static bool TryGetColorGradientPreset(int hashCode, out TextGradientPreset gradientPreset)
	{
		return instance.TryGetColorGradientPresetInternal(hashCode, out gradientPreset);
	}

	private bool TryGetColorGradientPresetInternal(int hashCode, out TextGradientPreset gradientPreset)
	{
		gradientPreset = null;
		if (m_ColorGradientReferenceLookup.TryGetValue(hashCode, out gradientPreset))
		{
			return true;
		}
		return false;
	}

	public static bool TryGetMaterial(int hashCode, out Material material)
	{
		return instance.TryGetMaterialInternal(hashCode, out material);
	}

	private bool TryGetMaterialInternal(int hashCode, out Material material)
	{
		material = null;
		if (m_FontMaterialReferenceLookup.TryGetValue(hashCode, out material))
		{
			return true;
		}
		return false;
	}
}
