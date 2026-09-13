using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Diagnostics.Application;
using System.Text;

namespace System.Xml;

internal static class XmlExceptionHelper
{
	private static void ThrowXmlException(XmlDictionaryReader reader, string res)
	{
		ThrowXmlException(reader, res, null);
	}

	private static void ThrowXmlException(XmlDictionaryReader reader, string res, string arg1)
	{
		ThrowXmlException(reader, res, arg1, null);
	}

	private static void ThrowXmlException(XmlDictionaryReader reader, string res, string arg1, string arg2)
	{
		ThrowXmlException(reader, res, arg1, arg2, null);
	}

	private static void ThrowXmlException(XmlDictionaryReader reader, string res, string arg1, string arg2, string arg3)
	{
		string text = SR.GetString(res, arg1, arg2, arg3);
		if (reader is IXmlLineInfo xmlLineInfo && xmlLineInfo.HasLineInfo())
		{
			text = text + " " + SR.GetString("Line {0}, position {1}.", xmlLineInfo.LineNumber, xmlLineInfo.LinePosition);
		}
		if (TD.ReaderQuotaExceededIsEnabled())
		{
			TD.ReaderQuotaExceeded(text);
		}
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(text));
	}

	public static void ThrowXmlException(XmlDictionaryReader reader, XmlException exception)
	{
		string text = exception.Message;
		if (reader is IXmlLineInfo xmlLineInfo && xmlLineInfo.HasLineInfo())
		{
			text = text + " " + SR.GetString("Line {0}, position {1}.", xmlLineInfo.LineNumber, xmlLineInfo.LinePosition);
		}
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(text));
	}

	private static string GetName(string prefix, string localName)
	{
		if (prefix.Length == 0)
		{
			return localName;
		}
		return prefix + ":" + localName;
	}

	private static string GetWhatWasFound(XmlDictionaryReader reader)
	{
		if (reader.EOF)
		{
			return SR.GetString("end of file");
		}
		switch (reader.NodeType)
		{
		case XmlNodeType.Element:
			return SR.GetString("element '{0}' from namespace '{1}'", GetName(reader.Prefix, reader.LocalName), reader.NamespaceURI);
		case XmlNodeType.EndElement:
			return SR.GetString("end element '{0}' from namespace '{1}'", GetName(reader.Prefix, reader.LocalName), reader.NamespaceURI);
		case XmlNodeType.Text:
		case XmlNodeType.Whitespace:
		case XmlNodeType.SignificantWhitespace:
			return SR.GetString("text '{0}'", reader.Value);
		case XmlNodeType.Comment:
			return SR.GetString("comment '{0}'", reader.Value);
		case XmlNodeType.CDATA:
			return SR.GetString("cdata '{0}'", reader.Value);
		default:
			return SR.GetString("node {0}", reader.NodeType);
		}
	}

	public static void ThrowStartElementExpected(XmlDictionaryReader reader)
	{
		ThrowXmlException(reader, "Start element expected. Found {0}.", GetWhatWasFound(reader));
	}

	public static void ThrowStartElementExpected(XmlDictionaryReader reader, string name)
	{
		ThrowXmlException(reader, "Start element '{0}' expected. Found {1}.", name, GetWhatWasFound(reader));
	}

	public static void ThrowStartElementExpected(XmlDictionaryReader reader, string localName, string ns)
	{
		ThrowXmlException(reader, "Start element '{0}' from namespace '{1}' expected. Found {2}.", localName, ns, GetWhatWasFound(reader));
	}

	public static void ThrowStartElementExpected(XmlDictionaryReader reader, XmlDictionaryString localName, XmlDictionaryString ns)
	{
		ThrowStartElementExpected(reader, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(ns));
	}

	public static void ThrowFullStartElementExpected(XmlDictionaryReader reader)
	{
		ThrowXmlException(reader, "Non-empty start element expected. Found {0}.", GetWhatWasFound(reader));
	}

	public static void ThrowFullStartElementExpected(XmlDictionaryReader reader, string name)
	{
		ThrowXmlException(reader, "Non-empty start element '{0}' expected. Found {1}.", name, GetWhatWasFound(reader));
	}

	public static void ThrowFullStartElementExpected(XmlDictionaryReader reader, string localName, string ns)
	{
		ThrowXmlException(reader, "Non-empty start element '{0}' from namespace '{1}' expected. Found {2}.", localName, ns, GetWhatWasFound(reader));
	}

	public static void ThrowFullStartElementExpected(XmlDictionaryReader reader, XmlDictionaryString localName, XmlDictionaryString ns)
	{
		ThrowFullStartElementExpected(reader, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(ns));
	}

	public static void ThrowEndElementExpected(XmlDictionaryReader reader, string localName, string ns)
	{
		ThrowXmlException(reader, "End element '{0}' from namespace '{1}' expected. Found {2}.", localName, ns, GetWhatWasFound(reader));
	}

	public static void ThrowMaxStringContentLengthExceeded(XmlDictionaryReader reader, int maxStringContentLength)
	{
		ThrowXmlException(reader, "XML max string content length exceeded. It must be less than {0}.", maxStringContentLength.ToString(NumberFormatInfo.CurrentInfo));
	}

	public static void ThrowMaxArrayLengthExceeded(XmlDictionaryReader reader, int maxArrayLength)
	{
		ThrowXmlException(reader, "The maximum array length quota ({0}) has been exceeded while reading XML data. This quota may be increased by changing the MaxArrayLength property on the XmlDictionaryReaderQuotas object used when creating the XML reader.", maxArrayLength.ToString(NumberFormatInfo.CurrentInfo));
	}

	public static void ThrowMaxArrayLengthOrMaxItemsQuotaExceeded(XmlDictionaryReader reader, int maxQuota)
	{
		ThrowXmlException(reader, "XML max array length or max items quota exceeded. It must be less than {0}.", maxQuota.ToString(NumberFormatInfo.CurrentInfo));
	}

	public static void ThrowMaxDepthExceeded(XmlDictionaryReader reader, int maxDepth)
	{
		ThrowXmlException(reader, "XML max depth exceeded. It must be less than {0}.", maxDepth.ToString(NumberFormatInfo.CurrentInfo));
	}

	public static void ThrowMaxBytesPerReadExceeded(XmlDictionaryReader reader, int maxBytesPerRead)
	{
		ThrowXmlException(reader, "XML max bytes per read exceeded. It must be less than {0}.", maxBytesPerRead.ToString(NumberFormatInfo.CurrentInfo));
	}

	public static void ThrowMaxNameTableCharCountExceeded(XmlDictionaryReader reader, int maxNameTableCharCount)
	{
		ThrowXmlException(reader, "The maximum nametable character count quota ({0}) has been exceeded while reading XML data. The nametable is a data structure used to store strings encountered during XML processing - long XML documents with non-repeating element names, attribute names and attribute values may trigger this quota. This quota may be increased by changing the MaxNameTableCharCount property on the XmlDictionaryReaderQuotas object used when creating the XML reader.", maxNameTableCharCount.ToString(NumberFormatInfo.CurrentInfo));
	}

	public static void ThrowBase64DataExpected(XmlDictionaryReader reader)
	{
		ThrowXmlException(reader, "Base64 encoded data expected. Found {0}.", GetWhatWasFound(reader));
	}

	public static void ThrowUndefinedPrefix(XmlDictionaryReader reader, string prefix)
	{
		ThrowXmlException(reader, "The prefix '{0}' is not defined.", prefix);
	}

	public static void ThrowProcessingInstructionNotSupported(XmlDictionaryReader reader)
	{
		ThrowXmlException(reader, "Processing instructions (other than the XML declaration) and DTDs are not supported.");
	}

	public static void ThrowInvalidXml(XmlDictionaryReader reader, byte b)
	{
		ThrowXmlException(reader, "The byte 0x{0} is not valid at this location.", b.ToString("X2", CultureInfo.InvariantCulture));
	}

	public static void ThrowUnexpectedEndOfFile(XmlDictionaryReader reader)
	{
		ThrowXmlException(reader, "Unexpected end of file. Following elements are not closed: {0}.", ((XmlBaseReader)reader).GetOpenElements());
	}

	public static void ThrowUnexpectedEndElement(XmlDictionaryReader reader)
	{
		ThrowXmlException(reader, "No matching start tag for end element.");
	}

	public static void ThrowTokenExpected(XmlDictionaryReader reader, string expected, char found)
	{
		ThrowXmlException(reader, "The token '{0}' was expected but found '{1}'.", expected, found.ToString());
	}

	public static void ThrowTokenExpected(XmlDictionaryReader reader, string expected, string found)
	{
		ThrowXmlException(reader, "The token '{0}' was expected but found '{1}'.", expected, found);
	}

	public static void ThrowInvalidCharRef(XmlDictionaryReader reader)
	{
		ThrowXmlException(reader, "Character reference not valid.");
	}

	public static void ThrowTagMismatch(XmlDictionaryReader reader, string expectedPrefix, string expectedLocalName, string foundPrefix, string foundLocalName)
	{
		ThrowXmlException(reader, "Start element '{0}' does not match end element '{1}'.", GetName(expectedPrefix, expectedLocalName), GetName(foundPrefix, foundLocalName));
	}

	public static void ThrowDuplicateXmlnsAttribute(XmlDictionaryReader reader, string localName, string ns)
	{
		string text = ((localName.Length != 0) ? ("xmlns:" + localName) : "xmlns");
		ThrowXmlException(reader, "Duplicate attribute found. Both '{0}' and '{1}' are from the namespace '{2}'.", text, text, ns);
	}

	public static void ThrowDuplicateAttribute(XmlDictionaryReader reader, string prefix1, string prefix2, string localName, string ns)
	{
		ThrowXmlException(reader, "Duplicate attribute found. Both '{0}' and '{1}' are from the namespace '{2}'.", GetName(prefix1, localName), GetName(prefix2, localName), ns);
	}

	public static void ThrowInvalidBinaryFormat(XmlDictionaryReader reader)
	{
		ThrowXmlException(reader, "The input source is not correctly formatted.");
	}

	public static void ThrowInvalidRootData(XmlDictionaryReader reader)
	{
		ThrowXmlException(reader, "The data at the root level is invalid.");
	}

	public static void ThrowMultipleRootElements(XmlDictionaryReader reader)
	{
		ThrowXmlException(reader, "There are multiple root elements.");
	}

	public static void ThrowDeclarationNotFirst(XmlDictionaryReader reader)
	{
		ThrowXmlException(reader, "No characters can appear before the XML declaration.");
	}

	public static void ThrowConversionOverflow(XmlDictionaryReader reader, string value, string type)
	{
		ThrowXmlException(reader, "The value '{0}' cannot be represented with the type '{1}'.", value, type);
	}

	public static void ThrowXmlDictionaryStringIDOutOfRange(XmlDictionaryReader reader)
	{
		ThrowXmlException(reader, "XmlDictionaryString IDs must be in the range from {0} to {1}.", 0.ToString(NumberFormatInfo.CurrentInfo), 536870911.ToString(NumberFormatInfo.CurrentInfo));
	}

	public static void ThrowXmlDictionaryStringIDUndefinedStatic(XmlDictionaryReader reader, int key)
	{
		ThrowXmlException(reader, "XmlDictionaryString ID {0} not defined in the static dictionary.", key.ToString(NumberFormatInfo.CurrentInfo));
	}

	public static void ThrowXmlDictionaryStringIDUndefinedSession(XmlDictionaryReader reader, int key)
	{
		ThrowXmlException(reader, "XmlDictionaryString ID {0} not defined in the XmlBinaryReaderSession.", key.ToString(NumberFormatInfo.CurrentInfo));
	}

	public static void ThrowEmptyNamespace(XmlDictionaryReader reader)
	{
		ThrowXmlException(reader, "The empty namespace requires a null or empty prefix.");
	}

	public static XmlException CreateConversionException(string value, string type, Exception exception)
	{
		return new XmlException(SR.GetString("The value '{0}' cannot be parsed as the type '{1}'.", value, type), exception);
	}

	public static XmlException CreateEncodingException(byte[] buffer, int offset, int count, Exception exception)
	{
		return CreateEncodingException(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: false).GetString(buffer, offset, count), exception);
	}

	public static XmlException CreateEncodingException(string value, Exception exception)
	{
		return new XmlException(SR.GetString("'{0}' contains invalid UTF8 bytes.", value), exception);
	}
}
