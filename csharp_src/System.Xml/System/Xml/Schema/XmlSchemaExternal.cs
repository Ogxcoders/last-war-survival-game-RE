using System.Xml.Serialization;

namespace System.Xml.Schema;

public abstract class XmlSchemaExternal : XmlSchemaObject
{
	private string location;

	private Uri baseUri;

	private XmlSchema schema;

	private string id;

	private XmlAttribute[] moreAttributes;

	private Compositor compositor;

	[XmlAttribute("schemaLocation", DataType = "anyURI")]
	public string SchemaLocation
	{
		get
		{
			return location;
		}
		set
		{
			location = value;
		}
	}

	[XmlIgnore]
	public XmlSchema Schema
	{
		get
		{
			return schema;
		}
		set
		{
			schema = value;
		}
	}

	[XmlAttribute("id", DataType = "ID")]
	public string Id
	{
		get
		{
			return id;
		}
		set
		{
			id = value;
		}
	}

	[XmlAnyAttribute]
	public XmlAttribute[] UnhandledAttributes
	{
		get
		{
			return moreAttributes;
		}
		set
		{
			moreAttributes = value;
		}
	}

	[XmlIgnore]
	internal Uri BaseUri
	{
		get
		{
			return baseUri;
		}
		set
		{
			baseUri = value;
		}
	}

	[XmlIgnore]
	internal override string IdAttribute
	{
		get
		{
			return Id;
		}
		set
		{
			Id = value;
		}
	}

	internal Compositor Compositor
	{
		get
		{
			return compositor;
		}
		set
		{
			compositor = value;
		}
	}

	internal override void SetUnhandledAttributes(XmlAttribute[] moreAttributes)
	{
		this.moreAttributes = moreAttributes;
	}
}
