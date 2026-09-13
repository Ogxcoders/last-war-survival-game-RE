using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Xml.Schema;

namespace System.Xml;

[Obsolete("Use XmlReader created by XmlReader.Create() method using appropriate XmlReaderSettings instead. https://go.microsoft.com/fwlink/?linkid=14202")]
[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
public class XmlValidatingReader : XmlReader, IXmlLineInfo, IXmlNamespaceResolver
{
	private XmlValidatingReaderImpl impl;

	public override XmlNodeType NodeType => impl.NodeType;

	public override string Name => impl.Name;

	public override string LocalName => impl.LocalName;

	public override string NamespaceURI => impl.NamespaceURI;

	public override string Prefix => impl.Prefix;

	public override bool HasValue => impl.HasValue;

	public override string Value => impl.Value;

	public override int Depth => impl.Depth;

	public override string BaseURI => impl.BaseURI;

	public override bool IsEmptyElement => impl.IsEmptyElement;

	public override bool IsDefault => impl.IsDefault;

	public override char QuoteChar => impl.QuoteChar;

	public override XmlSpace XmlSpace => impl.XmlSpace;

	public override string XmlLang => impl.XmlLang;

	public override int AttributeCount => impl.AttributeCount;

	public override bool EOF => impl.EOF;

	public override ReadState ReadState => impl.ReadState;

	public override XmlNameTable NameTable => impl.NameTable;

	public override bool CanResolveEntity => true;

	public override bool CanReadBinaryContent => true;

	public int LineNumber => impl.LineNumber;

	public int LinePosition => impl.LinePosition;

	public object SchemaType => impl.SchemaType;

	public XmlReader Reader => impl.Reader;

	public ValidationType ValidationType
	{
		get
		{
			return impl.ValidationType;
		}
		set
		{
			impl.ValidationType = value;
		}
	}

	public XmlSchemaCollection Schemas => impl.Schemas;

	public EntityHandling EntityHandling
	{
		get
		{
			return impl.EntityHandling;
		}
		set
		{
			impl.EntityHandling = value;
		}
	}

	public XmlResolver XmlResolver
	{
		set
		{
			impl.XmlResolver = value;
		}
	}

	public bool Namespaces
	{
		get
		{
			return impl.Namespaces;
		}
		set
		{
			impl.Namespaces = value;
		}
	}

	public Encoding Encoding => impl.Encoding;

	internal XmlValidatingReaderImpl Impl => impl;

	internal override IDtdInfo DtdInfo => impl.DtdInfo;

	public event ValidationEventHandler ValidationEventHandler
	{
		add
		{
			impl.ValidationEventHandler += value;
		}
		remove
		{
			impl.ValidationEventHandler -= value;
		}
	}

	public XmlValidatingReader(XmlReader reader)
	{
		impl = new XmlValidatingReaderImpl(reader);
		impl.OuterReader = this;
	}

	public XmlValidatingReader(string xmlFragment, XmlNodeType fragType, XmlParserContext context)
	{
		if (xmlFragment == null)
		{
			throw new ArgumentNullException("xmlFragment");
		}
		impl = new XmlValidatingReaderImpl(xmlFragment, fragType, context);
		impl.OuterReader = this;
	}

	public XmlValidatingReader(Stream xmlFragment, XmlNodeType fragType, XmlParserContext context)
	{
		if (xmlFragment == null)
		{
			throw new ArgumentNullException("xmlFragment");
		}
		impl = new XmlValidatingReaderImpl(xmlFragment, fragType, context);
		impl.OuterReader = this;
	}

	public override string GetAttribute(string name)
	{
		return impl.GetAttribute(name);
	}

	public override string GetAttribute(string localName, string namespaceURI)
	{
		return impl.GetAttribute(localName, namespaceURI);
	}

	public override string GetAttribute(int i)
	{
		return impl.GetAttribute(i);
	}

	public override bool MoveToAttribute(string name)
	{
		return impl.MoveToAttribute(name);
	}

	public override bool MoveToAttribute(string localName, string namespaceURI)
	{
		return impl.MoveToAttribute(localName, namespaceURI);
	}

	public override void MoveToAttribute(int i)
	{
		impl.MoveToAttribute(i);
	}

	public override bool MoveToFirstAttribute()
	{
		return impl.MoveToFirstAttribute();
	}

	public override bool MoveToNextAttribute()
	{
		return impl.MoveToNextAttribute();
	}

	public override bool MoveToElement()
	{
		return impl.MoveToElement();
	}

	public override bool ReadAttributeValue()
	{
		return impl.ReadAttributeValue();
	}

	public override bool Read()
	{
		return impl.Read();
	}

	public override void Close()
	{
		impl.Close();
	}

	public override string LookupNamespace(string prefix)
	{
		string text = impl.LookupNamespace(prefix);
		if (text != null && text.Length == 0)
		{
			text = null;
		}
		return text;
	}

	public override void ResolveEntity()
	{
		impl.ResolveEntity();
	}

	public override int ReadContentAsBase64(byte[] buffer, int index, int count)
	{
		return impl.ReadContentAsBase64(buffer, index, count);
	}

	public override int ReadElementContentAsBase64(byte[] buffer, int index, int count)
	{
		return impl.ReadElementContentAsBase64(buffer, index, count);
	}

	public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
	{
		return impl.ReadContentAsBinHex(buffer, index, count);
	}

	public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
	{
		return impl.ReadElementContentAsBinHex(buffer, index, count);
	}

	public override string ReadString()
	{
		impl.MoveOffEntityReference();
		return base.ReadString();
	}

	public bool HasLineInfo()
	{
		return true;
	}

	IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
	{
		return impl.GetNamespacesInScope(scope);
	}

	string IXmlNamespaceResolver.LookupNamespace(string prefix)
	{
		return impl.LookupNamespace(prefix);
	}

	string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
	{
		return impl.LookupPrefix(namespaceName);
	}

	public object ReadTypedValue()
	{
		return impl.ReadTypedValue();
	}
}
