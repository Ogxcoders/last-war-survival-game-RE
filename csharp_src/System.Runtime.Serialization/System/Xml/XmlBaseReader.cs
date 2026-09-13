using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace System.Xml;

internal abstract class XmlBaseReader : XmlDictionaryReader
{
	protected enum QNameType
	{
		Normal,
		Xmlns
	}

	protected class XmlNode
	{
		protected enum XmlNodeFlags
		{
			None = 0,
			CanGetAttribute = 1,
			CanMoveToElement = 2,
			HasValue = 4,
			AtomicValue = 8,
			SkipValue = 0x10,
			HasContent = 0x20
		}

		private XmlNodeType nodeType;

		private PrefixHandle prefix;

		private StringHandle localName;

		private ValueHandle value;

		private Namespace ns;

		private bool hasValue;

		private bool canGetAttribute;

		private bool canMoveToElement;

		private ReadState readState;

		private XmlAttributeTextNode attributeTextNode;

		private bool exitScope;

		private int depthDelta;

		private bool isAtomicValue;

		private bool skipValue;

		private QNameType qnameType;

		private bool hasContent;

		private bool isEmptyElement;

		private char quoteChar;

		public bool HasValue => hasValue;

		public ReadState ReadState => readState;

		public StringHandle LocalName => localName;

		public PrefixHandle Prefix => prefix;

		public bool CanGetAttribute => canGetAttribute;

		public bool CanMoveToElement => canMoveToElement;

		public XmlAttributeTextNode AttributeText => attributeTextNode;

		public bool SkipValue => skipValue;

		public ValueHandle Value => value;

		public int DepthDelta => depthDelta;

		public bool HasContent => hasContent;

		public XmlNodeType NodeType
		{
			get
			{
				return nodeType;
			}
			set
			{
				nodeType = value;
			}
		}

		public QNameType QNameType
		{
			get
			{
				return qnameType;
			}
			set
			{
				qnameType = value;
			}
		}

		public Namespace Namespace
		{
			get
			{
				return ns;
			}
			set
			{
				ns = value;
			}
		}

		public bool IsAtomicValue
		{
			get
			{
				return isAtomicValue;
			}
			set
			{
				isAtomicValue = value;
			}
		}

		public bool ExitScope
		{
			get
			{
				return exitScope;
			}
			set
			{
				exitScope = value;
			}
		}

		public bool IsEmptyElement
		{
			get
			{
				return isEmptyElement;
			}
			set
			{
				isEmptyElement = value;
			}
		}

		public char QuoteChar
		{
			get
			{
				return quoteChar;
			}
			set
			{
				quoteChar = value;
			}
		}

		public string ValueAsString
		{
			get
			{
				if (qnameType == QNameType.Normal)
				{
					return Value.GetString();
				}
				return Namespace.Uri.GetString();
			}
		}

		protected XmlNode(XmlNodeType nodeType, PrefixHandle prefix, StringHandle localName, ValueHandle value, XmlNodeFlags nodeFlags, ReadState readState, XmlAttributeTextNode attributeTextNode, int depthDelta)
		{
			this.nodeType = nodeType;
			this.prefix = prefix;
			this.localName = localName;
			this.value = value;
			ns = NamespaceManager.EmptyNamespace;
			hasValue = (nodeFlags & XmlNodeFlags.HasValue) != 0;
			canGetAttribute = (nodeFlags & XmlNodeFlags.CanGetAttribute) != 0;
			canMoveToElement = (nodeFlags & XmlNodeFlags.CanMoveToElement) != 0;
			isAtomicValue = (nodeFlags & XmlNodeFlags.AtomicValue) != 0;
			skipValue = (nodeFlags & XmlNodeFlags.SkipValue) != 0;
			hasContent = (nodeFlags & XmlNodeFlags.HasContent) != 0;
			this.readState = readState;
			this.attributeTextNode = attributeTextNode;
			exitScope = nodeType == XmlNodeType.EndElement;
			this.depthDelta = depthDelta;
			isEmptyElement = false;
			quoteChar = '"';
			qnameType = QNameType.Normal;
		}

		public bool IsLocalName(string localName)
		{
			if (qnameType == QNameType.Normal)
			{
				return LocalName == localName;
			}
			return Namespace.Prefix == localName;
		}

		public bool IsLocalName(XmlDictionaryString localName)
		{
			if (qnameType == QNameType.Normal)
			{
				return LocalName == localName;
			}
			return Namespace.Prefix == localName;
		}

		public bool IsNamespaceUri(string ns)
		{
			if (qnameType == QNameType.Normal)
			{
				return Namespace.IsUri(ns);
			}
			return ns == "http://www.w3.org/2000/xmlns/";
		}

		public bool IsNamespaceUri(XmlDictionaryString ns)
		{
			if (qnameType == QNameType.Normal)
			{
				return Namespace.IsUri(ns);
			}
			return ns.Value == "http://www.w3.org/2000/xmlns/";
		}

		public bool IsLocalNameAndNamespaceUri(string localName, string ns)
		{
			if (qnameType == QNameType.Normal)
			{
				if (LocalName == localName)
				{
					return Namespace.IsUri(ns);
				}
				return false;
			}
			if (Namespace.Prefix == localName)
			{
				return ns == "http://www.w3.org/2000/xmlns/";
			}
			return false;
		}

		public bool IsLocalNameAndNamespaceUri(XmlDictionaryString localName, XmlDictionaryString ns)
		{
			if (qnameType == QNameType.Normal)
			{
				if (LocalName == localName)
				{
					return Namespace.IsUri(ns);
				}
				return false;
			}
			if (Namespace.Prefix == localName)
			{
				return ns.Value == "http://www.w3.org/2000/xmlns/";
			}
			return false;
		}

		public bool IsPrefixAndLocalName(string prefix, string localName)
		{
			if (qnameType == QNameType.Normal)
			{
				if (Prefix == prefix)
				{
					return LocalName == localName;
				}
				return false;
			}
			if (prefix == "xmlns")
			{
				return Namespace.Prefix == localName;
			}
			return false;
		}

		public bool TryGetLocalNameAsDictionaryString(out XmlDictionaryString localName)
		{
			if (qnameType == QNameType.Normal)
			{
				return LocalName.TryGetDictionaryString(out localName);
			}
			localName = null;
			return false;
		}

		public bool TryGetNamespaceUriAsDictionaryString(out XmlDictionaryString ns)
		{
			if (qnameType == QNameType.Normal)
			{
				return Namespace.Uri.TryGetDictionaryString(out ns);
			}
			ns = null;
			return false;
		}

		public bool TryGetValueAsDictionaryString(out XmlDictionaryString value)
		{
			if (qnameType == QNameType.Normal)
			{
				return Value.TryGetDictionaryString(out value);
			}
			value = null;
			return false;
		}
	}

	protected class XmlElementNode : XmlNode
	{
		private XmlEndElementNode endElementNode;

		private int bufferOffset;

		public int NameOffset;

		public int NameLength;

		public XmlEndElementNode EndElement => endElementNode;

		public int BufferOffset
		{
			get
			{
				return bufferOffset;
			}
			set
			{
				bufferOffset = value;
			}
		}

		public XmlElementNode(XmlBufferReader bufferReader)
			: this(new PrefixHandle(bufferReader), new StringHandle(bufferReader), new ValueHandle(bufferReader))
		{
		}

		private XmlElementNode(PrefixHandle prefix, StringHandle localName, ValueHandle value)
			: base(XmlNodeType.Element, prefix, localName, value, (XmlNodeFlags)33, ReadState.Interactive, null, -1)
		{
			endElementNode = new XmlEndElementNode(prefix, localName, value);
		}
	}

	protected class XmlAttributeNode : XmlNode
	{
		public XmlAttributeNode(XmlBufferReader bufferReader)
			: this(new PrefixHandle(bufferReader), new StringHandle(bufferReader), new ValueHandle(bufferReader))
		{
		}

		private XmlAttributeNode(PrefixHandle prefix, StringHandle localName, ValueHandle value)
			: base(XmlNodeType.Attribute, prefix, localName, value, (XmlNodeFlags)15, ReadState.Interactive, new XmlAttributeTextNode(prefix, localName, value), 0)
		{
		}
	}

	protected class XmlEndElementNode : XmlNode
	{
		public XmlEndElementNode(PrefixHandle prefix, StringHandle localName, ValueHandle value)
			: base(XmlNodeType.EndElement, prefix, localName, value, XmlNodeFlags.HasContent, ReadState.Interactive, null, -1)
		{
		}
	}

	protected class XmlTextNode : XmlNode
	{
		protected XmlTextNode(XmlNodeType nodeType, PrefixHandle prefix, StringHandle localName, ValueHandle value, XmlNodeFlags nodeFlags, ReadState readState, XmlAttributeTextNode attributeTextNode, int depthDelta)
			: base(nodeType, prefix, localName, value, nodeFlags, readState, attributeTextNode, depthDelta)
		{
		}
	}

	protected class XmlAtomicTextNode : XmlTextNode
	{
		public XmlAtomicTextNode(XmlBufferReader bufferReader)
			: base(XmlNodeType.Text, new PrefixHandle(bufferReader), new StringHandle(bufferReader), new ValueHandle(bufferReader), (XmlNodeFlags)60, ReadState.Interactive, null, 0)
		{
		}
	}

	protected class XmlComplexTextNode : XmlTextNode
	{
		public XmlComplexTextNode(XmlBufferReader bufferReader)
			: base(XmlNodeType.Text, new PrefixHandle(bufferReader), new StringHandle(bufferReader), new ValueHandle(bufferReader), (XmlNodeFlags)36, ReadState.Interactive, null, 0)
		{
		}
	}

	protected class XmlWhitespaceTextNode : XmlTextNode
	{
		public XmlWhitespaceTextNode(XmlBufferReader bufferReader)
			: base(XmlNodeType.Whitespace, new PrefixHandle(bufferReader), new StringHandle(bufferReader), new ValueHandle(bufferReader), XmlNodeFlags.HasValue, ReadState.Interactive, null, 0)
		{
		}
	}

	protected class XmlCDataNode : XmlTextNode
	{
		public XmlCDataNode(XmlBufferReader bufferReader)
			: base(XmlNodeType.CDATA, new PrefixHandle(bufferReader), new StringHandle(bufferReader), new ValueHandle(bufferReader), (XmlNodeFlags)36, ReadState.Interactive, null, 0)
		{
		}
	}

	protected class XmlAttributeTextNode : XmlTextNode
	{
		public XmlAttributeTextNode(PrefixHandle prefix, StringHandle localName, ValueHandle value)
			: base(XmlNodeType.Text, prefix, localName, value, (XmlNodeFlags)47, ReadState.Interactive, null, 1)
		{
		}
	}

	protected class XmlInitialNode : XmlNode
	{
		public XmlInitialNode(XmlBufferReader bufferReader)
			: base(XmlNodeType.None, new PrefixHandle(bufferReader), new StringHandle(bufferReader), new ValueHandle(bufferReader), XmlNodeFlags.None, ReadState.Initial, null, 0)
		{
		}
	}

	protected class XmlDeclarationNode : XmlNode
	{
		public XmlDeclarationNode(XmlBufferReader bufferReader)
			: base(XmlNodeType.XmlDeclaration, new PrefixHandle(bufferReader), new StringHandle(bufferReader), new ValueHandle(bufferReader), XmlNodeFlags.CanGetAttribute, ReadState.Interactive, null, 0)
		{
		}
	}

	protected class XmlCommentNode : XmlNode
	{
		public XmlCommentNode(XmlBufferReader bufferReader)
			: base(XmlNodeType.Comment, new PrefixHandle(bufferReader), new StringHandle(bufferReader), new ValueHandle(bufferReader), XmlNodeFlags.HasValue, ReadState.Interactive, null, 0)
		{
		}
	}

	protected class XmlEndOfFileNode : XmlNode
	{
		public XmlEndOfFileNode(XmlBufferReader bufferReader)
			: base(XmlNodeType.None, new PrefixHandle(bufferReader), new StringHandle(bufferReader), new ValueHandle(bufferReader), XmlNodeFlags.None, ReadState.EndOfFile, null, 0)
		{
		}
	}

	protected class XmlClosedNode : XmlNode
	{
		public XmlClosedNode(XmlBufferReader bufferReader)
			: base(XmlNodeType.None, new PrefixHandle(bufferReader), new StringHandle(bufferReader), new ValueHandle(bufferReader), XmlNodeFlags.None, ReadState.Closed, null, 0)
		{
		}
	}

	private class AttributeSorter : IComparer
	{
		private object[] indeces;

		private XmlAttributeNode[] attributeNodes;

		private int attributeCount;

		private int attributeIndex1;

		private int attributeIndex2;

		public bool Sort(XmlAttributeNode[] attributeNodes, int attributeCount)
		{
			attributeIndex1 = -1;
			attributeIndex2 = -1;
			this.attributeNodes = attributeNodes;
			this.attributeCount = attributeCount;
			bool result = Sort();
			this.attributeNodes = null;
			this.attributeCount = 0;
			return result;
		}

		public void GetIndeces(out int attributeIndex1, out int attributeIndex2)
		{
			attributeIndex1 = this.attributeIndex1;
			attributeIndex2 = this.attributeIndex2;
		}

		public void Close()
		{
			if (indeces != null && indeces.Length > 32)
			{
				indeces = null;
			}
		}

		private bool Sort()
		{
			if (indeces != null && indeces.Length == attributeCount && IsSorted())
			{
				return true;
			}
			object[] array = new object[attributeCount];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = i;
			}
			indeces = array;
			Array.Sort(indeces, 0, attributeCount, this);
			return IsSorted();
		}

		private bool IsSorted()
		{
			for (int i = 0; i < indeces.Length - 1; i++)
			{
				if (Compare(indeces[i], indeces[i + 1]) >= 0)
				{
					attributeIndex1 = (int)indeces[i];
					attributeIndex2 = (int)indeces[i + 1];
					return false;
				}
			}
			return true;
		}

		public int Compare(object obj1, object obj2)
		{
			int num = (int)obj1;
			int num2 = (int)obj2;
			XmlAttributeNode xmlAttributeNode = attributeNodes[num];
			XmlAttributeNode xmlAttributeNode2 = attributeNodes[num2];
			int num3 = CompareQNameType(xmlAttributeNode.QNameType, xmlAttributeNode2.QNameType);
			if (num3 == 0)
			{
				if (xmlAttributeNode.QNameType == QNameType.Normal)
				{
					num3 = xmlAttributeNode.LocalName.CompareTo(xmlAttributeNode2.LocalName);
					if (num3 == 0)
					{
						num3 = xmlAttributeNode.Namespace.Uri.CompareTo(xmlAttributeNode2.Namespace.Uri);
					}
				}
				else
				{
					num3 = xmlAttributeNode.Namespace.Prefix.CompareTo(xmlAttributeNode2.Namespace.Prefix);
				}
			}
			return num3;
		}

		public int CompareQNameType(QNameType type1, QNameType type2)
		{
			return type1 - type2;
		}
	}

	private class NamespaceManager
	{
		private class XmlAttribute
		{
			private XmlSpace space;

			private string lang;

			private int depth;

			public int Depth
			{
				get
				{
					return depth;
				}
				set
				{
					depth = value;
				}
			}

			public string XmlLang
			{
				get
				{
					return lang;
				}
				set
				{
					lang = value;
				}
			}

			public XmlSpace XmlSpace
			{
				get
				{
					return space;
				}
				set
				{
					space = value;
				}
			}
		}

		private XmlBufferReader bufferReader;

		private Namespace[] namespaces;

		private int nsCount;

		private int depth;

		private Namespace[] shortPrefixUri;

		private static Namespace emptyNamespace = new Namespace(XmlBufferReader.Empty);

		private static Namespace xmlNamespace;

		private XmlAttribute[] attributes;

		private int attributeCount;

		private XmlSpace space;

		private string lang;

		public static Namespace XmlNamespace
		{
			get
			{
				if (xmlNamespace == null)
				{
					byte[] array = new byte[39]
					{
						120, 109, 108, 104, 116, 116, 112, 58, 47, 47,
						119, 119, 119, 46, 119, 51, 46, 111, 114, 103,
						47, 88, 77, 76, 47, 49, 57, 57, 56, 47,
						110, 97, 109, 101, 115, 112, 97, 99, 101
					};
					Namespace obj = new Namespace(new XmlBufferReader(array));
					obj.Prefix.SetValue(0, 3);
					obj.Uri.SetValue(3, array.Length - 3);
					xmlNamespace = obj;
				}
				return xmlNamespace;
			}
		}

		public static Namespace EmptyNamespace => emptyNamespace;

		public string XmlLang => lang;

		public XmlSpace XmlSpace => space;

		public NamespaceManager(XmlBufferReader bufferReader)
		{
			this.bufferReader = bufferReader;
			shortPrefixUri = new Namespace[28];
			shortPrefixUri[0] = emptyNamespace;
			namespaces = null;
			nsCount = 0;
			attributes = null;
			attributeCount = 0;
			space = XmlSpace.None;
			lang = string.Empty;
			depth = 0;
		}

		public void Close()
		{
			if (namespaces != null && namespaces.Length > 32)
			{
				namespaces = null;
			}
			if (attributes != null && attributes.Length > 4)
			{
				attributes = null;
			}
			lang = string.Empty;
		}

		public void Clear()
		{
			if (nsCount != 0)
			{
				if (shortPrefixUri != null)
				{
					for (int i = 0; i < shortPrefixUri.Length; i++)
					{
						shortPrefixUri[i] = null;
					}
				}
				shortPrefixUri[0] = emptyNamespace;
				nsCount = 0;
			}
			attributeCount = 0;
			space = XmlSpace.None;
			lang = string.Empty;
			depth = 0;
		}

		public void EnterScope()
		{
			depth++;
		}

		public void ExitScope()
		{
			while (nsCount > 0)
			{
				Namespace obj = namespaces[nsCount - 1];
				if (obj.Depth != depth)
				{
					break;
				}
				if (obj.Prefix.TryGetShortPrefix(out var type))
				{
					shortPrefixUri[(int)type] = obj.OuterUri;
				}
				nsCount--;
			}
			while (attributeCount > 0)
			{
				XmlAttribute xmlAttribute = attributes[attributeCount - 1];
				if (xmlAttribute.Depth != depth)
				{
					break;
				}
				space = xmlAttribute.XmlSpace;
				lang = xmlAttribute.XmlLang;
				attributeCount--;
			}
			depth--;
		}

		public void Sign(XmlSigningNodeWriter writer)
		{
			for (int i = 0; i < nsCount; i++)
			{
				PrefixHandle prefix = namespaces[i].Prefix;
				bool flag = false;
				for (int j = i + 1; j < nsCount; j++)
				{
					if (object.Equals(prefix, namespaces[j].Prefix))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					int offset;
					int length;
					byte[] prefixBuffer = prefix.GetString(out offset, out length);
					int offset2;
					int length2;
					byte[] nsBuffer = namespaces[i].Uri.GetString(out offset2, out length2);
					writer.WriteXmlnsAttribute(prefixBuffer, offset, length, nsBuffer, offset2, length2);
				}
			}
		}

		public void AddLangAttribute(string lang)
		{
			AddAttribute();
			this.lang = lang;
		}

		public void AddSpaceAttribute(XmlSpace space)
		{
			AddAttribute();
			this.space = space;
		}

		private void AddAttribute()
		{
			if (attributes == null)
			{
				attributes = new XmlAttribute[1];
			}
			else if (attributes.Length == attributeCount)
			{
				XmlAttribute[] destinationArray = new XmlAttribute[attributeCount * 2];
				Array.Copy(attributes, destinationArray, attributeCount);
				attributes = destinationArray;
			}
			XmlAttribute xmlAttribute = attributes[attributeCount];
			if (xmlAttribute == null)
			{
				xmlAttribute = new XmlAttribute();
				attributes[attributeCount] = xmlAttribute;
			}
			xmlAttribute.XmlLang = lang;
			xmlAttribute.XmlSpace = space;
			xmlAttribute.Depth = depth;
			attributeCount++;
		}

		public void Register(Namespace nameSpace)
		{
			if (nameSpace.Prefix.TryGetShortPrefix(out var type))
			{
				nameSpace.OuterUri = shortPrefixUri[(int)type];
				shortPrefixUri[(int)type] = nameSpace;
			}
			else
			{
				nameSpace.OuterUri = null;
			}
		}

		public Namespace AddNamespace()
		{
			if (namespaces == null)
			{
				namespaces = new Namespace[4];
			}
			else if (namespaces.Length == nsCount)
			{
				Namespace[] destinationArray = new Namespace[nsCount * 2];
				Array.Copy(namespaces, destinationArray, nsCount);
				namespaces = destinationArray;
			}
			Namespace obj = namespaces[nsCount];
			if (obj == null)
			{
				obj = new Namespace(bufferReader);
				namespaces[nsCount] = obj;
			}
			obj.Clear();
			obj.Depth = depth;
			nsCount++;
			return obj;
		}

		public Namespace LookupNamespace(PrefixHandleType prefix)
		{
			return shortPrefixUri[(int)prefix];
		}

		public Namespace LookupNamespace(PrefixHandle prefix)
		{
			if (prefix.TryGetShortPrefix(out var type))
			{
				return LookupNamespace(type);
			}
			for (int num = nsCount - 1; num >= 0; num--)
			{
				Namespace obj = namespaces[num];
				if (obj.Prefix == prefix)
				{
					return obj;
				}
			}
			if (prefix.IsXml)
			{
				return XmlNamespace;
			}
			return null;
		}

		public Namespace LookupNamespace(string prefix)
		{
			if (TryGetShortPrefix(prefix, out var shortPrefix))
			{
				return LookupNamespace(shortPrefix);
			}
			for (int num = nsCount - 1; num >= 0; num--)
			{
				Namespace obj = namespaces[num];
				if (obj.Prefix == prefix)
				{
					return obj;
				}
			}
			if (prefix == "xml")
			{
				return XmlNamespace;
			}
			return null;
		}

		private bool TryGetShortPrefix(string s, out PrefixHandleType shortPrefix)
		{
			switch (s.Length)
			{
			case 0:
				shortPrefix = PrefixHandleType.Empty;
				return true;
			case 1:
			{
				char c = s[0];
				if (c >= 'a' && c <= 'z')
				{
					shortPrefix = PrefixHandle.GetAlphaPrefix(c - 97);
					return true;
				}
				break;
			}
			}
			shortPrefix = PrefixHandleType.Empty;
			return false;
		}
	}

	protected class Namespace
	{
		private PrefixHandle prefix;

		private StringHandle uri;

		private int depth;

		private Namespace outerUri;

		private string uriString;

		public int Depth
		{
			get
			{
				return depth;
			}
			set
			{
				depth = value;
			}
		}

		public PrefixHandle Prefix => prefix;

		public StringHandle Uri => uri;

		public Namespace OuterUri
		{
			get
			{
				return outerUri;
			}
			set
			{
				outerUri = value;
			}
		}

		public Namespace(XmlBufferReader bufferReader)
		{
			prefix = new PrefixHandle(bufferReader);
			uri = new StringHandle(bufferReader);
			outerUri = null;
			uriString = null;
		}

		public void Clear()
		{
			uriString = null;
		}

		public bool IsUri(string s)
		{
			if ((object)s == uriString)
			{
				return true;
			}
			if (uri == s)
			{
				uriString = s;
				return true;
			}
			return false;
		}

		public bool IsUri(XmlDictionaryString s)
		{
			if ((object)s.Value == uriString)
			{
				return true;
			}
			if (uri == s)
			{
				uriString = s.Value;
				return true;
			}
			return false;
		}
	}

	private class QuotaNameTable : XmlNameTable
	{
		private XmlDictionaryReader reader;

		private XmlNameTable nameTable;

		private int maxCharCount;

		private int charCount;

		public QuotaNameTable(XmlDictionaryReader reader, int maxCharCount)
		{
			this.reader = reader;
			nameTable = new NameTable();
			this.maxCharCount = maxCharCount;
			charCount = 0;
		}

		public override string Get(char[] chars, int offset, int count)
		{
			return nameTable.Get(chars, offset, count);
		}

		public override string Get(string value)
		{
			return nameTable.Get(value);
		}

		private void Add(int charCount)
		{
			if (charCount > maxCharCount - this.charCount)
			{
				XmlExceptionHelper.ThrowMaxNameTableCharCountExceeded(reader, maxCharCount);
			}
			this.charCount += charCount;
		}

		public override string Add(char[] chars, int offset, int count)
		{
			string text = nameTable.Get(chars, offset, count);
			if (text != null)
			{
				return text;
			}
			Add(count);
			return nameTable.Add(chars, offset, count);
		}

		public override string Add(string value)
		{
			string text = nameTable.Get(value);
			if (text != null)
			{
				return text;
			}
			Add(value.Length);
			return nameTable.Add(value);
		}
	}

	private XmlBufferReader bufferReader;

	private XmlNode node;

	private NamespaceManager nsMgr;

	private XmlElementNode[] elementNodes;

	private XmlAttributeNode[] attributeNodes;

	private XmlAtomicTextNode atomicTextNode;

	private int depth;

	private int attributeCount;

	private int attributeStart;

	private XmlDictionaryReaderQuotas quotas;

	private XmlNameTable nameTable;

	private XmlDeclarationNode declarationNode;

	private XmlComplexTextNode complexTextNode;

	private XmlWhitespaceTextNode whitespaceTextNode;

	private XmlCDataNode cdataNode;

	private XmlCommentNode commentNode;

	private XmlElementNode rootElementNode;

	private int attributeIndex;

	private char[] chars;

	private string prefix;

	private string localName;

	private string ns;

	private string value;

	private int trailCharCount;

	private int trailByteCount;

	private char[] trailChars;

	private byte[] trailBytes;

	private bool rootElement;

	private bool readingElement;

	private XmlSigningNodeWriter signingWriter;

	private bool signing;

	private AttributeSorter attributeSorter;

	private static XmlInitialNode initialNode = new XmlInitialNode(XmlBufferReader.Empty);

	private static XmlEndOfFileNode endOfFileNode = new XmlEndOfFileNode(XmlBufferReader.Empty);

	private static XmlClosedNode closedNode = new XmlClosedNode(XmlBufferReader.Empty);

	private static BinHexEncoding binhexEncoding;

	private static Base64Encoding base64Encoding;

	private const string xmlns = "xmlns";

	private const string xml = "xml";

	private const string xmlnsNamespace = "http://www.w3.org/2000/xmlns/";

	private const string xmlNamespace = "http://www.w3.org/XML/1998/namespace";

	private static BinHexEncoding BinHexEncoding
	{
		get
		{
			if (binhexEncoding == null)
			{
				binhexEncoding = new BinHexEncoding();
			}
			return binhexEncoding;
		}
	}

	private static Base64Encoding Base64Encoding
	{
		get
		{
			if (base64Encoding == null)
			{
				base64Encoding = new Base64Encoding();
			}
			return base64Encoding;
		}
	}

	protected XmlBufferReader BufferReader => bufferReader;

	public override XmlDictionaryReaderQuotas Quotas => quotas;

	protected XmlNode Node => node;

	protected XmlElementNode ElementNode
	{
		get
		{
			if (depth == 0)
			{
				return rootElementNode;
			}
			return elementNodes[depth];
		}
	}

	protected bool OutsideRootElement => depth == 0;

	public override bool CanReadBinaryContent => true;

	public override bool CanReadValueChunk => true;

	public override string BaseURI => string.Empty;

	public override bool HasValue => node.HasValue;

	public override bool IsDefault => false;

	public override string this[int index] => GetAttribute(index);

	public override string this[string name] => GetAttribute(name);

	public override string this[string localName, string namespaceUri] => GetAttribute(localName, namespaceUri);

	public override int AttributeCount
	{
		get
		{
			if (node.CanGetAttribute)
			{
				return attributeCount;
			}
			return 0;
		}
	}

	public sealed override int Depth => depth + node.DepthDelta;

	public override bool EOF => node.ReadState == ReadState.EndOfFile;

	public sealed override bool IsEmptyElement => node.IsEmptyElement;

	public override string LocalName
	{
		get
		{
			if (localName == null)
			{
				localName = GetLocalName(enforceAtomization: true);
			}
			return localName;
		}
	}

	public override string NamespaceURI
	{
		get
		{
			if (ns == null)
			{
				ns = GetNamespaceUri(enforceAtomization: true);
			}
			return ns;
		}
	}

	public override XmlNameTable NameTable
	{
		get
		{
			if (nameTable == null)
			{
				nameTable = new QuotaNameTable(this, quotas.MaxNameTableCharCount);
				nameTable.Add("xml");
				nameTable.Add("xmlns");
				nameTable.Add("http://www.w3.org/2000/xmlns/");
				nameTable.Add("http://www.w3.org/XML/1998/namespace");
				for (PrefixHandleType prefixHandleType = PrefixHandleType.A; prefixHandleType <= PrefixHandleType.Z; prefixHandleType++)
				{
					nameTable.Add(PrefixHandle.GetString(prefixHandleType));
				}
			}
			return nameTable;
		}
	}

	public sealed override XmlNodeType NodeType => node.NodeType;

	public override string Prefix
	{
		get
		{
			if (prefix == null)
			{
				switch (node.QNameType)
				{
				case QNameType.Normal:
					prefix = node.Prefix.GetString(NameTable);
					break;
				case QNameType.Xmlns:
					if (node.Namespace.Prefix.IsEmpty)
					{
						prefix = string.Empty;
					}
					else
					{
						prefix = "xmlns";
					}
					break;
				default:
					prefix = "xml";
					break;
				}
			}
			return prefix;
		}
	}

	public override char QuoteChar => node.QuoteChar;

	public override ReadState ReadState => node.ReadState;

	public override string Value
	{
		get
		{
			if (value == null)
			{
				value = node.ValueAsString;
			}
			return value;
		}
	}

	public override Type ValueType
	{
		get
		{
			if (value == null && node.QNameType == QNameType.Normal)
			{
				Type type = node.Value.ToType();
				if (node.IsAtomicValue)
				{
					return type;
				}
				if (type == typeof(byte[]))
				{
					return type;
				}
			}
			return typeof(string);
		}
	}

	public override string XmlLang => nsMgr.XmlLang;

	public override XmlSpace XmlSpace => nsMgr.XmlSpace;

	public override bool CanCanonicalize => true;

	protected bool Signing => signing;

	protected XmlBaseReader()
	{
		bufferReader = new XmlBufferReader(this);
		nsMgr = new NamespaceManager(bufferReader);
		quotas = new XmlDictionaryReaderQuotas();
		rootElementNode = new XmlElementNode(bufferReader);
		atomicTextNode = new XmlAtomicTextNode(bufferReader);
		node = closedNode;
	}

	protected void MoveToNode(XmlNode node)
	{
		this.node = node;
		ns = null;
		localName = null;
		prefix = null;
		value = null;
	}

	protected void MoveToInitial(XmlDictionaryReaderQuotas quotas)
	{
		if (quotas == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("quotas");
		}
		quotas.InternalCopyTo(this.quotas);
		this.quotas.MakeReadOnly();
		nsMgr.Clear();
		depth = 0;
		attributeCount = 0;
		attributeStart = -1;
		attributeIndex = -1;
		rootElement = false;
		readingElement = false;
		signing = false;
		MoveToNode(initialNode);
	}

	protected XmlDeclarationNode MoveToDeclaration()
	{
		if (attributeCount < 1)
		{
			XmlExceptionHelper.ThrowXmlException(this, new XmlException(SR.GetString("Version not found in XML declaration.")));
		}
		if (attributeCount > 3)
		{
			XmlExceptionHelper.ThrowXmlException(this, new XmlException(SR.GetString("Malformed XML declaration.")));
		}
		if (!CheckDeclAttribute(0, "version", "1.0", checkLower: false, "XML version must be '1.0'."))
		{
			XmlExceptionHelper.ThrowXmlException(this, new XmlException(SR.GetString("Version not found in XML declaration.")));
		}
		if (attributeCount > 1)
		{
			if (CheckDeclAttribute(1, "encoding", null, checkLower: true, "XML encoding must be 'UTF-8'."))
			{
				if (attributeCount == 3 && !CheckStandalone(2))
				{
					XmlExceptionHelper.ThrowXmlException(this, new XmlException(SR.GetString("Malformed XML declaration.")));
				}
			}
			else if (!CheckStandalone(1) || attributeCount > 2)
			{
				XmlExceptionHelper.ThrowXmlException(this, new XmlException(SR.GetString("Malformed XML declaration.")));
			}
		}
		if (declarationNode == null)
		{
			declarationNode = new XmlDeclarationNode(bufferReader);
		}
		MoveToNode(declarationNode);
		return declarationNode;
	}

	private bool CheckStandalone(int attr)
	{
		XmlAttributeNode xmlAttributeNode = attributeNodes[attr];
		if (!xmlAttributeNode.Prefix.IsEmpty)
		{
			XmlExceptionHelper.ThrowXmlException(this, new XmlException(SR.GetString("Malformed XML declaration.")));
		}
		if (xmlAttributeNode.LocalName != "standalone")
		{
			return false;
		}
		if (!xmlAttributeNode.Value.Equals2("yes", checkLower: false) && !xmlAttributeNode.Value.Equals2("no", checkLower: false))
		{
			XmlExceptionHelper.ThrowXmlException(this, new XmlException(SR.GetString("'standalone' value in declaration must be 'yes' or 'no'.")));
		}
		return true;
	}

	private bool CheckDeclAttribute(int index, string localName, string value, bool checkLower, string valueSR)
	{
		XmlAttributeNode xmlAttributeNode = attributeNodes[index];
		if (!xmlAttributeNode.Prefix.IsEmpty)
		{
			XmlExceptionHelper.ThrowXmlException(this, new XmlException(SR.GetString("Malformed XML declaration.")));
		}
		if (xmlAttributeNode.LocalName != localName)
		{
			return false;
		}
		if (value != null && !xmlAttributeNode.Value.Equals2(value, checkLower))
		{
			XmlExceptionHelper.ThrowXmlException(this, new XmlException(SR.GetString(valueSR)));
		}
		return true;
	}

	protected XmlCommentNode MoveToComment()
	{
		if (commentNode == null)
		{
			commentNode = new XmlCommentNode(bufferReader);
		}
		MoveToNode(commentNode);
		return commentNode;
	}

	protected XmlCDataNode MoveToCData()
	{
		if (cdataNode == null)
		{
			cdataNode = new XmlCDataNode(bufferReader);
		}
		MoveToNode(cdataNode);
		return cdataNode;
	}

	protected XmlAtomicTextNode MoveToAtomicText()
	{
		XmlAtomicTextNode result = atomicTextNode;
		MoveToNode(result);
		return result;
	}

	protected XmlComplexTextNode MoveToComplexText()
	{
		if (complexTextNode == null)
		{
			complexTextNode = new XmlComplexTextNode(bufferReader);
		}
		MoveToNode(complexTextNode);
		return complexTextNode;
	}

	protected XmlTextNode MoveToWhitespaceText()
	{
		if (whitespaceTextNode == null)
		{
			whitespaceTextNode = new XmlWhitespaceTextNode(bufferReader);
		}
		if (nsMgr.XmlSpace == XmlSpace.Preserve)
		{
			whitespaceTextNode.NodeType = XmlNodeType.SignificantWhitespace;
		}
		else
		{
			whitespaceTextNode.NodeType = XmlNodeType.Whitespace;
		}
		MoveToNode(whitespaceTextNode);
		return whitespaceTextNode;
	}

	protected void MoveToEndElement()
	{
		if (depth == 0)
		{
			XmlExceptionHelper.ThrowInvalidBinaryFormat(this);
		}
		XmlElementNode xmlElementNode = elementNodes[depth];
		XmlEndElementNode endElement = xmlElementNode.EndElement;
		endElement.Namespace = xmlElementNode.Namespace;
		MoveToNode(endElement);
	}

	protected void MoveToEndOfFile()
	{
		if (depth != 0)
		{
			XmlExceptionHelper.ThrowUnexpectedEndOfFile(this);
		}
		MoveToNode(endOfFileNode);
	}

	protected XmlElementNode EnterScope()
	{
		if (depth == 0)
		{
			if (rootElement)
			{
				XmlExceptionHelper.ThrowMultipleRootElements(this);
			}
			rootElement = true;
		}
		nsMgr.EnterScope();
		depth++;
		if (depth > quotas.MaxDepth)
		{
			XmlExceptionHelper.ThrowMaxDepthExceeded(this, quotas.MaxDepth);
		}
		if (elementNodes == null)
		{
			elementNodes = new XmlElementNode[4];
		}
		else if (elementNodes.Length == depth)
		{
			XmlElementNode[] destinationArray = new XmlElementNode[depth * 2];
			Array.Copy(elementNodes, destinationArray, depth);
			elementNodes = destinationArray;
		}
		XmlElementNode xmlElementNode = elementNodes[depth];
		if (xmlElementNode == null)
		{
			xmlElementNode = new XmlElementNode(bufferReader);
			elementNodes[depth] = xmlElementNode;
		}
		attributeCount = 0;
		attributeStart = -1;
		attributeIndex = -1;
		MoveToNode(xmlElementNode);
		return xmlElementNode;
	}

	protected void ExitScope()
	{
		if (depth == 0)
		{
			XmlExceptionHelper.ThrowUnexpectedEndElement(this);
		}
		depth--;
		nsMgr.ExitScope();
	}

	private XmlAttributeNode AddAttribute(QNameType qnameType, bool isAtomicValue)
	{
		int num = attributeCount;
		if (attributeNodes == null)
		{
			attributeNodes = new XmlAttributeNode[4];
		}
		else if (attributeNodes.Length == num)
		{
			XmlAttributeNode[] destinationArray = new XmlAttributeNode[num * 2];
			Array.Copy(attributeNodes, destinationArray, num);
			attributeNodes = destinationArray;
		}
		XmlAttributeNode xmlAttributeNode = attributeNodes[num];
		if (xmlAttributeNode == null)
		{
			xmlAttributeNode = new XmlAttributeNode(bufferReader);
			attributeNodes[num] = xmlAttributeNode;
		}
		xmlAttributeNode.QNameType = qnameType;
		xmlAttributeNode.IsAtomicValue = isAtomicValue;
		xmlAttributeNode.AttributeText.QNameType = qnameType;
		xmlAttributeNode.AttributeText.IsAtomicValue = isAtomicValue;
		attributeCount++;
		return xmlAttributeNode;
	}

	protected Namespace AddNamespace()
	{
		return nsMgr.AddNamespace();
	}

	protected XmlAttributeNode AddAttribute()
	{
		return AddAttribute(QNameType.Normal, isAtomicValue: true);
	}

	protected XmlAttributeNode AddXmlAttribute()
	{
		return AddAttribute(QNameType.Normal, isAtomicValue: true);
	}

	protected XmlAttributeNode AddXmlnsAttribute(Namespace ns)
	{
		if (!ns.Prefix.IsEmpty && ns.Uri.IsEmpty)
		{
			XmlExceptionHelper.ThrowEmptyNamespace(this);
		}
		if (ns.Prefix.IsXml && ns.Uri != "http://www.w3.org/XML/1998/namespace")
		{
			XmlExceptionHelper.ThrowXmlException(this, new XmlException(SR.GetString("The prefix '{0}' can only be bound to the namespace '{1}'.", "xml", "http://www.w3.org/XML/1998/namespace")));
		}
		else if (ns.Prefix.IsXmlns && ns.Uri != "http://www.w3.org/2000/xmlns/")
		{
			XmlExceptionHelper.ThrowXmlException(this, new XmlException(SR.GetString("The prefix '{0}' can only be bound to the namespace '{1}'.", "xmlns", "http://www.w3.org/2000/xmlns/")));
		}
		nsMgr.Register(ns);
		XmlAttributeNode xmlAttributeNode = AddAttribute(QNameType.Xmlns, isAtomicValue: false);
		xmlAttributeNode.Namespace = ns;
		xmlAttributeNode.AttributeText.Namespace = ns;
		return xmlAttributeNode;
	}

	protected void FixXmlAttribute(XmlAttributeNode attributeNode)
	{
		if (!(attributeNode.Prefix == "xml"))
		{
			return;
		}
		if (attributeNode.LocalName == "lang")
		{
			nsMgr.AddLangAttribute(attributeNode.Value.GetString());
		}
		else if (attributeNode.LocalName == "space")
		{
			string text = attributeNode.Value.GetString();
			if (text == "preserve")
			{
				nsMgr.AddSpaceAttribute(XmlSpace.Preserve);
			}
			else if (text == "default")
			{
				nsMgr.AddSpaceAttribute(XmlSpace.Default);
			}
		}
	}

	public override void Close()
	{
		MoveToNode(closedNode);
		nameTable = null;
		if (attributeNodes != null && attributeNodes.Length > 16)
		{
			attributeNodes = null;
		}
		if (elementNodes != null && elementNodes.Length > 16)
		{
			elementNodes = null;
		}
		nsMgr.Close();
		bufferReader.Close();
		if (signingWriter != null)
		{
			signingWriter.Close();
		}
		if (attributeSorter != null)
		{
			attributeSorter.Close();
		}
	}

	private XmlAttributeNode GetAttributeNode(int index)
	{
		if (!node.CanGetAttribute)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("index", SR.GetString("Only Element nodes have attributes.")));
		}
		if (index < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("index", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (index >= attributeCount)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("index", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", attributeCount)));
		}
		return attributeNodes[index];
	}

	private XmlAttributeNode GetAttributeNode(string name)
	{
		if (name == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("name"));
		}
		if (!node.CanGetAttribute)
		{
			return null;
		}
		int num = name.IndexOf(':');
		string text;
		string text2;
		if (num == -1)
		{
			if (name == "xmlns")
			{
				text = "xmlns";
				text2 = string.Empty;
			}
			else
			{
				text = string.Empty;
				text2 = name;
			}
		}
		else
		{
			text = name.Substring(0, num);
			text2 = name.Substring(num + 1);
		}
		XmlAttributeNode[] array = attributeNodes;
		int num2 = attributeCount;
		int num3 = attributeStart;
		for (int i = 0; i < num2; i++)
		{
			if (++num3 >= num2)
			{
				num3 = 0;
			}
			XmlAttributeNode xmlAttributeNode = array[num3];
			if (xmlAttributeNode.IsPrefixAndLocalName(text, text2))
			{
				attributeStart = num3;
				return xmlAttributeNode;
			}
		}
		return null;
	}

	private XmlAttributeNode GetAttributeNode(string localName, string namespaceUri)
	{
		if (localName == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("localName"));
		}
		if (namespaceUri == null)
		{
			namespaceUri = string.Empty;
		}
		if (!node.CanGetAttribute)
		{
			return null;
		}
		XmlAttributeNode[] array = attributeNodes;
		int num = attributeCount;
		int num2 = attributeStart;
		for (int i = 0; i < num; i++)
		{
			if (++num2 >= num)
			{
				num2 = 0;
			}
			XmlAttributeNode xmlAttributeNode = array[num2];
			if (xmlAttributeNode.IsLocalNameAndNamespaceUri(localName, namespaceUri))
			{
				attributeStart = num2;
				return xmlAttributeNode;
			}
		}
		return null;
	}

	private XmlAttributeNode GetAttributeNode(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		if (localName == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("localName"));
		}
		if (namespaceUri == null)
		{
			namespaceUri = XmlDictionaryString.Empty;
		}
		if (!node.CanGetAttribute)
		{
			return null;
		}
		XmlAttributeNode[] array = attributeNodes;
		int num = attributeCount;
		int num2 = attributeStart;
		for (int i = 0; i < num; i++)
		{
			if (++num2 >= num)
			{
				num2 = 0;
			}
			XmlAttributeNode xmlAttributeNode = array[num2];
			if (xmlAttributeNode.IsLocalNameAndNamespaceUri(localName, namespaceUri))
			{
				attributeStart = num2;
				return xmlAttributeNode;
			}
		}
		return null;
	}

	public override string GetAttribute(int index)
	{
		return GetAttributeNode(index).ValueAsString;
	}

	public override string GetAttribute(string name)
	{
		return GetAttributeNode(name)?.ValueAsString;
	}

	public override string GetAttribute(string localName, string namespaceUri)
	{
		return GetAttributeNode(localName, namespaceUri)?.ValueAsString;
	}

	public override string GetAttribute(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		return GetAttributeNode(localName, namespaceUri)?.ValueAsString;
	}

	public override string LookupNamespace(string prefix)
	{
		Namespace obj = nsMgr.LookupNamespace(prefix);
		if (obj != null)
		{
			return obj.Uri.GetString(NameTable);
		}
		if (prefix == "xmlns")
		{
			return "http://www.w3.org/2000/xmlns/";
		}
		return null;
	}

	protected Namespace LookupNamespace(PrefixHandleType prefix)
	{
		Namespace obj = nsMgr.LookupNamespace(prefix);
		if (obj == null)
		{
			XmlExceptionHelper.ThrowUndefinedPrefix(this, PrefixHandle.GetString(prefix));
		}
		return obj;
	}

	protected Namespace LookupNamespace(PrefixHandle prefix)
	{
		Namespace obj = nsMgr.LookupNamespace(prefix);
		if (obj == null)
		{
			XmlExceptionHelper.ThrowUndefinedPrefix(this, prefix.GetString());
		}
		return obj;
	}

	protected void ProcessAttributes()
	{
		if (attributeCount > 0)
		{
			ProcessAttributes(attributeNodes, attributeCount);
		}
	}

	private void ProcessAttributes(XmlAttributeNode[] attributeNodes, int attributeCount)
	{
		for (int i = 0; i < attributeCount; i++)
		{
			XmlAttributeNode xmlAttributeNode = attributeNodes[i];
			if (xmlAttributeNode.QNameType == QNameType.Normal)
			{
				PrefixHandle prefixHandle = xmlAttributeNode.Prefix;
				if (!prefixHandle.IsEmpty)
				{
					xmlAttributeNode.Namespace = LookupNamespace(prefixHandle);
				}
				else
				{
					xmlAttributeNode.Namespace = NamespaceManager.EmptyNamespace;
				}
				xmlAttributeNode.AttributeText.Namespace = xmlAttributeNode.Namespace;
			}
		}
		if (attributeCount <= 1)
		{
			return;
		}
		if (attributeCount < 12)
		{
			for (int j = 0; j < attributeCount - 1; j++)
			{
				XmlAttributeNode xmlAttributeNode2 = attributeNodes[j];
				if (xmlAttributeNode2.QNameType == QNameType.Normal)
				{
					for (int k = j + 1; k < attributeCount; k++)
					{
						XmlAttributeNode xmlAttributeNode3 = attributeNodes[k];
						if (xmlAttributeNode3.QNameType == QNameType.Normal && xmlAttributeNode2.LocalName == xmlAttributeNode3.LocalName && xmlAttributeNode2.Namespace.Uri == xmlAttributeNode3.Namespace.Uri)
						{
							XmlExceptionHelper.ThrowDuplicateAttribute(this, xmlAttributeNode2.Prefix.GetString(), xmlAttributeNode3.Prefix.GetString(), xmlAttributeNode2.LocalName.GetString(), xmlAttributeNode2.Namespace.Uri.GetString());
						}
					}
					continue;
				}
				for (int l = j + 1; l < attributeCount; l++)
				{
					XmlAttributeNode xmlAttributeNode4 = attributeNodes[l];
					if (xmlAttributeNode4.QNameType == QNameType.Xmlns && xmlAttributeNode2.Namespace.Prefix == xmlAttributeNode4.Namespace.Prefix)
					{
						XmlExceptionHelper.ThrowDuplicateAttribute(this, "xmlns", "xmlns", xmlAttributeNode2.Namespace.Prefix.GetString(), "http://www.w3.org/2000/xmlns/");
					}
				}
			}
		}
		else
		{
			CheckAttributes(attributeNodes, attributeCount);
		}
	}

	private void CheckAttributes(XmlAttributeNode[] attributeNodes, int attributeCount)
	{
		if (attributeSorter == null)
		{
			attributeSorter = new AttributeSorter();
		}
		if (!attributeSorter.Sort(attributeNodes, attributeCount))
		{
			attributeSorter.GetIndeces(out var attributeIndex, out var attributeIndex2);
			if (attributeNodes[attributeIndex].QNameType == QNameType.Xmlns)
			{
				XmlExceptionHelper.ThrowDuplicateXmlnsAttribute(this, attributeNodes[attributeIndex].Namespace.Prefix.GetString(), "http://www.w3.org/2000/xmlns/");
			}
			else
			{
				XmlExceptionHelper.ThrowDuplicateAttribute(this, attributeNodes[attributeIndex].Prefix.GetString(), attributeNodes[attributeIndex2].Prefix.GetString(), attributeNodes[attributeIndex].LocalName.GetString(), attributeNodes[attributeIndex].Namespace.Uri.GetString());
			}
		}
	}

	public override void MoveToAttribute(int index)
	{
		MoveToNode(GetAttributeNode(index));
		attributeIndex = index;
	}

	public override bool MoveToAttribute(string name)
	{
		XmlNode attributeNode = GetAttributeNode(name);
		if (attributeNode == null)
		{
			return false;
		}
		MoveToNode(attributeNode);
		attributeIndex = attributeStart;
		return true;
	}

	public override bool MoveToAttribute(string localName, string namespaceUri)
	{
		XmlNode attributeNode = GetAttributeNode(localName, namespaceUri);
		if (attributeNode == null)
		{
			return false;
		}
		MoveToNode(attributeNode);
		attributeIndex = attributeStart;
		return true;
	}

	public override bool MoveToElement()
	{
		if (!node.CanMoveToElement)
		{
			return false;
		}
		if (depth == 0)
		{
			MoveToDeclaration();
		}
		else
		{
			MoveToNode(elementNodes[depth]);
		}
		attributeIndex = -1;
		return true;
	}

	public override XmlNodeType MoveToContent()
	{
		do
		{
			if (node.HasContent)
			{
				if ((node.NodeType != XmlNodeType.Text && node.NodeType != XmlNodeType.CDATA) || trailByteCount > 0)
				{
					break;
				}
				if (value == null)
				{
					if (!node.Value.IsWhitespace())
					{
						break;
					}
				}
				else if (!XmlConverter.IsWhitespace(value))
				{
					break;
				}
			}
			else if (node.NodeType == XmlNodeType.Attribute)
			{
				MoveToElement();
				break;
			}
		}
		while (Read());
		return node.NodeType;
	}

	public override bool MoveToFirstAttribute()
	{
		if (!node.CanGetAttribute || attributeCount == 0)
		{
			return false;
		}
		MoveToNode(GetAttributeNode(0));
		attributeIndex = 0;
		return true;
	}

	public override bool MoveToNextAttribute()
	{
		if (!node.CanGetAttribute)
		{
			return false;
		}
		int num = attributeIndex + 1;
		if (num >= attributeCount)
		{
			return false;
		}
		MoveToNode(GetAttributeNode(num));
		attributeIndex = num;
		return true;
	}

	private string GetLocalName(bool enforceAtomization)
	{
		if (localName != null)
		{
			return localName;
		}
		if (node.QNameType == QNameType.Normal)
		{
			if (enforceAtomization || nameTable != null)
			{
				return node.LocalName.GetString(NameTable);
			}
			return node.LocalName.GetString();
		}
		if (node.Namespace.Prefix.IsEmpty)
		{
			return "xmlns";
		}
		if (enforceAtomization || nameTable != null)
		{
			return node.Namespace.Prefix.GetString(NameTable);
		}
		return node.Namespace.Prefix.GetString();
	}

	private string GetNamespaceUri(bool enforceAtomization)
	{
		if (ns != null)
		{
			return ns;
		}
		if (node.QNameType == QNameType.Normal)
		{
			if (enforceAtomization || nameTable != null)
			{
				return node.Namespace.Uri.GetString(NameTable);
			}
			return node.Namespace.Uri.GetString();
		}
		return "http://www.w3.org/2000/xmlns/";
	}

	public override void GetNonAtomizedNames(out string localName, out string namespaceUri)
	{
		localName = GetLocalName(enforceAtomization: false);
		namespaceUri = GetNamespaceUri(enforceAtomization: false);
	}

	public override bool IsLocalName(string localName)
	{
		if (localName == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("localName"));
		}
		return node.IsLocalName(localName);
	}

	public override bool IsLocalName(XmlDictionaryString localName)
	{
		if (localName == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("localName"));
		}
		return node.IsLocalName(localName);
	}

	public override bool IsNamespaceUri(string namespaceUri)
	{
		if (namespaceUri == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
		}
		return node.IsNamespaceUri(namespaceUri);
	}

	public override bool IsNamespaceUri(XmlDictionaryString namespaceUri)
	{
		if (namespaceUri == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
		}
		return node.IsNamespaceUri(namespaceUri);
	}

	public sealed override bool IsStartElement()
	{
		switch (node.NodeType)
		{
		case XmlNodeType.Element:
			return true;
		case XmlNodeType.EndElement:
			return false;
		case XmlNodeType.None:
			Read();
			if (node.NodeType == XmlNodeType.Element)
			{
				return true;
			}
			break;
		}
		return MoveToContent() == XmlNodeType.Element;
	}

	public override bool IsStartElement(string name)
	{
		if (name == null)
		{
			return false;
		}
		int num = name.IndexOf(':');
		string text;
		string text2;
		if (num == -1)
		{
			text = string.Empty;
			text2 = name;
		}
		else
		{
			text = name.Substring(0, num);
			text2 = name.Substring(num + 1);
		}
		if ((node.NodeType == XmlNodeType.Element || IsStartElement()) && node.Prefix == text)
		{
			return node.LocalName == text2;
		}
		return false;
	}

	public override bool IsStartElement(string localName, string namespaceUri)
	{
		if (localName == null)
		{
			return false;
		}
		if (namespaceUri == null)
		{
			return false;
		}
		if ((node.NodeType == XmlNodeType.Element || IsStartElement()) && node.LocalName == localName)
		{
			return node.IsNamespaceUri(namespaceUri);
		}
		return false;
	}

	public override bool IsStartElement(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		if (localName == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localName");
		}
		if (namespaceUri == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
		}
		if ((node.NodeType == XmlNodeType.Element || IsStartElement()) && node.LocalName == localName)
		{
			return node.IsNamespaceUri(namespaceUri);
		}
		return false;
	}

	public override int IndexOfLocalName(string[] localNames, string namespaceUri)
	{
		if (localNames == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localNames");
		}
		if (namespaceUri == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
		}
		QNameType qNameType = node.QNameType;
		if (node.IsNamespaceUri(namespaceUri))
		{
			if (qNameType == QNameType.Normal)
			{
				StringHandle stringHandle = node.LocalName;
				for (int i = 0; i < localNames.Length; i++)
				{
					string text = localNames[i];
					if (text == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(string.Format(CultureInfo.InvariantCulture, "localNames[{0}]", i));
					}
					if (stringHandle == text)
					{
						return i;
					}
				}
			}
			else
			{
				PrefixHandle prefixHandle = node.Namespace.Prefix;
				for (int j = 0; j < localNames.Length; j++)
				{
					string text2 = localNames[j];
					if (text2 == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(string.Format(CultureInfo.InvariantCulture, "localNames[{0}]", j));
					}
					if (prefixHandle == text2)
					{
						return j;
					}
				}
			}
		}
		return -1;
	}

	public override int IndexOfLocalName(XmlDictionaryString[] localNames, XmlDictionaryString namespaceUri)
	{
		if (localNames == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localNames");
		}
		if (namespaceUri == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
		}
		QNameType qNameType = node.QNameType;
		if (node.IsNamespaceUri(namespaceUri))
		{
			if (qNameType == QNameType.Normal)
			{
				StringHandle stringHandle = node.LocalName;
				for (int i = 0; i < localNames.Length; i++)
				{
					XmlDictionaryString xmlDictionaryString = localNames[i];
					if (xmlDictionaryString == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(string.Format(CultureInfo.InvariantCulture, "localNames[{0}]", i));
					}
					if (stringHandle == xmlDictionaryString)
					{
						return i;
					}
				}
			}
			else
			{
				PrefixHandle prefixHandle = node.Namespace.Prefix;
				for (int j = 0; j < localNames.Length; j++)
				{
					XmlDictionaryString xmlDictionaryString2 = localNames[j];
					if (xmlDictionaryString2 == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(string.Format(CultureInfo.InvariantCulture, "localNames[{0}]", j));
					}
					if (prefixHandle == xmlDictionaryString2)
					{
						return j;
					}
				}
			}
		}
		return -1;
	}

	public override int ReadValueChunk(char[] chars, int offset, int count)
	{
		if (chars == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("chars"));
		}
		if (offset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (offset > chars.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", chars.Length)));
		}
		if (count < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count > chars.Length - offset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", chars.Length - offset)));
		}
		if (value == null && node.QNameType == QNameType.Normal && node.Value.TryReadChars(chars, offset, count, out var actual))
		{
			return actual;
		}
		string text = Value;
		actual = Math.Min(count, text.Length);
		text.CopyTo(0, chars, offset, actual);
		value = text.Substring(actual);
		return actual;
	}

	public override int ReadValueAsBase64(byte[] buffer, int offset, int count)
	{
		if (buffer == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("buffer"));
		}
		if (offset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (offset > buffer.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", buffer.Length)));
		}
		if (count < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count > buffer.Length - offset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", buffer.Length - offset)));
		}
		if (count == 0)
		{
			return 0;
		}
		if (value == null && trailByteCount == 0 && trailCharCount == 0 && node.QNameType == QNameType.Normal && node.Value.TryReadBase64(buffer, offset, count, out var actual))
		{
			return actual;
		}
		return ReadBytes(Base64Encoding, 3, 4, buffer, offset, Math.Min(count, 512), readContent: false);
	}

	public override string ReadElementContentAsString()
	{
		if (node.NodeType != XmlNodeType.Element)
		{
			MoveToStartElement();
		}
		if (node.IsEmptyElement)
		{
			Read();
			return string.Empty;
		}
		Read();
		string result = ReadContentAsString();
		ReadEndElement();
		return result;
	}

	public override string ReadElementString()
	{
		MoveToStartElement();
		if (IsEmptyElement)
		{
			Read();
			return string.Empty;
		}
		Read();
		string result = ReadString();
		ReadEndElement();
		return result;
	}

	public override string ReadElementString(string name)
	{
		MoveToStartElement(name);
		return ReadElementString();
	}

	public override string ReadElementString(string localName, string namespaceUri)
	{
		MoveToStartElement(localName, namespaceUri);
		return ReadElementString();
	}

	public override void ReadStartElement()
	{
		if (node.NodeType != XmlNodeType.Element)
		{
			MoveToStartElement();
		}
		Read();
	}

	public override void ReadStartElement(string name)
	{
		MoveToStartElement(name);
		Read();
	}

	public override void ReadStartElement(string localName, string namespaceUri)
	{
		MoveToStartElement(localName, namespaceUri);
		Read();
	}

	public override void ReadEndElement()
	{
		if (node.NodeType != XmlNodeType.EndElement && MoveToContent() != XmlNodeType.EndElement)
		{
			int num = ((node.NodeType == XmlNodeType.Element) ? (depth - 1) : depth);
			if (num == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("No corresponding start element is open.")));
			}
			XmlElementNode xmlElementNode = elementNodes[num];
			XmlExceptionHelper.ThrowEndElementExpected(this, xmlElementNode.LocalName.GetString(), xmlElementNode.Namespace.Uri.GetString());
		}
		Read();
	}

	public override bool ReadAttributeValue()
	{
		XmlAttributeTextNode attributeText = node.AttributeText;
		if (attributeText == null)
		{
			return false;
		}
		MoveToNode(attributeText);
		return true;
	}

	private void SkipValue(XmlNode node)
	{
		if (node.SkipValue)
		{
			Read();
		}
	}

	public override bool TryGetBase64ContentLength(out int length)
	{
		if (trailByteCount == 0 && trailCharCount == 0 && value == null)
		{
			XmlNode xmlNode = Node;
			if (xmlNode.IsAtomicValue)
			{
				return xmlNode.Value.TryGetByteArrayLength(out length);
			}
		}
		return base.TryGetBase64ContentLength(out length);
	}

	public override byte[] ReadContentAsBase64()
	{
		if (trailByteCount == 0 && trailCharCount == 0 && value == null)
		{
			XmlNode xmlNode = Node;
			if (xmlNode.IsAtomicValue)
			{
				byte[] array = xmlNode.Value.ToByteArray();
				if (array.Length > quotas.MaxArrayLength)
				{
					XmlExceptionHelper.ThrowMaxArrayLengthExceeded(this, quotas.MaxArrayLength);
				}
				SkipValue(xmlNode);
				return array;
			}
		}
		if (!bufferReader.IsStreamed)
		{
			return ReadContentAsBase64(quotas.MaxArrayLength, bufferReader.Buffer.Length);
		}
		return ReadContentAsBase64(quotas.MaxArrayLength, 65535);
	}

	public override int ReadElementContentAsBase64(byte[] buffer, int offset, int count)
	{
		if (!readingElement)
		{
			if (IsEmptyElement)
			{
				Read();
				return 0;
			}
			ReadStartElement();
			readingElement = true;
		}
		int num = ReadContentAsBase64(buffer, offset, count);
		if (num == 0)
		{
			ReadEndElement();
			readingElement = false;
		}
		return num;
	}

	public override int ReadContentAsBase64(byte[] buffer, int offset, int count)
	{
		if (buffer == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("buffer"));
		}
		if (offset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (offset > buffer.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", buffer.Length)));
		}
		if (count < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count > buffer.Length - offset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", buffer.Length - offset)));
		}
		if (count == 0)
		{
			return 0;
		}
		if (trailByteCount == 0 && trailCharCount == 0 && value == null && node.QNameType == QNameType.Normal)
		{
			int actual;
			while (node.NodeType != XmlNodeType.Comment && node.Value.TryReadBase64(buffer, offset, count, out actual))
			{
				if (actual != 0)
				{
					return actual;
				}
				Read();
			}
		}
		XmlNodeType nodeType = node.NodeType;
		if (nodeType == XmlNodeType.Element || nodeType == XmlNodeType.EndElement)
		{
			return 0;
		}
		return ReadBytes(Base64Encoding, 3, 4, buffer, offset, Math.Min(count, 512), readContent: true);
	}

	public override byte[] ReadContentAsBinHex()
	{
		return ReadContentAsBinHex(quotas.MaxArrayLength);
	}

	public override int ReadElementContentAsBinHex(byte[] buffer, int offset, int count)
	{
		if (!readingElement)
		{
			if (IsEmptyElement)
			{
				Read();
				return 0;
			}
			ReadStartElement();
			readingElement = true;
		}
		int num = ReadContentAsBinHex(buffer, offset, count);
		if (num == 0)
		{
			ReadEndElement();
			readingElement = false;
		}
		return num;
	}

	public override int ReadContentAsBinHex(byte[] buffer, int offset, int count)
	{
		if (buffer == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("buffer"));
		}
		if (offset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (offset > buffer.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", buffer.Length)));
		}
		if (count < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count > buffer.Length - offset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", buffer.Length - offset)));
		}
		if (count == 0)
		{
			return 0;
		}
		return ReadBytes(BinHexEncoding, 1, 2, buffer, offset, Math.Min(count, 512), readContent: true);
	}

	private int ReadBytes(Encoding encoding, int byteBlock, int charBlock, byte[] buffer, int offset, int byteCount, bool readContent)
	{
		if (trailByteCount > 0)
		{
			int num = Math.Min(trailByteCount, byteCount);
			Array.Copy(trailBytes, 0, buffer, offset, num);
			trailByteCount -= num;
			Array.Copy(trailBytes, num, trailBytes, 0, trailByteCount);
			return num;
		}
		XmlNodeType nodeType = node.NodeType;
		if (nodeType == XmlNodeType.Element || nodeType == XmlNodeType.EndElement)
		{
			return 0;
		}
		int num2 = ((byteCount >= byteBlock) ? (byteCount / byteBlock * charBlock) : charBlock);
		char[] charBuffer = GetCharBuffer(num2);
		int num3 = 0;
		while (true)
		{
			if (trailCharCount > 0)
			{
				Array.Copy(trailChars, 0, charBuffer, num3, trailCharCount);
				num3 += trailCharCount;
				trailCharCount = 0;
			}
			while (num3 < charBlock)
			{
				int num4;
				if (readContent)
				{
					num4 = ReadContentAsChars(charBuffer, num3, num2 - num3);
					if (num4 == 1 && charBuffer[num3] == '\n')
					{
						continue;
					}
				}
				else
				{
					num4 = ReadValueChunk(charBuffer, num3, num2 - num3);
				}
				if (num4 == 0)
				{
					break;
				}
				num3 += num4;
			}
			if (num3 >= charBlock)
			{
				trailCharCount = num3 % charBlock;
				if (trailCharCount > 0)
				{
					if (trailChars == null)
					{
						trailChars = new char[4];
					}
					num3 -= trailCharCount;
					Array.Copy(charBuffer, num3, trailChars, 0, trailCharCount);
				}
			}
			try
			{
				if (byteCount < byteBlock)
				{
					if (trailBytes == null)
					{
						trailBytes = new byte[3];
					}
					trailByteCount = encoding.GetBytes(charBuffer, 0, num3, trailBytes, 0);
					int num5 = Math.Min(trailByteCount, byteCount);
					Array.Copy(trailBytes, 0, buffer, offset, num5);
					trailByteCount -= num5;
					Array.Copy(trailBytes, num5, trailBytes, 0, trailByteCount);
					return num5;
				}
				return encoding.GetBytes(charBuffer, 0, num3, buffer, offset);
			}
			catch (FormatException ex)
			{
				int num6 = 0;
				int num7 = 0;
				while (true)
				{
					if (num7 < num3 && XmlConverter.IsWhitespace(charBuffer[num7]))
					{
						num7++;
						continue;
					}
					if (num7 == num3)
					{
						break;
					}
					charBuffer[num6++] = charBuffer[num7++];
				}
				if (num6 == num3)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(ex.Message, ex.InnerException));
				}
				num3 = num6;
			}
		}
	}

	public override string ReadContentAsString()
	{
		XmlNode xmlNode = Node;
		if (xmlNode.IsAtomicValue)
		{
			string text;
			if (value != null)
			{
				text = value;
				if (xmlNode.AttributeText == null)
				{
					value = string.Empty;
				}
			}
			else
			{
				text = xmlNode.Value.GetString();
				SkipValue(xmlNode);
				if (text.Length > quotas.MaxStringContentLength)
				{
					XmlExceptionHelper.ThrowMaxStringContentLengthExceeded(this, quotas.MaxStringContentLength);
				}
			}
			return text;
		}
		return ReadContentAsString(quotas.MaxStringContentLength);
	}

	public override bool ReadContentAsBoolean()
	{
		XmlNode xmlNode = Node;
		if (value == null && xmlNode.IsAtomicValue)
		{
			bool result = xmlNode.Value.ToBoolean();
			SkipValue(xmlNode);
			return result;
		}
		return XmlConverter.ToBoolean(ReadContentAsString());
	}

	public override long ReadContentAsLong()
	{
		XmlNode xmlNode = Node;
		if (value == null && xmlNode.IsAtomicValue)
		{
			long result = xmlNode.Value.ToLong();
			SkipValue(xmlNode);
			return result;
		}
		return XmlConverter.ToInt64(ReadContentAsString());
	}

	public override int ReadContentAsInt()
	{
		XmlNode xmlNode = Node;
		if (value == null && xmlNode.IsAtomicValue)
		{
			int result = xmlNode.Value.ToInt();
			SkipValue(xmlNode);
			return result;
		}
		return XmlConverter.ToInt32(ReadContentAsString());
	}

	public override DateTime ReadContentAsDateTime()
	{
		XmlNode xmlNode = Node;
		if (value == null && xmlNode.IsAtomicValue)
		{
			DateTime result = xmlNode.Value.ToDateTime();
			SkipValue(xmlNode);
			return result;
		}
		return XmlConverter.ToDateTime(ReadContentAsString());
	}

	public override double ReadContentAsDouble()
	{
		XmlNode xmlNode = Node;
		if (value == null && xmlNode.IsAtomicValue)
		{
			double result = xmlNode.Value.ToDouble();
			SkipValue(xmlNode);
			return result;
		}
		return XmlConverter.ToDouble(ReadContentAsString());
	}

	public override float ReadContentAsFloat()
	{
		XmlNode xmlNode = Node;
		if (value == null && xmlNode.IsAtomicValue)
		{
			float result = xmlNode.Value.ToSingle();
			SkipValue(xmlNode);
			return result;
		}
		return XmlConverter.ToSingle(ReadContentAsString());
	}

	public override decimal ReadContentAsDecimal()
	{
		XmlNode xmlNode = Node;
		if (value == null && xmlNode.IsAtomicValue)
		{
			decimal result = xmlNode.Value.ToDecimal();
			SkipValue(xmlNode);
			return result;
		}
		return XmlConverter.ToDecimal(ReadContentAsString());
	}

	public override UniqueId ReadContentAsUniqueId()
	{
		XmlNode xmlNode = Node;
		if (value == null && xmlNode.IsAtomicValue)
		{
			UniqueId result = xmlNode.Value.ToUniqueId();
			SkipValue(xmlNode);
			return result;
		}
		return XmlConverter.ToUniqueId(ReadContentAsString());
	}

	public override TimeSpan ReadContentAsTimeSpan()
	{
		XmlNode xmlNode = Node;
		if (value == null && xmlNode.IsAtomicValue)
		{
			TimeSpan result = xmlNode.Value.ToTimeSpan();
			SkipValue(xmlNode);
			return result;
		}
		return XmlConverter.ToTimeSpan(ReadContentAsString());
	}

	public override Guid ReadContentAsGuid()
	{
		XmlNode xmlNode = Node;
		if (value == null && xmlNode.IsAtomicValue)
		{
			Guid result = xmlNode.Value.ToGuid();
			SkipValue(xmlNode);
			return result;
		}
		return XmlConverter.ToGuid(ReadContentAsString());
	}

	public override object ReadContentAsObject()
	{
		XmlNode xmlNode = Node;
		if (value == null && xmlNode.IsAtomicValue)
		{
			object result = xmlNode.Value.ToObject();
			SkipValue(xmlNode);
			return result;
		}
		return ReadContentAsString();
	}

	public override object ReadContentAs(Type type, IXmlNamespaceResolver namespaceResolver)
	{
		if (type == typeof(ulong))
		{
			if (value == null && node.IsAtomicValue)
			{
				ulong num = node.Value.ToULong();
				SkipValue(node);
				return num;
			}
			return XmlConverter.ToUInt64(ReadContentAsString());
		}
		if (type == typeof(bool))
		{
			return ReadContentAsBoolean();
		}
		if (type == typeof(int))
		{
			return ReadContentAsInt();
		}
		if (type == typeof(long))
		{
			return ReadContentAsLong();
		}
		if (type == typeof(float))
		{
			return ReadContentAsFloat();
		}
		if (type == typeof(double))
		{
			return ReadContentAsDouble();
		}
		if (type == typeof(decimal))
		{
			return ReadContentAsDecimal();
		}
		if (type == typeof(DateTime))
		{
			return ReadContentAsDateTime();
		}
		if (type == typeof(UniqueId))
		{
			return ReadContentAsUniqueId();
		}
		if (type == typeof(Guid))
		{
			return ReadContentAsGuid();
		}
		if (type == typeof(TimeSpan))
		{
			return ReadContentAsTimeSpan();
		}
		if (type == typeof(object))
		{
			return ReadContentAsObject();
		}
		return base.ReadContentAs(type, namespaceResolver);
	}

	public override void ResolveEntity()
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("The reader cannot be advanced.")));
	}

	public override void Skip()
	{
		if (node.ReadState != ReadState.Interactive)
		{
			return;
		}
		if ((node.NodeType == XmlNodeType.Element || MoveToElement()) && !IsEmptyElement)
		{
			int num = Depth;
			while (Read() && num < Depth)
			{
			}
			if (node.NodeType == XmlNodeType.EndElement)
			{
				Read();
			}
		}
		else
		{
			Read();
		}
	}

	public override bool TryGetLocalNameAsDictionaryString(out XmlDictionaryString localName)
	{
		return node.TryGetLocalNameAsDictionaryString(out localName);
	}

	public override bool TryGetNamespaceUriAsDictionaryString(out XmlDictionaryString localName)
	{
		return node.TryGetNamespaceUriAsDictionaryString(out localName);
	}

	public override bool TryGetValueAsDictionaryString(out XmlDictionaryString value)
	{
		return node.TryGetValueAsDictionaryString(out value);
	}

	public override short[] ReadInt16Array(string localName, string namespaceUri)
	{
		return Int16ArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, quotas.MaxArrayLength);
	}

	public override short[] ReadInt16Array(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		return Int16ArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, quotas.MaxArrayLength);
	}

	public override int[] ReadInt32Array(string localName, string namespaceUri)
	{
		return Int32ArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, quotas.MaxArrayLength);
	}

	public override int[] ReadInt32Array(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		return Int32ArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, quotas.MaxArrayLength);
	}

	public override long[] ReadInt64Array(string localName, string namespaceUri)
	{
		return Int64ArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, quotas.MaxArrayLength);
	}

	public override long[] ReadInt64Array(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		return Int64ArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, quotas.MaxArrayLength);
	}

	public override float[] ReadSingleArray(string localName, string namespaceUri)
	{
		return SingleArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, quotas.MaxArrayLength);
	}

	public override float[] ReadSingleArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		return SingleArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, quotas.MaxArrayLength);
	}

	public override double[] ReadDoubleArray(string localName, string namespaceUri)
	{
		return DoubleArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, quotas.MaxArrayLength);
	}

	public override double[] ReadDoubleArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		return DoubleArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, quotas.MaxArrayLength);
	}

	public override decimal[] ReadDecimalArray(string localName, string namespaceUri)
	{
		return DecimalArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, quotas.MaxArrayLength);
	}

	public override decimal[] ReadDecimalArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		return DecimalArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, quotas.MaxArrayLength);
	}

	public override DateTime[] ReadDateTimeArray(string localName, string namespaceUri)
	{
		return DateTimeArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, quotas.MaxArrayLength);
	}

	public override DateTime[] ReadDateTimeArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		return DateTimeArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, quotas.MaxArrayLength);
	}

	public override Guid[] ReadGuidArray(string localName, string namespaceUri)
	{
		return GuidArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, quotas.MaxArrayLength);
	}

	public override Guid[] ReadGuidArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		return GuidArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, quotas.MaxArrayLength);
	}

	public override TimeSpan[] ReadTimeSpanArray(string localName, string namespaceUri)
	{
		return TimeSpanArrayHelperWithString.Instance.ReadArray(this, localName, namespaceUri, quotas.MaxArrayLength);
	}

	public override TimeSpan[] ReadTimeSpanArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		return TimeSpanArrayHelperWithDictionaryString.Instance.ReadArray(this, localName, namespaceUri, quotas.MaxArrayLength);
	}

	public string GetOpenElements()
	{
		string text = string.Empty;
		for (int num = depth; num > 0; num--)
		{
			string text2 = elementNodes[num].LocalName.GetString();
			if (num != depth)
			{
				text += ", ";
			}
			text += text2;
		}
		return text;
	}

	private char[] GetCharBuffer(int count)
	{
		if (count > 1024)
		{
			return new char[count];
		}
		if (chars == null || chars.Length < count)
		{
			chars = new char[count];
		}
		return chars;
	}

	private void SignStartElement(XmlSigningNodeWriter writer)
	{
		int offset;
		int length;
		byte[] prefixBuffer = node.Prefix.GetString(out offset, out length);
		int offset2;
		int length2;
		byte[] localNameBuffer = node.LocalName.GetString(out offset2, out length2);
		writer.WriteStartElement(prefixBuffer, offset, length, localNameBuffer, offset2, length2);
	}

	private void SignAttribute(XmlSigningNodeWriter writer, XmlAttributeNode attributeNode)
	{
		if (attributeNode.QNameType == QNameType.Normal)
		{
			int offset;
			int length;
			byte[] prefixBuffer = attributeNode.Prefix.GetString(out offset, out length);
			int offset2;
			int length2;
			byte[] localNameBuffer = attributeNode.LocalName.GetString(out offset2, out length2);
			writer.WriteStartAttribute(prefixBuffer, offset, length, localNameBuffer, offset2, length2);
			attributeNode.Value.Sign(writer);
			writer.WriteEndAttribute();
		}
		else
		{
			int offset3;
			int length3;
			byte[] prefixBuffer2 = attributeNode.Namespace.Prefix.GetString(out offset3, out length3);
			int offset4;
			int length4;
			byte[] nsBuffer = attributeNode.Namespace.Uri.GetString(out offset4, out length4);
			writer.WriteXmlnsAttribute(prefixBuffer2, offset3, length3, nsBuffer, offset4, length4);
		}
	}

	private void SignEndElement(XmlSigningNodeWriter writer)
	{
		int offset;
		int length;
		byte[] prefixBuffer = node.Prefix.GetString(out offset, out length);
		int offset2;
		int length2;
		byte[] localNameBuffer = node.LocalName.GetString(out offset2, out length2);
		writer.WriteEndElement(prefixBuffer, offset, length, localNameBuffer, offset2, length2);
	}

	private void SignNode(XmlSigningNodeWriter writer)
	{
		switch (node.NodeType)
		{
		case XmlNodeType.Element:
		{
			SignStartElement(writer);
			for (int i = 0; i < attributeCount; i++)
			{
				SignAttribute(writer, attributeNodes[i]);
			}
			writer.WriteEndStartElement(node.IsEmptyElement);
			break;
		}
		case XmlNodeType.Text:
		case XmlNodeType.CDATA:
		case XmlNodeType.Whitespace:
		case XmlNodeType.SignificantWhitespace:
			node.Value.Sign(writer);
			break;
		case XmlNodeType.XmlDeclaration:
			writer.WriteDeclaration();
			break;
		case XmlNodeType.Comment:
			writer.WriteComment(node.Value.GetString());
			break;
		case XmlNodeType.EndElement:
			SignEndElement(writer);
			break;
		default:
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException());
		case XmlNodeType.None:
			break;
		}
	}

	protected void SignNode()
	{
		if (signing)
		{
			SignNode(signingWriter);
		}
	}

	public override void StartCanonicalization(Stream stream, bool includeComments, string[] inclusivePrefixes)
	{
		if (signing)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("XML canonicalization started")));
		}
		if (signingWriter == null)
		{
			signingWriter = CreateSigningNodeWriter();
		}
		signingWriter.SetOutput(XmlNodeWriter.Null, stream, includeComments, inclusivePrefixes);
		nsMgr.Sign(signingWriter);
		signing = true;
	}

	public override void EndCanonicalization()
	{
		if (!signing)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("XML canonicalization was not started.")));
		}
		signingWriter.Flush();
		signingWriter.Close();
		signing = false;
	}

	protected abstract XmlSigningNodeWriter CreateSigningNodeWriter();
}
