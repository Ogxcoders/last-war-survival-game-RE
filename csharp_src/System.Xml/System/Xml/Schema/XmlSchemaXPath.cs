using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaXPath : XmlSchemaAnnotated
{
	private string xpath;

	[XmlAttribute("xpath")]
	[DefaultValue("")]
	public string XPath
	{
		get
		{
			return xpath;
		}
		set
		{
			xpath = value;
		}
	}
}
