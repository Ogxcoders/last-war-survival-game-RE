using System.IO;

namespace System.Xml;

internal class HtmlUtf8RawTextWriterIndent : HtmlUtf8RawTextWriter
{
	private int indentLevel;

	private int endBlockPos;

	private string indentChars;

	private bool newLineOnAttributes;

	public HtmlUtf8RawTextWriterIndent(Stream stream, XmlWriterSettings settings)
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
		elementScope.Push((byte)currentElementProperties);
		if (ns.Length == 0)
		{
			currentElementProperties = (ElementProperties)HtmlUtf8RawTextWriter.elementPropertySearch.FindCaseInsensitiveString(localName);
			if (endBlockPos == bufPos && (currentElementProperties & ElementProperties.BLOCK_WS) != ElementProperties.DEFAULT)
			{
				WriteIndent();
			}
			indentLevel++;
			bufBytes[bufPos++] = 60;
		}
		else
		{
			currentElementProperties = (ElementProperties)192u;
			if (endBlockPos == bufPos)
			{
				WriteIndent();
			}
			indentLevel++;
			bufBytes[bufPos++] = 60;
			if (prefix.Length != 0)
			{
				RawText(prefix);
				bufBytes[bufPos++] = 58;
			}
		}
		RawText(localName);
		attrEndPos = bufPos;
	}

	internal override void StartElementContent()
	{
		bufBytes[bufPos++] = 62;
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
