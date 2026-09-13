using System.Collections;
using System.Reflection;
using System.Security;
using System.Xml;

namespace System.Runtime.Serialization;

internal static class XmlFormatGeneratorStatics
{
	[SecurityCritical]
	private static MethodInfo writeStartElementMethod2;

	[SecurityCritical]
	private static MethodInfo writeStartElementMethod3;

	[SecurityCritical]
	private static MethodInfo writeEndElementMethod;

	[SecurityCritical]
	private static MethodInfo writeNamespaceDeclMethod;

	[SecurityCritical]
	private static PropertyInfo extensionDataProperty;

	[SecurityCritical]
	private static MethodInfo boxPointer;

	[SecurityCritical]
	private static ConstructorInfo dictionaryEnumeratorCtor;

	[SecurityCritical]
	private static MethodInfo ienumeratorMoveNextMethod;

	[SecurityCritical]
	private static MethodInfo ienumeratorGetCurrentMethod;

	[SecurityCritical]
	private static MethodInfo getItemContractMethod;

	[SecurityCritical]
	private static MethodInfo isStartElementMethod2;

	[SecurityCritical]
	private static MethodInfo isStartElementMethod0;

	[SecurityCritical]
	private static MethodInfo getUninitializedObjectMethod;

	[SecurityCritical]
	private static MethodInfo onDeserializationMethod;

	[SecurityCritical]
	private static MethodInfo unboxPointer;

	[SecurityCritical]
	private static PropertyInfo nodeTypeProperty;

	[SecurityCritical]
	private static ConstructorInfo serializationExceptionCtor;

	[SecurityCritical]
	private static ConstructorInfo extensionDataObjectCtor;

	[SecurityCritical]
	private static ConstructorInfo hashtableCtor;

	[SecurityCritical]
	private static MethodInfo getStreamingContextMethod;

	[SecurityCritical]
	private static MethodInfo getCollectionMemberMethod;

	[SecurityCritical]
	private static MethodInfo storeCollectionMemberInfoMethod;

	[SecurityCritical]
	private static MethodInfo storeIsGetOnlyCollectionMethod;

	[SecurityCritical]
	private static MethodInfo throwNullValueReturnedForGetOnlyCollectionExceptionMethod;

	private static MethodInfo throwArrayExceededSizeExceptionMethod;

	[SecurityCritical]
	private static MethodInfo incrementItemCountMethod;

	[SecurityCritical]
	private static MethodInfo demandSerializationFormatterPermissionMethod;

	[SecurityCritical]
	private static MethodInfo demandMemberAccessPermissionMethod;

	[SecurityCritical]
	private static MethodInfo internalDeserializeMethod;

	[SecurityCritical]
	private static MethodInfo moveToNextElementMethod;

	[SecurityCritical]
	private static MethodInfo getMemberIndexMethod;

	[SecurityCritical]
	private static MethodInfo getMemberIndexWithRequiredMembersMethod;

	[SecurityCritical]
	private static MethodInfo throwRequiredMemberMissingExceptionMethod;

	[SecurityCritical]
	private static MethodInfo skipUnknownElementMethod;

	[SecurityCritical]
	private static MethodInfo readIfNullOrRefMethod;

	[SecurityCritical]
	private static MethodInfo readAttributesMethod;

	[SecurityCritical]
	private static MethodInfo resetAttributesMethod;

	[SecurityCritical]
	private static MethodInfo getObjectIdMethod;

	[SecurityCritical]
	private static MethodInfo getArraySizeMethod;

	[SecurityCritical]
	private static MethodInfo addNewObjectMethod;

	[SecurityCritical]
	private static MethodInfo addNewObjectWithIdMethod;

	[SecurityCritical]
	private static MethodInfo replaceDeserializedObjectMethod;

	[SecurityCritical]
	private static MethodInfo getExistingObjectMethod;

	[SecurityCritical]
	private static MethodInfo getRealObjectMethod;

	[SecurityCritical]
	private static MethodInfo readMethod;

	[SecurityCritical]
	private static MethodInfo ensureArraySizeMethod;

	[SecurityCritical]
	private static MethodInfo trimArraySizeMethod;

	[SecurityCritical]
	private static MethodInfo checkEndOfArrayMethod;

	[SecurityCritical]
	private static MethodInfo getArrayLengthMethod;

	[SecurityCritical]
	private static MethodInfo readSerializationInfoMethod;

	[SecurityCritical]
	private static MethodInfo createUnexpectedStateExceptionMethod;

	[SecurityCritical]
	private static MethodInfo internalSerializeReferenceMethod;

	[SecurityCritical]
	private static MethodInfo internalSerializeMethod;

	[SecurityCritical]
	private static MethodInfo writeNullMethod;

	[SecurityCritical]
	private static MethodInfo incrementArrayCountMethod;

	[SecurityCritical]
	private static MethodInfo incrementCollectionCountMethod;

	[SecurityCritical]
	private static MethodInfo incrementCollectionCountGenericMethod;

	[SecurityCritical]
	private static MethodInfo getDefaultValueMethod;

	[SecurityCritical]
	private static MethodInfo getNullableValueMethod;

	[SecurityCritical]
	private static MethodInfo throwRequiredMemberMustBeEmittedMethod;

	[SecurityCritical]
	private static MethodInfo getHasValueMethod;

	[SecurityCritical]
	private static MethodInfo writeISerializableMethod;

	[SecurityCritical]
	private static MethodInfo writeExtensionDataMethod;

	[SecurityCritical]
	private static MethodInfo writeXmlValueMethod;

	[SecurityCritical]
	private static MethodInfo readXmlValueMethod;

	[SecurityCritical]
	private static MethodInfo throwTypeNotSerializableMethod;

	[SecurityCritical]
	private static PropertyInfo namespaceProperty;

	[SecurityCritical]
	private static FieldInfo contractNamespacesField;

	[SecurityCritical]
	private static FieldInfo memberNamesField;

	[SecurityCritical]
	private static MethodInfo extensionDataSetExplicitMethodInfo;

	[SecurityCritical]
	private static PropertyInfo childElementNamespacesProperty;

	[SecurityCritical]
	private static PropertyInfo collectionItemNameProperty;

	[SecurityCritical]
	private static PropertyInfo childElementNamespaceProperty;

	[SecurityCritical]
	private static MethodInfo getDateTimeOffsetMethod;

	[SecurityCritical]
	private static MethodInfo getDateTimeOffsetAdapterMethod;

	[SecurityCritical]
	private static MethodInfo traceInstructionMethod;

	[SecurityCritical]
	private static MethodInfo throwInvalidDataContractExceptionMethod;

	[SecurityCritical]
	private static PropertyInfo serializeReadOnlyTypesProperty;

	[SecurityCritical]
	private static PropertyInfo classSerializationExceptionMessageProperty;

	[SecurityCritical]
	private static PropertyInfo collectionSerializationExceptionMessageProperty;

	internal static MethodInfo WriteStartElementMethod2
	{
		[SecuritySafeCritical]
		get
		{
			if (writeStartElementMethod2 == null)
			{
				writeStartElementMethod2 = typeof(XmlWriterDelegator).GetMethod("WriteStartElement", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[2]
				{
					typeof(XmlDictionaryString),
					typeof(XmlDictionaryString)
				}, null);
			}
			return writeStartElementMethod2;
		}
	}

	internal static MethodInfo WriteStartElementMethod3
	{
		[SecuritySafeCritical]
		get
		{
			if (writeStartElementMethod3 == null)
			{
				writeStartElementMethod3 = typeof(XmlWriterDelegator).GetMethod("WriteStartElement", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[3]
				{
					typeof(string),
					typeof(XmlDictionaryString),
					typeof(XmlDictionaryString)
				}, null);
			}
			return writeStartElementMethod3;
		}
	}

	internal static MethodInfo WriteEndElementMethod
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

	internal static MethodInfo WriteNamespaceDeclMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (writeNamespaceDeclMethod == null)
			{
				writeNamespaceDeclMethod = typeof(XmlWriterDelegator).GetMethod("WriteNamespaceDecl", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[1] { typeof(XmlDictionaryString) }, null);
			}
			return writeNamespaceDeclMethod;
		}
	}

	internal static PropertyInfo ExtensionDataProperty
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

	internal static MethodInfo BoxPointer
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

	internal static ConstructorInfo DictionaryEnumeratorCtor
	{
		[SecuritySafeCritical]
		get
		{
			if (dictionaryEnumeratorCtor == null)
			{
				dictionaryEnumeratorCtor = Globals.TypeOfDictionaryEnumerator.GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[1] { Globals.TypeOfIDictionaryEnumerator }, null);
			}
			return dictionaryEnumeratorCtor;
		}
	}

	internal static MethodInfo MoveNextMethod
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

	internal static MethodInfo GetCurrentMethod
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

	internal static MethodInfo GetItemContractMethod
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

	internal static MethodInfo IsStartElementMethod2
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

	internal static MethodInfo IsStartElementMethod0
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

	internal static MethodInfo GetUninitializedObjectMethod
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

	internal static MethodInfo OnDeserializationMethod
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

	internal static MethodInfo UnboxPointer
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

	internal static PropertyInfo NodeTypeProperty
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

	internal static ConstructorInfo SerializationExceptionCtor
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

	internal static ConstructorInfo ExtensionDataObjectCtor
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

	internal static ConstructorInfo HashtableCtor
	{
		[SecuritySafeCritical]
		get
		{
			if (hashtableCtor == null)
			{
				hashtableCtor = Globals.TypeOfHashtable.GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, Globals.EmptyTypeArray, null);
			}
			return hashtableCtor;
		}
	}

	internal static MethodInfo GetStreamingContextMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (getStreamingContextMethod == null)
			{
				getStreamingContextMethod = typeof(XmlObjectSerializerContext).GetMethod("GetStreamingContext", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return getStreamingContextMethod;
		}
	}

	internal static MethodInfo GetCollectionMemberMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (getCollectionMemberMethod == null)
			{
				getCollectionMemberMethod = typeof(XmlObjectSerializerReadContext).GetMethod("GetCollectionMember", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return getCollectionMemberMethod;
		}
	}

	internal static MethodInfo StoreCollectionMemberInfoMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (storeCollectionMemberInfoMethod == null)
			{
				storeCollectionMemberInfoMethod = typeof(XmlObjectSerializerReadContext).GetMethod("StoreCollectionMemberInfo", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[1] { typeof(object) }, null);
			}
			return storeCollectionMemberInfoMethod;
		}
	}

	internal static MethodInfo StoreIsGetOnlyCollectionMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (storeIsGetOnlyCollectionMethod == null)
			{
				storeIsGetOnlyCollectionMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("StoreIsGetOnlyCollection", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return storeIsGetOnlyCollectionMethod;
		}
	}

	internal static MethodInfo ThrowNullValueReturnedForGetOnlyCollectionExceptionMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (throwNullValueReturnedForGetOnlyCollectionExceptionMethod == null)
			{
				throwNullValueReturnedForGetOnlyCollectionExceptionMethod = typeof(XmlObjectSerializerReadContext).GetMethod("ThrowNullValueReturnedForGetOnlyCollectionException", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return throwNullValueReturnedForGetOnlyCollectionExceptionMethod;
		}
	}

	internal static MethodInfo ThrowArrayExceededSizeExceptionMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (throwArrayExceededSizeExceptionMethod == null)
			{
				throwArrayExceededSizeExceptionMethod = typeof(XmlObjectSerializerReadContext).GetMethod("ThrowArrayExceededSizeException", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return throwArrayExceededSizeExceptionMethod;
		}
	}

	internal static MethodInfo IncrementItemCountMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (incrementItemCountMethod == null)
			{
				incrementItemCountMethod = typeof(XmlObjectSerializerContext).GetMethod("IncrementItemCount", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return incrementItemCountMethod;
		}
	}

	internal static MethodInfo DemandSerializationFormatterPermissionMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (demandSerializationFormatterPermissionMethod == null)
			{
				demandSerializationFormatterPermissionMethod = typeof(XmlObjectSerializerContext).GetMethod("DemandSerializationFormatterPermission", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return demandSerializationFormatterPermissionMethod;
		}
	}

	internal static MethodInfo DemandMemberAccessPermissionMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (demandMemberAccessPermissionMethod == null)
			{
				demandMemberAccessPermissionMethod = typeof(XmlObjectSerializerContext).GetMethod("DemandMemberAccessPermission", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return demandMemberAccessPermissionMethod;
		}
	}

	internal static MethodInfo InternalDeserializeMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (internalDeserializeMethod == null)
			{
				internalDeserializeMethod = typeof(XmlObjectSerializerReadContext).GetMethod("InternalDeserialize", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[5]
				{
					typeof(XmlReaderDelegator),
					typeof(int),
					typeof(RuntimeTypeHandle),
					typeof(string),
					typeof(string)
				}, null);
			}
			return internalDeserializeMethod;
		}
	}

	internal static MethodInfo MoveToNextElementMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (moveToNextElementMethod == null)
			{
				moveToNextElementMethod = typeof(XmlObjectSerializerReadContext).GetMethod("MoveToNextElement", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return moveToNextElementMethod;
		}
	}

	internal static MethodInfo GetMemberIndexMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (getMemberIndexMethod == null)
			{
				getMemberIndexMethod = typeof(XmlObjectSerializerReadContext).GetMethod("GetMemberIndex", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return getMemberIndexMethod;
		}
	}

	internal static MethodInfo GetMemberIndexWithRequiredMembersMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (getMemberIndexWithRequiredMembersMethod == null)
			{
				getMemberIndexWithRequiredMembersMethod = typeof(XmlObjectSerializerReadContext).GetMethod("GetMemberIndexWithRequiredMembers", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return getMemberIndexWithRequiredMembersMethod;
		}
	}

	internal static MethodInfo ThrowRequiredMemberMissingExceptionMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (throwRequiredMemberMissingExceptionMethod == null)
			{
				throwRequiredMemberMissingExceptionMethod = typeof(XmlObjectSerializerReadContext).GetMethod("ThrowRequiredMemberMissingException", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return throwRequiredMemberMissingExceptionMethod;
		}
	}

	internal static MethodInfo SkipUnknownElementMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (skipUnknownElementMethod == null)
			{
				skipUnknownElementMethod = typeof(XmlObjectSerializerReadContext).GetMethod("SkipUnknownElement", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return skipUnknownElementMethod;
		}
	}

	internal static MethodInfo ReadIfNullOrRefMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (readIfNullOrRefMethod == null)
			{
				readIfNullOrRefMethod = typeof(XmlObjectSerializerReadContext).GetMethod("ReadIfNullOrRef", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[3]
				{
					typeof(XmlReaderDelegator),
					typeof(Type),
					typeof(bool)
				}, null);
			}
			return readIfNullOrRefMethod;
		}
	}

	internal static MethodInfo ReadAttributesMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (readAttributesMethod == null)
			{
				readAttributesMethod = typeof(XmlObjectSerializerReadContext).GetMethod("ReadAttributes", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return readAttributesMethod;
		}
	}

	internal static MethodInfo ResetAttributesMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (resetAttributesMethod == null)
			{
				resetAttributesMethod = typeof(XmlObjectSerializerReadContext).GetMethod("ResetAttributes", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return resetAttributesMethod;
		}
	}

	internal static MethodInfo GetObjectIdMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (getObjectIdMethod == null)
			{
				getObjectIdMethod = typeof(XmlObjectSerializerReadContext).GetMethod("GetObjectId", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return getObjectIdMethod;
		}
	}

	internal static MethodInfo GetArraySizeMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (getArraySizeMethod == null)
			{
				getArraySizeMethod = typeof(XmlObjectSerializerReadContext).GetMethod("GetArraySize", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return getArraySizeMethod;
		}
	}

	internal static MethodInfo AddNewObjectMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (addNewObjectMethod == null)
			{
				addNewObjectMethod = typeof(XmlObjectSerializerReadContext).GetMethod("AddNewObject", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return addNewObjectMethod;
		}
	}

	internal static MethodInfo AddNewObjectWithIdMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (addNewObjectWithIdMethod == null)
			{
				addNewObjectWithIdMethod = typeof(XmlObjectSerializerReadContext).GetMethod("AddNewObjectWithId", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return addNewObjectWithIdMethod;
		}
	}

	internal static MethodInfo ReplaceDeserializedObjectMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (replaceDeserializedObjectMethod == null)
			{
				replaceDeserializedObjectMethod = typeof(XmlObjectSerializerReadContext).GetMethod("ReplaceDeserializedObject", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return replaceDeserializedObjectMethod;
		}
	}

	internal static MethodInfo GetExistingObjectMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (getExistingObjectMethod == null)
			{
				getExistingObjectMethod = typeof(XmlObjectSerializerReadContext).GetMethod("GetExistingObject", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return getExistingObjectMethod;
		}
	}

	internal static MethodInfo GetRealObjectMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (getRealObjectMethod == null)
			{
				getRealObjectMethod = typeof(XmlObjectSerializerReadContext).GetMethod("GetRealObject", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return getRealObjectMethod;
		}
	}

	internal static MethodInfo ReadMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (readMethod == null)
			{
				readMethod = typeof(XmlObjectSerializerReadContext).GetMethod("Read", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return readMethod;
		}
	}

	internal static MethodInfo EnsureArraySizeMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (ensureArraySizeMethod == null)
			{
				ensureArraySizeMethod = typeof(XmlObjectSerializerReadContext).GetMethod("EnsureArraySize", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return ensureArraySizeMethod;
		}
	}

	internal static MethodInfo TrimArraySizeMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (trimArraySizeMethod == null)
			{
				trimArraySizeMethod = typeof(XmlObjectSerializerReadContext).GetMethod("TrimArraySize", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return trimArraySizeMethod;
		}
	}

	internal static MethodInfo CheckEndOfArrayMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (checkEndOfArrayMethod == null)
			{
				checkEndOfArrayMethod = typeof(XmlObjectSerializerReadContext).GetMethod("CheckEndOfArray", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return checkEndOfArrayMethod;
		}
	}

	internal static MethodInfo GetArrayLengthMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (getArrayLengthMethod == null)
			{
				getArrayLengthMethod = Globals.TypeOfArray.GetProperty("Length").GetGetMethod();
			}
			return getArrayLengthMethod;
		}
	}

	internal static MethodInfo ReadSerializationInfoMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (readSerializationInfoMethod == null)
			{
				readSerializationInfoMethod = typeof(XmlObjectSerializerReadContext).GetMethod("ReadSerializationInfo", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return readSerializationInfoMethod;
		}
	}

	internal static MethodInfo CreateUnexpectedStateExceptionMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (createUnexpectedStateExceptionMethod == null)
			{
				createUnexpectedStateExceptionMethod = typeof(XmlObjectSerializerReadContext).GetMethod("CreateUnexpectedStateException", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[2]
				{
					typeof(XmlNodeType),
					typeof(XmlReaderDelegator)
				}, null);
			}
			return createUnexpectedStateExceptionMethod;
		}
	}

	internal static MethodInfo InternalSerializeReferenceMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (internalSerializeReferenceMethod == null)
			{
				internalSerializeReferenceMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("InternalSerializeReference", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return internalSerializeReferenceMethod;
		}
	}

	internal static MethodInfo InternalSerializeMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (internalSerializeMethod == null)
			{
				internalSerializeMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("InternalSerialize", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return internalSerializeMethod;
		}
	}

	internal static MethodInfo WriteNullMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (writeNullMethod == null)
			{
				writeNullMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("WriteNull", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[3]
				{
					typeof(XmlWriterDelegator),
					typeof(Type),
					typeof(bool)
				}, null);
			}
			return writeNullMethod;
		}
	}

	internal static MethodInfo IncrementArrayCountMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (incrementArrayCountMethod == null)
			{
				incrementArrayCountMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("IncrementArrayCount", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return incrementArrayCountMethod;
		}
	}

	internal static MethodInfo IncrementCollectionCountMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (incrementCollectionCountMethod == null)
			{
				incrementCollectionCountMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("IncrementCollectionCount", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[2]
				{
					typeof(XmlWriterDelegator),
					typeof(ICollection)
				}, null);
			}
			return incrementCollectionCountMethod;
		}
	}

	internal static MethodInfo IncrementCollectionCountGenericMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (incrementCollectionCountGenericMethod == null)
			{
				incrementCollectionCountGenericMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("IncrementCollectionCountGeneric", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return incrementCollectionCountGenericMethod;
		}
	}

	internal static MethodInfo GetDefaultValueMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (getDefaultValueMethod == null)
			{
				getDefaultValueMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("GetDefaultValue", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return getDefaultValueMethod;
		}
	}

	internal static MethodInfo GetNullableValueMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (getNullableValueMethod == null)
			{
				getNullableValueMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("GetNullableValue", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return getNullableValueMethod;
		}
	}

	internal static MethodInfo ThrowRequiredMemberMustBeEmittedMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (throwRequiredMemberMustBeEmittedMethod == null)
			{
				throwRequiredMemberMustBeEmittedMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("ThrowRequiredMemberMustBeEmitted", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return throwRequiredMemberMustBeEmittedMethod;
		}
	}

	internal static MethodInfo GetHasValueMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (getHasValueMethod == null)
			{
				getHasValueMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("GetHasValue", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return getHasValueMethod;
		}
	}

	internal static MethodInfo WriteISerializableMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (writeISerializableMethod == null)
			{
				writeISerializableMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("WriteISerializable", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return writeISerializableMethod;
		}
	}

	internal static MethodInfo WriteExtensionDataMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (writeExtensionDataMethod == null)
			{
				writeExtensionDataMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("WriteExtensionData", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return writeExtensionDataMethod;
		}
	}

	internal static MethodInfo WriteXmlValueMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (writeXmlValueMethod == null)
			{
				writeXmlValueMethod = typeof(DataContract).GetMethod("WriteXmlValue", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return writeXmlValueMethod;
		}
	}

	internal static MethodInfo ReadXmlValueMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (readXmlValueMethod == null)
			{
				readXmlValueMethod = typeof(DataContract).GetMethod("ReadXmlValue", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return readXmlValueMethod;
		}
	}

	internal static MethodInfo ThrowTypeNotSerializableMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (throwTypeNotSerializableMethod == null)
			{
				throwTypeNotSerializableMethod = typeof(DataContract).GetMethod("ThrowTypeNotSerializable", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return throwTypeNotSerializableMethod;
		}
	}

	internal static PropertyInfo NamespaceProperty
	{
		[SecuritySafeCritical]
		get
		{
			if (namespaceProperty == null)
			{
				namespaceProperty = typeof(DataContract).GetProperty("Namespace", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return namespaceProperty;
		}
	}

	internal static FieldInfo ContractNamespacesField
	{
		[SecuritySafeCritical]
		get
		{
			if (contractNamespacesField == null)
			{
				contractNamespacesField = typeof(ClassDataContract).GetField("ContractNamespaces", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return contractNamespacesField;
		}
	}

	internal static FieldInfo MemberNamesField
	{
		[SecuritySafeCritical]
		get
		{
			if (memberNamesField == null)
			{
				memberNamesField = typeof(ClassDataContract).GetField("MemberNames", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return memberNamesField;
		}
	}

	internal static MethodInfo ExtensionDataSetExplicitMethodInfo
	{
		[SecuritySafeCritical]
		get
		{
			if (extensionDataSetExplicitMethodInfo == null)
			{
				extensionDataSetExplicitMethodInfo = typeof(IExtensibleDataObject).GetMethod("set_ExtensionData");
			}
			return extensionDataSetExplicitMethodInfo;
		}
	}

	internal static PropertyInfo ChildElementNamespacesProperty
	{
		[SecuritySafeCritical]
		get
		{
			if (childElementNamespacesProperty == null)
			{
				childElementNamespacesProperty = typeof(ClassDataContract).GetProperty("ChildElementNamespaces", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return childElementNamespacesProperty;
		}
	}

	internal static PropertyInfo CollectionItemNameProperty
	{
		[SecuritySafeCritical]
		get
		{
			if (collectionItemNameProperty == null)
			{
				collectionItemNameProperty = typeof(CollectionDataContract).GetProperty("CollectionItemName", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return collectionItemNameProperty;
		}
	}

	internal static PropertyInfo ChildElementNamespaceProperty
	{
		[SecuritySafeCritical]
		get
		{
			if (childElementNamespaceProperty == null)
			{
				childElementNamespaceProperty = typeof(CollectionDataContract).GetProperty("ChildElementNamespace", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return childElementNamespaceProperty;
		}
	}

	internal static MethodInfo GetDateTimeOffsetMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (getDateTimeOffsetMethod == null)
			{
				getDateTimeOffsetMethod = typeof(DateTimeOffsetAdapter).GetMethod("GetDateTimeOffset", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return getDateTimeOffsetMethod;
		}
	}

	internal static MethodInfo GetDateTimeOffsetAdapterMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (getDateTimeOffsetAdapterMethod == null)
			{
				getDateTimeOffsetAdapterMethod = typeof(DateTimeOffsetAdapter).GetMethod("GetDateTimeOffsetAdapter", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return getDateTimeOffsetAdapterMethod;
		}
	}

	internal static MethodInfo TraceInstructionMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (traceInstructionMethod == null)
			{
				traceInstructionMethod = typeof(SerializationTrace).GetMethod("TraceInstruction", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return traceInstructionMethod;
		}
	}

	internal static MethodInfo ThrowInvalidDataContractExceptionMethod
	{
		[SecuritySafeCritical]
		get
		{
			if (throwInvalidDataContractExceptionMethod == null)
			{
				throwInvalidDataContractExceptionMethod = typeof(DataContract).GetMethod("ThrowInvalidDataContractException", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[2]
				{
					typeof(string),
					typeof(Type)
				}, null);
			}
			return throwInvalidDataContractExceptionMethod;
		}
	}

	internal static PropertyInfo SerializeReadOnlyTypesProperty
	{
		[SecuritySafeCritical]
		get
		{
			if (serializeReadOnlyTypesProperty == null)
			{
				serializeReadOnlyTypesProperty = typeof(XmlObjectSerializerWriteContext).GetProperty("SerializeReadOnlyTypes", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return serializeReadOnlyTypesProperty;
		}
	}

	internal static PropertyInfo ClassSerializationExceptionMessageProperty
	{
		[SecuritySafeCritical]
		get
		{
			if (classSerializationExceptionMessageProperty == null)
			{
				classSerializationExceptionMessageProperty = typeof(ClassDataContract).GetProperty("SerializationExceptionMessage", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return classSerializationExceptionMessageProperty;
		}
	}

	internal static PropertyInfo CollectionSerializationExceptionMessageProperty
	{
		[SecuritySafeCritical]
		get
		{
			if (collectionSerializationExceptionMessageProperty == null)
			{
				collectionSerializationExceptionMessageProperty = typeof(CollectionDataContract).GetProperty("SerializationExceptionMessage", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return collectionSerializationExceptionMessageProperty;
		}
	}
}
