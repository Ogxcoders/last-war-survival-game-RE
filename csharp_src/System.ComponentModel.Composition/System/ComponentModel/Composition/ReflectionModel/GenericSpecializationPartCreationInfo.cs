using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Microsoft.Internal;
using Microsoft.Internal.Collections;

namespace System.ComponentModel.Composition.ReflectionModel;

internal class GenericSpecializationPartCreationInfo : IReflectionPartCreationInfo, ICompositionElement
{
	private readonly IReflectionPartCreationInfo _originalPartCreationInfo;

	private readonly ReflectionComposablePartDefinition _originalPart;

	private readonly Type[] _specialization;

	private readonly string[] _specializationIdentities;

	private IEnumerable<ExportDefinition> _exports;

	private IEnumerable<ImportDefinition> _imports;

	private readonly Lazy<Type> _lazyPartType;

	private List<LazyMemberInfo> _members;

	private List<Lazy<ParameterInfo>> _parameters;

	private Dictionary<LazyMemberInfo, MemberInfo[]> _membersTable;

	private Dictionary<Lazy<ParameterInfo>, ParameterInfo> _parametersTable;

	private ConstructorInfo _constructor;

	private object _lock = new object();

	public ReflectionComposablePartDefinition OriginalPart => _originalPart;

	public bool IsDisposalRequired => _originalPartCreationInfo.IsDisposalRequired;

	public string DisplayName => Translate(_originalPartCreationInfo.DisplayName);

	public ICompositionElement Origin => _originalPartCreationInfo.Origin;

	public GenericSpecializationPartCreationInfo(IReflectionPartCreationInfo originalPartCreationInfo, ReflectionComposablePartDefinition originalPart, Type[] specialization)
	{
		GenericSpecializationPartCreationInfo genericSpecializationPartCreationInfo = this;
		Assumes.NotNull(originalPartCreationInfo);
		Assumes.NotNull(specialization);
		Assumes.NotNull(originalPart);
		_originalPartCreationInfo = originalPartCreationInfo;
		_originalPart = originalPart;
		_specialization = specialization;
		_specializationIdentities = new string[_specialization.Length];
		for (int i = 0; i < _specialization.Length; i++)
		{
			_specializationIdentities[i] = AttributedModelServices.GetTypeIdentity(_specialization[i]);
		}
		_lazyPartType = new Lazy<Type>(() => genericSpecializationPartCreationInfo._originalPartCreationInfo.GetPartType().MakeGenericType(specialization), LazyThreadSafetyMode.PublicationOnly);
	}

	public Type GetPartType()
	{
		return _lazyPartType.Value;
	}

	public Lazy<Type> GetLazyPartType()
	{
		return _lazyPartType;
	}

	public ConstructorInfo GetConstructor()
	{
		if (_constructor == null)
		{
			ConstructorInfo constructor = _originalPartCreationInfo.GetConstructor();
			ConstructorInfo constructor2 = null;
			if (constructor != null)
			{
				ConstructorInfo[] constructors = GetPartType().GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (ConstructorInfo constructorInfo in constructors)
				{
					if (constructorInfo.MetadataToken == constructor.MetadataToken)
					{
						constructor2 = constructorInfo;
						break;
					}
				}
			}
			Thread.MemoryBarrier();
			lock (_lock)
			{
				if (_constructor == null)
				{
					_constructor = constructor2;
				}
			}
		}
		return _constructor;
	}

	public IDictionary<string, object> GetMetadata()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>(_originalPartCreationInfo.GetMetadata(), StringComparers.MetadataKeyNames);
		dictionary.Remove("System.ComponentModel.Composition.IsGenericPart");
		dictionary.Remove("System.ComponentModel.Composition.GenericPartArity");
		dictionary.Remove("System.ComponentModel.Composition.GenericParameterConstraints");
		dictionary.Remove("System.ComponentModel.Composition.GenericParameterAttributes");
		return dictionary;
	}

	private MemberInfo[] GetAccessors(LazyMemberInfo originalLazyMember)
	{
		BuildTables();
		Assumes.NotNull(_membersTable);
		return _membersTable[originalLazyMember];
	}

	private ParameterInfo GetParameter(Lazy<ParameterInfo> originalParameter)
	{
		BuildTables();
		Assumes.NotNull(_parametersTable);
		return _parametersTable[originalParameter];
	}

	private void BuildTables()
	{
		if (_membersTable != null)
		{
			return;
		}
		PopulateImportsAndExports();
		List<LazyMemberInfo> list = null;
		List<Lazy<ParameterInfo>> parameters = null;
		lock (_lock)
		{
			if (_membersTable == null)
			{
				list = _members;
				parameters = _parameters;
				Assumes.NotNull(list);
			}
		}
		Dictionary<LazyMemberInfo, MemberInfo[]> membersTable = BuildMembersTable(list);
		Dictionary<Lazy<ParameterInfo>, ParameterInfo> parametersTable = BuildParametersTable(parameters);
		lock (_lock)
		{
			if (_membersTable == null)
			{
				_membersTable = membersTable;
				_parametersTable = parametersTable;
				Thread.MemoryBarrier();
				_parameters = null;
				_members = null;
			}
		}
	}

	private Dictionary<LazyMemberInfo, MemberInfo[]> BuildMembersTable(List<LazyMemberInfo> members)
	{
		Assumes.NotNull(members);
		Dictionary<LazyMemberInfo, MemberInfo[]> dictionary = new Dictionary<LazyMemberInfo, MemberInfo[]>();
		Dictionary<int, MemberInfo> dictionary2 = new Dictionary<int, MemberInfo>();
		Type partType = GetPartType();
		dictionary2[partType.MetadataToken] = partType;
		foreach (MethodInfo allMethod in partType.GetAllMethods())
		{
			dictionary2[allMethod.MetadataToken] = allMethod;
		}
		foreach (FieldInfo allField in partType.GetAllFields())
		{
			dictionary2[allField.MetadataToken] = allField;
		}
		foreach (LazyMemberInfo member in members)
		{
			MemberInfo[] accessors = member.GetAccessors();
			MemberInfo[] array = new MemberInfo[accessors.Length];
			for (int i = 0; i < accessors.Length; i++)
			{
				array[i] = ((accessors[i] != null) ? dictionary2[accessors[i].MetadataToken] : null);
			}
			dictionary[member] = array;
		}
		return dictionary;
	}

	private Dictionary<Lazy<ParameterInfo>, ParameterInfo> BuildParametersTable(List<Lazy<ParameterInfo>> parameters)
	{
		if (parameters != null)
		{
			Dictionary<Lazy<ParameterInfo>, ParameterInfo> dictionary = new Dictionary<Lazy<ParameterInfo>, ParameterInfo>();
			ParameterInfo[] parameters2 = GetConstructor().GetParameters();
			{
				foreach (Lazy<ParameterInfo> parameter in parameters)
				{
					dictionary[parameter] = parameters2[parameter.Value.Position];
				}
				return dictionary;
			}
		}
		return null;
	}

	private List<ImportDefinition> PopulateImports(List<LazyMemberInfo> members, List<Lazy<ParameterInfo>> parameters)
	{
		List<ImportDefinition> list = new List<ImportDefinition>();
		foreach (ImportDefinition import in _originalPartCreationInfo.GetImports())
		{
			if (import is ReflectionImportDefinition reflectionImport)
			{
				list.Add(TranslateImport(reflectionImport, members, parameters));
			}
		}
		return list;
	}

	private ImportDefinition TranslateImport(ReflectionImportDefinition reflectionImport, List<LazyMemberInfo> members, List<Lazy<ParameterInfo>> parameters)
	{
		bool flag = false;
		ContractBasedImportDefinition contractBasedImportDefinition = reflectionImport;
		if (reflectionImport is IPartCreatorImportDefinition partCreatorImportDefinition)
		{
			contractBasedImportDefinition = partCreatorImportDefinition.ProductImportDefinition;
			flag = true;
		}
		string contractName = Translate(contractBasedImportDefinition.ContractName);
		string requiredTypeIdentity = Translate(contractBasedImportDefinition.RequiredTypeIdentity);
		IDictionary<string, object> metadata = TranslateImportMetadata(contractBasedImportDefinition);
		ReflectionMemberImportDefinition reflectionMemberImportDefinition = reflectionImport as ReflectionMemberImportDefinition;
		ImportDefinition importDefinition = null;
		if (reflectionMemberImportDefinition != null)
		{
			LazyMemberInfo lazyMember = reflectionMemberImportDefinition.ImportingLazyMember;
			LazyMemberInfo importingLazyMember = new LazyMemberInfo(lazyMember.MemberType, () => GetAccessors(lazyMember));
			importDefinition = ((!flag) ? new ReflectionMemberImportDefinition(importingLazyMember, contractName, requiredTypeIdentity, contractBasedImportDefinition.RequiredMetadata, contractBasedImportDefinition.Cardinality, contractBasedImportDefinition.IsRecomposable, isPrerequisite: false, contractBasedImportDefinition.RequiredCreationPolicy, metadata, ((ICompositionElement)reflectionMemberImportDefinition).Origin) : new PartCreatorMemberImportDefinition(importingLazyMember, ((ICompositionElement)reflectionMemberImportDefinition).Origin, new ContractBasedImportDefinition(contractName, requiredTypeIdentity, contractBasedImportDefinition.RequiredMetadata, contractBasedImportDefinition.Cardinality, contractBasedImportDefinition.IsRecomposable, isPrerequisite: false, CreationPolicy.NonShared, metadata)));
			members.Add(lazyMember);
		}
		else
		{
			ReflectionParameterImportDefinition reflectionParameterImportDefinition = reflectionImport as ReflectionParameterImportDefinition;
			Assumes.NotNull(reflectionParameterImportDefinition);
			Lazy<ParameterInfo> lazyParameter = reflectionParameterImportDefinition.ImportingLazyParameter;
			Lazy<ParameterInfo> importingLazyParameter = new Lazy<ParameterInfo>(() => GetParameter(lazyParameter));
			importDefinition = ((!flag) ? new ReflectionParameterImportDefinition(importingLazyParameter, contractName, requiredTypeIdentity, contractBasedImportDefinition.RequiredMetadata, contractBasedImportDefinition.Cardinality, contractBasedImportDefinition.RequiredCreationPolicy, metadata, ((ICompositionElement)reflectionParameterImportDefinition).Origin) : new PartCreatorParameterImportDefinition(importingLazyParameter, ((ICompositionElement)reflectionParameterImportDefinition).Origin, new ContractBasedImportDefinition(contractName, requiredTypeIdentity, contractBasedImportDefinition.RequiredMetadata, contractBasedImportDefinition.Cardinality, isRecomposable: false, isPrerequisite: true, CreationPolicy.NonShared, metadata)));
			parameters.Add(lazyParameter);
		}
		return importDefinition;
	}

	private List<ExportDefinition> PopulateExports(List<LazyMemberInfo> members)
	{
		List<ExportDefinition> list = new List<ExportDefinition>();
		foreach (ExportDefinition export in _originalPartCreationInfo.GetExports())
		{
			if (export is ReflectionMemberExportDefinition reflectionExport)
			{
				list.Add(TranslateExpot(reflectionExport, members));
			}
		}
		return list;
	}

	public ExportDefinition TranslateExpot(ReflectionMemberExportDefinition reflectionExport, List<LazyMemberInfo> members)
	{
		LazyMemberInfo exportingLazyMember = reflectionExport.ExportingLazyMember;
		LazyMemberInfo capturedLazyMember = exportingLazyMember;
		ReflectionMemberExportDefinition capturedReflectionExport = reflectionExport;
		string contractName = Translate(reflectionExport.ContractName, reflectionExport.Metadata.GetValue<int[]>("System.ComponentModel.Composition.GenericExportParametersOrderMetadataName"));
		LazyMemberInfo member = new LazyMemberInfo(capturedLazyMember.MemberType, () => GetAccessors(capturedLazyMember));
		Lazy<IDictionary<string, object>> metadata = new Lazy<IDictionary<string, object>>(() => TranslateExportMetadata(capturedReflectionExport));
		ReflectionMemberExportDefinition result = new ReflectionMemberExportDefinition(member, new LazyExportDefinition(contractName, metadata), ((ICompositionElement)reflectionExport).Origin);
		members.Add(capturedLazyMember);
		return result;
	}

	private string Translate(string originalValue, int[] genericParametersOrder)
	{
		if (genericParametersOrder != null)
		{
			string[] array = GenericServices.Reorder(_specializationIdentities, genericParametersOrder);
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			object[] args = array;
			return string.Format(invariantCulture, originalValue, args);
		}
		return Translate(originalValue);
	}

	private string Translate(string originalValue)
	{
		CultureInfo invariantCulture = CultureInfo.InvariantCulture;
		object[] specializationIdentities = _specializationIdentities;
		return string.Format(invariantCulture, originalValue, specializationIdentities);
	}

	private IDictionary<string, object> TranslateImportMetadata(ContractBasedImportDefinition originalImport)
	{
		int[] value = originalImport.Metadata.GetValue<int[]>("System.ComponentModel.Composition.GenericImportParametersOrderMetadataName");
		if (value != null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>(originalImport.Metadata, StringComparers.MetadataKeyNames);
			dictionary["System.ComponentModel.Composition.GenericContractName"] = GenericServices.GetGenericName(originalImport.ContractName, value, _specialization.Length);
			dictionary["System.ComponentModel.Composition.GenericParameters"] = GenericServices.Reorder(_specialization, value);
			dictionary.Remove("System.ComponentModel.Composition.GenericImportParametersOrderMetadataName");
			return dictionary.AsReadOnly();
		}
		return originalImport.Metadata;
	}

	private IDictionary<string, object> TranslateExportMetadata(ReflectionMemberExportDefinition originalExport)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>(originalExport.Metadata, StringComparers.MetadataKeyNames);
		string value = originalExport.Metadata.GetValue<string>("ExportTypeIdentity");
		if (!string.IsNullOrEmpty(value))
		{
			dictionary["ExportTypeIdentity"] = Translate(value, originalExport.Metadata.GetValue<int[]>("System.ComponentModel.Composition.GenericExportParametersOrderMetadataName"));
		}
		dictionary.Remove("System.ComponentModel.Composition.GenericExportParametersOrderMetadataName");
		return dictionary;
	}

	private void PopulateImportsAndExports()
	{
		if (_exports != null && _imports != null)
		{
			return;
		}
		List<LazyMemberInfo> members = new List<LazyMemberInfo>();
		List<Lazy<ParameterInfo>> list = new List<Lazy<ParameterInfo>>();
		List<ExportDefinition> exports = PopulateExports(members);
		List<ImportDefinition> imports = PopulateImports(members, list);
		Thread.MemoryBarrier();
		lock (_lock)
		{
			if (_exports == null || _imports == null)
			{
				_members = members;
				if (list.Count > 0)
				{
					_parameters = list;
				}
				_exports = exports;
				_imports = imports;
			}
		}
	}

	public IEnumerable<ExportDefinition> GetExports()
	{
		PopulateImportsAndExports();
		return _exports;
	}

	public IEnumerable<ImportDefinition> GetImports()
	{
		PopulateImportsAndExports();
		return _imports;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is GenericSpecializationPartCreationInfo genericSpecializationPartCreationInfo))
		{
			return false;
		}
		if (_originalPartCreationInfo.Equals(genericSpecializationPartCreationInfo._originalPartCreationInfo))
		{
			return _specialization.IsArrayEqual(genericSpecializationPartCreationInfo._specialization);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return _originalPartCreationInfo.GetHashCode();
	}

	public static bool CanSpecialize(IDictionary<string, object> partMetadata, Type[] specialization)
	{
		int value = partMetadata.GetValue<int>("System.ComponentModel.Composition.GenericPartArity");
		if (value != specialization.Length)
		{
			return false;
		}
		object[] value2 = partMetadata.GetValue<object[]>("System.ComponentModel.Composition.GenericParameterConstraints");
		GenericParameterAttributes[] value3 = partMetadata.GetValue<GenericParameterAttributes[]>("System.ComponentModel.Composition.GenericParameterAttributes");
		if (value2 == null && value3 == null)
		{
			return true;
		}
		if (value2 != null && value2.Length != value)
		{
			return false;
		}
		if (value3 != null && value3.Length != value)
		{
			return false;
		}
		for (int i = 0; i < value; i++)
		{
			if (!GenericServices.CanSpecialize(specialization[i], (value2[i] as Type[]).CreateTypeSpecializations(specialization), value3[i]))
			{
				return false;
			}
		}
		return true;
	}
}
