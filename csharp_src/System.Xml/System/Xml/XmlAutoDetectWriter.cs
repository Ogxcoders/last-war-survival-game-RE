using System.IO;

namespace System.Xml;

internal class XmlAutoDetectWriter : XmlRawWriter, IRemovableWriter
{
	private XmlRawWriter wrapped;

	private OnRemoveWriter onRemove;

	private XmlWriterSettings writerSettings;

	private XmlEventCache eventCache;

	private TextWriter textWriter;

	private Stream strm;

	public OnRemoveWriter OnRemoveWriterEvent
	{
		get
		{
			return onRemove;
		}
		set
		{
			onRemove = value;
		}
	}

	public override XmlWriterSettings Settings => writerSettings;

	internal override IXmlNamespaceResolver NamespaceResolver
	{
		get
		{
			return resolver;
		}
		set
		{
			resolver = value;
			if (wrapped == null)
			{
				eventCache.NamespaceResolver = value;
			}
			else
			{
				wrapped.NamespaceResolver = value;
			}
		}
	}

	internal override bool SupportsNamespaceDeclarationInChunks => wrapped.SupportsNamespaceDeclarationInChunks;

	private XmlAutoDetectWriter(XmlWriterSettings writerSettings)
	{
		this.writerSettings = writerSettings.Clone();
		this.writerSettings.ReadOnly = true;
		eventCache = new XmlEventCache(string.Empty, hasRootNode: true);
	}

	public XmlAutoDetectWriter(TextWriter textWriter, XmlWriterSettings writerSettings)
		: this(writerSettings)
	{
		this.textWriter = textWriter;
	}

	public XmlAutoDetectWriter(Stream strm, XmlWriterSettings writerSettings)
		: this(writerSettings)
	{
		this.strm = strm;
	}

	public override void WriteDocType(string name, string pubid, string sysid, string subset)
	{
		EnsureWrappedWriter(XmlOutputMethod.Xml);
		wrapped.WriteDocType(name, pubid, sysid, subset);
	}

	public override void WriteStartElement(string prefix, string localName, string ns)
	{
		if (wrapped == null)
		{
			if (ns.Length == 0 && IsHtmlTag(localName))
			{
				CreateWrappedWriter(XmlOutputMethod.Html);
			}
			else
			{
				CreateWrappedWriter(XmlOutputMethod.Xml);
			}
		}
		wrapped.WriteStartElement(prefix, localName, ns);
	}

	public override void WriteStartAttribute(string prefix, string localName, string ns)
	{
		EnsureWrappedWriter(XmlOutputMethod.Xml);
		wrapped.WriteStartAttribute(prefix, localName, ns);
	}

	public override void WriteEndAttribute()
	{
		wrapped.WriteEndAttribute();
	}

	public override void WriteCData(string text)
	{
		if (TextBlockCreatesWriter(text))
		{
			wrapped.WriteCData(text);
		}
		else
		{
			eventCache.WriteCData(text);
		}
	}

	public override void WriteComment(string text)
	{
		if (wrapped == null)
		{
			eventCache.WriteComment(text);
		}
		else
		{
			wrapped.WriteComment(text);
		}
	}

	public override void WriteProcessingInstruction(string name, string text)
	{
		if (wrapped == null)
		{
			eventCache.WriteProcessingInstruction(name, text);
		}
		else
		{
			wrapped.WriteProcessingInstruction(name, text);
		}
	}

	public override void WriteWhitespace(string ws)
	{
		if (wrapped == null)
		{
			eventCache.WriteWhitespace(ws);
		}
		else
		{
			wrapped.WriteWhitespace(ws);
		}
	}

	public override void WriteString(string text)
	{
		if (TextBlockCreatesWriter(text))
		{
			wrapped.WriteString(text);
		}
		else
		{
			eventCache.WriteString(text);
		}
	}

	public override void WriteChars(char[] buffer, int index, int count)
	{
		WriteString(new string(buffer, index, count));
	}

	public override void WriteRaw(char[] buffer, int index, int count)
	{
		WriteRaw(new string(buffer, index, count));
	}

	public override void WriteRaw(string data)
	{
		if (TextBlockCreatesWriter(data))
		{
			wrapped.WriteRaw(data);
		}
		else
		{
			eventCache.WriteRaw(data);
		}
	}

	public override void WriteEntityRef(string name)
	{
		EnsureWrappedWriter(XmlOutputMethod.Xml);
		wrapped.WriteEntityRef(name);
	}

	public override void WriteCharEntity(char ch)
	{
		EnsureWrappedWriter(XmlOutputMethod.Xml);
		wrapped.WriteCharEntity(ch);
	}

	public override void WriteSurrogateCharEntity(char lowChar, char highChar)
	{
		EnsureWrappedWriter(XmlOutputMethod.Xml);
		wrapped.WriteSurrogateCharEntity(lowChar, highChar);
	}

	public override void WriteBase64(byte[] buffer, int index, int count)
	{
		EnsureWrappedWriter(XmlOutputMethod.Xml);
		wrapped.WriteBase64(buffer, index, count);
	}

	public override void WriteBinHex(byte[] buffer, int index, int count)
	{
		EnsureWrappedWriter(XmlOutputMethod.Xml);
		wrapped.WriteBinHex(buffer, index, count);
	}

	public override void Close()
	{
		EnsureWrappedWriter(XmlOutputMethod.Xml);
		wrapped.Close();
	}

	public override void Flush()
	{
		EnsureWrappedWriter(XmlOutputMethod.Xml);
		wrapped.Flush();
	}

	public override void WriteValue(object value)
	{
		EnsureWrappedWriter(XmlOutputMethod.Xml);
		wrapped.WriteValue(value);
	}

	public override void WriteValue(string value)
	{
		EnsureWrappedWriter(XmlOutputMethod.Xml);
		wrapped.WriteValue(value);
	}

	public override void WriteValue(bool value)
	{
		EnsureWrappedWriter(XmlOutputMethod.Xml);
		wrapped.WriteValue(value);
	}

	public override void WriteValue(DateTime value)
	{
		EnsureWrappedWriter(XmlOutputMethod.Xml);
		wrapped.WriteValue(value);
	}

	public override void WriteValue(DateTimeOffset value)
	{
		EnsureWrappedWriter(XmlOutputMethod.Xml);
		wrapped.WriteValue(value);
	}

	public override void WriteValue(double value)
	{
		EnsureWrappedWriter(XmlOutputMethod.Xml);
		wrapped.WriteValue(value);
	}

	public override void WriteValue(float value)
	{
		EnsureWrappedWriter(XmlOutputMethod.Xml);
		wrapped.WriteValue(value);
	}

	public override void WriteValue(decimal value)
	{
		EnsureWrappedWriter(XmlOutputMethod.Xml);
		wrapped.WriteValue(value);
	}

	public override void WriteValue(int value)
	{
		EnsureWrappedWriter(XmlOutputMethod.Xml);
		wrapped.WriteValue(value);
	}

	public override void WriteValue(long value)
	{
		EnsureWrappedWriter(XmlOutputMethod.Xml);
		wrapped.WriteValue(value);
	}

	internal override void WriteXmlDeclaration(XmlStandalone standalone)
	{
		EnsureWrappedWriter(XmlOutputMethod.Xml);
		wrapped.WriteXmlDeclaration(standalone);
	}

	internal override void WriteXmlDeclaration(string xmldecl)
	{
		EnsureWrappedWriter(XmlOutputMethod.Xml);
		wrapped.WriteXmlDeclaration(xmldecl);
	}

	internal override void StartElementContent()
	{
		wrapped.StartElementContent();
	}

	internal override void WriteEndElement(string prefix, string localName, string ns)
	{
		wrapped.WriteEndElement(prefix, localName, ns);
	}

	internal override void WriteFullEndElement(string prefix, string localName, string ns)
	{
		wrapped.WriteFullEndElement(prefix, localName, ns);
	}

	internal override void WriteNamespaceDeclaration(string prefix, string ns)
	{
		EnsureWrappedWriter(XmlOutputMethod.Xml);
		wrapped.WriteNamespaceDeclaration(prefix, ns);
	}

	internal override void WriteStartNamespaceDeclaration(string prefix)
	{
		EnsureWrappedWriter(XmlOutputMethod.Xml);
		wrapped.WriteStartNamespaceDeclaration(prefix);
	}

	internal override void WriteEndNamespaceDeclaration()
	{
		wrapped.WriteEndNamespaceDeclaration();
	}

	private static bool IsHtmlTag(string tagName)
	{
		if (tagName.Length != 4)
		{
			return false;
		}
		if (tagName[0] != 'H' && tagName[0] != 'h')
		{
			return false;
		}
		if (tagName[1] != 'T' && tagName[1] != 't')
		{
			return false;
		}
		if (tagName[2] != 'M' && tagName[2] != 'm')
		{
			return false;
		}
		if (tagName[3] != 'L' && tagName[3] != 'l')
		{
			return false;
		}
		return true;
	}

	private void EnsureWrappedWriter(XmlOutputMethod outMethod)
	{
		if (wrapped == null)
		{
			CreateWrappedWriter(outMethod);
		}
	}

	private bool TextBlockCreatesWriter(string textBlock)
	{
		if (wrapped == null)
		{
			if (XmlCharType.Instance.IsOnlyWhitespace(textBlock))
			{
				return false;
			}
			CreateWrappedWriter(XmlOutputMethod.Xml);
		}
		return true;
	}

	private void CreateWrappedWriter(XmlOutputMethod outMethod)
	{
		writerSettings.ReadOnly = false;
		writerSettings.OutputMethod = outMethod;
		if (outMethod == XmlOutputMethod.Html && writerSettings.IndentInternal == TriState.Unknown)
		{
			writerSettings.Indent = true;
		}
		writerSettings.ReadOnly = true;
		if (textWriter != null)
		{
			wrapped = ((XmlWellFormedWriter)XmlWriter.Create(textWriter, writerSettings)).RawWriter;
		}
		else
		{
			wrapped = ((XmlWellFormedWriter)XmlWriter.Create(strm, writerSettings)).RawWriter;
		}
		eventCache.EndEvents();
		eventCache.EventsToWriter(wrapped);
		if (onRemove != null)
		{
			onRemove(wrapped);
		}
	}
}
