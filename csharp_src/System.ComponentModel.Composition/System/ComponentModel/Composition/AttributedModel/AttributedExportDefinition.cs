using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.ReflectionModel;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.AttributedModel;

internal class AttributedExportDefinition : ExportDefinition
{
	private readonly AttributedPartCreationInfo _partCreationInfo;

	private readonly MemberInfo _member;

	private readonly ExportAttribute _exportAttribute;

	private readonly Type _typeIdentityType;

	private IDictionary<string, object> _metadata;

	public override IDictionary<string, object> Metadata
	{
		get
		{
			if (_metadata == null)
			{
				_member.TryExportMetadataForMember(out var dictionary);
				string value = (_exportAttribute.IsContractNameSameAsTypeIdentity() ? ContractName : _member.GetTypeIdentityFromExport(_typeIdentityType));
				dictionary.Add("ExportTypeIdentity", value);
				IDictionary<string, object> metadata = _partCreationInfo.GetMetadata();
				if (metadata != null && metadata.ContainsKey("System.ComponentModel.Composition.CreationPolicy"))
				{
					dictionary.Add("System.ComponentModel.Composition.CreationPolicy", metadata["System.ComponentModel.Composition.CreationPolicy"]);
				}
				if (_typeIdentityType != null && _member.MemberType != MemberTypes.Method && _typeIdentityType.ContainsGenericParameters)
				{
					dictionary.Add("System.ComponentModel.Composition.GenericExportParametersOrderMetadataName", GenericServices.GetGenericParametersOrder(_typeIdentityType));
				}
				_metadata = dictionary;
			}
			return _metadata;
		}
	}

	public AttributedExportDefinition(AttributedPartCreationInfo partCreationInfo, MemberInfo member, ExportAttribute exportAttribute, Type typeIdentityType, string contractName)
		: base(contractName, null)
	{
		Assumes.NotNull(partCreationInfo);
		Assumes.NotNull(member);
		Assumes.NotNull(exportAttribute);
		_partCreationInfo = partCreationInfo;
		_member = member;
		_exportAttribute = exportAttribute;
		_typeIdentityType = typeIdentityType;
	}
}
