using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition.ReflectionModel;

internal class LazyExportDefinition : ExportDefinition
{
	private readonly Lazy<IDictionary<string, object>> _metadata;

	public override IDictionary<string, object> Metadata => _metadata.Value ?? MetadataServices.EmptyMetadata;

	public LazyExportDefinition(string contractName, Lazy<IDictionary<string, object>> metadata)
		: base(contractName, null)
	{
		_metadata = metadata;
	}
}
