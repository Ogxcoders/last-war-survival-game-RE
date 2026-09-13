using Microsoft.Internal;

namespace System.ComponentModel.Composition.Primitives;

[Serializable]
internal class SerializableCompositionElement : ICompositionElement
{
	private readonly string _displayName;

	private readonly ICompositionElement _origin;

	public string DisplayName => _displayName;

	public ICompositionElement Origin => _origin;

	public SerializableCompositionElement(string displayName, ICompositionElement origin)
	{
		Assumes.IsTrue(origin?.GetType().IsSerializable ?? true);
		_displayName = displayName ?? string.Empty;
		_origin = origin;
	}

	public override string ToString()
	{
		return DisplayName;
	}

	public static ICompositionElement FromICompositionElement(ICompositionElement element)
	{
		if (element == null)
		{
			return null;
		}
		ICompositionElement origin = FromICompositionElement(element.Origin);
		return new SerializableCompositionElement(element.DisplayName, origin);
	}
}
