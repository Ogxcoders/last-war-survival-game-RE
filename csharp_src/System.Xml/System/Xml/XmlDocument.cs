using System.Collections;
using System.Globalization;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml;

public class XmlDocument : XmlNode
{
	private XmlImplementation implementation;

	private DomNameTable domNameTable;

	private XmlLinkedNode lastChild;

	private XmlNamedNodeMap entities;

	private Hashtable htElementIdMap;

	private Hashtable htElementIDAttrDecl;

	private SchemaInfo schemaInfo;

	private XmlSchemaSet schemas;

	private bool reportValidity;

	private bool actualLoadingStatus;

	private XmlNodeChangedEventHandler onNodeInsertingDelegate;

	private XmlNodeChangedEventHandler onNodeInsertedDelegate;

	private XmlNodeChangedEventHandler onNodeRemovingDelegate;

	private XmlNodeChangedEventHandler onNodeRemovedDelegate;

	private XmlNodeChangedEventHandler onNodeChangingDelegate;

	private XmlNodeChangedEventHandler onNodeChangedDelegate;

	internal bool fEntRefNodesPresent;

	internal bool fCDataNodesPresent;

	private bool preserveWhitespace;

	private bool isLoading;

	internal string strDocumentName;

	internal string strDocumentFragmentName;

	internal string strCommentName;

	internal string strTextName;

	internal string strCDataSectionName;

	internal string strEntityName;

	internal string strID;

	internal string strXmlns;

	internal string strXml;

	internal string strSpace;

	internal string strLang;

	internal string strEmpty;

	internal string strNonSignificantWhitespaceName;

	internal string strSignificantWhitespaceName;

	internal string strReservedXmlns;

	internal string strReservedXml;

	internal string baseURI;

	private XmlResolver resolver;

	internal bool bSetResolver;

	internal object objLock;

	private XmlAttribute namespaceXml;

	internal static EmptyEnumerator EmptyEnumerator = new EmptyEnumerator();

	internal static IXmlSchemaInfo NotKnownSchemaInfo = new XmlSchemaInfo(XmlSchemaValidity.NotKnown);

	internal static IXmlSchemaInfo ValidSchemaInfo = new XmlSchemaInfo(XmlSchemaValidity.Valid);

	internal static IXmlSchemaInfo InvalidSchemaInfo = new XmlSchemaInfo(XmlSchemaValidity.Invalid);

	internal SchemaInfo DtdSchemaInfo
	{
		get
		{
			return schemaInfo;
		}
		set
		{
			schemaInfo = value;
		}
	}

	public override XmlNodeType NodeType => XmlNodeType.Document;

	public override XmlNode ParentNode => null;

	public virtual XmlDocumentType DocumentType => (XmlDocumentType)FindChild(XmlNodeType.DocumentType);

	internal virtual XmlDeclaration Declaration
	{
		get
		{
			if (HasChildNodes)
			{
				return FirstChild as XmlDeclaration;
			}
			return null;
		}
	}

	public XmlImplementation Implementation => implementation;

	public override string Name => strDocumentName;

	public override string LocalName => strDocumentName;

	public XmlElement DocumentElement => (XmlElement)FindChild(XmlNodeType.Element);

	internal override bool IsContainer => true;

	internal override XmlLinkedNode LastNode
	{
		get
		{
			return lastChild;
		}
		set
		{
			lastChild = value;
		}
	}

	public override XmlDocument OwnerDocument => null;

	public XmlSchemaSet Schemas
	{
		get
		{
			if (schemas == null)
			{
				schemas = new XmlSchemaSet(NameTable);
			}
			return schemas;
		}
		set
		{
			schemas = value;
		}
	}

	internal bool CanReportValidity => reportValidity;

	internal bool HasSetResolver => bSetResolver;

	public virtual XmlResolver XmlResolver
	{
		set
		{
			if (value != null)
			{
				try
				{
					new NamedPermissionSet("FullTrust").Demand();
				}
				catch (SecurityException inner)
				{
					throw new SecurityException(Res.GetString("XmlResolver can be set only by fully trusted code."), inner);
				}
			}
			resolver = value;
			if (!bSetResolver)
			{
				bSetResolver = true;
			}
			XmlDocumentType documentType = DocumentType;
			if (documentType != null)
			{
				documentType.DtdSchemaInfo = null;
			}
		}
	}

	public XmlNameTable NameTable => implementation.NameTable;

	public bool PreserveWhitespace
	{
		get
		{
			return preserveWhitespace;
		}
		set
		{
			preserveWhitespace = value;
		}
	}

	public override bool IsReadOnly => false;

	internal XmlNamedNodeMap Entities
	{
		get
		{
			if (entities == null)
			{
				entities = new XmlNamedNodeMap(this);
			}
			return entities;
		}
		set
		{
			entities = value;
		}
	}

	internal bool IsLoading
	{
		get
		{
			return isLoading;
		}
		set
		{
			isLoading = value;
		}
	}

	internal bool ActualLoadingStatus
	{
		get
		{
			return actualLoadingStatus;
		}
		set
		{
			actualLoadingStatus = value;
		}
	}

	internal Encoding TextEncoding
	{
		get
		{
			if (Declaration != null)
			{
				string encoding = Declaration.Encoding;
				if (encoding.Length > 0)
				{
					return System.Text.Encoding.GetEncoding(encoding);
				}
			}
			return null;
		}
	}

	public override string InnerText
	{
		set
		{
			throw new InvalidOperationException(Res.GetString("The 'InnerText' of a 'Document' node is read-only and cannot be set."));
		}
	}

	public override string InnerXml
	{
		get
		{
			return base.InnerXml;
		}
		set
		{
			LoadXml(value);
		}
	}

	internal string Version => Declaration?.Version;

	internal string Encoding => Declaration?.Encoding;

	internal string Standalone => Declaration?.Standalone;

	public override IXmlSchemaInfo SchemaInfo
	{
		get
		{
			if (reportValidity)
			{
				XmlElement documentElement = DocumentElement;
				if (documentElement != null)
				{
					switch (documentElement.SchemaInfo.Validity)
					{
					case XmlSchemaValidity.Valid:
						return ValidSchemaInfo;
					case XmlSchemaValidity.Invalid:
						return InvalidSchemaInfo;
					}
				}
			}
			return NotKnownSchemaInfo;
		}
	}

	public override string BaseURI => baseURI;

	internal override XPathNodeType XPNodeType => XPathNodeType.Root;

	internal bool HasEntityReferences => fEntRefNodesPresent;

	internal XmlAttribute NamespaceXml
	{
		get
		{
			if (namespaceXml == null)
			{
				namespaceXml = new XmlAttribute(AddAttrXmlName(strXmlns, strXml, strReservedXmlns, null), this);
				namespaceXml.Value = strReservedXml;
			}
			return namespaceXml;
		}
	}

	public event XmlNodeChangedEventHandler NodeInserting
	{
		add
		{
			onNodeInsertingDelegate = (XmlNodeChangedEventHandler)Delegate.Combine(onNodeInsertingDelegate, value);
		}
		remove
		{
			onNodeInsertingDelegate = (XmlNodeChangedEventHandler)Delegate.Remove(onNodeInsertingDelegate, value);
		}
	}

	public event XmlNodeChangedEventHandler NodeInserted
	{
		add
		{
			onNodeInsertedDelegate = (XmlNodeChangedEventHandler)Delegate.Combine(onNodeInsertedDelegate, value);
		}
		remove
		{
			onNodeInsertedDelegate = (XmlNodeChangedEventHandler)Delegate.Remove(onNodeInsertedDelegate, value);
		}
	}

	public event XmlNodeChangedEventHandler NodeRemoving
	{
		add
		{
			onNodeRemovingDelegate = (XmlNodeChangedEventHandler)Delegate.Combine(onNodeRemovingDelegate, value);
		}
		remove
		{
			onNodeRemovingDelegate = (XmlNodeChangedEventHandler)Delegate.Remove(onNodeRemovingDelegate, value);
		}
	}

	public event XmlNodeChangedEventHandler NodeRemoved
	{
		add
		{
			onNodeRemovedDelegate = (XmlNodeChangedEventHandler)Delegate.Combine(onNodeRemovedDelegate, value);
		}
		remove
		{
			onNodeRemovedDelegate = (XmlNodeChangedEventHandler)Delegate.Remove(onNodeRemovedDelegate, value);
		}
	}

	public event XmlNodeChangedEventHandler NodeChanging
	{
		add
		{
			onNodeChangingDelegate = (XmlNodeChangedEventHandler)Delegate.Combine(onNodeChangingDelegate, value);
		}
		remove
		{
			onNodeChangingDelegate = (XmlNodeChangedEventHandler)Delegate.Remove(onNodeChangingDelegate, value);
		}
	}

	public event XmlNodeChangedEventHandler NodeChanged
	{
		add
		{
			onNodeChangedDelegate = (XmlNodeChangedEventHandler)Delegate.Combine(onNodeChangedDelegate, value);
		}
		remove
		{
			onNodeChangedDelegate = (XmlNodeChangedEventHandler)Delegate.Remove(onNodeChangedDelegate, value);
		}
	}

	public XmlDocument()
		: this(new XmlImplementation())
	{
	}

	public XmlDocument(XmlNameTable nt)
		: this(new XmlImplementation(nt))
	{
	}

	protected internal XmlDocument(XmlImplementation imp)
	{
		implementation = imp;
		domNameTable = new DomNameTable(this);
		XmlNameTable nameTable = NameTable;
		nameTable.Add(string.Empty);
		strDocumentName = nameTable.Add("#document");
		strDocumentFragmentName = nameTable.Add("#document-fragment");
		strCommentName = nameTable.Add("#comment");
		strTextName = nameTable.Add("#text");
		strCDataSectionName = nameTable.Add("#cdata-section");
		strEntityName = nameTable.Add("#entity");
		strID = nameTable.Add("id");
		strNonSignificantWhitespaceName = nameTable.Add("#whitespace");
		strSignificantWhitespaceName = nameTable.Add("#significant-whitespace");
		strXmlns = nameTable.Add("xmlns");
		strXml = nameTable.Add("xml");
		strSpace = nameTable.Add("space");
		strLang = nameTable.Add("lang");
		strReservedXmlns = nameTable.Add("http://www.w3.org/2000/xmlns/");
		strReservedXml = nameTable.Add("http://www.w3.org/XML/1998/namespace");
		strEmpty = nameTable.Add(string.Empty);
		baseURI = string.Empty;
		objLock = new object();
	}

	internal static void CheckName(string name)
	{
		int num = ValidateNames.ParseNmtoken(name, 0);
		if (num < name.Length)
		{
			throw new XmlException("The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(name, num));
		}
	}

	internal XmlName AddXmlName(string prefix, string localName, string namespaceURI, IXmlSchemaInfo schemaInfo)
	{
		return domNameTable.AddName(prefix, localName, namespaceURI, schemaInfo);
	}

	internal XmlName GetXmlName(string prefix, string localName, string namespaceURI, IXmlSchemaInfo schemaInfo)
	{
		return domNameTable.GetName(prefix, localName, namespaceURI, schemaInfo);
	}

	internal XmlName AddAttrXmlName(string prefix, string localName, string namespaceURI, IXmlSchemaInfo schemaInfo)
	{
		XmlName xmlName = AddXmlName(prefix, localName, namespaceURI, schemaInfo);
		if (!IsLoading)
		{
			object prefix2 = xmlName.Prefix;
			object namespaceURI2 = xmlName.NamespaceURI;
			object localName2 = xmlName.LocalName;
			if ((prefix2 == strXmlns || (prefix2 == strEmpty && localName2 == strXmlns)) ^ (namespaceURI2 == strReservedXmlns))
			{
				throw new ArgumentException(Res.GetString("The namespace declaration attribute has an incorrect 'namespaceURI': '{0}'.", namespaceURI));
			}
		}
		return xmlName;
	}

	internal bool AddIdInfo(XmlName eleName, XmlName attrName)
	{
		if (htElementIDAttrDecl == null || htElementIDAttrDecl[eleName] == null)
		{
			if (htElementIDAttrDecl == null)
			{
				htElementIDAttrDecl = new Hashtable();
			}
			htElementIDAttrDecl.Add(eleName, attrName);
			return true;
		}
		return false;
	}

	private XmlName GetIDInfoByElement_(XmlName eleName)
	{
		XmlName xmlName = GetXmlName(eleName.Prefix, eleName.LocalName, string.Empty, null);
		if (xmlName != null)
		{
			return (XmlName)htElementIDAttrDecl[xmlName];
		}
		return null;
	}

	internal XmlName GetIDInfoByElement(XmlName eleName)
	{
		if (htElementIDAttrDecl == null)
		{
			return null;
		}
		return GetIDInfoByElement_(eleName);
	}

	private WeakReference GetElement(ArrayList elementList, XmlElement elem)
	{
		ArrayList arrayList = new ArrayList();
		foreach (WeakReference element in elementList)
		{
			if (!element.IsAlive)
			{
				arrayList.Add(element);
			}
			else if ((XmlElement)element.Target == elem)
			{
				return element;
			}
		}
		foreach (WeakReference item in arrayList)
		{
			elementList.Remove(item);
		}
		return null;
	}

	internal void AddElementWithId(string id, XmlElement elem)
	{
		if (htElementIdMap == null || !htElementIdMap.Contains(id))
		{
			if (htElementIdMap == null)
			{
				htElementIdMap = new Hashtable();
			}
			ArrayList arrayList = new ArrayList();
			arrayList.Add(new WeakReference(elem));
			htElementIdMap.Add(id, arrayList);
		}
		else
		{
			ArrayList arrayList2 = (ArrayList)htElementIdMap[id];
			if (GetElement(arrayList2, elem) == null)
			{
				arrayList2.Add(new WeakReference(elem));
			}
		}
	}

	internal void RemoveElementWithId(string id, XmlElement elem)
	{
		if (htElementIdMap == null || !htElementIdMap.Contains(id))
		{
			return;
		}
		ArrayList arrayList = (ArrayList)htElementIdMap[id];
		WeakReference element = GetElement(arrayList, elem);
		if (element != null)
		{
			arrayList.Remove(element);
			if (arrayList.Count == 0)
			{
				htElementIdMap.Remove(id);
			}
		}
	}

	public override XmlNode CloneNode(bool deep)
	{
		XmlDocument xmlDocument = Implementation.CreateDocument();
		xmlDocument.SetBaseURI(baseURI);
		if (deep)
		{
			xmlDocument.ImportChildren(this, xmlDocument, deep);
		}
		return xmlDocument;
	}

	internal XmlResolver GetResolver()
	{
		return resolver;
	}

	internal override bool IsValidChildType(XmlNodeType type)
	{
		switch (type)
		{
		case XmlNodeType.ProcessingInstruction:
		case XmlNodeType.Comment:
		case XmlNodeType.Whitespace:
		case XmlNodeType.SignificantWhitespace:
			return true;
		case XmlNodeType.DocumentType:
			if (DocumentType != null)
			{
				throw new InvalidOperationException(Res.GetString("This document already has a 'DocumentType' node."));
			}
			return true;
		case XmlNodeType.Element:
			if (DocumentElement != null)
			{
				throw new InvalidOperationException(Res.GetString("This document already has a 'DocumentElement' node."));
			}
			return true;
		case XmlNodeType.XmlDeclaration:
			if (Declaration != null)
			{
				throw new InvalidOperationException(Res.GetString("This document already has an 'XmlDeclaration' node."));
			}
			return true;
		default:
			return false;
		}
	}

	private bool HasNodeTypeInPrevSiblings(XmlNodeType nt, XmlNode refNode)
	{
		if (refNode == null)
		{
			return false;
		}
		XmlNode xmlNode = null;
		if (refNode.ParentNode != null)
		{
			xmlNode = refNode.ParentNode.FirstChild;
		}
		while (xmlNode != null)
		{
			if (xmlNode.NodeType == nt)
			{
				return true;
			}
			if (xmlNode == refNode)
			{
				break;
			}
			xmlNode = xmlNode.NextSibling;
		}
		return false;
	}

	private bool HasNodeTypeInNextSiblings(XmlNodeType nt, XmlNode refNode)
	{
		for (XmlNode xmlNode = refNode; xmlNode != null; xmlNode = xmlNode.NextSibling)
		{
			if (xmlNode.NodeType == nt)
			{
				return true;
			}
		}
		return false;
	}

	internal override bool CanInsertBefore(XmlNode newChild, XmlNode refChild)
	{
		if (refChild == null)
		{
			refChild = FirstChild;
		}
		if (refChild == null)
		{
			return true;
		}
		switch (newChild.NodeType)
		{
		case XmlNodeType.XmlDeclaration:
			return refChild == FirstChild;
		case XmlNodeType.ProcessingInstruction:
		case XmlNodeType.Comment:
			return refChild.NodeType != XmlNodeType.XmlDeclaration;
		case XmlNodeType.DocumentType:
			if (refChild.NodeType != XmlNodeType.XmlDeclaration)
			{
				return !HasNodeTypeInPrevSiblings(XmlNodeType.Element, refChild.PreviousSibling);
			}
			break;
		case XmlNodeType.Element:
			if (refChild.NodeType != XmlNodeType.XmlDeclaration)
			{
				return !HasNodeTypeInNextSiblings(XmlNodeType.DocumentType, refChild);
			}
			break;
		}
		return false;
	}

	internal override bool CanInsertAfter(XmlNode newChild, XmlNode refChild)
	{
		if (refChild == null)
		{
			refChild = LastChild;
		}
		if (refChild == null)
		{
			return true;
		}
		switch (newChild.NodeType)
		{
		case XmlNodeType.ProcessingInstruction:
		case XmlNodeType.Comment:
		case XmlNodeType.Whitespace:
		case XmlNodeType.SignificantWhitespace:
			return true;
		case XmlNodeType.DocumentType:
			return !HasNodeTypeInPrevSiblings(XmlNodeType.Element, refChild);
		case XmlNodeType.Element:
			return !HasNodeTypeInNextSiblings(XmlNodeType.DocumentType, refChild.NextSibling);
		default:
			return false;
		}
	}

	public XmlAttribute CreateAttribute(string name)
	{
		string prefix = string.Empty;
		string localName = string.Empty;
		string namespaceURI = string.Empty;
		XmlNode.SplitName(name, out prefix, out localName);
		SetDefaultNamespace(prefix, localName, ref namespaceURI);
		return CreateAttribute(prefix, localName, namespaceURI);
	}

	internal void SetDefaultNamespace(string prefix, string localName, ref string namespaceURI)
	{
		if (prefix == strXmlns || (prefix.Length == 0 && localName == strXmlns))
		{
			namespaceURI = strReservedXmlns;
		}
		else if (prefix == strXml)
		{
			namespaceURI = strReservedXml;
		}
	}

	public virtual XmlCDataSection CreateCDataSection(string data)
	{
		fCDataNodesPresent = true;
		return new XmlCDataSection(data, this);
	}

	public virtual XmlComment CreateComment(string data)
	{
		return new XmlComment(data, this);
	}

	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public virtual XmlDocumentType CreateDocumentType(string name, string publicId, string systemId, string internalSubset)
	{
		return new XmlDocumentType(name, publicId, systemId, internalSubset, this);
	}

	public virtual XmlDocumentFragment CreateDocumentFragment()
	{
		return new XmlDocumentFragment(this);
	}

	public XmlElement CreateElement(string name)
	{
		string prefix = string.Empty;
		string localName = string.Empty;
		XmlNode.SplitName(name, out prefix, out localName);
		return CreateElement(prefix, localName, string.Empty);
	}

	internal void AddDefaultAttributes(XmlElement elem)
	{
		SchemaInfo dtdSchemaInfo = DtdSchemaInfo;
		SchemaElementDecl schemaElementDecl = GetSchemaElementDecl(elem);
		if (schemaElementDecl == null || schemaElementDecl.AttDefs == null)
		{
			return;
		}
		IDictionaryEnumerator dictionaryEnumerator = schemaElementDecl.AttDefs.GetEnumerator();
		while (dictionaryEnumerator.MoveNext())
		{
			SchemaAttDef schemaAttDef = (SchemaAttDef)dictionaryEnumerator.Value;
			if (schemaAttDef.Presence == SchemaDeclBase.Use.Default || schemaAttDef.Presence == SchemaDeclBase.Use.Fixed)
			{
				string empty = string.Empty;
				string name = schemaAttDef.Name.Name;
				string attrNamespaceURI = string.Empty;
				if (dtdSchemaInfo.SchemaType == SchemaType.DTD)
				{
					empty = schemaAttDef.Name.Namespace;
				}
				else
				{
					empty = schemaAttDef.Prefix;
					attrNamespaceURI = schemaAttDef.Name.Namespace;
				}
				XmlAttribute attributeNode = PrepareDefaultAttribute(schemaAttDef, empty, name, attrNamespaceURI);
				elem.SetAttributeNode(attributeNode);
			}
		}
	}

	private SchemaElementDecl GetSchemaElementDecl(XmlElement elem)
	{
		SchemaInfo dtdSchemaInfo = DtdSchemaInfo;
		if (dtdSchemaInfo != null)
		{
			XmlQualifiedName key = new XmlQualifiedName(elem.LocalName, (dtdSchemaInfo.SchemaType == SchemaType.DTD) ? elem.Prefix : elem.NamespaceURI);
			if (dtdSchemaInfo.ElementDecls.TryGetValue(key, out var value))
			{
				return value;
			}
		}
		return null;
	}

	private XmlAttribute PrepareDefaultAttribute(SchemaAttDef attdef, string attrPrefix, string attrLocalname, string attrNamespaceURI)
	{
		SetDefaultNamespace(attrPrefix, attrLocalname, ref attrNamespaceURI);
		XmlAttribute xmlAttribute = CreateDefaultAttribute(attrPrefix, attrLocalname, attrNamespaceURI);
		xmlAttribute.InnerXml = attdef.DefaultValueRaw;
		if (xmlAttribute is XmlUnspecifiedAttribute xmlUnspecifiedAttribute)
		{
			xmlUnspecifiedAttribute.SetSpecified(f: false);
		}
		return xmlAttribute;
	}

	public virtual XmlEntityReference CreateEntityReference(string name)
	{
		return new XmlEntityReference(name, this);
	}

	public virtual XmlProcessingInstruction CreateProcessingInstruction(string target, string data)
	{
		return new XmlProcessingInstruction(target, data, this);
	}

	public virtual XmlDeclaration CreateXmlDeclaration(string version, string encoding, string standalone)
	{
		return new XmlDeclaration(version, encoding, standalone, this);
	}

	public virtual XmlText CreateTextNode(string text)
	{
		return new XmlText(text, this);
	}

	public virtual XmlSignificantWhitespace CreateSignificantWhitespace(string text)
	{
		return new XmlSignificantWhitespace(text, this);
	}

	public override XPathNavigator CreateNavigator()
	{
		return CreateNavigator(this);
	}

	protected internal virtual XPathNavigator CreateNavigator(XmlNode node)
	{
		switch (node.NodeType)
		{
		case XmlNodeType.EntityReference:
		case XmlNodeType.Entity:
		case XmlNodeType.DocumentType:
		case XmlNodeType.Notation:
		case XmlNodeType.XmlDeclaration:
			return null;
		case XmlNodeType.Text:
		case XmlNodeType.CDATA:
		case XmlNodeType.SignificantWhitespace:
		{
			XmlNode xmlNode = node.ParentNode;
			if (xmlNode != null)
			{
				do
				{
					switch (xmlNode.NodeType)
					{
					case XmlNodeType.Attribute:
						return null;
					case XmlNodeType.EntityReference:
						goto IL_006a;
					}
					break;
					IL_006a:
					xmlNode = xmlNode.ParentNode;
				}
				while (xmlNode != null);
			}
			node = NormalizeText(node);
			break;
		}
		case XmlNodeType.Whitespace:
		{
			XmlNode xmlNode = node.ParentNode;
			if (xmlNode != null)
			{
				do
				{
					switch (xmlNode.NodeType)
					{
					case XmlNodeType.Attribute:
					case XmlNodeType.Document:
						return null;
					case XmlNodeType.EntityReference:
						goto IL_009f;
					}
					break;
					IL_009f:
					xmlNode = xmlNode.ParentNode;
				}
				while (xmlNode != null);
			}
			node = NormalizeText(node);
			break;
		}
		}
		return new DocumentXPathNavigator(this, node);
	}

	internal static bool IsTextNode(XmlNodeType nt)
	{
		if ((uint)(nt - 3) <= 1u || (uint)(nt - 13) <= 1u)
		{
			return true;
		}
		return false;
	}

	private XmlNode NormalizeText(XmlNode n)
	{
		XmlNode xmlNode = null;
		while (IsTextNode(n.NodeType))
		{
			xmlNode = n;
			n = n.PreviousSibling;
			if (n == null)
			{
				XmlNode xmlNode2 = xmlNode;
				while (xmlNode2.ParentNode != null && xmlNode2.ParentNode.NodeType == XmlNodeType.EntityReference)
				{
					if (xmlNode2.ParentNode.PreviousSibling != null)
					{
						n = xmlNode2.ParentNode.PreviousSibling;
						break;
					}
					xmlNode2 = xmlNode2.ParentNode;
					if (xmlNode2 == null)
					{
						break;
					}
				}
			}
			if (n == null)
			{
				break;
			}
			while (n.NodeType == XmlNodeType.EntityReference)
			{
				n = n.LastChild;
			}
		}
		return xmlNode;
	}

	public virtual XmlWhitespace CreateWhitespace(string text)
	{
		return new XmlWhitespace(text, this);
	}

	public virtual XmlNodeList GetElementsByTagName(string name)
	{
		return new XmlElementList(this, name);
	}

	public XmlAttribute CreateAttribute(string qualifiedName, string namespaceURI)
	{
		string prefix = string.Empty;
		string localName = string.Empty;
		XmlNode.SplitName(qualifiedName, out prefix, out localName);
		return CreateAttribute(prefix, localName, namespaceURI);
	}

	public XmlElement CreateElement(string qualifiedName, string namespaceURI)
	{
		string prefix = string.Empty;
		string localName = string.Empty;
		XmlNode.SplitName(qualifiedName, out prefix, out localName);
		return CreateElement(prefix, localName, namespaceURI);
	}

	public virtual XmlNodeList GetElementsByTagName(string localName, string namespaceURI)
	{
		return new XmlElementList(this, localName, namespaceURI);
	}

	public virtual XmlElement GetElementById(string elementId)
	{
		if (htElementIdMap != null)
		{
			ArrayList arrayList = (ArrayList)htElementIdMap[elementId];
			if (arrayList != null)
			{
				foreach (WeakReference item in arrayList)
				{
					XmlElement xmlElement = (XmlElement)item.Target;
					if (xmlElement != null && xmlElement.IsConnected())
					{
						return xmlElement;
					}
				}
			}
		}
		return null;
	}

	public virtual XmlNode ImportNode(XmlNode node, bool deep)
	{
		return ImportNodeInternal(node, deep);
	}

	private XmlNode ImportNodeInternal(XmlNode node, bool deep)
	{
		XmlNode xmlNode = null;
		if (node == null)
		{
			throw new InvalidOperationException(Res.GetString("Cannot import a null node."));
		}
		switch (node.NodeType)
		{
		case XmlNodeType.Element:
			xmlNode = CreateElement(node.Prefix, node.LocalName, node.NamespaceURI);
			ImportAttributes(node, xmlNode);
			if (deep)
			{
				ImportChildren(node, xmlNode, deep);
			}
			break;
		case XmlNodeType.Attribute:
			xmlNode = CreateAttribute(node.Prefix, node.LocalName, node.NamespaceURI);
			ImportChildren(node, xmlNode, deep: true);
			break;
		case XmlNodeType.Text:
			xmlNode = CreateTextNode(node.Value);
			break;
		case XmlNodeType.Comment:
			xmlNode = CreateComment(node.Value);
			break;
		case XmlNodeType.ProcessingInstruction:
			xmlNode = CreateProcessingInstruction(node.Name, node.Value);
			break;
		case XmlNodeType.XmlDeclaration:
		{
			XmlDeclaration xmlDeclaration = (XmlDeclaration)node;
			xmlNode = CreateXmlDeclaration(xmlDeclaration.Version, xmlDeclaration.Encoding, xmlDeclaration.Standalone);
			break;
		}
		case XmlNodeType.CDATA:
			xmlNode = CreateCDataSection(node.Value);
			break;
		case XmlNodeType.DocumentType:
		{
			XmlDocumentType xmlDocumentType = (XmlDocumentType)node;
			xmlNode = CreateDocumentType(xmlDocumentType.Name, xmlDocumentType.PublicId, xmlDocumentType.SystemId, xmlDocumentType.InternalSubset);
			break;
		}
		case XmlNodeType.DocumentFragment:
			xmlNode = CreateDocumentFragment();
			if (deep)
			{
				ImportChildren(node, xmlNode, deep);
			}
			break;
		case XmlNodeType.EntityReference:
			xmlNode = CreateEntityReference(node.Name);
			break;
		case XmlNodeType.Whitespace:
			xmlNode = CreateWhitespace(node.Value);
			break;
		case XmlNodeType.SignificantWhitespace:
			xmlNode = CreateSignificantWhitespace(node.Value);
			break;
		default:
			throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Res.GetString("Cannot import nodes of type '{0}'."), node.NodeType.ToString()));
		}
		return xmlNode;
	}

	private void ImportAttributes(XmlNode fromElem, XmlNode toElem)
	{
		int count = fromElem.Attributes.Count;
		for (int i = 0; i < count; i++)
		{
			if (fromElem.Attributes[i].Specified)
			{
				toElem.Attributes.SetNamedItem(ImportNodeInternal(fromElem.Attributes[i], deep: true));
			}
		}
	}

	private void ImportChildren(XmlNode fromNode, XmlNode toNode, bool deep)
	{
		for (XmlNode xmlNode = fromNode.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
		{
			toNode.AppendChild(ImportNodeInternal(xmlNode, deep));
		}
	}

	public virtual XmlAttribute CreateAttribute(string prefix, string localName, string namespaceURI)
	{
		return new XmlAttribute(AddAttrXmlName(prefix, localName, namespaceURI, null), this);
	}

	protected internal virtual XmlAttribute CreateDefaultAttribute(string prefix, string localName, string namespaceURI)
	{
		return new XmlUnspecifiedAttribute(prefix, localName, namespaceURI, this);
	}

	public virtual XmlElement CreateElement(string prefix, string localName, string namespaceURI)
	{
		XmlElement xmlElement = new XmlElement(AddXmlName(prefix, localName, namespaceURI, null), empty: true, this);
		if (!IsLoading)
		{
			AddDefaultAttributes(xmlElement);
		}
		return xmlElement;
	}

	public virtual XmlNode CreateNode(XmlNodeType type, string prefix, string name, string namespaceURI)
	{
		switch (type)
		{
		case XmlNodeType.Element:
			if (prefix != null)
			{
				return CreateElement(prefix, name, namespaceURI);
			}
			return CreateElement(name, namespaceURI);
		case XmlNodeType.Attribute:
			if (prefix != null)
			{
				return CreateAttribute(prefix, name, namespaceURI);
			}
			return CreateAttribute(name, namespaceURI);
		case XmlNodeType.Text:
			return CreateTextNode(string.Empty);
		case XmlNodeType.CDATA:
			return CreateCDataSection(string.Empty);
		case XmlNodeType.EntityReference:
			return CreateEntityReference(name);
		case XmlNodeType.ProcessingInstruction:
			return CreateProcessingInstruction(name, string.Empty);
		case XmlNodeType.XmlDeclaration:
			return CreateXmlDeclaration("1.0", null, null);
		case XmlNodeType.Comment:
			return CreateComment(string.Empty);
		case XmlNodeType.DocumentFragment:
			return CreateDocumentFragment();
		case XmlNodeType.DocumentType:
			return CreateDocumentType(name, string.Empty, string.Empty, string.Empty);
		case XmlNodeType.Document:
			return new XmlDocument();
		case XmlNodeType.SignificantWhitespace:
			return CreateSignificantWhitespace(string.Empty);
		case XmlNodeType.Whitespace:
			return CreateWhitespace(string.Empty);
		default:
			throw new ArgumentException(Res.GetString("Cannot create node of type {0}.", type));
		}
	}

	public virtual XmlNode CreateNode(string nodeTypeString, string name, string namespaceURI)
	{
		return CreateNode(ConvertToNodeType(nodeTypeString), name, namespaceURI);
	}

	public virtual XmlNode CreateNode(XmlNodeType type, string name, string namespaceURI)
	{
		return CreateNode(type, null, name, namespaceURI);
	}

	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public virtual XmlNode ReadNode(XmlReader reader)
	{
		XmlNode xmlNode = null;
		try
		{
			IsLoading = true;
			return new XmlLoader().ReadCurrentNode(this, reader);
		}
		finally
		{
			IsLoading = false;
		}
	}

	internal XmlNodeType ConvertToNodeType(string nodeTypeString)
	{
		return nodeTypeString switch
		{
			"element" => XmlNodeType.Element, 
			"attribute" => XmlNodeType.Attribute, 
			"text" => XmlNodeType.Text, 
			"cdatasection" => XmlNodeType.CDATA, 
			"entityreference" => XmlNodeType.EntityReference, 
			"entity" => XmlNodeType.Entity, 
			"processinginstruction" => XmlNodeType.ProcessingInstruction, 
			"comment" => XmlNodeType.Comment, 
			"document" => XmlNodeType.Document, 
			"documenttype" => XmlNodeType.DocumentType, 
			"documentfragment" => XmlNodeType.DocumentFragment, 
			"notation" => XmlNodeType.Notation, 
			"significantwhitespace" => XmlNodeType.SignificantWhitespace, 
			"whitespace" => XmlNodeType.Whitespace, 
			_ => throw new ArgumentException(Res.GetString("'{0}' does not represent any 'XmlNodeType'.", nodeTypeString)), 
		};
	}

	private XmlTextReader SetupReader(XmlTextReader tr)
	{
		tr.XmlValidatingReaderCompatibilityMode = true;
		tr.EntityHandling = EntityHandling.ExpandCharEntities;
		if (HasSetResolver)
		{
			tr.XmlResolver = GetResolver();
		}
		return tr;
	}

	public virtual void Load(string filename)
	{
		XmlTextReader xmlTextReader = SetupReader(new XmlTextReader(filename, NameTable));
		try
		{
			Load(xmlTextReader);
		}
		finally
		{
			xmlTextReader.Close();
		}
	}

	public virtual void Load(Stream inStream)
	{
		XmlTextReader xmlTextReader = SetupReader(new XmlTextReader(inStream, NameTable));
		try
		{
			Load(xmlTextReader);
		}
		finally
		{
			xmlTextReader.Impl.Close(closeInput: false);
		}
	}

	public virtual void Load(TextReader txtReader)
	{
		XmlTextReader xmlTextReader = SetupReader(new XmlTextReader(txtReader, NameTable));
		try
		{
			Load(xmlTextReader);
		}
		finally
		{
			xmlTextReader.Impl.Close(closeInput: false);
		}
	}

	public virtual void Load(XmlReader reader)
	{
		try
		{
			IsLoading = true;
			actualLoadingStatus = true;
			RemoveAll();
			fEntRefNodesPresent = false;
			fCDataNodesPresent = false;
			reportValidity = true;
			new XmlLoader().Load(this, reader, preserveWhitespace);
		}
		finally
		{
			IsLoading = false;
			actualLoadingStatus = false;
			reportValidity = true;
		}
	}

	public virtual void LoadXml(string xml)
	{
		XmlTextReader xmlTextReader = SetupReader(new XmlTextReader(new StringReader(xml), NameTable));
		try
		{
			Load(xmlTextReader);
		}
		finally
		{
			xmlTextReader.Close();
		}
	}

	public virtual void Save(string filename)
	{
		if (DocumentElement == null)
		{
			throw new XmlException("Invalid XML document. {0}", Res.GetString("The document does not have a root element."));
		}
		XmlDOMTextWriter xmlDOMTextWriter = new XmlDOMTextWriter(filename, TextEncoding);
		try
		{
			if (!preserveWhitespace)
			{
				xmlDOMTextWriter.Formatting = Formatting.Indented;
			}
			WriteTo(xmlDOMTextWriter);
			xmlDOMTextWriter.Flush();
		}
		finally
		{
			xmlDOMTextWriter.Close();
		}
	}

	public virtual void Save(Stream outStream)
	{
		XmlDOMTextWriter xmlDOMTextWriter = new XmlDOMTextWriter(outStream, TextEncoding);
		if (!preserveWhitespace)
		{
			xmlDOMTextWriter.Formatting = Formatting.Indented;
		}
		WriteTo(xmlDOMTextWriter);
		xmlDOMTextWriter.Flush();
	}

	public virtual void Save(TextWriter writer)
	{
		XmlDOMTextWriter xmlDOMTextWriter = new XmlDOMTextWriter(writer);
		if (!preserveWhitespace)
		{
			xmlDOMTextWriter.Formatting = Formatting.Indented;
		}
		Save(xmlDOMTextWriter);
	}

	public virtual void Save(XmlWriter w)
	{
		XmlNode xmlNode = FirstChild;
		if (xmlNode == null)
		{
			return;
		}
		if (w.WriteState == WriteState.Start)
		{
			if (xmlNode is XmlDeclaration)
			{
				if (Standalone.Length == 0)
				{
					w.WriteStartDocument();
				}
				else if (Standalone == "yes")
				{
					w.WriteStartDocument(standalone: true);
				}
				else if (Standalone == "no")
				{
					w.WriteStartDocument(standalone: false);
				}
				xmlNode = xmlNode.NextSibling;
			}
			else
			{
				w.WriteStartDocument();
			}
		}
		while (xmlNode != null)
		{
			xmlNode.WriteTo(w);
			xmlNode = xmlNode.NextSibling;
		}
		w.Flush();
	}

	public override void WriteTo(XmlWriter w)
	{
		WriteContentTo(w);
	}

	public override void WriteContentTo(XmlWriter xw)
	{
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				((XmlNode)enumerator.Current).WriteTo(xw);
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
	}

	public void Validate(ValidationEventHandler validationEventHandler)
	{
		Validate(validationEventHandler, this);
	}

	public void Validate(ValidationEventHandler validationEventHandler, XmlNode nodeToValidate)
	{
		if (schemas == null || schemas.Count == 0)
		{
			throw new InvalidOperationException(Res.GetString("The XmlSchemaSet on the document is either null or has no schemas in it. Provide schema information before calling Validate."));
		}
		if (nodeToValidate.Document != this)
		{
			throw new ArgumentException(Res.GetString("Cannot validate '{0}' because its owner document is not the current document.", "nodeToValidate"));
		}
		if (nodeToValidate == this)
		{
			reportValidity = false;
		}
		new DocumentSchemaValidator(this, schemas, validationEventHandler).Validate(nodeToValidate);
		if (nodeToValidate == this)
		{
			reportValidity = true;
		}
	}

	internal override XmlNodeChangedEventArgs GetEventArgs(XmlNode node, XmlNode oldParent, XmlNode newParent, string oldValue, string newValue, XmlNodeChangedAction action)
	{
		reportValidity = false;
		switch (action)
		{
		case XmlNodeChangedAction.Insert:
			if (onNodeInsertingDelegate == null && onNodeInsertedDelegate == null)
			{
				return null;
			}
			break;
		case XmlNodeChangedAction.Remove:
			if (onNodeRemovingDelegate == null && onNodeRemovedDelegate == null)
			{
				return null;
			}
			break;
		case XmlNodeChangedAction.Change:
			if (onNodeChangingDelegate == null && onNodeChangedDelegate == null)
			{
				return null;
			}
			break;
		}
		return new XmlNodeChangedEventArgs(node, oldParent, newParent, oldValue, newValue, action);
	}

	internal XmlNodeChangedEventArgs GetInsertEventArgsForLoad(XmlNode node, XmlNode newParent)
	{
		if (onNodeInsertingDelegate == null && onNodeInsertedDelegate == null)
		{
			return null;
		}
		string value = node.Value;
		return new XmlNodeChangedEventArgs(node, null, newParent, value, value, XmlNodeChangedAction.Insert);
	}

	internal override void BeforeEvent(XmlNodeChangedEventArgs args)
	{
		if (args == null)
		{
			return;
		}
		switch (args.Action)
		{
		case XmlNodeChangedAction.Insert:
			if (onNodeInsertingDelegate != null)
			{
				onNodeInsertingDelegate(this, args);
			}
			break;
		case XmlNodeChangedAction.Remove:
			if (onNodeRemovingDelegate != null)
			{
				onNodeRemovingDelegate(this, args);
			}
			break;
		case XmlNodeChangedAction.Change:
			if (onNodeChangingDelegate != null)
			{
				onNodeChangingDelegate(this, args);
			}
			break;
		}
	}

	internal override void AfterEvent(XmlNodeChangedEventArgs args)
	{
		if (args == null)
		{
			return;
		}
		switch (args.Action)
		{
		case XmlNodeChangedAction.Insert:
			if (onNodeInsertedDelegate != null)
			{
				onNodeInsertedDelegate(this, args);
			}
			break;
		case XmlNodeChangedAction.Remove:
			if (onNodeRemovedDelegate != null)
			{
				onNodeRemovedDelegate(this, args);
			}
			break;
		case XmlNodeChangedAction.Change:
			if (onNodeChangedDelegate != null)
			{
				onNodeChangedDelegate(this, args);
			}
			break;
		}
	}

	internal XmlAttribute GetDefaultAttribute(XmlElement elem, string attrPrefix, string attrLocalname, string attrNamespaceURI)
	{
		SchemaInfo dtdSchemaInfo = DtdSchemaInfo;
		SchemaElementDecl schemaElementDecl = GetSchemaElementDecl(elem);
		if (schemaElementDecl != null && schemaElementDecl.AttDefs != null)
		{
			IDictionaryEnumerator dictionaryEnumerator = schemaElementDecl.AttDefs.GetEnumerator();
			while (dictionaryEnumerator.MoveNext())
			{
				SchemaAttDef schemaAttDef = (SchemaAttDef)dictionaryEnumerator.Value;
				if ((schemaAttDef.Presence == SchemaDeclBase.Use.Default || schemaAttDef.Presence == SchemaDeclBase.Use.Fixed) && schemaAttDef.Name.Name == attrLocalname && ((dtdSchemaInfo.SchemaType == SchemaType.DTD && schemaAttDef.Name.Namespace == attrPrefix) || (dtdSchemaInfo.SchemaType != SchemaType.DTD && schemaAttDef.Name.Namespace == attrNamespaceURI)))
				{
					return PrepareDefaultAttribute(schemaAttDef, attrPrefix, attrLocalname, attrNamespaceURI);
				}
			}
		}
		return null;
	}

	internal XmlEntity GetEntityNode(string name)
	{
		if (DocumentType != null)
		{
			XmlNamedNodeMap xmlNamedNodeMap = DocumentType.Entities;
			if (xmlNamedNodeMap != null)
			{
				return (XmlEntity)xmlNamedNodeMap.GetNamedItem(name);
			}
		}
		return null;
	}

	internal void SetBaseURI(string inBaseURI)
	{
		baseURI = inBaseURI;
	}

	internal override XmlNode AppendChildForLoad(XmlNode newChild, XmlDocument doc)
	{
		if (!IsValidChildType(newChild.NodeType))
		{
			throw new InvalidOperationException(Res.GetString("The specified node cannot be inserted as the valid child of this node, because the specified node is the wrong type."));
		}
		if (!CanInsertAfter(newChild, LastChild))
		{
			throw new InvalidOperationException(Res.GetString("Cannot insert the node in the specified location."));
		}
		XmlNodeChangedEventArgs insertEventArgsForLoad = GetInsertEventArgsForLoad(newChild, this);
		if (insertEventArgsForLoad != null)
		{
			BeforeEvent(insertEventArgsForLoad);
		}
		XmlLinkedNode xmlLinkedNode = (XmlLinkedNode)newChild;
		if (lastChild == null)
		{
			xmlLinkedNode.next = xmlLinkedNode;
		}
		else
		{
			xmlLinkedNode.next = lastChild.next;
			lastChild.next = xmlLinkedNode;
		}
		lastChild = xmlLinkedNode;
		xmlLinkedNode.SetParentForLoad(this);
		if (insertEventArgsForLoad != null)
		{
			AfterEvent(insertEventArgsForLoad);
		}
		return xmlLinkedNode;
	}
}
