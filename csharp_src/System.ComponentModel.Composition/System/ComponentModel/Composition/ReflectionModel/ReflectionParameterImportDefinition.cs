using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Globalization;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.ReflectionModel;

internal class ReflectionParameterImportDefinition : ReflectionImportDefinition
{
	private Lazy<ParameterInfo> _importingLazyParameter;

	public Lazy<ParameterInfo> ImportingLazyParameter => _importingLazyParameter;

	public ReflectionParameterImportDefinition(Lazy<ParameterInfo> importingLazyParameter, string contractName, string requiredTypeIdentity, IEnumerable<KeyValuePair<string, Type>> requiredMetadata, ImportCardinality cardinality, CreationPolicy requiredCreationPolicy, IDictionary<string, object> metadata, ICompositionElement origin)
		: base(contractName, requiredTypeIdentity, requiredMetadata, cardinality, isRecomposable: false, isPrerequisite: true, requiredCreationPolicy, metadata, origin)
	{
		Assumes.NotNull(importingLazyParameter);
		_importingLazyParameter = importingLazyParameter;
	}

	public override ImportingItem ToImportingItem()
	{
		return new ImportingParameter(this, new ImportType(ImportingLazyParameter.GetNotNullValue("parameter").ParameterType, Cardinality));
	}

	protected override string GetDisplayName()
	{
		ParameterInfo notNullValue = ImportingLazyParameter.GetNotNullValue("parameter");
		return string.Format(CultureInfo.CurrentCulture, "{0} (Parameter=\"{1}\", ContractName=\"{2}\")", notNullValue.Member.GetDisplayName(), notNullValue.Name, ContractName);
	}
}
