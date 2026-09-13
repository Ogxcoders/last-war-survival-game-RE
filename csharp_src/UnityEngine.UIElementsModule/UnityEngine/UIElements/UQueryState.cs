using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements;

public struct UQueryState<T> : IEquatable<UQueryState<T>> where T : VisualElement
{
	private class ListQueryMatcher : UQuery.UQueryMatcher
	{
		public List<T> matches { get; set; }

		protected override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
		{
			matches.Add(element as T);
			return false;
		}

		public void Reset()
		{
			matches = null;
		}
	}

	private class ActionQueryMatcher : UQuery.UQueryMatcher
	{
		internal Action<T> callBack { get; set; }

		protected override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
		{
			if (element is T obj)
			{
				callBack(obj);
			}
			return false;
		}
	}

	private class DelegateQueryMatcher<TReturnType> : UQuery.UQueryMatcher
	{
		public static DelegateQueryMatcher<TReturnType> s_Instance = new DelegateQueryMatcher<TReturnType>();

		public Func<T, TReturnType> callBack { get; set; }

		public List<TReturnType> result { get; set; }

		protected override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
		{
			if (element is T arg)
			{
				result.Add(callBack(arg));
			}
			return false;
		}
	}

	private static UQuery.FirstQueryMatcher s_First = new UQuery.FirstQueryMatcher();

	private static UQuery.LastQueryMatcher s_Last = new UQuery.LastQueryMatcher();

	private static UQuery.IndexQueryMatcher s_Index = new UQuery.IndexQueryMatcher();

	private static ActionQueryMatcher s_Action = new ActionQueryMatcher();

	private readonly VisualElement m_Element;

	internal readonly List<RuleMatcher> m_Matchers;

	private static readonly ListQueryMatcher s_List = new ListQueryMatcher();

	internal UQueryState(VisualElement element, List<RuleMatcher> matchers)
	{
		m_Element = element;
		m_Matchers = matchers;
	}

	public UQueryState<T> RebuildOn(VisualElement element)
	{
		return new UQueryState<T>(element, m_Matchers);
	}

	public T First()
	{
		s_First.Run(m_Element, m_Matchers);
		T result = s_First.match as T;
		s_First.match = null;
		return result;
	}

	public T Last()
	{
		s_Last.Run(m_Element, m_Matchers);
		T result = s_Last.match as T;
		s_Last.match = null;
		return result;
	}

	public void ToList(List<T> results)
	{
		s_List.matches = results;
		s_List.Run(m_Element, m_Matchers);
		s_List.Reset();
	}

	public List<T> ToList()
	{
		List<T> list = new List<T>();
		ToList(list);
		return list;
	}

	public T AtIndex(int index)
	{
		s_Index.matchIndex = index;
		s_Index.Run(m_Element, m_Matchers);
		T result = s_Index.match as T;
		s_Index.match = null;
		return result;
	}

	public void ForEach(Action<T> funcCall)
	{
		ActionQueryMatcher actionQueryMatcher = s_Action;
		if (actionQueryMatcher.callBack != null)
		{
			actionQueryMatcher = new ActionQueryMatcher();
		}
		try
		{
			actionQueryMatcher.callBack = funcCall;
			actionQueryMatcher.Run(m_Element, m_Matchers);
		}
		finally
		{
			actionQueryMatcher.callBack = null;
		}
	}

	public void ForEach<T2>(List<T2> result, Func<T, T2> funcCall)
	{
		DelegateQueryMatcher<T2> delegateQueryMatcher = DelegateQueryMatcher<T2>.s_Instance;
		if (delegateQueryMatcher.callBack != null)
		{
			delegateQueryMatcher = new DelegateQueryMatcher<T2>();
		}
		try
		{
			delegateQueryMatcher.callBack = funcCall;
			delegateQueryMatcher.result = result;
			delegateQueryMatcher.Run(m_Element, m_Matchers);
		}
		finally
		{
			delegateQueryMatcher.callBack = null;
			delegateQueryMatcher.result = null;
		}
	}

	public List<T2> ForEach<T2>(Func<T, T2> funcCall)
	{
		List<T2> result = new List<T2>();
		ForEach(result, funcCall);
		return result;
	}

	public bool Equals(UQueryState<T> other)
	{
		return m_Element == other.m_Element && EqualityComparer<List<RuleMatcher>>.Default.Equals(m_Matchers, other.m_Matchers);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is UQueryState<T>))
		{
			return false;
		}
		return Equals((UQueryState<T>)obj);
	}

	public override int GetHashCode()
	{
		int num = 488160421;
		num = num * -1521134295 + EqualityComparer<VisualElement>.Default.GetHashCode(m_Element);
		return num * -1521134295 + EqualityComparer<List<RuleMatcher>>.Default.GetHashCode(m_Matchers);
	}

	public static bool operator ==(UQueryState<T> state1, UQueryState<T> state2)
	{
		return state1.Equals(state2);
	}

	public static bool operator !=(UQueryState<T> state1, UQueryState<T> state2)
	{
		return !(state1 == state2);
	}
}
