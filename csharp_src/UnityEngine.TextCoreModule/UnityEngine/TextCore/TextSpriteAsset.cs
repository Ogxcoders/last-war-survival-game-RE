using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.TextCore;

[Serializable]
internal class TextSpriteAsset : ScriptableObject
{
	internal Dictionary<uint, int> m_UnicodeLookup;

	internal Dictionary<int, int> m_NameLookup;

	internal Dictionary<uint, int> m_GlyphIndexLookup;

	[SerializeField]
	private string m_Version;

	[SerializeField]
	private int m_HashCode;

	[SerializeField]
	public Texture spriteSheet;

	[SerializeField]
	private Material m_Material;

	[SerializeField]
	private int m_MaterialHashCode;

	[SerializeField]
	private List<SpriteCharacter> m_SpriteCharacterTable = new List<SpriteCharacter>();

	[SerializeField]
	private List<SpriteGlyph> m_SpriteGlyphTable = new List<SpriteGlyph>();

	[SerializeField]
	public List<TextSpriteAsset> fallbackSpriteAssets;

	internal bool m_IsSpriteAssetLookupTablesDirty = false;

	private static List<int> s_SearchedSpriteAssets;

	public string version
	{
		get
		{
			return m_Version;
		}
		set
		{
			m_Version = value;
		}
	}

	public int hashCode
	{
		get
		{
			return m_HashCode;
		}
		set
		{
			m_HashCode = value;
		}
	}

	public Material material
	{
		get
		{
			return m_Material;
		}
		set
		{
			m_Material = value;
		}
	}

	public int materialHashCode => m_MaterialHashCode;

	public List<SpriteCharacter> spriteCharacterTable
	{
		get
		{
			return m_SpriteCharacterTable;
		}
		internal set
		{
			m_SpriteCharacterTable = value;
		}
	}

	public List<SpriteGlyph> spriteGlyphTable
	{
		get
		{
			return m_SpriteGlyphTable;
		}
		internal set
		{
			m_SpriteGlyphTable = value;
		}
	}

	private void Awake()
	{
		m_HashCode = TextUtilities.GetHashCodeCaseInSensitive(base.name);
		if (m_Material != null)
		{
			m_MaterialHashCode = TextUtilities.GetHashCodeCaseInSensitive(m_Material.name);
		}
	}

	private Material GetDefaultSpriteMaterial()
	{
		ShaderUtilities.GetShaderPropertyIDs();
		Shader shader = Shader.Find("TextMeshPro/Sprite");
		Material material = new Material(shader);
		material.SetTexture(ShaderUtilities.ID_MainTex, spriteSheet);
		material.hideFlags = HideFlags.HideInHierarchy;
		return material;
	}

	public void UpdateLookupTables()
	{
		if (m_GlyphIndexLookup == null)
		{
			m_GlyphIndexLookup = new Dictionary<uint, int>();
		}
		else
		{
			m_GlyphIndexLookup.Clear();
		}
		for (int i = 0; i < m_SpriteGlyphTable.Count; i++)
		{
			uint index = m_SpriteGlyphTable[i].index;
			if (!m_GlyphIndexLookup.ContainsKey(index))
			{
				m_GlyphIndexLookup.Add(index, i);
			}
		}
		if (m_NameLookup == null)
		{
			m_NameLookup = new Dictionary<int, int>();
		}
		else
		{
			m_NameLookup.Clear();
		}
		if (m_UnicodeLookup == null)
		{
			m_UnicodeLookup = new Dictionary<uint, int>();
		}
		else
		{
			m_UnicodeLookup.Clear();
		}
		for (int j = 0; j < m_SpriteCharacterTable.Count; j++)
		{
			int key = m_SpriteCharacterTable[j].hashCode;
			if (!m_NameLookup.ContainsKey(key))
			{
				m_NameLookup.Add(key, j);
			}
			uint unicode = m_SpriteCharacterTable[j].unicode;
			if (!m_UnicodeLookup.ContainsKey(unicode))
			{
				m_UnicodeLookup.Add(unicode, j);
			}
			uint glyphIndex = m_SpriteCharacterTable[j].glyphIndex;
			if (m_GlyphIndexLookup.TryGetValue(glyphIndex, out var value))
			{
				m_SpriteCharacterTable[j].glyph = m_SpriteGlyphTable[value];
			}
		}
		m_IsSpriteAssetLookupTablesDirty = false;
	}

	public int GetSpriteIndexFromHashcode(int hashCode)
	{
		if (m_NameLookup == null)
		{
			UpdateLookupTables();
		}
		if (m_NameLookup.TryGetValue(hashCode, out var value))
		{
			return value;
		}
		return -1;
	}

	public int GetSpriteIndexFromUnicode(uint unicode)
	{
		if (m_UnicodeLookup == null)
		{
			UpdateLookupTables();
		}
		if (m_UnicodeLookup.TryGetValue(unicode, out var value))
		{
			return value;
		}
		return -1;
	}

	public int GetSpriteIndexFromName(string spriteName)
	{
		if (m_NameLookup == null)
		{
			UpdateLookupTables();
		}
		int hashCodeCaseInSensitive = TextUtilities.GetHashCodeCaseInSensitive(spriteName);
		return GetSpriteIndexFromHashcode(hashCodeCaseInSensitive);
	}

	public static TextSpriteAsset SearchForSpriteByUnicode(TextSpriteAsset spriteAsset, uint unicode, bool includeFallbacks, out int spriteIndex)
	{
		if (spriteAsset == null)
		{
			spriteIndex = -1;
			return null;
		}
		spriteIndex = spriteAsset.GetSpriteIndexFromUnicode(unicode);
		if (spriteIndex != -1)
		{
			return spriteAsset;
		}
		if (s_SearchedSpriteAssets == null)
		{
			s_SearchedSpriteAssets = new List<int>();
		}
		s_SearchedSpriteAssets.Clear();
		int instanceID = spriteAsset.GetInstanceID();
		s_SearchedSpriteAssets.Add(instanceID);
		if (includeFallbacks && spriteAsset.fallbackSpriteAssets != null && spriteAsset.fallbackSpriteAssets.Count > 0)
		{
			return SearchForSpriteByUnicodeInternal(spriteAsset.fallbackSpriteAssets, unicode, includeFallbacks, out spriteIndex);
		}
		if (includeFallbacks && TextSettings.defaultSpriteAsset != null)
		{
			return SearchForSpriteByUnicodeInternal(TextSettings.defaultSpriteAsset, unicode, includeFallbacks, out spriteIndex);
		}
		spriteIndex = -1;
		return null;
	}

	private static TextSpriteAsset SearchForSpriteByUnicodeInternal(List<TextSpriteAsset> spriteAssets, uint unicode, bool includeFallbacks, out int spriteIndex)
	{
		for (int i = 0; i < spriteAssets.Count; i++)
		{
			TextSpriteAsset textSpriteAsset = spriteAssets[i];
			if (textSpriteAsset == null)
			{
				continue;
			}
			int instanceID = textSpriteAsset.GetInstanceID();
			if (!s_SearchedSpriteAssets.Contains(instanceID))
			{
				s_SearchedSpriteAssets.Add(instanceID);
				textSpriteAsset = SearchForSpriteByUnicodeInternal(textSpriteAsset, unicode, includeFallbacks, out spriteIndex);
				if (textSpriteAsset != null)
				{
					return textSpriteAsset;
				}
			}
		}
		spriteIndex = -1;
		return null;
	}

	private static TextSpriteAsset SearchForSpriteByUnicodeInternal(TextSpriteAsset spriteAsset, uint unicode, bool includeFallbacks, out int spriteIndex)
	{
		spriteIndex = spriteAsset.GetSpriteIndexFromUnicode(unicode);
		if (spriteIndex != -1)
		{
			return spriteAsset;
		}
		if (includeFallbacks && spriteAsset.fallbackSpriteAssets != null && spriteAsset.fallbackSpriteAssets.Count > 0)
		{
			return SearchForSpriteByUnicodeInternal(spriteAsset.fallbackSpriteAssets, unicode, includeFallbacks, out spriteIndex);
		}
		spriteIndex = -1;
		return null;
	}

	public static TextSpriteAsset SearchForSpriteByHashCode(TextSpriteAsset spriteAsset, int hashCode, bool includeFallbacks, out int spriteIndex)
	{
		if (spriteAsset == null)
		{
			spriteIndex = -1;
			return null;
		}
		spriteIndex = spriteAsset.GetSpriteIndexFromHashcode(hashCode);
		if (spriteIndex != -1)
		{
			return spriteAsset;
		}
		if (s_SearchedSpriteAssets == null)
		{
			s_SearchedSpriteAssets = new List<int>();
		}
		s_SearchedSpriteAssets.Clear();
		int instanceID = spriteAsset.GetInstanceID();
		s_SearchedSpriteAssets.Add(instanceID);
		if (includeFallbacks && spriteAsset.fallbackSpriteAssets != null && spriteAsset.fallbackSpriteAssets.Count > 0)
		{
			return SearchForSpriteByHashCodeInternal(spriteAsset.fallbackSpriteAssets, hashCode, includeFallbacks, out spriteIndex);
		}
		if (includeFallbacks && TextSettings.defaultSpriteAsset != null)
		{
			return SearchForSpriteByHashCodeInternal(TextSettings.defaultSpriteAsset, hashCode, includeFallbacks, out spriteIndex);
		}
		spriteIndex = -1;
		return null;
	}

	private static TextSpriteAsset SearchForSpriteByHashCodeInternal(List<TextSpriteAsset> spriteAssets, int hashCode, bool searchFallbacks, out int spriteIndex)
	{
		for (int i = 0; i < spriteAssets.Count; i++)
		{
			TextSpriteAsset textSpriteAsset = spriteAssets[i];
			if (textSpriteAsset == null)
			{
				continue;
			}
			int instanceID = textSpriteAsset.GetInstanceID();
			if (!s_SearchedSpriteAssets.Contains(instanceID))
			{
				s_SearchedSpriteAssets.Add(instanceID);
				textSpriteAsset = SearchForSpriteByHashCodeInternal(textSpriteAsset, hashCode, searchFallbacks, out spriteIndex);
				if (textSpriteAsset != null)
				{
					return textSpriteAsset;
				}
			}
		}
		spriteIndex = -1;
		return null;
	}

	private static TextSpriteAsset SearchForSpriteByHashCodeInternal(TextSpriteAsset spriteAsset, int hashCode, bool searchFallbacks, out int spriteIndex)
	{
		spriteIndex = spriteAsset.GetSpriteIndexFromHashcode(hashCode);
		if (spriteIndex != -1)
		{
			return spriteAsset;
		}
		if (searchFallbacks && spriteAsset.fallbackSpriteAssets != null && spriteAsset.fallbackSpriteAssets.Count > 0)
		{
			return SearchForSpriteByHashCodeInternal(spriteAsset.fallbackSpriteAssets, hashCode, searchFallbacks, out spriteIndex);
		}
		spriteIndex = -1;
		return null;
	}

	public void SortGlyphTable()
	{
		if (m_SpriteGlyphTable != null && m_SpriteGlyphTable.Count != 0)
		{
			m_SpriteGlyphTable = m_SpriteGlyphTable.OrderBy((SpriteGlyph item) => item.index).ToList();
		}
	}

	internal void SortCharacterTable()
	{
		if (m_SpriteCharacterTable != null && m_SpriteCharacterTable.Count > 0)
		{
			m_SpriteCharacterTable = m_SpriteCharacterTable.OrderBy((SpriteCharacter c) => c.unicode).ToList();
		}
	}

	internal void SortGlyphAndCharacterTables()
	{
		SortGlyphTable();
		SortCharacterTable();
	}
}
