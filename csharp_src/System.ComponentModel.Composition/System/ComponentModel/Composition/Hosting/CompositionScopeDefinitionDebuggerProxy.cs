using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition.Primitives;
using Microsoft.Internal;
using Microsoft.Internal.Collections;

namespace System.ComponentModel.Composition.Hosting;

internal class CompositionScopeDefinitionDebuggerProxy
{
	private readonly CompositionScopeDefinition _compositionScopeDefinition;

	public ReadOnlyCollection<ComposablePartDefinition> Parts => _compositionScopeDefinition.Parts.ToReadOnlyCollection();

	public IEnumerable<ExportDefinition> PublicSurface => _compositionScopeDefinition.PublicSurface.ToReadOnlyCollection();

	public virtual IEnumerable<CompositionScopeDefinition> Children => _compositionScopeDefinition.Children.ToReadOnlyCollection();

	public CompositionScopeDefinitionDebuggerProxy(CompositionScopeDefinition compositionScopeDefinition)
	{
		Requires.NotNull(compositionScopeDefinition, "compositionScopeDefinition");
		_compositionScopeDefinition = compositionScopeDefinition;
	}
}
