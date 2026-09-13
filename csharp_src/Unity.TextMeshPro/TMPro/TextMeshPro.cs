using System;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore;
using UnityEngine.UI;

namespace TMPro;

[DisallowMultipleComponent]
[RequireComponent(typeof(MeshRenderer))]
[AddComponentMenu("Mesh/TextMeshPro - Text")]
[ExecuteAlways]
[HelpURL("https://docs.unity3d.com/Packages/com.unity.textmeshpro@2.1")]
public class TextMeshPro : TMP_Text, ILayoutElement
{
	[SerializeField]
	private bool m_hasFontAssetChanged;

	private float m_previousLossyScaleY = -1f;

	[SerializeField]
	private Renderer m_renderer;

	private MeshFilter m_meshFilter;

	private CanvasRenderer m_CanvasRenderer;

	private bool m_isFirstAllocation;

	private int m_max_characters = 8;

	private int m_max_numberOfLines = 4;

	private TMP_SubMesh[] m_subTextObjects = new TMP_SubMesh[8];

	[SerializeField]
	private MaskingTypes m_maskType;

	private Matrix4x4 m_EnvMapMatrix;

	private Vector3[] m_RectTransformCorners = new Vector3[4];

	[NonSerialized]
	private bool m_isRegisteredForEvents;

	private static ProfilerMarker k_GenerateTextMarker = new ProfilerMarker("TMP Layout Text");

	private static ProfilerMarker k_SetArraySizesMarker = new ProfilerMarker("TMP.SetArraySizes");

	private static ProfilerMarker k_GenerateTextPhaseIMarker = new ProfilerMarker("TMP GenerateText - Phase I");

	private static ProfilerMarker k_ParseMarkupTextMarker = new ProfilerMarker("TMP Parse Markup Text");

	private static ProfilerMarker k_CharacterLookupMarker = new ProfilerMarker("TMP Lookup Character & Glyph Data");

	private static ProfilerMarker k_HandleGPOSFeaturesMarker = new ProfilerMarker("TMP Handle GPOS Features");

	private static ProfilerMarker k_CalculateVerticesPositionMarker = new ProfilerMarker("TMP Calculate Vertices Position");

	private static ProfilerMarker k_ComputeTextMetricsMarker = new ProfilerMarker("TMP Compute Text Metrics");

	private static ProfilerMarker k_HandleVisibleCharacterMarker = new ProfilerMarker("TMP Handle Visible Character");

	private static ProfilerMarker k_HandleWhiteSpacesMarker = new ProfilerMarker("TMP Handle White Space & Control Character");

	private static ProfilerMarker k_HandleHorizontalLineBreakingMarker = new ProfilerMarker("TMP Handle Horizontal Line Breaking");

	private static ProfilerMarker k_HandleVerticalLineBreakingMarker = new ProfilerMarker("TMP Handle Vertical Line Breaking");

	private static ProfilerMarker k_SaveGlyphVertexDataMarker = new ProfilerMarker("TMP Save Glyph Vertex Data");

	private static ProfilerMarker k_ComputeCharacterAdvanceMarker = new ProfilerMarker("TMP Compute Character Advance");

	private static ProfilerMarker k_HandleCarriageReturnMarker = new ProfilerMarker("TMP Handle Carriage Return");

	private static ProfilerMarker k_HandleLineTerminationMarker = new ProfilerMarker("TMP Handle Line Termination");

	private static ProfilerMarker k_SavePageInfoMarker = new ProfilerMarker("TMP Save Text Extent & Page Info");

	private static ProfilerMarker k_SaveProcessingStatesMarker = new ProfilerMarker("TMP Save Processing States");

	private static ProfilerMarker k_GenerateTextPhaseIIMarker = new ProfilerMarker("TMP GenerateText - Phase II");

	private static ProfilerMarker k_GenerateTextPhaseIIIMarker = new ProfilerMarker("TMP GenerateText - Phase III");

	[SerializeField]
	internal int _SortingLayer;

	[SerializeField]
	internal int _SortingLayerID;

	[SerializeField]
	internal int _SortingOrder;

	private bool m_currentAutoSizeMode;

	public int sortingLayerID
	{
		get
		{
			if (renderer == null)
			{
				return 0;
			}
			return m_renderer.sortingLayerID;
		}
		set
		{
			if (!(renderer == null))
			{
				m_renderer.sortingLayerID = value;
				_SortingLayerID = value;
				UpdateSubMeshSortingLayerID(value);
			}
		}
	}

	public int sortingOrder
	{
		get
		{
			if (renderer == null)
			{
				return 0;
			}
			return m_renderer.sortingOrder;
		}
		set
		{
			if (!(renderer == null))
			{
				m_renderer.sortingOrder = value;
				_SortingOrder = value;
				UpdateSubMeshSortingOrder(value);
			}
		}
	}

	public override bool autoSizeTextContainer
	{
		get
		{
			return m_autoSizeTextContainer;
		}
		set
		{
			if (m_autoSizeTextContainer != value)
			{
				m_autoSizeTextContainer = value;
				if (m_autoSizeTextContainer)
				{
					TMP_UpdateManager.RegisterTextElementForLayoutRebuild(this);
					SetLayoutDirty();
				}
			}
		}
	}

	[Obsolete("The TextContainer is now obsolete. Use the RectTransform instead.")]
	public TextContainer textContainer => null;

	public new Transform transform
	{
		get
		{
			if (m_transform == null)
			{
				m_transform = GetComponent<Transform>();
			}
			return m_transform;
		}
	}

	public Renderer renderer
	{
		get
		{
			if (m_renderer == null)
			{
				m_renderer = GetComponent<Renderer>();
			}
			return m_renderer;
		}
	}

	public override Mesh mesh
	{
		get
		{
			if (m_mesh == null)
			{
				m_mesh = new Mesh();
				m_mesh.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_mesh;
		}
	}

	public MeshFilter meshFilter
	{
		get
		{
			if (m_meshFilter == null)
			{
				m_meshFilter = GetComponent<MeshFilter>();
				if (m_meshFilter == null)
				{
					m_meshFilter = base.gameObject.AddComponent<MeshFilter>();
					m_meshFilter.hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector;
				}
			}
			return m_meshFilter;
		}
	}

	public MaskingTypes maskType
	{
		get
		{
			return m_maskType;
		}
		set
		{
			m_maskType = value;
			SetMask(m_maskType);
		}
	}

	public override event Action<TMP_TextInfo> OnPreRenderText;

	protected override void Awake()
	{
		m_renderer = GetComponent<Renderer>();
		if (m_renderer == null)
		{
			m_renderer = base.gameObject.AddComponent<Renderer>();
		}
		m_rectTransform = base.rectTransform;
		m_transform = transform;
		m_meshFilter = GetComponent<MeshFilter>();
		if (m_meshFilter == null)
		{
			m_meshFilter = base.gameObject.AddComponent<MeshFilter>();
		}
		if (m_mesh == null)
		{
			m_mesh = new Mesh();
			m_mesh.hideFlags = HideFlags.HideAndDontSave;
			m_meshFilter.sharedMesh = m_mesh;
			m_textInfo = new TMP_TextInfo(this);
		}
		m_meshFilter.hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector;
		LoadDefaultSettings();
		LoadFontAsset();
		if (m_TextProcessingArray == null)
		{
			m_TextProcessingArray = new UnicodeChar[m_max_characters];
		}
		m_cached_TextElement = new TMP_Character();
		m_isFirstAllocation = true;
		TMP_SubMesh[] componentsInChildren = GetComponentsInChildren<TMP_SubMesh>();
		if (componentsInChildren.Length != 0)
		{
			int num = componentsInChildren.Length;
			if (num + 1 > m_subTextObjects.Length)
			{
				Array.Resize(ref m_subTextObjects, num + 1);
			}
			for (int i = 0; i < num; i++)
			{
				m_subTextObjects[i + 1] = componentsInChildren[i];
			}
		}
		m_havePropertiesChanged = true;
		m_isAwake = true;
	}

	protected override void OnEnable()
	{
		if (m_isAwake)
		{
			if (!m_isRegisteredForEvents)
			{
				m_isRegisteredForEvents = true;
			}
			if (!m_IsTextObjectScaleStatic)
			{
				TMP_UpdateManager.RegisterTextObjectForUpdate(this);
			}
			meshFilter.sharedMesh = mesh;
			SetActiveSubMeshes(state: true);
			ComputeMarginSize();
			SetAllDirty();
		}
	}

	protected override void OnDisable()
	{
		if (m_isAwake)
		{
			TMP_UpdateManager.UnRegisterTextElementForRebuild(this);
			TMP_UpdateManager.UnRegisterTextObjectForUpdate(this);
			meshFilter.sharedMesh = null;
			SetActiveSubMeshes(state: false);
		}
	}

	protected override void OnDestroy()
	{
		if (m_mesh != null)
		{
			UnityEngine.Object.DestroyImmediate(m_mesh);
		}
		m_isRegisteredForEvents = false;
		TMP_UpdateManager.UnRegisterTextElementForRebuild(this);
		TMP_UpdateManager.UnRegisterTextObjectForUpdate(this);
	}

	protected override void LoadFontAsset()
	{
		ShaderUtilities.GetShaderPropertyIDs();
		if (m_fontAsset == null)
		{
			if (TMP_Settings.defaultFontAsset != null)
			{
				m_fontAsset = TMP_Settings.defaultFontAsset;
			}
			else
			{
				m_fontAsset = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
			}
			if (m_fontAsset == null)
			{
				Debug.LogWarning("The LiberationSans SDF Font Asset was not found. There is no Font Asset assigned to " + base.gameObject.name + ".", this);
				return;
			}
			if (m_fontAsset.characterLookupTable == null)
			{
				Debug.Log("Dictionary is Null!");
			}
			m_sharedMaterial = m_fontAsset.material;
			m_sharedMaterial.SetFloat("_CullMode", 0f);
			m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 4f);
			m_renderer.receiveShadows = false;
			m_renderer.shadowCastingMode = ShadowCastingMode.Off;
		}
		else
		{
			if (m_fontAsset.characterLookupTable == null)
			{
				m_fontAsset.ReadFontAssetDefinition();
			}
			if (m_sharedMaterial == null || m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex) == null || m_fontAsset.atlasTexture.GetInstanceID() != m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
			{
				if (m_fontAsset.material == null)
				{
					Debug.LogWarning("The Font Atlas Texture of the Font Asset " + m_fontAsset.name + " assigned to " + base.gameObject.name + " is missing.", this);
				}
				else
				{
					m_sharedMaterial = m_fontAsset.material;
				}
			}
			m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 4f);
			if (m_sharedMaterial.passCount == 1)
			{
				m_renderer.receiveShadows = false;
				m_renderer.shadowCastingMode = ShadowCastingMode.Off;
			}
		}
		m_padding = GetPaddingForMaterial();
		m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(m_sharedMaterial);
		GetSpecialCharacters(m_fontAsset);
		SetMaterialDirty();
	}

	private void UpdateEnvMapMatrix()
	{
		if (m_sharedMaterial.HasProperty(ShaderUtilities.ID_EnvMap) && !(m_sharedMaterial.GetTexture(ShaderUtilities.ID_EnvMap) == null))
		{
			Vector3 euler = m_sharedMaterial.GetVector(ShaderUtilities.ID_EnvMatrixRotation);
			m_EnvMapMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(euler), Vector3.one);
			m_sharedMaterial.SetMatrix(ShaderUtilities.ID_EnvMatrix, m_EnvMapMatrix);
		}
	}

	private void SetMask(MaskingTypes maskType)
	{
		switch (maskType)
		{
		case MaskingTypes.MaskOff:
			m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
			m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
			m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
			break;
		case MaskingTypes.MaskSoft:
			m_sharedMaterial.EnableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
			m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
			m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
			break;
		case MaskingTypes.MaskHard:
			m_sharedMaterial.EnableKeyword(ShaderUtilities.Keyword_MASK_HARD);
			m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
			m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
			break;
		}
	}

	private void SetMaskCoordinates(Vector4 coords)
	{
		m_sharedMaterial.SetVector(ShaderUtilities.ID_ClipRect, coords);
	}

	private void SetMaskCoordinates(Vector4 coords, float softX, float softY)
	{
		m_sharedMaterial.SetVector(ShaderUtilities.ID_ClipRect, coords);
		m_sharedMaterial.SetFloat(ShaderUtilities.ID_MaskSoftnessX, softX);
		m_sharedMaterial.SetFloat(ShaderUtilities.ID_MaskSoftnessY, softY);
	}

	private void EnableMasking()
	{
		if (m_sharedMaterial.HasProperty(ShaderUtilities.ID_ClipRect))
		{
			m_sharedMaterial.EnableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
			m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
			m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
			m_isMaskingEnabled = true;
			UpdateMask();
		}
	}

	private void DisableMasking()
	{
		if (m_sharedMaterial.HasProperty(ShaderUtilities.ID_ClipRect))
		{
			m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
			m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
			m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
			m_isMaskingEnabled = false;
			UpdateMask();
		}
	}

	private void UpdateMask()
	{
		if (m_isMaskingEnabled && m_isMaskingEnabled && m_fontMaterial == null)
		{
			CreateMaterialInstance();
		}
	}

	protected override Material GetMaterial(Material mat)
	{
		if (m_fontMaterial == null || m_fontMaterial.GetInstanceID() != mat.GetInstanceID())
		{
			m_fontMaterial = CreateMaterialInstance(mat);
		}
		m_sharedMaterial = m_fontMaterial;
		m_padding = GetPaddingForMaterial();
		SetVerticesDirty();
		SetMaterialDirty();
		return m_sharedMaterial;
	}

	protected override Material[] GetMaterials(Material[] mats)
	{
		int materialCount = m_textInfo.materialCount;
		if (m_fontMaterials == null)
		{
			m_fontMaterials = new Material[materialCount];
		}
		else if (m_fontMaterials.Length != materialCount)
		{
			TMP_TextInfo.Resize(ref m_fontMaterials, materialCount, isBlockAllocated: false);
		}
		for (int i = 0; i < materialCount; i++)
		{
			if (i == 0)
			{
				m_fontMaterials[i] = base.fontMaterial;
			}
			else
			{
				m_fontMaterials[i] = m_subTextObjects[i].material;
			}
		}
		m_fontSharedMaterials = m_fontMaterials;
		return m_fontMaterials;
	}

	protected override void SetSharedMaterial(Material mat)
	{
		m_sharedMaterial = mat;
		m_padding = GetPaddingForMaterial();
		SetMaterialDirty();
	}

	protected override Material[] GetSharedMaterials()
	{
		int materialCount = m_textInfo.materialCount;
		if (m_fontSharedMaterials == null)
		{
			m_fontSharedMaterials = new Material[materialCount];
		}
		else if (m_fontSharedMaterials.Length != materialCount)
		{
			TMP_TextInfo.Resize(ref m_fontSharedMaterials, materialCount, isBlockAllocated: false);
		}
		for (int i = 0; i < materialCount; i++)
		{
			if (i == 0)
			{
				m_fontSharedMaterials[i] = m_sharedMaterial;
			}
			else
			{
				m_fontSharedMaterials[i] = m_subTextObjects[i].sharedMaterial;
			}
		}
		return m_fontSharedMaterials;
	}

	protected override void SetSharedMaterials(Material[] materials)
	{
		int materialCount = m_textInfo.materialCount;
		if (m_fontSharedMaterials == null)
		{
			m_fontSharedMaterials = new Material[materialCount];
		}
		else if (m_fontSharedMaterials.Length != materialCount)
		{
			TMP_TextInfo.Resize(ref m_fontSharedMaterials, materialCount, isBlockAllocated: false);
		}
		for (int i = 0; i < materialCount; i++)
		{
			Texture texture = materials[i].GetTexture(ShaderUtilities.ID_MainTex);
			if (i == 0)
			{
				if (!(texture == null) && texture.GetInstanceID() == m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
				{
					m_sharedMaterial = (m_fontSharedMaterials[i] = materials[i]);
					m_padding = GetPaddingForMaterial(m_sharedMaterial);
				}
			}
			else if (!(texture == null) && texture.GetInstanceID() == m_subTextObjects[i].sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID() && m_subTextObjects[i].isDefaultMaterial)
			{
				m_subTextObjects[i].sharedMaterial = (m_fontSharedMaterials[i] = materials[i]);
			}
		}
	}

	protected override void SetOutlineThickness(float thickness)
	{
		thickness = Mathf.Clamp01(thickness);
		m_renderer.material.SetFloat(ShaderUtilities.ID_OutlineWidth, thickness);
		if (m_fontMaterial == null)
		{
			m_fontMaterial = m_renderer.material;
		}
		m_fontMaterial = m_renderer.material;
		m_sharedMaterial = m_fontMaterial;
		m_padding = GetPaddingForMaterial();
	}

	protected override void SetFaceColor(Color32 color)
	{
		m_renderer.material.SetColor(ShaderUtilities.ID_FaceColor, color);
		if (m_fontMaterial == null)
		{
			m_fontMaterial = m_renderer.material;
		}
		m_sharedMaterial = m_fontMaterial;
	}

	protected override void SetOutlineColor(Color32 color)
	{
		m_renderer.material.SetColor(ShaderUtilities.ID_OutlineColor, color);
		if (m_fontMaterial == null)
		{
			m_fontMaterial = m_renderer.material;
		}
		m_sharedMaterial = m_fontMaterial;
	}

	private void CreateMaterialInstance()
	{
		Material material = new Material(m_sharedMaterial);
		material.shaderKeywords = m_sharedMaterial.shaderKeywords;
		material.name += " Instance";
		m_fontMaterial = material;
	}

	protected override void SetShaderDepth()
	{
		if (m_isOverlay)
		{
			m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 0f);
			m_renderer.material.renderQueue = 4000;
			m_sharedMaterial = m_renderer.material;
		}
		else
		{
			m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 4f);
			m_renderer.material.renderQueue = -1;
			m_sharedMaterial = m_renderer.material;
		}
	}

	protected override void SetCulling()
	{
		if (m_isCullingEnabled)
		{
			m_renderer.material.SetFloat("_CullMode", 2f);
			for (int i = 1; i < m_subTextObjects.Length && m_subTextObjects[i] != null; i++)
			{
				Renderer renderer = m_subTextObjects[i].renderer;
				if (renderer != null)
				{
					renderer.material.SetFloat(ShaderUtilities.ShaderTag_CullMode, 2f);
				}
			}
			return;
		}
		m_renderer.material.SetFloat("_CullMode", 0f);
		for (int j = 1; j < m_subTextObjects.Length && m_subTextObjects[j] != null; j++)
		{
			Renderer renderer2 = m_subTextObjects[j].renderer;
			if (renderer2 != null)
			{
				renderer2.material.SetFloat(ShaderUtilities.ShaderTag_CullMode, 0f);
			}
		}
	}

	private void SetPerspectiveCorrection()
	{
		if (m_isOrthographic)
		{
			m_sharedMaterial.SetFloat(ShaderUtilities.ID_PerspectiveFilter, 0f);
		}
		else
		{
			m_sharedMaterial.SetFloat(ShaderUtilities.ID_PerspectiveFilter, 0.875f);
		}
	}

	internal override int SetArraySizes(UnicodeChar[] unicodeChars)
	{
		int num = 0;
		m_totalCharacterCount = 0;
		m_isUsingBold = false;
		m_isParsingText = false;
		tag_NoParsing = false;
		m_FontStyleInternal = m_fontStyle;
		m_fontStyleStack.Clear();
		m_FontWeightInternal = (((m_FontStyleInternal & FontStyles.Bold) == FontStyles.Bold) ? FontWeight.Bold : m_fontWeight);
		m_FontWeightStack.SetDefault(m_FontWeightInternal);
		m_currentFontAsset = m_fontAsset;
		m_currentMaterial = m_sharedMaterial;
		m_currentMaterialIndex = 0;
		TMP_Text.m_materialReferenceStack.SetDefault(new MaterialReference(m_currentMaterialIndex, m_currentFontAsset, null, m_currentMaterial, m_padding));
		TMP_Text.m_materialReferenceIndexLookup.Clear();
		MaterialReference.AddMaterialReference(m_currentMaterial, m_currentFontAsset, ref TMP_Text.m_materialReferences, TMP_Text.m_materialReferenceIndexLookup);
		if (m_textInfo == null)
		{
			m_textInfo = new TMP_TextInfo(m_InternalTextProcessingArraySize);
		}
		else if (m_textInfo.characterInfo.Length < m_InternalTextProcessingArraySize)
		{
			TMP_TextInfo.Resize(ref m_textInfo.characterInfo, m_InternalTextProcessingArraySize, isBlockAllocated: false);
		}
		m_textElementType = TMP_TextElementType.Character;
		if (m_overflowMode == TextOverflowModes.Ellipsis)
		{
			GetEllipsisSpecialCharacter(m_currentFontAsset);
			if (m_Ellipsis.character != null)
			{
				if (m_Ellipsis.fontAsset.GetInstanceID() != m_currentFontAsset.GetInstanceID())
				{
					if (TMP_Settings.matchMaterialPreset && m_currentMaterial.GetInstanceID() != m_Ellipsis.fontAsset.material.GetInstanceID())
					{
						m_Ellipsis.material = TMP_MaterialManager.GetFallbackMaterial(m_currentMaterial, m_Ellipsis.fontAsset.material);
					}
					else
					{
						m_Ellipsis.material = m_Ellipsis.fontAsset.material;
					}
					m_Ellipsis.materialIndex = MaterialReference.AddMaterialReference(m_Ellipsis.material, m_Ellipsis.fontAsset, ref TMP_Text.m_materialReferences, TMP_Text.m_materialReferenceIndexLookup);
					TMP_Text.m_materialReferences[m_Ellipsis.materialIndex].referenceCount = 0;
				}
			}
			else
			{
				m_overflowMode = TextOverflowModes.Truncate;
				if (!TMP_Settings.warningsDisabled)
				{
					Debug.LogWarning("The character used for Ellipsis is not available in font asset [" + m_currentFontAsset.name + "] or any potential fallbacks. Switching Text Overflow mode to Truncate.", this);
				}
			}
		}
		if (m_overflowMode == TextOverflowModes.Linked && m_linkedTextComponent != null && !m_isCalculatingPreferredValues)
		{
			m_linkedTextComponent.text = string.Empty;
		}
		for (int i = 0; i < unicodeChars.Length && unicodeChars[i].unicode != 0; i++)
		{
			if (m_textInfo.characterInfo == null || m_totalCharacterCount >= m_textInfo.characterInfo.Length)
			{
				TMP_TextInfo.Resize(ref m_textInfo.characterInfo, m_totalCharacterCount + 1, isBlockAllocated: true);
			}
			int num2 = unicodeChars[i].unicode;
			if (m_isRichText && num2 == 60)
			{
				int currentMaterialIndex = m_currentMaterialIndex;
				if (ValidateHtmlTag(unicodeChars, i + 1, out var endIndex))
				{
					int stringIndex = unicodeChars[i].stringIndex;
					i = endIndex;
					if ((m_FontStyleInternal & FontStyles.Bold) == FontStyles.Bold)
					{
						m_isUsingBold = true;
					}
					if (m_textElementType == TMP_TextElementType.Sprite)
					{
						TMP_Text.m_materialReferences[m_currentMaterialIndex].referenceCount++;
						m_textInfo.characterInfo[m_totalCharacterCount].character = (char)(57344 + m_spriteIndex);
						m_textInfo.characterInfo[m_totalCharacterCount].spriteIndex = m_spriteIndex;
						m_textInfo.characterInfo[m_totalCharacterCount].fontAsset = m_currentFontAsset;
						m_textInfo.characterInfo[m_totalCharacterCount].spriteAsset = m_currentSpriteAsset;
						m_textInfo.characterInfo[m_totalCharacterCount].materialReferenceIndex = m_currentMaterialIndex;
						m_textInfo.characterInfo[m_totalCharacterCount].textElement = m_currentSpriteAsset.spriteCharacterTable[m_spriteIndex];
						m_textInfo.characterInfo[m_totalCharacterCount].elementType = m_textElementType;
						m_textInfo.characterInfo[m_totalCharacterCount].index = stringIndex;
						m_textInfo.characterInfo[m_totalCharacterCount].stringLength = unicodeChars[i].stringIndex - stringIndex + 1;
						m_textElementType = TMP_TextElementType.Character;
						m_currentMaterialIndex = currentMaterialIndex;
						num++;
						m_totalCharacterCount++;
					}
					continue;
				}
			}
			bool flag = false;
			TMP_FontAsset currentFontAsset = m_currentFontAsset;
			Material currentMaterial = m_currentMaterial;
			int currentMaterialIndex2 = m_currentMaterialIndex;
			if (m_textElementType == TMP_TextElementType.Character)
			{
				if ((m_FontStyleInternal & FontStyles.UpperCase) == FontStyles.UpperCase)
				{
					if (char.IsLower((char)num2))
					{
						num2 = char.ToUpper((char)num2);
					}
				}
				else if ((m_FontStyleInternal & FontStyles.LowerCase) == FontStyles.LowerCase)
				{
					if (char.IsUpper((char)num2))
					{
						num2 = char.ToLower((char)num2);
					}
				}
				else if ((m_FontStyleInternal & FontStyles.SmallCaps) == FontStyles.SmallCaps && char.IsLower((char)num2))
				{
					num2 = char.ToUpper((char)num2);
				}
			}
			bool isUsingAlternativeTypeface;
			TMP_TextElement tMP_TextElement = GetTextElement((uint)num2, m_currentFontAsset, m_FontStyleInternal, m_FontWeightInternal, out isUsingAlternativeTypeface);
			if (tMP_TextElement == null)
			{
				int num3 = num2;
				num2 = (unicodeChars[i].unicode = ((TMP_Settings.missingGlyphCharacter == 0) ? 9633 : TMP_Settings.missingGlyphCharacter));
				tMP_TextElement = TMP_FontAssetUtilities.GetCharacterFromFontAsset((uint)num2, m_currentFontAsset, includeFallbacks: true, m_FontStyleInternal, m_FontWeightInternal, out isUsingAlternativeTypeface);
				if (tMP_TextElement == null && TMP_Settings.fallbackFontAssets != null && TMP_Settings.fallbackFontAssets.Count > 0)
				{
					tMP_TextElement = TMP_FontAssetUtilities.GetCharacterFromFontAssets((uint)num2, m_currentFontAsset, TMP_Settings.fallbackFontAssets, includeFallbacks: true, m_FontStyleInternal, m_FontWeightInternal, out isUsingAlternativeTypeface);
				}
				if (tMP_TextElement == null && TMP_Settings.defaultFontAsset != null)
				{
					tMP_TextElement = TMP_FontAssetUtilities.GetCharacterFromFontAsset((uint)num2, TMP_Settings.defaultFontAsset, includeFallbacks: true, m_FontStyleInternal, m_FontWeightInternal, out isUsingAlternativeTypeface);
				}
				if (tMP_TextElement == null)
				{
					num2 = (unicodeChars[i].unicode = 32);
					tMP_TextElement = TMP_FontAssetUtilities.GetCharacterFromFontAsset((uint)num2, m_currentFontAsset, includeFallbacks: true, m_FontStyleInternal, m_FontWeightInternal, out isUsingAlternativeTypeface);
				}
				if (tMP_TextElement == null)
				{
					num2 = (unicodeChars[i].unicode = 3);
					tMP_TextElement = TMP_FontAssetUtilities.GetCharacterFromFontAsset((uint)num2, m_currentFontAsset, includeFallbacks: true, m_FontStyleInternal, m_FontWeightInternal, out isUsingAlternativeTypeface);
				}
				if (!TMP_Settings.warningsDisabled)
				{
					Debug.LogWarning((num3 > 65535) ? $"The character with Unicode value \\U{num3:X8} was not found in the [{m_fontAsset.name}] font asset or any potential fallbacks. It was replaced by Unicode character \\u{tMP_TextElement.unicode:X4} in text object [{base.name}]." : $"The character with Unicode value \\u{num3:X4} was not found in the [{m_fontAsset.name}] font asset or any potential fallbacks. It was replaced by Unicode character \\u{tMP_TextElement.unicode:X4} in text object [{base.name}].", this);
				}
			}
			if (tMP_TextElement.elementType == TextElementType.Character && tMP_TextElement.textAsset.instanceID != m_currentFontAsset.instanceID)
			{
				flag = true;
				m_currentFontAsset = tMP_TextElement.textAsset as TMP_FontAsset;
			}
			m_textInfo.characterInfo[m_totalCharacterCount].elementType = TMP_TextElementType.Character;
			m_textInfo.characterInfo[m_totalCharacterCount].textElement = tMP_TextElement;
			m_textInfo.characterInfo[m_totalCharacterCount].isUsingAlternateTypeface = isUsingAlternativeTypeface;
			m_textInfo.characterInfo[m_totalCharacterCount].character = (char)num2;
			m_textInfo.characterInfo[m_totalCharacterCount].index = unicodeChars[i].stringIndex;
			m_textInfo.characterInfo[m_totalCharacterCount].stringLength = unicodeChars[i].length;
			m_textInfo.characterInfo[m_totalCharacterCount].fontAsset = m_currentFontAsset;
			if (tMP_TextElement.elementType == TextElementType.Sprite)
			{
				TMP_SpriteAsset tMP_SpriteAsset = tMP_TextElement.textAsset as TMP_SpriteAsset;
				m_currentMaterialIndex = MaterialReference.AddMaterialReference(tMP_SpriteAsset.material, tMP_SpriteAsset, ref TMP_Text.m_materialReferences, TMP_Text.m_materialReferenceIndexLookup);
				TMP_Text.m_materialReferences[m_currentMaterialIndex].referenceCount++;
				m_textInfo.characterInfo[m_totalCharacterCount].elementType = TMP_TextElementType.Sprite;
				m_textInfo.characterInfo[m_totalCharacterCount].materialReferenceIndex = m_currentMaterialIndex;
				m_textInfo.characterInfo[m_totalCharacterCount].spriteAsset = tMP_SpriteAsset;
				m_textInfo.characterInfo[m_totalCharacterCount].spriteIndex = (int)tMP_TextElement.glyphIndex;
				m_textElementType = TMP_TextElementType.Character;
				m_currentMaterialIndex = currentMaterialIndex2;
				num++;
				m_totalCharacterCount++;
				continue;
			}
			if (flag && m_currentFontAsset.instanceID != m_fontAsset.instanceID)
			{
				if (TMP_Settings.matchMaterialPreset)
				{
					m_currentMaterial = TMP_MaterialManager.GetFallbackMaterial(m_currentMaterial, m_currentFontAsset.material);
				}
				else
				{
					m_currentMaterial = m_currentFontAsset.material;
				}
				m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, m_currentFontAsset, ref TMP_Text.m_materialReferences, TMP_Text.m_materialReferenceIndexLookup);
			}
			if (tMP_TextElement != null && tMP_TextElement.glyph.atlasIndex > 0)
			{
				m_currentMaterial = TMP_MaterialManager.GetFallbackMaterial(m_currentFontAsset, m_currentMaterial, tMP_TextElement.glyph.atlasIndex);
				m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, m_currentFontAsset, ref TMP_Text.m_materialReferences, TMP_Text.m_materialReferenceIndexLookup);
				flag = true;
			}
			if (!char.IsWhiteSpace((char)num2) && num2 != 8203)
			{
				if (TMP_Text.m_materialReferences[m_currentMaterialIndex].referenceCount < 16383)
				{
					TMP_Text.m_materialReferences[m_currentMaterialIndex].referenceCount++;
				}
				else
				{
					m_currentMaterialIndex = MaterialReference.AddMaterialReference(new Material(m_currentMaterial), m_currentFontAsset, ref TMP_Text.m_materialReferences, TMP_Text.m_materialReferenceIndexLookup);
					TMP_Text.m_materialReferences[m_currentMaterialIndex].referenceCount++;
				}
			}
			m_textInfo.characterInfo[m_totalCharacterCount].material = m_currentMaterial;
			m_textInfo.characterInfo[m_totalCharacterCount].materialReferenceIndex = m_currentMaterialIndex;
			TMP_Text.m_materialReferences[m_currentMaterialIndex].isFallbackMaterial = flag;
			if (flag)
			{
				TMP_Text.m_materialReferences[m_currentMaterialIndex].fallbackMaterial = currentMaterial;
				m_currentFontAsset = currentFontAsset;
				m_currentMaterial = currentMaterial;
				m_currentMaterialIndex = currentMaterialIndex2;
			}
			m_totalCharacterCount++;
		}
		if (m_isCalculatingPreferredValues)
		{
			m_isCalculatingPreferredValues = false;
			return m_totalCharacterCount;
		}
		m_textInfo.spriteCount = num;
		int num4 = (m_textInfo.materialCount = TMP_Text.m_materialReferenceIndexLookup.Count);
		if (num4 > m_textInfo.meshInfo.Length)
		{
			TMP_TextInfo.Resize(ref m_textInfo.meshInfo, num4, isBlockAllocated: false);
		}
		if (num4 > m_subTextObjects.Length)
		{
			TMP_TextInfo.Resize(ref m_subTextObjects, Mathf.NextPowerOfTwo(num4 + 1));
		}
		if (m_VertexBufferAutoSizeReduction && m_textInfo.characterInfo.Length - m_totalCharacterCount > 256)
		{
			TMP_TextInfo.Resize(ref m_textInfo.characterInfo, Mathf.Max(m_totalCharacterCount + 1, 256), isBlockAllocated: true);
		}
		for (int j = 0; j < num4; j++)
		{
			if (j > 0)
			{
				if (m_subTextObjects[j] == null)
				{
					m_subTextObjects[j] = TMP_SubMesh.AddSubTextObject(this, TMP_Text.m_materialReferences[j]);
					m_textInfo.meshInfo[j].vertices = null;
				}
				if (m_subTextObjects[j].sharedMaterial == null || m_subTextObjects[j].sharedMaterial.GetInstanceID() != TMP_Text.m_materialReferences[j].material.GetInstanceID())
				{
					m_subTextObjects[j].sharedMaterial = TMP_Text.m_materialReferences[j].material;
					m_subTextObjects[j].fontAsset = TMP_Text.m_materialReferences[j].fontAsset;
					m_subTextObjects[j].spriteAsset = TMP_Text.m_materialReferences[j].spriteAsset;
				}
				if (TMP_Text.m_materialReferences[j].isFallbackMaterial)
				{
					m_subTextObjects[j].fallbackMaterial = TMP_Text.m_materialReferences[j].material;
					m_subTextObjects[j].fallbackSourceMaterial = TMP_Text.m_materialReferences[j].fallbackMaterial;
				}
			}
			int referenceCount = TMP_Text.m_materialReferences[j].referenceCount;
			if (m_textInfo.meshInfo[j].vertices == null || m_textInfo.meshInfo[j].vertices.Length < referenceCount * 4)
			{
				if (m_textInfo.meshInfo[j].vertices == null)
				{
					if (j == 0)
					{
						m_textInfo.meshInfo[j] = new TMP_MeshInfo(m_mesh, referenceCount + 1);
					}
					else
					{
						m_textInfo.meshInfo[j] = new TMP_MeshInfo(m_subTextObjects[j].mesh, referenceCount + 1);
					}
				}
				else
				{
					m_textInfo.meshInfo[j].ResizeMeshInfo((referenceCount > 1024) ? (referenceCount + 256) : Mathf.NextPowerOfTwo(referenceCount + 1));
				}
			}
			else if (m_VertexBufferAutoSizeReduction && referenceCount > 0 && m_textInfo.meshInfo[j].vertices.Length / 4 - referenceCount > 256)
			{
				m_textInfo.meshInfo[j].ResizeMeshInfo((referenceCount > 1024) ? (referenceCount + 256) : Mathf.NextPowerOfTwo(referenceCount + 1));
			}
			m_textInfo.meshInfo[j].material = TMP_Text.m_materialReferences[j].material;
		}
		for (int k = num4; k < m_subTextObjects.Length && m_subTextObjects[k] != null; k++)
		{
			if (k < m_textInfo.meshInfo.Length)
			{
				m_textInfo.meshInfo[k].ClearUnusedVertices(0, updateMesh: true);
			}
		}
		return m_totalCharacterCount;
	}

	public override void ComputeMarginSize()
	{
		if (base.rectTransform != null)
		{
			Rect rect = m_rectTransform.rect;
			m_marginWidth = rect.width - m_margin.x - m_margin.z;
			m_marginHeight = rect.height - m_margin.y - m_margin.w;
			m_PreviousRectTransformSize = rect.size;
			m_PreviousPivotPosition = m_rectTransform.pivot;
			m_RectTransformCorners = GetTextContainerLocalCorners();
		}
	}

	protected override void OnDidApplyAnimationProperties()
	{
		m_havePropertiesChanged = true;
		isMaskUpdateRequired = true;
		SetVerticesDirty();
	}

	protected override void OnTransformParentChanged()
	{
		SetVerticesDirty();
		SetLayoutDirty();
	}

	protected override void OnRectTransformDimensionsChange()
	{
		if (!(base.rectTransform != null) || !(Mathf.Abs(m_rectTransform.rect.width - m_PreviousRectTransformSize.x) < 0.0001f) || !(Mathf.Abs(m_rectTransform.rect.height - m_PreviousRectTransformSize.y) < 0.0001f) || !(Mathf.Abs(m_rectTransform.pivot.x - m_PreviousPivotPosition.x) < 0.0001f) || !(Mathf.Abs(m_rectTransform.pivot.y - m_PreviousPivotPosition.y) < 0.0001f))
		{
			ComputeMarginSize();
			SetVerticesDirty();
			SetLayoutDirty();
		}
	}

	internal override void InternalUpdate()
	{
		if (!m_havePropertiesChanged)
		{
			float y = m_rectTransform.lossyScale.y;
			if (Mathf.Abs(y - m_previousLossyScaleY) > 0.0001f && m_TextProcessingArray[0].unicode != 0)
			{
				float scaleDelta = y / m_previousLossyScaleY;
				UpdateSDFScale(scaleDelta);
				m_previousLossyScaleY = y;
			}
		}
		if (m_isUsingLegacyAnimationComponent)
		{
			m_havePropertiesChanged = true;
			OnPreRenderObject();
		}
	}

	private void OnPreRenderObject()
	{
		if (!m_isAwake || (!IsActive() && !m_ignoreActiveState))
		{
			return;
		}
		if (m_fontAsset == null)
		{
			Debug.LogWarning("Please assign a Font Asset to this " + transform.name + " gameobject.", this);
		}
		else if (m_havePropertiesChanged || m_isLayoutDirty)
		{
			if (isMaskUpdateRequired)
			{
				UpdateMask();
				isMaskUpdateRequired = false;
			}
			if (checkPaddingRequired)
			{
				UpdateMeshPadding();
			}
			ParseInputText();
			TMP_FontAsset.UpdateFontFeaturesForFontAssetsInQueue();
			if (m_enableAutoSizing)
			{
				m_fontSize = Mathf.Clamp(m_fontSizeBase, m_fontSizeMin, m_fontSizeMax);
			}
			m_maxFontSize = m_fontSizeMax;
			m_minFontSize = m_fontSizeMin;
			m_lineSpacingDelta = 0f;
			m_charWidthAdjDelta = 0f;
			m_isTextTruncated = false;
			m_havePropertiesChanged = false;
			m_isLayoutDirty = false;
			m_ignoreActiveState = false;
			m_IsAutoSizePointSizeSet = false;
			m_AutoSizeIterationCount = 0;
			SetActiveSubTextObjectRenderers(m_renderer.enabled);
			while (!m_IsAutoSizePointSizeSet)
			{
				GenerateTextMesh();
				m_AutoSizeIterationCount++;
			}
		}
	}

	protected virtual void GenerateTextMesh()
	{
		if (m_fontAsset == null || m_fontAsset.characterLookupTable == null)
		{
			Debug.LogWarning("Can't Generate Mesh! No Font Asset has been assigned to Object ID: " + GetInstanceID());
			m_IsAutoSizePointSizeSet = true;
			return;
		}
		if (m_textInfo != null)
		{
			m_textInfo.Clear();
		}
		if (m_TextProcessingArray == null || m_TextProcessingArray.Length == 0 || m_TextProcessingArray[0].unicode == 0)
		{
			ClearMesh(updateMesh: true);
			m_preferredWidth = 0f;
			m_preferredHeight = 0f;
			TMPro_EventManager.ON_TEXT_CHANGED(this);
			m_IsAutoSizePointSizeSet = true;
			return;
		}
		m_currentFontAsset = m_fontAsset;
		m_currentMaterial = m_sharedMaterial;
		m_currentMaterialIndex = 0;
		TMP_Text.m_materialReferenceStack.SetDefault(new MaterialReference(m_currentMaterialIndex, m_currentFontAsset, null, m_currentMaterial, m_padding));
		m_currentSpriteAsset = m_spriteAsset;
		if (m_spriteAnimator != null)
		{
			m_spriteAnimator.StopAllAnimations();
		}
		int totalCharacterCount = m_totalCharacterCount;
		float num = m_fontSize / (float)m_fontAsset.m_FaceInfo.pointSize * m_fontAsset.m_FaceInfo.scale * (m_isOrthographic ? 1f : 0.1f);
		float num2 = num;
		float num3 = m_fontSize * 0.01f * (m_isOrthographic ? 1f : 0.1f);
		m_fontScaleMultiplier = 1f;
		m_currentFontSize = m_fontSize;
		m_sizeStack.SetDefault(m_currentFontSize);
		float num4 = 0f;
		int num5 = 0;
		m_FontStyleInternal = m_fontStyle;
		m_FontWeightInternal = (((m_FontStyleInternal & FontStyles.Bold) == FontStyles.Bold) ? FontWeight.Bold : m_fontWeight);
		m_FontWeightStack.SetDefault(m_FontWeightInternal);
		m_fontStyleStack.Clear();
		m_lineJustification = m_HorizontalAlignment;
		m_lineJustificationStack.SetDefault(m_lineJustification);
		float num6 = 0f;
		float num7 = 0f;
		float num8 = 0f;
		m_baselineOffset = 0f;
		m_baselineOffsetStack.Clear();
		bool flag = false;
		Vector3 start = Vector3.zero;
		Vector3 zero = Vector3.zero;
		bool flag2 = false;
		Vector3 start2 = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		bool flag3 = false;
		Vector3 start3 = Vector3.zero;
		Vector3 end = Vector3.zero;
		m_fontColor32 = m_fontColor;
		m_htmlColor = m_fontColor32;
		m_underlineColor = m_htmlColor;
		m_strikethroughColor = m_htmlColor;
		m_colorStack.SetDefault(m_htmlColor);
		m_underlineColorStack.SetDefault(m_htmlColor);
		m_strikethroughColorStack.SetDefault(m_htmlColor);
		m_HighlightStateStack.SetDefault(new HighlightState(m_htmlColor, TMP_Offset.zero));
		m_colorGradientPreset = null;
		m_colorGradientStack.SetDefault(null);
		m_ItalicAngle = m_currentFontAsset.italicStyle;
		m_ItalicAngleStack.SetDefault(m_ItalicAngle);
		m_actionStack.Clear();
		m_isFXMatrixSet = false;
		m_lineOffset = 0f;
		m_lineHeight = -32767f;
		float num9 = m_currentFontAsset.m_FaceInfo.lineHeight - (m_currentFontAsset.m_FaceInfo.ascentLine - m_currentFontAsset.m_FaceInfo.descentLine);
		m_cSpacing = 0f;
		m_monoSpacing = 0f;
		m_xAdvance = 0f;
		tag_LineIndent = 0f;
		tag_Indent = 0f;
		m_indentStack.SetDefault(0f);
		tag_NoParsing = false;
		m_characterCount = 0;
		m_firstCharacterOfLine = m_firstVisibleCharacter;
		m_lastCharacterOfLine = 0;
		m_firstVisibleCharacterOfLine = 0;
		m_lastVisibleCharacterOfLine = 0;
		m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
		m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
		m_lineNumber = 0;
		m_startOfLineAscender = 0f;
		m_startOfLineDescender = 0f;
		m_lineVisibleCharacterCount = 0;
		bool flag4 = true;
		m_IsDrivenLineSpacing = false;
		m_firstOverflowCharacterIndex = -1;
		m_pageNumber = 0;
		int num10 = Mathf.Clamp(m_pageToDisplay - 1, 0, m_textInfo.pageInfo.Length - 1);
		m_textInfo.ClearPageInfo();
		Vector4 vector = m_margin;
		float num11 = ((m_marginWidth > 0f) ? m_marginWidth : 0f);
		float num12 = ((m_marginHeight > 0f) ? m_marginHeight : 0f);
		m_marginLeft = 0f;
		m_marginRight = 0f;
		m_width = -1f;
		float num13 = num11 + 0.0001f - m_marginLeft - m_marginRight;
		m_meshExtents.min = TMP_Text.k_LargePositiveVector2;
		m_meshExtents.max = TMP_Text.k_LargeNegativeVector2;
		m_textInfo.ClearLineInfo();
		m_maxCapHeight = 0f;
		m_maxTextAscender = 0f;
		m_ElementDescender = 0f;
		m_PageAscender = 0f;
		float maxVisibleDescender = 0f;
		bool isMaxVisibleDescenderSet = false;
		m_isNewPage = false;
		bool flag5 = true;
		m_isNonBreakingSpace = false;
		bool flag6 = false;
		int num14 = 0;
		CharacterSubstitution characterSubstitution = new CharacterSubstitution(-1, 0u);
		bool flag7 = false;
		SaveWordWrappingState(ref TMP_Text.m_SavedWordWrapState, -1, -1);
		SaveWordWrappingState(ref TMP_Text.m_SavedLineState, -1, -1);
		SaveWordWrappingState(ref TMP_Text.m_SavedEllipsisState, -1, -1);
		SaveWordWrappingState(ref TMP_Text.m_SavedLastValidState, -1, -1);
		SaveWordWrappingState(ref TMP_Text.m_SavedSoftLineBreakState, -1, -1);
		TMP_Text.m_EllipsisInsertionCandidateStack.Clear();
		int num15 = 0;
		Vector3 vector2 = default(Vector3);
		Vector3 vector3 = default(Vector3);
		Vector3 vector4 = default(Vector3);
		Vector3 vector5 = default(Vector3);
		for (int i = 0; i < m_TextProcessingArray.Length && m_TextProcessingArray[i].unicode != 0; i++)
		{
			num5 = m_TextProcessingArray[i].unicode;
			if (num15 > 5)
			{
				Debug.LogError("Line breaking recursion max threshold hit... Character [" + num5 + "] index: " + i);
				characterSubstitution.index = m_characterCount;
				characterSubstitution.unicode = 3u;
			}
			if (m_isRichText && num5 == 60)
			{
				m_isParsingText = true;
				m_textElementType = TMP_TextElementType.Character;
				if (ValidateHtmlTag(m_TextProcessingArray, i + 1, out var endIndex))
				{
					i = endIndex;
					if (m_textElementType == TMP_TextElementType.Character)
					{
						continue;
					}
				}
			}
			else
			{
				m_textElementType = m_textInfo.characterInfo[m_characterCount].elementType;
				m_currentMaterialIndex = m_textInfo.characterInfo[m_characterCount].materialReferenceIndex;
				m_currentFontAsset = m_textInfo.characterInfo[m_characterCount].fontAsset;
			}
			int currentMaterialIndex = m_currentMaterialIndex;
			bool isUsingAlternateTypeface = m_textInfo.characterInfo[m_characterCount].isUsingAlternateTypeface;
			m_isParsingText = false;
			bool flag8 = false;
			if (characterSubstitution.index == m_characterCount)
			{
				num5 = (int)characterSubstitution.unicode;
				m_textElementType = TMP_TextElementType.Character;
				flag8 = true;
				switch (num5)
				{
				case 3:
					m_textInfo.characterInfo[m_characterCount].textElement = m_currentFontAsset.characterLookupTable[3u];
					m_isTextTruncated = true;
					break;
				case 8230:
					m_textInfo.characterInfo[m_characterCount].textElement = m_Ellipsis.character;
					m_textInfo.characterInfo[m_characterCount].elementType = TMP_TextElementType.Character;
					m_textInfo.characterInfo[m_characterCount].fontAsset = m_Ellipsis.fontAsset;
					m_textInfo.characterInfo[m_characterCount].material = m_Ellipsis.material;
					m_textInfo.characterInfo[m_characterCount].materialReferenceIndex = m_Ellipsis.materialIndex;
					m_isTextTruncated = true;
					characterSubstitution.index = m_characterCount + 1;
					characterSubstitution.unicode = 3u;
					break;
				}
			}
			if (m_characterCount < m_firstVisibleCharacter && num5 != 3)
			{
				m_textInfo.characterInfo[m_characterCount].isVisible = false;
				m_textInfo.characterInfo[m_characterCount].character = '\u200b';
				m_textInfo.characterInfo[m_characterCount].lineNumber = 0;
				m_characterCount++;
				continue;
			}
			float num16 = 1f;
			if (m_textElementType == TMP_TextElementType.Character)
			{
				if ((m_FontStyleInternal & FontStyles.UpperCase) == FontStyles.UpperCase)
				{
					if (char.IsLower((char)num5))
					{
						num5 = char.ToUpper((char)num5);
					}
				}
				else if ((m_FontStyleInternal & FontStyles.LowerCase) == FontStyles.LowerCase)
				{
					if (char.IsUpper((char)num5))
					{
						num5 = char.ToLower((char)num5);
					}
				}
				else if ((m_FontStyleInternal & FontStyles.SmallCaps) == FontStyles.SmallCaps && char.IsLower((char)num5))
				{
					num16 = 0.8f;
					num5 = char.ToUpper((char)num5);
				}
			}
			float num17 = 0f;
			float num18 = 0f;
			float num19 = 0f;
			if (m_textElementType == TMP_TextElementType.Sprite)
			{
				m_currentSpriteAsset = m_textInfo.characterInfo[m_characterCount].spriteAsset;
				m_spriteIndex = m_textInfo.characterInfo[m_characterCount].spriteIndex;
				TMP_SpriteCharacter tMP_SpriteCharacter = m_currentSpriteAsset.spriteCharacterTable[m_spriteIndex];
				if (tMP_SpriteCharacter == null)
				{
					continue;
				}
				if (num5 == 60)
				{
					num5 = 57344 + m_spriteIndex;
				}
				else
				{
					m_spriteColor = TMP_Text.s_colorWhite;
				}
				float num20 = m_currentFontSize / (float)m_currentFontAsset.faceInfo.pointSize * m_currentFontAsset.faceInfo.scale * (m_isOrthographic ? 1f : 0.1f);
				if (m_currentSpriteAsset.m_FaceInfo.pointSize > 0)
				{
					float num21 = m_currentFontSize / (float)m_currentSpriteAsset.m_FaceInfo.pointSize * m_currentSpriteAsset.m_FaceInfo.scale * (m_isOrthographic ? 1f : 0.1f);
					num2 = tMP_SpriteCharacter.m_Scale * tMP_SpriteCharacter.m_Glyph.scale * num21;
					num18 = m_currentSpriteAsset.m_FaceInfo.ascentLine;
					num17 = m_currentSpriteAsset.m_FaceInfo.baseline * num20 * m_fontScaleMultiplier * m_currentSpriteAsset.m_FaceInfo.scale;
					num19 = m_currentSpriteAsset.m_FaceInfo.descentLine;
				}
				else
				{
					float num22 = m_currentFontSize / (float)m_currentFontAsset.m_FaceInfo.pointSize * m_currentFontAsset.m_FaceInfo.scale * (m_isOrthographic ? 1f : 0.1f);
					num2 = m_currentFontAsset.m_FaceInfo.ascentLine / tMP_SpriteCharacter.m_Glyph.metrics.height * tMP_SpriteCharacter.m_Scale * tMP_SpriteCharacter.m_Glyph.scale * num22;
					float num23 = num22 / num2;
					num18 = m_currentFontAsset.m_FaceInfo.ascentLine * num23;
					num17 = m_currentFontAsset.m_FaceInfo.baseline * num20 * m_fontScaleMultiplier * m_currentFontAsset.m_FaceInfo.scale;
					num19 = m_currentFontAsset.m_FaceInfo.descentLine * num23;
				}
				m_cached_TextElement = tMP_SpriteCharacter;
				m_textInfo.characterInfo[m_characterCount].elementType = TMP_TextElementType.Sprite;
				m_textInfo.characterInfo[m_characterCount].scale = num2;
				m_textInfo.characterInfo[m_characterCount].spriteAsset = m_currentSpriteAsset;
				m_textInfo.characterInfo[m_characterCount].fontAsset = m_currentFontAsset;
				m_textInfo.characterInfo[m_characterCount].materialReferenceIndex = m_currentMaterialIndex;
				m_currentMaterialIndex = currentMaterialIndex;
				num6 = 0f;
			}
			else if (m_textElementType == TMP_TextElementType.Character)
			{
				m_cached_TextElement = m_textInfo.characterInfo[m_characterCount].textElement;
				if (m_cached_TextElement == null)
				{
					continue;
				}
				m_currentFontAsset = m_textInfo.characterInfo[m_characterCount].fontAsset;
				m_currentMaterial = m_textInfo.characterInfo[m_characterCount].material;
				m_currentMaterialIndex = m_textInfo.characterInfo[m_characterCount].materialReferenceIndex;
				float num24 = ((!flag8 || m_TextProcessingArray[i].unicode != 10 || m_characterCount == m_firstCharacterOfLine) ? (m_currentFontSize * num16 / (float)m_currentFontAsset.m_FaceInfo.pointSize * m_currentFontAsset.m_FaceInfo.scale * (m_isOrthographic ? 1f : 0.1f)) : (m_textInfo.characterInfo[m_characterCount - 1].pointSize * num16 / (float)m_currentFontAsset.m_FaceInfo.pointSize * m_currentFontAsset.m_FaceInfo.scale * (m_isOrthographic ? 1f : 0.1f)));
				if (flag8 && num5 == 8230)
				{
					num18 = 0f;
					num19 = 0f;
				}
				else
				{
					num18 = m_currentFontAsset.m_FaceInfo.ascentLine;
					num19 = m_currentFontAsset.m_FaceInfo.descentLine;
				}
				num2 = num24 * m_fontScaleMultiplier * m_cached_TextElement.m_Scale * m_cached_TextElement.m_Glyph.scale;
				num17 = m_currentFontAsset.m_FaceInfo.baseline * num24 * m_fontScaleMultiplier * m_currentFontAsset.m_FaceInfo.scale;
				m_textInfo.characterInfo[m_characterCount].elementType = TMP_TextElementType.Character;
				m_textInfo.characterInfo[m_characterCount].scale = num2;
				num6 = ((m_currentMaterialIndex == 0) ? m_padding : m_subTextObjects[m_currentMaterialIndex].padding);
			}
			float num25 = num2;
			if (num5 == 173 || num5 == 3)
			{
				num2 = 0f;
			}
			m_textInfo.characterInfo[m_characterCount].character = (char)num5;
			m_textInfo.characterInfo[m_characterCount].pointSize = m_currentFontSize;
			m_textInfo.characterInfo[m_characterCount].color = m_htmlColor;
			m_textInfo.characterInfo[m_characterCount].underlineColor = m_underlineColor;
			m_textInfo.characterInfo[m_characterCount].strikethroughColor = m_strikethroughColor;
			m_textInfo.characterInfo[m_characterCount].highlightState = m_HighlightStateStack.current;
			m_textInfo.characterInfo[m_characterCount].style = m_FontStyleInternal;
			GlyphMetrics metrics = m_cached_TextElement.m_Glyph.metrics;
			bool flag9 = num5 <= 65535 && char.IsWhiteSpace((char)num5);
			TMP_GlyphValueRecord tMP_GlyphValueRecord = default(TMP_GlyphValueRecord);
			float num26 = m_characterSpacing;
			m_GlyphHorizontalAdvanceAdjustment = 0f;
			if (m_enableKerning)
			{
				uint glyphIndex = m_cached_TextElement.m_GlyphIndex;
				TMP_GlyphPairAdjustmentRecord value;
				if (m_characterCount < totalCharacterCount - 1)
				{
					uint key = (m_textInfo.characterInfo[m_characterCount + 1].textElement.m_GlyphIndex << 16) | glyphIndex;
					if (m_currentFontAsset.m_FontFeatureTable.m_GlyphPairAdjustmentRecordLookupDictionary.TryGetValue(key, out value))
					{
						tMP_GlyphValueRecord = value.m_FirstAdjustmentRecord.m_GlyphValueRecord;
						num26 = (((value.m_FeatureLookupFlags & FontFeatureLookupFlags.IgnoreSpacingAdjustments) == FontFeatureLookupFlags.IgnoreSpacingAdjustments) ? 0f : num26);
					}
				}
				if (m_characterCount >= 1)
				{
					uint glyphIndex2 = m_textInfo.characterInfo[m_characterCount - 1].textElement.m_GlyphIndex;
					uint key2 = (glyphIndex << 16) | glyphIndex2;
					if (m_currentFontAsset.m_FontFeatureTable.m_GlyphPairAdjustmentRecordLookupDictionary.TryGetValue(key2, out value))
					{
						tMP_GlyphValueRecord += value.m_SecondAdjustmentRecord.m_GlyphValueRecord;
						num26 = (((value.m_FeatureLookupFlags & FontFeatureLookupFlags.IgnoreSpacingAdjustments) == FontFeatureLookupFlags.IgnoreSpacingAdjustments) ? 0f : num26);
					}
				}
				m_GlyphHorizontalAdvanceAdjustment = tMP_GlyphValueRecord.xAdvance;
			}
			if (m_isRightToLeft)
			{
				m_xAdvance -= metrics.horizontalAdvance * (1f - m_charWidthAdjDelta) * num2;
				if (flag9 || num5 == 8203)
				{
					m_xAdvance -= m_wordSpacing * num3;
				}
			}
			float num27 = 0f;
			if (m_monoSpacing != 0f)
			{
				num27 = (m_monoSpacing / 2f - (metrics.width / 2f + metrics.horizontalBearingX) * num2) * (1f - m_charWidthAdjDelta);
				m_xAdvance += num27;
			}
			if (m_textElementType == TMP_TextElementType.Character && !isUsingAlternateTypeface && (m_FontStyleInternal & FontStyles.Bold) == FontStyles.Bold)
			{
				if (m_currentMaterial != null && m_currentMaterial.HasProperty(ShaderUtilities.ID_GradientScale))
				{
					float num28 = m_currentMaterial.GetFloat(ShaderUtilities.ID_GradientScale);
					num7 = m_currentFontAsset.boldStyle / 4f * num28 * m_currentMaterial.GetFloat(ShaderUtilities.ID_ScaleRatio_A);
					if (num7 + num6 > num28)
					{
						num6 = num28 - num7;
					}
				}
				else
				{
					num7 = 0f;
				}
				num8 = m_currentFontAsset.boldSpacing;
			}
			else
			{
				if (m_currentMaterial != null && m_currentMaterial.HasProperty(ShaderUtilities.ID_GradientScale) && m_currentMaterial.HasProperty(ShaderUtilities.ID_ScaleRatio_A))
				{
					float num29 = m_currentMaterial.GetFloat(ShaderUtilities.ID_GradientScale);
					num7 = m_currentFontAsset.normalStyle / 4f * num29 * m_currentMaterial.GetFloat(ShaderUtilities.ID_ScaleRatio_A);
					if (num7 + num6 > num29)
					{
						num6 = num29 - num7;
					}
				}
				else
				{
					num7 = 0f;
				}
				num8 = 0f;
			}
			vector2.x = m_xAdvance + (metrics.horizontalBearingX - num6 - num7 + tMP_GlyphValueRecord.m_XPlacement) * num2 * (1f - m_charWidthAdjDelta);
			vector2.y = num17 + (metrics.horizontalBearingY + num6 + tMP_GlyphValueRecord.m_YPlacement) * num2 - m_lineOffset + m_baselineOffset;
			vector2.z = 0f;
			vector3.x = vector2.x;
			vector3.y = vector2.y - (metrics.height + num6 * 2f) * num2;
			vector3.z = 0f;
			vector4.x = vector3.x + (metrics.width + num6 * 2f + num7 * 2f) * num2 * (1f - m_charWidthAdjDelta);
			vector4.y = vector2.y;
			vector4.z = 0f;
			vector5.x = vector4.x;
			vector5.y = vector3.y;
			vector5.z = 0f;
			if (m_textElementType == TMP_TextElementType.Character && !isUsingAlternateTypeface && (m_FontStyleInternal & FontStyles.Italic) == FontStyles.Italic)
			{
				float num30 = (float)m_ItalicAngle * 0.01f;
				Vector3 vector6 = new Vector3(num30 * ((metrics.horizontalBearingY + num6 + num7) * num2), 0f, 0f);
				Vector3 vector7 = new Vector3(num30 * ((metrics.horizontalBearingY - metrics.height - num6 - num7) * num2), 0f, 0f);
				Vector3 vector8 = new Vector3((vector6.x - vector7.x) / 2f, 0f, 0f);
				vector2 = vector2 + vector6 - vector8;
				vector3 = vector3 + vector7 - vector8;
				vector4 = vector4 + vector6 - vector8;
				vector5 = vector5 + vector7 - vector8;
			}
			if (m_isFXMatrixSet)
			{
				_ = m_FXMatrix.lossyScale.x;
				_ = 1f;
				Vector3 vector9 = (vector4 + vector3) / 2f;
				vector2 = m_FXMatrix.MultiplyPoint3x4(vector2 - vector9) + vector9;
				vector3 = m_FXMatrix.MultiplyPoint3x4(vector3 - vector9) + vector9;
				vector4 = m_FXMatrix.MultiplyPoint3x4(vector4 - vector9) + vector9;
				vector5 = m_FXMatrix.MultiplyPoint3x4(vector5 - vector9) + vector9;
			}
			m_textInfo.characterInfo[m_characterCount].bottomLeft = vector3;
			m_textInfo.characterInfo[m_characterCount].topLeft = vector2;
			m_textInfo.characterInfo[m_characterCount].topRight = vector4;
			m_textInfo.characterInfo[m_characterCount].bottomRight = vector5;
			m_textInfo.characterInfo[m_characterCount].origin = m_xAdvance;
			m_textInfo.characterInfo[m_characterCount].baseLine = num17 - m_lineOffset + m_baselineOffset;
			m_textInfo.characterInfo[m_characterCount].aspectRatio = (vector4.x - vector3.x) / (vector2.y - vector3.y);
			float num31 = ((m_textElementType == TMP_TextElementType.Character) ? (num18 * num2 / num16 + m_baselineOffset) : (num18 * num2 + m_baselineOffset));
			float num32 = ((m_textElementType == TMP_TextElementType.Character) ? (num19 * num2 / num16 + m_baselineOffset) : (num19 * num2 + m_baselineOffset));
			float num33 = num31;
			float num34 = num32;
			bool flag10 = m_characterCount == m_firstCharacterOfLine;
			if (flag10 || !flag9)
			{
				if (m_baselineOffset != 0f)
				{
					num33 = Mathf.Max((num31 - m_baselineOffset) / m_fontScaleMultiplier, num33);
					num34 = Mathf.Min((num32 - m_baselineOffset) / m_fontScaleMultiplier, num34);
				}
				m_maxLineAscender = Mathf.Max(num33, m_maxLineAscender);
				m_maxLineDescender = Mathf.Min(num34, m_maxLineDescender);
			}
			if (flag10 || !flag9)
			{
				m_textInfo.characterInfo[m_characterCount].adjustedAscender = num33;
				m_textInfo.characterInfo[m_characterCount].adjustedDescender = num34;
				m_ElementAscender = (m_textInfo.characterInfo[m_characterCount].ascender = num31 - m_lineOffset);
				m_ElementDescender = (m_textInfo.characterInfo[m_characterCount].descender = num32 - m_lineOffset);
			}
			else
			{
				m_textInfo.characterInfo[m_characterCount].adjustedAscender = m_maxLineAscender;
				m_textInfo.characterInfo[m_characterCount].adjustedDescender = m_maxLineDescender;
				m_ElementAscender = (m_textInfo.characterInfo[m_characterCount].ascender = m_maxLineAscender - m_lineOffset);
				m_ElementDescender = (m_textInfo.characterInfo[m_characterCount].descender = m_maxLineDescender - m_lineOffset);
			}
			if ((m_lineNumber == 0 || m_isNewPage) && (flag10 || !flag9))
			{
				m_maxTextAscender = m_maxLineAscender;
				m_maxCapHeight = Mathf.Max(m_maxCapHeight, m_currentFontAsset.m_FaceInfo.capLine * num2 / num16);
			}
			if (m_lineOffset == 0f && (flag10 || !flag9))
			{
				m_PageAscender = ((m_PageAscender > num31) ? m_PageAscender : num31);
			}
			m_textInfo.characterInfo[m_characterCount].isVisible = false;
			bool flag11 = (m_lineJustification & HorizontalAlignmentOptions.Flush) == HorizontalAlignmentOptions.Flush || (m_lineJustification & HorizontalAlignmentOptions.Justified) == HorizontalAlignmentOptions.Justified;
			if (num5 == 9 || (!flag9 && num5 != 8203 && num5 != 173 && num5 != 3) || (num5 == 173 && !flag7) || m_textElementType == TMP_TextElementType.Sprite)
			{
				m_textInfo.characterInfo[m_characterCount].isVisible = true;
				float marginLeft = m_marginLeft;
				float marginRight = m_marginRight;
				if (flag8)
				{
					marginLeft = m_textInfo.lineInfo[m_lineNumber].marginLeft;
					marginRight = m_textInfo.lineInfo[m_lineNumber].marginRight;
				}
				num13 = ((m_width != -1f) ? Mathf.Min(num11 + 0.0001f - marginLeft - marginRight, m_width) : (num11 + 0.0001f - marginLeft - marginRight));
				float num35 = Mathf.Abs(m_xAdvance) + ((!m_isRightToLeft) ? metrics.horizontalAdvance : 0f) * (1f - m_charWidthAdjDelta) * ((num5 == 173) ? num25 : num2);
				float num36 = m_maxTextAscender - (m_maxLineDescender - m_lineOffset) + ((m_lineOffset > 0f && !m_IsDrivenLineSpacing) ? (m_maxLineAscender - m_startOfLineAscender) : 0f);
				int characterCount = m_characterCount;
				if (num36 > num12 + 0.0001f)
				{
					if (m_firstOverflowCharacterIndex == -1)
					{
						m_firstOverflowCharacterIndex = m_characterCount;
					}
					if (m_enableAutoSizing)
					{
						if (m_lineSpacingDelta > m_lineSpacingMax && m_lineOffset > 0f && m_AutoSizeIterationCount < m_AutoSizeMaxIterationCount)
						{
							float num37 = (num12 - num36) / (float)m_lineNumber;
							m_lineSpacingDelta = Mathf.Max(m_lineSpacingDelta + num37 / num, m_lineSpacingMax);
							return;
						}
						if (m_fontSize > m_fontSizeMin && m_AutoSizeIterationCount < m_AutoSizeMaxIterationCount)
						{
							m_maxFontSize = m_fontSize;
							float num38 = Mathf.Max((m_fontSize - m_minFontSize) / 2f, 0.05f);
							m_fontSize -= num38;
							m_fontSize = Mathf.Max((float)(int)(m_fontSize * 20f + 0.5f) / 20f, m_fontSizeMin);
							return;
						}
					}
					switch (m_overflowMode)
					{
					case TextOverflowModes.Truncate:
						i = RestoreWordWrappingState(ref TMP_Text.m_SavedLastValidState);
						characterSubstitution.index = characterCount;
						characterSubstitution.unicode = 3u;
						continue;
					case TextOverflowModes.Ellipsis:
						if (TMP_Text.m_EllipsisInsertionCandidateStack.Count == 0)
						{
							i = -1;
							m_characterCount = 0;
							characterSubstitution.index = 0;
							characterSubstitution.unicode = 3u;
							m_firstCharacterOfLine = 0;
						}
						else
						{
							WordWrapState state = TMP_Text.m_EllipsisInsertionCandidateStack.Pop();
							i = RestoreWordWrappingState(ref state);
							i--;
							m_characterCount--;
							characterSubstitution.index = m_characterCount;
							characterSubstitution.unicode = 8230u;
							num15++;
						}
						continue;
					case TextOverflowModes.Linked:
						i = RestoreWordWrappingState(ref TMP_Text.m_SavedLastValidState);
						if (m_linkedTextComponent != null)
						{
							m_linkedTextComponent.text = text;
							m_linkedTextComponent.m_inputSource = m_inputSource;
							m_linkedTextComponent.firstVisibleCharacter = m_characterCount;
							m_linkedTextComponent.ForceMeshUpdate();
							m_isTextTruncated = true;
						}
						characterSubstitution.index = characterCount;
						characterSubstitution.unicode = 3u;
						continue;
					case TextOverflowModes.Page:
						if (i < 0 || characterCount == 0)
						{
							i = -1;
							m_characterCount = 0;
							characterSubstitution.index = 0;
							characterSubstitution.unicode = 3u;
							continue;
						}
						if (m_maxLineAscender - m_maxLineDescender > num12 + 0.0001f)
						{
							i = RestoreWordWrappingState(ref TMP_Text.m_SavedLineState);
							characterSubstitution.index = characterCount;
							characterSubstitution.unicode = 3u;
							continue;
						}
						i = RestoreWordWrappingState(ref TMP_Text.m_SavedLineState);
						m_isNewPage = true;
						m_firstCharacterOfLine = m_characterCount;
						m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
						m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
						m_startOfLineAscender = 0f;
						m_xAdvance = 0f + tag_Indent;
						m_lineOffset = 0f;
						m_maxTextAscender = 0f;
						m_PageAscender = 0f;
						m_lineNumber++;
						m_pageNumber++;
						continue;
					}
				}
				if (num35 > num13 * (flag11 ? 1.05f : 1f))
				{
					if (m_enableWordWrapping && m_characterCount != m_firstCharacterOfLine)
					{
						i = RestoreWordWrappingState(ref TMP_Text.m_SavedWordWrapState);
						float num39 = 0f;
						if (m_lineHeight == -32767f)
						{
							float adjustedAscender = m_textInfo.characterInfo[m_characterCount].adjustedAscender;
							num39 = ((m_lineOffset > 0f && !m_IsDrivenLineSpacing) ? (m_maxLineAscender - m_startOfLineAscender) : 0f) - m_maxLineDescender + adjustedAscender + (num9 + m_lineSpacingDelta) * num + m_lineSpacing * num3;
						}
						else
						{
							num39 = m_lineHeight + m_lineSpacing * num3;
							m_IsDrivenLineSpacing = true;
						}
						float num40 = m_maxTextAscender + num39 + m_lineOffset - m_textInfo.characterInfo[m_characterCount].adjustedDescender;
						if (m_textInfo.characterInfo[m_characterCount - 1].character == '\u00ad' && !flag7 && (m_overflowMode == TextOverflowModes.Overflow || num40 < num12 + 0.0001f))
						{
							characterSubstitution.index = m_characterCount - 1;
							characterSubstitution.unicode = 45u;
							i--;
							m_characterCount--;
							continue;
						}
						flag7 = false;
						if (m_textInfo.characterInfo[m_characterCount].character == '\u00ad')
						{
							flag7 = true;
							continue;
						}
						if (m_enableAutoSizing && flag5)
						{
							if (m_charWidthAdjDelta < m_charWidthMaxAdj / 100f && m_AutoSizeIterationCount < m_AutoSizeMaxIterationCount)
							{
								float num41 = num35;
								if (m_charWidthAdjDelta > 0f)
								{
									num41 /= 1f - m_charWidthAdjDelta;
								}
								float num42 = num35 - (num13 - 0.0001f) * (flag11 ? 1.05f : 1f);
								m_charWidthAdjDelta += num42 / num41;
								m_charWidthAdjDelta = Mathf.Min(m_charWidthAdjDelta, m_charWidthMaxAdj / 100f);
								return;
							}
							if (m_fontSize > m_fontSizeMin && m_AutoSizeIterationCount < m_AutoSizeMaxIterationCount)
							{
								m_maxFontSize = m_fontSize;
								float num43 = Mathf.Max((m_fontSize - m_minFontSize) / 2f, 0.05f);
								m_fontSize -= num43;
								m_fontSize = Mathf.Max((float)(int)(m_fontSize * 20f + 0.5f) / 20f, m_fontSizeMin);
								return;
							}
						}
						int previous_WordBreak = TMP_Text.m_SavedSoftLineBreakState.previous_WordBreak;
						if (flag5 && previous_WordBreak != -1 && previous_WordBreak != num14)
						{
							i = RestoreWordWrappingState(ref TMP_Text.m_SavedSoftLineBreakState);
							num14 = previous_WordBreak;
							if (m_textInfo.characterInfo[m_characterCount - 1].character == '\u00ad')
							{
								characterSubstitution.index = m_characterCount - 1;
								characterSubstitution.unicode = 45u;
								i--;
								m_characterCount--;
								continue;
							}
						}
						if (!(num40 > num12 + 0.0001f))
						{
							InsertNewLine(i, num, num2, num3, m_GlyphHorizontalAdvanceAdjustment, num8, num26, num13, num9, ref isMaxVisibleDescenderSet, ref maxVisibleDescender);
							flag4 = true;
							flag5 = true;
							continue;
						}
						if (m_firstOverflowCharacterIndex == -1)
						{
							m_firstOverflowCharacterIndex = m_characterCount;
						}
						if (m_enableAutoSizing)
						{
							if (m_lineSpacingDelta > m_lineSpacingMax && m_AutoSizeIterationCount < m_AutoSizeMaxIterationCount)
							{
								float num44 = (num12 - num40) / (float)(m_lineNumber + 1);
								m_lineSpacingDelta = Mathf.Max(m_lineSpacingDelta + num44 / num, m_lineSpacingMax);
								return;
							}
							if (m_charWidthAdjDelta < m_charWidthMaxAdj / 100f && m_AutoSizeIterationCount < m_AutoSizeMaxIterationCount)
							{
								float num45 = num35;
								if (m_charWidthAdjDelta > 0f)
								{
									num45 /= 1f - m_charWidthAdjDelta;
								}
								float num46 = num35 - (num13 - 0.0001f) * (flag11 ? 1.05f : 1f);
								m_charWidthAdjDelta += num46 / num45;
								m_charWidthAdjDelta = Mathf.Min(m_charWidthAdjDelta, m_charWidthMaxAdj / 100f);
								return;
							}
							if (m_fontSize > m_fontSizeMin && m_AutoSizeIterationCount < m_AutoSizeMaxIterationCount)
							{
								m_maxFontSize = m_fontSize;
								float num47 = Mathf.Max((m_fontSize - m_minFontSize) / 2f, 0.05f);
								m_fontSize -= num47;
								m_fontSize = Mathf.Max((float)(int)(m_fontSize * 20f + 0.5f) / 20f, m_fontSizeMin);
								return;
							}
						}
						switch (m_overflowMode)
						{
						case TextOverflowModes.Overflow:
						case TextOverflowModes.Masking:
						case TextOverflowModes.ScrollRect:
							InsertNewLine(i, num, num2, num3, m_GlyphHorizontalAdvanceAdjustment, num8, num26, num13, num9, ref isMaxVisibleDescenderSet, ref maxVisibleDescender);
							flag4 = true;
							flag5 = true;
							continue;
						case TextOverflowModes.Truncate:
							i = RestoreWordWrappingState(ref TMP_Text.m_SavedLastValidState);
							characterSubstitution.index = characterCount;
							characterSubstitution.unicode = 3u;
							continue;
						case TextOverflowModes.Ellipsis:
							if (TMP_Text.m_EllipsisInsertionCandidateStack.Count == 0)
							{
								i = -1;
								m_characterCount = 0;
								characterSubstitution.index = 0;
								characterSubstitution.unicode = 3u;
								m_firstCharacterOfLine = 0;
							}
							else
							{
								WordWrapState state2 = TMP_Text.m_EllipsisInsertionCandidateStack.Pop();
								i = RestoreWordWrappingState(ref state2);
								i--;
								m_characterCount--;
								characterSubstitution.index = m_characterCount;
								characterSubstitution.unicode = 8230u;
								num15++;
							}
							continue;
						case TextOverflowModes.Linked:
							if (m_linkedTextComponent != null)
							{
								m_linkedTextComponent.text = text;
								m_linkedTextComponent.m_inputSource = m_inputSource;
								m_linkedTextComponent.firstVisibleCharacter = m_characterCount;
								m_linkedTextComponent.ForceMeshUpdate();
								m_isTextTruncated = true;
							}
							characterSubstitution.index = m_characterCount;
							characterSubstitution.unicode = 3u;
							continue;
						case TextOverflowModes.Page:
							m_isNewPage = true;
							InsertNewLine(i, num, num2, num3, m_GlyphHorizontalAdvanceAdjustment, num8, num26, num13, num9, ref isMaxVisibleDescenderSet, ref maxVisibleDescender);
							m_startOfLineAscender = 0f;
							m_lineOffset = 0f;
							m_maxTextAscender = 0f;
							m_PageAscender = 0f;
							m_pageNumber++;
							flag4 = true;
							flag5 = true;
							continue;
						}
					}
					else
					{
						if (m_enableAutoSizing && m_AutoSizeIterationCount < m_AutoSizeMaxIterationCount)
						{
							if (m_charWidthAdjDelta < m_charWidthMaxAdj / 100f)
							{
								float num48 = num35;
								if (m_charWidthAdjDelta > 0f)
								{
									num48 /= 1f - m_charWidthAdjDelta;
								}
								float num49 = num35 - (num13 - 0.0001f) * (flag11 ? 1.05f : 1f);
								m_charWidthAdjDelta += num49 / num48;
								m_charWidthAdjDelta = Mathf.Min(m_charWidthAdjDelta, m_charWidthMaxAdj / 100f);
								return;
							}
							if (m_fontSize > m_fontSizeMin)
							{
								m_maxFontSize = m_fontSize;
								float num50 = Mathf.Max((m_fontSize - m_minFontSize) / 2f, 0.05f);
								m_fontSize -= num50;
								m_fontSize = Mathf.Max((float)(int)(m_fontSize * 20f + 0.5f) / 20f, m_fontSizeMin);
								return;
							}
						}
						switch (m_overflowMode)
						{
						case TextOverflowModes.Truncate:
							i = RestoreWordWrappingState(ref TMP_Text.m_SavedWordWrapState);
							characterSubstitution.index = characterCount;
							characterSubstitution.unicode = 3u;
							continue;
						case TextOverflowModes.Ellipsis:
							if (TMP_Text.m_EllipsisInsertionCandidateStack.Count == 0)
							{
								i = -1;
								m_characterCount = 0;
								characterSubstitution.index = 0;
								characterSubstitution.unicode = 3u;
								m_firstCharacterOfLine = 0;
							}
							else
							{
								WordWrapState state3 = TMP_Text.m_EllipsisInsertionCandidateStack.Pop();
								i = RestoreWordWrappingState(ref state3);
								i--;
								m_characterCount--;
								characterSubstitution.index = m_characterCount;
								characterSubstitution.unicode = 8230u;
								num15++;
							}
							continue;
						case TextOverflowModes.Linked:
							i = RestoreWordWrappingState(ref TMP_Text.m_SavedWordWrapState);
							if (m_linkedTextComponent != null)
							{
								m_linkedTextComponent.text = text;
								m_linkedTextComponent.m_inputSource = m_inputSource;
								m_linkedTextComponent.firstVisibleCharacter = m_characterCount;
								m_linkedTextComponent.ForceMeshUpdate();
								m_isTextTruncated = true;
							}
							characterSubstitution.index = m_characterCount;
							characterSubstitution.unicode = 3u;
							continue;
						}
					}
				}
				switch (num5)
				{
				case 9:
					m_textInfo.characterInfo[m_characterCount].isVisible = false;
					m_lastVisibleCharacterOfLine = m_characterCount;
					m_textInfo.lineInfo[m_lineNumber].spaceCount++;
					m_textInfo.spaceCount++;
					break;
				case 173:
					m_textInfo.characterInfo[m_characterCount].isVisible = false;
					break;
				default:
				{
					Color32 vertexColor = ((!m_overrideHtmlColors) ? m_htmlColor : m_fontColor32);
					if (m_textElementType == TMP_TextElementType.Character)
					{
						SaveGlyphVertexInfo(num6, num7, vertexColor);
					}
					else if (m_textElementType == TMP_TextElementType.Sprite)
					{
						SaveSpriteVertexInfo(vertexColor);
					}
					if (flag4)
					{
						flag4 = false;
						m_firstVisibleCharacterOfLine = m_characterCount;
					}
					m_lineVisibleCharacterCount++;
					m_lastVisibleCharacterOfLine = m_characterCount;
					m_textInfo.lineInfo[m_lineNumber].marginLeft = marginLeft;
					m_textInfo.lineInfo[m_lineNumber].marginRight = marginRight;
					break;
				}
				}
			}
			else
			{
				if (m_overflowMode == TextOverflowModes.Linked && (num5 == 10 || num5 == 11))
				{
					float num51 = m_maxTextAscender - (m_maxLineDescender - m_lineOffset) + ((m_lineOffset > 0f && !m_IsDrivenLineSpacing) ? (m_maxLineAscender - m_startOfLineAscender) : 0f);
					int characterCount2 = m_characterCount;
					if (num51 > num12 + 0.0001f)
					{
						if (m_firstOverflowCharacterIndex == -1)
						{
							m_firstOverflowCharacterIndex = m_characterCount;
						}
						i = RestoreWordWrappingState(ref TMP_Text.m_SavedLastValidState);
						if (m_linkedTextComponent != null)
						{
							m_linkedTextComponent.text = text;
							m_linkedTextComponent.m_inputSource = m_inputSource;
							m_linkedTextComponent.firstVisibleCharacter = m_characterCount;
							m_linkedTextComponent.ForceMeshUpdate();
							m_isTextTruncated = true;
						}
						characterSubstitution.index = characterCount2;
						characterSubstitution.unicode = 3u;
						continue;
					}
				}
				if ((num5 == 10 || num5 == 11 || num5 == 160 || num5 == 8199 || num5 == 8232 || num5 == 8233 || char.IsSeparator((char)num5)) && num5 != 173 && num5 != 8203 && num5 != 8288)
				{
					m_textInfo.lineInfo[m_lineNumber].spaceCount++;
					m_textInfo.spaceCount++;
				}
				if (num5 == 160)
				{
					m_textInfo.lineInfo[m_lineNumber].controlCharacterCount++;
				}
			}
			if (m_overflowMode == TextOverflowModes.Ellipsis && (!flag8 || num5 == 45))
			{
				float num52 = m_currentFontSize / (float)m_Ellipsis.fontAsset.m_FaceInfo.pointSize * m_Ellipsis.fontAsset.m_FaceInfo.scale * (m_isOrthographic ? 1f : 0.1f) * m_fontScaleMultiplier * m_Ellipsis.character.m_Scale * m_Ellipsis.character.m_Glyph.scale;
				float marginLeft2 = m_marginLeft;
				float marginRight2 = m_marginRight;
				if (num5 == 10 && m_characterCount != m_firstCharacterOfLine)
				{
					num52 = m_textInfo.characterInfo[m_characterCount - 1].pointSize / (float)m_Ellipsis.fontAsset.m_FaceInfo.pointSize * m_Ellipsis.fontAsset.m_FaceInfo.scale * (m_isOrthographic ? 1f : 0.1f) * m_fontScaleMultiplier * m_Ellipsis.character.m_Scale * m_Ellipsis.character.m_Glyph.scale;
					marginLeft2 = m_textInfo.lineInfo[m_lineNumber].marginLeft;
					marginRight2 = m_textInfo.lineInfo[m_lineNumber].marginRight;
				}
				float num53 = m_maxTextAscender - (m_maxLineDescender - m_lineOffset) + ((m_lineOffset > 0f && !m_IsDrivenLineSpacing) ? (m_maxLineAscender - m_startOfLineAscender) : 0f);
				float num54 = Mathf.Abs(m_xAdvance) + ((!m_isRightToLeft) ? m_Ellipsis.character.m_Glyph.metrics.horizontalAdvance : 0f) * (1f - m_charWidthAdjDelta) * num52;
				float num55 = ((m_width != -1f) ? Mathf.Min(num11 + 0.0001f - marginLeft2 - marginRight2, m_width) : (num11 + 0.0001f - marginLeft2 - marginRight2));
				if (num54 < num55 * (flag11 ? 1.05f : 1f) && num53 < num12 + 0.0001f)
				{
					SaveWordWrappingState(ref TMP_Text.m_SavedEllipsisState, i, m_characterCount);
					TMP_Text.m_EllipsisInsertionCandidateStack.Push(TMP_Text.m_SavedEllipsisState);
				}
			}
			m_textInfo.characterInfo[m_characterCount].lineNumber = m_lineNumber;
			m_textInfo.characterInfo[m_characterCount].pageNumber = m_pageNumber;
			if ((num5 != 10 && num5 != 11 && num5 != 13 && !flag8) || m_textInfo.lineInfo[m_lineNumber].characterCount == 1)
			{
				m_textInfo.lineInfo[m_lineNumber].alignment = m_lineJustification;
			}
			if (num5 == 9)
			{
				float num56 = m_currentFontAsset.m_FaceInfo.tabWidth * (float)(int)m_currentFontAsset.tabSize * num2;
				float num57 = Mathf.Ceil(m_xAdvance / num56) * num56;
				m_xAdvance = ((num57 > m_xAdvance) ? num57 : (m_xAdvance + num56));
			}
			else if (m_monoSpacing != 0f)
			{
				m_xAdvance += (m_monoSpacing - num27 + (m_currentFontAsset.normalSpacingOffset + num26) * num3 + m_cSpacing) * (1f - m_charWidthAdjDelta);
				if (flag9 || num5 == 8203)
				{
					m_xAdvance += m_wordSpacing * num3;
				}
			}
			else if (m_isRightToLeft)
			{
				m_xAdvance -= (tMP_GlyphValueRecord.m_XAdvance * num2 + (m_currentFontAsset.normalSpacingOffset + num26 + num8) * num3 + m_cSpacing) * (1f - m_charWidthAdjDelta);
				if (flag9 || num5 == 8203)
				{
					m_xAdvance -= m_wordSpacing * num3;
				}
			}
			else
			{
				float num58 = 1f;
				if (m_isFXMatrixSet)
				{
					num58 = m_FXMatrix.lossyScale.x;
				}
				m_xAdvance += ((metrics.horizontalAdvance * num58 + tMP_GlyphValueRecord.m_XAdvance) * num2 + (m_currentFontAsset.normalSpacingOffset + num26 + num8) * num3 + m_cSpacing) * (1f - m_charWidthAdjDelta);
				if (flag9 || num5 == 8203)
				{
					m_xAdvance += m_wordSpacing * num3;
				}
			}
			m_textInfo.characterInfo[m_characterCount].xAdvance = m_xAdvance;
			if (num5 == 13)
			{
				m_xAdvance = 0f + tag_Indent;
			}
			if (num5 == 10 || num5 == 11 || num5 == 3 || num5 == 8232 || num5 == 8233 || (num5 == 45 && flag8) || m_characterCount == totalCharacterCount - 1)
			{
				float num59 = m_maxLineAscender - m_startOfLineAscender;
				if (m_lineOffset > 0f && Math.Abs(num59) > 0.01f && !m_IsDrivenLineSpacing && !m_isNewPage)
				{
					AdjustLineOffset(m_firstCharacterOfLine, m_characterCount, num59);
					m_ElementDescender -= num59;
					m_lineOffset += num59;
					if (TMP_Text.m_SavedEllipsisState.lineNumber == m_lineNumber)
					{
						TMP_Text.m_SavedEllipsisState = TMP_Text.m_EllipsisInsertionCandidateStack.Pop();
						TMP_Text.m_SavedEllipsisState.startOfLineAscender += num59;
						TMP_Text.m_SavedEllipsisState.lineOffset += num59;
						TMP_Text.m_EllipsisInsertionCandidateStack.Push(TMP_Text.m_SavedEllipsisState);
					}
				}
				m_isNewPage = false;
				float num60 = m_maxLineAscender - m_lineOffset;
				float num61 = m_maxLineDescender - m_lineOffset;
				m_ElementDescender = ((m_ElementDescender < num61) ? m_ElementDescender : num61);
				if (!isMaxVisibleDescenderSet)
				{
					maxVisibleDescender = m_ElementDescender;
				}
				if (m_useMaxVisibleDescender && (m_characterCount >= m_maxVisibleCharacters || m_lineNumber >= m_maxVisibleLines))
				{
					isMaxVisibleDescenderSet = true;
				}
				m_textInfo.lineInfo[m_lineNumber].firstCharacterIndex = m_firstCharacterOfLine;
				m_textInfo.lineInfo[m_lineNumber].firstVisibleCharacterIndex = (m_firstVisibleCharacterOfLine = ((m_firstCharacterOfLine > m_firstVisibleCharacterOfLine) ? m_firstCharacterOfLine : m_firstVisibleCharacterOfLine));
				m_textInfo.lineInfo[m_lineNumber].lastCharacterIndex = (m_lastCharacterOfLine = m_characterCount);
				m_textInfo.lineInfo[m_lineNumber].lastVisibleCharacterIndex = (m_lastVisibleCharacterOfLine = ((m_lastVisibleCharacterOfLine < m_firstVisibleCharacterOfLine) ? m_firstVisibleCharacterOfLine : m_lastVisibleCharacterOfLine));
				m_textInfo.lineInfo[m_lineNumber].characterCount = m_textInfo.lineInfo[m_lineNumber].lastCharacterIndex - m_textInfo.lineInfo[m_lineNumber].firstCharacterIndex + 1;
				m_textInfo.lineInfo[m_lineNumber].visibleCharacterCount = m_lineVisibleCharacterCount;
				m_textInfo.lineInfo[m_lineNumber].lineExtents.min = new Vector2(m_textInfo.characterInfo[m_firstVisibleCharacterOfLine].bottomLeft.x, num61);
				m_textInfo.lineInfo[m_lineNumber].lineExtents.max = new Vector2(m_textInfo.characterInfo[m_lastVisibleCharacterOfLine].topRight.x, num60);
				m_textInfo.lineInfo[m_lineNumber].length = m_textInfo.lineInfo[m_lineNumber].lineExtents.max.x - num6 * num2;
				m_textInfo.lineInfo[m_lineNumber].width = num13;
				if (m_textInfo.lineInfo[m_lineNumber].characterCount == 1)
				{
					m_textInfo.lineInfo[m_lineNumber].alignment = m_lineJustification;
				}
				float num62 = ((m_currentFontAsset.normalSpacingOffset + num26 + num8) * num3 - m_cSpacing) * (1f - m_charWidthAdjDelta);
				if (m_textInfo.characterInfo[m_lastVisibleCharacterOfLine].isVisible)
				{
					m_textInfo.lineInfo[m_lineNumber].maxAdvance = m_textInfo.characterInfo[m_lastVisibleCharacterOfLine].xAdvance + (m_isRightToLeft ? num62 : (0f - num62));
				}
				else
				{
					m_textInfo.lineInfo[m_lineNumber].maxAdvance = m_textInfo.characterInfo[m_lastCharacterOfLine].xAdvance + (m_isRightToLeft ? num62 : (0f - num62));
				}
				m_textInfo.lineInfo[m_lineNumber].baseline = 0f - m_lineOffset;
				m_textInfo.lineInfo[m_lineNumber].ascender = num60;
				m_textInfo.lineInfo[m_lineNumber].descender = num61;
				m_textInfo.lineInfo[m_lineNumber].lineHeight = num60 - num61 + num9 * num;
				switch (num5)
				{
				case 10:
				case 11:
				case 45:
				case 8232:
				case 8233:
				{
					SaveWordWrappingState(ref TMP_Text.m_SavedLineState, i, m_characterCount);
					m_lineNumber++;
					flag4 = true;
					flag6 = false;
					flag5 = true;
					m_firstCharacterOfLine = m_characterCount + 1;
					m_lineVisibleCharacterCount = 0;
					if (m_lineNumber >= m_textInfo.lineInfo.Length)
					{
						ResizeLineExtents(m_lineNumber);
					}
					float adjustedAscender2 = m_textInfo.characterInfo[m_characterCount].adjustedAscender;
					if (m_lineHeight == -32767f)
					{
						float num63 = 0f - m_maxLineDescender + adjustedAscender2 + (num9 + m_lineSpacingDelta) * num + (m_lineSpacing + ((num5 == 10 || num5 == 8233) ? m_paragraphSpacing : 0f)) * num3;
						m_lineOffset += num63;
						m_IsDrivenLineSpacing = false;
					}
					else
					{
						m_lineOffset += m_lineHeight + (m_lineSpacing + ((num5 == 10 || num5 == 8233) ? m_paragraphSpacing : 0f)) * num3;
						m_IsDrivenLineSpacing = true;
					}
					m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
					m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
					m_startOfLineAscender = adjustedAscender2;
					m_xAdvance = 0f + tag_LineIndent + tag_Indent;
					SaveWordWrappingState(ref TMP_Text.m_SavedWordWrapState, i, m_characterCount);
					SaveWordWrappingState(ref TMP_Text.m_SavedLastValidState, i, m_characterCount);
					m_characterCount++;
					continue;
				}
				case 3:
					i = m_TextProcessingArray.Length;
					break;
				}
			}
			if (m_textInfo.characterInfo[m_characterCount].isVisible)
			{
				m_meshExtents.min.x = Mathf.Min(m_meshExtents.min.x, m_textInfo.characterInfo[m_characterCount].bottomLeft.x);
				m_meshExtents.min.y = Mathf.Min(m_meshExtents.min.y, m_textInfo.characterInfo[m_characterCount].bottomLeft.y);
				m_meshExtents.max.x = Mathf.Max(m_meshExtents.max.x, m_textInfo.characterInfo[m_characterCount].topRight.x);
				m_meshExtents.max.y = Mathf.Max(m_meshExtents.max.y, m_textInfo.characterInfo[m_characterCount].topRight.y);
			}
			if (m_overflowMode == TextOverflowModes.Page && num5 != 10 && num5 != 11 && num5 != 13 && num5 != 8232 && num5 != 8233)
			{
				if (m_pageNumber + 1 > m_textInfo.pageInfo.Length)
				{
					TMP_TextInfo.Resize(ref m_textInfo.pageInfo, m_pageNumber + 1, isBlockAllocated: true);
				}
				m_textInfo.pageInfo[m_pageNumber].ascender = m_PageAscender;
				m_textInfo.pageInfo[m_pageNumber].descender = ((m_ElementDescender < m_textInfo.pageInfo[m_pageNumber].descender) ? m_ElementDescender : m_textInfo.pageInfo[m_pageNumber].descender);
				if (m_pageNumber == 0 && m_characterCount == 0)
				{
					m_textInfo.pageInfo[m_pageNumber].firstCharacterIndex = m_characterCount;
				}
				else if (m_characterCount > 0 && m_pageNumber != m_textInfo.characterInfo[m_characterCount - 1].pageNumber)
				{
					m_textInfo.pageInfo[m_pageNumber - 1].lastCharacterIndex = m_characterCount - 1;
					m_textInfo.pageInfo[m_pageNumber].firstCharacterIndex = m_characterCount;
				}
				else if (m_characterCount == totalCharacterCount - 1)
				{
					m_textInfo.pageInfo[m_pageNumber].lastCharacterIndex = m_characterCount;
				}
			}
			if (m_enableWordWrapping || m_overflowMode == TextOverflowModes.Truncate || m_overflowMode == TextOverflowModes.Ellipsis || m_overflowMode == TextOverflowModes.Linked)
			{
				if ((flag9 || num5 == 8203 || num5 == 45 || num5 == 173) && (!m_isNonBreakingSpace || flag6) && num5 != 160 && num5 != 8199 && num5 != 8209 && num5 != 8239 && num5 != 8288)
				{
					SaveWordWrappingState(ref TMP_Text.m_SavedWordWrapState, i, m_characterCount);
					flag5 = false;
					TMP_Text.m_SavedSoftLineBreakState.previous_WordBreak = -1;
				}
				else if (!m_isNonBreakingSpace && ((((num5 > 4352 && num5 < 4607) || (num5 > 43360 && num5 < 43391) || (num5 > 44032 && num5 < 55295)) && !TMP_Settings.useModernHangulLineBreakingRules) || (num5 > 11904 && num5 < 40959) || (num5 > 63744 && num5 < 64255) || (num5 > 65072 && num5 < 65103) || (num5 > 65280 && num5 < 65519)))
				{
					bool num64 = TMP_Settings.linebreakingRules.leadingCharacters.ContainsKey(num5);
					bool flag12 = m_characterCount < totalCharacterCount - 1 && TMP_Settings.linebreakingRules.followingCharacters.ContainsKey(m_textInfo.characterInfo[m_characterCount + 1].character);
					if (!num64)
					{
						if (!flag12)
						{
							SaveWordWrappingState(ref TMP_Text.m_SavedWordWrapState, i, m_characterCount);
							flag5 = false;
						}
						if (flag5)
						{
							if (flag9)
							{
								SaveWordWrappingState(ref TMP_Text.m_SavedSoftLineBreakState, i, m_characterCount);
							}
							SaveWordWrappingState(ref TMP_Text.m_SavedWordWrapState, i, m_characterCount);
						}
					}
					else if (flag5 && flag10)
					{
						if (flag9)
						{
							SaveWordWrappingState(ref TMP_Text.m_SavedSoftLineBreakState, i, m_characterCount);
						}
						SaveWordWrappingState(ref TMP_Text.m_SavedWordWrapState, i, m_characterCount);
					}
				}
				else if (flag5)
				{
					if (flag9 || (num5 == 173 && !flag7))
					{
						SaveWordWrappingState(ref TMP_Text.m_SavedSoftLineBreakState, i, m_characterCount);
					}
					SaveWordWrappingState(ref TMP_Text.m_SavedWordWrapState, i, m_characterCount);
				}
			}
			SaveWordWrappingState(ref TMP_Text.m_SavedLastValidState, i, m_characterCount);
			m_characterCount++;
		}
		num4 = m_maxFontSize - m_minFontSize;
		if (m_enableAutoSizing && num4 > 0.051f && m_fontSize < m_fontSizeMax && m_AutoSizeIterationCount < m_AutoSizeMaxIterationCount)
		{
			if (m_charWidthAdjDelta < m_charWidthMaxAdj / 100f)
			{
				m_charWidthAdjDelta = 0f;
			}
			m_minFontSize = m_fontSize;
			float num65 = Mathf.Max((m_maxFontSize - m_fontSize) / 2f, 0.05f);
			m_fontSize += num65;
			m_fontSize = Mathf.Min((float)(int)(m_fontSize * 20f + 0.5f) / 20f, m_fontSizeMax);
			return;
		}
		m_IsAutoSizePointSizeSet = true;
		if (m_AutoSizeIterationCount >= m_AutoSizeMaxIterationCount)
		{
			Debug.Log("Auto Size Iteration Count: " + m_AutoSizeIterationCount + ". Final Point Size: " + m_fontSize);
		}
		if (m_characterCount == 0 || (m_characterCount == 1 && num5 == 3))
		{
			ClearMesh(updateMesh: true);
			TMPro_EventManager.ON_TEXT_CHANGED(this);
			return;
		}
		int index = TMP_Text.m_materialReferences[m_Underline.materialIndex].referenceCount * 4;
		m_textInfo.meshInfo[0].Clear(uploadChanges: false);
		Vector3 vector10 = Vector3.zero;
		Vector3[] rectTransformCorners = m_RectTransformCorners;
		switch (m_VerticalAlignment)
		{
		case VerticalAlignmentOptions.Top:
			vector10 = ((m_overflowMode == TextOverflowModes.Page) ? (rectTransformCorners[1] + new Vector3(0f + vector.x, 0f - m_textInfo.pageInfo[num10].ascender - vector.y, 0f)) : (rectTransformCorners[1] + new Vector3(0f + vector.x, 0f - m_maxTextAscender - vector.y, 0f)));
			break;
		case VerticalAlignmentOptions.Middle:
			vector10 = ((m_overflowMode == TextOverflowModes.Page) ? ((rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(0f + vector.x, 0f - (m_textInfo.pageInfo[num10].ascender + vector.y + m_textInfo.pageInfo[num10].descender - vector.w) / 2f, 0f)) : ((rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(0f + vector.x, 0f - (m_maxTextAscender + vector.y + maxVisibleDescender - vector.w) / 2f, 0f)));
			break;
		case VerticalAlignmentOptions.Bottom:
			vector10 = ((m_overflowMode == TextOverflowModes.Page) ? (rectTransformCorners[0] + new Vector3(0f + vector.x, 0f - m_textInfo.pageInfo[num10].descender + vector.w, 0f)) : (rectTransformCorners[0] + new Vector3(0f + vector.x, 0f - maxVisibleDescender + vector.w, 0f)));
			break;
		case VerticalAlignmentOptions.Baseline:
			vector10 = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(0f + vector.x, 0f, 0f);
			break;
		case VerticalAlignmentOptions.Geometry:
			vector10 = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(0f + vector.x, 0f - (m_meshExtents.max.y + vector.y + m_meshExtents.min.y - vector.w) / 2f, 0f);
			break;
		case VerticalAlignmentOptions.Capline:
			vector10 = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(0f + vector.x, 0f - (m_maxCapHeight - vector.y - vector.w) / 2f, 0f);
			break;
		}
		Vector3 vector11 = Vector3.zero;
		Vector3 zero3 = Vector3.zero;
		int index_X = 0;
		int index_X2 = 0;
		int num66 = 0;
		int lineCount = 0;
		int num67 = 0;
		bool flag13 = false;
		bool flag14 = false;
		int num68 = 0;
		int num69 = 0;
		float f = (m_previousLossyScaleY = transform.lossyScale.y);
		Color32 color = Color.white;
		Color32 underlineColor = Color.white;
		HighlightState highlightState = new HighlightState(new Color32(byte.MaxValue, byte.MaxValue, 0, 64), TMP_Offset.zero);
		float num70 = 0f;
		float num71 = 0f;
		float num72 = 0f;
		float num73 = 0f;
		float num74 = 0f;
		float num75 = TMP_Text.k_LargePositiveFloat;
		int num76 = 0;
		float num77 = 0f;
		float num78 = 0f;
		float b = 0f;
		TMP_CharacterInfo[] characterInfo = m_textInfo.characterInfo;
		for (int j = 0; j < m_characterCount; j++)
		{
			TMP_FontAsset fontAsset = characterInfo[j].fontAsset;
			char character = characterInfo[j].character;
			int lineNumber = characterInfo[j].lineNumber;
			TMP_LineInfo tMP_LineInfo = m_textInfo.lineInfo[lineNumber];
			lineCount = lineNumber + 1;
			HorizontalAlignmentOptions horizontalAlignmentOptions = tMP_LineInfo.alignment;
			switch (horizontalAlignmentOptions)
			{
			case HorizontalAlignmentOptions.Left:
				vector11 = (m_isRightToLeft ? new Vector3(0f - tMP_LineInfo.maxAdvance, 0f, 0f) : new Vector3(0f + tMP_LineInfo.marginLeft, 0f, 0f));
				break;
			case HorizontalAlignmentOptions.Center:
				vector11 = new Vector3(tMP_LineInfo.marginLeft + tMP_LineInfo.width / 2f - tMP_LineInfo.maxAdvance / 2f, 0f, 0f);
				break;
			case HorizontalAlignmentOptions.Geometry:
				vector11 = new Vector3(tMP_LineInfo.marginLeft + tMP_LineInfo.width / 2f - (tMP_LineInfo.lineExtents.min.x + tMP_LineInfo.lineExtents.max.x) / 2f, 0f, 0f);
				break;
			case HorizontalAlignmentOptions.Right:
				vector11 = (m_isRightToLeft ? new Vector3(tMP_LineInfo.marginLeft + tMP_LineInfo.width, 0f, 0f) : new Vector3(tMP_LineInfo.marginLeft + tMP_LineInfo.width - tMP_LineInfo.maxAdvance, 0f, 0f));
				break;
			case HorizontalAlignmentOptions.Justified:
			case HorizontalAlignmentOptions.Flush:
			{
				if (character == '\n' || character == '\u00ad' || character == '\u200b' || character == '\u2060' || character == '\u0003')
				{
					break;
				}
				char character2 = characterInfo[tMP_LineInfo.lastCharacterIndex].character;
				bool flag15 = (horizontalAlignmentOptions & HorizontalAlignmentOptions.Flush) == HorizontalAlignmentOptions.Flush;
				if ((!char.IsControl(character2) && lineNumber < m_lineNumber) || flag15 || tMP_LineInfo.maxAdvance > tMP_LineInfo.width)
				{
					if (lineNumber != num67 || j == 0 || j == m_firstVisibleCharacter)
					{
						vector11 = (m_isRightToLeft ? new Vector3(tMP_LineInfo.marginLeft + tMP_LineInfo.width, 0f, 0f) : new Vector3(tMP_LineInfo.marginLeft, 0f, 0f));
						flag13 = (char.IsSeparator(character) ? true : false);
						break;
					}
					float num79 = ((!m_isRightToLeft) ? (tMP_LineInfo.width - tMP_LineInfo.maxAdvance) : (tMP_LineInfo.width + tMP_LineInfo.maxAdvance));
					int num80 = tMP_LineInfo.visibleCharacterCount - 1 + tMP_LineInfo.controlCharacterCount;
					int num81 = (characterInfo[tMP_LineInfo.lastCharacterIndex].isVisible ? tMP_LineInfo.spaceCount : (tMP_LineInfo.spaceCount - 1)) - tMP_LineInfo.controlCharacterCount;
					if (flag13)
					{
						num81--;
						num80++;
					}
					float num82 = ((num81 > 0) ? m_wordWrappingRatios : 1f);
					if (num81 < 1)
					{
						num81 = 1;
					}
					if (character != '\u00a0' && (character == '\t' || char.IsSeparator(character)))
					{
						if (!m_isRightToLeft)
						{
							vector11 += new Vector3(num79 * (1f - num82) / (float)num81, 0f, 0f);
						}
						else
						{
							vector11 -= new Vector3(num79 * (1f - num82) / (float)num81, 0f, 0f);
						}
					}
					else if (!m_isRightToLeft)
					{
						vector11 += new Vector3(num79 * num82 / (float)num80, 0f, 0f);
					}
					else
					{
						vector11 -= new Vector3(num79 * num82 / (float)num80, 0f, 0f);
					}
				}
				else
				{
					vector11 = (m_isRightToLeft ? new Vector3(tMP_LineInfo.marginLeft + tMP_LineInfo.width, 0f, 0f) : new Vector3(tMP_LineInfo.marginLeft, 0f, 0f));
				}
				break;
			}
			}
			zero3 = vector10 + vector11;
			if (characterInfo[j].isVisible)
			{
				TMP_TextElementType elementType = characterInfo[j].elementType;
				switch (elementType)
				{
				case TMP_TextElementType.Character:
				{
					Extents lineExtents = tMP_LineInfo.lineExtents;
					float num83 = m_uvLineOffset * (float)lineNumber % 1f;
					switch (m_horizontalMapping)
					{
					case TextureMappingOptions.Character:
						characterInfo[j].vertex_BL.uv2.x = 0f;
						characterInfo[j].vertex_TL.uv2.x = 0f;
						characterInfo[j].vertex_TR.uv2.x = 1f;
						characterInfo[j].vertex_BR.uv2.x = 1f;
						break;
					case TextureMappingOptions.Line:
						if (m_textAlignment != TextAlignmentOptions.Justified)
						{
							characterInfo[j].vertex_BL.uv2.x = (characterInfo[j].vertex_BL.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num83;
							characterInfo[j].vertex_TL.uv2.x = (characterInfo[j].vertex_TL.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num83;
							characterInfo[j].vertex_TR.uv2.x = (characterInfo[j].vertex_TR.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num83;
							characterInfo[j].vertex_BR.uv2.x = (characterInfo[j].vertex_BR.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num83;
						}
						else
						{
							characterInfo[j].vertex_BL.uv2.x = (characterInfo[j].vertex_BL.position.x + vector11.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num83;
							characterInfo[j].vertex_TL.uv2.x = (characterInfo[j].vertex_TL.position.x + vector11.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num83;
							characterInfo[j].vertex_TR.uv2.x = (characterInfo[j].vertex_TR.position.x + vector11.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num83;
							characterInfo[j].vertex_BR.uv2.x = (characterInfo[j].vertex_BR.position.x + vector11.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num83;
						}
						break;
					case TextureMappingOptions.Paragraph:
						characterInfo[j].vertex_BL.uv2.x = (characterInfo[j].vertex_BL.position.x + vector11.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num83;
						characterInfo[j].vertex_TL.uv2.x = (characterInfo[j].vertex_TL.position.x + vector11.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num83;
						characterInfo[j].vertex_TR.uv2.x = (characterInfo[j].vertex_TR.position.x + vector11.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num83;
						characterInfo[j].vertex_BR.uv2.x = (characterInfo[j].vertex_BR.position.x + vector11.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num83;
						break;
					case TextureMappingOptions.MatchAspect:
					{
						switch (m_verticalMapping)
						{
						case TextureMappingOptions.Character:
							characterInfo[j].vertex_BL.uv2.y = 0f;
							characterInfo[j].vertex_TL.uv2.y = 1f;
							characterInfo[j].vertex_TR.uv2.y = 0f;
							characterInfo[j].vertex_BR.uv2.y = 1f;
							break;
						case TextureMappingOptions.Line:
							characterInfo[j].vertex_BL.uv2.y = (characterInfo[j].vertex_BL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num83;
							characterInfo[j].vertex_TL.uv2.y = (characterInfo[j].vertex_TL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num83;
							characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
							characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
							break;
						case TextureMappingOptions.Paragraph:
							characterInfo[j].vertex_BL.uv2.y = (characterInfo[j].vertex_BL.position.y - m_meshExtents.min.y) / (m_meshExtents.max.y - m_meshExtents.min.y) + num83;
							characterInfo[j].vertex_TL.uv2.y = (characterInfo[j].vertex_TL.position.y - m_meshExtents.min.y) / (m_meshExtents.max.y - m_meshExtents.min.y) + num83;
							characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
							characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
							break;
						case TextureMappingOptions.MatchAspect:
							Debug.Log("ERROR: Cannot Match both Vertical & Horizontal.");
							break;
						}
						float num84 = (1f - (characterInfo[j].vertex_BL.uv2.y + characterInfo[j].vertex_TL.uv2.y) * characterInfo[j].aspectRatio) / 2f;
						characterInfo[j].vertex_BL.uv2.x = characterInfo[j].vertex_BL.uv2.y * characterInfo[j].aspectRatio + num84 + num83;
						characterInfo[j].vertex_TL.uv2.x = characterInfo[j].vertex_BL.uv2.x;
						characterInfo[j].vertex_TR.uv2.x = characterInfo[j].vertex_TL.uv2.y * characterInfo[j].aspectRatio + num84 + num83;
						characterInfo[j].vertex_BR.uv2.x = characterInfo[j].vertex_TR.uv2.x;
						break;
					}
					}
					switch (m_verticalMapping)
					{
					case TextureMappingOptions.Character:
						characterInfo[j].vertex_BL.uv2.y = 0f;
						characterInfo[j].vertex_TL.uv2.y = 1f;
						characterInfo[j].vertex_TR.uv2.y = 1f;
						characterInfo[j].vertex_BR.uv2.y = 0f;
						break;
					case TextureMappingOptions.Line:
						characterInfo[j].vertex_BL.uv2.y = (characterInfo[j].vertex_BL.position.y - tMP_LineInfo.descender) / (tMP_LineInfo.ascender - tMP_LineInfo.descender);
						characterInfo[j].vertex_TL.uv2.y = (characterInfo[j].vertex_TL.position.y - tMP_LineInfo.descender) / (tMP_LineInfo.ascender - tMP_LineInfo.descender);
						characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
						characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
						break;
					case TextureMappingOptions.Paragraph:
						characterInfo[j].vertex_BL.uv2.y = (characterInfo[j].vertex_BL.position.y - m_meshExtents.min.y) / (m_meshExtents.max.y - m_meshExtents.min.y);
						characterInfo[j].vertex_TL.uv2.y = (characterInfo[j].vertex_TL.position.y - m_meshExtents.min.y) / (m_meshExtents.max.y - m_meshExtents.min.y);
						characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
						characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
						break;
					case TextureMappingOptions.MatchAspect:
					{
						float num85 = (1f - (characterInfo[j].vertex_BL.uv2.x + characterInfo[j].vertex_TR.uv2.x) / characterInfo[j].aspectRatio) / 2f;
						characterInfo[j].vertex_BL.uv2.y = num85 + characterInfo[j].vertex_BL.uv2.x / characterInfo[j].aspectRatio;
						characterInfo[j].vertex_TL.uv2.y = num85 + characterInfo[j].vertex_TR.uv2.x / characterInfo[j].aspectRatio;
						characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
						characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
						break;
					}
					}
					num70 = characterInfo[j].scale * Mathf.Abs(f) * (1f - m_charWidthAdjDelta);
					if (!characterInfo[j].isUsingAlternateTypeface && (characterInfo[j].style & FontStyles.Bold) == FontStyles.Bold)
					{
						num70 *= -1f;
					}
					float x = characterInfo[j].vertex_BL.uv2.x;
					float y = characterInfo[j].vertex_BL.uv2.y;
					float x2 = characterInfo[j].vertex_TR.uv2.x;
					float y2 = characterInfo[j].vertex_TR.uv2.y;
					float num86 = (int)x;
					float num87 = (int)y;
					x -= num86;
					x2 -= num86;
					y -= num87;
					y2 -= num87;
					characterInfo[j].vertex_BL.uv2.x = PackUV(x, y);
					characterInfo[j].vertex_BL.uv2.y = num70;
					characterInfo[j].vertex_TL.uv2.x = PackUV(x, y2);
					characterInfo[j].vertex_TL.uv2.y = num70;
					characterInfo[j].vertex_TR.uv2.x = PackUV(x2, y2);
					characterInfo[j].vertex_TR.uv2.y = num70;
					characterInfo[j].vertex_BR.uv2.x = PackUV(x2, y);
					characterInfo[j].vertex_BR.uv2.y = num70;
					break;
				}
				}
				if (j < m_maxVisibleCharacters && num66 < m_maxVisibleWords && lineNumber < m_maxVisibleLines && m_overflowMode != TextOverflowModes.Page)
				{
					characterInfo[j].vertex_BL.position += zero3;
					characterInfo[j].vertex_TL.position += zero3;
					characterInfo[j].vertex_TR.position += zero3;
					characterInfo[j].vertex_BR.position += zero3;
				}
				else if (j < m_maxVisibleCharacters && num66 < m_maxVisibleWords && lineNumber < m_maxVisibleLines && m_overflowMode == TextOverflowModes.Page && characterInfo[j].pageNumber == num10)
				{
					characterInfo[j].vertex_BL.position += zero3;
					characterInfo[j].vertex_TL.position += zero3;
					characterInfo[j].vertex_TR.position += zero3;
					characterInfo[j].vertex_BR.position += zero3;
				}
				else
				{
					characterInfo[j].vertex_BL.position = Vector3.zero;
					characterInfo[j].vertex_TL.position = Vector3.zero;
					characterInfo[j].vertex_TR.position = Vector3.zero;
					characterInfo[j].vertex_BR.position = Vector3.zero;
					characterInfo[j].isVisible = false;
				}
				switch (elementType)
				{
				case TMP_TextElementType.Character:
					FillCharacterVertexBuffers(j, index_X);
					break;
				case TMP_TextElementType.Sprite:
					FillSpriteVertexBuffers(j, index_X2);
					break;
				}
			}
			m_textInfo.characterInfo[j].bottomLeft += zero3;
			m_textInfo.characterInfo[j].topLeft += zero3;
			m_textInfo.characterInfo[j].topRight += zero3;
			m_textInfo.characterInfo[j].bottomRight += zero3;
			m_textInfo.characterInfo[j].origin += zero3.x;
			m_textInfo.characterInfo[j].xAdvance += zero3.x;
			m_textInfo.characterInfo[j].ascender += zero3.y;
			m_textInfo.characterInfo[j].descender += zero3.y;
			m_textInfo.characterInfo[j].baseLine += zero3.y;
			if (lineNumber != num67 || j == m_characterCount - 1)
			{
				if (lineNumber != num67)
				{
					m_textInfo.lineInfo[num67].baseline += zero3.y;
					m_textInfo.lineInfo[num67].ascender += zero3.y;
					m_textInfo.lineInfo[num67].descender += zero3.y;
					m_textInfo.lineInfo[num67].maxAdvance += zero3.x;
					m_textInfo.lineInfo[num67].lineExtents.min = new Vector2(m_textInfo.characterInfo[m_textInfo.lineInfo[num67].firstCharacterIndex].bottomLeft.x, m_textInfo.lineInfo[num67].descender);
					m_textInfo.lineInfo[num67].lineExtents.max = new Vector2(m_textInfo.characterInfo[m_textInfo.lineInfo[num67].lastVisibleCharacterIndex].topRight.x, m_textInfo.lineInfo[num67].ascender);
				}
				if (j == m_characterCount - 1)
				{
					m_textInfo.lineInfo[lineNumber].baseline += zero3.y;
					m_textInfo.lineInfo[lineNumber].ascender += zero3.y;
					m_textInfo.lineInfo[lineNumber].descender += zero3.y;
					m_textInfo.lineInfo[lineNumber].maxAdvance += zero3.x;
					m_textInfo.lineInfo[lineNumber].lineExtents.min = new Vector2(m_textInfo.characterInfo[m_textInfo.lineInfo[lineNumber].firstCharacterIndex].bottomLeft.x, m_textInfo.lineInfo[lineNumber].descender);
					m_textInfo.lineInfo[lineNumber].lineExtents.max = new Vector2(m_textInfo.characterInfo[m_textInfo.lineInfo[lineNumber].lastVisibleCharacterIndex].topRight.x, m_textInfo.lineInfo[lineNumber].ascender);
				}
			}
			if (char.IsLetterOrDigit(character) || character == '-' || character == '\u00ad' || character == '‐' || character == '‑')
			{
				if (!flag14)
				{
					flag14 = true;
					num68 = j;
				}
				if (flag14 && j == m_characterCount - 1)
				{
					int num88 = m_textInfo.wordInfo.Length;
					int wordCount = m_textInfo.wordCount;
					if (m_textInfo.wordCount + 1 > num88)
					{
						TMP_TextInfo.Resize(ref m_textInfo.wordInfo, num88 + 1);
					}
					num69 = j;
					m_textInfo.wordInfo[wordCount].firstCharacterIndex = num68;
					m_textInfo.wordInfo[wordCount].lastCharacterIndex = num69;
					m_textInfo.wordInfo[wordCount].characterCount = num69 - num68 + 1;
					m_textInfo.wordInfo[wordCount].textComponent = this;
					num66++;
					m_textInfo.wordCount++;
					m_textInfo.lineInfo[lineNumber].wordCount++;
				}
			}
			else if ((flag14 || (j == 0 && (!char.IsPunctuation(character) || char.IsWhiteSpace(character) || character == '\u200b' || j == m_characterCount - 1))) && (j <= 0 || j >= characterInfo.Length - 1 || j >= m_characterCount || (character != '\'' && character != '’') || !char.IsLetterOrDigit(characterInfo[j - 1].character) || !char.IsLetterOrDigit(characterInfo[j + 1].character)))
			{
				num69 = ((j == m_characterCount - 1 && char.IsLetterOrDigit(character)) ? j : (j - 1));
				flag14 = false;
				int num89 = m_textInfo.wordInfo.Length;
				int wordCount2 = m_textInfo.wordCount;
				if (m_textInfo.wordCount + 1 > num89)
				{
					TMP_TextInfo.Resize(ref m_textInfo.wordInfo, num89 + 1);
				}
				m_textInfo.wordInfo[wordCount2].firstCharacterIndex = num68;
				m_textInfo.wordInfo[wordCount2].lastCharacterIndex = num69;
				m_textInfo.wordInfo[wordCount2].characterCount = num69 - num68 + 1;
				m_textInfo.wordInfo[wordCount2].textComponent = this;
				num66++;
				m_textInfo.wordCount++;
				m_textInfo.lineInfo[lineNumber].wordCount++;
			}
			if ((m_textInfo.characterInfo[j].style & FontStyles.Underline) == FontStyles.Underline)
			{
				bool flag16 = true;
				int pageNumber = m_textInfo.characterInfo[j].pageNumber;
				m_textInfo.characterInfo[j].underlineVertexIndex = index;
				if (j > m_maxVisibleCharacters || lineNumber > m_maxVisibleLines || (m_overflowMode == TextOverflowModes.Page && pageNumber + 1 != m_pageToDisplay))
				{
					flag16 = false;
				}
				if (!char.IsWhiteSpace(character) && character != '\u200b')
				{
					num74 = Mathf.Max(num74, m_textInfo.characterInfo[j].scale);
					num71 = Mathf.Max(num71, Mathf.Abs(num70));
					num75 = Mathf.Min((pageNumber == num76) ? num75 : TMP_Text.k_LargePositiveFloat, m_textInfo.characterInfo[j].baseLine + base.font.m_FaceInfo.underlineOffset * num74);
					num76 = pageNumber;
				}
				if (!flag && flag16 && j <= tMP_LineInfo.lastVisibleCharacterIndex && character != '\n' && character != '\v' && character != '\r' && (j != tMP_LineInfo.lastVisibleCharacterIndex || !char.IsSeparator(character)))
				{
					flag = true;
					num72 = m_textInfo.characterInfo[j].scale;
					if (num74 == 0f)
					{
						num74 = num72;
						num71 = num70;
					}
					start = new Vector3(m_textInfo.characterInfo[j].bottomLeft.x, num75, 0f);
					color = m_textInfo.characterInfo[j].underlineColor;
				}
				if (flag && m_characterCount == 1)
				{
					flag = false;
					zero = new Vector3(m_textInfo.characterInfo[j].topRight.x, num75, 0f);
					num73 = m_textInfo.characterInfo[j].scale;
					DrawUnderlineMesh(start, zero, ref index, num72, num73, num74, num71, color);
					num74 = 0f;
					num71 = 0f;
					num75 = TMP_Text.k_LargePositiveFloat;
				}
				else if (flag && (j == tMP_LineInfo.lastCharacterIndex || j >= tMP_LineInfo.lastVisibleCharacterIndex))
				{
					if (char.IsWhiteSpace(character) || character == '\u200b')
					{
						int lastVisibleCharacterIndex = tMP_LineInfo.lastVisibleCharacterIndex;
						zero = new Vector3(m_textInfo.characterInfo[lastVisibleCharacterIndex].topRight.x, num75, 0f);
						num73 = m_textInfo.characterInfo[lastVisibleCharacterIndex].scale;
					}
					else
					{
						zero = new Vector3(m_textInfo.characterInfo[j].topRight.x, num75, 0f);
						num73 = m_textInfo.characterInfo[j].scale;
					}
					flag = false;
					DrawUnderlineMesh(start, zero, ref index, num72, num73, num74, num71, color);
					num74 = 0f;
					num71 = 0f;
					num75 = TMP_Text.k_LargePositiveFloat;
				}
				else if (flag && !flag16)
				{
					flag = false;
					zero = new Vector3(m_textInfo.characterInfo[j - 1].topRight.x, num75, 0f);
					num73 = m_textInfo.characterInfo[j - 1].scale;
					DrawUnderlineMesh(start, zero, ref index, num72, num73, num74, num71, color);
					num74 = 0f;
					num71 = 0f;
					num75 = TMP_Text.k_LargePositiveFloat;
				}
				else if (flag && j < m_characterCount - 1 && !color.Compare(m_textInfo.characterInfo[j + 1].underlineColor))
				{
					flag = false;
					zero = new Vector3(m_textInfo.characterInfo[j].topRight.x, num75, 0f);
					num73 = m_textInfo.characterInfo[j].scale;
					DrawUnderlineMesh(start, zero, ref index, num72, num73, num74, num71, color);
					num74 = 0f;
					num71 = 0f;
					num75 = TMP_Text.k_LargePositiveFloat;
				}
			}
			else if (flag)
			{
				flag = false;
				zero = new Vector3(m_textInfo.characterInfo[j - 1].topRight.x, num75, 0f);
				num73 = m_textInfo.characterInfo[j - 1].scale;
				DrawUnderlineMesh(start, zero, ref index, num72, num73, num74, num71, color);
				num74 = 0f;
				num71 = 0f;
				num75 = TMP_Text.k_LargePositiveFloat;
			}
			bool num90 = (m_textInfo.characterInfo[j].style & FontStyles.Strikethrough) == FontStyles.Strikethrough;
			float strikethroughOffset = fontAsset.m_FaceInfo.strikethroughOffset;
			if (num90)
			{
				bool flag17 = true;
				m_textInfo.characterInfo[j].strikethroughVertexIndex = index;
				if (j > m_maxVisibleCharacters || lineNumber > m_maxVisibleLines || (m_overflowMode == TextOverflowModes.Page && m_textInfo.characterInfo[j].pageNumber + 1 != m_pageToDisplay))
				{
					flag17 = false;
				}
				if (!flag2 && flag17 && j <= tMP_LineInfo.lastVisibleCharacterIndex && character != '\n' && character != '\v' && character != '\r' && (j != tMP_LineInfo.lastVisibleCharacterIndex || !char.IsSeparator(character)))
				{
					flag2 = true;
					num77 = m_textInfo.characterInfo[j].pointSize;
					num78 = m_textInfo.characterInfo[j].scale;
					start2 = new Vector3(m_textInfo.characterInfo[j].bottomLeft.x, m_textInfo.characterInfo[j].baseLine + strikethroughOffset * num78, 0f);
					underlineColor = m_textInfo.characterInfo[j].strikethroughColor;
					b = m_textInfo.characterInfo[j].baseLine;
				}
				if (flag2 && m_characterCount == 1)
				{
					flag2 = false;
					zero2 = new Vector3(m_textInfo.characterInfo[j].topRight.x, m_textInfo.characterInfo[j].baseLine + strikethroughOffset * num78, 0f);
					DrawUnderlineMesh(start2, zero2, ref index, num78, num78, num78, num70, underlineColor);
				}
				else if (flag2 && j == tMP_LineInfo.lastCharacterIndex)
				{
					if (char.IsWhiteSpace(character) || character == '\u200b')
					{
						int lastVisibleCharacterIndex2 = tMP_LineInfo.lastVisibleCharacterIndex;
						zero2 = new Vector3(m_textInfo.characterInfo[lastVisibleCharacterIndex2].topRight.x, m_textInfo.characterInfo[lastVisibleCharacterIndex2].baseLine + strikethroughOffset * num78, 0f);
					}
					else
					{
						zero2 = new Vector3(m_textInfo.characterInfo[j].topRight.x, m_textInfo.characterInfo[j].baseLine + strikethroughOffset * num78, 0f);
					}
					flag2 = false;
					DrawUnderlineMesh(start2, zero2, ref index, num78, num78, num78, num70, underlineColor);
				}
				else if (flag2 && j < m_characterCount && (m_textInfo.characterInfo[j + 1].pointSize != num77 || !TMP_Math.Approximately(m_textInfo.characterInfo[j + 1].baseLine + zero3.y, b)))
				{
					flag2 = false;
					int lastVisibleCharacterIndex3 = tMP_LineInfo.lastVisibleCharacterIndex;
					zero2 = ((j <= lastVisibleCharacterIndex3) ? new Vector3(m_textInfo.characterInfo[j].topRight.x, m_textInfo.characterInfo[j].baseLine + strikethroughOffset * num78, 0f) : new Vector3(m_textInfo.characterInfo[lastVisibleCharacterIndex3].topRight.x, m_textInfo.characterInfo[lastVisibleCharacterIndex3].baseLine + strikethroughOffset * num78, 0f));
					DrawUnderlineMesh(start2, zero2, ref index, num78, num78, num78, num70, underlineColor);
				}
				else if (flag2 && j < m_characterCount && fontAsset.GetInstanceID() != characterInfo[j + 1].fontAsset.GetInstanceID())
				{
					flag2 = false;
					zero2 = new Vector3(m_textInfo.characterInfo[j].topRight.x, m_textInfo.characterInfo[j].baseLine + strikethroughOffset * num78, 0f);
					DrawUnderlineMesh(start2, zero2, ref index, num78, num78, num78, num70, underlineColor);
				}
				else if (flag2 && !flag17)
				{
					flag2 = false;
					zero2 = new Vector3(m_textInfo.characterInfo[j - 1].topRight.x, m_textInfo.characterInfo[j - 1].baseLine + strikethroughOffset * num78, 0f);
					DrawUnderlineMesh(start2, zero2, ref index, num78, num78, num78, num70, underlineColor);
				}
			}
			else if (flag2)
			{
				flag2 = false;
				zero2 = new Vector3(m_textInfo.characterInfo[j - 1].topRight.x, m_textInfo.characterInfo[j - 1].baseLine + strikethroughOffset * num78, 0f);
				DrawUnderlineMesh(start2, zero2, ref index, num78, num78, num78, num70, underlineColor);
			}
			if ((m_textInfo.characterInfo[j].style & FontStyles.Highlight) == FontStyles.Highlight)
			{
				bool flag18 = true;
				int pageNumber2 = m_textInfo.characterInfo[j].pageNumber;
				if (j > m_maxVisibleCharacters || lineNumber > m_maxVisibleLines || (m_overflowMode == TextOverflowModes.Page && pageNumber2 + 1 != m_pageToDisplay))
				{
					flag18 = false;
				}
				if (!flag3 && flag18 && j <= tMP_LineInfo.lastVisibleCharacterIndex && character != '\n' && character != '\v' && character != '\r' && (j != tMP_LineInfo.lastVisibleCharacterIndex || !char.IsSeparator(character)))
				{
					flag3 = true;
					start3 = TMP_Text.k_LargePositiveVector2;
					end = TMP_Text.k_LargeNegativeVector2;
					highlightState = m_textInfo.characterInfo[j].highlightState;
				}
				if (flag3)
				{
					TMP_CharacterInfo tMP_CharacterInfo = m_textInfo.characterInfo[j];
					HighlightState highlightState2 = tMP_CharacterInfo.highlightState;
					bool flag19 = false;
					if (highlightState != tMP_CharacterInfo.highlightState)
					{
						end.x = (end.x - highlightState.padding.right + tMP_CharacterInfo.bottomLeft.x) / 2f;
						start3.y = Mathf.Min(start3.y, tMP_CharacterInfo.descender);
						end.y = Mathf.Max(end.y, tMP_CharacterInfo.ascender);
						DrawTextHighlight(start3, end, ref index, highlightState.color);
						flag3 = true;
						start3 = new Vector2(end.x, tMP_CharacterInfo.descender - highlightState2.padding.bottom);
						end = new Vector2(tMP_CharacterInfo.topRight.x + highlightState2.padding.right, tMP_CharacterInfo.ascender + highlightState2.padding.top);
						highlightState = tMP_CharacterInfo.highlightState;
						flag19 = true;
					}
					if (!flag19)
					{
						start3.x = Mathf.Min(start3.x, tMP_CharacterInfo.bottomLeft.x - highlightState.padding.left);
						start3.y = Mathf.Min(start3.y, tMP_CharacterInfo.descender - highlightState.padding.bottom);
						end.x = Mathf.Max(end.x, tMP_CharacterInfo.topRight.x + highlightState.padding.right);
						end.y = Mathf.Max(end.y, tMP_CharacterInfo.ascender + highlightState.padding.top);
					}
				}
				if (flag3 && m_characterCount == 1)
				{
					flag3 = false;
					DrawTextHighlight(start3, end, ref index, highlightState.color);
				}
				else if (flag3 && (j == tMP_LineInfo.lastCharacterIndex || j >= tMP_LineInfo.lastVisibleCharacterIndex))
				{
					flag3 = false;
					DrawTextHighlight(start3, end, ref index, highlightState.color);
				}
				else if (flag3 && !flag18)
				{
					flag3 = false;
					DrawTextHighlight(start3, end, ref index, highlightState.color);
				}
			}
			else if (flag3)
			{
				flag3 = false;
				DrawTextHighlight(start3, end, ref index, highlightState.color);
			}
			num67 = lineNumber;
		}
		m_textInfo.characterCount = m_characterCount;
		m_textInfo.spriteCount = m_spriteCount;
		m_textInfo.lineCount = lineCount;
		m_textInfo.wordCount = ((num66 == 0 || m_characterCount <= 0) ? 1 : num66);
		m_textInfo.pageCount = m_pageNumber + 1;
		if (m_renderMode == TextRenderFlags.Render && IsActive())
		{
			OnPreRenderText?.Invoke(m_textInfo);
			if (m_geometrySortingOrder != VertexSortingOrder.Normal)
			{
				m_textInfo.meshInfo[0].SortGeometry(VertexSortingOrder.Reverse);
			}
			m_mesh.MarkDynamic();
			m_mesh.vertices = m_textInfo.meshInfo[0].vertices;
			m_mesh.uv = m_textInfo.meshInfo[0].uvs0;
			m_mesh.uv2 = m_textInfo.meshInfo[0].uvs2;
			m_mesh.colors32 = m_textInfo.meshInfo[0].colors32;
			m_mesh.RecalculateBounds();
			for (int k = 1; k < m_textInfo.materialCount; k++)
			{
				m_textInfo.meshInfo[k].ClearUnusedVertices();
				if (!(m_subTextObjects[k] == null))
				{
					if (m_geometrySortingOrder != VertexSortingOrder.Normal)
					{
						m_textInfo.meshInfo[k].SortGeometry(VertexSortingOrder.Reverse);
					}
					m_subTextObjects[k].mesh.vertices = m_textInfo.meshInfo[k].vertices;
					m_subTextObjects[k].mesh.uv = m_textInfo.meshInfo[k].uvs0;
					m_subTextObjects[k].mesh.uv2 = m_textInfo.meshInfo[k].uvs2;
					m_subTextObjects[k].mesh.colors32 = m_textInfo.meshInfo[k].colors32;
					m_subTextObjects[k].mesh.RecalculateBounds();
				}
			}
		}
		TMPro_EventManager.ON_TEXT_CHANGED(this);
	}

	protected override Vector3[] GetTextContainerLocalCorners()
	{
		if (m_rectTransform == null)
		{
			m_rectTransform = base.rectTransform;
		}
		m_rectTransform.GetLocalCorners(m_RectTransformCorners);
		return m_RectTransformCorners;
	}

	private void SetMeshFilters(bool state)
	{
		if (m_meshFilter != null)
		{
			if (state)
			{
				m_meshFilter.sharedMesh = m_mesh;
			}
			else
			{
				m_meshFilter.sharedMesh = null;
			}
		}
		for (int i = 1; i < m_subTextObjects.Length && m_subTextObjects[i] != null; i++)
		{
			if (m_subTextObjects[i].meshFilter != null)
			{
				if (state)
				{
					m_subTextObjects[i].meshFilter.sharedMesh = m_subTextObjects[i].mesh;
				}
				else
				{
					m_subTextObjects[i].meshFilter.sharedMesh = null;
				}
			}
		}
	}

	protected override void SetActiveSubMeshes(bool state)
	{
		for (int i = 1; i < m_subTextObjects.Length && m_subTextObjects[i] != null; i++)
		{
			if (m_subTextObjects[i].enabled != state)
			{
				m_subTextObjects[i].enabled = state;
			}
		}
	}

	protected void SetActiveSubTextObjectRenderers(bool state)
	{
		for (int i = 1; i < m_subTextObjects.Length && m_subTextObjects[i] != null; i++)
		{
			Renderer renderer = m_subTextObjects[i].renderer;
			if (renderer != null && renderer.enabled != state)
			{
				renderer.enabled = state;
			}
		}
	}

	protected override void DestroySubMeshObjects()
	{
		for (int i = 1; i < m_subTextObjects.Length && m_subTextObjects[i] != null; i++)
		{
			UnityEngine.Object.DestroyImmediate(m_subTextObjects[i]);
		}
	}

	internal void UpdateSubMeshSortingLayerID(int id)
	{
		for (int i = 1; i < m_subTextObjects.Length; i++)
		{
			TMP_SubMesh tMP_SubMesh = m_subTextObjects[i];
			if (tMP_SubMesh != null && tMP_SubMesh.renderer != null)
			{
				tMP_SubMesh.renderer.sortingLayerID = id;
			}
		}
	}

	internal void UpdateSubMeshSortingOrder(int order)
	{
		for (int i = 1; i < m_subTextObjects.Length; i++)
		{
			TMP_SubMesh tMP_SubMesh = m_subTextObjects[i];
			if (tMP_SubMesh != null && tMP_SubMesh.renderer != null)
			{
				tMP_SubMesh.renderer.sortingOrder = order;
			}
		}
	}

	protected override Bounds GetCompoundBounds()
	{
		Bounds bounds = m_mesh.bounds;
		Vector3 min = bounds.min;
		Vector3 max = bounds.max;
		for (int i = 1; i < m_subTextObjects.Length && m_subTextObjects[i] != null; i++)
		{
			Bounds bounds2 = m_subTextObjects[i].mesh.bounds;
			min.x = ((min.x < bounds2.min.x) ? min.x : bounds2.min.x);
			min.y = ((min.y < bounds2.min.y) ? min.y : bounds2.min.y);
			max.x = ((max.x > bounds2.max.x) ? max.x : bounds2.max.x);
			max.y = ((max.y > bounds2.max.y) ? max.y : bounds2.max.y);
		}
		Vector3 center = (min + max) / 2f;
		Vector2 vector = max - min;
		return new Bounds(center, vector);
	}

	private void UpdateSDFScale(float scaleDelta)
	{
		if (scaleDelta == 0f || scaleDelta == float.PositiveInfinity || scaleDelta == float.NegativeInfinity)
		{
			m_havePropertiesChanged = true;
			OnPreRenderObject();
			return;
		}
		for (int i = 0; i < m_textInfo.materialCount; i++)
		{
			TMP_MeshInfo tMP_MeshInfo = m_textInfo.meshInfo[i];
			for (int j = 0; j < tMP_MeshInfo.uvs2.Length; j++)
			{
				tMP_MeshInfo.uvs2[j].y *= Mathf.Abs(scaleDelta);
			}
		}
		for (int k = 0; k < m_textInfo.meshInfo.Length; k++)
		{
			if (k == 0)
			{
				m_mesh.uv2 = m_textInfo.meshInfo[0].uvs2;
			}
			else
			{
				m_subTextObjects[k].mesh.uv2 = m_textInfo.meshInfo[k].uvs2;
			}
		}
	}

	public void SetMask(MaskingTypes type, Vector4 maskCoords)
	{
		SetMask(type);
		SetMaskCoordinates(maskCoords);
	}

	public void SetMask(MaskingTypes type, Vector4 maskCoords, float softnessX, float softnessY)
	{
		SetMask(type);
		SetMaskCoordinates(maskCoords, softnessX, softnessY);
	}

	public override void SetVerticesDirty()
	{
		if (!(this == null) && IsActive())
		{
			TMP_UpdateManager.RegisterTextElementForGraphicRebuild(this);
		}
	}

	public override void SetLayoutDirty()
	{
		m_isPreferredWidthDirty = true;
		m_isPreferredHeightDirty = true;
		if (!(this == null) && IsActive())
		{
			LayoutRebuilder.MarkLayoutForRebuild(base.rectTransform);
			m_isLayoutDirty = true;
		}
	}

	public override void SetMaterialDirty()
	{
		UpdateMaterial();
	}

	public override void SetAllDirty()
	{
		SetLayoutDirty();
		SetVerticesDirty();
		SetMaterialDirty();
	}

	public override void Rebuild(CanvasUpdate update)
	{
		if (this == null)
		{
			return;
		}
		switch (update)
		{
		case CanvasUpdate.Prelayout:
			if (m_autoSizeTextContainer)
			{
				m_rectTransform.sizeDelta = GetPreferredValues(float.PositiveInfinity, float.PositiveInfinity);
			}
			break;
		case CanvasUpdate.PreRender:
			OnPreRenderObject();
			if (m_isMaterialDirty)
			{
				UpdateMaterial();
				m_isMaterialDirty = false;
			}
			break;
		}
	}

	protected override void UpdateMaterial()
	{
		if (!(renderer == null) && !(m_sharedMaterial == null) && (m_renderer.sharedMaterial == null || m_renderer.sharedMaterial.GetInstanceID() != m_sharedMaterial.GetInstanceID()))
		{
			m_renderer.sharedMaterial = m_sharedMaterial;
		}
	}

	public override void UpdateMeshPadding()
	{
		m_padding = ShaderUtilities.GetPadding(m_sharedMaterial, m_enableExtraPadding, m_isUsingBold);
		m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(m_sharedMaterial);
		m_havePropertiesChanged = true;
		checkPaddingRequired = false;
		if (m_textInfo != null)
		{
			for (int i = 1; i < m_textInfo.materialCount; i++)
			{
				m_subTextObjects[i].UpdateMeshPadding(m_enableExtraPadding, m_isUsingBold);
			}
		}
	}

	public override void ForceMeshUpdate(bool ignoreActiveState = false, bool forceTextReparsing = false)
	{
		m_havePropertiesChanged = true;
		m_ignoreActiveState = ignoreActiveState;
		OnPreRenderObject();
	}

	public override TMP_TextInfo GetTextInfo(string text)
	{
		SetText(text);
		SetArraySizes(m_TextProcessingArray);
		m_renderMode = TextRenderFlags.DontRender;
		ComputeMarginSize();
		GenerateTextMesh();
		m_renderMode = TextRenderFlags.Render;
		return base.textInfo;
	}

	public override void ClearMesh(bool updateMesh)
	{
		if (m_textInfo.meshInfo[0].mesh == null)
		{
			m_textInfo.meshInfo[0].mesh = m_mesh;
		}
		m_textInfo.ClearMeshInfo(updateMesh);
	}

	public override void UpdateGeometry(Mesh mesh, int index)
	{
		mesh.RecalculateBounds();
	}

	public override void UpdateVertexData(TMP_VertexDataUpdateFlags flags)
	{
		int materialCount = m_textInfo.materialCount;
		for (int i = 0; i < materialCount; i++)
		{
			Mesh mesh = ((i != 0) ? m_subTextObjects[i].mesh : m_mesh);
			if ((flags & TMP_VertexDataUpdateFlags.Vertices) == TMP_VertexDataUpdateFlags.Vertices)
			{
				mesh.vertices = m_textInfo.meshInfo[i].vertices;
			}
			if ((flags & TMP_VertexDataUpdateFlags.Uv0) == TMP_VertexDataUpdateFlags.Uv0)
			{
				mesh.uv = m_textInfo.meshInfo[i].uvs0;
			}
			if ((flags & TMP_VertexDataUpdateFlags.Uv2) == TMP_VertexDataUpdateFlags.Uv2)
			{
				mesh.uv2 = m_textInfo.meshInfo[i].uvs2;
			}
			if ((flags & TMP_VertexDataUpdateFlags.Colors32) == TMP_VertexDataUpdateFlags.Colors32)
			{
				mesh.colors32 = m_textInfo.meshInfo[i].colors32;
			}
			mesh.RecalculateBounds();
		}
	}

	public override void UpdateVertexData()
	{
		int materialCount = m_textInfo.materialCount;
		for (int i = 0; i < materialCount; i++)
		{
			Mesh mesh;
			if (i == 0)
			{
				mesh = m_mesh;
			}
			else
			{
				m_textInfo.meshInfo[i].ClearUnusedVertices();
				mesh = m_subTextObjects[i].mesh;
			}
			mesh.vertices = m_textInfo.meshInfo[i].vertices;
			mesh.uv = m_textInfo.meshInfo[i].uvs0;
			mesh.uv2 = m_textInfo.meshInfo[i].uvs2;
			mesh.colors32 = m_textInfo.meshInfo[i].colors32;
			mesh.RecalculateBounds();
		}
	}

	public void UpdateFontAsset()
	{
		LoadFontAsset();
	}

	public void CalculateLayoutInputHorizontal()
	{
	}

	public void CalculateLayoutInputVertical()
	{
	}
}
