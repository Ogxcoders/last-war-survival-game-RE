using System.Collections.Generic;
using System.Linq;

namespace System.ComponentModel.Composition.Primitives;

public abstract class ComposablePartDefinition
{
	internal static readonly IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> _EmptyExports = Enumerable.Empty<Tuple<ComposablePartDefinition, ExportDefinition>>();

	public abstract IEnumerable<ExportDefinition> ExportDefinitions { get; }

	public abstract IEnumerable<ImportDefinition> ImportDefinitions { get; }

	public virtual IDictionary<string, object> Metadata => MetadataServices.EmptyMetadata;

	public abstract ComposablePart CreatePart();

	internal virtual IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
	{
		List<Tuple<ComposablePartDefinition, ExportDefinition>> list = null;
		foreach (ExportDefinition exportDefinition in ExportDefinitions)
		{
			if (definition.IsConstraintSatisfiedBy(exportDefinition))
			{
				if (list == null)
				{
					list = new List<Tuple<ComposablePartDefinition, ExportDefinition>>();
				}
				list.Add(new Tuple<ComposablePartDefinition, ExportDefinition>(this, exportDefinition));
			}
		}
		IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> enumerable = list;
		return enumerable ?? _EmptyExports;
	}

	internal virtual ComposablePartDefinition GetGenericPartDefinition()
	{
		return null;
	}
}
