#define UNITY_ASSERTIONS
using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements.StyleSheets;

internal static class StyleSheetCache
{
	private struct SheetHandleKey
	{
		public readonly int sheetInstanceID;

		public readonly int index;

		public SheetHandleKey(StyleSheet sheet, int index)
		{
			sheetInstanceID = sheet.GetInstanceID();
			this.index = index;
		}
	}

	private class SheetHandleKeyComparer : IEqualityComparer<SheetHandleKey>
	{
		public bool Equals(SheetHandleKey x, SheetHandleKey y)
		{
			return x.sheetInstanceID == y.sheetInstanceID && x.index == y.index;
		}

		public int GetHashCode(SheetHandleKey key)
		{
			int sheetInstanceID = key.sheetInstanceID;
			int hashCode = sheetInstanceID.GetHashCode();
			sheetInstanceID = key.index;
			return hashCode ^ sheetInstanceID.GetHashCode();
		}
	}

	private static SheetHandleKeyComparer s_Comparer;

	private static Dictionary<SheetHandleKey, int> s_EnumToIntCache;

	private static Dictionary<SheetHandleKey, StylePropertyID[]> s_RulePropertyIDsCache;

	private static Dictionary<string, StylePropertyID> s_NameToIDCache;

	private static StyleValue[] s_InitialStyleValues;

	private static Dictionary<string, string> s_DeprecatedNames;

	internal static string GetPropertyIDUssName(StylePropertyID propertyId)
	{
		foreach (KeyValuePair<string, StylePropertyID> item in s_NameToIDCache)
		{
			if (propertyId == item.Value)
			{
				return item.Key;
			}
		}
		return string.Empty;
	}

	static StyleSheetCache()
	{
		s_Comparer = new SheetHandleKeyComparer();
		s_EnumToIntCache = new Dictionary<SheetHandleKey, int>(s_Comparer);
		s_RulePropertyIDsCache = new Dictionary<SheetHandleKey, StylePropertyID[]>(s_Comparer);
		s_NameToIDCache = new Dictionary<string, StylePropertyID>
		{
			{
				"width",
				StylePropertyID.Width
			},
			{
				"height",
				StylePropertyID.Height
			},
			{
				"max-width",
				StylePropertyID.MaxWidth
			},
			{
				"max-height",
				StylePropertyID.MaxHeight
			},
			{
				"min-width",
				StylePropertyID.MinWidth
			},
			{
				"min-height",
				StylePropertyID.MinHeight
			},
			{
				"flex-wrap",
				StylePropertyID.FlexWrap
			},
			{
				"flex-basis",
				StylePropertyID.FlexBasis
			},
			{
				"flex-grow",
				StylePropertyID.FlexGrow
			},
			{
				"flex-shrink",
				StylePropertyID.FlexShrink
			},
			{
				"overflow",
				StylePropertyID.Overflow
			},
			{
				"-unity-overflow-clip-box",
				StylePropertyID.OverflowClipBox
			},
			{
				"left",
				StylePropertyID.PositionLeft
			},
			{
				"top",
				StylePropertyID.PositionTop
			},
			{
				"right",
				StylePropertyID.PositionRight
			},
			{
				"bottom",
				StylePropertyID.PositionBottom
			},
			{
				"margin-left",
				StylePropertyID.MarginLeft
			},
			{
				"margin-top",
				StylePropertyID.MarginTop
			},
			{
				"margin-right",
				StylePropertyID.MarginRight
			},
			{
				"margin-bottom",
				StylePropertyID.MarginBottom
			},
			{
				"padding-left",
				StylePropertyID.PaddingLeft
			},
			{
				"padding-top",
				StylePropertyID.PaddingTop
			},
			{
				"padding-right",
				StylePropertyID.PaddingRight
			},
			{
				"padding-bottom",
				StylePropertyID.PaddingBottom
			},
			{
				"position",
				StylePropertyID.Position
			},
			{
				"-unity-text-align",
				StylePropertyID.UnityTextAlign
			},
			{
				"-unity-font-style",
				StylePropertyID.FontStyleAndWeight
			},
			{
				"-unity-font",
				StylePropertyID.Font
			},
			{
				"font-size",
				StylePropertyID.FontSize
			},
			{
				"white-space",
				StylePropertyID.WhiteSpace
			},
			{
				"color",
				StylePropertyID.Color
			},
			{
				"flex-direction",
				StylePropertyID.FlexDirection
			},
			{
				"background-color",
				StylePropertyID.BackgroundColor
			},
			{
				"background-image",
				StylePropertyID.BackgroundImage
			},
			{
				"-unity-background-scale-mode",
				StylePropertyID.BackgroundScaleMode
			},
			{
				"-unity-background-image-tint-color",
				StylePropertyID.BackgroundImageTintColor
			},
			{
				"align-content",
				StylePropertyID.AlignContent
			},
			{
				"align-items",
				StylePropertyID.AlignItems
			},
			{
				"align-self",
				StylePropertyID.AlignSelf
			},
			{
				"justify-content",
				StylePropertyID.JustifyContent
			},
			{
				"border-left-color",
				StylePropertyID.BorderLeftColor
			},
			{
				"border-top-color",
				StylePropertyID.BorderTopColor
			},
			{
				"border-right-color",
				StylePropertyID.BorderRightColor
			},
			{
				"border-bottom-color",
				StylePropertyID.BorderBottomColor
			},
			{
				"border-left-width",
				StylePropertyID.BorderLeftWidth
			},
			{
				"border-top-width",
				StylePropertyID.BorderTopWidth
			},
			{
				"border-right-width",
				StylePropertyID.BorderRightWidth
			},
			{
				"border-bottom-width",
				StylePropertyID.BorderBottomWidth
			},
			{
				"border-top-left-radius",
				StylePropertyID.BorderTopLeftRadius
			},
			{
				"border-top-right-radius",
				StylePropertyID.BorderTopRightRadius
			},
			{
				"border-bottom-right-radius",
				StylePropertyID.BorderBottomRightRadius
			},
			{
				"border-bottom-left-radius",
				StylePropertyID.BorderBottomLeftRadius
			},
			{
				"-unity-slice-left",
				StylePropertyID.SliceLeft
			},
			{
				"-unity-slice-top",
				StylePropertyID.SliceTop
			},
			{
				"-unity-slice-right",
				StylePropertyID.SliceRight
			},
			{
				"-unity-slice-bottom",
				StylePropertyID.SliceBottom
			},
			{
				"opacity",
				StylePropertyID.Opacity
			},
			{
				"cursor",
				StylePropertyID.Cursor
			},
			{
				"visibility",
				StylePropertyID.Visibility
			},
			{
				"display",
				StylePropertyID.Display
			},
			{
				"border-color",
				StylePropertyID.BorderColor
			},
			{
				"border-radius",
				StylePropertyID.BorderRadius
			},
			{
				"border-width",
				StylePropertyID.BorderWidth
			},
			{
				"flex",
				StylePropertyID.Flex
			},
			{
				"margin",
				StylePropertyID.Margin
			},
			{
				"padding",
				StylePropertyID.Padding
			}
		};
		s_InitialStyleValues = new StyleValue[66];
		s_DeprecatedNames = new Dictionary<string, string>
		{
			{ "position-left", "left" },
			{ "position-top", "top" },
			{ "position-right", "right" },
			{ "position-bottom", "bottom" },
			{ "text-color", "color" },
			{ "slice-left", "-unity-slice-left" },
			{ "slice-top", "-unity-slice-top" },
			{ "slice-right", "-unity-slice-right" },
			{ "slice-bottom", "-unity-slice-bottom" },
			{ "text-alignment", "-unity-text-align" },
			{ "word-wrap", "-unity-word-wrap" },
			{ "font", "-unity-font" },
			{ "background-size", "-unity-background-scale-mode" },
			{ "font-style", "-unity-font-style" },
			{ "position-type", "position" },
			{ "border-left", "border-left-width" },
			{ "border-top", "border-top-width" },
			{ "border-right", "border-right-width" },
			{ "border-bottom", "border-bottom-width" }
		};
		s_InitialStyleValues[0] = StyleValue.Create(StylePropertyID.MarginLeft, 0f);
		s_InitialStyleValues[1] = StyleValue.Create(StylePropertyID.MarginTop, 0f);
		s_InitialStyleValues[2] = StyleValue.Create(StylePropertyID.MarginRight, 0f);
		s_InitialStyleValues[3] = StyleValue.Create(StylePropertyID.MarginBottom, 0f);
		s_InitialStyleValues[4] = StyleValue.Create(StylePropertyID.PaddingLeft, 0f);
		s_InitialStyleValues[5] = StyleValue.Create(StylePropertyID.PaddingTop, 0f);
		s_InitialStyleValues[6] = StyleValue.Create(StylePropertyID.PaddingRight, 0f);
		s_InitialStyleValues[7] = StyleValue.Create(StylePropertyID.PaddingBottom, 0f);
		s_InitialStyleValues[8] = StyleValue.Create(StylePropertyID.Position, 0);
		s_InitialStyleValues[9] = StyleValue.Create(StylePropertyID.PositionLeft, StyleKeyword.Auto);
		s_InitialStyleValues[10] = StyleValue.Create(StylePropertyID.PositionTop, StyleKeyword.Auto);
		s_InitialStyleValues[11] = StyleValue.Create(StylePropertyID.PositionRight, StyleKeyword.Auto);
		s_InitialStyleValues[12] = StyleValue.Create(StylePropertyID.PositionBottom, StyleKeyword.Auto);
		s_InitialStyleValues[13] = StyleValue.Create(StylePropertyID.Width, StyleKeyword.Auto);
		s_InitialStyleValues[14] = StyleValue.Create(StylePropertyID.Height, StyleKeyword.Auto);
		s_InitialStyleValues[15] = StyleValue.Create(StylePropertyID.MinWidth, StyleKeyword.Auto);
		s_InitialStyleValues[16] = StyleValue.Create(StylePropertyID.MinHeight, StyleKeyword.Auto);
		s_InitialStyleValues[17] = StyleValue.Create(StylePropertyID.MaxWidth, StyleKeyword.None);
		s_InitialStyleValues[18] = StyleValue.Create(StylePropertyID.MaxHeight, StyleKeyword.None);
		s_InitialStyleValues[19] = StyleValue.Create(StylePropertyID.FlexBasis, StyleKeyword.Auto);
		s_InitialStyleValues[20] = StyleValue.Create(StylePropertyID.FlexGrow, 0f);
		s_InitialStyleValues[21] = StyleValue.Create(StylePropertyID.FlexShrink, 1f);
		s_InitialStyleValues[22] = StyleValue.Create(StylePropertyID.BorderLeftColor, Color.clear);
		s_InitialStyleValues[23] = StyleValue.Create(StylePropertyID.BorderTopColor, Color.clear);
		s_InitialStyleValues[24] = StyleValue.Create(StylePropertyID.BorderRightColor, Color.clear);
		s_InitialStyleValues[25] = StyleValue.Create(StylePropertyID.BorderBottomColor, Color.clear);
		s_InitialStyleValues[26] = StyleValue.Create(StylePropertyID.BorderLeftWidth, 0f);
		s_InitialStyleValues[27] = StyleValue.Create(StylePropertyID.BorderTopWidth, 0f);
		s_InitialStyleValues[28] = StyleValue.Create(StylePropertyID.BorderRightWidth, 0f);
		s_InitialStyleValues[29] = StyleValue.Create(StylePropertyID.BorderBottomWidth, 0f);
		s_InitialStyleValues[30] = StyleValue.Create(StylePropertyID.BorderTopLeftRadius, 0f);
		s_InitialStyleValues[31] = StyleValue.Create(StylePropertyID.BorderTopRightRadius, 0f);
		s_InitialStyleValues[33] = StyleValue.Create(StylePropertyID.BorderBottomLeftRadius, 0f);
		s_InitialStyleValues[32] = StyleValue.Create(StylePropertyID.BorderBottomRightRadius, 0f);
		s_InitialStyleValues[34] = StyleValue.Create(StylePropertyID.FlexDirection, 0);
		s_InitialStyleValues[35] = StyleValue.Create(StylePropertyID.FlexWrap, 0);
		s_InitialStyleValues[36] = StyleValue.Create(StylePropertyID.JustifyContent, 0);
		s_InitialStyleValues[37] = StyleValue.Create(StylePropertyID.AlignContent, 1);
		s_InitialStyleValues[38] = StyleValue.Create(StylePropertyID.AlignSelf, 0);
		s_InitialStyleValues[39] = StyleValue.Create(StylePropertyID.AlignItems, 4);
		s_InitialStyleValues[40] = StyleValue.Create(StylePropertyID.UnityTextAlign, 0);
		s_InitialStyleValues[41] = StyleValue.Create(StylePropertyID.WhiteSpace, 0);
		s_InitialStyleValues[42] = StyleValue.Create(StylePropertyID.Font);
		s_InitialStyleValues[43] = StyleValue.Create(StylePropertyID.FontSize, 0f);
		s_InitialStyleValues[44] = StyleValue.Create(StylePropertyID.FontStyleAndWeight, 0);
		s_InitialStyleValues[45] = StyleValue.Create(StylePropertyID.BackgroundScaleMode, 0);
		s_InitialStyleValues[53] = StyleValue.Create(StylePropertyID.BackgroundImageTintColor, Color.white);
		s_InitialStyleValues[46] = StyleValue.Create(StylePropertyID.Visibility, 0);
		s_InitialStyleValues[47] = StyleValue.Create(StylePropertyID.Overflow, 0);
		s_InitialStyleValues[48] = StyleValue.Create(StylePropertyID.OverflowClipBox, 0);
		s_InitialStyleValues[49] = StyleValue.Create(StylePropertyID.Display, 0);
		s_InitialStyleValues[50] = StyleValue.Create(StylePropertyID.BackgroundImage);
		s_InitialStyleValues[51] = StyleValue.Create(StylePropertyID.Color, Color.black);
		s_InitialStyleValues[52] = StyleValue.Create(StylePropertyID.BackgroundColor, Color.clear);
		s_InitialStyleValues[54] = StyleValue.Create(StylePropertyID.SliceLeft, 0f);
		s_InitialStyleValues[55] = StyleValue.Create(StylePropertyID.SliceTop, 0f);
		s_InitialStyleValues[56] = StyleValue.Create(StylePropertyID.SliceRight, 0f);
		s_InitialStyleValues[57] = StyleValue.Create(StylePropertyID.SliceBottom, 0f);
		s_InitialStyleValues[58] = StyleValue.Create(StylePropertyID.Opacity, 1f);
		s_InitialStyleValues[59] = StyleValue.Create(StylePropertyID.BorderColor, StyleKeyword.Initial);
		s_InitialStyleValues[60] = StyleValue.Create(StylePropertyID.BorderRadius, StyleKeyword.Initial);
		s_InitialStyleValues[61] = StyleValue.Create(StylePropertyID.BorderWidth, StyleKeyword.Initial);
		s_InitialStyleValues[62] = StyleValue.Create(StylePropertyID.Flex, StyleKeyword.Initial);
		s_InitialStyleValues[63] = StyleValue.Create(StylePropertyID.Margin, StyleKeyword.Initial);
		s_InitialStyleValues[64] = StyleValue.Create(StylePropertyID.Padding, StyleKeyword.Initial);
		s_InitialStyleValues[65] = StyleValue.Create(StylePropertyID.Cursor, StyleKeyword.Initial);
	}

	internal static void ClearCaches()
	{
		s_EnumToIntCache.Clear();
		s_RulePropertyIDsCache.Clear();
	}

	internal static bool TryParseEnum<EnumType>(string enumValueName, out int intValue)
	{
		intValue = 0;
		try
		{
			enumValueName = enumValueName.Replace("-", string.Empty);
			object obj = Enum.Parse(typeof(EnumType), enumValueName, ignoreCase: true);
			if (obj != null)
			{
				intValue = (int)obj;
				return true;
			}
		}
		catch (Exception)
		{
			Debug.LogError("Invalid value for " + typeof(EnumType).Name + ": " + enumValueName);
		}
		return false;
	}

	internal static int GetEnumValue<T>(StyleSheet sheet, StyleValueHandle handle)
	{
		Debug.Assert(handle.valueType == StyleValueType.Enum);
		SheetHandleKey key = new SheetHandleKey(sheet, handle.valueIndex);
		int value = 0;
		if (!s_EnumToIntCache.TryGetValue(key, out value) && TryParseEnum<T>(sheet.ReadEnum(handle), out value))
		{
			s_EnumToIntCache.Add(key, value);
			return value;
		}
		return value;
	}

	internal static StylePropertyID[] GetPropertyIDs(StyleSheet sheet, int ruleIndex)
	{
		SheetHandleKey key = new SheetHandleKey(sheet, ruleIndex);
		if (!s_RulePropertyIDsCache.TryGetValue(key, out var value))
		{
			StyleRule styleRule = sheet.rules[ruleIndex];
			value = new StylePropertyID[styleRule.properties.Length];
			for (int i = 0; i < value.Length; i++)
			{
				value[i] = GetPropertyID(sheet, styleRule, i);
			}
			s_RulePropertyIDsCache.Add(key, value);
		}
		return value;
	}

	internal static StyleValue GetInitialValue(StylePropertyID propertyId)
	{
		Debug.Assert(propertyId != StylePropertyID.Unknown && propertyId != StylePropertyID.Custom);
		return s_InitialStyleValues[(int)propertyId];
	}

	private static string MapDeprecatedPropertyName(string name, string styleSheetName, int line)
	{
		s_DeprecatedNames.TryGetValue(name, out var value);
		return value ?? name;
	}

	internal static StylePropertyID GetPropertyIDFromName(string name)
	{
		if (s_NameToIDCache.TryGetValue(name, out var value))
		{
			return value;
		}
		return StylePropertyID.Unknown;
	}

	private static StylePropertyID GetPropertyID(StyleSheet sheet, StyleRule rule, int index)
	{
		StyleProperty styleProperty = rule.properties[index];
		string name = styleProperty.name;
		name = MapDeprecatedPropertyName(name, sheet.name, rule.line);
		if (!s_NameToIDCache.TryGetValue(name, out var value))
		{
			if (styleProperty.isCustomProperty)
			{
				return StylePropertyID.Custom;
			}
			return StylePropertyID.Unknown;
		}
		return value;
	}
}
