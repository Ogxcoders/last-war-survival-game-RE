using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Bindings;
using UnityEngine.TextCore.LowLevel;

namespace UnityEngine.TextCore;

[Serializable]
[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
internal class FontAsset : ScriptableObject
{
	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal enum AtlasPopulationMode
	{
		Static,
		Dynamic
	}

	[SerializeField]
	private string m_Version = "1.1.0";

	[SerializeField]
	private int m_HashCode;

	[SerializeField]
	private FaceInfo m_FaceInfo;

	[SerializeField]
	internal string m_SourceFontFileGUID;

	[SerializeField]
	internal Font m_SourceFontFile_EditorRef;

	[SerializeField]
	internal Font m_SourceFontFile;

	[SerializeField]
	private AtlasPopulationMode m_AtlasPopulationMode;

	[SerializeField]
	private List<Glyph> m_GlyphTable = new List<Glyph>();

	private Dictionary<uint, Glyph> m_GlyphLookupDictionary;

	[SerializeField]
	private List<Character> m_CharacterTable = new List<Character>();

	private Dictionary<uint, Character> m_CharacterLookupDictionary;

	private Texture2D m_AtlasTexture;

	[SerializeField]
	private Texture2D[] m_AtlasTextures;

	[SerializeField]
	internal int m_AtlasTextureIndex;

	[SerializeField]
	private int m_AtlasWidth;

	[SerializeField]
	private int m_AtlasHeight;

	[SerializeField]
	private int m_AtlasPadding;

	[SerializeField]
	private GlyphRenderMode m_AtlasRenderMode;

	[SerializeField]
	private List<GlyphRect> m_UsedGlyphRects;

	[SerializeField]
	private List<GlyphRect> m_FreeGlyphRects;

	private List<uint> m_GlyphIndexes = new List<uint>();

	private Dictionary<uint, List<uint>> s_GlyphLookupMap = new Dictionary<uint, List<uint>>();

	[SerializeField]
	private Material m_Material;

	[SerializeField]
	internal int m_MaterialHashCode;

	[SerializeField]
	internal KerningTable m_KerningTable = new KerningTable();

	private Dictionary<int, KerningPair> m_KerningLookupDictionary;

	[SerializeField]
	internal KerningPair m_EmptyKerningPair;

	[SerializeField]
	internal List<FontAsset> m_FallbackFontAssetTable;

	[SerializeField]
	internal FontAssetCreationSettings m_FontAssetCreationSettings;

	[SerializeField]
	internal FontWeights[] m_FontWeightTable = new FontWeights[10];

	[SerializeField]
	private float m_RegularStyleWeight = 0f;

	[SerializeField]
	private float m_RegularStyleSpacing = 0f;

	[SerializeField]
	private float m_BoldStyleWeight = 0.75f;

	[SerializeField]
	private float m_BoldStyleSpacing = 7f;

	[SerializeField]
	private byte m_ItalicStyleSlant = 35;

	[SerializeField]
	private byte m_TabMultiple = 10;

	internal bool m_IsFontAssetLookupTablesDirty = false;

	private List<Glyph> m_GlyphsToPack = new List<Glyph>();

	private List<Glyph> m_GlyphsPacked = new List<Glyph>();

	private List<Glyph> m_GlyphsToRender = new List<Glyph>();

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

	public FaceInfo faceInfo
	{
		get
		{
			return m_FaceInfo;
		}
		set
		{
			m_FaceInfo = value;
		}
	}

	public Font sourceFontFile => m_SourceFontFile;

	public AtlasPopulationMode atlasPopulationMode
	{
		get
		{
			return m_AtlasPopulationMode;
		}
		set
		{
			m_AtlasPopulationMode = value;
		}
	}

	public List<Glyph> glyphTable
	{
		get
		{
			return m_GlyphTable;
		}
		set
		{
			m_GlyphTable = value;
		}
	}

	public Dictionary<uint, Glyph> glyphLookupTable
	{
		get
		{
			if (m_GlyphLookupDictionary == null)
			{
				ReadFontAssetDefinition();
			}
			return m_GlyphLookupDictionary;
		}
	}

	public List<Character> characterTable
	{
		get
		{
			return m_CharacterTable;
		}
		set
		{
			m_CharacterTable = value;
		}
	}

	public Dictionary<uint, Character> characterLookupTable
	{
		get
		{
			if (m_CharacterLookupDictionary == null)
			{
				ReadFontAssetDefinition();
			}
			return m_CharacterLookupDictionary;
		}
	}

	public Texture2D atlasTexture
	{
		get
		{
			if (m_AtlasTexture == null)
			{
				m_AtlasTexture = atlasTextures[0];
			}
			return m_AtlasTexture;
		}
	}

	public Texture2D[] atlasTextures
	{
		get
		{
			if (m_AtlasTextures == null)
			{
			}
			return m_AtlasTextures;
		}
		set
		{
			m_AtlasTextures = value;
		}
	}

	public int atlasWidth
	{
		get
		{
			return m_AtlasWidth;
		}
		set
		{
			m_AtlasWidth = value;
		}
	}

	public int atlasHeight
	{
		get
		{
			return m_AtlasHeight;
		}
		set
		{
			m_AtlasHeight = value;
		}
	}

	public int atlasPadding
	{
		get
		{
			return m_AtlasPadding;
		}
		set
		{
			m_AtlasPadding = value;
		}
	}

	public GlyphRenderMode atlasRenderMode
	{
		get
		{
			return m_AtlasRenderMode;
		}
		set
		{
			m_AtlasRenderMode = value;
		}
	}

	internal List<GlyphRect> usedGlyphRects
	{
		get
		{
			return m_UsedGlyphRects;
		}
		set
		{
			m_UsedGlyphRects = value;
		}
	}

	internal List<GlyphRect> freeGlyphRects
	{
		get
		{
			return m_FreeGlyphRects;
		}
		set
		{
			m_FreeGlyphRects = value;
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
			m_MaterialHashCode = TextUtilities.GetHashCodeCaseInSensitive(m_Material.name);
		}
	}

	public int materialHashCode
	{
		get
		{
			return m_MaterialHashCode;
		}
		set
		{
			if (m_MaterialHashCode == 0)
			{
				m_MaterialHashCode = TextUtilities.GetHashCodeCaseInSensitive(m_Material.name);
			}
			m_MaterialHashCode = value;
		}
	}

	public KerningTable kerningTable
	{
		get
		{
			return m_KerningTable;
		}
		set
		{
			m_KerningTable = value;
		}
	}

	public Dictionary<int, KerningPair> kerningLookupDictionary => m_KerningLookupDictionary;

	public List<FontAsset> fallbackFontAssetTable
	{
		get
		{
			return m_FallbackFontAssetTable;
		}
		set
		{
			m_FallbackFontAssetTable = value;
		}
	}

	public FontAssetCreationSettings fontAssetCreationSettings
	{
		get
		{
			return m_FontAssetCreationSettings;
		}
		set
		{
			m_FontAssetCreationSettings = value;
		}
	}

	public FontWeights[] fontWeightTable
	{
		get
		{
			return m_FontWeightTable;
		}
		set
		{
			m_FontWeightTable = value;
		}
	}

	public float regularStyleWeight
	{
		get
		{
			return m_RegularStyleWeight;
		}
		set
		{
			m_RegularStyleWeight = value;
		}
	}

	public float regularStyleSpacing
	{
		get
		{
			return m_RegularStyleSpacing;
		}
		set
		{
			m_RegularStyleSpacing = value;
		}
	}

	public float boldStyleWeight
	{
		get
		{
			return m_BoldStyleWeight;
		}
		set
		{
			m_BoldStyleWeight = value;
		}
	}

	public float boldStyleSpacing
	{
		get
		{
			return m_BoldStyleSpacing;
		}
		set
		{
			m_BoldStyleSpacing = value;
		}
	}

	public byte italicStyleSlant
	{
		get
		{
			return m_ItalicStyleSlant;
		}
		set
		{
			m_ItalicStyleSlant = value;
		}
	}

	public byte tabMultiple
	{
		get
		{
			return m_TabMultiple;
		}
		set
		{
			m_TabMultiple = value;
		}
	}

	public static FontAsset CreateFontAsset(Font font)
	{
		return CreateFontAsset(font, 90, 9, GlyphRenderMode.SDFAA, 1024, 1024);
	}

	public static FontAsset CreateFontAsset(Font font, int samplingPointSize, int atlasPadding, GlyphRenderMode renderMode, int atlasWidth, int atlasHeight, AtlasPopulationMode atlasPopulationMode = AtlasPopulationMode.Dynamic)
	{
		FontAsset fontAsset = ScriptableObject.CreateInstance<FontAsset>();
		FontEngine.InitializeFontEngine();
		FontEngine.LoadFontFace(font, samplingPointSize);
		fontAsset.faceInfo = FontEngine.GetFaceInfo();
		if (atlasPopulationMode == AtlasPopulationMode.Dynamic)
		{
			fontAsset.m_SourceFontFile = font;
		}
		fontAsset.atlasPopulationMode = atlasPopulationMode;
		fontAsset.atlasWidth = atlasWidth;
		fontAsset.atlasHeight = atlasHeight;
		fontAsset.atlasPadding = atlasPadding;
		fontAsset.atlasRenderMode = renderMode;
		fontAsset.atlasTextures = new Texture2D[1];
		Texture2D texture2D = new Texture2D(0, 0, TextureFormat.Alpha8, mipChain: false);
		fontAsset.atlasTextures[0] = texture2D;
		int num;
		if ((renderMode & (GlyphRenderMode)16) == (GlyphRenderMode)16)
		{
			num = 0;
			Material material = new Material(ShaderUtilities.ShaderRef_MobileBitmap);
			material.SetTexture(ShaderUtilities.ID_MainTex, texture2D);
			material.SetFloat(ShaderUtilities.ID_TextureWidth, atlasWidth);
			material.SetFloat(ShaderUtilities.ID_TextureHeight, atlasHeight);
			fontAsset.material = material;
		}
		else
		{
			num = 1;
			Material material2 = new Material(ShaderUtilities.ShaderRef_MobileSDF);
			material2.SetTexture(ShaderUtilities.ID_MainTex, texture2D);
			material2.SetFloat(ShaderUtilities.ID_TextureWidth, atlasWidth);
			material2.SetFloat(ShaderUtilities.ID_TextureHeight, atlasHeight);
			material2.SetFloat(ShaderUtilities.ID_GradientScale, atlasPadding + num);
			material2.SetFloat(ShaderUtilities.ID_WeightNormal, fontAsset.regularStyleWeight);
			material2.SetFloat(ShaderUtilities.ID_WeightBold, fontAsset.boldStyleWeight);
			fontAsset.material = material2;
		}
		fontAsset.freeGlyphRects = new List<GlyphRect>
		{
			new GlyphRect(0, 0, atlasWidth - num, atlasHeight - num)
		};
		fontAsset.usedGlyphRects = new List<GlyphRect>();
		fontAsset.InitializeDictionaryLookupTables();
		return fontAsset;
	}

	private void Awake()
	{
		m_HashCode = TextUtilities.GetHashCodeCaseInSensitive(base.name);
		if (m_Material != null)
		{
			m_MaterialHashCode = TextUtilities.GetHashCodeCaseInSensitive(m_Material.name);
		}
	}

	internal void InitializeDictionaryLookupTables()
	{
		if (m_GlyphLookupDictionary == null)
		{
			m_GlyphLookupDictionary = new Dictionary<uint, Glyph>();
		}
		else
		{
			m_GlyphLookupDictionary.Clear();
		}
		for (int i = 0; i < m_GlyphTable.Count; i++)
		{
			Glyph glyph = m_GlyphTable[i];
			uint index = glyph.index;
			if (!m_GlyphLookupDictionary.ContainsKey(index))
			{
				m_GlyphLookupDictionary.Add(index, glyph);
			}
		}
		if (m_CharacterLookupDictionary == null)
		{
			m_CharacterLookupDictionary = new Dictionary<uint, Character>();
		}
		else
		{
			m_CharacterLookupDictionary.Clear();
		}
		for (int j = 0; j < m_CharacterTable.Count; j++)
		{
			Character character = m_CharacterTable[j];
			uint unicode = character.unicode;
			if (!m_CharacterLookupDictionary.ContainsKey(unicode))
			{
				m_CharacterLookupDictionary.Add(unicode, character);
			}
			if (m_GlyphLookupDictionary.ContainsKey(character.glyphIndex))
			{
				character.glyph = m_GlyphLookupDictionary[character.glyphIndex];
			}
		}
		if (m_KerningLookupDictionary == null)
		{
			m_KerningLookupDictionary = new Dictionary<int, KerningPair>();
		}
		else
		{
			m_KerningLookupDictionary.Clear();
		}
		List<KerningPair> kerningPairs = m_KerningTable.kerningPairs;
		if (kerningPairs == null)
		{
			return;
		}
		for (int k = 0; k < kerningPairs.Count; k++)
		{
			KerningPair kerningPair = kerningPairs[k];
			KerningPairKey kerningPairKey = new KerningPairKey(kerningPair.firstGlyph, kerningPair.secondGlyph);
			if (!m_KerningLookupDictionary.ContainsKey((int)kerningPairKey.key))
			{
				m_KerningLookupDictionary.Add((int)kerningPairKey.key, kerningPair);
			}
			else if (!TextSettings.warningsDisabled)
			{
				Debug.LogWarning("Kerning Key for [" + kerningPairKey.ascii_Left + "] and [" + kerningPairKey.ascii_Right + "] already exists.");
			}
		}
	}

	internal void ReadFontAssetDefinition()
	{
		InitializeDictionaryLookupTables();
		if (!m_CharacterLookupDictionary.ContainsKey(9u))
		{
			Glyph glyph = new Glyph(0u, new GlyphMetrics(0f, 0f, 0f, 0f, m_FaceInfo.tabWidth * (float)(int)tabMultiple), GlyphRect.zero, 1f, 0);
			m_CharacterLookupDictionary.Add(9u, new Character(9u, glyph));
		}
		if (!m_CharacterLookupDictionary.ContainsKey(10u))
		{
			Glyph glyph2 = new Glyph(0u, new GlyphMetrics(10f, 0f, 0f, 0f, 0f), GlyphRect.zero, 1f, 0);
			m_CharacterLookupDictionary.Add(10u, new Character(10u, glyph2));
			if (!m_CharacterLookupDictionary.ContainsKey(13u))
			{
				m_CharacterLookupDictionary.Add(13u, new Character(13u, glyph2));
			}
		}
		if (!m_CharacterLookupDictionary.ContainsKey(8203u))
		{
			Glyph glyph3 = new Glyph(0u, new GlyphMetrics(0f, 0f, 0f, 0f, 0f), GlyphRect.zero, 1f, 0);
			m_CharacterLookupDictionary.Add(8203u, new Character(8203u, glyph3));
		}
		if (!m_CharacterLookupDictionary.ContainsKey(8288u))
		{
			Glyph glyph4 = new Glyph(0u, new GlyphMetrics(0f, 0f, 0f, 0f, 0f), GlyphRect.zero, 1f, 0);
			m_CharacterLookupDictionary.Add(8288u, new Character(8288u, glyph4));
		}
		if (m_FaceInfo.capLine == 0f && m_CharacterLookupDictionary.ContainsKey(72u))
		{
			m_FaceInfo.capLine = m_CharacterLookupDictionary[72u].glyph.metrics.horizontalBearingY;
		}
		if (m_FaceInfo.scale == 0f)
		{
			m_FaceInfo.scale = 1f;
		}
		if (m_FaceInfo.strikethroughOffset == 0f)
		{
			m_FaceInfo.strikethroughOffset = m_FaceInfo.capLine / 2.5f;
		}
		if (m_AtlasPadding == 0 && material.HasProperty(ShaderUtilities.ID_GradientScale))
		{
			m_AtlasPadding = (int)material.GetFloat(ShaderUtilities.ID_GradientScale) - 1;
		}
		m_HashCode = TextUtilities.GetHashCodeCaseInSensitive(base.name);
		m_MaterialHashCode = TextUtilities.GetHashCodeCaseInSensitive(material.name);
	}

	internal void SortCharacterTable()
	{
		if (m_CharacterTable != null && m_CharacterTable.Count > 0)
		{
			m_CharacterTable = m_CharacterTable.OrderBy((Character c) => c.unicode).ToList();
		}
	}

	internal void SortGlyphTable()
	{
		if (m_GlyphTable != null && m_GlyphTable.Count > 0)
		{
			m_GlyphTable = m_GlyphTable.OrderBy((Glyph c) => c.index).ToList();
		}
	}

	internal void SortGlyphAndCharacterTables()
	{
		SortGlyphTable();
		SortCharacterTable();
	}

	internal bool HasCharacter(int character)
	{
		if (m_CharacterLookupDictionary == null)
		{
			return false;
		}
		if (m_CharacterLookupDictionary.ContainsKey((uint)character))
		{
			return true;
		}
		return false;
	}

	internal bool HasCharacter(char character)
	{
		if (m_CharacterLookupDictionary == null)
		{
			return false;
		}
		if (m_CharacterLookupDictionary.ContainsKey(character))
		{
			return true;
		}
		return false;
	}

	internal bool HasCharacter(char character, bool searchFallbacks)
	{
		if (m_CharacterLookupDictionary == null)
		{
			ReadFontAssetDefinition();
			if (m_CharacterLookupDictionary == null)
			{
				return false;
			}
		}
		if (m_CharacterLookupDictionary.ContainsKey(character))
		{
			return true;
		}
		if (searchFallbacks)
		{
			if (fallbackFontAssetTable != null && fallbackFontAssetTable.Count > 0)
			{
				for (int i = 0; i < fallbackFontAssetTable.Count && fallbackFontAssetTable[i] != null; i++)
				{
					if (fallbackFontAssetTable[i].HasCharacter_Internal(character, searchFallbacks))
					{
						return true;
					}
				}
			}
			if (TextSettings.fallbackFontAssets != null && TextSettings.fallbackFontAssets.Count > 0)
			{
				for (int j = 0; j < TextSettings.fallbackFontAssets.Count && TextSettings.fallbackFontAssets[j] != null; j++)
				{
					if (TextSettings.fallbackFontAssets[j].m_CharacterLookupDictionary == null)
					{
						TextSettings.fallbackFontAssets[j].ReadFontAssetDefinition();
					}
					if (TextSettings.fallbackFontAssets[j].m_CharacterLookupDictionary != null && TextSettings.fallbackFontAssets[j].HasCharacter_Internal(character, searchFallbacks))
					{
						return true;
					}
				}
			}
			if (TextSettings.defaultFontAsset != null)
			{
				if (TextSettings.defaultFontAsset.m_CharacterLookupDictionary == null)
				{
					TextSettings.defaultFontAsset.ReadFontAssetDefinition();
				}
				if (TextSettings.defaultFontAsset.m_CharacterLookupDictionary != null && TextSettings.defaultFontAsset.HasCharacter_Internal(character, searchFallbacks))
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool HasCharacter_Internal(char character, bool searchFallbacks)
	{
		if (m_CharacterLookupDictionary == null)
		{
			ReadFontAssetDefinition();
			if (m_CharacterLookupDictionary == null)
			{
				return false;
			}
		}
		if (m_CharacterLookupDictionary.ContainsKey(character))
		{
			return true;
		}
		if (searchFallbacks && fallbackFontAssetTable != null && fallbackFontAssetTable.Count > 0)
		{
			for (int i = 0; i < fallbackFontAssetTable.Count && fallbackFontAssetTable[i] != null; i++)
			{
				if (fallbackFontAssetTable[i].HasCharacter_Internal(character, searchFallbacks))
				{
					return true;
				}
			}
		}
		return false;
	}

	internal bool HasCharacters(string text, out List<char> missingCharacters)
	{
		if (m_CharacterLookupDictionary == null)
		{
			missingCharacters = null;
			return false;
		}
		missingCharacters = new List<char>();
		for (int i = 0; i < text.Length; i++)
		{
			if (!m_CharacterLookupDictionary.ContainsKey(text[i]))
			{
				missingCharacters.Add(text[i]);
			}
		}
		if (missingCharacters.Count == 0)
		{
			return true;
		}
		return false;
	}

	internal bool HasCharacters(string text)
	{
		if (m_CharacterLookupDictionary == null)
		{
			return false;
		}
		for (int i = 0; i < text.Length; i++)
		{
			if (!m_CharacterLookupDictionary.ContainsKey(text[i]))
			{
				return false;
			}
		}
		return true;
	}

	internal static string GetCharacters(FontAsset fontAsset)
	{
		string text = string.Empty;
		for (int i = 0; i < fontAsset.characterTable.Count; i++)
		{
			text += (char)fontAsset.characterTable[i].unicode;
		}
		return text;
	}

	internal static int[] GetCharactersArray(FontAsset fontAsset)
	{
		int[] array = new int[fontAsset.characterTable.Count];
		for (int i = 0; i < fontAsset.characterTable.Count; i++)
		{
			array[i] = (int)fontAsset.characterTable[i].unicode;
		}
		return array;
	}

	internal Character AddCharacter_Internal(uint unicode, Glyph glyph)
	{
		if (m_CharacterLookupDictionary.ContainsKey(unicode))
		{
			return m_CharacterLookupDictionary[unicode];
		}
		uint index = glyph.index;
		if (!m_GlyphLookupDictionary.ContainsKey(index))
		{
			if (glyph.glyphRect.width == 0 || glyph.glyphRect.width == 0)
			{
				m_GlyphTable.Add(glyph);
			}
			else
			{
				if (!FontEngine.TryPackGlyphInAtlas(glyph, m_AtlasPadding, GlyphPackingMode.ContactPointRule, m_AtlasRenderMode, m_AtlasWidth, m_AtlasHeight, m_FreeGlyphRects, m_UsedGlyphRects))
				{
					return null;
				}
				m_GlyphsToRender.Add(glyph);
			}
		}
		Character character = new Character(unicode, glyph);
		m_CharacterTable.Add(character);
		m_CharacterLookupDictionary.Add(unicode, character);
		UpdateAtlasTexture();
		return character;
	}

	internal bool TryAddCharacter(uint unicode, out Character character)
	{
		if (m_CharacterLookupDictionary.ContainsKey(unicode))
		{
			character = m_CharacterLookupDictionary[unicode];
			return true;
		}
		character = null;
		if (FontEngine.LoadFontFace(sourceFontFile, m_FaceInfo.pointSize) != FontEngineError.Success)
		{
			return false;
		}
		uint glyphIndex = FontEngine.GetGlyphIndex(unicode);
		if (glyphIndex == 0)
		{
			return false;
		}
		if (m_GlyphLookupDictionary.ContainsKey(glyphIndex))
		{
			character = new Character(unicode, m_GlyphLookupDictionary[glyphIndex]);
			m_CharacterTable.Add(character);
			m_CharacterLookupDictionary.Add(unicode, character);
			return true;
		}
		if (m_AtlasTextures[m_AtlasTextureIndex].width == 0 || m_AtlasTextures[m_AtlasTextureIndex].height == 0)
		{
			m_AtlasTextures[m_AtlasTextureIndex].Resize(m_AtlasWidth, m_AtlasHeight);
			FontEngine.ResetAtlasTexture(m_AtlasTextures[m_AtlasTextureIndex]);
		}
		if (FontEngine.TryAddGlyphToTexture(glyphIndex, m_AtlasPadding, GlyphPackingMode.BestShortSideFit, m_FreeGlyphRects, m_UsedGlyphRects, m_AtlasRenderMode, m_AtlasTextures[m_AtlasTextureIndex], out var glyph))
		{
			m_GlyphTable.Add(glyph);
			m_GlyphLookupDictionary.Add(glyphIndex, glyph);
			character = new Character(unicode, glyph);
			m_CharacterTable.Add(character);
			m_CharacterLookupDictionary.Add(unicode, character);
			return true;
		}
		return false;
	}

	internal void UpdateAtlasTexture()
	{
		if (m_GlyphsToRender.Count != 0)
		{
			FontEngine.RenderGlyphsToTexture(m_GlyphsToRender, m_AtlasPadding, m_AtlasRenderMode, m_AtlasTextures[m_AtlasTextureIndex]);
			m_AtlasTextures[m_AtlasTextureIndex].Apply(updateMipmaps: false, makeNoLongerReadable: false);
			for (int i = 0; i < m_GlyphsToRender.Count; i++)
			{
				Glyph glyph = m_GlyphsToRender[i];
				glyph.atlasIndex = m_AtlasTextureIndex;
				m_GlyphTable.Add(glyph);
				m_GlyphLookupDictionary.Add(glyph.index, glyph);
			}
			m_GlyphsPacked.Clear();
			m_GlyphsToRender.Clear();
			if (m_GlyphsToPack.Count <= 0)
			{
			}
		}
	}

	public bool TryAddCharacters(uint[] unicodes)
	{
		bool flag = false;
		m_GlyphIndexes.Clear();
		s_GlyphLookupMap.Clear();
		FontEngine.LoadFontFace(m_SourceFontFile, m_FaceInfo.pointSize);
		foreach (uint num in unicodes)
		{
			if (!m_CharacterLookupDictionary.ContainsKey(num))
			{
				uint glyphIndex = FontEngine.GetGlyphIndex(num);
				if (glyphIndex == 0)
				{
					flag = true;
				}
				else if (m_GlyphLookupDictionary.ContainsKey(glyphIndex))
				{
					Character character = new Character(num, m_GlyphLookupDictionary[glyphIndex]);
					m_CharacterTable.Add(character);
					m_CharacterLookupDictionary.Add(num, character);
				}
				else if (s_GlyphLookupMap.ContainsKey(glyphIndex))
				{
					s_GlyphLookupMap[glyphIndex].Add(num);
				}
				else
				{
					s_GlyphLookupMap.Add(glyphIndex, new List<uint> { num });
					m_GlyphIndexes.Add(glyphIndex);
				}
			}
		}
		if (m_GlyphIndexes == null || m_GlyphIndexes.Count == 0)
		{
			return true;
		}
		if (m_AtlasTextures[m_AtlasTextureIndex].width == 0 || m_AtlasTextures[m_AtlasTextureIndex].height == 0)
		{
			m_AtlasTextures[m_AtlasTextureIndex].Resize(m_AtlasWidth, m_AtlasHeight);
			FontEngine.ResetAtlasTexture(m_AtlasTextures[m_AtlasTextureIndex]);
		}
		Glyph[] glyphs;
		bool flag2 = FontEngine.TryAddGlyphsToTexture(m_GlyphIndexes, m_AtlasPadding, GlyphPackingMode.BestShortSideFit, m_FreeGlyphRects, m_UsedGlyphRects, m_AtlasRenderMode, m_AtlasTextures[m_AtlasTextureIndex], out glyphs);
		for (int j = 0; j < glyphs.Length && glyphs[j] != null; j++)
		{
			Glyph glyph = glyphs[j];
			uint index = glyph.index;
			m_GlyphTable.Add(glyph);
			m_GlyphLookupDictionary.Add(index, glyph);
			foreach (uint item in s_GlyphLookupMap[index])
			{
				Character character2 = new Character(item, glyph);
				m_CharacterTable.Add(character2);
				m_CharacterLookupDictionary.Add(item, character2);
			}
		}
		return flag2 && !flag;
	}

	public bool TryAddCharacters(string characters)
	{
		if (string.IsNullOrEmpty(characters) || m_AtlasPopulationMode == AtlasPopulationMode.Static)
		{
			if (m_AtlasPopulationMode == AtlasPopulationMode.Static)
			{
				Debug.LogWarning("Unable to add characters to font asset [" + base.name + "] because its AtlasPopulationMode is set to Static.", this);
			}
			else
			{
				Debug.LogWarning("Unable to add characters to font asset [" + base.name + "] because the provided character list is Null or Empty.", this);
			}
			return false;
		}
		if (FontEngine.LoadFontFace(m_SourceFontFile, m_FaceInfo.pointSize) != FontEngineError.Success)
		{
			return false;
		}
		bool flag = false;
		int length = characters.Length;
		m_GlyphIndexes.Clear();
		s_GlyphLookupMap.Clear();
		for (int i = 0; i < length; i++)
		{
			uint num = characters[i];
			if (m_CharacterLookupDictionary.ContainsKey(num))
			{
				continue;
			}
			uint glyphIndex = FontEngine.GetGlyphIndex(num);
			if (glyphIndex == 0)
			{
				flag = true;
			}
			else if (m_GlyphLookupDictionary.ContainsKey(glyphIndex))
			{
				Character character = new Character(num, m_GlyphLookupDictionary[glyphIndex]);
				m_CharacterTable.Add(character);
				m_CharacterLookupDictionary.Add(num, character);
			}
			else if (s_GlyphLookupMap.ContainsKey(glyphIndex))
			{
				if (!s_GlyphLookupMap[glyphIndex].Contains(num))
				{
					s_GlyphLookupMap[glyphIndex].Add(num);
				}
			}
			else
			{
				s_GlyphLookupMap.Add(glyphIndex, new List<uint> { num });
				m_GlyphIndexes.Add(glyphIndex);
			}
		}
		if (m_GlyphIndexes == null || m_GlyphIndexes.Count == 0)
		{
			Debug.LogWarning("No characters will be added to font asset [" + base.name + "] either because they are already present in the font asset or missing from the font file.");
			return true;
		}
		if (m_AtlasTextures[m_AtlasTextureIndex].width == 0 || m_AtlasTextures[m_AtlasTextureIndex].height == 0)
		{
			m_AtlasTextures[m_AtlasTextureIndex].Resize(m_AtlasWidth, m_AtlasHeight);
			FontEngine.ResetAtlasTexture(m_AtlasTextures[m_AtlasTextureIndex]);
		}
		Glyph[] glyphs;
		bool flag2 = FontEngine.TryAddGlyphsToTexture(m_GlyphIndexes, m_AtlasPadding, GlyphPackingMode.BestShortSideFit, m_FreeGlyphRects, m_UsedGlyphRects, m_AtlasRenderMode, m_AtlasTextures[m_AtlasTextureIndex], out glyphs);
		for (int j = 0; j < glyphs.Length && glyphs[j] != null; j++)
		{
			Glyph glyph = glyphs[j];
			uint index = glyph.index;
			m_GlyphTable.Add(glyph);
			m_GlyphLookupDictionary.Add(index, glyph);
			List<uint> list = s_GlyphLookupMap[index];
			int count = list.Count;
			for (int k = 0; k < count; k++)
			{
				uint num2 = list[k];
				Character character2 = new Character(num2, glyph);
				m_CharacterTable.Add(character2);
				m_CharacterLookupDictionary.Add(num2, character2);
			}
		}
		return flag2 && !flag;
	}

	internal void ClearFontAssetData()
	{
		if (m_GlyphTable != null)
		{
			m_GlyphTable.Clear();
		}
		if (m_CharacterTable != null)
		{
			m_CharacterTable.Clear();
		}
		if (m_UsedGlyphRects != null)
		{
			m_UsedGlyphRects.Clear();
		}
		if (m_FreeGlyphRects != null)
		{
			int num = (((m_AtlasRenderMode & (GlyphRenderMode)16) != (GlyphRenderMode)16) ? 1 : 0);
			m_FreeGlyphRects = new List<GlyphRect>
			{
				new GlyphRect(0, 0, m_AtlasWidth - num, m_AtlasHeight - num)
			};
		}
		if (m_GlyphsToPack != null)
		{
			m_GlyphsToPack.Clear();
		}
		if (m_GlyphsPacked != null)
		{
			m_GlyphsPacked.Clear();
		}
		if (m_KerningTable != null && m_KerningTable.kerningPairs != null)
		{
			m_KerningTable.kerningPairs.Clear();
		}
		m_AtlasTextureIndex = 0;
		if (m_AtlasTextures != null)
		{
			for (int i = 0; i < m_AtlasTextures.Length; i++)
			{
				Texture2D texture2D = m_AtlasTextures[i];
				if (!(texture2D == null))
				{
					if (texture2D.width != m_AtlasWidth || texture2D.height != m_AtlasHeight)
					{
						texture2D.Resize(m_AtlasWidth, m_AtlasHeight, TextureFormat.Alpha8, hasMipMap: false);
					}
					FontEngine.ResetAtlasTexture(texture2D);
					texture2D.Apply();
					if (i == 0)
					{
						m_AtlasTexture = texture2D;
					}
					m_AtlasTextures[i] = texture2D;
				}
			}
		}
		ReadFontAssetDefinition();
	}
}
