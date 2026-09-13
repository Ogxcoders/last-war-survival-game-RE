using System.Collections;
using System.Reflection;
using System.Text;

namespace System.Xml.Serialization;

internal class XmlSerializationWriterInterpreter : XmlSerializationWriter
{
	private class CallbackInfo
	{
		private XmlSerializationWriterInterpreter _swi;

		private XmlTypeMapping _typeMap;

		public CallbackInfo(XmlSerializationWriterInterpreter swi, XmlTypeMapping typeMap)
		{
			_swi = swi;
			_typeMap = typeMap;
		}

		internal void WriteObject(object ob)
		{
			_swi.WriteObject(_typeMap, ob, _typeMap.ElementName, _typeMap.Namespace, isNullable: false, needType: false, writeWrappingElem: false);
		}

		internal void WriteEnum(object ob)
		{
			_swi.WriteObject(_typeMap, ob, _typeMap.ElementName, _typeMap.Namespace, isNullable: false, needType: true, writeWrappingElem: false);
		}
	}

	private XmlMapping _typeMap;

	private SerializationFormat _format;

	private const string xmlNamespace = "http://www.w3.org/2000/xmlns/";

	public XmlSerializationWriterInterpreter(XmlMapping typeMap)
	{
		_typeMap = typeMap;
		_format = typeMap.Format;
	}

	protected override void InitCallbacks()
	{
		ArrayList relatedMaps = _typeMap.RelatedMaps;
		if (relatedMaps == null)
		{
			return;
		}
		foreach (XmlTypeMapping item in relatedMaps)
		{
			CallbackInfo callbackInfo = new CallbackInfo(this, item);
			if (item.TypeData.SchemaType == SchemaTypes.Enum)
			{
				AddWriteCallback(item.TypeData.Type, item.XmlType, item.Namespace, callbackInfo.WriteEnum);
			}
			else
			{
				AddWriteCallback(item.TypeData.Type, item.XmlType, item.Namespace, callbackInfo.WriteObject);
			}
		}
	}

	public void WriteRoot(object ob)
	{
		WriteStartDocument();
		if (_typeMap is XmlTypeMapping)
		{
			XmlTypeMapping xmlTypeMapping = (XmlTypeMapping)_typeMap;
			if (xmlTypeMapping.TypeData.SchemaType == SchemaTypes.Class || xmlTypeMapping.TypeData.SchemaType == SchemaTypes.Array)
			{
				TopLevelElement();
			}
			if (_format == SerializationFormat.Literal)
			{
				WriteObject(xmlTypeMapping, ob, xmlTypeMapping.ElementName, xmlTypeMapping.Namespace, isNullable: true, needType: false, writeWrappingElem: true);
			}
			else
			{
				WritePotentiallyReferencingElement(xmlTypeMapping.ElementName, xmlTypeMapping.Namespace, ob, xmlTypeMapping.TypeData.Type, suppressReference: true, isNullable: false);
			}
		}
		else
		{
			if (!(ob is object[]))
			{
				throw CreateUnknownTypeException(ob);
			}
			WriteMessage((XmlMembersMapping)_typeMap, (object[])ob);
		}
		WriteReferencedElements();
	}

	protected XmlTypeMapping GetTypeMap(Type type)
	{
		ArrayList relatedMaps = _typeMap.RelatedMaps;
		if (relatedMaps != null)
		{
			foreach (XmlTypeMapping item in relatedMaps)
			{
				if (item.TypeData.Type == type)
				{
					return item;
				}
			}
		}
		throw new InvalidOperationException("Type " + type?.ToString() + " not mapped");
	}

	protected virtual void WriteObject(XmlTypeMapping typeMap, object ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				if (_format == SerializationFormat.Literal)
				{
					WriteNullTagLiteral(element, namesp);
				}
				else
				{
					WriteNullTagEncoded(element, namesp);
				}
			}
			return;
		}
		if (ob is XmlNode)
		{
			if (_format == SerializationFormat.Literal)
			{
				WriteElementLiteral((XmlNode)ob, "", "", isNullable: true, typeMap.IsAny);
			}
			else
			{
				WriteElementEncoded((XmlNode)ob, "", "", isNullable: true, typeMap.IsAny);
			}
			return;
		}
		if (typeMap.TypeData.SchemaType == SchemaTypes.XmlSerializable)
		{
			WriteSerializable((IXmlSerializable)ob, element, namesp, isNullable, !typeMap.IsAny);
			return;
		}
		Type type = typeMap.TypeData.Type;
		if (!ob.GetType().IsAssignableFrom(type))
		{
			ob = ImplicitConvert(ob, type);
		}
		XmlTypeMapping realTypeMap = typeMap.GetRealTypeMap(ob.GetType());
		if (realTypeMap == null)
		{
			if (ob.GetType().IsArray && typeof(XmlNode).IsAssignableFrom(ob.GetType().GetElementType()))
			{
				base.Writer.WriteStartElement(element, namesp);
				foreach (XmlNode item in (IEnumerable)ob)
				{
					item.WriteTo(base.Writer);
				}
				base.Writer.WriteEndElement();
			}
			else
			{
				WriteTypedPrimitive(element, namesp, ob, xsiType: true);
			}
			return;
		}
		if (writeWrappingElem)
		{
			if (realTypeMap != typeMap || _format == SerializationFormat.Encoded)
			{
				needType = true;
			}
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType(realTypeMap.XmlType, realTypeMap.XmlTypeNamespace);
		}
		switch (realTypeMap.TypeData.SchemaType)
		{
		case SchemaTypes.Class:
			WriteObjectElement(realTypeMap, ob, element, namesp);
			break;
		case SchemaTypes.Array:
			WriteListElement(realTypeMap, ob, element, namesp);
			break;
		case SchemaTypes.Primitive:
			WritePrimitiveElement(realTypeMap, ob, element, namesp);
			break;
		case SchemaTypes.Enum:
			WriteEnumElement(realTypeMap, ob, element, namesp);
			break;
		}
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	protected virtual void WriteMessage(XmlMembersMapping membersMap, object[] parameters)
	{
		if (membersMap.HasWrapperElement)
		{
			TopLevelElement();
			WriteStartElement(membersMap.ElementName, membersMap.Namespace, _format == SerializationFormat.Encoded);
			if (base.Writer.LookupPrefix("http://www.w3.org/2001/XMLSchema") == null)
			{
				WriteAttribute("xmlns", "xsd", "http://www.w3.org/2001/XMLSchema", "http://www.w3.org/2001/XMLSchema");
			}
			if (base.Writer.LookupPrefix("http://www.w3.org/2001/XMLSchema-instance") == null)
			{
				WriteAttribute("xmlns", "xsi", "http://www.w3.org/2001/XMLSchema-instance", "http://www.w3.org/2001/XMLSchema-instance");
			}
		}
		WriteMembers((ClassMap)membersMap.ObjectMap, parameters, isValueList: true);
		if (membersMap.HasWrapperElement)
		{
			WriteEndElement();
		}
	}

	protected virtual void WriteObjectElement(XmlTypeMapping typeMap, object ob, string element, string namesp)
	{
		ClassMap classMap = (ClassMap)typeMap.ObjectMap;
		if (classMap.NamespaceDeclarations != null)
		{
			WriteNamespaceDeclarations((XmlSerializerNamespaces)classMap.NamespaceDeclarations.GetValue(ob));
		}
		WriteObjectElementAttributes(typeMap, ob);
		WriteObjectElementElements(typeMap, ob);
	}

	protected virtual void WriteObjectElementAttributes(XmlTypeMapping typeMap, object ob)
	{
		ClassMap map = (ClassMap)typeMap.ObjectMap;
		WriteAttributeMembers(map, ob, isValueList: false);
	}

	protected virtual void WriteObjectElementElements(XmlTypeMapping typeMap, object ob)
	{
		ClassMap map = (ClassMap)typeMap.ObjectMap;
		WriteElementMembers(map, ob, isValueList: false);
	}

	private void WriteMembers(ClassMap map, object ob, bool isValueList)
	{
		WriteAttributeMembers(map, ob, isValueList);
		WriteElementMembers(map, ob, isValueList);
	}

	private void WriteAttributeMembers(ClassMap map, object ob, bool isValueList)
	{
		XmlTypeMapMember defaultAnyAttributeMember = map.DefaultAnyAttributeMember;
		if (defaultAnyAttributeMember != null && MemberHasValue(defaultAnyAttributeMember, ob, isValueList))
		{
			ICollection collection = (ICollection)GetMemberValue(defaultAnyAttributeMember, ob, isValueList);
			if (collection != null)
			{
				foreach (XmlAttribute item in collection)
				{
					if (item.NamespaceURI != "http://www.w3.org/2000/xmlns/")
					{
						WriteXmlAttribute(item, ob);
					}
				}
			}
		}
		ICollection attributeMembers = map.AttributeMembers;
		if (attributeMembers == null)
		{
			return;
		}
		foreach (XmlTypeMapMemberAttribute item2 in attributeMembers)
		{
			if (MemberHasValue(item2, ob, isValueList))
			{
				WriteAttribute(item2.AttributeName, item2.Namespace, GetStringValue(item2.MappedType, item2.TypeData, GetMemberValue(item2, ob, isValueList)));
			}
		}
	}

	private void WriteElementMembers(ClassMap map, object ob, bool isValueList)
	{
		ICollection elementMembers = map.ElementMembers;
		if (elementMembers == null)
		{
			return;
		}
		foreach (XmlTypeMapMemberElement item in elementMembers)
		{
			if (!MemberHasValue(item, ob, isValueList))
			{
				continue;
			}
			object memberValue = GetMemberValue(item, ob, isValueList);
			Type type = item.GetType();
			if (type == typeof(XmlTypeMapMemberList))
			{
				WriteMemberElement((XmlTypeMapElementInfo)item.ElementInfo[0], memberValue);
			}
			else if (type == typeof(XmlTypeMapMemberFlatList))
			{
				if (memberValue != null)
				{
					WriteListContent(ob, item.TypeData, ((XmlTypeMapMemberFlatList)item).ListMap, memberValue, null);
				}
			}
			else if (type == typeof(XmlTypeMapMemberAnyElement))
			{
				if (memberValue != null)
				{
					WriteAnyElementContent((XmlTypeMapMemberAnyElement)item, memberValue);
				}
			}
			else if (!(type == typeof(XmlTypeMapMemberAnyAttribute)))
			{
				if (!(type == typeof(XmlTypeMapMemberElement)))
				{
					throw new InvalidOperationException("Unknown member type");
				}
				XmlTypeMapElementInfo elem = item.FindElement(ob, memberValue);
				WriteMemberElement(elem, memberValue);
			}
		}
	}

	private object GetMemberValue(XmlTypeMapMember member, object ob, bool isValueList)
	{
		if (isValueList)
		{
			return ((object[])ob)[member.GlobalIndex];
		}
		return member.GetValue(ob);
	}

	private bool MemberHasValue(XmlTypeMapMember member, object ob, bool isValueList)
	{
		if (isValueList)
		{
			if (member.IsOptionalValueType && !member.GetValueSpecified(ob))
			{
				return false;
			}
			return member.GlobalIndex < ((object[])ob).Length;
		}
		if (member.DefaultValue != DBNull.Value)
		{
			object obj = GetMemberValue(member, ob, isValueList);
			if (obj == null && member.DefaultValue == null)
			{
				return false;
			}
			if (obj != null && obj.GetType().IsEnum)
			{
				if (obj.Equals(member.DefaultValue))
				{
					return false;
				}
				Type underlyingType = Enum.GetUnderlyingType(obj.GetType());
				obj = Convert.ChangeType(obj, underlyingType, null);
			}
			if (obj != null && obj.Equals(member.DefaultValue))
			{
				return false;
			}
		}
		else if (member.IsOptionalValueType)
		{
			return member.GetValueSpecified(ob);
		}
		return true;
	}

	private void WriteMemberElement(XmlTypeMapElementInfo elem, object memberValue)
	{
		switch (elem.TypeData.SchemaType)
		{
		case SchemaTypes.XmlNode:
		{
			string name = (elem.WrappedElement ? elem.ElementName : "");
			if (_format == SerializationFormat.Literal)
			{
				WriteElementLiteral((XmlNode)memberValue, name, elem.Namespace, elem.IsNullable, any: false);
			}
			else
			{
				WriteElementEncoded((XmlNode)memberValue, name, elem.Namespace, elem.IsNullable, any: false);
			}
			break;
		}
		case SchemaTypes.Primitive:
		case SchemaTypes.Enum:
			if (_format == SerializationFormat.Literal)
			{
				WritePrimitiveValueLiteral(memberValue, elem.ElementName, elem.Namespace, elem.MappedType, elem.TypeData, elem.WrappedElement, elem.IsNullable);
			}
			else
			{
				WritePrimitiveValueEncoded(memberValue, elem.ElementName, elem.Namespace, new XmlQualifiedName(elem.DataTypeName, elem.DataTypeNamespace), elem.MappedType, elem.TypeData, elem.WrappedElement, elem.IsNullable);
			}
			break;
		case SchemaTypes.Array:
			if (memberValue == null)
			{
				if (elem.IsNullable)
				{
					if (_format == SerializationFormat.Literal)
					{
						WriteNullTagLiteral(elem.ElementName, elem.Namespace);
					}
					else
					{
						WriteNullTagEncoded(elem.ElementName, elem.Namespace);
					}
				}
			}
			else if (elem.MappedType.MultiReferenceType)
			{
				WriteReferencingElement(elem.ElementName, elem.Namespace, memberValue, elem.IsNullable);
			}
			else
			{
				WriteStartElement(elem.ElementName, elem.Namespace, memberValue);
				WriteListContent(null, elem.TypeData, (ListMap)elem.MappedType.ObjectMap, memberValue, null);
				WriteEndElement(memberValue);
			}
			break;
		case SchemaTypes.Class:
			if (elem.MappedType.MultiReferenceType)
			{
				if (elem.MappedType.TypeData.Type == typeof(object))
				{
					WritePotentiallyReferencingElement(elem.ElementName, elem.Namespace, memberValue, null, suppressReference: false, elem.IsNullable);
				}
				else
				{
					WriteReferencingElement(elem.ElementName, elem.Namespace, memberValue, elem.IsNullable);
				}
			}
			else
			{
				WriteObject(elem.MappedType, memberValue, elem.ElementName, elem.Namespace, elem.IsNullable, needType: false, writeWrappingElem: true);
			}
			break;
		case SchemaTypes.XmlSerializable:
			if (!elem.MappedType.TypeData.Type.IsInstanceOfType(memberValue))
			{
				memberValue = ImplicitConvert(memberValue, elem.MappedType.TypeData.Type);
			}
			WriteSerializable((IXmlSerializable)memberValue, elem.ElementName, elem.Namespace, elem.IsNullable);
			break;
		default:
			throw new NotSupportedException("Invalid value type");
		}
	}

	internal static object ImplicitConvert(object obj, Type type)
	{
		if (obj == null)
		{
			return null;
		}
		Type type2 = obj.GetType();
		while (type2 != typeof(object))
		{
			MethodInfo method = type2.GetMethod("op_Implicit", new Type[1] { type2 });
			if (method != null && method.ReturnType == type)
			{
				return method.Invoke(null, new object[1] { obj });
			}
			method = type.GetMethod("op_Implicit", new Type[1] { type2 });
			if (method != null && method.ReturnType == type)
			{
				return method.Invoke(null, new object[1] { obj });
			}
			type2 = type2.BaseType;
		}
		return obj;
	}

	private void WritePrimitiveValueLiteral(object memberValue, string name, string ns, XmlTypeMapping mappedType, TypeData typeData, bool wrapped, bool isNullable)
	{
		if (!wrapped)
		{
			WriteValue(GetStringValue(mappedType, typeData, memberValue));
		}
		else if (isNullable)
		{
			if (typeData.Type == typeof(XmlQualifiedName))
			{
				WriteNullableQualifiedNameLiteral(name, ns, (XmlQualifiedName)memberValue);
			}
			else
			{
				WriteNullableStringLiteral(name, ns, GetStringValue(mappedType, typeData, memberValue));
			}
		}
		else if (typeData.Type == typeof(XmlQualifiedName))
		{
			WriteElementQualifiedName(name, ns, (XmlQualifiedName)memberValue);
		}
		else
		{
			WriteElementString(name, ns, GetStringValue(mappedType, typeData, memberValue));
		}
	}

	private void WritePrimitiveValueEncoded(object memberValue, string name, string ns, XmlQualifiedName xsiType, XmlTypeMapping mappedType, TypeData typeData, bool wrapped, bool isNullable)
	{
		if (!wrapped)
		{
			WriteValue(GetStringValue(mappedType, typeData, memberValue));
		}
		else if (isNullable)
		{
			if (typeData.Type == typeof(XmlQualifiedName))
			{
				WriteNullableQualifiedNameEncoded(name, ns, (XmlQualifiedName)memberValue, xsiType);
			}
			else
			{
				WriteNullableStringEncoded(name, ns, GetStringValue(mappedType, typeData, memberValue), xsiType);
			}
		}
		else if (typeData.Type == typeof(XmlQualifiedName))
		{
			WriteElementQualifiedName(name, ns, (XmlQualifiedName)memberValue, xsiType);
		}
		else
		{
			WriteElementString(name, ns, GetStringValue(mappedType, typeData, memberValue), xsiType);
		}
	}

	protected virtual void WriteListElement(XmlTypeMapping typeMap, object ob, string element, string namesp)
	{
		if (_format == SerializationFormat.Encoded)
		{
			int listCount = GetListCount(typeMap.TypeData, ob);
			((ListMap)typeMap.ObjectMap).GetArrayType(listCount, out var localName, out var ns);
			string value = ((ns != string.Empty) ? FromXmlQualifiedName(new XmlQualifiedName(localName, ns)) : localName);
			WriteAttribute("arrayType", "http://schemas.xmlsoap.org/soap/encoding/", value);
		}
		WriteListContent(null, typeMap.TypeData, (ListMap)typeMap.ObjectMap, ob, null);
	}

	private void WriteListContent(object container, TypeData listType, ListMap map, object ob, StringBuilder targetString)
	{
		if (listType.Type.IsArray)
		{
			Array array = (Array)ob;
			for (int i = 0; i < array.Length; i++)
			{
				object value = array.GetValue(i);
				XmlTypeMapElementInfo xmlTypeMapElementInfo = map.FindElement(container, i, value);
				if (xmlTypeMapElementInfo != null && targetString == null)
				{
					WriteMemberElement(xmlTypeMapElementInfo, value);
				}
				else if (xmlTypeMapElementInfo != null && targetString != null)
				{
					targetString.Append(GetStringValue(xmlTypeMapElementInfo.MappedType, xmlTypeMapElementInfo.TypeData, value)).Append(" ");
				}
				else if (value != null)
				{
					throw CreateUnknownTypeException(value);
				}
			}
			return;
		}
		if (ob is ICollection)
		{
			int num = (int)ob.GetType().GetProperty("Count").GetValue(ob, null);
			PropertyInfo indexerProperty = TypeData.GetIndexerProperty(listType.Type);
			object[] array2 = new object[1];
			for (int j = 0; j < num; j++)
			{
				array2[0] = j;
				object value2 = indexerProperty.GetValue(ob, array2);
				XmlTypeMapElementInfo xmlTypeMapElementInfo2 = map.FindElement(container, j, value2);
				if (xmlTypeMapElementInfo2 != null && targetString == null)
				{
					WriteMemberElement(xmlTypeMapElementInfo2, value2);
				}
				else if (xmlTypeMapElementInfo2 != null && targetString != null)
				{
					targetString.Append(GetStringValue(xmlTypeMapElementInfo2.MappedType, xmlTypeMapElementInfo2.TypeData, value2)).Append(" ");
				}
				else if (value2 != null)
				{
					throw CreateUnknownTypeException(value2);
				}
			}
			return;
		}
		if (ob is IEnumerable)
		{
			foreach (object item in (IEnumerable)ob)
			{
				XmlTypeMapElementInfo xmlTypeMapElementInfo3 = map.FindElement(container, -1, item);
				if (xmlTypeMapElementInfo3 != null && targetString == null)
				{
					WriteMemberElement(xmlTypeMapElementInfo3, item);
				}
				else if (xmlTypeMapElementInfo3 != null && targetString != null)
				{
					targetString.Append(GetStringValue(xmlTypeMapElementInfo3.MappedType, xmlTypeMapElementInfo3.TypeData, item)).Append(" ");
				}
				else if (item != null)
				{
					throw CreateUnknownTypeException(item);
				}
			}
			return;
		}
		throw new Exception("Unsupported collection type");
	}

	private int GetListCount(TypeData listType, object ob)
	{
		if (listType.Type.IsArray)
		{
			return ((Array)ob).Length;
		}
		return (int)listType.Type.GetProperty("Count").GetValue(ob, null);
	}

	private void WriteAnyElementContent(XmlTypeMapMemberAnyElement member, object memberValue)
	{
		if (member.TypeData.Type == typeof(XmlElement) || member.TypeData.Type == typeof(XmlNode))
		{
			memberValue = new object[1] { memberValue };
		}
		foreach (object item in (Array)memberValue)
		{
			if (!(item is XmlNode xmlNode))
			{
				throw new InvalidOperationException($"XmlAnyElementAttribute can only be applied to members of type XmlElement, XmlElement[] or XmlNode[]. The target object is {item?.GetType()}");
			}
			if (xmlNode is XmlElement)
			{
				if (!member.IsElementDefined(xmlNode.Name, xmlNode.NamespaceURI))
				{
					throw CreateUnknownAnyElementException(xmlNode.Name, xmlNode.NamespaceURI);
				}
				if (_format == SerializationFormat.Literal)
				{
					WriteElementLiteral(xmlNode, "", "", isNullable: false, any: true);
				}
				else
				{
					WriteElementEncoded(xmlNode, "", "", isNullable: false, any: true);
				}
			}
			else
			{
				xmlNode.WriteTo(base.Writer);
			}
		}
	}

	protected virtual void WritePrimitiveElement(XmlTypeMapping typeMap, object ob, string element, string namesp)
	{
		base.Writer.WriteString(GetStringValue(typeMap, typeMap.TypeData, ob));
	}

	protected virtual void WriteEnumElement(XmlTypeMapping typeMap, object ob, string element, string namesp)
	{
		base.Writer.WriteString(GetEnumXmlValue(typeMap, ob));
	}

	private string GetStringValue(XmlTypeMapping typeMap, TypeData type, object value)
	{
		if (type.SchemaType == SchemaTypes.Array)
		{
			if (value == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			WriteListContent(null, typeMap.TypeData, (ListMap)typeMap.ObjectMap, value, stringBuilder);
			return stringBuilder.ToString().Trim();
		}
		if (type.SchemaType == SchemaTypes.Enum)
		{
			return GetEnumXmlValue(typeMap, value);
		}
		if (type.Type == typeof(XmlQualifiedName))
		{
			return FromXmlQualifiedName((XmlQualifiedName)value);
		}
		if (value == null)
		{
			return null;
		}
		return XmlCustomFormatter.ToXmlString(type, value);
	}

	private string GetEnumXmlValue(XmlTypeMapping typeMap, object ob)
	{
		if (ob == null)
		{
			return null;
		}
		return ((EnumMap)typeMap.ObjectMap).GetXmlName(typeMap.TypeFullName, ob);
	}
}
