using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace System.Runtime.Serialization.Json;

internal class XmlObjectSerializerWriteContextComplexJson : XmlObjectSerializerWriteContextComplex
{
	private EmitTypeInformation emitXsiType;

	private bool perCallXsiTypeAlreadyEmitted;

	private bool useSimpleDictionaryFormat;

	internal IList<Type> SerializerKnownTypeList => serializerKnownTypeList;

	public bool UseSimpleDictionaryFormat => useSimpleDictionaryFormat;

	internal XmlDictionaryString CollectionItemName => JsonGlobals.itemDictionaryString;

	public XmlObjectSerializerWriteContextComplexJson(DataContractJsonSerializer serializer, DataContract rootTypeDataContract)
		: base(serializer, serializer.MaxItemsInObjectGraph, new StreamingContext(StreamingContextStates.All), serializer.IgnoreExtensionDataObject)
	{
		emitXsiType = serializer.EmitTypeInformation;
		base.rootTypeDataContract = rootTypeDataContract;
		serializerKnownTypeList = serializer.knownTypeList;
		dataContractSurrogate = serializer.DataContractSurrogate;
		serializeReadOnlyTypes = serializer.SerializeReadOnlyTypes;
		useSimpleDictionaryFormat = serializer.UseSimpleDictionaryFormat;
	}

	internal static XmlObjectSerializerWriteContextComplexJson CreateContext(DataContractJsonSerializer serializer, DataContract rootTypeDataContract)
	{
		return new XmlObjectSerializerWriteContextComplexJson(serializer, rootTypeDataContract);
	}

	internal override bool WriteClrTypeInfo(XmlWriterDelegator xmlWriter, Type dataContractType, string clrTypeName, string clrAssemblyName)
	{
		return false;
	}

	internal override bool WriteClrTypeInfo(XmlWriterDelegator xmlWriter, DataContract dataContract)
	{
		return false;
	}

	internal override void WriteArraySize(XmlWriterDelegator xmlWriter, int size)
	{
	}

	protected override void WriteTypeInfo(XmlWriterDelegator writer, string dataContractName, string dataContractNamespace)
	{
		if (emitXsiType != EmitTypeInformation.Never)
		{
			if (string.IsNullOrEmpty(dataContractNamespace))
			{
				WriteTypeInfo(writer, dataContractName);
			}
			else
			{
				WriteTypeInfo(writer, dataContractName + ":" + TruncateDefaultDataContractNamespace(dataContractNamespace));
			}
		}
	}

	internal static string TruncateDefaultDataContractNamespace(string dataContractNamespace)
	{
		if (!string.IsNullOrEmpty(dataContractNamespace))
		{
			if (dataContractNamespace[0] == '#')
			{
				return "\\" + dataContractNamespace;
			}
			if (dataContractNamespace[0] == '\\')
			{
				return "\\" + dataContractNamespace;
			}
			if (dataContractNamespace.StartsWith("http://schemas.datacontract.org/2004/07/", StringComparison.Ordinal))
			{
				return "#" + dataContractNamespace.Substring(JsonGlobals.DataContractXsdBaseNamespaceLength);
			}
		}
		return dataContractNamespace;
	}

	private static bool RequiresJsonTypeInfo(DataContract contract)
	{
		return contract is ClassDataContract;
	}

	private void WriteTypeInfo(XmlWriterDelegator writer, string typeInformation)
	{
		writer.WriteAttributeString(null, "__type", null, typeInformation);
	}

	protected override bool WriteTypeInfo(XmlWriterDelegator writer, DataContract contract, DataContract declaredContract)
	{
		if ((contract.Name != declaredContract.Name || contract.Namespace != declaredContract.Namespace) && (!(contract.Name.Value == declaredContract.Name.Value) || !(contract.Namespace.Value == declaredContract.Namespace.Value)) && contract.UnderlyingType != Globals.TypeOfObjectArray && emitXsiType != EmitTypeInformation.Never)
		{
			if (RequiresJsonTypeInfo(contract))
			{
				perCallXsiTypeAlreadyEmitted = true;
				WriteTypeInfo(writer, contract.Name.Value, contract.Namespace.Value);
			}
			else if (declaredContract.UnderlyingType == typeof(Enum))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new SerializationException(SR.GetString("Enum type is not supported by DataContractJsonSerializer. The underlying type is '{0}'.", declaredContract.UnderlyingType)));
			}
			return true;
		}
		return false;
	}

	internal void WriteJsonISerializable(XmlWriterDelegator xmlWriter, ISerializable obj)
	{
		Type type = obj.GetType();
		SerializationInfo serializationInfo = new SerializationInfo(type, XmlObjectSerializer.FormatterConverter);
		GetObjectData(obj, serializationInfo, GetStreamingContext());
		if (DataContract.GetClrTypeFullName(type) != serializationInfo.FullTypeName)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Changing full type name is not supported. Serialization type name: '{0}', data contract type name: '{1}'.", serializationInfo.FullTypeName, DataContract.GetClrTypeFullName(type))));
		}
		WriteSerializationInfo(xmlWriter, type, serializationInfo);
	}

	internal static DataContract GetRevisedItemContract(DataContract oldItemContract)
	{
		if (oldItemContract != null && oldItemContract.UnderlyingType.IsGenericType && oldItemContract.UnderlyingType.GetGenericTypeDefinition() == Globals.TypeOfKeyValue)
		{
			return DataContract.GetDataContract(oldItemContract.UnderlyingType);
		}
		return oldItemContract;
	}

	protected override void WriteDataContractValue(DataContract dataContract, XmlWriterDelegator xmlWriter, object obj, RuntimeTypeHandle declaredTypeHandle)
	{
		JsonDataContract jsonDataContract = JsonDataContract.GetJsonDataContract(dataContract);
		if (emitXsiType == EmitTypeInformation.Always && !perCallXsiTypeAlreadyEmitted && RequiresJsonTypeInfo(dataContract))
		{
			WriteTypeInfo(xmlWriter, jsonDataContract.TypeName);
		}
		perCallXsiTypeAlreadyEmitted = false;
		DataContractJsonSerializer.WriteJsonValue(jsonDataContract, xmlWriter, obj, this, declaredTypeHandle);
	}

	protected override void WriteNull(XmlWriterDelegator xmlWriter)
	{
		DataContractJsonSerializer.WriteJsonNull(xmlWriter);
	}

	internal static void WriteJsonNameWithMapping(XmlWriterDelegator xmlWriter, XmlDictionaryString[] memberNames, int index)
	{
		xmlWriter.WriteStartElement("a", "item", "item");
		xmlWriter.WriteAttributeString(null, "item", null, memberNames[index].Value);
	}

	internal override void WriteExtensionDataTypeInfo(XmlWriterDelegator xmlWriter, IDataNode dataNode)
	{
		Type dataType = dataNode.DataType;
		if (dataType == Globals.TypeOfClassDataNode || dataType == Globals.TypeOfISerializableDataNode)
		{
			xmlWriter.WriteAttributeString(null, "type", null, "object");
			base.WriteExtensionDataTypeInfo(xmlWriter, dataNode);
		}
		else if (dataType == Globals.TypeOfCollectionDataNode)
		{
			xmlWriter.WriteAttributeString(null, "type", null, "array");
		}
		else if (!(dataType == Globals.TypeOfXmlDataNode) && dataType == Globals.TypeOfObject && dataNode.Value != null && RequiresJsonTypeInfo(GetDataContract(dataNode.Value.GetType())))
		{
			base.WriteExtensionDataTypeInfo(xmlWriter, dataNode);
		}
	}

	protected override void SerializeWithXsiType(XmlWriterDelegator xmlWriter, object obj, RuntimeTypeHandle objectTypeHandle, Type objectType, int declaredTypeID, RuntimeTypeHandle declaredTypeHandle, Type declaredType)
	{
		bool verifyKnownType = false;
		bool isInterface = declaredType.IsInterface;
		DataContract dataContract;
		if (isInterface && CollectionDataContract.IsCollectionInterface(declaredType))
		{
			dataContract = GetDataContract(declaredTypeHandle, declaredType);
		}
		else if (declaredType.IsArray)
		{
			dataContract = GetDataContract(declaredTypeHandle, declaredType);
		}
		else
		{
			dataContract = GetDataContract(objectTypeHandle, objectType);
			verifyKnownType = WriteTypeInfo(declaredContract: (declaredTypeID >= 0) ? GetDataContract(declaredTypeID, declaredTypeHandle) : GetDataContract(declaredTypeHandle, declaredType), writer: xmlWriter, contract: dataContract);
			HandleCollectionAssignedToObject(declaredType, ref dataContract, ref obj, ref verifyKnownType);
		}
		if (isInterface)
		{
			VerifyObjectCompatibilityWithInterface(dataContract, obj, declaredType);
		}
		SerializeAndVerifyType(dataContract, xmlWriter, obj, verifyKnownType, declaredType.TypeHandle, declaredType);
	}

	private static void VerifyObjectCompatibilityWithInterface(DataContract contract, object graph, Type declaredType)
	{
		Type type = contract.GetType();
		if (type == typeof(XmlDataContract) && !Globals.TypeOfIXmlSerializable.IsAssignableFrom(declaredType))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Object of type '{0}' is assigned to an incompatible interface '{1}'.", graph.GetType(), declaredType)));
		}
		if (type == typeof(CollectionDataContract) && !CollectionDataContract.IsCollectionInterface(declaredType))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Collection of type '{0}' is assigned to an incompatible interface '{1}'", graph.GetType(), declaredType)));
		}
	}

	private void HandleCollectionAssignedToObject(Type declaredType, ref DataContract dataContract, ref object obj, ref bool verifyKnownType)
	{
		if (!(declaredType != dataContract.UnderlyingType) || !(dataContract is CollectionDataContract))
		{
			return;
		}
		if (verifyKnownType)
		{
			VerifyType(dataContract, declaredType);
			verifyKnownType = false;
		}
		if (((CollectionDataContract)dataContract).Kind == CollectionKind.Dictionary)
		{
			IDictionary obj2 = obj as IDictionary;
			Dictionary<object, object> dictionary = new Dictionary<object, object>();
			foreach (DictionaryEntry item in obj2)
			{
				dictionary.Add(item.Key, item.Value);
			}
			obj = dictionary;
		}
		dataContract = GetDataContract(Globals.TypeOfIEnumerable);
	}

	internal override void SerializeWithXsiTypeAtTopLevel(DataContract dataContract, XmlWriterDelegator xmlWriter, object obj, RuntimeTypeHandle originalDeclaredTypeHandle, Type graphType)
	{
		bool verifyKnownType = false;
		Type underlyingType = rootTypeDataContract.UnderlyingType;
		bool isInterface = underlyingType.IsInterface;
		if ((!isInterface || !CollectionDataContract.IsCollectionInterface(underlyingType)) && !underlyingType.IsArray)
		{
			verifyKnownType = WriteTypeInfo(xmlWriter, dataContract, rootTypeDataContract);
			HandleCollectionAssignedToObject(underlyingType, ref dataContract, ref obj, ref verifyKnownType);
		}
		if (isInterface)
		{
			VerifyObjectCompatibilityWithInterface(dataContract, obj, underlyingType);
		}
		SerializeAndVerifyType(dataContract, xmlWriter, obj, verifyKnownType, underlyingType.TypeHandle, underlyingType);
	}

	private void VerifyType(DataContract dataContract, Type declaredType)
	{
		bool flag = false;
		if (dataContract.KnownDataContracts != null)
		{
			scopedKnownTypes.Push(dataContract.KnownDataContracts);
			flag = true;
		}
		if (!IsKnownType(dataContract, declaredType))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Type '{0}' with data contract name '{1}:{2}' is not expected. Add any types not known statically to the list of known types - for example, by using the KnownTypeAttribute attribute or by adding them to the list of known types passed to DataContractSerializer.", DataContract.GetClrTypeFullName(dataContract.UnderlyingType), dataContract.StableName.Name, dataContract.StableName.Namespace)));
		}
		if (flag)
		{
			scopedKnownTypes.Pop();
		}
	}

	internal override DataContract GetDataContract(RuntimeTypeHandle typeHandle, Type type)
	{
		DataContract dataContract = base.GetDataContract(typeHandle, type);
		DataContractJsonSerializer.CheckIfTypeIsReference(dataContract);
		return dataContract;
	}

	internal override DataContract GetDataContractSkipValidation(int typeId, RuntimeTypeHandle typeHandle, Type type)
	{
		DataContract dataContractSkipValidation = base.GetDataContractSkipValidation(typeId, typeHandle, type);
		DataContractJsonSerializer.CheckIfTypeIsReference(dataContractSkipValidation);
		return dataContractSkipValidation;
	}

	internal override DataContract GetDataContract(int id, RuntimeTypeHandle typeHandle)
	{
		DataContract dataContract = base.GetDataContract(id, typeHandle);
		DataContractJsonSerializer.CheckIfTypeIsReference(dataContract);
		return dataContract;
	}

	internal static DataContract ResolveJsonDataContractFromRootDataContract(XmlObjectSerializerContext context, XmlQualifiedName typeQName, DataContract rootTypeDataContract)
	{
		if (rootTypeDataContract.StableName == typeQName)
		{
			return rootTypeDataContract;
		}
		CollectionDataContract collectionDataContract = rootTypeDataContract as CollectionDataContract;
		while (collectionDataContract != null)
		{
			DataContract dataContract = ((!collectionDataContract.ItemType.IsGenericType || !(collectionDataContract.ItemType.GetGenericTypeDefinition() == typeof(KeyValue<, >))) ? context.GetDataContract(context.GetSurrogatedType(collectionDataContract.ItemType)) : context.GetDataContract(Globals.TypeOfKeyValuePair.MakeGenericType(collectionDataContract.ItemType.GetGenericArguments())));
			if (dataContract.StableName == typeQName)
			{
				return dataContract;
			}
			collectionDataContract = dataContract as CollectionDataContract;
		}
		return null;
	}

	protected override DataContract ResolveDataContractFromRootDataContract(XmlQualifiedName typeQName)
	{
		return ResolveJsonDataContractFromRootDataContract(this, typeQName, rootTypeDataContract);
	}
}
