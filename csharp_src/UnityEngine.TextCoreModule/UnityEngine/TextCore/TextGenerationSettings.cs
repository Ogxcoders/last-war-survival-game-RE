using UnityEngine.Bindings;

namespace UnityEngine.TextCore;

[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
internal class TextGenerationSettings
{
	public string text = null;

	public Rect screenRect;

	public Vector4 margins;

	public float scale = 1f;

	public FontAsset fontAsset;

	public Material material;

	public TextSpriteAsset spriteAsset;

	public FontStyles fontStyle = FontStyles.Normal;

	public TextAlignment textAlignment = TextAlignment.TopLeft;

	public TextOverflowMode overflowMode = TextOverflowMode.Overflow;

	public bool wordWrap = false;

	public float wordWrappingRatio;

	public Color color = Color.white;

	public TextGradientPreset fontColorGradient;

	public bool tintSprites;

	public bool overrideRichTextColors;

	public float fontSize = 18f;

	public bool autoSize;

	public float fontSizeMin;

	public float fontSizeMax;

	public bool enableKerning = true;

	public bool richText;

	public bool isRightToLeft;

	public bool extraPadding;

	public bool parseControlCharacters = true;

	public float characterSpacing;

	public float wordSpacing;

	public float lineSpacing;

	public float paragraphSpacing;

	public float lineSpacingMax;

	public int maxVisibleCharacters = 99999;

	public int maxVisibleWords = 99999;

	public int maxVisibleLines = 99999;

	public int firstVisibleCharacter = 0;

	public bool useMaxVisibleDescender;

	public FontWeight fontWeight = FontWeight.Regular;

	public int pageToDisplay = 1;

	public TextureMapping horizontalMapping = TextureMapping.Character;

	public TextureMapping verticalMapping = TextureMapping.Character;

	public float uvLineOffset;

	public VertexSortingOrder geometrySortingOrder = VertexSortingOrder.Normal;

	public bool inverseYAxis;

	public float charWidthMaxAdj;

	protected bool Equals(TextGenerationSettings other)
	{
		return string.Equals(text, other.text) && screenRect.Equals(other.screenRect) && margins.Equals(other.margins) && scale.Equals(other.scale) && object.Equals(fontAsset, other.fontAsset) && object.Equals(material, other.material) && object.Equals(spriteAsset, other.spriteAsset) && fontStyle == other.fontStyle && textAlignment == other.textAlignment && overflowMode == other.overflowMode && wordWrap == other.wordWrap && wordWrappingRatio.Equals(other.wordWrappingRatio) && color.Equals(other.color) && object.Equals(fontColorGradient, other.fontColorGradient) && tintSprites == other.tintSprites && overrideRichTextColors == other.overrideRichTextColors && fontSize.Equals(other.fontSize) && autoSize == other.autoSize && fontSizeMin.Equals(other.fontSizeMin) && fontSizeMax.Equals(other.fontSizeMax) && enableKerning == other.enableKerning && richText == other.richText && isRightToLeft == other.isRightToLeft && extraPadding == other.extraPadding && parseControlCharacters == other.parseControlCharacters && characterSpacing.Equals(other.characterSpacing) && wordSpacing.Equals(other.wordSpacing) && lineSpacing.Equals(other.lineSpacing) && paragraphSpacing.Equals(other.paragraphSpacing) && lineSpacingMax.Equals(other.lineSpacingMax) && maxVisibleCharacters == other.maxVisibleCharacters && maxVisibleWords == other.maxVisibleWords && maxVisibleLines == other.maxVisibleLines && firstVisibleCharacter == other.firstVisibleCharacter && useMaxVisibleDescender == other.useMaxVisibleDescender && fontWeight == other.fontWeight && pageToDisplay == other.pageToDisplay && horizontalMapping == other.horizontalMapping && verticalMapping == other.verticalMapping && uvLineOffset.Equals(other.uvLineOffset) && geometrySortingOrder == other.geometrySortingOrder && inverseYAxis == other.inverseYAxis && charWidthMaxAdj.Equals(other.charWidthMaxAdj);
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if ((object)obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((TextGenerationSettings)obj);
	}

	public override int GetHashCode()
	{
		int num = ((text != null) ? text.GetHashCode() : 0);
		num = (num * 397) ^ screenRect.GetHashCode();
		num = (num * 397) ^ margins.GetHashCode();
		num = (num * 397) ^ scale.GetHashCode();
		num = (num * 397) ^ ((fontAsset != null) ? fontAsset.GetHashCode() : 0);
		num = (num * 397) ^ ((material != null) ? material.GetHashCode() : 0);
		num = (num * 397) ^ ((spriteAsset != null) ? spriteAsset.GetHashCode() : 0);
		num = (num * 397) ^ (int)fontStyle;
		num = (num * 397) ^ (int)textAlignment;
		num = (num * 397) ^ (int)overflowMode;
		num = (num * 397) ^ wordWrap.GetHashCode();
		num = (num * 397) ^ wordWrappingRatio.GetHashCode();
		num = (num * 397) ^ color.GetHashCode();
		num = (num * 397) ^ ((fontColorGradient != null) ? fontColorGradient.GetHashCode() : 0);
		num = (num * 397) ^ tintSprites.GetHashCode();
		num = (num * 397) ^ overrideRichTextColors.GetHashCode();
		num = (num * 397) ^ fontSize.GetHashCode();
		num = (num * 397) ^ autoSize.GetHashCode();
		num = (num * 397) ^ fontSizeMin.GetHashCode();
		num = (num * 397) ^ fontSizeMax.GetHashCode();
		num = (num * 397) ^ enableKerning.GetHashCode();
		num = (num * 397) ^ richText.GetHashCode();
		num = (num * 397) ^ isRightToLeft.GetHashCode();
		num = (num * 397) ^ extraPadding.GetHashCode();
		num = (num * 397) ^ parseControlCharacters.GetHashCode();
		num = (num * 397) ^ characterSpacing.GetHashCode();
		num = (num * 397) ^ wordSpacing.GetHashCode();
		num = (num * 397) ^ lineSpacing.GetHashCode();
		num = (num * 397) ^ paragraphSpacing.GetHashCode();
		num = (num * 397) ^ lineSpacingMax.GetHashCode();
		num = (num * 397) ^ maxVisibleCharacters;
		num = (num * 397) ^ maxVisibleWords;
		num = (num * 397) ^ maxVisibleLines;
		num = (num * 397) ^ firstVisibleCharacter;
		num = (num * 397) ^ useMaxVisibleDescender.GetHashCode();
		num = (num * 397) ^ (int)fontWeight;
		num = (num * 397) ^ pageToDisplay;
		num = (num * 397) ^ (int)horizontalMapping;
		num = (num * 397) ^ (int)verticalMapping;
		num = (num * 397) ^ uvLineOffset.GetHashCode();
		num = (num * 397) ^ (int)geometrySortingOrder;
		num = (num * 397) ^ inverseYAxis.GetHashCode();
		return (num * 397) ^ charWidthMaxAdj.GetHashCode();
	}

	public static bool operator ==(TextGenerationSettings left, TextGenerationSettings right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(TextGenerationSettings left, TextGenerationSettings right)
	{
		return !object.Equals(left, right);
	}

	public void Copy(TextGenerationSettings other)
	{
		if (!(other == null))
		{
			text = other.text;
			screenRect = other.screenRect;
			margins = other.margins;
			scale = other.scale;
			fontAsset = other.fontAsset;
			material = other.material;
			spriteAsset = other.spriteAsset;
			fontStyle = other.fontStyle;
			textAlignment = other.textAlignment;
			overflowMode = other.overflowMode;
			wordWrap = other.wordWrap;
			wordWrappingRatio = other.wordWrappingRatio;
			color = other.color;
			fontColorGradient = other.fontColorGradient;
			tintSprites = other.tintSprites;
			overrideRichTextColors = other.overrideRichTextColors;
			fontSize = other.fontSize;
			autoSize = other.autoSize;
			fontSizeMin = other.fontSizeMin;
			fontSizeMax = other.fontSizeMax;
			enableKerning = other.enableKerning;
			richText = other.richText;
			isRightToLeft = other.isRightToLeft;
			extraPadding = other.extraPadding;
			parseControlCharacters = other.parseControlCharacters;
			characterSpacing = other.characterSpacing;
			wordSpacing = other.wordSpacing;
			lineSpacing = other.lineSpacing;
			paragraphSpacing = other.paragraphSpacing;
			lineSpacingMax = other.lineSpacingMax;
			maxVisibleCharacters = other.maxVisibleCharacters;
			maxVisibleWords = other.maxVisibleWords;
			maxVisibleLines = other.maxVisibleLines;
			firstVisibleCharacter = other.firstVisibleCharacter;
			useMaxVisibleDescender = other.useMaxVisibleDescender;
			fontWeight = other.fontWeight;
			pageToDisplay = other.pageToDisplay;
			horizontalMapping = other.horizontalMapping;
			verticalMapping = other.verticalMapping;
			uvLineOffset = other.uvLineOffset;
			geometrySortingOrder = other.geometrySortingOrder;
			inverseYAxis = other.inverseYAxis;
			charWidthMaxAdj = other.charWidthMaxAdj;
		}
	}
}
