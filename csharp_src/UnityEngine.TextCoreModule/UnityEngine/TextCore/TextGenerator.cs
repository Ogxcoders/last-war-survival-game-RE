using System.Collections.Generic;
using UnityEngine.Bindings;
using UnityEngine.TextCore.LowLevel;

namespace UnityEngine.TextCore;

[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
internal class TextGenerator
{
	private const int k_Tab = 9;

	private const int k_LineFeed = 10;

	private const int k_CarriageReturn = 13;

	private const int k_Space = 32;

	private const int k_DoubleQuotes = 34;

	private const int k_NumberSign = 35;

	private const int k_PercentSign = 37;

	private const int k_SingleQuote = 39;

	private const int k_Plus = 43;

	private const int k_Minus = 45;

	private const int k_Period = 46;

	private const int k_LesserThan = 60;

	private const int k_Equal = 61;

	private const int k_GreaterThan = 62;

	private const int k_Underline = 95;

	private const int k_NoBreakSpace = 160;

	private const int k_SoftHyphen = 173;

	private const int k_HyphenMinus = 45;

	private const int k_FigureSpace = 8199;

	private const int k_Hyphen = 8208;

	private const int k_NonBreakingHyphen = 8209;

	private const int k_ZeroWidthSpace = 8203;

	private const int k_NarrowNoBreakSpace = 8239;

	private const int k_WordJoiner = 8288;

	private const int k_HorizontalEllipsis = 8230;

	private const int k_RightSingleQuote = 8217;

	private const int k_Square = 9633;

	private const int k_HangulJamoStart = 4352;

	private const int k_HangulJamoEnd = 4607;

	private const int k_CjkStart = 11904;

	private const int k_CjkEnd = 40959;

	private const int k_HangulJameExtendedStart = 43360;

	private const int k_HangulJameExtendedEnd = 43391;

	private const int k_HangulSyllablesStart = 44032;

	private const int k_HangulSyllablesEnd = 55295;

	private const int k_CjkIdeographsStart = 63744;

	private const int k_CjkIdeographsEnd = 64255;

	private const int k_CjkFormsStart = 65072;

	private const int k_CjkFormsEnd = 65103;

	private const int k_CjkHalfwidthStart = 65280;

	private const int k_CjkHalfwidthEnd = 65519;

	private const int k_VerticesMax = 16383;

	private const int k_SpritesStart = 57344;

	private const float k_FloatUnset = -32767f;

	private const int k_MaxCharacters = 8;

	private static TextGenerator s_TextGenerator;

	private Vector3[] m_RectTransformCorners = new Vector3[4];

	private float m_MarginWidth;

	private float m_MarginHeight;

	private int[] m_CharBuffer = new int[8];

	private float m_PreferredWidth;

	private float m_PreferredHeight;

	private FontAsset m_CurrentFontAsset;

	private Material m_CurrentMaterial;

	private int m_CurrentMaterialIndex;

	private RichTextTagStack<MaterialReference> m_MaterialReferenceStack = new RichTextTagStack<MaterialReference>(new MaterialReference[16]);

	private float m_Padding;

	private TextSpriteAsset m_CurrentSpriteAsset;

	private int m_TotalCharacterCount;

	private float m_FontScale;

	private float m_FontSize;

	private float m_FontScaleMultiplier;

	private float m_CurrentFontSize;

	private RichTextTagStack<float> m_SizeStack = new RichTextTagStack<float>(16);

	private FontStyles m_FontStyleInternal = FontStyles.Normal;

	private FontStyleStack m_FontStyleStack;

	private FontWeight m_FontWeightInternal = FontWeight.Regular;

	private RichTextTagStack<FontWeight> m_FontWeightStack = new RichTextTagStack<FontWeight>(8);

	private TextAlignment m_LineJustification;

	private RichTextTagStack<TextAlignment> m_LineJustificationStack = new RichTextTagStack<TextAlignment>(16);

	private float m_BaselineOffset;

	private RichTextTagStack<float> m_BaselineOffsetStack = new RichTextTagStack<float>(new float[16]);

	private Color32 m_FontColor32;

	private Color32 m_HtmlColor;

	private Color32 m_UnderlineColor;

	private Color32 m_StrikethroughColor;

	private RichTextTagStack<Color32> m_ColorStack = new RichTextTagStack<Color32>(new Color32[16]);

	private RichTextTagStack<Color32> m_UnderlineColorStack = new RichTextTagStack<Color32>(new Color32[16]);

	private RichTextTagStack<Color32> m_StrikethroughColorStack = new RichTextTagStack<Color32>(new Color32[16]);

	private RichTextTagStack<Color32> m_HighlightColorStack = new RichTextTagStack<Color32>(new Color32[16]);

	private TextGradientPreset m_ColorGradientPreset;

	private RichTextTagStack<TextGradientPreset> m_ColorGradientStack = new RichTextTagStack<TextGradientPreset>(new TextGradientPreset[16]);

	private RichTextTagStack<int> m_ActionStack = new RichTextTagStack<int>(new int[16]);

	private bool m_IsFxMatrixSet;

	private float m_LineOffset;

	private float m_LineHeight;

	private float m_CSpacing;

	private float m_MonoSpacing;

	private float m_XAdvance;

	private float m_TagLineIndent;

	private float m_TagIndent;

	private RichTextTagStack<float> m_IndentStack = new RichTextTagStack<float>(new float[16]);

	private bool m_TagNoParsing;

	private int m_CharacterCount;

	private int m_FirstCharacterOfLine;

	private int m_LastCharacterOfLine;

	private int m_FirstVisibleCharacterOfLine;

	private int m_LastVisibleCharacterOfLine;

	private float m_MaxLineAscender;

	private float m_MaxLineDescender;

	private int m_LineNumber;

	private int m_LineVisibleCharacterCount;

	private int m_FirstOverflowCharacterIndex;

	private int m_PageNumber;

	private float m_MarginLeft;

	private float m_MarginRight;

	private float m_Width;

	private Extents m_MeshExtents;

	private float m_MaxCapHeight;

	private float m_MaxAscender;

	private float m_MaxDescender;

	private bool m_IsNewPage;

	private bool m_IsNonBreakingSpace;

	private WordWrapState m_SavedWordWrapState;

	private WordWrapState m_SavedLineState;

	private int m_LoopCountA;

	private TextElementType m_TextElementType;

	private bool m_IsParsingText;

	private int m_SpriteIndex;

	private Color32 m_SpriteColor;

	private TextElement m_CachedTextElement;

	private Color32 m_HighlightColor;

	private float m_CharWidthAdjDelta;

	private Matrix4x4 m_FxMatrix;

	private float m_MaxFontSize;

	private float m_MinFontSize;

	private bool m_IsCharacterWrappingEnabled;

	private float m_StartOfLineAscender;

	private float m_LineSpacingDelta;

	private bool m_IsMaskingEnabled;

	private MaterialReference[] m_MaterialReferences = new MaterialReference[16];

	private int m_SpriteCount = 0;

	private RichTextTagStack<int> m_StyleStack = new RichTextTagStack<int>(new int[16]);

	private int m_SpriteAnimationId;

	private uint[] m_InternalTextParsingBuffer = new uint[256];

	private RichTextTagAttribute[] m_Attributes = new RichTextTagAttribute[8];

	private XmlTagAttribute[] m_XmlAttribute = new XmlTagAttribute[8];

	private char[] m_RichTextTag = new char[128];

	private Dictionary<int, int> m_MaterialReferenceIndexLookup = new Dictionary<int, int>();

	private bool m_IsCalculatingPreferredValues;

	private TextSpriteAsset m_DefaultSpriteAsset;

	private bool m_TintSprite;

	private Character m_CachedEllipsisGlyphInfo;

	private Character m_CachedUnderlineGlyphInfo;

	private bool m_IsUsingBold;

	private bool m_IsSdfShader;

	private TextElementInfo[] m_InternalTextElementInfo;

	private int m_RecursiveCount;

	private static TextGenerator GetTextGenerator()
	{
		if (s_TextGenerator == null)
		{
			s_TextGenerator = new TextGenerator();
		}
		return s_TextGenerator;
	}

	public static void GenerateText(TextGenerationSettings settings, TextInfo textInfo)
	{
		if (settings.fontAsset == null || settings.fontAsset.characterLookupTable == null)
		{
			Debug.LogWarning("Can't Generate Mesh, No Font Asset has been assigned.");
			return;
		}
		if (textInfo == null)
		{
			Debug.LogError("Null TextInfo provided to TextGenerator. Cannot update its content.");
			return;
		}
		TextGenerator textGenerator = GetTextGenerator();
		textGenerator.Prepare(settings, textInfo);
		textGenerator.GenerateTextMesh(settings, textInfo);
	}

	public static Vector2 GetCursorPosition(TextGenerationSettings settings, int index)
	{
		if (settings.fontAsset == null || settings.fontAsset.characterLookupTable == null)
		{
			Debug.LogWarning("Can't Generate Mesh, No Font Asset has been assigned.");
			return Vector2.zero;
		}
		TextInfo textInfo = new TextInfo();
		GenerateText(settings, textInfo);
		return GetCursorPosition(textInfo, settings.screenRect, index);
	}

	public static Vector2 GetCursorPosition(TextInfo textInfo, Rect screenRect, int index)
	{
		if (textInfo.characterCount == 0)
		{
			return screenRect.position;
		}
		if (index >= textInfo.characterCount)
		{
			return new Vector2(textInfo.textElementInfo[textInfo.characterCount - 1].xAdvance + screenRect.position.x, screenRect.position.y);
		}
		return new Vector2(textInfo.textElementInfo[index].origin + screenRect.position.x, screenRect.position.y);
	}

	public static float GetPreferredWidth(TextGenerationSettings settings, TextInfo textInfo)
	{
		if (settings.fontAsset == null || settings.fontAsset.characterLookupTable == null)
		{
			Debug.LogWarning("Can't Generate Mesh, No Font Asset has been assigned.");
			return 0f;
		}
		TextGenerator textGenerator = GetTextGenerator();
		textGenerator.Prepare(settings, textInfo);
		return textGenerator.GetPreferredWidthInternal(settings, textInfo);
	}

	public static float GetPreferredHeight(TextGenerationSettings settings, TextInfo textInfo)
	{
		if (settings.fontAsset == null || settings.fontAsset.characterLookupTable == null)
		{
			Debug.LogWarning("Can't Generate Mesh, No Font Asset has been assigned.");
			return 0f;
		}
		TextGenerator textGenerator = GetTextGenerator();
		textGenerator.Prepare(settings, textInfo);
		return textGenerator.GetPreferredHeightInternal(settings, textInfo);
	}

	public static Vector2 GetPreferredValues(TextGenerationSettings settings, TextInfo textInfo)
	{
		if (settings.fontAsset == null || settings.fontAsset.characterLookupTable == null)
		{
			Debug.LogWarning("Can't Generate Mesh, No Font Asset has been assigned.");
			return Vector2.zero;
		}
		TextGenerator textGenerator = GetTextGenerator();
		textGenerator.Prepare(settings, textInfo);
		return textGenerator.GetPreferredValuesInternal(settings, textInfo);
	}

	private void Prepare(TextGenerationSettings generationSettings, TextInfo textInfo)
	{
		m_Padding = 6f;
		m_IsMaskingEnabled = false;
		GetSpecialCharacters(generationSettings.fontAsset);
		ComputeMarginSize(generationSettings.screenRect, generationSettings.margins);
		TextGeneratorUtilities.StringToCharArray(generationSettings.text, ref m_CharBuffer, ref m_StyleStack, generationSettings);
		SetArraySizes(m_CharBuffer, generationSettings, textInfo);
		if (generationSettings.autoSize)
		{
			m_FontSize = Mathf.Clamp(generationSettings.fontSize, generationSettings.fontSizeMin, generationSettings.fontSizeMax);
		}
		else
		{
			m_FontSize = generationSettings.fontSize;
		}
		m_MaxFontSize = generationSettings.fontSizeMax;
		m_MinFontSize = generationSettings.fontSizeMin;
		m_LineSpacingDelta = 0f;
		m_CharWidthAdjDelta = 0f;
		m_IsCharacterWrappingEnabled = false;
	}

	private void GenerateTextMesh(TextGenerationSettings generationSettings, TextInfo textInfo)
	{
		if (textInfo == null)
		{
			return;
		}
		textInfo.Clear();
		if (m_CharBuffer == null || m_CharBuffer.Length == 0 || m_CharBuffer[0] == 0)
		{
			ClearMesh(updateMesh: true, textInfo);
			m_PreferredWidth = 0f;
			m_PreferredHeight = 0f;
			return;
		}
		m_CurrentFontAsset = generationSettings.fontAsset;
		m_CurrentMaterial = generationSettings.material;
		m_CurrentMaterialIndex = 0;
		m_MaterialReferenceStack.SetDefault(new MaterialReference(m_CurrentMaterialIndex, m_CurrentFontAsset, null, m_CurrentMaterial, m_Padding));
		m_CurrentSpriteAsset = generationSettings.spriteAsset;
		int totalCharacterCount = m_TotalCharacterCount;
		float num = (m_FontScale = m_FontSize / (float)generationSettings.fontAsset.faceInfo.pointSize * generationSettings.fontAsset.faceInfo.scale);
		float num2 = num;
		m_FontScaleMultiplier = 1f;
		m_CurrentFontSize = m_FontSize;
		m_SizeStack.SetDefault(m_CurrentFontSize);
		m_FontStyleInternal = generationSettings.fontStyle;
		m_FontWeightInternal = (((m_FontStyleInternal & FontStyles.Bold) == FontStyles.Bold) ? FontWeight.Bold : generationSettings.fontWeight);
		m_FontWeightStack.SetDefault(m_FontWeightInternal);
		m_FontStyleStack.Clear();
		m_LineJustification = generationSettings.textAlignment;
		m_LineJustificationStack.SetDefault(m_LineJustification);
		float num3 = 0f;
		float num4 = 1f;
		m_BaselineOffset = 0f;
		m_BaselineOffsetStack.Clear();
		bool flag = false;
		Vector3 start = Vector3.zero;
		bool flag2 = false;
		Vector3 start2 = Vector3.zero;
		bool flag3 = false;
		Vector3 start3 = Vector3.zero;
		Vector3 vector = Vector3.zero;
		m_FontColor32 = generationSettings.color;
		m_HtmlColor = m_FontColor32;
		m_UnderlineColor = m_HtmlColor;
		m_StrikethroughColor = m_HtmlColor;
		m_ColorStack.SetDefault(m_HtmlColor);
		m_UnderlineColorStack.SetDefault(m_HtmlColor);
		m_StrikethroughColorStack.SetDefault(m_HtmlColor);
		m_HighlightColorStack.SetDefault(m_HtmlColor);
		m_ColorGradientPreset = null;
		m_ColorGradientStack.SetDefault(null);
		m_ActionStack.Clear();
		m_IsFxMatrixSet = false;
		m_LineOffset = 0f;
		m_LineHeight = -32767f;
		float num5 = m_CurrentFontAsset.faceInfo.lineHeight - (m_CurrentFontAsset.faceInfo.ascentLine - m_CurrentFontAsset.faceInfo.descentLine);
		m_CSpacing = 0f;
		m_MonoSpacing = 0f;
		m_XAdvance = 0f;
		m_TagLineIndent = 0f;
		m_TagIndent = 0f;
		m_IndentStack.SetDefault(0f);
		m_TagNoParsing = false;
		m_CharacterCount = 0;
		m_FirstCharacterOfLine = 0;
		m_LastCharacterOfLine = 0;
		m_FirstVisibleCharacterOfLine = 0;
		m_LastVisibleCharacterOfLine = 0;
		m_MaxLineAscender = -32767f;
		m_MaxLineDescender = 32767f;
		m_LineNumber = 0;
		m_LineVisibleCharacterCount = 0;
		bool flag4 = true;
		m_FirstOverflowCharacterIndex = -1;
		m_PageNumber = 0;
		int num6 = Mathf.Clamp(generationSettings.pageToDisplay - 1, 0, textInfo.pageInfo.Length - 1);
		int num7 = 0;
		int num8 = 0;
		Vector4 margins = generationSettings.margins;
		float marginWidth = m_MarginWidth;
		float marginHeight = m_MarginHeight;
		m_MarginLeft = 0f;
		m_MarginRight = 0f;
		m_Width = -1f;
		float num9 = marginWidth + 0.0001f - m_MarginLeft - m_MarginRight;
		m_MeshExtents.min = TextGeneratorUtilities.largePositiveVector2;
		m_MeshExtents.max = TextGeneratorUtilities.largeNegativeVector2;
		textInfo.ClearLineInfo();
		m_MaxCapHeight = 0f;
		m_MaxAscender = 0f;
		m_MaxDescender = 0f;
		float num10 = 0f;
		float num11 = 0f;
		bool flag5 = false;
		m_IsNewPage = false;
		bool flag6 = true;
		m_IsNonBreakingSpace = false;
		bool flag7 = false;
		bool flag8 = false;
		int num12 = 0;
		SaveWordWrappingState(ref m_SavedWordWrapState, -1, -1, textInfo);
		SaveWordWrappingState(ref m_SavedLineState, -1, -1, textInfo);
		m_LoopCountA++;
		Vector3 vector2 = default(Vector3);
		Vector3 vector3 = default(Vector3);
		Vector3 vector4 = default(Vector3);
		Vector3 vector5 = default(Vector3);
		for (int i = 0; i < m_CharBuffer.Length && m_CharBuffer[i] != 0; i++)
		{
			int num13 = m_CharBuffer[i];
			if (generationSettings.richText && num13 == 60)
			{
				m_IsParsingText = true;
				m_TextElementType = TextElementType.Character;
				if (ValidateHtmlTag(m_CharBuffer, i + 1, out var endIndex, generationSettings, textInfo))
				{
					i = endIndex;
					if (m_TextElementType == TextElementType.Character)
					{
						continue;
					}
				}
			}
			else
			{
				m_TextElementType = textInfo.textElementInfo[m_CharacterCount].elementType;
				m_CurrentMaterialIndex = textInfo.textElementInfo[m_CharacterCount].materialReferenceIndex;
				m_CurrentFontAsset = textInfo.textElementInfo[m_CharacterCount].fontAsset;
			}
			int currentMaterialIndex = m_CurrentMaterialIndex;
			bool isUsingAlternateTypeface = textInfo.textElementInfo[m_CharacterCount].isUsingAlternateTypeface;
			m_IsParsingText = false;
			if (m_CharacterCount < generationSettings.firstVisibleCharacter)
			{
				textInfo.textElementInfo[m_CharacterCount].isVisible = false;
				textInfo.textElementInfo[m_CharacterCount].character = '\u200b';
				m_CharacterCount++;
				continue;
			}
			float num14 = 1f;
			if (m_TextElementType == TextElementType.Character)
			{
				if ((m_FontStyleInternal & FontStyles.UpperCase) == FontStyles.UpperCase)
				{
					if (char.IsLower((char)num13))
					{
						num13 = char.ToUpper((char)num13);
					}
				}
				else if ((m_FontStyleInternal & FontStyles.LowerCase) == FontStyles.LowerCase)
				{
					if (char.IsUpper((char)num13))
					{
						num13 = char.ToLower((char)num13);
					}
				}
				else if ((m_FontStyleInternal & FontStyles.SmallCaps) == FontStyles.SmallCaps && char.IsLower((char)num13))
				{
					num14 = 0.8f;
					num13 = char.ToUpper((char)num13);
				}
			}
			if (m_TextElementType == TextElementType.Sprite)
			{
				m_CurrentSpriteAsset = textInfo.textElementInfo[m_CharacterCount].spriteAsset;
				m_SpriteIndex = textInfo.textElementInfo[m_CharacterCount].spriteIndex;
				SpriteCharacter spriteCharacter = m_CurrentSpriteAsset.spriteCharacterTable[m_SpriteIndex];
				if (spriteCharacter == null)
				{
					continue;
				}
				if (num13 == 60)
				{
					num13 = 57344 + m_SpriteIndex;
				}
				else
				{
					m_SpriteColor = Color.white;
				}
				float num15 = m_CurrentFontSize / (float)m_CurrentFontAsset.faceInfo.pointSize * m_CurrentFontAsset.faceInfo.scale;
				num2 = m_CurrentFontAsset.faceInfo.ascentLine / spriteCharacter.glyph.metrics.height * spriteCharacter.scale * num15;
				m_CachedTextElement = spriteCharacter;
				textInfo.textElementInfo[m_CharacterCount].elementType = TextElementType.Sprite;
				textInfo.textElementInfo[m_CharacterCount].scale = num15;
				textInfo.textElementInfo[m_CharacterCount].spriteAsset = m_CurrentSpriteAsset;
				textInfo.textElementInfo[m_CharacterCount].fontAsset = m_CurrentFontAsset;
				textInfo.textElementInfo[m_CharacterCount].materialReferenceIndex = m_CurrentMaterialIndex;
				m_CurrentMaterialIndex = currentMaterialIndex;
				num3 = 0f;
			}
			else if (m_TextElementType == TextElementType.Character)
			{
				m_CachedTextElement = textInfo.textElementInfo[m_CharacterCount].textElement;
				if (m_CachedTextElement == null)
				{
					continue;
				}
				m_CurrentFontAsset = textInfo.textElementInfo[m_CharacterCount].fontAsset;
				m_CurrentMaterial = textInfo.textElementInfo[m_CharacterCount].material;
				m_CurrentMaterialIndex = textInfo.textElementInfo[m_CharacterCount].materialReferenceIndex;
				m_FontScale = m_CurrentFontSize * num14 / (float)m_CurrentFontAsset.faceInfo.pointSize * m_CurrentFontAsset.faceInfo.scale;
				num2 = m_FontScale * m_FontScaleMultiplier * m_CachedTextElement.scale;
				textInfo.textElementInfo[m_CharacterCount].elementType = TextElementType.Character;
				textInfo.textElementInfo[m_CharacterCount].scale = num2;
				num3 = ((m_CurrentMaterialIndex == 0) ? m_Padding : GetPaddingForMaterial(m_CurrentMaterial, generationSettings.extraPadding));
			}
			float num16 = num2;
			if (num13 == 173)
			{
				num2 = 0f;
			}
			textInfo.textElementInfo[m_CharacterCount].character = (char)num13;
			textInfo.textElementInfo[m_CharacterCount].pointSize = m_CurrentFontSize;
			textInfo.textElementInfo[m_CharacterCount].color = m_HtmlColor;
			textInfo.textElementInfo[m_CharacterCount].underlineColor = m_UnderlineColor;
			textInfo.textElementInfo[m_CharacterCount].strikethroughColor = m_StrikethroughColor;
			textInfo.textElementInfo[m_CharacterCount].highlightColor = m_HighlightColor;
			textInfo.textElementInfo[m_CharacterCount].style = m_FontStyleInternal;
			textInfo.textElementInfo[m_CharacterCount].index = i;
			GlyphValueRecord glyphValueRecord = default(GlyphValueRecord);
			if (generationSettings.enableKerning)
			{
				KerningPair value;
				if (m_CharacterCount < totalCharacterCount - 1)
				{
					uint character = textInfo.textElementInfo[m_CharacterCount + 1].character;
					KerningPairKey kerningPairKey = new KerningPairKey((uint)num13, character);
					m_CurrentFontAsset.kerningLookupDictionary.TryGetValue((int)kerningPairKey.key, out value);
					if (value != null)
					{
						glyphValueRecord = value.firstGlyphAdjustments;
					}
				}
				if (m_CharacterCount >= 1)
				{
					uint character2 = textInfo.textElementInfo[m_CharacterCount - 1].character;
					KerningPairKey kerningPairKey2 = new KerningPairKey(character2, (uint)num13);
					m_CurrentFontAsset.kerningLookupDictionary.TryGetValue((int)kerningPairKey2.key, out value);
					if (value != null)
					{
						glyphValueRecord += value.secondGlyphAdjustments;
					}
				}
			}
			if (generationSettings.isRightToLeft)
			{
				m_XAdvance -= ((m_CachedTextElement.glyph.metrics.horizontalAdvance * num4 + generationSettings.characterSpacing + generationSettings.wordSpacing + m_CurrentFontAsset.regularStyleSpacing) * num2 + m_CSpacing) * (1f - m_CharWidthAdjDelta);
				if (char.IsWhiteSpace((char)num13) || num13 == 8203)
				{
					m_XAdvance -= generationSettings.wordSpacing * num2;
				}
			}
			float num17 = 0f;
			if (m_MonoSpacing != 0f)
			{
				num17 = (m_MonoSpacing / 2f - (m_CachedTextElement.glyph.metrics.width / 2f + m_CachedTextElement.glyph.metrics.horizontalBearingX) * num2) * (1f - m_CharWidthAdjDelta);
				m_XAdvance += num17;
			}
			float num19;
			if (m_TextElementType == TextElementType.Character && !isUsingAlternateTypeface && (m_FontStyleInternal & FontStyles.Bold) == FontStyles.Bold)
			{
				if (m_CurrentMaterial.HasProperty(ShaderUtilities.ID_GradientScale))
				{
					float num18 = m_CurrentMaterial.GetFloat(ShaderUtilities.ID_GradientScale);
					num19 = m_CurrentFontAsset.boldStyleWeight / 4f * num18 * m_CurrentMaterial.GetFloat(ShaderUtilities.ID_ScaleRatio_A);
					if (num19 + num3 > num18)
					{
						num3 = num18 - num19;
					}
				}
				else
				{
					num19 = 0f;
				}
				num4 = 1f + m_CurrentFontAsset.boldStyleSpacing * 0.01f;
			}
			else
			{
				if (m_CurrentMaterial.HasProperty(ShaderUtilities.ID_GradientScale))
				{
					float num20 = m_CurrentMaterial.GetFloat(ShaderUtilities.ID_GradientScale);
					num19 = m_CurrentFontAsset.regularStyleWeight / 4f * num20 * m_CurrentMaterial.GetFloat(ShaderUtilities.ID_ScaleRatio_A);
					if (num19 + num3 > num20)
					{
						num3 = num20 - num19;
					}
				}
				else
				{
					num19 = 0f;
				}
				num4 = 1f;
			}
			float num21 = m_CurrentFontAsset.faceInfo.baseline * m_FontScale * m_FontScaleMultiplier * m_CurrentFontAsset.faceInfo.scale;
			vector2.x = m_XAdvance + (m_CachedTextElement.glyph.metrics.horizontalBearingX - num3 - num19 + glyphValueRecord.xPlacement) * num2 * (1f - m_CharWidthAdjDelta);
			vector2.y = num21 + (m_CachedTextElement.glyph.metrics.horizontalBearingY + num3 + glyphValueRecord.yPlacement) * num2 - m_LineOffset + m_BaselineOffset;
			vector2.z = 0f;
			vector3.x = vector2.x;
			vector3.y = vector2.y - (m_CachedTextElement.glyph.metrics.height + num3 * 2f) * num2;
			vector3.z = 0f;
			vector4.x = vector3.x + (m_CachedTextElement.glyph.metrics.width + num3 * 2f + num19 * 2f) * num2 * (1f - m_CharWidthAdjDelta);
			vector4.y = vector2.y;
			vector4.z = 0f;
			vector5.x = vector4.x;
			vector5.y = vector3.y;
			vector5.z = 0f;
			if (m_TextElementType == TextElementType.Character && !isUsingAlternateTypeface && (m_FontStyleInternal & FontStyles.Italic) == FontStyles.Italic)
			{
				float num22 = (float)(int)m_CurrentFontAsset.italicStyleSlant * 0.01f;
				Vector3 vector6 = new Vector3(num22 * ((m_CachedTextElement.glyph.metrics.horizontalBearingY + num3 + num19) * num2), 0f, 0f);
				Vector3 vector7 = new Vector3(num22 * ((m_CachedTextElement.glyph.metrics.horizontalBearingY - m_CachedTextElement.glyph.metrics.height - num3 - num19) * num2), 0f, 0f);
				vector2 += vector6;
				vector3 += vector7;
				vector4 += vector6;
				vector5 += vector7;
			}
			if (m_IsFxMatrixSet)
			{
				Vector3 vector8 = (vector4 + vector3) / 2f;
				vector2 = m_FxMatrix.MultiplyPoint3x4(vector2 - vector8) + vector8;
				vector3 = m_FxMatrix.MultiplyPoint3x4(vector3 - vector8) + vector8;
				vector4 = m_FxMatrix.MultiplyPoint3x4(vector4 - vector8) + vector8;
				vector5 = m_FxMatrix.MultiplyPoint3x4(vector5 - vector8) + vector8;
			}
			textInfo.textElementInfo[m_CharacterCount].bottomLeft = vector3;
			textInfo.textElementInfo[m_CharacterCount].topLeft = vector2;
			textInfo.textElementInfo[m_CharacterCount].topRight = vector4;
			textInfo.textElementInfo[m_CharacterCount].bottomRight = vector5;
			textInfo.textElementInfo[m_CharacterCount].origin = m_XAdvance;
			textInfo.textElementInfo[m_CharacterCount].baseLine = num21 - m_LineOffset + m_BaselineOffset;
			textInfo.textElementInfo[m_CharacterCount].aspectRatio = (vector4.x - vector3.x) / (vector2.y - vector3.y);
			float num23 = m_CurrentFontAsset.faceInfo.ascentLine * ((m_TextElementType == TextElementType.Character) ? (num2 / num14) : textInfo.textElementInfo[m_CharacterCount].scale) + m_BaselineOffset;
			textInfo.textElementInfo[m_CharacterCount].ascender = num23 - m_LineOffset;
			m_MaxLineAscender = ((num23 > m_MaxLineAscender) ? num23 : m_MaxLineAscender);
			float num24 = m_CurrentFontAsset.faceInfo.descentLine * ((m_TextElementType == TextElementType.Character) ? (num2 / num14) : textInfo.textElementInfo[m_CharacterCount].scale) + m_BaselineOffset;
			float num25 = (textInfo.textElementInfo[m_CharacterCount].descender = num24 - m_LineOffset);
			m_MaxLineDescender = ((num24 < m_MaxLineDescender) ? num24 : m_MaxLineDescender);
			if ((m_FontStyleInternal & FontStyles.Subscript) == FontStyles.Subscript || (m_FontStyleInternal & FontStyles.Superscript) == FontStyles.Superscript)
			{
				float num26 = (num23 - m_BaselineOffset) / m_CurrentFontAsset.faceInfo.subscriptSize;
				num23 = m_MaxLineAscender;
				m_MaxLineAscender = ((num26 > m_MaxLineAscender) ? num26 : m_MaxLineAscender);
				float num27 = (num24 - m_BaselineOffset) / m_CurrentFontAsset.faceInfo.subscriptSize;
				num24 = m_MaxLineDescender;
				m_MaxLineDescender = ((num27 < m_MaxLineDescender) ? num27 : m_MaxLineDescender);
			}
			if (m_LineNumber == 0 || m_IsNewPage)
			{
				m_MaxAscender = ((m_MaxAscender > num23) ? m_MaxAscender : num23);
				m_MaxCapHeight = Mathf.Max(m_MaxCapHeight, m_CurrentFontAsset.faceInfo.capLine * num2 / num14);
			}
			if (m_LineOffset == 0f)
			{
				num10 = ((num10 > num23) ? num10 : num23);
			}
			textInfo.textElementInfo[m_CharacterCount].isVisible = false;
			if (num13 == 9 || num13 == 160 || num13 == 8199 || (!char.IsWhiteSpace((char)num13) && num13 != 8203) || m_TextElementType == TextElementType.Sprite)
			{
				textInfo.textElementInfo[m_CharacterCount].isVisible = true;
				num9 = ((m_Width != -1f) ? Mathf.Min(marginWidth + 0.0001f - m_MarginLeft - m_MarginRight, m_Width) : (marginWidth + 0.0001f - m_MarginLeft - m_MarginRight));
				textInfo.lineInfo[m_LineNumber].marginLeft = m_MarginLeft;
				bool flag9 = (m_LineJustification & (TextAlignment)16) == (TextAlignment)16 || (m_LineJustification & (TextAlignment)8) == (TextAlignment)8;
				float num28 = Mathf.Abs(m_XAdvance) + ((!generationSettings.isRightToLeft) ? m_CachedTextElement.glyph.metrics.horizontalAdvance : 0f) * (1f - m_CharWidthAdjDelta) * ((num13 != 173) ? num2 : num16);
				if (num28 > num9 * (flag9 ? 1.05f : 1f))
				{
					num8 = m_CharacterCount - 1;
					if (generationSettings.wordWrap && m_CharacterCount != m_FirstCharacterOfLine)
					{
						if (num12 == m_SavedWordWrapState.previousWordBreak || flag6)
						{
							if (generationSettings.autoSize && m_FontSize > generationSettings.fontSizeMin)
							{
								if (m_CharWidthAdjDelta < generationSettings.charWidthMaxAdj / 100f)
								{
									m_LoopCountA = 0;
									m_CharWidthAdjDelta += 0.01f;
									GenerateTextMesh(generationSettings, textInfo);
									return;
								}
								m_MaxFontSize = m_FontSize;
								m_FontSize -= Mathf.Max((m_FontSize - m_MinFontSize) / 2f, 0.05f);
								m_FontSize = (float)(int)(Mathf.Max(m_FontSize, generationSettings.fontSizeMin) * 20f + 0.5f) / 20f;
								if (m_LoopCountA <= 20)
								{
									GenerateTextMesh(generationSettings, textInfo);
								}
								return;
							}
							if (!m_IsCharacterWrappingEnabled)
							{
								if (!flag7)
								{
									flag7 = true;
								}
								else
								{
									m_IsCharacterWrappingEnabled = true;
								}
							}
							else
							{
								flag8 = true;
							}
						}
						i = RestoreWordWrappingState(ref m_SavedWordWrapState, textInfo);
						num12 = i;
						if (m_CharBuffer[i] == 173)
						{
							m_CharBuffer[i] = 45;
							GenerateTextMesh(generationSettings, textInfo);
							return;
						}
						if (m_LineNumber > 0 && !TextGeneratorUtilities.Approximately(m_MaxLineAscender, m_StartOfLineAscender) && m_LineHeight == -32767f && !m_IsNewPage)
						{
							float num29 = m_MaxLineAscender - m_StartOfLineAscender;
							TextGeneratorUtilities.AdjustLineOffset(m_FirstCharacterOfLine, m_CharacterCount, num29, textInfo);
							m_LineOffset += num29;
							m_SavedWordWrapState.lineOffset = m_LineOffset;
							m_SavedWordWrapState.previousLineAscender = m_MaxLineAscender;
						}
						m_IsNewPage = false;
						float num30 = m_MaxLineAscender - m_LineOffset;
						float num31 = m_MaxLineDescender - m_LineOffset;
						m_MaxDescender = ((m_MaxDescender < num31) ? m_MaxDescender : num31);
						if (!flag5)
						{
							num11 = m_MaxDescender;
						}
						if (generationSettings.useMaxVisibleDescender && (m_CharacterCount >= generationSettings.maxVisibleCharacters || m_LineNumber >= generationSettings.maxVisibleLines))
						{
							flag5 = true;
						}
						textInfo.lineInfo[m_LineNumber].firstCharacterIndex = m_FirstCharacterOfLine;
						textInfo.lineInfo[m_LineNumber].firstVisibleCharacterIndex = (m_FirstVisibleCharacterOfLine = ((m_FirstCharacterOfLine > m_FirstVisibleCharacterOfLine) ? m_FirstCharacterOfLine : m_FirstVisibleCharacterOfLine));
						textInfo.lineInfo[m_LineNumber].lastCharacterIndex = (m_LastCharacterOfLine = ((m_CharacterCount - 1 > 0) ? (m_CharacterCount - 1) : 0));
						textInfo.lineInfo[m_LineNumber].lastVisibleCharacterIndex = (m_LastVisibleCharacterOfLine = ((m_LastVisibleCharacterOfLine < m_FirstVisibleCharacterOfLine) ? m_FirstVisibleCharacterOfLine : m_LastVisibleCharacterOfLine));
						textInfo.lineInfo[m_LineNumber].characterCount = textInfo.lineInfo[m_LineNumber].lastCharacterIndex - textInfo.lineInfo[m_LineNumber].firstCharacterIndex + 1;
						textInfo.lineInfo[m_LineNumber].visibleCharacterCount = m_LineVisibleCharacterCount;
						textInfo.lineInfo[m_LineNumber].lineExtents.min = new Vector2(textInfo.textElementInfo[m_FirstVisibleCharacterOfLine].bottomLeft.x, num31);
						textInfo.lineInfo[m_LineNumber].lineExtents.max = new Vector2(textInfo.textElementInfo[m_LastVisibleCharacterOfLine].topRight.x, num30);
						textInfo.lineInfo[m_LineNumber].length = textInfo.lineInfo[m_LineNumber].lineExtents.max.x;
						textInfo.lineInfo[m_LineNumber].width = num9;
						textInfo.lineInfo[m_LineNumber].maxAdvance = textInfo.textElementInfo[m_LastVisibleCharacterOfLine].xAdvance - (generationSettings.characterSpacing + m_CurrentFontAsset.regularStyleSpacing) * num2 - m_CSpacing;
						textInfo.lineInfo[m_LineNumber].baseline = 0f - m_LineOffset;
						textInfo.lineInfo[m_LineNumber].ascender = num30;
						textInfo.lineInfo[m_LineNumber].descender = num31;
						textInfo.lineInfo[m_LineNumber].lineHeight = num30 - num31 + num5 * num;
						m_FirstCharacterOfLine = m_CharacterCount;
						m_LineVisibleCharacterCount = 0;
						SaveWordWrappingState(ref m_SavedLineState, i, m_CharacterCount - 1, textInfo);
						m_LineNumber++;
						flag4 = true;
						flag6 = true;
						if (m_LineNumber >= textInfo.lineInfo.Length)
						{
							TextGeneratorUtilities.ResizeLineExtents(m_LineNumber, textInfo);
						}
						if (m_LineHeight == -32767f)
						{
							float num32 = textInfo.textElementInfo[m_CharacterCount].ascender - textInfo.textElementInfo[m_CharacterCount].baseLine;
							float num33 = 0f - m_MaxLineDescender + num32 + (num5 + generationSettings.lineSpacing + m_LineSpacingDelta) * num;
							m_LineOffset += num33;
							m_StartOfLineAscender = num32;
						}
						else
						{
							m_LineOffset += m_LineHeight + generationSettings.lineSpacing * num;
						}
						m_MaxLineAscender = -32767f;
						m_MaxLineDescender = 32767f;
						m_XAdvance = 0f + m_TagIndent;
						continue;
					}
					if (generationSettings.autoSize && m_FontSize > generationSettings.fontSizeMin)
					{
						if (m_CharWidthAdjDelta < generationSettings.charWidthMaxAdj / 100f)
						{
							m_LoopCountA = 0;
							m_CharWidthAdjDelta += 0.01f;
							GenerateTextMesh(generationSettings, textInfo);
							return;
						}
						m_MaxFontSize = m_FontSize;
						m_FontSize -= Mathf.Max((m_FontSize - m_MinFontSize) / 2f, 0.05f);
						m_FontSize = (float)(int)(Mathf.Max(m_FontSize, generationSettings.fontSizeMin) * 20f + 0.5f) / 20f;
						if (m_LoopCountA <= 20)
						{
							GenerateTextMesh(generationSettings, textInfo);
						}
						return;
					}
					switch (generationSettings.overflowMode)
					{
					case TextOverflowMode.Overflow:
						if (m_IsMaskingEnabled)
						{
							DisableMasking();
						}
						break;
					case TextOverflowMode.Ellipsis:
						if (m_IsMaskingEnabled)
						{
							DisableMasking();
						}
						if (m_CharacterCount < 1)
						{
							textInfo.textElementInfo[m_CharacterCount].isVisible = false;
							break;
						}
						m_CharBuffer[i - 1] = 8230;
						m_CharBuffer[i] = 0;
						if (m_CachedEllipsisGlyphInfo != null)
						{
							textInfo.textElementInfo[num8].character = '…';
							textInfo.textElementInfo[num8].textElement = m_CachedEllipsisGlyphInfo;
							textInfo.textElementInfo[num8].fontAsset = m_MaterialReferences[0].fontAsset;
							textInfo.textElementInfo[num8].material = m_MaterialReferences[0].material;
							textInfo.textElementInfo[num8].materialReferenceIndex = 0;
						}
						else
						{
							Debug.LogWarning("Unable to use Ellipsis character since it wasn't found in the current Font Asset [" + generationSettings.fontAsset.name + "]. Consider regenerating this font asset to include the Ellipsis character (u+2026).");
						}
						m_TotalCharacterCount = num8 + 1;
						GenerateTextMesh(generationSettings, textInfo);
						return;
					case TextOverflowMode.Masking:
						if (!m_IsMaskingEnabled)
						{
							EnableMasking();
						}
						break;
					case TextOverflowMode.ScrollRect:
						if (!m_IsMaskingEnabled)
						{
							EnableMasking();
						}
						break;
					case TextOverflowMode.Truncate:
						if (m_IsMaskingEnabled)
						{
							DisableMasking();
						}
						textInfo.textElementInfo[m_CharacterCount].isVisible = false;
						break;
					}
				}
				if (num13 == 9 || num13 == 160 || num13 == 8199)
				{
					textInfo.textElementInfo[m_CharacterCount].isVisible = false;
					m_LastVisibleCharacterOfLine = m_CharacterCount;
					textInfo.lineInfo[m_LineNumber].spaceCount++;
					textInfo.spaceCount++;
				}
				else
				{
					Color32 vertexColor = ((!generationSettings.overrideRichTextColors) ? m_HtmlColor : m_FontColor32);
					if (m_TextElementType == TextElementType.Character)
					{
						SaveGlyphVertexInfo(num3, num19, vertexColor, generationSettings, textInfo);
					}
					else if (m_TextElementType == TextElementType.Sprite)
					{
						SaveSpriteVertexInfo(vertexColor, generationSettings, textInfo);
					}
				}
				if (textInfo.textElementInfo[m_CharacterCount].isVisible && num13 != 173)
				{
					if (flag4)
					{
						flag4 = false;
						m_FirstVisibleCharacterOfLine = m_CharacterCount;
					}
					m_LineVisibleCharacterCount++;
					m_LastVisibleCharacterOfLine = m_CharacterCount;
				}
			}
			else if ((num13 == 10 || char.IsSeparator((char)num13)) && num13 != 173 && num13 != 8203 && num13 != 8288)
			{
				textInfo.lineInfo[m_LineNumber].spaceCount++;
				textInfo.spaceCount++;
				if (num13 == 160)
				{
					textInfo.lineInfo[m_LineNumber].controlCharacterCount = 1;
				}
			}
			if (m_LineNumber > 0 && !TextGeneratorUtilities.Approximately(m_MaxLineAscender, m_StartOfLineAscender) && m_LineHeight == -32767f && !m_IsNewPage)
			{
				float num34 = m_MaxLineAscender - m_StartOfLineAscender;
				TextGeneratorUtilities.AdjustLineOffset(m_FirstCharacterOfLine, m_CharacterCount, num34, textInfo);
				num25 -= num34;
				m_LineOffset += num34;
				m_StartOfLineAscender += num34;
				m_SavedWordWrapState.lineOffset = m_LineOffset;
				m_SavedWordWrapState.previousLineAscender = m_StartOfLineAscender;
			}
			textInfo.textElementInfo[m_CharacterCount].lineNumber = m_LineNumber;
			textInfo.textElementInfo[m_CharacterCount].pageNumber = m_PageNumber;
			if ((num13 != 10 && num13 != 13 && num13 != 8230) || textInfo.lineInfo[m_LineNumber].characterCount == 1)
			{
				textInfo.lineInfo[m_LineNumber].alignment = m_LineJustification;
			}
			if (m_MaxAscender - num25 > marginHeight + 0.0001f)
			{
				if (generationSettings.autoSize && m_LineSpacingDelta > generationSettings.lineSpacingMax && m_LineNumber > 0)
				{
					m_LoopCountA = 0;
					m_LineSpacingDelta -= 1f;
					GenerateTextMesh(generationSettings, textInfo);
					return;
				}
				if (generationSettings.autoSize && m_FontSize > generationSettings.fontSizeMin)
				{
					m_MaxFontSize = m_FontSize;
					m_FontSize -= Mathf.Max((m_FontSize - m_MinFontSize) / 2f, 0.05f);
					m_FontSize = (float)(int)(Mathf.Max(m_FontSize, generationSettings.fontSizeMin) * 20f + 0.5f) / 20f;
					if (m_LoopCountA <= 20)
					{
						GenerateTextMesh(generationSettings, textInfo);
					}
					return;
				}
				if (m_FirstOverflowCharacterIndex == -1)
				{
					m_FirstOverflowCharacterIndex = m_CharacterCount;
				}
				switch (generationSettings.overflowMode)
				{
				case TextOverflowMode.Overflow:
					if (m_IsMaskingEnabled)
					{
						DisableMasking();
					}
					break;
				case TextOverflowMode.Ellipsis:
					if (m_IsMaskingEnabled)
					{
						DisableMasking();
					}
					if (m_LineNumber > 0)
					{
						m_CharBuffer[textInfo.textElementInfo[num8].index] = 8230;
						m_CharBuffer[textInfo.textElementInfo[num8].index + 1] = 0;
						if (m_CachedEllipsisGlyphInfo != null)
						{
							textInfo.textElementInfo[num8].character = '…';
							textInfo.textElementInfo[num8].textElement = m_CachedEllipsisGlyphInfo;
							textInfo.textElementInfo[num8].fontAsset = m_MaterialReferences[0].fontAsset;
							textInfo.textElementInfo[num8].material = m_MaterialReferences[0].material;
							textInfo.textElementInfo[num8].materialReferenceIndex = 0;
						}
						else
						{
							Debug.LogWarning("Unable to use Ellipsis character since it wasn't found in the current Font Asset [" + generationSettings.fontAsset.name + "]. Consider regenerating this font asset to include the Ellipsis character (u+2026).");
						}
						m_TotalCharacterCount = num8 + 1;
						GenerateTextMesh(generationSettings, textInfo);
					}
					else
					{
						ClearMesh(updateMesh: false, textInfo);
					}
					return;
				case TextOverflowMode.Masking:
					if (!m_IsMaskingEnabled)
					{
						EnableMasking();
					}
					break;
				case TextOverflowMode.ScrollRect:
					if (!m_IsMaskingEnabled)
					{
						EnableMasking();
					}
					break;
				case TextOverflowMode.Truncate:
					if (m_IsMaskingEnabled)
					{
						DisableMasking();
					}
					if (m_LineNumber > 0)
					{
						m_CharBuffer[textInfo.textElementInfo[num8].index + 1] = 0;
						m_TotalCharacterCount = num8 + 1;
						GenerateTextMesh(generationSettings, textInfo);
					}
					else
					{
						ClearMesh(updateMesh: false, textInfo);
					}
					return;
				case TextOverflowMode.Page:
					if (m_IsMaskingEnabled)
					{
						DisableMasking();
					}
					if (num13 == 13 || num13 == 10)
					{
						break;
					}
					if (i == 0)
					{
						return;
					}
					if (num7 == i)
					{
						m_CharBuffer[i] = 0;
					}
					num7 = i;
					i = RestoreWordWrappingState(ref m_SavedLineState, textInfo);
					m_IsNewPage = true;
					m_XAdvance = 0f + m_TagIndent;
					m_LineOffset = 0f;
					m_MaxAscender = 0f;
					num10 = 0f;
					m_LineNumber++;
					m_PageNumber++;
					continue;
				case TextOverflowMode.Linked:
					if (m_LineNumber > 0)
					{
						m_CharBuffer[i] = 0;
						m_TotalCharacterCount = m_CharacterCount;
						GenerateTextMesh(generationSettings, textInfo);
					}
					else
					{
						ClearMesh(updateMesh: true, textInfo);
					}
					return;
				}
			}
			if (num13 == 9)
			{
				float num35 = m_CurrentFontAsset.faceInfo.tabWidth * (float)(int)m_CurrentFontAsset.tabMultiple * num2;
				float num36 = Mathf.Ceil(m_XAdvance / num35) * num35;
				m_XAdvance = ((num36 > m_XAdvance) ? num36 : (m_XAdvance + num35));
			}
			else if (m_MonoSpacing != 0f)
			{
				m_XAdvance += (m_MonoSpacing - num17 + (generationSettings.characterSpacing + m_CurrentFontAsset.regularStyleSpacing) * num2 + m_CSpacing) * (1f - m_CharWidthAdjDelta);
				if (char.IsWhiteSpace((char)num13) || num13 == 8203)
				{
					m_XAdvance += generationSettings.wordSpacing * num2;
				}
			}
			else if (!generationSettings.isRightToLeft)
			{
				float num37 = 1f;
				if (m_IsFxMatrixSet)
				{
					num37 = m_FxMatrix.m00;
				}
				m_XAdvance += ((m_CachedTextElement.glyph.metrics.horizontalAdvance * num37 * num4 + generationSettings.characterSpacing + m_CurrentFontAsset.regularStyleSpacing + glyphValueRecord.xAdvance) * num2 + m_CSpacing) * (1f - m_CharWidthAdjDelta);
				if (char.IsWhiteSpace((char)num13) || num13 == 8203)
				{
					m_XAdvance += generationSettings.wordSpacing * num2;
				}
			}
			else
			{
				m_XAdvance -= glyphValueRecord.xAdvance * num2;
			}
			textInfo.textElementInfo[m_CharacterCount].xAdvance = m_XAdvance;
			if (num13 == 13)
			{
				m_XAdvance = 0f + m_TagIndent;
			}
			if (num13 == 10 || m_CharacterCount == totalCharacterCount - 1)
			{
				if (m_LineNumber > 0 && !TextGeneratorUtilities.Approximately(m_MaxLineAscender, m_StartOfLineAscender) && m_LineHeight == -32767f && !m_IsNewPage)
				{
					float num38 = m_MaxLineAscender - m_StartOfLineAscender;
					TextGeneratorUtilities.AdjustLineOffset(m_FirstCharacterOfLine, m_CharacterCount, num38, textInfo);
					m_LineOffset += num38;
				}
				m_IsNewPage = false;
				float num39 = m_MaxLineAscender - m_LineOffset;
				float num40 = m_MaxLineDescender - m_LineOffset;
				m_MaxDescender = ((m_MaxDescender < num40) ? m_MaxDescender : num40);
				if (!flag5)
				{
					num11 = m_MaxDescender;
				}
				if (generationSettings.useMaxVisibleDescender && (m_CharacterCount >= generationSettings.maxVisibleCharacters || m_LineNumber >= generationSettings.maxVisibleLines))
				{
					flag5 = true;
				}
				textInfo.lineInfo[m_LineNumber].firstCharacterIndex = m_FirstCharacterOfLine;
				textInfo.lineInfo[m_LineNumber].firstVisibleCharacterIndex = (m_FirstVisibleCharacterOfLine = ((m_FirstCharacterOfLine > m_FirstVisibleCharacterOfLine) ? m_FirstCharacterOfLine : m_FirstVisibleCharacterOfLine));
				textInfo.lineInfo[m_LineNumber].lastCharacterIndex = (m_LastCharacterOfLine = m_CharacterCount);
				textInfo.lineInfo[m_LineNumber].lastVisibleCharacterIndex = (m_LastVisibleCharacterOfLine = ((m_LastVisibleCharacterOfLine < m_FirstVisibleCharacterOfLine) ? m_FirstVisibleCharacterOfLine : m_LastVisibleCharacterOfLine));
				textInfo.lineInfo[m_LineNumber].characterCount = textInfo.lineInfo[m_LineNumber].lastCharacterIndex - textInfo.lineInfo[m_LineNumber].firstCharacterIndex + 1;
				textInfo.lineInfo[m_LineNumber].visibleCharacterCount = m_LineVisibleCharacterCount;
				textInfo.lineInfo[m_LineNumber].lineExtents.min = new Vector2(textInfo.textElementInfo[m_FirstVisibleCharacterOfLine].bottomLeft.x, num40);
				textInfo.lineInfo[m_LineNumber].lineExtents.max = new Vector2(textInfo.textElementInfo[m_LastVisibleCharacterOfLine].topRight.x, num39);
				textInfo.lineInfo[m_LineNumber].length = textInfo.lineInfo[m_LineNumber].lineExtents.max.x - num3 * num2;
				textInfo.lineInfo[m_LineNumber].width = num9;
				if (textInfo.lineInfo[m_LineNumber].characterCount == 1)
				{
					textInfo.lineInfo[m_LineNumber].alignment = m_LineJustification;
				}
				if (textInfo.textElementInfo[m_LastVisibleCharacterOfLine].isVisible)
				{
					textInfo.lineInfo[m_LineNumber].maxAdvance = textInfo.textElementInfo[m_LastVisibleCharacterOfLine].xAdvance - (generationSettings.characterSpacing + m_CurrentFontAsset.regularStyleSpacing) * num2 - m_CSpacing;
				}
				else
				{
					textInfo.lineInfo[m_LineNumber].maxAdvance = textInfo.textElementInfo[m_LastCharacterOfLine].xAdvance - (generationSettings.characterSpacing + m_CurrentFontAsset.regularStyleSpacing) * num2 - m_CSpacing;
				}
				textInfo.lineInfo[m_LineNumber].baseline = 0f - m_LineOffset;
				textInfo.lineInfo[m_LineNumber].ascender = num39;
				textInfo.lineInfo[m_LineNumber].descender = num40;
				textInfo.lineInfo[m_LineNumber].lineHeight = num39 - num40 + num5 * num;
				m_FirstCharacterOfLine = m_CharacterCount + 1;
				m_LineVisibleCharacterCount = 0;
				if (num13 == 10)
				{
					SaveWordWrappingState(ref m_SavedLineState, i, m_CharacterCount, textInfo);
					SaveWordWrappingState(ref m_SavedWordWrapState, i, m_CharacterCount, textInfo);
					m_LineNumber++;
					flag4 = true;
					flag7 = false;
					flag6 = true;
					if (m_LineNumber >= textInfo.lineInfo.Length)
					{
						TextGeneratorUtilities.ResizeLineExtents(m_LineNumber, textInfo);
					}
					if (m_LineHeight == -32767f)
					{
						float num33 = 0f - m_MaxLineDescender + num23 + (num5 + generationSettings.lineSpacing + generationSettings.paragraphSpacing + m_LineSpacingDelta) * num;
						m_LineOffset += num33;
					}
					else
					{
						m_LineOffset += m_LineHeight + (generationSettings.lineSpacing + generationSettings.paragraphSpacing) * num;
					}
					m_MaxLineAscender = -32767f;
					m_MaxLineDescender = 32767f;
					m_StartOfLineAscender = num23;
					m_XAdvance = 0f + m_TagLineIndent + m_TagIndent;
					num8 = m_CharacterCount - 1;
					m_CharacterCount++;
					continue;
				}
			}
			if (textInfo.textElementInfo[m_CharacterCount].isVisible)
			{
				m_MeshExtents.min.x = Mathf.Min(m_MeshExtents.min.x, textInfo.textElementInfo[m_CharacterCount].bottomLeft.x);
				m_MeshExtents.min.y = Mathf.Min(m_MeshExtents.min.y, textInfo.textElementInfo[m_CharacterCount].bottomLeft.y);
				m_MeshExtents.max.x = Mathf.Max(m_MeshExtents.max.x, textInfo.textElementInfo[m_CharacterCount].topRight.x);
				m_MeshExtents.max.y = Mathf.Max(m_MeshExtents.max.y, textInfo.textElementInfo[m_CharacterCount].topRight.y);
			}
			if (generationSettings.overflowMode == TextOverflowMode.Page && num13 != 13 && num13 != 10)
			{
				if (m_PageNumber + 1 > textInfo.pageInfo.Length)
				{
					TextInfo.Resize(ref textInfo.pageInfo, m_PageNumber + 1, isBlockAllocated: true);
				}
				textInfo.pageInfo[m_PageNumber].ascender = num10;
				textInfo.pageInfo[m_PageNumber].descender = ((num24 < textInfo.pageInfo[m_PageNumber].descender) ? num24 : textInfo.pageInfo[m_PageNumber].descender);
				if (m_PageNumber == 0 && m_CharacterCount == 0)
				{
					textInfo.pageInfo[m_PageNumber].firstCharacterIndex = m_CharacterCount;
				}
				else if (m_CharacterCount > 0 && m_PageNumber != textInfo.textElementInfo[m_CharacterCount - 1].pageNumber)
				{
					textInfo.pageInfo[m_PageNumber - 1].lastCharacterIndex = m_CharacterCount - 1;
					textInfo.pageInfo[m_PageNumber].firstCharacterIndex = m_CharacterCount;
				}
				else if (m_CharacterCount == totalCharacterCount - 1)
				{
					textInfo.pageInfo[m_PageNumber].lastCharacterIndex = m_CharacterCount;
				}
			}
			if (generationSettings.wordWrap || generationSettings.overflowMode == TextOverflowMode.Truncate || generationSettings.overflowMode == TextOverflowMode.Ellipsis)
			{
				if ((char.IsWhiteSpace((char)num13) || num13 == 8203 || num13 == 45 || num13 == 173) && (!m_IsNonBreakingSpace || flag7) && num13 != 160 && num13 != 8199 && num13 != 8209 && num13 != 8239 && num13 != 8288)
				{
					SaveWordWrappingState(ref m_SavedWordWrapState, i, m_CharacterCount, textInfo);
					m_IsCharacterWrappingEnabled = false;
					flag6 = false;
				}
				else if (((num13 > 4352 && num13 < 4607) || (num13 > 11904 && num13 < 40959) || (num13 > 43360 && num13 < 43391) || (num13 > 44032 && num13 < 55295) || (num13 > 63744 && num13 < 64255) || (num13 > 65072 && num13 < 65103) || (num13 > 65280 && num13 < 65519)) && !m_IsNonBreakingSpace)
				{
					if (flag6 || flag8 || (!TextSettings.linebreakingRules.leadingCharacters.ContainsKey(num13) && m_CharacterCount < totalCharacterCount - 1 && !TextSettings.linebreakingRules.followingCharacters.ContainsKey(textInfo.textElementInfo[m_CharacterCount + 1].character)))
					{
						SaveWordWrappingState(ref m_SavedWordWrapState, i, m_CharacterCount, textInfo);
						m_IsCharacterWrappingEnabled = false;
						flag6 = false;
					}
				}
				else if (flag6 || m_IsCharacterWrappingEnabled || flag8)
				{
					SaveWordWrappingState(ref m_SavedWordWrapState, i, m_CharacterCount, textInfo);
				}
			}
			m_CharacterCount++;
		}
		float num41 = m_MaxFontSize - m_MinFontSize;
		if (!m_IsCharacterWrappingEnabled && generationSettings.autoSize && num41 > 0.051f && m_FontSize < generationSettings.fontSizeMax)
		{
			m_MinFontSize = m_FontSize;
			m_FontSize += Mathf.Max((m_MaxFontSize - m_FontSize) / 2f, 0.05f);
			m_FontSize = (float)(int)(Mathf.Min(m_FontSize, generationSettings.fontSizeMax) * 20f + 0.5f) / 20f;
			if (m_LoopCountA <= 20)
			{
				GenerateTextMesh(generationSettings, textInfo);
			}
			return;
		}
		m_IsCharacterWrappingEnabled = false;
		if (m_CharacterCount == 0)
		{
			ClearMesh(updateMesh: true, textInfo);
			return;
		}
		int index = m_MaterialReferences[0].referenceCount * 4;
		textInfo.meshInfo[0].Clear(uploadChanges: false);
		Vector3 vector9 = Vector3.zero;
		Vector3[] rectTransformCorners = m_RectTransformCorners;
		switch (generationSettings.textAlignment)
		{
		case TextAlignment.TopLeft:
		case TextAlignment.TopCenter:
		case TextAlignment.TopRight:
		case TextAlignment.TopJustified:
		case TextAlignment.TopFlush:
		case TextAlignment.TopGeoAligned:
			vector9 = ((generationSettings.overflowMode == TextOverflowMode.Page) ? (rectTransformCorners[1] + new Vector3(0f + margins.x, 0f - textInfo.pageInfo[num6].ascender - margins.y, 0f)) : (rectTransformCorners[1] + new Vector3(0f + margins.x, 0f - m_MaxAscender - margins.y, 0f)));
			break;
		case TextAlignment.MiddleLeft:
		case TextAlignment.MiddleCenter:
		case TextAlignment.MiddleRight:
		case TextAlignment.MiddleJustified:
		case TextAlignment.MiddleFlush:
		case TextAlignment.MiddleGeoAligned:
			vector9 = ((generationSettings.overflowMode == TextOverflowMode.Page) ? ((rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(0f + margins.x, 0f - (textInfo.pageInfo[num6].ascender + margins.y + textInfo.pageInfo[num6].descender - margins.w) / 2f, 0f)) : ((rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(0f + margins.x, 0f - (m_MaxAscender + margins.y + num11 - margins.w) / 2f, 0f)));
			break;
		case TextAlignment.BottomLeft:
		case TextAlignment.BottomCenter:
		case TextAlignment.BottomRight:
		case TextAlignment.BottomJustified:
		case TextAlignment.BottomFlush:
		case TextAlignment.BottomGeoAligned:
			vector9 = ((generationSettings.overflowMode == TextOverflowMode.Page) ? (rectTransformCorners[0] + new Vector3(0f + margins.x, 0f - textInfo.pageInfo[num6].descender + margins.w, 0f)) : (rectTransformCorners[0] + new Vector3(0f + margins.x, 0f - num11 + margins.w, 0f)));
			break;
		case TextAlignment.BaselineLeft:
		case TextAlignment.BaselineCenter:
		case TextAlignment.BaselineRight:
		case TextAlignment.BaselineJustified:
		case TextAlignment.BaselineFlush:
		case TextAlignment.BaselineGeoAligned:
			vector9 = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(0f + margins.x, 0f, 0f);
			break;
		case TextAlignment.MidlineLeft:
		case TextAlignment.MidlineCenter:
		case TextAlignment.MidlineRight:
		case TextAlignment.MidlineJustified:
		case TextAlignment.MidlineFlush:
		case TextAlignment.MidlineGeoAligned:
			vector9 = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(0f + margins.x, 0f - (m_MeshExtents.max.y + margins.y + m_MeshExtents.min.y - margins.w) / 2f, 0f);
			break;
		case TextAlignment.CaplineLeft:
		case TextAlignment.CaplineCenter:
		case TextAlignment.CaplineRight:
		case TextAlignment.CaplineJustified:
		case TextAlignment.CaplineFlush:
		case TextAlignment.CaplineGeoAligned:
			vector9 = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(0f + margins.x, 0f - (m_MaxCapHeight - margins.y - margins.w) / 2f, 0f);
			break;
		}
		Vector3 vector10 = Vector3.zero;
		int num42 = 0;
		int lineCount = 0;
		int num43 = 0;
		bool flag10 = false;
		bool flag11 = false;
		int num44 = 0;
		Color32 color = Color.white;
		Color32 underlineColor = Color.white;
		Color32 color2 = new Color32(byte.MaxValue, byte.MaxValue, 0, 64);
		float num45 = 0f;
		float num46 = 0f;
		float num47 = 0f;
		float num48 = 32767f;
		int num49 = 0;
		float num50 = 0f;
		float num51 = 0f;
		float b = 0f;
		TextElementInfo[] textElementInfo = textInfo.textElementInfo;
		for (int j = 0; j < m_CharacterCount; j++)
		{
			FontAsset fontAsset = textElementInfo[j].fontAsset;
			char character3 = textElementInfo[j].character;
			int lineNumber = textElementInfo[j].lineNumber;
			LineInfo lineInfo = textInfo.lineInfo[lineNumber];
			lineCount = lineNumber + 1;
			TextAlignment alignment = lineInfo.alignment;
			switch (alignment)
			{
			case TextAlignment.TopLeft:
			case TextAlignment.MiddleLeft:
			case TextAlignment.BottomLeft:
			case TextAlignment.BaselineLeft:
			case TextAlignment.MidlineLeft:
			case TextAlignment.CaplineLeft:
				vector10 = (generationSettings.isRightToLeft ? new Vector3(0f - lineInfo.maxAdvance, 0f, 0f) : new Vector3(0f + lineInfo.marginLeft, 0f, 0f));
				break;
			case TextAlignment.TopCenter:
			case TextAlignment.MiddleCenter:
			case TextAlignment.BottomCenter:
			case TextAlignment.BaselineCenter:
			case TextAlignment.MidlineCenter:
			case TextAlignment.CaplineCenter:
				vector10 = new Vector3(lineInfo.marginLeft + lineInfo.width / 2f - lineInfo.maxAdvance / 2f, 0f, 0f);
				break;
			case TextAlignment.TopGeoAligned:
			case TextAlignment.MiddleGeoAligned:
			case TextAlignment.BottomGeoAligned:
			case TextAlignment.BaselineGeoAligned:
			case TextAlignment.MidlineGeoAligned:
			case TextAlignment.CaplineGeoAligned:
				vector10 = new Vector3(lineInfo.marginLeft + lineInfo.width / 2f - (lineInfo.lineExtents.min.x + lineInfo.lineExtents.max.x) / 2f, 0f, 0f);
				break;
			case TextAlignment.TopRight:
			case TextAlignment.MiddleRight:
			case TextAlignment.BottomRight:
			case TextAlignment.BaselineRight:
			case TextAlignment.MidlineRight:
			case TextAlignment.CaplineRight:
				vector10 = (generationSettings.isRightToLeft ? new Vector3(lineInfo.marginLeft + lineInfo.width, 0f, 0f) : new Vector3(lineInfo.marginLeft + lineInfo.width - lineInfo.maxAdvance, 0f, 0f));
				break;
			case TextAlignment.TopJustified:
			case TextAlignment.TopFlush:
			case TextAlignment.MiddleJustified:
			case TextAlignment.MiddleFlush:
			case TextAlignment.BottomJustified:
			case TextAlignment.BottomFlush:
			case TextAlignment.BaselineJustified:
			case TextAlignment.BaselineFlush:
			case TextAlignment.MidlineJustified:
			case TextAlignment.MidlineFlush:
			case TextAlignment.CaplineJustified:
			case TextAlignment.CaplineFlush:
			{
				if (character3 == '\u00ad' || character3 == '\u200b' || character3 == '\u2060')
				{
					break;
				}
				char character4 = textElementInfo[lineInfo.lastCharacterIndex].character;
				bool flag12 = (alignment & (TextAlignment)16) == (TextAlignment)16;
				if ((!char.IsControl(character4) && lineNumber < m_LineNumber) || flag12 || lineInfo.maxAdvance > lineInfo.width)
				{
					if (lineNumber != num43 || j == 0 || j == generationSettings.firstVisibleCharacter)
					{
						vector10 = (generationSettings.isRightToLeft ? new Vector3(lineInfo.marginLeft + lineInfo.width, 0f, 0f) : new Vector3(lineInfo.marginLeft, 0f, 0f));
						flag10 = (char.IsSeparator(character3) ? true : false);
						break;
					}
					float num52 = ((!generationSettings.isRightToLeft) ? (lineInfo.width - lineInfo.maxAdvance) : (lineInfo.width + lineInfo.maxAdvance));
					int num53 = lineInfo.visibleCharacterCount - 1 + lineInfo.controlCharacterCount;
					int num54 = (textElementInfo[lineInfo.lastCharacterIndex].isVisible ? lineInfo.spaceCount : (lineInfo.spaceCount - 1)) - lineInfo.controlCharacterCount;
					if (flag10)
					{
						num54--;
						num53++;
					}
					float num55 = ((num54 > 0) ? generationSettings.wordWrappingRatio : 1f);
					if (num54 < 1)
					{
						num54 = 1;
					}
					if (character3 != '\u00a0' && (character3 == '\t' || char.IsSeparator(character3)))
					{
						if (!generationSettings.isRightToLeft)
						{
							vector10 += new Vector3(num52 * (1f - num55) / (float)num54, 0f, 0f);
						}
						else
						{
							vector10 -= new Vector3(num52 * (1f - num55) / (float)num54, 0f, 0f);
						}
					}
					else if (!generationSettings.isRightToLeft)
					{
						vector10 += new Vector3(num52 * num55 / (float)num53, 0f, 0f);
					}
					else
					{
						vector10 -= new Vector3(num52 * num55 / (float)num53, 0f, 0f);
					}
				}
				else
				{
					vector10 = (generationSettings.isRightToLeft ? new Vector3(lineInfo.marginLeft + lineInfo.width, 0f, 0f) : new Vector3(lineInfo.marginLeft, 0f, 0f));
				}
				break;
			}
			}
			Vector3 vector11 = vector9 + vector10;
			if (textElementInfo[j].isVisible)
			{
				TextElementType elementType = textElementInfo[j].elementType;
				switch (elementType)
				{
				case TextElementType.Character:
				{
					Extents lineExtents = lineInfo.lineExtents;
					float num56 = generationSettings.uvLineOffset * (float)lineNumber % 1f;
					switch (generationSettings.horizontalMapping)
					{
					case TextureMapping.Character:
						textElementInfo[j].vertexBottomLeft.uv2.x = 0f;
						textElementInfo[j].vertexTopLeft.uv2.x = 0f;
						textElementInfo[j].vertexTopRight.uv2.x = 1f;
						textElementInfo[j].vertexBottomRight.uv2.x = 1f;
						break;
					case TextureMapping.Line:
						if (generationSettings.textAlignment != TextAlignment.MiddleJustified)
						{
							textElementInfo[j].vertexBottomLeft.uv2.x = (textElementInfo[j].vertexBottomLeft.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num56;
							textElementInfo[j].vertexTopLeft.uv2.x = (textElementInfo[j].vertexTopLeft.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num56;
							textElementInfo[j].vertexTopRight.uv2.x = (textElementInfo[j].vertexTopRight.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num56;
							textElementInfo[j].vertexBottomRight.uv2.x = (textElementInfo[j].vertexBottomRight.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num56;
						}
						else
						{
							textElementInfo[j].vertexBottomLeft.uv2.x = (textElementInfo[j].vertexBottomLeft.position.x + vector10.x - m_MeshExtents.min.x) / (m_MeshExtents.max.x - m_MeshExtents.min.x) + num56;
							textElementInfo[j].vertexTopLeft.uv2.x = (textElementInfo[j].vertexTopLeft.position.x + vector10.x - m_MeshExtents.min.x) / (m_MeshExtents.max.x - m_MeshExtents.min.x) + num56;
							textElementInfo[j].vertexTopRight.uv2.x = (textElementInfo[j].vertexTopRight.position.x + vector10.x - m_MeshExtents.min.x) / (m_MeshExtents.max.x - m_MeshExtents.min.x) + num56;
							textElementInfo[j].vertexBottomRight.uv2.x = (textElementInfo[j].vertexBottomRight.position.x + vector10.x - m_MeshExtents.min.x) / (m_MeshExtents.max.x - m_MeshExtents.min.x) + num56;
						}
						break;
					case TextureMapping.Paragraph:
						textElementInfo[j].vertexBottomLeft.uv2.x = (textElementInfo[j].vertexBottomLeft.position.x + vector10.x - m_MeshExtents.min.x) / (m_MeshExtents.max.x - m_MeshExtents.min.x) + num56;
						textElementInfo[j].vertexTopLeft.uv2.x = (textElementInfo[j].vertexTopLeft.position.x + vector10.x - m_MeshExtents.min.x) / (m_MeshExtents.max.x - m_MeshExtents.min.x) + num56;
						textElementInfo[j].vertexTopRight.uv2.x = (textElementInfo[j].vertexTopRight.position.x + vector10.x - m_MeshExtents.min.x) / (m_MeshExtents.max.x - m_MeshExtents.min.x) + num56;
						textElementInfo[j].vertexBottomRight.uv2.x = (textElementInfo[j].vertexBottomRight.position.x + vector10.x - m_MeshExtents.min.x) / (m_MeshExtents.max.x - m_MeshExtents.min.x) + num56;
						break;
					case TextureMapping.MatchAspect:
					{
						switch (generationSettings.verticalMapping)
						{
						case TextureMapping.Character:
							textElementInfo[j].vertexBottomLeft.uv2.y = 0f;
							textElementInfo[j].vertexTopLeft.uv2.y = 1f;
							textElementInfo[j].vertexTopRight.uv2.y = 0f;
							textElementInfo[j].vertexBottomRight.uv2.y = 1f;
							break;
						case TextureMapping.Line:
							textElementInfo[j].vertexBottomLeft.uv2.y = (textElementInfo[j].vertexBottomLeft.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num56;
							textElementInfo[j].vertexTopLeft.uv2.y = (textElementInfo[j].vertexTopLeft.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num56;
							textElementInfo[j].vertexTopRight.uv2.y = textElementInfo[j].vertexBottomLeft.uv2.y;
							textElementInfo[j].vertexBottomRight.uv2.y = textElementInfo[j].vertexTopLeft.uv2.y;
							break;
						case TextureMapping.Paragraph:
							textElementInfo[j].vertexBottomLeft.uv2.y = (textElementInfo[j].vertexBottomLeft.position.y - m_MeshExtents.min.y) / (m_MeshExtents.max.y - m_MeshExtents.min.y) + num56;
							textElementInfo[j].vertexTopLeft.uv2.y = (textElementInfo[j].vertexTopLeft.position.y - m_MeshExtents.min.y) / (m_MeshExtents.max.y - m_MeshExtents.min.y) + num56;
							textElementInfo[j].vertexTopRight.uv2.y = textElementInfo[j].vertexBottomLeft.uv2.y;
							textElementInfo[j].vertexBottomRight.uv2.y = textElementInfo[j].vertexTopLeft.uv2.y;
							break;
						case TextureMapping.MatchAspect:
							Debug.Log("ERROR: Cannot Match both Vertical & Horizontal.");
							break;
						}
						float num57 = (1f - (textElementInfo[j].vertexBottomLeft.uv2.y + textElementInfo[j].vertexTopLeft.uv2.y) * textElementInfo[j].aspectRatio) / 2f;
						textElementInfo[j].vertexBottomLeft.uv2.x = textElementInfo[j].vertexBottomLeft.uv2.y * textElementInfo[j].aspectRatio + num57 + num56;
						textElementInfo[j].vertexTopLeft.uv2.x = textElementInfo[j].vertexBottomLeft.uv2.x;
						textElementInfo[j].vertexTopRight.uv2.x = textElementInfo[j].vertexTopLeft.uv2.y * textElementInfo[j].aspectRatio + num57 + num56;
						textElementInfo[j].vertexBottomRight.uv2.x = textElementInfo[j].vertexTopRight.uv2.x;
						break;
					}
					}
					switch (generationSettings.verticalMapping)
					{
					case TextureMapping.Character:
						textElementInfo[j].vertexBottomLeft.uv2.y = 0f;
						textElementInfo[j].vertexTopLeft.uv2.y = 1f;
						textElementInfo[j].vertexTopRight.uv2.y = 1f;
						textElementInfo[j].vertexBottomRight.uv2.y = 0f;
						break;
					case TextureMapping.Line:
						textElementInfo[j].vertexBottomLeft.uv2.y = (textElementInfo[j].vertexBottomLeft.position.y - lineInfo.descender) / (lineInfo.ascender - lineInfo.descender);
						textElementInfo[j].vertexTopLeft.uv2.y = (textElementInfo[j].vertexTopLeft.position.y - lineInfo.descender) / (lineInfo.ascender - lineInfo.descender);
						textElementInfo[j].vertexTopRight.uv2.y = textElementInfo[j].vertexTopLeft.uv2.y;
						textElementInfo[j].vertexBottomRight.uv2.y = textElementInfo[j].vertexBottomLeft.uv2.y;
						break;
					case TextureMapping.Paragraph:
						textElementInfo[j].vertexBottomLeft.uv2.y = (textElementInfo[j].vertexBottomLeft.position.y - m_MeshExtents.min.y) / (m_MeshExtents.max.y - m_MeshExtents.min.y);
						textElementInfo[j].vertexTopLeft.uv2.y = (textElementInfo[j].vertexTopLeft.position.y - m_MeshExtents.min.y) / (m_MeshExtents.max.y - m_MeshExtents.min.y);
						textElementInfo[j].vertexTopRight.uv2.y = textElementInfo[j].vertexTopLeft.uv2.y;
						textElementInfo[j].vertexBottomRight.uv2.y = textElementInfo[j].vertexBottomLeft.uv2.y;
						break;
					case TextureMapping.MatchAspect:
					{
						float num58 = (1f - (textElementInfo[j].vertexBottomLeft.uv2.x + textElementInfo[j].vertexTopRight.uv2.x) / textElementInfo[j].aspectRatio) / 2f;
						textElementInfo[j].vertexBottomLeft.uv2.y = num58 + textElementInfo[j].vertexBottomLeft.uv2.x / textElementInfo[j].aspectRatio;
						textElementInfo[j].vertexTopLeft.uv2.y = num58 + textElementInfo[j].vertexTopRight.uv2.x / textElementInfo[j].aspectRatio;
						textElementInfo[j].vertexBottomRight.uv2.y = textElementInfo[j].vertexBottomLeft.uv2.y;
						textElementInfo[j].vertexTopRight.uv2.y = textElementInfo[j].vertexTopLeft.uv2.y;
						break;
					}
					}
					num45 = textElementInfo[j].scale * (1f - m_CharWidthAdjDelta) * 1f;
					if (!textElementInfo[j].isUsingAlternateTypeface && (textElementInfo[j].style & FontStyles.Bold) == FontStyles.Bold)
					{
						num45 *= -1f;
					}
					textElementInfo[j].vertexBottomLeft.uv2.x = 1f;
					textElementInfo[j].vertexBottomLeft.uv2.y = num45;
					textElementInfo[j].vertexTopLeft.uv2.x = 1f;
					textElementInfo[j].vertexTopLeft.uv2.y = num45;
					textElementInfo[j].vertexTopRight.uv2.x = 1f;
					textElementInfo[j].vertexTopRight.uv2.y = num45;
					textElementInfo[j].vertexBottomRight.uv2.x = 1f;
					textElementInfo[j].vertexBottomRight.uv2.y = num45;
					break;
				}
				}
				if (j < generationSettings.maxVisibleCharacters && num42 < generationSettings.maxVisibleWords && lineNumber < generationSettings.maxVisibleLines && generationSettings.overflowMode != TextOverflowMode.Page)
				{
					textElementInfo[j].vertexBottomLeft.position += vector11;
					textElementInfo[j].vertexTopLeft.position += vector11;
					textElementInfo[j].vertexTopRight.position += vector11;
					textElementInfo[j].vertexBottomRight.position += vector11;
				}
				else if (j < generationSettings.maxVisibleCharacters && num42 < generationSettings.maxVisibleWords && lineNumber < generationSettings.maxVisibleLines && generationSettings.overflowMode == TextOverflowMode.Page && textElementInfo[j].pageNumber == num6)
				{
					textElementInfo[j].vertexBottomLeft.position += vector11;
					textElementInfo[j].vertexTopLeft.position += vector11;
					textElementInfo[j].vertexTopRight.position += vector11;
					textElementInfo[j].vertexBottomRight.position += vector11;
				}
				else
				{
					textElementInfo[j].vertexBottomLeft.position = Vector3.zero;
					textElementInfo[j].vertexTopLeft.position = Vector3.zero;
					textElementInfo[j].vertexTopRight.position = Vector3.zero;
					textElementInfo[j].vertexBottomRight.position = Vector3.zero;
					textElementInfo[j].isVisible = false;
				}
				switch (elementType)
				{
				case TextElementType.Character:
					TextGeneratorUtilities.FillCharacterVertexBuffers(j, generationSettings, textInfo);
					break;
				case TextElementType.Sprite:
					TextGeneratorUtilities.FillSpriteVertexBuffers(j, generationSettings, textInfo);
					break;
				}
			}
			textInfo.textElementInfo[j].bottomLeft += vector11;
			textInfo.textElementInfo[j].topLeft += vector11;
			textInfo.textElementInfo[j].topRight += vector11;
			textInfo.textElementInfo[j].bottomRight += vector11;
			textInfo.textElementInfo[j].origin += vector11.x;
			textInfo.textElementInfo[j].xAdvance += vector11.x;
			textInfo.textElementInfo[j].ascender += vector11.y;
			textInfo.textElementInfo[j].descender += vector11.y;
			textInfo.textElementInfo[j].baseLine += vector11.y;
			if (lineNumber != num43 || j == m_CharacterCount - 1)
			{
				if (lineNumber != num43)
				{
					textInfo.lineInfo[num43].baseline += vector11.y;
					textInfo.lineInfo[num43].ascender += vector11.y;
					textInfo.lineInfo[num43].descender += vector11.y;
					textInfo.lineInfo[num43].lineExtents.min = new Vector2(textInfo.textElementInfo[textInfo.lineInfo[num43].firstCharacterIndex].bottomLeft.x, textInfo.lineInfo[num43].descender);
					textInfo.lineInfo[num43].lineExtents.max = new Vector2(textInfo.textElementInfo[textInfo.lineInfo[num43].lastVisibleCharacterIndex].topRight.x, textInfo.lineInfo[num43].ascender);
				}
				if (j == m_CharacterCount - 1)
				{
					textInfo.lineInfo[lineNumber].baseline += vector11.y;
					textInfo.lineInfo[lineNumber].ascender += vector11.y;
					textInfo.lineInfo[lineNumber].descender += vector11.y;
					textInfo.lineInfo[lineNumber].lineExtents.min = new Vector2(textInfo.textElementInfo[textInfo.lineInfo[lineNumber].firstCharacterIndex].bottomLeft.x, textInfo.lineInfo[lineNumber].descender);
					textInfo.lineInfo[lineNumber].lineExtents.max = new Vector2(textInfo.textElementInfo[textInfo.lineInfo[lineNumber].lastVisibleCharacterIndex].topRight.x, textInfo.lineInfo[lineNumber].ascender);
				}
			}
			if (char.IsLetterOrDigit(character3) || character3 == '-' || character3 == '\u00ad' || character3 == '‐' || character3 == '‑')
			{
				if (!flag11)
				{
					flag11 = true;
					num44 = j;
				}
				if (flag11 && j == m_CharacterCount - 1)
				{
					int num59 = textInfo.wordInfo.Length;
					int wordCount = textInfo.wordCount;
					if (textInfo.wordCount + 1 > num59)
					{
						TextInfo.Resize(ref textInfo.wordInfo, num59 + 1);
					}
					int num60 = j;
					textInfo.wordInfo[wordCount].firstCharacterIndex = num44;
					textInfo.wordInfo[wordCount].lastCharacterIndex = num60;
					textInfo.wordInfo[wordCount].characterCount = num60 - num44 + 1;
					num42++;
					textInfo.wordCount++;
					textInfo.lineInfo[lineNumber].wordCount++;
				}
			}
			else if ((flag11 || (j == 0 && (!char.IsPunctuation(character3) || char.IsWhiteSpace(character3) || character3 == '\u200b' || j == m_CharacterCount - 1))) && (j <= 0 || j >= textElementInfo.Length - 1 || j >= m_CharacterCount || (character3 != '\'' && character3 != '’') || !char.IsLetterOrDigit(textElementInfo[j - 1].character) || !char.IsLetterOrDigit(textElementInfo[j + 1].character)))
			{
				int num60 = ((j == m_CharacterCount - 1 && char.IsLetterOrDigit(character3)) ? j : (j - 1));
				flag11 = false;
				int num61 = textInfo.wordInfo.Length;
				int wordCount2 = textInfo.wordCount;
				if (textInfo.wordCount + 1 > num61)
				{
					TextInfo.Resize(ref textInfo.wordInfo, num61 + 1);
				}
				textInfo.wordInfo[wordCount2].firstCharacterIndex = num44;
				textInfo.wordInfo[wordCount2].lastCharacterIndex = num60;
				textInfo.wordInfo[wordCount2].characterCount = num60 - num44 + 1;
				num42++;
				textInfo.wordCount++;
				textInfo.lineInfo[lineNumber].wordCount++;
			}
			if ((textInfo.textElementInfo[j].style & FontStyles.Underline) == FontStyles.Underline)
			{
				bool flag13 = true;
				int pageNumber = textInfo.textElementInfo[j].pageNumber;
				if (j > generationSettings.maxVisibleCharacters || lineNumber > generationSettings.maxVisibleLines || (generationSettings.overflowMode == TextOverflowMode.Page && pageNumber + 1 != generationSettings.pageToDisplay))
				{
					flag13 = false;
				}
				if (!char.IsWhiteSpace(character3) && character3 != '\u200b')
				{
					num47 = Mathf.Max(num47, textInfo.textElementInfo[j].scale);
					num48 = Mathf.Min((pageNumber == num49) ? num48 : 32767f, textInfo.textElementInfo[j].baseLine + generationSettings.fontAsset.faceInfo.underlineOffset * num47);
					num49 = pageNumber;
				}
				if (!flag && flag13 && j <= lineInfo.lastVisibleCharacterIndex && character3 != '\n' && character3 != '\r' && (j != lineInfo.lastVisibleCharacterIndex || !char.IsSeparator(character3)))
				{
					flag = true;
					num46 = textInfo.textElementInfo[j].scale;
					if (num47 == 0f)
					{
						num47 = num46;
					}
					start = new Vector3(textInfo.textElementInfo[j].bottomLeft.x, num48, 0f);
					color = textInfo.textElementInfo[j].underlineColor;
				}
				if (flag && m_CharacterCount == 1)
				{
					flag = false;
					Vector3 end = new Vector3(textInfo.textElementInfo[j].topRight.x, num48, 0f);
					float scale = textInfo.textElementInfo[j].scale;
					DrawUnderlineMesh(start, end, ref index, num46, scale, num47, num45, color, generationSettings, textInfo);
					num47 = 0f;
					num48 = 32767f;
				}
				else if (flag && (j == lineInfo.lastCharacterIndex || j >= lineInfo.lastVisibleCharacterIndex))
				{
					Vector3 end;
					float scale;
					if (char.IsWhiteSpace(character3) || character3 == '\u200b')
					{
						int lastVisibleCharacterIndex = lineInfo.lastVisibleCharacterIndex;
						end = new Vector3(textInfo.textElementInfo[lastVisibleCharacterIndex].topRight.x, num48, 0f);
						scale = textInfo.textElementInfo[lastVisibleCharacterIndex].scale;
					}
					else
					{
						end = new Vector3(textInfo.textElementInfo[j].topRight.x, num48, 0f);
						scale = textInfo.textElementInfo[j].scale;
					}
					flag = false;
					DrawUnderlineMesh(start, end, ref index, num46, scale, num47, num45, color, generationSettings, textInfo);
					num47 = 0f;
					num48 = 32767f;
				}
				else if (flag && !flag13)
				{
					flag = false;
					Vector3 end = new Vector3(textInfo.textElementInfo[j - 1].topRight.x, num48, 0f);
					float scale = textInfo.textElementInfo[j - 1].scale;
					DrawUnderlineMesh(start, end, ref index, num46, scale, num47, num45, color, generationSettings, textInfo);
					num47 = 0f;
					num48 = 32767f;
				}
				else if (flag && j < m_CharacterCount - 1 && !ColorUtilities.CompareColors(color, textInfo.textElementInfo[j + 1].underlineColor))
				{
					flag = false;
					Vector3 end = new Vector3(textInfo.textElementInfo[j].topRight.x, num48, 0f);
					float scale = textInfo.textElementInfo[j].scale;
					DrawUnderlineMesh(start, end, ref index, num46, scale, num47, num45, color, generationSettings, textInfo);
					num47 = 0f;
					num48 = 32767f;
				}
			}
			else if (flag)
			{
				flag = false;
				Vector3 end = new Vector3(textInfo.textElementInfo[j - 1].topRight.x, num48, 0f);
				float scale = textInfo.textElementInfo[j - 1].scale;
				DrawUnderlineMesh(start, end, ref index, num46, scale, num47, num45, color, generationSettings, textInfo);
				num47 = 0f;
				num48 = 32767f;
			}
			bool flag14 = (textInfo.textElementInfo[j].style & FontStyles.Strikethrough) == FontStyles.Strikethrough;
			float strikethroughOffset = fontAsset.faceInfo.strikethroughOffset;
			if (flag14)
			{
				bool flag15 = j <= generationSettings.maxVisibleCharacters && lineNumber <= generationSettings.maxVisibleLines && (generationSettings.overflowMode != TextOverflowMode.Page || textInfo.textElementInfo[j].pageNumber + 1 == generationSettings.pageToDisplay);
				if (!flag2 && flag15 && j <= lineInfo.lastVisibleCharacterIndex && character3 != '\n' && character3 != '\r' && (j != lineInfo.lastVisibleCharacterIndex || !char.IsSeparator(character3)))
				{
					flag2 = true;
					num50 = textInfo.textElementInfo[j].pointSize;
					num51 = textInfo.textElementInfo[j].scale;
					start2 = new Vector3(textInfo.textElementInfo[j].bottomLeft.x, textInfo.textElementInfo[j].baseLine + strikethroughOffset * num51, 0f);
					underlineColor = textInfo.textElementInfo[j].strikethroughColor;
					b = textInfo.textElementInfo[j].baseLine;
				}
				if (flag2 && m_CharacterCount == 1)
				{
					flag2 = false;
					Vector3 end2 = new Vector3(textInfo.textElementInfo[j].topRight.x, textInfo.textElementInfo[j].baseLine + strikethroughOffset * num51, 0f);
					DrawUnderlineMesh(start2, end2, ref index, num51, num51, num51, num45, underlineColor, generationSettings, textInfo);
				}
				else if (flag2 && j == lineInfo.lastCharacterIndex)
				{
					Vector3 end2;
					if (char.IsWhiteSpace(character3) || character3 == '\u200b')
					{
						int lastVisibleCharacterIndex2 = lineInfo.lastVisibleCharacterIndex;
						end2 = new Vector3(textInfo.textElementInfo[lastVisibleCharacterIndex2].topRight.x, textInfo.textElementInfo[lastVisibleCharacterIndex2].baseLine + strikethroughOffset * num51, 0f);
					}
					else
					{
						end2 = new Vector3(textInfo.textElementInfo[j].topRight.x, textInfo.textElementInfo[j].baseLine + strikethroughOffset * num51, 0f);
					}
					flag2 = false;
					DrawUnderlineMesh(start2, end2, ref index, num51, num51, num51, num45, underlineColor, generationSettings, textInfo);
				}
				else if (flag2 && j < m_CharacterCount && (textInfo.textElementInfo[j + 1].pointSize != num50 || !TextGeneratorUtilities.Approximately(textInfo.textElementInfo[j + 1].baseLine + vector11.y, b)))
				{
					flag2 = false;
					int lastVisibleCharacterIndex3 = lineInfo.lastVisibleCharacterIndex;
					Vector3 end2 = ((j <= lastVisibleCharacterIndex3) ? new Vector3(textInfo.textElementInfo[j].topRight.x, textInfo.textElementInfo[j].baseLine + strikethroughOffset * num51, 0f) : new Vector3(textInfo.textElementInfo[lastVisibleCharacterIndex3].topRight.x, textInfo.textElementInfo[lastVisibleCharacterIndex3].baseLine + strikethroughOffset * num51, 0f));
					DrawUnderlineMesh(start2, end2, ref index, num51, num51, num51, num45, underlineColor, generationSettings, textInfo);
				}
				else if (flag2 && j < m_CharacterCount && fontAsset.GetInstanceID() != textElementInfo[j + 1].fontAsset.GetInstanceID())
				{
					flag2 = false;
					Vector3 end2 = new Vector3(textInfo.textElementInfo[j].topRight.x, textInfo.textElementInfo[j].baseLine + strikethroughOffset * num51, 0f);
					DrawUnderlineMesh(start2, end2, ref index, num51, num51, num51, num45, underlineColor, generationSettings, textInfo);
				}
				else if (flag2 && !flag15)
				{
					flag2 = false;
					Vector3 end2 = new Vector3(textInfo.textElementInfo[j - 1].topRight.x, textInfo.textElementInfo[j - 1].baseLine + strikethroughOffset * num51, 0f);
					DrawUnderlineMesh(start2, end2, ref index, num51, num51, num51, num45, underlineColor, generationSettings, textInfo);
				}
			}
			else if (flag2)
			{
				flag2 = false;
				Vector3 end2 = new Vector3(textInfo.textElementInfo[j - 1].topRight.x, textInfo.textElementInfo[j - 1].baseLine + strikethroughOffset * num51, 0f);
				DrawUnderlineMesh(start2, end2, ref index, num51, num51, num51, num45, underlineColor, generationSettings, textInfo);
			}
			if ((textInfo.textElementInfo[j].style & FontStyles.Highlight) == FontStyles.Highlight)
			{
				bool flag16 = true;
				int pageNumber2 = textInfo.textElementInfo[j].pageNumber;
				if (j > generationSettings.maxVisibleCharacters || lineNumber > generationSettings.maxVisibleLines || (generationSettings.overflowMode == TextOverflowMode.Page && pageNumber2 + 1 != generationSettings.pageToDisplay))
				{
					flag16 = false;
				}
				if (!flag3 && flag16 && j <= lineInfo.lastVisibleCharacterIndex && character3 != '\n' && character3 != '\r' && (j != lineInfo.lastVisibleCharacterIndex || !char.IsSeparator(character3)))
				{
					flag3 = true;
					start3 = TextGeneratorUtilities.largePositiveVector2;
					vector = TextGeneratorUtilities.largeNegativeVector2;
					color2 = textInfo.textElementInfo[j].highlightColor;
				}
				if (flag3)
				{
					Color32 highlightColor = textInfo.textElementInfo[j].highlightColor;
					bool flag17 = false;
					if (!ColorUtilities.CompareColors(color2, highlightColor))
					{
						vector.x = (vector.x + textInfo.textElementInfo[j].bottomLeft.x) / 2f;
						start3.y = Mathf.Min(start3.y, textInfo.textElementInfo[j].descender);
						vector.y = Mathf.Max(vector.y, textInfo.textElementInfo[j].ascender);
						DrawTextHighlight(start3, vector, ref index, color2, generationSettings, textInfo);
						flag3 = true;
						start3 = vector;
						vector = new Vector3(textInfo.textElementInfo[j].topRight.x, textInfo.textElementInfo[j].descender, 0f);
						color2 = textInfo.textElementInfo[j].highlightColor;
						flag17 = true;
					}
					if (!flag17)
					{
						start3.x = Mathf.Min(start3.x, textInfo.textElementInfo[j].bottomLeft.x);
						start3.y = Mathf.Min(start3.y, textInfo.textElementInfo[j].descender);
						vector.x = Mathf.Max(vector.x, textInfo.textElementInfo[j].topRight.x);
						vector.y = Mathf.Max(vector.y, textInfo.textElementInfo[j].ascender);
					}
				}
				if (flag3 && m_CharacterCount == 1)
				{
					flag3 = false;
					DrawTextHighlight(start3, vector, ref index, color2, generationSettings, textInfo);
				}
				else if (flag3 && (j == lineInfo.lastCharacterIndex || j >= lineInfo.lastVisibleCharacterIndex))
				{
					flag3 = false;
					DrawTextHighlight(start3, vector, ref index, color2, generationSettings, textInfo);
				}
				else if (flag3 && !flag16)
				{
					flag3 = false;
					DrawTextHighlight(start3, vector, ref index, color2, generationSettings, textInfo);
				}
			}
			else if (flag3)
			{
				flag3 = false;
				DrawTextHighlight(start3, vector, ref index, color2, generationSettings, textInfo);
			}
			num43 = lineNumber;
		}
		textInfo.characterCount = m_CharacterCount;
		textInfo.spriteCount = m_SpriteCount;
		textInfo.lineCount = lineCount;
		textInfo.wordCount = ((num42 == 0 || m_CharacterCount <= 0) ? 1 : num42);
		textInfo.pageCount = m_PageNumber + 1;
		if (generationSettings.geometrySortingOrder != VertexSortingOrder.Normal)
		{
			textInfo.meshInfo[0].SortGeometry(VertexSortingOrder.Reverse);
		}
		for (int k = 1; k < textInfo.materialCount; k++)
		{
			textInfo.meshInfo[k].ClearUnusedVertices();
			if (generationSettings.geometrySortingOrder != VertexSortingOrder.Normal)
			{
				textInfo.meshInfo[k].SortGeometry(VertexSortingOrder.Reverse);
			}
		}
	}

	private void SaveWordWrappingState(ref WordWrapState state, int index, int count, TextInfo textInfo)
	{
		state.currentFontAsset = m_CurrentFontAsset;
		state.currentSpriteAsset = m_CurrentSpriteAsset;
		state.currentMaterial = m_CurrentMaterial;
		state.currentMaterialIndex = m_CurrentMaterialIndex;
		state.previousWordBreak = index;
		state.totalCharacterCount = count;
		state.visibleCharacterCount = m_LineVisibleCharacterCount;
		state.visibleLinkCount = textInfo.linkCount;
		state.firstCharacterIndex = m_FirstCharacterOfLine;
		state.firstVisibleCharacterIndex = m_FirstVisibleCharacterOfLine;
		state.lastVisibleCharIndex = m_LastVisibleCharacterOfLine;
		state.fontStyle = m_FontStyleInternal;
		state.fontScale = m_FontScale;
		state.fontScaleMultiplier = m_FontScaleMultiplier;
		state.currentFontSize = m_CurrentFontSize;
		state.xAdvance = m_XAdvance;
		state.maxCapHeight = m_MaxCapHeight;
		state.maxAscender = m_MaxAscender;
		state.maxDescender = m_MaxDescender;
		state.maxLineAscender = m_MaxLineAscender;
		state.maxLineDescender = m_MaxLineDescender;
		state.previousLineAscender = m_StartOfLineAscender;
		state.preferredWidth = m_PreferredWidth;
		state.preferredHeight = m_PreferredHeight;
		state.meshExtents = m_MeshExtents;
		state.lineNumber = m_LineNumber;
		state.lineOffset = m_LineOffset;
		state.baselineOffset = m_BaselineOffset;
		state.vertexColor = m_HtmlColor;
		state.underlineColor = m_UnderlineColor;
		state.strikethroughColor = m_StrikethroughColor;
		state.highlightColor = m_HighlightColor;
		state.isNonBreakingSpace = m_IsNonBreakingSpace;
		state.tagNoParsing = m_TagNoParsing;
		state.basicStyleStack = m_FontStyleStack;
		state.colorStack = m_ColorStack;
		state.underlineColorStack = m_UnderlineColorStack;
		state.strikethroughColorStack = m_StrikethroughColorStack;
		state.highlightColorStack = m_HighlightColorStack;
		state.colorGradientStack = m_ColorGradientStack;
		state.sizeStack = m_SizeStack;
		state.indentStack = m_IndentStack;
		state.fontWeightStack = m_FontWeightStack;
		state.styleStack = m_StyleStack;
		state.baselineStack = m_BaselineOffsetStack;
		state.actionStack = m_ActionStack;
		state.materialReferenceStack = m_MaterialReferenceStack;
		state.lineJustificationStack = m_LineJustificationStack;
		state.spriteAnimationId = m_SpriteAnimationId;
		if (m_LineNumber < textInfo.lineInfo.Length)
		{
			state.lineInfo = textInfo.lineInfo[m_LineNumber];
		}
	}

	protected int RestoreWordWrappingState(ref WordWrapState state, TextInfo textInfo)
	{
		int previousWordBreak = state.previousWordBreak;
		m_CurrentFontAsset = state.currentFontAsset;
		m_CurrentSpriteAsset = state.currentSpriteAsset;
		m_CurrentMaterial = state.currentMaterial;
		m_CurrentMaterialIndex = state.currentMaterialIndex;
		m_CharacterCount = state.totalCharacterCount + 1;
		m_LineVisibleCharacterCount = state.visibleCharacterCount;
		textInfo.linkCount = state.visibleLinkCount;
		m_FirstCharacterOfLine = state.firstCharacterIndex;
		m_FirstVisibleCharacterOfLine = state.firstVisibleCharacterIndex;
		m_LastVisibleCharacterOfLine = state.lastVisibleCharIndex;
		m_FontStyleInternal = state.fontStyle;
		m_FontScale = state.fontScale;
		m_FontScaleMultiplier = state.fontScaleMultiplier;
		m_CurrentFontSize = state.currentFontSize;
		m_XAdvance = state.xAdvance;
		m_MaxCapHeight = state.maxCapHeight;
		m_MaxAscender = state.maxAscender;
		m_MaxDescender = state.maxDescender;
		m_MaxLineAscender = state.maxLineAscender;
		m_MaxLineDescender = state.maxLineDescender;
		m_StartOfLineAscender = state.previousLineAscender;
		m_PreferredWidth = state.preferredWidth;
		m_PreferredHeight = state.preferredHeight;
		m_MeshExtents = state.meshExtents;
		m_LineNumber = state.lineNumber;
		m_LineOffset = state.lineOffset;
		m_BaselineOffset = state.baselineOffset;
		m_HtmlColor = state.vertexColor;
		m_UnderlineColor = state.underlineColor;
		m_StrikethroughColor = state.strikethroughColor;
		m_HighlightColor = state.highlightColor;
		m_IsNonBreakingSpace = state.isNonBreakingSpace;
		m_TagNoParsing = state.tagNoParsing;
		m_FontStyleStack = state.basicStyleStack;
		m_ColorStack = state.colorStack;
		m_UnderlineColorStack = state.underlineColorStack;
		m_StrikethroughColorStack = state.strikethroughColorStack;
		m_HighlightColorStack = state.highlightColorStack;
		m_ColorGradientStack = state.colorGradientStack;
		m_SizeStack = state.sizeStack;
		m_IndentStack = state.indentStack;
		m_FontWeightStack = state.fontWeightStack;
		m_StyleStack = state.styleStack;
		m_BaselineOffsetStack = state.baselineStack;
		m_ActionStack = state.actionStack;
		m_MaterialReferenceStack = state.materialReferenceStack;
		m_LineJustificationStack = state.lineJustificationStack;
		m_SpriteAnimationId = state.spriteAnimationId;
		if (m_LineNumber < textInfo.lineInfo.Length)
		{
			textInfo.lineInfo[m_LineNumber] = state.lineInfo;
		}
		return previousWordBreak;
	}

	private bool ValidateRichTextTag(string sourceText, ref int readIndex, ref int writeIndex, TextGenerationSettings generationSettings, TextInfo textInfo)
	{
		int num = readIndex;
		int num2 = writeIndex;
		int length = sourceText.Length;
		bool flag = false;
		byte b = 0;
		bool flag2 = false;
		int num3 = 0;
		m_Attributes[num3].nameHashCode = 0;
		m_Attributes[num3].valueHashCode = 0;
		m_Attributes[num3].valueLength = 0;
		for (; readIndex < length && sourceText[readIndex] != 0; readIndex++)
		{
			uint num4 = sourceText[readIndex];
			if (writeIndex == m_InternalTextParsingBuffer.Length)
			{
				TextGeneratorUtilities.ResizeArray(m_InternalTextParsingBuffer);
			}
			m_InternalTextParsingBuffer[writeIndex] = num4;
			writeIndex++;
			switch (num4)
			{
			case 60u:
				if (readIndex > num)
				{
					break;
				}
				continue;
			case 62u:
				flag = true;
				break;
			default:
				if (b == 0)
				{
					if ((num4 >= 65 && num4 <= 90) || (num4 >= 97 && num4 <= 122) || num4 == 47 || num4 == 45)
					{
						m_Attributes[num3].nameHashCode = ((m_Attributes[num3].nameHashCode << 5) + m_Attributes[num3].nameHashCode) ^ (int)TextUtilities.ToUpperASCIIFast(num4);
						continue;
					}
					switch (num4)
					{
					case 61u:
						b = 1;
						continue;
					case 35u:
						m_Attributes[num3].nameHashCode = 81999901;
						b = 2;
						readIndex--;
						continue;
					case 32u:
						num3++;
						m_Attributes[num3].nameHashCode = 0;
						m_Attributes[num3].valueHashCode = 0;
						m_Attributes[num3].valueLength = 0;
						continue;
					}
					break;
				}
				if (b == 1)
				{
					flag2 = false;
					b = 2;
					if (num4 == 34)
					{
						flag2 = true;
						continue;
					}
				}
				if (b != 2)
				{
					continue;
				}
				if (num4 == 34)
				{
					if (flag2)
					{
						b = 0;
						continue;
					}
					break;
				}
				if (!flag2 && num4 == 32)
				{
					num3++;
					m_Attributes[num3].nameHashCode = 0;
					m_Attributes[num3].valueHashCode = 0;
					m_Attributes[num3].valueLength = 0;
					b = 0;
				}
				else
				{
					if (m_Attributes[num3].valueLength == 0)
					{
						m_Attributes[num3].valueStartIndex = readIndex;
					}
					m_Attributes[num3].valueHashCode = ((m_Attributes[num3].valueHashCode << 5) + m_Attributes[num3].valueHashCode) ^ (int)TextUtilities.ToUpperASCIIFast(num4);
					m_Attributes[num3].valueLength++;
				}
				continue;
			}
			break;
		}
		if (!flag)
		{
			readIndex = num;
			writeIndex = num2;
			return false;
		}
		return false;
	}

	protected bool ValidateHtmlTag(int[] chars, int startIndex, out int endIndex, TextGenerationSettings generationSettings, TextInfo textInfo)
	{
		int num = 0;
		byte b = 0;
		TagUnitType tagUnitType = TagUnitType.Pixels;
		TagValueType tagValueType = TagValueType.None;
		int num2 = 0;
		m_XmlAttribute[num2].nameHashCode = 0;
		m_XmlAttribute[num2].valueType = TagValueType.None;
		m_XmlAttribute[num2].valueHashCode = 0;
		m_XmlAttribute[num2].valueStartIndex = 0;
		m_XmlAttribute[num2].valueLength = 0;
		m_XmlAttribute[1].nameHashCode = 0;
		m_XmlAttribute[2].nameHashCode = 0;
		m_XmlAttribute[3].nameHashCode = 0;
		m_XmlAttribute[4].nameHashCode = 0;
		endIndex = startIndex;
		bool flag = false;
		bool flag2 = false;
		for (int i = startIndex; i < chars.Length && chars[i] != 0; i++)
		{
			if (num >= m_RichTextTag.Length)
			{
				break;
			}
			if (chars[i] == 60)
			{
				break;
			}
			uint num3 = (uint)chars[i];
			if (num3 == 62)
			{
				flag2 = true;
				endIndex = i;
				m_RichTextTag[num] = '\0';
				break;
			}
			m_RichTextTag[num] = (char)num3;
			num++;
			if (b == 1)
			{
				switch (tagValueType)
				{
				case TagValueType.None:
					if (num3 == 43 || num3 == 45 || num3 == 46 || (num3 >= 48 && num3 <= 57))
					{
						tagValueType = TagValueType.NumericalValue;
						m_XmlAttribute[num2].valueType = TagValueType.NumericalValue;
						m_XmlAttribute[num2].valueStartIndex = num - 1;
						m_XmlAttribute[num2].valueLength++;
						break;
					}
					switch (num3)
					{
					case 35u:
						tagValueType = TagValueType.ColorValue;
						m_XmlAttribute[num2].valueType = TagValueType.ColorValue;
						m_XmlAttribute[num2].valueStartIndex = num - 1;
						m_XmlAttribute[num2].valueLength++;
						break;
					case 34u:
						tagValueType = TagValueType.StringValue;
						m_XmlAttribute[num2].valueType = TagValueType.StringValue;
						m_XmlAttribute[num2].valueStartIndex = num;
						break;
					default:
						tagValueType = TagValueType.StringValue;
						m_XmlAttribute[num2].valueType = TagValueType.StringValue;
						m_XmlAttribute[num2].valueStartIndex = num - 1;
						m_XmlAttribute[num2].valueHashCode = ((m_XmlAttribute[num2].valueHashCode << 5) + m_XmlAttribute[num2].valueHashCode) ^ (int)TextUtilities.ToUpperASCIIFast(num3);
						m_XmlAttribute[num2].valueLength++;
						break;
					}
					break;
				case TagValueType.NumericalValue:
					if (num3 == 112 || num3 == 101 || num3 == 37 || num3 == 32)
					{
						b = 2;
						tagValueType = TagValueType.None;
						num2++;
						m_XmlAttribute[num2].nameHashCode = 0;
						m_XmlAttribute[num2].valueType = TagValueType.None;
						m_XmlAttribute[num2].valueHashCode = 0;
						m_XmlAttribute[num2].valueStartIndex = 0;
						m_XmlAttribute[num2].valueLength = 0;
						switch (num3)
						{
						case 101u:
							tagUnitType = TagUnitType.FontUnits;
							break;
						case 37u:
							tagUnitType = TagUnitType.Percentage;
							break;
						}
					}
					else if (b != 2)
					{
						m_XmlAttribute[num2].valueLength++;
					}
					break;
				case TagValueType.ColorValue:
					if (num3 != 32)
					{
						m_XmlAttribute[num2].valueLength++;
						break;
					}
					b = 2;
					tagValueType = TagValueType.None;
					num2++;
					m_XmlAttribute[num2].nameHashCode = 0;
					m_XmlAttribute[num2].valueType = TagValueType.None;
					m_XmlAttribute[num2].valueHashCode = 0;
					m_XmlAttribute[num2].valueStartIndex = 0;
					m_XmlAttribute[num2].valueLength = 0;
					break;
				case TagValueType.StringValue:
					if (num3 != 34)
					{
						m_XmlAttribute[num2].valueHashCode = ((m_XmlAttribute[num2].valueHashCode << 5) + m_XmlAttribute[num2].valueHashCode) ^ (int)TextUtilities.ToUpperASCIIFast(num3);
						m_XmlAttribute[num2].valueLength++;
						break;
					}
					b = 2;
					tagValueType = TagValueType.None;
					num2++;
					m_XmlAttribute[num2].nameHashCode = 0;
					m_XmlAttribute[num2].valueType = TagValueType.None;
					m_XmlAttribute[num2].valueHashCode = 0;
					m_XmlAttribute[num2].valueStartIndex = 0;
					m_XmlAttribute[num2].valueLength = 0;
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
					return false;
				}
				flag = true;
				b = 2;
				tagValueType = TagValueType.None;
				num2++;
				m_XmlAttribute[num2].nameHashCode = 0;
				m_XmlAttribute[num2].valueType = TagValueType.None;
				m_XmlAttribute[num2].valueHashCode = 0;
				m_XmlAttribute[num2].valueStartIndex = 0;
				m_XmlAttribute[num2].valueLength = 0;
			}
			if (b == 0)
			{
				m_XmlAttribute[num2].nameHashCode = ((m_XmlAttribute[num2].nameHashCode << 5) + m_XmlAttribute[num2].nameHashCode) ^ (int)TextUtilities.ToUpperASCIIFast(num3);
			}
			if (b == 2 && num3 == 32)
			{
				b = 0;
			}
		}
		if (!flag2)
		{
			return false;
		}
		if (m_TagNoParsing && m_XmlAttribute[0].nameHashCode != -294095813)
		{
			return false;
		}
		if (m_XmlAttribute[0].nameHashCode == -294095813)
		{
			m_TagNoParsing = false;
			return true;
		}
		if (m_RichTextTag[0] == '#' && num == 4)
		{
			m_HtmlColor = TextGeneratorUtilities.HexCharsToColor(m_RichTextTag, num);
			m_ColorStack.Add(m_HtmlColor);
			return true;
		}
		if (m_RichTextTag[0] == '#' && num == 5)
		{
			m_HtmlColor = TextGeneratorUtilities.HexCharsToColor(m_RichTextTag, num);
			m_ColorStack.Add(m_HtmlColor);
			return true;
		}
		if (m_RichTextTag[0] == '#' && num == 7)
		{
			m_HtmlColor = TextGeneratorUtilities.HexCharsToColor(m_RichTextTag, num);
			m_ColorStack.Add(m_HtmlColor);
			return true;
		}
		if (m_RichTextTag[0] == '#' && num == 9)
		{
			m_HtmlColor = TextGeneratorUtilities.HexCharsToColor(m_RichTextTag, num);
			m_ColorStack.Add(m_HtmlColor);
			return true;
		}
		Material material;
		switch ((TagHashCode)m_XmlAttribute[0].nameHashCode)
		{
		case TagHashCode.BOLD:
			m_FontStyleInternal |= FontStyles.Bold;
			m_FontStyleStack.Add(FontStyles.Bold);
			m_FontWeightInternal = FontWeight.Bold;
			return true;
		case TagHashCode.SLASH_BOLD:
			if ((generationSettings.fontStyle & FontStyles.Bold) != FontStyles.Bold && m_FontStyleStack.Remove(FontStyles.Bold) == 0)
			{
				m_FontStyleInternal &= ~FontStyles.Bold;
				m_FontWeightInternal = m_FontWeightStack.Peek();
			}
			return true;
		case TagHashCode.ITALIC:
			m_FontStyleInternal |= FontStyles.Italic;
			m_FontStyleStack.Add(FontStyles.Italic);
			return true;
		case TagHashCode.SLASH_ITALIC:
			if ((generationSettings.fontStyle & FontStyles.Italic) != FontStyles.Italic && m_FontStyleStack.Remove(FontStyles.Italic) == 0)
			{
				m_FontStyleInternal &= ~FontStyles.Italic;
			}
			return true;
		case TagHashCode.STRIKETHROUGH:
			m_FontStyleInternal |= FontStyles.Strikethrough;
			m_FontStyleStack.Add(FontStyles.Strikethrough);
			if ((long)m_XmlAttribute[1].nameHashCode == 81999901)
			{
				m_StrikethroughColor = TextGeneratorUtilities.HexCharsToColor(m_RichTextTag, m_XmlAttribute[1].valueStartIndex, m_XmlAttribute[1].valueLength);
				m_StrikethroughColor.a = ((m_HtmlColor.a < m_StrikethroughColor.a) ? m_HtmlColor.a : m_StrikethroughColor.a);
			}
			else
			{
				m_StrikethroughColor = m_HtmlColor;
			}
			m_StrikethroughColorStack.Add(m_StrikethroughColor);
			return true;
		case TagHashCode.SLASH_STRIKETHROUGH:
			if ((generationSettings.fontStyle & FontStyles.Strikethrough) != FontStyles.Strikethrough && m_FontStyleStack.Remove(FontStyles.Strikethrough) == 0)
			{
				m_FontStyleInternal &= ~FontStyles.Strikethrough;
			}
			return true;
		case TagHashCode.UNDERLINE:
			m_FontStyleInternal |= FontStyles.Underline;
			m_FontStyleStack.Add(FontStyles.Underline);
			if ((long)m_XmlAttribute[1].nameHashCode == 81999901)
			{
				m_UnderlineColor = TextGeneratorUtilities.HexCharsToColor(m_RichTextTag, m_XmlAttribute[1].valueStartIndex, m_XmlAttribute[1].valueLength);
				m_UnderlineColor.a = ((m_HtmlColor.a < m_UnderlineColor.a) ? m_HtmlColor.a : m_UnderlineColor.a);
			}
			else
			{
				m_UnderlineColor = m_HtmlColor;
			}
			m_UnderlineColorStack.Add(m_UnderlineColor);
			return true;
		case TagHashCode.SLASH_UNDERLINE:
			if ((generationSettings.fontStyle & FontStyles.Underline) != FontStyles.Underline)
			{
				m_UnderlineColor = m_UnderlineColorStack.Remove();
				if (m_FontStyleStack.Remove(FontStyles.Underline) == 0)
				{
					m_FontStyleInternal &= ~FontStyles.Underline;
				}
			}
			return true;
		case TagHashCode.MARK:
			m_FontStyleInternal |= FontStyles.Highlight;
			m_FontStyleStack.Add(FontStyles.Highlight);
			m_HighlightColor = TextGeneratorUtilities.HexCharsToColor(m_RichTextTag, m_XmlAttribute[0].valueStartIndex, m_XmlAttribute[0].valueLength);
			m_HighlightColor.a = ((m_HtmlColor.a < m_HighlightColor.a) ? m_HtmlColor.a : m_HighlightColor.a);
			m_HighlightColorStack.Add(m_HighlightColor);
			return true;
		case TagHashCode.SLASH_MARK:
			if ((generationSettings.fontStyle & FontStyles.Highlight) != FontStyles.Highlight)
			{
				m_HighlightColor = m_HighlightColorStack.Remove();
				if (m_FontStyleStack.Remove(FontStyles.Highlight) == 0)
				{
					m_FontStyleInternal &= ~FontStyles.Highlight;
				}
			}
			return true;
		case TagHashCode.SUBSCRIPT:
			m_FontScaleMultiplier *= ((m_CurrentFontAsset.faceInfo.subscriptSize > 0f) ? m_CurrentFontAsset.faceInfo.subscriptSize : 1f);
			m_BaselineOffsetStack.Push(m_BaselineOffset);
			m_BaselineOffset += m_CurrentFontAsset.faceInfo.subscriptOffset * m_FontScale * m_FontScaleMultiplier;
			m_FontStyleStack.Add(FontStyles.Subscript);
			m_FontStyleInternal |= FontStyles.Subscript;
			return true;
		case TagHashCode.SLASH_SUBSCRIPT:
			if ((m_FontStyleInternal & FontStyles.Subscript) == FontStyles.Subscript)
			{
				if (m_FontScaleMultiplier < 1f)
				{
					m_BaselineOffset = m_BaselineOffsetStack.Pop();
					m_FontScaleMultiplier /= ((m_CurrentFontAsset.faceInfo.subscriptSize > 0f) ? m_CurrentFontAsset.faceInfo.subscriptSize : 1f);
				}
				if (m_FontStyleStack.Remove(FontStyles.Subscript) == 0)
				{
					m_FontStyleInternal &= ~FontStyles.Subscript;
				}
			}
			return true;
		case TagHashCode.SUPERSCRIPT:
			m_FontScaleMultiplier *= ((m_CurrentFontAsset.faceInfo.superscriptSize > 0f) ? m_CurrentFontAsset.faceInfo.superscriptSize : 1f);
			m_BaselineOffsetStack.Push(m_BaselineOffset);
			m_BaselineOffset += m_CurrentFontAsset.faceInfo.superscriptOffset * m_FontScale * m_FontScaleMultiplier;
			m_FontStyleStack.Add(FontStyles.Superscript);
			m_FontStyleInternal |= FontStyles.Superscript;
			return true;
		case TagHashCode.SLASH_SUPERSCRIPT:
			if ((m_FontStyleInternal & FontStyles.Superscript) == FontStyles.Superscript)
			{
				if (m_FontScaleMultiplier < 1f)
				{
					m_BaselineOffset = m_BaselineOffsetStack.Pop();
					m_FontScaleMultiplier /= ((m_CurrentFontAsset.faceInfo.superscriptSize > 0f) ? m_CurrentFontAsset.faceInfo.superscriptSize : 1f);
				}
				if (m_FontStyleStack.Remove(FontStyles.Superscript) == 0)
				{
					m_FontStyleInternal &= ~FontStyles.Superscript;
				}
			}
			return true;
		case TagHashCode.FONT_WEIGHT:
		{
			float num4 = TextGeneratorUtilities.ConvertToFloat(m_RichTextTag, m_XmlAttribute[0].valueStartIndex, m_XmlAttribute[0].valueLength);
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
			return true;
		}
		case TagHashCode.SLASH_FONT_WEIGHT:
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
		case TagHashCode.POSITION:
		{
			float num4 = TextGeneratorUtilities.ConvertToFloat(m_RichTextTag, m_XmlAttribute[0].valueStartIndex, m_XmlAttribute[0].valueLength);
			if (num4 == -32767f)
			{
				return false;
			}
			switch (tagUnitType)
			{
			case TagUnitType.Pixels:
				m_XAdvance = num4;
				return true;
			case TagUnitType.FontUnits:
				m_XAdvance = num4 * m_FontScale * generationSettings.fontAsset.faceInfo.tabWidth / (float)(int)generationSettings.fontAsset.tabMultiple;
				return true;
			case TagUnitType.Percentage:
				m_XAdvance = m_MarginWidth * num4 / 100f;
				return true;
			default:
				return false;
			}
		}
		case TagHashCode.SLASH_POSITION:
			return true;
		case TagHashCode.VERTICAL_OFFSET:
		{
			float num4 = TextGeneratorUtilities.ConvertToFloat(m_RichTextTag, m_XmlAttribute[0].valueStartIndex, m_XmlAttribute[0].valueLength);
			if (num4 == -32767f)
			{
				return false;
			}
			switch (tagUnitType)
			{
			case TagUnitType.Pixels:
				m_BaselineOffset = num4;
				return true;
			case TagUnitType.FontUnits:
				m_BaselineOffset = num4 * m_FontScale * generationSettings.fontAsset.faceInfo.ascentLine;
				return true;
			case TagUnitType.Percentage:
				return false;
			default:
				return false;
			}
		}
		case TagHashCode.SLASH_VERTICAL_OFFSET:
			m_BaselineOffset = 0f;
			return true;
		case TagHashCode.PAGE:
			if (generationSettings.overflowMode == TextOverflowMode.Page)
			{
				m_XAdvance = 0f + m_TagLineIndent + m_TagIndent;
				m_LineOffset = 0f;
				m_PageNumber++;
				m_IsNewPage = true;
			}
			return true;
		case TagHashCode.NO_BREAK:
			m_IsNonBreakingSpace = true;
			return true;
		case TagHashCode.SLASH_NO_BREAK:
			m_IsNonBreakingSpace = false;
			return true;
		case TagHashCode.SIZE:
		{
			float num4 = TextGeneratorUtilities.ConvertToFloat(m_RichTextTag, m_XmlAttribute[0].valueStartIndex, m_XmlAttribute[0].valueLength);
			if (num4 == -32767f)
			{
				return false;
			}
			switch (tagUnitType)
			{
			case TagUnitType.Pixels:
				if (m_RichTextTag[5] == '+')
				{
					m_CurrentFontSize = m_FontSize + num4;
					m_SizeStack.Add(m_CurrentFontSize);
					m_FontScale = m_CurrentFontSize / (float)m_CurrentFontAsset.faceInfo.pointSize * m_CurrentFontAsset.faceInfo.scale;
					return true;
				}
				if (m_RichTextTag[5] == '-')
				{
					m_CurrentFontSize = m_FontSize + num4;
					m_SizeStack.Add(m_CurrentFontSize);
					m_FontScale = m_CurrentFontSize / (float)m_CurrentFontAsset.faceInfo.pointSize * m_CurrentFontAsset.faceInfo.scale;
					return true;
				}
				m_CurrentFontSize = num4;
				m_SizeStack.Add(m_CurrentFontSize);
				m_FontScale = m_CurrentFontSize / (float)m_CurrentFontAsset.faceInfo.pointSize * m_CurrentFontAsset.faceInfo.scale;
				return true;
			case TagUnitType.FontUnits:
				m_CurrentFontSize = m_FontSize * num4;
				m_SizeStack.Add(m_CurrentFontSize);
				m_FontScale = m_CurrentFontSize / (float)m_CurrentFontAsset.faceInfo.pointSize * m_CurrentFontAsset.faceInfo.scale;
				return true;
			case TagUnitType.Percentage:
				m_CurrentFontSize = m_FontSize * num4 / 100f;
				m_SizeStack.Add(m_CurrentFontSize);
				m_FontScale = m_CurrentFontSize / (float)m_CurrentFontAsset.faceInfo.pointSize * m_CurrentFontAsset.faceInfo.scale;
				return true;
			default:
				return false;
			}
		}
		case TagHashCode.SLASH_SIZE:
			m_CurrentFontSize = m_SizeStack.Remove();
			m_FontScale = m_CurrentFontSize / (float)m_CurrentFontAsset.faceInfo.pointSize * m_CurrentFontAsset.faceInfo.scale;
			return true;
		case TagHashCode.FONT:
		{
			int valueHashCode3 = m_XmlAttribute[0].valueHashCode;
			int nameHashCode = m_XmlAttribute[1].nameHashCode;
			int valueHashCode = m_XmlAttribute[1].valueHashCode;
			if (valueHashCode3 == -620974005)
			{
				m_CurrentFontAsset = m_MaterialReferences[0].fontAsset;
				m_CurrentMaterial = m_MaterialReferences[0].material;
				m_CurrentMaterialIndex = 0;
				m_FontScale = m_CurrentFontSize / (float)m_CurrentFontAsset.faceInfo.pointSize * m_CurrentFontAsset.faceInfo.scale;
				m_MaterialReferenceStack.Add(m_MaterialReferences[0]);
				return true;
			}
			if (!MaterialReferenceManager.TryGetFontAsset(valueHashCode3, out var fontAsset))
			{
				fontAsset = Resources.Load<FontAsset>(TextSettings.defaultFontAssetPath + new string(m_RichTextTag, m_XmlAttribute[0].valueStartIndex, m_XmlAttribute[0].valueLength));
				if (fontAsset == null)
				{
					return false;
				}
				MaterialReferenceManager.AddFontAsset(fontAsset);
			}
			if (nameHashCode == 0 && valueHashCode == 0)
			{
				m_CurrentMaterial = fontAsset.material;
				m_CurrentMaterialIndex = MaterialReference.AddMaterialReference(m_CurrentMaterial, fontAsset, m_MaterialReferences, m_MaterialReferenceIndexLookup);
				m_MaterialReferenceStack.Add(m_MaterialReferences[m_CurrentMaterialIndex]);
			}
			else
			{
				if ((long)nameHashCode != 825491659)
				{
					return false;
				}
				if (MaterialReferenceManager.TryGetMaterial(valueHashCode, out material))
				{
					m_CurrentMaterial = material;
					m_CurrentMaterialIndex = MaterialReference.AddMaterialReference(m_CurrentMaterial, fontAsset, m_MaterialReferences, m_MaterialReferenceIndexLookup);
					m_MaterialReferenceStack.Add(m_MaterialReferences[m_CurrentMaterialIndex]);
				}
				else
				{
					material = Resources.Load<Material>(TextSettings.defaultFontAssetPath + new string(m_RichTextTag, m_XmlAttribute[1].valueStartIndex, m_XmlAttribute[1].valueLength));
					if (material == null)
					{
						return false;
					}
					MaterialReferenceManager.AddFontMaterial(valueHashCode, material);
					m_CurrentMaterial = material;
					m_CurrentMaterialIndex = MaterialReference.AddMaterialReference(m_CurrentMaterial, fontAsset, m_MaterialReferences, m_MaterialReferenceIndexLookup);
					m_MaterialReferenceStack.Add(m_MaterialReferences[m_CurrentMaterialIndex]);
				}
			}
			m_CurrentFontAsset = fontAsset;
			m_FontScale = m_CurrentFontSize / (float)m_CurrentFontAsset.faceInfo.pointSize * m_CurrentFontAsset.faceInfo.scale;
			return true;
		}
		case TagHashCode.SLASH_FONT:
		{
			MaterialReference materialReference2 = m_MaterialReferenceStack.Remove();
			m_CurrentFontAsset = materialReference2.fontAsset;
			m_CurrentMaterial = materialReference2.material;
			m_CurrentMaterialIndex = materialReference2.index;
			m_FontScale = m_CurrentFontSize / (float)m_CurrentFontAsset.faceInfo.pointSize * m_CurrentFontAsset.faceInfo.scale;
			return true;
		}
		case TagHashCode.MATERIAL:
		{
			int valueHashCode = m_XmlAttribute[0].valueHashCode;
			if (valueHashCode == -620974005)
			{
				m_CurrentMaterial = m_MaterialReferences[0].material;
				m_CurrentMaterialIndex = 0;
				m_MaterialReferenceStack.Add(m_MaterialReferences[0]);
				return true;
			}
			if (MaterialReferenceManager.TryGetMaterial(valueHashCode, out material))
			{
				m_CurrentMaterial = material;
				m_CurrentMaterialIndex = MaterialReference.AddMaterialReference(m_CurrentMaterial, m_CurrentFontAsset, m_MaterialReferences, m_MaterialReferenceIndexLookup);
				m_MaterialReferenceStack.Add(m_MaterialReferences[m_CurrentMaterialIndex]);
			}
			else
			{
				material = Resources.Load<Material>(TextSettings.defaultFontAssetPath + new string(m_RichTextTag, m_XmlAttribute[0].valueStartIndex, m_XmlAttribute[0].valueLength));
				if (material == null)
				{
					return false;
				}
				MaterialReferenceManager.AddFontMaterial(valueHashCode, material);
				m_CurrentMaterial = material;
				m_CurrentMaterialIndex = MaterialReference.AddMaterialReference(m_CurrentMaterial, m_CurrentFontAsset, m_MaterialReferences, m_MaterialReferenceIndexLookup);
				m_MaterialReferenceStack.Add(m_MaterialReferences[m_CurrentMaterialIndex]);
			}
			return true;
		}
		case TagHashCode.SLASH_MATERIAL:
		{
			MaterialReference materialReference = m_MaterialReferenceStack.Remove();
			m_CurrentMaterial = materialReference.material;
			m_CurrentMaterialIndex = materialReference.index;
			return true;
		}
		case TagHashCode.SPACE:
		{
			float num4 = TextGeneratorUtilities.ConvertToFloat(m_RichTextTag, m_XmlAttribute[0].valueStartIndex, m_XmlAttribute[0].valueLength);
			if (num4 == -32767f)
			{
				return false;
			}
			switch (tagUnitType)
			{
			case TagUnitType.Pixels:
				m_XAdvance += num4;
				return true;
			case TagUnitType.FontUnits:
				m_XAdvance += num4 * m_FontScale * generationSettings.fontAsset.faceInfo.tabWidth / (float)(int)generationSettings.fontAsset.tabMultiple;
				return true;
			case TagUnitType.Percentage:
				return false;
			default:
				return false;
			}
		}
		case TagHashCode.ALPHA:
			if (m_XmlAttribute[0].valueLength != 3)
			{
				return false;
			}
			m_HtmlColor.a = (byte)(TextGeneratorUtilities.HexToInt(m_RichTextTag[7]) * 16 + TextGeneratorUtilities.HexToInt(m_RichTextTag[8]));
			return true;
		case TagHashCode.A:
			return false;
		case TagHashCode.SLASH_A:
			return true;
		case TagHashCode.LINK:
			if (m_IsParsingText && !m_IsCalculatingPreferredValues)
			{
				int linkCount = textInfo.linkCount;
				if (linkCount + 1 > textInfo.linkInfo.Length)
				{
					TextInfo.Resize(ref textInfo.linkInfo, linkCount + 1);
				}
				textInfo.linkInfo[linkCount].hashCode = m_XmlAttribute[0].valueHashCode;
				textInfo.linkInfo[linkCount].linkTextfirstCharacterIndex = m_CharacterCount;
				textInfo.linkInfo[linkCount].linkIdFirstCharacterIndex = startIndex + m_XmlAttribute[0].valueStartIndex;
				textInfo.linkInfo[linkCount].linkIdLength = m_XmlAttribute[0].valueLength;
				textInfo.linkInfo[linkCount].SetLinkId(m_RichTextTag, m_XmlAttribute[0].valueStartIndex, m_XmlAttribute[0].valueLength);
			}
			return true;
		case TagHashCode.SLASH_LINK:
			if (m_IsParsingText && !m_IsCalculatingPreferredValues && textInfo.linkCount < textInfo.linkInfo.Length)
			{
				textInfo.linkInfo[textInfo.linkCount].linkTextLength = m_CharacterCount - textInfo.linkInfo[textInfo.linkCount].linkTextfirstCharacterIndex;
				textInfo.linkCount++;
			}
			return true;
		case TagHashCode.ALIGN:
			switch ((TagHashCode)m_XmlAttribute[0].valueHashCode)
			{
			case TagHashCode.LEFT:
				m_LineJustification = TextAlignment.MiddleLeft;
				m_LineJustificationStack.Add(m_LineJustification);
				return true;
			case TagHashCode.RIGHT:
				m_LineJustification = TextAlignment.MiddleRight;
				m_LineJustificationStack.Add(m_LineJustification);
				return true;
			case TagHashCode.CENTER:
				m_LineJustification = TextAlignment.MiddleCenter;
				m_LineJustificationStack.Add(m_LineJustification);
				return true;
			case TagHashCode.JUSTIFIED:
				m_LineJustification = TextAlignment.MiddleJustified;
				m_LineJustificationStack.Add(m_LineJustification);
				return true;
			case TagHashCode.FLUSH:
				m_LineJustification = TextAlignment.MiddleFlush;
				m_LineJustificationStack.Add(m_LineJustification);
				return true;
			default:
				return false;
			}
		case TagHashCode.SLASH_ALIGN:
			m_LineJustification = m_LineJustificationStack.Remove();
			return true;
		case TagHashCode.WIDTH:
		{
			float num4 = TextGeneratorUtilities.ConvertToFloat(m_RichTextTag, m_XmlAttribute[0].valueStartIndex, m_XmlAttribute[0].valueLength);
			if (num4 == -32767f)
			{
				return false;
			}
			switch (tagUnitType)
			{
			case TagUnitType.Pixels:
				m_Width = num4;
				break;
			case TagUnitType.FontUnits:
				return false;
			case TagUnitType.Percentage:
				m_Width = m_MarginWidth * num4 / 100f;
				break;
			}
			return true;
		}
		case TagHashCode.SLASH_WIDTH:
			m_Width = -1f;
			return true;
		case TagHashCode.COLOR:
			if (m_RichTextTag[6] == '#' && num == 10)
			{
				m_HtmlColor = TextGeneratorUtilities.HexCharsToColor(m_RichTextTag, num);
				m_ColorStack.Add(m_HtmlColor);
				return true;
			}
			if (m_RichTextTag[6] == '#' && num == 11)
			{
				m_HtmlColor = TextGeneratorUtilities.HexCharsToColor(m_RichTextTag, num);
				m_ColorStack.Add(m_HtmlColor);
				return true;
			}
			if (m_RichTextTag[6] == '#' && num == 13)
			{
				m_HtmlColor = TextGeneratorUtilities.HexCharsToColor(m_RichTextTag, num);
				m_ColorStack.Add(m_HtmlColor);
				return true;
			}
			if (m_RichTextTag[6] == '#' && num == 15)
			{
				m_HtmlColor = TextGeneratorUtilities.HexCharsToColor(m_RichTextTag, num);
				m_ColorStack.Add(m_HtmlColor);
				return true;
			}
			switch ((TagHashCode)m_XmlAttribute[0].valueHashCode)
			{
			case TagHashCode.RED:
				m_HtmlColor = Color.red;
				m_ColorStack.Add(m_HtmlColor);
				return true;
			case TagHashCode.BLUE:
				m_HtmlColor = Color.blue;
				m_ColorStack.Add(m_HtmlColor);
				return true;
			case TagHashCode.BLACK:
				m_HtmlColor = Color.black;
				m_ColorStack.Add(m_HtmlColor);
				return true;
			case TagHashCode.GREEN:
				m_HtmlColor = Color.green;
				m_ColorStack.Add(m_HtmlColor);
				return true;
			case TagHashCode.WHITE:
				m_HtmlColor = Color.white;
				m_ColorStack.Add(m_HtmlColor);
				return true;
			case TagHashCode.ORANGE:
				m_HtmlColor = new Color32(byte.MaxValue, 128, 0, byte.MaxValue);
				m_ColorStack.Add(m_HtmlColor);
				return true;
			case TagHashCode.PURPLE:
				m_HtmlColor = new Color32(160, 32, 240, byte.MaxValue);
				m_ColorStack.Add(m_HtmlColor);
				return true;
			case TagHashCode.YELLOW:
				m_HtmlColor = Color.yellow;
				m_ColorStack.Add(m_HtmlColor);
				return true;
			default:
				return false;
			}
		case TagHashCode.SLASH_COLOR:
			m_HtmlColor = m_ColorStack.Remove();
			return true;
		case TagHashCode.GRADIENT:
		{
			int valueHashCode5 = m_XmlAttribute[0].valueHashCode;
			if (MaterialReferenceManager.TryGetColorGradientPreset(valueHashCode5, out var gradientPreset))
			{
				m_ColorGradientPreset = gradientPreset;
			}
			else
			{
				if (gradientPreset == null)
				{
					gradientPreset = Resources.Load<TextGradientPreset>(TextSettings.defaultColorGradientPresetsPath + new string(m_RichTextTag, m_XmlAttribute[0].valueStartIndex, m_XmlAttribute[0].valueLength));
				}
				if (gradientPreset == null)
				{
					return false;
				}
				MaterialReferenceManager.AddColorGradientPreset(valueHashCode5, gradientPreset);
				m_ColorGradientPreset = gradientPreset;
			}
			m_ColorGradientStack.Add(m_ColorGradientPreset);
			return true;
		}
		case TagHashCode.SLASH_GRADIENT:
			m_ColorGradientPreset = m_ColorGradientStack.Remove();
			return true;
		case TagHashCode.CHARACTER_SPACE:
		{
			float num4 = TextGeneratorUtilities.ConvertToFloat(m_RichTextTag, m_XmlAttribute[0].valueStartIndex, m_XmlAttribute[0].valueLength);
			if (num4 == -32767f)
			{
				return false;
			}
			switch (tagUnitType)
			{
			case TagUnitType.Pixels:
				m_CSpacing = num4;
				break;
			case TagUnitType.FontUnits:
				m_CSpacing = num4;
				m_CSpacing *= m_FontScale * generationSettings.fontAsset.faceInfo.tabWidth / (float)(int)generationSettings.fontAsset.tabMultiple;
				break;
			case TagUnitType.Percentage:
				return false;
			}
			return true;
		}
		case TagHashCode.SLASH_CHARACTER_SPACE:
			if (!m_IsParsingText)
			{
				return true;
			}
			if (m_CharacterCount > 0)
			{
				m_XAdvance -= m_CSpacing;
				textInfo.textElementInfo[m_CharacterCount - 1].xAdvance = m_XAdvance;
			}
			m_CSpacing = 0f;
			return true;
		case TagHashCode.MONOSPACE:
		{
			float num4 = TextGeneratorUtilities.ConvertToFloat(m_RichTextTag, m_XmlAttribute[0].valueStartIndex, m_XmlAttribute[0].valueLength);
			if (num4 == -32767f)
			{
				return false;
			}
			switch (tagUnitType)
			{
			case TagUnitType.Pixels:
				m_MonoSpacing = num4;
				break;
			case TagUnitType.FontUnits:
				m_MonoSpacing = num4;
				m_MonoSpacing *= m_FontScale * generationSettings.fontAsset.faceInfo.tabWidth / (float)(int)generationSettings.fontAsset.tabMultiple;
				break;
			case TagUnitType.Percentage:
				return false;
			}
			return true;
		}
		case TagHashCode.SLASH_MONOSPACE:
			m_MonoSpacing = 0f;
			return true;
		case TagHashCode.CLASS:
			return false;
		case TagHashCode.INDENT:
		{
			float num4 = TextGeneratorUtilities.ConvertToFloat(m_RichTextTag, m_XmlAttribute[0].valueStartIndex, m_XmlAttribute[0].valueLength);
			if (num4 == -32767f)
			{
				return false;
			}
			switch (tagUnitType)
			{
			case TagUnitType.Pixels:
				m_TagIndent = num4;
				break;
			case TagUnitType.FontUnits:
				m_TagIndent = num4;
				m_TagIndent *= m_FontScale * generationSettings.fontAsset.faceInfo.tabWidth / (float)(int)generationSettings.fontAsset.tabMultiple;
				break;
			case TagUnitType.Percentage:
				m_TagIndent = m_MarginWidth * num4 / 100f;
				break;
			}
			m_IndentStack.Add(m_TagIndent);
			m_XAdvance = m_TagIndent;
			return true;
		}
		case TagHashCode.SLASH_INDENT:
			m_TagIndent = m_IndentStack.Remove();
			return true;
		case TagHashCode.LINE_INDENT:
		{
			float num4 = TextGeneratorUtilities.ConvertToFloat(m_RichTextTag, m_XmlAttribute[0].valueStartIndex, m_XmlAttribute[0].valueLength);
			if (num4 == -32767f)
			{
				return false;
			}
			switch (tagUnitType)
			{
			case TagUnitType.Pixels:
				m_TagLineIndent = num4;
				break;
			case TagUnitType.FontUnits:
				m_TagLineIndent = num4;
				m_TagLineIndent *= m_FontScale * generationSettings.fontAsset.faceInfo.tabWidth / (float)(int)generationSettings.fontAsset.tabMultiple;
				break;
			case TagUnitType.Percentage:
				m_TagLineIndent = m_MarginWidth * num4 / 100f;
				break;
			}
			m_XAdvance += m_TagLineIndent;
			return true;
		}
		case TagHashCode.SLASH_LINE_INDENT:
			m_TagLineIndent = 0f;
			return true;
		case TagHashCode.SPRITE:
		{
			int valueHashCode4 = m_XmlAttribute[0].valueHashCode;
			m_SpriteIndex = -1;
			TextSpriteAsset spriteAsset;
			if (m_XmlAttribute[0].valueType == TagValueType.None || m_XmlAttribute[0].valueType == TagValueType.NumericalValue)
			{
				if (generationSettings.spriteAsset != null)
				{
					m_CurrentSpriteAsset = generationSettings.spriteAsset;
				}
				else if (m_DefaultSpriteAsset != null)
				{
					m_CurrentSpriteAsset = m_DefaultSpriteAsset;
				}
				else if (m_DefaultSpriteAsset == null)
				{
					if (TextSettings.defaultSpriteAsset != null)
					{
						m_DefaultSpriteAsset = TextSettings.defaultSpriteAsset;
					}
					else
					{
						m_DefaultSpriteAsset = Resources.Load<TextSpriteAsset>("Sprite Assets/Default Sprite Asset");
					}
					m_CurrentSpriteAsset = m_DefaultSpriteAsset;
				}
				if (m_CurrentSpriteAsset == null)
				{
					return false;
				}
			}
			else if (MaterialReferenceManager.TryGetSpriteAsset(valueHashCode4, out spriteAsset))
			{
				m_CurrentSpriteAsset = spriteAsset;
			}
			else
			{
				if (spriteAsset == null)
				{
					spriteAsset = Resources.Load<TextSpriteAsset>(TextSettings.defaultSpriteAssetPath + new string(m_RichTextTag, m_XmlAttribute[0].valueStartIndex, m_XmlAttribute[0].valueLength));
				}
				if (spriteAsset == null)
				{
					return false;
				}
				MaterialReferenceManager.AddSpriteAsset(valueHashCode4, spriteAsset);
				m_CurrentSpriteAsset = spriteAsset;
			}
			if (m_XmlAttribute[0].valueType == TagValueType.NumericalValue)
			{
				int num5 = (int)TextGeneratorUtilities.ConvertToFloat(m_RichTextTag, m_XmlAttribute[0].valueStartIndex, m_XmlAttribute[0].valueLength);
				if ((float)num5 == -32767f)
				{
					return false;
				}
				if (num5 > m_CurrentSpriteAsset.spriteCharacterTable.Count - 1)
				{
					return false;
				}
				m_SpriteIndex = num5;
			}
			m_SpriteColor = Color.white;
			m_TintSprite = false;
			for (int j = 0; j < m_XmlAttribute.Length && m_XmlAttribute[j].nameHashCode != 0; j++)
			{
				int nameHashCode2 = m_XmlAttribute[j].nameHashCode;
				int spriteIndex;
				switch ((TagHashCode)nameHashCode2)
				{
				case TagHashCode.NAME:
					m_CurrentSpriteAsset = TextSpriteAsset.SearchForSpriteByHashCode(m_CurrentSpriteAsset, m_XmlAttribute[j].valueHashCode, includeFallbacks: true, out spriteIndex);
					if (spriteIndex == -1)
					{
						return false;
					}
					m_SpriteIndex = spriteIndex;
					break;
				case TagHashCode.INDEX:
					spriteIndex = (int)TextGeneratorUtilities.ConvertToFloat(m_RichTextTag, m_XmlAttribute[1].valueStartIndex, m_XmlAttribute[1].valueLength);
					if ((float)spriteIndex == -32767f)
					{
						return false;
					}
					if (spriteIndex > m_CurrentSpriteAsset.spriteCharacterTable.Count - 1)
					{
						return false;
					}
					m_SpriteIndex = spriteIndex;
					break;
				case TagHashCode.TINT:
					m_TintSprite = TextGeneratorUtilities.ConvertToFloat(m_RichTextTag, m_XmlAttribute[j].valueStartIndex, m_XmlAttribute[j].valueLength) != 0f;
					break;
				case TagHashCode.COLOR:
					m_SpriteColor = TextGeneratorUtilities.HexCharsToColor(m_RichTextTag, m_XmlAttribute[j].valueStartIndex, m_XmlAttribute[j].valueLength);
					break;
				case TagHashCode.ANIM:
					Debug.LogWarning("Sprite animations are not currently supported in TextCore");
					break;
				default:
					if (nameHashCode2 != -991527447)
					{
						return false;
					}
					break;
				}
			}
			if (m_SpriteIndex == -1)
			{
				return false;
			}
			m_CurrentMaterialIndex = MaterialReference.AddMaterialReference(m_CurrentSpriteAsset.material, m_CurrentSpriteAsset, m_MaterialReferences, m_MaterialReferenceIndexLookup);
			m_TextElementType = TextElementType.Sprite;
			return true;
		}
		case TagHashCode.LOWERCASE:
			m_FontStyleInternal |= FontStyles.LowerCase;
			m_FontStyleStack.Add(FontStyles.LowerCase);
			return true;
		case TagHashCode.SLASH_LOWERCASE:
			if ((generationSettings.fontStyle & FontStyles.LowerCase) != FontStyles.LowerCase && m_FontStyleStack.Remove(FontStyles.LowerCase) == 0)
			{
				m_FontStyleInternal &= ~FontStyles.LowerCase;
			}
			return true;
		case TagHashCode.UPPERCASE:
		case TagHashCode.ALLCAPS:
			m_FontStyleInternal |= FontStyles.UpperCase;
			m_FontStyleStack.Add(FontStyles.UpperCase);
			return true;
		case TagHashCode.SLASH_ALLCAPS:
		case TagHashCode.SLASH_UPPERCASE:
			if ((generationSettings.fontStyle & FontStyles.UpperCase) != FontStyles.UpperCase && m_FontStyleStack.Remove(FontStyles.UpperCase) == 0)
			{
				m_FontStyleInternal &= ~FontStyles.UpperCase;
			}
			return true;
		case TagHashCode.SMALLCAPS:
			m_FontStyleInternal |= FontStyles.SmallCaps;
			m_FontStyleStack.Add(FontStyles.SmallCaps);
			return true;
		case TagHashCode.SLASH_SMALLCAPS:
			if ((generationSettings.fontStyle & FontStyles.SmallCaps) != FontStyles.SmallCaps && m_FontStyleStack.Remove(FontStyles.SmallCaps) == 0)
			{
				m_FontStyleInternal &= ~FontStyles.SmallCaps;
			}
			return true;
		case TagHashCode.MARGIN:
		{
			float num4 = TextGeneratorUtilities.ConvertToFloat(m_RichTextTag, m_XmlAttribute[0].valueStartIndex, m_XmlAttribute[0].valueLength);
			if (num4 == -32767f)
			{
				return false;
			}
			m_MarginLeft = num4;
			switch (tagUnitType)
			{
			case TagUnitType.FontUnits:
				m_MarginLeft *= m_FontScale * generationSettings.fontAsset.faceInfo.tabWidth / (float)(int)generationSettings.fontAsset.tabMultiple;
				break;
			case TagUnitType.Percentage:
				m_MarginLeft = (m_MarginWidth - ((m_Width != -1f) ? m_Width : 0f)) * m_MarginLeft / 100f;
				break;
			}
			m_MarginLeft = ((m_MarginLeft >= 0f) ? m_MarginLeft : 0f);
			m_MarginRight = m_MarginLeft;
			return true;
		}
		case TagHashCode.SLASH_MARGIN:
			m_MarginLeft = 0f;
			m_MarginRight = 0f;
			return true;
		case TagHashCode.MARGIN_LEFT:
		{
			float num4 = TextGeneratorUtilities.ConvertToFloat(m_RichTextTag, m_XmlAttribute[0].valueStartIndex, m_XmlAttribute[0].valueLength);
			if (num4 == -32767f)
			{
				return false;
			}
			m_MarginLeft = num4;
			switch (tagUnitType)
			{
			case TagUnitType.FontUnits:
				m_MarginLeft *= m_FontScale * generationSettings.fontAsset.faceInfo.tabWidth / (float)(int)generationSettings.fontAsset.tabMultiple;
				break;
			case TagUnitType.Percentage:
				m_MarginLeft = (m_MarginWidth - ((m_Width != -1f) ? m_Width : 0f)) * m_MarginLeft / 100f;
				break;
			}
			m_MarginLeft = ((m_MarginLeft >= 0f) ? m_MarginLeft : 0f);
			return true;
		}
		case TagHashCode.MARGIN_RIGHT:
		{
			float num4 = TextGeneratorUtilities.ConvertToFloat(m_RichTextTag, m_XmlAttribute[0].valueStartIndex, m_XmlAttribute[0].valueLength);
			if (num4 == -32767f)
			{
				return false;
			}
			m_MarginRight = num4;
			switch (tagUnitType)
			{
			case TagUnitType.FontUnits:
				m_MarginRight *= m_FontScale * generationSettings.fontAsset.faceInfo.tabWidth / (float)(int)generationSettings.fontAsset.tabMultiple;
				break;
			case TagUnitType.Percentage:
				m_MarginRight = (m_MarginWidth - ((m_Width != -1f) ? m_Width : 0f)) * m_MarginRight / 100f;
				break;
			}
			m_MarginRight = ((m_MarginRight >= 0f) ? m_MarginRight : 0f);
			return true;
		}
		case TagHashCode.LINE_HEIGHT:
		{
			float num4 = TextGeneratorUtilities.ConvertToFloat(m_RichTextTag, m_XmlAttribute[0].valueStartIndex, m_XmlAttribute[0].valueLength);
			if (num4 == -32767f || num4 == 0f)
			{
				return false;
			}
			m_LineHeight = num4;
			switch (tagUnitType)
			{
			case TagUnitType.FontUnits:
				m_LineHeight *= generationSettings.fontAsset.faceInfo.lineHeight * m_FontScale;
				break;
			case TagUnitType.Percentage:
				m_LineHeight = generationSettings.fontAsset.faceInfo.lineHeight * m_LineHeight / 100f * m_FontScale;
				break;
			}
			return true;
		}
		case TagHashCode.SLASH_LINE_HEIGHT:
			m_LineHeight = -32767f;
			return true;
		case TagHashCode.NO_PARSE:
			m_TagNoParsing = true;
			return true;
		case TagHashCode.ACTION:
		{
			int valueHashCode2 = m_XmlAttribute[0].valueHashCode;
			if (m_IsParsingText)
			{
				m_ActionStack.Add(valueHashCode2);
			}
			return false;
		}
		case TagHashCode.SLASH_ACTION:
			m_ActionStack.Remove();
			return false;
		case TagHashCode.SCALE:
		{
			float num4 = TextGeneratorUtilities.ConvertToFloat(m_RichTextTag, m_XmlAttribute[0].valueStartIndex, m_XmlAttribute[0].valueLength);
			if (num4 == -32767f)
			{
				return false;
			}
			m_FxMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(num4, 1f, 1f));
			m_IsFxMatrixSet = true;
			return true;
		}
		case TagHashCode.SLASH_SCALE:
			m_IsFxMatrixSet = false;
			return true;
		case TagHashCode.ROTATE:
		{
			float num4 = TextGeneratorUtilities.ConvertToFloat(m_RichTextTag, m_XmlAttribute[0].valueStartIndex, m_XmlAttribute[0].valueLength);
			if (num4 == -32767f)
			{
				return false;
			}
			m_FxMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, num4), Vector3.one);
			m_IsFxMatrixSet = true;
			return true;
		}
		case TagHashCode.SLASH_ROTATE:
			m_IsFxMatrixSet = false;
			return true;
		default:
			return false;
		}
	}

	private void SaveGlyphVertexInfo(float padding, float stylePadding, Color32 vertexColor, TextGenerationSettings generationSettings, TextInfo textInfo)
	{
		textInfo.textElementInfo[m_CharacterCount].vertexBottomLeft.position = textInfo.textElementInfo[m_CharacterCount].bottomLeft;
		textInfo.textElementInfo[m_CharacterCount].vertexTopLeft.position = textInfo.textElementInfo[m_CharacterCount].topLeft;
		textInfo.textElementInfo[m_CharacterCount].vertexTopRight.position = textInfo.textElementInfo[m_CharacterCount].topRight;
		textInfo.textElementInfo[m_CharacterCount].vertexBottomRight.position = textInfo.textElementInfo[m_CharacterCount].bottomRight;
		vertexColor.a = ((m_FontColor32.a < vertexColor.a) ? m_FontColor32.a : vertexColor.a);
		if (generationSettings.fontColorGradient == null)
		{
			textInfo.textElementInfo[m_CharacterCount].vertexBottomLeft.color = vertexColor;
			textInfo.textElementInfo[m_CharacterCount].vertexTopLeft.color = vertexColor;
			textInfo.textElementInfo[m_CharacterCount].vertexTopRight.color = vertexColor;
			textInfo.textElementInfo[m_CharacterCount].vertexBottomRight.color = vertexColor;
		}
		else if (!generationSettings.overrideRichTextColors && m_ColorStack.m_Index > 1)
		{
			textInfo.textElementInfo[m_CharacterCount].vertexBottomLeft.color = vertexColor;
			textInfo.textElementInfo[m_CharacterCount].vertexTopLeft.color = vertexColor;
			textInfo.textElementInfo[m_CharacterCount].vertexTopRight.color = vertexColor;
			textInfo.textElementInfo[m_CharacterCount].vertexBottomRight.color = vertexColor;
		}
		else
		{
			textInfo.textElementInfo[m_CharacterCount].vertexBottomLeft.color = generationSettings.fontColorGradient.bottomLeft * vertexColor;
			textInfo.textElementInfo[m_CharacterCount].vertexTopLeft.color = generationSettings.fontColorGradient.topLeft * vertexColor;
			textInfo.textElementInfo[m_CharacterCount].vertexTopRight.color = generationSettings.fontColorGradient.topRight * vertexColor;
			textInfo.textElementInfo[m_CharacterCount].vertexBottomRight.color = generationSettings.fontColorGradient.bottomRight * vertexColor;
		}
		if (m_ColorGradientPreset != null)
		{
			ref Color32 color = ref textInfo.textElementInfo[m_CharacterCount].vertexBottomLeft.color;
			color *= m_ColorGradientPreset.bottomLeft;
			ref Color32 color2 = ref textInfo.textElementInfo[m_CharacterCount].vertexTopLeft.color;
			color2 *= m_ColorGradientPreset.topLeft;
			ref Color32 color3 = ref textInfo.textElementInfo[m_CharacterCount].vertexTopRight.color;
			color3 *= m_ColorGradientPreset.topRight;
			ref Color32 color4 = ref textInfo.textElementInfo[m_CharacterCount].vertexBottomRight.color;
			color4 *= m_ColorGradientPreset.bottomRight;
		}
		if (!m_IsSdfShader)
		{
			stylePadding = 0f;
		}
		Vector2 uv = default(Vector2);
		uv.x = ((float)m_CachedTextElement.glyph.glyphRect.x - padding - stylePadding) / (float)m_CurrentFontAsset.atlasWidth;
		uv.y = ((float)m_CachedTextElement.glyph.glyphRect.y - padding - stylePadding) / (float)m_CurrentFontAsset.atlasHeight;
		Vector2 uv2 = default(Vector2);
		uv2.x = uv.x;
		uv2.y = ((float)m_CachedTextElement.glyph.glyphRect.y + padding + stylePadding + (float)m_CachedTextElement.glyph.glyphRect.height) / (float)m_CurrentFontAsset.atlasHeight;
		Vector2 uv3 = default(Vector2);
		uv3.x = ((float)m_CachedTextElement.glyph.glyphRect.x + padding + stylePadding + (float)m_CachedTextElement.glyph.glyphRect.width) / (float)m_CurrentFontAsset.atlasWidth;
		uv3.y = uv2.y;
		Vector2 uv4 = default(Vector2);
		uv4.x = uv3.x;
		uv4.y = uv.y;
		textInfo.textElementInfo[m_CharacterCount].vertexBottomLeft.uv = uv;
		textInfo.textElementInfo[m_CharacterCount].vertexTopLeft.uv = uv2;
		textInfo.textElementInfo[m_CharacterCount].vertexTopRight.uv = uv3;
		textInfo.textElementInfo[m_CharacterCount].vertexBottomRight.uv = uv4;
	}

	private void SaveSpriteVertexInfo(Color32 vertexColor, TextGenerationSettings generationSettings, TextInfo textInfo)
	{
		textInfo.textElementInfo[m_CharacterCount].vertexBottomLeft.position = textInfo.textElementInfo[m_CharacterCount].bottomLeft;
		textInfo.textElementInfo[m_CharacterCount].vertexTopLeft.position = textInfo.textElementInfo[m_CharacterCount].topLeft;
		textInfo.textElementInfo[m_CharacterCount].vertexTopRight.position = textInfo.textElementInfo[m_CharacterCount].topRight;
		textInfo.textElementInfo[m_CharacterCount].vertexBottomRight.position = textInfo.textElementInfo[m_CharacterCount].bottomRight;
		if (generationSettings.tintSprites)
		{
			m_TintSprite = true;
		}
		Color32 color = (m_TintSprite ? ColorUtilities.MultiplyColors(m_SpriteColor, vertexColor) : m_SpriteColor);
		color.a = ((color.a < m_FontColor32.a) ? (color.a = ((color.a < vertexColor.a) ? color.a : vertexColor.a)) : m_FontColor32.a);
		Color32 color2 = color;
		Color32 color3 = color;
		Color32 color4 = color;
		Color32 color5 = color;
		if (generationSettings.fontColorGradient != null)
		{
			color2 = (m_TintSprite ? ColorUtilities.MultiplyColors(color2, generationSettings.fontColorGradient.bottomLeft) : color2);
			color3 = (m_TintSprite ? ColorUtilities.MultiplyColors(color3, generationSettings.fontColorGradient.topLeft) : color3);
			color4 = (m_TintSprite ? ColorUtilities.MultiplyColors(color4, generationSettings.fontColorGradient.topRight) : color4);
			color5 = (m_TintSprite ? ColorUtilities.MultiplyColors(color5, generationSettings.fontColorGradient.bottomRight) : color5);
		}
		if (m_ColorGradientPreset != null)
		{
			color2 = (m_TintSprite ? ColorUtilities.MultiplyColors(color2, m_ColorGradientPreset.bottomLeft) : color2);
			color3 = (m_TintSprite ? ColorUtilities.MultiplyColors(color3, m_ColorGradientPreset.topLeft) : color3);
			color4 = (m_TintSprite ? ColorUtilities.MultiplyColors(color4, m_ColorGradientPreset.topRight) : color4);
			color5 = (m_TintSprite ? ColorUtilities.MultiplyColors(color5, m_ColorGradientPreset.bottomRight) : color5);
		}
		textInfo.textElementInfo[m_CharacterCount].vertexBottomLeft.color = color2;
		textInfo.textElementInfo[m_CharacterCount].vertexTopLeft.color = color3;
		textInfo.textElementInfo[m_CharacterCount].vertexTopRight.color = color4;
		textInfo.textElementInfo[m_CharacterCount].vertexBottomRight.color = color5;
		Vector2 uv = new Vector2((float)m_CachedTextElement.glyph.glyphRect.x / (float)m_CurrentSpriteAsset.spriteSheet.width, (float)m_CachedTextElement.glyph.glyphRect.y / (float)m_CurrentSpriteAsset.spriteSheet.height);
		Vector2 uv2 = new Vector2(uv.x, (float)(m_CachedTextElement.glyph.glyphRect.y + m_CachedTextElement.glyph.glyphRect.height) / (float)m_CurrentSpriteAsset.spriteSheet.height);
		Vector2 uv3 = new Vector2((float)(m_CachedTextElement.glyph.glyphRect.x + m_CachedTextElement.glyph.glyphRect.width) / (float)m_CurrentSpriteAsset.spriteSheet.width, uv2.y);
		Vector2 uv4 = new Vector2(uv3.x, uv.y);
		textInfo.textElementInfo[m_CharacterCount].vertexBottomLeft.uv = uv;
		textInfo.textElementInfo[m_CharacterCount].vertexTopLeft.uv = uv2;
		textInfo.textElementInfo[m_CharacterCount].vertexTopRight.uv = uv3;
		textInfo.textElementInfo[m_CharacterCount].vertexBottomRight.uv = uv4;
	}

	private void DrawUnderlineMesh(Vector3 start, Vector3 end, ref int index, float startScale, float endScale, float maxScale, float sdfScale, Color32 underlineColor, TextGenerationSettings generationSettings, TextInfo textInfo)
	{
		if (m_CachedUnderlineGlyphInfo == null)
		{
			if (!TextSettings.warningsDisabled)
			{
				Debug.LogWarning("Unable to add underline since the Font Asset doesn't contain the underline character.");
			}
			return;
		}
		int num = index + 12;
		if (num > textInfo.meshInfo[0].vertices.Length)
		{
			textInfo.meshInfo[0].ResizeMeshInfo(num / 4);
		}
		start.y = Mathf.Min(start.y, end.y);
		end.y = Mathf.Min(start.y, end.y);
		float num2 = m_CachedUnderlineGlyphInfo.glyph.metrics.width / 2f * maxScale;
		if (end.x - start.x < m_CachedUnderlineGlyphInfo.glyph.metrics.width * maxScale)
		{
			num2 = (end.x - start.x) / 2f;
		}
		float num3 = m_Padding * startScale / maxScale;
		float num4 = m_Padding * endScale / maxScale;
		float height = m_CachedUnderlineGlyphInfo.glyph.metrics.height;
		Vector3[] vertices = textInfo.meshInfo[0].vertices;
		vertices[index] = start + new Vector3(0f, 0f - (height + m_Padding) * maxScale, 0f);
		vertices[index + 1] = start + new Vector3(0f, m_Padding * maxScale, 0f);
		vertices[index + 2] = vertices[index + 1] + new Vector3(num2, 0f, 0f);
		vertices[index + 3] = vertices[index] + new Vector3(num2, 0f, 0f);
		vertices[index + 4] = vertices[index + 3];
		vertices[index + 5] = vertices[index + 2];
		vertices[index + 6] = end + new Vector3(0f - num2, m_Padding * maxScale, 0f);
		vertices[index + 7] = end + new Vector3(0f - num2, (0f - (height + m_Padding)) * maxScale, 0f);
		vertices[index + 8] = vertices[index + 7];
		vertices[index + 9] = vertices[index + 6];
		vertices[index + 10] = end + new Vector3(0f, m_Padding * maxScale, 0f);
		vertices[index + 11] = end + new Vector3(0f, (0f - (height + m_Padding)) * maxScale, 0f);
		if (generationSettings.inverseYAxis)
		{
			Vector3 vector = default(Vector3);
			vector.x = 0f;
			vector.y = generationSettings.screenRect.y + generationSettings.screenRect.height;
			vector.z = 0f;
			vertices[index].y = vertices[index].y * -1f + vector.y;
			vertices[index + 1].y = vertices[index + 1].y * -1f + vector.y;
			vertices[index + 2].y = vertices[index + 2].y * -1f + vector.y;
			vertices[index + 3].y = vertices[index + 3].y * -1f + vector.y;
			vertices[index + 4].y = vertices[index + 4].y * -1f + vector.y;
			vertices[index + 5].y = vertices[index + 5].y * -1f + vector.y;
			vertices[index + 6].y = vertices[index + 6].y * -1f + vector.y;
			vertices[index + 7].y = vertices[index + 7].y * -1f + vector.y;
			vertices[index + 8].y = vertices[index + 8].y * -1f + vector.y;
			vertices[index + 9].y = vertices[index + 9].y * -1f + vector.y;
			vertices[index + 10].y = vertices[index + 10].y * -1f + vector.y;
			vertices[index + 11].y = vertices[index + 11].y * -1f + vector.y;
		}
		Vector2[] uvs = textInfo.meshInfo[0].uvs0;
		Vector2 vector2 = new Vector2(((float)m_CachedUnderlineGlyphInfo.glyph.glyphRect.x - num3) / (float)generationSettings.fontAsset.atlasWidth, ((float)m_CachedUnderlineGlyphInfo.glyph.glyphRect.y - m_Padding) / (float)generationSettings.fontAsset.atlasHeight);
		Vector2 vector3 = new Vector2(vector2.x, ((float)(m_CachedUnderlineGlyphInfo.glyph.glyphRect.y + m_CachedUnderlineGlyphInfo.glyph.glyphRect.height) + m_Padding) / (float)generationSettings.fontAsset.atlasHeight);
		Vector2 vector4 = new Vector2(((float)m_CachedUnderlineGlyphInfo.glyph.glyphRect.x - num3 + (float)m_CachedUnderlineGlyphInfo.glyph.glyphRect.width / 2f) / (float)generationSettings.fontAsset.atlasWidth, vector3.y);
		Vector2 vector5 = new Vector2(vector4.x, vector2.y);
		Vector2 vector6 = new Vector2(((float)m_CachedUnderlineGlyphInfo.glyph.glyphRect.x + num4 + (float)m_CachedUnderlineGlyphInfo.glyph.glyphRect.width / 2f) / (float)generationSettings.fontAsset.atlasWidth, vector3.y);
		Vector2 vector7 = new Vector2(vector6.x, vector2.y);
		Vector2 vector8 = new Vector2(((float)m_CachedUnderlineGlyphInfo.glyph.glyphRect.x + num4 + (float)m_CachedUnderlineGlyphInfo.glyph.glyphRect.width) / (float)generationSettings.fontAsset.atlasWidth, vector3.y);
		Vector2 vector9 = new Vector2(vector8.x, vector2.y);
		uvs[index] = vector2;
		uvs[1 + index] = vector3;
		uvs[2 + index] = vector4;
		uvs[3 + index] = vector5;
		uvs[4 + index] = new Vector2(vector4.x - vector4.x * 0.001f, vector2.y);
		uvs[5 + index] = new Vector2(vector4.x - vector4.x * 0.001f, vector3.y);
		uvs[6 + index] = new Vector2(vector4.x + vector4.x * 0.001f, vector3.y);
		uvs[7 + index] = new Vector2(vector4.x + vector4.x * 0.001f, vector2.y);
		uvs[8 + index] = vector7;
		uvs[9 + index] = vector6;
		uvs[10 + index] = vector8;
		uvs[11 + index] = vector9;
		float x = (vertices[index + 2].x - start.x) / (end.x - start.x);
		float scale = Mathf.Abs(sdfScale);
		Vector2[] uvs2 = textInfo.meshInfo[0].uvs2;
		uvs2[index] = TextGeneratorUtilities.PackUV(0f, 0f, scale);
		uvs2[1 + index] = TextGeneratorUtilities.PackUV(0f, 1f, scale);
		uvs2[2 + index] = TextGeneratorUtilities.PackUV(x, 1f, scale);
		uvs2[3 + index] = TextGeneratorUtilities.PackUV(x, 0f, scale);
		float x2 = (vertices[index + 4].x - start.x) / (end.x - start.x);
		x = (vertices[index + 6].x - start.x) / (end.x - start.x);
		uvs2[4 + index] = TextGeneratorUtilities.PackUV(x2, 0f, scale);
		uvs2[5 + index] = TextGeneratorUtilities.PackUV(x2, 1f, scale);
		uvs2[6 + index] = TextGeneratorUtilities.PackUV(x, 1f, scale);
		uvs2[7 + index] = TextGeneratorUtilities.PackUV(x, 0f, scale);
		x2 = (vertices[index + 8].x - start.x) / (end.x - start.x);
		uvs2[8 + index] = TextGeneratorUtilities.PackUV(x2, 0f, scale);
		uvs2[9 + index] = TextGeneratorUtilities.PackUV(x2, 1f, scale);
		uvs2[10 + index] = TextGeneratorUtilities.PackUV(1f, 1f, scale);
		uvs2[11 + index] = TextGeneratorUtilities.PackUV(1f, 0f, scale);
		underlineColor.a = ((m_FontColor32.a < underlineColor.a) ? m_FontColor32.a : underlineColor.a);
		Color32[] colors = textInfo.meshInfo[0].colors32;
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

	private void DrawTextHighlight(Vector3 start, Vector3 end, ref int index, Color32 highlightColor, TextGenerationSettings generationSettings, TextInfo textInfo)
	{
		if (m_CachedUnderlineGlyphInfo == null)
		{
			if (!TextSettings.warningsDisabled)
			{
				Debug.LogWarning("Unable to add underline since the Font Asset doesn't contain the underline character.");
			}
			return;
		}
		int num = index + 4;
		if (num > textInfo.meshInfo[0].vertices.Length)
		{
			textInfo.meshInfo[0].ResizeMeshInfo(num / 4);
		}
		Vector3[] vertices = textInfo.meshInfo[0].vertices;
		vertices[index] = start;
		vertices[index + 1] = new Vector3(start.x, end.y, 0f);
		vertices[index + 2] = end;
		vertices[index + 3] = new Vector3(end.x, start.y, 0f);
		if (generationSettings.inverseYAxis)
		{
			Vector3 vector = default(Vector3);
			vector.x = 0f;
			vector.y = generationSettings.screenRect.y + generationSettings.screenRect.height;
			vector.z = 0f;
			vertices[index].y = vertices[index].y * -1f + vector.y;
			vertices[index + 1].y = vertices[index + 1].y * -1f + vector.y;
			vertices[index + 2].y = vertices[index + 2].y * -1f + vector.y;
			vertices[index + 3].y = vertices[index + 3].y * -1f + vector.y;
		}
		Vector2[] uvs = textInfo.meshInfo[0].uvs0;
		Vector2 vector2 = new Vector2(((float)m_CachedUnderlineGlyphInfo.glyph.glyphRect.x + (float)(m_CachedUnderlineGlyphInfo.glyph.glyphRect.width / 2)) / (float)generationSettings.fontAsset.atlasWidth, ((float)m_CachedUnderlineGlyphInfo.glyph.glyphRect.y + (float)m_CachedUnderlineGlyphInfo.glyph.glyphRect.height / 2f) / (float)generationSettings.fontAsset.atlasHeight);
		uvs[index] = vector2;
		uvs[1 + index] = vector2;
		uvs[2 + index] = vector2;
		uvs[3 + index] = vector2;
		Vector2[] uvs2 = textInfo.meshInfo[0].uvs2;
		Vector2 vector3 = new Vector2(0f, 1f);
		uvs2[index] = vector3;
		uvs2[1 + index] = vector3;
		uvs2[2 + index] = vector3;
		uvs2[3 + index] = vector3;
		highlightColor.a = ((m_FontColor32.a < highlightColor.a) ? m_FontColor32.a : highlightColor.a);
		Color32[] colors = textInfo.meshInfo[0].colors32;
		colors[index] = highlightColor;
		colors[1 + index] = highlightColor;
		colors[2 + index] = highlightColor;
		colors[3 + index] = highlightColor;
		index += 4;
	}

	private static void ClearMesh(bool updateMesh, TextInfo textInfo)
	{
		textInfo.ClearMeshInfo(updateMesh);
	}

	private void EnableMasking()
	{
		m_IsMaskingEnabled = true;
	}

	private void DisableMasking()
	{
		m_IsMaskingEnabled = false;
	}

	private void SetArraySizes(int[] chars, TextGenerationSettings generationSettings, TextInfo textInfo)
	{
		int num = 0;
		m_TotalCharacterCount = 0;
		m_IsUsingBold = false;
		m_IsParsingText = false;
		m_TagNoParsing = false;
		m_FontStyleInternal = generationSettings.fontStyle;
		m_FontWeightInternal = (((m_FontStyleInternal & FontStyles.Bold) == FontStyles.Bold) ? FontWeight.Bold : generationSettings.fontWeight);
		m_FontWeightStack.SetDefault(m_FontWeightInternal);
		m_CurrentFontAsset = generationSettings.fontAsset;
		m_CurrentMaterial = generationSettings.material;
		m_CurrentMaterialIndex = 0;
		m_MaterialReferenceStack.SetDefault(new MaterialReference(m_CurrentMaterialIndex, m_CurrentFontAsset, null, m_CurrentMaterial, m_Padding));
		m_MaterialReferenceIndexLookup.Clear();
		MaterialReference.AddMaterialReference(m_CurrentMaterial, m_CurrentFontAsset, m_MaterialReferences, m_MaterialReferenceIndexLookup);
		if (textInfo == null)
		{
			textInfo = new TextInfo();
		}
		m_TextElementType = TextElementType.Character;
		for (int i = 0; i < chars.Length && chars[i] != 0; i++)
		{
			if (textInfo.textElementInfo == null || m_TotalCharacterCount >= textInfo.textElementInfo.Length)
			{
				TextInfo.Resize(ref textInfo.textElementInfo, m_TotalCharacterCount + 1, isBlockAllocated: true);
			}
			int num2 = chars[i];
			if (generationSettings.richText && num2 == 60)
			{
				int currentMaterialIndex = m_CurrentMaterialIndex;
				if (ValidateHtmlTag(chars, i + 1, out var endIndex, generationSettings, textInfo))
				{
					i = endIndex;
					if ((m_FontStyleInternal & FontStyles.Bold) == FontStyles.Bold)
					{
						m_IsUsingBold = true;
					}
					if (m_TextElementType == TextElementType.Sprite)
					{
						m_MaterialReferences[m_CurrentMaterialIndex].referenceCount++;
						textInfo.textElementInfo[m_TotalCharacterCount].character = (char)(57344 + m_SpriteIndex);
						textInfo.textElementInfo[m_TotalCharacterCount].spriteIndex = m_SpriteIndex;
						textInfo.textElementInfo[m_TotalCharacterCount].fontAsset = m_CurrentFontAsset;
						textInfo.textElementInfo[m_TotalCharacterCount].spriteAsset = m_CurrentSpriteAsset;
						textInfo.textElementInfo[m_TotalCharacterCount].materialReferenceIndex = m_CurrentMaterialIndex;
						textInfo.textElementInfo[m_TotalCharacterCount].elementType = m_TextElementType;
						m_TextElementType = TextElementType.Character;
						m_CurrentMaterialIndex = currentMaterialIndex;
						num++;
						m_TotalCharacterCount++;
					}
					continue;
				}
			}
			bool isAlternativeTypeface = false;
			bool flag = false;
			FontAsset currentFontAsset = m_CurrentFontAsset;
			Material currentMaterial = m_CurrentMaterial;
			int currentMaterialIndex2 = m_CurrentMaterialIndex;
			if (m_TextElementType == TextElementType.Character)
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
			FontAsset fontAsset;
			Character character = FontUtilities.GetCharacterFromFontAsset((uint)num2, m_CurrentFontAsset, includeFallbacks: false, m_FontStyleInternal, m_FontWeightInternal, out isAlternativeTypeface, out fontAsset);
			if (character == null && m_CurrentFontAsset.fallbackFontAssetTable != null && m_CurrentFontAsset.fallbackFontAssetTable.Count > 0)
			{
				character = FontUtilities.GetCharacterFromFontAssets((uint)num2, m_CurrentFontAsset.fallbackFontAssetTable, includeFallbacks: true, m_FontStyleInternal, m_FontWeightInternal, out isAlternativeTypeface, out fontAsset);
			}
			if (character == null)
			{
				TextSpriteAsset spriteAsset = generationSettings.spriteAsset;
				if (spriteAsset != null)
				{
					int spriteIndex = -1;
					spriteAsset = TextSpriteAsset.SearchForSpriteByUnicode(spriteAsset, (uint)num2, includeFallbacks: true, out spriteIndex);
					if (spriteIndex != -1)
					{
						m_TextElementType = TextElementType.Sprite;
						textInfo.textElementInfo[m_TotalCharacterCount].elementType = m_TextElementType;
						m_CurrentMaterialIndex = MaterialReference.AddMaterialReference(spriteAsset.material, spriteAsset, m_MaterialReferences, m_MaterialReferenceIndexLookup);
						m_MaterialReferences[m_CurrentMaterialIndex].referenceCount++;
						textInfo.textElementInfo[m_TotalCharacterCount].character = (char)num2;
						textInfo.textElementInfo[m_TotalCharacterCount].spriteIndex = spriteIndex;
						textInfo.textElementInfo[m_TotalCharacterCount].fontAsset = m_CurrentFontAsset;
						textInfo.textElementInfo[m_TotalCharacterCount].spriteAsset = spriteAsset;
						textInfo.textElementInfo[m_TotalCharacterCount].materialReferenceIndex = m_CurrentMaterialIndex;
						m_TextElementType = TextElementType.Character;
						m_CurrentMaterialIndex = currentMaterialIndex2;
						num++;
						m_TotalCharacterCount++;
						continue;
					}
				}
			}
			if (character == null && TextSettings.fallbackFontAssets != null && TextSettings.fallbackFontAssets.Count > 0)
			{
				character = FontUtilities.GetCharacterFromFontAssets((uint)num2, TextSettings.fallbackFontAssets, includeFallbacks: true, m_FontStyleInternal, m_FontWeightInternal, out isAlternativeTypeface, out fontAsset);
			}
			if (character == null && TextSettings.defaultFontAsset != null)
			{
				character = FontUtilities.GetCharacterFromFontAsset((uint)num2, TextSettings.defaultFontAsset, includeFallbacks: true, m_FontStyleInternal, m_FontWeightInternal, out isAlternativeTypeface, out fontAsset);
			}
			if (character == null)
			{
				TextSpriteAsset defaultSpriteAsset = TextSettings.defaultSpriteAsset;
				if (defaultSpriteAsset != null)
				{
					int spriteIndex2 = -1;
					defaultSpriteAsset = TextSpriteAsset.SearchForSpriteByUnicode(defaultSpriteAsset, (uint)num2, includeFallbacks: true, out spriteIndex2);
					if (spriteIndex2 != -1)
					{
						m_TextElementType = TextElementType.Sprite;
						textInfo.textElementInfo[m_TotalCharacterCount].elementType = m_TextElementType;
						m_CurrentMaterialIndex = MaterialReference.AddMaterialReference(defaultSpriteAsset.material, defaultSpriteAsset, m_MaterialReferences, m_MaterialReferenceIndexLookup);
						m_MaterialReferences[m_CurrentMaterialIndex].referenceCount++;
						textInfo.textElementInfo[m_TotalCharacterCount].character = (char)num2;
						textInfo.textElementInfo[m_TotalCharacterCount].spriteIndex = spriteIndex2;
						textInfo.textElementInfo[m_TotalCharacterCount].fontAsset = m_CurrentFontAsset;
						textInfo.textElementInfo[m_TotalCharacterCount].spriteAsset = defaultSpriteAsset;
						textInfo.textElementInfo[m_TotalCharacterCount].materialReferenceIndex = m_CurrentMaterialIndex;
						m_TextElementType = TextElementType.Character;
						m_CurrentMaterialIndex = currentMaterialIndex2;
						num++;
						m_TotalCharacterCount++;
						continue;
					}
				}
			}
			if (character == null)
			{
				int num3 = num2;
				num2 = (chars[i] = ((TextSettings.missingGlyphCharacter == 0) ? 9633 : TextSettings.missingGlyphCharacter));
				character = FontUtilities.GetCharacterFromFontAsset((uint)num2, m_CurrentFontAsset, includeFallbacks: true, m_FontStyleInternal, m_FontWeightInternal, out isAlternativeTypeface, out fontAsset);
				if (character == null && TextSettings.fallbackFontAssets != null && TextSettings.fallbackFontAssets.Count > 0)
				{
					character = FontUtilities.GetCharacterFromFontAssets((uint)num2, TextSettings.fallbackFontAssets, includeFallbacks: true, m_FontStyleInternal, m_FontWeightInternal, out isAlternativeTypeface, out fontAsset);
				}
				if (character == null && TextSettings.defaultFontAsset != null)
				{
					character = FontUtilities.GetCharacterFromFontAsset((uint)num2, TextSettings.defaultFontAsset, includeFallbacks: true, m_FontStyleInternal, m_FontWeightInternal, out isAlternativeTypeface, out fontAsset);
				}
				if (character == null)
				{
					num2 = (chars[i] = 32);
					character = FontUtilities.GetCharacterFromFontAsset((uint)num2, m_CurrentFontAsset, includeFallbacks: true, m_FontStyleInternal, m_FontWeightInternal, out isAlternativeTypeface, out fontAsset);
					if (!TextSettings.warningsDisabled)
					{
						Debug.LogWarning("Character with ASCII value of " + num3 + " was not found in the Font Asset Glyph Table. It was replaced by a space.");
					}
				}
			}
			if (fontAsset != null && fontAsset.GetInstanceID() != m_CurrentFontAsset.GetInstanceID())
			{
				flag = true;
				m_CurrentFontAsset = fontAsset;
			}
			textInfo.textElementInfo[m_TotalCharacterCount].elementType = TextElementType.Character;
			textInfo.textElementInfo[m_TotalCharacterCount].textElement = character;
			textInfo.textElementInfo[m_TotalCharacterCount].isUsingAlternateTypeface = isAlternativeTypeface;
			textInfo.textElementInfo[m_TotalCharacterCount].character = (char)num2;
			textInfo.textElementInfo[m_TotalCharacterCount].fontAsset = m_CurrentFontAsset;
			if (flag)
			{
				if (TextSettings.matchMaterialPreset)
				{
					m_CurrentMaterial = MaterialManager.GetFallbackMaterial(m_CurrentMaterial, m_CurrentFontAsset.material);
				}
				else
				{
					m_CurrentMaterial = m_CurrentFontAsset.material;
				}
				m_CurrentMaterialIndex = MaterialReference.AddMaterialReference(m_CurrentMaterial, m_CurrentFontAsset, m_MaterialReferences, m_MaterialReferenceIndexLookup);
			}
			if (!char.IsWhiteSpace((char)num2) && num2 != 8203)
			{
				if (m_MaterialReferences[m_CurrentMaterialIndex].referenceCount < 16383)
				{
					m_MaterialReferences[m_CurrentMaterialIndex].referenceCount++;
				}
				else
				{
					m_CurrentMaterialIndex = MaterialReference.AddMaterialReference(new Material(m_CurrentMaterial), m_CurrentFontAsset, m_MaterialReferences, m_MaterialReferenceIndexLookup);
					m_MaterialReferences[m_CurrentMaterialIndex].referenceCount++;
				}
			}
			textInfo.textElementInfo[m_TotalCharacterCount].material = m_CurrentMaterial;
			textInfo.textElementInfo[m_TotalCharacterCount].materialReferenceIndex = m_CurrentMaterialIndex;
			m_MaterialReferences[m_CurrentMaterialIndex].isFallbackMaterial = flag;
			if (flag)
			{
				m_MaterialReferences[m_CurrentMaterialIndex].fallbackMaterial = currentMaterial;
				m_CurrentFontAsset = currentFontAsset;
				m_CurrentMaterial = currentMaterial;
				m_CurrentMaterialIndex = currentMaterialIndex2;
			}
			m_TotalCharacterCount++;
		}
		if (m_IsCalculatingPreferredValues)
		{
			m_IsCalculatingPreferredValues = false;
			return;
		}
		textInfo.spriteCount = num;
		int num4 = (textInfo.materialCount = m_MaterialReferenceIndexLookup.Count);
		if (num4 > textInfo.meshInfo.Length)
		{
			TextInfo.Resize(ref textInfo.meshInfo, num4, isBlockAllocated: false);
		}
		if (textInfo.textElementInfo.Length - m_TotalCharacterCount > 256)
		{
			TextInfo.Resize(ref textInfo.textElementInfo, Mathf.Max(m_TotalCharacterCount + 1, 256), isBlockAllocated: true);
		}
		for (int j = 0; j < num4; j++)
		{
			int referenceCount = m_MaterialReferences[j].referenceCount;
			if (textInfo.meshInfo[j].vertices == null || textInfo.meshInfo[j].vertices.Length < referenceCount * 4)
			{
				if (textInfo.meshInfo[j].vertices == null)
				{
					textInfo.meshInfo[j] = new MeshInfo(referenceCount + 1);
				}
				else
				{
					textInfo.meshInfo[j].ResizeMeshInfo((referenceCount > 1024) ? (referenceCount + 256) : Mathf.NextPowerOfTwo(referenceCount));
				}
			}
			else if (textInfo.meshInfo[j].vertices.Length - referenceCount * 4 > 1024)
			{
				textInfo.meshInfo[j].ResizeMeshInfo((referenceCount > 1024) ? (referenceCount + 256) : Mathf.Max(Mathf.NextPowerOfTwo(referenceCount), 256));
			}
			textInfo.meshInfo[j].material = m_MaterialReferences[j].material;
		}
	}

	private void ComputeMarginSize(Rect rect, Vector4 margins)
	{
		m_MarginWidth = rect.width - margins.x - margins.z;
		m_MarginHeight = rect.height - margins.y - margins.w;
		m_RectTransformCorners[0].x = 0f;
		m_RectTransformCorners[0].y = 0f;
		m_RectTransformCorners[1].x = 0f;
		m_RectTransformCorners[1].y = rect.height;
		m_RectTransformCorners[2].x = rect.width;
		m_RectTransformCorners[2].y = rect.height;
		m_RectTransformCorners[3].x = rect.width;
		m_RectTransformCorners[3].y = 0f;
	}

	private void GetSpecialCharacters(FontAsset fontAsset)
	{
		fontAsset.characterLookupTable.TryGetValue(95u, out m_CachedUnderlineGlyphInfo);
		fontAsset.characterLookupTable.TryGetValue(8230u, out m_CachedEllipsisGlyphInfo);
	}

	private float GetPaddingForMaterial(Material material, bool extraPadding)
	{
		ShaderUtilities.GetShaderPropertyIDs();
		if (material == null)
		{
			return 0f;
		}
		m_Padding = ShaderUtilities.GetPadding(material, extraPadding, m_IsUsingBold);
		m_IsMaskingEnabled = ShaderUtilities.IsMaskingEnabled(material);
		m_IsSdfShader = material.HasProperty(ShaderUtilities.ID_WeightNormal);
		return m_Padding;
	}

	private float GetPreferredWidthInternal(TextGenerationSettings generationSettings, TextInfo textInfo)
	{
		if (TextSettings.instance == null)
		{
			return 0f;
		}
		float defaultFontSize = (generationSettings.autoSize ? generationSettings.fontSizeMax : m_FontSize);
		m_MinFontSize = generationSettings.fontSizeMin;
		m_MaxFontSize = generationSettings.fontSizeMax;
		m_CharWidthAdjDelta = 0f;
		Vector2 largePositiveVector = TextGeneratorUtilities.largePositiveVector2;
		m_RecursiveCount = 0;
		return CalculatePreferredValues(defaultFontSize, largePositiveVector, ignoreTextAutoSizing: true, generationSettings, textInfo).x;
	}

	private float GetPreferredHeightInternal(TextGenerationSettings generationSettings, TextInfo textInfo)
	{
		if (TextSettings.instance == null)
		{
			return 0f;
		}
		float defaultFontSize = (generationSettings.autoSize ? generationSettings.fontSizeMax : m_FontSize);
		m_MinFontSize = generationSettings.fontSizeMin;
		m_MaxFontSize = generationSettings.fontSizeMax;
		m_CharWidthAdjDelta = 0f;
		Vector2 marginSize = new Vector2((m_MarginWidth != 0f) ? m_MarginWidth : 32767f, 32767f);
		m_RecursiveCount = 0;
		return CalculatePreferredValues(defaultFontSize, marginSize, !generationSettings.autoSize, generationSettings, textInfo).y;
	}

	private Vector2 GetPreferredValuesInternal(TextGenerationSettings generationSettings, TextInfo textInfo)
	{
		if (TextSettings.instance == null)
		{
			return Vector2.zero;
		}
		float defaultFontSize = (generationSettings.autoSize ? generationSettings.fontSizeMax : m_FontSize);
		m_MinFontSize = generationSettings.fontSizeMin;
		m_MaxFontSize = generationSettings.fontSizeMax;
		m_CharWidthAdjDelta = 0f;
		Vector2 marginSize = new Vector2((m_MarginWidth != 0f) ? m_MarginWidth : 32767f, 32767f);
		m_RecursiveCount = 0;
		return CalculatePreferredValues(defaultFontSize, marginSize, !generationSettings.autoSize, generationSettings, textInfo);
	}

	protected virtual Vector2 CalculatePreferredValues(float defaultFontSize, Vector2 marginSize, bool ignoreTextAutoSizing, TextGenerationSettings generationSettings, TextInfo textInfo)
	{
		if (generationSettings.fontAsset == null || generationSettings.fontAsset.characterLookupTable == null)
		{
			return Vector2.zero;
		}
		if (m_CharBuffer == null || m_CharBuffer.Length == 0 || m_CharBuffer[0] == 0)
		{
			return Vector2.zero;
		}
		m_CurrentFontAsset = generationSettings.fontAsset;
		m_CurrentMaterial = generationSettings.material;
		m_CurrentMaterialIndex = 0;
		m_MaterialReferenceStack.SetDefault(new MaterialReference(0, m_CurrentFontAsset, null, m_CurrentMaterial, m_Padding));
		int totalCharacterCount = m_TotalCharacterCount;
		if (m_InternalTextElementInfo == null || totalCharacterCount > m_InternalTextElementInfo.Length)
		{
			m_InternalTextElementInfo = new TextElementInfo[(totalCharacterCount > 1024) ? (totalCharacterCount + 256) : Mathf.NextPowerOfTwo(totalCharacterCount)];
		}
		float num = (m_FontScale = defaultFontSize / (float)generationSettings.fontAsset.faceInfo.pointSize * generationSettings.fontAsset.faceInfo.scale);
		float num2 = num;
		m_FontScaleMultiplier = 1f;
		m_CurrentFontSize = defaultFontSize;
		m_SizeStack.SetDefault(m_CurrentFontSize);
		m_FontStyleInternal = generationSettings.fontStyle;
		m_LineJustification = generationSettings.textAlignment;
		m_LineJustificationStack.SetDefault(m_LineJustification);
		m_BaselineOffset = 0f;
		m_BaselineOffsetStack.Clear();
		m_LineOffset = 0f;
		m_LineHeight = -32767f;
		float num3 = m_CurrentFontAsset.faceInfo.lineHeight - (m_CurrentFontAsset.faceInfo.ascentLine - m_CurrentFontAsset.faceInfo.descentLine);
		m_CSpacing = 0f;
		m_MonoSpacing = 0f;
		m_XAdvance = 0f;
		float a = 0f;
		m_TagLineIndent = 0f;
		m_TagIndent = 0f;
		m_IndentStack.SetDefault(0f);
		m_TagNoParsing = false;
		m_CharacterCount = 0;
		m_FirstCharacterOfLine = 0;
		m_MaxLineAscender = -32767f;
		m_MaxLineDescender = 32767f;
		m_LineNumber = 0;
		float x = marginSize.x;
		m_MarginLeft = 0f;
		m_MarginRight = 0f;
		m_Width = -1f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		m_IsCalculatingPreferredValues = true;
		m_MaxAscender = 0f;
		m_MaxDescender = 0f;
		bool flag = true;
		bool flag2 = false;
		WordWrapState state = default(WordWrapState);
		SaveWordWrappingState(ref state, 0, 0, textInfo);
		WordWrapState state2 = default(WordWrapState);
		int num7 = 0;
		m_RecursiveCount++;
		for (int i = 0; m_CharBuffer[i] != 0; i++)
		{
			int num8 = m_CharBuffer[i];
			m_TextElementType = textInfo.textElementInfo[m_CharacterCount].elementType;
			m_CurrentMaterialIndex = textInfo.textElementInfo[m_CharacterCount].materialReferenceIndex;
			m_CurrentFontAsset = m_MaterialReferences[m_CurrentMaterialIndex].fontAsset;
			int currentMaterialIndex = m_CurrentMaterialIndex;
			if (generationSettings.richText && num8 == 60)
			{
				m_IsParsingText = true;
				m_TextElementType = TextElementType.Character;
				if (ValidateHtmlTag(m_CharBuffer, i + 1, out var endIndex, generationSettings, textInfo))
				{
					i = endIndex;
					if (m_TextElementType == TextElementType.Character)
					{
						continue;
					}
				}
			}
			m_IsParsingText = false;
			bool isUsingAlternateTypeface = textInfo.textElementInfo[m_CharacterCount].isUsingAlternateTypeface;
			float num9 = 1f;
			if (m_TextElementType == TextElementType.Character)
			{
				if ((m_FontStyleInternal & FontStyles.UpperCase) == FontStyles.UpperCase)
				{
					if (char.IsLower((char)num8))
					{
						num8 = char.ToUpper((char)num8);
					}
				}
				else if ((m_FontStyleInternal & FontStyles.LowerCase) == FontStyles.LowerCase)
				{
					if (char.IsUpper((char)num8))
					{
						num8 = char.ToLower((char)num8);
					}
				}
				else if ((m_FontStyleInternal & FontStyles.SmallCaps) == FontStyles.SmallCaps && char.IsLower((char)num8))
				{
					num9 = 0.8f;
					num8 = char.ToUpper((char)num8);
				}
			}
			if (m_TextElementType == TextElementType.Sprite)
			{
				m_CurrentSpriteAsset = textInfo.textElementInfo[m_CharacterCount].spriteAsset;
				m_SpriteIndex = textInfo.textElementInfo[m_CharacterCount].spriteIndex;
				SpriteCharacter spriteCharacter = m_CurrentSpriteAsset.spriteCharacterTable[m_SpriteIndex];
				if (spriteCharacter == null)
				{
					continue;
				}
				if (num8 == 60)
				{
					num8 = 57344 + m_SpriteIndex;
				}
				m_CurrentFontAsset = generationSettings.fontAsset;
				float num10 = m_CurrentFontSize / (float)generationSettings.fontAsset.faceInfo.pointSize * generationSettings.fontAsset.faceInfo.scale;
				num2 = generationSettings.fontAsset.faceInfo.ascentLine / spriteCharacter.glyph.metrics.height * spriteCharacter.scale * num10;
				m_CachedTextElement = spriteCharacter;
				m_InternalTextElementInfo[m_CharacterCount].elementType = TextElementType.Sprite;
				m_InternalTextElementInfo[m_CharacterCount].scale = num10;
				m_CurrentMaterialIndex = currentMaterialIndex;
			}
			else if (m_TextElementType == TextElementType.Character)
			{
				m_CachedTextElement = textInfo.textElementInfo[m_CharacterCount].textElement;
				if (m_CachedTextElement == null)
				{
					continue;
				}
				m_CurrentMaterialIndex = textInfo.textElementInfo[m_CharacterCount].materialReferenceIndex;
				m_FontScale = m_CurrentFontSize * num9 / (float)m_CurrentFontAsset.faceInfo.pointSize * m_CurrentFontAsset.faceInfo.scale;
				num2 = m_FontScale * m_FontScaleMultiplier * m_CachedTextElement.scale;
				m_InternalTextElementInfo[m_CharacterCount].elementType = TextElementType.Character;
			}
			float num11 = num2;
			if (num8 == 173)
			{
				num2 = 0f;
			}
			m_InternalTextElementInfo[m_CharacterCount].character = (char)num8;
			GlyphValueRecord glyphValueRecord = default(GlyphValueRecord);
			if (generationSettings.enableKerning)
			{
				KerningPair value;
				if (m_CharacterCount < totalCharacterCount - 1)
				{
					uint character = textInfo.textElementInfo[m_CharacterCount + 1].character;
					KerningPairKey kerningPairKey = new KerningPairKey((uint)num8, character);
					m_CurrentFontAsset.kerningLookupDictionary.TryGetValue((int)kerningPairKey.key, out value);
					if (value != null)
					{
						glyphValueRecord = value.firstGlyphAdjustments;
					}
				}
				if (m_CharacterCount >= 1)
				{
					uint character2 = textInfo.textElementInfo[m_CharacterCount - 1].character;
					KerningPairKey kerningPairKey2 = new KerningPairKey(character2, (uint)num8);
					m_CurrentFontAsset.kerningLookupDictionary.TryGetValue((int)kerningPairKey2.key, out value);
					if (value != null)
					{
						glyphValueRecord += value.secondGlyphAdjustments;
					}
				}
			}
			float num12 = 0f;
			if (m_MonoSpacing != 0f)
			{
				num12 = m_MonoSpacing / 2f - (m_CachedTextElement.glyph.metrics.width / 2f + m_CachedTextElement.glyph.metrics.horizontalBearingX) * num2;
				m_XAdvance += num12;
			}
			float num13 = ((m_TextElementType != TextElementType.Character || isUsingAlternateTypeface || (m_FontStyleInternal & FontStyles.Bold) != FontStyles.Bold) ? 1f : (1f + m_CurrentFontAsset.boldStyleSpacing * 0.01f));
			m_InternalTextElementInfo[m_CharacterCount].baseLine = 0f - m_LineOffset + m_BaselineOffset;
			float num14 = m_CurrentFontAsset.faceInfo.ascentLine * ((m_TextElementType == TextElementType.Character) ? (num2 / num9) : m_InternalTextElementInfo[m_CharacterCount].scale) + m_BaselineOffset;
			m_InternalTextElementInfo[m_CharacterCount].ascender = num14 - m_LineOffset;
			m_MaxLineAscender = ((num14 > m_MaxLineAscender) ? num14 : m_MaxLineAscender);
			float num15 = m_CurrentFontAsset.faceInfo.descentLine * ((m_TextElementType == TextElementType.Character) ? (num2 / num9) : m_InternalTextElementInfo[m_CharacterCount].scale) + m_BaselineOffset;
			m_MaxLineDescender = ((num15 < m_MaxLineDescender) ? num15 : m_MaxLineDescender);
			if ((m_FontStyleInternal & FontStyles.Subscript) == FontStyles.Subscript || (m_FontStyleInternal & FontStyles.Superscript) == FontStyles.Superscript)
			{
				float num16 = (num14 - m_BaselineOffset) / m_CurrentFontAsset.faceInfo.subscriptSize;
				num14 = m_MaxLineAscender;
				m_MaxLineAscender = ((num16 > m_MaxLineAscender) ? num16 : m_MaxLineAscender);
				float num17 = (num15 - m_BaselineOffset) / m_CurrentFontAsset.faceInfo.subscriptSize;
				m_MaxLineDescender = ((num17 < m_MaxLineDescender) ? num17 : m_MaxLineDescender);
			}
			if (m_LineNumber == 0)
			{
				m_MaxAscender = ((m_MaxAscender > num14) ? m_MaxAscender : num14);
			}
			if (num8 == 9 || (!char.IsWhiteSpace((char)num8) && num8 != 8203) || m_TextElementType == TextElementType.Sprite)
			{
				float num18 = ((m_Width != -1f) ? Mathf.Min(x + 0.0001f - m_MarginLeft - m_MarginRight, m_Width) : (x + 0.0001f - m_MarginLeft - m_MarginRight));
				bool flag3 = (m_LineJustification & (TextAlignment)16) == (TextAlignment)16 || (m_LineJustification & (TextAlignment)8) == (TextAlignment)8;
				num6 = m_XAdvance + m_CachedTextElement.glyph.metrics.horizontalAdvance * (1f - m_CharWidthAdjDelta) * ((num8 != 173) ? num2 : num11);
				if (num6 > num18 * (flag3 ? 1.05f : 1f))
				{
					if (generationSettings.wordWrap && m_CharacterCount != m_FirstCharacterOfLine)
					{
						if (num7 == state2.previousWordBreak || flag)
						{
							if (!ignoreTextAutoSizing && m_CurrentFontSize > generationSettings.fontSizeMin)
							{
								if (m_CharWidthAdjDelta < generationSettings.charWidthMaxAdj / 100f)
								{
									m_RecursiveCount = 0;
									m_CharWidthAdjDelta += 0.01f;
									return CalculatePreferredValues(defaultFontSize, marginSize, ignoreTextAutoSizing: false, generationSettings, textInfo);
								}
								m_MaxFontSize = defaultFontSize;
								defaultFontSize -= Mathf.Max((defaultFontSize - m_MinFontSize) / 2f, 0.05f);
								defaultFontSize = (float)(int)(Mathf.Max(defaultFontSize, generationSettings.fontSizeMin) * 20f + 0.5f) / 20f;
								if (m_RecursiveCount > 20)
								{
									return new Vector2(num4, num5);
								}
								return CalculatePreferredValues(defaultFontSize, marginSize, ignoreTextAutoSizing: false, generationSettings, textInfo);
							}
							if (!m_IsCharacterWrappingEnabled)
							{
								m_IsCharacterWrappingEnabled = true;
							}
							else
							{
								flag2 = true;
							}
						}
						i = RestoreWordWrappingState(ref state2, textInfo);
						num7 = i;
						if (m_CharBuffer[i] == 173)
						{
							m_CharBuffer[i] = 45;
							return CalculatePreferredValues(defaultFontSize, marginSize, ignoreTextAutoSizing: true, generationSettings, textInfo);
						}
						if (m_LineNumber > 0 && !TextGeneratorUtilities.Approximately(m_MaxLineAscender, m_StartOfLineAscender) && m_LineHeight == -32767f)
						{
							float num19 = m_MaxLineAscender - m_StartOfLineAscender;
							m_LineOffset += num19;
							state2.lineOffset = m_LineOffset;
							state2.previousLineAscender = m_MaxLineAscender;
						}
						float num20 = m_MaxLineAscender - m_LineOffset;
						float num21 = m_MaxLineDescender - m_LineOffset;
						m_MaxDescender = ((m_MaxDescender < num21) ? m_MaxDescender : num21);
						m_FirstCharacterOfLine = m_CharacterCount;
						num4 += m_XAdvance;
						num5 = ((!generationSettings.wordWrap) ? Mathf.Max(num5, num20 - num21) : (m_MaxAscender - m_MaxDescender));
						SaveWordWrappingState(ref state, i, m_CharacterCount - 1, textInfo);
						m_LineNumber++;
						if (m_LineHeight == -32767f)
						{
							float num22 = m_InternalTextElementInfo[m_CharacterCount].ascender - m_InternalTextElementInfo[m_CharacterCount].baseLine;
							float num23 = 0f - m_MaxLineDescender + num22 + (num3 + generationSettings.lineSpacing + m_LineSpacingDelta) * num;
							m_LineOffset += num23;
							m_StartOfLineAscender = num22;
						}
						else
						{
							m_LineOffset += m_LineHeight + generationSettings.lineSpacing * num;
						}
						m_MaxLineAscender = -32767f;
						m_MaxLineDescender = 32767f;
						m_XAdvance = 0f + m_TagIndent;
						continue;
					}
					if (!ignoreTextAutoSizing && defaultFontSize > generationSettings.fontSizeMin)
					{
						if (m_CharWidthAdjDelta < generationSettings.charWidthMaxAdj / 100f)
						{
							m_RecursiveCount = 0;
							m_CharWidthAdjDelta += 0.01f;
							return CalculatePreferredValues(defaultFontSize, marginSize, ignoreTextAutoSizing: false, generationSettings, textInfo);
						}
						m_MaxFontSize = defaultFontSize;
						defaultFontSize -= Mathf.Max((defaultFontSize - m_MinFontSize) / 2f, 0.05f);
						defaultFontSize = (float)(int)(Mathf.Max(defaultFontSize, generationSettings.fontSizeMin) * 20f + 0.5f) / 20f;
						if (m_RecursiveCount > 20)
						{
							return new Vector2(num4, num5);
						}
						return CalculatePreferredValues(defaultFontSize, marginSize, ignoreTextAutoSizing: false, generationSettings, textInfo);
					}
				}
			}
			if (m_LineNumber > 0 && !TextGeneratorUtilities.Approximately(m_MaxLineAscender, m_StartOfLineAscender) && m_LineHeight == -32767f && !m_IsNewPage)
			{
				float num24 = m_MaxLineAscender - m_StartOfLineAscender;
				m_LineOffset += num24;
				m_StartOfLineAscender += num24;
				state2.lineOffset = m_LineOffset;
				state2.previousLineAscender = m_StartOfLineAscender;
			}
			if (num8 == 9)
			{
				float num25 = m_CurrentFontAsset.faceInfo.tabWidth * (float)(int)m_CurrentFontAsset.tabMultiple * num2;
				float num26 = Mathf.Ceil(m_XAdvance / num25) * num25;
				m_XAdvance = ((num26 > m_XAdvance) ? num26 : (m_XAdvance + num25));
			}
			else if (m_MonoSpacing != 0f)
			{
				m_XAdvance += (m_MonoSpacing - num12 + (generationSettings.characterSpacing + m_CurrentFontAsset.regularStyleSpacing) * num2 + m_CSpacing) * (1f - m_CharWidthAdjDelta);
				if (char.IsWhiteSpace((char)num8) || num8 == 8203)
				{
					m_XAdvance += generationSettings.wordSpacing * num2;
				}
			}
			else
			{
				m_XAdvance += ((m_CachedTextElement.glyph.metrics.horizontalAdvance * num13 + generationSettings.characterSpacing + m_CurrentFontAsset.regularStyleSpacing + glyphValueRecord.xAdvance) * num2 + m_CSpacing) * (1f - m_CharWidthAdjDelta);
				if (char.IsWhiteSpace((char)num8) || num8 == 8203)
				{
					m_XAdvance += generationSettings.wordSpacing * num2;
				}
			}
			if (num8 == 13)
			{
				a = Mathf.Max(a, num4 + m_XAdvance);
				num4 = 0f;
				m_XAdvance = 0f + m_TagIndent;
			}
			if (num8 == 10 || m_CharacterCount == totalCharacterCount - 1)
			{
				if (m_LineNumber > 0 && !TextGeneratorUtilities.Approximately(m_MaxLineAscender, m_StartOfLineAscender) && m_LineHeight == -32767f)
				{
					float num27 = m_MaxLineAscender - m_StartOfLineAscender;
					m_LineOffset += num27;
				}
				float num28 = m_MaxLineDescender - m_LineOffset;
				m_MaxDescender = ((m_MaxDescender < num28) ? m_MaxDescender : num28);
				m_FirstCharacterOfLine = m_CharacterCount + 1;
				if (num8 == 10 && m_CharacterCount != totalCharacterCount - 1)
				{
					a = Mathf.Max(a, num4 + num6);
					num4 = 0f;
				}
				else
				{
					num4 = Mathf.Max(a, num4 + num6);
				}
				num5 = m_MaxAscender - m_MaxDescender;
				if (num8 == 10)
				{
					SaveWordWrappingState(ref state, i, m_CharacterCount, textInfo);
					SaveWordWrappingState(ref state2, i, m_CharacterCount, textInfo);
					m_LineNumber++;
					if (m_LineHeight == -32767f)
					{
						float num23 = 0f - m_MaxLineDescender + num14 + (num3 + generationSettings.lineSpacing + generationSettings.paragraphSpacing + m_LineSpacingDelta) * num;
						m_LineOffset += num23;
					}
					else
					{
						m_LineOffset += m_LineHeight + (generationSettings.lineSpacing + generationSettings.paragraphSpacing) * num;
					}
					m_MaxLineAscender = -32767f;
					m_MaxLineDescender = 32767f;
					m_StartOfLineAscender = num14;
					m_XAdvance = 0f + m_TagLineIndent + m_TagIndent;
					m_CharacterCount++;
					continue;
				}
			}
			if (generationSettings.wordWrap || generationSettings.overflowMode == TextOverflowMode.Truncate || generationSettings.overflowMode == TextOverflowMode.Ellipsis)
			{
				if ((char.IsWhiteSpace((char)num8) || num8 == 8203 || num8 == 45 || num8 == 173) && !m_IsNonBreakingSpace && num8 != 160 && num8 != 8209 && num8 != 8239 && num8 != 8288)
				{
					SaveWordWrappingState(ref state2, i, m_CharacterCount, textInfo);
					m_IsCharacterWrappingEnabled = false;
					flag = false;
				}
				else if (((num8 > 4352 && num8 < 4607) || (num8 > 11904 && num8 < 40959) || (num8 > 43360 && num8 < 43391) || (num8 > 44032 && num8 < 55295) || (num8 > 63744 && num8 < 64255) || (num8 > 65072 && num8 < 65103) || (num8 > 65280 && num8 < 65519)) && !m_IsNonBreakingSpace)
				{
					if (flag || flag2 || (!TextSettings.linebreakingRules.leadingCharacters.ContainsKey(num8) && m_CharacterCount < totalCharacterCount - 1 && !TextSettings.linebreakingRules.followingCharacters.ContainsKey(m_InternalTextElementInfo[m_CharacterCount + 1].character)))
					{
						SaveWordWrappingState(ref state2, i, m_CharacterCount, textInfo);
						m_IsCharacterWrappingEnabled = false;
						flag = false;
					}
				}
				else if (flag || m_IsCharacterWrappingEnabled || flag2)
				{
					SaveWordWrappingState(ref state2, i, m_CharacterCount, textInfo);
				}
			}
			m_CharacterCount++;
		}
		float num29 = m_MaxFontSize - m_MinFontSize;
		if (!m_IsCharacterWrappingEnabled && !ignoreTextAutoSizing && num29 > 0.051f && defaultFontSize < generationSettings.fontSizeMax)
		{
			m_MinFontSize = defaultFontSize;
			defaultFontSize += Mathf.Max((m_MaxFontSize - defaultFontSize) / 2f, 0.05f);
			defaultFontSize = (float)(int)(Mathf.Min(defaultFontSize, generationSettings.fontSizeMax) * 20f + 0.5f) / 20f;
			if (m_RecursiveCount > 20)
			{
				return new Vector2(num4, num5);
			}
			return CalculatePreferredValues(defaultFontSize, marginSize, ignoreTextAutoSizing: false, generationSettings, textInfo);
		}
		m_IsCharacterWrappingEnabled = false;
		m_IsCalculatingPreferredValues = false;
		num4 += ((generationSettings.margins.x > 0f) ? generationSettings.margins.x : 0f);
		num4 += ((generationSettings.margins.z > 0f) ? generationSettings.margins.z : 0f);
		num5 += ((generationSettings.margins.y > 0f) ? generationSettings.margins.y : 0f);
		num5 += ((generationSettings.margins.w > 0f) ? generationSettings.margins.w : 0f);
		num4 = (float)(int)(num4 * 100f + 1f) / 100f;
		num5 = (float)(int)(num5 * 100f + 1f) / 100f;
		return new Vector2(num4, num5);
	}
}
