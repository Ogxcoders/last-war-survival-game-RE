using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition.ReflectionModel;

internal class PartCreatorExportDefinition : ExportDefinition
{
	private readonly ExportDefinition _productDefinition;

	private IDictionary<string, object> _metadata;

	public override string ContractName => "System.ComponentModel.Composition.Contracts.ExportFactory";

	public override IDictionary<string, object> Metadata
	{
		get
		{
			if (_metadata == null)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>(_productDefinition.Metadata);
				dictionary["ExportTypeIdentity"] = CompositionConstants.PartCreatorTypeIdentity;
				dictionary["ProductDefinition"] = _productDefinition;
				_metadata = dictionary.AsReadOnly();
			}
			return _metadata;
		}
	}

	public PartCreatorExportDefinition(ExportDefinition productDefinition)
	{
		_productDefinition = productDefinition;
	}

	internal static bool IsProductConstraintSatisfiedBy(ImportDefinition productImportDefinition, ExportDefinition exportDefinition)
	{
		object value = null;
		if (exportDefinition.Metadata.TryGetValue("ProductDefinition", out value) && value is ExportDefinition exportDefinition2)
		{
			return productImportDefinition.IsConstraintSatisfiedBy(exportDefinition2);
		}
		return false;
	}
}
