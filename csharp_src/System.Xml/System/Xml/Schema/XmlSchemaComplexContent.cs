using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaComplexContent : XmlSchemaContentModel
{
	private XmlSchemaContent content;

	private bool isMixed;

	private bool hasMixedAttribute;

	[XmlAttribute("mixed")]
	public bool IsMixed
	{
		get
		{
			return isMixed;
		}
		set
		{
			isMixed = value;
			hasMixedAttribute = true;
		}
	}

	[XmlElement("restriction", typeof(XmlSchemaComplexContentRestriction))]
	[XmlElement("extension", typeof(XmlSchemaComplexContentExtension))]
	public override XmlSchemaContent Content
	{
		get
		{
			return content;
		}
		set
		{
			content = value;
		}
	}

	[XmlIgnore]
	internal bool HasMixedAttribute => hasMixedAttribute;
}
