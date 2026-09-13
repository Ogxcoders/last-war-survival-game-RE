using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace System.Runtime.Serialization;

internal class XmlFormatReaderInterpreter
{
	private bool is_get_only_collection;

	private ClassDataContract classContract;

	private CollectionDataContract collectionContract;

	private object objectLocal;

	private Type objectType;

	private XmlReaderDelegator xmlReader;

	private XmlObjectSerializerReadContext context;

	private XmlDictionaryString[] memberNames;

	private XmlDictionaryString[] memberNamespaces;

	private XmlDictionaryString itemName;

	private XmlDictionaryString itemNamespace;

	public XmlFormatReaderInterpreter(ClassDataContract classContract)
	{
		this.classContract = classContract;
	}

	public XmlFormatReaderInterpreter(CollectionDataContract collectionContract, bool isGetOnly)
	{
		this.collectionContract = collectionContract;
		is_get_only_collection = isGetOnly;
	}

	public object ReadFromXml(XmlReaderDelegator xmlReader, XmlObjectSerializerReadContext context, XmlDictionaryString[] memberNames, XmlDictionaryString[] memberNamespaces)
	{
		this.xmlReader = xmlReader;
		this.context = context;
		this.memberNames = memberNames;
		this.memberNamespaces = memberNamespaces;
		CreateObject(classContract);
		context.AddNewObject(objectLocal);
		InvokeOnDeserializing(classContract);
		string text = null;
		if (HasFactoryMethod(classContract))
		{
			text = context.GetObjectId();
		}
		if (classContract.IsISerializable)
		{
			ReadISerializable(classContract);
		}
		else
		{
			ReadClass(classContract);
		}
		bool flag = InvokeFactoryMethod(classContract, text);
		if (Globals.TypeOfIDeserializationCallback.IsAssignableFrom(classContract.UnderlyingType))
		{
			((IDeserializationCallback)objectLocal).OnDeserialization(null);
		}
		InvokeOnDeserialized(classContract);
		if ((text == null || !flag) && classContract.UnderlyingType == Globals.TypeOfDateTimeOffsetAdapter)
		{
			objectLocal = DateTimeOffsetAdapter.GetDateTimeOffset((DateTimeOffsetAdapter)objectLocal);
		}
		return objectLocal;
	}

	public object ReadCollectionFromXml(XmlReaderDelegator xmlReader, XmlObjectSerializerReadContext context, XmlDictionaryString itemName, XmlDictionaryString itemNamespace, CollectionDataContract collectionContract)
	{
		this.xmlReader = xmlReader;
		this.context = context;
		this.itemName = itemName;
		this.itemNamespace = itemNamespace;
		this.collectionContract = collectionContract;
		ReadCollection(collectionContract);
		return objectLocal;
	}

	public void ReadGetOnlyCollectionFromXml(XmlReaderDelegator xmlReader, XmlObjectSerializerReadContext context, XmlDictionaryString itemName, XmlDictionaryString itemNamespace, CollectionDataContract collectionContract)
	{
		this.xmlReader = xmlReader;
		this.context = context;
		this.itemName = itemName;
		this.itemNamespace = itemNamespace;
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

	private bool InvokeFactoryMethod(ClassDataContract classContract, string objectId)
	{
		if (HasFactoryMethod(classContract))
		{
			objectLocal = CodeInterpreter.ConvertValue(context.GetRealObject((IObjectReference)objectLocal, objectId), Globals.TypeOfObject, classContract.UnderlyingType);
			return true;
		}
		return false;
	}

	private void ReadISerializable(ClassDataContract classContract)
	{
		ConstructorInfo iSerializableConstructor = classContract.GetISerializableConstructor();
		SerializationInfo serializationInfo = context.ReadSerializationInfo(xmlReader, classContract.UnderlyingType);
		iSerializableConstructor.Invoke(objectLocal, new object[2]
		{
			serializationInfo,
			context.GetStreamingContext()
		});
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
		int firstRequiredMember;
		bool[] requiredMembers = GetRequiredMembers(classContract, out firstRequiredMember);
		bool flag = firstRequiredMember < num;
		int requiredIndex = (flag ? firstRequiredMember : num);
		while (XmlObjectSerializerReadContext.MoveToNextElement(xmlReader))
		{
			int index = ((!flag) ? context.GetMemberIndex(xmlReader, memberNames, memberNamespaces, memberIndex, extensionData) : context.GetMemberIndexWithRequiredMembers(xmlReader, memberNames, memberNamespaces, memberIndex, requiredIndex, extensionData));
			if (num > 0)
			{
				ReadMembers(index, classContract, requiredMembers, ref memberIndex, ref requiredIndex);
			}
		}
		if (flag && requiredIndex < num)
		{
			XmlObjectSerializerReadContext.ThrowRequiredMemberMissingException(xmlReader, memberIndex, requiredIndex, memberNames);
		}
	}

	private int ReadMembers(int index, ClassDataContract classContract, bool[] requiredMembers, ref int memberIndex, ref int requiredIndex)
	{
		int num = ((classContract.BaseContract != null) ? ReadMembers(index, classContract.BaseContract, requiredMembers, ref memberIndex, ref requiredIndex) : 0);
		if (num <= index && index < num + classContract.Members.Count)
		{
			DataMember dataMember = classContract.Members[index - num];
			Type memberType = dataMember.MemberType;
			if (dataMember.IsRequired)
			{
				int i;
				for (i = index + 1; i < requiredMembers.Length && !requiredMembers[i]; i++)
				{
				}
				requiredIndex = i;
			}
			if (dataMember.IsGetOnlyCollection)
			{
				object member = CodeInterpreter.GetMember(dataMember.MemberInfo, objectLocal);
				context.StoreCollectionMemberInfo(member);
				ReadValue(memberType, dataMember.Name, classContract.StableName.Namespace);
			}
			else
			{
				object value = ReadValue(memberType, dataMember.Name, classContract.StableName.Namespace);
				CodeInterpreter.SetMember(dataMember.MemberInfo, objectLocal, value);
			}
			memberIndex = index;
		}
		return num + classContract.Members.Count;
	}

	private bool[] GetRequiredMembers(ClassDataContract contract, out int firstRequiredMember)
	{
		int num = contract.MemberNames.Length;
		bool[] array = new bool[num];
		GetRequiredMembers(contract, array);
		firstRequiredMember = 0;
		while (firstRequiredMember < num && !array[firstRequiredMember])
		{
			firstRequiredMember++;
		}
		return array;
	}

	private int GetRequiredMembers(ClassDataContract contract, bool[] requiredMembers)
	{
		int num = ((contract.BaseContract != null) ? GetRequiredMembers(contract.BaseContract, requiredMembers) : 0);
		List<DataMember> members = contract.Members;
		int num2 = 0;
		while (num2 < members.Count)
		{
			requiredMembers[num] = members[num2].IsRequired;
			num2++;
			num++;
		}
		return num;
	}

	private object ReadValue(Type type, string name, string ns)
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
					obj = InternalDeserialize(type, name, ns);
				}
			}
			else
			{
				if (type.IsValueType)
				{
					throw new SerializationException(SR.GetString("ValueType '{0}' cannot have ref to another object.", DataContract.GetClrTypeFullName(type)));
				}
				obj = CodeInterpreter.ConvertValue(context.GetExistingObject(text, type, name, ns), Globals.TypeOfObject, type);
			}
			if (flag && text != null)
			{
				obj = WrapNullableObject(type, obj, type2, num);
			}
		}
		else
		{
			obj = InternalDeserialize(type, name, ns);
		}
		return obj;
	}

	private object InternalDeserialize(Type type, string name, string ns)
	{
		Type type2 = (type.IsPointer ? Globals.TypeOfReflectionPointer : type);
		object obj = context.InternalDeserialize(xmlReader, DataContract.GetId(type2.TypeHandle), type2.TypeHandle, name, ns);
		if (type.IsPointer)
		{
			return XmlFormatGeneratorStatics.UnboxPointer.Invoke(null, new object[1] { obj });
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
		ConstructorInfo constructorInfo = collectionContract.Constructor;
		if (type.IsInterface)
		{
			switch (collectionContract.Kind)
			{
			case CollectionKind.GenericDictionary:
				type = Globals.TypeOfDictionaryGeneric.MakeGenericType(itemType.GetGenericArguments());
				constructorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, Globals.EmptyTypeArray, null);
				break;
			case CollectionKind.Dictionary:
				type = Globals.TypeOfHashtable;
				constructorInfo = XmlFormatGeneratorStatics.HashtableCtor;
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
		string text = collectionContract.ItemName;
		string itemNs = collectionContract.StableName.Namespace;
		if (!flag)
		{
			if (type.IsValueType)
			{
				objectLocal = FormatterServices.GetUninitializedObject(type);
			}
			else
			{
				objectLocal = constructorInfo.Invoke(new object[0]);
				context.AddNewObject(objectLocal);
			}
		}
		int arraySize = context.GetArraySize();
		string objectId = context.GetObjectId();
		bool flag2 = false;
		bool readResult = false;
		if (flag && TryReadPrimitiveArray(type, itemType, arraySize, out readResult))
		{
			flag2 = true;
		}
		if (!readResult)
		{
			if (arraySize == -1)
			{
				object obj = null;
				if (flag)
				{
					obj = Array.CreateInstance(itemType, 32);
				}
				int i;
				for (i = 0; i < int.MaxValue; i++)
				{
					if (IsStartElement(itemName, itemNamespace))
					{
						context.IncrementItemCount(1);
						object value = ReadCollectionItem(collectionContract, itemType, text, itemNs);
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
				context.IncrementItemCount(arraySize);
				if (flag)
				{
					objectLocal = Array.CreateInstance(itemType, arraySize);
					context.AddNewObject(objectLocal);
				}
				for (int j = 0; j < arraySize; j++)
				{
					if (IsStartElement(itemName, itemNamespace))
					{
						object value2 = ReadCollectionItem(collectionContract, itemType, text, itemNs);
						if (flag)
						{
							((Array)objectLocal).SetValue(value2, j);
						}
						else
						{
							StoreCollectionValue(objectLocal, itemType, value2, collectionContract);
						}
					}
					else
					{
						HandleUnexpectedItemInCollection(ref j);
					}
				}
				context.CheckEndOfArray(xmlReader, arraySize, itemName, itemNamespace);
			}
		}
		if (flag2)
		{
			context.AddNewObjectWithId(objectId, objectLocal);
		}
	}

	private void ReadGetOnlyCollection(CollectionDataContract collectionContract)
	{
		Type underlyingType = collectionContract.UnderlyingType;
		Type itemType = collectionContract.ItemType;
		bool flag = collectionContract.Kind == CollectionKind.Array;
		string text = collectionContract.ItemName;
		string itemNs = collectionContract.StableName.Namespace;
		objectLocal = context.GetCollectionMember();
		if (!IsStartElement(itemName, itemNamespace))
		{
			return;
		}
		if (objectLocal == null)
		{
			XmlObjectSerializerReadContext.ThrowNullValueReturnedForGetOnlyCollectionException(underlyingType);
			return;
		}
		int num = 0;
		if (flag)
		{
			num = ((Array)objectLocal).Length;
		}
		context.AddNewObject(objectLocal);
		int iterator = 0;
		while (iterator < int.MaxValue)
		{
			if (IsStartElement(itemName, itemNamespace))
			{
				context.IncrementItemCount(1);
				object value = ReadCollectionItem(collectionContract, itemType, text, itemNs);
				if (flag)
				{
					if (num == iterator)
					{
						XmlObjectSerializerReadContext.ThrowArrayExceededSizeException(num, underlyingType);
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
		context.CheckEndOfArray(xmlReader, num, itemName, itemNamespace);
	}

	private bool TryReadPrimitiveArray(Type type, Type itemType, int size, out bool readResult)
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
		case TypeCode.DateTime:
			text = "TryReadDateTimeArray";
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
		}
		if (text != null)
		{
			MethodInfo method = typeof(XmlReaderDelegator).GetMethod(text, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			object[] array = new object[5] { context, itemName, itemNamespace, size, objectLocal };
			readResult = (bool)method.Invoke(xmlReader, array);
			objectLocal = array.Last();
			return true;
		}
		return false;
	}

	private object ReadCollectionItem(CollectionDataContract collectionContract, Type itemType, string itemName, string itemNs)
	{
		if (collectionContract.Kind == CollectionKind.Dictionary || collectionContract.Kind == CollectionKind.GenericDictionary)
		{
			context.ResetAttributes();
			return CodeInterpreter.ConvertValue(collectionContract.ItemContract.ReadXmlValue(xmlReader, context), Globals.TypeOfObject, itemType);
		}
		return ReadValue(itemType, itemName, itemNs);
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
			collectionContract.AddMethod.Invoke(collection, new object[2] { member, member2 });
		}
		else
		{
			collectionContract.AddMethod.Invoke(collection, new object[1] { value });
		}
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
}
