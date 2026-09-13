using System.Threading.Tasks;
using System.Xml.XPath;

namespace System.Xml;

internal class XmlAsyncCheckWriter : XmlWriter
{
	private readonly XmlWriter coreWriter;

	private Task lastTask = AsyncHelper.DoneTask;

	internal XmlWriter CoreWriter => coreWriter;

	public override XmlWriterSettings Settings
	{
		get
		{
			XmlWriterSettings settings = coreWriter.Settings;
			settings = ((settings == null) ? new XmlWriterSettings() : settings.Clone());
			settings.Async = true;
			settings.ReadOnly = true;
			return settings;
		}
	}

	public override WriteState WriteState
	{
		get
		{
			CheckAsync();
			return coreWriter.WriteState;
		}
	}

	public override XmlSpace XmlSpace
	{
		get
		{
			CheckAsync();
			return coreWriter.XmlSpace;
		}
	}

	public override string XmlLang
	{
		get
		{
			CheckAsync();
			return coreWriter.XmlLang;
		}
	}

	public XmlAsyncCheckWriter(XmlWriter writer)
	{
		coreWriter = writer;
	}

	private void CheckAsync()
	{
		if (!lastTask.IsCompleted)
		{
			throw new InvalidOperationException(Res.GetString("An asynchronous operation is already in progress."));
		}
	}

	public override void WriteStartDocument()
	{
		CheckAsync();
		coreWriter.WriteStartDocument();
	}

	public override void WriteStartDocument(bool standalone)
	{
		CheckAsync();
		coreWriter.WriteStartDocument(standalone);
	}

	public override void WriteEndDocument()
	{
		CheckAsync();
		coreWriter.WriteEndDocument();
	}

	public override void WriteDocType(string name, string pubid, string sysid, string subset)
	{
		CheckAsync();
		coreWriter.WriteDocType(name, pubid, sysid, subset);
	}

	public override void WriteStartElement(string prefix, string localName, string ns)
	{
		CheckAsync();
		coreWriter.WriteStartElement(prefix, localName, ns);
	}

	public override void WriteEndElement()
	{
		CheckAsync();
		coreWriter.WriteEndElement();
	}

	public override void WriteFullEndElement()
	{
		CheckAsync();
		coreWriter.WriteFullEndElement();
	}

	public override void WriteStartAttribute(string prefix, string localName, string ns)
	{
		CheckAsync();
		coreWriter.WriteStartAttribute(prefix, localName, ns);
	}

	public override void WriteEndAttribute()
	{
		CheckAsync();
		coreWriter.WriteEndAttribute();
	}

	public override void WriteCData(string text)
	{
		CheckAsync();
		coreWriter.WriteCData(text);
	}

	public override void WriteComment(string text)
	{
		CheckAsync();
		coreWriter.WriteComment(text);
	}

	public override void WriteProcessingInstruction(string name, string text)
	{
		CheckAsync();
		coreWriter.WriteProcessingInstruction(name, text);
	}

	public override void WriteEntityRef(string name)
	{
		CheckAsync();
		coreWriter.WriteEntityRef(name);
	}

	public override void WriteCharEntity(char ch)
	{
		CheckAsync();
		coreWriter.WriteCharEntity(ch);
	}

	public override void WriteWhitespace(string ws)
	{
		CheckAsync();
		coreWriter.WriteWhitespace(ws);
	}

	public override void WriteString(string text)
	{
		CheckAsync();
		coreWriter.WriteString(text);
	}

	public override void WriteSurrogateCharEntity(char lowChar, char highChar)
	{
		CheckAsync();
		coreWriter.WriteSurrogateCharEntity(lowChar, highChar);
	}

	public override void WriteChars(char[] buffer, int index, int count)
	{
		CheckAsync();
		coreWriter.WriteChars(buffer, index, count);
	}

	public override void WriteRaw(char[] buffer, int index, int count)
	{
		CheckAsync();
		coreWriter.WriteRaw(buffer, index, count);
	}

	public override void WriteRaw(string data)
	{
		CheckAsync();
		coreWriter.WriteRaw(data);
	}

	public override void WriteBase64(byte[] buffer, int index, int count)
	{
		CheckAsync();
		coreWriter.WriteBase64(buffer, index, count);
	}

	public override void WriteBinHex(byte[] buffer, int index, int count)
	{
		CheckAsync();
		coreWriter.WriteBinHex(buffer, index, count);
	}

	public override void Close()
	{
		CheckAsync();
		coreWriter.Close();
	}

	public override void Flush()
	{
		CheckAsync();
		coreWriter.Flush();
	}

	public override string LookupPrefix(string ns)
	{
		CheckAsync();
		return coreWriter.LookupPrefix(ns);
	}

	public override void WriteNmToken(string name)
	{
		CheckAsync();
		coreWriter.WriteNmToken(name);
	}

	public override void WriteName(string name)
	{
		CheckAsync();
		coreWriter.WriteName(name);
	}

	public override void WriteQualifiedName(string localName, string ns)
	{
		CheckAsync();
		coreWriter.WriteQualifiedName(localName, ns);
	}

	public override void WriteValue(object value)
	{
		CheckAsync();
		coreWriter.WriteValue(value);
	}

	public override void WriteValue(string value)
	{
		CheckAsync();
		coreWriter.WriteValue(value);
	}

	public override void WriteValue(bool value)
	{
		CheckAsync();
		coreWriter.WriteValue(value);
	}

	public override void WriteValue(DateTime value)
	{
		CheckAsync();
		coreWriter.WriteValue(value);
	}

	public override void WriteValue(DateTimeOffset value)
	{
		CheckAsync();
		coreWriter.WriteValue(value);
	}

	public override void WriteValue(double value)
	{
		CheckAsync();
		coreWriter.WriteValue(value);
	}

	public override void WriteValue(float value)
	{
		CheckAsync();
		coreWriter.WriteValue(value);
	}

	public override void WriteValue(decimal value)
	{
		CheckAsync();
		coreWriter.WriteValue(value);
	}

	public override void WriteValue(int value)
	{
		CheckAsync();
		coreWriter.WriteValue(value);
	}

	public override void WriteValue(long value)
	{
		CheckAsync();
		coreWriter.WriteValue(value);
	}

	public override void WriteAttributes(XmlReader reader, bool defattr)
	{
		CheckAsync();
		coreWriter.WriteAttributes(reader, defattr);
	}

	public override void WriteNode(XmlReader reader, bool defattr)
	{
		CheckAsync();
		coreWriter.WriteNode(reader, defattr);
	}

	public override void WriteNode(XPathNavigator navigator, bool defattr)
	{
		CheckAsync();
		coreWriter.WriteNode(navigator, defattr);
	}

	protected override void Dispose(bool disposing)
	{
		CheckAsync();
		coreWriter.Dispose();
	}

	public override Task WriteStartDocumentAsync()
	{
		CheckAsync();
		return lastTask = coreWriter.WriteStartDocumentAsync();
	}

	public override Task WriteStartDocumentAsync(bool standalone)
	{
		CheckAsync();
		return lastTask = coreWriter.WriteStartDocumentAsync(standalone);
	}

	public override Task WriteEndDocumentAsync()
	{
		CheckAsync();
		return lastTask = coreWriter.WriteEndDocumentAsync();
	}

	public override Task WriteDocTypeAsync(string name, string pubid, string sysid, string subset)
	{
		CheckAsync();
		return lastTask = coreWriter.WriteDocTypeAsync(name, pubid, sysid, subset);
	}

	public override Task WriteStartElementAsync(string prefix, string localName, string ns)
	{
		CheckAsync();
		return lastTask = coreWriter.WriteStartElementAsync(prefix, localName, ns);
	}

	public override Task WriteEndElementAsync()
	{
		CheckAsync();
		return lastTask = coreWriter.WriteEndElementAsync();
	}

	public override Task WriteFullEndElementAsync()
	{
		CheckAsync();
		return lastTask = coreWriter.WriteFullEndElementAsync();
	}

	protected internal override Task WriteStartAttributeAsync(string prefix, string localName, string ns)
	{
		CheckAsync();
		return lastTask = coreWriter.WriteStartAttributeAsync(prefix, localName, ns);
	}

	protected internal override Task WriteEndAttributeAsync()
	{
		CheckAsync();
		return lastTask = coreWriter.WriteEndAttributeAsync();
	}

	public override Task WriteCDataAsync(string text)
	{
		CheckAsync();
		return lastTask = coreWriter.WriteCDataAsync(text);
	}

	public override Task WriteCommentAsync(string text)
	{
		CheckAsync();
		return lastTask = coreWriter.WriteCommentAsync(text);
	}

	public override Task WriteProcessingInstructionAsync(string name, string text)
	{
		CheckAsync();
		return lastTask = coreWriter.WriteProcessingInstructionAsync(name, text);
	}

	public override Task WriteEntityRefAsync(string name)
	{
		CheckAsync();
		return lastTask = coreWriter.WriteEntityRefAsync(name);
	}

	public override Task WriteCharEntityAsync(char ch)
	{
		CheckAsync();
		return lastTask = coreWriter.WriteCharEntityAsync(ch);
	}

	public override Task WriteWhitespaceAsync(string ws)
	{
		CheckAsync();
		return lastTask = coreWriter.WriteWhitespaceAsync(ws);
	}

	public override Task WriteStringAsync(string text)
	{
		CheckAsync();
		return lastTask = coreWriter.WriteStringAsync(text);
	}

	public override Task WriteSurrogateCharEntityAsync(char lowChar, char highChar)
	{
		CheckAsync();
		return lastTask = coreWriter.WriteSurrogateCharEntityAsync(lowChar, highChar);
	}

	public override Task WriteCharsAsync(char[] buffer, int index, int count)
	{
		CheckAsync();
		return lastTask = coreWriter.WriteCharsAsync(buffer, index, count);
	}

	public override Task WriteRawAsync(char[] buffer, int index, int count)
	{
		CheckAsync();
		return lastTask = coreWriter.WriteRawAsync(buffer, index, count);
	}

	public override Task WriteRawAsync(string data)
	{
		CheckAsync();
		return lastTask = coreWriter.WriteRawAsync(data);
	}

	public override Task WriteBase64Async(byte[] buffer, int index, int count)
	{
		CheckAsync();
		return lastTask = coreWriter.WriteBase64Async(buffer, index, count);
	}

	public override Task WriteBinHexAsync(byte[] buffer, int index, int count)
	{
		CheckAsync();
		return lastTask = coreWriter.WriteBinHexAsync(buffer, index, count);
	}

	public override Task FlushAsync()
	{
		CheckAsync();
		return lastTask = coreWriter.FlushAsync();
	}

	public override Task WriteNmTokenAsync(string name)
	{
		CheckAsync();
		return lastTask = coreWriter.WriteNmTokenAsync(name);
	}

	public override Task WriteNameAsync(string name)
	{
		CheckAsync();
		return lastTask = coreWriter.WriteNameAsync(name);
	}

	public override Task WriteQualifiedNameAsync(string localName, string ns)
	{
		CheckAsync();
		return lastTask = coreWriter.WriteQualifiedNameAsync(localName, ns);
	}

	public override Task WriteAttributesAsync(XmlReader reader, bool defattr)
	{
		CheckAsync();
		return lastTask = coreWriter.WriteAttributesAsync(reader, defattr);
	}

	public override Task WriteNodeAsync(XmlReader reader, bool defattr)
	{
		CheckAsync();
		return lastTask = coreWriter.WriteNodeAsync(reader, defattr);
	}

	public override Task WriteNodeAsync(XPathNavigator navigator, bool defattr)
	{
		CheckAsync();
		return lastTask = coreWriter.WriteNodeAsync(navigator, defattr);
	}
}
