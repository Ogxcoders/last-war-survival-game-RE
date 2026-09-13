using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace System.Xml;

public abstract class XmlDictionaryReader : XmlReader
{
	private class XmlWrappedReader : XmlDictionaryReader, IXmlLineInfo
	{
		private XmlReader reader;

		private XmlNamespaceManager nsMgr;

		public override int AttributeCount => reader.AttributeCount;

		public override string BaseURI => reader.BaseURI;

		public override bool CanReadBinaryContent => reader.CanReadBinaryContent;

		public override bool CanReadValueChunk => reader.CanReadValueChunk;

		public override int Depth => reader.Depth;

		public override bool EOF => reader.EOF;

		public override bool HasValue => reader.HasValue;

		public override bool IsDefault => reader.IsDefault;

		public override bool IsEmptyElement => reader.IsEmptyElement;

		public override string LocalName => reader.LocalName;

		public override string Name => reader.Name;

		public override string NamespaceURI => reader.NamespaceURI;

		public override XmlNameTable NameTable => reader.NameTable;

		public override XmlNodeType NodeType => reader.NodeType;

		public override string Prefix => reader.Prefix;

		public override char QuoteChar => reader.QuoteChar;

		public override ReadState ReadState => reader.ReadState;

		public override string this[int index] => reader[index];

		public override string this[string name] => reader[name];

		public override string this[string name, string namespaceUri] => reader[name, namespaceUri];

		public override string Value => reader.Value;

		public override string XmlLang => reader.XmlLang;

		public override XmlSpace XmlSpace => reader.XmlSpace;

		public override Type ValueType => reader.ValueType;

		public int LineNumber
		{
			get
			{
				if (!(reader is IXmlLineInfo xmlLineInfo))
				{
					return 1;
				}
				return xmlLineInfo.LineNumber;
			}
		}

		public int LinePosition
		{
			get
			{
				if (!(reader is IXmlLineInfo xmlLineInfo))
				{
					return 1;
				}
				return xmlLineInfo.LinePosition;
			}
		}

		public XmlWrappedReader(XmlReader reader, XmlNamespaceManager nsMgr)
		{
			this.reader = reader;
			this.nsMgr = nsMgr;
		}

		public override void Close()
		{
			reader.Close();
			nsMgr = null;
		}

		public override string GetAttribute(int index)
		{
			return reader.GetAttribute(index);
		}

		public override string GetAttribute(string name)
		{
			return reader.GetAttribute(name);
		}

		public override string GetAttribute(string name, string namespaceUri)
		{
			return reader.GetAttribute(name, namespaceUri);
		}

		public override bool IsStartElement(string name)
		{
			return reader.IsStartElement(name);
		}

		public override bool IsStartElement(string localName, string namespaceUri)
		{
			return reader.IsStartElement(localName, namespaceUri);
		}

		public override string LookupNamespace(string namespaceUri)
		{
			return reader.LookupNamespace(namespaceUri);
		}

		public override void MoveToAttribute(int index)
		{
			reader.MoveToAttribute(index);
		}

		public override bool MoveToAttribute(string name)
		{
			return reader.MoveToAttribute(name);
		}

		public override bool MoveToAttribute(string name, string namespaceUri)
		{
			return reader.MoveToAttribute(name, namespaceUri);
		}

		public override bool MoveToElement()
		{
			return reader.MoveToElement();
		}

		public override bool MoveToFirstAttribute()
		{
			return reader.MoveToFirstAttribute();
		}

		public override bool MoveToNextAttribute()
		{
			return reader.MoveToNextAttribute();
		}

		public override bool Read()
		{
			return reader.Read();
		}

		public override bool ReadAttributeValue()
		{
			return reader.ReadAttributeValue();
		}

		public override string ReadElementString(string name)
		{
			return reader.ReadElementString(name);
		}

		public override string ReadElementString(string localName, string namespaceUri)
		{
			return reader.ReadElementString(localName, namespaceUri);
		}

		public override string ReadInnerXml()
		{
			return reader.ReadInnerXml();
		}

		public override string ReadOuterXml()
		{
			return reader.ReadOuterXml();
		}

		public override void ReadStartElement(string name)
		{
			reader.ReadStartElement(name);
		}

		public override void ReadStartElement(string localName, string namespaceUri)
		{
			reader.ReadStartElement(localName, namespaceUri);
		}

		public override void ReadEndElement()
		{
			reader.ReadEndElement();
		}

		public override string ReadString()
		{
			return reader.ReadString();
		}

		public override void ResolveEntity()
		{
			reader.ResolveEntity();
		}

		public override int ReadElementContentAsBase64(byte[] buffer, int offset, int count)
		{
			return reader.ReadElementContentAsBase64(buffer, offset, count);
		}

		public override int ReadContentAsBase64(byte[] buffer, int offset, int count)
		{
			return reader.ReadContentAsBase64(buffer, offset, count);
		}

		public override int ReadElementContentAsBinHex(byte[] buffer, int offset, int count)
		{
			return reader.ReadElementContentAsBinHex(buffer, offset, count);
		}

		public override int ReadContentAsBinHex(byte[] buffer, int offset, int count)
		{
			return reader.ReadContentAsBinHex(buffer, offset, count);
		}

		public override int ReadValueChunk(char[] chars, int offset, int count)
		{
			return reader.ReadValueChunk(chars, offset, count);
		}

		public override bool ReadContentAsBoolean()
		{
			return reader.ReadContentAsBoolean();
		}

		public override DateTime ReadContentAsDateTime()
		{
			return reader.ReadContentAsDateTime();
		}

		public override decimal ReadContentAsDecimal()
		{
			return (decimal)reader.ReadContentAs(typeof(decimal), null);
		}

		public override double ReadContentAsDouble()
		{
			return reader.ReadContentAsDouble();
		}

		public override int ReadContentAsInt()
		{
			return reader.ReadContentAsInt();
		}

		public override long ReadContentAsLong()
		{
			return reader.ReadContentAsLong();
		}

		public override float ReadContentAsFloat()
		{
			return reader.ReadContentAsFloat();
		}

		public override string ReadContentAsString()
		{
			return reader.ReadContentAsString();
		}

		public override object ReadContentAs(Type type, IXmlNamespaceResolver namespaceResolver)
		{
			return reader.ReadContentAs(type, namespaceResolver);
		}

		public bool HasLineInfo()
		{
			if (!(reader is IXmlLineInfo xmlLineInfo))
			{
				return false;
			}
			return xmlLineInfo.HasLineInfo();
		}
	}

	internal const int MaxInitialArrayLength = 65535;

	public virtual bool CanCanonicalize => false;

	public virtual XmlDictionaryReaderQuotas Quotas => XmlDictionaryReaderQuotas.Max;

	public static XmlDictionaryReader CreateDictionaryReader(XmlReader reader)
	{
		if (reader == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("reader");
		}
		XmlDictionaryReader xmlDictionaryReader = reader as XmlDictionaryReader;
		if (xmlDictionaryReader == null)
		{
			xmlDictionaryReader = new XmlWrappedReader(reader, null);
		}
		return xmlDictionaryReader;
	}

	public static XmlDictionaryReader CreateBinaryReader(byte[] buffer, XmlDictionaryReaderQuotas quotas)
	{
		if (buffer == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
		}
		return CreateBinaryReader(buffer, 0, buffer.Length, quotas);
	}

	public static XmlDictionaryReader CreateBinaryReader(byte[] buffer, int offset, int count, XmlDictionaryReaderQuotas quotas)
	{
		return CreateBinaryReader(buffer, offset, count, null, quotas);
	}

	public static XmlDictionaryReader CreateBinaryReader(byte[] buffer, int offset, int count, IXmlDictionary dictionary, XmlDictionaryReaderQuotas quotas)
	{
		return CreateBinaryReader(buffer, offset, count, dictionary, quotas, null);
	}

	public static XmlDictionaryReader CreateBinaryReader(byte[] buffer, int offset, int count, IXmlDictionary dictionary, XmlDictionaryReaderQuotas quotas, XmlBinaryReaderSession session)
	{
		return CreateBinaryReader(buffer, offset, count, dictionary, quotas, session, null);
	}

	public static XmlDictionaryReader CreateBinaryReader(byte[] buffer, int offset, int count, IXmlDictionary dictionary, XmlDictionaryReaderQuotas quotas, XmlBinaryReaderSession session, OnXmlDictionaryReaderClose onClose)
	{
		XmlBinaryReader xmlBinaryReader = new XmlBinaryReader();
		xmlBinaryReader.SetInput(buffer, offset, count, dictionary, quotas, session, onClose);
		return xmlBinaryReader;
	}

	public static XmlDictionaryReader CreateBinaryReader(Stream stream, XmlDictionaryReaderQuotas quotas)
	{
		return CreateBinaryReader(stream, null, quotas);
	}

	public static XmlDictionaryReader CreateBinaryReader(Stream stream, IXmlDictionary dictionary, XmlDictionaryReaderQuotas quotas)
	{
		return CreateBinaryReader(stream, dictionary, quotas, null);
	}

	public static XmlDictionaryReader CreateBinaryReader(Stream stream, IXmlDictionary dictionary, XmlDictionaryReaderQuotas quotas, XmlBinaryReaderSession session)
	{
		return CreateBinaryReader(stream, dictionary, quotas, session, null);
	}

	public static XmlDictionaryReader CreateBinaryReader(Stream stream, IXmlDictionary dictionary, XmlDictionaryReaderQuotas quotas, XmlBinaryReaderSession session, OnXmlDictionaryReaderClose onClose)
	{
		XmlBinaryReader xmlBinaryReader = new XmlBinaryReader();
		xmlBinaryReader.SetInput(stream, dictionary, quotas, session, onClose);
		return xmlBinaryReader;
	}

	public static XmlDictionaryReader CreateTextReader(byte[] buffer, XmlDictionaryReaderQuotas quotas)
	{
		if (buffer == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
		}
		return CreateTextReader(buffer, 0, buffer.Length, quotas);
	}

	public static XmlDictionaryReader CreateTextReader(byte[] buffer, int offset, int count, XmlDictionaryReaderQuotas quotas)
	{
		return CreateTextReader(buffer, offset, count, null, quotas, null);
	}

	public static XmlDictionaryReader CreateTextReader(byte[] buffer, int offset, int count, Encoding encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose onClose)
	{
		XmlUTF8TextReader xmlUTF8TextReader = new XmlUTF8TextReader();
		xmlUTF8TextReader.SetInput(buffer, offset, count, encoding, quotas, onClose);
		return xmlUTF8TextReader;
	}

	public static XmlDictionaryReader CreateTextReader(Stream stream, XmlDictionaryReaderQuotas quotas)
	{
		return CreateTextReader(stream, null, quotas, null);
	}

	public static XmlDictionaryReader CreateTextReader(Stream stream, Encoding encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose onClose)
	{
		XmlUTF8TextReader xmlUTF8TextReader = new XmlUTF8TextReader();
		xmlUTF8TextReader.SetInput(stream, encoding, quotas, onClose);
		return xmlUTF8TextReader;
	}

	public static XmlDictionaryReader CreateMtomReader(Stream stream, Encoding encoding, XmlDictionaryReaderQuotas quotas)
	{
		if (encoding == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("encoding");
		}
		return CreateMtomReader(stream, new Encoding[1] { encoding }, quotas);
	}

	public static XmlDictionaryReader CreateMtomReader(Stream stream, Encoding[] encodings, XmlDictionaryReaderQuotas quotas)
	{
		return CreateMtomReader(stream, encodings, null, quotas);
	}

	public static XmlDictionaryReader CreateMtomReader(Stream stream, Encoding[] encodings, string contentType, XmlDictionaryReaderQuotas quotas)
	{
		return CreateMtomReader(stream, encodings, contentType, quotas, int.MaxValue, null);
	}

	public static XmlDictionaryReader CreateMtomReader(Stream stream, Encoding[] encodings, string contentType, XmlDictionaryReaderQuotas quotas, int maxBufferSize, OnXmlDictionaryReaderClose onClose)
	{
		XmlMtomReader xmlMtomReader = new XmlMtomReader();
		xmlMtomReader.SetInput(stream, encodings, contentType, quotas, maxBufferSize, onClose);
		return xmlMtomReader;
	}

	public static XmlDictionaryReader CreateMtomReader(byte[] buffer, int offset, int count, Encoding encoding, XmlDictionaryReaderQuotas quotas)
	{
		if (encoding == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("encoding");
		}
		return CreateMtomReader(buffer, offset, count, new Encoding[1] { encoding }, quotas);
	}

	public static XmlDictionaryReader CreateMtomReader(byte[] buffer, int offset, int count, Encoding[] encodings, XmlDictionaryReaderQuotas quotas)
	{
		return CreateMtomReader(buffer, offset, count, encodings, null, quotas);
	}

	public static XmlDictionaryReader CreateMtomReader(byte[] buffer, int offset, int count, Encoding[] encodings, string contentType, XmlDictionaryReaderQuotas quotas)
	{
		return CreateMtomReader(buffer, offset, count, encodings, contentType, quotas, int.MaxValue, null);
	}

	public static XmlDictionaryReader CreateMtomReader(byte[] buffer, int offset, int count, Encoding[] encodings, string contentType, XmlDictionaryReaderQuotas quotas, int maxBufferSize, OnXmlDictionaryReaderClose onClose)
	{
		XmlMtomReader xmlMtomReader = new XmlMtomReader();
		xmlMtomReader.SetInput(buffer, offset, count, encodings, contentType, quotas, maxBufferSize, onClose);
		return xmlMtomReader;
	}

	public virtual void StartCanonicalization(Stream stream, bool includeComments, string[] inclusivePrefixes)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
	}

	public virtual void EndCanonicalization()
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
	}

	public virtual void MoveToStartElement()
	{
		if (!IsStartElement())
		{
			XmlExceptionHelper.ThrowStartElementExpected(this);
		}
	}

	public virtual void MoveToStartElement(string name)
	{
		if (!IsStartElement(name))
		{
			XmlExceptionHelper.ThrowStartElementExpected(this, name);
		}
	}

	public virtual void MoveToStartElement(string localName, string namespaceUri)
	{
		if (!IsStartElement(localName, namespaceUri))
		{
			XmlExceptionHelper.ThrowStartElementExpected(this, localName, namespaceUri);
		}
	}

	public virtual void MoveToStartElement(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		if (!IsStartElement(localName, namespaceUri))
		{
			XmlExceptionHelper.ThrowStartElementExpected(this, localName, namespaceUri);
		}
	}

	public virtual bool IsLocalName(string localName)
	{
		return LocalName == localName;
	}

	public virtual bool IsLocalName(XmlDictionaryString localName)
	{
		if (localName == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localName");
		}
		return IsLocalName(localName.Value);
	}

	public virtual bool IsNamespaceUri(string namespaceUri)
	{
		if (namespaceUri == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
		}
		return NamespaceURI == namespaceUri;
	}

	public virtual bool IsNamespaceUri(XmlDictionaryString namespaceUri)
	{
		if (namespaceUri == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
		}
		return IsNamespaceUri(namespaceUri.Value);
	}

	public virtual void ReadFullStartElement()
	{
		MoveToStartElement();
		if (IsEmptyElement)
		{
			XmlExceptionHelper.ThrowFullStartElementExpected(this);
		}
		Read();
	}

	public virtual void ReadFullStartElement(string name)
	{
		MoveToStartElement(name);
		if (IsEmptyElement)
		{
			XmlExceptionHelper.ThrowFullStartElementExpected(this, name);
		}
		Read();
	}

	public virtual void ReadFullStartElement(string localName, string namespaceUri)
	{
		MoveToStartElement(localName, namespaceUri);
		if (IsEmptyElement)
		{
			XmlExceptionHelper.ThrowFullStartElementExpected(this, localName, namespaceUri);
		}
		Read();
	}

	public virtual void ReadFullStartElement(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		MoveToStartElement(localName, namespaceUri);
		if (IsEmptyElement)
		{
			XmlExceptionHelper.ThrowFullStartElementExpected(this, localName, namespaceUri);
		}
		Read();
	}

	public virtual void ReadStartElement(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		MoveToStartElement(localName, namespaceUri);
		Read();
	}

	public virtual bool IsStartElement(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		return IsStartElement(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri));
	}

	public virtual int IndexOfLocalName(string[] localNames, string namespaceUri)
	{
		if (localNames == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localNames");
		}
		if (namespaceUri == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
		}
		if (NamespaceURI == namespaceUri)
		{
			string localName = LocalName;
			for (int i = 0; i < localNames.Length; i++)
			{
				string text = localNames[i];
				if (text == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(string.Format(CultureInfo.InvariantCulture, "localNames[{0}]", i));
				}
				if (localName == text)
				{
					return i;
				}
			}
		}
		return -1;
	}

	public virtual int IndexOfLocalName(XmlDictionaryString[] localNames, XmlDictionaryString namespaceUri)
	{
		if (localNames == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localNames");
		}
		if (namespaceUri == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
		}
		if (NamespaceURI == namespaceUri.Value)
		{
			string localName = LocalName;
			for (int i = 0; i < localNames.Length; i++)
			{
				XmlDictionaryString xmlDictionaryString = localNames[i];
				if (xmlDictionaryString == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(string.Format(CultureInfo.InvariantCulture, "localNames[{0}]", i));
				}
				if (localName == xmlDictionaryString.Value)
				{
					return i;
				}
			}
		}
		return -1;
	}

	public virtual string GetAttribute(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		return GetAttribute(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri));
	}

	public virtual bool TryGetBase64ContentLength(out int length)
	{
		length = 0;
		return false;
	}

	public virtual int ReadValueAsBase64(byte[] buffer, int offset, int count)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
	}

	public virtual byte[] ReadContentAsBase64()
	{
		return ReadContentAsBase64(Quotas.MaxArrayLength, 65535);
	}

	internal byte[] ReadContentAsBase64(int maxByteArrayContentLength, int maxInitialCount)
	{
		if (TryGetBase64ContentLength(out var length))
		{
			if (length > maxByteArrayContentLength)
			{
				XmlExceptionHelper.ThrowMaxArrayLengthExceeded(this, maxByteArrayContentLength);
			}
			if (length <= maxInitialCount)
			{
				byte[] array = new byte[length];
				int num;
				for (int i = 0; i < length; i += num)
				{
					num = ReadContentAsBase64(array, i, length - i);
					if (num == 0)
					{
						XmlExceptionHelper.ThrowBase64DataExpected(this);
					}
				}
				return array;
			}
		}
		return ReadContentAsBytes(base64: true, maxByteArrayContentLength);
	}

	public override string ReadContentAsString()
	{
		return ReadContentAsString(Quotas.MaxStringContentLength);
	}

	protected string ReadContentAsString(int maxStringContentLength)
	{
		StringBuilder stringBuilder = null;
		string text = string.Empty;
		bool flag = false;
		while (true)
		{
			switch (NodeType)
			{
			case XmlNodeType.Attribute:
				text = Value;
				break;
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
			{
				string value = Value;
				if (text.Length == 0)
				{
					text = value;
					break;
				}
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(text);
				}
				if (stringBuilder.Length > maxStringContentLength - value.Length)
				{
					XmlExceptionHelper.ThrowMaxStringContentLengthExceeded(this, maxStringContentLength);
				}
				stringBuilder.Append(value);
				break;
			}
			case XmlNodeType.EntityReference:
				if (CanResolveEntity)
				{
					ResolveEntity();
					break;
				}
				goto default;
			default:
				flag = true;
				break;
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.Comment:
			case XmlNodeType.EndEntity:
				break;
			}
			if (flag)
			{
				break;
			}
			if (AttributeCount != 0)
			{
				ReadAttributeValue();
			}
			else
			{
				Read();
			}
		}
		if (stringBuilder != null)
		{
			text = stringBuilder.ToString();
		}
		if (text.Length > maxStringContentLength)
		{
			XmlExceptionHelper.ThrowMaxStringContentLengthExceeded(this, maxStringContentLength);
		}
		return text;
	}

	public override string ReadString()
	{
		return ReadString(Quotas.MaxStringContentLength);
	}

	protected string ReadString(int maxStringContentLength)
	{
		if (ReadState != ReadState.Interactive)
		{
			return string.Empty;
		}
		if (NodeType != XmlNodeType.Element)
		{
			MoveToElement();
		}
		if (NodeType == XmlNodeType.Element)
		{
			if (IsEmptyElement)
			{
				return string.Empty;
			}
			if (!Read())
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("The reader cannot be advanced.")));
			}
			if (NodeType == XmlNodeType.EndElement)
			{
				return string.Empty;
			}
		}
		StringBuilder stringBuilder = null;
		string text = string.Empty;
		while (IsTextNode(NodeType))
		{
			string value = Value;
			if (text.Length == 0)
			{
				text = value;
			}
			else
			{
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(text);
				}
				if (stringBuilder.Length > maxStringContentLength - value.Length)
				{
					XmlExceptionHelper.ThrowMaxStringContentLengthExceeded(this, maxStringContentLength);
				}
				stringBuilder.Append(value);
			}
			if (!Read())
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("The reader cannot be advanced.")));
			}
		}
		if (stringBuilder != null)
		{
			text = stringBuilder.ToString();
		}
		if (text.Length > maxStringContentLength)
		{
			XmlExceptionHelper.ThrowMaxStringContentLengthExceeded(this, maxStringContentLength);
		}
		return text;
	}

	public virtual byte[] ReadContentAsBinHex()
	{
		return ReadContentAsBinHex(Quotas.MaxArrayLength);
	}

	protected byte[] ReadContentAsBinHex(int maxByteArrayContentLength)
	{
		return ReadContentAsBytes(base64: false, maxByteArrayContentLength);
	}

	private byte[] ReadContentAsBytes(bool base64, int maxByteArrayContentLength)
	{
		byte[][] array = new byte[32][];
		int num = 384;
		int num2 = 0;
		int num3 = 0;
		byte[] array2;
		while (true)
		{
			array2 = new byte[num];
			array[num2++] = array2;
			int i;
			int num4;
			for (i = 0; i < array2.Length; i += num4)
			{
				num4 = ((!base64) ? ReadContentAsBinHex(array2, i, array2.Length - i) : ReadContentAsBase64(array2, i, array2.Length - i));
				if (num4 == 0)
				{
					break;
				}
			}
			if (num3 > maxByteArrayContentLength - i)
			{
				XmlExceptionHelper.ThrowMaxArrayLengthExceeded(this, maxByteArrayContentLength);
			}
			num3 += i;
			if (i < array2.Length)
			{
				break;
			}
			num *= 2;
		}
		array2 = new byte[num3];
		int num5 = 0;
		for (int j = 0; j < num2 - 1; j++)
		{
			Buffer.BlockCopy(array[j], 0, array2, num5, array[j].Length);
			num5 += array[j].Length;
		}
		Buffer.BlockCopy(array[num2 - 1], 0, array2, num5, num3 - num5);
		return array2;
	}

	protected bool IsTextNode(XmlNodeType nodeType)
	{
		if (nodeType != XmlNodeType.Text && nodeType != XmlNodeType.Whitespace && nodeType != XmlNodeType.SignificantWhitespace && nodeType != XmlNodeType.CDATA)
		{
			return nodeType == XmlNodeType.Attribute;
		}
		return true;
	}

	public virtual int ReadContentAsChars(char[] chars, int offset, int count)
	{
		int num = 0;
		while (true)
		{
			XmlNodeType nodeType = NodeType;
			if (nodeType == XmlNodeType.Element || nodeType == XmlNodeType.EndElement)
			{
				break;
			}
			if (IsTextNode(nodeType))
			{
				num = ReadValueChunk(chars, offset, count);
				if (num > 0 || nodeType == XmlNodeType.Attribute || !Read())
				{
					break;
				}
			}
			else if (!Read())
			{
				break;
			}
		}
		return num;
	}

	public override object ReadContentAs(Type type, IXmlNamespaceResolver namespaceResolver)
	{
		if (type == typeof(Guid[]))
		{
			string[] array = (string[])ReadContentAs(typeof(string[]), namespaceResolver);
			Guid[] array2 = new Guid[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = XmlConverter.ToGuid(array[i]);
			}
			return array2;
		}
		if (type == typeof(UniqueId[]))
		{
			string[] array3 = (string[])ReadContentAs(typeof(string[]), namespaceResolver);
			UniqueId[] array4 = new UniqueId[array3.Length];
			for (int j = 0; j < array3.Length; j++)
			{
				array4[j] = XmlConverter.ToUniqueId(array3[j]);
			}
			return array4;
		}
		return base.ReadContentAs(type, namespaceResolver);
	}

	public virtual string ReadContentAsString(string[] strings, out int index)
	{
		if (strings == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("strings");
		}
		string text = ReadContentAsString();
		index = -1;
		for (int i = 0; i < strings.Length; i++)
		{
			string text2 = strings[i];
			if (text2 == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(string.Format(CultureInfo.InvariantCulture, "strings[{0}]", i));
			}
			if (text2 == text)
			{
				index = i;
				return text2;
			}
		}
		return text;
	}

	public virtual string ReadContentAsString(XmlDictionaryString[] strings, out int index)
	{
		if (strings == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("strings");
		}
		string text = ReadContentAsString();
		index = -1;
		for (int i = 0; i < strings.Length; i++)
		{
			XmlDictionaryString xmlDictionaryString = strings[i];
			if (xmlDictionaryString == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(string.Format(CultureInfo.InvariantCulture, "strings[{0}]", i));
			}
			if (xmlDictionaryString.Value == text)
			{
				index = i;
				return xmlDictionaryString.Value;
			}
		}
		return text;
	}

	public override decimal ReadContentAsDecimal()
	{
		return XmlConverter.ToDecimal(ReadContentAsString());
	}

	public override float ReadContentAsFloat()
	{
		return XmlConverter.ToSingle(ReadContentAsString());
	}

	public virtual UniqueId ReadContentAsUniqueId()
	{
		return XmlConverter.ToUniqueId(ReadContentAsString());
	}

	public virtual Guid ReadContentAsGuid()
	{
		return XmlConverter.ToGuid(ReadContentAsString());
	}

	public virtual TimeSpan ReadContentAsTimeSpan()
	{
		return XmlConverter.ToTimeSpan(ReadContentAsString());
	}

	public virtual void ReadContentAsQualifiedName(out string localName, out string namespaceUri)
	{
		XmlConverter.ToQualifiedName(ReadContentAsString(), out var prefix, out localName);
		namespaceUri = LookupNamespace(prefix);
		if (namespaceUri == null)
		{
			XmlExceptionHelper.ThrowUndefinedPrefix(this, prefix);
		}
	}

	public override string ReadElementContentAsString()
	{
		string result;
		if (IsStartElement() && IsEmptyElement)
		{
			Read();
			result = string.Empty;
		}
		else
		{
			ReadStartElement();
			result = ReadContentAsString();
			ReadEndElement();
		}
		return result;
	}

	public override bool ReadElementContentAsBoolean()
	{
		bool result;
		if (IsStartElement() && IsEmptyElement)
		{
			Read();
			result = XmlConverter.ToBoolean(string.Empty);
		}
		else
		{
			ReadStartElement();
			result = ReadContentAsBoolean();
			ReadEndElement();
		}
		return result;
	}

	public override int ReadElementContentAsInt()
	{
		int result;
		if (IsStartElement() && IsEmptyElement)
		{
			Read();
			result = XmlConverter.ToInt32(string.Empty);
		}
		else
		{
			ReadStartElement();
			result = ReadContentAsInt();
			ReadEndElement();
		}
		return result;
	}

	public override long ReadElementContentAsLong()
	{
		long result;
		if (IsStartElement() && IsEmptyElement)
		{
			Read();
			result = XmlConverter.ToInt64(string.Empty);
		}
		else
		{
			ReadStartElement();
			result = ReadContentAsLong();
			ReadEndElement();
		}
		return result;
	}

	public override float ReadElementContentAsFloat()
	{
		float result;
		if (IsStartElement() && IsEmptyElement)
		{
			Read();
			result = XmlConverter.ToSingle(string.Empty);
		}
		else
		{
			ReadStartElement();
			result = ReadContentAsFloat();
			ReadEndElement();
		}
		return result;
	}

	public override double ReadElementContentAsDouble()
	{
		double result;
		if (IsStartElement() && IsEmptyElement)
		{
			Read();
			result = XmlConverter.ToDouble(string.Empty);
		}
		else
		{
			ReadStartElement();
			result = ReadContentAsDouble();
			ReadEndElement();
		}
		return result;
	}

	public override decimal ReadElementContentAsDecimal()
	{
		decimal result;
		if (IsStartElement() && IsEmptyElement)
		{
			Read();
			result = XmlConverter.ToDecimal(string.Empty);
		}
		else
		{
			ReadStartElement();
			result = ReadContentAsDecimal();
			ReadEndElement();
		}
		return result;
	}

	public override DateTime ReadElementContentAsDateTime()
	{
		DateTime result;
		if (IsStartElement() && IsEmptyElement)
		{
			Read();
			try
			{
				result = DateTime.Parse(string.Empty, NumberFormatInfo.InvariantInfo);
			}
			catch (ArgumentException exception)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(string.Empty, "DateTime", exception));
			}
			catch (FormatException exception2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(string.Empty, "DateTime", exception2));
			}
		}
		else
		{
			ReadStartElement();
			result = ReadContentAsDateTime();
			ReadEndElement();
		}
		return result;
	}

	public virtual UniqueId ReadElementContentAsUniqueId()
	{
		UniqueId result;
		if (IsStartElement() && IsEmptyElement)
		{
			Read();
			try
			{
				result = new UniqueId(string.Empty);
			}
			catch (ArgumentException exception)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(string.Empty, "UniqueId", exception));
			}
			catch (FormatException exception2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(string.Empty, "UniqueId", exception2));
			}
		}
		else
		{
			ReadStartElement();
			result = ReadContentAsUniqueId();
			ReadEndElement();
		}
		return result;
	}

	public virtual Guid ReadElementContentAsGuid()
	{
		Guid result;
		if (IsStartElement() && IsEmptyElement)
		{
			Read();
			try
			{
				result = Guid.Empty;
			}
			catch (ArgumentException exception)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(string.Empty, "Guid", exception));
			}
			catch (FormatException exception2)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(string.Empty, "Guid", exception2));
			}
			catch (OverflowException exception3)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateConversionException(string.Empty, "Guid", exception3));
			}
		}
		else
		{
			ReadStartElement();
			result = ReadContentAsGuid();
			ReadEndElement();
		}
		return result;
	}

	public virtual TimeSpan ReadElementContentAsTimeSpan()
	{
		TimeSpan result;
		if (IsStartElement() && IsEmptyElement)
		{
			Read();
			result = XmlConverter.ToTimeSpan(string.Empty);
		}
		else
		{
			ReadStartElement();
			result = ReadContentAsTimeSpan();
			ReadEndElement();
		}
		return result;
	}

	public virtual byte[] ReadElementContentAsBase64()
	{
		byte[] result;
		if (IsStartElement() && IsEmptyElement)
		{
			Read();
			result = new byte[0];
		}
		else
		{
			ReadStartElement();
			result = ReadContentAsBase64();
			ReadEndElement();
		}
		return result;
	}

	public virtual byte[] ReadElementContentAsBinHex()
	{
		byte[] result;
		if (IsStartElement() && IsEmptyElement)
		{
			Read();
			result = new byte[0];
		}
		else
		{
			ReadStartElement();
			result = ReadContentAsBinHex();
			ReadEndElement();
		}
		return result;
	}

	public virtual void GetNonAtomizedNames(out string localName, out string namespaceUri)
	{
		localName = LocalName;
		namespaceUri = NamespaceURI;
	}

	public virtual bool TryGetLocalNameAsDictionaryString(out XmlDictionaryString localName)
	{
		localName = null;
		return false;
	}

	public virtual bool TryGetNamespaceUriAsDictionaryString(out XmlDictionaryString namespaceUri)
	{
		namespaceUri = null;
		return false;
	}

	public virtual bool TryGetValueAsDictionaryString(out XmlDictionaryString value)
	{
		value = null;
		return false;
	}

	private void CheckArray(Array array, int offset, int count)
	{
		if (array == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("array"));
		}
		if (offset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (offset > array.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", array.Length)));
		}
		if (count < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count > array.Length - offset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", array.Length - offset)));
		}
	}

	public virtual bool IsStartArray(out Type type)
	{
		type = null;
		return false;
	}

	public virtual bool TryGetArrayLength(out int count)
	{
		count = 0;
		return false;
	}

	public virtual bool[] ReadBooleanArray(string localName, string namespaceUri)
	{
		return BooleanArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, Quotas.MaxArrayLength);
	}

	public virtual bool[] ReadBooleanArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		return BooleanArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, Quotas.MaxArrayLength);
	}

	public virtual int ReadArray(string localName, string namespaceUri, bool[] array, int offset, int count)
	{
		CheckArray(array, offset, count);
		int i;
		for (i = 0; i < count; i++)
		{
			if (!IsStartElement(localName, namespaceUri))
			{
				break;
			}
			array[offset + i] = ReadElementContentAsBoolean();
		}
		return i;
	}

	public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, bool[] array, int offset, int count)
	{
		return ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
	}

	public virtual short[] ReadInt16Array(string localName, string namespaceUri)
	{
		return Int16ArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, Quotas.MaxArrayLength);
	}

	public virtual short[] ReadInt16Array(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		return Int16ArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, Quotas.MaxArrayLength);
	}

	public virtual int ReadArray(string localName, string namespaceUri, short[] array, int offset, int count)
	{
		CheckArray(array, offset, count);
		int i;
		for (i = 0; i < count; i++)
		{
			if (!IsStartElement(localName, namespaceUri))
			{
				break;
			}
			int num = ReadElementContentAsInt();
			if (num < -32768 || num > 32767)
			{
				XmlExceptionHelper.ThrowConversionOverflow(this, num.ToString(NumberFormatInfo.CurrentInfo), "Int16");
			}
			array[offset + i] = (short)num;
		}
		return i;
	}

	public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, short[] array, int offset, int count)
	{
		return ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
	}

	public virtual int[] ReadInt32Array(string localName, string namespaceUri)
	{
		return Int32ArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, Quotas.MaxArrayLength);
	}

	public virtual int[] ReadInt32Array(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		return Int32ArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, Quotas.MaxArrayLength);
	}

	public virtual int ReadArray(string localName, string namespaceUri, int[] array, int offset, int count)
	{
		CheckArray(array, offset, count);
		int i;
		for (i = 0; i < count; i++)
		{
			if (!IsStartElement(localName, namespaceUri))
			{
				break;
			}
			array[offset + i] = ReadElementContentAsInt();
		}
		return i;
	}

	public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, int[] array, int offset, int count)
	{
		return ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
	}

	public virtual long[] ReadInt64Array(string localName, string namespaceUri)
	{
		return Int64ArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, Quotas.MaxArrayLength);
	}

	public virtual long[] ReadInt64Array(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		return Int64ArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, Quotas.MaxArrayLength);
	}

	public virtual int ReadArray(string localName, string namespaceUri, long[] array, int offset, int count)
	{
		CheckArray(array, offset, count);
		int i;
		for (i = 0; i < count; i++)
		{
			if (!IsStartElement(localName, namespaceUri))
			{
				break;
			}
			array[offset + i] = ReadElementContentAsLong();
		}
		return i;
	}

	public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, long[] array, int offset, int count)
	{
		return ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
	}

	public virtual float[] ReadSingleArray(string localName, string namespaceUri)
	{
		return SingleArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, Quotas.MaxArrayLength);
	}

	public virtual float[] ReadSingleArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		return SingleArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, Quotas.MaxArrayLength);
	}

	public virtual int ReadArray(string localName, string namespaceUri, float[] array, int offset, int count)
	{
		CheckArray(array, offset, count);
		int i;
		for (i = 0; i < count; i++)
		{
			if (!IsStartElement(localName, namespaceUri))
			{
				break;
			}
			array[offset + i] = ReadElementContentAsFloat();
		}
		return i;
	}

	public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, float[] array, int offset, int count)
	{
		return ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
	}

	public virtual double[] ReadDoubleArray(string localName, string namespaceUri)
	{
		return DoubleArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, Quotas.MaxArrayLength);
	}

	public virtual double[] ReadDoubleArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		return DoubleArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, Quotas.MaxArrayLength);
	}

	public virtual int ReadArray(string localName, string namespaceUri, double[] array, int offset, int count)
	{
		CheckArray(array, offset, count);
		int i;
		for (i = 0; i < count; i++)
		{
			if (!IsStartElement(localName, namespaceUri))
			{
				break;
			}
			array[offset + i] = ReadElementContentAsDouble();
		}
		return i;
	}

	public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, double[] array, int offset, int count)
	{
		return ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
	}

	public virtual decimal[] ReadDecimalArray(string localName, string namespaceUri)
	{
		return DecimalArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, Quotas.MaxArrayLength);
	}

	public virtual decimal[] ReadDecimalArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		return DecimalArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, Quotas.MaxArrayLength);
	}

	public virtual int ReadArray(string localName, string namespaceUri, decimal[] array, int offset, int count)
	{
		CheckArray(array, offset, count);
		int i;
		for (i = 0; i < count; i++)
		{
			if (!IsStartElement(localName, namespaceUri))
			{
				break;
			}
			array[offset + i] = ReadElementContentAsDecimal();
		}
		return i;
	}

	public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, decimal[] array, int offset, int count)
	{
		return ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
	}

	public virtual DateTime[] ReadDateTimeArray(string localName, string namespaceUri)
	{
		return DateTimeArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, Quotas.MaxArrayLength);
	}

	public virtual DateTime[] ReadDateTimeArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		return DateTimeArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, Quotas.MaxArrayLength);
	}

	public virtual int ReadArray(string localName, string namespaceUri, DateTime[] array, int offset, int count)
	{
		CheckArray(array, offset, count);
		int i;
		for (i = 0; i < count; i++)
		{
			if (!IsStartElement(localName, namespaceUri))
			{
				break;
			}
			array[offset + i] = ReadElementContentAsDateTime();
		}
		return i;
	}

	public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, DateTime[] array, int offset, int count)
	{
		return ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
	}

	public virtual Guid[] ReadGuidArray(string localName, string namespaceUri)
	{
		return GuidArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, Quotas.MaxArrayLength);
	}

	public virtual Guid[] ReadGuidArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		return GuidArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, Quotas.MaxArrayLength);
	}

	public virtual int ReadArray(string localName, string namespaceUri, Guid[] array, int offset, int count)
	{
		CheckArray(array, offset, count);
		int i;
		for (i = 0; i < count; i++)
		{
			if (!IsStartElement(localName, namespaceUri))
			{
				break;
			}
			array[offset + i] = ReadElementContentAsGuid();
		}
		return i;
	}

	public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, Guid[] array, int offset, int count)
	{
		return ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
	}

	public virtual TimeSpan[] ReadTimeSpanArray(string localName, string namespaceUri)
	{
		return TimeSpanArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, Quotas.MaxArrayLength);
	}

	public virtual TimeSpan[] ReadTimeSpanArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		return TimeSpanArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, Quotas.MaxArrayLength);
	}

	public virtual int ReadArray(string localName, string namespaceUri, TimeSpan[] array, int offset, int count)
	{
		CheckArray(array, offset, count);
		int i;
		for (i = 0; i < count; i++)
		{
			if (!IsStartElement(localName, namespaceUri))
			{
				break;
			}
			array[offset + i] = ReadElementContentAsTimeSpan();
		}
		return i;
	}

	public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, TimeSpan[] array, int offset, int count)
	{
		return ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
	}
}
