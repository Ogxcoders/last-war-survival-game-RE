using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaAttributeGroupRef : XmlSchemaAnnotated
{
	private XmlQualifiedName refName = XmlQualifiedName.Empty;

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
}
