using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Hosting;

public class CompositionBatch
{
	private class SingleExportComposablePart : ComposablePart
	{
		private readonly Export _export;

		public override IDictionary<string, object> Metadata => MetadataServices.EmptyMetadata;

		public override IEnumerable<ExportDefinition> ExportDefinitions => new ExportDefinition[1] { _export.Definition };

		public override IEnumerable<ImportDefinition> ImportDefinitions => Enumerable.Empty<ImportDefinition>();

		public SingleExportComposablePart(Export export)
		{
			Assumes.NotNull(export);
			_export = export;
		}

		public override object GetExportedValue(ExportDefinition definition)
		{
			Requires.NotNull(definition, "definition");
			if (definition != _export.Definition)
			{
				throw ExceptionBuilder.CreateExportDefinitionNotOnThisComposablePart("definition");
			}
			return _export.Value;
		}

		public override void SetImport(ImportDefinition definition, IEnumerable<Export> exports)
		{
			Requires.NotNull(definition, "definition");
			Requires.NotNullOrNullElements(exports, "exports");
			throw ExceptionBuilder.CreateImportDefinitionNotOnThisComposablePart("definition");
		}
	}

	private object _lock = new object();

	private bool _copyNeededForAdd;

	private bool _copyNeededForRemove;

	private List<ComposablePart> _partsToAdd;

	private ReadOnlyCollection<ComposablePart> _readOnlyPartsToAdd;

	private List<ComposablePart> _partsToRemove;

	private ReadOnlyCollection<ComposablePart> _readOnlyPartsToRemove;

	public ReadOnlyCollection<ComposablePart> PartsToAdd
	{
		get
		{
			lock (_lock)
			{
				_copyNeededForAdd = true;
				return _readOnlyPartsToAdd;
			}
		}
	}

	public ReadOnlyCollection<ComposablePart> PartsToRemove
	{
		get
		{
			lock (_lock)
			{
				_copyNeededForRemove = true;
				return _readOnlyPartsToRemove;
			}
		}
	}

	public CompositionBatch()
		: this(null, null)
	{
	}

	public CompositionBatch(IEnumerable<ComposablePart> partsToAdd, IEnumerable<ComposablePart> partsToRemove)
	{
		_partsToAdd = new List<ComposablePart>();
		if (partsToAdd != null)
		{
			foreach (ComposablePart item in partsToAdd)
			{
				if (item == null)
				{
					throw ExceptionBuilder.CreateContainsNullElement("partsToAdd");
				}
				_partsToAdd.Add(item);
			}
		}
		_readOnlyPartsToAdd = _partsToAdd.AsReadOnly();
		_partsToRemove = new List<ComposablePart>();
		if (partsToRemove != null)
		{
			foreach (ComposablePart item2 in partsToRemove)
			{
				if (item2 == null)
				{
					throw ExceptionBuilder.CreateContainsNullElement("partsToRemove");
				}
				_partsToRemove.Add(item2);
			}
		}
		_readOnlyPartsToRemove = _partsToRemove.AsReadOnly();
	}

	public void AddPart(ComposablePart part)
	{
		Requires.NotNull(part, "part");
		lock (_lock)
		{
			if (_copyNeededForAdd)
			{
				_partsToAdd = new List<ComposablePart>(_partsToAdd);
				_readOnlyPartsToAdd = _partsToAdd.AsReadOnly();
				_copyNeededForAdd = false;
			}
			_partsToAdd.Add(part);
		}
	}

	public void RemovePart(ComposablePart part)
	{
		Requires.NotNull(part, "part");
		lock (_lock)
		{
			if (_copyNeededForRemove)
			{
				_partsToRemove = new List<ComposablePart>(_partsToRemove);
				_readOnlyPartsToRemove = _partsToRemove.AsReadOnly();
				_copyNeededForRemove = false;
			}
			_partsToRemove.Add(part);
		}
	}

	public ComposablePart AddExport(Export export)
	{
		Requires.NotNull(export, "export");
		ComposablePart composablePart = new SingleExportComposablePart(export);
		AddPart(composablePart);
		return composablePart;
	}
}
