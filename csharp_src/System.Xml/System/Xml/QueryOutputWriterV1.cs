using System.Collections.Generic;

namespace System.Xml;

internal class QueryOutputWriterV1 : XmlWriter
{
	private XmlWriter wrapped;

	private bool inCDataSection;

	private Dictionary<XmlQualifiedName, XmlQualifiedName> lookupCDataElems;

	private BitStack bitsCData;

	private XmlQualifiedName qnameCData;

	private bool outputDocType;

	private bool inAttr;

	private string systemId;

	private string publicId;

	private XmlStandalone standalone;

	public override WriteState WriteState => wrapped.WriteState;

	public QueryOutputWriterV1(XmlWriter writer, XmlWriterSettings settings)
	{
		wrapped = writer;
		systemId = settings.DocTypeSystem;
		publicId = settings.DocTypePublic;
		if (settings.OutputMethod == XmlOutputMethod.Xml)
		{
			bool flag = false;
			if (systemId != null)
			{
				flag = true;
				outputDocType = true;
			}
			if (settings.Standalone == XmlStandalone.Yes)
			{
				flag = true;
				standalone = settings.Standalone;
			}
			if (flag)
			{
				if (settings.Standalone == XmlStandalone.Yes)
				{
					wrapped.WriteStartDocument(standalone: true);
				}
				else
				{
					wrapped.WriteStartDocument();
				}
			}
			if (settings.CDataSectionElements == null || settings.CDataSectionElements.Count <= 0)
			{
				return;
			}
			bitsCData = new BitStack();
			lookupCDataElems = new Dictionary<XmlQualifiedName, XmlQualifiedName>();
			qnameCData = new XmlQualifiedName();
			foreach (XmlQualifiedName cDataSectionElement in settings.CDataSectionElements)
			{
				lookupCDataElems[cDataSectionElement] = null;
			}
			bitsCData.PushBit(bit: false);
		}
		else if (settings.OutputMethod == XmlOutputMethod.Html && (systemId != null || publicId != null))
		{
			outputDocType = true;
		}
	}

	public override void WriteStartDocument()
	{
		wrapped.WriteStartDocument();
	}

	public override void WriteStartDocument(bool standalone)
	{
		wrapped.WriteStartDocument(standalone);
	}

	public override void WriteEndDocument()
	{
		wrapped.WriteEndDocument();
	}

	public override void WriteDocType(string name, string pubid, string sysid, string subset)
	{
		if (publicId == null && systemId == null)
		{
			wrapped.WriteDocType(name, pubid, sysid, subset);
		}
	}

	public override void WriteStartElement(string prefix, string localName, string ns)
	{
		EndCDataSection();
		if (outputDocType)
		{
			WriteState writeState = wrapped.WriteState;
			if (writeState == WriteState.Start || writeState == WriteState.Prolog)
			{
				wrapped.WriteDocType((prefix.Length != 0) ? (prefix + ":" + localName) : localName, publicId, systemId, null);
			}
			outputDocType = false;
		}
		wrapped.WriteStartElement(prefix, localName, ns);
		if (lookupCDataElems != null)
		{
			qnameCData.Init(localName, ns);
			bitsCData.PushBit(lookupCDataElems.ContainsKey(qnameCData));
		}
	}

	public override void WriteEndElement()
	{
		EndCDataSection();
		wrapped.WriteEndElement();
		if (lookupCDataElems != null)
		{
			bitsCData.PopBit();
		}
	}

	public override void WriteFullEndElement()
	{
		EndCDataSection();
		wrapped.WriteFullEndElement();
		if (lookupCDataElems != null)
		{
			bitsCData.PopBit();
		}
	}

	public override void WriteStartAttribute(string prefix, string localName, string ns)
	{
		inAttr = true;
		wrapped.WriteStartAttribute(prefix, localName, ns);
	}

	public override void WriteEndAttribute()
	{
		inAttr = false;
		wrapped.WriteEndAttribute();
	}

	public override void WriteCData(string text)
	{
		wrapped.WriteCData(text);
	}

	public override void WriteComment(string text)
	{
		EndCDataSection();
		wrapped.WriteComment(text);
	}

	public override void WriteProcessingInstruction(string name, string text)
	{
		EndCDataSection();
		wrapped.WriteProcessingInstruction(name, text);
	}

	public override void WriteWhitespace(string ws)
	{
		if (!inAttr && (inCDataSection || StartCDataSection()))
		{
			wrapped.WriteCData(ws);
		}
		else
		{
			wrapped.WriteWhitespace(ws);
		}
	}

	public override void WriteString(string text)
	{
		if (!inAttr && (inCDataSection || StartCDataSection()))
		{
			wrapped.WriteCData(text);
		}
		else
		{
			wrapped.WriteString(text);
		}
	}

	public override void WriteChars(char[] buffer, int index, int count)
	{
		if (!inAttr && (inCDataSection || StartCDataSection()))
		{
			wrapped.WriteCData(new string(buffer, index, count));
		}
		else
		{
			wrapped.WriteChars(buffer, index, count);
		}
	}

	public override void WriteBase64(byte[] buffer, int index, int count)
	{
		if (!inAttr && (inCDataSection || StartCDataSection()))
		{
			wrapped.WriteBase64(buffer, index, count);
		}
		else
		{
			wrapped.WriteBase64(buffer, index, count);
		}
	}

	public override void WriteEntityRef(string name)
	{
		EndCDataSection();
		wrapped.WriteEntityRef(name);
	}

	public override void WriteCharEntity(char ch)
	{
		EndCDataSection();
		wrapped.WriteCharEntity(ch);
	}

	public override void WriteSurrogateCharEntity(char lowChar, char highChar)
	{
		EndCDataSection();
		wrapped.WriteSurrogateCharEntity(lowChar, highChar);
	}

	public override void WriteRaw(char[] buffer, int index, int count)
	{
		if (!inAttr && (inCDataSection || StartCDataSection()))
		{
			wrapped.WriteCData(new string(buffer, index, count));
		}
		else
		{
			wrapped.WriteRaw(buffer, index, count);
		}
	}

	public override void WriteRaw(string data)
	{
		if (!inAttr && (inCDataSection || StartCDataSection()))
		{
			wrapped.WriteCData(data);
		}
		else
		{
			wrapped.WriteRaw(data);
		}
	}

	public override void Close()
	{
		wrapped.Close();
	}

	public override void Flush()
	{
		wrapped.Flush();
	}

	public override string LookupPrefix(string ns)
	{
		return wrapped.LookupPrefix(ns);
	}

	private bool StartCDataSection()
	{
		if (lookupCDataElems != null && bitsCData.PeekBit())
		{
			inCDataSection = true;
			return true;
		}
		return false;
	}

	private void EndCDataSection()
	{
		inCDataSection = false;
	}
}
