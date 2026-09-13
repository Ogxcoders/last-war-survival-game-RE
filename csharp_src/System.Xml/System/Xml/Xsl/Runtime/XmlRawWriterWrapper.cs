namespace System.Xml.Xsl.Runtime;

internal sealed class XmlRawWriterWrapper : XmlRawWriter
{
	private XmlWriter wrapped;

	public override XmlWriterSettings Settings => wrapped.Settings;

	public XmlRawWriterWrapper(XmlWriter writer)
	{
		wrapped = writer;
	}

	public override void WriteDocType(string name, string pubid, string sysid, string subset)
	{
		wrapped.WriteDocType(name, pubid, sysid, subset);
	}

	public override void WriteStartElement(string prefix, string localName, string ns)
	{
		wrapped.WriteStartElement(prefix, localName, ns);
	}

	public override void WriteStartAttribute(string prefix, string localName, string ns)
	{
		wrapped.WriteStartAttribute(prefix, localName, ns);
	}

	public override void WriteEndAttribute()
	{
		wrapped.WriteEndAttribute();
	}

	public override void WriteCData(string text)
	{
		wrapped.WriteCData(text);
	}

	public override void WriteComment(string text)
	{
		wrapped.WriteComment(text);
	}

	public override void WriteProcessingInstruction(string name, string text)
	{
		wrapped.WriteProcessingInstruction(name, text);
	}

	public override void WriteWhitespace(string ws)
	{
		wrapped.WriteWhitespace(ws);
	}

	public override void WriteString(string text)
	{
		wrapped.WriteString(text);
	}

	public override void WriteChars(char[] buffer, int index, int count)
	{
		wrapped.WriteChars(buffer, index, count);
	}

	public override void WriteRaw(char[] buffer, int index, int count)
	{
		wrapped.WriteRaw(buffer, index, count);
	}

	public override void WriteRaw(string data)
	{
		wrapped.WriteRaw(data);
	}

	public override void WriteEntityRef(string name)
	{
		wrapped.WriteEntityRef(name);
	}

	public override void WriteCharEntity(char ch)
	{
		wrapped.WriteCharEntity(ch);
	}

	public override void WriteSurrogateCharEntity(char lowChar, char highChar)
	{
		wrapped.WriteSurrogateCharEntity(lowChar, highChar);
	}

	public override void Close()
	{
		wrapped.Close();
	}

	public override void Flush()
	{
		wrapped.Flush();
	}

	public override void WriteValue(object value)
	{
		wrapped.WriteValue(value);
	}

	public override void WriteValue(string value)
	{
		wrapped.WriteValue(value);
	}

	public override void WriteValue(bool value)
	{
		wrapped.WriteValue(value);
	}

	public override void WriteValue(DateTime value)
	{
		wrapped.WriteValue(value);
	}

	public override void WriteValue(float value)
	{
		wrapped.WriteValue(value);
	}

	public override void WriteValue(decimal value)
	{
		wrapped.WriteValue(value);
	}

	public override void WriteValue(double value)
	{
		wrapped.WriteValue(value);
	}

	public override void WriteValue(int value)
	{
		wrapped.WriteValue(value);
	}

	public override void WriteValue(long value)
	{
		wrapped.WriteValue(value);
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (disposing)
			{
				((IDisposable)wrapped).Dispose();
			}
		}
		finally
		{
			base.Dispose(disposing);
		}
	}

	internal override void WriteXmlDeclaration(XmlStandalone standalone)
	{
	}

	internal override void WriteXmlDeclaration(string xmldecl)
	{
	}

	internal override void StartElementContent()
	{
	}

	internal override void WriteEndElement(string prefix, string localName, string ns)
	{
		wrapped.WriteEndElement();
	}

	internal override void WriteFullEndElement(string prefix, string localName, string ns)
	{
		wrapped.WriteFullEndElement();
	}

	internal override void WriteNamespaceDeclaration(string prefix, string ns)
	{
		if (prefix.Length == 0)
		{
			wrapped.WriteAttributeString(string.Empty, "xmlns", "http://www.w3.org/2000/xmlns/", ns);
		}
		else
		{
			wrapped.WriteAttributeString("xmlns", prefix, "http://www.w3.org/2000/xmlns/", ns);
		}
	}
}
