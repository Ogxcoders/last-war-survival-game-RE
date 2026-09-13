using System.Globalization;
using System.IO;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace System.Xml;

internal class XmlUtf8RawTextWriter : XmlRawWriter
{
	private readonly bool useAsync;

	protected byte[] bufBytes;

	protected Stream stream;

	protected Encoding encoding;

	protected XmlCharType xmlCharType = XmlCharType.Instance;

	protected int bufPos = 1;

	protected int textPos = 1;

	protected int contentPos;

	protected int cdataPos;

	protected int attrEndPos;

	protected int bufLen = 6144;

	protected bool writeToNull;

	protected bool hadDoubleBracket;

	protected bool inAttributeValue;

	protected NewLineHandling newLineHandling;

	protected bool closeOutput;

	protected bool omitXmlDeclaration;

	protected string newLineChars;

	protected bool checkCharacters;

	protected XmlStandalone standalone;

	protected XmlOutputMethod outputMethod;

	protected bool autoXmlDeclaration;

	protected bool mergeCDataSections;

	private const int BUFSIZE = 6144;

	private const int ASYNCBUFSIZE = 65536;

	private const int OVERFLOW = 32;

	private const int INIT_MARKS_COUNT = 64;

	public override XmlWriterSettings Settings => new XmlWriterSettings
	{
		Encoding = encoding,
		OmitXmlDeclaration = omitXmlDeclaration,
		NewLineHandling = newLineHandling,
		NewLineChars = newLineChars,
		CloseOutput = closeOutput,
		ConformanceLevel = ConformanceLevel.Auto,
		CheckCharacters = checkCharacters,
		AutoXmlDeclaration = autoXmlDeclaration,
		Standalone = standalone,
		OutputMethod = outputMethod,
		ReadOnly = true
	};

	internal override bool SupportsNamespaceDeclarationInChunks => true;

	protected XmlUtf8RawTextWriter(XmlWriterSettings settings)
	{
		useAsync = settings.Async;
		newLineHandling = settings.NewLineHandling;
		omitXmlDeclaration = settings.OmitXmlDeclaration;
		newLineChars = settings.NewLineChars;
		checkCharacters = settings.CheckCharacters;
		closeOutput = settings.CloseOutput;
		standalone = settings.Standalone;
		outputMethod = settings.OutputMethod;
		mergeCDataSections = settings.MergeCDataSections;
		if (checkCharacters && newLineHandling == NewLineHandling.Replace)
		{
			ValidateContentChars(newLineChars, "NewLineChars", allowOnlyWhitespace: false);
		}
	}

	public XmlUtf8RawTextWriter(Stream stream, XmlWriterSettings settings)
		: this(settings)
	{
		this.stream = stream;
		encoding = settings.Encoding;
		if (settings.Async)
		{
			bufLen = 65536;
		}
		bufBytes = new byte[bufLen + 32];
		if (!stream.CanSeek || stream.Position == 0L)
		{
			byte[] preamble = encoding.GetPreamble();
			if (preamble.Length != 0)
			{
				Buffer.BlockCopy(preamble, 0, bufBytes, 1, preamble.Length);
				bufPos += preamble.Length;
				textPos += preamble.Length;
			}
		}
		if (settings.AutoXmlDeclaration)
		{
			WriteXmlDeclaration(standalone);
			autoXmlDeclaration = true;
		}
	}

	internal override void WriteXmlDeclaration(XmlStandalone standalone)
	{
		if (!omitXmlDeclaration && !autoXmlDeclaration)
		{
			RawText("<?xml version=\"");
			RawText("1.0");
			if (encoding != null)
			{
				RawText("\" encoding=\"");
				RawText(encoding.WebName);
			}
			if (standalone != XmlStandalone.Omit)
			{
				RawText("\" standalone=\"");
				RawText((standalone == XmlStandalone.Yes) ? "yes" : "no");
			}
			RawText("\"?>");
		}
	}

	internal override void WriteXmlDeclaration(string xmldecl)
	{
		if (!omitXmlDeclaration && !autoXmlDeclaration)
		{
			WriteProcessingInstruction("xml", xmldecl);
		}
	}

	public override void WriteDocType(string name, string pubid, string sysid, string subset)
	{
		RawText("<!DOCTYPE ");
		RawText(name);
		if (pubid != null)
		{
			RawText(" PUBLIC \"");
			RawText(pubid);
			RawText("\" \"");
			if (sysid != null)
			{
				RawText(sysid);
			}
			bufBytes[bufPos++] = 34;
		}
		else if (sysid != null)
		{
			RawText(" SYSTEM \"");
			RawText(sysid);
			bufBytes[bufPos++] = 34;
		}
		else
		{
			bufBytes[bufPos++] = 32;
		}
		if (subset != null)
		{
			bufBytes[bufPos++] = 91;
			RawText(subset);
			bufBytes[bufPos++] = 93;
		}
		bufBytes[bufPos++] = 62;
	}

	public override void WriteStartElement(string prefix, string localName, string ns)
	{
		bufBytes[bufPos++] = 60;
		if (prefix != null && prefix.Length != 0)
		{
			RawText(prefix);
			bufBytes[bufPos++] = 58;
		}
		RawText(localName);
		attrEndPos = bufPos;
	}

	internal override void StartElementContent()
	{
		bufBytes[bufPos++] = 62;
		contentPos = bufPos;
	}

	internal override void WriteEndElement(string prefix, string localName, string ns)
	{
		if (contentPos != bufPos)
		{
			bufBytes[bufPos++] = 60;
			bufBytes[bufPos++] = 47;
			if (prefix != null && prefix.Length != 0)
			{
				RawText(prefix);
				bufBytes[bufPos++] = 58;
			}
			RawText(localName);
			bufBytes[bufPos++] = 62;
		}
		else
		{
			bufPos--;
			bufBytes[bufPos++] = 32;
			bufBytes[bufPos++] = 47;
			bufBytes[bufPos++] = 62;
		}
	}

	internal override void WriteFullEndElement(string prefix, string localName, string ns)
	{
		bufBytes[bufPos++] = 60;
		bufBytes[bufPos++] = 47;
		if (prefix != null && prefix.Length != 0)
		{
			RawText(prefix);
			bufBytes[bufPos++] = 58;
		}
		RawText(localName);
		bufBytes[bufPos++] = 62;
	}

	public override void WriteStartAttribute(string prefix, string localName, string ns)
	{
		if (attrEndPos == bufPos)
		{
			bufBytes[bufPos++] = 32;
		}
		if (prefix != null && prefix.Length > 0)
		{
			RawText(prefix);
			bufBytes[bufPos++] = 58;
		}
		RawText(localName);
		bufBytes[bufPos++] = 61;
		bufBytes[bufPos++] = 34;
		inAttributeValue = true;
	}

	public override void WriteEndAttribute()
	{
		bufBytes[bufPos++] = 34;
		inAttributeValue = false;
		attrEndPos = bufPos;
	}

	internal override void WriteNamespaceDeclaration(string prefix, string namespaceName)
	{
		WriteStartNamespaceDeclaration(prefix);
		WriteString(namespaceName);
		WriteEndNamespaceDeclaration();
	}

	internal override void WriteStartNamespaceDeclaration(string prefix)
	{
		if (prefix.Length == 0)
		{
			RawText(" xmlns=\"");
		}
		else
		{
			RawText(" xmlns:");
			RawText(prefix);
			bufBytes[bufPos++] = 61;
			bufBytes[bufPos++] = 34;
		}
		inAttributeValue = true;
	}

	internal override void WriteEndNamespaceDeclaration()
	{
		inAttributeValue = false;
		bufBytes[bufPos++] = 34;
		attrEndPos = bufPos;
	}

	public override void WriteCData(string text)
	{
		if (mergeCDataSections && bufPos == cdataPos)
		{
			bufPos -= 3;
		}
		else
		{
			bufBytes[bufPos++] = 60;
			bufBytes[bufPos++] = 33;
			bufBytes[bufPos++] = 91;
			bufBytes[bufPos++] = 67;
			bufBytes[bufPos++] = 68;
			bufBytes[bufPos++] = 65;
			bufBytes[bufPos++] = 84;
			bufBytes[bufPos++] = 65;
			bufBytes[bufPos++] = 91;
		}
		WriteCDataSection(text);
		bufBytes[bufPos++] = 93;
		bufBytes[bufPos++] = 93;
		bufBytes[bufPos++] = 62;
		textPos = bufPos;
		cdataPos = bufPos;
	}

	public override void WriteComment(string text)
	{
		bufBytes[bufPos++] = 60;
		bufBytes[bufPos++] = 33;
		bufBytes[bufPos++] = 45;
		bufBytes[bufPos++] = 45;
		WriteCommentOrPi(text, 45);
		bufBytes[bufPos++] = 45;
		bufBytes[bufPos++] = 45;
		bufBytes[bufPos++] = 62;
	}

	public override void WriteProcessingInstruction(string name, string text)
	{
		bufBytes[bufPos++] = 60;
		bufBytes[bufPos++] = 63;
		RawText(name);
		if (text.Length > 0)
		{
			bufBytes[bufPos++] = 32;
			WriteCommentOrPi(text, 63);
		}
		bufBytes[bufPos++] = 63;
		bufBytes[bufPos++] = 62;
	}

	public override void WriteEntityRef(string name)
	{
		bufBytes[bufPos++] = 38;
		RawText(name);
		bufBytes[bufPos++] = 59;
		if (bufPos > bufLen)
		{
			FlushBuffer();
		}
		textPos = bufPos;
	}

	public override void WriteCharEntity(char ch)
	{
		int num = ch;
		string s = num.ToString("X", NumberFormatInfo.InvariantInfo);
		if (checkCharacters && !xmlCharType.IsCharData(ch))
		{
			throw XmlConvert.CreateInvalidCharException(ch, '\0');
		}
		bufBytes[bufPos++] = 38;
		bufBytes[bufPos++] = 35;
		bufBytes[bufPos++] = 120;
		RawText(s);
		bufBytes[bufPos++] = 59;
		if (bufPos > bufLen)
		{
			FlushBuffer();
		}
		textPos = bufPos;
	}

	public unsafe override void WriteWhitespace(string ws)
	{
		fixed (char* ptr = ws)
		{
			char* pSrcEnd = ptr + ws.Length;
			if (inAttributeValue)
			{
				WriteAttributeTextBlock(ptr, pSrcEnd);
			}
			else
			{
				WriteElementTextBlock(ptr, pSrcEnd);
			}
		}
	}

	public unsafe override void WriteString(string text)
	{
		fixed (char* ptr = text)
		{
			char* pSrcEnd = ptr + text.Length;
			if (inAttributeValue)
			{
				WriteAttributeTextBlock(ptr, pSrcEnd);
			}
			else
			{
				WriteElementTextBlock(ptr, pSrcEnd);
			}
		}
	}

	public override void WriteSurrogateCharEntity(char lowChar, char highChar)
	{
		int num = XmlCharType.CombineSurrogateChar(lowChar, highChar);
		bufBytes[bufPos++] = 38;
		bufBytes[bufPos++] = 35;
		bufBytes[bufPos++] = 120;
		RawText(num.ToString("X", NumberFormatInfo.InvariantInfo));
		bufBytes[bufPos++] = 59;
		textPos = bufPos;
	}

	public unsafe override void WriteChars(char[] buffer, int index, int count)
	{
		fixed (char* ptr = &buffer[index])
		{
			if (inAttributeValue)
			{
				WriteAttributeTextBlock(ptr, ptr + count);
			}
			else
			{
				WriteElementTextBlock(ptr, ptr + count);
			}
		}
	}

	public unsafe override void WriteRaw(char[] buffer, int index, int count)
	{
		fixed (char* ptr = &buffer[index])
		{
			WriteRawWithCharChecking(ptr, ptr + count);
		}
		textPos = bufPos;
	}

	public unsafe override void WriteRaw(string data)
	{
		fixed (char* ptr = data)
		{
			WriteRawWithCharChecking(ptr, ptr + data.Length);
		}
		textPos = bufPos;
	}

	public override void Close()
	{
		try
		{
			FlushBuffer();
			FlushEncoder();
		}
		finally
		{
			writeToNull = true;
			if (stream != null)
			{
				try
				{
					stream.Flush();
				}
				finally
				{
					try
					{
						if (closeOutput)
						{
							stream.Close();
						}
					}
					finally
					{
						stream = null;
					}
				}
			}
		}
	}

	public override void Flush()
	{
		FlushBuffer();
		FlushEncoder();
		if (stream != null)
		{
			stream.Flush();
		}
	}

	protected virtual void FlushBuffer()
	{
		try
		{
			if (!writeToNull)
			{
				stream.Write(bufBytes, 1, bufPos - 1);
			}
		}
		catch
		{
			writeToNull = true;
			throw;
		}
		finally
		{
			bufBytes[0] = bufBytes[bufPos - 1];
			if (IsSurrogateByte(bufBytes[0]))
			{
				bufBytes[1] = bufBytes[bufPos];
				bufBytes[2] = bufBytes[bufPos + 1];
				bufBytes[3] = bufBytes[bufPos + 2];
			}
			textPos = ((textPos == bufPos) ? 1 : 0);
			attrEndPos = ((attrEndPos == bufPos) ? 1 : 0);
			contentPos = 0;
			cdataPos = 0;
			bufPos = 1;
		}
	}

	private void FlushEncoder()
	{
	}

	protected unsafe void WriteAttributeTextBlock(char* pSrc, char* pSrcEnd)
	{
		fixed (byte* ptr = bufBytes)
		{
			byte* ptr2 = ptr + bufPos;
			int num = 0;
			while (true)
			{
				byte* ptr3 = ptr2 + (pSrcEnd - pSrc);
				if (ptr3 > ptr + bufLen)
				{
					ptr3 = ptr + bufLen;
				}
				while (ptr2 < ptr3 && (xmlCharType.charProperties[num = *pSrc] & 0x80) != 0 && num <= 127)
				{
					*ptr2 = (byte)num;
					ptr2++;
					pSrc++;
				}
				if (pSrc >= pSrcEnd)
				{
					break;
				}
				if (ptr2 >= ptr3)
				{
					bufPos = (int)(ptr2 - ptr);
					FlushBuffer();
					ptr2 = ptr + 1;
					continue;
				}
				switch (num)
				{
				case 38:
					ptr2 = AmpEntity(ptr2);
					break;
				case 60:
					ptr2 = LtEntity(ptr2);
					break;
				case 62:
					ptr2 = GtEntity(ptr2);
					break;
				case 34:
					ptr2 = QuoteEntity(ptr2);
					break;
				case 39:
					*ptr2 = (byte)num;
					ptr2++;
					break;
				case 9:
					if (newLineHandling == NewLineHandling.None)
					{
						*ptr2 = (byte)num;
						ptr2++;
					}
					else
					{
						ptr2 = TabEntity(ptr2);
					}
					break;
				case 13:
					if (newLineHandling == NewLineHandling.None)
					{
						*ptr2 = (byte)num;
						ptr2++;
					}
					else
					{
						ptr2 = CarriageReturnEntity(ptr2);
					}
					break;
				case 10:
					if (newLineHandling == NewLineHandling.None)
					{
						*ptr2 = (byte)num;
						ptr2++;
					}
					else
					{
						ptr2 = LineFeedEntity(ptr2);
					}
					break;
				default:
					if (XmlCharType.IsSurrogate(num))
					{
						ptr2 = EncodeSurrogate(pSrc, pSrcEnd, ptr2);
						pSrc += 2;
					}
					else if (num <= 127 || num >= 65534)
					{
						ptr2 = InvalidXmlChar(num, ptr2, entitize: true);
						pSrc++;
					}
					else
					{
						ptr2 = EncodeMultibyteUTF8(num, ptr2);
						pSrc++;
					}
					continue;
				}
				pSrc++;
			}
			bufPos = (int)(ptr2 - ptr);
		}
	}

	protected unsafe void WriteElementTextBlock(char* pSrc, char* pSrcEnd)
	{
		fixed (byte* ptr = bufBytes)
		{
			byte* ptr2 = ptr + bufPos;
			int num = 0;
			while (true)
			{
				byte* ptr3 = ptr2 + (pSrcEnd - pSrc);
				if (ptr3 > ptr + bufLen)
				{
					ptr3 = ptr + bufLen;
				}
				while (ptr2 < ptr3 && (xmlCharType.charProperties[num = *pSrc] & 0x80) != 0 && num <= 127)
				{
					*ptr2 = (byte)num;
					ptr2++;
					pSrc++;
				}
				if (pSrc >= pSrcEnd)
				{
					break;
				}
				if (ptr2 >= ptr3)
				{
					bufPos = (int)(ptr2 - ptr);
					FlushBuffer();
					ptr2 = ptr + 1;
					continue;
				}
				switch (num)
				{
				case 38:
					ptr2 = AmpEntity(ptr2);
					break;
				case 60:
					ptr2 = LtEntity(ptr2);
					break;
				case 62:
					ptr2 = GtEntity(ptr2);
					break;
				case 9:
				case 34:
				case 39:
					*ptr2 = (byte)num;
					ptr2++;
					break;
				case 10:
					if (newLineHandling == NewLineHandling.Replace)
					{
						ptr2 = WriteNewLine(ptr2);
						break;
					}
					*ptr2 = (byte)num;
					ptr2++;
					break;
				case 13:
					switch (newLineHandling)
					{
					case NewLineHandling.Replace:
						if (pSrc[1] == '\n')
						{
							pSrc++;
						}
						ptr2 = WriteNewLine(ptr2);
						break;
					case NewLineHandling.Entitize:
						ptr2 = CarriageReturnEntity(ptr2);
						break;
					case NewLineHandling.None:
						*ptr2 = (byte)num;
						ptr2++;
						break;
					}
					break;
				default:
					if (XmlCharType.IsSurrogate(num))
					{
						ptr2 = EncodeSurrogate(pSrc, pSrcEnd, ptr2);
						pSrc += 2;
					}
					else if (num <= 127 || num >= 65534)
					{
						ptr2 = InvalidXmlChar(num, ptr2, entitize: true);
						pSrc++;
					}
					else
					{
						ptr2 = EncodeMultibyteUTF8(num, ptr2);
						pSrc++;
					}
					continue;
				}
				pSrc++;
			}
			bufPos = (int)(ptr2 - ptr);
			textPos = bufPos;
			contentPos = 0;
		}
	}

	protected unsafe void RawText(string s)
	{
		fixed (char* ptr = s)
		{
			RawText(ptr, ptr + s.Length);
		}
	}

	protected unsafe void RawText(char* pSrcBegin, char* pSrcEnd)
	{
		fixed (byte* ptr = bufBytes)
		{
			byte* ptr2 = ptr + bufPos;
			char* ptr3 = pSrcBegin;
			int num = 0;
			while (true)
			{
				byte* ptr4 = ptr2 + (pSrcEnd - ptr3);
				if (ptr4 > ptr + bufLen)
				{
					ptr4 = ptr + bufLen;
				}
				for (; ptr2 < ptr4; ptr2++)
				{
					if ((num = *ptr3) > 127)
					{
						break;
					}
					ptr3++;
					*ptr2 = (byte)num;
				}
				if (ptr3 >= pSrcEnd)
				{
					break;
				}
				if (ptr2 >= ptr4)
				{
					bufPos = (int)(ptr2 - ptr);
					FlushBuffer();
					ptr2 = ptr + 1;
				}
				else if (XmlCharType.IsSurrogate(num))
				{
					ptr2 = EncodeSurrogate(ptr3, pSrcEnd, ptr2);
					ptr3 += 2;
				}
				else if (num <= 127 || num >= 65534)
				{
					ptr2 = InvalidXmlChar(num, ptr2, entitize: false);
					ptr3++;
				}
				else
				{
					ptr2 = EncodeMultibyteUTF8(num, ptr2);
					ptr3++;
				}
			}
			bufPos = (int)(ptr2 - ptr);
		}
	}

	protected unsafe void WriteRawWithCharChecking(char* pSrcBegin, char* pSrcEnd)
	{
		fixed (byte* ptr = bufBytes)
		{
			char* ptr2 = pSrcBegin;
			byte* ptr3 = ptr + bufPos;
			int num = 0;
			while (true)
			{
				byte* ptr4 = ptr3 + (pSrcEnd - ptr2);
				if (ptr4 > ptr + bufLen)
				{
					ptr4 = ptr + bufLen;
				}
				while (ptr3 < ptr4 && (xmlCharType.charProperties[num = *ptr2] & 0x40) != 0 && num <= 127)
				{
					*ptr3 = (byte)num;
					ptr3++;
					ptr2++;
				}
				if (ptr2 >= pSrcEnd)
				{
					break;
				}
				if (ptr3 >= ptr4)
				{
					bufPos = (int)(ptr3 - ptr);
					FlushBuffer();
					ptr3 = ptr + 1;
					continue;
				}
				switch (num)
				{
				case 9:
				case 38:
				case 60:
				case 93:
					*ptr3 = (byte)num;
					ptr3++;
					break;
				case 13:
					if (newLineHandling == NewLineHandling.Replace)
					{
						if (ptr2[1] == '\n')
						{
							ptr2++;
						}
						ptr3 = WriteNewLine(ptr3);
					}
					else
					{
						*ptr3 = (byte)num;
						ptr3++;
					}
					break;
				case 10:
					if (newLineHandling == NewLineHandling.Replace)
					{
						ptr3 = WriteNewLine(ptr3);
						break;
					}
					*ptr3 = (byte)num;
					ptr3++;
					break;
				default:
					if (XmlCharType.IsSurrogate(num))
					{
						ptr3 = EncodeSurrogate(ptr2, pSrcEnd, ptr3);
						ptr2 += 2;
					}
					else if (num <= 127 || num >= 65534)
					{
						ptr3 = InvalidXmlChar(num, ptr3, entitize: false);
						ptr2++;
					}
					else
					{
						ptr3 = EncodeMultibyteUTF8(num, ptr3);
						ptr2++;
					}
					continue;
				}
				ptr2++;
			}
			bufPos = (int)(ptr3 - ptr);
		}
	}

	protected unsafe void WriteCommentOrPi(string text, int stopChar)
	{
		if (text.Length == 0)
		{
			if (bufPos >= bufLen)
			{
				FlushBuffer();
			}
			return;
		}
		fixed (char* ptr = text)
		{
			fixed (byte* ptr2 = bufBytes)
			{
				char* ptr3 = ptr;
				char* ptr4 = ptr + text.Length;
				byte* ptr5 = ptr2 + bufPos;
				int num = 0;
				while (true)
				{
					byte* ptr6 = ptr5 + (ptr4 - ptr3);
					if (ptr6 > ptr2 + bufLen)
					{
						ptr6 = ptr2 + bufLen;
					}
					while (ptr5 < ptr6 && (xmlCharType.charProperties[num = *ptr3] & 0x40) != 0 && num != stopChar && num <= 127)
					{
						*ptr5 = (byte)num;
						ptr5++;
						ptr3++;
					}
					if (ptr3 >= ptr4)
					{
						break;
					}
					if (ptr5 >= ptr6)
					{
						bufPos = (int)(ptr5 - ptr2);
						FlushBuffer();
						ptr5 = ptr2 + 1;
						continue;
					}
					switch (num)
					{
					case 45:
						*ptr5 = 45;
						ptr5++;
						if (num == stopChar && (ptr3 + 1 == ptr4 || ptr3[1] == '-'))
						{
							*ptr5 = 32;
							ptr5++;
						}
						break;
					case 63:
						*ptr5 = 63;
						ptr5++;
						if (num == stopChar && ptr3 + 1 < ptr4 && ptr3[1] == '>')
						{
							*ptr5 = 32;
							ptr5++;
						}
						break;
					case 93:
						*ptr5 = 93;
						ptr5++;
						break;
					case 13:
						if (newLineHandling == NewLineHandling.Replace)
						{
							if (ptr3[1] == '\n')
							{
								ptr3++;
							}
							ptr5 = WriteNewLine(ptr5);
						}
						else
						{
							*ptr5 = (byte)num;
							ptr5++;
						}
						break;
					case 10:
						if (newLineHandling == NewLineHandling.Replace)
						{
							ptr5 = WriteNewLine(ptr5);
							break;
						}
						*ptr5 = (byte)num;
						ptr5++;
						break;
					case 9:
					case 38:
					case 60:
						*ptr5 = (byte)num;
						ptr5++;
						break;
					default:
						if (XmlCharType.IsSurrogate(num))
						{
							ptr5 = EncodeSurrogate(ptr3, ptr4, ptr5);
							ptr3 += 2;
						}
						else if (num <= 127 || num >= 65534)
						{
							ptr5 = InvalidXmlChar(num, ptr5, entitize: false);
							ptr3++;
						}
						else
						{
							ptr5 = EncodeMultibyteUTF8(num, ptr5);
							ptr3++;
						}
						continue;
					}
					ptr3++;
				}
				bufPos = (int)(ptr5 - ptr2);
			}
		}
	}

	protected unsafe void WriteCDataSection(string text)
	{
		if (text.Length == 0)
		{
			if (bufPos >= bufLen)
			{
				FlushBuffer();
			}
			return;
		}
		fixed (char* ptr = text)
		{
			fixed (byte* ptr2 = bufBytes)
			{
				char* ptr3 = ptr;
				char* ptr4 = ptr + text.Length;
				byte* ptr5 = ptr2 + bufPos;
				int num = 0;
				while (true)
				{
					byte* ptr6 = ptr5 + (ptr4 - ptr3);
					if (ptr6 > ptr2 + bufLen)
					{
						ptr6 = ptr2 + bufLen;
					}
					while (ptr5 < ptr6 && (xmlCharType.charProperties[num = *ptr3] & 0x80) != 0 && num != 93 && num <= 127)
					{
						*ptr5 = (byte)num;
						ptr5++;
						ptr3++;
					}
					if (ptr3 >= ptr4)
					{
						break;
					}
					if (ptr5 >= ptr6)
					{
						bufPos = (int)(ptr5 - ptr2);
						FlushBuffer();
						ptr5 = ptr2 + 1;
						continue;
					}
					switch (num)
					{
					case 62:
						if (hadDoubleBracket && ptr5[-1] == 93)
						{
							ptr5 = RawEndCData(ptr5);
							ptr5 = RawStartCData(ptr5);
						}
						*ptr5 = 62;
						ptr5++;
						break;
					case 93:
						if (ptr5[-1] == 93)
						{
							hadDoubleBracket = true;
						}
						else
						{
							hadDoubleBracket = false;
						}
						*ptr5 = 93;
						ptr5++;
						break;
					case 13:
						if (newLineHandling == NewLineHandling.Replace)
						{
							if (ptr3[1] == '\n')
							{
								ptr3++;
							}
							ptr5 = WriteNewLine(ptr5);
						}
						else
						{
							*ptr5 = (byte)num;
							ptr5++;
						}
						break;
					case 10:
						if (newLineHandling == NewLineHandling.Replace)
						{
							ptr5 = WriteNewLine(ptr5);
							break;
						}
						*ptr5 = (byte)num;
						ptr5++;
						break;
					case 9:
					case 34:
					case 38:
					case 39:
					case 60:
						*ptr5 = (byte)num;
						ptr5++;
						break;
					default:
						if (XmlCharType.IsSurrogate(num))
						{
							ptr5 = EncodeSurrogate(ptr3, ptr4, ptr5);
							ptr3 += 2;
						}
						else if (num <= 127 || num >= 65534)
						{
							ptr5 = InvalidXmlChar(num, ptr5, entitize: false);
							ptr3++;
						}
						else
						{
							ptr5 = EncodeMultibyteUTF8(num, ptr5);
							ptr3++;
						}
						continue;
					}
					ptr3++;
				}
				bufPos = (int)(ptr5 - ptr2);
			}
		}
	}

	private static bool IsSurrogateByte(byte b)
	{
		return (b & 0xF8) == 240;
	}

	private unsafe static byte* EncodeSurrogate(char* pSrc, char* pSrcEnd, byte* pDst)
	{
		int num = *pSrc;
		if (num <= 56319)
		{
			if (pSrc + 1 < pSrcEnd)
			{
				int num2 = pSrc[1];
				if (num2 >= 56320 && (System.LocalAppContextSwitches.DontThrowOnInvalidSurrogatePairs || num2 <= 57343))
				{
					num = XmlCharType.CombineSurrogateChar(num2, num);
					*pDst = (byte)(0xF0 | (num >> 18));
					pDst[1] = (byte)(0x80 | ((num >> 12) & 0x3F));
					pDst[2] = (byte)(0x80 | ((num >> 6) & 0x3F));
					pDst[3] = (byte)(0x80 | (num & 0x3F));
					pDst += 4;
					return pDst;
				}
				throw XmlConvert.CreateInvalidSurrogatePairException((char)num2, (char)num);
			}
			throw new ArgumentException(Res.GetString("The surrogate pair is invalid. Missing a low surrogate character."));
		}
		throw XmlConvert.CreateInvalidHighSurrogateCharException((char)num);
	}

	private unsafe byte* InvalidXmlChar(int ch, byte* pDst, bool entitize)
	{
		if (checkCharacters)
		{
			throw XmlConvert.CreateInvalidCharException((char)ch, '\0');
		}
		if (entitize)
		{
			return CharEntity(pDst, (char)ch);
		}
		if (ch < 128)
		{
			*pDst = (byte)ch;
			pDst++;
		}
		else
		{
			pDst = EncodeMultibyteUTF8(ch, pDst);
		}
		return pDst;
	}

	internal unsafe void EncodeChar(ref char* pSrc, char* pSrcEnd, ref byte* pDst)
	{
		int num = *pSrc;
		if (XmlCharType.IsSurrogate(num))
		{
			pDst = EncodeSurrogate(pSrc, pSrcEnd, pDst);
			pSrc += 2;
		}
		else if (num <= 127 || num >= 65534)
		{
			pDst = InvalidXmlChar(num, pDst, entitize: false);
			pSrc++;
		}
		else
		{
			pDst = EncodeMultibyteUTF8(num, pDst);
			pSrc++;
		}
	}

	internal unsafe static byte* EncodeMultibyteUTF8(int ch, byte* pDst)
	{
		if (ch < 2048)
		{
			*pDst = (byte)(-64 | (ch >> 6));
		}
		else
		{
			*pDst = (byte)(-32 | (ch >> 12));
			pDst++;
			*pDst = (byte)(-128 | ((ch >> 6) & 0x3F));
		}
		pDst++;
		*pDst = (byte)(0x80 | (ch & 0x3F));
		return pDst + 1;
	}

	internal unsafe static void CharToUTF8(ref char* pSrc, char* pSrcEnd, ref byte* pDst)
	{
		int num = *pSrc;
		if (num <= 127)
		{
			*pDst = (byte)num;
			pDst++;
			pSrc++;
		}
		else if (XmlCharType.IsSurrogate(num))
		{
			pDst = EncodeSurrogate(pSrc, pSrcEnd, pDst);
			pSrc += 2;
		}
		else
		{
			pDst = EncodeMultibyteUTF8(num, pDst);
			pSrc++;
		}
	}

	protected unsafe byte* WriteNewLine(byte* pDst)
	{
		fixed (byte* ptr = bufBytes)
		{
			bufPos = (int)(pDst - ptr);
			RawText(newLineChars);
			return ptr + bufPos;
		}
	}

	protected unsafe static byte* LtEntity(byte* pDst)
	{
		*pDst = 38;
		pDst[1] = 108;
		pDst[2] = 116;
		pDst[3] = 59;
		return pDst + 4;
	}

	protected unsafe static byte* GtEntity(byte* pDst)
	{
		*pDst = 38;
		pDst[1] = 103;
		pDst[2] = 116;
		pDst[3] = 59;
		return pDst + 4;
	}

	protected unsafe static byte* AmpEntity(byte* pDst)
	{
		*pDst = 38;
		pDst[1] = 97;
		pDst[2] = 109;
		pDst[3] = 112;
		pDst[4] = 59;
		return pDst + 5;
	}

	protected unsafe static byte* QuoteEntity(byte* pDst)
	{
		*pDst = 38;
		pDst[1] = 113;
		pDst[2] = 117;
		pDst[3] = 111;
		pDst[4] = 116;
		pDst[5] = 59;
		return pDst + 6;
	}

	protected unsafe static byte* TabEntity(byte* pDst)
	{
		*pDst = 38;
		pDst[1] = 35;
		pDst[2] = 120;
		pDst[3] = 57;
		pDst[4] = 59;
		return pDst + 5;
	}

	protected unsafe static byte* LineFeedEntity(byte* pDst)
	{
		*pDst = 38;
		pDst[1] = 35;
		pDst[2] = 120;
		pDst[3] = 65;
		pDst[4] = 59;
		return pDst + 5;
	}

	protected unsafe static byte* CarriageReturnEntity(byte* pDst)
	{
		*pDst = 38;
		pDst[1] = 35;
		pDst[2] = 120;
		pDst[3] = 68;
		pDst[4] = 59;
		return pDst + 5;
	}

	private unsafe static byte* CharEntity(byte* pDst, char ch)
	{
		int num = ch;
		string text = num.ToString("X", NumberFormatInfo.InvariantInfo);
		*pDst = 38;
		pDst[1] = 35;
		pDst[2] = 120;
		pDst += 3;
		fixed (char* ptr = text)
		{
			char* ptr2 = ptr;
			while ((*(pDst++) = (byte)(*(ptr2++))) != 0)
			{
			}
		}
		pDst[-1] = 59;
		return pDst;
	}

	protected unsafe static byte* RawStartCData(byte* pDst)
	{
		*pDst = 60;
		pDst[1] = 33;
		pDst[2] = 91;
		pDst[3] = 67;
		pDst[4] = 68;
		pDst[5] = 65;
		pDst[6] = 84;
		pDst[7] = 65;
		pDst[8] = 91;
		return pDst + 9;
	}

	protected unsafe static byte* RawEndCData(byte* pDst)
	{
		*pDst = 93;
		pDst[1] = 93;
		pDst[2] = 62;
		return pDst + 3;
	}

	protected void ValidateContentChars(string chars, string propertyName, bool allowOnlyWhitespace)
	{
		if (allowOnlyWhitespace)
		{
			if (!xmlCharType.IsOnlyWhitespace(chars))
			{
				throw new ArgumentException(Res.GetString("XmlWriterSettings.{0} can contain only valid XML white space characters when XmlWriterSettings.CheckCharacters and XmlWriterSettings.NewLineOnAttributes are true.", propertyName));
			}
			return;
		}
		string text = null;
		int num = 0;
		object[] args;
		while (true)
		{
			if (num >= chars.Length)
			{
				return;
			}
			if (!xmlCharType.IsTextChar(chars[num]))
			{
				switch (chars[num])
				{
				case '&':
				case '<':
				case ']':
					args = XmlException.BuildCharExceptionArgs(chars, num);
					text = Res.GetString("'{0}', hexadecimal value {1}, is an invalid character.", args);
					break;
				default:
					if (XmlCharType.IsHighSurrogate(chars[num]))
					{
						if (num + 1 < chars.Length && XmlCharType.IsLowSurrogate(chars[num + 1]))
						{
							num++;
							goto IL_011c;
						}
						text = Res.GetString("The surrogate pair is invalid. Missing a low surrogate character.");
					}
					else
					{
						if (!XmlCharType.IsLowSurrogate(chars[num]))
						{
							goto IL_011c;
						}
						text = Res.GetString("Invalid high surrogate character (0x{0}). A high surrogate character must have a value from range (0xD800 - 0xDBFF).", ((uint)chars[num]).ToString("X", CultureInfo.InvariantCulture));
					}
					break;
				case '\t':
				case '\n':
				case '\r':
					goto IL_011c;
				}
				break;
			}
			goto IL_011c;
			IL_011c:
			num++;
		}
		args = new string[2] { propertyName, text };
		throw new ArgumentException(Res.GetString("XmlWriterSettings.{0} can contain only valid XML text content characters when XmlWriterSettings.CheckCharacters is true. {1}", args));
	}

	protected void CheckAsyncCall()
	{
		if (!useAsync)
		{
			throw new InvalidOperationException(Res.GetString("Set XmlWriterSettings.Async to true if you want to use Async Methods."));
		}
	}

	internal override async Task WriteXmlDeclarationAsync(XmlStandalone standalone)
	{
		CheckAsyncCall();
		if (!omitXmlDeclaration && !autoXmlDeclaration)
		{
			await RawTextAsync("<?xml version=\"").ConfigureAwait(continueOnCapturedContext: false);
			await RawTextAsync("1.0").ConfigureAwait(continueOnCapturedContext: false);
			if (encoding != null)
			{
				await RawTextAsync("\" encoding=\"").ConfigureAwait(continueOnCapturedContext: false);
				await RawTextAsync(encoding.WebName).ConfigureAwait(continueOnCapturedContext: false);
			}
			if (standalone != XmlStandalone.Omit)
			{
				await RawTextAsync("\" standalone=\"").ConfigureAwait(continueOnCapturedContext: false);
				await RawTextAsync((standalone == XmlStandalone.Yes) ? "yes" : "no").ConfigureAwait(continueOnCapturedContext: false);
			}
			await RawTextAsync("\"?>").ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	internal override Task WriteXmlDeclarationAsync(string xmldecl)
	{
		CheckAsyncCall();
		if (!omitXmlDeclaration && !autoXmlDeclaration)
		{
			return WriteProcessingInstructionAsync("xml", xmldecl);
		}
		return AsyncHelper.DoneTask;
	}

	public override async Task WriteDocTypeAsync(string name, string pubid, string sysid, string subset)
	{
		CheckAsyncCall();
		await RawTextAsync("<!DOCTYPE ").ConfigureAwait(continueOnCapturedContext: false);
		await RawTextAsync(name).ConfigureAwait(continueOnCapturedContext: false);
		if (pubid != null)
		{
			await RawTextAsync(" PUBLIC \"").ConfigureAwait(continueOnCapturedContext: false);
			await RawTextAsync(pubid).ConfigureAwait(continueOnCapturedContext: false);
			await RawTextAsync("\" \"").ConfigureAwait(continueOnCapturedContext: false);
			if (sysid != null)
			{
				await RawTextAsync(sysid).ConfigureAwait(continueOnCapturedContext: false);
			}
			bufBytes[bufPos++] = 34;
		}
		else if (sysid != null)
		{
			await RawTextAsync(" SYSTEM \"").ConfigureAwait(continueOnCapturedContext: false);
			await RawTextAsync(sysid).ConfigureAwait(continueOnCapturedContext: false);
			bufBytes[bufPos++] = 34;
		}
		else
		{
			bufBytes[bufPos++] = 32;
		}
		if (subset != null)
		{
			bufBytes[bufPos++] = 91;
			await RawTextAsync(subset).ConfigureAwait(continueOnCapturedContext: false);
			bufBytes[bufPos++] = 93;
		}
		bufBytes[bufPos++] = 62;
	}

	public override Task WriteStartElementAsync(string prefix, string localName, string ns)
	{
		CheckAsyncCall();
		bufBytes[bufPos++] = 60;
		Task task = ((prefix == null || prefix.Length == 0) ? RawTextAsync(localName) : RawTextAsync(prefix + ":" + localName));
		return task.CallVoidFuncWhenFinish(WriteStartElementAsync_SetAttEndPos);
	}

	private void WriteStartElementAsync_SetAttEndPos()
	{
		attrEndPos = bufPos;
	}

	internal override Task WriteEndElementAsync(string prefix, string localName, string ns)
	{
		CheckAsyncCall();
		if (contentPos != bufPos)
		{
			bufBytes[bufPos++] = 60;
			bufBytes[bufPos++] = 47;
			if (prefix != null && prefix.Length != 0)
			{
				return RawTextAsync(prefix + ":" + localName + ">");
			}
			return RawTextAsync(localName + ">");
		}
		bufPos--;
		bufBytes[bufPos++] = 32;
		bufBytes[bufPos++] = 47;
		bufBytes[bufPos++] = 62;
		return AsyncHelper.DoneTask;
	}

	internal override Task WriteFullEndElementAsync(string prefix, string localName, string ns)
	{
		CheckAsyncCall();
		bufBytes[bufPos++] = 60;
		bufBytes[bufPos++] = 47;
		if (prefix != null && prefix.Length != 0)
		{
			return RawTextAsync(prefix + ":" + localName + ">");
		}
		return RawTextAsync(localName + ">");
	}

	protected internal override Task WriteStartAttributeAsync(string prefix, string localName, string ns)
	{
		CheckAsyncCall();
		if (attrEndPos == bufPos)
		{
			bufBytes[bufPos++] = 32;
		}
		Task task = ((prefix == null || prefix.Length <= 0) ? RawTextAsync(localName + "=\"") : RawTextAsync(prefix + ":" + localName + "=\""));
		return task.CallVoidFuncWhenFinish(WriteStartAttribute_SetInAttribute);
	}

	private void WriteStartAttribute_SetInAttribute()
	{
		inAttributeValue = true;
	}

	protected internal override Task WriteEndAttributeAsync()
	{
		CheckAsyncCall();
		bufBytes[bufPos++] = 34;
		inAttributeValue = false;
		attrEndPos = bufPos;
		return AsyncHelper.DoneTask;
	}

	internal override async Task WriteNamespaceDeclarationAsync(string prefix, string namespaceName)
	{
		CheckAsyncCall();
		await WriteStartNamespaceDeclarationAsync(prefix).ConfigureAwait(continueOnCapturedContext: false);
		await WriteStringAsync(namespaceName).ConfigureAwait(continueOnCapturedContext: false);
		await WriteEndNamespaceDeclarationAsync().ConfigureAwait(continueOnCapturedContext: false);
	}

	internal override async Task WriteStartNamespaceDeclarationAsync(string prefix)
	{
		CheckAsyncCall();
		if (prefix.Length == 0)
		{
			await RawTextAsync(" xmlns=\"").ConfigureAwait(continueOnCapturedContext: false);
		}
		else
		{
			await RawTextAsync(" xmlns:").ConfigureAwait(continueOnCapturedContext: false);
			await RawTextAsync(prefix).ConfigureAwait(continueOnCapturedContext: false);
			bufBytes[bufPos++] = 61;
			bufBytes[bufPos++] = 34;
		}
		inAttributeValue = true;
	}

	internal override Task WriteEndNamespaceDeclarationAsync()
	{
		CheckAsyncCall();
		inAttributeValue = false;
		bufBytes[bufPos++] = 34;
		attrEndPos = bufPos;
		return AsyncHelper.DoneTask;
	}

	public override async Task WriteCDataAsync(string text)
	{
		CheckAsyncCall();
		if (mergeCDataSections && bufPos == cdataPos)
		{
			bufPos -= 3;
		}
		else
		{
			bufBytes[bufPos++] = 60;
			bufBytes[bufPos++] = 33;
			bufBytes[bufPos++] = 91;
			bufBytes[bufPos++] = 67;
			bufBytes[bufPos++] = 68;
			bufBytes[bufPos++] = 65;
			bufBytes[bufPos++] = 84;
			bufBytes[bufPos++] = 65;
			bufBytes[bufPos++] = 91;
		}
		await WriteCDataSectionAsync(text).ConfigureAwait(continueOnCapturedContext: false);
		bufBytes[bufPos++] = 93;
		bufBytes[bufPos++] = 93;
		bufBytes[bufPos++] = 62;
		textPos = bufPos;
		cdataPos = bufPos;
	}

	public override async Task WriteCommentAsync(string text)
	{
		CheckAsyncCall();
		bufBytes[bufPos++] = 60;
		bufBytes[bufPos++] = 33;
		bufBytes[bufPos++] = 45;
		bufBytes[bufPos++] = 45;
		await WriteCommentOrPiAsync(text, 45).ConfigureAwait(continueOnCapturedContext: false);
		bufBytes[bufPos++] = 45;
		bufBytes[bufPos++] = 45;
		bufBytes[bufPos++] = 62;
	}

	public override async Task WriteProcessingInstructionAsync(string name, string text)
	{
		CheckAsyncCall();
		bufBytes[bufPos++] = 60;
		bufBytes[bufPos++] = 63;
		await RawTextAsync(name).ConfigureAwait(continueOnCapturedContext: false);
		if (text.Length > 0)
		{
			bufBytes[bufPos++] = 32;
			await WriteCommentOrPiAsync(text, 63).ConfigureAwait(continueOnCapturedContext: false);
		}
		bufBytes[bufPos++] = 63;
		bufBytes[bufPos++] = 62;
	}

	public override async Task WriteEntityRefAsync(string name)
	{
		CheckAsyncCall();
		bufBytes[bufPos++] = 38;
		await RawTextAsync(name).ConfigureAwait(continueOnCapturedContext: false);
		bufBytes[bufPos++] = 59;
		if (bufPos > bufLen)
		{
			await FlushBufferAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		textPos = bufPos;
	}

	public override async Task WriteCharEntityAsync(char ch)
	{
		CheckAsyncCall();
		int num = ch;
		string text = num.ToString("X", NumberFormatInfo.InvariantInfo);
		if (checkCharacters && !xmlCharType.IsCharData(ch))
		{
			throw XmlConvert.CreateInvalidCharException(ch, '\0');
		}
		bufBytes[bufPos++] = 38;
		bufBytes[bufPos++] = 35;
		bufBytes[bufPos++] = 120;
		await RawTextAsync(text).ConfigureAwait(continueOnCapturedContext: false);
		bufBytes[bufPos++] = 59;
		if (bufPos > bufLen)
		{
			await FlushBufferAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		textPos = bufPos;
	}

	public override Task WriteWhitespaceAsync(string ws)
	{
		CheckAsyncCall();
		if (inAttributeValue)
		{
			return WriteAttributeTextBlockAsync(ws);
		}
		return WriteElementTextBlockAsync(ws);
	}

	public override Task WriteStringAsync(string text)
	{
		CheckAsyncCall();
		if (inAttributeValue)
		{
			return WriteAttributeTextBlockAsync(text);
		}
		return WriteElementTextBlockAsync(text);
	}

	public override async Task WriteSurrogateCharEntityAsync(char lowChar, char highChar)
	{
		CheckAsyncCall();
		int num = XmlCharType.CombineSurrogateChar(lowChar, highChar);
		bufBytes[bufPos++] = 38;
		bufBytes[bufPos++] = 35;
		bufBytes[bufPos++] = 120;
		await RawTextAsync(num.ToString("X", NumberFormatInfo.InvariantInfo)).ConfigureAwait(continueOnCapturedContext: false);
		bufBytes[bufPos++] = 59;
		textPos = bufPos;
	}

	public override Task WriteCharsAsync(char[] buffer, int index, int count)
	{
		CheckAsyncCall();
		if (inAttributeValue)
		{
			return WriteAttributeTextBlockAsync(buffer, index, count);
		}
		return WriteElementTextBlockAsync(buffer, index, count);
	}

	public override async Task WriteRawAsync(char[] buffer, int index, int count)
	{
		CheckAsyncCall();
		await WriteRawWithCharCheckingAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
		textPos = bufPos;
	}

	public override async Task WriteRawAsync(string data)
	{
		CheckAsyncCall();
		await WriteRawWithCharCheckingAsync(data).ConfigureAwait(continueOnCapturedContext: false);
		textPos = bufPos;
	}

	public override async Task FlushAsync()
	{
		CheckAsyncCall();
		await FlushBufferAsync().ConfigureAwait(continueOnCapturedContext: false);
		await FlushEncoderAsync().ConfigureAwait(continueOnCapturedContext: false);
		if (stream != null)
		{
			await stream.FlushAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	protected virtual async Task FlushBufferAsync()
	{
		try
		{
			if (!writeToNull)
			{
				await stream.WriteAsync(bufBytes, 1, bufPos - 1).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		catch
		{
			writeToNull = true;
			throw;
		}
		finally
		{
			bufBytes[0] = bufBytes[bufPos - 1];
			if (IsSurrogateByte(bufBytes[0]))
			{
				bufBytes[1] = bufBytes[bufPos];
				bufBytes[2] = bufBytes[bufPos + 1];
				bufBytes[3] = bufBytes[bufPos + 2];
			}
			textPos = ((textPos == bufPos) ? 1 : 0);
			attrEndPos = ((attrEndPos == bufPos) ? 1 : 0);
			contentPos = 0;
			cdataPos = 0;
			bufPos = 1;
		}
	}

	private Task FlushEncoderAsync()
	{
		return AsyncHelper.DoneTask;
	}

	[SecuritySafeCritical]
	protected unsafe int WriteAttributeTextBlockNoFlush(char* pSrc, char* pSrcEnd)
	{
		char* ptr = pSrc;
		fixed (byte* ptr2 = bufBytes)
		{
			byte* ptr3 = ptr2 + bufPos;
			int num = 0;
			while (true)
			{
				byte* ptr4 = ptr3 + (pSrcEnd - pSrc);
				if (ptr4 > ptr2 + bufLen)
				{
					ptr4 = ptr2 + bufLen;
				}
				while (ptr3 < ptr4 && (xmlCharType.charProperties[num = *pSrc] & 0x80) != 0 && num <= 127)
				{
					*ptr3 = (byte)num;
					ptr3++;
					pSrc++;
				}
				if (pSrc >= pSrcEnd)
				{
					break;
				}
				if (ptr3 >= ptr4)
				{
					bufPos = (int)(ptr3 - ptr2);
					return (int)(pSrc - ptr);
				}
				switch (num)
				{
				case 38:
					ptr3 = AmpEntity(ptr3);
					break;
				case 60:
					ptr3 = LtEntity(ptr3);
					break;
				case 62:
					ptr3 = GtEntity(ptr3);
					break;
				case 34:
					ptr3 = QuoteEntity(ptr3);
					break;
				case 39:
					*ptr3 = (byte)num;
					ptr3++;
					break;
				case 9:
					if (newLineHandling == NewLineHandling.None)
					{
						*ptr3 = (byte)num;
						ptr3++;
					}
					else
					{
						ptr3 = TabEntity(ptr3);
					}
					break;
				case 13:
					if (newLineHandling == NewLineHandling.None)
					{
						*ptr3 = (byte)num;
						ptr3++;
					}
					else
					{
						ptr3 = CarriageReturnEntity(ptr3);
					}
					break;
				case 10:
					if (newLineHandling == NewLineHandling.None)
					{
						*ptr3 = (byte)num;
						ptr3++;
					}
					else
					{
						ptr3 = LineFeedEntity(ptr3);
					}
					break;
				default:
					if (XmlCharType.IsSurrogate(num))
					{
						ptr3 = EncodeSurrogate(pSrc, pSrcEnd, ptr3);
						pSrc += 2;
					}
					else if (num <= 127 || num >= 65534)
					{
						ptr3 = InvalidXmlChar(num, ptr3, entitize: true);
						pSrc++;
					}
					else
					{
						ptr3 = EncodeMultibyteUTF8(num, ptr3);
						pSrc++;
					}
					continue;
				}
				pSrc++;
			}
			bufPos = (int)(ptr3 - ptr2);
		}
		return -1;
	}

	[SecuritySafeCritical]
	protected unsafe int WriteAttributeTextBlockNoFlush(char[] chars, int index, int count)
	{
		if (count == 0)
		{
			return -1;
		}
		fixed (char* ptr = &chars[index])
		{
			char* pSrcEnd = ptr + count;
			return WriteAttributeTextBlockNoFlush(ptr, pSrcEnd);
		}
	}

	[SecuritySafeCritical]
	protected unsafe int WriteAttributeTextBlockNoFlush(string text, int index, int count)
	{
		if (count == 0)
		{
			return -1;
		}
		fixed (char* ptr = text)
		{
			char* ptr2 = ptr + index;
			char* pSrcEnd = ptr2 + count;
			return WriteAttributeTextBlockNoFlush(ptr2, pSrcEnd);
		}
	}

	protected async Task WriteAttributeTextBlockAsync(char[] chars, int index, int count)
	{
		int curIndex = index;
		int leftCount = count;
		int writeLen;
		do
		{
			writeLen = WriteAttributeTextBlockNoFlush(chars, curIndex, leftCount);
			curIndex += writeLen;
			leftCount -= writeLen;
			if (writeLen >= 0)
			{
				await FlushBufferAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		while (writeLen >= 0);
	}

	protected Task WriteAttributeTextBlockAsync(string text)
	{
		int num = 0;
		int num2 = 0;
		int length = text.Length;
		num = WriteAttributeTextBlockNoFlush(text, num2, length);
		num2 += num;
		length -= num;
		if (num >= 0)
		{
			return _WriteAttributeTextBlockAsync(text, num2, length);
		}
		return AsyncHelper.DoneTask;
	}

	private async Task _WriteAttributeTextBlockAsync(string text, int curIndex, int leftCount)
	{
		await FlushBufferAsync().ConfigureAwait(continueOnCapturedContext: false);
		int writeLen;
		do
		{
			writeLen = WriteAttributeTextBlockNoFlush(text, curIndex, leftCount);
			curIndex += writeLen;
			leftCount -= writeLen;
			if (writeLen >= 0)
			{
				await FlushBufferAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		while (writeLen >= 0);
	}

	[SecuritySafeCritical]
	protected unsafe int WriteElementTextBlockNoFlush(char* pSrc, char* pSrcEnd, out bool needWriteNewLine)
	{
		needWriteNewLine = false;
		char* ptr = pSrc;
		fixed (byte* ptr2 = bufBytes)
		{
			byte* ptr3 = ptr2 + bufPos;
			int num = 0;
			while (true)
			{
				byte* ptr4 = ptr3 + (pSrcEnd - pSrc);
				if (ptr4 > ptr2 + bufLen)
				{
					ptr4 = ptr2 + bufLen;
				}
				while (ptr3 < ptr4 && (xmlCharType.charProperties[num = *pSrc] & 0x80) != 0 && num <= 127)
				{
					*ptr3 = (byte)num;
					ptr3++;
					pSrc++;
				}
				if (pSrc >= pSrcEnd)
				{
					break;
				}
				if (ptr3 >= ptr4)
				{
					bufPos = (int)(ptr3 - ptr2);
					return (int)(pSrc - ptr);
				}
				switch (num)
				{
				case 38:
					ptr3 = AmpEntity(ptr3);
					break;
				case 60:
					ptr3 = LtEntity(ptr3);
					break;
				case 62:
					ptr3 = GtEntity(ptr3);
					break;
				case 9:
				case 34:
				case 39:
					*ptr3 = (byte)num;
					ptr3++;
					break;
				case 10:
					if (newLineHandling == NewLineHandling.Replace)
					{
						bufPos = (int)(ptr3 - ptr2);
						needWriteNewLine = true;
						return (int)(pSrc - ptr);
					}
					*ptr3 = (byte)num;
					ptr3++;
					break;
				case 13:
					switch (newLineHandling)
					{
					case NewLineHandling.Replace:
						if (pSrc[1] == '\n')
						{
							pSrc++;
						}
						bufPos = (int)(ptr3 - ptr2);
						needWriteNewLine = true;
						return (int)(pSrc - ptr);
					case NewLineHandling.Entitize:
						ptr3 = CarriageReturnEntity(ptr3);
						break;
					case NewLineHandling.None:
						*ptr3 = (byte)num;
						ptr3++;
						break;
					}
					break;
				default:
					if (XmlCharType.IsSurrogate(num))
					{
						ptr3 = EncodeSurrogate(pSrc, pSrcEnd, ptr3);
						pSrc += 2;
					}
					else if (num <= 127 || num >= 65534)
					{
						ptr3 = InvalidXmlChar(num, ptr3, entitize: true);
						pSrc++;
					}
					else
					{
						ptr3 = EncodeMultibyteUTF8(num, ptr3);
						pSrc++;
					}
					continue;
				}
				pSrc++;
			}
			bufPos = (int)(ptr3 - ptr2);
			textPos = bufPos;
			contentPos = 0;
		}
		return -1;
	}

	[SecuritySafeCritical]
	protected unsafe int WriteElementTextBlockNoFlush(char[] chars, int index, int count, out bool needWriteNewLine)
	{
		needWriteNewLine = false;
		if (count == 0)
		{
			contentPos = 0;
			return -1;
		}
		fixed (char* ptr = &chars[index])
		{
			char* pSrcEnd = ptr + count;
			return WriteElementTextBlockNoFlush(ptr, pSrcEnd, out needWriteNewLine);
		}
	}

	[SecuritySafeCritical]
	protected unsafe int WriteElementTextBlockNoFlush(string text, int index, int count, out bool needWriteNewLine)
	{
		needWriteNewLine = false;
		if (count == 0)
		{
			contentPos = 0;
			return -1;
		}
		fixed (char* ptr = text)
		{
			char* ptr2 = ptr + index;
			char* pSrcEnd = ptr2 + count;
			return WriteElementTextBlockNoFlush(ptr2, pSrcEnd, out needWriteNewLine);
		}
	}

	protected async Task WriteElementTextBlockAsync(char[] chars, int index, int count)
	{
		int curIndex = index;
		int leftCount = count;
		bool needWriteNewLine = false;
		int writeLen;
		do
		{
			writeLen = WriteElementTextBlockNoFlush(chars, curIndex, leftCount, out needWriteNewLine);
			curIndex += writeLen;
			leftCount -= writeLen;
			if (needWriteNewLine)
			{
				await RawTextAsync(newLineChars).ConfigureAwait(continueOnCapturedContext: false);
				curIndex++;
				leftCount--;
			}
			else if (writeLen >= 0)
			{
				await FlushBufferAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		while (writeLen >= 0 || needWriteNewLine);
	}

	protected Task WriteElementTextBlockAsync(string text)
	{
		int num = 0;
		int num2 = 0;
		int length = text.Length;
		bool needWriteNewLine = false;
		num = WriteElementTextBlockNoFlush(text, num2, length, out needWriteNewLine);
		num2 += num;
		length -= num;
		if (needWriteNewLine)
		{
			return _WriteElementTextBlockAsync(newLine: true, text, num2, length);
		}
		if (num >= 0)
		{
			return _WriteElementTextBlockAsync(newLine: false, text, num2, length);
		}
		return AsyncHelper.DoneTask;
	}

	private async Task _WriteElementTextBlockAsync(bool newLine, string text, int curIndex, int leftCount)
	{
		bool needWriteNewLine = false;
		if (!newLine)
		{
			await FlushBufferAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		else
		{
			await RawTextAsync(newLineChars).ConfigureAwait(continueOnCapturedContext: false);
			curIndex++;
			leftCount--;
		}
		int writeLen;
		do
		{
			writeLen = WriteElementTextBlockNoFlush(text, curIndex, leftCount, out needWriteNewLine);
			curIndex += writeLen;
			leftCount -= writeLen;
			if (needWriteNewLine)
			{
				await RawTextAsync(newLineChars).ConfigureAwait(continueOnCapturedContext: false);
				curIndex++;
				leftCount--;
			}
			else if (writeLen >= 0)
			{
				await FlushBufferAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		while (writeLen >= 0 || needWriteNewLine);
	}

	[SecuritySafeCritical]
	protected unsafe int RawTextNoFlush(char* pSrcBegin, char* pSrcEnd)
	{
		fixed (byte* ptr = bufBytes)
		{
			byte* ptr2 = ptr + bufPos;
			char* ptr3 = pSrcBegin;
			int num = 0;
			while (true)
			{
				byte* ptr4 = ptr2 + (pSrcEnd - ptr3);
				if (ptr4 > ptr + bufLen)
				{
					ptr4 = ptr + bufLen;
				}
				for (; ptr2 < ptr4; ptr2++)
				{
					if ((num = *ptr3) > 127)
					{
						break;
					}
					ptr3++;
					*ptr2 = (byte)num;
				}
				if (ptr3 >= pSrcEnd)
				{
					break;
				}
				if (ptr2 >= ptr4)
				{
					bufPos = (int)(ptr2 - ptr);
					return (int)(ptr3 - pSrcBegin);
				}
				if (XmlCharType.IsSurrogate(num))
				{
					ptr2 = EncodeSurrogate(ptr3, pSrcEnd, ptr2);
					ptr3 += 2;
				}
				else if (num <= 127 || num >= 65534)
				{
					ptr2 = InvalidXmlChar(num, ptr2, entitize: false);
					ptr3++;
				}
				else
				{
					ptr2 = EncodeMultibyteUTF8(num, ptr2);
					ptr3++;
				}
			}
			bufPos = (int)(ptr2 - ptr);
		}
		return -1;
	}

	[SecuritySafeCritical]
	protected unsafe int RawTextNoFlush(string text, int index, int count)
	{
		if (count == 0)
		{
			return -1;
		}
		fixed (char* ptr = text)
		{
			char* ptr2 = ptr + index;
			char* pSrcEnd = ptr2 + count;
			return RawTextNoFlush(ptr2, pSrcEnd);
		}
	}

	protected Task RawTextAsync(string text)
	{
		int num = 0;
		int num2 = 0;
		int length = text.Length;
		num = RawTextNoFlush(text, num2, length);
		num2 += num;
		length -= num;
		if (num >= 0)
		{
			return _RawTextAsync(text, num2, length);
		}
		return AsyncHelper.DoneTask;
	}

	private async Task _RawTextAsync(string text, int curIndex, int leftCount)
	{
		await FlushBufferAsync().ConfigureAwait(continueOnCapturedContext: false);
		int writeLen;
		do
		{
			writeLen = RawTextNoFlush(text, curIndex, leftCount);
			curIndex += writeLen;
			leftCount -= writeLen;
			if (writeLen >= 0)
			{
				await FlushBufferAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		while (writeLen >= 0);
	}

	[SecuritySafeCritical]
	protected unsafe int WriteRawWithCharCheckingNoFlush(char* pSrcBegin, char* pSrcEnd, out bool needWriteNewLine)
	{
		needWriteNewLine = false;
		fixed (byte* ptr = bufBytes)
		{
			char* ptr2 = pSrcBegin;
			byte* ptr3 = ptr + bufPos;
			int num = 0;
			while (true)
			{
				byte* ptr4 = ptr3 + (pSrcEnd - ptr2);
				if (ptr4 > ptr + bufLen)
				{
					ptr4 = ptr + bufLen;
				}
				while (ptr3 < ptr4 && (xmlCharType.charProperties[num = *ptr2] & 0x40) != 0 && num <= 127)
				{
					*ptr3 = (byte)num;
					ptr3++;
					ptr2++;
				}
				if (ptr2 >= pSrcEnd)
				{
					break;
				}
				if (ptr3 >= ptr4)
				{
					bufPos = (int)(ptr3 - ptr);
					return (int)(ptr2 - pSrcBegin);
				}
				switch (num)
				{
				case 9:
				case 38:
				case 60:
				case 93:
					*ptr3 = (byte)num;
					ptr3++;
					break;
				case 13:
					if (newLineHandling == NewLineHandling.Replace)
					{
						if (ptr2[1] == '\n')
						{
							ptr2++;
						}
						bufPos = (int)(ptr3 - ptr);
						needWriteNewLine = true;
						return (int)(ptr2 - pSrcBegin);
					}
					*ptr3 = (byte)num;
					ptr3++;
					break;
				case 10:
					if (newLineHandling == NewLineHandling.Replace)
					{
						bufPos = (int)(ptr3 - ptr);
						needWriteNewLine = true;
						return (int)(ptr2 - pSrcBegin);
					}
					*ptr3 = (byte)num;
					ptr3++;
					break;
				default:
					if (XmlCharType.IsSurrogate(num))
					{
						ptr3 = EncodeSurrogate(ptr2, pSrcEnd, ptr3);
						ptr2 += 2;
					}
					else if (num <= 127 || num >= 65534)
					{
						ptr3 = InvalidXmlChar(num, ptr3, entitize: false);
						ptr2++;
					}
					else
					{
						ptr3 = EncodeMultibyteUTF8(num, ptr3);
						ptr2++;
					}
					continue;
				}
				ptr2++;
			}
			bufPos = (int)(ptr3 - ptr);
		}
		return -1;
	}

	[SecuritySafeCritical]
	protected unsafe int WriteRawWithCharCheckingNoFlush(char[] chars, int index, int count, out bool needWriteNewLine)
	{
		needWriteNewLine = false;
		if (count == 0)
		{
			return -1;
		}
		fixed (char* ptr = &chars[index])
		{
			char* pSrcEnd = ptr + count;
			return WriteRawWithCharCheckingNoFlush(ptr, pSrcEnd, out needWriteNewLine);
		}
	}

	[SecuritySafeCritical]
	protected unsafe int WriteRawWithCharCheckingNoFlush(string text, int index, int count, out bool needWriteNewLine)
	{
		needWriteNewLine = false;
		if (count == 0)
		{
			return -1;
		}
		fixed (char* ptr = text)
		{
			char* ptr2 = ptr + index;
			char* pSrcEnd = ptr2 + count;
			return WriteRawWithCharCheckingNoFlush(ptr2, pSrcEnd, out needWriteNewLine);
		}
	}

	protected async Task WriteRawWithCharCheckingAsync(char[] chars, int index, int count)
	{
		int curIndex = index;
		int leftCount = count;
		bool needWriteNewLine = false;
		int writeLen;
		do
		{
			writeLen = WriteRawWithCharCheckingNoFlush(chars, curIndex, leftCount, out needWriteNewLine);
			curIndex += writeLen;
			leftCount -= writeLen;
			if (needWriteNewLine)
			{
				await RawTextAsync(newLineChars).ConfigureAwait(continueOnCapturedContext: false);
				curIndex++;
				leftCount--;
			}
			else if (writeLen >= 0)
			{
				await FlushBufferAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		while (writeLen >= 0 || needWriteNewLine);
	}

	protected async Task WriteRawWithCharCheckingAsync(string text)
	{
		int curIndex = 0;
		int leftCount = text.Length;
		bool needWriteNewLine = false;
		int writeLen;
		do
		{
			writeLen = WriteRawWithCharCheckingNoFlush(text, curIndex, leftCount, out needWriteNewLine);
			curIndex += writeLen;
			leftCount -= writeLen;
			if (needWriteNewLine)
			{
				await RawTextAsync(newLineChars).ConfigureAwait(continueOnCapturedContext: false);
				curIndex++;
				leftCount--;
			}
			else if (writeLen >= 0)
			{
				await FlushBufferAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		while (writeLen >= 0 || needWriteNewLine);
	}

	[SecuritySafeCritical]
	protected unsafe int WriteCommentOrPiNoFlush(string text, int index, int count, int stopChar, out bool needWriteNewLine)
	{
		needWriteNewLine = false;
		if (count == 0)
		{
			return -1;
		}
		fixed (char* ptr = text)
		{
			char* num = ptr + index;
			fixed (byte* ptr2 = bufBytes)
			{
				char* ptr3 = num;
				char* ptr4 = ptr3;
				char* ptr5 = num + count;
				byte* ptr6 = ptr2 + bufPos;
				int num2 = 0;
				while (true)
				{
					byte* ptr7 = ptr6 + (ptr5 - ptr3);
					if (ptr7 > ptr2 + bufLen)
					{
						ptr7 = ptr2 + bufLen;
					}
					while (ptr6 < ptr7 && (xmlCharType.charProperties[num2 = *ptr3] & 0x40) != 0 && num2 != stopChar && num2 <= 127)
					{
						*ptr6 = (byte)num2;
						ptr6++;
						ptr3++;
					}
					if (ptr3 >= ptr5)
					{
						break;
					}
					if (ptr6 >= ptr7)
					{
						bufPos = (int)(ptr6 - ptr2);
						return (int)(ptr3 - ptr4);
					}
					switch (num2)
					{
					case 45:
						*ptr6 = 45;
						ptr6++;
						if (num2 == stopChar && (ptr3 + 1 == ptr5 || ptr3[1] == '-'))
						{
							*ptr6 = 32;
							ptr6++;
						}
						break;
					case 63:
						*ptr6 = 63;
						ptr6++;
						if (num2 == stopChar && ptr3 + 1 < ptr5 && ptr3[1] == '>')
						{
							*ptr6 = 32;
							ptr6++;
						}
						break;
					case 93:
						*ptr6 = 93;
						ptr6++;
						break;
					case 13:
						if (newLineHandling == NewLineHandling.Replace)
						{
							if (ptr3[1] == '\n')
							{
								ptr3++;
							}
							bufPos = (int)(ptr6 - ptr2);
							needWriteNewLine = true;
							return (int)(ptr3 - ptr4);
						}
						*ptr6 = (byte)num2;
						ptr6++;
						break;
					case 10:
						if (newLineHandling == NewLineHandling.Replace)
						{
							bufPos = (int)(ptr6 - ptr2);
							needWriteNewLine = true;
							return (int)(ptr3 - ptr4);
						}
						*ptr6 = (byte)num2;
						ptr6++;
						break;
					case 9:
					case 38:
					case 60:
						*ptr6 = (byte)num2;
						ptr6++;
						break;
					default:
						if (XmlCharType.IsSurrogate(num2))
						{
							ptr6 = EncodeSurrogate(ptr3, ptr5, ptr6);
							ptr3 += 2;
						}
						else if (num2 <= 127 || num2 >= 65534)
						{
							ptr6 = InvalidXmlChar(num2, ptr6, entitize: false);
							ptr3++;
						}
						else
						{
							ptr6 = EncodeMultibyteUTF8(num2, ptr6);
							ptr3++;
						}
						continue;
					}
					ptr3++;
				}
				bufPos = (int)(ptr6 - ptr2);
			}
			return -1;
		}
	}

	protected async Task WriteCommentOrPiAsync(string text, int stopChar)
	{
		if (text.Length == 0)
		{
			if (bufPos >= bufLen)
			{
				await FlushBufferAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			return;
		}
		int curIndex = 0;
		int leftCount = text.Length;
		bool needWriteNewLine = false;
		int writeLen;
		do
		{
			writeLen = WriteCommentOrPiNoFlush(text, curIndex, leftCount, stopChar, out needWriteNewLine);
			curIndex += writeLen;
			leftCount -= writeLen;
			if (needWriteNewLine)
			{
				await RawTextAsync(newLineChars).ConfigureAwait(continueOnCapturedContext: false);
				curIndex++;
				leftCount--;
			}
			else if (writeLen >= 0)
			{
				await FlushBufferAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		while (writeLen >= 0 || needWriteNewLine);
	}

	[SecuritySafeCritical]
	protected unsafe int WriteCDataSectionNoFlush(string text, int index, int count, out bool needWriteNewLine)
	{
		needWriteNewLine = false;
		if (count == 0)
		{
			return -1;
		}
		fixed (char* ptr = text)
		{
			char* num = ptr + index;
			fixed (byte* ptr2 = bufBytes)
			{
				char* ptr3 = num;
				char* ptr4 = num + count;
				char* ptr5 = ptr3;
				byte* ptr6 = ptr2 + bufPos;
				int num2 = 0;
				while (true)
				{
					byte* ptr7 = ptr6 + (ptr4 - ptr3);
					if (ptr7 > ptr2 + bufLen)
					{
						ptr7 = ptr2 + bufLen;
					}
					while (ptr6 < ptr7 && (xmlCharType.charProperties[num2 = *ptr3] & 0x80) != 0 && num2 != 93 && num2 <= 127)
					{
						*ptr6 = (byte)num2;
						ptr6++;
						ptr3++;
					}
					if (ptr3 >= ptr4)
					{
						break;
					}
					if (ptr6 >= ptr7)
					{
						bufPos = (int)(ptr6 - ptr2);
						return (int)(ptr3 - ptr5);
					}
					switch (num2)
					{
					case 62:
						if (hadDoubleBracket && ptr6[-1] == 93)
						{
							ptr6 = RawEndCData(ptr6);
							ptr6 = RawStartCData(ptr6);
						}
						*ptr6 = 62;
						ptr6++;
						break;
					case 93:
						if (ptr6[-1] == 93)
						{
							hadDoubleBracket = true;
						}
						else
						{
							hadDoubleBracket = false;
						}
						*ptr6 = 93;
						ptr6++;
						break;
					case 13:
						if (newLineHandling == NewLineHandling.Replace)
						{
							if (ptr3[1] == '\n')
							{
								ptr3++;
							}
							bufPos = (int)(ptr6 - ptr2);
							needWriteNewLine = true;
							return (int)(ptr3 - ptr5);
						}
						*ptr6 = (byte)num2;
						ptr6++;
						break;
					case 10:
						if (newLineHandling == NewLineHandling.Replace)
						{
							bufPos = (int)(ptr6 - ptr2);
							needWriteNewLine = true;
							return (int)(ptr3 - ptr5);
						}
						*ptr6 = (byte)num2;
						ptr6++;
						break;
					case 9:
					case 34:
					case 38:
					case 39:
					case 60:
						*ptr6 = (byte)num2;
						ptr6++;
						break;
					default:
						if (XmlCharType.IsSurrogate(num2))
						{
							ptr6 = EncodeSurrogate(ptr3, ptr4, ptr6);
							ptr3 += 2;
						}
						else if (num2 <= 127 || num2 >= 65534)
						{
							ptr6 = InvalidXmlChar(num2, ptr6, entitize: false);
							ptr3++;
						}
						else
						{
							ptr6 = EncodeMultibyteUTF8(num2, ptr6);
							ptr3++;
						}
						continue;
					}
					ptr3++;
				}
				bufPos = (int)(ptr6 - ptr2);
			}
			return -1;
		}
	}

	protected async Task WriteCDataSectionAsync(string text)
	{
		if (text.Length == 0)
		{
			if (bufPos >= bufLen)
			{
				await FlushBufferAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			return;
		}
		int curIndex = 0;
		int leftCount = text.Length;
		bool needWriteNewLine = false;
		int writeLen;
		do
		{
			writeLen = WriteCDataSectionNoFlush(text, curIndex, leftCount, out needWriteNewLine);
			curIndex += writeLen;
			leftCount -= writeLen;
			if (needWriteNewLine)
			{
				await RawTextAsync(newLineChars).ConfigureAwait(continueOnCapturedContext: false);
				curIndex++;
				leftCount--;
			}
			else if (writeLen >= 0)
			{
				await FlushBufferAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		while (writeLen >= 0 || needWriteNewLine);
	}
}
