namespace UnityEngine.UIElements.StyleSheets;

internal static class ShorthandApplicator
{
	public static void ApplyBorderColor(StylePropertyReader reader, VisualElementStylesData styleData)
	{
		CompileBoxArea(reader, out StyleColor top, out StyleColor right, out StyleColor bottom, out StyleColor left);
		if (top.keyword != StyleKeyword.Undefined)
		{
			top.value = Color.clear;
		}
		if (right.keyword != StyleKeyword.Undefined)
		{
			right.value = Color.clear;
		}
		if (bottom.keyword != StyleKeyword.Undefined)
		{
			bottom.value = Color.clear;
		}
		if (left.keyword != StyleKeyword.Undefined)
		{
			left.value = Color.clear;
		}
		styleData.borderTopColor.Apply(top, StylePropertyApplyMode.CopyIfEqualOrGreaterSpecificity);
		styleData.borderRightColor.Apply(right, StylePropertyApplyMode.CopyIfEqualOrGreaterSpecificity);
		styleData.borderBottomColor.Apply(bottom, StylePropertyApplyMode.CopyIfEqualOrGreaterSpecificity);
		styleData.borderLeftColor.Apply(left, StylePropertyApplyMode.CopyIfEqualOrGreaterSpecificity);
	}

	public static void ApplyBorderRadius(StylePropertyReader reader, VisualElementStylesData styleData)
	{
		CompileBoxArea(reader, out StyleLength top, out StyleLength right, out StyleLength bottom, out StyleLength left);
		if (top.keyword != StyleKeyword.Undefined)
		{
			top.value = 0f;
		}
		if (right.keyword != StyleKeyword.Undefined)
		{
			right.value = 0f;
		}
		if (left.keyword != StyleKeyword.Undefined)
		{
			left.value = 0f;
		}
		if (bottom.keyword != StyleKeyword.Undefined)
		{
			bottom.value = 0f;
		}
		styleData.borderTopLeftRadius = top;
		styleData.borderTopRightRadius = right;
		styleData.borderBottomLeftRadius = left;
		styleData.borderBottomRightRadius = bottom;
	}

	public static void ApplyBorderWidth(StylePropertyReader reader, VisualElementStylesData styleData)
	{
		CompileBoxArea(reader, out StyleLength top, out StyleLength right, out StyleLength bottom, out StyleLength left);
		if (top.keyword != StyleKeyword.Undefined)
		{
			top.value = 0f;
		}
		if (right.keyword != StyleKeyword.Undefined)
		{
			right.value = 0f;
		}
		if (bottom.keyword != StyleKeyword.Undefined)
		{
			bottom.value = 0f;
		}
		if (left.keyword != StyleKeyword.Undefined)
		{
			left.value = 0f;
		}
		styleData.borderTopWidth = top.ToStyleFloat();
		styleData.borderRightWidth = right.ToStyleFloat();
		styleData.borderBottomWidth = bottom.ToStyleFloat();
		styleData.borderLeftWidth = left.ToStyleFloat();
	}

	public static void ApplyFlex(StylePropertyReader reader, VisualElementStylesData styleData)
	{
		if (CompileFlexShorthand(reader, out var grow, out var shrink, out var basis))
		{
			styleData.flexGrow = grow;
			styleData.flexShrink = shrink;
			styleData.flexBasis = basis;
		}
	}

	public static void ApplyMargin(StylePropertyReader reader, VisualElementStylesData styleData)
	{
		CompileBoxArea(reader, out StyleLength top, out StyleLength right, out StyleLength bottom, out StyleLength left);
		styleData.marginTop = top;
		styleData.marginRight = right;
		styleData.marginBottom = bottom;
		styleData.marginLeft = left;
	}

	public static void ApplyPadding(StylePropertyReader reader, VisualElementStylesData styleData)
	{
		CompileBoxArea(reader, out StyleLength top, out StyleLength right, out StyleLength bottom, out StyleLength left);
		styleData.paddingTop = top;
		styleData.paddingRight = right;
		styleData.paddingBottom = bottom;
		styleData.paddingLeft = left;
	}

	private static bool CompileFlexShorthand(StylePropertyReader reader, out StyleFloat grow, out StyleFloat shrink, out StyleLength basis)
	{
		grow = 0f;
		shrink = 1f;
		basis = StyleKeyword.Auto;
		bool flag = false;
		int valueCount = reader.valueCount;
		if (valueCount == 1 && reader.IsValueType(0, StyleValueType.Keyword))
		{
			if (reader.IsKeyword(0, StyleValueKeyword.None))
			{
				flag = true;
				grow = 0f;
				shrink = 0f;
				basis = StyleKeyword.Auto;
			}
			else if (reader.IsKeyword(0, StyleValueKeyword.Auto))
			{
				flag = true;
				grow = 1f;
				shrink = 1f;
				basis = StyleKeyword.Auto;
			}
		}
		else if (valueCount <= 3)
		{
			flag = true;
			grow = 0f;
			shrink = 1f;
			basis = Length.Percent(0f);
			bool flag2 = false;
			bool flag3 = false;
			for (int i = 0; i < valueCount && flag; i++)
			{
				StyleValueType valueType = reader.GetValueType(i);
				if (valueType == StyleValueType.Dimension || valueType == StyleValueType.Keyword)
				{
					if (flag3)
					{
						flag = false;
						break;
					}
					flag3 = true;
					switch (valueType)
					{
					case StyleValueType.Keyword:
						if (reader.IsKeyword(i, StyleValueKeyword.Auto))
						{
							basis = StyleKeyword.Auto;
						}
						break;
					case StyleValueType.Dimension:
						basis = reader.ReadStyleLength(i);
						break;
					}
					if (flag2 && i != valueCount - 1)
					{
						flag = false;
					}
				}
				else if (valueType == StyleValueType.Float)
				{
					StyleFloat styleFloat = reader.ReadStyleFloat(i);
					if (!flag2)
					{
						flag2 = true;
						grow = styleFloat;
					}
					else
					{
						shrink = styleFloat;
					}
				}
				else
				{
					flag = false;
				}
			}
		}
		grow.specificity = reader.specificity;
		shrink.specificity = reader.specificity;
		basis.specificity = reader.specificity;
		return flag;
	}

	private static void CompileBoxArea(StylePropertyReader reader, out StyleLength top, out StyleLength right, out StyleLength bottom, out StyleLength left)
	{
		top = 0f;
		right = 0f;
		bottom = 0f;
		left = 0f;
		switch (reader.valueCount)
		{
		case 0:
			break;
		case 1:
			top = (right = (bottom = (left = reader.ReadStyleLength(0))));
			break;
		case 2:
			top = (bottom = reader.ReadStyleLength(0));
			left = (right = reader.ReadStyleLength(1));
			break;
		case 3:
			top = reader.ReadStyleLength(0);
			left = (right = reader.ReadStyleLength(1));
			bottom = reader.ReadStyleLength(2);
			break;
		default:
			top = reader.ReadStyleLength(0);
			right = reader.ReadStyleLength(1);
			bottom = reader.ReadStyleLength(2);
			left = reader.ReadStyleLength(3);
			break;
		}
	}

	private static void CompileBoxArea(StylePropertyReader reader, out StyleColor top, out StyleColor right, out StyleColor bottom, out StyleColor left)
	{
		top = Color.clear;
		right = Color.clear;
		bottom = Color.clear;
		left = Color.clear;
		switch (reader.valueCount)
		{
		case 0:
			break;
		case 1:
			top = (right = (bottom = (left = reader.ReadStyleColor(0))));
			break;
		case 2:
			top = (bottom = reader.ReadStyleColor(0));
			left = (right = reader.ReadStyleColor(1));
			break;
		case 3:
			top = reader.ReadStyleColor(0);
			left = (right = reader.ReadStyleColor(1));
			bottom = reader.ReadStyleColor(2);
			break;
		default:
			top = reader.ReadStyleColor(0);
			right = reader.ReadStyleColor(1);
			bottom = reader.ReadStyleColor(2);
			left = reader.ReadStyleColor(3);
			break;
		}
	}
}
