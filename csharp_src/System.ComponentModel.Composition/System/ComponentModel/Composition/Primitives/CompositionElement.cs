using System.Diagnostics;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Primitives;

[Serializable]
[DebuggerTypeProxy(typeof(CompositionElementDebuggerProxy))]
internal class CompositionElement : SerializableCompositionElement
{
	private static readonly ICompositionElement UnknownOrigin = new SerializableCompositionElement(Strings.CompositionElement_UnknownOrigin, null);

	private readonly object _underlyingObject;

	public object UnderlyingObject => _underlyingObject;

	public CompositionElement(object underlyingObject)
		: base(underlyingObject.ToString(), UnknownOrigin)
	{
		_underlyingObject = underlyingObject;
	}
}
