using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace System.Runtime.Serialization.Json;

internal class JsonFormatWriterInterpreter
{
	private ClassDataContract classContract;

	private CollectionDataContract collectionContract;

	private XmlWriterDelegator writer;

	private object obj;

	private XmlObjectSerializerWriteContextComplexJson context;

	private DataContract dataContract;

	private object objLocal;

	private XmlDictionaryString[] memberNames;

	private int typeIndex = 1;

	private int childElementIndex;

	private ClassDataContract classDataContract => (ClassDataContract)dataContract;

	private CollectionDataContract collectionDataContract => (CollectionDataContract)dataContract;

	public JsonFormatWriterInterpreter(ClassDataContract classContract)
	{
		this.classContract = classContract;
	}

	public JsonFormatWriterInterpreter(CollectionDataContract collectionContract)
	{
		this.collectionContract = collectionContract;
	}

	public void WriteToJson(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContextComplexJson context, ClassDataContract dataContract, XmlDictionaryString[] memberNames)
	{
		writer = xmlWriter;
		this.obj = obj;
		this.context = context;
		this.dataContract = dataContract;
		this.memberNames = memberNames;
		InitArgs(classContract.UnderlyingType);
		if (classContract.IsReadOnlyContract)
		{
			DataContract.ThrowInvalidDataContractException(classContract.SerializationExceptionMessage, null);
		}
		WriteClass(classContract);
	}

	public void WriteCollectionToJson(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContextComplexJson context, CollectionDataContract dataContract)
	{
		writer = xmlWriter;
		this.obj = obj;
		this.context = context;
		this.dataContract = dataContract;
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

	private void InvokeOnSerializing(ClassDataContract classContract, object objSerialized, XmlObjectSerializerWriteContext context)
	{
		if (classContract.BaseContract != null)
		{
			InvokeOnSerializing(classContract.BaseContract, objSerialized, context);
		}
		if (classContract.OnSerializing != null)
		{
			classContract.OnSerializing.Invoke(objSerialized, new object[1] { context.GetStreamingContext() });
		}
	}

	private void InvokeOnSerialized(ClassDataContract classContract, object objSerialized, XmlObjectSerializerWriteContext context)
	{
		if (classContract.BaseContract != null)
		{
			InvokeOnSerialized(classContract.BaseContract, objSerialized, context);
		}
		if (classContract.OnSerialized != null)
		{
			classContract.OnSerialized.Invoke(objSerialized, new object[1] { context.GetStreamingContext() });
		}
	}

	private void WriteClass(ClassDataContract classContract)
	{
		InvokeOnSerializing(classContract, objLocal, context);
		if (classContract.IsISerializable)
		{
			context.WriteJsonISerializable(writer, (ISerializable)objLocal);
		}
		else if (classContract.HasExtensionData)
		{
			ExtensionDataObject extensionData = ((IExtensibleDataObject)objLocal).ExtensionData;
			context.WriteExtensionData(writer, extensionData, -1);
			WriteMembers(classContract, extensionData, classContract);
		}
		else
		{
			WriteMembers(classContract, null, classContract);
		}
		InvokeOnSerialized(classContract, objLocal, context);
	}

	private void WriteCollection(CollectionDataContract collectionContract)
	{
		XmlDictionaryString collectionItemName = context.CollectionItemName;
		if (collectionContract.Kind == CollectionKind.Array)
		{
			Type itemType = collectionContract.ItemType;
			if (objLocal.GetType().GetElementType() != itemType)
			{
				throw new InvalidCastException($"Cannot cast array of {objLocal.GetType().GetElementType()} to array of {itemType}");
			}
			context.IncrementArrayCount(writer, (Array)objLocal);
			if (TryWritePrimitiveArray(collectionContract.UnderlyingType, itemType, () => objLocal, collectionItemName))
			{
				return;
			}
			WriteArrayAttribute();
			Array array = (Array)objLocal;
			int[] array2 = new int[1];
			for (int num = 0; num < array.Length; num++)
			{
				if (!TryWritePrimitive(itemType, null, null, num, collectionItemName, 0))
				{
					WriteStartElement(collectionItemName, 0);
					array2[0] = num;
					object value = array.GetValue(array2);
					WriteValue(itemType, value);
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
			methodInfo.Invoke(context, new object[2] { writer, objLocal });
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
		MethodInfo methodInfo2 = type.GetMethod("MoveNext", BindingFlags.Instance | BindingFlags.Public, null, Globals.EmptyTypeArray, null);
		MethodInfo methodInfo3 = type.GetMethod("get_Current", BindingFlags.Instance | BindingFlags.Public, null, Globals.EmptyTypeArray, null);
		if (methodInfo2 == null || methodInfo3 == null)
		{
			if (type.IsInterface)
			{
				if (methodInfo2 == null)
				{
					methodInfo2 = JsonFormatGeneratorStatics.MoveNextMethod;
				}
				if (methodInfo3 == null)
				{
					methodInfo3 = JsonFormatGeneratorStatics.GetCurrentMethod;
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
				if (methodInfo2 == null)
				{
					methodInfo2 = CollectionDataContract.GetTargetMethodWithName("MoveNext", type, interfaceType);
				}
				if (methodInfo3 == null)
				{
					methodInfo3 = CollectionDataContract.GetTargetMethodWithName("get_Current", type, interfaceType);
				}
			}
		}
		Type returnType = methodInfo3.ReturnType;
		object currentValue = null;
		IEnumerator enumerator = (IEnumerator)collectionContract.GetEnumeratorMethod.Invoke(objLocal, new object[0]);
		if (flag)
		{
			enumerator = (IEnumerator)type.GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[1] { Globals.TypeOfIDictionaryEnumerator }, null).Invoke(new object[1] { enumerator });
		}
		else if (flag2)
		{
			Type type3 = Globals.TypeOfIEnumeratorGeneric.MakeGenericType(Globals.TypeOfKeyValuePair.MakeGenericType(typeArguments));
			type.GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[1] { type3 }, null);
			enumerator = (IEnumerator)Activator.CreateInstance(type, enumerator);
		}
		bool num3 = flag || flag2;
		bool flag3 = num3 && context.UseSimpleDictionaryFormat;
		PropertyInfo memberInfo = null;
		PropertyInfo propertyInfo = null;
		if (num3)
		{
			Type type4 = Globals.TypeOfKeyValue.MakeGenericType(typeArguments);
			memberInfo = type4.GetProperty("Key");
			propertyInfo = type4.GetProperty("Value");
		}
		if (flag3)
		{
			WriteObjectAttribute();
			object[] parameters = new object[0];
			while ((bool)methodInfo2.Invoke(enumerator, parameters))
			{
				currentValue = methodInfo3.Invoke(enumerator, parameters);
				object member = CodeInterpreter.GetMember(memberInfo, currentValue);
				object member2 = CodeInterpreter.GetMember(propertyInfo, currentValue);
				WriteStartElement(member, 0);
				WriteValue(propertyInfo.PropertyType, member2);
				WriteEndElement();
			}
			return;
		}
		WriteArrayAttribute();
		object[] parameters2 = new object[0];
		while (enumerator != null && enumerator.MoveNext())
		{
			currentValue = methodInfo3.Invoke(enumerator, parameters2);
			if (methodInfo == null)
			{
				XmlFormatGeneratorStatics.IncrementItemCountMethod.Invoke(context, new object[1] { 1 });
			}
			if (!TryWritePrimitive(returnType, () => currentValue, null, null, collectionItemName, 0))
			{
				WriteStartElement(collectionItemName, 0);
				if (flag2 || flag)
				{
					DataContractJsonSerializer.WriteJsonValue(JsonDataContract.GetJsonDataContract(XmlObjectSerializerWriteContextComplexJson.GetRevisedItemContract(collectionDataContract.ItemContract)), writer, currentValue, context, currentValue.GetType().TypeHandle);
				}
				else
				{
					WriteValue(returnType, currentValue);
				}
				WriteEndElement();
			}
		}
	}

	private int WriteMembers(ClassDataContract classContract, ExtensionDataObject extensionData, ClassDataContract derivedMostClassContract)
	{
		int num = ((classContract.BaseContract != null) ? WriteMembers(classContract.BaseContract, extensionData, derivedMostClassContract) : 0);
		context.IncrementItemCount(classContract.Members.Count);
		int num2 = 0;
		while (num2 < classContract.Members.Count)
		{
			DataMember dataMember = classContract.Members[num2];
			Type memberType = dataMember.MemberType;
			object memberValue = null;
			if (dataMember.IsGetOnlyCollection)
			{
				context.StoreIsGetOnlyCollection();
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
				bool flag3 = DataContractJsonSerializer.CheckIfXmlNameRequiresMapping(classContract.MemberNames[num2]);
				if (flag3 || !TryWritePrimitive(memberType, flag2 ? ((Func<object>)(() => memberValue)) : null, dataMember.MemberInfo, null, null, num2 + childElementIndex))
				{
					if (flag3)
					{
						XmlObjectSerializerWriteContextComplexJson.WriteJsonNameWithMapping(writer, memberNames, num2 + childElementIndex);
					}
					else
					{
						WriteStartElement(null, num2 + childElementIndex);
					}
					if (memberValue == null)
					{
						memberValue = LoadMemberValue(dataMember);
					}
					WriteValue(memberType, memberValue);
					WriteEndElement();
				}
				if (classContract.HasExtensionData)
				{
					context.WriteExtensionData(writer, extensionData, num);
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

	private void WriteStartElement(object nameLocal, int nameIndex)
	{
		object obj = nameLocal ?? memberNames[nameIndex];
		if (nameLocal != null && nameLocal is string)
		{
			writer.WriteStartElement((string)obj, null);
		}
		else if (obj is XmlDictionaryString)
		{
			writer.WriteStartElement((XmlDictionaryString)obj, null);
		}
		else
		{
			writer.WriteStartElement(obj.ToString(), null);
		}
	}

	private void WriteEndElement()
	{
		writer.WriteEndElement();
	}

	private void WriteArrayAttribute()
	{
		writer.WriteAttributeString(null, "type", string.Empty, "array");
	}

	private void WriteObjectAttribute()
	{
		writer.WriteAttributeString(null, "type", null, "object");
	}

	private void WriteValue(Type memberType, object memberValue)
	{
		if (memberType.IsPointer)
		{
			_ = (Pointer)JsonFormatGeneratorStatics.BoxPointer.Invoke(null, new object[2] { memberValue, memberType });
		}
		bool flag = memberType.IsGenericType && memberType.GetGenericTypeDefinition() == Globals.TypeOfNullable;
		if (memberType.IsValueType && !flag)
		{
			PrimitiveDataContract primitiveDataContract = PrimitiveDataContract.GetPrimitiveDataContract(memberType);
			if (primitiveDataContract != null)
			{
				primitiveDataContract.XmlFormatContentWriterMethod.Invoke(writer, new object[1] { memberValue });
			}
			else
			{
				InternalSerialize(XmlFormatGeneratorStatics.InternalSerializeMethod, () => memberValue, memberType, writeXsiType: false);
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
			XmlFormatGeneratorStatics.WriteNullMethod.Invoke(context, new object[3]
			{
				writer,
				memberType,
				DataContract.IsTypeSerializable(memberType)
			});
			return;
		}
		PrimitiveDataContract primitiveDataContract2 = PrimitiveDataContract.GetPrimitiveDataContract(memberType);
		if (primitiveDataContract2 != null && primitiveDataContract2.UnderlyingType != Globals.TypeOfObject)
		{
			if (flag)
			{
				primitiveDataContract2.XmlFormatContentWriterMethod.Invoke(writer, new object[1] { memberValue });
			}
			else
			{
				primitiveDataContract2.XmlFormatContentWriterMethod.Invoke(context, new object[2] { writer, memberValue });
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
			XmlFormatGeneratorStatics.WriteNullMethod.Invoke(context, new object[3]
			{
				writer,
				memberType,
				DataContract.IsTypeSerializable(memberType)
			});
		}
		else
		{
			InternalSerialize(flag ? XmlFormatGeneratorStatics.InternalSerializeMethod : XmlFormatGeneratorStatics.InternalSerializeReferenceMethod, () => memberValue, memberType, writeXsiType: false);
		}
	}

	private void InternalSerialize(MethodInfo methodInfo, Func<object> memberValue, Type memberType, bool writeXsiType)
	{
		object obj = memberValue();
		bool flag = Type.GetTypeHandle(obj).Equals(CodeInterpreter.ConvertValue(obj, memberType, Globals.TypeOfObject));
		try
		{
			methodInfo.Invoke(context, new object[6]
			{
				writer,
				(memberValue != null) ? obj : null,
				flag,
				writeXsiType,
				DataContract.GetId(memberType.TypeHandle),
				memberType.TypeHandle
			});
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

	private bool TryWritePrimitive(Type type, Func<object> value, MemberInfo memberInfo, int? arrayItemIndex, XmlDictionaryString name, int nameIndex)
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
			obj = context;
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
		list.Add(null);
		primitiveDataContract.XmlFormatWriterMethod.Invoke(obj, list.ToArray());
		return true;
	}

	private bool TryWritePrimitiveArray(Type type, Type itemType, Func<object> value, XmlDictionaryString itemName)
	{
		if (PrimitiveDataContract.GetPrimitiveDataContract(itemType) == null)
		{
			return false;
		}
		string text = null;
		switch (Type.GetTypeCode(itemType))
		{
		case TypeCode.Boolean:
			text = "WriteJsonBooleanArray";
			break;
		case TypeCode.DateTime:
			text = "WriteJsonDateTimeArray";
			break;
		case TypeCode.Decimal:
			text = "WriteJsonDecimalArray";
			break;
		case TypeCode.Int32:
			text = "WriteJsonInt32Array";
			break;
		case TypeCode.Int64:
			text = "WriteJsonInt64Array";
			break;
		case TypeCode.Single:
			text = "WriteJsonSingleArray";
			break;
		case TypeCode.Double:
			text = "WriteJsonDoubleArray";
			break;
		}
		if (text != null)
		{
			WriteArrayAttribute();
			typeof(JsonWriterDelegator).GetMethod(text, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[3]
			{
				type,
				typeof(XmlDictionaryString),
				typeof(XmlDictionaryString)
			}, null).Invoke(writer, new object[3]
			{
				value(),
				itemName,
				null
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
