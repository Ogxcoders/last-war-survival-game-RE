namespace System.Xml;

public class XmlEntity : XmlNode
{
	private string publicId;

	private string systemId;

	private string notationName;

	private string name;

	private string unparsedReplacementStr;

	private string baseURI;

	private XmlLinkedNode lastChild;

	private bool childrenFoliating;

	public override bool IsReadOnly => true;

	public override string Name => name;

	public override string LocalName => name;

	public override string InnerText
	{
		get
		{
			return base.InnerText;
		}
		set
		{
			throw new InvalidOperationException(Res.GetString("The 'InnerText' of an 'Entity' node is read-only and cannot be set."));
		}
	}

	internal override bool IsContainer => true;

	internal override XmlLinkedNode LastNode
	{
		get
		{
			if (lastChild == null && !childrenFoliating)
			{
				childrenFoliating = true;
				new XmlLoader().ExpandEntity(this);
			}
			return lastChild;
		}
		set
		{
			lastChild = value;
		}
	}

	public override XmlNodeType NodeType => XmlNodeType.Entity;

	public string PublicId => publicId;

	public string SystemId => systemId;

	public string NotationName => notationName;

	public override string OuterXml => string.Empty;

	public override string InnerXml
	{
		get
		{
			return string.Empty;
		}
		set
		{
			throw new InvalidOperationException(Res.GetString("Cannot set the 'InnerXml' for the current node because it is either read-only or cannot have children."));
		}
	}

	public override string BaseURI => baseURI;

	internal XmlEntity(string name, string strdata, string publicId, string systemId, string notationName, XmlDocument doc)
		: base(doc)
	{
		this.name = doc.NameTable.Add(name);
		this.publicId = publicId;
		this.systemId = systemId;
		this.notationName = notationName;
		unparsedReplacementStr = strdata;
		childrenFoliating = false;
	}

	public override XmlNode CloneNode(bool deep)
	{
		throw new InvalidOperationException(Res.GetString("'Entity' and 'Notation' nodes cannot be cloned."));
	}

	internal override bool IsValidChildType(XmlNodeType type)
	{
		if (type != XmlNodeType.Text && type != XmlNodeType.Element && type != XmlNodeType.ProcessingInstruction && type != XmlNodeType.Comment && type != XmlNodeType.CDATA && type != XmlNodeType.Whitespace && type != XmlNodeType.SignificantWhitespace)
		{
			return type == XmlNodeType.EntityReference;
		}
		return true;
	}

	public override void WriteTo(XmlWriter w)
	{
	}

	public override void WriteContentTo(XmlWriter w)
	{
	}

	internal void SetBaseURI(string inBaseURI)
	{
		baseURI = inBaseURI;
	}
}
