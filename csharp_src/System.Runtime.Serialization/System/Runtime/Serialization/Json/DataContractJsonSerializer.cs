using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization.Json;

[TypeForwardedFrom("System.ServiceModel.Web, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
public sealed class DataContractJsonSerializer : XmlObjectSerializer
{
	internal IList<Type> knownTypeList;

	internal Dictionary<XmlQualifiedName, DataContract> knownDataContracts;

	private EmitTypeInformation emitTypeInformation;

	private IDataContractSurrogate dataContractSurrogate;

	private bool ignoreExtensionDataObject;

	private ReadOnlyCollection<Type> knownTypeCollection;

	private int maxItemsInObjectGraph;

	private DataContract rootContract;

	private XmlDictionaryString rootName;

	private bool rootNameRequiresMapping;

	private Type rootType;

	private bool serializeReadOnlyTypes;

	private DateTimeFormat dateTimeFormat;

	private bool useSimpleDictionaryFormat;

	public IDataContractSurrogate DataContractSurrogate => dataContractSurrogate;

	public bool IgnoreExtensionDataObject => ignoreExtensionDataObject;

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

	internal bool AlwaysEmitTypeInformation => emitTypeInformation == EmitTypeInformation.Always;

	public EmitTypeInformation EmitTypeInformation => emitTypeInformation;

	public bool SerializeReadOnlyTypes => serializeReadOnlyTypes;

	public DateTimeFormat DateTimeFormat => dateTimeFormat;

	public bool UseSimpleDictionaryFormat => useSimpleDictionaryFormat;

	private DataContract RootContract
	{
		get
		{
			if (rootContract == null)
			{
				rootContract = DataContract.GetDataContract((dataContractSurrogate == null) ? rootType : DataContractSerializer.GetSurrogatedType(dataContractSurrogate, rootType));
				CheckIfTypeIsReference(rootContract);
			}
			return rootContract;
		}
	}

	private XmlDictionaryString RootName => rootName ?? JsonGlobals.rootDictionaryString;

	public DataContractJsonSerializer(Type type)
		: this(type, (IEnumerable<Type>)null)
	{
	}

	public DataContractJsonSerializer(Type type, string rootName)
		: this(type, rootName, null)
	{
	}

	public DataContractJsonSerializer(Type type, XmlDictionaryString rootName)
		: this(type, rootName, null)
	{
	}

	public DataContractJsonSerializer(Type type, IEnumerable<Type> knownTypes)
		: this(type, knownTypes, int.MaxValue, ignoreExtensionDataObject: false, null, alwaysEmitTypeInformation: false)
	{
	}

	public DataContractJsonSerializer(Type type, string rootName, IEnumerable<Type> knownTypes)
		: this(type, rootName, knownTypes, int.MaxValue, ignoreExtensionDataObject: false, null, alwaysEmitTypeInformation: false)
	{
	}

	public DataContractJsonSerializer(Type type, XmlDictionaryString rootName, IEnumerable<Type> knownTypes)
		: this(type, rootName, knownTypes, int.MaxValue, ignoreExtensionDataObject: false, null, alwaysEmitTypeInformation: false)
	{
	}

	public DataContractJsonSerializer(Type type, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, IDataContractSurrogate dataContractSurrogate, bool alwaysEmitTypeInformation)
	{
		Initialize(type, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, dataContractSurrogate, alwaysEmitTypeInformation ? EmitTypeInformation.Always : EmitTypeInformation.AsNeeded, serializeReadOnlyTypes: false, null, useSimpleDictionaryFormat: false);
	}

	public DataContractJsonSerializer(Type type, string rootName, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, IDataContractSurrogate dataContractSurrogate, bool alwaysEmitTypeInformation)
	{
		EmitTypeInformation emitTypeInformation = (alwaysEmitTypeInformation ? EmitTypeInformation.Always : EmitTypeInformation.AsNeeded);
		XmlDictionary xmlDictionary = new XmlDictionary(2);
		Initialize(type, xmlDictionary.Add(rootName), knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, dataContractSurrogate, emitTypeInformation, serializeReadOnlyTypes: false, null, useSimpleDictionaryFormat: false);
	}

	public DataContractJsonSerializer(Type type, XmlDictionaryString rootName, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, IDataContractSurrogate dataContractSurrogate, bool alwaysEmitTypeInformation)
	{
		Initialize(type, rootName, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, dataContractSurrogate, alwaysEmitTypeInformation ? EmitTypeInformation.Always : EmitTypeInformation.AsNeeded, serializeReadOnlyTypes: false, null, useSimpleDictionaryFormat: false);
	}

	public DataContractJsonSerializer(Type type, DataContractJsonSerializerSettings settings)
	{
		if (settings == null)
		{
			settings = new DataContractJsonSerializerSettings();
		}
		Initialize(type, (settings.RootName == null) ? null : new XmlDictionary(1).Add(settings.RootName), settings.KnownTypes, settings.MaxItemsInObjectGraph, settings.IgnoreExtensionDataObject, settings.DataContractSurrogate, settings.EmitTypeInformation, settings.SerializeReadOnlyTypes, settings.DateTimeFormat, settings.UseSimpleDictionaryFormat);
	}

	public override bool IsStartObject(XmlReader reader)
	{
		return IsStartObjectHandleExceptions(new JsonReaderDelegator(reader));
	}

	public override bool IsStartObject(XmlDictionaryReader reader)
	{
		return IsStartObjectHandleExceptions(new JsonReaderDelegator(reader));
	}

	public override object ReadObject(Stream stream)
	{
		XmlObjectSerializer.CheckNull(stream, "stream");
		return ReadObject(JsonReaderWriterFactory.CreateJsonReader(stream, XmlDictionaryReaderQuotas.Max));
	}

	public override object ReadObject(XmlReader reader)
	{
		return ReadObjectHandleExceptions(new JsonReaderDelegator(reader, DateTimeFormat), verifyObjectName: true);
	}

	public override object ReadObject(XmlReader reader, bool verifyObjectName)
	{
		return ReadObjectHandleExceptions(new JsonReaderDelegator(reader, DateTimeFormat), verifyObjectName);
	}

	public override object ReadObject(XmlDictionaryReader reader)
	{
		return ReadObjectHandleExceptions(new JsonReaderDelegator(reader, DateTimeFormat), verifyObjectName: true);
	}

	public override object ReadObject(XmlDictionaryReader reader, bool verifyObjectName)
	{
		return ReadObjectHandleExceptions(new JsonReaderDelegator(reader, DateTimeFormat), verifyObjectName);
	}

	public override void WriteEndObject(XmlWriter writer)
	{
		WriteEndObjectHandleExceptions(new JsonWriterDelegator(writer));
	}

	public override void WriteEndObject(XmlDictionaryWriter writer)
	{
		WriteEndObjectHandleExceptions(new JsonWriterDelegator(writer));
	}

	public override void WriteObject(Stream stream, object graph)
	{
		XmlObjectSerializer.CheckNull(stream, "stream");
		XmlDictionaryWriter xmlDictionaryWriter = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, ownsStream: false);
		WriteObject(xmlDictionaryWriter, graph);
		xmlDictionaryWriter.Flush();
	}

	public override void WriteObject(XmlWriter writer, object graph)
	{
		WriteObjectHandleExceptions(new JsonWriterDelegator(writer, DateTimeFormat), graph);
	}

	public override void WriteObject(XmlDictionaryWriter writer, object graph)
	{
		WriteObjectHandleExceptions(new JsonWriterDelegator(writer, DateTimeFormat), graph);
	}

	public override void WriteObjectContent(XmlWriter writer, object graph)
	{
		WriteObjectContentHandleExceptions(new JsonWriterDelegator(writer, DateTimeFormat), graph);
	}

	public override void WriteObjectContent(XmlDictionaryWriter writer, object graph)
	{
		WriteObjectContentHandleExceptions(new JsonWriterDelegator(writer, DateTimeFormat), graph);
	}

	public override void WriteStartObject(XmlWriter writer, object graph)
	{
		WriteStartObjectHandleExceptions(new JsonWriterDelegator(writer), graph);
	}

	public override void WriteStartObject(XmlDictionaryWriter writer, object graph)
	{
		WriteStartObjectHandleExceptions(new JsonWriterDelegator(writer), graph);
	}

	internal static bool CheckIfJsonNameRequiresMapping(string jsonName)
	{
		if (jsonName != null)
		{
			if (!DataContract.IsValidNCName(jsonName))
			{
				return true;
			}
			for (int i = 0; i < jsonName.Length; i++)
			{
				if (XmlJsonWriter.CharacterNeedsEscaping(jsonName[i]))
				{
					return true;
				}
			}
		}
		return false;
	}

	internal static bool CheckIfJsonNameRequiresMapping(XmlDictionaryString jsonName)
	{
		if (jsonName != null)
		{
			return CheckIfJsonNameRequiresMapping(jsonName.Value);
		}
		return false;
	}

	internal static bool CheckIfXmlNameRequiresMapping(string xmlName)
	{
		if (xmlName != null)
		{
			return CheckIfJsonNameRequiresMapping(ConvertXmlNameToJsonName(xmlName));
		}
		return false;
	}

	internal static bool CheckIfXmlNameRequiresMapping(XmlDictionaryString xmlName)
	{
		if (xmlName != null)
		{
			return CheckIfXmlNameRequiresMapping(xmlName.Value);
		}
		return false;
	}

	internal static string ConvertXmlNameToJsonName(string xmlName)
	{
		return XmlConvert.DecodeName(xmlName);
	}

	internal static XmlDictionaryString ConvertXmlNameToJsonName(XmlDictionaryString xmlName)
	{
		if (xmlName != null)
		{
			return new XmlDictionary().Add(ConvertXmlNameToJsonName(xmlName.Value));
		}
		return null;
	}

	internal static bool IsJsonLocalName(XmlReaderDelegator reader, string elementName)
	{
		if (XmlObjectSerializerReadContextComplexJson.TryGetJsonLocalName(reader, out var name))
		{
			return elementName == name;
		}
		return false;
	}

	internal static object ReadJsonValue(DataContract contract, XmlReaderDelegator reader, XmlObjectSerializerReadContextComplexJson context)
	{
		return JsonDataContract.GetJsonDataContract(contract).ReadJsonValue(reader, context);
	}

	internal static void WriteJsonNull(XmlWriterDelegator writer)
	{
		writer.WriteAttributeString(null, "type", null, "null");
	}

	internal static void WriteJsonValue(JsonDataContract contract, XmlWriterDelegator writer, object graph, XmlObjectSerializerWriteContextComplexJson context, RuntimeTypeHandle declaredTypeHandle)
	{
		contract.WriteJsonValue(writer, graph, context, declaredTypeHandle);
	}

	internal override Type GetDeserializeType()
	{
		return rootType;
	}

	internal override Type GetSerializeType(object graph)
	{
		if (graph != null)
		{
			return graph.GetType();
		}
		return rootType;
	}

	internal override bool InternalIsStartObject(XmlReaderDelegator reader)
	{
		if (IsRootElement(reader, RootContract, RootName, XmlDictionaryString.Empty))
		{
			return true;
		}
		return IsJsonLocalName(reader, RootName.Value);
	}

	internal override object InternalReadObject(XmlReaderDelegator xmlReader, bool verifyObjectName)
	{
		if (MaxItemsInObjectGraph == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Maximum number of items that can be serialized or deserialized in an object graph is '{0}'.", MaxItemsInObjectGraph)));
		}
		if (verifyObjectName)
		{
			if (!InternalIsStartObject(xmlReader))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationExceptionWithReaderDetails(SR.GetString("Expecting element '{1}' from namespace '{0}'.", XmlDictionaryString.Empty, RootName), xmlReader));
			}
		}
		else if (!IsStartElement(xmlReader))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationExceptionWithReaderDetails(SR.GetString("Expecting state '{0}' when ReadObject is called.", XmlNodeType.Element), xmlReader));
		}
		DataContract dataContract = RootContract;
		if (dataContract.IsPrimitive && (object)dataContract.UnderlyingType == rootType)
		{
			return ReadJsonValue(dataContract, xmlReader, null);
		}
		return XmlObjectSerializerReadContextComplexJson.CreateContext(this, dataContract).InternalDeserialize(xmlReader, rootType, dataContract, null, null);
	}

	internal override void InternalWriteEndObject(XmlWriterDelegator writer)
	{
		writer.WriteEndElement();
	}

	internal override void InternalWriteObject(XmlWriterDelegator writer, object graph)
	{
		InternalWriteStartObject(writer, graph);
		InternalWriteObjectContent(writer, graph);
		InternalWriteEndObject(writer);
	}

	internal override void InternalWriteObjectContent(XmlWriterDelegator writer, object graph)
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
			graph = DataContractSerializer.SurrogateToDataContractType(dataContractSurrogate, graph, underlyingType, ref objType);
		}
		if (graph == null)
		{
			WriteJsonNull(writer);
			return;
		}
		if (underlyingType == objType)
		{
			if (dataContract.CanContainReferences)
			{
				XmlObjectSerializerWriteContextComplexJson xmlObjectSerializerWriteContextComplexJson = XmlObjectSerializerWriteContextComplexJson.CreateContext(this, dataContract);
				xmlObjectSerializerWriteContextComplexJson.OnHandleReference(writer, graph, canContainCyclicReference: true);
				xmlObjectSerializerWriteContextComplexJson.SerializeWithoutXsiType(dataContract, writer, graph, underlyingType.TypeHandle);
			}
			else
			{
				WriteJsonValue(JsonDataContract.GetJsonDataContract(dataContract), writer, graph, null, underlyingType.TypeHandle);
			}
			return;
		}
		XmlObjectSerializerWriteContextComplexJson xmlObjectSerializerWriteContextComplexJson2 = XmlObjectSerializerWriteContextComplexJson.CreateContext(this, RootContract);
		dataContract = GetDataContract(dataContract, underlyingType, objType);
		if (dataContract.CanContainReferences)
		{
			xmlObjectSerializerWriteContextComplexJson2.OnHandleReference(writer, graph, canContainCyclicReference: true);
			xmlObjectSerializerWriteContextComplexJson2.SerializeWithXsiTypeAtTopLevel(dataContract, writer, graph, underlyingType.TypeHandle, objType);
		}
		else
		{
			xmlObjectSerializerWriteContextComplexJson2.SerializeWithoutXsiType(dataContract, writer, graph, underlyingType.TypeHandle);
		}
	}

	internal override void InternalWriteStartObject(XmlWriterDelegator writer, object graph)
	{
		if (rootNameRequiresMapping)
		{
			writer.WriteStartElement("a", "item", "item");
			writer.WriteAttributeString(null, "item", null, RootName.Value);
		}
		else
		{
			writer.WriteStartElement(RootName, XmlDictionaryString.Empty);
		}
	}

	private void AddCollectionItemTypeToKnownTypes(Type knownType)
	{
		Type type = knownType;
		Type itemType;
		while (CollectionDataContract.IsCollection(type, out itemType))
		{
			if (itemType.IsGenericType && itemType.GetGenericTypeDefinition() == Globals.TypeOfKeyValue)
			{
				itemType = Globals.TypeOfKeyValuePair.MakeGenericType(itemType.GetGenericArguments());
			}
			knownTypeList.Add(itemType);
			type = itemType;
		}
	}

	private void Initialize(Type type, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, IDataContractSurrogate dataContractSurrogate, EmitTypeInformation emitTypeInformation, bool serializeReadOnlyTypes, DateTimeFormat dateTimeFormat, bool useSimpleDictionaryFormat)
	{
		XmlObjectSerializer.CheckNull(type, "type");
		rootType = type;
		if (knownTypes != null)
		{
			knownTypeList = new List<Type>();
			foreach (Type knownType in knownTypes)
			{
				knownTypeList.Add(knownType);
				if (knownType != null)
				{
					AddCollectionItemTypeToKnownTypes(knownType);
				}
			}
		}
		if (maxItemsInObjectGraph < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("maxItemsInObjectGraph", SR.GetString("The value of this argument must be non-negative.")));
		}
		this.maxItemsInObjectGraph = maxItemsInObjectGraph;
		this.ignoreExtensionDataObject = ignoreExtensionDataObject;
		this.dataContractSurrogate = dataContractSurrogate;
		this.emitTypeInformation = emitTypeInformation;
		this.serializeReadOnlyTypes = serializeReadOnlyTypes;
		this.dateTimeFormat = dateTimeFormat;
		this.useSimpleDictionaryFormat = useSimpleDictionaryFormat;
	}

	private void Initialize(Type type, XmlDictionaryString rootName, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, IDataContractSurrogate dataContractSurrogate, EmitTypeInformation emitTypeInformation, bool serializeReadOnlyTypes, DateTimeFormat dateTimeFormat, bool useSimpleDictionaryFormat)
	{
		Initialize(type, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, dataContractSurrogate, emitTypeInformation, serializeReadOnlyTypes, dateTimeFormat, useSimpleDictionaryFormat);
		this.rootName = ConvertXmlNameToJsonName(rootName);
		rootNameRequiresMapping = CheckIfJsonNameRequiresMapping(this.rootName);
	}

	internal static void CheckIfTypeIsReference(DataContract dataContract)
	{
		if (dataContract.IsReference)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Unsupported value for IsReference for type '{0}', IsReference value is {1}.", DataContract.GetClrTypeFullName(dataContract.UnderlyingType), dataContract.IsReference)));
		}
	}

	internal static DataContract GetDataContract(DataContract declaredTypeContract, Type declaredType, Type objectType)
	{
		DataContract dataContract = DataContractSerializer.GetDataContract(declaredTypeContract, declaredType, objectType);
		CheckIfTypeIsReference(dataContract);
		return dataContract;
	}
}
