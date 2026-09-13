using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Diagnostics;
using System.Security;
using System.Xml;
using System.Xml.Serialization;

namespace System.Runtime.Serialization;

internal class XmlObjectSerializerWriteContext : XmlObjectSerializerContext
{
	private ObjectReferenceStack byValObjectsInScope;

	private XmlSerializableWriter xmlSerializableWriter;

	private const int depthToCheckCyclicReference = 512;

	protected bool preserveObjectReferences;

	private ObjectToIdCache serializedObjects;

	private bool isGetOnlyCollection;

	private readonly bool unsafeTypeForwardingEnabled;

	protected bool serializeReadOnlyTypes;

	protected ObjectToIdCache SerializedObjects
	{
		get
		{
			if (serializedObjects == null)
			{
				serializedObjects = new ObjectToIdCache();
			}
			return serializedObjects;
		}
	}

	internal override bool IsGetOnlyCollection
	{
		get
		{
			return isGetOnlyCollection;
		}
		set
		{
			isGetOnlyCollection = value;
		}
	}

	internal bool SerializeReadOnlyTypes => serializeReadOnlyTypes;

	internal bool UnsafeTypeForwardingEnabled => unsafeTypeForwardingEnabled;

	internal static XmlObjectSerializerWriteContext CreateContext(DataContractSerializer serializer, DataContract rootTypeDataContract, DataContractResolver dataContractResolver)
	{
		if (!serializer.PreserveObjectReferences && serializer.DataContractSurrogate == null)
		{
			return new XmlObjectSerializerWriteContext(serializer, rootTypeDataContract, dataContractResolver);
		}
		return new XmlObjectSerializerWriteContextComplex(serializer, rootTypeDataContract, dataContractResolver);
	}

	internal static XmlObjectSerializerWriteContext CreateContext(NetDataContractSerializer serializer, Hashtable surrogateDataContracts)
	{
		return new XmlObjectSerializerWriteContextComplex(serializer, surrogateDataContracts);
	}

	protected XmlObjectSerializerWriteContext(DataContractSerializer serializer, DataContract rootTypeDataContract, DataContractResolver resolver)
		: base(serializer, rootTypeDataContract, resolver)
	{
		serializeReadOnlyTypes = serializer.SerializeReadOnlyTypes;
		unsafeTypeForwardingEnabled = true;
	}

	protected XmlObjectSerializerWriteContext(NetDataContractSerializer serializer)
		: base(serializer)
	{
		unsafeTypeForwardingEnabled = NetDataContractSerializer.UnsafeTypeForwardingEnabled;
	}

	internal XmlObjectSerializerWriteContext(XmlObjectSerializer serializer, int maxItemsInObjectGraph, StreamingContext streamingContext, bool ignoreExtensionDataObject)
		: base(serializer, maxItemsInObjectGraph, streamingContext, ignoreExtensionDataObject)
	{
		unsafeTypeForwardingEnabled = true;
	}

	internal void StoreIsGetOnlyCollection()
	{
		isGetOnlyCollection = true;
	}

	public void InternalSerializeReference(XmlWriterDelegator xmlWriter, object obj, bool isDeclaredType, bool writeXsiType, int declaredTypeID, RuntimeTypeHandle declaredTypeHandle)
	{
		if (!OnHandleReference(xmlWriter, obj, canContainCyclicReference: true))
		{
			InternalSerialize(xmlWriter, obj, isDeclaredType, writeXsiType, declaredTypeID, declaredTypeHandle);
		}
		OnEndHandleReference(xmlWriter, obj, canContainCyclicReference: true);
	}

	public virtual void InternalSerialize(XmlWriterDelegator xmlWriter, object obj, bool isDeclaredType, bool writeXsiType, int declaredTypeID, RuntimeTypeHandle declaredTypeHandle)
	{
		if (writeXsiType)
		{
			Type typeOfObject = Globals.TypeOfObject;
			SerializeWithXsiType(xmlWriter, obj, Type.GetTypeHandle(obj), null, -1, typeOfObject.TypeHandle, typeOfObject);
			return;
		}
		if (isDeclaredType)
		{
			DataContract dataContract = GetDataContract(declaredTypeID, declaredTypeHandle);
			SerializeWithoutXsiType(dataContract, xmlWriter, obj, declaredTypeHandle);
			return;
		}
		RuntimeTypeHandle typeHandle = Type.GetTypeHandle(obj);
		if (declaredTypeHandle.Equals(typeHandle))
		{
			DataContract dataContract2 = ((declaredTypeID >= 0) ? GetDataContract(declaredTypeID, declaredTypeHandle) : GetDataContract(declaredTypeHandle, null));
			SerializeWithoutXsiType(dataContract2, xmlWriter, obj, declaredTypeHandle);
		}
		else
		{
			SerializeWithXsiType(xmlWriter, obj, typeHandle, null, declaredTypeID, declaredTypeHandle, Type.GetTypeFromHandle(declaredTypeHandle));
		}
	}

	internal void SerializeWithoutXsiType(DataContract dataContract, XmlWriterDelegator xmlWriter, object obj, RuntimeTypeHandle declaredTypeHandle)
	{
		if (!OnHandleIsReference(xmlWriter, dataContract, obj))
		{
			if (dataContract.KnownDataContracts != null)
			{
				scopedKnownTypes.Push(dataContract.KnownDataContracts);
				WriteDataContractValue(dataContract, xmlWriter, obj, declaredTypeHandle);
				scopedKnownTypes.Pop();
			}
			else
			{
				WriteDataContractValue(dataContract, xmlWriter, obj, declaredTypeHandle);
			}
		}
	}

	internal virtual void SerializeWithXsiTypeAtTopLevel(DataContract dataContract, XmlWriterDelegator xmlWriter, object obj, RuntimeTypeHandle originalDeclaredTypeHandle, Type graphType)
	{
		bool verifyKnownType = false;
		Type originalUnderlyingType = rootTypeDataContract.OriginalUnderlyingType;
		if (originalUnderlyingType.IsInterface && CollectionDataContract.IsCollectionInterface(originalUnderlyingType))
		{
			if (base.DataContractResolver != null)
			{
				WriteResolvedTypeInfo(xmlWriter, graphType, originalUnderlyingType);
			}
		}
		else if (!originalUnderlyingType.IsArray)
		{
			verifyKnownType = WriteTypeInfo(xmlWriter, dataContract, rootTypeDataContract);
		}
		SerializeAndVerifyType(dataContract, xmlWriter, obj, verifyKnownType, originalDeclaredTypeHandle, originalUnderlyingType);
	}

	protected virtual void SerializeWithXsiType(XmlWriterDelegator xmlWriter, object obj, RuntimeTypeHandle objectTypeHandle, Type objectType, int declaredTypeID, RuntimeTypeHandle declaredTypeHandle, Type declaredType)
	{
		bool verifyKnownType = false;
		DataContract dataContractSkipValidation;
		if (declaredType.IsInterface && CollectionDataContract.IsCollectionInterface(declaredType))
		{
			dataContractSkipValidation = GetDataContractSkipValidation(DataContract.GetId(objectTypeHandle), objectTypeHandle, objectType);
			if (OnHandleIsReference(xmlWriter, dataContractSkipValidation, obj))
			{
				return;
			}
			dataContractSkipValidation = ((Mode != SerializationMode.SharedType || !dataContractSkipValidation.IsValidContract(Mode)) ? GetDataContract(declaredTypeHandle, declaredType) : dataContractSkipValidation.GetValidContract(Mode));
			if (!WriteClrTypeInfo(xmlWriter, dataContractSkipValidation) && base.DataContractResolver != null)
			{
				if (objectType == null)
				{
					objectType = Type.GetTypeFromHandle(objectTypeHandle);
				}
				WriteResolvedTypeInfo(xmlWriter, objectType, declaredType);
			}
		}
		else if (declaredType.IsArray)
		{
			dataContractSkipValidation = GetDataContract(objectTypeHandle, objectType);
			WriteClrTypeInfo(xmlWriter, dataContractSkipValidation);
			dataContractSkipValidation = GetDataContract(declaredTypeHandle, declaredType);
		}
		else
		{
			dataContractSkipValidation = GetDataContract(objectTypeHandle, objectType);
			if (OnHandleIsReference(xmlWriter, dataContractSkipValidation, obj))
			{
				return;
			}
			if (!WriteClrTypeInfo(xmlWriter, dataContractSkipValidation))
			{
				DataContract declaredContract = ((declaredTypeID >= 0) ? GetDataContract(declaredTypeID, declaredTypeHandle) : GetDataContract(declaredTypeHandle, declaredType));
				verifyKnownType = WriteTypeInfo(xmlWriter, dataContractSkipValidation, declaredContract);
			}
		}
		SerializeAndVerifyType(dataContractSkipValidation, xmlWriter, obj, verifyKnownType, declaredTypeHandle, declaredType);
	}

	internal bool OnHandleIsReference(XmlWriterDelegator xmlWriter, DataContract contract, object obj)
	{
		if (preserveObjectReferences || !contract.IsReference || isGetOnlyCollection)
		{
			return false;
		}
		bool newId = true;
		int id = SerializedObjects.GetId(obj, ref newId);
		byValObjectsInScope.EnsureSetAsIsReference(obj);
		if (newId)
		{
			xmlWriter.WriteAttributeString("z", DictionaryGlobals.IdLocalName, DictionaryGlobals.SerializationNamespace, string.Format(CultureInfo.InvariantCulture, "{0}{1}", "i", id));
			return false;
		}
		xmlWriter.WriteAttributeString("z", DictionaryGlobals.RefLocalName, DictionaryGlobals.SerializationNamespace, string.Format(CultureInfo.InvariantCulture, "{0}{1}", "i", id));
		return true;
	}

	protected void SerializeAndVerifyType(DataContract dataContract, XmlWriterDelegator xmlWriter, object obj, bool verifyKnownType, RuntimeTypeHandle declaredTypeHandle, Type declaredType)
	{
		bool flag = false;
		if (dataContract.KnownDataContracts != null)
		{
			scopedKnownTypes.Push(dataContract.KnownDataContracts);
			flag = true;
		}
		if (verifyKnownType && !IsKnownType(dataContract, declaredType))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Type '{0}' with data contract name '{1}:{2}' is not expected. Add any types not known statically to the list of known types - for example, by using the KnownTypeAttribute attribute or by adding them to the list of known types passed to DataContractSerializer.", DataContract.GetClrTypeFullName(dataContract.UnderlyingType), dataContract.StableName.Name, dataContract.StableName.Namespace)));
		}
		WriteDataContractValue(dataContract, xmlWriter, obj, declaredTypeHandle);
		if (flag)
		{
			scopedKnownTypes.Pop();
		}
	}

	internal virtual bool WriteClrTypeInfo(XmlWriterDelegator xmlWriter, DataContract dataContract)
	{
		return false;
	}

	internal virtual bool WriteClrTypeInfo(XmlWriterDelegator xmlWriter, Type dataContractType, string clrTypeName, string clrAssemblyName)
	{
		return false;
	}

	internal virtual bool WriteClrTypeInfo(XmlWriterDelegator xmlWriter, Type dataContractType, SerializationInfo serInfo)
	{
		return false;
	}

	public virtual void WriteAnyType(XmlWriterDelegator xmlWriter, object value)
	{
		xmlWriter.WriteAnyType(value);
	}

	public virtual void WriteString(XmlWriterDelegator xmlWriter, string value)
	{
		xmlWriter.WriteString(value);
	}

	public virtual void WriteString(XmlWriterDelegator xmlWriter, string value, XmlDictionaryString name, XmlDictionaryString ns)
	{
		if (value == null)
		{
			WriteNull(xmlWriter, typeof(string), isMemberTypeSerializable: true, name, ns);
			return;
		}
		xmlWriter.WriteStartElementPrimitive(name, ns);
		xmlWriter.WriteString(value);
		xmlWriter.WriteEndElementPrimitive();
	}

	public virtual void WriteBase64(XmlWriterDelegator xmlWriter, byte[] value)
	{
		xmlWriter.WriteBase64(value);
	}

	public virtual void WriteBase64(XmlWriterDelegator xmlWriter, byte[] value, XmlDictionaryString name, XmlDictionaryString ns)
	{
		if (value == null)
		{
			WriteNull(xmlWriter, typeof(byte[]), isMemberTypeSerializable: true, name, ns);
			return;
		}
		xmlWriter.WriteStartElementPrimitive(name, ns);
		xmlWriter.WriteBase64(value);
		xmlWriter.WriteEndElementPrimitive();
	}

	public virtual void WriteUri(XmlWriterDelegator xmlWriter, Uri value)
	{
		xmlWriter.WriteUri(value);
	}

	public virtual void WriteUri(XmlWriterDelegator xmlWriter, Uri value, XmlDictionaryString name, XmlDictionaryString ns)
	{
		if (value == null)
		{
			WriteNull(xmlWriter, typeof(Uri), isMemberTypeSerializable: true, name, ns);
			return;
		}
		xmlWriter.WriteStartElementPrimitive(name, ns);
		xmlWriter.WriteUri(value);
		xmlWriter.WriteEndElementPrimitive();
	}

	public virtual void WriteQName(XmlWriterDelegator xmlWriter, XmlQualifiedName value)
	{
		xmlWriter.WriteQName(value);
	}

	public virtual void WriteQName(XmlWriterDelegator xmlWriter, XmlQualifiedName value, XmlDictionaryString name, XmlDictionaryString ns)
	{
		if (value == null)
		{
			WriteNull(xmlWriter, typeof(XmlQualifiedName), isMemberTypeSerializable: true, name, ns);
			return;
		}
		if (ns != null && ns.Value != null && ns.Value.Length > 0)
		{
			xmlWriter.WriteStartElement("q", name, ns);
		}
		else
		{
			xmlWriter.WriteStartElement(name, ns);
		}
		xmlWriter.WriteQName(value);
		xmlWriter.WriteEndElement();
	}

	internal void HandleGraphAtTopLevel(XmlWriterDelegator writer, object obj, DataContract contract)
	{
		writer.WriteXmlnsAttribute("i", DictionaryGlobals.SchemaInstanceNamespace);
		if (contract.IsISerializable)
		{
			writer.WriteXmlnsAttribute("x", DictionaryGlobals.SchemaNamespace);
		}
		OnHandleReference(writer, obj, canContainCyclicReference: true);
	}

	internal virtual bool OnHandleReference(XmlWriterDelegator xmlWriter, object obj, bool canContainCyclicReference)
	{
		if (xmlWriter.depth < 512)
		{
			return false;
		}
		if (canContainCyclicReference)
		{
			if (byValObjectsInScope.Count == 0 && DiagnosticUtility.ShouldTraceWarning)
			{
				TraceUtility.Trace(TraceEventType.Warning, 196626, SR.GetString("Object with large depth"));
			}
			if (byValObjectsInScope.Contains(obj))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Object graph for type '{0}' contains cycles and cannot be serialized if references are not tracked. Consider using the DataContractAttribute with the IsReference property set to true.", DataContract.GetClrTypeFullName(obj.GetType()))));
			}
			byValObjectsInScope.Push(obj);
		}
		return false;
	}

	internal virtual void OnEndHandleReference(XmlWriterDelegator xmlWriter, object obj, bool canContainCyclicReference)
	{
		if (xmlWriter.depth >= 512 && canContainCyclicReference)
		{
			byValObjectsInScope.Pop(obj);
		}
	}

	public void WriteNull(XmlWriterDelegator xmlWriter, Type memberType, bool isMemberTypeSerializable)
	{
		CheckIfTypeSerializable(memberType, isMemberTypeSerializable);
		WriteNull(xmlWriter);
	}

	internal void WriteNull(XmlWriterDelegator xmlWriter, Type memberType, bool isMemberTypeSerializable, XmlDictionaryString name, XmlDictionaryString ns)
	{
		xmlWriter.WriteStartElement(name, ns);
		WriteNull(xmlWriter, memberType, isMemberTypeSerializable);
		xmlWriter.WriteEndElement();
	}

	public void IncrementArrayCount(XmlWriterDelegator xmlWriter, Array array)
	{
		IncrementCollectionCount(xmlWriter, array.GetLength(0));
	}

	public void IncrementCollectionCount(XmlWriterDelegator xmlWriter, ICollection collection)
	{
		IncrementCollectionCount(xmlWriter, collection.Count);
	}

	public void IncrementCollectionCountGeneric<T>(XmlWriterDelegator xmlWriter, ICollection<T> collection)
	{
		IncrementCollectionCount(xmlWriter, collection.Count);
	}

	private void IncrementCollectionCount(XmlWriterDelegator xmlWriter, int size)
	{
		IncrementItemCount(size);
		WriteArraySize(xmlWriter, size);
	}

	internal virtual void WriteArraySize(XmlWriterDelegator xmlWriter, int size)
	{
	}

	public static T GetDefaultValue<T>()
	{
		return default(T);
	}

	public static T GetNullableValue<T>(T? value) where T : struct
	{
		return value.Value;
	}

	public static void ThrowRequiredMemberMustBeEmitted(string memberName, Type type)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new SerializationException(SR.GetString("Member {0} in type {1} cannot be serialized. This exception is usually caused by trying to use a null value where a null value is not allowed. The '{0}' member is set to its default value (usually null or zero). The member's EmitDefault setting is 'false', indicating that the member should not be serialized. However, the member's IsRequired setting is 'true', indicating that it must be serialized. This conflict cannot be resolved.  Consider setting '{0}' to a non-default value. Alternatively, you can change the EmitDefaultValue property on the DataMemberAttribute attribute to true, or changing the IsRequired property to false.", memberName, type.FullName)));
	}

	public static bool GetHasValue<T>(T? value) where T : struct
	{
		return value.HasValue;
	}

	internal void WriteIXmlSerializable(XmlWriterDelegator xmlWriter, object obj)
	{
		if (xmlSerializableWriter == null)
		{
			xmlSerializableWriter = new XmlSerializableWriter();
		}
		WriteIXmlSerializable(xmlWriter, obj, xmlSerializableWriter);
	}

	internal static void WriteRootIXmlSerializable(XmlWriterDelegator xmlWriter, object obj)
	{
		WriteIXmlSerializable(xmlWriter, obj, new XmlSerializableWriter());
	}

	private static void WriteIXmlSerializable(XmlWriterDelegator xmlWriter, object obj, XmlSerializableWriter xmlSerializableWriter)
	{
		xmlSerializableWriter.BeginWrite(xmlWriter.Writer, obj);
		if (obj is IXmlSerializable xmlSerializable)
		{
			xmlSerializable.WriteXml(xmlSerializableWriter);
		}
		else if (obj is XmlElement xmlElement)
		{
			xmlElement.WriteTo(xmlSerializableWriter);
		}
		else
		{
			if (!(obj is XmlNode[] array))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Unknown XML type: '{0}'.", DataContract.GetClrTypeFullName(obj.GetType()))));
			}
			XmlNode[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].WriteTo(xmlSerializableWriter);
			}
		}
		xmlSerializableWriter.EndWrite();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[SecuritySafeCritical]
	internal void GetObjectData(ISerializable obj, SerializationInfo serInfo, StreamingContext context)
	{
		obj.GetObjectData(serInfo, context);
	}

	public void WriteISerializable(XmlWriterDelegator xmlWriter, ISerializable obj)
	{
		Type type = obj.GetType();
		SerializationInfo serializationInfo = new SerializationInfo(type, XmlObjectSerializer.FormatterConverter, !UnsafeTypeForwardingEnabled);
		GetObjectData(obj, serializationInfo, GetStreamingContext());
		if (!UnsafeTypeForwardingEnabled && serializationInfo.AssemblyName == "0")
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("ISerializable AssemblyName is set to \"0\" for type '{0}'.", DataContract.GetClrTypeFullName(obj.GetType()))));
		}
		WriteSerializationInfo(xmlWriter, type, serializationInfo);
	}

	internal void WriteSerializationInfo(XmlWriterDelegator xmlWriter, Type objType, SerializationInfo serInfo)
	{
		if (DataContract.GetClrTypeFullName(objType) != serInfo.FullTypeName)
		{
			if (base.DataContractResolver != null)
			{
				if (ResolveType(serInfo.ObjectType, objType, out var typeName, out var typeNamespace))
				{
					xmlWriter.WriteAttributeQualifiedName("z", DictionaryGlobals.ISerializableFactoryTypeLocalName, DictionaryGlobals.SerializationNamespace, typeName, typeNamespace);
				}
			}
			else
			{
				DataContract.GetDefaultStableName(serInfo.FullTypeName, out var localName, out var ns);
				xmlWriter.WriteAttributeQualifiedName("z", DictionaryGlobals.ISerializableFactoryTypeLocalName, DictionaryGlobals.SerializationNamespace, DataContract.GetClrTypeString(localName), DataContract.GetClrTypeString(ns));
			}
		}
		WriteClrTypeInfo(xmlWriter, objType, serInfo);
		IncrementItemCount(serInfo.MemberCount);
		SerializationInfoEnumerator enumerator = serInfo.GetEnumerator();
		while (enumerator.MoveNext())
		{
			SerializationEntry current = enumerator.Current;
			XmlDictionaryString clrTypeString = DataContract.GetClrTypeString(DataContract.EncodeLocalName(current.Name));
			xmlWriter.WriteStartElement(clrTypeString, DictionaryGlobals.EmptyString);
			object value = current.Value;
			if (value == null)
			{
				WriteNull(xmlWriter);
			}
			else
			{
				InternalSerializeReference(xmlWriter, value, isDeclaredType: false, writeXsiType: false, -1, Globals.TypeOfObject.TypeHandle);
			}
			xmlWriter.WriteEndElement();
		}
	}

	public void WriteExtensionData(XmlWriterDelegator xmlWriter, ExtensionDataObject extensionData, int memberIndex)
	{
		if (base.IgnoreExtensionDataObject || extensionData == null || extensionData.Members == null)
		{
			return;
		}
		for (int i = 0; i < extensionData.Members.Count; i++)
		{
			ExtensionDataMember extensionDataMember = extensionData.Members[i];
			if (extensionDataMember.MemberIndex == memberIndex)
			{
				WriteExtensionDataMember(xmlWriter, extensionDataMember);
			}
		}
	}

	private void WriteExtensionDataMember(XmlWriterDelegator xmlWriter, ExtensionDataMember member)
	{
		xmlWriter.WriteStartElement(member.Name, member.Namespace);
		IDataNode value = member.Value;
		WriteExtensionDataValue(xmlWriter, value);
		xmlWriter.WriteEndElement();
	}

	internal virtual void WriteExtensionDataTypeInfo(XmlWriterDelegator xmlWriter, IDataNode dataNode)
	{
		if (dataNode.DataContractName != null)
		{
			WriteTypeInfo(xmlWriter, dataNode.DataContractName, dataNode.DataContractNamespace);
		}
		WriteClrTypeInfo(xmlWriter, dataNode.DataType, dataNode.ClrTypeName, dataNode.ClrAssemblyName);
	}

	internal void WriteExtensionDataValue(XmlWriterDelegator xmlWriter, IDataNode dataNode)
	{
		IncrementItemCount(1);
		if (dataNode == null)
		{
			WriteNull(xmlWriter);
		}
		else
		{
			if (dataNode.PreservesReferences && OnHandleReference(xmlWriter, (dataNode.Value == null) ? dataNode : dataNode.Value, canContainCyclicReference: true))
			{
				return;
			}
			Type dataType = dataNode.DataType;
			if (dataType == Globals.TypeOfClassDataNode)
			{
				WriteExtensionClassData(xmlWriter, (ClassDataNode)dataNode);
			}
			else if (dataType == Globals.TypeOfCollectionDataNode)
			{
				WriteExtensionCollectionData(xmlWriter, (CollectionDataNode)dataNode);
			}
			else if (dataType == Globals.TypeOfXmlDataNode)
			{
				WriteExtensionXmlData(xmlWriter, (XmlDataNode)dataNode);
			}
			else if (dataType == Globals.TypeOfISerializableDataNode)
			{
				WriteExtensionISerializableData(xmlWriter, (ISerializableDataNode)dataNode);
			}
			else
			{
				WriteExtensionDataTypeInfo(xmlWriter, dataNode);
				if (dataType == Globals.TypeOfObject)
				{
					object value = dataNode.Value;
					if (value != null)
					{
						InternalSerialize(xmlWriter, value, isDeclaredType: false, writeXsiType: false, -1, value.GetType().TypeHandle);
					}
				}
				else
				{
					xmlWriter.WriteExtensionData(dataNode);
				}
			}
			if (dataNode.PreservesReferences)
			{
				OnEndHandleReference(xmlWriter, (dataNode.Value == null) ? dataNode : dataNode.Value, canContainCyclicReference: true);
			}
		}
	}

	internal bool TryWriteDeserializedExtensionData(XmlWriterDelegator xmlWriter, IDataNode dataNode)
	{
		object value = dataNode.Value;
		if (value == null)
		{
			return false;
		}
		Type type = ((dataNode.DataContractName == null) ? value.GetType() : Globals.TypeOfObject);
		InternalSerialize(xmlWriter, value, isDeclaredType: false, writeXsiType: false, -1, type.TypeHandle);
		return true;
	}

	private void WriteExtensionClassData(XmlWriterDelegator xmlWriter, ClassDataNode dataNode)
	{
		if (TryWriteDeserializedExtensionData(xmlWriter, dataNode))
		{
			return;
		}
		WriteExtensionDataTypeInfo(xmlWriter, dataNode);
		IList<ExtensionDataMember> members = dataNode.Members;
		if (members != null)
		{
			for (int i = 0; i < members.Count; i++)
			{
				WriteExtensionDataMember(xmlWriter, members[i]);
			}
		}
	}

	private void WriteExtensionCollectionData(XmlWriterDelegator xmlWriter, CollectionDataNode dataNode)
	{
		if (TryWriteDeserializedExtensionData(xmlWriter, dataNode))
		{
			return;
		}
		WriteExtensionDataTypeInfo(xmlWriter, dataNode);
		WriteArraySize(xmlWriter, dataNode.Size);
		IList<IDataNode> items = dataNode.Items;
		if (items != null)
		{
			for (int i = 0; i < items.Count; i++)
			{
				xmlWriter.WriteStartElement(dataNode.ItemName, dataNode.ItemNamespace);
				WriteExtensionDataValue(xmlWriter, items[i]);
				xmlWriter.WriteEndElement();
			}
		}
	}

	private void WriteExtensionISerializableData(XmlWriterDelegator xmlWriter, ISerializableDataNode dataNode)
	{
		if (TryWriteDeserializedExtensionData(xmlWriter, dataNode))
		{
			return;
		}
		WriteExtensionDataTypeInfo(xmlWriter, dataNode);
		if (dataNode.FactoryTypeName != null)
		{
			xmlWriter.WriteAttributeQualifiedName("z", DictionaryGlobals.ISerializableFactoryTypeLocalName, DictionaryGlobals.SerializationNamespace, dataNode.FactoryTypeName, dataNode.FactoryTypeNamespace);
		}
		IList<ISerializableDataMember> members = dataNode.Members;
		if (members != null)
		{
			for (int i = 0; i < members.Count; i++)
			{
				ISerializableDataMember serializableDataMember = members[i];
				xmlWriter.WriteStartElement(serializableDataMember.Name, string.Empty);
				WriteExtensionDataValue(xmlWriter, serializableDataMember.Value);
				xmlWriter.WriteEndElement();
			}
		}
	}

	private void WriteExtensionXmlData(XmlWriterDelegator xmlWriter, XmlDataNode dataNode)
	{
		if (TryWriteDeserializedExtensionData(xmlWriter, dataNode))
		{
			return;
		}
		IList<XmlAttribute> xmlAttributes = dataNode.XmlAttributes;
		if (xmlAttributes != null)
		{
			foreach (XmlAttribute item in xmlAttributes)
			{
				item.WriteTo(xmlWriter.Writer);
			}
		}
		WriteExtensionDataTypeInfo(xmlWriter, dataNode);
		IList<XmlNode> xmlChildNodes = dataNode.XmlChildNodes;
		if (xmlChildNodes == null)
		{
			return;
		}
		foreach (XmlNode item2 in xmlChildNodes)
		{
			item2.WriteTo(xmlWriter.Writer);
		}
	}

	protected virtual void WriteDataContractValue(DataContract dataContract, XmlWriterDelegator xmlWriter, object obj, RuntimeTypeHandle declaredTypeHandle)
	{
		dataContract.WriteXmlValue(xmlWriter, obj, this);
	}

	protected virtual void WriteNull(XmlWriterDelegator xmlWriter)
	{
		XmlObjectSerializer.WriteNull(xmlWriter);
	}

	private void WriteResolvedTypeInfo(XmlWriterDelegator writer, Type objectType, Type declaredType)
	{
		if (ResolveType(objectType, declaredType, out var typeName, out var typeNamespace))
		{
			WriteTypeInfo(writer, typeName, typeNamespace);
		}
	}

	private bool ResolveType(Type objectType, Type declaredType, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
	{
		if (!base.DataContractResolver.TryResolveType(objectType, declaredType, base.KnownTypeResolver, out typeName, out typeNamespace))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("An object of type '{0}' which derives from DataContractResolver returned false from its TryResolveType method when attempting to resolve the name for an object of type '{1}', indicating that the resolution failed. Change the TryResolveType implementation to return true.", DataContract.GetClrTypeFullName(base.DataContractResolver.GetType()), DataContract.GetClrTypeFullName(objectType))));
		}
		if (typeName == null)
		{
			if (typeNamespace == null)
			{
				return false;
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("An object of type '{0}' which derives from DataContractResolver returned a null typeName or typeNamespace but not both from its TryResolveType method when attempting to resolve the name for an object of type '{1}'. Change the TryResolveType implementation to return non-null values, or to return null values for both typeName and typeNamespace in order to serialize as the declared type.", DataContract.GetClrTypeFullName(base.DataContractResolver.GetType()), DataContract.GetClrTypeFullName(objectType))));
		}
		if (typeNamespace == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("An object of type '{0}' which derives from DataContractResolver returned a null typeName or typeNamespace but not both from its TryResolveType method when attempting to resolve the name for an object of type '{1}'. Change the TryResolveType implementation to return non-null values, or to return null values for both typeName and typeNamespace in order to serialize as the declared type.", DataContract.GetClrTypeFullName(base.DataContractResolver.GetType()), DataContract.GetClrTypeFullName(objectType))));
		}
		return true;
	}

	protected virtual bool WriteTypeInfo(XmlWriterDelegator writer, DataContract contract, DataContract declaredContract)
	{
		if (!XmlObjectSerializer.IsContractDeclared(contract, declaredContract))
		{
			if (base.DataContractResolver == null)
			{
				WriteTypeInfo(writer, contract.Name, contract.Namespace);
				return true;
			}
			WriteResolvedTypeInfo(writer, contract.OriginalUnderlyingType, declaredContract.OriginalUnderlyingType);
			return false;
		}
		return false;
	}

	protected virtual void WriteTypeInfo(XmlWriterDelegator writer, string dataContractName, string dataContractNamespace)
	{
		writer.WriteAttributeQualifiedName("i", DictionaryGlobals.XsiTypeLocalName, DictionaryGlobals.SchemaInstanceNamespace, dataContractName, dataContractNamespace);
	}

	protected virtual void WriteTypeInfo(XmlWriterDelegator writer, XmlDictionaryString dataContractName, XmlDictionaryString dataContractNamespace)
	{
		writer.WriteAttributeQualifiedName("i", DictionaryGlobals.XsiTypeLocalName, DictionaryGlobals.SchemaInstanceNamespace, dataContractName, dataContractNamespace);
	}
}
