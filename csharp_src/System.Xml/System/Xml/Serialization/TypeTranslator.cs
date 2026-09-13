using System.Collections;
using System.Globalization;
using System.Xml.Schema;

namespace System.Xml.Serialization;

internal class TypeTranslator
{
	private static Hashtable nameCache;

	private static Hashtable primitiveTypes;

	private static Hashtable primitiveArrayTypes;

	private static Hashtable nullableTypes;

	static TypeTranslator()
	{
		nameCache = new Hashtable();
		primitiveArrayTypes = Hashtable.Synchronized(new Hashtable());
		nameCache = Hashtable.Synchronized(nameCache);
		nameCache.Add(typeof(bool), new TypeData(typeof(bool), "boolean", isPrimitive: true));
		nameCache.Add(typeof(short), new TypeData(typeof(short), "short", isPrimitive: true));
		nameCache.Add(typeof(ushort), new TypeData(typeof(ushort), "unsignedShort", isPrimitive: true));
		nameCache.Add(typeof(int), new TypeData(typeof(int), "int", isPrimitive: true));
		nameCache.Add(typeof(uint), new TypeData(typeof(uint), "unsignedInt", isPrimitive: true));
		nameCache.Add(typeof(long), new TypeData(typeof(long), "long", isPrimitive: true));
		nameCache.Add(typeof(ulong), new TypeData(typeof(ulong), "unsignedLong", isPrimitive: true));
		nameCache.Add(typeof(float), new TypeData(typeof(float), "float", isPrimitive: true));
		nameCache.Add(typeof(double), new TypeData(typeof(double), "double", isPrimitive: true));
		nameCache.Add(typeof(DateTime), new TypeData(typeof(DateTime), "dateTime", isPrimitive: true));
		nameCache.Add(typeof(decimal), new TypeData(typeof(decimal), "decimal", isPrimitive: true));
		nameCache.Add(typeof(XmlQualifiedName), new TypeData(typeof(XmlQualifiedName), "QName", isPrimitive: true));
		nameCache.Add(typeof(string), new TypeData(typeof(string), "string", isPrimitive: true));
		XmlSchemaPatternFacet facet = new XmlSchemaPatternFacet
		{
			Value = "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}"
		};
		nameCache.Add(typeof(Guid), new TypeData(typeof(Guid), "guid", isPrimitive: true, (TypeData)nameCache[typeof(string)], facet));
		nameCache.Add(typeof(byte), new TypeData(typeof(byte), "unsignedByte", isPrimitive: true));
		nameCache.Add(typeof(sbyte), new TypeData(typeof(sbyte), "byte", isPrimitive: true));
		nameCache.Add(typeof(char), new TypeData(typeof(char), "char", isPrimitive: true, (TypeData)nameCache[typeof(ushort)], null));
		nameCache.Add(typeof(object), new TypeData(typeof(object), "anyType", isPrimitive: false));
		nameCache.Add(typeof(byte[]), new TypeData(typeof(byte[]), "base64Binary", isPrimitive: true));
		nameCache.Add(typeof(XmlNode), new TypeData(typeof(XmlNode), "XmlNode", isPrimitive: false));
		nameCache.Add(typeof(XmlElement), new TypeData(typeof(XmlElement), "XmlElement", isPrimitive: false));
		primitiveTypes = new Hashtable();
		foreach (TypeData value2 in nameCache.Values)
		{
			primitiveTypes.Add(value2.XmlType, value2);
		}
		primitiveTypes.Add("date", new TypeData(typeof(DateTime), "date", isPrimitive: true));
		primitiveTypes.Add("time", new TypeData(typeof(DateTime), "time", isPrimitive: true));
		primitiveTypes.Add("timePeriod", new TypeData(typeof(DateTime), "timePeriod", isPrimitive: true));
		primitiveTypes.Add("gDay", new TypeData(typeof(string), "gDay", isPrimitive: true));
		primitiveTypes.Add("gMonthDay", new TypeData(typeof(string), "gMonthDay", isPrimitive: true));
		primitiveTypes.Add("gYear", new TypeData(typeof(string), "gYear", isPrimitive: true));
		primitiveTypes.Add("gYearMonth", new TypeData(typeof(string), "gYearMonth", isPrimitive: true));
		primitiveTypes.Add("month", new TypeData(typeof(DateTime), "month", isPrimitive: true));
		primitiveTypes.Add("NMTOKEN", new TypeData(typeof(string), "NMTOKEN", isPrimitive: true));
		primitiveTypes.Add("NMTOKENS", new TypeData(typeof(string), "NMTOKENS", isPrimitive: true));
		primitiveTypes.Add("Name", new TypeData(typeof(string), "Name", isPrimitive: true));
		primitiveTypes.Add("NCName", new TypeData(typeof(string), "NCName", isPrimitive: true));
		primitiveTypes.Add("language", new TypeData(typeof(string), "language", isPrimitive: true));
		primitiveTypes.Add("integer", new TypeData(typeof(string), "integer", isPrimitive: true));
		primitiveTypes.Add("positiveInteger", new TypeData(typeof(string), "positiveInteger", isPrimitive: true));
		primitiveTypes.Add("nonPositiveInteger", new TypeData(typeof(string), "nonPositiveInteger", isPrimitive: true));
		primitiveTypes.Add("negativeInteger", new TypeData(typeof(string), "negativeInteger", isPrimitive: true));
		primitiveTypes.Add("nonNegativeInteger", new TypeData(typeof(string), "nonNegativeInteger", isPrimitive: true));
		primitiveTypes.Add("ENTITIES", new TypeData(typeof(string), "ENTITIES", isPrimitive: true));
		primitiveTypes.Add("ENTITY", new TypeData(typeof(string), "ENTITY", isPrimitive: true));
		primitiveTypes.Add("hexBinary", new TypeData(typeof(byte[]), "hexBinary", isPrimitive: true));
		primitiveTypes.Add("ID", new TypeData(typeof(string), "ID", isPrimitive: true));
		primitiveTypes.Add("IDREF", new TypeData(typeof(string), "IDREF", isPrimitive: true));
		primitiveTypes.Add("IDREFS", new TypeData(typeof(string), "IDREFS", isPrimitive: true));
		primitiveTypes.Add("NOTATION", new TypeData(typeof(string), "NOTATION", isPrimitive: true));
		primitiveTypes.Add("token", new TypeData(typeof(string), "token", isPrimitive: true));
		primitiveTypes.Add("normalizedString", new TypeData(typeof(string), "normalizedString", isPrimitive: true));
		primitiveTypes.Add("anyURI", new TypeData(typeof(string), "anyURI", isPrimitive: true));
		primitiveTypes.Add("base64", new TypeData(typeof(byte[]), "base64", isPrimitive: true));
		primitiveTypes.Add("duration", new TypeData(typeof(string), "duration", isPrimitive: true));
		nullableTypes = Hashtable.Synchronized(new Hashtable());
		foreach (DictionaryEntry primitiveType in primitiveTypes)
		{
			TypeData typeData2 = (TypeData)primitiveType.Value;
			TypeData value = new TypeData(typeData2.Type, typeData2.XmlType, isPrimitive: true)
			{
				IsNullable = true
			};
			nullableTypes.Add(primitiveType.Key, value);
		}
	}

	public static TypeData GetTypeData(Type type)
	{
		return GetTypeData(type, null);
	}

	public static TypeData GetTypeData(Type runtimeType, string xmlDataType, bool underlyingEnumType = false)
	{
		if (underlyingEnumType && runtimeType.IsEnum)
		{
			runtimeType = Enum.GetUnderlyingType(runtimeType);
		}
		Type type = runtimeType;
		bool flag = false;
		if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
		{
			flag = true;
			type = type.GetGenericArguments()[0];
		}
		if (xmlDataType != null && xmlDataType.Length != 0)
		{
			TypeData primitiveTypeData = GetPrimitiveTypeData(xmlDataType);
			if (type.IsArray && type != primitiveTypeData.Type)
			{
				TypeData typeData = (TypeData)primitiveArrayTypes[xmlDataType];
				if (typeData != null)
				{
					return typeData;
				}
				if (primitiveTypeData.Type == type.GetElementType())
				{
					typeData = new TypeData(type, GetArrayName(primitiveTypeData.XmlType), isPrimitive: false);
					primitiveArrayTypes[xmlDataType] = typeData;
					return typeData;
				}
				throw new InvalidOperationException("Cannot convert values of type '" + type.GetElementType()?.ToString() + "' to '" + xmlDataType + "'");
			}
			if (flag)
			{
				TypeData typeData2 = (TypeData)nullableTypes[primitiveTypeData.XmlType];
				if (typeData2 == null)
				{
					typeData2 = new TypeData(type, primitiveTypeData.XmlType, isPrimitive: false);
					typeData2.IsNullable = true;
					nullableTypes[primitiveTypeData.XmlType] = typeData2;
				}
				return typeData2;
			}
			return primitiveTypeData;
		}
		if (flag)
		{
			TypeData typeData3 = GetTypeData(type);
			if (typeData3 != null)
			{
				TypeData typeData4 = (TypeData)nullableTypes[typeData3.XmlType];
				if (typeData4 == null)
				{
					typeData4 = new TypeData(type, typeData3.XmlType, isPrimitive: false);
					typeData4.IsNullable = true;
					nullableTypes[typeData3.XmlType] = typeData4;
				}
				return typeData4;
			}
		}
		if (nameCache[runtimeType] is TypeData result)
		{
			return result;
		}
		string text;
		if (type.IsArray)
		{
			text = GetArrayName(GetTypeData(type.GetElementType()).XmlType);
		}
		else if (type.IsGenericType && !type.IsGenericTypeDefinition)
		{
			text = XmlConvert.EncodeLocalName(type.Name.Substring(0, type.Name.IndexOf('`'))) + "Of";
			Type[] genericArguments = type.GetGenericArguments();
			foreach (Type type2 in genericArguments)
			{
				text += ((type2.IsArray || type2.IsGenericType) ? GetTypeData(type2).XmlType : CodeIdentifier.MakePascal(XmlConvert.EncodeLocalName(type2.Name)));
			}
		}
		else
		{
			text = XmlConvert.EncodeLocalName(type.Name);
		}
		TypeData typeData5 = new TypeData(type, text, isPrimitive: false);
		if (flag)
		{
			typeData5.IsNullable = true;
		}
		nameCache[runtimeType] = typeData5;
		return typeData5;
	}

	public static bool IsPrimitive(Type type)
	{
		return GetTypeData(type).SchemaType == SchemaTypes.Primitive;
	}

	public static TypeData GetPrimitiveTypeData(string typeName)
	{
		return GetPrimitiveTypeData(typeName, nullable: false);
	}

	public static TypeData GetPrimitiveTypeData(string typeName, bool nullable)
	{
		TypeData typeData = (TypeData)primitiveTypes[typeName];
		if (typeData != null && !typeData.Type.IsValueType)
		{
			return typeData;
		}
		typeData = (TypeData)((nullable && nullableTypes != null) ? nullableTypes : primitiveTypes)[typeName];
		if (typeData == null)
		{
			throw new NotSupportedException("Data type '" + typeName + "' not supported");
		}
		return typeData;
	}

	public static TypeData FindPrimitiveTypeData(string typeName)
	{
		return (TypeData)primitiveTypes[typeName];
	}

	public static TypeData GetDefaultPrimitiveTypeData(TypeData primType)
	{
		if (primType.SchemaType == SchemaTypes.Primitive)
		{
			TypeData typeData = GetTypeData(primType.Type, null);
			if (typeData != primType)
			{
				return typeData;
			}
		}
		return primType;
	}

	public static bool IsDefaultPrimitiveTpeData(TypeData primType)
	{
		return GetDefaultPrimitiveTypeData(primType) == primType;
	}

	public static TypeData CreateCustomType(string typeName, string fullTypeName, string xmlType, SchemaTypes schemaType, TypeData listItemTypeData)
	{
		return new TypeData(typeName, fullTypeName, xmlType, schemaType, listItemTypeData);
	}

	public static string GetArrayName(string elemName)
	{
		return "ArrayOf" + char.ToUpper(elemName[0], CultureInfo.InvariantCulture) + elemName.Substring(1);
	}

	public static string GetArrayName(string elemName, int dimensions)
	{
		string text = GetArrayName(elemName);
		while (dimensions > 1)
		{
			text = "ArrayOf" + text;
			dimensions--;
		}
		return text;
	}

	public static void ParseArrayType(string arrayType, out string type, out string ns, out string dimensions)
	{
		int num = arrayType.LastIndexOf(":");
		if (num == -1)
		{
			ns = "";
		}
		else
		{
			ns = arrayType.Substring(0, num);
		}
		int num2 = arrayType.IndexOf("[", num + 1);
		if (num2 == -1)
		{
			throw new InvalidOperationException("Cannot parse WSDL array type: " + arrayType);
		}
		type = arrayType.Substring(num + 1, num2 - num - 1);
		dimensions = arrayType.Substring(num2);
	}
}
