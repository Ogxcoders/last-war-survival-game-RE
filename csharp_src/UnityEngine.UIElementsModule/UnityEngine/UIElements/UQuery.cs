using System;
using System.Collections.Generic;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements;

public static class UQuery
{
	internal interface IVisualPredicateWrapper
	{
		bool Predicate(object e);
	}

	internal class IsOfType<T> : IVisualPredicateWrapper where T : VisualElement
	{
		public static IsOfType<T> s_Instance = new IsOfType<T>();

		public bool Predicate(object e)
		{
			return e is T;
		}
	}

	internal class PredicateWrapper<T> : IVisualPredicateWrapper where T : VisualElement
	{
		private Func<T, bool> predicate;

		public PredicateWrapper(Func<T, bool> p)
		{
			predicate = p;
		}

		public bool Predicate(object e)
		{
			if (e is T arg)
			{
				return predicate(arg);
			}
			return false;
		}
	}

	internal abstract class UQueryMatcher : HierarchyTraversal
	{
		internal List<RuleMatcher> m_Matchers;

		public override void Traverse(VisualElement element)
		{
			base.Traverse(element);
		}

		protected virtual bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
		{
			return false;
		}

		private static void NoProcessResult(VisualElement e, MatchResultInfo i)
		{
		}

		public override void TraverseRecursive(VisualElement element, int depth)
		{
			int count = m_Matchers.Count;
			int count2 = m_Matchers.Count;
			for (int i = 0; i < count2; i++)
			{
				RuleMatcher matcher = m_Matchers[i];
				if (StyleSelectorHelper.MatchRightToLeft(element, matcher.complexSelector, delegate(VisualElement e, MatchResultInfo i2)
				{
					NoProcessResult(e, i2);
				}) && OnRuleMatchedElement(matcher, element))
				{
					return;
				}
			}
			Recurse(element, depth);
			if (m_Matchers.Count > count)
			{
				m_Matchers.RemoveRange(count, m_Matchers.Count - count);
			}
		}

		public virtual void Run(VisualElement root, List<RuleMatcher> matchers)
		{
			m_Matchers = matchers;
			Traverse(root);
		}
	}

	internal abstract class SingleQueryMatcher : UQueryMatcher
	{
		public VisualElement match { get; set; }

		public override void Run(VisualElement root, List<RuleMatcher> matchers)
		{
			match = null;
			base.Run(root, matchers);
		}
	}

	internal class FirstQueryMatcher : SingleQueryMatcher
	{
		protected override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
		{
			if (base.match == null)
			{
				base.match = element;
			}
			return true;
		}
	}

	internal class LastQueryMatcher : SingleQueryMatcher
	{
		protected override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
		{
			base.match = element;
			return false;
		}
	}

	internal class IndexQueryMatcher : SingleQueryMatcher
	{
		private int matchCount = -1;

		private int _matchIndex;

		public int matchIndex
		{
			get
			{
				return _matchIndex;
			}
			set
			{
				matchCount = -1;
				_matchIndex = value;
			}
		}

		public override void Run(VisualElement root, List<RuleMatcher> matchers)
		{
			matchCount = -1;
			base.Run(root, matchers);
		}

		protected override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
		{
			matchCount++;
			if (matchCount == _matchIndex)
			{
				base.match = element;
			}
			return matchCount >= _matchIndex;
		}
	}
}
