using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements;

internal struct ComputedStyle
{
	private VisualElement m_Element;

	private VisualElementStylesData stylesData => m_Element.specifiedStyle;

	private InheritedStylesData inheritedStylesData => m_Element.inheritedStyle;

	public StyleLength width => stylesData.width;

	public StyleLength height => stylesData.height;

	public StyleLength maxWidth => stylesData.maxWidth;

	public StyleLength maxHeight => stylesData.maxHeight;

	public StyleLength minWidth => stylesData.minWidth;

	public StyleLength minHeight => stylesData.minHeight;

	public StyleLength flexBasis => stylesData.flexBasis;

	public StyleFloat flexGrow => stylesData.flexGrow;

	public StyleFloat flexShrink => stylesData.flexShrink;

	public StyleEnum<FlexDirection> flexDirection => stylesData.flexDirection.ToStyleEnum((FlexDirection)stylesData.flexDirection.value);

	public StyleEnum<Wrap> flexWrap => stylesData.flexWrap.ToStyleEnum((Wrap)stylesData.flexWrap.value);

	public StyleEnum<Overflow> overflow => stylesData.overflow.ToStyleEnum((Overflow)stylesData.overflow.value);

	public StyleEnum<OverflowClipBox> unityOverflowClipBox => stylesData.unityOverflowClipBox.ToStyleEnum((OverflowClipBox)stylesData.unityOverflowClipBox.value);

	public StyleLength left => stylesData.left;

	public StyleLength top => stylesData.top;

	public StyleLength right => stylesData.right;

	public StyleLength bottom => stylesData.bottom;

	public StyleLength marginLeft => stylesData.marginLeft;

	public StyleLength marginTop => stylesData.marginTop;

	public StyleLength marginRight => stylesData.marginRight;

	public StyleLength marginBottom => stylesData.marginBottom;

	public StyleLength paddingLeft => stylesData.paddingLeft;

	public StyleLength paddingTop => stylesData.paddingTop;

	public StyleLength paddingRight => stylesData.paddingRight;

	public StyleLength paddingBottom => stylesData.paddingBottom;

	public StyleEnum<Position> position => stylesData.position.ToStyleEnum((Position)stylesData.position.value);

	public StyleEnum<Align> alignSelf => stylesData.alignSelf.ToStyleEnum((Align)stylesData.alignSelf.value);

	public StyleColor backgroundColor => stylesData.backgroundColor;

	public StyleBackground backgroundImage => stylesData.backgroundImage;

	public StyleEnum<ScaleMode> unityBackgroundScaleMode => stylesData.unityBackgroundScaleMode.ToStyleEnum((ScaleMode)stylesData.unityBackgroundScaleMode.value);

	public StyleColor unityBackgroundImageTintColor => stylesData.unityBackgroundImageTintColor;

	public StyleEnum<Align> alignItems => stylesData.alignItems.ToStyleEnum((Align)stylesData.alignItems.value);

	public StyleEnum<Align> alignContent => stylesData.alignContent.ToStyleEnum((Align)stylesData.alignContent.value);

	public StyleEnum<Justify> justifyContent => stylesData.justifyContent.ToStyleEnum((Justify)stylesData.justifyContent.value);

	public StyleColor borderLeftColor => stylesData.borderLeftColor;

	public StyleColor borderTopColor => stylesData.borderTopColor;

	public StyleColor borderRightColor => stylesData.borderRightColor;

	public StyleColor borderBottomColor => stylesData.borderBottomColor;

	public StyleFloat borderLeftWidth => stylesData.borderLeftWidth;

	public StyleFloat borderTopWidth => stylesData.borderTopWidth;

	public StyleFloat borderRightWidth => stylesData.borderRightWidth;

	public StyleFloat borderBottomWidth => stylesData.borderBottomWidth;

	public StyleLength borderTopLeftRadius => stylesData.borderTopLeftRadius;

	public StyleLength borderTopRightRadius => stylesData.borderTopRightRadius;

	public StyleLength borderBottomRightRadius => stylesData.borderBottomRightRadius;

	public StyleLength borderBottomLeftRadius => stylesData.borderBottomLeftRadius;

	public StyleInt unitySliceLeft => stylesData.unitySliceLeft;

	public StyleInt unitySliceTop => stylesData.unitySliceTop;

	public StyleInt unitySliceRight => stylesData.unitySliceRight;

	public StyleInt unitySliceBottom => stylesData.unitySliceBottom;

	public StyleFloat opacity => stylesData.opacity;

	public StyleEnum<DisplayStyle> display => stylesData.display.ToStyleEnum((DisplayStyle)stylesData.display.value);

	public StyleCursor cursor => stylesData.cursor;

	public StyleColor color => (stylesData.color.specificity != 0) ? stylesData.color : inheritedStylesData.color;

	public StyleFont unityFont => (stylesData.unityFont.specificity != 0) ? stylesData.unityFont : inheritedStylesData.font;

	public StyleLength fontSize
	{
		get
		{
			int specificity = stylesData.fontSize.specificity;
			if (specificity != 0)
			{
				float v = CalculatePixelFontSize(m_Element);
				StyleLength result = new StyleLength(v);
				result.specificity = specificity;
				return result;
			}
			return inheritedStylesData.fontSize;
		}
	}

	public StyleEnum<FontStyle> unityFontStyleAndWeight
	{
		get
		{
			StyleInt styleInt = ((stylesData.unityFontStyleAndWeight.specificity != 0) ? stylesData.unityFontStyleAndWeight : inheritedStylesData.unityFontStyle);
			return styleInt.ToStyleEnum((FontStyle)styleInt.value);
		}
	}

	public StyleEnum<TextAnchor> unityTextAlign
	{
		get
		{
			StyleInt styleInt = ((stylesData.unityTextAlign.specificity != 0) ? stylesData.unityTextAlign : inheritedStylesData.unityTextAlign);
			return styleInt.ToStyleEnum((TextAnchor)styleInt.value);
		}
	}

	public StyleEnum<Visibility> visibility
	{
		get
		{
			StyleInt styleInt = ((stylesData.visibility.specificity != 0) ? stylesData.visibility : inheritedStylesData.visibility);
			return styleInt.ToStyleEnum((Visibility)styleInt.value);
		}
	}

	public StyleEnum<WhiteSpace> whiteSpace
	{
		get
		{
			StyleInt styleInt = ((stylesData.whiteSpace.specificity != 0) ? stylesData.whiteSpace : inheritedStylesData.whiteSpace);
			return styleInt.ToStyleEnum((WhiteSpace)styleInt.value);
		}
	}

	public ComputedStyle(VisualElement element)
	{
		m_Element = element;
	}

	public static float CalculatePixelFontSize(VisualElement ve)
	{
		Length length = ve.specifiedStyle.fontSize.value;
		if (length.unit == LengthUnit.Percent)
		{
			float num = ve.hierarchy.parent?.resolvedStyle.fontSize ?? 0f;
			float value = num * length.value / 100f;
			length = new Length(value);
		}
		return length.value;
	}
}
