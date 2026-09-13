using Microsoft.Internal;

namespace System.ComponentModel.Composition.Primitives;

internal class CompositionElementDebuggerProxy
{
	private readonly CompositionElement _element;

	public string DisplayName => _element.DisplayName;

	public ICompositionElement Origin => _element.Origin;

	public object UnderlyingObject => _element.UnderlyingObject;

	public CompositionElementDebuggerProxy(CompositionElement element)
	{
		Requires.NotNull(element, "element");
		_element = element;
	}
}
