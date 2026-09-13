using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaGroup : XmlSchemaAnnotated
{
	private string name;

	private XmlSchemaGroupBase particle;

	private XmlSchemaParticle canonicalParticle;

	private XmlQualifiedName qname = XmlQualifiedName.Empty;

	private XmlSchemaGroup redefined;

	private int selfReferenceCount;

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

	[XmlElement("choice", typeof(XmlSchemaChoice))]
	[XmlElement("all", typeof(XmlSchemaAll))]
	[XmlElement("sequence", typeof(XmlSchemaSequence))]
	public XmlSchemaGroupBase Particle
	{
		get
		{
			return particle;
		}
		set
		{
			particle = value;
		}
	}

	[XmlIgnore]
	public XmlQualifiedName QualifiedName => qname;

	[XmlIgnore]
	internal XmlSchemaParticle CanonicalParticle
	{
		get
		{
			return canonicalParticle;
		}
		set
		{
			canonicalParticle = value;
		}
	}

	[XmlIgnore]
	internal XmlSchemaGroup Redefined
	{
		get
		{
			return redefined;
		}
		set
		{
			redefined = value;
		}
	}

	[XmlIgnore]
	internal int SelfReferenceCount
	{
		get
		{
			return selfReferenceCount;
		}
		set
		{
			selfReferenceCount = value;
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

	internal void SetQualifiedName(XmlQualifiedName value)
	{
		qname = value;
	}

	internal override XmlSchemaObject Clone()
	{
		return Clone(null);
	}

	internal XmlSchemaObject Clone(XmlSchema parentSchema)
	{
		XmlSchemaGroup xmlSchemaGroup = (XmlSchemaGroup)MemberwiseClone();
		if (XmlSchemaComplexType.HasParticleRef(particle, parentSchema))
		{
			xmlSchemaGroup.particle = XmlSchemaComplexType.CloneParticle(particle, parentSchema) as XmlSchemaGroupBase;
		}
		xmlSchemaGroup.canonicalParticle = XmlSchemaParticle.Empty;
		return xmlSchemaGroup;
	}
}
