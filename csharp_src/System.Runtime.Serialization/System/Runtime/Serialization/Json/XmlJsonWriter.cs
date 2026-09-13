using System.Globalization;
using System.IO;
using System.Security;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization.Json;

internal class XmlJsonWriter : XmlDictionaryWriter, IXmlJsonWriterInitializer
{
	private enum JsonDataType
	{
		None,
		Null,
		Boolean,
		Number,
		String,
		Object,
		Array
	}

	[Flags]
	private enum NameState
	{
		None = 0,
		IsWritingNameWithMapping = 1,
		IsWritingNameAttribute = 2,
		WrittenNameWithMapping = 4
	}

	private class JsonNodeWriter : XmlUTF8NodeWriter
	{
		[SecurityCritical]
		internal unsafe void WriteChars(char* chars, int charCount)
		{
			UnsafeWriteUTF8Chars(chars, charCount);
		}
	}

	private const char BACK_SLASH = '\\';

	private const char FORWARD_SLASH = '/';

	private const char HIGH_SURROGATE_START = '\ud800';

	private const char LOW_SURROGATE_END = '\udfff';

	private const char MAX_CHAR = '\ufffe';

	private const char WHITESPACE = ' ';

	private const char CARRIAGE_RETURN = '\r';

	private const char NEWLINE = '\n';

	private const char BACKSPACE = '\b';

	private const char FORM_FEED = '\f';

	private const char HORIZONTAL_TABULATION = '\t';

	private const string xmlNamespace = "http://www.w3.org/XML/1998/namespace";

	private const string xmlnsNamespace = "http://www.w3.org/2000/xmlns/";

	[SecurityCritical]
	private static BinHexEncoding binHexEncoding;

	private static char[] CharacterAbbrevs;

	private string attributeText;

	private JsonDataType dataType;

	private int depth;

	private bool endElementBuffer;

	private bool isWritingDataTypeAttribute;

	private bool isWritingServerTypeAttribute;

	private bool isWritingXmlnsAttribute;

	private bool isWritingXmlnsAttributeDefaultNs;

	private NameState nameState;

	private JsonNodeType nodeType;

	private JsonNodeWriter nodeWriter;

	private JsonNodeType[] scopes;

	private string serverTypeValue;

	private WriteState writeState;

	private bool wroteServerTypeAttribute;

	private bool indent;

	private string indentChars;

	private int indentLevel;

	public override XmlWriterSettings Settings => null;

	public override WriteState WriteState
	{
		get
		{
			if (writeState == WriteState.Closed)
			{
				return WriteState.Closed;
			}
			if (HasOpenAttribute)
			{
				return WriteState.Attribute;
			}
			switch (nodeType)
			{
			case JsonNodeType.None:
				return WriteState.Start;
			case JsonNodeType.Element:
				return WriteState.Element;
			case JsonNodeType.EndElement:
			case JsonNodeType.QuotedText:
			case JsonNodeType.StandaloneText:
				return WriteState.Content;
			default:
				return WriteState.Error;
			}
		}
	}

	public override string XmlLang => null;

	public override XmlSpace XmlSpace => XmlSpace.None;

	private static BinHexEncoding BinHexEncoding
	{
		[SecuritySafeCritical]
		get
		{
			if (binHexEncoding == null)
			{
				binHexEncoding = new BinHexEncoding();
			}
			return binHexEncoding;
		}
	}

	private bool HasOpenAttribute
	{
		get
		{
			if (!isWritingDataTypeAttribute && !isWritingServerTypeAttribute && !IsWritingNameAttribute)
			{
				return isWritingXmlnsAttribute;
			}
			return true;
		}
	}

	private bool IsClosed => WriteState == WriteState.Closed;

	private bool IsWritingCollection
	{
		get
		{
			if (depth > 0)
			{
				return scopes[depth] == JsonNodeType.Collection;
			}
			return false;
		}
	}

	private bool IsWritingNameAttribute => (nameState & NameState.IsWritingNameAttribute) == NameState.IsWritingNameAttribute;

	private bool IsWritingNameWithMapping => (nameState & NameState.IsWritingNameWithMapping) == NameState.IsWritingNameWithMapping;

	private bool WrittenNameWithMapping => (nameState & NameState.WrittenNameWithMapping) == NameState.WrittenNameWithMapping;

	public XmlJsonWriter()
		: this(indent: false, null)
	{
	}

	public XmlJsonWriter(bool indent, string indentChars)
	{
		this.indent = indent;
		if (indent)
		{
			if (indentChars == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("indentChars");
			}
			this.indentChars = indentChars;
		}
		InitializeWriter();
		if (CharacterAbbrevs == null)
		{
			CharacterAbbrevs = GetCharacterAbbrevs();
		}
	}

	private static char[] GetCharacterAbbrevs()
	{
		char[] array = new char[32];
		for (int i = 0; i < 32; i++)
		{
			if (!System.LocalAppContextSwitches.DoNotUseEcmaScriptV6EscapeControlCharacter && TryEscapeControlCharacter((char)i, out var abbrev))
			{
				array[i] = abbrev;
			}
			else
			{
				array[i] = '\0';
			}
		}
		return array;
	}

	private static bool TryEscapeControlCharacter(char ch, out char abbrev)
	{
		switch (ch)
		{
		case '\b':
			abbrev = 'b';
			break;
		case '\t':
			abbrev = 't';
			break;
		case '\n':
			abbrev = 'n';
			break;
		case '\f':
			abbrev = 'f';
			break;
		case '\r':
			abbrev = 'r';
			break;
		default:
			abbrev = ' ';
			return false;
		}
		return true;
	}

	public override void Close()
	{
		if (IsClosed)
		{
			return;
		}
		try
		{
			WriteEndDocument();
		}
		finally
		{
			try
			{
				nodeWriter.Flush();
				nodeWriter.Close();
			}
			finally
			{
				writeState = WriteState.Closed;
				if (depth != 0)
				{
					depth = 0;
				}
			}
		}
	}

	public override void Flush()
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		nodeWriter.Flush();
	}

	public override string LookupPrefix(string ns)
	{
		if (ns == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("ns");
		}
		if (ns == "http://www.w3.org/2000/xmlns/")
		{
			return "xmlns";
		}
		if (ns == "http://www.w3.org/XML/1998/namespace")
		{
			return "xml";
		}
		if (ns == string.Empty)
		{
			return string.Empty;
		}
		return null;
	}

	public void SetOutput(Stream stream, Encoding encoding, bool ownsStream)
	{
		if (stream == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("stream");
		}
		if (encoding == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("encoding");
		}
		if (encoding.WebName != Encoding.UTF8.WebName)
		{
			stream = new JsonEncodingStreamWrapper(stream, encoding, isReader: false);
		}
		else
		{
			encoding = null;
		}
		if (nodeWriter == null)
		{
			nodeWriter = new JsonNodeWriter();
		}
		nodeWriter.SetOutput(stream, ownsStream, encoding);
		InitializeWriter();
	}

	public override void WriteArray(string prefix, string localName, string namespaceUri, bool[] array, int offset, int count)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
	}

	public override void WriteArray(string prefix, string localName, string namespaceUri, short[] array, int offset, int count)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
	}

	public override void WriteArray(string prefix, string localName, string namespaceUri, int[] array, int offset, int count)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
	}

	public override void WriteArray(string prefix, string localName, string namespaceUri, long[] array, int offset, int count)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
	}

	public override void WriteArray(string prefix, string localName, string namespaceUri, float[] array, int offset, int count)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
	}

	public override void WriteArray(string prefix, string localName, string namespaceUri, double[] array, int offset, int count)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
	}

	public override void WriteArray(string prefix, string localName, string namespaceUri, decimal[] array, int offset, int count)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
	}

	public override void WriteArray(string prefix, string localName, string namespaceUri, DateTime[] array, int offset, int count)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
	}

	public override void WriteArray(string prefix, string localName, string namespaceUri, Guid[] array, int offset, int count)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
	}

	public override void WriteArray(string prefix, string localName, string namespaceUri, TimeSpan[] array, int offset, int count)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
	}

	public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, bool[] array, int offset, int count)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
	}

	public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, decimal[] array, int offset, int count)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
	}

	public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, double[] array, int offset, int count)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
	}

	public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, float[] array, int offset, int count)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
	}

	public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, int[] array, int offset, int count)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
	}

	public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, long[] array, int offset, int count)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
	}

	public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, short[] array, int offset, int count)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
	}

	public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, DateTime[] array, int offset, int count)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
	}

	public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, Guid[] array, int offset, int count)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
	}

	public override void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, TimeSpan[] array, int offset, int count)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("JSON WriteArray is not supported.")));
	}

	public override void WriteBase64(byte[] buffer, int index, int count)
	{
		if (buffer == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
		}
		if (index < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("index", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count > buffer.Length - index)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("JSON size exceeded remaining buffer space, by {0} byte(s).", buffer.Length - index)));
		}
		StartText();
		nodeWriter.WriteBase64Text(buffer, 0, buffer, index, count);
	}

	public override void WriteBinHex(byte[] buffer, int index, int count)
	{
		if (buffer == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
		}
		if (index < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("index", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count > buffer.Length - index)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("JSON size exceeded remaining buffer space, by {0} byte(s).", buffer.Length - index)));
		}
		StartText();
		WriteEscapedJsonString(BinHexEncoding.GetString(buffer, index, count));
	}

	public override void WriteCData(string text)
	{
		WriteString(text);
	}

	public override void WriteCharEntity(char ch)
	{
		WriteString(ch.ToString());
	}

	public override void WriteChars(char[] buffer, int index, int count)
	{
		if (buffer == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
		}
		if (index < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("index", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count > buffer.Length - index)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("JSON size exceeded remaining buffer space, by {0} byte(s).", buffer.Length - index)));
		}
		WriteString(new string(buffer, index, count));
	}

	public override void WriteComment(string text)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Method {0} is not supported in JSON.", "WriteComment")));
	}

	public override void WriteDocType(string name, string pubid, string sysid, string subset)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Method {0} is not supported in JSON.", "WriteDocType")));
	}

	public override void WriteEndAttribute()
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (!HasOpenAttribute)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("WriteEndAttribute was called while there is no open attribute.")));
		}
		if (isWritingDataTypeAttribute)
		{
			switch (attributeText)
			{
			case "number":
				ThrowIfServerTypeWritten("number");
				dataType = JsonDataType.Number;
				break;
			case "string":
				ThrowIfServerTypeWritten("string");
				dataType = JsonDataType.String;
				break;
			case "array":
				ThrowIfServerTypeWritten("array");
				dataType = JsonDataType.Array;
				break;
			case "object":
				dataType = JsonDataType.Object;
				break;
			case "null":
				ThrowIfServerTypeWritten("null");
				dataType = JsonDataType.Null;
				break;
			case "boolean":
				ThrowIfServerTypeWritten("boolean");
				dataType = JsonDataType.Boolean;
				break;
			default:
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Unexpected attribute value '{0}'.", attributeText)));
			}
			attributeText = null;
			isWritingDataTypeAttribute = false;
			if (!IsWritingNameWithMapping || WrittenNameWithMapping)
			{
				WriteDataTypeServerType();
			}
		}
		else if (isWritingServerTypeAttribute)
		{
			serverTypeValue = attributeText;
			attributeText = null;
			isWritingServerTypeAttribute = false;
			if ((!IsWritingNameWithMapping || WrittenNameWithMapping) && dataType == JsonDataType.Object)
			{
				WriteServerTypeAttribute();
			}
		}
		else if (IsWritingNameAttribute)
		{
			WriteJsonElementName(attributeText);
			attributeText = null;
			nameState = NameState.IsWritingNameWithMapping | NameState.WrittenNameWithMapping;
			WriteDataTypeServerType();
		}
		else if (isWritingXmlnsAttribute)
		{
			if (!string.IsNullOrEmpty(attributeText) && isWritingXmlnsAttributeDefaultNs)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("ns", SR.GetString("JSON namespace is specified as '{0}' but it must be empty.", attributeText));
			}
			attributeText = null;
			isWritingXmlnsAttribute = false;
			isWritingXmlnsAttributeDefaultNs = false;
		}
	}

	public override void WriteEndDocument()
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (nodeType != JsonNodeType.None)
		{
			while (depth > 0)
			{
				WriteEndElement();
			}
		}
	}

	public override void WriteEndElement()
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (depth == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Encountered an end element while there was no open element in JSON writer.")));
		}
		if (HasOpenAttribute)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON attribute must be closed first before calling {0} method.", "WriteEndElement")));
		}
		endElementBuffer = false;
		JsonNodeType jsonNodeType = ExitScope();
		if (jsonNodeType == JsonNodeType.Collection)
		{
			indentLevel--;
			if (indent)
			{
				if (nodeType == JsonNodeType.Element)
				{
					nodeWriter.WriteText(32);
				}
				else
				{
					WriteNewLine();
					WriteIndent();
				}
			}
			nodeWriter.WriteText(93);
			jsonNodeType = ExitScope();
		}
		else if (nodeType == JsonNodeType.QuotedText)
		{
			WriteJsonQuote();
		}
		else if (nodeType == JsonNodeType.Element)
		{
			if (dataType == JsonDataType.None && serverTypeValue != null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("On JSON writer data type '{0}' must be specified. Object string is '{1}', server type string is '{2}'.", "type", "object", "__type")));
			}
			if (IsWritingNameWithMapping && !WrittenNameWithMapping)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("On JSON writer data type '{0}' must be specified. Object string is '{1}', server type string is '{2}'.", "item", string.Empty, "item")));
			}
			if (dataType == JsonDataType.None || dataType == JsonDataType.String)
			{
				nodeWriter.WriteText(34);
				nodeWriter.WriteText(34);
			}
		}
		if (depth != 0)
		{
			switch (jsonNodeType)
			{
			case JsonNodeType.Element:
				endElementBuffer = true;
				break;
			case JsonNodeType.Object:
				indentLevel--;
				if (indent)
				{
					if (nodeType == JsonNodeType.Element)
					{
						nodeWriter.WriteText(32);
					}
					else
					{
						WriteNewLine();
						WriteIndent();
					}
				}
				nodeWriter.WriteText(125);
				if (depth > 0 && scopes[depth] == JsonNodeType.Element)
				{
					ExitScope();
					endElementBuffer = true;
				}
				break;
			}
		}
		dataType = JsonDataType.None;
		nodeType = JsonNodeType.EndElement;
		nameState = NameState.None;
		wroteServerTypeAttribute = false;
	}

	public override void WriteEntityRef(string name)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Method {0} is not supported in JSON.", "WriteEntityRef")));
	}

	public override void WriteFullEndElement()
	{
		WriteEndElement();
	}

	public override void WriteProcessingInstruction(string name, string text)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (!name.Equals("xml", StringComparison.OrdinalIgnoreCase))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("processing instruction is not supported in JSON writer."), "name"));
		}
		if (WriteState != WriteState.Start)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Attempt to write invalid XML declration.")));
		}
	}

	public override void WriteQualifiedName(string localName, string ns)
	{
		if (localName == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localName");
		}
		if (localName.Length == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("localName", SR.GetString("Empty string is invalid as a local name."));
		}
		if (ns == null)
		{
			ns = string.Empty;
		}
		base.WriteQualifiedName(localName, ns);
	}

	public override void WriteRaw(string data)
	{
		WriteString(data);
	}

	public override void WriteRaw(char[] buffer, int index, int count)
	{
		if (buffer == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
		}
		if (index < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("index", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count > buffer.Length - index)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("JSON size exceeded remaining buffer space, by {0} byte(s).", buffer.Length - index)));
		}
		WriteString(new string(buffer, index, count));
	}

	public override void WriteStartAttribute(string prefix, string localName, string ns)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (!string.IsNullOrEmpty(prefix))
		{
			if (!IsWritingNameWithMapping || !(prefix == "xmlns"))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("prefix", SR.GetString("JSON prefix must be null or empty. '{0}' is specified instead.", prefix));
			}
			if (ns != null && ns != "http://www.w3.org/2000/xmlns/")
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("The prefix '{0}' is bound to the namespace '{1}' and cannot be changed to '{2}'.", "xmlns", "http://www.w3.org/2000/xmlns/", ns), "ns"));
			}
		}
		else if (IsWritingNameWithMapping && ns == "http://www.w3.org/2000/xmlns/" && localName != "xmlns")
		{
			prefix = "xmlns";
		}
		if (!string.IsNullOrEmpty(ns))
		{
			if (IsWritingNameWithMapping && ns == "http://www.w3.org/2000/xmlns/")
			{
				prefix = "xmlns";
			}
			else
			{
				if (!string.IsNullOrEmpty(prefix) || !(localName == "xmlns") || !(ns == "http://www.w3.org/2000/xmlns/"))
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("ns", SR.GetString("JSON namespace is specified as '{0}' but it must be empty.", ns));
				}
				prefix = "xmlns";
				isWritingXmlnsAttributeDefaultNs = true;
			}
		}
		if (localName == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localName");
		}
		if (localName.Length == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("localName", SR.GetString("Empty string is invalid as a local name."));
		}
		if (nodeType != JsonNodeType.Element && !wroteServerTypeAttribute)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON attribute must have an owner element.")));
		}
		if (HasOpenAttribute)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON attribute must be closed first before calling {0} method.", "WriteStartAttribute")));
		}
		if (prefix == "xmlns")
		{
			isWritingXmlnsAttribute = true;
			return;
		}
		switch (localName)
		{
		case "type":
			if (dataType != JsonDataType.None)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON attribute '{0}' is already written.", "type")));
			}
			isWritingDataTypeAttribute = true;
			break;
		case "__type":
			if (serverTypeValue != null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON attribute '{0}' is already written.", "__type")));
			}
			if (dataType != JsonDataType.None && dataType != JsonDataType.Object)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Server type is specified for invalid data type in JSON. Server type: '{0}', type: '{1}', dataType: '{2}', object: '{3}'.", "__type", "type", dataType.ToString().ToLowerInvariant(), "object")));
			}
			isWritingServerTypeAttribute = true;
			break;
		case "item":
			if (WrittenNameWithMapping)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON attribute '{0}' is already written.", "item")));
			}
			if (!IsWritingNameWithMapping)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Encountered an end element while there was no open element in JSON writer.")));
			}
			nameState |= NameState.IsWritingNameAttribute;
			break;
		default:
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("localName", SR.GetString("Unexpected attribute local name '{0}'.", localName));
		}
	}

	public override void WriteStartDocument(bool standalone)
	{
		WriteStartDocument();
	}

	public override void WriteStartDocument()
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (WriteState != WriteState.Start)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid write state {1} for '{0}' method.", "WriteStartDocument", WriteState.ToString())));
		}
	}

	public override void WriteStartElement(string prefix, string localName, string ns)
	{
		if (localName == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localName");
		}
		if (localName.Length == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("localName", SR.GetString("Empty string is invalid as a local name."));
		}
		if (!string.IsNullOrEmpty(prefix) && (string.IsNullOrEmpty(ns) || !TrySetWritingNameWithMapping(localName, ns)))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("prefix", SR.GetString("JSON prefix must be null or empty. '{0}' is specified instead.", prefix));
		}
		if (!string.IsNullOrEmpty(ns) && !TrySetWritingNameWithMapping(localName, ns))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("ns", SR.GetString("JSON namespace is specified as '{0}' but it must be empty.", ns));
		}
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (HasOpenAttribute)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON attribute must be closed first before calling {0} method.", "WriteStartElement")));
		}
		if (nodeType != JsonNodeType.None && depth == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Multiple root element is not allowed on JSON writer.")));
		}
		switch (nodeType)
		{
		case JsonNodeType.None:
			if (!localName.Equals("root"))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid root element name '{0}' (root element is '{1}' in JSON).", localName, "root")));
			}
			EnterScope(JsonNodeType.Element);
			break;
		case JsonNodeType.Element:
			if (dataType != JsonDataType.Array && dataType != JsonDataType.Object)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Either Object or Array of JSON node type must be specified.")));
			}
			if (indent)
			{
				WriteNewLine();
				WriteIndent();
			}
			if (!IsWritingCollection)
			{
				if (nameState != NameState.IsWritingNameWithMapping)
				{
					WriteJsonElementName(localName);
				}
			}
			else if (!localName.Equals("item"))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid JSON item name '{0}' for array element (item element is '{1}' in JSON).", localName, "item")));
			}
			EnterScope(JsonNodeType.Element);
			break;
		case JsonNodeType.EndElement:
			if (endElementBuffer)
			{
				nodeWriter.WriteText(44);
			}
			if (indent)
			{
				WriteNewLine();
				WriteIndent();
			}
			if (!IsWritingCollection)
			{
				if (nameState != NameState.IsWritingNameWithMapping)
				{
					WriteJsonElementName(localName);
				}
			}
			else if (!localName.Equals("item"))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid JSON item name '{0}' for array element (item element is '{1}' in JSON).", localName, "item")));
			}
			EnterScope(JsonNodeType.Element);
			break;
		default:
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid call to JSON WriteStartElement method.")));
		}
		isWritingDataTypeAttribute = false;
		isWritingServerTypeAttribute = false;
		isWritingXmlnsAttribute = false;
		wroteServerTypeAttribute = false;
		serverTypeValue = null;
		dataType = JsonDataType.None;
		nodeType = JsonNodeType.Element;
	}

	public override void WriteString(string text)
	{
		if (HasOpenAttribute && text != null)
		{
			attributeText += text;
			return;
		}
		if (text == null)
		{
			text = string.Empty;
		}
		if ((dataType != JsonDataType.Array && dataType != JsonDataType.Object && nodeType != JsonNodeType.EndElement) || !XmlConverter.IsWhitespace(text))
		{
			StartText();
			WriteEscapedJsonString(text);
		}
	}

	public override void WriteSurrogateCharEntity(char lowChar, char highChar)
	{
		WriteString(string.Concat(highChar, lowChar));
	}

	public override void WriteValue(bool value)
	{
		StartText();
		nodeWriter.WriteBoolText(value);
	}

	public override void WriteValue(decimal value)
	{
		StartText();
		nodeWriter.WriteDecimalText(value);
	}

	public override void WriteValue(double value)
	{
		StartText();
		nodeWriter.WriteDoubleText(value);
	}

	public override void WriteValue(float value)
	{
		StartText();
		nodeWriter.WriteFloatText(value);
	}

	public override void WriteValue(int value)
	{
		StartText();
		nodeWriter.WriteInt32Text(value);
	}

	public override void WriteValue(long value)
	{
		StartText();
		nodeWriter.WriteInt64Text(value);
	}

	public override void WriteValue(Guid value)
	{
		StartText();
		nodeWriter.WriteGuidText(value);
	}

	public override void WriteValue(DateTime value)
	{
		StartText();
		nodeWriter.WriteDateTimeText(value);
	}

	public override void WriteValue(string value)
	{
		WriteString(value);
	}

	public override void WriteValue(TimeSpan value)
	{
		StartText();
		nodeWriter.WriteTimeSpanText(value);
	}

	public override void WriteValue(UniqueId value)
	{
		if (value == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
		}
		StartText();
		nodeWriter.WriteUniqueIdText(value);
	}

	public override void WriteValue(object value)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (value == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
		}
		if (value is Array)
		{
			WriteValue((Array)value);
		}
		else if (value is IStreamProvider)
		{
			WriteValue((IStreamProvider)value);
		}
		else
		{
			WritePrimitiveValue(value);
		}
	}

	public override void WriteWhitespace(string ws)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (ws == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("ws");
		}
		for (int i = 0; i < ws.Length; i++)
		{
			char c = ws[i];
			if (c != ' ' && c != '\t' && c != '\n' && c != '\r')
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("ws", SR.GetString("Only whitespace characters are allowed for {1} method. The specified value is '{0}'", c.ToString(), "WriteWhitespace"));
			}
		}
		WriteString(ws);
	}

	public override void WriteXmlAttribute(string localName, string value)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Method {0} is not supported in JSON.", "WriteXmlAttribute")));
	}

	public override void WriteXmlAttribute(XmlDictionaryString localName, XmlDictionaryString value)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Method {0} is not supported in JSON.", "WriteXmlAttribute")));
	}

	public override void WriteXmlnsAttribute(string prefix, string namespaceUri)
	{
		if (!IsWritingNameWithMapping)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Method {0} is not supported in JSON.", "WriteXmlnsAttribute")));
		}
	}

	public override void WriteXmlnsAttribute(string prefix, XmlDictionaryString namespaceUri)
	{
		if (!IsWritingNameWithMapping)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Method {0} is not supported in JSON.", "WriteXmlnsAttribute")));
		}
	}

	internal static bool CharacterNeedsEscaping(char ch)
	{
		if (ch != '/' && ch != '"' && ch >= ' ' && ch != '\\')
		{
			if (ch >= '\ud800')
			{
				if (ch > '\udfff')
				{
					return ch >= '\ufffe';
				}
				return true;
			}
			return false;
		}
		return true;
	}

	private static void ThrowClosed()
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("JSON writer is already closed.")));
	}

	private void CheckText(JsonNodeType nextNodeType)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (depth == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Text cannot be written outside the root element.")));
		}
		if (nextNodeType == JsonNodeType.StandaloneText && nodeType == JsonNodeType.QuotedText)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON writer cannot write standalone text after quoted text.")));
		}
	}

	private void EnterScope(JsonNodeType currentNodeType)
	{
		depth++;
		if (scopes == null)
		{
			scopes = new JsonNodeType[4];
		}
		else if (scopes.Length == depth)
		{
			JsonNodeType[] destinationArray = new JsonNodeType[depth * 2];
			Array.Copy(scopes, destinationArray, depth);
			scopes = destinationArray;
		}
		scopes[depth] = currentNodeType;
	}

	private JsonNodeType ExitScope()
	{
		JsonNodeType result = scopes[depth];
		scopes[depth] = JsonNodeType.None;
		depth--;
		return result;
	}

	private void InitializeWriter()
	{
		nodeType = JsonNodeType.None;
		dataType = JsonDataType.None;
		isWritingDataTypeAttribute = false;
		wroteServerTypeAttribute = false;
		isWritingServerTypeAttribute = false;
		serverTypeValue = null;
		attributeText = null;
		if (depth != 0)
		{
			depth = 0;
		}
		if (scopes != null && scopes.Length > 25)
		{
			scopes = null;
		}
		writeState = WriteState.Start;
		endElementBuffer = false;
		indentLevel = 0;
	}

	private static bool IsUnicodeNewlineCharacter(char c)
	{
		if (c != '\u0085' && c != '\u2028')
		{
			return c == '\u2029';
		}
		return true;
	}

	private void StartText()
	{
		if (HasOpenAttribute)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("On JSON writer WriteString must be used for writing attribute values.")));
		}
		if (dataType == JsonDataType.None && serverTypeValue != null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("On JSON writer data type '{0}' must be specified. Object string is '{1}', server type string is '{2}'.", "type", "object", "__type")));
		}
		if (IsWritingNameWithMapping && !WrittenNameWithMapping)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("On JSON writer data type '{0}' must be specified. Object string is '{1}', server type string is '{2}'.", "item", string.Empty, "item")));
		}
		if (dataType == JsonDataType.String || dataType == JsonDataType.None)
		{
			CheckText(JsonNodeType.QuotedText);
			if (nodeType != JsonNodeType.QuotedText)
			{
				WriteJsonQuote();
			}
			nodeType = JsonNodeType.QuotedText;
		}
		else if (dataType == JsonDataType.Number || dataType == JsonDataType.Boolean)
		{
			CheckText(JsonNodeType.StandaloneText);
			nodeType = JsonNodeType.StandaloneText;
		}
		else
		{
			ThrowInvalidAttributeContent();
		}
	}

	private void ThrowIfServerTypeWritten(string dataTypeSpecified)
	{
		if (serverTypeValue != null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("The specified data type is invalid for server type. Type: '{0}', specified data type: '{1}', server type: '{2}', object '{3}'.", "type", dataTypeSpecified, "__type", "object")));
		}
	}

	private void ThrowInvalidAttributeContent()
	{
		if (HasOpenAttribute)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid method call state between start and end attribute.")));
		}
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("JSON writer cannot write text after non-text attribute. Data type is '{0}'.", dataType.ToString().ToLowerInvariant())));
	}

	private bool TrySetWritingNameWithMapping(string localName, string ns)
	{
		if (localName.Equals("item") && ns.Equals("item"))
		{
			nameState = NameState.IsWritingNameWithMapping;
			return true;
		}
		return false;
	}

	private void WriteDataTypeServerType()
	{
		if (dataType != JsonDataType.None)
		{
			switch (dataType)
			{
			case JsonDataType.Array:
				EnterScope(JsonNodeType.Collection);
				nodeWriter.WriteText(91);
				indentLevel++;
				break;
			case JsonDataType.Object:
				EnterScope(JsonNodeType.Object);
				nodeWriter.WriteText(123);
				indentLevel++;
				break;
			case JsonDataType.Null:
				nodeWriter.WriteText("null");
				break;
			}
			if (serverTypeValue != null)
			{
				WriteServerTypeAttribute();
			}
		}
	}

	[SecuritySafeCritical]
	private unsafe void WriteEscapedJsonString(string str)
	{
		fixed (char* ptr = str)
		{
			int num = 0;
			int i;
			for (i = 0; i < str.Length; i++)
			{
				char c = ptr[i];
				if (c <= '/')
				{
					if (c == '/' || c == '"')
					{
						nodeWriter.WriteChars(ptr + num, i - num);
						nodeWriter.WriteText(92);
						nodeWriter.WriteText(c);
						num = i + 1;
					}
					else if (c < ' ')
					{
						nodeWriter.WriteChars(ptr + num, i - num);
						nodeWriter.WriteText(92);
						if (CharacterAbbrevs[(uint)c] == '\0')
						{
							nodeWriter.WriteText(117);
							nodeWriter.WriteText(string.Format(CultureInfo.InvariantCulture, "{0:x4}", (int)c));
							num = i + 1;
						}
						else
						{
							nodeWriter.WriteText(CharacterAbbrevs[(uint)c]);
							num = i + 1;
						}
					}
				}
				else if (c == '\\')
				{
					nodeWriter.WriteChars(ptr + num, i - num);
					nodeWriter.WriteText(92);
					nodeWriter.WriteText(c);
					num = i + 1;
				}
				else if ((c >= '\ud800' && (c <= '\udfff' || c >= '\ufffe')) || IsUnicodeNewlineCharacter(c))
				{
					nodeWriter.WriteChars(ptr + num, i - num);
					nodeWriter.WriteText(92);
					nodeWriter.WriteText(117);
					nodeWriter.WriteText(string.Format(CultureInfo.InvariantCulture, "{0:x4}", (int)c));
					num = i + 1;
				}
			}
			if (num < i)
			{
				nodeWriter.WriteChars(ptr + num, i - num);
			}
		}
	}

	private void WriteIndent()
	{
		for (int i = 0; i < indentLevel; i++)
		{
			nodeWriter.WriteText(indentChars);
		}
	}

	private void WriteNewLine()
	{
		nodeWriter.WriteText(13);
		nodeWriter.WriteText(10);
	}

	private void WriteJsonElementName(string localName)
	{
		WriteJsonQuote();
		WriteEscapedJsonString(localName);
		WriteJsonQuote();
		nodeWriter.WriteText(58);
		if (indent)
		{
			nodeWriter.WriteText(32);
		}
	}

	private void WriteJsonQuote()
	{
		nodeWriter.WriteText(34);
	}

	private void WritePrimitiveValue(object value)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (value == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("value"));
		}
		if (value is ulong)
		{
			WriteValue((ulong)value);
			return;
		}
		if (value is string)
		{
			WriteValue((string)value);
			return;
		}
		if (value is int)
		{
			WriteValue((int)value);
			return;
		}
		if (value is long)
		{
			WriteValue((long)value);
			return;
		}
		if (value is bool)
		{
			WriteValue((bool)value);
			return;
		}
		if (value is double)
		{
			WriteValue((double)value);
			return;
		}
		if (value is DateTime)
		{
			WriteValue((DateTime)value);
			return;
		}
		if (value is float)
		{
			WriteValue((float)value);
			return;
		}
		if (value is decimal)
		{
			WriteValue((decimal)value);
			return;
		}
		if (value is XmlDictionaryString)
		{
			WriteValue((XmlDictionaryString)value);
			return;
		}
		if (value is UniqueId)
		{
			WriteValue((UniqueId)value);
			return;
		}
		if (value is Guid)
		{
			WriteValue((Guid)value);
			return;
		}
		if (value is TimeSpan)
		{
			WriteValue((TimeSpan)value);
			return;
		}
		if (value.GetType().IsArray)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Nested array is not supported in JSON: '{0}'"), "value"));
		}
		base.WriteValue(value);
	}

	private void WriteServerTypeAttribute()
	{
		string value = serverTypeValue;
		JsonDataType jsonDataType = dataType;
		NameState nameState = this.nameState;
		WriteStartElement("__type");
		WriteValue(value);
		WriteEndElement();
		dataType = jsonDataType;
		this.nameState = nameState;
		wroteServerTypeAttribute = true;
	}

	private void WriteValue(ulong value)
	{
		StartText();
		nodeWriter.WriteUInt64Text(value);
	}

	private void WriteValue(Array array)
	{
		JsonDataType jsonDataType = dataType;
		dataType = JsonDataType.String;
		StartText();
		for (int i = 0; i < array.Length; i++)
		{
			if (i != 0)
			{
				nodeWriter.WriteText(32);
			}
			WritePrimitiveValue(array.GetValue(i));
		}
		dataType = jsonDataType;
	}
}
