using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace System.Xml;

internal sealed class XmlSqlBinaryReader : XmlReader, IXmlNamespaceResolver
{
	private enum ScanState
	{
		Doc,
		XmlText,
		Attr,
		AttrVal,
		AttrValPseudoValue,
		Init,
		Error,
		EOF,
		Closed
	}

	internal struct QName
	{
		public string prefix;

		public string localname;

		public string namespaceUri;

		public QName(string prefix, string lname, string nsUri)
		{
			this.prefix = prefix;
			localname = lname;
			namespaceUri = nsUri;
		}

		public void Set(string prefix, string lname, string nsUri)
		{
			this.prefix = prefix;
			localname = lname;
			namespaceUri = nsUri;
		}

		public void Clear()
		{
			prefix = (localname = (namespaceUri = string.Empty));
		}

		public bool MatchNs(string lname, string nsUri)
		{
			if (lname == localname)
			{
				return nsUri == namespaceUri;
			}
			return false;
		}

		public bool MatchPrefix(string prefix, string lname)
		{
			if (lname == localname)
			{
				return prefix == this.prefix;
			}
			return false;
		}

		public void CheckPrefixNS(string prefix, string namespaceUri)
		{
			if (this.prefix == prefix && this.namespaceUri != namespaceUri)
			{
				throw new XmlException("Prefix '{0}' is already assigned to namespace '{1}' and cannot be reassigned to '{2}' on this tag.", new string[3] { prefix, this.namespaceUri, namespaceUri });
			}
		}

		public override int GetHashCode()
		{
			return prefix.GetHashCode() ^ localname.GetHashCode();
		}

		public int GetNSHashCode(SecureStringHasher hasher)
		{
			return hasher.GetHashCode(namespaceUri) ^ hasher.GetHashCode(localname);
		}

		public override bool Equals(object other)
		{
			if (other is QName qName)
			{
				return this == qName;
			}
			return false;
		}

		public override string ToString()
		{
			if (prefix.Length == 0)
			{
				return localname;
			}
			return prefix + ":" + localname;
		}

		public static bool operator ==(QName a, QName b)
		{
			if (a.prefix == b.prefix && a.localname == b.localname)
			{
				return a.namespaceUri == b.namespaceUri;
			}
			return false;
		}

		public static bool operator !=(QName a, QName b)
		{
			return !(a == b);
		}
	}

	private struct ElemInfo
	{
		public QName name;

		public string xmlLang;

		public XmlSpace xmlSpace;

		public bool xmlspacePreserve;

		public NamespaceDecl nsdecls;

		public void Set(QName name, bool xmlspacePreserve)
		{
			this.name = name;
			xmlLang = null;
			xmlSpace = XmlSpace.None;
			this.xmlspacePreserve = xmlspacePreserve;
		}

		public NamespaceDecl Clear()
		{
			NamespaceDecl result = nsdecls;
			nsdecls = null;
			return result;
		}
	}

	private struct AttrInfo
	{
		public QName name;

		public string val;

		public int contentPos;

		public int hashCode;

		public int prevHash;

		public void Set(QName n, string v)
		{
			name = n;
			val = v;
			contentPos = 0;
			hashCode = 0;
			prevHash = 0;
		}

		public void Set(QName n, int pos)
		{
			name = n;
			val = null;
			contentPos = pos;
			hashCode = 0;
			prevHash = 0;
		}

		public void GetLocalnameAndNamespaceUri(out string localname, out string namespaceUri)
		{
			localname = name.localname;
			namespaceUri = name.namespaceUri;
		}

		public int GetLocalnameAndNamespaceUriAndHash(SecureStringHasher hasher, out string localname, out string namespaceUri)
		{
			localname = name.localname;
			namespaceUri = name.namespaceUri;
			return hashCode = name.GetNSHashCode(hasher);
		}

		public bool MatchNS(string localname, string namespaceUri)
		{
			return name.MatchNs(localname, namespaceUri);
		}

		public bool MatchHashNS(int hash, string localname, string namespaceUri)
		{
			if (hashCode == hash)
			{
				return name.MatchNs(localname, namespaceUri);
			}
			return false;
		}

		public void AdjustPosition(int adj)
		{
			if (contentPos != 0)
			{
				contentPos += adj;
			}
		}
	}

	private class NamespaceDecl
	{
		public string prefix;

		public string uri;

		public NamespaceDecl scopeLink;

		public NamespaceDecl prevLink;

		public int scope;

		public bool implied;

		public NamespaceDecl(string prefix, string nsuri, NamespaceDecl nextInScope, NamespaceDecl prevDecl, int scope, bool implied)
		{
			this.prefix = prefix;
			uri = nsuri;
			scopeLink = nextInScope;
			prevLink = prevDecl;
			this.scope = scope;
			this.implied = implied;
		}
	}

	private struct SymbolTables
	{
		public string[] symtable;

		public int symCount;

		public QName[] qnametable;

		public int qnameCount;

		public void Init()
		{
			symtable = new string[64];
			qnametable = new QName[16];
			symtable[0] = string.Empty;
			symCount = 1;
			qnameCount = 1;
		}
	}

	private class NestedBinXml
	{
		public SymbolTables symbolTables;

		public int docState;

		public NestedBinXml next;

		public NestedBinXml(SymbolTables symbolTables, int docState, NestedBinXml next)
		{
			this.symbolTables = symbolTables;
			this.docState = docState;
			this.next = next;
		}
	}

	internal static readonly Type TypeOfObject = typeof(object);

	internal static readonly Type TypeOfString = typeof(string);

	private static volatile Type[] TokenTypeMap = null;

	private static byte[] XsdKatmaiTimeScaleToValueLengthMap = new byte[8] { 3, 3, 3, 4, 4, 5, 5, 5 };

	private static ReadState[] ScanState2ReadState = new ReadState[9]
	{
		ReadState.Interactive,
		ReadState.Interactive,
		ReadState.Interactive,
		ReadState.Interactive,
		ReadState.Interactive,
		ReadState.Initial,
		ReadState.Error,
		ReadState.EndOfFile,
		ReadState.Closed
	};

	private Stream inStrm;

	private byte[] data;

	private int pos;

	private int mark;

	private int end;

	private long offset;

	private bool eof;

	private bool sniffed;

	private bool isEmpty;

	private int docState;

	private SymbolTables symbolTables;

	private XmlNameTable xnt;

	private bool xntFromSettings;

	private string xml;

	private string xmlns;

	private string nsxmlns;

	private string baseUri;

	private ScanState state;

	private XmlNodeType nodetype;

	private BinXmlToken token;

	private int attrIndex;

	private QName qnameOther;

	private QName qnameElement;

	private XmlNodeType parentNodeType;

	private ElemInfo[] elementStack;

	private int elemDepth;

	private AttrInfo[] attributes;

	private int[] attrHashTbl;

	private int attrCount;

	private int posAfterAttrs;

	private bool xmlspacePreserve;

	private int tokLen;

	private int tokDataPos;

	private bool hasTypedValue;

	private Type valueType;

	private string stringValue;

	private Dictionary<string, NamespaceDecl> namespaces;

	private NestedBinXml prevNameInfo;

	private XmlReader textXmlReader;

	private bool closeInput;

	private bool checkCharacters;

	private bool ignoreWhitespace;

	private bool ignorePIs;

	private bool ignoreComments;

	private DtdProcessing dtdProcessing;

	private SecureStringHasher hasher;

	private XmlCharType xmlCharType;

	private Encoding unicode;

	private byte version;

	public override XmlReaderSettings Settings
	{
		get
		{
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			if (xntFromSettings)
			{
				xmlReaderSettings.NameTable = xnt;
			}
			switch (docState)
			{
			case 0:
				xmlReaderSettings.ConformanceLevel = ConformanceLevel.Auto;
				break;
			case 9:
				xmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
				break;
			default:
				xmlReaderSettings.ConformanceLevel = ConformanceLevel.Document;
				break;
			}
			xmlReaderSettings.CheckCharacters = checkCharacters;
			xmlReaderSettings.IgnoreWhitespace = ignoreWhitespace;
			xmlReaderSettings.IgnoreProcessingInstructions = ignorePIs;
			xmlReaderSettings.IgnoreComments = ignoreComments;
			xmlReaderSettings.DtdProcessing = dtdProcessing;
			xmlReaderSettings.CloseInput = closeInput;
			xmlReaderSettings.ReadOnly = true;
			return xmlReaderSettings;
		}
	}

	public override XmlNodeType NodeType => nodetype;

	public override string LocalName => qnameOther.localname;

	public override string NamespaceURI => qnameOther.namespaceUri;

	public override string Prefix => qnameOther.prefix;

	public override bool HasValue
	{
		get
		{
			if (ScanState.XmlText == state)
			{
				return textXmlReader.HasValue;
			}
			return XmlReader.HasValueInternal(nodetype);
		}
	}

	public override string Value
	{
		get
		{
			if (stringValue != null)
			{
				return stringValue;
			}
			switch (state)
			{
			case ScanState.Doc:
				switch (nodetype)
				{
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.Comment:
				case XmlNodeType.DocumentType:
					return stringValue = GetString(tokDataPos, tokLen);
				case XmlNodeType.CDATA:
					return stringValue = CDATAValue();
				case XmlNodeType.XmlDeclaration:
					return stringValue = XmlDeclValue();
				case XmlNodeType.Text:
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					return stringValue = ValueAsString(token);
				}
				break;
			case ScanState.XmlText:
				return textXmlReader.Value;
			case ScanState.Attr:
			case ScanState.AttrValPseudoValue:
				return stringValue = GetAttributeText(attrIndex - 1);
			case ScanState.AttrVal:
				return stringValue = ValueAsString(token);
			}
			return string.Empty;
		}
	}

	public override int Depth
	{
		get
		{
			int num = 0;
			switch (state)
			{
			case ScanState.Doc:
				if (nodetype == XmlNodeType.Element || nodetype == XmlNodeType.EndElement)
				{
					num = -1;
				}
				break;
			case ScanState.XmlText:
				num = textXmlReader.Depth;
				break;
			case ScanState.Attr:
				if (parentNodeType != XmlNodeType.Element)
				{
					num = 1;
				}
				break;
			case ScanState.AttrVal:
			case ScanState.AttrValPseudoValue:
				if (parentNodeType != XmlNodeType.Element)
				{
					num = 1;
				}
				num++;
				break;
			default:
				return 0;
			}
			return elemDepth + num;
		}
	}

	public override string BaseURI => baseUri;

	public override bool IsEmptyElement
	{
		get
		{
			ScanState scanState = state;
			if ((uint)scanState <= 1u)
			{
				return isEmpty;
			}
			return false;
		}
	}

	public override XmlSpace XmlSpace
	{
		get
		{
			if (ScanState.XmlText != state)
			{
				for (int num = elemDepth; num >= 0; num--)
				{
					XmlSpace xmlSpace = elementStack[num].xmlSpace;
					if (xmlSpace != XmlSpace.None)
					{
						return xmlSpace;
					}
				}
				return XmlSpace.None;
			}
			return textXmlReader.XmlSpace;
		}
	}

	public override string XmlLang
	{
		get
		{
			if (ScanState.XmlText != state)
			{
				for (int num = elemDepth; num >= 0; num--)
				{
					string xmlLang = elementStack[num].xmlLang;
					if (xmlLang != null)
					{
						return xmlLang;
					}
				}
				return string.Empty;
			}
			return textXmlReader.XmlLang;
		}
	}

	public override Type ValueType => valueType;

	public override int AttributeCount
	{
		get
		{
			switch (state)
			{
			case ScanState.Doc:
			case ScanState.Attr:
			case ScanState.AttrVal:
			case ScanState.AttrValPseudoValue:
				return attrCount;
			case ScanState.XmlText:
				return textXmlReader.AttributeCount;
			default:
				return 0;
			}
		}
	}

	public override bool EOF => state == ScanState.EOF;

	public override XmlNameTable NameTable => xnt;

	public override ReadState ReadState => ScanState2ReadState[(int)state];

	public XmlSqlBinaryReader(Stream stream, byte[] data, int len, string baseUri, bool closeInput, XmlReaderSettings settings)
	{
		unicode = Encoding.Unicode;
		xmlCharType = XmlCharType.Instance;
		xnt = settings.NameTable;
		if (xnt == null)
		{
			xnt = new NameTable();
			xntFromSettings = false;
		}
		else
		{
			xntFromSettings = true;
		}
		xml = xnt.Add("xml");
		xmlns = xnt.Add("xmlns");
		nsxmlns = xnt.Add("http://www.w3.org/2000/xmlns/");
		this.baseUri = baseUri;
		state = ScanState.Init;
		nodetype = XmlNodeType.None;
		token = BinXmlToken.Error;
		elementStack = new ElemInfo[16];
		attributes = new AttrInfo[8];
		attrHashTbl = new int[8];
		symbolTables.Init();
		qnameOther.Clear();
		qnameElement.Clear();
		xmlspacePreserve = false;
		hasher = new SecureStringHasher();
		namespaces = new Dictionary<string, NamespaceDecl>(hasher);
		AddInitNamespace(string.Empty, string.Empty);
		AddInitNamespace(xml, xnt.Add("http://www.w3.org/XML/1998/namespace"));
		AddInitNamespace(xmlns, nsxmlns);
		valueType = TypeOfString;
		inStrm = stream;
		if (data != null)
		{
			this.data = data;
			end = len;
			pos = 2;
			sniffed = true;
		}
		else
		{
			this.data = new byte[4096];
			end = stream.Read(this.data, 0, 4096);
			pos = 0;
			sniffed = false;
		}
		mark = -1;
		eof = end == 0;
		offset = 0L;
		this.closeInput = closeInput;
		switch (settings.ConformanceLevel)
		{
		case ConformanceLevel.Auto:
			docState = 0;
			break;
		case ConformanceLevel.Fragment:
			docState = 9;
			break;
		case ConformanceLevel.Document:
			docState = 1;
			break;
		}
		checkCharacters = settings.CheckCharacters;
		dtdProcessing = settings.DtdProcessing;
		ignoreWhitespace = settings.IgnoreWhitespace;
		ignorePIs = settings.IgnoreProcessingInstructions;
		ignoreComments = settings.IgnoreComments;
		if (TokenTypeMap == null)
		{
			GenerateTokenTypeMap();
		}
	}

	public override string GetAttribute(string name, string ns)
	{
		if (ScanState.XmlText == state)
		{
			return textXmlReader.GetAttribute(name, ns);
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (ns == null)
		{
			ns = string.Empty;
		}
		int num = LocateAttribute(name, ns);
		if (-1 == num)
		{
			return null;
		}
		return GetAttribute(num);
	}

	public override string GetAttribute(string name)
	{
		if (ScanState.XmlText == state)
		{
			return textXmlReader.GetAttribute(name);
		}
		int num = LocateAttribute(name);
		if (-1 == num)
		{
			return null;
		}
		return GetAttribute(num);
	}

	public override string GetAttribute(int i)
	{
		if (ScanState.XmlText == state)
		{
			return textXmlReader.GetAttribute(i);
		}
		if (i < 0 || i >= attrCount)
		{
			throw new ArgumentOutOfRangeException("i");
		}
		return GetAttributeText(i);
	}

	public override bool MoveToAttribute(string name, string ns)
	{
		if (ScanState.XmlText == state)
		{
			return UpdateFromTextReader(textXmlReader.MoveToAttribute(name, ns));
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (ns == null)
		{
			ns = string.Empty;
		}
		int num = LocateAttribute(name, ns);
		if (-1 != num && state < ScanState.Init)
		{
			PositionOnAttribute(num + 1);
			return true;
		}
		return false;
	}

	public override bool MoveToAttribute(string name)
	{
		if (ScanState.XmlText == state)
		{
			return UpdateFromTextReader(textXmlReader.MoveToAttribute(name));
		}
		int num = LocateAttribute(name);
		if (-1 != num && state < ScanState.Init)
		{
			PositionOnAttribute(num + 1);
			return true;
		}
		return false;
	}

	public override void MoveToAttribute(int i)
	{
		if (ScanState.XmlText == state)
		{
			textXmlReader.MoveToAttribute(i);
			UpdateFromTextReader(needUpdate: true);
			return;
		}
		if (i < 0 || i >= attrCount)
		{
			throw new ArgumentOutOfRangeException("i");
		}
		PositionOnAttribute(i + 1);
	}

	public override bool MoveToFirstAttribute()
	{
		if (ScanState.XmlText == state)
		{
			return UpdateFromTextReader(textXmlReader.MoveToFirstAttribute());
		}
		if (attrCount == 0)
		{
			return false;
		}
		PositionOnAttribute(1);
		return true;
	}

	public override bool MoveToNextAttribute()
	{
		switch (state)
		{
		case ScanState.Doc:
		case ScanState.Attr:
		case ScanState.AttrVal:
		case ScanState.AttrValPseudoValue:
			if (attrIndex >= attrCount)
			{
				return false;
			}
			PositionOnAttribute(++attrIndex);
			return true;
		case ScanState.XmlText:
			return UpdateFromTextReader(textXmlReader.MoveToNextAttribute());
		default:
			return false;
		}
	}

	public override bool MoveToElement()
	{
		switch (state)
		{
		case ScanState.Attr:
		case ScanState.AttrVal:
		case ScanState.AttrValPseudoValue:
			attrIndex = 0;
			qnameOther = qnameElement;
			if (XmlNodeType.Element == parentNodeType)
			{
				token = BinXmlToken.Element;
			}
			else if (XmlNodeType.XmlDeclaration == parentNodeType)
			{
				token = BinXmlToken.XmlDecl;
			}
			else if (XmlNodeType.DocumentType == parentNodeType)
			{
				token = BinXmlToken.DocType;
			}
			nodetype = parentNodeType;
			state = ScanState.Doc;
			pos = posAfterAttrs;
			stringValue = null;
			return true;
		case ScanState.XmlText:
			return UpdateFromTextReader(textXmlReader.MoveToElement());
		default:
			return false;
		}
	}

	public override bool ReadAttributeValue()
	{
		stringValue = null;
		switch (state)
		{
		case ScanState.Attr:
			if (attributes[attrIndex - 1].val == null)
			{
				pos = attributes[attrIndex - 1].contentPos;
				BinXmlToken binXmlToken = RescanNextToken();
				if (BinXmlToken.Attr == binXmlToken || BinXmlToken.EndAttrs == binXmlToken)
				{
					return false;
				}
				token = binXmlToken;
				ReScanOverValue(binXmlToken);
				valueType = GetValueType(binXmlToken);
				state = ScanState.AttrVal;
			}
			else
			{
				token = BinXmlToken.Error;
				valueType = TypeOfString;
				state = ScanState.AttrValPseudoValue;
			}
			qnameOther.Clear();
			nodetype = XmlNodeType.Text;
			return true;
		case ScanState.AttrVal:
			return false;
		case ScanState.XmlText:
			return UpdateFromTextReader(textXmlReader.ReadAttributeValue());
		default:
			return false;
		}
	}

	public override void Close()
	{
		state = ScanState.Closed;
		nodetype = XmlNodeType.None;
		token = BinXmlToken.Error;
		stringValue = null;
		if (textXmlReader != null)
		{
			textXmlReader.Close();
			textXmlReader = null;
		}
		if (inStrm != null && closeInput)
		{
			inStrm.Close();
		}
		inStrm = null;
		pos = (end = 0);
	}

	public override string LookupNamespace(string prefix)
	{
		if (ScanState.XmlText == state)
		{
			return textXmlReader.LookupNamespace(prefix);
		}
		if (prefix != null && namespaces.TryGetValue(prefix, out var value))
		{
			return value.uri;
		}
		return null;
	}

	public override void ResolveEntity()
	{
		throw new NotSupportedException();
	}

	public override bool Read()
	{
		try
		{
			switch (state)
			{
			case ScanState.Init:
				return ReadInit(skipXmlDecl: false);
			case ScanState.Doc:
				return ReadDoc();
			case ScanState.XmlText:
				if (textXmlReader.Read())
				{
					return UpdateFromTextReader(needUpdate: true);
				}
				state = ScanState.Doc;
				nodetype = XmlNodeType.None;
				isEmpty = false;
				goto case ScanState.Doc;
			case ScanState.Attr:
			case ScanState.AttrVal:
			case ScanState.AttrValPseudoValue:
				MoveToElement();
				goto case ScanState.Doc;
			default:
				return false;
			}
		}
		catch (OverflowException ex)
		{
			state = ScanState.Error;
			throw new XmlException(ex.Message, ex);
		}
		catch
		{
			state = ScanState.Error;
			throw;
		}
	}

	private bool SetupContentAsXXX(string name)
	{
		if (!XmlReader.CanReadContentAs(NodeType))
		{
			throw CreateReadContentAsException(name);
		}
		switch (state)
		{
		case ScanState.Doc:
			if (NodeType == XmlNodeType.EndElement)
			{
				return true;
			}
			if (NodeType == XmlNodeType.ProcessingInstruction || NodeType == XmlNodeType.Comment)
			{
				while (Read() && (NodeType == XmlNodeType.ProcessingInstruction || NodeType == XmlNodeType.Comment))
				{
				}
				if (NodeType == XmlNodeType.EndElement)
				{
					return true;
				}
			}
			if (hasTypedValue)
			{
				return true;
			}
			break;
		case ScanState.Attr:
		{
			pos = attributes[attrIndex - 1].contentPos;
			BinXmlToken binXmlToken = RescanNextToken();
			if (BinXmlToken.Attr != binXmlToken && BinXmlToken.EndAttrs != binXmlToken)
			{
				token = binXmlToken;
				ReScanOverValue(binXmlToken);
				return true;
			}
			break;
		}
		case ScanState.AttrVal:
			return true;
		}
		return false;
	}

	private int FinishContentAsXXX(int origPos)
	{
		if (state == ScanState.Doc)
		{
			if (NodeType != XmlNodeType.Element && NodeType != XmlNodeType.EndElement)
			{
				while (Read())
				{
					XmlNodeType nodeType = NodeType;
					if (nodeType == XmlNodeType.Element)
					{
						break;
					}
					if ((uint)(nodeType - 7) > 1u)
					{
						if (nodeType == XmlNodeType.EndElement)
						{
							break;
						}
						throw ThrowNotSupported("Lists of BinaryXml value tokens not supported.");
					}
				}
			}
			return pos;
		}
		return origPos;
	}

	public override bool ReadContentAsBoolean()
	{
		int origPos = pos;
		bool flag = false;
		try
		{
			if (SetupContentAsXXX("ReadContentAsBoolean"))
			{
				try
				{
					switch (token)
					{
					case BinXmlToken.XSD_BOOLEAN:
						flag = data[tokDataPos] != 0;
						goto IL_0171;
					case BinXmlToken.SQL_SMALLINT:
					case BinXmlToken.SQL_INT:
					case BinXmlToken.SQL_REAL:
					case BinXmlToken.SQL_FLOAT:
					case BinXmlToken.SQL_MONEY:
					case BinXmlToken.SQL_BIT:
					case BinXmlToken.SQL_TINYINT:
					case BinXmlToken.SQL_BIGINT:
					case BinXmlToken.SQL_UUID:
					case BinXmlToken.SQL_DECIMAL:
					case BinXmlToken.SQL_NUMERIC:
					case BinXmlToken.SQL_BINARY:
					case BinXmlToken.SQL_VARBINARY:
					case BinXmlToken.SQL_DATETIME:
					case BinXmlToken.SQL_SMALLDATETIME:
					case BinXmlToken.SQL_SMALLMONEY:
					case BinXmlToken.SQL_IMAGE:
					case BinXmlToken.SQL_UDT:
					case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
					case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
					case BinXmlToken.XSD_KATMAI_DATEOFFSET:
					case BinXmlToken.XSD_KATMAI_TIME:
					case BinXmlToken.XSD_KATMAI_DATETIME:
					case BinXmlToken.XSD_KATMAI_DATE:
					case BinXmlToken.XSD_TIME:
					case BinXmlToken.XSD_DATETIME:
					case BinXmlToken.XSD_DATE:
					case BinXmlToken.XSD_BINHEX:
					case BinXmlToken.XSD_BASE64:
					case BinXmlToken.XSD_DECIMAL:
					case BinXmlToken.XSD_BYTE:
					case BinXmlToken.XSD_UNSIGNEDSHORT:
					case BinXmlToken.XSD_UNSIGNEDINT:
					case BinXmlToken.XSD_UNSIGNEDLONG:
					case BinXmlToken.XSD_QNAME:
						throw new InvalidCastException(Res.GetString("Token '{0}' does not support a conversion to Clr type '{1}'.", token, "Boolean"));
					case BinXmlToken.EndElem:
					case BinXmlToken.Element:
						return XmlConvert.ToBoolean(string.Empty);
					}
				}
				catch (InvalidCastException innerException)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", "Boolean", innerException, null);
				}
				catch (FormatException innerException2)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", "Boolean", innerException2, null);
				}
			}
			goto end_IL_0009;
			IL_0171:
			origPos = FinishContentAsXXX(origPos);
			return flag;
			end_IL_0009:;
		}
		finally
		{
			pos = origPos;
		}
		return base.ReadContentAsBoolean();
	}

	public override DateTime ReadContentAsDateTime()
	{
		int origPos = pos;
		try
		{
			DateTime result;
			if (SetupContentAsXXX("ReadContentAsDateTime"))
			{
				try
				{
					switch (token)
					{
					case BinXmlToken.SQL_DATETIME:
					case BinXmlToken.SQL_SMALLDATETIME:
					case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
					case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
					case BinXmlToken.XSD_KATMAI_DATEOFFSET:
					case BinXmlToken.XSD_KATMAI_TIME:
					case BinXmlToken.XSD_KATMAI_DATETIME:
					case BinXmlToken.XSD_KATMAI_DATE:
					case BinXmlToken.XSD_TIME:
					case BinXmlToken.XSD_DATETIME:
					case BinXmlToken.XSD_DATE:
						result = ValueAsDateTime();
						goto IL_017b;
					case BinXmlToken.SQL_SMALLINT:
					case BinXmlToken.SQL_INT:
					case BinXmlToken.SQL_REAL:
					case BinXmlToken.SQL_FLOAT:
					case BinXmlToken.SQL_MONEY:
					case BinXmlToken.SQL_BIT:
					case BinXmlToken.SQL_TINYINT:
					case BinXmlToken.SQL_BIGINT:
					case BinXmlToken.SQL_UUID:
					case BinXmlToken.SQL_DECIMAL:
					case BinXmlToken.SQL_NUMERIC:
					case BinXmlToken.SQL_BINARY:
					case BinXmlToken.SQL_VARBINARY:
					case BinXmlToken.SQL_SMALLMONEY:
					case BinXmlToken.SQL_IMAGE:
					case BinXmlToken.SQL_UDT:
					case BinXmlToken.XSD_BINHEX:
					case BinXmlToken.XSD_BASE64:
					case BinXmlToken.XSD_BOOLEAN:
					case BinXmlToken.XSD_DECIMAL:
					case BinXmlToken.XSD_BYTE:
					case BinXmlToken.XSD_UNSIGNEDSHORT:
					case BinXmlToken.XSD_UNSIGNEDINT:
					case BinXmlToken.XSD_UNSIGNEDLONG:
					case BinXmlToken.XSD_QNAME:
						throw new InvalidCastException(Res.GetString("Token '{0}' does not support a conversion to Clr type '{1}'.", token, "DateTime"));
					case BinXmlToken.EndElem:
					case BinXmlToken.Element:
						return XmlConvert.ToDateTime(string.Empty, XmlDateTimeSerializationMode.RoundtripKind);
					}
				}
				catch (InvalidCastException innerException)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", "DateTime", innerException, null);
				}
				catch (FormatException innerException2)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", "DateTime", innerException2, null);
				}
				catch (OverflowException innerException3)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", "DateTime", innerException3, null);
				}
			}
			goto end_IL_0007;
			IL_017b:
			origPos = FinishContentAsXXX(origPos);
			return result;
			end_IL_0007:;
		}
		finally
		{
			pos = origPos;
		}
		return base.ReadContentAsDateTime();
	}

	public override double ReadContentAsDouble()
	{
		int origPos = pos;
		try
		{
			double result;
			if (SetupContentAsXXX("ReadContentAsDouble"))
			{
				try
				{
					switch (token)
					{
					case BinXmlToken.SQL_REAL:
					case BinXmlToken.SQL_FLOAT:
						result = ValueAsDouble();
						goto IL_013e;
					case BinXmlToken.SQL_SMALLINT:
					case BinXmlToken.SQL_INT:
					case BinXmlToken.SQL_MONEY:
					case BinXmlToken.SQL_BIT:
					case BinXmlToken.SQL_TINYINT:
					case BinXmlToken.SQL_BIGINT:
					case BinXmlToken.SQL_UUID:
					case BinXmlToken.SQL_DECIMAL:
					case BinXmlToken.SQL_NUMERIC:
					case BinXmlToken.SQL_BINARY:
					case BinXmlToken.SQL_VARBINARY:
					case BinXmlToken.SQL_DATETIME:
					case BinXmlToken.SQL_SMALLDATETIME:
					case BinXmlToken.SQL_SMALLMONEY:
					case BinXmlToken.SQL_IMAGE:
					case BinXmlToken.SQL_UDT:
					case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
					case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
					case BinXmlToken.XSD_KATMAI_DATEOFFSET:
					case BinXmlToken.XSD_KATMAI_TIME:
					case BinXmlToken.XSD_KATMAI_DATETIME:
					case BinXmlToken.XSD_KATMAI_DATE:
					case BinXmlToken.XSD_TIME:
					case BinXmlToken.XSD_DATETIME:
					case BinXmlToken.XSD_DATE:
					case BinXmlToken.XSD_BINHEX:
					case BinXmlToken.XSD_BASE64:
					case BinXmlToken.XSD_BOOLEAN:
					case BinXmlToken.XSD_DECIMAL:
					case BinXmlToken.XSD_BYTE:
					case BinXmlToken.XSD_UNSIGNEDSHORT:
					case BinXmlToken.XSD_UNSIGNEDINT:
					case BinXmlToken.XSD_UNSIGNEDLONG:
					case BinXmlToken.XSD_QNAME:
						throw new InvalidCastException(Res.GetString("Token '{0}' does not support a conversion to Clr type '{1}'.", token, "Double"));
					case BinXmlToken.EndElem:
					case BinXmlToken.Element:
						return XmlConvert.ToDouble(string.Empty);
					}
				}
				catch (InvalidCastException innerException)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", "Double", innerException, null);
				}
				catch (FormatException innerException2)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", "Double", innerException2, null);
				}
				catch (OverflowException innerException3)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", "Double", innerException3, null);
				}
			}
			goto end_IL_0007;
			IL_013e:
			origPos = FinishContentAsXXX(origPos);
			return result;
			end_IL_0007:;
		}
		finally
		{
			pos = origPos;
		}
		return base.ReadContentAsDouble();
	}

	public override float ReadContentAsFloat()
	{
		int origPos = pos;
		try
		{
			float result;
			if (SetupContentAsXXX("ReadContentAsFloat"))
			{
				try
				{
					switch (token)
					{
					case BinXmlToken.SQL_REAL:
					case BinXmlToken.SQL_FLOAT:
						result = (float)ValueAsDouble();
						goto IL_013f;
					case BinXmlToken.SQL_SMALLINT:
					case BinXmlToken.SQL_INT:
					case BinXmlToken.SQL_MONEY:
					case BinXmlToken.SQL_BIT:
					case BinXmlToken.SQL_TINYINT:
					case BinXmlToken.SQL_BIGINT:
					case BinXmlToken.SQL_UUID:
					case BinXmlToken.SQL_DECIMAL:
					case BinXmlToken.SQL_NUMERIC:
					case BinXmlToken.SQL_BINARY:
					case BinXmlToken.SQL_VARBINARY:
					case BinXmlToken.SQL_DATETIME:
					case BinXmlToken.SQL_SMALLDATETIME:
					case BinXmlToken.SQL_SMALLMONEY:
					case BinXmlToken.SQL_IMAGE:
					case BinXmlToken.SQL_UDT:
					case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
					case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
					case BinXmlToken.XSD_KATMAI_DATEOFFSET:
					case BinXmlToken.XSD_KATMAI_TIME:
					case BinXmlToken.XSD_KATMAI_DATETIME:
					case BinXmlToken.XSD_KATMAI_DATE:
					case BinXmlToken.XSD_TIME:
					case BinXmlToken.XSD_DATETIME:
					case BinXmlToken.XSD_DATE:
					case BinXmlToken.XSD_BINHEX:
					case BinXmlToken.XSD_BASE64:
					case BinXmlToken.XSD_BOOLEAN:
					case BinXmlToken.XSD_DECIMAL:
					case BinXmlToken.XSD_BYTE:
					case BinXmlToken.XSD_UNSIGNEDSHORT:
					case BinXmlToken.XSD_UNSIGNEDINT:
					case BinXmlToken.XSD_UNSIGNEDLONG:
					case BinXmlToken.XSD_QNAME:
						throw new InvalidCastException(Res.GetString("Token '{0}' does not support a conversion to Clr type '{1}'.", token, "Float"));
					case BinXmlToken.EndElem:
					case BinXmlToken.Element:
						return XmlConvert.ToSingle(string.Empty);
					}
				}
				catch (InvalidCastException innerException)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", "Float", innerException, null);
				}
				catch (FormatException innerException2)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", "Float", innerException2, null);
				}
				catch (OverflowException innerException3)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", "Float", innerException3, null);
				}
			}
			goto end_IL_0007;
			IL_013f:
			origPos = FinishContentAsXXX(origPos);
			return result;
			end_IL_0007:;
		}
		finally
		{
			pos = origPos;
		}
		return base.ReadContentAsFloat();
	}

	public override decimal ReadContentAsDecimal()
	{
		int origPos = pos;
		try
		{
			decimal result;
			if (SetupContentAsXXX("ReadContentAsDecimal"))
			{
				try
				{
					switch (token)
					{
					case BinXmlToken.SQL_SMALLINT:
					case BinXmlToken.SQL_INT:
					case BinXmlToken.SQL_MONEY:
					case BinXmlToken.SQL_BIT:
					case BinXmlToken.SQL_TINYINT:
					case BinXmlToken.SQL_BIGINT:
					case BinXmlToken.SQL_DECIMAL:
					case BinXmlToken.SQL_NUMERIC:
					case BinXmlToken.SQL_SMALLMONEY:
					case BinXmlToken.XSD_DECIMAL:
					case BinXmlToken.XSD_BYTE:
					case BinXmlToken.XSD_UNSIGNEDSHORT:
					case BinXmlToken.XSD_UNSIGNEDINT:
					case BinXmlToken.XSD_UNSIGNEDLONG:
						result = ValueAsDecimal();
						goto IL_017a;
					case BinXmlToken.SQL_REAL:
					case BinXmlToken.SQL_FLOAT:
					case BinXmlToken.SQL_UUID:
					case BinXmlToken.SQL_BINARY:
					case BinXmlToken.SQL_VARBINARY:
					case BinXmlToken.SQL_DATETIME:
					case BinXmlToken.SQL_SMALLDATETIME:
					case BinXmlToken.SQL_IMAGE:
					case BinXmlToken.SQL_UDT:
					case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
					case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
					case BinXmlToken.XSD_KATMAI_DATEOFFSET:
					case BinXmlToken.XSD_KATMAI_TIME:
					case BinXmlToken.XSD_KATMAI_DATETIME:
					case BinXmlToken.XSD_KATMAI_DATE:
					case BinXmlToken.XSD_TIME:
					case BinXmlToken.XSD_DATETIME:
					case BinXmlToken.XSD_DATE:
					case BinXmlToken.XSD_BINHEX:
					case BinXmlToken.XSD_BASE64:
					case BinXmlToken.XSD_BOOLEAN:
					case BinXmlToken.XSD_QNAME:
						throw new InvalidCastException(Res.GetString("Token '{0}' does not support a conversion to Clr type '{1}'.", token, "Decimal"));
					case BinXmlToken.EndElem:
					case BinXmlToken.Element:
						return XmlConvert.ToDecimal(string.Empty);
					}
				}
				catch (InvalidCastException innerException)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", "Decimal", innerException, null);
				}
				catch (FormatException innerException2)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", "Decimal", innerException2, null);
				}
				catch (OverflowException innerException3)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", "Decimal", innerException3, null);
				}
			}
			goto end_IL_0007;
			IL_017a:
			origPos = FinishContentAsXXX(origPos);
			return result;
			end_IL_0007:;
		}
		finally
		{
			pos = origPos;
		}
		return base.ReadContentAsDecimal();
	}

	public override int ReadContentAsInt()
	{
		int origPos = pos;
		try
		{
			int result;
			if (SetupContentAsXXX("ReadContentAsInt"))
			{
				try
				{
					switch (token)
					{
					case BinXmlToken.SQL_SMALLINT:
					case BinXmlToken.SQL_INT:
					case BinXmlToken.SQL_MONEY:
					case BinXmlToken.SQL_BIT:
					case BinXmlToken.SQL_TINYINT:
					case BinXmlToken.SQL_BIGINT:
					case BinXmlToken.SQL_DECIMAL:
					case BinXmlToken.SQL_NUMERIC:
					case BinXmlToken.SQL_SMALLMONEY:
					case BinXmlToken.XSD_DECIMAL:
					case BinXmlToken.XSD_BYTE:
					case BinXmlToken.XSD_UNSIGNEDSHORT:
					case BinXmlToken.XSD_UNSIGNEDINT:
					case BinXmlToken.XSD_UNSIGNEDLONG:
						result = checked((int)ValueAsLong());
						goto IL_017b;
					case BinXmlToken.SQL_REAL:
					case BinXmlToken.SQL_FLOAT:
					case BinXmlToken.SQL_UUID:
					case BinXmlToken.SQL_BINARY:
					case BinXmlToken.SQL_VARBINARY:
					case BinXmlToken.SQL_DATETIME:
					case BinXmlToken.SQL_SMALLDATETIME:
					case BinXmlToken.SQL_IMAGE:
					case BinXmlToken.SQL_UDT:
					case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
					case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
					case BinXmlToken.XSD_KATMAI_DATEOFFSET:
					case BinXmlToken.XSD_KATMAI_TIME:
					case BinXmlToken.XSD_KATMAI_DATETIME:
					case BinXmlToken.XSD_KATMAI_DATE:
					case BinXmlToken.XSD_TIME:
					case BinXmlToken.XSD_DATETIME:
					case BinXmlToken.XSD_DATE:
					case BinXmlToken.XSD_BINHEX:
					case BinXmlToken.XSD_BASE64:
					case BinXmlToken.XSD_BOOLEAN:
					case BinXmlToken.XSD_QNAME:
						throw new InvalidCastException(Res.GetString("Token '{0}' does not support a conversion to Clr type '{1}'.", token, "Int32"));
					case BinXmlToken.EndElem:
					case BinXmlToken.Element:
						return XmlConvert.ToInt32(string.Empty);
					}
				}
				catch (InvalidCastException innerException)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", "Int32", innerException, null);
				}
				catch (FormatException innerException2)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", "Int32", innerException2, null);
				}
				catch (OverflowException innerException3)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", "Int32", innerException3, null);
				}
			}
			goto end_IL_0007;
			IL_017b:
			origPos = FinishContentAsXXX(origPos);
			return result;
			end_IL_0007:;
		}
		finally
		{
			pos = origPos;
		}
		return base.ReadContentAsInt();
	}

	public override long ReadContentAsLong()
	{
		int origPos = pos;
		try
		{
			long result;
			if (SetupContentAsXXX("ReadContentAsLong"))
			{
				try
				{
					switch (token)
					{
					case BinXmlToken.SQL_SMALLINT:
					case BinXmlToken.SQL_INT:
					case BinXmlToken.SQL_MONEY:
					case BinXmlToken.SQL_BIT:
					case BinXmlToken.SQL_TINYINT:
					case BinXmlToken.SQL_BIGINT:
					case BinXmlToken.SQL_DECIMAL:
					case BinXmlToken.SQL_NUMERIC:
					case BinXmlToken.SQL_SMALLMONEY:
					case BinXmlToken.XSD_DECIMAL:
					case BinXmlToken.XSD_BYTE:
					case BinXmlToken.XSD_UNSIGNEDSHORT:
					case BinXmlToken.XSD_UNSIGNEDINT:
					case BinXmlToken.XSD_UNSIGNEDLONG:
						result = ValueAsLong();
						goto IL_017a;
					case BinXmlToken.SQL_REAL:
					case BinXmlToken.SQL_FLOAT:
					case BinXmlToken.SQL_UUID:
					case BinXmlToken.SQL_BINARY:
					case BinXmlToken.SQL_VARBINARY:
					case BinXmlToken.SQL_DATETIME:
					case BinXmlToken.SQL_SMALLDATETIME:
					case BinXmlToken.SQL_IMAGE:
					case BinXmlToken.SQL_UDT:
					case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
					case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
					case BinXmlToken.XSD_KATMAI_DATEOFFSET:
					case BinXmlToken.XSD_KATMAI_TIME:
					case BinXmlToken.XSD_KATMAI_DATETIME:
					case BinXmlToken.XSD_KATMAI_DATE:
					case BinXmlToken.XSD_TIME:
					case BinXmlToken.XSD_DATETIME:
					case BinXmlToken.XSD_DATE:
					case BinXmlToken.XSD_BINHEX:
					case BinXmlToken.XSD_BASE64:
					case BinXmlToken.XSD_BOOLEAN:
					case BinXmlToken.XSD_QNAME:
						throw new InvalidCastException(Res.GetString("Token '{0}' does not support a conversion to Clr type '{1}'.", token, "Int64"));
					case BinXmlToken.EndElem:
					case BinXmlToken.Element:
						return XmlConvert.ToInt64(string.Empty);
					}
				}
				catch (InvalidCastException innerException)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", "Int64", innerException, null);
				}
				catch (FormatException innerException2)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", "Int64", innerException2, null);
				}
				catch (OverflowException innerException3)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", "Int64", innerException3, null);
				}
			}
			goto end_IL_0007;
			IL_017a:
			origPos = FinishContentAsXXX(origPos);
			return result;
			end_IL_0007:;
		}
		finally
		{
			pos = origPos;
		}
		return base.ReadContentAsLong();
	}

	public override object ReadContentAsObject()
	{
		int origPos = pos;
		try
		{
			if (SetupContentAsXXX("ReadContentAsObject"))
			{
				object result;
				try
				{
					result = ((NodeType != XmlNodeType.Element && NodeType != XmlNodeType.EndElement) ? ValueAsObject(token, returnInternalTypes: false) : string.Empty);
				}
				catch (InvalidCastException innerException)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", "Object", innerException, null);
				}
				catch (FormatException innerException2)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", "Object", innerException2, null);
				}
				catch (OverflowException innerException3)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", "Object", innerException3, null);
				}
				origPos = FinishContentAsXXX(origPos);
				return result;
			}
		}
		finally
		{
			pos = origPos;
		}
		return base.ReadContentAsObject();
	}

	public override object ReadContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
	{
		int origPos = pos;
		try
		{
			if (SetupContentAsXXX("ReadContentAs"))
			{
				object result;
				try
				{
					result = ((NodeType != XmlNodeType.Element && NodeType != XmlNodeType.EndElement) ? ((!(returnType == ValueType) && !(returnType == typeof(object))) ? ValueAs(token, returnType, namespaceResolver) : ValueAsObject(token, returnInternalTypes: false)) : string.Empty);
				}
				catch (InvalidCastException innerException)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), innerException, null);
				}
				catch (FormatException innerException2)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), innerException2, null);
				}
				catch (OverflowException innerException3)
				{
					throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), innerException3, null);
				}
				origPos = FinishContentAsXXX(origPos);
				return result;
			}
		}
		finally
		{
			pos = origPos;
		}
		return base.ReadContentAs(returnType, namespaceResolver);
	}

	IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
	{
		if (ScanState.XmlText == state)
		{
			return ((IXmlNamespaceResolver)textXmlReader).GetNamespacesInScope(scope);
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (XmlNamespaceScope.Local == scope)
		{
			if (elemDepth > 0)
			{
				for (NamespaceDecl namespaceDecl = elementStack[elemDepth].nsdecls; namespaceDecl != null; namespaceDecl = namespaceDecl.scopeLink)
				{
					dictionary.Add(namespaceDecl.prefix, namespaceDecl.uri);
				}
			}
		}
		else
		{
			foreach (NamespaceDecl value in namespaces.Values)
			{
				if ((value.scope != -1 || (scope == XmlNamespaceScope.All && "xml" == value.prefix)) && (value.prefix.Length > 0 || value.uri.Length > 0))
				{
					dictionary.Add(value.prefix, value.uri);
				}
			}
		}
		return dictionary;
	}

	string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
	{
		if (ScanState.XmlText == state)
		{
			return ((IXmlNamespaceResolver)textXmlReader).LookupPrefix(namespaceName);
		}
		if (namespaceName == null)
		{
			return null;
		}
		namespaceName = xnt.Get(namespaceName);
		if (namespaceName == null)
		{
			return null;
		}
		for (int num = elemDepth; num >= 0; num--)
		{
			for (NamespaceDecl namespaceDecl = elementStack[num].nsdecls; namespaceDecl != null; namespaceDecl = namespaceDecl.scopeLink)
			{
				if ((object)namespaceDecl.uri == namespaceName)
				{
					return namespaceDecl.prefix;
				}
			}
		}
		return null;
	}

	private void VerifyVersion(int requiredVersion, BinXmlToken token)
	{
		if (version < requiredVersion)
		{
			throw ThrowUnexpectedToken(token);
		}
	}

	private void AddInitNamespace(string prefix, string uri)
	{
		NamespaceDecl namespaceDecl = new NamespaceDecl(prefix, uri, elementStack[0].nsdecls, null, -1, implied: true);
		elementStack[0].nsdecls = namespaceDecl;
		namespaces.Add(prefix, namespaceDecl);
	}

	private void AddName()
	{
		string array = ParseText();
		int num = symbolTables.symCount++;
		string[] array2 = symbolTables.symtable;
		if (num == array2.Length)
		{
			string[] array3 = new string[checked(num * 2)];
			Array.Copy(array2, 0, array3, 0, num);
			array2 = (symbolTables.symtable = array3);
		}
		array2[num] = xnt.Add(array);
	}

	private void AddQName()
	{
		int num = ReadNameRef();
		int num2 = ReadNameRef();
		int num3 = ReadNameRef();
		int num4 = symbolTables.qnameCount++;
		QName[] array = symbolTables.qnametable;
		if (num4 == array.Length)
		{
			QName[] array2 = new QName[checked(num4 * 2)];
			Array.Copy(array, 0, array2, 0, num4);
			array = (symbolTables.qnametable = array2);
		}
		string[] symtable = symbolTables.symtable;
		string text = symtable[num2];
		string lname;
		string nsUri;
		if (num3 == 0)
		{
			if (num2 == 0 && num == 0)
			{
				return;
			}
			if (!text.StartsWith("xmlns", StringComparison.Ordinal))
			{
				goto IL_0106;
			}
			if (5 < text.Length)
			{
				if (6 == text.Length || ':' != text[5])
				{
					goto IL_0106;
				}
				lname = xnt.Add(text.Substring(6));
				text = xmlns;
			}
			else
			{
				lname = text;
				text = string.Empty;
			}
			nsUri = nsxmlns;
		}
		else
		{
			lname = symtable[num3];
			nsUri = symtable[num];
		}
		array[num4].Set(text, lname, nsUri);
		return;
		IL_0106:
		throw new XmlException("Invalid namespace declaration.", (string[])null);
	}

	private void NameFlush()
	{
		symbolTables.symCount = (symbolTables.qnameCount = 1);
		Array.Clear(symbolTables.symtable, 1, symbolTables.symtable.Length - 1);
		Array.Clear(symbolTables.qnametable, 0, symbolTables.qnametable.Length);
	}

	private void SkipExtn()
	{
		int num = ParseMB32();
		checked
		{
			pos += num;
			Fill(-1);
		}
	}

	private int ReadQNameRef()
	{
		int num = ParseMB32();
		if (num < 0 || num >= symbolTables.qnameCount)
		{
			throw new XmlException("Invalid QName ID.", string.Empty);
		}
		return num;
	}

	private int ReadNameRef()
	{
		int num = ParseMB32();
		if (num < 0 || num >= symbolTables.symCount)
		{
			throw new XmlException("Invalid QName ID.", string.Empty);
		}
		return num;
	}

	private bool FillAllowEOF()
	{
		if (eof)
		{
			return false;
		}
		byte[] array = data;
		int num = pos;
		int num2 = mark;
		int num3 = end;
		if (num2 == -1)
		{
			num2 = num;
		}
		if (num2 >= 0 && num2 < num3)
		{
			int num4 = num3 - num2;
			if (num4 > 7 * (array.Length / 8))
			{
				byte[] destinationArray = new byte[checked(array.Length * 2)];
				Array.Copy(array, num2, destinationArray, 0, num4);
				array = (data = destinationArray);
			}
			else
			{
				Array.Copy(array, num2, array, 0, num4);
			}
			num -= num2;
			num3 -= num2;
			tokDataPos -= num2;
			for (int i = 0; i < attrCount; i++)
			{
				attributes[i].AdjustPosition(-num2);
			}
			pos = num;
			mark = 0;
			offset += num2;
		}
		else
		{
			pos -= num3;
			mark -= num3;
			offset += num3;
			tokDataPos -= num3;
			num3 = 0;
		}
		int count = array.Length - num3;
		int num5 = inStrm.Read(array, num3, count);
		end = num3 + num5;
		eof = num5 <= 0;
		return num5 > 0;
	}

	private void Fill_(int require)
	{
		while (FillAllowEOF() && pos + require >= end)
		{
		}
		if (pos + require >= end)
		{
			throw ThrowXmlException("Unexpected end of file has occurred.");
		}
	}

	private void Fill(int require)
	{
		if (pos + require >= end)
		{
			Fill_(require);
		}
	}

	private byte ReadByte()
	{
		Fill(0);
		return data[pos++];
	}

	private ushort ReadUShort()
	{
		Fill(1);
		int num = pos;
		byte[] array = data;
		ushort result = (ushort)(array[num] + (array[num + 1] << 8));
		pos += 2;
		return result;
	}

	private int ParseMB32()
	{
		byte b = ReadByte();
		if (b > 127)
		{
			return ParseMB32_(b);
		}
		return b;
	}

	private int ParseMB32_(byte b)
	{
		uint num = (uint)(b & 0x7F);
		b = ReadByte();
		uint num2 = (uint)(b & 0x7F);
		num += num2 << 7;
		if (b > 127)
		{
			b = ReadByte();
			num2 = (uint)(b & 0x7F);
			num += num2 << 14;
			if (b > 127)
			{
				b = ReadByte();
				num2 = (uint)(b & 0x7F);
				num += num2 << 21;
				if (b > 127)
				{
					b = ReadByte();
					num2 = (uint)(b & 7);
					if (b > 7)
					{
						throw ThrowXmlException("The value is too big to fit into an Int32. The arithmetic operation resulted in an overflow.");
					}
					num += num2 << 28;
				}
			}
		}
		return (int)num;
	}

	private int ParseMB32(int pos)
	{
		byte[] array = data;
		byte num = array[pos++];
		uint num2 = (uint)(num & 0x7F);
		if (num > 127)
		{
			byte num3 = array[pos++];
			uint num4 = (uint)(num3 & 0x7F);
			num2 += num4 << 7;
			if (num3 > 127)
			{
				byte num5 = array[pos++];
				num4 = (uint)(num5 & 0x7F);
				num2 += num4 << 14;
				if (num5 > 127)
				{
					byte num6 = array[pos++];
					num4 = (uint)(num6 & 0x7F);
					num2 += num4 << 21;
					if (num6 > 127)
					{
						byte num7 = array[pos++];
						num4 = (uint)(num7 & 7);
						if (num7 > 7)
						{
							throw ThrowXmlException("The value is too big to fit into an Int32. The arithmetic operation resulted in an overflow.");
						}
						num2 += num4 << 28;
					}
				}
			}
		}
		return (int)num2;
	}

	private int ParseMB64()
	{
		byte b = ReadByte();
		if (b > 127)
		{
			return ParseMB32_(b);
		}
		return b;
	}

	private BinXmlToken PeekToken()
	{
		while (pos >= end && FillAllowEOF())
		{
		}
		if (pos >= end)
		{
			return BinXmlToken.EOF;
		}
		return (BinXmlToken)data[pos];
	}

	private BinXmlToken ReadToken()
	{
		while (pos >= end && FillAllowEOF())
		{
		}
		if (pos >= end)
		{
			return BinXmlToken.EOF;
		}
		return (BinXmlToken)data[pos++];
	}

	private BinXmlToken NextToken2(BinXmlToken token)
	{
		while (true)
		{
			switch (token)
			{
			case BinXmlToken.Name:
				AddName();
				break;
			case BinXmlToken.QName:
				AddQName();
				break;
			case BinXmlToken.NmFlush:
				NameFlush();
				break;
			case BinXmlToken.Extn:
				SkipExtn();
				break;
			default:
				return token;
			}
			token = ReadToken();
		}
	}

	private BinXmlToken NextToken1()
	{
		int num = pos;
		BinXmlToken binXmlToken;
		if (num >= end)
		{
			binXmlToken = ReadToken();
		}
		else
		{
			binXmlToken = (BinXmlToken)data[num];
			pos = num + 1;
		}
		if (binXmlToken >= BinXmlToken.NmFlush && binXmlToken <= BinXmlToken.Name)
		{
			return NextToken2(binXmlToken);
		}
		return binXmlToken;
	}

	private BinXmlToken NextToken()
	{
		int num = pos;
		if (num < end)
		{
			BinXmlToken binXmlToken = (BinXmlToken)data[num];
			if (binXmlToken < BinXmlToken.NmFlush || binXmlToken > BinXmlToken.Name)
			{
				pos = num + 1;
				return binXmlToken;
			}
		}
		return NextToken1();
	}

	private BinXmlToken PeekNextToken()
	{
		BinXmlToken binXmlToken = NextToken();
		if (BinXmlToken.EOF != binXmlToken)
		{
			pos--;
		}
		return binXmlToken;
	}

	private BinXmlToken RescanNextToken()
	{
		checked
		{
			while (true)
			{
				BinXmlToken binXmlToken = ReadToken();
				switch (binXmlToken)
				{
				case BinXmlToken.NmFlush:
					break;
				case BinXmlToken.Name:
				{
					int num2 = ParseMB32();
					pos += 2 * num2;
					break;
				}
				case BinXmlToken.QName:
					ParseMB32();
					ParseMB32();
					ParseMB32();
					break;
				case BinXmlToken.Extn:
				{
					int num = ParseMB32();
					pos += num;
					break;
				}
				default:
					return binXmlToken;
				}
			}
		}
	}

	private string ParseText()
	{
		int num = mark;
		try
		{
			if (num < 0)
			{
				mark = pos;
			}
			int start;
			int cch = ScanText(out start);
			return GetString(start, cch);
		}
		finally
		{
			if (num < 0)
			{
				mark = -1;
			}
		}
	}

	private int ScanText(out int start)
	{
		int num = ParseMB32();
		int num2 = mark;
		int num3 = pos;
		checked
		{
			pos += num * 2;
			if (pos > end)
			{
				Fill(-1);
			}
		}
		start = num3 - (num2 - mark);
		return num;
	}

	private string GetString(int pos, int cch)
	{
		checked
		{
			if (pos + cch * 2 > end)
			{
				throw new XmlException("Unexpected end of file has occurred.", (string[])null);
			}
			if (cch == 0)
			{
				return string.Empty;
			}
			if ((pos & 1) == 0)
			{
				return GetStringAligned(data, pos, cch);
			}
			return unicode.GetString(data, pos, cch * 2);
		}
	}

	private unsafe string GetStringAligned(byte[] data, int offset, int cch)
	{
		fixed (byte* ptr = data)
		{
			char* value = (char*)(ptr + offset);
			return new string(value, 0, cch);
		}
	}

	private string GetAttributeText(int i)
	{
		string val = attributes[i].val;
		if (val != null)
		{
			return val;
		}
		int num = pos;
		try
		{
			pos = attributes[i].contentPos;
			BinXmlToken binXmlToken = RescanNextToken();
			if (BinXmlToken.Attr == binXmlToken || BinXmlToken.EndAttrs == binXmlToken)
			{
				return "";
			}
			token = binXmlToken;
			ReScanOverValue(binXmlToken);
			return ValueAsString(binXmlToken);
		}
		finally
		{
			pos = num;
		}
	}

	private int LocateAttribute(string name, string ns)
	{
		for (int i = 0; i < attrCount; i++)
		{
			if (attributes[i].name.MatchNs(name, ns))
			{
				return i;
			}
		}
		return -1;
	}

	private int LocateAttribute(string name)
	{
		ValidateNames.SplitQName(name, out var prefix, out var lname);
		for (int i = 0; i < attrCount; i++)
		{
			if (attributes[i].name.MatchPrefix(prefix, lname))
			{
				return i;
			}
		}
		return -1;
	}

	private void PositionOnAttribute(int i)
	{
		attrIndex = i;
		qnameOther = attributes[i - 1].name;
		if (state == ScanState.Doc)
		{
			parentNodeType = nodetype;
		}
		token = BinXmlToken.Attr;
		nodetype = XmlNodeType.Attribute;
		state = ScanState.Attr;
		valueType = TypeOfObject;
		stringValue = null;
	}

	private void GrowElements()
	{
		ElemInfo[] destinationArray = new ElemInfo[elementStack.Length * 2];
		Array.Copy(elementStack, 0, destinationArray, 0, elementStack.Length);
		elementStack = destinationArray;
	}

	private void GrowAttributes()
	{
		AttrInfo[] destinationArray = new AttrInfo[attributes.Length * 2];
		Array.Copy(attributes, 0, destinationArray, 0, attrCount);
		attributes = destinationArray;
	}

	private void ClearAttributes()
	{
		if (attrCount != 0)
		{
			attrCount = 0;
		}
	}

	private void PushNamespace(string prefix, string ns, bool implied)
	{
		if (prefix == "xml")
		{
			return;
		}
		int num = elemDepth;
		namespaces.TryGetValue(prefix, out var value);
		if (value != null)
		{
			if (value.uri == ns)
			{
				if (!implied && value.implied && value.scope == num)
				{
					value.implied = false;
				}
				return;
			}
			qnameElement.CheckPrefixNS(prefix, ns);
			if (prefix.Length != 0)
			{
				for (int i = 0; i < attrCount; i++)
				{
					if (attributes[i].name.prefix.Length != 0)
					{
						attributes[i].name.CheckPrefixNS(prefix, ns);
					}
				}
			}
		}
		NamespaceDecl namespaceDecl = new NamespaceDecl(prefix, ns, elementStack[num].nsdecls, value, num, implied);
		elementStack[num].nsdecls = namespaceDecl;
		namespaces[prefix] = namespaceDecl;
	}

	private void PopNamespaces(NamespaceDecl firstInScopeChain)
	{
		NamespaceDecl namespaceDecl = firstInScopeChain;
		while (namespaceDecl != null)
		{
			if (namespaceDecl.prevLink == null)
			{
				namespaces.Remove(namespaceDecl.prefix);
			}
			else
			{
				namespaces[namespaceDecl.prefix] = namespaceDecl.prevLink;
			}
			NamespaceDecl scopeLink = namespaceDecl.scopeLink;
			namespaceDecl.prevLink = null;
			namespaceDecl.scopeLink = null;
			namespaceDecl = scopeLink;
		}
	}

	private void GenerateImpliedXmlnsAttrs()
	{
		for (NamespaceDecl namespaceDecl = elementStack[elemDepth].nsdecls; namespaceDecl != null; namespaceDecl = namespaceDecl.scopeLink)
		{
			if (namespaceDecl.implied)
			{
				if (attrCount == attributes.Length)
				{
					GrowAttributes();
				}
				QName n = ((namespaceDecl.prefix.Length != 0) ? new QName(xmlns, xnt.Add(namespaceDecl.prefix), nsxmlns) : new QName(string.Empty, xmlns, nsxmlns));
				attributes[attrCount].Set(n, namespaceDecl.uri);
				attrCount++;
			}
		}
	}

	private bool ReadInit(bool skipXmlDecl)
	{
		string text = null;
		if (!sniffed && ReadUShort() != 65503)
		{
			text = "Invalid BinaryXml signature.";
		}
		else
		{
			version = ReadByte();
			if (version != 1 && version != 2)
			{
				text = "Invalid BinaryXml protocol version.";
			}
			else
			{
				if (1200 == ReadUShort())
				{
					state = ScanState.Doc;
					if (BinXmlToken.XmlDecl == PeekToken())
					{
						pos++;
						attributes[0].Set(new QName(string.Empty, xnt.Add("version"), string.Empty), ParseText());
						attrCount = 1;
						if (BinXmlToken.Encoding == PeekToken())
						{
							pos++;
							attributes[1].Set(new QName(string.Empty, xnt.Add("encoding"), string.Empty), ParseText());
							attrCount++;
						}
						byte b = ReadByte();
						if (b != 0)
						{
							if ((uint)(b - 1) > 1u)
							{
								text = "Invalid BinaryXml standalone token.";
								goto IL_01e2;
							}
							attributes[attrCount].Set(new QName(string.Empty, xnt.Add("standalone"), string.Empty), (b == 1) ? "yes" : "no");
							attrCount++;
						}
						if (!skipXmlDecl)
						{
							QName qName = new QName(string.Empty, xnt.Add("xml"), string.Empty);
							qnameOther = (qnameElement = qName);
							nodetype = XmlNodeType.XmlDeclaration;
							posAfterAttrs = pos;
							return true;
						}
					}
					return ReadDoc();
				}
				text = "Unsupported BinaryXml codepage.";
			}
		}
		goto IL_01e2;
		IL_01e2:
		state = ScanState.Error;
		throw new XmlException(text, (string[])null);
	}

	private void ScanAttributes()
	{
		int num = -1;
		int num2 = -1;
		mark = pos;
		string text = null;
		bool flag = false;
		BinXmlToken binXmlToken;
		while (BinXmlToken.EndAttrs != (binXmlToken = NextToken()))
		{
			if (BinXmlToken.Attr == binXmlToken)
			{
				if (text != null)
				{
					PushNamespace(text, string.Empty, implied: false);
					text = null;
				}
				if (attrCount == attributes.Length)
				{
					GrowAttributes();
				}
				QName n = symbolTables.qnametable[ReadQNameRef()];
				attributes[attrCount].Set(n, pos);
				if (n.prefix == "xml")
				{
					if (n.localname == "lang")
					{
						num2 = attrCount;
					}
					else if (n.localname == "space")
					{
						num = attrCount;
					}
				}
				else if (Ref.Equal(n.namespaceUri, nsxmlns))
				{
					text = n.localname;
					if (text == "xmlns")
					{
						text = string.Empty;
					}
				}
				else if (n.prefix.Length != 0)
				{
					if (n.namespaceUri.Length == 0)
					{
						throw new XmlException("Cannot use a prefix with an empty namespace.", string.Empty);
					}
					PushNamespace(n.prefix, n.namespaceUri, implied: true);
				}
				else if (n.namespaceUri.Length != 0)
				{
					throw ThrowXmlException("Attribute '{0}' has namespace '{1}' but no prefix.", n.localname, n.namespaceUri);
				}
				attrCount++;
				flag = false;
			}
			else
			{
				ScanOverValue(binXmlToken, attr: true, checkChars: true);
				if (flag)
				{
					throw ThrowNotSupported("Lists of BinaryXml value tokens not supported.");
				}
				string text2 = stringValue;
				if (text2 != null)
				{
					attributes[attrCount - 1].val = text2;
					stringValue = null;
				}
				if (text != null)
				{
					string ns = xnt.Add(ValueAsString(binXmlToken));
					PushNamespace(text, ns, implied: false);
					text = null;
				}
				flag = true;
			}
		}
		if (num != -1)
		{
			string attributeText = GetAttributeText(num);
			XmlSpace xmlSpace = XmlSpace.None;
			if (attributeText == "preserve")
			{
				xmlSpace = XmlSpace.Preserve;
			}
			else if (attributeText == "default")
			{
				xmlSpace = XmlSpace.Default;
			}
			elementStack[elemDepth].xmlSpace = xmlSpace;
			xmlspacePreserve = XmlSpace.Preserve == xmlSpace;
		}
		if (num2 != -1)
		{
			elementStack[elemDepth].xmlLang = GetAttributeText(num2);
		}
		if (attrCount < 200)
		{
			SimpleCheckForDuplicateAttributes();
		}
		else
		{
			HashCheckForDuplicateAttributes();
		}
	}

	private void SimpleCheckForDuplicateAttributes()
	{
		for (int i = 0; i < attrCount; i++)
		{
			attributes[i].GetLocalnameAndNamespaceUri(out var localname, out var namespaceUri);
			for (int j = i + 1; j < attrCount; j++)
			{
				if (attributes[j].MatchNS(localname, namespaceUri))
				{
					throw new XmlException("'{0}' is a duplicate attribute name.", attributes[i].name.ToString());
				}
			}
		}
	}

	private void HashCheckForDuplicateAttributes()
	{
		int num;
		for (num = 256; num < attrCount; num = checked(num * 2))
		{
		}
		if (attrHashTbl.Length < num)
		{
			attrHashTbl = new int[num];
		}
		for (int i = 0; i < attrCount; i++)
		{
			string localname;
			string namespaceUri;
			int localnameAndNamespaceUriAndHash = attributes[i].GetLocalnameAndNamespaceUriAndHash(hasher, out localname, out namespaceUri);
			int num2 = localnameAndNamespaceUriAndHash & (num - 1);
			int num3 = attrHashTbl[num2];
			attrHashTbl[num2] = i + 1;
			attributes[i].prevHash = num3;
			while (num3 != 0)
			{
				num3--;
				if (attributes[num3].MatchHashNS(localnameAndNamespaceUriAndHash, localname, namespaceUri))
				{
					throw new XmlException("'{0}' is a duplicate attribute name.", attributes[i].name.ToString());
				}
				num3 = attributes[num3].prevHash;
			}
		}
		Array.Clear(attrHashTbl, 0, num);
	}

	private string XmlDeclValue()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < attrCount; i++)
		{
			if (i > 0)
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append(attributes[i].name.localname);
			stringBuilder.Append("=\"");
			stringBuilder.Append(attributes[i].val);
			stringBuilder.Append('"');
		}
		return stringBuilder.ToString();
	}

	private string CDATAValue()
	{
		string text = GetString(tokDataPos, tokLen);
		StringBuilder stringBuilder = null;
		while (PeekToken() == BinXmlToken.CData)
		{
			pos++;
			if (stringBuilder == null)
			{
				stringBuilder = new StringBuilder(text.Length + text.Length / 2);
				stringBuilder.Append(text);
			}
			stringBuilder.Append(ParseText());
		}
		if (stringBuilder != null)
		{
			text = stringBuilder.ToString();
		}
		stringValue = text;
		return text;
	}

	private void FinishCDATA()
	{
		while (true)
		{
			switch (PeekToken())
			{
			case BinXmlToken.CData:
				break;
			case BinXmlToken.EndCData:
				pos++;
				return;
			default:
				throw new XmlException("CDATA end token is missing.");
			}
			pos++;
			ScanText(out var _);
		}
	}

	private void FinishEndElement()
	{
		NamespaceDecl firstInScopeChain = elementStack[elemDepth].Clear();
		PopNamespaces(firstInScopeChain);
		elemDepth--;
	}

	private bool ReadDoc()
	{
		switch (nodetype)
		{
		case XmlNodeType.CDATA:
			FinishCDATA();
			break;
		case XmlNodeType.EndElement:
			FinishEndElement();
			break;
		case XmlNodeType.Element:
			if (isEmpty)
			{
				FinishEndElement();
				isEmpty = false;
			}
			break;
		}
		while (true)
		{
			nodetype = XmlNodeType.None;
			mark = -1;
			if (qnameOther.localname.Length != 0)
			{
				qnameOther.Clear();
			}
			ClearAttributes();
			attrCount = 0;
			valueType = TypeOfString;
			stringValue = null;
			hasTypedValue = false;
			token = NextToken();
			switch (token)
			{
			case BinXmlToken.EOF:
				if (elemDepth > 0)
				{
					throw new XmlException("Unexpected end of file has occurred.", (string[])null);
				}
				state = ScanState.EOF;
				return false;
			case BinXmlToken.Element:
				ImplReadElement();
				break;
			case BinXmlToken.EndElem:
				ImplReadEndElement();
				break;
			case BinXmlToken.DocType:
				ImplReadDoctype();
				if (dtdProcessing == DtdProcessing.Ignore || prevNameInfo != null)
				{
					continue;
				}
				break;
			case BinXmlToken.PI:
				ImplReadPI();
				if (ignorePIs)
				{
					continue;
				}
				break;
			case BinXmlToken.Comment:
				ImplReadComment();
				if (ignoreComments)
				{
					continue;
				}
				break;
			case BinXmlToken.CData:
				ImplReadCDATA();
				break;
			case BinXmlToken.Nest:
				ImplReadNest();
				sniffed = false;
				return ReadInit(skipXmlDecl: true);
			case BinXmlToken.EndNest:
				if (prevNameInfo != null)
				{
					ImplReadEndNest();
					return ReadDoc();
				}
				goto default;
			case BinXmlToken.XmlText:
				ImplReadXmlText();
				break;
			case BinXmlToken.SQL_SMALLINT:
			case BinXmlToken.SQL_INT:
			case BinXmlToken.SQL_REAL:
			case BinXmlToken.SQL_FLOAT:
			case BinXmlToken.SQL_MONEY:
			case BinXmlToken.SQL_BIT:
			case BinXmlToken.SQL_TINYINT:
			case BinXmlToken.SQL_BIGINT:
			case BinXmlToken.SQL_UUID:
			case BinXmlToken.SQL_DECIMAL:
			case BinXmlToken.SQL_NUMERIC:
			case BinXmlToken.SQL_BINARY:
			case BinXmlToken.SQL_CHAR:
			case BinXmlToken.SQL_NCHAR:
			case BinXmlToken.SQL_VARBINARY:
			case BinXmlToken.SQL_VARCHAR:
			case BinXmlToken.SQL_NVARCHAR:
			case BinXmlToken.SQL_DATETIME:
			case BinXmlToken.SQL_SMALLDATETIME:
			case BinXmlToken.SQL_SMALLMONEY:
			case BinXmlToken.SQL_TEXT:
			case BinXmlToken.SQL_IMAGE:
			case BinXmlToken.SQL_NTEXT:
			case BinXmlToken.SQL_UDT:
			case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
			case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
			case BinXmlToken.XSD_KATMAI_DATEOFFSET:
			case BinXmlToken.XSD_KATMAI_TIME:
			case BinXmlToken.XSD_KATMAI_DATETIME:
			case BinXmlToken.XSD_KATMAI_DATE:
			case BinXmlToken.XSD_TIME:
			case BinXmlToken.XSD_DATETIME:
			case BinXmlToken.XSD_DATE:
			case BinXmlToken.XSD_BINHEX:
			case BinXmlToken.XSD_BASE64:
			case BinXmlToken.XSD_BOOLEAN:
			case BinXmlToken.XSD_DECIMAL:
			case BinXmlToken.XSD_BYTE:
			case BinXmlToken.XSD_UNSIGNEDSHORT:
			case BinXmlToken.XSD_UNSIGNEDINT:
			case BinXmlToken.XSD_UNSIGNEDLONG:
			case BinXmlToken.XSD_QNAME:
				ImplReadData(token);
				if (XmlNodeType.Text == nodetype)
				{
					CheckAllowContent();
				}
				else if (ignoreWhitespace && !xmlspacePreserve)
				{
					continue;
				}
				return true;
			default:
				throw ThrowUnexpectedToken(token);
			}
			break;
		}
		return true;
	}

	private void ImplReadData(BinXmlToken tokenType)
	{
		mark = pos;
		switch (tokenType)
		{
		case BinXmlToken.SQL_CHAR:
		case BinXmlToken.SQL_NCHAR:
		case BinXmlToken.SQL_VARCHAR:
		case BinXmlToken.SQL_NVARCHAR:
		case BinXmlToken.SQL_TEXT:
		case BinXmlToken.SQL_NTEXT:
			valueType = TypeOfString;
			hasTypedValue = false;
			break;
		default:
			valueType = GetValueType(token);
			hasTypedValue = true;
			break;
		}
		nodetype = ScanOverValue(token, attr: false, checkChars: true);
		switch (PeekNextToken())
		{
		case BinXmlToken.SQL_SMALLINT:
		case BinXmlToken.SQL_INT:
		case BinXmlToken.SQL_REAL:
		case BinXmlToken.SQL_FLOAT:
		case BinXmlToken.SQL_MONEY:
		case BinXmlToken.SQL_BIT:
		case BinXmlToken.SQL_TINYINT:
		case BinXmlToken.SQL_BIGINT:
		case BinXmlToken.SQL_UUID:
		case BinXmlToken.SQL_DECIMAL:
		case BinXmlToken.SQL_NUMERIC:
		case BinXmlToken.SQL_BINARY:
		case BinXmlToken.SQL_CHAR:
		case BinXmlToken.SQL_NCHAR:
		case BinXmlToken.SQL_VARBINARY:
		case BinXmlToken.SQL_VARCHAR:
		case BinXmlToken.SQL_NVARCHAR:
		case BinXmlToken.SQL_DATETIME:
		case BinXmlToken.SQL_SMALLDATETIME:
		case BinXmlToken.SQL_SMALLMONEY:
		case BinXmlToken.SQL_TEXT:
		case BinXmlToken.SQL_IMAGE:
		case BinXmlToken.SQL_NTEXT:
		case BinXmlToken.SQL_UDT:
		case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
		case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
		case BinXmlToken.XSD_KATMAI_DATEOFFSET:
		case BinXmlToken.XSD_KATMAI_TIME:
		case BinXmlToken.XSD_KATMAI_DATETIME:
		case BinXmlToken.XSD_KATMAI_DATE:
		case BinXmlToken.XSD_TIME:
		case BinXmlToken.XSD_DATETIME:
		case BinXmlToken.XSD_DATE:
		case BinXmlToken.XSD_BINHEX:
		case BinXmlToken.XSD_BASE64:
		case BinXmlToken.XSD_BOOLEAN:
		case BinXmlToken.XSD_DECIMAL:
		case BinXmlToken.XSD_BYTE:
		case BinXmlToken.XSD_UNSIGNEDSHORT:
		case BinXmlToken.XSD_UNSIGNEDINT:
		case BinXmlToken.XSD_UNSIGNEDLONG:
		case BinXmlToken.XSD_QNAME:
			throw ThrowNotSupported("Lists of BinaryXml value tokens not supported.");
		}
	}

	private void ImplReadElement()
	{
		if (3 != docState || 9 != docState)
		{
			switch (docState)
			{
			case 0:
				docState = 9;
				break;
			case 1:
			case 2:
				docState = 3;
				break;
			case -1:
				throw ThrowUnexpectedToken(token);
			}
		}
		elemDepth++;
		if (elemDepth == elementStack.Length)
		{
			GrowElements();
		}
		QName name = symbolTables.qnametable[ReadQNameRef()];
		qnameOther = (qnameElement = name);
		elementStack[elemDepth].Set(name, xmlspacePreserve);
		PushNamespace(name.prefix, name.namespaceUri, implied: true);
		BinXmlToken binXmlToken = PeekNextToken();
		if (BinXmlToken.Attr == binXmlToken)
		{
			ScanAttributes();
			binXmlToken = PeekNextToken();
		}
		GenerateImpliedXmlnsAttrs();
		if (BinXmlToken.EndElem == binXmlToken)
		{
			NextToken();
			isEmpty = true;
		}
		else if (BinXmlToken.SQL_NVARCHAR == binXmlToken)
		{
			if (mark < 0)
			{
				mark = pos;
			}
			pos++;
			if (ReadByte() == 0)
			{
				if (247 != ReadByte())
				{
					pos -= 3;
				}
				else
				{
					pos--;
				}
			}
			else
			{
				pos -= 2;
			}
		}
		nodetype = XmlNodeType.Element;
		valueType = TypeOfObject;
		posAfterAttrs = pos;
	}

	private void ImplReadEndElement()
	{
		if (elemDepth == 0)
		{
			throw ThrowXmlException("Unexpected end tag.");
		}
		int num = elemDepth;
		if (1 == num && 3 == docState)
		{
			docState = -1;
		}
		qnameOther = elementStack[num].name;
		xmlspacePreserve = elementStack[num].xmlspacePreserve;
		nodetype = XmlNodeType.EndElement;
	}

	private void ImplReadDoctype()
	{
		if (dtdProcessing == DtdProcessing.Prohibit)
		{
			throw ThrowXmlException("DTD is prohibited in this XML document.");
		}
		switch (docState)
		{
		case 9:
			throw ThrowXmlException("DTD is not allowed in XML fragments.");
		default:
			throw ThrowXmlException("Unexpected DTD declaration.");
		case 0:
		case 1:
			docState = 2;
			qnameOther.localname = ParseText();
			if (BinXmlToken.System == PeekToken())
			{
				pos++;
				attributes[attrCount++].Set(new QName(string.Empty, xnt.Add("SYSTEM"), string.Empty), ParseText());
			}
			if (BinXmlToken.Public == PeekToken())
			{
				pos++;
				attributes[attrCount++].Set(new QName(string.Empty, xnt.Add("PUBLIC"), string.Empty), ParseText());
			}
			if (BinXmlToken.Subset == PeekToken())
			{
				pos++;
				mark = pos;
				tokLen = ScanText(out tokDataPos);
			}
			else
			{
				tokLen = (tokDataPos = 0);
			}
			nodetype = XmlNodeType.DocumentType;
			posAfterAttrs = pos;
			break;
		}
	}

	private void ImplReadPI()
	{
		qnameOther.localname = symbolTables.symtable[ReadNameRef()];
		mark = pos;
		tokLen = ScanText(out tokDataPos);
		nodetype = XmlNodeType.ProcessingInstruction;
	}

	private void ImplReadComment()
	{
		nodetype = XmlNodeType.Comment;
		mark = pos;
		tokLen = ScanText(out tokDataPos);
	}

	private void ImplReadCDATA()
	{
		CheckAllowContent();
		nodetype = XmlNodeType.CDATA;
		mark = pos;
		tokLen = ScanText(out tokDataPos);
	}

	private void ImplReadNest()
	{
		CheckAllowContent();
		prevNameInfo = new NestedBinXml(symbolTables, docState, prevNameInfo);
		symbolTables.Init();
		docState = 0;
	}

	private void ImplReadEndNest()
	{
		NestedBinXml nestedBinXml = prevNameInfo;
		symbolTables = nestedBinXml.symbolTables;
		docState = nestedBinXml.docState;
		prevNameInfo = nestedBinXml.next;
	}

	private void ImplReadXmlText()
	{
		CheckAllowContent();
		string xmlFragment = ParseText();
		XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xnt);
		foreach (NamespaceDecl value in namespaces.Values)
		{
			if (value.scope > 0)
			{
				xmlNamespaceManager.AddNamespace(value.prefix, value.uri);
			}
		}
		XmlReaderSettings settings = Settings;
		settings.ReadOnly = false;
		settings.NameTable = xnt;
		settings.DtdProcessing = DtdProcessing.Prohibit;
		if (elemDepth != 0)
		{
			settings.ConformanceLevel = ConformanceLevel.Fragment;
		}
		settings.ReadOnly = true;
		XmlParserContext context = new XmlParserContext(xnt, xmlNamespaceManager, XmlLang, XmlSpace);
		textXmlReader = new XmlTextReaderImpl(xmlFragment, context, settings);
		if (!textXmlReader.Read() || (textXmlReader.NodeType == XmlNodeType.XmlDeclaration && !textXmlReader.Read()))
		{
			state = ScanState.Doc;
			ReadDoc();
		}
		else
		{
			state = ScanState.XmlText;
			UpdateFromTextReader();
		}
	}

	private void UpdateFromTextReader()
	{
		XmlReader xmlReader = textXmlReader;
		nodetype = xmlReader.NodeType;
		qnameOther.prefix = xmlReader.Prefix;
		qnameOther.localname = xmlReader.LocalName;
		qnameOther.namespaceUri = xmlReader.NamespaceURI;
		valueType = xmlReader.ValueType;
		isEmpty = xmlReader.IsEmptyElement;
	}

	private bool UpdateFromTextReader(bool needUpdate)
	{
		if (needUpdate)
		{
			UpdateFromTextReader();
		}
		return needUpdate;
	}

	private void CheckAllowContent()
	{
		switch (docState)
		{
		case 0:
			docState = 9;
			break;
		default:
			throw ThrowXmlException("Data at the root level is invalid.");
		case 3:
		case 9:
			break;
		}
	}

	private void GenerateTokenTypeMap()
	{
		Type[] array = new Type[256];
		array[134] = typeof(bool);
		array[7] = typeof(byte);
		array[136] = typeof(sbyte);
		array[1] = typeof(short);
		array[137] = typeof(ushort);
		array[138] = typeof(uint);
		array[3] = typeof(float);
		array[4] = typeof(double);
		array[8] = typeof(long);
		array[139] = typeof(ulong);
		array[140] = typeof(XmlQualifiedName);
		array[2] = (array[6] = typeof(int));
		array[135] = (array[11] = (array[10] = (array[5] = (array[20] = typeof(decimal)))));
		array[125] = (array[126] = (array[127] = (array[131] = (array[130] = (array[129] = (array[18] = (array[19] = typeof(DateTime))))))));
		array[122] = (array[123] = (array[124] = typeof(DateTimeOffset)));
		array[133] = (array[132] = (array[27] = (array[23] = (array[12] = (array[15] = typeof(byte[]))))));
		array[13] = TypeOfString;
		array[16] = TypeOfString;
		array[22] = TypeOfString;
		array[14] = TypeOfString;
		array[17] = TypeOfString;
		array[24] = TypeOfString;
		array[9] = TypeOfString;
		if (TokenTypeMap == null)
		{
			TokenTypeMap = array;
		}
	}

	private Type GetValueType(BinXmlToken token)
	{
		Type obj = TokenTypeMap[(int)token];
		if (obj == null)
		{
			throw ThrowUnexpectedToken(token);
		}
		return obj;
	}

	private void ReScanOverValue(BinXmlToken token)
	{
		ScanOverValue(token, attr: true, checkChars: false);
	}

	private XmlNodeType ScanOverValue(BinXmlToken token, bool attr, bool checkChars)
	{
		checked
		{
			if (token == BinXmlToken.SQL_NVARCHAR)
			{
				if (mark < 0)
				{
					mark = pos;
				}
				tokLen = ParseMB32();
				tokDataPos = pos;
				pos += tokLen * 2;
				Fill(-1);
				if (checkChars && checkCharacters)
				{
					return CheckText(attr);
				}
				if (!attr)
				{
					return CheckTextIsWS();
				}
				return XmlNodeType.Text;
			}
			return ScanOverAnyValue(token, attr, checkChars);
		}
	}

	private XmlNodeType ScanOverAnyValue(BinXmlToken token, bool attr, bool checkChars)
	{
		if (mark < 0)
		{
			mark = pos;
		}
		checked
		{
			switch (token)
			{
			case BinXmlToken.SQL_BIT:
			case BinXmlToken.SQL_TINYINT:
			case BinXmlToken.XSD_BOOLEAN:
			case BinXmlToken.XSD_BYTE:
				tokDataPos = pos;
				tokLen = 1;
				pos++;
				break;
			case BinXmlToken.SQL_SMALLINT:
			case BinXmlToken.XSD_UNSIGNEDSHORT:
				tokDataPos = pos;
				tokLen = 2;
				pos += 2;
				break;
			case BinXmlToken.SQL_INT:
			case BinXmlToken.SQL_REAL:
			case BinXmlToken.SQL_SMALLDATETIME:
			case BinXmlToken.SQL_SMALLMONEY:
			case BinXmlToken.XSD_UNSIGNEDINT:
				tokDataPos = pos;
				tokLen = 4;
				pos += 4;
				break;
			case BinXmlToken.SQL_FLOAT:
			case BinXmlToken.SQL_MONEY:
			case BinXmlToken.SQL_BIGINT:
			case BinXmlToken.SQL_DATETIME:
			case BinXmlToken.XSD_TIME:
			case BinXmlToken.XSD_DATETIME:
			case BinXmlToken.XSD_DATE:
			case BinXmlToken.XSD_UNSIGNEDLONG:
				tokDataPos = pos;
				tokLen = 8;
				pos += 8;
				break;
			case BinXmlToken.SQL_UUID:
				tokDataPos = pos;
				tokLen = 16;
				pos += 16;
				break;
			case BinXmlToken.SQL_DECIMAL:
			case BinXmlToken.SQL_NUMERIC:
			case BinXmlToken.XSD_DECIMAL:
				tokDataPos = pos;
				tokLen = ParseMB64();
				pos += tokLen;
				break;
			case BinXmlToken.SQL_BINARY:
			case BinXmlToken.SQL_VARBINARY:
			case BinXmlToken.SQL_IMAGE:
			case BinXmlToken.SQL_UDT:
			case BinXmlToken.XSD_BINHEX:
			case BinXmlToken.XSD_BASE64:
				tokLen = ParseMB64();
				tokDataPos = pos;
				pos += tokLen;
				break;
			case BinXmlToken.SQL_CHAR:
			case BinXmlToken.SQL_VARCHAR:
			case BinXmlToken.SQL_TEXT:
				tokLen = ParseMB64();
				tokDataPos = pos;
				pos += tokLen;
				if (checkChars && checkCharacters)
				{
					Fill(-1);
					string text = ValueAsString(token);
					XmlConvert.VerifyCharData(text, ExceptionType.ArgumentException, ExceptionType.XmlException);
					stringValue = text;
				}
				break;
			case BinXmlToken.SQL_NCHAR:
			case BinXmlToken.SQL_NVARCHAR:
			case BinXmlToken.SQL_NTEXT:
				return ScanOverValue(BinXmlToken.SQL_NVARCHAR, attr, checkChars);
			case BinXmlToken.XSD_QNAME:
				tokDataPos = pos;
				ParseMB32();
				break;
			case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
			case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
			case BinXmlToken.XSD_KATMAI_DATEOFFSET:
			case BinXmlToken.XSD_KATMAI_TIME:
			case BinXmlToken.XSD_KATMAI_DATETIME:
			case BinXmlToken.XSD_KATMAI_DATE:
				VerifyVersion(2, token);
				tokDataPos = pos;
				tokLen = GetXsdKatmaiTokenLength(token);
				pos += tokLen;
				break;
			default:
				throw ThrowUnexpectedToken(token);
			}
			Fill(-1);
			return XmlNodeType.Text;
		}
	}

	private unsafe XmlNodeType CheckText(bool attr)
	{
		XmlCharType xmlCharType = this.xmlCharType;
		fixed (byte* ptr = data)
		{
			int num = pos;
			int num2 = tokDataPos;
			if (!attr)
			{
				while (true)
				{
					int num3 = num2 + 2;
					if (num3 > num)
					{
						if (!xmlspacePreserve)
						{
							return XmlNodeType.Whitespace;
						}
						return XmlNodeType.SignificantWhitespace;
					}
					if (ptr[num2 + 1] != 0 || (xmlCharType.charProperties[ptr[num2]] & 1) == 0)
					{
						break;
					}
					num2 = num3;
				}
			}
			char c;
			char c2;
			while (true)
			{
				int num4 = num2 + 2;
				if (num4 > num)
				{
					return XmlNodeType.Text;
				}
				c = (char)(ptr[num2] | (ptr[num2 + 1] << 8));
				if ((xmlCharType.charProperties[(uint)c] & 0x10) != 0)
				{
					num2 = num4;
					continue;
				}
				if (!XmlCharType.IsHighSurrogate(c))
				{
					throw XmlConvert.CreateInvalidCharException(c, '\0', ExceptionType.XmlException);
				}
				if (num2 + 4 > num)
				{
					throw ThrowXmlException("The surrogate pair is invalid. Missing a low surrogate character.");
				}
				c2 = (char)(ptr[num2 + 2] | (ptr[num2 + 3] << 8));
				if (!XmlCharType.IsLowSurrogate(c2))
				{
					break;
				}
				num2 += 4;
			}
			throw XmlConvert.CreateInvalidSurrogatePairException(c, c2);
		}
	}

	private XmlNodeType CheckTextIsWS()
	{
		byte[] array = data;
		int num = tokDataPos;
		while (true)
		{
			if (num < pos)
			{
				if (array[num + 1] != 0)
				{
					break;
				}
				byte b = array[num];
				if ((uint)(b - 9) > 1u && b != 13 && b != 32)
				{
					break;
				}
				num += 2;
				continue;
			}
			if (xmlspacePreserve)
			{
				return XmlNodeType.SignificantWhitespace;
			}
			return XmlNodeType.Whitespace;
		}
		return XmlNodeType.Text;
	}

	private void CheckValueTokenBounds()
	{
		if (end - tokDataPos < tokLen)
		{
			throw ThrowXmlException("Unexpected end of file has occurred.");
		}
	}

	private int GetXsdKatmaiTokenLength(BinXmlToken token)
	{
		switch (token)
		{
		case BinXmlToken.XSD_KATMAI_DATE:
			return 3;
		case BinXmlToken.XSD_KATMAI_TIME:
		case BinXmlToken.XSD_KATMAI_DATETIME:
		{
			Fill(0);
			byte scale = data[pos];
			return 4 + XsdKatmaiTimeScaleToValueLength(scale);
		}
		case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
		case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
		case BinXmlToken.XSD_KATMAI_DATEOFFSET:
		{
			Fill(0);
			byte scale = data[pos];
			return 6 + XsdKatmaiTimeScaleToValueLength(scale);
		}
		default:
			throw ThrowUnexpectedToken(this.token);
		}
	}

	private int XsdKatmaiTimeScaleToValueLength(byte scale)
	{
		if (scale > 7)
		{
			throw new XmlException("Arithmetic Overflow.", (string)null);
		}
		return XsdKatmaiTimeScaleToValueLengthMap[scale];
	}

	private long ValueAsLong()
	{
		CheckValueTokenBounds();
		switch (token)
		{
		case BinXmlToken.SQL_BIT:
		case BinXmlToken.SQL_TINYINT:
			return data[tokDataPos];
		case BinXmlToken.XSD_BYTE:
			return (sbyte)data[tokDataPos];
		case BinXmlToken.SQL_SMALLINT:
			return GetInt16(tokDataPos);
		case BinXmlToken.SQL_INT:
			return GetInt32(tokDataPos);
		case BinXmlToken.SQL_BIGINT:
			return GetInt64(tokDataPos);
		case BinXmlToken.XSD_UNSIGNEDSHORT:
			return GetUInt16(tokDataPos);
		case BinXmlToken.XSD_UNSIGNEDINT:
			return GetUInt32(tokDataPos);
		case BinXmlToken.XSD_UNSIGNEDLONG:
			return checked((long)GetUInt64(tokDataPos));
		case BinXmlToken.SQL_REAL:
		case BinXmlToken.SQL_FLOAT:
			return (long)ValueAsDouble();
		case BinXmlToken.SQL_MONEY:
		case BinXmlToken.SQL_DECIMAL:
		case BinXmlToken.SQL_NUMERIC:
		case BinXmlToken.SQL_SMALLMONEY:
		case BinXmlToken.XSD_DECIMAL:
			return (long)ValueAsDecimal();
		default:
			throw ThrowUnexpectedToken(token);
		}
	}

	private ulong ValueAsULong()
	{
		if (BinXmlToken.XSD_UNSIGNEDLONG == token)
		{
			CheckValueTokenBounds();
			return GetUInt64(tokDataPos);
		}
		throw ThrowUnexpectedToken(token);
	}

	private decimal ValueAsDecimal()
	{
		CheckValueTokenBounds();
		switch (token)
		{
		case BinXmlToken.SQL_SMALLINT:
		case BinXmlToken.SQL_INT:
		case BinXmlToken.SQL_BIT:
		case BinXmlToken.SQL_TINYINT:
		case BinXmlToken.SQL_BIGINT:
		case BinXmlToken.XSD_BYTE:
		case BinXmlToken.XSD_UNSIGNEDSHORT:
		case BinXmlToken.XSD_UNSIGNEDINT:
			return new decimal(ValueAsLong());
		case BinXmlToken.XSD_UNSIGNEDLONG:
			return new decimal(ValueAsULong());
		case BinXmlToken.SQL_REAL:
			return new decimal(GetSingle(tokDataPos));
		case BinXmlToken.SQL_FLOAT:
			return new decimal(GetDouble(tokDataPos));
		case BinXmlToken.SQL_SMALLMONEY:
			return new BinXmlSqlMoney(GetInt32(tokDataPos)).ToDecimal();
		case BinXmlToken.SQL_MONEY:
			return new BinXmlSqlMoney(GetInt64(tokDataPos)).ToDecimal();
		case BinXmlToken.SQL_DECIMAL:
		case BinXmlToken.SQL_NUMERIC:
		case BinXmlToken.XSD_DECIMAL:
			return new BinXmlSqlDecimal(data, tokDataPos, token == BinXmlToken.XSD_DECIMAL).ToDecimal();
		default:
			throw ThrowUnexpectedToken(token);
		}
	}

	private double ValueAsDouble()
	{
		CheckValueTokenBounds();
		switch (token)
		{
		case BinXmlToken.SQL_SMALLINT:
		case BinXmlToken.SQL_INT:
		case BinXmlToken.SQL_BIT:
		case BinXmlToken.SQL_TINYINT:
		case BinXmlToken.SQL_BIGINT:
		case BinXmlToken.XSD_BYTE:
		case BinXmlToken.XSD_UNSIGNEDSHORT:
		case BinXmlToken.XSD_UNSIGNEDINT:
			return ValueAsLong();
		case BinXmlToken.XSD_UNSIGNEDLONG:
			return ValueAsULong();
		case BinXmlToken.SQL_REAL:
			return GetSingle(tokDataPos);
		case BinXmlToken.SQL_FLOAT:
			return GetDouble(tokDataPos);
		case BinXmlToken.SQL_MONEY:
		case BinXmlToken.SQL_DECIMAL:
		case BinXmlToken.SQL_NUMERIC:
		case BinXmlToken.SQL_SMALLMONEY:
		case BinXmlToken.XSD_DECIMAL:
			return (double)ValueAsDecimal();
		default:
			throw ThrowUnexpectedToken(token);
		}
	}

	private DateTime ValueAsDateTime()
	{
		CheckValueTokenBounds();
		switch (token)
		{
		case BinXmlToken.SQL_DATETIME:
		{
			int num2 = tokDataPos;
			int int2 = GetInt32(num2);
			uint uInt2 = GetUInt32(num2 + 4);
			return BinXmlDateTime.SqlDateTimeToDateTime(int2, uInt2);
		}
		case BinXmlToken.SQL_SMALLDATETIME:
		{
			int num = tokDataPos;
			short @int = GetInt16(num);
			ushort uInt = GetUInt16(num + 2);
			return BinXmlDateTime.SqlSmallDateTimeToDateTime(@int, uInt);
		}
		case BinXmlToken.XSD_TIME:
			return BinXmlDateTime.XsdTimeToDateTime(GetInt64(tokDataPos));
		case BinXmlToken.XSD_DATE:
			return BinXmlDateTime.XsdDateToDateTime(GetInt64(tokDataPos));
		case BinXmlToken.XSD_DATETIME:
			return BinXmlDateTime.XsdDateTimeToDateTime(GetInt64(tokDataPos));
		case BinXmlToken.XSD_KATMAI_DATE:
			return BinXmlDateTime.XsdKatmaiDateToDateTime(data, tokDataPos);
		case BinXmlToken.XSD_KATMAI_DATETIME:
			return BinXmlDateTime.XsdKatmaiDateTimeToDateTime(data, tokDataPos);
		case BinXmlToken.XSD_KATMAI_TIME:
			return BinXmlDateTime.XsdKatmaiTimeToDateTime(data, tokDataPos);
		case BinXmlToken.XSD_KATMAI_DATEOFFSET:
			return BinXmlDateTime.XsdKatmaiDateOffsetToDateTime(data, tokDataPos);
		case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
			return BinXmlDateTime.XsdKatmaiDateTimeOffsetToDateTime(data, tokDataPos);
		case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
			return BinXmlDateTime.XsdKatmaiTimeOffsetToDateTime(data, tokDataPos);
		default:
			throw ThrowUnexpectedToken(token);
		}
	}

	private DateTimeOffset ValueAsDateTimeOffset()
	{
		CheckValueTokenBounds();
		return token switch
		{
			BinXmlToken.XSD_KATMAI_DATEOFFSET => BinXmlDateTime.XsdKatmaiDateOffsetToDateTimeOffset(data, tokDataPos), 
			BinXmlToken.XSD_KATMAI_DATETIMEOFFSET => BinXmlDateTime.XsdKatmaiDateTimeOffsetToDateTimeOffset(data, tokDataPos), 
			BinXmlToken.XSD_KATMAI_TIMEOFFSET => BinXmlDateTime.XsdKatmaiTimeOffsetToDateTimeOffset(data, tokDataPos), 
			_ => throw ThrowUnexpectedToken(token), 
		};
	}

	private string ValueAsDateTimeString()
	{
		CheckValueTokenBounds();
		switch (token)
		{
		case BinXmlToken.SQL_DATETIME:
		{
			int num2 = tokDataPos;
			int int2 = GetInt32(num2);
			uint uInt2 = GetUInt32(num2 + 4);
			return BinXmlDateTime.SqlDateTimeToString(int2, uInt2);
		}
		case BinXmlToken.SQL_SMALLDATETIME:
		{
			int num = tokDataPos;
			short @int = GetInt16(num);
			ushort uInt = GetUInt16(num + 2);
			return BinXmlDateTime.SqlSmallDateTimeToString(@int, uInt);
		}
		case BinXmlToken.XSD_TIME:
			return BinXmlDateTime.XsdTimeToString(GetInt64(tokDataPos));
		case BinXmlToken.XSD_DATE:
			return BinXmlDateTime.XsdDateToString(GetInt64(tokDataPos));
		case BinXmlToken.XSD_DATETIME:
			return BinXmlDateTime.XsdDateTimeToString(GetInt64(tokDataPos));
		case BinXmlToken.XSD_KATMAI_DATE:
			return BinXmlDateTime.XsdKatmaiDateToString(data, tokDataPos);
		case BinXmlToken.XSD_KATMAI_DATETIME:
			return BinXmlDateTime.XsdKatmaiDateTimeToString(data, tokDataPos);
		case BinXmlToken.XSD_KATMAI_TIME:
			return BinXmlDateTime.XsdKatmaiTimeToString(data, tokDataPos);
		case BinXmlToken.XSD_KATMAI_DATEOFFSET:
			return BinXmlDateTime.XsdKatmaiDateOffsetToString(data, tokDataPos);
		case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
			return BinXmlDateTime.XsdKatmaiDateTimeOffsetToString(data, tokDataPos);
		case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
			return BinXmlDateTime.XsdKatmaiTimeOffsetToString(data, tokDataPos);
		default:
			throw ThrowUnexpectedToken(token);
		}
	}

	private string ValueAsString(BinXmlToken token)
	{
		try
		{
			CheckValueTokenBounds();
			switch (token)
			{
			case BinXmlToken.SQL_NCHAR:
			case BinXmlToken.SQL_NVARCHAR:
			case BinXmlToken.SQL_NTEXT:
				return GetString(tokDataPos, tokLen);
			case BinXmlToken.XSD_BOOLEAN:
				if (data[tokDataPos] == 0)
				{
					return "false";
				}
				return "true";
			case BinXmlToken.SQL_SMALLINT:
			case BinXmlToken.SQL_INT:
			case BinXmlToken.SQL_BIT:
			case BinXmlToken.SQL_TINYINT:
			case BinXmlToken.SQL_BIGINT:
			case BinXmlToken.XSD_BYTE:
			case BinXmlToken.XSD_UNSIGNEDSHORT:
			case BinXmlToken.XSD_UNSIGNEDINT:
				return ValueAsLong().ToString(CultureInfo.InvariantCulture);
			case BinXmlToken.XSD_UNSIGNEDLONG:
				return ValueAsULong().ToString(CultureInfo.InvariantCulture);
			case BinXmlToken.SQL_REAL:
				return XmlConvert.ToString(GetSingle(tokDataPos));
			case BinXmlToken.SQL_FLOAT:
				return XmlConvert.ToString(GetDouble(tokDataPos));
			case BinXmlToken.SQL_UUID:
			{
				int num3 = tokDataPos;
				int @int = GetInt32(num3);
				short int2 = GetInt16(num3 + 4);
				short int3 = GetInt16(num3 + 6);
				return new Guid(@int, int2, int3, data[num3 + 8], data[num3 + 9], data[num3 + 10], data[num3 + 11], data[num3 + 12], data[num3 + 13], data[num3 + 14], data[num3 + 15]).ToString();
			}
			case BinXmlToken.SQL_SMALLMONEY:
				return new BinXmlSqlMoney(GetInt32(tokDataPos)).ToString();
			case BinXmlToken.SQL_MONEY:
				return new BinXmlSqlMoney(GetInt64(tokDataPos)).ToString();
			case BinXmlToken.SQL_DECIMAL:
			case BinXmlToken.SQL_NUMERIC:
			case BinXmlToken.XSD_DECIMAL:
				return new BinXmlSqlDecimal(data, tokDataPos, token == BinXmlToken.XSD_DECIMAL).ToString();
			case BinXmlToken.SQL_CHAR:
			case BinXmlToken.SQL_VARCHAR:
			case BinXmlToken.SQL_TEXT:
			{
				int num2 = tokDataPos;
				return Encoding.GetEncoding(GetInt32(num2)).GetString(data, num2 + 4, tokLen - 4);
			}
			case BinXmlToken.SQL_BINARY:
			case BinXmlToken.SQL_VARBINARY:
			case BinXmlToken.SQL_IMAGE:
			case BinXmlToken.SQL_UDT:
			case BinXmlToken.XSD_BASE64:
				return Convert.ToBase64String(data, tokDataPos, tokLen);
			case BinXmlToken.XSD_BINHEX:
				return BinHexEncoder.Encode(data, tokDataPos, tokLen);
			case BinXmlToken.SQL_DATETIME:
			case BinXmlToken.SQL_SMALLDATETIME:
			case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
			case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
			case BinXmlToken.XSD_KATMAI_DATEOFFSET:
			case BinXmlToken.XSD_KATMAI_TIME:
			case BinXmlToken.XSD_KATMAI_DATETIME:
			case BinXmlToken.XSD_KATMAI_DATE:
			case BinXmlToken.XSD_TIME:
			case BinXmlToken.XSD_DATETIME:
			case BinXmlToken.XSD_DATE:
				return ValueAsDateTimeString();
			case BinXmlToken.XSD_QNAME:
			{
				int num = ParseMB32(tokDataPos);
				if (num < 0 || num >= symbolTables.qnameCount)
				{
					throw new XmlException("Invalid QName ID.", string.Empty);
				}
				QName qName = symbolTables.qnametable[num];
				if (qName.prefix.Length == 0)
				{
					return qName.localname;
				}
				return qName.prefix + ":" + qName.localname;
			}
			default:
				throw ThrowUnexpectedToken(this.token);
			}
		}
		catch
		{
			state = ScanState.Error;
			throw;
		}
	}

	private object ValueAsObject(BinXmlToken token, bool returnInternalTypes)
	{
		CheckValueTokenBounds();
		switch (token)
		{
		case BinXmlToken.SQL_NCHAR:
		case BinXmlToken.SQL_NVARCHAR:
		case BinXmlToken.SQL_NTEXT:
			return GetString(tokDataPos, tokLen);
		case BinXmlToken.XSD_BOOLEAN:
			return data[tokDataPos] != 0;
		case BinXmlToken.SQL_BIT:
			return (int)data[tokDataPos];
		case BinXmlToken.SQL_TINYINT:
			return data[tokDataPos];
		case BinXmlToken.SQL_SMALLINT:
			return GetInt16(tokDataPos);
		case BinXmlToken.SQL_INT:
			return GetInt32(tokDataPos);
		case BinXmlToken.SQL_BIGINT:
			return GetInt64(tokDataPos);
		case BinXmlToken.XSD_BYTE:
			return (sbyte)data[tokDataPos];
		case BinXmlToken.XSD_UNSIGNEDSHORT:
			return GetUInt16(tokDataPos);
		case BinXmlToken.XSD_UNSIGNEDINT:
			return GetUInt32(tokDataPos);
		case BinXmlToken.XSD_UNSIGNEDLONG:
			return GetUInt64(tokDataPos);
		case BinXmlToken.SQL_REAL:
			return GetSingle(tokDataPos);
		case BinXmlToken.SQL_FLOAT:
			return GetDouble(tokDataPos);
		case BinXmlToken.SQL_UUID:
		{
			int num3 = tokDataPos;
			int @int = GetInt32(num3);
			short int2 = GetInt16(num3 + 4);
			short int3 = GetInt16(num3 + 6);
			return new Guid(@int, int2, int3, data[num3 + 8], data[num3 + 9], data[num3 + 10], data[num3 + 11], data[num3 + 12], data[num3 + 13], data[num3 + 14], data[num3 + 15]).ToString();
		}
		case BinXmlToken.SQL_SMALLMONEY:
		{
			BinXmlSqlMoney binXmlSqlMoney2 = new BinXmlSqlMoney(GetInt32(tokDataPos));
			if (returnInternalTypes)
			{
				return binXmlSqlMoney2;
			}
			return binXmlSqlMoney2.ToDecimal();
		}
		case BinXmlToken.SQL_MONEY:
		{
			BinXmlSqlMoney binXmlSqlMoney = new BinXmlSqlMoney(GetInt64(tokDataPos));
			if (returnInternalTypes)
			{
				return binXmlSqlMoney;
			}
			return binXmlSqlMoney.ToDecimal();
		}
		case BinXmlToken.SQL_DECIMAL:
		case BinXmlToken.SQL_NUMERIC:
		case BinXmlToken.XSD_DECIMAL:
		{
			BinXmlSqlDecimal binXmlSqlDecimal = new BinXmlSqlDecimal(data, tokDataPos, token == BinXmlToken.XSD_DECIMAL);
			if (returnInternalTypes)
			{
				return binXmlSqlDecimal;
			}
			return binXmlSqlDecimal.ToDecimal();
		}
		case BinXmlToken.SQL_CHAR:
		case BinXmlToken.SQL_VARCHAR:
		case BinXmlToken.SQL_TEXT:
		{
			int num2 = tokDataPos;
			return Encoding.GetEncoding(GetInt32(num2)).GetString(data, num2 + 4, tokLen - 4);
		}
		case BinXmlToken.SQL_BINARY:
		case BinXmlToken.SQL_VARBINARY:
		case BinXmlToken.SQL_IMAGE:
		case BinXmlToken.SQL_UDT:
		case BinXmlToken.XSD_BINHEX:
		case BinXmlToken.XSD_BASE64:
		{
			byte[] array = new byte[tokLen];
			Array.Copy(data, tokDataPos, array, 0, tokLen);
			return array;
		}
		case BinXmlToken.SQL_DATETIME:
		case BinXmlToken.SQL_SMALLDATETIME:
		case BinXmlToken.XSD_KATMAI_TIME:
		case BinXmlToken.XSD_KATMAI_DATETIME:
		case BinXmlToken.XSD_KATMAI_DATE:
		case BinXmlToken.XSD_TIME:
		case BinXmlToken.XSD_DATETIME:
		case BinXmlToken.XSD_DATE:
			return ValueAsDateTime();
		case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
		case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
		case BinXmlToken.XSD_KATMAI_DATEOFFSET:
			return ValueAsDateTimeOffset();
		case BinXmlToken.XSD_QNAME:
		{
			int num = ParseMB32(tokDataPos);
			if (num < 0 || num >= symbolTables.qnameCount)
			{
				throw new XmlException("Invalid QName ID.", string.Empty);
			}
			QName qName = symbolTables.qnametable[num];
			return new XmlQualifiedName(qName.localname, qName.namespaceUri);
		}
		default:
			throw ThrowUnexpectedToken(this.token);
		}
	}

	private XmlValueConverter GetValueConverter(XmlTypeCode typeCode)
	{
		return DatatypeImplementation.GetSimpleTypeFromTypeCode(typeCode).ValueConverter;
	}

	private object ValueAs(BinXmlToken token, Type returnType, IXmlNamespaceResolver namespaceResolver)
	{
		CheckValueTokenBounds();
		switch (token)
		{
		case BinXmlToken.SQL_NCHAR:
		case BinXmlToken.SQL_NVARCHAR:
		case BinXmlToken.SQL_NTEXT:
			return GetValueConverter(XmlTypeCode.UntypedAtomic).ChangeType(GetString(tokDataPos, tokLen), returnType, namespaceResolver);
		case BinXmlToken.XSD_BOOLEAN:
			return GetValueConverter(XmlTypeCode.Boolean).ChangeType(data[tokDataPos] != 0, returnType, namespaceResolver);
		case BinXmlToken.SQL_BIT:
			return GetValueConverter(XmlTypeCode.NonNegativeInteger).ChangeType((int)data[tokDataPos], returnType, namespaceResolver);
		case BinXmlToken.SQL_TINYINT:
			return GetValueConverter(XmlTypeCode.UnsignedByte).ChangeType(data[tokDataPos], returnType, namespaceResolver);
		case BinXmlToken.SQL_SMALLINT:
		{
			int @int = GetInt16(tokDataPos);
			return GetValueConverter(XmlTypeCode.Short).ChangeType(@int, returnType, namespaceResolver);
		}
		case BinXmlToken.SQL_INT:
		{
			int int2 = GetInt32(tokDataPos);
			return GetValueConverter(XmlTypeCode.Int).ChangeType(int2, returnType, namespaceResolver);
		}
		case BinXmlToken.SQL_BIGINT:
		{
			long int3 = GetInt64(tokDataPos);
			return GetValueConverter(XmlTypeCode.Long).ChangeType(int3, returnType, namespaceResolver);
		}
		case BinXmlToken.XSD_BYTE:
			return GetValueConverter(XmlTypeCode.Byte).ChangeType((int)(sbyte)data[tokDataPos], returnType, namespaceResolver);
		case BinXmlToken.XSD_UNSIGNEDSHORT:
		{
			int uInt = GetUInt16(tokDataPos);
			return GetValueConverter(XmlTypeCode.UnsignedShort).ChangeType(uInt, returnType, namespaceResolver);
		}
		case BinXmlToken.XSD_UNSIGNEDINT:
		{
			long num5 = GetUInt32(tokDataPos);
			return GetValueConverter(XmlTypeCode.UnsignedInt).ChangeType(num5, returnType, namespaceResolver);
		}
		case BinXmlToken.XSD_UNSIGNEDLONG:
		{
			decimal num4 = GetUInt64(tokDataPos);
			return GetValueConverter(XmlTypeCode.UnsignedLong).ChangeType(num4, returnType, namespaceResolver);
		}
		case BinXmlToken.SQL_REAL:
		{
			float single = GetSingle(tokDataPos);
			return GetValueConverter(XmlTypeCode.Float).ChangeType(single, returnType, namespaceResolver);
		}
		case BinXmlToken.SQL_FLOAT:
		{
			double num3 = GetDouble(tokDataPos);
			return GetValueConverter(XmlTypeCode.Double).ChangeType(num3, returnType, namespaceResolver);
		}
		case BinXmlToken.SQL_UUID:
			return GetValueConverter(XmlTypeCode.String).ChangeType(ValueAsString(token), returnType, namespaceResolver);
		case BinXmlToken.SQL_SMALLMONEY:
			return GetValueConverter(XmlTypeCode.Decimal).ChangeType(new BinXmlSqlMoney(GetInt32(tokDataPos)).ToDecimal(), returnType, namespaceResolver);
		case BinXmlToken.SQL_MONEY:
			return GetValueConverter(XmlTypeCode.Decimal).ChangeType(new BinXmlSqlMoney(GetInt64(tokDataPos)).ToDecimal(), returnType, namespaceResolver);
		case BinXmlToken.SQL_DECIMAL:
		case BinXmlToken.SQL_NUMERIC:
		case BinXmlToken.XSD_DECIMAL:
			return GetValueConverter(XmlTypeCode.Decimal).ChangeType(new BinXmlSqlDecimal(data, tokDataPos, token == BinXmlToken.XSD_DECIMAL).ToDecimal(), returnType, namespaceResolver);
		case BinXmlToken.SQL_CHAR:
		case BinXmlToken.SQL_VARCHAR:
		case BinXmlToken.SQL_TEXT:
		{
			int num2 = tokDataPos;
			Encoding encoding = Encoding.GetEncoding(GetInt32(num2));
			return GetValueConverter(XmlTypeCode.UntypedAtomic).ChangeType(encoding.GetString(data, num2 + 4, tokLen - 4), returnType, namespaceResolver);
		}
		case BinXmlToken.SQL_BINARY:
		case BinXmlToken.SQL_VARBINARY:
		case BinXmlToken.SQL_IMAGE:
		case BinXmlToken.SQL_UDT:
		case BinXmlToken.XSD_BINHEX:
		case BinXmlToken.XSD_BASE64:
		{
			byte[] array = new byte[tokLen];
			Array.Copy(data, tokDataPos, array, 0, tokLen);
			return GetValueConverter((token == BinXmlToken.XSD_BINHEX) ? XmlTypeCode.HexBinary : XmlTypeCode.Base64Binary).ChangeType(array, returnType, namespaceResolver);
		}
		case BinXmlToken.SQL_DATETIME:
		case BinXmlToken.SQL_SMALLDATETIME:
		case BinXmlToken.XSD_KATMAI_TIME:
		case BinXmlToken.XSD_KATMAI_DATETIME:
		case BinXmlToken.XSD_KATMAI_DATE:
		case BinXmlToken.XSD_DATETIME:
			return GetValueConverter(XmlTypeCode.DateTime).ChangeType(ValueAsDateTime(), returnType, namespaceResolver);
		case BinXmlToken.XSD_KATMAI_TIMEOFFSET:
		case BinXmlToken.XSD_KATMAI_DATETIMEOFFSET:
		case BinXmlToken.XSD_KATMAI_DATEOFFSET:
			return GetValueConverter(XmlTypeCode.DateTime).ChangeType(ValueAsDateTimeOffset(), returnType, namespaceResolver);
		case BinXmlToken.XSD_TIME:
			return GetValueConverter(XmlTypeCode.Time).ChangeType(ValueAsDateTime(), returnType, namespaceResolver);
		case BinXmlToken.XSD_DATE:
			return GetValueConverter(XmlTypeCode.Date).ChangeType(ValueAsDateTime(), returnType, namespaceResolver);
		case BinXmlToken.XSD_QNAME:
		{
			int num = ParseMB32(tokDataPos);
			if (num < 0 || num >= symbolTables.qnameCount)
			{
				throw new XmlException("Invalid QName ID.", string.Empty);
			}
			QName qName = symbolTables.qnametable[num];
			return GetValueConverter(XmlTypeCode.QName).ChangeType(new XmlQualifiedName(qName.localname, qName.namespaceUri), returnType, namespaceResolver);
		}
		default:
			throw ThrowUnexpectedToken(this.token);
		}
	}

	private short GetInt16(int pos)
	{
		byte[] array = data;
		return (short)(array[pos] | (array[pos + 1] << 8));
	}

	private ushort GetUInt16(int pos)
	{
		byte[] array = data;
		return (ushort)(array[pos] | (array[pos + 1] << 8));
	}

	private int GetInt32(int pos)
	{
		byte[] array = data;
		return array[pos] | (array[pos + 1] << 8) | (array[pos + 2] << 16) | (array[pos + 3] << 24);
	}

	private uint GetUInt32(int pos)
	{
		byte[] array = data;
		return (uint)(array[pos] | (array[pos + 1] << 8) | (array[pos + 2] << 16) | (array[pos + 3] << 24));
	}

	private long GetInt64(int pos)
	{
		byte[] array = data;
		uint num = (uint)(array[pos] | (array[pos + 1] << 8) | (array[pos + 2] << 16) | (array[pos + 3] << 24));
		return (long)(((ulong)(uint)(array[pos + 4] | (array[pos + 5] << 8) | (array[pos + 6] << 16) | (array[pos + 7] << 24)) << 32) | num);
	}

	private ulong GetUInt64(int pos)
	{
		byte[] array = data;
		uint num = (uint)(array[pos] | (array[pos + 1] << 8) | (array[pos + 2] << 16) | (array[pos + 3] << 24));
		return ((ulong)(uint)(array[pos + 4] | (array[pos + 5] << 8) | (array[pos + 6] << 16) | (array[pos + 7] << 24)) << 32) | num;
	}

	private unsafe float GetSingle(int offset)
	{
		byte[] array = data;
		uint num = (uint)(array[offset] | (array[offset + 1] << 8) | (array[offset + 2] << 16) | (array[offset + 3] << 24));
		return *(float*)(&num);
	}

	private unsafe double GetDouble(int offset)
	{
		uint num = (uint)(data[offset] | (data[offset + 1] << 8) | (data[offset + 2] << 16) | (data[offset + 3] << 24));
		ulong num2 = ((ulong)(uint)(data[offset + 4] | (data[offset + 5] << 8) | (data[offset + 6] << 16) | (data[offset + 7] << 24)) << 32) | num;
		return *(double*)(&num2);
	}

	private Exception ThrowUnexpectedToken(BinXmlToken token)
	{
		return ThrowXmlException("Unexpected BinaryXml token.");
	}

	private Exception ThrowXmlException(string res)
	{
		state = ScanState.Error;
		return new XmlException(res, (string[])null);
	}

	private Exception ThrowXmlException(string res, string arg1, string arg2)
	{
		state = ScanState.Error;
		return new XmlException(res, new string[2] { arg1, arg2 });
	}

	private Exception ThrowNotSupported(string res)
	{
		state = ScanState.Error;
		return new NotSupportedException(Res.GetString(res));
	}

	public override Task<string> GetValueAsync()
	{
		throw new NotSupportedException();
	}

	public override Task<bool> ReadAsync()
	{
		throw new NotSupportedException();
	}

	public override Task<object> ReadContentAsObjectAsync()
	{
		throw new NotSupportedException();
	}

	public override Task<object> ReadContentAsAsync(Type returnType, IXmlNamespaceResolver namespaceResolver)
	{
		throw new NotSupportedException();
	}

	public override Task<XmlNodeType> MoveToContentAsync()
	{
		throw new NotSupportedException();
	}

	public override Task<string> ReadContentAsStringAsync()
	{
		throw new NotSupportedException();
	}

	public override Task<int> ReadContentAsBase64Async(byte[] buffer, int index, int count)
	{
		throw new NotSupportedException();
	}

	public override Task<object> ReadElementContentAsAsync(Type returnType, IXmlNamespaceResolver namespaceResolver)
	{
		throw new NotSupportedException();
	}

	public override Task<object> ReadElementContentAsObjectAsync()
	{
		throw new NotSupportedException();
	}

	public override Task<int> ReadElementContentAsBinHexAsync(byte[] buffer, int index, int count)
	{
		throw new NotSupportedException();
	}

	public override Task<string> ReadInnerXmlAsync()
	{
		throw new NotSupportedException();
	}

	public override Task<string> ReadOuterXmlAsync()
	{
		throw new NotSupportedException();
	}

	public override Task<int> ReadValueChunkAsync(char[] buffer, int index, int count)
	{
		throw new NotSupportedException();
	}

	public override Task SkipAsync()
	{
		throw new NotSupportedException();
	}

	public override Task<string> ReadElementContentAsStringAsync()
	{
		throw new NotSupportedException();
	}
}
