using System.Collections;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Security;
using System.Xml;

namespace System.Runtime.Serialization;

internal static class JsonFormatGeneratorStatics
{
	[SecurityCritical]
	private static MethodInfo boxPointer;

	[SecurityCritical]
	private static PropertyInfo collectionItemNameProperty;

	[SecurityCritical]
	private static ConstructorInfo extensionDataObjectCtor;

	[SecurityCritical]
	private static PropertyInfo extensionDataProperty;

	[SecurityCritical]
	private static MethodInfo getItemContractMethod;

	[SecurityCritical]
	private static MethodInfo getJsonDataContractMethod;

	[SecurityCritical]
	private static MethodInfo getJsonMemberIndexMethod;

	[SecurityCritical]
	private static MethodInfo getRevisedItemContractMethod;

	[SecurityCritical]
	private static MethodInfo getUninitializedObjectMethod;

	[SecurityCritical]
	private static MethodInfo ienumeratorGetCurrentMethod;

	[SecurityCritical]
	private static MethodInfo ienumeratorMoveNextMethod;

	[SecurityCritical]
	private static MethodInfo isStartElementMethod0;

	[SecurityCritical]
	private static MethodInfo isStartElementMethod2;

	[SecurityCritical]
	private static PropertyInfo localNameProperty;

	[SecurityCritical]
	private static PropertyInfo namespaceProperty;

	[SecurityCritical]
	private static MethodInfo moveToContentMethod;

	[SecurityCritical]
	private static PropertyInfo nodeTypeProperty;

	[SecurityCritical]
	private static MethodInfo onDeserializationMethod;

	[SecurityCritical]
	private static MethodInfo readJsonValueMethod;

	[SecurityCritical]
	private static ConstructorInfo serializationExceptionCtor;

	[SecurityCritical]
	private static Type[] serInfoCtorArgs;

	[SecurityCritical]
	private static MethodInfo throwDuplicateMemberExceptionMethod;

	[SecurityCritical]
	private static MethodInfo throwMissingRequiredMembersMethod;

	[SecurityCritical]
	private static PropertyInfo typeHandleProperty;

	[SecurityCritical]
	private static MethodInfo unboxPointer;

	[SecurityCritical]
	private static PropertyInfo useSimpleDictionaryFormatReadProperty;

	[SecurityCritical]
	private static PropertyInfo useSimpleDictionaryFormatWriteProperty;

	[SecurityCritical]
	private static MethodInfo writeAttributeStringMethod;

	[SecurityCritical]
	private static MethodInfo writeEndElementMethod;

	[SecurityCritical]
	private static MethodInfo writeJsonISerializableMethod;

	[SecurityCritical]
	private static MethodInfo writeJsonNameWithMappingMethod;

	[SecurityCritical]
	private static MethodInfo writeJsonValueMethod;

	[SecurityCritical]
	private static MethodInfo writeStartElementMethod;

	[SecurityCritical]
	private static MethodInfo writeStartElementStringMethod;

	[SecurityCritical]
	private static MethodInfo parseEnumMethod;

	[SecurityCritical]
	private static MethodInfo getJsonMemberNameMethod;

	public static MethodInfo BoxPointer
	{
		[SecuritySafeCritical]
		get
		{
			if (boxPointer == null)
			{
				boxPointer = typeof(Pointer).GetMethod("Box");
			}
			return boxPointer;
		}
	}

	public static PropertyInfo CollectionItemNameProperty
	{
		[SecuritySafeCritical]
		get
		{
			if (collectionItemNameProperty == null)
			{
				collectionItemNameProperty = typeof(XmlObjectSerializerWriteContextComplexJson).GetProperty("CollectionItemName", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return collectionItemNameProperty;
		}
	}

	public static ConstructorInfo ExtensionDataObjectCtor
	{
		[SecuritySafeCritical]
		get
		{
			if (extensionDataObjectCtor == null)
			{
				extensionDataObjectCtor = typeof(ExtensionDataObject).GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
			}
			return extensionDataObjectCtor;
		}
	}

	public static PropertyInfo ExtensionDataProperty
	{
		[SecuritySafeCritical]
		get
		{
			if (extensionDataProperty == null)
			{
				extensionDataProperty = typeof(IExtensibleDataObject).GetProperty("ExtensionData");
			}
			return extensionDataProperty;
		}
	}

	public static MethodInfo GetCurrentMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (ienumeratorGetCurrentMethod == null)
			{
				ienumeratorGetCurrentMethod = typeof(IEnumerator).GetProperty("Current").GetGetMethod();
			}
			return ienumeratorGetCurrentMethod;
		}
	}

	public static MethodInfo GetItemContractMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (getItemContractMethod == null)
			{
				getItemContractMethod = typeof(CollectionDataContract).GetProperty("ItemContract", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetGetMethod(nonPublic: true);
			}
			return getItemContractMethod;
		}
	}

	public static MethodInfo GetJsonDataContractMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (getJsonDataContractMethod == null)
			{
				getJsonDataContractMethod = typeof(JsonDataContract).GetMethod("GetJsonDataContract", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return getJsonDataContractMethod;
		}
	}

	public static MethodInfo GetJsonMemberIndexMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (getJsonMemberIndexMethod == null)
			{
				getJsonMemberIndexMethod = typeof(XmlObjectSerializerReadContextComplexJson).GetMethod("GetJsonMemberIndex", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return getJsonMemberIndexMethod;
		}
	}

	public static MethodInfo GetRevisedItemContractMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (getRevisedItemContractMethod == null)
			{
				getRevisedItemContractMethod = typeof(XmlObjectSerializerWriteContextComplexJson).GetMethod("GetRevisedItemContract", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return getRevisedItemContractMethod;
		}
	}

	public static MethodInfo GetUninitializedObjectMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (getUninitializedObjectMethod == null)
			{
				getUninitializedObjectMethod = typeof(XmlFormatReaderGenerator).GetMethod("UnsafeGetUninitializedObject", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[1] { typeof(int) }, null);
			}
			return getUninitializedObjectMethod;
		}
	}

	public static MethodInfo IsStartElementMethod0
	{
		[SecuritySafeCritical]
		get
		{
			if (isStartElementMethod0 == null)
			{
				isStartElementMethod0 = typeof(XmlReaderDelegator).GetMethod("IsStartElement", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
			}
			return isStartElementMethod0;
		}
	}

	public static MethodInfo IsStartElementMethod2
	{
		[SecuritySafeCritical]
		get
		{
			if (isStartElementMethod2 == null)
			{
				isStartElementMethod2 = typeof(XmlReaderDelegator).GetMethod("IsStartElement", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[2]
				{
					typeof(XmlDictionaryString),
					typeof(XmlDictionaryString)
				}, null);
			}
			return isStartElementMethod2;
		}
	}

	public static PropertyInfo LocalNameProperty
	{
		[SecuritySafeCritical]
		get
		{
			if (localNameProperty == null)
			{
				localNameProperty = typeof(XmlReaderDelegator).GetProperty("LocalName", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return localNameProperty;
		}
	}

	public static PropertyInfo NamespaceProperty
	{
		[SecuritySafeCritical]
		get
		{
			if (namespaceProperty == null)
			{
				namespaceProperty = typeof(XmlReaderDelegator).GetProperty("NamespaceProperty", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return namespaceProperty;
		}
	}

	public static MethodInfo MoveNextMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (ienumeratorMoveNextMethod == null)
			{
				ienumeratorMoveNextMethod = typeof(IEnumerator).GetMethod("MoveNext");
			}
			return ienumeratorMoveNextMethod;
		}
	}

	public static MethodInfo MoveToContentMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (moveToContentMethod == null)
			{
				moveToContentMethod = typeof(XmlReaderDelegator).GetMethod("MoveToContent", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return moveToContentMethod;
		}
	}

	public static PropertyInfo NodeTypeProperty
	{
		[SecuritySafeCritical]
		get
		{
			if (nodeTypeProperty == null)
			{
				nodeTypeProperty = typeof(XmlReaderDelegator).GetProperty("NodeType", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return nodeTypeProperty;
		}
	}

	public static MethodInfo OnDeserializationMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (onDeserializationMethod == null)
			{
				onDeserializationMethod = typeof(IDeserializationCallback).GetMethod("OnDeserialization");
			}
			return onDeserializationMethod;
		}
	}

	public static MethodInfo ReadJsonValueMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (readJsonValueMethod == null)
			{
				readJsonValueMethod = typeof(DataContractJsonSerializer).GetMethod("ReadJsonValue", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return readJsonValueMethod;
		}
	}

	public static ConstructorInfo SerializationExceptionCtor
	{
		[SecuritySafeCritical]
		get
		{
			if (serializationExceptionCtor == null)
			{
				serializationExceptionCtor = typeof(SerializationException).GetConstructor(new Type[1] { typeof(string) });
			}
			return serializationExceptionCtor;
		}
	}

	public static Type[] SerInfoCtorArgs
	{
		[SecuritySafeCritical]
		get
		{
			if (serInfoCtorArgs == null)
			{
				serInfoCtorArgs = new Type[2]
				{
					typeof(SerializationInfo),
					typeof(StreamingContext)
				};
			}
			return serInfoCtorArgs;
		}
	}

	public static MethodInfo ThrowDuplicateMemberExceptionMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (throwDuplicateMemberExceptionMethod == null)
			{
				throwDuplicateMemberExceptionMethod = typeof(XmlObjectSerializerReadContextComplexJson).GetMethod("ThrowDuplicateMemberException", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return throwDuplicateMemberExceptionMethod;
		}
	}

	public static MethodInfo ThrowMissingRequiredMembersMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (throwMissingRequiredMembersMethod == null)
			{
				throwMissingRequiredMembersMethod = typeof(XmlObjectSerializerReadContextComplexJson).GetMethod("ThrowMissingRequiredMembers", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return throwMissingRequiredMembersMethod;
		}
	}

	public static PropertyInfo TypeHandleProperty
	{
		[SecuritySafeCritical]
		get
		{
			if (typeHandleProperty == null)
			{
				typeHandleProperty = typeof(Type).GetProperty("TypeHandle");
			}
			return typeHandleProperty;
		}
	}

	public static MethodInfo UnboxPointer
	{
		[SecuritySafeCritical]
		get
		{
			if (unboxPointer == null)
			{
				unboxPointer = typeof(Pointer).GetMethod("Unbox");
			}
			return unboxPointer;
		}
	}

	public static PropertyInfo UseSimpleDictionaryFormatReadProperty
	{
		[SecuritySafeCritical]
		get
		{
			if (useSimpleDictionaryFormatReadProperty == null)
			{
				useSimpleDictionaryFormatReadProperty = typeof(XmlObjectSerializerReadContextComplexJson).GetProperty("UseSimpleDictionaryFormat", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return useSimpleDictionaryFormatReadProperty;
		}
	}

	public static PropertyInfo UseSimpleDictionaryFormatWriteProperty
	{
		[SecuritySafeCritical]
		get
		{
			if (useSimpleDictionaryFormatWriteProperty == null)
			{
				useSimpleDictionaryFormatWriteProperty = typeof(XmlObjectSerializerWriteContextComplexJson).GetProperty("UseSimpleDictionaryFormat", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return useSimpleDictionaryFormatWriteProperty;
		}
	}

	public static MethodInfo WriteAttributeStringMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (writeAttributeStringMethod == null)
			{
				writeAttributeStringMethod = typeof(XmlWriterDelegator).GetMethod("WriteAttributeString", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[4]
				{
					typeof(string),
					typeof(string),
					typeof(string),
					typeof(string)
				}, null);
			}
			return writeAttributeStringMethod;
		}
	}

	public static MethodInfo WriteEndElementMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (writeEndElementMethod == null)
			{
				writeEndElementMethod = typeof(XmlWriterDelegator).GetMethod("WriteEndElement", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
			}
			return writeEndElementMethod;
		}
	}

	public static MethodInfo WriteJsonISerializableMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (writeJsonISerializableMethod == null)
			{
				writeJsonISerializableMethod = typeof(XmlObjectSerializerWriteContextComplexJson).GetMethod("WriteJsonISerializable", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return writeJsonISerializableMethod;
		}
	}

	public static MethodInfo WriteJsonNameWithMappingMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (writeJsonNameWithMappingMethod == null)
			{
				writeJsonNameWithMappingMethod = typeof(XmlObjectSerializerWriteContextComplexJson).GetMethod("WriteJsonNameWithMapping", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return writeJsonNameWithMappingMethod;
		}
	}

	public static MethodInfo WriteJsonValueMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (writeJsonValueMethod == null)
			{
				writeJsonValueMethod = typeof(DataContractJsonSerializer).GetMethod("WriteJsonValue", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return writeJsonValueMethod;
		}
	}

	public static MethodInfo WriteStartElementMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (writeStartElementMethod == null)
			{
				writeStartElementMethod = typeof(XmlWriterDelegator).GetMethod("WriteStartElement", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[2]
				{
					typeof(XmlDictionaryString),
					typeof(XmlDictionaryString)
				}, null);
			}
			return writeStartElementMethod;
		}
	}

	public static MethodInfo WriteStartElementStringMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (writeStartElementStringMethod == null)
			{
				writeStartElementStringMethod = typeof(XmlWriterDelegator).GetMethod("WriteStartElement", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[2]
				{
					typeof(string),
					typeof(string)
				}, null);
			}
			return writeStartElementStringMethod;
		}
	}

	public static MethodInfo ParseEnumMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (parseEnumMethod == null)
			{
				parseEnumMethod = typeof(Enum).GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new Type[2]
				{
					typeof(Type),
					typeof(string)
				}, null);
			}
			return parseEnumMethod;
		}
	}

	public static MethodInfo GetJsonMemberNameMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (getJsonMemberNameMethod == null)
			{
				getJsonMemberNameMethod = typeof(XmlObjectSerializerReadContextComplexJson).GetMethod("GetJsonMemberName", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[1] { typeof(XmlReaderDelegator) }, null);
			}
			return getJsonMemberNameMethod;
		}
	}
}
