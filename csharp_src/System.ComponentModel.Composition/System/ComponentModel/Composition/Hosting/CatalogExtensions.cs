using System.ComponentModel.Composition.Primitives;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Hosting;

public static class CatalogExtensions
{
	public static CompositionService CreateCompositionService(this ComposablePartCatalog composablePartCatalog)
	{
		Requires.NotNull(composablePartCatalog, "composablePartCatalog");
		return new CompositionService(composablePartCatalog);
	}
}
