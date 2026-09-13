using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace System.Runtime.Serialization;

internal class XmlFormatWriterInterpreter
{
	private ClassDataContract classContract;

	private CollectionDataContract collectionContract;

	private XmlWriterDelegator writer;

	private object obj;

	private XmlObjectSerializerWriteContext ctx;

	private DataContract dataContract;

	private object objLocal;

	private XmlDictionaryString[] contractNamespaces;

	private XmlDictionaryString[] memberNames;

	private XmlDictionaryString[] childElementNamespaces;

	private int typeIndex = 1;

	private int childElementIndex;

	private ClassDataContract classDataContract => (ClassDataContract)dataContract;

	private CollectionDataContract collectionDataContract => (CollectionDataContract)dataContract;

	public XmlFormatWriterInterpreter(ClassDataContract classContract)
	{
		this.classContract = classContract;
	}

	public XmlFormatWriterInterpreter(CollectionDataContract collectionContract)
	{
		this.collectionContract = collectionContract;
	}

	public void WriteToXml(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContext context, ClassDataContract dataContract)
	{
		writer = xmlWriter;
		this.obj = obj;
		ctx = context;
		this.dataContract = dataContract;
		InitArgs(classContract.UnderlyingType);
		if (classContract.IsReadOnlyContract)
		{
			DataContract.ThrowInvalidDataContractException(classContract.SerializationExceptionMessage, null);
		}
		WriteClass(classContract);
	}

	public void WriteCollectionToXml(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContext context, CollectionDataContract collectionContract)
	{
		writer = xmlWriter;
		this.obj = obj;
		ctx = context;
		dataContract = collectionContract;
		InitArgs(collectionContract.UnderlyingType);
		if (collectionContract.IsReadOnlyContract)
		{
			DataContract.ThrowInvalidDataContractException(collectionContract.SerializationExceptionMessage, null);
		}
		WriteCollection(collectionContract);
	}

	private void InitArgs(Type objType)
	{
		if (objType == Globals.TypeOfDateTimeOffsetAdapter)
		{
			objLocal = DateTimeOffsetAdapter.GetDateTimeOffsetAdapter((DateTimeOffset)obj);
		}
		else
		{
			objLocal = CodeInterpreter.ConvertValue(obj, typeof(object), objType);
		}
	}

	private void InvokeOnSerializing(ClassDataContract classContract, object objSerialized, XmlObjectSerializerWriteContext ctx)
	{
		if (classContract.BaseContract != null)
		{
			InvokeOnSerializing(classContract.BaseContract, objSerialized, ctx);
		}
		if (classContract.OnSerializing != null)
		{
			classContract.OnSerializing.Invoke(objSerialized, new object[1] { ctx.GetStreamingContext() });
		}
	}

	private void InvokeOnSerialized(ClassDataContract classContract, object objSerialized, XmlObjectSerializerWriteContext ctx)
	{
		if (classContract.BaseContract != null)
		{
			InvokeOnSerialized(classContract.BaseContract, objSerialized, ctx);
		}
		if (classContract.OnSerialized != null)
		{
			classContract.OnSerialized.Invoke(objSerialized, new object[1] { ctx.GetStreamingContext() });
		}
	}

	private void WriteClass(ClassDataContract classContract)
	{
		InvokeOnSerializing(classContract, objLocal, ctx);
		if (classContract.IsISerializable)
		{
			ctx.WriteISerializable(writer, (ISerializable)objLocal);
		}
		else
		{
			if (classContract.ContractNamespaces.Length > 1)
			{
				contractNamespaces = classDataContract.ContractNamespaces;
			}
			memberNames = classDataContract.MemberNames;
			for (int i = 0; i < classContract.ChildElementNamespaces.Length; i++)
			{
				if (classContract.ChildElementNamespaces[i] != null)
				{
					childElementNamespaces = classDataContract.ChildElementNamespaces;
				}
			}
			if (classContract.HasExtensionData)
			{
				ExtensionDataObject extensionData = ((IExtensibleDataObject)objLocal).ExtensionData;
				ctx.WriteExtensionData(writer, extensionData, -1);
				WriteMembers(classContract, extensionData, classContract);
			}
			else
			{
				WriteMembers(classContract, null, classContract);
			}
		}
		InvokeOnSerialized(classContract, objLocal, ctx);
	}

	private void WriteCollection(CollectionDataContract collectionContract)
	{
		XmlDictionaryString xmlDictionaryString = dataContract.Namespace;
		XmlDictionaryString collectionItemName = collectionDataContract.CollectionItemName;
		if (collectionContract.ChildElementNamespace != null)
		{
			writer.WriteNamespaceDecl(collectionDataContract.ChildElementNamespace);
		}
		if (collectionContract.Kind == CollectionKind.Array)
		{
			Type itemType = collectionContract.ItemType;
			if (objLocal.GetType().GetElementType() != itemType)
			{
				throw new InvalidCastException($"Cannot cast array of {objLocal.GetType().GetElementType()} to array of {itemType}");
			}
			ctx.IncrementArrayCount(writer, (Array)objLocal);
			if (TryWritePrimitiveArray(collectionContract.UnderlyingType, itemType, () => objLocal, collectionItemName, xmlDictionaryString))
			{
				return;
			}
			Array array = (Array)objLocal;
			int[] array2 = new int[1];
			for (int num = 0; num < array.Length; num++)
			{
				if (!TryWritePrimitive(itemType, null, null, num, xmlDictionaryString, collectionItemName, 0))
				{
					WriteStartElement(itemType, collectionContract.Namespace, xmlDictionaryString, collectionItemName, 0);
					array2[0] = num;
					object value = array.GetValue(array2);
					WriteValue(itemType, value, writeXsiType: false);
					WriteEndElement();
				}
			}
			return;
		}
		if (!collectionContract.UnderlyingType.IsAssignableFrom(objLocal.GetType()))
		{
			throw new InvalidCastException($"Cannot cast {objLocal.GetType()} to {collectionContract.UnderlyingType}");
		}
		MethodInfo methodInfo = null;
		switch (collectionContract.Kind)
		{
		case CollectionKind.Dictionary:
		case CollectionKind.List:
		case CollectionKind.Collection:
			methodInfo = XmlFormatGeneratorStatics.IncrementCollectionCountMethod;
			break;
		case CollectionKind.GenericList:
		case CollectionKind.GenericCollection:
			methodInfo = XmlFormatGeneratorStatics.IncrementCollectionCountGenericMethod.MakeGenericMethod(collectionContract.ItemType);
			break;
		case CollectionKind.GenericDictionary:
			methodInfo = XmlFormatGeneratorStatics.IncrementCollectionCountGenericMethod.MakeGenericMethod(Globals.TypeOfKeyValuePair.MakeGenericType(collectionContract.ItemType.GetGenericArguments()));
			break;
		}
		if (methodInfo != null)
		{
			methodInfo.Invoke(ctx, new object[2] { writer, objLocal });
		}
		bool flag = false;
		bool flag2 = false;
		Type type = null;
		Type[] typeArguments = null;
		if (collectionContract.Kind == CollectionKind.GenericDictionary)
		{
			flag2 = true;
			typeArguments = collectionContract.ItemType.GetGenericArguments();
			type = Globals.TypeOfGenericDictionaryEnumerator.MakeGenericType(typeArguments);
		}
		else if (collectionContract.Kind == CollectionKind.Dictionary)
		{
			flag = true;
			typeArguments = new Type[2]
			{
				Globals.TypeOfObject,
				Globals.TypeOfObject
			};
			type = Globals.TypeOfDictionaryEnumerator;
		}
		else
		{
			type = collectionContract.GetEnumeratorMethod.ReturnType;
		}
		MethodInfo method = type.GetMethod("MoveNext", BindingFlags.Instance | BindingFlags.Public, null, Globals.EmptyTypeArray, null);
		MethodInfo methodInfo2 = type.GetMethod("get_Current", BindingFlags.Instance | BindingFlags.Public, null, Globals.EmptyTypeArray, null);
		if (method == null || methodInfo2 == null)
		{
			if (type.IsInterface)
			{
				if (method == null)
				{
					method = XmlFormatGeneratorStatics.MoveNextMethod;
				}
				if (methodInfo2 == null)
				{
					methodInfo2 = XmlFormatGeneratorStatics.GetCurrentMethod;
				}
			}
			else
			{
				Type interfaceType = Globals.TypeOfIEnumerator;
				CollectionKind kind = collectionContract.Kind;
				if (kind == CollectionKind.GenericDictionary || kind == CollectionKind.GenericCollection || kind == CollectionKind.GenericEnumerable)
				{
					Type[] interfaces = type.GetInterfaces();
					foreach (Type type2 in interfaces)
					{
						if (type2.IsGenericType && type2.GetGenericTypeDefinition() == Globals.TypeOfIEnumeratorGeneric && type2.GetGenericArguments()[0] == collectionContract.ItemType)
						{
							interfaceType = type2;
							break;
						}
					}
				}
				if (method == null)
				{
					method = CollectionDataContract.GetTargetMethodWithName("MoveNext", type, interfaceType);
				}
				if (methodInfo2 == null)
				{
					methodInfo2 = CollectionDataContract.GetTargetMethodWithName("get_Current", type, interfaceType);
				}
			}
		}
		Type returnType = methodInfo2.ReturnType;
		object currentValue = null;
		IEnumerator enumerator = (IEnumerator)collectionContract.GetEnumeratorMethod.Invoke(objLocal, new object[0]);
		if (flag)
		{
			enumerator = new CollectionDataContract.DictionaryEnumerator((IDictionaryEnumerator)enumerator);
		}
		else if (flag2)
		{
			Type type3 = Globals.TypeOfIEnumeratorGeneric.MakeGenericType(Globals.TypeOfKeyValuePair.MakeGenericType(typeArguments));
			type.GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[1] { type3 }, null);
			enumerator = (IEnumerator)Activator.CreateInstance(type, enumerator);
		}
		object[] parameters = new object[0];
		while (enumerator != null && enumerator.MoveNext())
		{
			currentValue = methodInfo2.Invoke(enumerator, parameters);
			if (methodInfo == null)
			{
				XmlFormatGeneratorStatics.IncrementItemCountMethod.Invoke(ctx, new object[1] { 1 });
			}
			if (!TryWritePrimitive(returnType, () => currentValue, null, null, xmlDictionaryString, collectionItemName, 0))
			{
				WriteStartElement(returnType, collectionContract.Namespace, xmlDictionaryString, collectionItemName, 0);
				if (flag2 || flag)
				{
					collectionDataContract.ItemContract.WriteXmlValue(writer, currentValue, ctx);
				}
				else
				{
					WriteValue(returnType, currentValue, writeXsiType: false);
				}
				WriteEndElement();
			}
		}
	}

	private int WriteMembers(ClassDataContract classContract, ExtensionDataObject extensionData, ClassDataContract derivedMostClassContract)
	{
		int num = ((classContract.BaseContract != null) ? WriteMembers(classContract.BaseContract, extensionData, derivedMostClassContract) : 0);
		XmlDictionaryString xmlDictionaryString = ((contractNamespaces == null) ? dataContract.Namespace : contractNamespaces[typeIndex - 1]);
		ctx.IncrementItemCount(classContract.Members.Count);
		int num2 = 0;
		while (num2 < classContract.Members.Count)
		{
			DataMember dataMember = classContract.Members[num2];
			Type memberType = dataMember.MemberType;
			object memberValue = null;
			if (dataMember.IsGetOnlyCollection)
			{
				ctx.StoreIsGetOnlyCollection();
			}
			bool flag = true;
			bool flag2 = false;
			if (!dataMember.EmitDefaultValue)
			{
				flag2 = true;
				memberValue = LoadMemberValue(dataMember);
				flag = !IsDefaultValue(memberType, memberValue);
			}
			if (flag)
			{
				bool flag3 = CheckIfMemberHasConflict(dataMember, classContract, derivedMostClassContract);
				if (flag3 || !TryWritePrimitive(memberType, flag2 ? ((Func<object>)(() => memberValue)) : null, dataMember.MemberInfo, null, xmlDictionaryString, null, num2 + childElementIndex))
				{
					WriteStartElement(memberType, classContract.Namespace, xmlDictionaryString, null, num2 + childElementIndex);
					if (classContract.ChildElementNamespaces[num2 + childElementIndex] != null)
					{
						writer.WriteNamespaceDecl(childElementNamespaces[num2 + childElementIndex]);
					}
					if (memberValue == null)
					{
						memberValue = LoadMemberValue(dataMember);
					}
					WriteValue(memberType, memberValue, flag3);
					WriteEndElement();
				}
				if (classContract.HasExtensionData)
				{
					ctx.WriteExtensionData(writer, extensionData, num);
				}
			}
			else if (!dataMember.EmitDefaultValue && dataMember.IsRequired)
			{
				XmlObjectSerializerWriteContext.ThrowRequiredMemberMustBeEmitted(dataMember.Name, classContract.UnderlyingType);
			}
			num2++;
			num++;
		}
		typeIndex++;
		childElementIndex += classContract.Members.Count;
		return num;
	}

	internal bool IsDefaultValue(Type type, object value)
	{
		return GetDefaultValue(type)?.Equals(value) ?? (value == null);
	}

	internal object GetDefaultValue(Type type)
	{
		if (type.IsValueType)
		{
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Boolean:
				return false;
			case TypeCode.Char:
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
				return 0;
			case TypeCode.Int64:
			case TypeCode.UInt64:
				return 0L;
			case TypeCode.Single:
				return 0f;
			case TypeCode.Double:
				return 0.0;
			case TypeCode.Decimal:
				return 0m;
			case TypeCode.DateTime:
				return default(DateTime);
			}
		}
		return null;
	}

	private bool CheckIfMemberHasConflict(DataMember member, ClassDataContract classContract, ClassDataContract derivedMostClassContract)
	{
		if (CheckIfConflictingMembersHaveDifferentTypes(member))
		{
			return true;
		}
		string name = member.Name;
		string text = classContract.StableName.Namespace;
		ClassDataContract classDataContract = derivedMostClassContract;
		while (classDataContract != null && classDataContract != classContract)
		{
			if (text == classDataContract.StableName.Namespace)
			{
				List<DataMember> members = classDataContract.Members;
				for (int i = 0; i < members.Count; i++)
				{
					if (name == members[i].Name)
					{
						return CheckIfConflictingMembersHaveDifferentTypes(members[i]);
					}
				}
			}
			classDataContract = classDataContract.BaseContract;
		}
		return false;
	}

	private bool CheckIfConflictingMembersHaveDifferentTypes(DataMember member)
	{
		while (member.ConflictingMember != null)
		{
			if (member.MemberType != member.ConflictingMember.MemberType)
			{
				return true;
			}
			member = member.ConflictingMember;
		}
		return false;
	}

	private bool NeedsPrefix(Type type, XmlDictionaryString ns)
	{
		if (type == Globals.TypeOfXmlQualifiedName)
		{
			if (ns != null && ns.Value != null)
			{
				return ns.Value.Length > 0;
			}
			return false;
		}
		return false;
	}

	private void WriteStartElement(Type type, XmlDictionaryString ns, XmlDictionaryString namespaceLocal, XmlDictionaryString nameLocal, int nameIndex)
	{
		bool num = NeedsPrefix(type, ns);
		nameLocal = nameLocal ?? memberNames[nameIndex];
		if (num)
		{
			writer.WriteStartElement("q", nameLocal, namespaceLocal);
		}
		else
		{
			writer.WriteStartElement(nameLocal, namespaceLocal);
		}
	}

	private void WriteEndElement()
	{
		writer.WriteEndElement();
	}

	private void WriteValue(Type memberType, object memberValue, bool writeXsiType)
	{
		if (memberType.IsPointer)
		{
			_ = (Pointer)XmlFormatGeneratorStatics.BoxPointer.Invoke(null, new object[2] { memberValue, memberType });
		}
		bool flag = memberType.IsGenericType && memberType.GetGenericTypeDefinition() == Globals.TypeOfNullable;
		if (memberType.IsValueType && !flag)
		{
			PrimitiveDataContract primitiveDataContract = PrimitiveDataContract.GetPrimitiveDataContract(memberType);
			if (primitiveDataContract != null && !writeXsiType)
			{
				primitiveDataContract.XmlFormatContentWriterMethod.Invoke(writer, new object[1] { memberValue });
			}
			else
			{
				bool isDeclaredType = Type.GetTypeHandle(memberValue).Equals(CodeInterpreter.ConvertValue(memberValue, memberType, Globals.TypeOfObject));
				ctx.InternalSerialize(writer, memberValue, isDeclaredType, writeXsiType, DataContract.GetId(memberType.TypeHandle), memberType.TypeHandle);
			}
			return;
		}
		bool isNull;
		if (flag)
		{
			memberValue = UnwrapNullableObject(() => memberValue, ref memberType, out isNull);
		}
		else
		{
			isNull = memberValue == null;
		}
		if (isNull)
		{
			XmlFormatGeneratorStatics.WriteNullMethod.Invoke(ctx, new object[3]
			{
				writer,
				memberType,
				DataContract.IsTypeSerializable(memberType)
			});
			return;
		}
		PrimitiveDataContract primitiveDataContract2 = PrimitiveDataContract.GetPrimitiveDataContract(memberType);
		if (primitiveDataContract2 != null && primitiveDataContract2.UnderlyingType != Globals.TypeOfObject && !writeXsiType)
		{
			if (flag)
			{
				primitiveDataContract2.XmlFormatContentWriterMethod.Invoke(writer, new object[1] { memberValue });
			}
			else
			{
				primitiveDataContract2.XmlFormatContentWriterMethod.Invoke(ctx, new object[2] { writer, memberValue });
			}
			return;
		}
		bool flag2 = false;
		if (memberType == Globals.TypeOfObject || memberType == Globals.TypeOfValueType || ((IList)Globals.TypeOfNullable.GetInterfaces()).Contains((object)memberType))
		{
			object obj = CodeInterpreter.ConvertValue(memberValue, memberType.GetType(), Globals.TypeOfObject);
			memberValue = obj;
			flag2 = memberValue == null;
		}
		if (flag2)
		{
			XmlFormatGeneratorStatics.WriteNullMethod.Invoke(ctx, new object[3]
			{
				writer,
				memberType,
				DataContract.IsTypeSerializable(memberType)
			});
			return;
		}
		RuntimeTypeHandle typeHandle = Type.GetTypeHandle(memberValue);
		bool isDeclaredType2 = typeHandle.Equals(CodeInterpreter.ConvertValue(memberValue, memberType, Globals.TypeOfObject));
		if (flag)
		{
			ctx.InternalSerialize(writer, memberValue, isDeclaredType2, writeXsiType, DataContract.GetId(memberType.TypeHandle), memberType.TypeHandle);
		}
		else if (memberType == Globals.TypeOfObject)
		{
			DataContract dataContract = DataContract.GetDataContract(memberValue.GetType());
			writer.WriteAttributeQualifiedName("i", DictionaryGlobals.XsiTypeLocalName, DictionaryGlobals.SchemaInstanceNamespace, dataContract.Name, dataContract.Namespace);
			ctx.InternalSerializeReference(writer, memberValue, isDeclaredType: false, writeXsiType: false, -1, typeHandle);
		}
		else
		{
			ctx.InternalSerializeReference(writer, memberValue, isDeclaredType2, writeXsiType, DataContract.GetId(memberType.TypeHandle), memberType.TypeHandle);
		}
	}

	private object UnwrapNullableObject(Func<object> memberValue, ref Type memberType, out bool isNull)
	{
		object obj = memberValue();
		isNull = false;
		while (memberType.IsGenericType && memberType.GetGenericTypeDefinition() == Globals.TypeOfNullable)
		{
			Type type = memberType.GetGenericArguments()[0];
			if ((bool)XmlFormatGeneratorStatics.GetHasValueMethod.MakeGenericMethod(type).Invoke(null, new object[1] { obj }))
			{
				obj = XmlFormatGeneratorStatics.GetNullableValueMethod.MakeGenericMethod(type).Invoke(null, new object[1] { obj });
			}
			else
			{
				isNull = true;
				obj = XmlFormatGeneratorStatics.GetDefaultValueMethod.MakeGenericMethod(memberType).Invoke(null, new object[0]);
			}
			memberType = type;
		}
		return obj;
	}

	private bool TryWritePrimitive(Type type, Func<object> value, MemberInfo memberInfo, int? arrayItemIndex, XmlDictionaryString ns, XmlDictionaryString name, int nameIndex)
	{
		PrimitiveDataContract primitiveDataContract = PrimitiveDataContract.GetPrimitiveDataContract(type);
		if (primitiveDataContract == null || primitiveDataContract.UnderlyingType == Globals.TypeOfObject)
		{
			return false;
		}
		object obj = null;
		List<object> list = new List<object>();
		if (type.IsValueType)
		{
			obj = writer;
		}
		else
		{
			obj = ctx;
			list.Add(writer);
		}
		if (value != null)
		{
			list.Add(value());
		}
		else if (memberInfo != null)
		{
			list.Add(CodeInterpreter.GetMember(memberInfo, objLocal));
		}
		else
		{
			list.Add(((Array)objLocal).GetValue(new int[1] { arrayItemIndex.Value }));
		}
		if (name != null)
		{
			list.Add(name);
		}
		else
		{
			list.Add(memberNames[nameIndex]);
		}
		list.Add(ns);
		primitiveDataContract.XmlFormatWriterMethod.Invoke(obj, list.ToArray());
		return true;
	}

	private bool TryWritePrimitiveArray(Type type, Type itemType, Func<object> value, XmlDictionaryString itemName, XmlDictionaryString itemNamespace)
	{
		if (PrimitiveDataContract.GetPrimitiveDataContract(itemType) == null)
		{
			return false;
		}
		string text = null;
		switch (Type.GetTypeCode(itemType))
		{
		case TypeCode.Boolean:
			text = "WriteBooleanArray";
			break;
		case TypeCode.DateTime:
			text = "WriteDateTimeArray";
			break;
		case TypeCode.Decimal:
			text = "WriteDecimalArray";
			break;
		case TypeCode.Int32:
			text = "WriteInt32Array";
			break;
		case TypeCode.Int64:
			text = "WriteInt64Array";
			break;
		case TypeCode.Single:
			text = "WriteSingleArray";
			break;
		case TypeCode.Double:
			text = "WriteDoubleArray";
			break;
		}
		if (text != null)
		{
			typeof(XmlWriterDelegator).GetMethod(text, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[3]
			{
				type,
				typeof(XmlDictionaryString),
				typeof(XmlDictionaryString)
			}, null).Invoke(writer, new object[3]
			{
				value(),
				itemName,
				itemNamespace
			});
			return true;
		}
		return false;
	}

	private object LoadMemberValue(DataMember member)
	{
		return CodeInterpreter.GetMember(member.MemberInfo, objLocal);
	}
}
