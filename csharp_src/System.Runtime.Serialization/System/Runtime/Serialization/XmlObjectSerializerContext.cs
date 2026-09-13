using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization.Diagnostics.Application;
using System.Security;
using System.Xml;

namespace System.Runtime.Serialization;

internal class XmlObjectSerializerContext
{
	protected XmlObjectSerializer serializer;

	protected DataContract rootTypeDataContract;

	internal ScopedKnownTypes scopedKnownTypes;

	protected Dictionary<XmlQualifiedName, DataContract> serializerKnownDataContracts;

	private bool isSerializerKnownDataContractsSetExplicit;

	protected IList<Type> serializerKnownTypeList;

	[SecurityCritical]
	private bool demandedSerializationFormatterPermission;

	[SecurityCritical]
	private bool demandedMemberAccessPermission;

	private int itemCount;

	private int maxItemsInObjectGraph;

	private StreamingContext streamingContext;

	private bool ignoreExtensionDataObject;

	private DataContractResolver dataContractResolver;

	private KnownTypeDataContractResolver knownTypeResolver;

	private static MethodInfo incrementItemCountMethod;

	internal virtual SerializationMode Mode => SerializationMode.SharedContract;

	internal virtual bool IsGetOnlyCollection
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	internal static MethodInfo IncrementItemCountMethod
	{
		get
		{
			if (incrementItemCountMethod == null)
			{
				incrementItemCountMethod = typeof(XmlObjectSerializerContext).GetMethod("IncrementItemCount", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return incrementItemCountMethod;
		}
	}

	internal int RemainingItemCount => maxItemsInObjectGraph - itemCount;

	internal bool IgnoreExtensionDataObject => ignoreExtensionDataObject;

	protected DataContractResolver DataContractResolver => dataContractResolver;

	protected KnownTypeDataContractResolver KnownTypeResolver
	{
		get
		{
			if (knownTypeResolver == null)
			{
				knownTypeResolver = new KnownTypeDataContractResolver(this);
			}
			return knownTypeResolver;
		}
	}

	private Dictionary<XmlQualifiedName, DataContract> SerializerKnownDataContracts
	{
		get
		{
			if (!isSerializerKnownDataContractsSetExplicit)
			{
				serializerKnownDataContracts = serializer.KnownDataContracts;
				isSerializerKnownDataContractsSetExplicit = true;
			}
			return serializerKnownDataContracts;
		}
	}

	internal XmlObjectSerializerContext(XmlObjectSerializer serializer, int maxItemsInObjectGraph, StreamingContext streamingContext, bool ignoreExtensionDataObject, DataContractResolver dataContractResolver)
	{
		this.serializer = serializer;
		itemCount = 1;
		this.maxItemsInObjectGraph = maxItemsInObjectGraph;
		this.streamingContext = streamingContext;
		this.ignoreExtensionDataObject = ignoreExtensionDataObject;
		this.dataContractResolver = dataContractResolver;
	}

	internal XmlObjectSerializerContext(XmlObjectSerializer serializer, int maxItemsInObjectGraph, StreamingContext streamingContext, bool ignoreExtensionDataObject)
		: this(serializer, maxItemsInObjectGraph, streamingContext, ignoreExtensionDataObject, null)
	{
	}

	internal XmlObjectSerializerContext(DataContractSerializer serializer, DataContract rootTypeDataContract, DataContractResolver dataContractResolver)
		: this(serializer, serializer.MaxItemsInObjectGraph, new StreamingContext(StreamingContextStates.All), serializer.IgnoreExtensionDataObject, dataContractResolver)
	{
		this.rootTypeDataContract = rootTypeDataContract;
		serializerKnownTypeList = serializer.knownTypeList;
	}

	internal XmlObjectSerializerContext(NetDataContractSerializer serializer)
		: this(serializer, serializer.MaxItemsInObjectGraph, serializer.Context, serializer.IgnoreExtensionDataObject)
	{
	}

	[SecuritySafeCritical]
	public void DemandSerializationFormatterPermission()
	{
	}

	[SecuritySafeCritical]
	public void DemandMemberAccessPermission()
	{
	}

	public StreamingContext GetStreamingContext()
	{
		return streamingContext;
	}

	public void IncrementItemCount(int count)
	{
		if (count > maxItemsInObjectGraph - itemCount)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Maximum number of items that can be serialized or deserialized in an object graph is '{0}'.", maxItemsInObjectGraph)));
		}
		itemCount += count;
	}

	internal DataContract GetDataContract(Type type)
	{
		return GetDataContract(type.TypeHandle, type);
	}

	internal virtual DataContract GetDataContract(RuntimeTypeHandle typeHandle, Type type)
	{
		if (IsGetOnlyCollection)
		{
			return DataContract.GetGetOnlyCollectionDataContract(DataContract.GetId(typeHandle), typeHandle, type, Mode);
		}
		return DataContract.GetDataContract(typeHandle, type, Mode);
	}

	internal virtual DataContract GetDataContractSkipValidation(int typeId, RuntimeTypeHandle typeHandle, Type type)
	{
		if (IsGetOnlyCollection)
		{
			return DataContract.GetGetOnlyCollectionDataContractSkipValidation(typeId, typeHandle, type);
		}
		return DataContract.GetDataContractSkipValidation(typeId, typeHandle, type);
	}

	internal virtual DataContract GetDataContract(int id, RuntimeTypeHandle typeHandle)
	{
		if (IsGetOnlyCollection)
		{
			return DataContract.GetGetOnlyCollectionDataContract(id, typeHandle, null, Mode);
		}
		return DataContract.GetDataContract(id, typeHandle, Mode);
	}

	internal virtual void CheckIfTypeSerializable(Type memberType, bool isMemberTypeSerializable)
	{
		if (!isMemberTypeSerializable)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot be serialized. Consider marking it with the DataContractAttribute attribute, and marking all of its members you want serialized with the DataMemberAttribute attribute. Alternatively, you can ensure that the type is public and has a parameterless constructor - all public members of the type will then be serialized, and no attributes will be required.", memberType)));
		}
	}

	internal virtual Type GetSurrogatedType(Type type)
	{
		return type;
	}

	private DataContract GetDataContractFromSerializerKnownTypes(XmlQualifiedName qname)
	{
		Dictionary<XmlQualifiedName, DataContract> dictionary = SerializerKnownDataContracts;
		if (dictionary == null)
		{
			return null;
		}
		if (!dictionary.TryGetValue(qname, out var value))
		{
			return null;
		}
		return value;
	}

	internal static Dictionary<XmlQualifiedName, DataContract> GetDataContractsForKnownTypes(IList<Type> knownTypeList)
	{
		if (knownTypeList == null)
		{
			return null;
		}
		Dictionary<XmlQualifiedName, DataContract> nameToDataContractTable = new Dictionary<XmlQualifiedName, DataContract>();
		Dictionary<Type, Type> typesChecked = new Dictionary<Type, Type>();
		for (int i = 0; i < knownTypeList.Count; i++)
		{
			Type type = knownTypeList[i];
			if (type == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("One of the known types provided to the serializer via '{0}' argument was invalid because it was null. All known types specified must be non-null values.", "knownTypes")));
			}
			DataContract.CheckAndAdd(type, typesChecked, ref nameToDataContractTable);
		}
		return nameToDataContractTable;
	}

	internal bool IsKnownType(DataContract dataContract, Dictionary<XmlQualifiedName, DataContract> knownDataContracts, Type declaredType)
	{
		bool flag = false;
		if (knownDataContracts != null)
		{
			scopedKnownTypes.Push(knownDataContracts);
			flag = true;
		}
		bool result = IsKnownType(dataContract, declaredType);
		if (flag)
		{
			scopedKnownTypes.Pop();
		}
		return result;
	}

	internal bool IsKnownType(DataContract dataContract, Type declaredType)
	{
		DataContract dataContract2 = ResolveDataContractFromKnownTypes(dataContract.StableName.Name, dataContract.StableName.Namespace, null, declaredType);
		if (dataContract2 != null)
		{
			return dataContract2.UnderlyingType == dataContract.UnderlyingType;
		}
		return false;
	}

	private DataContract ResolveDataContractFromKnownTypes(XmlQualifiedName typeName)
	{
		DataContract dataContract = PrimitiveDataContract.GetPrimitiveDataContract(typeName.Name, typeName.Namespace);
		if (dataContract == null)
		{
			dataContract = scopedKnownTypes.GetDataContract(typeName);
			if (dataContract == null)
			{
				dataContract = GetDataContractFromSerializerKnownTypes(typeName);
			}
		}
		return dataContract;
	}

	private DataContract ResolveDataContractFromDataContractResolver(XmlQualifiedName typeName, Type declaredType)
	{
		if (TD.DCResolverResolveIsEnabled())
		{
			TD.DCResolverResolve(typeName.Name + ":" + typeName.Namespace);
		}
		Type type = DataContractResolver.ResolveName(typeName.Name, typeName.Namespace, declaredType, KnownTypeResolver);
		if (type == null)
		{
			return null;
		}
		return GetDataContract(type);
	}

	internal Type ResolveNameFromKnownTypes(XmlQualifiedName typeName)
	{
		return ResolveDataContractFromKnownTypes(typeName)?.OriginalUnderlyingType;
	}

	protected DataContract ResolveDataContractFromKnownTypes(string typeName, string typeNs, DataContract memberTypeContract, Type declaredType)
	{
		XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(typeName, typeNs);
		DataContract dataContract = ((DataContractResolver != null) ? ResolveDataContractFromDataContractResolver(xmlQualifiedName, declaredType) : ResolveDataContractFromKnownTypes(xmlQualifiedName));
		if (dataContract == null)
		{
			if (memberTypeContract != null && !memberTypeContract.UnderlyingType.IsInterface && memberTypeContract.StableName == xmlQualifiedName)
			{
				dataContract = memberTypeContract;
			}
			if (dataContract == null && rootTypeDataContract != null)
			{
				dataContract = ResolveDataContractFromRootDataContract(xmlQualifiedName);
			}
		}
		return dataContract;
	}

	protected virtual DataContract ResolveDataContractFromRootDataContract(XmlQualifiedName typeQName)
	{
		if (rootTypeDataContract.StableName == typeQName)
		{
			return rootTypeDataContract;
		}
		CollectionDataContract collectionDataContract = rootTypeDataContract as CollectionDataContract;
		while (collectionDataContract != null)
		{
			DataContract dataContract = GetDataContract(GetSurrogatedType(collectionDataContract.ItemType));
			if (dataContract.StableName == typeQName)
			{
				return dataContract;
			}
			collectionDataContract = dataContract as CollectionDataContract;
		}
		return null;
	}
}
