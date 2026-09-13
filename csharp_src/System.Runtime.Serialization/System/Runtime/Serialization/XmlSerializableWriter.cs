using System.Xml;

namespace System.Runtime.Serialization;

internal class XmlSerializableWriter : XmlWriter
{
	private XmlWriter xmlWriter;

	private int depth;

	private object obj;

	public override WriteState WriteState => xmlWriter.WriteState;

	public override XmlSpace XmlSpace => xmlWriter.XmlSpace;

	public override string XmlLang => xmlWriter.XmlLang;

	internal void BeginWrite(XmlWriter xmlWriter, object obj)
	{
		depth = 0;
		this.xmlWriter = xmlWriter;
		this.obj = obj;
	}

	internal void EndWrite()
	{
		if (depth != 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("IXmlSerializable.WriteXml method of type '{0}' did not close all open tags. Verify that the IXmlSerializable implementation is correct.", (obj == null) ? string.Empty : DataContract.GetClrTypeFullName(obj.GetType()))));
		}
		obj = null;
	}

	public override void WriteStartDocument()
	{
		if (WriteState == WriteState.Start)
		{
			xmlWriter.WriteStartDocument();
		}
	}

	public override void WriteEndDocument()
	{
		xmlWriter.WriteEndDocument();
	}

	public override void WriteStartDocument(bool standalone)
	{
		if (WriteState == WriteState.Start)
		{
			xmlWriter.WriteStartDocument(standalone);
		}
	}

	public override void WriteDocType(string name, string pubid, string sysid, string subset)
	{
	}

	public override void WriteStartElement(string prefix, string localName, string ns)
	{
		xmlWriter.WriteStartElement(prefix, localName, ns);
		depth++;
	}

	public override void WriteEndElement()
	{
		if (depth == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("IXmlSerializable.WriteXml method of type '{0}' attempted to close too many tags.  Verify that the IXmlSerializable implementation is correct.", (obj == null) ? string.Empty : DataContract.GetClrTypeFullName(obj.GetType()))));
		}
		xmlWriter.WriteEndElement();
		depth--;
	}

	public override void WriteFullEndElement()
	{
		if (depth == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("IXmlSerializable.WriteXml method of type '{0}' attempted to close too many tags.  Verify that the IXmlSerializable implementation is correct.", (obj == null) ? string.Empty : DataContract.GetClrTypeFullName(obj.GetType()))));
		}
		xmlWriter.WriteFullEndElement();
		depth--;
	}

	public override void Close()
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("This method cannot be called from IXmlSerializable implementations.")));
	}

	public override void WriteStartAttribute(string prefix, string localName, string ns)
	{
		xmlWriter.WriteStartAttribute(prefix, localName, ns);
	}

	public override void WriteEndAttribute()
	{
		xmlWriter.WriteEndAttribute();
	}

	public override void WriteCData(string text)
	{
		xmlWriter.WriteCData(text);
	}

	public override void WriteComment(string text)
	{
		xmlWriter.WriteComment(text);
	}

	public override void WriteProcessingInstruction(string name, string text)
	{
		xmlWriter.WriteProcessingInstruction(name, text);
	}

	public override void WriteEntityRef(string name)
	{
		xmlWriter.WriteEntityRef(name);
	}

	public override void WriteCharEntity(char ch)
	{
		xmlWriter.WriteCharEntity(ch);
	}

	public override void WriteWhitespace(string ws)
	{
		xmlWriter.WriteWhitespace(ws);
	}

	public override void WriteString(string text)
	{
		xmlWriter.WriteString(text);
	}

	public override void WriteSurrogateCharEntity(char lowChar, char highChar)
	{
		xmlWriter.WriteSurrogateCharEntity(lowChar, highChar);
	}

	public override void WriteChars(char[] buffer, int index, int count)
	{
		xmlWriter.WriteChars(buffer, index, count);
	}

	public override void WriteRaw(char[] buffer, int index, int count)
	{
		xmlWriter.WriteRaw(buffer, index, count);
	}

	public override void WriteRaw(string data)
	{
		xmlWriter.WriteRaw(data);
	}

	public override void WriteBase64(byte[] buffer, int index, int count)
	{
		xmlWriter.WriteBase64(buffer, index, count);
	}

	public override void WriteBinHex(byte[] buffer, int index, int count)
	{
		xmlWriter.WriteBinHex(buffer, index, count);
	}

	public override void Flush()
	{
		xmlWriter.Flush();
	}

	public override void WriteName(string name)
	{
		xmlWriter.WriteName(name);
	}

	public override void WriteQualifiedName(string localName, string ns)
	{
		xmlWriter.WriteQualifiedName(localName, ns);
	}

	public override string LookupPrefix(string ns)
	{
		return xmlWriter.LookupPrefix(ns);
	}

	public override void WriteNmToken(string name)
	{
		xmlWriter.WriteNmToken(name);
	}
}
