using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaSimpleContent : XmlSchemaContentModel
{
	private XmlSchemaContent content;

	[XmlElement("restriction", typeof(XmlSchemaSimpleContentRestriction))]
	[XmlElement("extension", typeof(XmlSchemaSimpleContentExtension))]
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
}
