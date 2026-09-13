using System.Collections.Generic;
using UnityEngine.UIElements.StyleSheets.Syntax;

namespace UnityEngine.UIElements.StyleSheets;

internal class StylePropertyValueMatcher : BaseStyleMatcher
{
	private List<StylePropertyValue> m_Values;

	private StylePropertyValue current => base.hasCurrent ? m_Values[m_CurrentIndex] : default(StylePropertyValue);

	public override int valueCount => m_Values.Count;

	public override bool isVariable => false;

	public MatchResult Match(Expression exp, List<StylePropertyValue> values)
	{
		MatchResult result = new MatchResult
		{
			errorCode = MatchResultErrorCode.None
		};
		if (values == null || values.Count == 0)
		{
			result.errorCode = MatchResultErrorCode.EmptyValue;
			return result;
		}
		Initialize();
		m_Values = values;
		bool flag = false;
		StyleValueHandle handle = m_Values[0].handle;
		if (handle.valueType == StyleValueType.Keyword && handle.valueIndex == 1)
		{
			MoveNext();
			flag = true;
		}
		else
		{
			flag = Match(exp);
		}
		if (!flag)
		{
			StyleSheet sheet = current.sheet;
			result.errorCode = MatchResultErrorCode.Syntax;
			result.errorValue = sheet.ReadAsString(current.handle);
		}
		else if (base.hasCurrent)
		{
			StyleSheet sheet2 = current.sheet;
			result.errorCode = MatchResultErrorCode.ExpectedEndOfValue;
			result.errorValue = sheet2.ReadAsString(current.handle);
		}
		return result;
	}

	protected override bool MatchKeyword(string keyword)
	{
		StylePropertyValue stylePropertyValue = current;
		if (stylePropertyValue.handle.valueType == StyleValueType.Keyword)
		{
			StyleValueKeyword valueIndex = (StyleValueKeyword)stylePropertyValue.handle.valueIndex;
			return valueIndex.ToUssString() == keyword.ToLower();
		}
		if (stylePropertyValue.handle.valueType == StyleValueType.Enum)
		{
			string text = stylePropertyValue.sheet.ReadEnum(stylePropertyValue.handle);
			return text == keyword.ToLower();
		}
		return false;
	}

	protected override bool MatchNumber()
	{
		return current.handle.valueType == StyleValueType.Float;
	}

	protected override bool MatchInteger()
	{
		return current.handle.valueType == StyleValueType.Float;
	}

	protected override bool MatchLength()
	{
		StylePropertyValue stylePropertyValue = current;
		if (stylePropertyValue.handle.valueType == StyleValueType.Dimension)
		{
			return stylePropertyValue.sheet.ReadDimension(stylePropertyValue.handle).unit == Dimension.Unit.Pixel;
		}
		if (stylePropertyValue.handle.valueType == StyleValueType.Float)
		{
			float b = stylePropertyValue.sheet.ReadFloat(stylePropertyValue.handle);
			return Mathf.Approximately(0f, b);
		}
		return false;
	}

	protected override bool MatchPercentage()
	{
		StylePropertyValue stylePropertyValue = current;
		if (stylePropertyValue.handle.valueType == StyleValueType.Dimension)
		{
			return stylePropertyValue.sheet.ReadDimension(stylePropertyValue.handle).unit == Dimension.Unit.Percent;
		}
		if (stylePropertyValue.handle.valueType == StyleValueType.Float)
		{
			float b = stylePropertyValue.sheet.ReadFloat(stylePropertyValue.handle);
			return Mathf.Approximately(0f, b);
		}
		return false;
	}

	protected override bool MatchColor()
	{
		StylePropertyValue stylePropertyValue = current;
		if (stylePropertyValue.handle.valueType == StyleValueType.Color)
		{
			return true;
		}
		if (stylePropertyValue.handle.valueType == StyleValueType.Enum)
		{
			Color color = Color.clear;
			string text = stylePropertyValue.sheet.ReadAsString(stylePropertyValue.handle);
			if (StyleSheetColor.TryGetColor(text.ToLower(), out color))
			{
				return true;
			}
		}
		return false;
	}

	protected override bool MatchResource()
	{
		return current.handle.valueType == StyleValueType.ResourcePath;
	}

	protected override bool MatchUrl()
	{
		return current.handle.valueType == StyleValueType.AssetReference;
	}
}
