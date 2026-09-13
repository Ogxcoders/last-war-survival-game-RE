namespace System.Xml;

internal class XmlUnspecifiedAttribute : XmlAttribute
{
	private bool fSpecified;

	public override bool Specified => fSpecified;

	public override string InnerText
	{
		set
		{
			base.InnerText = value;
			fSpecified = true;
		}
	}

	protected internal XmlUnspecifiedAttribute(string prefix, string localName, string namespaceURI, XmlDocument doc)
		: base(prefix, localName, namespaceURI, doc)
	{
	}

	public override XmlNode CloneNode(bool deep)
	{
		XmlDocument ownerDocument = OwnerDocument;
		XmlUnspecifiedAttribute obj = (XmlUnspecifiedAttribute)ownerDocument.CreateDefaultAttribute(Prefix, LocalName, NamespaceURI);
		obj.CopyChildren(ownerDocument, this, deep: true);
		obj.fSpecified = true;
		return obj;
	}

	public override XmlNode InsertBefore(XmlNode newChild, XmlNode refChild)
	{
		XmlNode result = base.InsertBefore(newChild, refChild);
		fSpecified = true;
		return result;
	}

	public override XmlNode InsertAfter(XmlNode newChild, XmlNode refChild)
	{
		XmlNode result = base.InsertAfter(newChild, refChild);
		fSpecified = true;
		return result;
	}

	public override XmlNode ReplaceChild(XmlNode newChild, XmlNode oldChild)
	{
		XmlNode result = base.ReplaceChild(newChild, oldChild);
		fSpecified = true;
		return result;
	}

	public override XmlNode RemoveChild(XmlNode oldChild)
	{
		XmlNode result = base.RemoveChild(oldChild);
		fSpecified = true;
		return result;
	}

	public override XmlNode AppendChild(XmlNode newChild)
	{
		XmlNode result = base.AppendChild(newChild);
		fSpecified = true;
		return result;
	}

	public override void WriteTo(XmlWriter w)
	{
		if (fSpecified)
		{
			base.WriteTo(w);
		}
	}

	internal void SetSpecified(bool f)
	{
		fSpecified = f;
	}
}
