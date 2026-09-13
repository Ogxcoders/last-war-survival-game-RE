using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml;

internal abstract class XmlRawWriter : XmlWriter
{
	protected XmlRawWriterBase64Encoder base64Encoder;

	protected IXmlNamespaceResolver resolver;

	public override WriteState WriteState
	{
		get
		{
			throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
		}
	}

	public override XmlSpace XmlSpace
	{
		get
		{
			throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
		}
	}

	public override string XmlLang
	{
		get
		{
			throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
		}
	}

	internal virtual IXmlNamespaceResolver NamespaceResolver
	{
		get
		{
			return resolver;
		}
		set
		{
			resolver = value;
		}
	}

	internal virtual bool SupportsNamespaceDeclarationInChunks => false;

	public override void WriteStartDocument()
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override void WriteStartDocument(bool standalone)
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override void WriteEndDocument()
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override void WriteDocType(string name, string pubid, string sysid, string subset)
	{
	}

	public override void WriteEndElement()
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override void WriteFullEndElement()
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override void WriteBase64(byte[] buffer, int index, int count)
	{
		if (base64Encoder == null)
		{
			base64Encoder = new XmlRawWriterBase64Encoder(this);
		}
		base64Encoder.Encode(buffer, index, count);
	}

	public override string LookupPrefix(string ns)
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override void WriteNmToken(string name)
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override void WriteName(string name)
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override void WriteQualifiedName(string localName, string ns)
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override void WriteCData(string text)
	{
		WriteString(text);
	}

	public override void WriteCharEntity(char ch)
	{
		WriteString(new string(new char[1] { ch }));
	}

	public override void WriteSurrogateCharEntity(char lowChar, char highChar)
	{
		WriteString(new string(new char[2] { lowChar, highChar }));
	}

	public override void WriteWhitespace(string ws)
	{
		WriteString(ws);
	}

	public override void WriteChars(char[] buffer, int index, int count)
	{
		WriteString(new string(buffer, index, count));
	}

	public override void WriteRaw(char[] buffer, int index, int count)
	{
		WriteString(new string(buffer, index, count));
	}

	public override void WriteRaw(string data)
	{
		WriteString(data);
	}

	public override void WriteValue(object value)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		WriteString(XmlUntypedConverter.Untyped.ToString(value, resolver));
	}

	public override void WriteValue(string value)
	{
		WriteString(value);
	}

	public override void WriteValue(DateTimeOffset value)
	{
		WriteString(XmlConvert.ToString(value));
	}

	public override void WriteAttributes(XmlReader reader, bool defattr)
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override void WriteNode(XmlReader reader, bool defattr)
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override void WriteNode(XPathNavigator navigator, bool defattr)
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	internal virtual void WriteXmlDeclaration(XmlStandalone standalone)
	{
	}

	internal virtual void WriteXmlDeclaration(string xmldecl)
	{
	}

	internal abstract void StartElementContent();

	internal virtual void OnRootElement(ConformanceLevel conformanceLevel)
	{
	}

	internal abstract void WriteEndElement(string prefix, string localName, string ns);

	internal virtual void WriteFullEndElement(string prefix, string localName, string ns)
	{
		WriteEndElement(prefix, localName, ns);
	}

	internal virtual void WriteQualifiedName(string prefix, string localName, string ns)
	{
		if (prefix.Length != 0)
		{
			WriteString(prefix);
			WriteString(":");
		}
		WriteString(localName);
	}

	internal abstract void WriteNamespaceDeclaration(string prefix, string ns);

	internal virtual void WriteStartNamespaceDeclaration(string prefix)
	{
		throw new NotSupportedException();
	}

	internal virtual void WriteEndNamespaceDeclaration()
	{
		throw new NotSupportedException();
	}

	internal virtual void WriteEndBase64()
	{
		base64Encoder.Flush();
	}

	internal virtual void Close(WriteState currentState)
	{
		Close();
	}

	public override Task WriteStartDocumentAsync()
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override Task WriteStartDocumentAsync(bool standalone)
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override Task WriteEndDocumentAsync()
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override Task WriteDocTypeAsync(string name, string pubid, string sysid, string subset)
	{
		return AsyncHelper.DoneTask;
	}

	public override Task WriteEndElementAsync()
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override Task WriteFullEndElementAsync()
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override Task WriteBase64Async(byte[] buffer, int index, int count)
	{
		if (base64Encoder == null)
		{
			base64Encoder = new XmlRawWriterBase64Encoder(this);
		}
		return base64Encoder.EncodeAsync(buffer, index, count);
	}

	public override Task WriteNmTokenAsync(string name)
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override Task WriteNameAsync(string name)
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override Task WriteQualifiedNameAsync(string localName, string ns)
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override Task WriteCDataAsync(string text)
	{
		return WriteStringAsync(text);
	}

	public override Task WriteCharEntityAsync(char ch)
	{
		return WriteStringAsync(new string(new char[1] { ch }));
	}

	public override Task WriteSurrogateCharEntityAsync(char lowChar, char highChar)
	{
		return WriteStringAsync(new string(new char[2] { lowChar, highChar }));
	}

	public override Task WriteWhitespaceAsync(string ws)
	{
		return WriteStringAsync(ws);
	}

	public override Task WriteCharsAsync(char[] buffer, int index, int count)
	{
		return WriteStringAsync(new string(buffer, index, count));
	}

	public override Task WriteRawAsync(char[] buffer, int index, int count)
	{
		return WriteStringAsync(new string(buffer, index, count));
	}

	public override Task WriteRawAsync(string data)
	{
		return WriteStringAsync(data);
	}

	public override Task WriteAttributesAsync(XmlReader reader, bool defattr)
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override Task WriteNodeAsync(XmlReader reader, bool defattr)
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override Task WriteNodeAsync(XPathNavigator navigator, bool defattr)
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	internal virtual Task WriteXmlDeclarationAsync(XmlStandalone standalone)
	{
		return AsyncHelper.DoneTask;
	}

	internal virtual Task WriteXmlDeclarationAsync(string xmldecl)
	{
		return AsyncHelper.DoneTask;
	}

	internal virtual Task StartElementContentAsync()
	{
		throw new NotImplementedException();
	}

	internal virtual Task WriteEndElementAsync(string prefix, string localName, string ns)
	{
		throw new NotImplementedException();
	}

	internal virtual Task WriteFullEndElementAsync(string prefix, string localName, string ns)
	{
		return WriteEndElementAsync(prefix, localName, ns);
	}

	internal virtual async Task WriteQualifiedNameAsync(string prefix, string localName, string ns)
	{
		if (prefix.Length != 0)
		{
			await WriteStringAsync(prefix).ConfigureAwait(continueOnCapturedContext: false);
			await WriteStringAsync(":").ConfigureAwait(continueOnCapturedContext: false);
		}
		await WriteStringAsync(localName).ConfigureAwait(continueOnCapturedContext: false);
	}

	internal virtual Task WriteNamespaceDeclarationAsync(string prefix, string ns)
	{
		throw new NotImplementedException();
	}

	internal virtual Task WriteStartNamespaceDeclarationAsync(string prefix)
	{
		throw new NotSupportedException();
	}

	internal virtual Task WriteEndNamespaceDeclarationAsync()
	{
		throw new NotSupportedException();
	}

	internal virtual Task WriteEndBase64Async()
	{
		return base64Encoder.FlushAsync();
	}
}
