using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime;
using System.Runtime.Serialization;
using System.Text;

namespace System.Xml;

internal class XmlMtomReader : XmlDictionaryReader, IXmlLineInfo, IXmlMtomReaderInitializer
{
	internal class MimePart
	{
		private Stream stream;

		private MimeHeaders headers;

		private byte[] buffer;

		private bool isReferencedFromInfoset;

		internal Stream Stream => stream;

		internal MimeHeaders Headers => headers;

		internal bool ReferencedFromInfoset
		{
			get
			{
				return isReferencedFromInfoset;
			}
			set
			{
				isReferencedFromInfoset = value;
			}
		}

		internal long Length
		{
			get
			{
				if (!stream.CanSeek)
				{
					return 0L;
				}
				return stream.Length;
			}
		}

		internal MimePart(Stream stream, MimeHeaders headers)
		{
			this.stream = stream;
			this.headers = headers;
		}

		internal byte[] GetBuffer(int maxBuffer, ref int remaining)
		{
			if (buffer == null)
			{
				MemoryStream memoryStream = (stream.CanSeek ? new MemoryStream((int)stream.Length) : new MemoryStream());
				int num = 256;
				byte[] array = new byte[num];
				int num2 = 0;
				do
				{
					num2 = stream.Read(array, 0, num);
					DecrementBufferQuota(maxBuffer, ref remaining, num2);
					if (num2 > 0)
					{
						memoryStream.Write(array, 0, num2);
					}
				}
				while (num2 > 0);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				buffer = memoryStream.GetBuffer();
				stream = memoryStream;
			}
			return buffer;
		}

		internal void Release(int maxBuffer, ref int remaining)
		{
			remaining += (int)Length;
			headers.Release(ref remaining);
		}
	}

	internal class XopIncludeReader : XmlDictionaryReader, IXmlLineInfo
	{
		private int chunkSize = 4096;

		private int bytesRemaining;

		private MimePart part;

		private ReadState readState;

		private XmlDictionaryReader parentReader;

		private string stringValue;

		private int stringOffset;

		private XmlNodeType nodeType;

		private MemoryStream binHexStream;

		private byte[] valueBuffer;

		private int valueOffset;

		private int valueCount;

		private bool finishedStream;

		public override XmlDictionaryReaderQuotas Quotas => parentReader.Quotas;

		public override XmlNodeType NodeType
		{
			get
			{
				if (readState != ReadState.Interactive)
				{
					return parentReader.NodeType;
				}
				return nodeType;
			}
		}

		public override string Value
		{
			get
			{
				if (readState != ReadState.Interactive)
				{
					return string.Empty;
				}
				if (stringValue == null)
				{
					int num = bytesRemaining;
					num -= num % 3;
					if (valueCount > 0 && valueOffset > 0)
					{
						Buffer.BlockCopy(valueBuffer, valueOffset, valueBuffer, 0, valueCount);
						valueOffset = 0;
					}
					num -= valueCount;
					if (valueBuffer == null)
					{
						valueBuffer = new byte[num];
					}
					else if (valueBuffer.Length < num)
					{
						Array.Resize(ref valueBuffer, num);
					}
					byte[] array = valueBuffer;
					int num2 = 0;
					int num3 = 0;
					while (num > 0)
					{
						num3 = part.Stream.Read(array, num2, num);
						if (num3 == 0)
						{
							finishedStream = true;
							break;
						}
						bytesRemaining -= num3;
						valueCount += num3;
						num -= num3;
						num2 += num3;
					}
					stringValue = Convert.ToBase64String(array, 0, valueCount);
				}
				return stringValue;
			}
		}

		public override int AttributeCount => 0;

		public override string BaseURI => parentReader.BaseURI;

		public override bool CanReadBinaryContent => true;

		public override bool CanReadValueChunk => true;

		public override bool CanResolveEntity => parentReader.CanResolveEntity;

		public override int Depth
		{
			get
			{
				if (readState != ReadState.Interactive)
				{
					return parentReader.Depth;
				}
				return parentReader.Depth + 1;
			}
		}

		public override bool EOF => readState == ReadState.EndOfFile;

		public override bool HasAttributes => false;

		public override bool HasValue => readState == ReadState.Interactive;

		public override bool IsDefault => false;

		public override bool IsEmptyElement => false;

		public override string LocalName
		{
			get
			{
				if (readState != ReadState.Interactive)
				{
					return parentReader.LocalName;
				}
				return string.Empty;
			}
		}

		public override string Name
		{
			get
			{
				if (readState != ReadState.Interactive)
				{
					return parentReader.Name;
				}
				return string.Empty;
			}
		}

		public override string NamespaceURI
		{
			get
			{
				if (readState != ReadState.Interactive)
				{
					return parentReader.NamespaceURI;
				}
				return string.Empty;
			}
		}

		public override XmlNameTable NameTable => parentReader.NameTable;

		public override string Prefix
		{
			get
			{
				if (readState != ReadState.Interactive)
				{
					return parentReader.Prefix;
				}
				return string.Empty;
			}
		}

		public override char QuoteChar => parentReader.QuoteChar;

		public override ReadState ReadState => readState;

		public override XmlReaderSettings Settings => parentReader.Settings;

		public override string this[int index] => null;

		public override string this[string name] => null;

		public override string this[string name, string ns] => null;

		public override string XmlLang => parentReader.XmlLang;

		public override XmlSpace XmlSpace => parentReader.XmlSpace;

		public override Type ValueType
		{
			get
			{
				if (readState != ReadState.Interactive)
				{
					return parentReader.ValueType;
				}
				return typeof(byte[]);
			}
		}

		int IXmlLineInfo.LineNumber => ((IXmlLineInfo)parentReader).LineNumber;

		int IXmlLineInfo.LinePosition => ((IXmlLineInfo)parentReader).LinePosition;

		public XopIncludeReader(MimePart part, XmlDictionaryReader reader)
		{
			if (part == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("part");
			}
			if (reader == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("reader");
			}
			this.part = part;
			parentReader = reader;
			readState = ReadState.Initial;
			nodeType = XmlNodeType.None;
			chunkSize = Math.Min(reader.Quotas.MaxBytesPerRead, chunkSize);
			bytesRemaining = chunkSize;
			finishedStream = false;
		}

		public override bool Read()
		{
			bool result = true;
			switch (readState)
			{
			case ReadState.Initial:
				readState = ReadState.Interactive;
				nodeType = XmlNodeType.Text;
				break;
			case ReadState.Interactive:
				if (finishedStream || (bytesRemaining == chunkSize && stringValue == null))
				{
					readState = ReadState.EndOfFile;
					nodeType = XmlNodeType.EndElement;
				}
				else
				{
					bytesRemaining = chunkSize;
				}
				break;
			case ReadState.EndOfFile:
				nodeType = XmlNodeType.None;
				result = false;
				break;
			}
			stringValue = null;
			binHexStream = null;
			valueOffset = 0;
			valueCount = 0;
			stringOffset = 0;
			CloseStreams();
			return result;
		}

		public override int ReadValueAsBase64(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
			}
			if (offset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (offset > buffer.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", buffer.Length)));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > buffer.Length - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", buffer.Length - offset)));
			}
			if (stringValue != null)
			{
				count = Math.Min(count, valueCount);
				if (count > 0)
				{
					Buffer.BlockCopy(valueBuffer, valueOffset, buffer, offset, count);
					valueOffset += count;
					valueCount -= count;
				}
				return count;
			}
			if (bytesRemaining < count)
			{
				count = bytesRemaining;
			}
			int i = 0;
			if (readState == ReadState.Interactive)
			{
				int num;
				for (; i < count; i += num)
				{
					num = part.Stream.Read(buffer, offset + i, count - i);
					if (num == 0)
					{
						finishedStream = true;
						break;
					}
				}
			}
			bytesRemaining -= i;
			return i;
		}

		public override int ReadContentAsBase64(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
			}
			if (offset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (offset > buffer.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", buffer.Length)));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > buffer.Length - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", buffer.Length - offset)));
			}
			if (valueCount > 0)
			{
				count = Math.Min(count, valueCount);
				Buffer.BlockCopy(valueBuffer, valueOffset, buffer, offset, count);
				valueOffset += count;
				valueCount -= count;
				return count;
			}
			if (chunkSize < count)
			{
				count = chunkSize;
			}
			int i = 0;
			if (readState == ReadState.Interactive)
			{
				int num;
				for (; i < count; i += num)
				{
					num = part.Stream.Read(buffer, offset + i, count - i);
					if (num == 0)
					{
						finishedStream = true;
						if (!Read())
						{
							break;
						}
					}
				}
			}
			bytesRemaining = chunkSize;
			return i;
		}

		public override int ReadContentAsBinHex(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
			}
			if (offset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (offset > buffer.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", buffer.Length)));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > buffer.Length - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", buffer.Length - offset)));
			}
			if (chunkSize < count)
			{
				count = chunkSize;
			}
			int num = 0;
			int num2 = 0;
			while (num < count)
			{
				if (binHexStream == null)
				{
					try
					{
						binHexStream = new MemoryStream(new BinHexEncoding().GetBytes(Value));
					}
					catch (FormatException ex)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(ex.Message, ex));
					}
				}
				int num3 = binHexStream.Read(buffer, offset + num, count - num);
				if (num3 == 0)
				{
					finishedStream = true;
					if (!Read())
					{
						break;
					}
					num2 = 0;
				}
				num += num3;
				num2 += num3;
			}
			if (stringValue != null && num2 > 0)
			{
				stringValue = stringValue.Substring(num2 * 2);
				stringOffset = Math.Max(0, stringOffset - num2 * 2);
				bytesRemaining = chunkSize;
			}
			return num;
		}

		public override int ReadValueChunk(char[] chars, int offset, int count)
		{
			if (chars == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("chars");
			}
			if (offset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (offset > chars.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", chars.Length)));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > chars.Length - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", chars.Length - offset)));
			}
			if (readState != ReadState.Interactive)
			{
				return 0;
			}
			_ = Value;
			count = Math.Min(stringValue.Length - stringOffset, count);
			if (count > 0)
			{
				stringValue.CopyTo(stringOffset, chars, offset, count);
				stringOffset += count;
			}
			return count;
		}

		public override string ReadContentAsString()
		{
			int num = Quotas.MaxStringContentLength;
			StringBuilder stringBuilder = new StringBuilder();
			do
			{
				string value = Value;
				if (value.Length > num)
				{
					XmlExceptionHelper.ThrowMaxStringContentLengthExceeded(this, Quotas.MaxStringContentLength);
				}
				num -= value.Length;
				stringBuilder.Append(value);
			}
			while (Read());
			return stringBuilder.ToString();
		}

		public override void Close()
		{
			CloseStreams();
			readState = ReadState.Closed;
		}

		private void CloseStreams()
		{
			if (binHexStream != null)
			{
				binHexStream.Close();
				binHexStream = null;
			}
		}

		public override string GetAttribute(int index)
		{
			return null;
		}

		public override string GetAttribute(string name)
		{
			return null;
		}

		public override string GetAttribute(string name, string ns)
		{
			return null;
		}

		public override string GetAttribute(XmlDictionaryString localName, XmlDictionaryString ns)
		{
			return null;
		}

		public override bool IsLocalName(string localName)
		{
			return false;
		}

		public override bool IsLocalName(XmlDictionaryString localName)
		{
			return false;
		}

		public override bool IsNamespaceUri(string ns)
		{
			return false;
		}

		public override bool IsNamespaceUri(XmlDictionaryString ns)
		{
			return false;
		}

		public override bool IsStartElement()
		{
			return false;
		}

		public override bool IsStartElement(string localName)
		{
			return false;
		}

		public override bool IsStartElement(string localName, string ns)
		{
			return false;
		}

		public override bool IsStartElement(XmlDictionaryString localName, XmlDictionaryString ns)
		{
			return false;
		}

		public override string LookupNamespace(string ns)
		{
			return parentReader.LookupNamespace(ns);
		}

		public override void MoveToAttribute(int index)
		{
		}

		public override bool MoveToAttribute(string name)
		{
			return false;
		}

		public override bool MoveToAttribute(string name, string ns)
		{
			return false;
		}

		public override bool MoveToElement()
		{
			return false;
		}

		public override bool MoveToFirstAttribute()
		{
			return false;
		}

		public override bool MoveToNextAttribute()
		{
			return false;
		}

		public override bool ReadAttributeValue()
		{
			return false;
		}

		public override string ReadInnerXml()
		{
			return ReadContentAsString();
		}

		public override string ReadOuterXml()
		{
			return ReadContentAsString();
		}

		public override void ResolveEntity()
		{
		}

		public override void Skip()
		{
			Read();
		}

		bool IXmlLineInfo.HasLineInfo()
		{
			return ((IXmlLineInfo)parentReader).HasLineInfo();
		}
	}

	private Encoding[] encodings;

	private XmlDictionaryReader xmlReader;

	private XmlDictionaryReader infosetReader;

	private MimeReader mimeReader;

	private Dictionary<string, MimePart> mimeParts;

	private OnXmlDictionaryReaderClose onClose;

	private bool readingBinaryElement;

	private int maxBufferSize;

	private int bufferRemaining;

	private MimePart part;

	public override XmlDictionaryReaderQuotas Quotas => xmlReader.Quotas;

	public override int AttributeCount => xmlReader.AttributeCount;

	public override string BaseURI => xmlReader.BaseURI;

	public override bool CanReadBinaryContent => xmlReader.CanReadBinaryContent;

	public override bool CanReadValueChunk => xmlReader.CanReadValueChunk;

	public override bool CanResolveEntity => xmlReader.CanResolveEntity;

	public override int Depth => xmlReader.Depth;

	public override bool EOF => xmlReader.EOF;

	public override bool HasAttributes => xmlReader.HasAttributes;

	public override bool HasValue => xmlReader.HasValue;

	public override bool IsDefault => xmlReader.IsDefault;

	public override bool IsEmptyElement => xmlReader.IsEmptyElement;

	public override string LocalName => xmlReader.LocalName;

	public override string Name => xmlReader.Name;

	public override string NamespaceURI => xmlReader.NamespaceURI;

	public override XmlNameTable NameTable => xmlReader.NameTable;

	public override XmlNodeType NodeType => xmlReader.NodeType;

	public override string Prefix => xmlReader.Prefix;

	public override char QuoteChar => xmlReader.QuoteChar;

	public override ReadState ReadState
	{
		get
		{
			if (xmlReader.ReadState != ReadState.Interactive && infosetReader != null)
			{
				return infosetReader.ReadState;
			}
			return xmlReader.ReadState;
		}
	}

	public override XmlReaderSettings Settings => xmlReader.Settings;

	public override string this[int index] => xmlReader[index];

	public override string this[string name] => xmlReader[name];

	public override string this[string name, string ns] => xmlReader[name, ns];

	public override string Value => xmlReader.Value;

	public override Type ValueType => xmlReader.ValueType;

	public override string XmlLang => xmlReader.XmlLang;

	public override XmlSpace XmlSpace => xmlReader.XmlSpace;

	public int LineNumber
	{
		get
		{
			if (xmlReader.ReadState == ReadState.Closed)
			{
				return 0;
			}
			if (!(xmlReader is IXmlLineInfo xmlLineInfo))
			{
				return 0;
			}
			return xmlLineInfo.LineNumber;
		}
	}

	public int LinePosition
	{
		get
		{
			if (xmlReader.ReadState == ReadState.Closed)
			{
				return 0;
			}
			if (!(xmlReader is IXmlLineInfo xmlLineInfo))
			{
				return 0;
			}
			return xmlLineInfo.LinePosition;
		}
	}

	internal static void DecrementBufferQuota(int maxBuffer, ref int remaining, int size)
	{
		if (remaining - size <= 0)
		{
			remaining = 0;
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("MTOM buffer quota exceeded. The maximum size is {0}.", maxBuffer)));
		}
		remaining -= size;
	}

	private void SetReadEncodings(Encoding[] encodings)
	{
		if (encodings == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("encodings");
		}
		for (int i = 0; i < encodings.Length; i++)
		{
			if (encodings[i] == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(string.Format(CultureInfo.InvariantCulture, "encodings[{0}]", i));
			}
		}
		this.encodings = new Encoding[encodings.Length];
		encodings.CopyTo(this.encodings, 0);
	}

	private void CheckContentType(string contentType)
	{
		if (contentType != null && contentType.Length == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("MTOM content type is invalid."), "contentType"));
		}
	}

	public void SetInput(byte[] buffer, int offset, int count, Encoding[] encodings, string contentType, XmlDictionaryReaderQuotas quotas, int maxBufferSize, OnXmlDictionaryReaderClose onClose)
	{
		SetInput(new MemoryStream(buffer, offset, count), encodings, contentType, quotas, maxBufferSize, onClose);
	}

	public void SetInput(Stream stream, Encoding[] encodings, string contentType, XmlDictionaryReaderQuotas quotas, int maxBufferSize, OnXmlDictionaryReaderClose onClose)
	{
		SetReadEncodings(encodings);
		CheckContentType(contentType);
		Initialize(stream, contentType, quotas, maxBufferSize);
		this.onClose = onClose;
	}

	private void Initialize(Stream stream, string contentType, XmlDictionaryReaderQuotas quotas, int maxBufferSize)
	{
		if (stream == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("stream");
		}
		this.maxBufferSize = maxBufferSize;
		bufferRemaining = maxBufferSize;
		string boundary;
		string start;
		string startInfo;
		if (contentType == null)
		{
			MimeMessageReader mimeMessageReader = new MimeMessageReader(stream);
			MimeHeaders mimeHeaders = mimeMessageReader.ReadHeaders(this.maxBufferSize, ref bufferRemaining);
			ReadMessageMimeVersionHeader(mimeHeaders.MimeVersion);
			ReadMessageContentTypeHeader(mimeHeaders.ContentType, out boundary, out start, out startInfo);
			stream = mimeMessageReader.GetContentStream();
			if (stream == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("MTOM message content is invalid.")));
			}
		}
		else
		{
			ReadMessageContentTypeHeader(new ContentTypeHeader(contentType), out boundary, out start, out startInfo);
		}
		mimeReader = new MimeReader(stream, boundary);
		mimeParts = null;
		readingBinaryElement = false;
		MimePart mimePart = ((start == null) ? ReadRootMimePart() : ReadMimePart(GetStartUri(start)));
		byte[] buffer = mimePart.GetBuffer(this.maxBufferSize, ref bufferRemaining);
		int count = (int)mimePart.Length;
		Encoding encoding = ReadRootContentTypeHeader(mimePart.Headers.ContentType, encodings, startInfo);
		CheckContentTransferEncodingOnRoot(mimePart.Headers.ContentTransferEncoding);
		if (xmlReader is IXmlTextReaderInitializer xmlTextReaderInitializer)
		{
			xmlTextReaderInitializer.SetInput(buffer, 0, count, encoding, quotas, null);
		}
		else
		{
			xmlReader = XmlDictionaryReader.CreateTextReader(buffer, 0, count, encoding, quotas, null);
		}
	}

	private void ReadMessageMimeVersionHeader(MimeVersionHeader header)
	{
		if (header != null && header.Version != MimeVersionHeader.Default.Version)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("MTOM message has invalid MIME version. Expected '{1}', got '{0}' instead.", header.Version, MimeVersionHeader.Default.Version)));
		}
	}

	private void ReadMessageContentTypeHeader(ContentTypeHeader header, out string boundary, out string start, out string startInfo)
	{
		if (header == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("MTOM message content type was not found.")));
		}
		if (string.Compare(MtomGlobals.MediaType, header.MediaType, StringComparison.OrdinalIgnoreCase) != 0 || string.Compare(MtomGlobals.MediaSubtype, header.MediaSubtype, StringComparison.OrdinalIgnoreCase) != 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("MTOM message is not multipart: media type should be '{0}', media subtype should be '{1}'.", MtomGlobals.MediaType, MtomGlobals.MediaSubtype)));
		}
		if (!header.Parameters.TryGetValue(MtomGlobals.TypeParam, out var value) || MtomGlobals.XopType != value)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("MTOM msssage type is not '{0}'.", MtomGlobals.XopType)));
		}
		if (!header.Parameters.TryGetValue(MtomGlobals.BoundaryParam, out boundary))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Required MTOM parameter '{0}' is not specified.", MtomGlobals.BoundaryParam)));
		}
		if (!MailBnfHelper.IsValidMimeBoundary(boundary))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("MIME boundary is invalid: '{0}'.", boundary)));
		}
		if (!header.Parameters.TryGetValue(MtomGlobals.StartParam, out start))
		{
			start = null;
		}
		if (!header.Parameters.TryGetValue(MtomGlobals.StartInfoParam, out startInfo))
		{
			startInfo = null;
		}
	}

	private Encoding ReadRootContentTypeHeader(ContentTypeHeader header, Encoding[] expectedEncodings, string expectedType)
	{
		if (header == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("MTOM root content type is not found.")));
		}
		if (string.Compare(MtomGlobals.XopMediaType, header.MediaType, StringComparison.OrdinalIgnoreCase) != 0 || string.Compare(MtomGlobals.XopMediaSubtype, header.MediaSubtype, StringComparison.OrdinalIgnoreCase) != 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("MTOM root should have media type '{0}' and subtype '{1}'.", MtomGlobals.XopMediaType, MtomGlobals.XopMediaSubtype)));
		}
		if (!header.Parameters.TryGetValue(MtomGlobals.CharsetParam, out var value) || value == null || value.Length == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Required MTOM root parameter '{0}' is not specified.", MtomGlobals.CharsetParam)));
		}
		Encoding encoding = null;
		for (int i = 0; i < encodings.Length; i++)
		{
			if (string.Compare(value, expectedEncodings[i].WebName, StringComparison.OrdinalIgnoreCase) == 0)
			{
				encoding = expectedEncodings[i];
				break;
			}
		}
		if (encoding == null)
		{
			if (string.Compare(value, "utf-16LE", StringComparison.OrdinalIgnoreCase) == 0)
			{
				for (int j = 0; j < encodings.Length; j++)
				{
					if (string.Compare(expectedEncodings[j].WebName, Encoding.Unicode.WebName, StringComparison.OrdinalIgnoreCase) == 0)
					{
						encoding = expectedEncodings[j];
						break;
					}
				}
			}
			else if (string.Compare(value, "utf-16BE", StringComparison.OrdinalIgnoreCase) == 0)
			{
				for (int k = 0; k < encodings.Length; k++)
				{
					if (string.Compare(expectedEncodings[k].WebName, Encoding.BigEndianUnicode.WebName, StringComparison.OrdinalIgnoreCase) == 0)
					{
						encoding = expectedEncodings[k];
						break;
					}
				}
			}
			if (encoding == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int l = 0; l < encodings.Length; l++)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(" | ");
					}
					stringBuilder.Append(encodings[l].WebName);
				}
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Unexpected charset on MTOM root. Expected '{1}', got '{0}' instead.", value, stringBuilder.ToString())));
			}
		}
		if (expectedType != null)
		{
			if (!header.Parameters.TryGetValue(MtomGlobals.TypeParam, out var value2) || value2 == null || value2.Length == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Required MTOM root parameter '{0}' is not specified.", MtomGlobals.TypeParam)));
			}
			if (value2 != expectedType)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Unexpected type on MTOM root. Expected '{1}', got '{0}' instead.", value2, expectedType)));
			}
		}
		return encoding;
	}

	private void CheckContentTransferEncodingOnRoot(ContentTransferEncodingHeader header)
	{
		if (header != null && header.ContentTransferEncoding == ContentTransferEncoding.Other)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("MTOM content transfer encoding value is not supported. Raw value is '{0}', '{1}' in 7bit encoding, '{2}' in 8bit encoding, and '{3}' in binary.", header.Value, ContentTransferEncodingHeader.SevenBit.ContentTransferEncodingValue, ContentTransferEncodingHeader.EightBit.ContentTransferEncodingValue, ContentTransferEncodingHeader.Binary.ContentTransferEncodingValue)));
		}
	}

	private void CheckContentTransferEncodingOnBinaryPart(ContentTransferEncodingHeader header)
	{
		if (header == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("MTOM content transfer encoding is not present. ContentTransferEncoding header is '{0}'.", ContentTransferEncodingHeader.Binary.ContentTransferEncodingValue)));
		}
		if (header.ContentTransferEncoding != ContentTransferEncoding.Binary)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid transfer encoding for MIME part: '{0}', in binary: '{1}'.", header.Value, ContentTransferEncodingHeader.Binary.ContentTransferEncodingValue)));
		}
	}

	private string GetStartUri(string startUri)
	{
		if (startUri.StartsWith("<", StringComparison.Ordinal))
		{
			if (startUri.EndsWith(">", StringComparison.Ordinal))
			{
				return startUri;
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid MTOM start URI: '{0}'.", startUri)));
		}
		return string.Format(CultureInfo.InvariantCulture, "<{0}>", startUri);
	}

	public override bool Read()
	{
		bool flag = xmlReader.Read();
		if (xmlReader.NodeType == XmlNodeType.Element)
		{
			XopIncludeReader xopIncludeReader = null;
			if (xmlReader.IsStartElement(MtomGlobals.XopIncludeLocalName, MtomGlobals.XopIncludeNamespace))
			{
				string text = null;
				while (xmlReader.MoveToNextAttribute())
				{
					if (xmlReader.LocalName == MtomGlobals.XopIncludeHrefLocalName && xmlReader.NamespaceURI == MtomGlobals.XopIncludeHrefNamespace)
					{
						text = xmlReader.Value;
					}
					else if (xmlReader.NamespaceURI == MtomGlobals.XopIncludeNamespace)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("xop Include element has invalid attribute: '{0}' in '{1}' namespace.", xmlReader.LocalName, MtomGlobals.XopIncludeNamespace)));
					}
				}
				if (text == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("xop Include element did not specify '{0}' attribute.", MtomGlobals.XopIncludeHrefLocalName)));
				}
				MimePart mimePart = ReadMimePart(text);
				CheckContentTransferEncodingOnBinaryPart(mimePart.Headers.ContentTransferEncoding);
				part = mimePart;
				xopIncludeReader = new XopIncludeReader(mimePart, xmlReader);
				xopIncludeReader.Read();
				xmlReader.MoveToElement();
				if (xmlReader.IsEmptyElement)
				{
					xmlReader.Read();
				}
				else
				{
					int depth = xmlReader.Depth;
					xmlReader.ReadStartElement();
					while (xmlReader.Depth > depth)
					{
						if (xmlReader.IsStartElement() && xmlReader.NamespaceURI == MtomGlobals.XopIncludeNamespace)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("xop Include element has invalid element: '{0}' in '{1}' namespace.", xmlReader.LocalName, MtomGlobals.XopIncludeNamespace)));
						}
						xmlReader.Skip();
					}
					xmlReader.ReadEndElement();
				}
			}
			if (xopIncludeReader != null)
			{
				xmlReader.MoveToContent();
				infosetReader = xmlReader;
				xmlReader = xopIncludeReader;
				xopIncludeReader = null;
			}
		}
		if (xmlReader.ReadState == ReadState.EndOfFile && infosetReader != null)
		{
			if (!flag)
			{
				flag = infosetReader.Read();
			}
			part.Release(maxBufferSize, ref bufferRemaining);
			xmlReader = infosetReader;
			infosetReader = null;
		}
		return flag;
	}

	private MimePart ReadMimePart(string uri)
	{
		MimePart value = null;
		if (uri == null || uri.Length == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("empty URI is invalid for MTOM MIME part.")));
		}
		string text = null;
		if (uri.StartsWith(MimeGlobals.ContentIDScheme, StringComparison.Ordinal))
		{
			text = string.Format(CultureInfo.InvariantCulture, "<{0}>", Uri.UnescapeDataString(uri.Substring(MimeGlobals.ContentIDScheme.Length)));
		}
		else if (uri.StartsWith("<", StringComparison.Ordinal))
		{
			text = uri;
		}
		if (text == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Invalid MTOM CID URI: '{0}'.", uri)));
		}
		if (mimeParts != null && mimeParts.TryGetValue(text, out value))
		{
			if (value.ReferencedFromInfoset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Specified MIME part '{0}' is referenced more than once.", text)));
			}
		}
		else
		{
			int maxMimeParts = AppSettings.MaxMimeParts;
			while (value == null && mimeReader.ReadNextPart())
			{
				MimeHeaders mimeHeaders = mimeReader.ReadHeaders(maxBufferSize, ref bufferRemaining);
				Stream contentStream = mimeReader.GetContentStream();
				if (contentStream == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("MTOM message content in MIME part is invalid.")));
				}
				ContentIDHeader contentIDHeader = mimeHeaders?.ContentID;
				if (contentIDHeader == null || contentIDHeader.Value == null)
				{
					int num = 256;
					byte[] buffer = new byte[num];
					int num2 = 0;
					do
					{
						num2 = contentStream.Read(buffer, 0, num);
					}
					while (num2 > 0);
					continue;
				}
				string value2 = mimeHeaders.ContentID.Value;
				MimePart mimePart = new MimePart(contentStream, mimeHeaders);
				if (mimeParts == null)
				{
					mimeParts = new Dictionary<string, MimePart>();
				}
				mimeParts.Add(value2, mimePart);
				if (mimeParts.Count > maxMimeParts)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("MIME parts number exceeded the maximum settings. Must be less than {0}. Specified as '{1}'.", maxMimeParts, "microsoft:xmldictionaryreader:maxmimeparts")));
				}
				if (value2.Equals(text))
				{
					value = mimePart;
				}
				else
				{
					mimePart.GetBuffer(maxBufferSize, ref bufferRemaining);
				}
			}
			if (value == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("MTOM part with URI '{0}' is not found.", uri)));
			}
		}
		value.ReferencedFromInfoset = true;
		return value;
	}

	private MimePart ReadRootMimePart()
	{
		if (!mimeReader.ReadNextPart())
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("MTOM root part is not found.")));
		}
		MimeHeaders headers = mimeReader.ReadHeaders(maxBufferSize, ref bufferRemaining);
		return new MimePart(mimeReader.GetContentStream() ?? throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("MTOM message content in MIME part is invalid."))), headers);
	}

	private void AdvanceToContentOnElement()
	{
		if (NodeType != XmlNodeType.Attribute)
		{
			MoveToContent();
		}
	}

	public override void Close()
	{
		xmlReader.Close();
		mimeReader.Close();
		OnXmlDictionaryReaderClose onXmlDictionaryReaderClose = onClose;
		onClose = null;
		if (onXmlDictionaryReaderClose == null)
		{
			return;
		}
		try
		{
			onXmlDictionaryReaderClose(this);
		}
		catch (Exception ex)
		{
			if (Fx.IsFatal(ex))
			{
				throw;
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperCallback(ex);
		}
	}

	public override string GetAttribute(int index)
	{
		return xmlReader.GetAttribute(index);
	}

	public override string GetAttribute(string name)
	{
		return xmlReader.GetAttribute(name);
	}

	public override string GetAttribute(string name, string ns)
	{
		return xmlReader.GetAttribute(name, ns);
	}

	public override string GetAttribute(XmlDictionaryString localName, XmlDictionaryString ns)
	{
		return xmlReader.GetAttribute(localName, ns);
	}

	public override bool IsLocalName(string localName)
	{
		return xmlReader.IsLocalName(localName);
	}

	public override bool IsLocalName(XmlDictionaryString localName)
	{
		return xmlReader.IsLocalName(localName);
	}

	public override bool IsNamespaceUri(string ns)
	{
		return xmlReader.IsNamespaceUri(ns);
	}

	public override bool IsNamespaceUri(XmlDictionaryString ns)
	{
		return xmlReader.IsNamespaceUri(ns);
	}

	public override bool IsStartElement()
	{
		return xmlReader.IsStartElement();
	}

	public override bool IsStartElement(string localName)
	{
		return xmlReader.IsStartElement(localName);
	}

	public override bool IsStartElement(string localName, string ns)
	{
		return xmlReader.IsStartElement(localName, ns);
	}

	public override bool IsStartElement(XmlDictionaryString localName, XmlDictionaryString ns)
	{
		return xmlReader.IsStartElement(localName, ns);
	}

	public override string LookupNamespace(string ns)
	{
		return xmlReader.LookupNamespace(ns);
	}

	public override void MoveToAttribute(int index)
	{
		xmlReader.MoveToAttribute(index);
	}

	public override bool MoveToAttribute(string name)
	{
		return xmlReader.MoveToAttribute(name);
	}

	public override bool MoveToAttribute(string name, string ns)
	{
		return xmlReader.MoveToAttribute(name, ns);
	}

	public override bool MoveToElement()
	{
		return xmlReader.MoveToElement();
	}

	public override bool MoveToFirstAttribute()
	{
		return xmlReader.MoveToFirstAttribute();
	}

	public override bool MoveToNextAttribute()
	{
		return xmlReader.MoveToNextAttribute();
	}

	public override bool ReadAttributeValue()
	{
		return xmlReader.ReadAttributeValue();
	}

	public override object ReadContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
	{
		AdvanceToContentOnElement();
		return xmlReader.ReadContentAs(returnType, namespaceResolver);
	}

	public override byte[] ReadContentAsBase64()
	{
		AdvanceToContentOnElement();
		return xmlReader.ReadContentAsBase64();
	}

	public override int ReadValueAsBase64(byte[] buffer, int offset, int count)
	{
		AdvanceToContentOnElement();
		return xmlReader.ReadValueAsBase64(buffer, offset, count);
	}

	public override int ReadContentAsBase64(byte[] buffer, int offset, int count)
	{
		AdvanceToContentOnElement();
		return xmlReader.ReadContentAsBase64(buffer, offset, count);
	}

	public override int ReadElementContentAsBase64(byte[] buffer, int offset, int count)
	{
		if (!readingBinaryElement)
		{
			if (IsEmptyElement)
			{
				Read();
				return 0;
			}
			ReadStartElement();
			readingBinaryElement = true;
		}
		int num = ReadContentAsBase64(buffer, offset, count);
		if (num == 0)
		{
			ReadEndElement();
			readingBinaryElement = false;
		}
		return num;
	}

	public override int ReadElementContentAsBinHex(byte[] buffer, int offset, int count)
	{
		if (!readingBinaryElement)
		{
			if (IsEmptyElement)
			{
				Read();
				return 0;
			}
			ReadStartElement();
			readingBinaryElement = true;
		}
		int num = ReadContentAsBinHex(buffer, offset, count);
		if (num == 0)
		{
			ReadEndElement();
			readingBinaryElement = false;
		}
		return num;
	}

	public override int ReadContentAsBinHex(byte[] buffer, int offset, int count)
	{
		AdvanceToContentOnElement();
		return xmlReader.ReadContentAsBinHex(buffer, offset, count);
	}

	public override bool ReadContentAsBoolean()
	{
		AdvanceToContentOnElement();
		return xmlReader.ReadContentAsBoolean();
	}

	public override int ReadContentAsChars(char[] chars, int index, int count)
	{
		AdvanceToContentOnElement();
		return xmlReader.ReadContentAsChars(chars, index, count);
	}

	public override DateTime ReadContentAsDateTime()
	{
		AdvanceToContentOnElement();
		return xmlReader.ReadContentAsDateTime();
	}

	public override decimal ReadContentAsDecimal()
	{
		AdvanceToContentOnElement();
		return xmlReader.ReadContentAsDecimal();
	}

	public override double ReadContentAsDouble()
	{
		AdvanceToContentOnElement();
		return xmlReader.ReadContentAsDouble();
	}

	public override int ReadContentAsInt()
	{
		AdvanceToContentOnElement();
		return xmlReader.ReadContentAsInt();
	}

	public override long ReadContentAsLong()
	{
		AdvanceToContentOnElement();
		return xmlReader.ReadContentAsLong();
	}

	public override object ReadContentAsObject()
	{
		AdvanceToContentOnElement();
		return xmlReader.ReadContentAsObject();
	}

	public override float ReadContentAsFloat()
	{
		AdvanceToContentOnElement();
		return xmlReader.ReadContentAsFloat();
	}

	public override string ReadContentAsString()
	{
		AdvanceToContentOnElement();
		return xmlReader.ReadContentAsString();
	}

	public override string ReadInnerXml()
	{
		return xmlReader.ReadInnerXml();
	}

	public override string ReadOuterXml()
	{
		return xmlReader.ReadOuterXml();
	}

	public override int ReadValueChunk(char[] buffer, int index, int count)
	{
		return xmlReader.ReadValueChunk(buffer, index, count);
	}

	public override void ResolveEntity()
	{
		xmlReader.ResolveEntity();
	}

	public override void Skip()
	{
		xmlReader.Skip();
	}

	public bool HasLineInfo()
	{
		if (xmlReader.ReadState == ReadState.Closed)
		{
			return false;
		}
		if (!(xmlReader is IXmlLineInfo xmlLineInfo))
		{
			return false;
		}
		return xmlLineInfo.HasLineInfo();
	}
}
