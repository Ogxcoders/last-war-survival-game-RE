using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Primitives;

public class ImportDefinition
{
	internal static readonly string EmptyContractName = string.Empty;

	private readonly Expression<Func<ExportDefinition, bool>> _constraint;

	private readonly ImportCardinality _cardinality = ImportCardinality.ExactlyOne;

	private readonly string _contractName = EmptyContractName;

	private readonly bool _isRecomposable;

	private readonly bool _isPrerequisite = true;

	private Func<ExportDefinition, bool> _compiledConstraint;

	private readonly IDictionary<string, object> _metadata = MetadataServices.EmptyMetadata;

	public virtual string ContractName => _contractName;

	public virtual IDictionary<string, object> Metadata => _metadata;

	public virtual ImportCardinality Cardinality => _cardinality;

	public virtual Expression<Func<ExportDefinition, bool>> Constraint
	{
		get
		{
			if (_constraint != null)
			{
				return _constraint;
			}
			throw ExceptionBuilder.CreateNotOverriddenByDerived("Constraint");
		}
	}

	public virtual bool IsPrerequisite => _isPrerequisite;

	public virtual bool IsRecomposable => _isRecomposable;

	protected ImportDefinition()
	{
	}

	public ImportDefinition(Expression<Func<ExportDefinition, bool>> constraint, string contractName, ImportCardinality cardinality, bool isRecomposable, bool isPrerequisite)
		: this(contractName, cardinality, isRecomposable, isPrerequisite, MetadataServices.EmptyMetadata)
	{
		Requires.NotNull(constraint, "constraint");
		_constraint = constraint;
	}

	public ImportDefinition(Expression<Func<ExportDefinition, bool>> constraint, string contractName, ImportCardinality cardinality, bool isRecomposable, bool isPrerequisite, IDictionary<string, object> metadata)
		: this(contractName, cardinality, isRecomposable, isPrerequisite, metadata)
	{
		Requires.NotNull(constraint, "constraint");
		_constraint = constraint;
	}

	internal ImportDefinition(string contractName, ImportCardinality cardinality, bool isRecomposable, bool isPrerequisite, IDictionary<string, object> metadata)
	{
		if (cardinality != ImportCardinality.ExactlyOne && cardinality != ImportCardinality.ZeroOrMore && cardinality != ImportCardinality.ZeroOrOne)
		{
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.ArgumentOutOfRange_InvalidEnum, "cardinality", cardinality, typeof(ImportCardinality).Name), "cardinality");
		}
		_contractName = contractName ?? EmptyContractName;
		_cardinality = cardinality;
		_isRecomposable = isRecomposable;
		_isPrerequisite = isPrerequisite;
		if (metadata != null)
		{
			_metadata = metadata;
		}
	}

	public virtual bool IsConstraintSatisfiedBy(ExportDefinition exportDefinition)
	{
		Requires.NotNull(exportDefinition, "exportDefinition");
		if (_compiledConstraint == null)
		{
			_compiledConstraint = Constraint.Compile();
		}
		return _compiledConstraint(exportDefinition);
	}

	public override string ToString()
	{
		return Constraint.Body.ToString();
	}
}
