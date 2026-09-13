using System.Xml;

namespace System.Runtime.Serialization;

internal static class DictionaryGlobals
{
	public static readonly XmlDictionaryString EmptyString;

	public static readonly XmlDictionaryString SchemaInstanceNamespace;

	public static readonly XmlDictionaryString SchemaNamespace;

	public static readonly XmlDictionaryString SerializationNamespace;

	public static readonly XmlDictionaryString XmlnsNamespace;

	public static readonly XmlDictionaryString XsiTypeLocalName;

	public static readonly XmlDictionaryString XsiNilLocalName;

	public static readonly XmlDictionaryString ClrTypeLocalName;

	public static readonly XmlDictionaryString ClrAssemblyLocalName;

	public static readonly XmlDictionaryString ArraySizeLocalName;

	public static readonly XmlDictionaryString IdLocalName;

	public static readonly XmlDictionaryString RefLocalName;

	public static readonly XmlDictionaryString ISerializableFactoryTypeLocalName;

	public static readonly XmlDictionaryString CharLocalName;

	public static readonly XmlDictionaryString BooleanLocalName;

	public static readonly XmlDictionaryString SignedByteLocalName;

	public static readonly XmlDictionaryString UnsignedByteLocalName;

	public static readonly XmlDictionaryString ShortLocalName;

	public static readonly XmlDictionaryString UnsignedShortLocalName;

	public static readonly XmlDictionaryString IntLocalName;

	public static readonly XmlDictionaryString UnsignedIntLocalName;

	public static readonly XmlDictionaryString LongLocalName;

	public static readonly XmlDictionaryString UnsignedLongLocalName;

	public static readonly XmlDictionaryString FloatLocalName;

	public static readonly XmlDictionaryString DoubleLocalName;

	public static readonly XmlDictionaryString DecimalLocalName;

	public static readonly XmlDictionaryString DateTimeLocalName;

	public static readonly XmlDictionaryString StringLocalName;

	public static readonly XmlDictionaryString ByteArrayLocalName;

	public static readonly XmlDictionaryString ObjectLocalName;

	public static readonly XmlDictionaryString TimeSpanLocalName;

	public static readonly XmlDictionaryString GuidLocalName;

	public static readonly XmlDictionaryString UriLocalName;

	public static readonly XmlDictionaryString QNameLocalName;

	public static readonly XmlDictionaryString Space;

	public static readonly XmlDictionaryString timeLocalName;

	public static readonly XmlDictionaryString dateLocalName;

	public static readonly XmlDictionaryString hexBinaryLocalName;

	public static readonly XmlDictionaryString gYearMonthLocalName;

	public static readonly XmlDictionaryString gYearLocalName;

	public static readonly XmlDictionaryString gMonthDayLocalName;

	public static readonly XmlDictionaryString gDayLocalName;

	public static readonly XmlDictionaryString gMonthLocalName;

	public static readonly XmlDictionaryString integerLocalName;

	public static readonly XmlDictionaryString positiveIntegerLocalName;

	public static readonly XmlDictionaryString negativeIntegerLocalName;

	public static readonly XmlDictionaryString nonPositiveIntegerLocalName;

	public static readonly XmlDictionaryString nonNegativeIntegerLocalName;

	public static readonly XmlDictionaryString normalizedStringLocalName;

	public static readonly XmlDictionaryString tokenLocalName;

	public static readonly XmlDictionaryString languageLocalName;

	public static readonly XmlDictionaryString NameLocalName;

	public static readonly XmlDictionaryString NCNameLocalName;

	public static readonly XmlDictionaryString XSDIDLocalName;

	public static readonly XmlDictionaryString IDREFLocalName;

	public static readonly XmlDictionaryString IDREFSLocalName;

	public static readonly XmlDictionaryString ENTITYLocalName;

	public static readonly XmlDictionaryString ENTITIESLocalName;

	public static readonly XmlDictionaryString NMTOKENLocalName;

	public static readonly XmlDictionaryString NMTOKENSLocalName;

	public static readonly XmlDictionaryString AsmxTypesNamespace;

	static DictionaryGlobals()
	{
		XmlDictionary xmlDictionary = new XmlDictionary(61);
		try
		{
			SchemaInstanceNamespace = xmlDictionary.Add("http://www.w3.org/2001/XMLSchema-instance");
			SerializationNamespace = xmlDictionary.Add("http://schemas.microsoft.com/2003/10/Serialization/");
			SchemaNamespace = xmlDictionary.Add("http://www.w3.org/2001/XMLSchema");
			XsiTypeLocalName = xmlDictionary.Add("type");
			XsiNilLocalName = xmlDictionary.Add("nil");
			IdLocalName = xmlDictionary.Add("Id");
			RefLocalName = xmlDictionary.Add("Ref");
			ArraySizeLocalName = xmlDictionary.Add("Size");
			EmptyString = xmlDictionary.Add(string.Empty);
			ISerializableFactoryTypeLocalName = xmlDictionary.Add("FactoryType");
			XmlnsNamespace = xmlDictionary.Add("http://www.w3.org/2000/xmlns/");
			CharLocalName = xmlDictionary.Add("char");
			BooleanLocalName = xmlDictionary.Add("boolean");
			SignedByteLocalName = xmlDictionary.Add("byte");
			UnsignedByteLocalName = xmlDictionary.Add("unsignedByte");
			ShortLocalName = xmlDictionary.Add("short");
			UnsignedShortLocalName = xmlDictionary.Add("unsignedShort");
			IntLocalName = xmlDictionary.Add("int");
			UnsignedIntLocalName = xmlDictionary.Add("unsignedInt");
			LongLocalName = xmlDictionary.Add("long");
			UnsignedLongLocalName = xmlDictionary.Add("unsignedLong");
			FloatLocalName = xmlDictionary.Add("float");
			DoubleLocalName = xmlDictionary.Add("double");
			DecimalLocalName = xmlDictionary.Add("decimal");
			DateTimeLocalName = xmlDictionary.Add("dateTime");
			StringLocalName = xmlDictionary.Add("string");
			ByteArrayLocalName = xmlDictionary.Add("base64Binary");
			ObjectLocalName = xmlDictionary.Add("anyType");
			TimeSpanLocalName = xmlDictionary.Add("duration");
			GuidLocalName = xmlDictionary.Add("guid");
			UriLocalName = xmlDictionary.Add("anyURI");
			QNameLocalName = xmlDictionary.Add("QName");
			ClrTypeLocalName = xmlDictionary.Add("Type");
			ClrAssemblyLocalName = xmlDictionary.Add("Assembly");
			Space = xmlDictionary.Add(" ");
			timeLocalName = xmlDictionary.Add("time");
			dateLocalName = xmlDictionary.Add("date");
			hexBinaryLocalName = xmlDictionary.Add("hexBinary");
			gYearMonthLocalName = xmlDictionary.Add("gYearMonth");
			gYearLocalName = xmlDictionary.Add("gYear");
			gMonthDayLocalName = xmlDictionary.Add("gMonthDay");
			gDayLocalName = xmlDictionary.Add("gDay");
			gMonthLocalName = xmlDictionary.Add("gMonth");
			integerLocalName = xmlDictionary.Add("integer");
			positiveIntegerLocalName = xmlDictionary.Add("positiveInteger");
			negativeIntegerLocalName = xmlDictionary.Add("negativeInteger");
			nonPositiveIntegerLocalName = xmlDictionary.Add("nonPositiveInteger");
			nonNegativeIntegerLocalName = xmlDictionary.Add("nonNegativeInteger");
			normalizedStringLocalName = xmlDictionary.Add("normalizedString");
			tokenLocalName = xmlDictionary.Add("token");
			languageLocalName = xmlDictionary.Add("language");
			NameLocalName = xmlDictionary.Add("Name");
			NCNameLocalName = xmlDictionary.Add("NCName");
			XSDIDLocalName = xmlDictionary.Add("ID");
			IDREFLocalName = xmlDictionary.Add("IDREF");
			IDREFSLocalName = xmlDictionary.Add("IDREFS");
			ENTITYLocalName = xmlDictionary.Add("ENTITY");
			ENTITIESLocalName = xmlDictionary.Add("ENTITIES");
			NMTOKENLocalName = xmlDictionary.Add("NMTOKEN");
			NMTOKENSLocalName = xmlDictionary.Add("NMTOKENS");
			AsmxTypesNamespace = xmlDictionary.Add("http://microsoft.com/wsdl/types/");
		}
		catch (Exception ex)
		{
			if (Fx.IsFatal(ex))
			{
				throw;
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperFatal(ex.Message, ex);
		}
	}
}
