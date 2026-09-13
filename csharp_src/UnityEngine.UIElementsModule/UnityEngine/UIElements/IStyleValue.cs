using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements;

internal interface IStyleValue<T>
{
	T value { get; set; }

	int specificity { get; set; }

	StyleKeyword keyword { get; set; }

	bool Apply<U>(U otherValue, StylePropertyApplyMode mode) where U : IStyleValue<T>;
}
