using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using Microsoft.Internal;
using Microsoft.Internal.Collections;

namespace System.ComponentModel.Composition.Hosting;

public class ExportsChangeEventArgs : EventArgs
{
	private readonly IEnumerable<ExportDefinition> _addedExports;

	private readonly IEnumerable<ExportDefinition> _removedExports;

	private IEnumerable<string> _changedContractNames;

	public IEnumerable<ExportDefinition> AddedExports => _addedExports;

	public IEnumerable<ExportDefinition> RemovedExports => _removedExports;

	public IEnumerable<string> ChangedContractNames
	{
		get
		{
			if (_changedContractNames == null)
			{
				_changedContractNames = (from export in AddedExports.Concat(RemovedExports)
					select export.ContractName).Distinct().ToArray();
			}
			return _changedContractNames;
		}
	}

	public AtomicComposition AtomicComposition { get; private set; }

	public ExportsChangeEventArgs(IEnumerable<ExportDefinition> addedExports, IEnumerable<ExportDefinition> removedExports, AtomicComposition atomicComposition)
	{
		Requires.NotNull(addedExports, "addedExports");
		Requires.NotNull(removedExports, "removedExports");
		_addedExports = addedExports.AsArray();
		_removedExports = removedExports.AsArray();
		AtomicComposition = atomicComposition;
	}
}
