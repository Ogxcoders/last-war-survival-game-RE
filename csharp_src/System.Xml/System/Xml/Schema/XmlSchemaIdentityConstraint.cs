using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaIdentityConstraint : XmlSchemaAnnotated
{
	private string name;

	private XmlSchemaXPath selector;

	private XmlSchemaObjectCollection fields = new XmlSchemaObjectCollection();

	private XmlQualifiedName qualifiedName = XmlQualifiedName.Empty;

	private CompiledIdentityConstraint compiledConstraint;

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

	[XmlElement("selector", typeof(XmlSchemaXPath))]
	public XmlSchemaXPath Selector
	{
		get
		{
			return selector;
		}
		set
		{
			selector = value;
		}
	}

	[XmlElement("field", typeof(XmlSchemaXPath))]
	public XmlSchemaObjectCollection Fields => fields;

	[XmlIgnore]
	public XmlQualifiedName QualifiedName => qualifiedName;

	[XmlIgnore]
	internal CompiledIdentityConstraint CompiledConstraint
	{
		get
		{
			return compiledConstraint;
		}
		set
		{
			compiledConstraint = value;
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
		qualifiedName = value;
	}
}
