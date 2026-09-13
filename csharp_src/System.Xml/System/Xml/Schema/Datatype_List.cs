using System.Collections;

namespace System.Xml.Schema;

internal class Datatype_List : Datatype_anySimpleType
{
	private DatatypeImplementation itemType;

	private int minListSize;

	public override Type ValueType => ListValueType;

	public override XmlTokenizedType TokenizedType => itemType.TokenizedType;

	internal override Type ListValueType => itemType.ListValueType;

	internal override FacetsChecker FacetsChecker => DatatypeImplementation.listFacetsChecker;

	public override XmlTypeCode TypeCode => itemType.TypeCode;

	internal override RestrictionFlags ValidRestrictionFlags => RestrictionFlags.Length | RestrictionFlags.MinLength | RestrictionFlags.MaxLength | RestrictionFlags.Pattern | RestrictionFlags.Enumeration | RestrictionFlags.WhiteSpace;

	internal DatatypeImplementation ItemType => itemType;

	internal override XmlValueConverter CreateValueConverter(XmlSchemaType schemaType)
	{
		XmlSchemaType xmlSchemaType = null;
		XmlSchemaComplexType xmlSchemaComplexType = schemaType as XmlSchemaComplexType;
		XmlSchemaSimpleType xmlSchemaSimpleType;
		if (xmlSchemaComplexType != null)
		{
			do
			{
				xmlSchemaSimpleType = xmlSchemaComplexType.BaseXmlSchemaType as XmlSchemaSimpleType;
				if (xmlSchemaSimpleType != null)
				{
					break;
				}
				xmlSchemaComplexType = xmlSchemaComplexType.BaseXmlSchemaType as XmlSchemaComplexType;
			}
			while (xmlSchemaComplexType != null && xmlSchemaComplexType != XmlSchemaComplexType.AnyType);
		}
		else
		{
			xmlSchemaSimpleType = schemaType as XmlSchemaSimpleType;
		}
		if (xmlSchemaSimpleType != null)
		{
			do
			{
				if (xmlSchemaSimpleType.Content is XmlSchemaSimpleTypeList xmlSchemaSimpleTypeList)
				{
					xmlSchemaType = xmlSchemaSimpleTypeList.BaseItemType;
					break;
				}
				xmlSchemaSimpleType = xmlSchemaSimpleType.BaseXmlSchemaType as XmlSchemaSimpleType;
			}
			while (xmlSchemaSimpleType != null && xmlSchemaSimpleType != DatatypeImplementation.AnySimpleType);
		}
		if (xmlSchemaType == null)
		{
			xmlSchemaType = DatatypeImplementation.GetSimpleTypeFromTypeCode(schemaType.Datatype.TypeCode);
		}
		return XmlListConverter.Create(xmlSchemaType.ValueConverter);
	}

	internal Datatype_List(DatatypeImplementation type)
		: this(type, 0)
	{
	}

	internal Datatype_List(DatatypeImplementation type, int minListSize)
	{
		itemType = type;
		this.minListSize = minListSize;
	}

	internal override int Compare(object value1, object value2)
	{
		Array array = (Array)value1;
		Array array2 = (Array)value2;
		if (array.Length != array2.Length)
		{
			return -1;
		}
		if (array is XmlAtomicValue[] array3)
		{
			XmlAtomicValue[] array4 = array2 as XmlAtomicValue[];
			for (int i = 0; i < array3.Length; i++)
			{
				XmlSchemaType xmlType = array3[i].XmlType;
				if (xmlType != array4[i].XmlType || !xmlType.Datatype.IsEqual(array3[i].TypedValue, array4[i].TypedValue))
				{
					return -1;
				}
			}
			return 0;
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (itemType.Compare(array.GetValue(j), array2.GetValue(j)) != 0)
			{
				return -1;
			}
		}
		return 0;
	}

	internal override Exception TryParseValue(object value, XmlNameTable nameTable, IXmlNamespaceResolver namespaceResolver, out object typedValue)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		string text = value as string;
		typedValue = null;
		if (text != null)
		{
			return TryParseValue(text, nameTable, namespaceResolver, out typedValue);
		}
		Exception ex;
		try
		{
			object obj = ValueConverter.ChangeType(value, ValueType, namespaceResolver);
			Array array = obj as Array;
			bool hasLexicalFacets = itemType.HasLexicalFacets;
			bool hasValueFacets = itemType.HasValueFacets;
			FacetsChecker facetsChecker = itemType.FacetsChecker;
			XmlValueConverter xmlValueConverter = itemType.ValueConverter;
			int num = 0;
			while (true)
			{
				if (num < array.Length)
				{
					object value2 = array.GetValue(num);
					if (hasLexicalFacets)
					{
						string parseString = (string)xmlValueConverter.ChangeType(value2, typeof(string), namespaceResolver);
						ex = facetsChecker.CheckLexicalFacets(ref parseString, itemType);
						if (ex != null)
						{
							break;
						}
					}
					if (hasValueFacets)
					{
						ex = facetsChecker.CheckValueFacets(value2, itemType);
						if (ex != null)
						{
							break;
						}
					}
					num++;
					continue;
				}
				if (HasLexicalFacets)
				{
					string parseString2 = (string)ValueConverter.ChangeType(obj, typeof(string), namespaceResolver);
					ex = DatatypeImplementation.listFacetsChecker.CheckLexicalFacets(ref parseString2, this);
					if (ex != null)
					{
						break;
					}
				}
				if (HasValueFacets)
				{
					ex = DatatypeImplementation.listFacetsChecker.CheckValueFacets(obj, this);
					if (ex != null)
					{
						break;
					}
				}
				typedValue = obj;
				return null;
			}
		}
		catch (FormatException ex2)
		{
			ex = ex2;
		}
		catch (InvalidCastException ex3)
		{
			ex = ex3;
		}
		catch (OverflowException ex4)
		{
			ex = ex4;
		}
		catch (ArgumentException ex5)
		{
			ex = ex5;
		}
		return ex;
	}

	internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
	{
		typedValue = null;
		Exception ex = DatatypeImplementation.listFacetsChecker.CheckLexicalFacets(ref s, this);
		if (ex == null)
		{
			ArrayList arrayList = new ArrayList();
			object obj;
			if (itemType.Variety == XmlSchemaDatatypeVariety.Union)
			{
				string[] array = XmlConvert.SplitString(s);
				int num = 0;
				while (num < array.Length)
				{
					ex = itemType.TryParseValue(array[num], nameTable, nsmgr, out var typedValue2);
					if (ex == null)
					{
						XsdSimpleValue xsdSimpleValue = (XsdSimpleValue)typedValue2;
						arrayList.Add(new XmlAtomicValue(xsdSimpleValue.XmlType, xsdSimpleValue.TypedValue, nsmgr));
						num++;
						continue;
					}
					goto IL_011b;
				}
				obj = arrayList.ToArray(typeof(XmlAtomicValue));
			}
			else
			{
				string[] array2 = XmlConvert.SplitString(s);
				int num2 = 0;
				while (num2 < array2.Length)
				{
					ex = itemType.TryParseValue(array2[num2], nameTable, nsmgr, out typedValue);
					if (ex == null)
					{
						arrayList.Add(typedValue);
						num2++;
						continue;
					}
					goto IL_011b;
				}
				obj = arrayList.ToArray(itemType.ValueType);
			}
			if (arrayList.Count < minListSize)
			{
				return new XmlSchemaException("The attribute value cannot be empty.", string.Empty);
			}
			ex = DatatypeImplementation.listFacetsChecker.CheckValueFacets(obj, this);
			if (ex == null)
			{
				typedValue = obj;
				return null;
			}
		}
		goto IL_011b;
		IL_011b:
		return ex;
	}
}
