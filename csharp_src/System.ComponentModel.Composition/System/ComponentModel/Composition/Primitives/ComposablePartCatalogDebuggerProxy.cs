using System.Collections.ObjectModel;
using Microsoft.Internal;
using Microsoft.Internal.Collections;

namespace System.ComponentModel.Composition.Primitives;

internal class ComposablePartCatalogDebuggerProxy
{
	private readonly ComposablePartCatalog _catalog;

	public ReadOnlyCollection<ComposablePartDefinition> Parts => _catalog.Parts.ToReadOnlyCollection();

	public ComposablePartCatalogDebuggerProxy(ComposablePartCatalog catalog)
	{
		Requires.NotNull(catalog, "catalog");
		_catalog = catalog;
	}
}
