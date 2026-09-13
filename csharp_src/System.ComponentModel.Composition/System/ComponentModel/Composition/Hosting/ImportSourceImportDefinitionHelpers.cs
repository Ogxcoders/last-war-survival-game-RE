using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq.Expressions;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Hosting;

internal static class ImportSourceImportDefinitionHelpers
{
	internal class NonImportSourceImportDefinition : ContractBasedImportDefinition
	{
		private ContractBasedImportDefinition _sourceDefinition;

		private IDictionary<string, object> _metadata;

		public override string ContractName => _sourceDefinition.ContractName;

		public override IDictionary<string, object> Metadata
		{
			get
			{
				IDictionary<string, object> dictionary = _metadata;
				if (dictionary == null)
				{
					dictionary = new Dictionary<string, object>(_sourceDefinition.Metadata);
					dictionary.Remove("System.ComponentModel.Composition.ImportSource");
					_metadata = dictionary;
				}
				return dictionary;
			}
		}

		public override ImportCardinality Cardinality => _sourceDefinition.Cardinality;

		public override Expression<Func<ExportDefinition, bool>> Constraint => _sourceDefinition.Constraint;

		public override bool IsPrerequisite => _sourceDefinition.IsPrerequisite;

		public override bool IsRecomposable => _sourceDefinition.IsRecomposable;

		public override string RequiredTypeIdentity => _sourceDefinition.RequiredTypeIdentity;

		public override IEnumerable<KeyValuePair<string, Type>> RequiredMetadata => _sourceDefinition.RequiredMetadata;

		public override CreationPolicy RequiredCreationPolicy => _sourceDefinition.RequiredCreationPolicy;

		public NonImportSourceImportDefinition(ContractBasedImportDefinition sourceDefinition)
		{
			Assumes.NotNull(sourceDefinition);
			_sourceDefinition = sourceDefinition;
			_metadata = null;
		}

		public override bool IsConstraintSatisfiedBy(ExportDefinition exportDefinition)
		{
			Requires.NotNull(exportDefinition, "exportDefinition");
			return _sourceDefinition.IsConstraintSatisfiedBy(exportDefinition);
		}

		public override string ToString()
		{
			return _sourceDefinition.ToString();
		}
	}

	public static ImportDefinition RemoveImportSource(this ImportDefinition definition)
	{
		if (!(definition is ContractBasedImportDefinition sourceDefinition))
		{
			return definition;
		}
		return new NonImportSourceImportDefinition(sourceDefinition);
	}
}
