using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaGroupRef : XmlSchemaParticle
{
	private XmlQualifiedName refName = XmlQualifiedName.Empty;

	private XmlSchemaGroupBase particle;

	private XmlSchemaGroup refined;

	[XmlAttribute("ref")]
	public XmlQualifiedName RefName
	{
		get
		{
			return refName;
		}
		set
		{
			refName = ((value == null) ? XmlQualifiedName.Empty : value);
		}
	}

	[XmlIgnore]
	public XmlSchemaGroupBase Particle => particle;

	[XmlIgnore]
	internal XmlSchemaGroup Redefined
	{
		get
		{
			return refined;
		}
		set
		{
			refined = value;
		}
	}

	internal void SetParticle(XmlSchemaGroupBase value)
	{
		particle = value;
	}
}
