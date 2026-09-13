using System.Collections.Generic;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements;

internal class VisualTreeStyleUpdaterTraversal : HierarchyTraversal
{
	private static readonly InheritedStylesData s_DefaultInheritedStyles = new InheritedStylesData();

	private InheritedStylesData m_ResolveInheritData = new InheritedStylesData();

	private StyleVariableContext m_ProcessVarContext = new StyleVariableContext();

	private HashSet<VisualElement> m_UpdateList = new HashSet<VisualElement>();

	private HashSet<VisualElement> m_ParentList = new HashSet<VisualElement>();

	private List<SelectorMatchRecord> m_TempMatchResults = new List<SelectorMatchRecord>();

	private StyleMatchingContext m_StyleMatchingContext = new StyleMatchingContext(OnProcessMatchResult);

	private StylePropertyReader m_StylePropertyReader = new StylePropertyReader();

	private float currentPixelsPerPoint { get; set; } = 1f;

	public void PrepareTraversal(float pixelsPerPoint)
	{
		currentPixelsPerPoint = pixelsPerPoint;
		m_StyleMatchingContext.inheritedStyle = s_DefaultInheritedStyles;
	}

	public void AddChangedElement(VisualElement ve)
	{
		m_UpdateList.Add(ve);
		PropagateToChildren(ve);
		PropagateToParents(ve);
	}

	public void Clear()
	{
		m_UpdateList.Clear();
		m_ParentList.Clear();
		m_TempMatchResults.Clear();
	}

	private void PropagateToChildren(VisualElement ve)
	{
		int childCount = ve.hierarchy.childCount;
		for (int i = 0; i < childCount; i++)
		{
			VisualElement visualElement = ve.hierarchy[i];
			if (m_UpdateList.Add(visualElement))
			{
				PropagateToChildren(visualElement);
			}
		}
	}

	private void PropagateToParents(VisualElement ve)
	{
		VisualElement parent = ve.hierarchy.parent;
		while (parent != null && m_ParentList.Add(parent))
		{
			parent = parent.hierarchy.parent;
		}
	}

	private static void OnProcessMatchResult(VisualElement current, MatchResultInfo info)
	{
		current.triggerPseudoMask |= info.triggerPseudoMask;
		current.dependencyPseudoMask |= info.dependencyPseudoMask;
	}

	public override void TraverseRecursive(VisualElement element, int depth)
	{
		if (ShouldSkipElement(element))
		{
			return;
		}
		bool flag = m_UpdateList.Contains(element);
		if (flag)
		{
			element.triggerPseudoMask = (PseudoStates)0;
			element.dependencyPseudoMask = (PseudoStates)0;
		}
		int count = m_StyleMatchingContext.styleSheetStack.Count;
		if (element.styleSheetList != null)
		{
			for (int i = 0; i < element.styleSheetList.Count; i++)
			{
				StyleSheet item = element.styleSheetList[i];
				m_StyleMatchingContext.styleSheetStack.Add(item);
			}
		}
		int customPropertiesCount = element.specifiedStyle.customPropertiesCount;
		InheritedStylesData inheritedStyle = m_StyleMatchingContext.inheritedStyle;
		if (flag)
		{
			m_StyleMatchingContext.currentElement = element;
			element.specifiedStyle.dpiScaling = element.scaledPixelsPerPoint;
			StyleSelectorHelper.FindMatches(m_StyleMatchingContext, m_TempMatchResults);
			ProcessMatchedRules(element, m_TempMatchResults);
			ResolveInheritance(element);
			m_StyleMatchingContext.currentElement = null;
			m_TempMatchResults.Clear();
		}
		else
		{
			m_StyleMatchingContext.inheritedStyle = element.propagatedStyle;
			m_StyleMatchingContext.variableContext = element.variableContext;
		}
		if (flag && (customPropertiesCount > 0 || element.specifiedStyle.customPropertiesCount > 0))
		{
			using CustomStyleResolvedEvent customStyleResolvedEvent = EventBase<CustomStyleResolvedEvent>.GetPooled();
			customStyleResolvedEvent.target = element;
			element.SendEvent(customStyleResolvedEvent);
		}
		Recurse(element, depth);
		m_StyleMatchingContext.inheritedStyle = inheritedStyle;
		if (m_StyleMatchingContext.styleSheetStack.Count > count)
		{
			m_StyleMatchingContext.styleSheetStack.RemoveRange(count, m_StyleMatchingContext.styleSheetStack.Count - count);
		}
	}

	private bool ShouldSkipElement(VisualElement element)
	{
		return !m_ParentList.Contains(element) && !m_UpdateList.Contains(element);
	}

	private void ProcessMatchedRules(VisualElement element, List<SelectorMatchRecord> matchingSelectors)
	{
		matchingSelectors.Sort((SelectorMatchRecord a, SelectorMatchRecord b) => SelectorMatchRecord.Compare(a, b));
		long num = element.fullTypeName.GetHashCode();
		num = (num * 397) ^ currentPixelsPerPoint.GetHashCode();
		int variableHash = m_StyleMatchingContext.variableContext.GetVariableHash();
		int num2 = 0;
		foreach (SelectorMatchRecord matchingSelector in matchingSelectors)
		{
			num2 += matchingSelector.complexSelector.rule.customPropertiesCount;
		}
		if (num2 > 0)
		{
			m_ProcessVarContext.AddInitialRange(m_StyleMatchingContext.variableContext);
		}
		foreach (SelectorMatchRecord matchingSelector2 in matchingSelectors)
		{
			StyleRule rule = matchingSelector2.complexSelector.rule;
			int specificity = matchingSelector2.complexSelector.specificity;
			num = (num * 397) ^ rule.GetHashCode();
			num = (num * 397) ^ specificity;
			if (rule.customPropertiesCount > 0)
			{
				ProcessMatchedVariables(matchingSelector2.sheet, rule);
			}
		}
		int num3 = variableHash;
		if (num2 > 0)
		{
			num3 = m_ProcessVarContext.GetVariableHash();
		}
		num = (num * 397) ^ num3;
		if (variableHash != num3)
		{
			if (!StyleCache.TryGetValue(num3, out StyleVariableContext data))
			{
				data = new StyleVariableContext(m_ProcessVarContext);
				StyleCache.SetValue(num3, data);
			}
			m_StyleMatchingContext.variableContext = data;
		}
		element.variableContext = m_StyleMatchingContext.variableContext;
		m_ProcessVarContext.Clear();
		if (StyleCache.TryGetValue(num, out var data2))
		{
			element.SetSharedStyles(data2);
			return;
		}
		data2 = new VisualElementStylesData(isShared: true);
		float dpiScaling = element.specifiedStyle.dpiScaling;
		foreach (SelectorMatchRecord matchingSelector3 in matchingSelectors)
		{
			m_StylePropertyReader.SetContext(matchingSelector3.sheet, matchingSelector3.complexSelector, m_StyleMatchingContext.variableContext, dpiScaling);
			data2.ApplyProperties(m_StylePropertyReader, m_StyleMatchingContext.inheritedStyle);
		}
		data2.ApplyLayoutValues();
		StyleCache.SetValue(num, data2);
		element.SetSharedStyles(data2);
	}

	private void ProcessMatchedVariables(StyleSheet sheet, StyleRule rule)
	{
		StyleProperty[] properties = rule.properties;
		foreach (StyleProperty styleProperty in properties)
		{
			if (styleProperty.isCustomProperty)
			{
				StyleVariable sv = new StyleVariable(styleProperty.name, sheet, styleProperty.values);
				m_ProcessVarContext.Add(sv);
			}
		}
	}

	private void ResolveInheritance(VisualElement element)
	{
		VisualElementStylesData specifiedStyle = element.specifiedStyle;
		InheritedStylesData other = (element.inheritedStyle = m_StyleMatchingContext.inheritedStyle);
		m_ResolveInheritData.CopyFrom(other);
		if (specifiedStyle.color.specificity != 0)
		{
			m_ResolveInheritData.color = specifiedStyle.color;
		}
		if (specifiedStyle.unityFont.specificity != 0)
		{
			m_ResolveInheritData.font = specifiedStyle.unityFont;
		}
		if (specifiedStyle.fontSize.specificity != 0)
		{
			m_ResolveInheritData.fontSize = new StyleLength(ComputedStyle.CalculatePixelFontSize(element));
			m_ResolveInheritData.fontSize.specificity = specifiedStyle.fontSize.specificity;
		}
		if (specifiedStyle.unityFontStyleAndWeight.specificity != 0)
		{
			m_ResolveInheritData.unityFontStyle = specifiedStyle.unityFontStyleAndWeight;
		}
		if (specifiedStyle.unityTextAlign.specificity != 0)
		{
			m_ResolveInheritData.unityTextAlign = specifiedStyle.unityTextAlign;
		}
		if (specifiedStyle.visibility.specificity != 0)
		{
			m_ResolveInheritData.visibility = specifiedStyle.visibility;
		}
		if (specifiedStyle.whiteSpace.specificity != 0)
		{
			m_ResolveInheritData.whiteSpace = specifiedStyle.whiteSpace;
		}
		if (!m_ResolveInheritData.Equals(other))
		{
			InheritedStylesData data = null;
			int hashCode = m_ResolveInheritData.GetHashCode();
			if (!StyleCache.TryGetValue(hashCode, out data))
			{
				data = new InheritedStylesData(m_ResolveInheritData);
				StyleCache.SetValue(hashCode, data);
			}
			m_StyleMatchingContext.inheritedStyle = data;
		}
		element.propagatedStyle = m_StyleMatchingContext.inheritedStyle;
	}
}
