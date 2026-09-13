using System.Globalization;
using System.IO;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace System.Xml;

internal class XmlEncodedRawTextWriter : XmlRawWriter
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

	protected int bufBytesUsed;

	protected char[] bufChars;

	protected Encoder encoder;

	protected TextWriter writer;

	protected bool trackTextContent;

	protected bool inTextContent;

	private int lastMarkPos;

	private int[] textContentMarks;

	private CharEntityEncoderFallback charEntityFallback;

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

	protected XmlEncodedRawTextWriter(XmlWriterSettings settings)
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

	public XmlEncodedRawTextWriter(TextWriter writer, XmlWriterSettings settings)
		: this(settings)
	{
		this.writer = writer;
		encoding = writer.Encoding;
		if (settings.Async)
		{
			bufLen = 65536;
		}
		bufChars = new char[bufLen + 32];
		if (settings.AutoXmlDeclaration)
		{
			WriteXmlDeclaration(standalone);
			autoXmlDeclaration = true;
		}
	}

	public XmlEncodedRawTextWriter(Stream stream, XmlWriterSettings settings)
		: this(settings)
	{
		this.stream = stream;
		encoding = settings.Encoding;
		if (settings.Async)
		{
			bufLen = 65536;
		}
		bufChars = new char[bufLen + 32];
		bufBytes = new byte[bufChars.Length];
		bufBytesUsed = 0;
		trackTextContent = true;
		inTextContent = false;
		lastMarkPos = 0;
		textContentMarks = new int[64];
		textContentMarks[0] = 1;
		charEntityFallback = new CharEntityEncoderFallback();
		encoding = (Encoding)settings.Encoding.Clone();
		encoding.EncoderFallback = charEntityFallback;
		encoder = encoding.GetEncoder();
		if (!stream.CanSeek || stream.Position == 0L)
		{
			byte[] preamble = encoding.GetPreamble();
			if (preamble.Length != 0)
			{
				this.stream.Write(preamble, 0, preamble.Length);
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
			if (trackTextContent && inTextContent)
			{
				ChangeTextContentMark(value: false);
			}
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
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
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
			bufChars[bufPos++] = '"';
		}
		else if (sysid != null)
		{
			RawText(" SYSTEM \"");
			RawText(sysid);
			bufChars[bufPos++] = '"';
		}
		else
		{
			bufChars[bufPos++] = ' ';
		}
		if (subset != null)
		{
			bufChars[bufPos++] = '[';
			RawText(subset);
			bufChars[bufPos++] = ']';
		}
		bufChars[bufPos++] = '>';
	}

	public override void WriteStartElement(string prefix, string localName, string ns)
	{
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		bufChars[bufPos++] = '<';
		if (prefix != null && prefix.Length != 0)
		{
			RawText(prefix);
			bufChars[bufPos++] = ':';
		}
		RawText(localName);
		attrEndPos = bufPos;
	}

	internal override void StartElementContent()
	{
		bufChars[bufPos++] = '>';
		contentPos = bufPos;
	}

	internal override void WriteEndElement(string prefix, string localName, string ns)
	{
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		if (contentPos != bufPos)
		{
			bufChars[bufPos++] = '<';
			bufChars[bufPos++] = '/';
			if (prefix != null && prefix.Length != 0)
			{
				RawText(prefix);
				bufChars[bufPos++] = ':';
			}
			RawText(localName);
			bufChars[bufPos++] = '>';
		}
		else
		{
			bufPos--;
			bufChars[bufPos++] = ' ';
			bufChars[bufPos++] = '/';
			bufChars[bufPos++] = '>';
		}
	}

	internal override void WriteFullEndElement(string prefix, string localName, string ns)
	{
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		bufChars[bufPos++] = '<';
		bufChars[bufPos++] = '/';
		if (prefix != null && prefix.Length != 0)
		{
			RawText(prefix);
			bufChars[bufPos++] = ':';
		}
		RawText(localName);
		bufChars[bufPos++] = '>';
	}

	public override void WriteStartAttribute(string prefix, string localName, string ns)
	{
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		if (attrEndPos == bufPos)
		{
			bufChars[bufPos++] = ' ';
		}
		if (prefix != null && prefix.Length > 0)
		{
			RawText(prefix);
			bufChars[bufPos++] = ':';
		}
		RawText(localName);
		bufChars[bufPos++] = '=';
		bufChars[bufPos++] = '"';
		inAttributeValue = true;
	}

	public override void WriteEndAttribute()
	{
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		bufChars[bufPos++] = '"';
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
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		if (prefix.Length == 0)
		{
			RawText(" xmlns=\"");
		}
		else
		{
			RawText(" xmlns:");
			RawText(prefix);
			bufChars[bufPos++] = '=';
			bufChars[bufPos++] = '"';
		}
		inAttributeValue = true;
		if (trackTextContent && !inTextContent)
		{
			ChangeTextContentMark(value: true);
		}
	}

	internal override void WriteEndNamespaceDeclaration()
	{
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		inAttributeValue = false;
		bufChars[bufPos++] = '"';
		attrEndPos = bufPos;
	}

	public override void WriteCData(string text)
	{
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		if (mergeCDataSections && bufPos == cdataPos)
		{
			bufPos -= 3;
		}
		else
		{
			bufChars[bufPos++] = '<';
			bufChars[bufPos++] = '!';
			bufChars[bufPos++] = '[';
			bufChars[bufPos++] = 'C';
			bufChars[bufPos++] = 'D';
			bufChars[bufPos++] = 'A';
			bufChars[bufPos++] = 'T';
			bufChars[bufPos++] = 'A';
			bufChars[bufPos++] = '[';
		}
		WriteCDataSection(text);
		bufChars[bufPos++] = ']';
		bufChars[bufPos++] = ']';
		bufChars[bufPos++] = '>';
		textPos = bufPos;
		cdataPos = bufPos;
	}

	public override void WriteComment(string text)
	{
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		bufChars[bufPos++] = '<';
		bufChars[bufPos++] = '!';
		bufChars[bufPos++] = '-';
		bufChars[bufPos++] = '-';
		WriteCommentOrPi(text, 45);
		bufChars[bufPos++] = '-';
		bufChars[bufPos++] = '-';
		bufChars[bufPos++] = '>';
	}

	public override void WriteProcessingInstruction(string name, string text)
	{
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		bufChars[bufPos++] = '<';
		bufChars[bufPos++] = '?';
		RawText(name);
		if (text.Length > 0)
		{
			bufChars[bufPos++] = ' ';
			WriteCommentOrPi(text, 63);
		}
		bufChars[bufPos++] = '?';
		bufChars[bufPos++] = '>';
	}

	public override void WriteEntityRef(string name)
	{
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		bufChars[bufPos++] = '&';
		RawText(name);
		bufChars[bufPos++] = ';';
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
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		bufChars[bufPos++] = '&';
		bufChars[bufPos++] = '#';
		bufChars[bufPos++] = 'x';
		RawText(s);
		bufChars[bufPos++] = ';';
		if (bufPos > bufLen)
		{
			FlushBuffer();
		}
		textPos = bufPos;
	}

	public unsafe override void WriteWhitespace(string ws)
	{
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
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
		if (trackTextContent && !inTextContent)
		{
			ChangeTextContentMark(value: true);
		}
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
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		int num = XmlCharType.CombineSurrogateChar(lowChar, highChar);
		bufChars[bufPos++] = '&';
		bufChars[bufPos++] = '#';
		bufChars[bufPos++] = 'x';
		RawText(num.ToString("X", NumberFormatInfo.InvariantInfo));
		bufChars[bufPos++] = ';';
		textPos = bufPos;
	}

	public unsafe override void WriteChars(char[] buffer, int index, int count)
	{
		if (trackTextContent && !inTextContent)
		{
			ChangeTextContentMark(value: true);
		}
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
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		fixed (char* ptr = &buffer[index])
		{
			WriteRawWithCharChecking(ptr, ptr + count);
		}
		textPos = bufPos;
	}

	public unsafe override void WriteRaw(string data)
	{
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
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
			else if (writer != null)
			{
				try
				{
					writer.Flush();
				}
				finally
				{
					try
					{
						if (closeOutput)
						{
							writer.Close();
						}
					}
					finally
					{
						writer = null;
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
		else if (writer != null)
		{
			writer.Flush();
		}
	}

	protected virtual void FlushBuffer()
	{
		try
		{
			if (writeToNull)
			{
				return;
			}
			if (stream != null)
			{
				if (trackTextContent)
				{
					charEntityFallback.Reset(textContentMarks, lastMarkPos);
					if ((lastMarkPos & 1) != 0)
					{
						textContentMarks[1] = 1;
						lastMarkPos = 1;
					}
					else
					{
						lastMarkPos = 0;
					}
				}
				EncodeChars(1, bufPos, writeAllToStream: true);
			}
			else
			{
				writer.Write(bufChars, 1, bufPos - 1);
			}
		}
		catch
		{
			writeToNull = true;
			throw;
		}
		finally
		{
			bufChars[0] = bufChars[bufPos - 1];
			textPos = ((textPos == bufPos) ? 1 : 0);
			attrEndPos = ((attrEndPos == bufPos) ? 1 : 0);
			contentPos = 0;
			cdataPos = 0;
			bufPos = 1;
		}
	}

	private void EncodeChars(int startOffset, int endOffset, bool writeAllToStream)
	{
		while (startOffset < endOffset)
		{
			if (charEntityFallback != null)
			{
				charEntityFallback.StartOffset = startOffset;
			}
			encoder.Convert(bufChars, startOffset, endOffset - startOffset, bufBytes, bufBytesUsed, bufBytes.Length - bufBytesUsed, flush: false, out var charsUsed, out var bytesUsed, out var _);
			startOffset += charsUsed;
			bufBytesUsed += bytesUsed;
			if (bufBytesUsed >= bufBytes.Length - 16)
			{
				stream.Write(bufBytes, 0, bufBytesUsed);
				bufBytesUsed = 0;
			}
		}
		if (writeAllToStream && bufBytesUsed > 0)
		{
			stream.Write(bufBytes, 0, bufBytesUsed);
			bufBytesUsed = 0;
		}
	}

	private void FlushEncoder()
	{
		if (stream != null)
		{
			encoder.Convert(bufChars, 1, 0, bufBytes, 0, bufBytes.Length, flush: true, out var _, out var bytesUsed, out var _);
			if (bytesUsed != 0)
			{
				stream.Write(bufBytes, 0, bytesUsed);
			}
		}
	}

	protected unsafe void WriteAttributeTextBlock(char* pSrc, char* pSrcEnd)
	{
		fixed (char* ptr = bufChars)
		{
			char* ptr2 = ptr + bufPos;
			int num = 0;
			while (true)
			{
				char* ptr3 = ptr2 + (pSrcEnd - pSrc);
				if (ptr3 > ptr + bufLen)
				{
					ptr3 = ptr + bufLen;
				}
				while (ptr2 < ptr3 && (xmlCharType.charProperties[num = *pSrc] & 0x80) != 0)
				{
					*ptr2 = (char)num;
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
					*ptr2 = (char)num;
					ptr2++;
					break;
				case 9:
					if (newLineHandling == NewLineHandling.None)
					{
						*ptr2 = (char)num;
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
						*ptr2 = (char)num;
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
						*ptr2 = (char)num;
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
						*ptr2 = (char)num;
						ptr2++;
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
		fixed (char* ptr = bufChars)
		{
			char* ptr2 = ptr + bufPos;
			int num = 0;
			while (true)
			{
				char* ptr3 = ptr2 + (pSrcEnd - pSrc);
				if (ptr3 > ptr + bufLen)
				{
					ptr3 = ptr + bufLen;
				}
				while (ptr2 < ptr3 && (xmlCharType.charProperties[num = *pSrc] & 0x80) != 0)
				{
					*ptr2 = (char)num;
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
					*ptr2 = (char)num;
					ptr2++;
					break;
				case 10:
					if (newLineHandling == NewLineHandling.Replace)
					{
						ptr2 = WriteNewLine(ptr2);
						break;
					}
					*ptr2 = (char)num;
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
						*ptr2 = (char)num;
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
						*ptr2 = (char)num;
						ptr2++;
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
		fixed (char* ptr = bufChars)
		{
			char* ptr2 = ptr + bufPos;
			char* ptr3 = pSrcBegin;
			int num = 0;
			while (true)
			{
				char* ptr4 = ptr2 + (pSrcEnd - ptr3);
				if (ptr4 > ptr + bufLen)
				{
					ptr4 = ptr + bufLen;
				}
				for (; ptr2 < ptr4; ptr2++)
				{
					if ((num = *ptr3) >= 55296)
					{
						break;
					}
					ptr3++;
					*ptr2 = (char)num;
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
					*ptr2 = (char)num;
					ptr2++;
					ptr3++;
				}
			}
			bufPos = (int)(ptr2 - ptr);
		}
	}

	protected unsafe void WriteRawWithCharChecking(char* pSrcBegin, char* pSrcEnd)
	{
		fixed (char* ptr = bufChars)
		{
			char* ptr2 = pSrcBegin;
			char* ptr3 = ptr + bufPos;
			int num = 0;
			while (true)
			{
				char* ptr4 = ptr3 + (pSrcEnd - ptr2);
				if (ptr4 > ptr + bufLen)
				{
					ptr4 = ptr + bufLen;
				}
				while (ptr3 < ptr4 && (xmlCharType.charProperties[num = *ptr2] & 0x40) != 0)
				{
					*ptr3 = (char)num;
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
					*ptr3 = (char)num;
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
						*ptr3 = (char)num;
						ptr3++;
					}
					break;
				case 10:
					if (newLineHandling == NewLineHandling.Replace)
					{
						ptr3 = WriteNewLine(ptr3);
						break;
					}
					*ptr3 = (char)num;
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
						*ptr3 = (char)num;
						ptr3++;
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
			fixed (char* ptr2 = bufChars)
			{
				char* ptr3 = ptr;
				char* ptr4 = ptr + text.Length;
				char* ptr5 = ptr2 + bufPos;
				int num = 0;
				while (true)
				{
					char* ptr6 = ptr5 + (ptr4 - ptr3);
					if (ptr6 > ptr2 + bufLen)
					{
						ptr6 = ptr2 + bufLen;
					}
					while (ptr5 < ptr6 && (xmlCharType.charProperties[num = *ptr3] & 0x40) != 0 && num != stopChar)
					{
						*ptr5 = (char)num;
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
						*ptr5 = '-';
						ptr5++;
						if (num == stopChar && (ptr3 + 1 == ptr4 || ptr3[1] == '-'))
						{
							*ptr5 = ' ';
							ptr5++;
						}
						break;
					case 63:
						*ptr5 = '?';
						ptr5++;
						if (num == stopChar && ptr3 + 1 < ptr4 && ptr3[1] == '>')
						{
							*ptr5 = ' ';
							ptr5++;
						}
						break;
					case 93:
						*ptr5 = ']';
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
							*ptr5 = (char)num;
							ptr5++;
						}
						break;
					case 10:
						if (newLineHandling == NewLineHandling.Replace)
						{
							ptr5 = WriteNewLine(ptr5);
							break;
						}
						*ptr5 = (char)num;
						ptr5++;
						break;
					case 9:
					case 38:
					case 60:
						*ptr5 = (char)num;
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
							*ptr5 = (char)num;
							ptr5++;
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
			fixed (char* ptr2 = bufChars)
			{
				char* ptr3 = ptr;
				char* ptr4 = ptr + text.Length;
				char* ptr5 = ptr2 + bufPos;
				int num = 0;
				while (true)
				{
					char* ptr6 = ptr5 + (ptr4 - ptr3);
					if (ptr6 > ptr2 + bufLen)
					{
						ptr6 = ptr2 + bufLen;
					}
					while (ptr5 < ptr6 && (xmlCharType.charProperties[num = *ptr3] & 0x80) != 0 && num != 93)
					{
						*ptr5 = (char)num;
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
						if (hadDoubleBracket && ptr5[-1] == ']')
						{
							ptr5 = RawEndCData(ptr5);
							ptr5 = RawStartCData(ptr5);
						}
						*ptr5 = '>';
						ptr5++;
						break;
					case 93:
						if (ptr5[-1] == ']')
						{
							hadDoubleBracket = true;
						}
						else
						{
							hadDoubleBracket = false;
						}
						*ptr5 = ']';
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
							*ptr5 = (char)num;
							ptr5++;
						}
						break;
					case 10:
						if (newLineHandling == NewLineHandling.Replace)
						{
							ptr5 = WriteNewLine(ptr5);
							break;
						}
						*ptr5 = (char)num;
						ptr5++;
						break;
					case 9:
					case 34:
					case 38:
					case 39:
					case 60:
						*ptr5 = (char)num;
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
							*ptr5 = (char)num;
							ptr5++;
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

	private unsafe static char* EncodeSurrogate(char* pSrc, char* pSrcEnd, char* pDst)
	{
		int num = *pSrc;
		if (num <= 56319)
		{
			if (pSrc + 1 < pSrcEnd)
			{
				int num2 = pSrc[1];
				if (num2 >= 56320 && (System.LocalAppContextSwitches.DontThrowOnInvalidSurrogatePairs || num2 <= 57343))
				{
					*pDst = (char)num;
					pDst[1] = (char)num2;
					pDst += 2;
					return pDst;
				}
				throw XmlConvert.CreateInvalidSurrogatePairException((char)num2, (char)num);
			}
			throw new ArgumentException(Res.GetString("The surrogate pair is invalid. Missing a low surrogate character."));
		}
		throw XmlConvert.CreateInvalidHighSurrogateCharException((char)num);
	}

	private unsafe char* InvalidXmlChar(int ch, char* pDst, bool entitize)
	{
		if (checkCharacters)
		{
			throw XmlConvert.CreateInvalidCharException((char)ch, '\0');
		}
		if (entitize)
		{
			return CharEntity(pDst, (char)ch);
		}
		*pDst = (char)ch;
		pDst++;
		return pDst;
	}

	internal unsafe void EncodeChar(ref char* pSrc, char* pSrcEnd, ref char* pDst)
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
			*pDst = (char)num;
			pDst++;
			pSrc++;
		}
	}

	protected void ChangeTextContentMark(bool value)
	{
		inTextContent = value;
		if (lastMarkPos + 1 == textContentMarks.Length)
		{
			GrowTextContentMarks();
		}
		textContentMarks[++lastMarkPos] = bufPos;
	}

	private void GrowTextContentMarks()
	{
		int[] destinationArray = new int[textContentMarks.Length * 2];
		Array.Copy(textContentMarks, destinationArray, textContentMarks.Length);
		textContentMarks = destinationArray;
	}

	protected unsafe char* WriteNewLine(char* pDst)
	{
		fixed (char* ptr = bufChars)
		{
			bufPos = (int)(pDst - ptr);
			RawText(newLineChars);
			return ptr + bufPos;
		}
	}

	protected unsafe static char* LtEntity(char* pDst)
	{
		*pDst = '&';
		pDst[1] = 'l';
		pDst[2] = 't';
		pDst[3] = ';';
		return pDst + 4;
	}

	protected unsafe static char* GtEntity(char* pDst)
	{
		*pDst = '&';
		pDst[1] = 'g';
		pDst[2] = 't';
		pDst[3] = ';';
		return pDst + 4;
	}

	protected unsafe static char* AmpEntity(char* pDst)
	{
		*pDst = '&';
		pDst[1] = 'a';
		pDst[2] = 'm';
		pDst[3] = 'p';
		pDst[4] = ';';
		return pDst + 5;
	}

	protected unsafe static char* QuoteEntity(char* pDst)
	{
		*pDst = '&';
		pDst[1] = 'q';
		pDst[2] = 'u';
		pDst[3] = 'o';
		pDst[4] = 't';
		pDst[5] = ';';
		return pDst + 6;
	}

	protected unsafe static char* TabEntity(char* pDst)
	{
		*pDst = '&';
		pDst[1] = '#';
		pDst[2] = 'x';
		pDst[3] = '9';
		pDst[4] = ';';
		return pDst + 5;
	}

	protected unsafe static char* LineFeedEntity(char* pDst)
	{
		*pDst = '&';
		pDst[1] = '#';
		pDst[2] = 'x';
		pDst[3] = 'A';
		pDst[4] = ';';
		return pDst + 5;
	}

	protected unsafe static char* CarriageReturnEntity(char* pDst)
	{
		*pDst = '&';
		pDst[1] = '#';
		pDst[2] = 'x';
		pDst[3] = 'D';
		pDst[4] = ';';
		return pDst + 5;
	}

	private unsafe static char* CharEntity(char* pDst, char ch)
	{
		int num = ch;
		string text = num.ToString("X", NumberFormatInfo.InvariantInfo);
		*pDst = '&';
		pDst[1] = '#';
		pDst[2] = 'x';
		pDst += 3;
		fixed (char* ptr = text)
		{
			char* ptr2 = ptr;
			while ((*(pDst++) = *(ptr2++)) != 0)
			{
			}
		}
		pDst[-1] = ';';
		return pDst;
	}

	protected unsafe static char* RawStartCData(char* pDst)
	{
		*pDst = '<';
		pDst[1] = '!';
		pDst[2] = '[';
		pDst[3] = 'C';
		pDst[4] = 'D';
		pDst[5] = 'A';
		pDst[6] = 'T';
		pDst[7] = 'A';
		pDst[8] = '[';
		return pDst + 9;
	}

	protected unsafe static char* RawEndCData(char* pDst)
	{
		*pDst = ']';
		pDst[1] = ']';
		pDst[2] = '>';
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
			if (trackTextContent && inTextContent)
			{
				ChangeTextContentMark(value: false);
			}
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
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
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
			bufChars[bufPos++] = '"';
		}
		else if (sysid != null)
		{
			await RawTextAsync(" SYSTEM \"").ConfigureAwait(continueOnCapturedContext: false);
			await RawTextAsync(sysid).ConfigureAwait(continueOnCapturedContext: false);
			bufChars[bufPos++] = '"';
		}
		else
		{
			bufChars[bufPos++] = ' ';
		}
		if (subset != null)
		{
			bufChars[bufPos++] = '[';
			await RawTextAsync(subset).ConfigureAwait(continueOnCapturedContext: false);
			bufChars[bufPos++] = ']';
		}
		bufChars[bufPos++] = '>';
	}

	public override Task WriteStartElementAsync(string prefix, string localName, string ns)
	{
		CheckAsyncCall();
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		bufChars[bufPos++] = '<';
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
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		if (contentPos != bufPos)
		{
			bufChars[bufPos++] = '<';
			bufChars[bufPos++] = '/';
			if (prefix != null && prefix.Length != 0)
			{
				return RawTextAsync(prefix + ":" + localName + ">");
			}
			return RawTextAsync(localName + ">");
		}
		bufPos--;
		bufChars[bufPos++] = ' ';
		bufChars[bufPos++] = '/';
		bufChars[bufPos++] = '>';
		return AsyncHelper.DoneTask;
	}

	internal override Task WriteFullEndElementAsync(string prefix, string localName, string ns)
	{
		CheckAsyncCall();
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		bufChars[bufPos++] = '<';
		bufChars[bufPos++] = '/';
		if (prefix != null && prefix.Length != 0)
		{
			return RawTextAsync(prefix + ":" + localName + ">");
		}
		return RawTextAsync(localName + ">");
	}

	protected internal override Task WriteStartAttributeAsync(string prefix, string localName, string ns)
	{
		CheckAsyncCall();
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		if (attrEndPos == bufPos)
		{
			bufChars[bufPos++] = ' ';
		}
		Task task = ((prefix == null || prefix.Length <= 0) ? RawTextAsync(localName) : RawTextAsync(prefix + ":" + localName));
		return task.CallVoidFuncWhenFinish(WriteStartAttribute_SetInAttribute);
	}

	private void WriteStartAttribute_SetInAttribute()
	{
		bufChars[bufPos++] = '=';
		bufChars[bufPos++] = '"';
		inAttributeValue = true;
	}

	protected internal override Task WriteEndAttributeAsync()
	{
		CheckAsyncCall();
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		bufChars[bufPos++] = '"';
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
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		if (prefix.Length == 0)
		{
			await RawTextAsync(" xmlns=\"").ConfigureAwait(continueOnCapturedContext: false);
		}
		else
		{
			await RawTextAsync(" xmlns:").ConfigureAwait(continueOnCapturedContext: false);
			await RawTextAsync(prefix).ConfigureAwait(continueOnCapturedContext: false);
			bufChars[bufPos++] = '=';
			bufChars[bufPos++] = '"';
		}
		inAttributeValue = true;
		if (trackTextContent && !inTextContent)
		{
			ChangeTextContentMark(value: true);
		}
	}

	internal override Task WriteEndNamespaceDeclarationAsync()
	{
		CheckAsyncCall();
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		inAttributeValue = false;
		bufChars[bufPos++] = '"';
		attrEndPos = bufPos;
		return AsyncHelper.DoneTask;
	}

	public override async Task WriteCDataAsync(string text)
	{
		CheckAsyncCall();
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		if (mergeCDataSections && bufPos == cdataPos)
		{
			bufPos -= 3;
		}
		else
		{
			bufChars[bufPos++] = '<';
			bufChars[bufPos++] = '!';
			bufChars[bufPos++] = '[';
			bufChars[bufPos++] = 'C';
			bufChars[bufPos++] = 'D';
			bufChars[bufPos++] = 'A';
			bufChars[bufPos++] = 'T';
			bufChars[bufPos++] = 'A';
			bufChars[bufPos++] = '[';
		}
		await WriteCDataSectionAsync(text).ConfigureAwait(continueOnCapturedContext: false);
		bufChars[bufPos++] = ']';
		bufChars[bufPos++] = ']';
		bufChars[bufPos++] = '>';
		textPos = bufPos;
		cdataPos = bufPos;
	}

	public override async Task WriteCommentAsync(string text)
	{
		CheckAsyncCall();
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		bufChars[bufPos++] = '<';
		bufChars[bufPos++] = '!';
		bufChars[bufPos++] = '-';
		bufChars[bufPos++] = '-';
		await WriteCommentOrPiAsync(text, 45).ConfigureAwait(continueOnCapturedContext: false);
		bufChars[bufPos++] = '-';
		bufChars[bufPos++] = '-';
		bufChars[bufPos++] = '>';
	}

	public override async Task WriteProcessingInstructionAsync(string name, string text)
	{
		CheckAsyncCall();
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		bufChars[bufPos++] = '<';
		bufChars[bufPos++] = '?';
		await RawTextAsync(name).ConfigureAwait(continueOnCapturedContext: false);
		if (text.Length > 0)
		{
			bufChars[bufPos++] = ' ';
			await WriteCommentOrPiAsync(text, 63).ConfigureAwait(continueOnCapturedContext: false);
		}
		bufChars[bufPos++] = '?';
		bufChars[bufPos++] = '>';
	}

	public override async Task WriteEntityRefAsync(string name)
	{
		CheckAsyncCall();
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		bufChars[bufPos++] = '&';
		await RawTextAsync(name).ConfigureAwait(continueOnCapturedContext: false);
		bufChars[bufPos++] = ';';
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
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		bufChars[bufPos++] = '&';
		bufChars[bufPos++] = '#';
		bufChars[bufPos++] = 'x';
		await RawTextAsync(text).ConfigureAwait(continueOnCapturedContext: false);
		bufChars[bufPos++] = ';';
		if (bufPos > bufLen)
		{
			await FlushBufferAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		textPos = bufPos;
	}

	public override Task WriteWhitespaceAsync(string ws)
	{
		CheckAsyncCall();
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		if (inAttributeValue)
		{
			return WriteAttributeTextBlockAsync(ws);
		}
		return WriteElementTextBlockAsync(ws);
	}

	public override Task WriteStringAsync(string text)
	{
		CheckAsyncCall();
		if (trackTextContent && !inTextContent)
		{
			ChangeTextContentMark(value: true);
		}
		if (inAttributeValue)
		{
			return WriteAttributeTextBlockAsync(text);
		}
		return WriteElementTextBlockAsync(text);
	}

	public override async Task WriteSurrogateCharEntityAsync(char lowChar, char highChar)
	{
		CheckAsyncCall();
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		int num = XmlCharType.CombineSurrogateChar(lowChar, highChar);
		bufChars[bufPos++] = '&';
		bufChars[bufPos++] = '#';
		bufChars[bufPos++] = 'x';
		await RawTextAsync(num.ToString("X", NumberFormatInfo.InvariantInfo)).ConfigureAwait(continueOnCapturedContext: false);
		bufChars[bufPos++] = ';';
		textPos = bufPos;
	}

	public override Task WriteCharsAsync(char[] buffer, int index, int count)
	{
		CheckAsyncCall();
		if (trackTextContent && !inTextContent)
		{
			ChangeTextContentMark(value: true);
		}
		if (inAttributeValue)
		{
			return WriteAttributeTextBlockAsync(buffer, index, count);
		}
		return WriteElementTextBlockAsync(buffer, index, count);
	}

	public override async Task WriteRawAsync(char[] buffer, int index, int count)
	{
		CheckAsyncCall();
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		await WriteRawWithCharCheckingAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
		textPos = bufPos;
	}

	public override async Task WriteRawAsync(string data)
	{
		CheckAsyncCall();
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
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
		else if (writer != null)
		{
			await writer.FlushAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	protected virtual async Task FlushBufferAsync()
	{
		_ = 1;
		try
		{
			if (writeToNull)
			{
				return;
			}
			if (stream != null)
			{
				if (trackTextContent)
				{
					charEntityFallback.Reset(textContentMarks, lastMarkPos);
					if ((lastMarkPos & 1) != 0)
					{
						textContentMarks[1] = 1;
						lastMarkPos = 1;
					}
					else
					{
						lastMarkPos = 0;
					}
				}
				await EncodeCharsAsync(1, bufPos, writeAllToStream: true).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				await writer.WriteAsync(bufChars, 1, bufPos - 1).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		catch
		{
			writeToNull = true;
			throw;
		}
		finally
		{
			bufChars[0] = bufChars[bufPos - 1];
			textPos = ((textPos == bufPos) ? 1 : 0);
			attrEndPos = ((attrEndPos == bufPos) ? 1 : 0);
			contentPos = 0;
			cdataPos = 0;
			bufPos = 1;
		}
	}

	private async Task EncodeCharsAsync(int startOffset, int endOffset, bool writeAllToStream)
	{
		while (startOffset < endOffset)
		{
			if (charEntityFallback != null)
			{
				charEntityFallback.StartOffset = startOffset;
			}
			encoder.Convert(bufChars, startOffset, endOffset - startOffset, bufBytes, bufBytesUsed, bufBytes.Length - bufBytesUsed, flush: false, out var charsUsed, out var bytesUsed, out var _);
			startOffset += charsUsed;
			bufBytesUsed += bytesUsed;
			if (bufBytesUsed >= bufBytes.Length - 16)
			{
				await stream.WriteAsync(bufBytes, 0, bufBytesUsed).ConfigureAwait(continueOnCapturedContext: false);
				bufBytesUsed = 0;
			}
		}
		if (writeAllToStream && bufBytesUsed > 0)
		{
			await stream.WriteAsync(bufBytes, 0, bufBytesUsed).ConfigureAwait(continueOnCapturedContext: false);
			bufBytesUsed = 0;
		}
	}

	private Task FlushEncoderAsync()
	{
		if (stream != null)
		{
			encoder.Convert(bufChars, 1, 0, bufBytes, 0, bufBytes.Length, flush: true, out var _, out var bytesUsed, out var _);
			if (bytesUsed != 0)
			{
				return stream.WriteAsync(bufBytes, 0, bytesUsed);
			}
		}
		return AsyncHelper.DoneTask;
	}

	[SecuritySafeCritical]
	protected unsafe int WriteAttributeTextBlockNoFlush(char* pSrc, char* pSrcEnd)
	{
		char* ptr = pSrc;
		fixed (char* ptr2 = bufChars)
		{
			char* ptr3 = ptr2 + bufPos;
			int num = 0;
			while (true)
			{
				char* ptr4 = ptr3 + (pSrcEnd - pSrc);
				if (ptr4 > ptr2 + bufLen)
				{
					ptr4 = ptr2 + bufLen;
				}
				while (ptr3 < ptr4 && (xmlCharType.charProperties[num = *pSrc] & 0x80) != 0)
				{
					*ptr3 = (char)num;
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
					*ptr3 = (char)num;
					ptr3++;
					break;
				case 9:
					if (newLineHandling == NewLineHandling.None)
					{
						*ptr3 = (char)num;
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
						*ptr3 = (char)num;
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
						*ptr3 = (char)num;
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
						*ptr3 = (char)num;
						ptr3++;
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
		fixed (char* ptr2 = bufChars)
		{
			char* ptr3 = ptr2 + bufPos;
			int num = 0;
			while (true)
			{
				char* ptr4 = ptr3 + (pSrcEnd - pSrc);
				if (ptr4 > ptr2 + bufLen)
				{
					ptr4 = ptr2 + bufLen;
				}
				while (ptr3 < ptr4 && (xmlCharType.charProperties[num = *pSrc] & 0x80) != 0)
				{
					*ptr3 = (char)num;
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
					*ptr3 = (char)num;
					ptr3++;
					break;
				case 10:
					if (newLineHandling == NewLineHandling.Replace)
					{
						bufPos = (int)(ptr3 - ptr2);
						needWriteNewLine = true;
						return (int)(pSrc - ptr);
					}
					*ptr3 = (char)num;
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
						*ptr3 = (char)num;
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
						*ptr3 = (char)num;
						ptr3++;
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
		fixed (char* ptr = bufChars)
		{
			char* ptr2 = ptr + bufPos;
			char* ptr3 = pSrcBegin;
			int num = 0;
			while (true)
			{
				char* ptr4 = ptr2 + (pSrcEnd - ptr3);
				if (ptr4 > ptr + bufLen)
				{
					ptr4 = ptr + bufLen;
				}
				for (; ptr2 < ptr4; ptr2++)
				{
					if ((num = *ptr3) >= 55296)
					{
						break;
					}
					ptr3++;
					*ptr2 = (char)num;
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
					*ptr2 = (char)num;
					ptr2++;
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
		fixed (char* ptr = bufChars)
		{
			char* ptr2 = pSrcBegin;
			char* ptr3 = ptr + bufPos;
			int num = 0;
			while (true)
			{
				char* ptr4 = ptr3 + (pSrcEnd - ptr2);
				if (ptr4 > ptr + bufLen)
				{
					ptr4 = ptr + bufLen;
				}
				while (ptr3 < ptr4 && (xmlCharType.charProperties[num = *ptr2] & 0x40) != 0)
				{
					*ptr3 = (char)num;
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
					*ptr3 = (char)num;
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
					*ptr3 = (char)num;
					ptr3++;
					break;
				case 10:
					if (newLineHandling == NewLineHandling.Replace)
					{
						bufPos = (int)(ptr3 - ptr);
						needWriteNewLine = true;
						return (int)(ptr2 - pSrcBegin);
					}
					*ptr3 = (char)num;
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
						*ptr3 = (char)num;
						ptr3++;
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
			fixed (char* ptr2 = bufChars)
			{
				char* ptr3 = num;
				char* ptr4 = ptr3;
				char* ptr5 = num + count;
				char* ptr6 = ptr2 + bufPos;
				int num2 = 0;
				while (true)
				{
					char* ptr7 = ptr6 + (ptr5 - ptr3);
					if (ptr7 > ptr2 + bufLen)
					{
						ptr7 = ptr2 + bufLen;
					}
					while (ptr6 < ptr7 && (xmlCharType.charProperties[num2 = *ptr3] & 0x40) != 0 && num2 != stopChar)
					{
						*ptr6 = (char)num2;
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
						*ptr6 = '-';
						ptr6++;
						if (num2 == stopChar && (ptr3 + 1 == ptr5 || ptr3[1] == '-'))
						{
							*ptr6 = ' ';
							ptr6++;
						}
						break;
					case 63:
						*ptr6 = '?';
						ptr6++;
						if (num2 == stopChar && ptr3 + 1 < ptr5 && ptr3[1] == '>')
						{
							*ptr6 = ' ';
							ptr6++;
						}
						break;
					case 93:
						*ptr6 = ']';
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
						*ptr6 = (char)num2;
						ptr6++;
						break;
					case 10:
						if (newLineHandling == NewLineHandling.Replace)
						{
							bufPos = (int)(ptr6 - ptr2);
							needWriteNewLine = true;
							return (int)(ptr3 - ptr4);
						}
						*ptr6 = (char)num2;
						ptr6++;
						break;
					case 9:
					case 38:
					case 60:
						*ptr6 = (char)num2;
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
							*ptr6 = (char)num2;
							ptr6++;
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
			fixed (char* ptr2 = bufChars)
			{
				char* ptr3 = num;
				char* ptr4 = num + count;
				char* ptr5 = ptr3;
				char* ptr6 = ptr2 + bufPos;
				int num2 = 0;
				while (true)
				{
					char* ptr7 = ptr6 + (ptr4 - ptr3);
					if (ptr7 > ptr2 + bufLen)
					{
						ptr7 = ptr2 + bufLen;
					}
					while (ptr6 < ptr7 && (xmlCharType.charProperties[num2 = *ptr3] & 0x80) != 0 && num2 != 93)
					{
						*ptr6 = (char)num2;
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
						if (hadDoubleBracket && ptr6[-1] == ']')
						{
							ptr6 = RawEndCData(ptr6);
							ptr6 = RawStartCData(ptr6);
						}
						*ptr6 = '>';
						ptr6++;
						break;
					case 93:
						if (ptr6[-1] == ']')
						{
							hadDoubleBracket = true;
						}
						else
						{
							hadDoubleBracket = false;
						}
						*ptr6 = ']';
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
						*ptr6 = (char)num2;
						ptr6++;
						break;
					case 10:
						if (newLineHandling == NewLineHandling.Replace)
						{
							bufPos = (int)(ptr6 - ptr2);
							needWriteNewLine = true;
							return (int)(ptr3 - ptr5);
						}
						*ptr6 = (char)num2;
						ptr6++;
						break;
					case 9:
					case 34:
					case 38:
					case 39:
					case 60:
						*ptr6 = (char)num2;
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
							*ptr6 = (char)num2;
							ptr6++;
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
