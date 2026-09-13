using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml;

public class XmlElement : XmlLinkedNode
{
	private XmlName name;

	private XmlAttributeCollection attributes;

	private XmlLinkedNode lastChild;

	internal XmlName XmlName
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	public override string Name => name.Name;

	public override string LocalName => name.LocalName;

	public override string NamespaceURI => name.NamespaceURI;

	public override string Prefix
	{
		get
		{
			return name.Prefix;
		}
		set
		{
			name = name.OwnerDocument.AddXmlName(value, LocalName, NamespaceURI, SchemaInfo);
		}
	}

	public override XmlNodeType NodeType => XmlNodeType.Element;

	public override XmlNode ParentNode => parentNode;

	public override XmlDocument OwnerDocument => name.OwnerDocument;

	internal override bool IsContainer => true;

	public bool IsEmpty
	{
		get
		{
			return lastChild == this;
		}
		set
		{
			if (value)
			{
				if (lastChild != this)
				{
					RemoveAllChildren();
					lastChild = this;
				}
			}
			else if (lastChild == this)
			{
				lastChild = null;
			}
		}
	}

	internal override XmlLinkedNode LastNode
	{
		get
		{
			if (lastChild != this)
			{
				return lastChild;
			}
			return null;
		}
		set
		{
			lastChild = value;
		}
	}

	public override XmlAttributeCollection Attributes
	{
		get
		{
			if (attributes == null)
			{
				lock (OwnerDocument.objLock)
				{
					if (attributes == null)
					{
						attributes = new XmlAttributeCollection(this);
					}
				}
			}
			return attributes;
		}
	}

	public virtual bool HasAttributes
	{
		get
		{
			if (attributes == null)
			{
				return false;
			}
			return attributes.Count > 0;
		}
	}

	public override IXmlSchemaInfo SchemaInfo => name;

	public override string InnerXml
	{
		get
		{
			return base.InnerXml;
		}
		set
		{
			RemoveAllChildren();
			new XmlLoader().LoadInnerXmlElement(this, value);
		}
	}

	public override string InnerText
	{
		get
		{
			return base.InnerText;
		}
		set
		{
			XmlLinkedNode lastNode = LastNode;
			if (lastNode != null && lastNode.NodeType == XmlNodeType.Text && lastNode.next == lastNode)
			{
				lastNode.Value = value;
				return;
			}
			RemoveAllChildren();
			AppendChild(OwnerDocument.CreateTextNode(value));
		}
	}

	public override XmlNode NextSibling
	{
		get
		{
			if (parentNode != null && parentNode.LastNode != this)
			{
				return next;
			}
			return null;
		}
	}

	internal override XPathNodeType XPNodeType => XPathNodeType.Element;

	internal override string XPLocalName => LocalName;

	internal XmlElement(XmlName name, bool empty, XmlDocument doc)
		: base(doc)
	{
		parentNode = null;
		if (!doc.IsLoading)
		{
			XmlDocument.CheckName(name.Prefix);
			XmlDocument.CheckName(name.LocalName);
		}
		if (name.LocalName.Length == 0)
		{
			throw new ArgumentException(Res.GetString("The local name for elements or attributes cannot be null or an empty string."));
		}
		this.name = name;
		if (empty)
		{
			lastChild = this;
		}
	}

	protected internal XmlElement(string prefix, string localName, string namespaceURI, XmlDocument doc)
		: this(doc.AddXmlName(prefix, localName, namespaceURI, null), empty: true, doc)
	{
	}

	public override XmlNode CloneNode(bool deep)
	{
		XmlDocument ownerDocument = OwnerDocument;
		bool isLoading = ownerDocument.IsLoading;
		ownerDocument.IsLoading = true;
		XmlElement xmlElement = ownerDocument.CreateElement(Prefix, LocalName, NamespaceURI);
		ownerDocument.IsLoading = isLoading;
		if (xmlElement.IsEmpty != IsEmpty)
		{
			xmlElement.IsEmpty = IsEmpty;
		}
		if (HasAttributes)
		{
			foreach (XmlAttribute attribute in Attributes)
			{
				XmlAttribute xmlAttribute2 = (XmlAttribute)attribute.CloneNode(deep: true);
				if (attribute is XmlUnspecifiedAttribute && !attribute.Specified)
				{
					((XmlUnspecifiedAttribute)xmlAttribute2).SetSpecified(f: false);
				}
				xmlElement.Attributes.InternalAppendAttribute(xmlAttribute2);
			}
		}
		if (deep)
		{
			xmlElement.CopyChildren(ownerDocument, this, deep);
		}
		return xmlElement;
	}

	internal override XmlNode AppendChildForLoad(XmlNode newChild, XmlDocument doc)
	{
		XmlNodeChangedEventArgs insertEventArgsForLoad = doc.GetInsertEventArgsForLoad(newChild, this);
		if (insertEventArgsForLoad != null)
		{
			doc.BeforeEvent(insertEventArgsForLoad);
		}
		XmlLinkedNode xmlLinkedNode = (XmlLinkedNode)newChild;
		if (lastChild == null || lastChild == this)
		{
			xmlLinkedNode.next = xmlLinkedNode;
			lastChild = xmlLinkedNode;
			xmlLinkedNode.SetParentForLoad(this);
		}
		else
		{
			XmlLinkedNode xmlLinkedNode2 = lastChild;
			xmlLinkedNode.next = xmlLinkedNode2.next;
			xmlLinkedNode2.next = xmlLinkedNode;
			lastChild = xmlLinkedNode;
			if (xmlLinkedNode2.IsText && xmlLinkedNode.IsText)
			{
				XmlNode.NestTextNodes(xmlLinkedNode2, xmlLinkedNode);
			}
			else
			{
				xmlLinkedNode.SetParentForLoad(this);
			}
		}
		if (insertEventArgsForLoad != null)
		{
			doc.AfterEvent(insertEventArgsForLoad);
		}
		return xmlLinkedNode;
	}

	internal override bool IsValidChildType(XmlNodeType type)
	{
		switch (type)
		{
		case XmlNodeType.Element:
		case XmlNodeType.Text:
		case XmlNodeType.CDATA:
		case XmlNodeType.EntityReference:
		case XmlNodeType.ProcessingInstruction:
		case XmlNodeType.Comment:
		case XmlNodeType.Whitespace:
		case XmlNodeType.SignificantWhitespace:
			return true;
		default:
			return false;
		}
	}

	public virtual string GetAttribute(string name)
	{
		XmlAttribute attributeNode = GetAttributeNode(name);
		if (attributeNode != null)
		{
			return attributeNode.Value;
		}
		return string.Empty;
	}

	public virtual void SetAttribute(string name, string value)
	{
		XmlAttribute attributeNode = GetAttributeNode(name);
		if (attributeNode == null)
		{
			attributeNode = OwnerDocument.CreateAttribute(name);
			attributeNode.Value = value;
			Attributes.InternalAppendAttribute(attributeNode);
		}
		else
		{
			attributeNode.Value = value;
		}
	}

	public virtual void RemoveAttribute(string name)
	{
		if (HasAttributes)
		{
			Attributes.RemoveNamedItem(name);
		}
	}

	public virtual XmlAttribute GetAttributeNode(string name)
	{
		if (HasAttributes)
		{
			return Attributes[name];
		}
		return null;
	}

	public virtual XmlAttribute SetAttributeNode(XmlAttribute newAttr)
	{
		if (newAttr.OwnerElement != null)
		{
			throw new InvalidOperationException(Res.GetString("The 'Attribute' node cannot be inserted because it is already an attribute of another element."));
		}
		return (XmlAttribute)Attributes.SetNamedItem(newAttr);
	}

	public virtual XmlAttribute RemoveAttributeNode(XmlAttribute oldAttr)
	{
		if (HasAttributes)
		{
			return Attributes.Remove(oldAttr);
		}
		return null;
	}

	public virtual XmlNodeList GetElementsByTagName(string name)
	{
		return new XmlElementList(this, name);
	}

	public virtual string GetAttribute(string localName, string namespaceURI)
	{
		XmlAttribute attributeNode = GetAttributeNode(localName, namespaceURI);
		if (attributeNode != null)
		{
			return attributeNode.Value;
		}
		return string.Empty;
	}

	public virtual string SetAttribute(string localName, string namespaceURI, string value)
	{
		XmlAttribute attributeNode = GetAttributeNode(localName, namespaceURI);
		if (attributeNode == null)
		{
			attributeNode = OwnerDocument.CreateAttribute(string.Empty, localName, namespaceURI);
			attributeNode.Value = value;
			Attributes.InternalAppendAttribute(attributeNode);
		}
		else
		{
			attributeNode.Value = value;
		}
		return value;
	}

	public virtual void RemoveAttribute(string localName, string namespaceURI)
	{
		RemoveAttributeNode(localName, namespaceURI);
	}

	public virtual XmlAttribute GetAttributeNode(string localName, string namespaceURI)
	{
		if (HasAttributes)
		{
			return Attributes[localName, namespaceURI];
		}
		return null;
	}

	public virtual XmlAttribute SetAttributeNode(string localName, string namespaceURI)
	{
		XmlAttribute xmlAttribute = GetAttributeNode(localName, namespaceURI);
		if (xmlAttribute == null)
		{
			xmlAttribute = OwnerDocument.CreateAttribute(string.Empty, localName, namespaceURI);
			Attributes.InternalAppendAttribute(xmlAttribute);
		}
		return xmlAttribute;
	}

	public virtual XmlAttribute RemoveAttributeNode(string localName, string namespaceURI)
	{
		if (HasAttributes)
		{
			XmlAttribute attributeNode = GetAttributeNode(localName, namespaceURI);
			Attributes.Remove(attributeNode);
			return attributeNode;
		}
		return null;
	}

	public virtual XmlNodeList GetElementsByTagName(string localName, string namespaceURI)
	{
		return new XmlElementList(this, localName, namespaceURI);
	}

	public virtual bool HasAttribute(string name)
	{
		return GetAttributeNode(name) != null;
	}

	public virtual bool HasAttribute(string localName, string namespaceURI)
	{
		return GetAttributeNode(localName, namespaceURI) != null;
	}

	public override void WriteTo(XmlWriter w)
	{
		if (GetType() == typeof(XmlElement))
		{
			WriteElementTo(w, this);
			return;
		}
		WriteStartElement(w);
		if (IsEmpty)
		{
			w.WriteEndElement();
			return;
		}
		WriteContentTo(w);
		w.WriteFullEndElement();
	}

	private static void WriteElementTo(XmlWriter writer, XmlElement e)
	{
		XmlNode xmlNode = e;
		XmlNode xmlNode2 = e;
		while (true)
		{
			e = xmlNode2 as XmlElement;
			if (e != null && e.GetType() == typeof(XmlElement))
			{
				e.WriteStartElement(writer);
				if (e.IsEmpty)
				{
					writer.WriteEndElement();
				}
				else
				{
					if (e.lastChild != null)
					{
						xmlNode2 = e.FirstChild;
						continue;
					}
					writer.WriteFullEndElement();
				}
			}
			else
			{
				xmlNode2.WriteTo(writer);
			}
			while (xmlNode2 != xmlNode && xmlNode2 == xmlNode2.ParentNode.LastChild)
			{
				xmlNode2 = xmlNode2.ParentNode;
				writer.WriteFullEndElement();
			}
			if (xmlNode2 != xmlNode)
			{
				xmlNode2 = xmlNode2.NextSibling;
				continue;
			}
			break;
		}
	}

	private void WriteStartElement(XmlWriter w)
	{
		w.WriteStartElement(Prefix, LocalName, NamespaceURI);
		if (HasAttributes)
		{
			XmlAttributeCollection xmlAttributeCollection = Attributes;
			for (int i = 0; i < xmlAttributeCollection.Count; i++)
			{
				xmlAttributeCollection[i].WriteTo(w);
			}
		}
	}

	public override void WriteContentTo(XmlWriter w)
	{
		for (XmlNode xmlNode = FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
		{
			xmlNode.WriteTo(w);
		}
	}

	public virtual XmlNode RemoveAttributeAt(int i)
	{
		if (HasAttributes)
		{
			return attributes.RemoveAt(i);
		}
		return null;
	}

	public virtual void RemoveAllAttributes()
	{
		if (HasAttributes)
		{
			attributes.RemoveAll();
		}
	}

	public override void RemoveAll()
	{
		base.RemoveAll();
		RemoveAllAttributes();
	}

	internal void RemoveAllChildren()
	{
		base.RemoveAll();
	}

	internal override void SetParent(XmlNode node)
	{
		parentNode = node;
	}

	internal override string GetXPAttribute(string localName, string ns)
	{
		if (ns == OwnerDocument.strReservedXmlns)
		{
			return null;
		}
		XmlAttribute attributeNode = GetAttributeNode(localName, ns);
		if (attributeNode != null)
		{
			return attributeNode.Value;
		}
		return string.Empty;
	}
}
