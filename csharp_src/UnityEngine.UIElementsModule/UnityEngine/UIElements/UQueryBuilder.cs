using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements;

public struct UQueryBuilder<T> : IEquatable<UQueryBuilder<T>> where T : VisualElement
{
	private List<StyleSelector> m_StyleSelectors;

	private List<StyleSelectorPart> m_Parts;

	private VisualElement m_Element;

	private List<RuleMatcher> m_Matchers;

	private StyleSelectorRelationship m_Relationship;

	private int pseudoStatesMask;

	private int negatedPseudoStatesMask;

	private List<StyleSelector> styleSelectors => m_StyleSelectors ?? (m_StyleSelectors = new List<StyleSelector>());

	private List<StyleSelectorPart> parts => m_Parts ?? (m_Parts = new List<StyleSelectorPart>());

	public UQueryBuilder(VisualElement visualElement)
	{
		this = default(UQueryBuilder<T>);
		m_Element = visualElement;
		m_Parts = null;
		m_StyleSelectors = null;
		m_Relationship = StyleSelectorRelationship.None;
		m_Matchers = new List<RuleMatcher>();
		pseudoStatesMask = (negatedPseudoStatesMask = 0);
	}

	public UQueryBuilder<T> Class(string classname)
	{
		AddClass(classname);
		return this;
	}

	public UQueryBuilder<T> Name(string id)
	{
		AddName(id);
		return this;
	}

	public UQueryBuilder<T2> Descendents<T2>(string name = null, params string[] classNames) where T2 : VisualElement
	{
		FinishCurrentSelector();
		AddType<T2>();
		AddName(name);
		AddClasses(classNames);
		return AddRelationship<T2>(StyleSelectorRelationship.Descendent);
	}

	public UQueryBuilder<T2> Descendents<T2>(string name = null, string classname = null) where T2 : VisualElement
	{
		FinishCurrentSelector();
		AddType<T2>();
		AddName(name);
		AddClass(classname);
		return AddRelationship<T2>(StyleSelectorRelationship.Descendent);
	}

	public UQueryBuilder<T2> Children<T2>(string name = null, params string[] classes) where T2 : VisualElement
	{
		FinishCurrentSelector();
		AddType<T2>();
		AddName(name);
		AddClasses(classes);
		return AddRelationship<T2>(StyleSelectorRelationship.Child);
	}

	public UQueryBuilder<T2> Children<T2>(string name = null, string className = null) where T2 : VisualElement
	{
		FinishCurrentSelector();
		AddType<T2>();
		AddName(name);
		AddClass(className);
		return AddRelationship<T2>(StyleSelectorRelationship.Child);
	}

	public UQueryBuilder<T2> OfType<T2>(string name = null, params string[] classes) where T2 : VisualElement
	{
		AddType<T2>();
		AddName(name);
		AddClasses(classes);
		return AddRelationship<T2>(StyleSelectorRelationship.None);
	}

	public UQueryBuilder<T2> OfType<T2>(string name = null, string className = null) where T2 : VisualElement
	{
		AddType<T2>();
		AddName(name);
		AddClass(className);
		return AddRelationship<T2>(StyleSelectorRelationship.None);
	}

	internal UQueryBuilder<T> SingleBaseType()
	{
		parts.Add(StyleSelectorPart.CreatePredicate(UQuery.IsOfType<T>.s_Instance));
		return this;
	}

	public UQueryBuilder<T> Where(Func<T, bool> selectorPredicate)
	{
		parts.Add(StyleSelectorPart.CreatePredicate(new UQuery.PredicateWrapper<T>(selectorPredicate)));
		return this;
	}

	private void AddClass(string c)
	{
		if (c != null)
		{
			parts.Add(StyleSelectorPart.CreateClass(c));
		}
	}

	private void AddClasses(params string[] classes)
	{
		if (classes != null)
		{
			for (int i = 0; i < classes.Length; i++)
			{
				AddClass(classes[i]);
			}
		}
	}

	private void AddName(string id)
	{
		if (id != null)
		{
			parts.Add(StyleSelectorPart.CreateId(id));
		}
	}

	private void AddType<T2>() where T2 : VisualElement
	{
		if ((object)typeof(T2) != typeof(VisualElement))
		{
			parts.Add(StyleSelectorPart.CreatePredicate(UQuery.IsOfType<T2>.s_Instance));
		}
	}

	private UQueryBuilder<T> AddPseudoState(PseudoStates s)
	{
		pseudoStatesMask |= (int)s;
		return this;
	}

	private UQueryBuilder<T> AddNegativePseudoState(PseudoStates s)
	{
		negatedPseudoStatesMask |= (int)s;
		return this;
	}

	public UQueryBuilder<T> Active()
	{
		return AddPseudoState(PseudoStates.Active);
	}

	public UQueryBuilder<T> NotActive()
	{
		return AddNegativePseudoState(PseudoStates.Active);
	}

	public UQueryBuilder<T> Visible()
	{
		return Where((T e) => e.visible);
	}

	public UQueryBuilder<T> NotVisible()
	{
		return Where((T e) => !e.visible);
	}

	public UQueryBuilder<T> Hovered()
	{
		return AddPseudoState(PseudoStates.Hover);
	}

	public UQueryBuilder<T> NotHovered()
	{
		return AddNegativePseudoState(PseudoStates.Hover);
	}

	public UQueryBuilder<T> Checked()
	{
		return AddPseudoState(PseudoStates.Checked);
	}

	public UQueryBuilder<T> NotChecked()
	{
		return AddNegativePseudoState(PseudoStates.Checked);
	}

	[Obsolete("Use Checked() instead")]
	public UQueryBuilder<T> Selected()
	{
		return AddPseudoState(PseudoStates.Checked);
	}

	[Obsolete("Use NotChecked() instead")]
	public UQueryBuilder<T> NotSelected()
	{
		return AddNegativePseudoState(PseudoStates.Checked);
	}

	public UQueryBuilder<T> Enabled()
	{
		return AddNegativePseudoState(PseudoStates.Disabled);
	}

	public UQueryBuilder<T> NotEnabled()
	{
		return AddPseudoState(PseudoStates.Disabled);
	}

	public UQueryBuilder<T> Focused()
	{
		return AddPseudoState(PseudoStates.Focus);
	}

	public UQueryBuilder<T> NotFocused()
	{
		return AddNegativePseudoState(PseudoStates.Focus);
	}

	private UQueryBuilder<T2> AddRelationship<T2>(StyleSelectorRelationship relationship) where T2 : VisualElement
	{
		UQueryBuilder<T2> result = new UQueryBuilder<T2>(m_Element);
		result.m_Matchers = m_Matchers;
		result.m_Parts = m_Parts;
		result.m_StyleSelectors = m_StyleSelectors;
		result.m_Relationship = ((relationship == StyleSelectorRelationship.None) ? m_Relationship : relationship);
		result.pseudoStatesMask = pseudoStatesMask;
		result.negatedPseudoStatesMask = negatedPseudoStatesMask;
		return result;
	}

	private void AddPseudoStatesRuleIfNecessasy()
	{
		if (pseudoStatesMask != 0 || negatedPseudoStatesMask != 0)
		{
			parts.Add(new StyleSelectorPart
			{
				type = StyleSelectorType.PseudoClass
			});
		}
	}

	private void FinishSelector()
	{
		FinishCurrentSelector();
		if (styleSelectors.Count > 0)
		{
			StyleComplexSelector styleComplexSelector = new StyleComplexSelector();
			styleComplexSelector.selectors = styleSelectors.ToArray();
			styleSelectors.Clear();
			m_Matchers.Add(new RuleMatcher
			{
				complexSelector = styleComplexSelector
			});
		}
	}

	private bool CurrentSelectorEmpty()
	{
		return parts.Count == 0 && m_Relationship == StyleSelectorRelationship.None && pseudoStatesMask == 0 && negatedPseudoStatesMask == 0;
	}

	private void FinishCurrentSelector()
	{
		if (!CurrentSelectorEmpty())
		{
			StyleSelector styleSelector = new StyleSelector();
			styleSelector.previousRelationship = m_Relationship;
			AddPseudoStatesRuleIfNecessasy();
			styleSelector.parts = m_Parts.ToArray();
			styleSelector.pseudoStateMask = pseudoStatesMask;
			styleSelector.negatedPseudoStateMask = negatedPseudoStatesMask;
			styleSelectors.Add(styleSelector);
			m_Parts.Clear();
			pseudoStatesMask = (negatedPseudoStatesMask = 0);
		}
	}

	public UQueryState<T> Build()
	{
		FinishSelector();
		return new UQueryState<T>(m_Element, m_Matchers);
	}

	public static implicit operator T(UQueryBuilder<T> s)
	{
		return s.First();
	}

	public static bool operator ==(UQueryBuilder<T> builder1, UQueryBuilder<T> builder2)
	{
		return builder1.Equals(builder2);
	}

	public static bool operator !=(UQueryBuilder<T> builder1, UQueryBuilder<T> builder2)
	{
		return !(builder1 == builder2);
	}

	public T First()
	{
		return Build().First();
	}

	public T Last()
	{
		return Build().Last();
	}

	public List<T> ToList()
	{
		return Build().ToList();
	}

	public void ToList(List<T> results)
	{
		Build().ToList(results);
	}

	public T AtIndex(int index)
	{
		return Build().AtIndex(index);
	}

	public void ForEach<T2>(List<T2> result, Func<T, T2> funcCall)
	{
		Build().ForEach(result, funcCall);
	}

	public List<T2> ForEach<T2>(Func<T, T2> funcCall)
	{
		return Build().ForEach(funcCall);
	}

	public void ForEach(Action<T> funcCall)
	{
		Build().ForEach(funcCall);
	}

	public bool Equals(UQueryBuilder<T> other)
	{
		return EqualityComparer<List<StyleSelector>>.Default.Equals(m_StyleSelectors, other.m_StyleSelectors) && EqualityComparer<List<StyleSelector>>.Default.Equals(styleSelectors, other.styleSelectors) && EqualityComparer<List<StyleSelectorPart>>.Default.Equals(m_Parts, other.m_Parts) && EqualityComparer<List<StyleSelectorPart>>.Default.Equals(parts, other.parts) && m_Element == other.m_Element && EqualityComparer<List<RuleMatcher>>.Default.Equals(m_Matchers, other.m_Matchers) && m_Relationship == other.m_Relationship && pseudoStatesMask == other.pseudoStatesMask && negatedPseudoStatesMask == other.negatedPseudoStatesMask;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is UQueryBuilder<T>))
		{
			return false;
		}
		return Equals((UQueryBuilder<T>)obj);
	}

	public override int GetHashCode()
	{
		int num = -949812380;
		num = num * -1521134295 + EqualityComparer<List<StyleSelector>>.Default.GetHashCode(m_StyleSelectors);
		num = num * -1521134295 + EqualityComparer<List<StyleSelector>>.Default.GetHashCode(styleSelectors);
		num = num * -1521134295 + EqualityComparer<List<StyleSelectorPart>>.Default.GetHashCode(m_Parts);
		num = num * -1521134295 + EqualityComparer<List<StyleSelectorPart>>.Default.GetHashCode(parts);
		num = num * -1521134295 + EqualityComparer<VisualElement>.Default.GetHashCode(m_Element);
		num = num * -1521134295 + EqualityComparer<List<RuleMatcher>>.Default.GetHashCode(m_Matchers);
		num = num * -1521134295 + m_Relationship.GetHashCode();
		num = num * -1521134295 + pseudoStatesMask.GetHashCode();
		return num * -1521134295 + negatedPseudoStatesMask.GetHashCode();
	}
}
