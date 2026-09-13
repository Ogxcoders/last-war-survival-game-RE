using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Xml.XPath;

namespace System.Xml;

internal class XmlMtomWriter : XmlDictionaryWriter, IXmlMtomWriterInitializer
{
	private static class MimeBoundaryGenerator
	{
		private static long id;

		private static string prefix;

		static MimeBoundaryGenerator()
		{
			prefix = Guid.NewGuid().ToString() + "+id=";
		}

		internal static string Next()
		{
			long num = Interlocked.Increment(ref id);
			return string.Format(CultureInfo.InvariantCulture, "{0}{1}", prefix, num);
		}
	}

	private class MimePart
	{
		internal IList<MtomBinaryData> binaryData;

		internal string contentID;

		internal string contentType;

		internal string contentTransferEncoding;

		internal int sizeInBytes;

		internal MimePart(IList<MtomBinaryData> binaryData, string contentID, string contentType, string contentTransferEncoding, int sizeOfBufferedBinaryData, int maxSizeInBytes)
		{
			this.binaryData = binaryData;
			this.contentID = contentID;
			this.contentType = contentType ?? MtomGlobals.DefaultContentTypeForBinary;
			this.contentTransferEncoding = contentTransferEncoding;
			sizeInBytes = GetSize(contentID, contentType, contentTransferEncoding, sizeOfBufferedBinaryData, maxSizeInBytes);
		}

		private static int GetSize(string contentID, string contentType, string contentTransferEncoding, int sizeOfBufferedBinaryData, int maxSizeInBytes)
		{
			int num = ValidateSizeOfMessage(maxSizeInBytes, 0, MimeGlobals.CRLF.Length * 3);
			if (contentTransferEncoding != null)
			{
				num += ValidateSizeOfMessage(maxSizeInBytes, num, MimeWriter.GetHeaderSize(MimeGlobals.ContentTransferEncodingHeader, contentTransferEncoding, maxSizeInBytes));
			}
			if (contentType != null)
			{
				num += ValidateSizeOfMessage(maxSizeInBytes, num, MimeWriter.GetHeaderSize(MimeGlobals.ContentTypeHeader, contentType, maxSizeInBytes));
			}
			if (contentID != null)
			{
				num += ValidateSizeOfMessage(maxSizeInBytes, num, MimeWriter.GetHeaderSize(MimeGlobals.ContentIDHeader, contentID, maxSizeInBytes));
				num += ValidateSizeOfMessage(maxSizeInBytes, num, 2);
			}
			return num + ValidateSizeOfMessage(maxSizeInBytes, num, sizeOfBufferedBinaryData);
		}
	}

	private const int MaxInlinedBytes = 767;

	private int maxSizeInBytes;

	private XmlDictionaryWriter writer;

	private XmlDictionaryWriter infosetWriter;

	private MimeWriter mimeWriter;

	private Encoding encoding;

	private bool isUTF8;

	private string contentID;

	private string contentType;

	private string initialContentTypeForRootPart;

	private string initialContentTypeForMimeMessage;

	private MemoryStream contentTypeStream;

	private List<MimePart> mimeParts;

	private IList<MtomBinaryData> binaryDataChunks;

	private int depth;

	private int totalSizeOfMimeParts;

	private int sizeOfBufferedBinaryData;

	private char[] chars;

	private byte[] bytes;

	private bool isClosed;

	private bool ownsStream;

	private XmlDictionaryWriter Writer
	{
		get
		{
			if (!IsInitialized)
			{
				Initialize();
			}
			return writer;
		}
	}

	private bool IsInitialized => initialContentTypeForRootPart == null;

	public override XmlWriterSettings Settings => Writer.Settings;

	public override WriteState WriteState => Writer.WriteState;

	public override string XmlLang => Writer.XmlLang;

	public override XmlSpace XmlSpace => Writer.XmlSpace;

	public void SetOutput(Stream stream, Encoding encoding, int maxSizeInBytes, string startInfo, string boundary, string startUri, bool writeMessageHeaders, bool ownsStream)
	{
		if (encoding == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("encoding");
		}
		if (maxSizeInBytes < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("maxSizeInBytes", SR.GetString("The value of this argument must be non-negative.")));
		}
		this.maxSizeInBytes = maxSizeInBytes;
		this.encoding = encoding;
		isUTF8 = IsUTF8Encoding(encoding);
		Initialize(stream, startInfo, boundary, startUri, writeMessageHeaders, ownsStream);
	}

	private void Initialize(Stream stream, string startInfo, string boundary, string startUri, bool writeMessageHeaders, bool ownsStream)
	{
		if (stream == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("stream");
		}
		if (startInfo == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("startInfo");
		}
		if (boundary == null)
		{
			boundary = GetBoundaryString();
		}
		if (startUri == null)
		{
			startUri = GenerateUriForMimePart(0);
		}
		if (!MailBnfHelper.IsValidMimeBoundary(boundary))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("MIME boundary is invalid: '{0}'.", boundary), "boundary"));
		}
		this.ownsStream = ownsStream;
		isClosed = false;
		depth = 0;
		totalSizeOfMimeParts = 0;
		sizeOfBufferedBinaryData = 0;
		binaryDataChunks = null;
		contentType = null;
		contentTypeStream = null;
		contentID = startUri;
		if (mimeParts != null)
		{
			mimeParts.Clear();
		}
		mimeWriter = new MimeWriter(stream, boundary);
		initialContentTypeForRootPart = GetContentTypeForRootMimePart(encoding, startInfo);
		if (writeMessageHeaders)
		{
			initialContentTypeForMimeMessage = GetContentTypeForMimeMessage(boundary, startUri, startInfo);
		}
	}

	private void Initialize()
	{
		if (isClosed)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("The XmlWriter is closed.")));
		}
		if (initialContentTypeForRootPart != null)
		{
			if (initialContentTypeForMimeMessage != null)
			{
				mimeWriter.StartPreface();
				mimeWriter.WriteHeader(MimeGlobals.MimeVersionHeader, MimeGlobals.DefaultVersion);
				mimeWriter.WriteHeader(MimeGlobals.ContentTypeHeader, initialContentTypeForMimeMessage);
				initialContentTypeForMimeMessage = null;
			}
			WriteMimeHeaders(contentID, initialContentTypeForRootPart, isUTF8 ? MimeGlobals.Encoding8bit : MimeGlobals.EncodingBinary);
			Stream contentStream = mimeWriter.GetContentStream();
			if (!(writer is IXmlTextWriterInitializer xmlTextWriterInitializer))
			{
				writer = XmlDictionaryWriter.CreateTextWriter(contentStream, encoding, ownsStream);
			}
			else
			{
				xmlTextWriterInitializer.SetOutput(contentStream, encoding, ownsStream);
			}
			contentID = null;
			initialContentTypeForRootPart = null;
		}
	}

	private static string GetBoundaryString()
	{
		return MimeBoundaryGenerator.Next();
	}

	internal static bool IsUTF8Encoding(Encoding encoding)
	{
		return encoding.WebName == "utf-8";
	}

	private static string GetContentTypeForMimeMessage(string boundary, string startUri, string startInfo)
	{
		StringBuilder stringBuilder = new StringBuilder(string.Format(CultureInfo.InvariantCulture, "{0}/{1};{2}=\"{3}\";{4}=\"{5}\"", MtomGlobals.MediaType, MtomGlobals.MediaSubtype, MtomGlobals.TypeParam, MtomGlobals.XopType, MtomGlobals.BoundaryParam, boundary));
		if (startUri != null && startUri.Length > 0)
		{
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, ";{0}=\"<{1}>\"", MtomGlobals.StartParam, startUri);
		}
		if (startInfo != null && startInfo.Length > 0)
		{
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, ";{0}=\"{1}\"", MtomGlobals.StartInfoParam, startInfo);
		}
		return stringBuilder.ToString();
	}

	private static string GetContentTypeForRootMimePart(Encoding encoding, string startInfo)
	{
		string text = string.Format(CultureInfo.InvariantCulture, "{0};{1}={2}", MtomGlobals.XopType, MtomGlobals.CharsetParam, CharSet(encoding));
		if (startInfo != null)
		{
			text = string.Format(CultureInfo.InvariantCulture, "{0};{1}=\"{2}\"", text, MtomGlobals.TypeParam, startInfo);
		}
		return text;
	}

	private static string CharSet(Encoding enc)
	{
		string webName = enc.WebName;
		if (string.Compare(webName, Encoding.UTF8.WebName, StringComparison.OrdinalIgnoreCase) == 0)
		{
			return webName;
		}
		if (string.Compare(webName, Encoding.Unicode.WebName, StringComparison.OrdinalIgnoreCase) == 0)
		{
			return "utf-16LE";
		}
		if (string.Compare(webName, Encoding.BigEndianUnicode.WebName, StringComparison.OrdinalIgnoreCase) == 0)
		{
			return "utf-16BE";
		}
		return webName;
	}

	public override void WriteStartElement(string prefix, string localName, string ns)
	{
		WriteBase64InlineIfPresent();
		ThrowIfElementIsXOPInclude(prefix, localName, ns);
		Writer.WriteStartElement(prefix, localName, ns);
		depth++;
	}

	public override void WriteStartElement(string prefix, XmlDictionaryString localName, XmlDictionaryString ns)
	{
		if (localName == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localName");
		}
		WriteBase64InlineIfPresent();
		ThrowIfElementIsXOPInclude(prefix, localName.Value, ns?.Value);
		Writer.WriteStartElement(prefix, localName, ns);
		depth++;
	}

	private void ThrowIfElementIsXOPInclude(string prefix, string localName, string ns)
	{
		if (ns == null && Writer is XmlBaseWriter xmlBaseWriter)
		{
			ns = xmlBaseWriter.LookupNamespace(prefix);
		}
		if (localName == MtomGlobals.XopIncludeLocalName && ns == MtomGlobals.XopIncludeNamespace)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("MTOM data must not contain xop:Include element. '{0}' element in '{1}' namespace.", MtomGlobals.XopIncludeLocalName, MtomGlobals.XopIncludeNamespace)));
		}
	}

	public override void WriteEndElement()
	{
		WriteXOPInclude();
		Writer.WriteEndElement();
		depth--;
		WriteXOPBinaryParts();
	}

	public override void WriteFullEndElement()
	{
		WriteXOPInclude();
		Writer.WriteFullEndElement();
		depth--;
		WriteXOPBinaryParts();
	}

	public override void WriteValue(IStreamProvider value)
	{
		if (value == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("value"));
		}
		if (Writer.WriteState == WriteState.Element)
		{
			if (binaryDataChunks == null)
			{
				binaryDataChunks = new List<MtomBinaryData>();
				contentID = GenerateUriForMimePart((mimeParts == null) ? 1 : (mimeParts.Count + 1));
			}
			binaryDataChunks.Add(new MtomBinaryData(value));
		}
		else
		{
			Writer.WriteValue(value);
		}
	}

	public override void WriteBase64(byte[] buffer, int index, int count)
	{
		if (Writer.WriteState == WriteState.Element)
		{
			if (buffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("buffer"));
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
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", buffer.Length - index)));
			}
			if (binaryDataChunks == null)
			{
				binaryDataChunks = new List<MtomBinaryData>();
				contentID = GenerateUriForMimePart((mimeParts == null) ? 1 : (mimeParts.Count + 1));
			}
			int num = ValidateSizeOfMessage(maxSizeInBytes, 0, totalSizeOfMimeParts);
			num += ValidateSizeOfMessage(maxSizeInBytes, num, sizeOfBufferedBinaryData);
			num += ValidateSizeOfMessage(maxSizeInBytes, num, count);
			sizeOfBufferedBinaryData += count;
			binaryDataChunks.Add(new MtomBinaryData(buffer, index, count));
		}
		else
		{
			Writer.WriteBase64(buffer, index, count);
		}
	}

	internal static int ValidateSizeOfMessage(int maxSize, int offset, int size)
	{
		if (size > maxSize - offset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("MTOM exceeded max size in bytes. The maximum size is {0}.", maxSize)));
		}
		return size;
	}

	private void WriteBase64InlineIfPresent()
	{
		if (binaryDataChunks != null)
		{
			WriteBase64Inline();
		}
	}

	private void WriteBase64Inline()
	{
		foreach (MtomBinaryData binaryDataChunk in binaryDataChunks)
		{
			if (binaryDataChunk.type == MtomBinaryDataType.Provider)
			{
				Writer.WriteValue(binaryDataChunk.provider);
			}
			else
			{
				Writer.WriteBase64(binaryDataChunk.chunk, 0, binaryDataChunk.chunk.Length);
			}
		}
		sizeOfBufferedBinaryData = 0;
		binaryDataChunks = null;
		contentType = null;
		contentID = null;
	}

	private void WriteXOPInclude()
	{
		if (binaryDataChunks == null)
		{
			return;
		}
		bool flag = true;
		long num = 0L;
		foreach (MtomBinaryData binaryDataChunk in binaryDataChunks)
		{
			long length = binaryDataChunk.Length;
			if (length < 0 || length > 767 - num)
			{
				flag = false;
				break;
			}
			num += length;
		}
		if (flag)
		{
			WriteBase64Inline();
			return;
		}
		if (mimeParts == null)
		{
			mimeParts = new List<MimePart>();
		}
		MimePart mimePart = new MimePart(binaryDataChunks, contentID, contentType, MimeGlobals.EncodingBinary, sizeOfBufferedBinaryData, maxSizeInBytes);
		mimeParts.Add(mimePart);
		totalSizeOfMimeParts += ValidateSizeOfMessage(maxSizeInBytes, totalSizeOfMimeParts, mimePart.sizeInBytes);
		totalSizeOfMimeParts += ValidateSizeOfMessage(maxSizeInBytes, totalSizeOfMimeParts, mimeWriter.GetBoundarySize());
		Writer.WriteStartElement(MtomGlobals.XopIncludePrefix, MtomGlobals.XopIncludeLocalName, MtomGlobals.XopIncludeNamespace);
		Writer.WriteStartAttribute(MtomGlobals.XopIncludeHrefLocalName, MtomGlobals.XopIncludeHrefNamespace);
		Writer.WriteValue(string.Format(CultureInfo.InvariantCulture, "{0}{1}", MimeGlobals.ContentIDScheme, contentID));
		Writer.WriteEndAttribute();
		Writer.WriteEndElement();
		binaryDataChunks = null;
		sizeOfBufferedBinaryData = 0;
		contentType = null;
		contentID = null;
	}

	public static string GenerateUriForMimePart(int index)
	{
		return string.Format(CultureInfo.InvariantCulture, "http://tempuri.org/{0}/{1}", index, DateTime.Now.Ticks);
	}

	private void WriteXOPBinaryParts()
	{
		if (depth > 0 || mimeWriter.WriteState == MimeWriterState.Closed)
		{
			return;
		}
		if (Writer.WriteState != WriteState.Closed)
		{
			Writer.Flush();
		}
		if (mimeParts != null)
		{
			foreach (MimePart mimePart in mimeParts)
			{
				WriteMimeHeaders(mimePart.contentID, mimePart.contentType, mimePart.contentTransferEncoding);
				Stream contentStream = mimeWriter.GetContentStream();
				int num = 256;
				int num2 = 0;
				byte[] buffer = new byte[num];
				Stream stream = null;
				foreach (MtomBinaryData binaryDatum in mimePart.binaryData)
				{
					if (binaryDatum.type == MtomBinaryDataType.Provider)
					{
						stream = binaryDatum.provider.GetStream();
						if (stream == null)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Stream returned by IStreamProvider cannot be null.")));
						}
						while (true)
						{
							num2 = stream.Read(buffer, 0, num);
							if (num2 <= 0)
							{
								break;
							}
							contentStream.Write(buffer, 0, num2);
							if (num < 65536 && num2 == num)
							{
								num *= 16;
								buffer = new byte[num];
							}
						}
						binaryDatum.provider.ReleaseStream(stream);
					}
					else
					{
						contentStream.Write(binaryDatum.chunk, 0, binaryDatum.chunk.Length);
					}
				}
			}
			mimeParts.Clear();
		}
		mimeWriter.Close();
	}

	private void WriteMimeHeaders(string contentID, string contentType, string contentTransferEncoding)
	{
		mimeWriter.StartPart();
		if (contentID != null)
		{
			mimeWriter.WriteHeader(MimeGlobals.ContentIDHeader, string.Format(CultureInfo.InvariantCulture, "<{0}>", contentID));
		}
		if (contentTransferEncoding != null)
		{
			mimeWriter.WriteHeader(MimeGlobals.ContentTransferEncodingHeader, contentTransferEncoding);
		}
		if (contentType != null)
		{
			mimeWriter.WriteHeader(MimeGlobals.ContentTypeHeader, contentType);
		}
	}

	public override void Close()
	{
		if (isClosed)
		{
			return;
		}
		isClosed = true;
		if (IsInitialized)
		{
			WriteXOPInclude();
			if (Writer.WriteState == WriteState.Element || Writer.WriteState == WriteState.Attribute || Writer.WriteState == WriteState.Content)
			{
				Writer.WriteEndDocument();
			}
			Writer.Flush();
			depth = 0;
			WriteXOPBinaryParts();
			Writer.Close();
		}
	}

	private void CheckIfStartContentTypeAttribute(string localName, string ns)
	{
		if (localName != null && localName == MtomGlobals.MimeContentTypeLocalName && ns != null && (ns == MtomGlobals.MimeContentTypeNamespace200406 || ns == MtomGlobals.MimeContentTypeNamespace200505))
		{
			contentTypeStream = new MemoryStream();
			infosetWriter = Writer;
			writer = XmlDictionaryWriter.CreateBinaryWriter(contentTypeStream);
			Writer.WriteStartElement("Wrapper");
			Writer.WriteStartAttribute(localName, ns);
		}
	}

	private void CheckIfEndContentTypeAttribute()
	{
		if (contentTypeStream == null)
		{
			return;
		}
		Writer.WriteEndAttribute();
		Writer.WriteEndElement();
		Writer.Flush();
		contentTypeStream.Position = 0L;
		XmlReader xmlReader = XmlDictionaryReader.CreateBinaryReader(contentTypeStream, null, XmlDictionaryReaderQuotas.Max, null, null);
		while (xmlReader.Read())
		{
			if (xmlReader.IsStartElement("Wrapper"))
			{
				contentType = xmlReader.GetAttribute(MtomGlobals.MimeContentTypeLocalName, MtomGlobals.MimeContentTypeNamespace200406);
				if (contentType == null)
				{
					contentType = xmlReader.GetAttribute(MtomGlobals.MimeContentTypeLocalName, MtomGlobals.MimeContentTypeNamespace200505);
				}
				break;
			}
		}
		writer = infosetWriter;
		infosetWriter = null;
		contentTypeStream = null;
		if (contentType != null)
		{
			Writer.WriteString(contentType);
		}
	}

	public override void Flush()
	{
		if (IsInitialized)
		{
			Writer.Flush();
		}
	}

	public override string LookupPrefix(string ns)
	{
		return Writer.LookupPrefix(ns);
	}

	public override void WriteAttributes(XmlReader reader, bool defattr)
	{
		Writer.WriteAttributes(reader, defattr);
	}

	public override void WriteBinHex(byte[] buffer, int index, int count)
	{
		WriteBase64InlineIfPresent();
		Writer.WriteBinHex(buffer, index, count);
	}

	public override void WriteCData(string text)
	{
		WriteBase64InlineIfPresent();
		Writer.WriteCData(text);
	}

	public override void WriteCharEntity(char ch)
	{
		WriteBase64InlineIfPresent();
		Writer.WriteCharEntity(ch);
	}

	public override void WriteChars(char[] buffer, int index, int count)
	{
		WriteBase64InlineIfPresent();
		Writer.WriteChars(buffer, index, count);
	}

	public override void WriteComment(string text)
	{
		if (depth != 0 || mimeWriter.WriteState != MimeWriterState.Closed)
		{
			WriteBase64InlineIfPresent();
			Writer.WriteComment(text);
		}
	}

	public override void WriteDocType(string name, string pubid, string sysid, string subset)
	{
		WriteBase64InlineIfPresent();
		Writer.WriteDocType(name, pubid, sysid, subset);
	}

	public override void WriteEndAttribute()
	{
		CheckIfEndContentTypeAttribute();
		Writer.WriteEndAttribute();
	}

	public override void WriteEndDocument()
	{
		WriteXOPInclude();
		Writer.WriteEndDocument();
		depth = 0;
		WriteXOPBinaryParts();
	}

	public override void WriteEntityRef(string name)
	{
		WriteBase64InlineIfPresent();
		Writer.WriteEntityRef(name);
	}

	public override void WriteName(string name)
	{
		WriteBase64InlineIfPresent();
		Writer.WriteName(name);
	}

	public override void WriteNmToken(string name)
	{
		WriteBase64InlineIfPresent();
		Writer.WriteNmToken(name);
	}

	protected override void WriteTextNode(XmlDictionaryReader reader, bool attribute)
	{
		Type valueType = reader.ValueType;
		if (valueType == typeof(string))
		{
			if (reader.CanReadValueChunk)
			{
				if (chars == null)
				{
					chars = new char[256];
				}
				int count;
				while ((count = reader.ReadValueChunk(chars, 0, chars.Length)) > 0)
				{
					WriteChars(chars, 0, count);
				}
			}
			else
			{
				WriteString(reader.Value);
			}
			if (!attribute)
			{
				reader.Read();
			}
		}
		else if (valueType == typeof(byte[]))
		{
			if (reader.CanReadBinaryContent)
			{
				if (bytes == null)
				{
					bytes = new byte[384];
				}
				int count2;
				while ((count2 = reader.ReadValueAsBase64(bytes, 0, bytes.Length)) > 0)
				{
					WriteBase64(bytes, 0, count2);
				}
			}
			else
			{
				WriteString(reader.Value);
			}
			if (!attribute)
			{
				reader.Read();
			}
		}
		else
		{
			base.WriteTextNode(reader, attribute);
		}
	}

	public override void WriteNode(XPathNavigator navigator, bool defattr)
	{
		WriteBase64InlineIfPresent();
		Writer.WriteNode(navigator, defattr);
	}

	public override void WriteProcessingInstruction(string name, string text)
	{
		WriteBase64InlineIfPresent();
		Writer.WriteProcessingInstruction(name, text);
	}

	public override void WriteQualifiedName(string localName, string namespaceUri)
	{
		WriteBase64InlineIfPresent();
		Writer.WriteQualifiedName(localName, namespaceUri);
	}

	public override void WriteRaw(char[] buffer, int index, int count)
	{
		WriteBase64InlineIfPresent();
		Writer.WriteRaw(buffer, index, count);
	}

	public override void WriteRaw(string data)
	{
		WriteBase64InlineIfPresent();
		Writer.WriteRaw(data);
	}

	public override void WriteStartAttribute(string prefix, string localName, string ns)
	{
		Writer.WriteStartAttribute(prefix, localName, ns);
		CheckIfStartContentTypeAttribute(localName, ns);
	}

	public override void WriteStartAttribute(string prefix, XmlDictionaryString localName, XmlDictionaryString ns)
	{
		Writer.WriteStartAttribute(prefix, localName, ns);
		if (localName != null && ns != null)
		{
			CheckIfStartContentTypeAttribute(localName.Value, ns.Value);
		}
	}

	public override void WriteStartDocument()
	{
		Writer.WriteStartDocument();
	}

	public override void WriteStartDocument(bool standalone)
	{
		Writer.WriteStartDocument(standalone);
	}

	public override void WriteString(string text)
	{
		if (depth != 0 || mimeWriter.WriteState != MimeWriterState.Closed || !XmlConverter.IsWhitespace(text))
		{
			WriteBase64InlineIfPresent();
			Writer.WriteString(text);
		}
	}

	public override void WriteString(XmlDictionaryString value)
	{
		if (depth != 0 || mimeWriter.WriteState != MimeWriterState.Closed || !XmlConverter.IsWhitespace(value.Value))
		{
			WriteBase64InlineIfPresent();
			Writer.WriteString(value);
		}
	}

	public override void WriteSurrogateCharEntity(char lowChar, char highChar)
	{
		WriteBase64InlineIfPresent();
		Writer.WriteSurrogateCharEntity(lowChar, highChar);
	}

	public override void WriteWhitespace(string whitespace)
	{
		if (depth != 0 || mimeWriter.WriteState != MimeWriterState.Closed)
		{
			WriteBase64InlineIfPresent();
			Writer.WriteWhitespace(whitespace);
		}
	}

	public override void WriteValue(object value)
	{
		if (value is IStreamProvider value2)
		{
			WriteValue(value2);
			return;
		}
		WriteBase64InlineIfPresent();
		Writer.WriteValue(value);
	}

	public override void WriteValue(string value)
	{
		if (depth != 0 || mimeWriter.WriteState != MimeWriterState.Closed || !XmlConverter.IsWhitespace(value))
		{
			WriteBase64InlineIfPresent();
			Writer.WriteValue(value);
		}
	}

	public override void WriteValue(bool value)
	{
		WriteBase64InlineIfPresent();
		Writer.WriteValue(value);
	}

	public override void WriteValue(DateTime value)
	{
		WriteBase64InlineIfPresent();
		Writer.WriteValue(value);
	}

	public override void WriteValue(double value)
	{
		WriteBase64InlineIfPresent();
		Writer.WriteValue(value);
	}

	public override void WriteValue(int value)
	{
		WriteBase64InlineIfPresent();
		Writer.WriteValue(value);
	}

	public override void WriteValue(long value)
	{
		WriteBase64InlineIfPresent();
		Writer.WriteValue(value);
	}

	public override void WriteValue(XmlDictionaryString value)
	{
		if (depth != 0 || mimeWriter.WriteState != MimeWriterState.Closed || !XmlConverter.IsWhitespace(value.Value))
		{
			WriteBase64InlineIfPresent();
			Writer.WriteValue(value);
		}
	}

	public override void WriteXmlnsAttribute(string prefix, string ns)
	{
		Writer.WriteXmlnsAttribute(prefix, ns);
	}

	public override void WriteXmlnsAttribute(string prefix, XmlDictionaryString ns)
	{
		Writer.WriteXmlnsAttribute(prefix, ns);
	}
}
