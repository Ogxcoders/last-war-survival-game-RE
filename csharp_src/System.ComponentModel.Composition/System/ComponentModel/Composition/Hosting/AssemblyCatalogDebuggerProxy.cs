using System.Collections.ObjectModel;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;
using Microsoft.Internal;
using Microsoft.Internal.Collections;

namespace System.ComponentModel.Composition.Hosting;

internal class AssemblyCatalogDebuggerProxy
{
	private readonly AssemblyCatalog _catalog;

	public Assembly Assembly => _catalog.Assembly;

	public ReadOnlyCollection<ComposablePartDefinition> Parts => _catalog.Parts.ToReadOnlyCollection();

	public AssemblyCatalogDebuggerProxy(AssemblyCatalog catalog)
	{
		Requires.NotNull(catalog, "catalog");
		_catalog = catalog;
	}
}
