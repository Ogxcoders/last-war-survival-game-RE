using System;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.UIElements.StyleSheets;
using UnityEngine.Yoga;

namespace UnityEngine.UIElements;

internal class InlineStyleAccess : StyleValueCollection, IStyle
{
	private bool m_HasInlineCursor;

	private StyleCursor m_InlineCursor;

	private VisualElement ve { get; set; }

	StyleLength IStyle.width
	{
		get
		{
			return GetStyleLength(StylePropertyID.Width);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.Width, value, ve.sharedStyle.width))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.Width = ve.computedStyle.width.ToYogaValue();
			}
		}
	}

	StyleLength IStyle.height
	{
		get
		{
			return GetStyleLength(StylePropertyID.Height);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.Height, value, ve.sharedStyle.height))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.Height = ve.computedStyle.height.ToYogaValue();
			}
		}
	}

	StyleLength IStyle.maxWidth
	{
		get
		{
			return GetStyleLength(StylePropertyID.MaxWidth);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.MaxWidth, value, ve.sharedStyle.maxWidth))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.MaxWidth = ve.computedStyle.maxWidth.ToYogaValue();
			}
		}
	}

	StyleLength IStyle.maxHeight
	{
		get
		{
			return GetStyleLength(StylePropertyID.MaxHeight);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.MaxHeight, value, ve.sharedStyle.maxHeight))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.MaxHeight = ve.computedStyle.maxHeight.ToYogaValue();
			}
		}
	}

	StyleLength IStyle.minWidth
	{
		get
		{
			return GetStyleLength(StylePropertyID.MinWidth);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.MinWidth, value, ve.sharedStyle.minWidth))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.MinWidth = ve.computedStyle.minWidth.ToYogaValue();
			}
		}
	}

	StyleLength IStyle.minHeight
	{
		get
		{
			return GetStyleLength(StylePropertyID.MinHeight);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.MinHeight, value, ve.sharedStyle.minHeight))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.MinHeight = ve.computedStyle.minHeight.ToYogaValue();
			}
		}
	}

	StyleLength IStyle.flexBasis
	{
		get
		{
			return GetStyleLength(StylePropertyID.FlexBasis);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.FlexBasis, value, ve.sharedStyle.flexBasis))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.FlexBasis = ve.computedStyle.flexBasis.ToYogaValue();
			}
		}
	}

	StyleFloat IStyle.flexGrow
	{
		get
		{
			return GetStyleFloat(StylePropertyID.FlexGrow);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.FlexGrow, value, ve.sharedStyle.flexGrow))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.FlexGrow = ve.computedStyle.flexGrow.value;
			}
		}
	}

	StyleFloat IStyle.flexShrink
	{
		get
		{
			return GetStyleFloat(StylePropertyID.FlexShrink);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.FlexShrink, value, ve.sharedStyle.flexShrink))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.FlexShrink = ve.computedStyle.flexShrink.value;
			}
		}
	}

	StyleEnum<Overflow> IStyle.overflow
	{
		get
		{
			StyleInt styleInt = GetStyleInt(StylePropertyID.Overflow);
			return new StyleEnum<Overflow>((Overflow)styleInt.value, styleInt.keyword);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.Overflow, value, ve.sharedStyle.overflow))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.Overflow);
				ve.yogaNode.Overflow = (YogaOverflow)ve.computedStyle.overflow.value;
			}
		}
	}

	StyleEnum<OverflowClipBox> IStyle.unityOverflowClipBox
	{
		get
		{
			StyleInt styleInt = GetStyleInt(StylePropertyID.OverflowClipBox);
			return new StyleEnum<OverflowClipBox>((OverflowClipBox)styleInt.value, styleInt.keyword);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.OverflowClipBox, value, ve.sharedStyle.unityOverflowClipBox))
			{
				ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
			}
		}
	}

	StyleLength IStyle.left
	{
		get
		{
			return GetStyleLength(StylePropertyID.PositionLeft);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.PositionLeft, value, ve.sharedStyle.left))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.Left = ve.computedStyle.left.ToYogaValue();
			}
		}
	}

	StyleLength IStyle.top
	{
		get
		{
			return GetStyleLength(StylePropertyID.PositionTop);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.PositionTop, value, ve.sharedStyle.top))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.Top = ve.computedStyle.top.ToYogaValue();
			}
		}
	}

	StyleLength IStyle.right
	{
		get
		{
			return GetStyleLength(StylePropertyID.PositionRight);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.PositionRight, value, ve.sharedStyle.right))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.Right = ve.computedStyle.right.ToYogaValue();
			}
		}
	}

	StyleLength IStyle.bottom
	{
		get
		{
			return GetStyleLength(StylePropertyID.PositionBottom);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.PositionBottom, value, ve.sharedStyle.bottom))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.Bottom = ve.computedStyle.bottom.ToYogaValue();
			}
		}
	}

	StyleLength IStyle.marginLeft
	{
		get
		{
			return GetStyleLength(StylePropertyID.MarginLeft);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.MarginLeft, value, ve.sharedStyle.marginLeft))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.MarginLeft = ve.computedStyle.marginLeft.ToYogaValue();
			}
		}
	}

	StyleLength IStyle.marginTop
	{
		get
		{
			return GetStyleLength(StylePropertyID.MarginTop);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.MarginTop, value, ve.sharedStyle.marginTop))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.MarginTop = ve.computedStyle.marginTop.ToYogaValue();
			}
		}
	}

	StyleLength IStyle.marginRight
	{
		get
		{
			return GetStyleLength(StylePropertyID.MarginRight);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.MarginRight, value, ve.sharedStyle.marginRight))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.MarginRight = ve.computedStyle.marginRight.ToYogaValue();
			}
		}
	}

	StyleLength IStyle.marginBottom
	{
		get
		{
			return GetStyleLength(StylePropertyID.MarginBottom);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.MarginBottom, value, ve.sharedStyle.marginBottom))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.MarginBottom = ve.computedStyle.marginBottom.ToYogaValue();
			}
		}
	}

	StyleColor IStyle.borderLeftColor
	{
		get
		{
			return GetStyleColor(StylePropertyID.BorderLeftColor);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.BorderLeftColor, value, ve.sharedStyle.borderLeftColor))
			{
				ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
			}
		}
	}

	StyleColor IStyle.borderTopColor
	{
		get
		{
			return GetStyleColor(StylePropertyID.BorderTopColor);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.BorderTopColor, value, ve.sharedStyle.borderTopColor))
			{
				ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
			}
		}
	}

	StyleColor IStyle.borderRightColor
	{
		get
		{
			return GetStyleColor(StylePropertyID.BorderRightColor);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.BorderRightColor, value, ve.sharedStyle.borderRightColor))
			{
				ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
			}
		}
	}

	StyleColor IStyle.borderBottomColor
	{
		get
		{
			return GetStyleColor(StylePropertyID.BorderBottomColor);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.BorderBottomColor, value, ve.sharedStyle.borderBottomColor))
			{
				ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
			}
		}
	}

	StyleFloat IStyle.borderLeftWidth
	{
		get
		{
			return GetStyleFloat(StylePropertyID.BorderLeftWidth);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.BorderLeftWidth, value, ve.sharedStyle.borderLeftWidth))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.BorderWidth | VersionChangeType.Repaint);
				ve.yogaNode.BorderLeftWidth = ve.computedStyle.borderLeftWidth.value;
			}
		}
	}

	StyleFloat IStyle.borderTopWidth
	{
		get
		{
			return GetStyleFloat(StylePropertyID.BorderTopWidth);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.BorderTopWidth, value, ve.sharedStyle.borderTopWidth))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.BorderWidth | VersionChangeType.Repaint);
				ve.yogaNode.BorderTopWidth = ve.computedStyle.borderTopWidth.value;
			}
		}
	}

	StyleFloat IStyle.borderRightWidth
	{
		get
		{
			return GetStyleFloat(StylePropertyID.BorderRightWidth);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.BorderRightWidth, value, ve.sharedStyle.borderRightWidth))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.BorderWidth | VersionChangeType.Repaint);
				ve.yogaNode.BorderRightWidth = ve.computedStyle.borderRightWidth.value;
			}
		}
	}

	StyleFloat IStyle.borderBottomWidth
	{
		get
		{
			return GetStyleFloat(StylePropertyID.BorderBottomWidth);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.BorderBottomWidth, value, ve.sharedStyle.borderBottomWidth))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.BorderWidth | VersionChangeType.Repaint);
				ve.yogaNode.BorderBottomWidth = ve.computedStyle.borderBottomWidth.value;
			}
		}
	}

	StyleLength IStyle.borderTopLeftRadius
	{
		get
		{
			return GetStyleLength(StylePropertyID.BorderTopLeftRadius);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.BorderTopLeftRadius, value, ve.sharedStyle.borderTopLeftRadius))
			{
				ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.BorderRadius | VersionChangeType.Repaint);
			}
		}
	}

	StyleLength IStyle.borderTopRightRadius
	{
		get
		{
			return GetStyleLength(StylePropertyID.BorderTopRightRadius);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.BorderTopRightRadius, value, ve.sharedStyle.borderTopRightRadius))
			{
				ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.BorderRadius | VersionChangeType.Repaint);
			}
		}
	}

	StyleLength IStyle.borderBottomRightRadius
	{
		get
		{
			return GetStyleLength(StylePropertyID.BorderBottomRightRadius);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.BorderBottomRightRadius, value, ve.sharedStyle.borderBottomRightRadius))
			{
				ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.BorderRadius | VersionChangeType.Repaint);
			}
		}
	}

	StyleLength IStyle.borderBottomLeftRadius
	{
		get
		{
			return GetStyleLength(StylePropertyID.BorderBottomLeftRadius);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.BorderBottomLeftRadius, value, ve.sharedStyle.borderBottomLeftRadius))
			{
				ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.BorderRadius | VersionChangeType.Repaint);
			}
		}
	}

	StyleLength IStyle.paddingLeft
	{
		get
		{
			return GetStyleLength(StylePropertyID.PaddingLeft);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.PaddingLeft, value, ve.sharedStyle.paddingLeft))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.PaddingLeft = ve.computedStyle.paddingLeft.ToYogaValue();
			}
		}
	}

	StyleLength IStyle.paddingTop
	{
		get
		{
			return GetStyleLength(StylePropertyID.PaddingTop);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.PaddingTop, value, ve.sharedStyle.paddingTop))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.PaddingTop = ve.computedStyle.paddingTop.ToYogaValue();
			}
		}
	}

	StyleLength IStyle.paddingRight
	{
		get
		{
			return GetStyleLength(StylePropertyID.PaddingRight);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.PaddingRight, value, ve.sharedStyle.paddingRight))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.PaddingRight = ve.computedStyle.paddingRight.ToYogaValue();
			}
		}
	}

	StyleLength IStyle.paddingBottom
	{
		get
		{
			return GetStyleLength(StylePropertyID.PaddingBottom);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.PaddingBottom, value, ve.sharedStyle.paddingBottom))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.PaddingBottom = ve.computedStyle.paddingBottom.ToYogaValue();
			}
		}
	}

	StyleEnum<Position> IStyle.position
	{
		get
		{
			StyleInt styleInt = GetStyleInt(StylePropertyID.Position);
			return new StyleEnum<Position>((Position)styleInt.value, styleInt.keyword);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.Position, value, ve.sharedStyle.position))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.PositionType = (YogaPositionType)ve.computedStyle.position.value;
			}
		}
	}

	StyleEnum<Align> IStyle.alignSelf
	{
		get
		{
			StyleInt styleInt = GetStyleInt(StylePropertyID.AlignSelf);
			return new StyleEnum<Align>((Align)styleInt.value, styleInt.keyword);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.AlignSelf, value, ve.sharedStyle.alignSelf))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.AlignSelf = (YogaAlign)ve.computedStyle.alignSelf.value;
			}
		}
	}

	StyleEnum<TextAnchor> IStyle.unityTextAlign
	{
		get
		{
			StyleInt styleInt = GetStyleInt(StylePropertyID.UnityTextAlign);
			return new StyleEnum<TextAnchor>((TextAnchor)styleInt.value, styleInt.keyword);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.UnityTextAlign, value, ve.sharedStyle.unityTextAlign))
			{
				ve.IncrementVersion(VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
			}
		}
	}

	StyleEnum<FontStyle> IStyle.unityFontStyleAndWeight
	{
		get
		{
			StyleInt styleInt = GetStyleInt(StylePropertyID.FontStyleAndWeight);
			return new StyleEnum<FontStyle>((FontStyle)styleInt.value, styleInt.keyword);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.FontStyleAndWeight, value, ve.sharedStyle.unityFontStyleAndWeight))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Styles);
			}
		}
	}

	StyleFont IStyle.unityFont
	{
		get
		{
			return GetStyleFont(StylePropertyID.Font);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.Font, value, ve.sharedStyle.unityFont))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Styles);
			}
		}
	}

	StyleLength IStyle.fontSize
	{
		get
		{
			return GetStyleLength(StylePropertyID.FontSize);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.FontSize, value, ve.sharedStyle.fontSize))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Styles);
			}
		}
	}

	StyleEnum<WhiteSpace> IStyle.whiteSpace
	{
		get
		{
			StyleInt styleInt = GetStyleInt(StylePropertyID.WhiteSpace);
			return new StyleEnum<WhiteSpace>((WhiteSpace)styleInt.value, styleInt.keyword);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.WhiteSpace, value, ve.sharedStyle.whiteSpace))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Styles);
			}
		}
	}

	StyleColor IStyle.color
	{
		get
		{
			return GetStyleColor(StylePropertyID.Color);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.Color, value, ve.sharedStyle.color))
			{
				ve.IncrementVersion(VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
			}
		}
	}

	StyleEnum<FlexDirection> IStyle.flexDirection
	{
		get
		{
			StyleInt styleInt = GetStyleInt(StylePropertyID.FlexDirection);
			return new StyleEnum<FlexDirection>((FlexDirection)styleInt.value, styleInt.keyword);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.FlexDirection, value, ve.sharedStyle.flexDirection))
			{
				ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
				ve.yogaNode.FlexDirection = (YogaFlexDirection)ve.computedStyle.flexDirection.value;
			}
		}
	}

	StyleColor IStyle.backgroundColor
	{
		get
		{
			return GetStyleColor(StylePropertyID.BackgroundColor);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.BackgroundColor, value, ve.sharedStyle.backgroundColor))
			{
				ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
			}
		}
	}

	StyleColor IStyle.borderColor
	{
		get
		{
			return GetStyleColor(StylePropertyID.BorderLeftColor);
		}
		set
		{
			bool flag = SetStyleValue(StylePropertyID.BorderLeftColor, value, ve.sharedStyle.borderLeftColor);
			flag |= SetStyleValue(StylePropertyID.BorderTopColor, value, ve.sharedStyle.borderTopColor);
			flag |= SetStyleValue(StylePropertyID.BorderRightColor, value, ve.sharedStyle.borderRightColor);
			if (flag | SetStyleValue(StylePropertyID.BorderBottomColor, value, ve.sharedStyle.borderBottomColor))
			{
				ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
			}
		}
	}

	StyleBackground IStyle.backgroundImage
	{
		get
		{
			return GetStyleBackground(StylePropertyID.BackgroundImage);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.BackgroundImage, value, ve.sharedStyle.backgroundImage))
			{
				ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
			}
		}
	}

	StyleEnum<ScaleMode> IStyle.unityBackgroundScaleMode
	{
		get
		{
			StyleInt styleInt = GetStyleInt(StylePropertyID.BackgroundScaleMode);
			return new StyleEnum<ScaleMode>((ScaleMode)styleInt.value, styleInt.keyword);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.BackgroundScaleMode, value, ve.sharedStyle.unityBackgroundScaleMode))
			{
				ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
			}
		}
	}

	StyleColor IStyle.unityBackgroundImageTintColor
	{
		get
		{
			return GetStyleColor(StylePropertyID.BackgroundImageTintColor);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.BackgroundImageTintColor, value, ve.sharedStyle.unityBackgroundImageTintColor))
			{
				ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
			}
		}
	}

	StyleEnum<Align> IStyle.alignItems
	{
		get
		{
			StyleInt styleInt = GetStyleInt(StylePropertyID.AlignItems);
			return new StyleEnum<Align>((Align)styleInt.value, styleInt.keyword);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.AlignItems, value, ve.sharedStyle.alignItems))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.AlignItems = (YogaAlign)ve.computedStyle.alignItems.value;
			}
		}
	}

	StyleEnum<Align> IStyle.alignContent
	{
		get
		{
			StyleInt styleInt = GetStyleInt(StylePropertyID.AlignContent);
			return new StyleEnum<Align>((Align)styleInt.value, styleInt.keyword);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.AlignContent, value, ve.sharedStyle.alignContent))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.AlignContent = (YogaAlign)ve.computedStyle.alignContent.value;
			}
		}
	}

	StyleEnum<Justify> IStyle.justifyContent
	{
		get
		{
			StyleInt styleInt = GetStyleInt(StylePropertyID.JustifyContent);
			return new StyleEnum<Justify>((Justify)styleInt.value, styleInt.keyword);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.JustifyContent, value, ve.sharedStyle.justifyContent))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.JustifyContent = (YogaJustify)ve.computedStyle.justifyContent.value;
			}
		}
	}

	StyleEnum<Wrap> IStyle.flexWrap
	{
		get
		{
			StyleInt styleInt = GetStyleInt(StylePropertyID.FlexWrap);
			return new StyleEnum<Wrap>((Wrap)styleInt.value, styleInt.keyword);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.FlexWrap, value, ve.sharedStyle.flexWrap))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.Wrap = (YogaWrap)ve.computedStyle.flexWrap.value;
			}
		}
	}

	StyleInt IStyle.unitySliceLeft
	{
		get
		{
			return GetStyleInt(StylePropertyID.SliceLeft);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.SliceLeft, value, ve.sharedStyle.unitySliceLeft))
			{
				ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
			}
		}
	}

	StyleInt IStyle.unitySliceTop
	{
		get
		{
			return GetStyleInt(StylePropertyID.SliceTop);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.SliceTop, value, ve.sharedStyle.unitySliceTop))
			{
				ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
			}
		}
	}

	StyleInt IStyle.unitySliceRight
	{
		get
		{
			return GetStyleInt(StylePropertyID.SliceRight);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.SliceRight, value, ve.sharedStyle.unitySliceRight))
			{
				ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
			}
		}
	}

	StyleInt IStyle.unitySliceBottom
	{
		get
		{
			return GetStyleInt(StylePropertyID.SliceBottom);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.SliceBottom, value, ve.sharedStyle.unitySliceBottom))
			{
				ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
			}
		}
	}

	StyleFloat IStyle.opacity
	{
		get
		{
			return GetStyleFloat(StylePropertyID.Opacity);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.Opacity, value, ve.sharedStyle.opacity))
			{
				ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Opacity);
			}
		}
	}

	StyleEnum<Visibility> IStyle.visibility
	{
		get
		{
			StyleInt styleInt = GetStyleInt(StylePropertyID.Visibility);
			return new StyleEnum<Visibility>((Visibility)styleInt.value, styleInt.keyword);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.Visibility, value, ve.sharedStyle.visibility))
			{
				ve.IncrementVersion(VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
			}
		}
	}

	StyleCursor IStyle.cursor
	{
		get
		{
			StyleCursor value = default(StyleCursor);
			if (TryGetInlineCursor(ref value))
			{
				return value;
			}
			return StyleKeyword.Null;
		}
		set
		{
			if (SetInlineCursor(value, ve.sharedStyle.cursor))
			{
				ve.IncrementVersion(VersionChangeType.Styles);
			}
		}
	}

	StyleEnum<DisplayStyle> IStyle.display
	{
		get
		{
			StyleInt styleInt = GetStyleInt(StylePropertyID.Display);
			return new StyleEnum<DisplayStyle>((DisplayStyle)styleInt.value, styleInt.keyword);
		}
		set
		{
			if (SetStyleValue(StylePropertyID.Display, value, ve.sharedStyle.display))
			{
				ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
				ve.yogaNode.Display = (YogaDisplay)ve.computedStyle.display.value;
			}
		}
	}

	public InlineStyleAccess(VisualElement ve)
	{
		this.ve = ve;
		if (ve.specifiedStyle.isShared)
		{
			VisualElementStylesData visualElementStylesData = new VisualElementStylesData(isShared: false);
			visualElementStylesData.Apply(ve.m_SharedStyle, StylePropertyApplyMode.Copy);
			ve.m_Style = visualElementStylesData;
		}
	}

	~InlineStyleAccess()
	{
		StyleValue value = default(StyleValue);
		if (TryGetStyleValue(StylePropertyID.BackgroundImage, ref value) && value.resource.IsAllocated)
		{
			value.resource.Free();
		}
		if (TryGetStyleValue(StylePropertyID.Font, ref value) && value.resource.IsAllocated)
		{
			value.resource.Free();
		}
	}

	private bool SetStyleValue(StylePropertyID id, StyleLength inlineValue, StyleLength sharedValue)
	{
		StyleValue value = default(StyleValue);
		if (TryGetStyleValue(id, ref value) && value.length == inlineValue.value && value.keyword == inlineValue.keyword)
		{
			return false;
		}
		value.id = id;
		value.keyword = inlineValue.keyword;
		value.length = inlineValue.value;
		SetStyleValue(value);
		int specificity = int.MaxValue;
		if (inlineValue.keyword == StyleKeyword.Null)
		{
			specificity = sharedValue.specificity;
			value.keyword = sharedValue.keyword;
			value.length = sharedValue.value;
		}
		ApplyStyleValue(value, specificity);
		return true;
	}

	private bool SetStyleValue(StylePropertyID id, StyleFloat inlineValue, StyleFloat sharedValue)
	{
		StyleValue value = default(StyleValue);
		if (TryGetStyleValue(id, ref value) && value.number == inlineValue.value && value.keyword == inlineValue.keyword)
		{
			return false;
		}
		value.id = id;
		value.keyword = inlineValue.keyword;
		value.number = inlineValue.value;
		SetStyleValue(value);
		int specificity = int.MaxValue;
		if (inlineValue.keyword == StyleKeyword.Null)
		{
			specificity = sharedValue.specificity;
			value.keyword = sharedValue.keyword;
			value.number = sharedValue.value;
		}
		ApplyStyleValue(value, specificity);
		return true;
	}

	private bool SetStyleValue(StylePropertyID id, StyleInt inlineValue, StyleInt sharedValue)
	{
		StyleValue value = default(StyleValue);
		if (TryGetStyleValue(id, ref value) && value.number == (float)inlineValue.value && value.keyword == inlineValue.keyword)
		{
			return false;
		}
		value.id = id;
		value.keyword = inlineValue.keyword;
		value.number = inlineValue.value;
		SetStyleValue(value);
		int specificity = int.MaxValue;
		if (inlineValue.keyword == StyleKeyword.Null)
		{
			specificity = sharedValue.specificity;
			value.keyword = sharedValue.keyword;
			value.number = sharedValue.value;
		}
		ApplyStyleValue(value, specificity);
		return true;
	}

	private bool SetStyleValue(StylePropertyID id, StyleColor inlineValue, StyleColor sharedValue)
	{
		StyleValue value = default(StyleValue);
		if (TryGetStyleValue(id, ref value) && value.color == inlineValue.value && value.keyword == inlineValue.keyword)
		{
			return false;
		}
		value.id = id;
		value.keyword = inlineValue.keyword;
		value.color = inlineValue.value;
		SetStyleValue(value);
		int specificity = int.MaxValue;
		if (inlineValue.keyword == StyleKeyword.Null)
		{
			specificity = sharedValue.specificity;
			value.keyword = sharedValue.keyword;
			value.color = sharedValue.value;
		}
		ApplyStyleValue(value, specificity);
		return true;
	}

	private bool SetStyleValue<T>(StylePropertyID id, StyleEnum<T> inlineValue, StyleInt sharedValue) where T : struct, IConvertible
	{
		StyleValue value = default(StyleValue);
		int num = UnsafeUtility.EnumToInt(inlineValue.value);
		if (TryGetStyleValue(id, ref value) && value.number == (float)num && value.keyword == inlineValue.keyword)
		{
			return false;
		}
		value.id = id;
		value.keyword = inlineValue.keyword;
		value.number = num;
		SetStyleValue(value);
		int specificity = int.MaxValue;
		if (inlineValue.keyword == StyleKeyword.Null)
		{
			specificity = sharedValue.specificity;
			value.keyword = sharedValue.keyword;
			value.number = sharedValue.value;
		}
		ApplyStyleValue(value, specificity);
		return true;
	}

	private bool SetStyleValue(StylePropertyID id, StyleBackground inlineValue, StyleBackground sharedValue)
	{
		StyleValue value = default(StyleValue);
		if (TryGetStyleValue(id, ref value))
		{
			VectorImage vectorImage = (value.resource.IsAllocated ? (value.resource.Target as VectorImage) : null);
			Texture2D texture2D = (value.resource.IsAllocated ? (value.resource.Target as Texture2D) : null);
			if (vectorImage == inlineValue.value.vectorImage && texture2D == inlineValue.value.texture && value.keyword == inlineValue.keyword)
			{
				return false;
			}
			if (value.resource.IsAllocated)
			{
				value.resource.Free();
			}
		}
		value.id = id;
		value.keyword = inlineValue.keyword;
		if (inlineValue.value.vectorImage != null)
		{
			value.resource = GCHandle.Alloc(inlineValue.value.vectorImage);
		}
		else if (inlineValue.value.texture != null)
		{
			value.resource = GCHandle.Alloc(inlineValue.value.texture);
		}
		else
		{
			value.resource = default(GCHandle);
		}
		SetStyleValue(value);
		int specificity = int.MaxValue;
		if (inlineValue.keyword == StyleKeyword.Null)
		{
			specificity = sharedValue.specificity;
			value.keyword = sharedValue.keyword;
			if (sharedValue.value.texture != null)
			{
				value.resource = GCHandle.Alloc(sharedValue.value.texture);
			}
			else if (sharedValue.value.vectorImage != null)
			{
				value.resource = GCHandle.Alloc(sharedValue.value.vectorImage);
			}
			else
			{
				value.resource = default(GCHandle);
			}
		}
		ApplyStyleValue(value, specificity);
		return true;
	}

	private bool SetStyleValue(StylePropertyID id, StyleFont inlineValue, StyleFont sharedValue)
	{
		StyleValue value = default(StyleValue);
		if (TryGetStyleValue(id, ref value) && value.resource.IsAllocated)
		{
			Font font = (value.resource.IsAllocated ? (value.resource.Target as Font) : null);
			if (font == inlineValue.value && value.keyword == inlineValue.keyword)
			{
				return false;
			}
			if (value.resource.IsAllocated)
			{
				value.resource.Free();
			}
		}
		value.id = id;
		value.keyword = inlineValue.keyword;
		value.resource = ((inlineValue.value != null) ? GCHandle.Alloc(inlineValue.value) : default(GCHandle));
		SetStyleValue(value);
		int specificity = int.MaxValue;
		if (inlineValue.keyword == StyleKeyword.Null)
		{
			specificity = sharedValue.specificity;
			value.keyword = sharedValue.keyword;
			value.resource = ((sharedValue.value != null) ? GCHandle.Alloc(sharedValue.value) : default(GCHandle));
		}
		ApplyStyleValue(value, specificity);
		return true;
	}

	private bool SetInlineCursor(StyleCursor inlineValue, StyleCursor sharedValue)
	{
		StyleCursor value = default(StyleCursor);
		if (TryGetInlineCursor(ref value) && value.value == inlineValue.value && value.keyword == inlineValue.keyword)
		{
			return false;
		}
		value.value = inlineValue.value;
		value.keyword = inlineValue.keyword;
		SetInlineCursor(value);
		int specificity = int.MaxValue;
		if (value.keyword == StyleKeyword.Null)
		{
			specificity = sharedValue.specificity;
			value.keyword = sharedValue.keyword;
			value.value = sharedValue.value;
		}
		ve.specifiedStyle.ApplyStyleCursor(value, specificity);
		return true;
	}

	private void ApplyStyleValue(StyleValue value, int specificity)
	{
		ve.specifiedStyle.ApplyStyleValue(value.id, value, specificity);
	}

	public bool TryGetInlineCursor(ref StyleCursor value)
	{
		if (m_HasInlineCursor)
		{
			value = m_InlineCursor;
			return true;
		}
		return false;
	}

	public void SetInlineCursor(StyleCursor value)
	{
		m_InlineCursor = value;
		m_HasInlineCursor = true;
	}
}
