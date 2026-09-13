using System.Collections;

namespace System.Xml.Schema;

internal abstract class DatatypeImplementation : XmlSchemaDatatype
{
	private class SchemaDatatypeMap : IComparable
	{
		private string name;

		private DatatypeImplementation type;

		private int parentIndex;

		public string Name => name;

		public int ParentIndex => parentIndex;

		internal SchemaDatatypeMap(string name, DatatypeImplementation type)
		{
			this.name = name;
			this.type = type;
		}

		internal SchemaDatatypeMap(string name, DatatypeImplementation type, int parentIndex)
		{
			this.name = name;
			this.type = type;
			this.parentIndex = parentIndex;
		}

		public static explicit operator DatatypeImplementation(SchemaDatatypeMap sdm)
		{
			return sdm.type;
		}

		public int CompareTo(object obj)
		{
			return string.Compare(name, (string)obj, StringComparison.Ordinal);
		}
	}

	private XmlSchemaDatatypeVariety variety;

	private RestrictionFacets restriction;

	private DatatypeImplementation baseType;

	private XmlValueConverter valueConverter;

	private XmlSchemaType parentSchemaType;

	private static Hashtable builtinTypes;

	private static XmlSchemaSimpleType[] enumToTypeCode;

	private static XmlSchemaSimpleType anySimpleType;

	private static XmlSchemaSimpleType anyAtomicType;

	private static XmlSchemaSimpleType untypedAtomicType;

	private static XmlSchemaSimpleType yearMonthDurationType;

	private static XmlSchemaSimpleType dayTimeDurationType;

	private static volatile XmlSchemaSimpleType normalizedStringTypeV1Compat;

	private static volatile XmlSchemaSimpleType tokenTypeV1Compat;

	private const int anySimpleTypeIndex = 11;

	internal static XmlQualifiedName QnAnySimpleType;

	internal static XmlQualifiedName QnAnyType;

	internal static FacetsChecker stringFacetsChecker;

	internal static FacetsChecker miscFacetsChecker;

	internal static FacetsChecker numeric2FacetsChecker;

	internal static FacetsChecker binaryFacetsChecker;

	internal static FacetsChecker dateTimeFacetsChecker;

	internal static FacetsChecker durationFacetsChecker;

	internal static FacetsChecker listFacetsChecker;

	internal static FacetsChecker qnameFacetsChecker;

	internal static FacetsChecker unionFacetsChecker;

	private static readonly DatatypeImplementation c_anySimpleType;

	private static readonly DatatypeImplementation c_anyURI;

	private static readonly DatatypeImplementation c_base64Binary;

	private static readonly DatatypeImplementation c_boolean;

	private static readonly DatatypeImplementation c_byte;

	private static readonly DatatypeImplementation c_char;

	private static readonly DatatypeImplementation c_date;

	private static readonly DatatypeImplementation c_dateTime;

	private static readonly DatatypeImplementation c_dateTimeNoTz;

	private static readonly DatatypeImplementation c_dateTimeTz;

	private static readonly DatatypeImplementation c_day;

	private static readonly DatatypeImplementation c_decimal;

	private static readonly DatatypeImplementation c_double;

	private static readonly DatatypeImplementation c_doubleXdr;

	private static readonly DatatypeImplementation c_duration;

	private static readonly DatatypeImplementation c_ENTITY;

	private static readonly DatatypeImplementation c_ENTITIES;

	private static readonly DatatypeImplementation c_ENUMERATION;

	private static readonly DatatypeImplementation c_fixed;

	private static readonly DatatypeImplementation c_float;

	private static readonly DatatypeImplementation c_floatXdr;

	private static readonly DatatypeImplementation c_hexBinary;

	private static readonly DatatypeImplementation c_ID;

	private static readonly DatatypeImplementation c_IDREF;

	private static readonly DatatypeImplementation c_IDREFS;

	private static readonly DatatypeImplementation c_int;

	private static readonly DatatypeImplementation c_integer;

	private static readonly DatatypeImplementation c_language;

	private static readonly DatatypeImplementation c_long;

	private static readonly DatatypeImplementation c_month;

	private static readonly DatatypeImplementation c_monthDay;

	private static readonly DatatypeImplementation c_Name;

	private static readonly DatatypeImplementation c_NCName;

	private static readonly DatatypeImplementation c_negativeInteger;

	private static readonly DatatypeImplementation c_NMTOKEN;

	private static readonly DatatypeImplementation c_NMTOKENS;

	private static readonly DatatypeImplementation c_nonNegativeInteger;

	private static readonly DatatypeImplementation c_nonPositiveInteger;

	private static readonly DatatypeImplementation c_normalizedString;

	private static readonly DatatypeImplementation c_NOTATION;

	private static readonly DatatypeImplementation c_positiveInteger;

	private static readonly DatatypeImplementation c_QName;

	private static readonly DatatypeImplementation c_QNameXdr;

	private static readonly DatatypeImplementation c_short;

	private static readonly DatatypeImplementation c_string;

	private static readonly DatatypeImplementation c_time;

	private static readonly DatatypeImplementation c_timeNoTz;

	private static readonly DatatypeImplementation c_timeTz;

	private static readonly DatatypeImplementation c_token;

	private static readonly DatatypeImplementation c_unsignedByte;

	private static readonly DatatypeImplementation c_unsignedInt;

	private static readonly DatatypeImplementation c_unsignedLong;

	private static readonly DatatypeImplementation c_unsignedShort;

	private static readonly DatatypeImplementation c_uuid;

	private static readonly DatatypeImplementation c_year;

	private static readonly DatatypeImplementation c_yearMonth;

	internal static readonly DatatypeImplementation c_normalizedStringV1Compat;

	internal static readonly DatatypeImplementation c_tokenV1Compat;

	private static readonly DatatypeImplementation c_anyAtomicType;

	private static readonly DatatypeImplementation c_dayTimeDuration;

	private static readonly DatatypeImplementation c_untypedAtomicType;

	private static readonly DatatypeImplementation c_yearMonthDuration;

	private static readonly DatatypeImplementation[] c_tokenizedTypes;

	private static readonly DatatypeImplementation[] c_tokenizedTypesXsd;

	private static readonly SchemaDatatypeMap[] c_XdrTypes;

	private static readonly SchemaDatatypeMap[] c_XsdTypes;

	internal static XmlSchemaSimpleType AnySimpleType => anySimpleType;

	internal static XmlSchemaSimpleType AnyAtomicType => anyAtomicType;

	internal static XmlSchemaSimpleType UntypedAtomicType => untypedAtomicType;

	internal static XmlSchemaSimpleType YearMonthDurationType => yearMonthDurationType;

	internal static XmlSchemaSimpleType DayTimeDurationType => dayTimeDurationType;

	internal override FacetsChecker FacetsChecker => miscFacetsChecker;

	internal override XmlValueConverter ValueConverter
	{
		get
		{
			if (valueConverter == null)
			{
				valueConverter = CreateValueConverter(parentSchemaType);
			}
			return valueConverter;
		}
	}

	public override XmlTokenizedType TokenizedType => XmlTokenizedType.None;

	public override Type ValueType => typeof(string);

	public override XmlSchemaDatatypeVariety Variety => variety;

	public override XmlTypeCode TypeCode => XmlTypeCode.None;

	internal override RestrictionFacets Restriction
	{
		get
		{
			return restriction;
		}
		set
		{
			restriction = value;
		}
	}

	internal override bool HasLexicalFacets
	{
		get
		{
			RestrictionFlags restrictionFlags = ((restriction != null) ? restriction.Flags : ((RestrictionFlags)0));
			if (restrictionFlags != 0 && (restrictionFlags & (RestrictionFlags.Pattern | RestrictionFlags.WhiteSpace | RestrictionFlags.TotalDigits | RestrictionFlags.FractionDigits)) != 0)
			{
				return true;
			}
			return false;
		}
	}

	internal override bool HasValueFacets
	{
		get
		{
			RestrictionFlags restrictionFlags = ((restriction != null) ? restriction.Flags : ((RestrictionFlags)0));
			if (restrictionFlags != 0 && (restrictionFlags & (RestrictionFlags.Length | RestrictionFlags.MinLength | RestrictionFlags.MaxLength | RestrictionFlags.Enumeration | RestrictionFlags.MaxInclusive | RestrictionFlags.MaxExclusive | RestrictionFlags.MinInclusive | RestrictionFlags.MinExclusive | RestrictionFlags.TotalDigits | RestrictionFlags.FractionDigits)) != 0)
			{
				return true;
			}
			return false;
		}
	}

	protected DatatypeImplementation Base => baseType;

	internal abstract Type ListValueType { get; }

	internal abstract RestrictionFlags ValidRestrictionFlags { get; }

	internal override XmlSchemaWhiteSpace BuiltInWhitespaceFacet => XmlSchemaWhiteSpace.Preserve;

	static DatatypeImplementation()
	{
		builtinTypes = new Hashtable();
		enumToTypeCode = new XmlSchemaSimpleType[55];
		QnAnySimpleType = new XmlQualifiedName("anySimpleType", "http://www.w3.org/2001/XMLSchema");
		QnAnyType = new XmlQualifiedName("anyType", "http://www.w3.org/2001/XMLSchema");
		stringFacetsChecker = new StringFacetsChecker();
		miscFacetsChecker = new MiscFacetsChecker();
		numeric2FacetsChecker = new Numeric2FacetsChecker();
		binaryFacetsChecker = new BinaryFacetsChecker();
		dateTimeFacetsChecker = new DateTimeFacetsChecker();
		durationFacetsChecker = new DurationFacetsChecker();
		listFacetsChecker = new ListFacetsChecker();
		qnameFacetsChecker = new QNameFacetsChecker();
		unionFacetsChecker = new UnionFacetsChecker();
		c_anySimpleType = new Datatype_anySimpleType();
		c_anyURI = new Datatype_anyURI();
		c_base64Binary = new Datatype_base64Binary();
		c_boolean = new Datatype_boolean();
		c_byte = new Datatype_byte();
		c_char = new Datatype_char();
		c_date = new Datatype_date();
		c_dateTime = new Datatype_dateTime();
		c_dateTimeNoTz = new Datatype_dateTimeNoTimeZone();
		c_dateTimeTz = new Datatype_dateTimeTimeZone();
		c_day = new Datatype_day();
		c_decimal = new Datatype_decimal();
		c_double = new Datatype_double();
		c_doubleXdr = new Datatype_doubleXdr();
		c_duration = new Datatype_duration();
		c_ENTITY = new Datatype_ENTITY();
		c_ENTITIES = (DatatypeImplementation)c_ENTITY.DeriveByList(1, null);
		c_ENUMERATION = new Datatype_ENUMERATION();
		c_fixed = new Datatype_fixed();
		c_float = new Datatype_float();
		c_floatXdr = new Datatype_floatXdr();
		c_hexBinary = new Datatype_hexBinary();
		c_ID = new Datatype_ID();
		c_IDREF = new Datatype_IDREF();
		c_IDREFS = (DatatypeImplementation)c_IDREF.DeriveByList(1, null);
		c_int = new Datatype_int();
		c_integer = new Datatype_integer();
		c_language = new Datatype_language();
		c_long = new Datatype_long();
		c_month = new Datatype_month();
		c_monthDay = new Datatype_monthDay();
		c_Name = new Datatype_Name();
		c_NCName = new Datatype_NCName();
		c_negativeInteger = new Datatype_negativeInteger();
		c_NMTOKEN = new Datatype_NMTOKEN();
		c_NMTOKENS = (DatatypeImplementation)c_NMTOKEN.DeriveByList(1, null);
		c_nonNegativeInteger = new Datatype_nonNegativeInteger();
		c_nonPositiveInteger = new Datatype_nonPositiveInteger();
		c_normalizedString = new Datatype_normalizedString();
		c_NOTATION = new Datatype_NOTATION();
		c_positiveInteger = new Datatype_positiveInteger();
		c_QName = new Datatype_QName();
		c_QNameXdr = new Datatype_QNameXdr();
		c_short = new Datatype_short();
		c_string = new Datatype_string();
		c_time = new Datatype_time();
		c_timeNoTz = new Datatype_timeNoTimeZone();
		c_timeTz = new Datatype_timeTimeZone();
		c_token = new Datatype_token();
		c_unsignedByte = new Datatype_unsignedByte();
		c_unsignedInt = new Datatype_unsignedInt();
		c_unsignedLong = new Datatype_unsignedLong();
		c_unsignedShort = new Datatype_unsignedShort();
		c_uuid = new Datatype_uuid();
		c_year = new Datatype_year();
		c_yearMonth = new Datatype_yearMonth();
		c_normalizedStringV1Compat = new Datatype_normalizedStringV1Compat();
		c_tokenV1Compat = new Datatype_tokenV1Compat();
		c_anyAtomicType = new Datatype_anyAtomicType();
		c_dayTimeDuration = new Datatype_dayTimeDuration();
		c_untypedAtomicType = new Datatype_untypedAtomicType();
		c_yearMonthDuration = new Datatype_yearMonthDuration();
		c_tokenizedTypes = new DatatypeImplementation[13]
		{
			c_string, c_ID, c_IDREF, c_IDREFS, c_ENTITY, c_ENTITIES, c_NMTOKEN, c_NMTOKENS, c_NOTATION, c_ENUMERATION,
			c_QNameXdr, c_NCName, null
		};
		c_tokenizedTypesXsd = new DatatypeImplementation[13]
		{
			c_string, c_ID, c_IDREF, c_IDREFS, c_ENTITY, c_ENTITIES, c_NMTOKEN, c_NMTOKENS, c_NOTATION, c_ENUMERATION,
			c_QName, c_NCName, null
		};
		c_XdrTypes = new SchemaDatatypeMap[38]
		{
			new SchemaDatatypeMap("bin.base64", c_base64Binary),
			new SchemaDatatypeMap("bin.hex", c_hexBinary),
			new SchemaDatatypeMap("boolean", c_boolean),
			new SchemaDatatypeMap("char", c_char),
			new SchemaDatatypeMap("date", c_date),
			new SchemaDatatypeMap("dateTime", c_dateTimeNoTz),
			new SchemaDatatypeMap("dateTime.tz", c_dateTimeTz),
			new SchemaDatatypeMap("decimal", c_decimal),
			new SchemaDatatypeMap("entities", c_ENTITIES),
			new SchemaDatatypeMap("entity", c_ENTITY),
			new SchemaDatatypeMap("enumeration", c_ENUMERATION),
			new SchemaDatatypeMap("fixed.14.4", c_fixed),
			new SchemaDatatypeMap("float", c_doubleXdr),
			new SchemaDatatypeMap("float.ieee.754.32", c_floatXdr),
			new SchemaDatatypeMap("float.ieee.754.64", c_doubleXdr),
			new SchemaDatatypeMap("i1", c_byte),
			new SchemaDatatypeMap("i2", c_short),
			new SchemaDatatypeMap("i4", c_int),
			new SchemaDatatypeMap("i8", c_long),
			new SchemaDatatypeMap("id", c_ID),
			new SchemaDatatypeMap("idref", c_IDREF),
			new SchemaDatatypeMap("idrefs", c_IDREFS),
			new SchemaDatatypeMap("int", c_int),
			new SchemaDatatypeMap("nmtoken", c_NMTOKEN),
			new SchemaDatatypeMap("nmtokens", c_NMTOKENS),
			new SchemaDatatypeMap("notation", c_NOTATION),
			new SchemaDatatypeMap("number", c_doubleXdr),
			new SchemaDatatypeMap("r4", c_floatXdr),
			new SchemaDatatypeMap("r8", c_doubleXdr),
			new SchemaDatatypeMap("string", c_string),
			new SchemaDatatypeMap("time", c_timeNoTz),
			new SchemaDatatypeMap("time.tz", c_timeTz),
			new SchemaDatatypeMap("ui1", c_unsignedByte),
			new SchemaDatatypeMap("ui2", c_unsignedShort),
			new SchemaDatatypeMap("ui4", c_unsignedInt),
			new SchemaDatatypeMap("ui8", c_unsignedLong),
			new SchemaDatatypeMap("uri", c_anyURI),
			new SchemaDatatypeMap("uuid", c_uuid)
		};
		c_XsdTypes = new SchemaDatatypeMap[45]
		{
			new SchemaDatatypeMap("ENTITIES", c_ENTITIES, 11),
			new SchemaDatatypeMap("ENTITY", c_ENTITY, 11),
			new SchemaDatatypeMap("ID", c_ID, 5),
			new SchemaDatatypeMap("IDREF", c_IDREF, 5),
			new SchemaDatatypeMap("IDREFS", c_IDREFS, 11),
			new SchemaDatatypeMap("NCName", c_NCName, 9),
			new SchemaDatatypeMap("NMTOKEN", c_NMTOKEN, 40),
			new SchemaDatatypeMap("NMTOKENS", c_NMTOKENS, 11),
			new SchemaDatatypeMap("NOTATION", c_NOTATION, 11),
			new SchemaDatatypeMap("Name", c_Name, 40),
			new SchemaDatatypeMap("QName", c_QName, 11),
			new SchemaDatatypeMap("anySimpleType", c_anySimpleType, -1),
			new SchemaDatatypeMap("anyURI", c_anyURI, 11),
			new SchemaDatatypeMap("base64Binary", c_base64Binary, 11),
			new SchemaDatatypeMap("boolean", c_boolean, 11),
			new SchemaDatatypeMap("byte", c_byte, 37),
			new SchemaDatatypeMap("date", c_date, 11),
			new SchemaDatatypeMap("dateTime", c_dateTime, 11),
			new SchemaDatatypeMap("decimal", c_decimal, 11),
			new SchemaDatatypeMap("double", c_double, 11),
			new SchemaDatatypeMap("duration", c_duration, 11),
			new SchemaDatatypeMap("float", c_float, 11),
			new SchemaDatatypeMap("gDay", c_day, 11),
			new SchemaDatatypeMap("gMonth", c_month, 11),
			new SchemaDatatypeMap("gMonthDay", c_monthDay, 11),
			new SchemaDatatypeMap("gYear", c_year, 11),
			new SchemaDatatypeMap("gYearMonth", c_yearMonth, 11),
			new SchemaDatatypeMap("hexBinary", c_hexBinary, 11),
			new SchemaDatatypeMap("int", c_int, 31),
			new SchemaDatatypeMap("integer", c_integer, 18),
			new SchemaDatatypeMap("language", c_language, 40),
			new SchemaDatatypeMap("long", c_long, 29),
			new SchemaDatatypeMap("negativeInteger", c_negativeInteger, 34),
			new SchemaDatatypeMap("nonNegativeInteger", c_nonNegativeInteger, 29),
			new SchemaDatatypeMap("nonPositiveInteger", c_nonPositiveInteger, 29),
			new SchemaDatatypeMap("normalizedString", c_normalizedString, 38),
			new SchemaDatatypeMap("positiveInteger", c_positiveInteger, 33),
			new SchemaDatatypeMap("short", c_short, 28),
			new SchemaDatatypeMap("string", c_string, 11),
			new SchemaDatatypeMap("time", c_time, 11),
			new SchemaDatatypeMap("token", c_token, 35),
			new SchemaDatatypeMap("unsignedByte", c_unsignedByte, 44),
			new SchemaDatatypeMap("unsignedInt", c_unsignedInt, 43),
			new SchemaDatatypeMap("unsignedLong", c_unsignedLong, 33),
			new SchemaDatatypeMap("unsignedShort", c_unsignedShort, 42)
		};
		CreateBuiltinTypes();
	}

	internal new static DatatypeImplementation FromXmlTokenizedType(XmlTokenizedType token)
	{
		return c_tokenizedTypes[(int)token];
	}

	internal new static DatatypeImplementation FromXmlTokenizedTypeXsd(XmlTokenizedType token)
	{
		return c_tokenizedTypesXsd[(int)token];
	}

	internal new static DatatypeImplementation FromXdrName(string name)
	{
		int num = Array.BinarySearch(c_XdrTypes, name, null);
		if (num >= 0)
		{
			return (DatatypeImplementation)c_XdrTypes[num];
		}
		return null;
	}

	private static DatatypeImplementation FromTypeName(string name)
	{
		int num = Array.BinarySearch(c_XsdTypes, name, null);
		if (num >= 0)
		{
			return (DatatypeImplementation)c_XsdTypes[num];
		}
		return null;
	}

	internal static XmlSchemaSimpleType StartBuiltinType(XmlQualifiedName qname, XmlSchemaDatatype dataType)
	{
		XmlSchemaSimpleType xmlSchemaSimpleType = new XmlSchemaSimpleType();
		xmlSchemaSimpleType.SetQualifiedName(qname);
		xmlSchemaSimpleType.SetDatatype(dataType);
		xmlSchemaSimpleType.ElementDecl = new SchemaElementDecl(dataType);
		xmlSchemaSimpleType.ElementDecl.SchemaType = xmlSchemaSimpleType;
		return xmlSchemaSimpleType;
	}

	internal static void FinishBuiltinType(XmlSchemaSimpleType derivedType, XmlSchemaSimpleType baseType)
	{
		derivedType.SetBaseSchemaType(baseType);
		derivedType.SetDerivedBy(XmlSchemaDerivationMethod.Restriction);
		if (derivedType.Datatype.Variety == XmlSchemaDatatypeVariety.Atomic)
		{
			XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = new XmlSchemaSimpleTypeRestriction();
			xmlSchemaSimpleTypeRestriction.BaseTypeName = baseType.QualifiedName;
			derivedType.Content = xmlSchemaSimpleTypeRestriction;
		}
		if (derivedType.Datatype.Variety == XmlSchemaDatatypeVariety.List)
		{
			XmlSchemaSimpleTypeList xmlSchemaSimpleTypeList = new XmlSchemaSimpleTypeList();
			derivedType.SetDerivedBy(XmlSchemaDerivationMethod.List);
			switch (derivedType.Datatype.TypeCode)
			{
			case XmlTypeCode.NmToken:
			{
				XmlSchemaSimpleType itemType = (xmlSchemaSimpleTypeList.BaseItemType = enumToTypeCode[34]);
				xmlSchemaSimpleTypeList.ItemType = itemType;
				break;
			}
			case XmlTypeCode.Entity:
			{
				XmlSchemaSimpleType itemType = (xmlSchemaSimpleTypeList.BaseItemType = enumToTypeCode[39]);
				xmlSchemaSimpleTypeList.ItemType = itemType;
				break;
			}
			case XmlTypeCode.Idref:
			{
				XmlSchemaSimpleType itemType = (xmlSchemaSimpleTypeList.BaseItemType = enumToTypeCode[38]);
				xmlSchemaSimpleTypeList.ItemType = itemType;
				break;
			}
			}
			derivedType.Content = xmlSchemaSimpleTypeList;
		}
	}

	internal static void CreateBuiltinTypes()
	{
		SchemaDatatypeMap schemaDatatypeMap = c_XsdTypes[11];
		XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(schemaDatatypeMap.Name, "http://www.w3.org/2001/XMLSchema");
		DatatypeImplementation datatypeImplementation = FromTypeName(xmlQualifiedName.Name);
		anySimpleType = StartBuiltinType(xmlQualifiedName, datatypeImplementation);
		datatypeImplementation.parentSchemaType = anySimpleType;
		builtinTypes.Add(xmlQualifiedName, anySimpleType);
		for (int i = 0; i < c_XsdTypes.Length; i++)
		{
			if (i != 11)
			{
				schemaDatatypeMap = c_XsdTypes[i];
				xmlQualifiedName = new XmlQualifiedName(schemaDatatypeMap.Name, "http://www.w3.org/2001/XMLSchema");
				datatypeImplementation = FromTypeName(xmlQualifiedName.Name);
				XmlSchemaSimpleType xmlSchemaSimpleType = (XmlSchemaSimpleType)(datatypeImplementation.parentSchemaType = StartBuiltinType(xmlQualifiedName, datatypeImplementation));
				builtinTypes.Add(xmlQualifiedName, xmlSchemaSimpleType);
				if (datatypeImplementation.variety == XmlSchemaDatatypeVariety.Atomic)
				{
					enumToTypeCode[(int)datatypeImplementation.TypeCode] = xmlSchemaSimpleType;
				}
			}
		}
		for (int j = 0; j < c_XsdTypes.Length; j++)
		{
			if (j != 11)
			{
				schemaDatatypeMap = c_XsdTypes[j];
				XmlSchemaSimpleType derivedType = (XmlSchemaSimpleType)builtinTypes[new XmlQualifiedName(schemaDatatypeMap.Name, "http://www.w3.org/2001/XMLSchema")];
				if (schemaDatatypeMap.ParentIndex == 11)
				{
					FinishBuiltinType(derivedType, anySimpleType);
					continue;
				}
				XmlSchemaSimpleType xmlSchemaSimpleType2 = (XmlSchemaSimpleType)builtinTypes[new XmlQualifiedName(c_XsdTypes[schemaDatatypeMap.ParentIndex].Name, "http://www.w3.org/2001/XMLSchema")];
				FinishBuiltinType(derivedType, xmlSchemaSimpleType2);
			}
		}
		xmlQualifiedName = new XmlQualifiedName("anyAtomicType", "http://www.w3.org/2003/11/xpath-datatypes");
		anyAtomicType = StartBuiltinType(xmlQualifiedName, c_anyAtomicType);
		c_anyAtomicType.parentSchemaType = anyAtomicType;
		FinishBuiltinType(anyAtomicType, anySimpleType);
		builtinTypes.Add(xmlQualifiedName, anyAtomicType);
		enumToTypeCode[10] = anyAtomicType;
		xmlQualifiedName = new XmlQualifiedName("untypedAtomic", "http://www.w3.org/2003/11/xpath-datatypes");
		untypedAtomicType = StartBuiltinType(xmlQualifiedName, c_untypedAtomicType);
		c_untypedAtomicType.parentSchemaType = untypedAtomicType;
		FinishBuiltinType(untypedAtomicType, anyAtomicType);
		builtinTypes.Add(xmlQualifiedName, untypedAtomicType);
		enumToTypeCode[11] = untypedAtomicType;
		xmlQualifiedName = new XmlQualifiedName("yearMonthDuration", "http://www.w3.org/2003/11/xpath-datatypes");
		yearMonthDurationType = StartBuiltinType(xmlQualifiedName, c_yearMonthDuration);
		c_yearMonthDuration.parentSchemaType = yearMonthDurationType;
		FinishBuiltinType(yearMonthDurationType, enumToTypeCode[17]);
		builtinTypes.Add(xmlQualifiedName, yearMonthDurationType);
		enumToTypeCode[53] = yearMonthDurationType;
		xmlQualifiedName = new XmlQualifiedName("dayTimeDuration", "http://www.w3.org/2003/11/xpath-datatypes");
		dayTimeDurationType = StartBuiltinType(xmlQualifiedName, c_dayTimeDuration);
		c_dayTimeDuration.parentSchemaType = dayTimeDurationType;
		FinishBuiltinType(dayTimeDurationType, enumToTypeCode[17]);
		builtinTypes.Add(xmlQualifiedName, dayTimeDurationType);
		enumToTypeCode[54] = dayTimeDurationType;
	}

	internal static XmlSchemaSimpleType GetSimpleTypeFromTypeCode(XmlTypeCode typeCode)
	{
		return enumToTypeCode[(int)typeCode];
	}

	internal static XmlSchemaSimpleType GetSimpleTypeFromXsdType(XmlQualifiedName qname)
	{
		return (XmlSchemaSimpleType)builtinTypes[qname];
	}

	internal static XmlSchemaSimpleType GetNormalizedStringTypeV1Compat()
	{
		if (normalizedStringTypeV1Compat == null)
		{
			XmlSchemaSimpleType xmlSchemaSimpleType = GetSimpleTypeFromTypeCode(XmlTypeCode.NormalizedString).Clone() as XmlSchemaSimpleType;
			xmlSchemaSimpleType.SetDatatype(c_normalizedStringV1Compat);
			xmlSchemaSimpleType.ElementDecl = new SchemaElementDecl(c_normalizedStringV1Compat);
			xmlSchemaSimpleType.ElementDecl.SchemaType = xmlSchemaSimpleType;
			normalizedStringTypeV1Compat = xmlSchemaSimpleType;
		}
		return normalizedStringTypeV1Compat;
	}

	internal static XmlSchemaSimpleType GetTokenTypeV1Compat()
	{
		if (tokenTypeV1Compat == null)
		{
			XmlSchemaSimpleType xmlSchemaSimpleType = GetSimpleTypeFromTypeCode(XmlTypeCode.Token).Clone() as XmlSchemaSimpleType;
			xmlSchemaSimpleType.SetDatatype(c_tokenV1Compat);
			xmlSchemaSimpleType.ElementDecl = new SchemaElementDecl(c_tokenV1Compat);
			xmlSchemaSimpleType.ElementDecl.SchemaType = xmlSchemaSimpleType;
			tokenTypeV1Compat = xmlSchemaSimpleType;
		}
		return tokenTypeV1Compat;
	}

	internal static XmlSchemaSimpleType[] GetBuiltInTypes()
	{
		return enumToTypeCode;
	}

	internal static XmlTypeCode GetPrimitiveTypeCode(XmlTypeCode typeCode)
	{
		XmlSchemaSimpleType xmlSchemaSimpleType = enumToTypeCode[(int)typeCode];
		while (xmlSchemaSimpleType.BaseXmlSchemaType != AnySimpleType)
		{
			xmlSchemaSimpleType = xmlSchemaSimpleType.BaseXmlSchemaType as XmlSchemaSimpleType;
		}
		return xmlSchemaSimpleType.TypeCode;
	}

	internal override XmlSchemaDatatype DeriveByRestriction(XmlSchemaObjectCollection facets, XmlNameTable nameTable, XmlSchemaType schemaType)
	{
		DatatypeImplementation obj = (DatatypeImplementation)MemberwiseClone();
		obj.restriction = FacetsChecker.ConstructRestriction(this, facets, nameTable);
		obj.baseType = this;
		obj.parentSchemaType = schemaType;
		obj.valueConverter = null;
		return obj;
	}

	internal override XmlSchemaDatatype DeriveByList(XmlSchemaType schemaType)
	{
		return DeriveByList(0, schemaType);
	}

	internal XmlSchemaDatatype DeriveByList(int minSize, XmlSchemaType schemaType)
	{
		if (variety == XmlSchemaDatatypeVariety.List)
		{
			throw new XmlSchemaException("A list data type must be derived from an atomic or union data type.", string.Empty);
		}
		if (variety == XmlSchemaDatatypeVariety.Union && !((Datatype_union)this).HasAtomicMembers())
		{
			throw new XmlSchemaException("A list data type must be derived from an atomic or union data type.", string.Empty);
		}
		return new Datatype_List(this, minSize)
		{
			variety = XmlSchemaDatatypeVariety.List,
			restriction = null,
			baseType = c_anySimpleType,
			parentSchemaType = schemaType
		};
	}

	internal new static DatatypeImplementation DeriveByUnion(XmlSchemaSimpleType[] types, XmlSchemaType schemaType)
	{
		return new Datatype_union(types)
		{
			baseType = c_anySimpleType,
			variety = XmlSchemaDatatypeVariety.Union,
			parentSchemaType = schemaType
		};
	}

	internal override void VerifySchemaValid(XmlSchemaObjectTable notations, XmlSchemaObject caller)
	{
	}

	public override bool IsDerivedFrom(XmlSchemaDatatype datatype)
	{
		if (datatype == null)
		{
			return false;
		}
		for (DatatypeImplementation datatypeImplementation = this; datatypeImplementation != null; datatypeImplementation = datatypeImplementation.baseType)
		{
			if (datatypeImplementation == datatype)
			{
				return true;
			}
		}
		if (((DatatypeImplementation)datatype).baseType == null)
		{
			Type type = GetType();
			Type type2 = datatype.GetType();
			if (!(type2 == type))
			{
				return type.IsSubclassOf(type2);
			}
			return true;
		}
		if (datatype.Variety == XmlSchemaDatatypeVariety.Union && !datatype.HasLexicalFacets && !datatype.HasValueFacets && variety != XmlSchemaDatatypeVariety.Union)
		{
			return ((Datatype_union)datatype).IsUnionBaseOf(this);
		}
		if ((variety == XmlSchemaDatatypeVariety.Union || variety == XmlSchemaDatatypeVariety.List) && restriction == null)
		{
			return datatype == anySimpleType.Datatype;
		}
		return false;
	}

	internal override bool IsEqual(object o1, object o2)
	{
		return Compare(o1, o2) == 0;
	}

	internal override bool IsComparable(XmlSchemaDatatype dtype)
	{
		XmlTypeCode typeCode = TypeCode;
		XmlTypeCode typeCode2 = dtype.TypeCode;
		if (typeCode == typeCode2)
		{
			return true;
		}
		if (GetPrimitiveTypeCode(typeCode) == GetPrimitiveTypeCode(typeCode2))
		{
			return true;
		}
		if (IsDerivedFrom(dtype) || dtype.IsDerivedFrom(this))
		{
			return true;
		}
		return false;
	}

	internal virtual XmlValueConverter CreateValueConverter(XmlSchemaType schemaType)
	{
		return null;
	}

	internal override object ParseValue(string s, Type typDest, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
	{
		return ValueConverter.ChangeType(ParseValue(s, nameTable, nsmgr), typDest, nsmgr);
	}

	public override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
	{
		object typedValue;
		Exception ex = TryParseValue(s, nameTable, nsmgr, out typedValue);
		if (ex != null)
		{
			throw new XmlSchemaException("The value '{0}' is invalid according to its schema type '{1}' - {2}", new string[3]
			{
				s,
				GetTypeName(),
				ex.Message
			}, ex, null, 0, 0, null);
		}
		if (Variety == XmlSchemaDatatypeVariety.Union)
		{
			return (typedValue as XsdSimpleValue).TypedValue;
		}
		return typedValue;
	}

	internal override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, bool createAtomicValue)
	{
		if (createAtomicValue)
		{
			object typedValue;
			Exception ex = TryParseValue(s, nameTable, nsmgr, out typedValue);
			if (ex != null)
			{
				throw new XmlSchemaException("The value '{0}' is invalid according to its schema type '{1}' - {2}", new string[3]
				{
					s,
					GetTypeName(),
					ex.Message
				}, ex, null, 0, 0, null);
			}
			return typedValue;
		}
		return ParseValue(s, nameTable, nsmgr);
	}

	internal override Exception TryParseValue(object value, XmlNameTable nameTable, IXmlNamespaceResolver namespaceResolver, out object typedValue)
	{
		Exception ex = null;
		typedValue = null;
		if (value == null)
		{
			return new ArgumentNullException("value");
		}
		if (value is string s)
		{
			return TryParseValue(s, nameTable, namespaceResolver, out typedValue);
		}
		try
		{
			object obj = value;
			if (value.GetType() != ValueType)
			{
				obj = ValueConverter.ChangeType(value, ValueType, namespaceResolver);
			}
			if (!HasLexicalFacets)
			{
				goto IL_008d;
			}
			string parseString = (string)ValueConverter.ChangeType(value, typeof(string), namespaceResolver);
			ex = FacetsChecker.CheckLexicalFacets(ref parseString, this);
			if (ex == null)
			{
				goto IL_008d;
			}
			goto end_IL_002b;
			IL_008d:
			if (!HasValueFacets)
			{
				goto IL_00a8;
			}
			ex = FacetsChecker.CheckValueFacets(obj, this);
			if (ex == null)
			{
				goto IL_00a8;
			}
			goto end_IL_002b;
			IL_00a8:
			typedValue = obj;
			return null;
			end_IL_002b:;
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

	internal string GetTypeName()
	{
		XmlSchemaType xmlSchemaType = parentSchemaType;
		if (xmlSchemaType == null || xmlSchemaType.QualifiedName.IsEmpty)
		{
			return base.TypeCodeString;
		}
		return xmlSchemaType.QualifiedName.ToString();
	}

	protected int Compare(byte[] value1, byte[] value2)
	{
		int num = value1.Length;
		if (num != value2.Length)
		{
			return -1;
		}
		for (int i = 0; i < num; i++)
		{
			if (value1[i] != value2[i])
			{
				return -1;
			}
		}
		return 0;
	}
}
