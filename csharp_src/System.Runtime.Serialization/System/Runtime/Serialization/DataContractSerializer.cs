using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace System.Runtime.Serialization;

public sealed class DataContractSerializer : XmlObjectSerializer
{
	private Type rootType;

	private DataContract rootContract;

	private bool needsContractNsAtRoot;

	private XmlDictionaryString rootName;

	private XmlDictionaryString rootNamespace;

	private int maxItemsInObjectGraph;

	private bool ignoreExtensionDataObject;

	private bool preserveObjectReferences;

	private IDataContractSurrogate dataContractSurrogate;

	private ReadOnlyCollection<Type> knownTypeCollection;

	internal IList<Type> knownTypeList;

	internal Dictionary<XmlQualifiedName, DataContract> knownDataContracts;

	private DataContractResolver dataContractResolver;

	private bool serializeReadOnlyTypes;

	public ReadOnlyCollection<Type> KnownTypes
	{
		get
		{
			if (knownTypeCollection == null)
			{
				if (knownTypeList != null)
				{
					knownTypeCollection = new ReadOnlyCollection<Type>(knownTypeList);
				}
				else
				{
					knownTypeCollection = new ReadOnlyCollection<Type>(Globals.EmptyTypeArray);
				}
			}
			return knownTypeCollection;
		}
	}

	internal override Dictionary<XmlQualifiedName, DataContract> KnownDataContracts
	{
		get
		{
			if (knownDataContracts == null && knownTypeList != null)
			{
				knownDataContracts = XmlObjectSerializerContext.GetDataContractsForKnownTypes(knownTypeList);
			}
			return knownDataContracts;
		}
	}

	public int MaxItemsInObjectGraph => maxItemsInObjectGraph;

	public IDataContractSurrogate DataContractSurrogate => dataContractSurrogate;

	public bool PreserveObjectReferences => preserveObjectReferences;

	public bool IgnoreExtensionDataObject => ignoreExtensionDataObject;

	public DataContractResolver DataContractResolver => dataContractResolver;

	public bool SerializeReadOnlyTypes => serializeReadOnlyTypes;

	private DataContract RootContract
	{
		get
		{
			if (rootContract == null)
			{
				rootContract = DataContract.GetDataContract((dataContractSurrogate == null) ? rootType : GetSurrogatedType(dataContractSurrogate, rootType));
				needsContractNsAtRoot = CheckIfNeedsContractNsAtRoot(rootName, rootNamespace, rootContract);
			}
			return rootContract;
		}
	}

	public DataContractSerializer(Type type)
		: this(type, (IEnumerable<Type>)null)
	{
	}

	public DataContractSerializer(Type type, IEnumerable<Type> knownTypes)
		: this(type, knownTypes, int.MaxValue, ignoreExtensionDataObject: false, preserveObjectReferences: false, null)
	{
	}

	public DataContractSerializer(Type type, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, bool preserveObjectReferences, IDataContractSurrogate dataContractSurrogate)
		: this(type, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, preserveObjectReferences, dataContractSurrogate, null)
	{
	}

	public DataContractSerializer(Type type, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, bool preserveObjectReferences, IDataContractSurrogate dataContractSurrogate, DataContractResolver dataContractResolver)
	{
		Initialize(type, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, preserveObjectReferences, dataContractSurrogate, dataContractResolver, serializeReadOnlyTypes: false);
	}

	public DataContractSerializer(Type type, string rootName, string rootNamespace)
		: this(type, rootName, rootNamespace, null)
	{
	}

	public DataContractSerializer(Type type, string rootName, string rootNamespace, IEnumerable<Type> knownTypes)
		: this(type, rootName, rootNamespace, knownTypes, int.MaxValue, ignoreExtensionDataObject: false, preserveObjectReferences: false, null)
	{
	}

	public DataContractSerializer(Type type, string rootName, string rootNamespace, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, bool preserveObjectReferences, IDataContractSurrogate dataContractSurrogate)
		: this(type, rootName, rootNamespace, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, preserveObjectReferences, dataContractSurrogate, null)
	{
	}

	public DataContractSerializer(Type type, string rootName, string rootNamespace, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, bool preserveObjectReferences, IDataContractSurrogate dataContractSurrogate, DataContractResolver dataContractResolver)
	{
		XmlDictionary xmlDictionary = new XmlDictionary(2);
		Initialize(type, xmlDictionary.Add(rootName), xmlDictionary.Add(DataContract.GetNamespace(rootNamespace)), knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, preserveObjectReferences, dataContractSurrogate, dataContractResolver, serializeReadOnlyTypes: false);
	}

	public DataContractSerializer(Type type, XmlDictionaryString rootName, XmlDictionaryString rootNamespace)
		: this(type, rootName, rootNamespace, null)
	{
	}

	public DataContractSerializer(Type type, XmlDictionaryString rootName, XmlDictionaryString rootNamespace, IEnumerable<Type> knownTypes)
		: this(type, rootName, rootNamespace, knownTypes, int.MaxValue, ignoreExtensionDataObject: false, preserveObjectReferences: false, null, null)
	{
	}

	public DataContractSerializer(Type type, XmlDictionaryString rootName, XmlDictionaryString rootNamespace, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, bool preserveObjectReferences, IDataContractSurrogate dataContractSurrogate)
		: this(type, rootName, rootNamespace, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, preserveObjectReferences, dataContractSurrogate, null)
	{
	}

	public DataContractSerializer(Type type, XmlDictionaryString rootName, XmlDictionaryString rootNamespace, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, bool preserveObjectReferences, IDataContractSurrogate dataContractSurrogate, DataContractResolver dataContractResolver)
	{
		Initialize(type, rootName, rootNamespace, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, preserveObjectReferences, dataContractSurrogate, dataContractResolver, serializeReadOnlyTypes: false);
	}

	public DataContractSerializer(Type type, DataContractSerializerSettings settings)
	{
		if (settings == null)
		{
			settings = new DataContractSerializerSettings();
		}
		Initialize(type, settings.RootName, settings.RootNamespace, settings.KnownTypes, settings.MaxItemsInObjectGraph, settings.IgnoreExtensionDataObject, settings.PreserveObjectReferences, settings.DataContractSurrogate, settings.DataContractResolver, settings.SerializeReadOnlyTypes);
	}

	private void Initialize(Type type, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, bool preserveObjectReferences, IDataContractSurrogate dataContractSurrogate, DataContractResolver dataContractResolver, bool serializeReadOnlyTypes)
	{
		XmlObjectSerializer.CheckNull(type, "type");
		rootType = type;
		if (knownTypes != null)
		{
			knownTypeList = new List<Type>();
			foreach (Type knownType in knownTypes)
			{
				knownTypeList.Add(knownType);
			}
		}
		if (maxItemsInObjectGraph < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("maxItemsInObjectGraph", SR.GetString("The value of this argument must be non-negative.")));
		}
		this.maxItemsInObjectGraph = maxItemsInObjectGraph;
		this.ignoreExtensionDataObject = ignoreExtensionDataObject;
		this.preserveObjectReferences = preserveObjectReferences;
		this.dataContractSurrogate = dataContractSurrogate;
		this.dataContractResolver = dataContractResolver;
		this.serializeReadOnlyTypes = serializeReadOnlyTypes;
	}

	private void Initialize(Type type, XmlDictionaryString rootName, XmlDictionaryString rootNamespace, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, bool preserveObjectReferences, IDataContractSurrogate dataContractSurrogate, DataContractResolver dataContractResolver, bool serializeReadOnlyTypes)
	{
		Initialize(type, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, preserveObjectReferences, dataContractSurrogate, dataContractResolver, serializeReadOnlyTypes);
		this.rootName = rootName;
		this.rootNamespace = rootNamespace;
	}

	internal override void InternalWriteObject(XmlWriterDelegator writer, object graph)
	{
		InternalWriteObject(writer, graph, null);
	}

	internal override void InternalWriteObject(XmlWriterDelegator writer, object graph, DataContractResolver dataContractResolver)
	{
		InternalWriteStartObject(writer, graph);
		InternalWriteObjectContent(writer, graph, dataContractResolver);
		InternalWriteEndObject(writer);
	}

	public override void WriteObject(XmlWriter writer, object graph)
	{
		WriteObjectHandleExceptions(new XmlWriterDelegator(writer), graph);
	}

	public override void WriteStartObject(XmlWriter writer, object graph)
	{
		WriteStartObjectHandleExceptions(new XmlWriterDelegator(writer), graph);
	}

	public override void WriteObjectContent(XmlWriter writer, object graph)
	{
		WriteObjectContentHandleExceptions(new XmlWriterDelegator(writer), graph);
	}

	public override void WriteEndObject(XmlWriter writer)
	{
		WriteEndObjectHandleExceptions(new XmlWriterDelegator(writer));
	}

	public override void WriteStartObject(XmlDictionaryWriter writer, object graph)
	{
		WriteStartObjectHandleExceptions(new XmlWriterDelegator(writer), graph);
	}

	public override void WriteObjectContent(XmlDictionaryWriter writer, object graph)
	{
		WriteObjectContentHandleExceptions(new XmlWriterDelegator(writer), graph);
	}

	public override void WriteEndObject(XmlDictionaryWriter writer)
	{
		WriteEndObjectHandleExceptions(new XmlWriterDelegator(writer));
	}

	public void WriteObject(XmlDictionaryWriter writer, object graph, DataContractResolver dataContractResolver)
	{
		WriteObjectHandleExceptions(new XmlWriterDelegator(writer), graph, dataContractResolver);
	}

	public override object ReadObject(XmlReader reader)
	{
		return ReadObjectHandleExceptions(new XmlReaderDelegator(reader), verifyObjectName: true);
	}

	public override object ReadObject(XmlReader reader, bool verifyObjectName)
	{
		return ReadObjectHandleExceptions(new XmlReaderDelegator(reader), verifyObjectName);
	}

	public override bool IsStartObject(XmlReader reader)
	{
		return IsStartObjectHandleExceptions(new XmlReaderDelegator(reader));
	}

	public override object ReadObject(XmlDictionaryReader reader, bool verifyObjectName)
	{
		return ReadObjectHandleExceptions(new XmlReaderDelegator(reader), verifyObjectName);
	}

	public override bool IsStartObject(XmlDictionaryReader reader)
	{
		return IsStartObjectHandleExceptions(new XmlReaderDelegator(reader));
	}

	public object ReadObject(XmlDictionaryReader reader, bool verifyObjectName, DataContractResolver dataContractResolver)
	{
		return ReadObjectHandleExceptions(new XmlReaderDelegator(reader), verifyObjectName, dataContractResolver);
	}

	internal override void InternalWriteStartObject(XmlWriterDelegator writer, object graph)
	{
		WriteRootElement(writer, RootContract, rootName, rootNamespace, needsContractNsAtRoot);
	}

	internal override void InternalWriteObjectContent(XmlWriterDelegator writer, object graph)
	{
		InternalWriteObjectContent(writer, graph, null);
	}

	internal void InternalWriteObjectContent(XmlWriterDelegator writer, object graph, DataContractResolver dataContractResolver)
	{
		if (MaxItemsInObjectGraph == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Maximum number of items that can be serialized or deserialized in an object graph is '{0}'.", MaxItemsInObjectGraph)));
		}
		DataContract dataContract = RootContract;
		Type underlyingType = dataContract.UnderlyingType;
		Type objType = ((graph == null) ? underlyingType : graph.GetType());
		if (dataContractSurrogate != null)
		{
			graph = SurrogateToDataContractType(dataContractSurrogate, graph, underlyingType, ref objType);
		}
		if (dataContractResolver == null)
		{
			dataContractResolver = DataContractResolver;
		}
		if (graph == null)
		{
			if (IsRootXmlAny(rootName, dataContract))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("A null value cannot be serialized at the top level for IXmlSerializable root type '{0}' since its IsAny setting is 'true'. This type must write all its contents including the root element. Verify that the IXmlSerializable implementation is correct.", underlyingType)));
			}
			XmlObjectSerializer.WriteNull(writer);
			return;
		}
		if (underlyingType == objType)
		{
			if (dataContract.CanContainReferences)
			{
				XmlObjectSerializerWriteContext xmlObjectSerializerWriteContext = XmlObjectSerializerWriteContext.CreateContext(this, dataContract, dataContractResolver);
				xmlObjectSerializerWriteContext.HandleGraphAtTopLevel(writer, graph, dataContract);
				xmlObjectSerializerWriteContext.SerializeWithoutXsiType(dataContract, writer, graph, underlyingType.TypeHandle);
			}
			else
			{
				dataContract.WriteXmlValue(writer, graph, null);
			}
			return;
		}
		XmlObjectSerializerWriteContext xmlObjectSerializerWriteContext2 = null;
		if (IsRootXmlAny(rootName, dataContract))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("An object of type '{0}' cannot be serialized at the top level for IXmlSerializable root type '{1}' since its IsAny setting is 'true'. This type must write all its contents including the root element. Verify that the IXmlSerializable implementation is correct.", objType, dataContract.UnderlyingType)));
		}
		dataContract = GetDataContract(dataContract, underlyingType, objType);
		xmlObjectSerializerWriteContext2 = XmlObjectSerializerWriteContext.CreateContext(this, RootContract, dataContractResolver);
		if (dataContract.CanContainReferences)
		{
			xmlObjectSerializerWriteContext2.HandleGraphAtTopLevel(writer, graph, dataContract);
		}
		xmlObjectSerializerWriteContext2.OnHandleIsReference(writer, dataContract, graph);
		xmlObjectSerializerWriteContext2.SerializeWithXsiTypeAtTopLevel(dataContract, writer, graph, underlyingType.TypeHandle, objType);
	}

	internal static DataContract GetDataContract(DataContract declaredTypeContract, Type declaredType, Type objectType)
	{
		if (declaredType.IsInterface && CollectionDataContract.IsCollectionInterface(declaredType))
		{
			return declaredTypeContract;
		}
		if (declaredType.IsArray)
		{
			return declaredTypeContract;
		}
		return DataContract.GetDataContract(objectType.TypeHandle, objectType, SerializationMode.SharedContract);
	}

	internal void SetDataContractSurrogate(IDataContractSurrogate adapter)
	{
		dataContractSurrogate = adapter;
	}

	internal override void InternalWriteEndObject(XmlWriterDelegator writer)
	{
		if (!IsRootXmlAny(rootName, RootContract))
		{
			writer.WriteEndElement();
		}
	}

	internal override object InternalReadObject(XmlReaderDelegator xmlReader, bool verifyObjectName)
	{
		return InternalReadObject(xmlReader, verifyObjectName, null);
	}

	internal override object InternalReadObject(XmlReaderDelegator xmlReader, bool verifyObjectName, DataContractResolver dataContractResolver)
	{
		if (MaxItemsInObjectGraph == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Maximum number of items that can be serialized or deserialized in an object graph is '{0}'.", MaxItemsInObjectGraph)));
		}
		if (dataContractResolver == null)
		{
			dataContractResolver = DataContractResolver;
		}
		if (verifyObjectName)
		{
			if (!InternalIsStartObject(xmlReader))
			{
				XmlDictionaryString topLevelElementName;
				XmlDictionaryString topLevelElementNamespace;
				if (rootName == null)
				{
					topLevelElementName = RootContract.TopLevelElementName;
					topLevelElementNamespace = RootContract.TopLevelElementNamespace;
				}
				else
				{
					topLevelElementName = rootName;
					topLevelElementNamespace = rootNamespace;
				}
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationExceptionWithReaderDetails(SR.GetString("Expecting element '{1}' from namespace '{0}'.", topLevelElementNamespace, topLevelElementName), xmlReader));
			}
		}
		else if (!IsStartElement(xmlReader))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationExceptionWithReaderDetails(SR.GetString("Expecting state '{0}' when ReadObject is called.", XmlNodeType.Element), xmlReader));
		}
		DataContract dataContract = RootContract;
		if (dataContract.IsPrimitive && (object)dataContract.UnderlyingType == rootType)
		{
			return dataContract.ReadXmlValue(xmlReader, null);
		}
		if (IsRootXmlAny(rootName, dataContract))
		{
			return XmlObjectSerializerReadContext.ReadRootIXmlSerializable(xmlReader, dataContract as XmlDataContract, isMemberType: false);
		}
		return XmlObjectSerializerReadContext.CreateContext(this, dataContract, dataContractResolver).InternalDeserialize(xmlReader, rootType, dataContract, null, null);
	}

	internal override bool InternalIsStartObject(XmlReaderDelegator reader)
	{
		return IsRootElement(reader, RootContract, rootName, rootNamespace);
	}

	internal override Type GetSerializeType(object graph)
	{
		if (graph != null)
		{
			return graph.GetType();
		}
		return rootType;
	}

	internal override Type GetDeserializeType()
	{
		return rootType;
	}

	internal static object SurrogateToDataContractType(IDataContractSurrogate dataContractSurrogate, object oldObj, Type surrogatedDeclaredType, ref Type objType)
	{
		object objectToSerialize = DataContractSurrogateCaller.GetObjectToSerialize(dataContractSurrogate, oldObj, objType, surrogatedDeclaredType);
		if (objectToSerialize != oldObj)
		{
			if (objectToSerialize == null)
			{
				objType = Globals.TypeOfObject;
			}
			else
			{
				objType = objectToSerialize.GetType();
			}
		}
		return objectToSerialize;
	}

	internal static Type GetSurrogatedType(IDataContractSurrogate dataContractSurrogate, Type type)
	{
		return DataContractSurrogateCaller.GetDataContractType(dataContractSurrogate, DataContract.UnwrapNullableType(type));
	}
}
