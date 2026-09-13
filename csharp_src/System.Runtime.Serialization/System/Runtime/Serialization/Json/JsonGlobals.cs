using System.Xml;

namespace System.Runtime.Serialization.Json;

internal static class JsonGlobals
{
	public static readonly int DataContractXsdBaseNamespaceLength = "http://schemas.datacontract.org/2004/07/".Length;

	public static readonly XmlDictionaryString dDictionaryString = new XmlDictionary().Add("d");

	public static readonly char[] floatingPointCharacters = new char[2] { '.', 'e' };

	public static readonly XmlDictionaryString itemDictionaryString = new XmlDictionary().Add("item");

	public static readonly XmlDictionaryString rootDictionaryString = new XmlDictionary().Add("root");

	public static readonly long unixEpochTicks = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

	public const string applicationJsonMediaType = "application/json";

	public const string arrayString = "array";

	public const string booleanString = "boolean";

	public const string CacheControlString = "Cache-Control";

	public const byte CollectionByte = 91;

	public const char CollectionChar = '[';

	public const string DateTimeEndGuardReader = ")/";

	public const string DateTimeEndGuardWriter = ")\\/";

	public const string DateTimeStartGuardReader = "/Date(";

	public const string DateTimeStartGuardWriter = "\\/Date(";

	public const string dString = "d";

	public const byte EndCollectionByte = 93;

	public const char EndCollectionChar = ']';

	public const byte EndObjectByte = 125;

	public const char EndObjectChar = '}';

	public const string ExpiresString = "Expires";

	public const string IfModifiedSinceString = "If-Modified-Since";

	public const string itemString = "item";

	public const string jsonerrorString = "jsonerror";

	public const string KeyString = "Key";

	public const string LastModifiedString = "Last-Modified";

	public const int maxScopeSize = 25;

	public const byte MemberSeparatorByte = 44;

	public const char MemberSeparatorChar = ',';

	public const byte NameValueSeparatorByte = 58;

	public const char NameValueSeparatorChar = ':';

	public const string NameValueSeparatorString = ":";

	public const string nullString = "null";

	public const string numberString = "number";

	public const byte ObjectByte = 123;

	public const char ObjectChar = '{';

	public const string objectString = "object";

	public const string publicString = "public";

	public const byte QuoteByte = 34;

	public const char QuoteChar = '"';

	public const string rootString = "root";

	public const string serverTypeString = "__type";

	public const string stringString = "string";

	public const string textJsonMediaType = "text/json";

	public const string trueString = "true";

	public const string typeString = "type";

	public const string ValueString = "Value";

	public const char WhitespaceChar = ' ';

	public const string xmlnsPrefix = "xmlns";

	public const string xmlPrefix = "xml";
}
