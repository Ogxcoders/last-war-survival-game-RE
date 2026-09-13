using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Primitives;

public class ContractBasedImportDefinition : ImportDefinition
{
	private readonly IEnumerable<KeyValuePair<string, Type>> _requiredMetadata = Enumerable.Empty<KeyValuePair<string, Type>>();

	private Expression<Func<ExportDefinition, bool>> _constraint;

	private readonly CreationPolicy _requiredCreationPolicy;

	private readonly string _requiredTypeIdentity;

	private bool _isRequiredMetadataValidated;

	public virtual string RequiredTypeIdentity => _requiredTypeIdentity;

	public virtual IEnumerable<KeyValuePair<string, Type>> RequiredMetadata
	{
		get
		{
			ValidateRequiredMetadata();
			return _requiredMetadata;
		}
	}

	public virtual CreationPolicy RequiredCreationPolicy => _requiredCreationPolicy;

	public override Expression<Func<ExportDefinition, bool>> Constraint
	{
		get
		{
			if (_constraint == null)
			{
				_constraint = ConstraintServices.CreateConstraint(ContractName, RequiredTypeIdentity, RequiredMetadata, RequiredCreationPolicy);
			}
			return _constraint;
		}
	}

	protected ContractBasedImportDefinition()
	{
	}

	public ContractBasedImportDefinition(string contractName, string requiredTypeIdentity, IEnumerable<KeyValuePair<string, Type>> requiredMetadata, ImportCardinality cardinality, bool isRecomposable, bool isPrerequisite, CreationPolicy requiredCreationPolicy)
		: this(contractName, requiredTypeIdentity, requiredMetadata, cardinality, isRecomposable, isPrerequisite, requiredCreationPolicy, MetadataServices.EmptyMetadata)
	{
	}

	public ContractBasedImportDefinition(string contractName, string requiredTypeIdentity, IEnumerable<KeyValuePair<string, Type>> requiredMetadata, ImportCardinality cardinality, bool isRecomposable, bool isPrerequisite, CreationPolicy requiredCreationPolicy, IDictionary<string, object> metadata)
		: base(contractName, cardinality, isRecomposable, isPrerequisite, metadata)
	{
		Requires.NotNullOrEmpty(contractName, "contractName");
		_requiredTypeIdentity = requiredTypeIdentity;
		if (requiredMetadata != null)
		{
			_requiredMetadata = requiredMetadata;
		}
		_requiredCreationPolicy = requiredCreationPolicy;
	}

	private void ValidateRequiredMetadata()
	{
		if (_isRequiredMetadataValidated)
		{
			return;
		}
		foreach (KeyValuePair<string, Type> requiredMetadatum in _requiredMetadata)
		{
			if (requiredMetadatum.Key == null || requiredMetadatum.Value == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.Argument_NullElement, "requiredMetadata"));
			}
		}
		_isRequiredMetadataValidated = true;
	}

	public override bool IsConstraintSatisfiedBy(ExportDefinition exportDefinition)
	{
		Requires.NotNull(exportDefinition, "exportDefinition");
		if (!StringComparers.ContractName.Equals(ContractName, exportDefinition.ContractName))
		{
			return false;
		}
		return MatchRequiredMatadata(exportDefinition);
	}

	private bool MatchRequiredMatadata(ExportDefinition definition)
	{
		if (!string.IsNullOrEmpty(RequiredTypeIdentity))
		{
			string value = definition.Metadata.GetValue<string>("ExportTypeIdentity");
			if (!StringComparers.ContractName.Equals(RequiredTypeIdentity, value))
			{
				return false;
			}
		}
		foreach (KeyValuePair<string, Type> requiredMetadatum in RequiredMetadata)
		{
			string key = requiredMetadatum.Key;
			Type value2 = requiredMetadatum.Value;
			object value3 = null;
			if (!definition.Metadata.TryGetValue(key, out value3))
			{
				return false;
			}
			if (value3 != null)
			{
				if (!value2.IsInstanceOfType(value3))
				{
					return false;
				}
			}
			else if (value2.IsValueType)
			{
				return false;
			}
		}
		if (RequiredCreationPolicy == CreationPolicy.Any)
		{
			return true;
		}
		CreationPolicy value4 = definition.Metadata.GetValue<CreationPolicy>("System.ComponentModel.Composition.CreationPolicy");
		if (value4 != CreationPolicy.Any)
		{
			return value4 == RequiredCreationPolicy;
		}
		return true;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append($"\n\tContractName\t{ContractName}");
		stringBuilder.Append($"\n\tRequiredTypeIdentity\t{RequiredTypeIdentity}");
		if (_requiredCreationPolicy != CreationPolicy.Any)
		{
			stringBuilder.Append($"\n\tRequiredCreationPolicy\t{RequiredCreationPolicy}");
		}
		if (_requiredMetadata.Count() > 0)
		{
			stringBuilder.Append($"\n\tRequiredMetadata");
			foreach (KeyValuePair<string, Type> requiredMetadatum in _requiredMetadata)
			{
				stringBuilder.Append($"\n\t\t{requiredMetadatum.Key}\t({requiredMetadatum.Value})");
			}
		}
		return stringBuilder.ToString();
	}
}
