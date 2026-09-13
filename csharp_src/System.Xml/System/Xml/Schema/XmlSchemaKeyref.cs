using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaKeyref : XmlSchemaIdentityConstraint
{
	private XmlQualifiedName refer = XmlQualifiedName.Empty;

	[XmlAttribute("refer")]
	public XmlQualifiedName Refer
	{
		get
		{
			return refer;
		}
		set
		{
			refer = ((value == null) ? XmlQualifiedName.Empty : value);
		}
	}
}
