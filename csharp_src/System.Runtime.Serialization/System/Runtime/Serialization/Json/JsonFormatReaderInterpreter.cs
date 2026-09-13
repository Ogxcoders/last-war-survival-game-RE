using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace System.Runtime.Serialization.Json;

internal class JsonFormatReaderInterpreter
{
	private enum KeyParseMode
	{
		Fail,
		AsString,
		UsingParseEnum,
		UsingCustomParse
	}

	private bool is_get_only_collection;

	private ClassDataContract classContract;

	private CollectionDataContract collectionContract;

	private object objectLocal;

	private Type objectType;

	private XmlReaderDelegator xmlReader;

	private XmlObjectSerializerReadContextComplexJson context;

	private XmlDictionaryString[] memberNames;

	private XmlDictionaryString emptyDictionaryString;

	private XmlDictionaryString itemName;

	private XmlDictionaryString itemNamespace;

	public JsonFormatReaderInterpreter(ClassDataContract classContract)
	{
		this.classContract = classContract;
	}

	public JsonFormatReaderInterpreter(CollectionDataContract collectionContract, bool isGetOnly)
	{
		this.collectionContract = collectionContract;
		is_get_only_collection = isGetOnly;
	}

	public object ReadFromJson(XmlReaderDelegator xmlReader, XmlObjectSerializerReadContextComplexJson context, XmlDictionaryString emptyDictionaryString, XmlDictionaryString[] memberNames)
	{
		this.xmlReader = xmlReader;
		this.context = context;
		this.emptyDictionaryString = emptyDictionaryString;
		this.memberNames = memberNames;
		CreateObject(classContract);
		context.AddNewObject(objectLocal);
		InvokeOnDeserializing(classContract);
		if (classContract.IsISerializable)
		{
			ReadISerializable(classContract);
		}
		else
		{
			ReadClass(classContract);
		}
		if (Globals.TypeOfIDeserializationCallback.IsAssignableFrom(classContract.UnderlyingType))
		{
			((IDeserializationCallback)objectLocal).OnDeserialization(null);
		}
		InvokeOnDeserialized(classContract);
		if (!InvokeFactoryMethod(classContract) && classContract.UnderlyingType == Globals.TypeOfDateTimeOffsetAdapter)
		{
			objectLocal = DateTimeOffsetAdapter.GetDateTimeOffset((DateTimeOffsetAdapter)objectLocal);
		}
		return objectLocal;
	}

	public object ReadCollectionFromJson(XmlReaderDelegator xmlReader, XmlObjectSerializerReadContextComplexJson context, XmlDictionaryString emptyDictionaryString, XmlDictionaryString itemName, CollectionDataContract collectionContract)
	{
		this.xmlReader = xmlReader;
		this.context = context;
		this.emptyDictionaryString = emptyDictionaryString;
		this.itemName = itemName;
		this.collectionContract = collectionContract;
		ReadCollection(collectionContract);
		return objectLocal;
	}

	public void ReadGetOnlyCollectionFromJson(XmlReaderDelegator xmlReader, XmlObjectSerializerReadContextComplexJson context, XmlDictionaryString emptyDictionaryString, XmlDictionaryString itemName, CollectionDataContract collectionContract)
	{
		this.xmlReader = xmlReader;
		this.context = context;
		this.emptyDictionaryString = emptyDictionaryString;
		this.itemName = itemName;
		this.collectionContract = collectionContract;
		ReadGetOnlyCollection(collectionContract);
	}

	private void CreateObject(ClassDataContract classContract)
	{
		Type type = (objectType = classContract.UnderlyingType);
		if (type.IsValueType && !classContract.IsNonAttributedType)
		{
			type = Globals.TypeOfValueType;
		}
		if (classContract.UnderlyingType == Globals.TypeOfDBNull)
		{
			objectLocal = DBNull.Value;
		}
		else if (classContract.IsNonAttributedType)
		{
			if (type.IsValueType)
			{
				objectLocal = FormatterServices.GetUninitializedObject(type);
			}
			else
			{
				objectLocal = classContract.GetNonAttributedTypeConstructor().Invoke(new object[0]);
			}
		}
		else
		{
			objectLocal = CodeInterpreter.ConvertValue(XmlFormatReaderGenerator.UnsafeGetUninitializedObject(DataContract.GetIdForInitialization(classContract)), Globals.TypeOfObject, type);
		}
	}

	private void InvokeOnDeserializing(ClassDataContract classContract)
	{
		if (classContract.BaseContract != null)
		{
			InvokeOnDeserializing(classContract.BaseContract);
		}
		if (classContract.OnDeserializing != null)
		{
			classContract.OnDeserializing.Invoke(objectLocal, new object[1] { context.GetStreamingContext() });
		}
	}

	private void InvokeOnDeserialized(ClassDataContract classContract)
	{
		if (classContract.BaseContract != null)
		{
			InvokeOnDeserialized(classContract.BaseContract);
		}
		if (classContract.OnDeserialized != null)
		{
			classContract.OnDeserialized.Invoke(objectLocal, new object[1] { context.GetStreamingContext() });
		}
	}

	private bool HasFactoryMethod(ClassDataContract classContract)
	{
		return Globals.TypeOfIObjectReference.IsAssignableFrom(classContract.UnderlyingType);
	}

	private bool InvokeFactoryMethod(ClassDataContract classContract)
	{
		if (HasFactoryMethod(classContract))
		{
			objectLocal = CodeInterpreter.ConvertValue(context.GetRealObject((IObjectReference)objectLocal, Globals.NewObjectId), Globals.TypeOfObject, classContract.UnderlyingType);
			return true;
		}
		return false;
	}

	private void ReadISerializable(ClassDataContract classContract)
	{
		ConstructorInfo constructor = classContract.UnderlyingType.GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, JsonFormatGeneratorStatics.SerInfoCtorArgs, null);
		if (constructor == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Constructor that takes SerializationInfo and StreamingContext is not found for '{0}'.", DataContract.GetClrTypeFullName(classContract.UnderlyingType))));
		}
		context.ReadSerializationInfo(xmlReader, classContract.UnderlyingType);
		constructor.Invoke(objectLocal, new object[1] { context.GetStreamingContext() });
	}

	private void ReadClass(ClassDataContract classContract)
	{
		if (classContract.HasExtensionData)
		{
			ExtensionDataObject extensionDataObject = new ExtensionDataObject();
			ReadMembers(classContract, extensionDataObject);
			for (ClassDataContract classDataContract = classContract; classDataContract != null; classDataContract = classDataContract.BaseContract)
			{
				MethodInfo extensionDataSetMethod = classDataContract.ExtensionDataSetMethod;
				if (extensionDataSetMethod != null)
				{
					extensionDataSetMethod.Invoke(objectLocal, new object[1] { extensionDataObject });
				}
			}
		}
		else
		{
			ReadMembers(classContract, null);
		}
	}

	private void ReadMembers(ClassDataContract classContract, ExtensionDataObject extensionData)
	{
		int num = classContract.MemberNames.Length;
		context.IncrementItemCount(num);
		int memberIndex = -1;
		BitFlagsGenerator bitFlagsGenerator = new BitFlagsGenerator(num);
		byte[] requiredElements = new byte[bitFlagsGenerator.GetLocalCount()];
		SetRequiredElements(classContract, requiredElements);
		SetExpectedElements(bitFlagsGenerator, 0);
		while (XmlObjectSerializerReadContext.MoveToNextElement(xmlReader))
		{
			int jsonMemberIndex = context.GetJsonMemberIndex(xmlReader, memberNames, memberIndex, extensionData);
			if (num > 0)
			{
				ReadMembers(jsonMemberIndex, classContract, bitFlagsGenerator, ref memberIndex);
			}
		}
		if (!CheckRequiredElements(bitFlagsGenerator, requiredElements))
		{
			XmlObjectSerializerReadContextComplexJson.ThrowMissingRequiredMembers(objectLocal, memberNames, bitFlagsGenerator.LoadArray(), requiredElements);
		}
	}

	private int ReadMembers(int index, ClassDataContract classContract, BitFlagsGenerator expectedElements, ref int memberIndex)
	{
		int num = ((classContract.BaseContract != null) ? ReadMembers(index, classContract.BaseContract, expectedElements, ref memberIndex) : 0);
		if (num <= index && index < num + classContract.Members.Count)
		{
			DataMember dataMember = classContract.Members[index - num];
			Type memberType = dataMember.MemberType;
			memberIndex = num;
			if (!expectedElements.Load(index))
			{
				XmlObjectSerializerReadContextComplexJson.ThrowDuplicateMemberException(objectLocal, memberNames, memberIndex);
			}
			if (dataMember.IsGetOnlyCollection)
			{
				object member = CodeInterpreter.GetMember(dataMember.MemberInfo, objectLocal);
				context.StoreCollectionMemberInfo(member);
				ReadValue(memberType, dataMember.Name);
			}
			else
			{
				object value = ReadValue(memberType, dataMember.Name);
				CodeInterpreter.SetMember(dataMember.MemberInfo, objectLocal, value);
			}
			memberIndex = index;
			ResetExpectedElements(expectedElements, index);
		}
		return num + classContract.Members.Count;
	}

	private bool CheckRequiredElements(BitFlagsGenerator expectedElements, byte[] requiredElements)
	{
		for (int i = 0; i < requiredElements.Length; i++)
		{
			if ((expectedElements.GetLocal(i) & requiredElements[i]) != 0)
			{
				return false;
			}
		}
		return true;
	}

	private int SetRequiredElements(ClassDataContract contract, byte[] requiredElements)
	{
		int num = ((contract.BaseContract != null) ? SetRequiredElements(contract.BaseContract, requiredElements) : 0);
		List<DataMember> members = contract.Members;
		int num2 = 0;
		while (num2 < members.Count)
		{
			if (members[num2].IsRequired)
			{
				BitFlagsGenerator.SetBit(requiredElements, num);
			}
			num2++;
			num++;
		}
		return num;
	}

	private void SetExpectedElements(BitFlagsGenerator expectedElements, int startIndex)
	{
		int bitCount = expectedElements.GetBitCount();
		for (int i = startIndex; i < bitCount; i++)
		{
			expectedElements.Store(i, value: true);
		}
	}

	private void ResetExpectedElements(BitFlagsGenerator expectedElements, int index)
	{
		expectedElements.Store(index, value: false);
	}

	private object ReadValue(Type type, string name)
	{
		Type type2 = type;
		object obj = null;
		bool flag = false;
		int num = 0;
		while (type.IsGenericType && type.GetGenericTypeDefinition() == Globals.TypeOfNullable)
		{
			num++;
			type = type.GetGenericArguments()[0];
		}
		PrimitiveDataContract primitiveDataContract = PrimitiveDataContract.GetPrimitiveDataContract(type);
		if ((primitiveDataContract != null && primitiveDataContract.UnderlyingType != Globals.TypeOfObject) || num != 0 || type.IsValueType)
		{
			context.ReadAttributes(xmlReader);
			string text = context.ReadIfNullOrRef(xmlReader, type, DataContract.IsTypeSerializable(type));
			if (text == null)
			{
				if (num != 0)
				{
					obj = Activator.CreateInstance(type2);
				}
				else
				{
					if (type.IsValueType)
					{
						throw new SerializationException(SR.GetString("ValueType '{0}' cannot be null.", DataContract.GetClrTypeFullName(type)));
					}
					obj = null;
				}
			}
			else if (text == string.Empty)
			{
				text = context.GetObjectId();
				if (type.IsValueType && !string.IsNullOrEmpty(text))
				{
					throw new SerializationException(SR.GetString("ValueType '{0}' cannot have id.", DataContract.GetClrTypeFullName(type)));
				}
				if (num != 0)
				{
					flag = true;
				}
				if (primitiveDataContract != null && primitiveDataContract.UnderlyingType != Globals.TypeOfObject)
				{
					obj = primitiveDataContract.XmlFormatReaderMethod.Invoke(xmlReader, new object[0]);
					if (!type.IsValueType)
					{
						context.AddNewObject(obj);
					}
				}
				else
				{
					obj = InternalDeserialize(type, name);
				}
			}
			else
			{
				if (type.IsValueType)
				{
					throw new SerializationException(SR.GetString("ValueType '{0}' cannot have ref to another object.", DataContract.GetClrTypeFullName(type)));
				}
				obj = CodeInterpreter.ConvertValue(context.GetExistingObject(text, type, name, string.Empty), Globals.TypeOfObject, type);
			}
			if (flag && text != null)
			{
				obj = WrapNullableObject(type, obj, type2, num);
			}
		}
		else
		{
			obj = InternalDeserialize(type, name);
		}
		return obj;
	}

	private object InternalDeserialize(Type type, string name)
	{
		Type type2 = (type.IsPointer ? Globals.TypeOfReflectionPointer : type);
		object obj = context.InternalDeserialize(xmlReader, DataContract.GetId(type2.TypeHandle), type2.TypeHandle, name, string.Empty);
		if (type.IsPointer)
		{
			return JsonFormatGeneratorStatics.UnboxPointer.Invoke(null, new object[1] { obj });
		}
		return CodeInterpreter.ConvertValue(obj, Globals.TypeOfObject, type);
	}

	private object WrapNullableObject(Type innerType, object innerValue, Type outerType, int nullables)
	{
		object obj = innerValue;
		for (int i = 1; i < nullables; i++)
		{
			Type type = Globals.TypeOfNullable.MakeGenericType(innerType);
			obj = Activator.CreateInstance(type, obj);
			innerType = type;
		}
		return Activator.CreateInstance(outerType, obj);
	}

	private void ReadCollection(CollectionDataContract collectionContract)
	{
		Type type = collectionContract.UnderlyingType;
		Type itemType = collectionContract.ItemType;
		bool flag = collectionContract.Kind == CollectionKind.Array;
		ConstructorInfo constructor = collectionContract.Constructor;
		if (type.IsInterface)
		{
			switch (collectionContract.Kind)
			{
			case CollectionKind.GenericDictionary:
				type = Globals.TypeOfDictionaryGeneric.MakeGenericType(itemType.GetGenericArguments());
				constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Globals.EmptyTypeArray, null);
				break;
			case CollectionKind.Dictionary:
				type = Globals.TypeOfHashtable;
				constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Globals.EmptyTypeArray, null);
				break;
			case CollectionKind.GenericList:
			case CollectionKind.GenericCollection:
			case CollectionKind.List:
			case CollectionKind.GenericEnumerable:
			case CollectionKind.Collection:
			case CollectionKind.Enumerable:
				type = itemType.MakeArrayType();
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			if (type.IsValueType)
			{
				objectLocal = FormatterServices.GetUninitializedObject(type);
			}
			else
			{
				objectLocal = constructor.Invoke(new object[0]);
				context.AddNewObject(objectLocal);
			}
		}
		if ((collectionContract.Kind == CollectionKind.Dictionary || collectionContract.Kind == CollectionKind.GenericDictionary) & context.UseSimpleDictionaryFormat)
		{
			ReadSimpleDictionary(collectionContract, itemType);
			return;
		}
		string objectId = context.GetObjectId();
		bool flag2 = false;
		bool readResult = false;
		if (flag && TryReadPrimitiveArray(itemType, out readResult))
		{
			flag2 = true;
		}
		if (!flag2)
		{
			object obj = null;
			if (flag)
			{
				obj = Array.CreateInstance(itemType, 32);
			}
			int i;
			for (i = 0; i < int.MaxValue; i++)
			{
				if (IsStartElement(itemName, emptyDictionaryString))
				{
					context.IncrementItemCount(1);
					object value = ReadCollectionItem(collectionContract, itemType);
					if (flag)
					{
						obj = XmlFormatGeneratorStatics.EnsureArraySizeMethod.MakeGenericMethod(itemType).Invoke(null, new object[2] { obj, i });
						((Array)obj).SetValue(value, i);
					}
					else
					{
						StoreCollectionValue(objectLocal, itemType, value, collectionContract);
					}
				}
				else
				{
					if (IsEndElement())
					{
						break;
					}
					HandleUnexpectedItemInCollection(ref i);
				}
			}
			if (flag)
			{
				MethodInfo methodInfo = XmlFormatGeneratorStatics.TrimArraySizeMethod.MakeGenericMethod(itemType);
				objectLocal = methodInfo.Invoke(null, new object[2] { obj, i });
				context.AddNewObjectWithId(objectId, objectLocal);
			}
		}
		else
		{
			context.AddNewObjectWithId(objectId, objectLocal);
		}
	}

	private void ReadSimpleDictionary(CollectionDataContract collectionContract, Type keyValueType)
	{
		Type[] genericArguments = keyValueType.GetGenericArguments();
		Type type = genericArguments[0];
		Type type2 = genericArguments[1];
		int num = 0;
		while (type.IsGenericType && type.GetGenericTypeDefinition() == Globals.TypeOfNullable)
		{
			num++;
			type = type.GetGenericArguments()[0];
		}
		DataContract memberTypeContract = ((ClassDataContract)collectionContract.ItemContract).Members[0].MemberTypeContract;
		KeyParseMode keyParseMode = KeyParseMode.Fail;
		if (type == Globals.TypeOfString || type == Globals.TypeOfObject)
		{
			keyParseMode = KeyParseMode.AsString;
		}
		else if (type.IsEnum)
		{
			keyParseMode = KeyParseMode.UsingParseEnum;
		}
		else if (memberTypeContract.ParseMethod != null)
		{
			keyParseMode = KeyParseMode.UsingCustomParse;
		}
		if (keyParseMode == KeyParseMode.Fail)
		{
			ThrowSerializationException(SR.GetString("Key type '{1}' for collection type '{0}' cannot be parsed in simple dictionary.", DataContract.GetClrTypeFullName(collectionContract.UnderlyingType), DataContract.GetClrTypeFullName(type)));
			return;
		}
		XmlNodeType xmlNodeType;
		while ((xmlNodeType = xmlReader.MoveToContent()) != XmlNodeType.EndElement)
		{
			if (xmlNodeType != XmlNodeType.Element)
			{
				ThrowUnexpectedStateException(XmlNodeType.Element);
			}
			context.IncrementItemCount(1);
			string jsonMemberName = XmlObjectSerializerReadContextComplexJson.GetJsonMemberName(xmlReader);
			object obj = null;
			switch (keyParseMode)
			{
			case KeyParseMode.AsString:
				obj = jsonMemberName;
				break;
			case KeyParseMode.UsingParseEnum:
				obj = Enum.Parse(type, jsonMemberName);
				break;
			case KeyParseMode.UsingCustomParse:
				obj = memberTypeContract.ParseMethod.Invoke(null, new object[1] { jsonMemberName });
				break;
			}
			if (num > 0)
			{
				obj = WrapNullableObject(type, obj, type2, num);
			}
			object obj2 = ReadValue(type2, string.Empty);
			collectionContract.AddMethod.Invoke(objectLocal, new object[2] { obj, obj2 });
		}
	}

	private void ReadGetOnlyCollection(CollectionDataContract collectionContract)
	{
		Type underlyingType = collectionContract.UnderlyingType;
		Type itemType = collectionContract.ItemType;
		bool flag = collectionContract.Kind == CollectionKind.Array;
		int arraySize = 0;
		objectLocal = context.GetCollectionMember();
		if ((collectionContract.Kind == CollectionKind.Dictionary || collectionContract.Kind == CollectionKind.GenericDictionary) && context.UseSimpleDictionaryFormat)
		{
			if (objectLocal == null)
			{
				XmlObjectSerializerReadContext.ThrowNullValueReturnedForGetOnlyCollectionException(underlyingType);
				return;
			}
			ReadSimpleDictionary(collectionContract, itemType);
			context.CheckEndOfArray(xmlReader, arraySize, itemName, emptyDictionaryString);
		}
		else
		{
			if (!IsStartElement(itemName, emptyDictionaryString))
			{
				return;
			}
			if (objectLocal == null)
			{
				XmlObjectSerializerReadContext.ThrowNullValueReturnedForGetOnlyCollectionException(underlyingType);
				return;
			}
			arraySize = 0;
			if (flag)
			{
				arraySize = ((Array)objectLocal).Length;
			}
			int iterator = 0;
			while (iterator < int.MaxValue)
			{
				if (IsStartElement(itemName, emptyDictionaryString))
				{
					context.IncrementItemCount(1);
					object value = ReadCollectionItem(collectionContract, itemType);
					if (flag)
					{
						if (arraySize == iterator)
						{
							XmlObjectSerializerReadContext.ThrowArrayExceededSizeException(arraySize, underlyingType);
						}
						else
						{
							((Array)objectLocal).SetValue(value, iterator);
						}
					}
					else
					{
						StoreCollectionValue(objectLocal, itemType, value, collectionContract);
					}
				}
				else
				{
					if (IsEndElement())
					{
						break;
					}
					HandleUnexpectedItemInCollection(ref iterator);
				}
			}
			context.CheckEndOfArray(xmlReader, arraySize, itemName, emptyDictionaryString);
		}
	}

	private bool TryReadPrimitiveArray(Type itemType, out bool readResult)
	{
		readResult = false;
		if (PrimitiveDataContract.GetPrimitiveDataContract(itemType) == null)
		{
			return false;
		}
		string text = null;
		switch (Type.GetTypeCode(itemType))
		{
		case TypeCode.Boolean:
			text = "TryReadBooleanArray";
			break;
		case TypeCode.Decimal:
			text = "TryReadDecimalArray";
			break;
		case TypeCode.Int32:
			text = "TryReadInt32Array";
			break;
		case TypeCode.Int64:
			text = "TryReadInt64Array";
			break;
		case TypeCode.Single:
			text = "TryReadSingleArray";
			break;
		case TypeCode.Double:
			text = "TryReadDoubleArray";
			break;
		case TypeCode.DateTime:
			text = "TryReadJsonDateTimeArray";
			break;
		}
		if (text != null)
		{
			MethodInfo method = typeof(JsonReaderDelegator).GetMethod(text, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			object[] array = new object[5] { context, itemName, emptyDictionaryString, -1, objectLocal };
			readResult = (bool)method.Invoke((JsonReaderDelegator)xmlReader, array);
			objectLocal = array.Last();
			return true;
		}
		return false;
	}

	private object ReadCollectionItem(CollectionDataContract collectionContract, Type itemType)
	{
		if (collectionContract.Kind == CollectionKind.Dictionary || collectionContract.Kind == CollectionKind.GenericDictionary)
		{
			context.ResetAttributes();
			return CodeInterpreter.ConvertValue(DataContractJsonSerializer.ReadJsonValue(XmlObjectSerializerWriteContextComplexJson.GetRevisedItemContract(collectionContract.ItemContract), xmlReader, context), Globals.TypeOfObject, itemType);
		}
		return ReadValue(itemType, "item");
	}

	private void StoreCollectionValue(object collection, Type valueType, object value, CollectionDataContract collectionContract)
	{
		if (collectionContract.Kind == CollectionKind.GenericDictionary || collectionContract.Kind == CollectionKind.Dictionary)
		{
			ClassDataContract obj = DataContract.GetDataContract(valueType) as ClassDataContract;
			DataMember dataMember = obj.Members[0];
			DataMember dataMember2 = obj.Members[1];
			object member = CodeInterpreter.GetMember(dataMember.MemberInfo, value);
			object member2 = CodeInterpreter.GetMember(dataMember2.MemberInfo, value);
			try
			{
				collectionContract.AddMethod.Invoke(collection, new object[2] { member, member2 });
				return;
			}
			catch (TargetInvocationException ex)
			{
				if (ex.InnerException != null)
				{
					throw ex.InnerException;
				}
				throw;
			}
		}
		collectionContract.AddMethod.Invoke(collection, new object[1] { value });
	}

	private void HandleUnexpectedItemInCollection(ref int iterator)
	{
		if (IsStartElement())
		{
			context.SkipUnknownElement(xmlReader);
			iterator--;
			return;
		}
		throw XmlObjectSerializerReadContext.CreateUnexpectedStateException(XmlNodeType.Element, xmlReader);
	}

	private bool IsStartElement(XmlDictionaryString name, XmlDictionaryString ns)
	{
		return xmlReader.IsStartElement(name, ns);
	}

	private bool IsStartElement()
	{
		return xmlReader.IsStartElement();
	}

	private bool IsEndElement()
	{
		return xmlReader.NodeType == XmlNodeType.EndElement;
	}

	private void ThrowUnexpectedStateException(XmlNodeType expectedState)
	{
		throw XmlObjectSerializerReadContext.CreateUnexpectedStateException(expectedState, xmlReader);
	}

	private void ThrowSerializationException(string msg, params object[] values)
	{
		if (values != null && values.Length != 0)
		{
			msg = string.Format(msg, values);
		}
		throw new SerializationException(msg);
	}
}
