using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaNotation : XmlSchemaAnnotated
{
	private string name;

	private string publicId;

	private string systemId;

	private XmlQualifiedName qname = XmlQualifiedName.Empty;

	[XmlAttribute("name")]
	public string Name
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

	[XmlAttribute("public")]
	public string Public
	{
		get
		{
			return publicId;
		}
		set
		{
			publicId = value;
		}
	}

	[XmlAttribute("system")]
	public string System
	{
		get
		{
			return systemId;
		}
		set
		{
			systemId = value;
		}
	}

	[XmlIgnore]
	internal XmlQualifiedName QualifiedName
	{
		get
		{
			return qname;
		}
		set
		{
			qname = value;
		}
	}

	[XmlIgnore]
	internal override string NameAttribute
	{
		get
		{
			return Name;
		}
		set
		{
			Name = value;
		}
	}
}
