using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;

namespace System.Xml;

internal class XmlNodeReaderNavigator
{
	internal struct VirtualAttribute
	{
		internal string name;

		internal string value;

		internal VirtualAttribute(string name, string value)
		{
			this.name = name;
			this.value = value;
		}
	}

	private XmlNode curNode;

	private XmlNode elemNode;

	private XmlNode logNode;

	private int attrIndex;

	private int logAttrIndex;

	private XmlNameTable nameTable;

	private XmlDocument doc;

	private int nAttrInd;

	private const string strPublicID = "PUBLIC";

	private const string strSystemID = "SYSTEM";

	private const string strVersion = "version";

	private const string strStandalone = "standalone";

	private const string strEncoding = "encoding";

	private int nDeclarationAttrCount;

	private int nDocTypeAttrCount;

	private int nLogLevel;

	private int nLogAttrInd;

	private bool bLogOnAttrVal;

	private bool bCreatedOnAttribute;

	internal VirtualAttribute[] decNodeAttributes = new VirtualAttribute[3]
	{
		new VirtualAttribute(null, null),
		new VirtualAttribute(null, null),
		new VirtualAttribute(null, null)
	};

	internal VirtualAttribute[] docTypeNodeAttributes = new VirtualAttribute[2]
	{
		new VirtualAttribute(null, null),
		new VirtualAttribute(null, null)
	};

	private bool bOnAttrVal;

	public XmlNodeType NodeType
	{
		get
		{
			XmlNodeType nodeType = curNode.NodeType;
			if (nAttrInd != -1)
			{
				if (bOnAttrVal)
				{
					return XmlNodeType.Text;
				}
				return XmlNodeType.Attribute;
			}
			return nodeType;
		}
	}

	public string NamespaceURI => curNode.NamespaceURI;

	public string Name
	{
		get
		{
			if (nAttrInd != -1)
			{
				if (bOnAttrVal)
				{
					return string.Empty;
				}
				if (curNode.NodeType == XmlNodeType.XmlDeclaration)
				{
					return decNodeAttributes[nAttrInd].name;
				}
				return docTypeNodeAttributes[nAttrInd].name;
			}
			if (IsLocalNameEmpty(curNode.NodeType))
			{
				return string.Empty;
			}
			return curNode.Name;
		}
	}

	public string LocalName
	{
		get
		{
			if (nAttrInd != -1)
			{
				return Name;
			}
			if (IsLocalNameEmpty(curNode.NodeType))
			{
				return string.Empty;
			}
			return curNode.LocalName;
		}
	}

	internal bool IsOnAttrVal => bOnAttrVal;

	internal XmlNode OwnerElementNode
	{
		get
		{
			if (bCreatedOnAttribute)
			{
				return null;
			}
			return elemNode;
		}
	}

	internal bool CreatedOnAttribute => bCreatedOnAttribute;

	public string Prefix => curNode.Prefix;

	public bool HasValue
	{
		get
		{
			if (nAttrInd != -1)
			{
				return true;
			}
			if (curNode.Value != null || curNode.NodeType == XmlNodeType.DocumentType)
			{
				return true;
			}
			return false;
		}
	}

	public string Value
	{
		get
		{
			string text = null;
			XmlNodeType nodeType = curNode.NodeType;
			if (nAttrInd != -1)
			{
				if (curNode.NodeType == XmlNodeType.XmlDeclaration)
				{
					return decNodeAttributes[nAttrInd].value;
				}
				return docTypeNodeAttributes[nAttrInd].value;
			}
			switch (nodeType)
			{
			case XmlNodeType.DocumentType:
				text = ((XmlDocumentType)curNode).InternalSubset;
				break;
			case XmlNodeType.XmlDeclaration:
			{
				StringBuilder stringBuilder = new StringBuilder(string.Empty);
				if (nDeclarationAttrCount == -1)
				{
					InitDecAttr();
				}
				for (int i = 0; i < nDeclarationAttrCount; i++)
				{
					stringBuilder.Append(decNodeAttributes[i].name + "=\"" + decNodeAttributes[i].value + "\"");
					if (i != nDeclarationAttrCount - 1)
					{
						stringBuilder.Append(" ");
					}
				}
				text = stringBuilder.ToString();
				break;
			}
			default:
				text = curNode.Value;
				break;
			}
			if (text != null)
			{
				return text;
			}
			return string.Empty;
		}
	}

	public string BaseURI => curNode.BaseURI;

	public XmlSpace XmlSpace => curNode.XmlSpace;

	public string XmlLang => curNode.XmlLang;

	public bool IsEmptyElement
	{
		get
		{
			if (curNode.NodeType == XmlNodeType.Element)
			{
				return ((XmlElement)curNode).IsEmpty;
			}
			return false;
		}
	}

	public bool IsDefault
	{
		get
		{
			if (curNode.NodeType == XmlNodeType.Attribute)
			{
				return !((XmlAttribute)curNode).Specified;
			}
			return false;
		}
	}

	public IXmlSchemaInfo SchemaInfo => curNode.SchemaInfo;

	public XmlNameTable NameTable => nameTable;

	public int AttributeCount
	{
		get
		{
			if (bCreatedOnAttribute)
			{
				return 0;
			}
			XmlNodeType nodeType = curNode.NodeType;
			switch (nodeType)
			{
			case XmlNodeType.Element:
				return ((XmlElement)curNode).Attributes.Count;
			default:
				if (!bOnAttrVal || nodeType == XmlNodeType.XmlDeclaration || nodeType == XmlNodeType.DocumentType)
				{
					break;
				}
				goto case XmlNodeType.Attribute;
			case XmlNodeType.Attribute:
				return elemNode.Attributes.Count;
			}
			switch (nodeType)
			{
			case XmlNodeType.XmlDeclaration:
				if (nDeclarationAttrCount != -1)
				{
					return nDeclarationAttrCount;
				}
				InitDecAttr();
				return nDeclarationAttrCount;
			case XmlNodeType.DocumentType:
				if (nDocTypeAttrCount != -1)
				{
					return nDocTypeAttrCount;
				}
				InitDocTypeAttr();
				return nDocTypeAttrCount;
			default:
				return 0;
			}
		}
	}

	private bool IsOnDeclOrDocType
	{
		get
		{
			XmlNodeType nodeType = curNode.NodeType;
			if (nodeType != XmlNodeType.XmlDeclaration)
			{
				return nodeType == XmlNodeType.DocumentType;
			}
			return true;
		}
	}

	public XmlDocument Document => doc;

	public XmlNodeReaderNavigator(XmlNode node)
	{
		curNode = node;
		logNode = node;
		XmlNodeType nodeType = curNode.NodeType;
		if (nodeType == XmlNodeType.Attribute)
		{
			elemNode = null;
			attrIndex = -1;
			bCreatedOnAttribute = true;
		}
		else
		{
			elemNode = node;
			attrIndex = -1;
			bCreatedOnAttribute = false;
		}
		if (nodeType == XmlNodeType.Document)
		{
			doc = (XmlDocument)curNode;
		}
		else
		{
			doc = node.OwnerDocument;
		}
		nameTable = doc.NameTable;
		nAttrInd = -1;
		nDeclarationAttrCount = -1;
		nDocTypeAttrCount = -1;
		bOnAttrVal = false;
		bLogOnAttrVal = false;
	}

	private bool IsLocalNameEmpty(XmlNodeType nt)
	{
		switch (nt)
		{
		case XmlNodeType.None:
		case XmlNodeType.Text:
		case XmlNodeType.CDATA:
		case XmlNodeType.Comment:
		case XmlNodeType.Document:
		case XmlNodeType.DocumentFragment:
		case XmlNodeType.Whitespace:
		case XmlNodeType.SignificantWhitespace:
		case XmlNodeType.EndElement:
		case XmlNodeType.EndEntity:
			return true;
		case XmlNodeType.Element:
		case XmlNodeType.Attribute:
		case XmlNodeType.EntityReference:
		case XmlNodeType.Entity:
		case XmlNodeType.ProcessingInstruction:
		case XmlNodeType.DocumentType:
		case XmlNodeType.Notation:
		case XmlNodeType.XmlDeclaration:
			return false;
		default:
			return true;
		}
	}

	private void CheckIndexCondition(int attributeIndex)
	{
		if (attributeIndex < 0 || attributeIndex >= AttributeCount)
		{
			throw new ArgumentOutOfRangeException("attributeIndex");
		}
	}

	private void InitDecAttr()
	{
		int num = 0;
		string version = doc.Version;
		if (version != null && version.Length != 0)
		{
			decNodeAttributes[num].name = "version";
			decNodeAttributes[num].value = version;
			num++;
		}
		version = doc.Encoding;
		if (version != null && version.Length != 0)
		{
			decNodeAttributes[num].name = "encoding";
			decNodeAttributes[num].value = version;
			num++;
		}
		version = doc.Standalone;
		if (version != null && version.Length != 0)
		{
			decNodeAttributes[num].name = "standalone";
			decNodeAttributes[num].value = version;
			num++;
		}
		nDeclarationAttrCount = num;
	}

	public string GetDeclarationAttr(XmlDeclaration decl, string name)
	{
		return name switch
		{
			"version" => decl.Version, 
			"encoding" => decl.Encoding, 
			"standalone" => decl.Standalone, 
			_ => null, 
		};
	}

	public string GetDeclarationAttr(int i)
	{
		if (nDeclarationAttrCount == -1)
		{
			InitDecAttr();
		}
		return decNodeAttributes[i].value;
	}

	public int GetDecAttrInd(string name)
	{
		if (nDeclarationAttrCount == -1)
		{
			InitDecAttr();
		}
		for (int i = 0; i < nDeclarationAttrCount; i++)
		{
			if (decNodeAttributes[i].name == name)
			{
				return i;
			}
		}
		return -1;
	}

	private void InitDocTypeAttr()
	{
		int num = 0;
		XmlDocumentType documentType = doc.DocumentType;
		if (documentType == null)
		{
			nDocTypeAttrCount = 0;
			return;
		}
		string publicId = documentType.PublicId;
		if (publicId != null)
		{
			docTypeNodeAttributes[num].name = "PUBLIC";
			docTypeNodeAttributes[num].value = publicId;
			num++;
		}
		publicId = documentType.SystemId;
		if (publicId != null)
		{
			docTypeNodeAttributes[num].name = "SYSTEM";
			docTypeNodeAttributes[num].value = publicId;
			num++;
		}
		nDocTypeAttrCount = num;
	}

	public string GetDocumentTypeAttr(XmlDocumentType docType, string name)
	{
		if (name == "PUBLIC")
		{
			return docType.PublicId;
		}
		if (name == "SYSTEM")
		{
			return docType.SystemId;
		}
		return null;
	}

	public string GetDocumentTypeAttr(int i)
	{
		if (nDocTypeAttrCount == -1)
		{
			InitDocTypeAttr();
		}
		return docTypeNodeAttributes[i].value;
	}

	public int GetDocTypeAttrInd(string name)
	{
		if (nDocTypeAttrCount == -1)
		{
			InitDocTypeAttr();
		}
		for (int i = 0; i < nDocTypeAttrCount; i++)
		{
			if (docTypeNodeAttributes[i].name == name)
			{
				return i;
			}
		}
		return -1;
	}

	private string GetAttributeFromElement(XmlElement elem, string name)
	{
		return elem.GetAttributeNode(name)?.Value;
	}

	public string GetAttribute(string name)
	{
		if (bCreatedOnAttribute)
		{
			return null;
		}
		return curNode.NodeType switch
		{
			XmlNodeType.Element => GetAttributeFromElement((XmlElement)curNode, name), 
			XmlNodeType.Attribute => GetAttributeFromElement((XmlElement)elemNode, name), 
			XmlNodeType.XmlDeclaration => GetDeclarationAttr((XmlDeclaration)curNode, name), 
			XmlNodeType.DocumentType => GetDocumentTypeAttr((XmlDocumentType)curNode, name), 
			_ => null, 
		};
	}

	private string GetAttributeFromElement(XmlElement elem, string name, string ns)
	{
		return elem.GetAttributeNode(name, ns)?.Value;
	}

	public string GetAttribute(string name, string ns)
	{
		if (bCreatedOnAttribute)
		{
			return null;
		}
		switch (curNode.NodeType)
		{
		case XmlNodeType.Element:
			return GetAttributeFromElement((XmlElement)curNode, name, ns);
		case XmlNodeType.Attribute:
			return GetAttributeFromElement((XmlElement)elemNode, name, ns);
		case XmlNodeType.XmlDeclaration:
			if (ns.Length != 0)
			{
				return null;
			}
			return GetDeclarationAttr((XmlDeclaration)curNode, name);
		case XmlNodeType.DocumentType:
			if (ns.Length != 0)
			{
				return null;
			}
			return GetDocumentTypeAttr((XmlDocumentType)curNode, name);
		default:
			return null;
		}
	}

	public string GetAttribute(int attributeIndex)
	{
		if (bCreatedOnAttribute)
		{
			return null;
		}
		switch (curNode.NodeType)
		{
		case XmlNodeType.Element:
			CheckIndexCondition(attributeIndex);
			return ((XmlElement)curNode).Attributes[attributeIndex].Value;
		case XmlNodeType.Attribute:
			CheckIndexCondition(attributeIndex);
			return ((XmlElement)elemNode).Attributes[attributeIndex].Value;
		case XmlNodeType.XmlDeclaration:
			CheckIndexCondition(attributeIndex);
			return GetDeclarationAttr(attributeIndex);
		case XmlNodeType.DocumentType:
			CheckIndexCondition(attributeIndex);
			return GetDocumentTypeAttr(attributeIndex);
		default:
			throw new ArgumentOutOfRangeException("attributeIndex");
		}
	}

	public void LogMove(int level)
	{
		logNode = curNode;
		nLogLevel = level;
		nLogAttrInd = nAttrInd;
		logAttrIndex = attrIndex;
		bLogOnAttrVal = bOnAttrVal;
	}

	public void RollBackMove(ref int level)
	{
		curNode = logNode;
		level = nLogLevel;
		nAttrInd = nLogAttrInd;
		attrIndex = logAttrIndex;
		bOnAttrVal = bLogOnAttrVal;
	}

	public void ResetToAttribute(ref int level)
	{
		if (bCreatedOnAttribute || !bOnAttrVal)
		{
			return;
		}
		if (IsOnDeclOrDocType)
		{
			level -= 2;
		}
		else
		{
			while (curNode.NodeType != XmlNodeType.Attribute && (curNode = curNode.ParentNode) != null)
			{
				level--;
			}
		}
		bOnAttrVal = false;
	}

	public void ResetMove(ref int level, ref XmlNodeType nt)
	{
		LogMove(level);
		if (bCreatedOnAttribute)
		{
			return;
		}
		if (nAttrInd != -1)
		{
			if (bOnAttrVal)
			{
				level--;
				bOnAttrVal = false;
			}
			nLogAttrInd = nAttrInd;
			level--;
			nAttrInd = -1;
			nt = curNode.NodeType;
			return;
		}
		if (bOnAttrVal && curNode.NodeType != XmlNodeType.Attribute)
		{
			ResetToAttribute(ref level);
		}
		if (curNode.NodeType == XmlNodeType.Attribute)
		{
			curNode = ((XmlAttribute)curNode).OwnerElement;
			attrIndex = -1;
			level--;
			nt = XmlNodeType.Element;
		}
		if (curNode.NodeType == XmlNodeType.Element)
		{
			elemNode = curNode;
		}
	}

	public bool MoveToAttribute(string name)
	{
		return MoveToAttribute(name, string.Empty);
	}

	private bool MoveToAttributeFromElement(XmlElement elem, string name, string ns)
	{
		XmlAttribute xmlAttribute = null;
		xmlAttribute = ((ns.Length != 0) ? elem.GetAttributeNode(name, ns) : elem.GetAttributeNode(name));
		if (xmlAttribute != null)
		{
			bOnAttrVal = false;
			elemNode = elem;
			curNode = xmlAttribute;
			attrIndex = elem.Attributes.FindNodeOffsetNS(xmlAttribute);
			if (attrIndex != -1)
			{
				return true;
			}
		}
		return false;
	}

	public bool MoveToAttribute(string name, string namespaceURI)
	{
		if (bCreatedOnAttribute)
		{
			return false;
		}
		XmlNodeType nodeType = curNode.NodeType;
		if (nodeType == XmlNodeType.Element)
		{
			return MoveToAttributeFromElement((XmlElement)curNode, name, namespaceURI);
		}
		if (nodeType == XmlNodeType.Attribute)
		{
			return MoveToAttributeFromElement((XmlElement)elemNode, name, namespaceURI);
		}
		if (nodeType == XmlNodeType.XmlDeclaration && namespaceURI.Length == 0)
		{
			if ((nAttrInd = GetDecAttrInd(name)) != -1)
			{
				bOnAttrVal = false;
				return true;
			}
		}
		else if (nodeType == XmlNodeType.DocumentType && namespaceURI.Length == 0 && (nAttrInd = GetDocTypeAttrInd(name)) != -1)
		{
			bOnAttrVal = false;
			return true;
		}
		return false;
	}

	public void MoveToAttribute(int attributeIndex)
	{
		if (bCreatedOnAttribute)
		{
			return;
		}
		XmlAttribute xmlAttribute = null;
		switch (curNode.NodeType)
		{
		case XmlNodeType.Element:
			CheckIndexCondition(attributeIndex);
			xmlAttribute = ((XmlElement)curNode).Attributes[attributeIndex];
			if (xmlAttribute != null)
			{
				elemNode = curNode;
				curNode = xmlAttribute;
				attrIndex = attributeIndex;
			}
			break;
		case XmlNodeType.Attribute:
			CheckIndexCondition(attributeIndex);
			xmlAttribute = ((XmlElement)elemNode).Attributes[attributeIndex];
			if (xmlAttribute != null)
			{
				curNode = xmlAttribute;
				attrIndex = attributeIndex;
			}
			break;
		case XmlNodeType.DocumentType:
		case XmlNodeType.XmlDeclaration:
			CheckIndexCondition(attributeIndex);
			nAttrInd = attributeIndex;
			break;
		}
	}

	public bool MoveToNextAttribute(ref int level)
	{
		if (bCreatedOnAttribute)
		{
			return false;
		}
		switch (curNode.NodeType)
		{
		case XmlNodeType.Attribute:
			if (attrIndex >= elemNode.Attributes.Count - 1)
			{
				return false;
			}
			curNode = elemNode.Attributes[++attrIndex];
			return true;
		case XmlNodeType.Element:
			if (curNode.Attributes.Count > 0)
			{
				level++;
				elemNode = curNode;
				curNode = curNode.Attributes[0];
				attrIndex = 0;
				return true;
			}
			break;
		case XmlNodeType.XmlDeclaration:
			if (nDeclarationAttrCount == -1)
			{
				InitDecAttr();
			}
			nAttrInd++;
			if (nAttrInd < nDeclarationAttrCount)
			{
				if (nAttrInd == 0)
				{
					level++;
				}
				bOnAttrVal = false;
				return true;
			}
			nAttrInd--;
			break;
		case XmlNodeType.DocumentType:
			if (nDocTypeAttrCount == -1)
			{
				InitDocTypeAttr();
			}
			nAttrInd++;
			if (nAttrInd < nDocTypeAttrCount)
			{
				if (nAttrInd == 0)
				{
					level++;
				}
				bOnAttrVal = false;
				return true;
			}
			nAttrInd--;
			break;
		}
		return false;
	}

	public bool MoveToParent()
	{
		XmlNode parentNode = curNode.ParentNode;
		if (parentNode != null)
		{
			curNode = parentNode;
			if (!bOnAttrVal)
			{
				attrIndex = 0;
			}
			return true;
		}
		return false;
	}

	public bool MoveToFirstChild()
	{
		XmlNode firstChild = curNode.FirstChild;
		if (firstChild != null)
		{
			curNode = firstChild;
			if (!bOnAttrVal)
			{
				attrIndex = -1;
			}
			return true;
		}
		return false;
	}

	private bool MoveToNextSibling(XmlNode node)
	{
		XmlNode nextSibling = node.NextSibling;
		if (nextSibling != null)
		{
			curNode = nextSibling;
			if (!bOnAttrVal)
			{
				attrIndex = -1;
			}
			return true;
		}
		return false;
	}

	public bool MoveToNext()
	{
		if (curNode.NodeType != XmlNodeType.Attribute)
		{
			return MoveToNextSibling(curNode);
		}
		return MoveToNextSibling(elemNode);
	}

	public bool MoveToElement()
	{
		if (bCreatedOnAttribute)
		{
			return false;
		}
		switch (curNode.NodeType)
		{
		case XmlNodeType.Attribute:
			if (elemNode != null)
			{
				curNode = elemNode;
				attrIndex = -1;
				return true;
			}
			break;
		case XmlNodeType.DocumentType:
		case XmlNodeType.XmlDeclaration:
			if (nAttrInd != -1)
			{
				nAttrInd = -1;
				return true;
			}
			break;
		}
		return false;
	}

	public string LookupNamespace(string prefix)
	{
		if (bCreatedOnAttribute)
		{
			return null;
		}
		if (prefix == "xmlns")
		{
			return nameTable.Add("http://www.w3.org/2000/xmlns/");
		}
		if (prefix == "xml")
		{
			return nameTable.Add("http://www.w3.org/XML/1998/namespace");
		}
		if (prefix == null)
		{
			prefix = string.Empty;
		}
		string name = ((prefix.Length != 0) ? ("xmlns:" + prefix) : "xmlns");
		XmlNode xmlNode = curNode;
		while (xmlNode != null)
		{
			if (xmlNode.NodeType == XmlNodeType.Element)
			{
				XmlElement xmlElement = (XmlElement)xmlNode;
				if (xmlElement.HasAttributes)
				{
					XmlAttribute attributeNode = xmlElement.GetAttributeNode(name);
					if (attributeNode != null)
					{
						return attributeNode.Value;
					}
				}
			}
			else if (xmlNode.NodeType == XmlNodeType.Attribute)
			{
				xmlNode = ((XmlAttribute)xmlNode).OwnerElement;
				continue;
			}
			xmlNode = xmlNode.ParentNode;
		}
		if (prefix.Length == 0)
		{
			return string.Empty;
		}
		return null;
	}

	internal string DefaultLookupNamespace(string prefix)
	{
		if (!bCreatedOnAttribute)
		{
			if (prefix == "xmlns")
			{
				return nameTable.Add("http://www.w3.org/2000/xmlns/");
			}
			if (prefix == "xml")
			{
				return nameTable.Add("http://www.w3.org/XML/1998/namespace");
			}
			if (prefix == string.Empty)
			{
				return nameTable.Add(string.Empty);
			}
		}
		return null;
	}

	internal string LookupPrefix(string namespaceName)
	{
		if (bCreatedOnAttribute || namespaceName == null)
		{
			return null;
		}
		if (namespaceName == "http://www.w3.org/2000/xmlns/")
		{
			return nameTable.Add("xmlns");
		}
		if (namespaceName == "http://www.w3.org/XML/1998/namespace")
		{
			return nameTable.Add("xml");
		}
		if (namespaceName == string.Empty)
		{
			return string.Empty;
		}
		XmlNode xmlNode = curNode;
		while (xmlNode != null)
		{
			if (xmlNode.NodeType == XmlNodeType.Element)
			{
				XmlElement xmlElement = (XmlElement)xmlNode;
				if (xmlElement.HasAttributes)
				{
					XmlAttributeCollection attributes = xmlElement.Attributes;
					for (int i = 0; i < attributes.Count; i++)
					{
						XmlAttribute xmlAttribute = attributes[i];
						if (!(xmlAttribute.Value == namespaceName))
						{
							continue;
						}
						if (xmlAttribute.Prefix.Length == 0 && xmlAttribute.LocalName == "xmlns")
						{
							if (LookupNamespace(string.Empty) == namespaceName)
							{
								return string.Empty;
							}
						}
						else if (xmlAttribute.Prefix == "xmlns")
						{
							string localName = xmlAttribute.LocalName;
							if (LookupNamespace(localName) == namespaceName)
							{
								return nameTable.Add(localName);
							}
						}
					}
				}
			}
			else if (xmlNode.NodeType == XmlNodeType.Attribute)
			{
				xmlNode = ((XmlAttribute)xmlNode).OwnerElement;
				continue;
			}
			xmlNode = xmlNode.ParentNode;
		}
		return null;
	}

	internal IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (bCreatedOnAttribute)
		{
			return dictionary;
		}
		XmlNode xmlNode = curNode;
		while (xmlNode != null)
		{
			if (xmlNode.NodeType == XmlNodeType.Element)
			{
				XmlElement xmlElement = (XmlElement)xmlNode;
				if (xmlElement.HasAttributes)
				{
					XmlAttributeCollection attributes = xmlElement.Attributes;
					for (int i = 0; i < attributes.Count; i++)
					{
						XmlAttribute xmlAttribute = attributes[i];
						if (xmlAttribute.LocalName == "xmlns" && xmlAttribute.Prefix.Length == 0)
						{
							if (!dictionary.ContainsKey(string.Empty))
							{
								dictionary.Add(nameTable.Add(string.Empty), nameTable.Add(xmlAttribute.Value));
							}
						}
						else if (xmlAttribute.Prefix == "xmlns")
						{
							string localName = xmlAttribute.LocalName;
							if (!dictionary.ContainsKey(localName))
							{
								dictionary.Add(nameTable.Add(localName), nameTable.Add(xmlAttribute.Value));
							}
						}
					}
				}
				if (scope == XmlNamespaceScope.Local)
				{
					break;
				}
			}
			else if (xmlNode.NodeType == XmlNodeType.Attribute)
			{
				xmlNode = ((XmlAttribute)xmlNode).OwnerElement;
				continue;
			}
			xmlNode = xmlNode.ParentNode;
		}
		if (scope != XmlNamespaceScope.Local)
		{
			if (dictionary.ContainsKey(string.Empty) && dictionary[string.Empty] == string.Empty)
			{
				dictionary.Remove(string.Empty);
			}
			if (scope == XmlNamespaceScope.All)
			{
				dictionary.Add(nameTable.Add("xml"), nameTable.Add("http://www.w3.org/XML/1998/namespace"));
			}
		}
		return dictionary;
	}

	public bool ReadAttributeValue(ref int level, ref bool bResolveEntity, ref XmlNodeType nt)
	{
		if (nAttrInd != -1)
		{
			if (!bOnAttrVal)
			{
				bOnAttrVal = true;
				level++;
				nt = XmlNodeType.Text;
				return true;
			}
			return false;
		}
		if (curNode.NodeType == XmlNodeType.Attribute)
		{
			XmlNode firstChild = curNode.FirstChild;
			if (firstChild != null)
			{
				curNode = firstChild;
				nt = curNode.NodeType;
				level++;
				bOnAttrVal = true;
				return true;
			}
		}
		else if (bOnAttrVal)
		{
			XmlNode xmlNode = null;
			if ((curNode.NodeType == XmlNodeType.EntityReference) & bResolveEntity)
			{
				curNode = curNode.FirstChild;
				nt = curNode.NodeType;
				level++;
				bResolveEntity = false;
				return true;
			}
			xmlNode = curNode.NextSibling;
			if (xmlNode == null)
			{
				XmlNode parentNode = curNode.ParentNode;
				if (parentNode != null && parentNode.NodeType == XmlNodeType.EntityReference)
				{
					curNode = parentNode;
					nt = XmlNodeType.EndEntity;
					level--;
					return true;
				}
			}
			if (xmlNode != null)
			{
				curNode = xmlNode;
				nt = curNode.NodeType;
				return true;
			}
			return false;
		}
		return false;
	}
}
