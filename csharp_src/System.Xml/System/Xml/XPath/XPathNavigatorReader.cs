using System.Collections.Generic;
using System.Xml.Schema;

namespace System.Xml.XPath;

internal class XPathNavigatorReader : XmlReader, IXmlNamespaceResolver
{
	private enum State
	{
		Initial,
		Content,
		EndElement,
		Attribute,
		AttrVal,
		InReadBinary,
		EOF,
		Closed,
		Error
	}

	private XPathNavigator nav;

	private XPathNavigator navToRead;

	private int depth;

	private State state;

	private XmlNodeType nodeType;

	private int attrCount;

	private bool readEntireDocument;

	protected IXmlLineInfo lineInfo;

	protected IXmlSchemaInfo schemaInfo;

	private ReadContentAsBinaryHelper readBinaryHelper;

	private State savedState;

	internal const string space = "space";

	internal static XmlNodeType[] convertFromXPathNodeType = new XmlNodeType[10]
	{
		XmlNodeType.Document,
		XmlNodeType.Element,
		XmlNodeType.Attribute,
		XmlNodeType.Attribute,
		XmlNodeType.Text,
		XmlNodeType.SignificantWhitespace,
		XmlNodeType.Whitespace,
		XmlNodeType.ProcessingInstruction,
		XmlNodeType.Comment,
		XmlNodeType.None
	};

	internal object UnderlyingObject => nav.UnderlyingObject;

	protected bool IsReading
	{
		get
		{
			if (state > State.Initial)
			{
				return state < State.EOF;
			}
			return false;
		}
	}

	internal override XmlNamespaceManager NamespaceManager => XPathNavigator.GetNamespaces(this);

	public override XmlNameTable NameTable => navToRead.NameTable;

	public override XmlReaderSettings Settings => new XmlReaderSettings
	{
		NameTable = NameTable,
		ConformanceLevel = ConformanceLevel.Fragment,
		CheckCharacters = false,
		ReadOnly = true
	};

	public override IXmlSchemaInfo SchemaInfo
	{
		get
		{
			if (nodeType == XmlNodeType.Text)
			{
				return null;
			}
			return nav.SchemaInfo;
		}
	}

	public override Type ValueType => nav.ValueType;

	public override XmlNodeType NodeType => nodeType;

	public override string NamespaceURI
	{
		get
		{
			if (nav.NodeType == XPathNodeType.Namespace)
			{
				return NameTable.Add("http://www.w3.org/2000/xmlns/");
			}
			if (NodeType == XmlNodeType.Text)
			{
				return string.Empty;
			}
			return nav.NamespaceURI;
		}
	}

	public override string LocalName
	{
		get
		{
			if (nav.NodeType == XPathNodeType.Namespace && nav.LocalName.Length == 0)
			{
				return NameTable.Add("xmlns");
			}
			if (NodeType == XmlNodeType.Text)
			{
				return string.Empty;
			}
			return nav.LocalName;
		}
	}

	public override string Prefix
	{
		get
		{
			if (nav.NodeType == XPathNodeType.Namespace && nav.LocalName.Length != 0)
			{
				return NameTable.Add("xmlns");
			}
			if (NodeType == XmlNodeType.Text)
			{
				return string.Empty;
			}
			return nav.Prefix;
		}
	}

	public override string BaseURI
	{
		get
		{
			if (state == State.Initial)
			{
				return navToRead.BaseURI;
			}
			return nav.BaseURI;
		}
	}

	public override bool IsEmptyElement => nav.IsEmptyElement;

	public override XmlSpace XmlSpace
	{
		get
		{
			XPathNavigator xPathNavigator = nav.Clone();
			do
			{
				if (xPathNavigator.MoveToAttribute("space", "http://www.w3.org/XML/1998/namespace"))
				{
					string text = XmlConvert.TrimString(xPathNavigator.Value);
					if (text == "default")
					{
						return XmlSpace.Default;
					}
					if (text == "preserve")
					{
						return XmlSpace.Preserve;
					}
					xPathNavigator.MoveToParent();
				}
			}
			while (xPathNavigator.MoveToParent());
			return XmlSpace.None;
		}
	}

	public override string XmlLang => nav.XmlLang;

	public override bool HasValue
	{
		get
		{
			if (nodeType != XmlNodeType.Element && nodeType != XmlNodeType.Document && nodeType != XmlNodeType.EndElement && nodeType != XmlNodeType.None)
			{
				return true;
			}
			return false;
		}
	}

	public override string Value
	{
		get
		{
			if (nodeType != XmlNodeType.Element && nodeType != XmlNodeType.Document && nodeType != XmlNodeType.EndElement && nodeType != XmlNodeType.None)
			{
				return nav.Value;
			}
			return string.Empty;
		}
	}

	public override int AttributeCount
	{
		get
		{
			if (attrCount < 0)
			{
				XPathNavigator elemNav = GetElemNav();
				int num = 0;
				if (elemNav != null)
				{
					if (elemNav.MoveToFirstNamespace(XPathNamespaceScope.Local))
					{
						do
						{
							num++;
						}
						while (elemNav.MoveToNextNamespace(XPathNamespaceScope.Local));
						elemNav.MoveToParent();
					}
					if (elemNav.MoveToFirstAttribute())
					{
						do
						{
							num++;
						}
						while (elemNav.MoveToNextAttribute());
					}
				}
				attrCount = num;
			}
			return attrCount;
		}
	}

	public override bool EOF => state == State.EOF;

	public override ReadState ReadState
	{
		get
		{
			switch (state)
			{
			case State.Initial:
				return ReadState.Initial;
			case State.Content:
			case State.EndElement:
			case State.Attribute:
			case State.AttrVal:
			case State.InReadBinary:
				return ReadState.Interactive;
			case State.EOF:
				return ReadState.EndOfFile;
			case State.Closed:
				return ReadState.Closed;
			default:
				return ReadState.Error;
			}
		}
	}

	public override bool CanReadBinaryContent => true;

	public override int Depth => depth;

	internal static XmlNodeType ToXmlNodeType(XPathNodeType typ)
	{
		return convertFromXPathNodeType[(int)typ];
	}

	public static XPathNavigatorReader Create(XPathNavigator navToRead)
	{
		XPathNavigator xPathNavigator = navToRead.Clone();
		IXmlLineInfo xli = xPathNavigator as IXmlLineInfo;
		IXmlSchemaInfo xmlSchemaInfo = xPathNavigator as IXmlSchemaInfo;
		if (xmlSchemaInfo == null)
		{
			return new XPathNavigatorReader(xPathNavigator, xli, xmlSchemaInfo);
		}
		return new XPathNavigatorReaderWithSI(xPathNavigator, xli, xmlSchemaInfo);
	}

	protected XPathNavigatorReader(XPathNavigator navToRead, IXmlLineInfo xli, IXmlSchemaInfo xsi)
	{
		this.navToRead = navToRead;
		lineInfo = xli;
		schemaInfo = xsi;
		nav = XmlEmptyNavigator.Singleton;
		state = State.Initial;
		depth = 0;
		nodeType = ToXmlNodeType(nav.NodeType);
	}

	IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
	{
		return nav.GetNamespacesInScope(scope);
	}

	string IXmlNamespaceResolver.LookupNamespace(string prefix)
	{
		return nav.LookupNamespace(prefix);
	}

	string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
	{
		return nav.LookupPrefix(namespaceName);
	}

	private XPathNavigator GetElemNav()
	{
		switch (state)
		{
		case State.Content:
			return nav.Clone();
		case State.Attribute:
		case State.AttrVal:
		{
			XPathNavigator xPathNavigator = nav.Clone();
			if (xPathNavigator.MoveToParent())
			{
				return xPathNavigator;
			}
			break;
		}
		case State.InReadBinary:
		{
			state = savedState;
			XPathNavigator elemNav = GetElemNav();
			state = State.InReadBinary;
			return elemNav;
		}
		}
		return null;
	}

	private XPathNavigator GetElemNav(out int depth)
	{
		XPathNavigator xPathNavigator = null;
		switch (state)
		{
		case State.Content:
			if (nodeType == XmlNodeType.Element)
			{
				xPathNavigator = nav.Clone();
			}
			depth = this.depth;
			break;
		case State.Attribute:
			xPathNavigator = nav.Clone();
			xPathNavigator.MoveToParent();
			depth = this.depth - 1;
			break;
		case State.AttrVal:
			xPathNavigator = nav.Clone();
			xPathNavigator.MoveToParent();
			depth = this.depth - 2;
			break;
		case State.InReadBinary:
			state = savedState;
			xPathNavigator = GetElemNav(out depth);
			state = State.InReadBinary;
			break;
		default:
			depth = this.depth;
			break;
		}
		return xPathNavigator;
	}

	private void MoveToAttr(XPathNavigator nav, int depth)
	{
		this.nav.MoveTo(nav);
		this.depth = depth;
		nodeType = XmlNodeType.Attribute;
		state = State.Attribute;
	}

	public override string GetAttribute(string name)
	{
		XPathNavigator xPathNavigator = nav;
		switch (xPathNavigator.NodeType)
		{
		case XPathNodeType.Attribute:
			xPathNavigator = xPathNavigator.Clone();
			if (!xPathNavigator.MoveToParent())
			{
				return null;
			}
			break;
		default:
			return null;
		case XPathNodeType.Element:
			break;
		}
		ValidateNames.SplitQName(name, out var prefix, out var lname);
		if (prefix.Length == 0)
		{
			if (lname == "xmlns")
			{
				return xPathNavigator.GetNamespace(string.Empty);
			}
			if (xPathNavigator == nav)
			{
				xPathNavigator = xPathNavigator.Clone();
			}
			if (xPathNavigator.MoveToAttribute(lname, string.Empty))
			{
				return xPathNavigator.Value;
			}
		}
		else
		{
			if (prefix == "xmlns")
			{
				return xPathNavigator.GetNamespace(lname);
			}
			if (xPathNavigator == nav)
			{
				xPathNavigator = xPathNavigator.Clone();
			}
			if (xPathNavigator.MoveToFirstAttribute())
			{
				do
				{
					if (xPathNavigator.LocalName == lname && xPathNavigator.Prefix == prefix)
					{
						return xPathNavigator.Value;
					}
				}
				while (xPathNavigator.MoveToNextAttribute());
			}
		}
		return null;
	}

	public override string GetAttribute(string localName, string namespaceURI)
	{
		if (localName == null)
		{
			throw new ArgumentNullException("localName");
		}
		XPathNavigator xPathNavigator = nav;
		switch (xPathNavigator.NodeType)
		{
		case XPathNodeType.Attribute:
			xPathNavigator = xPathNavigator.Clone();
			if (!xPathNavigator.MoveToParent())
			{
				return null;
			}
			break;
		default:
			return null;
		case XPathNodeType.Element:
			break;
		}
		if (namespaceURI == "http://www.w3.org/2000/xmlns/")
		{
			if (localName == "xmlns")
			{
				localName = string.Empty;
			}
			return xPathNavigator.GetNamespace(localName);
		}
		if (namespaceURI == null)
		{
			namespaceURI = string.Empty;
		}
		if (xPathNavigator == nav)
		{
			xPathNavigator = xPathNavigator.Clone();
		}
		if (xPathNavigator.MoveToAttribute(localName, namespaceURI))
		{
			return xPathNavigator.Value;
		}
		return null;
	}

	private static string GetNamespaceByIndex(XPathNavigator nav, int index, out int count)
	{
		string value = nav.Value;
		string result = null;
		if (nav.MoveToNextNamespace(XPathNamespaceScope.Local))
		{
			result = GetNamespaceByIndex(nav, index, out count);
		}
		else
		{
			count = 0;
		}
		if (count == index)
		{
			result = value;
		}
		count++;
		return result;
	}

	public override string GetAttribute(int index)
	{
		if (index >= 0)
		{
			XPathNavigator elemNav = GetElemNav();
			if (elemNav != null)
			{
				if (elemNav.MoveToFirstNamespace(XPathNamespaceScope.Local))
				{
					int count;
					string namespaceByIndex = GetNamespaceByIndex(elemNav, index, out count);
					if (namespaceByIndex != null)
					{
						return namespaceByIndex;
					}
					index -= count;
					elemNav.MoveToParent();
				}
				if (elemNav.MoveToFirstAttribute())
				{
					do
					{
						if (index == 0)
						{
							return elemNav.Value;
						}
						index--;
					}
					while (elemNav.MoveToNextAttribute());
				}
			}
		}
		throw new ArgumentOutOfRangeException("index");
	}

	public override bool MoveToAttribute(string localName, string namespaceName)
	{
		if (localName == null)
		{
			throw new ArgumentNullException("localName");
		}
		int num = depth;
		XPathNavigator elemNav = GetElemNav(out num);
		if (elemNav != null)
		{
			if (namespaceName == "http://www.w3.org/2000/xmlns/")
			{
				if (localName == "xmlns")
				{
					localName = string.Empty;
				}
				if (!elemNav.MoveToFirstNamespace(XPathNamespaceScope.Local))
				{
					goto IL_0078;
				}
				while (!(elemNav.LocalName == localName))
				{
					if (elemNav.MoveToNextNamespace(XPathNamespaceScope.Local))
					{
						continue;
					}
					goto IL_0078;
				}
			}
			else
			{
				if (namespaceName == null)
				{
					namespaceName = string.Empty;
				}
				if (!elemNav.MoveToAttribute(localName, namespaceName))
				{
					goto IL_0078;
				}
			}
			if (state == State.InReadBinary)
			{
				readBinaryHelper.Finish();
				state = savedState;
			}
			MoveToAttr(elemNav, num + 1);
			return true;
		}
		goto IL_0078;
		IL_0078:
		return false;
	}

	public override bool MoveToFirstAttribute()
	{
		int num;
		XPathNavigator elemNav = GetElemNav(out num);
		if (elemNav != null)
		{
			if (elemNav.MoveToFirstNamespace(XPathNamespaceScope.Local))
			{
				while (elemNav.MoveToNextNamespace(XPathNamespaceScope.Local))
				{
				}
			}
			else if (!elemNav.MoveToFirstAttribute())
			{
				goto IL_0028;
			}
			if (state == State.InReadBinary)
			{
				readBinaryHelper.Finish();
				state = savedState;
			}
			MoveToAttr(elemNav, num + 1);
			return true;
		}
		goto IL_0028;
		IL_0028:
		return false;
	}

	public override bool MoveToNextAttribute()
	{
		switch (state)
		{
		case State.Content:
			return MoveToFirstAttribute();
		case State.Attribute:
		{
			if (XPathNodeType.Attribute == nav.NodeType)
			{
				return nav.MoveToNextAttribute();
			}
			XPathNavigator xPathNavigator = nav.Clone();
			if (!xPathNavigator.MoveToParent())
			{
				return false;
			}
			if (!xPathNavigator.MoveToFirstNamespace(XPathNamespaceScope.Local))
			{
				return false;
			}
			if (xPathNavigator.IsSamePosition(nav))
			{
				xPathNavigator.MoveToParent();
				if (!xPathNavigator.MoveToFirstAttribute())
				{
					return false;
				}
				nav.MoveTo(xPathNavigator);
				return true;
			}
			XPathNavigator xPathNavigator2 = xPathNavigator.Clone();
			while (true)
			{
				if (!xPathNavigator.MoveToNextNamespace(XPathNamespaceScope.Local))
				{
					return false;
				}
				if (xPathNavigator.IsSamePosition(nav))
				{
					break;
				}
				xPathNavigator2.MoveTo(xPathNavigator);
			}
			nav.MoveTo(xPathNavigator2);
			return true;
		}
		case State.AttrVal:
			depth--;
			state = State.Attribute;
			if (!MoveToNextAttribute())
			{
				depth++;
				state = State.AttrVal;
				return false;
			}
			nodeType = XmlNodeType.Attribute;
			return true;
		case State.InReadBinary:
			state = savedState;
			if (!MoveToNextAttribute())
			{
				state = State.InReadBinary;
				return false;
			}
			readBinaryHelper.Finish();
			return true;
		default:
			return false;
		}
	}

	public override bool MoveToAttribute(string name)
	{
		int num;
		XPathNavigator elemNav = GetElemNav(out num);
		if (elemNav == null)
		{
			return false;
		}
		ValidateNames.SplitQName(name, out var prefix, out var lname);
		bool flag = false;
		if ((flag = prefix.Length == 0 && lname == "xmlns") || prefix == "xmlns")
		{
			if (flag)
			{
				lname = string.Empty;
			}
			if (!elemNav.MoveToFirstNamespace(XPathNamespaceScope.Local))
			{
				goto IL_00b3;
			}
			while (!(elemNav.LocalName == lname))
			{
				if (elemNav.MoveToNextNamespace(XPathNamespaceScope.Local))
				{
					continue;
				}
				goto IL_00b3;
			}
		}
		else if (prefix.Length == 0)
		{
			if (!elemNav.MoveToAttribute(lname, string.Empty))
			{
				goto IL_00b3;
			}
		}
		else
		{
			if (!elemNav.MoveToFirstAttribute())
			{
				goto IL_00b3;
			}
			while (!(elemNav.LocalName == lname) || !(elemNav.Prefix == prefix))
			{
				if (elemNav.MoveToNextAttribute())
				{
					continue;
				}
				goto IL_00b3;
			}
		}
		if (state == State.InReadBinary)
		{
			readBinaryHelper.Finish();
			state = savedState;
		}
		MoveToAttr(elemNav, num + 1);
		return true;
		IL_00b3:
		return false;
	}

	public override bool MoveToElement()
	{
		switch (state)
		{
		case State.Attribute:
		case State.AttrVal:
			if (!nav.MoveToParent())
			{
				return false;
			}
			depth--;
			if (state == State.AttrVal)
			{
				depth--;
			}
			state = State.Content;
			nodeType = XmlNodeType.Element;
			return true;
		case State.InReadBinary:
			state = savedState;
			if (!MoveToElement())
			{
				state = State.InReadBinary;
				return false;
			}
			readBinaryHelper.Finish();
			break;
		}
		return false;
	}

	public override void ResolveEntity()
	{
		throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
	}

	public override bool ReadAttributeValue()
	{
		if (state == State.InReadBinary)
		{
			readBinaryHelper.Finish();
			state = savedState;
		}
		if (state == State.Attribute)
		{
			state = State.AttrVal;
			nodeType = XmlNodeType.Text;
			depth++;
			return true;
		}
		return false;
	}

	public override int ReadContentAsBase64(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (state != State.InReadBinary)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
			savedState = state;
		}
		state = savedState;
		int result = readBinaryHelper.ReadContentAsBase64(buffer, index, count);
		savedState = state;
		state = State.InReadBinary;
		return result;
	}

	public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (state != State.InReadBinary)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
			savedState = state;
		}
		state = savedState;
		int result = readBinaryHelper.ReadContentAsBinHex(buffer, index, count);
		savedState = state;
		state = State.InReadBinary;
		return result;
	}

	public override int ReadElementContentAsBase64(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (state != State.InReadBinary)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
			savedState = state;
		}
		state = savedState;
		int result = readBinaryHelper.ReadElementContentAsBase64(buffer, index, count);
		savedState = state;
		state = State.InReadBinary;
		return result;
	}

	public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (state != State.InReadBinary)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
			savedState = state;
		}
		state = savedState;
		int result = readBinaryHelper.ReadElementContentAsBinHex(buffer, index, count);
		savedState = state;
		state = State.InReadBinary;
		return result;
	}

	public override string LookupNamespace(string prefix)
	{
		return nav.LookupNamespace(prefix);
	}

	public override bool Read()
	{
		attrCount = -1;
		switch (state)
		{
		case State.EOF:
		case State.Closed:
		case State.Error:
			return false;
		case State.Initial:
			nav = navToRead;
			state = State.Content;
			if (nav.NodeType == XPathNodeType.Root)
			{
				if (!nav.MoveToFirstChild())
				{
					SetEOF();
					return false;
				}
				readEntireDocument = true;
			}
			else if (XPathNodeType.Attribute == nav.NodeType)
			{
				state = State.Attribute;
			}
			nodeType = ToXmlNodeType(nav.NodeType);
			break;
		case State.Content:
			if (nav.MoveToFirstChild())
			{
				nodeType = ToXmlNodeType(nav.NodeType);
				depth++;
				state = State.Content;
				break;
			}
			if (nodeType == XmlNodeType.Element && !nav.IsEmptyElement)
			{
				nodeType = XmlNodeType.EndElement;
				state = State.EndElement;
				break;
			}
			goto case State.EndElement;
		case State.EndElement:
			if (depth == 0 && !readEntireDocument)
			{
				SetEOF();
				return false;
			}
			if (nav.MoveToNext())
			{
				nodeType = ToXmlNodeType(nav.NodeType);
				state = State.Content;
				break;
			}
			if (depth > 0 && nav.MoveToParent())
			{
				nodeType = XmlNodeType.EndElement;
				state = State.EndElement;
				depth--;
				break;
			}
			SetEOF();
			return false;
		case State.Attribute:
		case State.AttrVal:
			if (!nav.MoveToParent())
			{
				SetEOF();
				return false;
			}
			nodeType = ToXmlNodeType(nav.NodeType);
			depth--;
			if (state == State.AttrVal)
			{
				depth--;
			}
			goto case State.Content;
		case State.InReadBinary:
			state = savedState;
			readBinaryHelper.Finish();
			return Read();
		}
		return true;
	}

	public override void Close()
	{
		nav = XmlEmptyNavigator.Singleton;
		nodeType = XmlNodeType.None;
		state = State.Closed;
		depth = 0;
	}

	private void SetEOF()
	{
		nav = XmlEmptyNavigator.Singleton;
		nodeType = XmlNodeType.None;
		state = State.EOF;
		depth = 0;
	}
}
