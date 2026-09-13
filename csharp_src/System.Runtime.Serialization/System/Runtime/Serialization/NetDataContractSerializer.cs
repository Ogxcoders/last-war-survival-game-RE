using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters;
using System.Security;
using System.Security.Permissions;
using System.Xml;

namespace System.Runtime.Serialization;

public sealed class NetDataContractSerializer : XmlObjectSerializer, IFormatter
{
	private XmlDictionaryString rootName;

	private XmlDictionaryString rootNamespace;

	private StreamingContext context;

	private SerializationBinder binder;

	private ISurrogateSelector surrogateSelector;

	private int maxItemsInObjectGraph;

	private bool ignoreExtensionDataObject;

	private FormatterAssemblyStyle assemblyFormat;

	private DataContract cachedDataContract;

	private static Hashtable typeNameCache = new Hashtable();

	private static bool? unsafeTypeForwardingEnabled;

	internal static bool UnsafeTypeForwardingEnabled
	{
		[SecuritySafeCritical]
		get
		{
			if (!unsafeTypeForwardingEnabled.HasValue)
			{
				unsafeTypeForwardingEnabled = false;
			}
			return unsafeTypeForwardingEnabled.Value;
		}
	}

	public StreamingContext Context
	{
		get
		{
			return context;
		}
		set
		{
			context = value;
		}
	}

	public SerializationBinder Binder
	{
		get
		{
			return binder;
		}
		set
		{
			binder = value;
		}
	}

	public ISurrogateSelector SurrogateSelector
	{
		get
		{
			return surrogateSelector;
		}
		set
		{
			surrogateSelector = value;
		}
	}

	public FormatterAssemblyStyle AssemblyFormat
	{
		get
		{
			return assemblyFormat;
		}
		set
		{
			if (value != FormatterAssemblyStyle.Full && value != FormatterAssemblyStyle.Simple)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("'{0}': invalid assembly format.", value)));
			}
			assemblyFormat = value;
		}
	}

	public int MaxItemsInObjectGraph => maxItemsInObjectGraph;

	public bool IgnoreExtensionDataObject => ignoreExtensionDataObject;

	public NetDataContractSerializer()
		: this(new StreamingContext(StreamingContextStates.All))
	{
	}

	public NetDataContractSerializer(StreamingContext context)
		: this(context, int.MaxValue, ignoreExtensionDataObject: false, FormatterAssemblyStyle.Full, null)
	{
	}

	public NetDataContractSerializer(StreamingContext context, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, FormatterAssemblyStyle assemblyFormat, ISurrogateSelector surrogateSelector)
	{
		Initialize(context, maxItemsInObjectGraph, ignoreExtensionDataObject, assemblyFormat, surrogateSelector);
	}

	public NetDataContractSerializer(string rootName, string rootNamespace)
		: this(rootName, rootNamespace, new StreamingContext(StreamingContextStates.All), int.MaxValue, ignoreExtensionDataObject: false, FormatterAssemblyStyle.Full, null)
	{
	}

	public NetDataContractSerializer(string rootName, string rootNamespace, StreamingContext context, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, FormatterAssemblyStyle assemblyFormat, ISurrogateSelector surrogateSelector)
	{
		XmlDictionary xmlDictionary = new XmlDictionary(2);
		Initialize(xmlDictionary.Add(rootName), xmlDictionary.Add(DataContract.GetNamespace(rootNamespace)), context, maxItemsInObjectGraph, ignoreExtensionDataObject, assemblyFormat, surrogateSelector);
	}

	public NetDataContractSerializer(XmlDictionaryString rootName, XmlDictionaryString rootNamespace)
		: this(rootName, rootNamespace, new StreamingContext(StreamingContextStates.All), int.MaxValue, ignoreExtensionDataObject: false, FormatterAssemblyStyle.Full, null)
	{
	}

	public NetDataContractSerializer(XmlDictionaryString rootName, XmlDictionaryString rootNamespace, StreamingContext context, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, FormatterAssemblyStyle assemblyFormat, ISurrogateSelector surrogateSelector)
	{
		Initialize(rootName, rootNamespace, context, maxItemsInObjectGraph, ignoreExtensionDataObject, assemblyFormat, surrogateSelector);
	}

	private void Initialize(StreamingContext context, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, FormatterAssemblyStyle assemblyFormat, ISurrogateSelector surrogateSelector)
	{
		this.context = context;
		if (maxItemsInObjectGraph < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("maxItemsInObjectGraph", SR.GetString("The value of this argument must be non-negative.")));
		}
		this.maxItemsInObjectGraph = maxItemsInObjectGraph;
		this.ignoreExtensionDataObject = ignoreExtensionDataObject;
		this.surrogateSelector = surrogateSelector;
		AssemblyFormat = assemblyFormat;
	}

	private void Initialize(XmlDictionaryString rootName, XmlDictionaryString rootNamespace, StreamingContext context, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, FormatterAssemblyStyle assemblyFormat, ISurrogateSelector surrogateSelector)
	{
		Initialize(context, maxItemsInObjectGraph, ignoreExtensionDataObject, assemblyFormat, surrogateSelector);
		this.rootName = rootName;
		this.rootNamespace = rootNamespace;
	}

	public void Serialize(Stream stream, object graph)
	{
		base.WriteObject(stream, graph);
	}

	public object Deserialize(Stream stream)
	{
		return base.ReadObject(stream);
	}

	internal override void InternalWriteObject(XmlWriterDelegator writer, object graph)
	{
		Hashtable surrogateDataContracts = null;
		DataContract dataContract = GetDataContract(graph, ref surrogateDataContracts);
		InternalWriteStartObject(writer, graph, dataContract);
		InternalWriteObjectContent(writer, graph, dataContract, surrogateDataContracts);
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

	internal override void InternalWriteStartObject(XmlWriterDelegator writer, object graph)
	{
		Hashtable surrogateDataContracts = null;
		DataContract dataContract = GetDataContract(graph, ref surrogateDataContracts);
		InternalWriteStartObject(writer, graph, dataContract);
	}

	private void InternalWriteStartObject(XmlWriterDelegator writer, object graph, DataContract contract)
	{
		WriteRootElement(writer, contract, rootName, rootNamespace, CheckIfNeedsContractNsAtRoot(rootName, rootNamespace, contract));
	}

	public override void WriteObjectContent(XmlDictionaryWriter writer, object graph)
	{
		WriteObjectContentHandleExceptions(new XmlWriterDelegator(writer), graph);
	}

	internal override void InternalWriteObjectContent(XmlWriterDelegator writer, object graph)
	{
		Hashtable surrogateDataContracts = null;
		DataContract dataContract = GetDataContract(graph, ref surrogateDataContracts);
		InternalWriteObjectContent(writer, graph, dataContract, surrogateDataContracts);
	}

	private void InternalWriteObjectContent(XmlWriterDelegator writer, object graph, DataContract contract, Hashtable surrogateDataContracts)
	{
		if (MaxItemsInObjectGraph == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Maximum number of items that can be serialized or deserialized in an object graph is '{0}'.", MaxItemsInObjectGraph)));
		}
		if (IsRootXmlAny(rootName, contract))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("For type '{0}', IsAny is not supported by NetDataContractSerializer.", contract.UnderlyingType)));
		}
		if (graph == null)
		{
			XmlObjectSerializer.WriteNull(writer);
			return;
		}
		Type type = graph.GetType();
		if (contract.UnderlyingType != type)
		{
			contract = GetDataContract(graph, ref surrogateDataContracts);
		}
		XmlObjectSerializerWriteContext xmlObjectSerializerWriteContext = null;
		if (contract.CanContainReferences)
		{
			xmlObjectSerializerWriteContext = XmlObjectSerializerWriteContext.CreateContext(this, surrogateDataContracts);
			xmlObjectSerializerWriteContext.HandleGraphAtTopLevel(writer, graph, contract);
		}
		WriteClrTypeInfo(writer, contract, binder);
		contract.WriteXmlValue(writer, graph, xmlObjectSerializerWriteContext);
	}

	internal static void WriteClrTypeInfo(XmlWriterDelegator writer, DataContract dataContract, SerializationBinder binder)
	{
		if (dataContract.IsISerializable || dataContract is SurrogateDataContract)
		{
			return;
		}
		TypeInformation typeInformation = null;
		Type originalUnderlyingType = dataContract.OriginalUnderlyingType;
		string typeName = null;
		string assemblyName = null;
		binder?.BindToName(originalUnderlyingType, out assemblyName, out typeName);
		if (typeName == null)
		{
			typeInformation = GetTypeInformation(originalUnderlyingType);
			typeName = typeInformation.FullTypeName;
		}
		if (assemblyName == null)
		{
			assemblyName = ((typeInformation == null) ? GetTypeInformation(originalUnderlyingType).AssemblyString : typeInformation.AssemblyString);
			if (!UnsafeTypeForwardingEnabled && !originalUnderlyingType.Assembly.IsFullyTrusted && !IsAssemblyNameForwardingSafe(originalUnderlyingType.Assembly.FullName, assemblyName))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Type '{0}' in assembly '{1}' cannot be forwarded from assembly '{2}'.", DataContract.GetClrTypeFullName(originalUnderlyingType), originalUnderlyingType.Assembly.FullName, assemblyName)));
			}
		}
		WriteClrTypeInfo(writer, typeName, assemblyName);
	}

	internal static void WriteClrTypeInfo(XmlWriterDelegator writer, Type dataContractType, SerializationBinder binder, string defaultClrTypeName, string defaultClrAssemblyName)
	{
		string typeName = null;
		string assemblyName = null;
		binder?.BindToName(dataContractType, out assemblyName, out typeName);
		if (typeName == null)
		{
			typeName = defaultClrTypeName;
		}
		if (assemblyName == null)
		{
			assemblyName = defaultClrAssemblyName;
		}
		WriteClrTypeInfo(writer, typeName, assemblyName);
	}

	internal static void WriteClrTypeInfo(XmlWriterDelegator writer, Type dataContractType, SerializationBinder binder, SerializationInfo serInfo)
	{
		TypeInformation typeInformation = null;
		string typeName = null;
		string assemblyName = null;
		binder?.BindToName(dataContractType, out assemblyName, out typeName);
		if (typeName == null)
		{
			if (serInfo.IsFullTypeNameSetExplicit)
			{
				typeName = serInfo.FullTypeName;
			}
			else
			{
				typeInformation = GetTypeInformation(serInfo.ObjectType);
				typeName = typeInformation.FullTypeName;
			}
		}
		if (assemblyName == null)
		{
			assemblyName = ((!serInfo.IsAssemblyNameSetExplicit) ? ((typeInformation == null) ? GetTypeInformation(serInfo.ObjectType).AssemblyString : typeInformation.AssemblyString) : serInfo.AssemblyName);
		}
		WriteClrTypeInfo(writer, typeName, assemblyName);
	}

	private static void WriteClrTypeInfo(XmlWriterDelegator writer, string clrTypeName, string clrAssemblyName)
	{
		if (clrTypeName != null)
		{
			writer.WriteAttributeString("z", DictionaryGlobals.ClrTypeLocalName, DictionaryGlobals.SerializationNamespace, DataContract.GetClrTypeString(clrTypeName));
		}
		if (clrAssemblyName != null)
		{
			writer.WriteAttributeString("z", DictionaryGlobals.ClrAssemblyLocalName, DictionaryGlobals.SerializationNamespace, DataContract.GetClrTypeString(clrAssemblyName));
		}
	}

	public override void WriteEndObject(XmlDictionaryWriter writer)
	{
		WriteEndObjectHandleExceptions(new XmlWriterDelegator(writer));
	}

	internal override void InternalWriteEndObject(XmlWriterDelegator writer)
	{
		writer.WriteEndElement();
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

	internal override object InternalReadObject(XmlReaderDelegator xmlReader, bool verifyObjectName)
	{
		if (MaxItemsInObjectGraph == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Maximum number of items that can be serialized or deserialized in an object graph is '{0}'.", MaxItemsInObjectGraph)));
		}
		if (!IsStartElement(xmlReader))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationExceptionWithReaderDetails(SR.GetString("Expecting state '{0}' when ReadObject is called.", XmlNodeType.Element), xmlReader));
		}
		return XmlObjectSerializerReadContext.CreateContext(this).InternalDeserialize(xmlReader, null, null, null);
	}

	internal override bool InternalIsStartObject(XmlReaderDelegator reader)
	{
		return IsStartElement(reader);
	}

	internal DataContract GetDataContract(object obj, ref Hashtable surrogateDataContracts)
	{
		return GetDataContract((obj == null) ? Globals.TypeOfObject : obj.GetType(), ref surrogateDataContracts);
	}

	internal DataContract GetDataContract(Type type, ref Hashtable surrogateDataContracts)
	{
		return GetDataContract(type.TypeHandle, type, ref surrogateDataContracts);
	}

	internal DataContract GetDataContract(RuntimeTypeHandle typeHandle, Type type, ref Hashtable surrogateDataContracts)
	{
		DataContract dataContractFromSurrogateSelector = GetDataContractFromSurrogateSelector(surrogateSelector, Context, typeHandle, type, ref surrogateDataContracts);
		if (dataContractFromSurrogateSelector != null)
		{
			return dataContractFromSurrogateSelector;
		}
		if (cachedDataContract == null)
		{
			return cachedDataContract = DataContract.GetDataContract(typeHandle, type, SerializationMode.SharedType);
		}
		DataContract dataContract = cachedDataContract;
		if (dataContract.UnderlyingType.TypeHandle.Equals(typeHandle))
		{
			return dataContract;
		}
		return DataContract.GetDataContract(typeHandle, type, SerializationMode.SharedType);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[SecuritySafeCritical]
	[PermissionSet(SecurityAction.Demand, Unrestricted = true)]
	private static ISerializationSurrogate GetSurrogate(Type type, ISurrogateSelector surrogateSelector, StreamingContext context)
	{
		ISurrogateSelector selector;
		return surrogateSelector.GetSurrogate(type, context, out selector);
	}

	internal static DataContract GetDataContractFromSurrogateSelector(ISurrogateSelector surrogateSelector, StreamingContext context, RuntimeTypeHandle typeHandle, Type type, ref Hashtable surrogateDataContracts)
	{
		if (surrogateSelector == null)
		{
			return null;
		}
		if (type == null)
		{
			type = Type.GetTypeFromHandle(typeHandle);
		}
		DataContract builtInDataContract = DataContract.GetBuiltInDataContract(type);
		if (builtInDataContract != null)
		{
			return builtInDataContract;
		}
		if (surrogateDataContracts != null)
		{
			DataContract dataContract = (DataContract)surrogateDataContracts[type];
			if (dataContract != null)
			{
				return dataContract;
			}
		}
		DataContract dataContract2 = null;
		ISerializationSurrogate surrogate = GetSurrogate(type, surrogateSelector, context);
		if (surrogate != null)
		{
			dataContract2 = new SurrogateDataContract(type, surrogate);
		}
		else if (type.IsArray)
		{
			Type elementType = type.GetElementType();
			DataContract dataContract3 = GetDataContractFromSurrogateSelector(surrogateSelector, context, elementType.TypeHandle, elementType, ref surrogateDataContracts);
			if (dataContract3 == null)
			{
				dataContract3 = DataContract.GetDataContract(elementType.TypeHandle, elementType, SerializationMode.SharedType);
			}
			dataContract2 = new CollectionDataContract(type, dataContract3);
		}
		if (dataContract2 != null)
		{
			if (surrogateDataContracts == null)
			{
				surrogateDataContracts = new Hashtable();
			}
			surrogateDataContracts.Add(type, dataContract2);
			return dataContract2;
		}
		return null;
	}

	internal static TypeInformation GetTypeInformation(Type type)
	{
		TypeInformation typeInformation = null;
		object obj = typeNameCache[type];
		if (obj == null)
		{
			bool hasTypeForwardedFrom;
			string clrAssemblyName = DataContract.GetClrAssemblyName(type, out hasTypeForwardedFrom);
			typeInformation = new TypeInformation(DataContract.GetClrTypeFullNameUsingTypeForwardedFromAttribute(type), clrAssemblyName, hasTypeForwardedFrom);
			lock (typeNameCache)
			{
				typeNameCache[type] = typeInformation;
			}
		}
		else
		{
			typeInformation = (TypeInformation)obj;
		}
		return typeInformation;
	}

	private static bool IsAssemblyNameForwardingSafe(string originalAssemblyName, string newAssemblyName)
	{
		if (originalAssemblyName == newAssemblyName)
		{
			return true;
		}
		AssemblyName assemblyName = new AssemblyName(originalAssemblyName);
		AssemblyName assemblyName2 = new AssemblyName(newAssemblyName);
		if (string.Equals(assemblyName2.Name, "mscorlib", StringComparison.OrdinalIgnoreCase) || string.Equals(assemblyName2.Name, "mscorlib.dll", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		return IsPublicKeyTokenForwardingSafe(assemblyName.GetPublicKeyToken(), assemblyName2.GetPublicKeyToken());
	}

	private static bool IsPublicKeyTokenForwardingSafe(byte[] sourceToken, byte[] destinationToken)
	{
		if (sourceToken == null || destinationToken == null || sourceToken.Length == 0 || destinationToken.Length == 0 || sourceToken.Length != destinationToken.Length)
		{
			return false;
		}
		for (int i = 0; i < sourceToken.Length; i++)
		{
			if (sourceToken[i] != destinationToken[i])
			{
				return false;
			}
		}
		return true;
	}
}
