#define UNITY_ASSERTIONS
using System;
using UnityEngine.UIElements.StyleSheets;
using UnityEngine.Yoga;

namespace UnityEngine.UIElements;

internal static class StyleValueExtensions
{
	internal const int UndefinedSpecificity = 0;

	internal const int UnitySpecificity = -1;

	internal const int InlineSpecificity = int.MaxValue;

	internal static StyleFloat ToStyleFloat(this StyleLength styleLength)
	{
		StyleFloat result = new StyleFloat(styleLength.value.value, styleLength.keyword);
		result.specificity = styleLength.specificity;
		return result;
	}

	internal static StyleEnum<T> ToStyleEnum<T>(this StyleInt styleInt, T value) where T : struct, IConvertible
	{
		StyleEnum<T> result = new StyleEnum<T>(value, styleInt.keyword);
		result.specificity = styleInt.specificity;
		return result;
	}

	internal static StyleLength ToStyleLength(this StyleValue styleValue)
	{
		return new StyleLength(new Length(styleValue.number), styleValue.keyword);
	}

	internal static StyleFloat ToStyleFloat(this StyleValue styleValue)
	{
		return new StyleFloat(styleValue.number, styleValue.keyword);
	}

	internal static string DebugString<T>(this IStyleValue<T> styleValue)
	{
		return (styleValue.keyword != StyleKeyword.Undefined) ? $"{styleValue.keyword}" : $"{styleValue.value}";
	}

	internal static U GetSpecifiedValueOrDefault<T, U>(this T styleValue, U defaultValue) where T : IStyleValue<U>
	{
		if (styleValue.specificity != 0)
		{
			return styleValue.value;
		}
		return defaultValue;
	}

	internal static float GetSpecifiedValueOrDefault(this StyleLength styleValue, float defaultValue)
	{
		if (styleValue.specificity != 0)
		{
			return styleValue.value.value;
		}
		return defaultValue;
	}

	internal static YogaValue ToYogaValue(this StyleLength styleValue)
	{
		if (styleValue.keyword == StyleKeyword.Auto)
		{
			return YogaValue.Auto();
		}
		if (styleValue.keyword == StyleKeyword.None)
		{
			return float.NaN;
		}
		if (styleValue.specificity != 0)
		{
			Length value = styleValue.value;
			switch (value.unit)
			{
			case LengthUnit.Pixel:
				return YogaValue.Point(value.value);
			case LengthUnit.Percent:
				return YogaValue.Percent(value.value);
			default:
				Debug.LogAssertion($"Unexpected unit '{value.unit}'");
				return float.NaN;
			}
		}
		return float.NaN;
	}

	internal static bool CanApply(int specificity, int otherSpecificity, StylePropertyApplyMode mode)
	{
		switch (mode)
		{
		case StylePropertyApplyMode.Copy:
			return true;
		case StylePropertyApplyMode.CopyIfEqualOrGreaterSpecificity:
			if (specificity == 0 && otherSpecificity == -1)
			{
				return true;
			}
			return otherSpecificity >= specificity;
		case StylePropertyApplyMode.CopyIfNotInline:
			return specificity < int.MaxValue;
		default:
			Debug.Assert(condition: false, "Invalid mode " + mode);
			return false;
		}
	}

	internal static StyleKeyword ToStyleKeyword(this StyleValueKeyword styleValueKeyword)
	{
		return styleValueKeyword switch
		{
			StyleValueKeyword.Auto => StyleKeyword.Auto, 
			StyleValueKeyword.None => StyleKeyword.None, 
			StyleValueKeyword.Initial => StyleKeyword.Initial, 
			_ => StyleKeyword.Undefined, 
		};
	}
}
