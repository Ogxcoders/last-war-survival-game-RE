using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Globalization;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.ReflectionModel;

internal class ReflectionMemberExportDefinition : ExportDefinition, ICompositionElement
{
	private readonly LazyMemberInfo _member;

	private readonly ExportDefinition _exportDefinition;

	private readonly ICompositionElement _origin;

	private IDictionary<string, object> _metadata;

	public override string ContractName => _exportDefinition.ContractName;

	public LazyMemberInfo ExportingLazyMember => _member;

	public override IDictionary<string, object> Metadata
	{
		get
		{
			if (_metadata == null)
			{
				_metadata = _exportDefinition.Metadata.AsReadOnly();
			}
			return _metadata;
		}
	}

	string ICompositionElement.DisplayName => GetDisplayName();

	ICompositionElement ICompositionElement.Origin => _origin;

	public ReflectionMemberExportDefinition(LazyMemberInfo member, ExportDefinition exportDefinition, ICompositionElement origin)
	{
		Assumes.NotNull(exportDefinition);
		_member = member;
		_exportDefinition = exportDefinition;
		_origin = origin;
	}

	public override string ToString()
	{
		return GetDisplayName();
	}

	public int GetIndex()
	{
		return ExportingLazyMember.ToReflectionMember().UnderlyingMember.MetadataToken;
	}

	public ExportingMember ToExportingMember()
	{
		return new ExportingMember(this, ToReflectionMember());
	}

	private ReflectionMember ToReflectionMember()
	{
		return ExportingLazyMember.ToReflectionMember();
	}

	private string GetDisplayName()
	{
		return string.Format(CultureInfo.CurrentCulture, "{0} (ContractName=\"{1}\")", ToReflectionMember().GetDisplayName(), ContractName);
	}
}
