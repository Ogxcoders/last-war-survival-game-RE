using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml.Schema;

namespace System.Xml.Serialization;

internal class TypeData
{
	private Type type;

	private string elementName;

	private SchemaTypes sType;

	private Type listItemType;

	private string typeName;

	private string fullTypeName;

	private string csharpName;

	private string csharpFullName;

	private TypeData listItemTypeData;

	private TypeData listTypeData;

	private TypeData mappedType;

	private XmlSchemaPatternFacet facet;

	private MethodInfo typeConvertor;

	private bool hasPublicConstructor = true;

	private bool nullableOverride;

	private static Hashtable keywordsTable;

	private static string[] keywords = new string[80]
	{
		"abstract", "event", "new", "struct", "as", "explicit", "null", "switch", "base", "extern",
		"this", "false", "operator", "throw", "break", "finally", "out", "true", "fixed", "override",
		"try", "case", "params", "typeof", "catch", "for", "private", "foreach", "protected", "checked",
		"goto", "public", "unchecked", "class", "if", "readonly", "unsafe", "const", "implicit", "ref",
		"continue", "in", "return", "using", "virtual", "default", "interface", "sealed", "volatile", "delegate",
		"internal", "do", "is", "sizeof", "while", "lock", "stackalloc", "else", "static", "enum",
		"namespace", "object", "bool", "byte", "float", "uint", "char", "ulong", "ushort", "decimal",
		"int", "sbyte", "short", "double", "long", "string", "void", "partial", "yield", "where"
	};

	public string TypeName => typeName;

	public string XmlType => elementName;

	public Type Type => type;

	public string FullTypeName => fullTypeName;

	public string CSharpName
	{
		get
		{
			if (csharpName == null)
			{
				csharpName = ((Type == null) ? TypeName : ToCSharpName(Type, full: false));
			}
			return csharpName;
		}
	}

	public string CSharpFullName
	{
		get
		{
			if (csharpFullName == null)
			{
				csharpFullName = ((Type == null) ? TypeName : ToCSharpName(Type, full: true));
			}
			return csharpFullName;
		}
	}

	public SchemaTypes SchemaType => sType;

	public bool IsListType => SchemaType == SchemaTypes.Array;

	public bool IsComplexType
	{
		get
		{
			if (SchemaType != SchemaTypes.Class && SchemaType != SchemaTypes.Array && SchemaType != SchemaTypes.Enum && SchemaType != SchemaTypes.XmlNode && SchemaType != SchemaTypes.XmlSerializable)
			{
				return !IsXsdType;
			}
			return true;
		}
	}

	public bool IsValueType
	{
		get
		{
			if (type != null)
			{
				return type.IsValueType;
			}
			if (sType != SchemaTypes.Primitive)
			{
				return sType == SchemaTypes.Enum;
			}
			return true;
		}
	}

	public bool NullableOverride => nullableOverride;

	public bool IsNullable
	{
		get
		{
			if (nullableOverride)
			{
				return true;
			}
			if (IsValueType)
			{
				if (type != null && type.IsGenericType)
				{
					return type.GetGenericTypeDefinition() == typeof(Nullable<>);
				}
				return false;
			}
			return true;
		}
		set
		{
			nullableOverride = value;
		}
	}

	public TypeData ListItemTypeData
	{
		get
		{
			if (listItemTypeData == null && type != null)
			{
				listItemTypeData = TypeTranslator.GetTypeData(ListItemType);
			}
			return listItemTypeData;
		}
	}

	public Type ListItemType
	{
		get
		{
			if (this.type == null)
			{
				throw new InvalidOperationException("Property ListItemType is not supported for custom types");
			}
			if (listItemType != null)
			{
				return listItemType;
			}
			Type type = null;
			if (SchemaType != SchemaTypes.Array)
			{
				throw new InvalidOperationException(Type.FullName + " is not a collection");
			}
			if (this.type.IsArray)
			{
				listItemType = this.type.GetElementType();
			}
			else if (typeof(ICollection).IsAssignableFrom(this.type) || (type = GetGenericListItemType(this.type)) != null)
			{
				if (typeof(IDictionary).IsAssignableFrom(this.type))
				{
					throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "The type {0} is not supported because it implements IDictionary.", this.type.FullName));
				}
				if (type != null)
				{
					listItemType = type;
				}
				else
				{
					PropertyInfo indexerProperty = GetIndexerProperty(this.type);
					if (indexerProperty == null)
					{
						throw new InvalidOperationException("You must implement a default accessor on " + this.type.FullName + " because it inherits from ICollection");
					}
					listItemType = indexerProperty.PropertyType;
				}
				if (this.type.GetMethod("Add", new Type[1] { listItemType }) == null)
				{
					throw CreateMissingAddMethodException(this.type, "ICollection", listItemType);
				}
			}
			else
			{
				MethodInfo method = this.type.GetMethod("GetEnumerator", Type.EmptyTypes);
				if (method == null)
				{
					method = this.type.GetMethod("System.Collections.IEnumerable.GetEnumerator", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
				}
				PropertyInfo property = method.ReturnType.GetProperty("Current");
				if (property == null)
				{
					listItemType = typeof(object);
				}
				else
				{
					listItemType = property.PropertyType;
				}
				if (this.type.GetMethod("Add", new Type[1] { listItemType }) == null)
				{
					throw CreateMissingAddMethodException(this.type, "IEnumerable", listItemType);
				}
			}
			return listItemType;
		}
	}

	public TypeData ListTypeData
	{
		get
		{
			if (listTypeData != null)
			{
				return listTypeData;
			}
			listTypeData = new TypeData(TypeName + "[]", FullTypeName + "[]", TypeTranslator.GetArrayName(XmlType), SchemaTypes.Array, this);
			return listTypeData;
		}
	}

	public bool IsXsdType => mappedType == null;

	public TypeData MappedType
	{
		get
		{
			if (mappedType == null)
			{
				return this;
			}
			return mappedType;
		}
	}

	public XmlSchemaPatternFacet XmlSchemaPatternFacet => facet;

	public bool HasPublicConstructor => hasPublicConstructor;

	public TypeData(Type type, string elementName, bool isPrimitive)
		: this(type, elementName, isPrimitive, null, null)
	{
	}

	public TypeData(Type type, string elementName, bool isPrimitive, TypeData mappedType, XmlSchemaPatternFacet facet)
	{
		if (type.IsGenericTypeDefinition)
		{
			throw new InvalidOperationException("Generic type definition cannot be used in serialization. Only specific generic types can be used.");
		}
		this.mappedType = mappedType;
		this.facet = facet;
		this.type = type;
		typeName = type.Name;
		fullTypeName = type.FullName.Replace('+', '.');
		if (isPrimitive)
		{
			sType = SchemaTypes.Primitive;
		}
		else if (type.IsEnum)
		{
			sType = SchemaTypes.Enum;
		}
		else if (typeof(IXmlSerializable).IsAssignableFrom(type))
		{
			sType = SchemaTypes.XmlSerializable;
		}
		else if (typeof(XmlNode).IsAssignableFrom(type))
		{
			sType = SchemaTypes.XmlNode;
		}
		else if (type.IsArray || typeof(IEnumerable).IsAssignableFrom(type))
		{
			sType = SchemaTypes.Array;
		}
		else
		{
			sType = SchemaTypes.Class;
		}
		if (IsListType)
		{
			this.elementName = TypeTranslator.GetArrayName(ListItemTypeData.XmlType);
		}
		else
		{
			this.elementName = elementName;
		}
		if (sType == SchemaTypes.Array || sType == SchemaTypes.Class)
		{
			hasPublicConstructor = !type.IsInterface && (type.IsArray || type.GetConstructor(Type.EmptyTypes) != null || type.IsAbstract || type.IsValueType);
		}
		LookupTypeConvertor();
	}

	internal TypeData(string typeName, string fullTypeName, string xmlType, SchemaTypes schemaType, TypeData listItemTypeData)
	{
		elementName = xmlType;
		this.typeName = typeName;
		this.fullTypeName = fullTypeName.Replace('+', '.');
		this.listItemTypeData = listItemTypeData;
		sType = schemaType;
		hasPublicConstructor = true;
	}

	private void LookupTypeConvertor()
	{
		Type elementType = type;
		if (elementType.IsArray)
		{
			elementType = elementType.GetElementType();
		}
		XmlTypeConvertorAttribute customAttribute = elementType.GetCustomAttribute<XmlTypeConvertorAttribute>();
		if (customAttribute != null)
		{
			typeConvertor = elementType.GetMethod(customAttribute.Method, BindingFlags.Static | BindingFlags.NonPublic);
		}
	}

	internal void ConvertForAssignment(ref object value)
	{
		if (typeConvertor != null)
		{
			value = typeConvertor.Invoke(null, new object[1] { value });
		}
	}

	public static string ToCSharpName(Type type, bool full)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (type.IsArray)
		{
			stringBuilder.Append(ToCSharpName(type.GetElementType(), full));
			stringBuilder.Append('[');
			int arrayRank = type.GetArrayRank();
			for (int i = 1; i < arrayRank; i++)
			{
				stringBuilder.Append(',');
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}
		if (type.IsGenericType && !type.IsGenericTypeDefinition)
		{
			Type[] genericArguments = type.GetGenericArguments();
			int num = genericArguments.Length - 1;
			Type type2 = type;
			while (type2 != null)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Insert(0, '.');
				}
				int num2 = type2.Name.IndexOf('`');
				if (num2 != -1)
				{
					int num3 = num - int.Parse(type2.Name.Substring(num2 + 1));
					stringBuilder.Insert(0, '>');
					while (num > num3)
					{
						stringBuilder.Insert(0, ToCSharpName(genericArguments[num], full));
						if (num - 1 != num3)
						{
							stringBuilder.Insert(0, ',');
						}
						num--;
					}
					stringBuilder.Insert(0, '<');
					stringBuilder.Insert(0, type2.Name.Substring(0, num2));
				}
				else
				{
					stringBuilder.Insert(0, type2.Name);
				}
				type2 = type2.DeclaringType;
			}
			if (full && type.Namespace.Length > 0)
			{
				stringBuilder.Insert(0, type.Namespace + ".");
			}
			return stringBuilder.ToString();
		}
		if (type.DeclaringType != null)
		{
			stringBuilder.Append(ToCSharpName(type.DeclaringType, full)).Append('.');
			stringBuilder.Append(type.Name);
		}
		else
		{
			if (full && !string.IsNullOrEmpty(type.Namespace))
			{
				stringBuilder.Append(type.Namespace).Append('.');
			}
			stringBuilder.Append(type.Name);
		}
		return stringBuilder.ToString();
	}

	private static bool IsKeyword(string name)
	{
		if (keywordsTable == null)
		{
			Hashtable hashtable = new Hashtable();
			string[] array = keywords;
			foreach (string text in array)
			{
				hashtable[text] = text;
			}
			keywordsTable = hashtable;
		}
		return keywordsTable.Contains(name);
	}

	public static PropertyInfo GetIndexerProperty(Type collectionType)
	{
		PropertyInfo[] properties = collectionType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
		foreach (PropertyInfo propertyInfo in properties)
		{
			ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
			if (indexParameters != null && indexParameters.Length == 1 && indexParameters[0].ParameterType == typeof(int))
			{
				return propertyInfo;
			}
		}
		return null;
	}

	private static InvalidOperationException CreateMissingAddMethodException(Type type, string inheritFrom, Type argumentType)
	{
		return new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "To be XML serializable, types which inherit from {0} must have an implementation of Add({1}) at all levels of their inheritance hierarchy. {2} does not implement Add({1}).", inheritFrom, argumentType.FullName, type.FullName));
	}

	internal static Type GetGenericListItemType(Type type)
	{
		if (type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type.GetGenericTypeDefinition()))
		{
			Type[] genericArguments = type.GetGenericArguments();
			if (type.GetMethod("Add", genericArguments) != null)
			{
				return genericArguments[0];
			}
		}
		Type type2 = null;
		Type[] interfaces = type.GetInterfaces();
		for (int i = 0; i < interfaces.Length; i++)
		{
			if ((type2 = GetGenericListItemType(interfaces[i])) != null)
			{
				return type2;
			}
		}
		return null;
	}
}
