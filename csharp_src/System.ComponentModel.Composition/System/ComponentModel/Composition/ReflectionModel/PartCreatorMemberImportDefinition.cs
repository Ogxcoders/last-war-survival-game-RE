using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq.Expressions;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.ReflectionModel;

internal class PartCreatorMemberImportDefinition : ReflectionMemberImportDefinition, IPartCreatorImportDefinition
{
	private readonly ContractBasedImportDefinition _productImportDefinition;

	public ContractBasedImportDefinition ProductImportDefinition => _productImportDefinition;

	public override Expression<Func<ExportDefinition, bool>> Constraint => ConstraintServices.CreatePartCreatorConstraint(base.Constraint, _productImportDefinition);

	public PartCreatorMemberImportDefinition(LazyMemberInfo importingLazyMember, ICompositionElement origin, ContractBasedImportDefinition productImportDefinition)
		: base(importingLazyMember, "System.ComponentModel.Composition.Contracts.ExportFactory", CompositionConstants.PartCreatorTypeIdentity, productImportDefinition.RequiredMetadata, productImportDefinition.Cardinality, productImportDefinition.IsRecomposable, isPrerequisite: false, productImportDefinition.RequiredCreationPolicy, MetadataServices.EmptyMetadata, origin)
	{
		Assumes.NotNull(productImportDefinition);
		_productImportDefinition = productImportDefinition;
	}

	public override bool IsConstraintSatisfiedBy(ExportDefinition exportDefinition)
	{
		if (!base.IsConstraintSatisfiedBy(exportDefinition))
		{
			return false;
		}
		return PartCreatorExportDefinition.IsProductConstraintSatisfiedBy(_productImportDefinition, exportDefinition);
	}
}
