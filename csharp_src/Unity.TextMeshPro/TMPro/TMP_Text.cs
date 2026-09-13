using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using FixRTLTMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TextCore;
using UnityEngine.UI;

namespace TMPro;

public abstract class TMP_Text : MaskableGraphic
{
	protected struct CharacterSubstitution
	{
		public int index;

		public uint unicode;

		public CharacterSubstitution(int index, uint unicode)
		{
			this.index = index;
			this.unicode = unicode;
		}
	}

	internal enum TextInputSources
	{
		TextInputBox,
		SetText,
		SetTextArray,
		TextString
	}

	protected struct InputHtmlTagData
	{
		public int index;

		public string tagStr;
	}

	protected struct InputHtmlRange
	{
		public int startIndex;

		public int endIndex;

		public bool isArabicFixIgnore;
	}

	internal class EmojiSpriteInfo
	{
		public int index;

		public TMP_SpriteAsset spriteAsset;
	}

	[DebuggerDisplay("Unicode ({unicode})  '{(char)unicode}'")]
	internal struct UnicodeChar
	{
		public int unicode;

		public int stringIndex;

		public int length;

		public EmojiSpriteInfo emojiSpriteInfo;
	}

	protected struct SpecialCharacter
	{
		public TMP_Character character;

		public TMP_FontAsset fontAsset;

		public Material material;

		public int materialIndex;

		public SpecialCharacter(TMP_Character character, int materialIndex)
		{
			this.character = character;
			fontAsset = character.textAsset as TMP_FontAsset;
			material = ((fontAsset != null) ? fontAsset.material : null);
			this.materialIndex = materialIndex;
		}
	}

	private struct TextBackingContainer
	{
		private uint[] m_Array;

		private int m_Count;

		public int Capacity => m_Array.Length;

		public int Count
		{
			get
			{
				return m_Count;
			}
			set
			{
				m_Count = value;
			}
		}

		public uint this[int index]
		{
			get
			{
				return m_Array[index];
			}
			set
			{
				if (index >= m_Array.Length)
				{
					Resize(index);
				}
				m_Array[index] = value;
			}
		}

		public TextBackingContainer(int size)
		{
			m_Array = new uint[size];
			m_Count = 0;
		}

		public void Resize(int size)
		{
			size = Mathf.NextPowerOfTwo(size + 1);
			Array.Resize(ref m_Array, size);
		}
	}

	protected struct TashkeelLocation
	{
		public uint Tashkeel;

		public int Position;

		public uint Len;

		public TashkeelLocation(uint tashkeel, int position, uint len)
		{
			Tashkeel = tashkeel;
			Position = position;
			Len = len;
		}
	}

	private struct OutPutTxt
	{
		public uint Char;

		public uint Len;

		public OutPutTxt(uint charP, uint len)
		{
			Char = charP;
			Len = len;
		}
	}

	[SerializeField]
	[TextArea(5, 10)]
	protected string m_text;

	private bool m_IsTextBackingStringDirty;

	[SerializeField]
	protected ITextPreprocessor m_TextPreprocessor;

	[SerializeField]
	protected bool m_isRightToLeft;

	[SerializeField]
	protected TMP_FontAsset m_fontAsset;

	protected TMP_FontAsset m_currentFontAsset;

	protected bool m_isSDFShader;

	[SerializeField]
	protected Material m_sharedMaterial;

	protected Material m_currentMaterial;

	protected static MaterialReference[] m_materialReferences = new MaterialReference[4];

	protected static Dictionary<int, int> m_materialReferenceIndexLookup = new Dictionary<int, int>();

	protected static TMP_TextProcessingStack<MaterialReference> m_materialReferenceStack = new TMP_TextProcessingStack<MaterialReference>(new MaterialReference[16]);

	protected int m_currentMaterialIndex;

	[SerializeField]
	protected Material[] m_fontSharedMaterials;

	[SerializeField]
	protected Material m_fontMaterial;

	[SerializeField]
	protected Material[] m_fontMaterials;

	protected bool m_isMaterialDirty;

	[SerializeField]
	protected Color32 m_fontColor32 = Color.white;

	[SerializeField]
	protected Color m_fontColor = Color.white;

	protected static Color32 s_colorWhite = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	protected Color32 m_underlineColor = s_colorWhite;

	protected Color32 m_strikethroughColor = s_colorWhite;

	[SerializeField]
	protected bool m_enableVertexGradient;

	[SerializeField]
	protected ColorMode m_colorMode = ColorMode.FourCornersGradient;

	[SerializeField]
	protected VertexGradient m_fontColorGradient = new VertexGradient(Color.white);

	[SerializeField]
	protected TMP_ColorGradient m_fontColorGradientPreset;

	[SerializeField]
	protected TMP_SpriteAsset m_spriteAsset;

	[SerializeField]
	protected bool m_tintAllSprites;

	protected bool m_tintSprite;

	protected Color32 m_spriteColor;

	[SerializeField]
	protected TMP_StyleSheet m_StyleSheet;

	internal TMP_Style m_TextStyle;

	[SerializeField]
	protected int m_TextStyleHashCode;

	[SerializeField]
	protected bool m_overrideHtmlColors;

	[SerializeField]
	protected Color32 m_faceColor = Color.white;

	protected Color32 m_outlineColor = Color.black;

	protected float m_outlineWidth;

	[SerializeField]
	protected float m_fontSize = -99f;

	protected float m_currentFontSize;

	[SerializeField]
	protected float m_fontSizeBase = 36f;

	protected TMP_TextProcessingStack<float> m_sizeStack = new TMP_TextProcessingStack<float>(16);

	[SerializeField]
	protected FontWeight m_fontWeight = FontWeight.Regular;

	protected FontWeight m_FontWeightInternal = FontWeight.Regular;

	protected TMP_TextProcessingStack<FontWeight> m_FontWeightStack = new TMP_TextProcessingStack<FontWeight>(8);

	[SerializeField]
	protected bool m_enableAutoSizing;

	protected float m_maxFontSize;

	protected float m_minFontSize;

	protected int m_AutoSizeIterationCount;

	protected int m_AutoSizeMaxIterationCount = 100;

	protected bool m_IsAutoSizePointSizeSet;

	protected int m_fontFillingUint;

	[SerializeField]
	protected float m_fontSizeMin;

	[SerializeField]
	protected float m_fontSizeMax;

	[SerializeField]
	protected FontStyles m_fontStyle;

	protected FontStyles m_FontStyleInternal;

	protected TMP_FontStyleStack m_fontStyleStack;

	protected bool m_isUsingBold;

	[SerializeField]
	protected HorizontalAlignmentOptions m_HorizontalAlignment = HorizontalAlignmentOptions.Left;

	[SerializeField]
	protected VerticalAlignmentOptions m_VerticalAlignment = VerticalAlignmentOptions.Top;

	[SerializeField]
	[FormerlySerializedAs("m_lineJustification")]
	protected TextAlignmentOptions m_textAlignment = TextAlignmentOptions.Converted;

	protected HorizontalAlignmentOptions m_lineJustification;

	protected TMP_TextProcessingStack<HorizontalAlignmentOptions> m_lineJustificationStack = new TMP_TextProcessingStack<HorizontalAlignmentOptions>(new HorizontalAlignmentOptions[16]);

	protected Vector3[] m_textContainerLocalCorners = new Vector3[4];

	[SerializeField]
	protected float m_characterSpacing;

	protected float m_cSpacing;

	protected float m_monoSpacing;

	[SerializeField]
	protected float m_wordSpacing;

	[SerializeField]
	protected float m_lineSpacing;

	protected float m_lineSpacingDelta;

	protected float m_lineHeight = -32767f;

	protected bool m_IsDrivenLineSpacing;

	[SerializeField]
	protected float m_lineSpacingMax;

	[SerializeField]
	protected float m_paragraphSpacing;

	[SerializeField]
	protected float m_charWidthMaxAdj;

	protected float m_charWidthAdjDelta;

	[SerializeField]
	protected bool m_enableWordWrapping;

	protected bool m_isCharacterWrappingEnabled;

	protected bool m_isNonBreakingSpace;

	protected bool m_isIgnoringAlignment;

	[SerializeField]
	protected float m_wordWrappingRatios = 0.4f;

	[SerializeField]
	protected TextOverflowModes m_overflowMode;

	protected int m_firstOverflowCharacterIndex = -1;

	[SerializeField]
	protected TMP_Text m_linkedTextComponent;

	[SerializeField]
	internal TMP_Text parentLinkedComponent;

	protected bool m_isTextTruncated;

	[SerializeField]
	protected bool m_enableKerning;

	protected float m_GlyphHorizontalAdvanceAdjustment;

	[SerializeField]
	protected bool m_enableExtraPadding;

	[SerializeField]
	protected bool checkPaddingRequired;

	[SerializeField]
	protected bool m_isRichText = true;

	[SerializeField]
	protected bool m_parseCtrlCharacters = true;

	protected bool m_isOverlay;

	[SerializeField]
	protected bool m_isOrthographic;

	[SerializeField]
	protected bool m_isCullingEnabled;

	protected bool m_isMaskingEnabled;

	protected bool isMaskUpdateRequired;

	protected bool m_ignoreCulling = true;

	[SerializeField]
	protected TextureMappingOptions m_horizontalMapping;

	[SerializeField]
	protected TextureMappingOptions m_verticalMapping;

	[SerializeField]
	protected float m_uvLineOffset;

	protected TextRenderFlags m_renderMode = TextRenderFlags.Render;

	[SerializeField]
	protected VertexSortingOrder m_geometrySortingOrder;

	[SerializeField]
	protected bool m_IsTextObjectScaleStatic;

	[SerializeField]
	protected bool m_VertexBufferAutoSizeReduction;

	protected int m_firstVisibleCharacter;

	protected int m_maxVisibleCharacters = 99999;

	protected int m_maxVisibleWords = 99999;

	protected int m_maxVisibleLines = 99999;

	[SerializeField]
	protected bool m_useMaxVisibleDescender = true;

	[SerializeField]
	protected int m_pageToDisplay = 1;

	protected bool m_isNewPage;

	[SerializeField]
	protected Vector4 m_margin = new Vector4(0f, 0f, 0f, 0f);

	protected float m_marginLeft;

	protected float m_marginRight;

	protected float m_marginWidth;

	protected float m_marginHeight;

	protected float m_width = -1f;

	protected TMP_TextInfo m_textInfo;

	protected bool m_havePropertiesChanged;

	[SerializeField]
	protected bool m_isUsingLegacyAnimationComponent;

	protected Transform m_transform;

	protected RectTransform m_rectTransform;

	protected Vector2 m_PreviousRectTransformSize;

	protected Vector2 m_PreviousPivotPosition;

	protected bool m_autoSizeTextContainer;

	protected Mesh m_mesh;

	[SerializeField]
	protected bool m_isVolumetricText;

	protected TMP_SpriteAnimator m_spriteAnimator;

	protected float m_flexibleHeight = -1f;

	protected float m_flexibleWidth = -1f;

	protected float m_minWidth;

	protected float m_minHeight;

	protected float m_maxWidth;

	protected float m_maxHeight;

	protected LayoutElement m_LayoutElement;

	protected float m_preferredWidth;

	protected float m_renderedWidth;

	protected bool m_isPreferredWidthDirty;

	protected float m_preferredHeight;

	protected float m_renderedHeight;

	protected bool m_isPreferredHeightDirty;

	protected bool m_isCalculatingPreferredValues;

	protected int m_layoutPriority;

	protected bool m_isLayoutDirty;

	protected bool m_isAwake;

	internal bool m_isWaitingOnResourceLoad;

	internal TextInputSources m_inputSource;

	protected float m_fontScaleMultiplier;

	private static char[] m_htmlTag = new char[128];

	private static RichTextTagAttribute[] m_xmlAttribute = new RichTextTagAttribute[8];

	private static float[] m_attributeParameterValues = new float[16];

	protected List<InputHtmlTagData> inputHtmlTagDataList = new List<InputHtmlTagData>();

	protected int inputHtmlTagDealIndex;

	protected int inputHtmlTagPreDealCharIndex;

	protected bool isInputHtmlTagUse;

	protected bool isInputHtmlTagNeedSort;

	protected List<InputHtmlRange> inputHtmlRangeList = new List<InputHtmlRange>();

	protected bool hasArabicChar;

	protected bool isForceArabicLan;

	protected bool isArabicLanType;

	private StringBuilder _arabicFixsb;

	protected float tag_LineIndent;

	protected float tag_Indent;

	protected TMP_TextProcessingStack<float> m_indentStack = new TMP_TextProcessingStack<float>(new float[16]);

	protected bool tag_NoParsing;

	protected bool m_isParsingText;

	protected Matrix4x4 m_FXMatrix;

	protected bool m_isFXMatrixSet;

	internal UnicodeChar[] m_TextProcessingArray = new UnicodeChar[8];

	internal int m_InternalTextProcessingArraySize;

	private TMP_CharacterInfo[] m_internalCharacterInfo;

	protected int m_totalCharacterCount;

	protected static WordWrapState m_SavedWordWrapState = default(WordWrapState);

	protected static WordWrapState m_SavedLineState = default(WordWrapState);

	protected static WordWrapState m_SavedEllipsisState = default(WordWrapState);

	protected static WordWrapState m_SavedLastValidState = default(WordWrapState);

	protected static WordWrapState m_SavedSoftLineBreakState = default(WordWrapState);

	internal static TMP_TextProcessingStack<WordWrapState> m_EllipsisInsertionCandidateStack = new TMP_TextProcessingStack<WordWrapState>(8, 8);

	protected int m_characterCount;

	protected int m_firstCharacterOfLine;

	protected int m_firstVisibleCharacterOfLine;

	protected int m_lastCharacterOfLine;

	protected int m_lastVisibleCharacterOfLine;

	protected int m_lineNumber;

	protected int m_lineVisibleCharacterCount;

	protected int m_pageNumber;

	protected float m_PageAscender;

	protected float m_maxTextAscender;

	protected float m_maxCapHeight;

	protected float m_ElementAscender;

	protected float m_ElementDescender;

	protected float m_maxLineAscender;

	protected float m_maxLineDescender;

	protected float m_startOfLineAscender;

	protected float m_startOfLineDescender;

	protected float m_lineOffset;

	protected Extents m_meshExtents;

	protected Color32 m_htmlColor = new Color(255f, 255f, 255f, 128f);

	protected TMP_TextProcessingStack<Color32> m_colorStack = new TMP_TextProcessingStack<Color32>(new Color32[16]);

	protected TMP_TextProcessingStack<Color32> m_underlineColorStack = new TMP_TextProcessingStack<Color32>(new Color32[16]);

	protected TMP_TextProcessingStack<Color32> m_strikethroughColorStack = new TMP_TextProcessingStack<Color32>(new Color32[16]);

	protected TMP_TextProcessingStack<HighlightState> m_HighlightStateStack = new TMP_TextProcessingStack<HighlightState>(new HighlightState[16]);

	protected TMP_ColorGradient m_colorGradientPreset;

	protected TMP_TextProcessingStack<TMP_ColorGradient> m_colorGradientStack = new TMP_TextProcessingStack<TMP_ColorGradient>(new TMP_ColorGradient[16]);

	protected bool m_colorGradientPresetIsTinted;

	protected float m_tabSpacing;

	protected float m_spacing;

	protected TMP_TextProcessingStack<int>[] m_TextStyleStacks = new TMP_TextProcessingStack<int>[8];

	protected int m_TextStyleStackDepth;

	protected TMP_TextProcessingStack<int> m_ItalicAngleStack = new TMP_TextProcessingStack<int>(new int[16]);

	protected int m_ItalicAngle;

	protected TMP_TextProcessingStack<int> m_actionStack = new TMP_TextProcessingStack<int>(new int[16]);

	protected float m_padding;

	protected float m_baselineOffset;

	protected TMP_TextProcessingStack<float> m_baselineOffsetStack = new TMP_TextProcessingStack<float>(new float[16]);

	protected float m_xAdvance;

	protected TMP_TextElementType m_textElementType;

	protected TMP_TextElement m_cached_TextElement;

	protected SpecialCharacter m_Ellipsis;

	protected SpecialCharacter m_Underline;

	protected TMP_SpriteAsset m_defaultSpriteAsset;

	protected TMP_SpriteAsset m_currentSpriteAsset;

	protected int m_spriteCount;

	protected int m_spriteIndex;

	protected int m_spriteAnimationID;

	private static ProfilerMarker k_ParseTextMarker = new ProfilerMarker("TMP Parse Text");

	private static ProfilerMarker k_InsertNewLineMarker = new ProfilerMarker("TMP.InsertNewLine");

	private const string regex1 = "[#*0-9]\\uFE0F?\\u20E3|©\\uFE0F?|[®\\u203C\\u2049\\u2122\\u2139\\u2194-\\u2199\\u21A9\\u21AA]\\uFE0F?|[\\u231A\\u231B]|[\\u2328\\u23CF]\\uFE0F?|[\\u23E9-\\u23EC]|[\\u23ED-\\u23EF]\\uFE0F?|\\u23F0|[\\u23F1\\u23F2]\\uFE0F?|\\u23F3|[\\u23F8-\\u23FA\\u24C2\\u25AA\\u25AB\\u25B6\\u25C0\\u25FB\\u25FC]\\uFE0F?|[\\u25FD\\u25FE]|[\\u2600-\\u2604\\u260E\\u2611]\\uFE0F?|[\\u2614\\u2615]|\\u2618\\uFE0F?|\\u261D(?:\\uD83C[\\uDFFB-\\uDFFF]|\\uFE0F)?|[\\u2620\\u2622\\u2623\\u2626\\u262A\\u262E\\u262F\\u2638-\\u263A\\u2640\\u2642]\\uFE0F?|[\\u2648-\\u2653]|[\\u265F\\u2660\\u2663\\u2665\\u2666\\u2668\\u267B\\u267E]\\uFE0F?|\\u267F|\\u2692\\uFE0F?|\\u2693|[\\u2694-\\u2697\\u2699\\u269B\\u269C\\u26A0]\\uFE0F?|\\u26A1|\\u26A7\\uFE0F?|[\\u26AA\\u26AB]|[\\u26B0\\u26B1]\\uFE0F?|[\\u26BD\\u26BE\\u26C4\\u26C5]|\\u26C8\\uFE0F?|\\u26CE|[\\u26CF\\u26D1\\u26D3]\\uFE0F?|\\u26D4|\\u26E9\\uFE0F?|\\u26EA|[\\u26F0\\u26F1]\\uFE0F?|[\\u26F2\\u26F3]|\\u26F4\\uFE0F?|\\u26F5|[\\u26F7\\u26F8]\\uFE0F?|\\u26F9(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|\\uFE0F(?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\u26FA\\u26FD]|\\u2702\\uFE0F?|\\u2705|[\\u2708\\u2709]\\uFE0F?|[\\u270A\\u270B](?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\u270C\\u270D](?:\\uD83C[\\uDFFB-\\uDFFF]|\\uFE0F)?|\\u270F\\uFE0F?|[\\u2712\\u2714\\u2716\\u271D\\u2721]\\uFE0F?|\\u2728|[\\u2733\\u2734\\u2744\\u2747]\\uFE0F?|[\\u274C\\u274E\\u2753-\\u2755\\u2757]|\\u2763\\uFE0F?|\\u2764(?:\\u200D(?:\\uD83D\\uDD25|\\uD83E\\uDE79)|\\uFE0F(?:\\u200D(?:\\uD83D\\uDD25|\\uD83E\\uDE79))?)?|[\\u2795-\\u2797]|\\u27A1\\uFE0F?|[\\u27B0\\u27BF]|[\\u2934\\u2935\\u2B05-\\u2B07]\\uFE0F?|[\\u2B1B\\u2B1C\\u2B50\\u2B55]|[\\u3030\\u303D\\u3297\\u3299]\\uFE0F?|\\uD83C(?:[\\uDC04\\uDCCF]|[\\uDD70\\uDD71\\uDD7E\\uDD7F]\\uFE0F?|[\\uDD8E\\uDD91-\\uDD9A]|\\uDDE6\\uD83C[\\uDDE8-\\uDDEC\\uDDEE\\uDDF1\\uDDF2\\uDDF4\\uDDF6-\\uDDFA\\uDDFC\\uDDFD\\uDDFF]|\\uDDE7\\uD83C[\\uDDE6\\uDDE7\\uDDE9-\\uDDEF\\uDDF1-\\uDDF4\\uDDF6-\\uDDF9\\uDDFB\\uDDFC\\uDDFE\\uDDFF]|\\uDDE8\\uD83C[\\uDDE6\\uDDE8\\uDDE9\\uDDEB-\\uDDEE\\uDDF0-\\uDDF5\\uDDF7\\uDDFA-\\uDDFF]|\\uDDE9\\uD83C[\\uDDEA\\uDDEC\\uDDEF\\uDDF0\\uDDF2\\uDDF4\\uDDFF]|\\uDDEA\\uD83C[\\uDDE6\\uDDE8\\uDDEA\\uDDEC\\uDDED\\uDDF7-\\uDDFA]|\\uDDEB\\uD83C[\\uDDEE-\\uDDF0\\uDDF2\\uDDF4\\uDDF7]|\\uDDEC\\uD83C[\\uDDE6\\uDDE7\\uDDE9-\\uDDEE\\uDDF1-\\uDDF3\\uDDF5-\\uDDFA\\uDDFC\\uDDFE]|\\uDDED\\uD83C[\\uDDF0\\uDDF2\\uDDF3\\uDDF7\\uDDF9\\uDDFA]|\\uDDEE\\uD83C[\\uDDE8-\\uDDEA\\uDDF1-\\uDDF4\\uDDF6-\\uDDF9]|\\uDDEF\\uD83C[\\uDDEA\\uDDF2\\uDDF4\\uDDF5]|\\uDDF0\\uD83C[\\uDDEA\\uDDEC-\\uDDEE\\uDDF2\\uDDF3\\uDDF5\\uDDF7\\uDDFC\\uDDFE\\uDDFF]|\\uDDF1\\uD83C[\\uDDE6-\\uDDE8\\uDDEE\\uDDF0\\uDDF7-\\uDDFB\\uDDFE]|\\uDDF2\\uD83C[\\uDDE6\\uDDE8-\\uDDED\\uDDF0-\\uDDFF]|\\uDDF3\\uD83C[\\uDDE6\\uDDE8\\uDDEA-\\uDDEC\\uDDEE\\uDDF1\\uDDF4\\uDDF5\\uDDF7\\uDDFA\\uDDFF]|\\uDDF4\\uD83C\\uDDF2|\\uDDF5\\uD83C[\\uDDE6\\uDDEA-\\uDDED\\uDDF0-\\uDDF3\\uDDF7-\\uDDF9\\uDDFC\\uDDFE]|\\uDDF6\\uD83C\\uDDE6|\\uDDF7\\uD83C[\\uDDEA\\uDDF4\\uDDF8\\uDDFA\\uDDFC]|\\uDDF8\\uD83C[\\uDDE6-\\uDDEA\\uDDEC-\\uDDF4\\uDDF7-\\uDDF9\\uDDFB\\uDDFD-\\uDDFF]|\\uDDF9\\uD83C[\\uDDE6\\uDDE8\\uDDE9\\uDDEB-\\uDDED\\uDDEF-\\uDDF4\\uDDF7\\uDDF9\\uDDFB\\uDDFC\\uDDFF]|\\uDDFA\\uD83C[\\uDDE6\\uDDEC\\uDDF2\\uDDF3\\uDDF8\\uDDFE\\uDDFF]|\\uDDFB\\uD83C[\\uDDE6\\uDDE8\\uDDEA\\uDDEC\\uDDEE\\uDDF3\\uDDFA]|\\uDDFC\\uD83C[\\uDDEB\\uDDF8]|\\uDDFD\\uD83C\\uDDF0|\\uDDFE\\uD83C[\\uDDEA\\uDDF9]|\\uDDFF\\uD83C[\\uDDE6\\uDDF2\\uDDFC]|\\uDE01|\\uDE02\\uFE0F?|[\\uDE1A\\uDE2F\\uDE32-\\uDE36]|\\uDE37\\uFE0F?|[\\uDE38-\\uDE3A\\uDE50\\uDE51\\uDF00-\\uDF20]|[\\uDF21\\uDF24-\\uDF2C]\\uFE0F?|[\\uDF2D-\\uDF35]|\\uDF36\\uFE0F?|[\\uDF37-\\uDF7C]|\\uDF7D\\uFE0F?|[\\uDF7E-\\uDF84]|\\uDF85(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDF86-\\uDF93]|[\\uDF96\\uDF97\\uDF99-\\uDF9B\\uDF9E\\uDF9F]\\uFE0F?|[\\uDFA0-\\uDFC1]|\\uDFC2(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDFC3\\uDFC4](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDFC5\\uDFC6]|\\uDFC7(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDFC8\\uDFC9]|\\uDFCA(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDFCB\\uDFCC](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|\\uFE0F(?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDFCD\\uDFCE]\\uFE0F?|[\\uDFCF-\\uDFD3]|[\\uDFD4-\\uDFDF]\\uFE0F?|[\\uDFE0-\\uDFF0]|\\uDFF3(?:\\u200D(?:\\u26A7\\uFE0F?|\\uD83C\\uDF08)|\\uFE0F(?:\\u200D(?:\\u26A7\\uFE0F?|\\uD83C\\uDF08))?)?|\\uDFF4(?:\\u200D\\u2620\\uFE0F?|\\uDB40\\uDC67\\uDB40\\uDC62\\uDB40(?:\\uDC65\\uDB40\\uDC6E\\uDB40\\uDC67|\\uDC73\\uDB40\\uDC63\\uDB40\\uDC74|\\uDC77\\uDB40\\uDC6C\\uDB40\\uDC73)\\uDB40\\uDC7F)?|[\\uDFF5\\uDFF7]\\uFE0F?|[\\uDFF8-\\uDFFF])|\\uD83D(?:[\\uDC00-\\uDC07]|\\uDC08(?:\\u200D\\u2B1B)?|[\\uDC09-\\uDC14]|\\uDC15(?:\\u200D\\uD83E\\uDDBA)?|[\\uDC16-\\uDC3A]|\\uDC3B(?:\\u200D\\u2744\\uFE0F?)?|[\\uDC3C-\\uDC3E]|\\uDC3F\\uFE0F?|\\uDC40|\\uDC41(?:\\u200D\\uD83D\\uDDE8\\uFE0F?|\\uFE0F(?:\\u200D\\uD83D\\uDDE8\\uFE0F?)?)?|[\\uDC42\\uDC43](?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC44\\uDC45]|[\\uDC46-\\uDC50](?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC51-\\uDC65]|[\\uDC66\\uDC67](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC68(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D(?:\\uDC66(?:\\u200D\\uD83D\\uDC66)?|\\uDC67(?:\\u200D\\uD83D[\\uDC66\\uDC67])?|[\\uDC68\\uDC69]\\u200D\\uD83D(?:\\uDC66(?:\\u200D\\uD83D\\uDC66)?|\\uDC67(?:\\u200D\\uD83D[\\uDC66\\uDC67])?)|[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92])|\\uD83E[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])|\\uD83C(?:\\uDFFB(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68\\uD83C[\\uDFFB-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D\\uDC68\\uD83C[\\uDFFC-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFC(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68\\uD83C[\\uDFFB-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D\\uDC68\\uD83C[\\uDFFB\\uDFFD-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFD(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68\\uD83C[\\uDFFB-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D\\uDC68\\uD83C[\\uDFFB\\uDFFC\\uDFFE\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFE(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68\\uD83C[\\uDFFB-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D\\uDC68\\uD83C[\\uDFFB-\\uDFFD\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFF(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68\\uD83C[\\uDFFB-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D\\uDC68\\uD83C[\\uDFFB-\\uDFFE]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?))?|\\uDC69(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?[\\uDC68\\uDC69]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D(?:\\uDC66(?:\\u200D\\uD83D\\uDC66)?|\\uDC67(?:\\u200D\\uD83D[\\uDC66\\uDC67])?|\\uDC69\\u200D\\uD83D(?:\\uDC66(?:\\u200D\\uD83D\\uDC66)?|\\uDC67(?:\\u200D\\uD83D[\\uDC66\\uDC67])?)|[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92])|\\uD83E[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])|\\uD83C(?:\\uDFFB(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF]|\\uDC8B\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF])|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFC-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFC(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF]|\\uDC8B\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF])|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB\\uDFFD-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFD(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF]|\\uDC8B\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF])|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB\\uDFFC\\uDFFE\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFE(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF]|\\uDC8B\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF])|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFD\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFF(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF]|\\uDC8B\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF])|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFE]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?))?|\\uDC6A|[\\uDC6B-\\uDC6D](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC6E(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDC6F(?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|[\\uDC70\\uDC71](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDC72(?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC73(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDC74-\\uDC76](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC77(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDC78(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC79-\\uDC7B]|\\uDC7C(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC7D-\\uDC80]|[\\uDC81\\uDC82](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDC83(?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC84|\\uDC85(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC86\\uDC87](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDC88-\\uDC8E]|\\uDC8F(?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC90|\\uDC91(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC92-\\uDCA9]|\\uDCAA(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDCAB-\\uDCFC]|\\uDCFD\\uFE0F?|[\\uDCFF-\\uDD3D]|[\\uDD49\\uDD4A]\\uFE0F?|[\\uDD4B-\\uDD4E\\uDD50-\\uDD67]|[\\uDD6F\\uDD70\\uDD73]\\uFE0F?|\\uDD74(?:\\uD83C[\\uDFFB-\\uDFFF]|\\uFE0F)?|\\uDD75(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|\\uFE0F(?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDD76-\\uDD79]\\uFE0F?|\\uDD7A(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD87\\uDD8A-\\uDD8D]\\uFE0F?|\\uDD90(?:\\uD83C[\\uDFFB-\\uDFFF]|\\uFE0F)?|[\\uDD95\\uDD96](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDDA4|[\\uDDA5\\uDDA8\\uDDB1\\uDDB2\\uDDBC\\uDDC2-\\uDDC4\\uDDD1-\\uDDD3\\uDDDC-\\uDDDE\\uDDE1\\uDDE3\\uDDE8\\uDDEF\\uDDF3\\uDDFA]\\uFE0F?|[\\uDDFB-\\uDE2D]|\\uDE2E(?:\\u200D\\uD83D\\uDCA8)?|[\\uDE2F-\\uDE34]|\\uDE35(?:\\u200D\\uD83D\\uDCAB)?|\\uDE36(?:\\u200D\\uD83C\\uDF2B\\uFE0F?)?|[\\uDE37-\\uDE44]|[\\uDE45-\\uDE47](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDE48-\\uDE4A]|\\uDE4B(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDE4C(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDE4D\\uDE4E](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDE4F(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDE80-\\uDEA2]|\\uDEA3(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDEA4-\\uDEB3]|[\\uDEB4-\\uDEB6](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDEB7-\\uDEBF]|\\uDEC0(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDEC1-\\uDEC5]|\\uDECB\\uFE0F?|\\uDECC(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDECD-\\uDECF]\\uFE0F?|[\\uDED0-\\uDED2\\uDED5-\\uDED7]|[\\uDEE0-\\uDEE5\\uDEE9]\\uFE0F?|[\\uDEEB\\uDEEC]|[\\uDEF0\\uDEF3]\\uFE0F?|[\\uDEF4-\\uDEFC\\uDFE0-\\uDFEB])|\\uD83E(?:\\uDD0C(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD0D\\uDD0E]|\\uDD0F(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD10-\\uDD17]|[\\uDD18-\\uDD1C](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDD1D|[\\uDD1E\\uDD1F](?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD20-\\uDD25]|\\uDD26(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDD27-\\uDD2F]|[\\uDD30-\\uDD34](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDD35(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDD36(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD37-\\uDD39](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDD3A|\\uDD3C(?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|[\\uDD3D\\uDD3E](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDD3F-\\uDD45\\uDD47-\\uDD76]|\\uDD77(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD78\\uDD7A-\\uDDB4]|[\\uDDB5\\uDDB6](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDDB7|[\\uDDB8\\uDDB9](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDDBA|\\uDDBB(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDDBC-\\uDDCB]|[\\uDDCD-\\uDDCF](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDDD0|\\uDDD1(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD]))|\\uD83C(?:\\uDFFB(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D(?:\\uD83D\\uDC8B\\u200D)?\\uD83E\\uDDD1\\uD83C[\\uDFFC-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFC(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D(?:\\uD83D\\uDC8B\\u200D)?\\uD83E\\uDDD1\\uD83C[\\uDFFB\\uDFFD-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFD(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D(?:\\uD83D\\uDC8B\\u200D)?\\uD83E\\uDDD1\\uD83C[\\uDFFB\\uDFFC\\uDFFE\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFE(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D(?:\\uD83D\\uDC8B\\u200D)?\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFD\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFF(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D(?:\\uD83D\\uDC8B\\u200D)?\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFE]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?))?|[\\uDDD2\\uDDD3](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDDD4(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDDD5(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDDD6-\\uDDDD](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDDDE\\uDDDF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|[\\uDDE0-\\uDDFF\\uDE70-\\uDE74\\uDE78-\\uDE7A\\uDE80-\\uDE86\\uDE90-\\uDEA8\\uDEB0-\\uDEB6\\uDEC0-\\uDEC2\\uDED0-\\uDED6])";

	private static Regex Regex1 = new Regex("[#*0-9]\\uFE0F?\\u20E3|©\\uFE0F?|[®\\u203C\\u2049\\u2122\\u2139\\u2194-\\u2199\\u21A9\\u21AA]\\uFE0F?|[\\u231A\\u231B]|[\\u2328\\u23CF]\\uFE0F?|[\\u23E9-\\u23EC]|[\\u23ED-\\u23EF]\\uFE0F?|\\u23F0|[\\u23F1\\u23F2]\\uFE0F?|\\u23F3|[\\u23F8-\\u23FA\\u24C2\\u25AA\\u25AB\\u25B6\\u25C0\\u25FB\\u25FC]\\uFE0F?|[\\u25FD\\u25FE]|[\\u2600-\\u2604\\u260E\\u2611]\\uFE0F?|[\\u2614\\u2615]|\\u2618\\uFE0F?|\\u261D(?:\\uD83C[\\uDFFB-\\uDFFF]|\\uFE0F)?|[\\u2620\\u2622\\u2623\\u2626\\u262A\\u262E\\u262F\\u2638-\\u263A\\u2640\\u2642]\\uFE0F?|[\\u2648-\\u2653]|[\\u265F\\u2660\\u2663\\u2665\\u2666\\u2668\\u267B\\u267E]\\uFE0F?|\\u267F|\\u2692\\uFE0F?|\\u2693|[\\u2694-\\u2697\\u2699\\u269B\\u269C\\u26A0]\\uFE0F?|\\u26A1|\\u26A7\\uFE0F?|[\\u26AA\\u26AB]|[\\u26B0\\u26B1]\\uFE0F?|[\\u26BD\\u26BE\\u26C4\\u26C5]|\\u26C8\\uFE0F?|\\u26CE|[\\u26CF\\u26D1\\u26D3]\\uFE0F?|\\u26D4|\\u26E9\\uFE0F?|\\u26EA|[\\u26F0\\u26F1]\\uFE0F?|[\\u26F2\\u26F3]|\\u26F4\\uFE0F?|\\u26F5|[\\u26F7\\u26F8]\\uFE0F?|\\u26F9(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|\\uFE0F(?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\u26FA\\u26FD]|\\u2702\\uFE0F?|\\u2705|[\\u2708\\u2709]\\uFE0F?|[\\u270A\\u270B](?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\u270C\\u270D](?:\\uD83C[\\uDFFB-\\uDFFF]|\\uFE0F)?|\\u270F\\uFE0F?|[\\u2712\\u2714\\u2716\\u271D\\u2721]\\uFE0F?|\\u2728|[\\u2733\\u2734\\u2744\\u2747]\\uFE0F?|[\\u274C\\u274E\\u2753-\\u2755\\u2757]|\\u2763\\uFE0F?|\\u2764(?:\\u200D(?:\\uD83D\\uDD25|\\uD83E\\uDE79)|\\uFE0F(?:\\u200D(?:\\uD83D\\uDD25|\\uD83E\\uDE79))?)?|[\\u2795-\\u2797]|\\u27A1\\uFE0F?|[\\u27B0\\u27BF]|[\\u2934\\u2935\\u2B05-\\u2B07]\\uFE0F?|[\\u2B1B\\u2B1C\\u2B50\\u2B55]|[\\u3030\\u303D\\u3297\\u3299]\\uFE0F?|\\uD83C(?:[\\uDC04\\uDCCF]|[\\uDD70\\uDD71\\uDD7E\\uDD7F]\\uFE0F?|[\\uDD8E\\uDD91-\\uDD9A]|\\uDDE6\\uD83C[\\uDDE8-\\uDDEC\\uDDEE\\uDDF1\\uDDF2\\uDDF4\\uDDF6-\\uDDFA\\uDDFC\\uDDFD\\uDDFF]|\\uDDE7\\uD83C[\\uDDE6\\uDDE7\\uDDE9-\\uDDEF\\uDDF1-\\uDDF4\\uDDF6-\\uDDF9\\uDDFB\\uDDFC\\uDDFE\\uDDFF]|\\uDDE8\\uD83C[\\uDDE6\\uDDE8\\uDDE9\\uDDEB-\\uDDEE\\uDDF0-\\uDDF5\\uDDF7\\uDDFA-\\uDDFF]|\\uDDE9\\uD83C[\\uDDEA\\uDDEC\\uDDEF\\uDDF0\\uDDF2\\uDDF4\\uDDFF]|\\uDDEA\\uD83C[\\uDDE6\\uDDE8\\uDDEA\\uDDEC\\uDDED\\uDDF7-\\uDDFA]|\\uDDEB\\uD83C[\\uDDEE-\\uDDF0\\uDDF2\\uDDF4\\uDDF7]|\\uDDEC\\uD83C[\\uDDE6\\uDDE7\\uDDE9-\\uDDEE\\uDDF1-\\uDDF3\\uDDF5-\\uDDFA\\uDDFC\\uDDFE]|\\uDDED\\uD83C[\\uDDF0\\uDDF2\\uDDF3\\uDDF7\\uDDF9\\uDDFA]|\\uDDEE\\uD83C[\\uDDE8-\\uDDEA\\uDDF1-\\uDDF4\\uDDF6-\\uDDF9]|\\uDDEF\\uD83C[\\uDDEA\\uDDF2\\uDDF4\\uDDF5]|\\uDDF0\\uD83C[\\uDDEA\\uDDEC-\\uDDEE\\uDDF2\\uDDF3\\uDDF5\\uDDF7\\uDDFC\\uDDFE\\uDDFF]|\\uDDF1\\uD83C[\\uDDE6-\\uDDE8\\uDDEE\\uDDF0\\uDDF7-\\uDDFB\\uDDFE]|\\uDDF2\\uD83C[\\uDDE6\\uDDE8-\\uDDED\\uDDF0-\\uDDFF]|\\uDDF3\\uD83C[\\uDDE6\\uDDE8\\uDDEA-\\uDDEC\\uDDEE\\uDDF1\\uDDF4\\uDDF5\\uDDF7\\uDDFA\\uDDFF]|\\uDDF4\\uD83C\\uDDF2|\\uDDF5\\uD83C[\\uDDE6\\uDDEA-\\uDDED\\uDDF0-\\uDDF3\\uDDF7-\\uDDF9\\uDDFC\\uDDFE]|\\uDDF6\\uD83C\\uDDE6|\\uDDF7\\uD83C[\\uDDEA\\uDDF4\\uDDF8\\uDDFA\\uDDFC]|\\uDDF8\\uD83C[\\uDDE6-\\uDDEA\\uDDEC-\\uDDF4\\uDDF7-\\uDDF9\\uDDFB\\uDDFD-\\uDDFF]|\\uDDF9\\uD83C[\\uDDE6\\uDDE8\\uDDE9\\uDDEB-\\uDDED\\uDDEF-\\uDDF4\\uDDF7\\uDDF9\\uDDFB\\uDDFC\\uDDFF]|\\uDDFA\\uD83C[\\uDDE6\\uDDEC\\uDDF2\\uDDF3\\uDDF8\\uDDFE\\uDDFF]|\\uDDFB\\uD83C[\\uDDE6\\uDDE8\\uDDEA\\uDDEC\\uDDEE\\uDDF3\\uDDFA]|\\uDDFC\\uD83C[\\uDDEB\\uDDF8]|\\uDDFD\\uD83C\\uDDF0|\\uDDFE\\uD83C[\\uDDEA\\uDDF9]|\\uDDFF\\uD83C[\\uDDE6\\uDDF2\\uDDFC]|\\uDE01|\\uDE02\\uFE0F?|[\\uDE1A\\uDE2F\\uDE32-\\uDE36]|\\uDE37\\uFE0F?|[\\uDE38-\\uDE3A\\uDE50\\uDE51\\uDF00-\\uDF20]|[\\uDF21\\uDF24-\\uDF2C]\\uFE0F?|[\\uDF2D-\\uDF35]|\\uDF36\\uFE0F?|[\\uDF37-\\uDF7C]|\\uDF7D\\uFE0F?|[\\uDF7E-\\uDF84]|\\uDF85(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDF86-\\uDF93]|[\\uDF96\\uDF97\\uDF99-\\uDF9B\\uDF9E\\uDF9F]\\uFE0F?|[\\uDFA0-\\uDFC1]|\\uDFC2(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDFC3\\uDFC4](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDFC5\\uDFC6]|\\uDFC7(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDFC8\\uDFC9]|\\uDFCA(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDFCB\\uDFCC](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|\\uFE0F(?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDFCD\\uDFCE]\\uFE0F?|[\\uDFCF-\\uDFD3]|[\\uDFD4-\\uDFDF]\\uFE0F?|[\\uDFE0-\\uDFF0]|\\uDFF3(?:\\u200D(?:\\u26A7\\uFE0F?|\\uD83C\\uDF08)|\\uFE0F(?:\\u200D(?:\\u26A7\\uFE0F?|\\uD83C\\uDF08))?)?|\\uDFF4(?:\\u200D\\u2620\\uFE0F?|\\uDB40\\uDC67\\uDB40\\uDC62\\uDB40(?:\\uDC65\\uDB40\\uDC6E\\uDB40\\uDC67|\\uDC73\\uDB40\\uDC63\\uDB40\\uDC74|\\uDC77\\uDB40\\uDC6C\\uDB40\\uDC73)\\uDB40\\uDC7F)?|[\\uDFF5\\uDFF7]\\uFE0F?|[\\uDFF8-\\uDFFF])|\\uD83D(?:[\\uDC00-\\uDC07]|\\uDC08(?:\\u200D\\u2B1B)?|[\\uDC09-\\uDC14]|\\uDC15(?:\\u200D\\uD83E\\uDDBA)?|[\\uDC16-\\uDC3A]|\\uDC3B(?:\\u200D\\u2744\\uFE0F?)?|[\\uDC3C-\\uDC3E]|\\uDC3F\\uFE0F?|\\uDC40|\\uDC41(?:\\u200D\\uD83D\\uDDE8\\uFE0F?|\\uFE0F(?:\\u200D\\uD83D\\uDDE8\\uFE0F?)?)?|[\\uDC42\\uDC43](?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC44\\uDC45]|[\\uDC46-\\uDC50](?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC51-\\uDC65]|[\\uDC66\\uDC67](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC68(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D(?:\\uDC66(?:\\u200D\\uD83D\\uDC66)?|\\uDC67(?:\\u200D\\uD83D[\\uDC66\\uDC67])?|[\\uDC68\\uDC69]\\u200D\\uD83D(?:\\uDC66(?:\\u200D\\uD83D\\uDC66)?|\\uDC67(?:\\u200D\\uD83D[\\uDC66\\uDC67])?)|[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92])|\\uD83E[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])|\\uD83C(?:\\uDFFB(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68\\uD83C[\\uDFFB-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D\\uDC68\\uD83C[\\uDFFC-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFC(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68\\uD83C[\\uDFFB-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D\\uDC68\\uD83C[\\uDFFB\\uDFFD-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFD(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68\\uD83C[\\uDFFB-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D\\uDC68\\uD83C[\\uDFFB\\uDFFC\\uDFFE\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFE(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68\\uD83C[\\uDFFB-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D\\uDC68\\uD83C[\\uDFFB-\\uDFFD\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFF(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68\\uD83C[\\uDFFB-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D\\uDC68\\uD83C[\\uDFFB-\\uDFFE]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?))?|\\uDC69(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?[\\uDC68\\uDC69]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D(?:\\uDC66(?:\\u200D\\uD83D\\uDC66)?|\\uDC67(?:\\u200D\\uD83D[\\uDC66\\uDC67])?|\\uDC69\\u200D\\uD83D(?:\\uDC66(?:\\u200D\\uD83D\\uDC66)?|\\uDC67(?:\\u200D\\uD83D[\\uDC66\\uDC67])?)|[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92])|\\uD83E[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])|\\uD83C(?:\\uDFFB(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF]|\\uDC8B\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF])|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFC-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFC(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF]|\\uDC8B\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF])|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB\\uDFFD-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFD(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF]|\\uDC8B\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF])|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB\\uDFFC\\uDFFE\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFE(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF]|\\uDC8B\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF])|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFD\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFF(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF]|\\uDC8B\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF])|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFE]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?))?|\\uDC6A|[\\uDC6B-\\uDC6D](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC6E(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDC6F(?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|[\\uDC70\\uDC71](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDC72(?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC73(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDC74-\\uDC76](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC77(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDC78(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC79-\\uDC7B]|\\uDC7C(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC7D-\\uDC80]|[\\uDC81\\uDC82](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDC83(?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC84|\\uDC85(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC86\\uDC87](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDC88-\\uDC8E]|\\uDC8F(?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC90|\\uDC91(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC92-\\uDCA9]|\\uDCAA(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDCAB-\\uDCFC]|\\uDCFD\\uFE0F?|[\\uDCFF-\\uDD3D]|[\\uDD49\\uDD4A]\\uFE0F?|[\\uDD4B-\\uDD4E\\uDD50-\\uDD67]|[\\uDD6F\\uDD70\\uDD73]\\uFE0F?|\\uDD74(?:\\uD83C[\\uDFFB-\\uDFFF]|\\uFE0F)?|\\uDD75(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|\\uFE0F(?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDD76-\\uDD79]\\uFE0F?|\\uDD7A(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD87\\uDD8A-\\uDD8D]\\uFE0F?|\\uDD90(?:\\uD83C[\\uDFFB-\\uDFFF]|\\uFE0F)?|[\\uDD95\\uDD96](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDDA4|[\\uDDA5\\uDDA8\\uDDB1\\uDDB2\\uDDBC\\uDDC2-\\uDDC4\\uDDD1-\\uDDD3\\uDDDC-\\uDDDE\\uDDE1\\uDDE3\\uDDE8\\uDDEF\\uDDF3\\uDDFA]\\uFE0F?|[\\uDDFB-\\uDE2D]|\\uDE2E(?:\\u200D\\uD83D\\uDCA8)?|[\\uDE2F-\\uDE34]|\\uDE35(?:\\u200D\\uD83D\\uDCAB)?|\\uDE36(?:\\u200D\\uD83C\\uDF2B\\uFE0F?)?|[\\uDE37-\\uDE44]|[\\uDE45-\\uDE47](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDE48-\\uDE4A]|\\uDE4B(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDE4C(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDE4D\\uDE4E](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDE4F(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDE80-\\uDEA2]|\\uDEA3(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDEA4-\\uDEB3]|[\\uDEB4-\\uDEB6](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDEB7-\\uDEBF]|\\uDEC0(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDEC1-\\uDEC5]|\\uDECB\\uFE0F?|\\uDECC(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDECD-\\uDECF]\\uFE0F?|[\\uDED0-\\uDED2\\uDED5-\\uDED7]|[\\uDEE0-\\uDEE5\\uDEE9]\\uFE0F?|[\\uDEEB\\uDEEC]|[\\uDEF0\\uDEF3]\\uFE0F?|[\\uDEF4-\\uDEFC\\uDFE0-\\uDFEB])|\\uD83E(?:\\uDD0C(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD0D\\uDD0E]|\\uDD0F(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD10-\\uDD17]|[\\uDD18-\\uDD1C](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDD1D|[\\uDD1E\\uDD1F](?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD20-\\uDD25]|\\uDD26(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDD27-\\uDD2F]|[\\uDD30-\\uDD34](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDD35(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDD36(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD37-\\uDD39](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDD3A|\\uDD3C(?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|[\\uDD3D\\uDD3E](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDD3F-\\uDD45\\uDD47-\\uDD76]|\\uDD77(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD78\\uDD7A-\\uDDB4]|[\\uDDB5\\uDDB6](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDDB7|[\\uDDB8\\uDDB9](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDDBA|\\uDDBB(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDDBC-\\uDDCB]|[\\uDDCD-\\uDDCF](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDDD0|\\uDDD1(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD]))|\\uD83C(?:\\uDFFB(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D(?:\\uD83D\\uDC8B\\u200D)?\\uD83E\\uDDD1\\uD83C[\\uDFFC-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFC(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D(?:\\uD83D\\uDC8B\\u200D)?\\uD83E\\uDDD1\\uD83C[\\uDFFB\\uDFFD-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFD(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D(?:\\uD83D\\uDC8B\\u200D)?\\uD83E\\uDDD1\\uD83C[\\uDFFB\\uDFFC\\uDFFE\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFE(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D(?:\\uD83D\\uDC8B\\u200D)?\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFD\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFF(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D(?:\\uD83D\\uDC8B\\u200D)?\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFE]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?))?|[\\uDDD2\\uDDD3](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDDD4(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDDD5(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDDD6-\\uDDDD](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDDDE\\uDDDF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|[\\uDDE0-\\uDDFF\\uDE70-\\uDE74\\uDE78-\\uDE7A\\uDE80-\\uDE86\\uDE90-\\uDEA8\\uDEB0-\\uDEB6\\uDEC0-\\uDEC2\\uDED0-\\uDED6])", RegexOptions.Compiled);

	private static readonly Dictionary<string, EmojiSpriteInfo> emojiFastSearch = new Dictionary<string, EmojiSpriteInfo>();

	private static string EmojiVariationSelection = "fe0f";

	private static string CombiningEnclosingKeycap = "20e3";

	private static byte[] EmojiUtf32Bytes = new byte[64];

	private static StringBuilder EmojiNameStr = new StringBuilder();

	private static StringBuilder EmojiNameStrHigh = new StringBuilder();

	private static StringBuilder EmojiNameStrLow = new StringBuilder();

	protected static char InternalSpriteTagBegin = '<';

	protected static char InternalSpriteTagEnd = '>';

	protected bool m_supportSpriteEmoji;

	protected bool m_supportSpriteTagOnly;

	protected bool m_ignoreActiveState;

	private TextBackingContainer m_TextBackingArray = new TextBackingContainer(4);

	private TextBackingContainer m_TMPFixCharArray = new TextBackingContainer(4);

	private TextBackingContainer m_TMPFixCharLenArray = new TextBackingContainer(4);

	private static List<TashkeelLocation> TashkeelLocations = new List<TashkeelLocation>();

	private static uint ArabicEmptyChar = 65535u;

	private bool farsi;

	private bool preserveNumbers = true;

	private static readonly string Diacritics = "[\\u064B-\\u0652\\u0670]*";

	private static readonly string BoundaryStart = "(^|[\\s\\p{P}\\p{S}])";

	private static readonly string BoundaryEnd = "(?=$|[\\s\\p{P}\\p{S}])";

	private static readonly HashSet<char> TashkeelCharactersSet = new HashSet<char>
	{
		'\u064b', '\u064c', '\u064d', '\u064e', '\u064f', '\u0650', '\u0651', '\u0652', '\u0653', '\u0670',
		'ﱞ', 'ﱟ', 'ﱠ', 'ﱡ', 'ﱢ', 'ﱣ'
	};

	private static readonly Dictionary<char, char> ShaddaCombinationMap = new Dictionary<char, char>
	{
		['\u064c'] = 'ﱞ',
		['\u064d'] = 'ﱟ',
		['\u064e'] = 'ﱠ',
		['\u064f'] = 'ﱡ',
		['\u0650'] = 'ﱢ',
		['\u0670'] = 'ﱣ'
	};

	private static readonly Dictionary<char, char> MirroredCharsMap = new Dictionary<char, char>
	{
		['('] = ')',
		[')'] = '(',
		['['] = ']',
		[']'] = '[',
		['<'] = '>',
		['>'] = '<',
		['≥'] = '≤',
		['≤'] = '≥'
	};

	private static readonly HashSet<char> MirroredCharsSet = new HashSet<char>(MirroredCharsMap.Keys);

	private readonly decimal[] k_Power = new decimal[10] { 0.5m, 0.05m, 0.005m, 0.0005m, 0.00005m, 0.000005m, 0.0000005m, 0.00000005m, 0.000000005m, 0.0000000005m };

	protected static Vector2 k_LargePositiveVector2 = new Vector2(2.1474836E+09f, 2.1474836E+09f);

	protected static Vector2 k_LargeNegativeVector2 = new Vector2(-2.1474836E+09f, -2.1474836E+09f);

	protected static float k_LargePositiveFloat = 32767f;

	protected static float k_LargeNegativeFloat = -32767f;

	protected static int k_LargePositiveInt = int.MaxValue;

	protected static int k_LargeNegativeInt = -2147483647;

	public virtual string text
	{
		get
		{
			if (m_IsTextBackingStringDirty)
			{
				return InternalTextBackingArrayToString();
			}
			return m_text;
		}
		set
		{
			if (m_IsTextBackingStringDirty || m_text == null || value == null || m_text.Length != value.Length || !(m_text == value))
			{
				m_IsTextBackingStringDirty = false;
				m_text = value;
				m_inputSource = TextInputSources.TextString;
				m_havePropertiesChanged = true;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public ITextPreprocessor textPreprocessor
	{
		get
		{
			return m_TextPreprocessor;
		}
		set
		{
			m_TextPreprocessor = value;
		}
	}

	public bool isRightToLeftText
	{
		get
		{
			return m_isRightToLeft;
		}
		set
		{
			if (m_isRightToLeft != value)
			{
				m_isRightToLeft = value;
				m_havePropertiesChanged = true;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public TMP_FontAsset font
	{
		get
		{
			return m_fontAsset;
		}
		set
		{
			if (!(m_fontAsset == value))
			{
				m_fontAsset = value;
				LoadFontAsset();
				m_havePropertiesChanged = true;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public virtual Material fontSharedMaterial
	{
		get
		{
			return m_sharedMaterial;
		}
		set
		{
			if (!(m_sharedMaterial == value))
			{
				SetSharedMaterial(value);
				m_havePropertiesChanged = true;
				SetVerticesDirty();
				SetMaterialDirty();
			}
		}
	}

	public virtual Material[] fontSharedMaterials
	{
		get
		{
			return GetSharedMaterials();
		}
		set
		{
			SetSharedMaterials(value);
			m_havePropertiesChanged = true;
			SetVerticesDirty();
			SetMaterialDirty();
		}
	}

	public Material fontMaterial
	{
		get
		{
			return GetMaterial(m_sharedMaterial);
		}
		set
		{
			if (!(m_sharedMaterial != null) || m_sharedMaterial.GetInstanceID() != value.GetInstanceID())
			{
				m_sharedMaterial = value;
				m_padding = GetPaddingForMaterial();
				m_havePropertiesChanged = true;
				SetVerticesDirty();
				SetMaterialDirty();
			}
		}
	}

	public virtual Material[] fontMaterials
	{
		get
		{
			return GetMaterials(m_fontSharedMaterials);
		}
		set
		{
			SetSharedMaterials(value);
			m_havePropertiesChanged = true;
			SetVerticesDirty();
			SetMaterialDirty();
		}
	}

	public override Color color
	{
		get
		{
			return m_fontColor;
		}
		set
		{
			if (!(m_fontColor == value))
			{
				m_havePropertiesChanged = true;
				m_fontColor = value;
				SetVerticesDirty();
			}
		}
	}

	public float alpha
	{
		get
		{
			return m_fontColor.a;
		}
		set
		{
			if (m_fontColor.a != value)
			{
				m_fontColor.a = value;
				m_havePropertiesChanged = true;
				SetVerticesDirty();
			}
		}
	}

	public bool enableVertexGradient
	{
		get
		{
			return m_enableVertexGradient;
		}
		set
		{
			if (m_enableVertexGradient != value)
			{
				m_havePropertiesChanged = true;
				m_enableVertexGradient = value;
				SetVerticesDirty();
			}
		}
	}

	public VertexGradient colorGradient
	{
		get
		{
			return m_fontColorGradient;
		}
		set
		{
			m_havePropertiesChanged = true;
			m_fontColorGradient = value;
			SetVerticesDirty();
		}
	}

	public TMP_ColorGradient colorGradientPreset
	{
		get
		{
			return m_fontColorGradientPreset;
		}
		set
		{
			m_havePropertiesChanged = true;
			m_fontColorGradientPreset = value;
			SetVerticesDirty();
		}
	}

	public TMP_SpriteAsset spriteAsset
	{
		get
		{
			return m_spriteAsset;
		}
		set
		{
			m_spriteAsset = value;
			m_havePropertiesChanged = true;
			SetVerticesDirty();
			SetLayoutDirty();
		}
	}

	public bool tintAllSprites
	{
		get
		{
			return m_tintAllSprites;
		}
		set
		{
			if (m_tintAllSprites != value)
			{
				m_tintAllSprites = value;
				m_havePropertiesChanged = true;
				SetVerticesDirty();
			}
		}
	}

	public TMP_StyleSheet styleSheet
	{
		get
		{
			return m_StyleSheet;
		}
		set
		{
			m_StyleSheet = value;
			m_havePropertiesChanged = true;
			SetVerticesDirty();
			SetLayoutDirty();
		}
	}

	public TMP_Style textStyle
	{
		get
		{
			m_TextStyle = GetStyle(m_TextStyleHashCode);
			if (m_TextStyle == null)
			{
				m_TextStyle = TMP_Style.NormalStyle;
				m_TextStyleHashCode = m_TextStyle.hashCode;
			}
			return m_TextStyle;
		}
		set
		{
			m_TextStyle = value;
			m_TextStyleHashCode = m_TextStyle.hashCode;
			m_havePropertiesChanged = true;
			SetVerticesDirty();
			SetLayoutDirty();
		}
	}

	public bool overrideColorTags
	{
		get
		{
			return m_overrideHtmlColors;
		}
		set
		{
			if (m_overrideHtmlColors != value)
			{
				m_havePropertiesChanged = true;
				m_overrideHtmlColors = value;
				SetVerticesDirty();
			}
		}
	}

	public Color32 faceColor
	{
		get
		{
			if (m_sharedMaterial == null)
			{
				return m_faceColor;
			}
			m_faceColor = m_sharedMaterial.GetColor(ShaderUtilities.ID_FaceColor);
			return m_faceColor;
		}
		set
		{
			if (!m_faceColor.Compare(value))
			{
				SetFaceColor(value);
				m_havePropertiesChanged = true;
				m_faceColor = value;
				SetVerticesDirty();
				SetMaterialDirty();
			}
		}
	}

	public Color32 outlineColor
	{
		get
		{
			if (m_sharedMaterial == null)
			{
				return m_outlineColor;
			}
			m_outlineColor = m_sharedMaterial.GetColor(ShaderUtilities.ID_OutlineColor);
			return m_outlineColor;
		}
		set
		{
			if (!m_outlineColor.Compare(value))
			{
				SetOutlineColor(value);
				m_havePropertiesChanged = true;
				m_outlineColor = value;
				SetVerticesDirty();
			}
		}
	}

	public float outlineWidth
	{
		get
		{
			if (m_sharedMaterial == null)
			{
				return m_outlineWidth;
			}
			m_outlineWidth = m_sharedMaterial.GetFloat(ShaderUtilities.ID_OutlineWidth);
			return m_outlineWidth;
		}
		set
		{
			if (m_outlineWidth != value)
			{
				SetOutlineThickness(value);
				m_havePropertiesChanged = true;
				m_outlineWidth = value;
				SetVerticesDirty();
			}
		}
	}

	public float fontSize
	{
		get
		{
			return m_fontSize;
		}
		set
		{
			if (m_fontSize != value)
			{
				m_havePropertiesChanged = true;
				m_fontSize = value;
				if (!m_enableAutoSizing)
				{
					m_fontSizeBase = m_fontSize;
				}
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public FontWeight fontWeight
	{
		get
		{
			return m_fontWeight;
		}
		set
		{
			if (m_fontWeight != value)
			{
				m_fontWeight = value;
				m_havePropertiesChanged = true;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public float pixelsPerUnit
	{
		get
		{
			Canvas canvas = base.canvas;
			if (!canvas)
			{
				return 1f;
			}
			if (!font)
			{
				return canvas.scaleFactor;
			}
			if (m_currentFontAsset == null || m_currentFontAsset.faceInfo.pointSize <= 0 || m_fontSize <= 0f)
			{
				return 1f;
			}
			return m_fontSize / (float)m_currentFontAsset.faceInfo.pointSize;
		}
	}

	public bool enableAutoSizing
	{
		get
		{
			return m_enableAutoSizing;
		}
		set
		{
			if (m_enableAutoSizing != value)
			{
				m_enableAutoSizing = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public float fontSizeMin
	{
		get
		{
			return m_fontSizeMin;
		}
		set
		{
			if (m_fontSizeMin != value)
			{
				m_fontSizeMin = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public float fontSizeMax
	{
		get
		{
			return m_fontSizeMax;
		}
		set
		{
			if (m_fontSizeMax != value)
			{
				m_fontSizeMax = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public FontStyles fontStyle
	{
		get
		{
			return m_fontStyle;
		}
		set
		{
			if (m_fontStyle != value)
			{
				m_fontStyle = value;
				m_havePropertiesChanged = true;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public bool isUsingBold => m_isUsingBold;

	public HorizontalAlignmentOptions horizontalAlignment
	{
		get
		{
			return m_HorizontalAlignment;
		}
		set
		{
			if (m_HorizontalAlignment != value)
			{
				m_HorizontalAlignment = value;
				m_havePropertiesChanged = true;
				SetVerticesDirty();
			}
		}
	}

	public VerticalAlignmentOptions verticalAlignment
	{
		get
		{
			return m_VerticalAlignment;
		}
		set
		{
			if (m_VerticalAlignment != value)
			{
				m_VerticalAlignment = value;
				m_havePropertiesChanged = true;
				SetVerticesDirty();
			}
		}
	}

	public TextAlignmentOptions alignment
	{
		get
		{
			return (TextAlignmentOptions)((int)m_HorizontalAlignment | (int)m_VerticalAlignment);
		}
		set
		{
			HorizontalAlignmentOptions horizontalAlignmentOptions = (HorizontalAlignmentOptions)(value & (TextAlignmentOptions)255);
			VerticalAlignmentOptions verticalAlignmentOptions = (VerticalAlignmentOptions)(value & (TextAlignmentOptions)65280);
			if (m_HorizontalAlignment != horizontalAlignmentOptions || m_VerticalAlignment != verticalAlignmentOptions)
			{
				m_HorizontalAlignment = horizontalAlignmentOptions;
				m_VerticalAlignment = verticalAlignmentOptions;
				m_havePropertiesChanged = true;
				SetVerticesDirty();
			}
		}
	}

	public float characterSpacing
	{
		get
		{
			return m_characterSpacing;
		}
		set
		{
			if (m_characterSpacing != value)
			{
				m_havePropertiesChanged = true;
				m_characterSpacing = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public float wordSpacing
	{
		get
		{
			return m_wordSpacing;
		}
		set
		{
			if (m_wordSpacing != value)
			{
				m_havePropertiesChanged = true;
				m_wordSpacing = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public float lineSpacing
	{
		get
		{
			return m_lineSpacing;
		}
		set
		{
			if (m_lineSpacing != value)
			{
				m_havePropertiesChanged = true;
				m_lineSpacing = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public float lineSpacingAdjustment
	{
		get
		{
			return m_lineSpacingMax;
		}
		set
		{
			if (m_lineSpacingMax != value)
			{
				m_havePropertiesChanged = true;
				m_lineSpacingMax = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public float paragraphSpacing
	{
		get
		{
			return m_paragraphSpacing;
		}
		set
		{
			if (m_paragraphSpacing != value)
			{
				m_havePropertiesChanged = true;
				m_paragraphSpacing = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public float characterWidthAdjustment
	{
		get
		{
			return m_charWidthMaxAdj;
		}
		set
		{
			if (m_charWidthMaxAdj != value)
			{
				m_havePropertiesChanged = true;
				m_charWidthMaxAdj = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public bool enableWordWrapping
	{
		get
		{
			return m_enableWordWrapping;
		}
		set
		{
			if (m_enableWordWrapping != value)
			{
				m_havePropertiesChanged = true;
				m_enableWordWrapping = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public float wordWrappingRatios
	{
		get
		{
			return m_wordWrappingRatios;
		}
		set
		{
			if (m_wordWrappingRatios != value)
			{
				m_wordWrappingRatios = value;
				m_havePropertiesChanged = true;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public TextOverflowModes overflowMode
	{
		get
		{
			return m_overflowMode;
		}
		set
		{
			if (m_overflowMode != value)
			{
				m_overflowMode = value;
				m_havePropertiesChanged = true;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public bool isTextOverflowing
	{
		get
		{
			if (m_firstOverflowCharacterIndex != -1)
			{
				return true;
			}
			return false;
		}
	}

	public int firstOverflowCharacterIndex => m_firstOverflowCharacterIndex;

	public TMP_Text linkedTextComponent
	{
		get
		{
			return m_linkedTextComponent;
		}
		set
		{
			if (value == null)
			{
				ReleaseLinkedTextComponent(m_linkedTextComponent);
				m_linkedTextComponent = value;
			}
			else
			{
				if (IsSelfOrLinkedAncestor(value))
				{
					return;
				}
				ReleaseLinkedTextComponent(m_linkedTextComponent);
				m_linkedTextComponent = value;
				m_linkedTextComponent.parentLinkedComponent = this;
			}
			m_havePropertiesChanged = true;
			SetVerticesDirty();
			SetLayoutDirty();
		}
	}

	public bool isTextTruncated => m_isTextTruncated;

	public bool enableKerning
	{
		get
		{
			return m_enableKerning;
		}
		set
		{
			if (m_enableKerning != value)
			{
				m_havePropertiesChanged = true;
				m_enableKerning = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public bool extraPadding
	{
		get
		{
			return m_enableExtraPadding;
		}
		set
		{
			if (m_enableExtraPadding != value)
			{
				m_havePropertiesChanged = true;
				m_enableExtraPadding = value;
				UpdateMeshPadding();
				SetVerticesDirty();
			}
		}
	}

	public bool richText
	{
		get
		{
			return m_isRichText;
		}
		set
		{
			if (m_isRichText != value)
			{
				m_isRichText = value;
				m_havePropertiesChanged = true;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public bool parseCtrlCharacters
	{
		get
		{
			return m_parseCtrlCharacters;
		}
		set
		{
			if (m_parseCtrlCharacters != value)
			{
				m_parseCtrlCharacters = value;
				m_havePropertiesChanged = true;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public bool isOverlay
	{
		get
		{
			return m_isOverlay;
		}
		set
		{
			if (m_isOverlay != value)
			{
				m_isOverlay = value;
				SetShaderDepth();
				m_havePropertiesChanged = true;
				SetVerticesDirty();
			}
		}
	}

	public bool isOrthographic
	{
		get
		{
			return m_isOrthographic;
		}
		set
		{
			if (m_isOrthographic != value)
			{
				m_havePropertiesChanged = true;
				m_isOrthographic = value;
				SetVerticesDirty();
			}
		}
	}

	public bool enableCulling
	{
		get
		{
			return m_isCullingEnabled;
		}
		set
		{
			if (m_isCullingEnabled != value)
			{
				m_isCullingEnabled = value;
				SetCulling();
				m_havePropertiesChanged = true;
			}
		}
	}

	public bool ignoreVisibility
	{
		get
		{
			return m_ignoreCulling;
		}
		set
		{
			if (m_ignoreCulling != value)
			{
				m_havePropertiesChanged = true;
				m_ignoreCulling = value;
			}
		}
	}

	public TextureMappingOptions horizontalMapping
	{
		get
		{
			return m_horizontalMapping;
		}
		set
		{
			if (m_horizontalMapping != value)
			{
				m_havePropertiesChanged = true;
				m_horizontalMapping = value;
				SetVerticesDirty();
			}
		}
	}

	public TextureMappingOptions verticalMapping
	{
		get
		{
			return m_verticalMapping;
		}
		set
		{
			if (m_verticalMapping != value)
			{
				m_havePropertiesChanged = true;
				m_verticalMapping = value;
				SetVerticesDirty();
			}
		}
	}

	public float mappingUvLineOffset
	{
		get
		{
			return m_uvLineOffset;
		}
		set
		{
			if (m_uvLineOffset != value)
			{
				m_havePropertiesChanged = true;
				m_uvLineOffset = value;
				SetVerticesDirty();
			}
		}
	}

	public TextRenderFlags renderMode
	{
		get
		{
			return m_renderMode;
		}
		set
		{
			if (m_renderMode != value)
			{
				m_renderMode = value;
				m_havePropertiesChanged = true;
			}
		}
	}

	public VertexSortingOrder geometrySortingOrder
	{
		get
		{
			return m_geometrySortingOrder;
		}
		set
		{
			m_geometrySortingOrder = value;
			m_havePropertiesChanged = true;
			SetVerticesDirty();
		}
	}

	public bool isTextObjectScaleStatic
	{
		get
		{
			return m_IsTextObjectScaleStatic;
		}
		set
		{
			m_IsTextObjectScaleStatic = value;
			if (m_IsTextObjectScaleStatic)
			{
				TMP_UpdateManager.UnRegisterTextObjectForUpdate(this);
			}
			else
			{
				TMP_UpdateManager.RegisterTextObjectForUpdate(this);
			}
		}
	}

	public bool vertexBufferAutoSizeReduction
	{
		get
		{
			return m_VertexBufferAutoSizeReduction;
		}
		set
		{
			m_VertexBufferAutoSizeReduction = value;
			m_havePropertiesChanged = true;
			SetVerticesDirty();
		}
	}

	public int firstVisibleCharacter
	{
		get
		{
			return m_firstVisibleCharacter;
		}
		set
		{
			if (m_firstVisibleCharacter != value)
			{
				m_havePropertiesChanged = true;
				m_firstVisibleCharacter = value;
				SetVerticesDirty();
			}
		}
	}

	public int maxVisibleCharacters
	{
		get
		{
			return m_maxVisibleCharacters;
		}
		set
		{
			if (m_maxVisibleCharacters != value)
			{
				m_havePropertiesChanged = true;
				m_maxVisibleCharacters = value;
				SetVerticesDirty();
			}
		}
	}

	public int maxVisibleWords
	{
		get
		{
			return m_maxVisibleWords;
		}
		set
		{
			if (m_maxVisibleWords != value)
			{
				m_havePropertiesChanged = true;
				m_maxVisibleWords = value;
				SetVerticesDirty();
			}
		}
	}

	public int maxVisibleLines
	{
		get
		{
			return m_maxVisibleLines;
		}
		set
		{
			if (m_maxVisibleLines != value)
			{
				m_havePropertiesChanged = true;
				m_maxVisibleLines = value;
				SetVerticesDirty();
			}
		}
	}

	public bool useMaxVisibleDescender
	{
		get
		{
			return m_useMaxVisibleDescender;
		}
		set
		{
			if (m_useMaxVisibleDescender != value)
			{
				m_havePropertiesChanged = true;
				m_useMaxVisibleDescender = value;
				SetVerticesDirty();
			}
		}
	}

	public int pageToDisplay
	{
		get
		{
			return m_pageToDisplay;
		}
		set
		{
			if (m_pageToDisplay != value)
			{
				m_havePropertiesChanged = true;
				m_pageToDisplay = value;
				SetVerticesDirty();
			}
		}
	}

	public virtual Vector4 margin
	{
		get
		{
			return m_margin;
		}
		set
		{
			if (!(m_margin == value))
			{
				m_margin = value;
				ComputeMarginSize();
				m_havePropertiesChanged = true;
				SetVerticesDirty();
			}
		}
	}

	public TMP_TextInfo textInfo => m_textInfo;

	public bool havePropertiesChanged
	{
		get
		{
			return m_havePropertiesChanged;
		}
		set
		{
			if (m_havePropertiesChanged != value)
			{
				m_havePropertiesChanged = value;
				SetAllDirty();
			}
		}
	}

	public bool isUsingLegacyAnimationComponent
	{
		get
		{
			return m_isUsingLegacyAnimationComponent;
		}
		set
		{
			m_isUsingLegacyAnimationComponent = value;
		}
	}

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

	public new RectTransform rectTransform
	{
		get
		{
			if (m_rectTransform == null)
			{
				m_rectTransform = GetComponent<RectTransform>();
			}
			return m_rectTransform;
		}
	}

	public virtual bool autoSizeTextContainer { get; set; }

	public virtual Mesh mesh => m_mesh;

	public bool isVolumetricText
	{
		get
		{
			return m_isVolumetricText;
		}
		set
		{
			if (m_isVolumetricText != value)
			{
				m_havePropertiesChanged = value;
				m_textInfo.ResetVertexLayout(value);
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public Bounds bounds
	{
		get
		{
			if (m_mesh == null)
			{
				return default(Bounds);
			}
			return GetCompoundBounds();
		}
	}

	public Bounds textBounds
	{
		get
		{
			if (m_textInfo == null)
			{
				return default(Bounds);
			}
			return GetTextBounds();
		}
	}

	protected TMP_SpriteAnimator spriteAnimator
	{
		get
		{
			if (m_spriteAnimator == null)
			{
				m_spriteAnimator = GetComponent<TMP_SpriteAnimator>();
				if (m_spriteAnimator == null)
				{
					m_spriteAnimator = base.gameObject.AddComponent<TMP_SpriteAnimator>();
				}
			}
			return m_spriteAnimator;
		}
	}

	public float flexibleHeight => m_flexibleHeight;

	public float flexibleWidth => m_flexibleWidth;

	public float minWidth => m_minWidth;

	public float minHeight => m_minHeight;

	public float maxWidth => m_maxWidth;

	public float maxHeight => m_maxHeight;

	protected LayoutElement layoutElement
	{
		get
		{
			if (m_LayoutElement == null)
			{
				m_LayoutElement = GetComponent<LayoutElement>();
			}
			return m_LayoutElement;
		}
	}

	public virtual float preferredWidth
	{
		get
		{
			m_preferredWidth = GetPreferredWidth();
			return m_preferredWidth;
		}
	}

	public virtual float preferredHeight
	{
		get
		{
			m_preferredHeight = GetPreferredHeight();
			return m_preferredHeight;
		}
	}

	public virtual float renderedWidth => GetRenderedWidth();

	public virtual float renderedHeight => GetRenderedHeight();

	public int layoutPriority => m_layoutPriority;

	private StringBuilder arabicFixsb
	{
		get
		{
			if (_arabicFixsb == null)
			{
				_arabicFixsb = new StringBuilder();
			}
			else
			{
				_arabicFixsb.Clear();
			}
			return _arabicFixsb;
		}
		set
		{
			_arabicFixsb = value;
		}
	}

	public bool supportSpriteEmoji
	{
		get
		{
			return m_supportSpriteEmoji;
		}
		set
		{
			if (m_supportSpriteEmoji != value)
			{
				m_supportSpriteEmoji = value;
				m_havePropertiesChanged = true;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public bool supportSpriteTagOnly
	{
		get
		{
			if (m_supportSpriteTagOnly)
			{
				return !m_isRichText;
			}
			return false;
		}
		set
		{
			if (m_supportSpriteTagOnly != value)
			{
				m_supportSpriteTagOnly = value;
				m_havePropertiesChanged = true;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public static event Func<int, string, TMP_FontAsset> OnFontAssetRequest;

	public static event Func<int, string, TMP_SpriteAsset> OnSpriteAssetRequest;

	public virtual event Action<TMP_TextInfo> OnPreRenderText = delegate
	{
	};

	protected virtual void LoadFontAsset()
	{
	}

	protected virtual void SetSharedMaterial(Material mat)
	{
	}

	protected virtual Material GetMaterial(Material mat)
	{
		return null;
	}

	protected virtual void SetFontBaseMaterial(Material mat)
	{
	}

	protected virtual Material[] GetSharedMaterials()
	{
		return null;
	}

	protected virtual void SetSharedMaterials(Material[] materials)
	{
	}

	protected virtual Material[] GetMaterials(Material[] mats)
	{
		return null;
	}

	protected virtual Material CreateMaterialInstance(Material source)
	{
		Material obj = new Material(source)
		{
			shaderKeywords = source.shaderKeywords
		};
		obj.name += " (Instance)";
		return obj;
	}

	protected void SetVertexColorGradient(TMP_ColorGradient gradient)
	{
		if (!(gradient == null))
		{
			m_fontColorGradient.bottomLeft = gradient.bottomLeft;
			m_fontColorGradient.bottomRight = gradient.bottomRight;
			m_fontColorGradient.topLeft = gradient.topLeft;
			m_fontColorGradient.topRight = gradient.topRight;
			SetVerticesDirty();
		}
	}

	protected void SetTextSortingOrder(VertexSortingOrder order)
	{
	}

	protected void SetTextSortingOrder(int[] order)
	{
	}

	protected virtual void SetFaceColor(Color32 color)
	{
	}

	protected virtual void SetOutlineColor(Color32 color)
	{
	}

	protected virtual void SetOutlineThickness(float thickness)
	{
	}

	protected virtual void SetShaderDepth()
	{
	}

	protected virtual void SetCulling()
	{
	}

	internal virtual void UpdateCulling()
	{
	}

	protected virtual float GetPaddingForMaterial()
	{
		ShaderUtilities.GetShaderPropertyIDs();
		if (m_sharedMaterial == null)
		{
			return 0f;
		}
		m_padding = ShaderUtilities.GetPadding(m_sharedMaterial, m_enableExtraPadding, m_isUsingBold);
		m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(m_sharedMaterial);
		m_isSDFShader = m_sharedMaterial.HasProperty(ShaderUtilities.ID_WeightNormal);
		return m_padding;
	}

	protected virtual float GetPaddingForMaterial(Material mat)
	{
		if (mat == null)
		{
			return 0f;
		}
		m_padding = ShaderUtilities.GetPadding(mat, m_enableExtraPadding, m_isUsingBold);
		m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(m_sharedMaterial);
		m_isSDFShader = mat.HasProperty(ShaderUtilities.ID_WeightNormal);
		return m_padding;
	}

	protected virtual Vector3[] GetTextContainerLocalCorners()
	{
		return null;
	}

	public virtual void ForceMeshUpdate(bool ignoreActiveState = false, bool forceTextReparsing = false)
	{
	}

	public virtual void UpdateGeometry(Mesh mesh, int index)
	{
	}

	public virtual void UpdateVertexData(TMP_VertexDataUpdateFlags flags)
	{
	}

	public virtual void UpdateVertexData()
	{
	}

	public virtual void SetVertices(Vector3[] vertices)
	{
	}

	public virtual void UpdateMeshPadding()
	{
	}

	public override void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
	{
		base.CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
		InternalCrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
	}

	public override void CrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
	{
		base.CrossFadeAlpha(alpha, duration, ignoreTimeScale);
		InternalCrossFadeAlpha(alpha, duration, ignoreTimeScale);
	}

	protected virtual void InternalCrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
	{
	}

	protected virtual void InternalCrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
	{
	}

	protected void ParseInputText()
	{
		switch (m_inputSource)
		{
		case TextInputSources.TextInputBox:
		case TextInputSources.TextString:
			PopulateTextBackingArray((m_TextPreprocessor == null) ? m_text : m_TextPreprocessor.PreprocessText(m_text));
			PopulateTextProcessingArray();
			break;
		}
		SetArraySizes(m_TextProcessingArray);
	}

	private void PopulateTextBackingArray(string sourceText)
	{
		sourceText = TrySetArabicTMPFix(sourceText);
		int length = sourceText?.Length ?? 0;
		PopulateTextBackingArray(sourceText, 0, length);
	}

	private string TrySetArabicTMPFix(string sourceText)
	{
		string result = sourceText;
		if (hasArabicChar)
		{
			int length = sourceText.Length;
			if (length >= m_TMPFixCharLenArray.Capacity)
			{
				m_TMPFixCharLenArray.Resize(length);
			}
			if (length >= m_TMPFixCharArray.Capacity)
			{
				m_TMPFixCharArray.Resize(length);
			}
			for (int i = 0; i < length; i++)
			{
				m_TMPFixCharLenArray[i] = 1u;
				m_TMPFixCharArray[i] = sourceText[i];
			}
			m_TMPFixCharLenArray.Count = length;
			m_TMPFixCharArray.Count = length;
			m_TMPFixCharLenArray[length] = 0u;
			m_TMPFixCharArray[length] = 0u;
			SetInputHtmlRangeListSort();
			if (isArabicLanType)
			{
				SetArabicTxtFix(0, m_TMPFixCharArray.Count - 1);
			}
			else
			{
				for (int j = 0; j < length; j++)
				{
					if (isArabic(m_TMPFixCharArray[j]))
					{
						int rangeS = j;
						int rangeE = j;
						for (bool flag = j + 1 < length && isArabic(m_TMPFixCharArray[j + 1]); j < length && (isArabic(m_TMPFixCharArray[j]) || (m_TMPFixCharArray[j] == 32 && flag)); j++)
						{
							rangeE = j;
						}
						SetArabicTxtFix(rangeS, rangeE, isReverse: false);
					}
				}
			}
			ReSetTMPFixCharArray();
			result = GetTMPFixStr(0, m_TMPFixCharArray.Count - 1);
		}
		return result;
	}

	private string GetTMPFixStr(int rangeS, int rangeE)
	{
		StringBuilder stringBuilder = arabicFixsb;
		for (int i = rangeS; i < rangeE + 1; i++)
		{
			if (m_TMPFixCharArray[i] != ArabicEmptyChar)
			{
				char value = (char)m_TMPFixCharArray[i];
				stringBuilder.Append(value);
			}
		}
		return stringBuilder.ToString();
	}

	private int ClearTMPFixEmptyChar(int rangeS, int rangeE)
	{
		int num = 0;
		for (int i = rangeS; i < rangeE + 1; i++)
		{
			if (m_TMPFixCharArray[i] != ArabicEmptyChar)
			{
				m_TMPFixCharArray[rangeS + num] = m_TMPFixCharArray[i];
				m_TMPFixCharLenArray[rangeS + num] = m_TMPFixCharLenArray[i];
				num++;
			}
		}
		for (int j = rangeS + num; j < rangeE + 1; j++)
		{
			m_TMPFixCharArray[j] = ArabicEmptyChar;
			m_TMPFixCharLenArray[j] = 0u;
		}
		return num;
	}

	private void ReSetTMPFixCharArray()
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < m_TMPFixCharArray.Count; i++)
		{
			if (m_TMPFixCharArray[i] == ArabicEmptyChar)
			{
				num2++;
				continue;
			}
			m_TMPFixCharArray[num] = m_TMPFixCharArray[i];
			m_TMPFixCharLenArray[num] = m_TMPFixCharLenArray[i];
			num++;
		}
		m_TMPFixCharArray.Count = num;
		m_TMPFixCharLenArray.Count = num;
		m_TMPFixCharArray[num] = 0u;
		m_TMPFixCharLenArray[num] = 0u;
	}

	private void SetInputHtmlRangeListSort()
	{
		if (inputHtmlRangeList.Count > 0)
		{
			inputHtmlRangeList.Sort((InputHtmlRange a, InputHtmlRange b) => (a.startIndex >= b.startIndex) ? 1 : (-1));
		}
	}

	private void SetArabicTxtFix(int rangeS, int rangeE, bool isReverse = true)
	{
		if (rangeS < 0 || rangeE < 0 || rangeS > rangeE)
		{
			return;
		}
		bool flag = false;
		if (!richText)
		{
			if (inputHtmlRangeList.Count > 0)
			{
				int num = rangeS;
				for (int i = 0; i < inputHtmlRangeList.Count; i++)
				{
					InputHtmlRange inputHtmlRange = inputHtmlRangeList[i];
					if (inputHtmlRange.isArabicFixIgnore || ((num > inputHtmlRange.startIndex || inputHtmlRange.endIndex >= rangeE) && (num >= inputHtmlRange.startIndex || inputHtmlRange.endIndex > rangeE)))
					{
						continue;
					}
					flag = true;
					if (num < inputHtmlRange.startIndex)
					{
						SetArabicTxtFix(num, inputHtmlRange.startIndex - 1, isReverse);
						int num2 = inputHtmlRange.startIndex;
						if (m_TMPFixCharArray[num2] == 64)
						{
							num2++;
						}
						SetArabicTxtFix(num2, inputHtmlRange.endIndex, isReverse);
						num = inputHtmlRange.endIndex + 1;
					}
					else
					{
						int num3 = inputHtmlRange.startIndex;
						if (m_TMPFixCharArray[num3] == 64)
						{
							num3++;
						}
						SetArabicTxtFix(num3, inputHtmlRange.endIndex, isReverse);
						num = inputHtmlRange.endIndex + 1;
					}
				}
				if (flag && num <= rangeE)
				{
					SetArabicTxtFix(num, rangeE, isReverse);
				}
			}
		}
		else
		{
			int num4 = 0;
			bool flag2 = false;
			bool flag3 = false;
			int num5 = -1;
			int num6 = -1;
			int num7 = -1;
			int num8 = -1;
			int num9 = -1;
			int num10 = rangeE;
			for (int num11 = rangeE; num11 >= rangeS; num11--)
			{
				uint num12 = m_TMPFixCharArray[num11];
				uint num13 = ArabicEmptyChar;
				_ = ArabicEmptyChar;
				if (num11 - 1 >= rangeS)
				{
					num13 = m_TMPFixCharArray[num11 - 1];
				}
				if (num11 + 1 <= rangeE)
				{
					_ = m_TMPFixCharArray[num11 + 1];
				}
				if (num12 == 62 && num13 != 45)
				{
					num4 = 0;
					flag2 = false;
					flag3 = false;
					for (int num14 = num11 - 1; num14 >= rangeS; num14--)
					{
						if (m_TMPFixCharArray[num14] == 60)
						{
							if (m_TMPFixCharArray[num14 + 1] != 32)
							{
								flag2 = true;
								num5 = num14;
								num6 = num11;
								if (m_TMPFixCharArray[num14 + 1] == 47)
								{
									flag3 = true;
									num4 = 1;
								}
								num9 = num14;
							}
							break;
						}
					}
					if (flag2)
					{
						num11 = num9;
						if (flag3)
						{
							bool flag4 = false;
							while (num11 >= rangeS)
							{
								if (m_TMPFixCharArray[num11] == 62 && (num11 == rangeS || m_TMPFixCharArray[num11 - 1] != 45))
								{
									for (int num15 = num11 - 1; num15 >= rangeS; num15--)
									{
										if (m_TMPFixCharArray[num15] == 60)
										{
											if (m_TMPFixCharArray[num15 + 1] != 32 && m_TMPFixCharArray[num15 + 1] != 47)
											{
												num4--;
												if (num4 <= 0)
												{
													flag4 = true;
													num7 = num15;
													num8 = num11;
												}
											}
											else
											{
												num4++;
											}
											break;
										}
									}
									if (flag4)
									{
										break;
									}
								}
								num11--;
							}
							if (flag4)
							{
								if (num10 >= num6 + 1)
								{
									SetArabicTxtFix(num6 + 1, num10, isReverse);
								}
								if (num8 + 1 <= num5 - 1)
								{
									SetArabicTxtFix(num8 + 1, num5 - 1, isReverse);
								}
								num10 = num7 - 1;
								flag = true;
							}
							else
							{
								if (num10 >= num6 + 1)
								{
									SetArabicTxtFix(num6 + 1, num10, isReverse);
								}
								num10 = num5 - 1;
								flag = true;
							}
						}
						else
						{
							if (num10 >= num6 + 1)
							{
								SetArabicTxtFix(num6 + 1, num10, isReverse);
							}
							num10 = num5 - 1;
							flag = true;
						}
					}
				}
			}
			if (flag && num10 >= rangeS)
			{
				SetArabicTxtFix(rangeS, num10, isReverse);
			}
		}
		if (!flag)
		{
			ArabicTxtFixAllah(rangeS, rangeE);
			ArabicTxtGlyphFixer(rangeS, rangeE);
			ArabicTxtTashkeelFix(rangeS, rangeE);
			ArabicTxtLigatureFix(rangeS, rangeE, isReverse);
		}
	}

	private void ArabicTxtFixAllah(int rangeS, int rangeE)
	{
		if (rangeS == rangeE)
		{
			return;
		}
		bool flag = false;
		string tMPFixStr = GetTMPFixStr(rangeS, rangeE);
		foreach (Match item in Regex.Matches(tMPFixStr, BoundaryStart + "ا" + Diacritics + "ل" + Diacritics + "ل" + Diacritics + "ه" + Diacritics + BoundaryEnd))
		{
			flag = true;
			int length = item.Groups[0].Length;
			int length2 = item.Groups[1].Length;
			int num = item.Groups[1].Index + length2;
			m_TMPFixCharLenArray[rangeS + num] = (uint)(length - length2);
			m_TMPFixCharArray[rangeS + num] = 65010u;
			for (int i = 1; i < length - length2; i++)
			{
				m_TMPFixCharLenArray[rangeS + num + i] = 0u;
				m_TMPFixCharArray[rangeS + num + i] = ArabicEmptyChar;
			}
		}
		if (flag)
		{
			ClearTMPFixEmptyChar(rangeS, rangeE);
			tMPFixStr = GetTMPFixStr(rangeS, rangeE);
		}
		flag = false;
		foreach (Match item2 in Regex.Matches(tMPFixStr, BoundaryStart + "ل" + Diacritics + "ل" + Diacritics + "ه" + Diacritics + BoundaryEnd))
		{
			flag = true;
			int length3 = item2.Groups[0].Length;
			int length4 = item2.Groups[1].Length;
			int num2 = item2.Groups[1].Index + length4;
			m_TMPFixCharLenArray[rangeS + num2] = (uint)(length3 - length4);
			m_TMPFixCharArray[rangeS + num2] = 57600u;
			for (int j = 1; j < length3 - length4; j++)
			{
				m_TMPFixCharLenArray[rangeS + num2 + j] = 0u;
				m_TMPFixCharArray[rangeS + num2 + j] = ArabicEmptyChar;
			}
		}
		if (flag)
		{
			ClearTMPFixEmptyChar(rangeS, rangeE);
			tMPFixStr = GetTMPFixStr(rangeS, rangeE);
		}
		foreach (Match item3 in Regex.Matches(tMPFixStr, BoundaryStart + "ب" + Diacritics + "ا" + Diacritics + "ل" + Diacritics + "ل" + Diacritics + "ه" + Diacritics + BoundaryEnd))
		{
			int length5 = item3.Groups[0].Length;
			int length6 = item3.Groups[1].Length;
			int num3 = item3.Groups[1].Index + length6;
			m_TMPFixCharLenArray[rangeS + num3] = (uint)(length5 - length6);
			m_TMPFixCharArray[rangeS + num3] = 57601u;
			for (int k = 1; k < length5 - length6; k++)
			{
				m_TMPFixCharLenArray[rangeS + num3 + k] = 0u;
				m_TMPFixCharArray[rangeS + num3 + k] = ArabicEmptyChar;
			}
		}
	}

	private void ArabicTxtGlyphFixer(int rangeS, int rangeE)
	{
		TashkeelLocations.Clear();
		int num = ClearTMPFixEmptyChar(rangeS, rangeE);
		for (int i = rangeS; i <= rangeE && m_TMPFixCharArray[i] != ArabicEmptyChar; i++)
		{
			uint num2 = m_TMPFixCharArray[i];
			uint len = m_TMPFixCharLenArray[i];
			if (Char32Utils.IsUnicode16Char((int)num2) && TashkeelCharactersSet.Contains((char)num2))
			{
				TashkeelLocations.Add(new TashkeelLocation(num2, i, len));
				m_TMPFixCharArray[i] = ArabicEmptyChar;
			}
		}
		int num3 = ClearTMPFixEmptyChar(rangeS, rangeE);
		int num4 = rangeS + num3 - 1;
		for (int j = rangeS; j <= num4; j++)
		{
			if (farsi && m_TMPFixCharArray[j] == 1610)
			{
				m_TMPFixCharArray[j] = 1740u;
			}
			else if (!farsi && m_TMPFixCharArray[j] == 1740)
			{
				m_TMPFixCharArray[j] = 1610u;
			}
		}
		uint[] array = null;
		if (num4 - rangeS + 1 > 0)
		{
			array = new uint[num4 - rangeS + 1];
		}
		for (int k = rangeS; k <= num4; k++)
		{
			bool flag = false;
			uint num5 = m_TMPFixCharArray[k];
			int num6 = -1;
			if (num5 == 1604 && k <= num4 - 1)
			{
				uint num7 = m_TMPFixCharArray[k + 1];
				bool flag2 = false;
				switch (num7)
				{
				case 1573u:
					num6 = 65271;
					flag2 = true;
					break;
				case 1575u:
					num6 = 65273;
					flag2 = true;
					break;
				case 1571u:
					num6 = 65269;
					flag2 = true;
					break;
				case 1570u:
					num6 = 65267;
					flag2 = true;
					break;
				default:
					flag2 = false;
					break;
				}
				if (flag2)
				{
					flag = true;
				}
				if (flag)
				{
					num5 = (uint)num6;
				}
			}
			if (num5 == 1600 || num5 == 8204)
			{
				array[k - rangeS] = num5;
				continue;
			}
			if (num5 < 65535 && TextUtils.IsGlyphFixedArabicCharacter((char)num5))
			{
				char c = GlyphTable.Convert((char)num5);
				num5 = (uint)(IsMiddleLetter(m_TMPFixCharArray, rangeS, num4, k) ? (c + 3) : (IsFinishingLetter(m_TMPFixCharArray, rangeS, num4, k) ? (c + 1) : ((!IsLeadingLetter(m_TMPFixCharArray, rangeS, num4, k)) ? c : (c + 2))));
			}
			array[k - rangeS] = num5;
			if (flag)
			{
				m_TMPFixCharLenArray[k] += m_TMPFixCharLenArray[k + 1];
				m_TMPFixCharLenArray[k + 1] = 0u;
				k++;
			}
		}
		for (int l = rangeS; l <= num4; l++)
		{
			if (m_TMPFixCharLenArray[l] == 0)
			{
				m_TMPFixCharArray[l] = ArabicEmptyChar;
			}
			else
			{
				m_TMPFixCharArray[l] = array[l - rangeS];
			}
		}
		int num8 = rangeS + num - 1;
		for (int num9 = TashkeelLocations.Count - 1; num9 >= 0; num9--)
		{
			TashkeelLocation tashkeelLocation = TashkeelLocations[num9];
			int position = tashkeelLocation.Position;
			for (int num10 = num8; num10 > position; num10--)
			{
				m_TMPFixCharArray[num10] = m_TMPFixCharArray[num10 - num9 - 1];
				m_TMPFixCharLenArray[num10] = m_TMPFixCharLenArray[num10 - num9 - 1];
			}
			m_TMPFixCharArray[position] = tashkeelLocation.Tashkeel;
			m_TMPFixCharLenArray[position] = tashkeelLocation.Len;
			num8 = position - 1;
		}
	}

	private void ArabicTxtTashkeelFix(int rangeS, int rangeE)
	{
		int num = ClearTMPFixEmptyChar(rangeS, rangeE);
		int num2 = rangeS + num - 1;
		for (int i = rangeS; i < num2; i++)
		{
			uint num3 = m_TMPFixCharArray[i];
			uint num4 = m_TMPFixCharArray[i + 1];
			if (num3 == 1617 && ShaddaCombinationMap.ContainsKey((char)num4))
			{
				m_TMPFixCharArray[i] = ShaddaCombinationMap[(char)num4];
				m_TMPFixCharLenArray[i] += m_TMPFixCharLenArray[i + 1];
				m_TMPFixCharArray[i + 1] = ArabicEmptyChar;
				m_TMPFixCharLenArray[i + 1] = 0u;
			}
		}
	}

	private void FlushBufferToOutput(List<int> buffer, List<OutPutTxt> output)
	{
		for (int i = 0; i < buffer.Count; i++)
		{
			int index = buffer[buffer.Count - 1 - i];
			output.Add(new OutPutTxt(m_TMPFixCharArray[index], m_TMPFixCharLenArray[index]));
		}
		buffer.Clear();
	}

	private void ArabicTxtLigatureFix(int rangeS, int rangeE, bool isReverse)
	{
		int num = ClearTMPFixEmptyChar(rangeS, rangeE);
		int num2 = rangeS + num - 1;
		List<int> list = new List<int>(512);
		List<int> list2 = new List<int>(512);
		list.Clear();
		list2.Clear();
		List<OutPutTxt> list3 = new List<OutPutTxt>();
		list3.Clear();
		for (int num3 = num2; num3 >= rangeS; num3--)
		{
			bool flag = num3 > rangeS && num3 < num2;
			bool flag2 = num3 == rangeS;
			bool flag3 = num3 == num2;
			int num4 = (int)m_TMPFixCharArray[num3];
			int num5 = 0;
			if (!flag3)
			{
				num5 = (int)m_TMPFixCharArray[num3 + 1];
			}
			int num6 = 0;
			if (!flag2)
			{
				num6 = (int)m_TMPFixCharArray[num3 - 1];
			}
			if (Char32Utils.IsPunctuation(num4) || Char32Utils.IsSymbol(num4))
			{
				bool flag4 = MirroredCharsSet.Contains((char)num4);
				if (flag4)
				{
					num4 = MirroredCharsMap[(char)num4];
					m_TMPFixCharArray[num3] = (uint)num4;
				}
				bool flag5 = Char32Utils.IsRTLCharacter(num6);
				bool flag6 = Char32Utils.IsRTLCharacter(num5);
				bool flag7 = Char32Utils.IsNumber(num6, preserveNumbers, farsi);
				bool flag8 = Char32Utils.IsNumber(num5, preserveNumbers, farsi);
				Char32Utils.IsEnglishLetter(num6);
				Char32Utils.IsEnglishLetter(num5);
				bool flag9 = Char32Utils.IsWhiteSpace(num5);
				bool flag10 = Char32Utils.IsWhiteSpace(num6);
				bool flag11 = num4 == 95;
				bool flag12 = num4 == 46 || num4 == 1548 || num4 == 45;
				bool flag13 = num4 == 46 && flag7;
				bool flag14 = num4 == 58 && (flag5 || flag9);
				bool flag15 = num4 == 34 || num4 == 1548 || num4 == 1563 || num4 == 33;
				bool flag16 = num4 == 43 || num4 == 45 || num4 == 42 || num4 == 215 || num4 == 47;
				bool flag17 = num4 == 46 || num4 == 1548;
				bool flag18 = num4 == 62 && num6 == 45;
				bool flag19 = num4 == 45 && num6 == 60;
				if (flag)
				{
					if (flag8 && flag7 && Char32Utils.IsPunctuation(num4) && !flag4)
					{
						list.Add(num3);
					}
					else if (flag18)
					{
						num3--;
						list3.Add(new OutPutTxt(60u, m_TMPFixCharLenArray[num3 + 1]));
						list3.Add(new OutPutTxt(45u, m_TMPFixCharLenArray[num3]));
					}
					else if (flag19)
					{
						num3--;
						list3.Add(new OutPutTxt(45u, m_TMPFixCharLenArray[num3 + 1]));
						list3.Add(new OutPutTxt(62u, m_TMPFixCharLenArray[num3]));
					}
					else if ((flag6 && flag5) || (flag10 && flag12) || flag14 || flag15 || (flag9 && flag5) || (flag9 && flag17) || (flag6 && flag10) || ((flag6 || flag5) && flag11) || flag13 || (flag16 && (flag5 || flag10 || flag6)))
					{
						FlushBufferToOutput(list, list3);
						list3.Add(new OutPutTxt(m_TMPFixCharArray[num3], m_TMPFixCharLenArray[num3]));
					}
					else if (!flag4)
					{
						list.Add(num3);
					}
					else
					{
						FlushBufferToOutput(list, list3);
						list3.Add(new OutPutTxt(m_TMPFixCharArray[num3], m_TMPFixCharLenArray[num3]));
					}
				}
				else if (flag3)
				{
					bool flag20 = num4 == 33 || num4 == 1563 || num4 == 46 || num4 == 1567 || num4 == 34 || num4 == 58;
					if (flag4 || flag20)
					{
						FlushBufferToOutput(list, list3);
						list3.Add(new OutPutTxt(m_TMPFixCharArray[num3], m_TMPFixCharLenArray[num3]));
					}
					else
					{
						list.Add(num3);
					}
				}
				else if (flag2)
				{
					FlushBufferToOutput(list, list3);
					list3.Add(new OutPutTxt(m_TMPFixCharArray[num3], m_TMPFixCharLenArray[num3]));
				}
				continue;
			}
			if (flag)
			{
				bool flag21 = Char32Utils.IsEnglishLetter(num6);
				bool flag22 = Char32Utils.IsEnglishLetter(num5);
				bool flag23 = Char32Utils.IsNumber(num6, preserveNumbers, farsi);
				bool flag24 = Char32Utils.IsNumber(num5, preserveNumbers, farsi);
				bool flag25 = Char32Utils.IsSymbol(num6);
				bool flag26 = Char32Utils.IsSymbol(num5);
				bool flag27 = Char32Utils.IsPunctuation(num6);
				bool flag28 = Char32Utils.IsPunctuation(num5);
				bool flag29 = num6 == 32;
				bool flag30 = num5 == 32;
				bool flag31 = num6 == 58;
				bool flag32 = num4 == 32 || num4 == 160;
				bool flag33 = MirroredCharsSet.Contains((char)num6);
				if (flag32 && ((flag31 && !flag30) || flag33))
				{
					FlushBufferToOutput(list, list3);
					list3.Add(new OutPutTxt(m_TMPFixCharArray[num3], m_TMPFixCharLenArray[num3]));
					continue;
				}
				if (flag32 && (flag22 || flag24 || flag26 || flag28 || flag30) && (flag21 || flag23 || flag25 || flag27 || flag29))
				{
					list.Add(num3);
					continue;
				}
			}
			if ((Char32Utils.IsLetter(num4) && !Char32Utils.IsRTLCharacter(num4)) || Char32Utils.IsNumber(num4, preserveNumbers, farsi))
			{
				list.Add(num3);
				continue;
			}
			if ((num4 >= 55296 && num4 <= 56319) || (num4 >= 56320 && num4 <= 57343))
			{
				list.Add(num3);
				continue;
			}
			FlushBufferToOutput(list, list3);
			list3.Add(new OutPutTxt(m_TMPFixCharArray[num3], m_TMPFixCharLenArray[num3]));
		}
		FlushBufferToOutput(list, list3);
		for (int i = rangeS; i <= num2; i++)
		{
			int num7 = -1;
			int count = list3.Count;
			if (isReverse)
			{
				if (count - 1 - (i - rangeS) >= 0)
				{
					num7 = count - 1 - (i - rangeS);
				}
			}
			else if (i - rangeS < count)
			{
				num7 = i - rangeS;
			}
			if (num7 < 0)
			{
				m_TMPFixCharArray[i] = ArabicEmptyChar;
				m_TMPFixCharLenArray[i] = 0u;
			}
			else
			{
				OutPutTxt outPutTxt = list3[num7];
				m_TMPFixCharArray[i] = outPutTxt.Char;
				m_TMPFixCharLenArray[i] = outPutTxt.Len;
			}
		}
		int num8 = ClearTMPFixEmptyChar(rangeS, rangeE);
		for (int j = rangeS; j <= rangeS + num8 - 1; j++)
		{
			if (m_TMPFixCharArray[j] == 8204 && j - 1 >= rangeS)
			{
				m_TMPFixCharLenArray[j - 1] += m_TMPFixCharLenArray[j];
				m_TMPFixCharArray[j] = ArabicEmptyChar;
				m_TMPFixCharLenArray[j] = 0u;
			}
		}
	}

	public static bool isArabic(uint c)
	{
		if (c >= 1536 && c <= 1791)
		{
			return true;
		}
		if (c >= 1872 && c <= 1919)
		{
			return true;
		}
		if (c >= 64336 && c <= 64575)
		{
			return true;
		}
		if (c >= 65136 && c <= 65276)
		{
			return true;
		}
		return false;
	}

	private static bool IsMiddleLetter(TextBackingContainer letters, int rangeS, int rangeE, int index)
	{
		uint num = letters[index];
		uint num2 = 0u;
		if (index - 1 >= rangeS)
		{
			num2 = letters[index - 1];
		}
		uint num3 = 0u;
		if (index + 1 <= rangeE)
		{
			num3 = letters[index + 1];
		}
		bool flag = index >= rangeS && index <= rangeE && num != 1569 && num != 1570 && num != 1571 && num != 1573 && num != 1572 && num != 1575 && num != 1583 && num != 1584 && num != 1585 && num != 1586 && num != 1688 && num != 1608 && num != 8204;
		bool flag2 = index - 1 >= rangeS && num2 != 65152 && num2 != 65153 && num2 != 65155 && num2 != 65159 && num2 != 65157 && num2 != 1575 && num2 != 1583 && num2 != 1584 && num2 != 1585 && num2 != 1586 && num2 != 1688 && num2 != 1608 && num2 != 1570 && num2 != 1571 && num2 != 1573 && num2 != 1572 && num2 != 1569 && num2 != 65165 && num2 != 65193 && num2 != 65195 && num2 != 65197 && num2 != 65199 && num2 != 64394 && num2 != 65261 && num2 != 8204 && num2 < 65535 && TextUtils.IsGlyphFixedArabicCharacter((char)num2);
		return index + 1 <= rangeE && num3 < 65535 && TextUtils.IsGlyphFixedArabicCharacter((char)num3) && num3 != 8204 && num3 != 1569 && num3 != 65152 && flag2 && flag;
	}

	private static bool IsFinishingLetter(TextBackingContainer letters, int rangeS, int rangeE, int index)
	{
		uint num = letters[index];
		uint num2 = 0u;
		if (index - 1 >= rangeS)
		{
			num2 = letters[index - 1];
		}
		bool num3 = index - 1 >= rangeS && num2 != 32 && num2 != 1569 && num2 != 1570 && num2 != 1571 && num2 != 1573 && num2 != 1572 && num2 != 1575 && num2 != 1583 && num2 != 1584 && num2 != 1585 && num2 != 1586 && num2 != 1688 && num2 != 1608 && num2 != 65152 && num2 != 65153 && num2 != 65155 && num2 != 65159 && num2 != 65157 && num2 != 65165 && num2 != 65193 && num2 != 65195 && num2 != 65197 && num2 != 65199 && num2 != 64394 && num2 != 65261 && num2 != 8204 && num2 < 65535 && TextUtils.IsGlyphFixedArabicCharacter((char)num2);
		bool flag = num != 32 && num != 8204 && num != 1569;
		return num3 && flag;
	}

	private static bool IsLeadingLetter(TextBackingContainer letters, int rangeS, int rangeE, int index)
	{
		uint num = letters[index];
		uint num2 = 0u;
		if (index - 1 >= rangeS)
		{
			num2 = letters[index - 1];
		}
		uint num3 = 0u;
		if (index + 1 <= rangeE)
		{
			num3 = letters[index + 1];
		}
		bool num4 = index == rangeS || (num2 < 65535 && !TextUtils.IsGlyphFixedArabicCharacter((char)num2)) || num2 == 1569 || num2 == 1570 || num2 == 1571 || num2 == 1573 || num2 == 1572 || num2 == 1575 || num2 == 1583 || num2 == 1584 || num2 == 1585 || num2 == 1586 || num2 == 1688 || num2 == 1608 || num2 == 65153 || num2 == 65155 || num2 == 65159 || num2 == 65157 || num2 == 65165 || num2 == 65152 || num2 == 65193 || num2 == 65195 || num2 == 65197 || num2 == 65199 || num2 == 64394 || num2 == 65261 || num2 == 8204;
		bool flag = num != 32 && num != 1569 && num != 1571 && num != 1573 && num != 1570 && num != 1572 && num != 1575 && num != 1583 && num != 1584 && num != 1585 && num != 1586 && num != 1688 && num != 1608 && num != 8204;
		bool flag2 = index + 1 <= rangeE && num3 < 65535 && TextUtils.IsGlyphFixedArabicCharacter((char)num3) && num3 != 1569 && num3 != 8204;
		return num4 && flag && flag2;
	}

	private void PopulateTextBackingArray(string sourceText, int start, int length)
	{
		int num = 0;
		int i;
		if (sourceText == null)
		{
			i = 0;
			length = 0;
		}
		else
		{
			i = Mathf.Clamp(start, 0, sourceText.Length);
			length = Mathf.Clamp(length, 0, (start + length < sourceText.Length) ? length : (sourceText.Length - start));
		}
		if (length >= m_TextBackingArray.Capacity)
		{
			m_TextBackingArray.Resize(length);
		}
		for (int num2 = i + length; i < num2; i++)
		{
			m_TextBackingArray[num] = sourceText[i];
			num++;
		}
		m_TextBackingArray[num] = 0u;
		m_TextBackingArray.Count = num;
		TryFixCRToLF();
	}

	private void TryFixCRToLF()
	{
		int count = m_TextBackingArray.Count;
		int num = -1;
		for (int i = 0; i < count; i++)
		{
			int num2 = i - 1;
			int num3 = i + 1;
			if (m_TextBackingArray[i] == 13)
			{
				bool flag = true;
				if (flag && num2 >= 0 && num2 > num && m_TextBackingArray[num2] == 10)
				{
					flag = false;
				}
				if (flag && num3 < count && m_TextBackingArray[num3] == 10)
				{
					flag = false;
				}
				if (flag)
				{
					m_TextBackingArray[i] = 10u;
					num = i;
				}
			}
		}
	}

	private void PopulateTextBackingArray(StringBuilder sourceText, int start, int length)
	{
		int num = 0;
		int i;
		if (sourceText == null)
		{
			i = 0;
			length = 0;
		}
		else
		{
			i = Mathf.Clamp(start, 0, sourceText.Length);
			length = Mathf.Clamp(length, 0, (start + length < sourceText.Length) ? length : (sourceText.Length - start));
		}
		if (length >= m_TextBackingArray.Capacity)
		{
			m_TextBackingArray.Resize(length);
		}
		for (int num2 = i + length; i < num2; i++)
		{
			m_TextBackingArray[num] = sourceText[i];
			num++;
		}
		m_TextBackingArray[num] = 0u;
		m_TextBackingArray.Count = num;
	}

	private void PopulateTextBackingArray(char[] sourceText, int start, int length)
	{
		int num = 0;
		int i;
		if (sourceText == null)
		{
			i = 0;
			length = 0;
		}
		else
		{
			i = Mathf.Clamp(start, 0, sourceText.Length);
			length = Mathf.Clamp(length, 0, (start + length < sourceText.Length) ? length : (sourceText.Length - start));
		}
		if (length >= m_TextBackingArray.Capacity)
		{
			m_TextBackingArray.Resize(length);
		}
		for (int num2 = i + length; i < num2; i++)
		{
			m_TextBackingArray[num] = sourceText[i];
			num++;
		}
		m_TextBackingArray[num] = 0u;
		m_TextBackingArray.Count = num;
	}

	private int GetEmojiSpriteIndexFromHashCode(int hashCode, out TMP_SpriteAsset curSpriteAsset)
	{
		curSpriteAsset = null;
		int spriteIndex = -1;
		if (m_spriteAsset != null)
		{
			curSpriteAsset = m_spriteAsset;
		}
		else if (m_defaultSpriteAsset != null)
		{
			curSpriteAsset = m_defaultSpriteAsset;
		}
		else if (m_defaultSpriteAsset == null)
		{
			if (TMP_Settings.defaultSpriteAsset != null)
			{
				m_defaultSpriteAsset = TMP_Settings.defaultSpriteAsset;
			}
			else
			{
				m_defaultSpriteAsset = Resources.Load<TMP_SpriteAsset>("Sprite Assets/Default Sprite Asset");
			}
			curSpriteAsset = m_defaultSpriteAsset;
		}
		if (curSpriteAsset == null)
		{
			return -1;
		}
		curSpriteAsset = TMP_SpriteAsset.SearchForSpriteByHashCode(curSpriteAsset, hashCode, includeFallbacks: true, out spriteIndex);
		return spriteIndex;
	}

	private static void AppendUtf32Hex(StringBuilder str, byte[] bytes, int start)
	{
		if (bytes.Length - start >= 4)
		{
			if (str.Length > 0)
			{
				str.Append('-');
			}
			int num = 0;
			for (int num2 = 3; num2 >= 0; num2--)
			{
				num |= bytes[start + num2] << num2 * 8;
			}
			str.Append(num.ToString("x4"));
		}
	}

	private EmojiSpriteInfo GetEmojiSpriteInfo(string emoji, out int hashcode)
	{
		EmojiSpriteInfo emojiSpriteInfo = new EmojiSpriteInfo();
		hashcode = 0;
		TMP_SpriteAsset curSpriteAsset = null;
		EmojiNameStr.Clear();
		int num = -1;
		int byteCount = Encoding.UTF32.GetByteCount(emoji);
		if (byteCount > EmojiUtf32Bytes.Length)
		{
			EmojiUtf32Bytes = new byte[byteCount + 16];
		}
		byteCount = Encoding.UTF32.GetBytes(emoji, 0, emoji.Length, EmojiUtf32Bytes, 0);
		switch (byteCount)
		{
		case 4:
			AppendUtf32Hex(EmojiNameStr, EmojiUtf32Bytes, 0);
			hashcode = TextUtilities.GetHashCodeCaseSensitive(EmojiNameStr.ToString());
			num = GetEmojiSpriteIndexFromHashCode(hashcode, out curSpriteAsset);
			if (num < 0)
			{
				EmojiNameStr.Append('-');
				EmojiNameStr.Append(EmojiVariationSelection);
				hashcode = TextUtilities.GetHashCodeCaseSensitive(EmojiNameStr.ToString());
				num = GetEmojiSpriteIndexFromHashCode(hashcode, out curSpriteAsset);
			}
			break;
		case 8:
			EmojiNameStrHigh.Clear();
			EmojiNameStrLow.Clear();
			AppendUtf32Hex(EmojiNameStrHigh, EmojiUtf32Bytes, 0);
			AppendUtf32Hex(EmojiNameStrLow, EmojiUtf32Bytes, 4);
			EmojiNameStr.Append((object)EmojiNameStrHigh);
			EmojiNameStr.Append('-');
			EmojiNameStr.Append((object)EmojiNameStrLow);
			hashcode = TextUtilities.GetHashCodeCaseSensitive(EmojiNameStr.ToString());
			num = GetEmojiSpriteIndexFromHashCode(hashcode, out curSpriteAsset);
			if (num < 0)
			{
				if (EmojiNameStrLow.ToString().Equals(EmojiVariationSelection))
				{
					hashcode = TextUtilities.GetHashCodeCaseSensitive(EmojiNameStrHigh.ToString());
					num = GetEmojiSpriteIndexFromHashCode(hashcode, out curSpriteAsset);
				}
				else if (EmojiNameStrLow.ToString().Equals(CombiningEnclosingKeycap))
				{
					EmojiNameStr.Clear();
					EmojiNameStr.Append((object)EmojiNameStrHigh);
					EmojiNameStr.Append('-');
					EmojiNameStr.Append(EmojiVariationSelection);
					EmojiNameStr.Append('-');
					EmojiNameStr.Append((object)EmojiNameStrLow);
					hashcode = TextUtilities.GetHashCodeCaseSensitive(EmojiNameStr.ToString());
					num = GetEmojiSpriteIndexFromHashCode(hashcode, out curSpriteAsset);
				}
			}
			break;
		default:
			if (byteCount % 4 == 0)
			{
				int num2 = byteCount / 4;
				for (int i = 0; i < num2; i++)
				{
					AppendUtf32Hex(EmojiNameStr, EmojiUtf32Bytes, 4 * i);
				}
				hashcode = TextUtilities.GetHashCodeCaseSensitive(EmojiNameStr.ToString());
				num = GetEmojiSpriteIndexFromHashCode(hashcode, out curSpriteAsset);
				if (num < 0 && !EmojiNameStr.ToString().EndsWith(EmojiVariationSelection))
				{
					EmojiNameStr.Append('-');
					EmojiNameStr.Append(EmojiVariationSelection);
					hashcode = TextUtilities.GetHashCodeCaseSensitive(EmojiNameStr.ToString());
					num = GetEmojiSpriteIndexFromHashCode(hashcode, out curSpriteAsset);
				}
			}
			break;
		}
		if (num < 0)
		{
			hashcode = 0;
			return null;
		}
		emojiSpriteInfo.spriteAsset = curSpriteAsset;
		emojiSpriteInfo.index = num;
		return emojiSpriteInfo;
	}

	private void PopulateTextProcessingArray()
	{
		Match match = null;
		int num = -1;
		MatchCollection matchCollection = null;
		if (supportSpriteEmoji)
		{
			matchCollection = Regex1.Matches(m_text);
			if (matchCollection.Count > 0)
			{
				match = matchCollection[0];
				num = 0;
			}
		}
		int count = m_TextBackingArray.Count;
		if (m_TextProcessingArray.Length < count)
		{
			ResizeInternalArray(ref m_TextProcessingArray, count);
		}
		TMP_TextProcessingStack<int>.SetDefault(m_TextStyleStacks, 0);
		m_TextStyleStackDepth = 0;
		int writeIndex = 0;
		if (textStyle.hashCode != -1183493901)
		{
			InsertOpeningStyleTag(m_TextStyle, 0, ref m_TextProcessingArray, ref writeIndex);
		}
		int num2 = 0;
		for (int i = 0; i < count; i++)
		{
			uint num3 = m_TextBackingArray[i];
			if (num3 == 0)
			{
				break;
			}
			if (match != null && i == match.Index)
			{
				if (writeIndex == m_TextProcessingArray.Length)
				{
					ResizeInternalArray(ref m_TextProcessingArray);
				}
				EmojiSpriteInfo value = null;
				int hashcode = 0;
				string value2 = match.Value;
				if (!emojiFastSearch.TryGetValue(value2, out value))
				{
					value = GetEmojiSpriteInfo(value2, out hashcode);
					emojiFastSearch.Add(value2, value);
				}
				m_TextProcessingArray[writeIndex].emojiSpriteInfo = value;
				m_TextProcessingArray[writeIndex].unicode = 63;
				m_TextProcessingArray[writeIndex].stringIndex = i + num2;
				m_TextProcessingArray[writeIndex].length = match.Length;
				i += match.Length - 1;
				writeIndex++;
				num++;
				if (num < matchCollection?.Count)
				{
					match = matchCollection?[num];
					continue;
				}
				match = null;
				num = -1;
				continue;
			}
			if (m_inputSource == TextInputSources.TextInputBox && num3 == 92 && i < count - 1)
			{
				switch (m_TextBackingArray[i + 1])
				{
				case 92u:
					if (m_parseCtrlCharacters && count > i + 2)
					{
						if (writeIndex + 2 > m_TextProcessingArray.Length)
						{
							ResizeInternalArray(ref m_TextProcessingArray);
						}
						m_TextProcessingArray[writeIndex].unicode = (int)m_TextBackingArray[i + 1];
						m_TextProcessingArray[writeIndex].stringIndex = i + num2;
						m_TextProcessingArray[writeIndex].length = 1;
						m_TextProcessingArray[writeIndex].emojiSpriteInfo = null;
						m_TextProcessingArray[writeIndex + 1].unicode = (int)m_TextBackingArray[i + 2];
						m_TextProcessingArray[writeIndex + 1].stringIndex = i + num2;
						m_TextProcessingArray[writeIndex + 1].length = 1;
						i += 2;
						writeIndex += 2;
						continue;
					}
					break;
				case 110u:
					if (m_parseCtrlCharacters)
					{
						if (writeIndex == m_TextProcessingArray.Length)
						{
							ResizeInternalArray(ref m_TextProcessingArray);
						}
						m_TextProcessingArray[writeIndex].unicode = 10;
						m_TextProcessingArray[writeIndex].stringIndex = i + num2;
						m_TextProcessingArray[writeIndex].length = 1;
						m_TextProcessingArray[writeIndex].emojiSpriteInfo = null;
						i++;
						writeIndex++;
						continue;
					}
					break;
				case 114u:
					if (m_parseCtrlCharacters)
					{
						if (writeIndex == m_TextProcessingArray.Length)
						{
							ResizeInternalArray(ref m_TextProcessingArray);
						}
						m_TextProcessingArray[writeIndex].unicode = 13;
						m_TextProcessingArray[writeIndex].stringIndex = i + num2;
						m_TextProcessingArray[writeIndex].length = 1;
						m_TextProcessingArray[writeIndex].emojiSpriteInfo = null;
						i++;
						writeIndex++;
						continue;
					}
					break;
				case 116u:
					if (m_parseCtrlCharacters)
					{
						if (writeIndex == m_TextProcessingArray.Length)
						{
							ResizeInternalArray(ref m_TextProcessingArray);
						}
						m_TextProcessingArray[writeIndex].unicode = 9;
						m_TextProcessingArray[writeIndex].stringIndex = i + num2;
						m_TextProcessingArray[writeIndex].length = 1;
						m_TextProcessingArray[writeIndex].emojiSpriteInfo = null;
						i++;
						writeIndex++;
						continue;
					}
					break;
				case 118u:
					if (m_parseCtrlCharacters)
					{
						if (writeIndex == m_TextProcessingArray.Length)
						{
							ResizeInternalArray(ref m_TextProcessingArray);
						}
						m_TextProcessingArray[writeIndex].unicode = 11;
						m_TextProcessingArray[writeIndex].stringIndex = i + num2;
						m_TextProcessingArray[writeIndex].length = 1;
						m_TextProcessingArray[writeIndex].emojiSpriteInfo = null;
						i++;
						writeIndex++;
						continue;
					}
					break;
				case 117u:
					if (count > i + 5)
					{
						if (writeIndex == m_TextProcessingArray.Length)
						{
							ResizeInternalArray(ref m_TextProcessingArray);
						}
						m_TextProcessingArray[writeIndex].unicode = GetUTF16(m_TextBackingArray, i + 2);
						m_TextProcessingArray[writeIndex].stringIndex = i + num2;
						m_TextProcessingArray[writeIndex].length = 6;
						m_TextProcessingArray[writeIndex].emojiSpriteInfo = null;
						i += 5;
						writeIndex++;
						continue;
					}
					break;
				case 85u:
					if (count > i + 9)
					{
						if (writeIndex == m_TextProcessingArray.Length)
						{
							ResizeInternalArray(ref m_TextProcessingArray);
						}
						m_TextProcessingArray[writeIndex].unicode = GetUTF32(m_TextBackingArray, i + 2);
						m_TextProcessingArray[writeIndex].stringIndex = i + num2;
						m_TextProcessingArray[writeIndex].length = 10;
						m_TextProcessingArray[writeIndex].emojiSpriteInfo = null;
						i += 9;
						writeIndex++;
						continue;
					}
					break;
				}
			}
			if (num3 >= 55296 && num3 <= 56319 && count > i + 1 && m_TextBackingArray[i + 1] >= 56320 && m_TextBackingArray[i + 1] <= 57343)
			{
				if (writeIndex == m_TextProcessingArray.Length)
				{
					ResizeInternalArray(ref m_TextProcessingArray);
				}
				m_TextProcessingArray[writeIndex].unicode = (int)TMP_TextParsingUtilities.ConvertToUTF32(num3, m_TextBackingArray[i + 1]);
				m_TextProcessingArray[writeIndex].stringIndex = i + num2;
				m_TextProcessingArray[writeIndex].length = 2;
				m_TextProcessingArray[writeIndex].emojiSpriteInfo = null;
				i++;
				writeIndex++;
				continue;
			}
			if (num3 == 60 && m_isRichText)
			{
				switch ((MarkupTag)GetMarkupTagHashCode(m_TextBackingArray, i + 1))
				{
				case MarkupTag.BR:
					if (writeIndex == m_TextProcessingArray.Length)
					{
						ResizeInternalArray(ref m_TextProcessingArray);
					}
					m_TextProcessingArray[writeIndex].unicode = 10;
					m_TextProcessingArray[writeIndex].stringIndex = i + num2;
					m_TextProcessingArray[writeIndex].length = 4;
					m_TextProcessingArray[writeIndex].emojiSpriteInfo = null;
					writeIndex++;
					i += 3;
					continue;
				case MarkupTag.NBSP:
					if (writeIndex == m_TextProcessingArray.Length)
					{
						ResizeInternalArray(ref m_TextProcessingArray);
					}
					m_TextProcessingArray[writeIndex].unicode = 160;
					m_TextProcessingArray[writeIndex].stringIndex = i + num2;
					m_TextProcessingArray[writeIndex].length = 6;
					m_TextProcessingArray[writeIndex].emojiSpriteInfo = null;
					writeIndex++;
					i += 5;
					continue;
				case MarkupTag.ZWSP:
					if (writeIndex == m_TextProcessingArray.Length)
					{
						ResizeInternalArray(ref m_TextProcessingArray);
					}
					m_TextProcessingArray[writeIndex].unicode = 8203;
					m_TextProcessingArray[writeIndex].stringIndex = i + num2;
					m_TextProcessingArray[writeIndex].length = 6;
					m_TextProcessingArray[writeIndex].emojiSpriteInfo = null;
					writeIndex++;
					i += 5;
					continue;
				case MarkupTag.STYLE:
				{
					int k = writeIndex;
					if (ReplaceOpeningStyleTag(ref m_TextBackingArray, i, out var srcOffset, ref m_TextProcessingArray, ref writeIndex))
					{
						for (; k < writeIndex; k++)
						{
							m_TextProcessingArray[k].stringIndex = i + num2;
							m_TextProcessingArray[k].length = srcOffset - i + 1;
							m_TextProcessingArray[k].emojiSpriteInfo = null;
						}
						i = srcOffset;
						continue;
					}
					break;
				}
				case MarkupTag.SLASH_STYLE:
				{
					int j = writeIndex;
					ReplaceClosingStyleTag(ref m_TextBackingArray, i, ref m_TextProcessingArray, ref writeIndex);
					for (; j < writeIndex; j++)
					{
						m_TextProcessingArray[j].stringIndex = i + num2;
						m_TextProcessingArray[j].length = 8;
						m_TextProcessingArray[j].emojiSpriteInfo = null;
					}
					i += 7;
					continue;
				}
				}
			}
			int num4 = 1;
			if (hasArabicChar && i < m_TMPFixCharLenArray.Count)
			{
				num4 = (int)m_TMPFixCharLenArray[i];
			}
			if (writeIndex + num4 > m_TextProcessingArray.Length)
			{
				ResizeInternalArray(ref m_TextProcessingArray);
			}
			m_TextProcessingArray[writeIndex].unicode = (int)num3;
			m_TextProcessingArray[writeIndex].stringIndex = i + num2;
			m_TextProcessingArray[writeIndex].length = num4;
			m_TextProcessingArray[writeIndex].emojiSpriteInfo = null;
			if (hasArabicChar && num4 > 1)
			{
				for (int l = 0; l < num4 - 1; l++)
				{
					m_TextProcessingArray[writeIndex + 1 + l].unicode = 65279;
					m_TextProcessingArray[writeIndex + 1 + l].stringIndex = i + num2 + l + 1;
					m_TextProcessingArray[writeIndex + 1 + l].length = 1;
					m_TextProcessingArray[writeIndex + 1 + l].emojiSpriteInfo = null;
				}
				num2 += num4 - 1;
			}
			writeIndex += num4;
		}
		m_TextStyleStackDepth = 0;
		if (textStyle.hashCode != -1183493901)
		{
			InsertClosingStyleTag(ref m_TextProcessingArray, ref writeIndex);
		}
		if (writeIndex == m_TextProcessingArray.Length)
		{
			ResizeInternalArray(ref m_TextProcessingArray);
		}
		m_TextProcessingArray[writeIndex].unicode = 0;
		m_TextProcessingArray[writeIndex].emojiSpriteInfo = null;
		m_InternalTextProcessingArraySize = writeIndex;
	}

	private void SetTextInternal(string sourceText)
	{
		int length = sourceText?.Length ?? 0;
		PopulateTextBackingArray(sourceText, 0, length);
		TextInputSources inputSource = m_inputSource;
		m_inputSource = TextInputSources.TextString;
		PopulateTextProcessingArray();
		m_inputSource = inputSource;
	}

	public void SetText(string sourceText, bool syncTextInputBox = true)
	{
		int length = sourceText?.Length ?? 0;
		PopulateTextBackingArray(sourceText, 0, length);
		m_text = sourceText;
		m_inputSource = TextInputSources.TextString;
		PopulateTextProcessingArray();
		m_havePropertiesChanged = true;
		SetVerticesDirty();
		SetLayoutDirty();
	}

	public void SetText(string sourceText, float arg0)
	{
		SetText(sourceText, arg0, 0f, 0f, 0f, 0f, 0f, 0f, 0f);
	}

	public void SetText(string sourceText, float arg0, float arg1)
	{
		SetText(sourceText, arg0, arg1, 0f, 0f, 0f, 0f, 0f, 0f);
	}

	public void SetText(string sourceText, float arg0, float arg1, float arg2)
	{
		SetText(sourceText, arg0, arg1, arg2, 0f, 0f, 0f, 0f, 0f);
	}

	public void SetText(string sourceText, float arg0, float arg1, float arg2, float arg3)
	{
		SetText(sourceText, arg0, arg1, arg2, arg3, 0f, 0f, 0f, 0f);
	}

	public void SetText(string sourceText, float arg0, float arg1, float arg2, float arg3, float arg4)
	{
		SetText(sourceText, arg0, arg1, arg2, arg3, arg4, 0f, 0f, 0f);
	}

	public void SetText(string sourceText, float arg0, float arg1, float arg2, float arg3, float arg4, float arg5)
	{
		SetText(sourceText, arg0, arg1, arg2, arg3, arg4, arg5, 0f, 0f);
	}

	public void SetText(string sourceText, float arg0, float arg1, float arg2, float arg3, float arg4, float arg5, float arg6)
	{
		SetText(sourceText, arg0, arg1, arg2, arg3, arg4, arg5, arg6, 0f);
	}

	public void SetText(string sourceText, float arg0, float arg1, float arg2, float arg3, float arg4, float arg5, float arg6, float arg7)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int i = 0;
		int writeIndex = 0;
		for (; i < sourceText.Length; i++)
		{
			char c = sourceText[i];
			switch (c)
			{
			case '{':
				num4 = 1;
				continue;
			case '}':
				switch (num)
				{
				case 0:
					AddFloatToInternalTextBackingArray(arg0, num2, num3, ref writeIndex);
					break;
				case 1:
					AddFloatToInternalTextBackingArray(arg1, num2, num3, ref writeIndex);
					break;
				case 2:
					AddFloatToInternalTextBackingArray(arg2, num2, num3, ref writeIndex);
					break;
				case 3:
					AddFloatToInternalTextBackingArray(arg3, num2, num3, ref writeIndex);
					break;
				case 4:
					AddFloatToInternalTextBackingArray(arg4, num2, num3, ref writeIndex);
					break;
				case 5:
					AddFloatToInternalTextBackingArray(arg5, num2, num3, ref writeIndex);
					break;
				case 6:
					AddFloatToInternalTextBackingArray(arg6, num2, num3, ref writeIndex);
					break;
				case 7:
					AddFloatToInternalTextBackingArray(arg7, num2, num3, ref writeIndex);
					break;
				}
				num = 0;
				num4 = 0;
				num2 = 0;
				num3 = 0;
				continue;
			}
			if (num4 == 1 && c >= '0' && c <= '8')
			{
				num = c - 48;
				num4 = 2;
				continue;
			}
			if (num4 == 2)
			{
				switch (c)
				{
				case '.':
					num4 = 3;
					continue;
				case '0':
					num2++;
					continue;
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					num3 = c - 48;
					continue;
				case '#':
				case ',':
				case ':':
					continue;
				}
			}
			if (num4 == 3 && c == '0')
			{
				num3++;
				continue;
			}
			m_TextBackingArray[writeIndex] = c;
			writeIndex++;
		}
		m_TextBackingArray[writeIndex] = 0u;
		m_TextBackingArray.Count = writeIndex;
		m_IsTextBackingStringDirty = true;
		m_inputSource = TextInputSources.SetText;
		PopulateTextProcessingArray();
		m_havePropertiesChanged = true;
		SetVerticesDirty();
		SetLayoutDirty();
	}

	public void SetText(StringBuilder sourceText)
	{
		int length = sourceText?.Length ?? 0;
		SetText(sourceText, 0, length);
	}

	private void SetText(StringBuilder sourceText, int start, int length)
	{
		PopulateTextBackingArray(sourceText, start, length);
		m_IsTextBackingStringDirty = true;
		m_inputSource = TextInputSources.SetTextArray;
		PopulateTextProcessingArray();
		m_havePropertiesChanged = true;
		SetVerticesDirty();
		SetLayoutDirty();
	}

	public void SetText(char[] sourceText)
	{
		int length = ((sourceText != null) ? sourceText.Length : 0);
		SetCharArray(sourceText, 0, length);
	}

	public void SetText(char[] sourceText, int start, int length)
	{
		SetCharArray(sourceText, start, length);
	}

	public void SetCharArray(char[] sourceText)
	{
		int length = ((sourceText != null) ? sourceText.Length : 0);
		SetCharArray(sourceText, 0, length);
	}

	public void SetCharArray(char[] sourceText, int start, int length)
	{
		PopulateTextBackingArray(sourceText, start, length);
		m_IsTextBackingStringDirty = true;
		m_inputSource = TextInputSources.SetTextArray;
		PopulateTextProcessingArray();
		m_havePropertiesChanged = true;
		SetVerticesDirty();
		SetLayoutDirty();
	}

	private TMP_Style GetStyle(int hashCode)
	{
		TMP_Style tMP_Style = null;
		if (m_StyleSheet != null)
		{
			tMP_Style = m_StyleSheet.GetStyle(hashCode);
			if (tMP_Style != null)
			{
				return tMP_Style;
			}
		}
		if (TMP_Settings.defaultStyleSheet != null)
		{
			tMP_Style = TMP_Settings.defaultStyleSheet.GetStyle(hashCode);
		}
		return tMP_Style;
	}

	private bool ReplaceOpeningStyleTag(ref TextBackingContainer sourceText, int srcIndex, out int srcOffset, ref UnicodeChar[] charBuffer, ref int writeIndex)
	{
		int styleHashCode = GetStyleHashCode(ref sourceText, srcIndex + 7, out srcOffset);
		TMP_Style style = GetStyle(styleHashCode);
		if (style == null || srcOffset == 0)
		{
			return false;
		}
		m_TextStyleStackDepth++;
		m_TextStyleStacks[m_TextStyleStackDepth].Push(style.hashCode);
		int num = style.styleOpeningTagArray.Length;
		int[] sourceText2 = style.styleOpeningTagArray;
		for (int i = 0; i < num; i++)
		{
			int num2 = sourceText2[i];
			if (num2 == 92 && i + 1 < num)
			{
				switch (sourceText2[i + 1])
				{
				case 92:
					i++;
					break;
				case 110:
					num2 = 10;
					i++;
					break;
				case 117:
					if (i + 5 < num)
					{
						num2 = GetUTF16(sourceText2, i + 2);
						i += 5;
					}
					break;
				case 85:
					if (i + 9 < num)
					{
						num2 = GetUTF32(sourceText2, i + 2);
						i += 9;
					}
					break;
				}
			}
			if (num2 == 60)
			{
				switch ((MarkupTag)GetMarkupTagHashCode(sourceText2, i + 1))
				{
				case MarkupTag.BR:
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex].unicode = 10;
					writeIndex++;
					i += 3;
					continue;
				case MarkupTag.NBSP:
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex].unicode = 160;
					writeIndex++;
					i += 5;
					continue;
				case MarkupTag.ZWSP:
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex].unicode = 8203;
					writeIndex++;
					i += 5;
					continue;
				case MarkupTag.STYLE:
				{
					if (ReplaceOpeningStyleTag(ref sourceText2, i, out var srcOffset2, ref charBuffer, ref writeIndex))
					{
						i = srcOffset2;
						continue;
					}
					break;
				}
				case MarkupTag.SLASH_STYLE:
					ReplaceClosingStyleTag(ref sourceText2, i, ref charBuffer, ref writeIndex);
					i += 7;
					continue;
				}
			}
			if (writeIndex == charBuffer.Length)
			{
				ResizeInternalArray(ref charBuffer);
			}
			charBuffer[writeIndex].unicode = num2;
			writeIndex++;
		}
		m_TextStyleStackDepth--;
		return true;
	}

	private bool ReplaceOpeningStyleTag(ref int[] sourceText, int srcIndex, out int srcOffset, ref UnicodeChar[] charBuffer, ref int writeIndex)
	{
		int styleHashCode = GetStyleHashCode(ref sourceText, srcIndex + 7, out srcOffset);
		TMP_Style style = GetStyle(styleHashCode);
		if (style == null || srcOffset == 0)
		{
			return false;
		}
		m_TextStyleStackDepth++;
		m_TextStyleStacks[m_TextStyleStackDepth].Push(style.hashCode);
		int num = style.styleOpeningTagArray.Length;
		int[] sourceText2 = style.styleOpeningTagArray;
		for (int i = 0; i < num; i++)
		{
			int num2 = sourceText2[i];
			if (num2 == 92 && i + 1 < num)
			{
				switch (sourceText2[i + 1])
				{
				case 92:
					i++;
					break;
				case 110:
					num2 = 10;
					i++;
					break;
				case 117:
					if (i + 5 < num)
					{
						num2 = GetUTF16(sourceText2, i + 2);
						i += 5;
					}
					break;
				case 85:
					if (i + 9 < num)
					{
						num2 = GetUTF32(sourceText2, i + 2);
						i += 9;
					}
					break;
				}
			}
			if (num2 == 60)
			{
				switch ((MarkupTag)GetMarkupTagHashCode(sourceText2, i + 1))
				{
				case MarkupTag.BR:
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex].unicode = 10;
					writeIndex++;
					i += 3;
					continue;
				case MarkupTag.NBSP:
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex].unicode = 160;
					writeIndex++;
					i += 5;
					continue;
				case MarkupTag.ZWSP:
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex].unicode = 8203;
					writeIndex++;
					i += 5;
					continue;
				case MarkupTag.STYLE:
				{
					if (ReplaceOpeningStyleTag(ref sourceText2, i, out var srcOffset2, ref charBuffer, ref writeIndex))
					{
						i = srcOffset2;
						continue;
					}
					break;
				}
				case MarkupTag.SLASH_STYLE:
					ReplaceClosingStyleTag(ref sourceText2, i, ref charBuffer, ref writeIndex);
					i += 7;
					continue;
				}
			}
			if (writeIndex == charBuffer.Length)
			{
				ResizeInternalArray(ref charBuffer);
			}
			charBuffer[writeIndex].unicode = num2;
			writeIndex++;
		}
		m_TextStyleStackDepth--;
		return true;
	}

	private void ReplaceClosingStyleTag(ref TextBackingContainer sourceText, int srcIndex, ref UnicodeChar[] charBuffer, ref int writeIndex)
	{
		int hashCode = m_TextStyleStacks[m_TextStyleStackDepth + 1].Pop();
		TMP_Style style = GetStyle(hashCode);
		if (style == null)
		{
			return;
		}
		m_TextStyleStackDepth++;
		int num = style.styleClosingTagArray.Length;
		int[] sourceText2 = style.styleClosingTagArray;
		for (int i = 0; i < num; i++)
		{
			int num2 = sourceText2[i];
			if (num2 == 92 && i + 1 < num)
			{
				switch (sourceText2[i + 1])
				{
				case 92:
					i++;
					break;
				case 110:
					num2 = 10;
					i++;
					break;
				case 117:
					if (i + 5 < num)
					{
						num2 = GetUTF16(sourceText2, i + 2);
						i += 5;
					}
					break;
				case 85:
					if (i + 9 < num)
					{
						num2 = GetUTF32(sourceText2, i + 2);
						i += 9;
					}
					break;
				}
			}
			if (num2 == 60)
			{
				switch ((MarkupTag)GetMarkupTagHashCode(sourceText2, i + 1))
				{
				case MarkupTag.BR:
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex].unicode = 10;
					writeIndex++;
					i += 3;
					continue;
				case MarkupTag.NBSP:
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex].unicode = 160;
					writeIndex++;
					i += 5;
					continue;
				case MarkupTag.ZWSP:
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex].unicode = 8203;
					writeIndex++;
					i += 5;
					continue;
				case MarkupTag.STYLE:
				{
					if (ReplaceOpeningStyleTag(ref sourceText2, i, out var srcOffset, ref charBuffer, ref writeIndex))
					{
						i = srcOffset;
						continue;
					}
					break;
				}
				case MarkupTag.SLASH_STYLE:
					ReplaceClosingStyleTag(ref sourceText2, i, ref charBuffer, ref writeIndex);
					i += 7;
					continue;
				}
			}
			if (writeIndex == charBuffer.Length)
			{
				ResizeInternalArray(ref charBuffer);
			}
			charBuffer[writeIndex].unicode = num2;
			writeIndex++;
		}
		m_TextStyleStackDepth--;
	}

	private void ReplaceClosingStyleTag(ref int[] sourceText, int srcIndex, ref UnicodeChar[] charBuffer, ref int writeIndex)
	{
		int hashCode = m_TextStyleStacks[m_TextStyleStackDepth + 1].Pop();
		TMP_Style style = GetStyle(hashCode);
		if (style == null)
		{
			return;
		}
		m_TextStyleStackDepth++;
		int num = style.styleClosingTagArray.Length;
		int[] sourceText2 = style.styleClosingTagArray;
		for (int i = 0; i < num; i++)
		{
			int num2 = sourceText2[i];
			if (num2 == 92 && i + 1 < num)
			{
				switch (sourceText2[i + 1])
				{
				case 92:
					i++;
					break;
				case 110:
					num2 = 10;
					i++;
					break;
				case 117:
					if (i + 5 < num)
					{
						num2 = GetUTF16(sourceText2, i + 2);
						i += 5;
					}
					break;
				case 85:
					if (i + 9 < num)
					{
						num2 = GetUTF32(sourceText2, i + 2);
						i += 9;
					}
					break;
				}
			}
			if (num2 == 60)
			{
				switch ((MarkupTag)GetMarkupTagHashCode(sourceText2, i + 1))
				{
				case MarkupTag.BR:
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex].unicode = 10;
					writeIndex++;
					i += 3;
					continue;
				case MarkupTag.NBSP:
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex].unicode = 160;
					writeIndex++;
					i += 5;
					continue;
				case MarkupTag.ZWSP:
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex].unicode = 8203;
					writeIndex++;
					i += 5;
					continue;
				case MarkupTag.STYLE:
				{
					if (ReplaceOpeningStyleTag(ref sourceText2, i, out var srcOffset, ref charBuffer, ref writeIndex))
					{
						i = srcOffset;
						continue;
					}
					break;
				}
				case MarkupTag.SLASH_STYLE:
					ReplaceClosingStyleTag(ref sourceText2, i, ref charBuffer, ref writeIndex);
					i += 7;
					continue;
				}
			}
			if (writeIndex == charBuffer.Length)
			{
				ResizeInternalArray(ref charBuffer);
			}
			charBuffer[writeIndex].unicode = num2;
			writeIndex++;
		}
		m_TextStyleStackDepth--;
	}

	private bool InsertOpeningStyleTag(TMP_Style style, int srcIndex, ref UnicodeChar[] charBuffer, ref int writeIndex)
	{
		if (style == null)
		{
			return false;
		}
		m_TextStyleStacks[0].Push(style.hashCode);
		int num = style.styleOpeningTagArray.Length;
		int[] sourceText = style.styleOpeningTagArray;
		for (int i = 0; i < num; i++)
		{
			int num2 = sourceText[i];
			if (num2 == 92 && i + 1 < num)
			{
				switch (sourceText[i + 1])
				{
				case 92:
					i++;
					break;
				case 110:
					num2 = 10;
					i++;
					break;
				case 117:
					if (i + 5 < num)
					{
						num2 = GetUTF16(sourceText, i + 2);
						i += 5;
					}
					break;
				case 85:
					if (i + 9 < num)
					{
						num2 = GetUTF32(sourceText, i + 2);
						i += 9;
					}
					break;
				}
			}
			if (num2 == 60)
			{
				switch ((MarkupTag)GetMarkupTagHashCode(sourceText, i + 1))
				{
				case MarkupTag.BR:
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex].unicode = 10;
					writeIndex++;
					i += 3;
					continue;
				case MarkupTag.NBSP:
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex].unicode = 160;
					writeIndex++;
					i += 5;
					continue;
				case MarkupTag.ZWSP:
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex].unicode = 8203;
					writeIndex++;
					i += 5;
					continue;
				case MarkupTag.STYLE:
				{
					if (ReplaceOpeningStyleTag(ref sourceText, i, out var srcOffset, ref charBuffer, ref writeIndex))
					{
						i = srcOffset;
						continue;
					}
					break;
				}
				case MarkupTag.SLASH_STYLE:
					ReplaceClosingStyleTag(ref sourceText, i, ref charBuffer, ref writeIndex);
					i += 7;
					continue;
				}
			}
			if (writeIndex == charBuffer.Length)
			{
				ResizeInternalArray(ref charBuffer);
			}
			charBuffer[writeIndex].unicode = num2;
			writeIndex++;
		}
		m_TextStyleStackDepth = 0;
		return true;
	}

	private void InsertClosingStyleTag(ref UnicodeChar[] charBuffer, ref int writeIndex)
	{
		int hashCode = m_TextStyleStacks[0].Pop();
		TMP_Style style = GetStyle(hashCode);
		int num = style.styleClosingTagArray.Length;
		int[] sourceText = style.styleClosingTagArray;
		for (int i = 0; i < num; i++)
		{
			int num2 = sourceText[i];
			if (num2 == 92 && i + 1 < num)
			{
				switch (sourceText[i + 1])
				{
				case 92:
					i++;
					break;
				case 110:
					num2 = 10;
					i++;
					break;
				case 117:
					if (i + 5 < num)
					{
						num2 = GetUTF16(sourceText, i + 2);
						i += 5;
					}
					break;
				case 85:
					if (i + 9 < num)
					{
						num2 = GetUTF32(sourceText, i + 2);
						i += 9;
					}
					break;
				}
			}
			if (num2 == 60)
			{
				switch ((MarkupTag)GetMarkupTagHashCode(sourceText, i + 1))
				{
				case MarkupTag.BR:
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex].unicode = 10;
					writeIndex++;
					i += 3;
					continue;
				case MarkupTag.NBSP:
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex].unicode = 160;
					writeIndex++;
					i += 5;
					continue;
				case MarkupTag.ZWSP:
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex].unicode = 8203;
					writeIndex++;
					i += 5;
					continue;
				case MarkupTag.STYLE:
				{
					if (ReplaceOpeningStyleTag(ref sourceText, i, out var srcOffset, ref charBuffer, ref writeIndex))
					{
						i = srcOffset;
						continue;
					}
					break;
				}
				case MarkupTag.SLASH_STYLE:
					ReplaceClosingStyleTag(ref sourceText, i, ref charBuffer, ref writeIndex);
					i += 7;
					continue;
				}
			}
			if (writeIndex == charBuffer.Length)
			{
				ResizeInternalArray(ref charBuffer);
			}
			charBuffer[writeIndex].unicode = num2;
			writeIndex++;
		}
		m_TextStyleStackDepth = 0;
	}

	private int GetMarkupTagHashCode(int[] tagDefinition, int readIndex)
	{
		int num = 0;
		int num2 = readIndex + 16;
		int num3 = tagDefinition.Length;
		while (readIndex < num2 && readIndex < num3)
		{
			int num4 = tagDefinition[readIndex];
			if (num4 == 62 || num4 == 61 || num4 == 32)
			{
				return num;
			}
			num = ((num << 5) + num) ^ (int)TMP_TextUtilities.ToUpperASCIIFast((uint)num4);
			readIndex++;
		}
		return num;
	}

	private int GetMarkupTagHashCode(TextBackingContainer tagDefinition, int readIndex)
	{
		int num = 0;
		int num2 = readIndex + 16;
		int capacity = tagDefinition.Capacity;
		while (readIndex < num2 && readIndex < capacity)
		{
			uint num3 = tagDefinition[readIndex];
			if (num3 == 62 || num3 == 61 || num3 == 32)
			{
				return num;
			}
			num = ((num << 5) + num) ^ (int)TMP_TextUtilities.ToUpperASCIIFast(num3);
			readIndex++;
		}
		return num;
	}

	private int GetStyleHashCode(ref int[] text, int index, out int closeIndex)
	{
		int num = 0;
		closeIndex = 0;
		for (int i = index; i < text.Length; i++)
		{
			if (text[i] != 34)
			{
				if (text[i] == 62)
				{
					closeIndex = i;
					break;
				}
				num = ((num << 5) + num) ^ TMP_TextParsingUtilities.ToUpperASCIIFast((char)text[i]);
			}
		}
		return num;
	}

	private int GetStyleHashCode(ref TextBackingContainer text, int index, out int closeIndex)
	{
		int num = 0;
		closeIndex = 0;
		for (int i = index; i < text.Capacity; i++)
		{
			if (text[i] != 34)
			{
				if (text[i] == 62)
				{
					closeIndex = i;
					break;
				}
				num = ((num << 5) + num) ^ TMP_TextParsingUtilities.ToUpperASCIIFast((char)text[i]);
			}
		}
		return num;
	}

	private void ResizeInternalArray<T>(ref T[] array)
	{
		int newSize = Mathf.NextPowerOfTwo(array.Length + 1);
		Array.Resize(ref array, newSize);
	}

	private void ResizeInternalArray<T>(ref T[] array, int size)
	{
		size = Mathf.NextPowerOfTwo(size + 1);
		Array.Resize(ref array, size);
	}

	private void AddFloatToInternalTextBackingArray(float value, int padding, int precision, ref int writeIndex)
	{
		if (value < 0f)
		{
			m_TextBackingArray[writeIndex] = 45u;
			writeIndex++;
			value = 0f - value;
		}
		decimal num = (decimal)value;
		if (padding == 0 && precision == 0)
		{
			precision = 9;
		}
		else
		{
			num += k_Power[Mathf.Min(9, precision)];
		}
		long num2 = (long)num;
		AddIntegerToInternalTextBackingArray(num2, padding, ref writeIndex);
		if (precision <= 0)
		{
			return;
		}
		num -= (decimal)num2;
		if (!(num != 0m))
		{
			return;
		}
		m_TextBackingArray[writeIndex++] = 46u;
		for (int i = 0; i < precision; i++)
		{
			num *= 10m;
			long num3 = (long)num;
			m_TextBackingArray[writeIndex++] = (ushort)(num3 + 48);
			num -= (decimal)num3;
			if (num == 0m)
			{
				i = precision;
			}
		}
	}

	private void AddIntegerToInternalTextBackingArray(double number, int padding, ref int writeIndex)
	{
		int num = 0;
		int num2 = writeIndex;
		do
		{
			m_TextBackingArray[num2++] = (ushort)(number % 10.0 + 48.0);
			number /= 10.0;
			num++;
		}
		while (number > 0.999999999999999 || num < padding);
		int num3 = num2;
		while (writeIndex + 1 < num2)
		{
			num2--;
			uint value = m_TextBackingArray[writeIndex];
			m_TextBackingArray[writeIndex] = m_TextBackingArray[num2];
			m_TextBackingArray[num2] = value;
			writeIndex++;
		}
		writeIndex = num3;
	}

	private string InternalTextBackingArrayToString()
	{
		char[] array = new char[m_TextBackingArray.Count];
		for (int i = 0; i < m_TextBackingArray.Capacity; i++)
		{
			char c = (char)m_TextBackingArray[i];
			if (c == '\0')
			{
				break;
			}
			array[i] = c;
		}
		m_IsTextBackingStringDirty = false;
		return new string(array);
	}

	internal virtual int SetArraySizes(UnicodeChar[] unicodeChars)
	{
		return 0;
	}

	public Vector2 GetPreferredValues()
	{
		m_isPreferredWidthDirty = true;
		float x = GetPreferredWidth();
		m_isPreferredHeightDirty = true;
		float y = GetPreferredHeight();
		m_isPreferredWidthDirty = true;
		m_isPreferredHeightDirty = true;
		return new Vector2(x, y);
	}

	public Vector2 GetPreferredValues(float width, float height)
	{
		m_isCalculatingPreferredValues = true;
		ParseInputText();
		Vector2 vector = new Vector2(width, height);
		float x = GetPreferredWidth(vector);
		float y = GetPreferredHeight(vector);
		return new Vector2(x, y);
	}

	public Vector2 GetPreferredValues(string text)
	{
		m_isCalculatingPreferredValues = true;
		SetTextInternal(text);
		SetArraySizes(m_TextProcessingArray);
		Vector2 vector = k_LargePositiveVector2;
		float x = GetPreferredWidth(vector);
		float y = GetPreferredHeight(vector);
		return new Vector2(x, y);
	}

	public Vector2 GetPreferredValues(string text, float width, float height)
	{
		m_isCalculatingPreferredValues = true;
		SetTextInternal(text);
		SetArraySizes(m_TextProcessingArray);
		Vector2 vector = new Vector2(width, height);
		float x = GetPreferredWidth(vector);
		float y = GetPreferredHeight(vector);
		return new Vector2(x, y);
	}

	protected float GetPreferredWidth()
	{
		if (TMP_Settings.instance == null)
		{
			return 0f;
		}
		if (!m_isPreferredWidthDirty)
		{
			return m_preferredWidth;
		}
		float num = (m_enableAutoSizing ? m_fontSizeMax : m_fontSize);
		m_minFontSize = m_fontSizeMin;
		m_maxFontSize = m_fontSizeMax;
		m_charWidthAdjDelta = 0f;
		Vector2 marginSize = k_LargePositiveVector2;
		m_isCalculatingPreferredValues = true;
		ParseInputText();
		m_AutoSizeIterationCount = 0;
		float x = CalculatePreferredValues(ref num, marginSize, isTextAutoSizingEnabled: false, isWordWrappingEnabled: false).x;
		m_isPreferredWidthDirty = false;
		return x;
	}

	private float GetPreferredWidth(Vector2 margin)
	{
		float num = (m_enableAutoSizing ? m_fontSizeMax : m_fontSize);
		m_minFontSize = m_fontSizeMin;
		m_maxFontSize = m_fontSizeMax;
		m_charWidthAdjDelta = 0f;
		m_AutoSizeIterationCount = 0;
		return CalculatePreferredValues(ref num, margin, isTextAutoSizingEnabled: false, isWordWrappingEnabled: false).x;
	}

	protected float GetPreferredHeight()
	{
		if (TMP_Settings.instance == null)
		{
			return 0f;
		}
		if (!m_isPreferredHeightDirty)
		{
			return m_preferredHeight;
		}
		float num = (m_enableAutoSizing ? m_fontSizeMax : m_fontSize);
		m_minFontSize = m_fontSizeMin;
		m_maxFontSize = m_fontSizeMax;
		m_charWidthAdjDelta = 0f;
		Vector2 marginSize = new Vector2((m_marginWidth != 0f) ? m_marginWidth : k_LargePositiveFloat, k_LargePositiveFloat);
		m_isCalculatingPreferredValues = true;
		ParseInputText();
		m_IsAutoSizePointSizeSet = false;
		m_AutoSizeIterationCount = 0;
		float result = 0f;
		while (!m_IsAutoSizePointSizeSet)
		{
			result = CalculatePreferredValues(ref num, marginSize, m_enableAutoSizing, m_enableWordWrapping).y;
			m_AutoSizeIterationCount++;
		}
		m_isPreferredHeightDirty = false;
		return result;
	}

	private float GetPreferredHeight(Vector2 margin)
	{
		float num = (m_enableAutoSizing ? m_fontSizeMax : m_fontSize);
		m_minFontSize = m_fontSizeMin;
		m_maxFontSize = m_fontSizeMax;
		m_charWidthAdjDelta = 0f;
		m_IsAutoSizePointSizeSet = false;
		m_AutoSizeIterationCount = 0;
		float result = 0f;
		while (!m_IsAutoSizePointSizeSet)
		{
			result = CalculatePreferredValues(ref num, margin, m_enableAutoSizing, m_enableWordWrapping).y;
			m_AutoSizeIterationCount++;
		}
		return result;
	}

	public Vector2 GetRenderedValues()
	{
		return GetTextBounds().size;
	}

	public Vector2 GetRenderedValues(bool onlyVisibleCharacters)
	{
		return GetTextBounds(onlyVisibleCharacters).size;
	}

	private float GetRenderedWidth()
	{
		return GetRenderedValues().x;
	}

	protected float GetRenderedWidth(bool onlyVisibleCharacters)
	{
		return GetRenderedValues(onlyVisibleCharacters).x;
	}

	private float GetRenderedHeight()
	{
		return GetRenderedValues().y;
	}

	protected float GetRenderedHeight(bool onlyVisibleCharacters)
	{
		return GetRenderedValues(onlyVisibleCharacters).y;
	}

	protected virtual Vector2 CalculatePreferredValues(ref float fontSize, Vector2 marginSize, bool isTextAutoSizingEnabled, bool isWordWrappingEnabled)
	{
		if (m_fontAsset == null || m_fontAsset.characterLookupTable == null)
		{
			UnityEngine.Debug.LogWarning("Can't Generate Mesh! No Font Asset has been assigned to Object ID: " + GetInstanceID());
			m_IsAutoSizePointSizeSet = true;
			return Vector2.zero;
		}
		if (m_TextProcessingArray == null || m_TextProcessingArray.Length == 0 || m_TextProcessingArray[0].unicode == 0)
		{
			m_IsAutoSizePointSizeSet = true;
			return Vector2.zero;
		}
		m_currentFontAsset = m_fontAsset;
		m_currentMaterial = m_sharedMaterial;
		m_currentMaterialIndex = 0;
		m_materialReferenceStack.SetDefault(new MaterialReference(0, m_currentFontAsset, null, m_currentMaterial, m_padding));
		int totalCharacterCount = m_totalCharacterCount;
		if (m_internalCharacterInfo == null || totalCharacterCount > m_internalCharacterInfo.Length)
		{
			m_internalCharacterInfo = new TMP_CharacterInfo[(totalCharacterCount > 1024) ? (totalCharacterCount + 256) : Mathf.NextPowerOfTwo(totalCharacterCount)];
		}
		float num = fontSize / (float)m_fontAsset.faceInfo.pointSize * m_fontAsset.faceInfo.scale * (m_isOrthographic ? 1f : 0.1f);
		float num2 = num;
		float num3 = fontSize * 0.01f * (m_isOrthographic ? 1f : 0.1f);
		m_fontScaleMultiplier = 1f;
		m_currentFontSize = fontSize;
		m_sizeStack.SetDefault(m_currentFontSize);
		float num4 = 0f;
		m_FontStyleInternal = m_fontStyle;
		m_lineJustification = m_HorizontalAlignment;
		m_lineJustificationStack.SetDefault(m_lineJustification);
		m_baselineOffset = 0f;
		m_baselineOffsetStack.Clear();
		m_lineOffset = 0f;
		m_lineHeight = -32767f;
		float num5 = m_currentFontAsset.faceInfo.lineHeight - (m_currentFontAsset.faceInfo.ascentLine - m_currentFontAsset.faceInfo.descentLine);
		m_cSpacing = 0f;
		m_monoSpacing = 0f;
		m_xAdvance = 0f;
		float a = 0f;
		tag_LineIndent = 0f;
		tag_Indent = 0f;
		m_indentStack.SetDefault(0f);
		tag_NoParsing = false;
		m_characterCount = 0;
		m_firstCharacterOfLine = 0;
		m_maxLineAscender = k_LargeNegativeFloat;
		m_maxLineDescender = k_LargePositiveFloat;
		m_lineNumber = 0;
		m_startOfLineAscender = 0f;
		m_IsDrivenLineSpacing = false;
		float x = marginSize.x;
		m_marginLeft = 0f;
		m_marginRight = 0f;
		float num6 = 0f;
		float num7 = 0f;
		m_width = -1f;
		float num8 = x + 0.0001f - m_marginLeft - m_marginRight;
		float num9 = 0f;
		float num10 = 0f;
		float num11 = 0f;
		m_isCalculatingPreferredValues = true;
		m_maxCapHeight = 0f;
		m_maxTextAscender = 0f;
		m_ElementDescender = 0f;
		bool flag = false;
		bool flag2 = true;
		m_isNonBreakingSpace = false;
		bool flag3 = false;
		CharacterSubstitution characterSubstitution = new CharacterSubstitution(-1, 0u);
		bool flag4 = false;
		WordWrapState state = default(WordWrapState);
		WordWrapState state2 = default(WordWrapState);
		WordWrapState state3 = default(WordWrapState);
		m_AutoSizeIterationCount++;
		for (int i = 0; i < m_TextProcessingArray.Length && m_TextProcessingArray[i].unicode != 0; i++)
		{
			int num12 = m_TextProcessingArray[i].unicode;
			if (m_isRichText && num12 == 60)
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
			else if (m_TextProcessingArray[i].emojiSpriteInfo != null)
			{
				TMP_SpriteAsset tMP_SpriteAsset = m_TextProcessingArray[i].emojiSpriteInfo.spriteAsset;
				m_textElementType = TMP_TextElementType.Sprite;
				int currentMaterialIndex = MaterialReference.AddMaterialReference(tMP_SpriteAsset.material, tMP_SpriteAsset, ref m_materialReferences, m_materialReferenceIndexLookup);
				m_currentMaterialIndex = currentMaterialIndex;
			}
			else
			{
				m_textElementType = m_textInfo.characterInfo[m_characterCount].elementType;
				m_currentMaterialIndex = m_textInfo.characterInfo[m_characterCount].materialReferenceIndex;
				m_currentFontAsset = m_textInfo.characterInfo[m_characterCount].fontAsset;
			}
			int currentMaterialIndex2 = m_currentMaterialIndex;
			bool isUsingAlternateTypeface = m_textInfo.characterInfo[m_characterCount].isUsingAlternateTypeface;
			m_isParsingText = false;
			bool flag5 = false;
			if (characterSubstitution.index == m_characterCount)
			{
				num12 = (int)characterSubstitution.unicode;
				m_textElementType = TMP_TextElementType.Character;
				flag5 = true;
				switch (num12)
				{
				case 3:
					m_internalCharacterInfo[m_characterCount].textElement = m_currentFontAsset.characterLookupTable[3u];
					m_isTextTruncated = true;
					break;
				case 8230:
					m_internalCharacterInfo[m_characterCount].textElement = m_Ellipsis.character;
					m_internalCharacterInfo[m_characterCount].elementType = TMP_TextElementType.Character;
					m_internalCharacterInfo[m_characterCount].fontAsset = m_Ellipsis.fontAsset;
					m_internalCharacterInfo[m_characterCount].material = m_Ellipsis.material;
					m_internalCharacterInfo[m_characterCount].materialReferenceIndex = m_Ellipsis.materialIndex;
					m_isTextTruncated = true;
					characterSubstitution.index = m_characterCount + 1;
					characterSubstitution.unicode = 3u;
					break;
				}
			}
			if (m_characterCount < m_firstVisibleCharacter && num12 != 3)
			{
				m_internalCharacterInfo[m_characterCount].isVisible = false;
				m_internalCharacterInfo[m_characterCount].character = '\u200b';
				m_internalCharacterInfo[m_characterCount].lineNumber = 0;
				m_characterCount++;
				continue;
			}
			float num13 = 1f;
			if (m_textElementType == TMP_TextElementType.Character)
			{
				if ((m_FontStyleInternal & FontStyles.UpperCase) == FontStyles.UpperCase)
				{
					if (char.IsLower((char)num12))
					{
						num12 = char.ToUpper((char)num12);
					}
				}
				else if ((m_FontStyleInternal & FontStyles.LowerCase) == FontStyles.LowerCase)
				{
					if (char.IsUpper((char)num12))
					{
						num12 = char.ToLower((char)num12);
					}
				}
				else if ((m_FontStyleInternal & FontStyles.SmallCaps) == FontStyles.SmallCaps && char.IsLower((char)num12))
				{
					num13 = 0.8f;
					num12 = char.ToUpper((char)num12);
				}
			}
			float num14 = 0f;
			float num15 = 0f;
			if (m_textElementType == TMP_TextElementType.Sprite)
			{
				m_currentSpriteAsset = m_textInfo.characterInfo[m_characterCount].spriteAsset;
				m_spriteIndex = m_textInfo.characterInfo[m_characterCount].spriteIndex;
				TMP_SpriteCharacter tMP_SpriteCharacter = m_currentSpriteAsset.spriteCharacterTable[m_spriteIndex];
				if (tMP_SpriteCharacter == null)
				{
					continue;
				}
				if (num12 == 60)
				{
					num12 = 57344 + m_spriteIndex;
				}
				if (m_currentSpriteAsset.faceInfo.pointSize > 0)
				{
					float num16 = m_currentFontSize / (float)m_currentSpriteAsset.faceInfo.pointSize * m_currentSpriteAsset.faceInfo.scale * (m_isOrthographic ? 1f : 0.1f);
					num2 = tMP_SpriteCharacter.scale * tMP_SpriteCharacter.glyph.scale * num16;
					num14 = m_currentSpriteAsset.faceInfo.ascentLine;
					num15 = m_currentSpriteAsset.faceInfo.descentLine;
				}
				else
				{
					float num17 = m_currentFontSize / (float)m_currentFontAsset.faceInfo.pointSize * m_currentFontAsset.faceInfo.scale * (m_isOrthographic ? 1f : 0.1f);
					num2 = m_currentFontAsset.faceInfo.ascentLine / tMP_SpriteCharacter.glyph.metrics.height * tMP_SpriteCharacter.scale * tMP_SpriteCharacter.glyph.scale * num17;
					float num18 = num17 / num2;
					num14 = m_currentFontAsset.faceInfo.ascentLine * num18;
					num15 = m_currentFontAsset.faceInfo.descentLine * num18;
				}
				m_cached_TextElement = tMP_SpriteCharacter;
				m_internalCharacterInfo[m_characterCount].elementType = TMP_TextElementType.Sprite;
				m_internalCharacterInfo[m_characterCount].scale = num2;
				m_currentMaterialIndex = currentMaterialIndex2;
			}
			else if (m_textElementType == TMP_TextElementType.Character)
			{
				m_cached_TextElement = m_textInfo.characterInfo[m_characterCount].textElement;
				if (m_cached_TextElement == null)
				{
					continue;
				}
				m_currentMaterialIndex = m_textInfo.characterInfo[m_characterCount].materialReferenceIndex;
				float num19 = ((!flag5 || m_TextProcessingArray[i].unicode != 10 || m_characterCount == m_firstCharacterOfLine) ? (m_currentFontSize * num13 / (float)m_currentFontAsset.m_FaceInfo.pointSize * m_currentFontAsset.m_FaceInfo.scale * (m_isOrthographic ? 1f : 0.1f)) : (m_textInfo.characterInfo[m_characterCount - 1].pointSize * num13 / (float)m_currentFontAsset.m_FaceInfo.pointSize * m_currentFontAsset.m_FaceInfo.scale * (m_isOrthographic ? 1f : 0.1f)));
				if (flag5 && num12 == 8230)
				{
					num14 = 0f;
					num15 = 0f;
				}
				else
				{
					num14 = m_currentFontAsset.m_FaceInfo.ascentLine;
					num15 = m_currentFontAsset.m_FaceInfo.descentLine;
				}
				num2 = num19 * m_fontScaleMultiplier * m_cached_TextElement.scale;
				m_internalCharacterInfo[m_characterCount].elementType = TMP_TextElementType.Character;
			}
			float num20 = num2;
			if (num12 == 173 || num12 == 3)
			{
				num2 = 0f;
			}
			m_internalCharacterInfo[m_characterCount].character = (char)num12;
			GlyphMetrics metrics = m_cached_TextElement.m_Glyph.metrics;
			bool flag6 = num12 <= 65535 && char.IsWhiteSpace((char)num12);
			TMP_GlyphValueRecord tMP_GlyphValueRecord = default(TMP_GlyphValueRecord);
			float num21 = m_characterSpacing;
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
						num21 = (((value.m_FeatureLookupFlags & FontFeatureLookupFlags.IgnoreSpacingAdjustments) == FontFeatureLookupFlags.IgnoreSpacingAdjustments) ? 0f : num21);
					}
				}
				if (m_characterCount >= 1)
				{
					uint glyphIndex2 = m_textInfo.characterInfo[m_characterCount - 1].textElement.m_GlyphIndex;
					uint key2 = (glyphIndex << 16) | glyphIndex2;
					if (m_currentFontAsset.m_FontFeatureTable.m_GlyphPairAdjustmentRecordLookupDictionary.TryGetValue(key2, out value))
					{
						tMP_GlyphValueRecord += value.m_SecondAdjustmentRecord.m_GlyphValueRecord;
						num21 = (((value.m_FeatureLookupFlags & FontFeatureLookupFlags.IgnoreSpacingAdjustments) == FontFeatureLookupFlags.IgnoreSpacingAdjustments) ? 0f : num21);
					}
				}
				m_GlyphHorizontalAdvanceAdjustment = tMP_GlyphValueRecord.m_XAdvance;
			}
			float num22 = 0f;
			if (m_monoSpacing != 0f)
			{
				num22 = (m_monoSpacing / 2f - (m_cached_TextElement.glyph.metrics.width / 2f + m_cached_TextElement.glyph.metrics.horizontalBearingX) * num2) * (1f - m_charWidthAdjDelta);
				m_xAdvance += num22;
			}
			float num23 = 0f;
			if (m_textElementType == TMP_TextElementType.Character && !isUsingAlternateTypeface && (m_FontStyleInternal & FontStyles.Bold) == FontStyles.Bold)
			{
				num23 = m_currentFontAsset.boldSpacing;
			}
			m_internalCharacterInfo[m_characterCount].baseLine = 0f - m_lineOffset + m_baselineOffset;
			float num24 = ((m_textElementType == TMP_TextElementType.Character) ? (num14 * num2 / num13 + m_baselineOffset) : (num14 * num2 + m_baselineOffset));
			float num25 = ((m_textElementType == TMP_TextElementType.Character) ? (num15 * num2 / num13 + m_baselineOffset) : (num15 * num2 + m_baselineOffset));
			float num26 = num24;
			float num27 = num25;
			bool flag7 = m_characterCount == m_firstCharacterOfLine;
			if (flag7 || !flag6)
			{
				if (m_baselineOffset != 0f)
				{
					num26 = Mathf.Max((num24 - m_baselineOffset) / m_fontScaleMultiplier, num26);
					num27 = Mathf.Min((num25 - m_baselineOffset) / m_fontScaleMultiplier, num27);
				}
				m_maxLineAscender = Mathf.Max(num26, m_maxLineAscender);
				m_maxLineDescender = Mathf.Min(num27, m_maxLineDescender);
			}
			if (flag7 || !flag6)
			{
				m_internalCharacterInfo[m_characterCount].adjustedAscender = num26;
				m_internalCharacterInfo[m_characterCount].adjustedDescender = num27;
				m_ElementAscender = (m_internalCharacterInfo[m_characterCount].ascender = num24 - m_lineOffset);
				m_ElementDescender = (m_internalCharacterInfo[m_characterCount].descender = num25 - m_lineOffset);
			}
			else
			{
				m_internalCharacterInfo[m_characterCount].adjustedAscender = m_maxLineAscender;
				m_internalCharacterInfo[m_characterCount].adjustedDescender = m_maxLineDescender;
				m_ElementAscender = (m_internalCharacterInfo[m_characterCount].ascender = m_maxLineAscender - m_lineOffset);
				m_ElementDescender = (m_internalCharacterInfo[m_characterCount].descender = m_maxLineDescender - m_lineOffset);
			}
			if ((m_lineNumber == 0 || m_isNewPage) && (flag7 || !flag6))
			{
				m_maxTextAscender = m_maxLineAscender;
				m_maxCapHeight = Mathf.Max(m_maxCapHeight, m_currentFontAsset.m_FaceInfo.capLine * num2 / num13);
			}
			if (m_lineOffset == 0f && (!flag6 || m_characterCount == m_firstCharacterOfLine))
			{
				m_PageAscender = ((m_PageAscender > num24) ? m_PageAscender : num24);
			}
			bool flag8 = (m_lineJustification & HorizontalAlignmentOptions.Flush) == HorizontalAlignmentOptions.Flush || (m_lineJustification & HorizontalAlignmentOptions.Justified) == HorizontalAlignmentOptions.Justified;
			if (num12 == 9 || (!flag6 && num12 != 8203 && num12 != 173 && num12 != 3) || (num12 == 173 && !flag4) || m_textElementType == TMP_TextElementType.Sprite)
			{
				num8 = ((m_width != -1f) ? Mathf.Min(x + 0.0001f - m_marginLeft - m_marginRight, m_width) : (x + 0.0001f - m_marginLeft - m_marginRight));
				num11 = Mathf.Abs(m_xAdvance) + metrics.horizontalAdvance * (1f - m_charWidthAdjDelta) * ((num12 == 173) ? num20 : num2);
				_ = m_characterCount;
				if (num11 > num8 * (flag8 ? 1.05f : 1f) && isWordWrappingEnabled && m_characterCount != m_firstCharacterOfLine)
				{
					i = RestoreWordWrappingState(ref state);
					if (m_internalCharacterInfo[m_characterCount - 1].character == '\u00ad' && !flag4 && m_overflowMode == TextOverflowModes.Overflow)
					{
						characterSubstitution.index = m_characterCount - 1;
						characterSubstitution.unicode = 45u;
						i--;
						m_characterCount--;
						continue;
					}
					flag4 = false;
					if (m_internalCharacterInfo[m_characterCount].character == '\u00ad')
					{
						flag4 = true;
						continue;
					}
					if (isTextAutoSizingEnabled && flag2)
					{
						if (m_charWidthAdjDelta < m_charWidthMaxAdj / 100f && m_AutoSizeIterationCount < m_AutoSizeMaxIterationCount)
						{
							float num28 = num11;
							if (m_charWidthAdjDelta > 0f)
							{
								num28 /= 1f - m_charWidthAdjDelta;
							}
							float num29 = num11 - (num8 - 0.0001f) * (flag8 ? 1.05f : 1f);
							m_charWidthAdjDelta += num29 / num28;
							m_charWidthAdjDelta = Mathf.Min(m_charWidthAdjDelta, m_charWidthMaxAdj / 100f);
							return Vector2.zero;
						}
						if (fontSize > m_fontSizeMin && m_AutoSizeIterationCount < m_AutoSizeMaxIterationCount)
						{
							m_maxFontSize = fontSize;
							float num30 = Mathf.Max((fontSize - m_minFontSize) / 2f, 0.05f);
							fontSize -= num30;
							fontSize = Mathf.Max((float)(int)(fontSize * 20f + 0.5f) / 20f, m_fontSizeMin);
							return Vector2.zero;
						}
					}
					float num31 = m_maxLineAscender - m_startOfLineAscender;
					if (m_lineOffset > 0f && Math.Abs(num31) > 0.01f && !m_IsDrivenLineSpacing && !m_isNewPage)
					{
						m_ElementDescender -= num31;
						m_lineOffset += num31;
					}
					float num32 = m_maxLineAscender - m_lineOffset;
					float num33 = m_maxLineDescender - m_lineOffset;
					m_ElementDescender = ((m_ElementDescender < num33) ? m_ElementDescender : num33);
					if (!flag)
					{
						_ = m_ElementDescender;
					}
					if (m_useMaxVisibleDescender && (m_characterCount >= m_maxVisibleCharacters || m_lineNumber >= m_maxVisibleLines))
					{
						flag = true;
					}
					m_firstCharacterOfLine = m_characterCount;
					m_lineVisibleCharacterCount = 0;
					num9 += m_xAdvance;
					num10 = ((!isWordWrappingEnabled) ? Mathf.Max(num10, num32 - num33) : (m_maxTextAscender - m_ElementDescender));
					SaveWordWrappingState(ref state2, i, m_characterCount - 1);
					m_lineNumber++;
					float adjustedAscender = m_internalCharacterInfo[m_characterCount].adjustedAscender;
					if (m_lineHeight == -32767f)
					{
						m_lineOffset += 0f - m_maxLineDescender + adjustedAscender + (num5 + m_lineSpacingDelta) * num + m_lineSpacing * num3;
						m_IsDrivenLineSpacing = false;
					}
					else
					{
						m_lineOffset += m_lineHeight + m_lineSpacing * num3;
						m_IsDrivenLineSpacing = true;
					}
					m_maxLineAscender = k_LargeNegativeFloat;
					m_maxLineDescender = k_LargePositiveFloat;
					m_startOfLineAscender = adjustedAscender;
					m_xAdvance = 0f + tag_Indent;
					flag2 = true;
					continue;
				}
				num6 = m_marginLeft;
				num7 = m_marginRight;
			}
			if (num12 == 9)
			{
				float num34 = m_currentFontAsset.faceInfo.tabWidth * (float)(int)m_currentFontAsset.tabSize * num2;
				float num35 = Mathf.Ceil(m_xAdvance / num34) * num34;
				m_xAdvance = ((num35 > m_xAdvance) ? num35 : (m_xAdvance + num34));
			}
			else if (m_monoSpacing != 0f)
			{
				m_xAdvance += (m_monoSpacing - num22 + (m_currentFontAsset.normalSpacingOffset + num21) * num3 + m_cSpacing) * (1f - m_charWidthAdjDelta);
				if (flag6 || num12 == 8203)
				{
					m_xAdvance += m_wordSpacing * num3;
				}
			}
			else
			{
				m_xAdvance += ((metrics.horizontalAdvance + tMP_GlyphValueRecord.xAdvance) * num2 + (m_currentFontAsset.normalSpacingOffset + num21 + num23) * num3 + m_cSpacing) * (1f - m_charWidthAdjDelta);
				if (flag6 || num12 == 8203)
				{
					m_xAdvance += m_wordSpacing * num3;
				}
			}
			if (num12 == 13)
			{
				a = Mathf.Max(a, num9 + m_xAdvance);
				num9 = 0f;
				m_xAdvance = 0f + tag_Indent;
			}
			if (num12 == 10 || num12 == 11 || num12 == 3 || num12 == 8232 || num12 == 8233 || m_characterCount == totalCharacterCount - 1)
			{
				float num36 = m_maxLineAscender - m_startOfLineAscender;
				if (m_lineOffset > 0f && Math.Abs(num36) > 0.01f && !m_IsDrivenLineSpacing && !m_isNewPage)
				{
					m_ElementDescender -= num36;
					m_lineOffset += num36;
				}
				m_isNewPage = false;
				float num37 = m_maxLineDescender - m_lineOffset;
				m_ElementDescender = ((m_ElementDescender < num37) ? m_ElementDescender : num37);
				if (m_characterCount == totalCharacterCount - 1)
				{
					num9 = Mathf.Max(a, num9 + num11 + num6 + num7);
				}
				else
				{
					a = Mathf.Max(a, num9 + num11 + num6 + num7);
					num9 = 0f;
				}
				num10 = m_maxTextAscender - m_ElementDescender;
				switch (num12)
				{
				case 10:
				case 11:
				case 45:
				case 8232:
				case 8233:
				{
					SaveWordWrappingState(ref state2, i, m_characterCount);
					SaveWordWrappingState(ref state, i, m_characterCount);
					m_lineNumber++;
					m_firstCharacterOfLine = m_characterCount + 1;
					float adjustedAscender2 = m_internalCharacterInfo[m_characterCount].adjustedAscender;
					if (m_lineHeight == -32767f)
					{
						float num38 = 0f - m_maxLineDescender + adjustedAscender2 + (num5 + m_lineSpacingDelta) * num + (m_lineSpacing + ((num12 == 10 || num12 == 8233) ? m_paragraphSpacing : 0f)) * num3;
						m_lineOffset += num38;
						m_IsDrivenLineSpacing = false;
					}
					else
					{
						m_lineOffset += m_lineHeight + (m_lineSpacing + ((num12 == 10 || num12 == 8233) ? m_paragraphSpacing : 0f)) * num3;
						m_IsDrivenLineSpacing = true;
					}
					m_maxLineAscender = k_LargeNegativeFloat;
					m_maxLineDescender = k_LargePositiveFloat;
					m_startOfLineAscender = adjustedAscender2;
					m_xAdvance = 0f + tag_LineIndent + tag_Indent;
					m_characterCount++;
					continue;
				}
				case 3:
					i = m_TextProcessingArray.Length;
					break;
				}
			}
			if (isWordWrappingEnabled || m_overflowMode == TextOverflowModes.Truncate || m_overflowMode == TextOverflowModes.Ellipsis)
			{
				if ((flag6 || num12 == 8203 || num12 == 45 || num12 == 173) && !m_isNonBreakingSpace && num12 != 160 && num12 != 8199 && num12 != 8209 && num12 != 8239 && num12 != 8288)
				{
					SaveWordWrappingState(ref state, i, m_characterCount);
					flag2 = false;
					flag3 = false;
					state3.previous_WordBreak = -1;
				}
				else if (!m_isNonBreakingSpace && ((((num12 > 4352 && num12 < 4607) || (num12 > 43360 && num12 < 43391) || (num12 > 44032 && num12 < 55295)) && !TMP_Settings.useModernHangulLineBreakingRules) || (num12 > 11904 && num12 < 40959) || (num12 > 63744 && num12 < 64255) || (num12 > 65072 && num12 < 65103) || (num12 > 65280 && num12 < 65519)))
				{
					bool flag9 = TMP_Settings.linebreakingRules.leadingCharacters.ContainsKey(num12);
					bool flag10 = m_characterCount < totalCharacterCount - 1 && TMP_Settings.linebreakingRules.followingCharacters.ContainsKey(m_internalCharacterInfo[m_characterCount + 1].character);
					if (flag2 || !flag9)
					{
						if (!flag10)
						{
							SaveWordWrappingState(ref state, i, m_characterCount);
							flag2 = false;
						}
						if (flag2)
						{
							if (flag6)
							{
								SaveWordWrappingState(ref state3, i, m_characterCount);
							}
							SaveWordWrappingState(ref state, i, m_characterCount);
						}
					}
					flag3 = true;
				}
				else if (flag3)
				{
					if (!TMP_Settings.linebreakingRules.leadingCharacters.ContainsKey(num12))
					{
						SaveWordWrappingState(ref state, i, m_characterCount);
					}
					flag3 = false;
				}
				else if (flag2)
				{
					if (flag6 || (num12 == 173 && !flag4))
					{
						SaveWordWrappingState(ref state3, i, m_characterCount);
					}
					SaveWordWrappingState(ref state, i, m_characterCount);
					flag3 = false;
				}
			}
			m_characterCount++;
		}
		num4 = m_maxFontSize - m_minFontSize;
		if (isTextAutoSizingEnabled && num4 > 0.051f && fontSize < m_fontSizeMax && m_AutoSizeIterationCount < m_AutoSizeMaxIterationCount)
		{
			if (m_charWidthAdjDelta < m_charWidthMaxAdj / 100f)
			{
				m_charWidthAdjDelta = 0f;
			}
			m_minFontSize = fontSize;
			float num39 = Mathf.Max((m_maxFontSize - fontSize) / 2f, 0.05f);
			fontSize += num39;
			fontSize = Mathf.Min((float)(int)(fontSize * 20f + 0.5f) / 20f, m_fontSizeMax);
			return Vector2.zero;
		}
		m_IsAutoSizePointSizeSet = true;
		m_isCalculatingPreferredValues = false;
		num9 += ((m_margin.x > 0f) ? m_margin.x : 0f);
		num9 += ((m_margin.z > 0f) ? m_margin.z : 0f);
		num10 += ((m_margin.y > 0f) ? m_margin.y : 0f);
		num10 += ((m_margin.w > 0f) ? m_margin.w : 0f);
		num9 = (float)(int)(num9 * 100f + 1f) / 100f;
		num10 = (float)(int)(num10 * 100f + 1f) / 100f;
		return new Vector2(num9, num10);
	}

	protected virtual Bounds GetCompoundBounds()
	{
		return default(Bounds);
	}

	internal virtual Rect GetCanvasSpaceClippingRect()
	{
		return Rect.zero;
	}

	protected Bounds GetTextBounds()
	{
		if (m_textInfo == null || m_textInfo.characterCount > m_textInfo.characterInfo.Length)
		{
			return default(Bounds);
		}
		Extents extents = new Extents(k_LargePositiveVector2, k_LargeNegativeVector2);
		for (int i = 0; i < m_textInfo.characterCount && i < m_textInfo.characterInfo.Length; i++)
		{
			if (m_textInfo.characterInfo[i].isVisible)
			{
				extents.min.x = Mathf.Min(extents.min.x, m_textInfo.characterInfo[i].origin);
				extents.min.y = Mathf.Min(extents.min.y, m_textInfo.characterInfo[i].descender);
				extents.max.x = Mathf.Max(extents.max.x, m_textInfo.characterInfo[i].xAdvance);
				extents.max.y = Mathf.Max(extents.max.y, m_textInfo.characterInfo[i].ascender);
			}
		}
		Vector2 vector = default(Vector2);
		vector.x = extents.max.x - extents.min.x;
		vector.y = extents.max.y - extents.min.y;
		return new Bounds((extents.min + extents.max) / 2f, vector);
	}

	protected Bounds GetTextBounds(bool onlyVisibleCharacters)
	{
		if (m_textInfo == null)
		{
			return default(Bounds);
		}
		Extents extents = new Extents(k_LargePositiveVector2, k_LargeNegativeVector2);
		for (int i = 0; i < m_textInfo.characterCount && !((i > maxVisibleCharacters || m_textInfo.characterInfo[i].lineNumber > m_maxVisibleLines) && onlyVisibleCharacters); i++)
		{
			if (!onlyVisibleCharacters || m_textInfo.characterInfo[i].isVisible)
			{
				extents.min.x = Mathf.Min(extents.min.x, m_textInfo.characterInfo[i].origin);
				extents.min.y = Mathf.Min(extents.min.y, m_textInfo.characterInfo[i].descender);
				extents.max.x = Mathf.Max(extents.max.x, m_textInfo.characterInfo[i].xAdvance);
				extents.max.y = Mathf.Max(extents.max.y, m_textInfo.characterInfo[i].ascender);
			}
		}
		Vector2 vector = default(Vector2);
		vector.x = extents.max.x - extents.min.x;
		vector.y = extents.max.y - extents.min.y;
		return new Bounds((extents.min + extents.max) / 2f, vector);
	}

	protected void AdjustLineOffset(int startIndex, int endIndex, float offset)
	{
		Vector3 vector = new Vector3(0f, offset, 0f);
		for (int i = startIndex; i <= endIndex; i++)
		{
			m_textInfo.characterInfo[i].bottomLeft -= vector;
			m_textInfo.characterInfo[i].topLeft -= vector;
			m_textInfo.characterInfo[i].topRight -= vector;
			m_textInfo.characterInfo[i].bottomRight -= vector;
			m_textInfo.characterInfo[i].ascender -= vector.y;
			m_textInfo.characterInfo[i].baseLine -= vector.y;
			m_textInfo.characterInfo[i].descender -= vector.y;
			if (m_textInfo.characterInfo[i].isVisible)
			{
				m_textInfo.characterInfo[i].vertex_BL.position -= vector;
				m_textInfo.characterInfo[i].vertex_TL.position -= vector;
				m_textInfo.characterInfo[i].vertex_TR.position -= vector;
				m_textInfo.characterInfo[i].vertex_BR.position -= vector;
			}
		}
	}

	protected void ResizeLineExtents(int size)
	{
		size = ((size > 1024) ? (size + 256) : Mathf.NextPowerOfTwo(size + 1));
		TMP_LineInfo[] array = new TMP_LineInfo[size];
		for (int i = 0; i < size; i++)
		{
			if (i < m_textInfo.lineInfo.Length)
			{
				array[i] = m_textInfo.lineInfo[i];
				continue;
			}
			array[i].lineExtents.min = k_LargePositiveVector2;
			array[i].lineExtents.max = k_LargeNegativeVector2;
			array[i].ascender = k_LargeNegativeFloat;
			array[i].descender = k_LargePositiveFloat;
		}
		m_textInfo.lineInfo = array;
	}

	public virtual TMP_TextInfo GetTextInfo(string text)
	{
		return null;
	}

	public virtual void ComputeMarginSize()
	{
	}

	protected void InsertNewLine(int i, float baseScale, float currentElementScale, float currentEmScale, float glyphAdjustment, float boldSpacingAdjustment, float characterSpacingAdjustment, float width, float lineGap, ref bool isMaxVisibleDescenderSet, ref float maxVisibleDescender)
	{
		float num = m_maxLineAscender - m_startOfLineAscender;
		if (m_lineOffset > 0f && Math.Abs(num) > 0.01f && !m_IsDrivenLineSpacing && !m_isNewPage)
		{
			AdjustLineOffset(m_firstCharacterOfLine, m_characterCount, num);
			m_ElementDescender -= num;
			m_lineOffset += num;
		}
		float num2 = m_maxLineAscender - m_lineOffset;
		float num3 = m_maxLineDescender - m_lineOffset;
		m_ElementDescender = ((m_ElementDescender < num3) ? m_ElementDescender : num3);
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
		int num4 = (m_textInfo.lineInfo[m_lineNumber].lastCharacterIndex = (m_lastCharacterOfLine = ((m_characterCount - 1 > 0) ? (m_characterCount - 1) : 0)));
		m_textInfo.lineInfo[m_lineNumber].lastVisibleCharacterIndex = (m_lastVisibleCharacterOfLine = ((m_lastVisibleCharacterOfLine < m_firstVisibleCharacterOfLine) ? m_firstVisibleCharacterOfLine : m_lastVisibleCharacterOfLine));
		m_textInfo.lineInfo[m_lineNumber].characterCount = m_textInfo.lineInfo[m_lineNumber].lastCharacterIndex - m_textInfo.lineInfo[m_lineNumber].firstCharacterIndex + 1;
		m_textInfo.lineInfo[m_lineNumber].visibleCharacterCount = m_lineVisibleCharacterCount;
		m_textInfo.lineInfo[m_lineNumber].lineExtents.min = new Vector2(m_textInfo.characterInfo[m_firstVisibleCharacterOfLine].bottomLeft.x, num3);
		m_textInfo.lineInfo[m_lineNumber].lineExtents.max = new Vector2(m_textInfo.characterInfo[m_lastVisibleCharacterOfLine].topRight.x, num2);
		m_textInfo.lineInfo[m_lineNumber].length = m_textInfo.lineInfo[m_lineNumber].lineExtents.max.x;
		m_textInfo.lineInfo[m_lineNumber].width = width;
		float num5 = (glyphAdjustment * currentElementScale + (m_currentFontAsset.normalSpacingOffset + characterSpacingAdjustment + boldSpacingAdjustment) * currentEmScale - m_cSpacing) * (1f - m_charWidthAdjDelta);
		float xAdvance = (m_textInfo.lineInfo[m_lineNumber].maxAdvance = m_textInfo.characterInfo[m_lastVisibleCharacterOfLine].xAdvance + (m_isRightToLeft ? num5 : (0f - num5)));
		m_textInfo.characterInfo[num4].xAdvance = xAdvance;
		m_textInfo.lineInfo[m_lineNumber].baseline = 0f - m_lineOffset;
		m_textInfo.lineInfo[m_lineNumber].ascender = num2;
		m_textInfo.lineInfo[m_lineNumber].descender = num3;
		m_textInfo.lineInfo[m_lineNumber].lineHeight = num2 - num3 + lineGap * baseScale;
		m_firstCharacterOfLine = m_characterCount;
		m_lineVisibleCharacterCount = 0;
		SaveWordWrappingState(ref m_SavedLineState, i, m_characterCount - 1);
		m_lineNumber++;
		if (m_lineNumber >= m_textInfo.lineInfo.Length)
		{
			ResizeLineExtents(m_lineNumber);
		}
		if (m_lineHeight == -32767f)
		{
			float adjustedAscender = m_textInfo.characterInfo[m_characterCount].adjustedAscender;
			float num6 = 0f - m_maxLineDescender + adjustedAscender + (lineGap + m_lineSpacingDelta) * baseScale + m_lineSpacing * currentEmScale;
			m_lineOffset += num6;
			m_startOfLineAscender = adjustedAscender;
		}
		else
		{
			m_lineOffset += m_lineHeight + m_lineSpacing * currentEmScale;
		}
		m_maxLineAscender = k_LargeNegativeFloat;
		m_maxLineDescender = k_LargePositiveFloat;
		m_xAdvance = 0f + tag_Indent;
	}

	protected void SaveWordWrappingState(ref WordWrapState state, int index, int count)
	{
		state.currentFontAsset = m_currentFontAsset;
		state.currentSpriteAsset = m_currentSpriteAsset;
		state.currentMaterial = m_currentMaterial;
		state.currentMaterialIndex = m_currentMaterialIndex;
		state.previous_WordBreak = index;
		state.total_CharacterCount = count;
		state.visible_CharacterCount = m_lineVisibleCharacterCount;
		state.visible_LinkCount = m_textInfo.linkCount;
		state.firstCharacterIndex = m_firstCharacterOfLine;
		state.firstVisibleCharacterIndex = m_firstVisibleCharacterOfLine;
		state.lastVisibleCharIndex = m_lastVisibleCharacterOfLine;
		state.fontStyle = m_FontStyleInternal;
		state.italicAngle = m_ItalicAngle;
		state.fontScaleMultiplier = m_fontScaleMultiplier;
		state.currentFontSize = m_currentFontSize;
		state.xAdvance = m_xAdvance;
		state.maxCapHeight = m_maxCapHeight;
		state.maxAscender = m_maxTextAscender;
		state.maxDescender = m_ElementDescender;
		state.startOfLineAscender = m_startOfLineAscender;
		state.maxLineAscender = m_maxLineAscender;
		state.maxLineDescender = m_maxLineDescender;
		state.pageAscender = m_PageAscender;
		state.preferredWidth = m_preferredWidth;
		state.preferredHeight = m_preferredHeight;
		state.meshExtents = m_meshExtents;
		state.lineNumber = m_lineNumber;
		state.lineOffset = m_lineOffset;
		state.baselineOffset = m_baselineOffset;
		state.isDrivenLineSpacing = m_IsDrivenLineSpacing;
		state.glyphHorizontalAdvanceAdjustment = m_GlyphHorizontalAdvanceAdjustment;
		state.cSpace = m_cSpacing;
		state.mSpace = m_monoSpacing;
		state.horizontalAlignment = m_lineJustification;
		state.marginLeft = m_marginLeft;
		state.marginRight = m_marginRight;
		state.vertexColor = m_htmlColor;
		state.underlineColor = m_underlineColor;
		state.strikethroughColor = m_strikethroughColor;
		state.isNonBreakingSpace = m_isNonBreakingSpace;
		state.tagNoParsing = tag_NoParsing;
		state.basicStyleStack = m_fontStyleStack;
		state.italicAngleStack = m_ItalicAngleStack;
		state.colorStack = m_colorStack;
		state.underlineColorStack = m_underlineColorStack;
		state.strikethroughColorStack = m_strikethroughColorStack;
		state.highlightStateStack = m_HighlightStateStack;
		state.colorGradientStack = m_colorGradientStack;
		state.sizeStack = m_sizeStack;
		state.indentStack = m_indentStack;
		state.fontWeightStack = m_FontWeightStack;
		state.baselineStack = m_baselineOffsetStack;
		state.actionStack = m_actionStack;
		state.materialReferenceStack = m_materialReferenceStack;
		state.lineJustificationStack = m_lineJustificationStack;
		state.spriteAnimationID = m_spriteAnimationID;
		if (m_lineNumber < m_textInfo.lineInfo.Length)
		{
			state.lineInfo = m_textInfo.lineInfo[m_lineNumber];
		}
	}

	protected int RestoreWordWrappingState(ref WordWrapState state)
	{
		int previous_WordBreak = state.previous_WordBreak;
		m_currentFontAsset = state.currentFontAsset;
		m_currentSpriteAsset = state.currentSpriteAsset;
		m_currentMaterial = state.currentMaterial;
		m_currentMaterialIndex = state.currentMaterialIndex;
		m_characterCount = state.total_CharacterCount + 1;
		m_lineVisibleCharacterCount = state.visible_CharacterCount;
		m_textInfo.linkCount = state.visible_LinkCount;
		m_firstCharacterOfLine = state.firstCharacterIndex;
		m_firstVisibleCharacterOfLine = state.firstVisibleCharacterIndex;
		m_lastVisibleCharacterOfLine = state.lastVisibleCharIndex;
		m_FontStyleInternal = state.fontStyle;
		m_ItalicAngle = state.italicAngle;
		m_fontScaleMultiplier = state.fontScaleMultiplier;
		m_currentFontSize = state.currentFontSize;
		m_xAdvance = state.xAdvance;
		m_maxCapHeight = state.maxCapHeight;
		m_maxTextAscender = state.maxAscender;
		m_ElementDescender = state.maxDescender;
		m_startOfLineAscender = state.startOfLineAscender;
		m_maxLineAscender = state.maxLineAscender;
		m_maxLineDescender = state.maxLineDescender;
		m_PageAscender = state.pageAscender;
		m_preferredWidth = state.preferredWidth;
		m_preferredHeight = state.preferredHeight;
		m_meshExtents = state.meshExtents;
		m_lineNumber = state.lineNumber;
		m_lineOffset = state.lineOffset;
		m_baselineOffset = state.baselineOffset;
		m_IsDrivenLineSpacing = state.isDrivenLineSpacing;
		m_GlyphHorizontalAdvanceAdjustment = state.glyphHorizontalAdvanceAdjustment;
		m_cSpacing = state.cSpace;
		m_monoSpacing = state.mSpace;
		m_lineJustification = state.horizontalAlignment;
		m_marginLeft = state.marginLeft;
		m_marginRight = state.marginRight;
		m_htmlColor = state.vertexColor;
		m_underlineColor = state.underlineColor;
		m_strikethroughColor = state.strikethroughColor;
		m_isNonBreakingSpace = state.isNonBreakingSpace;
		tag_NoParsing = state.tagNoParsing;
		m_fontStyleStack = state.basicStyleStack;
		m_ItalicAngleStack = state.italicAngleStack;
		m_colorStack = state.colorStack;
		m_underlineColorStack = state.underlineColorStack;
		m_strikethroughColorStack = state.strikethroughColorStack;
		m_HighlightStateStack = state.highlightStateStack;
		m_colorGradientStack = state.colorGradientStack;
		m_sizeStack = state.sizeStack;
		m_indentStack = state.indentStack;
		m_FontWeightStack = state.fontWeightStack;
		m_baselineOffsetStack = state.baselineStack;
		m_actionStack = state.actionStack;
		m_materialReferenceStack = state.materialReferenceStack;
		m_lineJustificationStack = state.lineJustificationStack;
		m_spriteAnimationID = state.spriteAnimationID;
		if (m_lineNumber < m_textInfo.lineInfo.Length)
		{
			m_textInfo.lineInfo[m_lineNumber] = state.lineInfo;
		}
		return previous_WordBreak;
	}

	protected virtual void SaveGlyphVertexInfo(float padding, float style_padding, Color32 vertexColor)
	{
		m_textInfo.characterInfo[m_characterCount].vertex_BL.position = m_textInfo.characterInfo[m_characterCount].bottomLeft;
		m_textInfo.characterInfo[m_characterCount].vertex_TL.position = m_textInfo.characterInfo[m_characterCount].topLeft;
		m_textInfo.characterInfo[m_characterCount].vertex_TR.position = m_textInfo.characterInfo[m_characterCount].topRight;
		m_textInfo.characterInfo[m_characterCount].vertex_BR.position = m_textInfo.characterInfo[m_characterCount].bottomRight;
		vertexColor.a = ((m_fontColor32.a < vertexColor.a) ? m_fontColor32.a : vertexColor.a);
		if (!m_enableVertexGradient)
		{
			m_textInfo.characterInfo[m_characterCount].vertex_BL.color = vertexColor;
			m_textInfo.characterInfo[m_characterCount].vertex_TL.color = vertexColor;
			m_textInfo.characterInfo[m_characterCount].vertex_TR.color = vertexColor;
			m_textInfo.characterInfo[m_characterCount].vertex_BR.color = vertexColor;
		}
		else if (!m_overrideHtmlColors && m_colorStack.index > 1)
		{
			m_textInfo.characterInfo[m_characterCount].vertex_BL.color = vertexColor;
			m_textInfo.characterInfo[m_characterCount].vertex_TL.color = vertexColor;
			m_textInfo.characterInfo[m_characterCount].vertex_TR.color = vertexColor;
			m_textInfo.characterInfo[m_characterCount].vertex_BR.color = vertexColor;
		}
		else if (m_fontColorGradientPreset != null)
		{
			m_textInfo.characterInfo[m_characterCount].vertex_BL.color = m_fontColorGradientPreset.bottomLeft * vertexColor;
			m_textInfo.characterInfo[m_characterCount].vertex_TL.color = m_fontColorGradientPreset.topLeft * vertexColor;
			m_textInfo.characterInfo[m_characterCount].vertex_TR.color = m_fontColorGradientPreset.topRight * vertexColor;
			m_textInfo.characterInfo[m_characterCount].vertex_BR.color = m_fontColorGradientPreset.bottomRight * vertexColor;
		}
		else
		{
			m_textInfo.characterInfo[m_characterCount].vertex_BL.color = m_fontColorGradient.bottomLeft * vertexColor;
			m_textInfo.characterInfo[m_characterCount].vertex_TL.color = m_fontColorGradient.topLeft * vertexColor;
			m_textInfo.characterInfo[m_characterCount].vertex_TR.color = m_fontColorGradient.topRight * vertexColor;
			m_textInfo.characterInfo[m_characterCount].vertex_BR.color = m_fontColorGradient.bottomRight * vertexColor;
		}
		if (m_colorGradientPreset != null)
		{
			if (m_colorGradientPresetIsTinted)
			{
				ref Color32 reference = ref m_textInfo.characterInfo[m_characterCount].vertex_BL.color;
				reference *= m_colorGradientPreset.bottomLeft;
				ref Color32 reference2 = ref m_textInfo.characterInfo[m_characterCount].vertex_TL.color;
				reference2 *= m_colorGradientPreset.topLeft;
				ref Color32 reference3 = ref m_textInfo.characterInfo[m_characterCount].vertex_TR.color;
				reference3 *= m_colorGradientPreset.topRight;
				ref Color32 reference4 = ref m_textInfo.characterInfo[m_characterCount].vertex_BR.color;
				reference4 *= m_colorGradientPreset.bottomRight;
			}
			else
			{
				m_textInfo.characterInfo[m_characterCount].vertex_BL.color = m_colorGradientPreset.bottomLeft.MinAlpha(vertexColor);
				m_textInfo.characterInfo[m_characterCount].vertex_TL.color = m_colorGradientPreset.topLeft.MinAlpha(vertexColor);
				m_textInfo.characterInfo[m_characterCount].vertex_TR.color = m_colorGradientPreset.topRight.MinAlpha(vertexColor);
				m_textInfo.characterInfo[m_characterCount].vertex_BR.color = m_colorGradientPreset.bottomRight.MinAlpha(vertexColor);
			}
		}
		if (!m_isSDFShader)
		{
			style_padding = 0f;
		}
		GlyphRect glyphRect = m_cached_TextElement.m_Glyph.glyphRect;
		Vector2 uv = default(Vector2);
		uv.x = ((float)glyphRect.x - padding - style_padding) / (float)m_currentFontAsset.m_AtlasWidth;
		uv.y = ((float)glyphRect.y - padding - style_padding) / (float)m_currentFontAsset.m_AtlasHeight;
		Vector2 uv2 = default(Vector2);
		uv2.x = uv.x;
		uv2.y = ((float)glyphRect.y + padding + style_padding + (float)glyphRect.height) / (float)m_currentFontAsset.m_AtlasHeight;
		Vector2 uv3 = default(Vector2);
		uv3.x = ((float)glyphRect.x + padding + style_padding + (float)glyphRect.width) / (float)m_currentFontAsset.m_AtlasWidth;
		uv3.y = uv2.y;
		Vector2 uv4 = default(Vector2);
		uv4.x = uv3.x;
		uv4.y = uv.y;
		m_textInfo.characterInfo[m_characterCount].vertex_BL.uv = uv;
		m_textInfo.characterInfo[m_characterCount].vertex_TL.uv = uv2;
		m_textInfo.characterInfo[m_characterCount].vertex_TR.uv = uv3;
		m_textInfo.characterInfo[m_characterCount].vertex_BR.uv = uv4;
	}

	protected virtual void SaveSpriteVertexInfo(Color32 vertexColor)
	{
		m_textInfo.characterInfo[m_characterCount].vertex_BL.position = m_textInfo.characterInfo[m_characterCount].bottomLeft;
		m_textInfo.characterInfo[m_characterCount].vertex_TL.position = m_textInfo.characterInfo[m_characterCount].topLeft;
		m_textInfo.characterInfo[m_characterCount].vertex_TR.position = m_textInfo.characterInfo[m_characterCount].topRight;
		m_textInfo.characterInfo[m_characterCount].vertex_BR.position = m_textInfo.characterInfo[m_characterCount].bottomRight;
		if (m_tintAllSprites)
		{
			m_tintSprite = true;
		}
		Color32 color = (m_tintSprite ? m_spriteColor.Multiply(vertexColor) : m_spriteColor);
		color.a = ((color.a < m_fontColor32.a) ? (color.a = ((color.a < vertexColor.a) ? color.a : vertexColor.a)) : m_fontColor32.a);
		Color32 color2 = color;
		Color32 color3 = color;
		Color32 color4 = color;
		Color32 color5 = color;
		if (m_enableVertexGradient)
		{
			if (m_fontColorGradientPreset != null)
			{
				color2 = (m_tintSprite ? color2.Multiply(m_fontColorGradientPreset.bottomLeft) : color2);
				color3 = (m_tintSprite ? color3.Multiply(m_fontColorGradientPreset.topLeft) : color3);
				color4 = (m_tintSprite ? color4.Multiply(m_fontColorGradientPreset.topRight) : color4);
				color5 = (m_tintSprite ? color5.Multiply(m_fontColorGradientPreset.bottomRight) : color5);
			}
			else
			{
				color2 = (m_tintSprite ? color2.Multiply(m_fontColorGradient.bottomLeft) : color2);
				color3 = (m_tintSprite ? color3.Multiply(m_fontColorGradient.topLeft) : color3);
				color4 = (m_tintSprite ? color4.Multiply(m_fontColorGradient.topRight) : color4);
				color5 = (m_tintSprite ? color5.Multiply(m_fontColorGradient.bottomRight) : color5);
			}
		}
		if (m_colorGradientPreset != null)
		{
			color2 = (m_tintSprite ? color2.Multiply(m_colorGradientPreset.bottomLeft) : color2);
			color3 = (m_tintSprite ? color3.Multiply(m_colorGradientPreset.topLeft) : color3);
			color4 = (m_tintSprite ? color4.Multiply(m_colorGradientPreset.topRight) : color4);
			color5 = (m_tintSprite ? color5.Multiply(m_colorGradientPreset.bottomRight) : color5);
		}
		m_textInfo.characterInfo[m_characterCount].vertex_BL.color = color2;
		m_textInfo.characterInfo[m_characterCount].vertex_TL.color = color3;
		m_textInfo.characterInfo[m_characterCount].vertex_TR.color = color4;
		m_textInfo.characterInfo[m_characterCount].vertex_BR.color = color5;
		GlyphRect glyphRect = m_cached_TextElement.m_Glyph.glyphRect;
		Vector2 uv = new Vector2((float)glyphRect.x / (float)m_currentSpriteAsset.spriteSheet.width, (float)glyphRect.y / (float)m_currentSpriteAsset.spriteSheet.height);
		Vector2 uv2 = new Vector2(uv.x, (float)(glyphRect.y + glyphRect.height) / (float)m_currentSpriteAsset.spriteSheet.height);
		Vector2 uv3 = new Vector2((float)(glyphRect.x + glyphRect.width) / (float)m_currentSpriteAsset.spriteSheet.width, uv2.y);
		Vector2 uv4 = new Vector2(uv3.x, uv.y);
		m_textInfo.characterInfo[m_characterCount].vertex_BL.uv = uv;
		m_textInfo.characterInfo[m_characterCount].vertex_TL.uv = uv2;
		m_textInfo.characterInfo[m_characterCount].vertex_TR.uv = uv3;
		m_textInfo.characterInfo[m_characterCount].vertex_BR.uv = uv4;
	}

	protected virtual void FillCharacterVertexBuffers(int i, int index_X4)
	{
		int materialReferenceIndex = m_textInfo.characterInfo[i].materialReferenceIndex;
		index_X4 = m_textInfo.meshInfo[materialReferenceIndex].vertexCount;
		if (index_X4 >= m_textInfo.meshInfo[materialReferenceIndex].vertices.Length)
		{
			m_textInfo.meshInfo[materialReferenceIndex].ResizeMeshInfo(Mathf.NextPowerOfTwo((index_X4 + 4) / 4));
		}
		TMP_CharacterInfo[] characterInfo = m_textInfo.characterInfo;
		m_textInfo.characterInfo[i].vertexIndex = index_X4;
		m_textInfo.meshInfo[materialReferenceIndex].vertices[index_X4] = characterInfo[i].vertex_BL.position;
		m_textInfo.meshInfo[materialReferenceIndex].vertices[1 + index_X4] = characterInfo[i].vertex_TL.position;
		m_textInfo.meshInfo[materialReferenceIndex].vertices[2 + index_X4] = characterInfo[i].vertex_TR.position;
		m_textInfo.meshInfo[materialReferenceIndex].vertices[3 + index_X4] = characterInfo[i].vertex_BR.position;
		m_textInfo.meshInfo[materialReferenceIndex].uvs0[index_X4] = characterInfo[i].vertex_BL.uv;
		m_textInfo.meshInfo[materialReferenceIndex].uvs0[1 + index_X4] = characterInfo[i].vertex_TL.uv;
		m_textInfo.meshInfo[materialReferenceIndex].uvs0[2 + index_X4] = characterInfo[i].vertex_TR.uv;
		m_textInfo.meshInfo[materialReferenceIndex].uvs0[3 + index_X4] = characterInfo[i].vertex_BR.uv;
		m_textInfo.meshInfo[materialReferenceIndex].uvs2[index_X4] = characterInfo[i].vertex_BL.uv2;
		m_textInfo.meshInfo[materialReferenceIndex].uvs2[1 + index_X4] = characterInfo[i].vertex_TL.uv2;
		m_textInfo.meshInfo[materialReferenceIndex].uvs2[2 + index_X4] = characterInfo[i].vertex_TR.uv2;
		m_textInfo.meshInfo[materialReferenceIndex].uvs2[3 + index_X4] = characterInfo[i].vertex_BR.uv2;
		m_textInfo.meshInfo[materialReferenceIndex].colors32[index_X4] = characterInfo[i].vertex_BL.color;
		m_textInfo.meshInfo[materialReferenceIndex].colors32[1 + index_X4] = characterInfo[i].vertex_TL.color;
		m_textInfo.meshInfo[materialReferenceIndex].colors32[2 + index_X4] = characterInfo[i].vertex_TR.color;
		m_textInfo.meshInfo[materialReferenceIndex].colors32[3 + index_X4] = characterInfo[i].vertex_BR.color;
		m_textInfo.meshInfo[materialReferenceIndex].vertexCount = index_X4 + 4;
	}

	protected virtual void FillCharacterVertexBuffers(int i, int index_X4, bool isVolumetric)
	{
		int materialReferenceIndex = m_textInfo.characterInfo[i].materialReferenceIndex;
		index_X4 = m_textInfo.meshInfo[materialReferenceIndex].vertexCount;
		if (index_X4 >= m_textInfo.meshInfo[materialReferenceIndex].vertices.Length)
		{
			m_textInfo.meshInfo[materialReferenceIndex].ResizeMeshInfo(Mathf.NextPowerOfTwo((index_X4 + (isVolumetric ? 8 : 4)) / 4));
		}
		TMP_CharacterInfo[] characterInfo = m_textInfo.characterInfo;
		m_textInfo.characterInfo[i].vertexIndex = index_X4;
		m_textInfo.meshInfo[materialReferenceIndex].vertices[index_X4] = characterInfo[i].vertex_BL.position;
		m_textInfo.meshInfo[materialReferenceIndex].vertices[1 + index_X4] = characterInfo[i].vertex_TL.position;
		m_textInfo.meshInfo[materialReferenceIndex].vertices[2 + index_X4] = characterInfo[i].vertex_TR.position;
		m_textInfo.meshInfo[materialReferenceIndex].vertices[3 + index_X4] = characterInfo[i].vertex_BR.position;
		m_textInfo.meshInfo[materialReferenceIndex].uvs0[index_X4] = characterInfo[i].vertex_BL.uv;
		m_textInfo.meshInfo[materialReferenceIndex].uvs0[1 + index_X4] = characterInfo[i].vertex_TL.uv;
		m_textInfo.meshInfo[materialReferenceIndex].uvs0[2 + index_X4] = characterInfo[i].vertex_TR.uv;
		m_textInfo.meshInfo[materialReferenceIndex].uvs0[3 + index_X4] = characterInfo[i].vertex_BR.uv;
		if (isVolumetric)
		{
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[4 + index_X4] = characterInfo[i].vertex_BL.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[5 + index_X4] = characterInfo[i].vertex_TL.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[6 + index_X4] = characterInfo[i].vertex_TR.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[7 + index_X4] = characterInfo[i].vertex_BR.uv;
		}
		m_textInfo.meshInfo[materialReferenceIndex].uvs2[index_X4] = characterInfo[i].vertex_BL.uv2;
		m_textInfo.meshInfo[materialReferenceIndex].uvs2[1 + index_X4] = characterInfo[i].vertex_TL.uv2;
		m_textInfo.meshInfo[materialReferenceIndex].uvs2[2 + index_X4] = characterInfo[i].vertex_TR.uv2;
		m_textInfo.meshInfo[materialReferenceIndex].uvs2[3 + index_X4] = characterInfo[i].vertex_BR.uv2;
		if (isVolumetric)
		{
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[4 + index_X4] = characterInfo[i].vertex_BL.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[5 + index_X4] = characterInfo[i].vertex_TL.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[6 + index_X4] = characterInfo[i].vertex_TR.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[7 + index_X4] = characterInfo[i].vertex_BR.uv2;
		}
		m_textInfo.meshInfo[materialReferenceIndex].colors32[index_X4] = characterInfo[i].vertex_BL.color;
		m_textInfo.meshInfo[materialReferenceIndex].colors32[1 + index_X4] = characterInfo[i].vertex_TL.color;
		m_textInfo.meshInfo[materialReferenceIndex].colors32[2 + index_X4] = characterInfo[i].vertex_TR.color;
		m_textInfo.meshInfo[materialReferenceIndex].colors32[3 + index_X4] = characterInfo[i].vertex_BR.color;
		if (isVolumetric)
		{
			Color32 color = new Color32(byte.MaxValue, byte.MaxValue, 128, byte.MaxValue);
			m_textInfo.meshInfo[materialReferenceIndex].colors32[4 + index_X4] = color;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[5 + index_X4] = color;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[6 + index_X4] = color;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[7 + index_X4] = color;
		}
		m_textInfo.meshInfo[materialReferenceIndex].vertexCount = index_X4 + ((!isVolumetric) ? 4 : 8);
	}

	protected virtual void FillSpriteVertexBuffers(int i, int index_X4)
	{
		int materialReferenceIndex = m_textInfo.characterInfo[i].materialReferenceIndex;
		index_X4 = m_textInfo.meshInfo[materialReferenceIndex].vertexCount;
		if (index_X4 >= m_textInfo.meshInfo[materialReferenceIndex].vertices.Length)
		{
			m_textInfo.meshInfo[materialReferenceIndex].ResizeMeshInfo(Mathf.NextPowerOfTwo((index_X4 + 4) / 4));
		}
		TMP_CharacterInfo[] characterInfo = m_textInfo.characterInfo;
		m_textInfo.characterInfo[i].vertexIndex = index_X4;
		m_textInfo.meshInfo[materialReferenceIndex].vertices[index_X4] = characterInfo[i].vertex_BL.position;
		m_textInfo.meshInfo[materialReferenceIndex].vertices[1 + index_X4] = characterInfo[i].vertex_TL.position;
		m_textInfo.meshInfo[materialReferenceIndex].vertices[2 + index_X4] = characterInfo[i].vertex_TR.position;
		m_textInfo.meshInfo[materialReferenceIndex].vertices[3 + index_X4] = characterInfo[i].vertex_BR.position;
		m_textInfo.meshInfo[materialReferenceIndex].uvs0[index_X4] = characterInfo[i].vertex_BL.uv;
		m_textInfo.meshInfo[materialReferenceIndex].uvs0[1 + index_X4] = characterInfo[i].vertex_TL.uv;
		m_textInfo.meshInfo[materialReferenceIndex].uvs0[2 + index_X4] = characterInfo[i].vertex_TR.uv;
		m_textInfo.meshInfo[materialReferenceIndex].uvs0[3 + index_X4] = characterInfo[i].vertex_BR.uv;
		m_textInfo.meshInfo[materialReferenceIndex].uvs2[index_X4] = characterInfo[i].vertex_BL.uv2;
		m_textInfo.meshInfo[materialReferenceIndex].uvs2[1 + index_X4] = characterInfo[i].vertex_TL.uv2;
		m_textInfo.meshInfo[materialReferenceIndex].uvs2[2 + index_X4] = characterInfo[i].vertex_TR.uv2;
		m_textInfo.meshInfo[materialReferenceIndex].uvs2[3 + index_X4] = characterInfo[i].vertex_BR.uv2;
		m_textInfo.meshInfo[materialReferenceIndex].colors32[index_X4] = characterInfo[i].vertex_BL.color;
		m_textInfo.meshInfo[materialReferenceIndex].colors32[1 + index_X4] = characterInfo[i].vertex_TL.color;
		m_textInfo.meshInfo[materialReferenceIndex].colors32[2 + index_X4] = characterInfo[i].vertex_TR.color;
		m_textInfo.meshInfo[materialReferenceIndex].colors32[3 + index_X4] = characterInfo[i].vertex_BR.color;
		m_textInfo.meshInfo[materialReferenceIndex].vertexCount = index_X4 + 4;
	}

	protected virtual void DrawUnderlineMesh(Vector3 start, Vector3 end, ref int index, float startScale, float endScale, float maxScale, float sdfScale, Color32 underlineColor)
	{
		GetUnderlineSpecialCharacter(m_fontAsset);
		if (m_Underline.character == null)
		{
			if (!TMP_Settings.warningsDisabled)
			{
				UnityEngine.Debug.LogWarning("Unable to add underline since the primary Font Asset doesn't contain the underline character.", this);
			}
			return;
		}
		int materialIndex = m_Underline.materialIndex;
		int num = index + 12;
		if (num > m_textInfo.meshInfo[materialIndex].vertices.Length)
		{
			m_textInfo.meshInfo[materialIndex].ResizeMeshInfo(num / 4);
		}
		start.y = Mathf.Min(start.y, end.y);
		end.y = Mathf.Min(start.y, end.y);
		GlyphMetrics metrics = m_Underline.character.glyph.metrics;
		GlyphRect glyphRect = m_Underline.character.glyph.glyphRect;
		float num2 = metrics.width / 2f * maxScale;
		if (end.x - start.x < metrics.width * maxScale)
		{
			num2 = (end.x - start.x) / 2f;
		}
		float num3 = m_padding * startScale / maxScale;
		float num4 = m_padding * endScale / maxScale;
		float underlineThickness = m_Underline.fontAsset.faceInfo.underlineThickness;
		Vector3[] vertices = m_textInfo.meshInfo[materialIndex].vertices;
		vertices[index] = start + new Vector3(0f, 0f - (underlineThickness + m_padding) * maxScale, 0f);
		vertices[index + 1] = start + new Vector3(0f, m_padding * maxScale, 0f);
		vertices[index + 2] = vertices[index + 1] + new Vector3(num2, 0f, 0f);
		vertices[index + 3] = vertices[index] + new Vector3(num2, 0f, 0f);
		vertices[index + 4] = vertices[index + 3];
		vertices[index + 5] = vertices[index + 2];
		vertices[index + 6] = end + new Vector3(0f - num2, m_padding * maxScale, 0f);
		vertices[index + 7] = end + new Vector3(0f - num2, (0f - (underlineThickness + m_padding)) * maxScale, 0f);
		vertices[index + 8] = vertices[index + 7];
		vertices[index + 9] = vertices[index + 6];
		vertices[index + 10] = end + new Vector3(0f, m_padding * maxScale, 0f);
		vertices[index + 11] = end + new Vector3(0f, (0f - (underlineThickness + m_padding)) * maxScale, 0f);
		Vector2[] uvs = m_textInfo.meshInfo[materialIndex].uvs0;
		int atlasWidth = m_Underline.fontAsset.atlasWidth;
		int atlasHeight = m_Underline.fontAsset.atlasHeight;
		Vector2 vector = new Vector2(((float)glyphRect.x - num3) / (float)atlasWidth, ((float)glyphRect.y - m_padding) / (float)atlasHeight);
		Vector2 vector2 = new Vector2(vector.x, ((float)(glyphRect.y + glyphRect.height) + m_padding) / (float)atlasHeight);
		Vector2 vector3 = new Vector2(((float)glyphRect.x - num3 + (float)glyphRect.width / 2f) / (float)atlasWidth, vector2.y);
		Vector2 vector4 = new Vector2(vector3.x, vector.y);
		Vector2 vector5 = new Vector2(((float)glyphRect.x + num4 + (float)glyphRect.width / 2f) / (float)atlasWidth, vector2.y);
		Vector2 vector6 = new Vector2(vector5.x, vector.y);
		Vector2 vector7 = new Vector2(((float)glyphRect.x + num4 + (float)glyphRect.width) / (float)atlasWidth, vector2.y);
		Vector2 vector8 = new Vector2(vector7.x, vector.y);
		uvs[index] = vector;
		uvs[1 + index] = vector2;
		uvs[2 + index] = vector3;
		uvs[3 + index] = vector4;
		uvs[4 + index] = new Vector2(vector3.x - vector3.x * 0.001f, vector.y);
		uvs[5 + index] = new Vector2(vector3.x - vector3.x * 0.001f, vector2.y);
		uvs[6 + index] = new Vector2(vector3.x + vector3.x * 0.001f, vector2.y);
		uvs[7 + index] = new Vector2(vector3.x + vector3.x * 0.001f, vector.y);
		uvs[8 + index] = vector6;
		uvs[9 + index] = vector5;
		uvs[10 + index] = vector7;
		uvs[11 + index] = vector8;
		float num5 = 0f;
		float x = (vertices[index + 2].x - start.x) / (end.x - start.x);
		float scale = Mathf.Abs(sdfScale);
		Vector2[] uvs2 = m_textInfo.meshInfo[materialIndex].uvs2;
		uvs2[index] = PackUV(0f, 0f, scale);
		uvs2[1 + index] = PackUV(0f, 1f, scale);
		uvs2[2 + index] = PackUV(x, 1f, scale);
		uvs2[3 + index] = PackUV(x, 0f, scale);
		num5 = (vertices[index + 4].x - start.x) / (end.x - start.x);
		x = (vertices[index + 6].x - start.x) / (end.x - start.x);
		uvs2[4 + index] = PackUV(num5, 0f, scale);
		uvs2[5 + index] = PackUV(num5, 1f, scale);
		uvs2[6 + index] = PackUV(x, 1f, scale);
		uvs2[7 + index] = PackUV(x, 0f, scale);
		num5 = (vertices[index + 8].x - start.x) / (end.x - start.x);
		uvs2[8 + index] = PackUV(num5, 0f, scale);
		uvs2[9 + index] = PackUV(num5, 1f, scale);
		uvs2[10 + index] = PackUV(1f, 1f, scale);
		uvs2[11 + index] = PackUV(1f, 0f, scale);
		underlineColor.a = ((m_fontColor32.a < underlineColor.a) ? m_fontColor32.a : underlineColor.a);
		Color32[] colors = m_textInfo.meshInfo[materialIndex].colors32;
		colors[index] = underlineColor;
		colors[1 + index] = underlineColor;
		colors[2 + index] = underlineColor;
		colors[3 + index] = underlineColor;
		colors[4 + index] = underlineColor;
		colors[5 + index] = underlineColor;
		colors[6 + index] = underlineColor;
		colors[7 + index] = underlineColor;
		colors[8 + index] = underlineColor;
		colors[9 + index] = underlineColor;
		colors[10 + index] = underlineColor;
		colors[11 + index] = underlineColor;
		index += 12;
	}

	protected virtual void DrawTextHighlight(Vector3 start, Vector3 end, ref int index, Color32 highlightColor)
	{
		if (m_Underline.character == null)
		{
			GetUnderlineSpecialCharacter(m_fontAsset);
			if (m_Underline.character == null)
			{
				if (!TMP_Settings.warningsDisabled)
				{
					UnityEngine.Debug.LogWarning("Unable to add highlight since the primary Font Asset doesn't contain the underline character.", this);
				}
				return;
			}
		}
		int materialIndex = m_Underline.materialIndex;
		int num = index + 4;
		if (num > m_textInfo.meshInfo[materialIndex].vertices.Length)
		{
			m_textInfo.meshInfo[materialIndex].ResizeMeshInfo(num / 4);
		}
		Vector3[] vertices = m_textInfo.meshInfo[materialIndex].vertices;
		vertices[index] = start;
		vertices[index + 1] = new Vector3(start.x, end.y, 0f);
		vertices[index + 2] = end;
		vertices[index + 3] = new Vector3(end.x, start.y, 0f);
		Vector2[] uvs = m_textInfo.meshInfo[materialIndex].uvs0;
		int atlasWidth = m_Underline.fontAsset.atlasWidth;
		int atlasHeight = m_Underline.fontAsset.atlasHeight;
		GlyphRect glyphRect = m_Underline.character.glyph.glyphRect;
		Vector2 vector = new Vector2(((float)glyphRect.x + (float)(glyphRect.width / 2)) / (float)atlasWidth, ((float)glyphRect.y + (float)glyphRect.height / 2f) / (float)atlasHeight);
		uvs[index] = vector;
		uvs[1 + index] = vector;
		uvs[2 + index] = vector;
		uvs[3 + index] = vector;
		Vector2[] uvs2 = m_textInfo.meshInfo[materialIndex].uvs2;
		Vector2 vector2 = new Vector2(0f, 1f);
		uvs2[index] = vector2;
		uvs2[1 + index] = vector2;
		uvs2[2 + index] = vector2;
		uvs2[3 + index] = vector2;
		highlightColor.a = ((m_fontColor32.a < highlightColor.a) ? m_fontColor32.a : highlightColor.a);
		Color32[] colors = m_textInfo.meshInfo[materialIndex].colors32;
		colors[index] = highlightColor;
		colors[1 + index] = highlightColor;
		colors[2 + index] = highlightColor;
		colors[3 + index] = highlightColor;
		index += 4;
	}

	protected void LoadDefaultSettings()
	{
		if (m_fontSize == -99f || m_isWaitingOnResourceLoad)
		{
			m_rectTransform = rectTransform;
			if (TMP_Settings.autoSizeTextContainer)
			{
				autoSizeTextContainer = true;
			}
			else if (GetType() == typeof(TextMeshPro))
			{
				if (m_rectTransform.sizeDelta == new Vector2(100f, 100f))
				{
					m_rectTransform.sizeDelta = TMP_Settings.defaultTextMeshProTextContainerSize;
				}
			}
			else if (m_rectTransform.sizeDelta == new Vector2(100f, 100f))
			{
				m_rectTransform.sizeDelta = TMP_Settings.defaultTextMeshProUITextContainerSize;
			}
			m_enableWordWrapping = TMP_Settings.enableWordWrapping;
			m_enableKerning = TMP_Settings.enableKerning;
			m_enableExtraPadding = TMP_Settings.enableExtraPadding;
			m_tintAllSprites = TMP_Settings.enableTintAllSprites;
			m_parseCtrlCharacters = TMP_Settings.enableParseEscapeCharacters;
			m_fontSize = (m_fontSizeBase = TMP_Settings.defaultFontSize);
			m_fontSizeMin = m_fontSize * TMP_Settings.defaultTextAutoSizingMinRatio;
			m_fontSizeMax = m_fontSize * TMP_Settings.defaultTextAutoSizingMaxRatio;
			m_isWaitingOnResourceLoad = false;
			raycastTarget = TMP_Settings.enableRaycastTarget;
			m_IsTextObjectScaleStatic = TMP_Settings.isTextObjectScaleStatic;
		}
		else if (m_textAlignment < (TextAlignmentOptions)255)
		{
			m_textAlignment = TMP_Compatibility.ConvertTextAlignmentEnumValues(m_textAlignment);
		}
		if (m_textAlignment != TextAlignmentOptions.Converted)
		{
			m_HorizontalAlignment = (HorizontalAlignmentOptions)(m_textAlignment & (TextAlignmentOptions)255);
			m_VerticalAlignment = (VerticalAlignmentOptions)(m_textAlignment & (TextAlignmentOptions)65280);
			m_textAlignment = TextAlignmentOptions.Converted;
		}
	}

	protected void GetSpecialCharacters(TMP_FontAsset fontAsset)
	{
		GetEllipsisSpecialCharacter(fontAsset);
		GetUnderlineSpecialCharacter(fontAsset);
	}

	protected void GetEllipsisSpecialCharacter(TMP_FontAsset fontAsset)
	{
		bool isAlternativeTypeface;
		TMP_Character tMP_Character = TMP_FontAssetUtilities.GetCharacterFromFontAsset(8230u, fontAsset, includeFallbacks: false, m_FontStyleInternal, m_FontWeightInternal, out isAlternativeTypeface);
		if (tMP_Character == null && fontAsset.m_FallbackFontAssetTable != null && fontAsset.m_FallbackFontAssetTable.Count > 0)
		{
			tMP_Character = TMP_FontAssetUtilities.GetCharacterFromFontAssets(8230u, fontAsset, fontAsset.m_FallbackFontAssetTable, includeFallbacks: true, m_FontStyleInternal, m_FontWeightInternal, out isAlternativeTypeface);
		}
		if (tMP_Character == null && TMP_Settings.fallbackFontAssets != null && TMP_Settings.fallbackFontAssets.Count > 0)
		{
			tMP_Character = TMP_FontAssetUtilities.GetCharacterFromFontAssets(8230u, fontAsset, TMP_Settings.fallbackFontAssets, includeFallbacks: true, m_FontStyleInternal, m_FontWeightInternal, out isAlternativeTypeface);
		}
		if (tMP_Character == null && TMP_Settings.defaultFontAsset != null)
		{
			tMP_Character = TMP_FontAssetUtilities.GetCharacterFromFontAsset(8230u, TMP_Settings.defaultFontAsset, includeFallbacks: true, m_FontStyleInternal, m_FontWeightInternal, out isAlternativeTypeface);
		}
		if (tMP_Character != null)
		{
			m_Ellipsis = new SpecialCharacter(tMP_Character, 0);
		}
	}

	protected void GetUnderlineSpecialCharacter(TMP_FontAsset fontAsset)
	{
		bool isAlternativeTypeface;
		TMP_Character characterFromFontAsset = TMP_FontAssetUtilities.GetCharacterFromFontAsset(95u, fontAsset, includeFallbacks: false, FontStyles.Normal, FontWeight.Regular, out isAlternativeTypeface);
		if (characterFromFontAsset != null)
		{
			m_Underline = new SpecialCharacter(characterFromFontAsset, 0);
		}
		else if (!TMP_Settings.warningsDisabled)
		{
			UnityEngine.Debug.LogWarning("The character used for Underline is not available in font asset [" + fontAsset.name + "].", this);
		}
	}

	protected void ReplaceTagWithCharacter(int[] chars, int insertionIndex, int tagLength, char c)
	{
		chars[insertionIndex] = c;
		for (int i = insertionIndex + tagLength; i < chars.Length; i++)
		{
			chars[i - 3] = chars[i];
		}
	}

	protected TMP_FontAsset GetFontAssetForWeight(int fontWeight)
	{
		bool num = (m_FontStyleInternal & FontStyles.Italic) == FontStyles.Italic || (m_fontStyle & FontStyles.Italic) == FontStyles.Italic;
		TMP_FontAsset tMP_FontAsset = null;
		int num2 = fontWeight / 100;
		if (num)
		{
			return m_currentFontAsset.fontWeightTable[num2].italicTypeface;
		}
		return m_currentFontAsset.fontWeightTable[num2].regularTypeface;
	}

	internal TMP_TextElement GetTextElement(uint unicode, TMP_FontAsset fontAsset, FontStyles fontStyle, FontWeight fontWeight, out bool isUsingAlternativeTypeface)
	{
		TMP_Character tMP_Character = TMP_FontAssetUtilities.GetCharacterFromFontAsset(unicode, fontAsset, includeFallbacks: false, fontStyle, fontWeight, out isUsingAlternativeTypeface);
		if (tMP_Character != null)
		{
			return tMP_Character;
		}
		if (fontAsset.m_FallbackFontAssetTable != null && fontAsset.m_FallbackFontAssetTable.Count > 0)
		{
			tMP_Character = TMP_FontAssetUtilities.GetCharacterFromFontAssets(unicode, fontAsset, fontAsset.m_FallbackFontAssetTable, includeFallbacks: true, fontStyle, fontWeight, out isUsingAlternativeTypeface);
		}
		if (tMP_Character != null)
		{
			return tMP_Character;
		}
		if (fontAsset.instanceID != m_fontAsset.instanceID)
		{
			tMP_Character = TMP_FontAssetUtilities.GetCharacterFromFontAsset(unicode, m_fontAsset, includeFallbacks: false, fontStyle, fontWeight, out isUsingAlternativeTypeface);
			if (tMP_Character != null)
			{
				m_currentMaterialIndex = 0;
				m_currentMaterial = m_materialReferences[0].material;
				return tMP_Character;
			}
			if (m_fontAsset.m_FallbackFontAssetTable != null && m_fontAsset.m_FallbackFontAssetTable.Count > 0)
			{
				tMP_Character = TMP_FontAssetUtilities.GetCharacterFromFontAssets(unicode, fontAsset, m_fontAsset.m_FallbackFontAssetTable, includeFallbacks: true, fontStyle, fontWeight, out isUsingAlternativeTypeface);
			}
			if (tMP_Character != null)
			{
				return tMP_Character;
			}
		}
		if (m_spriteAsset != null)
		{
			TMP_SpriteCharacter spriteCharacterFromSpriteAsset = TMP_FontAssetUtilities.GetSpriteCharacterFromSpriteAsset(unicode, m_spriteAsset, includeFallbacks: true);
			if (spriteCharacterFromSpriteAsset != null)
			{
				return spriteCharacterFromSpriteAsset;
			}
		}
		if (TMP_Settings.fallbackFontAssets != null && TMP_Settings.fallbackFontAssets.Count > 0)
		{
			tMP_Character = TMP_FontAssetUtilities.GetCharacterFromFontAssets(unicode, fontAsset, TMP_Settings.fallbackFontAssets, includeFallbacks: true, fontStyle, fontWeight, out isUsingAlternativeTypeface);
		}
		if (tMP_Character != null)
		{
			return tMP_Character;
		}
		if (TMP_Settings.defaultFontAsset != null)
		{
			tMP_Character = TMP_FontAssetUtilities.GetCharacterFromFontAsset(unicode, TMP_Settings.defaultFontAsset, includeFallbacks: true, fontStyle, fontWeight, out isUsingAlternativeTypeface);
		}
		if (tMP_Character != null)
		{
			return tMP_Character;
		}
		if (TMP_Settings.defaultSpriteAsset != null)
		{
			TMP_SpriteCharacter spriteCharacterFromSpriteAsset2 = TMP_FontAssetUtilities.GetSpriteCharacterFromSpriteAsset(unicode, TMP_Settings.defaultSpriteAsset, includeFallbacks: true);
			if (spriteCharacterFromSpriteAsset2 != null)
			{
				return spriteCharacterFromSpriteAsset2;
			}
		}
		return null;
	}

	protected virtual void SetActiveSubMeshes(bool state)
	{
	}

	protected virtual void DestroySubMeshObjects()
	{
	}

	public virtual void ClearMesh()
	{
	}

	public virtual void ClearMesh(bool uploadGeometry)
	{
	}

	public virtual string GetParsedText()
	{
		if (m_textInfo == null)
		{
			return string.Empty;
		}
		int characterCount = m_textInfo.characterCount;
		char[] array = new char[characterCount];
		for (int i = 0; i < characterCount && i < m_textInfo.characterInfo.Length; i++)
		{
			array[i] = m_textInfo.characterInfo[i].character;
		}
		return new string(array);
	}

	internal bool IsSelfOrLinkedAncestor(TMP_Text targetTextComponent)
	{
		if (targetTextComponent == null)
		{
			return true;
		}
		if (parentLinkedComponent != null && parentLinkedComponent.IsSelfOrLinkedAncestor(targetTextComponent))
		{
			return true;
		}
		if (GetInstanceID() == targetTextComponent.GetInstanceID())
		{
			return true;
		}
		return false;
	}

	internal void ReleaseLinkedTextComponent(TMP_Text targetTextComponent)
	{
		if (!(targetTextComponent == null))
		{
			TMP_Text tMP_Text = targetTextComponent.linkedTextComponent;
			if (tMP_Text != null)
			{
				ReleaseLinkedTextComponent(tMP_Text);
			}
			targetTextComponent.text = string.Empty;
			targetTextComponent.firstVisibleCharacter = 0;
			targetTextComponent.linkedTextComponent = null;
			targetTextComponent.parentLinkedComponent = null;
		}
	}

	protected Vector2 PackUV(float x, float y, float scale)
	{
		Vector2 result = default(Vector2);
		result.x = (int)(x * 511f);
		result.y = (int)(y * 511f);
		result.x = result.x * 4096f + result.y;
		result.y = scale;
		return result;
	}

	protected float PackUV(float x, float y)
	{
		double num = (int)(x * 511f);
		double num2 = (int)(y * 511f);
		return (float)(num * 4096.0 + num2);
	}

	internal virtual void InternalUpdate()
	{
	}

	protected int HexToInt(char hex)
	{
		return hex switch
		{
			'0' => 0, 
			'1' => 1, 
			'2' => 2, 
			'3' => 3, 
			'4' => 4, 
			'5' => 5, 
			'6' => 6, 
			'7' => 7, 
			'8' => 8, 
			'9' => 9, 
			'A' => 10, 
			'B' => 11, 
			'C' => 12, 
			'D' => 13, 
			'E' => 14, 
			'F' => 15, 
			'a' => 10, 
			'b' => 11, 
			'c' => 12, 
			'd' => 13, 
			'e' => 14, 
			'f' => 15, 
			_ => 15, 
		};
	}

	protected int GetUTF16(string text, int i)
	{
		return 0 + (HexToInt(text[i]) << 12) + (HexToInt(text[i + 1]) << 8) + (HexToInt(text[i + 2]) << 4) + HexToInt(text[i + 3]);
	}

	protected int GetUTF16(int[] text, int i)
	{
		return 0 + (HexToInt((char)text[i]) << 12) + (HexToInt((char)text[i + 1]) << 8) + (HexToInt((char)text[i + 2]) << 4) + HexToInt((char)text[i + 3]);
	}

	internal int GetUTF16(uint[] text, int i)
	{
		return 0 + (HexToInt((char)text[i]) << 12) + (HexToInt((char)text[i + 1]) << 8) + (HexToInt((char)text[i + 2]) << 4) + HexToInt((char)text[i + 3]);
	}

	protected int GetUTF16(StringBuilder text, int i)
	{
		return 0 + (HexToInt(text[i]) << 12) + (HexToInt(text[i + 1]) << 8) + (HexToInt(text[i + 2]) << 4) + HexToInt(text[i + 3]);
	}

	private int GetUTF16(TextBackingContainer text, int i)
	{
		return 0 + (HexToInt((char)text[i]) << 12) + (HexToInt((char)text[i + 1]) << 8) + (HexToInt((char)text[i + 2]) << 4) + HexToInt((char)text[i + 3]);
	}

	protected int GetUTF32(string text, int i)
	{
		return 0 + (HexToInt(text[i]) << 28) + (HexToInt(text[i + 1]) << 24) + (HexToInt(text[i + 2]) << 20) + (HexToInt(text[i + 3]) << 16) + (HexToInt(text[i + 4]) << 12) + (HexToInt(text[i + 5]) << 8) + (HexToInt(text[i + 6]) << 4) + HexToInt(text[i + 7]);
	}

	protected int GetUTF32(int[] text, int i)
	{
		return 0 + (HexToInt((char)text[i]) << 28) + (HexToInt((char)text[i + 1]) << 24) + (HexToInt((char)text[i + 2]) << 20) + (HexToInt((char)text[i + 3]) << 16) + (HexToInt((char)text[i + 4]) << 12) + (HexToInt((char)text[i + 5]) << 8) + (HexToInt((char)text[i + 6]) << 4) + HexToInt((char)text[i + 7]);
	}

	internal int GetUTF32(uint[] text, int i)
	{
		return 0 + (HexToInt((char)text[i]) << 28) + (HexToInt((char)text[i + 1]) << 24) + (HexToInt((char)text[i + 2]) << 20) + (HexToInt((char)text[i + 3]) << 16) + (HexToInt((char)text[i + 4]) << 12) + (HexToInt((char)text[i + 5]) << 8) + (HexToInt((char)text[i + 6]) << 4) + HexToInt((char)text[i + 7]);
	}

	protected int GetUTF32(StringBuilder text, int i)
	{
		return 0 + (HexToInt(text[i]) << 28) + (HexToInt(text[i + 1]) << 24) + (HexToInt(text[i + 2]) << 20) + (HexToInt(text[i + 3]) << 16) + (HexToInt(text[i + 4]) << 12) + (HexToInt(text[i + 5]) << 8) + (HexToInt(text[i + 6]) << 4) + HexToInt(text[i + 7]);
	}

	private int GetUTF32(TextBackingContainer text, int i)
	{
		return 0 + (HexToInt((char)text[i]) << 28) + (HexToInt((char)text[i + 1]) << 24) + (HexToInt((char)text[i + 2]) << 20) + (HexToInt((char)text[i + 3]) << 16) + (HexToInt((char)text[i + 4]) << 12) + (HexToInt((char)text[i + 5]) << 8) + (HexToInt((char)text[i + 6]) << 4) + HexToInt((char)text[i + 7]);
	}

	protected Color32 HexCharsToColor(char[] hexChars, int tagCount)
	{
		switch (tagCount)
		{
		case 4:
		{
			byte r8 = (byte)(HexToInt(hexChars[1]) * 16 + HexToInt(hexChars[1]));
			byte g8 = (byte)(HexToInt(hexChars[2]) * 16 + HexToInt(hexChars[2]));
			byte b8 = (byte)(HexToInt(hexChars[3]) * 16 + HexToInt(hexChars[3]));
			return new Color32(r8, g8, b8, byte.MaxValue);
		}
		case 5:
		{
			byte r7 = (byte)(HexToInt(hexChars[1]) * 16 + HexToInt(hexChars[1]));
			byte g7 = (byte)(HexToInt(hexChars[2]) * 16 + HexToInt(hexChars[2]));
			byte b7 = (byte)(HexToInt(hexChars[3]) * 16 + HexToInt(hexChars[3]));
			byte a4 = (byte)(HexToInt(hexChars[4]) * 16 + HexToInt(hexChars[4]));
			return new Color32(r7, g7, b7, a4);
		}
		case 7:
		{
			byte r6 = (byte)(HexToInt(hexChars[1]) * 16 + HexToInt(hexChars[2]));
			byte g6 = (byte)(HexToInt(hexChars[3]) * 16 + HexToInt(hexChars[4]));
			byte b6 = (byte)(HexToInt(hexChars[5]) * 16 + HexToInt(hexChars[6]));
			return new Color32(r6, g6, b6, byte.MaxValue);
		}
		case 9:
		{
			byte r5 = (byte)(HexToInt(hexChars[1]) * 16 + HexToInt(hexChars[2]));
			byte g5 = (byte)(HexToInt(hexChars[3]) * 16 + HexToInt(hexChars[4]));
			byte b5 = (byte)(HexToInt(hexChars[5]) * 16 + HexToInt(hexChars[6]));
			byte a3 = (byte)(HexToInt(hexChars[7]) * 16 + HexToInt(hexChars[8]));
			return new Color32(r5, g5, b5, a3);
		}
		case 10:
		{
			byte r4 = (byte)(HexToInt(hexChars[7]) * 16 + HexToInt(hexChars[7]));
			byte g4 = (byte)(HexToInt(hexChars[8]) * 16 + HexToInt(hexChars[8]));
			byte b4 = (byte)(HexToInt(hexChars[9]) * 16 + HexToInt(hexChars[9]));
			return new Color32(r4, g4, b4, byte.MaxValue);
		}
		case 11:
		{
			byte r3 = (byte)(HexToInt(hexChars[7]) * 16 + HexToInt(hexChars[7]));
			byte g3 = (byte)(HexToInt(hexChars[8]) * 16 + HexToInt(hexChars[8]));
			byte b3 = (byte)(HexToInt(hexChars[9]) * 16 + HexToInt(hexChars[9]));
			byte a2 = (byte)(HexToInt(hexChars[10]) * 16 + HexToInt(hexChars[10]));
			return new Color32(r3, g3, b3, a2);
		}
		case 13:
		{
			byte r2 = (byte)(HexToInt(hexChars[7]) * 16 + HexToInt(hexChars[8]));
			byte g2 = (byte)(HexToInt(hexChars[9]) * 16 + HexToInt(hexChars[10]));
			byte b2 = (byte)(HexToInt(hexChars[11]) * 16 + HexToInt(hexChars[12]));
			return new Color32(r2, g2, b2, byte.MaxValue);
		}
		case 15:
		{
			byte r = (byte)(HexToInt(hexChars[7]) * 16 + HexToInt(hexChars[8]));
			byte g = (byte)(HexToInt(hexChars[9]) * 16 + HexToInt(hexChars[10]));
			byte b = (byte)(HexToInt(hexChars[11]) * 16 + HexToInt(hexChars[12]));
			byte a = (byte)(HexToInt(hexChars[13]) * 16 + HexToInt(hexChars[14]));
			return new Color32(r, g, b, a);
		}
		default:
			return new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}
	}

	protected Color32 HexCharsToColor(char[] hexChars, int startIndex, int length)
	{
		switch (length)
		{
		case 7:
		{
			byte r2 = (byte)(HexToInt(hexChars[startIndex + 1]) * 16 + HexToInt(hexChars[startIndex + 2]));
			byte g2 = (byte)(HexToInt(hexChars[startIndex + 3]) * 16 + HexToInt(hexChars[startIndex + 4]));
			byte b2 = (byte)(HexToInt(hexChars[startIndex + 5]) * 16 + HexToInt(hexChars[startIndex + 6]));
			return new Color32(r2, g2, b2, byte.MaxValue);
		}
		case 9:
		{
			byte r = (byte)(HexToInt(hexChars[startIndex + 1]) * 16 + HexToInt(hexChars[startIndex + 2]));
			byte g = (byte)(HexToInt(hexChars[startIndex + 3]) * 16 + HexToInt(hexChars[startIndex + 4]));
			byte b = (byte)(HexToInt(hexChars[startIndex + 5]) * 16 + HexToInt(hexChars[startIndex + 6]));
			byte a = (byte)(HexToInt(hexChars[startIndex + 7]) * 16 + HexToInt(hexChars[startIndex + 8]));
			return new Color32(r, g, b, a);
		}
		default:
			return s_colorWhite;
		}
	}

	private int GetAttributeParameters(char[] chars, int startIndex, int length, ref float[] parameters)
	{
		int lastIndex = startIndex;
		int num = 0;
		while (lastIndex < startIndex + length)
		{
			parameters[num] = ConvertToFloat(chars, startIndex, length, out lastIndex);
			length -= lastIndex - startIndex + 1;
			startIndex = lastIndex + 1;
			num++;
		}
		return num;
	}

	protected float ConvertToFloat(char[] chars, int startIndex, int length)
	{
		int lastIndex;
		return ConvertToFloat(chars, startIndex, length, out lastIndex);
	}

	protected float ConvertToFloat(char[] chars, int startIndex, int length, out int lastIndex)
	{
		if (startIndex == 0)
		{
			lastIndex = 0;
			return -32768f;
		}
		int num = startIndex + length;
		bool flag = true;
		float num2 = 0f;
		int num3 = 1;
		if (chars[startIndex] == '+')
		{
			num3 = 1;
			startIndex++;
		}
		else if (chars[startIndex] == '-')
		{
			num3 = -1;
			startIndex++;
		}
		float num4 = 0f;
		for (int i = startIndex; i < num; i++)
		{
			uint num5 = chars[i];
			if (num5 < 48 || num5 > 57)
			{
				switch (num5)
				{
				case 46u:
					break;
				case 44u:
					if (i + 1 < num && chars[i + 1] == ' ')
					{
						lastIndex = i + 1;
					}
					else
					{
						lastIndex = i;
					}
					if (num4 > 32767f)
					{
						return -32768f;
					}
					return num4;
				default:
					continue;
				}
			}
			if (num5 == 46)
			{
				flag = false;
				num2 = 0.1f;
			}
			else if (flag)
			{
				num4 = num4 * 10f + (float)((num5 - 48) * num3);
			}
			else
			{
				num4 += (float)(num5 - 48) * num2 * (float)num3;
				num2 *= 0.1f;
			}
		}
		lastIndex = num;
		if (num4 > 32767f)
		{
			return -32768f;
		}
		return num4;
	}

	internal bool ValidateHtmlTag(UnicodeChar[] chars, int startIndex, out int endIndex)
	{
		int num = 0;
		byte b = 0;
		int num2 = 0;
		m_xmlAttribute[num2].nameHashCode = 0;
		m_xmlAttribute[num2].valueHashCode = 0;
		m_xmlAttribute[num2].valueStartIndex = 0;
		m_xmlAttribute[num2].valueLength = 0;
		TagValueType tagValueType = (m_xmlAttribute[num2].valueType = TagValueType.None);
		TagUnitType tagUnitType = (m_xmlAttribute[num2].unitType = TagUnitType.Pixels);
		m_xmlAttribute[1].nameHashCode = 0;
		m_xmlAttribute[2].nameHashCode = 0;
		m_xmlAttribute[3].nameHashCode = 0;
		m_xmlAttribute[4].nameHashCode = 0;
		endIndex = startIndex;
		bool flag = false;
		bool flag2 = false;
		for (int i = startIndex; i < chars.Length && chars[i].unicode != 0; i++)
		{
			if (num >= m_htmlTag.Length)
			{
				break;
			}
			if (chars[i].unicode == 60)
			{
				break;
			}
			int unicode = chars[i].unicode;
			if (unicode == 62)
			{
				flag2 = true;
				endIndex = i;
				m_htmlTag[num] = '\0';
				break;
			}
			m_htmlTag[num] = (char)unicode;
			num++;
			if (b == 1)
			{
				switch (tagValueType)
				{
				case TagValueType.None:
					switch (unicode)
					{
					case 43:
					case 45:
					case 46:
					case 48:
					case 49:
					case 50:
					case 51:
					case 52:
					case 53:
					case 54:
					case 55:
					case 56:
					case 57:
						tagUnitType = TagUnitType.Pixels;
						tagValueType = (m_xmlAttribute[num2].valueType = TagValueType.NumericalValue);
						m_xmlAttribute[num2].valueStartIndex = num - 1;
						m_xmlAttribute[num2].valueLength++;
						break;
					default:
						switch (unicode)
						{
						case 35:
							tagUnitType = TagUnitType.Pixels;
							tagValueType = (m_xmlAttribute[num2].valueType = TagValueType.ColorValue);
							m_xmlAttribute[num2].valueStartIndex = num - 1;
							m_xmlAttribute[num2].valueLength++;
							break;
						case 34:
							tagUnitType = TagUnitType.Pixels;
							tagValueType = (m_xmlAttribute[num2].valueType = TagValueType.StringValue);
							m_xmlAttribute[num2].valueStartIndex = num;
							break;
						default:
							tagUnitType = TagUnitType.Pixels;
							tagValueType = (m_xmlAttribute[num2].valueType = TagValueType.StringValue);
							m_xmlAttribute[num2].valueStartIndex = num - 1;
							m_xmlAttribute[num2].valueHashCode = ((m_xmlAttribute[num2].valueHashCode << 5) + m_xmlAttribute[num2].valueHashCode) ^ unicode;
							m_xmlAttribute[num2].valueLength++;
							break;
						}
						break;
					}
					break;
				case TagValueType.NumericalValue:
					if (unicode == 112 || unicode == 101 || unicode == 37 || unicode == 32)
					{
						b = 2;
						tagValueType = TagValueType.None;
						tagUnitType = unicode switch
						{
							101 => m_xmlAttribute[num2].unitType = TagUnitType.FontUnits, 
							37 => m_xmlAttribute[num2].unitType = TagUnitType.Percentage, 
							_ => m_xmlAttribute[num2].unitType = TagUnitType.Pixels, 
						};
						num2++;
						m_xmlAttribute[num2].nameHashCode = 0;
						m_xmlAttribute[num2].valueHashCode = 0;
						m_xmlAttribute[num2].valueType = TagValueType.None;
						m_xmlAttribute[num2].unitType = TagUnitType.Pixels;
						m_xmlAttribute[num2].valueStartIndex = 0;
						m_xmlAttribute[num2].valueLength = 0;
					}
					else if (b != 2)
					{
						m_xmlAttribute[num2].valueLength++;
					}
					break;
				case TagValueType.ColorValue:
					if (unicode != 32)
					{
						m_xmlAttribute[num2].valueLength++;
						break;
					}
					b = 2;
					tagValueType = TagValueType.None;
					tagUnitType = TagUnitType.Pixels;
					num2++;
					m_xmlAttribute[num2].nameHashCode = 0;
					m_xmlAttribute[num2].valueType = TagValueType.None;
					m_xmlAttribute[num2].unitType = TagUnitType.Pixels;
					m_xmlAttribute[num2].valueHashCode = 0;
					m_xmlAttribute[num2].valueStartIndex = 0;
					m_xmlAttribute[num2].valueLength = 0;
					break;
				case TagValueType.StringValue:
					if (unicode != 34)
					{
						m_xmlAttribute[num2].valueHashCode = ((m_xmlAttribute[num2].valueHashCode << 5) + m_xmlAttribute[num2].valueHashCode) ^ unicode;
						m_xmlAttribute[num2].valueLength++;
						break;
					}
					b = 2;
					tagValueType = TagValueType.None;
					tagUnitType = TagUnitType.Pixels;
					num2++;
					m_xmlAttribute[num2].nameHashCode = 0;
					m_xmlAttribute[num2].valueType = TagValueType.None;
					m_xmlAttribute[num2].unitType = TagUnitType.Pixels;
					m_xmlAttribute[num2].valueHashCode = 0;
					m_xmlAttribute[num2].valueStartIndex = 0;
					m_xmlAttribute[num2].valueLength = 0;
					break;
				}
			}
			if (unicode == 61)
			{
				b = 1;
			}
			if (b == 0 && unicode == 32)
			{
				if (flag)
				{
					return false;
				}
				flag = true;
				b = 2;
				tagValueType = TagValueType.None;
				tagUnitType = TagUnitType.Pixels;
				num2++;
				m_xmlAttribute[num2].nameHashCode = 0;
				m_xmlAttribute[num2].valueType = TagValueType.None;
				m_xmlAttribute[num2].unitType = TagUnitType.Pixels;
				m_xmlAttribute[num2].valueHashCode = 0;
				m_xmlAttribute[num2].valueStartIndex = 0;
				m_xmlAttribute[num2].valueLength = 0;
			}
			if (b == 0)
			{
				m_xmlAttribute[num2].nameHashCode = (m_xmlAttribute[num2].nameHashCode << 3) - m_xmlAttribute[num2].nameHashCode + unicode;
			}
			if (b == 2 && unicode == 32)
			{
				b = 0;
			}
		}
		if (!flag2)
		{
			return false;
		}
		if (tag_NoParsing && m_xmlAttribute[0].nameHashCode != 53822163 && m_xmlAttribute[0].nameHashCode != 49429939)
		{
			return false;
		}
		if (m_xmlAttribute[0].nameHashCode == 53822163 || m_xmlAttribute[0].nameHashCode == 49429939)
		{
			tag_NoParsing = false;
			return true;
		}
		if (m_htmlTag[0] == '#' && num == 4)
		{
			m_htmlColor = HexCharsToColor(m_htmlTag, num);
			m_colorStack.Add(m_htmlColor);
			return true;
		}
		if (m_htmlTag[0] == '#' && num == 5)
		{
			m_htmlColor = HexCharsToColor(m_htmlTag, num);
			m_colorStack.Add(m_htmlColor);
			return true;
		}
		if (m_htmlTag[0] == '#' && num == 7)
		{
			m_htmlColor = HexCharsToColor(m_htmlTag, num);
			m_colorStack.Add(m_htmlColor);
			return true;
		}
		if (m_htmlTag[0] == '#' && num == 9)
		{
			m_htmlColor = HexCharsToColor(m_htmlTag, num);
			m_colorStack.Add(m_htmlColor);
			return true;
		}
		float num3 = 0f;
		Material currentMaterial;
		switch (m_xmlAttribute[0].nameHashCode)
		{
		case 66:
		case 98:
			m_FontStyleInternal |= FontStyles.Bold;
			m_fontStyleStack.Add(FontStyles.Bold);
			m_FontWeightInternal = FontWeight.Bold;
			return true;
		case 395:
		case 427:
			if ((m_fontStyle & FontStyles.Bold) != FontStyles.Bold && m_fontStyleStack.Remove(FontStyles.Bold) == 0)
			{
				m_FontStyleInternal &= ~FontStyles.Bold;
				m_FontWeightInternal = m_FontWeightStack.Peek();
			}
			return true;
		case 73:
		case 105:
			m_FontStyleInternal |= FontStyles.Italic;
			m_fontStyleStack.Add(FontStyles.Italic);
			if (m_xmlAttribute[1].nameHashCode == 276531 || m_xmlAttribute[1].nameHashCode == 186899)
			{
				m_ItalicAngle = (int)ConvertToFloat(m_htmlTag, m_xmlAttribute[1].valueStartIndex, m_xmlAttribute[1].valueLength);
				if (m_ItalicAngle < -180 || m_ItalicAngle > 180)
				{
					return false;
				}
			}
			else
			{
				m_ItalicAngle = m_currentFontAsset.italicStyle;
			}
			m_ItalicAngleStack.Add(m_ItalicAngle);
			return true;
		case 402:
		case 434:
			if ((m_fontStyle & FontStyles.Italic) != FontStyles.Italic)
			{
				m_ItalicAngle = m_ItalicAngleStack.Remove();
				if (m_fontStyleStack.Remove(FontStyles.Italic) == 0)
				{
					m_FontStyleInternal &= ~FontStyles.Italic;
				}
			}
			return true;
		case 83:
		case 115:
			m_FontStyleInternal |= FontStyles.Strikethrough;
			m_fontStyleStack.Add(FontStyles.Strikethrough);
			if (m_xmlAttribute[1].nameHashCode == 281955 || m_xmlAttribute[1].nameHashCode == 192323)
			{
				m_strikethroughColor = HexCharsToColor(m_htmlTag, m_xmlAttribute[1].valueStartIndex, m_xmlAttribute[1].valueLength);
				m_strikethroughColor.a = ((m_htmlColor.a < m_strikethroughColor.a) ? m_htmlColor.a : m_strikethroughColor.a);
			}
			else
			{
				m_strikethroughColor = m_htmlColor;
			}
			m_strikethroughColorStack.Add(m_strikethroughColor);
			return true;
		case 412:
		case 444:
			if ((m_fontStyle & FontStyles.Strikethrough) != FontStyles.Strikethrough && m_fontStyleStack.Remove(FontStyles.Strikethrough) == 0)
			{
				m_FontStyleInternal &= ~FontStyles.Strikethrough;
			}
			m_strikethroughColor = m_strikethroughColorStack.Remove();
			return true;
		case 85:
		case 117:
			m_FontStyleInternal |= FontStyles.Underline;
			m_fontStyleStack.Add(FontStyles.Underline);
			if (m_xmlAttribute[1].nameHashCode == 281955 || m_xmlAttribute[1].nameHashCode == 192323)
			{
				m_underlineColor = HexCharsToColor(m_htmlTag, m_xmlAttribute[1].valueStartIndex, m_xmlAttribute[1].valueLength);
				m_underlineColor.a = ((m_htmlColor.a < m_underlineColor.a) ? m_htmlColor.a : m_underlineColor.a);
			}
			else
			{
				m_underlineColor = m_htmlColor;
			}
			m_underlineColorStack.Add(m_underlineColor);
			return true;
		case 414:
		case 446:
			if ((m_fontStyle & FontStyles.Underline) != FontStyles.Underline)
			{
				m_underlineColor = m_underlineColorStack.Remove();
				if (m_fontStyleStack.Remove(FontStyles.Underline) == 0)
				{
					m_FontStyleInternal &= ~FontStyles.Underline;
				}
			}
			m_underlineColor = m_underlineColorStack.Remove();
			return true;
		case 30245:
		case 43045:
		{
			m_FontStyleInternal |= FontStyles.Highlight;
			m_fontStyleStack.Add(FontStyles.Highlight);
			Color32 color = new Color32(byte.MaxValue, byte.MaxValue, 0, 64);
			TMP_Offset padding = TMP_Offset.zero;
			for (int j = 0; j < m_xmlAttribute.Length && m_xmlAttribute[j].nameHashCode != 0; j++)
			{
				switch (m_xmlAttribute[j].nameHashCode)
				{
				case 30245:
				case 43045:
					if (m_xmlAttribute[j].valueType == TagValueType.ColorValue)
					{
						color = HexCharsToColor(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
					}
					break;
				case 281955:
					color = HexCharsToColor(m_htmlTag, m_xmlAttribute[j].valueStartIndex, m_xmlAttribute[j].valueLength);
					break;
				case 15087385:
					if (GetAttributeParameters(m_htmlTag, m_xmlAttribute[j].valueStartIndex, m_xmlAttribute[j].valueLength, ref m_attributeParameterValues) != 4)
					{
						return false;
					}
					padding = new TMP_Offset(m_attributeParameterValues[0], m_attributeParameterValues[1], m_attributeParameterValues[2], m_attributeParameterValues[3]);
					padding *= m_fontSize * 0.01f * (m_isOrthographic ? 1f : 0.1f);
					break;
				}
			}
			color.a = ((m_htmlColor.a < color.a) ? m_htmlColor.a : color.a);
			HighlightState item = new HighlightState(color, padding);
			m_HighlightStateStack.Push(item);
			return true;
		}
		case 143092:
		case 155892:
			if ((m_fontStyle & FontStyles.Highlight) != FontStyles.Highlight)
			{
				m_HighlightStateStack.Remove();
				if (m_fontStyleStack.Remove(FontStyles.Highlight) == 0)
				{
					m_FontStyleInternal &= ~FontStyles.Highlight;
				}
			}
			return true;
		case 4728:
		case 6552:
		{
			m_fontScaleMultiplier *= ((m_currentFontAsset.faceInfo.subscriptSize > 0f) ? m_currentFontAsset.faceInfo.subscriptSize : 1f);
			m_baselineOffsetStack.Push(m_baselineOffset);
			float num4 = m_currentFontSize / (float)m_currentFontAsset.faceInfo.pointSize * m_currentFontAsset.faceInfo.scale * (m_isOrthographic ? 1f : 0.1f);
			m_baselineOffset += m_currentFontAsset.faceInfo.subscriptOffset * num4 * m_fontScaleMultiplier;
			m_fontStyleStack.Add(FontStyles.Subscript);
			m_FontStyleInternal |= FontStyles.Subscript;
			return true;
		}
		case 20849:
		case 22673:
			if ((m_FontStyleInternal & FontStyles.Subscript) == FontStyles.Subscript)
			{
				if (m_fontScaleMultiplier < 1f)
				{
					m_baselineOffset = m_baselineOffsetStack.Pop();
					m_fontScaleMultiplier /= ((m_currentFontAsset.faceInfo.subscriptSize > 0f) ? m_currentFontAsset.faceInfo.subscriptSize : 1f);
				}
				if (m_fontStyleStack.Remove(FontStyles.Subscript) == 0)
				{
					m_FontStyleInternal &= ~FontStyles.Subscript;
				}
			}
			return true;
		case 4742:
		case 6566:
		{
			m_fontScaleMultiplier *= ((m_currentFontAsset.faceInfo.superscriptSize > 0f) ? m_currentFontAsset.faceInfo.superscriptSize : 1f);
			m_baselineOffsetStack.Push(m_baselineOffset);
			float num4 = m_currentFontSize / (float)m_currentFontAsset.faceInfo.pointSize * m_currentFontAsset.faceInfo.scale * (m_isOrthographic ? 1f : 0.1f);
			m_baselineOffset += m_currentFontAsset.faceInfo.superscriptOffset * num4 * m_fontScaleMultiplier;
			m_fontStyleStack.Add(FontStyles.Superscript);
			m_FontStyleInternal |= FontStyles.Superscript;
			return true;
		}
		case 20863:
		case 22687:
			if ((m_FontStyleInternal & FontStyles.Superscript) == FontStyles.Superscript)
			{
				if (m_fontScaleMultiplier < 1f)
				{
					m_baselineOffset = m_baselineOffsetStack.Pop();
					m_fontScaleMultiplier /= ((m_currentFontAsset.faceInfo.superscriptSize > 0f) ? m_currentFontAsset.faceInfo.superscriptSize : 1f);
				}
				if (m_fontStyleStack.Remove(FontStyles.Superscript) == 0)
				{
					m_FontStyleInternal &= ~FontStyles.Superscript;
				}
			}
			return true;
		case -330774850:
		case 2012149182:
			num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
			if (num3 == -32768f)
			{
				return false;
			}
			switch ((int)num3)
			{
			case 100:
				m_FontWeightInternal = FontWeight.Thin;
				break;
			case 200:
				m_FontWeightInternal = FontWeight.ExtraLight;
				break;
			case 300:
				m_FontWeightInternal = FontWeight.Light;
				break;
			case 400:
				m_FontWeightInternal = FontWeight.Regular;
				break;
			case 500:
				m_FontWeightInternal = FontWeight.Medium;
				break;
			case 600:
				m_FontWeightInternal = FontWeight.SemiBold;
				break;
			case 700:
				m_FontWeightInternal = FontWeight.Bold;
				break;
			case 800:
				m_FontWeightInternal = FontWeight.Heavy;
				break;
			case 900:
				m_FontWeightInternal = FontWeight.Black;
				break;
			}
			m_FontWeightStack.Add(m_FontWeightInternal);
			return true;
		case -1885698441:
		case 457225591:
			m_FontWeightStack.Remove();
			if (m_FontStyleInternal == FontStyles.Bold)
			{
				m_FontWeightInternal = FontWeight.Bold;
			}
			else
			{
				m_FontWeightInternal = m_FontWeightStack.Peek();
			}
			return true;
		case 4556:
		case 6380:
			num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
			if (num3 == -32768f)
			{
				return false;
			}
			switch (tagUnitType)
			{
			case TagUnitType.Pixels:
				m_xAdvance = num3 * (m_isOrthographic ? 1f : 0.1f);
				return true;
			case TagUnitType.FontUnits:
				m_xAdvance = num3 * m_currentFontSize * (m_isOrthographic ? 1f : 0.1f);
				return true;
			case TagUnitType.Percentage:
				m_xAdvance = m_marginWidth * num3 / 100f;
				return true;
			default:
				return false;
			}
		case 20677:
		case 22501:
			m_isIgnoringAlignment = false;
			return true;
		case 11642281:
		case 16034505:
			num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
			if (num3 == -32768f)
			{
				return false;
			}
			switch (tagUnitType)
			{
			case TagUnitType.Pixels:
				m_baselineOffset = num3 * (m_isOrthographic ? 1f : 0.1f);
				return true;
			case TagUnitType.FontUnits:
				m_baselineOffset = num3 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
				return true;
			case TagUnitType.Percentage:
				return false;
			default:
				return false;
			}
		case 50348802:
		case 54741026:
			m_baselineOffset = 0f;
			return true;
		case 31191:
		case 43991:
			if (m_overflowMode == TextOverflowModes.Page)
			{
				m_xAdvance = 0f + tag_LineIndent + tag_Indent;
				m_lineOffset = 0f;
				m_pageNumber++;
				m_isNewPage = true;
			}
			return true;
		case 31169:
		case 43969:
			m_isNonBreakingSpace = true;
			return true;
		case 144016:
		case 156816:
			m_isNonBreakingSpace = false;
			return true;
		case 32745:
		case 45545:
			num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
			if (num3 == -32768f)
			{
				return false;
			}
			switch (tagUnitType)
			{
			case TagUnitType.Pixels:
				if (m_htmlTag[5] == '+')
				{
					m_currentFontSize = m_fontSize + num3;
					m_sizeStack.Add(m_currentFontSize);
					return true;
				}
				if (m_htmlTag[5] == '-')
				{
					m_currentFontSize = m_fontSize + num3;
					m_sizeStack.Add(m_currentFontSize);
					return true;
				}
				m_currentFontSize = num3;
				m_sizeStack.Add(m_currentFontSize);
				return true;
			case TagUnitType.FontUnits:
				m_currentFontSize = m_fontSize * num3;
				m_sizeStack.Add(m_currentFontSize);
				return true;
			case TagUnitType.Percentage:
				m_currentFontSize = m_fontSize * num3 / 100f;
				m_sizeStack.Add(m_currentFontSize);
				return true;
			default:
				return false;
			}
		case 145592:
		case 158392:
			m_currentFontSize = m_sizeStack.Remove();
			return true;
		case 28511:
		case 41311:
		{
			int valueHashCode2 = m_xmlAttribute[0].valueHashCode;
			int nameHashCode = m_xmlAttribute[1].nameHashCode;
			int valueHashCode3 = m_xmlAttribute[1].valueHashCode;
			if (valueHashCode2 == 764638571 || valueHashCode2 == 523367755)
			{
				m_currentFontAsset = m_materialReferences[0].fontAsset;
				m_currentMaterial = m_materialReferences[0].material;
				m_currentMaterialIndex = 0;
				m_materialReferenceStack.Add(m_materialReferences[0]);
				return true;
			}
			MaterialReferenceManager.TryGetFontAsset(valueHashCode2, out var fontAsset);
			if (fontAsset == null)
			{
				fontAsset = TMP_Text.OnFontAssetRequest?.Invoke(valueHashCode2, new string(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength));
				if (fontAsset == null)
				{
					fontAsset = Resources.Load<TMP_FontAsset>(TMP_Settings.defaultFontAssetPath + new string(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength));
				}
				if (fontAsset == null)
				{
					return false;
				}
				MaterialReferenceManager.AddFontAsset(fontAsset);
			}
			if (nameHashCode == 0 && valueHashCode3 == 0)
			{
				m_currentMaterial = fontAsset.material;
				m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, fontAsset, ref m_materialReferences, m_materialReferenceIndexLookup);
				m_materialReferenceStack.Add(m_materialReferences[m_currentMaterialIndex]);
			}
			else
			{
				if (nameHashCode != 103415287 && nameHashCode != 72669687)
				{
					return false;
				}
				if (MaterialReferenceManager.TryGetMaterial(valueHashCode3, out currentMaterial))
				{
					m_currentMaterial = currentMaterial;
					m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, fontAsset, ref m_materialReferences, m_materialReferenceIndexLookup);
					m_materialReferenceStack.Add(m_materialReferences[m_currentMaterialIndex]);
				}
				else
				{
					currentMaterial = Resources.Load<Material>(TMP_Settings.defaultFontAssetPath + new string(m_htmlTag, m_xmlAttribute[1].valueStartIndex, m_xmlAttribute[1].valueLength));
					if (currentMaterial == null)
					{
						return false;
					}
					MaterialReferenceManager.AddFontMaterial(valueHashCode3, currentMaterial);
					m_currentMaterial = currentMaterial;
					m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, fontAsset, ref m_materialReferences, m_materialReferenceIndexLookup);
					m_materialReferenceStack.Add(m_materialReferences[m_currentMaterialIndex]);
				}
			}
			m_currentFontAsset = fontAsset;
			return true;
		}
		case 141358:
		case 154158:
		{
			MaterialReference materialReference = m_materialReferenceStack.Remove();
			m_currentFontAsset = materialReference.fontAsset;
			m_currentMaterial = materialReference.material;
			m_currentMaterialIndex = materialReference.index;
			return true;
		}
		case 72669687:
		case 103415287:
		{
			int valueHashCode3 = m_xmlAttribute[0].valueHashCode;
			if (valueHashCode3 == 764638571 || valueHashCode3 == 523367755)
			{
				m_currentMaterial = m_materialReferences[0].material;
				m_currentMaterialIndex = 0;
				m_materialReferenceStack.Add(m_materialReferences[0]);
				return true;
			}
			if (MaterialReferenceManager.TryGetMaterial(valueHashCode3, out currentMaterial))
			{
				m_currentMaterial = currentMaterial;
				m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, m_currentFontAsset, ref m_materialReferences, m_materialReferenceIndexLookup);
				m_materialReferenceStack.Add(m_materialReferences[m_currentMaterialIndex]);
			}
			else
			{
				currentMaterial = Resources.Load<Material>(TMP_Settings.defaultFontAssetPath + new string(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength));
				if (currentMaterial == null)
				{
					return false;
				}
				MaterialReferenceManager.AddFontMaterial(valueHashCode3, currentMaterial);
				m_currentMaterial = currentMaterial;
				m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, m_currentFontAsset, ref m_materialReferences, m_materialReferenceIndexLookup);
				m_materialReferenceStack.Add(m_materialReferences[m_currentMaterialIndex]);
			}
			return true;
		}
		case 343615334:
		case 374360934:
		{
			MaterialReference materialReference2 = m_materialReferenceStack.Remove();
			m_currentMaterial = materialReference2.material;
			m_currentMaterialIndex = materialReference2.index;
			return true;
		}
		case 230446:
		case 320078:
			num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
			if (num3 == -32768f)
			{
				return false;
			}
			switch (tagUnitType)
			{
			case TagUnitType.Pixels:
				m_xAdvance += num3 * (m_isOrthographic ? 1f : 0.1f);
				return true;
			case TagUnitType.FontUnits:
				m_xAdvance += num3 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
				return true;
			case TagUnitType.Percentage:
				return false;
			default:
				return false;
			}
		case 186622:
		case 276254:
			if (m_xmlAttribute[0].valueLength != 3)
			{
				return false;
			}
			m_htmlColor.a = (byte)(HexToInt(m_htmlTag[7]) * 16 + HexToInt(m_htmlTag[8]));
			return true;
		case 1750458:
			return false;
		case 426:
			return true;
		case 30266:
		case 43066:
			if (m_isParsingText && !m_isCalculatingPreferredValues)
			{
				int linkCount = m_textInfo.linkCount;
				if (linkCount + 1 > m_textInfo.linkInfo.Length)
				{
					TMP_TextInfo.Resize(ref m_textInfo.linkInfo, linkCount + 1);
				}
				m_textInfo.linkInfo[linkCount].textComponent = this;
				m_textInfo.linkInfo[linkCount].hashCode = m_xmlAttribute[0].valueHashCode;
				m_textInfo.linkInfo[linkCount].linkTextfirstCharacterIndex = m_characterCount;
				m_textInfo.linkInfo[linkCount].linkIdFirstCharacterIndex = startIndex + m_xmlAttribute[0].valueStartIndex;
				m_textInfo.linkInfo[linkCount].linkIdLength = m_xmlAttribute[0].valueLength;
				m_textInfo.linkInfo[linkCount].SetLinkID(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
			}
			return true;
		case 143113:
		case 155913:
			if (m_isParsingText && !m_isCalculatingPreferredValues && m_textInfo.linkCount < m_textInfo.linkInfo.Length)
			{
				m_textInfo.linkInfo[m_textInfo.linkCount].linkTextLength = m_characterCount - m_textInfo.linkInfo[m_textInfo.linkCount].linkTextfirstCharacterIndex;
				m_textInfo.linkCount++;
			}
			return true;
		case 186285:
		case 275917:
			switch (m_xmlAttribute[0].valueHashCode)
			{
			case 3774683:
				m_lineJustification = HorizontalAlignmentOptions.Left;
				m_lineJustificationStack.Add(m_lineJustification);
				return true;
			case 136703040:
				m_lineJustification = HorizontalAlignmentOptions.Right;
				m_lineJustificationStack.Add(m_lineJustification);
				return true;
			case -458210101:
				m_lineJustification = HorizontalAlignmentOptions.Center;
				m_lineJustificationStack.Add(m_lineJustification);
				return true;
			case -523808257:
				m_lineJustification = HorizontalAlignmentOptions.Justified;
				m_lineJustificationStack.Add(m_lineJustification);
				return true;
			case 122383428:
				m_lineJustification = HorizontalAlignmentOptions.Flush;
				m_lineJustificationStack.Add(m_lineJustification);
				return true;
			default:
				return false;
			}
		case 976214:
		case 1065846:
			m_lineJustification = m_lineJustificationStack.Remove();
			return true;
		case 237918:
		case 327550:
			num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
			if (num3 == -32768f)
			{
				return false;
			}
			switch (tagUnitType)
			{
			case TagUnitType.Pixels:
				m_width = num3 * (m_isOrthographic ? 1f : 0.1f);
				break;
			case TagUnitType.FontUnits:
				return false;
			case TagUnitType.Percentage:
				m_width = m_marginWidth * num3 / 100f;
				break;
			}
			return true;
		case 1027847:
		case 1117479:
			m_width = -1f;
			return true;
		case 192323:
		case 281955:
			if (m_htmlTag[6] == '#' && num == 10)
			{
				m_htmlColor = HexCharsToColor(m_htmlTag, num);
				m_colorStack.Add(m_htmlColor);
				return true;
			}
			if (m_htmlTag[6] == '#' && num == 11)
			{
				m_htmlColor = HexCharsToColor(m_htmlTag, num);
				m_colorStack.Add(m_htmlColor);
				return true;
			}
			if (m_htmlTag[6] == '#' && num == 13)
			{
				m_htmlColor = HexCharsToColor(m_htmlTag, num);
				m_colorStack.Add(m_htmlColor);
				return true;
			}
			if (m_htmlTag[6] == '#' && num == 15)
			{
				m_htmlColor = HexCharsToColor(m_htmlTag, num);
				m_colorStack.Add(m_htmlColor);
				return true;
			}
			switch (m_xmlAttribute[0].valueHashCode)
			{
			case 125395:
				m_htmlColor = Color.red;
				m_colorStack.Add(m_htmlColor);
				return true;
			case -992792864:
				m_htmlColor = new Color32(173, 216, 230, byte.MaxValue);
				m_colorStack.Add(m_htmlColor);
				return true;
			case 3573310:
				m_htmlColor = Color.blue;
				m_colorStack.Add(m_htmlColor);
				return true;
			case 3680713:
				m_htmlColor = new Color32(128, 128, 128, byte.MaxValue);
				m_colorStack.Add(m_htmlColor);
				return true;
			case 117905991:
				m_htmlColor = Color.black;
				m_colorStack.Add(m_htmlColor);
				return true;
			case 121463835:
				m_htmlColor = Color.green;
				m_colorStack.Add(m_htmlColor);
				return true;
			case 140357351:
				m_htmlColor = Color.white;
				m_colorStack.Add(m_htmlColor);
				return true;
			case 26556144:
				m_htmlColor = new Color32(byte.MaxValue, 128, 0, byte.MaxValue);
				m_colorStack.Add(m_htmlColor);
				return true;
			case -36881330:
				m_htmlColor = new Color32(160, 32, 240, byte.MaxValue);
				m_colorStack.Add(m_htmlColor);
				return true;
			case 554054276:
				m_htmlColor = Color.yellow;
				m_colorStack.Add(m_htmlColor);
				return true;
			default:
				return false;
			}
		case 69403544:
		case 100149144:
		{
			int valueHashCode5 = m_xmlAttribute[0].valueHashCode;
			if (MaterialReferenceManager.TryGetColorGradientPreset(valueHashCode5, out var gradientPreset))
			{
				m_colorGradientPreset = gradientPreset;
			}
			else
			{
				if (gradientPreset == null)
				{
					gradientPreset = Resources.Load<TMP_ColorGradient>(TMP_Settings.defaultColorGradientPresetsPath + new string(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength));
				}
				if (gradientPreset == null)
				{
					return false;
				}
				MaterialReferenceManager.AddColorGradientPreset(valueHashCode5, gradientPreset);
				m_colorGradientPreset = gradientPreset;
			}
			m_colorGradientPresetIsTinted = false;
			for (int m = 1; m < m_xmlAttribute.Length && m_xmlAttribute[m].nameHashCode != 0; m++)
			{
				int nameHashCode3 = m_xmlAttribute[m].nameHashCode;
				if (nameHashCode3 == 33019 || nameHashCode3 == 45819)
				{
					m_colorGradientPresetIsTinted = ConvertToFloat(m_htmlTag, m_xmlAttribute[m].valueStartIndex, m_xmlAttribute[m].valueLength) != 0f;
				}
			}
			m_colorGradientStack.Add(m_colorGradientPreset);
			return true;
		}
		case 340349191:
		case 371094791:
			m_colorGradientPreset = m_colorGradientStack.Remove();
			return true;
		case 1356515:
		case 1983971:
			num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
			if (num3 == -32768f)
			{
				return false;
			}
			switch (tagUnitType)
			{
			case TagUnitType.Pixels:
				m_cSpacing = num3 * (m_isOrthographic ? 1f : 0.1f);
				break;
			case TagUnitType.FontUnits:
				m_cSpacing = num3 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
				break;
			case TagUnitType.Percentage:
				return false;
			}
			return true;
		case 6886018:
		case 7513474:
			if (!m_isParsingText)
			{
				return true;
			}
			if (m_characterCount > 0)
			{
				m_xAdvance -= m_cSpacing;
				m_textInfo.characterInfo[m_characterCount - 1].xAdvance = m_xAdvance;
			}
			m_cSpacing = 0f;
			return true;
		case 1524585:
		case 2152041:
			num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
			if (num3 == -32768f)
			{
				return false;
			}
			switch (tagUnitType)
			{
			case TagUnitType.Pixels:
				m_monoSpacing = num3 * (m_isOrthographic ? 1f : 0.1f);
				break;
			case TagUnitType.FontUnits:
				m_monoSpacing = num3 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
				break;
			case TagUnitType.Percentage:
				return false;
			}
			return true;
		case 7054088:
		case 7681544:
			m_monoSpacing = 0f;
			return true;
		case 280416:
			return false;
		case 982252:
		case 1071884:
			m_htmlColor = m_colorStack.Remove();
			return true;
		case 1441524:
		case 2068980:
			num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
			if (num3 == -32768f)
			{
				return false;
			}
			switch (tagUnitType)
			{
			case TagUnitType.Pixels:
				tag_Indent = num3 * (m_isOrthographic ? 1f : 0.1f);
				break;
			case TagUnitType.FontUnits:
				tag_Indent = num3 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
				break;
			case TagUnitType.Percentage:
				tag_Indent = m_marginWidth * num3 / 100f;
				break;
			}
			m_indentStack.Add(tag_Indent);
			m_xAdvance = tag_Indent;
			return true;
		case 6971027:
		case 7598483:
			tag_Indent = m_indentStack.Remove();
			return true;
		case -842656867:
		case 1109386397:
			num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
			if (num3 == -32768f)
			{
				return false;
			}
			switch (tagUnitType)
			{
			case TagUnitType.Pixels:
				tag_LineIndent = num3 * (m_isOrthographic ? 1f : 0.1f);
				break;
			case TagUnitType.FontUnits:
				tag_LineIndent = num3 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
				break;
			case TagUnitType.Percentage:
				tag_LineIndent = m_marginWidth * num3 / 100f;
				break;
			}
			m_xAdvance += tag_LineIndent;
			return true;
		case -445537194:
		case 1897386838:
			tag_LineIndent = 0f;
			return true;
		case 1619421:
		case 2246877:
		{
			int valueHashCode4 = m_xmlAttribute[0].valueHashCode;
			m_spriteIndex = -1;
			TMP_SpriteAsset tMP_SpriteAsset;
			if (m_xmlAttribute[0].valueType == TagValueType.None || m_xmlAttribute[0].valueType == TagValueType.NumericalValue)
			{
				if (m_spriteAsset != null)
				{
					m_currentSpriteAsset = m_spriteAsset;
				}
				else if (m_defaultSpriteAsset != null)
				{
					m_currentSpriteAsset = m_defaultSpriteAsset;
				}
				else if (m_defaultSpriteAsset == null)
				{
					if (TMP_Settings.defaultSpriteAsset != null)
					{
						m_defaultSpriteAsset = TMP_Settings.defaultSpriteAsset;
					}
					else
					{
						m_defaultSpriteAsset = Resources.Load<TMP_SpriteAsset>("Sprite Assets/Default Sprite Asset");
					}
					m_currentSpriteAsset = m_defaultSpriteAsset;
				}
				if (m_currentSpriteAsset == null)
				{
					return false;
				}
			}
			else if (MaterialReferenceManager.TryGetSpriteAsset(valueHashCode4, out tMP_SpriteAsset))
			{
				m_currentSpriteAsset = tMP_SpriteAsset;
			}
			else
			{
				if (tMP_SpriteAsset == null)
				{
					tMP_SpriteAsset = TMP_Text.OnSpriteAssetRequest?.Invoke(valueHashCode4, new string(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength));
					if (tMP_SpriteAsset == null)
					{
						tMP_SpriteAsset = Resources.Load<TMP_SpriteAsset>(TMP_Settings.defaultSpriteAssetPath + new string(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength));
					}
				}
				if (tMP_SpriteAsset == null)
				{
					return false;
				}
				MaterialReferenceManager.AddSpriteAsset(valueHashCode4, tMP_SpriteAsset);
				m_currentSpriteAsset = tMP_SpriteAsset;
			}
			if (m_xmlAttribute[0].valueType == TagValueType.NumericalValue)
			{
				int num5 = (int)ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num5 == -32768)
				{
					return false;
				}
				if (num5 > m_currentSpriteAsset.spriteCharacterTable.Count - 1)
				{
					return false;
				}
				m_spriteIndex = num5;
			}
			m_spriteColor = s_colorWhite;
			m_tintSprite = false;
			for (int l = 0; l < m_xmlAttribute.Length && m_xmlAttribute[l].nameHashCode != 0; l++)
			{
				int nameHashCode2 = m_xmlAttribute[l].nameHashCode;
				int spriteIndex = 0;
				switch (nameHashCode2)
				{
				case 30547:
				case 43347:
					m_currentSpriteAsset = TMP_SpriteAsset.SearchForSpriteByHashCode(m_currentSpriteAsset, m_xmlAttribute[l].valueHashCode, includeFallbacks: true, out spriteIndex);
					if (spriteIndex == -1)
					{
						return false;
					}
					m_spriteIndex = spriteIndex;
					break;
				case 205930:
				case 295562:
					spriteIndex = (int)ConvertToFloat(m_htmlTag, m_xmlAttribute[1].valueStartIndex, m_xmlAttribute[1].valueLength);
					if (spriteIndex == -32768)
					{
						return false;
					}
					if (spriteIndex > m_currentSpriteAsset.spriteCharacterTable.Count - 1)
					{
						return false;
					}
					m_spriteIndex = spriteIndex;
					break;
				case 33019:
				case 45819:
					m_tintSprite = ConvertToFloat(m_htmlTag, m_xmlAttribute[l].valueStartIndex, m_xmlAttribute[l].valueLength) != 0f;
					break;
				case 192323:
				case 281955:
					m_spriteColor = HexCharsToColor(m_htmlTag, m_xmlAttribute[l].valueStartIndex, m_xmlAttribute[l].valueLength);
					break;
				case 26705:
				case 39505:
					if (GetAttributeParameters(m_htmlTag, m_xmlAttribute[l].valueStartIndex, m_xmlAttribute[l].valueLength, ref m_attributeParameterValues) != 3)
					{
						return false;
					}
					m_spriteIndex = (int)m_attributeParameterValues[0];
					if (m_isParsingText)
					{
						spriteAnimator.DoSpriteAnimation(m_characterCount, m_currentSpriteAsset, m_spriteIndex, (int)m_attributeParameterValues[1], (int)m_attributeParameterValues[2]);
					}
					break;
				default:
					if (nameHashCode2 != 2246877 && nameHashCode2 != 1619421)
					{
						return false;
					}
					break;
				}
			}
			if (m_spriteIndex == -1)
			{
				return false;
			}
			m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentSpriteAsset.material, m_currentSpriteAsset, ref m_materialReferences, m_materialReferenceIndexLookup);
			m_textElementType = TMP_TextElementType.Sprite;
			return true;
		}
		case 514803617:
		case 730022849:
			m_FontStyleInternal |= FontStyles.LowerCase;
			m_fontStyleStack.Add(FontStyles.LowerCase);
			return true;
		case -1883544150:
		case -1668324918:
			if ((m_fontStyle & FontStyles.LowerCase) != FontStyles.LowerCase && m_fontStyleStack.Remove(FontStyles.LowerCase) == 0)
			{
				m_FontStyleInternal &= ~FontStyles.LowerCase;
			}
			return true;
		case 9133802:
		case 13526026:
		case 566686826:
		case 781906058:
			m_FontStyleInternal |= FontStyles.UpperCase;
			m_fontStyleStack.Add(FontStyles.UpperCase);
			return true;
		case -1831660941:
		case -1616441709:
		case 47840323:
		case 52232547:
			if ((m_fontStyle & FontStyles.UpperCase) != FontStyles.UpperCase && m_fontStyleStack.Remove(FontStyles.UpperCase) == 0)
			{
				m_FontStyleInternal &= ~FontStyles.UpperCase;
			}
			return true;
		case 551025096:
		case 766244328:
			m_FontStyleInternal |= FontStyles.SmallCaps;
			m_fontStyleStack.Add(FontStyles.SmallCaps);
			return true;
		case -1847322671:
		case -1632103439:
			if ((m_fontStyle & FontStyles.SmallCaps) != FontStyles.SmallCaps && m_fontStyleStack.Remove(FontStyles.SmallCaps) == 0)
			{
				m_FontStyleInternal &= ~FontStyles.SmallCaps;
			}
			return true;
		case 1482398:
		case 2109854:
			switch (m_xmlAttribute[0].valueType)
			{
			case TagValueType.NumericalValue:
				num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num3 == -32768f)
				{
					return false;
				}
				switch (tagUnitType)
				{
				case TagUnitType.Pixels:
					m_marginLeft = num3 * (m_isOrthographic ? 1f : 0.1f);
					break;
				case TagUnitType.FontUnits:
					m_marginLeft = num3 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
					break;
				case TagUnitType.Percentage:
					m_marginLeft = (m_marginWidth - ((m_width != -1f) ? m_width : 0f)) * num3 / 100f;
					break;
				}
				m_marginLeft = ((m_marginLeft >= 0f) ? m_marginLeft : 0f);
				m_marginRight = m_marginLeft;
				return true;
			case TagValueType.None:
			{
				for (int k = 1; k < m_xmlAttribute.Length && m_xmlAttribute[k].nameHashCode != 0; k++)
				{
					switch (m_xmlAttribute[k].nameHashCode)
					{
					case 42823:
						num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[k].valueStartIndex, m_xmlAttribute[k].valueLength);
						if (num3 == -32768f)
						{
							return false;
						}
						switch (m_xmlAttribute[k].unitType)
						{
						case TagUnitType.Pixels:
							m_marginLeft = num3 * (m_isOrthographic ? 1f : 0.1f);
							break;
						case TagUnitType.FontUnits:
							m_marginLeft = num3 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
							break;
						case TagUnitType.Percentage:
							m_marginLeft = (m_marginWidth - ((m_width != -1f) ? m_width : 0f)) * num3 / 100f;
							break;
						}
						m_marginLeft = ((m_marginLeft >= 0f) ? m_marginLeft : 0f);
						break;
					case 315620:
						num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[k].valueStartIndex, m_xmlAttribute[k].valueLength);
						if (num3 == -32768f)
						{
							return false;
						}
						switch (m_xmlAttribute[k].unitType)
						{
						case TagUnitType.Pixels:
							m_marginRight = num3 * (m_isOrthographic ? 1f : 0.1f);
							break;
						case TagUnitType.FontUnits:
							m_marginRight = num3 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
							break;
						case TagUnitType.Percentage:
							m_marginRight = (m_marginWidth - ((m_width != -1f) ? m_width : 0f)) * num3 / 100f;
							break;
						}
						m_marginRight = ((m_marginRight >= 0f) ? m_marginRight : 0f);
						break;
					}
				}
				return true;
			}
			default:
				return false;
			}
		case 7011901:
		case 7639357:
			m_marginLeft = 0f;
			m_marginRight = 0f;
			return true;
		case -855002522:
		case 1100728678:
			num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
			if (num3 == -32768f)
			{
				return false;
			}
			switch (tagUnitType)
			{
			case TagUnitType.Pixels:
				m_marginLeft = num3 * (m_isOrthographic ? 1f : 0.1f);
				break;
			case TagUnitType.FontUnits:
				m_marginLeft = num3 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
				break;
			case TagUnitType.Percentage:
				m_marginLeft = (m_marginWidth - ((m_width != -1f) ? m_width : 0f)) * num3 / 100f;
				break;
			}
			m_marginLeft = ((m_marginLeft >= 0f) ? m_marginLeft : 0f);
			return true;
		case -1690034531:
		case -884817987:
			num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
			if (num3 == -32768f)
			{
				return false;
			}
			switch (tagUnitType)
			{
			case TagUnitType.Pixels:
				m_marginRight = num3 * (m_isOrthographic ? 1f : 0.1f);
				break;
			case TagUnitType.FontUnits:
				m_marginRight = num3 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
				break;
			case TagUnitType.Percentage:
				m_marginRight = (m_marginWidth - ((m_width != -1f) ? m_width : 0f)) * num3 / 100f;
				break;
			}
			m_marginRight = ((m_marginRight >= 0f) ? m_marginRight : 0f);
			return true;
		case -842693512:
		case 1109349752:
			num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
			if (num3 == -32768f)
			{
				return false;
			}
			switch (tagUnitType)
			{
			case TagUnitType.Pixels:
				m_lineHeight = num3 * (m_isOrthographic ? 1f : 0.1f);
				break;
			case TagUnitType.FontUnits:
				m_lineHeight = num3 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
				break;
			case TagUnitType.Percentage:
			{
				float num4 = m_currentFontSize / (float)m_currentFontAsset.faceInfo.pointSize * m_currentFontAsset.faceInfo.scale * (m_isOrthographic ? 1f : 0.1f);
				m_lineHeight = m_fontAsset.faceInfo.lineHeight * num3 / 100f * num4;
				break;
			}
			}
			return true;
		case -445573839:
		case 1897350193:
			m_lineHeight = -32767f;
			return true;
		case 10723418:
		case 15115642:
			tag_NoParsing = true;
			return true;
		case 1286342:
		case 1913798:
		{
			int valueHashCode = m_xmlAttribute[0].valueHashCode;
			if (m_isParsingText)
			{
				m_actionStack.Add(valueHashCode);
				UnityEngine.Debug.Log("Action ID: [" + valueHashCode + "] First character index: " + m_characterCount);
			}
			return true;
		}
		case 6815845:
		case 7443301:
			if (m_isParsingText)
			{
				UnityEngine.Debug.Log("Action ID: [" + m_actionStack.CurrentItem() + "] Last character index: " + (m_characterCount - 1));
			}
			m_actionStack.Remove();
			return true;
		case 226050:
		case 315682:
			num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
			if (num3 == -32768f)
			{
				return false;
			}
			m_FXMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(num3, 1f, 1f));
			m_isFXMatrixSet = true;
			return true;
		case 1015979:
		case 1105611:
			m_isFXMatrixSet = false;
			return true;
		case 1600507:
		case 2227963:
			num3 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
			if (num3 == -32768f)
			{
				return false;
			}
			m_FXMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, num3), Vector3.one);
			m_isFXMatrixSet = true;
			return true;
		case 7130010:
		case 7757466:
			m_isFXMatrixSet = false;
			return true;
		case 227814:
		case 317446:
			return false;
		case 1017743:
		case 1107375:
			return true;
		case 670:
		case 926:
			return true;
		case 2973:
		case 3229:
			return true;
		case 660:
		case 916:
			return true;
		case 2963:
		case 3219:
			return true;
		case 656:
		case 912:
			return false;
		case 2959:
		case 3215:
			return false;
		default:
			return false;
		}
	}

	public void AddInputHtmlTag(string tagStrStart, string tagStrEnd, int tagIndexStart, int tagIndexEnd, bool isArabicFixIngore = false)
	{
		InputHtmlTagData item = new InputHtmlTagData
		{
			index = tagIndexStart,
			tagStr = tagStrStart
		};
		InputHtmlTagData item2 = new InputHtmlTagData
		{
			index = tagIndexEnd + 1,
			tagStr = tagStrEnd
		};
		inputHtmlTagDataList.Add(item);
		inputHtmlTagDataList.Add(item2);
		InputHtmlRange item3 = new InputHtmlRange
		{
			startIndex = tagIndexStart,
			endIndex = tagIndexEnd,
			isArabicFixIgnore = isArabicFixIngore
		};
		inputHtmlRangeList.Add(item3);
		isInputHtmlTagUse = true;
		isInputHtmlTagNeedSort = true;
	}

	public void SetInputHtmlTagEmpty()
	{
		inputHtmlTagDataList.Clear();
		inputHtmlRangeList.Clear();
		inputHtmlTagDealIndex = 0;
		inputHtmlTagPreDealCharIndex = 0;
		isInputHtmlTagUse = false;
		isInputHtmlTagNeedSort = false;
	}

	internal void SetInputHtmlTagSortAndDefault()
	{
		if (isInputHtmlTagNeedSort)
		{
			isInputHtmlTagNeedSort = false;
			inputHtmlTagDataList.Sort((InputHtmlTagData a, InputHtmlTagData b) => (a.index >= b.index) ? 1 : (-1));
		}
		inputHtmlTagDealIndex = 0;
		inputHtmlTagPreDealCharIndex = 0;
	}

	internal bool ValidateInputHtmlTag(int startIndex)
	{
		if (!isInputHtmlTagUse)
		{
			return false;
		}
		int count = inputHtmlTagDataList.Count;
		if (startIndex > 0 && inputHtmlTagPreDealCharIndex >= startIndex)
		{
			inputHtmlTagDealIndex = 0;
			for (int i = 0; i < count; i++)
			{
				if (inputHtmlTagDataList[i].index < startIndex)
				{
					inputHtmlTagDealIndex = i + 1;
				}
			}
		}
		inputHtmlTagPreDealCharIndex = startIndex;
		while (inputHtmlTagDealIndex < count)
		{
			InputHtmlTagData inputHtmlTagData = inputHtmlTagDataList[inputHtmlTagDealIndex];
			if (inputHtmlTagData.index > startIndex)
			{
				break;
			}
			if (inputHtmlTagData.index < startIndex)
			{
				inputHtmlTagDealIndex++;
				continue;
			}
			inputHtmlTagDealIndex++;
			char[] array = inputHtmlTagData.tagStr.ToCharArray();
			int num = 0;
			byte b = 0;
			int num2 = 0;
			m_xmlAttribute[num2].nameHashCode = 0;
			m_xmlAttribute[num2].valueHashCode = 0;
			m_xmlAttribute[num2].valueStartIndex = 0;
			m_xmlAttribute[num2].valueLength = 0;
			TagValueType tagValueType = (m_xmlAttribute[num2].valueType = TagValueType.None);
			TagUnitType tagUnitType = (m_xmlAttribute[num2].unitType = TagUnitType.Pixels);
			m_xmlAttribute[1].nameHashCode = 0;
			m_xmlAttribute[2].nameHashCode = 0;
			m_xmlAttribute[3].nameHashCode = 0;
			m_xmlAttribute[4].nameHashCode = 0;
			bool flag = false;
			bool flag2 = false;
			if (array.Length == 0 || array.Length >= m_htmlTag.Length)
			{
				break;
			}
			bool flag3 = true;
			foreach (int num3 in array)
			{
				m_htmlTag[num] = (char)num3;
				num++;
				if (b == 1)
				{
					switch (tagValueType)
					{
					case TagValueType.None:
						switch (num3)
						{
						case 43:
						case 45:
						case 46:
						case 48:
						case 49:
						case 50:
						case 51:
						case 52:
						case 53:
						case 54:
						case 55:
						case 56:
						case 57:
							tagUnitType = TagUnitType.Pixels;
							tagValueType = (m_xmlAttribute[num2].valueType = TagValueType.NumericalValue);
							m_xmlAttribute[num2].valueStartIndex = num - 1;
							m_xmlAttribute[num2].valueLength++;
							break;
						default:
							switch (num3)
							{
							case 35:
								tagUnitType = TagUnitType.Pixels;
								tagValueType = (m_xmlAttribute[num2].valueType = TagValueType.ColorValue);
								m_xmlAttribute[num2].valueStartIndex = num - 1;
								m_xmlAttribute[num2].valueLength++;
								break;
							case 34:
								tagUnitType = TagUnitType.Pixels;
								tagValueType = (m_xmlAttribute[num2].valueType = TagValueType.StringValue);
								m_xmlAttribute[num2].valueStartIndex = num;
								break;
							default:
								tagUnitType = TagUnitType.Pixels;
								tagValueType = (m_xmlAttribute[num2].valueType = TagValueType.StringValue);
								m_xmlAttribute[num2].valueStartIndex = num - 1;
								m_xmlAttribute[num2].valueHashCode = ((m_xmlAttribute[num2].valueHashCode << 5) + m_xmlAttribute[num2].valueHashCode) ^ num3;
								m_xmlAttribute[num2].valueLength++;
								break;
							}
							break;
						}
						break;
					case TagValueType.NumericalValue:
						if (num3 == 112 || num3 == 101 || num3 == 37 || num3 == 32)
						{
							b = 2;
							tagValueType = TagValueType.None;
							tagUnitType = num3 switch
							{
								101 => m_xmlAttribute[num2].unitType = TagUnitType.FontUnits, 
								37 => m_xmlAttribute[num2].unitType = TagUnitType.Percentage, 
								_ => m_xmlAttribute[num2].unitType = TagUnitType.Pixels, 
							};
							num2++;
							m_xmlAttribute[num2].nameHashCode = 0;
							m_xmlAttribute[num2].valueHashCode = 0;
							m_xmlAttribute[num2].valueType = TagValueType.None;
							m_xmlAttribute[num2].unitType = TagUnitType.Pixels;
							m_xmlAttribute[num2].valueStartIndex = 0;
							m_xmlAttribute[num2].valueLength = 0;
						}
						else if (b != 2)
						{
							m_xmlAttribute[num2].valueLength++;
						}
						break;
					case TagValueType.ColorValue:
						if (num3 != 32)
						{
							m_xmlAttribute[num2].valueLength++;
							break;
						}
						b = 2;
						tagValueType = TagValueType.None;
						tagUnitType = TagUnitType.Pixels;
						num2++;
						m_xmlAttribute[num2].nameHashCode = 0;
						m_xmlAttribute[num2].valueType = TagValueType.None;
						m_xmlAttribute[num2].unitType = TagUnitType.Pixels;
						m_xmlAttribute[num2].valueHashCode = 0;
						m_xmlAttribute[num2].valueStartIndex = 0;
						m_xmlAttribute[num2].valueLength = 0;
						break;
					case TagValueType.StringValue:
						if (num3 != 34)
						{
							m_xmlAttribute[num2].valueHashCode = ((m_xmlAttribute[num2].valueHashCode << 5) + m_xmlAttribute[num2].valueHashCode) ^ num3;
							m_xmlAttribute[num2].valueLength++;
							break;
						}
						b = 2;
						tagValueType = TagValueType.None;
						tagUnitType = TagUnitType.Pixels;
						num2++;
						m_xmlAttribute[num2].nameHashCode = 0;
						m_xmlAttribute[num2].valueType = TagValueType.None;
						m_xmlAttribute[num2].unitType = TagUnitType.Pixels;
						m_xmlAttribute[num2].valueHashCode = 0;
						m_xmlAttribute[num2].valueStartIndex = 0;
						m_xmlAttribute[num2].valueLength = 0;
						break;
					}
				}
				if (num3 == 61)
				{
					b = 1;
				}
				if (b == 0 && num3 == 32)
				{
					if (flag)
					{
						flag3 = false;
						break;
					}
					flag = true;
					b = 2;
					tagValueType = TagValueType.None;
					tagUnitType = TagUnitType.Pixels;
					num2++;
					m_xmlAttribute[num2].nameHashCode = 0;
					m_xmlAttribute[num2].valueType = TagValueType.None;
					m_xmlAttribute[num2].unitType = TagUnitType.Pixels;
					m_xmlAttribute[num2].valueHashCode = 0;
					m_xmlAttribute[num2].valueStartIndex = 0;
					m_xmlAttribute[num2].valueLength = 0;
				}
				if (b == 0)
				{
					m_xmlAttribute[num2].nameHashCode = (m_xmlAttribute[num2].nameHashCode << 3) - m_xmlAttribute[num2].nameHashCode + num3;
				}
				if (b == 2 && num3 == 32)
				{
					b = 0;
				}
			}
			if (flag3)
			{
				flag2 = true;
				m_htmlTag[num] = '\0';
			}
			if (!flag2 || (tag_NoParsing && m_xmlAttribute[0].nameHashCode != 53822163 && m_xmlAttribute[0].nameHashCode != 49429939))
			{
				continue;
			}
			if (m_xmlAttribute[0].nameHashCode == 53822163 || m_xmlAttribute[0].nameHashCode == 49429939)
			{
				tag_NoParsing = false;
				continue;
			}
			if (m_htmlTag[0] == '#' && num == 4)
			{
				m_htmlColor = HexCharsToColor(m_htmlTag, num);
				m_colorStack.Add(m_htmlColor);
				continue;
			}
			if (m_htmlTag[0] == '#' && num == 5)
			{
				m_htmlColor = HexCharsToColor(m_htmlTag, num);
				m_colorStack.Add(m_htmlColor);
				continue;
			}
			if (m_htmlTag[0] == '#' && num == 7)
			{
				m_htmlColor = HexCharsToColor(m_htmlTag, num);
				m_colorStack.Add(m_htmlColor);
				continue;
			}
			if (m_htmlTag[0] == '#' && num == 9)
			{
				m_htmlColor = HexCharsToColor(m_htmlTag, num);
				m_colorStack.Add(m_htmlColor);
				continue;
			}
			float num4 = 0f;
			Material material;
			switch (m_xmlAttribute[0].nameHashCode)
			{
			case 66:
			case 98:
				m_FontStyleInternal |= FontStyles.Bold;
				m_fontStyleStack.Add(FontStyles.Bold);
				m_FontWeightInternal = FontWeight.Bold;
				break;
			case 395:
			case 427:
				if ((m_fontStyle & FontStyles.Bold) != FontStyles.Bold && m_fontStyleStack.Remove(FontStyles.Bold) == 0)
				{
					m_FontStyleInternal &= ~FontStyles.Bold;
					m_FontWeightInternal = m_FontWeightStack.Peek();
				}
				break;
			case 73:
			case 105:
				m_FontStyleInternal |= FontStyles.Italic;
				m_fontStyleStack.Add(FontStyles.Italic);
				if (m_xmlAttribute[1].nameHashCode == 276531 || m_xmlAttribute[1].nameHashCode == 186899)
				{
					m_ItalicAngle = (int)ConvertToFloat(m_htmlTag, m_xmlAttribute[1].valueStartIndex, m_xmlAttribute[1].valueLength);
					if (m_ItalicAngle < -180 || m_ItalicAngle > 180)
					{
						break;
					}
				}
				else
				{
					m_ItalicAngle = m_currentFontAsset.italicStyle;
				}
				m_ItalicAngleStack.Add(m_ItalicAngle);
				break;
			case 402:
			case 434:
				if ((m_fontStyle & FontStyles.Italic) != FontStyles.Italic)
				{
					m_ItalicAngle = m_ItalicAngleStack.Remove();
					if (m_fontStyleStack.Remove(FontStyles.Italic) == 0)
					{
						m_FontStyleInternal &= ~FontStyles.Italic;
					}
				}
				break;
			case 83:
			case 115:
				m_FontStyleInternal |= FontStyles.Strikethrough;
				m_fontStyleStack.Add(FontStyles.Strikethrough);
				if (m_xmlAttribute[1].nameHashCode == 281955 || m_xmlAttribute[1].nameHashCode == 192323)
				{
					m_strikethroughColor = HexCharsToColor(m_htmlTag, m_xmlAttribute[1].valueStartIndex, m_xmlAttribute[1].valueLength);
					m_strikethroughColor.a = ((m_htmlColor.a < m_strikethroughColor.a) ? m_htmlColor.a : m_strikethroughColor.a);
				}
				else
				{
					m_strikethroughColor = m_htmlColor;
				}
				m_strikethroughColorStack.Add(m_strikethroughColor);
				break;
			case 412:
			case 444:
				if ((m_fontStyle & FontStyles.Strikethrough) != FontStyles.Strikethrough && m_fontStyleStack.Remove(FontStyles.Strikethrough) == 0)
				{
					m_FontStyleInternal &= ~FontStyles.Strikethrough;
				}
				m_strikethroughColor = m_strikethroughColorStack.Remove();
				break;
			case 85:
			case 117:
				m_FontStyleInternal |= FontStyles.Underline;
				m_fontStyleStack.Add(FontStyles.Underline);
				if (m_xmlAttribute[1].nameHashCode == 281955 || m_xmlAttribute[1].nameHashCode == 192323)
				{
					m_underlineColor = HexCharsToColor(m_htmlTag, m_xmlAttribute[1].valueStartIndex, m_xmlAttribute[1].valueLength);
					m_underlineColor.a = ((m_htmlColor.a < m_underlineColor.a) ? m_htmlColor.a : m_underlineColor.a);
				}
				else
				{
					m_underlineColor = m_htmlColor;
				}
				m_underlineColorStack.Add(m_underlineColor);
				break;
			case 414:
			case 446:
				if ((m_fontStyle & FontStyles.Underline) != FontStyles.Underline)
				{
					m_underlineColor = m_underlineColorStack.Remove();
					if (m_fontStyleStack.Remove(FontStyles.Underline) == 0)
					{
						m_FontStyleInternal &= ~FontStyles.Underline;
					}
				}
				m_underlineColor = m_underlineColorStack.Remove();
				break;
			case 30245:
			case 43045:
			{
				m_FontStyleInternal |= FontStyles.Highlight;
				m_fontStyleStack.Add(FontStyles.Highlight);
				Color32 color = new Color32(byte.MaxValue, byte.MaxValue, 0, 64);
				TMP_Offset padding = TMP_Offset.zero;
				for (int n = 0; n < m_xmlAttribute.Length && m_xmlAttribute[n].nameHashCode != 0; n++)
				{
					switch (m_xmlAttribute[n].nameHashCode)
					{
					case 30245:
					case 43045:
						if (m_xmlAttribute[n].valueType == TagValueType.ColorValue)
						{
							color = HexCharsToColor(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
						}
						break;
					case 281955:
						color = HexCharsToColor(m_htmlTag, m_xmlAttribute[n].valueStartIndex, m_xmlAttribute[n].valueLength);
						break;
					case 15087385:
						if (GetAttributeParameters(m_htmlTag, m_xmlAttribute[n].valueStartIndex, m_xmlAttribute[n].valueLength, ref m_attributeParameterValues) == 4)
						{
							padding = new TMP_Offset(m_attributeParameterValues[0], m_attributeParameterValues[1], m_attributeParameterValues[2], m_attributeParameterValues[3]);
							padding *= m_fontSize * 0.01f * (m_isOrthographic ? 1f : 0.1f);
						}
						break;
					}
				}
				color.a = ((m_htmlColor.a < color.a) ? m_htmlColor.a : color.a);
				HighlightState item = new HighlightState(color, padding);
				m_HighlightStateStack.Push(item);
				break;
			}
			case 143092:
			case 155892:
				if ((m_fontStyle & FontStyles.Highlight) != FontStyles.Highlight)
				{
					m_HighlightStateStack.Remove();
					if (m_fontStyleStack.Remove(FontStyles.Highlight) == 0)
					{
						m_FontStyleInternal &= ~FontStyles.Highlight;
					}
				}
				break;
			case 4728:
			case 6552:
			{
				m_fontScaleMultiplier *= ((m_currentFontAsset.faceInfo.subscriptSize > 0f) ? m_currentFontAsset.faceInfo.subscriptSize : 1f);
				m_baselineOffsetStack.Push(m_baselineOffset);
				float num5 = m_currentFontSize / (float)m_currentFontAsset.faceInfo.pointSize * m_currentFontAsset.faceInfo.scale * (m_isOrthographic ? 1f : 0.1f);
				m_baselineOffset += m_currentFontAsset.faceInfo.subscriptOffset * num5 * m_fontScaleMultiplier;
				m_fontStyleStack.Add(FontStyles.Subscript);
				m_FontStyleInternal |= FontStyles.Subscript;
				break;
			}
			case 20849:
			case 22673:
				if ((m_FontStyleInternal & FontStyles.Subscript) == FontStyles.Subscript)
				{
					if (m_fontScaleMultiplier < 1f)
					{
						m_baselineOffset = m_baselineOffsetStack.Pop();
						m_fontScaleMultiplier /= ((m_currentFontAsset.faceInfo.subscriptSize > 0f) ? m_currentFontAsset.faceInfo.subscriptSize : 1f);
					}
					if (m_fontStyleStack.Remove(FontStyles.Subscript) == 0)
					{
						m_FontStyleInternal &= ~FontStyles.Subscript;
					}
				}
				break;
			case 4742:
			case 6566:
			{
				m_fontScaleMultiplier *= ((m_currentFontAsset.faceInfo.superscriptSize > 0f) ? m_currentFontAsset.faceInfo.superscriptSize : 1f);
				m_baselineOffsetStack.Push(m_baselineOffset);
				float num5 = m_currentFontSize / (float)m_currentFontAsset.faceInfo.pointSize * m_currentFontAsset.faceInfo.scale * (m_isOrthographic ? 1f : 0.1f);
				m_baselineOffset += m_currentFontAsset.faceInfo.superscriptOffset * num5 * m_fontScaleMultiplier;
				m_fontStyleStack.Add(FontStyles.Superscript);
				m_FontStyleInternal |= FontStyles.Superscript;
				break;
			}
			case 20863:
			case 22687:
				if ((m_FontStyleInternal & FontStyles.Superscript) == FontStyles.Superscript)
				{
					if (m_fontScaleMultiplier < 1f)
					{
						m_baselineOffset = m_baselineOffsetStack.Pop();
						m_fontScaleMultiplier /= ((m_currentFontAsset.faceInfo.superscriptSize > 0f) ? m_currentFontAsset.faceInfo.superscriptSize : 1f);
					}
					if (m_fontStyleStack.Remove(FontStyles.Superscript) == 0)
					{
						m_FontStyleInternal &= ~FontStyles.Superscript;
					}
				}
				break;
			case -330774850:
			case 2012149182:
				num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num4 != -32768f)
				{
					switch ((int)num4)
					{
					case 100:
						m_FontWeightInternal = FontWeight.Thin;
						break;
					case 200:
						m_FontWeightInternal = FontWeight.ExtraLight;
						break;
					case 300:
						m_FontWeightInternal = FontWeight.Light;
						break;
					case 400:
						m_FontWeightInternal = FontWeight.Regular;
						break;
					case 500:
						m_FontWeightInternal = FontWeight.Medium;
						break;
					case 600:
						m_FontWeightInternal = FontWeight.SemiBold;
						break;
					case 700:
						m_FontWeightInternal = FontWeight.Bold;
						break;
					case 800:
						m_FontWeightInternal = FontWeight.Heavy;
						break;
					case 900:
						m_FontWeightInternal = FontWeight.Black;
						break;
					}
					m_FontWeightStack.Add(m_FontWeightInternal);
				}
				break;
			case -1885698441:
			case 457225591:
				m_FontWeightStack.Remove();
				if (m_FontStyleInternal == FontStyles.Bold)
				{
					m_FontWeightInternal = FontWeight.Bold;
				}
				else
				{
					m_FontWeightInternal = m_FontWeightStack.Peek();
				}
				break;
			case 4556:
			case 6380:
				num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num4 != -32768f)
				{
					switch (tagUnitType)
					{
					case TagUnitType.Pixels:
						m_xAdvance = num4 * (m_isOrthographic ? 1f : 0.1f);
						break;
					case TagUnitType.FontUnits:
						m_xAdvance = num4 * m_currentFontSize * (m_isOrthographic ? 1f : 0.1f);
						break;
					case TagUnitType.Percentage:
						m_xAdvance = m_marginWidth * num4 / 100f;
						break;
					}
				}
				break;
			case 20677:
			case 22501:
				m_isIgnoringAlignment = false;
				break;
			case 11642281:
			case 16034505:
				num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num4 != -32768f)
				{
					switch (tagUnitType)
					{
					case TagUnitType.Pixels:
						m_baselineOffset = num4 * (m_isOrthographic ? 1f : 0.1f);
						break;
					case TagUnitType.FontUnits:
						m_baselineOffset = num4 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
						break;
					}
				}
				break;
			case 50348802:
			case 54741026:
				m_baselineOffset = 0f;
				break;
			case 31191:
			case 43991:
				if (m_overflowMode == TextOverflowModes.Page)
				{
					m_xAdvance = 0f + tag_LineIndent + tag_Indent;
					m_lineOffset = 0f;
					m_pageNumber++;
					m_isNewPage = true;
				}
				break;
			case 31169:
			case 43969:
				m_isNonBreakingSpace = true;
				break;
			case 144016:
			case 156816:
				m_isNonBreakingSpace = false;
				break;
			case 32745:
			case 45545:
				num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num4 == -32768f)
				{
					break;
				}
				switch (tagUnitType)
				{
				case TagUnitType.Pixels:
					if (m_htmlTag[5] == '+')
					{
						m_currentFontSize = m_fontSize + num4;
						m_sizeStack.Add(m_currentFontSize);
					}
					else if (m_htmlTag[5] == '-')
					{
						m_currentFontSize = m_fontSize + num4;
						m_sizeStack.Add(m_currentFontSize);
					}
					else
					{
						m_currentFontSize = num4;
						m_sizeStack.Add(m_currentFontSize);
					}
					break;
				case TagUnitType.FontUnits:
					m_currentFontSize = m_fontSize * num4;
					m_sizeStack.Add(m_currentFontSize);
					break;
				case TagUnitType.Percentage:
					m_currentFontSize = m_fontSize * num4 / 100f;
					m_sizeStack.Add(m_currentFontSize);
					break;
				}
				break;
			case 145592:
			case 158392:
				m_currentFontSize = m_sizeStack.Remove();
				break;
			case 28511:
			case 41311:
			{
				int valueHashCode5 = m_xmlAttribute[0].valueHashCode;
				int nameHashCode3 = m_xmlAttribute[1].nameHashCode;
				int valueHashCode4 = m_xmlAttribute[1].valueHashCode;
				if (valueHashCode5 == 764638571 || valueHashCode5 == 523367755)
				{
					m_currentFontAsset = m_materialReferences[0].fontAsset;
					m_currentMaterial = m_materialReferences[0].material;
					m_currentMaterialIndex = 0;
					m_materialReferenceStack.Add(m_materialReferences[0]);
					break;
				}
				MaterialReferenceManager.TryGetFontAsset(valueHashCode5, out var fontAsset);
				if (fontAsset == null)
				{
					fontAsset = TMP_Text.OnFontAssetRequest?.Invoke(valueHashCode5, new string(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength));
					if (fontAsset == null)
					{
						fontAsset = Resources.Load<TMP_FontAsset>(TMP_Settings.defaultFontAssetPath + new string(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength));
					}
					if (fontAsset == null)
					{
						break;
					}
					MaterialReferenceManager.AddFontAsset(fontAsset);
				}
				if (nameHashCode3 == 0 && valueHashCode4 == 0)
				{
					m_currentMaterial = fontAsset.material;
					m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, fontAsset, ref m_materialReferences, m_materialReferenceIndexLookup);
					m_materialReferenceStack.Add(m_materialReferences[m_currentMaterialIndex]);
				}
				else
				{
					if (nameHashCode3 != 103415287 && nameHashCode3 != 72669687)
					{
						break;
					}
					if (MaterialReferenceManager.TryGetMaterial(valueHashCode4, out material))
					{
						m_currentMaterial = material;
						m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, fontAsset, ref m_materialReferences, m_materialReferenceIndexLookup);
						m_materialReferenceStack.Add(m_materialReferences[m_currentMaterialIndex]);
					}
					else
					{
						material = Resources.Load<Material>(TMP_Settings.defaultFontAssetPath + new string(m_htmlTag, m_xmlAttribute[1].valueStartIndex, m_xmlAttribute[1].valueLength));
						if (material == null)
						{
							break;
						}
						MaterialReferenceManager.AddFontMaterial(valueHashCode4, material);
						m_currentMaterial = material;
						m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, fontAsset, ref m_materialReferences, m_materialReferenceIndexLookup);
						m_materialReferenceStack.Add(m_materialReferences[m_currentMaterialIndex]);
					}
				}
				m_currentFontAsset = fontAsset;
				break;
			}
			case 141358:
			case 154158:
			{
				MaterialReference materialReference2 = m_materialReferenceStack.Remove();
				m_currentFontAsset = materialReference2.fontAsset;
				m_currentMaterial = materialReference2.material;
				m_currentMaterialIndex = materialReference2.index;
				break;
			}
			case 72669687:
			case 103415287:
			{
				int valueHashCode4 = m_xmlAttribute[0].valueHashCode;
				if (valueHashCode4 == 764638571 || valueHashCode4 == 523367755)
				{
					m_currentMaterial = m_materialReferences[0].material;
					m_currentMaterialIndex = 0;
					m_materialReferenceStack.Add(m_materialReferences[0]);
					break;
				}
				if (MaterialReferenceManager.TryGetMaterial(valueHashCode4, out material))
				{
					m_currentMaterial = material;
					m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, m_currentFontAsset, ref m_materialReferences, m_materialReferenceIndexLookup);
					m_materialReferenceStack.Add(m_materialReferences[m_currentMaterialIndex]);
					break;
				}
				material = Resources.Load<Material>(TMP_Settings.defaultFontAssetPath + new string(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength));
				if (!(material == null))
				{
					MaterialReferenceManager.AddFontMaterial(valueHashCode4, material);
					m_currentMaterial = material;
					m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, m_currentFontAsset, ref m_materialReferences, m_materialReferenceIndexLookup);
					m_materialReferenceStack.Add(m_materialReferences[m_currentMaterialIndex]);
				}
				break;
			}
			case 343615334:
			case 374360934:
			{
				MaterialReference materialReference = m_materialReferenceStack.Remove();
				m_currentMaterial = materialReference.material;
				m_currentMaterialIndex = materialReference.index;
				break;
			}
			case 230446:
			case 320078:
				num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num4 != -32768f)
				{
					switch (tagUnitType)
					{
					case TagUnitType.Pixels:
						m_xAdvance += num4 * (m_isOrthographic ? 1f : 0.1f);
						break;
					case TagUnitType.FontUnits:
						m_xAdvance += num4 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
						break;
					}
				}
				break;
			case 186622:
			case 276254:
				if (m_xmlAttribute[0].valueLength == 3)
				{
					m_htmlColor.a = (byte)(HexToInt(m_htmlTag[7]) * 16 + HexToInt(m_htmlTag[8]));
				}
				break;
			case 30266:
			case 43066:
				if (!m_isCalculatingPreferredValues)
				{
					int linkCount = m_textInfo.linkCount;
					if (linkCount + 1 > m_textInfo.linkInfo.Length)
					{
						TMP_TextInfo.Resize(ref m_textInfo.linkInfo, linkCount + 1);
					}
					m_textInfo.linkInfo[linkCount].textComponent = this;
					m_textInfo.linkInfo[linkCount].hashCode = m_xmlAttribute[0].valueHashCode;
					m_textInfo.linkInfo[linkCount].linkTextfirstCharacterIndex = m_characterCount;
					m_textInfo.linkInfo[linkCount].linkIdFirstCharacterIndex = startIndex + m_xmlAttribute[0].valueStartIndex;
					m_textInfo.linkInfo[linkCount].linkIdLength = m_xmlAttribute[0].valueLength;
					m_textInfo.linkInfo[linkCount].SetLinkID(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				}
				break;
			case 143113:
			case 155913:
				if (!m_isCalculatingPreferredValues && m_textInfo.linkCount < m_textInfo.linkInfo.Length)
				{
					m_textInfo.linkInfo[m_textInfo.linkCount].linkTextLength = m_characterCount - m_textInfo.linkInfo[m_textInfo.linkCount].linkTextfirstCharacterIndex;
					m_textInfo.linkCount++;
				}
				break;
			case 186285:
			case 275917:
				switch (m_xmlAttribute[0].valueHashCode)
				{
				case 3774683:
					m_lineJustification = HorizontalAlignmentOptions.Left;
					m_lineJustificationStack.Add(m_lineJustification);
					break;
				case 136703040:
					m_lineJustification = HorizontalAlignmentOptions.Right;
					m_lineJustificationStack.Add(m_lineJustification);
					break;
				case -458210101:
					m_lineJustification = HorizontalAlignmentOptions.Center;
					m_lineJustificationStack.Add(m_lineJustification);
					break;
				case -523808257:
					m_lineJustification = HorizontalAlignmentOptions.Justified;
					m_lineJustificationStack.Add(m_lineJustification);
					break;
				case 122383428:
					m_lineJustification = HorizontalAlignmentOptions.Flush;
					m_lineJustificationStack.Add(m_lineJustification);
					break;
				}
				break;
			case 976214:
			case 1065846:
				m_lineJustification = m_lineJustificationStack.Remove();
				break;
			case 237918:
			case 327550:
				num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num4 != -32768f)
				{
					switch (tagUnitType)
					{
					case TagUnitType.Pixels:
						m_width = num4 * (m_isOrthographic ? 1f : 0.1f);
						break;
					case TagUnitType.Percentage:
						m_width = m_marginWidth * num4 / 100f;
						break;
					}
				}
				break;
			case 1027847:
			case 1117479:
				m_width = -1f;
				break;
			case 192323:
			case 281955:
				if (m_htmlTag[6] == '#' && num == 10)
				{
					m_htmlColor = HexCharsToColor(m_htmlTag, num);
					m_colorStack.Add(m_htmlColor);
					break;
				}
				if (m_htmlTag[6] == '#' && num == 11)
				{
					m_htmlColor = HexCharsToColor(m_htmlTag, num);
					m_colorStack.Add(m_htmlColor);
					break;
				}
				if (m_htmlTag[6] == '#' && num == 13)
				{
					m_htmlColor = HexCharsToColor(m_htmlTag, num);
					m_colorStack.Add(m_htmlColor);
					break;
				}
				if (m_htmlTag[6] == '#' && num == 15)
				{
					m_htmlColor = HexCharsToColor(m_htmlTag, num);
					m_colorStack.Add(m_htmlColor);
					break;
				}
				switch (m_xmlAttribute[0].valueHashCode)
				{
				case 125395:
					m_htmlColor = Color.red;
					m_colorStack.Add(m_htmlColor);
					break;
				case -992792864:
					m_htmlColor = new Color32(173, 216, 230, byte.MaxValue);
					m_colorStack.Add(m_htmlColor);
					break;
				case 3573310:
					m_htmlColor = Color.blue;
					m_colorStack.Add(m_htmlColor);
					break;
				case 3680713:
					m_htmlColor = new Color32(128, 128, 128, byte.MaxValue);
					m_colorStack.Add(m_htmlColor);
					break;
				case 117905991:
					m_htmlColor = Color.black;
					m_colorStack.Add(m_htmlColor);
					break;
				case 121463835:
					m_htmlColor = Color.green;
					m_colorStack.Add(m_htmlColor);
					break;
				case 140357351:
					m_htmlColor = Color.white;
					m_colorStack.Add(m_htmlColor);
					break;
				case 26556144:
					m_htmlColor = new Color32(byte.MaxValue, 128, 0, byte.MaxValue);
					m_colorStack.Add(m_htmlColor);
					break;
				case -36881330:
					m_htmlColor = new Color32(160, 32, 240, byte.MaxValue);
					m_colorStack.Add(m_htmlColor);
					break;
				case 554054276:
					m_htmlColor = Color.yellow;
					m_colorStack.Add(m_htmlColor);
					break;
				}
				break;
			case 69403544:
			case 100149144:
			{
				int valueHashCode2 = m_xmlAttribute[0].valueHashCode;
				if (MaterialReferenceManager.TryGetColorGradientPreset(valueHashCode2, out var gradientPreset))
				{
					m_colorGradientPreset = gradientPreset;
				}
				else
				{
					if (gradientPreset == null)
					{
						gradientPreset = Resources.Load<TMP_ColorGradient>(TMP_Settings.defaultColorGradientPresetsPath + new string(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength));
					}
					if (gradientPreset == null)
					{
						break;
					}
					MaterialReferenceManager.AddColorGradientPreset(valueHashCode2, gradientPreset);
					m_colorGradientPreset = gradientPreset;
				}
				m_colorGradientPresetIsTinted = false;
				for (int l = 1; l < m_xmlAttribute.Length && m_xmlAttribute[l].nameHashCode != 0; l++)
				{
					int nameHashCode = m_xmlAttribute[l].nameHashCode;
					if (nameHashCode == 33019 || nameHashCode == 45819)
					{
						m_colorGradientPresetIsTinted = ConvertToFloat(m_htmlTag, m_xmlAttribute[l].valueStartIndex, m_xmlAttribute[l].valueLength) != 0f;
					}
				}
				m_colorGradientStack.Add(m_colorGradientPreset);
				break;
			}
			case 340349191:
			case 371094791:
				m_colorGradientPreset = m_colorGradientStack.Remove();
				break;
			case 1356515:
			case 1983971:
				num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num4 != -32768f)
				{
					switch (tagUnitType)
					{
					case TagUnitType.Pixels:
						m_cSpacing = num4 * (m_isOrthographic ? 1f : 0.1f);
						break;
					case TagUnitType.FontUnits:
						m_cSpacing = num4 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
						break;
					}
				}
				break;
			case 6886018:
			case 7513474:
				if (m_isParsingText)
				{
					if (m_characterCount > 0)
					{
						m_xAdvance -= m_cSpacing;
						m_textInfo.characterInfo[m_characterCount - 1].xAdvance = m_xAdvance;
					}
					m_cSpacing = 0f;
				}
				break;
			case 1524585:
			case 2152041:
				num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num4 != -32768f)
				{
					switch (tagUnitType)
					{
					case TagUnitType.Pixels:
						m_monoSpacing = num4 * (m_isOrthographic ? 1f : 0.1f);
						break;
					case TagUnitType.FontUnits:
						m_monoSpacing = num4 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
						break;
					}
				}
				break;
			case 7054088:
			case 7681544:
				m_monoSpacing = 0f;
				break;
			case 982252:
			case 1071884:
				m_htmlColor = m_colorStack.Remove();
				break;
			case 1441524:
			case 2068980:
				num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num4 != -32768f)
				{
					switch (tagUnitType)
					{
					case TagUnitType.Pixels:
						tag_Indent = num4 * (m_isOrthographic ? 1f : 0.1f);
						break;
					case TagUnitType.FontUnits:
						tag_Indent = num4 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
						break;
					case TagUnitType.Percentage:
						tag_Indent = m_marginWidth * num4 / 100f;
						break;
					}
					m_indentStack.Add(tag_Indent);
					m_xAdvance = tag_Indent;
				}
				break;
			case 6971027:
			case 7598483:
				tag_Indent = m_indentStack.Remove();
				break;
			case -842656867:
			case 1109386397:
				num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num4 != -32768f)
				{
					switch (tagUnitType)
					{
					case TagUnitType.Pixels:
						tag_LineIndent = num4 * (m_isOrthographic ? 1f : 0.1f);
						break;
					case TagUnitType.FontUnits:
						tag_LineIndent = num4 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
						break;
					case TagUnitType.Percentage:
						tag_LineIndent = m_marginWidth * num4 / 100f;
						break;
					}
					m_xAdvance += tag_LineIndent;
				}
				break;
			case -445537194:
			case 1897386838:
				tag_LineIndent = 0f;
				break;
			case 1619421:
			case 2246877:
			{
				int valueHashCode3 = m_xmlAttribute[0].valueHashCode;
				m_spriteIndex = -1;
				TMP_SpriteAsset tMP_SpriteAsset;
				if (m_xmlAttribute[0].valueType == TagValueType.None || m_xmlAttribute[0].valueType == TagValueType.NumericalValue)
				{
					if (m_spriteAsset != null)
					{
						m_currentSpriteAsset = m_spriteAsset;
					}
					else if (m_defaultSpriteAsset != null)
					{
						m_currentSpriteAsset = m_defaultSpriteAsset;
					}
					else if (m_defaultSpriteAsset == null)
					{
						if (TMP_Settings.defaultSpriteAsset != null)
						{
							m_defaultSpriteAsset = TMP_Settings.defaultSpriteAsset;
						}
						else
						{
							m_defaultSpriteAsset = Resources.Load<TMP_SpriteAsset>("Sprite Assets/Default Sprite Asset");
						}
						m_currentSpriteAsset = m_defaultSpriteAsset;
					}
					if (m_currentSpriteAsset == null)
					{
						break;
					}
				}
				else if (MaterialReferenceManager.TryGetSpriteAsset(valueHashCode3, out tMP_SpriteAsset))
				{
					m_currentSpriteAsset = tMP_SpriteAsset;
				}
				else
				{
					if (tMP_SpriteAsset == null)
					{
						tMP_SpriteAsset = TMP_Text.OnSpriteAssetRequest?.Invoke(valueHashCode3, new string(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength));
						if (tMP_SpriteAsset == null)
						{
							tMP_SpriteAsset = Resources.Load<TMP_SpriteAsset>(TMP_Settings.defaultSpriteAssetPath + new string(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength));
						}
					}
					if (tMP_SpriteAsset == null)
					{
						break;
					}
					MaterialReferenceManager.AddSpriteAsset(valueHashCode3, tMP_SpriteAsset);
					m_currentSpriteAsset = tMP_SpriteAsset;
				}
				if (m_xmlAttribute[0].valueType == TagValueType.NumericalValue)
				{
					int num6 = (int)ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
					if (num6 == -32768 || num6 > m_currentSpriteAsset.spriteCharacterTable.Count - 1)
					{
						break;
					}
					m_spriteIndex = num6;
				}
				m_spriteColor = s_colorWhite;
				m_tintSprite = false;
				for (int m = 0; m < m_xmlAttribute.Length && m_xmlAttribute[m].nameHashCode != 0; m++)
				{
					int nameHashCode2 = m_xmlAttribute[m].nameHashCode;
					int spriteIndex = 0;
					switch (nameHashCode2)
					{
					case 30547:
					case 43347:
						m_currentSpriteAsset = TMP_SpriteAsset.SearchForSpriteByHashCode(m_currentSpriteAsset, m_xmlAttribute[m].valueHashCode, includeFallbacks: true, out spriteIndex);
						if (spriteIndex != -1)
						{
							m_spriteIndex = spriteIndex;
						}
						break;
					case 205930:
					case 295562:
						spriteIndex = (int)ConvertToFloat(m_htmlTag, m_xmlAttribute[1].valueStartIndex, m_xmlAttribute[1].valueLength);
						if (spriteIndex != -32768 && spriteIndex <= m_currentSpriteAsset.spriteCharacterTable.Count - 1)
						{
							m_spriteIndex = spriteIndex;
						}
						break;
					case 33019:
					case 45819:
						m_tintSprite = ConvertToFloat(m_htmlTag, m_xmlAttribute[m].valueStartIndex, m_xmlAttribute[m].valueLength) != 0f;
						break;
					case 192323:
					case 281955:
						m_spriteColor = HexCharsToColor(m_htmlTag, m_xmlAttribute[m].valueStartIndex, m_xmlAttribute[m].valueLength);
						break;
					case 26705:
					case 39505:
						if (GetAttributeParameters(m_htmlTag, m_xmlAttribute[m].valueStartIndex, m_xmlAttribute[m].valueLength, ref m_attributeParameterValues) == 3)
						{
							m_spriteIndex = (int)m_attributeParameterValues[0];
							if (m_isParsingText)
							{
								spriteAnimator.DoSpriteAnimation(m_characterCount, m_currentSpriteAsset, m_spriteIndex, (int)m_attributeParameterValues[1], (int)m_attributeParameterValues[2]);
							}
						}
						break;
					default:
						if (nameHashCode2 != 2246877)
						{
							_ = 1619421;
						}
						break;
					}
				}
				if (m_spriteIndex != -1)
				{
					m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentSpriteAsset.material, m_currentSpriteAsset, ref m_materialReferences, m_materialReferenceIndexLookup);
					m_textElementType = TMP_TextElementType.Sprite;
				}
				break;
			}
			case 514803617:
			case 730022849:
				m_FontStyleInternal |= FontStyles.LowerCase;
				m_fontStyleStack.Add(FontStyles.LowerCase);
				break;
			case -1883544150:
			case -1668324918:
				if ((m_fontStyle & FontStyles.LowerCase) != FontStyles.LowerCase && m_fontStyleStack.Remove(FontStyles.LowerCase) == 0)
				{
					m_FontStyleInternal &= ~FontStyles.LowerCase;
				}
				break;
			case 9133802:
			case 13526026:
			case 566686826:
			case 781906058:
				m_FontStyleInternal |= FontStyles.UpperCase;
				m_fontStyleStack.Add(FontStyles.UpperCase);
				break;
			case -1831660941:
			case -1616441709:
			case 47840323:
			case 52232547:
				if ((m_fontStyle & FontStyles.UpperCase) != FontStyles.UpperCase && m_fontStyleStack.Remove(FontStyles.UpperCase) == 0)
				{
					m_FontStyleInternal &= ~FontStyles.UpperCase;
				}
				break;
			case 551025096:
			case 766244328:
				m_FontStyleInternal |= FontStyles.SmallCaps;
				m_fontStyleStack.Add(FontStyles.SmallCaps);
				break;
			case -1847322671:
			case -1632103439:
				if ((m_fontStyle & FontStyles.SmallCaps) != FontStyles.SmallCaps && m_fontStyleStack.Remove(FontStyles.SmallCaps) == 0)
				{
					m_FontStyleInternal &= ~FontStyles.SmallCaps;
				}
				break;
			case 1482398:
			case 2109854:
				switch (m_xmlAttribute[0].valueType)
				{
				case TagValueType.NumericalValue:
					num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
					if (num4 != -32768f)
					{
						switch (tagUnitType)
						{
						case TagUnitType.Pixels:
							m_marginLeft = num4 * (m_isOrthographic ? 1f : 0.1f);
							break;
						case TagUnitType.FontUnits:
							m_marginLeft = num4 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
							break;
						case TagUnitType.Percentage:
							m_marginLeft = (m_marginWidth - ((m_width != -1f) ? m_width : 0f)) * num4 / 100f;
							break;
						}
						m_marginLeft = ((m_marginLeft >= 0f) ? m_marginLeft : 0f);
						m_marginRight = m_marginLeft;
					}
					break;
				case TagValueType.None:
				{
					for (int k = 1; k < m_xmlAttribute.Length && m_xmlAttribute[k].nameHashCode != 0; k++)
					{
						switch (m_xmlAttribute[k].nameHashCode)
						{
						case 42823:
							num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[k].valueStartIndex, m_xmlAttribute[k].valueLength);
							if (num4 != -32768f)
							{
								switch (m_xmlAttribute[k].unitType)
								{
								case TagUnitType.Pixels:
									m_marginLeft = num4 * (m_isOrthographic ? 1f : 0.1f);
									break;
								case TagUnitType.FontUnits:
									m_marginLeft = num4 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
									break;
								case TagUnitType.Percentage:
									m_marginLeft = (m_marginWidth - ((m_width != -1f) ? m_width : 0f)) * num4 / 100f;
									break;
								}
								m_marginLeft = ((m_marginLeft >= 0f) ? m_marginLeft : 0f);
							}
							break;
						case 315620:
							num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[k].valueStartIndex, m_xmlAttribute[k].valueLength);
							if (num4 != -32768f)
							{
								switch (m_xmlAttribute[k].unitType)
								{
								case TagUnitType.Pixels:
									m_marginRight = num4 * (m_isOrthographic ? 1f : 0.1f);
									break;
								case TagUnitType.FontUnits:
									m_marginRight = num4 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
									break;
								case TagUnitType.Percentage:
									m_marginRight = (m_marginWidth - ((m_width != -1f) ? m_width : 0f)) * num4 / 100f;
									break;
								}
								m_marginRight = ((m_marginRight >= 0f) ? m_marginRight : 0f);
							}
							break;
						}
					}
					break;
				}
				}
				break;
			case 7011901:
			case 7639357:
				m_marginLeft = 0f;
				m_marginRight = 0f;
				break;
			case -855002522:
			case 1100728678:
				num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num4 != -32768f)
				{
					switch (tagUnitType)
					{
					case TagUnitType.Pixels:
						m_marginLeft = num4 * (m_isOrthographic ? 1f : 0.1f);
						break;
					case TagUnitType.FontUnits:
						m_marginLeft = num4 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
						break;
					case TagUnitType.Percentage:
						m_marginLeft = (m_marginWidth - ((m_width != -1f) ? m_width : 0f)) * num4 / 100f;
						break;
					}
					m_marginLeft = ((m_marginLeft >= 0f) ? m_marginLeft : 0f);
				}
				break;
			case -1690034531:
			case -884817987:
				num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num4 != -32768f)
				{
					switch (tagUnitType)
					{
					case TagUnitType.Pixels:
						m_marginRight = num4 * (m_isOrthographic ? 1f : 0.1f);
						break;
					case TagUnitType.FontUnits:
						m_marginRight = num4 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
						break;
					case TagUnitType.Percentage:
						m_marginRight = (m_marginWidth - ((m_width != -1f) ? m_width : 0f)) * num4 / 100f;
						break;
					}
					m_marginRight = ((m_marginRight >= 0f) ? m_marginRight : 0f);
				}
				break;
			case -842693512:
			case 1109349752:
				num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num4 != -32768f)
				{
					switch (tagUnitType)
					{
					case TagUnitType.Pixels:
						m_lineHeight = num4 * (m_isOrthographic ? 1f : 0.1f);
						break;
					case TagUnitType.FontUnits:
						m_lineHeight = num4 * (m_isOrthographic ? 1f : 0.1f) * m_currentFontSize;
						break;
					case TagUnitType.Percentage:
					{
						float num5 = m_currentFontSize / (float)m_currentFontAsset.faceInfo.pointSize * m_currentFontAsset.faceInfo.scale * (m_isOrthographic ? 1f : 0.1f);
						m_lineHeight = m_fontAsset.faceInfo.lineHeight * num4 / 100f * num5;
						break;
					}
					}
				}
				break;
			case -445573839:
			case 1897350193:
				m_lineHeight = -32767f;
				break;
			case 10723418:
			case 15115642:
				tag_NoParsing = true;
				break;
			case 1286342:
			case 1913798:
			{
				int valueHashCode = m_xmlAttribute[0].valueHashCode;
				if (m_isParsingText)
				{
					m_actionStack.Add(valueHashCode);
					UnityEngine.Debug.Log("Action ID: [" + valueHashCode + "] First character index: " + m_characterCount);
				}
				break;
			}
			case 6815845:
			case 7443301:
				if (m_isParsingText)
				{
					UnityEngine.Debug.Log("Action ID: [" + m_actionStack.CurrentItem() + "] Last character index: " + (m_characterCount - 1));
				}
				m_actionStack.Remove();
				break;
			case 226050:
			case 315682:
				num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num4 != -32768f)
				{
					m_FXMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(num4, 1f, 1f));
					m_isFXMatrixSet = true;
				}
				break;
			case 1015979:
			case 1105611:
				m_isFXMatrixSet = false;
				break;
			case 1600507:
			case 2227963:
				num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
				if (num4 != -32768f)
				{
					m_FXMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, num4), Vector3.one);
					m_isFXMatrixSet = true;
				}
				break;
			case 7130010:
			case 7757466:
				m_isFXMatrixSet = false;
				break;
			}
		}
		return false;
	}

	internal bool ValidateSpriteTag(UnicodeChar[] chars, int startIndex, out int endIndex)
	{
		int num = 0;
		byte b = 0;
		int num2 = 0;
		m_xmlAttribute[num2].nameHashCode = 0;
		m_xmlAttribute[num2].valueHashCode = 0;
		m_xmlAttribute[num2].valueStartIndex = 0;
		m_xmlAttribute[num2].valueLength = 0;
		TagValueType tagValueType = (m_xmlAttribute[num2].valueType = TagValueType.None);
		TagUnitType tagUnitType = (m_xmlAttribute[num2].unitType = TagUnitType.Pixels);
		m_xmlAttribute[1].nameHashCode = 0;
		m_xmlAttribute[2].nameHashCode = 0;
		m_xmlAttribute[3].nameHashCode = 0;
		m_xmlAttribute[4].nameHashCode = 0;
		endIndex = startIndex;
		bool flag = false;
		bool flag2 = false;
		for (int i = startIndex; i < chars.Length && chars[i].unicode != 0; i++)
		{
			if (num >= m_htmlTag.Length)
			{
				break;
			}
			if (chars[i].unicode == InternalSpriteTagBegin)
			{
				break;
			}
			int unicode = chars[i].unicode;
			if (unicode == InternalSpriteTagEnd)
			{
				flag2 = true;
				endIndex = i;
				m_htmlTag[num] = '\0';
				break;
			}
			m_htmlTag[num] = (char)unicode;
			num++;
			if (b == 1)
			{
				switch (tagValueType)
				{
				case TagValueType.None:
					switch (unicode)
					{
					case 43:
					case 45:
					case 46:
					case 48:
					case 49:
					case 50:
					case 51:
					case 52:
					case 53:
					case 54:
					case 55:
					case 56:
					case 57:
						tagUnitType = TagUnitType.Pixels;
						tagValueType = (m_xmlAttribute[num2].valueType = TagValueType.NumericalValue);
						m_xmlAttribute[num2].valueStartIndex = num - 1;
						m_xmlAttribute[num2].valueLength++;
						break;
					default:
						switch (unicode)
						{
						case 35:
							tagUnitType = TagUnitType.Pixels;
							tagValueType = (m_xmlAttribute[num2].valueType = TagValueType.ColorValue);
							m_xmlAttribute[num2].valueStartIndex = num - 1;
							m_xmlAttribute[num2].valueLength++;
							break;
						case 34:
							tagUnitType = TagUnitType.Pixels;
							tagValueType = (m_xmlAttribute[num2].valueType = TagValueType.StringValue);
							m_xmlAttribute[num2].valueStartIndex = num;
							break;
						default:
							tagUnitType = TagUnitType.Pixels;
							tagValueType = (m_xmlAttribute[num2].valueType = TagValueType.StringValue);
							m_xmlAttribute[num2].valueStartIndex = num - 1;
							m_xmlAttribute[num2].valueHashCode = ((m_xmlAttribute[num2].valueHashCode << 5) + m_xmlAttribute[num2].valueHashCode) ^ unicode;
							m_xmlAttribute[num2].valueLength++;
							break;
						}
						break;
					}
					break;
				case TagValueType.NumericalValue:
					if (unicode == 112 || unicode == 101 || unicode == 37 || unicode == 32)
					{
						b = 2;
						tagValueType = TagValueType.None;
						switch (unicode)
						{
						case 101:
							tagUnitType = (m_xmlAttribute[num2].unitType = TagUnitType.FontUnits);
							break;
						case 37:
							tagUnitType = (m_xmlAttribute[num2].unitType = TagUnitType.Percentage);
							break;
						default:
							tagUnitType = (m_xmlAttribute[num2].unitType = TagUnitType.Pixels);
							break;
						}
						num2++;
						m_xmlAttribute[num2].nameHashCode = 0;
						m_xmlAttribute[num2].valueHashCode = 0;
						m_xmlAttribute[num2].valueType = TagValueType.None;
						m_xmlAttribute[num2].unitType = TagUnitType.Pixels;
						m_xmlAttribute[num2].valueStartIndex = 0;
						m_xmlAttribute[num2].valueLength = 0;
					}
					else if (b != 2)
					{
						m_xmlAttribute[num2].valueLength++;
					}
					break;
				case TagValueType.ColorValue:
					if (unicode != 32)
					{
						m_xmlAttribute[num2].valueLength++;
						break;
					}
					b = 2;
					tagValueType = TagValueType.None;
					tagUnitType = TagUnitType.Pixels;
					num2++;
					m_xmlAttribute[num2].nameHashCode = 0;
					m_xmlAttribute[num2].valueType = TagValueType.None;
					m_xmlAttribute[num2].unitType = TagUnitType.Pixels;
					m_xmlAttribute[num2].valueHashCode = 0;
					m_xmlAttribute[num2].valueStartIndex = 0;
					m_xmlAttribute[num2].valueLength = 0;
					break;
				case TagValueType.StringValue:
					if (unicode != 34)
					{
						m_xmlAttribute[num2].valueHashCode = ((m_xmlAttribute[num2].valueHashCode << 5) + m_xmlAttribute[num2].valueHashCode) ^ unicode;
						m_xmlAttribute[num2].valueLength++;
						break;
					}
					b = 2;
					tagValueType = TagValueType.None;
					tagUnitType = TagUnitType.Pixels;
					num2++;
					m_xmlAttribute[num2].nameHashCode = 0;
					m_xmlAttribute[num2].valueType = TagValueType.None;
					m_xmlAttribute[num2].unitType = TagUnitType.Pixels;
					m_xmlAttribute[num2].valueHashCode = 0;
					m_xmlAttribute[num2].valueStartIndex = 0;
					m_xmlAttribute[num2].valueLength = 0;
					break;
				}
			}
			if (unicode == 61)
			{
				b = 1;
			}
			if (b == 0 && unicode == 32)
			{
				if (flag)
				{
					return false;
				}
				flag = true;
				b = 2;
				tagValueType = TagValueType.None;
				tagUnitType = TagUnitType.Pixels;
				num2++;
				m_xmlAttribute[num2].nameHashCode = 0;
				m_xmlAttribute[num2].valueType = TagValueType.None;
				m_xmlAttribute[num2].unitType = TagUnitType.Pixels;
				m_xmlAttribute[num2].valueHashCode = 0;
				m_xmlAttribute[num2].valueStartIndex = 0;
				m_xmlAttribute[num2].valueLength = 0;
			}
			if (b == 0)
			{
				m_xmlAttribute[num2].nameHashCode = (m_xmlAttribute[num2].nameHashCode << 3) - m_xmlAttribute[num2].nameHashCode + unicode;
			}
			if (b == 2 && unicode == 32)
			{
				b = 0;
			}
		}
		if (!flag2)
		{
			return false;
		}
		if (m_xmlAttribute[0].nameHashCode != 2246877 && m_xmlAttribute[0].nameHashCode != 1619421)
		{
			return false;
		}
		int valueHashCode = m_xmlAttribute[0].valueHashCode;
		m_spriteIndex = -1;
		TMP_SpriteAsset tMP_SpriteAsset;
		if (m_xmlAttribute[0].valueType == TagValueType.None || m_xmlAttribute[0].valueType == TagValueType.NumericalValue)
		{
			if (m_spriteAsset != null)
			{
				m_currentSpriteAsset = m_spriteAsset;
			}
			else if (m_defaultSpriteAsset != null)
			{
				m_currentSpriteAsset = m_defaultSpriteAsset;
			}
			else if (m_defaultSpriteAsset == null)
			{
				if (TMP_Settings.defaultSpriteAsset != null)
				{
					m_defaultSpriteAsset = TMP_Settings.defaultSpriteAsset;
				}
				else
				{
					m_defaultSpriteAsset = Resources.Load<TMP_SpriteAsset>("Sprite Assets/Default Sprite Asset");
				}
				m_currentSpriteAsset = m_defaultSpriteAsset;
			}
			if (m_currentSpriteAsset == null)
			{
				return false;
			}
		}
		else if (MaterialReferenceManager.TryGetSpriteAsset(valueHashCode, out tMP_SpriteAsset))
		{
			m_currentSpriteAsset = tMP_SpriteAsset;
		}
		else
		{
			if (tMP_SpriteAsset == null)
			{
				tMP_SpriteAsset = TMP_Text.OnSpriteAssetRequest?.Invoke(valueHashCode, new string(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength));
				if (tMP_SpriteAsset == null)
				{
					tMP_SpriteAsset = Resources.Load<TMP_SpriteAsset>(TMP_Settings.defaultSpriteAssetPath + new string(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength));
				}
			}
			if (tMP_SpriteAsset == null)
			{
				return false;
			}
			MaterialReferenceManager.AddSpriteAsset(valueHashCode, tMP_SpriteAsset);
			m_currentSpriteAsset = tMP_SpriteAsset;
		}
		if (m_xmlAttribute[0].valueType == TagValueType.NumericalValue)
		{
			int num3 = (int)ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
			if (num3 == -32768)
			{
				return false;
			}
			if (num3 > m_currentSpriteAsset.spriteCharacterTable.Count - 1)
			{
				return false;
			}
			m_spriteIndex = num3;
		}
		m_spriteColor = s_colorWhite;
		m_tintSprite = false;
		for (int j = 0; j < m_xmlAttribute.Length && m_xmlAttribute[j].nameHashCode != 0; j++)
		{
			int nameHashCode = m_xmlAttribute[j].nameHashCode;
			int spriteIndex = 0;
			switch (nameHashCode)
			{
			case 30547:
			case 43347:
				m_currentSpriteAsset = TMP_SpriteAsset.SearchForSpriteByHashCode(m_currentSpriteAsset, m_xmlAttribute[j].valueHashCode, includeFallbacks: true, out spriteIndex);
				if (spriteIndex == -1)
				{
					return false;
				}
				m_spriteIndex = spriteIndex;
				break;
			case 205930:
			case 295562:
				spriteIndex = (int)ConvertToFloat(m_htmlTag, m_xmlAttribute[1].valueStartIndex, m_xmlAttribute[1].valueLength);
				if (spriteIndex == -32768)
				{
					return false;
				}
				if (spriteIndex > m_currentSpriteAsset.spriteCharacterTable.Count - 1)
				{
					return false;
				}
				m_spriteIndex = spriteIndex;
				break;
			case 33019:
			case 45819:
				m_tintSprite = ConvertToFloat(m_htmlTag, m_xmlAttribute[j].valueStartIndex, m_xmlAttribute[j].valueLength) != 0f;
				break;
			case 192323:
			case 281955:
				m_spriteColor = HexCharsToColor(m_htmlTag, m_xmlAttribute[j].valueStartIndex, m_xmlAttribute[j].valueLength);
				break;
			case 26705:
			case 39505:
				if (GetAttributeParameters(m_htmlTag, m_xmlAttribute[j].valueStartIndex, m_xmlAttribute[j].valueLength, ref m_attributeParameterValues) != 3)
				{
					return false;
				}
				m_spriteIndex = (int)m_attributeParameterValues[0];
				if (m_isParsingText)
				{
					spriteAnimator.DoSpriteAnimation(m_characterCount, m_currentSpriteAsset, m_spriteIndex, (int)m_attributeParameterValues[1], (int)m_attributeParameterValues[2]);
				}
				break;
			default:
				if (nameHashCode != 2246877 && nameHashCode != 1619421)
				{
					return false;
				}
				break;
			}
		}
		if (m_spriteIndex == -1)
		{
			return false;
		}
		m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentSpriteAsset.material, m_currentSpriteAsset, ref m_materialReferences, m_materialReferenceIndexLookup);
		m_textElementType = TMP_TextElementType.Sprite;
		return true;
	}
}
