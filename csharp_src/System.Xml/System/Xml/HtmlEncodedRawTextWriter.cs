using System.IO;

namespace System.Xml;

internal class HtmlEncodedRawTextWriter : XmlEncodedRawTextWriter
{
	protected ByteStack elementScope;

	protected ElementProperties currentElementProperties;

	private AttributeProperties currentAttributeProperties;

	private bool endsWithAmpersand;

	private byte[] uriEscapingBuffer;

	private string mediaType;

	private bool doNotEscapeUriAttributes;

	protected static TernaryTreeReadOnly elementPropertySearch;

	protected static TernaryTreeReadOnly attributePropertySearch;

	private const int StackIncrement = 10;

	public HtmlEncodedRawTextWriter(TextWriter writer, XmlWriterSettings settings)
		: base(writer, settings)
	{
		Init(settings);
	}

	public HtmlEncodedRawTextWriter(Stream stream, XmlWriterSettings settings)
		: base(stream, settings)
	{
		Init(settings);
	}

	internal override void WriteXmlDeclaration(XmlStandalone standalone)
	{
	}

	internal override void WriteXmlDeclaration(string xmldecl)
	{
	}

	public override void WriteDocType(string name, string pubid, string sysid, string subset)
	{
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		RawText("<!DOCTYPE ");
		if (name == "HTML")
		{
			RawText("HTML");
		}
		else
		{
			RawText("html");
		}
		if (pubid != null)
		{
			RawText(" PUBLIC \"");
			RawText(pubid);
			if (sysid != null)
			{
				RawText("\" \"");
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
		elementScope.Push((byte)currentElementProperties);
		if (ns.Length == 0)
		{
			if (trackTextContent && inTextContent)
			{
				ChangeTextContentMark(value: false);
			}
			currentElementProperties = (ElementProperties)elementPropertySearch.FindCaseInsensitiveString(localName);
			bufChars[bufPos++] = '<';
			RawText(localName);
			attrEndPos = bufPos;
		}
		else
		{
			currentElementProperties = ElementProperties.HAS_NS;
			base.WriteStartElement(prefix, localName, ns);
		}
	}

	internal override void StartElementContent()
	{
		bufChars[bufPos++] = '>';
		contentPos = bufPos;
		if ((currentElementProperties & ElementProperties.HEAD) != ElementProperties.DEFAULT)
		{
			WriteMetaElement();
		}
	}

	internal override void WriteEndElement(string prefix, string localName, string ns)
	{
		if (ns.Length == 0)
		{
			if (trackTextContent && inTextContent)
			{
				ChangeTextContentMark(value: false);
			}
			if ((currentElementProperties & ElementProperties.EMPTY) == 0)
			{
				bufChars[bufPos++] = '<';
				bufChars[bufPos++] = '/';
				RawText(localName);
				bufChars[bufPos++] = '>';
			}
		}
		else
		{
			base.WriteEndElement(prefix, localName, ns);
		}
		currentElementProperties = (ElementProperties)elementScope.Pop();
	}

	internal override void WriteFullEndElement(string prefix, string localName, string ns)
	{
		if (ns.Length == 0)
		{
			if (trackTextContent && inTextContent)
			{
				ChangeTextContentMark(value: false);
			}
			if ((currentElementProperties & ElementProperties.EMPTY) == 0)
			{
				bufChars[bufPos++] = '<';
				bufChars[bufPos++] = '/';
				RawText(localName);
				bufChars[bufPos++] = '>';
			}
		}
		else
		{
			base.WriteFullEndElement(prefix, localName, ns);
		}
		currentElementProperties = (ElementProperties)elementScope.Pop();
	}

	public override void WriteStartAttribute(string prefix, string localName, string ns)
	{
		if (ns.Length == 0)
		{
			if (trackTextContent && inTextContent)
			{
				ChangeTextContentMark(value: false);
			}
			if (attrEndPos == bufPos)
			{
				bufChars[bufPos++] = ' ';
			}
			RawText(localName);
			if ((currentElementProperties & (ElementProperties)7u) != ElementProperties.DEFAULT)
			{
				currentAttributeProperties = (AttributeProperties)((uint)attributePropertySearch.FindCaseInsensitiveString(localName) & (uint)currentElementProperties);
				if ((currentAttributeProperties & AttributeProperties.BOOLEAN) != AttributeProperties.DEFAULT)
				{
					inAttributeValue = true;
					return;
				}
			}
			else
			{
				currentAttributeProperties = AttributeProperties.DEFAULT;
			}
			bufChars[bufPos++] = '=';
			bufChars[bufPos++] = '"';
		}
		else
		{
			base.WriteStartAttribute(prefix, localName, ns);
			currentAttributeProperties = AttributeProperties.DEFAULT;
		}
		inAttributeValue = true;
	}

	public override void WriteEndAttribute()
	{
		if ((currentAttributeProperties & AttributeProperties.BOOLEAN) != AttributeProperties.DEFAULT)
		{
			attrEndPos = bufPos;
		}
		else
		{
			if (endsWithAmpersand)
			{
				OutputRestAmps();
				endsWithAmpersand = false;
			}
			if (trackTextContent && inTextContent)
			{
				ChangeTextContentMark(value: false);
			}
			bufChars[bufPos++] = '"';
		}
		inAttributeValue = false;
		attrEndPos = bufPos;
	}

	public override void WriteProcessingInstruction(string target, string text)
	{
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		bufChars[bufPos++] = '<';
		bufChars[bufPos++] = '?';
		RawText(target);
		bufChars[bufPos++] = ' ';
		WriteCommentOrPi(text, 63);
		bufChars[bufPos++] = '>';
		if (bufPos > bufLen)
		{
			FlushBuffer();
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
				WriteHtmlAttributeTextBlock(ptr, pSrcEnd);
			}
			else
			{
				WriteHtmlElementTextBlock(ptr, pSrcEnd);
			}
		}
	}

	public override void WriteEntityRef(string name)
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override void WriteCharEntity(char ch)
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override void WriteSurrogateCharEntity(char lowChar, char highChar)
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
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

	private void Init(XmlWriterSettings settings)
	{
		if (elementPropertySearch == null)
		{
			attributePropertySearch = new TernaryTreeReadOnly(HtmlTernaryTree.htmlAttributes);
			elementPropertySearch = new TernaryTreeReadOnly(HtmlTernaryTree.htmlElements);
		}
		elementScope = new ByteStack(10);
		uriEscapingBuffer = new byte[5];
		currentElementProperties = ElementProperties.DEFAULT;
		mediaType = settings.MediaType;
		doNotEscapeUriAttributes = settings.DoNotEscapeUriAttributes;
	}

	protected void WriteMetaElement()
	{
		RawText("<META http-equiv=\"Content-Type\"");
		if (mediaType == null)
		{
			mediaType = "text/html";
		}
		RawText(" content=\"");
		RawText(mediaType);
		RawText("; charset=");
		RawText(encoding.WebName);
		RawText("\">");
	}

	protected unsafe void WriteHtmlElementTextBlock(char* pSrc, char* pSrcEnd)
	{
		if ((currentElementProperties & ElementProperties.NO_ENTITIES) != ElementProperties.DEFAULT)
		{
			RawText(pSrc, pSrcEnd);
		}
		else
		{
			WriteElementTextBlock(pSrc, pSrcEnd);
		}
	}

	protected unsafe void WriteHtmlAttributeTextBlock(char* pSrc, char* pSrcEnd)
	{
		if ((currentAttributeProperties & (AttributeProperties)7u) != AttributeProperties.DEFAULT)
		{
			if ((currentAttributeProperties & AttributeProperties.BOOLEAN) == 0)
			{
				if ((currentAttributeProperties & (AttributeProperties)5u) != AttributeProperties.DEFAULT && !doNotEscapeUriAttributes)
				{
					WriteUriAttributeText(pSrc, pSrcEnd);
				}
				else
				{
					WriteHtmlAttributeText(pSrc, pSrcEnd);
				}
			}
		}
		else if ((currentElementProperties & ElementProperties.HAS_NS) != ElementProperties.DEFAULT)
		{
			WriteAttributeTextBlock(pSrc, pSrcEnd);
		}
		else
		{
			WriteHtmlAttributeText(pSrc, pSrcEnd);
		}
	}

	private unsafe void WriteHtmlAttributeText(char* pSrc, char* pSrcEnd)
	{
		if (endsWithAmpersand)
		{
			if (pSrcEnd - pSrc > 0 && *pSrc != '{')
			{
				OutputRestAmps();
			}
			endsWithAmpersand = false;
		}
		fixed (char* ptr = bufChars)
		{
			char* pDst = ptr + bufPos;
			char c = '\0';
			while (true)
			{
				char* ptr2 = pDst + (pSrcEnd - pSrc);
				if (ptr2 > ptr + bufLen)
				{
					ptr2 = ptr + bufLen;
				}
				while (pDst < ptr2 && (xmlCharType.charProperties[(uint)(c = *pSrc)] & 0x80) != 0)
				{
					*(pDst++) = c;
					pSrc++;
				}
				if (pSrc >= pSrcEnd)
				{
					break;
				}
				if (pDst >= ptr2)
				{
					bufPos = (int)(pDst - ptr);
					FlushBuffer();
					pDst = ptr + 1;
					continue;
				}
				switch (c)
				{
				case '&':
					if (pSrc + 1 == pSrcEnd)
					{
						endsWithAmpersand = true;
					}
					else if (pSrc[1] != '{')
					{
						pDst = XmlEncodedRawTextWriter.AmpEntity(pDst);
						break;
					}
					*(pDst++) = c;
					break;
				case '"':
					pDst = XmlEncodedRawTextWriter.QuoteEntity(pDst);
					break;
				case '\t':
				case '\'':
				case '<':
				case '>':
					*(pDst++) = c;
					break;
				case '\r':
					pDst = XmlEncodedRawTextWriter.CarriageReturnEntity(pDst);
					break;
				case '\n':
					pDst = XmlEncodedRawTextWriter.LineFeedEntity(pDst);
					break;
				default:
					EncodeChar(ref pSrc, pSrcEnd, ref pDst);
					continue;
				}
				pSrc++;
			}
			bufPos = (int)(pDst - ptr);
		}
	}

	private unsafe void WriteUriAttributeText(char* pSrc, char* pSrcEnd)
	{
		if (endsWithAmpersand)
		{
			if (pSrcEnd - pSrc > 0 && *pSrc != '{')
			{
				OutputRestAmps();
			}
			endsWithAmpersand = false;
		}
		fixed (char* ptr = bufChars)
		{
			char* ptr2 = ptr + bufPos;
			char c = '\0';
			while (true)
			{
				char* ptr3 = ptr2 + (pSrcEnd - pSrc);
				if (ptr3 > ptr + bufLen)
				{
					ptr3 = ptr + bufLen;
				}
				while (ptr2 < ptr3 && (xmlCharType.charProperties[(uint)(c = *pSrc)] & 0x80) != 0 && c < '\u0080')
				{
					*(ptr2++) = c;
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
				switch (c)
				{
				case '&':
					if (pSrc + 1 == pSrcEnd)
					{
						endsWithAmpersand = true;
					}
					else if (pSrc[1] != '{')
					{
						ptr2 = XmlEncodedRawTextWriter.AmpEntity(ptr2);
						break;
					}
					*(ptr2++) = c;
					break;
				case '"':
					ptr2 = XmlEncodedRawTextWriter.QuoteEntity(ptr2);
					break;
				case '\t':
				case '\'':
				case '<':
				case '>':
					*(ptr2++) = c;
					break;
				case '\r':
					ptr2 = XmlEncodedRawTextWriter.CarriageReturnEntity(ptr2);
					break;
				case '\n':
					ptr2 = XmlEncodedRawTextWriter.LineFeedEntity(ptr2);
					break;
				default:
					fixed (byte* ptr4 = uriEscapingBuffer)
					{
						byte* ptr5 = ptr4;
						byte* pDst = ptr5;
						XmlUtf8RawTextWriter.CharToUTF8(ref pSrc, pSrcEnd, ref pDst);
						for (; ptr5 < pDst; ptr5++)
						{
							*(ptr2++) = '%';
							*(ptr2++) = "0123456789ABCDEF"[*ptr5 >> 4];
							*(ptr2++) = "0123456789ABCDEF"[*ptr5 & 0xF];
						}
					}
					continue;
				}
				pSrc++;
			}
			bufPos = (int)(ptr2 - ptr);
		}
	}

	private void OutputRestAmps()
	{
		bufChars[bufPos++] = 'a';
		bufChars[bufPos++] = 'm';
		bufChars[bufPos++] = 'p';
		bufChars[bufPos++] = ';';
	}
}
