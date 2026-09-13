using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaAnnotation : XmlSchemaObject
{
	private string id;

	private XmlSchemaObjectCollection items = new XmlSchemaObjectCollection();

	private XmlAttribute[] moreAttributes;

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

	[XmlElement("documentation", typeof(XmlSchemaDocumentation))]
	[XmlElement("appinfo", typeof(XmlSchemaAppInfo))]
	public XmlSchemaObjectCollection Items => items;

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

	internal override void SetUnhandledAttributes(XmlAttribute[] moreAttributes)
	{
		this.moreAttributes = moreAttributes;
	}
}
