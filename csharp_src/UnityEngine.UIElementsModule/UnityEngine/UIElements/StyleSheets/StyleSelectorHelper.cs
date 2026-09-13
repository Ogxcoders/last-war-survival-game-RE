#define UNITY_ASSERTIONS
using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements.StyleSheets;

internal static class StyleSelectorHelper
{
	public static MatchResultInfo MatchesSelector(VisualElement element, StyleSelector selector)
	{
		bool flag = true;
		StyleSelectorPart[] parts = selector.parts;
		int num = parts.Length;
		for (int i = 0; i < num && flag; i++)
		{
			switch (parts[i].type)
			{
			case StyleSelectorType.Class:
				flag = element.ClassListContains(parts[i].value);
				break;
			case StyleSelectorType.ID:
				flag = element.name == parts[i].value;
				break;
			case StyleSelectorType.Type:
				flag = element.typeName == parts[i].value;
				break;
			case StyleSelectorType.Predicate:
				flag = parts[i].tempData is UQuery.IVisualPredicateWrapper visualPredicateWrapper && visualPredicateWrapper.Predicate(element);
				break;
			default:
				flag = false;
				break;
			case StyleSelectorType.Wildcard:
			case StyleSelectorType.PseudoClass:
				break;
			}
		}
		int num2 = 0;
		int num3 = 0;
		bool flag2 = flag;
		if (flag2 && selector.pseudoStateMask != 0)
		{
			flag = ((uint)selector.pseudoStateMask & (uint)element.pseudoStates) == (uint)selector.pseudoStateMask;
			if (flag)
			{
				num3 = selector.pseudoStateMask;
			}
			else
			{
				num2 = selector.pseudoStateMask;
			}
		}
		if (flag2 && selector.negatedPseudoStateMask != 0)
		{
			flag &= ((uint)selector.negatedPseudoStateMask & (uint)(~element.pseudoStates)) == (uint)selector.negatedPseudoStateMask;
			if (flag)
			{
				num2 |= selector.negatedPseudoStateMask;
			}
			else
			{
				num3 |= selector.negatedPseudoStateMask;
			}
		}
		return new MatchResultInfo(flag, (PseudoStates)num2, (PseudoStates)num3);
	}

	public static bool MatchRightToLeft(VisualElement element, StyleComplexSelector complexSelector, Action<VisualElement, MatchResultInfo> processResult)
	{
		VisualElement visualElement = element;
		int num = complexSelector.selectors.Length - 1;
		VisualElement visualElement2 = null;
		int num2 = -1;
		while (num >= 0 && visualElement != null)
		{
			MatchResultInfo arg = MatchesSelector(visualElement, complexSelector.selectors[num]);
			processResult(visualElement, arg);
			if (!arg.success)
			{
				if (num < complexSelector.selectors.Length - 1 && complexSelector.selectors[num + 1].previousRelationship == StyleSelectorRelationship.Descendent)
				{
					visualElement = visualElement.parent;
					continue;
				}
				if (visualElement2 != null)
				{
					visualElement = visualElement2;
					num = num2;
					continue;
				}
				break;
			}
			if (num < complexSelector.selectors.Length - 1 && complexSelector.selectors[num + 1].previousRelationship == StyleSelectorRelationship.Descendent)
			{
				visualElement2 = visualElement.parent;
				num2 = num;
			}
			if (--num < 0)
			{
				return true;
			}
			visualElement = visualElement.parent;
		}
		return false;
	}

	private static void FastLookup(IDictionary<string, StyleComplexSelector> table, List<SelectorMatchRecord> matchedSelectors, StyleMatchingContext context, string input, ref SelectorMatchRecord record)
	{
		if (!table.TryGetValue(input, out var value))
		{
			return;
		}
		while (value != null)
		{
			if (MatchRightToLeft(context.currentElement, value, context.processResult))
			{
				record.complexSelector = value;
				matchedSelectors.Add(record);
			}
			value = value.nextInTable;
		}
	}

	public static void FindMatches(StyleMatchingContext context, List<SelectorMatchRecord> matchedSelectors)
	{
		Debug.Assert(matchedSelectors.Count == 0);
		Debug.Assert(context.currentElement != null, "context.currentElement != null");
		VisualElement currentElement = context.currentElement;
		for (int i = 0; i < context.styleSheetStack.Count; i++)
		{
			StyleSheet styleSheet = context.styleSheetStack[i];
			SelectorMatchRecord record = new SelectorMatchRecord(styleSheet, i);
			FastLookup(styleSheet.orderedTypeSelectors, matchedSelectors, context, currentElement.typeName, ref record);
			FastLookup(styleSheet.orderedTypeSelectors, matchedSelectors, context, "*", ref record);
			if (!string.IsNullOrEmpty(currentElement.name))
			{
				FastLookup(styleSheet.orderedNameSelectors, matchedSelectors, context, currentElement.name, ref record);
			}
			foreach (string @class in currentElement.classList)
			{
				FastLookup(styleSheet.orderedClassSelectors, matchedSelectors, context, @class, ref record);
			}
		}
	}
}
