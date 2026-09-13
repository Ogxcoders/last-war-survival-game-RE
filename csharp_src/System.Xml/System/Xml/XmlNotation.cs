namespace System.Xml;

public class XmlNotation : XmlNode
{
	private string publicId;

	private string systemId;

	private string name;

	public override string Name => name;

	public override string LocalName => name;

	public override XmlNodeType NodeType => XmlNodeType.Notation;

	public override bool IsReadOnly => true;

	public string PublicId => publicId;

	public string SystemId => systemId;

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

	internal XmlNotation(string name, string publicId, string systemId, XmlDocument doc)
		: base(doc)
	{
		this.name = doc.NameTable.Add(name);
		this.publicId = publicId;
		this.systemId = systemId;
	}

	public override XmlNode CloneNode(bool deep)
	{
		throw new InvalidOperationException(Res.GetString("'Entity' and 'Notation' nodes cannot be cloned."));
	}

	public override void WriteTo(XmlWriter w)
	{
	}

	public override void WriteContentTo(XmlWriter w)
	{
	}
}
