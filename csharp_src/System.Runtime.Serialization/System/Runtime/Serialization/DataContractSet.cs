using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization;

internal class DataContractSet
{
	private Dictionary<XmlQualifiedName, DataContract> contracts;

	private Dictionary<DataContract, object> processedContracts;

	private IDataContractSurrogate dataContractSurrogate;

	private Hashtable surrogateDataTable;

	private Dictionary<XmlQualifiedName, DataContract> knownTypesForObject;

	private ICollection<Type> referencedTypes;

	private ICollection<Type> referencedCollectionTypes;

	private Dictionary<XmlQualifiedName, object> referencedTypesDictionary;

	private Dictionary<XmlQualifiedName, object> referencedCollectionTypesDictionary;

	private Dictionary<XmlQualifiedName, DataContract> Contracts
	{
		get
		{
			if (contracts == null)
			{
				contracts = new Dictionary<XmlQualifiedName, DataContract>();
			}
			return contracts;
		}
	}

	private Dictionary<DataContract, object> ProcessedContracts
	{
		get
		{
			if (processedContracts == null)
			{
				processedContracts = new Dictionary<DataContract, object>();
			}
			return processedContracts;
		}
	}

	private Hashtable SurrogateDataTable
	{
		get
		{
			if (surrogateDataTable == null)
			{
				surrogateDataTable = new Hashtable();
			}
			return surrogateDataTable;
		}
	}

	internal Dictionary<XmlQualifiedName, DataContract> KnownTypesForObject
	{
		get
		{
			return knownTypesForObject;
		}
		set
		{
			knownTypesForObject = value;
		}
	}

	public DataContract this[XmlQualifiedName key]
	{
		get
		{
			DataContract value = DataContract.GetBuiltInDataContract(key.Name, key.Namespace);
			if (value == null)
			{
				Contracts.TryGetValue(key, out value);
			}
			return value;
		}
	}

	public IDataContractSurrogate DataContractSurrogate => dataContractSurrogate;

	internal DataContractSet(IDataContractSurrogate dataContractSurrogate)
		: this(dataContractSurrogate, null, null)
	{
	}

	internal DataContractSet(IDataContractSurrogate dataContractSurrogate, ICollection<Type> referencedTypes, ICollection<Type> referencedCollectionTypes)
	{
		this.dataContractSurrogate = dataContractSurrogate;
		this.referencedTypes = referencedTypes;
		this.referencedCollectionTypes = referencedCollectionTypes;
	}

	internal DataContractSet(DataContractSet dataContractSet)
	{
		if (dataContractSet == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("dataContractSet"));
		}
		dataContractSurrogate = dataContractSet.dataContractSurrogate;
		referencedTypes = dataContractSet.referencedTypes;
		referencedCollectionTypes = dataContractSet.referencedCollectionTypes;
		foreach (KeyValuePair<XmlQualifiedName, DataContract> item in dataContractSet)
		{
			Add(item.Key, item.Value);
		}
		if (dataContractSet.processedContracts == null)
		{
			return;
		}
		foreach (KeyValuePair<DataContract, object> processedContract in dataContractSet.processedContracts)
		{
			ProcessedContracts.Add(processedContract.Key, processedContract.Value);
		}
	}

	internal void Add(Type type)
	{
		DataContract dataContract = GetDataContract(type);
		EnsureTypeNotGeneric(dataContract.UnderlyingType);
		Add(dataContract);
	}

	internal static void EnsureTypeNotGeneric(Type type)
	{
		if (type.ContainsGenericParameters)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Generic type '{0}' is not exportable.", type)));
		}
	}

	private void Add(DataContract dataContract)
	{
		Add(dataContract.StableName, dataContract);
	}

	public void Add(XmlQualifiedName name, DataContract dataContract)
	{
		if (!dataContract.IsBuiltInDataContract)
		{
			InternalAdd(name, dataContract);
		}
	}

	internal void InternalAdd(XmlQualifiedName name, DataContract dataContract)
	{
		DataContract value = null;
		if (Contracts.TryGetValue(name, out value))
		{
			if (!value.Equals(dataContract))
			{
				if (dataContract.UnderlyingType == null || value.UnderlyingType == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Duplicate contract in data contract set was found, for '{0}' in '{1}' namespace.", dataContract.StableName.Name, dataContract.StableName.Namespace)));
				}
				bool flag = DataContract.GetClrTypeFullName(dataContract.UnderlyingType) == DataContract.GetClrTypeFullName(value.UnderlyingType);
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Duplicate type contract in data contract set. Type name '{0}', for data contract '{1}' in '{2}' namespace.", flag ? dataContract.UnderlyingType.AssemblyQualifiedName : DataContract.GetClrTypeFullName(dataContract.UnderlyingType), flag ? value.UnderlyingType.AssemblyQualifiedName : DataContract.GetClrTypeFullName(value.UnderlyingType), dataContract.StableName.Name, dataContract.StableName.Namespace)));
			}
		}
		else
		{
			Contracts.Add(name, dataContract);
			if (dataContract is ClassDataContract)
			{
				AddClassDataContract((ClassDataContract)dataContract);
			}
			else if (dataContract is CollectionDataContract)
			{
				AddCollectionDataContract((CollectionDataContract)dataContract);
			}
			else if (dataContract is XmlDataContract)
			{
				AddXmlDataContract((XmlDataContract)dataContract);
			}
		}
	}

	private void AddClassDataContract(ClassDataContract classDataContract)
	{
		if (classDataContract.BaseContract != null)
		{
			Add(classDataContract.BaseContract.StableName, classDataContract.BaseContract);
		}
		if (!classDataContract.IsISerializable && classDataContract.Members != null)
		{
			for (int i = 0; i < classDataContract.Members.Count; i++)
			{
				DataMember dataMember = classDataContract.Members[i];
				DataContract memberTypeDataContract = GetMemberTypeDataContract(dataMember);
				if (dataContractSurrogate != null && dataMember.MemberInfo != null)
				{
					object customDataToExport = DataContractSurrogateCaller.GetCustomDataToExport(dataContractSurrogate, dataMember.MemberInfo, memberTypeDataContract.UnderlyingType);
					if (customDataToExport != null)
					{
						SurrogateDataTable.Add(dataMember, customDataToExport);
					}
				}
				Add(memberTypeDataContract.StableName, memberTypeDataContract);
			}
		}
		AddKnownDataContracts(classDataContract.KnownDataContracts);
	}

	private void AddCollectionDataContract(CollectionDataContract collectionDataContract)
	{
		if (collectionDataContract.IsDictionary)
		{
			ClassDataContract classDataContract = collectionDataContract.ItemContract as ClassDataContract;
			AddClassDataContract(classDataContract);
		}
		else
		{
			DataContract itemTypeDataContract = GetItemTypeDataContract(collectionDataContract);
			if (itemTypeDataContract != null)
			{
				Add(itemTypeDataContract.StableName, itemTypeDataContract);
			}
		}
		AddKnownDataContracts(collectionDataContract.KnownDataContracts);
	}

	private void AddXmlDataContract(XmlDataContract xmlDataContract)
	{
		AddKnownDataContracts(xmlDataContract.KnownDataContracts);
	}

	private void AddKnownDataContracts(Dictionary<XmlQualifiedName, DataContract> knownDataContracts)
	{
		if (knownDataContracts == null)
		{
			return;
		}
		foreach (DataContract value in knownDataContracts.Values)
		{
			Add(value);
		}
	}

	internal XmlQualifiedName GetStableName(Type clrType)
	{
		if (dataContractSurrogate != null)
		{
			return DataContract.GetStableName(DataContractSurrogateCaller.GetDataContractType(dataContractSurrogate, clrType));
		}
		return DataContract.GetStableName(clrType);
	}

	internal DataContract GetDataContract(Type clrType)
	{
		if (dataContractSurrogate == null)
		{
			return DataContract.GetDataContract(clrType);
		}
		DataContract builtInDataContract = DataContract.GetBuiltInDataContract(clrType);
		if (builtInDataContract != null)
		{
			return builtInDataContract;
		}
		Type dataContractType = DataContractSurrogateCaller.GetDataContractType(dataContractSurrogate, clrType);
		builtInDataContract = DataContract.GetDataContract(dataContractType);
		if (!SurrogateDataTable.Contains(builtInDataContract))
		{
			object customDataToExport = DataContractSurrogateCaller.GetCustomDataToExport(dataContractSurrogate, clrType, dataContractType);
			if (customDataToExport != null)
			{
				SurrogateDataTable.Add(builtInDataContract, customDataToExport);
			}
		}
		return builtInDataContract;
	}

	internal DataContract GetMemberTypeDataContract(DataMember dataMember)
	{
		if (dataMember.MemberInfo != null)
		{
			Type memberType = dataMember.MemberType;
			if (dataMember.IsGetOnlyCollection)
			{
				if (dataContractSurrogate != null && DataContractSurrogateCaller.GetDataContractType(dataContractSurrogate, memberType) != memberType)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Surrogates with get-only collections are not supported. Type '{1}' contains '{2}' which is of '{0}' type.", DataContract.GetClrTypeFullName(memberType), DataContract.GetClrTypeFullName(dataMember.MemberInfo.DeclaringType), dataMember.MemberInfo.Name)));
				}
				return DataContract.GetGetOnlyCollectionDataContract(DataContract.GetId(memberType.TypeHandle), memberType.TypeHandle, memberType, SerializationMode.SharedContract);
			}
			return GetDataContract(memberType);
		}
		return dataMember.MemberTypeContract;
	}

	internal DataContract GetItemTypeDataContract(CollectionDataContract collectionContract)
	{
		if (collectionContract.ItemType != null)
		{
			return GetDataContract(collectionContract.ItemType);
		}
		return collectionContract.ItemContract;
	}

	internal object GetSurrogateData(object key)
	{
		return SurrogateDataTable[key];
	}

	internal void SetSurrogateData(object key, object surrogateData)
	{
		SurrogateDataTable[key] = surrogateData;
	}

	public bool Remove(XmlQualifiedName key)
	{
		if (DataContract.GetBuiltInDataContract(key.Name, key.Namespace) != null)
		{
			return false;
		}
		return Contracts.Remove(key);
	}

	public IEnumerator<KeyValuePair<XmlQualifiedName, DataContract>> GetEnumerator()
	{
		return Contracts.GetEnumerator();
	}

	internal bool IsContractProcessed(DataContract dataContract)
	{
		return ProcessedContracts.ContainsKey(dataContract);
	}

	internal void SetContractProcessed(DataContract dataContract)
	{
		ProcessedContracts.Add(dataContract, dataContract);
	}

	private Dictionary<XmlQualifiedName, object> GetReferencedTypes()
	{
		if (referencedTypesDictionary == null)
		{
			referencedTypesDictionary = new Dictionary<XmlQualifiedName, object>();
			referencedTypesDictionary.Add(DataContract.GetStableName(Globals.TypeOfNullable), Globals.TypeOfNullable);
			if (referencedTypes != null)
			{
				foreach (Type referencedType in referencedTypes)
				{
					if (referencedType == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Referenced types cannot contain null.")));
					}
					AddReferencedType(referencedTypesDictionary, referencedType);
				}
			}
		}
		return referencedTypesDictionary;
	}

	private Dictionary<XmlQualifiedName, object> GetReferencedCollectionTypes()
	{
		if (referencedCollectionTypesDictionary == null)
		{
			referencedCollectionTypesDictionary = new Dictionary<XmlQualifiedName, object>();
			if (referencedCollectionTypes != null)
			{
				foreach (Type referencedCollectionType in referencedCollectionTypes)
				{
					if (referencedCollectionType == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Referenced collection types cannot contain null.")));
					}
					AddReferencedType(referencedCollectionTypesDictionary, referencedCollectionType);
				}
			}
			XmlQualifiedName stableName = DataContract.GetStableName(Globals.TypeOfDictionaryGeneric);
			if (!referencedCollectionTypesDictionary.ContainsKey(stableName) && GetReferencedTypes().ContainsKey(stableName))
			{
				AddReferencedType(referencedCollectionTypesDictionary, Globals.TypeOfDictionaryGeneric);
			}
		}
		return referencedCollectionTypesDictionary;
	}

	private void AddReferencedType(Dictionary<XmlQualifiedName, object> referencedTypes, Type type)
	{
		if (!IsTypeReferenceable(type))
		{
			return;
		}
		XmlQualifiedName stableName;
		try
		{
			stableName = GetStableName(type);
		}
		catch (InvalidDataContractException)
		{
			return;
		}
		catch (InvalidOperationException)
		{
			return;
		}
		if (referencedTypes.TryGetValue(stableName, out var value))
		{
			Type type2 = value as Type;
			if (type2 != null)
			{
				if (type2 != type)
				{
					referencedTypes.Remove(stableName);
					List<Type> list = new List<Type>();
					list.Add(type2);
					list.Add(type);
					referencedTypes.Add(stableName, list);
				}
			}
			else
			{
				List<Type> list2 = (List<Type>)value;
				if (!list2.Contains(type))
				{
					list2.Add(type);
				}
			}
		}
		else
		{
			referencedTypes.Add(stableName, type);
		}
	}

	internal bool TryGetReferencedType(XmlQualifiedName stableName, DataContract dataContract, out Type type)
	{
		return TryGetReferencedType(stableName, dataContract, useReferencedCollectionTypes: false, out type);
	}

	internal bool TryGetReferencedCollectionType(XmlQualifiedName stableName, DataContract dataContract, out Type type)
	{
		return TryGetReferencedType(stableName, dataContract, useReferencedCollectionTypes: true, out type);
	}

	private bool TryGetReferencedType(XmlQualifiedName stableName, DataContract dataContract, bool useReferencedCollectionTypes, out Type type)
	{
		if ((useReferencedCollectionTypes ? GetReferencedCollectionTypes() : GetReferencedTypes()).TryGetValue(stableName, out var value))
		{
			type = value as Type;
			if (type != null)
			{
				return true;
			}
			List<Type> list = (List<Type>)value;
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			for (int i = 0; i < list.Count; i++)
			{
				Type type2 = list[i];
				if (!flag)
				{
					flag = type2.IsGenericTypeDefinition;
				}
				stringBuilder.AppendFormat("{0}\"{1}\" ", Environment.NewLine, type2.AssemblyQualifiedName);
				if (dataContract != null)
				{
					DataContract dataContract2 = GetDataContract(type2);
					stringBuilder.Append(SR.GetString((dataContract2 != null && dataContract2.Equals(dataContract)) ? "Reference type matches." : "Reference type does not match."));
				}
			}
			if (flag)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString(useReferencedCollectionTypes ? "Ambiguous collection types were referenced: {0}" : "Ambiguous types were referenced: {0}", stringBuilder.ToString())));
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString(useReferencedCollectionTypes ? "In '{0}' element in '{1}' namespace, ambiguous collection types were referenced: {2}" : "In '{0}' element in '{1}' namespace, ambiguous types were referenced: {2}", XmlConvert.DecodeName(stableName.Name), stableName.Namespace, stringBuilder.ToString())));
		}
		type = null;
		return false;
	}

	private static bool IsTypeReferenceable(Type type)
	{
		try
		{
			Type itemType;
			return type.IsSerializable || type.IsDefined(Globals.TypeOfDataContractAttribute, inherit: false) || (Globals.TypeOfIXmlSerializable.IsAssignableFrom(type) && !type.IsGenericTypeDefinition) || CollectionDataContract.IsCollection(type, out itemType) || ClassDataContract.IsNonAttributedTypeValidForSerialization(type);
		}
		catch (Exception exception)
		{
			if (Fx.IsFatal(exception))
			{
				throw;
			}
		}
		return false;
	}
}
