using System;
using System.Collections.Generic;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements;

internal class StyleMatchingContext
{
	public List<StyleSheet> styleSheetStack;

	public StyleVariableContext variableContext;

	public VisualElement currentElement;

	public Action<VisualElement, MatchResultInfo> processResult;

	public InheritedStylesData inheritedStyle;

	public StyleMatchingContext(Action<VisualElement, MatchResultInfo> processResult)
	{
		styleSheetStack = new List<StyleSheet>();
		variableContext = StyleVariableContext.none;
		currentElement = null;
		this.processResult = processResult;
	}
}
