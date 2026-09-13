using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Globalization;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.ReflectionModel;

internal class ReflectionMemberImportDefinition : ReflectionImportDefinition
{
	private LazyMemberInfo _importingLazyMember;

	public LazyMemberInfo ImportingLazyMember => _importingLazyMember;

	public ReflectionMemberImportDefinition(LazyMemberInfo importingLazyMember, string contractName, string requiredTypeIdentity, IEnumerable<KeyValuePair<string, Type>> requiredMetadata, ImportCardinality cardinality, bool isRecomposable, bool isPrerequisite, CreationPolicy requiredCreationPolicy, IDictionary<string, object> metadata, ICompositionElement origin)
		: base(contractName, requiredTypeIdentity, requiredMetadata, cardinality, isRecomposable, isPrerequisite, requiredCreationPolicy, metadata, origin)
	{
		Assumes.NotNull(contractName);
		_importingLazyMember = importingLazyMember;
	}

	public override ImportingItem ToImportingItem()
	{
		ReflectionWritableMember reflectionWritableMember = ImportingLazyMember.ToReflectionWriteableMember();
		return new ImportingMember(this, reflectionWritableMember, new ImportType(reflectionWritableMember.ReturnType, Cardinality));
	}

	protected override string GetDisplayName()
	{
		return string.Format(CultureInfo.CurrentCulture, "{0} (ContractName=\"{1}\")", ImportingLazyMember.ToReflectionMember().GetDisplayName(), ContractName);
	}
}
