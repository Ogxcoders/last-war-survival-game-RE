using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Permissions;
using System.Text;

namespace System.Xml;

[EditorBrowsable(EditorBrowsableState.Never)]
[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
public class XmlTextReader : XmlReader, IXmlLineInfo, IXmlNamespaceResolver
{
	private XmlTextReaderImpl impl;

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

	public override bool CanReadValueChunk => false;

	public int LineNumber => impl.LineNumber;

	public int LinePosition => impl.LinePosition;

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

	public bool Normalization
	{
		get
		{
			return impl.Normalization;
		}
		set
		{
			impl.Normalization = value;
		}
	}

	public Encoding Encoding => impl.Encoding;

	public WhitespaceHandling WhitespaceHandling
	{
		get
		{
			return impl.WhitespaceHandling;
		}
		set
		{
			impl.WhitespaceHandling = value;
		}
	}

	[Obsolete("Use DtdProcessing property instead.")]
	public bool ProhibitDtd
	{
		get
		{
			return impl.DtdProcessing == DtdProcessing.Prohibit;
		}
		set
		{
			impl.DtdProcessing = ((!value) ? DtdProcessing.Parse : DtdProcessing.Prohibit);
		}
	}

	public DtdProcessing DtdProcessing
	{
		get
		{
			return impl.DtdProcessing;
		}
		set
		{
			impl.DtdProcessing = value;
		}
	}

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

	internal XmlTextReaderImpl Impl => impl;

	internal override XmlNamespaceManager NamespaceManager => impl.NamespaceManager;

	internal bool XmlValidatingReaderCompatibilityMode
	{
		set
		{
			impl.XmlValidatingReaderCompatibilityMode = value;
		}
	}

	internal override IDtdInfo DtdInfo => impl.DtdInfo;

	protected XmlTextReader()
	{
		impl = new XmlTextReaderImpl();
		impl.OuterReader = this;
	}

	protected XmlTextReader(XmlNameTable nt)
	{
		impl = new XmlTextReaderImpl(nt);
		impl.OuterReader = this;
	}

	public XmlTextReader(Stream input)
	{
		impl = new XmlTextReaderImpl(input);
		impl.OuterReader = this;
	}

	public XmlTextReader(string url, Stream input)
	{
		impl = new XmlTextReaderImpl(url, input);
		impl.OuterReader = this;
	}

	public XmlTextReader(Stream input, XmlNameTable nt)
	{
		impl = new XmlTextReaderImpl(input, nt);
		impl.OuterReader = this;
	}

	public XmlTextReader(string url, Stream input, XmlNameTable nt)
	{
		impl = new XmlTextReaderImpl(url, input, nt);
		impl.OuterReader = this;
	}

	public XmlTextReader(TextReader input)
	{
		impl = new XmlTextReaderImpl(input);
		impl.OuterReader = this;
	}

	public XmlTextReader(string url, TextReader input)
	{
		impl = new XmlTextReaderImpl(url, input);
		impl.OuterReader = this;
	}

	public XmlTextReader(TextReader input, XmlNameTable nt)
	{
		impl = new XmlTextReaderImpl(input, nt);
		impl.OuterReader = this;
	}

	public XmlTextReader(string url, TextReader input, XmlNameTable nt)
	{
		impl = new XmlTextReaderImpl(url, input, nt);
		impl.OuterReader = this;
	}

	public XmlTextReader(Stream xmlFragment, XmlNodeType fragType, XmlParserContext context)
	{
		impl = new XmlTextReaderImpl(xmlFragment, fragType, context);
		impl.OuterReader = this;
	}

	public XmlTextReader(string xmlFragment, XmlNodeType fragType, XmlParserContext context)
	{
		impl = new XmlTextReaderImpl(xmlFragment, fragType, context);
		impl.OuterReader = this;
	}

	public XmlTextReader(string url)
	{
		impl = new XmlTextReaderImpl(url, new NameTable());
		impl.OuterReader = this;
	}

	public XmlTextReader(string url, XmlNameTable nt)
	{
		impl = new XmlTextReaderImpl(url, nt);
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

	public override void Skip()
	{
		impl.Skip();
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

	public IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
	{
		return impl.GetNamespacesInScope(scope);
	}

	public void ResetState()
	{
		impl.ResetState();
	}

	public TextReader GetRemainder()
	{
		return impl.GetRemainder();
	}

	public int ReadChars(char[] buffer, int index, int count)
	{
		return impl.ReadChars(buffer, index, count);
	}

	public int ReadBase64(byte[] array, int offset, int len)
	{
		return impl.ReadBase64(array, offset, len);
	}

	public int ReadBinHex(byte[] array, int offset, int len)
	{
		return impl.ReadBinHex(array, offset, len);
	}
}
