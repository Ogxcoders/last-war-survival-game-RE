namespace System.ComponentModel.Composition.Primitives;

internal static class CompositionElementExtensions
{
	public static ICompositionElement ToSerializableElement(this ICompositionElement element)
	{
		return SerializableCompositionElement.FromICompositionElement(element);
	}

	public static ICompositionElement ToElement(this Export export)
	{
		if (export is ICompositionElement result)
		{
			return result;
		}
		return export.Definition.ToElement();
	}

	public static ICompositionElement ToElement(this ExportDefinition definition)
	{
		return ToElementCore(definition);
	}

	public static ICompositionElement ToElement(this ImportDefinition definition)
	{
		return ToElementCore(definition);
	}

	public static ICompositionElement ToElement(this ComposablePart part)
	{
		return ToElementCore(part);
	}

	public static ICompositionElement ToElement(this ComposablePartDefinition definition)
	{
		return ToElementCore(definition);
	}

	public static string GetDisplayName(this ComposablePartDefinition definition)
	{
		return GetDisplayNameCore(definition);
	}

	public static string GetDisplayName(this ComposablePartCatalog catalog)
	{
		return GetDisplayNameCore(catalog);
	}

	private static string GetDisplayNameCore(object value)
	{
		if (value is ICompositionElement compositionElement)
		{
			return compositionElement.DisplayName;
		}
		return value.ToString();
	}

	private static ICompositionElement ToElementCore(object value)
	{
		if (value is ICompositionElement result)
		{
			return result;
		}
		return new CompositionElement(value);
	}
}
