using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TextCore;
using UnityEngine.TextCore.LowLevel;

namespace TMPro;

[Serializable]
[ExcludeFromPreset]
public class TMP_FontAsset : TMP_Asset
{
	[SerializeField]
	private string m_Version;

	[SerializeField]
	internal string m_SourceFontFileGUID;

	[SerializeField]
	private Font m_SourceFontFile;

	[SerializeField]
	private AtlasPopulationMode m_AtlasPopulationMode;

	private bool m_loadGlyphFromSystemFonts;

	private string[] m_fallbackSystemFonts;

	[SerializeField]
	internal FaceInfo m_FaceInfo;

	[SerializeField]
	internal List<Glyph> m_GlyphTable = new List<Glyph>();

	internal Dictionary<uint, Glyph> m_GlyphLookupDictionary;

	[SerializeField]
	internal List<TMP_Character> m_CharacterTable = new List<TMP_Character>();

	internal Dictionary<uint, TMP_Character> m_CharacterLookupDictionary;

	internal Texture2D m_AtlasTexture;

	[SerializeField]
	internal Texture2D[] m_AtlasTextures;

	[SerializeField]
	internal int m_AtlasTextureIndex;

	[SerializeField]
	private bool m_IsMultiAtlasTexturesEnabled;

	[SerializeField]
	private bool m_ClearDynamicDataOnBuild;

	[SerializeField]
	private List<GlyphRect> m_UsedGlyphRects;

	[SerializeField]
	private List<GlyphRect> m_FreeGlyphRects;

	[SerializeField]
	private FaceInfo_Legacy m_fontInfo;

	[SerializeField]
	public Texture2D atlas;

	[SerializeField]
	internal int m_AtlasWidth;

	[SerializeField]
	internal int m_AtlasHeight;

	[SerializeField]
	internal int m_AtlasPadding;

	[SerializeField]
	internal GlyphRenderMode m_AtlasRenderMode;

	[SerializeField]
	internal List<TMP_Glyph> m_glyphInfoList;

	[SerializeField]
	[FormerlySerializedAs("m_kerningInfo")]
	internal KerningTable m_KerningTable = new KerningTable();

	[SerializeField]
	internal TMP_FontFeatureTable m_FontFeatureTable = new TMP_FontFeatureTable();

	[SerializeField]
	private List<TMP_FontAsset> fallbackFontAssets;

	[SerializeField]
	internal List<TMP_FontAsset> m_FallbackFontAssetTable;

	[SerializeField]
	internal FontAssetCreationSettings m_CreationSettings;

	[SerializeField]
	private TMP_FontWeightPair[] m_FontWeightTable = new TMP_FontWeightPair[10];

	[SerializeField]
	private TMP_FontWeightPair[] fontWeights;

	public float normalStyle;

	public float normalSpacingOffset;

	public float boldStyle = 0.75f;

	public float boldSpacing = 7f;

	public byte italicStyle = 35;

	public byte tabSize = 10;

	internal bool IsFontAssetLookupTablesDirty;

	private static ProfilerMarker k_ReadFontAssetDefinitionMarker = new ProfilerMarker("TMP.ReadFontAssetDefinition");

	private static ProfilerMarker k_AddSynthesizedCharactersMarker = new ProfilerMarker("TMP.AddSynthesizedCharacters");

	private static ProfilerMarker k_TryAddCharacterMarker = new ProfilerMarker("TMP.TryAddCharacter");

	private static ProfilerMarker k_TryAddCharactersMarker = new ProfilerMarker("TMP.TryAddCharacters");

	private static ProfilerMarker k_TryAddCharacterFromSystemFontMarker = new ProfilerMarker("TMP.TryAddCharacterFromSystemFont");

	private static ProfilerMarker k_UpdateGlyphAdjustmentRecordsMarker = new ProfilerMarker("TMP.UpdateGlyphAdjustmentRecords");

	private static ProfilerMarker k_ClearFontAssetDataMarker = new ProfilerMarker("TMP.ClearFontAssetData");

	private static ProfilerMarker k_UpdateFontAssetDataMarker = new ProfilerMarker("TMP.UpdateFontAssetData");

	private static string s_DefaultMaterialSuffix = " Atlas Material";

	internal HashSet<int> FallbackSearchQueryLookup = new HashSet<int>();

	private static HashSet<int> k_SearchedFontAssetLookup;

	private static List<TMP_FontAsset> k_FontAssets_FontFeaturesUpdateQueue = new List<TMP_FontAsset>();

	private static HashSet<int> k_FontAssets_FontFeaturesUpdateQueueLookup = new HashSet<int>();

	private static List<TMP_FontAsset> k_FontAssets_AtlasTexturesUpdateQueue = new List<TMP_FontAsset>();

	private static HashSet<int> k_FontAssets_AtlasTexturesUpdateQueueLookup = new HashSet<int>();

	private List<Glyph> m_GlyphsToRender = new List<Glyph>();

	private List<Glyph> m_GlyphsRendered = new List<Glyph>();

	private List<uint> m_GlyphIndexList = new List<uint>();

	private List<uint> m_GlyphIndexListNewlyAdded = new List<uint>();

	internal List<uint> m_GlyphsToAdd = new List<uint>();

	internal HashSet<uint> m_GlyphsToAddLookup = new HashSet<uint>();

	internal List<TMP_Character> m_CharactersToAdd = new List<TMP_Character>();

	internal HashSet<uint> m_CharactersToAddLookup = new HashSet<uint>();

	internal List<uint> s_MissingCharacterList = new List<uint>();

	internal HashSet<uint> m_MissingUnicodesFromFontFile = new HashSet<uint>();

	internal static uint[] k_GlyphIndexArray;

	private static string[] s_SystemFontNames = null;

	private static HashSet<uint> s_MissingUnicodesFromSystemFont = new HashSet<uint>();

	private static Dictionary<uint, string> s_UnicodeAndSystemFontMap = new Dictionary<uint, string>();

	public string version
	{
		get
		{
			return m_Version;
		}
		internal set
		{
			m_Version = value;
		}
	}

	public Font sourceFontFile
	{
		get
		{
			return m_SourceFontFile;
		}
		internal set
		{
			m_SourceFontFile = value;
		}
	}

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

	public bool loadGlyphFromSystemFonts
	{
		get
		{
			return m_loadGlyphFromSystemFonts;
		}
		set
		{
			if (value && m_AtlasPopulationMode != AtlasPopulationMode.Dynamic)
			{
				Debug.LogWarning("The atlas population mode is static, can't load glyph from system fonts");
				m_loadGlyphFromSystemFonts = false;
			}
			else
			{
				m_loadGlyphFromSystemFonts = value;
			}
		}
	}

	public string[] fallbackSystemFonts
	{
		get
		{
			return m_fallbackSystemFonts;
		}
		set
		{
			if (value != null && value.Length != 0)
			{
				m_fallbackSystemFonts = value;
			}
			else
			{
				Debug.LogWarning("Fallback system fonts array should not be empty or null.");
			}
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

	public List<Glyph> glyphTable
	{
		get
		{
			return m_GlyphTable;
		}
		internal set
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

	public List<TMP_Character> characterTable
	{
		get
		{
			return m_CharacterTable;
		}
		internal set
		{
			m_CharacterTable = value;
		}
	}

	public Dictionary<uint, TMP_Character> characterLookupTable
	{
		get
		{
			if (m_CharacterLookupDictionary == null)
			{
				ReadFontAssetDefinition();
			}
			return m_CharacterLookupDictionary;
		}
		set
		{
			m_CharacterLookupDictionary = value;
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
			_ = m_AtlasTextures;
			return m_AtlasTextures;
		}
		set
		{
			m_AtlasTextures = value;
		}
	}

	public int atlasTextureCount => m_AtlasTextureIndex + 1;

	public bool isMultiAtlasTexturesEnabled
	{
		get
		{
			return m_IsMultiAtlasTexturesEnabled;
		}
		set
		{
			m_IsMultiAtlasTexturesEnabled = value;
		}
	}

	internal bool clearDynamicDataOnBuild
	{
		get
		{
			return m_ClearDynamicDataOnBuild;
		}
		set
		{
			m_ClearDynamicDataOnBuild = value;
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

	[Obsolete("The fontInfo property and underlying type is now obsolete. Please use the faceInfo property and FaceInfo type instead.")]
	public FaceInfo_Legacy fontInfo => m_fontInfo;

	public int atlasWidth
	{
		get
		{
			return m_AtlasWidth;
		}
		internal set
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
		internal set
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
		internal set
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
		internal set
		{
			m_AtlasRenderMode = value;
		}
	}

	public TMP_FontFeatureTable fontFeatureTable
	{
		get
		{
			return m_FontFeatureTable;
		}
		internal set
		{
			m_FontFeatureTable = value;
		}
	}

	public List<TMP_FontAsset> fallbackFontAssetTable
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

	public FontAssetCreationSettings creationSettings
	{
		get
		{
			return m_CreationSettings;
		}
		set
		{
			m_CreationSettings = value;
		}
	}

	public TMP_FontWeightPair[] fontWeightTable
	{
		get
		{
			return m_FontWeightTable;
		}
		internal set
		{
			m_FontWeightTable = value;
		}
	}

	public static TMP_FontAsset CreateFontAsset(Font font)
	{
		return CreateFontAsset(font, 90, 9, GlyphRenderMode.SDFAA, 1024, 1024);
	}

	public static TMP_FontAsset CreateFontAsset(Font font, int samplingPointSize, int atlasPadding, GlyphRenderMode renderMode, int atlasWidth, int atlasHeight, AtlasPopulationMode atlasPopulationMode = AtlasPopulationMode.Dynamic, bool enableMultiAtlasSupport = true)
	{
		FontEngine.InitializeFontEngine();
		if (FontEngine.LoadFontFace(font, samplingPointSize) != FontEngineError.Success)
		{
			Debug.LogWarning("Unable to load font face for [" + font.name + "]. Make sure \"Include Font Data\" is enabled in the Font Import Settings.", font);
			return null;
		}
		TMP_FontAsset tMP_FontAsset = ScriptableObject.CreateInstance<TMP_FontAsset>();
		tMP_FontAsset.m_Version = "1.1.0";
		tMP_FontAsset.faceInfo = FontEngine.GetFaceInfo();
		if (atlasPopulationMode == AtlasPopulationMode.Dynamic)
		{
			tMP_FontAsset.sourceFontFile = font;
		}
		tMP_FontAsset.atlasPopulationMode = atlasPopulationMode;
		tMP_FontAsset.atlasWidth = atlasWidth;
		tMP_FontAsset.atlasHeight = atlasHeight;
		tMP_FontAsset.atlasPadding = atlasPadding;
		tMP_FontAsset.atlasRenderMode = renderMode;
		tMP_FontAsset.atlasTextures = new Texture2D[1];
		Texture2D texture2D = new Texture2D(0, 0, TextureFormat.Alpha8, mipChain: false);
		tMP_FontAsset.atlasTextures[0] = texture2D;
		tMP_FontAsset.isMultiAtlasTexturesEnabled = enableMultiAtlasSupport;
		int num;
		if ((renderMode & (GlyphRenderMode)16) == (GlyphRenderMode)16)
		{
			num = 0;
			Material material = new Material(ShaderUtilities.ShaderRef_MobileBitmap);
			material.SetTexture(ShaderUtilities.ID_MainTex, texture2D);
			material.SetFloat(ShaderUtilities.ID_TextureWidth, atlasWidth);
			material.SetFloat(ShaderUtilities.ID_TextureHeight, atlasHeight);
			tMP_FontAsset.material = material;
		}
		else
		{
			num = 1;
			Material material2 = new Material(ShaderUtilities.ShaderRef_MobileSDF);
			material2.SetTexture(ShaderUtilities.ID_MainTex, texture2D);
			material2.SetFloat(ShaderUtilities.ID_TextureWidth, atlasWidth);
			material2.SetFloat(ShaderUtilities.ID_TextureHeight, atlasHeight);
			material2.SetFloat(ShaderUtilities.ID_GradientScale, atlasPadding + num);
			material2.SetFloat(ShaderUtilities.ID_WeightNormal, tMP_FontAsset.normalStyle);
			material2.SetFloat(ShaderUtilities.ID_WeightBold, tMP_FontAsset.boldStyle);
			tMP_FontAsset.material = material2;
		}
		tMP_FontAsset.freeGlyphRects = new List<GlyphRect>(8)
		{
			new GlyphRect(0, 0, atlasWidth - num, atlasHeight - num)
		};
		tMP_FontAsset.usedGlyphRects = new List<GlyphRect>(8);
		tMP_FontAsset.ReadFontAssetDefinition();
		return tMP_FontAsset;
	}

	private void Awake()
	{
		if (material != null && string.IsNullOrEmpty(m_Version))
		{
			UpgradeFontAsset();
		}
	}

	public void ReadFontAssetDefinition()
	{
		if (material != null && string.IsNullOrEmpty(m_Version))
		{
			UpgradeFontAsset();
		}
		InitializeDictionaryLookupTables();
		AddSynthesizedCharactersAndFaceMetrics();
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
		hashCode = TMP_TextUtilities.GetSimpleHashCode(base.name);
		materialHashCode = TMP_TextUtilities.GetSimpleHashCode(base.name + s_DefaultMaterialSuffix);
		IsFontAssetLookupTablesDirty = false;
	}

	internal void InitializeDictionaryLookupTables()
	{
		InitializeGlyphLookupDictionary();
		InitializeCharacterLookupDictionary();
		InitializeGlyphPaidAdjustmentRecordsLookupDictionary();
	}

	internal void InitializeGlyphLookupDictionary()
	{
		if (m_GlyphLookupDictionary == null)
		{
			m_GlyphLookupDictionary = new Dictionary<uint, Glyph>();
		}
		else
		{
			m_GlyphLookupDictionary.Clear();
		}
		if (m_GlyphIndexList == null)
		{
			m_GlyphIndexList = new List<uint>();
		}
		else
		{
			m_GlyphIndexList.Clear();
		}
		if (m_GlyphIndexListNewlyAdded == null)
		{
			m_GlyphIndexListNewlyAdded = new List<uint>();
		}
		else
		{
			m_GlyphIndexListNewlyAdded.Clear();
		}
		int count = m_GlyphTable.Count;
		for (int i = 0; i < count; i++)
		{
			Glyph glyph = m_GlyphTable[i];
			uint index = glyph.index;
			if (!m_GlyphLookupDictionary.ContainsKey(index))
			{
				m_GlyphLookupDictionary.Add(index, glyph);
				m_GlyphIndexList.Add(index);
			}
		}
	}

	internal void InitializeCharacterLookupDictionary()
	{
		if (m_CharacterLookupDictionary == null)
		{
			m_CharacterLookupDictionary = new Dictionary<uint, TMP_Character>();
		}
		else
		{
			m_CharacterLookupDictionary.Clear();
		}
		for (int i = 0; i < m_CharacterTable.Count; i++)
		{
			TMP_Character tMP_Character = m_CharacterTable[i];
			uint unicode = tMP_Character.unicode;
			uint glyphIndex = tMP_Character.glyphIndex;
			if (!m_CharacterLookupDictionary.ContainsKey(unicode))
			{
				m_CharacterLookupDictionary.Add(unicode, tMP_Character);
				tMP_Character.textAsset = this;
				tMP_Character.glyph = m_GlyphLookupDictionary[glyphIndex];
			}
		}
		if (FallbackSearchQueryLookup == null)
		{
			FallbackSearchQueryLookup = new HashSet<int>();
		}
		else
		{
			FallbackSearchQueryLookup.Clear();
		}
	}

	internal void InitializeGlyphPaidAdjustmentRecordsLookupDictionary()
	{
		if (m_KerningTable != null && m_KerningTable.kerningPairs != null && m_KerningTable.kerningPairs.Count > 0)
		{
			UpgradeGlyphAdjustmentTableToFontFeatureTable();
		}
		if (m_FontFeatureTable.m_GlyphPairAdjustmentRecordLookupDictionary == null)
		{
			m_FontFeatureTable.m_GlyphPairAdjustmentRecordLookupDictionary = new Dictionary<uint, TMP_GlyphPairAdjustmentRecord>();
		}
		else
		{
			m_FontFeatureTable.m_GlyphPairAdjustmentRecordLookupDictionary.Clear();
		}
		List<TMP_GlyphPairAdjustmentRecord> glyphPairAdjustmentRecords = m_FontFeatureTable.m_GlyphPairAdjustmentRecords;
		if (glyphPairAdjustmentRecords == null)
		{
			return;
		}
		for (int i = 0; i < glyphPairAdjustmentRecords.Count; i++)
		{
			TMP_GlyphPairAdjustmentRecord tMP_GlyphPairAdjustmentRecord = glyphPairAdjustmentRecords[i];
			uint key = new GlyphPairKey(tMP_GlyphPairAdjustmentRecord).key;
			if (!m_FontFeatureTable.m_GlyphPairAdjustmentRecordLookupDictionary.ContainsKey(key))
			{
				m_FontFeatureTable.m_GlyphPairAdjustmentRecordLookupDictionary.Add(key, tMP_GlyphPairAdjustmentRecord);
			}
		}
	}

	internal void AddSynthesizedCharactersAndFaceMetrics()
	{
		bool isFontFaceLoaded = false;
		if (m_AtlasPopulationMode == AtlasPopulationMode.Dynamic)
		{
			isFontFaceLoaded = FontEngine.LoadFontFace(sourceFontFile, m_FaceInfo.pointSize) == FontEngineError.Success;
		}
		AddSynthesizedCharacter(3u, isFontFaceLoaded, addImmediately: true);
		AddSynthesizedCharacter(9u, isFontFaceLoaded, addImmediately: true);
		AddSynthesizedCharacter(10u, isFontFaceLoaded);
		AddSynthesizedCharacter(11u, isFontFaceLoaded);
		AddSynthesizedCharacter(13u, isFontFaceLoaded);
		AddSynthesizedCharacter(1564u, isFontFaceLoaded);
		AddSynthesizedCharacter(8203u, isFontFaceLoaded);
		AddSynthesizedCharacter(8206u, isFontFaceLoaded);
		AddSynthesizedCharacter(8207u, isFontFaceLoaded);
		AddSynthesizedCharacter(8232u, isFontFaceLoaded);
		AddSynthesizedCharacter(8233u, isFontFaceLoaded);
		AddSynthesizedCharacter(8288u, isFontFaceLoaded);
		if (m_FaceInfo.capLine == 0f && m_CharacterLookupDictionary.ContainsKey(88u))
		{
			uint glyphIndex = m_CharacterLookupDictionary[88u].glyphIndex;
			m_FaceInfo.capLine = m_GlyphLookupDictionary[glyphIndex].metrics.horizontalBearingY;
		}
		if (m_FaceInfo.meanLine == 0f && m_CharacterLookupDictionary.ContainsKey(120u))
		{
			uint glyphIndex2 = m_CharacterLookupDictionary[120u].glyphIndex;
			m_FaceInfo.meanLine = m_GlyphLookupDictionary[glyphIndex2].metrics.horizontalBearingY;
		}
	}

	private void AddSynthesizedCharacter(uint unicode, bool isFontFaceLoaded, bool addImmediately = false)
	{
		if (m_CharacterLookupDictionary.ContainsKey(unicode))
		{
			return;
		}
		if (isFontFaceLoaded && FontEngine.GetGlyphIndex(unicode) != 0)
		{
			if (addImmediately)
			{
				GlyphLoadFlags flags = (((m_AtlasRenderMode & (GlyphRenderMode)4) == (GlyphRenderMode)4) ? (GlyphLoadFlags.LOAD_NO_HINTING | GlyphLoadFlags.LOAD_NO_BITMAP) : GlyphLoadFlags.LOAD_NO_BITMAP);
				if (FontEngine.TryGetGlyphWithUnicodeValue(unicode, flags, out var glyph))
				{
					m_CharacterLookupDictionary.Add(unicode, new TMP_Character(unicode, this, glyph));
				}
			}
		}
		else
		{
			Glyph glyph = new Glyph(0u, new GlyphMetrics(0f, 0f, 0f, 0f, 0f), GlyphRect.zero, 1f, 0);
			m_CharacterLookupDictionary.Add(unicode, new TMP_Character(unicode, this, glyph));
		}
	}

	internal void AddCharacterToLookupCache(uint unicode, TMP_Character character)
	{
		m_CharacterLookupDictionary.Add(unicode, character);
		FallbackSearchQueryLookup.Add(character.textAsset.instanceID);
	}

	internal void SortCharacterTable()
	{
		if (m_CharacterTable != null && m_CharacterTable.Count > 0)
		{
			m_CharacterTable = m_CharacterTable.OrderBy((TMP_Character c) => c.unicode).ToList();
		}
	}

	internal void SortGlyphTable()
	{
		if (m_GlyphTable != null && m_GlyphTable.Count > 0)
		{
			m_GlyphTable = m_GlyphTable.OrderBy((Glyph c) => c.index).ToList();
		}
	}

	internal void SortFontFeatureTable()
	{
		m_FontFeatureTable.SortGlyphPairAdjustmentRecords();
	}

	internal void SortAllTables()
	{
		SortGlyphTable();
		SortCharacterTable();
		SortFontFeatureTable();
	}

	public bool HasCharacter(int character)
	{
		if (m_CharacterLookupDictionary == null)
		{
			return false;
		}
		return m_CharacterLookupDictionary.ContainsKey((uint)character);
	}

	public bool HasCharacter(char character, bool searchFallbacks = false, bool tryAddCharacter = false)
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
		if (tryAddCharacter && m_AtlasPopulationMode == AtlasPopulationMode.Dynamic && TryAddCharacterInternal(character, out var _))
		{
			return true;
		}
		if (searchFallbacks)
		{
			if (k_SearchedFontAssetLookup == null)
			{
				k_SearchedFontAssetLookup = new HashSet<int>();
			}
			else
			{
				k_SearchedFontAssetLookup.Clear();
			}
			k_SearchedFontAssetLookup.Add(GetInstanceID());
			if (fallbackFontAssetTable != null && fallbackFontAssetTable.Count > 0)
			{
				for (int i = 0; i < fallbackFontAssetTable.Count && fallbackFontAssetTable[i] != null; i++)
				{
					TMP_FontAsset tMP_FontAsset = fallbackFontAssetTable[i];
					int item = tMP_FontAsset.GetInstanceID();
					if (k_SearchedFontAssetLookup.Add(item) && tMP_FontAsset.HasCharacter_Internal(character, searchFallbacks: true, tryAddCharacter))
					{
						return true;
					}
				}
			}
			if (TMP_Settings.fallbackFontAssets != null && TMP_Settings.fallbackFontAssets.Count > 0)
			{
				for (int j = 0; j < TMP_Settings.fallbackFontAssets.Count && TMP_Settings.fallbackFontAssets[j] != null; j++)
				{
					TMP_FontAsset tMP_FontAsset2 = TMP_Settings.fallbackFontAssets[j];
					int item2 = tMP_FontAsset2.GetInstanceID();
					if (k_SearchedFontAssetLookup.Add(item2) && tMP_FontAsset2.HasCharacter_Internal(character, searchFallbacks: true, tryAddCharacter))
					{
						return true;
					}
				}
			}
			if (TMP_Settings.defaultFontAsset != null)
			{
				TMP_FontAsset defaultFontAsset = TMP_Settings.defaultFontAsset;
				int item3 = defaultFontAsset.GetInstanceID();
				if (k_SearchedFontAssetLookup.Add(item3) && defaultFontAsset.HasCharacter_Internal(character, searchFallbacks: true, tryAddCharacter))
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool HasCharacter_Internal(uint character, bool searchFallbacks = false, bool tryAddCharacter = false)
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
		if (tryAddCharacter && atlasPopulationMode == AtlasPopulationMode.Dynamic && TryAddCharacterInternal(character, out var _))
		{
			return true;
		}
		if (searchFallbacks)
		{
			if (fallbackFontAssetTable == null || fallbackFontAssetTable.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < fallbackFontAssetTable.Count && fallbackFontAssetTable[i] != null; i++)
			{
				TMP_FontAsset tMP_FontAsset = fallbackFontAssetTable[i];
				int item = tMP_FontAsset.GetInstanceID();
				if (k_SearchedFontAssetLookup.Add(item) && tMP_FontAsset.HasCharacter_Internal(character, searchFallbacks: true, tryAddCharacter))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool HasCharacters(string text, out List<char> missingCharacters)
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

	public bool HasCharacters(string text, out uint[] missingCharacters, bool searchFallbacks = false, bool tryAddCharacter = false)
	{
		missingCharacters = null;
		if (m_CharacterLookupDictionary == null)
		{
			ReadFontAssetDefinition();
			if (m_CharacterLookupDictionary == null)
			{
				return false;
			}
		}
		s_MissingCharacterList.Clear();
		for (int i = 0; i < text.Length; i++)
		{
			bool flag = true;
			uint num = text[i];
			if (m_CharacterLookupDictionary.ContainsKey(num) || (tryAddCharacter && atlasPopulationMode == AtlasPopulationMode.Dynamic && TryAddCharacterInternal(num, out var _)))
			{
				continue;
			}
			if (searchFallbacks)
			{
				if (k_SearchedFontAssetLookup == null)
				{
					k_SearchedFontAssetLookup = new HashSet<int>();
				}
				else
				{
					k_SearchedFontAssetLookup.Clear();
				}
				k_SearchedFontAssetLookup.Add(GetInstanceID());
				if (fallbackFontAssetTable != null && fallbackFontAssetTable.Count > 0)
				{
					for (int j = 0; j < fallbackFontAssetTable.Count && fallbackFontAssetTable[j] != null; j++)
					{
						TMP_FontAsset tMP_FontAsset = fallbackFontAssetTable[j];
						int item = tMP_FontAsset.GetInstanceID();
						if (k_SearchedFontAssetLookup.Add(item) && tMP_FontAsset.HasCharacter_Internal(num, searchFallbacks: true, tryAddCharacter))
						{
							flag = false;
							break;
						}
					}
				}
				if (flag && TMP_Settings.fallbackFontAssets != null && TMP_Settings.fallbackFontAssets.Count > 0)
				{
					for (int k = 0; k < TMP_Settings.fallbackFontAssets.Count && TMP_Settings.fallbackFontAssets[k] != null; k++)
					{
						TMP_FontAsset tMP_FontAsset2 = TMP_Settings.fallbackFontAssets[k];
						int item2 = tMP_FontAsset2.GetInstanceID();
						if (k_SearchedFontAssetLookup.Add(item2) && tMP_FontAsset2.HasCharacter_Internal(num, searchFallbacks: true, tryAddCharacter))
						{
							flag = false;
							break;
						}
					}
				}
				if (flag && TMP_Settings.defaultFontAsset != null)
				{
					TMP_FontAsset defaultFontAsset = TMP_Settings.defaultFontAsset;
					int item3 = defaultFontAsset.GetInstanceID();
					if (k_SearchedFontAssetLookup.Add(item3) && defaultFontAsset.HasCharacter_Internal(num, searchFallbacks: true, tryAddCharacter))
					{
						flag = false;
					}
				}
			}
			if (flag)
			{
				s_MissingCharacterList.Add(num);
			}
		}
		if (s_MissingCharacterList.Count > 0)
		{
			missingCharacters = s_MissingCharacterList.ToArray();
			return false;
		}
		return true;
	}

	public bool HasCharacters(string text)
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

	public static string GetCharacters(TMP_FontAsset fontAsset)
	{
		string text = string.Empty;
		for (int i = 0; i < fontAsset.characterTable.Count; i++)
		{
			text += (char)fontAsset.characterTable[i].unicode;
		}
		return text;
	}

	public static int[] GetCharactersArray(TMP_FontAsset fontAsset)
	{
		int[] array = new int[fontAsset.characterTable.Count];
		for (int i = 0; i < fontAsset.characterTable.Count; i++)
		{
			array[i] = (int)fontAsset.characterTable[i].unicode;
		}
		return array;
	}

	internal uint GetGlyphIndex(uint unicode)
	{
		if (m_CharacterLookupDictionary.ContainsKey(unicode))
		{
			return m_CharacterLookupDictionary[unicode].glyphIndex;
		}
		if (FontEngine.LoadFontFace(sourceFontFile, m_FaceInfo.pointSize) != FontEngineError.Success)
		{
			return 0u;
		}
		return FontEngine.GetGlyphIndex(unicode);
	}

	internal static void RegisterFontAssetForFontFeatureUpdate(TMP_FontAsset fontAsset)
	{
		int item = fontAsset.instanceID;
		if (k_FontAssets_FontFeaturesUpdateQueueLookup.Add(item))
		{
			k_FontAssets_FontFeaturesUpdateQueue.Add(fontAsset);
		}
	}

	internal static void UpdateFontFeaturesForFontAssetsInQueue()
	{
		int count = k_FontAssets_FontFeaturesUpdateQueue.Count;
		for (int i = 0; i < count; i++)
		{
			k_FontAssets_FontFeaturesUpdateQueue[i].UpdateGlyphAdjustmentRecords();
		}
		if (count > 0)
		{
			k_FontAssets_FontFeaturesUpdateQueue.Clear();
			k_FontAssets_FontFeaturesUpdateQueueLookup.Clear();
		}
	}

	internal static void RegisterFontAssetForAtlasTextureUpdate(TMP_FontAsset fontAsset)
	{
		int item = fontAsset.instanceID;
		if (k_FontAssets_AtlasTexturesUpdateQueueLookup.Add(item))
		{
			k_FontAssets_AtlasTexturesUpdateQueue.Add(fontAsset);
		}
	}

	internal static void UpdateAtlasTexturesForFontAssetsInQueue()
	{
		int count = k_FontAssets_AtlasTexturesUpdateQueueLookup.Count;
		for (int i = 0; i < count; i++)
		{
			k_FontAssets_AtlasTexturesUpdateQueue[i].TryAddGlyphsToAtlasTextures();
		}
		if (count > 0)
		{
			k_FontAssets_AtlasTexturesUpdateQueue.Clear();
			k_FontAssets_AtlasTexturesUpdateQueueLookup.Clear();
		}
	}

	public bool TryAddCharacters(uint[] unicodes, bool includeFontFeatures = false)
	{
		uint[] missingUnicodes;
		return TryAddCharacters(unicodes, out missingUnicodes, includeFontFeatures);
	}

	public bool TryAddCharacters(uint[] unicodes, out uint[] missingUnicodes, bool includeFontFeatures = false)
	{
		if (unicodes == null || unicodes.Length == 0 || m_AtlasPopulationMode == AtlasPopulationMode.Static)
		{
			if (m_AtlasPopulationMode == AtlasPopulationMode.Static)
			{
				Debug.LogWarning("Unable to add characters to font asset [" + base.name + "] because its AtlasPopulationMode is set to Static.", this);
			}
			else
			{
				Debug.LogWarning("Unable to add characters to font asset [" + base.name + "] because the provided Unicode list is Null or Empty.", this);
			}
			missingUnicodes = null;
			return false;
		}
		if (FontEngine.LoadFontFace(m_SourceFontFile, m_FaceInfo.pointSize) != FontEngineError.Success)
		{
			missingUnicodes = unicodes.ToArray();
			return false;
		}
		if (m_CharacterLookupDictionary == null || m_GlyphLookupDictionary == null)
		{
			ReadFontAssetDefinition();
		}
		m_GlyphsToAdd.Clear();
		m_GlyphsToAddLookup.Clear();
		m_CharactersToAdd.Clear();
		m_CharactersToAddLookup.Clear();
		s_MissingCharacterList.Clear();
		bool flag = false;
		int num = unicodes.Length;
		for (int i = 0; i < num; i++)
		{
			uint num2 = unicodes[i];
			if (m_CharacterLookupDictionary.ContainsKey(num2))
			{
				continue;
			}
			uint glyphIndex = FontEngine.GetGlyphIndex(num2);
			if (glyphIndex == 0)
			{
				switch (num2)
				{
				case 160u:
					glyphIndex = FontEngine.GetGlyphIndex(32u);
					break;
				case 173u:
				case 8209u:
					glyphIndex = FontEngine.GetGlyphIndex(45u);
					break;
				}
				if (glyphIndex == 0)
				{
					s_MissingCharacterList.Add(num2);
					flag = true;
					continue;
				}
			}
			TMP_Character tMP_Character = new TMP_Character(num2, glyphIndex);
			if (m_GlyphLookupDictionary.ContainsKey(glyphIndex))
			{
				tMP_Character.glyph = m_GlyphLookupDictionary[glyphIndex];
				tMP_Character.textAsset = this;
				m_CharacterTable.Add(tMP_Character);
				m_CharacterLookupDictionary.Add(num2, tMP_Character);
				continue;
			}
			if (m_GlyphsToAddLookup.Add(glyphIndex))
			{
				m_GlyphsToAdd.Add(glyphIndex);
			}
			if (m_CharactersToAddLookup.Add(num2))
			{
				m_CharactersToAdd.Add(tMP_Character);
			}
		}
		if (m_GlyphsToAdd.Count == 0)
		{
			missingUnicodes = unicodes;
			return false;
		}
		if (m_AtlasTextures[m_AtlasTextureIndex].width == 0 || m_AtlasTextures[m_AtlasTextureIndex].height == 0)
		{
			m_AtlasTextures[m_AtlasTextureIndex].Resize(m_AtlasWidth, m_AtlasHeight);
			FontEngine.ResetAtlasTexture(m_AtlasTextures[m_AtlasTextureIndex]);
		}
		Glyph[] glyphs;
		bool flag2 = FontEngine.TryAddGlyphsToTexture(m_GlyphsToAdd, m_AtlasPadding, GlyphPackingMode.BestShortSideFit, m_FreeGlyphRects, m_UsedGlyphRects, m_AtlasRenderMode, m_AtlasTextures[m_AtlasTextureIndex], out glyphs);
		for (int j = 0; j < glyphs.Length && glyphs[j] != null; j++)
		{
			Glyph glyph = glyphs[j];
			uint index = glyph.index;
			glyph.atlasIndex = m_AtlasTextureIndex;
			m_GlyphTable.Add(glyph);
			m_GlyphLookupDictionary.Add(index, glyph);
			m_GlyphIndexListNewlyAdded.Add(index);
			m_GlyphIndexList.Add(index);
		}
		m_GlyphsToAdd.Clear();
		for (int k = 0; k < m_CharactersToAdd.Count; k++)
		{
			TMP_Character tMP_Character2 = m_CharactersToAdd[k];
			if (!m_GlyphLookupDictionary.TryGetValue(tMP_Character2.glyphIndex, out var value))
			{
				m_GlyphsToAdd.Add(tMP_Character2.glyphIndex);
				continue;
			}
			tMP_Character2.glyph = value;
			tMP_Character2.textAsset = this;
			m_CharacterTable.Add(tMP_Character2);
			m_CharacterLookupDictionary.Add(tMP_Character2.unicode, tMP_Character2);
			m_CharactersToAdd.RemoveAt(k);
			k--;
		}
		if (m_IsMultiAtlasTexturesEnabled && !flag2)
		{
			while (!flag2)
			{
				flag2 = TryAddGlyphsToNewAtlasTexture();
			}
		}
		if (includeFontFeatures)
		{
			UpdateGlyphAdjustmentRecords();
		}
		for (int l = 0; l < m_CharactersToAdd.Count; l++)
		{
			TMP_Character tMP_Character3 = m_CharactersToAdd[l];
			s_MissingCharacterList.Add(tMP_Character3.unicode);
		}
		missingUnicodes = null;
		if (s_MissingCharacterList.Count > 0)
		{
			missingUnicodes = s_MissingCharacterList.ToArray();
		}
		if (flag2)
		{
			return !flag;
		}
		return false;
	}

	public bool TryAddCharacters(string characters, bool includeFontFeatures = false)
	{
		string missingCharacters;
		return TryAddCharacters(characters, out missingCharacters, includeFontFeatures);
	}

	public bool TryAddCharacters(string characters, out string missingCharacters, bool includeFontFeatures = false)
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
			missingCharacters = characters;
			return false;
		}
		if (FontEngine.LoadFontFace(m_SourceFontFile, m_FaceInfo.pointSize) != FontEngineError.Success)
		{
			missingCharacters = characters;
			return false;
		}
		if (m_CharacterLookupDictionary == null || m_GlyphLookupDictionary == null)
		{
			ReadFontAssetDefinition();
		}
		m_GlyphsToAdd.Clear();
		m_GlyphsToAddLookup.Clear();
		m_CharactersToAdd.Clear();
		m_CharactersToAddLookup.Clear();
		s_MissingCharacterList.Clear();
		bool flag = false;
		int length = characters.Length;
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
				switch (num)
				{
				case 160u:
					glyphIndex = FontEngine.GetGlyphIndex(32u);
					break;
				case 173u:
				case 8209u:
					glyphIndex = FontEngine.GetGlyphIndex(45u);
					break;
				}
				if (glyphIndex == 0)
				{
					s_MissingCharacterList.Add(num);
					flag = true;
					continue;
				}
			}
			TMP_Character tMP_Character = new TMP_Character(num, glyphIndex);
			if (m_GlyphLookupDictionary.ContainsKey(glyphIndex))
			{
				tMP_Character.glyph = m_GlyphLookupDictionary[glyphIndex];
				tMP_Character.textAsset = this;
				m_CharacterTable.Add(tMP_Character);
				m_CharacterLookupDictionary.Add(num, tMP_Character);
				continue;
			}
			if (m_GlyphsToAddLookup.Add(glyphIndex))
			{
				m_GlyphsToAdd.Add(glyphIndex);
			}
			if (m_CharactersToAddLookup.Add(num))
			{
				m_CharactersToAdd.Add(tMP_Character);
			}
		}
		if (m_GlyphsToAdd.Count == 0)
		{
			missingCharacters = characters;
			return false;
		}
		if (m_AtlasTextures[m_AtlasTextureIndex].width == 0 || m_AtlasTextures[m_AtlasTextureIndex].height == 0)
		{
			m_AtlasTextures[m_AtlasTextureIndex].Resize(m_AtlasWidth, m_AtlasHeight);
			FontEngine.ResetAtlasTexture(m_AtlasTextures[m_AtlasTextureIndex]);
		}
		Glyph[] glyphs;
		bool flag2 = FontEngine.TryAddGlyphsToTexture(m_GlyphsToAdd, m_AtlasPadding, GlyphPackingMode.BestShortSideFit, m_FreeGlyphRects, m_UsedGlyphRects, m_AtlasRenderMode, m_AtlasTextures[m_AtlasTextureIndex], out glyphs);
		for (int j = 0; j < glyphs.Length && glyphs[j] != null; j++)
		{
			Glyph glyph = glyphs[j];
			uint index = glyph.index;
			glyph.atlasIndex = m_AtlasTextureIndex;
			m_GlyphTable.Add(glyph);
			m_GlyphLookupDictionary.Add(index, glyph);
			m_GlyphIndexListNewlyAdded.Add(index);
			m_GlyphIndexList.Add(index);
		}
		m_GlyphsToAdd.Clear();
		for (int k = 0; k < m_CharactersToAdd.Count; k++)
		{
			TMP_Character tMP_Character2 = m_CharactersToAdd[k];
			if (!m_GlyphLookupDictionary.TryGetValue(tMP_Character2.glyphIndex, out var value))
			{
				m_GlyphsToAdd.Add(tMP_Character2.glyphIndex);
				continue;
			}
			tMP_Character2.glyph = value;
			tMP_Character2.textAsset = this;
			m_CharacterTable.Add(tMP_Character2);
			m_CharacterLookupDictionary.Add(tMP_Character2.unicode, tMP_Character2);
			m_CharactersToAdd.RemoveAt(k);
			k--;
		}
		if (m_IsMultiAtlasTexturesEnabled && !flag2)
		{
			while (!flag2)
			{
				flag2 = TryAddGlyphsToNewAtlasTexture();
			}
		}
		if (includeFontFeatures)
		{
			UpdateGlyphAdjustmentRecords();
		}
		missingCharacters = string.Empty;
		for (int l = 0; l < m_CharactersToAdd.Count; l++)
		{
			TMP_Character tMP_Character3 = m_CharactersToAdd[l];
			s_MissingCharacterList.Add(tMP_Character3.unicode);
		}
		if (s_MissingCharacterList.Count > 0)
		{
			missingCharacters = s_MissingCharacterList.UintToString();
		}
		if (flag2)
		{
			return !flag;
		}
		return false;
	}

	internal bool TryAddCharacterInternal(uint unicode, out TMP_Character character)
	{
		character = null;
		if ((!loadGlyphFromSystemFonts && m_MissingUnicodesFromFontFile.Contains(unicode)) || s_MissingUnicodesFromSystemFont.Contains(unicode))
		{
			return false;
		}
		if (FontEngine.LoadFontFace(sourceFontFile, m_FaceInfo.pointSize) != FontEngineError.Success)
		{
			return false;
		}
		uint glyphIndex = FontEngine.GetGlyphIndex(unicode);
		if (glyphIndex == 0)
		{
			switch (unicode)
			{
			case 160u:
				glyphIndex = FontEngine.GetGlyphIndex(32u);
				break;
			case 173u:
			case 8209u:
				glyphIndex = FontEngine.GetGlyphIndex(45u);
				break;
			}
			if (glyphIndex == 0)
			{
				m_MissingUnicodesFromFontFile.Add(unicode);
				if (loadGlyphFromSystemFonts)
				{
					return TryAddCharacterFromSystemFonts(unicode, out character);
				}
				return false;
			}
		}
		if (m_GlyphLookupDictionary.ContainsKey(glyphIndex))
		{
			character = new TMP_Character(unicode, this, m_GlyphLookupDictionary[glyphIndex]);
			m_CharacterTable.Add(character);
			m_CharacterLookupDictionary.Add(unicode, character);
			return true;
		}
		Glyph glyph = null;
		if (!m_AtlasTextures[m_AtlasTextureIndex].isReadable)
		{
			Debug.LogWarning("Unable to add the requested character to font asset [" + base.name + "]'s atlas texture. Please make the texture [" + m_AtlasTextures[m_AtlasTextureIndex].name + "] readable.", m_AtlasTextures[m_AtlasTextureIndex]);
			return false;
		}
		if (m_AtlasTextures[m_AtlasTextureIndex].width == 0 || m_AtlasTextures[m_AtlasTextureIndex].height == 0)
		{
			m_AtlasTextures[m_AtlasTextureIndex].Resize(m_AtlasWidth, m_AtlasHeight);
			FontEngine.ResetAtlasTexture(m_AtlasTextures[m_AtlasTextureIndex]);
		}
		if (FontEngine.TryAddGlyphToTexture(glyphIndex, m_AtlasPadding, GlyphPackingMode.BestShortSideFit, m_FreeGlyphRects, m_UsedGlyphRects, m_AtlasRenderMode, m_AtlasTextures[m_AtlasTextureIndex], out glyph))
		{
			glyph.atlasIndex = m_AtlasTextureIndex;
			m_GlyphTable.Add(glyph);
			m_GlyphLookupDictionary.Add(glyphIndex, glyph);
			character = new TMP_Character(unicode, this, glyph);
			m_CharacterTable.Add(character);
			m_CharacterLookupDictionary.Add(unicode, character);
			m_GlyphIndexList.Add(glyphIndex);
			m_GlyphIndexListNewlyAdded.Add(glyphIndex);
			if (TMP_Settings.getFontFeaturesAtRuntime)
			{
				RegisterFontAssetForFontFeatureUpdate(this);
			}
			return true;
		}
		if (m_IsMultiAtlasTexturesEnabled)
		{
			SetupNewAtlasTexture();
			if (FontEngine.TryAddGlyphToTexture(glyphIndex, m_AtlasPadding, GlyphPackingMode.BestShortSideFit, m_FreeGlyphRects, m_UsedGlyphRects, m_AtlasRenderMode, m_AtlasTextures[m_AtlasTextureIndex], out glyph))
			{
				glyph.atlasIndex = m_AtlasTextureIndex;
				m_GlyphTable.Add(glyph);
				m_GlyphLookupDictionary.Add(glyphIndex, glyph);
				character = new TMP_Character(unicode, this, glyph);
				m_CharacterTable.Add(character);
				m_CharacterLookupDictionary.Add(unicode, character);
				m_GlyphIndexList.Add(glyphIndex);
				m_GlyphIndexListNewlyAdded.Add(glyphIndex);
				if (TMP_Settings.getFontFeaturesAtRuntime)
				{
					RegisterFontAssetForFontFeatureUpdate(this);
				}
				return true;
			}
		}
		return false;
	}

	internal bool TryAddCharacterFromSystemFonts(uint unicode, out TMP_Character character)
	{
		character = null;
		string[] array = m_fallbackSystemFonts;
		if (array == null || array.Length == 0)
		{
			if (s_SystemFontNames == null)
			{
				s_SystemFontNames = Font.GetPathsToOSFonts();
				s_SystemFontNames = s_SystemFontNames.Distinct().ToArray();
			}
			array = s_SystemFontNames;
		}
		return TryAddCharacterFromSystemFonts_Internal(unicode, array, out character);
	}

	private bool IsInPrivateUseArea(uint value)
	{
		int num = 57344;
		int num2 = 61439;
		if (value >= num)
		{
			return value <= num2;
		}
		return false;
	}

	private bool TryAddCharacterFromSystemFonts_Internal(uint unicode, string[] systemFonts, out TMP_Character character)
	{
		character = null;
		if (IsInPrivateUseArea(unicode))
		{
			return false;
		}
		if (systemFonts == null || systemFonts.Length == 0)
		{
			return false;
		}
		if (s_MissingUnicodesFromSystemFont.Contains(unicode))
		{
			return false;
		}
		uint num = 0u;
		if (s_UnicodeAndSystemFontMap.ContainsKey(unicode))
		{
			string text = s_UnicodeAndSystemFontMap[unicode];
			if (FontEngine.LoadFontFace(text, m_FaceInfo.pointSize, 0) != FontEngineError.Success)
			{
				Debug.LogError("The System Font " + text + " is not exist! ");
				return false;
			}
			num = FontEngine.GetGlyphIndex(unicode);
			if (num == 0)
			{
				switch (unicode)
				{
				case 160u:
					num = FontEngine.GetGlyphIndex(32u);
					break;
				case 173u:
				case 8209u:
					num = FontEngine.GetGlyphIndex(45u);
					break;
				}
			}
			if (num == 0)
			{
				Debug.LogError("An unexpected error occurred with the system font '" + text + "'.");
				return false;
			}
		}
		else
		{
			foreach (string text2 in systemFonts)
			{
				if (text2.IndexOf("emoji", StringComparison.OrdinalIgnoreCase) >= 0 || text2.IndexOf("LastResort", StringComparison.OrdinalIgnoreCase) >= 0 || FontEngine.LoadFontFace(text2, m_FaceInfo.pointSize, 0) != FontEngineError.Success)
				{
					continue;
				}
				num = FontEngine.GetGlyphIndex(unicode);
				if (num == 0)
				{
					switch (unicode)
					{
					case 160u:
						num = FontEngine.GetGlyphIndex(32u);
						break;
					case 173u:
					case 8209u:
						num = FontEngine.GetGlyphIndex(45u);
						break;
					}
					if (num == 0)
					{
						FontEngine.UnloadFontFace();
						continue;
					}
				}
				s_UnicodeAndSystemFontMap[unicode] = text2;
				break;
			}
		}
		if (num == 0)
		{
			s_MissingUnicodesFromSystemFont.Add(unicode);
			return false;
		}
		if (m_GlyphLookupDictionary.ContainsKey(num))
		{
			character = new TMP_Character(unicode, this, m_GlyphLookupDictionary[num]);
			m_CharacterTable.Add(character);
			m_CharacterLookupDictionary.Add(unicode, character);
			FontEngine.UnloadFontFace();
			return true;
		}
		Glyph glyph = null;
		if (!m_AtlasTextures[m_AtlasTextureIndex].isReadable)
		{
			Debug.LogWarning("Unable to add the requested character to font asset [" + base.name + "]'s atlas texture. Please make the texture [" + m_AtlasTextures[m_AtlasTextureIndex].name + "] readable.", m_AtlasTextures[m_AtlasTextureIndex]);
			FontEngine.UnloadFontFace();
			return false;
		}
		if (m_AtlasTextures[m_AtlasTextureIndex].width == 0 || m_AtlasTextures[m_AtlasTextureIndex].height == 0)
		{
			m_AtlasTextures[m_AtlasTextureIndex].Resize(m_AtlasWidth, m_AtlasHeight);
			FontEngine.ResetAtlasTexture(m_AtlasTextures[m_AtlasTextureIndex]);
		}
		if (FontEngine.TryAddGlyphToTexture(num, m_AtlasPadding, GlyphPackingMode.BestShortSideFit, m_FreeGlyphRects, m_UsedGlyphRects, m_AtlasRenderMode, m_AtlasTextures[m_AtlasTextureIndex], out glyph))
		{
			glyph.atlasIndex = m_AtlasTextureIndex;
			m_GlyphTable.Add(glyph);
			m_GlyphLookupDictionary.Add(num, glyph);
			character = new TMP_Character(unicode, this, glyph);
			m_CharacterTable.Add(character);
			m_CharacterLookupDictionary.Add(unicode, character);
			m_GlyphIndexList.Add(num);
			m_GlyphIndexListNewlyAdded.Add(num);
			if (TMP_Settings.getFontFeaturesAtRuntime)
			{
				RegisterFontAssetForFontFeatureUpdate(this);
			}
			FontEngine.UnloadFontFace();
			return true;
		}
		if (m_IsMultiAtlasTexturesEnabled)
		{
			SetupNewAtlasTexture();
			if (FontEngine.TryAddGlyphToTexture(num, m_AtlasPadding, GlyphPackingMode.BestShortSideFit, m_FreeGlyphRects, m_UsedGlyphRects, m_AtlasRenderMode, m_AtlasTextures[m_AtlasTextureIndex], out glyph))
			{
				glyph.atlasIndex = m_AtlasTextureIndex;
				m_GlyphTable.Add(glyph);
				m_GlyphLookupDictionary.Add(num, glyph);
				character = new TMP_Character(unicode, this, glyph);
				m_CharacterTable.Add(character);
				m_CharacterLookupDictionary.Add(unicode, character);
				m_GlyphIndexList.Add(num);
				m_GlyphIndexListNewlyAdded.Add(num);
				if (TMP_Settings.getFontFeaturesAtRuntime)
				{
					RegisterFontAssetForFontFeatureUpdate(this);
				}
				FontEngine.UnloadFontFace();
				return true;
			}
		}
		return false;
	}

	internal bool TryGetCharacter_and_QueueRenderToTexture(uint unicode, out TMP_Character character)
	{
		character = null;
		if (m_MissingUnicodesFromFontFile.Contains(unicode))
		{
			return false;
		}
		if (FontEngine.LoadFontFace(sourceFontFile, m_FaceInfo.pointSize) != FontEngineError.Success)
		{
			return false;
		}
		uint glyphIndex = FontEngine.GetGlyphIndex(unicode);
		if (glyphIndex == 0)
		{
			switch (unicode)
			{
			case 160u:
				glyphIndex = FontEngine.GetGlyphIndex(32u);
				break;
			case 173u:
			case 8209u:
				glyphIndex = FontEngine.GetGlyphIndex(45u);
				break;
			}
			if (glyphIndex == 0)
			{
				m_MissingUnicodesFromFontFile.Add(unicode);
				return false;
			}
		}
		if (m_GlyphLookupDictionary.ContainsKey(glyphIndex))
		{
			character = new TMP_Character(unicode, this, m_GlyphLookupDictionary[glyphIndex]);
			m_CharacterTable.Add(character);
			m_CharacterLookupDictionary.Add(unicode, character);
			return true;
		}
		GlyphLoadFlags flags = ((((GlyphRenderMode)4 & m_AtlasRenderMode) == (GlyphRenderMode)4) ? (GlyphLoadFlags.LOAD_NO_HINTING | GlyphLoadFlags.LOAD_NO_BITMAP) : GlyphLoadFlags.LOAD_NO_BITMAP);
		Glyph glyph = null;
		if (FontEngine.TryGetGlyphWithIndexValue(glyphIndex, flags, out glyph))
		{
			m_GlyphTable.Add(glyph);
			m_GlyphLookupDictionary.Add(glyphIndex, glyph);
			character = new TMP_Character(unicode, this, glyph);
			m_CharacterTable.Add(character);
			m_CharacterLookupDictionary.Add(unicode, character);
			m_GlyphIndexList.Add(glyphIndex);
			m_GlyphIndexListNewlyAdded.Add(glyphIndex);
			if (TMP_Settings.getFontFeaturesAtRuntime)
			{
				RegisterFontAssetForFontFeatureUpdate(this);
			}
			m_GlyphsToRender.Add(glyph);
			RegisterFontAssetForAtlasTextureUpdate(this);
			return true;
		}
		return false;
	}

	internal void TryAddGlyphsToAtlasTextures()
	{
	}

	private bool TryAddGlyphsToNewAtlasTexture()
	{
		SetupNewAtlasTexture();
		Glyph[] glyphs;
		bool result = FontEngine.TryAddGlyphsToTexture(m_GlyphsToAdd, m_AtlasPadding, GlyphPackingMode.BestShortSideFit, m_FreeGlyphRects, m_UsedGlyphRects, m_AtlasRenderMode, m_AtlasTextures[m_AtlasTextureIndex], out glyphs);
		for (int i = 0; i < glyphs.Length && glyphs[i] != null; i++)
		{
			Glyph glyph = glyphs[i];
			uint index = glyph.index;
			glyph.atlasIndex = m_AtlasTextureIndex;
			m_GlyphTable.Add(glyph);
			m_GlyphLookupDictionary.Add(index, glyph);
			m_GlyphIndexListNewlyAdded.Add(index);
			m_GlyphIndexList.Add(index);
		}
		m_GlyphsToAdd.Clear();
		for (int j = 0; j < m_CharactersToAdd.Count; j++)
		{
			TMP_Character tMP_Character = m_CharactersToAdd[j];
			if (!m_GlyphLookupDictionary.TryGetValue(tMP_Character.glyphIndex, out var value))
			{
				m_GlyphsToAdd.Add(tMP_Character.glyphIndex);
				continue;
			}
			tMP_Character.glyph = value;
			tMP_Character.textAsset = this;
			m_CharacterTable.Add(tMP_Character);
			m_CharacterLookupDictionary.Add(tMP_Character.unicode, tMP_Character);
			m_CharactersToAdd.RemoveAt(j);
			j--;
		}
		return result;
	}

	private void SetupNewAtlasTexture()
	{
		m_AtlasTextureIndex++;
		if (m_AtlasTextures.Length == m_AtlasTextureIndex)
		{
			Array.Resize(ref m_AtlasTextures, m_AtlasTextures.Length * 2);
		}
		m_AtlasTextures[m_AtlasTextureIndex] = new Texture2D(m_AtlasWidth, m_AtlasHeight, TextureFormat.Alpha8, mipChain: false);
		FontEngine.ResetAtlasTexture(m_AtlasTextures[m_AtlasTextureIndex]);
		int num = (((m_AtlasRenderMode & (GlyphRenderMode)16) != (GlyphRenderMode)16) ? 1 : 0);
		m_FreeGlyphRects.Clear();
		m_FreeGlyphRects.Add(new GlyphRect(0, 0, m_AtlasWidth - num, m_AtlasHeight - num));
		m_UsedGlyphRects.Clear();
	}

	internal void UpdateAtlasTexture()
	{
		if (m_GlyphsToRender.Count != 0)
		{
			if (m_AtlasTextures[m_AtlasTextureIndex].width == 0 || m_AtlasTextures[m_AtlasTextureIndex].height == 0)
			{
				m_AtlasTextures[m_AtlasTextureIndex].Resize(m_AtlasWidth, m_AtlasHeight);
				FontEngine.ResetAtlasTexture(m_AtlasTextures[m_AtlasTextureIndex]);
			}
			m_AtlasTextures[m_AtlasTextureIndex].Apply(updateMipmaps: false, makeNoLongerReadable: false);
		}
	}

	internal void UpdateGlyphAdjustmentRecords()
	{
		int recordCount;
		GlyphPairAdjustmentRecord[] glyphPairAdjustmentRecords = FontEngine.GetGlyphPairAdjustmentRecords(m_GlyphIndexList, out recordCount);
		m_GlyphIndexListNewlyAdded.Clear();
		if (glyphPairAdjustmentRecords == null || glyphPairAdjustmentRecords.Length == 0)
		{
			return;
		}
		if (m_FontFeatureTable == null)
		{
			m_FontFeatureTable = new TMP_FontFeatureTable();
		}
		for (int i = 0; i < glyphPairAdjustmentRecords.Length && glyphPairAdjustmentRecords[i].firstAdjustmentRecord.glyphIndex != 0; i++)
		{
			uint key = (glyphPairAdjustmentRecords[i].secondAdjustmentRecord.glyphIndex << 16) | glyphPairAdjustmentRecords[i].firstAdjustmentRecord.glyphIndex;
			if (!m_FontFeatureTable.m_GlyphPairAdjustmentRecordLookupDictionary.ContainsKey(key))
			{
				TMP_GlyphPairAdjustmentRecord tMP_GlyphPairAdjustmentRecord = new TMP_GlyphPairAdjustmentRecord(glyphPairAdjustmentRecords[i]);
				m_FontFeatureTable.m_GlyphPairAdjustmentRecords.Add(tMP_GlyphPairAdjustmentRecord);
				m_FontFeatureTable.m_GlyphPairAdjustmentRecordLookupDictionary.Add(key, tMP_GlyphPairAdjustmentRecord);
			}
		}
	}

	internal void UpdateGlyphAdjustmentRecords(uint[] glyphIndexes)
	{
		GlyphPairAdjustmentRecord[] glyphPairAdjustmentTable = FontEngine.GetGlyphPairAdjustmentTable(glyphIndexes);
		if (glyphPairAdjustmentTable == null || glyphPairAdjustmentTable.Length == 0)
		{
			return;
		}
		if (m_FontFeatureTable == null)
		{
			m_FontFeatureTable = new TMP_FontFeatureTable();
		}
		for (int i = 0; i < glyphPairAdjustmentTable.Length && glyphPairAdjustmentTable[i].firstAdjustmentRecord.glyphIndex != 0; i++)
		{
			uint key = (glyphPairAdjustmentTable[i].secondAdjustmentRecord.glyphIndex << 16) | glyphPairAdjustmentTable[i].firstAdjustmentRecord.glyphIndex;
			if (!m_FontFeatureTable.m_GlyphPairAdjustmentRecordLookupDictionary.ContainsKey(key))
			{
				TMP_GlyphPairAdjustmentRecord tMP_GlyphPairAdjustmentRecord = new TMP_GlyphPairAdjustmentRecord(glyphPairAdjustmentTable[i]);
				m_FontFeatureTable.m_GlyphPairAdjustmentRecords.Add(tMP_GlyphPairAdjustmentRecord);
				m_FontFeatureTable.m_GlyphPairAdjustmentRecordLookupDictionary.Add(key, tMP_GlyphPairAdjustmentRecord);
			}
		}
	}

	internal void UpdateGlyphAdjustmentRecords(List<uint> glyphIndexes)
	{
	}

	internal void UpdateGlyphAdjustmentRecords(List<uint> newGlyphIndexes, List<uint> allGlyphIndexes)
	{
	}

	private void CopyListDataToArray<T>(List<T> srcList, ref T[] dstArray)
	{
		int count = srcList.Count;
		if (dstArray == null)
		{
			dstArray = new T[count];
		}
		else
		{
			Array.Resize(ref dstArray, count);
		}
		for (int i = 0; i < count; i++)
		{
			dstArray[i] = srcList[i];
		}
	}

	public void ClearFontAssetData(bool setAtlasSizeToZero = false)
	{
		ClearFontAssetTables();
		ClearAtlasTextures(setAtlasSizeToZero);
		ReadFontAssetDefinition();
	}

	internal void ClearFontAssetDataInternal()
	{
		ClearFontAssetTables();
		ClearAtlasTextures(setAtlasSizeToZero: true);
	}

	internal void UpdateFontAssetData()
	{
		uint[] array = new uint[m_CharacterTable.Count];
		for (int i = 0; i < m_CharacterTable.Count; i++)
		{
			array[i] = m_CharacterTable[i].unicode;
		}
		ClearFontAssetTables();
		ClearAtlasTextures(setAtlasSizeToZero: true);
		ReadFontAssetDefinition();
		if (array.Length != 0)
		{
			TryAddCharacters(array, includeFontFeatures: true);
		}
	}

	internal void ClearFontAssetTables()
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
			m_FreeGlyphRects.Clear();
			m_FreeGlyphRects.Add(new GlyphRect(0, 0, m_AtlasWidth - num, m_AtlasHeight - num));
		}
		if (m_GlyphsToRender != null)
		{
			m_GlyphsToRender.Clear();
		}
		if (m_GlyphsRendered != null)
		{
			m_GlyphsRendered.Clear();
		}
		if (m_FontFeatureTable != null && m_FontFeatureTable.m_GlyphPairAdjustmentRecords != null)
		{
			m_FontFeatureTable.glyphPairAdjustmentRecords.Clear();
		}
	}

	internal void ClearAtlasTextures(bool setAtlasSizeToZero = false)
	{
		m_AtlasTextureIndex = 0;
		if (m_AtlasTextures == null)
		{
			return;
		}
		Texture2D texture2D = null;
		for (int i = 1; i < m_AtlasTextures.Length; i++)
		{
			texture2D = m_AtlasTextures[i];
			if (!(texture2D == null))
			{
				UnityEngine.Object.DestroyImmediate(texture2D, allowDestroyingAssets: true);
			}
		}
		Array.Resize(ref m_AtlasTextures, 1);
		texture2D = (m_AtlasTexture = m_AtlasTextures[0]);
		if (!texture2D.isReadable)
		{
			Debug.LogWarning("Unable to reset font asset [" + base.name + "]'s atlas texture. Please make the texture [" + texture2D.name + "] readable.", texture2D);
			return;
		}
		if (setAtlasSizeToZero)
		{
			texture2D.Resize(0, 0, TextureFormat.Alpha8, hasMipMap: false);
		}
		else if (texture2D.width != m_AtlasWidth || texture2D.height != m_AtlasHeight)
		{
			texture2D.Resize(m_AtlasWidth, m_AtlasHeight, TextureFormat.Alpha8, hasMipMap: false);
		}
		FontEngine.ResetAtlasTexture(texture2D);
		texture2D.Apply();
	}

	internal void UpgradeFontAsset()
	{
		m_Version = "1.1.0";
		Debug.Log("Upgrading font asset [" + base.name + "] to version " + m_Version + ".", this);
		m_FaceInfo.familyName = m_fontInfo.Name;
		m_FaceInfo.styleName = string.Empty;
		m_FaceInfo.pointSize = (int)m_fontInfo.PointSize;
		m_FaceInfo.scale = m_fontInfo.Scale;
		m_FaceInfo.lineHeight = m_fontInfo.LineHeight;
		m_FaceInfo.ascentLine = m_fontInfo.Ascender;
		m_FaceInfo.capLine = m_fontInfo.CapHeight;
		m_FaceInfo.meanLine = m_fontInfo.CenterLine;
		m_FaceInfo.baseline = m_fontInfo.Baseline;
		m_FaceInfo.descentLine = m_fontInfo.Descender;
		m_FaceInfo.superscriptOffset = m_fontInfo.SuperscriptOffset;
		m_FaceInfo.superscriptSize = m_fontInfo.SubSize;
		m_FaceInfo.subscriptOffset = m_fontInfo.SubscriptOffset;
		m_FaceInfo.subscriptSize = m_fontInfo.SubSize;
		m_FaceInfo.underlineOffset = m_fontInfo.Underline;
		m_FaceInfo.underlineThickness = m_fontInfo.UnderlineThickness;
		m_FaceInfo.strikethroughOffset = m_fontInfo.strikethrough;
		m_FaceInfo.strikethroughThickness = m_fontInfo.strikethroughThickness;
		m_FaceInfo.tabWidth = m_fontInfo.TabWidth;
		if (m_AtlasTextures == null || m_AtlasTextures.Length == 0)
		{
			m_AtlasTextures = new Texture2D[1];
		}
		m_AtlasTextures[0] = atlas;
		m_AtlasWidth = (int)m_fontInfo.AtlasWidth;
		m_AtlasHeight = (int)m_fontInfo.AtlasHeight;
		m_AtlasPadding = (int)m_fontInfo.Padding;
		switch (m_CreationSettings.renderMode)
		{
		case 0:
			m_AtlasRenderMode = GlyphRenderMode.SMOOTH_HINTED;
			break;
		case 1:
			m_AtlasRenderMode = GlyphRenderMode.SMOOTH;
			break;
		case 2:
			m_AtlasRenderMode = GlyphRenderMode.RASTER_HINTED;
			break;
		case 3:
			m_AtlasRenderMode = GlyphRenderMode.RASTER;
			break;
		case 6:
			m_AtlasRenderMode = GlyphRenderMode.SDF16;
			break;
		case 7:
			m_AtlasRenderMode = GlyphRenderMode.SDF32;
			break;
		}
		if (fontWeights != null && fontWeights.Length != 0)
		{
			m_FontWeightTable[4] = fontWeights[4];
			m_FontWeightTable[7] = fontWeights[7];
		}
		if (fallbackFontAssets != null && fallbackFontAssets.Count > 0)
		{
			if (m_FallbackFontAssetTable == null)
			{
				m_FallbackFontAssetTable = new List<TMP_FontAsset>(fallbackFontAssets.Count);
			}
			for (int i = 0; i < fallbackFontAssets.Count; i++)
			{
				m_FallbackFontAssetTable.Add(fallbackFontAssets[i]);
			}
		}
		if (m_CreationSettings.sourceFontFileGUID != null || m_CreationSettings.sourceFontFileGUID != string.Empty)
		{
			m_SourceFontFileGUID = m_CreationSettings.sourceFontFileGUID;
		}
		else
		{
			Debug.LogWarning("Font asset [" + base.name + "] doesn't have a reference to its source font file. Please assign the appropriate source font file for this asset in the Font Atlas & Material section of font asset inspector.", this);
		}
		m_GlyphTable.Clear();
		m_CharacterTable.Clear();
		bool flag = false;
		for (int j = 0; j < m_glyphInfoList.Count; j++)
		{
			TMP_Glyph tMP_Glyph = m_glyphInfoList[j];
			Glyph glyph = new Glyph();
			uint index = (uint)(j + 1);
			glyph.index = index;
			glyph.glyphRect = new GlyphRect((int)tMP_Glyph.x, m_AtlasHeight - (int)(tMP_Glyph.y + tMP_Glyph.height + 0.5f), (int)(tMP_Glyph.width + 0.5f), (int)(tMP_Glyph.height + 0.5f));
			glyph.metrics = new GlyphMetrics(tMP_Glyph.width, tMP_Glyph.height, tMP_Glyph.xOffset, tMP_Glyph.yOffset, tMP_Glyph.xAdvance);
			glyph.scale = tMP_Glyph.scale;
			glyph.atlasIndex = 0;
			m_GlyphTable.Add(glyph);
			TMP_Character item = new TMP_Character((uint)tMP_Glyph.id, this, glyph);
			if (tMP_Glyph.id == 32)
			{
				flag = true;
			}
			m_CharacterTable.Add(item);
		}
		if (!flag)
		{
			Debug.Log("Synthesizing Space for [" + base.name + "]");
			Glyph glyph2 = new Glyph(0u, new GlyphMetrics(0f, 0f, 0f, 0f, m_FaceInfo.ascentLine / 5f), GlyphRect.zero, 1f, 0);
			m_GlyphTable.Add(glyph2);
			m_CharacterTable.Add(new TMP_Character(32u, this, glyph2));
		}
		ReadFontAssetDefinition();
	}

	private void UpgradeGlyphAdjustmentTableToFontFeatureTable()
	{
		Debug.Log("Upgrading font asset [" + base.name + "] Glyph Adjustment Table.", this);
		if (m_FontFeatureTable == null)
		{
			m_FontFeatureTable = new TMP_FontFeatureTable();
		}
		int count = m_KerningTable.kerningPairs.Count;
		m_FontFeatureTable.m_GlyphPairAdjustmentRecords = new List<TMP_GlyphPairAdjustmentRecord>(count);
		for (int i = 0; i < count; i++)
		{
			KerningPair kerningPair = m_KerningTable.kerningPairs[i];
			uint glyphIndex = 0u;
			if (m_CharacterLookupDictionary.TryGetValue(kerningPair.firstGlyph, out var value))
			{
				glyphIndex = value.glyphIndex;
			}
			uint glyphIndex2 = 0u;
			if (m_CharacterLookupDictionary.TryGetValue(kerningPair.secondGlyph, out var value2))
			{
				glyphIndex2 = value2.glyphIndex;
			}
			TMP_GlyphAdjustmentRecord firstAdjustmentRecord = new TMP_GlyphAdjustmentRecord(glyphIndex, new TMP_GlyphValueRecord(kerningPair.firstGlyphAdjustments.xPlacement, kerningPair.firstGlyphAdjustments.yPlacement, kerningPair.firstGlyphAdjustments.xAdvance, kerningPair.firstGlyphAdjustments.yAdvance));
			TMP_GlyphAdjustmentRecord secondAdjustmentRecord = new TMP_GlyphAdjustmentRecord(glyphIndex2, new TMP_GlyphValueRecord(kerningPair.secondGlyphAdjustments.xPlacement, kerningPair.secondGlyphAdjustments.yPlacement, kerningPair.secondGlyphAdjustments.xAdvance, kerningPair.secondGlyphAdjustments.yAdvance));
			TMP_GlyphPairAdjustmentRecord item = new TMP_GlyphPairAdjustmentRecord(firstAdjustmentRecord, secondAdjustmentRecord);
			m_FontFeatureTable.m_GlyphPairAdjustmentRecords.Add(item);
		}
		m_KerningTable.kerningPairs = null;
		m_KerningTable = null;
	}
}
