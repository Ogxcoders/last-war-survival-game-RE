using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Runtime.Serialization;

internal class XmlSerializableReader : XmlReader, IXmlLineInfo, IXmlTextParser
{
	private XmlReaderDelegator xmlReader;

	private int startDepth;

	private bool isRootEmptyElement;

	private XmlReader innerReader;

	private XmlReader InnerReader => innerReader;

	public override XmlReaderSettings Settings => InnerReader.Settings;

	public override XmlNodeType NodeType => InnerReader.NodeType;

	public override string Name => InnerReader.Name;

	public override string LocalName => InnerReader.LocalName;

	public override string NamespaceURI => InnerReader.NamespaceURI;

	public override string Prefix => InnerReader.Prefix;

	public override bool HasValue => InnerReader.HasValue;

	public override string Value => InnerReader.Value;

	public override int Depth => InnerReader.Depth;

	public override string BaseURI => InnerReader.BaseURI;

	public override bool IsEmptyElement => InnerReader.IsEmptyElement;

	public override bool IsDefault => InnerReader.IsDefault;

	public override char QuoteChar => InnerReader.QuoteChar;

	public override XmlSpace XmlSpace => InnerReader.XmlSpace;

	public override string XmlLang => InnerReader.XmlLang;

	public override IXmlSchemaInfo SchemaInfo => InnerReader.SchemaInfo;

	public override Type ValueType => InnerReader.ValueType;

	public override int AttributeCount => InnerReader.AttributeCount;

	public override string this[int i] => InnerReader[i];

	public override string this[string name] => InnerReader[name];

	public override string this[string name, string namespaceURI] => InnerReader[name, namespaceURI];

	public override bool EOF => InnerReader.EOF;

	public override ReadState ReadState => InnerReader.ReadState;

	public override XmlNameTable NameTable => InnerReader.NameTable;

	public override bool CanResolveEntity => InnerReader.CanResolveEntity;

	public override bool CanReadBinaryContent => InnerReader.CanReadBinaryContent;

	public override bool CanReadValueChunk => InnerReader.CanReadValueChunk;

	public override bool HasAttributes => InnerReader.HasAttributes;

	bool IXmlTextParser.Normalized
	{
		get
		{
			if (InnerReader is IXmlTextParser xmlTextParser)
			{
				return xmlTextParser.Normalized;
			}
			return xmlReader.Normalized;
		}
		set
		{
			if (!(InnerReader is IXmlTextParser xmlTextParser))
			{
				xmlReader.Normalized = value;
			}
			else
			{
				xmlTextParser.Normalized = value;
			}
		}
	}

	WhitespaceHandling IXmlTextParser.WhitespaceHandling
	{
		get
		{
			if (InnerReader is IXmlTextParser xmlTextParser)
			{
				return xmlTextParser.WhitespaceHandling;
			}
			return xmlReader.WhitespaceHandling;
		}
		set
		{
			if (!(InnerReader is IXmlTextParser xmlTextParser))
			{
				xmlReader.WhitespaceHandling = value;
			}
			else
			{
				xmlTextParser.WhitespaceHandling = value;
			}
		}
	}

	int IXmlLineInfo.LineNumber
	{
		get
		{
			if (InnerReader is IXmlLineInfo xmlLineInfo)
			{
				return xmlLineInfo.LineNumber;
			}
			return xmlReader.LineNumber;
		}
	}

	int IXmlLineInfo.LinePosition
	{
		get
		{
			if (InnerReader is IXmlLineInfo xmlLineInfo)
			{
				return xmlLineInfo.LinePosition;
			}
			return xmlReader.LinePosition;
		}
	}

	internal void BeginRead(XmlReaderDelegator xmlReader)
	{
		if (xmlReader.NodeType != XmlNodeType.Element)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializerReadContext.CreateUnexpectedStateException(XmlNodeType.Element, xmlReader));
		}
		this.xmlReader = xmlReader;
		startDepth = xmlReader.Depth;
		innerReader = xmlReader.UnderlyingReader;
		isRootEmptyElement = InnerReader.IsEmptyElement;
	}

	internal void EndRead()
	{
		if (isRootEmptyElement)
		{
			xmlReader.Read();
			return;
		}
		if (xmlReader.IsStartElement() && xmlReader.Depth == startDepth)
		{
			xmlReader.Read();
		}
		while (xmlReader.Depth > startDepth)
		{
			if (!xmlReader.Read())
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializerReadContext.CreateUnexpectedStateException(XmlNodeType.EndElement, xmlReader));
			}
		}
	}

	public override bool Read()
	{
		XmlReader xmlReader = InnerReader;
		if (xmlReader.Depth == startDepth && (xmlReader.NodeType == XmlNodeType.EndElement || (xmlReader.NodeType == XmlNodeType.Element && xmlReader.IsEmptyElement)))
		{
			return false;
		}
		return xmlReader.Read();
	}

	public override void Close()
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("This method cannot be called from IXmlSerializable implementations.")));
	}

	public override string GetAttribute(string name)
	{
		return InnerReader.GetAttribute(name);
	}

	public override string GetAttribute(string name, string namespaceURI)
	{
		return InnerReader.GetAttribute(name, namespaceURI);
	}

	public override string GetAttribute(int i)
	{
		return InnerReader.GetAttribute(i);
	}

	public override bool MoveToAttribute(string name)
	{
		return InnerReader.MoveToAttribute(name);
	}

	public override bool MoveToAttribute(string name, string ns)
	{
		return InnerReader.MoveToAttribute(name, ns);
	}

	public override void MoveToAttribute(int i)
	{
		InnerReader.MoveToAttribute(i);
	}

	public override bool MoveToFirstAttribute()
	{
		return InnerReader.MoveToFirstAttribute();
	}

	public override bool MoveToNextAttribute()
	{
		return InnerReader.MoveToNextAttribute();
	}

	public override bool MoveToElement()
	{
		return InnerReader.MoveToElement();
	}

	public override string LookupNamespace(string prefix)
	{
		return InnerReader.LookupNamespace(prefix);
	}

	public override bool ReadAttributeValue()
	{
		return InnerReader.ReadAttributeValue();
	}

	public override void ResolveEntity()
	{
		InnerReader.ResolveEntity();
	}

	public override bool IsStartElement()
	{
		return InnerReader.IsStartElement();
	}

	public override bool IsStartElement(string name)
	{
		return InnerReader.IsStartElement(name);
	}

	public override bool IsStartElement(string localname, string ns)
	{
		return InnerReader.IsStartElement(localname, ns);
	}

	public override XmlNodeType MoveToContent()
	{
		return InnerReader.MoveToContent();
	}

	public override object ReadContentAsObject()
	{
		return InnerReader.ReadContentAsObject();
	}

	public override bool ReadContentAsBoolean()
	{
		return InnerReader.ReadContentAsBoolean();
	}

	public override DateTime ReadContentAsDateTime()
	{
		return InnerReader.ReadContentAsDateTime();
	}

	public override double ReadContentAsDouble()
	{
		return InnerReader.ReadContentAsDouble();
	}

	public override int ReadContentAsInt()
	{
		return InnerReader.ReadContentAsInt();
	}

	public override long ReadContentAsLong()
	{
		return InnerReader.ReadContentAsLong();
	}

	public override string ReadContentAsString()
	{
		return InnerReader.ReadContentAsString();
	}

	public override object ReadContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
	{
		return InnerReader.ReadContentAs(returnType, namespaceResolver);
	}

	public override int ReadContentAsBase64(byte[] buffer, int index, int count)
	{
		return InnerReader.ReadContentAsBase64(buffer, index, count);
	}

	public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
	{
		return InnerReader.ReadContentAsBinHex(buffer, index, count);
	}

	public override int ReadValueChunk(char[] buffer, int index, int count)
	{
		return InnerReader.ReadValueChunk(buffer, index, count);
	}

	public override string ReadString()
	{
		return InnerReader.ReadString();
	}

	bool IXmlLineInfo.HasLineInfo()
	{
		if (InnerReader is IXmlLineInfo xmlLineInfo)
		{
			return xmlLineInfo.HasLineInfo();
		}
		return xmlReader.HasLineInfo();
	}
}
