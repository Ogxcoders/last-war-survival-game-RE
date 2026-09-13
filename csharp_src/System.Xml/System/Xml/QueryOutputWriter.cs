using System.Collections.Generic;

namespace System.Xml;

internal class QueryOutputWriter : XmlRawWriter
{
	private XmlRawWriter wrapped;

	private bool inCDataSection;

	private Dictionary<XmlQualifiedName, int> lookupCDataElems;

	private BitStack bitsCData;

	private XmlQualifiedName qnameCData;

	private bool outputDocType;

	private bool checkWellFormedDoc;

	private bool hasDocElem;

	private bool inAttr;

	private string systemId;

	private string publicId;

	private int depth;

	internal override IXmlNamespaceResolver NamespaceResolver
	{
		get
		{
			return resolver;
		}
		set
		{
			resolver = value;
			wrapped.NamespaceResolver = value;
		}
	}

	public override XmlWriterSettings Settings
	{
		get
		{
			XmlWriterSettings settings = wrapped.Settings;
			settings.ReadOnly = false;
			settings.DocTypeSystem = systemId;
			settings.DocTypePublic = publicId;
			settings.ReadOnly = true;
			return settings;
		}
	}

	internal override bool SupportsNamespaceDeclarationInChunks => wrapped.SupportsNamespaceDeclarationInChunks;

	public QueryOutputWriter(XmlRawWriter writer, XmlWriterSettings settings)
	{
		wrapped = writer;
		systemId = settings.DocTypeSystem;
		publicId = settings.DocTypePublic;
		if (settings.OutputMethod == XmlOutputMethod.Xml)
		{
			if (systemId != null)
			{
				outputDocType = true;
				checkWellFormedDoc = true;
			}
			if (settings.AutoXmlDeclaration && settings.Standalone == XmlStandalone.Yes)
			{
				checkWellFormedDoc = true;
			}
			if (settings.CDataSectionElements.Count <= 0)
			{
				return;
			}
			bitsCData = new BitStack();
			lookupCDataElems = new Dictionary<XmlQualifiedName, int>();
			qnameCData = new XmlQualifiedName();
			foreach (XmlQualifiedName cDataSectionElement in settings.CDataSectionElements)
			{
				lookupCDataElems[cDataSectionElement] = 0;
			}
			bitsCData.PushBit(bit: false);
		}
		else if (settings.OutputMethod == XmlOutputMethod.Html && (systemId != null || publicId != null))
		{
			outputDocType = true;
		}
	}

	internal override void WriteXmlDeclaration(XmlStandalone standalone)
	{
		wrapped.WriteXmlDeclaration(standalone);
	}

	internal override void WriteXmlDeclaration(string xmldecl)
	{
		wrapped.WriteXmlDeclaration(xmldecl);
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
		if (checkWellFormedDoc)
		{
			if (depth == 0 && hasDocElem)
			{
				throw new XmlException("Document cannot have multiple document elements.", string.Empty);
			}
			depth++;
			hasDocElem = true;
		}
		if (outputDocType)
		{
			wrapped.WriteDocType((prefix.Length != 0) ? (prefix + ":" + localName) : localName, publicId, systemId, null);
			outputDocType = false;
		}
		wrapped.WriteStartElement(prefix, localName, ns);
		if (lookupCDataElems != null)
		{
			qnameCData.Init(localName, ns);
			bitsCData.PushBit(lookupCDataElems.ContainsKey(qnameCData));
		}
	}

	internal override void WriteEndElement(string prefix, string localName, string ns)
	{
		EndCDataSection();
		wrapped.WriteEndElement(prefix, localName, ns);
		if (checkWellFormedDoc)
		{
			depth--;
		}
		if (lookupCDataElems != null)
		{
			bitsCData.PopBit();
		}
	}

	internal override void WriteFullEndElement(string prefix, string localName, string ns)
	{
		EndCDataSection();
		wrapped.WriteFullEndElement(prefix, localName, ns);
		if (checkWellFormedDoc)
		{
			depth--;
		}
		if (lookupCDataElems != null)
		{
			bitsCData.PopBit();
		}
	}

	internal override void StartElementContent()
	{
		wrapped.StartElementContent();
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

	internal override void WriteNamespaceDeclaration(string prefix, string ns)
	{
		wrapped.WriteNamespaceDeclaration(prefix, ns);
	}

	internal override void WriteStartNamespaceDeclaration(string prefix)
	{
		wrapped.WriteStartNamespaceDeclaration(prefix);
	}

	internal override void WriteEndNamespaceDeclaration()
	{
		wrapped.WriteEndNamespaceDeclaration();
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
		if (checkWellFormedDoc && !hasDocElem)
		{
			throw new XmlException("Document does not have a root element.", string.Empty);
		}
	}

	public override void Flush()
	{
		wrapped.Flush();
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
