using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using Microsoft.Internal;
using Microsoft.Internal.Collections;

namespace System.ComponentModel.Composition.Hosting;

public class ComposablePartCatalogChangeEventArgs : EventArgs
{
	private readonly IEnumerable<ComposablePartDefinition> _addedDefinitions;

	private readonly IEnumerable<ComposablePartDefinition> _removedDefinitions;

	public IEnumerable<ComposablePartDefinition> AddedDefinitions => _addedDefinitions;

	public IEnumerable<ComposablePartDefinition> RemovedDefinitions => _removedDefinitions;

	public AtomicComposition AtomicComposition { get; private set; }

	public ComposablePartCatalogChangeEventArgs(IEnumerable<ComposablePartDefinition> addedDefinitions, IEnumerable<ComposablePartDefinition> removedDefinitions, AtomicComposition atomicComposition)
	{
		Requires.NotNull(addedDefinitions, "addedDefinitions");
		Requires.NotNull(removedDefinitions, "removedDefinitions");
		_addedDefinitions = addedDefinitions.AsArray();
		_removedDefinitions = removedDefinitions.AsArray();
		AtomicComposition = atomicComposition;
	}
}
