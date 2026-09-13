using System;

namespace UnityEngine.UIElements;

public static class UQueryExtensions
{
	private class MissingVisualElementException : Exception
	{
		public MissingVisualElementException()
		{
		}

		public MissingVisualElementException(string message)
			: base(message)
		{
		}
	}

	private static UQueryState<VisualElement> SingleElementEmptyQuery = new UQueryBuilder<VisualElement>(null).Build();

	private static UQueryState<VisualElement> SingleElementNameQuery = new UQueryBuilder<VisualElement>(null).Name(string.Empty).Build();

	private static UQueryState<VisualElement> SingleElementClassQuery = new UQueryBuilder<VisualElement>(null).Class(string.Empty).Build();

	private static UQueryState<VisualElement> SingleElementNameAndClassQuery = new UQueryBuilder<VisualElement>(null).Name(string.Empty).Class(string.Empty).Build();

	private static UQueryState<VisualElement> SingleElementTypeQuery = new UQueryBuilder<VisualElement>(null).SingleBaseType().Build();

	private static UQueryState<VisualElement> SingleElementTypeAndNameQuery = new UQueryBuilder<VisualElement>(null).SingleBaseType().Name(string.Empty).Build();

	private static UQueryState<VisualElement> SingleElementTypeAndClassQuery = new UQueryBuilder<VisualElement>(null).SingleBaseType().Class(string.Empty).Build();

	private static UQueryState<VisualElement> SingleElementTypeAndNameAndClassQuery = new UQueryBuilder<VisualElement>(null).SingleBaseType().Name(string.Empty).Class(string.Empty)
		.Build();

	public static T Q<T>(this VisualElement e, string name = null, params string[] classes) where T : VisualElement
	{
		return e.Query<T>(name, classes).Build().First();
	}

	public static VisualElement Q(this VisualElement e, string name = null, params string[] classes)
	{
		return e.Query<VisualElement>(name, classes).Build().First();
	}

	public static T Q<T>(this VisualElement e, string name = null, string className = null) where T : VisualElement
	{
		if ((object)typeof(T) == typeof(VisualElement))
		{
			return e.Q(name, className) as T;
		}
		UQueryState<VisualElement> uQueryState;
		if (name == null)
		{
			if (className == null)
			{
				uQueryState = SingleElementTypeQuery.RebuildOn(e);
				uQueryState.m_Matchers[0].complexSelector.selectors[0].parts[0] = StyleSelectorPart.CreatePredicate(UQuery.IsOfType<T>.s_Instance);
				return uQueryState.First() as T;
			}
			uQueryState = SingleElementTypeAndClassQuery.RebuildOn(e);
			uQueryState.m_Matchers[0].complexSelector.selectors[0].parts[0] = StyleSelectorPart.CreatePredicate(UQuery.IsOfType<T>.s_Instance);
			uQueryState.m_Matchers[0].complexSelector.selectors[0].parts[1] = StyleSelectorPart.CreateClass(className);
			return uQueryState.First() as T;
		}
		if (className == null)
		{
			uQueryState = SingleElementTypeAndNameQuery.RebuildOn(e);
			uQueryState.m_Matchers[0].complexSelector.selectors[0].parts[0] = StyleSelectorPart.CreatePredicate(UQuery.IsOfType<T>.s_Instance);
			uQueryState.m_Matchers[0].complexSelector.selectors[0].parts[1] = StyleSelectorPart.CreateId(name);
			return uQueryState.First() as T;
		}
		uQueryState = SingleElementTypeAndNameAndClassQuery.RebuildOn(e);
		uQueryState.m_Matchers[0].complexSelector.selectors[0].parts[0] = StyleSelectorPart.CreatePredicate(UQuery.IsOfType<T>.s_Instance);
		uQueryState.m_Matchers[0].complexSelector.selectors[0].parts[1] = StyleSelectorPart.CreateId(name);
		uQueryState.m_Matchers[0].complexSelector.selectors[0].parts[2] = StyleSelectorPart.CreateClass(className);
		return uQueryState.First() as T;
	}

	internal static T MandatoryQ<T>(this VisualElement e, string name, string className = null) where T : VisualElement
	{
		T val = e.Q<T>(name, className);
		if (val == null)
		{
			throw new MissingVisualElementException("Element not found: " + name);
		}
		return val;
	}

	public static VisualElement Q(this VisualElement e, string name = null, string className = null)
	{
		UQueryState<VisualElement> uQueryState;
		if (name == null)
		{
			if (className == null)
			{
				return SingleElementEmptyQuery.RebuildOn(e).First();
			}
			uQueryState = SingleElementClassQuery.RebuildOn(e);
			uQueryState.m_Matchers[0].complexSelector.selectors[0].parts[0] = StyleSelectorPart.CreateClass(className);
			return uQueryState.First();
		}
		if (className == null)
		{
			uQueryState = SingleElementNameQuery.RebuildOn(e);
			uQueryState.m_Matchers[0].complexSelector.selectors[0].parts[0] = StyleSelectorPart.CreateId(name);
			return uQueryState.First();
		}
		uQueryState = SingleElementNameAndClassQuery.RebuildOn(e);
		uQueryState.m_Matchers[0].complexSelector.selectors[0].parts[0] = StyleSelectorPart.CreateId(name);
		uQueryState.m_Matchers[0].complexSelector.selectors[0].parts[1] = StyleSelectorPart.CreateClass(className);
		return uQueryState.First();
	}

	internal static VisualElement MandatoryQ(this VisualElement e, string name, string className = null)
	{
		VisualElement visualElement = e.Q<VisualElement>(name, className);
		if (visualElement == null)
		{
			throw new MissingVisualElementException("Element not found: " + name);
		}
		return visualElement;
	}

	public static UQueryBuilder<VisualElement> Query(this VisualElement e, string name = null, params string[] classes)
	{
		return e.Query<VisualElement>(name, classes);
	}

	public static UQueryBuilder<VisualElement> Query(this VisualElement e, string name = null, string className = null)
	{
		return e.Query<VisualElement>(name, className);
	}

	public static UQueryBuilder<T> Query<T>(this VisualElement e, string name = null, params string[] classes) where T : VisualElement
	{
		return new UQueryBuilder<VisualElement>(e).OfType<T>(name, classes);
	}

	public static UQueryBuilder<T> Query<T>(this VisualElement e, string name = null, string className = null) where T : VisualElement
	{
		return new UQueryBuilder<VisualElement>(e).OfType<T>(name, className);
	}

	public static UQueryBuilder<VisualElement> Query(this VisualElement e)
	{
		return new UQueryBuilder<VisualElement>(e);
	}
}
