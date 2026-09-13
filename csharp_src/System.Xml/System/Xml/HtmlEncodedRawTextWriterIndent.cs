using System.IO;

namespace System.Xml;

internal class HtmlEncodedRawTextWriterIndent : HtmlEncodedRawTextWriter
{
	private int indentLevel;

	private int endBlockPos;

	private string indentChars;

	private bool newLineOnAttributes;

	public HtmlEncodedRawTextWriterIndent(TextWriter writer, XmlWriterSettings settings)
		: base(writer, settings)
	{
		Init(settings);
	}

	public HtmlEncodedRawTextWriterIndent(Stream stream, XmlWriterSettings settings)
		: base(stream, settings)
	{
		Init(settings);
	}

	public override void WriteDocType(string name, string pubid, string sysid, string subset)
	{
		base.WriteDocType(name, pubid, sysid, subset);
		endBlockPos = bufPos;
	}

	public override void WriteStartElement(string prefix, string localName, string ns)
	{
		if (trackTextContent && inTextContent)
		{
			ChangeTextContentMark(value: false);
		}
		elementScope.Push((byte)currentElementProperties);
		if (ns.Length == 0)
		{
			currentElementProperties = (ElementProperties)HtmlEncodedRawTextWriter.elementPropertySearch.FindCaseInsensitiveString(localName);
			if (endBlockPos == bufPos && (currentElementProperties & ElementProperties.BLOCK_WS) != ElementProperties.DEFAULT)
			{
				WriteIndent();
			}
			indentLevel++;
			bufChars[bufPos++] = '<';
		}
		else
		{
			currentElementProperties = (ElementProperties)192u;
			if (endBlockPos == bufPos)
			{
				WriteIndent();
			}
			indentLevel++;
			bufChars[bufPos++] = '<';
			if (prefix.Length != 0)
			{
				RawText(prefix);
				bufChars[bufPos++] = ':';
			}
		}
		RawText(localName);
		attrEndPos = bufPos;
	}

	internal override void StartElementContent()
	{
		bufChars[bufPos++] = '>';
		contentPos = bufPos;
		if ((currentElementProperties & ElementProperties.HEAD) != ElementProperties.DEFAULT)
		{
			WriteIndent();
			WriteMetaElement();
			endBlockPos = bufPos;
		}
		else if ((currentElementProperties & ElementProperties.BLOCK_WS) != ElementProperties.DEFAULT)
		{
			endBlockPos = bufPos;
		}
	}

	internal override void WriteEndElement(string prefix, string localName, string ns)
	{
		indentLevel--;
		bool num = (currentElementProperties & ElementProperties.BLOCK_WS) != 0;
		if (num && endBlockPos == bufPos && contentPos != bufPos)
		{
			WriteIndent();
		}
		base.WriteEndElement(prefix, localName, ns);
		contentPos = 0;
		if (num)
		{
			endBlockPos = bufPos;
		}
	}

	public override void WriteStartAttribute(string prefix, string localName, string ns)
	{
		if (newLineOnAttributes)
		{
			RawText(newLineChars);
			indentLevel++;
			WriteIndent();
			indentLevel--;
		}
		base.WriteStartAttribute(prefix, localName, ns);
	}

	protected override void FlushBuffer()
	{
		endBlockPos = ((endBlockPos == bufPos) ? 1 : 0);
		base.FlushBuffer();
	}

	private void Init(XmlWriterSettings settings)
	{
		indentLevel = 0;
		indentChars = settings.IndentChars;
		newLineOnAttributes = settings.NewLineOnAttributes;
	}

	private void WriteIndent()
	{
		RawText(newLineChars);
		for (int num = indentLevel; num > 0; num--)
		{
			RawText(indentChars);
		}
	}
}
