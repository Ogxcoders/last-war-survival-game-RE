using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml;

public class XmlAttribute : XmlNode
{
	private XmlName name;

	private XmlLinkedNode lastChild;

	internal int LocalNameHash => name.HashCode;

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

	public override XmlNode ParentNode => null;

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
			name = name.OwnerDocument.AddAttrXmlName(value, LocalName, NamespaceURI, SchemaInfo);
		}
	}

	public override XmlNodeType NodeType => XmlNodeType.Attribute;

	public override XmlDocument OwnerDocument => name.OwnerDocument;

	public override string Value
	{
		get
		{
			return InnerText;
		}
		set
		{
			InnerText = value;
		}
	}

	public override IXmlSchemaInfo SchemaInfo => name;

	public override string InnerText
	{
		set
		{
			if (PrepareOwnerElementInElementIdAttrMap())
			{
				string innerText = base.InnerText;
				base.InnerText = value;
				ResetOwnerElementInElementIdAttrMap(innerText);
			}
			else
			{
				base.InnerText = value;
			}
		}
	}

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

	public virtual bool Specified => true;

	public virtual XmlElement OwnerElement => parentNode as XmlElement;

	public override string InnerXml
	{
		set
		{
			RemoveAll();
			new XmlLoader().LoadInnerXmlAttribute(this, value);
		}
	}

	public override string BaseURI
	{
		get
		{
			if (OwnerElement != null)
			{
				return OwnerElement.BaseURI;
			}
			return string.Empty;
		}
	}

	internal override XmlSpace XmlSpace
	{
		get
		{
			if (OwnerElement != null)
			{
				return OwnerElement.XmlSpace;
			}
			return XmlSpace.None;
		}
	}

	internal override string XmlLang
	{
		get
		{
			if (OwnerElement != null)
			{
				return OwnerElement.XmlLang;
			}
			return string.Empty;
		}
	}

	internal override XPathNodeType XPNodeType
	{
		get
		{
			if (IsNamespace)
			{
				return XPathNodeType.Namespace;
			}
			return XPathNodeType.Attribute;
		}
	}

	internal override string XPLocalName
	{
		get
		{
			if (name.Prefix.Length == 0 && name.LocalName == "xmlns")
			{
				return string.Empty;
			}
			return name.LocalName;
		}
	}

	internal bool IsNamespace => Ref.Equal(name.NamespaceURI, name.OwnerDocument.strReservedXmlns);

	internal XmlAttribute(XmlName name, XmlDocument doc)
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
			throw new ArgumentException(Res.GetString("The attribute local name cannot be empty."));
		}
		this.name = name;
	}

	protected internal XmlAttribute(string prefix, string localName, string namespaceURI, XmlDocument doc)
		: this(doc.AddAttrXmlName(prefix, localName, namespaceURI, null), doc)
	{
	}

	public override XmlNode CloneNode(bool deep)
	{
		XmlDocument ownerDocument = OwnerDocument;
		XmlAttribute xmlAttribute = ownerDocument.CreateAttribute(Prefix, LocalName, NamespaceURI);
		xmlAttribute.CopyChildren(ownerDocument, this, deep: true);
		return xmlAttribute;
	}

	internal bool PrepareOwnerElementInElementIdAttrMap()
	{
		if (OwnerDocument.DtdSchemaInfo != null)
		{
			XmlElement ownerElement = OwnerElement;
			if (ownerElement != null)
			{
				return ownerElement.Attributes.PrepareParentInElementIdAttrMap(Prefix, LocalName);
			}
		}
		return false;
	}

	internal void ResetOwnerElementInElementIdAttrMap(string oldInnerText)
	{
		OwnerElement?.Attributes.ResetParentInElementIdAttrMap(oldInnerText, InnerText);
	}

	internal override XmlNode AppendChildForLoad(XmlNode newChild, XmlDocument doc)
	{
		XmlNodeChangedEventArgs insertEventArgsForLoad = doc.GetInsertEventArgsForLoad(newChild, this);
		if (insertEventArgsForLoad != null)
		{
			doc.BeforeEvent(insertEventArgsForLoad);
		}
		XmlLinkedNode xmlLinkedNode = (XmlLinkedNode)newChild;
		if (lastChild == null)
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
		if (type != XmlNodeType.Text)
		{
			return type == XmlNodeType.EntityReference;
		}
		return true;
	}

	public override XmlNode InsertBefore(XmlNode newChild, XmlNode refChild)
	{
		XmlNode result;
		if (PrepareOwnerElementInElementIdAttrMap())
		{
			string innerText = InnerText;
			result = base.InsertBefore(newChild, refChild);
			ResetOwnerElementInElementIdAttrMap(innerText);
		}
		else
		{
			result = base.InsertBefore(newChild, refChild);
		}
		return result;
	}

	public override XmlNode InsertAfter(XmlNode newChild, XmlNode refChild)
	{
		XmlNode result;
		if (PrepareOwnerElementInElementIdAttrMap())
		{
			string innerText = InnerText;
			result = base.InsertAfter(newChild, refChild);
			ResetOwnerElementInElementIdAttrMap(innerText);
		}
		else
		{
			result = base.InsertAfter(newChild, refChild);
		}
		return result;
	}

	public override XmlNode ReplaceChild(XmlNode newChild, XmlNode oldChild)
	{
		XmlNode result;
		if (PrepareOwnerElementInElementIdAttrMap())
		{
			string innerText = InnerText;
			result = base.ReplaceChild(newChild, oldChild);
			ResetOwnerElementInElementIdAttrMap(innerText);
		}
		else
		{
			result = base.ReplaceChild(newChild, oldChild);
		}
		return result;
	}

	public override XmlNode RemoveChild(XmlNode oldChild)
	{
		XmlNode result;
		if (PrepareOwnerElementInElementIdAttrMap())
		{
			string innerText = InnerText;
			result = base.RemoveChild(oldChild);
			ResetOwnerElementInElementIdAttrMap(innerText);
		}
		else
		{
			result = base.RemoveChild(oldChild);
		}
		return result;
	}

	public override XmlNode PrependChild(XmlNode newChild)
	{
		XmlNode result;
		if (PrepareOwnerElementInElementIdAttrMap())
		{
			string innerText = InnerText;
			result = base.PrependChild(newChild);
			ResetOwnerElementInElementIdAttrMap(innerText);
		}
		else
		{
			result = base.PrependChild(newChild);
		}
		return result;
	}

	public override XmlNode AppendChild(XmlNode newChild)
	{
		XmlNode result;
		if (PrepareOwnerElementInElementIdAttrMap())
		{
			string innerText = InnerText;
			result = base.AppendChild(newChild);
			ResetOwnerElementInElementIdAttrMap(innerText);
		}
		else
		{
			result = base.AppendChild(newChild);
		}
		return result;
	}

	public override void WriteTo(XmlWriter w)
	{
		w.WriteStartAttribute(Prefix, LocalName, NamespaceURI);
		WriteContentTo(w);
		w.WriteEndAttribute();
	}

	public override void WriteContentTo(XmlWriter w)
	{
		for (XmlNode xmlNode = FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
		{
			xmlNode.WriteTo(w);
		}
	}

	internal override void SetParent(XmlNode node)
	{
		parentNode = node;
	}
}
